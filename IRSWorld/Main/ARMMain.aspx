<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ARMMain.aspx.cs" Inherits="IRSWorld.Main.ARMMain" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Automated Remittance Machine</title>
    <script src="../js/Main/ARM.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/Main/jsCalendar.js" type="text/javascript"></script>
    <link href="../css/ARM.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var arm_page = new arm_page();
    </script>
    <script language="vbscript" type="text/vbscript">
        Sub Print()
               OLECMDID_PRINT = 6
               OLECMDEXECOPT_DONTPROMPTUSER = 2
               OLECMDEXECOPT_PROMPTUSER = 1
               call WB.ExecWB(OLECMDID_PRINT, OLECMDEXECOPT_DONTPROMPTUSER, 1)
        End Sub
        document.write "<object ID='WB' WIDTH=0 HEIGHT=0 CLASSID='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2'></object>"
    </script>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
</head>
<body onload="arm_page.loadARMInfo(1)" onunload="" onclick="arm_page.setFocus();">
    <form id="form1" runat="server">
    <div class="background">
        <input id="txtCard" type="text" onkeyup="arm_page.submitCard();" class="textLarge" style="text-decoration: none; color: white; cursor: text; margin: 0px; border-top-style: none; border-right-style: none; border-left-style: none; border-bottom-style: none; background-color: transparent; width:0px; height:0px; left: -100px; position: absolute; top: -100px;" />
        <div id="divSecurity" style="position:absolute;z-index:101;top:204px;left:76px;display:none;"><img src="../images/cardsecurity.jpg" width="200" height="150" /></div>
        <div id="divReceipt" style="position:absolute;z-index:101;top:32px;left:449px;display:none;">
            <font color="191970">
                <table width="300px" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" rowspan="2" style="font-family:Arial Black; font-size:18px;"><b>PNB</b></td>
                        <td align="center" style="font-family:Arial Black; font-size:8px;"><b><u>REMITTANCE CENTERS, INC.</u></b></td>
                    </tr>
                    <tr>
                        <td align="center" style="font-family:Times New Roman; font-size:6px;"><i>A Philippine National Bank Affiliate</i></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2" style="font-family:Arial; font-size:8px;"><center><b><span id="spnRBranch">&nbsp;</span></b>&nbsp;</center></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Date</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRTransactionDate">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Reference Number</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRReferenceNo">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Remitter</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRRemitter">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Beneficiary</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRBeneficiary">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Card Number</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRCardNumber">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Authorization Code</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRAuthorizationCode">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Remittance Amount</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRRemittanceAmount">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Service Fee</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRServiceFee">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">TOTAL AMOUNT</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRTotalAmount">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Exchange Rate</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRExchangeRate">&nbsp;</span></td>
                    </tr>
                    <tr><td colspan="2" style="font-family:Arial; font-size:8px;">&nbsp;</td></tr>
                    <tr>
                        <td align="left" style="font-family:Arial; font-size:8px;">Amount to Beneficiary</td>
                        <td align="left" style="font-family:Arial; font-size:8px;"><span id="spnRAmountToBeneficiary">&nbsp;</span></td>
                    </tr>
                </table>
            </font>
        </div>
        
        <div id="divMerchantType" style="z-index:101;position:absolute;left:1274px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:383px;width:700px;height:440px;background-color:White;">
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="4" class="textMedium" align="center">Please enter payment information for <span id="spnMerchantName" class="textMedium"></span>:</td>
                </tr>
                <tr><td colspan="4" style="height:auto;"><hr /></td></tr>
                <tr>
                    <td colspan="4">
                        <div id="divMerchantDDL1" style="display:block;">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="30%">
                                        <span id="spnMerchantDDL1" class="textMedium"></span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="65%">
                                        <select id="ddlMerchantType1" class="ddl" style="width:480px;" onchange="arm_page.getDDL('ddlMerchantType1', 'lblMerchantType1', true);"></select>
                                    </td>
                                    <td width="4%">
                                        <span id="spnMerchantTypeAsterisk1" class="textMedium" style="color:Red;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divMerchantDDL2" style="display:none;">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="30%">
                                        <span id="spnMerchantDDL2" class="textMedium"></span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="65%">
                                        <select id="ddlMerchantType2" class="ddl" style="width:480px;" onchange="arm_page.getDDL('ddlMerchantType2', 'lblMerchantType2', true);"></select>
                                    </td>
                                    <td width="4%">
                                        <span id="spnMerchantTypeAsterisk2" class="textMedium" style="color:Red;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divMerchantDDL3" style="display:none;">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td width="30%">
                                        <span id="spnMerchantDDL3" class="textMedium"></span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="65%">
                                        <select id="ddlMerchantType3" class="ddl" style="width:480px;" onchange="arm_page.getDDL('ddlMerchantType3', 'lblMerchantType3', true);"></select>
                                    </td>
                                    <td width="4%">
                                        <span id="spnMerchantTypeAsterisk3" class="textMedium" style="color:Red;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divCalendar" style="display:none;">
                            <table width="100%">
                                <tr>
                                    <td width="20%" align="left">
                                        <span id="spnDateFrom" class="textMedium">Date From</span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="25%">
                                        <input id="txtDateFrom" type="text" value="Click here to edit" style="width:200px;" onclick="arm_page.checkCalendar('txtDateFrom');" />
                                    </td>
                                    <td width="4%">
                                        <span id="spnDateAsterisk1" class="textMedium" style="color:Red;"></span>
                                    </td>
                                    <td width="20%" align="left">
                                        <span id="spnDateTo" class="textMedium">Date To</span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="25%">
                                        <input id="txtDateTo" type="text" value="Click here to edit" style="width:200px;" onclick="arm_page.checkCalendar('txtDateTo');" />
                                    </td>
                                    <td width="4%">
                                        <span id="spnDateAsterisk2" class="textMedium" style="color:Red;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr><td colspan="4">&nbsp;</td></tr>
                <tr>
                    <td colspan="4">
                        <div id="divMerchantDesc1" style="display:none;">
                            <table width="100%">
                                <tr>
                                    <td width="30%">
                                        <span id="spnMerchantDetail1" class="textMedium"></span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="65%">
                                        <input id="txtMerchantDetail1" type="text" style="width:98%;" value="Click here to edit" onclick="arm_page.showQwerty('txtMerchantDetail1');" />
                                    </td>
                                    <td width="4%">
                                        <span id="spnMerchantDetailAsterisk1" class="textMedium" style="color:Red;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>    
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divMerchantDesc2" style="display:none;">
                            <table width="100%">
                                <tr>
                                    <td width="30%">
                                        <span id="spnMerchantDetail2" class="textMedium"></span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="65%">
                                        <input id="txtMerchantDetail2" type="text" style="width:98%;" value="Click here to edit" onclick="arm_page.showQwerty('txtMerchantDetail2');" />
                                    </td>
                                    <td width="4%">
                                        <span id="spnMerchantDetailAsterisk2" class="textMedium" style="color:Red;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divMerchantDesc3" style="display:none;">
                            <table width="100%">
                                <tr>
                                    <td width="30%">
                                        <span id="spnMerchantDetail3" class="textMedium"></span>
                                    </td>
                                    <td width="1%">:</td>
                                    <td width="65%">
                                        <input id="txtMerchantDetail3" type="text" style="width:98%;" value="Click here to edit" onclick="arm_page.showQwerty('txtMerchantDetail3');" />
                                    </td>
                                    <td width="4%">
                                        <span id="spnMerchantDetailAsterisk3" class="textMedium" style="color:Red;"></span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr><td colspan="4" style="height: 18px">&nbsp;</td></tr>
                <tr>
                    <td colspan="4">
                        <span id="spnQwerty">&nbsp;</span>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left" width="50%">
                        <span id="spnAsterisk" class="textMedium" style="color:red;">* Mandatory Field</span>
                    </td>
                    <td align="right" width="50%">
                        <input id="btnCancel" type="button" value="Cancel" class="btnSmall" onclick="arm_page.closeQwerty(true);" style="height: 50px" />
                        <input id="btnSubmit" type="button" value="Submit" class="btnSmall" onclick="arm_page.closeQwerty(false);" style="height: 50px" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="divModal" style="z-index:101;position:absolute;left:43px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:597px;width:800px;height:500px;">
            <table>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="header">
            <table width="100%">
                <tr>
                    <td style="width: 99%;" class="frame_body">
                        <div style="width: 100%"class="ds_gray">
                            <uc1:page_header id="Page_header2" runat="server">
                            </uc1:page_header>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td id="tdNavbar" style="width:100%;height: 25px;" class="navbar" align="left">
                        <table id="tblMenuBar" height="100%" border="0">
                        <tr>
                            <td width="72%" align="left" valign="middle"><marquee><span class="textHeader"><b>Automated Remittance Machine</b></span></marquee></td>
                            <td width="20%" align="right" valign="middle"><span id="spnClock" style="vertical-align:middle;" class="textHeader"></span>&nbsp;&nbsp;&nbsp;</td>
                            <td width="8%" align="right" valign="middle"><input id="btnQuit" type="button" value="Quit" class="btnSmall" onclick="arm_page.submitQuit();" /></td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="footer">
            <table width="100%" border="0">
                <tr>
                    <td colspan="2" valign="middle"><center><span id="spnLoading" class="textMedium" style="color:White;"></span></center></td>
                </tr>
                <tr>
                    <td align="left"><input id="btnBack" type="button" value="Back" class="btn" onclick="arm_page.submitBackContinue(arm_page.getPageID(false), false);" disabled="disabled" style="display:none;" /></td>
                    <td align="right"><input id="btnContinue" type="button" value="Continue" class="btn" onclick="arm_page.submitBackContinue(arm_page.getPageID(true), true);" style="display:none;"/></td>
                </tr>
            </table>
            <!--span id="spnFooter" class="textFooter"></span-->
        </div>
        
        <center>
        <div id="content" class="transbox">
            <div id="divPage1" style="width: 100%; display: block;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span class="textLarge">WELCOME</span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>                
                    <tr>
                        <td colspan="2"><span id="spnRemitterName" class="textMedium" /></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span class="textLarge">Today's Exchange Rate is</span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span id="spnExchangeRate" class="textMedium" /></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><a href="javascript:arm_page.submitRemittanceMerchant(true);"><img src="../images/Remittance.jpg" width="200" height="50" /></a></td>
                        <td><a href="javascript:arm_page.submitRemittanceMerchant(false);"><img src="../images/BillsPayment.jpg" width="200" height="50" /></a></td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage2" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2" style="height: 45px"><span class="textLarge">Please select your beneficiary:</span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><select id="ddlBeneficiary" class="ddl" onchange="arm_page.getDDL('ddlBeneficiary', 'lblBeneficiary', true);"></select></td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage3" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="3">&nbsp;</td></tr>
                    <tr>
                        <td colspan="3"><span id="spnAmount" class="textLarge"></span></td>
                    </tr>
                </table>
                <table border="4" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center">
                            <span id="spnTargetCurrency" class="textLarge"></span>
                        </td>
                        <td colspan="2" align="right">
                            <input id="txtAmount" type="text" value="0.00" class="textLarge" maxlength="10" style="text-align:right; width: 255px;" onblur="javascript:return(arm_page.autoFormatCurrency(this,',','.',event));"/>
                        </td>
                    </tr>
                    <tr>
                        <td><input id="btnOne" type="button" value="1" onclick="arm_page.calculate(1);" class="btnMedium" /></td>
                        <td><input id="btnTwo" type="button" value="2" onclick="arm_page.calculate(2);" class="btnMedium" /></td>
                        <td><input id="btnThree" type="button" value="3" onclick="arm_page.calculate(3);" class="btnMedium" /></td>
                    </tr>
                    <tr>
                        <td><input id="btnFour" type="button" value="4" onclick="arm_page.calculate(4);" class="btnMedium" /></td>
                        <td><input id="btnFive" type="button" value="5" onclick="arm_page.calculate(5);" class="btnMedium" /></td>
                        <td><input id="btnSix" type="button" value="6" onclick="arm_page.calculate(6);" class="btnMedium" /></td>
                    </tr>
                    <tr>
                        <td><input id="btnSeven" type="button" value="7" onclick="arm_page.calculate(7);" class="btnMedium" /></td>
                        <td><input id="btnEight" type="button" value="8" onclick="arm_page.calculate(8);" class="btnMedium" /></td>
                        <td><input id="btnNine" type="button" value="9" onclick="arm_page.calculate(9);" class="btnMedium" /></td>
                    </tr>
                    <tr>
                        <td><input id="btnPoint" type="button" value="." onclick="arm_page.calculate('.');" class="btnMedium" /></td>
                        <td><input id="btnZero" type="button" value="0" onclick="arm_page.calculate(0);" class="btnMedium" /></td>
                        <td><input id="btnClear" type="button" value="Clear" onclick="document.getElementById('txtAmount').value = '0.00';" class="btnMedium" /></td>
                    </tr>
                </table>
                <table width="100%" cellpadding="0" cellspacing="0">
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td><b><span class="textMedium">**Additional bank charge will be computed on top of the remitted amount.**</span></b></td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage4" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span class="textLarge">Please select a purpose for this remittance transaction:</span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><select id="ddlPurpose" class="ddl" onchange="arm_page.getDDL('ddlPurpose', 'lblPurpose', true);"></select></td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage5" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span class="textLarge">Please select your source of fund:</span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><select id="ddlTransactionSource" class="ddl" onchange="arm_page.getDDL('ddlTransactionSource', 'lblTransactionSource', true);"></select></td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage6" style="width: 100%; display: none;" onclick="arm_page.setFocus();">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span class="textLarge">Please swipe your <span id="spnCardType"></span>:</span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><input id="btnRefreshCard" type="button" value="Refresh" onclick="arm_page.setFocus();" /></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span class="textMedium">If card has no response, please click the refresh button and swipe your DEBIT/CREDIT card again.</span></td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage7" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="3">&nbsp;</td></tr>
                    <tr>
                        <td colspan="3"><span class="textMedium">Please enter your <span id="Span1"></span> security code:</span></td>
                    </tr>
                    <tr><td colspan="3">&nbsp;</td></tr>
                </table>
                <table border="4" cellpadding="0" cellspacing="0">
                    <tr><td colspan="3"><input id="txtSecurityCode" type="password" class="textLarge" maxlength="3" style="text-align:right; width:388px;" /></td></tr>
                    <tr>
                        <td><input id="btnSOne" type="button" value="1" onclick="arm_page.addValue(1, 'txtSecurityCode');" class="btnMedium" /></td>
                        <td><input id="btnSTwo" type="button" value="2" onclick="arm_page.addValue(2, 'txtSecurityCode');" class="btnMedium" /></td>
                        <td><input id="btnSThree" type="button" value="3" onclick="arm_page.addValue(3, 'txtSecurityCode');" class="btnMedium" /></td>
                    </tr>
                    <tr>
                        <td><input id="btnSFour" type="button" value="4" onclick="arm_page.addValue(4, 'txtSecurityCode');" class="btnMedium" /></td>
                        <td><input id="btnSFive" type="button" value="5" onclick="arm_page.addValue(5, 'txtSecurityCode');" class="btnMedium" /></td>
                        <td><input id="btnSSix" type="button" value="6" onclick="arm_page.addValue(6, 'txtSecurityCode');" class="btnMedium" /></td>
                    </tr>
                    <tr>
                        <td><input id="btnSSeven" type="button" value="7" onclick="arm_page.addValue(7, 'txtSecurityCode');" class="btnMedium" /></td>
                        <td><input id="btnSEight" type="button" value="8" onclick="arm_page.addValue(8, 'txtSecurityCode');" class="btnMedium" /></td>
                        <td><input id="btnSNine" type="button" value="9" onclick="arm_page.addValue(9, 'txtSecurityCode');" class="btnMedium" /></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td><input id="btnSZero" type="button" value="0" onclick="arm_page.addValue(0, 'txtSecurityCode');" class="btnMedium" /></td>
                        <td><input id="btnSClear" type="button" value="Clear" onclick="document.getElementById('txtSecurityCode').value = '';" class="btnMedium" /></td>
                    </tr>
                </table>
            </center>
            </div>

            <div id="divPage8" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span id="spnTransactionInfo" class="textMedium" style="display:none;">Transaction almost complete, Please verify transaction information below to continue:</span></td>
                    </tr>
                    <tr><td colspan="2"><hr />&nbsp;</td></tr>
                </table>
            </center>
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="10%">&nbsp;</td>
                    <td width="90%" align="left"><span id="spnTransaction" class="textSmall"></span></td></tr>
            </table>
            </div>
            
            <div id="divPage9" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span class="textMedium">Your transaction was successful with reference number:</span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2"><b><span id="spnReferenceNo" class="textMedium"></span></b></td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2"><hr />&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2"><span class="textMedium">Would you like to print a receipt?</span></td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td><input id="btnPrintYes" type="button" class="btnMedium" value="Yes" onclick="arm_page.submitPrint(true);" /></td>
                        <td><input id="btnPrintNo" type="button" class="btnMedium" value="No" onclick="arm_page.submitPrint(false);" /></td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage10" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2"><span class="textLarge">Thank you for using</span></td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2"><span class="textLarge">Philippine National Bank</span></td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2"><span class="textLarge">Automated Remittance System</span></td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2"><span class="textLarge">Goodbye!</span></td></tr>
                </table>
            </center>
            </div>
            
            <div id="divQwerty" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right">
                            <b><input type="button" value="X" onclick="javascript:arm_page.clearQwerty();" class="textSmall" style="width: 30px; height: 22px;" />&nbsp;</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" value="1" onclick="javascript:arm_page.submitLetter('1');" class="btnLetters" />
                            <input type="button" value="2" onclick="javascript:arm_page.submitLetter('2');" class="btnLetters" />
                            <input type="button" value="3" onclick="javascript:arm_page.submitLetter('3');" class="btnLetters" />
                            <input type="button" value="4" onclick="javascript:arm_page.submitLetter('4');" class="btnLetters" />
                            <input type="button" value="5" onclick="javascript:arm_page.submitLetter('5');" class="btnLetters" />
                            <input type="button" value="6" onclick="javascript:arm_page.submitLetter('6');" class="btnLetters" />
                            <input type="button" value="7" onclick="javascript:arm_page.submitLetter('7');" class="btnLetters" />
                            <input type="button" value="8" onclick="javascript:arm_page.submitLetter('8');" class="btnLetters" />
                            <input type="button" value="9" onclick="javascript:arm_page.submitLetter('9');" class="btnLetters" />
                            <input type="button" value="0" onclick="javascript:arm_page.submitLetter('0');" class="btnLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" value="Q" onclick="javascript:arm_page.submitLetter('Q');" class="btnLetters" />
                            <input type="button" value="W" onclick="javascript:arm_page.submitLetter('W');" class="btnLetters" />
                            <input type="button" value="E" onclick="javascript:arm_page.submitLetter('E');" class="btnLetters" />
                            <input type="button" value="R" onclick="javascript:arm_page.submitLetter('R');" class="btnLetters" />
                            <input type="button" value="T" onclick="javascript:arm_page.submitLetter('T');" class="btnLetters" />
                            <input type="button" value="Y" onclick="javascript:arm_page.submitLetter('Y');" class="btnLetters" />
                            <input type="button" value="U" onclick="javascript:arm_page.submitLetter('U');" class="btnLetters" />
                            <input type="button" value="I" onclick="javascript:arm_page.submitLetter('I');" class="btnLetters" />
                            <input type="button" value="O" onclick="javascript:arm_page.submitLetter('O');" class="btnLetters" />
                            <input type="button" value="P" onclick="javascript:arm_page.submitLetter('P');" class="btnLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" value="A" onclick="javascript:arm_page.submitLetter('A');" class="btnLetters" />
                            <input type="button" value="S" onclick="javascript:arm_page.submitLetter('S');" class="btnLetters" />
                            <input type="button" value="D" onclick="javascript:arm_page.submitLetter('D');" class="btnLetters" />
                            <input type="button" value="F" onclick="javascript:arm_page.submitLetter('F');" class="btnLetters" />
                            <input type="button" value="G" onclick="javascript:arm_page.submitLetter('G');" class="btnLetters" />
                            <input type="button" value="H" onclick="javascript:arm_page.submitLetter('H');" class="btnLetters" />
                            <input type="button" value="J" onclick="javascript:arm_page.submitLetter('J');" class="btnLetters" />
                            <input type="button" value="K" onclick="javascript:arm_page.submitLetter('K');" class="btnLetters" />
                            <input type="button" value="L" onclick="javascript:arm_page.submitLetter('L');" class="btnLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" value="Z" onclick="javascript:arm_page.submitLetter('Z');" class="btnLetters" />
                            <input type="button" value="X" onclick="javascript:arm_page.submitLetter('X');" class="btnLetters" />
                            <input type="button" value="C" onclick="javascript:arm_page.submitLetter('C');" class="btnLetters" />
                            <input type="button" value="V" onclick="javascript:arm_page.submitLetter('V');" class="btnLetters" />
                            <input type="button" value="B" onclick="javascript:arm_page.submitLetter('B');" class="btnLetters" />
                            <input type="button" value="N" onclick="javascript:arm_page.submitLetter('N');" class="btnLetters" />
                            <input type="button" value="M" onclick="javascript:arm_page.submitLetter('M');" class="btnLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" value="Backspace" onclick="javascript:arm_page.submitLetter('backspace');" class="btnLetters" style="width: 110px" />
                            <input type="button" value="Space" onclick="javascript:arm_page.submitLetter('space');" class="btnLetters" style="width: 110px" />
                            <input type="button" value="Clear" onclick="javascript:arm_page.submitLetter('clear');" class="btnLetters" style="width: 100px" />
                        </td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divLetters" style="width: 100%; display: none;">
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <input id="btnA" type="button" value="A" onclick="javascript:arm_page.submitLetter('A');" class="btnLetters" />
                            <input id="btnB" type="button" value="B" onclick="javascript:arm_page.submitLetter('B');" class="btnLetters" />
                            <input id="btnC" type="button" value="C" onclick="javascript:arm_page.submitLetter('C');" class="btnLetters" />
                            <input id="btnD" type="button" value="D" onclick="javascript:arm_page.submitLetter('D');" class="btnLetters" />
                            <input id="btnE" type="button" value="E" onclick="javascript:arm_page.submitLetter('E');" class="btnLetters" />
                            <input id="btnF" type="button" value="F" onclick="javascript:arm_page.submitLetter('F');" class="btnLetters" />
                            <input id="btnG" type="button" value="G" onclick="javascript:arm_page.submitLetter('G');" class="btnLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="btnH" type="button" value="H" onclick="javascript:arm_page.submitLetter('H');" class="btnLetters" />
                            <input id="btnI" type="button" value="I" onclick="javascript:arm_page.submitLetter('I');" class="btnLetters" />
                            <input id="btnJ" type="button" value="J" onclick="javascript:arm_page.submitLetter('J');" class="btnLetters" />
                            <input id="btnK" type="button" value="K" onclick="javascript:arm_page.submitLetter('K');" class="btnLetters" />
                            <input id="btnL" type="button" value="L" onclick="javascript:arm_page.submitLetter('L');" class="btnLetters" />
                            <input id="btnM" type="button" value="M" onclick="javascript:arm_page.submitLetter('M');" class="btnLetters" />
                            <input id="btnN" type="button" value="N" onclick="javascript:arm_page.submitLetter('N');" class="btnLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="btnO" type="button" value="O" onclick="javascript:arm_page.submitLetter('O');" class="btnLetters" />
                            <input id="btnP" type="button" value="P" onclick="javascript:arm_page.submitLetter('P');" class="btnLetters" />
                            <input id="btnQ" type="button" value="Q" onclick="javascript:arm_page.submitLetter('Q');" class="btnLetters" />
                            <input id="btnR" type="button" value="R" onclick="javascript:arm_page.submitLetter('R');" class="btnLetters" />
                            <input id="btnS" type="button" value="S" onclick="javascript:arm_page.submitLetter('S');" class="btnLetters" />
                            <input id="btnT" type="button" value="T" onclick="javascript:arm_page.submitLetter('T');" class="btnLetters" />
                            <input id="btnU" type="button" value="U" onclick="javascript:arm_page.submitLetter('U');" class="btnLetters" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="btnV" type="button" value="V" onclick="javascript:arm_page.submitLetter('V');" class="btnLetters" />
                            <input id="btnW" type="button" value="W" onclick="javascript:arm_page.submitLetter('W');" class="btnLetters" />
                            <input id="btnX" type="button" value="X" onclick="javascript:arm_page.submitLetter('X');" class="btnLetters" />
                            <input id="btnY" type="button" value="Y" onclick="javascript:arm_page.submitLetter('Y');" class="btnLetters" />
                            <input id="btnZ" type="button" value="Z" onclick="javascript:arm_page.submitLetter('Z');" class="btnLetters" />
                            <input id="btnLAll" type="button" value="All" onclick="javascript:arm_page.submitLetter('');" class="btnLetters" />
                            <input id="btnLClear" type="button" value="Clear" onclick="javascript:arm_page.submitLetter('clear');" class="btnLetters" />
                        </td>
                    </tr>
                </table>
            </center>
            </div>
            
            <div id="divPage11" style="width: 100%; display: none;"> <!--Merchant-->
            <center>
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><span id="spnMerchantSelect" class="textLarge"></span></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2"><select id="ddlMerchant" class="ddl" onchange="arm_page.validateMerchant();"></select></td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2" align="left">
                            <span id="spnMerchant" class="textMedium"></span>
                        </td>
                    </tr>
                    <tr><td colspan="2">&nbsp;</td></tr>
                    <tr>
                        <td colspan="2">
                            <span id="spnMerchantCount" class="textMedium"></span>
                        </td>
                    </tr>
                </table>
            </center>
            </div>
        </div>
        </center>
    </div>

    <input id="lblAccountNo" type="hidden" />    
    <input id="lblAmount" type="hidden" />
    <input id="lblAuthCode" type="hidden" />
    <input id="lblBenAcctSeqID" type="hidden" />
    <input id="lblBeneficiary" type="hidden" />
    <input id="lblBeneficiaryAddress" type="hidden" />
    <input id="lblBeneficiaryID" type="hidden" />
    <input id="lblBeneficiaryName" type="hidden" />
    <input id="lblBranchCode" type="hidden" />
    <input id="lblCardName" type="hidden" />
    <input id="lblCardType" type="hidden" />
    <input id="lblCharges" type="hidden" />
    <input id="lblChargeType" type="hidden" />
    <input id="lblComputationLevel" type="hidden" />    
    <input id="lblDBCards" type="hidden" />
    <input id="lblDCCard" type="hidden" />
    <input id="lblExpiryDate" type="hidden" />
    <input id="lblForex" type="hidden" />
    <input id="lblGFMCNo" type="hidden" />
    <input id="lblImplementationCode" type="hidden" />
    <input id="lblKioskCurrency" type="hidden" />
    <input id="lblLoanType" type="hidden" />
    <input id="lblLocalTime" type="hidden" />
    <input id="lblMerchant" type="hidden" />
    <input id="lblMerchantSeqID" type="hidden" />
    <input id="lblMerchantAcctSeqID" type="hidden" />
    <input id="lblMerchantID" type="hidden" />
    <input id="lblMerchantName" type="hidden" />
    <input id="lblMerchantProductCode" type="hidden" />
    <input id="lblMerchantType1" type="hidden" />
    <input id="lblMerchantType2" type="hidden" />
    <input id="lblMerchantType3" type="hidden" />
    <input id="lblORNo" type="hidden" />
    <input id="lblPageID" type="hidden" />
    <input id="lblPayMode" type="hidden" />
    <input id="lblPGMessage" type="hidden" />
    <input id="lblProductCode" type="hidden" />
    <input id="lblPurpose" type="hidden" />
    <input id="lblRemAcctSeqID" type="hidden" />
    <input id="lblRemitterID" type="hidden" />
    <input id="lblSecurityCode" type="hidden" />
    <input id="lblServerTime" type="hidden" />
    <input id="lblStatusDetail" type="hidden" />
    <input id="lblSourceCurrency" type="hidden" />
    <input id="lblSourceCurrencyCode" type="hidden" />
    <input id="lblSourceCurrencyShortCode" type="hidden" />
    <input id="lblTargetCurrency" type="hidden" />
    <input id="lblTargetCurrencyCode" type="hidden" />
    <input id="lblTargetCurrencyShortCode" type="hidden" />
    <input id="lblTransactionDate" type="hidden" />
    <input id="lblTransactionSEQID" type="hidden" />
    <input id="lblTransactionSource" type="hidden" />
    </form>
</body>
</html>