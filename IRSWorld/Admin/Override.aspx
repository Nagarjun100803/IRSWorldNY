<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Override.aspx.cs" Inherits="IRSWorld.Admin.Override" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Override dialog Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/commons.js" type="text/javascript"></script> 
    <script src="../js/Admin/jsOverride.js" type="text/javascript"></script>
    <script type="text/javascript"> 
    </script>    
</head>
<body onload="page_setup();" onkeyup="pwd_click();">
    <form id="form1" runat="server">
    <div id=divmain style="width:500px; height:200px; display:block;">
    <table style="width:100%; height: 100%"cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 100%; height: 100%;" align="center" valign="middle">
                <table id="tblContent" cellspacing="0" cellpadding="0" style="width:100%; height:30%; border-collapse:collapse">
                    <tr>
                        <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                            <img src="../images/toe.GIF" /></td>
                        <td id="tdTitle" align="left" class="table_head_dark" style="width:60%; height: 24px; border-collapse:collapse">
                            Override Password Verification</td>
                        <td align="left" valign="bottom" style="height: 24px"> 
                            <img align="absbottom" src="../images/rend.gif" /></td>                    
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 100%; height:100%; background: azure">
                            <table style="width:100%; height:100%" class="table_border">
                                <tr>
                                    <td style="width: 100%">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%">
                                        <!-- content starts here -->
                                        <table style="width:98%" cellpadding="0" cellspacing="5px">
                                            <tr style="display:block; border-collapse:collapse">
                                                <td style="width: 30%" class="cell_caption">
                                                    User ID:&nbsp;
                                                </td>
                                                <td style="width: 70%" align="left">
                                                    <input id="txtUserId" maxlength="256" type="text" style="width: 100%; height: 20px" />
                                                </td>
                                            </tr>
                                            <tr style="border-collapse:collapse">
                                                <td style="width: 30%" class="cell_caption"> Password:&nbsp;
                                                </td>
                                                <td style="width: 70%" align="left">
                                                    <input id="txtPasswd" maxlength="256" type="password" style="width: 100%; height: 20px" />
                                                </td>
                                            </tr>                                       
                                            <tr>
                                            <td style="height: 8px; width: 156px;"></td>
                                            <td></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%" align="center" valign="middle" colspan="2">
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
</body>
</html>
