<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GFCAggregation.aspx.cs" Inherits="IRSWorld.GFC.GFCAggregation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/GFC/jsGFCAggregation.js"                 type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>     
    <script type="text/javascript">        
        var aggr = new GFC_Aggregation();
    </script> 
</head>
<body onload="javascript:aggr.on_load();">
    <center>
        <form id="form1" runat="server">
            <table id="tbl_main" cellpadding="0" cellspacing="5" style="width:100%; height:100%;margin-top: 30px;">                
                <tr>
                    <td style="width: 79%" align="center" valign="top">
                        <%--<div id = "CIFENTRY" style= "display:block; overflow:auto; height:500px;">--%>
                        <div id = "DIVAGGRELIMIT" style= "display:block; overflow:auto; height:220px;">
                            <table style="width:50%;" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" >
                                            <tr>
                                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                    <img src="../images/toe.GIF" /></td>    
                                                <td id='tdHeader' class="table_head_dark" style="width: 50%; height: 24px">
                                                    GFC AGGREGATION</td>
                                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                    <img align="absBottom" src="../images/rend.gif" /></td> 
                                             </tr>                                                                              
                                        </table>    
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan="2" style="background:azure; height: 95px;">
                                        <table id="tblCIF" style="width:100%; border:solid 3px #336699;" >
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div id="CIFSearch">
                                                        <table id="Table2" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id='tdSearchHeader' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                </td>
                                                            </tr>
                                                            <tr id="trLasname">
                                                                <td class="cell_caption" style="width: 20%; height: 25px" >
                                                                    Amount Limit :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtAmount" type="text" maxlength="200" style="text-align: left; width: 98%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="capMandatory" colspan="4" style="width:70%" class="comment" align="left">
                                                                &nbsp;<label id="lblMandatory">Note* Amount limit currency is always in Philippine Peso</label></td>
                                                            </tr> 
                                                           <tr>
                                                                <td width ="20%" align="right" style="height: 22px" colspan="4"><input id="btnCancel"   class = "button" onclick = "javascript:aggr.btnCancel_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Cancel" style="width: 18%"/>&nbsp;
                                                                    <input id="btnUpdate"   class = "button" onclick = "javascript:aggr.btnUpdate_click();"   onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Update" style="width: 18%"/>&nbsp;</td>
                                                            </tr>                                                            
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>                            
                        </div>
                        <div id = "DIVSEARCHAGGRE" style= "display:block; overflow:auto; height:410px;">
                            <table cellspacing="0" cellpadding="0" style="width: 87%">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" >
                                            <tr>
                                                <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                    <img src="../images/toe.GIF" /></td>    
                                                <td id='td1' class="table_head_dark" style="width: 50%; height: 24px">
                                                    GFC AGGREGATION INQUIRY</td>
                                                <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                    <img align="absBottom" src="../images/rend.gif" /></td> 
                                             </tr>                                                                              
                                        </table>    
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan="2" style="background:azure; height: 95px;">
                                        <table id="Table1" style="width:100%; border:solid 3px #336699;" >
                                            <tr>
                                                <td style="width: 100%;">
                                                    <div id="DIVSEARCH">
                                                        <table id="Table3" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id='td2' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    GFC Information</td>
                                                            </tr>
                                                            <tr id="tr1">
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    CIF ID :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtCIFID" type="text" maxlength="200" style="text-align: left; width: 71%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr id="tr6">
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Old
                                                                    CIF ID :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtOldCIFID" type="text" maxlength="200" style="text-align: left; width: 71%;"/>
                                                                    </td>
                                                            </tr> 
                                                            <tr id="tr2">
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Beneficiary Last Name :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtLastName" type="text" maxlength="200" style="text-align: left; width: 71%;"/>
                                                                    </td>
                                                            </tr> 
                                                            <tr id="tr3">
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Beneficiary First Name :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtFirstName" type="text" maxlength="200" style="text-align: left; width: 71%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr id="tr4">
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Beneficiary Middle Name :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtMiddleName" type="text" maxlength="200" style="text-align: left; width: 71%;"/>
                                                                    </td>
                                                            </tr>
                                                            <tr id="tr5">
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Beneficiary GFC Number :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtGFCNumber" type="text" maxlength="200" style="text-align: left; width: 71%;"/>
                                                                    </td>
                                                            </tr>   
