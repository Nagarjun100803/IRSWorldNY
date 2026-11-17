<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GFCMaintenance.aspx.cs" Inherits="IRSWorld.GFC.GFCMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GFC MAINTENANCE</title>
    <link href="../css/Default.css" rel="stylesheet"        type="text/css" />
    <script src="../js/commons.js"                        type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>    
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>
    <script src="../js/GFC/jsGFCMaintenance.js"                   type="text/javascript"></script>  
   
    <script type="text/javascript">    
        var GFC_Main = new GFC_Main();     
       
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
       
        function correctPage()
        {
             document.getElementById("Main").style.height = parseInt(top.document.getElementById('divCenter').offsetHeight);             
        }   
        
    </script> 
</head>
<body onload = "javascript:GFC_Main.page_onload();correctPage();", onunload = "javascript:GFC_Main.page_unload();" onkeyup="javascript:GFC_Main.on_key_press();" onresizeend="correctPage();">
    <center>
        <form id="form1" runat="server">
           <div id="divPopProcess" style="width:250px; height:100px; background:White; z-index: 101; position: absolute; left: 150px;
            color: darkblue; padding: 10px; display: none; font-family: Arial;
            top: 741px;">
            <table style="width:100%; height:100%;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center; color:Chocolate ">Processing request, Please wait... </label>
                        <div id="divLoadingMsg">
                            <img src="../images/ajaxloader.gif" id="imgAjaxIcon" />
                        </div>
                    </td>
                </tr>
