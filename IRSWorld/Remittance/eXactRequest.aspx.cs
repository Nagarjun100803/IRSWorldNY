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

namespace IRSWorld.Remittance
{
    [Serializable]
    public partial class eXactRequest : System.Web.UI.Page
    {
        eXact ext = new eXact();

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(eXactRequest));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(eXact));
            ext.transaction_amount = Convert.ToDecimal(this.getTranxAmount());
            ext.x_invoice_num = this.getRefNo();
            ext.country_code = this.getCountry(((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
            Response.Redirect(ext.post_request());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTranxAmount()
        {
            if (Session["EXACT_AMOUNT"] != null)
            {
                new csAuditLog().writeAudit("eXact", "getTranxAmount", (string)Session["EXACT_AMOUNT"], "getTranxAmount", "", csAuditLog.Category.Insert);
                return (string)Session["EXACT_AMOUNT"];
            }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRefNo()
        {
            if (Session["EXACT_REFNO"] != null)
            {
                new csAuditLog().writeAudit("eXact", "getRefNo", (string)Session["EXACT_REFNO"], "getRefNo", "", csAuditLog.Category.Insert);
                return (string)Session["EXACT_REFNO"];
            }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string geteXact_REQUEST()
        {
            if (Session["eXact_REQUEST"] != null)
            {
                new csAuditLog().writeAudit("eXact", "eXactRequest", (string)Session["eXact_REQUEST"], "eXact_REQUEST", "", csAuditLog.Category.Insert);
                return (string)Session["eXact_REQUEST"];
            }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string geteXact_URL()
        {
            if (Session["eXact_URL"] != null)
            {
                new csAuditLog().writeAudit("eXact", "eXactRequest", (string)Session["eXact_URL"], "eXact_URL", "", csAuditLog.Category.Insert);
                return (string)Session["eXact_URL"];
            }
            return "";
        }

        private int getCountry(string location)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataTable dt = sqlkonek.getDataSet("SELECT * From tbl_branches where Branchcode ='" + location + "'", false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["Country"]) > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["Country"]);
                }
                else
                {
                    return 0;
                }
                
            }
            else
            {
                return 0;
            }
        }
    }
}
