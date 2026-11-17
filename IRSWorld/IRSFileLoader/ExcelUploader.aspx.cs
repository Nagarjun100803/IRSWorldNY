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
    [Serializable]
    public partial class ExcelUploader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ExcelUploader));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            //check appropiate role
            int role = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            bool isadmin = ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin;

            hfAllowedView.Value = "OK";
            hfuserrole.Value = new irsExcelFileUpload().get_fileuploadole(role).ToUpper();//uf.get_fileuploadole(role); 

            Session["DL_FILE_NAME"] = null;
            ispostback = this.IsPostBack;
        }

       
        protected void btnPost_Click(object sender, EventArgs e)
        {
            this.CTRL_VALS = null;
            if (fileBrowse.PostedFile.ContentLength > 0)
            {
                

                irsExcelFileUpload excelFile = this.IRS_EXCEL_FILE_UPLOAD();
                try
                {
                    fileBrowse.PostedFile.SaveAs(excelFile.fileDir(fileBrowse.PostedFile.FileName));
                    excelFile.readFile();
                    this.CTRL_VALS = excelFile.FileDetails;
                    //Session["AMOUNT_SUMMARY"] = excelFile.AmountSummary;
                }
                catch (Exception ex)
                {
                    excelFile.writeAudit(ex.Message + " : " + ex.StackTrace, csAuditLog.Category.File);
                    Session["UPLOADER_MSG"] = "ERROR: " + ex.Message;
                    //Commented to remove warning...
                    //RegisterStartupScript("ERROR:", "<script language='javascript'> alert('" + ex.Message  + "')</script>");
                    new csControl().showMessage(this.Page, "ERROR:", ex.Message);
                }
                finally
                {
                    if (excelFile.MESSAGE.Contains("ERROR"))
                    {
                        Session["UPLOADER_MSG"] = excelFile.MESSAGE;
                        ispostback = false;
                        this.CTRL_VALS = null;
                    }
                    Session["IRS_EXCEL_FILE_UPLOAD"] = excelFile;
                    excelFile = null;
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
                string role_des = new irsExcelFileUpload().getLookupCode("user roles", i_role);
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
            Session["IRS_EXCEL_FILE_UPLOAD"] = null;
            Session["CTRL_VALS"] = null;
            this.CTRL_VALS = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool client_logged_in()
        {
            return (Session["IRS_CLIENT_DATA"] == null && ((irsClientData)Session["IRS_CLIENT_DATA"]).HostName == null && ((irsClientData)Session["IRS_CLIENT_DATA"]).HostName.ToUpper().Contains("TIMEOUT")) ? false : true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool upload_ini(int type, int format, string filename, string rate_type)
        {
            if (client_logged_in())
            {
                try
                {
                    irsExcelFileUpload uploader = this.IRS_EXCEL_FILE_UPLOAD(IRS_CLIENT_DATA().Location, IRS_CLIENT_DATA().UserId, type, format, filename, rate_type);
                    return true;
                }
                catch (Exception ex)
                {
                    Session["UPLOADER_MSG"] = ex.Message;
                    this.session_time_out();
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool upload_from_ftp(int type, int format, string uploadpathname, string rate_type)
        {
            if (client_logged_in())
            {
                this.upload_ini(type, format, uploadpathname, rate_type);
                irsExcelFileUpload batchFile = this.IRS_EXCEL_FILE_UPLOAD();
                try
                {
                    batchFile.readFile(uploadpathname);
                    this.CTRL_VALS = batchFile.FileDetails;
                    return true;
                }
                catch
                {
                    this.session_time_out();
                }
                finally
                {
                    Session["IRS_EXCEL_FILE_UPLOAD"] = batchFile;
                    batchFile = null;
                }
            }
            return false;
        }

        private irsExcelFileUpload AMOUNT_SUMMARY()
        {
            if (Session["AMOUNT_SUMMARY"] == null)
            {
                Session["AMOUNT_SUMMARY"] = new irsExcelFileUpload();
            }
            return (irsExcelFileUpload)Session["AMOUNT_SUMMARY"];
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
                //irsBatchUpload bu = new irsBatchUpload(type, format, filename, rate_type);
                irsExcelFileUpload fu = new irsExcelFileUpload(type, format, filename, rate_type);
                try
                {
                    ds = fu.get_tempfile_data(((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
                    return ds;
                }
                catch //(Exception e)
                {
                    this.session_time_out();
                }
                finally
                {
                    ds = null;
                    fu = null;
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
                    this.upload_ini(type, format, filename, ratetype);
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
        public DataSet load_rate_type()
        {
            if (client_logged_in())
            {
                irsTimeZone timezone = new irsTimeZone();
                return (DataSet)new irsExcelFileUpload().get_rate_type(
                     timezone.branchLocalTime(((irsClientData)Session["IRS_CLIENT_DATA"]).Location),
                    ((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable show_uploaded_files(string type)
        {
            if (client_logged_in())
            {
                return (DataTable)new irsExcelFileUpload().show_uploaded_files(
                    ((irsClientData)Session["IRS_CLIENT_DATA"]).Role,
                    ((irsClientData)Session["IRS_CLIENT_DATA"]).Location,
                    ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin,
                    type);
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool download_file(string fname, string ext)
        {
            if (client_logged_in())
            {
                Session["DL_FILE_NAME"] = fname + "~" + ext;
                return true;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet file_format(string filetype)
        {
            if (client_logged_in())
            {
                irsExcelFileUpload upload = this.IRS_EXCEL_FILE_UPLOAD();
                try
                {
                    return (DataSet)upload.uploaderFileFormat(filetype);
                }
                catch
                {
                }
                finally
                {
                    Session["IRS_EXCEL_FILE_UPLOAD"] = (irsExcelFileUpload)upload;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_file_format(int val)
        {
            if (client_logged_in())
            {
                irsExcelFileUpload upload = this.IRS_EXCEL_FILE_UPLOAD();
                upload.FileFormat = val;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet file_type(string type)
        {
            if (client_logged_in())
            {
                return (DataSet)new irsExcelFileUpload().uploaderFileType(type);
            }
            return null;
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
        public bool approve_file(string fn, int ratetype)
        {
            if (client_logged_in())
            {
                irsExcelFileUpload batchFile = this.IRS_EXCEL_FILE_UPLOAD();
                batchFile.RateType = ratetype.ToString();
                batchFile.FileName = fn;

                try
                {
                    if (batchFile.updateInventory())
                    {
                        if (batchFile.savedToTemp())
                        {
                            batchFile.MESSAGE = "";
                            batchFile.moveToLoadingDirectory(fn);
                            return true;
                        }
                    }

                    this.set_upload_msg(batchFile.MESSAGE);
                }
                catch (Exception ex)
                {
                    Session["UPLOADER_MSG"] = ex.Message;
                }
                finally
                {
                    if (Session["UPLOADER_MSG"] == null)
                    {
                        this.set_upload_msg(batchFile.MESSAGE);
                    }

                    this.ispostback = false;
                    batchFile = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool process_file(string rate_type)
        {
            if (client_logged_in())
            {
                irsExcelFileUpload batchFile = this.IRS_EXCEL_FILE_UPLOAD();
                batchFile.RateType = rate_type;

                try
                {
                    if (batchFile.updateInventory())
                    {
                        batchFile.moveToLoadingDirectory();
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
                        this.set_upload_msg(batchFile.MESSAGE);
                    }

                    batchFile = null;
                    this.ispostback = false;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool save_file(int type, int format, string ratetype, string totrec, string runamt)
        {
            if (client_logged_in())
            {
                int userrole = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
                irsExcelFileUpload batchFile = this.IRS_EXCEL_FILE_UPLOAD();
                string role = batchFile.getLookupDesc("User Roles", userrole);

                try
                {
                    if (!batchFile.file_exists())
                    {
                        if (batchFile.savedToTemp())
                        {
                            batchFile.MESSAGE = "";
                            batchFile.selectedRow = -1;
                            if (batchFile.insertRecord())
                            {
                                batchFile.setParamVal(totrec, runamt, ratetype, role);
                                if (batchFile.commitRecord())
                                {
                                    if (batchFile.get_fileuploadole(userrole).ToUpper().Contains("AGENT"))
                                    {
                                        batchFile.moveToTempLoadingDirectory();
                                    }
                                    else
                                    {
                                        batchFile.moveToLoadingDirectory(batchFile.FileName);
                                    }
                                    this.set_upload_msg(batchFile.MESSAGE);
                                    return true;
                                }
                            }
                        }
                    }

                    this.set_upload_msg(batchFile.MESSAGE);
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
                        this.set_upload_msg(batchFile.MESSAGE);
                    }

                    this.ispostback = false;
                    Session["FILE_BROWSE"] = null;
                    batchFile = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_defaults()
        {
            if (client_logged_in())
            {
                if (!this.IRS_EXCEL_FILE_UPLOAD().checkDefaults())
                {
                    this.set_upload_msg(this.IRS_EXCEL_FILE_UPLOAD().MESSAGE);
                    return false;
                }
            }
            return true;
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public bool check_rates(string type)
        //{
        //    if (client_logged_in())
        //    {
        //        char c = Convert.ToChar(type.Substring(0, 1));
        //        return this.IRS_EXCEL_FILE_UPLOAD().checkRates(c);
        //    }
        //    return false;
        //}

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
                Response.Redirect("../IRSFileLoader/UploadFile.aspx");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_presummary(string fileformat, string fileuploadtype)
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
            string sql = "select fldcode from tbl_lookup where fldgroup = 'user roles' ";
            sql += "and flddescription like '%" + userroledesc + "%'";
            DataTable dt;
            try
            {
                dt = sh.getDataSet(sh.getSqlCommand(sql, false), "TBLROLES").Tables[0];
                return Convert.ToInt32(dt.Rows[0]["FLDCODE"].ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private bool upload_by_agent()
        {
            //string userdesc = "";
            return false;
        }
        private bool upload_by_maker()
        {
            //string userdesc = "Maker - ORD";
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

        private DataTable get_files_loaded(int _format, DateTime _br_time)
        {
            pyrl_decryptor pd = new pyrl_decryptor();
            return pd.populate_ftpfiles(_format, _br_time);
        }

        private irsUploadFile IRS_UPLOAD_FILE(string br, string user, int type, int format, string filename, string rate_type)
        {

            Session["IRS_UPLOAD_FILE"] = new irsUploadFile(br, user, type, format, filename, rate_type);

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

        private irsExcelFileUpload IRS_EXCEL_FILE_UPLOAD(string br, string user, int type, int format, string filename, string rate_type)
        {
            if (Session["IRS_EXCEL_FILE_UPLOAD"] == null)
            {
                Session["IRS_EXCEL_FILE_UPLOAD"] = new irsExcelFileUpload(br, user, type, format, filename, rate_type);
            }
            else
            {
                irsExcelFileUpload batchFile = IRS_EXCEL_FILE_UPLOAD();
                batchFile.setFile(type, format, filename, rate_type);
                Session["IRS_EXCEL_FILE_UPLOAD"] = batchFile;
                batchFile = null;
            }

            return IRS_EXCEL_FILE_UPLOAD();
        }

        private irsExcelFileUpload IRS_EXCEL_FILE_UPLOAD()
        {
            if (Session["IRS_EXCEL_FILE_UPLOAD"] == null)
            {
                
                Session["IRS_EXCEL_FILE_UPLOAD"] = new irsExcelFileUpload();
            }
            return (irsExcelFileUpload)Session["IRS_EXCEL_FILE_UPLOAD"];
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
        #endregion
    }
}