<%--                <tr>
                    <td align="center">
                        
                    </td>     
                </tr>  --%>
            </table>
           </div>  
            <table id="tbl_main" cellpadding="0" cellspacing="5" style="width:100%; height:100%;margin-top: 30px;">                
                <tr>
                    <td style="width:20%" valign="top">
                        <div id = "CIFMENU" style= "display:block; top:100px;" >
                          <table style="width:100%" cellpadding="0" cellspacing="5">
          <%--                  <tr id="trMaster">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; width: 182px;">
                                    <a id="Master" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                        Claim Card</a>
                                </td>
                            </tr>
                            <tr id="trCIFClass">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; width: 182px;">
                                    <a id="CIFClass" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                        Cancellation Approval</a>
                                </td>
                            </tr>
                            <tr id="trAddress">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; width: 182px;">
                                    <a id="Address" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;" >
                                        Replacement Approval</a>
                                </td>
                            </tr>   --%>       

                            <tr id="tr2">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; width: 182px;">
                                    <a id="A2" href="javascript:GFC_Main.backToRemInfo();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;" >
                                        Back to Remitter Info</a>
                                </td>
                            </tr>
                            <tr id="tr1">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; width: 182px;">
                                    <a id="A1" href="javascript:GFC_Main.viewMode(0);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                        Search New</a>
                                </td>
                            </tr>                   
                            <tr id="trAddNewClient">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1; width: 182px;">
                                    <a id="A3" href="javascript:GFC_Main.AddNewCIF();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                        Add New Client</a>
                                </td>
                            </tr>                                                                                                     
                        </table>
                    </div>
                    </td>
                    <td style="width: 79%" align="center" valign="top">
                        <%--<div id = "CIFENTRY" style= "display:block; overflow:auto; height:500px;">--%>
                        <%--<div id = "Main" style= "display:block; overflow:auto; height:410px;">--%>
                        <div id = "Main" style= "display:block; overflow:auto; height:500px;"> 
                            <table style="width:90%" cellspacing="0" cellpadding="0"> 
                                <tr>                                    
                                    <td style="width:60%; vertical-align:bottom; text-align:left" align="left">
                                        <table style="width:100%"cellpadding="0" cellspacing="0" >
                                            <tr>
                                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                    <img src="../images/toe.GIF" /></td>    
                                                <td id='tdHeader' class="table_head_dark" style="width: 40%; height: 24px">
                                                    Global Filipino Card </td>
                                                <td align="left" style="width: 60%; height: 24px" valign="bottom">
                                                    <img align="absBottom" src="../images/rend.gif" /></td> 
                                             </tr>                                                                              
                                        </table>                                            
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td style="background:azure; height: 1130px;">
                                        <table id="tblCIF" style="width:100%; border:solid 3px #336699;" >
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div id="CIFSearch">
                                                        <table id="Table2" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id='tdSearchHeader' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    Client Search</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    *Client Type:&nbsp;</td>
                                                                <td colspan="4" align="left" style="height: 25px;">
                                                                    <select id='ddlClientType' style="width: 86%" onchange="javascript:GFC_Main.ddlClientType_on_change();"><option selected='selected'></option>
                                                                    </select></td>
                                                            </tr>                                                            
                                                            <tr id="trLasname">
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >Last Name:&nbsp;</td>
                                                                <td colspan="4" align="left" style="height: 25px;">
                                                                    <input id="txtSearchLastName" type="text" maxlength="200" style="text-align: left; width: 85%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >First Name:&nbsp;</td>
                                                                <td colspan="4" align="left" style="height: 25px">
                                                                    <input id="txtSearchFirstName" type="text" maxlength="200" style="text-align: left; width: 85%;" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >Middle Name:&nbsp;</td>
                                                                <td colspan="4" align="left" style="height: 25px">
                                                                    <input id="txtSearchMiddleName" type="text" maxlength="50" style="text-align: left; width: 85%;" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    CIF ID:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    <input id="txtSearchCIFID" type="text" maxlength="200" style="text-align: left; width: 85%;" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    Old CIF ID:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    <input id="txtSearchOLDCIFID" type="text" maxlength="200" style="text-align: left; width: 85%;" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    CARD No.:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    <input id="txtSearchCARDNo" type="text" maxlength="200" style="text-align: left; width: 85%;" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    CK No.:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    <input id="txtSearchCKNo" type="text" maxlength="200" style="text-align: left; width: 85%;" /></td>
                                                            </tr>  
                                                           <tr>
                                                                <%--<td width = "60%" align="right" colspan="3" style="height: 22px"></td>
                                                                <td width ="27%" style="height: 22px"><input id="btnSearch"   class = "button" onclick = "javascript:GFC_Main.panel_main_search();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Search" style="width: 100%"/></td>                                                               
                                                                <td width = "13%" style="height: 22px"></td>  --%>                                                               
                                                                <td width ="100%" style="height: 22px" colspan="5"><input id="btnSearch"   class = "button" onclick = "javascript:GFC_Main.panel_main_search();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Search" style="width: 174px"/></td>                                                                                                                               
                                                            </tr>                                                            
                                                        </table>
                                                    </div>            
                                                    <div id = "CIFPERSONALINFOENTRY" style= "display:block"> 
                                                        <table id="tblIndividualHeader" style="width:100%" cellpadding="0">
                                                            <tr>
                                                                <td class="cell_desc" style="width: 11%; height: 24px">
                                                                    Remitter Name :</td>
                                                                <td align="left" style="width: 24%; height: 24px;" colspan="4">
                                                                    <label id="lblCustLName" class="cell_desc"></label>&nbsp;
                                                                    <label id="lblCustFName" class="cell_desc"></label>&nbsp;
                                                                    <label id="lblCustMName" class="cell_desc"></label>
                                                                </td>
                                                                <td class="cell_desc" style="width: 7%; height: 24px" >CIF ID :</td>
                                                                <td align="left" style="width: 26%; height: 24px;" colspan="4">
                                                                    <label id="lblCIFID" class="cell_desc">
                                                                        </label></td>
                                                            </tr>                                                                                                                                                                                   
                                                         </table>
                                                    </div>             
                                                    <div id="CIFINDIVIDUAL">
                                                        <table id="tblCIFIndividual" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id="tdMainInfo" class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    Client Information&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >CIF ID:&nbsp;</td>
                                                                <td colspan="4" align="left" style="height: 25px;">
                                                                    &nbsp;<input id="txtCIFID" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled"/>
                                                                    </td>
                                                            </tr>                                                            
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >Last Name:&nbsp;</td>
                                                                <td colspan="4" align="left" style="height: 25px;">
                                                                    &nbsp;<input id="txtLastname" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled"/>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >First Name:&nbsp;</td>
                                                                <td colspan="4" align="left">
                                                                    &nbsp;<input id="txtFirstname" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >Middle Name:&nbsp;</td>
                                                                <td colspan="4" align="left" style="height: 25px">
                                                                    &nbsp;<input id="txtMiddlename" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled" /></td>
                                                            </tr>
                                                            <%--<tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    Address1:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    &nbsp;<input id="txtIndAlias" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    Address2:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    &nbsp;<input id="Text1" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    Address3:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    &nbsp;<input id="Text2" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled" /></td>
                                                            </tr>        
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    Address4:&nbsp;</td>
                                                                <td align="left" colspan="4" style="height: 25px">
                                                                    &nbsp;<input id="Text3" type="text" maxlength="200" style="text-align: left; width: 85%;" disabled="disabled" /></td>
                                                            </tr>                   --%>                                                                                              
