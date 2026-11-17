<%@ Page Language="C#" AutoEventWireup="true" Codebehind="BranchOperation.aspx.cs"
    Inherits="IRSWorld.BranchSetup.BranchOperation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Branch Operation</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_BranchOperation.js" type="text/javascript"></script>
    <script type="text/javascript">
            var Branch_Operation = new Branch_Operation();
    </script>

</head>
<body onload="javascript:Branch_Operation.load();">
    <center>
        <form id="form1" runat="server">
            <div id="divBodyRegion" style="width: 98%; display: none; vertical-align: top; margin-top: 30px;">
                <table id="tblRegion" style="width: 45%; display: block">
                    <tr>
                        <td id="td3" style="width: 100%; border-collapse: collapse; vertical-align: middle;">
                            <table style="width:100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="vertical-align: bottom">
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" style="width: 1%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="tdRegionPageTitle" class="table_head_dark" style="width: 50%; height: 24px">
                                                    BRANCH</td>
                                                <td align="left" style="width: 100%; height: 24px; vertical-align: bottom;">
                                                    <img align="absBottom" src="../images/rend.gif" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                        align="center">
                                        <br />
                                        <table cellpadding="0" cellspacing="5" style="width:95%">
                                            <tr>
                                                <td class="cell_caption" style="width: 35%; height: 23px">
                                                    BY BRANCH :&nbsp;</td>
                                                <td align="left" style="width: 65%; height: 23px;">
                                                    <select <%--onclick="javascript:branch_operation.branch();"--%> id="ddlBranch" style="width: 100%">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </td>
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0" style="width:100%">
                                            <tr>
                                                <td style="width: 100%; height: 51px;" align="center">
                                                    <table id="tbl_button" cellpadding="0" cellspacing="5">
                                                        <tr>
                                                            <td style="width: 20%; vertical-align: middle;" align="center">
                                                                <input id="btn_Search" class="button" onclick="javascript:Branch_Operation.check_region();"
                                                                    type="button" value="Search" /></td>
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
            <div id="divBodyBranchOperation" style="width: 98%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBranchOperation" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBranchOperation" style="width: 85%; border-collapse: collapse;
                            vertical-align: middle;">
                            <div id="divBranchOperationDefault" style="width: 80%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: bottom">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 0%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="tdPageTitle" class="table_head_dark" style="width: 50%; height: 24px">
                                                        BRANCH OPERATION</td>
                                                    <td align="left" style="width: 100%; height: 24px; vertical-align: bottom;">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                            align="center">
                                            <br />
                                            <div id="divBranchOperation" style="width: 95%; display: block">
                                                <table id="tblBranchOperationDefault" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Branch :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtBranch" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Email :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtEmail" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Description :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <textarea id="txtDescription" rows="2" style="width: 100%"></textarea>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <div id="DIV_BRANCHES" onmousemove="javascript:Branch_Operation.active_grid = 'DIV_BRANCHES';">
                                                                <br />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnNew" class="button" onclick="javascript:Branch_Operation.insert_record_sub_branch();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="New" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnEdit" class="button" onclick="javascript:Branch_Operation.update_record();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Edit" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSave" class="button" onclick="javascript:Branch_Operation.commit_record();"
                                                                            type="button" value="Save" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCancel" class="button" onclick="javascript:Branch_Operation.load();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="div1" style="display: none">
                                            </div>
                                            <div id="div2" style="display: none">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divBodySubBranch" style="width: 98%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodySubBranch" style="width: 100%; display: block">
                    <tr>
                        <td id="tdSubBranch" style="width: 85%; height: 94%; border-collapse: collapse; vertical-align: middle;">
                            <div id="divSubBranchDefault" style="width: 80%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: bottom">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 0%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td2" class="table_head_dark" style="width: 50%; height: 24px">
                                                        SUB BRANCH</td>
                                                    <td align="left" style="width: 100%; height: 24px; vertical-align: bottom;">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                            align="center">
                                            <br />
                                            <div id="divSubBranch" style="width: 95%; display: block">
                                                <table id="tblSubBranch" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Sub-Branch :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtSubBranch" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Address :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtAddress" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Tel No : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtTelNo" style="width: 100%" />
                                                        </td>
                                                </table>
                                                <input id="txtHiddenSeqId2" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons2" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSubNew" class="button" onclick="javascript:Branch_Operation.insert_record_sub_branch();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="New" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSubEdit" class="button" onclick="javascript:Branch_Operation.update_record_sub_branch();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Edit" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSubSave" class="button" onclick="javascript:Branch_Operation.commit_record_sub_branch();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Save" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSubCancel" class="button" onclick="javascript:Branch_Operation.load_record();Branch_Operation.load_sub_branch_grid();Branch_Operation.open_division('divBodyBranchOperation');"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="div1" style="display: none">
                                            </div>
                                            <div id="div2" style="display: none">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp</td>
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
