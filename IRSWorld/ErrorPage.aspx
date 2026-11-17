<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="IRSWorld.ErrorPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>SYSTEM MESSAGE</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="./css/reskin.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function fix_size()
        {
            if (sessionStorage.getItem("ErrorMessage") != null || sessionStorage.getItem("ErrorMessage") != undefined) {
                document.getElementById("lblErrMsg").innerHTML = sessionStorage.getItem("ErrorMessage");
            }
            else {
                document.getElementById("lblErrMsg").innerHTML = "Session Closed!";
            }
            div = document.getElementById("divMain");
            div.style.height = document.documentElement.clientHeight + 'px';
            div.style.width = document.documentElement.clientWidth + 'px';
            sessionStorage.removeItem("ErrorMessage");
        }
        
        function do_login_action()
        {
            sessionStorage.removeItem("ErrorMessage");
            if (document.getElementById("hidPath")) {
                //alert('hidpath: ' + document.getElementById("hidPath").value);
                if (document.getElementById("hidPath").value.length > 0) {
                    irs_login.action = document.getElementById("hidPath").value;
                    document.irs_login.submit();
                }
            }
        }
        
    </script>
</head>
<body onload="fix_size();" onresize="fix_size();">
    <form id="form1" runat="server">
    <div id=divMain class=content_height>
        <table style="width:100%; height:100%" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width:100%; height:100%; vertical-align:middle" valign="middle" align="center">
                    <table style="width:70%">
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
                            <td style="width:100%;" align=center>
                                <input type="button" value="Login" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button'" onclick="do_login_action();" /> <!--onclick="document.irs_login.submit();" /> -->
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input id="hidPath" type="hidden" runat=server value="./Main/IRSMain.aspx?load=../Admin/Login.aspx" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <form name="irs_login" id="irs_login" method="post" action="IRSWorldNY/Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_self"></form>
</body>
</html>
