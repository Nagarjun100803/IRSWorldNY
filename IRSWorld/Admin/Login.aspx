<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="IRSWorld.Admin.Login" %>
<%@ Register Src="../Controls/MessageBoard.ascx" TagName="msg_board" TagPrefix="uc3" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>

<!DOCTYPE html> <!--PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"-->

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRSWorld Login Page</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default_min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .login_rr{display:block}
        .login_rr *
        {
          display:block;
          height:1px;
          overflow:hidden;
          background:dimgray;
        }
        .login_rr1
        {
          margin-left:3px;
          margin-right:3px;
          padding-left:1px;
          padding-right:1px;
          border-left:1px solid azure;
          border-right:1px solid azure;
          background:dimgray;
        }
        .login_rr2
        {
          margin-left:1px;
          margin-right:1px;
          padding-right:1px;
          padding-left:1px;
          border-left:1px solid #f1f3f4;
          border-right:1px solid #f1f3f4;
          background:dimgray;
        }
        .login_rr3
        {
          margin-left:1px;
          margin-right:1px;
          border-left:1px solid dimgray;
          border-right:1px solid dimgray;
        }
        .login_rr4
        {
          border-left:1px solid dimgray;
          border-right:1px solid dimgray;
        }
        .login_rr5
        {
          border-left:1px solid dimgray;
          border-right:1px solid dimgray;
        }
        .login_rrfg
        {
          background:dimgray;
        }
        .login_opacified
        {
	        background: dimgray;
	        filter: alpha(opacity=95);
	        -moz-opacity: 0.95;
	        opacity: 0.95;
	        -khtml-opacity: 0.95;
        }        
    </style>
    <script src="../js/commons_min.js" type="text/javascript"></script>
    <script src="../js/ipconfig_min.js" type="text/javascript"></script>
    <script src="../js/Admin/Login_min.js" type="text/javascript"></script>
