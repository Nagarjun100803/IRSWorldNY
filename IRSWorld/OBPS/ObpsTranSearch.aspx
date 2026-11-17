<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ObpsTranSearch.aspx.cs" Inherits="IRSWorld.OBPS.ObpsTranSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head id="Head1" runat="server">
    <title>OBPS - Search</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>
    <script src="../js/OBPS/jsobpssearch.js"           type="text/javascript"></script>
    <script src="../js/popcalendar.js"                      type="text/javascript"></script>
    <script type="text/javascript">
        var obps_transearch = new obps_transearch();
    </script>
    
</head>

<body onload="javascript:obps_transearch.load();">
    <form id="form1" runat="server">
                    
    <center>
        <br />
        <div id="div_search" style="width:50%; display: block;">
            <table width="100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td id="capSearch" align="left" class="table_head_dark" style="width:25%; height: 24px; border-collapse:collapse">Search</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table width="100%" cellpadding="0" cellspacing="0" class="table_body" style="border: solid 1px skyblue">
                                        <tr>
                                            <td class="table_caption" style="width: 50%"; >Search by:</td>    
                                            <td class="table_caption" style="width: 50%;" align="left"><label class="table_caption" style=" text-align:center; font-size:medium" >&nbsp; Remitter </label></td>                                                        
                                        </tr>
                                        <tr>
                                            <td style="width:100%;height: 115px" align="center" valign="middle" colspan = "2">
                                                <table width="96%" cellspacing="5" cellpadding="0">
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 35%";><label id="lblLastName" class="cell_desc" >&nbsp; Last Name : </label> </td>
                                                        <td align="left" style="width: 65%";><input id="txtSearchLastname" type="text" style="width: 100%;text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 35%";><label id="lblFirstName" class="cell_desc" >&nbsp; First Name : </label> </td>
                                                        <td align="left" style="width: 65%";><input id="txtSearchFirstname" type="text" style="width: 100%;text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 35%";><label id="lblMiddleName" class="cell_desc" >&nbsp; Middle Name : </label> </td>
                                                        <td align="left" style="width: 65%";><input id="txtSearchMiddlename" type="text" style="width: 100%;text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 35%";><label id="lblIdType" class="cell_desc" >&nbsp; ID Type : </label> </td>
                                                        <td align="left" style="width: 65%";><select id="ddlIdType" class="cell_desc" size="1" style="width: 100%; text-align: left;"></select></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 35%";><label id="lblIdNumber" class="cell_desc" >&nbsp; ID Number : </label> </td>
                                                        <td align="left" style="width: 65%";><input id="txtSearchCardNo" type="text" style="width: 98.5%;text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                               </table>  
                                           </td> 
                                       </tr> 
                                       <tr>
                                            <td style="height: 28px" valign="top" colspan = "2">
                                                <input type="button" value="Search"  onclick = "javascript:obps_transearch.search_customers_info();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnsearch" /></td>
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
    </center>            
 
    <center>           &nbsp;</center>            

    <center>           
        <div id="div_improd_change" style="width:80%; display: block;">
            <table width="100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td id="Td2" align="left" class="table_head_dark" style="width:70%; height: 24px; border-collapse:collapse">Change Product / Implementation</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure;">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table width="100%" cellpadding="0" cellspacing="0" class="table_body" style="border: solid 1px skyblue">
                                        <tr>
                                            <td class="table_caption" style="width: 100%"; >Please provide the following :</td>    
                                        </tr>
                                        <tr>
                                            <td style="width:100%; height: 19px;" align="center" valign="middle" colspan = "2">
                                               <div id = "DIV_PROD_IMPLEM" onmousemove = "javascript:obps_transearch.active_grid = 'DIV_PROD_IMPLEM';">test</div>
                                            </td> 
                                       </tr> 
                                       <tr>
                                            <td style="height: 28px" valign="top" colspan = "2">
                                                </td>
                                    </tr>
                                        <tr>
                                            <td colspan="2" style="height: 28px" valign="top">
                                                <input type="button" value="Cancel" onclick="javascript:obps_transearch.open_division('div_results');" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /></td>
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
    </center>            

    
     <center>   
        <div id="div_results" style="width: 100%; height: 80%; display: block">
            <table width="100%">
                <tr>
                    <td style="width: 84%; padding-top: 30px">
                        <table width="96%" height="100%" cellspacing="0" cellpadding="0" style="background-color:Azure">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px;"><img src="../images/toe.GIF" /></td>
                                <td id="Td16" align="left" class="table_head_dark" style="width:20%; height: 24px; border-collapse:collapse;">Remitter's Transaction</td>
                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; height: 100%">
                                    <table style="width:100%; height: 100%;" class="table_border">
                                        
                                        <tr>
                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                <!-- content starts here -->
                                                 
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr><td><label id="lblRemitterName" class="cell_desc" style="font-size: 20px; color: blue;" >Alvin C. Santos</label></td></tr>
		                                                <tr><td><label id="lblRemitterAddress" class="cell_desc" style="font-size: medium; font-weight: normal;" >Via Fauche 37, Milan, Italy</label></td></tr>
                                                    </table>
                                                    
                                                    <br/>
                                                    <table width="100%" cellpadding="0" cellspacing="0" class="table_body" style="border: solid 1px skyblue">
                                                        <tr><td class="table_caption" style="width: 100%"; >
                                                            Remitters Transaction</td></tr>
                                                        <tr><td align="center" valign="middle" colspan = "2"><div id = "DIV_REM_TXN" onmousemove = "javascript:obps_transearch.active_grid = 'DIV_REM_TXN';">test</div></td></tr>
                                                    </table>
                                                    
                                                    <br/>

                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr><td></td></tr>
		                                                <tr><td><label id="lblBenefAddress" class="cell_desc" style="font-size: medium; font-weight: normal;" >
                                                            </label></td></tr>
                                                    </table>
                                                    
                                                    <br/>
                                                    <table width="100%" cellpadding="0" cellspacing="0" class="table_body" style="border: solid 1px skyblue">
                                                        <tr><td class="table_caption" style="width: 100%"; >
                                                            Available Merchants</td></tr>
                                                        <tr><td align="center" valign="middle" colspan = "2"><div id = "DIV_BENE_PROD_IMP" onmousemove = "javascript:obps_transearch.active_grid = 'DIV_BENE_PROD_IMP';">test</div></td></tr>
                                                    </table>
                                                    

                                                    <table id = "tblRemBeneTransaction" width="100%" cellpadding="0" cellspacing="5px">
                                                        <tr>
                                                            <td style="width:100%;" valign="top"><div id = "DIV_BENE_LIST"></div></td>                                                                
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table width="100%" cellpadding="0" cellspacing="0" id="TABLE1" >
                                                                    <tr>
                                                                        <td style="height: 28px; width: 100%" valign="top" align = "center">
                                                                            &nbsp;&nbsp;
                                                                            <input type="button"  style="width: 15% " value="Change Merchant"  onclick = "javascript:obps_transearch.open_division('div_improd_change');" class="button" />&nbsp;
                                                                            <input type="button"  style="width: 15% " value="Search"          onclick = "javascript:location.reload(true);"        class="button" />&nbsp;&nbsp;
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
                 
                    </td>
                </tr>
            </table>                    
        </div>
    </center>
