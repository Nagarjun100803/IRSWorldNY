<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSApproval.aspx.cs" Inherits="IRSWorld.GRS.GRSApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>GRS Remittance Approval</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>

    <script src="../js/jsCommons.js"                type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"           type="text/javascript"></script>
    <script src="../js/js_sidebar.js"               type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js"            type="text/javascript"></script>
    
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>    
    <script src="../js/Internet/jsInternetPage.js" type="text/javascript"></script>
    <script src="../js/GRS/js_GRSApproval.js" type="text/javascript"></script>    
    <script src ="../js/popcalendar.js"                     type="text/javascript"></script> 
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>    
</head>
<body onload="javascript:ide.init_state();">
<center>
    <form id="form1" runat="server">
    
        <div id="divPopProcess" style="width:250px; height:100px; background:White; z-index: 101; position: absolute; left: 34px;
        color: darkblue; padding: 10px; display: none; font-family: Arial;
        top: 1967px;">
        <table style="width:100%; height:100%;">
            <tr>
                <td>
                    <label id="lblLoadingMsg" style="text-align:center; color:Chocolate ">Processing request, Please wait... </label>
                    <div id="divLoadingMsg">
                        <img src="../images/ajaxloader.gif" id="imgAjaxIcon" />
                    </div>
                </td>
            </tr>
        </table>
        </div>        
        
        <div style="width: 100%">
            <table cellpadding="0" cellspacing="5" width="100%" height="100%">
                <tr>
                    <td align="center" class="" colspan="2" style="width: 100%; border-collapse: collapse;" valign="top">
                        <div style="display: block; width: 100%; height: 100%">
                            <table width="100%">
                                <tr>
                                    <td style="vertical-align: top; width: 20%; padding-top: 30px" valign="top">
                                    
                                    <div id="divDataEntryMenu" style="display:block;">                                    
                                     
                         <table width="100%" cellpadding="0" cellspacing="5">
                            <tr id="trCreateTransaction">
                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                    <a href="javascript:ide.create_new_batch();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Create Batch</a>
                                </td>
                            </tr>
                            <tr id="trTransactionHistory">
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:20px;">
                                    <a href="" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                        Search Batch</a>
                                </td>
                            </tr>
                            <tr id="trSearchCIF" style="display:block;">
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:20px;">
                                    <a href="javascript:internet_page.searchCIF(true);" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Search Client</a>
                                </td>
                            </tr>
                            <tr id="trAddCIF" style="display:block;">
                                <td style="text-align:right;border-bottom:solid 1px #5697c1;height:20px;">
                                    <a href="javascript:show_frame_dialog('../../GRS/Processing/GRSDataEntry.aspx?isRemit=true', 'internetClose', '100%', '100%', '0');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Add Client</a>
                                </td>
                            </tr> 
                        </table>
                    </div>
                                    
                          
                                    
                                    </td>
                                    <td style="width: 83%; height:auto; padding-top: 30px" valign="top" align="center">
                                    
                                    <div id="divisearchbatch" style="display:none" >
                                        &nbsp;<input id="txtctr" type="text" style="width: 11%" />
                                        <input id="txttotitems_ctr" type="text" style="width: 11%" />
                                        <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 1%;">
                                                <tr>
                                                    <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td nowrap align="left" class="table_head_dark" width="20%">
                                                        Search Batch</td>
                                                    <td nowrap align="left" valign="bottom" class="bg_body" style="WIDTH: 324px">
                                                        <img align="absbottom" src="../images/rend.gif" />&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width: 100%; ">
                                                        <table class="table_border" style="width: 100%; ">
                                                            <tr>
                                                                <td style="width: 100%; height: 8px;color:Red" align="center">
                                                                    <table id="Table4" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width: 100%; ">
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                File Format :</td>
                                                                            <td align="left" style="width: 29%; height: 23px">
                                                                                <select id="ddFileFormat" style="width: 147px; height: 21px">
                                                                                    <option selected="selected"></option>
                                                                                </select>
                                                                                *</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                Batch Date (From) :</td>
                                                                            <td align="left" style="width: auto; height: 23px;color:Red"><input id="txtBatchDateFrom" type="text" style="width: 49%;text-align:right;" />
                                                                                <img align="absMiddle" onclick="popUpCalendar(this, document.getElementById('txtBatchDateFrom'), 'mm/dd/yyyy')"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                                *</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                Batch Date (To) :</td>
                                                                            <td align="left" style="width: 29%; height: 23px; color:Red" >
                                                                                <input id="txtBatchDateTo" type="text" style="width: 49%;text-align:right;" />
                                                                                <img align="absMiddle" onclick="popUpCalendar(this, document.getElementById('txtBatchDateTo'), 'mm/dd/yyyy')"
                                                                                    src="../images/calendar/imgcalen.png" />
                                                                                *</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                Batch No :</td>
                                                                            <td align="left" style="width: 29%; height: 23px">
                                                                                <input id="txtBatchNo_" type="text" style="width: 59%;text-align:right;" /></td>
                                                                        </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td  colspan="2" style="height: 30px">
                                                                                        &nbsp;<input id="btnSearch" style="width: 32%; height: 20px;" type="button" value="Search" onclick="javascript:ide.btnSearch_click()" /></td>
                                                                                </tr>
                                                                            </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                                  
                                     
                                    
