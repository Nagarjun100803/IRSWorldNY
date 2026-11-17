<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Extract_PCHC.aspx.cs" Inherits="IRSWorld.GRS.Extract.Extract_PCHC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GRS Extract PCHC</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    
    <script src="../../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../../js/jsgridselector.js"                   type="text/javascript"></script>
    <script src="../../js/popcalendar.js"                      type="text/javascript"></script>
    
    <script src="../../js/GRS/js_PCHCExtract.js"               type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var PCHCx = new PCHCExrt();
    </script>
</head>
<body onload = "Page_Setup();">
    <form id="form1" runat="server">
    <center>
        <div id="divpopProcess" style="z-index: 101; position: absolute; left: 166px; color: darkblue;
                padding: 10px; display: none; font-family: Arial; top: 70px;
                width: 400px; height:200px;">
            <table style="width:100%; height:100%;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center; " class="label2"></label>
                        <div id="divLoadingMsg">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    <div>
        <table width="100%" cellpadding="0" cellspacing="5">
            <tr>
                <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 600px;" class="content_height" colspan="2">
                    <div id="dvExtract" style="width: 100%; height: 75%; display: block; overflow-x: hidden; overflow-y: scroll;">
                        <table width="100%">
                            <tr>
                                <td style="width: 78%; padding-top: 30px">
                                    <table width="45%" cellspacing="0" cellpadding="0" style="background-color: Azure">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:1%; height:24px;">
                                                <img src="../../images/toe.GIF" alt="" /></td>
                                            <td nowrap id="capResult" align="left" class="table_head_dark" style="width:20%">PCHC Outward</td>
                                            <td align="left" valign="bottom" style="" class="bg_body">
                                                <img align="absbottom" src="../../images/rend.gif" alt="" />&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 100%">
                                                <table class="table_border" style="width: 100%; height: 100%;">
                                                    <tr>
                                                        <td style="width: 100%; height: 21px"></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; width: 100%; height: 100%">
                                                            <!-- Content Starts Here -->
                                                            <table cellpadding="0" cellspacing="5" width="100%">
                                                                <tr>
                                                                    <td style="width: 100%; height: 100%" valign="center">
                                                                        <div id="dvPCHCExtract" class="table_body" style="margin: 0px; width: 98%;height: 100%">
                                                                            <table id="tblextract" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                            width = "100%">
                                                                                <tr>
                                                                                    <td class="table_caption" colspan="2">File Extract</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width:50%" align="right">
                                                                                        Extract:&nbsp;&nbsp;
                                                                                    </td>
                                                                                    <td style="width:50%" align="left">
                                                                                        <select id="ddlExtract" onchange="PCHCx.getFileNo();"></select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width:50%" align="right">
                                                                                        Date of Transfer:&nbsp;&nbsp;
                                                                                    </td>
                                                                                    <td style="width:50%" align="left">
                                                                                        <input id="txtDateOfTransfer" readonly="readonly" style="width:80%" onchange="PCHCx.getFileNo();" />
                                                                                        <img id="imgCalendar" src="../../images/calendar/imgcalen.png" alt="Change date" onclick="popUpCalendar(this, document.forms[0].txtDateOfTransfer, 'mm/dd/yyyy',document.getElementById('dvPCHCExtract'));"/>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width:50%" align="right">
                                                                                        File No:&nbsp;&nbsp;
                                                                                    </td>
                                                                                    <td style="width:50%" align="left">
                                                                                        <select id="ddFileNo" onchange="PCHCx.getSFileNo();"></select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width:50%" align="right">
                                                                                        Sub File No:&nbsp;&nbsp;
                                                                                    </td>
                                                                                    <td style="width:50%" align="left">
                                                                                        <select id="ddSFileNo"></select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2" style="width:50%" align="center">
                                                                                        <input id="btnExtract" type="button" value="Extract" style="width:100px;" onclick="PCHCx.Extract();" />
                                                                                    </td>                                                                                    
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%; height: 100%" valign="center">
                                                                        <div id="dvPCHCExtractSummary" class="table_body" style="margin: 0px; width: 98%;height: 100%; display:none;">
                                                                            <table id="tblextractsummary" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;"
                                                                            width = "100%">
                                                                                <tr>
                                                                                    <td class="table_caption" colspan="4">Summary Of Amounts</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" align="right" style="width:25%">Date of Transfer:&nbsp;</td>
                                                                                    <td style="width:25%" align="left"><label id="lblDateOfTransfer"></label></td>
                                                                                    <td class="cell_caption" align="right" style="width:25%">File No:&nbsp;</td>
                                                                                    <td style="width:25%" align="left"><label id="lblFileNo"></label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="right" style="width:25%">&nbsp;</td>
                                                                                    <td style="width:25%" align="left">&nbsp;</td>
                                                                                    <td class="cell_caption" align="right" style="width:25%">Sub File No:&nbsp;</td>
                                                                                    <td style="width:25%" align="left"><label id="lblSFileNo"></label></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4">
                                                                                        <div id="dvgrvSummaryOfAmount" style="width:100%">
                                                                                            
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="4">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="3" align="right">&nbsp;</td>
                                                                                    <td align="left">
                                                                                        <input id="btnClear" type="button" value="Clear" onclick="clearMe();" />
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
</body>
</html>
