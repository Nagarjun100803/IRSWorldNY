<%@ Page Language="C#" AutoEventWireup="true" Codebehind="OBPSRemitter.aspx.cs" Inherits="IRSWorld.BillsPayment.OBPSRemitter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>OBPS-Remitter</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/jsCommons.js"                            type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                       type="text/javascript"></script>
    <script src="../js/js_sidebar.js"                           type="text/javascript"></script>
    <script src="../js/BillsPayment/jsBillsPayOBPSRemitter.js"  type="text/javascript"></script>
    
    <script src="../js/popcalendar.js"                          type="text/javascript"></script>

    <script type="text/javascript">
        var BillsPayOBPSRemitter = new BillsPayOBPSRemitter();
    
        var isShift = false;
        
        function keyUP(keyCode) 
	    { 
		    if(keyCode==16)
				    isShift = false; 
	    } 

        function validate(keyCode,ValidationType)
        {
            if (ValidationType=='isAlpha')
                return  (keyCode == 8 || keyCode == 9 || keyCode == 32 || (keyCode >= 65 && keyCode <= 90))
            else if (ValidationType=='isNumeric')
            {
                if(keyCode==16)
                    isShift = true;	
                return ((keyCode >= 48 && keyCode <= 57 && isShift == false)|| keyCode == 8 || keyCode == 9 || (keyCode >= 96 && keyCode <= 105)  ) 
            }
            else if (ValidationType=='isAlphaNumeric')
            {
                if(keyCode==16)
                    isShift = true;
                return ((keyCode >= 48 && keyCode <= 57 && isShift == false)|| keyCode == 8 || keyCode == 9 || keyCode == 32 || (keyCode >= 96 && keyCode <= 105) || (keyCode >= 65 && keyCode <= 90)  ) 
            }               
        }


	  

    </script>

</head>
<body onload="javascript:BillsPayOBPSRemitter.load_all_dropdownlist();BillsPayOBPSRemitter.construct_side_bar('divtdBillsPayOBPSRemitterMenu');">
    <form id="form1" runat="server">
        <div style="width: 100%">
            <table style="width:100%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width: 100%; border-collapse: collapse; height: 380px;"
                        class="content_height" colspan="2">
                        <div id="divBillsPayOBPSRemitter" style="width: 100%; height: 100%; display: block">
                            <table style="width:100%">
                                <tr>
                                    <td id="BillsPayOBPSRemitterMenu" valign="top" style="width: 20%; vertical-align: top;
                                        padding-top: 30px;">
                                        <div id="divtdBillsPayOBPSRemitterMenu" style="margin-top: 20px; display: block;">

                                            <script type="text/javascript">
