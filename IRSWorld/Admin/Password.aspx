<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Password.aspx.cs" Inherits="IRSWorld.Admin.Password" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IRS Password Verification Page</title>
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    </style>
    <script src="../js/commons_min.js" type="text/javascript"></script>  
    <script src="../js/Admin/Password_min.js" type="text/javascript">
    </script>  
</head>
<body onload="page_setup();" onresize="fix_height();" >
    <center>
    <form id="form1" runat="server">
    <div id="divPasswd" style="width: 100%; height:expression(document.documentElement.availHeight); display:block; vertical-align:middle" class="opacified">
    <table style="width:100%;height:100%" cellpadding="0" cellspacing="0">
        <tr>
            <td id="tdContent" style="width: 100%; height: 100%; vertical-align:middle;" align="center" valign=middle class="opacified">
                <table id="tblContent" cellspacing="0" cellpadding="0" style="width:42%; height: 30%;" class="opacified">
                    <tr>
                        <td id="tdTitle" align="left" class="table_head_dark" style="width:96%; height: 24px; border-collapse:collapse">
                            Password Verification</td>
                        <td align="left"  style="width: 20px; height: 24px; " class="table_head_dark"> 
                            <input type="button" value="x" onclick="pwd_cancel(this);" style="width:100%; color: #FF7F50; text-align: center; background-color:Transparent;" />
                        </td>                    
                    </tr>
                    <tr>
                        <td colspan="2" style="background: white" align=center>
                            <table cellpadding=0 cellspacing=0 style="width:100%;height:100%" class="table_border">
                                <tr>
                                    <td style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%" align=center>
                                        <!-- content starts here -->
                                        <table cellspacing=0 cellpadding=0 style="width:100%">
                                            <tr>
                                                <td align=center>
                                                    <!--
                                                    <div style="width: 96%;">
                                                      <b class="rr">
                                                      <b class="rr1"><b></b></b>
                                                      <b class="rr2"><b></b></b>
                                                      <b class="rr3"></b>
                                                      <b class="rr4"></b>
                                                      <b class="rr5"></b></b>                                                                
                                                      <div class="rrfg">                                                
                                                    -->
                                                    <table cellpadding="0" cellspacing="5px" style="width:100%;"> <!-- border:solid 1px #87ceeb;"> -->
                                                        <tr id="trLoginPasswdRow" style="display:block;border-collapse:collapse">
                                                            <td id="tdLoginPasswd" style="width: 161px" class="cell_caption">Enter Password:&nbsp;
                                                            </td>
                                                            <td style="width: 400px" align="left">
                                                                <input id="txtPasswd" maxlength="256" type="password" style="width: 100%; height: 20px"  autocomplete="off"/>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNewPasswdRow" style="display:none; border-collapse:collapse">
                                                            <td id="tdNewPasswordRow" style="width: 161px" class="cell_caption">New Password:&nbsp;
                                                            </td>
                                                            <td style="width: 400px" align="left">
                                                                <input id="txtPasswdNew" maxlength="256" type="password" style="width: 100%; height: 20px"  autocomplete="off"/>
                                                            </td>
                                                        </tr>
                                                        <tr id="trConfirmRow" style="display:block; border-collapse:collapse">
                                                            <td id="tdConfirmRow" style="width: 161px" class="cell_caption">Confirm Password:&nbsp;
                                                            </td>
                                                            <td style="width: 400px" align="left">
                                                                <input id="txtPasswdConf" maxlength="256" type="password" style="width: 100%; height: 20px" autocomplete="off" />
                                                            </td>
                                                        </tr>
                                                    </table> 
                                                    <!--
                                                      </div>
                                                      <b class="rr">
                                                      <b class="rr5"></b>
                                                      <b class="rr4"></b>
                                                      <b class="rr3"></b>
                                                      <b class="rr2"><b></b></b>
                                                      <b class="rr1"><b></b></b></b>                                                                    
                                                    </div>                                                    
                                                    -->                                                         
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td align="center" class="cell_caption" style="text-align:center;"><b>Password must be 8-20 characters, case sensitive.</b></td>--%>
                                                <td align="center" class="cell_caption" style="text-align:center;"><b>Password must contain at least one digit, upper case, lower case, special and 8-20 characters.</b></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%; vertical-align:middle" align="center" valign="middle" colspan="2">
                                                    <table style="width:60%">
                                                        <tr>
                                                            <td align="left" style="width: 50%">
                                                                <input id="btnPasswdOk" type="button" value="Submit" class="button" style="width: 96px" onmouseover="this.className='button_over';" onmouseout="this.className='button'" onclick="pwd_submit(this);"/>
                                                            </td>
                                                            <td align="right" style="width: 50%">
                                                                <INPUT style="WIDTH: 96px" id="btnPasswdCancel" class="button" type=button value="Cancel"  onmouseover="this.className='warning_over';" onmouseout="this.className='button'" onclick="pwd_cancel(this);" />                                                            
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- content ends here -->
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>                                    
                            </table>
                        </td>
                    </tr>
                </table>            
            </td>
        </tr>
    </table>
    </div>
    </form>
    </center>
</body>
</html>
