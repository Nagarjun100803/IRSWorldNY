<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PaymentGatewayDefaults.aspx.cs"
    Inherits="IRSWorld.BranchSetup.PaymentGatewayDefaults" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment Gateway Defaults</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_BranchGateway.js" type="text/javascript"></script>
    <script type="text/javascript">
            var Payment_Gateway = new Payment_Gateway();
    </script>

</head>
<body onload="javascript:Payment_Gateway.load();">
    <center>
        <form id="form1" runat="server">
            <div id="divBodyRegion" style="width: 98%; display: none; vertical-align: top; margin-top: 30px;">
                <table id="tblRegion" style="width: 50%;"><!-- display: block"-->
                    <tr>
                        <td id="td3" style="width: 100%; border-collapse: collapse; vertical-align: middle;">
                            <table style="width:100%;" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="vertical-align: bottom">
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" style="width: 1%; height: 24px; border-collapse: collapse; vertical-align: bottom">
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="tdRegionPageTitle" class="table_head_dark" style="width: 50%; height: 24px">
                                                    Region</td>
                                                <td align="left" style="width: 100%; height: 24px; vertical-align: bottom">
                                                    <img align="absBottom" src="../images/rend.gif" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                        align="center">
                                        <br />
                                        <table cellpadding="0" cellspacing="5" width="95%">
                                            <tr>
                                                <td class="cell_caption" style="width: 35%; height: 23px">
                                                    BY REGION :&nbsp;</td>
                                                <td align="left" style="width: 65%; height: 23px;">
                                                    <select id="ddlRegion" style="width: 100%" onchange="javascript:Payment_Gateway.load_country();" >
                                                    </select>
                                                </td>
                                                </tr>
                                                <tr id = "idCountry" style="display:none;">
                                                <td class="cell_caption" style="width: 35%; height: 23px">
                                                    COUNTRY :&nbsp;</td>
                                                <td align="left" style="width: 65%; height: 23px;">
                                                    <select id="ddlCountry" style="width: 100%">
                                                    </select>
                                                </td>
                                                </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="width: 100%; height: 51px;" align="center">
                                                    <table id="tbl_button" cellpadding="0" cellspacing="5">
                                                        <tr>
                                                            <td style="width: 20%; vertical-align: middle" align="center">
                                                                <input id="btn_Search" class="button" onclick="javascript:Payment_Gateway.check_region();"
                                                                    type="button" value="Search" /></td>
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
            <div id="divBodyAuthorizeNet" style="width: 98%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblAuthorizeNet" style="width: 100%;">
                    <tr>
                        <td id="tdAuthorizeNet" style="width: 85%; border-collapse: collapse;
                            vertical-align: middle;">
                            <div id="divAuthorizeNetDefault" style="width: 80%;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: bottom">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 0%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="tdPageTitle" class="table_head_dark" style="width: 50%; height: 24px">
                                                        AUTHORIZE.NET DEFAULTS</td>
                                                    <td align="left" style="width: 100%; height: 24px; vertical-align: bottom;">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                            align="center">
                                            <br />
                                            <div id="divAuthorizeNet" style="width: 95%; display: block">
                                                <table id="tblAuthorizeNetDefault" cellpadding="0" cellspacing="5" width="95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            URL :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtXURL" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Login :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtXLogin" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height: 23px">
                                                            Tran Key :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtXTrankey" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height: 23px">
                                                            &nbsp;Version : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtXVersion" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height: 23px">
                                                            &nbsp; Test Request : &nbsp;
                                                        </td>
                                                        <td align="left" style="height: 24px; width: 376px;">
                                                            <select id="ddlXTestRequest" style="width: 100%">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height: 23px">
                                                            &nbsp; Type : &nbsp;
                                                        </td>
                                                        <td align="left" style="height: 24px; width: 376px;">
                                                            <select id="ddlXType" style="width: 100%">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height: 23px">
                                                            &nbsp; Relay Response : &nbsp;
                                                        </td>
                                                        <td style="height: 23px; text-align: left;">
                                                            <input id="chkXRelayResponse" style="width: auto;" type="checkbox" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height: 23px">
                                                            &nbsp; Method : &nbsp;
                                                        </td>
                                                        <td align="left" style="height: 24px; width: 376px;">
                                                            <select id="ddlXMethod" style="width: 100%">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Merchant Email :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtXMerchantEmail" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Data Delimiter :&nbsp;</td>
                                                        <td style="height: 23px; text-align: left;">
                                                            <input id="chkXDataDelimiter" style="width: auto;" type="checkbox" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Character Delimiter :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtXCharacterDelimiter" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Email Customer :&nbsp;</td>
                                                        <td style="height: 23px; text-align: left;">
                                                            <input id="chkXEmailCustomer" style="width: auto;" type="checkbox" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnXEdit" class="button" onclick="javascript:Payment_Gateway.update_record();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Edit" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnXSave" class="button" onclick="javascript:Payment_Gateway.commit_record();"
                                                                            type="button" value="Save" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnXCancel" class="button" onclick="javascript:Payment_Gateway.load();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="divReceipt" style="display: none">
                                            </div>
                                            <div id="divRecptArchive" style="display: none">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divBodyChaseExact" style="width: 98%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyChaseExact" style="width: 100%;">
                    <tr>
                        <td id="tdChaseExact" style="width: 85%; border-collapse: collapse;
                            vertical-align: middle;">
                            <div id="divChaseExactDefault" style="width: 80%;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: bottom">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 0%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td4" class="table_head_dark" style="width: 50%; height: 24px">
                                                        CHASE.E-XACT DEFAULTS</td>
                                                    <td align="left" style="width: 100%; height: 24px; vertical-align: bottom;">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                            align="center">
                                            <br />
                                            <div id="divChaseExact" style="width: 95%; display: block">
                                                <table id="tblExactChaseDefault" cellpadding="0" cellspacing="5" width="95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            URL :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtExactURL" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Login :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtExactLogin" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="height: 23px">
                                                            Transaction Key :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtExactTranKey" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Response Key :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtExactResponseKey" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                   <tr>
                                                        <td class="cell_caption" style="width: 35%; height: 23px">
                                                            Description :&nbsp;</td>
                                                        <td align="left" style="width: 376px; height: 24px;">
                                                            <input id="txtExactDescription" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtHiddenSeqID3" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons3" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnExactEdit" class="button" onclick="javascript:Payment_Gateway.update_record_exact();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Edit" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnExactSave" class="button" onclick="javascript:Payment_Gateway.commit_record_exact();"
                                                                            type="button" value="Save" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnExactCancel" class="button" onclick="javascript:Payment_Gateway.load();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="div6" style="display: none">
                                            </div>
                                            <div id="div7" style="display: none">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divBodySagepay" style="width: 98%; display: none; vertical-align: top; margin-top: 30px;">
                <table id="tblBodySagePay" style="width: 100%;">
                    <tr>
                        <td id="tdSagePay" style="width: 85%; border-collapse: collapse; vertical-align: middle;">
                            <div id="divSagePayDefault" style="width: 80%;">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: bottom">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 0%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td2" class="table_head_dark" style="width: 50%; height: 24px">
                                                        SAGEPAY DEFAULTS</td>
                                                    <td align="left" style="width: 100%; height: 24px; vertical-align: bottom;">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse" class="table_body"
                                            align="center">
                                            <br />
                                            <div id="divSagePay" style="width: 95%; display: block">
                                                <table id="tblSagePay" cellpadding="0" cellspacing="5" width="95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            URL :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtSagePayURL" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            3D Secure URL :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txt3DSecureURL" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            VPS Protocol :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtVPSProtocol" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Tx Type : &nbsp;
                                                        </td>
                                                        <td align="left" style="height: 24px; width: 70%;">
                                                            <select id="ddlTxType" style="width: 100%">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Vendor :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtVendor" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Description :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtDescription" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Agree Gift Aid :&nbsp;</td>
                                                        <td align="left" style="width: 70%; height: 24px; text-align: left">
                                                            <input id="ChkGiftAidPayment" style="width: auto;" type="checkbox" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Apply AVSCV2 : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 70%; height: 24px; text-align: left">
                                                            <input id="ChkApplyAVSCV2" style="width: auto;" type="checkbox" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Apply 3DSecure : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 70%; height: 24px; text-align: left">
                                                            <input id="ChkApply3DSecure" style="width: auto;" type="checkbox" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Account Type : &nbsp;
                                                        </td>
                                                        <td align="left" style="height: 24px; width: 70%;">
                                                            <select id="ddlAccountType" style="width: 100%">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Merchant Email : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 70%; height: 24px;">
                                                            <input id="txtMerchantEmail" style="width: 100%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtHiddenSeqId2" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons2" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnEdit" class="button" onclick="javascript:Payment_Gateway.update_record_sagepay();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Edit" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSave" class="button" onclick="javascript:Payment_Gateway.commit_record_sagepay();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Save" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCancel" class="button" onclick="javascript:Payment_Gateway.load();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="div4" style="display: none">
                                            </div>
                                            <div id="div5" style="display: none">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </center>
</body>
</html>
