<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VIP.aspx.cs" Inherits="IRSWorld.VIP.VIP" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <!--meta http-equiv="refresh" content="5"-->
    <title>VIP System - Data Entry</title>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>
    <script src="../js/popcalendar_min.js" type="text/javascript"></script>
    <script src="../js/jsExpand.js" type="text/javascript"></script>
    <script src="../js/jsBalloon.js" type="text/javascript"></script>
    <script src="../js/jsImageResize.js" type="text/javascript"></script>
    <script src="../js/VIP/jsVIP.js" type="text/javascript"></script>
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var vip_page = new vip_page();
    </script>
</head>
<body onload="javascript:vip_page.initialize();">
    <form id="form1" runat="server">
    <div>
        <!-- VARIABLES -->
        <input id="lblSEQID" type="hidden" style="width:1px;" />
    
        <!-- POP MODAL SCREEN -->
        <div id="divPopProcess" style="height:200px;background:White;z-index:75;position:absolute;left:1006px;color:darkblue;padding:10px;display:none;font-family:tahoma;top:15px;">
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
        
        <!-- GRID -->
        <div id="divSearchGrid" style="z-index:75;position:absolute;left:1004px;color:darkblue;border:solid 2px #000;padding:10px;font-family:tahoma;top:260px;height:auto;width:600px;display:none;">
            <div id="divLoading"><center><IMG SRC='../images/ajaxloader.gif'></IMG></center></div>
            <div id="divSearchGridTop" style="display:block;"></div>
            <br />
            <div id="divSearchGridBottom" style="display:block;">
                <table style="width:600px;" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="text-align:center;">
                            <input id="btnGSearch" class="button" onclick="javascript:vip_page.showSearch();" type="button" value="Back to Search" style="width:200px;" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        
        <div id="divTable" style="width:100%;">
            <table border="0" style="width:100%;">
                <tr>
                    <td style="width:15%;vertical-align:top;height:100%;">
                    <center>
                        <table style="width:100%;">
                            <tr><td>&nbsp;</td></tr>
                            <tr>
                                <td style="color: black;text-align:right;border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                    <a href="javascript:vip_page.showLink('Search');" style="color: black;" class="cell_caption">Search/Add Client</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: black;text-align:right;border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                    <a href="javascript:vip_page.showLink('View');" style="color: black;" class="cell_caption">View Clients</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: black;text-align:right;border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                    <a href="javascript:vip_page.showLink('Extract');" style="color: black;" class="cell_caption">Print the card</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: black;text-align:right;border-bottom:solid 1px #5697C1;" onmouseover="this.style.backgroundColor = 'Azure';" onmouseout="this.style.backgroundColor='transparent';">
                                    <a href="javascript:vip_page.showLink('Report');" style="color: black;" class="cell_caption">Reports</a>
                                </td>
                            </tr>
                        </table>
                    </center>
                    </td>
                    
                    <td style="width:85%;" align="center">
                        <!-- SEARCH SCREEN -->
                        <div id="divSearch" style="display:block;">
                            &nbsp;<table cellpadding="0" cellspacing="0" style="width:400px;">
                                <tr>
                                    <td style="height:auto;width:400px;">
                                        <table cellpadding="0" cellspacing="0" style="width:400px;">
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align:bottom;text-align:left;width:400px;" colspan="3">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width:0%; border-collapse:collapse;height:auto;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>
                                                            <td class="table_head_dark" style="height:auto;width:150px;">
                                                                SEARCH</td>
                                                            <td style="height:auto;text-align:left;vertical-align:bottom;width:250px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width:400px;">
                                                    <table border="0" style="border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:400px;">
                                                         <tr>
                                                            <td style="width:100px;" class="cell_caption">
                                                                Remitter ID&nbsp;
                                                            </td>
                                                            <td style="text-align:left;width:300px;">
                                                                <input id="txtSRemitterID" style="text-align:left;width:290px;" type="text" value="" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:100px;" class="cell_caption">
                                                                Last Name&nbsp;
                                                            </td>
                                                            <td style="text-align:left;width:300px;">
                                                                <input id="txtSLastName" style="text-align:left;width:290px;" type="text" value="" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="height:auto;width:100px;" class="cell_caption">
                                                                First Name&nbsp;
                                                            </td>
                                                            <td style="height:auto; text-align:left;width:300px;">
                                                                <input id="txtSFirstName" style="text-align:left;width:290px;" type="text" value="" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width:100px;" class="cell_caption">
                                                                Middle Name&nbsp;
                                                            </td>
                                                            <td style="text-align:left;width:300px;">
                                                                <input id="txtSMiddleName" style="text-align:left; width:290px;" type="text" value="" />
                                                            </td>
                                                        </tr>
                                                        <tr style="text-align:center;">
                                                            <td colspan="2">
                                                                <table style="width:400px;">
                                                                    <tr>
                                                                        <td nowrap style="height:auto;"><input id="btnAddNewVIP" onclick="javascript:vip_page.addNewVIP();" type="button" value="Add New VIP" style="width:100px;" /></td>
                                                                        <td nowrap style="height:auto;"><input id="btnSearch" onclick="javascript:vip_page.callSearchCIF();" type="button" value="Search" style="width:100px;" /></td>
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
                        
                        <!-- SEPERATOR -->
                        <div id="divSeperator">
                            &nbsp;
                        </div>
                        
                        <!-- DATA ENTRY -->
                        <div id="divDataEntry" style="display:none;">
                            <br />
                            <table cellpadding="0" cellspacing="0" style="width:700px;">
                                <tr>
                                    <td style="vertical-align:bottom;text-align:left;" colspan="3">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width:0%; border-collapse:collapse;height:11px;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                                <td class="table_head_dark" style="height:11px;width:300px;">
                                                    VIP Data Entry</td>
                                                <td style="height:11px;text-align:left;vertical-align:bottom;width:400px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>    
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width:700px;">
                                        <table style="border-right: #394397 3px solid;border-top: #394397 3px solid;border-left: #394397 3px solid; border-bottom:#394397 3px solid;height:auto;">
                                            <tr>
                                                <td class="table_caption" colspan="4" style="color:white;text-indent:5px;text-align:left;height:25px;width:700px;">
                                                    Client Information</td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Remitter ID:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:200px;">
                                                    <input id="txtRemitterID" type="text" style="width:197px;" readonly="readOnly" />
                                                </td>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Status:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:200px;">
                                                    <span id="spnStatus" class="cell_caption" style="color:Navy;">New</span>
                                                </td>                                        
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Last Name:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                    <input id="txtLastName" type="text" style="width:547px;" maxlength="60" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    First Name:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                    <input id="txtFirstName" type="text" style="width:547px;" maxlength="60" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Middle Initial:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                    <input id="txtMiddleName" type="text" style="width:547px;" maxlength="4" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Mobile Number:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:200px;">
                                                    <input id="txtMobileNo" type="text" style="width:197px;" maxlength="8" onkeypress="javascript:return vip_page.isNumberKey(event);" />
                                                </td>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    <span id="spnHKID" style="padding-bottom:1px;border-bottom:1px dotted #900;cursor:pointer;" onmouseover="javascript:tooltip.show(vip_page.showBalloon('HKID'));" onmouseout="javascript:tooltip.hide();">HK ID:</span>&nbsp;
                                                </td>
                                                <td style="height:auto;text-align:left;width:200px;">
                                                    <input id="txtHKID" type="text" style="width:189px;" maxlength="13" />
                                                </td>
                                            </tr>      
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Organization Name:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                    <select id="ddlOrganization" class="ddl" style="width:547px" onchange="javascript:vip_page.getDDLAjax('ddlOrganization');"></select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    <span id="apnApplicationDate" style="padding-bottom:1px;border-bottom:1px dotted #900;cursor:pointer;" onmouseover="javascript:tooltip.show(vip_page.showBalloon('ApplicationDate'));" onmouseout="javascript:tooltip.hide();">Application Date:</span>&nbsp;
                                                </td>
                                                <td style="height:auto;text-align:left;width:200px;">
                                                    <input id="txtApplicationDate" type="text" style="width:177px;" readonly="readOnly" maxlength="10" onchange="javascript:vip_page.setExpiry();" />
                                                    <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtApplicationDate, 'yyyy-mm-dd',document.getElementById('divDataEntry'));" style="vertical-align:middle;">
                                                </td>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Expiry Date:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:200px;">
                                                    <input id="txtExpiryDate" type="text" style="width:197px;" readonly="readOnly" />
                                                </td>                                        
                                            </tr>
                                            <tr id="trPrinted" style="display:none;">
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Printed:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                    <input id="chkPrinted" type="checkbox" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="width:700px;">
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                            <div id="divButtonsAdd" style="display:none;">
                                <table cellpadding="0" cellspacing="0" style="width:700px;">
                                    <tr>
                                        <td style="height: 22px">
                                            <input id="btnClear" type="button" value="Clear" onclick="javascript:vip_page.clearText();" style="width:200px;"/>
                                        </td>
                                        <td style="height: 22px">
                                            <input id="btnSaveVIP" type="button" value="Save VIP Information" onclick="javascript:vip_page.saveVIP();" style="width:200px;"/>
                                        </td>
                                        <td style="height: 22px">
                                            <input id="btnBackSearch" type="button" value="Back to Search" onclick="javascript:vip_page.showSearch();" style="width:200px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divButtonsUpdate" style="display:none;">
                                <table cellpadding="0" cellspacing="0" style="width:700px;">
                                    <tr>
                                        <td style="height: 22px">
                                            <input id="btnDelete" type="button" value="Delete VIP Client" onclick="javascript:vip_page.deleteVIP();" style="width:200px;"/>
                                        </td>
                                        <td style="height: 22px">
                                            <input id="btnUpdate" type="button" value="Update VIP Information" onclick="javascript:vip_page.updateVIP();" style="width:200px;"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        
                        <!-- PRINTING OF CARD -->
                        <div id="divPrint" style="display:none;">
                            <br />
                            <table cellpadding="0" cellspacing="0" style="width:700px;">
                                <tr>
                                    <td style="vertical-align:bottom;text-align:left;" colspan="3">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width:0%; border-collapse:collapse;height:11px;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                                <td class="table_head_dark" style="height:11px;width:300px;">
                                                    VIP Printing of Card</td>
                                                <td style="height:11px;text-align:left;vertical-align:bottom;width:400px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>    
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width:700px;">
                                        <table style="border-right: #394397 3px solid;border-top: #394397 3px solid;border-left: #394397 3px solid; border-bottom:#394397 3px solid;height:auto;">
                                            <tr>
                                                <td class="table_caption" colspan="4" style="color:white;text-indent:5px;text-align:left;height:25px;width:700px;">
                                                    Extract Information</td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Organization Name:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                    <select id="ddlPOrganization" class="ddl" style="width:547px" onchange="javascript:vip_page.getDDLAjax('ddlPOrganization');"></select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="height:auto;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="cell_caption" style="height:auto;width:136px;">Date From:&nbsp;
                                                            </td>
                                                            <td>
                                                                <input id="txtDateFrom" style="text-align:left; width:80%; vertical-align:middle;" type="text" />
                                                                <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtDateFrom, 'yyyy-mm-dd',document.getElementById('divPrint'));" style="vertical-align:middle;">
                                                            </td>
                                                            <td style="width:1px;">&nbsp;</td>
                                                            <td class="cell_caption" style="height:auto;width:150px;">Date To:&nbsp;
                                                            </td>
                                                            <td>
                                                                <input id="txtDateTo" style="text-align:left; width:80%; vertical-align:middle;" type="text" />
                                                                <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtDateTo, 'yyyy-mm-dd',document.getElementById('divPrint'));" style="vertical-align:middle;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="right">
                                                    <input id="btnExtractToFile" type="button" value="Extract to File" onclick="javascript:vip_page.extractToFile();" style="width:200px;"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="width:700px;">
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                        
                        <!-- REPORTS -->
                        <div id="divReport" style="display:none;">
                            <br />
                            <table cellpadding="0" cellspacing="0" style="width:700px;">
                                <tr>
                                    <td style="vertical-align:bottom;text-align:left;" colspan="3">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width:0%; border-collapse:collapse;height:11px;text-align:left;vertical-align:bottom;"><img src="../images/reskin/toe.GIF" /></td>    
                                                <td class="table_head_dark" style="height:11px;width:300px;">
                                                    VIP Reports</td>
                                                <td style="height:11px;text-align:left;vertical-align:bottom;width:400px;"><img align="absBottom" src="../images/reskin/rend.gif" /></td>    
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="width:700px;">
                                        <table style="border-right: #394397 3px solid;border-top: #394397 3px solid;border-left: #394397 3px solid; border-bottom:#394397 3px solid;height:auto;">
                                            <tr>
                                                <td class="table_caption" colspan="4" style="color:white;text-indent:5px;text-align:left;height:25px;width:700px;">
                                                    Extract Information</td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height:auto;width:150px;">
                                                    Report Name:&nbsp;</td>
                                                <td style="height:auto;text-align:left;width:550px;" colspan="3">
                                                    <select id="ddlReport" class="ddl" style="width:547px" onchange="javascript:vip_page.getDDLAjax('ddlReport');"></select>
                                                </td>
                                            </tr>
                                            <tr id="trDate" style="display:none;">
                                                <td colspan="4">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="cell_caption" style="height:auto;width:136px;">Date From:&nbsp;
                                                            </td>
                                                            <td>
                                                                <input id="txtRDateFrom" style="text-align:left; width:80%; vertical-align:middle;" type="text" />
                                                                <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtRDateFrom, 'yyyy-mm-dd',document.getElementById('divPrint'));" style="vertical-align:middle;">
                                                            </td>
                                                            <td style="width:1px;">&nbsp;</td>
                                                            <td class="cell_caption" style="height:auto;width:150px;">Date To:&nbsp;
                                                            </td>
                                                            <td>
                                                                <input id="txtRDateTo" style="text-align:left; width:80%; vertical-align:middle;" type="text" />
                                                                <img src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtRDateTo, 'yyyy-mm-dd',document.getElementById('divPrint'));" style="vertical-align:middle;">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="trQuarter" style="display:none;">
                                                <td colspan="4">
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="cell_caption" style="height:auto;width:136px;">Year:&nbsp;
                                                            </td>
                                                            <td>
                                                                <select id="ddlYear" class="ddl" style="width:180px" onchange="javascript:vip_page.getDDLAjax('ddlYear');"></select>
                                                            </td>
                                                            <td style="width:1px;">&nbsp;</td>
                                                            <td class="cell_caption" style="height:auto;width:150px;">Quarter:&nbsp;
                                                            </td>
                                                            <td>
                                                                <select id="ddlQuarter" class="ddl" style="width:180px" onchange="javascript:vip_page.getDDLAjax('ddlQuarter');"></select>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" align="right">
                                                    <input id="btnGenerateReport" type="button" value="Generate Report" onclick="javascript:vip_page.generateReport();" style="width:200px;" disabled="disabled" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="width:700px;">
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                        
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>