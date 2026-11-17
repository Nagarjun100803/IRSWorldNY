<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemOTC.aspx.cs" Inherits="IRSWorld.Remittance.RemOTC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <title>OVER THE COUNTER - REMITTANCE</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href  = "../css/Default_min.css" rel="stylesheet" type = "text/css" />    
    <script src = "../js/commons_min.js"                    type = "text/javascript"></script>
    <script src = "../js/popcalendar_min.js"                type = "text/javascript"></script>       
    <script src = "../js/jscommonmethod_min.js"             type = "text/javascript"></script>
    <script src = "../js/js_sidebar_min.js"                 type = "text/javascript"></script>
    <script src = "../js/jshttprequest_min.js"              type = "text/javascript"></script>
    <script src = "../js/jsmodalwindow_min.js"              type = "text/javascript"></script>
    <script src = "../js/jsgridselector_min.js"             type = "text/javascript"></script>    
    <script src = "../js/Remittance/jsRemOTC_min.js"        type = "text/javascript"></script>        
    <script src = "../js/Remittance/jsremsaving_min.js"     type = "text/javascript"></script>
    <script src = "../js/Remittance/jsRemExternals_min.js"  type = "text/javascript"></script>    
    <script src = "../js/CardRem/jsGFMCMaintenance_min.js"  type = "text/javascript"></script>
    <script src = "../js/RemoteOverride/js_RemOverride.js" type = "text/javascript"></script>
    <script src = "../js/js_generic_min.js" type = "text/javascript"></script> 
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-migrate-3.3.1.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/bootstrap.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/bootstrap.bundle.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/buttons.dataTables.min.css" rel="stylesheet" />
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/responsive.dataTables.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery.dataTables.min.js"></script>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/dataTables.responsive.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/waitMe.min.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/waitMe.min.js"></script>
    <link href="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-ui.css" rel="stylesheet" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-ui.min.js"></script>
    <style>
        .nav-pills .nav-link.active, .nav-pills .show>.nav-link {
            color: #f0f8ff;
            background-color: #5697c1;
        }
        .nav-link {
            font-size: small;
        }
        table.dataTable thead th, table.dataTable thead td {
            padding: 0px 0px;
            font-size: small;
            background-color: #5697c1;
            color: #f0f8ff;
            border-left: 6px solid #5697c1;
        }
        .table-bordered>:not(caption)>*>* {
            border-width: 1px 1px;
        }
        table.dataTable tbody tr {
            font-size:small;
            padding: 0px 0px;
            border-left: 6px solid lightgray;
        }
        /*#tblHitList tbody tr {
            font-size:small;
            padding: 0px 0px;
        }*/
        table.dataTable tbody th, table.dataTable tbody td {
            padding: 3px 8px;
        }
        .form-control:focus {
            box-shadow: 0 0 0 0rem rgb(13 110 253 / 25%);
        }
        .nav-link {
            color: #3f769a;
            font-weight: bold;
        }
    </style>    
    <script type="text/javascript">
        function validate_input_ach(evt)
        {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 47 && charCode < 58) || charCode == 127) 
            {
                return true;
            }
            return false;
        }
    </script>        
</head>
   
