<%@ Page Language="C#" MasterPageFile="Default.master" AutoEventWireup="true" Inherits="IRSWorld.Admin.ChallengeUser" Title="Sign On" Codebehind="ChallengeUser.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="bharosaPageBody">
        <form name="loginForm" id="loginForm" autocomplete="off" onsubmit="javascript:return validateFields();">

        <script language="javascript" type="text/javascript">
<!--

function validateFields() {
if (Bharosa_QuestionPad) {
var elem = Bharosa_QuestionPad.dataField;
if (elem) {
var answer = elem.value;
answer = trimstring(answer);
elem.value = answer;
if (answer == '') {
alert("Please enter the answer.");
elem.focus();
return false;
}
}
}
return true;
}

whatsThisName="QP_whatsthis";
// -->
        </script>

        <div class="bharosaPageTitle">
            Your Security Device
        </div>
        <div class="bharosaPageMessage">
            For your safety please answer the following security question.
        </div>
        <asp:PlaceHolder ID="ErrorPlaceHolder" Visible="false" runat="server">
            <div class="loginError" title="Sorry, the identification you entered was not recognized. Please try again.">
                <img height="7" alt="Error" src="../bharosa_web/images/red_square.gif" style="width:7; vertical-align:top" align="top" border="0" />
                    <!--valign="top" />-->
                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
            </div>
        </asp:PlaceHolder>
        <div class="challengeUser">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:Label ID="textpad" runat="server" Text="Label"></asp:Label>

                        <script language="javascript" type="text/javascript">

        if (Bharosa_QuestionPad && Bharosa_QuestionPad != undefined) {
            Bharosa_QuestionPad.reset();
            Bharosa_QuestionPad.setFocus();
            Bharosa_QuestionPad.customClickEnter = function f() {
            return validateFields();
            }
        }
        // -->
                        </script>

                        <ul class="links" style="visibility:hidden">
                            <li><a href="javascript:infoWindow('html/notImage.html');" title="Not your image and phrase?">
                                Not your image and phrase?</a></li>
                            <li><a href="javascript:helpWindow('html/QP_whatsthis');" title="What's this?">What's
                                this?</a></li>
                            <li><a href="javascript:infoWindow('html/forgotanswer.html');" title="Forgot Answer?">
                                Forgot Answer?</a></li>
                        </ul>
                    </td>
                </tr>
            </table>
        </div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server">
        <div class="registerSubmit">
           <asp:CheckBox ID="registerDevice" runat="server" Text="Check to Register computer / device that you are currently using as a safe device"
                TextAlign="left" />
        </div>
           </asp:PlaceHolder>
        </form>
    </div>
</asp:Content>
