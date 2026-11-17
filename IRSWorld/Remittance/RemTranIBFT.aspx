
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranIBFT.aspx.cs" Inherits="IRSWorld.Remittance.RemTranIBFT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Remittance - InterBank Fund Transfer</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
        <style type="text/css"></style>
        <script src="../js/jsCommons.js" type="text/javascript"></script>
        <script src="../js/jscommonmethod.js" type="text/javascript"></script>
        <script src="../js/js_sidebar.js"  type="text/javascript"></script>
        <script src = "../js/jsmodalwindow.js" type="text/javascript"></script>
        <script src = "../js/Remittance/jsremsaving.js"  type="text/javascript"></script>
        <script src = "../js/Remittance/jsremcommons.js" type="text/javascript"></script>
        <script src="../js/Remittance/jsRemTranIBFT.js" type="text/javascript"></script>
        <script type="text/javascript">
            var RemTran_IBFT = new RemTran_IBFT();
    
        </script>
    
</head>
<body onload="javascript:RemTran_IBFT.load_all_dropdownlist(); RemTran_IBFT.construct_side_bar('divtdRemInterBankFundTransferMenu');">
    <form id="form1" runat="server">
    <div style="width:100%">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divRemInterBankFundTransfer" style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td id = "tdRemInterBankFundTransferMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdRemInterBankFundTransferMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
                                            //var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>
                                    </div>
                                     <a id="ahider" href="#" onclick="javascript:RemTran_IBFT.set_side_bar_visibility('divtdRemInterBankFundTransferMenu', 'ahider');">«</a>               
                                    </td>                                
                                    <td style="width: 78%; padding-top: 30px">
                                        <table width="96%"  cellspacing="0" cellpadding="0" style="background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Remittance - InterBank Fund Transfer</td>
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
                                                                            <td style="width: 78%; height: 100%;"valign="center">
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divIBFTRemitterInfo" >
                                                                                <table id="tblIBFTRemRemitter" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblIBFTRemName" class="cell_desc" style="font-size: medium;" >&nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px;">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblIBFTRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table> 
                                                                          <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divIBFTRemBeneficiaryInfo" >
                                                                                 <table id="tblIBFTRemBeneficiary" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%" colspan = "3"><label id="lblIBFTRemBeneName" class="cell_desc" style="font-size: medium;" >&nbsp;Janet S. Romasanta</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%;" colspan = "3"><label id="lblIBFTRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div>   
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px; width: 3px;">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table> 
                                                                               <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divIBFTRemAccountInfo">                                                                               
                                                                                 <table id="tblIBFTRemAccount" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 24%; height: 23px">Megalink Member Name :&nbsp;</td>
                                                                                        <td style="width:33%; height:24px;"><select id="ddlIBFTRemMegalinkMemberName" style="width:98%; text-align:left; font-weight: normal;"></select></td>
                                                                                        <td class="cell_caption" style="width: 13%; height: 24px">A/C Ccy :&nbsp;</td>
                                                                                        <td style="width:38%; height:24px;"><select id="ddlIBFTRemACCcy" style="width:100%; text-align:left; font-weight: normal;"></select></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 24%; height: 24px">Account Number :&nbsp;</td>
                                                                                        <td style="width:33%; height:24px;"><input type="text" id="txtIBFTRemAcctNo" style="width:96%; text-align:left; font-weight:normal;" /></td>
                                                                                        <td class="cell_caption" style="width:13%; height: 24px">Account Type :&nbsp;</td>
                                                                                        <td style="width: 7%; height: 24px;"><select id="ddlIBFTRemAcctType" style="width:100%; text-align:left; font-weight:normal;"></select></td>
                                                                                       
                                                                                        
                                                                                    </tr>
                                                                                    
                                                                                    <tr>
                                                                                        
                                                                                    </tr>
                                                                               </table>
                                                                               </div>
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px; width: 3px;">
                                                                                        </td> 
                                                                                    </tr> 
                                                                                </table> 
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divIBFTRemAmountInfo">                                                                               
                                                                                 <table id="tblIBFTRemAmount" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblIBFTRemTARC" class="cell_desc" style="font-size: 15px; font-weight: bold;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblIBFTRemNRARC" class="cell_desc" style="font-size: 15px; font-weight: normal;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblIBFTRemNRAPE" class="cell_desc" style="font-size: 15px; font-weight: normal;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblIBFTRemNTBACC" class="cell_desc" style="font-size: 15px; font-weight: bold;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px; width: 3px;">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table>
                                                                            
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" value="View Transaction Details"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" onclick="javascript:RemTran_IBFT.load_transaction_view_details();"/></td>
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
