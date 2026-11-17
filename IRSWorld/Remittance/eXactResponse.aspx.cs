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
using IRSClassLib;
using IRSWorld.TranClass;

namespace IRSWorld.Remittance
{
    [Serializable]
    public partial class eXactResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(eXactResponse));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(eXact));
            //string messageS = Request.Form[""];
            //string strReturn = new irsPaymentGateway().GetENETSResponse(messageS);
            //Session["EXACT_RESPONSE"] = strReturn;

            eXact exact = new eXact();

            exact.get_response(Request);

            string s1 = exact.x_response_code;
            string s2 = exact.x_response_reason_text;
            string s3 = exact.exact_ctr;
            string s4 = exact.exact_issname;
            string s5 = exact.exact_issconf;
            string s6 = exact.expiry_date;
            string s7 = exact.authorization_num;
            string s8 = exact.merchant_country;
            string s9 = exact.card_holders_name;
            string s10 = exact.card_number;

            string strReturn = s1 + ";" + s2 + ";" + s3 + ";" + s4 + ";" + s5 + ";" + s6 + ";" + s7 + ";" + s8 + ";" + s9 + ";" + s10;
            string messageS = "x_response_code=" + s1 + ";x_response_reason_text=" + s2 + ";exact_ctr=" + s3 + ";exact_issname=" + s4
                + ";exact_issconf=" + s5 + ";expiry_date=" + s6 + ";authorization_num=" + s7 + ";merchant_country=" 
                + s8 + ";card_holders_name=" + s9 + ";card_number=" + s10;
            Session["EXACT_RESPONSE"] = strReturn;

            new csAuditLog().writeAudit("EXACT", "eXactResponse", strReturn, messageS, "", csAuditLog.Category.Insert);
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "parent.internetClose('eXact');parent.close_frame_dialog(false);", true);
        }
    }
}
