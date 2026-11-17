using System;
using System.Text;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.IRSFileLoader
{
    [Serializable]
    public partial class FEDACHMatchingUploadFile : System.Web.UI.Page
    {
        //int fileType = 0;
        //int fileFormat = 0;
        //int rateType = 0;
        string uploaded_file_name = "";
        double filesize = 0;
        string file_name = "";
        string stemp_dir = "";
        string MESSAGE ="";
        string TempDest = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(FEDACHMatchingUploadFile));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            //check appropiate role
            int role = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            bool isadmin = ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin;

            hfAllowedView.Value = "OK";
            //hfuserrole.Value = new irsBatchFileUpload().get_fileuploadole(role).ToUpper();//uf.get_fileuploadole(role); 

            
            Session["DL_FILE_NAME"] = null;
            Session["UL_FILE_NAME"] = null;
            ispostback = this.IsPostBack;
            
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            this.CTRL_VALS = null;
            
            if (fileBrowse.PostedFile.ContentLength > 0)
            {
                
                try
                {
                    string uploadedfile = fileBrowse.PostedFile.FileName;

                    string ul_filename  = this.fileDir(uploadedfile);
                    Session["UL_FILE_NAME"] = ul_filename;
                    fileBrowse.PostedFile.SaveAs(ul_filename);
                    this.readFile(ul_filename);                    
                    this.CTRL_VALS = this.FileDetails();
                    ispostback = true;
                }
                catch (Exception ex)
                {
                    this.MESSAGE = "ERROR: " + ex.Message;
                    this.writeAudit(ex.Message + " : " + ex.StackTrace, csAuditLog.Category.File);
                    Session["UPLOADER_MSG"] = this.MESSAGE;
                }
                finally
                {                    
                        
                 
                }
            }
        }

        #region AjaxPro Method
      

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool is_agent()
        {
            if (client_logged_in())
            {
                int i_role = this.IRS_CLIENT_DATA().Role;
                string role_des = new irsBatchFileUpload().getLookupCode("user roles", i_role);
                return (role_des.ToUpper().IndexOf("AGENT") > -1);
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void reset_objects()
        {
            Session["FILE_BROWSE"] = null;
            Session["UPLOADER_MSG"] = null;
            Session["DL_FILE_NAME"] = null;
            Session["UL_FILE_NAME"] = null;
            Session["IRS_BATCH_FILE_UPLOAD"] = null;
            Session["CTRL_VALS"] = null;
            this.CTRL_VALS = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool client_logged_in()
        {
            return (Session["IRS_CLIENT_DATA"] == null && ((irsClientData)Session["IRS_CLIENT_DATA"]).HostName == null && ((irsClientData)Session["IRS_CLIENT_DATA"]).HostName.ToUpper().Contains("TIMEOUT")) ? false : true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool upload_ini(string filename)
        {
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool upload_from_ftp(int type, int format, string uploadpathname, string rate_type)
        {
            if (client_logged_in())
            {
                this.upload_ini(uploadpathname);
                //irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();
                try
                {
                    this.readFile(uploadpathname);
                    this.CTRL_VALS = this.FileDetails();
                    return true;
                }
                catch
                {
                    this.session_time_out();
                }
                finally
                {
                    
                }
            }
            return false;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string replace(string val, string old, string _new)
        {
            return val.Replace(old, _new);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet show_summary(string type, string format, string filename, string rate_type)
        {
            if (client_logged_in())
            {
                DataSet ds = new DataSet();
                irsBatchUpload bu = new irsBatchUpload(type, format, filename, rate_type);
                try
                {
                    ds = bu.get_tempfile_data(((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
                    return ds;
                }
                catch //(Exception e)
                {
                    this.session_time_out();
                }
                finally
                {
                    ds = null;
                    bu = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable show_ftp_summary(int type, int format, string filename, string ratetype)
        {
            if (client_logged_in())
            {
                int role = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
                string branch = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
                bool isadmin = ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin;
                irsTimeZone itz = new irsTimeZone();
                DateTime br_time = itz.branchLocalTime(branch);
                try
                {
                    this.upload_ini(filename);
                    if (type == 3)
                    {
                        if (role == 16 || isadmin)
                        {
                            return this.get_files_loaded(format, br_time);
                        }
                    }
                }
                catch //(Exception e)
                {

                    this.session_time_out();
                }
                finally { }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool get_is_postback()
        {
            return (bool)ispostback;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_is_postback(bool val)
        {
            if (!val)
            {
                Session["FILE_BROWSE"] = null;
                Session["DL_FILE_NAME"] = null;
                Session["CTRL_VALS"] = null;
            }
            ispostback = val;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable show_uploaded_files(string type)
        {
            sqlHelperWs sws = new sqlHelperWs();

            try
            {
                SqlCommand cmd = sws.getSqlCommand("proc_get_fedmatching_info", true);
                return sws.getDataSet(cmd, "TBLUPLOAD").Tables[0];
            }
            catch
            {
                return null;
            }
            finally { sws = null; }

        }
     
                      
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string uploader_msg()
        {
            if (client_logged_in())
            {
                try
                {
                    return (string)Session["UPLOADER_MSG"];
                }
                catch { }
                finally
                {
                    Session["UPLOADER_MSG"] = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ctrl_vals()
        {
            if (client_logged_in())
            {
                return (string)this.CTRL_VALS;
            }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void session_time_out()
        {
            Page.ClientScript.RegisterStartupScript(typeof(System.String), "anything", "<script type='text/javascript'>alert('Session timeout.');</script>");
            Session.Abandon();
            Response.Redirect("ErrorPage.aspx");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void redirect_to_self()
        {
            if (client_logged_in())
            {
                Response.Redirect("../IRSFileLoader/FEDACHMatchinUploadFile.aspx");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_presummary(string fileformat, string fileuploadtype)
        {
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool save_file()
        {
            if (client_logged_in())
            {       
                try
                {   
                    if (!this.file_exists())
                    {
                        if (this.savedToTemp())
                        {
                            this.MESSAGE = "";                         
                            this.moveToLoadingDirectory();
                            return true;
                         }
                         this.set_upload_msg(this.MESSAGE);
                         return false;
                    }
                }
                catch (Exception ex)
                {
                    this.set_upload_msg(ex.Message);
                    return false;
                }
                finally
                {
                    if (Session["UPLOADER_MSG"] == null)
                    {
                        this.set_upload_msg(this.MESSAGE);
                    }

                    this.ispostback = false;
                    Session["FILE_BROWSE"] = null;
                    
                }
            }
            return false;
        }

        #endregion

        #region PR1VATE
        
        private System.Web.UI.HtmlControls.HtmlInputFile inputFile()
        {
            if (Session["FILE_BROWSE"] != null)
            {
                return (System.Web.UI.HtmlControls.HtmlInputFile)Session["FILE_BROWSE"];
            }
            return null;
        }

        private DataTable get_files_loaded(int _format, DateTime _br_time)
        {
            pyrl_decryptor pd = new pyrl_decryptor();
            return pd.populate_ftpfiles(_format, _br_time);
        }

        private irsClientData IRS_CLIENT_DATA()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
                return (irsClientData)Session["IRS_CLIENT_DATA"];
            else
            {
                Response.Redirect("ErrorPage.aspx");
                return null;
            }
        }

        private void set_upload_msg(string msg)
        {
            Session["UPLOADER_MSG"] = null;
            Session["UPLOADER_MSG"] = msg;
        }

        private string CTRL_VALS
        {
            get { return (string)Session["CTRL_VALS"]; }
            set { Session["CTRL_VALS"] = value; }
        }

        private bool ispostback
        {
            get { return (bool)Session["POSTBACK"]; }
            set { Session["POSTBACK"] = value; }
        }

        private void can_view_ftp_files(string role)
        {
            //string sql = "";
        }

        private void readFile(string filename)
        {
            
            csFileIo io = new csFileIo();
            try
            {
                this.file_name = io.getFilename(filename);



                filesize = io.getFileSize(filename);
            }
            finally { io = null; }

        }

        private string FileDetails()
        {
            return this.file_name + "|" +
                this.getFileSize() + "|";
        }

        public string fileDir(string postedFile)
        {
            this.temp_dir();
            this.uploaded_file_name = new csFileIo().getFilename(postedFile);
            csFileIo io = new csFileIo();
            if (!io.directoryExist(this.stemp_dir))
            {
                io.createDirectory(this.stemp_dir);
            }
            io = null;
            return this.stemp_dir + @"\" + this.uploaded_file_name;
        }

        private void temp_dir()
        {            
            try
            {
                this.stemp_dir = new irsConfiguration().appsetting("temp_dir").ToString();
            }
            catch
            {
                throw new Exception("Incomplete config: temp_dir");
            }

            if (!(new csFileIo().directoryExist(this.stemp_dir)))
            {
                new csFileIo().createDirectory(this.stemp_dir);
            }
        }

        private string getFileSize()
        {
            double file_size_kb = Convert.ToDouble(this.filesize) / 1000;
            double file_size_mb = Convert.ToDouble(this.filesize) / 1000000;

            if (file_size_kb > 0.01)
            {
                return file_size_kb.ToString() + " kb";
            }
            else if (file_size_mb > 0.01)
            {
                return file_size_mb.ToString() + " mb";
            }
            else
            {
                return this.filesize.ToString() + " bytes";
            }
        }

        private bool file_exists()
        {
            return System.IO.File.Exists(this.uploaded_file_name);
            
        }

        private void moveToLoadingDirectory()
        {
            csFileIo fio = new csFileIo();
            string ftpConString = new irsConfiguration().FTPConnectionString;
            csFtp ftp = new csFtp(ftpConString);
            string nl = Environment.NewLine;
            string loadingpath ="";
            try
            {
                loadingpath = new irsConfiguration().appsetting("FEDMATCHFOLDER");

                StringBuilder sb_file = new StringBuilder();

                if (ftp.connect())
                {
                    if (ftp.binary(true))
                    {
                        string[] dir = loadingpath.Replace(@"\", "/").Split('/');
                        //for (int i = 0; i < dir.Length; i++)
                        //{
                        //    if (dir[i] != "")
                        //    {
                        //        ftp.mkdir(dir[i]);
                        //        ftp.cwd(dir[i]);
                        //    }
                        //}

                        if (ftp.cwd(loadingpath))
                        {
                            this.uploaded_file_name = (string)Session["UL_FILE_NAME"];
                            if (ftp.put(this.uploaded_file_name))
                            {
                                if (ftp.Message != null)
                                {
                                    if (!ftp.Message.Contains("550")) this.MESSAGE = ftp.Message + " : ";

                                    else this.MESSAGE += "The file " + this.file_name + " will now be processed for Matching.";

                                }

                                this.MESSAGE += "The file " + this.file_name + " will now be processed for FEDACH Matching.";

                                this.writeAudit("FEDACH FILE UPLOADED - " + this.file_name + "-" + ((irsClientData)Session["IRS_CLIENT_DATA"]).UserId,csAuditLog.Category.FTP);

                                this.writeAudit("FTP Successful - " + this.MESSAGE, csAuditLog.Category.FTP);
                            }
                            else
                            {
                                this.MESSAGE = "ERROR: Cannot proceed to your request. " + ftp.Message;
                            }
                        }
                        else
                        {
                            this.MESSAGE = "ERROR: Ftp failed - " + ftp.Message;
                        }
                        //}
                    }
                    else
                    {
                        this.MESSAGE = "ERROR: Ftp failed - " + ftp.Message;
                    }
                }
            }
            catch (Exception e)
            {
                this.MESSAGE = e.Message;
                throw new Exception(e.Message);
            }
            finally
            {
                if (this.MESSAGE.Contains("ERROR"))
                {
                    this.writeAudit("FTP ERROR: " + this.MESSAGE, csAuditLog.Category.FTP);
                }

                ftp.disconnect();
                ftp = null;
                this.uploadMailer();                
                this.deleteTempFile();
            }
        }

        private bool savedToTemp()
        {
            this.temp_dir();
            string ftps = new irsConfiguration().FTPConnectionString;
            string ftps1 = new irsConfiguration().appsetting("FTPSource1");
            string ftps2 = new irsConfiguration().appsetting("FTPSource2");
            irsFileDownloader dl = new irsFileDownloader();

            if ((ftps != null && ftps.Length == 0) | (ftps1 != null && ftps1.Length == 0) | (ftps2 != null && ftps2.Length == 0))
            {
                this.MESSAGE = "Config not defined.";
                this.writeAudit("savedToTemp - " + this.MESSAGE, csAuditLog.Category.FTP);
                return false;
            }
            string[] cons = new string[] { ftps };

            this.file_name = (string)Session["UL_FILE_NAME"] ;
            for (int i = 0; i < cons.Length; i++)
            {
                if (dl.check_file(this.file_name, "temp", cons[i]))
                {
                    return dl.download_file(this.file_name, "temp", this.TempDest, cons[i]);
                }
                
            }
            return true;
        }

        private void deleteTempFile()
        {
            
            if (new csFileIo().exist(this.file_name))
            {
                this.TempDest = this.file_name;
                new csFileIo().delete(this.TempDest);
            }
        }

        private void uploadMailer()
        {
            //csMailer mail = new csMailer();
            //string subject = "IRSWORLD Upload - " + this._file_name;
            //string[] recipient = this.emailRecipient(false, true);
            //string msg = "";
            //msg = (this._file_type_desc.ToUpper() == "ARS") ? this.emailUpload() : this.defaultEmailMessage();

            //try
            //{
            //    if (mail.sendMail(subject, recipient, msg))
            //    {
            //        this.MESSAGE += "\nYou will receive an email containing file summary.";
            //        writeAudit("Email done - " + mail.Message, csAuditLog.Category.Insert);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.writeAudit("Email Failure: " + ex.Message + " - uploadMailer", csAuditLog.Category.Insert);
            //}
        }

        private void writeAudit(string Description, csAuditLog.Category Category)
        {
            new csAuditLog().writeAudit("", "FEDACH File Loader", Description, "", "", Category);
        }
        #endregion

      
    }
}