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
using Bharosa.VCrypt.Common.Auth;
using Bharosa.VCrypt.Client;
using IRSWorld.bharosa;

using IRSClassLib;

namespace IRSWorld.Admin
{

    public partial class ChallengeUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BharosaTrace.Enter("ChallengeUser.Page_Load");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            if (sessionData == null)
            {
                new csControl().oaam_close_frame(this.Page, false, "Session Expired", "", (short)-9);
                return;
            }
            AppUtil appUtil = AppUtil.GetInstance();
            String customerId = sessionData.CustomerId;
            IBharosaClient client = BharosaClientFactory.getClientInstance();
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            /* Added by Troy 20160922 */
            irsClientData id = (irsClientData)Session["IRS_CLIENT_DATA"];
            this.PlaceHolder1.Visible = !(proxy.IsDeviceMarkedSafe(sessionData.RequestId).IsSuccess);

            bool bflag = false;
            if (Request.QueryString["bflag"] != null)
            {
                bflag = Convert.ToBoolean(Request.QueryString["bflag"]);
            }
            short iflag = -9;
            if (Request.QueryString["iflag"] != null)
            {
                iflag = Convert.ToInt16(Request.QueryString["iflag"]);
            }
            String iu_msg = "Parameter Error.";
            if (Request.QueryString["iu_msg"] != null)
            {
                iu_msg = Request.QueryString["iu_msg"];
            }

            if (!IsPostBack)
            {
                //
                // Fetch Secret Question for the user
                //
                VCryptQuestion secretQuestion = proxy.getSecretQuestion(customerId);
                String question = secretQuestion != null ? secretQuestion.Question : null;

                if (StringUtil.IsEmpty(question))
                {
                    BharosaTrace.Warn("No preassigned challenge question for this user, so getting some dummy question: " + question);

                    question = BharosaConfig.get("dummy.challenge.question", "What year did you graduate from college?");
                }

                question = HttpUtil.encode(question);

                AuthentiPadData padData = appUtil.CreateAuthentiPad(Session, AuthDeviceType.DeviceQuestionPad);

                sessionData["questionPadData"] = padData;
                sessionData[padData.PadName] = padData.AuthPad;

                padData.AuthPad.QuestionText = question;

                this.textpad.Text = client.getAuthentiPadHTML(padData.AuthPad, true, false);
                /* Added by Troy 20160921 OAAM bypassed */
                id.OAAMScreen = true;
            }
            else
            {
                id.OAAMSuccess = false;
                AuthentiPadData questionPad = (AuthentiPadData)sessionData["questionPadData"];
                string answer = Request.Params[questionPad.DataFieldName];
                VCryptAuthUser user = sessionData.CurrentUser;
                VCryptAuthResult res = null;
                int authStatus = 0;
                string nextPage = null;

                //
                // Verify the answer entered by the user
                //
                res = proxy.authenticateQuestion(customerId, answer);

                if (res != null && res.ResultCode == 0)
                {
                    authStatus = AuthStatusEnum.Success;
                    bool isSafe = proxy.MarkDeviceSafe(sessionData.RequestId, registerDevice.Checked);
                    BharosaTrace.Debug("MarkDevice status" + Convert.ToString(isSafe));
                    nextPage = "Success.aspx";
                }
                else
                {
                    authStatus = AuthStatusEnum.WrongAnswer;
                    /* VAPT 20180222 */
                    string ipAddress = string.Empty;
                    string u = Session["SES_USER_LOG_ID"].ToString();
                    try
                    {
                        if (Context.Request.ServerVariables["HTTP_VIA"] != null)
                        {
                            ipAddress = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                        }
                        else
                        {
                            ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        }
                        Session["IPAddress"] = ipAddress;
                        new csAuditLog().writeVAPTLogger(u, "LOGIN", "USER ATTEMPTED TO ANSWER THE CHALLENGE QUESTION WITH AN INVALID PASSWORD", "", "", ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    }
                    catch (Exception ex)
                    {
                        new csAuditLog().writeVAPTLogger(u, "LOGIN", "USER ATTEMPTED TO ANSWER THE CHALLENGE QUESTION WITH AN INVALID PASSWORD", ex.Message, "", ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    }
                }

                //
                // Update tracker with the status of user-authentication 
                //
                appUtil.UpdateAuthStatus(Request, Response, Session, sessionData.LoginId, sessionData.CustomerId,
                                         user.CustomerGroupId, ClientTypeEnum.Question, authStatus);

                //
                // let the user answer again, if rules permit it
                //
                if (authStatus == AuthStatusEnum.WrongAnswer)
                {
                    ChallengeUserRulesResultInfo result = appUtil.RunChallengeUserRules(Request, Session);

                    if (result != null && ((result.Result == ChallengeUserRulesResult.ChallengeUser) || (result.Result == ChallengeUserRulesResult.Allow)))
                    {
                        this.lblErrorMsg.Text = "Incorrect answer, please try again.";
                        ErrorPlaceHolder.Visible = true;
                    }
                    else
                    {
                        authStatus = AuthStatusEnum.Block;
                        nextPage = appUtil.GetLoginPage(sessionData) + "?action=block";

                        //
                        // Update tracker with the status of user-authentication 
                        //
                        appUtil.UpdateAuthStatus(Request, Response, Session, sessionData.LoginId, sessionData.CustomerId,
                                                 user.CustomerGroupId, ClientTypeEnum.Question, authStatus);

                        //Response.Redirect(nextPage, true);
                        new csControl().oaam_close_frame(this.Page, false, "User Blocked.", sessionData.LoginId, (short)-9);
                    }
                }
                else
                {
                    /* Added by Troy 20160921 OAAM bypassed */
                    id.OAAMSuccess = true;
                    Session["OAAM_AUTHENTICATED"] = true;
                    //Response.Redirect(nextPage, true);

                    /* VAPT 20180222 */
                    string ipAddress = string.Empty;
                    string u = Session["SES_USER_LOG_ID"].ToString();
                    try
                    {
                        if (Context.Request.ServerVariables["HTTP_VIA"] != null)
                        {
                            ipAddress = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                        }
                        else
                        {
                            ipAddress = Context.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        }
                        new csAuditLog().writeVAPTLogger(u, "LOGIN", "CHALLENGE QUESTION WAS ANSWERED SUCCESSFULLY", "", "", ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                    }
                    catch (Exception ex)
                    {
                        new csAuditLog().writeVAPTLogger(u, "LOGIN", "CHALLENGE QUESTION WAS ANSWERED SUCCESSFULLY", ex.Message, "", ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                    }

                    new csControl().oaam_close_frame(this.Page, bflag, iu_msg, sessionData.LoginId, iflag);
                }

            }
        }
    }
}
