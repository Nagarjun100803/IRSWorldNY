<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ARSW.aspx.cs" Inherits="IRSWorld.ARS.ARSW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Agent Remittance Transaction</title>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <script src="../js/ARS/jsARSW.js" type="text/javascript"></script>
    <script src="../js/popcalendar_min.js" type="text/javascript"></script>
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var ars_page = new ars_page();
    </script>
</head>
<body onload="javascript:ars_page.initialize();">
    <form id="form1" runat="server">
    <div id="divPage" style="overflow: auto;">
        <table style="width:100%" cellpadding="0" cellspacing="5">
            <tr>
                <td style="vertical-align:top;height:534px;text-align:center; margin-left:auto; margin-right:auto; width:100%;">
                    <div id="divTransaction" style="width:100%;">
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
                                                                        <span id="spnRName" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        Address:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnRAddress" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        Occupation:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnROccupation" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        Date of Birth:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnRDOB" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        ID:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnRID" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" colspan="2">&nbsp;
                                                                    </td>
                                                                    <td style="width:70%; text-align:right;">
                                                                        <input id="btnEditRemitter" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="EDIT REMITTER" style="width:140px;" onclick="javascript:ars_page.showLink('REMITTER')"; />
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
                                                                        <span id="spnBName" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        Address:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnBAddress" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        Zip / Postal Code:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnBZipCode" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        Date of Birth:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnBDOB" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" class="cell_caption">
                                                                        Tel No:
                                                                    </td>
                                                                    <td colspan="2" style="background-color:aliceblue;width:70%;">
                                                                        <span id="spnBContactNo" class="label2"></span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:30%;" colspan="2">&nbsp;
                                                                    </td>
                                                                    <td style="width:70%; text-align:right;">
                                                                        <input id="btnEditBeneficiary" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="EDIT BENEFICIARY" style="width:140px;"  onclick="javascript:ars_page.showLink('BENEFICIARY')"; />
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
                                                                    <td style="text-align: center;" class="cell_caption">
                                                                        PRODUCT</td>
                                                                    <td colspan="2" style="width:25%;">
                                                                        <select id="ddlProduct" class="ddl" style="width:97%;" onchange="javascript:ars_page.getDDLAjax('ddlProduct');"></select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:15%; height: 22px;">
                                                                        <input id="btnAddProduct" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="ADD PRODUCT" style="width:97%;"  onclick="javascript:ars_page.showLink('PRODUCT')"; />
                                                                    </td>
                                                                    <td colspan="2" style="width:25%; height: 22px;">&nbsp;</td>
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
                                                                    <td style="width:15%; font-weight: bold; text-align: center;" class="cell_caption">
                                                                        Remitting Currency:
                                                                    </td>
                                                                    <td style="width:12%; font-weight: bold; text-align: center;" class="cell_caption">
                                                                        Charge Type:
                                                                    </td>
                                                                    <td style="width:12%; font-weight: bold; text-align: center;" class="cell_caption">
                                                                        Pay Via:
                                                                    </td>
                                                                </tr>                                                                    
                                                                <tr>
                                                                    <td style="width:15%;">
                                                                        <select id="ddlRemittingCurrency" class="ddl" style="width:97%;" onchange="javascript:ars_page.getDDLAjax('ddlRemittingCurrency');"></select>
                                                                    </td>
                                                                    <td style="width:12%;">
                                                                        <select id="ddlChargeType" class="ddl" style="width:97%;" onchange="javascript:ars_page.getDDLAjax('ddlChargeType');"></select>
                                                                    </td>
                                                                    <td style="width:12%;">
                                                                        <select id="ddlPaymentMethod" class="ddl" style="width:97%;" onchange="javascript:ars_page.getDDLAjax('ddlPaymentMethod');"></select>
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
                                                                        <select id="ddlPurpose" class="ddl" style="width:97%;" onchange="javascript:ars_page.getDDLAjax('ddlPurpose');"></select>
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <span id="spnCExchangeRateCurrency" class="label2"></span><input id="txtForex" style="width:50%;" type="text" value="0.00" maxlength="15" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:15%;" class="cell_caption">
                                                                        Amount to be Sent:
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <span id="spnNetCurrency" class="label2"></span>
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <input id="txtNetAmount" type="text" value="0.00" maxlength="15" onkeyup="javascript:return ars_page.preventsubmit(event);" onblur="javascript:ars_page.setComputation('2');" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:15%;" class="cell_caption">
                                                                        Remittance Fee:
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <span id="spnChargeCurrency" class="label2"></span>
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <input id="txtChargeAmount" type="text" value="0.00" maxlength="13" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:15%;" class="cell_caption">
                                                                        Total Cost:
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <span id="spnGrossCurrency" class="label2"></span>
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <input id="txtGrossAmount" type="text" value="0.00" maxlength="15" onkeyup="javascript:return ars_page.preventsubmit(event);" onblur="javascript:ars_page.setComputation('1');" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width:15%;" class="cell_caption">
                                                                        Amount to be Received:
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <span id="spnNetToBenCurrency" class="label2"></span>
                                                                    </td>
                                                                    <td style="background-color:aliceblue;height:auto;text-align:right;width:12%;">
                                                                        <input id="txtNetToBenAmount" type="text" value="0.00" maxlength="15" onkeyup="javascript:return ars_page.preventsubmit(event);" onblur="javascript:ars_page.setComputation('3');" />
                                                                    </td>
                                                                </tr>
                                                                <tr>                                                            
                                                                    <td style="width:15%; text-align:right;">
                                                                        <input id="btnCompute" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="COMPUTE" style="width:97%;" onclick="javascript:ars_page.compute();" />
                                                                    </td>
                                                                    <td style="width:12%; text-align:right;">
                                                                        <input id="btnOverride" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="OVERRIDE" style="width:97%;" onclick="javascript:ars_page.override();" />
                                                                    </td>
                                                                    <td style="width:12%; text-align:right;">
                                                                        <input id="btnRemit" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="REMIT" style="width:97%;" onclick="javascript:ars_page.remit();" />
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
                    </div>
                    
                    <div id="divAggregate" style="height:auto;background:White;z-index:75;position:absolute;left:500px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:438px;">
                        <div id="divLoading"><center><IMG SRC='../images/ajaxloader.gif'></IMG></center></div>
                        <div id="divAggregateTop" style="display:block;"></div>
                        <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:100%">
                            <tr>
                                <td colspan="2" class="table_caption" style="color: white; text-indent: 5px;text-align:center;height:25px;width:100%;">
                                    REMITTANCE AGGREGATION</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div id="divAggregateGrid"></div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:50%"><input id="btnCancel" type="button" value="Cancel" class="button" onclick="javascript:ars_page.hidePanel('divAggregate');" /></td>
                                <td style="width:50%"><input id="btnContinue" type="button" value="Continue" class="button" onclick="javascript:ars_page.remitfinal();" /></td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divClearingDate" style="height:auto;background:White;z-index:75;position:absolute;left:755px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:601px;">
                        <table style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:500px;">
                            <tr>
                                <td align="right" style="width:30%;" class="cell_caption">Clearing Date:&nbsp;</td>
                                <td align="left" style="width:70%;"><input id="txtClearingDate" style="text-align:left; width:80%; vertical-align:middle;" type="text" />
                                        <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtClearingDate, 'yyyy-mm-dd',document.getElementById('divClearingDate'));" style="vertical-align:middle;"></td>
                            </tr>
                            <tr>
                                <td align="right" style="width:30%;" class="cell_caption">Check Number:&nbsp;</td>
                                <td align="left" style="width:70%;"><input id="txtCheckNumber" style="text-align:left; width:80%; vertical-align:middle;" type="text" /></td>
                            </tr>
                            <tr>
                                <td align="right" colspan="2"><input id="btnSubmitClearing" type="button" value="Submit" class="button" style="width:100px;" onclick="javascript:ars_page.submitClearingDate();" /></td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divPopProcess" style="height:200px;background:White;z-index:75;position:absolute;left:1013px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:13px;">
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
                    
                    <div id="divTransactionHistoryModal" style="z-index:75;position:absolute;left:7px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:574px;height:auto;">
                        <table style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:700px;">
                            <tr>
                                <td colspan="2" style="height:auto;width:700px;">
                                    <table style="" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td rowspan="3" style="width:100px;">
                                                <img src="../images/PNBLOGOembsmaller.png" />
                                            </td>
                                            <td style="height:auto;width:600px;text-align:center;">
                                                <b><span id="spnBranch"></span></b>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td style="FONT-SIZE:9px;height:auto;width:600px;text-align:center;"><b><span id="spnDescription"></span></b></td>
                                        </tr>
                                        <tr>
                                            <td style="FONT-SIZE:9px;height:auto;width:600px;text-align:center;">
                                                <table border="2" style="" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width:700px;"><b><span id="spnBranch1"></span></b></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="FONT-SIZE:9px;height:auto;background-color:paleturquoise;width:700px;">Email: <span id="spnEmail"></span></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="table_head_dark" style="width:700px;"><center><span id="spnPGResponse"></span></center></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="FONT-SIZE: 12px;">
                                    <table style="width:700px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height:auto;text-align:left;width:350px;"><b>Remitter's Information</b></td>
                                            <td style="height:auto;text-align:left;width:350px;"><b>Beneficiary's Information</b></td>
                                        </tr>
                                        <tr>
                                            <td style="background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;">
                                                <span id="spnTRemitterName"></span>
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;">
                                                <span id="spnTBeneficiaryName"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;">
                                                <span id="spnTRemitterAddress"></span>
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;">
                                                <span id="spnTBeneficiaryAddress"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height:auto;text-align:left;width:350px;">&nbsp;</td>
                                            <td style="background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;">
                                                <span id="spnTBeneficiaryAccount"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height:auto;text-align:left;width:350px;">&nbsp;</td>
                                            <td style="background-color:aliceblue;height:auto; FONT-SIZE: 9px;text-align:left;width:350px;">
                                                <span id="spnTBeneficiaryBranch"></span>
                                            </td>
                                        </tr>
                                        <tr><td colspan="2"><hr /></td></tr>
                                        <tr>
                                            <td colspan="2" style="height:auto;">
                                                <table style="" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">Reference No</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;:&nbsp;<span id="spnTReferenceNo"></span>
                                                        </td>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">Exchange Rate</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:88px;">
                                                            &nbsp;:&nbsp;<span id="spnTExchangeRateCurrency"></span>
                                                        </td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:87px;">
                                                            <span id="spnTExchangeRate">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">Transaction Date</td>
                                                        <td colspan="4" style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;:&nbsp;<span id="spnTTransactionDate">##/##/####</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">
                                                            Status</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;:&nbsp;<span id="spnTStatus"></span>
                                                        </td>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">
                                                            Total Cost</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:88px;">
                                                            &nbsp;:&nbsp;<span id="spnNetAmountCurrency"></span>
                                                        </td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:87px;">
                                                            <span id="spnTGrossAmount">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">Encoded By</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;:&nbsp;<span id="spnTEncodedBy"></span>
                                                        </td>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">
                                                            Service Fee</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:88px;">
                                                            &nbsp;:&nbsp;<span id="spnTBankChargeCurrency"></span>
                                                        </td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:87px;">
                                                            <span id="spnTBankCharge">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">Sent Via</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;:&nbsp;<span id="spnTSubType"></span>
                                                        </td>
                                                        <td class="cell_caption" style="text-align:right;width:175px;">
                                                            Net Amount</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:88px;">
                                                            &nbsp;:&nbsp;<span id="spnTGrossAmountCurrency"></span>
                                                        </td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:87px;">
                                                            <span id="spnNetAmount">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">Product Type</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;:&nbsp;<span id="spnTProductDescription"></span>
                                                        </td>
                                                        <td class="cell_caption" style="height:auto;text-align:right;width:175px;">
                                                            Amount to be Received</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:88px;"><b>
                                                            &nbsp;:&nbsp;<span id="spnTNetToBenefAmountCurrency"></span></b>
                                                        </td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:87px;"><b>
                                                            <span id="spnTNetToBenefAmount">0.00</span></b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height:auto;">
                                <td style="width:350px;">
                                    <span id="spnError"></span>
                                </td>
                                <td style="text-align:right;width:350px;">
                                    <table style="width:347px;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td nowrap style="height:22px; width:auto;">
                                                <input id="btnSendReceipt" type="button" class="button" value="Send thru Email" onclick="javascript:ars_page.sendReceiptEmail();" style="visibility:visible;width:110px;" />
                                                <input id="btnPrintPanel" type="button" class="button" value="Print" onclick="javascript:ars_page.printDiv('divTransactionHistoryModal');" style="visibility:visible;width:110px;" />
                                                <input id="btnClosePanel" type="button" class="button" value="Close" onclick="javascript:ars_page.closeTransactionHistoryDiv();" style="width:110px;" />
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
                <td style="height:17px;vertical-align:middle; width: 582px;"><center><span id="spnLoading"></span></center></td>
            </tr>
        </table>        
    </div>
    </form>
</body>
</html>