<%--                                    <div id="diviinquiry" style="display:none" >
                                            <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 1%;">
                                                <tr>
                                                    <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td nowrap align="left" class="table_head_dark" width="20%">
                                                        Search Batch</td>
                                                    <td nowrap align="left" valign="bottom" class="bg_body" style="WIDTH: 400px">
                                                        <img align="absbottom" src="../images/rend.gif" />&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="width: 100%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 100%; height: 8px" align="center">
                                                                    <table id="Table3" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;width: 100%; ">
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 17%; height: 23px">
                                                                                        Batch No :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="Text6" type="text" style="width: 14%" />
                                                                                        <select id="Select3" style="width: 67%; height: 21px">
                                                                                            <option selected="selected"></option>
                                                                                        </select>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td  colspan="2" style="height: 23px">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>--%>
                                    <div id="divibatchholder" style="display:none;height:100%;overflow:auto ;" >
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure" width="96%">
                                        <tr>
                                            <td nowrap align="left" valign="bottom" style="width:1%;height:24px;">
                                                <img src="../images/toe.GIF" /></td>
                                            <td nowrap align="left" class="table_head_dark" width="20%">
                                                Batch Information</td>
                                            <td nowrap align="left" valign="bottom" class="bg_body" style="">
                                                <img align="absbottom" src="../images/rend.gif" />&nbsp;</td>
                                        </tr>
                                        <tr>
                                        <td colspan="3" style="height: 100%">
                                    <div id="diviBatchSummary" style="display:block;" >
                                            <table class="table_border" cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">                                               
                                                <tr>
                                                    <td style="width: 104%; height: 100%">
                                                    <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid" cellpadding="0" cellspacing="8" id="Table6" width="100%">
                                                        <tr>
                                                            <td style="width: 19%; height: 23px;" class="cell_caption" >
                                                                File Source :</td>
                                                            <td style="width: 29%; height: 23px;" align="left">
                                                                <input style="width: 88%" id="txtFileSource" type="text"></td>
                                                            <td >
                                                                </td>
                                                            <td class="comment" style="width: 85%; height: 23px" align="left">
                                                                 <label id="lblMandatory">
                                                                     &nbsp; &nbsp; &nbsp; Amount &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                                     &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; Items</label>
                                                                </td>
                                                        </tr>
                                                        <tr style="font-size: 13pt;">
                                                            <td style="width: 19%; height: 23px" class="cell_caption">
                                                                Batch No :</td>
                                                            <td style="width: 29%; height: 23px" align="left">
                                                                <input style="width: 89%;" id="txtBatchNo" type="text"></td>
                                                            <td style="width: 22%; height: 23px;" align="left" class="cell_caption">
                                                                Batch Total :</td>
                                                            <td style="width: 85%; height: 23px" align="left">
                                                                <input style="width: 66%;" id="txtBatchTotalAmount" type="text">
                                                                <input style="width: 22%;" id="txtBatchTotalItems" type="text"></td>
                                                        </tr>
                                                        <tr style="font-size: 13pt;" >
                                                            <td style="width: 19%; height: 23px" class="cell_caption">
                                                                Total Amount :</td>
                                                            <td style="width: 29%; height: 23px;" align="left"
                                                                                                    >
                                                                <input style="width: 89%;" id="txtTotalAmount" type="text"></td>
                                                            <td style="width: 22%; height: 23px;" class="cell_caption" align="left"
                                                                                                    >
                                                                Computed :</td>
                                                            <td style="width: 85%; height: 23px;" align="left">
                                                                <input style="width: 66%" id="txtComputedTotalAmount" type="text">
                                                                <input style="width: 22%;" id="txtComputedTotalItems" type="text"></td>
                                                        </tr>
                                                        <tr style="font-size: 13pt;" >
                                                            <td style="width: 19%; height: 23px" class="cell_caption">
                                                                Total Items :</td>
                                                            <td style="width: 29%; height: 23px;" align="left"
                                                                                                    >
                                                                <input style="width: 89%;" id="txtTotalItems" type="text"></td>
                                                            <td style="width: 22%; height: 23px;" class="cell_caption" align="left"
                                                                                                    >
                                                                Difference :</td>
                                                            <td style="width: 85%; height: 23px;" align="left">
                                                                <input style="width: 66%" id="txtDifferenceTotalAmount" type="text">
                                                                <input style="width: 22%;" id="txtDifferenceTotalItems" type="text"></td>
                                                        </tr>          
                                                        <tr style="font-size: 13pt;" >
                                                            <td style="width: 19%; height: 23px" class="cell_caption">
                                                                Transaction Date :</td>
                                                            <td style="width: 29%; height: 23px;" align="left"
                                                                                                    >
                                                                <input style="width: 78%;" id="Text8" type="text">
                                                                <img align="absMiddle" onclick="popUpCalendar(this, document.getElementById('txtBatchDateTo'), 'mm/dd/yyyy')"
                                                                                    src="../images/calendar/imgcalen.png" /></td>
                                                            <td style="width: 22%; height: 23px;" class="cell_caption" align="left"
                                                                                                    >
                                                                Rate Type :</td>
                                                            <td style="width: 85%; height: 23px;" align="left">
                                                                &nbsp;<select id="Select1" style="width: 97px; height: 21px">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                                <input id="Button2" style="width: 41%; height: 20px;" type="button" value="Re-Compute" onclick="javascript:ide.btnSearch_click()" /></td>
                                                        </tr>                                                                                                         
                                                    </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>                                                     
                                    <div id="diviDebit" style="display:none" >
                                        <table cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">
                                            <tr>
                                                <td colspan="3" style="width: 100%; height: 100%">
                                                    <table class="table_border" style="width: 100%; height: 100%">
                                                        <tr>
                                                            <td style="width: 100%; height: 8px" align="right">
                                                                <table cellpadding="0" cellspacing="5" width="100%">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 100%" valign="middle">
                                                                            <div class="table_body" style="margin: 0px; width: 98%;
                                                                                height: 100%">
                                                                                <table id="tblAPASRemRemitter" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width: 102%;">
                                                                                    <tr>
                                                                                        <td align="left" colspan="4" style="height: 44px" >
                                                                                            
                                                                                            <table style="width: 344px">
                                                                                            <tr>
                                                                                            <td class="cell_caption" style="width: 54px;text-align:center ;">&nbsp;Debit</td>
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)"
                                                                                                onmouseout="javascript:ide.handle_mouseover('Credit','hoverout',this)"   
                                                                                                onclick="javascript:ide.handle_mouseover('Credit','click',this)"
                                                                                                    >&nbsp;Credit</td>
                                                                                            <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)"
                                                                                                onmouseout="javascript:ide.handle_mouseover('Summary','hoverout',this)"  
                                                                                                onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                    >&nbsp;Summary</td>
                                                                                            </tr>
                                                                                            </table>
                                                                                            
                                                                                            </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Output Seq. No :</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            <input id="txtOutSeqno" type="text" style="width: 88%" /></td>
                                                                                        <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                            Rem. Bank :</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <select id="ddRemBank" style="width: 157px; height: 21px">
                                                                                                <option selected="selected"></option>
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Refnum Theirs:&nbsp;</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            <input id="txtRefnumTheirs" type="text" style="width: 88%" /></td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                            Prot. Bank:</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <select id="ddProtBank" style="width: 157px; height: 21px">
                                                                                                <option selected="selected"></option>
                                                                                            </select>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Date Sent :&nbsp;</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            <label id="lblAPASRemAddress" class="cell_desc" style="font-weight: normal; font-size: 15px">
                                                                                            <input id="Text1" type="text" style="width: 88%" /></label></td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                            Currency Code: &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <select id="ddDRRemCurr" style="width: 157px; height: 21px">
                                                                                                <option selected="selected"></option>
                                                                                            </select></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Date Received :</td>
                                                                                        <td align="left" style="width: 31%; height: 23px" >
                                                                                            <input id="Text2" type="text" style="width: 88%" /></td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption" >
                                                                                            </td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            </td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 31%; height: 23px">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption" >
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            &nbsp;</td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 24px" >
                                                                                            </td>
                                                                                        <td align="left" style="width: 31%; height: 24px">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 22%; height: 24px" class="cell_caption">
                                                                                            &nbsp;</td>
                                                                                        <td align="left" style="width: 85%; height: 24px">
                                                                                            &nbsp;</td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                            
                                                                            <div id = "diviGridBatchItems" style= "display:block">
                                                                                <table id="tblGridIds" width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure;">
                                                                                    <tr>
                                                                                        <td style="width: 100%; text-align: center; text-indent: 5px; color: Black; height: 19px;" colspan="5"></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td style="height: 2px; text-align: center;"><div id = "CIFIDGRID" style= "display:none">  PLACE GRIDVIEW ID'S</div></td>
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
                                        
                                    <div id="diviCredit" style="display:none ;" >
                                            <table cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">
                                                <tr>
                                                    <td colspan="3" style="width: 98%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 103%;">
                                                                    <table cellpadding="0" cellspacing="5" width="100%">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%" valign="middle"><table id="Table3" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                <tr>
                                                                                    <td align="left" colspan="4" style="height: 23px">
                                                                                        <table style="width: 344px" >
                                                                                            <tr>
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)"
                                                                                                onmouseout="javascript:ide.handle_mouseover('Debit','hoverout',this)"   
                                                                                                onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Debit</td>
                                                                                                <td style="width: 54px;text-align:center ;" class="cell_caption">
                                                                                                    &nbsp;Credit</td>                                                                                                
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Summary','hoverin',this)"
                                                                                                onmouseout="javascript:ide.handle_mouseover('Summary','hoverout',this)"  
                                                                                                onclick="javascript:ide.handle_mouseover('Summary','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Summary</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                
                                                                            </table>
                                                                                <table id="Table1" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                    <tr>
                                                                                        <td class="table_caption" colspan="4" style="color: black; text-indent: 5px; text-align: left">
                                                                                            Computation</td>
                                                                                    </tr>                                                                                    
                                                                                    <tr>
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Rate Used :</td>
                                                                                        <td align="left" style="width: 29%; height: 23px">
                                                                                            <input id="txtGrossPeso" type="text" style="width: 89%" /></td>
                                                                                        <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                            Commission :</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="txtNetPeso" type="text" style="width: 88%" /></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            US$ Eqv. :</td>
                                                                                        <td align="left" style="width: 29%; height: 23px">
                                                                                            <input id="txtRemAmt" type="text" style="width: 89%" /></td>
                                                                                        <td align="left" style="width: 22%; height: 23px" class="cell_caption" >
                                                                                            Service Fee(USD) :</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="txtNetDollar" type="text" style="width: 89%" /></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Gross Peso :</td>
                                                                                        <td align="left" style="width: 29%; height: 23px">
                                                                                            <input id="Text4" type="text" style="width: 89%" /></td>
                                                                                        <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                            Total Charges :</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="txtTotCharges" type="text" style="width: 89%" /></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Op. Cost :</td>
                                                                                        <td align="left" style="width: 29%; height: 23px">
                                                                                            <input id="Text3" type="text" style="width: 89%" /></td>
                                                                                        <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                            Net Peso :</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="Text5" type="text" style="width: 89%" /></td>
                                                                                    </tr>
                                                                                    <tr style="font-size: 13pt">
                                                                                        <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                            Doc. Stamp :</td>
                                                                                        <td align="left" style="width: 29%; height: 23px">
                                                                                            <input id="Text6" type="text" style="width: 89%" /></td>
                                                                                        <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                            Net US$ :</td>
                                                                                        <td align="left" style="width: 85%; height: 23px">
                                                                                            <input id="Text7" type="text" style="width: 89%" /></td>
                                                                                    </tr>                                                                                                                                                                        
                                                                                </table>
                                                                            <table id="Table2" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; height: 102%;"
                                                                                    width="100%">
                                                                                <tr>
                                                                                    <td class="table_caption" colspan="4" style="color: black; text-indent: 5px; text-align: left">
                                                                                        Credit Information</td>
                                                                                </tr>                                                                                     
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Benef Last Name :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtBeneLastName" type="text" style="width: 88%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        Reference No :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtReferenceNo" type="text" style="width: 88%" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Benef First Name. :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtBeneFirstName" type="text" style="width: 88%" /></td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                        External Ref. No. :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtExtRefNo" type="text" style="width: 88%" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Benef Address :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtBenefAddress" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        PNB Branch :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <input id="txtPnbBranch" type="text" style="width: 88%" /></td>
                                                                                </tr>
