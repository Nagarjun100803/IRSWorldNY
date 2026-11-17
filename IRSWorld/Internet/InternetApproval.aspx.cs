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
using IRSClassLib;
using IRSWorld.TranClass;

namespace IRSWorld.Internet
{
    public partial class InternetApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetApproval));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetRemitter));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranGFMCCommons));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        protected void buttonDL_Click(object sender, EventArgs e)
        {
            String res = "";
            String filename = this.HPath.Value;
            try
            {
                String msg = String.Empty;
                if (!download_File(filename, out msg))
                {
                    throw new Exception(msg);
                }
            }
            catch
            {

            }
        }

        public bool download_File(String Path, out String msg)
        {
            msg = String.Empty;
            try
            {
                csFileIo fio = new csFileIo();
                String fname = fio.getFilename(Path);

                HttpResponse response = Context.Response;

                if (fio.exist(Path))
                {
                    response.ContentType = "application/zip";
                    response.AddHeader("content-Disposition", "attachment;filename=" + fname);
                    response.WriteFile(Path);
                    response.Flush();
                    response.Close();

                    //fio.delete(Path);

                    return true;
                }
                else { msg = "Error file: " + fname + " does not exist."; }
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getSupportingDocs(String seq_id)
        {
            get_supporting_docs_from_ftp(seq_id);

            csFileIo fio = new csFileIo();
            String dir = new irsConfiguration().appsetting("temp_dir").ToString() + @"\INTERNETCLIENT\" + seq_id;
            if (fio.directoryExist(dir))
            {
                String[] fileList = fio.listFiles(dir);
                DataSet ds = new DataSet();

                DataTable dt = new DataTable();

                dt.Columns.Add("FldValue");
                dt.Columns.Add("FldDescription");

                foreach (String f in fileList)
                {
                    dt.Rows.Add(new Object[] { f, fio.getFilename(f) });
                }

                ds.Tables.Add(dt);

                return ds;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void delete_temp_files(String seq_id)
        {
            try
            {
                csFileIo fio = new csFileIo();

                String DropDir = new irsConfiguration().appsetting("temp_dir").ToString() + @"\INTERNETCLIENT\" + seq_id;

                if (fio.directoryExist(DropDir))
                {
                    String[] filelist = fio.listFiles(DropDir);
                    foreach (String f in filelist)
                    {
                        fio.delete(f);
                    }
                }
            }
            catch { }
        }

        private bool get_supporting_docs_from_ftp(String seq_id)
        {
            try
            {
                csFileIo fio = new csFileIo();

                String DropDir = "";
                String ftpConString = "";
                String ftpRootPath = "";
                //CHR - ITG Password encrypt
                //String RunMode = "";
                String[] filelist = null;
                String FileTag = "IRSD_" + seq_id; //stands for "Internet Remitter Supporting Documents"

                try
                {
                    //CHR - ITG Password encrypt
                    //RunMode = new irsConfiguration().appsetting("RunMode").ToString();
                    //ftpConString = new irsConfiguration().appsetting(RunMode + "FTPConnectionString").ToString();
                    ftpConString = new irsConfiguration().FTPConnectionString;
                    DropDir = new irsConfiguration().appsetting("temp_dir").ToString() + @"INTERNETCLIENT\" + seq_id;
                    ftpRootPath = new irsConfiguration().appsetting("RootPath").ToString() + @"INTERNETCLIENT\";

                    if (!fio.directoryExist(DropDir))
                    {
                        fio.createDirectory(DropDir);
                    }
                }
                catch { throw new Exception("Incomplete config info"); }

                csFtp ftp = new csFtp(ftpConString);
                if (ftp.connect())
                {

                    bool remoteDirAvailable = false;

                    if (!ftp.cwd(ftpRootPath))
                    {
                        if (ftp.mkdir(ftpRootPath))
                        {
                            if (ftp.cwd(ftpRootPath))
                            {
                                remoteDirAvailable = true;
                            }
                        }
                    }
                    else
                    {
                        remoteDirAvailable = true;
                    }

                    if (remoteDirAvailable)
                    {
                        filelist = ftp.nls(FileTag + "_*");
                        ftp.delay = 20;
                        ftp.binary(true);
                        foreach (String f in filelist)
                        {
                            //ftp.get(f, DropDir + @"\" + f.Replace(FileTag + "_", ""));
                            ftp.get(f, DropDir + @"\" + f);
                        }
                    }

                    ftp.disconnect();
                }
            }
            catch
            { }
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void mailer(String seq_id)
        {
            try
            {
                csMailer mail = new csMailer();
                DataSet ds = get_client_details(seq_id);
                String Subject = "Account Creation for Internet Client : " + ds.Tables[0].Rows[0]["FirstName"].ToString().ToUpper() + " " + ds.Tables[0].Rows[0]["MiddleName"].ToString().ToUpper() + " " + ds.Tables[0].Rows[0]["LastName"].ToString().ToUpper();
                mail.sendMail(Subject, getRecipients(), createMsg(ds));
            }
            catch { }
        }

        private String createMsg(DataSet ds)
        {
            String msg = "";
            DataRow dr = ds.Tables[0].Rows[0];
            msg += "CIFID           : " + dr["CIFID"].ToString().ToUpper() + "\n";
            msg += "First Name      : " + dr["FirstName"].ToString().ToUpper() + "\n";
            msg += "Last Name       : " + dr["LastName"].ToString().ToUpper() + "\n";
            msg += "Middle Name     : " + dr["MiddleName"].ToString().ToUpper() + "\n";
            msg += "Email Address   : " + dr["Email"].ToString() + "\n";
            msg += "Preferred UserID: " + dr["UserID"].ToString() + "\n";
            msg += "Branch Code     : " + dr["BranchCode"].ToString().ToUpper() + "\n\n";

            return msg;
        }

        private String[] getRecipients()
        {
            sqlHelperWs commons = new sqlHelperWs();

            try
            {
                String sql = "select * from tbl_lookup where fldgroup = 'InternetClientApproverMails'";
                DataSet ds = commons.getDataSet(commons.getSqlCommand(sql, false));

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    String RList = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        RList += ds.Tables[0].Rows[i]["FldDescription"].ToString() + "~";
                    }

                    return RList.Substring(0, RList.Length - 1).Split('~');
                }
            }
            catch { }
            finally
            {
                commons = null;
            }
            return null;
        }

        private DataSet get_client_details(string seq_id)
        {
            sqlHelperWs commons = new sqlHelperWs();

            try
            {
                String sql = "select * from tbl_internet_remitter where seq_id = " + seq_id;
                return commons.getDataSet(commons.getSqlCommand(sql, false));
            }
            catch
            {
                return null;
            }
            finally
            {
                commons = null;
            }
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public String create_response_file(String FileName)
        //{
        //    try
        //    {
        //        String msg = "";
        //        if (new irsEXCOAlRajhiResponse().create_response_file(FileName, out msg))
        //        {
        //            return msg;
        //        }
        //        else
        //        {
        //            throw new Exception(msg);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to create file, Reason : " + ex.Message);
        //    }
        //}

    }




}