<%--      <center> --%>
        <div id="div_merchants" style="width: 100%; height: 80%; display: block">
            <table width="100%" height="100%" cellspacing="0" cellpadding="0" style="background-color:Azure">
                <tr>
                <td style="height: 340px;" valign="top">
                <div width="20%" style="background-color:Azure">
                <table style="width:100%;background-color:Azure" >
                <tr>
                <a id="A1" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
               </tr>
               <tr>
                <a id="A4" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
                </tr>
                <tr>
                <a id="A5" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
               </tr>
               <tr>
                <a id="A6" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
                </tr>
                <tr>
                <a id="A7" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
               </tr>
               <tr>
                <a id="A8" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
                </tr>
                <tr>
                <a id="A9" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
               </tr>
               <tr>
                <a id="A10" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
                </tr>
                <tr>
                <a id="A11" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
               </tr>
               <tr>
                <a id="A12" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center></center></a>
                </tr>
                <tr>
                <a id="A2" href="javascript:obps_transearch.open_division('div_charges');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center>Charges</center></a>
               </tr>
               <tr>
                <a id="A3" href="javascript:location.reload(true);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><center>Search</center></a>
                </tr>
                </table>
                </div>
                </td>
                
                <td style="height: 340px;">
                <div  style="width:97%;background-color:Azure">
                <table cellspacing="0" cellpadding="0" style="background-color:Azure">
                <tr style="background-color:Azure">
                <td align="left" valign="bottom" style="width:1%;height:23px; background-color:Azure"><img src="../images/toe.GIF" /></td>
                                <td id="capResult" align="left" class="table_head_dark" style="width:20%; height: 23px; border-collapse:collapse;">Merchant</td>
                                <td align="left" valign="bottom" style="height: 23px"  class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                <table style="width:85%; height: 100%; background-color:Azure" class="table_border">
                                        
                                        <tr>
                                            <td colspan ="2" style="width: 228%; height: 100%; vertical-align: top">
                                             <table id="Table2" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699; "  >
                                                   
                                                   
                                                   <tr>
                                                        <td class="table_caption" colspan="4" style="width: 100%; text-align: left; text-indent: 5px;
                                                         color: Black; height: 26px;" valign="top">
                                                        MERCHANT INFO.</td>

                                                    </tr>
                                                    <tr>
                                                         <td class="cell_caption" style="width: 25%; height: 24px">
                                                          Merchant Name:&nbsp;</td>
                                                          <td align="left" colspan="3" style="width: 75%; height: 24px">
                                                          <label class="cell_desc" id="lbl_merchant_name">
                                                          </label>
                                                         </td>
                                                   </tr>
                                                   <tr>
                                                         <td class="cell_caption" style="width: 25%; height: 24px">
                                                          Merchant Product Code:&nbsp;</td>
                                                          <td align="left" colspan="3" style="width: 75%; height: 24px">
                                                          <label class="cell_desc" id="lbl_merchant_prodcode">
                                                          </label>
                                                         </td>
                                                   </tr>
                                                   <tr>
                                                            <td class="cell_caption" style="width: 25%; height: 24px">
                                                                  <label id="lbl_MID">
                                                                  </label>
                                                               &nbsp;</td>
                                                              <td align="left" colspan="3" style="width: 75%; height: 24px">
                                                                  <label id="lbl_merchant_id">
                                                              </label>
                                                               <input id="txtMemberID" class="cell_desc" style="width: 34%; display: none" /></td>
                                                    </tr>
                                                                                                                        
                                                    <tr>
                                                              <td class="cell_caption" style="width: 25%; height: 24px">
                                                                 Account Currency:&nbsp;</td>
                                                                  <td align="left" colspan="3" style="width: 75%; height: 24px">
                                                                        <label class="cell_desc" id="lbl_merchant_currency">
                                                                          </label>
                                                                          <select id="ddMAccountCurrency"  class="cell_desc" style="width: 35%; display: none">
                                                                          </select>
                                                                 </td>
                                                     </tr>
                                                      <tr>
                                                                                                                  
                                                      </tr>
                                                       <tr>
                                                                <td class="cell_caption" style="width: 25%; height: 24px">
                                                                Target Amount:&nbsp;</td>
                                                                 <td align="left" style="width: 25%; height: 24px">
                                                                 <label class="cell_desc"  id="lbl_target_amount">
                                                                  </label>
                                                                  <input id="txt_target_amount" class="cell_desc" style="width: 95%;" />
                                                                  </td>
                                                                    <td class="cell_caption" style="width: 25%; height: 24px">
                                                                 Amount :&nbsp;</td>
                                                                       <td align="left" style="width: 25%; height: 24px">
                                                                       <label class="cell_desc"  id="lbl_amount">
                                                               </label>
                                                                   <input id="txt_amount" class="cell_desc" style="width: 95%;" />

                                                              </td>
                                                       </tr>
                                                </table>                        
                                            </tr>
                                            <tr>
                                                <table id="tblBillsPayOBPSTransactionsMDetails" width="85%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                     <%-- testing1--%>
                                                                                                                       
                                                                                                                       
                                                    <tr>
                                                        <td class="table_caption" colspan="4" style="width: 85%; text-align: left; text-indent: 5px;color: Black; height: 26px;" valign="top">
                                                                                                                        
                                                        MERCHANT DETAILS
                                                        </td>
                                                    </tr>
                                                    
                                                 </table>
                                        </tr>
                                    </table>
                                    </div>
                                </td>
                
                             </tr>
                
         
                            <tr>
                             
                             <td colspan="2">
                                    <table style="width:20%"  cellpadding="0" cellspacing="5">
                                            <tr id="trMaster">

                                            </tr>
                                       </table>
                                    </td>
                                    <td  colspan="3">
                                    
                                </td>
                </tr>
