<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Disclaimer.aspx.cs" Inherits="IRSWorld.BranchSetup.Disclaimer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Disclaimer</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />

    <script src="../js/jspad/jspad.js" type="text/javascript"></script>

    <script src="../js/jsCommons.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jshttprequest.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/BranchSetup/js_Disclaimer.js" type="text/javascript"></script>

    <script type="text/javascript">
            var new_disclaimer = new Disclaimer();
    </script>

</head>
<body onload="javascript:new_disclaimer.load();">
    <center>
        <form id="form1" runat="server">
            <div id="divBodyRegion" style="width: 98%; display: none; vertical-align: top; margin-top: 30px;">
                <table id="tblRegion" style="width: 45%; display: block">
                    <tr>
                        <td id="td3" style="width: 100%; border-collapse: collapse; vertical-align: middle;">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="vertical-align: bottom">
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" style="width: 1%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="tdRegionPageTitle" class="table_head_dark" style="width: 50%; height: 24px">
                                                    BRANCH</td>
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
                                        <table cellpadding="0" cellspacing="5" style="width: 100%;">
                                            <tr>
                                                <td class="cell_caption" style="width: 35%; height: 23px">
                                                    BY BRANCH :&nbsp;</td>
                                                <td align="left" style="width: 65%; height: 23px;">
                                                    <select id="ddlRegion" style="width: 100%">
                                                    </select>
                                                </td>
                                        </table>
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td style="width: 100%; height: 51px;" align="center">
                                                    <table id="tbl_button" cellpadding="0" cellspacing="5">
                                                        <tr>
                                                            <td style="width: 20%; vertical-align: middle" align="center">
                                                                <input id="btn_Search" class="button" onclick="javascript:new_disclaimer.check_region();"
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
            <div id="divBodyDisclaimer" style="width: 98%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblDisclaimer" style="width: 100%; display: block">
                    <tr>
                        <td id="tdDisclaimer" style="width: 98%; border-collapse: collapse;
                            vertical-align: middle;">
                            <div id="divDisclaimerDefault" style="width: 100%;">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="vertical-align: bottom">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 0%; height: 24px; border-collapse: collapse; vertical-align: bottom">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="tdPageTitle" class="table_head_dark" style="width: 30%; height: 24px">
                                                        DISCLAIMER</td>
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
                                            <div id="divDisclaimer" style="width: 98%; display: block">
                                                <table id="tblDisclaimerDefault" cellpadding="0" cellspacing="5" style="width: 95%">
                                                    <tr>
                                                        <td id="txtDisclaimer" style="width: 100%;">
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnEdit" class="button" onclick="javascript:new_disclaimer.update_record();"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Edit" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSave" class="button" onclick="javascript:new_disclaimer.commit_record();"
                                                                            type="button" value="Save" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCancel" class="button" onclick="javascript:new_disclaimer.load();"
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
        </form>
    </center>
</body>
</html>
