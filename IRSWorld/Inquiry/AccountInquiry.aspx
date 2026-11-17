<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountInquiry.aspx.cs" Inherits="IRSWorld.Inquiry.AccountInquiry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Flexcube Account Inquiry</title>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/Inquiry/jsAccountInquiry.js" type="text/javascript"></script>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var account_inquiry = new account_inquiry();
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table style="width:100%">
        <tr>
            <td align="center" style="vertical-align:top;width:100%;height:auto;">
                <div>
                    <table cellpadding="0" cellspacing="0" style="width:800px">
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                        <td style="width:60%; vertical-align:bottom" align=left>
                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:24px;"><img src="../images/toe.GIF" /></td>
                                    <td class="table_head_dark" style="width:40%;height:24px;">Account Inquiry</td>
                                    <td align="left" style="width:60%;height:24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                </tr>
                            </table>
                        </td>
                        </tr>
                        <tr>
                            <td> <!--colspan="3"> -->
                                <table  style="width:100%; border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                    <tr>
                                        <td colspan="5" class="table_caption" style="color: black; text-indent: 5px;height:26px;" align="left">
                                            <input id="chkOnline" type="checkbox" checked="checked" style="width: 20px" />
                                            ONLINE VALIDATION
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption" style="width:20%;height:20px;text-align:center;">
                                            <input id="rbBalanceInquiry" type="radio" name="Inquiry" style="width:20px;" checked="CHECKED" />
                                            Balance Inquiry
                                        </td>
                                        <td class="cell_caption" style="width:20%;height:20px;text-align:center;">
                                            <input id="rbLastTransactionInquiry" type="radio" name="Inquiry" style="width:20px;" />
                                            Last&nbsp;<input id="txtTransactionCount" type="text" value="5" style="width: 10%; text-align:right;" maxlength="2" onkeypress="javascript:return account_inquiry.isNumberKey(event);" />&nbsp;transaction(s)
                                        </td>
                                        <td class="cell_caption" style="width:20%;height:20px;text-align:center;">
                                            <input id="rbHoldInquiry" type="radio" name="Inquiry" style="width:20px;" />
                                            Hold Inquiry
                                        </td>
                                        <td class="cell_caption" style="width:20%;height:20px;text-align:center;">
                                            <input id="rbFloatInquiry" type="radio" name="Inquiry" style="width:20px;" />
                                            Float Inquiry
                                        </td>
                                        <td class="cell_caption" style="width:20%;height:20px;text-align:center;">
                                            <input id="rbAccountStatusInquiry" type="radio" name="Inquiry" style="width:20px;" />
                                            Account Status 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption" style="text-align:center;">
                                            Enter Account No.:</td>
                                        <td colspan="3" align="left">
                                            <input id="txtAccountNo" type="text" maxlength="50" onkeypress="javascript:return account_inquiry.isNumberKey(event);" onkeydown="if (event.keyCode == 13) document.getElementById('btnSubmit').click()" style="width: 98%" />
                                        </td>
                                        <td>
                                            <input id="btnSubmit" type="button" value="Submit" onclick="javascript:account_inquiry.inquireAccount();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div>
                    <table cellpadding="0" cellspacing="0" style="width:800px">
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width:60%; vertical-align:bottom" align=left>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="bottom" style="width:0%; border-collapse:collapse;height:24px;"><img src="../images/toe.GIF" /></td>
                                        <td class="table_head_dark" style="width:40%;height:24px;">
                                            GFC Inquiry</td>
                                        <td align="left" style="width:60%;height:24px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td> <!-- colspan="3"> -->
                                <table  style="width:100%; border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;border-bottom: #336699 3px solid">
                                    <tr>
                                        <td colspan="5" class="table_caption" style="color: black; text-indent: 5px;height:26px;" align="left">
                                            NOTE: This is for existing cards with CK links only.</td>
                                    </tr>
                                    <tr>
                                        <td class="cell_caption" style="text-align:center; width:20%;">
                                            Enter Card No.:</td>
                                        <td colspan="3" align="left" style="width:60%">
                                            <input id="txtCardNo" type="text" maxlength="16" onkeypress="javascript:return account_inquiry.isNumberKey(event);" onkeydown="if (event.keyCode == 13) document.getElementById('btnGFCtoCK').click()" style="width: 98%" />
                                        </td>
                                        <td style="width:20%">
                                            <input id="btnGFCtoCK" type="button" value="Submit" onclick="javascript:account_inquiry.inquireCard();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                
                <div id="divPopProcess" style="width:400px;height:200px;background:White;z-index:101;position:absolute;left:572px;color:darkblue;padding:10px;display:none;font-family:Arial;top:360px;">
                    <table style="width:100%; height:100%;">
                        <tr>
                            <td>
                                <label id="lblLoadingMsg" style="text-align:center;color:Chocolate"></label>
                                <div id="divLoadingMsg"></div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        
        
    </table>
    </form>
</body>
</html>