<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GFCSearch.aspx.cs" Inherits="IRSWorld.GFC.GFCSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/GFC/jsGFCSearch.js"                 type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>     
    <script type="text/javascript">        
        var gfc = new GFC_Search();
    </script> 
</head>
<body onload = javascript:gfc.page_onload();>
    <form id="form1" runat="server">
    <div>
        <table id="tbl_main" cellpadding="0" cellspacing="5" style="margin-top: 30px; height: 100%;
            width:100%">
            <tr>
                <td align="center" style="width: 79%" valign="top">
                    <%--<div id = "CIFENTRY" style= "display:block; overflow:auto; height:500px;">--%>
                    <div id="Main" >
                        <table cellpadding="0" cellspacing="0" style="width: 72%">
                            <tr>
                                <td style="width: 470px">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width: 0%; border-collapse: collapse; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="tdHeader" class="table_head_dark" style="width: 50%; height: 24px">
                                                GFC Locked Records</td>
                                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td colspan="2" style="background: azure; height: 95px">
                                    <table id="tblCIF" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                        border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid">
                                        <tr>
                                            <td style="width: 100%;">
                                                <div id="Note">
                                                    <table id="Table2" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                                        border-top: #336699 1px solid; background: azure; border-left: #336699 1px solid;
                                                        border-bottom: #336699 1px solid; width:100%">
<%--                                                        <tr>
                                                            <td id="tdSearchHeader" class="table_caption" colspan="5" style="color: black; text-indent: 5px;
                                                                text-align: left">
                                                            </td>
                                                        </tr>
                                                        <tr id="trLasname">
                                                            <td class="cell_caption" style="width: 20%; height: 25px">
                                                                Amount Limit :&nbsp;</td>
                                                            <td align="left" colspan="3" style="height: 25px">
                                                                <input id="txtAmount" maxlength="200" style="width: 98%; text-align: left" type="text" />
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td id="capMandatory" align="left" class="comment" colspan="4" style="width:70%">
                                                                &nbsp;<label id="lblMandatory">Note* Click on the grid the record you want to unlock.</label></td>
                                                        </tr>
<%--                                                        <tr>
                                                            <td align="right" colspan="4" style="height: 22px" width="20%">
                                                                <input id="btnCancel" class="button" onclick="javascript:aggr.btnCancel_click();"
                                                                    onmouseout="this.className = 'button';" onmouseover="this.className='button_over';"
                                                                    style="width: 18%" type="button" value="Cancel" />&nbsp;
                                                                <input id="btnUpdate" class="button" onclick="javascript:aggr.btnUpdate_click();"
                                                                    onmouseout="this.className = 'button';" onmouseover="this.className='button_over';"
                                                                    style="width: 18%" type="button" value="Update" />&nbsp;</td>
                                                        </tr>--%>
                                                    </table>
                                                </div> 
                                                <div id = "DIVGRID" style="display: block; overflow: auto; height:350px;" >
                                                    <table id="Table19" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id="td4" class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    </td>
                                                            </tr>
                                                            <tr> 
                                                                <td style="height: 74px">
                                                                    <div id = "GRID1" style= "display:block">
                                                                        <table id="Table20" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                            <tr>
                                                                                <td style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 2px"><div id = "Div6" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
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
    
    </div>
    </form>
</body>
</html>
