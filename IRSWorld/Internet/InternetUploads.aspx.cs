using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;


using IRSClassLib;

namespace IRSWorld.Internet
{
    public partial class InternetUploads : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetUploads));

        }

        public String Cif_Info()
        {
            return ((irsClientData)Session["IRS_CLIENT_DATA"]).CIFID;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String upload_Documents(Object[] filelist)
        {
            string errmsg = "";
            string msg = "";
            string cifid = Cif_Info();
            string seq_ID = getSeq_Id(cifid);
            int err = 0;
            try
            {
                if (filelist[0].ToString() != "")
                {
                    if (!check_docs(filelist[0].ToString(), out errmsg)) 
                    {
                        msg += "FILE1:" + errmsg + "\n";
                        err++;
                    }
                }
                if (filelist[1].ToString() != "")
                {
                    if (!check_docs(filelist[1].ToString(), out errmsg))
                    {
                        msg += "FILE2:" + errmsg + "\n";
                        err++;
                    }
                }
                if (filelist[2].ToString() != "")
                {
                    if (!check_docs(filelist[2].ToString(), out errmsg))
                    {
                        msg += "FILE3:" + errmsg + "\n";
                        err++;
                    }
                }

                if (err > 0)
                {
                    return msg;
                }
                else 
                {
                    if (filelist[0].ToString() != "")
                    { uploaded(seq_ID, filelist[0].ToString(),cifid); }
                    if (filelist[1].ToString() != "")
                    { uploaded(seq_ID, filelist[1].ToString(),cifid); }
                    if (filelist[2].ToString() != "")
                    { uploaded(seq_ID, filelist[2].ToString(),cifid); }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to save record.\n" + ex.Message);
            }
            return msg;
        }
        

        private bool check_docs(String FileName, out String errmsg)
        {
            errmsg = "";
            int ok = 0;
            try
            {
                csFileIo fio = new csFileIo();
                string ext = fio.getFileExtension(FileName);
                string[] extName = { "JPG", "JPEG", "PNG" };

                for (int k = 0; k < extName.Length; k++)
                {
                    if (extName[k].ToUpper() == ext.ToUpper())
                    { ok = 1; }
                }
                if (!fio.exist(FileName))
                {
                    throw new Exception("File Does not exist.");
                }
                else if (fio.getFileSize(FileName) > 1024000) //File Size Limit is 1MB
                {
                    throw new Exception("File size is too big.");
                }
                else if (ok != 1)
                {
                    throw new Exception("Invalid file Format.Only images are Allowed.");
                }

                return true;
            }
            catch (Exception ex)
            {
                errmsg = "Failed to upload File " + FileName + "\n Reason, " + ex.Message;
            }
            return false;
        }

        private void uploaded(String SEQ_ID, String Filename,String cifid)
        {
            String ftpConString = "";
            String ftpRootPath = "";
            String FDir = "";
            String FName = "";
            String ext = "";
            String newFName = "";
            String name = "";
            String[] list;
            try
            {
                csFileIo fio = new csFileIo();
                sqlHelperWs sql = new sqlHelperWs();

                FDir = fio.getDirectory(Filename);
                FName = fio.getFilename(Filename);
                ext = fio.getFileExtension(Filename);
                DataTable dt = sql.getDataSet("InsertDocuments", true, new Object[] { SEQ_ID, cifid,FName }).Tables[0];
                name = FName.Substring(0,(FName.IndexOf(ext)-1));
                newFName = FDir + @"\" + "IRSD_" + SEQ_ID + "_" + name + "_" + dt.Rows[0].ItemArray[0].ToString() + "." + ext;
                ftpConString = new irsConfiguration().FTPConnectionString;
                ftpRootPath = new irsConfiguration().appsetting("RootPath").ToString() + @"INTERNETCLIENT\";
                csFtp ftp = new csFtp(ftpConString);
                
                if (ftp.connect())
                {
                    if (ftp.cwd(ftpRootPath))
                    {
                        ftp.put(Filename,newFName);
                    }
                    ftp.disconnect();
                }
            }
            catch (Exception e)
            { throw new Exception(e.Message);}
        }

        private String getSeq_Id(String CIFID)
        {
           sqlHelperWs sql = new sqlHelperWs();
           string seq_id = "";
           string cmd = "Select Seq_ID [SEQ_ID]From tbl_Internet_remitter Where RemitterCIFID = '" + CIFID + "'";
           DataTable dt = sql.getDataSet(cmd, false, new Object[] { }).Tables[0];
           
           if (dt.Rows.Count > 0)
           {
               seq_id = dt.Rows[0]["SEQ_ID"].ToString();
           }
           return seq_id;
        }

        private String[] getFileList()
        {
            string ftpRootPath = "";
            string[] filelist;

            ftpRootPath = new irsConfiguration().appsetting("RootPath").ToString();
            string[] ftpConString = new irsConfiguration().FTPConnectionString.Split(';');
            string[] server = ftpConString[0].Split('=');
            string[] userid = ftpConString[1].Split('=');
            string[] password = ftpConString[2].Split('=');

            server[1] = "ftp://" + server[1] + "/" + ftpRootPath + "/INTERNETCLIENT/*";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(server[1]);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(userid[1], password[1]);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            return filelist = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

        }

    } 
}
