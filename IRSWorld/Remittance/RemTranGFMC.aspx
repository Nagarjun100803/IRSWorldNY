<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranGFMC.aspx.cs" Inherits="IRSWorld.Remittance.RemTranGFMC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Remittance - GFMC</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js"  type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"           type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving.js"  type="text/javascript"></script>
    <script src = "../js/Remittance/jsremcommons.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsRemTranGFMC.js" type="text/javascript"></script>    
    <script type="text/javascript">
            var RemTran_GFMC = new RemTran_GFMC();
    
    </script>
</head>
<body onload="javascript:RemTran_GFMC.construct_side_bar('divtdRemGFMCMenu');">
    <form id="form2" runat="server">
        <div style="width:100%">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divRemGFMC" style="width: 100%; height: 100%; display: block">
                            <table style="width:100%">
                                <tr>
                                    <td id = "tdRemGFMCMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdRemGFMCMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
//                                            var sider = new js_sidebar('sidemenu', sidebar);
//                                            document.write(sider.toString());
                                        </script>
                                    </div>
                                     <a id=ahider href=# onclick="javascript:RemTran_GFMC.set_side_bar_visibility('divtdRemGFMCMenu', 'ahider');">«</a>               
                                    </td>                                
                                    <td style="width: 78%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%;  background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Remittance - GFMC</td>
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
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divGFMCRemRemitterInfo" >
                                                                                <table id="tblGFMCRemRemitter" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblGFMCRemName" class="cell_desc" style="font-size: medium;" >&nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblGFMCRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table> 
                                                                             <div style ="width:98%; height:100%; margin:0; " class="table_body" id="divGFMCRemBeneficiaryInfo">
                                                                                 <table id="tblGFMCRemBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height:23px">Beneficiary :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblGFMCRemBeneName" class="cell_desc" style="font-size: medium; ">&nbsp; Janet Romasanta</label></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblGFMCRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                     </tr>
                                                                                 </table>
                                                                              </div>
                                                                               <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                              </table> 
                                                                              <div style ="width:98%; height:30%; margin:0;" class="table_body" id="divGFMCRemBeneficiaryBank">
                                                                                 <div style =" width:100%; height:30%; margin:0;" class="table_body" id="divGFMCRemBeneficiaryBankInfo">
                                                                                 <table id="tblGFMCRemBeneficiaryBank" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">PNB Branch :&nbsp; </td>
                                                                                             <td align="left" style="width:35%"><label id= "lblGFMCRemPNBBranch" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  1394 PNB Sta. Maria Bulacan</label></td>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">CA/SA No. :&nbsp;</td>
                                                                                             <td align="left" style="width:35%"><label id= "lblGFMCRemCaSaNo" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 11112222333374444</label></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">Account Type :&nbsp; </td>
                                                                                             <td align="left" style="width:35%"><label id= "lblGFMCRemAccountType" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; Savings</label></td>
                                                                                             <td class="cell_caption" style="width: 15%; height:23px">A/C Currency :&nbsp;</td>
                                                                                             <td align="left" style="width:35%"><label id= "lblGFMCRemACCurrency" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; PHP</label></td>
                                                                                         </tr>
                                                                                     <%--</div>--%>
                                                                                 </table>
                                                                                 </div>
                                                                                 <div style ="width:100%; height:30%; margin:0; <%--display:none;--%>" class="table_body" id="divGFMCRemBeneficiaryAvail">
                                                                                    <table id="tblGFMCRemBeneficiaryAvail" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 27%; height:24px">A/C Currency :&nbsp; </td>
                                                                                             <td style="width: 40%; height: 24px;"><select id="ddlGFMCRemACCurrency" style="width:98%; font-weight:normal;"/></td>
                                                                                             <td align="left" style="width:17%; height: 24px;"> <input name="rdoGFMCRemAvailment" id="rdoGFMCRemAvailment" style=" border:none; background-color:transparent; width:auto;" type="radio" onclick="javascript:RemTran_GFMC.generate_avail('rdoGFMCRemPurchase', 'txtGFMCRemGlobalCardNo', 'txtGFMCRemIDPresented');"/><label id= "lblGFMCBeneAvailment" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Avail</label></td>
                                                                                             <td align="left" style="width:16%; height: 24px;"> <input name="rdoGFMCRemPurchase" id="rdoGFMCRemPurchase" style=" border:none; background-color:transparent; width:auto;" type="radio" onclick="javascript:RemTran_GFMC.generate_purchase('rdoGFMCRemAvailment', 'txtGFMCRemGlobalCardNo', 'txtGFMCRemIDPresented');"/><label id= "lblGFMCBenePurchase" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Purchase</label></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 27%; height:24px">Card Reference No :&nbsp; </td>
                                                                                            <td align="left" style="width:40%; height: 24px;"><input id="txtGFMCRemCardRefNo" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            <td style="width:17%; height: 24px;"><input style=" width:70%;" id="btnGFMCRemGet" type="button" value="GET" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                            <td style="width:16%; height: 24px;"><input style=" width:70%;" id="btnGFMCRemClear" type="button" value="CLEAR" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 27%; height:24px">Global Filipino Card No :&nbsp; </td>
                                                                                            <td align="left" style="width:40%; height: 24px;"><input id="txtGFMCRemGlobalCardNo" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            <td style="width:17%; height: 24px;"><input style=" width:70%;" id="btnGFMCRemTest" type="button" value="TEST" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                            <td style="width:16%; height: 24px;"><input style=" width:70%;" id="btnGFMCRemCancel" type="button" value="CANCEL" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 25%; height:24px">ID Presented :&nbsp; </td>
                                                                                            <td colspan=4 align="left" style="width:75%; height: 24px;"><input id="txtGFMCRemIDPresented" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            
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
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Total Amount (Rmt Ccy): </td>
                                                                                          <td align="right" style="width:20%"><label id= "lblGFMCRemTARC" class="cell_desc" style="font-size: medium;">&nbsp; 100,000.00</label></td>
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Net Remit Amount (Rmt Ccy):&nbsp;</td>
                                                                                          <td align="right" style="width:20%"><label id= "lblGFMCRemNRARC" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 100,000.00</label></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Net Remit Amount (Php Equiv): </td>
                                                                                          <td align="right" style="width:20%"><label id= "lblGFMCRemNRAPE" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 100,000.00</label></td>
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Net to Beneficiary (A/C Ccy):&nbsp;</td>
                                                                                          <td align="right" style="width:20%"><label id= "lblGFMCRemNTBACC" class="cell_desc" style="font-size: medium;">&nbsp; 100,000.00</label></td>
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
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" value="View Transaction Details" class="button" onclick="javascript:RemTran_GFMC.load_transaction_view_details();" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
                                                                        </tr>
     
                                                                    </table>

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
            </div>   
    </form>
</body>
</html>
