<%@ Page Language="C#" AutoEventWireup="true" Codebehind="BranchDefaults.aspx.cs"
    Inherits="IRSWorld.BranchSetup.BranchDefaults" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Branch Defaults</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_branchdefaults_min.js" type="text/javascript"> </script>
</head>
<body id="theBody" onload="page_setup();"> <!-- set_init_value();"> -->
    <center>
        <form id="form1" runat="server">
            <div id="divBody" style="width: 100%; vertical-align: top; margin-top: 30px;">
                <table id="tblContent" style="width: 100%;">
                    <tr>
                        <td id="tdContent" align=center style="width: 98%; border-collapse: collapse;">
                            <table id="divBranchSetupContent" style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width:100%; vertical-align:bottom" align=left>
                                        <table style="width:100%" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td style="width:60%; vertical-align:bottom; text-align:left">
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom;">
                                                            <img src="../images/toe.GIF" /></td>
                                                        <td id="td1" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                                            Branch Defaults</td>
                                                        <td align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        </table>
                                    </td>                                              
                                </tr>
                                <tr id="Tr1">
                                    <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse;"
                                        class="table_body">
                                        <table style="width: 100%">
                                            <tr>
                                                <td id="search" class="tab_selected" align="left">
                                                    <a href="javascript:select('search');">Search</a></td>
                                                <td id="bene_tab" class="tab_unselected" align="left">
                                                    <a href="javascript:select('Beneficiary');">Beneficiary</a></td>
                                                <td id="rem_tab" class="tab_unselected" align="left">
                                                    <a href="javascript:select('Remitter');">Remitter</a>&nbsp;</td>
                                            </tr>
                                        </table>
                                        <table id="tblBranch"  style="width: 100%">
                                        <tr>
                                            <td colspan="3" class="table_caption" style="width:100%; text-align: left">
                                                Branch</td>
                                          </tr>  
                                            <tr>                                            
                                                <td class="cell_caption" style="width: 20%; height: 14px;" align="right">
                                                    Select Branch</td>
                                                <td style="width:40%"align="left">
                                                    <select id="ddlBranches" style="width: 277px" onchange="new getDropDown().Load_LocalCurrency();new getDropDown().Load_AccountCurrency();new getDropDown().Load_RemitCurrency();"
                                                        name="BranchCode">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </td>
                                                <td style="width:40%">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="3">
                                                    <input id="btnSearch" class="cancel_button" onmouseout="this.className='cancel_button';"
                                                        onmouseover="this.className='cancel_button_over';" type="button" value="Search"
                                                        onclick="set_selected_form('Beneficiary');form_selected('','false');getBranchDefaults();populate_rec();"
                                                        style="width: 19%" /></td>
                                            </tr>                                                                                    
                                        </table>
                                        <table id="Table1" style="width: 100%; vertical-align: top">
                                            <tr>
                                                <td style="width: 94%;" colspan="2" align="left">
                                                    <table id="tblRemitter" style="width:100%">
                                                        <caption class="table_caption" style="text-align: left">
                                                            Remitter</caption>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 7px;">
                                                                Worker Type:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<select id="ddlWorkType" name="WorkerType">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="width: 20%; height: 7px;">
                                                                District:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <input id="txtDistrict" type="text" name="District" onkeyup="check_char_inputted('txtDistrict');" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: auto; height: 20%;">
                                                                Remitter Type:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                &nbsp;<select id="ddlRemitterType" name="RemitterType">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="height: 7px; width: 20%;">
                                                                City:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <input id="txtCity" type="text" name="City" onkeyup="check_char_inputted('txtCity');" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 7px;">
                                                                Remitter Charge Type:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                &nbsp;<select id="ddlRemitChargeType" name="ChargeType">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="width: 20%; height: 7px;">
                                                                State:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <input id="txtState" type="text" name="State" onkeyup="check_char_inputted('txtState');" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20; height: 7px;">
                                                                VIP Status:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                &nbsp;<select id="ddlVipStat" name="VIPCardStatus">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="width: 20%; height: 7px;">
                                                                Region:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <input id="txtRegion" type="text" name="Region" onkeyup="check_char_inputted('txtRegion');" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%;">
                                                                Gender</td>
                                                            <td align="left" style="width: 30%;">
                                                                &nbsp;<select id="ddlGender" name="Gender">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="width: 20%;">
                                                                Country:&nbsp</td>
                                                            <td align="left" style="width: 30%;">
                                                                <select id="ddlCountry2" style="width: 91%" name="RemitterCountry">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%;">
                                                                Civil Status:</td>
                                                            <td align="left" style="width: 30%;">
                                                                &nbsp;<select id="ddlCivilStat" name="Civil Status">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="width: 20%;" align="left">
                                                                PO Box District</td>
                                                            <td align="left" style="width: 30%;">
                                                                <input id="txtPOBoxDistrict" type="text" name="POBoxDistrict" onkeyup="check_char_inputted('txtPOBoxDistrict');" />&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Occupation:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<select id="ddlOccupation" name="Occupation">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Home Zip Code:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <input id="txtHomeZip" type="text" name="HomeZipCode" onkeyup="check_char_inputted('txtHomeZip');" />&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Citizenship:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<select id="ddlCitizenship" name="Citizenship">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Office Zip Code:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <input id="txtOfficeZip" type="text" name="OfficeZipCode" onkeyup="check_char_inputted('txtOfficeZip');" />&nbsp;</td>
                                                        </tr>
                                                         <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">Age Limit:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<input id="txtAgeLimit" name="AgeLimit" style="width: 18%" type="text" /></td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Remitter Inputted ID</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <input id="chkInputtedId" style="width: 20%" type="checkbox" name="RemitterInputtedId" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="width: 20%" class="table_caption">
                                                                Preferred:
                                                            </td>
                                                            <td style="width: 20%; height: 8px;" class="cell_caption">
                                                                On Behalf</td>
                                                            <td style="width: 30%">
                                                                <input id="chkOnBehalf" style="width: 20%" type="checkbox" name="OnBehalf" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Mail:
                                                            </td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlMail" name="PreferredMailingAddress">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Card Transaction:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <input id="chkCardTranx" style="width: 20%" type="checkbox" name="CardTransaction" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Contact:
                                                            </td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlContact" name="ContactType">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Risk Rating:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<select id="ddlRiskRating" style="width: 90%" name="RiskRating"><option selected="selected">
                                                                </option>
                                                                </select></td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblBene" style="width: 100%; vertical-align: top">
                                                        <caption class="table_caption" style="text-align: left;">
                                                            Beneficiary</caption>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 14px;" align="right">
                                                                &nbsp; &nbsp;&nbsp;Beneficiary Type:&nbsp</td>
                                                            <td align="left" style="height: 14px; width: 30%;">
                                                                <select id="ddlBenType" style="width: 95%" name="BeneficiaryType">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; height: 14px;" colspan="" rowspan=""
                                                                align="right">
                                                                Relation to Remitter:&nbsp</td>
                                                            <td align="left" style="height: 14px; width: 30%;">
                                                                <select id="ddlRelation" style="width: 95%" name="Relationship">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 8px;" align="right">
                                                                Product Code:</td>
                                                            <td align="left" style="height: 8px; width: 30%;">
                                                                <select id="ddlProductCode" style="width: 95%" name="ProductCode">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; height: 8px;" align="right">
                                                                Notification Mode:&nbsp</td>
                                                            <td align="left" style="height: 8px; width: 30%;">
                                                                <select id="ddlNotify" style="width: 95%" name="NotificationMode">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 11px;" align="right">
                                                                SMS Notification:</td>
                                                            <td align="left" style="height: 11px; width: 30%;">
                                                                <input id="chkSMSNotification" name="SMSNotification" style="width: 15%;" type="checkbox" /></td>
                                                            <td class="cell_caption" style="width: 20%; height: 11px;" align="right">
                                                                Country:&nbsp</td>
                                                            <td align="left" style="height: 11px; width: 30%;">
                                                                <select id="ddlCountry" style="width: 95%" name="Country">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%; height: 22px;">
                                                            </td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;&nbsp;</td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Charge Group:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlChargeGroup" style="width: 95%" name="ChargeGroup">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 20%; height: 13px;" align="right">
                                                            </td>
                                                            <td align="left" style="height: 13px; width: 30%;">
                                                                <input id="chkSameAsRemitter" style="width: 15%; display: none;" type="checkbox"
                                                                    name="SameAsRemitter" checked="CHECKED" /></td>
                                                            <td style="width: 20%; height: 13px;" align="right">
                                                                &nbsp</td>
                                                            <td align="left" style="height: 13px; width: 30%;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblRemittance" style="width: 100%;">
                                                        <caption class="table_caption" style="text-align: left">
                                                            Remittance</caption>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Source of Funds:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlSourceOfFunds" style="width: 95%" name="SourceOfFunds">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20; height: 7px;">
                                                                Local Currency:&nbsp</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <select id="ddlLocalCurrency" style="width: 95%" name="LocalCurrency">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Pay Mode:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <select id="ddlPayMode" style="width: 95%" name="PayMode">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="height: 7px; width: 20%">
                                                                Rapid Remit Code:&nbsp</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <select id="ddlRapid" style="width: 95%" name="RapidRemitCode">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Receipt Mode:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <select id="ddlReceiptMode" style="width: 95%" name="ReceiptMode">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; height: 7px;">
                                                                Rapid Remit Key:&nbsp</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <input id="chkRapidRemit" style="width: 15%" type="checkbox" name="RapidRemitKey" />
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Account Type:</td>
                                                            <td align="left" style="height: 7px; width: 30%;">
                                                                <select id="ddlAccountType" style="width: 95%" name="AccountType">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td style="width: auto; height: 20%;" class="cell_caption">
                                                                Expired ID Threshold</td>
                                                            <td align="left" style="height: 7px; width: 30%;"><input id="txtThreshold" type="text" name="IDThreshold" onkeyup="check_char_inputted('txtThreshold');" maxlength=3 style="width: 19%; text-align:right" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Account Currency:</td>
                                                            <td align="left" style="width: 30%;">
                                                                <select id="ddlAccountCurrency" style="width: 95%" name="AccountCurrency">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%;">
                                                                Transaction Source:&nbsp</td>
                                                            <td align="left" style="width: 30%;">
                                                                <select id="ddltrnxSource" style="width: 95%" name="TranxSource">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Remitting Currency:</td>
                                                            <td align="left" style="width: 30%;">
                                                                <select id="ddlRemitCurr" style="width: 95%" name="RemittingCurrency">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: auto;">
                                                                Transaction Purpose:&nbsp</td>
                                                            <td align="left" style="width: 20%;">
                                                                <select id="ddltrnxPurpose" style="width: 95%" name="TranxPurpose">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Covering Bank:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlCoveringBank" style="width: 95%" name="CoveringBank">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Computation:&nbsp</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlComputation" style="width: 95%" name="Computation">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Implementation Type:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlImplemType" style="width: 95%" name="ImplementationType">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Half Page</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <input id="chkHalf" type="checkbox" style="width:15%" name="HalfPage" />
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                GFC ChargeType:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                <select id="ddlGFCCharge" style="width: 95%" name="GFCChargeType">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td style="width: 20%; height: 22px;" class="cell_caption">
                                                            On Behalf
                                                            </td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                            <input id="chkBehalf" type="checkbox" style="width:15%" name="OnBehalf2" />
                                                            <input id="txtSSSNo" style="width: 222px; text-align: right; display: none;" type="text"
                                                                    name="SSSAcctNo" onchange="onChangeText('txtSSSNo');" maxlength="12" />
                                                            </td>
                                                                
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Stream Lining:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<input id="chkStream" type="checkbox" style="width:15%" name="StreamLining" /></td>
                                                            
                                                                
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Charge Type for PHP:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<select id="ddlChargeTypePHP" style="width: 95%" name="ChargeTypePHP">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            
                                                                
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Charge Type for USD:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<select id="ddlChargeTypeUSD" style="width: 95%" name="ChargeTypeUSD">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            
                                                                
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Charge Type for EUR:</td>
                                                            <td align="left" style="height: 22px; width: 30%;">
                                                                &nbsp;<select id="ddlChargeTypeEUR" style="width: 95%" name="ChargeTypeEUR">
                                                                    <option selected="selected"></option>
                                                                </select></td>
                                                            
                                                                
                                                        </tr>
                                                    </table>
                                                    <table id="tblSelFields"  style="width: 100%">
                                                        <caption class="table_caption" style="text-align: left">
                                                            Remittance Aggregation</caption>
                                                        <tr>
                                                            <td class="cell_caption" style="height: 22px; width: 20%;">
                                                                Date Value 1:</td>
                                                            <td style="width: 20%" align="left">
                                                                <input id="txtDateValue1" type="text" name="RemittanceAggregation" onchange="onChangeText('txtDateValue1');"
                                                                    style="width: 90%;" /></td>
                                                            <td style="width: 15%">
                                                                <select id="ddlDateValue1" style="width: 70%" name="RemittanceAggregation">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td style="width: 45%; height: 22px;">
                                                                <fieldset>
                                                                    <legend class="cell_desc">Cash Aggregation</legend>
                                                                    <table style="width:100%">
                                                                        <tr>
                                                                        <td class="cell_caption" style="width: 35%; height: 22px;">
                                                                            Date Value:</td>
                                                                        <td style="width: 35%" align="left">
                                                                            <input id="txtCashAggValue1" type="text" name="CashAggregation" onchange="onChangeText('txtCashAggValue1');"
                                                                                style="width: 90%" /></td>
                                                                        <td style="width: 30%">
                                                                            <select id="ddlCashAggValue1" style="width: 70%" name="CashAggregation">
                                                                                <option selected="selected"></option>
                                                                            </select>
                                                                        </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>                                                            
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Date Value 2:</td>
                                                            <td style="width: 20%" align="left">
                                                                <input id="txtDateValue2" type="text" name="RemittanceAggregation" onchange="onChangeText('txtDateValue2');"
                                                                    style="width: 90%" /></td>
                                                            <td style="width: 15%">
                                                                <select id="ddlDateValue2" style="width: 70%" name="RemittanceAggregation">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td style="width: 45%;height: 22px; display:none" rowspan="2" id="tdAgentAgg">
                                                                <fieldset>
                                                                    <legend class="cell_desc">Agent Aggregation</legend>
                                                                    <table style="width:100%">
                                                                        <tr>
                                                                        <td class="cell_caption" style="width: 35%; height: 22px;">
                                                                            Date Value:</td>
                                                                        <td style="width: 35%" align="left">
                                                                            <input id="txtAgentAggValue1" type="text" name="AgentAggregation" onchange="onChangeText('txtAgentAggValue1');"
                                                                                style="width: 90%" /></td>
                                                                        <td style="width: 30%">
                                                                            <select id="ddlAgentAggValue1" style="width: 70%" name="AgentAggregation">
                                                                                <option selected="selected"></option>
                                                                            </select>
                                                                        </td>
                                                                        </tr>
                                                                    </table>
                                                                </fieldset>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 22px;">
                                                                Date Value 3:</td>
                                                            <td style="width: 20%" align="left">
                                                                <input id="txtDateValue3" type="text" name="RemittanceAggregation" onchange="onChangeText('txtDateValue3');"
                                                                    style="width: 90%" /></td>
                                                            <td style="width: 15%">
                                                                <select id="ddlDateValue3" style="width: 70%" name="RemittanceAggregation">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblDefaults" style="width:100%; margin-top: 2%">
                                                        <caption class="table_caption" style="text-align: left">
                                                            Setup</caption>
                                                        <tr>
                                                            <td id="tdDefaults" align="center" style="width: 40%;">
                                                            </td>
                                                            <td style="width:10%">&nbsp;</td>
                                                            <td valign="top" style="width: 50%">
                                                                <table id="tblExtra" style="width:100%;">
                                                                    <tr>
                                                                        <td class="cell_caption" style="width: 40%;">
                                                                            Enabled Round up Billed Amount (OBPS)&nbsp</td>
                                                                        <td style="height: 22px; width: 60%;">
                                                                            <select id="ddlRoundUp" style="width: 60%;" name="RoundupOBPSAmount">
                                                                                <option selected="selected"></option>
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption">
                                                                            SSS Backend Charge (OBPS)&nbsp</td>
                                                                        <td align="left" style="height: 22px;">
                                                                            <input id="txtSSSBackend" type="text" name="SSSBackendCharge" onkeyup="check_char_inputted('txtSSSBackend');" style="width: 90%" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption">
                                                                            SSS Excess (OBPS)&nbsp</td>
                                                                        <td style="height: 22px;">
                                                                            <select id="ddlExcess" style="width: 60%;" name="ExcessSSS">
                                                                                <option selected="selected"></option>
                                                                            </select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2" style="width:100%;height:10px; font-size:8px;">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption">
                                                                            Restrict editing of currency rates&nbsp</td>
                                                                        <td style="height: 22px;">
                                                                            <input id="chkCurrRateNoEdit" style="width: 10%" type="checkbox" name="CurrencyRatesNoEdit" />&nbsp;<label class="cell_desc" style="font-size:10px;">(Only USD-PHP under SourceToTarget is editable)</label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption">
                                                                            Detail mandatory when occupation type is Others (CIF)&nbsp</td>
                                                                        <td style="height: 22px;">
                                                                            <input id="chkRemDetDesc" style="width: 10%" type="checkbox" name="OccupationOthersMandatory" /></td>
                                                                    </tr>
                                                                    
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table id="tblVIP" style="width:50%; margin-top: 2%">
                                                        <caption class="table_caption" style="text-align: left">
                                                            Setup VIP</caption>
                                                        <tr>
                                                            <td id="tdVIP" align="center" style="width: 100%;">
                                                            
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <center>
                                                        <table id="tblButtonsMain" cellpadding="0" cellspacing="5" style="width:70%">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%">
                                                                    <table cellpadding="0" cellspacing="5" style="width:75%">
                                                                        <tr>
                                                                            <td style="width: 20%">
                                                                                <input id="btnEdit" class="button" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" type="button" value="Edit" onclick="enabled_button('Edit');" /></td>
                                                                            <td style="width: 20%">
                                                                                <input id="btnSave" class="button" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" type="button" value="Save" disabled="disabled"
                                                                                    onclick="save_branchdefaults();enabled_button('Save');" /></td>
                                                                            <td style="width: 20%">
                                                                                <input id="btnCancel" class="button" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='warning_over';" type="button" value="Cancel"
                                                                                    onclick="enabled_button('Cancel');" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table id="tblRemitterButton" cellpadding="0" cellspacing="5" style="width:70%">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%">
                                                                    <table cellpadding="0" cellspacing="5" style="width:75%">
                                                                        <tr>
                                                                            <td style="width: 20%">
                                                                                <input id="btnEdit2" class="button" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" type="button" value="Edit" onclick="enabled_button2('Edit');" /></td>
                                                                            <td style="width: 20%">
                                                                                <input id="btnSave2" class="button" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" type="button" value="Save" disabled="disabled"
                                                                                    onclick="enabled_button2('Save');" /></td>
                                                                            <td style="width: 20%">
                                                                                <input id="btnCancel2" class="button" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='warning_over';" type="button" value="Cancel"
                                                                                    onclick="enabled_button2('Cancel');" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
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
            </div>
        </form>
        <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx"
            target="_parent">
        </form>
        <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BranchSetup/BranchDefaults.aspx"
            target="_parent">
        </form>
    </center>
</body>
</html>
