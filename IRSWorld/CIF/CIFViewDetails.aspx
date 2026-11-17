<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIFViewDetails.aspx.cs" Inherits="IRSWorld.CIF.CIFViewDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CIF Details</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/commons.js"              type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"         type="text/javascript"></script> 
    <script src="../js/js_generic.js"         type="text/javascript"></script> 
    <script src="../js/jshttprequest.js"          type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"          type="text/javascript"></script> 
    <script src="../js/jsgridselector.js"         type="text/javascript"></script> 
    <script src="../js/CIF/js_cif_viewdetails.js" type="text/javascript"></script>
    <!--<script src="../js/CIF/js_cif_main.js"        type="text/javascript"></script>-->
    
    <script type="text/javascript">    
        var cifviewdetails = new cif_viewdetails();
    </script>
    
</head>
<body onload="javascript:cifviewdetails.load_main();" onresize="fix_dim();"> <!-- -->
<center>
    <form id="form1" runat="server">
        <div id=divmain style="width:100%;">
            <table style="width:100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align=right style="width: 18%; vertical-align:top; padding-top:30px">
                        <a id="Back" href="javascript:cifviewdetails.back();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Back</a>
                    </td>
                    <td id=tdmain align="center" style="width:82%; vertical-align:top; padding-top:30px;" valign="top">
                        <div id=divover style="width:100%; height:100%; overflow:auto">
                        <table cellspacing="0" cellpadding="0" style="width:90%; background-color:Azure">
                            <tr>
                                <td>
                                <table cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                            <img src="../images/toe.GIF" /></td>    
                                        <td id="capResult" class="table_head_dark" style="width: 50%; height: 24px">
                                            View CIF Details</td>
                                        <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>  
                                </table> 
                                </td>   
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; height: 100%">
                                    <table style="width:100%; height: 100%;" class="table_border" cellspacing="8px">
                                        <tr>
                                            <td>
                                                <div id="CIFVIEWINDIVIDUAL" style="display:none;">
                                                    <table id="tblCIFIndividualDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="6">Individual</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Last Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblLastName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">First Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblFirstName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Middle Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblMiddleName" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">CIFID:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblCIFID" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Alias:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblAlias" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Title:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblTitle" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Gender:&nbsp;</td>
                                                            <td align="left" style="width: 197px"><label id="lblGender" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Birth Date:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBirthDate" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Birth Place:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBirthPlace" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Age:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblAge" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Citizenship:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblCitizenship" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Civil Status:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblCivilStatus" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Religion:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblReligion" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Branch:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBranch" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Remarks:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblRemarks" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Date Created:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblDateCreated" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>                                                        
                                                    </table>
                                                </div>
                                                
                                                <div id="CIFVIEWBUSINESS" style="display:none">
                                                    <table id="tblCIFBusinessDetails" cellpadding="0" cellspacing="4" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Business</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Business Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblBusinessName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 13%; height: 24px">Short Name:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblShortName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Alias:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBusAlias" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">CIFID:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblBusCIFID" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 13%; height: 24px">Type of Business:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBusType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Branch:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBusBranch" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 13%; height: 24px">Remarks:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBusRemarks" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 13%; height: 24px">Date Created:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBusDateCreated" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>                                                    
                                                    </table>
                                                </div>
                                                
                                                <br />
                                                <div id="CIFVIEWREMINFO" style="display:none">                                                
                                                    <table id="tblRemitterInfoDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Remitters Other Information</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">VIP Status:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblVIPStatus" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 16%; height: 24px">Remitter Points:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblRemitterPoints" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Risk Rating:</td>
                                                            <td align="left" style="width: 21%"><label id="lblRiskRating" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Charge Type:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblChargeType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 16%; height: 24px"></td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="Label6" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 12%; height: 24px"></td>
                                                            <td align="left" style="width: 21%"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWADDRESS" style="display:none">                                                
                                                    <table id="tblCIFAddressDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Address</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Room No. / Bldg. / PO Box:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblRoomNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Province:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblProvince" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Preferred Mailing Address:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblPrefMailAdd" class="cell_desc" style="font-weight: normal"></label></td>
                                                            
                                                        </tr>                                                    
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Street No./ Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblStreetNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Zip Code:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblZipCode" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Remarks:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblAddRemarks" class="cell_desc" style="font-weight: normal"></label></td>                                                        
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Municipality:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblMunicipality" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Address Type:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblAddressType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 12%; height: 24px"></td>
                                                            <td align="left" style="width: 21%; height: 24px"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFADDRESSGRID" style="display:none">                                                
                                                    <table id="tblAddressGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Address</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWADDRESSGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWOCCUPATIONS" style="display:none">                                                
                                                    <table id="tblCIFOccupationDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Occupation</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Occupation:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblOccupation" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Position:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblPosition" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Currency:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblCurrency" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Occupation Type:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblOccupationType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Employer:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblEmployer" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Remarks:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblOccupationRemarks" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Worker Type:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblWorkerType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Salary:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblSalary" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 12%; height: 24px"></td>
                                                            <td align="left" style="width: 21%; height: 24px"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFOCCUPATIONSGRID" style="display:none">                                                
                                                    <table id="tblOccupationsGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Occupations</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWOCCUPATIONSGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWACCOUNTS" style="display:none">                                                
                                                    <table id="tblCIFAccountsDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Accounts</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Account No:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblAccountNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Status:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblAccountStatus" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Foreign Bank:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblForeignBank" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Account Type:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblAccountType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">PNB Branch:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblPNBBranch" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Remarks:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblAccountsRemarks" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Currency:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblAccountsCurrency" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Domestic Bank:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblDomesticBank" class="cell_desc" style="font-weight: normal">                                                            Domestic Bank</label></td>
                                                            <td style="width: 12%; height: 24px"></td>
                                                            <td align="left" style="width: 21%; height: 24px"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFACCOUNTSGRID" style="display:none">                                                
                                                    <table id="tblAccountsGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Accounts</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWACCOUNTSGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWCARDS" style="display:none">                                                
                                                    <table id="tblCIFCardsDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Cards</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Card No.:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblCardNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Expiry Date:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblExpiryDate" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Card Image:&nbsp;</td>
                                                            <td align="left" rowspan="3" style="width: 21%">
                                                            </td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Card Type:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblCardType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Issuing Bank:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblIssuingBank" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Card Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblCardName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Security No.:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblSecurityNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px"></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Card Class:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblCardClass" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Issuing No.:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblIssuingNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Country:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblCountry" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Issue Date:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblIssueDate" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Currency:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblCardCurrency" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Remarks:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblCardsRemarks" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFCARDSGRID" style="display:none">                                                
                                                    <table id="tblCardsGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Cards</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWCARDSGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWIDS" style="display:none">                                                
                                                    <table id="tblCIFIdsDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">IDs</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">ID No:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblIDNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Expiry Date:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblExpirydate" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">ID Image:&nbsp;</td>
                                                            <td align="left" rowspan="2" style="width: 21%"></td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">ID Type:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblIDType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Remarks:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblIDRemarks" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 12%; height: 24px"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFIDSGRID" style="display:none">                                                
                                                    <table id="tblIdsGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">IDs</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWIDSGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWCONTACTS" style="display:none">                                                
                                                    <table id="tblCIFContactsDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Contacts</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Contact No:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblContactNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Preferred No:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblPreferredNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Email Address:&nbsp;</td>
                                                            <td align="left" style="width: 21%"><label id="lblEmailAddress" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Contact Type:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblContactType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">SMS Alert:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblSMSAlert" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 12%; height: 24px"></td>
                                                            <td align="left" style="width: 21%"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFCONTACTSGRID" style="display:none">                                                
                                                    <table id="tblContactsGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Contacts</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWCONTACTSGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWMERCHANTINFO" style="display:none">                                                
                                                    <table id="tblMerchantInfoDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Merchants Other Information</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Merchant Code:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblMerchantCode" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Currency:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblMerCurrency" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Product Code:&nbsp;</td>
                                                            <td align="left" style="width: 21%"><label id="lblProductCode" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr> 
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Account No:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblMerAccountNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Account Type:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblMerAccountType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 12%; height: 24px"></td>
                                                            <td align="left" style="width: 21%"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFMERINFOGRID" style="display:none">                                                
                                                    <table id="tblMerInfoGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Merchants Information</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWMERINFOGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                
                                                
                                                <div id="CIFVIEWBENEFICIARIES" style="display:none">                                                
                                                    <table id="tblBeneficiariesDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Beneficiaries</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Full Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblBeneFullName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Product Code:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblBeneProductCode" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Notification Mode:&nbsp;</td>
                                                            <td align="left" style="width: 21%"><label id="lblNotificationMode" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Relationship:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblRelationship" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Implementation Type:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblImplementationType" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Beneficiary Account No:&nbsp;</td>
                                                            <td align="left" style="width: 21%"><label id="lblBeneAccountNo" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFBENEGRID" style="display:none">                                                
                                                    <table id="tblBeneGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Beneficiaries</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWBENEGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWMERCHANTS" style="display:none">                                                
                                                    <table id="tblMerchants" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Merchants</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Merchant CIFID:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="Label1" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Business Name:&nbsp;</td>
                                                            <td align="left" style="width: 37%; height: 24px"></td>
                                                            <td style="width: 12%; height: 24px">&nbsp;</td>
                                                            <td align="left" style="width: 21%"><label id="Label3" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Account No-Currency:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="Label4" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Product Code:&nbsp;</td>
                                                            <td align="left" style="width: 37%; height: 24px"><label id="Label5" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td style="width: 12%; height: 24px">&nbsp;</td>
                                                            <td align="left" style="width: 21%"></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFMERCHANTSGRID" style="display:none">                                                
                                                    <table id="tblMerchantsGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Merchants</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWMERCHANTSGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFVIEWSPOUSE" style="display:none">                                                
                                                    <table id="tblCIFSpouseDetails" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black" colspan="6">Spouse</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Last Name:&nbsp;</td>
                                                            <td align="left" style="width: 22%; height: 24px"><label id="lblSpouseLastName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">First Name:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblSpouseFirstName" class="cell_desc" style="font-weight: normal"></label></td>
                                                            <td class="cell_caption" style="width: 12%; height: 24px">Middle Name:&nbsp;</td>
                                                            <td align="left" style="width: 21%; height: 24px"><label id="lblSpouseMiddleName" class="cell_desc" style="font-weight: normal"></label></td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFSPOUSEGRID" style="display:none">                                                
                                                    <table id="tblSpouseGrid" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Spouse</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWSPOUSEGRID"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFOTHERIDSGRID" style="display:none">                                                
                                                    <table id="tblOtherIDs" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Other CIF IDs</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWOTHERIDS"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
                                                </div>
                                                
                                                <div id="CIFWATCHLISTGRID" style="display:none">                                                
                                                    <table id="tblWatchlist" cellpadding="0" cellspacing="2" style="width:98%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="text-align: left; text-indent: 5px; color: Black">Watchlist</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="CIFVIEWWATCHLIST"></div>
                                                            </td>
                                                        </tr>
                                                    </table><br />
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
</center>
</body>
</html>
