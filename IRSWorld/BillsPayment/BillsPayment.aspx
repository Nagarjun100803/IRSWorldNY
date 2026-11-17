<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillsPayment.aspx.cs" Inherits="IRSWorld.BillsPayment.BillsPayment" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>

<html>
<head>
    <title>Bills Payment</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>        
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    
    
    <script src="../js/BillsPayment/jsBillsPayRemTeller.js" type="text/javascript"></script>
    
    <script src="../js/popcalendar.js" type="text/javascript"></script>

    <script language = "javascript" type="text/javascript">
        var commons = new common_method();        
//        var remteller = new rem_teller(commons);
        var remteller = new bills_rem_teller();
        var bills = new frm_bills();
        
        function execute_ajx_command(param)
        {            
            if (remteller != null && remteller != "undefined")
            {
                remteller.do_ajax(param);                 
            }             
        }
        
        function isNumeric(keyCode) 
        { 
            return ((keyCode >= 48 && keyCode <= 57) || keyCode == 8 || 
                    (keyCode >= 96 && keyCode <= 105)) 
        } 

	    var isShift=false; 

	    function keyUP(keyCode) 
	    { 
		    if(keyCode==16)
				    isShift = false; 
	    } 

	    function isNumeric(keyCode) 
	    { 
		    if(keyCode==16) 
				    isShift = true; 	  

		    return ((keyCode >= 48 && keyCode <= 57 || keyCode == 8 ||
				    (keyCode >= 96 && keyCode <= 105)) && isShift == false); 
	    } 
	    
	    function load_main()
        {
            if (top == self)
            {
                document.irs_loader.submit();            
            }
        }
    </script>
    
</head>
<body onload = "javascript:load_main();bills.hide_panel();">
    <form id="form1" runat="server">

        <table style="width:100%" cellpadding="0" cellspacing="5">            
            <tr>
              <td>
                <table style="width:100%" cellpadding="0" cellspacing="5">
                        <tr>
                            <td align="center" valign="middle" style="width:80%; border-collapse:collapse;" class="content_height" colspan = "2">                               
                                <table id = "tblSubMain" style="width:100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="table_head_dark"  style="width:50%; height: 26px;">Overseas Bills Payment</td>
                                        <td align="left" valign="bottom" style="width: 50%; height: 26px;"><img align="absbottom" src="../images/rend.gif" /></td>                                            
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table id = "tblBillsPayment" style="width:100%; border:solid 3px #336699;">
                                                <tr></tr>
                                                <tr>
                                                        <td style="width: 56%;" valign="top">
                                                            <!-- content starts here, if necessary increase/decrease the size of the bounding table -->                                                                
                                                            <div id = "divBPRemitter" style = "width:100%; overflow:scroll">
                                                                <table cellpadding="0" style="width:100%; color: #0000ff" id="TABLE1">
                                                                    <tr id = "Rem">
                                                                        <td style="width: 50%; height: 140px; border-left-color: royalblue; border-bottom-color: royalblue; border-top-style: solid; border-top-color: royalblue; border-right-style: solid; border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid;"valign="middle">
                                                                            <table id="tblBPRemitter" style="width:100%" cellpadding="0" >
                                                                                <tr>
                                                                                    <td class="table_caption" colspan = "2" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" > REMMITER INFO. &nbsp;</td>
                                                                                    <td align="left" style="width: 15%; height: 24px;"><input id="btnSearchRemitter" onclick = "javascript:remteller.callLookUp('REMITTER');" class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';" type="button" value="Search"/></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Remitter ID:&nbsp;</td>
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "lbl_rem_id"></label></td>                                                                                        
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px" >Complete Name:&nbsp;</td>
                                                                                    <td align="left" style="width: 75%; height: 24px;" colspan = "2"><label id = "lbl_rem_fullname"></label></td>
                                                                               </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px" >Address:&nbsp;</td>
                                                                                    <td align="left" style="width: 75%; height: 24px;" colspan = "2"><label id = "lbl_rem_address"></label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px" >Telephone Number:&nbsp;</td>
                                                                                    <td align="left" style="width: 75%; height: 24px;" colspan = "2"><label id = "lbl_rem_telno"></label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id = "Mer">
                                                                        <td style="width: 50%; border-left-color: royalblue; border-bottom-color: royalblue; border-top-style: solid; border-top-color: royalblue; border-right-style: solid; border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid;"valign="top">
                                                                            <table id="tblBPMerchants" style="width:100%" cellpadding="0" >
                                                                                <tr>
                                                                                    <td class="table_caption" colspan = "2" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >MERCHANT INFO.</td>
                                                                                    <td align="left" style="width: 15%; height: 24px;"><input id="btnSearchMerchant" onclick = "javascript:remteller.callLookUp('MERCHANT');" class="enable_button" onmouseover="this.className='mouseover_button';" onmouseout="this.className='enable_button';" type="button" value="Search" /> </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Merchant Name:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "lbl_merchant_name"></label></td>                                                                                        
                                                                                </tr>
                                                                                
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px"><label id = "lbl_MID"></label>&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "lbl_merchant_id"></label><input id = "txtMemberID" style="width:100px; display:none" /></td>                                                                                        
                                                                                </tr>
                                                                                <%--<tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Account No:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "lbl_merchant_acctno"></label></td>                                                                                        
                                                                                </tr>       --%>                                                                         
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Account Currency:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "lbl_merchant_currency"></label><select id = "ddMAccountCurrency" style="width:100px; display:none"></select></td>                                                                                        
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Product Code:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><label id = "lbl_merchant_prodcode"></label><select id = "ddMProductCode" style="width:100px; display:none"></select></td>                                                                                        
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 31%; height: 24px">Target Amount:&nbsp;</td>                                                                                        
                                                                                    <td align="left" colspan="2" style="width: 75%; height: 24px"><input id="txtTargetAmount" style="width:100px; display:none" onkeydown="javascript:return isNumeric(event.keyCode);" onkeyup="javascript:keyUP(event.keyCode);"/></td>                                                                                        
                                                                                </tr>

                                                                            </table>
                                                                        </td>
                                                                    </tr>         
                                                                    <tr id = "MerD">
                                                                        <td style="width: 50%; border-left-color: royalblue; border-bottom-color: royalblue; border-top-style: solid; border-top-color: royalblue; border-right-style: solid; border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid;"valign="top">
                                                                            <table id="tblMDetails" style="width:100%" cellpadding="0" >
                                                                                <tr>
                                                                                    <td class="table_caption" colspan = "3" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >MERCHANT DETAILS</td>
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
                                                        <td id="trxncolumn" style="width: 50%; height: 538px;" valign="top"> 
                                                            <div id = "divtransactiondetails" style = "width:100%; overflow:scroll">
                                                                <table cellpadding="0" style="width: 125px" >
                                                                    <tr>
                                                                        <td style="width: 70%; height: auto; border-left-color: royalblue; border-bottom-color: royalblue; border-top-style: solid; border-top-color: royalblue; border-right-style: solid; border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid;"valign="top">
                                                                            <table id="tblTellerTxnDetails" style="width:100%" cellpadding="0" >
                                                                                <tr>
                                                                                    <td class="table_caption" colspan = "4" style="width: 330px; text-align: left; text-indent: 5px; color: Black" valign="top">TRANSACTION DETAILS</td>                            
