<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranGeneric.aspx.cs" Inherits="IRSWorld.Remittance.RemTranGeneric" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">

    <title>Remittance Module</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>    
    <script src = "../js/jsCommons.js"                      type = "text/javascript"></script>
    <script src = "../js/jscommonmethod.js"                 type = "text/javascript"></script>
    <script src = "../js/js_sidebar.js"                     type = "text/javascript"></script>
    <script src = "../js/jshttprequest.js"                  type = "text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"                  type = "text/javascript"></script>
    <script src = "../js/jsgridselector.js"                 type = "text/javascript"></script>    
    <script src = "../js/Remittance/jsremsaving.js"         type = "text/javascript"></script>
    <script src = "../js/Remittance/jsremcommons.js"        type = "text/javascript"></script>
    <script src = "../js/Remittance/js_rem_tran_generic.js" type = "text/javascript"></script>
    
    <script type="text/javascript">
        var remittance = new rem_transactions();
    </script>

    
</head>

<body onload = "javascript:remittance.begin_process();">
    <form id="form1" runat="server">
        <div id="div_content_top" >
            <table style="border: solid 1px #336699; width:100%;" cellspacing="1" cellpadding="0" >                    
                <tr>
                    <td style="width: 5%; text-align: center; height: 20px" class="cell_caption">PURPOSE</td>
                    <td style="width: 5%; text-align: center; height: 20px" class="cell_caption">SOURCE OF FUND</td>
                    <td style="width: 5%; text-align: center; height: 20px" class="cell_caption">TRANSACTION SOURCE</td>
                </tr>
                <tr>
                    <td style="width: 5%; text-align: center;"><label id="lbl_purpose" class="cell_desc"></label></td>
                    <td style="width: 5%; text-align: center;"><label id="lbl_source_of_funds" class="cell_desc"></label></td>
                    <td style="width: 5%; text-align: center;"><label id="lbl_transaction_source" class="cell_desc"></label></td>
                </tr>                    
            </table>            
        </div>        
       
       <br style="line-height:3px"/>        
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 370px;" class="content_height" colspan = "2">
                        <table style="width:100%">
                            <tr>
                                <td id = "td_remittance" valign="top" style="width:18%; vertical-align:top; padding-top: 10px;"><div id = "div_transaction_menu" style="margin-top: 20px;display:block; overflow:auto"></div><a id="a_hide_btn" href="#" onclick="javascript:remittance.show_or_close_side_bar();">«</a></td>
                                <td style="width: 90%">
                                    <table cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure" id="TABLE3">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:1%;height:24px;"> <img src="../images/toe.GIF" /></td>                                                    
                                            <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">REMITTANCE</td>                                                
                                            <td align="left" valign="bottom" style="height: 24px"  class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>                                                     
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 100%">
                                                <table style="width:100%; height: 100%;" class="table_border">                                                            
                                                    <tr>
                                                        <td style="width: auto; height: auto; vertical-align: top">                                                                    
                                                             <!-- content starts here -->
                                                             <table style="width:100%" cellpadding="0" cellspacing="2px" id="tbl_contents">
                                                                        <tr>
                                                                            <td style="width: 50%; height:auto; vertical-align: top;">
                                                                               <div style="width:100%; margin: 0" class="table_body" id="div_main_info" >                                                                                                                 
                                                                                            <table cellpadding="0" cellspacing="2" style="width:100%; border: solid 1px #336699" id="TABLE2">
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 17%">Branch :</td>
                                                                                                    <td align="left" style="width: 30%; height: 23px"><label id="lbl_transacting_branch" class="cell_desc"></label></td>                                                                                                        
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 17%">Product Type :</td>
                                                                                                    <td align="left" style="width: 30%; height: 23px"><label id="lbl_product_type" class="cell_desc"></label></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 17%">Implementation :</td>
                                                                                                    <td align="left" style="width: 30%; height: 23px"><label id="lbl_implementation_type" class="cell_desc"></label></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 17%">Transaction Status :
                                                                                                    </td>
                                                                                                    <td align="left" style="width: 30%; height: 23px" ><label class="cell_desc" id="lbl_txn_status"></label></td>                                                                                                                                                                                                
                                                                                                 </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 17%">Charge Type :</td>
                                                                                                    <td class="cell_desc" align="left" style="width: 30%; height: 23px"><label id="lbl_charge_type" class="cell_desc"></label></td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <br style="line-height:3px"/>
                                                                                            <table id="tbl_remitter_info" cellpadding="0" cellspacing="2" style="width:100%; border: solid 1px #336699;">
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 15%; height: 23px">Remitter :</td>
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_rem_fullname" class="cell_desc"></label></td>                                                                                                                                
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 15%; height: 23px">Address :</td>                                                                                            
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_rem_address" class="cell_desc"></label></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary :</td>                                                                                                                                
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_bene_fullname" class="cell_desc"></label></td>                                                                                                                            
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 15%; height: 23px">Address :</td>                                                                                                                                
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_bene_address" class="cell_desc"></label></td>                                                                                                                            
                                                                                                </tr>
                                                                                            </table>
                                                                                            <br style="line-height:3px"/>                                                                                         
                                                                                            <table id="Table1" cellpadding="0" cellspacing="2" style="width:100%; border: solid 1px #336699;">
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 33%; height: 23px">Bank :</td>                                                                                                                            
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_bank_name" class="cell_desc"></label></td>                                                                                                                        
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 33%; height: 23px">Account No :</td>                                                                                                                            
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_account_no" class="cell_desc"></label></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 33%; height: 23px">Account Type :</td>                                                                                                                            
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_account_type" class="cell_desc"></label></td>
                                                                                                    
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="cell_caption" style="width: 33%; height: 23px">Account Currency :</td>
                                                                                                    <td align="left" style="width: 85%"><label id="lbl_account_currency" class="cell_desc"></label></td>                                                                                                                        
                                                                                                </tr>
                                                                                            </table>
                                                                                    </div>                                                        
                                                                        </td>
                                                                        <td style="vertical-align: top; width:auto; height: auto;">
                                                                            <div style="width:100%; margin: 0" class="table_body" id="div1">
                                                                                <table style="border: solid 1px #336699; width:100%;" cellspacing="1" cellpadding="0">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; text-align: right; height:24px">Pay Mode :</td>                                                                                            
                                                                                        <td class="cell_desc" style="text-align: left;" colspan="2"><label id="lbl_pay_mode" class="cell_desc"></label></td>                                                                                            
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; text-align:right; height:24px">Computation :</td>
                                                                                        <td class="cell_desc" style="text-align: left"><label class="cell_desc" id="lbl_computation_type"></label></td>
                                                                                        <td class="cell_desc" colspan="1" style="text-align: left"><input id="btn_compute_method" style="width: 91%" type="button" value="CHANGE" onclick = "javascript:remittance.change_compute_method();"/></td>
                                                                                    </tr>
                                                                                    <tr id = "tr_amount">
                                                                                        <td class="cell_caption" style="width: 30%; text-align:right"><span style="color: #993300">&nbsp;AMOUNT :</span></td>
                                                                                        <td class="cell_desc" style="text-align: left; width: 41%;"><input id="txt_remitting_amount" style="text-align:right; color: #993333;" type="text" onkeyup = "javascript:remittance.compute_charges();"/></td>
                                                                                        <td class="cell_desc" style="height: 24px; text-align: left"><input id="btn_remit" style="width: 91%; color: #993333;" type="button" value="REMIT" onclick = "javascript:remittance.commit_transaction();"/></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br style="line-height:3px"/>
                                                                            <div style="width:100%; margin: 0" class="table_body" id="div_computed_charges"></div>
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
                    </td>
                </tr>
        
    </form>
</body>
</html>