//                                            var sider = new js_sidebar('sidemenu', sidebar);
//                                            document.write(sider.toString());
                                            </script>

                                        </div>
                                        <a id="ahider" href="#" onclick="javascript:BillsPayOBPSRemitter.set_side_bar_visibility('divtdOBPSRRemitterMenu','ahider');">
                                            «
                                        </a>
                                    </td>
                                    <td style="width: 78%; padding-top: 30px">
                                        <table cellspacing="0" cellpadding="0" style="width:96%; background-color: Azure">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                    <img src="../images/toe.GIF" /></td>
                                                <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">
                                                    OBPS - Remitter Walkin</td>
                                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                    <img align="absbottom" src="../images/rend.gif" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table style="width: 100%; height: 100%;" class="table_border">
                                                        <tr>
                                                            <td style="width: 100%; height: 21px;">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                                <!-- content starts here -->
                                                                <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%;" valign="center">
                                                                            <div style="width: 98%; height: 100%; margin: 0" class="table_body" id="divBillsPayOBPSRemitterInfo">
                                                                                <table id="tblBillsPayOBPSRemitter" cellpadding="0" cellspacing="8"
                                                                                    style="width:100%; border: solid 1px #336699;">
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Last Name :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="5" align="left" style="width: 85%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterLastName" type="text" style="width: 100%; font-weight: normal;" 
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            First Name :&nbsp;</td>
                                                                                        <td colspan="5" align="left" style="width: 85%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterFirstName" type="text" style="width: 100%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Middle Name :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="2" align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterMiddleName" type="text" style="width: 98%; font-weight: normal;" 
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);"/>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Alias :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="2" align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterAlias" type="text" style="width: 100%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Title :&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 20%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterTitle" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 11%; height: 23px">
                                                                                            Civil Status :&nbsp;</td>
                                                                                        <td style="width: 15%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterCivilStatus" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Gender :&nbsp;</td>
                                                                                        <td style="width: 15%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterGender" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Birthdate :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 20%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterBirthdate" type="text" style="width:80%; font-weight: normal;" 
                                                                                              readonly="readonly"
                                                                                              onchange="javascript:BillsPayOBPSRemitter.compute_age();" />
                                                                                              
                                                                                            <img src="../images/calendar/imgcalen.png" alt="Pick a date." 
                                                                                                onclick="popUpCalendar(this, document.forms[0].txtBillsPayOBPSRemitterBirthdate, 'mm/dd/yyyy');"/>
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 11%; height: 23px">
                                                                                            Age :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 15%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterAge" type="text" style="width: 100%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isNumeric');"
                                                                                            readonly="readonly"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Birthplace :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 15%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterBirthplace" type="text" style="width: 100%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Religion :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 20%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterReligion" style="width: 97.369%; font-weight: normal;" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 11%; height: 23px">
                                                                                            Citizenship :&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterCitizenship" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Occupation Type :&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterOccupationType" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Contact Nos :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 20%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterContactNos" type="text" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 11%; height: 23px">
                                                                                            Email :&nbsp;
                                                                                        </td>
                                                                                        <td align="left" style="width: 15%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterEmail" type="text" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Worker Type :&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 15%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterWorkerType" style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 24px">
                                                                                            Room No/Bldg :&nbsp;</td>
                                                                                        <td colspan="5" align="left" style="width: 87%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterRoomNo" type="text" style="width: 99.3%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Street No/Name :&nbsp;</td>
                                                                                        <td colspan="5" align="left" style="width: 87%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterStreetNo" type="text" style="width: 99.3%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            District :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="2" align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterDistrict" type="text" style="width: 100%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            City :&nbsp;</td>
                                                                                        <td colspan="2" align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterCity" type="text" style="width: 100%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            State :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="2" align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterState" type="text" style="width: 100%; font-weight: normal;"
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            Region :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="2" align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterRegion" type="text" style="width: 100%; font-weight: normal;" 
                                                                                            onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                            onkeyup="javascript:keyUP(event.keyCode);"/>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Country :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="2" style="width: 18%; height: 24px;">
                                                                                            <select id="ddlBillsPayOBPSRemitterCountry" style="width: 100%; font-weight: normal;"  />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 15%; height: 23px">
                                                                                            ZipCode :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="2" align="left" style="width: 35%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterZipCode" type="text"  style="width: 100%; font-weight: normal;" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 14%; height: 23px">
                                                                                            Remarks :&nbsp;
                                                                                        </td>
                                                                                        <td colspan="5" align="left" style="width: 85%; height: 24px;">
                                                                                            <input id="txtBillsPayOBPSRemitterRemarks" type="text" style="width: 100%; font-weight: normal;" 
                                                                                                onkeydown ="javascript:return validate(event.keyCode,'isAlphaNumeric');"
                                                                                                onkeyup="javascript:keyUP(event.keyCode);"/>
                                                                                        </td>
                                                                                        </tr>
                                                                                </table>
                                                                            </div>
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="height: 10px">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 28px; width: 100%" valign="top">
                                                                            <input style="height: 28px; width: 10%" type="button" value="Clear" class="button"
                                                                                onclick="javascript:BillsPayOBPSRemitter.clear_field();" 
                                                                                onmouseover="this.className='button_over';"
                                                                                onmouseout="this.className='button';" />
                                                                            <input style="height: 28px; width: 10%" id="btnSave" type="button" value="Save" class="button" onclick="javascript:BillsPayOBPSRemitter.btn_save();" 
                                                                                onmouseover="this.className='button_over';"
                                                                                onmouseout="this.className='button';" />
                                                                            <input style="height: 28px; width: 10%" type="button" value="Cancel" class="button"
                                                                                onmouseover="this.className='button_over';" 
                                                                                onmouseout="this.className='button';"
                                                                                onclick="javascript:close_frame_dialog();" />
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
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
