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
namespace IRSWorld.BranchSetup
{
    public partial class Reports_LookupUI : System.Web.UI.Page
    {
        #region VARIABLES
        private string tempbranchcode = "LDN";
        private string temprole = "1";
        #endregion
        private irsReport_LookupUI IRSReports
        {
            get
            {
                if (Session["IRS_Reports"] == null)
                {
                    Session["IRS_Reports"] = new irsReport_LookupUI();
                }
                return (irsReport_LookupUI)Session["IRS_Reports"];
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(IRSWorld.BranchSetup.Reports_LookupUI));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(IRSWorld.Admin.Modules));
            this.hfsessionid.Value = HttpContext.Current.Session.SessionID;
            //this.txtbranchcode.Value = IRSClientData.Location; 
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
            if (Session["AdminApprovalFlagReportLookupMessage"] != null)
            {
                if (new irsUsers().adminApproverSwitch())
                {
                    return Session["AdminApprovalFlagReportLookupMessage"].ToString();
                }
            }
            return ret;
        }

        #region "AJAX PUBLIC METHOD"
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet show_param_grid(string seq_id)
        {
            try
            {
                //ds = IRSReports.load_report_group_lookup(IRSClientData.Location ,IRSClientData.Role.ToString() );
                return this.get_param_grid(seq_id);
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable load_sp_parameter(string spname)
        {
            DataTable  dt = new DataTable();
            try
            {
                //ds = IRSReports.load_report_group_lookup(IRSClientData.Location ,IRSClientData.Role.ToString() );
                dt = IRSReports.get_sp_params(spname);
                return dt;
            }
            catch
            {
                return null;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool CheckSQL(string SQL)
        {
            return IRSReports.validate_sql(SQL);


        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_group()
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup(IRSClientData.Location ,IRSClientData.Role.ToString() );
                ds = IRSReports.load_report_maingroup("REPTYPE");
                return ds;
            }
            catch
            {
                return null;
            }

        }
       
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_all()
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = IRSReports.load_report_maingroup("REPORTS");
                return ds;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_group_per_branch(string subgroupid)
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, this.tempbranchcode, this.temprole);
                return ds;
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_rep_per_branch(string subgroupid)
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = IRSReports.load_report_per_branch(subgroupid);
                return ds;
            }
            catch
            {
                return null;
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_branch(string branchcode)
        {
            DataSet ds = new DataSet();
            irsBranchLookup brl = null;
            try
            {
                brl = new irsBranchLookup();
                //ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, IRSClientData.Location);
                ds = brl.get_branches(branchcode);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                brl = null;
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read )]
        public Object applyEdits(String adds, String dels, String eds, String subgroupid)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            int norec = 0;

            /* RFC-31620 */
            DataTable dt = new DataTable();
            DataRow dr;
            bool isEdit = false;

            dt.Columns.Add("ACTIONTYPE");
            dt.Columns.Add("MAIN_SEQ_ID");
            dt.Columns.Add("MainGroupID");
            dt.Columns.Add("MainGroupDesc");
            dt.Columns.Add("SubGroupID");
            dt.Columns.Add("SubGroupDesc");
            dt.Columns.Add("TextDisplay");
            dt.Columns.Add("ParameterNames");
            dt.Columns.Add("ParameterTypes");
            dt.Columns.Add("ParameterDisplay");
            dt.Columns.Add("sp_Name");
            dt.Columns.Add("Role");
            dt.Columns.Add("SQLValue");

            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();

                    /* RFC-31620 */
                    if (tt != null && tt.Length > 0)
                    {
                        if (tt[0].ToLower().Contains("edit"))
                        {
                            isEdit = true;
                        }
                    }

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
                                    //ht.Add(cols[0], "");
                                    continue;
                                }
                                ht.Add(cols[0], cols[1]);
                            }

                            /* RFC-31620 */
                            dr = dt.NewRow();
                            if (isEdit)
                            {
                                dr[0] = "EDIT";
                            }
                            else
                            {
                                dr[0] = "ADD";
                            }
                            dr[1] = ht["SEQ_ID"];
                            if (ht["MainGroupID"] != null && ht["MainGroupID"].ToString() == string.Empty)
                            {
                                dr[2] = "REPORTS";
                                dr[3] = "REPORTS";
                            }
                            else
                            {
                                dr[2] = ht["MainGroupID"];
                                dr[3] = ht["MainGroupID"];
                            }
                            dr[4] = ht["SubGroupID"];
                            dr[5] = ht["SubGroupDesc"];
                            dr[6] = ht["TextDisplay"];
                            dr[7] = ht["ParameterNames"];
                            dr[8] = ht["Parametertypes"];
                            dr[9] = ht["ParameterDisplay"];
                            dr[10] = ht["sp_Name"];
                            if (ht["Role"] != null && ht["Role"].ToString() != "undefined")
                            {
                                dr[11] = ht["Role"];
                            }
                            else
                            {
                                dr[11] = "";
                            }
                            if (ht["SQLValue"] != null && ht["Role"].ToString() != "undefined")
                            {
                                dr[12] = ht["SQLValue"];
                            }
                            else
                            {
                                dr[12] = "";
                            }
                            dt.Rows.Add(dr);

