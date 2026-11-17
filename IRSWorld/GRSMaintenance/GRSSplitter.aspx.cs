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
using System.Text;
using System.IO;
using IRSClassLib;

namespace IRSWorld.GRSMaintenance
{
    public partial class GRSSplitter : System.Web.UI.Page
    {

        #region "Page Load"

        protected void Page_Load(object sender, EventArgs e)
        {
            splitter_ui_code ui = new splitter_ui_code();

            AjaxPro.Utility.RegisterTypeForAjax(typeof(GRSSplitter));

            if (!Page.IsPostBack)
            {
                this.construct_pris_directory();
            }
            this.choose_payroll_source();
            this.Page.Response.Cache.SetNoStore();
            this.Page.Response.AppendHeader("Pragma", "no-cache");
        }

        private void construct_pris_directory()
        {
            // Creation of folder if not existing
            // What connection will use
            //csFileIo io = new csFileIo();
            //irsSqlCommons com = new irsSqlCommons();

            //string pris_directory =   com.getAppSetting("prisUploadDir");
            //string pris_encrypted = pris_directory + @"\PRIS_ENCRYPTED\";
            //string pris_decrypted = pris_directory + @"\PRIS_UNENCRYPTED\";

            //if (io.directoryExist(pris_encrypted) == false)
            //{
            //    io.createDirectory(pris_encrypted);
            //}
            //if (io.directoryExist(pris_decrypted) == false)
            //{
            //    io.createDirectory(pris_decrypted);
            //}

            //io = null;
            //com = null;
        }
        #endregion


        #region "file validation"

        private void choose_payroll_source()
        {
            HtmlInputFile file = null;

            file = (HtmlInputFile)Page.FindControl("File1");

            if (file != null)
            {
                if (file.PostedFile != null)
                {
                    if (file.PostedFile.ContentLength > 0)
                    {
                        this.check_file();
                        file.Dispose();
                        file = null;
                        return;
                    }
                }
            }
            this.do_portal_loading(); file = null;
        }

        private void check_file()
        {
            String msg = String.Empty;
            csFileIo fio = new csFileIo();
            HtmlInputFile file = null;

            try
            {
                file = (HtmlInputFile)Page.FindControl("File1");
                if (file.PostedFile.ContentLength > 0)
                {
                    String new_path = add_backslash(copy_file(file, out msg));
                    if (new_path != null && msg == "")
                    {
                        new csControl().execScript(this.Page, "FILE_COPIED_", "file_work('" + new_path + "');");  
                        return;
                    }
                }
                else
                {
                    msg = "The input file is empty";
                }
            }
            catch (Exception e)
            {
                msg = "check_file" + e.Message;
            }
            finally
            {
                if (file != null) { file.Dispose(); file = null; }
                if (fio != null) { fio = null; }
            }
        }

