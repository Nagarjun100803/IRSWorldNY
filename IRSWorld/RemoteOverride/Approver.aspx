<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Approver.aspx.cs" Inherits="IRSWorld.RemoteOverride.Approver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Approver</title>
    <!--
    <style>
    
    div.divMonitor {position:fixed;background:#fff4c8;border:1px solid #ffcc00;width:150px;z-index:100;}
    div.divMonitor a, div.floating-menu h3 {display:block;margin:0 0.5em;}
    
    </style>
    -->
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />    
    <script src="../js/jsCommons.js"                        type="text/javascript"> </script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript" > </script>
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"  ></script>
    <script src = "../js/Remittance/jsRemExternals.js"  type = "text/javascript"></script>
    <script src="../js/RemoteOverride/js_RemApprover.js"    type="text/javascript" ></script>
</head>

<body style="text-align: center" onload ="javascript:rapopups.construct_ra_objects();" >
<center>
<form id="form1" runat="server">

<!--
    <div id="divMonitor" style="background-color:silver ;display:none;width:450px;" >                
         <table style="width: 408px; height: auto">            
            <tr>
                <td style="height:auto;">
                <input type="hidden" id="hseqid" value=""/>
                <div id="divforApproval">                    
                </div>
                </td>              
            </tr>
            <tr>
                <td>
                <table class="cell_desc" border="0" cellpadding ="0" cellspacing ="0" style="width: 288px">
                    <tr>
                        <td class="table_caption" style="width: 341px; height: 29px;" colspan="2">
                        </td>
                        
                    </tr>
                <tr><td class="cell_caption" style="height: 29px">Password:&nbsp;
                </td><td style="width:50%;text-align:left; height: 29px;" class="cell_caption">
                    <input id="txtpassword" type="password" /></td></tr>
                <tr style="text-align:center ;" class="cell_caption">
                <td colspan="2" style="height: 32px">
                   <input id="btnApprove" type="button" value="Approve" onclick="javascript:ra.approve_click(2);" style="width: 28%"/>
                    &nbsp;
                   <input id="btnDisapprove" type="button" value="Disapprove" onclick="javascript:ra.approve_click(3);" style="width: 29%" />
                   </td>
                   </tr>
                   </table>
                </td>
            </tr>
            <tr>
                <td>
                    <input type="hidden" id="txtApproverID" size ="10" maxlength="10" value="sysadm1"/>
                    <input type="hidden" id="hStatus" value ="not busy"  />
                    <input type="hidden" id="hRole" value ="Admin"  />
                </td>
            </tr>
            
        </table>
    </div>
    -->
    <div style="width:90%">
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
                                                                                <label id="ddl_compute_mode"></label>
                                                                            &nbsp;</td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Transaction Date :</td>                                                                            
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_date"></label></td>
                                                                            
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" style="width: 20%; text-align: right; font-weight: bold;">Amount To Send :</td>
                                                                        <td class="cell_desc" style="width: 30%; height: 19px; text-align: left; font-weight: bold;"><label id="txt_remit_amount"></label></td>
                                                                        <td class="cell_caption" style="width: 20%; text-align: right">Transaction Status :</td>
                                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_status"></label></td>
                                                                    </tr>
                                                                    <tr style="height: 19px">
                                                                        <td class="cell_caption" rowspan="3" style="width: 20%; text-align: right">Message :</td>
                                                                        <td class="cell_desc" rowspan="3" style="width: 30%; text-align: left"><textarea id="txt_messages" cols="20" rows="3" style="width: 95%" disabled="disabled"></textarea></td>
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
                                                                        <td class = "cell_caption" style = "width: 20%; text-align: right">Payment Type :</td>
                                                                        <td class = "cell_desc"    style = "width: 30%; height: 19px; text-align: left"><label id = "lbl_payment_type"></label></td>                                                                            
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
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Amount To Be Received :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_beneficiary_amount"></label></td>                                                                            
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Transaction Source :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_transaction_source"></label></td>
                                                                    </tr>
                                                                    <tr style ="height: 19px">
                                                                        <td class = "cell_caption" style="text-align: right; width: 20%;">Peso Equivalent :</td>
                                                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_peso_equivalent"></label></td>                                                                            
                                                                        <td class = "cell_caption" style="width: 20%; text-align: right">Source of Fund :</td>
                                                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lbl_source_of_fund"></label></td>
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
                                                                    <td class="table_caption" colspan="1" style="text-align: right; width:auto"><input id="Button1" type="button" value="SEARCH" style="width: 120px; height: 24px"  disabled="disabled" /></td>
                                                                </tr>
                                                                <tr style="height: 19px">
                                                                    <td class="cell_caption" style="width: 30%; text-align: right">Customer ID :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;">
                                                                        <label id = "lbl_remitter_id"></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Remitter Name :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;">
                                                                        <label id = "lbl_remitter_name"></label>
                                                                    </td>
                                                                        
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="text-align: right; width: 30%;">Address :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left; width: 529px;">
                                                                        <label id = "lbl_remitter_address"></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>                    
                                                    <td style="width: 50%; height: 100%; vertical-align:top">                                                        
                                                            <table cellpadding = "0"; cellspacing = "1" style = "width:100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" id="Table4">
                                                                <tr>
                                                                    <td class = "table_caption" colspan="2" style="text-align:left;width:100%">&nbsp;BENEFICIARY</td>
                                                                    <td class="table_caption" colspan="1" style="text-align: right; width:auto">
                                                                        <input id="Button2" type="button" value="SEARCH" style="width: 120px; height: 24px"  disabled="disabled" /></td>
                                                                </tr>
                                                                <tr style="height: 19px">
                                                                    <td class="cell_caption" style="width: 30%; text-align: right">Customer ID :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left">
                                                                        <label id = "lbl_beneficiary_id"></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="width: 40%; text-align: right;">Beneficiary &nbsp;Name :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left">
                                                                        <label id = "lbl_beneficiary_name"></label>
                                                                    </td>
                                                                </tr>
                                                                <tr style ="height: 19px">
                                                                    <td class = "cell_caption" style="width: 30%; text-align: right;">Address :</td>
                                                                    <td class="cell_desc" colspan="2" style="text-align: left">
                                                                        <label id = "lbl_beneficiary_address"></label>
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
                                <input id="btnApprove" type="button" value="Approve" onclick="javascript:ra.display_password_prompt(2);" style="width: 18%; height: 24px" disabled="disabled"   />    
                                <input id="btnDisapprove" type="button" value="Disapprove" onclick="javascript:ra.display_password_prompt(3);" style="width: 18%; height: 24px;" disabled="disbled" />                            
                                <input id="btnCancel" type="button" value="Cancel" onclick="javascript:ra.approve_click(4);" style = "width: 18%; height: 24px" disabled="disabled" />
                              <!--  <input id="btnViewOverride" type="button" value="View Override" onclick="javascript:ra.approve_click(4);" style = "width: 18%; height: 24px" disabled="disabled" /> -->
                                <input type="hidden" id="txtApproverID"  value=""/>
                                <input type="hidden" id="hStatus" value ="not busy"  />
                                <input type="hidden" id="hRole" value ="Admin"  />
                                <input type="hidden" id="hseqid" value=""/>
                                <input type="hidden" id="hAppstat" value="" />
                                &nbsp;
                            </td>
                        </tr>
                                </table>
                            </td>
                        </tr>

                       
                    </table>    
    </div>
    
    
    
    </form>
    
     <div id="divPassword" style="position:absolute;z-index:101; background-color: azure ;display:none;width:240px; height: auto;border:solid 1px midnightblue" >                
        <table class="cell_desc" border="0" cellpadding ="0" cellspacing ="0" style="width: 240px">
            <tr>
                <td class="table_caption" style="height: 29px;" colspan="2">
                &nbsp;
                </td>
            </tr>
            <tr>
                <td class="cell_caption" style="height: 29px; ">Password:&nbsp;
                </td>
                <td style="text-align:left; width:150px;height: 29px;" class="cell_caption">
                <input id="txtpassword" type="password"  onkeypress="javascript:ra.handle_keypress(event);" />
                </td>
            </tr>
            <tr style="text-align:center ;" class="cell_caption" >
                <td colspan="2" style="height: 32px" nowrap>
                <input id="btnOK" type="button" value="Confirm" onclick="javascript:ra.get_approve_stat();" style="width: 33%" />
                &nbsp;
                <input id="btnCancelPwd" type="button" value="Cancel" onclick="javascript:ra.approve_click(4);" style="width: 33%; height: 20px" />
                </td>
            </tr>
        </table>
            
        
    </div>
    </center>
</body>

</html>