<%--                                                            <tr id="tr8">
                                                                <td class="cell_caption" style="width: 31%; height: 25px" >
                                                                    Beneficiary Birthday :&nbsp;</td>
                                                                <td colspan="3" align="left" style="height: 25px;">
                                                                    <input id="txtBirthDay" type="text" maxlength="200" style="text-align: left; width: 71%;"/>
                                                                    </td>
                                                            </tr>   --%>                                                                                                                                                                                                                                              
                                                        </table>
                                                    </div>
                                                    <div id = "DIVSEARCHBTN">  
                                                        <table id="Table4" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%;">                                                                                                    
                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "Table5">
                                                                        <tr>
                                                                            <td style="width: 18%; height: 22px;"><input id="btnSearch"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Search" style="width: 174px" onclick="javascript:aggr.btnSearch_click();" /></td>
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>
                                                    </div>
                                                    <div id = "DIVCIFINFO" style= "display:block"> 
                                                        <table id="tblIndividualHeader" style="width:100%" cellpadding="0">
                                                            <tr>
                                                                <td class="cell_desc" style="width: 11%; height: 24px">
                                                                    Beneficiary Name :</td>
                                                                <td align="left" style="width: 24%; height: 24px;" colspan="4">
                                                                    <label id="lblCustLName" class="cell_desc"></label>&nbsp;
                                                                    <label id="lblCustFName" class="cell_desc"></label>&nbsp;
                                                                    <label id="lblCustMName" class="cell_desc"></label>
                                                                </td>
                                                                <td class="cell_desc" style="width: 7%; height: 24px" >
                                                                    &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;CIF ID :</td>
                                                                <td align="left" style="width: 26%; height: 24px;" colspan="4">
                                                                    <label id="lblCIFID" class="cell_desc">
                                                                        </label></td>
                                                            </tr>        
                                                            <tr>
                                                                <td class="cell_desc" style="width: 11%; height: 24px">
                                                                    &nbsp;&nbsp; Money Card No. :</td>
                                                                <td align="left" style="width: 24%; height: 24px;" colspan="4">
                                                                    <label id="lblGFCNo" class="cell_desc"></label>&nbsp;                                                                    
                                                                </td>
                                                                <td class="cell_desc" style="width: 7%; height: 24px" >
                                                                    Birthday :</td>
                                                                <td align="left" style="width: 26%; height: 24px;" colspan="4">
                                                                    <label id="lblOldCIFID" class="cell_desc">
                                                                        </label></td>
                                                            </tr>                                                                                                                                                                             
                                                         </table>
                                                    </div>           
                                                    <div id="DIVAGGREPARAM">
                                                        <table id="Table6" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id='td3' class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    Aggregation Parameters</td>
                                                            </tr>
                                                            <tr id="tr7">
                                                                <td class="cell_caption" style="width: 22%; height: 25px" >
                                                                    Month :&nbsp;</td>
                                                                <td align="left" style="width: 22%; height: 25px">
                                                                    &nbsp;<select id="ddlMonth" style="width: 95%"><option selected="selected"></option>
                                                                    </select></td>
                                                                   <td class="cell_caption" style="width: 5%; height: 25px" >
                                                                       Year :&nbsp;</td>
                                                                <td align="left" style="width: 22%; height: 25px">
                                                                    &nbsp;<input id="txtYear" type="text" maxlength="200" style="text-align: left; width: 44%;"/></td> 
                                                            </tr>                                                                                                                                                                        
                                                        </table>
                                                        <table id="Table7" cellpadding="0" cellspacing="5px" style="width:100%; background:azure;">
                                                            <tr>
                                                                <td align="center" colspan="3" style="width: 100%; height: 51px;">                                                                                                    
                                                                    <table style="width:70%" cellpadding="0" cellspacing="5px" id = "Table8">
                                                                        <tr>
                                                                            <td style="width: 18%"><input id="btnPreview"    class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Preview" style="width: 174px" onclick="javascript:aggr.btnPreview_click();" /></td>
                                                                            <td style="width: 18%"><input id="btnReport"   class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Report" style="width: 174px" onclick="javascript:aggr.btnReport_click();"/></td>
                                                                            <td style="width: 18%"><input id="btnBackToSearch"   class = "button"    onmouseover = "this.className='button_over';" onmouseout = "this.className = 'button';" type="button" value = "Back To Search" style="width: 174px" onclick="javascript:aggr.btnBack_click();"/></td>                                                                                                                                                                                        
                                                                        </tr>
                                                                    </table>                                                                                                    
                                                                </td>
                                                            </tr>                                                                    
                                                        </table>                                                        
                                                    </div>
                                                    <div id = "DIVAGGREDETAILS" style="display: block; overflow: auto; height:350px;" >
                                                    <table id="Table19" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                            <tr>
                                                                <td id="td4" class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left">
                                                                    Aggregation Details</td>
                                                            </tr>
                                                            <tr> 
                                                                <td style="height: 74px">
                                                                    <div id = "AGGREDETAILS" style= "display:block">
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
