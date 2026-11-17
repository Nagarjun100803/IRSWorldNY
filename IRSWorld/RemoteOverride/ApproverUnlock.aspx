<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproverUnlock.aspx.cs" Inherits="IRSWorld.RemoteOverride.ApproverUnlock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Unlock Items</title>
    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />    
    <script src="../js/jsCommons.js"                        type="text/javascript"> </script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript" > </script>
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"  ></script>
    
    <script src="../js/RemoteOverride/js_RemApproverUnlock.js" type="text/javascript"></script>
    
        <script type="text/javascript">
           var getBranch = new getBranches();
    </script>
    
</head>
<body onload="javascript:page_load();">
    <form id="form1" runat="server">
    <div style="width:100%">
         <table width="100%">
            <tr>
                <td style="vertical-align: top; text-align: center;">
                    <br />
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 65%;">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td align="left" class="table_head_dark" style="width: 70%; border-collapse: collapse;height: 24px">
                                Unlock Items</td>
                            <td align="left" class="bg_body" style="width: 171px; height: 24px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%; height: 100%">
                                <table class="table_border" style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="vertical-align: top; width: 60%; height: 44%">
                                            <div id="divForAdmin" style="width: 75%; background-color: azure; text-align: center; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px;">Select Branch : 
                                            <select id="ddlBranch" onchange="javascript:chngBranch();" style="width:75%"></select>
                                            </div>
                                            <!-- content starts here -->
                                             <div id="divApproverUnlock">
                                <table id="tblApproverUnlock" style="width: 100%;" class="table_border">
                                    <tr>
                                        <td id="tdApproverUnlock" style="width: 100%; height: 21px;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                                            <!-- content ends here -->
                                            
                                            <br />
                                            <input id="btnUnlock" onclick="javascript:unlock();" style="width: 15%" type="button"
                                                value="Unlock" />
                                            <input id="btnCancel" type="button" value="Cancel" style="width: 15%" /></td>
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
