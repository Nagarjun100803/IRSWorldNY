<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="3DOverride.aspx.cs" Inherits="IRSWorld._3D._DOverride" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jsCommons.js"></script>
    <script type="text/javascript" src="../js/jscommonmethod.js"></script>
    <script type="text/javascript" src="../js/jsmodalwindow.js"></script>
    <script type="text/javascript" src="../js/jsgridselector.js"></script>
    <script type="text/javascript" src="../js/3D/js3DOverride.js"></script>
    <script type="text/javascript" src="../js/popcalendar.js"></script>


</head>
<body onload="page_setup();">
    <form id="form1" runat="server">
    
    <div style="margin-top:2%">
    <center>
        <table id="tblSearch" cellpadding="0" cellspacing="0" style="width: 60%; background-color: azure">
            <tr>
                <td align="left" style="width: 1%; height: 24px" valign="bottom">
                    <img src="../images/toe.GIF" /></td>
                <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                    height: 24px">
                    Search</td>
                <td align="left" class="bg_body" style="width: 100%; height: 24px" valign="bottom">
                    <img align="absBottom" src="../images/rend.gif" /></td>
            </tr>
            <tr>
                <td colspan="3" style="width: 100%; height: 103%">
                    <table class="table_border" style="width: 100%; height: 100%">
                        <tr>
                            <td style="vertical-align: top; width: 100%; height: 100%">
                                <!-- content starts here -->
                                <table cellpadding="0" width="100%">
                                    <tr>
                                        <td style="width: 100%; height: 100px; text-align: center" valign=top>
                                            <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid;
                                                border-bottom: #336699 1px solid" width="100%">
                                                <tr>
                                                    <td class="table_caption" colspan="2" style="height: 23px">
                                                        LIST OF 3D TRANSACTIONS</td>
                                                </tr>
                                                
                                                <tr>
                                                    <td id="Td3" class="cell_caption" style="width: 20%">
                                                        Courier:</td>
                                                    <td id="Td5" class="cell_desc" style="width: 20%; text-align: left">
                                                        &nbsp;<select id="ddlCourier" 
                                                            style="width: 96%">
                                                            <option selected="selected"></option>
                                                        </select></td>
                                                </tr>
                                                
                                                <tr>
                                                    <td id="Td8" class="cell_caption" style="width: 20%; height: 21px">
                                                        Date:
                                                    </td>
                                                    <td id="Td9" style="width: 20%; height: 21px; text-align: left">
                                                        <input id="txtDate" readonly="readonly" style="width: 61%" type="text" />
                                                        <img align="absMiddle" onclick="popUpCalendar(this, document.getElementById('txtDate'), 'mm/dd/yyyy')"
                                                            src="../images/calendar/imgcalen.png" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan=2 id="tbl_data">
                                                    
                                                    </td>
                                                </tr>
                                              
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <!-- content ends here -->
                                <input id="btnSearch" style="width: 30%" onclick="get_override_details();"
                                    type="button" value="Search" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    
        <table id="tblTranDetails" cellpadding="0" cellspacing="0" style="background-color: azure"
            width="80%">
            <tr>
                <td align="left" style="width: 1%; height: 24px" valign="bottom">
                    <img src="../images/toe.GIF" /></td>
                <td id="Td4" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                    height: 24px">
                    3D Override</td>
                <td align="left" class="bg_body" style="width: 443px; height: 24px" valign="bottom">
                    <img align="absBottom" src="../images/rend.gif" /></td>
            </tr>
            <tr>
                <td colspan="3" style="width: 100%; height: 100%">
                    <table class="table_border" style="width: 100%">
                        <tr>
                            <td style="vertical-align: top; width: 100%; height: 82%">
                                <!-- content starts here -->
                                <table cellpadding="0" cellspacing="1" width="100%">
                                    <tr>
                                        <td style="width: 98%; height: 95%" valign="center">
                                            <table id="Table1" cellpadding="0" cellspacing="1" style="border-right: #336699 1px solid;
                                                border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                width="98%">
                                                <tr>
                                                    <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                        &nbsp;Reference No (Ours) :</td>
                                                    <td align="left" style="width: 9%; height: 23px">
                                                        <label id="lblRefNoOurs" class="cell_desc">
                                                            <strong><span style="font-size: 9pt; color: #000080; font-family: Arial">&nbsp;</span></strong></label></td>
                                                    <td align="right" class="cell_caption" style="font-weight: bold; font-size: 9pt;
                                                        width: 8%; color: #000080; font-family: Arial; height: 18px">
                                                        Transaction Date:</td>
                                                    <td align="left" style="width: 18%; height: 23px">
                                                        <label id="lblTranxDate" class="cell_desc">
                                                            <strong><span style="font-size: 9pt; color: #000080; font-family: Arial">&nbsp;</span></strong></label></td>
                                                </tr>
                                                <tr style="font-weight: bold; font-size: 9pt; color: #000080; font-family: Arial">
                                                    <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                        Reference Number (Theirs) :</td>
                                                    <td align="left" style="width: 9%; height: 23px">
                                                        <label id="lblRefnumTheirs" class="cell_desc">
                                                            <span style="color: #000080">&nbsp;</span></label></td>
                                                    <td align="right" class="cell_caption" style="width: 8%; color: #000080; height: 18px">
                                                        Product Name :&nbsp;</td>
                                                    <td align="left" style="width: 18%; height: 23px">
                                                        <label id="lblProdNm" class="cell_desc">
                                                            <span style="color: #000080">&nbsp;</span></label></td>
                                                </tr>
                                                <tr style="font-size: 9pt; color: #000080; font-family: Arial">
                                                    <td align="right" class="cell_caption" style="width: 11%; height: 18px">
                                                        &nbsp;Remittance Date :</td>
                                                    <td align="left" style="width: 9%; height: 23px">
                                                        <label id="lblRemittanceDate" class="cell_desc">
                                                            <strong><span style="color: #000080">&nbsp;</span></strong></label></td>
                                                    <td align="right" class="cell_caption" style="font-weight: bold; width: 8%; color: #000080;
                                                        height: 18px">
                                                        &nbsp;Product Type :</td>
                                                    <td align="left" style="width: 18%; height: 23px">
                                                        <label id="lblProdType" class="cell_desc">
                                                            <strong><span style="color: #000080">&nbsp;</span></strong></label></td>
                                                </tr>
                                                <tr style="font-weight: bold; font-size: 9pt; color: #000080; font-family: Arial">
                                                    <td align="right" class="cell_caption" style="width: 11%; height: 22px">
                                                        Remitting Bank:</td>
                                                    <td align="left" style="width: 9%; height: 22px">
                                                        <label id="lblRemittingBank" class="cell_desc">
                                                            <span style="color: #000080">&nbsp;</span></label></td>
                                                    <td align="right" class="cell_caption" style="width: 8%; color: #000080; height: 22px">
                                                        &nbsp;Amount :</td>
                                                    <td align="left" style="width: 18%; height: 22px">
                                                        <label id="lblAmount" class="cell_desc">
                                                            <span style="color: #000080">&nbsp;</span></label></td>
                                                </tr>
                                                
                                            </table>
                                            <br />
                                            <table id="tblRemTVRemitter" cellpadding="0" cellspacing="2" style="border-right: #336699 1px solid;
                                                border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                width="98%">
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%; height: 27px">
                                                        Remitter :</td>
                                                    <td align="left" style="width: 35%; height: 27px">
                                                        <label id="lblRemitterName" class="cell_desc">
                                                            <strong><span style="font-size: 9pt; color: #000080; font-family: Arial">&nbsp;</span></strong></label>
                                                    </td>
                                                    <td class="cell_caption" style="width: 15%; height: 27px">
                                                        Beneficiary:
                                                    </td>
                                                    <td align="left" style="width: 35%; height: 27px">
                                                        <label id="lblBeneficiaryName" class="cell_desc">
                                                            <strong><span style="font-size: 9pt; color: #000080; font-family: Arial">&nbsp;</span></strong></label>
                                                    </td>
                                                </tr>
                                                <tr style="font-size: 13pt">
                                                    <td class="cell_caption" style="width: 15%; height: 69px">
                                                        Address :</td>
                                                    <td align="left" style="width: 35%; height: 69px" valign=top>
                                                        <label id="lblRemitterAddress" class="cell_desc">
                                                            <strong><span style="font-size: 9pt; color: #000080; font-family: Arial">&nbsp;</span></strong></label>
                                                    </td>
                                                    <td class="cell_caption" style="width: 15%; height: 69px" >
                                                        Address :</td>
                                                    <td align="left" style="width: 35%; height: 69px" valign=top>
                                                        <label id="lblBeneficiaryAddress" class="cell_desc">
                                                            <strong><span style="font-size: 9pt; color: #000080; font-family: Arial">&nbsp;</span></strong></label>
                                                    </td>
                                                </tr>
                                                <tr style="font-size: 13pt">
                                                    <td class="cell_caption" style="width: 15%; height: 24px">
                                                        Courier Reason :</td>
                                                    <td align="left" style="width: 35%; height: 24px" colspan=3><select id="ddlReason" style="width: 40%">
                                                        <option selected="selected"></option>
                                                    </select>
                                                    </td>
                                                   
                                                </tr>
                                                <tr style="font-size: 13pt">
                                                    <td class="cell_caption" style="width: 15%; height: 24px">
                                                        Courier Remarks :</td>
                                                    <td align="left" style="width: 35%; height: 24px" colspan=3>
                                                        <textarea id="txtRemarks" rows="2" style="width: 40%"></textarea>
                                                        <input id="txtSeq_Hidden" style="width: 4%" type="text" visible=false /></td>
                                                   
                                                </tr>
                                                <tr style="font-size: 13pt">
                                                    <td class="cell_caption" style="width: 15%; height: 24px">
                                                        Date :</td>
                                                    <td align="left" style="width: 40%; height: 24px" colspan=3><input id="txtTranDate" style="width: 40%" type="text" />
                                                        <img align="absMiddle" onclick="popUpCalendar(this, document.getElementById('txtTranDate'), 'mm/dd/yyyy')"
                                                            src="../images/calendar/imgcalen.png" /></td>
                                                   
                                                </tr>
                                                <tr style="font-size: 13pt">
                                                    <td class="cell_caption" style="width: 15%; height: 24px">
                                                        Status :</td>
                                                    <td align="left" style="width: 35%; height: 24px" colspan=3>
                                                        <select id="ddlStatus" style="width: 40%">
                                                            <option selected="selected"></option>
                                                        </select>
                                                    </td>
                                                   
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 98%; height: 28px; text-align: center" valign="top">
                                            <input id="btnOverride" class="btnSave" onclick="update_status();" style="width: 15%;
                                                height: 28px" type="button" value="Override" />
                                            <input id="btnUpdateCancel" class="btnUpdateCancel" onclick="set_visibility('Cancel');"
                                                style="width: 15%; height: 28px" type="button" value="Cancel" /></td>
                                    </tr>
                                </table>
                                <!-- content ends here -->
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </center>
    </div>
    </form>
</body>
</html>
