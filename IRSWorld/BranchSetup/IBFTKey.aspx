<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IBFTKey.aspx.cs" Inherits="IRSWorld.BranchSetup.IBFTKey" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IBFT Key Setup</title>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/BranchSetup/jsIBFTKey.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ibft_key = new ibft_key();
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <center>
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
                                    IBFT Key Setup</td>
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
                                    </td>
                            </tr>
                            <tr>
                                <td class="cell_caption" style="text-align:center; width:20%;">Business Key</td>
                                <td align="left" style="width:29%">
                                    <input id="txtBusiness" type="password" maxlength="8" style="width: 98%" onkeypress="javascript:return ibft_key.isNumberKey(event);" />
                                </td>
                                <td style="width:2%">&nbsp;</td>
                                <td class="cell_caption" style="text-align:center; width:20%;">
                                    Security Key</td>
                                <td align="left" style="width:29%">
                                    <input id="txtSecurity" type="password" maxlength="8" style="width: 98%" onkeypress="javascript:return ibft_key.isNumberKey(event);" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="5" align="right">
                                    <input id="btnSubmit" type="button" value="Submit" style="width:28%;" onclick="javascript:ibft_key.submitKey();" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPopProcess" style="height:200px;background:White;z-index:75;position:absolute;left:115px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:206px;">
            <table style="height:100%;width:350px;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center;color:Chocolate"></label>
                            <div id="divLoadingMsg">
                            </div>
                    </td>
                </tr>
            </table>
        </div>
    </center>
    </form>
</body>
</html>