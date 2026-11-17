<%@ Page Language="C#" AutoEventWireup="true" Codebehind="InternetRemitterARS.aspx.cs"
    Inherits="IRSWorld.Internet.InternetRemitterARS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Internet Remitter</title>
    <link href="../css/reskin_min.css" rel="Stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/commons_min.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/js_generic_min.js" type="text/javascript"></script>

    <script src="../js/Internet/jsInternetRemitterARS.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>

    <script src="../js/jsgridselector_min.js" type="text/javascript"></script>

    <script src="../js/popcalendar_min.js" type="text/javascript"></script>

    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/datagrid.js" type="text/javascript""></script>

    <script src="../js/CardRem/jsGFMCMaintenance.js" type="text/javascript"></script>

    <script type="text/javascript">
            var ars_client = new ars_client();
    </script>

</head>
<body onload="javascript:ars_client.initialize();">
    <form id="form1" runat="server">
        <!-- CONTAINER -->
        <div style="width: 100%; height: 100%; overflow: scroll; position: absolute;">
            <!-- POP MODAL SCREEN -->
            <div id="divPopProcess" style="height: 200px; background: White; z-index: 75; position: absolute;
                left: 1006px; color: darkblue; padding: 10px; display: none; font-family: tahoma;
                top: 15px;">
                <table style="height: 100%; width: 350px;">
                    <tr>
                        <td>
                            <label id="lblLoadingMsg" style="text-align: center; color: Chocolate">
                            </label>
                            <div id="divLoadingMsg">
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- SEARCH SCREEN -->
            <div id="divSearch" style="display: block;">
                <table>
                    <tr>
                        <td id="tdSearchMain" colspan="2" style="width: 100%; text-align: center; vertical-align: middle;
                            border-collapse: collapse; height: 2px;">
                            <center>
                                <div id="divInternetRemitterSearch">
                                    <table cellpadding="0" cellspacing="0" style="width: 80%">
                                        <tr>
                                            <td style="width: 100%; height: auto;">
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td colspan="3" style="vertical-align: bottom; width: 800px; text-align: left">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="vertical-align: bottom; width: 0%; border-collapse: collapse; height: auto;
                                                                        text-align: left">
                                                                        <img src="../images/reskin/toe.GIF" /></td>
                                                                    <td class="table_head_dark" style="width: 550px; height: auto">
                                                                        CUSTOMER SEARCH</td>
                                                                    <td style="vertical-align: bottom; width: 250px; height: auto; text-align: left">
                                                                        <img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3" style="width: 1024px; height: auto;">
                                                            <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid;
                                                                border-left: #394397 3px solid; width: 1024px; border-bottom: #394397 3px solid">
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 100px; height: 23px;">
                                                                        Last Name
                                                                    </td>
                                                                    <td style="width: 200px; text-align: left; height: 23px;">
                                                                        <input id="txtSLastName" style="width: 190px; text-align: left" type="text" />
                                                                    </td>
                                                                    <td class="cell_caption" style="width: 100px; height: 23px">
                                                                        First Name
                                                                    </td>
                                                                    <td style="width: 200px; height: 23px; text-align: left">
                                                                        <input id="txtSFirstName" style="width: 190px; text-align: left" type="text" />
                                                                    </td>
                                                                    <td class="cell_caption" style="width: 100px; height: 23px;">
                                                                        Middle Name
                                                                    </td>
                                                                    <td style="width: 200px; text-align: left; height: 23px;">
                                                                        <input id="txtSMiddleName" style="width: 190px; text-align: left" type="text" />
                                                                    </td>
                                                                </tr>
                                                                <tr style="text-align: center">
                                                                    <td colspan="6">
                                                                        <table style="width: 1024px">
                                                                            <tr>
                                                                                <td nowrap="nowrap" style="width: 140px; height: auto">
                                                                                    <input id="btnRemSearch" class="button" onclick="javascript:ars_client.load_grid_viewer();"
                                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                        style="width: 150px" type="button" value="REMITTER SEARCH" /></td>
                                                                                <td style="width: 130px">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 130px">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 130px">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 130px">
                                                                                    &nbsp;</td>
                                                                                <td nowrap="nowrap" style="width: 140px; height: auto">
                                                                                    <input id="btnAddRemCIF" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                        onclick="javascript:ars_client.hideSearch(true, this);" style="width: 150px"
                                                                                        type="button" value="ADD NEW REMITTER" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trSearchResults" style="display: none;">
                                                                    <td colspan="6">
                                                                        <div id="divSearchResults">
                                                                            <table>
                                                                                <tr>
                                                                                    <td id="tdReportLookup" colspan="2" style="width: 100%; border-collapse: collapse;
                                                                                        height: auto; display: block;">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                        <div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="width: 801px">
                                                                                        <input id="btnAddNewBenef" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                            onclick="javascript:ars_client.getRemitterInfo();" style="width: 150px" type="button"
                                                                                            value="ADD NEW BENEFICIARY" />
                                                                                    </td>
                                                                                    <%--<td style="width: 801px">
                                                                                    <input id="btnViewRemitter" class="button" onmouseout="this.className='button';"
                                                                                        onmouseover="this.className='button_over';" onclick="javascript:ars_client.getRemitterAndBeneInfo();"
                                                                                        style="width: 150px" type="button" value="EDIT INFORMATION" />
                                                                                </td>
                                                                                <td>
                                                                                    <input id="btnCloseSearch" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                        style="width: 150px" type="button" value="CLOSE" />
                                                                                </td>--%>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                            <td colspan="6">
                                                                <table style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;
                                                                    width: 1024px; border-bottom: #394397 3px solid; height: auto">
                                                                    <tr>
                                                                        <td colspan="6">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="5" style="width: 900px; height: 21px;">
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 801px">
                                                                            <input id="btnAddNewBenef" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                onclick="javascript:ars_client.hideSearch(true);" style="width: 150px" type="button"
                                                                                value="ADD NEW BENEFICIARY" />
                                                                        </td>
                                                                        <td>
                                                                            <input id="btnCloseSearch" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                style="width: 150px" type="button" value="CLOSE" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>--%>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </center>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- MAIN SCREEN -->
            <div id="divmain" style="width: 100%; height: auto; padding-top: 15px; display: none;">
                <table style="width: 100%; height: 100%;">
                    <tr>
                    </tr>
                    <tr>
                        <td style="width: 86%; height: auto; vertical-align: top; padding-left: 5px;">
                            <div id="divInternetRemitter" style="width: 100%; height: 65%;">
                                <table style="width: 99%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width: 100%;">
                                            <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="vertical-align: bottom; width: 0%; height: 24px;
                                                        border-collapse: collapse">
                                                        <img src="../images/reskin/toe.GIF" /></td>
                                                    <td id="capResult" class="table_head_dark" style="width: 40%; height: 24px;">
                                                        Customer Enrollment / Adding Products Screen</td>
                                                    <td id="tdpopup2" align="left" style="width: 60%; vertical-align: bottom; height: 24px">
                                                        <img align="absbottom" src="../images/reskin/rend.GIF" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 100%">
                                            <table style="width: 80%; height: 100%;" class="table_border">
                                                <tr>
                                                    <td style="width: 100%; height: 100%; vertical-align: top">
                                                        <table id="tbl_content" width="100%px">
                                                            <tr>
                                                                <td style="width: 50%;">
                                                                    <!-- REMITTER INFORMATION -->
                                                                    <div id="divRemitter">
                                                                        <table id="tblRemInfo1" style="width: 100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: center; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="8">
                                                                                    REMITTER&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Last Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemLName" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astRemLName">*</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Date of Birth&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; height: 24px; color: red;">
                                                                                    <input id="txtDateofBirth" type="text" style="width: 90%;" maxlength="200" onkeyup="javascript:ars_client.validateDate();" /><label
                                                                                        id="astDateofBirth">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Middle Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemMName" type="text" style="width: 90%;" maxlength="80" /><label id="astRemFName">*</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Place of Birth&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; height: 24px; color: red;">
                                                                                    <input id="txtPlaceofBirth" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astPlaceofBirth">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>First Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemFName" type="text" style="width: 90%;" maxlength="80" /><label id="astRemMName">*</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Contact No&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; height: 24px; color: red;">
                                                                                    <input id="txtContactNo" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astContactNo">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Nationality&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <select id="ddRemCitizenship" style="width: 90%;">
                                                                                    </select>
                                                                                    <label id="astNationality">*</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Occupation&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; height: 24px; color: red;">
                                                                                    <input id="txtOccupation" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astOccupation">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: left; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="8">
                                                                                    CURRENT RESIDENCE ADDRESS&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Street No. / Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemAddress1" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astRemAddress1">*</label></td>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Zip / Postal Code&nbsp;</td>
                                                                                <td align="left" style="width: 309px; color: red; height: 24px">
                                                                                    <input id="txtRemCurrZipCode" type="text" style="width: 89%;" maxlength="200" /><label
                                                                                        id="astRemAddress2">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>State&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtState" type="text" style="width: 90%;" maxlength="200" /><label id="astState">*</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Country&nbsp;</td>
                                                                                <td align="left" style="width: 309px; color: red; height: 24px">
                                                                                    <select id="ddRemCountry" style="width: 90%;">
                                                                                    </select>
                                                                                    <label id="astRemCountry">
                                                                                        *</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>City&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemCity" type="text" style="width: 90%;" maxlength="200" /><label id="astRemCity">*</label></td>
                                                                                <td colspan="2">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td class="cell_caption" style="width: 80%; height: 24px; text-align: center;">
                                                                                                Residence Address is Permanent Address:
                                                                                            </td>
                                                                                            <td align="left" style="width: 10%; color: red; height: 24px">
                                                                                                <input id="chkRemPermanentAdd" style="width: 100%" type="checkbox" /></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: left; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="8">
                                                                                    MAILING ADDRESS&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Stree No. / Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemAddress1_Mail" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astRemAddress1_Mail">*</label></td>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Zip / Postal Code&nbsp;</td>
                                                                                <td align="left" style="width: 309px; color: red; height: 24px">
                                                                                    <input id="txtRemMailZipcode" type="text" style="width: 89%;" maxlength="200" /><label
                                                                                        id="Label1">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>State&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtState_Mail" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astRemPOBox_Mail">*</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Country&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; color: red; height: 24px">
                                                                                    <select id="ddRemCountry_Mail" style="width: 90%;">
                                                                                    </select>
                                                                                    <label id="astRemDistrict_Mail">
                                                                                        *</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>City&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemCity_Mail" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astRemCity_Mail">*</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Email Address&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; color: red; height: 24px">
                                                                                    <input id="txtRemEmailAdd" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astRemEmailAdd">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: left; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="8">
                                                                                    Identification&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>ID Type&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 17px; color: red;">
                                                                                    <select id="ddRemIDType" style="width: 92%;">
                                                                                    </select>
                                                                                    <label id="astRemIDType">
                                                                                        *</label>
                                                                                </td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>ID Place of Issuance&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; color: red; height: 24px">
                                                                                    <input id="txtRemPlaceOfIssuance" type="text" style="width: 86%;" maxlength="50" /><label
                                                                                        id="astPlaceOfIssuance">*</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>ID No&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 31%; height: 24px; color: red;">
                                                                                    <input id="txtRemIDNo" type="text" style="width: 90%;" maxlength="50" />
                                                                                    <label id="astRemIDNo">
                                                                                        *</label></td>
                                                                                <td class="cell_caption" style="width: 153px; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Expiry Date (mm/dd/yyyy)&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 180px; color: red; height: 17px">
                                                                                    <input id="txtRemIDExpDate" type="text" style="width: 74%;" maxlength="10" />&nbsp;
                                                                                    <label id="astRemIDExpDate">
                                                                                        *</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" style="width: 18%; height: 23px" colspan="8">
                                                                                    <input id="btnRemBenSame" style="height: 28px; width: 100%" type="button" value="REMITTER and BENEFICIARY are the same"
                                                                                        onclick="javascript:ars_client.SameRemAndBene();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <br />
                                                                    <!-- SELECTION OF PRODUCTCODES -->
                                                                    <div id="divProducts">
                                                                        <table id="tblProducts" style="width: 100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: center; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="8">
                                                                                    ADDING PRODUCTS&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: left; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="3">
                                                                                    REMITTANCE IMPLEMENTATION&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 18%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Product&nbsp;</td>
                                                                                <td align="left" colspan="2" style="width: 31%; height: 17px; color: red;">
                                                                                    <label id="Label2">
                                                                                        *
                                                                                        <select id="ddProduct" style="width: 92%;" onchange="javascript:ars_client.showSpecificProduct(this);">
                                                                                        </select>
                                                                                    </label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <br />
                                                                </td>
                                                                <td valign="top" style="width: 50%;">
                                                                    <!-- BENEFICIARY DETAILS -->
                                                                    <div id="divBeneficiary">
                                                                        <table id="tblBenefInfo" style="width: 100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: center; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="4">
                                                                                    BENEFICIARY&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Last Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefLName" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefLName">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Middle Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefMName" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefMName">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>First Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefFName" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefFName">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Steet No / Name&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefStreet" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefStreet">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Town / City&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefCity" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefCity">*</label></td>
                                                                            </tr>
                                                                            <tr id="searchzipcode">
                                                                                <td align="left" style="width: 40%; height: 23px">
                                                                                    <input id="btnBenefZipcode" style="height: 28px; width: 97%" type="button" value="Zipcode /Postal Code"
                                                                                        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                </td>
                                                                                <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                    <input id="txtBeneZip" type="text" onkeypress="javascript:return ars_client.preventsubmit(event);" 
                                                                                    style="width: 90%;" maxlength="400" /><label id="astBeneZip">*</label>
                                                                                    <br />
                                                                                    <select id="ddlZipCode" style="display:none; width:auto; font-weight: bold;position:absolute; z-index:1;" onclick="ars_client.zip_visibility('select')"></select>
                                                                                    </td>
                                                                            </tr>
                                                                            <%--<tr id="searchzipcodeDdl">
                                                                                <td align="left" style="width: 40%; height: 23px">
                                                                                    <input id="Button1" style="height: 28px; width: 97%" type="button" value="Zipcode /Postal Code"
                                                                                        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                </td>
                                                                                <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                    
                                                                                </td>
                                                                            </tr>--%>
                                                                            <tr id="lblfield" style="display:none;">
                                                                                <td align="left" style="width: 40%; height: 23px">
                                                                                    <input id="btnBenefZipCode2" style="height: 28px; width: 97%" type="button" value="Zipcode /Postal Code"
                                                                                        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                </td>
                                                                                <td align="left" style="width: 35%; height: 24px; color: red;" colspan="3">
                                                                                    <input type="text" id="txtSearchedZip" style="font-weight: bold; width:90%" readonly="readonly" />
							                                                        <br />
							                                                        <input type="button" style="width:auto;" id="btnSearch" class="button" value="Change ZipCode" onclick="ars_client.zip_visibility('Search')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Province&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefProvince" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefProvince">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Date of Birth&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefDOB" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefDOB">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Email Address&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefEmailAdd" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefEmailAdd">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 40%; height: 24px; text-align: center;">
                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Telephone No&nbsp;</td>
                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                    <input id="txtBenefTelNo" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                        id="astBenefTelNo">*</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td align="left" style="width: 18%; height: 23px" colspan="4">
                                                                                    <input id="btnSaveBenef" style="height: 28px; width: 190px;" disabled="disabled;"
                                                                                        onclick="javascript:ars_client.editCIF('Beneficiary');" type="button" value="EDIT Beneficiary Details"
                                                                                        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td align="left" style="width: 18%; height: 23px" colspan="4">
                                                                                    <input id="btnSaveRemBenef" style="height: 28px; width: 100%" type="button" value="SAVE REMITTER and BENEFICIARY"
                                                                                        onclick="javascript:ars_client.check_CIF_Watchlist()" class="button" onmouseover="this.className='button_over';"
                                                                                        onmouseout="this.className='button';" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                    <!-- IMPLEMENTATION DETAILS -->
                                                                    <div id="divImplementation">
                                                                        <table id="tblImplementation" style="width: 100%; border: solid 1px #394397;">
                                                                            <tr>
                                                                                <td class="table_caption" style="text-align: center; text-indent: 5px; color: white;
                                                                                    height: 26px;" colspan="4">
                                                                                    IMPLEMENTATION DETAILS&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 345px">
                                                                                    <div id="divImplementationApas">
                                                                                        <table id="tblImplemDetails" style="width: 100%; border: solid 1px #394397;">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 35%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Currency&nbsp;</td>
                                                                                                <td align="left" style="width: 25%; color: red; height: 24px" colspan="3">
                                                                                                    <select id="ddImplemApasCurrency" style="width: 90%;">
                                                                                                    </select>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                </td>
                                                                                                <td align="left" style="width: 18%; height: 23px" colspan="4">
                                                                                                    <input id="btnSaveDetails" style="height: 28px; width: 190px;" disabled="disabled;"
                                                                                                        type="button" value="Save Details" class="button" onmouseover="this.className='button_over';"
                                                                                                        onmouseout="this.className='button';" onclick="javascript:ars_client.validateProduct('APAS', '');" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <br />
                                                                                    <div id="divImplementationCredittoPNBacct" style="display: none;">
                                                                                        <table id="tblImplemCredittoPNBacct" style="width: 100%; border: solid 1px #394397;">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Account Number&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtBenefPNBaccountno" type="text" style="width: 90%;" maxlength="200" /><label
                                                                                                        id="astBenefPNBaccountno">*</label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right" style="width: 18%; height: 23px" colspan="4">
                                                                                                    <input id="btnValidateAcctNo" style="height: 28px; width: 190px;" type="button" value="Validate PNB Account Number"
                                                                                                        onclick="javascript:ars_client.product_validatePNBAccount();" class="button"
                                                                                                        onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr id="trImplementationCreditToPNBAcct" style="display: none;">
                                                                                                <td colspan="5">
                                                                                                    <table style="width: 100%;">
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Currency&nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                                <input id="txtBenefPNBcurrency" type="text" style="width: 90%;" maxlength="200" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Account Type&nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                                <input id="txtBenefPNBaccountType" type="text" style="width: 90%;" maxlength="200" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Account Status&nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                                <input id="txtBenefPNBaccountStatus" type="text" style="width: 90%;" maxlength="200" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                                <em><span style="font-size: 10pt; color: #ff0000"></span></em>Bank&nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                                <input id="txtBenefPNBbank" type="text" style="width: 90%;" maxlength="200" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="right" style="width: 18%; height: 23px" colspan="2">
                                                                                                                <input id="btnPNBproduct_saveDetails" style="height: 28px; width: 190px;" type="button"
                                                                                                                    value="Save Details" onclick="javascript:ars_client.product_CreditToPNBAcct_save();"
                                                                                                                    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                            <td align="right" style="width: 18%; height: 23px" colspan="2">
                                                                                                                <input id="btnPNBproduct_Remit" style="height: 28px; width: 190px;" disabled="disabled"
                                                                                                                    type="button" value="Remit" onclick="" class="button" onmouseover="this.className='button_over';"
                                                                                                                    onmouseout="this.className='button';" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="divImplemValidatedAccount" style="display: none;">
                                                                                        <table id="tblValidatedAccount" style="width: 100%; border: solid 1px #394397;">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Currency&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtValidAccountCurr" type="text" style="width: 90%;" maxlength="200" disabled="disabled;" /><label
                                                                                                        id="astValidAccountCurr">*</label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Account Type&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtValidAccountType" type="text" style="width: 90%;" maxlength="200" disabled="disabled;" /><label
                                                                                                        id="astValidAccountType">*</label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Account Status&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtValidAccountStatus" type="text" style="width: 90%;" maxlength="200"
                                                                                                        disabled="disabled;" /><label id="astValidAccountStatus">*</label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Bank&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="4" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtValidBank" type="text" style="width: 90%;" maxlength="200" disabled="disabled;" /><label
                                                                                                        id="astValidBank">*</label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                </td>
                                                                                                <td align="left" style="width: 35%; height: 23px" colspan="4">
                                                                                                    <input id="btValidSaveDetails" style="height: 28px; width: 190px;" disabled="disabled;"
                                                                                                        type="button" value="Save Details" class="button" onmouseover="this.className='button_over';"
                                                                                                        onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <br />
                                                                                    <div id="divImplemCredittoGFC" style="display: none;">
                                                                                        <table id="tblCredittoGFC" style="width: 100%; border: solid 1px #394397;">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Name&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtGFCName" type="text" style="width: 90%;" maxlength="200" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>CIFID&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtGFCCIFID" type="text" style="width: 90%;" maxlength="200" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Mode&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 25%; color: red; height: 24px" colspan="3">
                                                                                                    <select id="ddGFCMode" style="width: 90%;">
                                                                                                        <option value="1">Issue Remitter Card</option>
                                                                                                        <option value="2">Issue Beneficiary Card</option>
                                                                                                        <option value="3">Remit to Existing Card</option>
                                                                                                    </select>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>A/C Currency&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 25%; color: red; height: 24px" colspan="3">
                                                                                                    <select id="ddGFCCurrency" style="width: 90%;">
                                                                                                    </select>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>CK Referenceno&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtCKRefNo" type="text" style="width: 90%;" maxlength="200" disabled="disabled;" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="right" style="width: 18%; height: 23px">
                                                                                                    <input id="btnGetGFCCK" style="height: 28px; width: 100%;" type="button" value="GET"
                                                                                                        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                                <td align="right" style="width: 18%; height: 23px">
                                                                                                    <input id="btnGetGFCCancel" style="height: 28px; width: 100%;" type="button" value="CANCEL"
                                                                                                        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                                <td align="right" style="width: 18%; height: 23px">
                                                                                                    <input id="btnSaveGFC" style="height: 28px; width: 100%;" type="button" value="SAVE"
                                                                                                        onclick="javascript:ars_client.product_GFC_save();" class="button" onmouseover="this.className='button_over';"
                                                                                                        onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                                <td align="right" style="width: 18%; height: 23px">
                                                                                                    <input id="btnCloseGFC" style="height: 28px; width: 100%;" type="button" value="CLOSE"
                                                                                                        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <br />
                                                                                    <div id="divImplemCredittoOtherBank" style="display: none;">
                                                                                        <table id="tblImplemCredittoOtherBank" style="width: 100%; border: solid 1px #394397;">
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Account Number&nbsp;
                                                                                                </td>
                                                                                                <td align="left" colspan="3" style="width: 35%; height: 24px; color: red;">
                                                                                                    <input id="txtOtherBankAccountNo" type="text" style="width: 90%;" maxlength="200" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Account Currency&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 25%; color: red; height: 24px" colspan="3">
                                                                                                    <select id="ddCredittoOtherBankCurr" style="width: 90%;" onchange="javascript:ars_client.load_dropdown('ddOtherBankName');">
                                                                                                    </select>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Bank Name&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 25%; color: red; height: 24px" colspan="3">
                                                                                                    <select id="ddOtherBankName" onclick="" style="width: 90%;">
                                                                                                    </select>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                </td>
                                                                                                <td align="right" style="width: 18%; height: 23px">
                                                                                                    <input id="btnSaveOtherBank" style="height: 28px; width: 100%;" type="button" value="Save Details"
                                                                                                        onclick="javascript:ars_client.product_OtherBank_save();" class="button" onmouseover="this.className='button_over';"
                                                                                                        onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <br />
                                                                                    <div id="divImplemDoortoDoor" style="display: none;">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 25%; height: 24px; text-align: center;">
                                                                                                    <em><span style="font-size: 10pt; color: #ff0000"></span></em>Delivery Destinations&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 25%; color: red; height: 24px" colspan="3">
                                                                                                    <select id="ddDoorProduct" style="width: 90%;" onchange="javascript:ars_client.validateProduct('DTD', '');">
                                                                                                        <option></option>
                                                                                                        <option value="8">Metro Manila</option>
                                                                                                        <option value="9">Provincial</option>
                                                                                                    </select>
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
                                                        <table id="tblInternetRemitter" style="width: 100%" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <div id="divImgLoad" style="z-index: 101; position: absolute; left: 112px; color: darkblue;
                                                                        border: solid 2px #000; padding: 10px; font-family: tahoma; top: 409px; display: none;">
                                                                        <table style="width: 35%;" cellpadding="0" cellspacing="6">
                                                                            <tr>
                                                                                <td class="cell_desc" align="left" style="font-size: 12px; width: 30%;">
                                                                                    <label id="lblLoad">
                                                                                        Loading...</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height: 19px">
                                                                                    <center>
                                                                                        <div id="divLoading">
                                                                                        </div>
                                                                                    </center>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
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
                            </div>
                            <!--END OF divInternetRemitter-->
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