<%--                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%;" rowspan="2" valign="top">
                                                                                        Rem. Address :</td>
                                                                                    <td align="left" style="width: 29%;" rowspan="2" valign="top">
                                                                                        <label id="Label2" class="cell_desc" style="font-weight: normal; font-size: 15px">
                                                                                            <textarea id="txtRemAdd" style="width: 176px; height: 48px"></textarea></label></td>
                                                                                    <td align="left" style="width: 22%;" class="cell_caption" rowspan="2" valign="top">
                                                                                        Benef. Address :</td>
                                                                                    <td align="left" style="width: 85%;" rowspan="2" valign="top">
                                                                                        <textarea id="txtAddress" style="width: 176px; height: 48px"></textarea></td>
                                                                                </tr>--%>
<%--                                                                                <tr style="font-size: 13pt">
                                                                                </tr>--%>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Implementation :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><select id="ddlImplementation" style="width: 89%; height: 21px" >
                                                                                        <option selected="selected"></option>
                                                                                    </select>
                                                                                    </td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption" nowrap>
                                                                                        Payee Type :&nbsp;</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtPayeeType" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Amount :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px">
                                                                                        <input id="txtAmount" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                        Notice Code :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <input id="txtNoticeCode" type="text" style="width: 89%" /></td>
                                                                                </tr>
