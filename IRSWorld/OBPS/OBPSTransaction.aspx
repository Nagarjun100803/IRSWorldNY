<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OBPSTransaction.aspx.cs" Inherits="IRSWorld.OBPS.OBPSTransaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch Defaults</title>
 <link href="../css/Default.css" rel="stylesheet"       type="text/css" />
 <script src="../js/jsCommons.js" type="text/javascript"></script>
 <script src="../js/jscommonmethod.js" type="text/javascript"></script>
 <!--<script src="../js/BranchSetup/js_branchdefaults.js" type="text/javascript"> </script>-->
 
     <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>        
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    
    
    <script src="../js/OBPS/js_OBPSTransaction.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
 
 
</head>
   
  

<body onload="page_setup();" onunload="">

    <form id="form1" runat="server">
 
             
    <div id="lblValues"></div>
    </form>
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_parent"></form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../OBPS/OBPSTransaction.aspx" target="_parent">
        <table cellpadding="0" cellspacing="5" width="100%">
            <tr>
                <td align="left" class="content_height" colspan="2" style="width: 80%; border-collapse: collapse"
                    valign="middle">
                    <table id="tblSubMain" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="table_head_dark" style="width: 50%; height: 26px">
                    Overseas Bills Payment</td>
                            <td align="left" style="width: 50%; height: 26px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table id="tblBillsPayment" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                    border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid">
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                            <div id="divBPRemitter" style="width: 100%">
                                                <table id="TABLE1" cellpadding="0" style="color: #0000ff" width="100%">
                                                    <tr id="Rem">
                                                        <td style="border-left-color: royalblue; border-bottom-color: royalblue; width: 50%;
                                                            border-top-style: solid; border-top-color: royalblue; border-right-style: solid;
                                                            border-left-style: solid; height: 140px; border-right-color: royalblue; border-bottom-style: solid"
                                                            >
                                                            <table id="head" width="100%">
                                                                <tr>
                                                                    <td class="table_caption"  style="width: 85%; color: black; text-indent: 5px;
                                                                        height: 26px; text-align: left" valign="top">
                                                                        REMMITER INFO. &nbsp;</td>
                                                                        
                                                                </tr>
                                                            
                                                            </table>
                                                            <table id="search" width=100% style="left">
                                                                 
                                                                <tr>
                                                                 <td class="cell_caption" style="width: 25%; height: 24px">
                                                                        Walk-In:&nbsp;</td>
                                                                    <td style="width: 126px">
                                                                        
                                                                        <input id="Radio2" type="radio" style="width: 69%" /></td>
                                                                    <td class="cell_caption" style="width: 18%; height: 24px">
                                                                        Existing:&nbsp;</td>
                                                                    <td>
                                                                        
                                                                        <input id="Radio1" style="width: 69%" type="radio" /></td>
                                                                </tr>
                                                                
                                                                
                                                            
                                                            </table>
                                                           
                                                            <table id="btnsearch" width=60%>
                                                            <tr>
                                                                    <td style="width: 205px">
                                                                    <input id="btnSearchRemitter" class="enable_button" onclick="getRemitterInfo();"
                                                                            onmouseout="this.className='enable_button';" onmouseover="this.className='mouseover_button';"
                                                                            type="button" value="Search" style="width: 100%" />
                                                                    </td>
                                                                    <td style="width: 220px">
                                                                    <input id="btnNew" class="enable_button"
                                                                            onmouseout="this.className='enable_button';" onmouseover="this.className='mouseover_button';"
                                                                            type="button" value="New" style="width: 100%" />
                                                                    </td>
                                                                </tr>
                                                            
                                                            </table>
                                                         
                                                            
                                                            
                                                            
                                                            <table id="tblBPRemitter" cellpadding="0" width="100%">
                                                                
                                                               
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 24px">
                                                                        Remitter ID:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 85%; height: 24px">
                                                                        <label id="lbl_rem_id">
                                                                            12345</label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 558px; height: 24px">
                                                                        Complete Name:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 85%; height: 24px">
                                                                        <label id="lbl_rem_fullname">
                                                                            Dizon, Michael Ray C.</label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 558px; height: 24px">
                                                                        Address:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 85%; height: 24px">
                                                                        <label id="lbl_rem_address">
                                                                            Malabon City</label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 558px; height: 24px">
                                                                        Telephone Number:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 85%; height: 24px">
                                                                        <label id="lbl_rem_telno">
                                                                            4478886</label></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="Mer">
                                                        <td style="border-left-color: royalblue; border-bottom-color: royalblue; width: 50%;
                                                            border-top-style: solid; border-top-color: royalblue; border-right-style: solid;
                                                            border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid"
                                                            valign="top">
                                                            <table id="tblBPMerchants" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td class="table_caption" colspan="2" style="width: 85%; color: black; text-indent: 5px;
                                                                        height: 26px; text-align: left" valign="top">
                                                                        MERCHANT INFO.</td>
                                                                    <td align="left" style="width: 15%; height: 24px">
                                                                        <input id="btnSearchMerchant" class="enable_button" 
                                                                            onmouseout="this.className='enable_button';" onmouseover="this.className='mouseover_button';"
                                                                            type="button" value="Search" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 31%; height: 24px">
                                                                        Merchant Name:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                        <label id="lbl_merchant_name">
                                                                        </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 31%; height: 24px">
                                                                        ID:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                        <label id="lbl_merchant_id">
                                                                        </label>
                                                                        <input id="txtMemberID" style="display: none; width: 300px" /></td>
                                                                </tr>
                                                                <%--<tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Account No:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "lbl_merchant_acctno"></label></td>                                                                                        
                                                                                </tr>       --%>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 31%; height: 24px">
                                                                        Account Currency:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                        <label id="lbl_merchant_currency">
                                                                        </label>
                                                                        <select id="ddMAccountCurrency" style="display: none; width: 300px">
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 31%; height: 24px">
                                                                        Product Code:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                        <label id="lbl_merchant_prodcode">
                                                                        </label>
                                                                        <select id="ddMProductCode" style="display: none; width: 300px">
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 31%; height: 24px">
                                                                        Target Amount:&nbsp;</td>
                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                        <input id="txtTargetAmount" onkeydown="javascript:return isNumeric(event.keyCode);"
                                                                            onkeyup="javascript:keyUP(event.keyCode);" style="display: none; width: 300px" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr id="MerD">
                                                        <td style="border-left-color: royalblue; border-bottom-color: royalblue; width: 50%;
                                                            border-top-style: solid; border-top-color: royalblue; border-right-style: solid;
                                                            border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid; height: 78px;"
                                                            valign="top">
                                                            <table id="tblMDetails" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td class="table_caption" colspan="3" style="width: 85%; color: black; text-indent: 5px;
                                                                        height: 26px; text-align: left" valign="top">
                                                                        MERCHANT DETAILS</td>
                                                                </tr>
                                                                <tr>
                                                                    <td id="merchdetails">
                                                                    
                                                                    </td>
                                                                
                                                                </tr>
                                                                
                                                                
                                                                
                                                                
                                                                <%--<tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Merchant Name:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "Label1"></label></td>                                                                                        
                                                                                </tr>
                                                                                
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Product Name:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "Label6"></label></td>                                                                                        
                                                                                </tr>--%>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                        <td id="trxncolumn" style="width: 50%; height: 538px" valign="top">
                                            <div id="divtransactiondetails" style="width: 100%">
                                                <table cellpadding="0" width="100%" >
                                                    <tr>
                                                        <td style="border-left-color: royalblue; border-bottom-color: royalblue; width: 100%;
                                                            border-top-style: solid; border-top-color: royalblue; border-right-style: solid;
                                                            border-left-style: solid; height: auto; border-right-color: royalblue; border-bottom-style: solid"
                                                            valign="top">
                                                            <table id="tblTellerTxnDetails" cellpadding="0" width="100%">
                                                                <tr>
                                                                    <td class="table_caption" colspan="4" style="width: 330px; color: black; text-indent: 5px;
                                                                        text-align: left" valign="top">
                                                                        TRANSACTION DETAILS</td>
                                                                    <%--                                                                                    <td align="left" style="width: 70px; height: 24px;"> </td>                         
