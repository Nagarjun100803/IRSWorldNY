<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receipt.aspx.cs" Inherits="IRSWorld.Remittance.Receipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Receipt</title>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/Internet/jsInternetPage.js" type="text/javascript"></script>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var internet_page = new internet_page();
    </script>
</head>
<body onload="">
    <form id="form1" runat="server">
    <div id="divTransactionHistoryModal" style="z-index:75;position:absolute;left:64px;color:darkblue;border:solid 2px #000;padding:10px;font-family:Arial;top:31px;width:800px;height:auto;">
        <table style="width:100%; border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
            <tr>
                <td colspan="2">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td rowspan="6" style="width:6%;border:solid 1px #000;">
                                <img src="../images/PNBLOGOembsmaller.png" / height="77" width="70">
                            </td>
                            <td style="width:50%">
                                <b><span style="font-size:11pt;">PHILIPPINE NATIONAL BANK</span></b></td>
                            <td align="right" style="width:20%;FONT-SIZE:9px;">Transaction Date:</td>
                            <td style="width:1%;">&nbsp;</td>
                            <td align="left" style="width:23%;FONT-SIZE:9px;">
                                September 16, 2011</td>
                        </tr>
                        <tr>
                            <td style="width:50%">
                                <span style="font-size:9pt;">PNB Global Remit and PNB Rapid Remit</span></td>
                            <td align="right" style="width:20%;FONT-SIZE:9px;height:auto;">
                                Clearing Date:</td>
                            <td style="width:1%;">&nbsp;</td>
                            <td align="left" style="width:23%;FONT-SIZE:9px;">
                                September 16, 2011</td>
                        </tr>
                        <tr>
                            <td style="width:50%; height: 18px;">
                                <span style="font-size:8pt;"><strong>WEST COVINA BRANCH</strong></span></td>
                            <td align="right" style="width:20%;FONT-SIZE:9px;height:18px;">
                                Time:</td>
                            <td style="width:1%; height: 18px;">&nbsp;</td>
                            <td align="left" style="width:23%;FONT-SIZE:9px;">
                                09:16 AM</td>
                        </tr>
                        <tr>
                            <td style="width:50%">
                                <span style="font-size:8pt;">1559-K E. Amar Rd. West Covina, CA 91792</span></td>
                            <td align="right" style="width:20%;FONT-SIZE:9px;height:auto;">
                                Loyalty Points:</td>
                            <td style="width:1%;">&nbsp;</td>
                            <td align="left" style="width:23%;FONT-SIZE:9px;">
                                0</td>
                        </tr>
                        <tr>
                            <td style="width:50%">
                                <span style="font-size:8pt;">Tel: (626) 854-2045 &amp; (626) 854-2044 / Fax: (626) 854-7346</span></td>
                            <td align="right" style="width:20%;FONT-SIZE:9px;height:auto;">
                                </td>
                            <td style="width:1%;">&nbsp;</td>
                            <td align="left" style="width:23%;FONT-SIZE:9px;">
                                </td>
                        </tr>
                        <tr>
                            <td style="width:50%">
                                <span style="font-size:8pt; text-decoration: underline;">Email: westcovina@pnbrci.com
                                    / www.pnbrci.com</span></td>
                            <td align="right" style="width:20%;FONT-SIZE:9px;height:auto;">
                                Status:</td>
                            <td style="width:1%;">&nbsp;</td>
                            <td align="left" style="width:23%;FONT-SIZE:9px;">
                                <strong><span style="font-size: 9pt">
                                Credited Online</span></strong></td>
                        </tr>
                    </table>
                    <br />
                    <table width="100%" cellpadding="0" cellspacing="0" style="border:solid 1px #000;">
                        <tr>
                            <td style="width:33%;border:solid 1px #000;"><span style="font-size:9pt;">Remitter ID No.: 1TOK000000001</span></td>
                            <td style="width:33%;border:solid 1px #000;"><span style="font-size:9pt;">Official Receipt: PNB001234</span></td>
                            <td style="width:33%;border:solid 1px #000;"><span style="font-size:9pt;">Reference No.: 1TOK091611000001</span></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="center" width="50%" style="border:solid 1px #000;" colspan="2">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="50%">
                                                        <span style="font-size:9pt; text-decoration: underline;">REMITTER INFO</span>
                                                    </td>
                                                    <td width="50%">
                                                        <span style="font-size:9pt; text-decoration: underline;">REMITTANCE DETAILS</span>
                                                    </td>
                                                </tr>
                                            </table>
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
    
    <input id="btnPrintPanel" type="button" value="Print" onclick="javascript:internet_page.printDiv('divTransactionHistoryModal');" style="width:50px; visibility:visible;" />
    <input id="btnClosePanel" type="button" value="Close" onclick="javascript:internet_page.hidePanel('divTransactionHistoryModal');" style="width: 50px" />
    </form>
</body>
</html>
