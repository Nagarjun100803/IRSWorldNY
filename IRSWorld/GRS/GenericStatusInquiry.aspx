<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenericStatusInquiry.aspx.cs" Inherits="IRSWorld.GRS.GenericStatusInquiry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Generic Status Inquiry</title>
    <link href="../css/Default.css" rel="stylesheet"        type="text/css" />
    <script src="../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../js/js_sidebar.js"                       type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>    
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>    
    <script src ="../js/popcalendar.js"                     type="text/javascript"></script>
    <script src ="../js/GRS/js_GenericStatusInquiry.js"     type="text/javascript"></script>
</head>
<body onload="javascript: gsi.page_setup();">
    
        <center>
            <form id="form1" runat="server">
                <div id="divMainPanel" style="width: 100%">
                    <table cellpadding="0" cellspacing="5" height="100%" width="100%" align="left">
                        <tr>
                            <td align="left" class="" colspan="2" style="width: 64%; border-collapse: collapse"
                                valign="top">
                                <div style="display: block; width: 100%; height: 100%">
                                    <table style="width: 912px" >
                                        <tr>
                                            <td align="center" style="padding-top: 30px;  " valign="top">
                                              
                                               <div id="diviinquiry" style="display: block">
                                                    <table cellpadding="0" cellspacing="0" style="width: 85%; background-color: azure">
                                                        <tr>
                                                            <td align="left" nowrap="nowrap" style="width: 1%; height: 24px" valign="bottom">
                                                                <img src="../images/toe.GIF" /></td>
                                                            <td align="left" class="table_head_dark" nowrap="nowrap" width="20%" id="tdtransource">
                                                                STATUS</td>
                                                            <td align="left" class="bg_body" nowrap="nowrap" style="width: 259px" valign="bottom">
                                                                <img align="absBottom" src="../images/rend.gif" />&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="width: 100%; height: 100%">
                                                                <table class="table_border" style="width: 100%; height: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 100%; height: 8px">
                                                                            <table id="Table3" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                        STATUS:</td>
                                                                                    <td align="left" colspan="3" style="height: 23px">
                                                                                        <select id="ddlStatus" style="width: auto; height: 21px">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                        TO:</td>
                                                                                    <td align="left" style="width: 26%; height: 23px">
                                                                                    <input id="txtToDate" type="text" style="width: 66%;text-align:right;" />
                                                                                <img align="absMiddle" onclick="javascript:popUpCalendar(this, document.getElementById('txtToDate'), 'mm/dd/yyyy')"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                                    </td>
                                                                                    <td align="left" class="cell_caption" style="width: 24%; height: 23px">
                                                                                        FROM:</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtFromDate" type="text" style="width: 62%;text-align:right;" />
                                                                                        <img align="absMiddle" onclick="javascript:popUpCalendar(this, document.getElementById('txtFromDate'), 'mm/dd/yyyy')"
                                                                                    src="../images/calendar/imgcalen.png" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td colspan="4" style="height: 31px">
                                                                                        <input id="btnSearch" onclick="javascript:gsi.btnSearch_Click();" style="width: 17%"
                                                                                        type="button" value="Search" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td colspan="4" style="height: 31px" id="tdStatus">
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
                                             
                                            </td>
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
