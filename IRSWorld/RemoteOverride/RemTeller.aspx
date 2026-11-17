<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTeller.aspx.cs" Inherits="IRSWorld.RemoteOverride.RemTeller" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Teller</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />    
    <script type="text/javascript" src="../js/jsCommons.js"> </script>
    <script type="text/javascript" src="../js/jscommonmethod.js"> </script>
    <script type="text/javascript" src="../js/Remittance/jsRemExternals.js"> </script>
    <script type="text/javascript" src="../js/RemoteOverride/js_RemOverride.js"> </script>
</head>
<body >
    <form id="form1" runat="server">
    <div id="divOTC" style="text-align: center">
    
    <table id = "tbl_folder_content" cellpadding="0" cellspacing="0" style="width:auto;background-color: azure">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom"><img src="../images/toe.GIF" style="width: 11px; height: 24px"/></td>
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
                                                    <tr><td class = "table_caption" colspan="2" style ="text-align:center"><label id = "lbl_otc_caption"></label> </td></tr>
                                                        <tr>
                                                            <td style="text-align: center" colspan="2">
                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%" id="Table2">
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Computation Type :</td>                                                                            
                                                                        <td class="cell_desc" style="width: 30%; height: 19px; text-align: left">
                                                                                <select id="ddl_compute_mode" style="width: 50%" >
                                                                                    <option selected="selected" value="1">SAMPLE1</option>
                                                                                    <option value="2">SAMPLE2</option>
                                                                                    <option></option>
                                                                                </select>
                                                                            
                                                                        </td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Transaction Date :</td>                                                                            
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_date">09/07/19/76</label></td>
                                                                            
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style="width: 20%; text-align: right; font-weight: bold;">Amount To Send :</td>
                                                                        <td class="cell_desc" style="width: 30%; height: 19px; text-align: left; font-weight: bold;"><input id="txt_remit_amount" type="text" style="width: 50%" /><br /><em><span style="color: red">(PRESS ENTER TO COMPUTE CHARGES)</span></em></td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Transaction Status :</td>
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_status">s</label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" rowspan="3" style="width: 20%; text-align: right">Message :</td>
                                                                        <td class="cell_desc" rowspan="3" style="width: 30%; text-align: left"><textarea id="txt_messages" cols="20" rows="3" style="width: 95%"></textarea></td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Bank Name :</td>
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_branch_name">PNB</label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style   = "width: 20%; text-align: right">Account Currency :</td>
                                                                        <td class="cell_desc"    style   = "width: 30%; text-align: left"><label id = "lbl_account_currency">PHP</label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style = "width: 20%; height: 19px; text-align: right">Account Number :</td>
                                                                        <td class="cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_account_number">111111111111</label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Payment Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_payment_type">A</label></td>                                                                            
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Product Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; text-align: left"><label id = "lbl_product_type">BPO</label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Remitting Currency :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_remitting_currency">USD</label></td>                                                                            
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Implementation Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; text-align: left"><label id = "lbl_implementation_type">DC</label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Charge Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_charge_type">BBB</label> </td>                                                                            
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Receipt Mode :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; text-align: left"><label id = "lbl_receipt_mode">XXXX</label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style = "text-align: right; width: 20%;">Transaction Charge :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_transaction_charge">1500</label></td>                                                                            
                                                                        <td class="cell_caption" rowspan="2" style="width: 20%; text-align: right">Purpose :</td>
                                                                        <td class="cell_desc" rowspan="2" style="width: 30%; text-align: left"><label id = "lbl_purpose">DEPOSIT</label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Amount To Be Paid :</td>
                                                                        <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_amount_to_be_paid">50000</label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Amount To Be Received :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_beneficiary_amount">48500</label></td>                                                                            
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Transaction Source :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_source">SOURCE</label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Peso Equivalent :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_peso_equivalent">150000</label></td>                                                                            
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Source of Fund :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_source_of_fund">SALARY</label></td>
                                                                    </tr>
                                                                </table>
                                                                </td>
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
                                                                <tr>
                                                                    <td class = "table_caption" colspan="2" style="text-align:left;width:100%">&nbsp;REMITTER</td>
                                                                    <td class="table_caption" colspan="1" style="text-align: right; width:auto"><input id="Button1" type="button" value="SEARCH" style="width: 120px; height: 24px" onclick = "javascript:iremit.check_current_irsw_session('CLIENT');" disabled="disabled" /></td>
                                                                </tr>
                                                                <tr style="height: 19px">
                                                                    <td class="cell_caption" style="width: 30%; text-align: right">Customer ID :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;">
                                                                        <label id = "lbl_remitter_id">1</label>
                                                                    </td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Remitter Name :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;">
                                                                        <label id = "lbl_remitter_name">aaa</label>
                                                                    </td>
                                                                        
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Address :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;">
                                                                        <label id = "lbl_remitter_address">bbb</label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>                    
                                                    <td style="width: 50%; height: 100%; vertical-align:top">                                                        
                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table4">
                                                                <tr>
                                                                    <td class = "table_caption" colspan="2" style="text-align:left;width:100%">&nbsp;BENEFICIARY</td>
                                                                    <td class="table_caption" colspan="1" style="text-align: right; width:auto">
                                                                        <input id="Button2" type="button" value="SEARCH" style="width: 120px; height: 24px" onclick = "javascript:iremit.check_current_irsw_session('OPEN_BENEFICIARY');" disabled="disabled" /></td>
                                                                </tr>
                                                                <tr style="height: 19px">
                                                                    <td class="cell_caption" style="text-align: right">Customer ID :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left">
                                                                        <label id = "lbl_beneficiary_id">2</label>
                                                                    </td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right;">Beneficiary &nbsp;Name :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left">
                                                                        <label id = "lbl_beneficiary_name">cccc</label>
                                                                    </td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right;">Address :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left">
                                                                        <label id = "lbl_beneficiary_address">ddddd</label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </td>
                                    </tr>
                                     <tr>
                            <td style="vertical-align: top; text-align:center">
                                <input id="tbnWaive" type="button" value="Waive" onclick="javascript:ro.prepare_override(838,1);" style="width: 18%; height: 24px"   />                                
                                <input id="btnOverride" type="button" value="Override" onclick="javascript:ro.prepare_override(838,2);" style = "width: 18%; height: 24px" /> 
                                &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    

    
    </div>
    </form>
</body>
</html>
