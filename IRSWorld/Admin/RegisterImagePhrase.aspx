<%@ Page Language="C#" MasterPageFile="Default.master" AutoEventWireup="true"
    Inherits="IRSWorld.Admin.RegisterImagePhrase" Title="Sign On" Codebehind="RegisterImagePhrase.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <input type="hidden" name="__EVENTTARGET" id="__EVENTTARGET" value="" />
    <input type="hidden" name="__EVENTARGUMENT" id="__EVENTARGUMENT" value="" />
    <div id=divPageBody class="bharosaPageBody" style="overflow:auto">

        <script type="text/javascript">
            <!--
            var theForm = document.forms['aspnetForm'];
            if (!theForm) {
                theForm = document.aspnetForm;
            }
            
            function __doPostBack(eventTarget, eventArgument) {
                if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                    theForm.__EVENTTARGET.value = eventTarget;
                    theForm.__EVENTARGUMENT.value = eventArgument;
                    theForm.submit();
                }
            }
        
            function validateFields() {
                return true;
            }
            
            function goto_main()
            {
                top.load_center();
            }
            //-->
        </script>

        <div class="bharosaPageTitle">
            Your Security Device
        </div>
        <div class="bharosaPageMessage">
            This is your personalized virtual authentication device.<br />
            From now on, never enter your password unless you see this exact device.
        </div>
        <div class="registerAuthenticatorImage">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <asp:Label ID="lblDemoAuthenticator" runat="server" Text="DemoAuthenticator"></asp:Label>
                    </td>
                    <td>
                        <span class="demoAuthenticatorDivider">
                            <img src="../bharosa_web/images/device_arrows.gif" />
                        </span>
                    </td>
                </tr>
            </table>
            <div class="demoAuthenticatorLabel">
            </div>
            <table cellpadding=0 cellspacing=0 border=0>
                <tr>
                    <td align=left style="width:30%; display:none">
                        <div class="demoAuthenticatorMessage">
                            <asp:Label ID="LinkWhatsThis" runat="server"></asp:Label>
                        </div>                    
                    </td>
                    <td align=left style="width:50%">
                        <div class="demoAuthenticatorMessage">
                            Get a new <a id="A1" runat="server" href="javascript:__doPostBack('ChangeImageAndCaption', '');"
                                class='redLinkBold'>image and phrase</a>
                        </div>
                    </td>
                    <td align=right style="width:50%">
                        <div class="demoAuthenticatorMessage">
                            <asp:Label ID="lblSetPad" runat="server" Text=""> </asp:Label><br />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="registerSubmit">
            To accept this security device, image and phrase click >>
            <asp:Button ID="register" CssClass="submitButton" Text="Continue" runat="server"
                OnClick="register_Click" />
        </div>
    </div>
</asp:Content>
