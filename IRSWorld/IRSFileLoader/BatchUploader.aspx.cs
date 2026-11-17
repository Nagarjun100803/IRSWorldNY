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
    public partial class BatchUploader : System.Web.UI.Page
    {
        //int fileType = 0;
        //int fileFormat = 0;
        //int rateType = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BatchUploader));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            //check appropiate role
            int role = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            bool isadmin = ((irsClientData)Session["IRS_CLIENT_DATA"]).isSystemAdmin;

            hfAllowedView.Value = "OK";
            hfuserrole.Value = new irsBatchFileUpload().get_fileuploadole(role).ToUpper();//uf.get_fileuploadole(role); 

            //new PostBackOptions(btnPost).ClientSubmit = true;

            //Session["UPLOADER_MSG"] = null;
            Session["DL_FILE_NAME"] = null;
            ispostback = this.IsPostBack;
            //if (this.IsPostBack)
            //{
            //    //irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();

            //    //this.CTRL_VALS = null;

            //    //if (batchFile.ValidUploadCriteria)
            //    //{
            //    //    if (fileBrowse.PostedFile.ContentLength > 0)
            //    //    {
            //    //        try
            //    //        {
            //    //            string fn = new csFileIo().getFilename(fileBrowse.PostedFile.FileName);
            //    //            fileBrowse.PostedFile.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "temp" + @"\" + fn);
            //    //            batchFile.readFile(fn);
            //    //            this.CTRL_VALS = batchFile.FileDetails;
            //    ispostback = true;
            //    //            return;
            //    //        }
            //    //        catch (Exception ex)
            //    //        {
            //    //            Session["UPLOADER_MSG"] = ex.Message;
            //    //        }
            //    //        finally
            //    //        {
            //    //            if (batchFile.MESSAGE.Contains("ERROR")) Session["UPLOADER_MSG"] = batchFile.MESSAGE;
            //    //            Session["IRS_BATCH_FILE_UPLOAD"] = batchFile;
            //    //            batchFile = null;
            //    //        }
            //    //    }
            //    //}
            //}
            //else ispostback = false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validateRole(string type)
        {
            /* VAPT 20180222 */
            int i = 0;
            int role = ((irsClientData)Session["IRS_CLIENT_DATA"]).Role;
            DataTable dt = new irsInternetPage().getLookup("BatchUploaderRoleC");
            int allowedRole = 0;
            bool ok = false;

            if (type == "C")
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        allowedRole = Convert.ToInt32(dt.Rows[i]["FldCode"]);
                        if (role == allowedRole)
                        {
                            ok = true;
                            break;
                        }
                    }
                    if (!ok)
                    {
                        new csAuditLog().writeVAPTLogger("", this.Page.ToString(), "USER IS ACCESSING A RESTRICTED PAGE", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('You are not authorized to view this page. Access denied!');");
                    }
                }
                else
                {
                    new csAuditLog().writeVAPTLogger("", this.Page.ToString(), "USER IS ACCESSING A RESTRICTED PAGE", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('You are not authorized to view this page. Access denied!');");
                }
            }
            else
            {
                dt = new irsInternetPage().getLookup("BatchUploaderRoleNC");
                if (type == "NC")
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (i = 0; i <= (dt.Rows.Count - 1); i++)
                        {
                            allowedRole = Convert.ToInt32(dt.Rows[i]["FldCode"]);
                            if (role == allowedRole)
                            {
                                ok = true;
                                break;
                            }
                        }
                        if (!ok)
                        {
                            new csAuditLog().writeVAPTLogger("", this.Page.ToString(), "USER IS ACCESSING A RESTRICTED PAGE", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('You are not authorized to view this page. Access denied!');");
                        }
                    }
                    else
                    {
                        new csAuditLog().writeVAPTLogger("", this.Page.ToString(), "USER IS ACCESSING A RESTRICTED PAGE", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('You are not authorized to view this page. Access denied!');");
                    }
                }
                else
                {
                    new csAuditLog().writeVAPTLogger("", this.Page.ToString(), "USER IS ACCESSING A RESTRICTED PAGE", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('You are not authorized to view this page. Access denied!');");
                }
            }
            return ok;
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            this.CTRL_VALS = null;
            if (fileBrowse.PostedFile.ContentLength > 0)
            {
                irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();
                try
                {
                    fileBrowse.PostedFile.SaveAs(batchFile.fileDir(fileBrowse.PostedFile.FileName));
                    batchFile.readFile();
                    this.CTRL_VALS = batchFile.FileDetails;
                }
                catch (Exception ex)
                {
                    batchFile.writeAudit(ex.Message + " : " + ex.StackTrace, csAuditLog.Category.File); 
                    Session["UPLOADER_MSG"] = "ERROR: " + ex.Message;
                }
                finally
                {
                    if (batchFile.MESSAGE.Contains("ERROR"))
                    {
                        Session["UPLOADER_MSG"] = batchFile.MESSAGE;
                        ispostback = false;
                        this.CTRL_VALS = null;
                    }
                    Session["IRS_BATCH_FILE_UPLOAD"] = batchFile;
                    batchFile = null;
                }
            }
        }

        #region AjaxPro Method
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool reject_file(string fn) {
            if (client_logged_in())
            {
                irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();
                batchFile.FileName = fn;
                
                try
                {
                    if (batchFile.updateInventory("REJECT")) {
                        return true;
                    }

                    this.set_upload_msg(batchFile.MESSAGE);
                }
                catch (Exception ex)
                {
                    Session["UPLOADER_MSG"] = ex.Message;
                }
                finally
                {
                    this.ispostback = false;
                    batchFile = null;
                }
            }
            return false;
        }

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
            Session["IRS_BATCH_FILE_UPLOAD"] = null;
            Session["CTRL_VALS"] = null;
            this.CTRL_VALS = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool client_logged_in()
        {
            return (Session["IRS_CLIENT_DATA"] == null && ((irsClientData)Session["IRS_CLIENT_DATA"]).HostName == null && ((irsClientData)Session["IRS_CLIENT_DATA"]).HostName.ToUpper().Contains("TIMEOUT")) ? false : true;
        }

        /* RFC-32416 OFAC UPLOAD VALIDATION SEQUENCE */

        private void ofac_validation_insert_routine()
        {
            DataSet ds = null;
            irsSqlCommonsWs coms = null;

            string compliance_type = string.Empty;
            string ofac_type = string.Empty;
            string file_prefix = string.Empty;
            string file_numpart = string.Empty;
            string file_extension = string.Empty;
            string file_name = string.Empty;
            string user_id = string.Empty;
            string user_br = string.Empty;

            try
            {
                if (Session["OFAC_COMPLIANCE_TYPE"] != null)
                {                    
                    compliance_type = (string)Session["OFAC_COMPLIANCE_TYPE"];
                    ofac_type = (string)Session["OFAC_FILE_TYPE"];
                    file_prefix = (string)Session["OFAC_FILE_PREFIX"];
                    file_numpart = (string)Session["OFAC_FILE_NUMPART"];
                    file_extension = (string)Session["OFAC_FILE_EXTENSION"];
                    file_name = (string)Session["OFAC_FILE_NAME"];
                    user_id = (string)Session["OFAC_USER_ID"];
                    user_br = (string)Session["OFAC_USER_BR"];

                    if (compliance_type == "1" && (ofac_type == "6" || ofac_type == "7" || ofac_type == "8" || ofac_type == "30" || ofac_type == "31" || ofac_type == "32"))
                    {
                        coms = new irsSqlCommonsWs();
                        ds = coms.getDataSet("ofac_upload_seq_val", true, new object[] { 2, ofac_type, file_prefix, file_numpart, file_extension, file_name, user_id, user_br });
                    }                    
                }
            }
            catch
            {
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (coms != null)
                {
                    coms = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ofac_validation_routine(string compliance_type, string ofac_type, string file_name)
        {
            DataSet ds = null;

            irsSqlCommonsWs coms = new irsSqlCommonsWs();

            string user_id = string.Empty;
            string user_br = string.Empty;
            string file_prefix = string.Empty;
            string file_numpart = string.Empty;
            string file_extension = string.Empty;
            string file_other_pref = string.Empty;
            string[] file_array = null;
            string[] file_array_ext = null;

            try
            {
                if (compliance_type != null && compliance_type == "1")
                {
                    file_array = file_name.Split('_');
                    file_array_ext = file_name.Split('.');

                    user_id = ((irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                    user_br = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;


                    if (file_array_ext != null && file_array_ext.Length == 2 && ofac_type != null && (ofac_type == "6" || ofac_type == "7" || ofac_type == "8") && file_array.Length >= 2)
                    {
                        file_prefix = file_array[0];
                        file_numpart = file_array[1];
                        file_numpart = file_numpart.Replace("." + file_array_ext[1], "");
                        file_extension = file_array_ext[1];

                        if (file_array.Length == 3)
                        {
                            file_other_pref = file_array[2];
                            file_other_pref = file_other_pref.Replace("." + file_array_ext[1], "");
                        }
                    }

                    if (file_array_ext != null && file_array_ext.Length == 2 && ofac_type != null && (ofac_type == "30" || ofac_type == "31" || ofac_type == "32") && file_array.Length >= 3)
                    {
                        file_prefix = file_array[0] + "_" + file_array[1];
                        file_numpart = file_array[2];
                        file_numpart = file_numpart.Replace("." + file_array_ext[1], "");
                        file_extension = file_array_ext[1];

                        if (file_array.Length == 4)
                        {
                            file_other_pref = file_array[3];
                            file_other_pref = file_other_pref.Replace("." + file_array_ext[1], "");
                        }
                    }

                    coms = new irsSqlCommonsWs();

                    if (coms != null)
                    {
                        Session["OFAC_COMPLIANCE_TYPE"] = null;
                        Session["OFAC_FILE_TYPE"] = null;
                        Session["OFAC_FILE_PREFIX"] = null; 
                        Session["OFAC_FILE_NUMPART"] = null ;
                        Session["OFAC_FILE_EXTENSION"] = null;
                        Session["OFAC_FILE_NAME"] = null;
                        Session["OFAC_USER_ID"] = null;
                        Session["OFAC_USER_BR"] = null;

                        Session["OFAC_COMPLIANCE_TYPE"] = compliance_type;
                        Session["OFAC_FILE_TYPE"] = ofac_type;
                        Session["OFAC_FILE_PREFIX"] = file_prefix;
                        Session["OFAC_FILE_NUMPART"] = file_numpart;
                        Session["OFAC_FILE_EXTENSION"] = file_extension;
                        Session["OFAC_FILE_NAME"] = file_name;
                        Session["OFAC_USER_ID"] = user_id;
                        Session["OFAC_USER_BR"] = user_br;

                        ds = coms.getDataSet("ofac_upload_seq_val", true, new object[] { compliance_type, ofac_type, file_prefix, file_numpart, file_extension, file_name, user_id, user_br });

                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            return ds.Tables[0].Rows[0][0].ToString();
                        }                        
                    }
                }

                return " ";
            }
            catch
            {
                return " ";
            }
            finally
            {
                coms = null;
            }
        }
        /* RFC-32416 OFAC UPLOAD VALIDATION SEQUENCE */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool upload_ini(int type, int format, string filename, string rate_type)
        {
            if (client_logged_in())
            {
                try
                {
                    irsBatchFileUpload uploader = this.IRS_BATCH_FILE_UPLOAD(IRS_CLIENT_DATA().Location, IRS_CLIENT_DATA().UserId, type, format, filename, rate_type);
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
                irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();
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
                    Session["IRS_BATCH_FILE_UPLOAD"] = batchFile;
                    batchFile = null;
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
                    if (format.ToString().Trim().ToUpper() == "ANB")
                    {
                        irsEXCO_ANB_Approval ea = new irsEXCO_ANB_Approval(filename);

                        return ea.read_summary();
                    }
                    else
                    {
                    ds = bu.get_tempfile_data(((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
                    }
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
                return (DataSet)new irsBatchFileUpload().get_rate_type(
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
                return (DataTable)new irsBatchFileUpload().show_uploaded_files(
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
                irsBatchFileUpload upload = this.IRS_BATCH_FILE_UPLOAD();
                try
                {
                    return (DataSet)upload.uploaderFileFormat(filetype);
                }
                catch
                {
                }
                finally
                {
                    Session["IRS_BATCH_FILE_UPLOAD"] = (irsBatchFileUpload)upload;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_file_format(int val)
        {
            if (client_logged_in())
            {
                irsBatchFileUpload upload = this.IRS_BATCH_FILE_UPLOAD();
                upload.FileFormat = val;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet file_type(string type)
        {
            if (client_logged_in())
            {
                return (DataSet)new irsBatchFileUpload().uploaderFileType(type);
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
                irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();
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
                irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();
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
                irsBatchFileUpload batchFile = this.IRS_BATCH_FILE_UPLOAD();
                string role = batchFile.getLookupDesc("User Roles", userrole);

                try
                {
                    int FileType = 0;
                    int.TryParse(batchFile.FileDetails.Split('|')[0], out FileType);
                    
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

                                    // For Al Rajhi File                                                                        
                                    if (new IRSClassLib.irsEXCOAlRajhiResponse().check_if_alrajhi(batchFile.FileFormat, FileType))
                                    {
                                        new IRSClassLib.irsEXCOAlRajhiResponse().update_alrajhi_file_status(batchFile.FileName, true);
                                    }

                                    /* RFC-32416 OFAC UPLOAD VALIDATION SEQUENCE */

                                    this.ofac_validation_insert_routine();

                                    /* RFC-32416 OFAC UPLOAD VALIDATION SEQUENCE */

                                    return true;
                                }
                            }
                        }
                    }
                    // For Al Rajhi File
                    if (new IRSClassLib.irsEXCOAlRajhiResponse().check_if_alrajhi(batchFile.FileFormat, FileType))
                    {
                        new IRSClassLib.irsEXCOAlRajhiResponse().update_alrajhi_file_status(batchFile.FileName, false);
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
                if (!this.IRS_BATCH_FILE_UPLOAD().checkDefaults())
                {
                    this.set_upload_msg(this.IRS_BATCH_FILE_UPLOAD().MESSAGE);
                    return false;
                }
            }
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_rates(string type)
        {
            if (client_logged_in())
            {
                char c = Convert.ToChar(type.Substring(0, 1));
                return this.IRS_BATCH_FILE_UPLOAD().checkRates(c);
            }
            return false;
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
                Response.Redirect("../IRSFileLoader/UploadFile.aspx");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_presummary(string fileformat, string fileuploadtype)
        {
            return null;
        }

        /*************************************************/
        //Added code for FedAch Optimization 04052018
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_fedach_loading_summary(string filename)
        {
            irsBatchFileUpload upload = new irsBatchFileUpload();
            return upload.getFedAchLoadingSummary(filename);
        }
        /*************************************************/
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

        private irsBatchFileUpload IRS_BATCH_FILE_UPLOAD(string br, string user, int type, int format, string filename, string rate_type)
        {
            if (Session["IRS_BATCH_FILE_UPLOAD"] == null)
            {
                Session["IRS_BATCH_FILE_UPLOAD"] = new irsBatchFileUpload(br, user, type, format, filename, rate_type);
            }
            else
            {
                irsBatchFileUpload batchFile = IRS_BATCH_FILE_UPLOAD();
                batchFile.setFile(type, format, filename, rate_type);
                Session["IRS_BATCH_FILE_UPLOAD"] = batchFile;
                batchFile = null;
            }

            return IRS_BATCH_FILE_UPLOAD();
        }

        private irsBatchFileUpload IRS_BATCH_FILE_UPLOAD()
        {
            if (Session["IRS_BATCH_FILE_UPLOAD"] == null)
            {
                Session["IRS_BATCH_FILE_UPLOAD"] = new irsBatchFileUpload();
            }
            return (irsBatchFileUpload)Session["IRS_BATCH_FILE_UPLOAD"];
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