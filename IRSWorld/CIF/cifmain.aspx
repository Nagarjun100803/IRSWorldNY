<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cifmain.aspx.cs" Inherits="IRSWorld.CIF.cifmain" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRS CUSTOMER INFO.</title>
    <link href="../css/Default_min.css" rel="stylesheet"        type="text/css" />
    <script src="../js/jsCommons_min.js"                        type="text/javascript"></script>
    <!--script src="../js/jsCommons.js"                        type="text/javascript"></script-->
    <script src="../js/jscommonmethod_min.js"                   type="text/javascript"></script>
    <!--script src="../js/jscommonmethod.js"                   type="text/javascript"></script-->
    <!--script src="../js/js_generic.js"                       type="text/javascript"></script-->
    <script src="../js/js_generic_min.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js"                   type="text/javascript"></script>
    <script src="../js/js_sidebar_min.js"                       type="text/javascript"></script>
    <script src="../js/CIF/js_cif_main.js"                  type="text/javascript"></script>
    <script src="../js/CIF/js_cif_master.js"                type="text/javascript"></script>
    <script src="../js/CIF/js_cif_ids.js"                   type="text/javascript"></script>
    <script src="../js/CIF/js_cif_accounts.js"              type="text/javascript"></script>
    <script src="../js/CIF/js_cif_address.js"               type="text/javascript"></script>
    <script src="../js/CIF/js_cif_beneficiaries.js"         type="text/javascript"></script>
    <script src="../js/CIF/js_cif_cards.js"                 type="text/javascript"></script>
    <script src="../js/CIF/js_cif_contacts.js"              type="text/javascript"></script>
    <script src="../js/CIF/js_cif_merchants.js"             type="text/javascript"></script>
    <script src="../js/CIF/js_cif_occupations.js"           type="text/javascript"></script>
    <script src="../js/CIF/js_cif_remitters.js"             type="text/javascript"></script>
    <script src="../js/CIF/js_cif_classifications.js"       type="text/javascript"></script>
    <script src="../js/CIF/js_cif_merchant.js"              type="text/javascript"></script>    
    <script src="../js/CIF/js_cif_spouse.js"                type="text/javascript"></script>
    <script src="../js/CIF/js_cif_other_ids.js"             type="text/javascript"></script>
    <script src="../js/CIF/js_cif_van.js"                   type="text/javascript"></script>
    <script src="../js/CIF/js_cif_watchlist.js"             type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemitter_min.js"      type="text/javascript"></script>
    <!--script src="../js/Internet/jsInternetRemitter.js"      type="text/javascript"></script-->
    <script src="../js/Internet/jsInternetRemitterCards_min.js" type="text/javascript"></script>
    <script src="../js/Admin/Login_min.js"                      type="text/javascript"></script>
    <script src="../js/CardRem/jsGFMCMaintenance_min.js" type="text/javascript"></script> 
    <!--START:RFC-35248-->
    <script src="../js/Remittance/jsRemExternals_min.js" type = "text/javascript"></script>
    <!--END-->
    
    <!--<script src="../js/CIF/js_cif_viewdetails.js"           type="text/javascript"></script>-->
    <script src="../js/popcalendar.js" type="text/javascript"></script> 
    <script type="text/javascript">    
        var cif_main  = new cif_main();
        var generic = new generic_method();
       
        function gfc_response()
        {
            cif_main.gfc_execute_response();
        } 
    </script>
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
<body onload="javascript:set_focus();load_main();cif_main.load_all_dropdownlist();cif_main.get_clientdata();construct_side_bar('CIFMENU');"  onunload="javascript:close_session();" onkeyup="javascript:cifbtns_click(event);" onresize="fix_dim();">
    <center>
        <form id="form1" runat="server">
            <table id="CIF"  cellpadding="0" cellspacing="0" style="width:100%;height:100%;"><!--cellspacing="30" -->
                <tr>
                    <td id=tdCIF colspan="2" align="center" style="width:100%;height:100%; border-collapse:collapse; vertical-align:middle" valign="middle">
                        <table style="width:100%"  cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="2"; align=center>
                                    <div id = "CIFSEARCHMAIN" style= "display:block; position:relative; margin-top:30px;" ><!--position:relative; top:80px;-->
                                        <table id = "tblCIFSEARCHMAIN" style="width:43%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <table style="width:100%" cellpadding="0" cellspacing="0" >
                                                        <tr>
                                                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                                <img src="../images/toe.GIF" /></td>    
                                                            <td class="table_head_dark" style="width: 50%; height: 24px">
                                                                Client Information</td>
                                                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                                <img align="absBottom" src="../images/rend.gif" /></td> 
                                                        </tr>  
                                                    </table>    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="divMainSearch" style="width: 100%;display: block; vertical-align:middle">
                                                        <table style="width:100%; border:solid 3px #336699; background:azure;">
                                                            <tr>
                                                                <td>
                                                                    <table id="tblMainSearch" cellpadding="0" cellspacing="2" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                        <tr><td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: White" colspan="2">
                                                                            Search or Add New</td></tr>                                                                            
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                Class:</td>
                                                                            <td align="left" style="width: 60%; height: 23px">                                                                                   
                                                                                <select id="ddSRole" size="1" style="width: 98%; text-align: left;" onchange="javascript:cif_main.get_set_cif_type();">
                                                                                </select>                                                                    
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px">Type:</td>
                                                                            <td align="left" style="width: 60%; height:23px">                                                                                   
                                                                              <select id="ddSType" size="1" onchange="javascript:cif_main.change_search_label();" style="width: 98%; text-align: left;">
                                                                              </select> 
                                                                            </td>
                                                                        </tr>                                                              
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                <label id="lblSearchLNorBN">Last Name:</label></td>
                                                                            <td align="left" style="width: 60%">
                                                                                <input id="txtSearchLastName" type="text" style="width: 96%; height: 100%; text-align: left;" onfocus="get_focus(this.id);"/></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                <label id="lblSearchFNorSN">First Name:</label></td>
                                                                            <td align="left" style="width: 60%">
                                                                                <input id="txtSearchFirstName" type="text" style="width: 96%; height: 100%; text-align: left;" onfocus="get_focus(this.id);" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                <label id="lblSearchMNorNB">Middle Name:</label></td>
                                                                            <td align="left" style="width: 60%">
                                                                                <input id="txtSearchMiddleName" type="text" style="width: 96%; height: 100%; text-align: left;" onfocus="get_focus(this.id);" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                <label id="lblSearchCIFID">CIF ID:</label></td>
                                                                            <td align="left" style="width: 60%">
                                                                                <input id="txtSearchCIFID" type="text" style="width: 96%; height: 100%; text-align: left;" onfocus="get_focus(this.id);" maxlength="40"/></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px">
                                                                                <label id="lblSearchOldCIFID">Old CIF ID:</label></td>
                                                                            <td align="left" style="width: 60%">
                                                                                <input id="txtSearchOLDCIFID" type="text" style="width: 96%; height: 100%; text-align: left;" onfocus="get_focus(this.id);"/></td>
                                                                        </tr>
                                                                        <!--
                                                                        <tr id="tr1">
                                                                            <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                                                <a id="A1" href="javascript:show_frame_dialog('../CIF/cifmain.aspx?function=addrem', 'cifclose', '100%', '100%', '0');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">test OTC</a>
                                                                            </td>
                                                                        </tr>
                                                                        -->
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table id="tblButtonsSearchCIF" style="width:85%" cellpadding="0" cellspacing="5px">
                                                                        <tr>
                                                                            <td align="center" colspan="3" style="width: 100%; height: 50px;">
                                                                                <div id="_divButtonsSearchCIF" style="width: 65%; background: transparent">
                                                                                    <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                                        <tr>
                                                                                            <td style="width: 100px" id="tdBtnSearchCIF"><input id="btnSearchCIF" onclick="javascript:cif_main.check_session();cif_main.panel_main_search();"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 90%" /></td>
                                                                                            <td style="width: 100px" id="tdBtnAddCIF"><input id="btnAddCIF"    onclick="javascript:cif_main.check_session();cif_main.panel_main_insert();"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add"    style="width: 90%"/></td>
                                                                                            <td style="width: 100px; display:none" id="tdBtnCancelAddUser"><input id="btnCancelAddUser"    onclick="cancel_adduser()"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Cancel"    style="width: 90%"/></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                    <input id="txtHiddenSeqId"          type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenCIFID"          type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenMasterSeqId"    type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenRemitterSeqId"  type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenCIFType"        type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenCIFClass"       type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenBeneCIFID"      type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenMerchantCIFID"  type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenErrorMsg"       type="hidden" style="width: 5%" />   
                                                                                    <input id="txtHiddenAccountSeqId"   type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenMerchantInfoSeqId"   type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenZipSeqId"       type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenBankSeqId"      type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenBranchSeqId"    type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenParameter"      type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenParamCIFID"     type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenUserRole"       type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenUserCIFID"      type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenUserID"         type="hidden" style="width: 5%" />    
                                                                                    <input id="txtHiddenUserLocation"   type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenActivePanel"    type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenAcctBankSeqId"  type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenAcctForBankSeqId"    type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenAcctDomBankSeqId"    type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenAcctPNBBranchSeqId"  type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenAcctBankFlag"        type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenCIFBrCode"           type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenCIFBrName"           type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenMerCode"             type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenMerFieldLabel"       type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenChargeTypeFldVal"    type="hidden" style="width: 5%" />  
                                                                                    <input id="txtHiddenChargeTypeFldDesc"   type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenDummyClass"          type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenSearchFlag"          type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenSearchZipPostFlag"   type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenPostalCode"          type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenPostalSeqId"         type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenKBIAccount"          type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenMT103BankSeqId"      type="hidden" style="width: 5%" /> 
                                                                                    <input id="txtHiddenISMT103"             type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenBranchType"          type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenGFMCNo"              type="text"   style="text-decoration: none; color: white; cursor: text; margin: 0px; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none; background-color: transparent; width:0px; height:0px; left: -100px; position: absolute; top: -100px;"/>
                                                                                    <input id="txtHiddenVanProductSeqID"     type="hidden" style="width: 5%" />
                                                                                    <input id="txtHiddenVanBeneCIFID"        type="hidden" style="width: 5%" />
                                                                                </div>
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
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top; width: 15%; padding-top:30px; height: 605px;" align=right>
                                    <div id = "CIFMENU" style= "display:none; " >
                                        <div id="divtdInternetRemitterMenu" style="margin-top: 20px; display: block;"></div>
                                    </div>
                                    <div id = "CIFMENU2" style= "display:none; top:100px;" >
                                          <table id = "tblCIFMENU" style="width:100%" cellpadding="0" cellspacing="5">
                                            <tr id="trMaster">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Master" href="javascript:cif_main.panel_to_display();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Personal Info</a>
                                                </td>
                                            </tr>
                                            <tr id="trCIFClass">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFClass" href="javascript:cif_main.panel_cif_classifications();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">CIF Class</a>
                                                </td>
                                            </tr>
                                            <tr id="trAddress">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Address" href="javascript:cif_main.panel_address();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Address</a>
                                                </td>
                                            </tr>                                    
                                            <tr id="trOccupation">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Occupation" href="javascript:cif_main.panel_occupation();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Occupations</a>
                                                </td>
                                            </tr>           
                                            <tr id="trAccounts">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Accounts" href="javascript:cif_main.panel_accounts();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Accounts</a>
                                                </td>
                                            </tr>                                                                                                                                       
                                            <tr id="trCards">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Cards" href="javascript:cif_main.panel_cards();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Cards</a>
                                                </td>
                                            </tr>                                                                                                                                       
                                            <tr id="trIds">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Ids" href="javascript:cif_main.panel_ids();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Ids</a>
                                                </td>
                                            </tr>                                                                                                                                       
                                            <tr id="trContacts">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Contacts" href="javascript:cif_main.panel_contacts();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Contacts</a>
                                                </td>
                                            </tr>                                                                                                                                       
                                            <tr id="trMerchantOtherInfo">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1; height: 20px;">
                                                    <a id="MerchantOtherInfo" href="javascript:cif_main.panel_merchant_info();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Merchant Info</a>
                                                </td>
                                            </tr>    
                                            <tr id="trRemitterInfo">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="RemitterInfo" href="javascript:cif_main.panel_remitters_info();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Remitter Info</a>
                                                </td>
                                            </tr>                                                                                                                                   
                                            <tr id="trBeneficiaries">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Beneficiaries" href="javascript:cif_main.panel_attach_beneficiary();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Beneficiaries</a>
                                                </td>
                                            </tr>
                                            <tr id="trMerchants">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Merchants" href="javascript:cif_main.panel_attach_merchant();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Merchants</a>
                                                </td>
                                            </tr>       
                                            <tr id="trSpouse">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Spouse" href="javascript:cif_main.panel_spouse();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Spouse</a>
                                                </td>
                                            </tr>     
                                            <tr id="trCIFOtherIDs">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="OtherIDs" href="javascript:cif_main.panel_other_ids();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Other CIF IDs</a>
                                                </td>
                                            </tr>
                                            <tr id="trCIFVan">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Van" href="javascript:cif_main.panel_van();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Virtual Acct. No.</a>
                                                </td>
                                            </tr>                                               
                                            <tr id="trWatchlist">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="Watchlist" href="javascript:cif_main.panel_watchlist();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Watchlist</a>
                                                </td>
                                            </tr>
                                            <tr id="trViewDetails">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="ViewDetails" href="javascript:cif_main.cif_view_details();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">View Details</a>
                                                </td>
                                            </tr>  
                                            <tr id="trCIFGefu" style="display:none">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFGefu" href="javascript:cif_main.cif_gefu();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">CIF Gefu</a>
                                                </td>
                                            </tr>                                                                                                           
                                            <tr id="trCIFSearch" >
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFSearch" href="javascript:cif_main.panel_cif_main_search();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Search</a>
                                                </td>
                                            </tr>  
                                            <!--                                          
                                            <tr id="trOTCAddBene">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFOTCAddBene" href="javascript:add_rem_or_bene(2);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Add Beneficiary</a>
                                                </td>
                                            </tr>-->
                                            <tr id="trBackToOTC">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFOTC" href="javascript:cif_main.panel_otc_back();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Back to OTC</a>
                                                </td>
                                            </tr>                                            
                                            <tr id="trBackToOTC2">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFOTC2" href="javascript:cif_main.panel_otc_back2();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Back to OTC</a>
                                                </td>
                                            </tr>
                                            <tr id="trCIFClose">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFClose" href="javascript:cif_main.panel_cif_close();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Close</a>
                                                </td>
                                            </tr>   
                                            <tr id="trCIFBack">
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a id="CIFBack" href="javascript:cif_main.back_to_page();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Back</a>
                                                </td>
                                            </tr>                                                                                                                               
                                        </table>
                                    </div>
                                 </td>
                                <td style=" vertical-align:top; width: 83%; padding-top:30px; height: 605px;" align=center>
                                    <div id = "CIFENTRY" style= "display:none; height:410px; overflow:auto;"><!--height:410px-->
                                            <table style="width:90%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <table style="width:100%" cellpadding="0" cellspacing="0" >
                                                            <tr>
                                                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                                    <img src="../images/toe.GIF" /></td>    
                                                                <td class="table_head_dark" style="width: 50%; height: 24px">Client Maintenance</td>
                                                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                                    <img align="absBottom" src="../images/rend.gif" /></td> 
                                                             </tr>                                                                              
                                                        </table>    
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="background:azure;">
                                                        <table id="tblCIF" style="width:100%; border:solid 3px #336699;">
                                                            <tr>
                                                                <td style="width: 100%;">
                                                                    <div id="CIFINDIVIDUAL" style="display:none;overflow:auto;">
                                                                        <table id="tblCIFIndividual" cellpadding="0" cellspacing="5" style="width:100%;border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" colspan="6" style="color: black; text-indent: 5px; text-align: left">
                                                                                    Individual
                                                                                <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 25px" >Last Name:&nbsp;</td>
                                                                                <td colspan="5" align="left" style="height: 25px; color:red">                                                                                
                                                                                    <input id="txtLastname" type="text" maxlength="200" style="text-align: left; width: 85%;"/><label id="astLastName"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 25px" >First Name:&nbsp;</td>
                                                                                <td colspan="5" align="left" style="color: red; height: 25px;">
                                                                                    <input id="txtFirstname" type="text" maxlength="50" style="text-align: left; width: 85%;" /><label id="astFirstName"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 25px" >Middle Name:&nbsp;</td>
                                                                                <td colspan="5" align="left" style="color: red; height: 25px;">
                                                                                    <input id="txtMiddlename" type="text" maxlength="50" style="text-align: left; width: 85%;" /><label id="astMiddleName"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 25px" >Alias:&nbsp;</td>
                                                                                <td align="left" colspan="5" style="color: red; height: 25px;">
                                                                                    <input id="txtIndAlias" type="text" maxlength="200" style="text-align: left; width: 85%;" /><label id="astIndAlias"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 24px" >Title:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color: red; width: 20%;" >
                                                                                    <select id="ddTitle" style="width: 94%">
                                                                                        <option selected="selected"></option>
                                                                                    </select><label id="astTitle"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 13%; height: 24px" >Birthplace:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color: red;" colspan="2">
                                                                                    <input id="txtBirthplace" type="text"  maxlength="200" style="width: 65%" /><label id="astBirthPlace"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 24px" >Birth Date: MM/DD/YYYY&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color:Red; width: 20%;">
                                                                                    <input id="txtBirthday" style="width: 84%" type="text" maxlength="10" onchange="javascript:cif_main.compute_age();"/>
                                                                                    <img id="calBirthday" align="absMiddle" onclick="javascript:if(document.getElementById('txtBirthday').disabled == false){popUpCalendar(this, document.getElementById('txtBirthday'), 'mm/dd/yyyy');}"
                                                                                        src="../images/calendar/imgcalen.png" />
                                                                                    <label id="astBirthDate"></label>
                                                                                    <!--<img alt="popupcalendar" onclick="popUpCalendar(this, document.forms[0].txtBirthday, 'mm/yyyy');" src="../images/calendar/imgcalen.png" />-->
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 13%; height: 24px" >Age:&nbsp;</td>
                                                                                <td runat="server" align="left" colspan="2" style="height: 24px; color:Red;">
                                                                                    <input id="txtAge" type="text" style="width: 15%" maxlength="4"  onchange="javascript:cif_main.compute_age();"/><label id="astAge"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 24px" >Citizenship:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color:Red; width: 20%;">
                                                                                    <select id="ddCitizenship" style="width: 94%">
                                                                                        <option selected="selected"></option>
                                                                                    </select><label id="astCitizenship"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 13%; height: 24px" >Civil Status:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color: red;">
                                                                                    <select id="ddCivilStatus" style="width: 67%">
                                                                                        <option selected="selected"></option>
                                                                                    </select><label id="astCivilStatus"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 24px" >Religion:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color: Red; width: 20%;">
                                                                                    <select id="ddReligion" style="width: 94%">
                                                                                        <option selected="selected"></option>
                                                                                    </select><label id="astReligion"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 13%; height: 24px" >Gender:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color:Red;">
                                                                                    <select id="ddGender" style="width: 67%">
                                                                                        <option selected="selected"></option>
                                                                                    </select><label id="astGender"></label></td>
                                                                            </tr>                                                                            
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 24px">
                                                                                    Branch:&nbsp;</td>
                                                                                <td align="left" colspan="3" style="height: 24px">
                                                                                    <input id="txtMasterIndBranchCode" style="width: 97%" type="text"  maxlength="10" onchange="javascript:cif_main.compute_age();"/></td>
                                                                                <td id="tdBranchCode" align="left" style="width: 30%; height: 24px; color:Red">
                                                                                    <input id="btnSearchBranchCode" onclick="javascript:cif_main.panel_search_branchcode();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 29%; height:19px;" /><label id="astBranchCode">*</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" colspan="5" style="color: red;">
                                                                                    <input id="txtIndRemarks" style="width: 85%" type="text"  maxlength="512"/><label id="astIndRemarks"></label></td>
                                                                            </tr>
                                                                            <tr style="display:none">
                                                                                <td class="cell_caption" style="width: 14%; height: 24px" >Watchlist:&nbsp;</td>
                                                                                <td align="left" colspan="5" style="height: 24px; color:Red">
                                                                                    <select id="ddIndWatchlist" style="width: 86%">
                                                                                        <option selected="selected"></option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                
                                                                    <div id = "CIFBUSINESS" style= "display:none;overflow:auto; ">  
                                                                        <table id="tblUserBusiness" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="6">Business</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px" >Business Name:&nbsp;</td>
                                                                                <td align="left" style="width: 88%; height: 24px; color:Red" colspan="5">
                                                                                    <input id="txtBusinessName" type="text" style="width: 85%; text-align: left;" maxlength="200" /><label id="astBusinessName"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px" >
                                                                                    Short Name:&nbsp;</td>
                                                                                <td align="left" style="width: 88%; height: 24px; color:Red" colspan="5">
                                                                                    <input id="txtShortName" type="text" style="width: 85%;  text-align: left;" maxlength="50" /><label id="astShortName"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px" >Alias:&nbsp;</td>
                                                                                <td align="left" style="width: 88%; height: 24px; color:Red" colspan="5">
                                                                                    <input id="txtBusAlias" type="text" style="width: 85%; text-align: left;" maxlength="200" /><label id="astBusAlias"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px" >Type of Business:&nbsp;</td>
                                                                                <td align="left" style="width: 88%; height: 24px; color:Red" colspan="5">
                                                                                    <input id="txtNatureBusiness" type="text" style="width: 85%; text-align: left;" maxlength="50" /><label id="astNatureBusiness"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <!--<td class="cell_caption" style="width: 18%; height: 23px">Status:&nbsp;</td>
                                                                                <td align="left" style="width: 31%; height: 23px">
                                                                                   <select id="ddBusStatus" style="width: 100%; height: 100%"></select>
                                                                                </td>-->
                                                                                <td class="cell_caption" style="width: 15%; height: 24px;" >
                                                                                    Branch:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 23px; width: 50%;">
                                                                                    <input id="txtMasterBusBranchCode" style="width: 96%" type="text"/></td>
                                                                                <td id="tdBusBranchCode1" align="left" style="width: 14%; height: 24px; color:Red">
                                                                                    <input id="btnSearchBusBranchCode" onclick="javascript:cif_main.panel_search_branchcode();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 86%; height:19px;" /><label id="astBusBranchCode">*</label>
                                                                                </td>
                                                                                <td style="width: 25%; height: 23px"></td>
                                                                            </tr>                                                                                           
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" style="width: 88%; height: 24px; color:Red" colspan="5">
                                                                                    <input id="txtBusRemarks" type="text" style="width: 85%; text-align: left;" maxlength="512" /><label id="astBusRemarks"></label></td>
                                                                            </tr>
                                                                            <tr style="display:none"> 
                                                                                <td class="cell_caption" style="width: 18%; height: 23px">Watchlist:&nbsp;</td>
                                                                                <td align="left" style="width: 31%; height: 23px; color:Red" colspan="4">
                                                                                   <select id="ddBusWatchlist" style="width: 86%; height: 100%"></select><label id="Label7"></label>
                                                                                </td> 
                                                                            </tr>
                                                                         </table>
                                                                   </div> 
                                                                
                                                                    <div id = "CIFPERSONALINFOENTRY" style= "display:none;overflow:auto; "> 
                                                                        <table id="tblIndividualHeader" style="width:100%" cellpadding="0">
                                                                            <tr>
                                                                                <td class="cell_desc" style="width: 5%; height: 24px">NAME :</td>
                                                                                <td align="left" style="width: 35%; height: 24px;" colspan="4">
                                                                                    <label id="lblCustLName" class="cell_desc"></label>
                                                                                    <label id="lblCustFName" class="cell_desc"></label>
                                                                                    <label id="lblCustMName" class="cell_desc"></label>
                                                                                </td>
                                                                                <td class="cell_desc" style="width: 5%; height: 24px" >CIF ID :</td>
                                                                                <td align="left" style="width: 30%; height: 24px;" colspan="4">
                                                                                    <label id="lblCIFID" class="cell_desc">
                                                                                        </label></td>
                                                                            </tr>                                                                                                                                                                                   
                                                                         </table>
                                                                      </div> 
                                                                
                                                                    <div id = "CIFBUSINESSINFOENTRY" style= "display:none;overflow:auto; "> 
                                                                        <table id="tblBusinessHeader" style="width:100%" cellpadding="0" cellspacing="5">
                                                                            <tr>
                                                                                <td class="cell_desc" style="width: 14%; height: 24px" >BUSINESS NAME :</td>
                                                                                <td align="left" style="width: 40%; height: 24px;" colspan="4">
                                                                                    <label id="lblBusinessName" class="cell_desc"></label></td>
                                                                                <td class="cell_desc" style="width: 7%; height: 24px" >CIF ID :</td>
                                                                                <td align="left" style="width: 30%; height: 24px;" colspan="4">
                                                                                    <label id="lblBusCIFID" class="cell_desc"></label></td>
                                                                            </tr>
                                                                         </table>
                                                                       </div> 
                                                                
                                                                   <div id = "CIFCLASSIFICATIONSMAIN" style= "display:none;overflow:auto; ">   
                                                                        <table id ="tblCIFCLASSIFICATIONSMAIN" cellpadding="0" cellspacing="5" style="width:100%;border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5"> List of Classifications</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="vertical-align:top; height:100%; width:50%;">
                                                                                    <div id = "CIFCURRENTCLASS" style= "display:none;">   
                                                                                        <table id="tblGridCurrentClass" cellpadding="0" cellspacing="5" style="width:100%;border: solid 1px #336699;">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black;" colspan="5"> Current</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="height: 190px" valign="top"><div id = "CIFGRIDCURRENTCLASS" style= "display:none" onmouseover = "javascript:cif_main.active_cifrole_grid('GRIDCLASSREMOVE');">PLACE GRIDVIEW CURRENT CLASSIFICATIONS</div></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="vertical-align:top; height:100%; width:50%;">
                                                                                    <div id = "CIFGRIDCLASS" style= "display:none">   
                                                                                        <table id="tblGridClass" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699;">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="5"> Available</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="height: 190px" valign="top"><div id = "CIFMASTERGRID" style= "display:none"  onmouseover = "javascript:cif_main.active_cifrole_grid('GRIDCLASSADD');">PLACE GRIDVIEW AVAILABLE CLASSIFICATIONS</div> </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                
                                                                   <div id = "CIFADDRESSMAIN" style="width:100%;display:none;"> 
                                                                        <table id="tblAddress"  cellpadding="0" cellspacing="5" style="width:100%;border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">Address
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 20%; height: 24px" >Room No. / Bldg.:&nbsp;</td>
                                                                                <td align="left" style="width: 80%; height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtAddress1" type="text" style="width: 81%; text-align: left;" maxlength="100" /><label id="astAddress1"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 20%; height: 24px" >PO Box:&nbsp;</td>
                                                                                <td align="left" style="width: 80%; height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtPOBox" type="text" style="width: 81%; text-align: left;" maxlength="100" /><label id="astPOBox"></label></td>
                                                                            </tr>                                                                            
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px" >Street No./ Name:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtAddress2" type="text" style="width: 81%; text-align: left;" maxlength="100" /><label id="astAddress2"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px" >
                                                                                    District:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtDistrict" type="text" style="width: 81%; text-align: left;" maxlength="100" /><label id="astDistrict"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px" >
                                                                                    City:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtCity" type="text" style="width: 81%; text-align: left;" maxlength="100" /><label id="astCity"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px" >
                                                                                    State:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtState" type="text" style="width: 81%; text-align: left;" maxlength="100" /><label id="astState"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px" >
                                                                                    Region:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtRegion" type="text" style="width: 81%; text-align: left;" maxlength="100" /><label id="astRegion"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px" >
                                                                                    Country:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red" colspan="3">
                                                                                    <select id="ddCountryCode" style="width: 81%;"></select>
                                                                                    <label id="astCountryCode"></label></td>
                                                                            </tr> 
                                                                            <%--<tr>
                                                                                <td class="cell_caption" style="height: 24px" >
                                                                                    Address (for 3k Report used):&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red" colspan="3">
                                                                                    <input id="txt3kAddress" type="text" maxlength=100  style="width:81%"/>
                                                                                </td>
                                                                            </tr>      --%>                                                                        
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px">
                                                                                    Zip Code:&nbsp;</td>
                                                                                <td align="left" colspan="3" style="color: red; height: 24px">
                                                                                    <input id="txtMuniProv" type="text" style="width: 81%; text-align: left;" maxlength="100" />
                                                                                    <input id="btnSearchZipCode" onclick="javascript:cif_main.panel_search_zipcode();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 60px; height:19px" /><label id="astZipCode"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="4" style="width:100%;">
                                                                                    <div id="trPostalCode" style="width:100%; display:none">
                                                                                        <table cellpadding="0" cellspacing="5"  style="width:100%; background:azure;" id="tblPostalCode">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 24%; height: 23px">
                                                                                                    Postal Code (State):&nbsp;</td>
                                                                                                <td align="left" colspan="2" style="height: 100%;"><input id="txtPostalCode" type="text" style="width: 99%; text-align: left;" maxlength="100" /></td>
                                                                                                <td align="left" style="width: 12%; color: red"><input id="btnSearchPostalCode" onclick="javascript:cif_main.panel_search_postalcode();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 50px; height:19px" /><label id="Label1"></label></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 15%; height: 23px">Address Type:&nbsp;</td>
                                                                                <td align="left" style="width: 20%; height:23px; color:Red">
                                                                                   <select id="ddAddressType" style="width: 93%;"></select><label id="astAddressType"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">Preferred Mailing Address:&nbsp;</td>
                                                                                <td align="left" style="width: 15%; color:Red">
                                                                                    &nbsp;<input id="chkPrefMailAdd" style="width: 12%" type="checkbox" /><label id="astPrefMailAdd"></label></td>
                                                                            </tr>  
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 23px" >Remarks:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 23px; color:Red" colspan="3">
                                                                                    <input id="txtAddRemarks" type="text" style="width: 81%; text-align: left;" maxlength="255" /><label id="astAddRemarks"></label></td>
                                                                            </tr>                                                                                            
                                                                         </table>
                                                                    </div>                                                                                                                                                             
                                                                    
                                                                   <div id = "CIFOCCUPATION" style= "display:none;overflow:auto; ">   
                                                                        <table id="tblOccupation" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="4">Occupation
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px">Occupation:&nbsp;</td>
                                                                                <td align="left" style="width: 80%; height:23px; color:Red" colspan="3">
                                                                                    <select id="ddOccupation" style="width: 78%;"></select><label id="astOccupation"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 9%; height: 24px">
                                                                                    Detailed Description:&nbsp;</td>
                                                                                <td align="left" style="width: 13%; height: 24px; color:Red"><input id="txtDetailedDesc" type="text" style="width: 90%; text-align: left;" maxlength="20" /><label id="astDetailedDesc"></label></td>
                                                                                <td class="cell_caption" style="width: 7%; height: 24px">
                                                                                    Worker Type:&nbsp;</td>
                                                                                <td align="left" style="width: 20%; height: 24px; color:Red">
                                                                                   <select id="ddWorkerType" style="width: 55%;"></select><label id="astWorkerType"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 9%; height: 24px">Occupation Type:&nbsp;</td>
                                                                                <td align="left" style="width: 13%; height: 24px; color:Red">
                                                                                    <select id="ddOccupationType" style="width: 92%;"></select><label id="astOccupationType"></label></td>

                                                                                <td class="cell_caption" style="width: 7%; height: 24px">Employer:&nbsp;</td>
                                                                                <td align="left" style="width: 20%; height: 24px; color:Red"><input id="txtEmployer" type="text" style="width: 54%; text-align: left;" maxlength="100" /><label id="astEmployer"></label></td>                                                      
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 9%; height: 24px">Position:&nbsp;</td>
                                                                                <td align="left" style="width: 13%; height: 24px; color:Red">
                                                                                    <input id="txtPosition" type="text" style="width: 90%; text-align: left;" maxlength="50" /><label id="astPosition"></label>
                                                                                </td>
                                                                                <td style="width: 7%; height: 23px"></td>
                                                                                <td align="left" style="width: 20%; height:23px">
                                                                                    &nbsp;</td>                                                      
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px">Annual Salary:&nbsp;</td>
                                                                                <td align="left" style="width: 13%; color:Red">
                                                                                    <input id="txtSalary" type="text" value="0.00" style="width: 90%; text-align: left;" onkeypress="javascript:return(generic.autoFormatCurrency(this,',','.',event));"/><label id="astSalary"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 7%; height: 24px">
                                                                                    Currency:&nbsp;</td>
                                                                                <td align="left" style="width: 20%; height: 24px; color:Red">
                                                                                   <select id="ddCurrency" style="width: 55%;"></select><label id="astOccuCurrency"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px">Source of Funds:&nbsp;</td>
                                                                                <td align="left" style="width: 80%; height:23px; color:Red" colspan="3">
                                                                                    <select id="ddSourceOfFunds" style="width: 78%;" onchange="javascript:cif_main.ClearAnnualSalary();"></select><label id="astSourceOfFunds"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 9%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" style="width: 80%; height: 24px; color:Red" colspan="3">
                                                                                    <input id="txtOccuRemarks" type="text" style="width: 77%; text-align: left;" maxlength="50" /><label id="astOccuRemarks"></label></td>
                                                                            </tr>
                                                                         </table>
                                                                    </div>                                                                                
                                                                    
                                                                     <div id = "CIFPNBEMPLOYEE" style= "display:none;overflow:auto; "> 
                                                                        <table id="tblPNBOccupation"  cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">
                                                                                    PNB Employee</td>
                                                                            </tr>
                                                                            <tr style="display:none;">
                                                                                <td class="cell_caption" style="width: 8%; height: 23px">Occupation:&nbsp;</td>
                                                                                <td align="left" style="width: 80%; height:23px" colspan="3">
                                                                                    <select id="ddPNBOccupation" style="width: 78%; height: 100%"></select></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 8%; height: 24px">Occupation Type:&nbsp;</td>
                                                                                <td align="left" style="width: 10%; height: 24px;">
                                                                                    <select id="ddPNBOccupationType" style="width: 96%; height: 100%"></select></td>

                                                                                <td class="cell_caption" style="width: 6%; height: 24px">Worker Type:&nbsp;</td>
                                                                                <td align="left" style="width: 20%; height: 24px;">
                                                                                   <select id="ddPNBWorkerType" style="width: 60%; height: 100%"></select>
                                                                                </td>                                                      
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 8%; height: 23px">Position:&nbsp;</td>
                                                                                <td align="left" style="width: 10%">
                                                                                    <input id="txtPNBPosition" type="text" style="width: 95%; height: 100%; text-align: left;" maxlength="50" />
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 6%; height: 23px">Division:&nbsp;</td>
                                                                                <td align="left" style="width: 20%">
                                                                                    <input id="txtPNBDivision" type="text" style="width: 59%; height: 100%; text-align: left;" maxlength="50" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 8%; height: 23px">Sector:&nbsp;</td>
                                                                                <td align="left" style="width: 10%">
                                                                                    <input id="txtPNBSector" type="text" style="width: 95%; height: 100%; text-align: left;" maxlength="50" />
                                                                                </td>

                                                                                <td class="cell_caption" style="width: 6%; height: 23px">Supervisor:&nbsp;</td>
                                                                                <td align="left" style="width: 20%">
                                                                                    <input id="txtPNBSupervisor" type="text" style="width: 59%; height: 100%; text-align: left;" maxlength="100" /></td>                                                      
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 8%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" style="width: 80%; height: 24px;" colspan="4">
                                                                                    <input id="txtPNBOccuRemarks" type="text" style="width: 78%; height: 100%; text-align: left;" maxlength="50" /></td>
                                                                            </tr>
                                                                         </table>
                                                                    </div>
                                                                
                                                                    <div id = "CIFCARDSMAIN" style= "display:none;overflow:auto; ">
                                                                        <table id="tblCards" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr><td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="6">Cards</td></tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Card No.:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color:Red"><input id="txtCardNo" type="text" style="width: 93%; height: 100%; text-align: left;" maxlength="50" /><label id="astCardNo"></label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px;">Card Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddCardType" style="width: 95%;"></select><label id="astCardType"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Card Name:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color:Red"><input id="txtCardName" type="text" style="width: 93%; height: 100%; text-align: left;" maxlength="50" /><label id="astCardName"></label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">Card Class:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddCardClass" style="width: 95%;"></select><label id="astCardClass"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Issue Date: 
                                                                                    <br />
                                                                                    MM/YYYY&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color:Red"><input id="txtIssueDate" type="text" style="width: 88%; height: 100%; text-align: left;" maxlength="7" />
                                                                                <img id="calIssueDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtIssueDate').disabled == false){popUpCalendar(this, document.getElementById('txtIssueDate'), 'mm/yyyy');}"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                                <label id="astCarIssueDate"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">Expiry Date: MM/YYYY&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color:Red"><input id="txtExpiryDate" type="text" style="width: 86%; height: 100%; text-align: left;" maxlength="7" />
                                                                                <img id="calExpiryDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtExpiryDate').disabled == false){popUpCalendar(this, document.getElementById('txtExpiryDate'), 'mm/yyyy');}"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                                <label id="astCarExpiryDate"></label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 100%;">Issuing Bank:&nbsp;</td>
                                                                                <td align="left" style="width: 27%; height: 24px;"><input id="txtIssuingBank" type="text" style="width: 95%; text-align: left;" maxlength="1000" /></td>
                                                                                <td align="left" style="width: 9%; height: 24px; color:Red"><input id="btnSearchBank" class="button" onclick="javascript:cif_main.panel_search_bank();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 90%" /><label id="astIssuingBank"></label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">Currency:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddCardsCurrency" style="width: 95%"></select><label id="astCardCurr"></label></td>  
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Issuing No.:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color:Red"><input id="txtIssueNo" type="text" style="width: 93%; height: 100%; text-align: left;" maxlength="4" /><label id="astIssueNo"></label></td>  
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">Is ARM:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><input id="chkIsARM" style="width: 14%" type="checkbox" /><label id="astIsArm"></label></td>  
                                                                            </tr>  
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Country:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color:Red"><select id="ddCountry" style="width: 95%"></select><label id="astCountry"></label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px; display:none" id="tdGFMCLabel">GFC Account:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; display:none" id="tdGFMCDD"><label id="lblGFMCAccountNo" class="cell_desc"></label></td>
                                                                            </tr>
                                                                            <!--<tr style="display:none">
                                                                                <td class="cell_caption" style="width: 13%; height: 24px">Card Image:&nbsp;</td>
                                                                                <td align="left" colspan="4" style="width:60%; height: 24px;"><input id="fileUploadCardImage" type="file" style="width:84%;"/></td>
                                                                            </tr>                                                                                                                                                                                      
                                                                            <tr style="display:none">
                                                                                <td style="width: 13%; height: 23px"></td>
                                                                                <td align="left" colspan="4" style="width: 60%"><label id="lblCardImage" class="cell_desc">card image</label></td>
                                                                            </tr>-->
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" style="width: 60%; height: 24px; color:Red" colspan="5" rowspan="2"><input id="txtCardRemarks" type="text" style="width: 83%; text-align: left;" maxlength="255" /><label id="astCardRemarks"></label></td>                                                                                                    
                                                                            </tr>
                                                                         </table>                                                                             
                                                                   </div>                                                                               
                                                                   
                                                                   <div id = "CIFACCOUNTSMAIN" style= "display:none;overflow:auto; ">                                                                                      
                                                                        <table id="tblAccounts" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="6">Accounts</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Account No.:&nbsp;</td>
                                                                                <td align="left" style="color:Red" >
                                                                                    <input id="txtAccountNo" type="text" style="width: 95%; text-align: left;" maxlength="40" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" onkeyup="cif_main.keyup(txtAccountNo.id, event)";/><label id="astAccountNo"></label></td>
                                                                                <td align="left" style="width: 9%; height: 23px" class="comment"> Press enter  </td>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Account Type:&nbsp;</td>
                                                                                <td align="left" style="width: 29%; height:23px; color:Red"><select id="ddAccountType" style="width: 94%;"></select><label id="astAccountType"></label></td>
                                                                            </tr>
                                                                            <tr id="trLoading5" style="display: none">
                                                                                <td style="width: 13%; height: 23px"><div id="divLoading5"></div></td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 23px"><label id="lblLoad5">Loading...</label></td>
                                                                                <td style="width: 13%; height: 23px">
                                                                                </td>
                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">PNB Branch:&nbsp;</td>
                                                                                <td align="left"><input id="txtPNBBranchCode" type="text" style="width: 95%; text-align: left;" maxlength="1000"/></td>
                                                                                <td align="left" style="width: 9%; height: 23px; color:Red">
                                                                                    <input id="btnSearchPNBBranch" class="button" onclick="javascript:cif_main.panel_search_acctpnbbranch();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 80%;" /><label id="astPNBBranchCode"></label></td>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">
                                                                                    Currency:&nbsp;</td>
                                                                                <td align="left" style="width: 29%; height: 23px; color:Red">
                                                                                    <select id="ddAccCurrency" style="width: 94%;"></select><label id="astAccCurrency"></label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">Other Bank Type:&nbsp;</td>
                                                                                <td align="left" style="height: 23px;"><select id="ddOtherBankType" style="width: 96%;">
                                                                                </select>
                                                                                </td>
                                                                                <td align="left" style="width: 9%; height: 23px; color:Red">
                                                                                    <label id="astDomBankCode"></label></td>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px;">Status:&nbsp;</td>
                                                                                <td align="left" style="width: 29%; height:23px; color:Red"><select id="ddAccountStatus" style="width: 94%;"></select><label id="astAccountStatus"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">
                                                                                    Other Banks:&nbsp;</td>
                                                                                <td align="left" style="height: 23px">
                                                                                    <input id="txtDomBankCode" type="text" style="width: 95%; text-align: left;" maxlength="1000"/></td>
                                                                                <td align="left" style="width: 9%; color: red; height: 23px">
                                                                                    <input id="btnSearchDomBank" class="button" onclick="javascript:cif_main.panel_search_acctdombank();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 80%" /></td>
                                                                                <td style="width: 13%; height: 23px"></td>
                                                                                <td align="left" style="width: 29%; color: red; height: 23px">
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trLblSearchBank" style="display:none">
                                                                                <td align="left" colspan="5" style="height: 23px" class="comment">
                                                                                     * Please enter Bank Name and Bank Address below if Bank is not in the selection.&nbsp;</td>
                                                                            </tr>
                                                                            <tr id="trSearchOtherBank1" style="display: none">
                                                                                <td class="cell_caption" style="width: 13%; height: 23px">
                                                                                    Bank Details:&nbsp;</td>
                                                                                <td align="left" colspan="3"><input id="txtMT103BankName" type="text" style="width: 98%; text-align: left;"/></td>
                                                                                <td align="left" style="width: 29%; color: red; height: 23px">
                                                                                    <input id="Button1" class="button" onclick="javascript:cif_main.panel_search_other_banks();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 41%" /></td>
                                                                            </tr>
                                                                            <tr id="trOtherBank1" style="display:none">
                                                                                <td class="cell_caption" style="width: 13%; height: 24px">
                                                                                    Bank Name:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color:Red">
                                                                                    <input id="txtOthBankName" type="text" style="width: 93%; text-align: left;"/><label id="Label45"></label></td>
                                                                            </tr>
                                                                            <tr id="trOtherBank2" style="display:none">
                                                                                <td class="cell_caption" style="width: 13%; height: 24px;">
                                                                                    Bank Address:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color:Red">
                                                                                    <input id="txtOthBankAddress" type="text" style="width: 93%; text-align: left;"/><label id="Label46"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 13%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="height: 24px; color:Red">
                                                                                    <input id="txtAccountRemarks" type="text" style="width: 93%; text-align: left;" maxlength="255" /><label id="astAccountRemarks"></label></td>
                                                                            </tr>
                                                                         </table>                                                                              
                                                                     </div>                                                                                                                                                               
                                                                 
                                                                     <div id = "CIFACCOUNTSMERCHANT" style= "display:none;overflow:auto; ">                                                                                      
                                                                        <table id="tblMerchantOtherInfoEntry" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="6"> Merchants Other Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 34%; height: 24px">Merchant Type:&nbsp;</td>
                                                                                <td align="left" colspan="4" style="width: 40%; height: 24px; color:Red">
                                                                                    <select id="ddMerchantType" style="width: 96%;"></select><label id="Label48"></label>
                                                                                </td>
                                                                                <td align="left" class="comment" style="width:25%">* For OBPS use</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 34%; height: 24px" >Account No. - Currency - Account Type:&nbsp;</td>
                                                                                <td align="left" colspan="4" style="width: 40%; height: 24px; color:Red">
                                                                                    <select id="ddMerchantAccountNo" style="width: 96%;"></select><label id="astMerAccountNo"></label>
                                                                                </td>
                                                                                <td align="left" style="width:25%"></td>
                                                                            </tr> 
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 34%; height: 24px" >Merchant Product Code:&nbsp;</td>
                                                                                <td align="left" colspan="4" style="width: 40%; height: 24px; color:Red">
                                                                                    <select id="ddMerchantProductName" style="width: 96%;"></select><label id="astMerPName"></label>
                                                                                </td>
                                                                                <td align="left" style="width:25%"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 34%; height: 24px">
                                                                                    Product Code:&nbsp;</td>
                                                                                <td align="left" colspan="4" style="width: 40%; height: 24px; color:Red">
                                                                                    <select id="ddProductCodeMer" style="width: 96%;" onchange="javascript:cif_main.set_mer_implementation_type()"></select><label id="astProdCodeMer"></label>
                                                                                </td>
                                                                                <td align="left" style="width:25%"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 34%; height: 24px">Implementation Type:&nbsp;</td>
                                                                                <td align="left" colspan="4" style="width: 40%; height: 24px; color:Red">
                                                                                    <select id="ddMerchantImplementationType" style="width: 96%;">
                                                                                    </select><label id="astMerIType"></label>
                                                                                </td>
                                                                                <td align="left" style="width:25%"></td>
                                                                            </tr>
                                                                         </table>                                                                               
                                                                   </div>
                                                                   
                                                                   <div id="divAddMemberID" style="width:40%;z-index:101;position:absolute;left:658px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 158px;">
		                                                                <table style="width:100%" cellpadding="0" cellspacing="6">
			                                                                <tr>
				                                                                <td colspan="2" style="height: 19px"><label id="lblInfoHeader"><b>Enter Member ID</b></label></td>
			                                                                </tr>
			                                                                <tr>
				                                                                <td class="cell_caption" align="left" style="font-size: 12px; width: 30%; height: 21px;"><label id="lblMemID">Member ID:</label></td>
				                                                                <td class="label1" align="left" style="color: red;width: 66%; height: 21px;">&nbsp;<input id="txtMemID" type="text" style="width: 87%; height: 100%; text-align: left;" onkeyup="cif_main.keyup(txtMemID.id, event);"/>
                                                                                    <label id="astMemID">*</label></td>
			                                                                </tr>
			                                                                <tr>
				                                                                <td id="capMandatory" colspan="2" style="width:70%" class="comment" align="left">
				                                                                &nbsp;<label id="lblMandatory">* Mandatory</label></td>
			                                                                </tr>
			                                                                <tr>
			                                                                    <td colspan="2" class="label1" align="right" style="height: 20px">
			                                                                        <table style="width:100%">
			                                                                            <tr>
			                                                                                <td style="width:50%" align=center>
					                                                                            <input id="btnAddMemId" type="button" value="Ok" style="width:60px;" onclick="cif_main.add_memberid()"/>
			                                                                                </td>
			                                                                                <td style="width:50%" align=center>
					                                                                            <input id="btnCloseMemId" type="button" value="Cancel" style="width:60px;" onclick="cif_main.close_memid()"/>
			                                                                                </td>
			                                                                            </tr>
			                                                                        </table>
				                                                                </td>
			                                                                </tr>
		                                                                </table>
                                                                   </div>
                                                                   
                                                                   <div id="divImgLoad" style="z-index:101;position:absolute;left:704px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 318px;">
		                                                                <table style="width:35%" cellpadding="0" cellspacing="6">
			                                                                <tr>
				                                                                <td class="cell_desc" align="left" style="font-size: 12px; width: 30%;">
				                                                                <label id="lblLoad">Loading...</label></td>
			                                                                </tr>
			                                                                <tr>
			                                                                    <td>
			                                                                        <center><div id="divLoading"></div></center>
			                                                                    </td>
			                                                                </tr>
		                                                                </table>
                                                                   </div>
                                                                   
                                                                   <div id="divAddCIFClass" style="z-index:101;position:absolute;left:727px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 463px;"><!--top: 150px;-->
                                                                    	<table id ="tblAddCIFClass" cellpadding="0" cellspacing="5" style="width:40%; border: solid 1px #336699; background:azure;">
	                                                                        <tr>
		                                                                        <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5"> List of Classifications</td>
	                                                                        </tr>
	                                                                        <tr>
		                                                                        <td style="vertical-align:top; height:100%; width:50%;">
			                                                                        <div id = "divAvailClass">   
				                                                                        <table id="Table4" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699;">
					                                                                        <tr>
						                                                                        <td class="cell_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black;" colspan="5"> Available</td>
					                                                                        </tr>
					                                                                        <tr>
						                                                                        <td style="height: 190px" valign="top"><div id = "DIVAVAILCLASSGRID">PLACE GRIDVIEW CURRENT CLASSIFICATIONS</div></td>
					                                                                        </tr>
				                                                                        </table>
			                                                                        </div>
		                                                                        </td>
	                                                                        </tr>
                                                                        </table>
                                                                        <table>
	                                                                        <tr>
                                                                                <td><input id="btnCancelAddCIFClass" class = "button" onclick = "javascript:cif_main.cancel_add_cif();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel"/></td>  
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                   
                                                                   <div id = "CIFREMITTERSMAIN" style= "display:none;overflow:auto; ">
                                                                        <table id="tblRemitter" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" colspan="6" style="color: black; text-indent: 5px; height: 24px;
                                                                                    text-align: left">
                                                                                    Remitter Other Information</td>
                                                                                <td align="left" style="height: 24px">
                                                                                    <input id="btnUpdateRemitter" class="button" onclick = "javascript:cif_main.check_session();cif_main.btn_commit();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Update"/>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 23%; height: 24px" >VIP Status:&nbsp;</td>
                                                                                <td align="left" style="width: 38%; height: 24px; color:Red" colspan="4">
                                                                                    <select id="ddVIPStatus" style="width: 94%;"></select><label id="astVIPStatus"></label>
                                                                                </td>
                                                                                <td align="left" colspan="1" style="width: 66%; height: 24px">
                                                                                </td>
                                                                                <td align="left" colspan="1" style="width: 10%; height: 24px">
                                                                                </td>
                                                                            </tr> 
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 23%; height: 24px" >Remitter Points:&nbsp;</td>
                                                                                <td align="left" style="width: 38%; height: 24px; color:Red" colspan="4">
                                                                                    <input id="txtRemitterPoints" type="text" style="width: 92%; text-align: left;"  onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));"/><label id="astRemitterPts"></label></td>
                                                                                <td align="left" colspan="1" style="width: 66%; height: 24px">
                                                                                </td>
                                                                                <td align="left" colspan="1" style="width: 10%; height: 24px">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 23%; height: 24px">
                                                                                    Risk Rating:&nbsp;</td>
                                                                                <td align="left" colspan="4" style="width: 38%; color: red; height: 24px">
                                                                                    <select id="ddRiskRating" style="width: 94%;">
                                                                                    </select><label id="astRiskRating"></label>
                                                                                </td>
                                                                                <td align="left" colspan="1" style="width: 66%; height: 24px">
                                                                                </td>
                                                                                <td align="left" colspan="1" style="width: 10%; height: 24px">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 23%; height: 24px">
                                                                                    Charge Type: &nbsp;</td>
                                                                                <td align="left" colspan="4" style="width: 38%; height: 24px;  border: solid 1px #336699; height: 100%">
                                                                                    <label id="lblChargeType" class="cell_desc"></label>
                                                                                </td>
                                                                                <td align="left" colspan="1" style="width: 66%; height: 24px; color:Red">
                                                                                    <input id="btnSearchChargeType" class="button" onclick="javascript:cif_main.panel_search_chargetype();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 27%" /><label id="astChargeType"></label></td>
                                                                                <td align="left" colspan="1" style="width: 10%; height: 24px">
                                                                                </td>
                                                                            </tr>
                                                                        </table>                                                                                        
                                                                    </div>
                                                                                                                                                 
                                                                   <div id = "CIFIDMAIN" style= "display:none;overflow:auto; "> 
                                                                        <table id="tblID" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr><td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">IDs</td></tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">ID No.:&nbsp;</td>
                                                                                <td align="left" style="width: 19%; color:Red"><input id="txtIDNo" type="text" style="width: 90%; text-align: left;" maxlength="50" /><label id="astIDNo"></label></td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px">ID Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddIDType" style="width: 70%;" onchange="javascript:cif_main.show_visa_type();"></select><label id="astIDType"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                    ID Classification:</td>
                                                                                <td align="left" style="width: 19%; height:23px; color: red">
                                                                                    <select id="ddIDClassification" style="width: 93%;">
                                                                                    </select><label id="astIDClassification"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px; display:none;" >ID Image:&nbsp;</td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px;">Visa Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddVisaType" style="width: 70%;"></select><label id="Label59"></label></td>
                                                                                <td align="left" style="width: 25%; display:none;">
                                                                                    </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">Expiry Date: MM/DD/YYYY&nbsp;</td>
                                                                                <td align="left" style="width: 19%; color:Red"><input id="txtIDExpiryDate" type="text" style="width: 83%; text-align: left;" maxlength="10" />
                                                                                <img id="calIDExpiryDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtIDExpiryDate').disabled == false){popUpCalendar(this, document.getElementById('txtIDExpiryDate'), 'mm/dd/yyyy');}"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                                <label id="astIDExpiryDate"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="display: none; width: 9%; height: 23px">
                                                                                </td>
                                                                                <td style="width: 9%; height: 23px">
                                                                                </td>
                                                                                <td align="left" style="width: 25%; color: red; height: 23px">
                                                                                </td>
                                                                                <td align="left" style="display: none; width: 25%">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                             <td class="cell_caption" style="width: 17%; height: 23px">ID Place of Issuance:&nbsp;</td>
                                                                             <td align="left" style="width: 19%; color:Red"><input id="txtPlaceOfIssuance" type="text" style="width: 90%; text-align: left;" maxlength="50" /><label id="astPlaceOfIssuance"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px; color:Red" colspan="3"><input id="txtIDRemarks" type="text" style="width: 84%; text-align: left;" maxlength="255" /><label id="astIDRemarks"></label></td>                                                                                                        
                                                                            </tr>
                                                                        </table>                                                                             
                                                                   </div>
                                                                   
                                                                   <div id = "CIFVANMAIN" style= "display:none;overflow:auto; "> 
                                                                        <table id="tblVan" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">Virtual Account Number</td></tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">Product:&nbsp;</td>
                                                                                <td align="left" style="width: 19%; color:Red" colspan="3"><input id="txtVanProduct" type="text" style="width: 76%; text-align: left;" maxlength="50" />
                                                                                    <input id="Button2" onclick="javascript:cif_main.panel_search_van_products();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 62px; height:19px" /><label id="astVanProduct"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                    Virtual Acct. No.:&nbsp;</td>
                                                                                <td align="left" style="width: 19%; height:23px; color: red">
                                                                                    <input id="txtVan" type="text" maxlength="30" style="width: 92%"/><label id="astVan"></label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px; display:none;" >Van Type:&nbsp;</td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px;">
                                                                                    Van Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddVanType" style="width: 70%;"></select><label id="astVanType"></label></td>
                                                                                <td align="left" style="width: 25%; display:none;">
                                                                                </td>
                                                                            </tr>
                                                                        </table>                                                                             
                                                                   </div>  
                                                                    
                                                                    <div id = "CIFCONTACTSMAIN" style= "display:none;overflow:auto; ">
                                                                        <table id="tblContact" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 125px; text-align: left; text-indent: 5px; color: Black" colspan="6">Contacts
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 24px; width: 21%;">Contact Type:&nbsp;</td>
                                                                                <%--<td colspan="3" align="left" style="width:29%; height: 24px; color:Red"><select id="ddContactType" style="width: 94%;" onchange="javascript:cif_main.show_smsalert();"></select><label id="astContactType"></label></td>--%>
                                                                                <td colspan="3" align="left" style="width:29%; height: 24px; color:Red"><select id="ddContactType" style="width: 94%;"></select><label id="astContactType"></label></td>                                                                                                
                                                                                <td class="cell_caption" style="width: 20%;height: 24px">Preferred No:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red"><input id="chkPreferredNo" style="width: 9%" type="checkbox" /><label id="astPreferredNo"></label></td> 
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 21%; height: 24px">
                                                                                    Contact Number:&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 29%; height: 24px; color:Red">
                                                                                    <input id="txtContactNo" type="text" maxlength="30" style="width: 92%" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));"/><label id="astContactNo"></label></td>
