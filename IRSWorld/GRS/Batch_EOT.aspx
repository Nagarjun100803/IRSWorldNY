<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Batch_EOT.aspx.cs" Inherits="IRSWorld.GRS.Batch_EOT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Batch End of Transaction</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script src="../js/GRS/js_BatchEOT.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script type="text/javascript">
        var hide_show = new hide_show();

    </script>
</head>
<body onload="javascript:page_load();">
    <form id="form1" runat="server">
    
 
    
    <div id="divImgLoad" style="z-index:101;position:absolute;left:690px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 310px;">
	    <table width="40%" cellpadding="0" cellspacing="6">
		    <tr>
		    <td class="cell_desc" align="left" style="font-size: 12px; width: 30%;">
			    <label id="lblLoad">Loading...</label></td>
		    </tr>
		    <tr>
		    <td>
			    <center><div id="divLoading"></div></center>
		    </td>
		    </tr>
        </table>
    </div>
    
    <div id="divBatchEOT" align = "center" style="width: 650px; height: auto">
    
        <table>
        <tr>
            <td id="tdSideBar_BatchEOT" style="width: 300px; vertical-align:text-top">
                 <div id="divSideBar_BatchEOT">
                 </div>
            </td>
            
            <td style="width:350px;">
                <table width="100%"  cellspacing="0" cellpadding="0" >
               
                    <tr>
                        
                        <td align="left" valign="bottom" style="width:1%;height:24px;">
                            <img src="../images/toe.GIF" /></td>
                        <td id="capResult" align="left" class="table_head_dark" style="width:50%; height: 24px; border-collapse:collapse;">Batch End of Transfer</td>
                        <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                            <img align="absbottom" src="../images/rend.gif" /></td>
                    </tr>
                    
                    <tr>
                        
                        <td colspan="3" style="width: 100%; height: 100%">
                            <table style="width:100%; height: 100%; background:azure;" class="table_border">
                                <tr>
                                    <td nowrap="nowrap" style="width: 40%; height: 20px; text-align: right" class="cell_caption">Date of Transfer:&nbsp;</td>
                                    <td nowrap="nowrap" align="left"><input type = "text" id = "txtDateTransfer" style ="width: 50%" disabled="disabled" />&nbsp;<input type="checkbox" id="chkOverride" style="width:15%" onclick="javascript:hide_show.override();" />
                                            <label style="font-size:smaller; font-family:Arial; font-style:italic; color:Red">*override</label></td>
                                    <td align="left" style="width:5%" ><img id="imgCalendar" src="../images/calendar/imgcalen.png" style="display:none" onclick="popUpCalendar(this, document.forms[0].txtDateTransfer, 'mm/dd/yyyy');"/>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td nowrap="nowrap" style="width: 40%; height: 20px; text-align: right" class="cell_caption">File Indicator:&nbsp;</td>
                                    <td align="left"> <input type = "text" id = "txtFileIndicator" style ="width: 50%" disabled="disabled" /></td>
                                    <td></td>
                                </tr>
                                <tr id="trOverrideDate" style="display: none">
                                    <td nowrap="nowrap" style="width: 40%; height: 20px; text-align: right" class="cell_caption">Override Date:&nbsp;</td>
                                    <td nowrap="nowrap" align="left"><input type = "text" id = "txtOverrideDate" style ="width: 50%;" disabled="disabled" />
                                        <img id="imgCaloverride" src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtOverrideDate, 'mm/dd/yyyy');"/>
                                    </td>
                                </tr>
                                <%--SUMMARY --%>
                                
                                <tr>
                            
                                    <td colspan="3" style="width: 100%; height: 100%">
                                        <table id="tblEOTsummary" style="width:100%; height: 100%; background:azure; display: none" class="table_border">
                                            <tr>
                                                <td id="tdEOTsummary" style="width:100%"></td>
                                                
                                            </tr>
                                        </table>
                                        
                                    </td>
                                </tr>
                                
                                <tr align="center" id="trBack" style="display:none">
                                    <td colspan="3" align="center"><input type="button" value="Back" style="width:25%" onclick="history.go()"/></td>
                                </tr>
                                    
                                <%--CHECKBOX --%>
                               <tr><td colspan="3">
                                    <table id="tblChkbox" align="center" style="width: 50%; ">
                                        <tr>
                                            <td style="width: 20%" align="right"><input type = "checkbox" id="chkReload" value = "chkReload" onclick="javascript:hide_show.chkbox();" style="width:30%" /></td>
                                            <td style="width: 20%; text-align: left" class="cell_desc">&nbsp;Reload</td>
                                        </tr>
                                        
                                        <tr id="trReupload" style="display: none">
                                           <td id="tdChkReupload" style="width: 20%; " align="right"><input type = "checkbox" id="chkReupload" style="width:30%; " /></td>
                                           <td id="tdReupload" style="width: 20%; text-align: left; " class="cell_desc">&nbsp;Reupload</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%" align="right"><input type="button" value="OK" id="btnOK" onclick="javascript:saveStatus();" /></td>
                                            <td style="width: 20%" align="left"> <input type="button" value="Cancel" id="btnCancel" onclick="history.go()" /></td>
                                       </tr>
                                    </table>
                                
                                <%--BUTTONS FOR REPORT --%>
                                <table id="tblBtnReport" align="center" style="width:100%; display:none">
                                    <tr>
                                        <td style="width:50%" align="center"><input id="btnATE" type="button" value="View ATE Proofsheet" /></td>
                                        <td style="width:50%" align="center"><input id="btnTOSD" type="button" value="View TOSD Proofsheet" /></td>
                                    </tr>
                                    
                                    <tr>
                                        <td style="width:50%" align="center"><input id="btnGEFU" type="button" value="View GEFU Proofsheet" /></td>
                                        <td style="width:50%" align="center"><input id="btnBack" type="button" value="Back" onclick="history.go()"/></td>
                                    </tr>
                                
                                </table>
                                
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
