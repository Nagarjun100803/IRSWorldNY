<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VANTerritory.aspx.cs" Inherits="IRSWorld.Earthport.VANTerritory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Earthport VAN Teritory Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/Earthport/js_VANTerritory.js" type="text/javascript"></script>

<script language="javascript" type="text/javascript">
         var Van_Territory = new Van_Territory();  
</script>
</head>
<body onload = "javascript:Van_Territory.load_grid_viewer();Van_Territory.load_all_dropdownlist();">
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="Machine_Setup" style="display: block">
            <table cellpadding="0" cellspacing="0" width="50%">
                <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" >
                        <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                        <img src="../images/toe.GIF" /></td>    
                        <td class="table_head_dark" style="width: 50%; height: 24px">
                            Earthport Territory Maintenance</td>
                        <td align="left" style="width: 100%; height: 24px" valign="bottom">
                        <img align="absBottom" src="../images/rend.gif" /></td>   
                    </table>    

                </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 900px">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 49%; border-bottom: #336699 3px solid; height: 301px;">
                            <tr>
                                <td style="width: 100%; height: 100%">
                                    <table id="tblMachineSetup" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; text-align: center; width: 794px; height: 259px;">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 26px;">
                                                TERRITORY_DETAILS</td>
                                        </tr>
                                      
                                    <tr>
                                       <td style="height: 150px; width: 900px; text-align: center; background-color: azure;">
                                     
                                                 <table id="tblVANField" cellspacing="8" cellpadding="0" width="100%">
                                          
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%">
                                                        Country :<asp:Label ID="Label11" runat="server" ForeColor="Red" Height="8px" Text="*"
                                                            Width="5px"></asp:Label>&nbsp;</td>
                                                    <td style="WIDTH: 21%" align="left" >
                                                        &nbsp;<select id="ddCountry" style="width: 100%"></select></td>
                                                        <td align="left" style="width: 15%; text-align: right;" class="cell_caption">
                                                             Currency :<asp:Label ID="Label12" runat="server" ForeColor="Red" Height="8px" Text="*"
                                                            Width="5px"></asp:Label></td>
                                                    <td align="left" style="width: 21%; text-align: center;">
                                                        &nbsp;<select id="ddCurrency" style="width: 100%">
                                                             </select></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%; height: 20px;">
                                                        Bank Name :<asp:Label ID="Label6" runat="server" ForeColor="Red" Height="8px" Text="*"
                                                            Width="5px"></asp:Label>&nbsp;</td>
                                                    <td style="WIDTH: 21%; color: red; height: 20px;" align="left"><input id="txtBankName" type="text" style="width: 100%"/></td>
                                                     <td align="left" style="width: 15%; text-align: right; height: 20px;" class="cell_caption">
                                                         Bank Code:
                                                         <asp:Label ID="Label1" runat="server" ForeColor="Red" Text="(number)"></asp:Label></td>
                                                    <td align="left" style="width: 12%; text-align: center; height: 20px;"><input id="txtBankCode" type="text" style="width: 100%" /></td>
                                               </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%; height: 20px;">
                                                        Account Holding Branch :
                                                        <asp:Label ID="Label7" runat="server" ForeColor="Red" Height="8px" Text="*" Width="5px"></asp:Label>&nbsp;</td>
                                                    <td style="WIDTH: 21%; height: 20px;" align="left"><input id="txtAccountHoldingBank" type="text" style="width: 100%" /></td>
                                                    <td align="left" style="width: 15%; text-align: right; height: 20px;" class="cell_caption">
                                                        Branch Code:
                                                        <asp:Label ID="Label2" runat="server" ForeColor="Red" Text="(number)"></asp:Label></td>
                                                    <td align="left" style="width: 12%; text-align: center; height: 20px;"><input id="txtBranchCode" type="text" style="width: 100%" /></td>
                                             </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%">
                                                        Address:&nbsp;</td>
                                                    <td style="WIDTH: 21%" align="left" ><input id="txtAddress" type="text" style="width: 100%" /></td>
                                                    <td align="left" style="width: 15%; text-align: right;" class="cell_caption">
                                                        Cle RIB:
                                                        <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="(number)"></asp:Label></td>
                                                    <td align="left" style="width: 12%; text-align: center;"><input id="txtCleRIB" type="text" style="width: 100%" /></td>
                                                </tr>
                                                      <tr>
                                                    <td class="cell_caption" style="width: 15%">
                                                        Account Name:<asp:Label ID="Label8" runat="server" ForeColor="Red" Height="8px" Text="*"
                                                            Width="3px"></asp:Label>&nbsp;</td>
                                                    <td style="WIDTH: 21%" align="left" ><input id="txtAccountName" type="text" style="width: 100%" /></td>
                                                    <td align="left" style="width: 15%; text-align: right;" class="cell_caption">
                                                        Clearing Code:
                                                        <asp:Label ID="Label4" runat="server" ForeColor="Red" Text="(number)"></asp:Label></td>
                                                    <td align="left" style="width: 12%; text-align: center;"><input id="txtClearingCode" type="text" style="width: 100%" /></td>
                                                </tr>
                                                 <tr>
                                                    <td class="cell_caption" style="width: 15%">
                                                        Account Number:&nbsp;</td>
                                                    <td style="WIDTH: 21%" align="left" ><input id="txtAccountNumber" type="text" style="width: 100%" /></td>
                                                    <td align="left" style="width: 15%; text-align: right;" class="cell_caption">
                                                        Giro No:</td>
                                                    <td align="left" style="width: 12%; text-align: center;"><input id="txtGiroNo" type="text" style="width: 100%" /></td>
                                                      </tr>
                                                 <tr>
                                                    <td class="cell_caption" style="width: 15%">
                                                        IBAN:<asp:Label ID="Label9" runat="server" ForeColor="Red" Height="8px" Text="*"
                                                            Width="3px"></asp:Label>&nbsp;</td>
                                                    <td style="WIDTH: 21%" align="left" ><input id="txtIBAN" type="text" style="width: 100%" /></td>
                                                    <td align="left" style="width: 15%; text-align: right;" class="cell_caption">
                                                        Sort Code:</td>
                                                    <td align="left" style="width: 12%; text-align: center;"><input id="txtSortCode" type="text" style="width: 100%" /></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%">
                                                        SWIFT BIC:<asp:Label ID="Label10" runat="server" ForeColor="Red" Height="8px" Text="*"
                                                            Width="3px"></asp:Label>&nbsp;</td>
                                                    <td style="WIDTH: 21%" align="left" ><input id="txtSWIFTBIC" type="text" style="width: 100%" /></td>
                                                    <td align="left" style="width: 15%; text-align: right;" class="cell_caption">
                                                        Check Digit:
                                                        <asp:Label ID="Label5" runat="server" ForeColor="Red" Text="(number)"></asp:Label></td>
                                                    <td align="left" style="width: 12%; text-align: center;"><input id="txtCheckDigit" type="text" style="width: 100%" /></td>
                                                  </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%">
                                                        BSB:&nbsp;</td>
                                                    <td style="WIDTH: 21%" align="left" ><input id="txtBSB" type="text" style="width: 100%" /></td>
                                                    <td align="left" style="width: 15%; text-align: right;" class="cell_caption">
                                                        Bank Giro Credit No:</td>
                                                    <td align="left" style="width: 12%; text-align: center;"><input id="txtBankGiroCreditNo" type="text" style="width: 100%" /></td>
                                                </tr>
                                               
                                          
                                           </table>
                                    
                                                        <table cellpadding="0" cellspacing="5" width="300px" id="tblbuttons">
                                                            <tr>
                                                                <td style="width: 25%; height: 22px;">
                                                                    <input id="btnAdd" class="button" onclick = "javascript:Van_Territory.insert_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="  Add  " /></td>
                                                                <td style="width: 25%; height: 22px;">
                                                                    <input id="btnEdit" class="button" onclick = "javascript:Van_Territory.update_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value= "  Edit  " disabled="disabled" /></td>
                                                                <td style="width: 25%; height: 22px;" >
                                                                    <input id="btnDelete" class="button" onclick = "javascript:Van_Territory.delete_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value= "Delete" disabled="disabled" /></td>
                                                                <td style="width: 25%; height: 22px;" >
                                                                    <input id="btnSave" class="button" onclick = "javascript:Van_Territory.commit_record();"  onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value=" Save " disabled ="disabled"/></td>
                                                                <td style="width: 25%; height: 22px;" >
                                                                    <input id="btnCancel" class="button" onclick = "javascript:Van_Territory.rollback_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="Cancel" disabled ="disabled" /></td>
                                                            </tr>
                                                        </table>
                                                       
                                                       <br />
                                                       <input id="txtHiddenSeqID"  type="hidden" style="width: 2%" />
                                       </td>
                                           
                                    </tr>
                                        
                                    </table>
                                    
                                        <div style="text-align: center" id="EarthportTerritoryGrid">
                                        </div>
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
