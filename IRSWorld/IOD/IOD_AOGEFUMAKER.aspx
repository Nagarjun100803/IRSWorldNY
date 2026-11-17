<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IOD_AOGEFUMAKER.aspx.cs" Inherits="IRSWorld.GRS.GRSBatchEntries" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js"                type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"           type="text/javascript"></script>
    <script src="../js/js_sidebar.js"               type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js"            type="text/javascript"></script>    
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>    
    <script src ="../js/popcalendar.js"                     type="text/javascript"></script>
    <script src="../js/GRS/js_GRSBatchEntries.js" type="text/javascript"></script>    
    <script src="../js/Internet/jsInternetPage.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ie_page = new internet_page();
    </script>
</head>
<body onload="javascript: ide.page_setup();">
    
        <center>
            <form id="form1" runat="server">
                <div id="divMainPanel" style="width: 100%">
                    <table cellpadding="0" cellspacing="5" height="100%" width="100%" align="left">
                        <tr>
                            <td align="left" class="" colspan="2" style="width: 100%; border-collapse: collapse"
                                valign="top">
                                <div style="display: block; width: 100%; height: 100%">
                                    <table style="width: 912px" >
                                        <tr>
                                            <td align="center" style="width: 140px; padding-top: 30px" valign="top">
                                                <table cellpadding="0" cellspacing="5">
                                                    <tr id="trCreateBatch" >
                                                        <td nowrap="nowrap" style="border-bottom: #5697c1 1px solid; text-align: right">
                                                            <a href="javascript:ide.create_new_batch();" style="font-weight: bold; font-size: 12px;
                                                                font-style: italic; font-family: Arial">Create Batch</a>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSearchBatch" >
                                                        <td nowrap="nowrap" style="border-bottom: #5697c1 1px solid; height: 20px; text-align: right">
                                                            <a href="javascript:ide.lnkSearch_click();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial">
                                                                Search Batch</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td align="center" style="padding-top: 30px;  " valign="top">
                                               <div id="divicreatebatch" style="display: none">
                                                <table cellpadding="0" cellspacing="0" style="width: 1%; background-color: azure; height: 224px;">
                                                    <tr>
                                                        <td align="left" nowrap="nowrap" style="width: 1%; height: 24px" valign="bottom">
                                                            <img src="../images/toe.GIF" /></td>
                                                        <td align="left" class="table_head_dark" nowrap="nowrap" width="20%">
                                                            Create Batch</td>
                                                        <td align="left" class="bg_body" nowrap="nowrap" style="width: 400px" valign="bottom">
                                                            <img align="absBottom" src="../images/rend.gif" />&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="width: 100%">
                                                            <table class="table_border" style="width: 100%">
                                                                <tr>
                                                                    <td align="center" style=" height: 8px; width: 500px;">
                                                                        <table id="Table4" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                            border-top: #336699 1px solid; border-left: #336699 1px solid;border-bottom: #336699 1px solid; height: 168px; width: 496px;">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px" nowrap>
                                                                                    File Format :</td>
                                                                                <td align="left" style="width: 246px; height: 23px">
                                                                                    <select id="ddFileFormat" style="width: auto; height: 21px">
                                                                                        <option selected="selected"></option>
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" nowrap="nowrap" style="width: 17%; height: 23px">
                                                                                    Rate :</td>
                                                                                <td align="left" style="height: 23px">
                                                                                    <select id="ddlRateType" style="width: auto; height: auto">
                                                                                    </select>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px" nowrap>
                                                                                    No of items :</td>
                                                                                <td align="left" style=" height: 23px" >
                                                                                    <input id="txtbatchitems" style="width: 28%; text-align: right" type="text" onkeypress="javascript:return ie_page.isNumberKey(event);" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 17%; height: 23px" nowrap>
                                                                                    Total Remitting Amount :</td>
                                                                                <td align="left" style="height: 23px">
                                                                                    <input id="txttotremamt" style="width: 46%; text-align: right" type="text" onkeypress="javascript:return(ie_page.autoFormatCurrency(this,',','.',event));"/></td>
                                                                            </tr>
                                                                            <tr style="font-size: 13pt">
                                                                                <td colspan="2" style="height: 30px">
                                                                                    <input id="btnCreateBatch" onclick="javascript:ide.prepare_new_batch();" style="width: 17%"
                                                                                        type="button" value="Create" />
                                                                                    <input id="btnCancelCreate" onclick="javascript:ide.btnCancelCreate_click()" style="width: 17%;
                                                                                        height: 20px" type="button" value="Cancel" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                </div> 
                                               <div id="diviinquiry" style="display: none">
                                                    <table cellpadding="0" cellspacing="0" style="width: 1%; background-color: azure">
                                                        <tr>
                                                            <td align="left" nowrap="nowrap" style="width: 1%; height: 24px" valign="bottom">
                                                                <img src="../images/toe.GIF" /></td>
                                                            <td align="left" class="table_head_dark" nowrap="nowrap" width="20%">
                                                                Search Batch</td>
                                                            <td align="left" class="bg_body" nowrap="nowrap" style="width: 400px" valign="bottom">
                                                                <img align="absBottom" src="../images/rend.gif" />&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="width: 100%; height: 100%">
                                                                <table class="table_border" style="width: 100%; height: 100%">
                                                                    <tr>
                                                                        <td align="center" style="width: 100%; height: 8px">
                                                                            <table id="Table3" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                border-top: #336699 1px solid; border-left: #336699 1px solid; width: 100%; border-bottom: #336699 1px solid">
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        File Format :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <select id="ddSFileFormat" style="width: auto; height: 21px">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        Transaction Date :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                    <input id="txtBatchDate" type="text" style="width: 49%;text-align:right;" />
                                                                                <img align="absMiddle" onclick="javascript:popUpCalendar(this, document.getElementById('txtBatchDate'), 'mm/dd/yyyy')"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        Batch No :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtbatchnum" style="width: 27%" type="text"  onkeypress="javascript:return ie_page.isNumberKey(event);" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td colspan="2" style="height: 31px">
                                                                                        <input id="btnSearch" onclick="javascript:ide.btnSearch_click();" style="width: 17%"
                                                                                        type="button" value="Search" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td colspan="2"   >
                                                                                    <div id="divbatchinfo" style="display:none; width:100%; ">
                                                                                        <table style="width: 152px">
                                                                                        <tr><td id="tdbatchinfo"></td></tr>
                                                                                        <tr><td ><input id="Button1" onclick="javascript:ide.btnAddItem_click();" 
                                                                                        type="button" value="Add Item" style="width: 53%" /></td></tr>
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
                                                </div>
                                             
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hfTranxDate" runat="server" />
                              </div>
                              
                            </td>
                        </tr>
                    </table>
                   
                </div>
            </form>
        </center>

</body>
</html>
