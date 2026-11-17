<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchLookup.aspx.cs" Inherits="IRSWorld.BranchSetup.BranchLookup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Branch Lookup</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>      
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>  
    <script src="../js/BranchSetup/js_BranchLookup.js"></script>
</head>
<body onload="javascript:this.page_setup();" topmargin ="6" leftmargin="0">
    <center>
    <form id="form1" runat="server">
    <div id="divsublookup" style="overflow: auto; padding-top: 30px"  class=content_height>
        <table style="width:100%; height:100%" cellpadding="10" cellspacing="0">
            <tr>    
                <td align="center" valign="top" style="width:100%; border-collapse:collapse" class="content_height">
                    <table style="width:90%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td colspan="3" style ="text-align:left; width:100%;height:auto;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr >
                                        <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                            <img src="../images/toe.GIF" /></td>
                                        <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                            Branch Lookup</td>
                                        <td id="tdpopup2" align="left" valign="bottom" style="height: 24px" class="bg_body">
                                            <img align="absbottom" src="../images/rend.gif" />
                                        </td>
                                    </tr>  
                                </table>
                                </td>
                            </tr>                             
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                <table style="width:100%; border:solid 3px #336699;">
                                <tr>
                                        <td style="width: 18%; height: 21px;" align="left" class="cell_caption">
                                            &nbsp; Branch Code:</td>
                                        <td align="left" style="height: 21px"><div id="divbranchcode">
                                            <select id="ddBranchCode"  onchange="javascript: set_pagectr(1);show_groups()" style="width: auto">
                                                <option selected="selected"></option>
                                               
                                            </select>
                                            <asp:HiddenField ID="txtbranchcode" runat="server" EnableViewState="False" />
                                            <asp:HiddenField ID="txtisadmin" runat="server" EnableViewState="False" /><asp:HiddenField ID="txtpagectr" runat="server" EnableViewState="False" Value="1" />
                                             </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 18%; height: 21px;" align="left" class="cell_caption">
                                            &nbsp; Field Group:</td>
                                        <td align="left" style="height: 21px"><div id="divfldgroup">
                                            &nbsp;<input type="text" ID="txtsearch" style="width: 30%" onkeyup="javascript: filter_dropdown(this.value,'ddFldGroup')" disabled="disabled"/> <!--Width="192px" -->
                                            <select id="ddFldGroup" style="width:auto ;" onchange="javascript: set_pagectr(1);show_groups();" language="javascript" >
                                                <option selected="selected"></option>
                                               
                                            </select></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="cell_caption" style="width: 18%; height: 21px">
                                            Search By:</td>
                                        <td align="left" style="height: 21px">
                                            &nbsp;<input type="text" ID="txtsearchby" style="width: 30%" onkeyup="javascript: load_search_datagrid()" disabled="disabled"/>&nbsp;<select id="ddlColumns" style="width:auto ;" onchange="javascript: set_pagectr(1);show_groups();" language="javascript" > <!--Width="192px" -->
                                                    <option selected="selected" value="FLDCODE">CODE</option>
                                                    <option value="FLDDESCRIPTION">DESCRIPTION</option>
                                                </select></td>
                                    </tr>
                                    <tr>
                                        <td id="tdBRSLookup" colspan="2" style="width: 100%; height: 21px">
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
    </form>
    </center>
</body>
</html>
