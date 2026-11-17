using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using IRSClassLib;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;

namespace IRSWorld.BranchSetup
{
    public partial class ParamApproval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ParamApproval));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getrolecode()
        {
            //=======================================================================
            //RFC-31620 : IAMD Maker-Checker
            //1 - processor, 2-approver
            irsSqlCommonsWs sql = new irsSqlCommonsWs(true);
            DataTable dt = new DataTable();
            string userID = "";

            try
            {
                string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();

                Session["loginrolecode"] = "";
                userID = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);

                //string role = new irsInternetPage().getLookupValue("User Roles", "FldValue", roleid, "FldDescription");
                //if (role.Equals("Parameter Administrator"))
                //{
                //    Session["loginrolecode"] = "1";
                //}
                //else if (role.Equals("Parameter Administrator Approver"))
                //{
                //    Session["loginrolecode"] = "2";
                //}
                //else
                //{
                //    Session["loginrolecode"] = "0";
                //}

                dt = sql.execStoredProcedureTable("proc_get_param_role", new object[] { roleid });

                if (dt != null && dt.Rows.Count > 0)
                {
                    Session["loginrolecode"] = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(userID, "ParamApproval", "Method: getrolecode", "", "ERROR: " + ex.Message, csAuditLog.Category.Select);
            }
            finally
            {
                sql = null;
            }

            return Convert.ToString(Session["loginrolecode"]);
            //=======================================================================
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> getStatusList(string currStatus)
        {
            //LogToFile = new csAuditLog();
            irsSqlCommonsWs sql = new irsSqlCommonsWs(true);
            try
            {
                //=======================================================================
                //RFC-31620 : IAMD Maker-Checker
                //return sql.getDataSet("sp_paramApproval_getStatusList", true, new string[] { currStatus }).Tables[0].AsEnumerable().ToList();

                string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
                return sql.getDataSet("sp_paramApproval_getStatusList", true, new string[] { currStatus, roleid }).Tables[0].AsEnumerable().ToList();
                //=======================================================================
            }
            catch (Exception ex)
            {
                //LogToFile.createLogs("getEnrolleeList|" + ex.Message);
                //try
                //{
                //    new csAuditLog().writeAudit("", "enrollment", "Method: getEnrolleeList", "Current Session: " + new de().getCurrentSessionID(), "ERROR: " + ex.Message, csAuditLog.Category.Select);
                //}
                //catch { }
                return null;
            }
            finally
            {
                //LogToFile = null;
                sql = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> getStatusListDet(string seqid, string menu, string isview)
        {
            string userID = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);
            irsSqlCommonsWs sql = new irsSqlCommonsWs(true);
            try
            {
                //=======================================================================
                //RFC-31620 : IAMD Maker-Checker
                //return sql.getDataSet("sp_paramApproval_getStatusListDet", true, new string[] { seqid, menu, isview }).Tables[0].AsEnumerable().ToList();

                string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
                return sql.getDataSet("sp_paramApproval_getStatusListDet", true, new string[] { seqid, menu, isview, roleid }).Tables[0].AsEnumerable().ToList();
                //=======================================================================
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(userID, "ParamApproval", "Method: getStatusListDet", "", "ERROR: " + ex.Message, csAuditLog.Category.Select);
                return null;
            }
            finally
            {
                //LogToFile = null;
                sql = null;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string updateStatus(string seqid, string status, string menuName)
        {
            string userID = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);

            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                if (commons != null)
                {
                    //=======================================================================
                    //RFC-31620 : IAMD Maker-Checker
                    //int res = commons.execStoredProcedureUpdate("sp_paramApproval_updStat", new Object[] { seqid, status, menuName, userID });
                    //DataTable dt = commons.execStoredProcedureTable("sp_paramApproval_updStat", new Object[] { seqid, status, menuName, userID });

                    string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
                    DataTable dt = commons.execStoredProcedureTable("sp_paramApproval_updStat", new Object[] { seqid, status, menuName, userID, roleid });
                    //=======================================================================

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0][0].ToString();
                    }
                    //if (res == -1)
                    //{
                    //    throw new Exception(commons.Message);
                    //}
                    //else
                    //{
                    //    return true;
                    //}
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(userID, "ParamApproval", "Method: updateStatus", "", "ERROR: " + ex.Message, csAuditLog.Category.Select);
            }
            finally
            {
                commons = null;
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string GenerateAuditLog(string dfr, string dto)
        {
            string userID = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);
            string pdfOutFolder = new irsConfiguration().ConnectionStringNoRunMode("ReportExportPath");
            pdfOutFolder = Path.Combine(pdfOutFolder, "ParamAuditLog");
            string pdffilename = DateTime.Now.ToString("yyyyMMddmmss") + ".pdf";

            //check if output folder exists
            if (!Directory.Exists(pdfOutFolder))
            {
                Directory.CreateDirectory(pdfOutFolder);
            }

            //delete all files in out directory
            try
            {
                foreach (string fi in Directory.GetFiles(pdfOutFolder))
                {
                    File.Delete(fi);
                }

            }
            catch { }

            //generate pdf file
            string finalpdf = Path.Combine(pdfOutFolder, pdffilename);
            try
            {
                DataTable dt = new DataTable();
                irsSqlCommonsWs sql = new irsSqlCommonsWs(true);

                //=======================================================================
                //RFC-31620 : IAMD Maker-Checker
                //dt = sql.getDataSet("sp_paramApproval_generate_AuditReport", true, new string[] { dfr, dto }).Tables[0];

                string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
                dt = sql.getDataSet("sp_paramApproval_generate_AuditReport", true, new string[] { dfr, dto, roleid }).Tables[0];
                //=======================================================================

                if (dt.Rows.Count > 0)
                {
                    ReportDocument repDoc = new ReportDocument();
                    repDoc.Load(Server.MapPath("~/Reports/Parameter/rptParamAuditLog.rpt"));
                    repDoc.SetDataSource(dt);
                    repDoc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, finalpdf);
                    System.Threading.Thread.Sleep(2000);
                }
                else
                {
                    return "No data to extract, try changing your parameter";
                }

            }
            catch(Exception ex)
            {
                new csAuditLog().writeAudit(userID, "ParamApproval", "Method: GenerateAuditLog", "", "ERROR: " + ex.Message, csAuditLog.Category.Select);
                return "ERROR | " +  ex.Message;
            }
            new csAuditLog().writeAudit(userID, "ParamApproval", "Method: GenerateAuditLog", "", "Report Generated Successfully | Parameter: DateFrom=" + dfr + " DateTo=" + dto, csAuditLog.Category.Select);
            return pdffilename;
        }

        //=======================================================================
        //RFC-31620 : IAMD Maker-Checker
        private irsUsers IRSUsers
        {
            get
            {
                if (Session["IRS_USERS"] == null)
                {
                    if (Session["IRS_CLIENT_DATA"] == null) throw new Exception("Session Expired.");
                    Session["IRS_USERS"] = new irsUsers();
                }
                return (irsUsers)Session["IRS_USERS"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_user_details(string tmp_userid, string tmp_menuname)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataTable dt = new DataTable();
            string retval = "";

            string userID = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);

            try {
                string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
                dt = commons.execStoredProcedureTable("proc_get_userstable_approval_details", new object[] { tmp_userid, tmp_menuname, roleid });

                if (dt != null && dt.Rows.Count > 0)
                {
                    retval = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex) {
                new csAuditLog().writeAudit(userID, "ParamApproval", "Method: get_user_details", "", "ERROR: " + ex.Message, csAuditLog.Category.Select);
            }
            finally {
                commons = null;
            }
            return retval;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String send_email(String userid, String emailad, String email_op)
        {
            string userID = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);

            if (!IRSUsers.email_password(userid, emailad, email_op))
            {
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger(userID, "ParamApproval", "EMAIL OF PASSWORD SENDING FAILED", "UserID=[" + userid + "] EmailAddress=[" + emailad + "]", IRSUsers.Message, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                throw new Exception(IRSUsers.Message);
            }
            else
            {
                /* VAPT 20180222 */
                new csAuditLog().writeVAPTLogger(userID, "ParamApproval", "PASSWORD SUCCESSFULLY SENT TO USER", "UserID=[" + userid + "] EmailAddress=[" + emailad + "]", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
            }
            return emailad;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String disclose_password(String userid, String discloser_password)
        {
            String s = IRSUsers.disclose_password(userid, discloser_password);
            if (s == null)
            {
                throw new Exception(IRSUsers.Message);
            }

            return s;
        }
        //=======================================================================
    }
}