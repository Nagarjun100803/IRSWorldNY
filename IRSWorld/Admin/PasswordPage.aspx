<%@ Page Language="C#" MasterPageFile="Default.master" AutoEventWireup="true"
    Inherits="IRSWorld.Admin.PasswordPage" Title="Sign On" Codebehind="PasswordPage.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="bharosaPageBody" style="margin-top:45px">
        <script language="javascript" type="text/javascript">
<!--

function validateFields() {
    if (Bharosa_PasswordPad) {
        var elem = Bharosa_PasswordPad.dataField;
        if (elem) {
            var password = elem.value;
            password = trimstring(password);
            elem.value = password;
            if (password == '' || password == ":") {
                if (Bharosa_PasswordPad.type = 'T_KEYPAD') return true;
                alert("Please enter the password.");
                elem.focus();
                return false;
            }
        }
    }
    return true;
}

whatsThisName="TP_whatsthis";
// -->
        </script>

        <div class="bharosaPageTitle">
            Sign In:
        </div>
        <div class="bharosaPageMessage">
            <asp:Label ID="lblInstruction" runat="server"></asp:Label>
        </div>
        <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align=center style="text-align:center;">
                    <asp:Label ID="lblPasswordPad" runat="server" Text="Label" style="text-align:center"></asp:Label>

                    <script language="javascript" type="text/javascript">
        try
        {
            if (Bharosa_PasswordPad && Bharosa_PasswordPad != undefined ) {
                Bharosa_PasswordPad.reset();
                Bharosa_PasswordPad.setFocus();
                Bharosa_PasswordPad.customClickEnter = function f() {
                return validateFields();
                }
            }
        }
        catch (e) {}
        // -->
                    </script>
                    <!--
                    <ul class="links" style="visibility:hidden">
                        <li>
                            <asp:Label ID="LinkWhatsThis" runat="server"></asp:Label>
                            </li>
                        <li><a href="javascript:infoWindow('html/notImage.html');" title="Not your image and phrase?">
                            Not your image and phrase?</a></li>
                    </ul>
                    -->
                    <br />
                    <br />
                </td>
            </tr>
        </table>
        <div id="divForgot" runat="server" style="text-align:center;">
            <asp:LinkButton ID="lnkForgot" style="font-family: Arial; font-size:12px; color:navy;" runat="server" OnClick="lnkForgot_Click">Forgot your Password?</asp:LinkButton>
            &nbsp;&nbsp<label style="font-family:Arial; font-style:italic; font-size:11px; color: navy">(Note: To get into this link, enter any character in the password box.)</label>
        <br />
        </div>
    </div>
</asp:Content>