<%--                                                            <tr style="display:block">
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >Title:&nbsp;</td>
                                                                <td align="left" style="width: 30%; height: 24px">
                                                                    &nbsp;<input id="txtTitle" style="width: 94%" type="text"  maxlength="200" disabled="disabled"/></td>
                                                                <td class="cell_caption" style="width: 14%; height: 24px" >Birthplace:&nbsp;</td>
                                                                <td align="left" style="height: 24px" colspan="2">
                                                                    &nbsp;<input id="txtBirthplace" type="text"  maxlength="200" style="width: 67%" disabled="disabled" /></td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >Birth Date:&nbsp;</td>
                                                                <td align="left" style="width: 30%; height: 24px">
                                                                    &nbsp;<input id="txtBirthday" style="width: 94%" type="text"  maxlength="200" disabled="disabled"/>                                                                    
                                                                </td>
                                                                <td class="cell_caption" style="width: 14%; height: 24px" >
                                                                    Age:&nbsp;</td>
                                                                <td id="Td1" runat="server" align="left" colspan="2" style="height: 24px">
                                                                    &nbsp;<input id="txtAge" type="text" style="width: 67%" maxlength="4" disabled="disabled" /></td>
                                                            </tr>
                                                            <%--<tr style= "display:none;">
                                                                <td class="cell_caption" style="width: 18%; height: 25px" >
                                                                    State:&nbsp;</td>
                                                                <td align="left" style="width: 30%; height: 25px">
                                                                    &nbsp;<input id="txtCitizenship" style="width: 94%" type="text"  maxlength="200" disabled="disabled"/></td>
                                                                <td class="cell_caption" style="width: 14%; height: 25px" >
                                                                    Tel. No.:&nbsp;</td>
                                                                <td align="left" colspan="2" style="height: 25px">
                                                                    &nbsp;<input id="txtCivilStatus" type="text"  maxlength="200" style="width: 67%" disabled="disabled" /></td>
                                                            </tr>
                                                            <tr style= "display:none;">
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >
                                                                    Country:&nbsp;</td>
                                                                <td align="left" style="width: 30%; height: 24px">
                                                                    &nbsp;<input id="txtReligion" style="width: 94%" type="text"  maxlength="200" disabled="disabled"/></td>
                                                                <td class="cell_caption" style="width: 14%; height: 24px" >
                                                                    Mobile No.:&nbsp;</td>
                                                                <td align="left" colspan="2" style="height: 24px">
                                                                    &nbsp;<input id="txtGender" type="text"  maxlength="200" style="width: 67%" disabled="disabled" /></td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >Status:&nbsp;</td>
                                                                <td align="left" style="width: 30%; height: 24px">
                                                                    &nbsp;<input id="txtStatus" style="width: 94%" type="text"  maxlength="200" disabled="disabled"/></td> 
                                                                <td class="cell_caption" style="width: 14%; height: 24px;">
                                                                    Branch:&nbsp;</td>
                                                                 <td align="left" colspan="2" style="height: 24px">
                                                                    &nbsp;<input id="txtBranch" type="text"  maxlength="200" style="width: 67%" disabled="disabled" /></td>
                                                            </tr>                                                                            
                                                        </table>
                                                    </div>
                                                    <div id = "GRID1" style= "display:block">
                                                        <table id="Table14" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                <tr>
                                                                    <td id="tdGRID1" class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                        Beneficiaries</td>
                                                                </tr>
                                                                <tr> 
                                                                    <td style="height: 74px">
                                                                        <div id = "CIFIDGRIDBENEF" style= "display:block">
                                                                            <table id="tblGridIds" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                                <tr>
                                                                                    <td style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="height: 2px"><div id = "CIFIDGRID" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
                                                                                </tr>
                                                                            </table>
                                                                       </div>                                                                    
                                                                    </td>
                                                                </tr> 
                                                        </table>
                                                    </div>
                                                    <div id = "GRIDOTHERDETAILS">
                                                         <div id = "GRIDADDRESS" style= "display:block" onmouseover = "javascript:GFC_Main.change_other_grid_mode(0);">
                                                            <table id="Table17" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                    <tr>
                                                                        <td id="td3" class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                            Address(es)</td>
                                                                    </tr>
                                                                    <tr> 
                                                                        <td style="height: 74px">
                                                                            <div id = "GRIDBENEFADDRESS" style= "display:block">
                                                                                <table id="Table18" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                                    <tr>
                                                                                        <td style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 2px"><div id = "Div5" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
                                                                                    </tr>
                                                                                </table>
                                                                           </div>                                                                    
                                                                        </td>
                                                                    </tr> 
                                                            </table>
                                                        </div>
                                                        <div id = "GRIDCONTACTS" style= "display:block" onmouseover = "javascript:GFC_Main.change_other_grid_mode(1);">
                                                            <table id="Table19" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                    <tr>
                                                                        <td id="td4" class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                            Contact(s)</td>
                                                                    </tr>
                                                                    <tr> 
                                                                        <td style="height: 74px">
                                                                            <div id = "GRIDBENEFCONTACTS" style= "display:block">
                                                                                <table id="Table20" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                                    <tr>
                                                                                        <td style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 2px"><div id = "Div6" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
                                                                                    </tr>
                                                                                </table>
                                                                           </div>                                                                    
                                                                        </td>
                                                                    </tr> 
                                                            </table>
                                                        </div>
                                                        <div id = "GRID2" style= "display:block"  onmouseover = "javascript:GFC_Main.change_other_grid_mode(2);">
                                                            <table id="Table15" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                    <tr>
                                                                        <td id="td2" class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                            Card History</td>
                                                                    </tr>
                                                                    <tr> 
                                                                        <td style="height: 74px">
                                                                            <div id = "GRIDCARDHISTORY" style= "display:block">
                                                                                <table id="Table16" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                                                    <tr>
                                                                                        <td style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 2px"><div id = "Div4" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
                                                                                    </tr>
                                                                                </table>
                                                                           </div>                                                                    
                                                                        </td>
                                                                    </tr> 
                                                            </table>
                                                        </div>
                                                    </div>
