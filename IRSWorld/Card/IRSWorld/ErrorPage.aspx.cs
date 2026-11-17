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

namespace IRSWorld
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["ErrorMessage"] != null)
                {
                    lblErrMsg.Text = Request.QueryString["ErrorMessage"];
                }
                if (Request.QueryString["EndMessage"] != null)
                {
                    lblEndMsg.Text = Request.QueryString["EndMessage"];
                }
            }
            else
            {
                Exception ex = Server.GetLastError().GetBaseException();
                if (ex != null)
                {
                    lblErrMsg.Text = ex.Message;
                    if (ex.InnerException != null)
                    {
                        lblEndMsg.Text = ex.InnerException.Message;
                    }
                }
            }

            if (Session["IRS_CLIENT_DATA"] != null)
            {
                ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).logout();
            }
        }
    }
}
