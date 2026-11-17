<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="tranx_profile_createV2.aspx.cs" Inherits="IRSWorld.Report_Profiler.tranx_profile_createV2" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Transaction Report Profile</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    
    
     <script src="../js/Report_Profiler/js_rpt_profile_tranxV2.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>

     <script src="../js/jsCommons.js" type="text/javascript"></script>
     <script src="../js/jshttprequest.js" type="text/javascript"></script>
     <script src="../js/jscommonmethod.js" type="text/javascript"></script>
     <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
     <script src="../js/jsgridselector.js" type="text/javascript"></script>              
     <script src="../js/Reports/js_ReportsTemplate.js" type="text/javascript"></script>              
     <script src = "../js/Remittance/jsRemExternals.js" type = "text/javascript"></script>
    <script type="text/javascript"> <!--language="javascript"-->
    var report_builder = new report_builder();  
function TABLE1_onclick() {

}

</script> 
</head>



<body onload = "javascript:report_builder.initializepage();">
    <center>                  
    <div  style="overflow:auto ;">
    <form id="form1" runat="server" >
                   
    <div id="divmain" style="width: 90%; margin: 0; height: 450px;  overflow: auto; padding-top: 30px">
                <table id="tblMain" cellspacing="0" cellpadding="0" style="background-color: Azure; width: 95%;">
                
             <tr>    
                <td align="center" valign="top" style="width:130%; border-collapse:collapse" class="content_height">
                    <table width="90%" cellspacing="0" cellpadding="0">
                         <tr>
                            <td style="width:60%; vertical-align:bottom; text-align:left" align="left">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="bottom" style="width:0%;height:24px; border-collapse: collapse"> 
                                            <img src="../images/toe.GIF" /></td>
                                        <td id="capLabel" align="left" class="table_head_dark" style="width:60%; height: 24px; border-collapse:collapse">
                                            Report Builder</td>
                                        <td id="tdpopup2" align="left" valign="bottom" style="height: 24px; width: 40%;" class="bg_body">
                                            <img align="absbottom" src="../images/rend.gif" /></td>
                                    </tr>
                                </table>
                            </td>
                          </tr>
                            <tr>
                                <td style="width: 100%;">
                                <table style="width:100%; border:solid 3px #336699;" id="TABLE1" onclick="return TABLE1_onclick()">
                                <tr>
                                            <td class="table_caption" colspan="3" style="height: 26px"; >
                                                CREATE REPORT PROFILE</td>    
                                   </tr>
                                   <tr><td colspan="4">
                                   <table>
                                   <tr>
                                   <td align="left" class="cell_caption_left" style="height: 21px; width: 4%;">
                                       Profile(s)</td>
                                            <td align="left" style="width:13%; height:24px;" colspan = "3" class="cell_caption">
                                            <SELECT style="WIDTH: 69%" id="ddProfiles"></SELECT>
                                             <input id="btnLoadProfile" style="width:30%;" type="button" value="Load Profile" onclick="javascript:report_builder.LoadProfile();"/>
                                            </td>
                                   </tr>
                                   <tr>
                                   <td align="left" class="cell_caption_left" style="width: 4%;">
                                       Create New Profile</td>
                                            <td align="left" style="width:13%;" colspan = "3" class="cell_caption">
                                            <input id="txtProfileName"       style="text-align:left; width:69%;"   type="text" maxlength="50" />
                                             <input id="btnCheck" style="width:30%;" type="button" value="Check Availability" onclick="javascript:report_builder.Click_CheckProfileName();"/>
                                            </td> 
                                   </tr>
                                   </table>
                                   </td>
                                   </tr>
                                  
                                   <tr>
                                  <td colspan = "3" >
                                   <table cellspacing="0" cellpadding="0" style="width: 100%">
                                    <tr>
                                   <td class="cell_caption_center" style="height: 16px">Remit Method
                                   </td>
                                   <td class="cell_caption_center" style="height: 16px">
                                       Batch Types</td>
                                   </tr>
                                    <tr>
                                   <td class="cell_caption" style="height: 37px; width: 351px;"  >
                                       <table style="width: 100%">
                                                                                  
                                         
                                          
                                          <tr>
                                          <td style="width: 375px" align=left class="cell_caption_left">
                                              <input id="chkAllRemitMethod" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAllRemitMethod('ddRemitMethod','chkAllRemitMethod');"/>
                                              All</td>
                                          </tr>
                                          
                                       </table>
                                       
                                   <td class="cell_caption" style="height: 37px">
                                   <table style="width: 100%">
                                                                                  
                                 
                                          <tr>
                                          <td style="width: 375px" align="left" class="cell_caption_left">
                                              <input id="chkAllBatchTypes" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('ddBatchTypes','chkAllBatchTypes');" />
                                              All</td>
                                          </tr>
                                       </table>
                                       </td>
                                   </tr>
                                     <tr>
                                      <td align="left" style="width: 50%">
                                        <select name="Remit_Method" size="7"  multiple="multiple" id = "ddRemitMethod"  style="width: 100%" >
                                        </select></td>
                                        <td align="left" style="width: 50%">
                                        <select name="Batch_Type" size="7"  multiple="multiple" id = "ddBatchTypes" style="width: 100%">
                                        </select>
                                        </td>                        
                                   </tr>
                                  <tr>
                                   <td class="cell_caption" style="height: 37px; width: 351px;"  >
                                       <table style="width: 100%">
                                                                                  
                                           <tr>
                                              
                                        <td align="center"  colspan = "3" class="cell_caption_center">
                                       Remitting Branch:
                                   </td>                                           
                                          </tr>
                                          
                                          <tr>
                                          <td style="width: 375px" align=left class="cell_caption_left">
                                              <input id="chkAllOverseas" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('ddOverseasBranches','chkAllOverseas');"/>
                                              All Branches</td>
                                          </tr>
                                          
                                       </table>
                                      </td>
                                   <td class="cell_caption" style="height: 37px">
                                   <table style="width: 100%">
                                                                                  
                                           <tr>
                                              
                                   <td align="center"  colspan = "3" class="cell_caption_center">
                                       PNB
                                       Maintaining Branch:
                                   </td>
                                            
                                          </tr>
                                          <tr>
                                          <td style="width: 375px" align="left" class="cell_caption_left">
                                              <input id="chkAllDomestic" type="checkbox" style="width: 15px" onclick="javascript:report_builder.CheckselectAll('ddDomesticBranches','chkAllDomestic');"/>
                                              All Branches (check to include non pnb)</td>
                                          </tr>
                                       </table>
                                       </td>
                                   </tr>
                                     <tr>
                                      <td align="left" style="width: 50%">
                                        <select name="Remitting_Branch" size="7"  multiple="multiple" id = "ddOverseasBranches"  style="width: 99%">
                                        </select></td>
                                        <td align="left" style="width: 50%">
                                        <select name="Crediting_Branch" size="7"  multiple="multiple" id = "ddDomesticBranches" style="width: 100%">
                                        </select>
                                        </td>                        
                                   </tr>
                                   <tr>
                                   <td colspan="3" class="cell_caption_center" style="height: 16px" >
                                   Filter
                                   </td>
                                   </tr>
                                   <tr>
                                   <td>
                                  <table width = "100%">
                                  <tr>
                                  <td style="width:40%" class="cell_caption_left">
                                      Product Code
                                  </td>
                                    <td style="width:50%" class="cell_caption_center">
                                        &nbsp;<select id="ddProductCode" style="width: 100%">
                                            <option selected="selected"></option>
                                        </select></td>
                                  </tr>
                                  <tr>
                                  <td style="width:40%" class="cell_caption_left">
                                      Transaction Source&nbsp;</td>
                                    <td style="width:50%" class="cell_caption_center">
                                        &nbsp;<select id="ddTransactionSource" style="width: 100%">
                                            <option selected="selected"></option>
                                        </select></td>
                                  </tr>
                                  <tr>
                                  <td style="width:40%" class="cell_caption_left">
                                      Pay Mode&nbsp;</td>
                                    <td style="width:50%" class="cell_caption_center">
                                        &nbsp;<select id="ddPayMode" style="width: 100%">
                                            <option selected="selected"></option>
                                        </select></td>
                                  </tr>
                                  <tr>
                                  <td style="width:40%" class="cell_caption_left">
                                      Transaction Status&nbsp;</td>
                                    <td style="width:50%" class="cell_caption_center">
                                        &nbsp;<select id="ddTransactionStatus" style="width: 100%">
                                            <option selected="selected"></option>
                                        </select></td>
                                  </tr>
                                  </table>
                                   </td>
                                   <td>
                                   <table style="width: 103%; height: 79px">
                                  <tr>
                                  <td style="width:23%; height: 1px;" class="cell_caption_center">
                                      Remitter ID
                                  </td>
                                    <td style="width:50%; height: 1px;" class="cell_caption_center">
                                        <input id="txtRemitterID" type="text" style="width: 75%" maxlength="16" /></td>
                                  </tr>
                                  <tr>
                                  <td style="width:23%" class="cell_caption_center">
                                      Beneficiary ID</td>
                                    <td style="width:50%" class="cell_caption_center">
                                        <input id="txtBeneficiaryID" type="text" style="width: 75%" maxlength="16" /></td>
                                  </tr>
                                  <tr>
                                  <td style="width:23%; height: 12px;" class="cell_caption_center">
                                      Account No.</td>
                                    <td style="width:50%; height: 12px;" class="cell_caption_center">
                                        <input id="txtAccountNo" type="text" style="width: 76%" maxlength="25" /></td>
                                  </tr>
                                  <tr>
                                  <td style="width:23%" class="cell_caption_left">
                                      &nbsp;</td>
                                    <td style="width:50%" class="cell_caption_center">
                                        </td>
                                  </tr>
                                  </table>
                                   </td>
                                   </tr>
                                                                  
                                                                                                      
                                  </table>
                                  
                                      </td>
                                      
                                   </tr>
                                   <tr>
                                   <td class="cell_caption_center" colspan = "3" id="tddaterange">
                                       Date Range</td>                                       
                                   </tr>
                                   <tr>
                                   <td align="left" class="cell_caption_center" style="height: 9px; width: 268px;">
                                       Filter:&nbsp;
                                       <select id="ddDateFilter" style="width: 84%">
                                          
                                       </select>
                                   </td>
                                            <td align="left" class="cell_caption_center" style="width:215px; height:9px;" >
                                                From:
                                                <img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtDateFrom, 'yyyy-mm-dd',document.getElementById('divmain'));">
                                            <input id="txtDateFrom"       style="text-align:left; width:65%;"   type="text" disabled="disabled" /></td>
                                            <td align="left" class="cell_caption_center" style="width:226px; height:9px;" >
                                                To:
                                         <img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtDateTo, 'yyyy-mm-dd',document.getElementById('divmain'));">
                                         <input id="txtDateTo"       style="text-align:left; width:73%;"   type="text" disabled="disabled" />
                                            </td>
                                            
                                   </tr>
                                   
                                      <tr>
                                   <td class="cell_caption_center" style="width: 268px">
                                   Currency</td>
                                    <td  class="cell_caption_center" style="width: 215px">
                                        Computation</td>
                                    <td class="cell_caption_center" style="width: 226px">
                                        &nbsp;<select id="ddAmtComputation" style="width: 75%">
                                           <option selected="selected"></option>
                                       </select></td>
                                   </tr>
                                   <tr>
                                   <td class="cell_caption_left" style="width: 268px">
                                       Remitting :&nbsp;
                                       <select id="ddRemittingCCY" style="width: 66%">
                                       </select></td>
                                   <td class="cell_caption_center" style="width: 215px">
                                       Condition</td >
                                   <td class="cell_caption_center" style="width: 226px"><select id="ddAmtFilter" style="width: 75%">
                                   </select></td >
                                   </tr>
                                   <tr>
                                   <td class="cell_caption_left" style="width: 268px">
                                   Target : &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                       <select id="ddTargetCCY" style="width: 66%">
                                       </select>
                                   </td>
                                   <td class="cell_caption_center" style="width: 215px">
                                       Amount</td >
                                   <td class="cell_caption_center" style="width: 226px"><input id="txtAmount"  style="text-align:right; width:75%;"   type="text" value="0.00" maxlength="12">
                                   </td >
                                   </tr>
                                   <tr>
                                   <td colspan ="3">
                                   <table style="width: 100%">
                                   <tr>
                                   <td colspan="2" class="cell_caption_center">
                                       Field Selection</td>
                                   </tr>
                                   <tr>
                                   <td style="width:50%" class="cell_caption_center">
                                       Choices</td>
                                   <td style="width:50%" class="cell_caption_center">
                                       Selected</td>
                                   </tr>
                                   </table>
                                   </td>
                                   </tr>
                                   
                                   <tr>
                                   <td colspan ="3">
                                   <table style="width: 100%">
                                   <tr>
                                   <td style="width:45%; "><select name="FieldChoices"  multiple="multiple" id = "ddFieldChoices"  style="width: 100%; height: 153px;" size="7">
                                   </select>
                                   </td>
                                   <td style="width:10%;" class="cell_caption_center">
                                   <table style="width: 100%">
                                   <tr>
                                   <td>
                                       <input id="btnMoveAllRight" type="button" value=">>" onclick="javascript:report_builder.moveitemsAll('ddFieldChoices','ddFieldSelected');"/>
                                       </td>
                                   </tr>
                                   <tr>
                                   <td>
                                       <input id="btnMoveRight" type="button" value=">" onclick="javascript:report_builder.moveitems('ddFieldChoices','ddFieldSelected');"/>
                                    </td>
                                   </tr>
                                   <tr>
                                   <td>
                                       <input id="btnMoveLeft" type="button" value="<" onclick="javascript:report_builder.moveitems('ddFieldSelected','ddFieldChoices');"/>
                                   </td>
                                   </tr>
                                   <tr>
                                   <td>
                                   <input id="btnMoveAllLeft" type="button" value="<<" onclick="javascript:report_builder.moveitemsAll('ddFieldSelected','ddFieldChoices');"/>
                                   </td>
                                   </tr>
                                   </table>
                                   </td>
                                   <td style="width: 45%; "><select name="Field_Selected" size="7"  multiple="multiple" id = "ddFieldSelected" style="width: 100%; height: 153px;">
                                   </select>
                                       <div id="divPopProcess" style="width:400px; right:200px;; color: #00008B;
                padding: 10px; display: none; font-family: Arial;
                width: 300px; height:150px; position:relative">
            <table style="width:100%; height:100%;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center; width=100%;" class="label2"></label>
                        <div id="divLoadingMsg">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
                                   
                                   </td>
                                   
                                   </tr>
                                   
                                   </table>
                                   </td>
                                   </tr>
                                   <tr>
                                   <td colspan="4" >
                                       
                                   <table style="width: 100%; height: 22px">
                                   <tr> 
                                   <td style="width: 100%; " colspan  ="3" class="cell_caption_center">
                                   <input id="btnReset" type="button" value="RESET" style="width: 15%"  onclick="javascript:report_builder.initializepage();"/>
                                   <input id="btnParse" type="button" value="PARSE" style="width: 15%" onclick="javascript:report_builder.ParseData();"/>
                                   <input id="btnPreview" type="button" value="PREVIEW" style="width: 15%" disabled="disabled" onclick="javascript:report_builder.PreviewData();"/>
                                   <input id="btnExtract" type="button" value="EXTRACT" style="width: 15%" disabled="disabled" onclick="javascript:report_builder.ExtractReport();"/>
                                   <input id="btnSave" type="button" value="SAVE" style="width: 15%"  disabled="disabled" onclick = "javascript:report_builder.PreSaveReportProfile();"/>
                                   </td>
                                   </tr>
                                    
                                   </table>
                           
                                   
                                   </td>
                                   </tr>
                                                                   
                                   <tr>
                                  
                                   <td colspan = "3" style="height: 10px" >
                                     
  
            
                                    
                                   <div id="divhidden" style="VISIBILITY: hidden; width: 90%">
                                   <textarea id="txtSQLQuery" style="width: 377px; height: 16px" cols="">
                                            </textarea>
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
    </div>
    </center>
</body>
</html>