<body onload = "javascript:iremit.get_transacting_branch();" onkeydown = "javascript:iremit.check_current_irsw_session(event);">
    <form id="form1" runat="server">
        <center>
             <table id = "tbl_outermost" cellpadding = "10"; cellspacing = "0" style = "width:100%">
                <tr>
                    <td id="td_remittance" style="vertical-align: top; width:220px; height: 587px; padding-top: 10px"><div id="div_transaction_menu" style="margin-top: 20px; display: block; overflow: auto"></div><a id="anchor_off" href="#" onclick="javascript:iremit.load_unload_sidebar();">«</a></td>
                    <td style="width:auto; height: 587px;">
                        <table id = "tbl_folder_content" cellpadding="0" cellspacing="0" style="width:auto;background-color: azure">
                            <tr>
                                <td style ="text-align:left; width:100%">
                                    <table cellpadding="0" cellspacing="0" style ="width:100%" >
                                        <tr>
                                            <td align = "left" style = "width:10px; height:auto; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                            <td align = "left" style = "width: 45%; height:auto; border-collapse:collapse" class="table_head_dark" >OVER THE COUNTER - REMITTANCE</td>
                                            <td align = "left" style = "width: 59%; height:auto; vertical-align:bottom; border-collapse:collapse"><img align="absBottom" src="../images/rend.gif" /></td> 
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                            <td colspan="3">
                                <table id = "tbl_content_container" class="table_border" style="width:100%;height:100%">
                                    <tr>
                                        <td style="height: 100%">
                                            <div id = "div_otc_transaction_details"; style="width:100%; height:200px; overflow: auto;">
                                                <center>                                                
                                                    <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="TABLE1">
                                                    <tr><td class = "table_caption" colspan="2" style ="text-align:center"><label id = "lbl_otc_caption"></label> </td></tr>
                                                        <tr>
                                                            <td style="text-align: center" colspan="2">
                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%" id="Table2">
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Computation Type :</td>                                                                            
                                                                        <td class="cell_desc" style="width: 30%; height: 19px; text-align: left">
                                                                                <select id="ddl_compute_mode" style="width: 50%" onchange = "javascript:iremit.set_product_charges_id();">
                                                                                    <option selected="selected"></option>
                                                                                </select>                                                                            
                                                                                <input id="txt_swipe_container" type="text" style="width: 1px; text-decoration: none; border-style:none; cursor:text; color:White" />
                                                                        </td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Transaction Date :</td>                                                                            
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_date"></label></td>
                                                                            
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style="width: 20%; text-align: right; font-weight: bold;">Amount To Send :</td>
                                                                        <td class="cell_desc" style="width: 30%; height: 19px; text-align: left; font-weight: bold;">
                                                                            <input id="txt_remit_amount" type="text" style="width: 50%"/><br /><em><span style="color: red">(PRESS ENTER TO COMPUTE CHARGES)</span></em></td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Transaction Status :</td>
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_status"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" rowspan="3" style="width: 20%; text-align: right">Message :</td>
                                                                        <td class="cell_desc" rowspan="3" style="width: 30%; text-align: left"><textarea id="txt_messages" cols="20" rows="3" style="width: 95%"></textarea></td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Bank Name :</td>
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_branch_name"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style   = "width: 20%; text-align: right">Account Currency :</td>
                                                                        <td class="cell_desc"    style   = "width: 30%; text-align: left"><label id = "lbl_account_currency"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style = "width: 20%; height: 19px; text-align: right">Account Number :</td>
                                                                        <td class="cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_account_number"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Remitter Account No (optional) :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><input type="text" id="txt_account_no" style="width:226px" /></td>                                                                                                                                                        
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Product Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; text-align: left"><label id = "lbl_product_type"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Remitting Currency :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_remitting_currency"></label></td>                                                                            
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Implementation Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; text-align: left"><label id = "lbl_implementation_type"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Charge Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_charge_type"></label> </td>                                                                            
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Receipt Mode :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; text-align: left"><label id = "lbl_receipt_mode"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style = "text-align: right; width: 20%;">Transaction Charge :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_transaction_charge"></label></td>                                                                            
                                                                        <td class="cell_caption" rowspan="2" style="width: 20%; text-align: right">Purpose :</td>
                                                                        <td class="cell_desc" rowspan="2" style="width: 30%; text-align: left"><label id = "lbl_purpose"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Amount To Be Paid :</td>
                                                                        <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_amount_to_be_paid"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%; height: 19px;">Amount To Be Received :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_beneficiary_amount"></label></td>                                                                            
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right; height: 19px;">Transaction Source :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left; height: 19px;"><label id = "lbl_transaction_source"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Peso Equivalent :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_peso_equivalent"></label></td>                                                                            
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Source of Fund :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_source_of_fund"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "" style="text-align: right; width: 20%; height: 19px;"></td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"></td>                                                                            
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right; height: 19px;">Payment Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_payment_type"></label></td>
                                                                    </tr>
                                                                </table>
                                                                </td>
                                                        </tr>
                                                </table>
                                            </center>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top">                                            
                                            <table id = "tbl_left_and_right" cellpadding = "0" cellspacing = "2" style ="width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
                                                <tr>
                                                    <td style="height: 100%">
                                                        <div style="width:100%; height:200px; overflow: auto;">
                                                            <center>
                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="TABLE2">
                                                                    <tr>
                                                                        <td style="width: 50%; height: 100%; vertical-align:top">
                                                      
                                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table3">
                                                                                    <tr>
                                                                                        <td class = "table_caption" colspan="2" style="text-align:left;width:100%">&nbsp;REMITTER</td>
                                                                                        <td class="table_caption" colspan="1" style="text-align: right; width:auto"><input id="btn_otc_remitter_search" type="button" value="SEARCH" style="width: 120px; height: 24px" onclick = "javascript: iremit.clear_session_pendingovr();" /></td> <!--CJ Changed for Clearing Pending ovr Session-->
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 30%; text-align: right">Customer ID :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;"><label id = "lbl_remitter_id"></label></td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 30%;">Remitter Name :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;"><label id = "lbl_remitter_name"></label></td>                                                                        
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 30%;">Address :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;"><label id = "lbl_remitter_address"></label></td>
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 30%; text-align: right">Telephone No. :</td>
                                                                                        <td class="cell_desc" colspan="2" style="width: 529px; text-align: left"><label id = "lbl_rem_tel"></label></td>
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 30%; text-align: right">Mobile No. :&nbsp;</td>
                                                                                        <td class="cell_desc" colspan="2" style="width: 529px; text-align: left"><label id = "lbl_rem_mobile"></label></td>
                                                                                    </tr>                                                                
                                                                                </table>
                                                                            </td> 
                                                                           
                                                                        <td style="width: 50%; height: 100%; vertical-align:top">                                                        
                                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table4">
                                                                                    <tr>
                                                                                        <td class = "table_caption" colspan="2" style="text-align:left;width:100%">&nbsp;BENEFICIARY</td>
                                                                                        <td class="table_caption" colspan="1" style="text-align: right; width:auto">
                                                                                            <input id="btn_otc_beneficiary_search" type="button" value="SEARCH" style="width: 120px; height: 24px" onclick = "javascript:iremit.check_current_irsw_session('OPEN_BENEFICIARY');" /></td>
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 35%; text-align: right">Customer ID :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left"><label id = "lbl_beneficiary_id"></label></td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 35%;">Beneficiary &nbsp;Name :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left"><label id = "lbl_beneficiary_name"></label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 35%;">Address :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left"><label id = "lbl_beneficiary_address"></label></td>
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 35%; text-align: right">Telephone No. :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left"><label id = "lbl_ben_tel"></label></td>
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 35%; text-align: right">Mobile No. :</td>
                                                                                        <td class="cell_desc" colspan="2" style="text-align: left"><label id = "lbl_ben_mobile"></label></td>
                                                                                    </tr>
                                                                                </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 50%; height: 100%; vertical-align:top"; colspan="1">
                                                                            <div id="divRemAccountDetails"; style="width:100%; height:100%; display:none">
                                                                                <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table5">
                                                                                    <tr>
                                                                                        <td class = "table_caption" colspan="2" style="text-align:left;width:100%">&nbsp;REMITTER ACCOUNT DETAILS</td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Remitter Account Type :</td>
                                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left">
                                                                                            <select id="ddRemAccountType" style="width: 228px">
                                                                                                        <option value="0"></option>
                                                                                                        <option value="1">Savings</option>
                                                                                                        <option value="2">Current</option>
                                                                                                    </select>                                
                                                                                            <label id="astRemAccountType"; style="color : Red">*</label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="height: 19px">
                                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Remitter Account No :</td>
                                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><input type="text" id="txtRemAccountNo" style="width:230px" onkeypress="return validate_input_ach(event);" /><label id="astRemAccountNo"; style="color : Red">*</label></td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Remitter Routing No :</td>
                                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><input type="text" id="txtRemRoutingNo" style="width:230px" onkeypress="return validate_input_ach(event);" /><label id="astRemRoutingNo"; style="color : Red">*</label></td>
                                                                                    </tr>
                                                                                    <tr style ="height: 19px">
                                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Remitter Bank Name :</td>
                                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><input type="text" id="txtRemBankName" style="width:230px" /><label id="astRemBankName"; style="color : Red">*</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td> 
                                                                    </tr>
                                                               </table>
                                                            </center>
                                                        </div>
                                                  </td>
                                               </tr>
                                            </table>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; text-align:center">
                                            <input id="btn_sendmoney" type="button" value="SEND MONEY" style="width: 18%; height: 24px"                     onclick = "javascript:iremit.check_current_irsw_session('COMMIT_REMITTANCE');" />
                                            <input id = "btn_waiver"    type = "button" value = "WAIVE CHARGES"  style = "width: 18%; height: 24px"         onclick = "javascript:iremit.check_current_irsw_session('OPEN_WAIVED');" />
                                            <input id = "btn_override"  type = "button" value = "OVERRIDE"       style = "width: 18%; height: 24px"         onclick = "javascript:iremit.check_current_irsw_session('OPEN_OVERRIDE');" />
                                            <input id = "btn_breakdown" type = "button" value = "SHOW BREAKDOWN" style = "width: 18%; height: 24px"         onclick = "javascript:iremit.check_current_irsw_session('OPEN_BREAKDOWN');" />
                                            <input id = "btn_rem_aggregation" type="button" value="REMITTANCE AGGREGATION" style="width: 25%; height: 24px" onclick = "javascript:iremit.check_current_irsw_session('OPEN_AGGREGATES');" />
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
    </form>
</body>
</html>
