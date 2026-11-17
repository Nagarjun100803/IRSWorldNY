<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TBG_MAINTENANCE.aspx.cs" Inherits="IRSWorld.GRS.TBG_MAINTENANCE" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>TBG Punla Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script src="../js/GRS/js_TBGMaintenance.js" type="text/javascript"></script>
    <script type="text/javascript">
           var TBG_Maintenance = new TBG_Maintenance();
    </script>
</head>
<body onload="javascript:TBG_Maintenance.Load_RecordID()">
    <form id="form1" runat="server">
     <div align = "center" style="width: 850px; height: auto">
    
        <table>
        <tr>
            <td id="tdSideBar_TBGmaint" style="width: 200px; vertical-align:text-top">
                 <div id="divSideBar_TBGmaint">
                 </div>
            </td>
            
            <td style="width:650px;">
                <table width="100%"  cellspacing="0" cellpadding="0" >
               
                    <tr>
                        
                        <td align="left" valign="bottom" style="width:1%;height:24px;">
                            <img src="../images/toe.GIF" /></td>
                        <td id="capResult" align="left" class="table_head_dark" style="width:50%; height: 24px; border-collapse:collapse;">TBG Punla Maintenance</td>
                        <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                            <img align="absbottom" src="../images/rend.gif" /></td>
                    </tr>
                    
                    <tr>
                        
                        <td colspan="3" style="width: 100%; height: 100%">
                            <table style="width:100%; height: 100%; background:azure;" class="table_border">
                                <tr>
                                    <td nowrap="nowrap" style="width: 30%; height: 20px; text-align: right" class="cell_caption">Record ID&nbsp;</td>
                                    <td> <select id = "ddRecordID" style="width: 99%" onchange="javascript:populate_rec();"></select></td>
                                </tr>
                            </table>
                            <div id="divTBGmaint">
                                <table id="tblTBGmaint" style="width: 100%;" class="table_border">
                                    <tr>
                                        <td id="tdTBGmaint" style="width: 100%; height: 21px;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
