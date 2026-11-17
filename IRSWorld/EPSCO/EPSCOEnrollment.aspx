<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EPSCOEnrollment.aspx.cs" Inherits="IRSWorld.EPSCO.EPSCOEnrollment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>EPSCO Enrollment</title>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>
    <script src="../js/EPSCO/jsEPSCOTransactions.js" type="text/javascript"></script>
    <script src="../js/EPSCO/jsEPSCOEnrollment.js" type="text/javascript"></script>
    <script src="../js/jsExpand.js" type="text/javascript"></script>
    <script src="../js/jsBalloon.js" type="text/javascript"></script>
    <script src="../js/jsImageResize.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../css/cssWeb.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript">
    
        var epsco_client = new epsco_client();    
        var epsco_trans = new epsco_trans();
           
        function execute_ajx_command(param)
        {            
            if (epsco_trans != null && epsco_trans != "undefined")
            { }
            if (epsco_client != null && epsco_client != "undefined")
            {
                epsco_client.do_ajaxgridview(param);          
            }              
        }
        
        function isNumeric(e)
        {    
            if(!e.shiftKey)
            {
                var numTags = new Array(8,9,37,39,48,49,50,51,52,53,54,55,56,57,96,97,98,99,100,101,102,103,104,105);
                
                for(n in numTags)
                {
                    if(numTags[n] == e.keyCode) return true;
                }
            }
            
            return false;
        }
        
        function isAlpha(e)
        {    
            //if(!e.shiftKey)
            //{
                var alphaTags = new Array(8,9,32,37,39,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90);
                
                for(a in alphaTags)
                {
                    if(alphaTags[a] == e.keyCode) return true;
                }
            //}
            
            return false;
        }
    </script>
    
    <style type="text/css" media="screen,print">
      @font-face 
      {
        font-family: IDAutomationHC39M; 
        src: local("IDAutomationHC39M"), url("../barcode/IDAutomationHC39M.ttf") format("truetype");
      }
    </style>
       
