<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetPage.aspx.cs" Inherits="IRSWorld.Internet.InternetPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Remit Anywhere Online</title>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <script src="../js/Internet/jsInternetPage_min.js" type="text/javascript"></script>
    <script src="../js/jsExpand.js" type="text/javascript"></script>
    <script src="../js/jsBalloon.js" type="text/javascript"></script>
    <script src="../js/jsImageResize.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsRemExternals_min.js" type="text/javascript"></script>
    <script src="../js/popcalendar_min.js" type="text/javascript"></script>
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var internet_page = new internet_page();
        var agentRem = new agent_remittance();
    </script>
</head>
<body onload="javascript:internet_page.fixdim();internet_page.initializePage();" onresize="javascript:internet_page.fixdim();" onkeydown="return showKeyCode(event)" onunload="javascript:internet_page.close_session();">
    <form id="form1" runat="server">
        <div id="divPage" style="overflow: auto;">
        <table style="" cellpadding="0" cellspacing="5">
            <tr>
                <td id="tdInternetPagemenu"  style="vertical-align:top;height:100%;"><!--border-right-style:groove; border-right-color:Navy;--><!--menu-->
                    <div id="divInternetPageMenu" style="display:block;"><!-- border-right-style:groove; border-right-color:Navy;"-->
                         <table style="width:244px;" cellpadding="0" cellspacing="5">
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #black;height:auto;margin-left:auto;margin-right:auto; width:240px;">
                                    <img src="../images/u.gif" name="imgfirst"><a href="#first" onclick="shoh('first');" id="ahfirst" style="color: black; font-family: Tahoma; font-weight:bold;" >&nbsp;Transactions</a>
                                    <div style="display: none; overflow:hidden; width:240px;" id="first" >
                                        <table style="width:240px;">
                                            <tr id="trCreateTransaction">
                                                <td style="color: black;text-align:right;border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:internet_page.createTransaction('create');" style="color: black;">Create Transaction</a>
                                                </td>
                                            </tr>
                                            <tr id="trBillsPayment" style="display:none;">
                                                <td style="color: black; text-align:right;border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:window.open('../BillsPayment/OBPSTransactions.aspx', '_self');" style="color: black;">Bills Payment</a>
                                                </td>
                                            </tr>
                                            <tr id="trTransactionHistory">
                                                <td style="color: black;text-align:right;border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:internet_page.createTransaction('history');" style="color: black;">Transaction History</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right; border-bottom: solid 1px #black;height:auto;margin-left:auto;margin-right:auto; width:240px;">
                                    <img src="../images/u.gif" name="imgsecond"><a href="#first" onclick="shoh('second');" id="ahsecond" style="color: black; font-family: Tahoma;font-weight:bold;" >&nbsp;My Accounts</a>
                                    <div style="display: none; overflow:hidden; width:240px;" id="second" >
                                        <table style="width:240px;">
                                            <tr id="trAddCIF" style="text-align:right;margin-left:auto;margin-right:auto;">
                                                <td style="border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:internet_page.addBeneURL();" style="color: black;" id="lnkAddBene">Add Beneficiary / Accounts</a>
                                                </td>
                                            </tr>
                                            <tr id="trPassword" style="text-align:right;margin-left:auto;margin-right:auto;">
                                                <td style="border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:internet_page.showLinks('Password');" style="color: black;">Change Password</a>
                                                </td>
                                            </tr>
                                            <tr id="trOAAM" style="text-align:right;margin-left:auto;margin-right:auto;">
                                                <td style="border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:internet_page.showLinks('OAAM');" style="color: black;">Change Image / Phrase</a>
                                                </td>
                                            </tr>
                                            <tr style="text-align:right;margin-left:auto;margin-right:auto;">
                                                <td style="border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:internet_page.viewRemInfoURL();" style="color: black;">View Remitter Information</a>
                                                </td>
                                            </tr>
                                            <%--<tr>
                                                <td>
                                                    <a href="javascript:internet_page.getTodayForex();" style="font-style:italic; color: black;">Update Foreign Exchange</a>
                                                </td>
                                            </tr>--%>
                                            <tr id="trDislaimer" style="text-align:right;margin-left:auto;margin-right:auto;">
                                                <td style="border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                                    <a href="javascript:internet_page.showDisclaimer();" style="color: black;">Show Terms and Conditions</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trSearchCIF" style="display:none;">
                                <td style="text-align:right;border-bottom:solid 1px #black;height:auto;">
                                    <a href="javascript:internet_page.searchCIF(true);" style="color: black;font-family: Tahoma;font-weight:bold;">Search Client&nbsp;</a>
                                </td>
                            </tr>
                            <tr>
                                <td id="rLogout" style="text-align:right;border-bottom:solid 1px #black;height:auto;">
                                    <a href="javascript:internet_page.showLinks('Logout');" style="color: black;font-family: Tahoma;font-weight:bold;">Logout&nbsp;</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width:243px;">
                                    <table style="width:243px;">
                                        <tr>
                                            <!--td id="tdbackforex" style="background-image: url('../images/forex.jpg'); background-repeat: no-repeat; height:221px;"-->
                                            <td>
                                                <table style="border-style:none; border-spacing:0;border-collapse:collapse;">
                                                    <tr><td style="background-image:url('../images/forex_top.jpg');background-repeat: no-repeat;width:223px; height:28px;"></td></tr>                                                    
                                                    <tr style="text-align:right; margin-left:auto; margin-right:auto;background-color:#26A6E1;">
                                                        <td>
                                                            <span id="spnForexDate" style="color:White;"></span>
                                                        </td>
                                                    </tr>
                                                    <tr style="text-align:center; margin-left:auto; margin-right:auto;background-color:#26A6E1;">
                                                        <td>
                                                            <span id="spnForexValue"></span>
                                                        </td>
                                                    </tr>
                                                    <tr><td style="background-image:url('../images/forex_bottom.jpg');background-repeat: no-repeat; width:223px; height:19px;"></td></tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                             <tr>
                                 <td style="height: 100%;">
                                 </td>
                             </tr>
                        </table>
                    </div>
                </td>
                <td id="tdInternetPageCenter" style="vertical-align:top;height:auto;text-align:center; margin-left:auto; margin-right:auto;">
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
                    
                    <div id="divDisclaimerAlert" style="z-index:75;word-wrap:break-word;overflow:scroll;text-align:left;position:absolute;left:1071px;color:darkblue;border:solid 2px #000;padding:10px;font-family:tahoma;top:1978px;height:auto;display:none;">
                        <table style="width:600px;" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td><span id="spnDisclaimerAlert"></span></td>
                            </tr>
                            <tr>
                                <td style="height: 19px;text-align:right;">
                                    <input id="btnDisclaimerClose" type="button" class="button" value="Close" onclick="javascript:internet_page.hidePanel('divDisclaimerAlert');" style=""/>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="divRemitterName">
                        <table id="tblRemitterName" border="0" cellpadding="0" cellspacing="0" style="display:none;margin-left:auto;margin-right:auto;">
                            <tr><td>&nbsp;</td></tr>
                            <tr>
                                <td><span id="spnRemitterName" class="label4"></span></td>
                            </tr>
                            <tr><td>&nbsp;</td></tr>
                        </table>
                    </div>
                    
                    <div id="divStartPage" style="display:none;">
                        <%--<table id="tblStartPage" border="0" cellpadding="0" cellspacing="0" style="width:700px;margin-left:auto;margin-right:auto;">
                            <tr>
                                <td>
                                    <h2>What do you want to do today?</h2>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table cellspacing="10" border="0" cellpadding="5" style="width:700px;margin-left:auto;margin-right:auto;">
                                        <tr>
                                            <td><h3><a href="javascript:internet_page.createTransaction('create');">1. Send Remittance</a></h3></td>
                                        </tr>
                                        <tr>
                                            <td><h3><a href="javascript:show_frame_dialog('../Internet/InternetRemitter.aspx?function=add_bene', 'internetRemitterClose', '100%', '100%', '0');">2. Add Beneficiary / Account</a></h3></td>
                                        </tr>
                                        <tr>
                                            <td><h3><a href="javascript:internet_page.createTransaction('history');">3. Review Transaction History</a></h3></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>--%>
                        <table id="tblStartPage" border="0" cellpadding="0" cellspacing="0" style="width:700px;margin-left:auto;margin-right:auto;">
                            <tr>
                                <td>
                                    <img src="../images/task.jpg" style="width:592px; height:230px; border-style:none;" usemap="#Map" />
                                    <map name="Map" id="Map">
                                        <area shape="rect" coords="341,76,501,107" href="javascript:internet_page.createTransaction('create');" />
                                        <area shape="rect" coords="340,120,565,151" href="javascript:internet_page.addBeneURL();" />
                                        <area shape="rect" coords="339,164,574,195" href="javascript:internet_page.createTransaction('history');" />
                                    </map>
                                </td>
                            </tr>                        
                        </table>
                    </div>
                    
                    <div id="divTransactionHistory" style="display:none;">
                        <table style="width:700px;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="vertical-align:bottom;text-align:left;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:0%;border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>
                                            <td class="table_head_dark" style="height:auto;width:300px;">Transaction History</td>
                                            <td style="height:auto;text-align:left;vertical-align:bottom;width:400px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>                                
                            </tr>
                            <tr>
                                <td colspan="3" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid; border-bottom: #394397 3px solid;height:auto;">
                                    <div id="divRemitterTransaction">Transaction Grid Here</div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divTransaction" style="display:none;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:0%; border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="height:auto;width:300px;">Create Transaction</td>
                                            <td style="height:auto;text-align:left;vertical-align:bottom;width:400px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                        </tr>
                                    </table>                                
                                </td>                                
                            </tr>
                            <tr>
                                <td style="text-align:left;margin-left:auto;margin-right:auto;">
                                    <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;">
                                        <tr>
                                            <td colspan="4" class="table_caption" style="color: white; text-indent: 5px;text-align:left;height:25px;width:700px;">Remittance</td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Account No:&nbsp;<i>(optional)</i></td>
                                            <td style="height:auto; text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><input id="txtRemAcctNo" type="text" /></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;"><input id="btnSearchRemAcctNo" type="button" class="button" value="Search" onclick="javascript:internet_page.searchRemAcctNo();" style="width:100px;"/></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Send Money To:&nbsp;</td>
                                            <td style="height:auto; text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><select id="ddlBeneficiary" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlBeneficiary', 'lblBeneficiaryID', true);" style="width:400px;"></select></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;"><input id="btnBeneficiary" type="button" class="button" value="Show Detail" onclick="javascript:internet_page.showBeneficiaryInformation(true);" style="width:100px;"/></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Product:&nbsp;</td>
                                            <td class="cell_caption" style="height:auto; text-align:left;margin-left:auto;margin-right:auto;" ><span id="spnProductDesc" class="label1"></span></td>
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Account:&nbsp;<span id="spnAccountNo" class="label1"></span></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Purpose of Remittance:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><select id="ddlPurpose" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlPurpose', 'lblPurpose', true);" style="width:400px;"></select></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr id="trPayMode" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Pay via:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><select id="ddlPaymentMethod" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlPaymentMethod', 'lblPaymentMethod', true);" style="width:400px;"></select></td>
                                            <td style="text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr id="trOnline" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Choose the account / card no.:&nbsp;</td>
                                            <td style="height:auto;text-align:right;margin-left:auto;margin-right:auto;width:90px;"><span id="spnSourceCurrency" class="label2" style="width:85px;"></span></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:310px;"><select id="ddlDCCard" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlDCCard', 'lblDCCard', true);" style="width:307px;"></select></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr id="trAgent1" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Charge Type:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><select id="ddlChargeType" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlChargeType', 'lblChargeType', true);" style="width:400px;"></select></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr id="trAgent5" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Transaction Source:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><select id="ddlTranxSource" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlTranxSource', 'lblTranxSource', true);" style="width:400px;"></select></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr id="trAgent6" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Special Message:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><input id="txtSpecialMessage" type="text" style="width:400px;" maxlength="512" /></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr id="trAgent2" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;"><span>Computation</span>:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;" colspan="2"><select id="ddlComputation" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlComputation', 'lblComputation', true);" onmouseover="javascript:tooltip.show(internet_page.showBalloon('Computation'));" onmouseout="javascript:tooltip.hide();" style="width:400px;"></select></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:100px;">
                                                <span style="padding-bottom:1px; border-bottom:1px dotted #900; cursor:pointer;" onmouseover="javascript:tooltip.show(internet_page.showBalloon('Computation'));" onmouseout="javascript:tooltip.hide();"><b>?</b></span>&nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trAgent3" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Remitting Currency:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:90px;"><select id="ddlRemittingCurrency" class="ddl" onchange="javascript:internet_page.getDDLAjax('ddlRemittingCurrency', 'lblRemittingCurrency', true);" style="width:85px;"></select></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:310px;"><input id="txtExchangeRate" type="text" style="width:307px;" readonly="readonly" /></td>
                                            <td style="text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">Amount:&nbsp;</td>
                                            <td style="height:auto;text-align:right;margin-left:auto;margin-right:auto;width:90px;"><span id="spnRemittingCurrency" class="label2" style="width:85px;"></span></td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:310px;"><input id="txtAmount" type="text" value="0.00" onblur="javascript:internet_page.getDDLAjax('', '', '');" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" style="width:307px;" maxlength="13" /></td>
                                            <td style="text-align:left;margin-left:auto;margin-right:auto;width:100px;"><input id="btnCompute" type="button" class="button" value="Compute" onclick="javascript:internet_page.computeRemittance(false);" style="width:95px;display:none;"/></td>
                                        </tr>
                                        <tr id="trAgent4" style="display:none;">
                                            <td class="cell_caption" style="width:200px;height:auto;margin-left:auto;margin-right:auto;">
                                                Amount to be Received:&nbsp;</td>
                                            <td style="height:auto;text-align:left;margin-left:auto;margin-right:auto;width:400px;" colspan="2"><span id="spnNetToBeneAmount" class="label2" style="width:390px;"></span></td>
                                            <td style="text-align:left;margin-left:auto;margin-right:auto;width:100px;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="height:auto;text-align:center;width:550px;"><input id="btnSubmit" class="button" onmouseout="this.className='button';" onclick="javascript:internet_page.computeRemittance(false);" onmouseover="this.className='button_over';" type="button" value="Compute" style="width:150px;" disabled="disabled" /></td><!--  onclick="javascript:internet_page.checkTransaction();"-->
                                            <td colspan="1" style="height:auto;text-align:left;width:150px;"><input id="btnRemAggregate" type="button" class="button" value="Remittance Aggregate" onclick="javascript:internet_page.rem_aggregates();" style="width:150px;" disabled="disabled" onmouseout="this.className='button';" onmouseover="this.className='button_over';"/></td>                                            
                                        </tr>
                                        <%--<tr>
                                            <td colspan="4" style="height:auto;">
                                                Note: Click <b><i>Compute</i></b> button to view computation.
			                                    If correct, click <b><i>Ok</i></b> button for confirmation.<br />
			                                    To complete transaction, click <b><i>Submit</i></b> Transaction button.
			                                </td>
                                        </tr>--%>
                                    </table>
                                </td>
                            </tr><tr><td style="text-align:center;margin-left:auto;margin-right:auto;"><span id="spnHitMessage" style="font-weight:bold;color:Red;width:500px"></span></td></tr>
                            <tr><td style="text-align:center;margin-left:auto;margin-right:auto;"><span id="spnHitMessage2" style="font-weight:bold;color:Red;width:500px"></span></td></tr>
                        </table>
                    </div>
                    
                    <div id="divReadMe" style="display:none;">
                        <table cellpadding="0" cellspacing="0" style="">
                            <tr>
                                <td style="vertical-align:bottom;text-align:left;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:0%;border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>
                                            <td class="table_head_dark" style="height:1px;">IMPORTANT REMINDER</td>
                                            <td style="height:auto;text-align:left;vertical-align:bottom;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>   
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="">
                                    <table style="border-right: #394397 3px solid;border-top: #394397 3px solid;border-left: #394397 3px solid; border-bottom:#394397 3px solid;">
                                        <tr>
                                            <td class="cell_caption" style="text-align:left;"><span id="spnReminder" style="text-align:left;"></span></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr><td colspan="3">&nbsp;</td></tr>
                        </table>
                    </div>
                                        
                    <div id="divTransactionInfo" style="display:none;">
                        <table cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td style="vertical-align:bottom;text-align:left;" colspan="3">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:0%; border-collapse:collapse;height:11px;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="height:11px;width:300px;">Transaction Information</td>
                                            <td style="height:11px;text-align:left;vertical-align:bottom;width:400px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>    
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width:700px;">
                                    <table style="border-right: #394397 3px solid;border-top: #394397 3px solid;border-left: #394397 3px solid; border-bottom:#394397 3px solid;height:auto;">
                                        <tr>
                                            <td class="table_caption" colspan="4" style="color:white;text-indent:5px;text-align:left;height:25px;width:700px;">
                                                Remittance
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">Reference No.:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnReferenceNo" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                            <td class="cell_caption" style="height:auto;width:150px;">Target Account:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTIBeneficiaryAccount" type="text" readonly="readonly" style="width:197px;" />
                                            </td>                                        
                                        </tr>                                                    
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">Beneficiary Name:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                <input id="spnTIBeneficiaryName" type="text" readonly="readonly" style="width:547px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">Beneficiary Address:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                <input id="spnTIBeneficiaryAddress" type="text" readonly="readonly" style="width:547px;" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">Mode of Payment:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTIProductCode" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                            <td class="cell_caption" style="height:auto;width:150px;"><span id="spnPM1"></span>:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTIDCCard" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">
                                                Gross Amount:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTIGrossAmount" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                            <td class="cell_caption" style="height:auto;width:150px;"><span id="spnPM2"></span>:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTIExpiryDate" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">Charges:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTICharges" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                            <td class="cell_caption" style="height:auto;width:150px;"><span id="spnPM3"></span>:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;" >
                                                <input id="spnTICardName" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">
                                                Amount to be Received:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTINetToBenefAmount" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                            <td class="cell_caption" style="height:auto;width:150px;">Exchange Rate Used:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:200px;">
                                                <input id="spnTIForex" type="text" readonly="readonly" style="width:197px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="table_caption" colspan="4" style="color: white; text-indent: 5px;text-align: left; height:25px;width:700px;">
                                                Payment Gateway
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">Authentication Code:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                <input id="spnTIAuthCode" type="text" readonly="readonly" style="width:547px;" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="cell_caption" style="height:auto;width:150px;">
                                                Response:&nbsp;</td>
                                            <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                <input id="spnTIResponse" type="text" readonly="readonly" style="width:547px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td>
                                    <input id="btnNewRemit" type="button" class="button" value="Remit Again" onclick="javascript:internet_page.createTransaction('create');" style="width:200px;"/>
                                </td>
                                <td>
                                    <input id="btnRemHistory" type="button" class="button" value="View Remittance History" onclick="javascript:internet_page.createTransaction('history');" style="width:200px;"/>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divDisclaimer" style="height:auto;word-wrap:break-word;overflow:scroll;text-align:left;border:solid 1px midnightblue;display:none;">
                        <table style="width:700px;" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="height: 17px"><span id="spnDisclaimer"></span></td>
                            </tr>
                            <tr>
                                <td style="height:auto;"><input id="chkDisclaimer" type="checkbox" onclick="javascript:internet_page.checkDisclaimer();" style="border:none;width:15px;" /><span class="label2">I have read and agree to the above terms and conditions</span></td>
                            </tr>
                            <tr>
                                <td>
                                    <input id="btnAccept" type="button" class="button" value="Accept" onclick="javascript:internet_page.acceptDisclaimer(true);" style=""/>
                                    <input id="btnDecline" type="button" class="button" value="Decline" onclick="javascript:internet_page.acceptDisclaimer(false);" style="" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divHidden">
                        <table border="0" style="" cellpadding="0" cellspacing="0">
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
                                    <input id="lblTranxSource" type="hidden" style="width:1px; height:1px;" />
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
                                    <input id="lblRemInfoCIFID" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoRemitterName" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoBirthdate" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoEmployer" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoOccupation" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoDetailedOccupation" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoSourceOfFunds" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoWorkerType" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoSalary" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoSalaryCcy" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoRemBeneficiaryName" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoRelationship" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoIDType" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemInfoIDClass" type="hidden" style="width:1px; height:1px;" />
                                    <input id="lblRemIDsType" type="hidden" style="width:1px; height:1px;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divTransactionHistoryModal" style="z-index:75;position:absolute;left:1072px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:575px;height:auto;">
                        <table style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:700px;">
                            <tr>
                                <td colspan="2" style="height:auto;width:700px;">
                                    <table style="" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td rowspan="3" style="width:100px;">
                                                <img src="../images/PNBLOGOembsmaller.png" /> <!--onmouseover="javascript:sli(this,250,198,506,400);" onmouseout="javascript:ssi(this,250,198,506,400);" /-->
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
                                                        <%--<td style="width: 3px"><b><span id="spnBranch2"></span></b></td>
                                                        <td><b><span id="spnBranch3"></span></b></td>--%>
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
                                                    <%--<tr>
                                                        <td class="cell_caption" style="height:auto;text-align:right;">Validated By</td>
                                                        <td colspan="4" style="background-color:aliceblue;height:auto;text-align:left;">
                                                            &nbsp;:&nbsp;<span id="spnTValidatedBy"></span>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <%--<td class="cell_caption" style="text-align:right;width:175px;">
                                                            Type</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;:&nbsp;<span id="spnTType"></span>
                                                        </td>--%>
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
                                                    <%--<tr>
                                                        
                                                        <td style="height:auto;text-align:left;"></td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:left;width:175px;">
                                                            &nbsp;</td>
                                                        <td style="background-color:aliceblue;height:auto;text-align:right;width:175px;"><b>&nbsp;</b>
                                                        </td>
                                                    </tr>--%>
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
                                            <td nowrap style="height:auto; width:auto;">
                                                <input id="btnSendReceipt" type="button" class="button" value="Send thru Email" onclick="javascript:internet_page.sendReceiptEmail();" style="visibility:visible;width:110px;" />
                                                <input id="btnPrintPanel" type="button" class="button" value="Print" onclick="javascript:internet_page.printDiv('divTransactionHistoryModal');" style="visibility:visible;width:110px;" />
                                                <input id="btnClosePanel" type="button" class="button" value="Close" onclick="javascript:internet_page.hidePanel('divTransactionHistoryModal');" style="width:110px;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divBeneficiary" style="z-index:75;position:absolute;left:1071px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:1098px;width:auto;height:auto;">
                        <table cellpadding="0" cellspacing="0" style="width:700px;">
                            <tr>
                                <td style="vertical-align:bottom;text-align:left;width:700px;" colspan="3">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:0%; border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="height:auto;width:200px;">Beneficiary Information</td>
                                            <td style="height:auto;text-align:left;vertical-align:bottom;width:500px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>   
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:700px;" colspan="3">
                                    <table style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:700px;">
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Beneficiary Name</td>
                                            <td colspan="3" style="background-color:aliceblue;height:auto;width:550px;">:
                                                <input id="spnBeneficiaryName" style="font-size: 12px; width:540px;" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Address</td>
                                            <td colspan="3" style="background-color:aliceblue;height:auto;width:550px;">:
                                                <input id="spnAddress" style="font-size: 12px; width:503px;" readonly="readonly" />
                                                <input id="btnALeft" type="button" value="<" onclick="javascript:internet_page.getCIFInfo('Address', false);" style="width:15px;" />
                                                <input id="btnARight" type="button" value=">" onclick="javascript:internet_page.getCIFInfo('Address', true);" style="width:15px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Contacts</td>
                                            <td colspan="3" style="background-color:aliceblue;height:auto;width:550px;">:
                                                <input id="spnContacts" style="font-size: 12px; width:503px;" readonly="readonly" />
                                                <input id="btnCLeft" type="button" value="<" onclick="javascript:internet_page.getCIFInfo('Contacts', false);" style="width:15px;" />
                                                <input id="btnCRight" type="button" value=">" onclick="javascript:internet_page.getCIFInfo('Contacts', true);" style="width:15px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Email Address</td>
                                            <td colspan="3" style="background-color:aliceblue;height:auto;width:550px;">:
                                                <input id="spnEmailAddress" style="font-size: 12px; width:503px;" readonly="readonly" />
                                                <input id="btnELeft" type="button" value="<" onclick="javascript:internet_page.getCIFInfo('Email', false);" style="width:15px;" />
                                                <input id="btnERight" type="button" value=">" onclick="javascript:internet_page.getCIFInfo('Email', true);" style="width:15px;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Product Type</td>
                                            <td colspan="3" style="background-color:aliceblue;height:auto;width:550px;">:
                                                <input id="spnProductCode" style="font-size: 12px; width:540px;" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Account Number</td>
                                            <td style="background-color:aliceblue;height:auto;width:200px;">:
                                                <input id="spnAccountNumber" style="font-size: 12px; width:190px;" readonly="readonly" />
                                            </td>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Account Type</td>
                                            <td style="background-color:aliceblue;height:auto;width:200px;">:
                                                <input id="spnAccountType" style="font-size: 12px;width:190px;" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">PNB Branch</td>
                                            <td style="background-color:aliceblue;height:auto;width:200px;">:
                                                <input id="spnPNBBranch" style="font-size: 12px; width:190px;" readonly="readonly" />
                                            </td>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Currency</td>
                                            <td style="background-color:aliceblue;height:auto;width:200px;">:
                                                <input id="spnProductCurrency" style="font-size: 12px;width:190px;" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="font-size:12px;height:auto;width:150px;">Other Bank</td>
                                            <td colspan="3" style="background-color:aliceblue;height:auto;width:550px;">:
                                                <input id="spnOtherBank" style="font-size: 12px; width:540px;" readonly="readonly" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height:auto">
                                                <table style="width:700px;">
                                                    <tr>
                                                        <!--td width="50%" align="right"><input id="btnBAccountInquiry" type="button" value="Account Inquiry" onclick="javascript:internet_page.getAccountInquiry();" style="width:0px;display:none;" /></td-->
                                                        <td style="text-align:center;"><input id="btnBClosePanel" type="button" class="button" value="Close" onclick="javascript:internet_page.hidePanel('divBeneficiary');" style="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align:center;"><span id="spnAccountLoading"></span></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divSecurity" style="z-index:75;position:absolute;left:1071px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:1427px;height:auto;">
                        <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width:500px;">
                            <tr>
                                <td style="vertical-align:bottom;text-align:left;width:500px;" colspan="3">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;width:0%;"><img src="../images/reskin/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="height:auto;width:300px;">Please enter your security code</td>
                                            <td style="height:auto;text-align:left;vertical-align:bottom;width:200px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid; border-bottom: #394397 3px solid;" colspan="3">
                                    <table>
                                        <tr>                                        
                                            <td style="height:250px;width:300px;"><img src="../images/cardsecurity.jpg" style="height:250px;width:300px;" /></td>                                                                    
                                            <td style="height:auto;width:300px;">
                                                <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px;">
                                                    <tr>
                                                        <td><input id="txtSecurityCode" type="password" maxlength="3" style="" onkeypress="javascript:return internet_page.isNumberKey(event);" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height:auto">
                                                            <input id="btnSubmitTransaction" class="button" onclick="javascript:internet_page.submitTransaction();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Submit" style="" />
                                                            <input id="btnCancelTransaction" class="button" onclick="javascript:internet_page.submitCancel();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="" />
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
                    
                    <div id="divRemitterInfo" style="z-index:75;position:absolute;left:191px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:1700px;height:auto;">
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
                           <%-- <tr>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    Employer:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoEmployer" type="text" style="width:200px;" maxlength="100" /></td>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    Worker Type:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoWorkerType" class="ddl" style="width:200px"></select></td>
                                
                            </tr>
                            <tr>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    Annual Salary:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><input id="txtRemInfoSalary" type="text" style="width:200px;" maxlength="8" /></td>
                                <td class="cell_caption" style="vertical-align:bottom;text-align:left;width:150px;">
                                    Salary Currency:
                                </td>
                                <td style="vertical-align:bottom;text-align:left;width:250px;"><select id="ddlRemInfoSalaryCcy" class="ddl" style="width:200px"></select></td>
                            </tr>--%>
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
                    
                    <div id="divRemitterIds" style="z-index:75;position:absolute;left:191px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:2000px;height:auto;">
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
                    
                    <div id="div3DSecure" style="z-index:75;position:absolute;left:191px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:813px;width:auto;height:auto;">
                    </div>
                    
                    <div id="divSearch" style="z-index:75;position:absolute;left:1073px;color:darkblue;border:solid 2px #000;padding:10px;font-family:tahoma;top:252px;width:auto;height:auto;display:none;">
                        <table cellpadding="0" cellspacing="0" style="width:400px;">
                            <tr>
                                <td style="height:auto;width:400px;">
                                    <table cellpadding="0" cellspacing="0" style="width:400px;">
                                        <tr>
                                            <td style="vertical-align:bottom;text-align:left;width:400px;" colspan="3">
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width:0%; border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>
                                                        <td class="table_head_dark" style="height:auto;width:150px;">
                                                            SEARCH</td>
                                                        <td style="height:auto;text-align:left;vertical-align:bottom;width:250px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width:400px;">
                                                <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:400px;">
                                                    <tr>
                                                        <td style="width:100px;" class="cell_caption">
                                                            Search By
                                                        </td>
                                                        <td style="text-align:left;width:300px;">
                                                            <select id="ddlSearchBy" class="ddl" onchange="javascript:internet_page.getDDL('ddlSearchBy', 'lblCIFClass', true);" style="width:290px;" onkeydown="javascript:internet_page.callSearch();"></select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:100px;" class="cell_caption">
                                                            Last Name
                                                        </td>
                                                        <td style="text-align:left;width:300px;">
                                                            <input id="txtSLastName" style="text-align:left;width:290px;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height:auto;width:100px;" class="cell_caption">
                                                            First Name
                                                        </td>
                                                        <td style="height:auto; text-align:left;width:300px;">
                                                            <input id="txtSFirstName" style="text-align:left;width:290px;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:100px;" class="cell_caption">
                                                            Middle Name
                                                        </td>
                                                        <td style="text-align:left;width:300px;">
                                                            <input id="txtSMiddleName" style="text-align:left; width:290px;" type="text" value="" onkeydown="javascript:internet_page.callSearch();" />
                                                        </td>
                                                    </tr>
                                                    <%--<tr>
                                                        <td style="width:100px;" class="cell_caption">
                                                            is Exact
                                                        </td>
                                                        <td style="text-align:left;width:300px;">
                                                            <input id="chkExactMatch" style="width: 12%" type="checkbox" /></td>
                                                    </tr>--%>
                                                    <tr style="text-align:center;">
                                                        <td colspan="2">
                                                            <table style="width:400px;">
                                                                <tr>
                                                                    <td nowrap style="height:auto;"><input id="btnAddCIF" class="button" onclick="javascript:show_frame_dialog('../Internet/InternetRemitter.aspx?isRemit=true', 'internetClose(null)', '100%', '100%', '0');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Add Client" style="width:100px;" /></td>
                                                                    <td nowrap style="height:auto;"><input id="btnSearch" class="button" onclick="javascript:internet_page.searchCIF(false);" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search" style="width:100px;" /></td>
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
                    
                    <div id="divSearchGrid" style="z-index:75;position:absolute;left:1073px;color:darkblue;border:solid 2px #000;padding:10px;display:none;font-family:tahoma;top:493px;width:auto;height:auto;">
                        <div id="divSearchGridTop" style="display:block;"></div>
                        <br />
                        <div id="divSearchGridBottom" style="display:block;">
                            <table style="width:850px;" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="text-align:center;">
                                        <input id="btnGSearch" class="button" onclick="javascript:internet_page.cancelSearch();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back to Search" style="width:100px;" />
                                        <input id="btnGClose" class="button" onclick="internet_page.showSearch('divSearchGrid', '');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Close" style="width:100px;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    
                    <div id="divCharges" style="color:darkblue;padding:10px;display:none;font-family:tahoma;height:auto;width:700px; text-align:center;">
                        <table cellpadding="0" cellspacing="0" style="FONT-SIZE: 12px; width:410px;">
                            <tr>
                                <td colspan="3" style="vertical-align:bottom;text-align:left;width:400px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                            <td class="table_head_dark" style="height:auto;">Transaction Computation</td>
                                            <td style="height:auto;text-align:left;vertical-align:bottom;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan ="3" style="width:400px;">
                                    <table style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;">
                                        <tr>
                                            <td class="cell_caption" style="height:auto;text-align:right;width:150px;"><span id="spnVIP"></span>Exchange Rate: </td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:100px;">
                                                <span id="spnCExchangeRateCurrency"></span>
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:150px;">
                                                <input id="txtCExchangeRate" type="text" value="0.00" maxlength="15" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" /> <!--onkeypress="javascript:return internet_page.isNumberKeyPeriod(event);" /-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="height:auto;text-align:right;width:150px;">
                                                Amount to be sent:</td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:100px;">
                                                <span id="spnCNetAmountCurrency"></span>
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:150px;">
                                                <input id="txtCNetAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="height:auto; text-align:right;width:150px;">
                                                Service Fee:</td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:100px;">
                                                <span id="spnCBankChargeAmountCurrency"></span>
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:150px;">
                                                <input id="txtCBankChargeAmount" type="text" value="0.00" maxlength="13" onkeypress="javascript:return(internet_page.autoFormatCurrency(this,',','.',event));" /> <!--onkeypress="javascript:return internet_page.isNumberKey(event);" /-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="text-align:right;width:150px;">
                                                Total Cost:</td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:100px;">
                                                <span id="spnCGrossAmountCurrency"></span>
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:150px;">
                                                <input id="txtCGrossAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="text-align:right;width:150px;">
                                                Amount to be Received:</td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:100px;">
                                                <span id="spnCNetToBenefAmountCurrency"></span>
                                            </td>
                                            <td style="background-color:aliceblue;height:auto;text-align:right;width:150px;">
                                                <input id="txtCNetToBenefAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr>
                                        <!--tr>
                                            <td class="cell_caption" style="width:40%" align="left"> Converted Amount in PHP:</td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:19%;">
                                                <span id="spnCPHPConvertedAmountCurrency"></span>
                                            </td>
                                            <td align="right" style="background-color:aliceblue;height:auto;width:40%;">
                                                <input id="txtCPHPConvertedAmount" type="text" readonly="readonly" value="0.00" />
                                            </td>
                                        </tr-->
                                        <tr>
                                            <td colspan="3">
                                                <table style="width:400px;text-align:center;">
                                                    <tr>
                                                        <td nowrap><input id="btnConfirmRem" type="button" class="button" value="Remit Now" onclick="javascript:internet_page.confirmTransaction();" style="width:80px;"/></td>
                                                        <td nowrap><input id="btnACompute" type="button" class="button" value="Compute" onclick="javascript:internet_page.computeRemittance(false);" style="width:80px; display:none;"/></td>
                                                        <td nowrap><input id="btnOverride" type="button" class="button" value="Override" onclick="javascript:internet_page.computeRemittance(true);" style="width:80px;"/></td>
                                                        <td nowrap><input id="btnCClosePanel" type="button" class="button" value="Close" onclick="javascript:internet_page.closeComputation();" style="width:80px;" /></td>
                                                    </tr>                                                    
                                                </table>
                                            </td>
                                        </tr>                                        
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <b>Reminder:</b> Make sure your <b><i>pop-up blocker</i></b> is disabled to allow appearance of pop-up windows. 
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height:auto;vertical-align:middle;"><center><span id="spnLoading"></span></center></td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>