using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace IRSClassLib
{
    [Serializable]
    /* Author: Carlos H. Raymundo   */
    /* (c) 2008                     */
    public class csFtp
    {
        private String host;
        private String userid;
        private String passwd;
        private IPAddress ip;
        private int port = 21;

        private TcpClient ftp;
        private TcpClient dataport;
        private Stream ns;

        private String message;
        private bool bin;
        public int delay = 5;

        public String Message
        {
            get { return message; }
        }

        public String Host
        {
            get { return host; }
            set { host = value; }
        }

        public String User
        {
            get { return userid; }
            set { userid = value; }
        }

        public String Password
        {
            get { return passwd; }
            set { passwd = value; }
        }

        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        public void disconnect()
        {
            ftp_send("quit");
            ftp.Close();
        }

        public IPAddress toIPAddress()
        {
            try
            {
                return IPAddress.Parse(host);
            }
            catch { }
            return null;
        }

        private String getLine()
        {
            StringBuilder sb = new StringBuilder();
            byte[] b = new byte[1];
            bool cr = false;

            while (true)
            {
                int i = ns.Read(b, 0, 1);
                if (i < 1) break;
                if (cr & b[0] == 10) //LF
                {
                    break;
                }
                if (b[0] == 13)		// CR
                {
                    cr = true;
                }
                else
                {
                    sb.Append(Convert.ToChar(b[0]));
                    if (cr) cr = false;
                }
            }
            // System.Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        private String ftp_send(String s)
        {
            // System.Console.WriteLine(s);
            byte[] buff = Encoding.Default.GetBytes(s + "\r\n");
            try
            {
                ns.Write(buff, 0, buff.Length);
                return getLine();
            }
            catch (Exception e)
            {
                message = "Send Failed: " + e.Message;
            }
            return null;
        }

        private bool logon()
        {
            String s = ftp_send("user " + userid);
            if (s == null) return false;

            if (s.ToLower().IndexOf("password") > -1)
            {
                s = ftp_send("pass " + passwd);
                if (s == null) return false;
                bool is_login = false;
                while (s.Substring(0, 3).Equals("230"))
                {
                    /* FOR CBIP */
                    if (s.ToLower().IndexOf("logged in.") > -1 || s.ToLower().IndexOf("login success") > -1)
                    {
                        is_login = true;
                        break;
                    }
                    s = getLine();
                }
                if (is_login) return true;

                /* Not sufficient to trap welcome messages 
                   replaced by while loop codes  above.
                if (s.Substring(0,3).Equals("230"))
                {
                    return true;
                }
                * */

                message = s;
            }

            return false;
        }

        public bool connect()
        {
            try
            {
                System.Console.WriteLine("csFtp (c) 2008 PNB - Carlos Raymundo");

                ftp = new TcpClient();

                ip = toIPAddress();

                if (ip != null) ftp.Connect(ip, port);
                else ftp.Connect(host, port);

                ns = ftp.GetStream();
                String s = getLine();
                if (s.Substring(0, 3).Equals("220"))
                {
                    while (s.Substring(0, 4).Equals("220-"))
                    {
                        s = getLine();
                    }
                    // Commented: replaced by codes above to handle multiline
                    // banner.
                    // if (s.Substring(0, 4).Equals("220-")) s = getLine();
                    if (logon())
                    {
                        return true;
                    }
                }
                else
                {
                    message = s;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return false;
        }

        public bool binary(bool on)
        {
            this.bin = on;
            String s = ftp_send("TYPE " + ((on) ? "I" : "A"));
            if (s.Substring(0, 3).Equals("200"))
            {
                return true;
            }
            message = s;
            return false;
        }

        public String pwd() // print current working directory function
        {
            String s = ftp_send("pwd");
            if (s != null && s.Substring(0, 3).Equals("257"))
            {
                String wd = s.Substring(s.IndexOf("\"") + 1, s.LastIndexOf("\"") - 5);
                return wd;
            }

            message = s;
            return null;
        }

        public bool cwd(String path)
        {
            String s = ftp_send("CWD " + path);

            if (s.IndexOf("success") > -1) return true;
            message = s;
            return false;
        }

        public bool mkdir(String path)
        {
            String s = ftp_send("mkd " + path);

            if (s.IndexOf("created.") > -1) return true;
            message = s;
            return false;
        }

        private bool getDataPort()
        {
            String s = ftp_send("PASV");

            // "227 Entering Passive Mode (172,22,25,7,5,76).\r\n"
            if (s == null) return false;

            if (s.Substring(0, 3).Equals("227"))
            {
                String[] numbers = new Regex("(\\()(.*)(\\))").Match(s).Groups[2].Value.Split(',');
                int dport = int.Parse(numbers[4]) * 256 + int.Parse(numbers[5]);

                dataport = new TcpClient();

                if (ip != null)
                    dataport.Connect(ip, dport);
                else
                    dataport.Connect(host, dport);
                return true;
            }
            return false;
        }

        private ArrayList getListFromPort()
        {
            ArrayList al = new ArrayList();
            TextReader rdr = (TextReader)new StreamReader(dataport.GetStream());
            while (true)
            {
                String s = rdr.ReadLine();
                if (s == null) break;
                al.Add(s);
            }
            dataport.Close();
            dataport = null;
            String t = getLine();

            return al;
        }

        private String[] getFileListing(bool pasv_mode)
        {
            ArrayList al = null;

            if (pasv_mode) al = getListFromPort();
            else
            {
                al = new ArrayList();
                while (true)
                {
                    String s = getLine();
                    if (s.IndexOf("Transfer Complete") > -1) break;
                    if (s.Substring(0, 4).Equals("200 ")) continue;
                    if (s.Substring(0, 4).Equals("150 ")) continue;
                    al.Add(s);
                }
            }
            String[] ss = new String[al.Count];
            al.CopyTo(ss);
            return ss;
        }

        #region commented to implement nlst
        /*
        public String[] ls(String wild_string)
        {
            bool pasv_mode = getDataPort();
            String lscmd = (wild_string == null) ? "LIST" : "LIST " + wild_string;
            String s = ftp_send(lscmd);
            if ((s.Substring(0, 3).Equals("150")) | (s.IndexOf("starting") > -1))
            {
                String[] ss = getFileListing(pasv_mode);
                return ss;
            }

            message = s;
            return null;
        }

        public String[] ls()
        {
            return ls(null);
        }
        */
        #endregion

        private String[] list(String wild_string, bool ls_short)
        {
            String lstype = (ls_short) ? "NLST" : "LIST";
            bool pasv_mode = getDataPort();
            String lscmd = (wild_string == null) ? lstype : lstype + " " + wild_string;
            String s = ftp_send(lscmd);
            if ((s.Substring(0, 3).Equals("150")) | (s.IndexOf("starting") > -1))
            {
                String[] ss = getFileListing(pasv_mode);
                return ss;
            }

            message = s;
            return null;
        }

        public String[] ls()
        {
            return list(null, false);
        }

        public String[] ls(String fpattern)
        {
            return list(fpattern, false);
        }

        public String[] nls()
        {
            return list(null, true);
        }

        public String[] nls(String fpattern)
        {
            return list(fpattern, true);
        }

        private bool begin_upload(String infile, Stream ds)
        {
            try
            {
                int nb = 4096;
                byte[] buff = new byte[nb];
                FileStream ls = new FileStream(infile, FileMode.Open, FileAccess.Read, FileShare.Read);

                while (true)
                {
                    int br = ls.Read(buff, 0, nb);
                    if (br < 1) break;

                    ds.Write(buff, 0, br);
                }
                ls.Close();
                ds.Close();
                return true;
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return false;
        }

        public bool put(String path, String newpath)
        {
            String infile = System.IO.Path.GetFileName(newpath);
            if (infile == null || infile == String.Empty) return false;

            bool ok = false;
            getDataPort();
            String s = ftp_send("stor " + infile);
            if (s.Substring(0, 3).Equals("550"))
            {
                message = s;
                ok = false;
            }
            else
            {
                ok = begin_upload(path, dataport.GetStream());
                s = getLine();
                if (s.IndexOf("Transfer complete") == -1)
                {
                    message = s;
                }
            }
            dataport.Close();
            dataport = null;

            return ok;
        }

        public bool put(String path)
        {
            return put(path, path);
        }

        private byte[] unix2dos(byte[] buff, int br)
        {
            // scans for x0a without 0x0d replace it with \r\n
            MemoryStream ms = new MemoryStream();
            ms.Position = 0;

            byte bcr = 13;
            byte blf = 10;
            bool cr = false;

            for (int i = 0; i < br; i++)
            {
                if (buff[i] == bcr)		// CR
                {
                    cr = true;
                }
                else if (buff[i] == blf) //LF
                {
                    if (!cr)
                    {
                        ms.WriteByte(bcr);
                    }
                    else
                    {
                        cr = false;
                    }
                }

                ms.WriteByte(buff[i]);
            }
            ms.Position = 0;
            return ms.ToArray();
        }

        public long begin_download(String outpath, Stream ds)
        {
            try
            {
                long rc = 0;
                int nb = 4096;
                byte[] buff = new byte[nb];
                FileStream ls = new FileStream(outpath, FileMode.Create);

                while (true)
                {
                    int br = ds.Read(buff, 0, nb);
                    if (br < 1) break;

                    byte[] xb = (this.bin) ? buff : unix2dos(buff, br);

                    ls.Write(xb, 0, xb.Length);
                    rc += xb.Length;
                    if (this.bin) { System.Threading.Thread.Sleep(delay); }
                }
                ls.Close();
                ds.Close();
                return rc;
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return -1;
        }

        public long get(String filename, String outpath)
        {
            String infile = System.IO.Path.GetFileName(filename);
            if (infile == null || infile == String.Empty) return -1;

            long rc = -1;
            getDataPort();
            String s = ftp_send("retr " + infile);

            if (s.Substring(0, 3).Equals("550"))
            {
                message = s;
            }
            else
            {
                rc = begin_download(outpath, dataport.GetStream());
                s = getLine();
                if (s.IndexOf("Transfer complete") == -1)
                {
                    message = s;
                }
            }
            dataport.Close();
            dataport = null;

            return rc;
        }

        public bool delete(String filename)
        {
            String s = ftp_send("DELE " + filename);

            if (s.IndexOf("success") > -1) return true;
            message = s;
            return false;
        }


        public String systype()
        {
            if (ftp != null)
            {
                String s = ftp_send("SYST");
                if (s == null || s.Length < 1) return "";
                // 215 Windows_NT
                return s.Substring(4);
            }
            return null;
        }

        public csFtp(String host, String userid, String passwd)
        {
            Host = host;
            User = userid;
            Password = passwd;
            port = 21;
        }

        public csFtp(String host, int port, String userid, String passwd)
        {
            Host = host;
            User = userid;
            Password = passwd;
            Port = port;
        }

        public csFtp(String connstring)
        {
            String[] ss = connstring.Split(';');
            foreach (String s in ss)
            {
                String[] tt = s.Split('=');
                if (tt[0].Trim().ToLower().Equals("host") 
                || tt[0].Trim().ToLower().Equals("server"))
                {
                    Host = tt[1].Trim();
                    continue;
                }
                if (tt[0].Trim().ToLower().Equals("user") 
                || tt[0].Trim().ToLower().Equals("uid")
                || tt[0].Trim().ToLower().Equals("userid"))
                {
                    User = tt[1].Trim();
                    continue;
                }
                if (tt[0].Trim().ToLower().Equals("password") 
                || tt[0].Trim().ToLower().Equals("pwd"))
                {
                    Password = tt[1].Trim();
                    continue;
                }
                if (tt[0].Trim().ToLower().Equals("port"))
                {
                    try
                    {
                        Port = int.Parse(tt[1].Trim());
                    }
                    catch
                    {
                        Port = 21;
                    }
                    continue;
                }
            }
        }

        public csFtp()
        { }

        ~csFtp()
        {
            host = null;
            userid = null;
            passwd = null;
            ftp = null;
            dataport = null;
            ns = null;
            ip = null;
        }

    }

}
