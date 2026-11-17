<%@ Page Language="C#" AutoEventWireup="true" Codebehind="RemTranAPAS.aspx.cs" Inherits="IRSWorld.Remittance.RemTranAPAS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittance - APAS</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/jsCommons.js"                type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"           type="text/javascript"></script>
    <script src="../js/js_sidebar.js"               type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js"            type="text/javascript"></script>
    <script src="../js/Remittance/jsremsaving.js"   type="text/javascript"></script>
    <script src="../js/Remittance/jsremcommons.js"  type="text/javascript"></script>
    <script src="../js/Remittance/jsRemTranAPAS.js" type="text/javascript"></script>

    <script type="text/javascript">    
            var RemTran_APAS = new RemTran_APAS();
    </script>

</head>
<body onload="javascript:RemTran_APAS.construct_side_bar('divtdRemAPASMenu');">
    <form id="form1" runat="server">
        <div style="width: 100%">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 380px;"
                        class="content_height" colspan="2">
                        <div id="divRemAPAS" style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td id="tdRemAPAS" valign="top" style="width: 20%; vertical-align: top; padding-top: 30px;">
                                        <!-- border-right: solid 1px #5697c1;"> -->
                                        <div id="divtdRemAPASMenu" style="margin-top: 20px; display: block;">

                                            <script type="text/javascript">
                                            //temp
//                                            var sider = new js_sidebar('sidemenu', sidebar);
//                                            document.write(sider.toString());
                                            </script>

                                        </div>
                                        <a id="ahider" href="#" 
                                        onclick="javascript:RemTran_APAS.set_side_bar_visibility('divtdRemAPASMenu','ahider');">«
                                        </a>
                                    </td>
                                    <td style="width: 80%; padding-top: 30px">
                                        <table width="96%" cellspacing="0" cellpadding="0" style="background-color: Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width: 1%; height: 24px;">
                                                    <img src="../images/toe.GIF" />
                                                </td>
                                                <td id="capResult" align="left" class="table_head_dark" 
                                                style="width: 40%; height: 23px; border-collapse: collapse;">
                                                    Remittance - Advise and Pay
                                                </td>
                                                <td align="left" valign="bottom" style="height: 24px" class="bg_body">
                                                    <img align="absbottom" src="../images/rend.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width: 100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px;">
                                                                <table width="100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%;" valign="middle">
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divAPASRemRemitterInfo">
                                                                                <table id="tblAPASRemRemitter" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Remitter :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px;">
                                                                                            <label id="lblAPASRemName" class="cell_desc" style="font-size: medium;">
                                                                                                &nbsp;Alvin C. Santos
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Address :&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px;">
                                                                                            <label id="lblAPASRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;Via Fauche 37, Milan, Italy
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px; width: 3px;">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <%-- start here  --%>
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divAPASRemBeneficiaryInfo">
                                                                                <table id="tblAPASRemBeneficiary" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px;">
                                                                                            Beneficiary :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px;">
                                                                                            <label id="lblAPASRemBeneName" class="cell_desc" style="font-size: medium;">
                                                                                                &nbsp;Janet S. Romata
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px;">
                                                                                            Address :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px;">
                                                                                            <label id="lblAPASRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria Bulacan,
                                                                                                Philippines, 3022
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divAPASRemAmountInfo">
                                                                                <table id="tblAPASRemAmount" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px;">
                                                                                            Total Amount (Rmt Ccy) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 23%; height: 23px;">
                                                                                            <label id="lblAPASRemTARC" class="cell_desc" style="font-size: 15px; font-weight: bold">
                                                                                                100,000.00
                                                                                            </label>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px;">
                                                                                            Net Remit Amount (Rmt Ccy) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 23%; height: 23px;">
                                                                                            <label id="lblAPASRemNRARC" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                100,000.00
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px;">
                                                                                            Net Remit Amount (Php Eqv) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 23%; height: 23px;">
                                                                                            <label id="lblAPASRemNRAPE" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                100,000.00
                                                                                            </label>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px;">
                                                                                            Net to Beneficiary(A/C Ccy) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 23%; height: 23px;">
                                                                                            <label id="lblAPASRemNTBACC" class="cell_desc" style="font-size: 15px; font-weight: bold;">
                                                                                                100,000.00
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 28px; width: 100%" valign="top">
                                                                            <input style="height: 28px; width: 30%" type="button" value="View Transaction Details" class="button" 
                                                                                onmouseover="this.className='button_over';" onclick="javascript:RemTran_APAS.load_transaction_view_details();"
                                                                                onmouseout="this.className='button';" id="Button1" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                <!-- content ends here -->
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
    </form>
</body>
</html>
