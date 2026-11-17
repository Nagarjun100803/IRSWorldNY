<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileExtractMonitor.aspx.cs" Inherits="IRSWorld.EOD.FileExtractMonitor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Extracted File Inquiry</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />    
    <script src="../js/js_sidebar.js" type="text/javascript"></script>       
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jscommons.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/EOD/FileExtractMonitor.js" type="text/javascript"></script>    
    <script src="../js/Reports/js_Reports_LookupUI.js" type="text/javascript"></script>    
    
</head>
<body onload="javascript:fem.init_state();">
 <div  style="overflow:auto ;">
   
        <form id="form1" runat="server" method="post" >
        <table cellspacing="30" cellpadding="6" >
            <tr>
            <td valign="top" nowrap> &nbsp;<div id="divFileUploadSideBar"></div></td>
            <td height="250">&nbsp;
            <div >
                <table cellspacing="0" cellpadding="0" style="width:100%; " align="center">
                    <tr>
                        <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                            <img src="../images/toe.GIF" /></td>
                        <td nowrap align="left" class="table_head_dark" width="20%"></td>
                        <td nowrap align="left" valign="bottom" class="bg_body" style="">
                            <img align="absbottom" src="../images/rend.gif" />&nbsp;</td>
                    </tr>
                    <tr>
                        <td nowrap colspan="3" style="" align="left">
                        <div id="tblHolder" >
                            <table style=" border:solid 3px #336699; background:Azure; " cellpadding="0" cellspacing="0">
                                <tr style="height:auto">
                                    <td  align="center" valign="top">                                  
                                        <table style="" cellpadding="4" cellspacing="4">
                                            <tr>
                                                <td style="width:100%;" align="center" valign="middle" colspan="0">
                                                    <div id="divUploadFile" style="width:576px; vertical-align:middle; border:solid 3px midnightblue; " align="center">
                                                        <table style="width:100%;" cellpadding="2" cellspacing="2">
                                                            <tr>
                                                                <td valign="middle" style="border:solid 1px #336699; height: 265px;">
                                                                    <table style="width:100%; background:Azure; height: 160px;" cellspacing="2" cellpadding="2">
                                                                        <tr>
                                                                            <td align="left" class="table_head_dark"  style="height: 10px" colspan="2">
                                                                                Extracted File</td>
                                                                        </tr>                                                                        
                                                                        <tr>
                                                                            <td nowrap  class="cell_caption" style="width:20%; height: 26px;">Extract Type:</td>
                                                                            <td nowrap style="height:26px;width:70%;text-align:left;">
                                                                            <select id="ddExtractType">          </select></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" nowrap="nowrap" style="width: 20%; height: 26px">
                                                                                Status:</td>
                                                                            <td nowrap="nowrap" style="width: 70%; height: 26px; text-align: left">
                                                                                <select id="ddStatus" onpropertychange="javascript:fem.load_extracted_file();">
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        
                                                                        <tr>
                                                                            <td class="cell_caption" nowrap="nowrap"  style="width:20%">
                                                                                Date Extracted:</td>
                                                                            <td nowrap="nowrap" style="height:20px;width:70%;text-align:left;">
                                                                                <input id="txtDate" disabled="disabled" style="width: 24%" type="text"  />
                                                                                <img id="imgCaloverride" onclick="popUpCalendar(this, document.forms[0].txtDate, 'mm/dd/yyyy');"
                                                                                    src="../images/calendar/imgcalen.png" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2" style="height: 150px" valign="top">                                                                                
                                                                                <div id="divGridHolder" style="width:auto; vertical-align:middle; border:solid 3px midnightblue;  display:none; background:Azure">
                                                                                    &nbsp;</div>
                                                                                <br />
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
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
                        </td>
                    </tr>
                </table>
            </div>
            </td>
            </tr>
        </table>
        </form>
</div>
</body>
</html>