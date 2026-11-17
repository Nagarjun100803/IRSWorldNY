<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetApproval.aspx.cs" Inherits="IRSWorld.Internet.InternetApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Internet Approval</title>
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/commons_min.js"                            type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js"                       type="text/javascript"></script>
    <script src="../js/js_generic_min.js"                           type="text/javascript"></script>
    <script src="../js/js_sidebar_min.js"                           type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js"                        type="text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"                        type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js"                       type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemSideBar_min.js"        type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemitter_min.js"          type="text/javascript"></script>
    <script src="../js/Internet/jsInternetBeneficiary_min.js"       type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemitterCards_min.js"     type="text/javascript"></script>
    <script src="../js/Internet/jsInternetRemitterProducts_min.js"  type="text/javascript"></script>
    <script src="../js/Internet/jsInternetGridView.js"          type="text/javascript"></script>
    <script src="../js/CIF/js_cif_beneficiaries.js"             type="text/javascript"></script>
    <script src="../js/CardRem/jsGFMCMaintenance.js"            type="text/javascript"></script> 
    <script src="../js/Internet/jsInternetApproval.js"          type="text/javascript"></script>
    <script src="../js/Remittance/jsRemExternals.js"       type="text/javascript"></script>

    <script type="text/javascript">
            var Internet_Remitter           = new Internet_Remitter();
            var Internet_Beneficiary        = new Internet_Beneficiary();
            var Internet_Remitter_Cards     = new Internet_Remitter_Cards();
            var Internet_Remitter_Main      = new Internet_Remitter_Main();
            var Internet_Remitter_Products  = new Internet_Remitter_Products();
            var Internet_Approval           = new Internet_Approval();
            var generic                     = new generic_method();
    </script>
