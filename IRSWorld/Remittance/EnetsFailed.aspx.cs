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

namespace IRSWorld.Remittance
{
    [Serializable]
    public partial class EnetsFailed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EnetsFailed));
            string messageS = Request.Form["message"];
            string strReturn = new irsPaymentGateway().GetENETSResponse(messageS);
            Session["ENETS_RESPONSE"] = strReturn;
            new csAuditLog().writeAudit("ENETS", "EnetsFailed", strReturn, messageS, "", csAuditLog.Category.Insert);
            ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
        }
    }
}