</head>
<body onload="epsco_client.page_initialize();">
    <form id="form1" method="post" runat="server">
    <div id="divPage" style="overflow:auto;margin:auto;">
                     <div id="divEnrollCIF" style="position:absolute;left:10%;top: 978px;width:100%;height:95%;overflow:auto;">
                        <table style="margin:auto; width:760px;" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" valign="bottom" style="width:10px;border-collapse:collapse;height:11px;background-color:#369;"><img src="../images/toe.GIF" /></td>    
                                <td align="left" class="table_head_dark" style="width:50%;height:11px;">Client Enrollment</td>
                                <td align="left" style="width:100%;height:11px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>    
                            </tr>
                            <tr>
                                <td colspan="3" style="width:100%; height: 92px;">
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;height:auto;width:100%">
                                        <tr>
                                            <td class="table_caption" colspan="3" style="width:100%;color:black;text-indent:5px;text-align:left;height:25px;">Remitter&nbsp;Information</td>
                                            <td align="center" style="height: 25px; width: 200px;">
                                                <input style="width:188px;" id="btnSearchRem" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';" onclick="javascript:epsco_client.load_remitter_search();epsco_client.isZipCodeShow=false;" type="button" value="Search Remitter" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">HK ID:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px; color: teal;">
                                                <input id="txtHK_ID" type="text" style="width: 383px;text-transform:uppercase" maxlength="11" onkeydown="javascript:return epsco_client.formatHKID(this,event);" /><label id="mnHK_ID" style="color:White">*</label>&nbsp;&nbsp;<span style="color: teal"> AA######(N)</span></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Enrolling Branch:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <select id="ddlEnrollingBranch" style="width: 563px;font-weight:lighter;font-size:10pt;"><option selected="selected"></option></select>&nbsp;<label id="mnRemBranch" style="color:White">*</label>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Last Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtRemLastName" onkeydown="javascript:if(!isAlpha(event)){return false;}"  type="text" style="width: 565px" maxlength="60" /> <label id="mnRemLName" style="color:White">*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">First Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtRemFirstName" onkeydown="javascript:if(!isAlpha(event)){return false;}" type="text" style="width: 565px" maxlength="60" /> <label id="mnRemFName" style="color:White">*</label>
                                            </td>                                        
                                        </tr>                                                    
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Middle Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtRemMiddleName" onkeydown="javascript:if(!isAlpha(event)){return false;}" type="text" style="width: 565px" maxlength="60" /> <label id="mnRemMName" style="color:White">*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption"  style="width:23%;height:19px;">Birth Date (mm/dd/yyyy):&nbsp;</td>
                                            <td align="left" style="width:27%;height:19px;">
                                                <input id="txtRemBDate" type="text" style="width: 190px" maxlength="10"/>
                                                <span id="mnRemBDate" style="color:White;">*&nbsp;</span>
                                            </td>
                                            <td colspan="2">
                                                <img id="calRemBDate" align="absMiddle" onclick="javascript:if(document.getElementById('txtRemBDate').disabled == false){popUpCalendar(this, document.getElementById('txtRemBDate'), 'mm/dd/yyyy', document.getElementById('divEnrollCIF'));}" src="../images/calendar/imgcalen.png" />
                                            </td>
                                                
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:20px;">Zip Code:&nbsp;</td>
                                            <td align="left" style="width:27%;height:20px;">
                                                <input id="txtRemZipCode" type="text" style="width: 190px" maxlength="7" /> <label id="mnRemZipCode" style="color:White">*</label>
                                            </td>
                                            <td colspan="2" style="height: 20px">
                                                <!--input id="btnRemZipCode" type="button" onclick="javascript:epsco_client.load_zipcode_search()" value="Search" style="width: 22%;" /-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:7px;">Room No./Bldg.:&nbsp;</td>
                                            <td align="left" style="width:27%;height:7px;">
                                                <input id="txtRemAdd1" type="text" style="width: 190px" maxlength="60" /> <label id="mnRemAdd1" style="color:White">*</label>
                                            </td>
                                            <td class="cell_caption" style="width:23%;height:7px;">No./Street:&nbsp;</td>
                                            <td align="left" style="height:7px; width: 200px;">
                                                <input id="txtRemAdd2" type="text" style="width: 180px" maxlength="60" /> <label id="mnRemAdd2" style="color:White">*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">PO Box:&nbsp;</td>
                                            <td align="left" style="width:27%;height:19px;">
                                                <input id="txtRemAdd3" type="text" style="width: 190px" maxlength="60" />
                                            </td>
                                            <td class="cell_caption" style="width:23%;height:19px;">District:&nbsp;</td>
                                            <td align="left" style="height:19px; width: 200px;">
                                                <input id="txtRemAdd4" type="text" style="width: 180px" maxlength="60" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:14px;">City:&nbsp;</td>
                                            <td align="left" style="width:27%;height:14px;">
                                                <input id="txtRemAdd5" type="text" style="width: 190px" maxlength="60" />
                                            </td>
                                            <td class="cell_caption" style="width:23%;height:14px;">State:&nbsp;</td>
                                            <td align="left" style="height:14px; width: 200px;">
                                                <input id="txtRemAdd6" type="text" style="width: 180px" maxlength="60" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:14px;">Region:&nbsp;</td>
                                            <td align="left" style="width:27%;height:14px;">
                                                <input id="txtRemAdd7" type="text" style="width: 190px" maxlength="60" />&nbsp;
                                            </td>
                                            <td class="cell_caption" style="width:23%;height:14px;">Country:&nbsp;</td>
                                            <td align="left" style="height:14px; width: 200px;">
                                                <select id="ddlRemCountry" style="width: 180px"><option selected="selected"></option></select>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                             <td class="cell_caption" style="width:23%;height:19px;">E-mail Address:&nbsp;</td>
                                            <td colspan="3" align="left" style="height:19px; width: 200px;">
                                            <input id="txtRemEmail" type="text" style="width: 190px" maxlength="60" /><span id="mnRemEmail" style="color:White;">*</span></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Telephone No.:&nbsp;</td>
                                            <td align="left" style="width:27%;height:19px;">
                                                <input id="txtRemTelNo" type="text" style="width: 190px" onkeydown="javascript:if(!isNumeric(event)){return false;}" maxlength="10" />
                                            </td>
                                            <td class="cell_caption" style="width:23%;height:19px;">Mobile No.:&nbsp;</td>
                                            <td align="left" style="height:19px; width: 200px;">
                                                 <input id="txtRemMobNo" type="text" style="width: 180px" onkeydown="javascript:if(!isNumeric(event)){return false;}" maxlength="15" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4" style="height: 40px">
                                                <!--input id="btnAddRem" onclick="epsco_client.enroll_client(1,'ADD');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="ADD" style="width: 130px" /-->
                                                <input id="btnSaveRem" onclick="epsco_client.enroll_client(1,'UPDATE');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="UPDATE" style="width: 130px" disabled="disabled" />
                                                <input id="btnClearRem" onclick="epsco_client.clearfields(1);" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="CLEAR" style="width: 130px"/>
                                                <!--input id="btnCancelRem" onclick="javascript:hidePanelEP('divEnrollCIF');javascript:showPanelEP('divSearch');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 130px" /></td-->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="table_caption" colspan="4" style="width:125px;color:black;text-indent:5px;text-align:left;height:25px;">Beneficiary&nbsp;Information
                                            </td>                                       
                                        </tr>
                                        <tr>
                                            <td id="tdBeneficiary" align="center" colspan="4" style="overflow:inherit; height: 17px;">
                                                <div id="grdBeneficiary" style="width:auto;height:auto;"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption"  style="width:23%;height:19px;">Account No:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:25%;height:19px;">
                                                <input id="txtBenefAcctNo" onkeydown="javascript:epsco_client.getAcctBranchCode(this.value,event)" type="text" style="width: 190px" maxlength="40"/> <label style="color:Red">Press ENTER to retrieve corresponding branch name.</label></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption"  style="width:23%;height:19px;">Branch:&nbsp;</td>
                                            <td align="left" colspan="3" style="height:19px;">
                                                <select id="ddlBenBranch" disabled="disabled" style="width: 560px;font-weight:lighter;font-size:10pt; height: 20px;"><option selected="selected"></option></select>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Last Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtBenefLastName" onkeydown="javascript:if(!isAlpha(event)){return false;}" type="text" style="width: 560px" maxlength="60" /> <label id="mnBenLName" style="color:White">*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">First Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtBenefFirstName" onkeydown="javascript:if(!isAlpha(event)){return false;}" type="text" style="width: 560px" maxlength="60" /> <label id="mnBenFName" style="color:White">*</label>
                                            </td>                                        
                                        </tr>                                                    
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Middle Name:&nbsp;</td>
                                            <td align="left" colspan="3" style="width:75%;height:19px;">
                                                <input id="txtBenefMiddleName" onkeydown="javascript:if(!isAlpha(event)){return false;}" type="text" style="width: 560px" maxlength="60" /> <label id="mnBenMName" style="color:White">*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption"  style="width:23%;height:19px;">Birth Date (mm/dd/yyyy):&nbsp;</td>
                                            <td align="left" style="width:27%;height:19px;">
                                                <input id="txtBenefBDate" type="text" style="width: 190px" maxlength="10"/>
                                                <span id="mnBenBDate" style="color:White;">*</span>
                                            </td>
                                            <td colspan="2" style="height: 19px">
                                                <img id="Img2" align="absMiddle" onclick="javascript:if(document.getElementById('txtBenefBDate').disabled == false){popUpCalendar(this, document.getElementById('txtBenefBDate'), 'mm/dd/yyyy', document.getElementById('divEnrollCIF'));}" src="../images/calendar/imgcalen.png" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:20px;">Zip Code:&nbsp;</td>
                                            <td align="left" style="height:20px; width: 200px;">
                                                <input id="txtBenefZipCode" type="text" style="width: 190px" maxlength="7" readonly="readOnly" /><label id="mnBenZipCode" style="color:White;">*</label></td>
                                            <td colspan="2" style="height: 20px"><input id="btnBenZipCode" type="button" value="Search" onclick="javascript:epsco_client.load_zipcode_search();epsco_client.isRemitterShow=false;epsco_client.selected_field=2" style="width: 15%" /></td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Room No./Bldg.:&nbsp;</td>
                                            <td align="left" style="width:27%;height:19px;">
                                                <input id="txtBenefAdd1" type="text" style="width: 190px" maxlength="60" /> <label id="mnBenAdd1" style="color:White">*</label>
                                            </td>
                                            <td class="cell_caption" style="width:23%;height:19px;">No./Street:&nbsp;</td>
                                            <td align="left" style="height:19px; width: 200px;">
                                                <input id="txtBenefAdd2" type="text" style="width: 180px" maxlength="60" /> <label id="mnBenAdd2" style="color:White">*</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">PO Box:&nbsp;</td>
                                            <td colspan="3" align="left" style="width:27%;height:19px;">
                                                <input id="txtBenefAdd3" type="text" style="width: 190px" maxlength="60" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:23%;height:19px;">Telephone No.:&nbsp;</td>
                                            <td align="left" style="width:27%;height:19px;">
                                                <input id="txtBenefTelNo" type="text" style="width: 190px" onkeydown="javascript:if(!isNumeric(event)){return false;}" maxlength="10"/>
                                            </td>
                                            <td class="cell_caption" style="width:23%;height:19px;">Mobile No.:&nbsp;</td>
                                            <td align="left" style="height:19px; width: 200px;">
                                                 <input id="txtBenefMobNo" type="text" style="width: 180px" onkeydown="javascript:if(!isNumeric(event)){return false;}" maxlength="15"/>
                                            </td>
                                        </tr>
                                        <!--tr>
                                            <td class="cell_caption"  style="width:25%;height:19px;font-size:11px">Beneficiary same as Remitter:&nbsp;</td>
                                            <td colspan="3" align="left" style="height: 19px">
                                                <input type="checkbox" id="chkSameAsRemitter" style="width: 8%; height: 24px;" />

                                            </td>
                                        </tr-->
                                        <tr>
                                            <td align="center" colspan="4" style="height: 40px"> &nbsp;
                                                <input id="btnAddBene" onclick="javascript:epsco_client.enroll_client(0,'ADD');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="SAVE" style="width: 130px" />
                                                <input id="btnSaveBene" onclick="javascript:epsco_client.enroll_client(2,'UPDATE');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="UPDATE" style="width: 130px" disabled="disabled" />
                                                <input id="btnDeleteEnrollee" onclick="javascript:epsco_client.delete_client();" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="DELETE" style="width: 130px" />&nbsp;
                                                <input id="btnClearBene" onclick="javascript:epsco_client.clearfields(2);" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="CLEAR" style="width: 130px"/>
                                                <input id="btnCancelEnroll" onclick="javascript:clearForm('divEnrollCIF');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 130px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>     
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divSearchClient" style="z-index:75;position:absolute;color:darkblue;border:solid 2px #000;padding:10px;font-family:Arial;width:803px;height:auto;display:none; left: 3px; top: 187px;">
                        <table cellpadding="0" cellspacing="0" style="width:806px;">
                            <tr>
                                <td align="left" valign="bottom" style="width:11px; border-collapse:collapse;height:11px;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:50%;height:11px;">Search Client</td>
                                <td align="left" style="width:107%;height:11px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>    
                            </tr>
                            <tr valign="top">
                                <td colspan="3" style="width:100%;height:649%">
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;width:100%;height:100%">
                                        <tr>
                                            <td style="width: 240px; height: 36px">
                                                 Last Name:
                                                 <input id="txtsearchLName1" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(1);};" type="text" style="width:auto" />
                                            </td>
                                            <td style="width: 240px; height: 36px">
                                                 First Name:
                                                 <input id="txtsearchFName1" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(1);};" type="text" style="width:auto" />
                                            </td>
                                            <td style="width: 315px; height: 36px">
                                                 Middle Name:
                                                 <input id="txtsearchMName1" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(1);};" type="text" style="width:auto" />
                                                 <input id="btnSearchRemName" type="button" value="Search" onclick="javascript:epsco_client.autoRemSearch(1);" style="width:82px; height: 18px;position:relative;left:10px; top: 0px;" /></td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="3" valign="middle" align="center" style="height:100%;width:100%">
                                                <div id="grdRemitter"></div>
                                            </td>
                                        </tr>
                                     </table>
                                </td>
                             </tr>
                             <tr><td style="width: 11px">&nbsp;</td></tr>
                             <tr>
                                <td valign="top" align="center" colspan="3">
                                    <input id="btnCloseCIFSearch" type="button" value="Close" onclick="javascript:hidePanelEP('divSearchClient');" style="width:150px;" />
                                </td>
                             </tr>
                        </table>
                    </div>
                    
                    
                    <div id="divRemCard" style="z-index:99;position:absolute;border:solid 2px #000;padding:10px;font-family:Arial;width:492px;height:400px;overflow:auto;display:none;left:861px;top:165px;">
                        <center>
                        <table cellpadding="0" cellspacing="0" style="width:400px;margin:0;">
                            <tr>
                                <td style="width:399px;height:227px;">
                                    <table cellpadding="0" cellspacing="0" style="border:solid 1px black;">
                                        <tr>
                                            <td align="center" style="height: 70px;">
                                                <img src="../images/header/pnb_logo.gif" alt="" style="width: 46px; height: 44px;position:relative;left:10px;top:5px;"/>
                                            </td>
                                            <td align="left" style="height: 70px; width: 329px;">
                                                <label style="font-family:Arial;font-weight:bold;font-size:36pt;color:rgb(57,100,153);position:relative;left:-10px;top:5px;">
                                                    PNB
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" style="background-color:rgb(217,217,217); height: 24px;">&nbsp;</td>
                                        </tr>                                  
                                        <tr>
                                            <td align="center" colspan="2" style="height: 85px">
                                            <input type="text" id="lblEPSCORefNo" style="font-family:Arial;font-size:10pt;color:Black;background-color:Transparent;border:0px;width:396px;text-align:center;" value="HSBC(BPS) 10000001025000"/>
                                            <input type="text" id="lblEPSCORefNoCode" style="font-family:IDAutomationHC39M;font-size:12pt;color:Black;background-color:Transparent;border:0px;width:396px;text-align:center; font-weight:lighter; height: 48px;" value="10000001025000" />
                                            <!--div style="background-color:White;width:500px;height:10px;position:relative;top:-20px"></div-->
                                            </td>
                                        </tr>
                                        <tr style="background-color:rgb(48,194,170)">
                                            <td valign="bottom" align="left" style="width: 151px; height: 25px;">
                                                <label style="font-family:Arial;font-weight:bold;font-size:10pt;position:relative;left:20px;color:White;">
                                                Remitter
                                                </label>
                                            </td>
                                            <td valign="bottom" align="left"  style="width: 329px; height: 25px">
                                                <b style="color:White;">:</b>
                                                <input type="text" id="lblRemName" style="font-family:Arial;font-size:10pt;color:White;background-color:Transparent;border:0px;color:White; width: 250px;position:relative;top:-1px;" value="PNB REMITTANCE CENTER LTD"/>
                                            </td>
                                        </tr>
                                        <tr style="background-color:rgb(48,194,170)">
                                            <td align="left"  style="width: 151px; height: 49px">
                                                <label style="font-family:Arial;font-weight:bold;font-size:10pt;position:relative;left:20px;color:White;">
                                                Beneficiary
                                                </label>
                                            </td>
                                            <td valign="Bottom" align="left" style="height: 49px; width: 329px;">
                                                <b style="color:White;">:</b>
                                                <input type="text" id="lblBenName"  style="font-family:Arial;font-size:10pt;background-color:Transparent;border:0px;color:White;width:250px;position:relative;top:2px;" value="PHILIPPINE NATIONAL BANK"/>
                                                <br />
                                                <input type="text" id="lblBenAcctNo" style="font-family:Arial;font-size:10pt;background-color:Transparent;border:0px;position:relative;left:10px;width:250px;color:White; top: 0px;" value=""/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                          <tr>
                              <td style="width:399px;height:15px;">
                                    &nbsp;
                              </td>
                          </tr>
                          <tr>
                                <td style="width:399px;height:239px;">
                                    <table cellpadding="5" cellspacing="0" style="border:solid 1px black;width:398px;height:239px;font-size:7pt;">
                                        <tr>
                                            <td style="width: 398px;height:5px;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr style="background-color:Black;border:solid 1px black;height:48px;">
                                            <td style="width: 398px; height: 25px;">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td valign="top" style="text-align:justify;color:Black;width:398px;height:171px;">
                                            For rate Inquiries, Call PNB HK Hotline No. 2543 9555.<br />
                                            This is your card when you remit to the beneficiary account number indicated herein. <br />
                                            Remittance to any retail outlets shall be subject to the following:<br />
                                            <ul>
                                                <li style="list-style-type:square">
                                                    Amount should be paid in HK Dollars only.
                                                </li>
                                                <li style="list-style-type:square">
                                                    The transaction amount should not exceed HK$ 5,000 per beneficiary per day. 
                                                </li>
                                            </ul>
                                            For remittance at PNB counters, please refer to the branch for specific guidelines on on-line payments.<br />
                                            PNB shall remit the amount based on the current exchange rates net applicable charges for the services.<br />
                                            The use of this card means acceptance of the terms and conditions in the application 
                                            form executed by the cardholder. This card is a property of PNB Global Remittance and Financial Company (HK) Limited.
                                            If found, please return to its Hong Kong office at 26/F.,Worldwide House, 18 Des Voeux Road, Central, Hong Kong.
                                            </td>
                                        </tr>
                                    </table>
                                </td>  
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 17px; width: 451px;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="3" style="height: 20px; width: 451px;">
                                    <input id="btnPrintRemCard" type="button" value="Print" onclick="javascript:epsco_client.PrintRemCard('divRemCard');" style="width:150px;" />
                                    <input id="btnCloseRemCard" type="button" value="Close" onclick="javascript:hidePanelEP('divRemCard');" style="width:150px;" />
                                </td>
                            </tr>
                        </table>
                        </center>   
                    </div>
                    
                    <div id="divEnrollees" style="position:absolute;color:darkblue;padding:10px;font-family:Arial;width:100%;height:90%;display:none; left: 9px; top: 373px;overflow:auto;"> 
                        <table cellpadding="0" cellspacing="0" style="width:913px;margin:auto;">
                            <tr>
                                <td align="left" valign="bottom" style="width:11px; border-collapse:collapse;height:11px;"><img src="../images/toe.GIF" /></td>    
                                <td class="table_head_dark" style="width:50%;height:11px;">EPSCO Card Generation</td>
                                <td align="left" style="width:100%;height:11px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>    
                            </tr>
                            <tr valign="top">
                                <td colspan="3" style="width:100%;height:91%">
                                    <table style="border-right: #336699 3px solid;border-top: #336699 3px solid;border-left: #336699 3px solid; border-bottom:#336699 3px solid;width:100%;height:100%">
                                        <tr>
                                            <td style="width: 34%; height: 30px">
                                                &nbsp;Last Name:
                                                 <input id="txtsearchLName2" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(2);};" type="text" style="width:170px" />
                                            </td>
                                            <td style="height: 30px; width: 33%;"> First Name:
                                                 <input id="txtsearchFName2" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(2);};" type="text" style="width:170px;" />
                                            </td>
                                            <td style="height: 30px; width: 33%;"> &nbsp;Middle Name:
                                                 <input id="txtsearchMName2" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(2);};" type="text" style="width:170px" />
                                            </td>
                                        </tr>
                                        <tr >
                                            <td style="height: 30px; width: 251px;position:relative;left:20px"> Status:<select id="ddlEnrollStatus"  onchange="javascript:document.getElementById('txtsearchLName2').value='';document.getElementById('txtsearchFName2').value='';document.getElementById('txtsearchMName2').value='';epsco_client.autoRemSearch(2);" style="width: 170px; height: 23px; vertical-align:middle;position:relative;left:6px;">
                                                    <option selected="selected" value="0">Pending</option>
                                                    <option value="1">Approved</option>
                                                    <option value="2">For Printing</option>
                                                </select>
                                            </td>
                                            <td colspan="2" style="height: 30px;position:relative;left:2px">
                                            
                                                <label id="lbldtfrom">Date From:</label>
                                                <input id="txtEnrollDate1" type="text" style="width: 131px" maxlength="10" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(2);};"/>
                                                <img id="calEnrollDate1" align="absMiddle" onclick="javascript:if(document.getElementById('txtEnrollDate1').disabled == false){popUpCalendar(this, document.getElementById('txtEnrollDate1'), 'mm/dd/yyyy', document.getElementById('divEnrollees'));}" src="../images/calendar/imgcalen.png" />
                                                <label id="lbldtto">To:</label>
                                                <input id="txtEnrollDate2" type="text" style="width: 130px" maxlength="10" onkeydown="javascript:if(event.keyCode==13){epsco_client.autoRemSearch(2);};"/>
                                                <img id="calEnrollDate2" align="absMiddle" onclick="javascript:if(document.getElementById('txtEnrollDate2').disabled == false){popUpCalendar(this, document.getElementById('txtEnrollDate2'), 'mm/dd/yyyy', document.getElementById('divEnrollees'));}" src="../images/calendar/imgcalen.png" />&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 37px">
                                                <label style="position:relative;left:5px;font-size:8pt;color:rgb(67,137,130);">Note: Press <i>Enter</i> or Click <b>SEARCH</b> button to retrieve info.</label>
                                            </td>
                                            <td colspan="2" style="height: 37px;position:relative;left:60px;">
                                                <input id="btnApproverSearch" type="button" value="Search" onclick="javascript:epsco_client.autoRemSearch(2);" style="width:130px; font-size: 10pt;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="tdEnrollees" colspan="3" valign="middle" align="center">
                                                <div id="grdEnrollees" style="height:60%;width:100%;"></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center" style="height: 50px;"> 
                                                <input id="btnExportAll" onclick="javascript:epsco_client.batch_export_data_all();" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Export All" style="width: 130px" />
                                                <input id="btnApproverGenerateCard" onclick="javascript:if(document.getElementById('ddlEnrollStatus').selectedIndex == 0){epsco_client.batch_approve();}else{epsco_client.batch_export_data();}" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Approve" style="width: 130px" />
                                                <asp:HiddenField ID="h_obj_data" value="" runat="server"/>
                                                <asp:HiddenField ID="h_Export_all" value="" runat="server"/>
                                                <asp:Button ID="invoke_export_all" runat="server" Text="" OnClick="invoke_Export_All_Click" Width="0px" Height="0px" TabIndex="99" UseSubmitBehavior="False" />
                                                <asp:Button ID="invoke_obj_data" runat="server" Text="" OnClick="invoke_obj_data_Click" Width="0px" Height="0px" TabIndex="99" UseSubmitBehavior="False" />
                                            </td>
                                        </tr>
                                     </table>
                                </td>
                             </tr>
                             <tr><td style="width: 11px">&nbsp;</td></tr>
                        </table>
                    </div>
                    
                    <div id="divZipCodes" style="z-index:99;position:absolute;color:darkblue;padding:10px;font-family:Arial;width:370px;height:auto;display:none;left:186px;top:27px;overflow:auto;">
                            <table style="border:0;margin:auto;width:350px;">
                                <tr>
                                    <td style="height: 36px">FIND: <input id="txtsearchZipCode" onkeyup="javascript:epsco_client.load_grid_viewer(3);" onload="this.focus()" type="text" style="width:320px" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="grdZipCodes" style="height:auto;width:350px;"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height:50px" align="center"><input id="btnCloseZipCodes" onclick="hidePanelEP('divZipCodes');" class="button" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Close" style="width: 130px" /></td>
                                </tr>
                            </table>      
                    </div>
        </div>
    </form>
</body>
</html>
