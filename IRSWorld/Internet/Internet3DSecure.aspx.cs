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

namespace IRSWorld.Internet
{
    [Serializable]
    public partial class Internet3DSecure : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Internet3DSecure));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getENETS_REQUEST()
        {
            if (Session["ENETS_REQUEST"] != null)
            {
                new csAuditLog().writeAudit("ENETS", "Internet3DSecure", (string)Session["ENETS_REQUEST"], "ENETS_REQUEST", "", csAuditLog.Category.Insert);
                return (string)Session["ENETS_REQUEST"];                
            }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getENETS_URL()
        {
            if (Session["ENETS_URL"] != null)
            {
                new csAuditLog().writeAudit("ENETS", "Internet3DSecure", (string)Session["ENETS_URL"], "ENETS_URL", "", csAuditLog.Category.Insert);
                return (string)Session["ENETS_URL"];
            }
            return "";
        }
    }
}
