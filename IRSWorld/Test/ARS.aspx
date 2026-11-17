<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ARS.aspx.cs" Inherits="IRSWorld.Test.ARS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ARS API</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />

    <script src="../js/jsCommons.js" type="text/javascript"></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jshttprequest.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/Test/js_ARSAPI.js" type="text/javascript"></script>

    <script type="text/javascript">
            var ARS_API = new ARS_API();
        
        function fixdim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmain');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                var td = document.getElementById("tdmain");
                td.style.height = div.style.height;
                td.style.width = div.style.width;
            
                /*
                div = document.getElementById('divARSAPIMain');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPIRBDENROLL');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPIBENENROLL');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPIINFOAMEND');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPINEWREMIT');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPICHGPWD');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPICHKENROLL');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPICHKBENE');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPICHKAMEND');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPICHKREMIT');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                
                div = document.getElementById('divBodyARSAPIRBDREMIT');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
                */
            }
        }
    </script>

</head>
<body onload="fixdim(); javascript:ARS_API.load();" onresize="fixdim();">
    <center>
        <form id="form1" runat="server">
        <div id="divmain" style="margin:0 auto; width:100%; overflow:auto; padding-top: 30px;">
        <table style="width:100%" cellpadding=0 cellspacing=0>
            <tr>
            <td id=tdmain  align=center  style="margin:0 auto; width:100%; vertical-align:top;">
            <div id="divARSAPIMain" style="width: 65%; display: none; vertical-align: top;">
                <table id="tblARSAPIMain" style="width: 60%; display: block" cellspacing=0 cellpadding=0>
                    <tr>
                        <td align="left" valign="bottom" style="vertical-align:bottom; width: 0%; height: 24px; border-collapse: collapse">
                            <img align="absBottom" src="../images/toe.GIF" /></td>
                        <td id="td1" class="table_head_dark" style="vertical-align:bottom; width: 60%; height: 24px">
                            AGENT REMITTANCE SYSTEM API</td>
                        <td align="left" style="width: 40%; height: 24px;vertical-align:bottom; " valign="bottom">
                            <img align="absBottom" src="../images/rend.gif" /></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                            class="table_body" align="center">
                            <br />
                            <div id="divKeyword" style="width: 95%; display: block">
                                <table id="tblKeyWord" cellpadding="0" cellspacing="5" style="width:95%">
                                    <tr>
                                        <td class="cell_caption" style="width: 40%; height: 23px">
                                            &nbsp; Keyword :&nbsp;</td>
                                        <td align="left" style="width: 60%; height: 24px;">
                                           <%--<input id="txtKeyword" style="width: 98%" />--%>
                                            <select <%--onclick="javascript:Branch_Operation.branch();"--%> id="ddlKeyword" style="width: 100%">
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                                <input id="txtHiddenSeqIdKeyword" type="hidden" />
                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%" align="center">
                                            <table id="tblKeywordButton" cellpadding="0" cellspacing="5">
                                                <tr>
                                                    <td style="width: 20%">
                                                        <input id="btnKeywordOK" class="button" onclick="javascript:ARS_API.keyword();" onmouseout="this.className='button';"
                                                            onmouseover="this.className='button_over';" type="button" value="OK" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp</td>
                    </tr>
                </table>
            </div>
            <div id="divBodyARSAPIRBDENROLL" style="width: 85%; height:auto; display: none; vertical-align: top;">
                <table id="tblBodyARSAPIRBDENROLL" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPIRBDENROLL" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPIRBDENROLL" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:85%; vertical-align: bottom; text-align: left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 11px; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="tdPageTitle" class="table_head_dark" style="width: 80%; height: 24px">
                                                        RBDENROLL - REMITTER/BENEFICIARY ENROLLMENT</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center">
                                            <br />
                                            <div id="divARSAPIRBDENROLLMain" style="width: 95%; display: block">
                                                <table id="tblARSAPIRBDENROLLMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 28%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 22%; height: 24px;">
                                                            <input id="txtRBDENROLLUserId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            &nbsp; * Remitter Country Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterCountryCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtRBDENROLLPassword" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter Occupation :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterOccupation" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Agent TransactionRefNo : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLAgentTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter ID1 :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterID1" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent RemitterCustomerId: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLAgentRemitterCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Remitter ID2 :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterID2" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent BeneficiaryCustomerId : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLAgentBeneCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; * Beneficiary FirstName:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneFirstName" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Remitter FirstName : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterFirstName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary MiddleName :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneMiddleName" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Remitter MiddleName: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterMiddleName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; * Beneficiary LastName :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneLastName" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Remitter LastName:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterLastName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary BirthDate :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneBirthDate" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp;Remitter BirthDate:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterBitrthdate" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary BirthPlace :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneBirthplace" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Remitter BirthPlace:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterBirthplace" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 24px">
                                                            &nbsp;Beneficiary Mobile No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneMobileNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 24px">
                                                            &nbsp;Remitter MobileNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterMobileNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            Beneficiary Contact No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneContactNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            *
                                                            Remitter Contact No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterContactNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary Rm Bldg :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneRmBldg" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter Rm Bldg :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterRmBldg" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Beneficiary Street :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneStreet" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter Street :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterStreet" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary District :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneDistrict" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter District :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterDistrict" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            Beneficiary State :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneState" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter State :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterState" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            *
                                                            &nbsp;Beneficiary City :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneCity" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter City :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterCity" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            Beneficiary Region :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneRegion" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter Region :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterRegion" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Beneficiary ZipCode :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneZipCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Remitter ZipCode :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterZipCode" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary Email :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneEmail" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Remitter Email :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLRemitterEmail" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            *
                                                            &nbsp;Beneficiary Country Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDENROLLBeneCountryCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <input id="txtRBDENROLLHiddenSeqID" type="hidden" />
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" align="center">
                                                        <table id="tblRBDENROLLbuttons" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input id="btnRBDENROLLOK" class="button" onclick="javascript:ARS_API.rbdenroll_commit_record();"
                                                                        type="button" value="OK" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnRBDENROLLCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 11px">
                                            &nbsp</td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divBodyARSAPIBENENROLL" style="width: 85%; height:400px; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPIBENENROLL" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPIBENENROLL" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPIBENENROLL" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:60%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td6" class="table_head_dark" style="width: 40%; height: 24px">
                                                        BENENROLL - BENEFICIARY ENROLLMENT</td>
                                                    <td align="left" style="width: 60%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center"> <!--colspan="3"-->
                                            <br />
                                            <div id="divARSAPIBENENROLLMain" style="width: 95%; display: block">
                                                <table id="tblARSAPIBENENROLLMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 28%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLUSerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Beneficiary Contact No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneContactNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtBENENROLLPassword" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Remitter Country Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLRemitterCountryCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Agent TransactionRefNo : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLAgentTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary Rm Bldg :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneRmBldg" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent RemitterCustomerId: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLAgentRemitterCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Beneficiary Street :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneStreet" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent BeneficiaryCustomerId : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLAgentBeneCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary District :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneDistrict" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Beneficiary FirstName : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneFirstName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary State:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneState" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Beneficiary MiddleName: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneMiddleName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* &nbsp;Beneficiary City :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneCity" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Beneficiary LastName:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneLastName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary Region :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneRegion" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp;Beneficiary BirthDate:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneBirthDate" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Beneficiary ZipCode :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneZipCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Beneficiary BirthPlace:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneBirthPlace" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary Email :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneEmail" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 24px">
                                                            &nbsp;Beneficiary MobileNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneMobileNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 24px">
                                                            &nbsp;* Beneficiary Country Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtBENENROLLBeneCountryCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <input id="txtBENENROLLHiddenSeqID" type="hidden" />
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" align="center">
                                                        <table id="tblBENENROLLbuttons" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input id="btnBENENROLLOK" class="button" onclick="javascript:ARS_API.benenroll_commit_record();"
                                                                        type="button" value="OK" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnBENENROLLCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
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
            <div id="divBodyARSAPIINFOAMEND" style="width: 85%; height:400px; display: none; vertical-align: top;
                margin-top: 30px; ">
                <table id="tblBodyARSAPIINFOAMEND" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPIINFOAMEND" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPIINFOAMEND" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:85%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td5" class="table_head_dark" style="width: 80%; height: 24px">
                                                        INFOAMEND - REMITTER/BENEFICIARY AMENDMENT</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center">
                                            <br />
                                            <div id="divARSAPIINFOAMENDMain" style="width: 95%; display: block">
                                                <table id="tblARSAPIINFOAMENDMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDUserId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;Street :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDStreet" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input type="password" id="txtINFOAMENDPassword" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp; District :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDDistrict" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            * Agent TransactionRefNo : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDAgentTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;State :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDState" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp; * Agent CustomerId: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDAgentCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;City :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDCity" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp; * FirstName : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDFirstName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;Region :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDRegion" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp; MiddleName: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDMiddleName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp; * ZipCode:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDZipCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            * LastName:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDLastName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp; Email :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDEmail" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;BirthDate:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDBirthDate" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;* Country Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDCountryCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            BirthPlace:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDBirthPlace" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;Occupation :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDOccupation" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;MobileNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDMobileNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp; Remitter ID1 :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDRemitterID1" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;* ContactNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDContactNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;Remitter ID2 :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDRemitterID2" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 24px">
                                                            &nbsp;Rm Bldg :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 23px;">
                                                            <input id="txtINFOAMENDRmBldg" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                            <input id="txtINFOAMENDHiddenSeqID" type="hidden" />
                                            <table cellpadding="0" cellspacing="0" style="width:100%">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 100%" align="center">
                                                        <table id="tblINFOAMENDbuttons" cellpadding="0" cellspacing="5">
                                                            <tr>
                                                                <td style="width: 20%">
                                                                    <input id="btnINFOAMENDOK" class="button" onclick="javascript:ARS_API.infoamend_commit_record();"
                                                                        type="button" value="OK" /></td>
                                                                <td style="width: 20%">
                                                                    <input id="btnINFOAMENDCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
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
            <div id="divBodyARSAPINEWREMIT" style="width: 85%; height:400px; display: none; vertical-align: top;
                margin-top: 30px; ">
                <table id="tblBodyARSAPINEWREMIT" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPINEWREMIT" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPINEWREMIT" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:60%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td4" class="table_head_dark" style="width: 40%; height: 24px">
                                                        NEWREMIT - NEW REMITTANCE</td>
                                                    <td align="left" style="width:60%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center">
                                            <br />
                                            <div id="divARSAPINEWREMITMain" style="width: 95%; display: block">
                                                <table id="tblARSAPINEWREMITMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Account No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITAccountNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtNEWREMITPassword" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; GFMC Card No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITGFMCCardNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Agent TransactionRefNo : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITAgentTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Bank Name :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITBankName" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent RemitterCustomerId: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITAgentRemitterCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Source Amount :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITSourceAmount" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent BeneficiaryCustomerId : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITAgentBeneCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; * Source Currency :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITSourceCurrency" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Remitter FirstName : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITRemitterFirstName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; SourceToUSD :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITSourcetoUSD" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Remitter MiddleName: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITRemitterMiddleName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; SourceToPHP :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITSourceToPHP" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Remitter LastName:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITRemitterLastName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; USDToPHP :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITUSDToPHP" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Remitter Account No:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITRemitterAccountNo" style="width: 98%" />
                                                        </td>
                                                        
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Beneficiary FirstName:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITBeneFirstName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Service Charge :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITServiceCharge" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Beneficiary MiddleName:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITBeneMiddleName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Target Amount :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITTargetAmount" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Beneficiary LastName :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITBeneLastName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Target Currency :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITTargetCurrency" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Product Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITProductCode" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Remittance Purpose :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtNEWREMITRemittancePurpose" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtNEWREMITHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblNEWREMITbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnNEWREMITOK" class="button" onclick="javascript:ARS_API.newremit_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnNEWREMITCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPICHGPWD" style="width: 85%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPICHGPWD" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPICHGPWD" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPICHGPWD" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:60%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td7" class="table_head_dark" style="width: 40%; height: 24px">
                                                        CHGPWD - CHANGE PASSWORD</td>
                                                    <td align="left" style="width: 60%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center"><!--colspan="3"-->
                                            <br />
                                            <div id="divARSAPICHGPWDMain" style="width: 95%; display: block">
                                                <table id="tblARSAPICHGPWDMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHGPWDUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            &nbsp;* Agent TransactionRefNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHGPWDAgentTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHGPWDPassword" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            &nbsp;* New Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHGPWDNewPassword" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtCHGPWDHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblCHGPWDbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHGPWDOK" class="button" onclick="javascript:ARS_API.chgpwd_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHGPWDCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPICHKENROLL" style="width: 85%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPICHKENROLL" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPICHKENROLL" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPICHKENROLL" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:85%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td8" class="table_head_dark" style="width: 80%; height: 24px">
                                                        CHKENROLL - CHECK REM/BENE ENROLLMENT</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center"> <!--colspan="3"-->
                                            <br />
                                            <div id="divARSAPICHKENROLLMain" style="width: 95%; display: block">
                                                <table id="tblARSAPICHKENROLLMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKENROLLUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            &nbsp;* TransactionRefNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKENROLLTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHKENROLLPassword" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtCHKENROLLHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblCHKENROLLbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKENROLLOK" class="button" onclick="javascript:ARS_API.chkenroll_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKENROLLCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPICHKBENE" style="width: 85%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPICHKBENE" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPICHKBENE" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPICHKBENE" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:85%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td9" class="table_head_dark" style="width: 80%; height: 24px">
                                                        CHKBENE - CHECK BENEFICIARY ENROLLMENT</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center"> <!-- colspan="3"-->
                                            <br />
                                            <div id="divARSAPICHKBENEMain" style="width: 95%; display: block">
                                                <table id="tblARSAPICHKBENEMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKBENEUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            &nbsp;* TransactionRefNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKBENETransactionRefNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHKBENEPassword" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtCHKBENEHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblCHKBENEbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKBENEOK" class="button" onclick="javascript:ARS_API.chkbene_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKBENECancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPICHKAMEND" style="width: 85%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPICHKAMEND" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPICHKAMEND" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPICHKAMEND" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                    <td style="width:85%; vertical-align:bottom; text-align:left">
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                            <img src="../images/toe.GIF" /></td>
                                        <td id="td10" class="table_head_dark" style="width: 80%; height: 24px">
                                            CHKAMEND - CHECK REM/BENE AMENDMENT</td>
                                        <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                            <img align="absBottom" src="../images/rend.gif" /></td>
                                    </tr>
                                    </table>
                                    </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center"> <!-- colspan="3"-->
                                            <br />
                                            <div id="divARSAPICHKAMENDMain" style="width: 95%; display: block">
                                                <table id="tblARSAPICHKAMENDMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKAMENDUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            &nbsp;* TransactionRefNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKAMENDTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHKAMENDPassword" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtCHKAMENDHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblCHKAMENDbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKAMENDOK" class="button" onclick="javascript:ARS_API.chkamend_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKAMENDCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPICHKREMIT" style="width: 85%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPICHKREMIT" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPICHKREMIT" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPICHKREMIT" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:85%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td11" class="table_head_dark" style="width: 80%; height: 24px">
                                                        CHKREMIT - CHECK NEW REMITTANCE</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center"> <!-- colspan="3"-->
                                            <br />
                                            <div id="divARSAPICHKREMITMain" style="width: 95%; display: block">
                                                <table id="tblARSAPICHKREMITMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKREMITUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            &nbsp;* TransactionRefNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKREMITTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 25%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHKREMITPassword" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtCHKREMITHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblCHKREMITbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKREMITOK" class="button" onclick="javascript:ARS_API.chkremit_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKREMITCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPICHKRB" style="width: 85%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPICHKRB" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPICHKRB" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPICHKRB" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 85%; vertical-align: bottom; text-align:left">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td12" class="table_head_dark" style="width: 80%; height: 24px">
                                                        CHKRB - CHECK REM/BENE ENROLLMENT STATUS</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center">
                                            <br />
                                            <div id="divARSAPICHKRBMain" style="width: 95%; display: block">
                                                <table id="tblARSAPICHKRBMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKRBUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp;* Agent Remitter CustomerID :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKRBAgentRemitterCustomerID" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHKRBPassword" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp;* Agent Beneficiary CustomerID :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKRBAgentBeneCustomerID" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtCHKRBHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblCHKRBbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKRBOK" class="button" onclick="javascript:ARS_API.chkrb_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKRBCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPICHKONLINE" style="width: 85%; display: none; vertical-align: top;
                margin-top: 30px;">
                <table id="tblBodyARSAPICHKONLINE" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPICHKONLINE" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPICHKONLINE" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:85%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td13" class="table_head_dark" style="width: 80%; height: 24px">
                                                        CHKONLINE(PING) - CHECK CONNECTION STATUS</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center">
                                            <br />
                                            <div id="divARSAPICHKONLINEMain" style="width: 95%; display: block">
                                                <table id="tblARSAPICHKONLINEMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKONLINEUserId" style="width: 98%" /></td>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp;* Account No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtCHKONLINEAccountNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtCHKONLINEPassword" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtCHKONLINEHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblCHKONLINEbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKONLINEOK" class="button" onclick="javascript:ARS_API.chkonline_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnCHKONLINECancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            <div id="divBodyARSAPIRBDREMIT" style="width: 85%; height:400px; display: none; vertical-align: top;
                margin-top: 30px; ">
                <table id="tblBodyARSAPIRBDREMIT" style="width: 100%; display: block">
                    <tr>
                        <td id="tdBodyARSAPIRBDREMIT" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divARSAPIRBDREMIT" style="width: 100%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width:85%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="td14" class="table_head_dark" style="width: 80%; height: 24px">
                                                        RBDREMIT - REM/BENE ENROLLMENT AND REMITTANCE</td>
                                                    <td align="left" style="width: 20%; height: 24px" valign="bottom">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center"> <!-- colspan="3"-->
                                            <br />
                                            <div id="divARSAPIRBDREMITMain" style="width: 95%; display: block">
                                                <table id="tblARSAPIRBDREMITMain" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td class="cell_caption" style="width: 28%; height: 23px">
                                                            * UserId :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITUserId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary BirthDate :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneBirthDate" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Password :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input type="password" id="txtRBDREMITPassword" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary BirthPlace :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneBirthPlace" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            * Agent TransactionRefNo : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITAgentTransactionRefNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 24px">
                                                            &nbsp;Beneficiary Mobile No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneMobileNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent RemitterCustomerId: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITAgentRemitterCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            Beneficiary Contact No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneContactNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; * Agent BeneficiaryCustomerId : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITAgentBeneCustomerId" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Beneficiary Rm Bldg :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneRmBldg" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 24px">
                                                            &nbsp; * Remitter FirstName : &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterFirstName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 24px">
                                                            &nbsp; * Beneficiary Street:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneStreet" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp; Remitter MiddleName: &nbsp;
                                                        </td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterMiddleName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary District :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneDistrict" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp;* Remitter LastName:</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterLastName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary State :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneState" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            &nbsp;Remitter BirthDate:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterBirthDate" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; * Beneficiary City :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneCity" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 23px">
                                                            Remitter BirthPlace:&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterBirthPlace" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary Region :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneRegion" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 30%; height: 24px">
                                                            &nbsp;Remitter MobileNo :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterMobileNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; * Beneficiary ZipCode :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneZipCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            *
                                                            Remitter Contact No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterContactNo" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp; Beneficiary Email :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneEmail" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter Rm Bldg :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterRmBldg" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            *&nbsp; Beneficiary Country Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneCountryCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter Street :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterStreet" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* ProductCode :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITProductCode" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter District :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterDistrict" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Account No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITAccountNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter State :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterState" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            GFMC Card No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITGFMCCardNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter City :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterCity" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Bank Name :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBankName" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Remitter Region :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterRegion" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            Source Amount :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITSourceAmount" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Remitter ZipCode :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterZipCode" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Source Currency :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITSourceCurrency" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Remitter Email :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterEmail" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Source To USD :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITSourceToUSD" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            *&nbsp; Remitter Country Code :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterCountryCode" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Source to PHP :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITSourceToPHP" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Remitter Occupation :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterOccupation" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;USD To PHP :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITUSDToPHP" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Remitter ID1 :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterID1" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;Service Charge :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITServiceCharge" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Remitter ID2 :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterID2" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Target Amount :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITTargetAmount" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                     <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Remitter Account No :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemitterAccountNo" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;* Beneficiary FirstName :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneFirstName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Target Currency :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITTargetCurrency" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;Beneficiary MiddleName :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneMiddleName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Remittance Purpose :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITRemittancePurpose" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 22%; height: 23px">
                                                            &nbsp;* Beneficiary LastName :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITBeneLastName" style="width: 98%" />
                                                        </td>
                                                        <td class="cell_caption" style="width: 20%; height: 23px">
                                                            &nbsp;* Transaction Type :&nbsp;</td>
                                                        <td align="left" style="width: 25%; height: 24px;">
                                                            <input id="txtRBDREMITTransactionType" style="width: 98%" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtRBDREMITHiddenSeqID" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblRBDREMITbuttons" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnRBDREMITOK" class="button" onclick="javascript:ARS_API.rbdremit_commit_record();"
                                                                            type="button" value="OK" /></td>
                                                                    <td style="width: 20%">
                                                                        <input id="btnRBDREMITCancel" class="button" onclick="javascript:location.reload(true);<%--ARS_API.load();--%>"
                                                                            type="button" value="Cancel" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp</td>
                    </tr>
                </table>
            </div>
            </td> </tr> </table> </div>
            <div id="divResult" style="width: 80%; display: none; vertical-align: top; margin-top: 30px;">
                <table id="tblBodyAPIResult" style="width: 100%; display: block">
                    <tr>
                        <td id="tdAPIResult" style="width: 85%; height: 94%; border-collapse: collapse;"
                            valign="middle">
                            <div id="divAPIResultDefault" style="width: 80%;">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                            <img src="../images/toe.GIF" /></td>
                                        <td id="td2" class="table_head_dark" style="width: 50%; height: 24px">
                                            AGENT REMITTANCE SYSTEM API RESULT</td>
                                        <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                            <img align="absBottom" src="../images/rend.gif" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; border: solid 3px #336699; border-collapse: collapse"
                                            class="table_body" align="center">
                                            <br />
                                            <div id="divAPIResult" style="width: 95%; display: block">
                                                <table id="tblAPIResult" cellpadding="0" cellspacing="5" style="width:95%">
                                                    <tr>
                                                        <td id="tdResult" style="width: 30%; height: 23px">
                                                            <label id="lblResult">
                                                            </label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input id="txtHiddenSeqId2" type="hidden" />
                                                <table cellpadding="0" cellspacing="0" style="width:100%">
                                                    <tr>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%" align="center">
                                                            <table id="tblbuttons2" cellpadding="0" cellspacing="5">
                                                                <tr>
                                                                    <td style="width: 20%">
                                                                        <input id="btnSave" class="button" onclick="javascript:ARS_API.keyword();<%--location.reload(true);--%>" onmouseout="this.className='button';"
                                                                            onmouseover="this.className='button_over';" type="button" value="OK" /></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
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
            </td>
            </tr>
        </table>
        </div>
        </form>
    </center>
</body>
</html>
