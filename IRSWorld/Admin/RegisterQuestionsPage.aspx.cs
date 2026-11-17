using System;
using System.Web;
using System.Web.SessionState;
using Bharosa.VCrypt.Client;
using Bharosa.VCrypt.Common;
using Bharosa.VCrypt.Common.Util;
using Bharosa.VCrypt.Common.Auth;
using IRSWorld.bharosa;
using IRSClassLib;

namespace IRSWorld.Admin
{
    public partial class RegisterQuestionsPage : System.Web.UI.Page
    {
        private bool not_main = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            BharosaTrace.Enter("RegisterQuestionsPage.Page_Load()");

            AppSessionData sessionData = AppSessionData.GetInstance(Session);
            IBharosaProxy proxy = BharosaClientFactory.getProxyInstance();
            IBharosaClient client = BharosaClientFactory.getClientInstance();
            AppUtil appUtil = AppUtil.GetInstance();
            string customerId = sessionData.CustomerId;
            string padName = BharosaConfig.get("register.questions.questionpad.name", "Bharosa_Register_Pad");
            string mLocale = (string)Session["Locale"];

            registerSubmitPlaceHolder.Visible = !(proxy.IsDeviceMarkedSafe(sessionData.RequestId).IsSuccess);

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
                VCryptQuestionList questions = proxy.getAllMappedSignOnQuestions(customerId);

