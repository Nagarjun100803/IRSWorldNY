<%@ Page Language="C#" AutoEventWireup="true" Codebehind="TranxFileMaintenance.aspx.cs"
    Inherits="IRSWorld.GRSMaintenance.TranxFileMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction File Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/FileLoader/js_TranxFileMaintenance.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script type="text/javascript">
            var filemaintenance_dropdown = new filemaintenance_dropdown();
           
        </script>
</head>
<body onload="javascript:filemaintenance_dropdown.page_load();">
    <form id="form1" runat="server">
        <div style="width: auto; height: auto">
            <br />
            <table style="width: auto; height: auto">
                <tr>
                    <td id="tdSBTxnFileMaintenance" style="width: 200px; vertical-align:text-top">
                        <div id="divSBTxnFileMaintenance">
                        </div>
                    </td>
                    <td style="width: 50px"></td>
                    <td align="center" style="width: 500px; vertical-align: top">
                        <table style="width: 100%; height: auto;" cellpadding="0" cellspacing="0">
                            <tr>
                                <td nowrap="nowrap" align="left" valign="bottom" style="width: 1%; height: 24px;">
                                    <img alt="" src="../images/toe.GIF" /></td>
                                <td nowrap="nowrap" align="left" class="table_head_dark" width="20%">
                                    Transaction File Setup&nbsp;</td>
                                <td nowrap="nowrap" align="left" valign="bottom" class="bg_body" style="height: 24px">
                                    <img alt="" align="absbottom" src="../images/rend.gif"/>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width:510px; border: solid 3px #336699">
                                    <div style="width:100%; height: auto;">
                                        <table style="width: 100%; height: auto;" cellpadding="2" cellspacing="2">
                                            <tr>
                                                <td style="width: 100%">
                                                    <table style="width: 100%; height: auto" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td nowrap="nowrap">
                                                                <div style="width: auto; height: auto; display: block; border: solid 1px #3366CC">
                                                                    <table style="width: 100%; height: auto" cellpadding="2" cellspacing="2">
                                                                        <tr>
                                                                            <td nowrap="nowrap" colspan="2" style="width: 100%">
                                                                                <table style="width: 100%">
                                                                                    <tr id="trBranch">
                                                                                        <td nowrap="nowrap" style="width: 18%; height: 20px; text-align: center" class="cell_caption">
                                                                                            &nbsp;Branch</td>
                                                                                        <td colspan="4" nowrap="nowrap" style="width: 82%; height: 20px">
                                                                                            <select id="ddBranch" style="width: 99%; height: auto" onchange="javascript:filemaintenance_dropdown.branch_select();">
                                                                                            </select>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td nowrap="nowrap" style="width: 18%; height: 20px; text-align: center" class="cell_caption">
                                                                                            &nbsp;File Type</td>
                                                                                        <td nowrap="nowrap" style="width: 32%; height: 20px">
                                                                                            <select id="ddFileType" style="width: 99%; height: auto" onchange="javascript:filemaintenance_dropdown.fileformat_load();">
                                                                                            </select>
                                                                                        </td>
                                                                                        <td nowrap="nowrap" style="width: 18%; height: 20px; text-align: center" class="cell_caption">
                                                                                            &nbsp;File Format</td>
                                                                                        <td nowrap="nowrap" style="width: 32%; height: 20px">
                                                                                            <select id="ddFileFormat" style="width: 99%; height: auto" onchange="javascript:populate_rec()">
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                   
                                                                                </table>
                                                                                <div id="divEmailSetup">
                                                                                    <table id="tblMailAddress" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                                                                   border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid">
                                                                                         <tr>
                                                                                             <td id="tdMailAddress" style="width: 100%; height: 21px;">
                                                                                             </td>
                                                                                         </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <div id="divMethodSetup">
                                                                                    <table id="tblFileProducts" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                                                                border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid"> 
                                                                                        <tr>
                                                                                            <td id="tdMethodSetup" style="width: 100%; height: 21px;">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                                <div id="divBatchSetup">
                                                                                    <table id="tblBatchSetup" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                                                                border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid"> 
                                                                                        <tr>
                                                                                            <td id="tdBatchSetup" style="width: 100%; height: 21px;">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
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
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
