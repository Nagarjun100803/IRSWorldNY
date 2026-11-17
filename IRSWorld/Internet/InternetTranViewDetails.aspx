<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetTranViewDetails.aspx.cs" Inherits="IRSWorld.Internet.InternetTranViewDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Internet - Transaction Details</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsremsaving.js" type="text/javascript"></script>
    <script src="../js/Internet/jsInternetTranViewDetails.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        var InternetTran_Details = new InternetTran_Details();
    </script>
                
</head>
<body onload="javascript:InternetTran_Details.load_transaction_details();">
    <form id="form1" runat="server">
        <div style="width:100%">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divInternetTranView" style="width: 100%; height: 100%; display: block">
                            <table style="width:100%">
                                <tr>
                                    <td style="width: 80%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">View Transaction Details</td>
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
                                                                            <input style="height: 28px; width: 10%" type="button" value="Post" onclick="javascript:InternetTran_Details.post_transaction();" class="button" />&nbsp; &nbsp;&nbsp;
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
            </div>   
    </form>
</body>
</html>
