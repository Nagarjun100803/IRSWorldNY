<%@ Page Language="C#" AutoEventWireup="true" Codebehind="RemTranDoorToDoor.aspx.cs" Inherits="IRSWorld.Remittance.RemTranDoorToDoor" %>

    

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittance - Door to Door</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving.js"  type="text/javascript"></script>
    <script src = "../js/Remittance/jsremcommons.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsRemTranDoorToDoor.js" type="text/javascript"></script>
    <script type="text/javascript">
        var RemTranDoorToDoor = new RemTranDoorToDoor();
    </script>

</head>
<body onload="javascript:RemTranDoorToDoor.construct_side_bar('divtdRemDoortoDoorMenu')">
    <form id="form1" runat="server">
        <div style="width: 100%">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 380px;"
                        class="content_height" colspan="2">
                        <div id="divRemDoortoDoor" style="width: 100%; height: 100%; display: block">
                            <table style="width:100%">
                                <tr>
                                    <td id="tdRemDoortoDoor" valign="top" style="width: 20%; vertical-align: top; padding-top: 30px;">
                                        <!-- border-right: solid 1px #5697c1;"> -->
                                        <div id="divtdRemDoortoDoorMenu" style="margin-top: 20px; display: block;">

                                            <script type="text/javascript">
                                         //   var sider = new js_sidebar('sidemenu', sidebar);
                                           // document.write(sider.toString());


                                            </script>

                                        </div>
                                        <a id="ahider" href="#" onclick="javascript:RemTranDoorToDoor.set_side_bar_visibility('divtdRemDoortoDoorMenu','ahider')">«</a>
                                    </td>
                                    <td style="width: 81%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%; background-color: Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width: 1%; height: 24px;">
                                                    <img src="../images/toe.GIF" />
                                                </td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                                    border-collapse: collapse;">
                                                    Remittance - Door to Door
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
                                                                <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%;" valign="center">
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divDoortoDoorRemitterInfo">
                                                                                <table id="tblDoortoDoorRemRemitter" cellpadding="0" cellspacing="8"
                                                                                    style="border: solid 1px #336699; width:100%; ">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Remitter :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 23px;">
                                                                                            <label id="lblDoortoDoorRemName" class="cell_desc" style="font-size: medium;">
                                                                                                &nbsp;Alvin C. Santos
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 21px">
                                                                                            Address :&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 21px;">
                                                                                            <label id="lblDoortoDoorRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;Via Fauche 37, Milan, Italy
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <%-- start here  --%>
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divDoortoDoorRemBeneficiaryInfo">
                                                                                <table id="tblDoortoDoorRemBeneficiary" cellpadding="0" cellspacing="8"
                                                                                    style="border: solid 1px #336699; width:100%">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 24px;">
                                                                                            Beneficiary :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 24px;" colspan="3">
                                                                                            <label id="lblDoortoDoorRemBeneName" class="cell_desc" style="font-size: medium;">
                                                                                                &nbsp;Janet S. Romata
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 24px;">
                                                                                            Address :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 85%; height: 24px;" colspan="3">
                                                                                            <label id="lblDoortoDoorRemBeneAddress" class="cell_desc" style="font-size: 15px;
                                                                                                font-weight: normal;">
                                                                                                &nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria Bulacan,
                                                                                                Philippines, 3022
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 24px;">
                                                                                            Payee Bank :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemBenePayeeBank" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;Bank of China
                                                                                            </label>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 24px;">
                                                                                            CA/SA No. :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 35%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemBeneCASANo" class="cell_desc" style="font-size: 15px;
                                                                                                font-weight: normal;">
                                                                                                &nbsp;1111222233334444
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 24px;">
                                                                                            Account Type :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 35%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemBeneAccountType" class="cell_desc" style="font-size: 15px;
                                                                                                font-weight: normal;">
                                                                                                &nbsp;Savings
                                                                                            </label>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 24px;">
                                                                                            A/C Currency :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 35%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemBeneACCurrency" class="cell_desc" style="font-size: 15px;
                                                                                                font-weight: normal;">
                                                                                                &nbsp;PHP
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
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divDoortoDoorRemAmountInfo">
                                                                                <table id="tblDoortoDoorRemAmount" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 25%; height: 24px;">
                                                                                            Total Amount (Rmt Ccy) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 25%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemTARC" class="cell_desc" style="font-size: 15px; font-weight: bold">
                                                                                                100,000.00</label></td>
                                                                                        <td class="cell_caption" style="width: 25%; height: 24px;">
                                                                                            Net Remit Amount (Rmt Ccy) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 25%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemNRARC" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                100,000.00
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 25%; height: 24px;">
                                                                                            Net Remit Amount (Php Eqv) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 25%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemNRAPE" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                100,000.00
                                                                                            </label>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 25%; height: 24px;">
                                                                                            Net to Beneficiary(A/C Ccy) :&nbsp;
                                                                                        </td>
                                                                                        <td align="right" style="width: 25%; height: 24px;">
                                                                                            <label id="lblDoortoDoorRemNTBACC" class="cell_desc" style="font-size: 15px; font-weight: bold;">
                                                                                                100,000.00
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                          
                                                                           
                                                                           
                                                                            <%--gladys--%>
                                                                            
                                                                           
                                                                            <%--will--%>
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 28px; width: 100%" valign="top">
                                                                            <input style="height: 28px; width: 30%" type="button" value="View Transaction Details"
                                                                               onclick="javascript:RemTranDoorToDoor.load_transaction_view_details();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="Button1"  /></td>
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
