using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using System.Net;
using System.IO;


namespace IRSWorld.Internet
{
    public partial class InternetRemitterApproval : System.Web.UI.Page
    {
        public String file_Name = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetRemitterApproval));
            
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitter()
        {
            return new irsInternetRemitterApproval().getRemitter();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void updateDocument(string cifid, string option)
        {
            new irsInternetRemitterApproval().updateDocument(cifid, option);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void setValue(String file)
        {
            hf_Filename.Value = file;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable file_Exists(string cifid)
        {
            int seq_id = 0;
            string[] list = new string[]{ };
            DataTable dtk = new DataTable();
            string[] ftpConString = new irsConfiguration().FTPConnectionString.Split(';');
            string[] server = ftpConString[0].Split('=');
            string[] userid = ftpConString[1].Split('=');
            string[] password = ftpConString[2].Split('=');
            server[1] = "ftp://" + server[1] + "/IRS_Loading/INTERNETCLIENT/*";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server[1]);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(userid[1], password[1]);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            string[] filelist = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            sqlHelperWs sql = new sqlHelperWs();
            
            DataTable dt = sql.getDataSet("SELECT TOP 1 SEQ_ID FROM TBL_INTERNET_REMITTER WHERE REMITTERCIFID = '" + cifid + "'", false, new object[] { }).Tables[0];
            if (dt.Rows.Count > 0)
            {
                seq_id = int.Parse(dt.Rows[0].ItemArray[0].ToString());
                DataRow dr = null;
                string cList = "";
                foreach (string f in filelist)
                {
                    
                    string[] p = f.Split(new char[] { '_' });
                    if (p.Length > 1)
                    {
                        if (seq_id.ToString() == p[1])
                        {
                            cList += f + "~";
                        }
                    }
                }
                dtk.Columns.Add(new DataColumn("Document", typeof(string)));
                dr = dtk.NewRow();
                string arrList = cList.Substring(0, cList.Length - 1);
                list = arrList.Split(new string[] {"~"}, StringSplitOptions.RemoveEmptyEntries);
                //for (int k = 0; list.Length > k; k++)
                //{
                //    dr["Document"] = list[k];
                //}
                //dtk.Rows.Add(dr); 
                foreach (string k in list)
                {
                    dtk.Rows.Add(k);
                }
            }
            return dtk;
        }

        protected void test_download(object sender, EventArgs e)
        {
           csFileIo fio = new csFileIo();
            int bytesRead ;
            FileStream writeStream;
            string ftpRootPath = "";
            string filepath = "";
            ftpRootPath = new irsConfiguration().appsetting("InternetDownload").ToString();

            //if (!fio.directoryExist(ftpRootPath))
            //{
            //    fio.createDirectory(ftpRootPath);
            //}
            string[] ftpConString = new irsConfiguration().FTPConnectionString.Split(';');
            
            string[] server = ftpConString[0].Split('=');
            string[] userid = ftpConString[1].Split('=');
            string[] password = ftpConString[2].Split('=');

            server[1] = "ftp://" + server[1] + "/IRS_Loading/INTERNETCLIENT/*";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server[1]);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            request.Credentials = new NetworkCredential(userid[1], password[1]);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            string[] filelist = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            sqlHelperWs sql = new sqlHelperWs();

            DataTable dt = sql.getDataSet("SELECT TOP 1 SEQ_ID FROM TBL_INTERNET_REMITTER WHERE REMITTERCIFID = '" + dl_cifid.Value + "'", false, new object[] { }).Tables[0];

            int seq_id = 0;

            if (dt.Rows.Count > 0)
            {
                seq_id = int.Parse(dt.Rows[0].ItemArray[0].ToString());
            }
            foreach (string f in filelist)
            {
                {
                    if (hf_Filename.Value == f)
                    {
                        reader.Close();
                        response.Close();

                        filepath = server[1].Substring(0, server[1].Length - 1) + f;

                        request = (FtpWebRequest)WebRequest.Create(filepath);
                        request.Method = WebRequestMethods.Ftp.DownloadFile;
                        request.Credentials = new NetworkCredential(userid[1], password[1]);
                        response = (FtpWebResponse)request.GetResponse();
                        responseStream = response.GetResponseStream();
                        writeStream = new FileStream(ftpRootPath + f, FileMode.Create);
                        int Length = 1048576;
                        Byte[] buffer = new Byte[Length];
                        bytesRead = responseStream.Read(buffer, 0, Length);
                        while (bytesRead > 0)
                        {
                            Response.OutputStream.Write(buffer, 0, bytesRead);
                            //writeStream.Write(buffer, 0, bytesRead);
                            bytesRead = responseStream.Read(buffer, 0, Length);
                        }
                        writeStream.Close();
                        break;
                    }
                }
            }
            string FName = fio.getFilename(filepath);
            if (FName != "")
            {
                Context.Response.ContentType = "image/jpeg";
                Context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + FName);
             
                //Context.Response.TransmitFile(@"~\" + ftpRootPath + @"\" + FName);
                Context.Response.Flush();
            }
            else
            { }

            Response.End();
            reader.Close();
            response.Close();

        }


    }


}
