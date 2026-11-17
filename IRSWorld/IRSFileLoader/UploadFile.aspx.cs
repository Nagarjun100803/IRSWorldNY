using System;
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
    public partial class UploadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //check appropiate role

            int role = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            bool isadmin = ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin; 

          
            hfAllowedView.Value = "OK";
          

            //irsUploadFile uf = new irsUploadFile();
            hfuserrole.Value = new irsUploadFile().get_fileuploadole(role).ToUpper();//uf.get_fileuploadole(role); 

            new PostBackOptions(btnPost).ClientSubmit = true;
            AjaxPro.Utility.RegisterTypeForAjax(typeof(UploadFile));
            Session["UPLOADER_MSG"] = null;

            if (this.IsPostBack)
                ispostback = true;
            else ispostback = false;
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (fileBrowse.PostedFile.ContentLength > 0)
            {
                try
                {
                    irsUploadFile uploader = this.IRS_UPLOAD_FILE();
                    this.setInputFile();
                    string rec_count = uploader.verifile(fileBrowse);
                    string ctrl_vals = uploader.FILE_TYPE.ToString() + "|"
                                + uploader.FILE_FORMAT.ToString() + "|"
                                + fileBrowse.PostedFile.FileName.Replace("\\", "/") + "|"
                                + rec_count;
                    this.CTRL_VALS = ctrl_vals;
                    
                }
                catch (Exception ex)
                {
                    Session["UPLOADER_MSG"] = ex.Message;//"ERROR: FILE MAY NOT BE INTENDED FOR THE FORMAT YOU ARE LOADING";
                    //Page.ClientScript.RegisterStartupScript(typeof(System.String), "anything", "<script type='text/javascript'>alert(" + ex.Message + ");</script>");
                    //this.ispostback = false;
                    //this.Page_Load(new object(), System.EventArgs.Empty);
                    //throw ex;
                }
            }
        }

        #region AjaxPro Method
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void reset_objects()
        {
            Session["FILE_BROWSE"] = null;
            Session["UPLOADER_MSG"] = null;
            Session["DL_FILE_NAME"] = null;
            Session["IRS_UPLOAD_FILE"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool client_logged_in()
        {
            if (Session["IRS_CLIENT_DATA"] != null) return true;
            else
            {
                this.session_time_out();
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool upload_ini(int type, int format, string filename, string rate_type)
        {
            try
            {
                irsUploadFile uploader = this.IRS_UPLOAD_FILE(IRS_CLIENT_DATA().Location, IRS_CLIENT_DATA().UserId, type, format, filename, rate_type);
                return true;
            }
            catch (Exception ex)
            {
                Session["UPLOADER_MSG"] = ex.Message;
                this.session_time_out();
            }
            
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool upload_from_ftp(int type, int format, string uploadpathname, string rate_type)
        {
            try
            {

                this.upload_ini(type, format, uploadpathname,rate_type);
                irsUploadFile uploader = this.IRS_UPLOAD_FILE();
                string rec_count = uploader.veriftpfile(uploadpathname);
                string ctrl_vals = uploader.FILE_TYPE.ToString() + "|"
                            + uploader.FILE_FORMAT.ToString() + "|"
                            + uploadpathname.Replace("\\", "/") + "|"
                            + rec_count;
                this.CTRL_VALS = ctrl_vals;
                return true;
            }
            catch
            {
                this.session_time_out();
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet show_summary(string type, string format, string filename, string rate_type)
        {
            DataSet ds = new DataSet();
            try
            {
                irsBatchUpload bu = new irsBatchUpload(type, format, filename, rate_type);

                ds = bu.get_tempfile_data(((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
                return ds;
            }
            catch
            {

                this.session_time_out();
            }
            finally
            {
                ds = null;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable show_ftp_summary(int type, int format, string filename,string ratetype)
        {
           
            int role = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            string branch = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location ;
            bool isadmin = ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin;
            irsTimeZone itz = new irsTimeZone();
            DateTime br_time = itz.branchLocalTime(branch);
            try
            {
                this.upload_ini(type, format, filename, ratetype);
                if (type == 3)
                {
                    if (role == 16 || isadmin)
                    {
                        return this.get_files_loaded(format, br_time);
                    }
                }

                return null;
            }
            catch
            {
                return null; //this.session_time_out();
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool get_is_postback()
        {
            return (bool)ispostback;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void set_is_postback(bool val)
        {
            ispostback = val;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rate_type()
        {
            irsTimeZone timezone = new irsTimeZone();
            return (DataSet)new irsUploadFile().get_rate_type( 
                 timezone.branchLocalTime( ((irsClientData)Session["IRS_CLIENT_DATA"]).Location  ) , 
                ((irsClientData)Session["IRS_CLIENT_DATA"]).Location  );
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable show_uploaded_files()
        {
            return (DataTable)new irsUploadFile().show_uploaded_files(
                ((irsClientData)Session["IRS_CLIENT_DATA"]).Role,
                ((irsClientData)Session["IRS_CLIENT_DATA"]).Location,
                ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin); 
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool download_file(string fname)
        {
            Session["DL_FILE_NAME"] = fname;
            return true;
        }   

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet file_format(string filetype)
        {
            return (DataSet)new irsUploadFile().uploaderFileFormat(filetype);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet file_type()
        {
            return (DataSet)new irsUploadFile().uploaderFileType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string uploader_msg()
        {
            return (string)Session["UPLOADER_MSG"];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool approve_file(string fn, int ratetype)
        {
            irsUploadFile upld = this.IRS_UPLOAD_FILE();
            upld.RATE_TYPE = ratetype.ToString();
            upld.FILE_NAME = fn;

            try
            {
                if (upld.updateInventory())
                {
                    upld.moveToLoadingDirectory(fn);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Session["UPLOADER_MSG"] = ex.Message;
            }
            finally
            {
                if (Session["UPLOADER_MSG"] == null)
                {
                    this.set_upload_msg(upld.MESSAGE);
                }

                this.ispostback = false;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool process_file(string rate_type)
        {
            irsUploadFile uploader = this.IRS_UPLOAD_FILE();
            uploader.RATE_TYPE = rate_type;

            try
            {
                if (uploader.updateInventory())
                {
                    uploader.moveToLoadingDirectory();
                    return true;
                }
                else
                {
                    return false;
                 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Session["UPLOADER_MSG"] == null)
                {
                    this.set_upload_msg(uploader.MESSAGE);
                }

                this.ispostback = false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool save_file(int type, int format, string ratetype, string totrec, string runamt)
        {
            int userrole = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            irsUploadFile uploader = this.IRS_UPLOAD_FILE();


            try
            {
                if (!uploader.file_exists())//browser))
                {
                    //if (uploader.prepare_records())
                    //{
                        if (uploader.savedToTemp())
                        {
                            uploader.selectedRow = -1;
                            if (uploader.insertRecord())
                            {
                                uploader.setParamVal(totrec, runamt, ratetype);
                                if (uploader.commitRecord())
                                {
                                    if (uploader.get_fileuploadole(userrole).ToUpper().Contains("AGENT"))
                                    {
                                        uploader.moveToTempLoadingDirectory();
                                    }
                                    else
                                    {
                                        uploader.moveToLoadingDirectory(uploader.FILE_NAME);
                                    }
                                    this.set_upload_msg(uploader.MESSAGE);
                                    return true;
                                }
                            }
                        }
                    //}
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
                    this.set_upload_msg(uploader.MESSAGE);
                }

                this.ispostback = false;
                Session["FILE_BROWSE"] = null;
                Session["IRS_UPLOAD_FILE"] = null;
                uploader = null;
            }
            return false;
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool check_rates()
        {
            return this.IRS_UPLOAD_FILE().checkRates();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool check_rates(int rateType)
        {
            return this.IRS_UPLOAD_FILE().checkRates(rateType);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string ctrl_vals()
        {
            return (string)this.CTRL_VALS;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void session_time_out()
        {
            Page.ClientScript.RegisterStartupScript(typeof(System.String), "anything", "<script type='text/javascript'>alert('Session timeout.');</script>");
            Session.Abandon();
            Response.Redirect("ErrorPage.aspx");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void redirect_to_self()
        {
            Response.Redirect("../IRSFileLoader/UploadFile.aspx");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet get_presummary(string fileformat,string fileuploadtype)
        {
            return null;
        }
        #endregion

        #region PR1VATE        
        private void setInputFile()
        {
            Session["FILE_BROWSE"] = fileBrowse;
        }

        private void setFTPInputFile(string fullpath)
        {
            StreamReader sr = new StreamReader(fullpath);
            Session["FILE_BROWSE"] = sr;
        }

        private int get_role_by_desc(string userroledesc)
        {
            sqlHelper sh = new sqlHelper(); 
            string sql = "select fldcode from tbl_lookup where fldgroup = 'user roles' " ;
            sql += "and flddescription like '%" +  userroledesc  + "%'";
            DataTable dt;
            try
            {
                 dt = sh.getDataSet(sh.getSqlCommand(sql, false), "TBLROLES").Tables[0];
                 return Convert.ToInt32(dt.Rows[0]["FLDCODE"].ToString());
            }
            catch(Exception e)
            {
                throw new Exception(e.Message );
            }
        }

        private bool upload_by_agent()
        {
            return false;
        }
        private bool upload_by_maker()
        {
            return false;
        }

        private System.Web.UI.HtmlControls.HtmlInputFile inputFile()
        {
            if (Session["FILE_BROWSE"] != null)
            {
                return (System.Web.UI.HtmlControls.HtmlInputFile)Session["FILE_BROWSE"];
            }
            return null;
        }

        private DataTable get_files_loaded(int _format, DateTime  _br_time)
        {
            pyrl_decryptor pd = new pyrl_decryptor();
            return pd.populate_ftpfiles(_format, _br_time);             
        }

        private irsUploadFile IRS_UPLOAD_FILE(string br, string user, int type, int format,string filename,string rate_type)
        {

            Session["IRS_UPLOAD_FILE"] = new irsUploadFile(br, user  , type, format, filename,rate_type );

            return IRS_UPLOAD_FILE();
        }

        private irsUploadFile IRS_UPLOAD_FILE()
        {
            if (Session["IRS_UPLOAD_FILE"] == null)
            {
                Session["IRS_UPLOAD_FILE"] = new irsUploadFile();
            }
            return (irsUploadFile)Session["IRS_UPLOAD_FILE"];
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

        //private string ctrl_vals()
        //{
        //    return (string)Session["CTRL_VALS"];
        //}

        private void can_view_ftp_files(string role)
        {            
        }
        #endregion
    }
}