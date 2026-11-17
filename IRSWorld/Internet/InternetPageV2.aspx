<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetPageV2.aspx.cs" Inherits="IRSWorld.Internet.InternetPageV2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Agent Remittance Transaction</title>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsRemExternals.js" type="text/javascript"></script>
    <script src="../js/Internet/jsInternetPageV2_min.js" type="text/javascript"></script>
    <script src="../js/popcalendar_min.js" type="text/javascript"></script>
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var internet_page = new internet_page();
        var agentRem = new agent_remittance();
    </script>
</head>
<body onload="javascript:internet_page.fixdim();internet_page.initializePage();">
    <form id="form1" runat="server">
        <div id="divPage" style="overflow: auto;">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td style="vertical-align:top;height:auto;text-align:center; margin-left:auto; margin-right:auto; width:100%;">
                        <div id="divSearch" style="z-index:75;position:absolute;left:100px;color:darkblue;border:solid 2px #000;padding:10px;font-family:tahoma;top:10px;width:auto;height:auto;">
                        <table cellpadding="0" cellspacing="0" style="width:1024px;">
                            <tr>
                                <td style="height:auto;width:1024px;">
                                    <table cellpadding="0" cellspacing="0" style="width:1024px;">
                                        <tr>
                                            <td style="vertical-align:bottom;text-align:left;width:800px;" colspan="3">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width:0%; border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>
                                                        <td class="table_head_dark" style="height:auto;width:550px;">
                                                            CUSTOMER SEARCH</td>
                                                        <td style="height:auto;text-align:left;vertical-align:bottom;width:250px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width:1024px;">
                                                <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:1024px;">
                                                    <tr>
                                                        <td style="width:100px;" class="cell_caption">
                                                            Last Name
                                                        </td>
                                                        <td style="text-align:left;width:200px;">
                                                            <input id="txtSLastName" style="text-align:left;width:190px;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                        <td style="height:auto;width:100px;" class="cell_caption">
                                                            First Name
                                                        </td>
                                                        <td style="height:auto; text-align:left;width:200px;">
                                                            <input id="txtSFirstName" style="text-align:left;width:190px;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                        <td style="width:100px;" class="cell_caption">
                                                            Middle Name
                                                        </td>
                                                        <td style="text-align:left;width:200px;">
                                                            <input id="txtSMiddleName" style="text-align:left; width:190px;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr style="text-align:center;">
                                                        <td colspan="6">
                                                            <table style="width:1024px;">
                                                                <tr>
                                                                    <td nowrap style="height:auto;width:140px;"><input id="btnSearch" class="button" onclick="javascript:internet_page.searchCIF(false);" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="REMITTER SEARCH" style="width:150px;" /></td>
                                                                    <td style="width:130px;">&nbsp;</td>
                                                                    <td style="width:130px;">&nbsp;</td>
                                                                    <td style="width:130px;">&nbsp;</td>
                                                                    <td style="width:130px;">&nbsp;</td>
                                                                    <td nowrap style="height:auto;width:140px;"><input id="btnAddCIF" class="button" onclick="javascript:show_frame_dialog('../Internet/InternetRemitter.aspx?isRemit=true', 'internetClose(null)', '100%', '100%', '0');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="ADD NEW REMITTER" style="width:150px;" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSearchResults" style="display:none;">
                                                        <td colspan="6">
                                                            <table style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid; border-bottom: #394397 3px solid;height:auto;width:1024px;">
                                                                <tr>
                                                                    <td colspan="6">
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr style="text-align:center;">
                                                                    <td colspan="6">
                                                                        <div id="divSearchResult"></div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="5" style="width:900px;">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td><input id="btnCloseSearch" class="button" onclick="javascript:internet_page.clearSearch();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="CLOSE" style="width:150px;" /></td>
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
                        </div>
                        <div id="divTransaction" style="display:none;width:100%;">
                            <table cellpadding="0" cellspacing="0" style="width:100%;">
                                <tr>
                                    <td style="text-align:left;margin-left:auto;margin-right:auto;">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width:0%; border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                                <td class="table_head_dark" style="height:auto;width:60%;">TRANSACTION SCREEN</td>
                                                <td style="height:auto;text-align:left;vertical-align:bottom;width:40%;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                            </tr>
                                        </table>                                
                                    </td>                                
                                </tr>
                                <tr>
                                    <td style="margin-left:auto;margin-right:auto;">
                                        <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:100%">
                                            <tr>
                                                <td style="margin-left:auto;margin-right:auto; width: 50%;">
                                                    <table style="width:100%">
                                                        <tr>
                                                            <td style="width:100%">
                                                                <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:100%">
                                                                    <tr>
                                                                        <td colspan="3" class="table_caption" style="color: white; text-indent: 5px;text-align:center;height:25px;width:100%;">REMITTER</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Name:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnRemitterName" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Address:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnRemitterAddress" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Occupation:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnRemitterOccupation" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Date of Birth:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnRemitterDOB" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            ID:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnRemitterIds" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" colspan="2">&nbsp;
                                                                        </td>
                                                                        <td style="width:70%; text-align:right;">
                                                                            <input id="btnEditRemitter" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="EDIT REMITTER" style="width:140px;" onclick="javascript:internet_page.createSession('ARSEDITREMITTER');" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:100%;">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%">
                                                                <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:100%">
                                                                    <tr>
                                                                        <td colspan="3" class="table_caption" style="color: white; text-indent: 5px;text-align:center;height:25px;width:100%;">BENEFICIARY</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Name:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnBeneficiaryName" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Address:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnBeneficiaryAddress" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Zip / Postal Code:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnBeneficiaryZipCode" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Date of Birth:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnBeneficiaryDOB" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" class="cell_caption">
                                                                            Contact No:
                                                                        </td>
                                                                        <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                            <span id="spnBeneficiaryContactNo" class="label2"></span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:30%;" colspan="2">&nbsp;
                                                                        </td>
                                                                        <td style="width:70%; text-align:right;">
                                                                            <input id="btnEditBeneficiary" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="EDIT BENEFICIARY" style="width:140px;" onclick="javascript:internet_page.createSession('ARSEDITBENEFICIARY');" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>                                        
                                                </td>
                                                <td style="margin-left:auto;margin-right:auto;width:50%">
                                                    <table style="width:100%">
                                                        <tr>
                                                            <td>
                                                                <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;">
                                                                    <tr>
                                                                        <td colspan="3" class="table_caption" style="color: white; text-indent: 5px;text-align:center;height:25px;width:40%;">REMITTANCE DETAILS</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Product:
                                                                        </td>
                                                                        <td colspan="2" style="width:25%;">
                                                                            <select id="ddlBeneficiaryProduct" class="ddl" style="width:97%;" onchange="javascript:internet_page.changeBeneProduct();"></select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;">
                                                                            <input id="btnAddProduct" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="ADD PRODUCT" style="width:97%;" onclick="javascript:internet_page.createSession('ARSADDPRODUCT');" />
                                                                        </td>
                                                                        <td colspan="2" style="width:25%;">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" style="width:40%;">&nbsp;</td>
                                                                    </tr>
                                                                    <tr id="trAgent5" style="display:none;">
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Transaction Source:
                                                                        </td>
                                                                        <td colspan="2" style="width:25%;">
                                                                            <select id="ddlTranxSource" class="ddl" style="width:97%;"></select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trAgent6" style="display:none;">
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Special Message:
                                                                        </td>
                                                                        <td colspan="2" style="width:25%;">
                                                                            <input id="txtSpecialMessage" type="text" style="width:97%;" maxlength="512" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" style="width:40%;">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Remitting Currency:
                                                                        </td>
                                                                        <td style="width:12%;" class="cell_caption">
                                                                            Charge Type:
                                                                        </td>
                                                                        <td style="width:12%;" class="cell_caption">
                                                                            Pay Via:
                                                                        </td>
                                                                    </tr>                                                                    
                                                                    <tr>
                                                                        <td style="width:15%;">
                                                                            <select id="ddlRemittingCurrency" class="ddl" style="width:97%;"></select>
                                                                        </td>
                                                                        <td style="width:12%;">
                                                                            <select id="ddlChargeType" class="ddl" style="width:97%;"></select>
                                                                        </td>
                                                                        <td style="width:12%;">
                                                                            <select id="ddlPaymentMethod" class="ddl" style="width:97%;"></select>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="3" style="width:40%;">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Purpose of Remittance:
                                                                        </td>
                                                                        <td style="width:12%;">
                                                                            <select id="ddlPurpose" class="ddl" style="width:97%;"></select>
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <span id="spnCExchangeRateCurrency" class="label2"></span><input id="txtCExchangeRate" style="width:50%;" type="text" value="0.00" maxlength="15" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Amount to be Sent:
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <span id="spnCNetAmountCurrency" class="label2"></span>
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <input id="txtCNetAmount" type="text" value="0.00" maxlength="15" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Remittance Fee:
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <span id="spnCBankChargeAmountCurrency" class="label2"></span>
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <input id="txtCBankChargeAmount" type="text" value="0.00" maxlength="13" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Total Cost:
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <span id="spnCGrossAmountCurrency" class="label2"></span>
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <input id="txtCGrossAmount" type="text" value="0.00" maxlength="15" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:15%;" class="cell_caption">
                                                                            Amount to be Received:
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <span id="spnCNetToBenefAmountCurrency" class="label2"></span>
                                                                        </td>
                                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                            <input id="txtCNetToBenefAmount" type="text" value="0.00" maxlength="15" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>                                                            
                                                                        <td style="width:15%; text-align:right;">
                                                                            <input id="btnCompute" class="button" disabled="disabled" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="COMPUTE" style="width:97%;" onclick="javascript:internet_page.getCharges(false);" />
                                                                        </td>
                                                                        <td style="width:12%; text-align:right;">
                                                                            <input id="btnOverride" class="button" disabled="disabled" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="OVERRIDE" style="width:97%;" onclick="javascript:internet_page.setOverride();" />
                                                                        </td>
                                                                        <td style="width:12%; text-align:right;">
                                                                            <input id="btnSubmit" class="button" disabled="disabled" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="REMIT" style="width:97%;" onclick="javascript:internet_page.rem_aggregates();" />
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
                                <tr><td style="text-align:center;margin-left:auto;margin-right:auto;"><span id="spnHitMessage" style="font-weight:bold;color:Red;width:100%"></span></td></tr>
                                <tr><td style="text-align:center;margin-left:auto;margin-right:auto;"><span id="spnHitMessage2" style="font-weight:bold;color:Red;width:100%"></span></td></tr>
                            </table>
                        </div>
                        
                        <div id="divRemitterInfo" style="z-index:75;position:absolute;left:191px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:500px;height:auto;">
                            <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width:800px;">
                                <tr>
                                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Remitter CIFID:
                                    </td>
                                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:250px;"><span id="spnRemInfoCIFID" class="label2" style="width:85px;"></span></td>
                                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Remitter Name:
                                    </td>
                                    <td class="table_caption" style="vertical-align:bottom;text-align:left;width:250px;"><span id="spnRemInfoRemitterName" class="label2" style="width:85px;"></span></td>
                                </tr>
                                <tr>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        BirthDate:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoBirthdate" type="text" style="width:200px;" maxlength="13" readonly="readonly" />
                                    <img id="imgBirthDate" src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtRemInfoBirthdate, 'mm/dd/yyyy',document.getElementById('divRemitterInfo'));"/><label id="mnBirthDate" style="color:Red; display:none">*</label></td>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Source Of Funds:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoSourceOfFunds" class="ddl" style="width:200px"></select><label id="mnSourceOfFunds" style="color:Red; display:none">*</label></td>                                
                                </tr>
                                <tr>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Occupation:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoOccupation" class="ddl" style="width:200px"></select><label id="mnOccupation" style="color:Red; display:none">*</label></td>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Detailed Occupation:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoDetailedOccupation" type="text" style="width:200px;" maxlength="50" /></td>                                
                                </tr>                               
                                 <tr>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Beneficiary:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><span id="spnRemInfoRemBeneficiaryName" class="label2" style="width:85px;"></span></td>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Relationship:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoRelationship" class="ddl" style="width:200px"></select><label id="mnRelationship" style="color:Red; display:none">*</label></td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table>
                                            <tr>
                                                <td style="vertical-align:bottom;text-align:left;width:200px;">&nbsp;</td>
                                                <td style="vertical-align:bottom;text-align:center;width:200px;">                                                
                                                    <input id="btnRemitterInfoCancel" type="button" value="Cancel" style="width:180px;" disabled="disabled" onclick="javascript:agentRem.editSysControl_RemitterInfo()" />
                                                </td>
                                                <td style="vertical-align:bottom;text-align:center;width:200px;">
                                                    <input id="btnRemitterInfoSave" type="button" value="Save" style="width:180px;" disabled="disabled" onclick="javascript:agentRem.saveSysControl_RemitterInfo()" />
                                                </td>
                                                <td style="vertical-align:bottom;text-align:left;width:200px;">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width:800px;">
                                            <tr>
                                                <td class="table_caption" style="vertical-align:bottom;text-align:left;" colspan = "4">Remitter IDs</td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                                    ID type:
                                                </td>
                                                <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoIDType" class="ddl" style="width:200px" disabled="disabled"></select></td>
                                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                                    ID No:
                                                </td>
                                                <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoIDNo" type="text" style="width:200px;" maxlength="50" disabled="disabled"/></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                                    Expiry Date:
                                                </td>
                                                <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoExpiryDate" type="text" style="width:200px;" maxlength="50" readonly="readonly" />&nbsp;
                                                <img id="imgIDExpiryDate" src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtRemInfoExpiryDate, 'mm/dd/yyyy',document.getElementById('divRemitterInfo'));" disabled="disabled"/></td>
                                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                                    ID Classification:
                                                </td>
                                                <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoIDClass" class="ddl" style="width:200px" disabled="disabled"></select></td>
                                            </tr>
                                            <tr>
                                                <td colspan = "4"><div id="divGridIds"></div></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <table>
                                            <tr>
                                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoAddID" type="button" value="Add" style="width:150px;" onclick="javascript:agentRem.addSystemControl_RemitterIDs()" /></td>
                                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoEditID" type="button" value="Edit" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.editSystemControl_RemitterIDs()"/></td>
                                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoSaveID" type="button" value="Save" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.saveSystemControl_RemitterIDs()"/></td>
                                                <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemInfoCancelID" type="button" value="Cancel" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.cancelSystemControl_RemitterIDs()"/></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        
                        <div id="divRemitterIds" style="z-index:75;position:absolute;left:191px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:800px;height:auto;">
                            <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width:800px;">
                                <tr>
                                    <td class="table_caption" style="vertical-align:bottom;text-align:left;" colspan = "4">Remitter IDs</td>
                                </tr>
                                <tr>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        ID type:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemIDsType" class="ddl" style="width:200px" disabled="disabled"></select></td>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        ID No:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemIDsNo" type="text" style="width:200px;" maxlength="50" disabled="disabled"/></td>
                                </tr>
                                <tr>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Expiry Date:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemIDsExpDate" type="text" style="width:200px;" maxlength="50" readonly="readonly" />&nbsp;
                                    <img id="imgRemIDsExpDate" src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtRemIDsExpDate, 'mm/dd/yyyy',document.getElementById('divRemitterIds'));" disabled="disabled"/></td>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        Place of Issuance:
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemIDsPOI" type="text" style="width:200px;" maxlength="50" disabled="disabled"/></td>
                                </tr>
                                <tr id="trRemIDs" style="display:none;">
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                        ID Classification
                                    </td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemIDsClassification" class="ddl" style="width:200px" disabled="disabled"></select></td>
                                    <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">&nbsp;</td>
                                    <td style="vertical-align:bottom;text-align:left;width:250px;">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan = "4"><div id="dvRemIDsGrid"></div></td>
                                </tr>
                                <tr>
                                    <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemIDsAdd" type="button" value="Add" style="width:150px;" onclick="javascript:agentRem.add_RemitterIDs()" /></td>
                                    <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemIDsEdit" type="button" value="Edit" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.edit_RemitterIDs()"/></td>
                                    <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemIDsSave" type="button" value="Save" style="width:150px;" disabled="disabled" onclick="javascript:agentRem.save_RemitterIDs()"/></td>
                                    <td style="vertical-align:bottom;text-align:center;width:200px;"><input id="btnRemIDsCancel" type="button" value="Cancel" style="width:150px;" onclick="javascript:agentRem.cancel_RemitterIDs()"/></td>
                                </tr>
                            </table>
                        </div>
                        <div id="divPopProcess" style="height:200px;background:White;z-index:75;position:absolute;left:1072px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:12px;">
                            <table style="height:100%;width:350px;">
                                <tr>
                                    <td>
                                        <label id="lblLoadingMsg" style="text-align:center;color:Chocolate"></label>
                                            <div id="divLoadingMsg">
                                            </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="divHitList" style="height:auto;background:White;z-index:75;position:absolute;left:200px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:980px;">
                            <table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
                                <tr>
                                    <td class = "cell_desc" colspan="2" style="text-align: center"><label>CLIENT HIT LIST</label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align: center" ><div id = "div_client_hit_list_all"></div></td>
                                </tr>
                               <tr>
                                   <td colspan="2" style="text-align: center">
                                    <input id = "btn_client_hit_list_ok_all" type = "button" value = "OK" style="width: 100px; height: 25px" onclick="javascript:internet_page.hidePanel('divHitList');internet_page._isHit=false;" />&nbsp;<input id = "btn_client_hit_list_cancel_all" type = "button" value = "CANCEL" style="width: 100px; height: 25px" onclick="javascript:internet_page.hidePanel('divHitList');internet_page.initializePage();" />
                                   </td>
                               </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height:17px;vertical-align:middle; width: 582px;"><center><span id="spnLoading"></span></center></td>
                </tr>
            </table>        
        </div>
    </form>
</body>
</html>
