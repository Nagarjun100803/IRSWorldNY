<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ORD_3D_Monitoring_List.aspx.cs" Inherits="IRSWorld._3D.ORD_3D_Monitoring_List" %>

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
    <script src = "../js/3D/ORD_3D_Monitoring_List.js"      type = "text/javascript">

</script>       
        
</head>
<body id="theBody" onload="search_ORD3D.load_search_currency();fixmain();" style="font-size: medium">
    <form id="Form1" runat="server">
    <div id="divBody">
    <center>
            <div id="ORDSEARCH3D" style="width:auto">
                <br />
                        <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 60%;">
                            <tr>
                                <td style="width: 60%; vertical-align: bottom; text-align: left">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width: 0%; height: 24px" valign=bottom>
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                                height: 24px">
                                                3D Monitoring List</td>
                                            <td align="left" class="bg_body" style="height: 24px; width: 60%;" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 100%">
                                    <table class="table_border" style="width: 100%; height: 100%">                                    
                                        <tr>
                                            <td style="vertical-align: top; width: 100%; height: 100%">
                                                <!-- content starts here -->
                                                <table cellpadding="0" style="width:100%">
                                                    <tr>
                                                        <td style="text-align: center; width: 100%; height: 157px;">
                                                            <table style="width:100; border: solid 1px #336699;">
                                                                <tr>
                                                                    <td class="table_caption" colspan="2" style="height: 23px">
                                                                        LIST OF 3D TRANSACTIONS</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 23px" id="Td1">
                                                                        Status:
                                                                    </td>
                                                                    <td style="width: 20%; height: 23px; text-align: left; color: red;" id="Td2">
                                                                        <select id="ddl_ORD3D_Status_Code" style="width: 94%">
                                                                            <option selected="selected"></option>
                                                                        </select>
                                                                        *</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%" id="Td3">
                                                                        Courier:</td>
                                                                    <td style="width: 20%; text-align: left; color: red;" id="Td5">
                                                                    <select id="ddlORDCourier" style="width: 94%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                        *</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 23px" id="Td6">
                                                                        Currency:</td>
                                                                    <td style="width: 20%; height: 23px; text-align: left; color: red;" id="Td7">
                                                                    <select id="ddl_ORD3D_search_currency" style="width: 94%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                        *</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 21px" id="Td8">
                                                                        Remitting Date (From):</td>
                                                                    <td style="width: 20%; height: 21px; text-align: left; color: red;" id="Td9">
                                                                    <input id="txtORDRemDateFrom" style="width: 61%" type="text" readonly="readOnly" />
                                                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtORDRemDateFrom'), 'mm/dd/yyyy')" />
                                                                        *</td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 20%; height: 21px" id="Td11">
                                                                        Remitting Date (To):</td>
                                                                    <td style="width: 20%; height: 21px; text-align: left; color: red;" id="Td12"><input id="txtORDRemDateTo" style="width: 61%" type="text" readonly="readOnly" />
                                                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtORDRemDateTo'), 'mm/dd/yyyy')" />
                                                                        *</td>
                                                                </tr>
                                                            </table>
                                                            </td>
                                                    </tr>
                                                    
                                                </table>
                                                <!-- content ends here -->
                                                &nbsp; &nbsp; &nbsp;<input id="btnORDSearch" onclick="javascript:ORD3D.load_popup_summary();" style="width: 20%"
                                                    type="button" value="Search" /></td>
                                        </tr>
                                    </table>
                                    
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table id="tbl_search3D" cellpadding="0" cellspacing="0" style="background-color: azure; width: 80% ;display:none">
                            <tr>
                                <td style="width: 100%; height: 100%">
                                    
                                    <table class="table_border" style="width: 100%; height: 100%">
                                        <tr>
                                            <td style="vertical-align: top; width: 100%"> <%--height: 70%">--%>
                                                <!-- content starts here -->
                                                <table cellpadding="0" style="width:100%">
                                                    <tr>
                                                        <td style="text-align: center; width: 80%;">
                                                        <div id="div_courier_search" style="width:100%;"> <%--style="overflow:scroll;width:100%">--%>
                                                        </div>
                                                        <table style="width:100%">
                                                            <tr>
                                                                <td>
                                                                    <input id="chkAll" type=checkbox onclick="ORD3D.check_all(document.getElementById('chkAll').checked);" />
                                                                </td>
                                                                <td align=left class="cell_desc">
                                                                Select All
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        
                                                        
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                                <table style="width:20%">
                                                    <tr>
                                                        <td style="width:50%">
                                                            <input id="btnSendAll" onclick="ORD3D.batch_send_to_courier();" style="width: 100%;display:block"
                                                            type="button" value="Send" />
                                                        </td>
                                                        <td style="width:50%">
                                                            <input id="btnReassign"  style="width: 100%;display:block"
                                                            type="button" value="Re-Assign" onclick="ORD3D.popup_update();" />
                                                        </td>
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
        </center>           
        <center>
        <div id="TRANORDDETAILS" style ="display:block;text-align:center">
            <table id="tblORDTranDetails" cellpadding="0" cellspacing="0" style="background-color: azure; width:80%">
                <tr>
                    <td style="width: 60%; vertical-align: bottom; text-align: left">
                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="left" style="width: 0%; height: 24px" valign="bottom">
                                    <img src="../images/toe.GIF" /></td>
                                <td id="Td4" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                    height: 24px">
                                    3D Transaction Details</td>
                                <td align="left" class="bg_body" style="width: 100%; height: 24px" valign="bottom">
                                    <img align="absBottom" src="../images/rend.gif" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; height: 100%"> <!--colspan="3"-->
                        <table class="table_border" style="width: 100%">
                            <tr>
                                <td style="vertical-align: top; width: 100%; height: 82%">
                                    <!-- content starts here -->
                                        <table cellpadding="0" cellspacing="1" style="width:100%">
                                            <tr>
                                                <td style="width: 98%; height: 95%">
                                                    <table id="Table1" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                                        width:98%">
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                                &nbsp;Reference No :</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblORDReference" class="cell_desc"></label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                Status :</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblORDStatus" class="cell_desc"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                                Reference Number (Theirs) :</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblORDRefnumTheirs" class="cell_desc"></label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                Status Date :&nbsp;</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblORDStatusDate" class="cell_desc"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                                &nbsp;Remittance Date :</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblORDRemittanceDate" class="cell_desc"></label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                &nbsp;Product Name :</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblORDProductName" class="cell_desc"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_caption" style="width: 11%; height: 22px">
                                                                Transaction Date :</td>
                                                            <td align="left" style="width: 9%; height: 22px">
                                                                <label id="lblORDTransactionDate" class="cell_desc"></label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 22px">
                                                                &nbsp;DTB Payee Bank :</td>
                                                            <td align="left" style="width: 18%; height: 22px">
                                                                <label id="lblORDPayeeBank" class="cell_desc"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="cell_caption" style="width: 11%; height: 18px">
                                                                Account Currency:</td>
                                                            <td align="left" style="width: 9%; height: 23px">
                                                                <label id="lblORDAccountCurrency" class="cell_desc"></label></td>
                                                            <td align="right" class="cell_caption" style="width: 8%; height: 18px">
                                                                &nbsp;Amount :</td>
                                                            <td align="left" style="width: 18%; height: 23px">
                                                                <label id="lblORDTransactionAmount" class="cell_desc"></label></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table id="Table2" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid;
                                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                                        width:98%">
                                                        <tr>
                                                            <td class="cell_caption" style="width: 15%; height: 27px">
                                                                Remitter :</td>
                                                            <td align="left" style="width: 33%; height: 27px">
                                                                <label id="lblORDRemitterName" class="cell_desc">
                                                                    &nbsp;</label>
                                                            </td>
                                                            <td class="cell_caption" style="width: 19%; height: 27px">
                                                                Beneficiary:
                                                            </td>
                                                            <td align="left" style="width: 35%; height: 27px">
                                                                <label id="lblORDBeneficiaryName" class="cell_desc">
                                                                    &nbsp;</label>
                                                            </td>
                                                        </tr>
                                                        <tr style="font-size: 13pt">
                                                            <td class="cell_caption" style="width: 15%; height: 24px">
                                                                Address :</td>
                                                            <td align="left" style="width: 33%; height: 24px">
                                                                <label id="lblORDRemitterAddress" class="cell_desc">
                                                                    &nbsp;</label>
                                                            </td>
                                                            <td class="cell_caption" style="width: 19%; height: 24px">
                                                                Address :</td>
                                                            <td align="left" style="width: 35%; height: 24px">
                                                                <label id="lblORDBeneficiaryAddress" class="cell_desc">
                                                                    &nbsp;</label>
                                                            </td>
                                                        </tr>
                                                        <tr style="font-size: 13pt">
                                                            <td class="cell_caption" style="width: 15%; height: 24px">
                                                                Special Message :</td>
                                                            <td align="left" style="width: 33%; height: 24px">
                                                                <label id="lblSpecialMesg" class="cell_desc">
                                                                        &nbsp;</label>
                                                            </td>
                                                            <td class="cell_caption" style="width: 19%; height: 24px">
                                                                Beneficiary Contact No :</td>
                                                            <td align="left" style="width: 35%; height: 24px">
                                                                <label id="lblContact" class="cell_desc">
                                                                    &nbsp;</label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td style="width: 98%; height: 28px; text-align:center" valign="top">
                                                <input onclick="ORD3D.send_txn();" style="width: 15%;
                                                        height: 28px" type="button" value="Send To Courier" id="btnORDSend" />
                                                <input onclick="ORD3D.display_update();" style="width: 15%;
                                                        height: 28px" type="button" value="Re-Assign" id="btnORDReassign" />
                                                <input onclick="ORD3D.do_cancel();" style="width: 15%;
                                                        height: 28px" type="button" value="Back to List" id="btnORD3DCancel" /></td>
                                            
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
        <div id="TRANXORDUPDATES" style="width:auto; position:absolute; z-index:99; left:120px">
                <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 70%;" id="tbl_ORDCourier_Updates">
                    <tr>
                        <td colspan="3" style="width: 100%; height: 100%">
                            <table class="table_border" style="width: 100%; height: 100%">
                                <tr>
                                    <td style="vertical-align: top; width: 125%; height: 100%">
                                        <!-- content starts here -->
                                        <table cellpadding="0" style="width:100%">
                                            <tr>
                                                <td style="text-align: center; width: 100%; height: 120px;">
                                                    <table style="width:100%; border: solid 1px #336699;">
                                                        <tr>
                                                            <td class="table_caption" colspan="2" style="height: 23px">
                                                                3D Transactions Updates</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 23px" id="Td13">
                                                                Courier From:</td>
                                                            <td style="width: 20%; height: 23px; text-align: left;" id="Td14">
                                                            <input id="txtORDCourierFrom" style="width: 95%" type="text" readonly="readOnly" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 12%; height: 23px;" id="Td15">
                                                                Courier To:</td>
                                                            <td style="width: 20%; text-align: left; color: red; height: 23px;" id="Td16">
                                                            <select id="ddl_ORD3D_CourierTo" style="width: 96%">
                                                                <option selected="selected"></option>
                                                            </select>
                                                                *</td>
                                                        </tr>
                                                        
                                                        
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <!-- content ends here -->
                                        <input id="btnORDUpdate" onclick="ORD3D.update();" style="width: 19%"
                                                    type="button" value="Update" />
                                        <input id="btnORDCancelUpdate" onclick="ORD3D.returndetails();" style="width: 19%"
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
