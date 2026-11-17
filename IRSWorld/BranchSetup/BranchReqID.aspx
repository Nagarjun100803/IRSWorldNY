<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchReqID.aspx.cs" Inherits="IRSWorld.BranchSetup.BranchReqID" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Branch Valid ID</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href  = "../css/Default_min.css" rel="stylesheet" type = "text/css" />    
    <script src = "../js/commons_min.js"                    type = "text/javascript"></script>
    <script src = "../js/popcalendar_min.js"                type = "text/javascript"></script>
    <script src = "../js/jscommonmethod_min.js"             type = "text/javascript"></script>
    <script src = "../js/jshttprequest_min.js"              type = "text/javascript"></script>
    <script src = "../js/jsmodalwindow_min.js"              type = "text/javascript"></script>
    <script src = "../js/jsgridselector_min.js"             type = "text/javascript"></script>
    <script src = "../js/BranchSetup/js_branchreqid.js" type = "text/javascript"></script>
    <script src = "../js/js_generic_min.js" type = "text/javascript"></script>
</head>
<body style="text-align:center" onload="javascript:req_br.load_branch_list();">    
    <form id = "Form1" runat="server">
    <br />    
    <table id="tbl_folder_content" cellpadding="0" cellspacing="0" style="width: auto;background-color: azure">
        <tr>
            <td style="width: 100%; text-align: left">
                <table cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align="left" style="vertical-align: bottom; width: 10px; border-collapse: collapse;
                            height: auto">
                            <img align="left" src="../images/toe.GIF" /></td>
                        <td align="left" class="table_head_dark" style="width: 46%; border-collapse: collapse;
                            height: auto">
                            BRANCH VALID ID</td>
                        <td align="left" style="vertical-align: bottom; width: 59%; border-collapse: collapse;
                            height: auto">
                            <img align="absBottom" src="../images/rend.gif" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <table id="tbl_content_container" class="table_border" style="width: 100%; height: 100%">
                    <tr>
                        <td style="height: 100%; width: 650px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 650px;">
                            <table id="Table1" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                <tr>
                                    <td class="table_caption" colspan="3" style="text-align: left">
                                        &nbsp;SELECT BRANCH</td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; text-align: right">
                                        Branch :&nbsp;
                                    </td>
                                    <td class="cell_desc" colspan="2" style="width: 529px; text-align: left">
                                        <label id="Label1">
                                            <select id="cbo_branch" onchange="javascript:req_br.branch_code = this.value; req_br.load_branch_req_id();">
                                                <option selected="selected"></option>
                                            </select>
                                        </label></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td colspan="3" style="text-align: right"></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 650px">
                            <table id="Table2" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                <tr>
                                    <td class="table_caption" colspan="3" style="text-align: left">
                                        &nbsp;LIST OF BRANCH REQUIRED ID - SELECT TO EDIT OR DELETE RECORD</td>
                                </tr>
                                <tr style="height: 19px">
                                    <td colspan="3" style="text-align: right">
                                        <div id="div_branch_req_id">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 650px">
                            <table id="Table3" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                <tr>
                                    <td class="table_caption" colspan="3" style="text-align: left">
                                        &nbsp;DATA ENTRY</td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">
                                        ID Type :&nbsp;
                                    </td>
                                    <td class="cell_desc" colspan="2" style="width: 529px; height: 19px; text-align: left">
                                        <label id="lbl_remitter_name">
                                            <select id="cbo_id_type">
                                                <option selected="selected"></option>
                                            </select>
                                        </label></td>
                                </tr>
                                <tr style="height: 19px">
                                    <td class="cell_caption" style="width: 20%; height: 19px; text-align: right">
                                        Required :&nbsp;
                                    </td>
                                    <td class="cell_desc" colspan="2" style="width: 529px; height: 19px; text-align: left">
                                        <input id="chk_required" type="checkbox" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; text-align: center; width: 650px;">
                            <input id="btn_add" type="button" value="ADD" style="width: 10%" onclick="javascript:req_br.insert_record();" />
                            <input id="btn_edit" type="button" value="EDIT" style="width: 10%" onclick="javascript:req_br.update_record();"/>
                            <input id="btn_delete" type="button" value="DELETE" style="width: 10%" onclick="javascript:req_br.delete_record();"/>
                            <input id="btn_save" type="button" value="SAVE" style="width: 10%" onclick="javascript:req_br.commit_record();"/>
                            <input id="btn_cancel" type="button" value="CANCEL" style="width: 10%" onclick="javascript:req_br.cancel_record();"/></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
