<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Charges_Tab.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Charges_Tab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch </title>
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
        <div id="Charges" style="display: block">
            <table cellpadding="0" cellspacing="0" width="40%">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                    <img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px">
                                    Charges Tab</td>
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
                                    <table>
                                        <tr>
                                            <td style="width: 100%; height: 303px">
                                                <div style="text-align: center">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 100px; height: 193px">
                                                                <table id="tblOccupation" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                    width="600">
                                                                    <tr>
                                                                        <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                                            height: 26px; text-align: left">
                                                                            Charges</td>
                                                                    </tr>
                                                                    <tr>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 217px">
                                                                            <div style="text-align: center">
                                                                                <table>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 133px; height: 26px">
                                                                                            Charge Group:&nbsp;</td>
                                                                                        <td style="width: 100px; height: 26px">
                                                                                            <select id="ddChargeGroup" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td style="width: 100px; height: 26px">
                                                                                        </td>
                                                                                        <td style="width: 100px; height: 26px">
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                            Product Type/Code:&nbsp;</td>
                                                                                        <td style="width: 100px">
                                                                                            <select id="ddProductType" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                            Implementation Type:&nbsp;</td>
                                                                                        <td style="width: 100px">
                                                                                            <select id="ddImplementationType" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                            Default Computation:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 100px">
                                                                                            <select id="ddDefaultComputation" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                            Default Status:&nbsp;</td>
                                                                                        <td style="width: 100px">
                                                                                            <select id="ddDefaultStatus" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 133px; height: 22px">
                                                                                            Remittance Currency:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 100px; height: 22px">
                                                                                            <select id="ddRemittanceCurrency" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 133px; height: 22px">
                                                                                            Remitter Charge Type:&nbsp;</td>
                                                                                        <td style="width: 100px; height: 22px">
                                                                                            <select id="ddRemitterChargeType" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                            Pay Mode:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 100px">
                                                                                            <select id="ddPayMode" style="width: 150px; height: 100%">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                            Clearing Days:&nbsp;</td>
                                                                                        <td style="width: 100px">
                                                                                            <input id="txtClearingDays" style="width: 145px; height: 100%; text-align: right"
                                                                                                type="text" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br />
                                                                            <div style="text-align: center">
                                                                                &nbsp;<div id="CIFChargeGrid" style="display: block">
                                                                                    GRIDVIEW CONTROL.</div>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="CIFButton" style="display: block">
                                                    <div id="CIFMainButton" style="display: block">
                                                        <table id="TABLE1" cellpadding="0" cellspacing="5" width="250px">
                                                            <tr>
                                                                <td style="width: 25%; height: 34px">
                                                                    <input id="btnSave" class="enable_button" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Save" /></td>
                                                                <td style="width: 25%; height: 34px">
                                                                    <input id="btnEdit" class="enable_button" disabled="disabled" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Edit" /></td>
                                                                <td style="width: 25%; height: 34px">
                                                                    <input id="btnClear" class="enable_button" disabled="disabled" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Clear" /></td>
                                                                <td style="width: 25%; height: 34px">
                                                                    <input id="btnCancel" class="enable_button" onclick="maint_cancel_click();" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </div>
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
        </div>
    </center>
    </div>
    </form>
</body>
</html>
