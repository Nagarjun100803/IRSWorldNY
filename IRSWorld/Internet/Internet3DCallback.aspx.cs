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
    public partial class Internet3DCallback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Internet3DCallback));
            Session["3DMD"] = Request.Form["MD"];
            Session["3DPaRes"] = Server.UrlEncode(Request.Form["PaRes"]);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getMD()
        {
            return Session["3DMD"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getPaRes()
        {
            return Session["3DPaRes"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitPaymentGateway3D(string Request)
        {
            string sReturn = new irsPaymentGateway().send3DRequest(Request);
            /* E2EAPICHANGE 20220209 Removed OAAM */
            return sReturn;
        }
    }
}