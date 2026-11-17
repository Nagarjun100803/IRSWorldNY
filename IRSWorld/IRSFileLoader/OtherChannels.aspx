<%@Page Language="C#" AutoEventWireup="true" CodeBehind="OtherChannels.aspx.cs" Inherits="IRSWorld.IRSFileLoader.OtherChannels" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow:scroll">
<head id="Head1" runat="server">
    <title>IRS WORLD - OTHERCHANNELS</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css"/>
    <style type="text/css"></style>
    <script src="../js/FileLoader/js_otherchannels.js" type="text/javascript"></script>
    <script src="../js/FileLoader/js_otherchannelsdefaults.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var vw = new js_otherchannels();
        var def = new js_otherchannels_defaults();
        
        function showpanel(divname)
        {
            
            var div = document.getElementById(divname);
            
            divmask = new maskpanel();
            divmask.show();
            
            document.getElementById(divname).style.display = "block";
            
            div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
            div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
        }

        function hidepanel(divname)
        {
            document.getElementById(divname).style.display = "none";
            divmask.hide();
        }
        
    </script>
</head>
<body onload="javascript:page_load()">
    <center>
        <form id="form1" runat="server">
            <div id="divPopProcess" style="width:400px; height:200px; background:White; z-index: 101; position: absolute; left: 276px;
                color: darkblue; padding: 10px; display: none; font-family: Arial;
                top: 148px;">
                <table style="width:100%; height:100%;">
                    <tr>
                        <td>
                            <label style="text-align:center; color:Chocolate ">Please wait...</label>
                            <div id="divLoadingMsg">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divView" style="width:850px; height:auto; background:White; z-index: 101; position: absolute; left: 44px;
                color: darkblue; padding: 10px; display: none; font-family: Arial; top: 389px;" align="center">
                <table style="width:100%; height:auto; background:White; overflow:auto" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="left" valign="bottom" style="width:0%; height:24px; background:Gray">
                            <img src="../images/toe.gif" /></td>
                        <td align="left" class="table_head_dark" style="width:30%; height: 24px; text-align:left"><span id="spnTranxDetails">Transaction Details - Mizuho</span></td>
                        <td align="left" valign="bottom" style="height:24px; width:70%; background:White" class="bg_body">
                            <img align="absbottom" src="../images/rend.gif" />
                        </td>
                    </tr>
                    <tr>
                        <td style="border:solid 3px #336699; width:100%; height:auto" colspan="3">
                        <div id="divTranxLoading"></div>
                        <div id="divTranxDetails">
                            <table style="width:100%; height:100%" cellspacing="5" cellpadding="2">
                                <tr>
                                    <td style="width:50%;" colspan="2">
                                        <table style="border:solid 1px #115599; width:50%; height:100%" cellspacing="4" cellpadding="1">
                                            <tr>
                                                <td colspan="4" class="table_caption" style="text-align:center">Money Matters
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td width="25%">
                                                </td>--%>
                                                <td style="width:50%" class="cell_caption">Amount (Remit Ccy) :
                                                </td>
                                                <td style="text-align:right; border:solid 1px Gray"><span id="spnAmountRemitCcy" class="label2">21,475.35</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td width="25%">
                                                </td>--%>
                                                <td style="width:50%" class="cell_caption">Actual Amount (PHP) :
                                                </td>
                                                <td style="text-align:right; border:solid 1px Gray"><span id="spnActualAmountPHP" class="label2">1,000,000.00</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td width="25%">
                                                </td>--%>
                                                <td style="width:50%" class="cell_caption">Total Amount (PHP) :
                                                </td>
                                                <td style="text-align:right; border:solid 1px Gray"><span id="spnTotalAmountPHP" class="label2">1,000,000.00</span>
                                                </td>
                                                <%--<td width="25%">
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:50%; border:solid 1px #115599" colspan="1">
                                        <table style="width:100%; height:100%" cellspacing="4" cellpadding="1">
                                            <tr>
                                                <td colspan="3" class="table_caption" style="text-align:center">Remitter Info
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:35%" class="cell_caption">Remitter Name :
                                                </td>
                                                <td style="text-align:left; border:solid 1px Gray"><span id="spnRemitterName" class="cell_desc">Dencio M. Plorencio</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:35%" class="cell_caption">Remitter Address :
                                                </td>
                                                <td style="text-align:left; border:solid 1px Gray"><span id="spnRemitterAddress" class="cell_desc">Ortigas Extension, Pasig City</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td style="text-align:left; border:solid 1px Gray"><span id="spnRemitterAddress2" class="cell_desc">Philippines</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:35%" class="cell_caption">Remitting Currency :
                                                </td>
                                                <td style="text-align:left; border:solid 1px Gray"><span id="spnRemitCurrency" class="cell_desc">USD</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width:50%; height:100%; border:solid 1px #115599" colspan="1">
                                        <table style="width:100%; height:100%" cellspacing="4" cellpadding="1">
                                            <tr>
                                                <td colspan="3" class="table_caption" style="text-align:center">Beneficiary Info
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:35%" class="cell_caption">Beneficiary Name :
                                                </td>
                                                <td style="width:auto; text-align:left; border:solid 1px Gray" colspan="2"><span id="spnBenefName" class="cell_desc">Dencio M. Plorencio</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:35%" class="cell_caption">Beneficiary Address :
                                                </td>
                                                <td style="width:auto; text-align:left; border:solid 1px Gray" colspan="2"><span id="spnBenefAddress" class="cell_desc">Ortigas Extension, Pasig City</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td style="width:auto; text-align:left; border:solid 1px Gray" colspan="2"><span id="spnBenefAddress2" class="cell_desc">Philippines</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width:35%" class="cell_caption">Account Currency :
                                                </td>
                                                <td style="width:auto; text-align:left; border:solid 1px Gray" colspan="2"><span id="spnAcctCur" class="cell_desc">PHP</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="4"><br /><input value="Close" style="width:75px; height:25px;" type="button" class="button" id="idbtnCloseView" onclick="javascript:vw.close_view()" /></td>
                    </tr>
                </table>
            </div>
            <%--<div id="divView" style="width:900px; height:450px; background:White; z-index: 101; position: absolute; left: 4px;
                color: darkblue; padding: 10px; display: none; font-family: Arial;
                top: 543px; overflow-y:scroll">
                <table style="width:100%; height:auto" cellpadding="6" cellspacing="6">
                <tr><td class="table_caption" style="color:Black; text-indent:5px; width: 692px;" align="left" colspan="2"><label id="lblViewRecordHead" style="color:Black">&nbsp;OTHER CHANNELS - VIEW DETAILS</label></td>
                </tr>
                <tr>
                    <td style="height: auto; width: 692px;" colspan="2">
                    <br />
                        <div style="border: solid 1px #336699; width:100%" class="div">
                            <table style="width:100%; height:auto; TEXT-ALIGN: center; color: MidnightBlue; font-family:Arial; font-weight: bold; font-size: 12px; background:White" cellpadding="4" cellspacing="4" >
                                <tr>
                                    <td style="height:18px; width:14%;" class="cell_caption">File Format:&nbsp;</td>
                                    <td style="height:18px; width:22%; background-color:White; border:solid 1px midnightblue" ><label id="lblFileFormat" style="width:100%; height:auto;">FEDACH</label></td>
                                    <td style="height: 18px; width: 13%;" class="cell_caption">Date Uploaded:&nbsp;</td>
                                    <td style="height: 18px; width:13%; background-color:White; border:solid 1px midnightblue" ><label id="lblDateUploaded" style="width:100%; height:auto;">&nbsp;MMDDYYYY</label></td>
                                </tr>
                                <tr>
                                    <td style="height:18px; width:14%;" class="cell_caption">Reference No.:&nbsp;</td>
                                    <td style="height:18px; width:22%; background-color:White; border:solid 1px midnightblue" ><label id="lblRefNo" style="width:100%; height:auto;">1FED121509000001</label></td>
                                    <td style="height: 18px; width: 13%; " class="cell_caption">Batch ID:&nbsp;</td>
                                    <td style="height: 18px; width: 25%; background-color:White; border:solid 1px midnightblue" ><label id="lblBatchID" style="width:100%; height:auto;" >&nbsp;FedACHTest_20090803.ach</label></td>
                                </tr>
                            </table>
                        </div>
                    <br />
                        <div style="border: solid 1px #336699; width:100%" class="div">
                            <table style="width:100%; height:auto; background:White" cellpadding="4" cellspacing="4">
                                <tr>
                                    <td style="height: 18px; width: 18%;" class="cell_caption">Remitter:&nbsp;</td>
                                    <td style="height: 18px; width:82%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblRemitName" style="width:100%; height:auto">&nbsp;John Mark H. Jacob</label></td>
                                </tr>
                                <tr>
                                    <td style="height: 18px; width: 18%;" class="cell_caption">Address:&nbsp;</td>
                                    <td style="height: 18px; width:82%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblRemitAddress" style="width:100%; height:auto">&nbsp;149 Philand Extension, Pasong Tamo, Tandang Sora, Quezon City, Phils.</label></td>
                                </tr>
                            </table>
                        </div>
                    <br />
                        <div style="border: solid 1px #336699; width:100%" class="div">
                            <table style="width:100%; height:auto; background:White" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <table style="width:100%; height:auto" cellpadding="4" cellspacing="4">
                                            <tr>
                                                <td align="left" style="height: 18px; width: 15%;" class="cell_caption">Beneficiary:&nbsp;</td>
                                                <td align="left" style="height: 18px; width:30%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblBenefName" style="width:100%; height:auto">&nbsp;Nelhya B. Borja</label></td>
                                                <td style="width:18%; height:18px" class="cell_caption" align="right">Trace Number:</td>
                                                <td style="width:30%; height:18px; border:solid 1px midnightblue" class="data_caption_left" ><label id="lblTraceNo" style="width:100%; height:auto">&nbsp;0260030014652</label></td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height: 18px; width: 15%;" class="cell_caption">Address:&nbsp;</td>
                                                <td colspan="5" align="left" style="height: 18px; width:82%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblBenefAddress" style="width:100%; height:auto">&nbsp;NPA</label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table style="width:100%; height:auto" cellpadding="4" cellspacing="4">
                                            <tr>
                                                <td align="left" style="height:18px; width:15%" class="cell_caption">Ca / Sa No.:&nbsp;</td>
                                                <td align="left" style="height:18px; width:27%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblBranchAcct" style="width:100%; height:auto">&nbsp;0-2-10-2-22-0</label></td>
                                                <td align="left" style="height:18px; width:20%" class="cell_caption">Branch of Account:&nbsp;</td>
                                                <td align="left" style="height:18px; width:30%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblCasaNo" style="width:100%; height:auto">&nbsp;Main - PNB Main</label></td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height:18px; width:15%" class="cell_caption">Account Type:&nbsp;</td>
                                                <td align="left" style="height:18px; width:20%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblAcctType" style="width:100%; height:auto">&nbsp;CA</label></td>
                                                <td align="left" style="height:18px; width:20%" class="cell_caption">A/C Currency:&nbsp;</td>
                                                <td align="left" style="height:18px; width:30%; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblAcctCcy" style="width:100%; height:auto">&nbsp;USD</label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    <br />
                        <div style="border: solid 1px #336699; width:100%" class="div">
                            <table style="width:100%; height:auto; background:White" cellpadding="4" cellspacing="4">
                                <tr>
                                    <td class="cell_caption" style="width: 24%; height: 18px">Date Of Implementation:&nbsp;</td>
                                    <td align="right" style="width: 20%; height: 18px; background-color:White; border:solid 1px midnightblue" class="data_caption_left"><label id="lblImplemDate" style="width:100%; height:auto">&nbsp;MMddyyyy</label></td>
                                    <td class="cell_caption" style="width: 10%; height: 18px"> Amount :&nbsp;</td>
                                    <td align="right" style="width: 29%; height: 18px; background-color:White; border:solid 1px midnightblue" class="data_caption_right"><label id="lblAmount" style="width:100%; height:auto">&nbsp;20 000.00</label></td>
                                </tr>                                               
                                <tr>
                                    <td class="cell_caption" style="width: 22%; height: 18px">Special Message :&nbsp;</td>
                                    <td align="right" style="width: 70%; height: 18px; background-color:White; border:solid 1px midnightblue" class="data_caption_left" colspan="4"><label id="lblSpecialMsg" style="width:100%; height:auto; font-size:x-small"></label></td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right"><input value="Close" style="width:75px; height:25px;" type="button" class="button" id="idbtnCloseView" onclick="javascript:vw.close_view()" /></td>
                </tr>
            </table>
            </div>--%>
            <table style="width:auto; height:auto" cellpadding="2" cellspacing="2">
                <tr>
                    <td id="tdOtherChannelsMenu" style="width:auto; height:auto; vertical-align:top;" >
                        <div id="divOtherChannelsMenu">
                        </div>
                        <a id="btnAnchor" href="#" onclick="sidebar.get_side_bar_visibility('divOtherChannelsMenu', 'btnAnchor')"
                        style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial"> <<</a>
                    </td>
                    <td style="width:959px; height:auto; border-left:solid 1px Gray" valign="top" align="center">
                        <div style="width:100%; height:440px;">
                            <table cellpadding="1" cellspacing="1" style="width:100%; height:100%">
                                <tr>
                                    <td colspan="4" style="width: 99%; height: 100%;" align="left" valign="top">
                                        <div id="divOtherChannels" style="width:auto; height:100%;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width:1%; height:24px">
                                                        <img src="../images/toe.gif" /></td>
                                                    <td id="lblFolderName" align="left" class="table_head_dark" style="width:29%; height: 24px; text-align:left">&nbsp;<label id="lblOtherChannels" style="width:100%; height:auto"></label></td>
                                                    <td align="left" valign="bottom" style="height:24px; width: 70%;" class="bg_body">
                                                        <img align="absbottom" src="../images/rend.gif" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width:auto; height:400px; vertical-align:top" align="left">
                                                        <div id="divMAIN" style="border:solid 4px #336699; width:97%; height:400px;" align="center">
                                                            <table style="height:97%; width:100%">
                                                            <tr><td style="width:100%; height:100%; vertical-align:top">
                                                            <br />
                                                            <div id="divSearch" style="width:350px; height:auto;">
                                                            <div style="height:auto; width:100%; border:solid 3px midnightblue;" ><div><table style="width:100%; text-indent:5px"><tr><td class="table_caption" align="left" >SEARCH UPLOADED FILES - Records</td></tr></table></div>
                                                                <div style="width:95%; border:solid 1px #336699; background:Azure;">
                                                                    <table style="width:95%; height:auto;" cellpadding="2" cellspacing="2">
                                                                        <tr>
                                                                            <td style="height:25px; width:27%" class="cell_caption">File Format:&nbsp;</td>
                                                                            <td align="left" colspan="4" style="height: 25px"><select id="ddFileFormat" style="width:100%"></select></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="height:18px; width:27%" class="cell_caption">File Status:&nbsp;</td>
                                                                            <td align="left" colspan="4" style="height: 25px"><select id="ddFileStatus" style="width:100%"></select></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width:27%" class="cell_caption">Date from:&nbsp;</td>
                                                                            <td style="width:33%"><input type="text" style="width:95%;" id="txtDateFrom" /></td>
                                                                            <td style="width:6%" class="cell_caption">
                                                                                to:&nbsp;</td>
                                                                            <td style="width:33%"><input type="text" style="width:95%;" id="txtDateTo" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" colspan="5"><input id="btnSearch" value="Search" class="button" type="button" onclick="javascript:vw.panel_search_otherchannels()"/></td>
                                                                        </tr>
                                                                    </table>
                                                                <input id="txtHiddenFileFormat" type="hidden" style="width:5%" />
                                                                </div>
                                                            </div><br /></div>
                                                            <div style="width:98%;">
                                                                <%--<div id="divShowDefaults">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td id="tdShowDefaults" align="right"; style="font-weight:bolder">
                                                                                <a style="width:20%" id="lnkShowDefaults" href="#" onclick="javascript:def.show_defaults()">Show Defaults</a>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>--%>
                                                                <div id="divOtherChannelsDefaults" style="width:100%; height:auto; border:solid 3px midnightblue" >
                                                                <table style="width:100%; height:auto" cellpadding="6" cellspacing="6">
                                                                    <tr>
                                                                        <td align="left" colspan="4" style="width:100%; height:20px" class="table_caption">OTHER CHANNELS - DEFAULTS</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <div id="divDefaultsValue" style="border: solid 1px #336699; width:100%">
                                                                                <table style="width:100%; height:auto" cellpadding="4" cellspacing="4">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width:25%; text-align:center">FILE FORMAT</td>
                                                                                        <td class="data_caption_left" style="width:25%"><select onchange="javascript:def.ddFFDefaults_onchange()" id="ddFFDefaults" style="width:100%"></select></td>
                                                                                        <td style="width:45%; text-align:right"><a id="lnkPrevious" href="#" onclick="javascript:def.lnkPrevious_click()"><<</a></td>
                                                                                        <td style="width:5%; text-align:right"><a id="lnkNext" href="#" onclick="javascript:def.lnkNext_click()">>></a></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width:20%; text-align:center">REMIT CURRENCY</td>
                                                                                        <td class="data_caption_left" style="width:20%"><select onchange="javascript:def.remit_cur_select()" id="ddRemitCurrency" style="width:100%"></select></td>
                                                                                    </tr>
                                                                                </table>
                                                                                <br />
                                                                                <table style="width:100%; height:auto" cellpadding="4" cellspacing="4">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width:20%; text-align:center">TRANSACTION STATUS</td>
                                                                                        <td class="cell_caption" style="width:20%; text-align:center">
                                                                                            TRANSACTION SOURCE</td>
                                                                                        <td class="cell_caption" style="width:20%; text-align:center">RECEIPT MODE</td>
                                                                                        <td class="cell_caption" style="width:20%; text-align:center">TRANSACTION PURPOSE</td>
                                                                                        <td class="cell_caption" style="width:20%; text-align:center">SOURCE OF FUNDS</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td id="tdTranxStat" class="data_caption_left" style="width:20%; text-align:center"><label id="lblTranxStat">NEW TRANSACTION</label></td>
                                                                                        <td id="tdTranxSource" class="data_caption_left" style="width:20%; text-align:center"><label id="lblTranxSource">CASH</label></td>
                                                                                        <td id="tdReceiptMode" class="data_caption_left" style="width:20%; text-align:center"><label id="lblReceiptMode">MAIL</label></td>
                                                                                        <td id="tdTranxPurpose" class="data_caption_left" style="width:20%; text-align:center"><label id="lblTranxPurpose">FAMILY SUPPORT</label></td>
                                                                                        <td id="tdSourceFunds" class="data_caption_left" style="width:20%; text-align:center"><label id="lblSourceFunds">SALARY</label></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                        <table style="width:100%; height:auto" cellpadding="6" cellspacing="6">
                                                                        <tr><td colspan="4">
                                                                            <div id="dgSelectDefaults"><a id="lnkProdCharg" href="javascript:def.search_prod_charge()" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">SELECT PRODUCT CHARGES</a><br /></div>
                                                                            <div id="dgDefaults"></div>
                                                                        </td></tr>
                                                                        <tr><td><div id="divDefaultsControl">
                                                                        <table style="width:100%; height:auto"><tr>
                                                                            <td id="tdPostSave" style="width:30%" align="right"><input onclick="javascript:def.btnPost_click()" id="btnPostSave" type="button" value="POST" class="button" style="width:66%"/></td>
                                                                            <td style="width:20%"><input onclick="javascript:def.btnAdd_click()" id="btnAdd" type="button" value="ADD" class="button" style="width:100%" /></td>
                                                                            <td style="width:20%"><input onclick="javascript:def.btnEdit_click()" id="btnEdit" type="button" value="EDIT" class="button" style="width:100%" /></td>
                                                                            <td style="width:30%" align="left"><input onclick="javascript:def.btnCancel_click()" id="btnCancelDelete" value="DELETE" type="button" class="button" style="width:66%" /></td>
                                                                        </tr></table>
                                                                        </div></td></tr>
                                                                        </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                </div><br />
                                                                </div>
                                                                <div id="divGrid" style="width:98%; border:solid 3px midnightblue" align="center">
                                                                    <table style="width:100%" cellspacing="6" cellpadding="6">
                                                                        <tr><td align="left" colspan="3" style="width:100%; height:20px" class="table_caption">&nbsp;<label id=lblRecordListHead style="width:auto; height:auto;">OTHER CHANNELS - VIEW BY BATCH</label></td></tr>                                                
                                                                        <tr>
                                                                            <td style="height:auto; width:100%" colspan="3">
                                                                                <div id="dgRecords">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <div id="divGrdControl" style="">
                                                                <br />
                                                                    <div>
                                                                        <table style="width:100%" cellpadding="2" cellspacing="2">
                                                                            <tr>
                                                                                <td style="width:27%; height:16px" align="right">
                                                                                <td style="width:26%; height:16px; border:solid 1px #336699" align="center" class="cell_caption" >SELECT BY RECORD STATUS:&nbsp;</td>
                                                                                <td style="width:15%; height:16px; border:solid 1px #336699" align="left">
                                                                                    <select id="ddRecordStatus"  style="width:103px" onchange="javascript:vw.status_changed();"></select>
                                                                                </td>
                                                                                <td style="width:28%; height:16px;" colspan="2">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width:28%"></td>
                                                                                <td align="center" colspan="2" style="width:40%; border:solid 1px #336699"><input value="PROCESS TO IRS" style="width:100%; height:30px" type="button" class ="button" id="btnProcessFile" onclick="javascript:vw.process_file()" /></td>
                                                                                <td style="width:28%" colspan="2"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
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
                        </div>
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>