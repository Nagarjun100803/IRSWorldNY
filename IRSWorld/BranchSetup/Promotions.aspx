<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Promotions.aspx.cs" Inherits="IRSWorld.BranchSetup.Promotions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Promotion </title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    </style>

    <script src="../js/jsCommons.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jshttprequest.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <script src="../js/BranchSetup/jsPromotions.js" type="text/javascript"></script>

    <script src="../js/popcalendar.js" type="text/javascript"></script>

    <script type="text/javascript">
        var promotions = new promotions();
    </script>

</head>
<body onload="javascript:promotions.initializePage();">
    <form id="form1" runat="server">
        <br />
        <div id="divCenter">
            <%--<iframe id="frmCenter" name="center_frame" frameborder="0" scrolling="no" allowtransparency="true"
                style="z-index: 0"></iframe>--%>
        </div>
        <div id="divPromoDates" style="width: 500px; vertical-align: middle; margin: 0px auto;
            border: solid 2px midnightblue; z-index: 5" class="ds_gray">
            <table id="tblPromoDates" style="width: 100%" cellspacing="5">
                <caption class="table_caption" style="height: 22px; vertical-align: middle">
                    Promotions</caption>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Promo Code&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txtPromoCode" style="width: 116px; text-align: left" maxlength="3" />
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Promo Description&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txtPromoDescription" style="width: 305px; text-align: left"
                            maxlength="50" />
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Minimum Remittance Amount&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txtMinRemAmount" style="width: 116px; text-align: left" />
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Minimum Transaction Count&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txtMinTransCount" style="width: 116px; text-align: left" />
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Receive Promo Voucher&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                       <select style="width: 116px" id="ddReceiveVoucher">
                            <option value="1">True</option>
                            <option value="0">False</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Valid From:&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txtPopValidFrom" style="width: 116px; text-align: left" readonly="readonly" />
                        <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtPopValidFrom, 'mm/dd/yyyy');" />
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Valid To:&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txtPopValidTo" style="width: 116px; text-align: left" readonly="readonly" />
                        <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtPopValidTo, 'mm/dd/yyyy');" />
                    </td>
                </tr>
                <%--<tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Promo Email Subject&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txEmailSubject" style="width: 116px; text-align: left" />
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        Customer Qualification Qty.&nbsp</td>
                    <td align="left" style="height: 22px; width: 220px;">
                        <input type="text" id="txtQualificationQty" style="width: 116px; text-align: left" />
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 60px;">
                        Promo Email Content&nbsp</td>
                    <td align="left" style="height: 60px; width: 220px;">
                        <textarea id="txtEmailContent" style="width: 304px; height: 75px; text-align: left"></textarea>
                    </td>
                </tr>--%>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 60px;">
                        Promo Voucher Content&nbsp</td>
                    <td align="left" style="height: 60px; width: 220px;">
                        <textarea id="txtPromoContent" style="width: 304px; height: 75px; text-align: left"></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 60px;">
                        Terms and Conditions&nbsp</td>
                    <td align="left" style="height: 60px; width: 220px;">
                        <textarea id="txtPromoTerms" style="width: 304px; height: 75px; text-align: left"></textarea>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; height: 28px;" align="center">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%; height: 24px;" align="center">
                                    <input id="Button3" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';"
                                        onclick="open_remitter_result();" type="button" value="Search" />
                                </td>
                                <td style="width: 20%; height: 24px;" align="center">
                                    <input id="btnAdd" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';"
                                        onclick="promotions.insertRecord();" type="button" value="Add" />
                                </td>
                                <td style="width: 20%; height: 24px;" align="center">
                                    <input id="Button1" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';"
                                        onclick="promotions.updateRecord();" type="button" value="Update" />
                                </td>
                                <td style="width: 20%; height: 24px;" align="center">
                                    <input id="Button2" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';"
                                        onclick="promotions.removePromoCode();" type="button" value="Delete" />
                                </td>
                                <td style="width: 20%; height: 24px;" align="center">
                                    <input id="btnCancel" onmouseover="this.className='warning_over';" class="button"
                                        onmouseout="this.className='button';" onclick="clearFields();" type="button"
                                        value="Cancel" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <hr style="margin: 0px auto; border: solid 2px midnightblue;" />
            <table id="Table2" style="width: 100%" cellspacing="5">
                <tr>
                    <td class="cell_caption" style="width: 35%; height: 22px;">
                        State&nbsp
                    </td>
                    <td>
                        <select style="width: 210px" id="ddState">
                            <option value="9999999999">All States</option>
                        </select>
                    </td>
                    <td style="width: 20%" align="center">
                        <input id="btnAddState" onmouseover="this.className='button_over';" class="button"
                            onmouseout="this.className='button';" onclick="promotions.insertState();" type="button"
                            value="Add State" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="border-right: #394397 3px solid; border-top: #394397 3px solid;
                        border-left: #394397 3px solid; border-bottom: #394397 3px solid; height: auto;">
                        <div id="divStateList" style="display: block;">
                            State Grid Here
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