                if (questions != null && questions.Count != 0)
                {
                    BharosaTrace.Warn("User already has challenge questions mapped..");

                    //string nextPage = BharosaConfig.get("success.page.url", "Success.aspx"); // TODO: redirect to a page that give detailed message to the user

                    //Response.Redirect(nextPage, true);

                    Session["SHOW_IMAGE_PHRASE"] = null;
                    if (not_main)
                    {
                        new csControl().execScript(this.Page, "doOther", "close_frame_dialog();");
                    }
                    else
                    {
                        new csControl().execScript(this.Page, "doOther", "goto_main();");
                    }
                }
                else
                {
                    int num = Int32.Parse((string)sessionData["questionGroupCount"]);

                    questions = new VCryptQuestionList();

                    for (int i = 0; i < num; i++)
                    {
                        string answerPad = padName + i;
                        string questionId = Request.Params["question" + i];
                        string questionText = Request.Params["questionText" + i];
                        string answer = Request.Params[answerPad + "DataField"];

                        if (StringUtil.IsEmpty(questionId) || StringUtil.IsEmpty(questionText))
                        {
                            if (StringUtil.IsEmpty(answer))
                            {
                                BharosaTrace.Warn("Empty answer entered");
                                continue;
                            }
                        }

                        BharosaTrace.Debug("Adding security question '" + questionId + "[" + questionText + "]" + "' to user " + customerId + "...");

                        VCryptQuestion question = new VCryptQuestion();
                        BharosaStringList answerList = new BharosaStringList();

                        question.QuestionId = Int64.Parse(questionId);
                        question.Question = questionText;
                        question.AnswerList = new BharosaStringList();

                        question.AnswerList.Add(answer);

                        questions.Add(question);
                    }

                    VCryptResponse response = proxy.addQuestionsEx(customerId, questions);

                    /* VAPT 20180222 */
                    string ipAddress = string.Empty;
                    string u = Session["SES_USER_LOG_ID"].ToString();

                    if (response == null || !response.IsSuccess)
                    {
                        BharosaTrace.Warn("Failed to add security questions. " + response);
                        // TODO: display the error message to the user

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
                            new csAuditLog().writeVAPTLogger(u, "LOGIN", "USER FAILED TO ENROLL NEW CHALLENGE QUESTION", response.ToString(), "", ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeVAPTLogger(u, "LOGIN", "USER FAILED TO ENROLL NEW CHALLENGE QUESTION", response.ToString(), ex.Message, ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        }
                    }
                    else
                    {
                        bool isSafe = proxy.MarkDeviceSafe(sessionData.RequestId, registerDevice.Checked);
                        BharosaTrace.Debug("MarkDevice status" + Convert.ToString(isSafe));
                        BharosaTrace.Debug("Successfully added security question");
                        
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
                            new csAuditLog().writeVAPTLogger(u, "LOGIN", "USER SUCCESSFULLY ENROLLED NEW CHALLENGE QUESTION", "", "", ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                        }
                        catch (Exception ex)
                        {
                            new csAuditLog().writeVAPTLogger(u, "LOGIN", "USER SUCCESSFULLY ENROLLED NEW CHALLENGE QUESTION", ex.Message, "", ipAddress, csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                        }
                    }

                    //string nextPage = BharosaConfig.get("success.page.url", "Success.aspx");

                    //Response.Redirect(nextPage, true);

                    Session["SHOW_IMAGE_PHRASE"] = null;
                    if (not_main)
                    {
                        new csControl().execScript(this.Page, "doOther", "close_frame_dialog();");
                    }
                    else
                    {
                        new csControl().execScript(this.Page, "doOther", "goto_main();");
                    }
                }
            }
            else
            {
                VCryptQuestionList questions = proxy.getAllMappedSignOnQuestions(customerId);

                if (questions != null && questions.Count != 0)
                {
                    BharosaTrace.Warn("User already has challenge questions mapped..");

                    //string nextPage = BharosaConfig.get("success.page.url", "Success.aspx");
                    // TODO: redirect to a page that give detailed message to the user

                    //Response.Redirect(nextPage, true);

                    Session["SHOW_IMAGE_PHRASE"] = null;
                    if (not_main)
                    {
                        new csControl().execScript(this.Page, "doOther", "close_frame_dialog();");
                    }
                    else
                    {
                        new csControl().execScript(this.Page, "doOther", "goto_main();");
                    }
                }

                VCryptQuestionList[] groups = proxy.getSignOnQuestions(sessionData.CustomerId, mLocale);

                if (groups == null || groups.Length == 0)
                {
                    BharosaTrace.Warn("Failed to get challenge questions for registration");

                    //string nextPage = BharosaConfig.get("success.page.url", "Success.aspx");
                    // TODO: redirect to a page that give detailed message to the user

                    //Response.Redirect(nextPage, true);

                    Session["SHOW_IMAGE_PHRASE"] = null;
                    if (not_main)
                    {
                        new csControl().execScript(this.Page, "doOther", "close_frame_dialog();");
                    }
                    else
                    {
                        new csControl().execScript(this.Page, "doOther", "goto_main();");
                    }
                }

                string questionHTML = "";

                for (int j = 0; j < groups.Length; j++)
                {
                    questionHTML += "<tr onclick='showPad(\"" + j + "\");'>"
                                    + "<td id=\"registerQuestionsLabel" + j + "\" class=\"registerQuestionsLabel\">"
                                    + "        " + (j + 1) + ")"
                                    + "   </td>"
                                    + "   <td id=\"registerQuestionsInput" + j + "\" class=\"registerQuestionsInput" + (j == 0 ? "Selected" : "") + "\" valign='middle'>"
                                    + "      <select class='registerQuestionsSelect' name='question" + j + "' id='question" + j + "' onfocus='showPad(\"" + j + "\");'"
                                    + "          onchange='" + padName + j + ".setFocus();'>"
                                    + "          <option value=''>Select One</option>";

                    int len = groups[j].Count;

                    for (int i = 0; i < len; i++)
                    {
                        VCryptQuestion question = groups[j][i];

                        string questionText = question.Question;

                        long questionId = question.QuestionId;

                        questionHTML += "<option value='" + questionId + "'>" + HttpUtil.encode(questionText) + "</option>";
                    }

                    questionHTML += "</select>" + "</td>"
                                    + "<td id=\"registerQuestionsArrow" + j + "\" class=\"registerQuestionsArrow" + (j == 0 ? "Selected" : "") + "\" >"
                                    + "        &nbsp;"
                                    + "    </td>"
                                    + "</tr>";

                    if (j != (groups.Length - 1))
                    {
                        questionHTML += "<tr><td colspan='2' style='height:65px'></td></tr>";
                    }
                }

                this.listQuestions.Text = questionHTML;

                string questionpads = "";

                for (int i = 0; i < groups.Length; i++)
                {
                    AuthentiPad pad = appUtil.CreateQuestionPad(Session, padName + i, false);
                    sessionData[pad.PadName] = pad;
                    questionpads += client.getAuthentiPadHTML(pad, false, false);
                }

                this.listTextpads.Text = questionpads;

                sessionData["questionGroupCount"] = groups.Length.ToString();
                sessionData["questionError"] = "";
            }

            BharosaTrace.Exit("RegisterQuestionsPage.Page_Load()");
        }
    }
}
