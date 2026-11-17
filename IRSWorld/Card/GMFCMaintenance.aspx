<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GMFCMaintenance.aspx.cs" Inherits="IRSWorld.Card.GMFCMaintenance" %>

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
    <script type="text/javascript">
       var gfmcmaintenance = new GFMC_Maintenance();

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
                        javascript:gfmcmaintenance.btnGet_Click();
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
        <div style="width:100%">
            <table width="100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse; height: 380px;" class="content_height" colspan = "2">
                    
                        <div id="divRemGFMC" style="width: 100%; height: 100%; display: block">
                            <table width="100%">
                                <tr>                                                                
                                    <td style="width: 78%; padding-top: 30px">
                                        <table width="96%"  cellspacing="0" cellpadding="0" style="background-color:Azure">
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
                                                                    <table width="100%" cellpadding="0" cellspacing="5px">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%;"valign="center">
                                                                              
                                                                             <div style ="width:98%; height:100%; margin:0; " class="table_body" id="divGFMCRemBeneficiaryInfo">
                                                                                 <table id="tblGFMCRemBeneficiary" width="100%" cellpadding="0" cellspacing="8" style="border:solid 1px #336699;">
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height:23px">Beneficiary :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblGFMCRemBeneName" class="cell_desc" style="font-size: medium; ">&nbsp; Janet Romasanta</label></td>
                                                                                     </tr>
                                                                                     <tr>
                                                                                         <td class="cell_caption" style="width: 15%; height: 23px">Address :&nbsp; </td>
                                                                                         <td colspan=3 align="left" style="width:85%"><label id= "lblGFMCRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan, Philippines, 3022</label></td>
                                                                                     </tr>
                                                                                 </table>
                                                                              </div>                                                                               
                                                                              <div style ="width:98%; height:30%; margin:0;" class="table_body" id="divGFMCRemBeneficiaryBank">
                                                                                 <div style =" width:100%; height:30%; margin:0;" class="table_body" id="divGFMCRemBeneficiaryBankInfo">
                                                                                 <table id="tblGFMCRemBeneficiaryBank" width="100%" cellpadding="0" cellspacing="8" style="border:solid 1px #336699;">
                                                                                     
                                                                                         <tr>
                                                                                             <td class="cell_caption" style="width: 15%; height:24px">CA/SA No :&nbsp; </td>
                                                                                             <td align="left" style="width:35%; height: 24px;"><label id= "lblGFMCRemCaSaNo" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp;  123456</label></td>
                                                                                             <td class="cell_caption" style="width: 15%; height:24px">
                                                                                                 A/C Currency :&nbsp;</td>
                                                                                             <td align="left" style="width:35%; height: 24px;"><label id= "lblCurrency" class="cell_desc" style="font-size: 15px; font-weight:normal;">&nbsp; 
                                                                                                 <select id="ddlCurrency" style="width: 95%">
                                                                                                     <option selected="selected"></option>
                                                                                                 </select></label></td>
                                                                                         </tr>
                                                                                     <%--</div>--%>
                                                                                 </table>
                                                                                 </div>
                                                                                 <div style ="width:100%; height:30%; margin:0; <%--display:none;--%>" class="table_body" id="divGFMCRemBeneficiaryAvail">
                                                                                    <table id="tblGFMCRemBeneficiaryAvail" width="100%" cellpadding="0" cellspacing="8" style="border:solid 1px #336699;">
                                                                                     
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