<%--                                                                                    <td align="left" style="width: 70px; height: 24px;"> </td>                         
--%>                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 170px; height: 31px">Remitting Currency:&nbsp;</td>
                                                                                    <td align="left" style="width: 36px; height: 31px;"><select id="ddRemCurrency" style="width: 87px; height: 24px"></select></td>
<%--                                                                                </tr>                                                                                
                                                                                 <tr>--%>
                                                                                    <td class="cell_caption" style="width: 50px; height: 31px" >Payment Mode:&nbsp;</td>
                                                                                    <td align="left" style="width: 70px; height: 31px;"><select id="ddPayMode" style="width: 100px; height: 24px"></select></td>
                                                                                </tr>                                                                                
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 170px; height: 24px" >Source of Funds:&nbsp;</td>
                                                                                    <td align="left" style="width: 200px" colspan = "3"><select id="ddSourceFunds" style="width: 248px; height: 24px"></select></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 170px; height: 24px" >Check No:</td>
                                                                                    <td align="left" style="width: 36px; height: 31px;"><input id="txtCheckNo" type="text" style="width: 90px; height: 24px" onkeydown="javascript:return isNumeric(event.keyCode);" onkeyup="javascript:keyUP(event.keyCode);" /></td>
                                                                                    <td class="cell_caption" style="width: 50px; height: 24px" >Clearance Date :</td>
                                                                                    <td align="left" style="width: 90px; height: 31px;"><input id="txtClearDate" type="text" readonly="readonly" style="width: 72px; height: 24px" /><img src="../images/calendar/imgcalen.png" alt="Pick a date." onclick="popUpCalendar(this, document.forms[0].txtClearDate, 'mm/dd/yyyy');"/></td>
                                                                                
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="cell_caption" style="width: 330px; height: 24px" colspan="4"><hr style="width:429px" /></td>
                                                                                    
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 170px; height: 24px" >Net Remit Amount(Rmt Ccy) :</td>
                                                                                    <td align="left" style="width: 70px" colspan = "3"><input id="Text1" type="text" readonly="readonly" style="width: 251px; height: 24px" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 170px; height: 24px" >Total Amount(Rmt Ccy):</td>
                                                                                    <td align="left" style="width: 70px" colspan = "3"><input id="Text2" type="text" readonly="readonly" style="width: 251px; height: 24px" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 170px; height: 24px" >Net Remit Amount(PHP Equiv):</td>
                                                                                    <td align="left" style="width: 70px" colspan = "3"><input id="Text3" type="text" readonly="readonly" style="width: 251px; height: 24px" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 170px; height: 24px" >Net To Merchant :</td>
                                                                                    <td align="left" style="width: 70px" colspan = "3"><input id="Text4" type="text" readonly="readonly" style="width: 251px; height: 24px" /></td>
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
              </td>             
            </tr>            
        </table>    

    


    </form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BillsPayment/BillsPayment.aspx" target="_parent"></form>
</body>
</html>
