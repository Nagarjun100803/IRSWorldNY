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
            HtmlInputHidden hthid = (HtmlInputHidden)Page.FindControl("hidPath");
            if (hthid != null & Session["IRS_CLIENT_DATA"] != null)
            {
                IRSClassLib.irsClientData icd = (IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"];
                String url = new IRSClassLib.irsConfiguration().appsetting("IRSWorld_Portal_Site");
                if (icd.AppPath != String.Empty && (!icd.AppPath.Equals("Devel")))
                {
                    if (String.IsNullOrEmpty(url))
                    {
                        url = Request.Url.Scheme + "://" + Request.Url.Authority + "/IRSWorld/Default.aspx";
                    }
                    
                    hthid.Attributes.Remove("value");
                    hthid.Attributes.Add("value", url);
                }
            }

            if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString["ErrorMessage"] != null)
                {
                    if (Request.QueryString["Inet"] != null)
                    {
                        lblErrMsg.Text = "Thank you for using PNB online remittance system.";
                    }
                    else
                    {
                        lblErrMsg.Text = Request.QueryString["ErrorMessage"];
                    }
                }
                if (Request.QueryString["EndMessage"] != null)
                {
                    lblEndMsg.Text = Request.QueryString["EndMessage"];
                }
                if (Request.QueryString["ErrorMessage"] == null & Request.QueryString["EndMessage"] == null)
                {
                    try
                    {
                        String s = Server.GetLastError().Message;
                        if (s != null && s.Length > 0)
                        {
                            lblErrMsg.Text = s;
                        }
                    }
                    catch { }
                }
            }
            else
            {
                Exception ex = null;

                try
                {
                    ex = Server.GetLastError().GetBaseException();
                }
                catch
                { }

                if (ex != null)
                {
                    lblErrMsg.Text = ex.Message;
                    if (ex.InnerException != null)
                    {
                        lblEndMsg.Text = ex.InnerException.Message;
                    }
                }
                else
                {
                    try
                    {
                        String s = Server.GetLastError().Message;
                        if (s != null && s.Length > 0)
                        {
                            lblErrMsg.Text = s;
                        }
                    }
                    catch { }
                }
            }

            if (Session["IRS_CLIENT_DATA"] != null)
            {
                ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).logout();
            }

            try
            {
                // Make sure
                Session.Clear();
                Session.Abandon();

                int cookieCount = HttpContext.Current.Request.Cookies.Count;
                for (int i = 0; i < cookieCount; i++)
                {
                    HttpCookie cookie = HttpContext.Current.Request.Cookies[i];
                    if (cookie != null)
                    {
                        String cookieName = cookie.Name;
                        HttpCookie expiredCookie = new HttpCookie(cookieName);
                        expiredCookie.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Current.Response.Cookies.Add(expiredCookie); // overwrite it
                    }
                }

                // clear cookies server side
                HttpContext.Current.Request.Cookies.Clear();
            }
            catch { }
        }
    }
}