<%--                                                                                <tr style="font-size: 13pt">
                                                                                    <td align="right" colspan="4" style="height:5px;text-align:left;" valign="middle" >
                                                                                        </td>
                                                                                </tr>--%>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px" >
                                                                                        Rem Name :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px" ><input id="txtRemName" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" style="width: 22%; height: 23px" class="cell_caption">
                                                                                        Account Currency :
                                                                                    </td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtAcctCurr" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px" >
                                                                                        Rem Address :
                                                                                        </td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><input id="txtRemAddress" type="text" style="width: 89%" /></td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px" >
                                                                                        Account Number :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <input id="txtAcctNumber" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td class="cell_caption" style="width: 19%; height: 23px">
                                                                                        Payee Bank :</td>
                                                                                    <td align="left" style="width: 29%; height: 23px"><select id="ddlPayeeBank" style="width: 91%; height: 21px" >
                                                                                        <option selected="selected"></option>
                                                                                    </select>
                                                                                    </td>
                                                                                    <td align="left" class="cell_caption" style="width: 22%; height: 23px">
                                                                                    Worker Type :</td>
                                                                                    <td align="left" style="width: 85%; height: 23px"><input id="txtWorkerType" type="text" style="width: 89%" /></td>
                                                                                </tr>
                                                                            </table>
                                                                                <%-- start here  --%>
                                                                                &nbsp;
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
                                    <div id="diviSummary" style="display:none;" >
                                            <table cellpadding="0" cellspacing="0" style="background-color: azure" width="100%" height="100%">
                                                <tr>
                                                    <td colspan="3" style="width: 104%; height: 100%">
                                                        <table class="table_border" style="width: 100%; height: 100%">
                                                            <tr>
                                                                <td style="width: 100%; height: 8px">
                                                                    <table cellpadding="0" cellspacing="5" width="100%">
                                                                        <tr>
                                                                            <td style="width: 100%; height: 100%" valign="middle"><table id="Table5" cellpadding="0" cellspacing="8" style="border-right: #336699 1px solid;
                                                                                    border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                                                                    width="100%">
                                                                                <tr>
                                                                                    <td align="left" colspan="4" style="height: 23px">
                                                                                        <table style="width: 344px">
                                                                                            <tr>
                                                                                                
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Debit','hoverin',this)"
                                                                                                onmouseout="javascript:ide.handle_mouseover('Debit','hoverout',this)" 
                                                                                                onclick="javascript:ide.handle_mouseover('Debit','click',this)"
                                                                                                    >
                                                                                                    &nbsp;Debit</td>
                                                                                                <td style="width: 54px;text-align:center ;" class="tab_unselected" 
                                                                                                onmouseover="javascript:ide.handle_mouseover('Credit','hoverin',this)"
                                                                                                onmouseout="javascript:ide.handle_mouseover('Credit','hoverout',this)" 
                                                                                                onclick="javascript:ide.handle_mouseover('Credit','click')"
                                                                                                    >
                                                                                                    &nbsp;Credit</td>
                                                                                                    <td class="cell_caption" style="width: 54px;text-align:center ;">
                                                                                                    &nbsp;Summary</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 6%; height: 23px">
                                                                                        </td>
                                                                                    <td id = "lblApprove1st" align="right" style="width: 12%; height: 23px">
                                                                                        <strong>0</strong></td>
                                                                                    <td align="left" style="width: 7%; height: 23px">
                                                                                        </td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <strong>
                                                                                        Approved Item/s by Approving Officer I</strong></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td style="width: 6%; height: 23px">
                                                                                        </td>
                                                                                    <td id = "lblApprove2nd" align="right" style="width: 12%; height: 23px">
                                                                                        <strong>0</strong></td>
                                                                                    <td align="left" style="width: 7%; height: 23px"  >
                                                                                        </td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <strong>
                                                                                        Approved Item/s by Approving Officer II</strong></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td style="width: 6%; height: 23px">
                                                                                        </td>
                                                                                    <td id = "lblUnactedItems" align="right" style="width: 12%; height: 23px">
                                                                                        <strong>0</strong></td>
                                                                                    <td align="left" style="width: 7%; height: 23px">
                                                                                        </td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <strong>Unacted Item/s (For approval)</strong></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td style="width: 6%; height: 23px">
                                                                                        </td>
                                                                                    <td id = "lblFor2ndApproval" align="right" style="width: 12%; height: 23px">
                                                                                        <strong>0</strong></td>
                                                                                    <td align="left" style="width: 7%; height: 23px">
                                                                                        </td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <strong>Item/s that require second approval</strong></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td style="width: 6%; height: 23px">
                                                                                        </td>
                                                                                    <td id="lblDisapprove" align="right" style="width: 12%; height: 23px">
                                                                                        <strong>0</strong></td>
                                                                                    <td align="left" style="width: 7%; height: 23px">
                                                                                        </td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <strong>Disapproved Item/s</strong></td>
                                                                                </tr>
                                                                                <tr style="font-size: 13pt">
                                                                                    <td style="width: 6%; height: 23px">
                                                                                        </td>
                                                                                    <td id="lblTotalItems" align="right" style="width: 12%; height: 23px">
                                                                                        <strong>0</strong></td>
                                                                                    <td align="left" style="width: 7%; height: 23px">
                                                                                        </td>
                                                                                    <td align="left" style="width: 85%; height: 23px">
                                                                                        <strong>Batch Total Item/s</strong></td>
                                                                                </tr>                                                                                                                                                                                                                                                
                                                                            </table>
                                                                                <%-- start here  --%>
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
                                               
                                    <div id="btnButtons">
                                        <br />
                                        <input id="btnPrevious" style="width: 11%" type="button" value="Previous" onclick="javascript:ide.btnPrevious_click()"/>
                                        <input id="btnNext" style="width: 11%" type="button" value="Next" onclick="javascript:ide.btnNext_click()"/>
                                        <input id="btnUpdate" style="width: 11%" type="button" value="Approve" onclick="javascript:ide.btnUpdate_click()"/>
                                        <input id="btnReject" style="width: 11%" type="button" value="Reject" onclick="javascript:ide.btnReject_click()"/></div>  
                                               
                                                </td>
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
            <asp:HiddenField ID="hfSessionid" runat="server" />
            <asp:HiddenField ID="hfGUID" runat="server" />
            
        </div>
    
    </form>
 </center>
</body>
</html>
