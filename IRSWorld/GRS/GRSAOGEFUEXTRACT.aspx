<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSAOGEFUEXTRACT.aspx.cs" Inherits="IRSWorld.GRS.GRSAOGEFUEXTRACT" %>

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
  
    <form id="form1" runat="server">
    <div id="divsublookup" style="width:93%; height:100%" class=content_height>
        <table width="100%" height="100%" cellpadding="30" cellspacing="5">
            <tr>    
                <td align="center" class="content_height" style="width: 16%; border-collapse: collapse"
                    valign="top">
                </td>
                <td align="center" valign="top" style="width:117%; border-collapse:collapse" class="content_height">
                    <table cellspacing="0" cellpadding="0" style="width: 91%" id="TABLE1">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                    <img src="../images/toe.GIF" /></td>
                                <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                    BPO Account Open Viewer</td>
                                <td id=tdpopup2 align="left" valign="bottom" style="height: 24px; width: 270px;" class="bg_body">
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                <table style="width:100%; border:solid 3px #336699;">
                                <tr>
                                        <td style="width: 23%; height: 21px;" align="left" class="cell_caption">
                                            &nbsp; Branch Code:</td>
                                        <td align="left" style="height: 21px; width: 451px;"><div id="divbranchcode">
                                            <select id="ddBranchCode"  onchange="javascript: set_pagectr(1);show_groups()" style="width: auto">
                                                <option selected="selected"></option>
                                               
                                            </select>
                                            &nbsp;
                                             </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 23%; height: 21px;" align="left" class="cell_caption">
                                            &nbsp; Transaction Date :</td>
                                        <td align="left" style="height: 21px; width: 451px;"><div id="divfldgroup">
                                            <input id="txtDate" style="width: 30%; text-align: right" type="text" />
                                            <img align="absMiddle" onclick="javascript:popUpCalendar(this, document.getElementById('txtBatchDate'), 'mm/dd/yyyy')"
                                                src="../images/calendar/imgcalen.png" /></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="height: 28px">
                                            <input id="btnSearch" onclick="javascript:ide.btnSearch_click();" style="width: 17%"
                                                type="button" value="Search" /></td>
                                    </tr>
                                    <tr>
                                        <td id="tdIOD" colspan="2" style="width: 100%; height: 21px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 100%; height: 21px">
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

</body>
</html>
