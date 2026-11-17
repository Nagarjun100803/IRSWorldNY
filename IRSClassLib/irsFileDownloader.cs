using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Sql;
using System.Collections;
using System.Data;
using System.Net;
using System.IO;
namespace IRSClassLib
{
    [Serializable]
    public class irsFileDownloader : sqlHelperWs
    {
        private string _temp_dir = @"C:\temp\Downloaded Files\";
        private string _file_dir;
        private string _message = "";

        private string _file_name;
        private string _file_type_desc;
        private string _file_format_desc;
        private string _loading_path;
        private string _ext;

        private int _file_type;
        private int _file_format;
        private int _record_count;

        private double _hash_total;

        private csFtp ftp = null;


        String Host = "";
        String UserName = "";
        String Password = "";
        int Port = 21;


        public string FileDirectory
        {
            get
            {
                return this._file_dir;
            }
        }

        public string FileName
        {
            get
            {
                return this._file_name + "." + this._ext;
            }
        }

        public string ErrMessage
        {
            get
            {
                return this._message;
            }
        }

        private bool getFileInventrory()
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getInventoryByFilename", new string[] { this._file_name });
                if (dt != null && dt.Rows.Count > 0)
                {
                    this._file_type = Convert.ToInt32(dt.Rows[0]["FileType"]);
                    this._file_type_desc = dt.Rows[0]["FileTypeDesc"].ToString();
                    this._file_format = Convert.ToInt32(dt.Rows[0]["FileFormat"]);
                    this._file_format_desc = dt.Rows[0]["FileFormatDesc"].ToString();
                    this._loading_path = dt.Rows[0]["Destination"].ToString();
                    this._record_count = Convert.ToInt32(dt.Rows[0]["RecordCount"]);
                    this._hash_total = Convert.ToDouble(dt.Rows[0]["HashTotal"]);
                    return true;
                }
            }
            catch
            {
                writeAudit(Message, csAuditLog.Category.Select);
            }
            finally
            {
                dt = null;
            }
            return false;
        }

        private string readFile(string path)
        {
            csFileIo io = new csFileIo();

            try
            {
                if (io.open_input(path, (int)csFileIo.access_modes.BINARY))
                {
                    return io.readAllText(path);
                }
            }
            catch
            { }
            finally
            {
                io.close_input();
                io = null;
            }

            writeAudit(io.Message + ": " + path, csAuditLog.Category.File);
            return "";
        }

        private bool getFileFTP()
        {
            string[] cons = new string[] { new irsConfiguration().FTPConnectionString, new irsConfiguration().appsetting("FTPSource1"), new irsConfiguration().appsetting("FTPSource2") };
            for (int i = 0; i < cons.Length; i++)
            {
                ftp = new csFtp(cons[i]);
                string root_path = new irsConfiguration().appsetting("RootPath");
                string ftp_path = root_path + this._file_type_desc + @"\" + this._file_format_desc + @"\" + "Archive";
                string ftp_temp = @"\temp\";

                csFileIo io = new csFileIo();
                try
                {
                    if (ftp.connect())
                    {
                        if (ftp.binary(true))
                        {
                            ftp.cwd(ftp_temp);
                            string[] ls = this.listFiles();
                            if (ls != null && ls.Length > 0)
                            {
                                for (int ndx = 0; ndx < ls.Length; ndx++)
                                {
                                    if (ls[ndx].ToUpper().Contains(this._file_name.ToUpper()) & ls[ndx].ToUpper().Contains(this._ext.ToUpper()))
                                    {
                                        string dl_path = this._temp_dir + @"\" + FileName;
                                        this._file_dir = dl_path;

                                        if (!io.directoryExist(this._temp_dir))
                                        {
                                            io.createDirectory(this._temp_dir);
                                        }
                                        if (io.exist(dl_path))
                                        {
                                            io.delete(dl_path);
                                        }

                                        if (ftp.get(FileName, dl_path) > -1)
                                        {
                                            return true;// readFile(dl_path);
                                        }
                                        else
                                        {
                                            writeAudit("FTP " + ftp.Host + " - " + ftp.Message + ": " + ftp_temp, csAuditLog.Category.FTP);
                                        }
                                    }
                                }
                            }



                            ftp.cwd(@"\");
                            ftp.cwd(ftp_path);
                            ls = this.listFiles();

                            if (ls != null && ls.Length > 0)
                            {
                                for (int ndx = 0; ndx < ls.Length; ndx++)
                                {
                                    if (ls[ndx].ToUpper().Contains(this._file_name.ToUpper()) & ls[ndx].ToUpper().Contains(this._ext.ToUpper()))
                                    {
                                        string dl_path = this._temp_dir + @"\" + FileName;
                                        this._file_dir = dl_path;
                                        if (ftp.get(FileName, dl_path) > -1)
                                        {
                                            return true;//readFile(dl_path);
                                        }
                                        else
                                        {
                                            writeAudit("FTP " + ftp.Host + " - " + ftp.Message + ": " + ftp_path, csAuditLog.Category.FTP);
                                        }
                                    }
                                }
                            }


                            if (this.FileName.ToUpper().Contains(".ERR"))
                            {
                                this._message = "The file " + new csFileIo().getFilenameNoExt(this._file_name) + " does not contain any error on the record.";
                            }
                            else this._message = "Invalid request. Please approve the file first or if the file is already approved, please wait for the confirmation email then try again.";
                        }
                    }
                    else
                    {
                        this._message = "Unable to connect to files server. Please contact system administrator. " + "<" + cons[i] + ">";
                        writeAudit(this._message + ": " + FileName, csAuditLog.Category.FTP);
                    }
                }
                catch (Exception e)
                {
                    this._message = e.Message;
                }
                finally
                {
                    ftp.disconnect();
                    ftp = null;
                    io = null;
                }

                writeAudit(this._message + ": " + ftp_path + " - " + FileName, csAuditLog.Category.FTP);
            }
            return false;
        }


        private bool getFileFTP_3()
        {
            string[] cons = new string[] { new irsConfiguration().FTPConnectionString, new irsConfiguration().appsetting("FTPSource1"), new irsConfiguration().appsetting("FTPSource2") };



            for (int i = 0; i < cons.Length; i++)
            {
                //ftp = new csFtp();
                string root_path = new irsConfiguration().appsetting("RootPath");
                string ftp_path = root_path + this._file_type_desc + @"\" + this._file_format_desc + @"\" + "Archive";
                string ftp_temp = @"\temp\";

                string dl_path = this._temp_dir + @"\" + FileName;
                this._file_dir = dl_path;


                //this.csFtp(cons[i]);

                try
                {

                    if (this.check_file(FileName, ftp_temp, cons[i]))
                    {
                        if (!download_file(FileName, ftp_temp, dl_path, cons[i]))
                        {
                            throw new Exception("CANNOT DOWNLOAD THE FILE : " + FileName + " : " + ftp_temp);
                        }
                        return true;
                    }

                    if (this.check_file(FileName, ftp_path, cons[i]))
                    {
                        if (!download_file(FileName, ftp_path, dl_path, cons[i]))
                        {
                            throw new Exception("CANNOT DOWNLOAD THE FILE : " + FileName + " : " + ftp_path);
                        }
                        return true;
                    }

                    if (this.FileName.ToUpper().Contains(".ERR"))
                    {
                        this._message = "The file " + new csFileIo().getFilenameNoExt(this._file_name) + " does not contain any error on the record.";
                    }
                    else this._message = "Invalid request. Please approve the file first or if the file is already approved, please wait for the confirmation email then try again.";

                    return false;
                }
                catch (Exception ex)
                {


                    if (this.FileName.ToUpper().Contains(".ERR"))
                    {
                        this._message = "The file " + new csFileIo().getFilenameNoExt(this._file_name) + " does not contain any error on the record.";
                    }
                    else this._message = "Invalid request. Please approve the file first or if the file is already approved, please wait for the confirmation email then try again.";

                    writeAudit(ex.Message + ":" + ex.StackTrace, csAuditLog.Category.FTP);

                    return false;
                }


                writeAudit(this._message + ": " + ftp_path + " - " + FileName, csAuditLog.Category.FTP);
            }
            return false;
        }

        public bool download_file(string sfile, string ftp_temp, string dl_path, string ftpconstr)
        {

            this.csFtp(ftpconstr);

            FileStream outputStream = null;
            FtpWebRequest reqFTP = null;
            FtpWebResponse response = null;
            Stream ftpStream = null;
            try
            {
                outputStream = new FileStream(dl_path, FileMode.Create);

                /* FOR CBIP */
                if (this.Port != 21)
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" +
                                        this.Host + ":" + this.Port + "/" + ftp_temp + "/" + sfile));
                }
                else
                {
                    reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" +
                                        this.Host + "/" + ftp_temp + "/" + sfile));
                }
                reqFTP.Proxy = null;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(this.UserName,
                                                            this.Password);
                response = (FtpWebResponse)reqFTP.GetResponse();
                ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[2048];

                readCount = ftpStream.Read(buffer, 0, bufferSize);

                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();

                return true;
            }
            catch (Exception e)
            {
                this._message = e.Message + ":" + e.StackTrace;
                writeAudit(this._message + ": " + ftp_temp + " - " + FileName + ":" + dl_path, csAuditLog.Category.FTP);
                return false;
            }
            finally
            {
                ftpStream.Close();
                ftpStream = null;
                response.Close();
                response = null;
                reqFTP = null;
                outputStream.Dispose();
                outputStream = null;

            }

        }
        public bool check_file(string filename, string sdir, string ftpconstr)
        {
            this.csFtp(ftpconstr);

            FtpWebRequest request = null;
            FtpWebResponse response = null;
            Stream responseStream = null;
            StreamReader reader = null;
            string sconts = "";
            try
            {
                // Get the object used to communicate with the server.
                /* FOR CBIP */
                if (this.Port != 21)
                {
                    request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + this.Host + ":" + this.Port + @"/" + sdir));
                }
                else
                {
                    request = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + this.Host + @"/" + sdir));
                }
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                // This example assumes the FTP site uses anonymous logon.
                request.Proxy = null;
                request.Credentials = new NetworkCredential(this.UserName, this.Password);

                response = (FtpWebResponse)request.GetResponse();

                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                sconts = reader.ReadToEnd();

                if (sconts.Contains(filename)) return true;
                return false;

            }
            catch (Exception e)
            {
                this._message = e.Message + ":" + e.StackTrace;
                writeAudit(this._message + ": " + sdir + " - " + FileName, csAuditLog.Category.FTP);
                return false;
            }
            finally
            {
                reader.Dispose();
                reader = null;
                responseStream.Dispose();
                responseStream = null;
                response.Close();

                request = null;
            }

        }
        private void csFtp(String connstring)
        {
            String[] ss = connstring.Split(';');
            foreach (String s in ss)
            {
                String[] tt = s.Split('=');
                if (tt[0].Trim().ToLower().Equals("host")
                || tt[0].Trim().ToLower().Equals("server"))
                {
                    this.Host = tt[1].Trim();
                    continue;
                }
                if (tt[0].Trim().ToLower().Equals("user")
                || tt[0].Trim().ToLower().Equals("uid")
                || tt[0].Trim().ToLower().Equals("userid"))
                {
                    this.UserName = tt[1].Trim();
                    continue;
                }
                if (tt[0].Trim().ToLower().Equals("password")
                || tt[0].Trim().ToLower().Equals("pwd"))
                {
                    this.Password = tt[1].Trim();
                    continue;
                }
                if (tt[0].Trim().ToLower().Equals("port"))
                {
                    try
                    {
                        this.Port = int.Parse(tt[1].Trim());
                    }
                    catch
                    {
                        this.Port = 21;
                    }
                    continue;
                }
            }
        }


        private String[] listFiles(String pattern)
        {
            if (ftp != null)
            {
                try
                {
                    String[] ff = ftp.nls(pattern);

                    if (ff != null && ff.Length > 0)
                    {
                        return ff;
                    }
                }
                catch (Exception e)
                {
                    writeAudit(pattern + ": " + e.Message, csAuditLog.Category.FTP);
                }
            }
            return null;
        }

        private String[] listFiles()
        {
            if (ftp != null)
            {
                try
                {
                    String[] ff = ftp.ls();

                    if (ff != null && ff.Length > 0)
                    {
                        return ff;
                    }
                }
                catch (Exception e)
                {
                    writeAudit(e.Message, csAuditLog.Category.FTP);
                    return null;
                }
            }
            writeAudit(ftp.Message, csAuditLog.Category.FTP);
            return null;
        }

        private string getTemp()
        {
            string[] cons = new string[] { new irsConfiguration().FTPConnectionString, new irsConfiguration().appsetting("FTPSource1"), new irsConfiguration().appsetting("FTPSource2") };
            for (int i = 0; i < cons.Length; i++)
            {
                ftp = new csFtp(cons[i]);
                string root_path = @"\temp";

                try
                {
                    if (ftp.connect())
                    {
                        ftp.cwd(root_path);
                        string[] ls = this.listFiles();

                        if (ls != null && ls.Length > 0)
                        {
                            for (int ndx = 0; ndx < ls.Length; ndx++)
                            {
                                if (ls[ndx].ToUpper().Contains(this._file_name.ToUpper()) & ls[ndx].ToUpper().Contains(this._ext.ToUpper()))
                                {
                                    string dl_path = this._temp_dir + @"\" + FileName;
                                    this._file_dir = dl_path;
                                    if (ftp.get(FileName, dl_path) > -1)
                                    {
                                        this._message = "";
                                        return readFile(dl_path);// + @"\" + this._file_name + "." + this._ext);
                                    }
                                    else
                                    {
                                        writeAudit(ftp.Message + ": " + root_path, csAuditLog.Category.FTP);
                                    }
                                }
                            }
                        }


                        if (this.FileName.ToUpper().Contains(".ERR"))
                        {
                            this._message = "The file " + new csFileIo().getFilenameNoExt(this._file_name) + " does not contain any error on the record.";
                        }
                        else this._message = "Invalid request. Please approve the file first or if the file is already approved, please wait for the confirmation email then try again.";
                    }
                    else
                    {
                        this._message = "Unable to connect to files server. Please contact system administrator. " + "<" + cons[i] + ">";
                        writeAudit(this._message + ": " + FileName, csAuditLog.Category.FTP);
                    }
                }
                catch (Exception e)
                {
                    this._message = e.Message;
                    return "";
                }
                finally
                {
                    ftp.disconnect();
                    ftp = null;
                }

                writeAudit(this._message + ": " + root_path + " - " + FileName, csAuditLog.Category.FTP);
            }
            return "";
        }

        public bool get_file_content(string fn)
        {

            this._file_name = fn;

            csFileIo nio = new csFileIo();

            try
            {
                if (getFileInventrory())
                {

                    string f_dir = (fn.Contains(":")) ? fn : this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + fn;
                    this._file_dir = f_dir;

                    if (nio.exist(f_dir))
                    {
                        return true;//this.readFile(f_dir);
                    }
                    else
                    {
                        //string rec = this.getFileFTP();
                        return this.getFileFTP();
                        //content = (rec == "") ? this.getTemp() : rec;
                    }
                }
            }
            catch (Exception e)
            {
                writeAudit(fn + ": " + e.Message, csAuditLog.Category.File);
            }
            finally
            {
                nio.close_input();
                nio = null;
            }

            return false;
        }

        public bool get_file_content(string[] file)
        {
            string content = "";
            this._file_name = file[0];
            this._ext = (file[1].Length == 0) ? new csFileIo().getFileExtension(this._file_name) : file[1];


            csFileIo nio = new csFileIo();

            try
            {
                if (getFileInventrory())
                {
                    this._file_name = new csFileIo().getFilenameNoExt(this._file_name);
                    string f_dir = (this._file_name.Contains(":")) ? this._file_name : this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + this._file_name;
                    if (nio.exist(f_dir))
                    {
                        return true;
                    }
                    else
                    {
                        //return this.getFileFTP();
                        return this.getFileFTP_3();
                    }
                }
            }
            catch (Exception e)
            {
                this._message = e.Message;
            }
            finally
            {
                nio.close_input();
                nio = null;
            }

            writeAudit(this._file_name + ": " + this._message, csAuditLog.Category.File);
            return false;
        }

        public void deleteFile()
        {
            new csFileIo().delete(this._file_dir);
        }

        public void writeAudit(string Description, csAuditLog.Category Category)
        {
            new csAuditLog().writeAudit("", "File Downloader", Description, "", "", Category);
        }

        public irsFileDownloader()
        {
            new csFileIo().createDirectory(this._temp_dir);
        }
    }
}
