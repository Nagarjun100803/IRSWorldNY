<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Beneficiary.aspx.cs" Inherits="IRSWorld.CIF.Beneficiary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRS CUSTOMER INFO.</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/CIF/js_cif_beneficiaries.js" type="text/javascript"></script>
    <script src="../js/CIF/js_cif_master.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var cif_master = new cif_master();
    
    </script>
    
    <script type="text/javascript">
    
        function test_closer()
        {
            var si = parent.document.getElementById('txtCifId').innerText;
            if (si == null || si == '') si = '0';
            var i = parseFloat(si);
            
            i++;
            tst_name = 'Lastname Firstname mi';
            var ra = new Array(2);
            ra[0] = i;
            ra[1] = tst_name;
            
            parent.returnValue = ra;
            
            close_frame_dialog();
        }                               
    </script>
</head>
<body>
    <center>
        <form id="form1" runat="server">
        <div style="width:100%">
<%--            ** cif main.** &nbsp; &nbsp;
            <input type="button" id="btntest" value="close" onclick="test_closer();" style="width: 8%" />
--%>     


            <table style="width:80%" cellpadding="0" cellspacing="5">
                    <tr>
                        <td align="center" valign="middle" style="width:80%; border-collapse:collapse" class="content_height" colspan = "2">
                            <div id = "CIFSearchMain" style =" display:block" >
                                <table style="width:50%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td class="table_head_dark"  style="width:50%; height: 26px;">
                                            Remitter Information</td>
                                        <td align="left" valign="bottom" style="width: 44%; height: 26px;"> <img align="absbottom" src="../images/cap.gif" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table style="width:100%; border:solid 3px #336699;">
                                                <tr>                                                    
                                                </tr>
                                                <tr>
                                                    <td style="width: 70%; height:200px;">
                                                        <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                                        <div id="div1" style="width: 90%;display: block; vertical-align:middle">
                                                          <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                              <tr>
                                                                    <td style="width: 78%; height: 130px;"valign="top">
                                                                        <table id="Table1" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699;">
                                                                            <tr><td class="table_caption" style="width: 133px; text-align: left; text-indent: 5px; color: Black" colspan="3">Input CIF ID</td></tr>                                                                            
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 133px; height: 23px">CIF Id:&nbsp;</td>
                                                                                <td align="left" style="width: 230px" class="cell_desc"><input id="txtSCIFID" type="text" style="width: 133px; height: 100%; text-align: right;" /></td>
                                                                                
                                                                            </tr>  
                                                                             <tr>
                                                                              <td class="cell_caption" style="width: 133px; height: 23px">First Name:&nbsp;</td>
                                                                                <td align="left" style="width: 230px" class="cell_desc"><input id="txtFirstName" type="text" style="width: 133px; height: 100%; text-align: right;" /></td>
                                                                             </tr>    
                                                                             <tr>
                                                                              <td class="cell_caption" style="width: 133px; height: 23px">Last Name:&nbsp;</td>
                                                                                <td align="left" style="width: 230px" class="cell_desc"><input id="txtLastName" type="text" style="width: 133px; height: 100%; text-align: right;" /></td>
                                                                             </tr>                                                                             
                                                                        </table>
                                                                    </td>
                                                             </tr>
                                                            </table>
                                                        </div>

                                                        <table id="tblButtonsSearchCIF" style="width:100%" cellpadding="0" cellspacing="5px">
                                                            <tr>
                                                                    <td align="center" colspan="3" style="width: 100%; height: 50px;">
                                                                        <div id="_divButtonsSearchCIF" style="width: 60%; background: transparent">
                                                                            <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                                <tr>
                                                                                    <td style="width: 25%"><input id="btnSearchCIF" class="edit_button" onmouseover="this.className='edit_button_over';" onmouseout="this.className='edit_button';" type="button" value="Search" style="width: 31%" /></td>                                                                                        
                                                                                </tr>
                                                                            </table>
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

                            <div id = "CIFMain" style =" display:none" >
                                <table style="width:100%"  cellpadding="0" cellspacing="5">
                                    <tr>
                                        <td style =" vertical-align:top">
                                                <div id = "CIFMenu" style =" display:none" >
                                                      <table style="width:100%"  cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td id="td1" style="width: 100%; text-align: right; height: 19px;">
                                                                    <a id="Beneficiary" href="#"  onclick="MBeneficiary()" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Beneficiary</a>
                                                                </td>
                                                            </tr>                                    
                                                            <tr>
                                                                <td id="td2" style="width: 100%; text-align: right">
                                                                    <a id="Merchant" href="#" onclick="MMerchant()" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Merchant</a>
                                                                </td>
                                                            </tr>           
                                                            <tr>
                                                                <td id="td3" style="width: 100%; text-align: right; height: 9px;">
                                                                    
                                                                </td>
                                                            </tr>                                                                                                                                       
                                                            <tr>
                                                                <td id="td4" style="width: 100%; text-align: right">
                                                                    
                                                                </td>
                                                            </tr>                                                                                                                                       
                                                            <tr>
                                                                <td id="td5" style="width: 100%; text-align: right; height: 19px;">
                                                                    
                                                                </td>
                                                            </tr>                                                                                                                                       
                                                            <tr>
                                                                <td id="td6" style="width: 100%; text-align: right">
                                                                    
                                                                </td>
                                                            </tr>                                                                                                                                       
                                                                                                                                                     
                                                        </table>
                                                </div>
                                         </td>
                                         <td> 
                                                <div id = "CIFEntry" style =" display:none" >
                                                        <table style="width:100%" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td class="table_head_dark"  style="width:30%; height: 26px;">Client Information</td>
                                                                <td align="left" valign="bottom" style="width: 472px; height: 26px;"> 
                                                                    <img align="absbottom" src="../images/cap.gif" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table style="width:100%; border:solid 3px #336699;">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 21px;">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 100%; height: auto;">
                                                                                <div id = "CIFMainGrid" style =" display:none" >GRIDVIEW CONTROL.
                                                                                <table id="Table2" cellpadding="0" cellspacing="5" style="width:600px; border: solid 1px #336699;">
                                                                                        <tr>
                                                                                            <td class="table_caption" style="width: 125px; text-align: left; text-indent: 5px; color: Black" colspan="5">Beneficiary
                                                                                            </td>
                                                                                        </tr>
                                                                                        </table> 
                                                                                        </div>                                                                               
                                                                                <div id = "CIFIndividual" style =" display:none" >  GRIDVIEW CONTROL.
                                                                                <table id="Table3" cellpadding="0" cellspacing="5" style="width:600px; border: solid 1px #336699;">
                                                                                        <tr>
                                                                                            <td class="table_caption" style="width: 125px; text-align: left; text-indent: 5px; color: Black" colspan="5">Merchant
                                                                                            </td>
                                                                                        </tr>
                                                                                        </table> 
                                                                                </div>    
                                                                                
                                                                                <div id = "CIFBusiness" style =" display:none" >  &nbsp;</div> 
                                                                                
                                                                               <div id = "CIFAddressMain" style =" display:none" >   
                                                                                
                                                                                  


                                                                                                                                                                                                        
                                                                               <div id = "CIFMainButton" style =" display:none" >  
                                                                                   <table id="tblButtonsMain" cellpadding="0" cellspacing="5" style="width:80%">
                                                                                       <tr>
                                                                                           <td align="center" colspan="3" style="width: 100%">
                                                                                               <table cellpadding="0" cellspacing="5" style="width:80%">
                                                                                                   <tr>
                                                                                                       <td style="width: 20%">
                                                                                                           <input id="btnAdd" class="add_button" disabled="disabled" onclick="maint_add_click();"
                                                                                                               onmouseout="this.className='add_button';" onmouseover="this.className='add_button_over';"
                                                                                                               type="button" value="Add" /></td>
                                                                                                       <td style="width: 20%">
                                                                                                           <input id="btnEdit" class="edit_button" disabled="disabled" onmouseout="this.className='edit_button';"
                                                                                                               onmouseover="this.className='edit_button_over';" type="button" value="Edit" /></td>
                                                                                                       <td style="width: 20%">
                                                                                                           <input id="btnDelete" class="delete_button" disabled="disabled" onmouseout="this.className='delete_button';"
                                                                                                               onmouseover="this.className='delete_button_over';" type="button" value="Delete" /></td>
                                                                                                       <td style="width: 20%">
                                                                                                           <input id="btnSave" class="save_button" onmouseout="this.className='save_button';"
                                                                                                               onmouseover="this.className='save_button_over';" type="button" value="Save" /></td>
                                                                                                       <td style="width: 20%">
                                                                                                           <input id="btnCancel" class="cancel_button" onclick="maint_cancel_click();" onmouseout="this.className='cancel_button';"
                                                                                                               onmouseover="this.className='cancel_button_over';" type="button" value="Cancel" /></td>
                                                                                                   </tr>
                                                                                               </table>
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


                            
                                                </div>
                                                 
                                                
                                         </td>
                                    </tr> 
                                </table>
                                 

                            
                            </div>
                    </td>
                </tr> 
            </table> 
 
           


        </div>
        </form>
    </center>
</body>
</html>
