<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tiered_Charges.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Tiered_Charges" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Tiered Charges </title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
   <%-- <script src="../js/CIF/js_cif_beneficiaries.js" type="text/javascript"></script>
    <script src="../js/CIF/js_cif_master.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
    
        var cif_master = new cif_master();
    
    </script>

</head>
<body>
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="Tiered_Charges" style="display: block">
            <table cellpadding="0" cellspacing="0" style="width:50%">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                    <img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px">
                                    Tiered Charges</td>
                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                    <img align="absBottom" src="../images/rend.gif" /></td>  
                            </tr> 
                        </table>    
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 100%; border-bottom: #336699 3px solid">
                            <tr>
                                <td style="width: 100%; height: 21px">
                                    
                                   <table id="tblTiered_Charges" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                        width:530">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 26px;">Charge Details</td>
                                        </tr>
                                       <tr>
                                       
                                       </tr>
                                        <tr>
                                       <td style="height: 217px; width: 588px;">
                                           <div style="text-align: center">
                                               <table>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                           Charge Name:&nbsp;</td>
                                                       
                                                           <td style="width: 100px" colspan="4">
                                                               <select id="ddChargeName" style="width: 342px; height: 100%">
                                                               </select>
                                                           </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                           Currency:&nbsp;</td>
                                                       <td style="width: 100px" colspan="4">
                                                           <select id="ddCurrency" style="width: 342px; height: 100%">
                                                           </select>
                                                       </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                           Computation Type:&nbsp;
                                                       </td>
                                                       <td style="width: 100px" colspan="4">
                                                           <select id="ddComputationType" style="width: 342px; height: 100%">
                                                           </select>
                                                       </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                           Back-End Charge?:&nbsp;
                                                       </td>
                                                       <td style="width: 100px">
                                                           <select id="ddBackEndCharge" style="width: 100px; height: 100%">
                                                           </select>
                                                       </td>
                                                       <td class="cell_caption" style="width: 133px; height: 23px">
                                                           Charge to Benef?:&nbsp;
                                                       </td>
                                                       <td style="width: 100px">
                                                           <select id="ddChargeToBenef" style="width: 100px; height: 100%">
                                                           </select>
                                                       </td>
                                                   </tr>
                                               </table>
                                           </div>
                                           <br />
                                           <div style="text-align: center">
                                               <table>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                           Sort Order:&nbsp;
                                                       </td>
                                                       <td style="width: 100px">
                                                           <input id="txtSortOrder" style="width: 342px; height: 100%; text-align: left" type="text" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                       Greater Than or Eqaul To:&nbsp;</td>
                                                       <td style="width: 100px">
                                                           <input id="txtGreaterThanOrEqualTo" style="width: 342px; height: 100%; text-align: left"
                                                               type="text" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                           Less Than:&nbsp;</td>
                                                       <td style="width: 100px">
                                                           <input id="txtLessThan" style="width: 342px; height: 100%; text-align: left"
                                                               type="text" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 150px; height: 23px">
                                                           &nbsp;Tie Charge Amount/Rate:&nbsp;</td>
                                                       <td style="width: 100px">
                                                           <input id="txtTieChargeAmountRate" style="width: 342px; height: 100%; text-align: left"
                                                               type="text" /></td>
                                                   </tr>
                                                   <tr>

                                                   </tr>
                                                   
                                               </table>
                                           </div>
                                           
                                                <table cellpadding="0" cellspacing="5" style="width:250px" id="TABLE1">
                                                    <tr>
                                                        <td style="width: 25%">
                                                            <input id="btnSave" class="enable_button" onmouseout="this.className='enable_button';"
                                                                onmouseover="this.className='mouseover_button';" type="button" value="Save" /></td>
                                                        <td style="width: 25%" >
                                                            <input id="btnEdit" class="enable_button" disabled="disabled" onmouseout="this.className='enable_button';"
                                                                onmouseover="this.className='mouseover_button';" type="button" value="Edit" /></td>
                                                        <td style="width: 25%" >
                                                            <input id="btnClear" class="enable_button" disabled="disabled" onmouseout="this.className='enable_button';"
                                                                onmouseover="this.className='mouseover_button';" type="button" value="Clear" /></td>
                                                        <td style="width: 25%" >
                                                            <input id="btnCancel" class="enable_button" onclick="maint_cancel_click();" onmouseout="this.className='enable_button';"
                                                                onmouseover="this.className='mouseover_button';" type="button" value="Cancel" /></td>
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
        </div>
    </center>
    </div>
    </form>
</body>
</html>

