<%@ Page Language="C#" AutoEventWireup="true" Codebehind="InternetRemitterApproval.aspx.cs"
    Inherits="IRSWorld.Internet.InternetRemitterApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Internet Remitter Approval</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />

    <script src="../js/commons.js" type="text/javascript"></script>

    <script src="../js/datagrid.js" type="text/javascript"></script>

    <script src="../js/internet/jsInternetRemitterApproval.js" type="text/javascript"></script>

    <script src="../js/jsCommons.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <%--<script src="../js/jsgridselector.js" type="text/javascript"></script>--%>
</head>
<body onload="javascript:IRA.populate_rec();">
    <center>
        <form id="form1" runat="server">
            <div style="width: 100%; height: 100%; vertical-align: top; margin-top: 30px;">
                <table height="100%" cellpadding="0" cellspacing="5" style="width: 95%">
                    <tr>
                        <td style="width: 100%; border-collapse: collapse; vertical-align: top">
                            <div id="remList" style="width: 99%; display: block;">
                                <table id="tblSearch" width="85%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 60%; vertical-align: bottom; text-align: left; height: 43px;">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="capSearch" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                                        border-collapse: collapse">
                                                        Internet Remitter Approval</td>
                                                    <td align="left" valign="bottom" style="height: 24px; width: 60%;" class="bg_body">
                                                        <img align="absbottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblInternetRemApproval" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                    border-left: #336699 3px solid; width: 85%; border-bottom: #336699 3px solid;">
                                    <tr>
                                        <td id="InterNetRemApp" style="width: 85%; height: 21px;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="DocList" style="width: 65%; display: none; position:absolute; z-index:99; left: 129px; top: 72px;">
                                <table id="tblDocumentList" style="border-right: #336699 3px solid; display: block;
                                    border-top: #336699 3px solid; border-left: #336699 3px solid; width: 99%; border-bottom: #336699 3px solid;
                                    width: 99%">
                                    <tr>
                                        <td id="documentList" style="width: 98%; height: 21px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="Td1" style="width: 98%; height: 21px;">
                                        <a href="#" onclick="javascript:IRA.hidePanel('DocList')">[CLOSE]</a>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Button Width="22px" Height="0px" ID="Button1" OnClick="test_download" runat="server" />
                            <asp:HiddenField ID="dl_cifid" runat="server" />
                            <asp:HiddenField ID="hf_Filename" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 17%; height: 18px;" align="right">
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </center>
</body>
</html>
