<%@ Page Language="C#" MasterPageFile="~/Admin/Default.master" AutoEventWireup="true" Inherits="IRSWorld.Admin.LoginJumpPage" Title="Loading" Codebehind="LoginJumpPage.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="1" height="1" id="flash" align="middle">
        <param name="allowScriptAccess" value="sameDomain" />
        <param name="movie" value="../bharosa_web/flash/bharosa.swf" />
        <param name="quality" value="low" />
        <param name="bgcolor" value="#ffffff" />
        <param name="FlashVars" value="dcUrl=CookieManager.aspx"/>
        <embed src="../bharosa_web/flash/bharosa.swf" flashvars="dcUrl=CookieManager.aspx"
               quality="low" bgcolor="#ffffff" width="1" height="1" name="flash" align="middle"
               allowscriptaccess="sameDomain" type="application/x-shockwaveflash"/>
    </object>
        <table style="width: 755px; height: 175px">
            <tr>
                <td style="width: 232px">Logging in. Please wait...</td>
            </tr>
        </table>
    <script type="text/javascript">
    <!--
    // redirect to LoginHandlerPage after waiting for one second
    //window.setTimeout("window.location = 'LoginHandlerPage.aspx?js_closer=login_close';", 1000);
    //-->
    </script>
</asp:Content>

