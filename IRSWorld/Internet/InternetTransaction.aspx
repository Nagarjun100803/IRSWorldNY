<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetTransaction.aspx.cs" Inherits="IRSWorld.Internet.InternetTransaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Internet-Transaction</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js"  type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"           type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src = "../js/jsgridselector.js"           type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetSideBar.js"  type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetTransaction.js" type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetGridView.js" type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetTranSaving.js" type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving.js" type="text/javascript"></script>
    <script type="text/javascript">
            var Internet_Transaction = new Internet_Transaction();
    
    </script>
</head>
<body onload="javascript:Internet_Transaction.load()">
    <form id="form1" runat="server">
        <div style="width:100%">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        
                            <table style="width:100%">
                                <tr>
                                 <td id = "tdInternetTransactionMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdInternetTransactionMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
//                                            var sider = new js_sidebar('sidemenu', sidebar);
//                                            document.write(sider.toString());
                                        </script>
                                    </div>
                                    <a id=ahider href=# onclick="javascript:Internet_Transaction.set_side_bar_visibility('divtdInternetTransactionMenu', 'ahider');">«</a>
                                 </td>            
                                                               
                                 <td style="width: 78%; padding-top: 30px">
                                     <div id="divInternetTransaction" style="width: 100%; height: 100%; display: block; overflow:scroll; height:400px;">
                                        <table cellspacing="0" cellpadding="0" style="width:96%;  background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Internet - Transaction</td>
                                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
							                <tr>
								                <td colspan="3" style="width: 100%; height: 100%">
									                <table style="width:100%; height: 100%;" class="table_border">
                										
										                <tr>
											                <td style="width: 100%; height: 100%; vertical-align: top">
												                <!-- content starts here -->
                												 
													                <table style="width:100%" cellpadding="0" cellspacing="0">
														                <tr><td><label id="lblInternetTransactionRemName" class="cell_desc" style="font-size: 20px; color: blue;" >Alvin C. Santos</label></td></tr>
														                <tr><td><label id="lblInternetTransactionRemAddress" class="cell_desc" style="font-size: medium; font-weight: normal;" >Via Fauche 37, Milan, Italy</label></td></tr>
														                <tr><td><input id="txtHiddenRemCIFID"  type="hidden" value="1072102109000001"/></td></tr>
													                </table>
                													
													                <br/>
													                <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
														                <tr><td class="table_caption" style="width: 100%"; >
															                Remitters Transaction</td></tr>
														                <tr><td align="center" valign="middle" colspan = "2"><div id = "divInternetTransactionRemTran" onmousemove = "javascript:Internet_Transaction.active_grid = 'divInternetTransactionRemTran';">test</div></td></tr>
													                </table>
                													
													                <br/>

													                <table style="width:100%" cellpadding="0" cellspacing="0">
														                <tr><td><label id="lblInternetTransactionBeneName" class="cell_desc" style="font-size: 20px; color: blue;" >
															                Francisco Balagtas</label></td></tr>
														                <tr><td><label id="lblInternetTransactionBenefAddress" class="cell_desc" style="font-size: medium; font-weight: normal;" >
															                Pilipinas</label></td></tr>
															             <tr><td><input id="txtHiddenBeneCIFID"  type="hidden" value="1072121109000002"/></td></tr>
													                </table>
                													
													                <br/>
													                <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
														                <tr><td class="table_caption" style="width: 100%"; >
															                Beneficiaries Default Product &amp; Implementation</td></tr>
														                <tr><td align="center" valign="middle" colspan = "2"><div id = "divInternetTransactionBeneImp" onmousemove = "javascript:Internet_Transaction.active_grid = 'divInternetTransactionBeneImp';">test</div></td></tr>
													                </table>
                													

													                <table id = "tblInternetTransactionRemBeneTransaction" style="width:100%" cellpadding="0" cellspacing="5px">
														                <tr>
															                <td style="width:100%;" valign="top"><div id = "DIV_BENE_LIST"></div></td>                                                                
														                </tr>
														                <tr>
															                <td>
																                <table style="width:100%" cellpadding="0" cellspacing="0" id="TABLE1" >
																	                <tr>
																		                <td style="height: 28px; width: 100%" valign="top" align = "center">
																			                &nbsp;&nbsp;
																			                <input type="button"  style="width: 15% " value="GFMC" onclick = "javascript:<%--Internet_Transaction.open_division('divInternetGFMC');--%><%--Internet_Transaction.construct_side_bar('divtdInternetTransactionMenu');--%>Internet_Transaction.gfmc();" class="button" />&nbsp;
																			                <input type="button"  style="width: 20% " value="Credit To Account" onclick = "javascript:<%--Internet_Transaction.open_division('divInternetCreditToAccount');--%><%--Internet_Transaction.construct_side_bar('divtdInternetTransactionMenu');--%>Internet_Transaction.credit();" class="button" />&nbsp;&nbsp;
																			                <input type="button"  style="width: 15% " value="Rapid Remit" onclick = "javascript:<%--Internet_Transaction.open_division('divInternetRapidRemit');--%><%--Internet_Transaction.construct_side_bar('divtdInternetTransactionMenu');--%>Internet_Transaction.rapid();" class="button" />&nbsp;&nbsp;
																		                </td>                                                                                    
																	                </tr>
																                </table> 
															                </td>
                																		
														                </tr>
													                </table>                                                                                                   
												                <!-- content ends here -->
											                </td>
										                </tr>
										                <tr>
											                <td style="width: 100%; height: 8px">
											                </td>
										                </tr>                                    
									                </table>
									             </td>
									         </tr>
									     </table>
									</div>
									<div id="divInternetRapidRemit" style="width: 100%; height: 100%; display: none">
									   <table id="tblInternetRapidRemit" cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="Td1" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Internet - Rapid Remit</td>
                                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width:100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                    <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                        <tr>
                                                                            <td style="width: 78%; height: 100%;"valign="center">
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divInternetRRRemitterInfo" >
                                                                                <table id="tblInternetRRRemRemitter" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetRRRemName" class="cell_desc" style="font-size: medium;" >&nbsp;<span style="font-size: 13pt">Alvin C. Santos</span></label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetRRRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                            <br />
                                                                             
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divInternetRRRemBeneficiaryInfo" >
                                                                                 <table id="tblInternetRRRemBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%" colspan = "3"><label id="lblInternetRRRemBeneName" class="cell_desc" style="font-size: medium;" >&nbsp;<span style="font-size: 13pt">Janet Romasanta</span></label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 34px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 34px;" colspan = "3"><label id="lblInternetRRRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan,
                                                                                            Philippines, 3022</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">PNB Branch :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblInternetRRRemPNBBranch" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;1394 PNB Sta. Maria Bulacan</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">CA / SA No. :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblInternetRRRemBeneCASANo" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;11112222333374444</label></td>                                                                                        
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Account Type :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblInternetRRRemBeneAccountType" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Savings</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">A/C Currency :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblInternetRRRemBeneACCurrency" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;PHP</label></td>                                                                                        
                                                                                    </tr>
                                                                                </table>
                                                                             </div>   
                                                                             
                                                                             <br />
  
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divInternetRemAmountInfo" >                                                                               
                                                                                 <table id="tblInternetRRRemAmount" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetRRRemTARC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetRRRemNRARC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetRRRemNRAPE" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetRRRemNTBACC"  type="text" style="width:96%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                             <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td>
                                                                                     
                                                                                </tr> 
                                                                             </table>
                                                                             <%--<br />--%>

                                                                            
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" value="View Transaction Details" onclick="javascript:Internet_Transaction.load_transaction_details('Rapid Remit');" 
                                                                            class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                        </tr>
     
                                                                    </table>

                                                                <!-- content ends here -->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                   </div>
                                   <div id="divInternetGFMC" style="width: 100%; height: 100%; display: none">
                                       <table cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="Td2" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Internet - GFMC</td>
                                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width:100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                    <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%;"valign="center">
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divInternetGFMCRemRemitterInfo" >
                                                                                <table id="tblInternetGFMCRemRemitter" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetGFMCRemName" class="cell_desc" style="font-size: medium;" >&nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetGFMCRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table> 
                                                                             <div style ="width:98%; height:100%; margin:0; " class="table_body" id="divInternetGFMCRemBeneficiaryInfo">
                                                                                 <table id="tblInternetGFMCRemBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height:23px">Beneficiary :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblInternetGFMCRemBeneName" class="cell_desc" style="font-size: medium; ">&nbsp; Janet Romasanta</label></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblInternetGFMCRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                     </tr>
                                                                                 </table>
                                                                              </div>
                                                                               <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                              </table> 
                                                                              <div style ="width:98%; height:30%; margin:0;" class="table_body" id="divInternetGFMCRemBeneficiaryBank">
                                                                                 <div style =" width:100%; height:30%; margin:0;" class="table_body" id="divInternetGFMCRemBeneficiaryBankInfo">
                                                                                 <table id="tblGFMCRemBeneficiaryBank" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">PNB Branch :&nbsp; </td>
                                                                                             <td align="left" style="width:35%"><label id= "lblInternetGFMCPNBBranch" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  1394 PNB Sta. Maria Bulacan</label></td>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">CA/SA No. :&nbsp;</td>
                                                                                             <td align="left" style="width:35%"><label id= "lblInternetGFMCCaSaNo" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 11112222333374444</label></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">Account Type :&nbsp; </td>
                                                                                             <td align="left" style="width:35%"><label id= "lblInternetGFMCAccountType" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; Savings</label></td>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">A/C Currency :&nbsp;</td>
                                                                                             <td align="left" style="width:35%"><label id= "lblInternetGFMCACCurrency" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; PHP</label></td>
                                                                                         </tr>
                                                                                     <%--</div>--%>
                                                                                 </table>
                                                                                 </div>
                                                                                 <div style ="width:100%; height:30%; margin:0; <%--display:none;--%>" class="table_body" id="divInternetGFMCRemBeneficiaryAvail">
                                                                                    <table id="tblInternetGFMCRemBeneficiaryAvail" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 27%; height:24px">A/C Currency :&nbsp; </td>
                                                                                             <td style="width: 40%; height: 24px;"><select id="ddlInternetGFMCRemACCurrency" style="width:98%; font-weight:normal;"/></td>
                                                                                             <td align="left" style="width:17%; height: 24px;"> <input name="rdoInternetGFMCRemAvailment" id="rdoInternetGFMCRemAvailment" style=" border:none; background-color:transparent; width:auto;" type="radio" onclick="javascript:Internet_Transaction.generate_avail();"/><label id= "lblGFMCBeneAvailment" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Avail</label></td>
                                                                                             <td align="left" style="width:16%; height: 24px;"> <input name="rdoInternetGFMCRemPurchase" id="rdoInternetGFMCRemPurchase" style=" border:none; background-color:transparent; width:auto;" type="radio" onclick="javascript:Internet_Transaction.generate_purchase();"/><label id= "lblGFMCBenePurchase" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Purchase</label></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 27%; height:24px">Card Reference No :&nbsp; </td>
                                                                                            <td align="left" style="width:40%; height: 24px;"><input id="txtInternetGFMCRemCardRefNo" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            <td style="width:17%; height: 24px;"><input style=" width:70%;" id="btnInternetGFMCRemGet" type="button" value="GET" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                            <td style="width:16%; height: 24px;"><input style=" width:70%;" id="btnInternetGFMCRemClear" type="button" value="CLEAR" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 27%; height:24px">Global Filipino Card No :&nbsp; </td>
                                                                                            <td align="left" style="width:40%; height: 24px;"><input id="txtInternetGFMCRemGlobalCardNo" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            <td style="width:17%; height: 24px;"><input style=" width:70%;" id="btnInternetGFMCRemTest" type="button" value="TEST" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                            <td style="width:16%; height: 24px;"><input style=" width:70%;" id="btnInternetGFMCRemCancel" type="button" value="CANCEL" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 25%; height:24px">ID Presented :&nbsp; </td>
                                                                                            <td colspan=4 align="left" style="width:75%; height: 24px;"><input id="txtInternetGFMCRemIDPresented" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            
                                                                                         </tr>
                                                                                     </table>
                                                                                  </div>
                                                                             </div>
                                                                             <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                             </table> 
                                                                             <div style="width:98%; height:100%; margin:0 " class="table_body" id="divGFMCRemAmountInfo">
                                                                                 <table id="tblGFMCRemAmount" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetGFMCRTARC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetGFMCNRARC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetGFMCNRAPE" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetGMFCNTBACC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                  </table>
                                                                              </div>
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td>
                                                                                      
                                                                                </tr> 
                                                                             </table>
                                                                             
                                                                       <%--      <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                             </table>--%>
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" value="View Transaction Details" class="button" onclick="javascript:Internet_Transaction.load_transaction_details('GFMC');" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                        </tr>
     
                                                                    </table>

                                                                <!-- content ends here -->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                       </table>
                                   </div>
                                   <div id="divInternetCreditToAccount" style="width: 100%; height: 100%; display: none">
                                       <table cellspacing="0" cellpadding="0" style="width:96%; background-color: Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width: 1%; height: 24px;">
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="Td3" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                                    border-collapse: collapse;">
                                                    Internet - Credit To Account</td>
                                                <td align="left" valign="bottom" style="height: 24px" class="bg_body">
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width: 100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 78%; height: 100%;" valign="center">
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divInternetCARemitterInfo">
                                                                                <table id="tblCARemRemitter" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%">
                                                                                            <label id="lblInternetCARemName" class="cell_desc" style="font-size: medium;">
                                                                                                &nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px;">
                                                                                            Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%">
                                                                                            <label id="lblInternetCARemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divInternetCARemBeneficiaryInfo">
                                                                                <table id="tblInternetCARemBeneficiary"  cellpadding="0" cellspacing="8"
                                                                                    style="width:100%; border: solid 1px #336699">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Beneficiary :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%" colspan="3">
                                                                                            <label id="lblInternetCARemBeneName" class="cell_desc" style="font-size: medium;">
                                                                                                &nbsp;Janet S. Romasanta</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 34px">
                                                                                            Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 34px;" colspan="3">
                                                                                            <label id="lblInternetCARemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan,
                                                                                                Philippines, 3022</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            PNB Branch :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%">
                                                                                            <label id="lblInternetCARemPNBBranch" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;1394 PNB Sta. Maria Bulacan</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            CA / SA No. :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%">
                                                                                            <label id="lblInternetCARemCASANo" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;1111222233334444</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Account Type :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%">
                                                                                            <label id="lblInternetCARemAccountType" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;Savings</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            A/C Currency :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%">
                                                                                            <label id="lblInternetCARemBeneACCurrency" class="cell_desc" style="font-size: 15px; font-weight: normal;">
                                                                                                &nbsp;PHP</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px; width: 3px;">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divCARemAmountInfo">
                                                                                <table id="tblCARemAmount" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetCARRemTARC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetCARRemNRARC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetCARRemNRAPE" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><input id="txtInternetCARRemNTBACC" type="text" style="width:96%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td>
                                                                                      
                                                                                </tr> 
                                                                             </table>                                                                           
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                    <tr>
                                                                        <td style="height: 28px; width: 100%" valign="top">
                                                                            <input style="height: 28px; width: 30%" type="button" value="View Transaction Details" onclick="javascript:Internet_Transaction.load_transaction_details('Credit to Account')"
                                                                                class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /></td>
                                                                    </tr>
                                                                </table>
                                                                <!-- content ends here -->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="divInternetTranView" style="width: 100%; height: 100%; display: none">
                            <table style="width:100%">
                                <tr>
                                    <td style="width: 80%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult2" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">View Transaction Details</td>
                                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width:100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                <div style="width:100%; height:400px; margin: 0; overflow:auto" class="table_body" id="div1" >
                                                                    <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%;" valign="center">
                                                                           
                                                                                <table id="tblInternetTVDetails" cellpadding="0" cellspacing="8" style="width:98%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td style="width: 16%; height: 21px;" align="right" class="cell_caption">&nbsp;Reference No :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVRefNo" class="cell_desc" style="font-weight: normal">&nbsp;1111222233334444</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Covering Bank :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVCoverBank" class="cell_desc" style="font-weight: normal">&nbsp;PNB Italy</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Pay Mode :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVPayMode" class="cell_desc" style="font-weight: normal">&nbsp;Cash</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 16%; height: 21px;" align="right" class="cell_caption">&nbsp;Transaction Source :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVTranSource" class="cell_desc" style="font-weight: normal">&nbsp;Banco Posta</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Source of Fund :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVSourceFund" class="cell_desc" style="font-weight: normal">&nbsp;Salary</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Receipt Mode :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVReceiptMode" class="cell_desc" style="font-weight: normal">&nbsp;Over the Counter</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 16%; height: 21px;" align="right" class="cell_caption">&nbsp;Transaction Date :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVTranDate" class="cell_desc" style="font-weight: normal">&nbsp;November 03, 2009</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Date Today:</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVDateToday" class="cell_desc" style="font-weight: normal">&nbsp;November 03, 2009</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Remit Currency :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVRemitCurrency" class="cell_desc" style="font-weight: normal">&nbsp;EURO</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 16%; height: 21px;" align="right" class="cell_caption">&nbsp;Commission Status :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVCommStatus" class="cell_desc" style="font-weight: normal">&nbsp;Monthly</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Status :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVTranStatus" class="cell_desc" style="font-weight: normal">&nbsp;For Processing</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Local Currency :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVLocalCurrency" class="cell_desc" style="font-weight: normal">&nbsp;EURO</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 16%; height: 21px;" align="right" class="cell_caption">&nbsp;Charge Type :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVChargeType" class="cell_desc" style="font-weight: normal">&nbsp;Standard</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Purpose :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVPurpose" class="cell_desc" style="font-weight: normal">&nbsp;Family Support</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;EURO to USD :</td>
                                                                                        <td align="right" style="width: 18%; height: 23px;"><label id="lblInternetTVEuroToUSD" class="cell_desc" style="font-weight: normal">&nbsp;36.00</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="width: 16%; height: 21px;" align="right" class="cell_caption">&nbsp;Product Type :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVProductType" class="cell_desc" style="font-weight: normal">&nbsp;Rapid Remit</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;Implementation :</td>
                                                                                        <td align="left" style="width: 18%; height: 23px;"><label id="lblInternetTVImplementation" class="cell_desc" style="font-weight: normal">&nbsp;Credit to Account</label></td>
                                                                                        <td style="width: 15%; height: 21px;" align="right" class="cell_caption">&nbsp;USD to PHP :</td>
                                                                                        <td align="right" style="width: 18%; height: 23px;"><label id="lblInternetTVUSDToEuro" class="cell_desc" style="font-weight: normal">&nbsp;45.00</label></td>
                                                                                    </tr>
                                                                                    
                                                                                </table>
                                                                            
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table> 
                                                                                <table id="tblInternetTVRemitter" cellpadding="0" cellspacing="8" style="width:98%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetTVRemName" class="cell_desc" style="font-size: medium;" >&nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblInternetTVRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table> 
                                                                                
                                                                                 <table id="tblInternetTVRemitterBeneficiary" cellpadding="0" cellspacing="8" style="width:98%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary :</td>
                                                                                        <td align="left" style="width: 85%" colspan = "3"><label id="lblInternetTVBeneName" class="cell_desc" style="font-size: medium;" >&nbsp;Janet S. Romasanta</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :</td>
                                                                                        <td align="left" style="width: 85%" colspan = "3"><label id="lblInternetTVBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table> 
                                                                                 <table id="tblInternetTVAmounts" cellpadding="0" cellspacing="8" style="width:98%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 25%; height: 23px">Total Amount (Rmt Ccy) :</td>
                                                                                        <td align="right" style="width: 25%; height: 23px;"><label id="lblInternetTVTARC" class="cell_desc" style="font-size: 15px; font-weight: bold;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 25%; height: 23px">Net Remit Amount (Rmt Ccy) :</td>
                                                                                        <td align="right" style="width: 25%; height: 23px;"><label id="lblInternetTVNRARC" class="cell_desc" style="font-size: 15px; font-weight: normal;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 25%; height: 23px">Net Remit Amount (Php Eqv) :</td>
                                                                                        <td align="right" style="width: 25%; height: 23px;"><label id="lblInternetNRAPE" class="cell_desc" style="font-size: 15px; font-weight: normal;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 25%; height: 23px">Net to Beneficary (A/C Ccy) :</td>
                                                                                        <td align="right" style="width: 25%; height: 23px;"><label id="lblInternetNTBACC" class="cell_desc" style="font-size: 15px; font-weight: bold;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table> 
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                            <td style="height: 28px; width: 50%" valign="top" >
                                                                            <input style="height: 28px; width: 10%" type="button" value="Post" onclick="javascript:Internet_Transaction.post();" class="button" />&nbsp; &nbsp;&nbsp;
                                                                            </td>
                                                                            
                                                                        </tr>
     
                                                                    </table>
                                                                </div>
                                                                <!-- content ends here -->
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
                                           
                        </td>
                                </tr>
                            </table>
                    
                    </td>
                    
                </tr>
            </table>
        </div>   
    </form>
</body>
</html>
