<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EPSCOProcessing.aspx.cs" Inherits="IRSWorld.EPSCO.EPSCOProcessing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EPSCO Pre-Processing</title>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>
    <script src="../js/EPSCO/jsEPSCOTransactions.js" type="text/javascript"></script>
    <script src="../js/EPSCO/jsEPSCOProcessing.js" type="text/javascript"></script>
    <script src="../js/jsExpand.js" type="text/javascript"></script>
    <script src="../js/jsBalloon.js" type="text/javascript"></script>
    <script src="../js/jsImageResize.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
       
        var epsco_proc = new epsco_proc();
        
        function execute_ajx_command(param)
        {
            if (epsco_proc != null && epsco_proc != "undefined")
            {
                epsco_proc.do_ajaxgridview(param);       
            } 
        }
        function execute_ajaxgridview_command(param)
        {
            if (epsco_proc != null && epsco_proc != "undefined")
            {
                epsco_proc.do_ajaxgridview(param);       
            }     
        }
    </script>
    
</head>
<body onload="javascript:epsco_proc.page_initialize();">
    <form id="form1" runat="server">
    <div id="divPage" style="overflow:auto;margin:auto;">
                <div id="divPreProcessing" style="position:absolute;color:darkblue;padding:10px;font-family:Arial;width:100%;height:90%;display:none; left: 9px; top: 373px;overflow:auto;"> 
                        <table cellpadding="0" cellspacing="0" style="width:913px;margin:auto;">
                            <tr>
                                <td style="width:60%; vertical-align:bottom" align=left>
                                    <table style="width:100%" cellpadding="0" cellspacing="0">                            
                                        <tr>
                                            <td align="left" valign="bottom" style="width:11px; border-collapse:collapse;height:11px;"><img src="../images/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="width:40%;height:11px;">EPSCO Pre-Processing</td>
                                            <td align="left" style="width:60%;height:11px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>    
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td  style="width:100%;height:91%"> <!--colspan="3"-->
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;width:100%;height:100%">
                                        <tr >
                                            <td style="height: 35px; width: 560px;">
                                                Date Uploaded &nbsp;From: 
                                                <input id="txtUploadDate1" type="text" readonly="readonly" style="width: 110px" maxlength="10" onchange="javascript:epsco_proc.populate_data();"/>
                                                <img id="calUploadDate1" align="absMiddle" onclick="javascript:popUpCalendar(this, document.getElementById('txtUploadDate1'), 'mm/dd/yyyy', document.getElementById('divPreProcessing'));" src="../images/calendar/imgcalen.png" />
                                                To:
                                                <input id="txtUploadDate2" type="text" readonly="readonly" style="width: 110px" maxlength="10"/ onchange="javascript:epsco_proc.populate_data();">
                                                <img id="calUploadDate2" align="absMiddle" onclick="javascript:popUpCalendar(this, document.getElementById('txtUploadDate2'), 'mm/dd/yyyy', document.getElementById('divPreProcessing'));" src="../images/calendar/imgcalen.png" />&nbsp;
                                                Amt. exceeded from $5000:<input type="checkbox" style="width: 19px; height: 15px;" id="chkTrapped" onclick="javascript:epsco_proc.populate_data();" />&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td id="tdTransactions" valign="middle" align="center">
                                                <div id="grdTransactions" style="height:60%;width:100%;"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="height: 50px">
                                                <input id="btnSubmitTransactions"  class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" onclick="javascript:epsco_proc.insert_into_remittance();" type="button" value="Submit Transactions" style="width: 153px" />
                                            </td>
                                        </tr>
                                     </table>
                                </td>
                             </tr>
                             <tr><td colspan="3" style="width: 11px">&nbsp;</td></tr>
                        </table>
                    </div>
    </div>
    </form>
</body>
</html>
