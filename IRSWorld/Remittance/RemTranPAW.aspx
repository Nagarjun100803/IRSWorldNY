
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranPAW.aspx.cs" Inherits="IRSWorld.Remittance.RemTranPAW" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Remittance - Pay Anywhere in the World</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
        <style type="text/css"></style>
        <script src="../js/jsCommons.js" type="text/javascript"></script>
        <script src="../js/jscommonmethod.js" type="text/javascript"></script>
        <script src="../js/js_sidebar.js"  type="text/javascript"></script>
        <script src="../js/Remittance/jsRemTranPAW.js" type="text/javascript"></script>
        <script src = "../js/jsmodalwindow.js"           type="text/javascript"></script>
        <script src = "../js/Remittance/jsremsaving.js"  type="text/javascript"></script>
        <script src = "../js/Remittance/jsremcommons.js" type="text/javascript"></script>
        <script type="text/javascript">
            var RemTran_PAW = new RemTran_PAW();
    
        </script>
        
</head>
<body onload="javascript:RemTran_PAW.construct_side_bar('divtdRemPayAnywhereInTheWorldMenu')">
    <form id="form1" runat="server">
    <div style="width:100%">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divRemPayAnywhereInTheWorld" style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>
                                    <td id = "tdRemPayAnywhereInTheWorldMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdRemPayAnywhereInTheWorldMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
                                            //var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>
                                    </div>
                                     <a id="ahider" href="#" onclick="javascript:RemTran_PAW.set_side_bar_visibility('divtdRemPayAnywhereInTheWorldMenu', 'ahider')">«</a>               
                                    </td>                                
                                    <td style="width: 77%; padding-top: 30px">
                                        <table width="96%"  cellspacing="0" cellpadding="0" style="background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Remittance - Pay Anywhere in the World</td>
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
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divPAWRemitterInfo" >
                                                                                <table id="tblPAWRemRemitter" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblPAWRemName" class="cell_desc" style="font-size: 15px; font-weight: medium;" >&nbsp;Alvin C. Santos</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px;">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblPAWRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table> 
                                                                           <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divPAWRemBeneficiaryInfo" >
                                                                                 <table id="tblPAWRemBeneficiary" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%" colspan = "3"><label id="lblPAWRemBeneName" class="cell_desc" style="font-size: medium;" >&nbsp;Janet S. Romasanta</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 34px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 34px;" colspan = "3"><label id="lblPAWRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                    </tr>
                                                                                  
                                                                               </table>
                                                                           </div>
                                                                             <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td> 
                                                                                </tr> 
                                                                           </table>
                                                                           <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divPAWRemAmountInfo">                                                                               
                                                                                 <table id="tblPAWRemAmount" width="100%" cellpadding="0" cellspacing="8" style="border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblPAWRemTARC" class="cell_desc" style="font-size: 15px; font-weight: bold;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblPAWRemNRARC" class="cell_desc" style="font-size: 15px; font-weight: normal;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblPAWRemNRAPE" class="cell_desc" style="font-size: 15px; font-weight: normal;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 27%; height: 23px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 23%; height: 23px;"><label id="lblPAWRemNTBACC" class="cell_desc" style="font-size: 15px; font-weight: bold;" >100,000.00 &nbsp;</label></td>
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
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" value="View Transaction Details"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" onclick="javascript:RemTran_PAW.load_transaction_view_details()"/></td>
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
