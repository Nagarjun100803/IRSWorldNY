<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Valid_IDs.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Valid_IDs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Valid IDs </title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
   <%-- <script src="../js/CIF/js_cif_beneficiaries.js" type="text/javascript"></script>
    <script src="../js/CIF/js_cif_master.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
    
        var cif_master = new cif_master();
    
function Select2_onclick() {

}

    </script>

</head>
<body>
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="Valid_IDs" style="display: block">
            <table cellpadding="0" cellspacing="0" style="width: 30%">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                    <img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px">
                                    Branch Setup</td>
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
                                    <table id="tblValid_IDs" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                        width="400">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 26px;">
                                                Valid IDs</td>
                                        </tr>
                                       <tr>
                                       <!--eto un--><td style="height: 160px; width: 400px;">
                                           <div style="text-align: center">
                                               <table id="TABLE2">
                                                   <tr>
                                                       <td class="cell_caption" style="width: 464px; height: 23px">
                                                           1:&nbsp;</td>
                                                       
                                                           <td align="left" style="width: 260px; height: 24px;" colspan="4">
                                                               &nbsp;<select id="ddValidID1" style="width: 150px; height: 100%"></select></td>
                                                       <td align="left" colspan="1" style="width: 237px; height: 24px">
                                                                <input id="txtValidID1" type="text" style="width: 150px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 464px; height: 23px">
                                                           2:&nbsp;</td>
                                                      <td align="left" style="width: 260px; height: 24px;" colspan="4">
                                                          &nbsp;<select id="ddValidID2" style="width: 150px; height: 100%"></select></td>
                                                       <td align="left" colspan="1" style="width: 237px; height: 24px"><input id="txtValidID2" type="text" style="width: 150px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 464px; height: 23px">
                                                           3:&nbsp;</td>
                                                       <td align="left" style="width: 260px; height: 24px;" colspan="4">
                                                           &nbsp;<select id="ddValidID3" style="width: 150px; height: 100%" onclick="return Select2_onclick()"></select></td>
                                                       <td align="left" colspan="1" style="width: 237px; height: 24px"><input id="txtValidID3" type="text" style="width: 150px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 464px; height: 23px">
                                                           4:&nbsp;
                                                       </td>
                                                      <td align="left" style="width: 260px; height: 24px;" colspan="4">
                                                          &nbsp;<select id="ddValidID4" style="width: 150px; height: 100%"></select>
                                                      </td>
                                                       <td align="left" colspan="1" style="width: 237px; height: 24px"><input id="txtValidID4" type="text" style="width: 150px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                        </table>              
    
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
