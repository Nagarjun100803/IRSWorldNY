<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetAddBeneficiary.aspx.cs" Inherits="IRSWorld.Internet.InternetAddBeneficiary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Internet Remitter</title>
    <link href="../css/reskin_min.css" rel="Stylesheet" type="text/css" />
    <style type="text/css"></style>

     <script src="../js/commons_min.js"                             type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js"                       type="text/javascript"></script>
    <script src="../js/js_generic_min.js"                           type="text/javascript"></script>
    <script src="../js/js_sidebar_min.js"                           type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js"                        type="text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"                        type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js"                       type="text/javascript"></script>
    <script src="../js/Internet/jsInternetAddBeneSideBar.js"        type="text/javascript"></script>
    <script src="../js/Internet/jsInternetAddBeneficiary.js"        type="text/javascript"></script>
    <script src="../js/Internet/jsaddbeneficiary.js"                type="text/javascript"></script>
    <script src="../js/Internet/jsInternetAccounts_min.js"          type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemitterCards_min.js"     type="text/javascript"></script>
    <script src="../js/Internet/jsAddBeneProducts.js"               type="text/javascript"></script>
    <script src="../js/Internet/jsInternetGridView_min.js"          type="text/javascript"></script>
    <script src="../js/CIF/js_cif_beneficiaries_min.js"             type="text/javascript"></script>
    <script src="../js/CardRem/jsGFMCMaintenance_min.js"            type="text/javascript"></script> 
    <script src="../js/Internet/jsInternetRemitterIds_min.js"       type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemitterVan_min.js"       type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemitterNotes_min.js"     type="text/javascript"></script>
    <script src="../js/Remittance/jsRemExternals_min.js"            type="text/javascript"></script>
    <script src="../js/popcalendar_min.js"                          type="text/javascript"></script>
    <script type="text/javascript">
            var Internet_Remitter           = new Internet_Remitter(); 
            var Internet_Beneficiary        = new Internet_Beneficiary();
            var Internet_Accounts           = new Internet_Accounts();
            var Internet_Remitter_Cards     = new Internet_Remitter_Cards();
            var Internet_Remitter_Main      = new Internet_Remitter_Main();
            var Internet_Remitter_Products  = new Internet_Remitter_Products();
            var Internet_Remitter_GFC       = new GFMC_Maintenance();
            var generic                     = new generic_method();
            var Internet_Remitter_Ids       = new Internet_Remitter_Ids();
            var Internet_Remitter_Van       = new Internet_Remitter_Van();
            var Internet_Remitter_Notes     = new Internet_Remitter_Notes();
    </script>
    
</head>
<body onresize="divInetRemitResize();" onload="javascript:Internet_Remitter_Main.init_main();" onunload="javascript:close_session();" onkeyup="search_check_gfmc(event);"> <!--onkeyup="check_gfmc(event)"> -->
    <form id="form1" runat="server" autocomplete="Off">
        <div id=divmain style="width: 100%; padding-top:15px">
                <table style="width:100%">
                    <tr>
                        <td colspan="2">
                            <input style="WIDTH: 1%" id="txtHiddenSearchSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenSearchCIFID" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenCardSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenBeneSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenBeneMasterSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenNewlyAddedCardSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenUserID" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenUserRole" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenUserLoc" type="hidden" />                                                                    
                            <input style="WIDTH: 1%" id="txtHiddenRemCIFID" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenBeneCIFID" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenSelectedBeneCIFID" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenBankSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenUserCIFID" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenCardBankCode" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenZipSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenZipSeqId_Com" type="hidden" /> 
                            <input style="WIDTH: 1%" id="txtHiddenZipSeqId_Mail" type="hidden" />  
                            <input style="WIDTH: 1%" id="txtHiddenZip" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenRemAge" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenBeneAge" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenBeneProdSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenKBIAccount" type="hidden" />   
                            <input style="WIDTH: 1%" id="txtHiddenMT103BankSeqId" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenFCCustCurrenctField" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenChargeType" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenVanProductSeqID" type="hidden" />
                            <input style="WIDTH: 1%" id="txtHiddenVanBeneCIFID" type="hidden" />
                            <input id="txtHiddenGFMCNo" type="text" style="text-decoration: none; color: white; cursor: text; margin: 0px; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none; background-color: transparent; width:0px; height:0px; left: -100px; position: absolute; top: -100px;" onfocus="get_focus(this.id);"/> 
                        </td>
                    </tr>
                    <tr>
                        <td id="tdSearchMain" colspan="2" style="width:100%;text-align:center; vertical-align:middle; border-collapse:collapse" onkeyup="search_check_gfmc(event);">
                            <center>
                            <div id="divInternetRemitterSearch" style="width: 360px;font-family:tahoma;width:auto;height:auto;display:none;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td id="tdInternetRemitterSearch" style="width:400px; height:194px; text-align:center; vertical-align:middle;">
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td style="width:100%">
                                                        <table cellpadding="0" cellspacing="0" style="width:100%">
                                                            <tr>
                                                                <td align="left" valign="bottom" style="vertical-align:bottom; width:0%; height:24px; border-collapse:collapse"> 
                                                                    <img src="../images/reskin/toe.GIF" /></td>    
                                                                <td class="table_head_dark" style="width: 60%; height: 24px">
                                                                    SEARCH</td>
                                                                <td align="left" style="width: 40%; height: 24px; vertical-align:bottom;">
                                                                    <img align="absBottom" src="../images/reskin/rend.GIF" /></td> 
                                                            </tr>  
                                                        </table> 
                                                    </td>                                                               
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%">
                                                        <table id="tbsearch" border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid">
                                                            <tr>
                                                                <td colspan=2 style="width: 100%;height: 8px;"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:250px;" class="cell_caption">
                                                                    Search By
                                                                </td>
                                                                <td style="width:450px;" align="left">
                                                                    <select id="ddSearchBy" class="ddl" style="width:99%"></select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:250px;" class="cell_caption">
                                                                    Last Name
                                                                </td>
                                                                <td style="width:450px;" align="left">
                                                                    <input id="txtSearchLName" style="width:97%;text-align:left;" type="text" value=""  onkeyup="Internet_Remitter_Main.keyup(this.id, event);" onfocus="get_focus(this.id);"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:250px;" class="cell_caption">
                                                                    First Name
                                                                </td>
                                                                <td style="width:450px;" align="left">
                                                                    <input id="txtSearchFName" style="width:97%;text-align:left;" type="text" value="" onkeyup="Internet_Remitter_Main.keyup(this.id, event);" onfocus="get_focus(this.id);"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:250px;" class="cell_caption">
                                                                    Middle Name
                                                                </td>
                                                                <td style="width:450px;" align="left">
                                                                    <input id="txtSearchMName" style="width:97%;text-align:left;" type="text" value="" onkeyup="Internet_Remitter_Main.keyup(this.id, event);" onfocus="get_focus(this.id);"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:250px;" class="cell_caption">
                                                                    Birth Date 
                                                                    <!--<br /> -->
                                                                    (mm/dd/yyyy)
                                                                </td>
                                                                <td style="width:450px;" align="left">
                                                                    <input id="txtSearchBDate" style="width:97%;text-align:left;" type="text" value="" maxlength="10" onkeyup="Internet_Remitter_Main.keyup(this.id, event);" onfocus="get_focus(this.id);"/>
                                                                </td>
                                                            </tr>
                                                            <!--
                                                            <tr>
                                                                <td style="width:250px;" class="cell_caption">
                                                                    test 
                                                                </td>
                                                                <td style="width:450px;" align="left">
                                                                    <a id="a1" href="javascript:show_frame_dialog('../Internet/InternetRemitter.aspx?isRemit=obp', 'obpClose', '100%', '100%', '0');">test otc</a>
                                                                </td>
                                                            </tr>
                                                            -->
                                                            <tr>
                                                                <td style="width:250px;" class="cell_caption">
                                                                    Exact Match
                                                                </td>
                                                                <td style="width:450px;" align="left">
                                                                    &nbsp;<input id="chkExactMatch" style="width: 12%" type="checkbox" onclick="javascript:Internet_Remitter_Products.IsRuralBank_OnClick();"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 28px; width:700px; vertical-align:top;text-align:center;" colspan = "2">
                                                                    <input type="button" value="Search" onclick="javascript:Internet_Remitter_Main.search_rem_cif();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnSearch"  style="width: 120px"/>
                                                                    <input type="button" value="Add New Remitter" onclick="javascript:Internet_Remitter_Main.btn_add_remitter();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="Button2" style="width: 120px"/>
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
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%; vertical-align: top; display: none; visibility: hidden;">
                            <div id="divAppMenu" style="margin-top: 5px; display:none">
                                <a id="a1hider" href="javascript:Internet_Remitter_Main.set_side_bar_visibility('divAppMenu', 'a1hider');">«</a>
                            </div>
                        </td>
                        <td style="width: 85%; vertical-align: top; padding-left:5px;" >
                            <div id="divInternetBeneApproval" style="width:100%;display:none;">
                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr>
                                        <td style="width: 100%">
                                            <table cellpadding=0 cellspacing=0>
                                            <tr>
                                                <td align="left" valign="bottom" style="vertical-align:bottom; width:0%; border-collapse:collapse;height:24px;"><img src="../images/reskin/toe.GIF" /></td>
                                                <td class="table_head_dark" style="width:50%;height:24px;">
                                                Internet Beneficiary Approval</td>
                                                <td align="left" style="width:50%;height:24px;vertical-align:bottom;" valign="bottom"><img align="absBottom" src="../images/reskin/rend.GIF" /></td>
                                            </tr>
                                            </table>  
                                        </td>  
                                    </tr>
                                    <tr>
                                        <td style="width:100%;vertical-align:top;">
                                            <table style="width:97%;border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;">
                                                <tr>
                                                    <td style="width: 80%">
                                                        <div id="divIntBeneForApp" style="width:100%; display:none">
                                                            <table id="tblIntBeneForApp" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                <tr>
                                                                    <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Beneficiaries ( For Approval )</td>
                                                        			
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5"><div id="DIVBENEFORAPP">  PLACE GRIDVIEW BENEFICIARIES TO APPROVE</div></td>
                                                                </tr>
                                                                <tr id="trLoading2" style="display: none">
                                                                    <td style="text-align:center" class="cell_desc" colspan="5"><label id="lblLoad4">Loading...</label>
                                                                    <div id="divLoading4"></div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>                                                                
                                                        <div id = "divIntCardForApp" style= "display:none">
                                                            <table id="tblntCardForApp" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                <tr>
                                                                    <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Cards ( For Approval )</td>
                                                                </tr>
                                                                <tr>
                                                                    <td><div id = "DIVCARDFORAPP">  PLACE GRIDVIEW CARDS TO APPROVE</div></td>
                                                                </tr>
                                                            </table>
                                                        </div>         
                                                        <div id="divImgLoad2" style="z-index:101;left:501px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: tahoma; top: 500px;text-align:center;">
                                                            <table style="width:35%;" cellpadding="0" cellspacing="6">
                                                                <tr>
                                                                    <td class="cell_desc" style="text-align:left; font-size: 12px; width: 30%; height: 23px;">
                                                                    <label id="lblLoad2">Loading...</label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <center><div id="divLoading2"></div></center>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                       </div>                                                       
                                                        <div id="divBeneficiary" style="z-index:101;left:724px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: tahoma; top: 37px; text-align:center;"><!--top: 18px;-->
                                                            <table style="width:97%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td colspan="4" style="height: 19px"><label id="lblInfoHeader"><b>Beneficiary Information</b></label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4"><hr /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px; height: 4px;"><label id="lblClientName">Beneficiary Name:</label></td>
                                                                    <td colspan="3" style="width:75%; text-align:left; height: 4px;" class="label1">&nbsp;<span id="spnBeneName" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px;">Address:</td>
                                                                    <td colspan="3" style="width:75%; text-align:left;" class="label1">&nbsp;<span id="spnBeneAddress" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px">
                                                                        Birth Date:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;" >&nbsp;<span id="spnBeneBDate" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px">Telephone No:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;" >&nbsp;<span id="spnBeneTelNo" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px">Mobile No:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;" >&nbsp;<span id="spnBeneMobNo" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px;">Email Address:</td>
                                                                    <td colspan="3" style="width:75%; text-align:left;" class="label1">&nbsp;<span id="spnBeneEmailAdd" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr><td colspan="4">&nbsp;</td></tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px;">Product Code:</td>
                                                                    <td colspan="3" style="width:75%; text-align:left;" class="label1">&nbsp;<span id="spnBeneProductCode" style="font-size: 12px"></span></td>        
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px;">Account Number:</td>
                                                                    <td colspan="3" style="width:75%; text-align:left;" class="label1">&nbsp;<span id="spnBeneAccountNumber" style="font-size: 12px"></span></td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px;">Currency:</td>
                                                                    <td colspan="3" style="width:75%; text-align:left;" class="label1">&nbsp;<span id="spnBeneProductCurrency" style="font-size: 12px"></span></td>        
                                                                </tr>
                                                                <tr><td colspan="4">&nbsp;</td></tr>                                                                       
                                                                <tr>
                                                                    <td colspan="4" class="cell_desc" style="text-align:right; height: 20px">
                                                                        <input id="btnApproveBene" type="button" class = "button"value="Approve" style="width:70px;" onclick="Internet_Beneficiary.approve_bene();"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_desc" colspan="4" style="text-align:right; height: 20px">
                                                                        Reason:&nbsp;<input id="txtRejectBeneReason" type="text" style="width: 300px;" maxlength="500" />
                                                                        <input id="btnRejectBene" type="button" class="button" value="Reject" style="width:70px;" onclick="Internet_Beneficiary.reject_bene();"/></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="label1" colspan="4" style="text-align:right;height: 20px">
                                                                        <input id="btnBClosePanel" type="button" class="button" value="Close" style="width:70px;" onclick="hidePanel('divBeneficiary');"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_desc" colspan="4" style="text-align:left; height: 20px">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>                                                         
                                                        <div id="divCard" style="z-index:101;left:937px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: tahoma; top: 445px;text-align:center;"><!--top: 18px;-->
                                                            <table style="width:97%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td colspan="4" style="height: 19px"><label id="Label1"><b>Card Information</b></label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4"><hr /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px;"><label id="Label2">
                                                                        Remitter Name:</label></td>
                                                                    <td colspan="3" style="width:75%; text-align:left;" class="label1">&nbsp;<span id="spnRemName" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px;">
                                                                        Card No:</td>
                                                                    <td colspan="3" style="width:75%; text-align:left;" class="label1">&nbsp;<span id="spnCardNo" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px">
                                                                        Card Type:</td>
                                                                    <td align="left" class="label1" colspan="3" style="width:75%; text-align:left;">&nbsp;<span id="spnCardType" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px">
                                                                        Card Name:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;">&nbsp;<span id="spnCardName" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px; height: 119px;">
                                                                        Card Class:</td>
                                                                    <td colspan="3" style="width:75%; height: 119px; text-align:left;" class="label1">&nbsp;<span id="spnCardClass" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px">
                                                                        Issue Date:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;">&nbsp;<span id="spnIssueDate" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px">
                                                                        Expiry Date:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;">&nbsp;<span id="spnExpiryDate" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="text-align:left; font-size: 12px; width: 138px; height: 18px;">
                                                                        Issuing Bank:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;height: 18px">&nbsp;<span id="spnIssuingBank" style="font-size: 12px"></span></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="font-size: 12px; width: 138px;text-align:left">
                                                                        Currency:</td>
                                                                    <td class="label1" colspan="3" style="width:75%; text-align:left;">&nbsp;<span id="spnCurrency" style="font-size: 12px"></span></td>
                                                                </tr>                                                                    
                                                                <tr>
                                                                    <td colspan="4" class="label1" style="height: 20px; text-align:right;">
                                                                        <input id="btnApproveCard" type="button" class="button" value="Approve" style="width:70px;" onclick="Internet_Remitter_Cards.approve_card();"/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_desc" colspan="4" style="height: 20px; text-align:right;">
                                                                        Reason:&nbsp;<input id="txtRejectCardReason" type="text" style="width: 300px;" maxlength="500" />
                                                                        <input id="btnRejectCard" type="button" class="button" value="Reject" style="width:70px;" onclick=""/>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="label1" colspan="4" style="height: 20px;text-align:right;">
                                                                        <input id="Button4" type="button" class="button" value="Close" style="width:70px;" onclick="hidePanel('divCard');"/></td>
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
                    <tr>
                        <td id="tdInternetRemitterMenu" style="width: 15%; vertical-align: top; display: none; visibility: hidden;">
                            <div id="divInternetRemitterMenu" style="display:none">
                                <div id="divtdInternetRemitterMenu" style="margin-top: 5px; display: block;">

                                    <script type="text/javascript">
