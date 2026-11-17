<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemOvrApproval.aspx.cs" Inherits="IRSWorld.RemOvrApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>REMOTE OVERRIDE SCREEN</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href  = "../css/Default_min.css" rel="stylesheet" type = "text/css" />
    <script src = "../js/commons_min.js"                    type = "text/javascript"></script>
    <script src = "../js/js_generic.js"                     type="text/javascript"></script>
    <script src = "../js/popcalendar_min.js"                type = "text/javascript"></script>
    <script src = "../js/jscommonmethod_min.js"             type = "text/javascript"></script>
    <script src = "../js/jshttprequest_min.js"              type = "text/javascript"></script>
    <script src = "../js/jsmodalwindow_min.js"              type = "text/javascript"></script>
    <script src = "../js/jsgridselector_min.js"             type = "text/javascript"></script>
    <script src = "../js/Remittance/jsRemExternals_min.js"      type = "text/javascript"></script>
    <script src = "../js/Remittance/jsremPendingOvr_min.js"     type = "text/javascript"></script>
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
        }
        .table-bordered>:not(caption)>*>* {
            border-width: 1px 1px;
        }
        table.dataTable tbody tr {
            font-size:small;
            padding: 0px 0px;
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
            font-weight:bold;
        }
    </style>
</head>
<body onload = "javascript:rem_ovr_apr.load_screen();">
    <form id="form1" runat="server" autocomplete="off">
        <div id = "div_remoteovrotc" style="width:100%; overflow:auto">
            <center>
                <table id="tbl_folder_content" cellpadding="0" cellspacing="0" style="width: auto; background-color: azure">
            <tr>
                <td style="width: 100%; text-align: left">
                    <table cellpadding="0" cellspacing="0" style="width: auto">
                        <tr>
                            <td align="left" style="vertical-align: bottom; width: 10px; border-collapse: collapse;
                                height: auto">
                                <img align="left" src="../images/toe.GIF" /></td>
                            <td align="left" class="table_head_dark" style="width: 45%; border-collapse: collapse;
                                height: auto">
                                REMOTE OVERRIDE</td>
                            <td align="left" style="vertical-align: bottom; width: 59%; border-collapse: collapse;
                                height: auto">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <table id="tbl_content_container" class="table_border" style="width: 900px; height: 100%">
                        <tr>
                            <td style="height: 100%">
                                <div id="div_pending_ovr" style="width:auto; height: 100%">
                                    <center>
                                    
                                    <table id="tbl_pending_search" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                                <tr>
                                                    <td class="table_caption" colspan="2" style="text-align: center">
                                                        <label id="Label2">
                                                            SEARCH CRITERIA</label></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="text-align: center">
                                                        <table id="Table1" cellpadding="0" cellspacing="1" style="width: 100%">
                                                            <tr style="height: 19px">
                                                                <td class="cell_caption" style="width: 20%; text-align: right; height: 18px;">
                                                                    Reference Number :</td>
                                                                <td class="cell_desc" style="width: 30%; text-align: left">                                                                    
                                                                        <input id="txt_reference_no" type="text" /></td>
                                                            </tr>
                                                            <tr style="height: 19px">
                                                                <td class="cell_caption" style="font-weight: bold; width: 20%; text-align: right; height: 6px;">
                                                                    Remitting Branch :</td>
                                                                <td class="cell_desc" style="font-weight: bold; width: 30%; height: 6px; text-align: left">
                                                                    
                                                                        <select id="ddl_branches">
                                                                            <option selected="selected"></option>
                                                                        </select>
                                                                   </td>
                                                            </tr>
                                                            <tr style="height: 19px">
                                                                <td class="cell_caption" rowspan="1" style="width: 20%; height: 18px; text-align: right">
                                                                    &nbsp;Date From (mm/dd/yyyy)&nbsp;:</td>
                                                                <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left">
                                                                    
                                                                        <input id="txt_date_from" type="text" />
                                                                    <img align="absMiddle" onclick="javascript:rem_ovr_apr.open_calendar(this,'txt_date_from');" src="../images/calendar/imgcalen.png" /></td>
                                                            </tr>
                                                            <tr style="height: 19px">
                                                                <td class="cell_caption" rowspan="1" style="width: 20%; height: 18px; text-align: right">
                                                                    Date To (mm/dd/yyyy) :</td>
                                                                <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left">                                                                    
                                                                        <input id="txt_date_to" type="text" />
                                                                    <img align="absMiddle" onclick="javascript:rem_ovr_apr.open_calendar(this,'txt_date_to');" src="../images/calendar/imgcalen.png" /></td>
                                                            </tr>
                                                        </table>
                                                        <input id="btn_search_list" type="button" value="SEARCH" onclick = "javascript:rem_ovr_apr.search_pending_list();"/>
                                                        <input id="btn_clear_search" type="button" value="CLEAR" onclick = "javascript:rem_ovr_apr.pending_list_reload();"/></td>
                                                </tr>
                                            </table>
                                    </center>
                                    <center>
                                        &nbsp;</center>
                                    <center>
                                        <table id="Table5" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                            <tr>
                                                <td class="table_caption" colspan="2" style="text-align: center">
                                                    <label id="Label5">
                                                        LIST OF CREATED CIF QUEUED FOR OVERRIDE - SELECT TO APPROVE OR REJECT</label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center" class="cell_desc">
                                                    <div id="div_enrollment_pending_ovr" onmouseover="javascript:rem_ovr_apr.set_grd_flag_3();"><img src="../images/ajaxloader.gif" />&nbsp;</div>
                                                    <table id="Table6" cellpadding="0" cellspacing="1" style="width: 100%">
                                                            <tr>
                                                                <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">Last Name :</td>
                                                                <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;"><label id="lbl_hit_last_name"></label></td>                                                            
                                                                <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">Client ID :</td>
                                                                <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;"><label id="lbl_hit_client_id"></label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">First Name :</td>
                                                                <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id="lbl_hit_first_name"></label></td>                                                            
                                                                <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Date Created :</td>
                                                                <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id="lbl_hit_date_created"></label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Middle Name :</td>
                                                                <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id="lbl_hit_middle_name"></label></td>
                                                                <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">CIF Classification :</td>
                                                                <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id="lbl_hit_cif_class"></label></td>
                                                            </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Birth Date :</td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id="lbl_hit_birthdate"></td>
                                                            <td class="cell_desc" style="width: 20%; height: 19px; text-align: right"></td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Address :</td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_hit_address"></label></td>
                                                            <td class="cell_desc" style="width: 20%; height: 19px; text-align: right"></td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Country :</td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_hit_country"></label></td>
                                                            <td class="cell_desc" style="width: 20%; height: 19px; text-align: right"></td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Zip Code :</td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_hit_zipcode"></label></td>                                                        
                                                            <td class="cell_desc" style="width: 20%; height: 19px; text-align: right"></td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"></td>
                                                        </tr>
                                                        </table>
                                                    <br />
                                                    <input id="btn_hit_approve" type="button" value="APPROVE" onclick = "javascript: rem_ovr_apr.validate_Pending_override_appr_cif();"/>
                                                    <input id="btn_hit_reject" type="button" value="REJECT" onclick = "javascript: rem_ovr_apr.validate_Pending_override_reject_cif();"/>
                                                    <input id="btn_hit_view" type="button" value="VIEW HIT LIST" onclick = "javascript:rem_ovr_apr.hit_mod_source_get();"/></td>
                                            </tr>
                                        </table>
                                    </center>
                                    <center>
                                            <br />
                                    
                                    
                                        <TABLE style="BORDER-RIGHT: #336699 1px solid; BORDER-TOP: #336699 1px solid; BORDER-LEFT: #336699 1px solid; WIDTH: 100%; BORDER-BOTTOM: #336699 1px solid" id="tbl_pending_override_list" cellSpacing=1 cellPadding=0>
                                            <TBODY><TR><TD style="TEXT-ALIGN: center" class="table_caption" colSpan=2>
                                                <LABEL id="Label4">LIST OF TRANSACTION - SELECT TO OVERRIDE</LABEL></TD></TR><TR><TD 
                                            style="TEXT-ALIGN: center" colSpan=2>
                                                <TABLE style="WIDTH: 100%" id="tbl_pending_list" cellSpacing=1 cellPadding=0>
                                                <TBODY>
                                                    <TR style="HEIGHT: 19px">
                                                        <td class="cell_desc" colspan="4" style="height: 19px; text-align: center">
                                                            <div id = "div_ovrpending_list" onmouseover="javascript:rem_ovr_apr.set_grd_flag_1();"><img src="../images/ajaxloader.gif" /></div>
                                                        </td>
                                                   </TR>
                                                </TBODY>
                                            </TABLE>                                                                                            
                  </TD></TR>
                                            </TBODY>
                                        </TABLE>
                                        <br />
                                            <table id="tbl_pending_override_details" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                            <tr>
                                                <td class="table_caption" colspan="2" style="text-align: center">
                                                    <label id="lbl_pending_ovr_caption">
                                                        TRANSACTION DETAILS</label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center">
                                                    <table id="Table2" cellpadding="0" cellspacing="1" style="width: 100%">
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">
                                                                Reference Number :</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id = "lbl_reference_no"></label>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">
                                                                Transaction Date :</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id="lbl_transaction_date">
                                                                    </label></td>
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" style="font-weight: bold; width: 20%; text-align: right; height: 19px;">
                                                                Net Remitting Amount :</td>
                                                            <td class="cell_desc" style="font-weight: bold; width: 30%; height: 19px; text-align: left">
                                                                <label id = "lbl_net_amt"></label>
                                                                </td>
                                                            <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">
                                                                Transaction Status :</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id="lbl_transaction_status">
                                                                    </label></td>
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" rowspan="1" style="width: 20%; height: 19px; text-align: right">
                                                                Gross Remitting Amount :</td>
                                                            <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id = "lbl_gross_amt"></label></td>
                                                            <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">
                                                                Product Type :</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id="lbl_product_code"></label></td>
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" rowspan="1" style="width: 20%; height: 19px; text-align: right">
                                                                Amount To Be Received :</td>
                                                            <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id = "lbl_net_ben_amt"></label></td>
                                                            <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">
                                                                Implementation Type :</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id="lbl_implementation_type"></label></td>
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" rowspan="1" style="width: 20%; height: 19px; text-align: right">
                                                                Peso Equivalent :</td>
                                                            <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id = "lbl_net_ben_php"></label></td>
                                                            <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">
                                                                Computation Type :</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id="lbl_computation_level"></label></td>                                                                    
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" rowspan="1" style="width: 20%; text-align: right; height: 19px;">
                                                                Transaction Charge :</td>
                                                            <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id ="lbl_total_charge"></label></td>
                                                            <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">
                                                                Purpose &nbsp;:</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;">
                                                                <label id = "lbl_trans_purpose"></label> </td>
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" rowspan="1" style="width: 20%; height: 19px; text-align: right">
                                                                Account Number :</td>
                                                            <td class="cell_desc" rowspan="1" style="width: 30%; height: 19px; text-align: left">
                                                                <label id = "lbl_account_no"></label></td>
                                                            <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Source of Fund :</td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id="lbl_source_of_fund"></label></td>
                                                            
                                                                
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" rowspan="3" style="width: 20%; text-align: right">
                                                                Message &nbsp;:</td>
                                                            <td class="cell_desc" rowspan="3" style="width: 30%; text-align: left"><label id="lbl_special_message"></label>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; text-align: right">Payment Method :</td>
                                                            <td class="cell_desc" style="width: 30%; text-align: left"><label id="lbl_pay_mode"></label></td>
                                                                
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Transaction Source :</td>                                                                
                                                            <td class="cell_desc" style="width: 30%; text-align: left"><label id="lbl_trans_source"></label></td>
                                                                    
                                                        </tr>
                                                        <tr style="height: 19px">
                                                            <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">Nature of Override :</td>
                                                            <td class="cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lbl_nature_of_override"></label> </td>
                                                                
                                                                    
                                                        </tr>
                                                    </table>
                                                    </td>
                                            </tr>
                                        </table>
                                        <br />
                                            <table id="tbl_pending_client_info" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                        <tr>
                                            <td class="table_caption" colspan="2" style="text-align: center">
                                                <label id="Label1">
                                                    CLIENT INFORMATION</label></td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="text-align: center">
                                                <table id="Table3" cellpadding="0" cellspacing="1" style="width: 100%">
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Remitter CIFID :</td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id="lbl_rem_cifid_id"></label></td>
                                                        <td class="cell_caption" style="width: 20%; text-align: right">Beneficiary CIFID :</td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id="lbl_ben_cifid_id"></label></td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" style="width: 20%; text-align: right; height: 18px;">Remitter Name :</td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id="lbl_rem_fullname"></label></td>
                                                        <td class="cell_caption" style="width: 20%; text-align: right">Beneficiary Name :</td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id="lbl_ben_fullname"></label></td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" rowspan="1" style="width: 20%; height: 19px; text-align: right">Birth Date :</td>                                                            
                                                        <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left; height: 19px;"><label id = "lbl_remitter_birthdate"></label></td>                                                            
                                                        <td class="cell_caption" style="width: 20%; text-align: right; height: 19px;">Birth Date :</td>                                                            
                                                        <td class="cell_desc" style="width: 30%; text-align: left; height: 19px;"><label id = "lbl_beneficiary_birthdate"></label></td>                                                            
                                                    </tr>                                                    
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" style="font-weight: bold; width: 20%; text-align: right; height: 6px;">Address :</td>
                                                        <td class="cell_desc" style="font-weight: bold; width: 30%; height: 6px; text-align: left"><label id = "lbl_rem_address"></label></td>
                                                        <td class="cell_caption" style="width: 20%; text-align: right; height: 6px;">Address :</td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left; height: 6px;"><label id="lbl_ben_address"></label></td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" rowspan="1" style="width: 20%; height: 18px; text-align: right">Country :</td>
                                                        <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left"><label id = "lbl_rem_country"></label></td>
                                                        <td class="cell_desc" style="width: 20%; text-align: right"></td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left"></td>                                                            
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" rowspan="1" style="width: 20%; height: 18px; text-align: right">Zip Code :</td>
                                                        <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left"><label id ="lbl_rem_zipcode"></label></td>
                                                        <td class="cell_caption" style="width: 20%; text-align: right">Zip Code :</td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_ben_zipcode"></label></td>                                                            
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" rowspan="1" style="width: 20%; height: 18px; text-align: right">Contact Nos. &nbsp;:</td>
                                                        <td class="cell_desc" rowspan="1" style="width: 30%; text-align: left"><label id = "lbl_rem_contact"></label></td>
                                                        <td class="cell_caption" style="width: 20%; text-align: right">Contact Nos. &nbsp;:</td>
                                                        <td class="cell_desc" style="width: 30%; text-align: left"><label id = "lbl_ben_contact"></label></td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Tellers Reason For Discounting (Individual) :</td>
                                                        <td class="cell_desc" style="text-align: left" colspan="4">                                                                    
                                                                <select id="ddl_reason_discounting" style="width: 95%" disabled="disabled" onchange="javascript:rem_ovr_apr.reason_on_change();">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 19px; display:none">
                                                        <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Reason Other Details :</td>
                                                        <td class="cell_desc" style="text-align: left" colspan="4">                                                                    
                                                                        <input id="txt_reason_discounting" type="text"  style="width:95%" disabled="disabled" /></td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Tellers Reason For Discounting (Business) :</td>
                                                        <td class="cell_desc" style="text-align: left" colspan="4">                                                                    
                                                                <select id="ddl_reason_discounting_bus" style="width: 95%" disabled="disabled" onchange="javascript:rem_ovr_apr.reason_on_change();">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 19px">
                                                        <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Approvers Disposition :</td>
                                                        <td class="cell_desc" style="text-align: left" colspan="4">                                                                    
                                                                <select id="ddl_approver_disposition" style="width: 95%" onchange="javascript:rem_ovr_apr.disposition_on_change();">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 19px;">
                                                        <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Approvers Remarks :</td>
                                                        <td class="cell_desc" style="text-align: left" colspan="4">                                                                    
                                                                        <input id="txt_approver_remarks" type="text"  style="width:95%"/></td>
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
<%--                            <td><table id="Table4" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                <tr>
                                    <td class="table_caption" colspan="4" style="text-align: center">
                                        <label id="Label3">
                                            CLIENT HIT LIST&nbsp; ( TRANSACTION )</label></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="text-align: center" class="cell_desc"><div id="pending_ovr_hitlist" onmouseover="javascript:rem_ovr_apr.set_grd_flag_2();">NO RECORD FOUND.</div></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Search details via Record No. :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtrecordno" style = "width: 98%" onkeypress="javascript:return(generic.isNumberKey(this,",",".",event));" onkeyup="javascript:rem_ovr_apr.display_watchlist_record(event);"/></td>
                                    <td style="text-align: left" class="comment">Press enter</td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Full Name :</td>
                                    <td colspan="3" class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtFullName" style = "width: 100%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Last Name :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtLastName" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Alias Last Name :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtAliasLName" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">First Name :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtFirstName" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Alias First Name :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtAliasFName" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Midde Initial :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtMiddleInitial" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Alias Middle :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtAliasMName" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Suffix :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtSuffix" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Alias Suffix :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtAliasSuffix" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Title :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtTitle" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Nationality :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtNationality" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Date of Birth :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtDateofBirth" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Town of Birth :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtTownofBirth" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Age :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtAge" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Country of Birth :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtCountryofBirth" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Position :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtPosition" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Business Name :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtBusinessName" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Days Listed :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtDays" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Listed On :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtListedOn" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Last Updated :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtLastUpdated" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">City :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtCity" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">State :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtState" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Country :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtCountry" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Zip Code :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtZipCode" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Phone Number :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtPhoneNumber" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Passport Details :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtPassport" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">DBA Name :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtDbaName" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Group Type :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtGroupType" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Alias Type :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtAliasType" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">Regime :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtRegime" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Group ID :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtGroupId" style = "width: 98%" readonly="readonly"/></td>
                                    <td class="cell_caption" style="width: 20%; text-align: right">File Type :</td>
                                    <td class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtRegulatory" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 18px; text-align: right">Remarks :</td>
                                    <td colspan="3" class="cell_desc" style="width: 30%; text-align: left"><input type = "text" id = "txtRemarks" style = "width: 98%" readonly="readonly"/></td>
                                </tr>
                            </table>
                            </td>--%>
                        </tr>
                        <tr><td style="vertical-align: top; height: 22px;">
                            <input id="btn_ovr_approved" type="button" value="APPROVE" onclick = "javascript: rem_ovr_apr.validate_Pending_override_appr();"/>&nbsp
                            <input id="btn_ovr_cancelled" type="button" value="REJECT" onclick = "    javascript: rem_ovr_apr.validate_Pending_override_reject();"/>&nbsp
                            <input id="btn_hit_view_tranx" type="button" value="VIEW HIT LIST" onclick = "javascript:rem_ovr_apr.hit_mod_source_get_transaction();"/>
                            </td></tr>
                        <tr><td style="vertical-align: top; text-align: center">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
            </center>   
            <!-- The Modal -->
            <div class="modal" id="remModal" data-backdrop="false" >
              <div class="modal-dialog modal-dialog-centered modal-lg">
                <div class="modal-content">
                    <!-- Modal Header -->
                    <div class="modal-header">
                        <img align="center" src="../images/rfv_lg_pb_hz_rgb_pos_World-Check.png" width="200px" height="50px"/>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                  <div class="modal-body">
                    <div>
                        <input type="label" class="form-control" id="txtwithHIT" readonly="readonly" style="background-color:white; color: red; border:hidden;"/>&nbsp
                        <table id="tblHitList" class="table table-striped table-bordered" style="width:100%">
                            <thead>
                                <tr>
                                    <th class="text-center">Match Score</th>
                                    <th class="text-center">Matched Name Type</th>
                                    <th class="text-center">Primary Name</th>
                                    <th class="text-center">Category</th>
                                    <th class="text-center">Date Of Birth</th>
                                    <th class="text-center">Nationality</th>
                                    <th class="text-center">Special Interest Category</th>
                                    <th class="text-center">Provider Type</th>
                                    <th class="text-center">Alias</th>
                                    <th class="text-center">Remarks</th>
                                    <th class="text-center">Resolution</th>
                                    <th class="text-center">Resolution Remarks</th>
                                    <th class="text-center">Created Date</th>
                                    <th class="text-center">Updated Date</th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                  </div>
                </div>
              </div>
            </div>      
             <!--MessageBox Section-->
            <!--MessageBox Section-->
            <div id="divMessageBox">
                <label id="lblMessagebox"></label>
                <span id="spanMessagebox"></span>
            </div>         
        
        </div>
    </form>
</body>
</html>