<%--                                                   <div id = "CIFACCOUNTSMAIN">                                                                                      
                                                        <table id="tblAccounts" width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="6">
                                                                    Card Reference No. (CK Acct. No.)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 14%; height: 23px">Account No.:&nbsp;</td>
                                                                <td align="left" style="width: 272px">
                                                                    &nbsp;<input id="txtAccountNo" type="text" style="width: 95%; height: 100%; text-align: left;" maxlength="50" disabled="disabled" /></td>
                                                                <td align="left" style="width: 100px"><input id="btnGETCK"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "GET" style="width: 56px" onclick="javascript:GFC_Main.btnGETCK_click();"/></td>                                                                    
                                                                <td class="cell_caption" style="width: 13%; height: 23px">
                                                                    Currency:&nbsp;</td>
                                                                <td align="left" style="width: 23%; height:23px">
                                                                    <label id= 'lblMode' class='cell_desc' style='font-size: 15px; font-weight:normal;'>
                                                                        &nbsp;<select id='ddlCurr' style="width: 46%" ><option selected='selected'></option></select></label></td>
                                                            </tr>
                                                         </table>                                                                              
                                                     </div>  --%>    
                                                     <div id = "SHOWHIDEOTHERDETAILS">  
                                                        <table id="Table21" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "Table22">
                                                                        <tr>
                                                                            <td style="width: 18%"><input id="btnShowOtherDetails"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Show Other Details" style="width: 174px" onclick="javascript:GFC_Main.btnShowOtherDetails_click();" /></td>                                                                            
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div>    
                                                     <div id = "CIFACCOUNTSMAIN">                                                                                      
                                                        <table id="tblAccounts" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="6">
                                                                    CK Reference No. (CK Acct. No.)</td>
                                                            </tr>
                                                            <tr>                                                                
                                                                <td class="cell_caption" style="width: 13%; height: 23px">
                                                                    Currency:&nbsp;</td>
                                                                <td align="left" style="width: 11%; height:23px">
                                                                    <label id= 'lblMode' class='cell_desc' style='font-size: 15px; font-weight:normal;'>
                                                                        &nbsp;<select id='ddlCurr' style="width: 88%" ><option selected='selected'></option></select></label></td>
                                                                       <td class="cell_caption" style="width: 14%; height: 23px">Account No.:&nbsp;</td>
                                                                <td align="left" style="width: 199px">
                                                                    &nbsp;<input id="txtAccountNo" type="text" style="width: 123%; height: 100%; text-align: left;" maxlength="50" disabled="disabled" /></td>
                                                                <td align="left" style="width: 100px"><input id="btnGETCK"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "GET" style="width: 56px" onclick="javascript:GFC_Main.btnGETCK_click();"/></td> 
                                                            </tr>
                                                         </table>                                                                              
                                                     </div>                                              
                                                    <div id = "CIFCARDSMAIN">
                                                        <table id="tblCards" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr><td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="6">
                                                                Global Filipino Card Number</td></tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 24px">
                                                                    GFC No.:&nbsp;</td>
                                                                <td align="left" colspan="4" style="width: 82%; height: 24px;"><input id="txtCardNo" type="text" style="width: 85%; height: 100%; text-align: left;" maxlength="200" /></td>                                                                
                                                            </tr>
                                                            <tr id="trCardType">
                                                                <td class="cell_caption" style="width: 18%; height: 26px">
                                                                    GFC Type:&nbsp;</td>
                                                                <td align="left" colspan="4" style="width: 82%; height: 26px;"><input id="txtCardType" type="text" style="width: 85%; height: 100%; text-align: left;" maxlength="200" disabled="disabled" /></td>                                                                
                                                            </tr>
                                                            <tr id="trClaimeDate">
                                                                <td class="cell_caption" style="width: 18%; height: 21px">
                                                                    Date Claimed:&nbsp;</td>
                                                                <td align="left" colspan="4" style="width: 82%; height: 21px;"><input id="txtDateClaimed" type="text" style="width: 85%; height: 100%; text-align: left;" maxlength="200" disabled="disabled" /></td>
                                                            </tr>
                                                            <tr id="trIDPresented"> 
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >
                                                                    *ID's Presented:&nbsp;</td>
                                                                <td align="left" style="width: 82%; height: 24px;" colspan="4" ><input id="txtIDPresented" type="text" style="width: 85%; height: 100%; text-align: left;" maxlength="255" />
                                                                </td>                                                                                                    
                                                            </tr>   
                                                            <tr id="trReason">
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >
                                                                    Reason:&nbsp;</td>
                                                                <td align="left" style="width: 82%; height: 24px;" colspan="4">
                                                                    &nbsp;<select id="ddlReason" style="width: 85%" onchange="javascript:GFC_Main.ddlReason_OnChange();">
                                                                        <option selected="selected"></option>
                                                                    </select></td>                                                                                                    
                                                            </tr>     
                                                            <tr id="trSpecifyReason">
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >
                                                                    *Specify:&nbsp;</td>
                                                                <td align="left" style="width: 82%; height: 24px;" colspan="4"><input id="txtSpecify" type="text" style="width: 85%; height: 100%; text-align: left;" maxlength="255" />
                                                                </td>                                                                                                    
                                                            </tr>   
                                                            <tr id = "trNewGFCNo">
                                                                <td class="cell_caption" style="width: 18%; height: 24px" >
                                                                    New GFC No:&nbsp;</td>
                                                                <td align="left" style="width: 82%; height: 24px;" colspan="4"><input id="txtNewCardNo" type="text" style="width: 85%; height: 100%; text-align: left;" maxlength="255" />
                                                                </td>                                                                                                    
                                                            </tr>                                                                                                                                                                                                                                                                                                
                                                         </table>                                                                             
                                                   </div>
                                                  <div id = "BATCHENROLLMENT">
                                                    <div id = "dvGROUP">                                                                                      
                                                        <table id="Table11" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="6">
                                                                    Batch Enrollment</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 18%; height: 23px">
                                                                    Group:&nbsp;</td>
                                                                <td align="left" style="width: 272px" colspan="5">
                                                                    &nbsp;<select id='ddlGroups' style="width: 84%" onchange="javascript:GFC_Main.ddlGroups_OnChange();"><option selected='selected'></option></select></td>                                                                
                                                            </tr>
                                                         </table>                                                                              
                                                     </div>      
                                                    <div id = "dvCARDLIST" style= "display:block">
                                                        <table id="Table10" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="5"></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 2px"><div id = "Div2" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
                                                            </tr>
                                                        </table>
                                                    </div>        
                                                  </div>    
                                                   <div id = "CARDREPCANBTN">  
                                                        <table id="Table3" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table style="width:70%;" cellpadding="0" cellspacing="5px" id = "Table4">
                                                                        <tr>
                                                                            <td style="width: 18%"><input id="btnReplacement"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "For Replacement" style="width: 174px" onclick="javascript:GFC_Main.btnReplacement_click();" /></td>
                                                                            <td style="width: 18%"><input id="btnCancellation"   class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "For Cancellation" style="width: 174px" onclick="javascript:GFC_Main.btnCancellation_click();"/></td>                                                                                                                                                                                        
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div>      
                                                   <div id="REASONDIALOG" style="z-index:101;position:absolute;left:294px;color:darkblue;border:solid 2px #000;padding:10px;display:block; font-family: Arial; top: 161px;">
                                                        <table style="width:70%" cellpadding="0" cellspacing="6">
                                                            <tr>
                                                                <td class="table_caption" colspan="4" style="color: black; text-indent: 5px; text-align: left">Reason for Rejection*</td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" class="label1" align="left" style="width: 70%">&nbsp;<input id="txtReason" type="text" style="width: 98%; height: 100%; text-align: left;" maxlength="100" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td id="capMandatory" colspan="4" class="comment" align="left" style="width:70%; height: 14px;display: none">
                                                                &nbsp;<label id="lblMandatory">* Mandatory</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4" class="label1" align="right" style="height: 20px">
	                                                                <input id="btnProceed" type="button" value="Ok" style="width:60px;" onclick="javascript:GFC_Main.btnProceed_click();"/>
	                                                                <input id="btnCloseReasonDialog" type="button" value="Cancel" style="width:60px;" onclick="javascript:GFC_Main.closeReasonDialog_click();"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                   </div>                                                    
                                                   <div id = "APPRREJECTBTN">  
                                                        <table id="Table5" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%; height: 51px;">                                                                                                    
                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "Table6">
                                                                        <tr>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnApprove"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Approve" style="width: 174px" onclick="javascript:GFC_Main.btnApprove_click();" /></td>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnReject"   class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Reject" style="width: 174px" onclick="javascript:GFC_Main.btnReject_click();"/></td>                                                                                                                                                                                        
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div>        
                                                   <div id = "SAVECANCELBTN">  
                                                        <table id="Table7" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "Table8">
                                                                        <tr>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnReleaseGFC"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Release GFC" style="width: 156px" onclick="javascript:GFC_Main.btnReleaseGFC_click();"/></td>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnCancel"   class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel" style="width: 166px" onclick="javascript:GFC_Main.btnCancel_click();"/></td>                                                                                                                                                                                        
                                                                         </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div> 
                                                   <div id = "FORMS">  
                                                        <table id="Table12" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "Table13">
                                                                        <tr>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnReleaseForm"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Print Release Form" style="width: 154px" onclick="javascript:GFC_Main.btnReleaseForm_click();"/></td>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnTermsCondition"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Print Terms & Condition" style="width: 168px" onclick="javascript:GFC_Main.btnTermsCondition_click();"/></td>                                                                                                                                                                                                                                                                    
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div>       
                                                   <div id = "SUBMITCANCELBTN">  
                                                        <table id="Table1" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "Table9">
                                                                        <tr>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnSubmit"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Submit" style="width: 174px" onclick="javascript:GFC_Main.btnSubmit_click();"/></td>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnACancel"   class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel" style="width: 174px" onclick="javascript:GFC_Main.btnACancel_click();"/></td>                                                                                                                                                                                                                                                                    
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div> 
                                                                                                              
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
<%--                                                   <div id = "CIFMAINBUTTON">  
                                                        <table id="tblButtonsMain"  "100%" cellpadding="0" cellspacing="5px" style="background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table width="70%" cellpadding="0" cellspacing="5px" id = "tblbuttons">
                                                                        <tr>
                                                                            <td style="width: 18%"><input id="btnAdd"    class = "button" onclick = "javascript:cif_main.btn_insert();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Add"/></td>
                                                                            <td style="width: 18%"><input id="btnEdit"   class = "button" onclick = "javascript:cif_main.btn_update();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Edit"/></td>                                                                                                            
                                                                            <td style="width: 18%"><input id="btnDelete" class = "button" onclick = "javascript:cif_main.btn_delete();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Delete"/></td>
                                                                            <td style="width: 18%"><input id="btnSave"   class = "button" onclick = "javascript:cif_main.btn_commit();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Save"/></td>
                                                                            <td style="width: 18%"><input id="btnCancel1" class = "button" onclick = "javascript:cif_main.btn_rollback();" onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel"/></td>                                                                                                                
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div> 
                                                   <div id = "ClOSECANCELBTNS">
                                                        <table id="Table1" width="100%" cellpadding="0" cellspacing="5px" style="background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table width="30%" cellpadding="0" cellspacing="5px" id = "tblClOSECANCELBTNS">
                                                                        <tr>
                                                                            <td style="width: 18%"><input id="btnClose"    class = "button" onclick = "closer();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Close"/></td>
                                                                            <td style="width: 18%"><input id="btnCancel_1" class = "button" onclick = "cancel();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel"/></td>  
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>                                                                    
                                                    </div>   --%>
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
        </form>
    </center> 
</body>
</html>
