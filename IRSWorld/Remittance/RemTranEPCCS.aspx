<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranEPCCS.aspx.cs" Inherits="IRSWorld.Remittance.RemTranEPCCS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remittance - EPCS</title>
    <link href = "../css/Default.css" rel="stylesheet" type="text/css" />
    <style type = "text/css"></style>
    <script src = "../js/jsCommons.js"              type="text/javascript"></script>
    <script src = "../js/jscommonmethod.js"         type="text/javascript"></script>
    <script src = "../js/js_sidebar.js"             type="text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"          type="text/javascript"></script>
    <script src = "../js/Remittance/jsremsaving.js" type="text/javascript"></script>
    <script src = "../js/Remittance/jsremcommons.js"type="text/javascript"></script>
    <script src="../js/Remittance/jsRemTranEPCS.js" type="text/javascript"></script>    
    <script type="text/javascript">
            var remtran_epcs = new RemTranEPCS();
    </script>
</head>
<body onload="javascript:remtran_epcs.construct_side_bar('divtdRemEPCSMenu');">
    <form id="form1" runat="server">
        <br />
                                            <table cellpadding = "0"; cellspacing = "1" style = "width:700px;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
                                                <tr>
                                                    <td class = "cell_desc" colspan="2" style="height: 23px"><label id="lbl_3d_note"></label></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 10%; height: 20px">FIND:</td>
                                                    <td style="width: 20%; height: 23px; text-align: left"><input id = "Text2" onkeyup="javascript:execute_ajaxgridview_command('ajx_search$')" type = "text" /></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 23px"><div id="div_3d_non_osa"></div></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 10%; height: 20px">BRANCH CODE:</td>
                                                    <td style="width: 40%; height: 20px; text-align: left" ><label id="lbl_3d_branch_code"></label></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 10%; height: 20px">BRANCH NAME:</td>
                                                    <td style="width: 40%; height: 20px; text-align: left"><label id="lbl_3d_branch_name"></label></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 10%; height: 20px">TARGET CURRENCY:</td>
                                                    <td style="width: 40%; height: 20px; text-align: left"><select id="ddltargetccy"></select></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 23px; text-align:center"><input id="btn_3D_branch_ok" style="width: 100px; height: 25px" type="button" value="OK" />&nbsp;<input id="btn_3d_branch_cancel" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>
                                                        
                                                </tr>
                                            </table>
        <br />
        <br />
        <br />
        <br />
        <br />
        
        <table border-bottom: border-left: border-top: solid="">
            <tr>
                <td class="cell_caption" height: style="width: 15%; height: 20px">ADDRESS:</td>                    
                <td height: left="" style="width: 50px; height: 20px" text-align:><label id="lbl_3d_address"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" height: style="width: 15%; height: 20px">ZIP CODE:</td>
                <td height: left="" style="width: 50px; height: 20px" text-align:><label id="lbl_3d_zipcode"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" height: style="width: 15%; height: 20px">TARGET CURRENCY:</td>
                <td height: left="" style="width: 50px; height: 20px" text-align:><select id="Select1"></select></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center"><input id="btn_3D_ZIP_OK" type="button" value="OK" style="width: 100px; height: 25px" />&nbsp;<input id="btn_3D_ZIP_Cancel" type="button" value="Cancel" style="width: 100px; height: 25px" /></td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        
        
        <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
            <tr>
                <td class="cell_caption" style="width: 12%; height: 23px">TARGET CURRENCY:</td>                    
                <td style="width: 20%; height: 23px; text-align: left"><select id="Select2"></select></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 23px; text-align: center"><input id="Button1" style="width: 100px; height: 25px" type="button" value="OK" /><input id="Button2" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>
            </tr>
        </table>
        <br /><table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
            <tr>
                <td class="cell_caption" style="width: 12%; height: 23px">
                    TARGET CURRENCY:</td>
                <td style="width: 20%; height: 23px; text-align: left">
                    <select id="Select3">
                    </select>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 23px; text-align: center">
                    <input id="Button4" style="width: 100px; height: 25px" type="button" value="OK" /><input id="Button5" style="width: 100px; height: 25px" type="button" value="Cancel" /></td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <br />
        <table border-bottom: border-left: border-top: solid="">
            <tr>
                <td class = "cell_desc" colspan="2" style="text-align: center"><label>SELECT REMITTERS REPRESENTATIVE OR CREATE NEW REPRESENTATIVE.</label></td>                
            </tr>
            <tr>
                <td colspan="2" style="text-align: center" ><div id = "div_proxy_remitters"></div> </td>                
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 20px">LAST NAME :</td>
                <td style="width: 60px; height: 20px"><label id="Label1"><input id="txt_proxy_lastname" type="text"  style = "width:auto"/></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 20px">FIRST NAME :</td>
                <td class="cell_desc" style="width: 60px; height: 20px"><label id="Label2"><input id="txt_proxy_firstname" type="text" style = "width:auto" /></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 20px">MIDDLE NAME :</td>
                <td class="cell_desc" style="width: 60px; height: 20px"><input id="txt_proxy_middlename" type="text" style = "width:auto" /></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 20px">PRESENT ADDRESS :</td>
                <td class="cell_desc" style="width: 60px; height: 20px"><input id="txt_proxy_present_address" type="text" style = "width:auto"  /></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: center"><input id="Button3" type="button" value="OK" style="width: 100px; height: 25px" /></td>                    
            </tr>
        </table>
        <br />
        <br />
        <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
            <tr>
                <td class = "cell_desc" colspan="2" style="height: 23px; text-align: center">EXCEPTION LIST</td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">REMITTER ID :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><label id = "lbl_rem_exception_remitter_id"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">LAST NAME :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><label id = "lbl_rem_exception_last_name"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">FIRST NAME :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><label id = "lbl_rem_exception_first_name"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">MIDDLE NAME :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><label id = "lbl_rem_exception_middle_name"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">REASON FOR EXCEPTION :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><label id = "lbl_rem_exception_reason_for_exception"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">REASON FOR CANCELLATION :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><label id = "lbl_rem_exception_reason_for_cancellation"></label></td>
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">USER NAME :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><input id="txt_rem_exception_username" type="text" /></td>                    
            </tr>
            <tr>
                <td class="cell_caption" style="width: 20%; height: 23px">PASSWORD :</td>
                <td class="cell_desc" style="width: 20%; height: 23px; text-align: left"><input id="txt_rem_exception_password" type="text" /></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 23px; text-align: center">
                    <input id="btn_rem_exception_continue" style="width: 100px; height: 25px" type="button" value="CONTINUE" />&nbsp;<input id="btn_rem_exception_revoke" style="width: 100px; height: 25px" type="button" value="REVOKE" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid">
            <tr><td colspan="2" style="height: 23px; text-align: center"><img src="../images/ajaxloader.gif" /></td></tr>                            
            <tr><td class = "cell_desc" colspan="2" style="height: 23px; text-align: center"><label id = "lbl_credit_casa_message">SYSTEM IS PROCESSING YOUR REQUEST...</label></td></tr>
            <tr><td colspan="2" style="height: 23px; text-align: center"></td></tr>
        </table>
        <br />
    </form>
</body>
</html>
