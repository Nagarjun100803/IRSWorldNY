using System;
using System.Data;
using System.IO;
using System.Net;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using IRSWorld.TranClass;

namespace IRSWorld.Faultlog
{
    public partial class Faultlog : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        private String location;
        private String user;
        private String branch_code;
        private String user_id;
        private String time;
        public String finalPath;
        protected void Page_Load(object sender, EventArgs e)
        {
            //new PostBackOptions(ScreenShot).ClientSubmit = true;
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Faultlog));
            //Session["UPLOAD_MSG"] = null;
            //if (Page.IsPostBack)
            //{
            //    ScreenShot.Visible = true;

            //}
            if (this.IsPostBack)
            {
                ispostback = true;
            }
            else ispostback = false;

        }

        public void btnUpload_Click(object sender, EventArgs e)
        {
            //if (ScreenShot.PostedFile.ContentLength > 0)
            //{
                //irsFaultlog uploadFile = new irsFaultlog();
                //try
                //{
                //    finalPath = uploadFile.readFile(ScreenShot);
                //}
                //catch (Exception ex)
                //{
                //    Session["UPLOAD_MSG"] = ex.Message;
                //}
                //finally
                //{
                //    Session["IRS_SCREENSHOT_UPLOAD"] = uploadFile;
                //    uploadFile = null;
                //}
            //}
            try
            {
                irsConfiguration config = new irsConfiguration();
                if (System.IO.Directory.Exists(config.appsetting("Upload")) == false)
                {
                    System.IO.Directory.CreateDirectory(config.appsetting("Upload"));
                }
                string myFolder = config.appsetting("Upload");

                csFileIo io = new csFileIo();
                string source_path = ScreenShot.PostedFile.FileName;
                string fileName = io.getFilename(source_path);
                string extension = io.getFileExtension(source_path);


                string contenttype = this.GetContentType(fileName);

                string[] type = contenttype.Split('/');
                if (type[0] == "image" || type[1] == "msword" || type[1] == "vnd.ms-excel")
                {

                    string path = myFolder + @"\";
                    string pathname = path + @"\" + fileName;
                    //csFileIo io = new csFileIo();
                    if (!io.directoryExist(path)) io.createDirectory(path);
                    //if (!Directory.Exists(path))
                    //{
                    //    string strFolderToCreate = path;
                    //    if (path.EndsWith(@"\"))
                    //        strFolderToCreate = path.Substring(0, (strFolderToCreate.Length - 1));

                    //    Directory.CreateDirectory(strFolderToCreate);
                    //}
                    //file.PostedFile.SaveAs(pathname);

                    path = path.Replace("\\", "/");

                    string fullPath = path + fileName;


                    Stream s = File.OpenRead(ScreenShot.PostedFile.FileName);

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, (int)s.Length);

                    int len = (int)s.Length;

                    if (len > 10485760)
                    {
                        //return "Error~ Image Size is too large!";
                        return;
                    }
                    ScreenShot.PostedFile.SaveAs(fullPath);

                    s.Dispose();
                    s.Close();

                    //FileStream fs = new FileStream(fullPath, FileMode.Create);
                    //fs.Write(buffer, 0, len);

                    //if (type[0] == "image")
                    //{
                    //    Bitmap bmp = new Bitmap(fs);

                    //    bmp.Save(fileName);
                    //    bmp.Dispose();
                    //}



                    //fs.Dispose();
                    //fs.Close();

                    return;
                }
                else
                {
                    return;
                }

                //commons.execScalar("sp_update_faultlog_ref_no", new object[] { seq_id, irsw_ref_no });
                //return true;
            }
            catch //(Exception ex)  // commented to remove warning
            {
                //string error = "Error~" + e.Message;
                return;
            }
           
        }

        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_finalPath()
        {
            return finalPath;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool get_is_postback()
        {
            return (bool)ispostback;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_is_postback(bool val)
        {
            ispostback = val;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_keyword(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_Faultlog_Status");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "FaultlogStatus");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return null;

        }

        #endregion

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_faultlog("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record_comment()
        {
            return this.do_faultlog_comment("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["FAULTLOG_SEQ_ID"] = field_group;

            return this.do_faultlog("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record_comment(string field_group)
        {
            Session["FAULTLOG_COMMENT_SEQ_ID"] = field_group;

            return this.do_faultlog_comment("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record()
        {
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_faultlog("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record_comment(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_faultlog_comment("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_faultlog("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record_comment()
        {
            return this.do_faultlog_comment("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["FAULTLOG_MESSAGE"] != null)
            {
                return (string)Session["FAULTLOG_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message_comment()
        {
            if (Session["FAULTLOG_COMMENT_MESSAGE"] != null)
            {
                return (string)Session["FAULTLOG_COMMENT_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["FAULTLOG_MESSAGE"] = null;
            Session["FAULTLOG_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message_comment(string msg)
        {
            Session["FAULTLOG_COMMENT_MESSAGE"] = null;
            Session["FAULTLOG_COMMENT_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_FAULTLOG_RECORD(string column_values)
        {
            irsFaultlog faultlog = this.FAULTLOG();

            string[] s = column_values.Split('^');

            if (s != null && s.Length > 0)
            {
                if (faultlog.set_record(s) == false)
                {
                    this.set_message(faultlog.messages); return false;
                }

                Session["IRS_FAULTLOG"] = faultlog;

                return true;
            }

            faultlog = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_FAULTLOG_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_FAULTLOG_COMMENT_RECORD(string column_values)
        {
            irsFaultlogComment faultlog_comment = this.FAULTLOG_COMMENT();

            string[] s = column_values.Split('^');

            if (s != null && s.Length > 0)
            {
                if (faultlog_comment.set_record(s) == false)
                {
                    this.set_message(faultlog_comment.messages); return false;
                }

                Session["IRS_FAULTLOG_COMMENT"] = faultlog_comment;

                return true;
            }

            faultlog_comment = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_FAULTLOG_COMMENT_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_FAULTLOG_RECORD(string client_seq_id)
        {
            irsFaultlog faultlog = this.FAULTLOG();
            try
            {
                if (faultlog != null)
                {
                    if (faultlog.get_record(client_seq_id) == true)
                    {
                        return faultlog.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_FAULTLOG"] = faultlog;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_FAULTLOG_COMMENT_RECORD(string client_seq_id)
        {
            irsFaultlogComment faultlog_comment = this.FAULTLOG_COMMENT();
            try
            {
                if (faultlog_comment != null)
                {
                    if (faultlog_comment.get_record(client_seq_id) == true)
                    {
                        return faultlog_comment.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_FAULTLOG_COMMENT"] = faultlog_comment;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string FAULTLOG_SEQ_ID()
        {
            if (Session["FAULTLOG_SEQ_ID"] != null)
            {
                return (string)Session["FAULTLOG_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string FAULTLOG_COMMENT_SEQ_ID()
        {
            if (Session["FAULTLOG_COMMENT_SEQ_ID"] != null)
            {
                return (string)Session["FAULTLOG_COMMENT_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_branchname(string branch_code)
        {
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                object br = commons.execScalar("sp_get_BranchName", new string[] { branch_code });
                return br.ToString();
            }
            catch (Exception e)
            {
                string error = "Unable to get BranchName" + e.Message;
                return error;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_fullname(string user_id)
        {
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                object br = commons.execScalar("sp_get_FullName", new string[] { user_id });
                return br.ToString();
            }
            catch (Exception e)
            {
                string error = "Unable to get User" + e.Message;
                return error;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void session_time_out()
        {
            Response.StatusCode = 302;
            Response.RedirectLocation = "../ErrorPage.aspx?ErrorMessage=Session%20Expired&EndMessage=Please Log-out";
            Response.Flush();
            Response.End();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string user_info()
        {
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                irsClientData id = (irsClientData)HttpContext.Current.Session["IRS_CLIENT_DATA"];
               
                String sql = "SELECT FullName FROM TBL_CIF_MASTER WHERE CIFID = " + "'" + id.CIFID + "'";
                String sql2 = "SELECT BranchName FROM TBL_BRANCHES WHERE BranchCode = " + "'" + id.Location + "'";
                
                user_id = id.UserId;
                branch_code = id.Location;

                if (user_id != null && branch_code != null)
                {
                    user = Convert.ToString(commons.execScalar(sql));
                    location = Convert.ToString(commons.execScalar(sql2));
                    time = new irsTimeZone().branchLocalTime(id.Location).ToString("MM/dd/yyyy hh:mm tt");
                }
                else
                {
                    this.session_time_out();
                }
                

            }
            catch //(Exception e) // commented to remove warning
            {
                this.session_time_out();
            }

            return user + "~" + branch_code +"~" +  location +"~" + user_id + "~" + time;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_refno(int seq_id, string irsw_ref_no)
        {
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                //String sql = "UPDATE TBL_FAULTLOG SET IssueReference = " + "'" + irsw_ref_no + "'" + "WHERE SEQ_ID = " + seq_id ;
                commons.execScalar("sp_update_faultlog_ref_no", new object[] { seq_id, irsw_ref_no });
                return true;
            }
            catch (Exception e)
            {
                string error = "Unable to Update!" + e.Message;
                return false;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string upload_scrshot(System.Web.UI.WebControls.FileUpload file)
        {
            try
            {
                irsConfiguration config = new irsConfiguration();
                if (System.IO.Directory.Exists(config.appsetting("Upload")) == false)
                {
                    System.IO.Directory.CreateDirectory(config.appsetting("Upload"));
                }
                string myFolder = config.appsetting("Upload");

                csFileIo io = new csFileIo();
                string source_path = file.PostedFile.FileName;
                string fileName = io.getFilename(source_path);
                string extension = io.getFileExtension(source_path);


                string contenttype = this.GetContentType(fileName);

                string[] type = contenttype.Split('/');
                if (type[0] == "image" || type[1] == "msword" || type[1] == "vnd.ms-excel")
                {

                    string path = myFolder + @"\";
                    string pathname = path + @"\" + fileName;
                    //csFileIo io = new csFileIo();
                    if (!io.directoryExist(path)) io.createDirectory(path);
                    //if (!Directory.Exists(path))
                    //{
                    //    string strFolderToCreate = path;
                    //    if (path.EndsWith(@"\"))
                    //        strFolderToCreate = path.Substring(0, (strFolderToCreate.Length - 1));

                    //    Directory.CreateDirectory(strFolderToCreate);
                    //}
                    //file.PostedFile.SaveAs(pathname);

                    path = path.Replace("\\", "/");

                    string fullPath = path + fileName;


                    Stream s = File.OpenRead(file.PostedFile.FileName);

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, (int)s.Length);

                    int len = (int)s.Length;

                    if (len > 10485760)
                    {
                        return "Error~ Image Size is too large!";
                    }
                    s.Dispose();
                    s.Close();

                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    fs.Write(buffer, 0, len);

                    if (type[0] == "image")
                    {
                        Bitmap bmp = new Bitmap(fs);

                        bmp.Save(fileName);
                        bmp.Dispose();
                    }



                    fs.Dispose();
                    fs.Close();

                    return fullPath;
                }
                else 
                {
                    return "Error~ Invalid image type!";
                }

                //commons.execScalar("sp_update_faultlog_ref_no", new object[] { seq_id, irsw_ref_no });
                //return true;
            }
            catch (Exception e)
            {
                string error = "Error~" + e.Message;
                return error;
            }

        }

        //btnFaultlogIssueSave
        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public string upload_scrshot(string get_path)
        //{
        //    try
        //    {
        //        irsFaultlog faultlog = new irsFaultlog();
        //        faultlog.readFile(get_path)

        //        irsConfiguration config = new irsConfiguration();
        //        if (System.IO.Directory.Exists(config.appsetting("Upload")) == false)
        //        {
        //            System.IO.Directory.CreateDirectory(config.appsetting("Upload"));
        //        }
        //        //string fileName = Path.GetFileName(n);
        //        string myFolder = config.appsetting("Upload");

        //        //string myFolder = "C:" + @"\" + "Inetpub" + @"\" + "wwwroot" + @"\" + "IRSWorld" + @"\" + "IRSWorld" + @"\" + "Faultlog" + @"\" + "Upload";
        //        string fileName = Path.GetFileName(get_path);
        //        string extension = Path.GetExtension(get_path);
               

        //        string contenttype = this.GetContentType(fileName);
                
        //        string[] type = contenttype.Split('/');
        //        if (type[0] == "image" || type[1] == "msword")
        //        {

        //            irsFaultlog faultlog = new irsFaultlog();
        //            if (!faultlog.saveFile(get_path))
        //            { 
                        
        //            }

        //            string path = myFolder + @"\";

        //            if (!Directory.Exists(path))
        //            {
        //                string strFolderToCreate = path;
        //                if (path.EndsWith(@"\"))
        //                    strFolderToCreate = path.Substring(0, (strFolderToCreate.Length - 1));

        //                Directory.CreateDirectory(strFolderToCreate);
        //            }

        //            path = path.Replace("\\", "/");

        //            string fullPath = path + fileName;


        //            Stream s = File.OpenRead(get_path);

        //            byte[] buffer = new byte[s.Length];
        //            s.Read(buffer, 0, (int)s.Length);

        //            int len = (int)s.Length;

        //            if (len > 10485760)
        //            {
        //                return "Error~ Image Size is too large!";
        //            }
        //            s.Dispose();
        //            s.Close();

        //            FileStream fs = new FileStream(fullPath, FileMode.Create);
        //            fs.Write(buffer, 0, len);

        //            if (type[0] == "image")
        //            {
        //                Bitmap bmp = new Bitmap(fs);

        //                bmp.Save(fileName);
        //                bmp.Dispose();
        //            }

                    

        //            fs.Dispose();
        //            fs.Close();

        //            return fullPath;
        //        }
        //        else 
        //        {
        //            return "Error~ Invalid image type!";
        //        }

        //        //commons.execScalar("sp_update_faultlog_ref_no", new object[] { seq_id, irsw_ref_no });
        //        //return true;
        //    }
        //    catch (Exception e)
        //    {
        //        string error = "Error~" + e.Message;
        //        return error;
        //    }

        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_path(int seq_id)
        {
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                irsClientData id = (irsClientData)HttpContext.Current.Session["IRS_CLIENT_DATA"];

                String sql = "SELECT ScreenShot FROM TBL_FAULTLOG WHERE SEQ_ID = " + "'" + seq_id  + "'";

                return Convert.ToString(commons.execScalar(sql));
                
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void download_screenshot(String path)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(path));
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(path).Length.ToString());
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.TransmitFile(path);
                Response.End();
            }
            catch { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_faultlogstatus()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_getFaultlogStatus");
                    return commons.getDataSet(sqcommands, "CountFaultlogStatus");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return null;

        }


        #endregion

        #region PRIVATE METHOD: FAULTLOG

        private irsFaultlog FAULTLOG()
        {
            if (Session["IRS_FAULTLOG"] == null)
            {
                Session["IRS_FAULTLOG"] = new irsFaultlog();
            }

            return (irsFaultlog)Session["IRS_FAULTLOG"];
        }

        private irsFaultlogComment FAULTLOG_COMMENT()
        {
            if (Session["IRS_FAULTLOG_COMMENT"] == null)
            {
                Session["IRS_FAULTLOG_COMMENT"] = new irsFaultlogComment();
            }

            return (irsFaultlogComment)Session["IRS_FAULTLOG_COMMENT"];
        }

        private bool do_faultlog(string instruction)
        {
            irsFaultlog cad = this.FAULTLOG();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cad.selectedRow = -1;
                            if (cad.insert_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            cad.seq_id = this.FAULTLOG_SEQ_ID();
                            cad.selectedRow = 0;

                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_FAULTLOG_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (cad.commit_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            cad.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {

                Session["IRS_FAULTLOG"] = null;
                Session["IRS_FAULTLOG"] = cad;
                Session["FAULTLOG_SEQ_ID"] = null;
                Session["FAULTLOG_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private string GetContentType(string fileName)
        {

            string contentType = "application/octetstream";

            string ext = Path.GetExtension(fileName).ToLower();

            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (registryKey != null && registryKey.GetValue("Content Type") != null)

                contentType = registryKey.GetValue("Content Type").ToString();

            return contentType;

        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool download_file(string fname)
        {
            Session["DL_FILE_NAME"] = fname;
            return true;
        }   


        private bool download(String path)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(path));
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(path).Length.ToString());
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.TransmitFile(path);
                Response.End();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool do_faultlog_comment(string instruction)
        {
            irsFaultlogComment cad = this.FAULTLOG_COMMENT();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cad.selectedRow = -1;
                            if (cad.insert_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            cad.seq_id = this.FAULTLOG_COMMENT_SEQ_ID();
                            cad.selectedRow = 0;

                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_FAULTLOG_COMMENT_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (cad.commit_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            cad.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {

                Session["IRS_FAULTLOG_COMMENT"] = null;
                Session["IRS_FAULTLOG_COMMENT"] = cad;
                Session["FAULTLOG_COMMENT_SEQ_ID"] = null;
                Session["FAULTLOG_COMMENT_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private bool ispostback
        {
            get { return (bool)Session["POSTBACK"]; }
            set { Session["POSTBACK"] = value; }
        }

        #endregion
    }
}
