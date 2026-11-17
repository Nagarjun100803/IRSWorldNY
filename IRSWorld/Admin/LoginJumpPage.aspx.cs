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
using Bharosa.VCrypt.Common.Util;
using Bharosa.VCrypt.Client;
using System.Text;

using IRSClassLib;
using IRSWorld.bharosa;

namespace IRSWorld.Admin
{
    public partial class LoginJumpPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BharosaTrace.Enter("LoginJumpPage.Page_Load()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            AppUtil appUtil = AppUtil.GetInstance();
            bool pass = false;

            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            if (id == null)
            {
                new csControl().execScript(this.Page, "SPERR_PAGE", "spawn_errpage('Session Expired.');");
            }

            // try - catch added by chr including call to csControl
            try
            {
                if (!StringUtil.IsEmpty(sessionData.LoginId))
                {
                    //
                    // Create the user in Bharosa server, if it already does not exist
                    //

                    String oaam_usergroup = new irsUsers().getUserGroupByRole(sessionData.LoginId);
                    if (oaam_usergroup == null)
                    {
                        new csControl().oaam_close_frame(this.Page, false, "User: (" + sessionData.LoginId + ") Does not exist!", sessionData.LoginId, (short)0);
                        return;
                    }
                    appUtil.InitUser(Session, oaam_usergroup);
                    id.OAAM = true;
                    pass = true;
                    Response.Redirect("LoginHandlerPage.aspx?js_closer=login_close");
                }
                else
                {
                    new csControl().oaam_close_frame(this.Page, false, "Jump Page - Login ID is empty!", sessionData.LoginId, (short) 0);
                }
            }
            catch (Exception ex)
            {
                if (!pass)
                {
                    id.OAAM = false;
                    new csControl().oaam_close_frame(this.Page, false, "Jump Page - " + ex.Message, sessionData.LoginId, (short)-1);
                }
            }

            // - flash object embeded in this page (LoginJumpPage.aspx) will call CookieManager.aspx to register flash cookie

            // The javascript block will redirect to the LoginHandlerPage 
            // page after handling flash cookie.


            BharosaTrace.Exit("LoginJumpPage.Page_Load()");
        }
    }
}
