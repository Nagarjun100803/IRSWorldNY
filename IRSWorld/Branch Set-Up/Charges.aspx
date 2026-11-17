<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Charges.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Charges" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Charges </title>
    <link href="../css/Default.css" rel="stylesheet"       type="text/css" />
    <script src="../js/jsCommons.js"                       type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                  type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                   type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                  type="text/javascript"></script>
    <script src="../js/BranchSetup/js_charges.js"          type="text/javascript"></script>
    <script type="text/javascript">
    
        var charges = new charges();
    
    </script>
    
</head>
<body onload="javascript:new common_method().to_read_only('tblCharges1',true);new common_method().to_read_only('tblCharges2',true);charges.load_grid_viewer();charges.load_all_dropdownlist();">
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="Charges" style="display: block">
            <table cellpadding="0" cellspacing="0" width="20%">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                    <img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width: 50%; height: 24px">
                                    Charges</td>
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
                                    <table id="tblCharges" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                        width="530">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                height: 26px; text-align: left">
                                                Charge Details</td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                            <td style="width: 588px; height: 217px">
                                                <div style="text-align: center">
                                                    <table id="tblCharges1">
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Charge Name:&nbsp;</td>
                                                            <td style="width: 100px">
                                                                <select id="ddChargeName" style="width: 342px; height: 100%">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Currency:&nbsp;</td>
                                                            <td style="width: 100px">
                                                                <select id="ddCurrency" style="width: 342px; height: 100%">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Computation Type:&nbsp;
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select id="ddComputationType" style="width: 342px; height: 100%">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Back-End Charge?:&nbsp;
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select id="ddBackEndCharge" style="width: 342px; height: 100%">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <br />
                                                <div style="text-align: center">
                                                    <table id="tblCharges2">
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Value:&nbsp;
                                                            </td>
                                                            <td style="width: 100px">
                                                                <input id="txtValue" style="width: 342px; height: 100%; text-align: right" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Minimum Amount:&nbsp;</td>
                                                            <td style="width: 100px">
                                                                <input id="txtMinimumAmount" style="width: 342px; height: 100%; text-align: right"
                                                                    type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Denominator:&nbsp;</td>
                                                            <td style="width: 100px">
                                                                <input id="txtDenominator" style="width: 342px; height: 100%; text-align: right"
                                                                    type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 23px">
                                                                Charge to Beneficiary:&nbsp;
                                                            </td>
                                                            <td style="width: 100px">
                                                                <select id="ddChargetoBene" style="width: 345px; height: 100%">
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 150px; height: 26px">
                                                                GL Account: &nbsp;</td>
                                                            <td style="width: 100px">
                                                                <input id="txtGlAccount" style="width: 342px; height: 100%; text-align: right" type="text" /></td>
                                                        </tr>

                                                    </table>
                                                    <table cellpadding="0" cellspacing="5" width="300px" id="tblbuttons">
                                                            <tr>
                                                                <td style="width: 25%">
                                                                    <input id="btnAdd" class="enable_button" onclick = "javascript:charges.insert_record();" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="  Add  " /></td>
                                                                <td style="width: 25%">
                                                                    <input id="btnEdit" class="enable_button" onclick = "javascript:charges.update_record();" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value= "  Edit  " disabled="disabled" /></td>
                                                                <td style="width: 25%" >
                                                                    <input id="btnSave" class="enable_button" onclick = "javascript:charges.commit_record();"  onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value=" Save " disabled ="disabled"/></td>
                                                                <td style="width: 25%" >
                                                                    <input id="btnCancel" class="enable_button" onclick = "javascript:charges.rollback_record();" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Cancel" disabled ="disabled" /></td>
                                                            </tr>
                                                        </table>
                                                    
                                                </div>
                                                <input id="txtHiddenSeqID" type="hidden" /></td>
                                        </tr>
                                    </table>
                                    <div style="text-align: center" id="ChargesGrid">
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

