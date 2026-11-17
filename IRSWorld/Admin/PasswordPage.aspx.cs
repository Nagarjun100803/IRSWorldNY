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
using Bharosa.VCrypt.Common.Util;
using IRSClassLib;
using IRSWorld.bharosa;

namespace IRSWorld.Admin
{
    public partial class PasswordPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /* VAPT IAG 20170516 */
            Session["OAAM_AUTHENTICATED"] = null;
            BharosaTrace.Enter("PasswordPage.Page_Load()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            AppUtil appUtil = AppUtil.GetInstance();

            if (sessionData == null)
            {
                new csControl().execScript(this.Page, "SEXPIRED", "spawn_errpage('Session Expired');");
                return;
            }

            if (!IsPostBack)
            {
                /*
                irsUsers iux = new irsUsers();
                if (iux.user_exist("USERID", sessionData.LoginId))
                {
                    short iflag = Convert.ToInt16(iux.getColumn("Flags"));
                    if (iflag != iux.FLAG_ACTIVE)
                    {
                        divForgot.Style.Add("display", "none");
                    }
                }
                */
                /* VAPT IAG 20170515 */
                irsClientData icd = (IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"];
                irsUsers iux = new irsUsers();
                if (icd.UserExist)
                {
                    short iflag = Convert.ToInt16(icd.UserDBDatatable.Rows[0]["Flags"]);
                    if (iflag != iux.FLAG_ACTIVE)
                    {
                        divForgot.Style.Add("display", "none");
                    }
                }
                else
                {
                    new csControl().execScript(this.Page, "USERDIS", "spawn_errpage('User does not exist.');");
                    return;
                }

                IBharosaClient client = BharosaClientFactory.getClientInstance();

                string loginPage = appUtil.GetLoginPage(sessionData);
                int clientType = ClientTypeEnum.Normal;
                int authStatus = AuthStatusEnum.Pending;

                //
                // Update tracker with the user auth status as Pending
                //
                appUtil.UpdateAuthStatus(Request, Response, Session, sessionData.LoginId, sessionData.CustomerId,
                                         sessionData.CurrentUser == null ? null : sessionData.CurrentUser.CustomerGroupId,
                                         clientType, authStatus);

                //
                // Run the pre auth rules
                //
                PreAuthResultInfo res = appUtil.RunPreAuthRules(Request, Session);
                AuthentiPadResultInfo authPadRes = null;

                //
                // If preauth rules return 'block', update the status in the tracker and bailout
                //
                if (res.Result == PreAuthResult.Block || res.Result == PreAuthResult.None)
                {
                    authStatus = AuthStatusEnum.Block;

                    appUtil.UpdateAuthStatus(Request, Response, Session, sessionData.LoginId, sessionData.CustomerId,
                                             sessionData.CurrentUser == null ? null : sessionData.CurrentUser.CustomerGroupId,
                                             clientType, authStatus);

                    BharosaTrace.Debug("PasswordPage.Page_Load... Login blocked by pre-authentication rules.");
                    // commented by chr in liu 
                    // Response.Redirect(loginPage + "?action=block");
                    //TBD: Logout User and Transfer User to Block Page
                    new csControl().oaam_close_frame(this.Page, false, "User (" + sessionData.LoginId + ") is blocked.", sessionData.LoginId, -9);
                    return;
                }
                /* else if (res.Result == PreAuthResult.None)
                 {
                     BharosaTrace.Debug("PasswordPage.Page_load... Pre-auth result was [None]."
                                      + "Now running the AuthentiPadRules...");
                     authPadRes = appUtil.RunAuthentiPadRules(Request, Session);
                 } */
                else if (res.Result == PreAuthResult.Allow)
                {
                    BharosaTrace.Debug("PasswordPage.Page_load... Pre-auth result was [Allow]."
                                     + "Now running the AuthentiPadRules...");
                    authPadRes = appUtil.RunAuthentiPadRules(Request, Session);
                }

                //
                // Lets create the authentipad and render it.
                //
                AuthentiPadData pinPadData = appUtil.CreateAuthentiPad(Session, "Bharosa_PasswordPad");

                lblPasswordPad.Text = client.getAuthentiPadHTML(pinPadData.AuthPad, true, false);

                //
                // The passwordPadData is saved to the session to be used in the postback
                //
                sessionData["passwordPadData"] = pinPadData;
                sessionData[pinPadData.PadName] = pinPadData.AuthPad;

                //
                // Show instruction and help link depending upon the authenticator used
                //
                switch (authPadRes.DeviceType)
                {
                    case AuthDeviceType.DeviceTextPad:
                        lblInstruction.Text = "Please use this secure TextPad to enter your password.";
                        LinkWhatsThis.Text = "<a href=\"javascript:helpWindow('html/TP_whatsthis');\" title=\"What's this?\">What's this?</a>";
                        break;
                    case AuthDeviceType.DeviceKeyPad:
                        lblInstruction.Text = "Please use this secure KeyPad to enter your password.";
                        LinkWhatsThis.Text = "<a href=\"javascript:helpWindow('html/KP_whatsthis');\" title=\"What's this?\">What's this?</a>";
                        break;
                    case AuthDeviceType.None:
                        lblInstruction.Text = "Please use this secure TextPad to enter your password.";
                        LinkWhatsThis.Text = "<a href=\"javascript:helpWindow('html/TP_whatsthis');\" title=\"What's this?\">What's this?</a>";
                        break;
                }
            }
            else
            {
                VCryptAuthUser user = sessionData.CurrentUser;
                string loginId = sessionData.LoginId;
                string loginPage = appUtil.GetLoginPage(sessionData);
                string nextPage = BharosaConfig.get("success.page.url", "Success.aspx");

                //
                // get the keypad object from sessionData
                //
                AuthentiPadData padData = (AuthentiPadData)sessionData["passwordPadData"];
                AuthentiPad authpad = padData.AuthPad;
                int clientType = padData.ClientType;

                //
                // get the encoded password value from HTTP request
                //
                string encodedPassword = Request.Params[padData.DataFieldName];

                //
                // decode the password
                //
                string password = authpad.decodeInput(encodedPassword);

                //
                // we don't need password-authentipad any more..
                //
                sessionData["passwordPadData"] = null;
                sessionData[padData.PadName] = null;

                #region Commented Code by CHR
                /* original code commented by chr to use irsUsers.
                //
                // Validate the username and password
                //
                int authStatus = appUtil.IsValidLogin(loginId, password);

                if (authStatus == AuthStatusEnum.UserDisabled)
                {
                    nextPage = loginPage + "?action=disabled_error";
                }
                else if (authStatus == AuthStatusEnum.InvalidUser)
                {
                    nextPage = loginPage + "?action=invalid_error";
                }
                else if (authStatus == AuthStatusEnum.WrongPassword)
                {
                    nextPage = loginPage + "?action=password_error";
                }
                else if (authStatus == AuthStatusEnum.Success)
                {
                    if (sessionData.InitialUserStatus == UserStatus.Invalid)
                    {
                        appUtil.SetUserStatusValid(Session);
                    }

                    nextPage = BharosaConfig.get("success.page.url", "Success.aspx");
                }
                 * * end comment by chr */
                #endregion

                int authStatus = AuthStatusEnum.Pending;
                irsUsers iu = new irsUsers();
                //Object[] retval = iu.validate(loginId, password);
                /* VAPT IAG 20170515 */
                Object[] retval = iu.validate_login(loginId, password);
                bool bflag = (bool)retval[0];
                short iflag = (short)retval[4];
                String iu_msg = (String)retval[1];
                if (bflag)
                {
                    authStatus = AuthStatusEnum.Success;
                    BharosaTrace.Debug("if user were set invalid then set to valid now");

                    //Nitesh: Here we have to set User status as valid
                    if (sessionData.InitialUserStatus == UserStatus.Invalid)
                    {
                        BharosaTrace.Debug("Setting the user status valid");
                        appUtil.SetUserStatusValid(Session);
                    }
                }
                else
                {
                    if (iflag == iu.FLAG_NEW) authStatus = AuthStatusEnum.Pending;
                    else if (iflag == iu.FLAG_DISABLED) //| iflag == iu.FLAG_EXPIRED)
                    {
                        authStatus = AuthStatusEnum.UserDisabled;
                        iflag = -9;
                    }
                    else if (iu_msg.IndexOf("does not exist!") > -1) authStatus = AuthStatusEnum.InvalidUser;
                    else if (iu_msg.IndexOf("is currently logged on.") > -1)
                    {
                        authStatus = AuthStatusEnum.Block;
                        iflag = -9;
                    }
                    else
                    {
                        authStatus = AuthStatusEnum.WrongPassword;
                    }
                    /*
                    else if (iu_msg.IndexOf("is not correct!") > -1 | iu_msg.IndexOf("exceeded allowed tries.") > -1) authStatus = AuthStatusEnum.WrongPassword;
                    else if (iu_msg.IndexOf("password is expired.") > -1)
                    {
                        authStatus = AuthStatusEnum.UserDisabled;
                        iflag = -9;
                    }
                     * */
                }

                if (authStatus == AuthStatusEnum.Success && iflag == iu.FLAG_NEW)
                {
                    Session["SHOW_IMAGE_PHRASE"] = true;
                }

                //
                // Update tracker with the status of user-authentication
                //
                appUtil.UpdateAuthStatus(Request, Response, Session, sessionData.LoginId, sessionData.CustomerId,
                                         sessionData.CurrentUser == null ? null : sessionData.CurrentUser.CustomerGroupId,
                                         clientType, authStatus);

                //
                // If the login is successful, run post-auth rules to determine what to do next
                //
                if (authStatus == AuthStatusEnum.Success)
                {
                    PostAuthResultInfo res = appUtil.RunPostAuthRules(Request, Session);

                    switch (res.Result)
                    {
                        case PostAuthResult.Block:
                        case PostAuthResult.None:
                            //
                            // username/password entered is valid; but rules blocked the login
                            // update the tracker with the status and redirect to blocked page
                            //
                            if (authStatus == AuthStatusEnum.Success)
                            {
                                appUtil.UpdateAuthStatus(Request, Response, Session, sessionData.LoginId, sessionData.CustomerId,
                                                         sessionData.CurrentUser == null ? null : sessionData.CurrentUser.CustomerGroupId,
                                                         clientType, AuthStatusEnum.Block);
                                BharosaTrace.Debug("PasswordPage.Page_Load()...username/password entered is valid; but rules blocked access.");
                            }

                            //Logout User and show user a common block page
                            //nextPage = loginPage + "?action=block";
                            iu_msg = "User Blocked.";
                            bflag = false;
                            iflag = -9;
                            break;

                        case PostAuthResult.Allow:
                            BharosaConfig.get("success.page.url", "Success.aspx");
                            break;
                        //
                        // for tracker-only integration, treat the following results as success
                        // for tracker&authenticator integration, following results will redirect the user to different pages (like challenge, register, etc)
                        //
                        case PostAuthResult.ChallengeUser:
                            nextPage = BharosaConfig.get("challenge.user.page.url", "ChallengeUser.aspx");
                            break;
                        case PostAuthResult.RegisterQuestions:
                            //nextPage = BharosaConfig.get("register.questions.page.url", "RegisterQuestionsPage.aspx");
                            Session["SHOW_IMAGE_PHRASE"] = true;
                            break;
                        case PostAuthResult.RegisterUser:
                            //sessionData["RegistrationOptional"] = "false";
                            //nextPage = BharosaConfig.get("personalization.page.url", "PersonalizationPage.aspx");
                            Session["SHOW_IMAGE_PHRASE"] = true;
                            break;
                        case PostAuthResult.RegisterUserOptional:
                            //sessionData["RegistrationOptional"] = "true";
                            //nextPage = BharosaConfig.get("personalization.page.url", "PersonalizationPage.aspx");
                            Session["SHOW_IMAGE_PHRASE"] = true;
                            break;
                    }
                }
                else if (authStatus == AuthStatusEnum.WrongPassword)
                {

                    BharosaTrace.Debug("PasswordPage.Page_Load(): Lets Allow OAAM to know about wrong password...");

                    if (AppUtil.GetInstance().userBlockedOnWrongPassword(Request, Session))
                    {
                        // Do blocking routine.
                        new csControl().oaam_close_frame(this.Page, false, "User (" + sessionData.LoginId + ") Blocked. You have exceeded maximum tries.", sessionData.LoginId, -9);
                    }
                }

                BharosaTrace.Debug("PasswordPage.Page_Load(): nextPage=" + nextPage);
                if (nextPage.IndexOf("ChallengeUser") > -1)
                {
                    Response.Redirect(nextPage + "?bflag=" + bflag.ToString() + "&iu_msg=" + iu_msg + "&iflag=" + iflag + "&js_closer=login_close", true);
                }
                else
                {
                    new csControl().oaam_close_frame(this.Page, bflag, iu_msg, (String)retval[2], iflag);
                }
            }

            BharosaTrace.Exit("PasswordPage.Page_Load()");
        }

        protected void lnkForgot_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChallengeUser.aspx?bflag=false&iu_msg=Forgot%20Password&iflag=5&js_closer=login_close", true);
        }
    }
}