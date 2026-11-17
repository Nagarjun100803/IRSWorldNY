<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIF_profile_create.aspx.cs" Inherits="IRSWorld.Report_Profiler.CIF_profile_create" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CIF PROFILE</title>
    
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    
    <script src="../js/Report_Profiler/JS_CIF_Profile.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>              
    <script src="../js/Reports/js_ReportsTemplate.js" type="text/javascript"></script>              
    <script src = "../js/Remittance/jsRemExternals.js" type = "text/javascript"></script>



</head>
<body onload = "javascript:cif.initializePage();">
    <center>                  
    <div  style="overflow:auto ;">
    <form id="form1" runat="server" >
                   
    <div id="divmain" style="width: 89%; margin: 0; height: 450px;  overflow: auto">
                <table id="tblMain" cellspacing="0" cellpadding="0" style="background-color: Azure; width: 95%;">
                
             <tr>    
                <td align="center" valign="top" style="width:130%; border-collapse:collapse" class="content_height">
                    <table width="90%" cellspacing="0" cellpadding="0">
                         <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                    <img src="../images/toe.GIF" /></td>
                                <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                    Report Builder</td>
                                <td id="tdpopup2" align="left" valign="bottom" style="height: 24px; width: 407px;" class="bg_body">
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%;">
                                <table style="width:100%; border:solid 3px #336699;" id="TABLE1">
                                <tr>
                                            <td class="table_caption" colspan="3" style="height: 26px"; >
                                                CREATE CIF PROFILE</td>    
                                   </tr>
                                   <tr><td colspan="4">
                                   <table>
                                   <tr>
                                   <td align="left" class="cell_caption_left" style="height: 21px; width: 4%;">
                                       Profile/s</td>
                                            <td align="left" style="width:13%; height:24px;" colspan = "3" class="cell_caption">
                                            <SELECT style="WIDTH: 69%" id="ddlCIFProfiles"></SELECT>
                                             <input id="btnLoadProfile" style="width:30%;" type="button" value="Load Profile" onclick="javascript:cif.loadExistingProfile('ddlCIFProfiles');"/>
                                            </td>
                                   </tr>
                                   <tr>
                                   <td align="left" class="cell_caption_left" style="width: 4%;">
                                       Create New Profile</td>
                                            <td align="left" style="width:13%;" colspan = "3" class="cell_caption">
                                            <input id="txtProfileName"       style="text-align:left; width:68%;"   type="text" maxlength="50" />
                                             <input id="btnCheckProfile" style="width:30%;" type="button" value="Check Availability" onclick="javascript:cif.isAvailable();"/>
                                            </td> 
                                   </tr>
                                   </table>
                                   </td>
                                   </tr>
                                  
                                   <tr>
                                  <td colspan = "3" style="height: 508px" >
                                   <table cellspacing="0" cellpadding="0" style="width: 100%">
                                    <tr>
                                   <td class="cell_caption_center" style="height: 16px">
                                       CIF Branch</td>
                                   <td class="cell_caption_center" style="height: 16px">
                                       CIF Type/s</td>
                                   </tr>
                                    <tr>
                                   <td class="cell_caption" style="height: 37px; width: 351px;"  >
                                       <table style="width: 100%">
                                                                                  
                                         
                                          
                                          <tr>
                                          <td style="width: 375px" align=left class="cell_caption_left">
                                              <input id="chbxCIFBranches" type="checkbox" style="width: 15px" onclick="javascript:cif.checkAll('chbxCIFBranches','lstCIFBranches')"/>
                                              All</td>
                                          </tr>
                                          
                                       </table>
                                       
                                   <td class="cell_caption" style="height: 37px">
                                   <table style="width: 100%">
                                                                                  
                                 
                                          <tr>
                                          <td style="width: 375px" align="left" class="cell_caption_left">
                                              <input id="chbxCIFType" type="checkbox" style="width: 15px" onclick="javascript:cif.checkAll('chbxCIFType','lstCIFType')" />
                                              All</td>
                                          </tr>
                                       </table>
                                       </td>
                                   </tr>
                                     <tr>
                                      <td align="left" style="width: 50%">
                                        <select name="CIF_branches" size="7"  multiple="multiple" id = "lstCIFBranches"  style="width: 100%" >
                                        </select></td>
                                        <td align="left" style="width: 50%">
                                        <select name="CIF Type" size="7"  multiple="multiple" id = "lstCIFType" style="width: 100%">
                                        </select>
                                        </td>                        
                                   </tr>
                                  <tr>
                                   <td class="cell_caption" style="height: 37px; width: 351px;"  >
                                       <table style="width: 100%">
                                                                                  
                                           <tr>
                                              
                                        <td align="center"  colspan = "3" class="cell_caption_center">
                                            Account Maintaining Branch</td>                                           
                                          </tr>
                                          
                                          <tr>
                                          <td style="width: 375px" align=left class="cell_caption_left">
                                              <input id="chbxCIFAcctBranch" type="checkbox" style="width: 15px" onclick="javascript:cif.checkAll('chbxCIFAcctBranch','lstMaintainingBranch')"/>
                                              All</td>
                                          </tr>
                                          
                                       </table>
                                      </td>
                                   <td class="cell_caption" style="height: 37px">
                                   <table style="width: 100%">
                                                                                  
                                           <tr>
                                              
                                   <td align="center"  colspan = "3" class="cell_caption_center">
                                   </td>
                                            
                                          </tr>
                                          <tr>
                                          <td style="width: 375px" align="left" class="cell_caption_left">
                                              <input id="cbxUndefined" type="checkbox" style="width: 15px" onclick=""/>
                                              All</td>
                                          </tr>
                                       </table>
                                       </td>
                                   </tr>
                                     <tr>
                                      <td align="left" style="width: 50%; height: 111px;">
                                        <select name="Account Maintaining Branch" size="7"  multiple="multiple" id = "lstMaintainingBranch"  style="width: 99%">
                                        </select></td>
                                        <td align="left" style="width: 50%; height: 111px;">
                                        <select name="" size="7"  multiple="multiple" id = "lstUndefined" style="width: 100%">
                                        </select>
                                        </td>                        
                                   </tr>
                                   <tr>
                                   <td colspan="3" class="cell_caption_center" style="height: 16px" >
                                   Filter
                                   </td>
                                   </tr>
                                   <tr>
                                   <td style="height: 154px">
                                  <table width = "100%">
                                  <tr>
                                  <td style="width:25%" class="cell_caption_center">
                                      CIF Class</td>
                                    <td style="width:50%" class="cell_caption_center">
                                        <select id="ddlCIFClass" style="width: 94%" onchange="javascript:evaluateCIFClass('ddlCIFClass')">
                                            <option selected="selected"></option>
                                        </select></td>
                                  </tr>
                                  <tr>
                                  <td style="width:25%; height: 24px;" class="cell_caption_center">
                                      CIFID No.</td>
                                    <td style="width:50%; height: 24px;" class="cell_caption_center">
                                        <input id="txtIdNumber" type="text" style="width: 92%" maxlength="12" /></td>
                                  </tr>
                                  <tr>
                                  <td style="width:25%" class="cell_caption_center">
                                      CIFGUID No.</td>
                                    <td style="width:50%" class="cell_caption_center"><input id="txtCIFGUID" type="text" maxlength="36" style="width: 92%"/></td>
                                  </tr>
                                 <tr>
                                  <td style="width:25%; height: 18px;" class="cell_caption_center">
                                      Remitter Inputted ID</td>
                                    <td style="width:50%; height: 18px;" class="cell_caption_center">
                                        <input id="txtRemitInputID" type="text" style="width: 92%" maxlength="13"/></td>
                                  </tr>
                                  <tr>
                                  <td style="width:25%; height: 18px;" class="cell_caption_center">
                                      CIF Name</td>
                                    <td style="width:50%; height: 18px;" class="cell_caption_center">
                                        <input id="txtCIFName" type="text" style="width: 92%"/></td>
                                  </tr>
                                  <tr>
                                  <td style="width:25%; height: 18px;" class="cell_caption_center">
                                      </td>
                                    <td style="width:50%; height: 18px;" class="cell_caption_center">
                                        </td>
                                  </tr>
                                  </table>
                                   </td>
                                   <td style="height: 154px">
                                   <table style="width: 103%; height: 79px">
                                  <tr>
                                  <td style="width:26%; height: 1px;" class="cell_caption_center">
                                      Account Type&nbsp;</td>
                                    <td style="width:50%; height: 1px;" class="cell_caption_center">
                                        <select id="ddlCIFAcctType" style="width: 91%">
                                            <option selected="selected"></option>
                                        </select></td>
                                  </tr>
                                  <tr>
                                  <td style="width:26%; height: 24px;" class="cell_caption_center">
                                      Account Status</td>
                                    <td style="width:50%; height: 24px;" class="cell_caption_center"><select id="ddlCIFAcctStatus" style="width: 91%">
                                        <option selected="selected"></option>
                                    </select>
                                    </td>
                                  </tr>
                                  <tr>
                                  <td style="width:26%; height: 12px;" class="cell_caption_center">
                                      Account Currency</td>
                                    <td style="width:50%; height: 12px;" class="cell_caption_center"><select id="ddlCIFAcctCcy" style="width: 90%">
                                        <option selected="selected"></option>
                                    </select>
                                    </td>
                                  </tr>
                                  <tr>
                                  <td style="width:26%; height: 12px;" class="cell_caption_center">
                                      Account No.</td>
                                    <td style="width:50%; height: 12px;" class="cell_caption_center">
                                        <input id="txtAccountNo" type="text" style="width: 88%" maxlength="25" />&nbsp;</td>
                                  </tr>
                                  <tr>
                                  <td style="width:26%; height: 12px;" class="cell_caption_center">
                                      Merchant Product/s</td>
                                    <td style="width:50%; height: 12px;" class="cell_caption_center">
                                        <select id="ddlMerchProdCode" style="width: 90%">
                                        <option selected="selected"></option>
                                    </select></td>
                                  </tr>
                                       <tr>
                                           <td class="cell_caption_center" style="width: 26%; height: 12px">
                                               Merchant Member ID</td>
                                           <td class="cell_caption_center" style="width: 50%; height: 12px">
                                        <input id="txtMerchMemID" type="text" style="width: 88%" maxlength="25" /></td>
                                       </tr>
                                  
                                  
                                  </table>
                                   </td>
                                   </tr>
                                                                  
                                                                                                      
                                  </table>
                                  
                                      </td>
                                      
                                   </tr>
                                   <tr>
                                   <td class="cell_caption_center" colspan = "3" id="tddaterange" style="height: 18px">
                                       </td>                                       
                                   </tr>
                                   <tr>
                                   <td align="left" class="cell_caption_center" style="height: 9px; width: 269px;">
                                       Date 
                                       Filter:
                                       <select id="ddlDateFilter" style="width: 72%">
                                       </select></td>
                                            <td align="left" class="cell_caption_center" style="width:261px; height:9px;" >
                                                From:
                                                <img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtDateFrom, 'yyyy-mm-dd',document.getElementById('divmain'));">
                                                <input id="txtDateFrom"       style="text-align:left; width:59%;"   type="text" disabled="disabled" />
                                                </td>
                                            <td align="left" class="cell_caption_center" style="width:268px; height:9px;" >
                                                To: &nbsp;<img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtDateTo, 'yyyy-mm-dd',document.getElementById('divmain'));">
                                         <input id="txtDateTo"       style="text-align:left; width:62%;"   type="text" disabled="disabled" />
                                         </td>
                                            
                                   </tr>
                                   
                                      <tr>
                                   <td class="cell_caption_center" style="width: 269px; height: 24px;">
                                       &nbsp;</td>
                                    <td  class="cell_caption_center" style="width: 261px; height: 24px;"></td>
                                    <td class="cell_caption_center" style="width: 268px; height: 24px;">&nbsp;</td>
                                   </tr>
                                   <tr>
                                   <td class="cell_caption_center" style="width: 269px; height: 25px;">
                                       &nbsp;</td>
                                   <td class="cell_caption_center" style="width: 261px; height: 25px;"></td >
                                   <td class="cell_caption_center" style="width: 268px; height: 25px;"></td >
                                   </tr>
                                   <tr>
                                   <td class="cell_caption_center" style="width: 269px; height: 24px;">
                                       &nbsp;</td>
                                   <td class="cell_caption_center" style="width: 261px; height: 24px;">
                                       Record/s to Display</td >
                                   <td class="cell_caption_center" style="width: 268px; height: 24px;"><input id="txtTopSelect" type="text" style="width: 80%" maxlength="25" onkeypress="return isNumberKey(event,this)"/>
                                   </td >
                                   </tr>
                                   <tr>
                                   <td colspan ="3" style="height: 63px">
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
                                   <td style="width:44%; ">
                                        <select name="List_Select"  multiple="multiple" id = "lstSelectParam"  style="width: 100%; height: 153px;" size="7">
                                   </select>
                                   </td>
                                   <td style="width:10%;" class="cell_caption_center">
                                   <table style="width: 100%">
                                   <tr>
                                   <td>
                                       <input id="btnMoveAllRight" type="button" value=">>" onclick="javascript:cif.selectAllItems('lstSelectParam','lstSelectedParam');"/>
                                       </td>
                                   </tr>
                                   <tr>
                                   <td>
                                       <input id="btnMoveRight" type="button" value=">" onclick="javascript:cif.selectedItems('lstSelectParam','lstSelectedParam');"/>
                                    </td>
                                   </tr>
                                   <tr>
                                   <td>
                                       <input id="btnMoveLeft" type="button" value="<" onclick="javascript:cif.selectedItems('lstSelectedParam','lstSelectParam');"/>
                                   </td>
                                   </tr>
                                   <tr>
                                   <td>
                                   <input id="btnMoveAllLeft" type="button" value="<<" onclick="javascript:cif.selectAllItems('lstSelectedParam','lstSelectParam');"/>
                                   </td>
                                   </tr>
                                   </table>
                                   </td>
                                   <td style="width: 45%; ">
                                    <select name="List_Selected" size="7"  multiple="multiple" id = "lstSelectedParam" style="width: 100%; height: 153px;"></select>
        <div id="divPopProcess" style="width:400px; right:200px; color:Transparent;
                padding: 10px; display: none; font-family: Arial;
                width: 300px; height:150px; position:absolute">
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
                                   <input id="btnReset" type="button" value="RESET" style="width: 15%"  onclick="javascript:cif.initializePage();"/>
                                   <input id="btnParse" type="button" value="PARSE" style="width: 15%" onclick="javascript:cif.parseData();"/>
                                   <input id="btnPreview" type="button" value="PREVIEW" style="width: 15%" onclick="javascript:cif.previewData();"/>
                                   <input id="btnExtract" type="button" value="EXTRACT" style="width: 15%" onclick="javascript:cif.extractFile();"/>
                                   <input id="btnSave" type="button" value="SAVE" style="width: 15%" onclick = "javascript:cif.saveProfile();"/>
                                   </td>
                                   </tr>
                                    
                                   </table>
                           
                                   
                                   </td>
                                   </tr>
                                                                   
                                   <tr>
                                  
                                   <td colspan = "3" style="height: 10px" >
                                     
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
