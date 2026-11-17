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
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.GFC
{
    public partial class GFCAggregation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GFCAggregation));
        }

        #region public method
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String getAggreAmount()
        {
            try
            {
                DataSet ds = doGFC.execProcedure("sp_GFCAggregation", new Object[] { 0, " "," ",0 });

                //if (ds != null) { return ds.Tables[0].Rows[0][0].ToString(); }
                if (ds != null) { return float.Parse(ds.Tables[0].Rows[0][0].ToString()).ToString("###,##0.00"); }
                else { throw new Exception(doGFC.message); }
            }
            catch //(Exception ex) // commented to remove warning
            {
                throw new Exception("Failed to get GFC Aggregation amount");
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool UpdateAggreAmount(String Amount)
        {
            try
            {
                String UserId = doGFC.get_irsclient_data("UID");
                String BranchCode = doGFC.get_irsclient_data("LOCATION");
                float AggreAmount = 0;

                if (!float.TryParse(Amount, out AggreAmount))
                {
                    throw new Exception("Invalid Amount");
                }

                if (doGFC.execProcedureUpdate("sp_GFCAggregation", new Object[] { AggreAmount, UserId, BranchCode, 1 })) { return true; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {
                //throw new Exception("Failed to Update Aggregation amount");
                throw new Exception("Failed to Update Aggregation amount, Reason : " + ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getSearchDetails(String SeqID, String Mode)
        {
            try
            {
                Object[] plist = { SeqID, Mode };
                DataSet ds = doGFC.execProcedure("sp_getGFCClient_Info", plist);

                if (ds != null) { return ds; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get client info, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet get_drop_down_data()
        {
            try
            {
                String sql = "select FldValue, FldDescription from tbl_lookup where fldgroup = 'GFCMONTHS'";
                DataSet ds = doGFC.getDataSet(sql);

                if (ds != null) { return ds; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get drop down list data, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String set_report_Monthly_Transactions(String Month, String Year, String CIFID, String CardNo)
        {
            try
            {
                String[] plist =
                {
                    Month,
                    Year,    
                    CIFID,
                    CardNo
                };

                String result = doGFC.setReport("/GFC/rptmonthlyTransactions.rpt", plist);

                if (result == "")
                {
                    throw new Exception(doGFC.message);
                }

                return result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion
        #region private method
        private irsGFC doGFC
        {
            get
            {
                if (Session["GFCAggregate"] == null)
                {
                    Session["GFCAggregate"] = new irsGFC();
                }
                return (irsGFC)Session["GFCAggregate"];
            }
        }
        #endregion
    }
}
