<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadedTransaction.aspx.cs" Inherits="IRSWorld.IRSFileLoader.UploadedTransaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-y:scroll">
<head runat="server">
    <title>Uploaded Transactions</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css"/>
    <style type="text/css"></style>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table style="width:100%; height:auto;" cellspacing="4" cellpadding="2">
            <tr>
                <td id="tdUploadedTranxSideBar" style="width:19%; vertical-align:top">
                    <div id="divUploadedTranxSideBar" style="width:100%">
                    </div>
                </td>
                <td style="width:1%; border-left: solid 1px Gray">
                </td>
                <td style="width:80%">
                    <div>
                        <table style="width:100%; height:auto" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:1px;height:24px;">
                                <img src="../images/toe.gif" align="absbottom" />
                                </td>
                                <td class="table_head_dark" style="width:30%;height:24px; text-align:left">UPLOADED TRANSACTIONS</td>
                                <td style="width:75%;height:24px">
                                <img src="../images/rend.gif" align="absbottom" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width:100%; height:auto; border:solid 3px #336699" colspan="3">
                                    <div style="width:100; height:450px">
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
</body>
</html>
