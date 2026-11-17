<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GFMCMaintenance.aspx.cs" Inherits="IRSWorld.Card.GMFCMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>GFMC Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js"  type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"           type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving.js"  type="text/javascript"></script>
    <script src = "../js/Remittance/jsremcommons.js" type="text/javascript"></script>
    <script src="../js/Remittance/jsRemTranGFMC.js" type="text/javascript"></script>
    <script src="../js/CardRem/jsGFMCMaintenance.js" type="text/javascript"></script>
    <script src="../js/CardRem/jsCardRemTeller.js" type="text/javascript"></script>      
    <script type="text/javascript">
       var gfmcmaintenance = new GFMC_Maintenance();
       var card = new card_rem_teller();

       function call(MethodID, Parameter)
       { 
            try
            {                
                var Param; 
                
                if (Parameter != null && Parameter != '')
                {
                       Param = Parameter.split(':');
                }          
                                    
                switch (MethodID)
                {                
                    case "getDetails":
                        javascript:gfmcmaintenance.getDetails();                        
                        break;                    
                    case "btnGet_Click":
                        javascript:card.open_override_form('308', 'card');  
                        //javascript:gfmcmaintenance.btnGet_Click();
                        break;
                    case "btnTest_Click":
                        javascript:gfmcmaintenance.btnTest_Click();
                        break;            
                    case "btnSave_Click":
                        javascript:gfmcmaintenance.btnSave_Click();                       
                        break;
                    case "btnCancel_Click":
                        javascript:gfmcmaintenance.btnCancel_Click();
                        break;
                    case "rdoGFMCRemAvailment_Click":
                        javascript:gfmcmaintenance.rdoGFMCRemAvailment_Click();
                        break;
                    case "rdoGFMCRemPurchase_Click":
                        javascript:gfmcmaintenance.rdoGFMCRemPurchase_Click();
                        break;
                }  
            }
            catch(e)
            {
                alert('Error : ' + e);               
            }  
       }      
   
    </script>
