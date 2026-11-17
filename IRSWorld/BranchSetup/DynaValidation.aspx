<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DynaValidation.aspx.cs" Inherits="IRSWorld.BranchSetup.DynaValidation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Dynamic Validation</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_dynavalidation_min.js" type="text/javascript"></script>     
</head>
<script language="javascript">
    var dyna_validation = new dyna_validation(); 
</script>
<body id="theBody" onload="javascript:dyna_validation.page_setup();">
    <center>
    <form id="form1" runat="server">
    <div id="dynadiv" style="width:100%; height:100%;padding-top:30px">
        <table id="dynatbl" style="width:82%;height:100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td align="center" valign="top" style="width:100%; border-collapse:collapse" class="content_height">
                    <table style="width:90%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:60%; vertical-align:bottom; text-align:left" align=left>
                                    <table style="width:100%"cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:0px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                        <td id="capLabel" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            Dynamic Validation</td>
                                        <td id="tdpopup2"  align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                        
                            </tr>
                            <tr>
                                <td style="width: 100%;"> <!--colspan="3"-->
                                <table style="width:100%; border:solid 3px #336699;">
                                    <tr>
                                        <td align="left" class="cell_caption" style="width: 18%; height: 21px">
                                            BranchCode:</td>
                                        <td id="ddlBranchCode" align="left" style="height: 21px"><div id="divbranchcode">
                                            <select id="ddBranchCode" style="width: 28%" onchange="dyna_validation.ddclass_change();">
                                                <option selected="selected"></option>
                                            </select>
                                            <asp:HiddenField ID="txtpagectr" runat="server" EnableViewState="False" Value="1" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 18%; height: 21px;" align="left" class="cell_caption">
                                            Class:&nbsp; </td>
                                        <td align="left" style="height: 21px"><div id="divclass"><select id="ddClass" style="width: 28%" onchange="dyna_validation.dropdown_menu2();">
                                            <option selected="selected"></option>
                                        </select>
                                        </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="cell_caption" style="width: 18%; height: 21px">
                                            Menu:</td>
                                        <td align="left" style="height: 21px"><div id="divmenu">
                                            <select id="ddMenu" style="width: 28%" onchange="dyna_validation.load_datagrid();">
                                                <option selected="selected"></option>
                                            </select>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tddynatab" colspan="2" style="width: 100%; height: 21px; background: azure">
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