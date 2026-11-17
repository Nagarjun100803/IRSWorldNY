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
namespace IRSWorld.IRSFileLoader
{
    public partial class FEDACHViewer : System.Web.UI.Page
    {
        csAuditLog log = new csAuditLog();

        public string ErrorMessage = "";

        public string ExtractFileName = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
            if (!Page.IsPostBack)
            {
                this.hidBranch.Value = IRS_CLIENT_DATA().Location;

                AjaxPro.Utility.RegisterTypeForAjax(typeof(FEDACHViewer));
            }
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

        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string create_fed_file(string file_name, int report_type)
        {

            csAuditLog log = new csAuditLog();
            try
            {
                if (this.create_text_file(file_name, report_type))
                {
                    return this.ExtractFileName;
                }                
                return this.ErrorMessage;
            }
            catch(Exception e)
            {
                log.writeAudit(IRS_CLIENT_DATA().UserId, "FEDWATCHLIST", "create_fed_file", file_name, e.StackTrace, csAuditLog.Category.File);
                return "ERROR: " + e.StackTrace ;
            }
            finally
            {
                log = null;   
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int check_watchlist_loading()
        {

            csAuditLog log = new csAuditLog();
            irsFedAchWatchlist fedviewer = new irsFedAchWatchlist();
            try
            {
                return fedviewer.Check_Ongoing_Watchlist(IRS_CLIENT_DATA().Location);
            }
            catch (Exception e)
            {
                log.writeAudit(IRS_CLIENT_DATA().UserId, "FEDWATCHLIST", "check_watchlist_loading", "", e.StackTrace, csAuditLog.Category.File);
                return 3;
            }
            finally
            {
                log = null; fedviewer = null;
            }
        }
        #endregion

        private bool create_text_file(string fed_filename, int report_type_value)
        {
            csFileIo io = new csFileIo();
            csAuditLog log = new csAuditLog();
            irsSqlCommons com = new irsSqlCommons();
            irsConfiguration conf = new irsConfiguration();
            DataSet ds = new DataSet();
            irsFedAchWatchlist fedviewer = new irsFedAchWatchlist();
            string folderpath = conf.appsetting("FedAchExtractPath");
            string prefixname = conf.appsetting("FedPrefixFileName");
            string report_type_name = "";

            try
            {
                ExtractFileName = "";
                log.writeAudit(IRS_CLIENT_DATA().UserId, "FEDWATCHLIST", "CREATING FILE", fed_filename, "", csAuditLog.Category.File);

                if (this.create_folder())
                {
                    if (report_type_value == 1)
                    {
                        report_type_name = "Regular";
                    }
                    else if (report_type_value == 2)
                    {
                        report_type_name = "Alias";
                    }
                    else if (report_type_value == 3)
                    {
                        report_type_name = "Exception";
                    }

                    ds = fedviewer.Get_FedAch_CIF_to_Match(fed_filename, IRS_CLIENT_DATA().Location, IRS_CLIENT_DATA().UserId, report_type_value);
                    string new_extension = prefixname + io.getFilenameNoExt(fed_filename) + "_" + report_type_name + ".csv";
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {
                            string fullpath = folderpath + "\\" +  new_extension;
                            if (io.exist(fullpath))
                            {
                                io.delete(fullpath);
                            }

                            io.open_output(fullpath, (int)csFileIo.write_modes.CREATE_NEW);

                            ExtractFileName = new_extension; 
                            
                            string colums = conf.appsetting("FedColumns");

                            io.putLine(colums);

                            string data = "";
                            foreach (DataRow r in ds.Tables[0].Rows)
                            {

                                data = "'" + r["FED_REFNO"].ToString() + "," +
                                       r["FEDACH_DATE"].ToString() + "," +
                                       r["REMITTER_NAME"].ToString() + "," +
                                       r["BENEFICIARY_NAME"].ToString() + "," +
                                       "'" + r["BENEF_ACCOUNTNO"].ToString() + "," +
                                       r["SOURCE_CURRENCY"].ToString() + "," +
                                       r["SOURCE_AMOUNT"].ToString() + "," +
                                       r["FEDACH_MATCHED_DATA"].ToString() + "," +
                                       r["SDN_FINCEN_DATA"].ToString() + "," +
                                       r["SDN_FINCEN_REFNO"].ToString() + "," +
                                       r["SDN_FINCEN_NAME"].ToString() + ","+
                                        r["FILEFORMAT"].ToString() + ","+
                                        r["ALIAS_LASTNAME"].ToString() + "," +
                                        r["ALIAS_FIRSTNAME"].ToString() + "," +
                                        r["ALIAS_MIDDLENAME"].ToString();

                                io.putLine(data);
                            }
                            io.close_output();
                            io = null;

                            return true;
                        }
                    }
                }
                return false;

            }
            catch (Exception e)
            {
                log.writeAudit(IRS_CLIENT_DATA().UserId, "FEDWATCHLIST", "ERROR CREATING FILE", fed_filename, "", csAuditLog.Category.File);
                return false;
            }
            finally { io = null; ds = null; fedviewer = null; }
        }

        private bool create_folder()
        {
            irsConfiguration conf = new irsConfiguration();
            csAuditLog log = new csAuditLog();
            string folderpath = conf.appsetting("FedAchExtractPath");
            
            try
            {
                if (!System.IO.Directory.Exists(folderpath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(folderpath);
                    }
                    catch (Exception e) { log.writeAudit(IRS_CLIENT_DATA().UserId, "FEDWATCHLIST", "ERROR CREATING FOLDER", folderpath, "", csAuditLog.Category.File); return false; }
                }


                return true;
            }            
            finally { log = null; conf = null; }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable Get_File_List(string datefrom, string dateto)
        {
            irsFedAchWatchlist fedviewer = new irsFedAchWatchlist();
            string branchcode = IRS_CLIENT_DATA().Location;
            string userid = IRS_CLIENT_DATA().UserId;

            DataSet ds = fedviewer.Get_File_List(datefrom, dateto, branchcode, userid);
            return ds.Tables[0];
        }
        
    }
}
