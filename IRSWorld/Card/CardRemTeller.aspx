<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CardRemTeller.aspx.cs" Inherits="IRSWorld.Card.CardRemTeller" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Card Remittance Teller</title>    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />        
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script> 
    <script src="../js/CardRem/jsCardRemTeller.js" type="text/javascript"></script>
    <script src="../js/CardRem/jsGFMCMaintenance.js" type="text/javascript"></script> 
    <script src="../js/Remittance/jsRemExternals.js" type="text/javascript"></script> 
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src ="../js/popcalendar.js" type="text/javascript"></script>      
    <script language = "javascript" type="text/javascript">
            
       var card = new card_rem_teller();
       var gfmc = new GFMC_Maintenance();
       var acct = new rem_modal_accounts();
       var openacct = new rem_modal_open_accounts();
       var chkpymnt = new rem_modal_check_payments();
       var popUps = new rem_modal_popups();
       
       function call(MethodID, Parameter)
       {

            try
            {
                var Param; 
                
                if (Parameter != null && Parameter != '')
                {
                       Param = Parameter.split(':');
                }          
               
                //var card = document.getElementById('txtCardNo').value.toString();                   
                switch (MethodID)
                {                
                    case "btnCrDrCard_Click":
                        javascript:card.open_debit_credit_card_form('1', 'card');
                        //alert(card.R);
                        //var a = card.Response();
                        //alert(a.toString())
                        //alert(new card_rem_teller());
                        break;
                    case "btnGFMCCard_Click":
                        javascript:gfmc.open_gfc_form('1752030610000001','','gfmc');
                        break;
                    case "btnAcct_Click":
                        //javascript:acct.open_account_form('1740030810000001', '5', 'acct');
                        javascript:acct.account_form('1126051710000001', '5', 'acct', document.getElementById('txtCardNo').value);
                        break;
                    case "btnOpenAcct_Click":
                        javascript:openacct.open_account_form('5', 'openacct');
                        break;
                    case "btnCheckPayment_Click":
                        javascript:chkpymnt.open_check_payment_form('chkpymnt');
                        break;
                    case "btn_door_to_door_Click":
                        javascript:popUps.open_doortodoor('1752030610000002');
                        break;
                    case "btn_apas_batch_Click":
                        javascript:popUps.open_apas_batch();
                        break;
                    case "btn_apas_online_Click":
                        javascript:popUps.open_apas_online();
                        break;
                    case "btn_paw_Click":
                        javascript:popUps.open_paw();
                        break;
                    case "btn_Commit_Click":
                        //javascript:openacct.commit_transaction("1");
                        chkpymnt.commit_transaction("3");
                        break;
                 }       
            }
            catch(e)
            {
                alert('Error : ' + e);
            }  
       }      
       
       function gfc_response()
       {
            var r;
            r = gfmc.CardDetails();
            
            var msg = ""; 
            for(var i = 0; i < r.length; i++)
            {
                msg += r[i].toString() + ' | ';
            }
           
            alert(msg);  
       }
       
       function push_open_account_result()
       {
            //var response = javascript:acct.Response();
            alert('pumasok!!! ' + openacct.branch_code);
       }
       
       function push_account_result()
       {
            //var response = javascript:acct.Response();
            alert('pumasok!!! ' + acct.account_seqid);
       }
       
       function push_check_payment_result()
       {
            alert('pumasok!!! ' + chkpymnt.check_number);
       }
       
       function push_popup_result()
       {
            alert('pumasok!!!');
       }
       
       function getResponse()
       {
            var r;
            r = gfmc.CardDetails();
            
            var msg = ""; 
            for(var i = 0; i < r.length; i++)
            {
                msg += r[i].toString() + ' | ';
            }
           
            alert(msg);  
       }
       
    </script>
    
</head>
<body> 
    <form id="form1" runat="server">
        &nbsp;<table style="border:solid 3px #336699; width: 380px;">
            <tr>
                                <td style="width: 51%; height: 50px;" valign="top"> 
                                    <table style="width:100%" cellpadding="0" >
                                        <tr>
                                            <td style="width: 54%; height: 121px; border-left-color: royalblue; border-bottom-color: royalblue; border-top-style: solid; border-top-color: royalblue; border-right-style: solid; border-left-style: solid; border-right-color: royalblue; border-bottom-style: solid;"valign="top">
                                                <table style="width:100%" cellpadding="0" >
                                                    <tr>
                                                            <td class="cell_caption" style="width: 31%; height: 21px">
                                                                Debit Credit Card:&nbsp;</td>
                                                            <td align="left" colspan="2" style="width: 75%; height: 21px">
                                                                <label id = "Label1">
                                                                    <input id="Button2" class="button" onclick="call('btnCrDrCard_Click');" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                        value="GET" />
                                                                    <input id="Button3" class="button" onclick="call('btnAcct_Click');" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                        value="GET" />
                                                                        
                                                                       
                                                                <input id="btnOpen" class="button" onclick="call('btnOpenAcct_Click');" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" style="width: 42%" type="button"
                                                                        value="open account" /></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table style="width:100%" cellpadding="0" >
                                                        <tr>
                                                            <td class="cell_caption" style="width: 31%; height: 24px">
                                                                GFMC Card:&nbsp;</td>
                                                            <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                <label id = "txtCardNo2">
                                                                    <input id="btnGFMCCard" class="button" onclick="call('btnGFMCCard_Click');" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                        value="GET" />
                                                                    <input id="txtCardNo" maxlength="255" style="font-size: 15px; font-weight:normal; width: 30%;"
                                                                        type="text" />
                                                                    <input id="btnCheckPayment" class="button" onclick="call('btnCheckPayment_Click');" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" style="width: 42%" type="button"
                                                                        value="checkPayment" /></label></td>
                                                        </tr>
                                    </table>
                                               <table> 
                                                    <tr>
                                                        <td>
                                                             <input id="Button1" class="button" onclick="getResponse();" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" style="width: 82%" type="button"
                                                                                    value="GET" />
                                                            <input id="btn_door_to_door" class="button" onclick="call('btn_door_to_door_Click');" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                                    value="Door to door" />
                                                            <input id="btn_apas_batch" class="button" onclick="call('btn_apas_batch_Click');" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                                    value="APAS BAtch" />
                                                            <input id="btn_apas_online" class="button" onclick="call('btn_apas_online_Click');" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                                    value="APAS Online" />
                                                           <input id="btn_paw" class="button" onclick="call('btn_paw_Click');" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                                    value="PAW" />
                                                            &nbsp;&nbsp;
                                                            <input id="btn_Commit" class="button" onclick="call('btn_Commit_Click');" onmouseout="this.className='button';"
                                                                                    onmouseover="this.className='button_over';" style="width: 25%" type="button"
                                                                                    value="Commit" />
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

