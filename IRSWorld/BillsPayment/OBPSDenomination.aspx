<%@ Page Language="C#" AutoEventWireup="true" Codebehind="OBPSDenomination.aspx.cs"
    Inherits="IRSWorld.BillsPayment.OBPSDenomination" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OBPS Denomination</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/jsCommons.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jshttprequest.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/BillsPayment/jsOBPSDenomination.js" type="text/javascript"></script>

    <script type="text/javascript">
        var obps_denomination = new OBPS_Denomination();
    </script>

</head>
<body onload="javascript:obps_denomination.load();">
    <form id="form1" runat="server">
        <center>
            <div style="width: 90%;">
                <table style="width:100%" cellpadding="0" cellspacing="5">
                    <tr>
                        <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 380px;"
                            class="content_height" colspan="2">
                            <div id="divBillsPayOBPSDenomination" style="width: 100%; height: 100%; display: block">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width: 78%; padding-top: 30px">
                                            <table cellspacing="0" cellpadding="0" style="width:96%; background-color: Azure">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 1%; height: 24px;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="capResult" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                                        border-collapse: collapse;">
                                                        OBPS - Denomination</td>
                                                    <td align="left" valign="bottom" style="height: 24px" class="bg_body">
                                                        <img align="absbottom" src="../images/rend.gif" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width: 100%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 100%; height: 21px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top; width: 100%; height: 100%">
                                                                    <!-- content starts here -->
                                                                    <table cellpadding="0" cellspacing="5" style="width:100%">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%" valign="center">
                                                                                <div id="divBillsPayOBPSRemitterInfo" class="table_body" style="margin: 0px; width: 98%;
                                                                                    height: 100%">
                                                                                    <table id="tblBillsPayOBPSRemitter" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid;
                                                                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                                                                        width:100%">
                                                                                        <tr>
                                                                                            <td class="cell_caption" style="width: 20%; height: 23px;">
                                                                                                Reference No : &nbsp;
                                                                                                <td style="width: 20%; height: 23px; text-align: left;">
                                                                                                    <label id="lblRefNo" class="cell_desc" style="font-size: small">
                                                                                                        1</label></td>
                                                                                                <td class="cell_desc" style="width: 20%; height: 23px;">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td class="cell_desc" style="width: 20%; height: 23px;">
                                                                                                </td>
                                                                                                <td class="cell_desc" style="width: 20%; height: 23px;">
                                                                                                </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td class="cell_caption" style="width: 20%; height: 23px;">
                                                                                                Denominations : &nbsp;
                                                                                            </td>
                                                                                            <td class="cell_desc" style="width: 20%; height: 23px;">
                                                                                                <select id="ddlDenominations" class="cell_desc" size="1" style="width: 100%; text-align: left;">
                                                                                                </select>
                                                                                            </td>
                                                                                            <td class="cell_caption" style="width: 20%; height: 23px;">
                                                                                                Value : &nbsp;
                                                                                            </td>
                                                                                            <td class="cell_desc" style="width: 20%; height: 23px;">
                                                                                                <input id="txtValue" style="width: 95%; text-align: right;" />
                                                                                            </td>
                                                                                            <td class="cell_desc" style="width: 20%; height: 23px;">
                                                                                                <input type="button" value="Add" onclick="javascript:obps_denomination.insert_record();obps_denomination.commit_record();obps_denomination.load_denomination_grid();"
                                                                                                    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"
                                                                                                    id="btnAdd" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table id="tblTransactions" style="width: 100%;">
                                                                        <tr>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 50%;" valign="top">
                                                                                <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                                                                <div id="divDenomDetails" style="width: 100%; <%--overflow: scroll--%>">
                                                                                    <table cellpadding="0" style="width:98%; height:200px; border: solid 3px #336699; color: #0000ff" id="tblDenominations">
                                                                                        <tr id="DenomDetails">
                                                                                            <td style="width: 50%; vertical-align:top"> <!--valign="top"--> <%--<%--border-left-color: royalblue; border-bottom-color: royalblue;
                                                                                                border-top-style: solid; border-top-color: royalblue; border-right-style: solid;
                                                                                                border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid;--%>
                                                                                                
                                                                                                
                                                                                                <table id="tblDenomDetails" style="width:94%" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td class="table_caption" colspan="3" style="width: 85%; text-align: left; text-indent: 5px;
                                                                                                            color: Black; height: 26px;" valign="top">
                                                                                                            DENOMINATION DETAILS</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center" valign="middle" colspan="2">
                                                                                                            <div id="DIV_DENOM" onmousemove="javascript:obps_denomination.active_grid = 'DIV_DENOM';">
                                                                                                                test<br />
                                                                                                            </div>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                            <td style="width: 50%;" valign="top">
                                                                                <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                                                                <div id="divTransactions" style="width: 100%; <%--overflow: scroll--%>">
                                                                                    <table cellpadding="0" style="width:98%; border: solid 3px #336699; color: #0000ff" id="TABLE2">
                                                                                        <tr id="Tr1">
                                                                                            <td style="width: 50%;"valign="top"> <%--border-left-color: royalblue; border-bottom-color: royalblue;
                                                                                                border-top-style: solid; border-top-color: royalblue; border-right-style: solid;
                                                                                                border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid;"
                                                                                                --%>
                                                                                                <table id="tblComputations" style="width:94%" cellpadding="0">
                                                                                                    <tr>
                                                                                                        <td class="table_caption" colspan="3" style="width: 85%; text-align: left; text-indent: 5px;
                                                                                                            color: Black; height: 26px;" valign="top">
                                                                                                            TRANSACTION DETAILS</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                            Total Cash : &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 30%; height: 23px; text-align: left">
                                                                                                            <label id="lblTotalCash" class="cell_desc" style="text-align: left; font-size: small">
                                                                                                                &nbsp;
                                                                                                            </label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                            Total Checks : &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 30%; height: 23px; text-align: left">
                                                                                                            <label id="lblTotalChecks" class="cell_desc" style="text-align: left; font-size: small">
                                                                                                                &nbsp;
                                                                                                            </label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                            Total ATM/DEBIT : &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 30%; height: 23px; text-align: left">
                                                                                                            <label id="lblTotalATMDebit" class="cell_desc" style="text-align: left; font-size: small">
                                                                                                                &nbsp;
                                                                                                            </label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                            Total Others : &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 30%; height: 23px; text-align: left">
                                                                                                            <label id="lblTotalOthers" class="cell_desc" style="text-align: left; font-size: small">
                                                                                                                &nbsp;
                                                                                                            </label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                            Grand Total : &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 30%; height: 23px; text-align: left">
                                                                                                            <label id="lblGrandTotal" class="cell_desc" style="text-align: left; font-size: medium">
                                                                                                            </label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                            Change : &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 30%; height: 23px; text-align: left">
                                                                                                            <label id="lblChange" class="cell_desc" style="text-align: left; font-size: medium">
                                                                                                                &nbsp;
                                                                                                            </label>
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
                <!-- content ends here -->
            </div>
        </center>
    </form>
</body>
</html>
