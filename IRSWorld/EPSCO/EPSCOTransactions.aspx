<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EPSCOTransactions.aspx.cs" Inherits="IRSWorld.EPSCO.EPSCOTransactions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EPSCO Transaction</title>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/EPSCO/jsEPSCOTransactions.js" type="text/javascript"></script>
    <script src="../js/EPSCO/jsEPSCOEnrollment.js" type="text/javascript"></script>
    <script src="../js/jsExpand.js" type="text/javascript"></script>
    <script src="../js/jsBalloon.js" type="text/javascript"></script>
    <script src="../js/jsImageResize.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
        var epsco_client = new epsco_client();    
        var epsco_trans = new epsco_trans();
           
        function execute_ajx_command(param)
        {            
            if (epsco_trans != null && epsco_trans != "undefined")
            {
                epsco_trans.do_ajax(param);          
            }
            if (epsco_client != null && epsco_client != "undefined")
            {
                epsco_client.do_ajax(param);          
            }              
        }

    </script>
</head>
<body onload="epsco_trans.page_initialize();">
    <form id="form1" runat="server">
    <div id="divPage" style="overflow: auto;">
        <table style="width:100%" cellpadding="0" cellspacing="5">
            <tr>
                <%--<td width="25%" style="vertical-align:top;height:auto;"><!--menu-->
                    <div id="divEPSCOMenu" style="display:block; border-right-style:groove; border-right-color:Navy;">
                         <table width="100%" cellpadding="0" cellspacing="5">
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;height:auto;width:300px;">
                                    <img src="../images/u.gif" name="imgfirst"><a href="#first" onClick="shoh('first');" >&nbsp;Transactions</a>
                                    <div style="display: none;" id="first" >
                                        <table width="100%">
                                            <tr id="trCreateTransaction">
                                                <td>
                                                    <a href="javascript:epsco_trans.createTransaction('create');" style="font-style:italic;">Create Transaction</a>
                                                </td>
                                            </tr>
                                            <tr id="trTransactionHistory">
                                                <td>
                                                    <a href="javascript:epsco_trans.createTransaction('history');" style="font-style:italic;">Transaction History</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;height:auto;width:300px;">
                                    <img src="../images/u.gif" name="imgsecond"><a href="#first" onClick="shoh('second');" >&nbsp;My Accounts</a>
                                    <div style="display: none;" id="second" >
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <a href="javascript:epsco_trans.showLinks('Password');" style="font-style:italic;">Change Password</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <a href="javascript:epsco_trans.showLinks('OAAM');" style="font-style:italic;">Change Image / Phrase</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <a href="javascript:epsco_trans.getTodayForex();" style="font-style:italic;">Update Foreign Exchange</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <a href="javascript:epsco_trans.showDisclaimer();" style="font-style:italic;">Show disclaimer</a>
                                                </td>
                                            </tr>
                                            <tr id="trAddCIF" style="display:none;">
                                                <td>
                                                    <a href="javascript:show_frame_dialog('../Internet/InternetRemitter.aspx', 'internetRemitterClose', '100%', '100%', '0');" style="font-style:italic;">Add Beneficiary/Cards</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trSearchCIF" style="display:none;">
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:auto;width:300px;">
                                    <a href="javascript:epsco_trans.searchCIF(true);" style="font-style:italic;">Search Client</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:auto;width:300px;">
                                    <a href="javascript:epsco_trans.showLinks('Logout');" style="font-style:italic;">Logout</a>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td style="background-image: url('../images/forex.jpg'); background-repeat: no-repeat; width: 100px; height:220px;">
                                                <table>
                                                    <tr><td>&nbsp;</td></tr>
                                                    <tr>
                                                        <td align="right">
                                                            <span id="spnForexDate" style="color:White;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <span id="spnForexValue"></span>
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
                </td>--%>
                <td align="center" style="vertical-align:top;height:846px; width: 73%;">
                    <div id="divPopProcess" style="width:400px;height:200px;background:White;z-index:75;position:absolute;left:1072px;color:darkblue;padding:10px;display:none;font-family:Arial;top:12px;">
                        <table style="width:100%; height:100%;">
                            <tr>
                                <td>
                                    <label id="lblLoadingMsg" style="text-align:center;color:Chocolate"></label>
                                        <div id="divLoadingMsg">
                                        </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divDisclaimerAlert" style="z-index:75;word-wrap:break-word;overflow:scroll;text-align:left;position:absolute;left:1072px;color:darkblue;border:solid 2px #000;padding:10px;font-family:Arial;top:1979px;width:701px;height:auto;display:none;">
                        <table border="0" style="width:100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><span id="spnDisclaimerAlert"></span></td>
                            </tr>
                            <tr>
                                <td align="right" style="height: 7px">
                                    <input id="btnDisclaimerClose" type="button" value="Close" onclick="javascript:epsco_trans.hidePanel('divDisclaimerAlert');" style="width: 100px;"/>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divRemitterName">
                        <table id="tblRemitterName" border="0" cellpadding="0" cellspacing="0" style="width:700px; display:none">
                            <tr><td>&nbsp;</td></tr>
                            <tr>
                                <td><span id="spnRemitterName" class="label4"></span></td>
                            </tr>
                            <tr><td>&nbsp;</td></tr>
                        </table>
                    </div>
                    
                    <div id="divTransactionHistory" style="display:none;">
                        <table style="width:700px" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" valign="bottom" style="width:0%;border-collapse:collapse;height:auto;"><img src="../images/toe.GIF" /></td>
                                <td class="table_head_dark" style="width:50%;height:auto;">Transaction History</td>
                                <td align="left" style="width:100%;height:auto;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>   
                            </tr>
                            <tr>
                                <td colspan="3" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid; border-bottom: #336699 3px solid;height:auto;">
                                    <div id="divRemitterTransaction">Transaction Grid Here</div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divTransaction" style="display:none;">
                        <table cellpadding="0" cellspacing="0" style="width:700px">
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:auto;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:50%;height:auto;">Remittance Process</td>
                                <td align="left" style="width:100%;height:auto;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>   
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table border="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                        <tr>
                                            <td colspan="4" class="table_caption" style="color: black; text-indent: 5px;text-align:left;height:25px;width:364px;">Remittance</td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:auto;">Remitter ID:&nbsp;</td>
                                            <td align="left" style="width:400px;height:auto;" colspan="2"><span class="label2" id = "spnRemitterID"></span></td>                                            
                                            <td style="width:100px;height:auto;"><input id="btnSearchRemitter" type="button" value="Search Remitter" style="width:100px;" onclick="showPanelEP('divSearch');" /></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:22px;">Beneficiary:&nbsp;</td>
                                            <td align="left" style="width:400px;height:22px;" colspan="2"><!--select id="ddlBeneficiary" class="ddl" onchange="javascript:epsco_trans.getBenefInfo();" style="width:202px;"></select-->
                                                <asp:Label ID="lblBeneficiary" CssClass="label2" runat="server" Width="398px" Height="20px"></asp:Label></td>
                                            <td style="width:100px;height:22px;"><input id="btnBeneficiary" type="button" value="Show Detail" onclick="javascript:showPanelEP('divBeneficiary');" style="width:100px;"/></td>
                                        </tr>
                                        <%--<tr>
                                            <td class="cell_caption" style="width:200px;height:auto;">Transaction Purpose:&nbsp;</td>
                                            <td align="left" style="width:400px;height:auto;" colspan="2"><select id="ddlPurpose" class="ddl" onchange="javascript:epsco_trans.getDDLAjax('ddlPurpose', 'lblPurpose', true);" style="width:400px;"></select></td>
                                            <td style="width:100px;height:auto;">&nbsp;</td>
                                        </tr>--%>
                                        <%--<tr>
                                            <td class="cell_caption" style="width:200px;height:auto;">Payment Method:&nbsp;</td>
                                            <td align="left" style="width:400px;height:auto;" colspan="2"><select id="ddlPaymentMethod" class="ddl" onchange="javascript:epsco_trans.getDDLAjax('ddlPaymentMethod', 'lblPaymentMethod', true);" style="width:400px;"></select></td>
                                            <td style="width:100px;">&nbsp;</td>
                                        </tr>--%>
                                        <%--<tr id="trOnline" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;">D/C Card/Account No.:&nbsp;</td>
                                            <td align="right" style="width:80px;height:auto;"><span id="spnSourceCurrency" class="label2"></span></td>
                                            <td align="left" style="width:325px;height:auto;"><select id="ddlDCCard" class="ddl" onchange="javascript:epsco_trans.getDDLAjax('ddlDCCard', 'lblDCCard', true);" style="width:319px;"></select></td>
                                            <td style="width:100px;">&nbsp;</td>
                                        </tr>--%>
                                        <%--<tr id="trAgent1" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;">Charge Type:&nbsp;</td>
                                            <td align="left" style="width:400px;height:auto;" colspan="2"><select id="ddlChargeType" class="ddl" onchange="javascript:epsco_trans.getDDLAjax('ddlChargeType', 'lblChargeType', true);" style="width:400px;"></select></td>
                                            <td style="width:100px;height:auto;">&nbsp;</td>
                                        </tr>--%>
                                        <%--<tr id="trAgent2" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;"><span>Computation</span>:&nbsp;</td>
                                            <td align="left" style="width:400px;height:auto;" colspan="2"><select id="ddlComputation" class="ddl" onchange="javascript:epsco_trans.getDDLAjax('ddlComputation', 'lblComputation', true);" style="width:400px;"></select></td>
                                            <td style="width:100px;height:auto;" align="left">
                                                <span style="padding-bottom:1px; border-bottom:1px dotted #900; cursor:pointer;" onmouseover="javascript:tooltip.show(epsco_trans.showBalloon('Computation'));" onmouseout="javascript:tooltip.hide();"><b>?</b></span>&nbsp;
                                            </td>
                                        </tr>--%>
                                        <tr id="trAgent3" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:20px;">Remitting Currency:&nbsp;</td>
                                            <td align="left" style="width:39px;height:20px;" id="lblRemittingCurr" class="label2"></td>
                                            <td align="left" style="width:200px;height:20px;"><input id="txtExchangeRate" type="text" style="width:252px;" readonly="readonly" /></td>
                                            <td style="width:100px; height: 20px;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:20px;">Amount:&nbsp;</td>
                                            <!--td align="right" style="width:29px;height:auto;"><span id="spnRemittingCurrency" class="label2"></span></td-->
                                            <td colspan="2" align="left" style="width:200px;height:20px;"><input id="txtAmount" type="text" value="0.00" onblur="" onkeypress="javascript:return(epsco_trans.autoFormatCurrency(this,',','.',event));" style="width:306px;" maxlength="13" /></td>
                                            <td style="width:100px; height: 20px;"><input id="btnCompute" type="button" value="Show Detail" onclick="javascript:showPanelEP('divCharges');javascript:epsco_trans.initCDet();" style="width:100px;"/></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:20px;">
                                                Amount to be Received:&nbsp;</td>
                                            <td align="left" style="width:400px;height:20px;" colspan="2"><span id="spnNetToBeneAmount" class="label2"></span></td>
                                            <td style="width:100px; height: 20px;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height:22px;">
                                                &nbsp;<input id="btnSubmit" class="button" onclick="javascript:epsco_trans.checkTransaction();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Submit Transaction" style="width: 200px" disabled="disabled" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr><tr><td colspan="3">&nbsp;</td></tr>
                        </table>
                    </div>
                    
                    <div id="divReadMe" style="display:none;">
                        <table cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td align="left" valign="bottom" style="width:0%;border-collapse:collapse;height:auto;"><img src="../images/toe.GIF" /></td>
                                <td class="table_head_dark" style="width:50%;height:1px;">IMPORTANT REMINDER</td>
                                <td align="left" style="width:100%;height:auto;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>   
                            </tr>
                            <tr>
                                <td colspan="3" style="width:100%">
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;width:100%;">
                                        <tr>
                                            <td align="left" class="cell_caption" style="text-align:left;"><span id="spnReminder" style="text-align:left;"></span></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr><td colspan="3">&nbsp;</td></tr>
                        </table>
                    </div>
                                        
                    <div id="divTransactionInfo" style="display:none;">
                        <table cellpadding="0" cellspacing="0" style="width:700px">
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:11px;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:50%;height:11px;">Transaction Information</td>
                                <td align="left" style="width:100%;height:11px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>    
                            </tr>
                            <tr>
                                <td colspan="3" style="width:100%; height: 92px;">
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;height:auto;width:100%">
                                        <tr>
                                            <td class="table_caption" colspan="4" style="width:125px;color:black;text-indent:5px;text-align:left;height:25px;">
                                                Remittance
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:auto;">Reference No.:&nbsp;</td>
                                            <td align="left" style="width:25%;height:auto;">
                                                <input id="spnReferenceNo" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                            <td class="cell_caption" style="width:25%;height:auto;">Target Account:&nbsp;</td>
                                            <td align="left" style="width:25%;height:auto;">
                                                <input id="spnTIBeneficiaryAccount" type="text" readonly="readonly" style="width: 96%" />
                                            </td>                                        
                                        </tr>                                                    
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:auto;">Beneficiary Name:&nbsp;</td>
                                            <td align="left" style="width:75%;height:auto;" colspan="3">
                                                <input id="spnTIBeneficiaryName" type="text" readonly="readonly" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:auto;">Beneficiary Address:&nbsp;</td>
                                            <td align="left" style="width:80%;height:auto;" colspan="3">
                                                <input id="spnTIBeneficiaryAddress" type="text" readonly="readonly" style="width: 99%" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="width: 25%; height:auto;">Mode of Payment:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;">
                                                <input id="spnTIProductCode" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                            <td class="cell_caption" style="width: 25%; height:auto;"><span id="spnPM1"></span>:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;">
                                                <input id="spnTIDCCard" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="width: 25%; height:auto;">
                                                Gross Amount:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;">
                                                <input id="spnTIGrossAmount" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                            <td class="cell_caption" style="width: 25%; height:auto;"><span id="spnPM2"></span>:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;">
                                                <input id="spnTIExpiryDate" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 25%; height:auto;">Charges:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;">
                                                <input id="spnTICharges" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                            <td class="cell_caption" style="width: 25%; height:auto;"><span id="spnPM3"></span>:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;" >
                                                <input id="spnTICardName" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="width: 25%; height:auto;">
                                                Amount to be Received:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;">
                                                <input id="spnTINetToBenefAmount" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                            <td class="cell_caption" style="width: 25%; height:auto;">Exchange Rate Used:&nbsp;</td>
                                            <td align="left" style="width: 25%; height:auto;">
                                                <input id="spnTIForex" type="text" readonly="readonly" style="width: 96%" />
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td class="table_caption" colspan="4" style="width: 125px; color: black; text-indent: 5px;text-align: left; height:25px;">
                                                Payment Gateway
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 25%; height:auto">Authentication Code:&nbsp;</td>
                                            <td align="left" style="width: 75%; height:auto;" colspan="3">
                                                <input id="spnTIAuthCode" type="text" readonly="readonly" style="width: 99%" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="width: 25%; height:auto;">
                                                Response:&nbsp;</td>
                                            <td align="left" style="width: 75%; height:auto;" colspan="3">
                                                <input id="spnTIResponse" type="text" readonly="readonly" style="width: 99%" />
                                            </td>
                                        </tr>--%>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <%--<div id="divDisclaimer" style="width:90%;height:auto;word-wrap:break-word;overflow:scroll;text-align:left;border:solid 1px midnightblue;display:none;">
                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="height: 17px"><span id="spnDisclaimer"></span></td>
                            </tr>
                            <tr>
                                <td style="height:auto"><input id="chkDisclaimer" type="checkbox" onclick="javascript:epsco_trans.checkDisclaimer();" style="width:20px; border:none;" /><span class="label2">I have read and agree to the above terms and conditions</span></td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="btnAccept" type="button" value="Accept" onclick="javascript:epsco_trans.acceptDisclaimer(true);" style="width: 100px;"/>
                                    <input id="btnDecline" type="button" value="Decline" onclick="javascript:epsco_trans.acceptDisclaimer(false);" style="width: 100px;" />
                                </td>
                            </tr>
                        </table>
                    </div>--%>
                    
                    <div id="divHidden">
                        <table border="0" style="width:100%" cellpadding="0" cellspacing="0">
                            <tr> <!--hidden-->
                                <td style="height: 17px">
                                    <input id="lbl3DSecure" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lbl3DSecureURL" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblAgentID" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblAuthCode" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblBenAcctSeqID" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblBeneficiaryID" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblBeneficiaryInfo" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblCIFClass" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblChargeType" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblComputation" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblDCCard" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblForex" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblImplementationCode" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblLocalTime" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblLocation" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblORNo" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblPaymentMethod" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblPGMessage" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblProductCode" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblPurpose" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblReferenceNo" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemAcctSeqID" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemitterID" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemittingCurrency" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblServerTime" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblSourceCurrency" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblStatusDetail" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblTargetCurrency" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblTransactionDate" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblTransactionSEQID" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblUserID" type="hidden" style="width:1px; height:1px;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divTransactionHistoryModal" style="z-index:75;position:absolute;left:1072px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:575px;width:800px;height:auto;">
                        <table style="width:100%; border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                            <tr>
                                <td colspan="2" style="height:auto;">
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td rowspan="3" style="width: 10%">
                                                <img src="../images/PNBLOGOembsmaller.png" /> <!--onmouseover="javascript:sli(this,250,198,506,400);" onmouseout="javascript:ssi(this,250,198,506,400);" /-->
                                            </td>
                                            <td style="height:auto;width:90%">
                                                <b>Philippine National Bank&nbsp;<span id="spnBranch"></span></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="FONT-SIZE:9px;height:auto;"><b><span id="spnDescription"></span></b></td>
                                        </tr>
                                        <tr>
                                            <td style="FONT-SIZE:9px;height:auto;">
                                                <table border="2" style="width:100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><b><span id="spnBranch1"></span></b></td>
                                                        <td><b><span id="spnBranch2"></span></b></td>
                                                        <td><b><span id="spnBranch3"></span></b></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="FONT-SIZE:9px;height:auto;background-color:paleturquoise;">Email: <span id="spnEmail"></span></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="table_head_dark" style="width: 50%;"><center><span id="spnPGResponse"></span></center></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="FONT-SIZE: 12px; height: 305px;">
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="height:auto;width:50%"><b>Remitter's Information</b></td>
                                            <td align="left" style="height:auto;width:50%"><b>Beneficiary's Information</b></td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="background-color:aliceblue;height:auto;width:50%;FONT-SIZE: 9px;">
                                                <span id="spnTRemitterName"></span>
                                            </td>
                                            <td align="left" style="background-color:aliceblue;height:auto;width:50%;FONT-SIZE: 9px;">
                                                <span id="spnTBeneficiaryName"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="background-color:aliceblue;height:auto;width:50%;FONT-SIZE: 9px;">
                                                <span id="spnTRemitterAddress"></span>
                                            </td>
                                            <td align="left" style="background-color:aliceblue;height:auto;width:50%;FONT-SIZE: 9px;">
                                                <span id="spnTBeneficiaryAddress"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height:auto;width:50%">&nbsp;</td>
                                            <td align="left" style="background-color:aliceblue;height:auto;width:50%;FONT-SIZE: 9px;">
                                                <span id="spnTBeneficiaryAccount"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="height:auto;width:50%">&nbsp;</td>
                                            <td align="left" style="background-color:aliceblue;height:auto; width:50%;FONT-SIZE: 9px;">
                                                <span id="spnTBeneficiaryBranch"></span>
                                            </td>
                                        </tr>
                                        <tr><td colspan="2"><hr /></td></tr>
                                        <tr>
                                            <td colspan="2" style="height:191px;">
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="width: 22%; height:auto;">Reference No</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:30%">
                                                            &nbsp;:&nbsp;<span id="spnTReferenceNo"></span>
                                                        </td>
                                                        <td class="cell_caption" align="left" style="height:auto;width:22%">Exchange Rate</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:12%;">
                                                            &nbsp;:&nbsp;<span id="spnTExchangeRateCurrency"></span>
                                                        </td>
                                                        <td align="right" style="background-color:aliceblue;height:auto;width:14%">
                                                            <span id="spnTExchangeRate">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="height:auto;width:22%;">Transaction Date</td>
                                                        <td colspan="4" align="left" style="background-color:aliceblue;height:auto;width:30%;">
                                                            &nbsp;:&nbsp;<span id="spnTTransactionDate">##/##/####</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="height:19px;width: 22%;">
                                                            Status</td>
                                                        <td align="left" style="background-color:aliceblue;height:19px; width:30%;">
                                                            &nbsp;:&nbsp;<span id="spnTStatus"></span>
                                                        </td>
                                                        <td class="cell_caption" align="left" style="width:22%;height:19px;">
                                                            Gross Amount</td>
                                                        <td align="left" style="background-color:aliceblue;height:19px;width:12%;">
                                                            &nbsp;:&nbsp;<span id="spnNetAmountCurrency"></span>
                                                        </td>
                                                        <td align="right" style="background-color:aliceblue;height:19px;width:14%;">
                                                            <span id="spnTGrossAmount">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="width: 22%;height:auto;">Encoded By</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:30%">
                                                            &nbsp;:&nbsp;<span id="spnTEncodedBy"></span>
                                                        </td>
                                                        <td class="cell_caption" align="left" style="width:22%;height:auto;">
                                                            Charges</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:12%">
                                                            &nbsp;:&nbsp;<span id="spnTBankChargeCurrency"></span>
                                                        </td>
                                                        <td align="right" style="background-color:aliceblue;height:auto;width:14%">
                                                            <span id="spnTBankCharge">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="width:22%;height:auto;">Validated By</td>
                                                        <td colspan="4" align="left" style="background-color:aliceblue;height:auto;width:30%">
                                                            &nbsp;:&nbsp;<span id="spnTValidatedBy"></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="width: 22%">
                                                            Type</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:30%">
                                                            &nbsp;:&nbsp;<span id="spnTType"></span>
                                                        </td>
                                                        <td class="cell_caption" align="left" style="width:22%">
                                                            Net Amount</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:12%">
                                                            &nbsp;:&nbsp;<span id="spnTGrossAmountCurrency"></span>
                                                        </td>
                                                        <td align="right" style="background-color:aliceblue;height:auto;width:14%">
                                                            <span id="spnNetAmount">0.00</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="width:22%;height:auto;">Sub Type</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:30%">
                                                            &nbsp;:&nbsp;<span id="spnTSubType"></span>
                                                        </td>
                                                        <td class="cell_caption" align="left" style="height:auto;width:22%">
                                                            Amount to be Received</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:12%"><b>
                                                            &nbsp;:&nbsp;<span id="spnTNetToBenefAmountCurrency"></span></b>
                                                        </td>
                                                        <td align="right" style="background-color:aliceblue;height:auto;width:14%"><b>
                                                            <span id="spnTNetToBenefAmount">0.00</span></b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" align="left" style="width:22%;height:auto;">Product Type</td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:30%">
                                                            &nbsp;:&nbsp;<span id="spnTProductDescription"></span>
                                                        </td>
                                                        <td align="left" style="height:auto;width:22%"></td>
                                                        <td align="left" style="background-color:aliceblue;height:auto;width:12%">
                                                            &nbsp;</td>
                                                        <td align="right" style="background-color:aliceblue;height:auto;width:14%"><b>&nbsp;</b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height:auto;">
                                <td style="width:70%">
                                    <span id="spnError"></span>
                                </td>
                                <td align="right" style="width:30%;">
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="height:auto; width:auto; white-space:nowrap;"> <!--nowrap -->
                                                <input id="btnPrintPanel" type="button" value="Print" onclick="javascript:epsco_trans.printDiv('divTransactionHistoryModal');" style="width:50px; visibility:visible;" />
                                                <input id="btnClosePanel" type="button" value="Close" onclick="javascript:epsco_trans.hidePanel('divTransactionHistoryModal');" style="width: 50px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divBeneficiary" style="z-index:75;position:absolute;left:1071px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:1098px;width:800px;height:auto;">
                        <table cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:auto;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:50%;height:auto;">Beneficiary Information</td>
                                <td align="left" style="width:100%;height:auto;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>   
                            </tr>
                            <tr>
                                <td style="width:100%" colspan="3">
                                    <table style="width:100%; border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:25%;">Beneficiary (L/F/M)Name</td>
                                            <td style="width:25%; background-color:aliceblue;height:auto;">:
                                                <input id="spnBeneficiaryLName" style="font-size: 12px; width:90.5%" readonly="readonly" />
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;width:25%;">
                                                <input id="spnBeneficiaryFName" style="font-size: 12px; width:96.5%" readonly="readonly" />
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;width:25%;">
                                                <input id="spnBeneficiaryMName" style="font-size: 12px; width:96.5%" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">Address</td>
                                            <td colspan="3" style="width:75%; background-color:aliceblue;height:auto;">:
                                                <input id="spnAddress" style="font-size: 12px; width: 83%;" readonly="readonly" />
                                                <input id="btnALeft" type="button" value="<" onclick="javascript:epsco_trans.getCIFInfo('Address', false);" style="width:32px;" />
                                                <input id="btnARight" type="button" value=">" onclick="javascript:epsco_trans.getCIFInfo('Address', true);" style="width:32px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">Contacts</td>
                                            <td colspan="3" style="width:75%; background-color:aliceblue;height:auto;">:
                                                <input id="spnContacts" style="font-size: 12px; width: 83%;" readonly="readonly" />
                                                <input id="btnCLeft" type="button" value="<" onclick="javascript:epsco_trans.getCIFInfo('Contacts', false);" style="width:32px;" />
                                                <input id="btnCRight" type="button" value=">" onclick="javascript:epsco_trans.getCIFInfo('Contacts', true);" style="width:32px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">Email Address</td>
                                            <td colspan="3" style="width:75%; background-color:aliceblue;height:auto;">:
                                                <input id="spnEmailAddress" style="font-size: 12px; width: 83%;" readonly="readonly" />
                                                <input id="btnELeft" type="button" value="<" onclick="javascript:epsco_trans.getCIFInfo('Email', false);" style="width:32px;" />
                                                <input id="btnERight" type="button" value=">" onclick="javascript:epsco_trans.getCIFInfo('Email', true);" style="width:32px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td width="25%" class="cell_caption" style="font-size:12px;height:auto;">Product Type</td>
                                            <td width="75%" colspan="3" style="background-color:aliceblue;height:auto;">:
                                                <input id="spnProductCode" style="font-size: 12px; width:96.5%" readonly="readonly" />
                                            </td>--%>
                                            <td colspan="4" style="width:auto;height:auto">
                                                <div id="grdAccounts"></div>
                                            </td>                                            
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">Account Number</td>
                                            <td style="width:25%; background-color:aliceblue;height:auto;">:
                                                <input id="spnAccountNumber" style="font-size: 12px; width: 90%;" readonly="readonly" />
                                            </td>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">Account Type</td>
                                            <td style="width:25%; background-color:aliceblue;height:auto;">:
                                                <input id="spnAccountType" style="font-size: 12px;" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">PNB Branch</td>
                                            <td style="width:25%; background-color:aliceblue;height:auto;">:
                                                <input id="spnPNBBranch" style="font-size: 12px; width: 90%;" readonly="readonly" />
                                            </td>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">Currency</td>
                                            <td style="width:25%; background-color:aliceblue;height:auto;">:
                                                <input id="spnProductCurrency" style="font-size: 12px;" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%; font-size:12px;height:auto;">Other Bank</td>
                                            <td colspan="3" style="width:75%; background-color:aliceblue;height:auto;">:
                                                <input id="spnOtherBank" style="font-size: 12px; width:96.5%" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height:auto">
                                                <table style="width:100%">
                                                    <tr>
                                                        <!--td width="50%" align="right"><input id="btnBAccountInquiry" type="button" value="Account Inquiry" onclick="javascript:epsco_trans.getAccountInquiry();" style="width:0px;display:none;" /></td-->
                                                        <td style="width:50%" align="center"><input id="btnBClosePanel" type="button" value="Close" onclick="javascript:hidePanelEP('divBeneficiary');" style="width:150px;" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="height: 17px"><span id="spnAccountLoading"></span></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divSecurity" style="z-index:75;position:absolute;left:1071px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:1427px;width:477px;height:auto;">
                        <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width: 100%;">
                            <tr>
                                <td align="left" valign="bottom" style="width:171px; border-collapse:collapse;height:auto;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 75%;height:auto;">Please enter your security code</td>
                                <td align="left" style="width:100%;height:auto;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>                                                           
                            </tr>
                            <tr>
                                <td style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid; border-bottom: #336699 3px solid;" colspan="3">
                                    <table>
                                        <tr>                                        
                                            <td style="width:50%;height:auto;"><img src="../images/cardsecurity.jpg" style="width:200; height:auto;" /></td>                                                                    
                                            <td style="width:50%;height:auto;">
                                                <table cellpadding="0" cellspacing="0" style="width:100%; FONT-SIZE: 12px; ">
                                                    <tr>
                                                        <td><input id="txtSecurityCode" type="password" maxlength="3" style="width: 95%" onkeypress="javascript:return epsco_trans.isNumberKey(event);" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height:auto">
                                                            <input id="btnSubmitTransaction" class="button" onclick="javascript:epsco_trans.submitTransaction();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Submit" style="width: 90px" />
                                                            <input id="btnCancelTransaction" class="button" onclick="javascript:epsco_trans.submitCancel();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 90px" />
                                                        </td>
                                                    </tr>                  
                                                </table>
                                            </td>
                                        </tr>                                                                            
                                    </table>
                                    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="spnPGLoading"></span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="div3DSecure" style="z-index:75;position:absolute;left:343px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:1476px;width:auto;height:auto;">
                    </div>
                    <div id="divSearch" style="z-index:75;position:absolute;left:1073px;color:darkblue;border:solid 2px #000;padding:10px;font-family:Arial;top:252px;width:auto;height:auto;display:none;">
                        <table cellpadding="0" cellspacing="0" style="width: 350px">
                            <tr>
                                <td style="width:802px;height:auto;">
                                    <table cellpadding="0" cellspacing="0" style="width:350px">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:auto;"><img src="../images/toe.GIF" /></td>
                                            <td class="table_head_dark" style="width:50%;height:auto;">
                                                CLIENT REMITTANCE</td>
                                            <td align="left" style="width:100%;height:auto;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table border="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid; height: 78px;">
                                                    <tr>
                                                        <td style="width:202px; height: 24px;" class="cell_caption">
                                                            CIF ID
                                                        </td>
                                                        <td style="width:450px; height: 24px;" align="left">
                                                            <input id="txtSCIFID" style="width:95%;text-align:left;" type="text" value="" onkeydown="javascript:epsco_trans.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <!--
                                                    <tr>
                                                        <td style="width:250px;" class="cell_caption">
                                                            Last Name
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtSLastName" style="width:95%;text-align:left;" type="text" value="" onkeydown="javascript:epsco_trans.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px; height:auto;" class="cell_caption">
                                                            First Name
                                                        </td>
                                                        <td style="width:450px; height:auto;" align="left">
                                                            <input id="txtSFirstName" style="width:95%;text-align:left;" type="text" value="" onkeydown="javascript:epsco_trans.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:250px;" class="cell_caption">
                                                            Middle Name
                                                        </td>
                                                        <td style="width:450px;" align="left">
                                                            <input id="txtSMiddleName" style="width:95%;text-align:left;" type="text" value="" onkeydown="javascript:epsco_trans.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    -->
                                                    <tr align="center">
                                                        <td colspan="2" style="height: 24px">
                                                            <table style="width:100%">
                                                                <tr>
                                                                    <!--td nowrap style="height:26px;"><input id="btnAddCIF" class="button" onclick="javascript:show_frame_dialog('../Internet/InternetRemitter.aspx?isRemit=true', 'internetClose', '100%', '100%', '0');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Add Client" style="width:100px;" /></td-->
                                                                    <!--td nowrap style="height:26px;"><input id="btnAddCIF" class="button" onclick="javascript:epsco_client.showEnrollmentForm();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Add Client" style="width:100px;" /></td-->
                                                                    <td nowrap style="height:26px;"><input id="btnSearch" class="button" onclick="javascript:epsco_trans.popRemitterDetail();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search" style="width:100px;" /></td>
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
                    
                    <div id="divSearchGrid" style="z-index:75;position:absolute;left:1073px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:493px;width:auto;height:auto;">
                        <div id="divSearchGridTop" style="display:block;"></div>
                        <br />
                        <div id="divSearchGridBottom" style="display:block;">
                            <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="center" style="height: 22px">
                                        <input id="btnGSearch" class="button" onclick="javascript:epsco_trans.cancelSearch();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back to Search" style="width:150px;" />
                                        <input id="btnGClose" class="button" onclick="epsco_trans.showSearch('divSearchGrid', '');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Close" style="width:150px;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    
                    <div id="divCharges" style="z-index:75;position:absolute;left:1070px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:Arial;top:1716px;height:auto;width:auto;">
                        <table cellpadding="0" cellspacing="0" style="width:400px; FONT-SIZE: 12px">
                            <tr>
                                <td align="left" valign="bottom" style="width:171px;border-collapse:collapse;height:auto;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:75%;height:auto;">Transaction Computation</td>
                                <td align="left" style="width:100%;height:auto;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>   
                            </tr>
                            <tr>
                                <td colspan ="3" style="width: 100%; height: 198px;">
                                    <table style="width:100%; border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                        <tr>
                                            <td class="cell_caption" align="left" style="height:19px;width:40%;">Exchange Rate: </td>
                                            <td align="right" style="background-color:aliceblue;height:19px;width:5%;">
                                                <span id="spnCExchangeRateCurrency"></span>
                                            </td>
                                            <td align="right" style="background-color:aliceblue;height:19px;width:40%;">
                                                <input id="txtCExchangeRate" type="text" value="0.00" maxlength="5" readonly="readOnly" /> <!--onkeypress="javascript:return epsco_trans.isNumberKeyPeriod(event);" /-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:40%" align="left">
                                                Gross Amount:</td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:5%;">
                                                <span id="spnCGrossAmountCurrency"></span>
                                            </td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:40%;">
                                                <input id="txtCGrossAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" align="left" style="height:auto; width:40%;">
                                                Charges:</td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:5%;">
                                                <span id="spnCBankChargeAmountCurrency"></span>
                                            </td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:40%;">
                                                <input id="txtCBankChargeAmount" type="text" value="0.00" maxlength="13" readonly="readOnly" /> <!--onkeypress="javascript:return epsco_trans.isNumberKey(event);" /-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" align="left" style="height:auto;width:40%;">
                                                Net Amount:</td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:5%;">
                                                <span id="spnCNetAmountCurrency"></span>
                                            </td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:40%;">
                                                <input id="txtCNetAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:40%; height: 19px;" align="left">
                                                Amount to be Received:</td>
                                            <td align="right" style="background-color:aliceblue;height:19px;width:5%;">
                                                <span id="spnCNetToBenefAmountCurrency"></span>
                                            </td>
                                            <td align="right" style="background-color:aliceblue;height:19px;width:40%;">
                                                <input id="txtCNetToBenefAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:40%" align="left"> Converted Amount in PHP:</td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:5%;">
                                                <span id="spnCPHPConvertedAmountCurrency"></span>
                                            </td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:40%;">
                                                <input id="txtCPHPConvertedAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <table style="width:100%">
                                                    <tr>
                                                        <td nowrap style="height: 19px"><!--input id="btnACompute" type="button" value="Compute" onclick="javascript:epsco_trans.computeRemittance(false);" style="width: 100px;"/--></td>
                                                        <td nowrap style="height: 19px"><!--input id="btnOverride" type="button" value="Override" onclick="javascript:epsco_trans.computeRemittance(true);" style="width: 100px;"/--></td>
                                                        <td nowrap style="height: 19px"><input id="btnCClosePanel" type="button" value="Close" onclick="javascript:hidePanelEP('divCharges');" style="width: 100px;" /></td>
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
                <td colspan="2" valign="middle" style="height:17px;"><center><span id="spnLoading"></span></center></td>
            </tr>
        </table>
                    
                     <div id="divEnrollCIF" style="position:absolute;display:none;width:800px;height:430px;overflow:auto;">
                        <table cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:11px;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:50%;height:11px;">Client Enrollment</td>
                                <td align="left" style="width:100%;height:11px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>    
                            </tr>
                            <tr>
                                <td colspan="3" style="width:100%; height: 92px;">
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;height:auto;width:100%">
                                        <tr>
                                            <td class="table_caption" colspan="3" style="width:100%;color:black;text-indent:5px;text-align:left;height:25px;">Remitter&nbsp;Information</td>
                                            <td align="right">
                                                <input style="width:162px;" id="btnSearchRem" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';" onclick="javascript:epsco_client.popCIFGrid(1);" type="button" value="Search Remitter" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Last Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtRemLastName" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">First Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtRemFirstName" type="text" style="width: 99%" />
                                            </td>                                        
                                        </tr>                                                    
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Middle Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtRemMiddleName" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption"  style="width:25%;height:19px;">Birth Date (mm/dd/yyyy):&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtRemBDate" type="text" style="width: 80%" maxlength="10"/>
                                                <img id="calRemBDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtRemBDate').disabled == false){popUpCalendar(this, document.getElementById('txtRemBDate'), 'mm/dd/yyyy', document.getElementById('divEnrollCIF'));}" src="../images/calendar/imgcalen.png" />  
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Room No./Bldg.:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtRemAdd1" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">No./Street:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                <input id="txtRemAdd2" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">PO Box:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtRemAdd3" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">District:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                <input id="txtRemAdd4" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">City:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtRemAdd5" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">State:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                <input id="txtRemAdd6" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Region:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtRemAdd7" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">Country:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                <select id="ddlRemCountry" style="width: 95%"><option selected="selected"></option></select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Zip Code:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtRemZipCode" type="text" style="width: 99%" maxlength="7" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">E-mail Address:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                 <input id="txtRemEmail" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Telephone No.:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtRemTelNo" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">Mobile No.:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                 <input id="txtRemMobNo" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4" style="height: 40px">
                                                <!--input id="btnAddRem" onclick="epsco_client.enroll_client(1,'ADD');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="ADD" style="width: 130px" /-->
                                                <input id="btnSaveRem" onclick="epsco_client.enroll_client(1,'UPDATE');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="SAVE" style="width: 130px" disabled="disabled" />
                                                <!--input id="btnCancelRem" onclick="javascript:hidePanelEP('divEnrollCIF');javascript:showPanelEP('divSearch');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 130px" /></td-->
                                        </tr>
                                        <tr>
                                            <td class="table_caption" colspan="4" style="width:125px;color:black;text-indent:5px;text-align:left;height:25px;">Beneficiary&nbsp;Information
                                            </td>
                                            <!--td>
                                                <input style="WIDTH: 162px" id="btnSearchBene" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';" onclick="epsco_client.popCIFGrid(2);" type="button" value="Search Beneficiary" />
                                            </td-->
                                                                                            
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4" style="overflow:inherit">
                                                <div id="grdBeneficiary" style="width:auto;height:auto;"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption"  style="width:25%;height:19px;">Account No:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtBenefAcctNo" type="text" style="width: 99%" maxlength="40"/>
                                            </td>
                                            <td class="cell_caption"  style="width:25%;height:19px;">Branch:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                <select id="ddlBenBranch" style="width: 95%"><option selected="selected"></option></select>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Last Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtBenefLastName" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">First Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtBenefFirstName" type="text" style="width: 99%" />
                                            </td>                                        
                                        </tr>                                                    
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Middle Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtBenefMiddleName" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption"  style="width:25%;height:19px;">Birth Date (mm/dd/yyyy):&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtBenefBDate" type="text" style="width: 80%" maxlength="10"/>
                                                <img id="Img1" align="absMiddle" onclick="javascript:if(document.getElementById('txtBenefBDate').disabled == false){popUpCalendar(this, document.getElementById('txtBenefBDate'), 'mm/dd/yyyy', document.getElementById('divEnrollCIF'));}" src="../images/calendar/imgcalen.png" />
                                                  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Room No./Bldg.:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtBenefAdd1" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">No./Street:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                <input id="txtBenefAdd2" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">PO Box:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtBenefAdd3" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:25%;height:19px;">Zip Code:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                <input id="txtBenefZipCode" type="text" style="width: 99%" maxlength="7" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:25%;height:19px;">Telephone No.:&nbsp;</td>
                                            <td align="left" style="width:25%;height:19px;">
                                                <input id="txtBenefTelNo" type="text" style="width: 99%" />
                                            </td>
                                            <td class="cell_caption" style="width:26%;height:19px;">Mobile No.:&nbsp;</td>
                                            <td align="left" style="width:74%;height:19px;">
                                                 <input id="txtBenefMobNo" type="text" style="width: 99%" />
                                            </td>
                                        </tr>
                                        <!--tr>
                                            <td class="cell_caption"  style="width:25%;height:19px;font-size:11px">Beneficiary same as Remitter:&nbsp;</td>
                                            <td colspan="3" align="left" style="height: 19px">
                                                <input type="checkbox" id="chkSameAsRemitter" style="width: 8%; height: 24px;" />

                                            </td>
                                        </tr-->
                                        <tr>
                                            <td align="center" colspan="4" style="height: 40px"> 
                                                <input id="btnAddBene" onclick="epsco_client.enroll_client(0,'ADD');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="ADD" style="width: 130px" />
                                                <input id="btnSaveBene" onclick="epsco_client.enroll_client(2,'UPDATE');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="SAVE" style="width: 130px" disabled="disabled" />
                                                <input id="btnCancelBene" onclick="javascript:hidePanelEP('divEnrollCIF');javascript:showPanelEP('divSearch');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 130px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>     
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divSearchClient" style="z-index:75;position:absolute;left:18px;color:darkblue;border:solid 2px #000;padding:10px;font-family:Arial;top:1548px;width:750px;height:100%;display:none;">
                        <table cellpadding="0" cellspacing="0" style="width:750px;">
                            <tr>
                                <td align="left" valign="bottom" style="width:11px; border-collapse:collapse;height:11px;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:50%;height:11px;">Search Client</td>
                                <td align="left" style="width:100%;height:11px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>    
                            </tr>
                            <tr valign="top">
                                <td colspan="3" style="width:100%;height:100%">
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;width:100%;height:100%">
                                        <tr>
                                            <td valign="middle" align="center" style="height:100%;width:100%">
                                                <div id="grdRemitter"></div>
                                            </td>
                                        </tr>
                                     </table>
                                </td>
                             </tr>
                             <tr><td style="width: 11px">&nbsp;</td></tr>
                             <tr>
                                <td valign="top" align="center" colspan="3">
                                    <input id="btnCloseCIFSearch" type="button" value="Close" onclick="javascript:hidePanelEP('divSearchClient');" style="width:150px;" />
                                </td>
                             </tr>
                        </table>
                    </div>
        </div>
    </form>
</body>
</html>
