<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OTCTemplate.aspx.cs" Inherits="IRSWorld.Template.OTCTemplate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OTC Remittance [Template]</title>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/Template/jsOTCTemplate.js" type="text/javascript"></script>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var OTC_page = new OTC_page();
    </script>
</head>
<body onload="javascript:OTC_page.inititalizePage();">
    <form id="form1" runat="server">
    <div id="divHelp" style="display:none;position:absolute;border-style:solid;background-color:white;padding:5px;width:200px; left: 928px; top: 9px;"><span id="spnHelp" class="label2"></span></div>
    <div id="divSearchGrid" style="z-index:101;position:absolute;left:26px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 578px;width:700px;height:330px;">
        <div id="divSearchGridTop" style="display:block;">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center"></td>
                </tr>
            </table>
        </div>
        <br />
        <div id="divSearchGridBottom" style="display:block;">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <input id="btnGSearch" class="button" onclick="OTC_page.showSearch('divSearchGrid', 'divSearch');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back to Search" style="width:150px" />
                        <input id="btnGClose" class="button" onclick="OTC_page.showSearch('divSearchGrid', '');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Close" style="width:150px" />
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td align="center">
                        <span id="spnLoading"></span>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divSearch" style="z-index:101;position:absolute;left:800px;color:darkblue;border:solid 2px #000;padding:10px;font-family:Arial;top:572px;width:350px;height:215px;display:none;">
        <table cellpadding="0" cellspacing="0" style="width: 350px">
            <tr>
                <td style="width: 802px; height: 194px">
                    <table cellpadding="0" cellspacing="0" width="350px">
                        <tr>
                            <td align="left" valign="bottom" style="width:0%; border-collapse:collapse; height: 24px;"><img src="../images/toe.GIF" /></td>    
                            <td class="table_head_dark" style="width: 50%; height: 24px;">
                                SEARCH</td>
                            <td align="left" style="width: 100%; height: 24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table border="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                    <tr>
                                        <td style="width:250px;" class="cell_caption">
                                            Search By
                                        </td>
                                        <td style="width:450px;" align="left">
                                            <select id="ddlSearchBy" class="ddl" onchange="javascript:OTC_page.getDDL('ddlSearchBy', 'lblCIFClass', true);" style="width:99%" onkeydown="javascript:OTC_page.callSearch();"></select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:250px;" class="cell_caption">
                                            CIF ID
                                        </td>
                                        <td style="width:450px;" align="left">
                                            <input id="txtSCIFID" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:OTC_page.callSearch();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:250px;" class="cell_caption">
                                            Last Name
                                        </td>
                                        <td style="width:450px;" align="left">
                                            <input id="txtSLastName" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:OTC_page.callSearch();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:250px;" class="cell_caption">
                                            First Name
                                        </td>
                                        <td style="width:450px;" align="left">
                                            <input id="txtSFirstName" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:OTC_page.callSearch();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:250px;" class="cell_caption">
                                            Middle Name
                                        </td>
                                        <td style="width:450px;" align="left">
                                            <input id="txtSMiddleName" style="width:99%;text-align:left;" type="text" value="" onkeydown="javascript:OTC_page.callSearch();" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <input id="btnSearch" class="button" onclick="javascript:OTC_page.searchLink('Search', false);" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search" style="width:100px" />
                                            <input id="btnCloseSearch" class="button" onclick="javascript:OTC_page.hidePanel('divSearch');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Close" style="width:100px" />
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
    <div>
        <table width="100%" cellpadding="0" cellspacing="5">
            <tr>
                <td style="vertical-align:top; width:20%; height: 100%;"><!--menu-->
                    <div id="divOTC" style="display:block;">
                         <table width="100%" border="0" cellpadding="0" cellspacing="5">
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="javascript:OTC_page.searchLink('Search', true);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Search Remitter/Beneficiary</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="javascript:OTC_page.searchLink('ChargeType', false);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Charge Type</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="javascript:OTC_page.searchLink('Payment', false);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Payment Method</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="javascript:OTC_page.searchLink('BeneficiaryAccounts', false);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Beneficiary Accounts</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Transaction Source</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; height: 20px;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">KYC Representative</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Override Exchange Rates</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Waive Charges</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Denominations</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; height: 20px;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Add Remitter Cards</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Add Remitter Accounts</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Add Beneficiary</a>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td align="center" style="vertical-align:top; width:80%; height:100%;">
                    <div id="divTransaction" style="display:block;">
                        <table cellpadding="0" cellspacing="0" width="750px">
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; border-collapse:collapse; height: 24px;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px;">
                                    REMITTANCE INFORMATION</td>
                                <td align="left" style="width: 100%; height: 24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                        <tr>
                                            <td colspan="4" class="table_caption" align="center">
                                                <span id="spnExchangeRate" class="label2">Today's Exchange Rate: </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Amount</a>
                                            </td>
                                            <td colspan="2" style="width:450px;" align="left">
                                                <input id="txtAmount" style="width:50%;text-align:right;" type="text" value="0.00" onkeydown="javascript:OTC_page.getComputation();" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Currency</a>
                                            </td>
                                            <td colspan="2" style="width:450px;" align="left">
                                                <select id="ddlSourceCurrency" class="ddl" onchange="javascript:OTC_page.getExchangeRate();" style="width:50%"></select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Computation Type</a>
                                            </td>
                                            <td colspan="2" style="width:450px;" align="left">
                                                <select id="ddlComputation" class="ddl" onchange="javascript:OTC_page.getDDL('ddlComputation', 'lblComputation', true);" style="width:50%"></select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Transaction Charge</a>
                                            </td>
                                            <td colspan="2" style="width:500px;" align="left">
                                                <span id="spnCharge" style="width:50%;text-align:right;" class="label2">0.00</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Amount to be received (Beneficiary)</a>
                                            </td>
                                            <td colspan="2" style="width:500px;" align="left">
                                                <span id="spnBenefAmount" style="width:50%;text-align:right;" class="label2">0.00</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Peso Equivalent</a>
                                            </td>
                                            <td colspan="2" style="width:500px;" align="left">
                                                <span id="spnPHPAmount" style="width:50%;text-align:right;" class="label2">0.00</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Charge Type</a>
                                            </td>
                                            <td colspan="2" style="width:500px;" align="left">
                                                <span id="spnChargeType" style="width:50%;text-align:right;" class="label2"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Mode of Transaction</a>
                                            </td>
                                            <td colspan="2" style="width:500px;" align="left">
                                                <span id="spnProductCode" style="width:50%;text-align:right;" class="label2"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="width:250px;" class="cell_caption">
                                                <a>Special Message</a>
                                            </td>
                                            <td colspan="2" style="width:500px;" align="left">
                                                <input id="txtSpecialMessage" style="width:99%;text-align:left;" type="text" value="" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />
                    <div id="divRemitterBeneficiaryInformation" style="display:block;">
                        <table cellpadding="0" cellspacing="0" width="800px">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="350px">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%; border-collapse:collapse; height: 24px;"><img src="../images/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="width: 50%; height: 24px;">REMITTER</td>
                                            <td align="left" style="width: 100%; height: 24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Remitter Name</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnRemitterName" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Address</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnRemitterAddress" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Country</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnRemitterCountry" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Zip/Postal Code</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnRemitterZipCode" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a onmouseover="javascript:OTC_page.showHelp('PM');" onmouseout="javascript:OTC_page.hideHelp();">Payment Method</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnPaymentMethod" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a><span id="spnPM1">Card No.</span></a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtPM1" style="width:99%;text-align:left;border-top-style:none;border-right-style:none;border-left-style:none;border-bottom-style:none;" readonly="readonly" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a><span id="spnPM2">Card Expiry</span></a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtPM2" style="width:99%;text-align:left;border-top-style:none;border-right-style:none;border-left-style:none;border-bottom-style:none;" readonly="readonly" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a><span id="spnPM3">Currency</span></a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtPM3" style="width:99%;text-align:left;border-top-style:none;border-right-style:none;border-left-style:none;border-bottom-style:none;" readonly="readonly" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a><span id="spnPM4">Card Class</span></a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtPM4" style="width:99%;text-align:left;border-top-style:none;border-right-style:none;border-left-style:none;border-bottom-style:none;" readonly="readonly" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" style="height: 43px">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="center" width="30%">
                                                                        <i><span id="spnRemitterCount" class="label2">0 of 0</span></i></td>
                                                                    <td align="right" width="70%">
                                                                        <input id="btnRPrevious" onclick="javascript:OTC_page.getCIF('Remitter', false);" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Previous" style="width:100px" />
                                                                        <input id="btnRNext" onclick="javascript:OTC_page.getCIF('Remitter', true);" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Next" style="width:100px" />                                                                        
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0" width="350px">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%; border-collapse:collapse; height: 24px;"><img src="../images/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="width: 50%; height: 24px;">BENEFICIARY</td>
                                            <td align="left" style="width: 100%; height: 24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Beneficiary Name</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryName" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Address</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryAddress" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Country</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryCountry" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Zip/Postal Code</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryZipCode" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Maintaining Branch</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryMaintainingBranch" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Account No.</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryAccountNo" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Account Type</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryAccountType" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Currency</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryAccountCurrency" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;height:20px;" class="cell_caption">
                                                            <a>Other Banks</a>
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <span id="spnBeneficiaryOtherBanks" style="width:50%;text-align:right;" class="label2"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="center" width="30%">
                                                                        <i><span id="spnBeneficiaryCount" class="label2">0 of 0</span></i></td>
                                                                    <td align="right" width="70%">
                                                                        <input id="btnBPrevious" onclick="javascript:OTC_page.getCIF('Beneficiary', false);" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Previous" style="width:100px" />
                                                                        <input id="btnBNext" onclick="javascript:OTC_page.getCIF('Beneficiary', true);" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Next" style="width:100px" />                                                                        
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    <br />
                    <div id="divMessage" style="display:none;">
                    <table cellpadding="0" cellspacing="0" width="830px">
                        <tr>
                            <td>
                                <input id="txtMessage" type="text" style="color:Red;height:100px;" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    </div>
                        <input id="btnSubmit" class="button" onclick="" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Send Money Now" style="width:200px" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="divHidden">
        <input id="lblBeneficiaryAcctSeqID" type="hidden" />
        <input id="lblBeneficiaryID" type="hidden" />
        <input id="lblLocation" type="hidden" />
        <input id="lblCIFClass" type="hidden" />
        <input id="lblChargeType" type="hidden" />
        <input id="lblComputation" type="hidden" />
        <input id="lblImplementationCode" type="hidden" />
        <input id="lblPaymentMethod" type="hidden" />
        <input id="lblProductCode" type="hidden" />
        <input id="lblRemitterAcctSeqID" type="hidden" />
        <input id="lblRemitterID" type="hidden" />
        <input id="lblSourceCurrency" type="hidden" />
        <input id="lblSourceForex" type="hidden" />
        <input id="lblTargetForex" type="hidden" />
        <input id="lblTargetCurrency" type="hidden" />
    </div>
    </form>
</body>
</html>