</head>
<body onload="javascript:Internet_Remitter_Main.get_clientdata();Internet_Approval.load_defaults();Internet_Approval.construct_side_bar('divAppMenu');"> 
    <form id="form1" runat="server">
    <div style="width: 100%">
        <table style="width:100%" cellpadding="0" cellspacing="10">
            <tr>
                <td style="width: 20%; vertical-align: top; height: 474px;">
                    <div id="divAppMenu" style="margin-top: 20px;">
                        <a id="a1hider" href="javascript:Internet_Remitter_Main.set_side_bar_visibility('divAppMenu', 'a1hider');">«</a>
                    </div>
                    <input style="WIDTH: 1%" id="txtHiddenUserID" type="hidden" />
                    <input style="WIDTH: 1%" id="txtHiddenUserCIFID" type="hidden" />
                    <input style="WIDTH: 1%" id="txtHiddenUserRole" type="hidden" />
                    <input style="WIDTH: 1%" id="txtHiddenUserLoc" type="hidden" />
                </td>
                <td style="width: 80%; vertical-align: top; height: 474px;" >
                    <div id="divInternetBeneApproval" style="width:100%; padding-top:30px;">
                        <table cellpadding="0" cellspacing="0" style="width:100%">
                            <tr>
                                <td style="width:100%">
                                    <table cellpadding="0" cellspacing="0" style="width:100%">
                                        <tr>
                                            <td align="left" valign="bottom" style="vertical-align:bottom; width:0%; height:24px; border-collapse:collapse"> 
                                                <img src="../images/reskin/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="width: 35%; height: 24px">
                                                Internet Approval</td>
                                            <td align="left" style="width: 65%; height: 24px; vertical-align:bottom;">
                                                <img align="absBottom" src="../images/reskin/rend.GIF" /></td> 
                                        </tr>  
                                    </table> 
                                </td>                                   
                            </tr>
                            <tr>
                                <td style="height: 174px; vertical-align:top;">
                                    <table style="border-right: #394398 3px solid; border-top: #394398 3px solid; border-left: #394398 3px solid;border-bottom: #394398 3px solid; width:90%">
                                        <tr>
                                            <td style="width: 100%">
                                                <div id = "divIntRemForApp" style= "display:none">
                                                    <table id="tblIntRemForApp" cellpadding="0" cellspacing="5" style="border: solid 1px #394398; background:#FFFFFF; width: 100%;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: White; " colspan="5">List of Remitters ( For Approval )</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5"><div id = "DIVREMFORAPP" style="font-size: 12px; font-family: Tahoma">  PLACE GRIDVIEW REMITTERS TO APPROVE</div></td>
                                                        </tr>
                                                        <tr id="trLoading3" style="display: none">
                                                            <td align="center" class="cell_desc" colspan="5" style="text-align: center;"><label id="lblLoad5">Loading...</label>
                                                            <div id="divLoading5"></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>                                              
                                                <div id = "divIntBeneForApp">
                                                    <table id="tblIntBeneForApp" cellpadding="0" cellspacing="5" style="border: solid 1px #394398; background:#FFFFFF; width: 100%;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: White" colspan="5">List of Beneficiaries ( For Approval )</td>
                                                			
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5" ><div id = "DIVBENEFORAPP" style="font-size: 12px; font-family: Tahoma">  PLACE GRIDVIEW BENEFICIARIES TO APPROVE</div></td>
                                                        </tr>
                                                        <tr id="trLoading2" style="display: none">
                                                            <td align="center" class="cell_desc" colspan="5"><label id="lblLoad4">Loading...</label>
                                                            <div id="divLoading4"></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>                                                                
                                                <div id = "divIntCardForApp" style="display:none; text-align: center;" >
                                                    <table id="tblntCardForApp" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394398; background:#FFFFFF;">
                                                        <tr>
                                                            <td class="table_caption"; style="text-align: left; text-indent: 5px; color: White" colspan="5">List of Cards ( For Approval )</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height: 19px"><div id = "DIVCARDFORAPP" style="font-size: 12px; font-family: Tahoma">  PLACE GRIDVIEW CARDS TO APPROVE</div></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id = "divIntRemRej" style= "display:none">
                                                    <table id="tblIntRemRej" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394398; background:#FFFFFF;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: White" colspan="5">List of Remitters ( Rejected )</td>
                                                        </tr>
                                                        <tr>
                                                            <td><div id = "DIVREMREJ" style="font-size: 12px; font-family: Tahoma">  PLACE GRIDVIEW REJECTED REMITTERS</div></td>
                                                        </tr>
                                                    </table>
                                                </div>                                                 
                                                <div id = "divIntBeneRej" style= "display:none">
                                                    <table id="tblIntBeneRej" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394398; background:#FFFFFF;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: White; height: 26px;" colspan="5">List of Beneficiaries ( Rejected )</td>
                                                        </tr>
                                                        <tr>
                                                            <td><div id = "DIVBENEREJ" style="font-size: 12px; font-family: Tahoma">  PLACE GRIDVIEW REJECTED BENEFICIARIES</div></td>
                                                        </tr>
                                                    </table>
                                                </div>         
                                                <div id = "divIntCardRej" style= "display:none">
                                                    <table id="tblIntCardRej" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #394398; background:#FFFFFF;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: White" colspan="5">List of Cards ( Rejected )</td>
                                                        </tr>
                                                        <tr>
                                                            <td><div id = "DIVCARDREJ" style="font-size: 12px; font-family: Tahoma">  PLACE GRIDVIEW REJECTED CARDS</div></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="divImgLoad2" style="z-index:101;position:absolute;left:501px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 500px;">
                                                    <table style="width:35%" cellpadding="0" cellspacing="6">
                                                        <tr>
                                                            <td class="cell_desc" align="left" style="font-size: 12px; width: 30%;">
                                                            <label id="lblLoad2">Loading...</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <center><div id="divLoading2"></div></center>
                                                            </td>
                                                        </tr>
                                                    </table>
                                               </div>                                                       
                                                <div id="divBeneficiary" style="z-index:101;position:absolute;left:860px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 154px;"><!--top: 18px;-->
                                                    <table style="width:75%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td colspan="4" style="height: 19px"><label id="lblInfoHeader"><b>Beneficiary Information</b></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4"><hr /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;"><label id="lblClientName">Beneficiary Name:</label></td>
                                                            <td style="width: 75%" colspan="3" class="label1" align="left">&nbsp;<span id="spnBeneName" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;">Address:</td>
                                                            <td style="width: 75%" class="label1" align="left">&nbsp;<span id="spnBeneAddress" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">
                                                                Birth Date:</td>
                                                            <td style="width: 75%" align="left" class="label1" colspan="3">&nbsp;<span id="spnBeneBDate" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">Telephone No:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnBeneTelNo" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">Mobile No:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnBeneMobNo" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;">Email Address:</td>
                                                            <td colspan="3" class="label1" align="left" style="width: 75%" >&nbsp;<span id="spnBeneEmailAdd" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr><td colspan="4">&nbsp;</td></tr>
                                                        <tr id='trProdCode'>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;">Product Code:</td>
                                                            <td colspan="3" class="label1" align="left" style="width: 75%">&nbsp;<span id="spnBeneProductCode" style="font-size: 12px"></span></td>        
                                                        </tr>
                                                        <tr id='trBankName'>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;">Bank Name:</td>
                                                            <td colspan="3" class="label1" align="left" style="width: 75%">&nbsp;<span id="spnBeneBankName" style="font-size: 12px"></span></td>        
                                                        </tr>
                                                        <tr id='trAcctNum'>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px; height: 22px;">Account Number:</td>
                                                            <td colspan="3" class="label1" align="left" style="height: 22px; width: 75%">&nbsp;<span id="spnBeneAccountNumber" style="font-size: 12px"></span></td>
                                                            
                                                        </tr>
                                                        <tr id='trCurrency'>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;">Currency:</td>
                                                            <td colspan="3" class="label1" align="left" style="width: 75%">&nbsp;<span id="spnBeneProductCurrency" style="font-size: 12px"></span></td>        
                                                        </tr>
                                                        <tr id='trSupportingDocs'>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;">Supporting Documents:</td>
                                                            <td colspan="3" class="label1" align="left" style="width: 75%">&nbsp;<select id="ddSupportingDocs" style="width: 59%; background-color: white;">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                                <input id="btnGetSupportingDocs" type="button" class="button" value="Download" style="width:76px;" onclick="Internet_Approval.download_rem_supporting_docs();"/></td>        
                                                        </tr>
                                                        <tr><td colspan="4">&nbsp;</td></tr>                                                                       
                                                        <tr>
                                                            <td colspan="4" class="cell_desc" align="right" style="height: 20px">
                                                                <input id="btnApproveBene" type="button" class="button" value="Approve" style="width:70px;" onclick="Internet_Approval.approve();"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" colspan="4" style="height: 20px">
                                                                Reason:&nbsp;<input id="txtRejectBeneReason" type="text" style="width: 300px; background-color: white;" maxlength="500" />
                                                                <input id="btnRejectBene" type="button" class="button" value="Reject" style="width:70px;" onclick="Internet_Approval.reject();"/></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="label1" colspan="4" style="height: 20px">
                                                                <input id="btnBClosePanel" type="button" class="button"  value="Close" style="width:70px;" onclick="hidePanel('divBeneficiary');javascript:Internet_Approval.delete_temp_files();"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_desc" colspan="4" style="height: 20px">
                                                                <asp:Button ID="Button1" runat="server" Text="Button" class="button" style="width:105px" OnClick="buttonDL_Click"/>
                                                                <asp:HiddenField ID="HPath" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                
                                                <div id="divCard" style="z-index:101;position:absolute;left:724px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 445px;"><!--top: 18px;-->
                                                    <table style="width:75%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td colspan="4" style="height: 19px"><label id="Label1"><b>Card Information</b></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4"><hr /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;"><label id="Label2">
                                                                Remitter Name:</label></td>
                                                            <td colspan="3" class="label1" align="left"style="width: 75%">&nbsp;<span id="spnRemName" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px;">
                                                                Card No:</td>
                                                            <td colspan="3" class="label1" align="left" style="width: 75%">&nbsp;<span id="spnCardNo" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">
                                                                Card Type:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnCardType" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">
                                                                Card Name:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnCardName" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" align="left" style="font-size: 12px; width: 138px; height: 21px;">
                                                                Card Class:</td>
                                                            <td colspan="3" class="label1" align="left" style="height: 21px; width: 75%">&nbsp;<span id="spnCardClass" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">
                                                                Issue Date:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnIssueDate" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">
                                                                Expiry Date:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnExpiryDate" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px">
                                                                Issuing Bank:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnIssuingBank" style="font-size: 12px"></span></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="font-size: 12px; width: 138px; height: 18px">
                                                                Currency:</td>
                                                            <td align="left" class="label1" colspan="3" style="width: 75%">&nbsp;<span id="spnCurrency" style="font-size: 12px"></span></td>
                                                        </tr>                                                                    
                                                        <tr>
                                                            <td colspan="4" class="label1" align="right" style="height: 20px">
                                                                <input id="btnApproveCard" type="button" class="button" value="Approve" style="width:70px;" onclick="Internet_Remitter_Cards.approve_card();"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" colspan="4" style="height: 20px">
                                                                Reason:&nbsp;<input id="txtRejectCardReason" type="text" style="width: 300px; background-color: white;" maxlength="500" />
                                                                <input id="btnRejectCard" type="button" class="button" value="Reject" style="width:70px;" onclick="Internet_Remitter_Cards.reject_card();"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="label1" colspan="4" style="height: 20px">
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
        </table>
    </div>
    </form>
</body>
</html>
