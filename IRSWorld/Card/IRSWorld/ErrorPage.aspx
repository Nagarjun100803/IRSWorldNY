<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="IRSWorld.ErrorPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="./css/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%" cellspacing="0" cellpadding="0">
            <tr>
                <td class="content_height" valign="middle" align="center">
                    <table width="70%">
                        <tr>
                            <td width="100%" align="center" valign="middle" style="border-collapse: collapse;">
                                <asp:Label ID="lblErrMsg" runat="server" Text="..." style="font-weight: bold; font-size: 14pt; color: red; font-family: Arial"></asp:Label></td>
                        </tr>
                    
                        <tr>
                            <td width="100%" align="center" valign="middle">
                                <asp:Label ID="lblEndMsg" runat="server" Text="IRSWORLD SESSION WAS TERMINATED." style="font-weight: bold; font-size: 2em; color: maroon; font-family: Arial"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" value="Login" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button'"  onclick="document.irs_login.submit();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <form name="irs_login" id="irs_login" method="post" action="./Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_self"></form>
</body>
</html>
