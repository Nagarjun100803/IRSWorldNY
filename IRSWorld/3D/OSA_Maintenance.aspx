<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OSA_Maintenance.aspx.cs" Inherits="IRSWorld._3D.OSA_Maintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>OSA Maintenance</title>
    
    <link href="../css/Default.css"                                     type="text/css" rel="stylesheet"/>
    <script src="../js/jscommonmethod.js"                               type="text/javascript"></script>
    <script src="../js/jsCommons.js"                                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                               type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js"                                type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                                type="text/javascript"></script>
    <script src="../js/popcalendar.js"                                  type="text/javascript"></script>
    <script src="../js/3D/ORD3DMaintenance.js"                          type="text/javascript"></script>

</head>
<body onload="ord3d_maintenance.show_div('div_tbl_maintenance');">
    <form id="form1" runat="server">
    <center>
    <div>
        <table>
            <tr>
                <td id="tdCouriermaintenance" style="vertical-align: top; width: 20%; height: 52px">
                    <table style="width: 100%">
                        <tr>
                            <td style="border-bottom: #5697c1 1px solid; text-align: right">
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: #5697c1 1px solid; text-align: right">
                                <a id="OSA_maintenance_viewer" href="javascript:ord3d_maintenance.show_div('div_tbl_maintenance');"
                                    style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial">
                                    <span style="color: #336699">OSA Maintenance</span></a>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: #5697c1 1px solid; height: 22px; text-align: right">
                                <a id="ORD3DBranch" href="javascript:ord3d_maintenance.show_div('div_tbl_3Dbranch');"
                                    style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial">
                                    <span style="color: #336699">3D Branch Maintenance</span></a>
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: #5697c1 1px solid; height: 22px; text-align: right">
                                <a id="A1" href="javascript:ord3d_maintenance.show_div('div_Threshold');"
                                    style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial">
                                    <span style="color: #336699">Courier Threshold Maintenance</span></a>
                            </td>
                        </tr>
                        <%--        <tr>
                                <td style="text-align:right; border-bottom:solid 1px #5697c1;">
                                    <a id="aGRSGEFU" href="javascript:grsGEFUMaintenance.show_div('div_GRS_GEFU');"style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">GEFU Extraction</a>
                                </td>
                            </tr>--%>
                    </table>
                </td>
                <td id="td_osa_maintenance" align="center" style="vertical-align: top; width: 77%; height: 52px">
                    <table>
                        <tr>
                            <td style="width: 753px; height: 100%;">
                                <div id="div_tbl_maintenance" style="display: none">
                                    <table cellpadding="0" cellspacing="0" style="width: 750px">
                                        <tr>
                                            <td align="left" style="width: 11px; border-collapse: collapse; height: 24px" valign="bottom">
                                            </td>
                                            <td align="left" style="width: 51%; height: 24px" valign="bottom">
                                            </td>
                                        </tr>
                                        <tr>
                                            <!-- header -->
                                            <td align="left" style="width: 11px; border-collapse: collapse; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td class="table_head_dark" style="width: 51%; height: 24px">
                                                OSA Maintenance</td>
                                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                <img align="bottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <!-- main body -->
                                            <td colspan="3">
                                                <table id="tbl_osa_maintenance" style="width: 100%; border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                    background: azure; border-left: #336699 3px solid; border-bottom: #336699 3px solid">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 13%; height: 24px">
                                                            Municipality :</td>
                                                        <td align="left" style="width: 15%; height: 24px"><input id="txtMunicipality" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 97%; text-align: left" type="text" /></td>
                                                        <td class="cell_caption" style="width: 13%; height: 24px">
                                                            Province :
                                                        </td>
                                                        <td align="left" style="width: 15%; height: 24px"><input id="txtProvince" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 97%; text-align: left" type="text" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 13%; height: 28px">
                                                            Region :</td>
                                                        <td align="left" style="width: 15%; height: 28px"><input id="txtRegion" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 97%; text-align: left" type="text" /></td>
                                                        <td class="cell_caption" style="width: 13%; height: 28px">
                                                            Zip Code :</td>
                                                        <td align="left" style="width: 15%; height: 28px" id="tdtes"><input id="txtZip" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 76%; text-align: left" type="text" size="" />
                                                            <input id="btnZip" class="button" onclick="javascript:ord3d_maintenance.get_zip();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 36px" type="button" value="..." disabled="disabled" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 13%; height: 24px">
                                                            Courier :</td>
                                                        <td align="left" style="width: 15%; height: 24px">
                                                            <select id="cboOSA" disabled="disabled" style="width: 100%">
                                                            </select>
                                                        </td>
                                                        <td class="cell_caption" style="width: 13%; height: 24px">
                                                            OSA Indicator :</td>
                                                        <td align="left" style="width: 15%; height: 24px">
                                                            &nbsp;<input id="chkOSA" style="width: 29%" type="checkbox" disabled="disabled" checked="CHECKED" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="4" style="height: 24px">
                                                            <div id="div_tbl_osa" style="width: 750px; text-align: center"  onmouseover="javascript:ord3d_maintenance.grid_nm = 'div_tbl_maintenance'">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <!-- second body -->
                                        <tr>
                                            <td colspan="4">
                                                <table id="tbl_osa_buttons" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid; border-top: #336699 1px solid; background: azure; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                                    <tr>
                                                        <td style="height: 22px">
                                                            <input id="btn_osa_add" class="button" onclick="javascript:ord3d_maintenance.insert_record();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Add" />
                                                        </td> 
                                                        <td style="height: 22px">
                                                            <input id="btn_osa_edit" class="button" onclick="javascript:ord3d_maintenance.update_record();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Edit" />
                                                        </td>
                                                        <td style="height: 22px">
                                                            <input id="btn_osa_save" class="button" disabled="disabled" onclick="javascript:ord3d_maintenance.save_record();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Save" />
                                                        </td>
                                                        <td style="height: 22px">
                                                            <input id="btn_osa_cancel" class="button" disabled="disabled" onclick="javascript:ord3d_maintenance.update_cancel();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Cancel" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="height: 40px">
                                                <br />
                                                <input id="Hidden1" type="hidden" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 753px; height: 100%;">
                                <div id="div_tbl_3Dbranch" style="display: none">
                                            <table cellpadding="0" cellspacing="0" style="width: 750px">
                                                <tr>
                                                    <td align="left" style="width: 11px; border-collapse: collapse; height: 24px" valign="bottom">
                                                    </td>
                                                    <td align="left" style="width: 51%; height: 24px" valign="bottom">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!-- header -->
                                                    <td align="left" style="width: 11px; border-collapse: collapse; height: 23px" valign="bottom">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td class="table_head_dark" style="width: 51%; height: 23px">
                                                        3D Branch Maintenance</td>
                                                    <td align="left" style="width: 100%; height: 23px" valign="bottom">
                                                        <img align="bottom" src="../images/rend.gif" /></td>
                                                </tr>
                                                <tr>
                                                    <!-- main body -->
                                                    <td colspan="3" style="height: 113px">
                                                        <table id="tbl_branch_maintenance" style="width: 100%; border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                    background: azure; border-left: #336699 3px solid; border-bottom: #336699 3px solid">
                                                            <tr>
                                                                <td class="cell_caption" style="width: 13%; height: 24px">
                                                                    Branch Code :</td>
                                                                <td align="left" style="width: 15%; height: 24px">
                                                                    <input id="txtBranchCd" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 97%; text-align: left" type="text" /></td>
                                                                <td class="cell_caption" style="width: 13%; height: 24px">
                                                                    Branch Name &nbsp;:
                                                                </td>
                                                                <td align="left" style="width: 15%; height: 24px">
                                                                    <input id="txtBranchName" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 97%; text-align: left" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 13%; height: 24px">
                                                                    Courier :</td>
                                                                <td align="left" style="width: 15%; height: 24px"><input id="txtBranchCourier" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 97%; text-align: left" type="text" /></td>
                                                                <td class="cell_caption" style="width: 13%; height: 24px">
                                                                    Domestic Branch Location :</td>
                                                                <td align="left" style="width: 15%; height: 24px">
                                                                    <input id="txtBranchLoc" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                style="width: 97%; text-align: left" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" style="height: 24px">
                                                                    <div id="div_tbl_branch" style="width: 750px; text-align: center" onmouseover="javascript:ord3d_maintenance.grid_nm = 'div_tbl_3Dbranch'">
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <!-- second body -->
                                            </table>
                                        </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 753px; height: 100%;">                          
                                <div id="div_Threshold" style="display: none">
                                        <table cellpadding="0" cellspacing="0" style="width: 750px">
                                            <tr>
                                                <td align="left" style="width: 11px; border-collapse: collapse; height: 24px" valign="bottom">
                                                </td>
                                                <td align="left" style="width: 51%; height: 24px" valign="bottom">
                                                </td>
                                            </tr>
                                            <tr>
                                                <!-- header -->
                                                <td align="left" style="width: 11px; border-collapse: collapse; height: 24px" valign="bottom">
                                                    <img src="../images/toe.GIF" /></td>
                                                <td class="table_head_dark" style="width: 51%; height: 24px">
                                                    Courier Threshold Maintenance</td>
                                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                    <img align="bottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <!-- main body -->
                                                <td colspan="3">
                                                    <table id="tbl_threshold" style="width: 100%; border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                                background: azure; border-left: #336699 3px solid; border-bottom: #336699 3px solid">
                                                        <tr>
                                                            <td class="cell_caption" style="width: 13%; height: 19px" id="cb">
                                                                Courier :</td>
                                                            <td align="left" style="width: 15%; height: 19px">
                                                                <select id="cboThreshCourier" disabled="disabled" style="width: 100%">
                                                                </select>
                                                            </td>
                                                            <td class="cell_caption" style="width: 13%; height: 19px">
                                                                Currency :&nbsp;</td>
                                                            <td align="left" style="width: 15%; height: 19px">
                                                                <select id="cboThreshCurrency" disabled="disabled" style="width: 100%">
                                                                </select></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 13%; height: 24px">
                                                                Account Number :</td>
                                                            <td align="left" style="width: 15%; height: 24px">
                                                                <input id="txtAccountNum" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                            style="width: 97%; text-align: right" type="text" /></td>
                                                            <td class="cell_caption" style="width: 13%; height: 24px">
                                                                Threshold Amount :</td>
                                                            <td align="left" style="width: 15%; height: 24px">
                                                                <input id="txtThreshAmount" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"
                                                                            style="width: 97%; text-align: right" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4" style="height: 24px">
                                                                <div id="divThreshold" style="width: 750px; text-align: center"  onmouseover="javascript:ord3d_maintenance.grid_nm = 'div_Threshold'">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <!-- second body -->
                                            <tr>
                                                <td colspan="4">
                                                    <table id="tbl_thresh_buttons" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid; border-top: #336699 1px solid; background: azure; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                                        <tr>
                                                            <td style="height: 5px">
                                                                <input id="btn_thresh_add" class="button" onclick="javascript:ord3d_maintenance.insert_thresh_record();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Add" />
                                                            </td>
                                                            <td style="height: 5px; width: 187px;">
                                                                <input id="btn_thresh_edit" class="button" onclick="javascript:ord3d_maintenance.update_threshold();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Edit" />
                                                            </td>
                                                            <td style="height: 5px">
                                                                <input id="btn_thresh_save" class="button" disabled="disabled" onclick="javascript:ord3d_maintenance.save_thresh();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Save" />
                                                            </td>
                                                            <td style="height: 5px; width: 187px;">
                                                                <input id="btn_thresh_cancel" class="button" disabled="disabled" onclick="javascript:ord3d_maintenance.update_thresh_cancel();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" style="width: 100px" type="button" value="Cancel" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="height: 40px">
                                                    <br />
                                                    <input id="Hidden2" type="hidden" />
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
    </center>
    </form>
</body>
</html>
