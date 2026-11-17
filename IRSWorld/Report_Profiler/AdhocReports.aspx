<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdhocReports.aspx.cs" Inherits="IRSWorld.Report_Profiler.AdhocReports" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Adhoc Reports</title>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <!--link href="../css/Default.css" rel="stylesheet" type="text/css" / -->
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    <script src="../js/Report_Profiler/jsAdhocReports.js" type="text/javascript"></script>
    <script type="text/javascript">
        var adhoc_reports = new adhoc_reports();
    </script>
</head>
<body onload="javascript:adhoc_reports.initializePage();">
    <form id="form1" runat="server">
    <div id="divAdhoc">
    <center>
        <div>
            <table border="0" cellpadding="0" cellspacing="0" style="width:800px">
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td style="vertical-align:bottom;text-align:left;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width:0%;border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>
                                <td class="table_head_dark" style="height:auto;width:300px;">
                                    Adhoc Reports</td>
                                <td style="height:auto;text-align:left;vertical-align:bottom;width:400px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                            </tr>
                        </table>
                    </td>   
                </tr>
                <tr>
                    <td colspan="3" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid; border-bottom: #394397 3px solid;height:auto;" align="left"> <!-- colspan="3"> -->
                        <table border="0" style="width:100%; border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                            <tr>
                                <td colspan="5" class="table_caption" style="color: black; text-indent: 5px;height:26px;" align="left"></td>
                            </tr>
                            <tr style="height:25px;">
                                <td class="cell_caption" style="text-align:center; width:20%;">Report Name</td>
                                <td colspan="4" align="left" style="width:80%;"><select id="ddlAdhocReports" class="ddl" style="width:100%;"></select></td>
                            </tr>
                            <tr style="height:25px;">
                                <td class="cell_caption" style="width:20%; text-align:center;">Date From</td>
                                <td style="width:24%;" align="left">
                                    <input id="txtDateFrom" style="text-align:left; width:80%; vertical-align:middle;" type="text" />
                                    <img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtDateFrom, 'yyyy-mm-dd',document.getElementById('divAdhoc'));" style="vertical-align:middle;">
                                </td>
                                <td style="width:2%;">&nbsp;</td>
                                <td class="cell_caption" style="width:20%; text-align:center;">Date To</td>
                                <td style="width:24%;" align="left">
                                    <input id="txtDateTo" style="text-align:left; width:80%; vertical-align:middle;" type="text" />
                                    <img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtDateTo, 'yyyy-mm-dd',document.getElementById('divAdhoc'));" style="vertical-align:middle;">
                                </td>
                            </tr>
                            <tr style="height:25px;">
                                <td colspan="5" align="right"><input id="btnSubmit" type="button" value="Submit" style="width:25%;" onclick="javascript:adhoc_reports.generateReport();" /></td>
                            </tr>
                        </table>
                        &nbsp;Note: Please limit your extract to one (1) week to one (1) month data only.
                        Extract can cause delay to all remittance transactions.</td>
                </tr>
            </table>
        </div>
        <div id="divPopProcess" style="height:200px;background:White;z-index:75;position:absolute;left:350px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:270px;">
            <table style="height:100%;width:350px;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center;color:Chocolate"></label>
                            <div id="divLoadingMsg">
                            </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divCenter"></div>
    </center>
    </div>
    </form>
</body>
</html>
