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

namespace IRSWorld.BranchSetup
{
    public partial class BranchSetup : System.Web.UI.Page
    {

        private irsBranchSetup IRSBranchSetup
        {
            get
            {
                if (Session["IRS_BRANCH_SETUP"] == null)
                {
                    Session["IRS_BRANCH_SETUP"] = new irsBranchSetup();
                }
                return (irsBranchSetup)Session["IRS_BRANCH_SETUP"];
            }
        }

        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }

        private bool isAuthorized(String branch)
        {
            bool isSysad = IRSClientData.isSystemAdmin;

            if (isSysad) return true;

            bool isBrad = IRSClientData.isBranchAdmin;
            if (isBrad)
            {
                if (branch == null || branch == String.Empty) return true;
                return (IRSClientData.Location.Equals(branch));
            }

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BranchSetup));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        /* RFC-31620 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAdminApproverTag()
        {
            string ret = string.Empty;
            if (Session["AdminApprovalFlagBranchSetupMessage"] != null)
            {
                if (new irsUsers().adminApproverSwitch())
                {
                    return Session["AdminApprovalFlagBranchSetupMessage"].ToString();
                }
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_brsetup()
        {
            Session["IRS_BRANCH_SETUP"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchSetup(String brcode)
        {
            DataTable dtsetup = IRSBranchSetup.getbranchsetup(brcode);
            if (dtsetup == null) throw new Exception(IRSBranchSetup.Message);

            return dtsetup;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getBranches(String x)
        {
            String brcode = IRSClientData.Location;
            String errmsg = "";
            Object[] ds = IRSBranchSetup.getInitData(out errmsg);
            if (errmsg.Length > 0)
            {
                throw new Exception(errmsg);
            }

            DataTable dtsetup = getBranchSetup(brcode);

            return new Object[] 
            { 
                brcode, 
                ds[0], // ddSetupBranch
                ds[1], // ddcommsetup
                ds[2], // ddreqid
                dtsetup, 
                /* RFC-31620 */
                true //IRSClientData.isSystemAdmin 
            };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object delete_setup(int seqid, String brcode)
        {
            /* RFC-31620 */
            Session["AdminApprovalFlagBranchSetupMessage"] = null;
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("ACTIONTYPE");
            dt.Columns.Add("Main_SEQ_ID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("CutOffStart");
            dt.Columns.Add("CutOffEnd");
            dt.Columns.Add("StalePeriod");
            dt.Columns.Add("CommSetup");
            dt.Columns.Add("RemittanceAmtLimit");
            dt.Columns.Add("AggregateAmtLimit");
            dt.Columns.Add("SupervisorLimit");
            dt.Columns.Add("TellerLimit");
            dt.Columns.Add("ARMLimit");
            dt.Columns.Add("InetLimit");
            dt.Columns.Add("RequiredID");
            dt.Columns.Add("FxDeviation");
            dt.Columns.Add("TDD_StImp_Min");
            dt.Columns.Add("TDD_StImp_Max");
            dt.Columns.Add("TDD_FirstTimeStImp_Min");
            dt.Columns.Add("TDD_FirstTimeStImp_Max");
            dt.Columns.Add("CashAggregationLimit");
            dt.Columns.Add("CashAggregationAmtLimit");
            dt.Columns.Add("AgentSysControl");
            dt.Columns.Add("AgentLogReport");
            dt.Columns.Add("ThreeDayAggregation");
            dt.Columns.Add("ThirtyDayAggregation");
            dt.Columns.Add("AgentAggAmtReject");
            dt.Columns.Add("DailyLimit");
            dt.Columns.Add("AgentCheckIDAmount");

            if (new irsUsers().adminApproverSwitch())
            {
                DataTable dtSetup = new irsUsers().getBranchSetup(brcode);
                dr = dt.NewRow();
                dr[0] = "DELETE";
                dr[1] = seqid;
                dr[2] = brcode;

                if (dtSetup != null && dtSetup.Rows.Count > 0)
                {
                    dr[3] = new irsUsers().CleanDataTable(dt, "CutOffStart");
                    dr[4] = new irsUsers().CleanDataTable(dt, "StalePeriod");
                    dr[5] = new irsUsers().CleanDataTable(dt, "CommSetup");
                    dr[6] = new irsUsers().CleanDataTable(dt, "RemittanceAmtLimit");
                    dr[7] = new irsUsers().CleanDataTable(dt, "AggregateAmtLimit");
                    dr[8] = new irsUsers().CleanDataTable(dt, "SupervisorLimit");
                    dr[9] = new irsUsers().CleanDataTable(dt, "TellerLimit");
                    dr[10] = new irsUsers().CleanDataTable(dt, "ARMLimit");
                    dr[11] = new irsUsers().CleanDataTable(dt, "InetLimit");
                    dr[12] = new irsUsers().CleanDataTable(dt, "RequiredID");
                    dr[13] = new irsUsers().CleanDataTable(dt, "FxDeviation");
                    dr[14] = new irsUsers().CleanDataTable(dt, "TDD_StImp_Min");
                    dr[15] = new irsUsers().CleanDataTable(dt, "TDD_StImp_Max");
                    dr[16] = new irsUsers().CleanDataTable(dt, "TDD_FirstTimeStImp_Min");
                    dr[17] = new irsUsers().CleanDataTable(dt, "TDD_FirstTimeStImp_Max");
                    dr[18] = new irsUsers().CleanDataTable(dt, "CashAggregationLimit");
                    dr[19] = new irsUsers().CleanDataTable(dt, "CashAggregationAmtLimit");
                    dr[20] = new irsUsers().CleanDataTable(dt, "AgentSysControl");
                    dr[21] = new irsUsers().CleanDataTable(dt, "AgentLogReport");
                    dr[22] = new irsUsers().CleanDataTable(dt, "ThreeDayAggregation");
                    dr[23] = new irsUsers().CleanDataTable(dt, "ThirtyDayAggregation");
                    dr[24] = new irsUsers().CleanDataTable(dt, "AgentAggAmtReject");
                    dr[25] = new irsUsers().CleanDataTable(dt, "DailyLimit");
                    dr[26] = new irsUsers().CleanDataTable(dt, "AgentCheckIDAmount");
                }
                dt.Rows.Add(dr);
                if (new irsUsers().IsMakerAllowedBranchSetup(irsUsers.ActionType.DELETE, seqid.ToString()))
                {
                    throw new Exception("There is a pending approval for the same branch setup. Submission for delete to checker failed.");
                }
                new irsUsers().saveBranchSetupMakerChecker(irsUsers.ActionType.DELETE, dt);
                Session["AdminApprovalFlagBranchSetupMessage"] = "RECORD SUBMITTED FOR APPROVAL";
                return null;
            }
            else
            {
                String errmsg = "";
                bool ok = IRSBranchSetup.delete_setup(seqid, out errmsg);
                if (!ok)
                {
                    throw new Exception(errmsg);
                }

                DataTable dtBranchSetup = IRSBranchSetup.getbranchsetup(brcode);
                if (dtBranchSetup != null) return dtBranchSetup;

                throw new Exception(IRSBranchSetup.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object save_setup(String srec)
        {
            Session["AdminApprovalFlagBranchSetupMessage"] = null;
            Hashtable ht = new Hashtable();
            String[] ss = srec.Split('~');

            /* RFC-31620 */
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("ACTIONTYPE");
            dt.Columns.Add("Main_SEQ_ID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("CutOffStart");
            dt.Columns.Add("CutOffEnd");
            dt.Columns.Add("StalePeriod");
            dt.Columns.Add("CommSetup");
            dt.Columns.Add("RemittanceAmtLimit");
            dt.Columns.Add("AggregateAmtLimit");
            dt.Columns.Add("SupervisorLimit");
            dt.Columns.Add("TellerLimit");
            dt.Columns.Add("ARMLimit");
            dt.Columns.Add("InetLimit");
            dt.Columns.Add("RequiredID");
            dt.Columns.Add("FxDeviation");
            dt.Columns.Add("TDD_StImp_Min");
            dt.Columns.Add("TDD_StImp_Max");
            dt.Columns.Add("TDD_FirstTimeStImp_Min");
            dt.Columns.Add("TDD_FirstTimeStImp_Max");
            dt.Columns.Add("CashAggregationLimit");
            dt.Columns.Add("CashAggregationAmtLimit");
            dt.Columns.Add("AgentSysControl");
            dt.Columns.Add("AgentLogReport");
            dt.Columns.Add("ThreeDayAggregation");
            dt.Columns.Add("ThirtyDayAggregation");
            dt.Columns.Add("AgentAggAmtReject");
            dt.Columns.Add("DailyLimit");
            dt.Columns.Add("AgentCheckIDAmount");

            foreach (String s in ss)
            {
                String[] tt = s.Split('=');
                if (tt.Length > 1 && tt.Length < 3)
                {
                    ht.Add(tt[0], tt[1]);
                }
                else
                {
                    throw new Exception("Invalid column and value pair: " + s);
                }
            }

            if (ht.Count > 0 & ht.ContainsKey("crud"))
            {
                /* RFC-31620 */
                dr = dt.NewRow();
                if (ht["crud"].ToString().ToUpper() == "EDIT")
                {
                    dr[0] = "EDIT";
                    dr[1] = ht["SEQ_ID"];
                }
                else
                {
                    dr[0] = "ADD";
                    dr[1] = null;
                }
                dr[2] = ht["BranchCode"];
                dr[3] = ht["CutOffStart"];
                dr[4] = ht["CutOffEnd"];
                dr[5] = ht["StalePeriod"];
                dr[6] = ht["CommSetup"];
                dr[7] = ht["RemittanceAmtLimit"];
                dr[8] = ht["AggregateAmtLimit"];
                dr[9] = ht["SupervisorLimit"];
                dr[10] = ht["TellerLimit"];
                dr[11] = ht["ARMLimit"];
                dr[12] = ht["InetLimit"];
                dr[13] = ht["RequiredID"];
                dr[14] = ht["FxDeviation"];
                dr[15] = ht["TDD_StImp_Min"];
                dr[16] = ht["TDD_StImp_Max"];
                dr[17] = ht["TDD_FirstTimeStImp_Min"];
                dr[18] = ht["TDD_FirstTImeStImp_Max"];
                dr[19] = ht["CashAggregationLimit"];
                dr[20] = ht["CashAggregationAmtLimit"];
                dr[21] = ht["AgentSysControl"];
                dr[22] = ht["AgentLogReport"];
                dr[23] = ht["ThreeDayAggregation"];
                dr[24] = ht["ThirtyDayAggregation"];
                dr[25] = ht["AgentAggAmtReject"];
                dr[26] = ht["DailyLimit"];
                dr[27] = ht["AgentCheckIDAmount"];
                dt.Rows.Add(dr);

                /* RFC-31620 */
                if (new irsUsers().adminApproverSwitch())
                {
                    Session["AdminApprovalFlagBranchSetupMessage"] = "RECORD SUBMITTED FOR APPROVAL";
                    if (dt.Rows[0][0].ToString().ToUpper() == "EDIT")
                    {
                        if (new irsUsers().IsMakerAllowedBranchSetup(irsUsers.ActionType.EDIT, ht["SEQ_ID"].ToString()))
                        {
                            throw new Exception("There is a pending approval for the same branch setup. Submission for edit to checker failed.");
                        }
                        new irsUsers().saveBranchSetupMakerChecker(irsUsers.ActionType.EDIT, dt);
                        return dt;
                    }
                    else
                    {
                        new irsUsers().saveBranchSetupMakerChecker(irsUsers.ActionType.ADD, dt);
                        return null;
                    }
                }
                else
                {
                    String errmsg = "";
                    DataTable dtSave = IRSBranchSetup.save_setup(ht, out errmsg);
                    if (dtSave == null) throw new Exception(errmsg);
                    return dtSave;
                }
            }

            throw new Exception("Invalid save parameters " + srec);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isAgent(string BranchCode)
        {
            DataTable dt = null;
            sqlHelperWs commons = new sqlHelperWs();

            try
            {
                string s = "SELECT * FROM TBL_BRANCHES WHERE BRANCHCODE='" + BranchCode.Trim() + "' AND BRANCHTYPE=4";

                //dt = commons.getDataSet(commons.getSqlCommand(s, false)).Tables[0];
                dt = commons.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            return false;
        }
    }
}
