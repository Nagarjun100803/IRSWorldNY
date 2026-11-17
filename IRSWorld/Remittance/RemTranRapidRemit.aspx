<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranRapidRemit.aspx.cs" Inherits="IRSWorld.Remittance.RemTranRapidRemit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remittance - Rapid Remit</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    
    <script src = "../js/jsCommons.js"               type="text/javascript"></script>
    <script src = "../js/jscommonmethod.js"          type="text/javascript"></script>
    <script src = "../js/js_sidebar.js"              type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"           type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving.js"  type="text/javascript"></script>
    <script src = "../js/Remittance/jsremcommons.js" type="text/javascript"></script>
    <script src = "../js/Remittance/jsRemTranRapidRemit.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var rapid_remit = new rem_rapid_remit();
    </script>

</head>
<body onload="javascript:rapid_remit.construct_side_bar('divtdRemRapidRemitMenu');">
    <form id="form1" runat="server">
        <div style="width:100%">
        <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divRemRapidRemit" style="width: 100%; height: 100%; display: block">
                            <table style="width:100%">
                                <tr>
                                    <td id = "tdRemRapidRemitMenu" valign="top" style="width:20%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    
                                    <div id = "divtdRemRapidRemitMenu" style="margin-top: 20px;display:block;">
                                        <script type="text/javascript">
                                            //var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>
                                    </div>
                                     <a id="ahider" href="#" onclick="javascript:rapid_remit.set_side_bar_visibility('divtdRemRapidRemitMenu','ahider');">«</a>
                                    </td>                                
                                    <td style="width: 80%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Remittance - Rapid Remit</td>
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
                                                                
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divRRRemRemitterInfo" >
                                                                                <table id="tblRRRemRemitter" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Remitter :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblRRRemName" class="cell_desc" style="font-size: medium;" >&nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%"><label id="lblRRRemAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                            <br />
                                                                             
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divRRRemBeneficiaryInfo" >
                                                                                 <table id="tblRRRemBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%" colspan = "3"><label id="lblRRRemBeneName" class="cell_desc" style="font-size: medium;" >&nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 34px">Address :&nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 34px;" colspan = "3"><label id="lblRRRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">PNB Branch :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblRRRemPNBBranch" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">CA / SA No. :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblRRRemBeneCASANo" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;</label></td>                                                                                        
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">Account Type :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblRRRemBeneAccountType" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">A/C Currency :&nbsp;</td>
                                                                                        <td align="left" style="width: 35%"><label id="lblRRRemBeneACCurrency" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;</label></td>                                                                                        
                                                                                    </tr>
                                                                                </table>
                                                                             </div>   
                                                                             
                                                                             <br />
  
                                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="divRRRemAmountInfo" >                                                                               
                                                                                 <table id="tblRRRemAmount" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><label id="lblRRRemTARC" class="cell_desc" style="font-size: 15px; font-weight: bold;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><label id="lblRRRemNRARC" class="cell_desc" style="font-size: 15px; font-weight: normal;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><label id="lblRRRemNRAPE" class="cell_desc" style="font-size: 15px; font-weight: normal;" > 100,000.00&nbsp;</label></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 23px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 23px;"><label id="lblRRRemNTBACC" class="cell_desc" style="font-size: 15px; font-weight: bold;" >100,000.00 &nbsp;</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                             </div> 
                                                                             
                                                                             <br />

                                                                            
                                                                            </td>
                                                                        </tr>
                                                                       <tr>
                                                                            <td style="height: 28px; width: 100%" valign="top" ><input style="height: 28px; width: 30%" type="button" value="View Transaction Details" onclick="javascript:rapid_remit.load_transaction_view_details();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"/></td>
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
