<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSDataEntry.aspx.cs" Inherits="IRSWorld.GRS.GRSDataEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ORD Data Entry</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jsCommons.js"                type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"           type="text/javascript"></script>
    <script src="../js/js_sidebar.js"               type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js"            type="text/javascript"></script>    
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>    
    <script src="../js/Internet/jsInternetPage.js" type="text/javascript"></script>
    <script src="../js/GRS/js_GRSDataEntry.js" type="text/javascript"></script>
</head>
<body onload="ide.init_state();">
    
        <center>
            <form id="form1" runat="server">
                <div id="divMainPanel" style="width: 100%">
                    <table cellpadding="0" cellspacing="5" height="100%" width="100%">
                        <tr>
                            <td align="center" class="" colspan="2" style="width: 100%; border-collapse: collapse"
                                valign="top">
                                <div style="display: block; width: 100%; height: 100%">
                                    <table width="100%">
                                        <tr>
                                            <td style="vertical-align: top; padding-top: 30px; " valign="top">
                                                <div id="divDataEntryMenu" style="display: block">
                                                    <table cellpadding="0" cellspacing="5">
                                                        <tr id="trCreateBatch" style="display: none">
                                                            <td style="border-bottom: #5697c1 1px solid; text-align: right" nowrap>
                                                                <a href="javascript:ide.create_new_batch();" style="font-weight: bold; font-size: 12px;
                                                                    font-style: italic; font-family: Arial">Create Batch</a>
                                                            </td>
                                                        </tr>
                                                        <tr id="trSearchBatch" style="display: none">
                                                            <td style="border-bottom: #5697c1 1px solid; height: 20px; text-align: right" nowrap >
                                                                <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial">
                                                                    Search Batch</a>
                                                            </td>
                                                        </tr>
                                                        <tr id="trSearchCIF" style="display: none" nowrap>
                                                            <td style="border-bottom: #5697c1 1px solid; height: 20px; text-align: right">
                                                                <a href="javascript:cif.searchCIF();" style="font-weight: bold; font-size: 12px;
                                                                    font-style: italic; font-family: Arial">Search Client</a>
                                                            </td>
                                                        </tr>
                                                        <tr id="trAddCIF" style="display: none">
                                                            <td style="border-bottom: #5697c1 1px solid; height: 20px; text-align: right">
                                                                <a href="javascript:show_frame_dialog('../../Internet/InternetRemitter.aspx?isRemit=true', 'internetClose', '100%', '100%', '0');"
                                                                    style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial">
                                                                    Add Client</a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td align="center" style="padding-top: 30px;  width: 795px;" valign="top">
                                                <div id="divicreatebatch" style="display: none">
                                                    &nbsp;<input id="txtctr" style="width: 11%" type="text" />
                                                    <input id="txttotitems_ctr" style="width: 11%" type="text" />
                                                    <table cellpadding="0" cellspacing="0" style="width: 10%; background-color: azure">
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
                                                                        <td align="center" style="width: 100%; height: 8px">
                                                                            <table id="Table4" cellpadding="0" width = "100%" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                border-top: #336699 1px solid; border-left: #336699 1px solid; width: 81%; border-bottom: #336699 1px solid">
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        File Format :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <select id="ddFileFormat" style="width: auto; height: 21px">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        Rate Type :</td>
                                                                                    <td align="left" style="width: auto; height: 23px">
                                                                                        <select id="ddlRateType" style="width: auto">
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        No of items :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtbatchitems" style="width: 59%; text-align: right" type="text" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        Total Remitting Amount :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txttotremamt" style="width: 59%; text-align: right" type="text" /></td>
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
                                                                                        Batch No :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="Text6" style="width: 14%" type="text" />
                                                                                        <select id="Select3" style="width: 67%; height: 21px">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td colspan="2" style="height: 23px">
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
                                                <div id="divibatchholder" style="display: none; overflow: auto; height: 100%">
                                                    <table cellpadding="0" cellspacing="0" style="background-color: azure" width="96%">
                                                        <tr>
                                                            <td align="left" nowrap="nowrap" style="width: 1%; height: 24px" valign="bottom">
                                                                <img src="../images/toe.GIF" /></td>
                                                            <td align="left" class="table_head_dark" nowrap="nowrap" width="20%">
                                                                Batch Information</td>
                                                            <td align="left" class="bg_body" nowrap="nowrap" valign="bottom">
                                                                <img align="absBottom" src="../images/rend.gif" />&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="height: 100%">
                                                                <div id="diviDebit" style="display: none">
                                                                    <table cellpadding="0" cellspacing="0" height="100%" style="background-color: azure"
                                                                        width="100%">
                                                                        <tr>
                                                                            <td colspan="3" style="width: 100%; height: 100%">
                                                                                <table class="table_border" style="width: 100%; height: 100%">
                                                                                    <tr>
                                                                                        <td align="right" style="width: 100%; height: 8px">
                                                                                            <table cellpadding="0" cellspacing="5" width="100%">
                                                                                                <tr>
                                                                                                    <td style="width: 100%; height: 100%" valign="middle">
                                                                                                        <div id="" class="table_body" style="margin: 0px; width: 98%; height: 100%">
                                                                                                            <table id="tblAPASRemRemitter" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                                                border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                                                width="100%">
                                                                                                                <tr>
                                                                                                                    <td align="left" colspan="4">
                                                                                                                        <table style="width: 344px">
                                                                                                                            <tr>
                                                                                                                                <td class="cell_caption" style="width: 54px; text-align: center">
                                                                                                                                    &nbsp;Debit</td>
                                                                                                                                <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Credit','click',this)"
                                                                                                                                    onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)" style="width: 54px;
                                                                                                                                    text-align: center">
                                                                                                                                    &nbsp;Credit</td>
                                                                                                                                <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Charges','click',this)"
                                                                                                                                    onmouseover="javascript:ide.handle_mouseover('Charges','hoverin',this)" style="width: 54px;
                                                                                                                                    text-align: center">
                                                                                                                                    &nbsp;Charges</td>
                                                                                                                                <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                                                    onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)" style="width: 54px;
                                                                                                                                    text-align: center">
                                                                                                                                    &nbsp;Summary</td>
                                                                                                                            </tr>
                                                                                                                        </table>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                        Output Seq. No :</td>
                                                                                                                    <td align="left" style="width: 31%; height: 23px">
                                                                                                                        <input id="txtOutSeqno" style="width: 88%" type="text" /></td>
                                                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                        Rem. Bank :</td>
                                                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                                                        <select id="ddRemBank" style="width: auto; height: 21px">
                                                                                                                            <option selected="selected"></option>
                                                                                                                        </select>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                        Refnum Theirs:&nbsp;</td>
                                                                                                                    <td align="left" style="width: 31%; height: 23px">
                                                                                                                        <input id="txtRefnumTheirs" style="width: 88%" type="text" /></td>
                                                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                        Prot. Bank:</td>
                                                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                                                        <select id="ddProtBank" style="width: auto; height: 21px">
                                                                                                                            <option selected="selected"></option>
                                                                                                                        </select>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="font-size: 13pt">
                                                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                        Date Sent :&nbsp;</td>
                                                                                                                    <td align="left" style="width: 31%; height: 23px">
                                                                                                                        <label id="lblAPASRemAddress" class="cell_desc" style="font-weight: normal; font-size: 15px">
                                                                                                                            <input id="txtDateSent" style="width: 88%" type="text" /></label></td>
                                                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                        </td>
                                                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                                                        &nbsp;</td>
                                                                                                                </tr>
                                                                                                                <tr style="font-size: 13pt">
                                                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                        Date Received :</td>
                                                                                                                    <td align="left" style="width: 31%; height: 23px">
                                                                                                                        <input id="txtDateReceived" style="width: 88%" type="text" /></td>
                                                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="font-size: 13pt">
                                                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                        &nbsp;</td>
                                                                                                                    <td align="left" style="width: 31%; height: 23px">
                                                                                                                        &nbsp;</td>
                                                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                        &nbsp;</td>
                                                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                                                        &nbsp;</td>
                                                                                                                </tr>
                                                                                                                <tr style="font-size: 13pt">
                                                                                                                    <td class="cell_caption" style="width: 19%; height: 24px">
                                                                                                                    </td>
                                                                                                                    <td align="left" style="width: 31%; height: 24px">
                                                                                                                        &nbsp;</td>
                                                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 24px">
                                                                                                                        &nbsp;</td>
                                                                                                                    <td align="left" style="width: 85%; height: 24px">
                                                                                                                        &nbsp;</td>
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
                                                                </div>
                                                                <div id="diviCredit" style="display: none">
                                                                    <table cellpadding="0" cellspacing="0" height="100%" style="background-color: azure"
                                                                        width="100%">
                                                                        <tr>
                                                                            <td colspan="3" style="width: 98%; height: 100%">
                                                                                <table class="table_border" style="width: 100%; height: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 103%">
                                                                                            <table cellpadding="0" cellspacing="5" width="100%">
                                                                                                <tr>
                                                                                                    <td style="width: 100%; height: 100%" valign="middle">
                                                                                                        <table id="Table2" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                                                                                            height: 102%" width="100%">
                                                                                                            <tr>
                                                                                                                <td align="left" colspan="4">
                                                                                                                    <table style="width: 344px">
                                                                                                                        <tr>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Debit</td>
                                                                                                                            <td class="cell_caption" style="width: 54px; text-align: center">
                                                                                                                                &nbsp;Credit</td>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Charges','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Charges','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Charges</td>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Summary</td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Remitter ID :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtRemId" style="width: 88%" type="text" disabled="disabled" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Benef ID :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtBeneId" style="width: 88%" type="text" disabled="disabled" /></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Remitter Accountno :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtRemAcct" style="width: 88%" type="text" disabled="disabled" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Benef&nbsp; AccountNo :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtBeneAcct" style="width: 88%" type="text" disabled="disabled" /></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Remitter Name :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtRemName" style="width: 89%" type="text" disabled="disabled" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Benef Name :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtBeneName" style="width: 88%" type="text" disabled="disabled" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" rowspan="2" style="width: 19%" valign="top">
                                                                                                                    Rem. Address :</td>
                                                                                                                <td align="left" rowspan="2" style="width: 29%" valign="top">
                                                                                                                    <label id="Label2" class="cell_desc" style="font-weight: normal; font-size: 15px">
                                                                                                                        <textarea id="txtRemAdd" style="width: 176px; height: 48px" disabled="disabled"></textarea></label></td>
                                                                                                                <td align="left" class="cell_caption" rowspan="2" style="width: 22%" valign="top">
                                                                                                                    Benef. Address :</td>
                                                                                                                <td align="left" rowspan="2" style="width: 85%" valign="top">
                                                                                                                    <textarea id="txtBeneAdd" style="width: 176px; height: 48px" disabled="disabled"></textarea></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" class="cell_caption" nowrap="nowrap" style="width: 22%; height: 23px">
                                                                                                                    B. Worker Type :&nbsp;</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtWType" style="width: 89%" type="text" disabled="disabled" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    ACCT CCY :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <select id="ddCRAcctCurr" style="width: auto; height: 21px">
                                                                                                                        <option selected="selected"></option>
                                                                                                                    </select>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    B. Account Status :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtBeneAcctStat" style="width: 89%" type="text" disabled="disabled" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td align="right" colspan="4" style="height: 5px; text-align: left" valign="middle">
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Product :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <select id="ddProdCode" style="width: 98%; height: 21px" onchange="javascript:ide.load_implementation();">
                                                                                                                        <option selected="selected"></option>
                                                                                                                    </select>
                                                                                                                </td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Amount :
                                                                                                                </td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtAmt" style="width: 89%" type="text" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Implementation :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <select id="ddImplem" style="width: 98%; height: 21px">
                                                                                                                        <option selected="selected"></option>
                                                                                                                    </select></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Remitting CCY :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                        <select id="ddDRRemCurr" style="width: auto; height: 21px">
                                                                                                                            <option selected="selected"></option>
                                                                                                                        </select></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Charge Type :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px"><select id="ddChargeType" style="width: 98%; height: 21px">
                                                                                                                    <option selected="selected"></option>
                                                                                                                </select>
                                                                                                                </td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    &nbsp;</td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <%-- start here  --%>
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
                                                                <div id="diviCharges" style="display: none">
                                                                    <table cellpadding="0" cellspacing="0" height="100%" style="background-color: azure"
                                                                        width="100%">
                                                                        <tr>
                                                                            <td colspan="3" style="width: 100%; height: 100%">
                                                                                <table class="table_border" style="width: 100%; height: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 100%; height: 8px">
                                                                                            <table cellpadding="0" cellspacing="5" width="100%">
                                                                                                <tr>
                                                                                                    <td style="width: 100%; height: 100%" valign="middle">
                                                                                                        <table id="Table1" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                                            width="100%">
                                                                                                            <tr>
                                                                                                                <td align="left" colspan="4" style="height: 23px">
                                                                                                                    <table style="width: 344px">
                                                                                                                        <tr>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Debit</td>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Credit','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Credit</td>
                                                                                                                            <td class="cell_caption" style="width: 54px; text-align: center">
                                                                                                                                &nbsp;Charges</td>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Summary</td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td id="tdservicefees" align="left" colspan="4" style="height: 23px">
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Gross Peso :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtGrossPeso" style="width: 88%" type="text" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Net Peso :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtNetPeso" style="width: 88%" type="text" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Amount :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtRemAmt" style="width: 89%" type="text" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Net Dollar :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtNetDollar" style="width: 89%" type="text" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    Rate Used :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="Text4" style="width: 89%" type="text" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    Total Charges :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtTotCharges" style="width: 89%" type="text" /></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <%-- start here  --%>
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
                                                                <div id="diviSummary" style="display: none">
                                                                    <table cellpadding="0" cellspacing="0" height="100%" style="background-color: azure"
                                                                        width="100%">
                                                                        <tr>
                                                                            <td colspan="3" style="width: 104%; height: 100%">
                                                                                <table class="table_border" style="width: 100%; height: 100%">
                                                                                    <tr>
                                                                                        <td style="width: 100%; height: 8px">
                                                                                            <table cellpadding="0" cellspacing="5" width="100%">
                                                                                                <tr>
                                                                                                    <td style="width: 100%; height: 100%" valign="middle">
                                                                                                        <table id="Table5" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                                            width="100%">
                                                                                                            <tr>
                                                                                                                <td align="left" colspan="4" style="height: 23px">
                                                                                                                    <table style="width: 344px">
                                                                                                                        <tr>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Debit</td>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Credit','click')"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Credit</td>
                                                                                                                            <td class="tab_unselected" onclick="javascript:ide.handle_mouseover('Charges','click',this)"
                                                                                                                                onmouseover="javascript:ide.handle_mouseover('Charges','hoverin',this)" style="width: 54px;
                                                                                                                                text-align: center">
                                                                                                                                &nbsp;Charges</td>
                                                                                                                            <td class="cell_caption" style="width: 54px; text-align: center">
                                                                                                                                &nbsp;Summary</td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                </td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtTe" style="width: 88%" type="text" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    </td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtUserId" style="width: 88%" type="text" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    TOTA L ITEMS :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtTotItems" style="width: 89%" type="text" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    CURRENT ITEMS :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtCurrItems" style="width: 89%" type="text" /></td>
                                                                                                            </tr>
                                                                                                            <tr style="font-size: 13pt">
                                                                                                                <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                                                    TOTAL AMOUNT :</td>
                                                                                                                <td align="left" style="width: 29%; height: 23px">
                                                                                                                    <input id="txtTotAmt" style="width: 89%" type="text" /></td>
                                                                                                                <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                                                    CURRENT AMOUNT :</td>
                                                                                                                <td align="left" style="width: 85%; height: 23px">
                                                                                                                    <input id="txtCurrAmt" style="width: 89%" type="text" /></td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                        <%-- start here  --%>
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
                                                                <div id="btnButtons">
                                                                    <br />
                                                                    <table>
                                                                        <tr>
                                                                            <td nowrap="nowrap" style="text-align: right;" colspan="4">
                                                                            <table><tr><td> <div id="batchstatus"></div> </td> </tr></table>
                                                                            </td>
                                                                        </tr>
                                                                    <tr>
                                                                    <td style="width: 11%" nowrap>
                                                                    <input id="btnPrevious"  type="button" value="Previous" />
                                                                    </td>
                                                                    <td style="width: 11%" nowrap>
                                                                    <input id="btnNext" type="button" value="Next" />
                                                                    </td>
                                                                    <td style="width: 11%" nowrap>
                                                                    <input id="btnUpdate"  type="button" value="Save" />
                                                                    </td>
                                                                    <td style="width: 11%" nowrap>
                                                                    <input id="btnBack" onclick="javascript:ide.btnCancelCreate_click()" 
                                                                        type="button" value="Back" />
                                                                        </td>
                                                                        </tr></table>
                                                                </div>
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
                    <asp:HiddenField ID="hfSessionid" runat="server" />
                    <asp:HiddenField ID="hfGUID" runat="server" />
                    <asp:HiddenField ID="hfBranchCode" runat="server" />
                </div>
                <div id="divSearch" style="border-right: #000 2px solid; padding-right: 10px; border-top: #000 2px solid;
                    display: none; padding-left: 10px; z-index: 101; left: 8px; padding-bottom: 10px;
                    border-left: #000 2px solid; width: auto; color: darkblue; padding-top: 10px;
                    border-bottom: #000 2px solid; font-family: Arial; position: absolute; top: 1928px;
                    height: auto">
                    <table cellpadding="0" cellspacing="0" style="width: 350px">
                        <tr>
                            <td style="width: 802px; height: 194px">
                                <table cellpadding="0" cellspacing="0" width="350">
                                    <tr>
                                        <td align="left" style="width: 0%; border-collapse: collapse; height: 24px" valign="bottom">
                                            <img src="../images/toe.GIF" /></td>
                                        <td class="table_head_dark" style="width: 50%; height: 24px">
                                            SEARCH</td>
                                        <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                            <img align="absBottom" src="../images/rend.gif" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table border="0" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                                border-left: #336699 3px solid; border-bottom: #336699 3px solid">
                                                <tr>
                                                    <td class="cell_caption" style="width: 250px">
                                                        Search By
                                                    </td>
                                                    <td align="left" style="width: 450px">
                                                        <select id="ddlSearchBy" class="ddl" name="ddSearch" "
                                                             style="width: 99%">
                                                            <option selected="selected" value="1">Remitter</option>
                                                            <option value="2">Beneficiary</option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 250px">
                                                        Last Name
                                                    </td>
                                                    <td align="left" style="width: 450px">
                                                        <input id="txtSLastName" onkeydown="javascript:internet_page.callSearch();" style="width: 99%;
                                                            text-align: left" type="text" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 250px">
                                                        First Name
                                                    </td>
                                                    <td align="left" style="width: 450px">
                                                        <input id="txtSFirstName" onkeydown="javascript:internet_page.callSearch();" style="width: 99%;
                                                            text-align: left" type="text" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 250px">
                                                        Middle Name
                                                    </td>
                                                    <td align="left" style="width: 450px">
                                                        <input id="txtSMiddleName" onkeydown="javascript:internet_page.callSearch();" style="width: 99%;
                                                            text-align: left" type="text" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 250px">
                                                        Account No</td>
                                                    <td align="left" style="width: 450px">
                                                        <input id="txtSAcctNo" onkeydown="javascript:internet_page.callSearch();" style="width: 99%;
                                                            text-align: left" type="text" /></td>
                                                </tr>
                                                <tr align="center">
                                                    <td colspan="2">
                                                        &nbsp;<input id="btnSearch" class="button" onclick="javascript:cif.load_cif_info();"
                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                            style="width: 72px; height: 22px" type="button" value="Search" />
                                                        <input id="btnSearchCancel" class="button" onclick="javascript:cif.btnCancel_click();"
                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                            style="width: 72px; height: 22px" type="button" value="Cancel" /></td>
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
