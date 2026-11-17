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
using Bharosa.VCrypt.Client;
using Bharosa.VCrypt.Common;
using Bharosa.VCrypt.Common.Auth;
using Bharosa.VCrypt.Common.Tracker;
using Bharosa.VCrypt.Common.Util;

using IRSClassLib;
using IRSWorld.bharosa;

namespace IRSWorld.Admin
{

    public partial class LoginHandlerPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BharosaTrace.Enter("LoginHandlerPage.Page_Load()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            AppUtil appUtil = AppUtil.GetInstance();
            string loginPage = appUtil.GetLoginPage(sessionData);

            string loginId = sessionData.LoginId;

            int clientType = ClientTypeEnum.Normal;

            if (StringUtil.IsEmpty(loginId))
            {
                BharosaTrace.Error("LoginHandlerPage.Page_Load(): loginId not set!");

                // Response.Redirect(loginPage + "?action=error", true); // by chr
                // csControl call added by chr
                new csControl().oaam_close_frame(this.Page, false, "Login id is empty", null, (short)0);

                return;
            }

            int waitTime = BharosaConfig.getInt("flashcookie.waittime", 1000);

            //
            // Initialize with tracker
            //
            appUtil.InitTracker(Request, Response, Session);

            //Response.Redirect("Password.aspx?function=login&userid='" + loginId + "'", true);
            Response.Redirect("PasswordPage.aspx?js_closer=login_close", true);

            BharosaTrace.Exit("LoginHandlerPage.Page_Load()");
        }
    }
}
