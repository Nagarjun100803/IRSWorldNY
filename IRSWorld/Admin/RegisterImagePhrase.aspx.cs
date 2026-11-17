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

using IRSClassLib;
using IRSWorld.bharosa;

namespace IRSWorld.Admin
{

    public partial class RegisterImagePhrase : System.Web.UI.Page
    {
        private bool not_main = false;

        protected void Page_Load(object sender, EventArgs e)
        {

            BharosaTrace.Enter("RegisterImagePhrase.Page_Load");

            try
            {
                if (Session["IRS_CLIENT_DATA"] == null)
                {
                    new csControl().oaam_close_frame(this.Page, false, "Session Expired", "", (short)-9);
                    return;
                }

                irsClientData icd = (irsClientData)Session["IRS_CLIENT_DATA"];
                if (!icd.OAAM)
                {
                    Session["SHOW_IMAGE_PHRASE"] = null;
                    new csControl().execScript(this.Page, "doOther", "close_frame_dialog();"); 
                    return;
                }
            }
            catch { }

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            AppUtil appUtil = AppUtil.GetInstance();
            string eventTarget = Request.Params["__EVENTTARGET"];
            AuthentiPadData passwordPad = null;

            if (Request.QueryString.Count > 0)
            {
                try
                {
                    not_main = (Request.QueryString["main"].Equals("false"));
                }
                catch 
                { }
            }

            if (IsPostBack)
            {
                if (StringUtil.IsEmpty(eventTarget))
                {
                    // we don't need password pad any more..
                    passwordPad = (AuthentiPadData)sessionData["passwordPadData"];

                    if (passwordPad != null)
                    {
                        sessionData["passwordPadData"] = null;
                        sessionData[passwordPad.PadName] = null;
                    }

                    string nextPage = "RegisterQuestionsPage.aspx";
                    if (not_main)
                    {
                        nextPage += "?main=false";
                    }

                    Response.Redirect(nextPage, true);
                }
                else if (eventTarget.Equals("ChangeImageAndCaption"))
                {
                    appUtil.AssignNewImageAndCaption(Session);
                }
                else if (eventTarget.Equals("SetKeyPad"))
                {
                    sessionData.UserAuthDevice = AuthDeviceType.DeviceKeyPadFull;
                    bool ret = appUtil.SetAuthMode(Session, AuthDeviceType.DeviceKeyPadFull);
                }
                else if (eventTarget.Equals("SetTextPad"))
                {
                    sessionData.UserAuthDevice = AuthDeviceType.DeviceTextPad;
                    bool ret = appUtil.SetAuthMode(Session, AuthDeviceType.DeviceTextPad);
                }
            }

            //
            // option to change the auth-device during registration is not available to new users
            // 
            if (sessionData.PreAuthRes.Result == PreAuthResult.Allow)//sessionData.PostAuthRes.AllowAuthDeviceChange
            {
                switch (sessionData.UserAuthDevice)
                {
                    case AuthDeviceType.DeviceKeyPad:
                    case AuthDeviceType.DeviceKeyPadFull:

                        this.lblSetPad.Text = "<a id=\"A1\" runat=\"server\" href=\"javascript:__doPostBack('SetTextPad', '');\" style=\"border: 0px; padding: 1px;\" tabindex=\"4\" class='redLinkBold'>"
                                            + "Reset</a> to the default security device";

                        this.LinkWhatsThis.Text = "<a href=\"javascript:helpWindow('html/KP_whatsthis');\">Learn more about your device</a>";

                        break;

                    case AuthDeviceType.DeviceTextPad:
                    case AuthDeviceType.DeviceTextPadGeneric:
                    default:

                        this.lblSetPad.Text = "<a id=\"A2\" runat=\"server\" href=\"javascript:__doPostBack('SetKeyPad', '');\" style=\"border: 0px; padding: 1px;\" tabindex=\"4\" class='redLinkBold'>"
                                                + "Upgrade</a> to a higher security device";

                        this.LinkWhatsThis.Text = "<a href=\"javascript:helpWindow('html/TP_whatsthis');\">Learn more about your device</a>";

                        break;
                }

                if (sessionData.UserAuthDevice == AuthDeviceType.DeviceTextPadGeneric)
                {
                    sessionData.UserAuthDevice = AuthDeviceType.DeviceTextPad;
                }
            }

            passwordPad = appUtil.CreateSampleAuthentipad(Session, "Bharosa_RegistrationPad");

            passwordPad.AuthPad.IsDisplayOnly = true;

            sessionData["passwordPadData"] = passwordPad;
            sessionData[passwordPad.PadName] = passwordPad.AuthPad;

            this.lblDemoAuthenticator.Text = passwordPad.AuthPad.HTML;

            BharosaTrace.Exit("RegisterImagePhrase.Page_Load");

        }

        protected void register_Click(object sender, EventArgs e)
        {
            //
            // Redirect the user to the page where he can register
            // the challenge questions
            //
            String nextPage = "RegisterQuestions.aspx";
            if (not_main)
            {
                nextPage += "?main=false";
            }            
            Response.Redirect(nextPage, true);
        }
    }

}
