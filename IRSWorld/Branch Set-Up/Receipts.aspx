<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Receipts.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Receipts" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Receipts </title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
   <%-- <script src="../js/CIF/js_cif_beneficiaries.js" type="text/javascript"></script>
    <script src="../js/CIF/js_cif_master.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
    
        var cif_master = new cif_master();
    
    </script>
    
    <script type="text/javascript">
    
           
    </script>
</head>
<body>
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="Receipts" style="display: block">
            <table cellpadding="0" cellspacing="0" width="50%">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                    <img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px">
                                    Receipt Setup</td>
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
                                <td style="width: 100%; height: 100%">
    
                                    <table id="tblReceipts" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                        width="500">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 26px;">
                                                Receipts</td>
                                        </tr>
                                       <tr>
                                       
                                       </tr>
                                    <tr>
                                       <td style="height: 150px; width: 500px;">
                                           <div style="text-align: center">
                                               <table>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Header1:&nbsp;</td>
                                                       
                                                           <td align="left" style="width: 305px; height: 24px;" colspan="4">
                                                                <input id="txtHeader1" type="text" style="width: 300px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Header2:&nbsp;</td>
                                                      <td align="left" style="width: 305px; height: 24px;" colspan="4"><input id="txtHeader2" type="text" style="width: 300px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Name of Branch:&nbsp;
                                                       </td>
                                                       <td align="left" style="width: 305px; height: 24px;" colspan="4"><input id="txtNameOfBranch" type="text" style="width: 300px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Address:&nbsp;
                                                       </td>
                                                      <td align="left" style="width: 305px; height: 24px;" colspan="4"><input id="txtAddress" type="text" style="width: 300px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Contact Numbers:&nbsp;
                                                       </td>
                                                       <td align="left" style="width: 305px; height: 24px;" colspan="4"><input id="txtContactNos" type="text" style="width: 300px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Email:&nbsp;
                                                       </td>
                                                       <td align="left" style="width: 305px; height: 24px;" colspan="4"><input id="txtEmail" type="text" style="width: 300px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                   

                                                        <table cellpadding="0" cellspacing="5" width="250px" id="TABLE1">
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
