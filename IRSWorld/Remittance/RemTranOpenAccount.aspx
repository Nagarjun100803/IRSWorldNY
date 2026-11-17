<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranOpenAccount.aspx.cs" Inherits="IRSWorld.Remittance.RemTranOpenAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remittance - Open Account</title>
<link href="../css/Default.css" rel="stylesheet" type="text/css" />
        <style type="text/css"></style>
        <script src="../js/jsCommons.js" type="text/javascript"></script>
        <script src="../js/jscommonmethod.js" type="text/javascript"></script>
        <script src="../js/js_sidebar.js"  type="text/javascript"></script>
        <script src="../js/Remittance/js_remtranopenaccount.js" type="text/javascript"></script>
        <script type="text/javascript">
            var remtranopenaccount = new remtranopenaccount();
    
        </script>
</head>
<body onload="javascript:remtranopenaccount.load_all_dropdownlist();">
    <form id="form1" runat="server">
        <div style="width:100%">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divRemOpenAccount" style="width: 100%; height: 100%; display: block">
                            <table style="width:100%">
                                <tr>
                                    <td id = "tdRemOpenAccountMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdRemOpenAccountMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                           // document.write(sider.toString());
                                        </script>
                                    </div>
                                     <a id=ahider href=# onclick=javascript:hide_menu()>«</a>               
                                    </td>                                
                                    <td style="width: 80%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%;  background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Remittance - Open Account</td>
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
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divOARemitterInfo" >
                                                                                <table id="tblOARemRemitter" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblOARemName" class="cell_desc" style="font-size: medium;" >&nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px;">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblOARemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table> 
                                                                          <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divOARemBeneficiaryInfo" >
                                                                                 <table id="tblOARemBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%" colspan = "3"><label id="lblOARemBeneName" class="cell_desc" style="font-size: medium;" >&nbsp;Janet S. Romasanta</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 34px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 34px;" colspan = "3"><label id="lblOARemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">PNB Branch :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblOARemPNBBranch" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;1394 PNB Sta. Maria Bulacan</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">CA / SA No. :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblOARemPNBBranchCASANo" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;1111222233334444</label></td>                                                                                        
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Account Type :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblOARemAccountType" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Savings</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">A/C Currency :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblOARemBeneACCurrency" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;PHP</label></td>                                                                                        
                                                                                    </tr>
                                                                                </table>
                                                                             </div>   
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px; width: 3px;">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table> 
  
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divOARemAmountInfo">                                                                               
                                                                                 <table id="tblOARemAmount" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblOARemTARC" class="cell_desc" style="font-size: 15px; font-weight: bold;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblOARemNRARC" class="cell_desc" style="font-size: 15px; font-weight: normal;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblOARemNRAPE" class="cell_desc" style="font-size: 15px; font-weight: normal;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblOARemNTBACC" class="cell_desc" style="font-size: 15px; font-weight: bold;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px; width: 3px;">
                                                                                        </td> 
                                                                                    </tr> 
                                                                               </table>
                                                                            <div style="width:98%; height:100%; margin:0; display:none" class="table_body" id="divOARemCheck">
                                                                                <table id="tblOARemCheck" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699; ">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width:15%; height:24px;">
                                                                                            Check No :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtOARemCheckNo" type="text" style="width:98%; font-weight:normal;" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width:15%; height:24px;">
                                                                                            Check Date :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtOARemCheckDate" type="text" style="width:98%; font-weight:normal";/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width:15%; height:24px;">
                                                                                            Bank :&nbsp;
                                                                                        </td>
                                                                                        <td>
                                                                                            <select id="ddlOARemBank" style="width:98%; font-weight:normal;"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                              </div> 
                                                                             
                                                                                <table>
                                                                                    <tr>
                                                                                        <td style="height: 10px; width: 3px;">
                                                                                        </td> 
                                                                                    </tr> 
                                                                                </table>                                                                         
                                                                              <div style ="width:98%; height:100%; margin:0" class="table_body" id="divOARemPurpose">
                                                                                 <table id="tblOARemPurpose" cellpadding="0" cellspacing="8" style ="width:100%; border: solid 1px #336699;">
                                                                                     <tr> 
                                                                                        <td class="cell_caption" style="width: 15%; height:23px" align="left">Purpose :&nbsp;</td>
                                                                                        <td style="width:34%; height:24px;"><select id="ddlOARemPurpose" style="width:100%; text-align:left; font-weight: normal;"></select></td>
                                                                                        <td style="width:50%; height:24px;"><input type="text" id="txtOARemPurpose" style="width:98%; text-align:left; font-weight:normal;" /></td>
                                                                                     </tr>
                                                                                   </table>
                                                                               </div>
                                                                                   <table>
                                                                                        <tr>
                                                                                            <td style="height: 10px; width: 3px;">
                                                                                            </td> 
                                                                                        </tr> 
                                                                                   </table>                                                 
                                                                           <div style="width:98%; height:100%; display: none; margin:0" class="table_body" id="divOARemMessage">
                                                                                 <table id="tblOARemMessage" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px"; align="left">Message :&nbsp; </td>
                                                                                        <td rowspan="2" align="left" style="width: 85%;">
                                                                                            <textarea cols="1" rows="1" id="txtOARemMessage" style="width: 99%; height: 80px;"></textarea></td>
                                                                                    </tr>
                                                                                    <tr style="height: 50px;"></tr>
                                                                                 </table>
                                                                             </div>
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" value="View Transaction Details"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
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
