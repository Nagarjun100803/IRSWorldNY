<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranForVerificationSearch.aspx.cs" Inherits="IRSWorld.Remittance.RemTranForVerificationSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Transaction Viewer</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>
    <script src="../js/Reports/js_ReportsTemplate.js" type="text/javascript"></script>              
    <script src="../js/Remittance/jsremittersearch.js" type = "text/javascript"></script>
    <script src="../js/Remittance/jr_forverification_search.js" type = "text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    
    <script type="text/javascript">
        var rem_search = new for_verification_search();
    
        function fix_height()
        {
            if (top.document.getElementById("divCenter"))
            {
                var cent = top.document.getElementById("divCenter");
                var div = document.getElementById("divmain");
                if (div)
                {
                    div.style.width = cent.offsetWidth + "px";
                    div.style.height = cent.offsetHeight + "px";
                    
                    var divr = document.getElementById('div_results');
                    if (divr)
                    {
                        divr.style.height = div.style.height;
                    }
                }
            }
        }    
    </script>
</head>
<body onload="fix_height(); javascript:rem_search.reload_param();" onresize="fix_height();">
    <form id="form1" runat="server">
    <div id=divmain style="width:100%; height:100%; padding-top: 30px;">
    <table style="width:100%" cellpadding="0" cellspacing="5">
        <tr>
            <td id="tdMenu" style=" vertical-align:top; width:18%; overflow: auto">
                <div id="divShowMenu" style=" display: none; width: 100%">
                    <table  style="width:100%" cellpadding="0" cellspacing="1">
                        <tr>
                            <td style="width: auto"></td>
                        </tr>
                        <tr>
                            <td style="text-align: right; border-bottom: solid 1px #5697c1; width: auto; height: 20px;">
                                <a id="A4" href="javascript:rem_search.show_hide_menu('0');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">>></a>
                                
                            </td>
                        </tr>    
                    </table>    
                </div>
                <div id="divSearchMenu"  style=" display: block; width:100% ">
                    <table  style="width:100%" cellpadding="0" cellspacing="1">
                        <tr>
                            <td  align="left";style="width: 100px"></td>
                        </tr>
                        <tr>
                            <td   style="text-align: right; border-bottom: solid 1px #5697c1; width: auto;">
                               <a id="Master" href="javascript:rem_search.open_division('div_search');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">New Search</a>
                           
                            </td>
                        </tr>
                         <%--<tr>
                            <td  style="text-align: right; border-bottom: solid 1px #5697c1; width: auto;">
                                <a id="A1" href="javascript:rem_search.load_forapproval();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Batch Approval</a>
                                
                            </td>
                        </tr>--%>
                        
                         <tr>
                            <td  style="text-align: right; border-bottom: solid 1px #5697c1; width: auto;">
                                <a id="A3" href="javascript:rem_search.show_hide_menu('1');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"><<</a>
                                
                            </td>
                        </tr>
                    </table>
                 </div>
            </td>
            
             <td id="tdMain" style="width: 82%; vertical-align:top";>
           
                 <center>   
                    <div id="div_results"   style="width: 100%;  display: block; overflow:auto; height:90%; vertical-align:top">
                        <table style="width:100%">
                            <tr>
                                <td style="width: 85%;">
                                    <table height="100%" cellspacing="0" cellpadding="0" style="width:98%;background-color:Azure">
                                        <tr>
                                            <td style="width:100%; vertical-align:bottom" align=left>
                                                <table style="width: 100%" cellpadding="0" cellspacing="0" >
                                                <tr>
                                                <td style="width:60%; vertical-align:bottom" align=left>
                                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                        <td align="left" style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom;">
                                                            <img src="../images/toe.GIF" /></td>
                                                        <td id="tdStatus" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                                            Viewer</td>
                                                        <td align="left" style="width:60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                                        </tr>
                                                    </table>
                                                </td>
                                                </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 97%">
                                                <table style="width:100%; height: 100%;" class="table_border">
                                                    
                                                    <tr>
                                                        <td style="width: 100%; height: 100%; vertical-align: top">
                                                            <!-- content starts here -->
                                                             
                                                                <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%;border: solid 1px skyblue">
                                                                    <tr>
                                                                        <td class="table_caption" style="width: 100%"; >List of Transactions
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" valign="middle" colspan = "2">
                                                                            <div id = "DIV_REMITTANCE_LIST" onmousemove = "javascript:rem_search.active_grid = 'DIV_REM_TXN';">
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                
                                                                <table id = "tblRemBeneTransaction" style="width:100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width:100%;" valign="top">
                                                                            <div id = "div1">
                                                                            </div>
                                                                        </td>                                                                
                                                                    </tr>
                                                                </table>                                                                                                   
                                                            <!-- content ends here -->
                                                            
                                                            <div id="divProcess">
                                                                <div id="divLoadingMsg">
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 8px">
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
                </center>                  
                                                 
                <center>
                    <br />
                    <div id="div_search"  style="width:100%; display: none; ">
                        <table cellspacing="0" cellpadding="0" style="width: 60%">
                            <tr>
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:1%; height:24px; border-collapse:collapse; vertical-align:bottom;">
                                            <img src="../images/toe.GIF" /></td>
                                        <td id="capSearch" class="table_head_dark" style="width: 30%; height: 24px; border-collapse:collapse" align=left>
                                            Search</td>
                                        <td align="left" style="width: 54%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                
                                    
                                    <table style="width:100%;" class="table_border">
                                        <tr>
                                            <td style="width: 92%; height: 21px;">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 92%; height: 49px;">
                                                <!-- content starts here -->
                                                <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
                                                    <%--<tr>
                                                        <td class="table_caption" style="width: 40%"; >
                                                            Type of Remittance:</td>    
                                                        <td class="table_caption" style="width: 50%;" align="left"><select id="ddlSearchClass" size="1"  style="width: 90%; text-align: left;" onchange="javascript:rem_search.manipulate_typeofremittance_choices2();" onclick=""></select></td>                                                        
                                                    </tr>--%>
                                                    <tr>
                                                        <td style="width:100%;height: 115px" align="center" valign="middle" colspan = "2">
                                                            <%--<div id="div_batchupload_details" style="width:100%; display: none;">
                                                                <table style="width:96%" cellspacing="5" cellpadding="0" id="Table1">
                                                                     <tr>
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 21px;";>&nbsp;File Type :</td>
                                                                    <td align="left" style="width: 42%; height: 21px;";><select id="ddlFiletype" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.get_batchuploaf_fileformat();" >
                                                                    </select>
                                                                    </td> 
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 21px;";>&nbsp;File Format :</td>
                                                                    <td align="left" style="width: 42%; height: 21px;";><select id="ddlFileFormat" size="1"  style="width: 95%; text-align: left;" >
                                                                    </select>
                                                                    
                                                                    </td> 
                                                                </tr>
                                                                
                                                                </table>
                                                             </div>--%>
                                                            <div> 
                                                            <table style="width:96%" cellspacing="5" cellpadding="0" id="tblSearch">
                                                                
                                                                 <%--<tr id="trTranxStatus" class="cell_desc">
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 21px;";>&nbsp;Transaction Status:</td>
                                                                    <td align="left" style="width: 42%; height: 21px;";><select id="ddltranxstatus" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.manipulate_search_choices();">
                                                                    </select>
                                                                    </td> 
                                                                </tr>--%>
                                                               
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="width: 32%";>&nbsp;Transaction Date <b>(mm/dd/yyyy)</b> :</td>
                                                                    <td align="left" style="width: 42%";><input id="txtsearchtranxdate" type="text" style="width: 85%;text-align: left; font-size: medium;"  disabled="disabled"/>
                                                                    <img id="imgCaloverride" src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtsearchtranxdate, 'mm/dd/yyyy');"/>
                                                                    </td> 
                                                                    
                                                                </tr>
                                                                <%--<tr id="trclearingdate">
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 25px;";>&nbsp;Clearing Date <b>(mm/dd/yyyy)</b> :</td>
                                                                    <td align="left" style="width: 42%; height: 25px;";><input id="txtsearchclearingdate" type="text" style="width: 85%;text-align: left; font-size: medium;" disabled="disabled" />
                                                                    <img id="img1" src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txtsearchclearingdate, 'mm/dd/yyyy');"/>
                                                                    </td> 
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 21px;";>&nbsp;Pay Mode :</td>
                                                                    <td align="left" style="width: 42%; height: 21px;";><select id="ddlpaymode" size="1"  style="width: 95%; text-align: left;" >
                                                                    </select>
                                                                    </td> 
                                                                </tr>
                                                                
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 21px;";>&nbsp;Product Code :</td>
                                                                    <td align="left" style="width: 42%; height: 21px;";><select id="ddlproductcode" size="1"  style="width: 95%; text-align: left;" >
                                                                    </select>
                                                                    </td> 
                                                                </tr>
                                                                
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 21px;";>&nbsp;Implementation Type :</td>
                                                                    <td align="left" style="width: 42%; height: 21px;";><select id="ddlimplemtype" size="1"  style="width: 95%; text-align: left;" >
                                                                    </select>
                                                                    </td> 
                                                                </tr>--%>
                                                                
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 25px;";>&nbsp;Reference No. :</td>
                                                                    <td align="left" style="width: 42%; height: 25px;";><input id="txtsearchrefno" type="text" style="width: 94%;text-align: left; font-size: medium;" /></td> 
                                                                </tr>
                                                           
                                                                <%--<tr id="trbatchno">
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 8px;";>&nbsp;Batch Internal No. :</td>
                                                                    <td align="left" style="width: 42%; height: 8px;";><input id="txtbatchno" type="text" style="width: 94%;text-align: left; font-size: medium;" /></td> 
                                                                </tr>
                                                                 <tr id="tr1">
                                                                    <td align="right" class="cell_desc" style="width: 32%; height: 8px;";>&nbsp;ORD Batch No. :</td>
                                                                    <td align="left" style="width: 42%; height: 8px;";><input id="txtordbatchno" type="text" style="width: 94%;text-align: left; font-size: medium;" /></td> 
                                                                </tr>--%>
                                                           </table> 
                                                           </div> 
                                                       </td> 
                                                   </tr> 
                                                   <tr id="trseach">
                                                        <td style="height: 28px" valign="top" colspan = "2">
                                                            <input type="button" value="Search"  onclick = "javascript:rem_search.remittance_search();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnsearch" />&nbsp;
                                                            <input type="button" value="Clear"  onclick = "javascript:rem_search.clear_search_entry();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnclear" />
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
                    </div>
                </center> 
                                           
                <center>
                    <br />
                    <div id="div_tranxdetails" style="width:100%; display: none;">
                        <table cellspacing="0" cellpadding="0" style="width: 60%">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                                <td id="Td1" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">Batch Approval</td>
                                <td align="left" valign="bottom" style="height: 24px; width: 197px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                
                                    
                                    <table   style="width:100%; " class="table_border">
                                        <tr>
                                            <td style="width: 92%; height: 19px;" align="right">
                                                
                                                
                                                <a id="A2" href="javascript:rem_search.load_forapproval_summary();" style="font-weight:bold; font-size: 12px; font-style:italic ; font-family: Arial;">Summary</a>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 92%; height: 49px;">
                                                <!-- content starts here -->
                                                <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
                                                     <tr>
                                                                <td  align="right"; class="table_caption" style="width: 40%; height: 26px;"; >
                                                                    Type of Remittance: </td>    
                                                                <td class="table_caption" style="width: 38%; height: 26px;" align="left"><select id="ddlotherchannels" size="1"  style="width: 94%; text-align: left;" onchange="javascript:rem_search.manipulate_typeofremittance_choices1();" onclick=""></select></td>                                                        
                                                            </tr>
                                                    <tr>
                                                        <td style="width:100%;height: 115px" align="center" valign="middle" colspan = "2">
                                                            <div id="divbatchupload" style="width:100%; display: none;">
                                                                <table style="width:96%; height:50%" cellspacing="5" cellpadding="0"  >
                                                                 <tr>
                                                                    <td  align="right" style="height: 21px; width: 32%;"  class="cell_desc">
                                                                        &nbsp;File Type:&nbsp;</td>
                                                                    <td style="height: 21px; width: 42%;"  align="left"><select id="ddlfiletype_forapproval" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.get_batchuploaf_fileformat_forapproval();">
                                                                    </select>
                                                                    </td>
                                                                    
                                                                 </tr>
                                                                 <tr>
                                                                    <td  align="right" style="height: 21px; width: 32%;"  class="cell_desc">
                                                                        &nbsp;File Format:&nbsp;</td>
                                                                    <td style="height: 21px; width: 42%;"  align="left"><select id="ddlfileformat_forapproval" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.manipulate_search_choices();">
                                                                    </select>
                                                                    </td>
                                                                    
                                                                 </tr>
                                                                </table>
                                                            </div>
                                                            <div id="divbatchupload2">
                                                            <table cellspacing="5" cellpadding="0"  style="width:96%; height:100%">
                                                             
                                                           
                                                            <tr>
                                                                <td  align="right" style="height: 21px; width: 32%;" class="cell_desc">
                                                                    &nbsp;Transaction Status:&nbsp;</td>
                                                                <td style="height: 21px; width: 42%;" align="left"><select id="ddltranstatus2" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.manipulate_search_choices();"></select>
                                                            </tr>
                                                            <tr>
                                                                <td  align="right" style="height: 21px; width: 32%;" class="cell_desc">
                                                                    No of Transactions to Process:&nbsp;</td>
                                                                <td style="height: 21px; width: 42%;" align="left"><input id="txtnumtrans" type="text" style="width: 88%;text-align: left;"  maxlength="10"/></td>
                                                            </tr>
                                                            
                                                             <tr>
                                                                <td  align="right" style="height: 21px; width: 32%;" class="cell_desc">
                                                                    Transaction Date [mm/dd/yyyy]:</td>
                                                                <td style="height: 21px; width: 42%;" align="left">
                                                                    <input id="txttranxdate2" type="text" style="width: 84%;text-align: left;"  disabled="disabled"/>&nbsp;
                                                                    <img id="img2" src="../images/calendar/imgcalen.png" onclick="popUpCalendar(this, document.forms[0].txttranxdate2, 'mm/dd/yyyy');"/>
                                                                </td>
                                                                
                                                            </tr>
                                                             <tr>
                                                                <td  align="right" style="height: 21px; width: 32%;" class="cell_desc">
                                                                    &nbsp;Paymode:</td>
                                                                <td style="height: 21px; width: 42%;" align="left"><select id="ddlpaymode2" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.manipulate_search_choices();">
                                                                </select>
                                                                </td>
                                                                    
                                                              </tr>
                                                               <tr>
                                                                <td  align="right" style="height: 21px; width: 32%;" class="cell_desc">
                                                                    &nbsp;Product Code:</td>
                                                                <td style="height: 21px; width: 42%;" align="left"><select id="ddlproductcode2" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.manipulate_search_choices();">
                                                                </select>
                                                                </td>
                                                                    
                                                              </tr>
                                                               <tr>
                                                                <td  align="right" style="height: 21px; width: 32%;" class="cell_desc">
                                                                    &nbsp;Implementation Type:</td>
                                                                <td style="height: 21px; width: 42%;" align="left"><select id="ddlimplemtype2" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.manipulate_search_choices();">
                                                                </select>
                                                                </td>
                                                                    
                                                              </tr>
                                                               <tr>
                                                                <td  align="right" style="height: 21px; width: 32%;" class="cell_desc">
                                                                    &nbsp;ORD Batch No.:</td>
                                                                <td style="height: 21px; width: 42%;" align="left"><select id="ddlORDBatchNo" size="1"  style="width: 95%; text-align: left;" onchange="javascript:rem_search.manipulate_search_choices();">
                                                                </select>
                                                                </td>
                                                                    
                                                              </tr>
                                                              
                                                        </table>
                                                        </div>
                                                       </td> 
                                                   </tr> 
                                                   <tr id="tr4">
                                                        <td style="height: 28px" valign="top" colspan = "2">
                                                            <table style="width:100%">
                                                                <tr>
                                                                    <td align="right"  style="width:60%">
                                                                        <input  onmouseover="style.cursor='hand'" id="Button2" onclick = "javascript:rem_search.validate_forapproval_processing();" class="button" style="width: 118px; "
                                                                        type="button" value="Approve" />
                                                                   </td>
                                                                   <td class="cell_desc" align="right">
                                                                         Review Details
                                                                        <input    id="chkreviewdetails" type="checkbox" style="width:20px ; border:0" checked="CHECKED" /> 
                                                                   </td>
                                                                </tr>
                                                            </table>
                                                            <%--<input  onmouseover="style.cursor='hand'" id="Button1" onclick = "javascript:rem_search.validate_forapproval_processing();" class="button" style="width: 118px"
                                                        type="button" value="Approve" />   <p class="cell_desc">Review Details</p>
                                                            <input    id="Checkbox1" type="checkbox" style="width:20px ; border:0" checked="CHECKED" />    
                                                            </td>
                                                            --%>
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
                    </div>
                </center> 
            </td>
        </tr>
        
    </table>
     <div id="div_forapproval"   style="width:85%;z-index:101;position:absolute;left:42px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 1091px;width:971px;height:250px;">
           <div style="height:215px; border: solid 1px skyblue">
           <table style="width:100%">
           <tr>
               <td style="height:50px">
                       
                       <table style="width:100%" align="center">
                        <tr>
                            <td align="center">
                                Summary
                            </td>
                        </tr>
                        
                        <tr>
                            
                            <td align="center" style="height: 25px">
                                <div id="div_forapproval_list"> </div>
                            </td>
                        </tr>
                            <tr >
                            <td>
                                <table style="width:100%" align="center">
                                    <tr style ="height: 19px">
                                        <td class = "cell_caption" style="width: 7%; text-align: right">For Approval :</td>
                                        <td class = "cell_desc" style="width: 30%; height: 19px; text-align: left"><label id = "lblCountForApproval"></label></td>
                                            
                                        
                                    </tr>
                                    <tr style ="height: 19px">
                                      
                                        <td class = "cell_caption" style="width: 7%; text-align: right">For Re-Process :</td>
                                        <td class = "cell_desc" style="width: 30%; text-align: left"><label id = "lblCountForReprocess"></label></td>
                                    </tr>
                                </table>
                             </td>   
                        </tr>
                         
                      
                   </table>
                </td>
           </tr>
           </table>
           </div>
           <div >
           <table style="width:100%">
                <tr>
                            <td align="center" style="height: 24px">
                                <input id="Button3"   class="button" onclick="javascript:hidePanel('div_forapproval');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back" style="width:162px" />
                            </td>
                    </tr>
                    
           </table>
           </div>
    </div >
    <div id="div_reviewdetails"   style="z-index:101;position:absolute;left:43px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 1400px;height:auto; width:95%">
           <div id="div_border" style="height:auto; border: solid 1px skyblue">
           <table style="width:100%">
           <tr>
               <td style="height:180px">
                       
                       <table style="width:100%" align="center">
                        <tr>
                            <td align="center">
                                Summary
                            </td>
                        </tr>
                        
                        <tr>
                            
                            <td align="center" style="height: 25px">
                                <div id="div_forreview_list" style="overflow:auto"> </div>
                            </td>
                         </tr>
                         
                      
                   </table>
                </td>
           </tr>
           </table>
           </div>
           <div >
           <table style="width:100%">
                <tr>
                            <td align="center" style="height: 24px">
                                <input id="btnapprove"   class="button" onclick="javascript:rem_search.process_for_approval_with_viewdetails();" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Approve" style="width:162px" />
                                <input id="btnback"   class="button" onclick="javascript:hidePanel('div_reviewdetails');" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Back" style="width:162px" />
                            </td>
                    </tr>
                    
           </table>
           </div>
    </div >
    </div>
    </form>
</body>
</html>