</head>
<body onload="detect_setup();" onresize="fix_height();" onkeyup="setFocus();">
    <center>
    <form id="form1" runat="server">
    <div id=divContent style="width:100%; vertical-align:middle; background-image:url(../images/header/pnb_hands.gif); background-repeat:no-repeat; background-position: right;">
        <table style="width:100%" cellpadding="0" cellspacing="5">
            <tr>
                <td id="tdContent" align="center" style="width:100%; border-collapse:collapse; vertical-align:middle;"> <!--class="content_height"> -->
                    <uc3:msg_board ID="msgs" runat="server" style="width:80%" />
                    <div id='divmac' style="width: 100%; display: none;">
                        <table style="width:50%" cellpadding="0" cellspacing="5"  class="table_body">
                            <tr>
                                <td  class="table_caption" style="width:100%;" colspan="2">
                                    <span style="text-align: justify">
                                        The feature of this machine needs to be known to the system.
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align:right"><label>Machine Name:</label>&nbsp;
                                </td>
                                <td style="text-align: left;">
                                    <input id="txtmacname" type="text" style="width: 99%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align:right">
                                    <label>Mac Address:</label>&nbsp;</td>
                                <td><input id="txtmacaddr" type="text" style="width: 99%" /></td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align:right">
                                    <label>IP Address:</label>&nbsp;</td>
                                <td><input id="Text1" type="text" style="width: 99%" /></td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align: right">
                                    <label>location:</label>&nbsp;
                                </td>
                                <td>
                                    <select id="opt_location" style="width: 100%" >
                                    </select>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align: right">
                                    <label>Credit Card Reader</label>&nbsp;</td>
                                <td align="left">
                                    <input type="checkbox" style="width: 7%" />&nbsp;<span class="comment">Check if available.</span>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <input type="button" value="Submit" class="blue_button" style="text-align:center;" onclick="mach_submit();" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id='divlogin' style="width:100%; display: block; vertical-align:middle; background-color:Transparent; visibility:hidden">
                        <table style="width:100%" cellpadding=0 cellspacing=0>
                            <tr>
                                <td align="center" style="width: 100%;height:100%; vertical-align:middle">
                                    <table style="width:40%" cellspacing="0" cellpadding="0">                                                
                                        <tr>
                                            <td colspan="3" style="width: 100%;background:transparent;">
                                                <div style="width: 98%;background-color:transparent;">
                                                  <b class="login_rr">
                                                  <b class="login_rr1"><b></b></b>
                                                  <b class="login_rr2"><b></b></b>
                                                  <b class="login_rr3"></b>
                                                  <b class="login_rr4"></b>
                                                  <b class="login_rr5"></b></b>                                                                
                                                  <div class="login_rrfg">                                                                
                                                    <table cellpadding="0" cellspacing="5" class="login_opacified" style="width:100%; border:solid 1px dimgray;">
                                                        <tr>
                                                            <td align="left" style="width:100%">
                                                                <table style="width:100%" cellspacing=0 cellpadding=0>
                                                                    <tr>
                                                                        <td align=left style="width:50%;padding-left:10px">
                                                                            <img src="../images/header/log_icon.gif" align=absmiddle />
                                                                            <span style="color: Ivory; font-family:Arial; font-size: 14px; font-weight:bold; text-align:left;">
                                                                                Enter Access key
                                                                            </span>
                                                                        </td>
                                                                        <td align=right style="width:50%; padding-right:10px">
                                                                            <span id="spnVersion" style="color: Ivory; font-family:Arial; font-size: 11px; font-weight:bold; text-align:left;"></span>&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                            </td>
                                                        </tr>                                                        
                                                        <tr>
                                                            <td style="width:100%; height: 24px; font-size: 14px; text-align:left; padding-left: 20px; vertical-align:bottom">
                                                                <span style="color: Ivory; font-family:Arial; font-size: 11px; font-weight:bold; text-align:left;">
                                                                    User ID:
                                                                </span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <input id="txtUserId" type="text" style="font-size: 14px; width: 92%; height: 25px;" onpropertychange="close_msgboard();" />
                                                            </td>
                                                        </tr>
                                                        <tr>    
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td align="center">
                                                                <table style="width:60%">
                                                                <tr>
                                                                    <td style="width:50%" align="center">
                                                                        <input id="btnEnter" type="button" value="Enter" style="width: 120px" class="button" onclick="submit_userid(document.getElementById('txtUserId').value);" onmouseover="this.className='button_over';" onmouseout="this.className = 'button';" />
                                                                    </td>
                                                                    <td style="width:50%" align="center">
                                                                        <input id="btnCancel" type="button" value="Close" style="width: 120px" class="button" onclick="spawn_errpage('Login Closed.');" onmouseover="this.className='warning_over';" onmouseout="this.className = 'button';" />
                                                                    </td>                                                                                    
                                                                </tr>
                                                                </table>
                                                            </td>
                                                        </tr>                                                        
                                                        <%--<tr>
                                                            <td align="right">
                                                                <a href="../Internet/InternetRemitterEnrollment.aspx" style="color: Ivory; font-family:Arial; font-size: 14px; font-weight:bold; text-align:left; font-style:italic;">
                                                                    <span style="text-decoration: underline">Create An Online Account</span></a></td>
                                                        </tr>--%>
                                                    </table>
                                                  </div>
                                                  <b class="login_rr">
                                                  <b class="login_rr5"></b>
                                                  <b class="login_rr4"></b>
                                                  <b class="login_rr3"></b>
                                                  <b class="login_rr2"><b></b></b>
                                                  <b class="login_rr1"><b></b></b></b>                                                                    
                                                </div>                                            

                                            </td>
                                        </tr>
                                    </table>                                                            
                                </td>
                                <td id="tdmarky" align="right" style="height:100%; width: 64%; border-collapse:collapse;display: none;" rowspan="2">
                                    <marquee id='marky' direction="down"  style="width:60px; font-family:Arial; font-size:12px; text-align: center; color:navy; display: block; height: 100%;">
                                        <img align="absmiddle" src="../images/GlobalFilipino.gif" /><br />
                                        S&nbsp;<br />
                                        w&nbsp;<br />
                                        i&nbsp;<br />
                                        p&nbsp;<br />
                                        e&nbsp;<br />
                                        &nbsp;<br />
                                        c&nbsp;<br />
                                        a&nbsp;<br />
                                        r&nbsp;<br />
                                        d&nbsp;<br />
                                        </marquee>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id='divForgot' style="width: 100%; display: none;vertical-align:middle; background-color:Transparent;">
                        <table style="width:50%; border: solid 1px midnightblue; background-color:Transparent;" cellpadding="0" cellspacing="5"  class="table_body">
                            <tr>
                                <td  class="table_head_blue" style="width:100%; height: 24px;" colspan="2">
                                    <span style="text-align: left; padding-left: 13px; font-size: Small">
                                        Forgot your password?
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan=2 style="width: 30%; text-align:left; padding-left: 20px">
                                    <label class=cell_desc>Please select the email address where your password will be sent.</label>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align:left; height: 10px" colspan=2>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%; text-align: right">
                                    <label class=cell_desc>Email Address:</label>&nbsp;
                                </td>
                                <td style="text-align:left;">
                                    <select id="ddlEmail" style="width: 90%" >
                                    </select>
                                </td>
                            </tr>
                            <!--tr>
                                <td style="width: 30%; text-align: right">
                                    <label class=cell_desc>Other</label>&nbsp;</td>
                                <td align="left">
                                    <input id=ckEmail type="checkbox" style="width: 7%" />&nbsp;<span class="comment">Type the email address below.</span>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td><input type=text id=txtEmail style="width:90%" /></td>
                            </tr-->
                            <tr>
                                <td style="width: 30%; text-align:left; height: 10px" colspan=2>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="center">
                                    <input id="btnSend" type="button" value="Send" style="width: 120px" class="button" onclick="javascript:email_password();" onmouseover="this.className='button_over';" onmouseout="this.className = 'button';" />&nbsp;&nbsp;
                                    <input id="btnClose" type="button" value="Cancel" style="width: 120px" class="button" onclick="spawn_errpage('Password Mailing Canceled.');" onmouseover="this.className='warning_over';" onmouseout="this.className = 'button';" /></td>
                            </tr>
                        </table>
                    </div>                    
                    
                </td>
            </tr>        
        </table>
    </div>
    </form>
    </center>
</body>
</html>
