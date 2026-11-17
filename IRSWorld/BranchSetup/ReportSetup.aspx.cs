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
    public partial class ReportSetup : System.Web.UI.Page
    {
        private   irsReportSetup IRSReportSetup
        {
            get
            {
                if (Session["IRS_REPORT_SETUP"] == null)
                {
                    Session["IRS_REPORT_SETUP"] = new irsReportSetup();
                }
                return (irsReportSetup)Session["IRS_REPORT_SETUP"];
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

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ReportSetup));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            this.txtbranchcode.Value = IRSClientData.Location;
            this.txtisadmin.Value = (IRSClientData.isSystemAdmin)? "true" : "false";
        }

        /* RFC-31620 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAdminApproverTag()
        {
            string ret = string.Empty;
            if (Session["AdminApprovalFlagReportSetupMessage"] != null)
            {
                if (new irsUsers().adminApproverSwitch())
                {
                    return Session["AdminApprovalFlagReportSetupMessage"].ToString();
                }
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_rep_group(string branchcode,int isadmin)
        {
            DataSet ds = new DataSet();
            try
            {
                /* VAPT 20190322 */
                string loc = new irsInternetPage().getLookupValue("ACDBranchCode", "FldDescription", IRSClientData.Location, "FldDescription");
                if (loc != string.Empty)
                {
                    isadmin = 1;
                }
                ds = IRSReportSetup.load_report_maingroup(branchcode, isadmin);
                return ds;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_datagrid(string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = IRSReportSetup.load_report_perbranch (branchcode);
                return ds;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object applyEdits(String adds, String dels, String eds)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            string branchcode = "";
            int norec = 0;

            /* RFC-31620 */
            DataTable dt = new DataTable();
            DataRow dr;

            dt.Columns.Add("ACTIONTYPE");
            dt.Columns.Add("MAIN_SEQ_ID");
            dt.Columns.Add("REP_ID");
            dt.Columns.Add("BranchCode");
            dt.Columns.Add("ReportGroup");
            dt.Columns.Add("ReportName");

            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {
                        String[] uu = tt[i].Split('|');
                        if ((uu[0].ToString().Length == 0))
                        {
                            norec++;
                        }
                        else
                        {
                            int row = int.Parse(uu[0].Replace(",", ""));
                            Hashtable ht = new Hashtable();
                            for (int j = 1; j < (uu.Length - norec); j++)
                            {
                                String[] cols = uu[j].Split(sep);
                                if (cols.Length < 2)
                                {
                                    ht.Add(cols[0], "");
                                    continue;
                                }

                                ht.Add(cols[0], cols[1]);
                            }

                            /* RFC-31620 */
                            dr = dt.NewRow();
                            if (ht["status"].ToString() == "NOT ACTIVE")
                            {
                                dr[0] = "DELETE";
                            }
                            else
                            {
                                dr[0] = "ADD";
                            }
                            dr[1] = ht["SEQ_ID"];
                            dr[2] = ht["REP_ID"];
                            dr[3] = ht["BranchCode"];
                            dr[4] = new irsInternetPage().getLookupValue("ReportGroup", "FldCode", ht["SUBGROUPID"].ToString(), "FldDescription");
                            dr[5] = ht["FldDescription"];
                            dt.Rows.Add(dr);

                            branchcode = ht["BRCD"].ToString();
                            bool ok = IRSReportSetup.applyUpdates(crud, row, ht);
                            if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSReportSetup.Message);
                        }
                    }
                }
            }
            if (errs.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
            }

            /* RFC-31620 */
            if (new irsUsers().adminApproverSwitch())
            {
                Session["AdminApprovalFlagReportSetupMessage"] = null;
                string action = "RECORD SUBMITTED FOR APPROVAL";

                int i = 0;
                DataTable dtReportSetup = new DataTable(); //For multiple edits
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        dtReportSetup = new DataTable();
                        DataRow drReportSetup;

                        dtReportSetup.Columns.Add("ACTIONTYPE");
                        dtReportSetup.Columns.Add("MAIN_SEQ_ID");
                        dtReportSetup.Columns.Add("REP_ID");
                        dtReportSetup.Columns.Add("BranchCode");
                        dtReportSetup.Columns.Add("ReportGroup");
                        dtReportSetup.Columns.Add("ReportName");
                        drReportSetup = dtReportSetup.NewRow();

                        drReportSetup[0] = dt.Rows[i][0];
                        drReportSetup[1] = dt.Rows[i][1];
                        drReportSetup[2] = dt.Rows[i][2];
                        drReportSetup[3] = dt.Rows[i][3];
                        drReportSetup[4] = dt.Rows[i][4];
                        drReportSetup[5] = dt.Rows[i][5];
                        dtReportSetup.Rows.Add(drReportSetup);

                        if (dt.Rows[i][0].ToString().ToUpper() == "DELETE")
                        {
                            new irsUsers().saveReportSetupMakerChecker(irsUsers.ActionType.DELETE, dtReportSetup);
                        }
                        else
                        {
                            new irsUsers().saveReportSetupMakerChecker(irsUsers.ActionType.ADD, dtReportSetup);
                        }
                    }
                }

                Session["AdminApprovalFlagReportSetupMessage"] = action;
                return new irsReportSetup().get_reportlookup(branchcode).Tables[0];
            }
            else
            {
                return IRSReportSetup.saveUpdates(branchcode);
            }
        }
    }
}