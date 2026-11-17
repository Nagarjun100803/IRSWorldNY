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
using System.Data.SqlClient ;

namespace IRSWorld.FED
{   
    public partial class FEDACHStatusUpdater : System.Web.UI.Page
    {
        public String fname = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(FEDACHStatusUpdater));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_transactions(String fedrefno,string datefrom ,string dateto,string status)
        {
            sqlHelperWs sql = new sqlHelperWs();

            //update_status_table(FileName);
            string branchcode = IRS_CLIENT_DATA().Location;
            DataSet ds = sql.execStoredProcedure("proc_select_fed_transactions", new Object[] { fedrefno, Convert.ToDateTime(datefrom),Convert.ToDateTime(dateto), status,branchcode });
            return ds.Tables[0];
        }

      

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_status(String param)
        {
            String[] paramlist = param.Split('|');
            String errmsg = "";
            String errlist = "";
            foreach (String p in paramlist)
            {
                if (p != null && p != "")
                {
                    String[] p1 = p.Split('~');
                    if (!update(p1[0].ToString(), p1[1].ToString(),p1[2].ToString()
                        , p1[3].ToString(),p1[4].ToString(), out errmsg))
                    {
                        errlist = errlist + p1[2] + " : " + errmsg + "\n";
                    }
                }
            }

            if (errlist != null && errlist != "")
            {
                throw new Exception("Failed to update status, please see details below\n\n" + errlist);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_records(string rem_seq_id, string refno, string Status, string AccountNo, string BeneName)
        {
            string errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                string userid = IRS_CLIENT_DATA().UserId;
                this.writeAudit("STATUS BEFORE UPDATE:" + userid + "~" + Status + "~" + rem_seq_id + "~" + AccountNo + "~" + BeneName, csAuditLog.Category.Update);

                irsTimeZone timezone = new irsTimeZone();
                int i = commons.execStoredProcedureUpdate("proc_update_fed_status", new Object[] {
                                    IRS_CLIENT_DATA().Location,
                                    rem_seq_id, refno, Status ,userid
                                    ,timezone.branchLocalTime(IRS_CLIENT_DATA().Location).ToString()
                                    ,AccountNo
                                    ,BeneName
                                    });

                if (i < 1)
                {
                    errmsg = "Failed to update record. Please verify status of the transaction. ";

                    if (commons.Message != null && commons.Message != "")
                    {
                        throw new Exception(commons.Message);
                    }

                    throw new Exception("Record not found.");
                }
                else
                {
                    this.writeAudit("STATUS AFTER UPDATE:" + userid + "~" + Status + "~" + rem_seq_id + "~" + AccountNo + "~" + BeneName, csAuditLog.Category.Update);
                }
            }
            catch (Exception ex)
            {
                errmsg = "Failed to update record. Please verify status of the transaction. " + (ex.Message);
            }
            finally
            {
                if (commons != null) { commons = null; }
                this.writeAudit("STATUS REACHED", csAuditLog.Category.Update);
            }

            return errmsg;
        }

        private bool update(String rem_seq_id, String refno, String Status, String AccountNo,String BeneName,out String errmsg)
        {
            errmsg = "";
            sqlHelperWs commons = new sqlHelperWs();
            try
            {
                string userid = IRS_CLIENT_DATA().UserId;
                this.writeAudit("STATUS BEFORE UPDATE:" + userid + "~" + Status + "~" + rem_seq_id + "~" + AccountNo + "~" + BeneName, csAuditLog.Category.Update);

                irsTimeZone timezone = new irsTimeZone();
                int i = commons.execStoredProcedureUpdate("proc_update_fed_status", new Object[] { 
                                    IRS_CLIENT_DATA().Location,
                                    rem_seq_id, refno, Status ,userid
                                    ,timezone.branchLocalTime(IRS_CLIENT_DATA().Location).ToString()
                                    ,AccountNo
                                    ,BeneName
                                    });

                if (i < 1)
                {
                    errmsg = "no row updated";

                    if (commons.Message != null && commons.Message != "")
                    {
                        throw new Exception(commons.Message);
                    }

                    throw new Exception("Record not found");
                }
                else
                {
                    this.writeAudit("STATUS AFTER UPDATE:" + userid + "~" + Status + "~" + rem_seq_id + "~" + AccountNo + "~" + BeneName, csAuditLog.Category.Update);
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                errmsg = "no row updated" + (ex.Message);
            }
            finally
            {
                if (commons != null) { commons = null; }
                this.writeAudit("STATUS REACHED",csAuditLog.Category.Update);
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_status()
        {
            
            sqlHelperWs sql = new sqlHelperWs();
            DataSet ds = sql.execStoredProcedure("proc_get_fed_status", new Object[] { IRS_CLIENT_DATA().Location });
            return ds.Tables[0];
        }            
        
        #region PRIVATE METHOD
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
        private void writeAudit(string Description, csAuditLog.Category Category)
        {
            csAuditLog csa = new csAuditLog();
            try
            {
                if (IRS_CLIENT_DATA().UserId != null)
                {
                    csa.writeAudit(IRS_CLIENT_DATA().UserId, "FEDACH STATUS UPDATER", Description, "", "", Category);
                }
                else
                {
                    csa.writeAudit("FEDSTATUPDATER", "FEDACH STATUS UPDATER", Description, "", "", Category);
                }
            }
            finally { csa = null; }
        }
        #endregion

    }
        
 
}