                            bool ok = IRSReports.applyUpdates(crud, row, ht);
                            if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSReports.Message);
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
                Session["AdminApprovalFlagReportLookupMessage"] = null;
                string action = "RECORD SUBMITTED FOR APPROVAL";

                int i = 0;
                DataTable dtReportLookup = new DataTable(); //For multiple edits
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        dtReportLookup = new DataTable();
                        DataRow drReportLookup;

                        dtReportLookup.Columns.Add("ACTIONTYPE");
                        dtReportLookup.Columns.Add("MAIN_SEQ_ID");
                        dtReportLookup.Columns.Add("MainGroupID");
                        dtReportLookup.Columns.Add("MainGroupDesc");
                        dtReportLookup.Columns.Add("SubGroupID");
                        dtReportLookup.Columns.Add("SubGroupDesc");
                        dtReportLookup.Columns.Add("TextDisplay");
                        dtReportLookup.Columns.Add("ParameterNames");
                        dtReportLookup.Columns.Add("ParameterTypes");
                        dtReportLookup.Columns.Add("ParameterDisplay");
                        dtReportLookup.Columns.Add("sp_Name");
                        dtReportLookup.Columns.Add("Role");
                        dtReportLookup.Columns.Add("SQLValue");
                        drReportLookup = dtReportLookup.NewRow();

                        drReportLookup[0] = dt.Rows[i][0];
                        drReportLookup[1] = dt.Rows[i][1];
                        drReportLookup[2] = dt.Rows[i][2];
                        drReportLookup[3] = dt.Rows[i][3];
                        drReportLookup[4] = dt.Rows[i][4];
                        drReportLookup[5] = dt.Rows[i][5];
                        drReportLookup[6] = dt.Rows[i][6];
                        drReportLookup[7] = dt.Rows[i][7];
                        drReportLookup[8] = dt.Rows[i][8];
                        drReportLookup[9] = dt.Rows[i][9];
                        drReportLookup[10] = dt.Rows[i][10];
                        drReportLookup[11] = dt.Rows[i][11];
                        drReportLookup[12] = dt.Rows[i][12];
                        dtReportLookup.Rows.Add(drReportLookup);

                        if (dt.Rows[i][0].ToString().ToUpper() == "EDIT")
                        {
                            new irsUsers().saveReportLookupMakerChecker(irsUsers.ActionType.EDIT, dtReportLookup);
                        }
                        else
                        {
                            new irsUsers().saveReportLookupMakerChecker(irsUsers.ActionType.ADD, dtReportLookup);
                        }
                    }
                }

                Session["AdminApprovalFlagReportLookupMessage"] = action;
                return new irsReport_LookupUI().load_report_maingroup("Reports").Tables[0];
            }
            else
            {
                return IRSReports.saveUpdates("Reports");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object getFs()
        {
            String root = Server.MapPath("~/Reports");
            Object[] retval = IRSReports.getFs(root);
            if (retval == null) throw new Exception(IRSReports.Message);            
            return new Object[] { root, retval };
        }
        
        #endregion
        #region for class method
        public string getReportSeqId(String adds, String dels, String eds)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            int norec = 0;
            irsReports rep = new irsReports();
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
                                    //ht.Add(cols[0], "");
                                    continue;
                                }

                                ht.Add(cols[0], cols[1]);
                            }


                            bool ok = rep.applyUpdates(crud, row, ht);
                            if (!ok) errs.Add(crud + " Row: " + row + " - " + rep.Message);
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
            return rep.IdentityValue.ToString();
         
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet get_param_grid(string seq_id)
        {
            sqlHelper com = new sqlHelper();
            
            
            try
            {
                return com.getDataSet(com.getSqlCommand("SELECT ParamName ParameterNames ,ParamType ParameterTypes,ParamDisplay ParameterDisplay,SqlValue SqlValue,Rep_Seq_id, Rep_Seq_Id FROM  tbl_report_lookup where Rep_seq_id = '" + seq_id + "'", false), "TPARAM");
            }
            catch //(Exception e)  // commented to remove warning
            {
            }
            return null;
        }
        #endregion 
    }
}