--%>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 31px">
                                                                        Remitting Currency:&nbsp;</td>
                                                                    <td align="left" style="width: 36px; height: 31px">
                                                                        <select id="ddRemCurrency" style="width: 265px; height: 24px">
                                                                        </select>
                                                                    </td>
                                                                    <%--                                                                                </tr>                                                                                
                                                                                 <tr>--%>
                                                                   
                                                                </tr>
                                                                <tr>
                                                                     <td class="cell_caption" style="width: 160px; height: 31px">
                                                                        Payment&nbsp; Mode:&nbsp;</td>
                                                                    <td align="left" style="width: 70px; height: 31px">
                                                                        <select id="ddPayMode" style="width: 266px; height: 24px">
                                                                        </select>
                                                                    </td>
                                                                
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 24px">
                                                                        Source of Funds:&nbsp;</td>
                                                                    <td align="left" colspan="3" style="width: 200px">
                                                                        <select id="ddSourceFunds" style="width: 266px; height: 24px">
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 24px">
                                                                        Check No:</td>
                                                                    <td align="left" style="width: 36px; height: 24px">
                                                                        <input id="txtCheckNo" onkeydown="javascript:return isNumeric(event.keyCode);" onkeyup="javascript:keyUP(event.keyCode);"
                                                                            style="width: 266px; height: 18px" type="text" /></td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 24px">
                                                                        Clearance&nbsp; Date :</td>
                                                                    <td align="left" style="width: 90px; height: 24px">
                                                                        <input id="txtClearDate" readonly="readonly" style="width: 266px; height: 19px" type="text" /><img
                                                                            alt="Pick a date." onclick="popUpCalendar(this, document.getElementById('txtClearDate'), 'mm/dd/yyyy');"
                                                                            src="../images/calendar/imgcalen.png" id="imgdt" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="cell_caption" colspan="4" style="width: 330px; height: 24px">
                                                                        
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 5px">
                                                                        Net Remit Amount(Rmt Ccy) :</td>
                                                                    <td align="left" colspan="3" style="width: 70px; height: 5px;">
                                                                        <input id="Text1" readonly="readonly" style="width: 251px; height: 24px" type="text" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 24px">
                                                                        Total Amount(Rmt Ccy):</td>
                                                                    <td align="left" colspan="3" style="width: 70px">
                                                                        <input id="Text2" readonly="readonly" style="width: 251px; height: 24px" type="text" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 24px">
                                                                        Net Remit Amount(PHP Equiv):</td>
                                                                    <td align="left" colspan="3" style="width: 70px">
                                                                        <input id="Text3" readonly="readonly" style="width: 251px; height: 24px" type="text" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 160px; height: 24px">
                                                                        Net To Merchant :</td>
                                                                    <td align="left" colspan="3" style="width: 70px">
                                                                        <input id="Text4" readonly="readonly" style="width: 251px; height: 24px" type="text" /></td>
                                                                </tr>
                                                                <%-- <tr>
                                                                                    <td class="cell_caption" style="width: 130px; height: 24px" >Purpose:&nbsp;</td>
                                                                                    <td align="left" style="width: 200px" colspan = "3"><select id="ddPurpose" style="width: 300px; height: 24px"></select></td>
                                                                                </tr>  
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 130px; height: 24px" >IRS Product Code:&nbsp;</td>
                                                                                    <td align="left" style="width: 200px" colspan = "3"><select id="ddProductCode" style="width: 300px; height: 24px"></select></td>
                                                                                </tr>  
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 130px; height: 24px" >IRS Implementation:&nbsp;</td>
                                                                                    <td align="left" style="width: 200px" colspan = "3"><select id="ddImplementation" style="width: 300px; height: 24px"></select></td>
                                                                                </tr>  --%>
                                                            </table>
                                                            <%--<asp:TextBox ID="txt" runat="server"></asp:TextBox>--%>
                                                            <input id="btnCharges" class="enable_button" 
                                                                            onmouseout="this.className='enable_button';" onmouseover="this.className='mouseover_button';"
                                                                            type="button" value="View Charges" style="width: 50%" />
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
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
