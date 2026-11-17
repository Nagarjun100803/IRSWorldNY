<%@ Page Language="C#" AutoEventWireup="true" Codebehind="tranx_profile_create.aspx.cs"
    Inherits="IRSWorld.Report_Profiler.tranx_profile_create" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Report Profile</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />

    <script src="../js/Report_Profiler/js_rpt_profile_tranx.js" type="text/javascript"></script>

    <script src="../js/popcalendar.js" type="text/javascript"></script>

    <script src="../js/jsCommons.js" type="text/javascript"></script>

    <script src="../js/jshttprequest.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/Reports/js_ReportsTemplate.js" type="text/javascript"></script>

    <script src="../js/Remittance/jsRemExternals.js" type="text/javascript"></script>

</head>
<body onload="javascript:report_builder.initializepage();">
    <center>
        <div style="overflow: auto;">
            <form id="form1" runat="server">
                <div id="divmain" style="width: 88%; margin: 0; height: 437px; overflow: auto; padding-top: 25px;">
                    <table id="tblMain" cellspacing="0" cellpadding="0" style="background-color: Azure;
                        width: 95%;">
                        <tr>
                            <td align="center" valign="top" style="width: 129%; border-collapse: collapse;" class="content_height">
                                <table width="90%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="bottom" style="width: 1%; height: 24px; border-collapse: collapse">
                                            <img src="../images/toe.GIF" /></td>
                                        <td id="capLabel" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                            color: White; border-collapse: collapse">
                                            Report Builder</td>
                                        <td id="tdpopup2" align="left" valign="bottom" style="height: 24px; width: 436px;"
                                            class="bg_body">
                                            <img align="absbottom" src="../images/rend.gif" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%;">
                                            <table style="width: 100%; border: solid 3px #336699;" id="TABLE1">
                                                <tr>
                                                    <td class="table_caption" colspan="3" style="height: 26px; color: White; background: #336699">
                                                        CREATE REPORT PROFILE
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4">
                                                        <table>
                                                            <tr>
                                                                <td align="left" class="cell_caption_left" style="height: 24px; width: 4%;">
                                                                    Profile(s)</td>
                                                                <td align="left" style="width: 13%; height: 24px;" colspan="3" class="cell_caption">
                                                                    <select style="width: 69%; float: left;" id="ddProfiles">
                                                                    </select>
                                                                    <input id="btnLoadProfile" style="width: 30%;" type="button" value="Load Profile"
                                                                        onclick="javascript:report_builder.LoadProfile();" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="cell_caption_left" style="width: 4%;">
                                                                    Create New Profile</td>
                                                                <td align="left" style="width: 13%;" colspan="3" class="cell_caption">
                                                                    <input id="txtProfileName" style="text-align: left; float: left; width: 68%;" type="text"
                                                                        maxlength="50" />
                                                                    <input id="btnCheck" style="width: 30%; float: right;" type="button" value="Check Availability"
                                                                        onclick="javascript:report_builder.Click_CheckProfileName();" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="cell_caption_left" style="width: 4%;">
                                                                    <input id="chkOFWReports" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckOFWReport('chkOFWReports');" />
                                                                    Generate OFW Reports</td>
                                                                <td align="left" style="width: 13%;" colspan="3" class="cell_caption">
                                                                    <select style="width: 69.5%; float: left;" id="ddOFWReports" onchange="javascript:report_builder.createReportParams();">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 482px">
                                                        <table cellspacing="0" cellpadding="0" style="width: 100%">
                                                            <tr>
                                                                <td class="cell_caption" style="height: 37px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td class="cell_caption_center" style="height: 16px; color: White; background: #336699;">
                                                                                Remit Method
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px" align="left" class="cell_caption_left">
                                                                                <input id="chkAllRemitMethod" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('ddRemitMethod','chkAllRemitMethod');" />
                                                                                Select All</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td class="cell_caption" style="height: 37px; width: 477px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td class="cell_caption_center" style="height: 16px; color: White; background: #336699;">
                                                                                Batch Types
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px" align="left" class="cell_caption_left">
                                                                                <input id="chkAllBatchTypes" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('lstBatchTypes','chkAllBatchTypes');" />
                                                                                Select All</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 50%">
                                                                    <select name="Remit_Method" size="7" multiple="multiple" id="ddRemitMethod" style="width: 100%">
                                                                    </select>
                                                                </td>
                                                                <td align="left" style="width: 50%">
                                                                    <select name="Batch_Type" size="7" multiple="multiple" id="lstBatchTypes" style="width: 100%">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="height: 37px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td align="center" class="cell_caption_center" style="height: 16px; color: White;
                                                                                background: #336699">
                                                                                Remitting Branch
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px;" align="left" class="cell_caption_left">
                                                                                &nbsp;<select style="width: 50%; float: none;" id="ddRegion" onchange="javascript:report_builder.SetBranchRegion(this.value);"></select>
                                                                                Region</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px; height: 22px;" align="left" class="cell_caption_left">
                                                                                <input id="chkAllOverseas" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('ddOverseasBranches','chkAllOverseas');" />
                                                                                Select All</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td class="cell_caption" style="height: 37px; width: 477px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td align="center" class="cell_caption_center" style="height: 16px; color: White;
                                                                                background: #336699">
                                                                                PNB Maintaining Branch
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px; height: 23px;" align="left" class="cell_caption_left">
                                                                                &nbsp;<select style="width: 50%; float: none; display: none;" id="Batek"></select></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px; height: 22px;" align="left" class="cell_caption_left">
                                                                                <input id="chkAllDomestic" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('ddDomesticBranches','chkAllDomestic');" />All
                                                                                Branches (check to include non pnb)</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 50%">
                                                                    <select name="Remitting_Branch" size="7" multiple="multiple" id="ddOverseasBranches"
                                                                        style="width: 100%">
                                                                    </select>
                                                                </td>
                                                                <td align="left" style="width: 50%">
                                                                    <select name="Crediting_Branch" size="7" multiple="multiple" id="ddDomesticBranches"
                                                                        style="width: 100%">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="height: 37px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td class="cell_caption_center" style="height: 15px; color: White; background: #336699">
                                                                                Product Code
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px" align="left" class="cell_caption_left">
                                                                                <input id="chkAllProduct" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('lstProduct','chkAllProduct');" />
                                                                                Select All</td>
                                                                        </tr>
                                                                    </table>
                                                                    <td class="cell_caption" style="height: 37px; width: 477px;">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td class="cell_caption_center" style="height: 15px; color: White; background: #336699;">
                                                                                    Transaction Source
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 375px; height: 19px;" align="left" class="cell_caption_left">
                                                                                    <input id="chkAllTranxSource" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('lstTranxSource','chkAllTranxSource');" />
                                                                                    Select All</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 50%; height: 111px;">
                                                                    <select name="ProductCode" size="7" multiple="multiple" id="lstProduct" style="width: 100%">
                                                                    </select>
                                                                </td>
                                                                <td align="left" style="width: 50%; height: 111px;">
                                                                    <select name="TranxSource" size="7" multiple="multiple" id="lstTranxSource" style="width: 100%">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="height: 37px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td class="cell_caption_center" style="height: 16px; color: White; background: #336699">
                                                                                Pay Mode
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px" align="left" class="cell_caption_left">
                                                                                <input id="chkAllPayMode" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('lstPayMode','chkAllPayMode');" />
                                                                                Select All</td>
                                                                        </tr>
                                                                    </table>
                                                                    <td class="cell_caption" style="height: 37px; width: 477px;">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td class="cell_caption_center" style="height: 16px; color: White; background: #336699;">
                                                                                    Transaction Status</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 375px" align="left" class="cell_caption_left">
                                                                                    <input id="chkAllTranxStatus" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('lstTranxStatus','chkAllTranxStatus');" />
                                                                                    Select All</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 50%">
                                                                    <select name="PayMode" size="7" multiple="multiple" id="lstPayMode" style="width: 100%">
                                                                    </select>
                                                                </td>
                                                                <td align="left" style="width: 50%">
                                                                    <select name="TranxStatus" size="7" multiple="multiple" id="lstTranxStatus" style="width: 100%">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="height: 37px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td class="cell_caption_center" style="height: 16px; color: White; background: #336699">
                                                                                Biller/Merchant
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px" align="left" class="cell_caption_left">
                                                                                <input id="chkAllMerchants" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('lstMerchants','chkAllMerchants');" />
                                                                                Select All</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td class="cell_caption">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td class="cell_caption_center" style="height: 16px; color: White; background: #336699;">
                                                                                States</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 375px; text-align: left; height: 19px;" align="left" class="cell_caption_left">
                                                                            <input id="chkAllStates" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('lstStates','chkAllStates');" />
                                                                                Select All</td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" style="width: 50%;">
                                                                    <select name="Merchants" multiple="multiple" size="7" id="lstMerchants" style="width: 100%;">
                                                                    </select>
                                                                </td>
                                                                <td align="left" style="width: 50%">
                                                                    <select name="States" multiple="multiple" size="7" style="width: 100%" id="lstStates">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <table cellspacing="0" cellpadding="0" style="width: 100%; height: 25%;">
                                                            <tr>
                                                                <td class="cell_caption_center" style="height: 16px; color: White; background: #336699;"
                                                                    colspan="3">
                                                                    Filter</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" style="height: 64px">
                                                                    <table width="100%" >
                                                                        <tr>
                                                                            <td style="width: 25%; height: 24px;" class="cell_caption_left">
                                                                                &nbsp;Remitter ID
                                                                            </td>
                                                                            <td style="width: 25%; height: 24px;" class="cell_caption_center">
                                                                                <input id="txtRemitterID" type="text" style="width: 85%" maxlength="16" />&nbsp;</td>
                                                                            <td style="width: 25%; height: 24px;" class="cell_caption_left">
                                                                                &nbsp;Beneficiary ID</td>
                                                                            <td style="width: 25%; height: 24px;" class="cell_caption_left">
                                                                                <input id="txtBeneficiaryID" type="text" style="width: 85%" maxlength="16" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 25%; height: 18px;" class="cell_caption_left">
                                                                                &nbsp;Account Number</td>
                                                                            <td style="width: 25%; height: 18px;" class="cell_caption_center">
                                                                                <input id="txtAccountNo" type="text" style="width: 85%" maxlength="25" />&nbsp;</td>
                                                                            <td style="width: 25%; height: 18px;" class="cell_caption_left">
                                                                                &nbsp;Merchant Member ID</td>
                                                                            <td style="width: 25%; height: 18px;" class="cell_caption_left">
                                                                                <input id="txtMerchMemId" type="text" style="width: 85%" maxlength="25" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption_left" style="width: 25%">
                                                                                &nbsp;Field Criteria&nbsp;</td>
                                                                            <td class="cell_caption_center" style="width: 25%">
                                                                                <select style="width: 87%" id="ddFieldCriteria">
                                                                                </select></td>
                                                                            <td style="width: 25%" class="cell_caption_left">
                                                                            </td>
                                                                            <td style="width: 25%" class="cell_caption_left">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption_center" colspan="3" id="tddaterange" style="height: 18px;
                                                        color: White; background: #336699">
                                                        Date Range</td>
                                                </tr>
                                                <tr>
                                                    <td align="left" class="cell_caption_center" style="height: 9px; width: 269px;">
                                                        Filter:&nbsp;
                                                        <select id="ddDateFilter" style="width: 84%">
                                                        </select>
                                                    </td>
                                                    <td align="left" class="cell_caption_center" style="width: 215px; height: 9px;">
                                                        From:
                                                        <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtDateFrom, 'yyyy-mm-dd',document.getElementById('divmain'));">
                                                        <input id="txtDateFrom" style="text-align: left; width: 65%;" type="text" disabled="disabled" /></td>
                                                    <td align="left" class="cell_caption_center" style="width: 226px; height: 9px;">
                                                        To:
                                                        <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtDateTo, 'yyyy-mm-dd',document.getElementById('divmain'));">
                                                        <input id="txtDateTo" style="text-align: left; width: 73%;" type="text" disabled="disabled" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption_center" style="width: 268px; height: 24px;">
                                                        Currency</td>
                                                    <td class="cell_caption_center" style="width: 215px; height: 24px;">
                                                        Computation</td>
                                                    <td class="cell_caption_center" style="width: 226px; height: 24px;">
                                                        &nbsp;<select id="ddAmtComputation" style="width: 75%">
                                                            <option selected="selected"></option>
                                                        </select></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption_left" style="width: 268px">
                                                        Remitting :&nbsp;
                                                        <select id="ddRemittingCCY" style="width: 66%">
                                                        </select>
                                                    </td>
                                                    <td class="cell_caption_center" style="width: 215px">
                                                        Condition</td>
                                                    <td class="cell_caption_center" style="width: 226px">
                                                        <select id="ddAmtFilter" style="width: 75%">
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption_left" style="width: 268px">
                                                        Target : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <select id="ddTargetCCY" style="width: 67%">
                                                        </select>
                                                    </td>
                                                    <td class="cell_caption_center" style="width: 215px">
                                                        Amount</td>
                                                    <td class="cell_caption_center" style="width: 226px">
                                                        <input id="txtAmount" style="text-align: right; width: 75%;" type="text" value="0.00"
                                                            maxlength="12">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 63px">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td colspan="2" class="cell_caption_center" style="color: White; background: #336699">
                                                                    Field Selection</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 50%" class="cell_caption_center">
                                                                    Choices</td>
                                                                <td style="width: 50%" class="cell_caption_center">
                                                                    Selected</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 45%; height: 174px;">
                                                                    <select name="FieldChoices" multiple="multiple" id="ddFieldChoices" style="width: 100%;
                                                                        height: 153px;" size="7">
                                                                    </select>
                                                                </td>
                                                                <td style="width: 10%; height: 174px;">
                                                                    <table style="width: 100%" class="cell_caption_center">
                                                                        <tr>
                                                                            <td>
                                                                                <input id="btnMoveAllRight" type="button" value=">>" onclick="javascript:report_builder.moveitemsAll('ddFieldChoices','ddFieldSelected');"
                                                                                    style="width: 55px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <input id="btnMoveRight" type="button" value=">" onclick="javascript:report_builder.moveitems('ddFieldChoices','ddFieldSelected');"
                                                                                    style="width: 55px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <input id="btnMoveLeft" type="button" value="<" onclick="javascript:report_builder.moveitems('ddFieldSelected','ddFieldChoices');"
                                                                                    style="width: 55px" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <input id="btnMoveAllLeft" type="button" value="<<" onclick="javascript:report_builder.moveitemsAll('ddFieldSelected','ddFieldChoices');"
                                                                                    style="width: 55px" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 45%; height: 174px;">
                                                                    <select name="Field_Selected" size="7" multiple="multiple" id="ddFieldSelected" style="width: 100%;
                                                                        height: 153px;">
                                                                    </select>
                                                                    <div id="divPopProcess" style="width: 400px; right: 200px; color: Black; padding: 10px;
                                                                        display: none; font-family: Arial; width: 300px; height: 150px; position: absolute;
                                                                        z-index: 99;">
                                                                        <table style="width: 100%; height: 100%; display: block;">
                                                                            <tr>
                                                                                <td style="width: 296px">
                                                                                    <label id="lblLoadingMsg" style="text-align: center;" class="label2">
                                                                                    </label>
                                                                                    <div id="divLoadingMsg">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="divExplore" style="width: auto; border: solid 1px #336699; display: none;
                                                                        position: absolute; z-index: 101;" class="ds_gray">
                                                                        <table style="width: auto;" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td style="width: 108px; height: 30px; font: bold 12px Arial; color: Ivory; color: White;
                                                                                    background: #336699" class="navbar" align="left">
                                                                                    &nbsp;&nbsp;Input Parameters</td>
                                                                                <td class="navbar" align="right" style="width: 15px; height: 30px;">
                                                                                    <input type="button" value="x" onclick="close_explore();" style="width: 15px; color: red"
                                                                                        class="table_caption" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" colspan="2" style="height: auto;">
                                                                                    <div id="pnlParameters" style="width: auto; height: auto;">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" colspan="2" style="height: 28px">
                                                                                    <input id="btnGenerateOFW" style="width: auto" type="button" value="Generate" onclick="javascript:report_builder.generateOFWReport();" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="4" style="height: 48px">
                                                        <table style="width: 100%; height: 22px">
                                                            <tr>
                                                                <td style="width: 100%;" colspan="3" class="cell_caption_center">
                                                                    <input id="btnReset" type="button" value="RESET" style="width: 15%" onclick="javascript:report_builder.initializepage();" />
                                                                    <input id="btnParse" type="button" value="PARSE" style="width: 15%" onclick="javascript:report_builder.ParseData();" />
                                                                    <input id="btnPreview" type="button" value="PREVIEW" style="width: 15%" disabled="disabled"
                                                                        onclick="javascript:report_builder.PreviewData();" />
                                                                    <input id="btnExtract" type="button" value="EXTRACT" style="width: 15%" disabled="disabled"
                                                                        onclick="javascript:report_builder.ExtractReport();" />
                                                                    <input id="btnSave" type="button" value="SAVE" style="width: 15%" disabled="disabled"
                                                                        onclick="javascript:report_builder.PreSaveReportProfile(document.getElementById('txtProfileName').value);" />
                                                                    <input id="btnSaveAs" type="button" value="SAVE AS" style="width: 15%" onclick="javascript:report_builder.PreSaveReportProfileAs();" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <div id="divSaveRptAs" style="width: auto; display: none; position: absolute; z-index: 99;
                                                            color: Ivory" align="center" class="ds_gray">
                                                            <table style="width: 425px; height: 100%; border: solid 2px midnightblue;">
                                                                <tr>
                                                                    <td style="width: 100%; color: White; background: #336699;" colspan="1" class="cell_caption_center">
                                                                        Save report profile as:
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;" colspan="1" class="cell_caption_center">
                                                                        <input id="txtSaveRptAs" type="text" style="width: 99%" onfocus="this.select()" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%;" colspan="1" class="cell_caption_center">
                                                                        <input id="btnSaveRptAsOk" type="button" value="Ok" style="width: 25%" onclick="javascript:report_builder.SaveReportProfileAs(document.getElementById('txtSaveRptAs').value)" />
                                                                        <input id="btnSaveRptAsCancel" type="button" value="Cancel" style="width: 25%" onclick="javascript:report_builder.CancelSaveReportProfileAs();" />
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
                            </td>
                        </tr>
                    </table>
                </div>
            </form>
        </div>
    </center>
</body>
</html>
