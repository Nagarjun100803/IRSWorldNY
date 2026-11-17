<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Messages.aspx.cs" Inherits="IRSWorld.BranchSetup.Messages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Message Entry / Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script src="../js/jspad/jspad.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_Messages.js"type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body id="theBody" onload=page_setup();>
    <center>
    <form id="form1" runat="server">
    <div id="divBody" style="width:98%;display:none; vertical-align:top; margin-top:20px;">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td id="tdBody" align="center" style="width:100%; border-collapse:collapse; vertical-align:middle">
                    <table id="tblBody" style="width:95%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width: 60%; vertical-align:bottom; text-align: left" align="left">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="bottom" style="width:0%;height:24px; border-collapse: collapse"> 
                                            <img src="../images/toe.GIF" /></td>
                                        <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                            Messages</td>
                                        <td align="left" valign="bottom" style="width:60%; height: 24px" class="bg_body">
                                            <img align="absbottom" src="../images/rend.gif" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td id=tdContent colspan="3" style="width: 100%; height:100%; border:solid 3px #336699;" valign=middle>
                                <br />
                                <table id=tblContent style="width: 100%;" class=table_body>
                                    <tr>
                                        <td id=tdsearchbar style="width:100%; border: solid 1px midnightblue"  colspan="3" valign=top >
                                            <table id=tblsearch style="width:98%">
                                                <tr>
                                                    <td colspan=5 style="width: 35%;"  valign=top>
                                                        <table style="width: 80%">
                                                            <tr>
                                                                <td style="width:15%"class=cell_caption align=left valign=bottom>Origin:&nbsp</td>
                                                                <td align=left>
                                                                    <select id=ddorigin style="width:90%" onchange="enable_button(document.getElementById('btnSearch'), true);" ></select>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 55%;" valign=top>
                                                        <table id=tbldtsearch style="width: 100%; border: solid 1px silver;">
                                                            <tr>
                                                                <td style="width: 30%">
                                                                    <select id=dddatetype style="width: 100%" onchange="enable_button(document.getElementById('btnSearch'), true);">
                                                                        <option value=1 selected>Posting Date</option>
                                                                        <option value=2>Date Created</option>
                                                                    </select>
                                                                </td>                                                            
                                                                <td style="width: 5%;" class=cell_caption>From:&nbsp;</td>
                                                                <td style="width: 30%;" align=left>
                                                                    <input type=text  id=txtdtfrom style="width: 70%"  onchange="enable_button(document.getElementById('btnSearch'), true);" />
                                                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtdtfrom, 'mm/dd/yyyy');" />
                                                                </td>
                                                                <td class=cell_caption align=left style="width: 5%;">To:&nbsp;</td>
                                                                <td style="width: 30%;" align=left>
                                                                    <input type=text id=txtdtto style="width: 70%"  onchange="enable_button(document.getElementById('btnSearch'), true);" />
                                                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtdtto, 'mm/dd/yyyy');" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>                                                    
                                                    <td style="width: 10%">
                                                        <input type=button id=btnSearch class=button onmouseover="this.className='button_over';" onmouseout="this.className='button';" value="Search" disabled onclick="do_search();" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr> 
                                    <tr><td style="width:100%;height:10px;" colspan=3><hr /></td></tr>                                
                                    <tr>
                                        <td id=tdmsgindex style="width: 100%" colspan=2>
                                            <table id="tblmsgindex" style="width:100%" cellspacing="0" cellpadding="0"  class=cell_caption>
                                                <thead id="tblmsgindex_hdr" style="height: 22px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                    <tr>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 10%">
                                                            Date Created</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 10%">
                                                            Type</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 10%">
                                                            Target Type</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 15%"> Target</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 35%">
                                                            Subject</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 10%">Posting Date</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 10%">Expire Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tblmsgindex_body">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </tbody>
                                                <tfoot style="background-color:#99CCCC; color:#fffff7; font-weight: bold">
                                                    <tr>
                                                        <td colspan="7" style="width: 100%; height:21px;" align="right" valign="middle">
                                                            <table style="width:200px">
                                                            <tr>
                                                                <td style="width: 5%; height: 23px;" align="right"> 
                                                                    <a id="pg_prev" style="text-align: right; cursor: hand; font-weight: bold" href="javascript:fxgridiew.pg_previous_clicked();">&lt;</a>
                                                                </td>
                                                                <td style="width: 5%; height: 23px;" align="right">
                                                                    <span id="pg_start">0</span>
                                                                </td>
                                                                <td style="width: 5%; height: 23px;" align="center">
                                                                    &nbsp;Of&nbsp;
                                                                </td>
                                                                <td align="left" style="width: 5%; height: 23px;">
                                                                    <span id="pg_end">0</span>
                                                                </td>
                                                                <td align="left" style="width: 5%; height: 23px;">
                                                                    <a id="pg_next" href="javascript:pg_next();" style="cursor:hand; font-weight: bold">&gt;</a>
                                                                </td>
                                                                <td align="left" style="width: 15%; height: 23px;">
                                                                    <select id="pg_select" style="width: 100%"></select>
                                                                </td>
                                                            </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tfoot>                                            
                                            </table>                                        
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 21px">
                                            <input id="btnPopclose" type=button class=button style="display:none"onmouseover="this.className = 'warning_over';" onmouseout="this.className = 'button';" value="Close" onclick="close_frame_dialog();" />
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
    <div id="divPopusers" style="width: 330px; height: 370px; border: solid 1px midnightblue; display: none; z-index: 15; left: 0px; top: 0px;" class="ds_gray">
        <table style="width:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:320px; height: 15px; font: bold 12px Arial; color: Ivory" class="navbar" align="left">&nbsp;&nbsp;Message Targets</td>
                <td style="width:10px" class="navbar">
                    <input type="button" value="x" onclick="close_popusers();" style="width:15px; color: red" class="table_caption" />
                </td>
            </tr>
            <tr>
                <td style="width:100%;" align="left" colspan="2" valign=top>
                    <table style="width:100%">
                        <tr>
                            <td style="width:100%" align=center>
                                <select id=ddtargets style="width: 60%" onchange="ddtargets_change(this);">
                                </select>
                            </td> </tr>
                        <tr>
                            <td style="width:100%">
                                <div id=tdtargets style="width:320px; overflow-x:auto"></div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>    
    <div id="divPopnew" style="width: 90%; border: solid 1px midnightblue; display: none; z-index: 10;" class="ds_gray">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td id=td_popup style="width:98%; height: 24px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;
                    <span id=spPopTitle style="text-align:left;width:100%">New Message</span>
                </td>
                <td style="width:10px; height: 24px;" class="table_head_dark" align="right">
                    <input type="button" value="x" onclick="close_popup();" style="width:15px; color: red" class="table_head_dark" />
                </td>
            </tr>
            <tr>
                <td id=tdpopgrid style="width: 100%;" align="left" colspan="2" valign=top>
                    <table id=tblnewmsg style="width:100%" class=table_body>
                        <tr>
                            <td style="width: 15%" class=cell_caption>To:&nbsp;</td>
                            <td style="width: 45%" colspan=3>
                                <input id=txtmsgto type=text style="width:89%;height:100%" onblur="blur_msgto(this);" />&nbsp;
                                <input id=btnmsgto type=button class=button style="width:25px" value= "»" onclick="show_pop_targets();" />
                            </td>
                            <!--
                            <td style="width: 15%" class=cell_caption>Msg. Type:&nbsp;</td>
                            <td style="width: 25%">
                                <input id=txtMsgType type=text style="width:81%;height:100%" onblur="blur_msgto(this);" />&nbsp;
                                <input id=btnmsgtype type=button class=button style="width:25px" value= "»" onclick="show_pop_targets();" />
                            </td>  
                            -->                      
                        </tr>
                         <tr>
                            <td style="width: 15%" class=cell_caption>Subject:&nbsp;</td>
                            <td style="width: 85%" colspan="3">
                                <input id=txtmsgsubj type=text style="width:98%;height:100%" />&nbsp;
                            </td>
                        </tr> 
                        <tr>
                            <td style="width: 15%" class=cell_caption>Posting Date:&nbsp;</td>
                            <td style="width: 35%;">
                                <input id=txtpostdate type=text style="width:70%;height:100%" onchange="validate_postexdate(this);" />
                                <img id=calpostdate src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtpostdate'), 'mm/dd/yyyy');" />
                            </td> 
                            <td style="width: 15%;" class=cell_caption>
                                Expire Date:&nbsp;</td>
                            <td style="width: 35%;">
                                <input id=txtexpdate type=text style="width:70%;height:100%" onchange="validate_postexdate(this);" />&nbsp;
                                <img id=calexpdate align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtexpdate'), 'mm/dd/yyyy');"
                                    src="../images/calendar/imgcalen.png" />
                            </td>                                    
                            <!--                           
                            <td colspan=3 style="width:100%">
                                <table style="width:100%">
                                    <tr>
                                        <td style="width: 10%;" class=cell_caption>
                                            Posting Date:&nbsp;</td>
                                        <td style="width: 20%;">
                                            <input id=txtpostdate type=text style="width:80%;height:100%" onchange="validate_postexdate(this);" />
                                            <img id=calpostdate src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtpostdate'), 'mm/dd/yyyy');" />
                                        </td> 
                                        <td style="width:4%">&nbsp;</td>
                                        <td style="width: 10%;" class=cell_caption>
                                            Expire Date:&nbsp;</td>
                                        <td style="width: 20%;">
                                            <input id=txtexpdate type=text style="width:80%;height:100%" onchange="validate_postexdate(this);" />&nbsp;
                                            <img id=calexpdate align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txtexpdate'), 'mm/dd/yyyy');"
                                                src="../images/calendar/imgcalen.png" />
                                        </td>                                    
                                    </tr>
                                </table>
                            </td>
                            -->
                        </tr>
                        <tr><td colspan=4 style="width:100%;height:10px"><hr /></td></tr>                       
                        <tr>
                            <td id=tdmsgarea colspan=4 style="width:100%;" align=center>
                                <div id=divmsgarea style="width:725px; height: 230px; overflow:auto; margin:0 auto;"></div>
                            </td>
                        </tr>
                        <tr><td colspan=4 style="width:100%;height:10px"><hr /></td></tr>                       
                        <tr>
                            <td colspan=4 style="width:100%;height:10px" align=center>
                                <input type=button class=button onmouseover="this.className = 'warning_over';" onmouseout="this.className = 'button';" value="Close" onclick="close_popup();" />
                            </td>
                        </tr>                       
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="divPopFind" style="width: 100%;border: solid 1px midnightblue; display: none; z-index: 20;">
        <table  cellpadding="0" cellspacing="0">
            <tr>
                <td id=tdPopFind align=center style="width:100%; vertical-align:middle">
                    <table  cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:320px; height: 24px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Find</td>
                            <td style="width:10px; height: 24px;" class="table_head_dark" align="right">
                                <input type="button" value="x" onclick="find_close_popup();" style="width:15px; color: red" class="table_head_dark" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%;" align="center" colspan="2">
                                <div>
                                    <table cellspacing=5 style="width:100%">
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>                                      
                                        <tr>
                                            <td class=cell_desc style="width:70%; text-align:left; vertical-align: top; padding-left: 5px">
                                                <input id=txtfind type=text style="width:100%" onchange="reset_search(0);" />
                                            </td>
                                        </tr>  
                                        <tr>
                                            <td>
                                                <input type=text id="txt_search_point" style="visibility:hidden; width: 50px; text-align:right" value="0" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:100%;vertical-align:middle" align="center" colspan="2">
                                <div style="width:100%; vertical-align:top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 18%">
                                                <input id="btnFind" class="button" onclick="btn_find_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Find" /></td>
                                            <td style="width: 18%">
                                                <input id="btnReset" class="button" onclick="reset_search(0);btn_find_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Restart" /></td>
                                            <td style="width: 18%">
                                                <input id="btnFindCancel" class="button" onclick="find_close_popup();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Close" /></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
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
    </form>
    </center>
</body>
</html>
