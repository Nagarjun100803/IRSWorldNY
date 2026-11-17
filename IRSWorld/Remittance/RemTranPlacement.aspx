<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranPlacement.aspx.cs" Inherits="IRSWorld.Remittance.RemTranPlacement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <title>OVER THE COUNTER - REMITTANCE</title>    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src = "../js/jsCommons.js"                      type = "text/javascript"></script>
    <script src = "../js/jscommonmethod.js"                 type = "text/javascript"></script>
    <script src = "../js/js_sidebar.js"                     type = "text/javascript"></script>
    <script src = "../js/jshttprequest.js"                  type = "text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"                  type = "text/javascript"></script>
    <script src = "../js/jsgridselector.js"                 type = "text/javascript"></script>
    <script src = "../js/Remittance/jsRemTranPlacement.js"  type = "text/javascript"></script>
    
</head>

<body onload = "javascript:iremit.load_sidebar();">
    <form id="form1" runat="server">
        <center>
            <table id = "tbl_outermost" cellpadding = "0"; cellspacing = "0" style = "width:100%">
                <tr>
                    <td id="td_remittance" style="vertical-align: top; width:20%; height: auto; padding-top: 10px"><div id="div_transaction_menu" style="margin-top: 20px; display: block; overflow: auto"></div><a id="anchor_off" href="#" onclick="javascript:iremit.load_unload_sidebar();">«</a></td>
                    <td style="width:auto; height: auto;">
                        <table id = "tbl_folder_content" cellpadding="0" cellspacing="0" style="width:auto;background-color: azure">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom"><img src="../images/toe.GIF" /></td>
                            <td align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;height: 24px">OVER THE COUNTER - REMITTANCE</td>
                            <td align="left" class="bg_body" style="height: 24px; width: 41px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table id = "tbl_content_container" class="table_border" style="width:100%;height:100%">
                                    <tr>
                                        <td style="height: 100%">
                                            <center>
                                                    <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="TABLE1">
                                                    <tr><td class = "table_caption" colspan="2" style ="text-align:center">REMITTANCE INFORMATION - EXCHANGE RATE: 1 = 45.00</td></tr>
                                                    <tr style ="height: 19px">
                                                        <td class = "cell_caption" style="text-align: right; width: 18%;">Computation Type :</td>
                                                        <td class = "cell_desc" style="text-align: left; width: 70%;">
                                                            <select id="Select2" style="width: 35%">
                                                            <option selected="selected"></option>
                                                        </select></td>
                                                    </tr>
                                                    <tr style ="height: 19px">
                                                        <td class = "cell_caption" style="text-align: right; width: 18%;">Amount :</td>
                                                        <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="Text1" type="text" style="width: 34%" />(PRESS ENTER TO COMPUTE CHARGES)</td>
                                                    </tr>
                                                    <tr style ="height: 19px">
                                                        <td class = "cell_caption" style="text-align: right; width: 18%;">Special Message :</td>
                                                        <td class = "cell_desc" style="text-align: left; width: 70%;"><input id="Text2" type="text" style="width: 88%" /></td>
                                                    </tr>
                                                        <tr>
                                                            <td style="text-align: left" colspan="2">
                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%" id="Table2">
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Payment Type :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Product Type :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_product_type"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Currency :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Implementation Type :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_implementation_type"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Charge Type :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Receipt Mode :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_receipt_mode"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Transaction Charge :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Purpose :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_purpose"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Amount To Be Recieved :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Transaction Source :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_source"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Peso Equivalent :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Source of Fund :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_source_of_fund"></label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    <tr>                                                        
                                                            <td colspan="2" style="text-align: center"><input id="btn_sendmoney" type="button" value="SEND MONEY" style="width: 23%; height: 24px" /></td>
                                                    </tr>
                                                </table>
                                        </center>
                                            </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top">
                                            <table id = "tbl_left_and_right" cellpadding = "0" cellspacing = "2" style ="width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
                                                <tr>
                                                    <td style="width: 50%; height: 100%; vertical-align:top">
                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table3">
                                                                <tr><td class = "table_caption" colspan="2" style ="text-align:center">REMITTER</td></tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Remitter Name :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%;"><label id = "lbl_remitter_name"></label></td>
                                                                        
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Address :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%;"><label id = "lbl_remitter_address"></label></td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%; height: 16px;">Country :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%; height: 16px;"><label id = "lbl_remitter_country"></label></td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Zip Code :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%;"><label id = "lbl_remitter_zip"></label></td>
                                                                        
                                                                </tr>
                                                            </table>
                                                        </td>                    
                                                    <td style="width: 50%; height: 100%; vertical-align:top">                                                        
                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table4">
                                                                <tr>
                                                                    <td class = "table_caption" colspan="2" style ="text-align:center">BENEFICIARY</td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Beneficiary &nbsp;Name :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%;"><label id = "lbl_beneficiary_name"></label></td>                                                                    
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Address :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%;"><label id = "lbl_beneficiary_address"></label></td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%; height: 16px;">Country :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%; height: 16px;"><label id = "lbl_beneficiary_country"></label></td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%; height: 16px;">Zip Code :</td>
                                                                    <td class = "cell_desc" style="text-align: left; width: 70%"><label id = "lbl_beneficiary_zip"></label></td>
                                                                </tr>
                                                            </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top"><table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table5">
                                            <tr>
                                                <td class = "table_caption" colspan="4" style ="text-align:center">ACCOUNT INFORMATION</td>
                                            </tr>
                                            <tr class = "cell_caption" style ="height: 19px; text-align:center">
                                                <td>Account Number</td>                                                    
                                                <td>Account Type</td>
                                                <td>Account Currency</td>
                                                <td>Bank Name</td>
                                            </tr>
                                            <tr class = "cell_desc" style="height: 19px; text-align:center">
                                                <td><label id = "lbl_account_number"></label></td>
                                                <td><label id = "lbl_account_type"></label></td>
                                                <td><label id = "lbl_account_currency"></label></td>
                                                <td><label id = "lbl_account_bankname"></label></td>
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
        </center>
        <br />
    </form>
</body>
</html>
