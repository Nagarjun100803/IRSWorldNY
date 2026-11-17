<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewOtherChannels.aspx.cs" Inherits="IRSWorld.IRSFileLoader.OtherChannelsView" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>My First Web Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css"/>
    <style type="text/css"></style>
    <script src="../js/FileLoader/js_viewotherchannels.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var fLoad = new js_viewotherchannels();
        
//        function load_main()
//        {
//            if (top==self)
//            {
//                document.viewotherchannels.submit();
//            }
//        }

    </script>
</head>
<body onload="javascript:fLoad.page_load()">
    <center>
        <form id="form1" runat="server">
            <table style="width:100%; height:auto" cellpadding="6" cellspacing="6">
                <tr>
                    <td style="width:20%; height:auto; vertical-align:middle;" >
                        <table style="width:100%; height:auto">
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">View Records</a></td></tr>
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">Other Channels Setup</a></td></tr>
                        </table>
                    </td>
                    <td style="width:80%; height:auto; vertical-align:middle" align="left">
                        <div id="divFileLoader" style="display:none; height:auto; width:350px">
                            <table cellpadding="0" cellspacing="0" style="width:100%;height:auto">
                                <tr>
                                    <td align="left" valign="bottom" style="width:1%; height:24px">
                                        <img src="../images/toe.gif" /></td>
                                    <td align="left" class="table_head_dark" style="width:40%; height:24px; text-align:left; text-indent:5px">
                                        Other Channels</td>
                                    <td align="left" valign="bottom" style="height:24px" class="bg_body">
                                        <img align="absbottom" src="../images/rend.gif" />
                                    </td> 
                                </tr>
                                <tr>                   
                                    <td colspan="4" align="center" style="height:auto; width:416px">
                                        <div style="height:auto; width:auto; border:solid 4px #336699; background:Azure;">
                                        <div id="divSearch" style="display:block; background:Azure">
                                            <div><table style="width:100%; text-indent:5px"><tr><td class="table_caption" align="left" style="color:Black">SEARCH UPLOADED FILES - Records</td></tr></table></div>
                                            <br />
                                            <div style="width:95%; border:solid 1px #336699">
                                                <table style="width:100%; height:auto; background:Azure" cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td style="height:18px; width:27%" class="cell_caption">File Format:&nbsp;</td>
                                                        <td align="left" colspan="4"><select id="ddFileFormat" style="width:100%"></select></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width:27%" class="cell_caption">Date from:&nbsp;</td>
                                                        <td style="width:33%"><input type="text" style="width:95%;" /></td>
                                                        <td style="width:6%" class="cell_caption">
                                                            to:&nbsp;</td>
                                                        <td style="width:33%"><input type="text" style="width:95%;" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="5"><input id="btnSearch" value="Search" class="button" type="button" onclick="javascript:fLoad.panel_search_otherchannels()"/></td>
                                                    </tr>
                                                </table>
                                            </div><input id="txtHiddenFileFormat" type="hidden" style="width:5%" />
                                        </div>
                                        <br />                            
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="width:auto; height:auto;">
                            <table cellpadding="0" cellspacing="0" style="width:90%; height:auto">
                                <tr>
                                    <td colspan="4" style="width: 695px; height: auto;" align="center">
                                        <div id="divUploadedRecords" style="display:none; background:Azure">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width:1%; height:24px">
                                                        <img src="../images/toe.gif" /></td>
                                                    <td id="lblFolderName" align="left" class="table_head_dark" width="28%" style="height: 24px; text-align:left">View Details&nbsp;<label id="lblRecordStatus" style="width:100%; height:auto"></label></td>
                                                    <td align="left" valign="bottom" style="height:24px" class="bg_body">
                                                        <img align="absbottom" src="../images/rend.gif" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width:100%; height:auto">
                                                        <table style="border:solid 4px #336699; width:100%; height:auto" cellpadding="2" cellspacing="2">
                                                            <tr><td class="table_caption" style="color:Black; text-indent:5px" align="left" colspan="2"><label id="lblViewRecordHead" style="color:Black">&nbsp;</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: auto;" colspan="2">
                                                                <br />
                                                                    <div style="border: solid 1px #336699; width:90%" class="div">
                                                                        <table style="width:100%; height:auto; TEXT-ALIGN: center; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px; background:Azure" cellpadding="4" cellspacing="4" >
                                                                            <tr>
                                                                                <td style="height:18px; width:14%;" class="cell_caption">File Format:&nbsp;</td>
                                                                                <td style="height:18px; width:22%; background-color:Azure; border:solid 1px midnightblue" ><label id="lblFileFormat" style="width:100%; height:auto;">FEDACH</label></td>
                                                                                <td style="height: 18px; width: 13%;" class="cell_caption">Date Uploaded:&nbsp;</td>
                                                                                <td style="height: 18px; width:13%; background-color:Azure; border:solid 1px midnightblue" ><label id="lblDateUploaded" style="width:100%; height:auto;">&nbsp;MMDDYYYY</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height:18px; width:14%;" class="cell_caption">Reference No.:&nbsp;</td>
                                                                                <td style="height:18px; width:22%; background-color:Azure; border:solid 1px midnightblue" ><label id="lblRefNo" style="width:100%; height:auto;">1FED121509000001</label></td>
                                                                                <td style="height: 18px; width: 13%; " class="cell_caption">Batch ID:&nbsp;</td>
                                                                                <td style="height: 18px; width: 25%; background-color:Azure; border:solid 1px midnightblue" ><label id="lblBatchID" style="width:100%; height:auto;" >&nbsp;FedACHTest_20090803.ach</label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                <br />
                                                                    <div style="border: solid 1px #336699; width:90%" class="div">
                                                                        <table style="width:100%; height:auto; background:Azure" cellpadding="4" cellspacing="4">
                                                                            <tr>
                                                                                <td style="height: 18px; width: 18%;" class="cell_caption">Remitter:&nbsp;</td>
                                                                                <td style="height: 18px; width:82%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblRemitName" style="width:100%; height:auto">&nbsp;John Mark H. Jacob</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 18px; width: 18%;" class="cell_caption">Address:&nbsp;</td>
                                                                                <td style="height: 18px; width:82%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblRemitAddress" style="width:100%; height:auto">&nbsp;149 Philand Extension, Pasong Tamo, Tandang Sora, Quezon City, Phils.</label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                <br />
                                                                    <div style="border: solid 1px #336699; width:90%" class="div">
                                                                        <table style="width:100%; height:auto; background:Azure" cellpadding="0" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <table style="width:100%; height:auto" cellpadding="4" cellspacing="4">
                                                                                        <tr>
                                                                                            <td align="left" style="height: 18px; width: 15%;" class="cell_caption">Beneficiary:&nbsp;</td>
                                                                                            <td align="left" style="height: 18px; width:30%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblBenefName" style="width:100%; height:auto">&nbsp;Nelhya B. Borja</label></td>
                                                                                            <td style="width:18%; height:18px" class="cell_caption" align="right">Trace Number:</td>
                                                                                            <td style="width:30%; height:18px; border:solid 1px midnightblue" class="data_caption_left" ><label id="lblTraceNo" style="width:100%; height:auto">&nbsp;0260030014652</label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" style="height: 18px; width: 15%;" class="cell_caption">Address:&nbsp;</td>
                                                                                            <td colspan="5" align="left" style="height: 18px; width:82%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblBenefAddress" style="width:100%; height:auto">&nbsp;NPA</label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table style="width:100%; height:auto" cellpadding="4" cellspacing="4">
                                                                                        <tr>
                                                                                            <td align="left" style="height:18px; width:15%" class="cell_caption">Ca / Sa No.:&nbsp;</td>
                                                                                            <td align="left" style="height:18px; width:27%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblBranchAcct" style="width:100%; height:auto">&nbsp;0-2-10-2-22-0</label></td>
                                                                                            <td align="left" style="height:18px; width:20%" class="cell_caption">Branch of Account:&nbsp;</td>
                                                                                            <td align="left" style="height:18px; width:30%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblCasaNo" style="width:100%; height:auto">&nbsp;Main - PNB Main</label></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" style="height:18px; width:15%" class="cell_caption">Account Type:&nbsp;</td>
                                                                                            <td align="left" style="height:18px; width:20%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblAcctType" style="width:100%; height:auto">&nbsp;CA</label></td>
                                                                                            <td align="left" style="height:18px; width:20%" class="cell_caption">A/C Currency:&nbsp;</td>
                                                                                            <td align="left" style="height:18px; width:30%; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblAcctCcy" style="width:100%; height:auto">&nbsp;USD</label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                <br />
                                                                    <div style="border: solid 1px #336699; width:90%" class="div">
                                                                        <table style="width:100%; height:auto; background:Azure" cellpadding="4" cellspacing="4">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 24%; height: 18px">Date Of Implementation:&nbsp;</td>
                                                                                <td align="right" style="width: 20%; height: 18px; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left"><label id="lblImplemDate" style="width:100%; height:auto">&nbsp;MMddyyyy</label></td>
                                                                                <td class="cell_caption" style="width: 10%; height: 18px"> Amount :&nbsp;</td>
                                                                                <td align="right" style="width: 29%; height: 18px; background-color:Azure; border:solid 1px midnightblue" class="data_caption_right"><label id="lblAmount" style="width:100%; height:auto">&nbsp;20 000.00</label></td>
                                                                            </tr>
                                                                            <%--<tr>
                                                                                <td class="cell_caption" style="width: 30%; height: 18px">Net Remit Amount (Php Eqv) :&nbsp;</td>
                                                                                <td align="right" style="width: 20%; height: 18px; background-color:Azure; border:solid 1px midnightblue" class="data_caption_right"><label id="lblNetRmtAmtPhp" style="width:100%; height:auto">&nbsp;1 000 000.00</label></td>
                                                                                <td class="cell_caption" style="width: 25%; height: 18px">Net to Beneficary (A/C Ccy) :&nbsp;</td>
                                                                                <td align="right" style="width: 25%; height: 18px; background-color:Azure; border:solid 1px midnightblue" class="data_caption_right"><label id="lblNetBenAmtAcctCcy" style="width:100%; height:auto">&nbsp;20 000.00</label></td>
                                                                            </tr>--%>                                                
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 22%; height: 18px">Special Message :&nbsp;</td>
                                                                                <td align="right" style="width: 70%; height: 18px; background-color:Azure; border:solid 1px midnightblue" class="data_caption_left" colspan="4"><label id="lblSpecialMsg" style="width:100%; height:auto; font-size:x-small"></label></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                <br />
                                                                    <!-- content ends here -->
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="1" style="width:53%" align="right"><a href="" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial"><< PREVIOUS</a></td>
                                                                <td colspan="1" style="width:47%" align="left"><a href="" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">NEXT >></a></td>
                                                            </tr>
                                                        </table>                                                 
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                        <div id="divCommit" style="display:none">
                                            <table style="width:100%" cellspacing="2" cellpadding="2" border="2">
                                                <tr>
                                                    <td style="height:auto"><input type="button" id="btnCommit" onclick="javascript:fLoad.commit_click()" class="button" value="Commit" style="height:27px; width:100px"/></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: auto; width: 695px;">
                                        <div  id="divGrid" style="display:none; background:Azure; border:solid 3px #336699">
                                            <table style="width:100%" cellspacing="2" cellpadding="2">
                                                <tr><td align="left" colspan="3" style="width:100%; height:20px" class="table_caption">&nbsp;<label id=lblRecordList style="width:auto; height:auto; color:Black"></label></td></tr>                                                
                                                <tr>
                                                    <td style="height:auto; width:100%" colspan="3">
                                                        <div id="dgRecords">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:58%; height:21px" align="right"><input style="width:75px; height:24px" type="button" class ="button" id="btnProcessFile" /></td>
                                                    <td style="width:27%; height:21px" align="center" class="cell_caption" >SELECT BY RECORD STATUS:&nbsp;</td>
                                                    <td style="width:15%; height:21px;" align="right">
                                                        <select id="ddRecordStatus"  style="width:auto" onchange="javascript:fLoad.status_changed();"></select>
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
        </form>
    </center>
    <form name="viewotherchannels" id="viewotherchannels" method="post" action="../Main/IRSMain.aspx?load=../IRSFileLoader/OtherChannelsView.aspx" target="_parent"></form>
</body>
</html>
