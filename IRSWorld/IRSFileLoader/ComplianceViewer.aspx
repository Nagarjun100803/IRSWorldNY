<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComplianceViewer.aspx.cs" Inherits="IRSWorld.IRSFileLoader.ComplianceViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Compliance Viewer</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/FileLoader/js_complianceviewer.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    var compliance_viewer = new compliance_viewer();
    
    function page_setup()
    {
        if (top != self)
        {
            top.frame_scroll("yes");
        }
    }
    
    function page_off()
    {
        if (top != self)
        {
            top.frame_scroll("no");
        }
    }
    
    function fixdim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmain');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
            }
        }
   
    </script>
  

</head>
<body onload="fixdim(); javascript:compliance_viewer.hide_panel();compliance_viewer.load_all_dropdownlist();" onresize="fixdim();"> <!--onunload="page_off();" -->
    <form id="form1" runat="server">
    <div id="divmain" style="padding-top: 30px; width:100%; height:600px; overflow:auto">
        <table>
            <tr>
                <td style="width: 18%; vertical-align:top;">
                 <div id="divComplianceMenu" style="width:100%;vertical-align: top">
                        <table style="width:100%; height:auto">
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="../IRSFileLoader/ComplianceViewer.aspx" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">Compliance Viewer</a></td></tr>
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="../IRSFileLoader/HitViewer.aspx" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">Hit Viewer</a></td></tr>
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="../IRSFileLoader/FEDACHViewer.aspx" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">FEDACH Viewer</a></td></tr>
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;"></td>
                            <%--<a href="javascript:page_load()" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">TEST</a></td>--%></tr>
                        </table>
                  </div>
              
                </td>
                
                <td style="width: 82%">
                    <table cellpadding="0" cellspacing="0" style="width:30%">
                    <tr>
                        <td> 
                            <table cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                        <img src="../images/toe.GIF" /></td>    
                                    <td class="table_head_dark" style="width: 50%; height: 24px">
                                        Compliance </td>
                                    <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                        <img align="absBottom" src="../images/rend.gif" /></td>   
                              </tr>
                            </table>    
                        </td>
                            
                    </tr>
                        <tr>
                            <td style="width: 700px"><table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid; width: 80%; border-bottom: #336699 3px solid">
                                <tr>
                                    <td style="width: 100%; height: 21px; background-color: azure" >
                                        <div style="text-align: center" id="divComplianceViewer">
                                            <table style="width:800px; border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;" cellpadding="0" cellspacing="5"><!--width="800">-->
                                                <tr >
                                                    <td class="table_caption" colspan="5" style="width: 99%; color: black; text-indent: 5px;
                                                        height: 26px; text-align: left">
                                                        Information</td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr >
                                                    <td style="width: 785px; height: 190px; background-color: azure; text-align:center;" align=center><center>
                                                        <div id="ComplianceEntry" style="background-color: azure; text-align: center">
                                                            <table id="tblCompliance" style="width: 767px">
                                                                <tbody>
                                                                    <tr>
                                                                        <td colspan="4" style="width: 123px; height: 55px">
                                                                            <div id="divName">
                                                                                <table id="tblCompliance1" style="border-right: midnightblue 2px solid; border-top: midnightblue 2px solid;
                                                                                    border-left: midnightblue 2px solid; width: 716px; border-bottom: midnightblue 2px solid">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td class="cell_caption" colspan="1" style="width: 93px; height: 26px; text-align: right">
                                                                                                Full Name:</td>
                                                                                            <td colspan="3" style="width: 300px; height: 26px; text-align: right">
                                                                                                <input id="txtFullName" style="font-weight: bold; font-size: 15pt; width: 98%" type="text" /></td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                            <div id="divBusName">
                                                                                <table id="tblCompliance7" style="border-right: midnightblue 2px solid; border-top: midnightblue 2px solid;
                                                                                    border-left: midnightblue 2px solid; width: 716px; border-bottom: midnightblue 2px solid">
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td class="cell_caption" colspan="1" style="width: 93px; height: 26px; text-align: right">
                                                                                                Business Name:</td>
                                                                                            <td colspan="3" style="width: 300px; height: 26px; text-align: right">
                                                                                                <input id="txtBusinessName1" style="font-weight: bold; font-size: 15pt; width: 177%"
                                                                                                    type="text" /></td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table id="tblCompliance2" style="border-right: #336699 1px solid; border-top: #336699 1px solid;
                                                                                border-left: #336699 1px solid; width: 700px; border-bottom: #336699 1px solid">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Last Name:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 176px; height: 26px; text-align: left">
                                                                                            <input id="txtLastName" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 23px">
                                                                                            &nbsp;Alias LastName: &nbsp;</td>
                                                                                        <td style="width: 183px; text-align: left">
                                                                                            <input id="txtAliasLName" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 23px">
                                                                                            First Name:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 176px; text-align: left">
                                                                                            <input id="txtFirstName" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 29px">
                                                                                            &nbsp;Alias FirstName:&nbsp;</td>
                                                                                        <td style="width: 183px; height: 29px; text-align: left">
                                                                                            <input id="txtAliasFName" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 23px">
                                                                                            Middle Initial:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 176px; text-align: left">
                                                                                            <input id="txtMiddleInitial" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 23px">
                                                                                            Alias MiddleInitial:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 183px; text-align: left">
                                                                                            <input id="txtAliasMName" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 29px">
                                                                                            Suffix:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 176px; height: 29px; text-align: left">
                                                                                            <input id="txtSuffix" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 23px">
                                                                                            Alias Suffix:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 183px; text-align: left">
                                                                                            <input id="txtAliasSuffix" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 23px">
                                                                                            &nbsp;Title:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 176px; text-align: left">
                                                                                            <input id="txtTitle" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Nationality:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 183px; text-align: left">
                                                                                            <input id="txtNationality" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table id="tblCompliance3" style="border-right: #336699 1px solid; border-top: #336699 1px solid;
                                                                                border-left: #336699 1px solid; width: 700px; border-bottom: #336699 1px solid">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 126px; height: 26px">
                                                                                            Date of Birth:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 196px; height: 26px; text-align: left">
                                                                                            <input id="txtDateofBirth" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Town of Birth:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; text-align: left">
                                                                                            <input id="txtTownofBirth" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 126px; height: 26px">
                                                                                            Age:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 196px; text-align: left">
                                                                                            <input id="txtAge" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Country of Birth:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; text-align: left">
                                                                                            <input id="txtCountryofBirth" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 126px; height: 29px">
                                                                                            Position:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 196px; height: 29px; text-align: left">
                                                                                            <input id="txtPosition" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 29px">
                                                                                            Business Name:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; height: 29px; text-align: left">
                                                                                            <input id="txtBusinessName" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table id="tblCompliance4" style="border-right: #336699 1px solid; border-top: #336699 1px solid;
                                                                                border-left: #336699 1px solid; width: 700px; border-bottom: #336699 1px solid">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Days Listed:</td>
                                                                                        <td style="width: 202px; text-align: left">
                                                                                            <input id="txtDays" style="width: 98%" type="text" /></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Listed On:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; text-align: left">
                                                                                            <input id="txtListedOn" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Last Updated:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; text-align: left">
                                                                                            <input id="txtLastUpdated" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Other Information:&nbsp;
                                                                                        </td>
                                                                                        <td colspan="4" style="width: 100px; height: 26px; text-align: left">
                                                                                            <input id="txtOther" style="width: 99%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="table_caption" colspan="5" style="width: 100%; color: black; text-indent: 5px;
                                                                            height: 26px; text-align: left">
                                                                            Additionals
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <br />
                                                                            <table id="tblCompliance5" style="border-right: #336699 1px solid; border-top: #336699 1px solid;
                                                                                border-left: #336699 1px solid; width: 700px; border-bottom: #336699 1px solid">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 25px">
                                                                                            City:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 194px; height: 25px">
                                                                                            <input id="txtCity" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            State:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 176px; height: 26px">
                                                                                            <input id="txtState" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Country:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 194px; height: 26px">
                                                                                            <input id="txtCountry" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Zip Code:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 176px; height: 26px">
                                                                                            <input id="txtZipCode" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Phone Number:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 194px; height: 26px">
                                                                                            <input id="txtPhoneNumber" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Passport Details:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; height: 26px">
                                                                                            <input id="txtPassport" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                            <br />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="4">
                                                                            <table id="tblCompliance6" style="border-right: #336699 1px solid; border-top: #336699 1px solid;
                                                                                border-left: #336699 1px solid; width: 700px; border-bottom: #336699 1px solid">
                                                                                <tbody>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            DBA Name:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 191px; height: 26px">
                                                                                            <input id="txtDbaName" style="width: 98%" type="text" /></td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Group Type:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; height: 26px">
                                                                                            <input id="txtGroupType" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Alias Type:
                                                                                        </td>
                                                                                        <td style="width: 191px; height: 26px">
                                                                                            <input id="txtAliasType" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 25px">
                                                                                            Regime:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; height: 25px">
                                                                                            <input id="txtRegime" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            Group ID:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 191px; height: 26px">
                                                                                            <input id="txtGroupId" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                        <td class="cell_caption" style="width: 123px; height: 26px">
                                                                                            File Type:&nbsp;
                                                                                        </td>
                                                                                        <td style="width: 202px; height: 26px">
                                                                                            <input id="txtRegulatory" style="width: 98%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 123px; height: 23px">
                                                                                            Remarks:&nbsp;
                                                                                        </td>
                                                                                        <td colspan="4" style="width: 100px; text-align: left">
                                                                                            <input id="txtRemarks" style="width: 99%" type="text" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="4">
                                                                                        </td>
                                                                                    </tr>
                                                                                </tbody>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        <input id="txtHiddenSeqID" type="hidden" />
                                                        <div id="ComplianceGrid">
                                                        </div>
                                                        <div id="ComplianceBtn" style="background-color: azure">
                                                            <table id="tblbuttons" cellpadding="0" cellspacing="5" style="border-right: #336699 2px solid;
                                                                border-top: #336699 2px solid; border-left: #336699 2px solid; width: 60%; border-bottom: #336699 2px solid;">
                                                                    <tr>
                                                                        <td class="cell_caption" style="width: 25%; text-align: right">
                                                                            Search Criteria:&nbsp;</td>
                                                                        <td style="width: 25%">
                                                                            <select id="ddSearchBy" style="width: 100%">
                                                                                <option selected="selected"></option>
                                                                            </select>
                                                                        </td>                                                                
                                                                        <td style="width: 10%">
                                                                            <select id="ddEqualorLike" style="width: 100%">
                                                                                <option selected="selected"></option>
                                                                                <option value="=" style="text-align:left"> = </option>
                                                                                <option value="like" style="text-align:left"> like </option>
                                                                            </select>
                                                                        </td>    
                                                                        <td style="width: 25%">
                                                                            <input id="txtSearchBy" style="width: 97%" type="text" /></td>                                                          
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="cell_caption" style="width: 24%; text-align: right" >
                                                                            File Format:&nbsp;</td>
                                                                        <td style="width: 38%">
                                                                            <select id="ddFileFormat" style="width: 100%">
                                                                                <option selected="selected"></option>
                                                                            </select>
                                                                        </td> 
                                                                        <td style="width: 38%" colspan= "2">
                                                                            <input id="btnSearch" class="button" onclick="javascript:compliance_viewer.panel_search_compliance()"
                                                                                onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                style="width: 100%" type="button" value="Search" /></td>
                                                                    </tr>
                                                            </table>
                                                        </div>
                                                        <div id="ComplianceBtn2" style="background-color: azure">
                                                            <table id="tblbuttons2" cellpadding="0" cellspacing="5" style="width:60%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 10%">
                                                                        </td>
                                                                        <td style="width: 10%">
                                                                            <input id="btnBack" class="button" onclick="javascript:compliance_viewer.hide_panel();"
                                                                                onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                                style="width: 126px" type="button" value="Back to Search" /></td>
                                                                        <td style="width: 10%">
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                        </center>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
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

</body>
</html>
