k<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Batch_EOT.aspx.cs" Inherits="IRSWorld.GRS.EOT.Batch_EOT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Batch End of Transaction</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../../js/jsCommons.js" type="text/javascript"></script>
    <script src="../../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../../js/js_sidebar.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div align = "center" style="width: 550px; height: auto">
    
        <table>
        <tr>
            <td id="tdSideBar_BatchEOT" style="width: 200px; vertical-align:text-top">
                 <div id="divSideBar_BatchEOT">
                 </div>
            </td>
            
            <td style="width:350px;">
                <table width="100%"  cellspacing="0" cellpadding="0" >
               
                    <tr>
                        
                        <td align="left" valign="bottom" style="width:1%;height:24px;">
                            <img src="../../images/toe.GIF" /></td>
                        <td id="capResult" align="left" class="table_head_dark" style="width:50%; height: 24px; border-collapse:collapse;">Batch End of Transfer</td>
                        <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                            <img align="absbottom" src="../../images/rend.gif" /></td>
                    </tr>
                    
                    <tr>
                        
                        <td colspan="3" style="width: 100%; height: 100%">
                            <table style="width:100%; height: 100%; background:azure;" class="table_border">
                                <tr>
                                    <td nowrap="nowrap" style="width: 40%; height: 20px; text-align: right" class="cell_caption">Date of Transfer:&nbsp;</td>
                                    <td > <input type = "text" id = "txtDateTransfer" style ="width: 99%" /></td>
                                </tr>
                                
                                <tr>
                                    <td nowrap="nowrap" style="width: 40%; height: 20px; text-align: right" class="cell_caption">File Indicator:&nbsp;</td>
                                    <td > <input type = "text" id = "txtFileIndicator" style ="width: 99%" /></td>
                                </tr>
                               <tr><td colspan="2"><table align="center" style="width: 50%">
                                <tr>
                                    <td style="width: 12%" align="right"><input type = "checkbox" id="chkReload" style="width:50%" /></td><td nowrap="nowrap" style="width: 28%; text-align: left" class="cell_desc">&nbsp;Reload</td>
                                </tr>
                                
                                <tr>
                                   <td style="width: 12%" align="right"><input type = "checkbox" id="chkReupload" style="width:50%" /></td><td nowrap="nowrap" style="width: 28%; text-align: left" class="cell_desc">&nbsp;Reload / Reupload</td>
                                </tr>
                                </table>
                                <tr><td align="right" style="width:50%"><input type = "button" value="OK" id="btnOK" style="width:50%" /></td>
                                <td align="left" style="width:50%"><input type="reset" value="Cancel" id="btnCancel" style="width:50%" /></td>
                               </tr>
                                </td></tr> 
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
