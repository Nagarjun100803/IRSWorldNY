<%@ Page Language="C#" MasterPageFile="default.master" AutoEventWireup="true"
    Inherits="IRSWorld.Admin.RegisterQuestionsPage" Title="Sign On" Codebehind="RegisterQuestionsPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="bharosaPage">
        <script type="text/javascript">
            function goto_main()
            {
                top.load_center();
            }
        </script>
        <div class="bharosaPageBody">
            
            <div class="bharosaPageTitle">
                Security Questions
            </div>
            
            <div class="bharosaPageMessage">
                We will use your security questions and answers to confirm your identity at times
                when extra safety is needed.
            </div>
            
            <div class="registerQuestions">
            
                <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
            
                    <tr>
                        <td valign="top">
                            <span class="textheadings" style="margin-left: 30px;"><b>Questions</b></span> <span
                                class="textmed">(Choose a question from each below)</span>
                        </td>
            
                        <td valign="top" id="utilContainer">
                            <span class="textheadings" style="margin-left: 30px;"><b>Answers</b></span></td>
                    </tr>
            
                    <tr>
                        <td valign="top">
            
                            <table style="width:100%" cellpadding="0" cellspacing="0" border="0">
                                <asp:Label ID="listQuestions" runat="server" Text="Label"></asp:Label>
                            </table>

                        </td>

                        <td valign="top">
                            <asp:Label ID="listTextpads" runat="server" Text="Label"></asp:Label>
                        </td>

                    </tr>

                </table>

                <script language="javascript" type="text/javascript">
  <!--
                <% 
                    int num = Int32.Parse((string)IRSWorld.bharosa.AppSessionData.GetInstance(Session)["questionGroupCount"]);

                    string padName = Bharosa.VCrypt.Common.Util.BharosaConfig.get("register.questions.questionpad.name", "Bharosa_Register_Pad");

                    for (int i = 0; i < num; i++)
                    {
                        string answerPad = padName + i;
                    %>

                            <%= answerPad %>.customClickEnter = function f() { 
                     
                            onClickEnterTP('<%= i %>'); 
                            
                        }; // end of function f()
                        
                     <%   if (num > 0) {  %>
                     
                            <%= answerPad %>.hide();
                            
                     <%   } // end of if
                     
                    } // end of for loop
                %>

  var maxPinLength = <%=Bharosa.VCrypt.Common.Util.BharosaConfig.getInt("max.pin.length", 33)%>;
  var minPinLength = <%=Bharosa.VCrypt.Common.Util.BharosaConfig.getInt("min.pin.length", 3)%>;

  // -->
                </script>

            </div>

            <script language="javascript" type="text/javascript">
  <!--
  var num = <%=Int32.Parse((string) IRSWorld.bharosa.AppSessionData.GetInstance(Session)["questionGroupCount"]) %> 
  focusIndex = '0';

  if (num > 0) {
    document.getElementById("question0").focus();
  }

  function validateFields() {
    return validateSecurityQAforTP();
  }

  // -->
            </script>

        </div>
        
    </div>
    <asp:PlaceHolder ID="registerSubmitPlaceHolder" runat="server">
    <div class="registerSubmit">
        <asp:CheckBox ID="registerDevice" runat="server" Text="Check to Register computer / device that you are currently using as a safe device"
            TextAlign="left" />
    </div>
    </asp:PlaceHolder>
</asp:Content>