<%--    </center>--%>
        <center>   
        <div id="div_charges" style="width: 80%; height: 80%; display: block">
            <table width="100%">
                <tr>
                    <td style="width: 84%; padding-top: 30px">
                        <table width="96%" height="100%" cellspacing="0" cellpadding="0" style="background-color:Azure">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px;"><img src="../images/toe.GIF" /></td>
                                <td id="Td3" align="left" class="table_head_dark" style="width:20%; height: 24px; border-collapse:collapse;">Charges</td>
                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; height: 100%">
                                    <table style="width:100%; height: 100%;" class="table_border">
                                        
                                        <tr>
                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                <!-- content starts here -->
                                                 
                                                    <table width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                                                                            
                                                                                                                                            
                                                            <tr>
                                                                <td class="cell_caption" style="width:26%; height: 23px;">
                                                                      Net Remit Amount(Rmt Ccy) :</td>
                                                                <td style="width:24%; height: 23px;">
                                                                      <input id="txtNetRemitAmountRmtCcy" type="text" readonly="readonly" style="height: auto; width: 97%;" /></td>
                                                                <td class="cell_caption" style="width:26%; height: 23px;">
                                                                       Commission :</td>
                                                                <td colspan="2" style="width:24%; height: 23px;">
                                                                        <input id="txtCommission" type="text" readonly="readonly" style="width: 97%; height: auto" /></td>
                                                                                                                                            
                                                           </tr>
                                                                                                                                            
                                                           <tr>
                                                                <td class="cell_caption" style="width:26%; height: 24px;">
                                                                         Cable Charges :</td>
                                                                 <td style="width:24%; height: 24px;">
                                                                          <input id="txtCableCharges" type="text" readonly="readonly" style="height: auto; width: 97%;" /></td>
                                                                 <td class="cell_caption" style="width:26%; height: 24px;">
                                                                          Back End Charges :</td>
                                                                 <td colspan="2" style="width:24%; height: 24px;">
                                                                           <input id="txtBackEndCharges" type="text" readonly="readonly" style="width: 97%; height: auto" /></td>
                                                                                                                                            
                                                           </tr>
                                                                                                                                            
                                                           <tr>
                                                                  <td class="cell_caption" style="width:26%; height: 24px;">
                                                                            Total Charge (Loc Ccy) :</td>
                                                                   <td style="width:24%; height: 24px;">
                                                                             <input id="txtTotalChargeLocCcy" type="text" readonly="readonly" style="height: auto; width: 97%;" /></td>
                                                                   <td class="cell_caption" style="width:26%; height: 24px;">
                                                                              Total Amount (Loc Ccy) :</td>
                                                                   <td colspan="2" style="width:24%; height: 24px;">
                                                                               <input id="txtTotalAmountLocCcy" type="text" readonly="readonly" style="width: 97%; height: auto" /></td>
                                                                                                                                            
                                                           </tr>
                                                           <tr>
                                                                  <td class="cell_caption" style="width:26%; height: 24px;">
                                                                              Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                  <td style="width:24%; height: 24px;">
                                                                              <input id="txtTotalAmountRmtCcy" type="text" readonly="readonly" style="height: auto; width: 97%;" /></td>
                                                                  <td class="cell_caption" style="width:26%; height: 24px;">
                                                                               Net Remit Amount(PHP Equiv):</td>
                                                                  <td colspan="2" style="width:24%; height: 24px;">
                                                                                <input id="txtNetRemitAmountRmtCcyPHPEquiv" type="text" readonly="readonly" style="width: 97%; height: auto" /></td>
                                                                                                                                            
                                                          </tr>
                                                          <tr>
                                                                  <td class="cell_caption" style="width:26%; height: 24px;">
                                                                              Service Fee :</td>
                                                                  <td style="width:24%; height: 24px;">
                                                                               <input id="txtServiceFee" type="text" readonly="readonly" style="height: auto; width: 97%;" /></td>
                                                                  <td class="cell_caption" style="width:26%; height: 24px;">
                                                                               Out-of-pocket Expense :</td>
                                                                  <td colspan="2" style="width:24%; height: 24px;">
                                                                               <input id="txtOutOfPocketExpense" type="text" readonly="readonly" style="width: 97%; height: auto" /></td>
                                                                                                                                            
                                                            </tr>
                                                            <tr>
                                                                   <td class="cell_caption" style="width:26%; height: 24px;">
                                                                               Documentary Stamps :</td>
                                                                   <td style="width:24%; height: 24px;">
                                                                               <input id="txtDocumentaryStamps" type="text" readonly="readonly" style="height: auto; width: 97%;" /></td>
                                                                   <td class="cell_caption" style="width:26%; height: 24px;">
                                                                                Net to Merchant (A/C Ccy):</td>
                                                                   <td colspan="2" style="width:24%; height: 24px;">
                                                                                <input id="txtNetToMerchantACCcy" type="text" readonly="readonly" style="width: 97%; height: auto" /></td>
                                                                                                                                            
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
                 
                    </td>
                </tr>
            </table>                    
        </div>
    </center>

    </form>
     <%--       </table>
        </div>--%>
</body>
</html>
