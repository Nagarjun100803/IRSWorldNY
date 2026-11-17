<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Courier_3D_List.aspx.cs" Inherits="IRSWorld._3D.Courier_3D_List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Doot-To-Door</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src = "../js/jsCommons.js"                      type = "text/javascript"></script>
    <script src = "../js/jscommonmethod.js"                 type = "text/javascript"></script>
    <script src = "../js/jshttprequest.js"                  type = "text/javascript"></script>
    <script src = "../js/jsmodalwindow.js"                  type = "text/javascript"></script>
    <script src = "../js/jsgridselector.js"                 type = "text/javascript"></script>
    <script src ="../js/popcalendar.js"                     type="text/javascript"></script>
    <script src = "../js/3D/Courier_Tran_List.js"           type = "text/javascript"></script>       
        
</head>
<body id="theBody" onload="search_3D.load_search_currency();fixmain();" style="font-size: medium">
    <form id="Form1" runat="server">
    <div id="divBody" style="width:100%">
    <center>
            <div id="SEARCH3D" style="width:auto">
                <br />
                        <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 60%;">
                            <tr>
                                <td style="width: 60%; vertical-align: bottom; text-align: left">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width: 0%; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                                height: 24px">
                                                3D List</td>
                                            <td align="left" class="bg_body" style="height: 24px; width: 60%;" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 103%"> <!--colspan="3" -->
                                    <table class="table_border" style="width: 100%; height: 100%">                                    
                                        <tr>
                                            <td style="vertical-align: top; width: 100%; height: 100%">
                                                <!-- content starts here -->
                                                <table cellpadding="0" style="width:100%">
                                                    <tr>
                                                        <td style="text-align: center; width: 100%; height: 157px;">
                                                            <table style="width:100%; border: solid 1px #336699;">
                                                                <tr>
                                                                    <td class="table_caption" colspan="2" style="height: 23px">
                                                                        LIST OF 3D TRANSACTIONS</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 23px" id="Td1">
                                                                        Status:
                                                                    </td>
                                                                    <td style="width: 20%; height: 23px; text-align: left;" id="Td2">
                                                                        <select id="ddl_3D_Status_Code" style="width: 97%" onchange="courier.set_button_visibility(document.getElementById('ddl_3D_Status_Code').value)">
                                                                            <option selected="selected"></option>
                                                                        </select>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%" id="Td3">
                                                                        Courier:</td>
                                                                    <td class="cell_desc" style="width: 20%; text-align: left;" id="Td5">
                                                                        &nbsp;<input id="txtCouVal" style="width: 29%" type="text" disabled="disabled" />
                                                                        <input id="txtCourier" style="width: 63%" type="text" disabled="disabled" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 23px" id="Td6">
                                                                        Currency:</td>
                                                                    <td style="width: 20%; height: 23px; text-align: left;" id="Td7">
                                                                        &nbsp;<select id="ddl_3D_search_currency" style="width: 95%">
                                                                        <option selected="selected"></option>
                                                                    </select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 21px" id="Td8">
                                                                        Remitting Date (From):</td>
                                                                    <td style="width: 20%; height: 21px; text-align: left;" id="Td9">
                                                                    <input id="txtRemDateFrom" style="width: 61%" type="text" readonly="readOnly" />
                                                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtRemDateFrom'), 'mm/dd/yyyy')" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 21px" id="Td11">
                                                                        Remitting Date (To):</td>
                                                                    <td style="width: 20%; height: 21px; text-align: left;" id="Td12"><input id="txtRemDateTo" style="width: 61%" type="text" readonly="readOnly" />
                                                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtRemDateTo'), 'mm/dd/yyyy')" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                           </td>
                                                    </tr>
                                                    
                                                </table>
                                                <!-- content ends here -->
                                                <input id="btnSearch" onclick="courier.load_popup_summary();" style="width: 30%"
                                                    type="button" value="Search" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                <br />
                <table id="tbl_search_3D" cellpadding="0" cellspacing="0" style="display: none; width: 80%;
                    background-color: azure; display:none">
                    <tr>
                        <td colspan="3" style="width: 100%; height: 100%">
                            <table class="table_border" style="width: 100%; height: 100%">
                                <tr>
                                    <td style="vertical-align: top; width: 100%; height: 70%">
                                        <!-- content starts here -->
                                        <table cellpadding="0" style="width:100%">
                                            <tr>
                                                <td style="width: 80%; height: 70%; text-align: center">
                                                    <div id="div_courier_search" style="width:100%"> <%--style="overflow: scroll; width: 100%">--%>
                                                    </div>
                                                    
                                                    
                                                </td>
                                                
                                            </tr>
                                            <tr>
                                                <td style="width: 80%; height: 70%; text-align: center">
                                                    &nbsp;<input class="btnAcknowledge" onclick="courier.acknowledge_txns();" style="width: 20%;
                                                        height: 22px" type="button" value="Acknowledge" id="btnAcknowledge"  />
                                                    &nbsp;&nbsp;
                                                    <input class="btnForDelivery" onclick="courier.for_delivery_txns();" style="width: 20%;
                                                        height: 22px" type="button" value="For Delivery" id="btnForDelivery" /></td>
                                            </tr>
                                            
                                        </table>
                                        
                                    </td>
                                </tr>
                            </table>                          
                        </td>
                    </tr>
 
                </table>
                    </div>
        </center>           
        <center>
        <div id="TRANDETAILS" style ="display:block;text-align:center">
            <table id="tblTranDetails" cellpadding="0" cellspacing="0" style="width:80%; background-color: azure" >
                <tr>
                    <td style="width: 60%; vertical-align: bottom; text-align: left">
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                    <img src="../images/toe.GIF" /></td>
                                <td id="Td4" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                    height: 24px">
                                    3D Transaction Details</td>
                                <td align="left" class="bg_body" style="width: 443px; height: 24px" valign="bottom">
                                    <img align="absBottom" src="../images/rend.gif" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 100%">
                        <table class="table_border" style="width: 100%">
                            <tr>
                                <td style="vertical-align: top; width: 100%; height: 82%">
                                    <!-- content starts here -->
                                        <table cellpadding="0" cellspacing="1" style="width:100%">
                                            <tr>
                                                <td style="width: 98%; height: 95%" valign="center">
                                                    <table id="Table1" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                                        width:98%">
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                                &nbsp;Reference No :</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblReference" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                Status :</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblStatus" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                                Reference Number (Theirs) :</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblRefnumTheirs" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                Status Date :&nbsp;</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblStatusDate" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                                &nbsp;Remittance Date :</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblRemittanceDate" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                &nbsp;Product Name :</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblProductName" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 22px">
                                                                Transaction Date :</td>
                                                            <td align="left" style="width: 9%; height: 22px">
                                                                <label id="lblTransactionDate" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 22px">
                                                                &nbsp;DTB Payee Bank :</td>
                                                            <td align="left" style="width: 18%; height: 22px">
                                                                <label id="lblPayeeBank" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="width: 11%; height: 18px">
                                                                Account Currency:</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblAccountCurrency" class="cell_desc">
                                                                    &nbsp;</label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                &nbsp;Amount :</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblTransactionAmount" class="cell_desc">
                                                                    </label></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table id="tblRemTVRemitter" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid;
                                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                                        width:98%">
                                                        <tr>
                                                            <td class="cell_caption" style="width: 15%; height: 27px">
                                                                Remitter :</td>
                                                            <td align="left" style="width: 35%; height: 27px;">
                                                                <label id="lblRemitterName" class="cell_desc">
                                                                    &nbsp;</label>
                                                            </td>
                                                            <td class="cell_caption" style="width: 15%; height: 27px">
                                                                Beneficiary:
                                                            </td>
                                                            <td align="left" style="width: 35%; height: 27px">
                                                            <label id="lblBeneficiaryName" class="cell_desc">&nbsp;</label>
                                                            </td>
                                                        </tr>
                                                        <tr style="font-size: 13pt">
                                                            <td class="cell_caption" style="width: 15%; height: 24px">Address :</td>
                                                            <td align="left" style="width: 35%; height: 24px">
                                                                <label id="lblRemitterAddress" class="cell_desc">&nbsp;</label>
                                                            </td>
                                                            <td class="cell_caption" style="width: 15%; height: 24px">Address :</td>
                                                            <td align="left" style="width: 35%; height: 24px">
                                                                <label id="lblBeneficiaryAddress" class="cell_desc">&nbsp;</label>
                                                            </td>
                                                        </tr>
                                                        <tr style="font-size: 13pt">
                                                            <td class="cell_caption" style="width: 15%; height: 24px">
                                                                Special Message:</td>
                                                            <td align="left" style="width: 35%; height: 24px">
                                                                <label id="lblMessage" class="cell_desc">&nbsp;</label></td>
                                                            <td class="cell_caption" style="width: 15%; height: 24px">
                                                                Contact No:</td>
                                                            <td align="left" style="width: 35%; height: 24px">
                                                                <label id="lblBeneContact" class="cell_desc">&nbsp;</label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td style="width: 98%; height: 28px; text-align:center" valign="top">
                                                <input class="btnSave" onclick="courier.display_update();" style="width: 15%;
                                                        height: 28px" type="button" value="Update Status" id="btnSave" />
                                                <input class="btnUpdateCancel" onclick="courier.do_cancel();" style="width: 15%;
                                                        height: 28px" type="button" value="Cancel" id="btnUpdateCancel"/></td>
                                            
                                            </tr>
                                        </table>
                                    <!-- content ends here -->
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            </div>
            <div id="TRANXUPDATES" style ="display:block;text-align:center">
                <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 70%;" id="tbl_Courier_Updates">
                    <tr>
                        <td style="width: 100%; height: 100%">
                            <table class="table_border" style="width: 100%; height: 100%">
                                <tr>
                                    <td style="vertical-align: top; width: 125%; height: 100%">
                                        <!-- content starts here -->
                                        <table cellpadding="0" style="width:100%">
                                            <tr>
                                                <td style="text-align: center; width: 100%; height: 157px;">
                                                    <table style="width:100%; border: solid 1px #336699;">
                                                        <tr>
                                                            <td class="table_caption" colspan="2" style="height: 23px">
                                                                Courier's Entry</td>
                                                        </tr>
                                                        <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 21px" id="Td21">
                                                            3D Status:</td>
                                                            <td style="width: 20%; height: 21px; text-align: left;" id="Td22">
                                                                <select id="ddl_Courier_Status" style="width: 94%">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr><td class="cell_caption" style="width: 20%; height: 21px" id="Td17">
                                                            Reason/s:</td>
                                                            <td style="width: 20%; height: 21px; text-align: left;" id="Td18">
                                                                <select id="ddl_3D_Reason" style="width: 94%">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>
                                                        <tr><td class="cell_caption" style="width: 20%; height: 21px" id="Td19">
                                                            Remarks:</td>
                                                            <td style="width: 20%; height: 21px; text-align: left;" id="Td20">
                                                                <input id="txtRemarks" style="width: 93%" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 20%; height: 21px">
                                                                Date Updated:</td>
                                                            <td style="width: 20%; height: 21px; text-align: left">
                                                                <input id="txtDateUpdated" style="width: 61%" type="text" readonly="readOnly" />
                                                                <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtDateUpdated'), 'mm/dd/yyyy')" /></td>
                                                        </tr>
                                                    </table>
                                                    <table style="width:100%; border: solid 1px #336699;">
                                                        <tr>
                                                            <td class="table_caption" colspan="2" style="height: 23px">
                                                                Courier's Remarks</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 14%; height: 23px" id="Td13">
                                                                Received By:</td>
                                                            <td style="width: 20%; height: 23px; text-align: left;" id="Td14">
                                                            <input id="txtReceivedBy" style="width: 93%" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 14%" id="Td15">
                                                                ID's Received:</td>
                                                            <td class="cell_desc" style="width: 20%; text-align: left;" id="Td16">
                                                            <input id="txtIDReceived" style="width: 93%" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 14%; height: 21px" id="Td25">
                                                                Reason/s:</td>
                                                            <td style="width: 20%; height: 21px; text-align: left;" id="Td26">
                                                            <input id="lblCou_Reasons" style="width: 93%" type="text" disabled="disabled" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 14%; height: 21px" id="Td27">
                                                                Remarks:</td>
                                                            <td style="width: 20%; height: 21px; text-align: left;" id="Td28">
                                                            <input id="lblCou_Remarks" style="width: 93%" type="text" disabled="disabled" /></td>
                                                        </tr>
                                                        
                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- content ends here -->
                                        <input id="btnUpdate" onclick="courier.update();" style="width: 19%"
                                                    type="button" value="Save" /> 
                                        <input id="btnCancel" onclick="courier.returndetails();" style="width: 19%"
                                                    type="button" value="Cancel" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            <br />
        </div>
        </center>
    </div>
    </form>
</body>
</html>
