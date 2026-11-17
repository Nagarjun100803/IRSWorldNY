<%@ Page Language="C#" AutoEventWireup="true" Codebehind="InternetRemitterEnrollment.aspx.cs"
    Inherits="IRSWorld.Internet.InternetRemitterEnrollment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PNB Global Remit</title>
    <link href="../css/reskin_min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jscommonmethod_min.js" type="text/javascript"></script>

    <script src="../js/ipconfig_min.js" type="text/javascript"></script>

    <script src="../js/js_generic_min.js" type="text/javascript"></script>

    <script src="../js/Admin/user_record.js" type="text/javascript"></script>

    <script src="../js/jshttprequest_min.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow_min.js" type="text/javascript"></script>

    <script src="../js/jsgridselector_min.js" type="text/javascript"></script>

    <script src="../js/commons_min.js" type="text/javascript"></script>

    <script src="../js/popcalendar_min.js" type="text/javascript"></script>

    <script src="../js/Internet/jsInternetRemitterEnrollment.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
         var IRE = new InternetRemitterEnrollment();
                        

    </script>

</head>
<body onload="javascript:IRE.init();" onresize="fixdim();">
    <center>
        <form id="Form1" runat="server">
            &nbsp;
            <%--<div id="Main" style="display: block; overflow: auto; height: 500px">--%>
            <div id="Main" style="display: block; height: 400px; overflow: auto;">
                <center>
                    <div id="divQandA" style="width: auto; height: auto; display: none; border-right: #000 2px solid;
                        padding-right: 10px; border-top: #000 2px solid; padding-left: 10px; z-index: 75;
                        left: 902px; padding-bottom: 10px; border-left: #000 2px solid; color: darkblue;
                        padding-top: 10px; border-bottom: #000 2px solid; font-family: tahoma; position: absolute;
                        top: 307px;">
                        <!--  z-index: 101; position: absolute; -->
                        <%--<table id="Table1" cellpadding="0" cellspacing="5" style="border-right: #394398 1px solid;
                                            border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid;
                                            border-bottom: #394398 1px solid" width="100%">
                                        <tr>
                                            <td id="Td10" class="table_caption" colspan="5" style="color: white; text-indent: 5px;
                                                    text-align: left">
                                                Idology
                                                Questionaire</td>
                                        </tr>
                                        <tr>
                                            <td style="width: 9%; height: 9px">
                                                &nbsp;</td>
                                            <td id="tdQuestion" align="left" style="width: 51%; height: 9px">
                                                </td>                                           
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 9%; height: 24px">
                                                <input id="chkAns0" onclick="javascript:IRE.Ans_OnClick('0');"
                                                    style="width: 21%" type="checkbox" />&nbsp;</td>
                                            <td id="tdQuestionAns1" align="left" style="width: 51%; height: 24px">
                                                &nbsp;
                                                </td>                                    
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 9%; height: 24px">
                                                <input id="chkAns1" onclick="javascript:IRE.Ans_OnClick('1');"
                                                    style="width: 21%" type="checkbox" />&nbsp;</td>
                                            <td id="tdQuestionAns2" align="left" style="width: 51%; height: 24px">
                                                &nbsp;
                                                </td>                                        
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 9%; height: 24px">
                                                <input id="chkAns2" onclick="javascript:IRE.Ans_OnClick('2');"
                                                    style="width: 21%" type="checkbox" />&nbsp;</td>
                                            <td id="tdQuestionAns3" align="left" style="width: 51%; height: 24px">
                                                &nbsp;
                                                </td>                                        
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 9%; height: 24px">
                                                <input id="chkAns3" onclick="javascript:IRE.Ans_OnClick('3');"
                                                    style="width: 21%" type="checkbox" />&nbsp;</td>
                                            <td id="tdQuestionAns4" align="left" style="width: 51%; height: 24px">
                                                &nbsp;
                                                </td>                                        
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 9%; height: 24px">
                                                <input id="chkAns4" onclick="javascript:IRE.Ans_OnClick('4');"
                                                    style="width: 21%" type="checkbox" />&nbsp;</td>
                                            <td id="tdQuestionAns5" align="left" style="width: 51%; height: 24px">
                                                &nbsp;
                                                </td>                                        
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 9%; height: 24px">
                                                &nbsp;</td>
                                            <td align="left" style="width: 51%; height: 24px">
                                                &nbsp;
                                                <input id="btnPrevQuestion"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Prev" style="width: 66px" onclick="javascript:IRE.btnPrev_click();"/>
                                                <input id="btnNextQuestion"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Next" style="width: 66px" onclick="javascript:IRE.btnNext_click();"/>
                                                <input id="btnSubmitAns"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Submit" style="width: 66px" onclick="javascript:IRE.btnSubmitAns_click();"/>
                                                <input id="btnCancelAns"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel" style="width: 66px" onclick="javascript:IRE.btnCancel_click();"/></td>                                        
                                        </tr>                                                                                
                                    </table>--%>
                    </div>
                    <table cellpadding="0" cellspacing="0" style="width: 85%; height: 725px">
                        <tr>
                            <td style="vertical-align: bottom;">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 0%; border-collapse: collapse; height: 24px" valign="bottom">
                                            <img src="../images/reskin/toe.GIF" /></td>
                                        <td id="tdHeader" class="table_head_dark" style="width: 50%; height: 24px">
                                            PNB GLOBAL REMIT&nbsp;</td>
                                        <td align="left" style="width: 50%; height: 24px" valign="bottom">
                                            <img align="absBottom" src="../images/reskin/rend.gif" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="background: white; height: 644px; width: 1091px; vertical-align: top;">
                                <!--valign="top"-->
                                <table id="tblCIF" style="border-right: #394398 3px solid; border-top: #394398 3px solid;
                                    border-left: #394398 3px solid; width: 100%; border-bottom: #394398 3px solid">
                                    <tr>
                                        <td style="width: 99%; height: 306px;">
                                            <div id="CIFSearch" style="display: none;">
                                                <table id="Table2" cellpadding="0" cellspacing="5" style="border-right: #394398 1px solid;
                                                    border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid;
                                                    border-bottom: #394398 1px solid; width: 100%;">
                                                    <tr>
                                                        <td id="Td2" class="table_caption" colspan="5" style="color: White; text-indent: 5px;
                                                            text-align: left">
                                                            REMITTANCE APPLICATION FORM&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 15%; height: 24px">
                                                            Date of Registration:&nbsp;</td>
                                                        <td align="left" style="width: 51%; height: 24px">
                                                            &nbsp;<input id="txtDateReg" maxlength="200" style="width: 98%" type="text" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 9%; height: 24px">
                                                            Ref. No:&nbsp;</td>
                                                        <td id="Td3" runat="server" align="left" colspan="2" style="width: 352px; height: 24px">
                                                            &nbsp;<input id="txtRefNo" maxlength="4" style="width: 94%" type="text" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 15%; height: 24px">
                                                            GFMC Card No:&nbsp;</td>
                                                        <td align="left" style="width: 51%; height: 24px">
                                                            &nbsp;<input id="txtGFCCardNo" maxlength="200" style="width: 98%" type="text" /></td>
                                                        <td class="cell_caption" style="width: 9%; height: 24px">
                                                            Remitter ID No:&nbsp;</td>
                                                        <td align="left" colspan="2" style="width: 352px; height: 24px">
                                                            &nbsp;<input id="txtRemIDNo" maxlength="200" style="width: 94%" type="text" /></td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="CIFINDIVIDUAL" style="width: 100%">
                                                <table id="tblRemInfo" cellpadding="0" cellspacing="5" style="border-right: #394398 1px solid;
                                                    border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid;
                                                    border-bottom: #394398 1px solid; width: 100%;">
                                                    <tr>
                                                        <td id="Td4" class="table_caption" colspan="6" style="color: white; text-indent: 5px;
                                                            text-align: left; height: 26px;">
                                                            REMITTER'S INFORMATION&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 15%; height: 24px">
                                                            User Name:&nbsp;</td>
                                                        <td align="left" style="width: 45%; height: 24px; color: Green; font-size:small" colspan="5">
                                                            &nbsp;<input id="txtUserID" maxlength="16" style="width: 24%" type="text" onchange="javascript:IRE.txtfield_changed(this);"
                                                                onkeypress="javascript:IRE.check_if_user_exist(event);" value="" />&nbsp;<i> *press enter to check if available</i></td>
                                                                
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 15%; height: 24px">
                                                            Salutation:&nbsp;</td>
                                                        <td align="left" style="width: 45%; height: 24px; color: Red" colspan="5">
                                                            &nbsp;<select id="ddlSalutation" style="width: 24%">
                                                                <option value=""></option>
                                                                <option value="MR">MR</option>
                                                                <option value="MS">MS</option>
                                                                <option value="MRS">MRS</option>
                                                            </select>
                                                            *</td>
                                                        <%--<td class="cell_caption" style="width: 10%; height: 24px">
                                                Birthdate:&nbsp;</td>
                                            <td id="Td5" align="left" style="width: 371px; height: 24px">
                                                &nbsp;<input id="txtBirthDate"  maxlength="200" style="width: 94%" type="text" /></td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 15%; height: 24px">
                                                            Last Name:&nbsp;</td>
                                                        <td align="left" style="width: 45%; height: 24px; color: Red" colspan="5">
                                                            &nbsp;<input id="txtLName" maxlength="200" style="width: 52%" type="text" value="" />*</td>
                                                        <%--<td class="cell_caption" style="width: 10%; height: 24px">
                                                Birthdate:&nbsp;</td>
                                            <td id="Td5" align="left" style="width: 371px; height: 24px">
                                                &nbsp;<input id="txtBirthDate"  maxlength="200" style="width: 94%" type="text" /></td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 15%; height: 24px">
                                                            First Name:&nbsp;</td>
                                                        <td align="left" style="width: 45%; height: 24px; color: Red" colspan="5">
                                                            &nbsp;<input id="txtFName" maxlength="200" style="width: 52%" type="text" value="" />*</td>
                                                        <%--<td class="cell_caption" style="width: 10%; height: 24px">
                                                Birth Place:&nbsp;</td>
                                            <td id="Td6" align="left" style="width: 371px; height: 24px">
                                                &nbsp;<input id="txtBirthPlace"  maxlength="100" style="width: 94%" type="text" /></td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 15%; height: 24px">
                                                            Middle Name:&nbsp;</td>
                                                        <td align="left" style="width: 45%; height: 24px" colspan="5">
                                                            &nbsp;<input id="txtMName" maxlength="200" style="width: 52%" type="text" /></td>
                                                        <%--<td class="cell_caption" style="width: 10%; height: 24px">
                                                Gender:</td>
                                            <td id="Td7" align="left" style="width: 371px; height: 24px">
                                                &nbsp;<select id="ddGender" style="width: 95%"></select></td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Birthdate:<br />
                                                            MM/YYYY&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan=5>
                                                            &nbsp;<input id="txtBirthDate" maxlength="200" style="width: 25%" type="text" value="" />
                                                            <img id="calRemIDExpiryDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtBirthDate').disabled == false){popUpCalendar(this, document.getElementById('txtBirthDate'), 'mm/yyyy', document.getElementById('Main'));}"
                                                                src="../images/calendar/imgcalen.png" style="width: 16px" />*</td>
                                                        <%--<td class="cell_caption" style="width: 10%; height: 24px">
                                                Birth Place:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtBirthPlace"  maxlength="200" style="width: 90%"
                                                        type="text" /></td>                                                        
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Gender:&nbsp;</td>
                                            <td align="left" style="width: 24%; height: 24px; color: Red">
                                                &nbsp;<select id="ddGender" style="width: 80%"></select>*</td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            SSS ID/SSN No:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan="5">
                                                            &nbsp;<input id="txtSSSIDNo" maxlength="200" style="width: 52%" type="text" value="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Tel.No.:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan="5">
                                                            &nbsp;<input id="txtTelNo" maxlength="200" style="width: 52%" type="text" value="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Mobile No.:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan="5">
                                                            &nbsp;<input id="txtMobileNo" maxlength="200" style="width: 52%" type="text" value="" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Email Address:&nbsp;</td>
                                                        <td align="left" style="width: 24%; height: 24px; color: Red" colspan="5">
                                                            &nbsp;<input id="txtEmailAdd" maxlength="200" style="width: 52%" type="text" value="" />*</td>
                                                    </tr>
                                                    
                                                    
                                                    <tr>
                                                    <td colspan="6"></td>
                                                        <%--<td id="Td10" class="table_caption" colspan="6" style="color: white; text-indent: 5px;
                                                    text-align: left">
                                                ID&nbsp;</td>--%>
                                                    </tr>
                                                     <tr>
                                                    <td colspan="6"></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            ID No:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan="2">
                                                            &nbsp;<input id="txtIDNo" maxlength="200" style="width: 89%" type="text" value="" />*</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            ID Type:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan="2">
                                                            <select id="ddIDType" style="width: 94%">
                                                            </select>
                                                            *</td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            ID Expiry Date:<br />
                                                            MM/DD/YYYY:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan="2">
                                                            &nbsp;<input id="txtIDExpiry" maxlength="200" style="width: 86%; " type="text" value="" />
                                                            <img id="Img1" align="absMiddle" onclick="javascript:if(document.getElementById('txtIDExpiry').disabled == false)
                                                        {
                                                            popUpCalendar(this, document.getElementById('txtIDExpiry'), 'mm/dd/yyyy', document.getElementById('Main'));
                                                        }" src="../images/calendar/imgcalen.png" />*</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            State of Issuance:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red" colspan="2">
                                                            &nbsp;<select id="ddlStateIssuance" style="width: 92%"></select>
                                                            *</td>
                                                    </tr>
                                                     <tr>
                                                    <td colspan="6"></td>
                                                    </tr>
                                                     <tr>
                                                    <td colspan="6"></td>
                                                    </tr>
                                                    <tr>
                                                        <%--<td id="Td5" class="table_caption" colspan="6" style="color: white; text-indent: 5px;
                                                    text-align: left">
                                                HOME ADDRESS&nbsp;</td>--%>
                                                    </tr>
                                                    <tr>
                                                        
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Street No.:&nbsp;</td>
                                                        <td align="left" style="width: 23%; color:Red; height: 24px" colspan=2>
                                                            &nbsp;<input id="txtStreet" maxlength="200" style="width: 89%" type="text" value="" />*</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Room No/ Bldg.:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px" colspan=2>
                                                            &nbsp;<input id="txtRoomNo" maxlength="200" style="width: 89%" type="text" /></td>    
                                                        <td class="cell_caption" style="width: 10%; height: 24px; display:none">
                                                            PO Box:&nbsp;</td>
                                                        <td align="left" style="width: 24%; height: 24px; display:none">
                                                            &nbsp;<input id="txtPOBox" maxlength="200" style="width: 92%" type="text" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            City:&nbsp;</td>
                                                        <td align="left" style="width: 23%; color:Red; height: 24px" colspan="2">
                                                            &nbsp;<input id="txtCity" maxlength="200" style="width: 89%" type="text" value="" />*</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px" >
                                                            State:&nbsp;</td>
                                                        <td align="left" style="width: 23%; color:Red; height: 24px" colspan="2">
                                                            &nbsp;<select id="ddState" style="width: 92%"></select>*</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px; display:none">
                                                            Region:&nbsp;</td>
                                                        <td align="left" style="width: 24%; height: 24px; display:none">
                                                            &nbsp;<input id="txtRegion" maxlength="200" style="width: 92%" type="text" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Country:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px" colspan="2">
                                                            &nbsp;<select id="ddCountry" style="width: 92%"></select></td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Zip Code:&nbsp;</td>
                                                        <td align="left" style="width: 23%; color:Red; height: 24px; " colspan="2">
                                                            &nbsp;<input id="txtZipCode" maxlength="200" style="width: 89%" type="text" value=""  />*</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px;display:none">
                                                            District:&nbsp;</td>
                                                        <td align="left" style="width: 24%; height: 24px; display:none">
                                                            &nbsp;<input id="txtDistrict" maxlength="200" style="width: 92%" type="text" /></td>
                                                    </tr>
                                                    <%--<tr>
                                            <td id="Td6" class="table_caption" colspan="6" style="color: white; text-indent: 5px;
                                                    text-align: left">
                                                COMPANY ADDRESS&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Room No./Bldg.:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px" >
                                                &nbsp;<input id="txtRoomNo_Com"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                No/Street:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtStreet_Com"  maxlength="200" style="width: 90%"
                                                        type="text" /></td>                                                        
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                PO Box:&nbsp;</td>
                                            <td align="left" style="width: 24%; height: 24px">
                                                &nbsp;<input id="txtPOBox_Com"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                City:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtCity_Com"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                State:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtState_Com"  maxlength="200" style="width: 90%"
                                                        type="text" /></td>                                                        
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Region:&nbsp;</td>
                                            <td align="left" style="width: 24%; height: 24px">
                                                &nbsp;<input id="txtRegion_Com"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Country:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<select id="ddCountry_Com" style="width: 92%"></select></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Zip Code:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtZipCode_Com"  maxlength="200" style="width: 90%"
                                                        type="text" /></td>                                                        
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                District:&nbsp;</td>
                                            <td align="left" style="width: 24%; height: 24px">
                                                &nbsp;<input id="txtDistrict_Com"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                        </tr>
                                        <tr>
                                            <td id="Td7" class="table_caption" colspan="6" style="color: white; text-indent: 5px;
                                                    text-align: left">
                                                MAILING ADDRESS&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Room No./Bldg.:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px" >
                                                &nbsp;<input id="txtRoomNo_Mail"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                No/Street:&nbsp;</td>
                                            <td align="left" style="width: 21%; height: 24px">
                                                &nbsp;<input id="txtStreet_Mail"  maxlength="200" style="width: 90%"
                                                        type="text" /></td>                                                        
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                PO Box:&nbsp;</td>
                                            <td align="left" style="width: 24%; height: 24px">
                                                &nbsp;<input id="txtPOBox_Mail"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                City:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtCity_Mail"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                State:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtState_Mail"  maxlength="200" style="width: 90%"
                                                        type="text" /></td>                                                        
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Region:&nbsp;</td>
                                            <td align="left" style="width: 24%; height: 24px">
                                                &nbsp;<input id="txtRegion_Mail"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Country:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<select id="ddCountry_Mail" style="width: 92%"></select></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Zip Code:&nbsp;</td>
                                            <td align="left" style="width: 23%; height: 24px">
                                                &nbsp;<input id="txtZipCode_Mail"  maxlength="200" style="width: 90%"
                                                        type="text" /></td>                                                        
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                District:&nbsp;</td>
                                            <td align="left" style="width: 24%; height: 24px">
                                                &nbsp;<input id="txtDistrict_Mail"  maxlength="200" style="width: 92%"
                                                        type="text" /></td>
                                        </tr>--%>
                                                    <tr>
                                                        <%-- <td id="Td8" class="table_caption" colspan="6" style="color: white; text-indent: 5px;
                                                    text-align: left">
                                                CONTACTS&nbsp;</td>--%>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            &nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px; color: Red">
                                                            &nbsp;</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                        </td>
                                                        <td align="left" style="width: 23%; height: 24px;" class="comment">
                                                            &nbsp;</td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                        </td>
                                                        <td align="left" style="width: 24%; height: 24px; color: Red">
                                                            &nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <%-- <td id="Td9" class="table_caption" colspan="6" style="color: white; text-indent: 5px;
                                                    text-align: left">
                                                EMPLOYMENT&nbsp;</td>--%>
                                                    </tr>
                                                    <tr style="display:none;">
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Employer's Name:&nbsp;</td>
                                                        <td align="left" style="width: 57%; height: 24px" colspan="3">
                                                            &nbsp;<input id="txtEmpName" maxlength="200" style="width: 96%" type="text" /></td>
                                                        <td class="cell_caption" style="width: 10%; height: 24px">
                                                            Occupation:&nbsp;</td>
                                                        <td align="left" style="width: 23%; height: 24px">
                                                            &nbsp;<select id="ddOccupation" style="width: 94%"></select></td>
                                                    </tr>
                                                    <%--<tr>
                                            <td class="cell_caption" style="width: 15%; height: 24px">
                                                Employer's Address:&nbsp;</td>
                                            <td align="left" style="width: 45%; height: 24px" colspan="3">
                                                &nbsp;<input id="txtEmpAddress"  maxlength="200" style="width: 96%"
                                                        type="text" /></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Branch Code:&nbsp;</td>
                                            <td align="left" style="width: 371px; height: 24px; color: Red">
                                                &nbsp;<select id="ddBranchCode" style="width: 94%"></select>*</td>
                                        </tr>   --%>
                                                    <%--                                        <tr>
                                            <td class="cell_caption" style="width: 15%; height: 24px">
                                                Purpose of Remittance:&nbsp;</td>
                                            <td align="left" style="width: 45%; height: 24px" colspan="3">
                                                &nbsp;<input id="txtPurpose"  maxlength="200" style="width: 98%"
                                                        type="text" /></td>
                                            <td class="cell_caption" style="width: 10%; height: 24px">
                                                Relationship to Recipient:&nbsp;</td>
                                            <td align="left" style="width: 371px; height: 24px">
                                                &nbsp;<input id="txtRelationship"  maxlength="200" style="width: 94%"
                                                        type="text" /></td>
                                        </tr> --%>
                                                </table>
                                            </div>
                                            <div id="divUpload" style="display: none;">
                                                <table id="tblUpload" cellpadding="0" cellspacing="5" style="border-right: #394398 1px solid;
                                                    border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid;
                                                    border-bottom: #394398 1px solid; width: 100%;">
                                                    <tr>
                                                        <td id="Td1" class="table_caption" colspan="5" style="color: white; text-indent: 5px;
                                                            text-align: left">
                                                            Supporting Document Upload&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 9%; height: 24px">
                                                            File 1 :&nbsp;</td>
                                                        <td align="left" style="width: 51%; height: 24px">
                                                            &nbsp;<input id="File1" runat="server" type="file" style="width: 68%" />
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <td class="cell_caption" style="width: 9%; height: 24px">
                                                            File 2 :&nbsp;</td>
                                                        <td align="left" style="width: 51%; height: 24px">
                                                            &nbsp;<input id="File2" runat="server" type="file" style="width: 68%" />
                                                        </td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <td class="cell_caption" style="width: 9%; height: 24px">
                                                            File 3 :&nbsp;</td>
                                                        <td align="left" style="width: 51%; height: 24px">
                                                            &nbsp;<input id="File3" runat="server" type="file" style="width: 68%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <div id="GRID1" style="text-align: center;">
                                                <%-- <table id="Table16" cellpadding="0" cellspacing="5" style="display:none; border-right: #394398 1px solid;
                                            border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid;
                                            border-bottom: #394398 1px solid" width="100%">
                                            <tr>
                                                <td id="Td1" class="table_caption" colspan="6" style="color: black; text-indent: 5px;
                                                    text-align: left">
                                                    REMITTER’S INSTRUCTION&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%; height: 24px">
                                                    &nbsp;<input id="rdGFCA" style="width: 96%" type="radio" title="prod" /></td>
                                                <td align="left" style="width: 19%; height: 24px" colspan="5">
                                                    &nbsp;<strong>Apply for a Global Filipino Money Card (GFMC) to be claimed by the beneficiary</strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%; height: 24px">
                                                    &nbsp;<input id="rdCTA" style="width: 96%" type="radio" title="prod" /></td>
                                                <td align="left" style="width: 19%; height: 24px" colspan="5">
                                                    &nbsp;<strong>For deposit to PNB Account No:</strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%; height: 24px">
                                                    &nbsp;<input id="rdGFC" style="width: 96%" type="radio" title="prod" /></td>
                                                <td align="left" style="width: 19%; height: 24px" colspan="5">
                                                    &nbsp;<strong>For deposit to GFMC No:</strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%; height: 22px">
                                                    &nbsp;<input id="rdAPAS" style="width: 96%" type="radio" title="prod" /></td>
                                                <td align="left" style="width: 19%; height: 22px" colspan="5">
                                                    &nbsp;<strong>For pick-up at PNB counters or payout partners</strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%; height: 24px">
                                                    &nbsp;<input id="rdDTD" style="width: 96%" type="radio" title="prod" /></td>
                                                <td align="left" style="width: 19%; height: 24px" colspan="5">
                                                    &nbsp;<strong>Door to door delivery</strong></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 15%; height: 24px">
                                                    &nbsp;<input id="rdPDD" style="width: 96%" type="radio" title="prod" /></td>
                                                <td align="left" style="width: 19%; height: 24px" colspan="5">
                                                    &nbsp;<strong>Deposit to other bank (indicate bank information below)</strong></td>
                                            </tr>                                                                                                                                                                                                                                                                        
                                            <tr>
                                                <td class="cell_caption" style="width: 15%; height: 24px">
                                                    Bank's Name:&nbsp;</td>
                                                <td align="left" style="width: 19%; height: 24px">
                                                    &nbsp;<input id="txtProdBankName"  maxlength="200" style="width: 96%"
                                                            type="text" /></td>
                                                <td class="cell_caption" style="width: 13%; height: 24px">
                                                    Account No:&nbsp;</td>
                                                <td align="left" style="width: 19%; height: 24px">
                                                    &nbsp;<input id="txtProdAcctNo"  maxlength="200" style="width: 94%"
                                                            type="text" /></td>                                                        
                                                <td class="cell_caption" style="width: 14%; height: 24px">
                                                    Branch:&nbsp;</td>
                                                <td align="left" style="width: 428px; height: 24px">
                                                    &nbsp;<input id="txtProdBranchCode"  maxlength="200" style="width: 94%"
                                                            type="text" /></td>
                                            </tr>                                            
                                            
                                        </table>--%>
                                                <%--                                        <strong><span style="font-size: 14pt">(Company Use Only)<br />
                                        </span></strong>--%>
                                                <%--<table id="Table1" cellpadding="0" cellspacing="5" style="display:none; border-right: #394398 1px solid;
                                            border-top: #394398 1px solid; background: #FFFFFF; border-left: #394398 1px solid;
                                            border-bottom: #394398 1px solid; " width="100%">
                                            <tr>
                                                <td class="table_caption" style="width: 4%; height: 24px; color:Black; text-align: center;" colspan="2">
                                                    Type of Identification (except OHIP)&nbsp;</td>
                                                <td class="table_caption" style="width: 4%; height: 24px; color:Black; text-align: center;" colspan="2">
                                                    ID Number</td>
                                                <td class="table_caption" style="width: 4%; height: 24px; color:Black; text-align: center;" colspan = "2">
                                                    Place Of Issue/Expiration Date</td>
                                            </tr>                                            
                                            <tr>
                                                <td class="cell_caption" style="width: 4%; height: 24px; text-align: left;" colspan="2">
                                                    &nbsp;
                                                    Driver's License:&nbsp;</td>
                                                <td style="width: 4%; height: 24px" colspan="2"><input id="txtRemID1"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                                <td style="width: 4%; height: 24px" colspan = "2"><input id="txtRemID1Expiry"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 4%; height: 24px; text-align: left;" colspan="2">
                                                    &nbsp; Passport:&nbsp;</td>
                                                <td style="width: 4%; height: 24px" colspan="2"><input id="txtRemID2"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                                <td style="width: 4%; height: 24px" colspan = "2"><input id="txtRemID2Expiry"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 4%; height: 24px; text-align: left;" colspan="2">
                                                    &nbsp; Citizenship Card:&nbsp;</td>
                                                <td style="width: 4%; height: 24px" colspan="2"><input id="txtRemID3"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                                <td style="width: 4%; height: 24px" colspan = "2"><input id="txtRemID3Expiry"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 4%; height: 24px; text-align: left;" colspan="2">
                                                    &nbsp; Permanent Resident Card:&nbsp;</td>
                                                <td style="width: 4%; height: 24px" colspan="2"><input id="txtRemID4"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                                <td style="width: 4%; height: 24px" colspan = "2"><input id="txtRemID4Expiry"  maxlength="200" style="width: 98%"
                                                            type="text" /></td>
                                            </tr>                                                                                                                                                                                

                                        </table>--%>
                                                <table id="tbl_btn_submit" cellpadding="0" cellspacing="5px" style="background: #FFFFFF;
                                                    width: 100%">
                                                    <tr>
                                                        <td align="center" colspan="3" style="width: 100%;">
                                                            <table width="70%" cellpadding="0" cellspacing="5px" id="Table9">
                                                                <tr>
                                                                    <td style="width: 18%; height: 22px;">
                                                                        <input id="btnSubmit" class="button" onmouseover="this.className='button_over';"
                                                                            onmouseout="this.className = 'button';" type="button" value="Submit" style="width: 200px"
                                                                            onclick="javascript:IRE.btnSubmit_click();" /></td>
                                                                    <td style="width: 18%; height: 22px;">
                                                                        <input id="btnACancel" class="button" onmouseover="this.className='button_over';"
                                                                            onmouseout="this.className = 'button';" type="button" value="Cancel" style="width: 200px"
                                                                            onclick="javascript:IRE.btnACancel_click();" /></td>
                                                                    <td style="width: 18%; height: 22px;">
                                                                        <input id="btnGoBack" class="button" onmouseover="this.className='button_over';"
                                                                            onmouseout="this.className = 'button';" type="button" value="Go Back to Login Page"
                                                                            style="width: 200px" onclick="document.irs_login.submit();" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <%-- <table id="tbl_btn_upload" width="100%" cellpadding="0" cellspacing="5px" style="background:#FFFFFF;">
                                            <tr>
                                                <td align="center" colspan="3" style="width: 100%; height: 51px;">                                                                                                    
                                                    <table width="70%" cellpadding="0" cellspacing="5px" id = "Table4">
                                                        <tr>
                                                            <td style="width: 18%; height: 22px;"></td>
                                                            <td style="width: 27%; height: 22px;"><input id="btnUpload"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Upload" style="width: 200px" onclick="javascript:IRE.btnbtnUpload_click();"/></td>                                                                                                                                                                                                                                                                    
                                                        </tr>
                                                    </table>                                                                                                    
                                                </td>
                                            </tr>                                                                    
                                        </table>           --%>
                                            </div>
                                            <asp:HiddenField ID="HIPAdd" runat="server" Value="123" />
                                            <asp:HiddenField ID="HPath" runat="server" Value="123" />
                                            <input id="btnInsertUserRec" class="button" onmouseover="this.className='button_over';"
                                                onmouseout="this.className = 'button';" type="button" value="knd0025" style="width: 60px;
                                                display: none;" onclick="javascript:IRE.maint_save_click();" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <div id="divPopProcess" style="height: 200px; background: White; z-index: 75; position: absolute;
                left: 1072px; color: darkblue; padding: 10px; display: none; font-family: tahoma;
                top: 12px;">
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
        </form>
    </center>
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx"
        target="_top">
    </form>
</body>
</html>