        private String add_backslash(String path)
        {
            StringBuilder sb = new StringBuilder();
            char[] cc = path.ToCharArray();
            for (int i = 0; i < cc.Length; i++)
            {
                if (cc[i] == '\\')
                {
                    sb.Append(@"\");
                }
                sb.Append(cc[i]);
            }
            return sb.ToString();
        }

        private String copy_file(HtmlInputFile file, out String msg)
        {
            msg = "";

            csFileIo tio = null;
            StreamReader sr = null;

            try
            {
                tio = new csFileIo();

                String new_path = getOutputPath(file.Value);

                if (tio.open_output(new_path, (int)csFileIo.write_modes.CREATE))
                {
                    sr = new StreamReader(file.PostedFile.InputStream);

                    while (true)
                    {
                        String buff = sr.ReadLine();
                        if (buff == null) break;
                        tio.putLine(buff);
                    }

                    tio.close_output();

                    return tio.OutPutFileName;
                }
                else
                {
                    msg = "Unable to open output " + new_path;
                }
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            finally
            {
                if (tio != null) { tio = null; }
                if (sr != null) { sr.Dispose(); sr = null; }
            }

            return null;
        }

        private String getOutputPath(String local_path)
        {
            String fname = new csFileIo().getFilename(local_path);
            //hard code muna.. ask sir jeff
            String path = "C:\\GRSFILELOADER\\GRSWEBFileLoader\\GRSWebFileLoader\\";//AppDomain.CurrentDomain.BaseDirectory;
            return path + @"temp\" + fname;
        }

        private void do_portal_loading()
        {
            bool isencrypted = false;

            try
            {
                // this.pnlUpload.Visible = false;

                if (Request.QueryString["isfromportal"] != null)
                {
                    if (Request.QueryString["isfromportal"].ToString().Trim().ToUpper() == "TRUE")
                    {
                        isencrypted = (Request.QueryString["isencrypted"].ToString().Trim().ToUpper().Contains("YES")) ? true : false;
                        Session["isfromportal"] = Request.QueryString["isfromportal"].ToString().Trim().ToUpper();
                        Session["isencrypted"] = isencrypted;
                        this.choose_dataitem(Request.QueryString["filename"].ToString(), Convert.ToBoolean(Request.QueryString["isfromportal"]), isencrypted);
                    }
                }
            }
            catch (Exception ex)
            {
                new csControl().execScript(this.Page, "PAGE_MESSAGE_", "do_process_msg('" + ex.Message + "',1);");
            }
        }

        private void choose_dataitem(string portalfile, bool isfromportal, bool isencrypted)
        {
            csFileIo io = new csFileIo();
            irsSqlCommons com = new irsSqlCommons();
  
            string strfile = string.Empty;
            string fname = io.getFilename(portalfile);
            string targetfile = "";


            if (isencrypted)
            {
              ////  strfile = com.getAppSetting("prisUploadDir") + @"\PRIS_ENCRYPTED\" + fname;
            }
            else
            {
             ////   strfile = com.getAppSetting("prisUploadDir") + @"\PRIS_UNENCRYPTED\" + fname;
            }

            targetfile = getOutputPath(strfile);
            io.copy(strfile, targetfile, true);

            targetfile = add_backslash(targetfile);

             new csControl().execScript(this.Page, "FILE_COPIED_", "file_work('" + targetfile + "');");

            io = null;
            com = null;

        }
        /*
                private void decrypt_pris(string path)
                {
                    csFileIo io = new csFileIo();
                    pyrl_decryptor dec = new pyrl_decryptor();

                    try
                    {
                        if (io != null)
                        {
                            if (io.exist(path) == true)
                            {
                                string decrypted_txt = dec.Decrypt(io.readAllText(path));

                                io.delete(path);

                                if (io.open_output(path, (int)csFileIo.write_modes.CREATE_NEW) == true)
                                {
                                    io.putLine(decrypted_txt);
                                    io.close_output();
                                }
                            }
                        }
                    }
                    catch { }
                    finally
                    {
                        if (io != null) { io = null; }
                        if (dec != null) { dec = null; }
                    }
                }
          */
        #endregion

        protected void btn_post_Click(object sender, EventArgs e)
        {
            /* DO NOT DELETE THIS SEVER SIDE METHOD EVENTHOUGH YOU CANT SEE ANY CODE RELATED ON IT. THIS IS TRIGGERED BY JS EVENT*/
            /* ACTUAL CODE IS TRIGGERED THROUGH PAGE POST BACK */
        }

        #region "Count PUNLA Summary"

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string do_load_payroll(string path)
        {
            csFileIo fio = new csFileIo();
            splitter_ui_code pyrl = new splitter_ui_code();
            string msg = string.Empty;

            try
            {
                msg = this.get_validation_result(path);
   
                if (msg != string.Empty)
                {
                    if (msg.Trim().Length > 0)
                    {
                        throw new Exception(msg);
                    }
                }
                return pyrl.PUNLA_summary(path);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                pyrl = null;
            }
        }

        private string get_validation_result(string path)
        {
            // this.decrypt_pris(path);

            string s = new splitter_ui_code().validate_input_file(path);

            if (s.ToUpper().IndexOf("INVALID") > 0)
            {
                //this.pnlUpload.Visible = false;
            }
            return s;
        }
        #endregion

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string do_split_file(string path)
        {
            csFileIo fio = new csFileIo();
            splitter_ui_code pyrl = new splitter_ui_code();
            string msg = string.Empty;
            try
            {
                msg = this.get_validation_result(path);

                if (msg != string.Empty)
                {
                    if (msg.Trim().Length > 0)
                    {
                        throw new Exception(msg);
                    }
                }
                return pyrl.PUNLA_split_file(path);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                pyrl = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string cancel_loading(string spath)
        {
            csFileIo io = null;
            try
            {
                io = new csFileIo();
                io.delete(spath);
                return "FILE CANCELLED ...";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                io = null;
            }

        }

    }
}
