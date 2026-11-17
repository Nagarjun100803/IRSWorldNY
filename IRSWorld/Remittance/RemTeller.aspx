<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTeller.aspx.cs" Inherits="IRSWorld.Remittance.RemTeller" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">

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
    <script src = "../js/Remittance/jsRemTranRemittanceCommons.js"  type = "text/javascript"></script>
    <script src = "../js/Remittance/jsRemTranRemittance.js"         type = "text/javascript"></script>    

    <script type="text/javascript">
        var remittance = new rem_transactions();
    </script>
</head>

<body onload = "javascript:remittance.begin_process();">                        
    <form id="form1" runat="server">    
        <table style="width:100%">
            <tr>
                <td id="td_remittance" style="vertical-align: top; width: 16%; padding-top: 10px; height: auto;"><div id="div_transaction_menu" style="margin-top: 20px; display: block; overflow: auto"></div><a id="a_hide_btn" href="#" onclick="javascript:remittance.show_or_close_side_bar();">«</a></td>
                <td style="width: 98%; height: auto;">
                    <table id="TABLE3" cellpadding="0" cellspacing="0" style="background-color: azure; width:100%">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom"><img src="../images/toe.GIF" /></td>
                            <td align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;height: 24px">REMITTANCE</td>
                            <td align="left" class="bg_body" style="height: 24px" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%; height: auto">
                                <table class="table_border" style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="vertical-align: top">
                                            <center>
                                                <table cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;border: solid 1px #336699">
                                                    <tr><td class = "table_caption" style="height: 20px; width: 780px;"><center><label id = "lbl_swapable"></label></center></td></tr>                                                        
                                                    <tr>
                                                        <td style="vertical-align: top">
                                                            <br style="line-height: 5px" />
                                                            <div id="div_general_info_ui" style="vertical-align: middle; width: 100%; height: 350px; overflow:scroll">
                                                            <table cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                <tr>
                                                                    <td style="width:50%; vertical-align:top"><div id = "div_calculator_review" style ="width:100%"></div></td>
                                                                    <td style="width:50%; vertical-align:top">
                                                                        <table id="Table1" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; text-align: center; height: 18px;">BRANCH CODE</td>                                                                        
                                                                                <td class="cell_caption" style="width: 10%; text-align: center; height: 18px;">PRODUCT TYPE</td>                                                                        
                                                                                <td class="cell_caption" style="width: 10%; text-align: center; height: 18px;">IMPLEMENTATION TYPE</td>                                                                        
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_desc" style="width: 10%; text-align: center; height: 18px;"><label id="lbl_transacting_branch"></label>&nbsp;</td>
                                                                                <td class="cell_desc" style="width: 10%; text-align: center; height: 18px;"><label id="lbl_product_type"></label>&nbsp;</td>
                                                                                <td class="cell_desc" style="width: 10%; text-align: center; height: 18px;"><label id="lbl_implementation_type"></label>&nbsp;</td>
                                                                            </tr>
                                                                        </table>
                                                                        <br style="line-height: 5px" />
                                                                        <table id="Table9" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; text-align: center; height: 18px;">CHARGE TYPE</td>
                                                                                <td class="cell_caption" style="width: 10%; text-align: center; height: 18px;">PAY MODE</td>
                                                                                <td class="cell_caption" style="width: 10%; text-align: center; height: 18px;">COMPUTATION TYPE</td>                                                                        
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_desc" style="width: 10%; text-align: center; height: 18px;"><label id="lbl_charge_type"></label>&nbsp;</td>                                                                        
                                                                                <td class="cell_desc" style="width: 10%; text-align: center; height: 18px;"><label id="lbl_pay_mode_2"></label>&nbsp;</td>                                                                        
                                                                                <td class="cell_desc" style="width: 10%; text-align: center; height: 18px;"><label id="lbl_computation_type_2"></label>&nbsp;</td>                                                                        
                                                                            </tr>
                                                                        </table>
                                                                        <br style="line-height: 5px" />
                                                                        <table id="Table2" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">TRANSACTION STATUS :</td>
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_txn_status"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">PURPOSE :</td>                                                                                    
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_purpose_2"></label>&nbsp;</td>                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">SOURCE OF FUND :</td>                                                                                    
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_source_of_funds_2"></label>&nbsp;</td>                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">TRANSACTION SOURCE :</td>                                                                                    
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_transaction_source_2"></label>&nbsp;</td>                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">REMITTER NAME :</td>                                                                        
                                                                                <td class="cell_desc"    style="width: 50%; height: 18px; text-align: left"><label id="lbl_rem_fullname" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%;height: 18px; text-align: right">REMITTER ADDRESS :</td>                                                                        
                                                                                <td class="cell_desc"    style="width: 50%; text-align: left; height: 18px;"><label id="lbl_rem_address" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%;height: 18px; text-align: right">BENEFICIARY NAME :</td>                                                                        
                                                                                <td class="cell_desc"    style="width: 50%; text-align: left; height: 18px;"><label id="lbl_bene_fullname" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%;height: 18px; text-align: right">BENEFICIARY ADDRESS :</td>
                                                                                <td class="cell_desc"    style="width: 50%; text-align: left; height: 18px;"><label id="lbl_bene_address" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">BANK NAME :</td>
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_bank_name" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">ACCOUNT NUMBER :</td>
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_account_no" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">ACCOUNT TYPE :</td>
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_account_type" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px; text-align: right">ACCOUNT CURRENCY :</td>                                                                        
                                                                                <td class="cell_desc" style="width: 50%; height: 18px; text-align: left"><label id="lbl_account_currency" class="cell_desc"></label>&nbsp;</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    
                                                                </tr>
                                                            </table>                                                            
                                                        </div>                                                        
                                                        <div id = "div_calculator_ui" style="vertical-align: middle; width: 100%; height: 350px; overflow:scroll">
                                                        <table id="Table8" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:95%">
                                                            <tr>
                                                                <td class="cell_caption" style="width: 3%; height: 18px; text-align: right">PAY MODE :</td>
                                                                <td class="cell_desc"    style="width: 10%; height: 18px; text-align: left"><label id="lbl_pay_mode"></label>&nbsp;</td>                                                                    
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 3%;height: 18px; text-align: right">COMPUTATION TYPE &nbsp;:</td>                                                                    
                                                                <td class="cell_desc"    style="width: 10%; text-align: left; height: 18px;"><label id = "lbl_computation_type"></label></td>                                                                    
                                                            </tr>
                                                            <tr id = "tr_amounts">
                                                                <td class="cell_caption" style="width: 3%; height: 18px; text-align: right">AMOUNT :</td>
                                                                <td class="cell_desc" style="width: 10%; height: 18px; text-align: left"><input id="txt_remitting_amount" onkeyup="javascript:remittance.chk_remitting_amount(event);" style="width: 41%" type="text" />&nbsp;(PRESS ENTER KEY TO COMPUTE)</td>
                                                            </tr>
                                                        </table>
                                                            <br style="line-height: 5px" />
                                                            <div id="div_computed_charges" style="width:95%"></div>
                                                        </div>                                                        
                                                        <div id = "div_purpose_ui" style="vertical-align: middle; width: 98%; height: auto">
                                                            <table id="Table5" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%; height: 18px; text-align: right">PURPOSE :</td>                                                                    
                                                                    <td class="cell_desc"    style="width: 10%; height: 18px; text-align: left"><label id="lbl_purpose" class="cell_desc"></label>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%;height: 18px; text-align: right">FIND &nbsp;:</td>                                                                    
                                                                    <td class="cell_desc"    style="width: 10%; text-align: left; height: 18px;"><input id="txt_find_purpose" type="text" onkeyup = "javascript:var search_plus = 'ajx_search$' + this.value; execute_ajaxgridview_command(search_plus);"/></td>
                                                                </tr><tr><td  colspan="2" style="height: 18px"><div id = "div_purpose_grd"></div></td></tr>
                                                            </table>
                                                        </div>
                                                         <div id = "div_source_of_fund_ui" style="vertical-align: middle; width: 98%; height: auto">
                                                            <table id="Table6" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%; height: 18px; text-align: right">SOURCE OF FUND :</td>                                                                        
                                                                    <td class="cell_desc"    style="width: 10%; height: 18px; text-align: left"><label id="lbl_source_of_funds" class="cell_desc"></label>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%;height: 18px; text-align: right">FIND &nbsp;:</td>                                                                    
                                                                    <td class="cell_desc"    style="width: 10%; text-align: left; height: 18px;"><input id="Text1" type="text" onkeyup = "javascript:var search_plus = 'ajx_search$' + this.value; execute_ajaxgridview_command(search_plus);"/></td>
                                                                </tr>
                                                                <tr><td  colspan="2" style="height: 18px"><div id = "div_source_of_fund_grd"></div></td></tr>
                                                            </table>
                                                        </div>                                                        
                                                         <div id = "div_transaction_source_ui" style="vertical-align: middle; width: 98%; height: auto">
                                                            <table id="Table7" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%; height: 18px; text-align: right">TRANSACTION SOURCE&nbsp; :</td>
                                                                    <td class="cell_desc"    style="width: 10%; height: 18px; text-align: left"><label id="lbl_transaction_source" class="cell_desc"></label>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%;height: 18px; text-align: right">FIND &nbsp;:</td>
                                                                    <td class="cell_desc"    style="width: 10%; text-align: left; height: 18px;"><input id="Text2" type="text" onkeyup = "javascript:var search_plus = 'ajx_search$' + this.value; execute_ajaxgridview_command(search_plus);"/></td>
                                                                </tr>
                                                                <tr><td  colspan="2" style="height: 18px"><div id = "div_transaction_source_grd"></div></td></tr>
                                                            </table>
                                                        </div>
                                                        <div id = "div_benef_accounts_ui" style="vertical-align: middle; width: 98%; height: auto">
                                                            <table id="Table4" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:100%">
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%; height: 18px; text-align: right">BENEFICIARY ACCOUNT NO.&nbsp; :</td>
                                                                    <td class="cell_desc"    style="width: 10%; height: 18px; text-align: left"><label id="lbl_account_no_selection" class="cell_desc"></label>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 3%;height: 18px; text-align: right">FIND &nbsp;:</td>
                                                                    <td class="cell_desc"    style="width: 10%; text-align: left; height: 18px;"><input id="Text3" type="text" onkeyup = "javascript:var search_plus = 'ajx_search$' + this.value; execute_ajaxgridview_command(search_plus);"/></td>
                                                                </tr>
                                                                <tr><td  colspan="2" style="height: 18px"><div id = "div_benef_accounts_grd"></div></td></tr>
                                                            </table>                                                            
                                                        </div>
                                                        <br style="line-height: 5px" />
                                                        </td>
                                                      </tr>
                                                </table>
                                            </center>
                                            <center>
                                                <div id = "div_button_below_ui" style ="width:100%">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 30%"><input id="btn_remit" onclick="javascript:remittance.commit_transaction();" style="width: 101%" type="button" value="REMIT" /></td>
                                                            <td style="width: 30%"><input id="btn_cancel" onclick="javascript:remittance.cancel_transaction();" style="width: 97%" type="button" value="CANCEL TRANSACTION" /></td>
                                                            <td style="width: 30%"><input id="btn_compute_method" onclick="javascript:remittance.change_compute_method();" style="width: 97%" type="button" value="CHANGE COMPUTATION TYPE" /></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </center>
                                         </td>   
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>    
            </tr>
        </table>


    </form>
</body>
</html>
