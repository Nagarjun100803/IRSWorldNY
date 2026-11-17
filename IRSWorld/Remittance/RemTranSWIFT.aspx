<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranSWIFT.aspx.cs" Inherits="IRSWorld.Remittance.RemTranSWIFT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remittance - SWIFT</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js"  type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"           type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving.js"  type="text/javascript"></script>
    <script src = "../js/Remittance/jsremcommons.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsRemTranSWIFT.js" type="text/javascript"></script>    
    <script type="text/javascript">
            var RemTran_SWIFT = new RemTran_SWIFT();
    </script>
    
</head>
<body onload="javascript:RemTran_SWIFT.construct_side_bar('divtdRemSWIFTMenu');">
    <form id="form2" runat="server">
        <div style="width:100%">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:98%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divRemSWIFT" style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td id = "tdRemSWIFTMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdRemSWIFTMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
//                                            var sider = new js_sidebar('sidemenu', sidebar);
//                                            document.write(sider.toString());

                                        </script>
                                    </div>
                                     <a id=ahider href=# onclick="javascript:RemTran_SWIFT.set_side_bar_visibility('divtdRemSWIFTMenu', 'ahider');">«</a>               
                                    </td>                                
                                    <td style="width: 77%; padding-top: 30px">
                                        <table width="96%"  cellspacing="0" cellpadding="0" style="background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Remittance - SWIFT</td>
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
                                                                    <table width="100%" cellpadding="0" cellspacing="5px">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%;"valign="center">
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divSWIFTRemRemitterInfo" >
                                                                                <table id="tblSWIFTRemRemitter" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblSWIFTRemName" class="cell_desc" style="font-size: medium;" >&nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblSWIFTRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table> 
                                                                             <div style ="width:98%; height:100%; margin:0; " class="table_body" id="divSWIFTRemBeneficiaryInfo">
                                                                                 <table id="tblSWIFTRemBeneficiary" width="100%" cellpadding="0" cellspacing="8" style="border:solid 1px #336699;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height:23px">Beneficiary :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblSWIFTRemBeneName" class="cell_desc" style="font-size: medium; ">&nbsp; Janet Romasanta</label></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblSWIFTRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                     </tr>
                                                                                 </table>
                                                                              </div>
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                              </table> 
                                                                              <div style ="width:98%; height:100%; margin:0; " class="table_body" id="divSWIFTRemBankReceiver">
                                                                                <table id="tblSWIFTRemBankReceiver" width="100%" cellpadding="0" cellspacing="8" style="border:solid 1px #336699;">
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px">Bank/Receiver:&nbsp;</td>
                                                                                         <td colspan=4 align="left" style="width:85%; height: 24px;"><select id="ddlSWIFTRemBankReceiver" style="width:100%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 26px">Acount No :&nbsp;</td>
                                                                                         <td align="left" style="width: 35%; height: 26px;"><input id="txtSWIFTRemAcountNo" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                         <td class="cell_caption" style="width: 20%; height: 26px">A/C Currency :&nbsp;</td>
                                                                                         <td align="left" style="width:30%; height: 26px;"><select id="ddlSWIFTRemACCurrency" style="width:98%; font-weight:normal";/></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px">Bank Name :&nbsp;</td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblSWIFTRemBankName" class="cell_desc" style="font-size: medium; ">&nbsp; Industrial and Commercial Bank of Vietnam</label></td>
                                                                                    </tr>
                                                                                 
                                                                                   <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Branch Info :&nbsp;</td>
                                                                                        <td align="left" style="width:35%"><label id= "lblSWIFTRemBranchInfo" class="cell_desc" style="font-size: medium; ">&nbsp; HEAD OFFICE</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">City/Region :&nbsp;</td>
                                                                                        <td align="left" style="width:35%"><label id= "lblSWIFTRemCityRegion" class="cell_desc" style="font-size: medium; ">&nbsp; Hanoi</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan=2 align="left" style="width:50%; height: 24px;"> <input id="chkSWIFTRemGenerateMT103" style=" width:auto;" type="checkbox" onclick="javascript:RemTran_SWIFT.generate_mt('divSWIFTRemMessageOne')" /><label id= "lblSWIFTRemGenerateMT103" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Generate MT103</label></td>
                                                                                        <td colspan=2 align="left" style="width:50%; height: 24px;"> <input id="chkSWIFTRemGenerateMT102" style=" width:auto;" type="checkbox" onclick="javascript:RemTran_SWIFT.generate_mt('divSWIFTRemMessageTwo')"/><label id= "lblSWIFTRemGenerateMT102" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Generate MT202</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                              </div>
                                                                               <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                              </table>
                                                                               <div style="width:98%; height:100%; margin:0; display:none;" class="table_body" id="divSWIFTRemMessageOne">
                                                                                 <table id="tblSWIFTRemMessageOne" width="100%" cellpadding="0" cellspacing="2" style="border:solid 1px #336699;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 1:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT103Line1" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 2:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT103Line2" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 3:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT103Line3" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 4:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT103Line4" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 5:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT103Line5" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                 </table>
                                                                             </div>
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                              </table>  
                                                                               <div style="width:98%; height:100%; margin:0; display:none;>" class="table_body" id="divSWIFTRemMessageTwo">
                                                                                 <table id="tblSWIFTRemMessageTwo" width="100%" cellpadding="0" cellspacing="2" style="border:solid 1px #336699;">
                                                                                             <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 1:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT202Line1" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 2:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT202Line2" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 3:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT202Line3" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 26px;">Message Line 4:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT202Line4" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                         <td class="cell_caption" style="width: 15%">Message Line 5:&nbsp; </td>
                                                                                         <td align="left" style="width: 85%; height: 26px;"><input id="txtSWIFTRemMT202Line5" type="text" style="width:98%; font-weight:normal;" /></td>
                                                                                    </tr>
                                                                                 </table>
                                                                             </div>
                                                                              <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                              </table> 
                                                                             <div style="width:98%; height:100%; margin:0 " class="table_body" id="divSWIFTRemAmountInfo">
                                                                                 <table id="tblSWIFTRemAmount" width="100%" cellpadding="0" cellspacing="8" style="border:solid 1px #336699;">
                                                                                     <tr>
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Total Amount (Rmt Ccy): </td>
                                                                                          <td align="right" style="width:20%"><label id= "lblSWIFTRemTARC" class="cell_desc" style="font-size: medium;">&nbsp; 100,000.00</label></td>
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Net Remit Amount (Rmt Ccy):&nbsp;</td>
                                                                                          <td align="right" style="width:20%"><label id= "lblSWIFTRemNRARC" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 100,000.00</label></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Net Remit Amount (Php Equiv): </td>
                                                                                          <td align="right" style="width:20%"><label id= "lblSWIFTRemNRAPE" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 100,000.00</label></td>
                                                                                          <td class="cell_caption" style="width: 30%; height:23px">Net to Beneficiary (A/C Ccy):&nbsp;</td>
                                                                                          <td align="right" style="width:20%"><label id= "lblSWIFTRemNTBACC" class="cell_desc" style="font-size: medium;">&nbsp; 100,000.00</label></td>
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
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" onclick="javascript:RemTran_SWIFT.load_transaction_view_details();" value="View Transaction Details" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
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
