<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryBuilder.aspx.cs" Inherits="IRSWorld.Query.QueryBuilder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Query Builder</title>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/Query/jsQueryBuilder.js" type="text/javascript"></script>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var query_builder = new query_builder();
    </script>
</head>
<body onload="javascript:query_builder.initializePage();">
    <form id="form1" runat="server">
    <div>
    <table width="100%">
        <tr>
            <td align="center" style="vertical-align:top;width:100%;height:auto;">
                <div>
                    <table cellpadding="0" cellspacing="0" width="800px">
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:24px;"><img src="../images/toe.GIF" /></td>
                            <td class="table_head_dark" style="width:50%;height:24px;">
                                Query Builder</td>
                            <td align="left" style="width:100%;height:24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table width="100%" style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                    <tr>
                                        <td colspan="2" class="table_caption" style="color: black; text-indent: 5px;height:26px;" align="left">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="30%" class="cell_caption" style="text-align:center;">
                                            Profile Name:</td>
                                        <td width="70%" align="left">
                                            <input id="txtAccountNo" type="text" style="width: 98%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="table_caption">Overseas Branch</td>
                                                    <td class="table_caption">Domestic Branch</td>
                                                    <td class="table_caption">Agent Branch</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <select id="lstOverseasBranch" size="10" multiple="multiple" style="width:100%;"></select>
                                                    </td>
                                                    <td>
                                                        <select id="lstDomesticBranch" size="10" multiple="multiple" style="width:100%;"></select>
                                                    </td>
                                                    <td>
                                                        <select id="lstAgentBranch" size="10" multiple="multiple" style="width:100%;"></select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="table_caption">
                                                        <input id="chkOverseas" type="checkbox" style="width:25px;" />&nbsp;Select all
                                                    </td>
                                                    <td class="table_caption">
                                                        <input id="chkDomestic" type="checkbox" style="width:25px;" />&nbsp;Select all
                                                    </td>
                                                    <td class="table_caption">
                                                        <input id="chkAgent" type="checkbox" style="width:25px;" />&nbsp;Select all
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="30%" class="cell_caption" style="text-align:center;">
                                            Remit Method:</td>
                                        <td width="70%" align="left">
                                            <select id="ddlRemitMethod"></select>
                                            <input id="chkRemitMethod" type="checkbox" style="width:25px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="30%" class="cell_caption" style="text-align:center;">
                                            Product Code:</td>
                                        <td width="70%" align="left">
                                            <select id="ddlProductCode"></select>
                                            <input id="chkProductCode" type="checkbox" style="width:25px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="divPopProcess" style="width:400px;height:200px;background:White;z-index:101;position:absolute;left:974px;color:darkblue;padding:10px;display:none;font-family:Arial;top:189px;">
                    <table style="width:100%; height:100%;">
                        <tr>
                            <td>
                                <label id="lblLoadingMsg" style="text-align:center;color:Chocolate;"></label>
                                    <div id="divLoadingMsg">
                                    </div>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="divHidden">
                    <input id="lblRemitMethod" type="hidden" />
                </div>
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