<%--                                                                                <td class="cell_caption" style="width: 20%; height: 24px">
                                                                                    SMS Alert:&nbsp;</td>
                                                                                <td align="left" style="height: 24px; color:Red">
                                                                                    <input id="chkSMSAlert" style="width: 9%;" type="checkbox" /><label id="astSMSAlert"></label></td>--%>
                                                                                <td></td>
                                                                                <td></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="height: 23px; width: 21%;">Email Address:&nbsp;</td>
                                                                                <td colspan="5" align="left" style="color:Red"><input id="txtEmailAddress" type="text" style="width: 62%" maxlength="255" /><label id="astEmailAddress"></label></td>
                                                                            </tr>
                                                                            <tr id ="trContactRemarks" style= "display:none">
                                                                                <td class="cell_caption" style="height: 23px; width: 21%;">Remarks:&nbsp;</td>
                                                                                <td colspan="5" align="left" style="color:Red"><input id="txtContactRemarks" type="text" style="width: 62%" maxlength="255" /><label id="Label65"></label></td>
                                                                            </tr>
                                                                        </table>                                                                            
                                                                   </div>                                                                               
                                                                   
                                                                   <div id = "CIFSPOUSEMAIN" style= "display:none;overflow:auto; "> 
                                                                        <table id="tblSpouse" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr id="trSpouseWoBtn">
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">Spouse</td>
                                                                            </tr>
                                                                            <tr id="trSpouseWdBtn">
                                                                                <td class="table_caption" colspan = "2" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >Spouse</td>
                                                                                <td align="left" style="width: 15%; height: 24px;">
                                                                                    <input id="btnAddtoBene" class="button" onclick = "javascript:cif_main.set_spouse_as_bene();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add to Beneficiaries" style="width: 100%" />                                                                                                   
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 24px" >Last Name:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px;" colspan="4">
                                                                                    <input id="txtSpouseLName" type="text" style="width: 50%; height: 100%; text-align: left;" maxlength="50" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 24px" >First Name:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px;" colspan="4">
                                                                                    <input id="txtSpouseFName" type="text" style="width: 50%; height: 100%; text-align: left;" maxlength="50" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 24px" >Middle Name:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px;" colspan="4">
                                                                                    <input id="txtSpouseMName" type="text" style="width: 50%; height: 100%; text-align: left;" maxlength="50" /></td>
                                                                            </tr>                                                                                            
                                                                         </table>
                                                                    </div> 
                                                                   
                                                                    <div id = "CIFGEFUMAIN" style= "display:none;overflow:auto; ">
                                                                        <table id="tblCIFGEFU" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" colspan = "2" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >CIF GEFU</td>
                                                                                <td align="left" style="width: 10%; height: 24px;">
                                                                                    <input id="btnCreateGefu" onclick="javascript:cif_main.create_gefu();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Create" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 22%; height: 24px">
                                                                                    Date: MM/DD/YYYY&nbsp;</td>
                                                                                <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                                    <input id="txtCIFDate" style="width: 27%" type="text" maxlength="10" onkeyup="cif_main.keyup(txtCIFDate.id, event);"/></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                    
                                                                   
                                                                   <div id = "CIFATTACHBENEMAIN" style= "display:none">   
                                                                       <table id="tblAttachBene" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" colspan = "2" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >Search Beneficiary</td>
                                                                                <td align="left" style="width: 10%; height: 24px;">
                                                                                    <input id="btnSearchBene" onclick="javascript:cif_main.check_session();cif_main.panel_search_bene();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 24px">Last Name:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                                    <input id="txtSearchLName" style="width: 50%" type="text" maxlength="200" onkeyup="cif_main.keyup(txtSearchLName.id, event);"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 24px" >First Name:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px;" colspan = "2">
                                                                                    <input id="txtSearchFName" style="width: 50%" type="text" maxlength="50" onkeyup="cif_main.keyup(txtSearchFName.id, event);" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 24px" >Middle Name:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px;" colspan = "2">
                                                                                    <input id="txtSearchMName" style="width: 50%" type="text" maxlength="50" onkeyup="cif_main.keyup(txtSearchMName.id, event);" /></td>
                                                                            </tr>                                                                             
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 24px" >CIF ID:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px;" colspan = "2">
                                                                                    <input id="txtSearchBeneCIFID" style="width: 50%" type="text" maxlength="16" onkeyup="cif_main.keyup(txtSearchBeneCIFID.id, event);" /></td>
                                                                            </tr>                                                                               
                                                                        </table><br />
                                                                        
                                                                        <div id = "CIFUPDATEBENE" style= "display:none">
                                                                            <table id="tblUpdateBene" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                                <tr>
                                                                                    <td class="table_caption" colspan="2" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >
                                                                                        Update Beneficiary</td>
                                                                                    <td align="left" style="width: 10%; height: 24px;">
                                                                                        <input id="btnUpdateBene" class="button" onclick = "javascript:cif_main.check_session();cif_main.btn_update();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Update" disabled="disabled"/>
                                                                                    </td>
                                                                                    <td align="left" style="width: 10%; height: 24px;">
                                                                                        <input id="btnDeleteBene" class="button" onclick = "javascript:cif_main.check_session();cif_main.btn_delete();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Delete" disabled="disabled"/>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 26%; height: 24px" >Beneficiary Name:&nbsp;</td>
                                                                                    <td align="left" style="width: 74%; height: 24px;" colspan="4">
                                                                                        <label id="lblBeneFNameOrBN" class="cell_desc";>&nbsp;</label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 26%; height: 24px" >Relationship:&nbsp;</td>
                                                                                    <td align="left" style="width: 74%; height: 24px; color:Red" colspan="4">
                                                                                        <select id="ddBeneRelationship" style="width: 60%;">
                                                                                            <option selected="selected"></option></select><label id="astBeneRel"></label>
                                                                                    </td>
                                                                                </tr> 
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 26%; height: 24px" >Product Code:&nbsp;</td>
                                                                                    <td align="left" style="width: 74%; height: 24px; color:Red" colspan="4">
                                                                                        <select id="ddBeneProductCode" onchange="javascript:cif_main.set_implementation_type();cif_main.show_hide_beneaccountno(this.value);" style="width: 60%;">
                                                                                            <option selected="selected"></option></select><label id="astBenePCode"></label>
                                                                                    </td>
                                                                                </tr> 
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 26%; height: 24px" >Implementation Type:&nbsp;</td>
                                                                                    <td align="left" style="width: 74%; height: 24px; color:Red" colspan="4">
                                                                                        <select id="ddBeneImpCode" style="width: 60%;">
                                                                                            <option selected="selected"></option></select><label id="astBeneICode"></label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 26%; height: 24px" >Notification Mode:&nbsp;</td>
                                                                                    <td align="left" style="width: 74%; height: 24px; color:Red" colspan="4">
                                                                                        <select id="ddBeneNotificationMode" style="width: 60%;">
                                                                                            <option selected="selected"></option></select><label id="astBeneNMode"></label>
                                                                                    </td>
                                                                                </tr>                                                                                                   
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 26%; height: 24px">Beneficiary Account No:&nbsp;</td>
                                                                                    <td align="left" colspan="4" style="width: 74%; height: 24px; color:Red">
                                                                                        <select id="ddBeneAccountNo" style="width: 60%;" onchange="javascipt:cif_main.set_target_curr();">
                                                                                            <option selected="selected"></option>
                                                                                        </select><label id="astBeneAccountNo"></label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 26%; height: 24px">
                                                                                        Target Currency:&nbsp;</td>
                                                                                    <td align="left" colspan="4" style="width: 74%; height: 24px; color:Red">
                                                                                        <select id="ddTargetCurr" style="width: 60%;">
                                                                                            <option selected="selected"></option>
                                                                                        </select>*
                                                                                        <input id="btnGFC2" class="button" onclick="javascript:cif_main.GFC_Enrollment();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Enroll GFC" style="width: 17%" /></td>
                                                                                </tr>
                                                                            </table>  <br />                                                                                      
                                                                        </div>
                                                                        
                                                                        <div id = "CIFATTACHBENEENTRY" style= "display:none">   
                                                                            <table id="tblBeneGrid" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                                <tr>
                                                                                    <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">
                                                                                        List of Beneficiaries</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><div id = "CIFBENEGRID" style= "display:none">   PLACE GRIDVIEW REMITTER'S BENEFICIARIES</div></td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>                                                                                    
                                                                   </div> 
                                                                   
                                                                   <div id = "CIFATTACHMERCHANTMAIN" style= "display:none;overflow:auto; ">   
                                                                       <table id="tblAttachMerchant" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" colspan = "2" style="width: 90%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >Search Merchant</td>
                                                                                <td align="left" style="width: 10%; height: 24px;">
                                                                                    <input id="btnSearchMerchants" onclick="javascript:cif_main.check_session();cif_main.panel_search_merchants();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                    Business Name:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                                    <input id="txtSearchBusName" style="width: 43%" type="text" onkeyup="cif_main.keyup(txtSearchBusName.id, event);"/></td>
                                                                            </tr>
                                                                        </table><br />
                                                                        
                                                                        <table id="tblUpdateMerchant" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" colspan = "3" style="width: 90%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >Update Member ID No</td>
                                                                                <td align="left" style="width: 10%; height: 24px;">
                                                                                    <input id="btnUpdateMemID" onclick="javascript:cif_main.check_session();cif_main.btn_update();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Update" disabled="disabled" />
                                                                                </td>
                                                                                <td align="left" style="width: 10%; height: 24px;">
                                                                                    <input id="btnDeleteMerchant" onclick="javascript:cif_main.check_session();cif_main.btn_delete();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Delete" disabled="disabled"/>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 31%; height: 24px">Merchant Business Name:&nbsp;</td>
                                                                                <td class="cell_desc" align="left" colspan="3" style="height: 24px">
                                                                                    <label id="lblMerBusinessName"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 31%; height: 24px">Account No. - Currency:&nbsp;</td>
                                                                                <td class="cell_desc" align="left" colspan="3" style="height: 24px">
                                                                                    <label id="lblMerAccountNo"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 31%; height: 24px">Merchant Product Code:&nbsp;</td>
                                                                                <td class="cell_desc" align="left" colspan="3" style="height: 24px">
                                                                                    <label id="lblMerchantProdCode"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 31%; height: 24px">Product Code:&nbsp;</td>
                                                                                <td class="cell_desc" align="left" colspan="3" style="height: 24px">
                                                                                    <label id="lblMerProdCode"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 31%; height: 24px">Implementation Type:&nbsp;</td>
                                                                                <td class="cell_desc" align="left" colspan="3" style="height: 24px">
                                                                                    <label id="lblMerImpType"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 31%; height: 24px"><label id="lablMemID">Member ID:</label>&nbsp;</td>
                                                                                <td align="left" colspan="3" style="height: 24px; color:Red">
                                                                                    <input id="txtMemberIDNo" style="width: 50%" type="text"/><label id="astMemberIDNo"></label></td>
                                                                            </tr>
                                                                        </table><br />
                                                                        
                                                                        <div id = "CIFATTACHMERCHANTENTRY" style= "display:none">   
                                                                            <table id="tblMerchantGrid" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                                <tr>
                                                                                    <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">
                                                                                        List of Merchants</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td><div id = "CIFMERCHANTGRID" style= "display:none">   PLACE GRIDVIEW REMITTER'S MERCHANTS</div></td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>                                                                                    
                                                                   </div> 
                                                                                                                                                                                                                                                                                                              
                                                                   <div id = "CIFMAINBUTTON" style= "display:none">  
                                                                        <table id="tblButtonsMain"  cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                                            <tr>
                                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "tblbuttons">
                                                                                        <tr>
                                                                                            <td style="width: 18%" id="trBTNADD"><input id="btnAdd"    class = "button" onclick = "javascript:cif_main.check_session();cif_main.btn_insert();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Add"/></td>
                                                                                            <td style="width: 18%"><input id="btnEdit"   class = "button" onclick = "javascript:cif_main.check_session();cif_main.btn_update();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Edit"/></td>                                                                                                            
                                                                                            <td id= "tdDelete" style="width: 18%"><input id="btnDelete" class = "button" onclick = "javascript:cif_main.check_session();cif_main.btn_delete();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Delete"/></td>
                                                                                            <td style="width: 18%"><input id="btnSave"   class = "button" onclick = "javascript:cif_main.check_session();cif_main.btn_commit();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Save"/></td>
                                                                                            <td style="width: 18%"><input id="btnCancel" class = "button" onclick = "javascript:cif_main.btn_rollback();" onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel"/></td>                                                                                                                
                                                                                        </tr>
                                                                                    </table>                                                                                                    
                                                                                </td>
                                                                            </tr>                                                                    
                                                                        </table>
                                                                    </div> 
                                                                    <div id = "ClOSECANCELBTNS" style="display:block">
                                                                        <table id="Table1" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                                            <tr>
                                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                                    <table style="width:30%" cellpadding="0" cellspacing="5px" id = "tblClOSECANCELBTNS">
                                                                                        <tr>
                                                                                            <td style="width: 18%"><input id="btnClose"    class = "button" onclick = "closer();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Close"/></td>
                                                                                            <td style="width: 18%"><input id="btnCancel_1" class = "button" onclick = "cancel();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel"/></td>  
                                                                                        </tr>
                                                                                    </table>                                                                                                    
                                                                                </td>
                                                                            </tr>                                                                    
                                                                        </table>                                                                    
                                                                    </div>
                                                                    
                                                                    <!--GRIDS-->   
                                                                    
                                                                    <div id = "CIFWATCHLIST" style= "display:none;overflow:auto; ">
                                                                         <table id="tblWatchlist" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">Watchlist</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="comment" colspan="5" align="left"><span id="spnUntag"></span></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFWATCHLISTGRID" style= "display:none; background:azure;">  PLACE GRIDVIEW WATCHLIST</div></td>
                                                                            </tr>
                                                                          </table>
                                                                    </div>
                                                                    
                                                                    <div id = "CIFOTHERIDS" style= "display:none">
                                                                         <table id="tblOtherCIFIDs" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">List of Other CIF IDs</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFOTHERIDSGRID" style= "display:none">  PLACE GRIDVIEW OTHER CIF IDS</div></td>
                                                                            </tr>
                                                                            <tr id="trCIFFCAPI">
                                                                                <td align="right"> <input id="btnAddCIFtoFCAPI"    class = "button" onclick = "javascript:cif_main.check_session();cif_main.insert_CIF_to_FC();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Add CIF to FC" style="width: 146px"/>
                                                                                    &nbsp; &nbsp;
                                                                                </td>
                                                                            </tr>
                                                                          </table>
                                                                    </div>
                                                                    <div id="divSaveCIFtoFCLoading" style="display:none">                                                                                                                                                                                                                                    
                                                                        <table style="width:100%">
                                                                            <tr>
                                                                                <td style="width: 19%; height: 23px"><div id="divSaveCIFtoFCLoading_" class="table_body"></div></td>
                                                                                <td align="left" style="width: 71%; height: 24px" class="cell_desc" colspan="2"><label id="Label4">Loading...</label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id = "CIFSPOUSEGRIDMAIN" style= "display:none">
                                                                         <table id="tblGridSpouse" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">List of Spouse Information</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFSPOUSEGRID" style= "display:none">  PLACE GRIDVIEW SPOUSE</div></td>
                                                                            </tr>
                                                                          </table>
                                                                     </div>
                                                                      
                                                                    <div id = "CIFMERCHANTSGRIDMAIN" style= "display:none">
                                                                        <table id="tblMerchantOtherInfo" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">List of Merchants Other Information</td>                                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFMERCHANTSGRID" style= "display:none">  PLACE GRIDVIEW MERCHANTS OTHER INFORMATION</div></td>
                                                                            </tr>
                                                                         </table>
                                                                   </div>
                                                                    
                                                                    <div id = "CIFACCOUNTSGRIDMAIN" style= "display:none">
                                                                         <table id="tblGridAccounts" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">List of Accounts Information</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFACCOUNTSGRID" style= "display:none">  PLACE GRIDVIEW ACCOUNTS</div></td>
                                                                            </tr>
                                                                          </table>
                                                                     </div>
                                                                      
                                                                    <div id = "CIFCARDSGRIDMAIN" style= "display:none">
                                                                        <table id="tblGridCards" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="5">List of Cards Information</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFCARDSGRID" style= "display:none">  PLACE GRIDVIEW CARDS</div></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                                                                                                   
                                                                    <div id = "CIFPNBGRID" style= "display:none">
                                                                        <table id="tblGridPNBOccupation" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">List of Occupation Information</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFPNBOCCUPATIONGRID" style= "display:none">   PLACE GRIDVIEW PNB OCCUPATIONS</div></td>
                                                                            </tr>
                                                                        </table>                                                                               
                                                                    </div>
                                                                        
                                                                    <div id = "CIFOCCGRID" style= "display:none">
                                                                        <table id="tblGridOccupation" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">List of Occupation Information</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFOCCUPATIONSGRID" style= "display:none">   PLACE GRIDVIEW OCCUPATIONS</div></td>
                                                                            </tr>
                                                                        </table>                                                                               
                                                                    </div>
                                                                     
                                                                    <div id = "CIFADDRESSGRIDMAIN" style= "display:none">
                                                                        <table id="tblGridAddress" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="5">List of Address Information</td>
                                                                                
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 19px"><div id = "CIFADDRESSGRID" style= "display:none">PLACE GRID VIEW ADDRESS</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                    <div id = "CIFIDGRIDMAIN" style= "display:none">
                                                                        <table id="tblGridIds"  cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="5">List of IDs Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 19px"><div id = "CIFIDGRID" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                   
                                                                   <div id = "CIFVANGRIDMAIN" style= "display:none">
                                                                        <table id="tblGridVan" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="5">List of VAN Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 19px"><div id = "CIFVANGRID" style= "display:none">  PLACE GRIDVIEW VAN</div></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                   
                                                                    <div id = "CIFCONTACTSGRIDMAIN" style= "display:none">
                                                                        <table id="tblGridContacts" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="5">List of Contacts Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "CIFCONTACTSGRID" style= "display:none">   PLACE GRIDVIEW CONTACTS</div></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                                                                                                   
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
                    </td>
                </tr>        
            </table>
        </form>
    </center>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../CIF/cifmain.aspx" target="_parent"></form>
</body>
</html>
