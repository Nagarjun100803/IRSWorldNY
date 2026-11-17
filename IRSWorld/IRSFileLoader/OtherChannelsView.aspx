<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherChannelsView.aspx.cs" Inherits="IRSWorld.IRSFileLoader.OtherChannelsView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>My First Web Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css"/>
    <style type="text/css"></style>
    
    <script src="../js/FileLoader/js_viewotherchannels.js" type="text/javascript"></script>
    <%--<script type="text/javascript">
    
        var fLoad = new js_viewotherchannels();
        
        function load_main()
        {
            if (top==self)
            {
                document.viewotherchannels.submit();
            }
        }
        
    </script>--%>
</head>
<body onload="javascript:load_main();">
    <center>
        <form id="form1" runat="server">
            <div style="overflow:auto">
                <table width="100%" cellpadding="0" cellspacing="5" height="100%">
                    <tr>
                        <td align="center" valign="top" style="width:100%; border-collapse:collapse; height:150;" class="content_height" colspan="2">
                            <table style="width:100%; height:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" style="width: 942px" align="center">
                                        <table cellpadding="0" cellspacing="0" style="width:90%; height:18px">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%; height:24px">
                                                    <img src="../images/toe.gif" /></td>
                                                <td align="left" class="table_head_dark" width="25%" style="height: 24px; text-align:left">View Uploaded Records</td>
                                                <td align="left" valign="bottom" style="height:24px" class="bg_body">
                                                    <img align="absbottom" src="../images/rend.gif" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width:100%; height:100%">
                                                    <table style="border:solid 3px #336699; background:Azure; width:100%; height:100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="height: 104px">
                                                                <table style="width:80%; height:100%" cellpadding="2" cellspacing="2">
                                                                    <tr>
                                                                        <td style="width: 760px">
                                                                            <!-- content starts here -->
                                                                            <br />
                                                                            <div style="border: solid 1px #336699;" class="div">
                                                                                <table style="width:100%; height:100%" cellpadding="4" cellspacing="4">
                                                                                    <tr>
                                                                                        <td style="height:18px; width:13%;" class="cell_caption">File Format:</td>
                                                                                        <td width="20%" style="height:18px" ></td>
                                                                                        <td style="height: 18px; width: 10%;" class="cell_caption">Batch ID:</td>
                                                                                        <td width="20%" style="height: 18px"></td>
                                                                                        <td style="height: 18px; width: 16%;" class="cell_caption">Date Uploaded:</td>
                                                                                        <td width="20%" style="height: 18px"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br />
                                                                            <div style="border: solid 1px #336699;" class="div">
                                                                                <table style="width:100%; height:100%" cellpadding="4" cellspacing="4">
                                                                                    <tr>
                                                                                        <td style="height: 18px; width: 20%;" class="cell_caption">Remitter Name:&nbsp;</td>
                                                                                        <td style="height: 18px; width:80%" ></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 18px; width: 20%;" class="cell_caption">Address:&nbsp;</td>
                                                                                        <td style="height: 18px; width:80%" ></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br />
                                                                            <div style="border: solid 1px #336699;" class="div">
                                                                                <table style="width:100%; height:100%" cellpadding="4" cellspacing="4">
                                                                                    <tr>
                                                                                        <td align="left" style="height: 18px; width: 20%;" class="cell_caption">Beneficiary Name:&nbsp;</td>
                                                                                        <td colspan="3" align="left" style="height: 18px; width:20%" ></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="height: 18px; width: 20%;" class="cell_caption">Address:&nbsp;</td>
                                                                                        <td colspan="3" align="left" style="height: 18px; width:20%" ></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="height:18px; width:20%" class="cell_caption">
                                                                                            Branch of Account:&nbsp;</td>
                                                                                        <td align="left" style="height:18px; width:30%"></td>
                                                                                        <td align="left" style="height:18px; width:20%" class="cell_caption">
                                                                                            Ca / Sa No.:&nbsp;</td>
                                                                                        <td align="left" style="height:18px; width:30%"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" style="height:18px; width:20%" class="cell_caption">
                                                                                            Account Type:&nbsp;</td>
                                                                                        <td align="left" style="height:18px; width:30%"></td>
                                                                                        <td align="left" style="height:18px; width:20%" class="cell_caption">
                                                                                            A/C Currency:&nbsp;</td>
                                                                                        <td align="left" style="height:18px; width:30%"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br />
                                                                            <div style="border: solid 1px #336699; width:100%" class="div">
                                                                                <table style="width:100%; height:100%" cellpadding="4" cellspacing="4">
                                                                                    <tr>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">Reference Number:</td>
                                                                                        <td width="25%" style="height: 18px; text-align:left" ></td>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">Remitting Currency:</td>
                                                                                        <td width="25%" style="height: 18px; text-align:left"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">
                                                                                            Covering Bank:</td>
                                                                                        <td width="25%" style="height: 18px; text-align:left" ></td>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">Local Currency:</td>
                                                                                        <td width="25%" style="height: 18px; text-align:left"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">Implementation Date:</td>
                                                                                        <td width="25%" style="height: 18px" ></td>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">
                                                                                            EURO to USD:</td>
                                                                                        <td width="25%" style="height: 18px"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">
                                                                                            Status:</td>
                                                                                        <td width="25%" style="height: 18px" ></td>
                                                                                        <td style="height: 18px; width: 25%;" class="cell_caption">
                                                                                            USD to PHP:</td>
                                                                                        <td width="25%" style="height: 18px"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br />
                                                                            <div style="border: solid 1px #336699;" class="div">
                                                                                <table style="width:100%; height:100%" cellpadding="4" cellspacing="4">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 18px">Total Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 18px;"></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 18px">Net Remit Amount (Rmt Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 18px;"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 30%; height: 18px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 18px;"></td>
                                                                                        <td class="cell_caption" style="width: 30%; height: 18px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                        <td align="right" style="width: 20%; height: 18px;"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            <br />
                                                                            <!-- content ends here -->
                                                                        </td>
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
                        </td>
                    </tr>
                </table>
            </div>
        </form>
    </center>
    <form name="viewotherchannels" id="viewotherchannels" method="post" action="../Main/IRSMain.aspx?load=../IRSFileLoader/OtherChannelsView.aspx" target="_parent" style="overflow:auto"></form>
</body>
</html>
