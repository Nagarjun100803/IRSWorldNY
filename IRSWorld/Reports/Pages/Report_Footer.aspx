<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_Footer.aspx.cs" Inherits="IRSWorld.Reports.Pages.Report_Footer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Report Footer</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />    
    <script src="../../js/js_sidebar.js" type="text/javascript"></script>       
    <script src="../../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../../js/jscommons.js" type="text/javascript"></script>
    <script src="../../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../../js/Reports/js_Report_Footer.js" type="text/javascript"></script>
</head>
<body topmargin ="6" leftmargin="0" onload="javascript:rf.init_state();">
        <div style="overflow: auto" align="center">
            <form id="form1" runat="server" method="post">
                <table cellpadding="6" cellspacing="30">
                    <tr>
                        <td height="250" style="width: 626px">
                            &nbsp;
                            <div>
                                <table align="center" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="left" nowrap="nowrap" style="width: 1%; height: 24px" valign="bottom">
                                            <img src="../../images/toe.GIF" /></td>
                                        <td align="left" class="table_head_dark" nowrap="nowrap" width="20%">
                                            Report Footer</td>
                                        <td align="left" class="bg_body" nowrap="nowrap" valign="bottom">
                                            <img align="absBottom" src="../../images/rend.gif" />&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="3" nowrap="nowrap">
                                            <div id="tblHolder">
                                                <table cellpadding="0" cellspacing="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                    background: azure; border-left: #336699 3px solid; border-bottom: #336699 3px solid">
                                                    <tr style="height: auto">
                                                        <td align="center" valign="top">
                                                            <table cellpadding="4" cellspacing="4">
                                                                <tr>
                                                                    <td align="center" colspan="0" style="width: 100%" valign="middle">
                                                                        <div id="divUploadFile" align="center" style="border-right: midnightblue 3px solid;
                                                                            border-top: midnightblue 3px solid; vertical-align: middle; border-left: midnightblue 3px solid;
                                                                            width: 576px; border-bottom: midnightblue 3px solid">
                                                                            <table cellpadding="2" cellspacing="2" style="width: 100%">
                                                                                <tr>
                                                                                    <td style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid;
                                                                                        border-bottom: #336699 1px solid; height: 265px" valign="middle">
                                                                                        <table cellpadding="2" cellspacing="2" style="background: azure; width: 100%; height: 160px">
                                                                                            <tr>
                                                                                                <td align="left" class="table_head_dark" colspan="2" style="height: 10px">
                                                                                                    </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" nowrap="nowrap" style="width: 20%; height: 26px">
                                                                                                    Branch:</td>
                                                                                                <td nowrap="nowrap" style="width: 70%; height: 26px; text-align: left">
                                                                                                    &nbsp;<select id="ddBranch">
                                                                                                    </select>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" nowrap="nowrap" style="width: 20%; height: 26px">
                                                                                                    Enable:</td>
                                                                                                <td nowrap="nowrap" style="width: 70%; height: 26px; text-align: left">
                                                                                                    &nbsp;<input id="chkEnable" style="width: 7%" type="checkbox" /></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" nowrap="nowrap" style="width: 20%; height: 26px">
                                                                                                    &nbsp;Template:</td>
                                                                                                <td nowrap="nowrap" style="height: 26px; text-align: left">
                                                                                                <div id="divuploaded" style="display:none ;width:auto;">
                                                                                                <label id="txtfilepath"></label>
                                                                                                <input type="button" id="btnChange" value="Change" style="width: 16%" onclick="javascript:rf.change_click();"/>
                                                                                                </div>
                                                                                                <div id="divupload" style="display:none ;width:auto;">
                                                                                                <input id="fileBrowse" runat="server" onpropertychange="javascript:rf.file_browse()"
                                                                                                        style="width: 70%; height: 24px" type="file" />
                                                                                                        <INPUT style="WIDTH: 16%" id="btnCancel" onclick="javascript:rf.cancel_click();" type=button value="Cancel" />
                                                                                                </div>
                                                                                                    </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="" colspan="2" nowrap="nowrap" style="height: 26px">
                                                                                                <INPUT style="WIDTH: 16%" id="Button1" onclick="javascript:rf.save_click();" type=button value="Save" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2" style="height: 150px" valign="top">
                                                                                                    <div id="divGridHolder" style="border-right: midnightblue 3px solid; border-top: midnightblue 3px solid;
                                                                                                        display: none; background: azure; vertical-align: middle; border-left: midnightblue 3px solid;
                                                                                                        width: auto; border-bottom: midnightblue 3px solid">
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