</head>
<body onload="call('getDetails');">
    <form id="form2" runat="server">
           <!--div id="content_mod_div">
           </div-->
           <div id="GFMC">  
                                        <table cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:23px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 23px; border-collapse:collapse;">GFMC Maintenance</td>
                                                <td align="left" valign="bottom" style="height: 23px"  class="bg_body"> 
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
                                                                              
                                                                             <div style ="width:98%; height:100%; margin:0; " class="table_body" id="divGFMCRemBeneficiaryInfo">
                                                                                 <table id="tblGFMCRemBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height:23px">Beneficiary :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblGFMCRemBeneName" class="cell_desc" style="font-size: medium; ">
                                                                                             <span style="font-size: 13pt">&nbsp; Janet Romasanta</span></label></td>
                                                                                     </tr>
                                                                                     <tr style="font-size: 13pt">
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblGFMCRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                     </tr>
                                                                                 </table>
                                                                              </div>                                                                               
                                                                              <div style ="width:98%; height:30%; margin:0;" class="table_body" id="divGFMCRemBeneficiaryBank">
                                                                                 <div style =" width:100%; height:30%; margin:0;" class="table_body" id="divGFMCRemBeneficiaryBankInfo">
                                                                                 <table id="tblGFMCRemBeneficiaryBank" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 15%; height:24px">CA/SA No :&nbsp; </td>
                                                                                             <td align="left" style="width:35%; height: 24px;"><label id= "lblGFMCRemCaSaNo" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  123456</label></td>
                                                                                             <td class="cell_caption" style="width: 15%; height:24px">
                                                                                                 A/C Currency :&nbsp;</td>
                                                                                             <td align="left" style="width:35%; height: 24px;"><label id= "lblCurrency" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 
                                                                                                 <select id="ddlCurrency" style="width: 95%" >
                                                                                                     <option selected="selected"></option>
                                                                                                 </select></label></td>
                                                                                         </tr>
                                                                                     <%--</div>--%>
                                                                                 </table>
                                                                                 </div>
                                                                                 <div style ="width:100%; height:30%; margin:0; <%--display:none;--%>" class="table_body" id="divGFMCRemBeneficiaryAvail">
                                                                                    <table id="tblGFMCRemBeneficiaryAvail" cellpadding="0" cellspacing="8" style="width:100%; border:solid 1px #336699;">
                                                                                     
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 27%; height:24px">A/C Currency :&nbsp; </td>                                                                                             
                                                                                             <td align="left" style="width:40%; height: 24px;"><input id="txtGFMCRemACCurrency" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                             
                                                                                             <td align="left" style="width:17%; height: 24px;"> <input name="rdoGFMCRemAvailment" id="rdoGFMCRemAvailment" style=" border:none; background-color:transparent; width:auto;" type="radio" onclick="call('rdoGFMCRemAvailment_Click');" title="option" checked="CHECKED"/><label id= "lblGFMCBeneAvailment" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Avail</label></td>
                                                                                             <td align="left" style="width:16%; height: 24px;"> <input name="rdoGFMCRemPurchase" id="rdoGFMCRemPurchase" style=" border:none; background-color:transparent; width:auto;" type="radio" onclick="call('rdoGFMCRemPurchase_Click');" title="option"/><label id= "lblGFMCBenePurchase" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  Purchase</label></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 27%; height:24px">Card Reference No :&nbsp; </td>
                                                                                            <td align="left" style="width:40%; height: 24px;"><input id="txtGFMCRemCardRefNo" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            <td style="width:17%; height: 24px;"><input style=" width:70%;" id="btnGetCK" type="button" value="GET" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" onclick = "call('btnGet_Click');"/></td>                                                                                            
                                                                                            <td style="width:16%; height: 24px;"><input style=" width:70%;" id="btnCancel" type="button" value="CANCEL" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" onclick="call('btnCancel_Click');"/></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 27%; height:24px">Global Filipino Card No :&nbsp; </td>
                                                                                            <td align="left" style="width:40%; height: 24px;"><input id="txtGFMCRemGlobalCardNo" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            <td style="width:17%; height: 24px;"><input style=" width:70%;" id="btnTest" type="button" value="TEST" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" onclick="call('btnTest_Click');"/></td>
                                                                                            <td style="width:16%; height: 24px;"><input style=" width:70%;" id="btnSave" type="button" value="SAVE" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" onclick="call('btnSave_Click');"/></td>
                                                                                         </tr>
                                                                                         <tr>
                                                                                            <td class="cell_caption" style="width: 27%; height:24px">ID Presented :&nbsp; </td>
                                                                                            <td align="left" style="width:40%; height: 24px;"><input id="txtGFMCRemIDPresented" type="text" style="width:98%; font-weight:normal";/></td>
                                                                                            
                                                                                            <td style="width:17%; height: 24px;"></td>
                                                                                            <td style="width:16%; height: 24px;"></td> 
                                                                                            
                                                                                            
                                                                                         </tr>
                                                                                     </table>
                                                                                  </div>
                                                                             </div>     
                                                                            </td>
                                                                        </tr>
                                                                    </table>

                                                                <!-- content ends here -->
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                                         
            </div>
            <div id='MAIN'>
                 <table  cellspacing='0' cellpadding='0' style='background-color:Azure; width: 63%;'>
                     <tr>
                        <td colspan='3' style='width: 97%; height: 100%'>
                                    <table style='width:100%; height: 100%;' class='table_border'>
                                        <tr>
                                            <td style='width: 100%; height: 100%; vertical-align: top'>
                                                <!-- content starts here -->
                                                    <table style="width:100%" cellpadding='0' cellspacing='1'>
                                                        <tr>
                                                            <td style='width: 100%; height: 100%;'valign='center'>
                                                                  <div id = 'CIFINFO' style= 'display:block'> 
                                                                        <table id='tblIndividualHeader' style="width:100%" cellpadding='0'>
                                                                            <tr>
                                                                                <td class='cell_desc' style="width: 19%; height: 24px">
                                                                                    Beneficiary Name :</td>
                                                                                <td align='left' style="width: 38%; height: 24px;" colspan='4'>
                                                                                    <label id='lblCustLName' class='cell_desc'></label>&nbsp;
                                                                                    <label id='lblCustFName' class='cell_desc'></label>&nbsp;
                                                                                    <label id='lblCustMName' class='cell_desc'></label>
                                                                                </td>
                                                                                <td class='cell_desc' style='width: 7%; height: 24px' >CIF ID :</td>
                                                                                <td align='left' style='width: 26%; height: 24px;' colspan='4'>
                                                                                    <label id='lblCIFID' class='cell_desc'>
                                                                                        </label></td>
                                                                            </tr>                                                                                                                                                                                   
                                                                         </table>
                                                                    </div>
                                                                     <div id = 'ACCOUNTGRID' style= 'display:block'>
                                                                        <table id='Table10' cellpadding='0' cellspacing='5' style='width:100%; border: solid 1px #336699; background:azure;'>
                                                                            <tr>
                                                                                <td style='width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;' colspan='5'></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style='height: 2px'><div id = 'Div2' style= 'display:none'>  PLACE GRIDVIEW ID'S</div></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                   <div id = 'RMAADDBTN'>
                                                                      <table cellpadding='0' cellspacing='5' style='width:100%; border: solid 1px #336699; background:azure;'>
                                                                         <tr>
                                                                             <td style='width:20%; height: 20px' align='right'>
                                                                                <input style=' width:150px;' id='btnRMAADD' type='button' value='ADD NEW' />&nbsp;
                                                                             </td>  
                                                                         </tr>
                                                                     </table> 
                                                                   </div>      
                                                                   <div id = 'ACCOUNTINFO'>
                                                                    <table id='Table2' cellpadding='0' cellspacing='3' style='width:100%; border:solid 2px #336699;'>
                                                                         <tr>
                                                                             <td class='cell_caption' style='width: 11%; height:24px'>
                                                                                 Account Number :&nbsp;</td>
                                                                             <td align='left' style='width:35%; height: 24px;'><label id= 'lblMode' class='cell_desc' style='font-size: 15px; font-weight:normal;'>
                                                                                 <input id='txtAcctnumber' type='text' style="font-weight:normal" maxlength="50" size="";/></label></td>  
                                                                         </tr>             
                                                                         <tr>
                                                                             <td class='cell_caption' style='width: 11%; height:24px'>
                                                                                 Currency :&nbsp;</td>
                                                                             <td align='left' style='width:35%; height: 24px;'><label id= 'Label2' class='cell_desc' style='font-size: 15px; font-weight:normal;'>
                                                                                 <select id='ddlCurr' style='width: 98%'>
                                                                                     <option selected='selected'></option>
                                                                                 </select></label></td>  
                                                                         </tr> 
                                                                         <tr>
                                                                            <td class='cell_caption' style='width: 11%; height:24px'>
                                                                                Account Type :&nbsp; </td>
                                                                            <td align='left' style='width:40%; height: 24px;'><select id='ddlAcctType' style='width: 98%'>
                                                                                <option selected='selected'></option>
                                                                            </select>
                                                                            </td>
                                                                         </tr>
                                                                         <tr>
                                                                             <td class='cell_caption' style='width: 11%; height:24px'>
                                                                                 Bank Name :&nbsp; </td>
                                                                             <td align='left' style='width:40%; height: 24px;'><select id='ddlBankName' style='width: 98%'>
                                                                                 <option selected='selected'></option>
                                                                             </select>
                                                                             </td>
                                                                         </tr>                                                                         
                                                                     </table>
                                                                    <table id='tblButtons' cellpadding='0' cellspacing='5' style='width:100%; border: solid 1px #336699; background:azure;'>
                                                                     <tr>
                                                                         <td style='width:20%; height: 20px' align='right'>
                                                                            <input style=' width:150px;' id='btnOK' type='button' value='OK' />&nbsp;
                                                                             <input style=' width:150px;' id='btnCANCEL' type='button' value='CLOSE' />
                                                                             &nbsp;</td>  
                                                                     </tr>                        
                                                                    </table> 
                                                                   </div>
                                                           <div id = 'Div4' style= 'display:block'>
                                                        <table id='Table3' cellpadding='0' cellspacing='5' style='width:100%; border: solid 1px #336699; background:azure;'>
                                                            <tr>
                                                                <td style='width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;' colspan='5'>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style='height: 2px'>
                                                                    <div id = 'Div3' style= 'display:none'>
                                                                        PLACE GRIDVIEW ID'S</div>
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
    
                <table  cellspacing='0' cellpadding='0' style="background-color:Azure; width: 50%;">
                    <tr>
                        <td colspan='3' style="width: 77%; height: 100%">
                            <table style='width:100%; height: 100%;' class='table_border'>
                                <tr>
                                    <td style='width: 100%; height: 100%; vertical-align: top'>
                                        <!-- content starts here -->
                                        <table style="width:100%" cellpadding='0' cellspacing='1'>
                                            <tr>
                                                <td style='width: 100%; height: 100%;'valign='center'>
                                                    <div id = 'Div6'>
                                                        <table style="width:100%" cellpadding='0' cellspacing='3' >
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Account Type :&nbsp;</td>
                                                                <td align='left' style="width:35%; height: 24px;">
                                                                    <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>
                                                                        &nbsp;<select id='ddlRMOAcctType' style="width: 98%">
                                                                            <option selected='selected'></option>
                                                                        </select></label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Currency :&nbsp;</td>
                                                                <td align='left' style="width:35%; height: 24px;">
                                                                    <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>
                                                                        &nbsp;<select id='ddlRMOCurrency' style="width: 98%">
                                                                            <option selected='selected'></option>
                                                                        </select>
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Branch Name :&nbsp;
                                                                </td>
                                                                <td align='left' style="width:35%; height: 24px;">
                                                                    &nbsp;<input id='txtRMOBranchName' type='text' style="font-weight:normal; width: 97%;" maxlength="50";/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Branch Code :&nbsp;
                                                                </td>
                                                                <td align='left' style="width:35%; height: 24px;">
                                                                    &nbsp;<input id='txtRMOBranchCode' type='text' style="font-weight:normal; width: 97%;" maxlength="50";/></td>
                                                            </tr>
                                                        </table>                                                        
                                                    </div>
                                                     <div id = 'Div1' style= 'display:block'>
                                                        <table cellpadding='0' cellspacing='5' style='width:100%; background:azure;'>
                                                            <tr>
                                                                <td style='width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;' colspan='5'></td>
                                                            </tr>
                                                            <tr>
                                                                <td style='height: 2px'><div id = 'Div7' style= 'display:none'>  PLACE GRIDVIEW ID'S</div></td>
                                                            </tr>
                                                        </table>
                                                    </div>       
                                                    <div id = 'Div8'>
                                                      <table cellpadding='0' cellspacing='5' style='width:100%; background:azure;'>
                                                         <tr>
                                                             <td style='width:20%; height: 20px' align='right'>
                                                                <input style=' width:150px;' id='btnRMOOK' type='button' value='OK' />&nbsp;
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
                <br />
                <br />
                <table  cellspacing='0' cellpadding='0' style="background-color:Azure; width: 50%;">
                    <tr>
                        <td colspan='3' style="width: 77%; height: 100%">
                       <%-- rem_modal_check_payments --%>
                            <table style='width:100%; height: 100%;'>
                                <tr>
                                    <td style='width: 100%; height: 100%; vertical-align: top'>
                                        <!-- content starts here -->
                                        <table style="width:100%" cellpadding='0' cellspacing='1'>
                                            <tr>
                                                <td style='width: 100%; height: 100%;'valign='center'>
                                                    <div id = 'Div5'>
                                                        <table style="width:100%" cellpadding='0' cellspacing='3' >
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Clearing Date :&nbsp;</td>
                                                                <td align='left' style='width:35%; height: 24px;'>
                                                                    <label class='cell_desc' style='font-size: 15px; font-weight:normal;' id='txtCPAcctNumber'>
                                                                        &nbsp;<input id='txtCPClearingDate' type='text' style='font-weight:normal; width: 90%;' maxlength='50' disabled='disabled';/>
                                                                        <img src='../images/calendar/imgcalen.png' align='absmiddle' onclick='popUpCalendar(this, document.getElementById(/"txtCPClearingDate"/), /"mm/dd/yyyy"/)' /></label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Account Number :&nbsp;</td>
                                                                <td align='left' style='width:35%; height: 24px;'>
                                                                    <label class='cell_desc' style='font-size: 15px; font-weight:normal;'>
                                                                        &nbsp;<input id='txtCPAcctNo' type='text' style='font-weight:normal; width: 97%;' maxlength='50';/>
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Bank Name :&nbsp;
                                                                </td>
                                                                <td align='left' style='width:35%; height: 24px;'>
                                                                    &nbsp;<input id='txtCPBankName' type='text' style='font-weight:normal; width: 97%;' maxlength='50' disabled='disabled';/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class='cell_caption' style='width: 11%; height:24px'>
                                                                    Search Bank :&nbsp;
                                                                </td>
                                                                <td align='left' style='width:35%; height: 24px;'>
                                                                    &nbsp;<input id='txtCPSearchBank' type='text' style='font-weight:normal; width: 97%;' maxlength='50';/></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id = 'Div10' style= 'display:block'>
                                                        <table cellpadding='0' cellspacing='5' style='width:100%; background:azure;'>
                                                            <tr>
                                                                <td style='width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;' colspan='5'>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style='height: 2px'>
                                                                    <div id = 'CPCHECKPAYMENTGRID' style= 'display:none'>
                                                                        PLACE GRIDVIEW ID'S</div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <div id = 'Div11'>
                                                        <table cellpadding='0' cellspacing='5' style='width:100%; background:azure;'>
                                                            <tr>
                                                                <td style='width:20%; height: 20px' align='right'>
                                                                    <input style=' width:150px;' id='Button1' type='button' value='OK' />&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <input id="lbl3dSecure" style="width: 27%" type="hidden" />
                                        <input id="lbl3dSecureURL" style="width: 27%" type="hidden" />
                                        <input id="Hidden3" type="hidden" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
            </div>      
    </form>
</body>
</html>
