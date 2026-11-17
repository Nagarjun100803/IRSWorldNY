<%@ Page Language="C#" AutoEventWireup="true" Codebehind="InternetUploads.aspx.cs"
    Inherits="IRSWorld.Internet.InternetUploads" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/internet/jsInternetUploads.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="divUpload" style="display: block; width: 100%;  margin-top:50px;" align="center">
                <table style="width: 75%;" cellpadding="0" cellspacing="0">
                    <tr>
                        <table style="width: 65%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                    <img src="../images/toe.GIF" /></td>
                                <td id="capSearch" align="left" class="table_head_dark" style="width: 30%; height: 24px;
                                    border-collapse: collapse">
                                    Upload Supporting Documents</td>
                                <td align="left" valign="bottom" style="height: 24px; width: 44%;" class="bg_body">
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                        </table>
                        <table id="tblUpload" cellpadding="0" cellspacing="5" style="border-right: #394398 1px solid;
                            border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid;
                            border-bottom: #394398 1px solid; width: 65%;">
                            <tr>
                                <td id="Td1" class="table_caption" colspan="5" style="text-indent: 5px; text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td class="cell_caption" style="width: 20%; height: 24px">
                                    File 1 :&nbsp;</td>
                                <td align="left" style="width: 70%; height: 24px">
                                    &nbsp;<input id="File1" runat="server" type="file" style="width:95%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="cell_caption" style="width: 9%; height: 24px">
                                    File 2 :&nbsp;</td>
                                <td align="left" style="width: 70%; height: 24px">
                                    &nbsp;<input id="File2" runat="server" type="file" style="width: 95%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="cell_caption" style="width: 9%; height: 24px">
                                    File 3 :&nbsp;</td>
                                <td align="left" style="width: 70%; height: 24px">
                                    &nbsp;<input id="File3" runat="server" type="file" style="width: 95%" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="width: 51%; height: 24px">
                                    <input id="btnUploadFile" value="Upload" type="button" onclick="javascript:upload.uploadDocuments();"
                                        style="width: 115px" />
                                    <input id="btnCancelUpload" value="Close" type="button" onclick="javascript:upload.clearAll();"
                                        style="width: 115px" />
                                </td>
                            </tr>
                        </table>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
