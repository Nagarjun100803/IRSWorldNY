<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportSetup.aspx.cs" Inherits="IRSWorld.BranchSetup.ReportSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Branch Lookup</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script> 
    <script src="../js/datagrid.js" type="text/javascript"></script> 
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>     
    <script src="../js/BranchSetup/js_ReportSetup.js"></script>
</head>
<body onload="javascript:this.load_ddl();" topmargin ="0" leftmargin="0">
    <center>
    <form id="form1" runat="server">
    <div id="divsublookup" style="width:82%; height:100%" class=content_height>
        <table style="width:400; height:300" cellpadding="30" cellspacing="5">
            <tr>    
                <td align="center" valign="top" style="border-collapse:collapse" class="content_height">
                <div style="overflow:auto ;width:400;height:300">
                    <table style="width:90%" cellspacing="0" cellpadding="0">
                            <tr>
                                <!-- by chr -->
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table style="width:100%" cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img src="../images/toe.GIF" /></td>
                                        <td id="Td1" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            Report Setup</td>
                                        <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                               
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                <table style="width:100%; border:solid 3px #336699;">
                                <tr>
                                        <td style="width: 18%; height: 21px;" align="left" class="cell_caption" nowrap >
                                            &nbsp; Branch Code</td>
                                        <td align="left" style="height: 21px" nowrap>
                                        <div id="divbranchcode" >
                                            <select id="ddBranchCode"   style="width: auto ; height: 21px" onchange="javascript:change_branch(this.value);" >
                                                <option selected="selected"></option>
                                            </select>
                                            <asp:HiddenField ID="txtbranchcode" runat="server" EnableViewState="False" />
                                            <asp:HiddenField ID="txtisadmin" runat="server" EnableViewState="False" />
                                             </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 18%; height: 21px;" align="left" class="cell_caption" nowrap >
                                            &nbsp; Report Group</td>
                                        <td align="left" style="height: 21px" nowrap><div id="divfldgroup">
                                            <select id="ddlRepMain" name="ddlRepMain" onchange="javascript:show_groups();" style="width: auto ; 
                                                height: 21px">
                                                <option selected="selected"></option>
                                            </select>
                                        </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdReportLookup" colspan="2" style="width: 100%; height: 21px">
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
    </div> 
    </form>
    </center>
</body>
</html>
