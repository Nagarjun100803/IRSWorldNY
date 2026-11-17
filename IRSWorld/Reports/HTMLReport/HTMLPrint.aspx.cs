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

namespace IRSWorld.Reports.HTMLReport
{
    public partial class HTMLPrint : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(HTMLPrint));

            this.hidQueryString.Value = Request.QueryString.ToString();

        }

        private irsClientData IRSClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }
                return null;
            }
        }

        
    }
}
