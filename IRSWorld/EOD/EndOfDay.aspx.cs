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
using CrystalDecisions.CrystalReports.Engine;
using IRSWorld.TranClass;
namespace IRSWorld.EOD
{
    public partial class EndOfDay : System.Web.UI.Page
    {
        private irsEndOfDay eod = new irsEndOfDay();
        private irsClientData IRSClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }
                return null;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EndOfDay));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            irsTimeZone it = new irsTimeZone();
            csAuditLog audit = new csAuditLog();

            try
            {
                audit.writeAudit(IRSClientData.UserId, "EOD", "EodDayLogs", "INITIALIZE", "", csAuditLog.Category.Select);
            }
            catch { }

            try
            {
                if (IRSClientData != null)
                {
                    this.hdBranchCode.Value = IRSClientData.Location;
                    this.hdUserID.Value = IRSClientData.UserId;
                    this.hdTranxDate.Value = it.branchLocalTime(IRSClientData.Location).ToString();
                }
                else
                {
                    audit.writeAudit(this.hdUserID.Value, "EOD", "EodDayError", "EXPIRED SESSION", "", csAuditLog.Category.Select);
                    ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "eval(spawn_errpage('SESSION EXPIRED'))", true);
                }
            }

            catch (Exception em)
            {
                try
                {
                    audit.writeAudit(IRSClientData.UserId, "EOD", "EodDayError", em.Message, em.StackTrace, csAuditLog.Category.Select);
                }
                catch { }
                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "eval(spawn_errpage('SESSION EXPIRED'))", true);
            }
            finally
            {
                try
                {
                    audit.writeAudit(IRSClientData.UserId, "EOD", "EodDayLogs", "ENDDED", "", csAuditLog.Category.Select);
                }
                catch { }
            }
        }
      
       

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String exec_forcepush()
        {
            irsReport_LookupUI ir = new irsReport_LookupUI();
            csAuditLog ca = new csAuditLog();
            try
            {
                string BranchCode = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
                string strTranxdate = new irsTimeZone().branchLocalTime(BranchCode).ToString("MM/dd/yyyy");
                if (ir.exec_forcepush(strTranxdate, BranchCode))
                {
                    return "";
                }
                return "ERROR:CANNOT CONTINUE FORCE PUSH.";


            }
            catch (Exception e)
            {
                try
                {
                    ca.writeAudit(this.hdUserID.Value, "EOD", "Report Viewer", e.StackTrace, e.Message, csAuditLog.Category.Select);
                }
                catch { }

                return "ERROR:" + e.Message;
            }
            finally
            {
                if (ir != null) ir = null;
                if (ca != null) ca = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_transaction_date(string branchcode)
        {
            irsTimeZone it = new irsTimeZone();
            try
            {
                return Convert.ToDateTime(it.branchLocalTime(branchcode)).ToString();
            }
            catch
            {
                return "INVALID BRANCH SETUP: TIMEZONE IS NOT CONFIGURED FOR THIS BRANCH";
            }
            finally
            {
                if (it != null) it = null;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet chk_eod_items(string branchcode)
        {
            /* VAPT 20180222 */
            new csAuditLog().writeVAPTLogger("", "EOD", "USER HAS EXECUTED EOD PROCESS", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);

            csAuditLog ca = new csAuditLog();
            irsTimeZone it = new irsTimeZone();
            string connstring = new irsConfiguration().ConnectionString;
            string[] arr_conn_details = connstring.Split(';');
            string[] server = arr_conn_details[0].Split('=');
            string[] database = arr_conn_details[2].Split('=');
            string[] userid = arr_conn_details[3].Split('=');
            string[] password = arr_conn_details[4].Split('=');
            irsEndOfDay eod = new irsEndOfDay(userid[1], password[1], server[1], database[1]);

            string sp_name = "proc_chk_itemsforeod";

            string tranxdate =it.branchLocalTime(branchcode).ToString("MM/dd/yyyy");
            string sp_params = tranxdate + ',' + branchcode ;

            try
            {
                if (IRSClientData != null)
                    ca.writeAudit(IRSClientData.UserId, "EOD", "EndofDayLogs", "Check_eod_items", sp_params, csAuditLog.Category.Select);
            
                return eod.execStoredProc(sp_name, sp_params.Split(','));
            }
            catch(Exception er)
            {
                if (IRSClientData != null)
                    ca.writeAudit(IRSClientData.UserId, "EOD", "EndofDayError", er.StackTrace, er.Message, csAuditLog.Category.Select);

                return null;
            }
            finally
            {
                if (eod != null) eod = null;
                if (it != null) it = null;
                if (ca != null) ca = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_report(string tranxdate)
        {
            string s = "EOD:" + tranxdate;
            HttpContext.Current.Session["IRS_ReportsUI"] = null;
            HttpContext.Current.Session["IRS_ReportsUI"] = "EOD:" + Convert.ToDateTime(tranxdate).ToString("MM/dd/yyyy");
            return s;
            
        }
        public bool update_trnx()
        {
            Object[] o = new Object[3];
            o[0] = "";
            o[1] = "";
            o[2] = "";
            return true;
        }

        private void set_handle_report(string _merge, string _params, string _fixedcode)
        {

            irsReports rep = new irsReports(_merge, _params, "false");

            string arr1 = rep.rep_values[0].ToString();
            System.Web.HttpContext.Current.Session["IRS_ReportsUI"] = null;
            System.Web.HttpContext.Current.Session["IRS_ReportsUI"] = _fixedcode + ":" + arr1;

            rep = null;
        }
        
    }
}
