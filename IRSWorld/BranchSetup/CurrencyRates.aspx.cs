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
    public partial class CurrencyRates : System.Web.UI.Page
    {
        private irsCurrencyRates IRSCurrencyRates
        {
            get
            {
                if (Session["IRS_CURRENCY_RATES"] == null)
                {
                    Session["IRS_CURRENCY_RATES"] = new irsCurrencyRates();
                }
                return (irsCurrencyRates)Session["IRS_CURRENCY_RATES"];
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(CurrencyRates));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_currates()
        {
            Session["IRS_CURRENCY_RATES"] = null;
        }

        private String[] create_options(DataRowCollection rows, String val_fld, String data_fld)
        {
            ArrayList al = new ArrayList();
            foreach (DataRow row in rows)
            {
                al.Add(row[val_fld].ToString().Trim() + "~" + row[data_fld].ToString().Trim());
            }
            String[] ss = new String[al.Count];
            al.CopyTo(ss);
            return ss;
        }

        private String[] getBranchOptions()
        {
            DataTable dt = IRSCurrencyRates.getBranches();

            if (dt == null) throw new Exception("GET Branches: " + IRSCurrencyRates.Message);
            return create_options(dt.Rows, "BranchCode", "BranchName");
        }

        private Object[] getCurrencyOptions(String brcode)
        {
            DataSet ds = IRSCurrencyRates.getCurrencyTables(brcode);
            if (ds == null) throw new Exception("GET Lookup currency: " + IRSCurrencyRates.Message);
            String[] src_curr_opts = null;
            String[] tgt_curr_opts = null;
            if (ds.Tables.Contains("LOCAL"))
            {
                src_curr_opts = create_options(ds.Tables["LOCAL"].Rows, "fldValue", "fldCode");
            }
            if (ds.Tables.Contains("ACCOUNT"))
            {
                tgt_curr_opts = create_options(ds.Tables["ACCOUNT"].Rows, "fldValue", "fldCode");
            }

            return new Object[] { src_curr_opts, tgt_curr_opts };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getBranchLocalTime(String brcode)
        {
            return new Object[] { new irsTimeZone().branchLocalTime(brcode), IRSCurrencyRates.get_edit_setup(brcode) };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getBranchDdl(String x)
        {
            String currbr = IRSClientData.Location;
            String[] bropt = getBranchOptions();

            DataTable dtrt = IRSCurrencyRates.getRateTypes();

            Object[] datebr = getBranchLocalTime(currbr);

            return new Object[] 
            { 
                currbr, 
                bropt,
                IRSClientData.isSystemAdmin,
                dtrt,
                datebr[0],
                datebr[1]
            };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int[] hasChanges()
        {
            return (IRSCurrencyRates.hasChanges());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getCurrencyRates(String sdate, String branch, int rate_type, bool is_ord)
        {
            String xbr = (branch != null && branch.Length > 0) ? branch : IRSClientData.Location;
            try
            {
                Object[] curropt = getCurrencyOptions(xbr);
                DataTable dt = IRSCurrencyRates.getCurrencyRates(DateTime.Parse(sdate), xbr, rate_type, true);
                if (dt == null)
                {
                    throw new Exception(IRSCurrencyRates.Message);
                }
                object[] next_combo = IRSCurrencyRates.getNextCombo(rate_type, is_ord);
                return new object[] { curropt[0], curropt[1], dt, next_combo, IRSCurrencyRates.hasChanges(rate_type), rate_type };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] uploadRates(String sdt, String brcode, bool over, int rate_type, bool is_ord)
        {
            String xbr = (brcode != null && brcode.Length > 0) ? brcode : IRSClientData.Location;
            try
            {
                if (IRSCurrencyRates.upload_cybermag_rates(DateTime.Parse(sdt), xbr, over, rate_type, is_ord))
                {
                    return getCurrencyRates(sdt, xbr, rate_type, is_ord);
                }
                else
                {
                    throw new Exception(IRSCurrencyRates.Message);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getCybermagRates(String brcode, String sdt)
        {
            try
            {
                Object[] rs = IRSCurrencyRates.getUploadRates(brcode, DateTime.Parse(sdt));
                if (rs[0] != null)
                {
                    return rs;
                }
                throw new Exception(rs[1].ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool comboExist(int rate_type, String src_curr, String tgt_curr)
        {
            short s_src_curr = Convert.ToInt16(src_curr);
            short s_tgt_curr = Convert.ToInt16(tgt_curr);
            return IRSCurrencyRates.comboExist(rate_type, s_src_curr, s_tgt_curr);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getNextCombo(int rate_type)
        {
            return IRSCurrencyRates.getNextCombo(rate_type, false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyPost(String adds, String dels, String eds)
        {
            int seq_id = 0;
            string BranchCode = "";
            string TranxDate = "";
            int SourceCurrency = 0;
            int TargetCurrency = 0;
            decimal SourceToTarget = 0;
            decimal TargetToSource = 0;
            string DateInserted = "";
            int CurrencyType = 0;
            decimal MidSourceToTarget = 0;
            decimal MidTargetToSource = 0;
            int RatesMode = 0;
            int action_type = 0;

            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {
                        String[] uu = tt[i].Split('|');
                        int row = int.Parse(uu[0]);
                        Hashtable ht = new Hashtable();
                        for (int j = 1; j < uu.Length; j++)
                        {
                            String[] cols = uu[j].Split(sep);
                            if (cols.Length < 2) continue;
                            ht.Add(cols[0], cols[1]);
                        }

                        //==========================================================
                        // RFC-22918 : Currency rates issue in edit/delete record
                        //bool ok = IRSCurrencyRates.applyUpdates(crud, row, ht);
                        //if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSCurrencyRates.Message);

                        if (ht["SEQ_ID"].ToString() == ""){ seq_id = 0; }
                        else { seq_id = Convert.ToInt32(ht["SEQ_ID"].ToString()); }

                        BranchCode = ht["BranchCode"].ToString();
                        TranxDate = ht["TranxDate"].ToString();
                        SourceCurrency = Convert.ToInt32(ht["SourceCurrency"].ToString());
                        TargetCurrency = Convert.ToInt32(ht["TargetCurrency"].ToString());
                        SourceToTarget = Convert.ToDecimal(ht["SourceToTarget"].ToString());
                        TargetToSource = Convert.ToDecimal(ht["TargetToSource"].ToString());
                        DateInserted = ht["DateInserted"].ToString();
                        CurrencyType = Convert.ToInt32(ht["CurrencyType"].ToString());
                        MidSourceToTarget = 0;
                        MidTargetToSource = 0;

                        if (ht["RatesMode"].ToString() == "true" || ht["RatesMode"].ToString() == "1") { RatesMode = 1; }
                        
                        if (crud.ToUpper() == "ADD"){ action_type = 1; }
                        else if (crud.ToUpper() == "EDIT") { action_type = 2; }
                        else if (crud.ToUpper() == "DELETE") { action_type = 3; }
                        else { action_type = 0; }

                        if (this.update_currencyrates_record(seq_id, BranchCode, TranxDate, SourceCurrency, TargetCurrency, SourceToTarget, TargetToSource, DateInserted, CurrencyType, MidSourceToTarget, MidTargetToSource, RatesMode, action_type) < 0)
                        {
                            return false;
                        }
                        //==========================================================
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

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool saveUpdates(int rate_type, String adds, String dels, String eds)
        {
            bool ok = true;
            if (adds.Length > 0 | dels.Length > 0 | eds.Length > 0)
            {
                ok = applyPost(adds, dels, eds);
            }

            if (ok & !IRSCurrencyRates.saveUpdates(rate_type))
            {
                throw new Exception("Save changes: " + IRSCurrencyRates.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void setUploadColumn(String col)
        {
            String upcol = col.ToUpper() + "_RATE";
            IRSCurrencyRates.UploadColumn = upcol;
        }

        //==========================================================
        // RFC-22918 : Currency rates issue in edit/delete record
        public int update_currencyrates_record(int seq_id, string BranchCode, string TranxDate, int SourceCurrency, int TargetCurrency, decimal SourceToTarget, decimal TargetToSource, string DateInserted, int CurrencyType, decimal MidSourceToTarget, decimal MidTargetToSource, int RatesMode, int action_type)
        {
            /* VAPT IAG 20191007 */
            string RequestTime = System.DateTime.Now.ToString();
            /*********************/

            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataTable dt_old_record = new DataTable();
            int i_retval = 0;
            string newValue = "";
            string oldValue = "";

            try
            {
                dt_old_record = commons.execStoredProcedureTable("proc_get_currencyrates_record", new object[] { seq_id, BranchCode, TranxDate, 0 });
                i_retval = commons.execStoredProcedureUpdate("proc_update_currencyrates_record", new object[] { seq_id, BranchCode, TranxDate, SourceCurrency, TargetCurrency, SourceToTarget, TargetToSource, DateInserted, CurrencyType, MidSourceToTarget, MidTargetToSource, RatesMode, action_type });

                if (action_type == 1)
                {
                    newValue = "BRANCHCODE=" + BranchCode.ToString() + " ~ ";
                    newValue += "TRANXDATE=" + TranxDate.ToString() + " ~ ";
                    newValue += "SOURCECURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", SourceCurrency.ToString(), "FldCode") + " ~ ";
                    newValue += "TARGETCURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", TargetCurrency.ToString(), "FldCode") + " ~ ";
                    newValue += "SOURCETOTARGET=" + SourceToTarget.ToString() + " ~ ";
                    newValue += "TARGETTOSOURCE=" + TargetToSource.ToString() + " ~ ";
                    newValue += "DATEINSERTED=" + DateInserted.ToString();
                    new csAuditLog().writeVAPTLogger("", "BOD", "CREATED RATES FOR THE DAY", "", newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                }
                else if (action_type == 2)
                {
                    oldValue = "SEQ_ID=" + dt_old_record.Rows[0]["SEQ_ID"].ToString() + " ~ ";
                    oldValue += "BRANCHCODE=" + dt_old_record.Rows[0]["BranchCode"].ToString() + " ~ ";
                    oldValue += "TRANXDATE=" + dt_old_record.Rows[0]["TranxDate"].ToString() + " ~ ";
                    oldValue += "SOURCECURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dt_old_record.Rows[0]["SourceCurrency"].ToString(), "FldCode") + " ~ ";
                    oldValue += "TARGETCURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", dt_old_record.Rows[0]["TargetCurrency"].ToString(), "FldCode") + " ~ ";
                    oldValue += "SOURCETOTARGET=" + dt_old_record.Rows[0]["SourceToTarget"].ToString() + " ~ ";
                    oldValue += "TARGETTOSOURCE=" + dt_old_record.Rows[0]["TargetToSource"].ToString() + " ~ ";
                    oldValue += "DATEINSERTED=" + dt_old_record.Rows[0]["DateInserted"].ToString() + " ~ ";

                    newValue = "SEQ_ID=" + seq_id.ToString() + " ~ ";
                    newValue += "BRANCHCODE=" + BranchCode.ToString() + " ~ ";
                    newValue += "TRANXDATE=" + TranxDate.ToString() + " ~ ";
                    newValue += "SOURCECURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", SourceCurrency.ToString(), "FldCode") + " ~ ";
                    newValue += "TARGETCURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", TargetCurrency.ToString(), "FldCode") + " ~ ";
                    newValue += "SOURCETOTARGET=" + SourceToTarget.ToString() + " ~ ";
                    newValue += "TARGETTOSOURCE=" + TargetToSource.ToString() + " ~ ";
                    newValue += "DATEINSERTED=" + DateInserted.ToString() + " ~ ";
                    new csAuditLog().writeVAPTLogger("", "BOD", "EDITED RATES FOR THE DAY", oldValue, newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                }
                else if (action_type == 3)
                {
                    newValue = "SEQ_ID=" + seq_id.ToString() + " ~ ";
                    newValue += "BRANCHCODE=" + BranchCode.ToString() + " ~ ";
                    newValue += "TRANXDATE=" + TranxDate.ToString() + " ~ ";
                    newValue += "SOURCECURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", SourceCurrency.ToString(), "FldCode") + " ~ ";
                    newValue += "TARGETCURRENCY=" + new irsInternetPage().getLookupValue("Currency", "FldValue", TargetCurrency.ToString(), "FldCode") + " ~ ";
                    newValue += "SOURCETOTARGET=" + SourceToTarget.ToString() + " ~ ";
                    newValue += "TARGETTOSOURCE=" + TargetToSource.ToString() + " ~ ";
                    newValue += "DATEINSERTED=" + DateInserted.ToString() + " ~ ";
                    new csAuditLog().writeVAPTLogger("", "BOD", "DELETED RATES FOR THE DAY", "", newValue, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                }
                    
            }
            catch (Exception ex)
            {
                throw new Exception("Save changes : " + ex.Message);
            }
            finally
            {
                commons = null;
            }
            return i_retval;
        }
        //==========================================================
    }
}