//                                            var sider = new js_sidebar('sidemenu', sidebar);
//                                            document.write(sider.toString());


                                    </script>

                                </div>
                                <!--<a id="ahider" href="#" onclick="javascript:Internet_Remitter_Main.set_side_bar_visibility('divtdInternetRemitterMenu', 'ahider');">
                                «</a>--> 
                                </div>
                                <a id="ahider" href="javascript:Internet_Remitter_Main.set_side_bar_visibility('divInternetRemitterMenu', 'ahider');">
                                «</a>
                        </td>
                        <td style="width: 85%; vertical-align:top; padding-left: 5px;"> <!--padding-top: 30px-->
                            <div id="divInternetRemitter" style="width: 85%; height: 420px; display: none; overflow: auto;">
                                <table style="width:99%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width:100%;">
                                            <table style="width:100%;" cellpadding="0" cellspacing="0" >
                                                <tr>
                                                    <td align="left" valign="bottom" style="vertical-align:bottom; width:0%;height:24px; border-collapse: collapse"> 
                                                        <img src="../images/reskin/toe.GIF" /></td>
                                                    <td id="capResult" class="table_head_dark" style="width:40%; height: 24px;">
                                                        Internet Remitter</td>
                                                    <td id="tdpopup2" align="left" style="width: 60%; vertical-align:bottom; height: 24px">
                                                        <img align="absbottom" src="../images/reskin/rend.GIF" />
                                                    </td>
                                                </tr>  
                                            </table>    
                                        </td>                                            
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 100%">
                                            <table style="width: 100%; height: 100%;" class="table_border">
                                                <tr>
                                                    <td style="width: 100%; height: 100%; vertical-align: top">
                                                        <table id="tblInternetRemitter" style="width:100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="text-align:center;">
                                                                    <div id="divProfileHeader" style="width: 100%; height: 100%; margin: 0; display:none;" class="table_body">
                                                                        <table id="tblProfileHeader" style="width:100%">
                                                                            <tr>
                                                                                <td class="label4" style="width: 50%"><label id="lblHeaderRemName"></label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>  
                                                            <tr>
                                                                <td>
                                                                    <div id="divBeneSearch" style="width: 100%; height: 100%; margin: 0; display:none" class="table_body">
                                                                        <table id="Table1" cellpadding="0" cellspacing="0" style="width:100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td style="width:100%">
                                                                                    <table style="width:100%" id="Table2" cellspacing="3">
                                                                                        <tr id="tr5">
                                                                                            <td class="table_caption" style="width: 80%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">
                                                                                                Search Beneficiary</td>
                                                                                            <td align="left" style="width: 20%; height: 24px; color: red;">
                                                                                                <input id="Button9" style="height: 25px; width: 95%" type="button" value="Search" class="button"
                                                                                                onclick="javascript:Internet_Remitter_Main.search_bene2();" onmouseover="this.className='button_over';"
                                                                                                onmouseout="this.className='button';" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td style="width:100%;">
                                                                                                <table style="width:100%">
                                                                                                    <tr>
				                                                                                        <td class="cell_caption" style="width: 18%; height: 23px">
					                                                                                        Search By:&nbsp;
				                                                                                        </td>
				                                                                                        <td align="left" colspan="3" style="height: 24px;">
					                                                                                        <select id="ddSearchBeneByCIFType" class="ddl" style="width:56%" onchange="javascript:Internet_Remitter_Main.change_searchbybene_label();" onkeyup="Internet_Remitter_Main.keyup(this.id, event);"></select>
				                                                                                        </td>
			                                                                                        </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                            <span id="spnSearchBeneLN" style="font-size: 12px">Last Name:</span></td>
                                                                                                        <td align="left" colspan="3" style="height: 24px;">
                                                                                                            <input id="txtSearchBeneByLN" type="text" style="width: 55%;" maxlength="200" onkeyup="Internet_Remitter_Main.keyup(this.id, event);"/></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                            <span id="spnSearchBeneFN" style="font-size: 12px">First Name:</span></td>
                                                                                                        <td align="left" colspan="3" style="height: 24px;">
                                                                                                            <input id="txtSearchBeneByFN" type="text" style="width: 55%;" maxlength="200" onkeyup="Internet_Remitter_Main.keyup(this.id, event);"/></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                            <span id="spnSearchBeneMN" style="font-size: 12px">Middle Name:</span></td>
                                                                                                        <td align="left" colspan="3" style="height: 24px;">
                                                                                                            <input id="txtSearchBeneByMN" type="text" style="width: 55%;" maxlength="200" onkeyup="Internet_Remitter_Main.keyup(this.id, event);"/></td>
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
                                                            <tr>
                                                                <td style="width: 100%; height: 100%; vertical-align:middle; text-align: right;">
                                                                    <div id="divRemInfo" style="width: 100%; height: 100%; margin: 0; display:none" class="table_body">
                                                                        <table id="tblRemInfo" cellpadding="0" cellspacing="0" style="width:100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td style="width:100%">
                                                                                    <table style="width:100%" id="tblRemInfo0" cellspacing="5">
                                                                                        <tr>
                                                                                            <td style="width:100%">
                                                                                                <div id="trHeaderRem" class="table_body">
                                                                                                    <table style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="width: 80%; text-align: left; text-indent: 5px; color: white; height: 26px;">
                                                                                                                Remitter Information</td>
                                                                                                            <td align="left" style="width: 20%; height: 24px; color: red;">
                                                                                                                <select id="ddCIFType" style="width: 100%" onchange="javascript:Internet_Remitter_Main.show_panel_rem_ind_or_business(this.value);Internet_Remitter.set_branchdef_values();">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                <div id="trHeaderRemClient" class="table_body">
                                                                                                    <table style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="2">
                                                                                                                Remitter Information</td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>                                                                                            
                                                                                        <tr>
                                                                                            <td style="width: 100%; color: black; text-indent: 5px;
                                                                                                height: 26px; text-align: left">
                                                                                                <div id="DIV_IND_REM" style="width:100%" class="table_body">
                                                                                                    <table id="tblRemInfo1" style="width:100%;">
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Last Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="width:82%; height: 24px; color: red;">
                                                                                                                <input id="txtRemLName" type="text" style="width: 95%;" maxlength="200" /><label id="astRemLName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>First Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="width:82%; height: 24px; color: red;">
                                                                                                                <input id="txtRemFName" type="text" style="width: 95%;" maxlength="80" /><label id="astRemFName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Middle Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="width: 82%; height: 24px; color: red;">
                                                                                                                <input id="txtRemMName" type="text" style="width: 95%;" maxlength="80" /><label id="astRemMName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Birth Date (mm/dd/yyyy):&nbsp;<br />
                                                                                                                </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBDate" type="text" style="width: 82%;"
                                                                                                                    onchange="javascript:Internet_Remitter.compute_age(1);" maxlength="10" />
                                                                                                                <img id="calRemBDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtRemBDate').disabled == false){popUpCalendar(this, document.getElementById('txtRemBDate'), 'mm/dd/yyyy', document.getElementById('divInternetRemitter'));}"
                                                                                                                    src="../images/calendar/imgcalen.png" />                                                                                                                            
                                                                                                                <label id="astRemBDate">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Birth Place:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBPlace" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBPlace">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                HOME ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>                                                                                                        
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemAddress1" type="text" style="width: 90%;" maxlength="200" /><label id="astRemAddress1">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemAddress2" type="text" style="width: 87%;" maxlength="200" /><label id="astRemAddress2">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemPOBox" type="text" style="width: 90%;" maxlength="200" /><label id="astRemPOBox">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                District:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemDistrict" type="text" style="width: 87%;" maxlength="200" /><label id="astRemDistrict">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                City:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemCity" type="text" style="width: 90%;" maxlength="200" /><label id="astRemCity">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                State:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemState" type="text" style="width: 87%;" maxlength="200" /><label id="astRemState">*</label></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Region:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemRegion" type="text" style="width: 90%;" maxlength="200" /><label id="astRemRegion">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Country:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddRemCountry" style="width: 87%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemCountry">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Zip Code:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemZip" type="text" style="width: 90%;" maxlength="200" /><label id="astRemZip">*</label></td>                                                                                                            
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="chkRemPrefMailAdd" style="width: 14%" type="checkbox" onclick="javascript:Internet_Remitter.onCheck_Preferred(1,'chkRemPrefMailAdd');"/><label id="astRemPrefMailAdd">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr style="display:none">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Address Type:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddRemAddressType" style="width: 91%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemAddressType">*</label>
                                                                                                            </td>
                                                                                                            <td></td>
                                                                                                            <td style="width: 32%"></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                COMPANY ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>                                                                                                        
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemAddress1_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemAddress1_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemAddress2_Com" type="text" style="width: 87%;" maxlength="200" /><label id="astRemAddress2_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemPOBox_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemPOBox_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                District:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemDistrict_Com" type="text" style="width: 87%;" maxlength="200" /><label id="astRemDistrict_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                City:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemCity_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemCity_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                State:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemState_Com" type="text" style="width: 87%;" maxlength="200" /><label id="astRemState_Com">*</label></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Region:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemRegion_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemRegion_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Country:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddRemCountry_Com" style="width: 87%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemCountry_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Zip Code:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemZip_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemZip_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="chkRemPrefMailAdd_Com" style="width: 14%" type="checkbox" onclick="javascript:Internet_Remitter.onCheck_Preferred(1,'chkRemPrefMailAdd_Com');"/><label id="astRemPrefMailAdd_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                MAILING ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>                                                                                                        
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemAddress1_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemAddress1_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemAddress2_Mail" type="text" style="width: 87%;" maxlength="200" /><label id="astRemAddress2_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemPOBox_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemPOBox_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                District:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemDistrict_Mail" type="text" style="width: 87%;" maxlength="200" /><label id="astRemDistrict_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                City:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemCity_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemCity_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                State:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="txtRemState_Mail" type="text" style="width: 87%;" maxlength="200" /><label id="astRemState_Mail">*</label></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Region:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemRegion_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemRegion_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Country:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddRemCountry_Mail" style="width: 87%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemCountry_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Zip Code:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemZip_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemZip_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><input id="chkRemPrefMailAdd_Mail" style="width: 14%" type="checkbox" onclick="javascript:Internet_Remitter.onCheck_Preferred(1,'chkRemPrefMailAdd_Mail');"/><label id="astRemPrefMailAdd_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                CONTACTS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Telephone No:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemTelNo" type="text" style="width: 90%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astRemTelNo">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Mobile No:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color:Red">
                                                                                                                <input id="txtRemMobileNo" type="text" style="width: 87%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astRemMobileNo">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Email Address:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color:Red">
                                                                                                                <input id="txtRemEmailAdd" type="text" style="width: 90%;" maxlength="200" /><label id="astRemEmailAdd">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Contact:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemPrefContact" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemPrefContact">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                EMPLOYMENT&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Company Name:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemCompany" type="text" style="width: 90%;
                                                                                                                   " maxlength="200" /><label id="astRemCompany">*</label>
                                                                                                            </td>
                                                                                                            <td style="width: 18%; height: 23px">
                                                                                                                </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="display:none">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Company Address:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;" colspan="4">
                                                                                                                <input id="txtAddress3k" type="text" style="width: 96%;" maxlength="200" /></td>
                                                                                                            
                                                                                                        </tr>                                                                                                                   
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Occupation:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemOccupation" style="width: 91%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemOccupation">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Detailed Occupation:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="txtRemDetDesc" type="text" style="width: 88%;" maxlength="200" /><label id="astRemDetDesc">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Annual Salary:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="txtRemAnnualSal" type="text" style="width: 90%;" maxlength="200" onkeypress="javascript:return(generic.autoFormatCurrency(this,',','.',event));"/><label id="astRemAnnualSal">*</label>    
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Currency:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemSalCurr" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemSalCurr">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Source of Funds:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemSourceOfFunds" style="width: 91%" onchange="javascript:Internet_Remitter.ClearAnnualSalary();">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemSourceOfFunds">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Worker Type:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemWorkerType" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemWorkerType">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                ID's&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 17px">
                                                                                                                ID No (1):</td>
                                                                                                            <td align="left" style="width: 32%; height: 17px; color: red;"><input id="txtRemIDNo1" type="text" style="width: 90%;" maxlength="50" /><label id="astRemIDNo1">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 17px">
                                                                                                                ID Expiry Date (mm/dd/yyyy):<br />
                                                                                                                &nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 17px">
                                                                                                                <input id="txtRemIDExpDate" type="text" style="width: 79%;" maxlength="10" />
                                                                                                                <img id="calRemIDExpDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtRemIDExpDate').disabled == false){popUpCalendar(this, document.getElementById('txtRemIDExpDate'), 'mm/dd/yyyy',document.getElementById('divInternetRemitter'));}"
                                                                                                                    src="../images/calendar/imgcalen.png" />                                                                                                                                                                                                 
                                                                                                                <label id="astRemIDExpDate">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                ID Type:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemIDType1" style="width: 92%" onchange="javascript:Internet_Remitter_Main.show_visa_type1(this.value, false);">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemIDType1">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                ID Classification:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemIDClassification1" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemIDClassification1">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                &nbsp;Visa Type:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemVisaType1" style="width: 92%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                            <td style="width: 18%; height: 23px">
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 17px">
                                                                                                                ID No (2):</td>
                                                                                                            <td align="left" style="width: 32%; height: 17px; color: red;"><input id="txtRemIDNo2" type="text" style="width: 90%;" maxlength="50" /><label id="astRemIDNo2">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 17px">
                                                                                                                ID Expiry Date (mm/dd/yyyy):<br />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 17px">
                                                                                                                <input id="txtRemIDExpDate2" type="text" style="width: 65%;" maxlength="10" />
                                                                                                                <img id="calRemIDExpDate2" align="absMiddle" onclick="javascript:if(document.getElementById('txtRemIDExpDate2').disabled == false){popUpCalendar(this, document.getElementById('txtRemIDExpDate2'), 'mm/dd/yyyy', document.getElementById('divInternetRemitter'));}"
                                                                                                                    src="../images/calendar/imgcalen.png" />                                                                                                                        
                                                                                                                <label id="astRemIDExpDate2">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                ID Type:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemIDType2" style="width: 92%" onchange="javascript:Internet_Remitter_Main.show_visa_type2(this.value, false);">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemIDType2">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                ID Classification:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemIDClassification2" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemIDClassification2">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 26px">
                                                                                                                &nbsp;Visa Type:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 26px">
                                                                                                                <select id="ddRemVisaType2" style="width: 92%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                            <%--<td id="tdRemVIPTypeC" class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                VIP Type:</td>
                                                                                                            <td id="tdRemVIPType" align="left" style="width: 240px; height: 24px; color: red;">
                                                                                                                <select id="ddRemVIPType" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>--%>
                                                                                                        </tr>
                                                                                                        <tr id = "trRemOthers">
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                OTHERS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr id="trRemRiskRating">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">Risk Rating:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemRiskRating" style="width: 92%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemRiskRating">*</label>
                                                                                                            </td>
                                                                                                            <td id="tdRemVIPTypeC" class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                VIP Type:</td>
                                                                                                            <td id="tdRemVIPType" align="left" style="width: 32; height: 24px; color: red;">
                                                                                                                <select id="ddRemVIPType" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>                                                                                                                    
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Referred By:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="txtRemReferredBy" type="text" style="width: 91%;" maxlength="100" /><label style="width: 5%;" id="astRemReferredBy">*</label>
                                                                                                            </td>
                                                                                                            <td id="tdRemOldIDCap" class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                Old Remitter ID:
                                                                                                                </td>
                                                                                                            <td id="tdRemOldID" align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemOldID" type="text" style="width: 72%;" maxlength="100" /><label style="width: 5%;" id="astRemOldID">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr id="trRemRemitterTypes"> <!--style="display:block"> -->
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">Web Remitter:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="chkRemWebRemitter" style="width: 14%" type="checkbox" onclick = "javascript:Internet_Remitter.showOverride('chkRemWebRemitter');"/>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                Phone Remitter:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="chkRemPhoneRemitter" style="width: 14%" type="checkbox" onclick = "javascript:Internet_Remitter.showOverride('chkRemPhoneRemitter');"/>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trGarnish">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px"> Garnish:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="chkRemGarnish" style="width: 14%" type="checkbox"/>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trChargeType">
                                                                                                           <td align="left" style="width: 18%; height: 24px">
                                                                                                                <input id="btnChargeType" style="height: 28px; width: 98%" type="button" value="Charge Type" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.get_Charge_type('txtRemChargeType');" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtRemChargeType" type="text" style="width: 95%;" maxlength="400" /></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr id="trFCRemCustID1">
                                                                                                            <td align="left" style="width: 18%; height: 23px">
                                                                                                                <input id="btnFCRemCustID1" style="height: 28px; width: 98%" type="button" value="FC Customer ID" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.insert_CIF_to_FC('rem', 'txtRemFCCustId1');" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtRemFCCustId1" type="text" style="width: 95%;" maxlength="400" /></td>
                                                                                                        </tr>                                                                                                                 
                                                                                                        <tr>
                                                                                                            <td style="width: 18%; height: 24px">
                                                                                                            </td>
                                                                                                            <td align="left" colspan="3" style="height: 24px" class="cell_desc">
                                                                                                                <div id="trSameAsRem" style="display:none" class="table_body">
                                                                                                                    <input id="chkSameAsRem" style="width: 6%" type="checkbox"/>&nbsp; 
                                                                                                                    Remitter and Beneficiary are the same
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                <div id="DIV_BUS_REM" style="width:100%; display:none" class="table_body">
                                                                                                    <table id="tblRemInfo2" style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Business Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtRemBusinessName" type="text" style="width: 95%;" maxlength="200" /><label id="astRemBusinessName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Short Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtRemShortName" type="text" style="width: 95%;" maxlength="80" /><label id="astRemShortName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Alias:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtRemBAlias" type="text" style="width: 95%;" maxlength="80" /><label id="astRemBAlias">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Business Type:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="color: red; height: 24px">
                                                                                                            <input id="txtRemBBusType" type="text" style="width: 95%;" maxlength="80" /><label id="astRemBBusType">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                HOME ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBAddress1" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBAddress1">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="txtRemBAddress2" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBAddress2">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBPOBox" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBPOBox">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                District:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><input id="txtRemBDistrict" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBDistrict">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                City:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBCity" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBCity">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                State:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><input id="txtRemBState" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBState">*</label></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Region:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBRegion" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBRegion">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Country:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><select id="ddRemBCountry" style="width: 88%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemBCountry">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Zip Code:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBZip" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBZip">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="chkRemBPrefMailAdd" style="width: 14%" type="checkbox"/><label id="astRemBPrefMailAdd">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr style="display:none">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Address Type:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddRemBAddressType" style="width: 92%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemBAddressType">*</label>
                                                                                                            </td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                COMPANY ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBAddress1_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBAddress1_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="txtRemBAddress2_Com" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBAddress2_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBPOBox_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBPOBox_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                District:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><input id="txtRemBDistrict_Com" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBDistrict_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                City:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBCity_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBCity_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                State:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><input id="txtRemBState_Com" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBState_Com">*</label></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Region:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBRegion_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBRegion_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Country:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><select id="ddRemBCountry_Com" style="width: 88%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemBCountry_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Zip Code:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBZip_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBZip_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="chkRemBPrefMailAdd_Com" style="width: 14%" type="checkbox"/><label id="astRemBPrefMailAdd_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                MAILING ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBAddress1_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBAddress1_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="txtRemBAddress2_Mail" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBAddress2_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBPOBox_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBPOBox_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                District:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><input id="txtRemBDistrict_Mail" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBDistrict_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                City:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBCity_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBCity_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                State:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><input id="txtRemBState_Mail" type="text" style="width: 87%;" maxlength="200" /><label id="astRemBState_Mail">*</label></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Region:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBRegion_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBRegion_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Country:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 240px; color: red; height: 24px"><select id="ddRemBCountry_Mail" style="width: 88%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astRemBCountry_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Zip Code:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBZip_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBZip_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="chkRemBPrefMailAdd_Mail" style="width: 14%" type="checkbox"/><label id="astRemBPrefMailAdd_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                CONTACTS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Telephone No:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBTelNo" type="text" style="width: 90%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astRemBTelNo">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Mobile No:</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color:Red">
                                                                                                                <input id="txtRemBMobileNo" type="text" style="width: 87%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astRemBMobileNo">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Email Address:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color:Red">
                                                                                                                <input id="txtRemBEmailAdd" type="text" style="width: 90%;" maxlength="200" /><label id="astRemBEmailAdd">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Preferred Contact:</td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px">
                                                                                                                <select id="ddBusRemPrefContact" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBusRemPrefContact">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                ID's&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 17px">
                                                                                                                ID No (1):</td>
                                                                                                            <td align="left" style="width: 35%; height: 17px; color: red;"><input id="txtRemBIDNo" type="text" style="width: 90%;" maxlength="50" /><label id="astRemBIDNo">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 17px">
                                                                                                                ID Expiry Date:<br />
                                                                                                                MM/DD/YYYY&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 17px">
                                                                                                            <input id="txtRemBIDExpDate" type="text" style="width: 79%;" maxlength="10" />
                                                                                                            <img alt="calendar" onclick="javascript:if(document.getElementById('txtRemBIDExpDate').disabled == false){popUpCalendar(this, document.getElementById('txtRemBIDExpDate'), 'mm/dd/yyyy', document.getElementById('divInternetRemitter'));}"
                                                                                                                src="../images/calendar/imgcalen.png" />                                                                                                                    
                                                                                                            <label id="astRemBIDExpDate">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                ID Type:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemBIDType" style="width: 92%" onchange="javascript:Internet_Remitter_Main.show_visa_type1(this.value, false);">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemBIDType">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                                ID Classification:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemBIDClassification" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemBIDClassification">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                &nbsp;Visa Type:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddRemBVisaType" style="width: 92%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select>
                                                                                                            </td>
                                                                                                            <td style="width: 18%; height: 23px">
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                ID No (2):</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;"><input id="txtRemBIDNo2" type="text" style="width: 90%;" maxlength="50" /><label id="astRemBIDNo2">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                ID Expiry Date:<br />
                                                                                                                MM/DD/YYYY&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                            <input id="txtRemBIDExpDate2" type="text" style="width: 79%;" maxlength="10" />
                                                                                                            <img id="calRemBIDExpDate2" align="absMiddle" onclick="javascript:if(document.getElementById('txtRemBIDExpDate2').disabled == false){popUpCalendar(this, document.getElementById('txtRemBIDExpDate2'), 'mm/dd/yyyy', document.getElementById('divInternetRemitter'));}"
                                                                                                                src="../images/calendar/imgcalen.png" />                                                                                                                    
                                                                                                            <label id="astRemBIDExpDate2">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                ID Type:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemBIDType2" style="width: 92%" onchange="javascript:Internet_Remitter_Main.show_visa_type2(this.value, false);">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemBIDType2">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                                ID Classification:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemBIDClassification2" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemBIDClassification2">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                &nbsp;Visa Type:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddRemBVisaType2" style="width: 92%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select>
                                                                                                            </td>
                                                                                                            <%--<td id="tdRemBVIPTypeC" class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                VIP Type:</td>
                                                                                                            <td id="tdRemBVIPType" align="left" style="width: 240px; height: 24px; color: red;">
                                                                                                                <select id="ddRemBVIPType" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>--%>                                                                                                         
                                                                                                        </tr>
                                                                                                        <tr id = "trRemBOthers">
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                OTHERS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr id="trRemBRiskRating">                                                                                                                    
                                                                                                            <td class="cell_caption"style="width: 18%; height: 23px">Risk Rating:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <select id="ddRemBRiskRating" style="width: 92%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astRemBRiskRating">*</label>
                                                                                                            </td>
                                                                                                            <td id="tdRemBVIPTypeC" class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                VIP Type:</td>
                                                                                                            <td id="tdRemBVIPType" align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <select id="ddRemBVIPType" style="width: 98%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>                                                                                                                    
                                                                                                            <td class="cell_caption"style="width: 18%; height: 23px">
                                                                                                                Referred By:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="txtRemBReferredBy" type="text" style="width: 91%;" maxlength="100" /><label id="astRemBReferredBy">*</label>
                                                                                                            </td>
                                                                                                            <td id="tdRemBOldIDCap" class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                Old Remitter ID:</td>
                                                                                                            <td id="tdRemBOldID" align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtRemBOldID" type="text" style="width: 87%;" maxlength="100" />
                                                                                                                <label style="width:5%" id="astRemBOldID">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trRemBRemitterTypes"> <!--style="display:block"> -->
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">Web Remitter:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px; text-align:left;">
                                                                                                                <input id="chkRemBWebRemitter" style="width:14%"  type="checkbox" onclick = "javascript:Internet_Remitter.showOverride('chkRemBWebRemitter');"/>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                Phone Remitter:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;; text-align:left">
                                                                                                                <input id="chkRemBPhoneRemitter" style="width:14%"  type="checkbox" onclick = "javascript:Internet_Remitter.showOverride('chkRemBPhoneRemitter');"/>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trRemBGarnish">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px"> Garnish:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="chkRemBGarnish" style="width: 14%" type="checkbox"/>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;">
                                                                                                                </td>
                                                                                                            <td align="left" style="width:  32%; height: 24px; color: red;">
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trChargeType2">
                                                                                                           <td align="left" style="width: 18%; height: 23px">
                                                                                                                <input id="btnChargeType2" style="height: 28px; width: 98%" type="button" value="Charge Type" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.get_Charge_type('txtRemBChargeType');" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 82%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtRemBChargeType" type="text" style="width: 95%;" maxlength="400" /></td>
                                                                                                        </tr>                                                                                                                 
                                                                                                        <tr id="trFCRemCustID2">
                                                                                                            <td align="left" style="width: 18%; height: 23px">
                                                                                                                <input id="btnFCRemCustID2" style="height: 28px; width: 98%" type="button" value="FC Customer ID" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.insert_CIF_to_FC('rem','txtRemFCCustId2');" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 82%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtRemFCCustId2" type="text" style="width: 95%;" maxlength="400" /></td>
                                                                                                        </tr>                                                                                                                 
                                                                                                        <tr>
                                                                                                            <td style="width: 18%; height: 24px">
                                                                                                            </td>
                                                                                                            <td align="left" colspan="3" style="width:82%; height: 24px" class="cell_desc">
                                                                                                                <div id="trSameAsRem2" class="table_body">
                                                                                                                    <input id="chkSameAsRem2" style="width: 6%" type="checkbox"/>&nbsp; Remitter and Beneficiary are the same
                                                                                                                </div>                                                                                                                        
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                <div id="divRemSaveCIFtoFCLoading" style="display:none">                                                                                                                                                                                                                                    
                                                                                                    <table style="width:100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 18%; height: 23px"><div id="div1" class="table_body"></div></td>
                                                                                                            <td align="left" style="width: 82%; height: 24px" class="cell_desc" colspan="2"><label id="Label3">Loading...</label></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>                                                                                                        
                                                                                            </td>
                                                                                        </tr>  
                                                                                        <tr>
                                                                                            <td style="width:100%; text-align:right;">
                                                                                                <div id="trRemBtns1" style="display:none" class="table_body">
                                                                                                    <input id="btnAddRem" style="height: 28px; width: 110px; display:none" type="button" value="Add Remitter" class="button"
                                                                                                    onclick="javascript:Internet_Remitter.add_remitter();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnEditRem" style="height: 28px; width: 110px" type="button" value="Edit" class="button"
                                                                                                    onclick="javascript:Internet_Remitter.edit_remitter();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnSaveRem" style="height: 28px; width: 110px" type="button" value="Save Remitter" class="button"
                                                                                                    onclick="javascript:Internet_Remitter.btn_save_remitter();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnCancelRem" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                                    onclick="javascript:Internet_Remitter.load_client_record();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                </div>
                                                                                                <div id="trRemBtns2" style="display:none" class="table_body">
                                                                                                    <input id="btnAddBene" style="height: 28px; width: 150px" type="button" value="Add Beneficiary" class="button"
                                                                                                    onclick="javascript:Internet_Remitter_Main.show_panel_add_bene(true);" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div><!--END OF divRemInfo-->
                                                                    
                                                                    <div id="divBeneInfo" style="width: 100%; height: 100%; margin: 0; display:none; visibility: hidden; text-align: left;" class="table_body">
                                                                        <table id="tblBeneInfo" cellpadding="0" cellspacing="0" style="width:100%; border: solid 1px #394397; visibility: visible;">
                                                                            <tr>
                                                                                <td style="width:100%">
                                                                                    <table style="width:98%; visibility: visible;" id="tblBeneInfo0" cellspacing="5">
                                                                                        <tr>
                                                                                            <td colspan="6">
                                                                                                <div id="trHeaderBene">
                                                                                                    <table style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;">
                                                                                                                Beneficiary Information</td>
                                                                                                            <td align="left" style="width: 15%; height: 24px; color: red;">
                                                                                                                <select id="ddCIFType2" style="width: 100%" onchange="javascript:Internet_Remitter_Main.show_panel_bene_ind_or_business(this.value);Internet_Beneficiary.set_branchdef_values();">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                <div id="trHeaderBeneClient">
                                                                                                    <table style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="2">
                                                                                                                Beneficiary Information</td>
                                                                                                            <td class="table_caption" colspan="1" style="width: 85%; color: white; text-indent: 5px;
                                                                                                                height: 26px; text-align: left">
                                                                                                                <input id="Button14" style="width: 139px" type="button" value="Mode of Remittance" onclick="javascript:Internet_Remitter_Main.show_panel_add_prod();" /></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>                                                                                           
                                                                                        <tr>
                                                                                            <td colspan="6" style="width: 100%; color: black; text-indent: 5px;
                                                                                                height: 26px; text-align: left">
                                                                                                <div id="DIV_IND_BEN" style="width:100" class="table_body">
                                                                                                    <table id="tblBeneInfo1" style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Last Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtBeneLName" type="text" style="width: 95%;" maxlength="200" /><label id="astBeneLName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>First Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtBeneFName" type="text" style="width: 95%;" maxlength="80" /><label id="astBeneFName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Middle Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtBeneMName" type="text" style="width: 95%;" maxlength="80" /><label id="astBeneMName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Birth Date (mm/dd/yyyy):&nbsp;<br />
                                                                                                                &nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBDate" type="text" style="width: 81%;"
                                                                                                                    onchange="javascript:Internet_Remitter.compute_age(2);" maxlength="10" />
                                                                                                                <img id="calBeneBDate" onclick="javascript:if(document.getElementById('txtBeneBDate').disabled == false){popUpCalendar(this, document.getElementById('txtBeneBDate'), 'mm/dd/yyyy', document.getElementById('divInternetRemitter'));}"
                                                                                                                    src="../images/calendar/imgcalen.png" />                                                                                                                               
                                                                                                                <label id="astBeneBDate">*</label></td>
                                                                                                            <td class="cell_caption" style="height: 24px; visibility: hidden; color: black; width: 173px;">
                                                                                                                Birth Place:&nbsp;</td>
                                                                                                            <td align="left" style="height: 24px; color:Red; visibility: hidden;">
                                                                                                                <input id="txtBeneBPlace" type="text" style="width: 87%; visibility: hidden;" maxlength="200" /><label id="astBeneBPlace">*</label></td>
                                                                                                        </tr>
                                                                                                        <%--<tr>
                                                                                                            <td class="comment" style="width: 18%; height: 24px; text-align:right">
                                                                                                                </td>
                                                                                                            <td class="comment" align="left" style="width: 32%; height: 24px;">
                                                                                                                &nbsp;* Address</td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="color: red; height: 24px"><label id="Label10"></label></td>
                                                                                                        </tr>--%>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                HOME ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Address 1:&nbsp;</td>
                                                                                                            <td align="left" colspan="2">
                                                                                                                <input id="txtBeneAddress1" type="text" style="width: 90%;" maxlength="50" /></td>
                                                                                                            <td align="left" style="visibility: hidden; width: 173px">
                                                                                                                <input id="txtBenePOBox" type="text" style="width: 90%;" maxlength="200" /></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Address 2:&nbsp;</td>
                                                                                                            <td colspan="2" style="width: 18%; height: 24px">
                                                                                                                
                                                                                                                <input id="txtBeneAddress2" type="text" style="width: 90%;" maxlength="50" /></td>
                                                                                                            <td style="width: 18%; height: 24px"></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" style="width: 18%; height: 23px">
                                                                                                                <input id="Button6" style="height: 28px; width: 98%" type="button" value="Zipcode" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.panel_search_zipcode_dom(2);" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtBeneZip" type="text" style="width: 95%;" maxlength="400" /><label id="astBeneZip">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr id="tr_benef_address_type">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px; display:none">
                                                                                                                Address Type:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px; display:none"><select id="ddBeneAddressType" style="width: 92%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astBeneAddressType">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 173px; height: 23px; visibility: hidden;">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="color: red; height: 24px;"><input id="chkBenePrefMailAdd" style="width: 14%; visibility: hidden;" type="checkbox" onclick="javascript:Internet_Beneficiary.onCheck_Preferred(1,'chkBenePrefMailAdd');"/><label id="astBenePrefMailAdd" style="visibility: hidden">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="height: 23px; display: none; visibility: hidden; width: 18%;" colspan="4">
                                                                                                                COMPANY ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px; display: none; visibility: hidden;">
                                                                                                                No/Street:&nbsp;</td>
                                                                                                            <td align="left" style="width: 18%; height: 23px; display: none; visibility: hidden;">
                                                                                                                <input id="txtBeneAddress2_Com" type="text" style="width: 90%;" maxlength="50" /><label id="astBeneAddress2_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 173px; height: 23px; display: none; visibility: hidden;">
                                                                                                                Room No. / Bldg.:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id='tr7' style="display:none">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px; display: none; visibility: hidden;">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 18%; height: 23px; display: none; visibility: hidden;">
                                                                                                                <input id="txtBenePOBox_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astBenePOBox_Com">*</label></td>
                                                                                                            <td style="display: none; visibility: hidden; width: 173px; height: 23px"></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr>
                                                                                                            <td align="left" style="width: 18%; height: 23px; display: none; visibility: hidden;">
                                                                                                                <input id="Button10" style="height: 28px; width: 98%" type="button" value="Zipcode" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.panel_search_zipcode_dom(1);" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 18%; height: 23px; display: none; visibility: hidden;" colspan="3">
                                                                                                                <input id="txtBeneZip_Com" type="text" style="width: 95%;" maxlength="400" /><label id="astBeneZip_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr id="tr8">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px; display:none; visibility: hidden;">
                                                                                                                &nbsp;</td>
                                                                                                            <td align="left" style="width: 18%; height: 23px; display:none; visibility: hidden;">
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 173px; height: 23px; display: none; visibility: hidden;">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                CONTACTS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Telephone No:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneTelNo" type="text" style="width: 90%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astBeneTelNo">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 173px; height: 23px">
                                                                                                                Mobile No:&nbsp;</td>
                                                                                                            <td align="left" style="height: 24px; color: red;">
                                                                                                                <input id="txtBeneMobileNo" type="text" style="width: 87%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astBeneMobileNo">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Email Address:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">                                                                                                                        
                                                                                                                <input id="txtBeneEmailAdd" type="text" style="width: 90%;" maxlength="200" /><label id="astBeneEmailAdd">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 173px; height: 23px">
                                                                                                                &nbsp;</td>
                                                                                                            <td align="left" style="height: 24px;" class="comment">
                                                                                                                <label id="comBeneMobileNo">* For Text Alert Service</label></td>
                                                                                                        </tr>                                                                                                                
                                                                                                        <tr style="display:none">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px; display:none">
                                                                                                                </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color:Red; display:none">
                                                                                                                </td>
                                                                                                            <td class="cell_caption" style="width: 173px; height: 23px; display:none">
                                                                                                                Preferred Contact:</td>
                                                                                                            <td align="left" style="color: red; height: 24px; display:none">
                                                                                                                <select id="ddBenePrefContact" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBenePrefContact">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="height: 24px; display: none; visibility: hidden; width: 18%;" colspan="4">
                                                                                                                RELATIONSHIP&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px; display: none; visibility: hidden;">
                                                                                                                Relationship:</td>
                                                                                                            <td align="left" style="width: 18%; height: 24px; display: none; visibility: hidden;">
                                                                                                                <select id="ddBeneRel" style="width: 92%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBeneRel">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 173px; height: 24px; display:none">
                                                                                                                Notification Mode:</td>
                                                                                                            <td align="left" style="color: red; height: 24px; display:none">
                                                                                                                <select id="ddBeneNotMode" style="width: 90%" onchange="javascript:Internet_Beneficiary.check_notif(1, this.selectedIndex);">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBeneNotMode">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="trModeOfRemittance" style="display:none">
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                MODE OF REMITTANCE&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="2">
                                                                                                                <div id="trProduct" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblBeneProduct">                                                                                                                        
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                                Mode of Remittance:</td>
                                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;"><select id="ddBProductType" style="width: 92%" onchange="javascript:Internet_Remitter_Main.show_acctno_field();">
                                                                                                                                <option selected="selected"></option>
                                                                                                                            </select><label id="Label31">*</label></td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                                <div id="trAcctNo" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblBeneAccountNo">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption"  style="width: 18%; height: 24px;">
                                                                                                                                Account No:</td>
                                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;">
                                                                                                                                <input id="txtBAccountNo" type="text" style="width: 90%;" maxlength="40" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" onkeyup="acct_check_gfmc(event);" /><label id="Label32">*</label></td>
                                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                                            </td>
                                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                                <div id="trGFCNo" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblGFCNo">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption"  style="width: 18%; height: 24px;">
                                                                                                                                GFC No:</td>
                                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;">
                                                                                                                                <input id="txtBGFCNo" type="text" style="width: 90%;" maxlength="40" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" onkeyup="acct_check_bgfmcno(event);"/><label id="Label6">*</label></td>
                                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                                            </td>
                                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                                <div id="trNoteAcctValidation" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblNoteAcctValidation">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption"  style="width: 18%; height: 24px;">
                                                                                                                                </td>
                                                                                                                            <td align="left" style="width: 35%; height: 24px;" class="comment"><label id="Label7">* Press Enter to Validate</label></td>
                                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                                            </td>
                                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                                <div id="trTarCurrency" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblProduct">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                                Target Currency:</td>
                                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><select id="ddBTargetCurr" style="width: 92%">
                                                                                                                                <option selected="selected"></option>
                                                                                                                            </select><label id="Label33">*</label></td>
                                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                                            </td>
                                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                                <div id="trOtherBankName" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblOtherBankName">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                                Other Bank Name:</td>
                                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><select id="ddBOtherBankName" style="width: 92%">
                                                                                                                                <option selected="selected"></option>
                                                                                                                            </select><label id="Label8">*</label></td>
                                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                                            </td>
                                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                                <div id="trOtherBankNameRural" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblOtherBankNameRural">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                                Other Bank Name:</td>
                                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="txtBRuralBankName" type="text" style="width: 90%;"/><label id="Label11">*</label></td>
                                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                                            </td>
                                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                                <div id="trIsRuralBank" style="display:none" class="table_body">
                                                                                                                    <table style="width:100%" id="tblIsRuralBank">
                                                                                                                        <tr>
                                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                                </td>
                                                                                                                            <td align="left" style="width: 35%; height: 24px" class="comment"><input id="chkIsRuralBankBene" style="width: 8%" type="checkbox" onclick="javascript:Internet_Beneficiary.IsRuralBank_OnClick();"/>
                                                                                                                                * Check if Bank is not in the list</td>
                                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                                            </td>
                                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="display:none">
                                                                                                            <td align="left" style="width: 18%; height: 23px;">
                                                                                                                <input id="btnFCBeneCustID1" style="height: 28px; width: 98%" type="button" value="FC Customer ID" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.insert_CIF_to_FC('bene','txtBeneFCCustId1');" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtBeneFCCustId1" type="text" style="width: 95%;" maxlength="400" /></td>
                                                                                                        </tr>                                                                                                                 
                                                                                                    </table>
                                                                                                </div>
                                                                                                <div id="DIV_BUS_BEN" style="width:100" class="table_body">
                                                                                                    <table id="tblBeneInfo2" style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Business Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtBeneBusinessName" type="text" style="width: 95%;" maxlength="200" /><label id="astBeneBusinessName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Short Name:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtBeneShortName" type="text" style="width: 95%;" maxlength="80" /><label id="astBeneShortName">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Alias:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="height: 24px; color: red;">
                                                                                                                <input id="txtBeneBAlias" type="text" style="width: 95%;" maxlength="80" /><label id="astBeneBAlias">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Business Type:&nbsp;</td>
                                                                                                            <td align="left" colspan="3" style="color: red; height: 24px">
                                                                                                                <input id="txtBeneBBusType" type="text" style="width: 95%;" maxlength="80" /><label id="astBeneBBusType">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                HOME ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBAddress1" type="text" style="width: 90%;" maxlength="50" /><label id="astBeneBAddress1">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="txtBeneBAddress2" type="text" style="width: 87%;" maxlength="50" /><label id="astBeneBAddress2">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBPOBox" type="text" style="width: 90%;" maxlength="200" /><label id="astBeneBPOBox">*</label></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                        </tr>                                                                                                                 
                                                                                                        <tr>
                                                                                                            <td align="left" style="width: 18%; height: 23px">
                                                                                                                <input id="Button7" style="height: 28px; width: 98%" type="button" value="Zipcode" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.panel_search_zipcode_dom(2);" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtBeneBZip" type="text" style="width: 95%;" maxlength="400" /><label id="astBeneBZip">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;display:none">
                                                                                                                Address Type:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px; display:none"><select id="ddBeneBAddressType" style="width: 92%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astBeneBAddressType">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px;"><input id="chkBeneBPrefMailAdd" style="width: 14%" type="checkbox" onclick="javascript:Internet_Beneficiary.onCheck_Preferred(2,'chkBeneBPrefMailAdd');"/><label id="astBeneBPrefMailAdd">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                COMPANY ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBAddress1_Com" type="text" style="width: 90%;" maxlength="50" /><label id="astBeneBAddress1_Com">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="txtBeneBAddress2_Com" type="text" style="width: 87%;" maxlength="50" /><label id="astBeneBAddress2_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBPOBox_Com" type="text" style="width: 90%;" maxlength="200" /><label id="astBeneBPOBox_Com">*</label></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                        </tr>                                                                                                                 
                                                                                                        <tr>
                                                                                                            <td align="left" style="width: 18%; height: 23px">
                                                                                                                <input id="Button12" style="height: 28px; width: 98%" type="button" value="Zipcode" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.panel_search_zipcode_dom(1);" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtBeneBZip_Com" type="text" style="width: 95%;" maxlength="400" /><label id="astBeneBZip_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;display:none">
                                                                                                                &nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px; display:none">
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px;"><input id="chkBeneBPrefMailAdd_Com" style="width: 14%" type="checkbox" onclick="javascript:Internet_Beneficiary.onCheck_Preferred(2,'chkBeneBPrefMailAdd_Com');"/><label id="astBeneBPrefMailAdd_Com">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                MAILING ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Room No. / Bldg.:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBAddress1_Mail" type="text" style="width: 90%;" maxlength="50" /><label id="astBeneBAddress1_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                No/Street:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><input id="txtBeneBAddress2_Mail" type="text" style="width: 87%;" maxlength="50" /><label id="astBeneBAddress2_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBPOBox_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astBeneBPOBox_Mail">*</label></td>
                                                                                                            <td></td>
                                                                                                            <td></td>
                                                                                                        </tr>                                                                                                                 
                                                                                                        <tr>
                                                                                                            <td align="left" style="width: 18%; height: 23px">
                                                                                                                <input id="Button13" style="height: 28px; width: 98%" type="button" value="Zipcode" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.panel_search_zipcode_dom(3);" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                                                <input id="txtBeneBZip_Mail" type="text" style="width: 95%;" maxlength="400" /><label id="astBeneBZip_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;display:none">
                                                                                                                </td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px;display:none">
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px;"><input id="chkBeneBPrefMailAdd_Mail" style="width: 14%" type="checkbox" onclick="javascript:Internet_Beneficiary.onCheck_Preferred(2,'chkBeneBPrefMailAdd_Mail');"/><label id="astBeneBPrefMailAdd_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                CONTACTS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Telephone No:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBTelNo" type="text" style="width: 90%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astBeneBTelNo">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Mobile No:&nbsp;</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;">
                                                                                                                <input id="txtBeneBMobileNo" type="text" style="width: 87%;" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" maxlength="25" /><label id="astBeneBMobileNo">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                Email Address:</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red;">                                                                                                                        
                                                                                                                <input id="txtBeneBEmailAdd" type="text" style="width: 90%;" maxlength="200" /><label id="astBeneBEmailAdd">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px">
                                                                                                                &nbsp;</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px;" class="comment">
                                                                                                                <label id="comBeneBMobileNo">* For Text Alert Service</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px; display:none">
                                                                                                                </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color:Red; display:none">
                                                                                                                <label></label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 23px;display:none">
                                                                                                                Preferred Contact:</td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px;display:none">
                                                                                                                <select id="ddBusBenePrefContact" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBusBenePrefContact">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                RELATIONSHIP&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Relationship:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px"><select id="ddBeneBRel" style="width: 92%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astBeneBRel">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;display:none">
                                                                                                                Notification Mode:</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color:Red;display:none"><select id="ddBeneBNotMode" style="width: 92%" onchange="javascript:Internet_Beneficiary.check_notif(2, this.selectedIndex);">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="astBeneBNotMode">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="tr2" style="display:none">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Mode of Remittance:</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;"><select id="Select2" style="width: 92%" onchange="javascript:Internet_Remitter_Main.show_acctno_field();">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="Label45">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr id="tr3" style="display: none">
                                                                                                            <td class="cell_caption"  style="width: 18%; height: 24px;">
                                                                                                                Account No:</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;"><input id="Text12" type="text" style="width: 90%;" maxlength="40" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));"/><label id="Label46">*</label></td>
                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                            </td>
                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr id="tr4" style="display: none">
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Target Currency:</td>
                                                                                                            <td align="left" style="width: 35%; color: red; height: 24px"><select id="Select3" style="width: 92%">
                                                                                                                <option selected="selected"></option>
                                                                                                            </select><label id="Label47">*</label></td>
                                                                                                            <td class="cell_caption" style="display: none; width: 18%; height: 24px">
                                                                                                            </td>
                                                                                                            <td align="left" style="display: none; width: 35%; height: 24px">
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" style="width: 18%; height: 24px;display:none">
                                                                                                                <input id="btnFCBeneCustID2" style="height: 28px; width: 98%" type="button" value="FC Customer ID" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.insert_CIF_to_FC('bene','txtBeneFCCustId2');" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color: red;display:none" colspan="3">
                                                                                                                <input id="txtBeneFCCustId2" type="text" style="width: 95%;" maxlength="400" /></td>
                                                                                                        </tr>                                                                                                                
                                                                                                    </table>
                                                                                                </div>
                                                                                                
                                                                                                <div id="divSearchBeneficiary" style="text-align:center; z-index:101;left:675px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: tahoma; top: 1144px;"><!--top: 18px;-->
                                                                                                    <table cellpadding="0" cellspacing="0" style="width:300px">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table cellspacing=0 cellpadding=0>
                                                                                                                <tr>
                                                                                                                    <td align="left" valign="bottom" style="vertical-align:bottom; width:0%; border-collapse:collapse;height:24px;"><img src="../images/reskin/toe.GIF" /></td>
                                                                                                                    <td class="table_head_dark" style="width:50%;height:24px;">
                                                                                                                        Search Beneficiary</td>
                                                                                                                    <td align="left" style="vertical-align:bottom; width:100%;height:24px;" valign="bottom"><img align="absBottom" src="../images/reskin/rend.GIF" /></td>
                                                                                                                </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid">
                                                                                                                    <tr>
                                                                                                                        <td style="width:250px;" class="cell_caption">
                                                                                                                            Search By
                                                                                                                        </td>
                                                                                                                        <td style="width:450px;" align="left">
                                                                                                                            <select id="ddSearchBeneByCIFType2" class="ddl" style="width:99%" onchange="javascript:Internet_Remitter_Main.change_searchbybene_label();" onkeyup="Internet_Remitter_Main.keyup(this.id, event);"></select>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width:250px;" class="cell_caption">
                                                                                                                            <span id="spnSearchBeneLN2" style="font-size: 12px">Last Name</span>
                                                                                                                        </td>
                                                                                                                        <td style="width:450px;" align="left">
                                                                                                                            <input id="txtSearchBeneByLN2" style="width:97%;text-align:left;" type="text" onkeyup="Internet_Remitter_Main.keyup(this.id, event);" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width:250px;" class="cell_caption">
                                                                                                                            <span id="spnSearchBeneFN2" style="font-size: 12px">First Name</span>
                                                                                                                        </td>
                                                                                                                        <td style="width:450px;" align="left">
                                                                                                                            <input id="txtSearchBeneByFN2" style="width:97%;text-align:left;" type="text" onkeyup="Internet_Remitter_Main.keyup(this.id, event);" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="width:250px;" class="cell_caption">
                                                                                                                            <span id="spnSearchBeneMN2" style="font-size: 12px">Middle Name</span>
                                                                                                                        </td>
                                                                                                                        <td style="width:450px;" align="left">
                                                                                                                            <input id="txtSearchBeneByMN2" style="width:97%;text-align:left;" type="text" onkeyup="Internet_Remitter_Main.keyup(this.id, event);" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td style="height: 28px; width:700px;text-align:center;vertical-align:top;" valign="top" colspan = "2" align="center">
                                                                                                                            <input type="button" value="Search" onclick="javascript:Internet_Remitter_Main.search_bene2();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnSearchBenef"  style="width: 100px"/>
                                                                                                                            <input type="button" value="Cancel" onclick="javascript:hidePanel('divSearchBeneficiary');" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="Button3"  style="width: 100px"/>
                                                                                                                        </td>
                                                                                                                   </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                                

                                                                                                
                                                                                                <div id="divBeneSaveCIFtoFCLoading" style="display:none">                                                                                                                                                                                                                                    
                                                                                                    <table style="width:100%">
                                                                                                        <tr>
                                                                                                            <td style="width: 19%; height: 23px"><div id="div2" class="table_body"></div></td>
                                                                                                            <td style="width: 71%; height: 24px; text-align:left;" class="cell_desc" colspan="2"><label id="lbLoadingText">Loading...</label></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>                                                                                                         
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="7" style="text-align:right; width: 774px;">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="7" style="visibility: hidden; width: 774px; text-align: right; display: none;">
                                                                                                <div style="width: 556px; height: 286px">
                                                                                                    <table id="Table4" style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="color: red; height: 24px; visibility: hidden; width: 32%;" colspan="4">
                                                                                                                MAILING ADDRESS&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 32%; height: 24px; visibility: hidden; color: red;">
                                                                                                                No/Street:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red; visibility: hidden;">
                                                                                                                <input id="txtBeneAddress2_Mail" type="text" style="width: 90%;" maxlength="50" /><label id="astBeneAddress2_Mail">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 32%; height: 24px; visibility: hidden; color: red;">
                                                                                                                Room No. / Bldg.:<em><span style="font-size: 10pt; color: #ff0000"> </span></em>
                                                                                                            </td>
                                                                                                            <td align="left" style="color: red; height: 24px"><input id="txtBeneAddress1_Mail" type="text" style="width: 87%;" maxlength="50" /><label id="astBeneAddress1_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 32%; height: 24px; visibility: hidden; color: red;">
                                                                                                                PO Box:&nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red; visibility: hidden;">
                                                                                                                <input id="txtBenePOBox_Mail" type="text" style="width: 90%;" maxlength="200" /><label id="astBenePOBox_Mail">*</label></td>
                                                                                                            <td style="visibility: hidden; width: 32%; color: red; height: 24px"><input id="txtBeneAddress1_hidden" type="text" style="width: 24%;" maxlength="50" /><label id="astBeneAddress1">*Room
                                                                                                                No. / Bldg.:</label></td>
                                                                                                            <td></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" style="width: 32%; height: 24px; visibility: hidden; color: red;">
                                                                                                                <input id="Button11" style="height: 28px; width: 98%" type="button" value="Zipcode" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Main.panel_search_zipcode_dom(3);" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 32%; height: 24px; color: red; visibility: hidden;" colspan="3">
                                                                                                                <input id="txtBeneZip_Mail" type="text" style="width: 95%;" maxlength="400" /><label id="astBeneZip_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 32%; height: 24px; visibility: hidden; color: red;">
                                                                                                                &nbsp;</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px; visibility: hidden;">
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 32%; height: 24px; visibility: hidden; color: red;">
                                                                                                                Preferred Address:
                                                                                                            </td>
                                                                                                            <td align="left" style="color: red; height: 24px;"><input id="chkBenePrefMailAdd_Mail" style="width: 14%" type="checkbox" onclick="javascript:Internet_Beneficiary.onCheck_Preferred(1,'chkBenePrefMailAdd_Mail');"/><label id="astBenePrefMailAdd_Mail">*</label></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                    <input id="Text1" type="text" style="width: 87%;" maxlength="50" /><input id="Checkbox1" style="width: 14%" type="checkbox" onclick="javascript:Internet_Beneficiary.onCheck_Preferred(1,'chkBenePrefMailAdd_Com');"/><br />
                                                                                                    <table id="Table5" style="width:100%">
                                                                                                        <tr>
                                                                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                                                OCCUPATION&nbsp;</td>                                                                                                            
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Occupation:</td>
                                                                                                            <td align="left" style="width: 35%; height: 24px; color:Red">
                                                                                                                <select id="ddBeneOccupation" style="width: 92%" onchange="javascript:Internet_Beneficiary.BeneOccupationChanged();">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBeneOccupation">*</label></td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Company Name:&nbsp;</td>
                                                                                                            <td align="left" style="height: 24px; color:Red">
                                                                                                                <input id="txtBeneCompany" type="text" style="width: 90%;
                                                                                                                   " maxlength="200" /><label id="astBeneCompany">*</label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Detailed Description:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="txtBeneDetDesc" type="text" style="width: 90%;" maxlength="200" /><label id="astBeneDetDesc">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px;display:none">
                                                                                                                Worker Type:</td>
                                                                                                            <td align="left" style="color: red; height: 24px;display:none">
                                                                                                                <select id="ddBeneWorkerType" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBeneWorkerType">*</label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Annual Salary:</td>
                                                                                                            <td align="left" style="width: 32%; color: red; height: 24px">
                                                                                                                <input id="txtBeneAnnualSal" type="text" style="width: 90%;" maxlength="200" onkeypress="javascript:return(generic.autoFormatCurrency(this,',','.',event));"/><label id="astBeneAnnualSal">*</label>
                                                                                                            </td>
                                                                                                            <td class="cell_caption" style="width: 18%; height: 24px">
                                                                                                                Currency:</td>
                                                                                                            <td align="left" style="color: red; height: 24px">
                                                                                                                <select id="ddBeneSalCurr" style="width: 90%">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select><label id="astBeneSalCurr">*</label>
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
                                                                    </div><!--END OF divBeneInfo-->                                                                           
                                                                                                <div id="trBeneBtns1" class="table_body" style="text-align: left">
                                                                                                    &nbsp;<input id="btnAddBene1" style="height: 28px; width: 110px" type="button" value="Adds" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.btn_add_bene_cif();Internet_Remitter_Main.show_asterisks(2, ddCIFType2.value);" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnEditBene1" style="height: 28px; width: 110px; display: none; visibility: hidden;" type="button" value="Edit" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.edit_bene();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnSaveBene1" style="height: 28px; width: 115px" type="button" value="Save Beneficiary" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.btn_save_beneficiary();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnCancelBene1" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.load_benecif_record(2);Internet_Remitter_Main.disable_cif_type2(true);" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                </div>
                                                                                                <div id="trBeneBtns2" class="table_body" style="text-align: left">
                                                                                                    <input id="btnAddBene2" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.btn_add_bene_req();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnEditBene2" style="height: 28px; width: 110px; display: none; visibility: hidden;" type="button" value="Edit" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.edit_bene_2();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnSenReq" style="height: 28px; width: 115px; visibility: hidden;" type="button" value="Save Beneficiary" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.btn_send_req();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                    <input id="btnCancelBene2" style="height: 28px; width: 115px; visibility: hidden;" type="button" value="Cancel" class="button"
                                                                                                    onclick="javascript:Internet_Beneficiary.load_record();" onmouseover="this.className='button_over';"
                                                                                                    onmouseout="this.className='button';" />
                                                                                                </div>
                                                                    <br />
                                                                    <input id="txtBeneAddress1_Com" type="text" style="width: 87%; display: none; visibility: hidden;" maxlength="50" /><input id="chkBenePrefMailAdd_Com" style="width: 87%; display: none; visibility: hidden;" type="checkbox" onclick="javascript:Internet_Beneficiary.onCheck_Preferred(1,'chkBenePrefMailAdd_Com');"/></td>
                                                            </tr>
                                                            
                                                            
                                                            <tr>
                                                                <td>
                                                                    <div id="divProductInfo" style="width: 100%; height: 100%; margin: 0; display:none;" class="table_body">
                                                                        <table id="tblProductInfo" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td id="tdCapAddProducts" class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="4">
                                                                                    Add Products</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                    Beneficiary Name:&nbsp;</td>
                                                                                <td align="left" style="width: 45%; height: 24px; color:Red"><select onchange="javascript:Internet_Remitter_Products.show_fields();" id="ddBeneficiaryName" style="width: 225px">
                                                                                    <option selected="selected"></option></select>*</td>
                                                                                <td align="left" style="width: 35%; height: 24px; display:none">
                                                                                     <input id="btnRemit" type="button" value="Remit" style="width:100px;" onclick="javascript:Internet_Remitter_Products.remit();"/>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trProductType">
                                                                                <td id="tdProductType" class="cell_caption" style="width: 27%; height: 24px">
                                                                                    Product Type:&nbsp;</td>
                                                                                <td align="left" style="width: 45%; height: 24px; color:Red">
                                                                                    <select onchange="javascript:Internet_Remitter_Products.Productype_Onchange();" id="ddProductType" style="width: 225px">
                                                                                        <option selected="selected"></option></select>*</td>
                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 18%; height: 24px" colspan="3">
                                                                                    <div id="divAccountNo" class="table_body">
                                                                                        <table style="width:100%" id="tblAccountNo">
                                                                                            <tr>
                                                                                                <td id="tdAccountNo" class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Account No:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:Red"><input id="txtAccountNo" type="text" style="width: 220px;" maxlength="40" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" onkeyup="Internet_Remitter_Products.check_digit_acctno(event);" />*</td>
                                                                                                <td align="left" style="width: 35%; height: 24px;">
                                                                                                    <div id="divValidateAcct" class="table_body">
                                                                                                        <label class="comment">* Press enter to validate</label>    
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divLoadingx" style="display:none" class="table_body">
                                                                                        <table style="width:100%">
                                                                                            <tr>
                                                                                                <td style="width: 26%; height: 23px"><div id="divLoading3" class="table_body"></div></td>
                                                                                                <td align="left" style="width: 71%; height: 24px" class="cell_desc" colspan="2"><label id="lblLoad3">Loading...</label></td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divTarCurr" class="table_body">
                                                                                        <table style="width:100%" id="tblTarCurr">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Target Currency:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:red"><select id="ddTargetCurrency" style="width: 223px" onchange="validate_currency_per_product();">
                                                                                                    <option selected="selected"></option>
                                                                                                </select>*
                                                                                                </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divOthBankName" class="table_body">
                                                                                        <table style="width:100%" id="tblOthBankName">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 23px">
                                                                                                    Other Bank Name:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:red"><select id="ddOtherBankName" style="width: 223px">
                                                                                                    <option selected="selected"></option>
                                                                                                </select>*
                                                                                                    </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divOthBankNamePDDTS" class="table_body">
                                                                                        <table style="width:100%" id="tblOthBankNamePDDTS">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 23px">
                                                                                                    Other Bank Name:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:red"><select id="ddOtherBankNamePDDTS" style="width: 223px">
                                                                                                    <option selected="selected"></option>
                                                                                                </select>*
                                                                                                    </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                                                     
                                                                                    <div id="divBranchCode" class="table_body">
                                                                                        <table style="width:100%" id="tblBranchCode">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Branch Code:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:Red"><select id="ddBranchCode" style="width: 223px">
                                                                                                    <option selected="selected"></option>
                                                                                                </select>
                                                                                                </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divAccountType" class="table_body">
                                                                                        <table style="width:100%" id="tblAccountType">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Account Type:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:Red"><select id="ddAccountType" style="width: 223px">
                                                                                                    <option selected="selected"></option>
                                                                                                </select>
                                                                                                </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divAccountCurr" class="table_body">
                                                                                        <table style="width:100%" id="tblAccountCurr">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Account Currency:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:red"><select id="ddAccountCurrency" style="width: 223px">
                                                                                                    <option selected="selected"></option>
                                                                                                </select>
                                                                                                </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divAccountStatus" class="table_body">
                                                                                        <table style="width:100%" id="tblAccountStatus">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Account Status:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color:red"><select id="ddAccountStatus" style="width: 223px;">
                                                                                                    <option selected="selected"></option>
                                                                                                </select>
                                                                                                </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                                                            
                                                                                    <div id="divCommentOthers" class="table_body">
                                                                                        <table style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" colspan="3" style="height: 24px" class="comment">
                                                                                                    * Please enter Bank Name and Bank Address below if Bank is not in the selection. 
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divSearchOtherBank1" class="table_body">
                                                                                        <table style="width:100%" id="tblSearchOtherBank1">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Bank Details:&nbsp;</td>
                                                                                                <td align="left" colspan="2" style="color: red; height: 24px">
                                                                                                    <input id="txtMT103BankName" type="text" style="width: 219px; text-align: left;"/>
                                                                                                    <input id="Button5" onclick="javascript:Internet_Remitter_Main.panel_search_other_banks();"
                                                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                                        style="width: 100px" type="button" class = "button" value="Search" />*
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divOtherBank1" class="table_body">
                                                                                        <table style="width:100%" id="tblOtherBank1">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Bank Name:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px; color: red;">
                                                                                                    <input id="txtOthBankName" type="text" style="width: 219px; text-align: left;"/>*
                                                                                                </td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divOtherBank2" class="table_body">
                                                                                        <table style="width:100%" id="tblOtherBank2">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 24px">
                                                                                                    Bank Address:&nbsp;</td>
                                                                                                <td align="left" style="width: 42%; height: 24px">
                                                                                                    <input id="txtOthBankAddress" type="text" style="width: 219px; text-align: left;"/></td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divIsRuralBank" class="table_body">
                                                                                        <table style="width:100%" id="Table3">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 27%; height: 23px">
                                                                                                    &nbsp;</td>
                                                                                                <td align="left" class="comment">
                                                                                                    &nbsp;<input id="chkIsRuralBank" style="width: 8%" type="checkbox" onclick="javascript:Internet_Remitter_Products.IsRuralBank_OnClick();"/>
                                                                                                    * Please check if Bank is not in the list</td>
                                                                                                <td align="left" style="width: 35%; height: 24px">
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                                                             
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align:right;" colspan="4">
                                                                                    <div id="trProdBtns1" class="table_body">
                                                                                        <input id="btnAddProd1" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                        onclick="Internet_Remitter_Products.btn_add_prod();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnEditProd1" style="height: 28px; width: 110px" type="button" value="Edit" class="button"
                                                                                        onclick="Internet_Remitter_Products.btn_edit_prod();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnSaveProd1" style="height: 28px; width: 110px" type="button" value="Save Product" class="button"
                                                                                        onclick="Internet_Remitter_Products.btn_save_prod()" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnCancelProd1" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                        onclick="Internet_Remitter_Products.load_record()" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                         </table>
                                                                         
                                                                     </div>
                                                                </td>
                                                            </tr> 
                                                            
                                                            <tr>
                                                                <td>
                                                                    <div id="divInternetRemitterAddCards" style="width: 100%; height: 100%; margin: 0; display:none;" class="table_body">
                                                                        <table id="tblAddCards" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">
                                                                                    Add Cards</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Card No:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color: red;"><input id="txtCardNo" type="text" style="width: 90%; height: 100%; text-align: left;" maxlength="16"/><label id="astCardNo">*</label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Card Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color: red;"><select id="ddCardType" style="width: 90%">
                                                                                </select><label id="astCardType">*</label></td>                                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Card Name:&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color: red;"><input id="txtCardName" type="text" style="width: 90%; height: 100%; text-align: left;"/><label id="astCardName">*</label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Card Class:</td>
                                                                                <td align="left" style="width: 25%; color: red;"><select id="ddCardClass" style="width: 90%">
                                                                                </select><label id="astCardClass">*</label></td>                                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">Issue Date (mm/yyyy):&nbsp;</td>
                                                                                <td align="left" colspan="2" style="color: red;">
                                                                                    <input id="txtIssueDate" type="text" style="width: 85%; height: 100%; text-align: left;" maxlength="7"/>
                                                                                    <img id="calIssueDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtIssueDate').disabled == false){popUpCalendar(this, document.getElementById('txtIssueDate'), 'mm/yyyy');}"
                                                                                        src="../images/calendar/imgcalen.png" />                                                                                               
                                                                                    <label id="astIssueDate">*</label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">Expiry Date (mm/yyyy):&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color: red;">
                                                                                    <input id="txtExpiryDate" type="text" style="width: 82%; height: 100%; text-align: left;" maxlength="7"/>
                                                                                    <img id="calExpiryDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtExpiryDate').disabled == false){popUpCalendar(this, document.getElementById('txtExpiryDate'), 'mm/yyyy');}"
                                                                                        src="../images/calendar/imgcalen.png" />
                                                                                    <label id="astExpiryDate">*</label></td>                                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 25px">Issuing Bank:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height: 25px; color: red;"><input id="txtBankName" type="text" style="width: 94%; text-align: left;" maxlength="7"/><label id="astBankName">*</label></td>
                                                                                <td align="left" style="width: 8%; height: 25px; font-size: 12pt; font-family: Times New Roman;">&nbsp;<input id="btnSearchBank" class="button" onclick="javascript:Internet_Remitter_Main.search_card_bank();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 60px" /></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 25px;">
                                                                                    Currency:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height: 25px; color:Red"><select id="ddCardsCurrency" style="width: 90%"></select><label id="astCardsCurrency">*</label></td>
                                                                            </tr>
                                                                            <tr id="trOthBankName" style="display:none">
                                                                                <td class="cell_caption" style="width: 10%; height: 25px">
                                                                                    Issuing Bank Name:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height: 25px; color: red;"><input id="txtOthBankName1" type="text" style="width: 94%; text-align: left;" maxlength="100"/>*<label id="Label12"></label></td>
                                                                                <td align="left" style="width: 8%; height: 25px; font-size: 12pt; font-family: Times New Roman;">&nbsp;</td>
                                                                                <td class="cell_caption" style="width: 10%; height: 25px;">
                                                                                    &nbsp;</td>
                                                                                <td align="left" style="width: 25%; height: 25px; color:Red"><label id="Label13"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 25px">
                                                                                    Issue Number:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height: 25px; color:Red"><input id="txtIssueNumber" type="text" style="width: 94%; text-align: left;" maxlength="2"/><label id="astIssueNumber">*</label></td>
                                                                                <td align="left" style="width: 8%; height: 25px;" class="comment">&nbsp;* ( Put 0 if NA )</td>
                                                                                <td class="cell_caption" style="width: 10%; height: 25px;">
                                                                                    &nbsp;</td>
                                                                                <td align="left" style="width: 25%; height: 25px; color:Red"><label id="Label15"></label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="5" style="text-align:right;">
                                                                                    <div class="table_body" id="trRemCardBtns1">
                                                                                        <input id="btnAddCard1" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                        onclick="Internet_Remitter_Cards.btn_add_card();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnEditCard1" style="height: 28px; width: 110px" type="button" value="Edit" class="button"
                                                                                        onclick="Internet_Remitter_Cards.btn_edit_card();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnSaveCard1" style="height: 28px; width: 110px" type="button" value="Save Card" class="button"
                                                                                        onclick="Internet_Remitter_Cards.btn_save_card();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnCancelCard1" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                        onclick="Internet_Remitter_Cards.btn_cancel_card_req();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                    </div>
                                                                                    <div class="table_body" id="trRemCardBtns2">
                                                                                        <input id="btnAddCard2" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                        onclick="Internet_Remitter_Cards.btn_add_card_req();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnSaveCard2" style="height: 28px; width: 110px" type="button" value="Send Request" class="button"
                                                                                        onclick="javascript:Internet_Remitter_Cards.btn_save();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                        <input id="btnCancelCard2" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                        onclick="javascript:Internet_Remitter_Cards.load_record();" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                         </table>
                                                                     </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div id="divVanInfo" style="width: 100%; height: 100%; margin: 0; display:none;" class="table_body">
                                                                        <table id="tblVanInfo" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">
                                                                                    Add Virtual Account Number</td>                                                                                                                                                                                            
                                                                            </tr>                                                                                    
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    <INPUT style="WIDTH: 98%; HEIGHT: 28px" id="Button1" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';" onclick="javascript:Internet_Remitter_Main.panel_search_van_products();" type=button value="Product" /></td>
                                                                                <td align="left" style="color: red; width: 34%;" colspan="3"><input id="txtRemVanProduct" type="text" style="width: 96%; height: 100%; text-align: left;" maxlength="16"/><label id="astRemVanProduct">*
                                                                                    &nbsp;
                                                                                </label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Virtual Acct. No.:&nbsp;</td>
                                                                                <td align="left" style="color: red; width: 34%;"><input id="txtRemVan" type="text" style="width: 89%; height: 100%; text-align: left;" maxlength="16"/><label id="astRemVan">*</label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    VAN Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color: red;"><select id="ddRemVanType" style="width: 90%">
                                                                                </select><label id="astRemVanType">*</label></td>                                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="4" style="text-align:right;">
                                                                                    <input id="btnAddVan" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                    onclick="Internet_Remitter_Van.btn_add_van();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnEditVan" style="height: 28px; width: 110px" type="button" value="Edit" class="button"
                                                                                    onclick="Internet_Remitter_Van.btn_edit_van();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnSaveVan" style="height: 28px; width: 110px" type="button" value="Save VAN" class="button"
                                                                                    onclick="Internet_Remitter_Van.btn_save_van();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnCancelVan" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                    onclick="Internet_Remitter_Van.btn_cancel_van();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                </td>
                                                                            </tr>
                                                                         </table>
                                                                     </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div id="divNotesInfo" style="width: 100%; height: 100%; margin: 0; display:none;" class="table_body">
                                                                        <table id="tblNotesInfo" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">
                                                                                    Add Client Notes</td>                                                                                                                                                                                            
                                                                            </tr>                                                                                    
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Note:&nbsp;</td>
                                                                                <td align="left" style="color: red; width: 34%;" colspan="3"><input id="txtRemNotes" type="text" style="width: 96%; height: 100%; text-align: left;"/><label id="Label4">*
                                                                                    &nbsp;
                                                                                </label></td>
                                                                            </tr>
                                                                            <tr id="trNoteTellerDate">
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Teller Name:&nbsp;</td>
                                                                                <td align="left" style="color: red; width: 34%;"><input id="txtRemNotesTeller" type="text" style="width: 89%; height: 100%; text-align: left;" /><label id="Label9"></label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 23px">
                                                                                    Date Added:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color: red;"><input id="txtRemNotesDate" type="text" style="width: 89%; text-align: left;" /><label id="Label10"></label></td>                                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="4" style="text-align:right;">
                                                                                    <input id="btnAddNotes" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                    onclick="Internet_Remitter_Notes.btn_add_notes();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <%--<input id="Button16" style="height: 28px; width: 110px" type="button" value="Edit" class="button"
                                                                                    onclick="Internet_Remitter_Van.btn_edit_van();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />--%>
                                                                                    <input id="btnSaveNotes" style="height: 28px; width: 110px" type="button" value="Save Note" class="button"
                                                                                    onclick="Internet_Remitter_Notes.btn_save_notes();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnCancelNotes" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                    onclick="Internet_Remitter_Notes.btn_cancel_notes();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                </td>
                                                                            </tr>
                                                                         </table>
                                                                     </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div id="divAccountInfo" style="width: 100%; height: 100%; margin: 0; display:none;" class="table_body">
                                                                        <table id="tblAccountInfo" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="8">
                                                                                    Add Accounts</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                    Account No.:&nbsp;</td>
                                                                                <td align="left" style="color: red; width: 482px;"><input id="txtRemAcctNo" type="text" style="width: 92%; height: 100%; text-align: left;" maxlength="40" onkeypress="javascript:return(generic.isNumberKey(this,',','.',event));" onkeyup="Internet_Accounts.check_digit_acctno(this.value, event);"/><label id="astRemAcctNo">*</label></td>
                                                                                <td id="tdAcctPressEnterComment"class="comment">Press enter</td>
                                                                                <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                    Account Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color: red;"><select id="ddRemAcctType" style="width: 90%">
                                                                                </select><label id="astRemAcctType">*</label></td>                                                                                                    
                                                                            </tr>
                                                                            <tr id="trLoading1" style="display:none">
                                                                                <td style="width: 14%; height: 23px"><div id="divLoading1"></div>
                                                                                </td>
                                                                                <td colspan="2" class="cell_desc"><label id="lblLoad1">Loading...</label></td>
                                                                                <td style="width: 14%; height: 23px">
                                                                                </td>
                                                                                <td align="left" style="width: 25%; color: red">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 25px;">
                                                                                    PNB Branch:&nbsp;</td>
                                                                                <td align="left" style="width: 482px; height: 25px; color: red;"><input id="txtRemPNBBranch" type="text" style="width: 92%; text-align: left;"/><label id="astRemPNBBranch">*</label></td>
                                                                                <td align="left" style="width: 11%; height: 25px; font-size: 12pt; font-family: Times New Roman;"><input id="btnSearchPNBBranch" class="button" onclick="javascript:Internet_Remitter_Main.search_pnb_branch();"
                                                                                onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 90%" /></td>
                                                                                <td class="cell_caption" style="width: 14%; height: 25px;">
                                                                                    Currency:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height: 25px; color:Red"><select id="ddRemAcctCurr" style="width: 90%"></select><label id="astRemAcctCurr">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 14%; height: 23px" class="cell_caption">
                                                                                    Other Bank Type:&nbsp;</td>
                                                                                <td align="left" style="color: red;"><select id="ddOtherBankType" style="width: 94%" onchange = "javascript:Internet_Accounts.ddOtherBankType_OnChange();">
                                                                                </select>
                                                                                </td>
                                                                                <td align="left" style="font-size: 12pt; width: 11%; font-family: Times New Roman;
                                                                                    height: 25px">
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                    Status:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color: red;"><select id="ddRemAcctStat" style="width: 90%"></select><label id="astRemAcctStat">*</label></td>                                                                                                    
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 14%; height: 25px">
                                                                                    Other Banks:&nbsp;</td>
                                                                                <td align="left" style="width: 482px; color: red; height: 25px"><input id="txtRemOthBank" type="text" style="width: 92%; text-align: left;"/><label id="astRemOthBank">*</label></td>
                                                                                <td align="left" style="font-size: 12pt; width: 11%; font-family: Times New Roman;
                                                                                    height: 25px"><input id="Button8" class="button" onclick="javascript:Internet_Remitter_Main.search_oth_bank();"
                                                                                onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" style="width: 90%" /></td>
                                                                                <td class="cell_caption" style="width: 14%; height: 23px;" >
                                                                                    Routing Number:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; color: red; height: 25px;"><input id="txtRemRoutingNo" type="text" style="width: 89%; text-align: left;"/><label id="astRemRoutingNo">*</label></td>
                                                                            </tr>
                                                                                                                                                                
                                                                            <tr id="tr1">
                                                                                <td colspan="6" style="text-align:right; height: 24px">
                                                                                    <input id="btnAddAcct" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                    onclick="javascript:Internet_Accounts.btn_add();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnEditAcct" style="height: 28px; width: 110px" type="button" value="Edit" class="button"
                                                                                    onclick="javascript:Internet_Accounts.btn_edit();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnSaveAcct" style="height: 28px; width: 110px" type="button" value="Save Account" class="button"
                                                                                    onclick="javascript:Internet_Accounts.btn_save();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnCancelAcct" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                    onclick="javascript:Internet_Accounts.btn_cancel();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                </td>
                                                                            </tr>
                                                                         </table>
                                                                     </div>
                                                                </td>
                                                            </tr>
                                                            
                                                            <tr>
                                                                <td>
                                                                   <div id = "divIdsInfo" style= "display:none;"> 
                                                                        <table id="tblIds" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr><td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">IDs</td></tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">ID No.:&nbsp;</td>
                                                                                <td align="left" style="width: 19%; color:Red"><input id="txtRemIDNo" type="text" style="width: 90%; text-align: left;" maxlength="50" /><label id="astRemIDNo">*</label></td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px">ID Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddRemIDType" style="width: 70%;" onchange="javascript:Internet_Remitter_Ids.show_visa_type();"></select><label id="astRemIDType">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                    ID Classification:</td>
                                                                                <td align="left" style="width: 19%; height:23px; color: red">
                                                                                    <select id="ddRemIDClassification" style="width: 93%;">
                                                                                    </select><label id="astRemIDClassification">*</label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px; display:none;" >ID Image:&nbsp;</td>
                                                                                <td class="cell_caption" style="width: 9%; height: 23px;">Visa Type:&nbsp;</td>
                                                                                <td align="left" style="width: 25%; height:23px; color:Red"><select id="ddRemVisaType" style="width: 70%;"></select><label id="astRemVisaType"></label></td>
                                                                                <td align="left" style="width: 25%; display:none;">
                                                                                    </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px">Expiry Date (mm/dd/yyyy):&nbsp;</td>
                                                                                <td align="left" style="width: 19%; color:Red">
                                                                                    <input id="txtRemIDExpiryDate" type="text" style="width: 81%; text-align: left;" maxlength="10" />
                                                                                    <img id="calRemIDExpiryDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtRemIDExpiryDate').disabled == false){popUpCalendar(this, document.getElementById('txtRemIDExpiryDate'), 'mm/dd/yyyy');}"
                                                                                        src="../images/calendar/imgcalen.png" />
                                                                                    
                                                                                    <label id="astRemIDExpiryDate">*</label></td>
                                                                                <td class="cell_caption" style="display: none; width: 9%; height: 23px">
                                                                                </td>
                                                                                <td style="width: 9%; height: 23px"><input id="btnGetGFCCK" style="height: 28px; width: 98%" type="button" value="GFC CK Acct" class="button"
                                                                                                                onclick="javascript:Internet_Remitter_Ids.btn_get_gfc_ck();" onmouseover="this.className='button_over';"
                                                                                                                onmouseout="this.className='button';" /></td>
                                                                                <td align="left" style="width: 25%; color: red; height: 23px"><input id="txtGFCCK" type="text" style="width: 69%; text-align: left;" maxlength="10" /></td>
                                                                                <td align="left" style="display: none; width: 25%">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 24px" >Remarks:&nbsp;</td>
                                                                                <td align="left" style="width: 75%; height: 24px; color:Red" colspan="3"><input id="txtRemIDRemarks" type="text" style="width: 85%; text-align: left;" maxlength="255" /><label id="astRemIDRemarks">*</label></td>                                                                                                        
                                                                            </tr>
                                                                            <tr id="tr6">
                                                                                <td colspan="6" style="text-align:right;height: 24px">
                                                                                    <input id="btnAddIds" style="height: 28px; width: 110px" type="button" value="Add" class="button"
                                                                                    onclick="javascript:Internet_Remitter_Ids.btn_add();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnEditIds" style="height: 28px; width: 110px" type="button" value="Edit" class="button"
                                                                                    onclick="javascript:Internet_Remitter_Ids.btn_edit();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnDeleteIds" style="height: 28px; width: 110px" type="button" value="Delete" class="button"
                                                                                    onclick="javascript:Internet_Remitter_Ids.btn_delete();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnSaveIds" style="height: 28px; width: 110px" type="button" value="Save ID" class="button"
                                                                                    onclick="javascript:Internet_Remitter_Ids.btn_save();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                    <input id="btnCancelIds" style="height: 28px; width: 110px" type="button" value="Cancel" class="button"
                                                                                    onclick="javascript:Internet_Remitter_Ids.btn_cancel();" onmouseover="this.className='button_over';"
                                                                                    onmouseout="this.className='button';" />
                                                                                </td>
                                                                            </tr>                                                                                    
                                                                        </table>                                                                             
                                                                   </div>
                                                                    <div id="divEnrollGFCLoading" style="display:none">                                                                                                                                                                                                                                    
                                                                        <table style="width:100%">
                                                                            <tr>
                                                                                <td style="width: 19%; height: 23px"><div id="div3" class="table_body"></div></td>
                                                                                <td align="left" style="width: 71%; height: 24px" class="cell_desc" colspan="2"><label id="Label5">Loading...</label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>                                                                                                                                                     
                                                                </td>
                                                            </tr>             
                                                            <tr></tr>                                                                   
                                                                                                                  
                                                            <tr>
                                                                <td>                                                                                                                                                 
                                                                    <div id = "divBeneGrid" style= "display:none">
                                                                        <table id="tblGridBene" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
	                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Beneficiaries</td>
                                                                    			
                                                                            </tr>
                                                                            <tr>
	                                                                            <td><div id = "DIVGRIDBENE" onmouseover="javascript:Internet_Remitter_Main.get_active_grid('bene_forapp');">  PLACE GRIDVIEW BENE FOR APPROVAL</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                   
                                                                    
                                                                    <div id = "divBeneGridCIF" style= "display:none">
                                                                        <table id="tblGridBeneCIF" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
	                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Beneficiaries</td>
                                                                    			
                                                                            </tr>
                                                                            <tr>
	                                                                            <td><div id = "DIVGRIDBENECIF" onmouseover="javascript:Internet_Remitter_Main.get_active_grid('bene_cif');">  PLACE GRIDVIEW BENE CIF</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                    <div id = "divAcctGridCIF" style= "display:none">
                                                                        <table id="tblGridAcctCIF" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
	                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Accounts Information</td>
                                                                            </tr>
                                                                            <tr>
	                                                                            <td><div id = "DIVGRIDACCTCIF">  PLACE GRIDVIEW ACCOUNTS CIF</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                    <div id = "divCardGridCIF" style= "display:none">
                                                                        <table id="tblGridCardCIF" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
	                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Cards Information</td>
                                                                    			
                                                                            </tr>
                                                                            <tr>
	                                                                            <td><div id = "DIVGRIDCARDCIF">  PLACE GRIDVIEW CARDS CIF</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                     <div id = "divInternetRemitterCardsGrid" style= "display:none">
                                                                        <table id="tblGridCards" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
	                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Cards Information</td>
                                                                    			
                                                                            </tr>
                                                                            <tr>
	                                                                            <td><div id = "INTCARDSGRID">  PLACE GRIDVIEW CARDS FOR APPROVAL</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                    <div id = "divApproveCards" style= "display:none">
                                                                        <table id="tblApproveGridCards" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
                                                                                <td class="comment">* Select card to approve</td>		                                                                                
                                                                            </tr>
                                                                            <tr>
	                                                                            <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Cards Information</td>
                                                                    			
                                                                            </tr>
                                                                            <tr>
	                                                                            <td><div id = "APPINTCARDSGRID">  PLACE GRIDVIEW CARDS TO APPROVE</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                    <div id = "divInternetRemitterProductsGrid" style= "display:none">
                                                                        <table id="tblProductsCards" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
	                                                                            <td id="tdCapBenProducts" class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Beneficiary's Products</td>
                                                                    			
                                                                            </tr>
                                                                            <tr>
	                                                                            <td><div id = "DIVPRODUCTSGRID">  PLACE GRIDVIEW PRODUCTS</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    
                                                                    <div id="divImgLoad" style="z-index:101;position:absolute;left:628px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: tahoma; top: 452px;">
                                                                        <table style="width:35%;" cellpadding="0" cellspacing="6">
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
                                                                   
                                                                    <div id = "divInternetRemitterIdsGrid" style= "display:none">
                                                                        <table id="tblGridIds" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of IDs Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "DIVIDSGRID">  PLACE GRIDVIEW ID'S</div></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                   
                                                                   <div id = "divInternetRemitterVanGrid" style= "display:none">
                                                                        <table id="tblGridVan" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Van Information</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "DIVVANGRID">  PLACE GRIDVIEW VAN</div></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>                                                                           
                                                                   <div id = "divInternetRemitterNotesGrid" style= "display:none">
                                                                        <table id="tblGridNotes" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394397; background:white;">
                                                                            <tr>
                                                                                <td class="table_caption" style="width: 85%; text-align: left; text-indent: 5px; color: white; height: 26px;" colspan="5">List of Client Notes</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><div id = "DIVNOTESGRID">  PLACE GRIDVIEW NOTES</div></td>
                                                                            </tr>
                                                                        </table>
                                                                   </div>
                                                                </td>
                                                            </tr>
                                                           
                                                        </table>
                                                        <!-- content ends here -->
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div><!--END OF divInternetRemitter-->
                        </td>
                    </tr>
                </table>
        </div>
    </form>
</body>
</html>
