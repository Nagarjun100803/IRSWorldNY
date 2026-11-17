<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAdmin2.aspx.cs" Inherits="IRSWorld.Admin.UserAdmin2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRS User Maintenance</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../js/Admin/treetab.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A { margin-right: 12px }
    </style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/Admin/user_record2.js" type="text/javascript"></script>
    <script src="../js/Admin/treetab.js" type="text/javascript"></script>
    <script src="../js/Admin/UserAdmin2.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript">
	</script>
</head>
<body onload="page_setup();" onunload="close_session();" onresize="fixdim();">
    <center>
    <form id="form1" runat="server">
    <div id="divContent" style="width:100%; height: 100%; overflow:auto">
        <table width="100%" height="100%" cellpadding="0" cellspacing="5">
            <tr>
                <td align="center" valign="middle" style="width:100%; border-collapse:collapse;" class="content_height">
                    <div id="divSearch" style="width:60%; display: none;" onkeyup="search_enter_click(e);">
                        <table width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                        <td id="tdPageTitle" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            User Profile Search</td>
                                        <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                            
                            </tr>
                            <tr>
                                <td style="width: 100%; background: azure">
                                    <table style="width:100%;" class="table_border">
                                        <tr>
                                            <td style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                <!-- content starts here -->
                                                <table width="100%" cellpadding="0" cellspacing="5" class="table_body" style="border: solid 1px skyblue">
                                                    <tr>
                                                        <td class="table_caption">
                                                        Search users by category
                                                        </td>                                                            
                                                    </tr>
                                                    <tr>
                                                        <td style="width:100%;height: 100px" align="center" valign="middle">
                                                            <table width="96%" cellspacing="5" cellpadding="0">
                                                                <tr>
                                                                    <td align="right" style="width: 25%">
                                                                        <select id="ddl_search_cat" style="height: 21px; width: 100%; text-align: right;" onclick="show_search_calendar();">
                                                                            <option value="1" SELECTED>User id</option>
                                                                            <option value="2">User Name</option>
                                                                            <option value="3">Date Created</option>
                                                                            <option value="4">Date Expired</option>
                                                                        </select>
                                                                    </td>                                                                
                                                                    <td style="width:75%; border-collapse: collapse" align="left">
                                                                        <input type="text" id="txt_search" style="width: 93%; height: 21px;" />
                                                                        <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txt_search'), 'mm/dd/yyyy');" style="visibility:hidden" id="img_search_calendar" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc">Location:</td>
                                                                    <td align="left"><select id="ddl_search_location"></select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="height: 21px">Role:</td>
                                                                    <td align="left" style="height: 21px"><select id="ddl_search_roles"></select></td>
                                                                </tr> 
                                                                <tr>
                                                                    <td align="right" class="cell_desc">Flag:</td>
                                                                    <td align="left"><select id="ddl_search_flags"></select></td>
                                                                </tr>  
                                                                <tr style="display: none; border-collapse:collapse;">
                                                                    <td align="right" class="cell_desc">Logged on/off:</td>
                                                                    <td align="left">
                                                                        <select id="ddl_search_logon" style="height: 21px; width: 29%; text-align: right;" onclick="show_search_calendar();">
                                                                            <option value="0" SELECTED>All</option>
                                                                            <option value="1">Logged on</option>
                                                                            <option value="2">Logged off</option>
                                                                        </select>                                                                    
                                                                        </td>
                                                                </tr>   
                                                                 
                                                                </table>
                                                        </td>
                                                    </tr>
                                                    <tr><td style="height: 10px" class="comment">&nbsp;</td></tr>
                                                    <tr>
                                                        <td style="height: 28px; vertical-align: bottom" valign="bottom" align=center>
                                                            <input id="btn_search" type="button" value="Search" onclick="javascript:search_clicked();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                        </td>
                                                    </tr>
                                                    <tr><td style="height: 10px" class="comment">&nbsp;</td></tr>
                                                </table>
                                                <!-- content ends here -->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>                                    
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                    <div id="divResult" style="width: 100%; height: 100%; display: none">
                    
                        <table width="100%">
                            <tr>
                                <td valign="top" style="width:16%; vertical-align:top; padding-top: 30px;">
                                     <div id=sideserch style="width:100%">
                                        <script type="text/javascript">
                                            var sideA = new Array
                                            (
                                                new Array(1, 0, 'Search', 'javascript:show_search();'),
                                                new Array(2, 0, 'Add User', 'javascript:show_user_maint();maint_add_click();'),
                                                new Array(3, 0, 'Close', 'javascript:if (confirm("Logoff session?")) { spawn_errpage("Closed");}')
                                            );
                                            
                                            var sidemenuA = new js_sidebar('sidemenuA', sideA);
                                            document.write(sidemenuA.toString());
                                        </script>
                                     </div>
                                </td>                                
                                <td style="width: 84%; padding-top: 30px">
                                    <table width="96%" height="100%" cellspacing="0" cellpadding="0" style="background-color:Azure">
                                        <tr>
                                            <td style="width:100%; vertical-align:bottom" align=left>
                                                <table cellpadding="0" cellspacing="0" >
                                                <tr>
                                                    <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                                    <td class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                                        Search Result</td>
                                                    <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                                        <img align="absBottom" src="../images/rend.gif" /></td> 
                                                </tr>
                                                </table>
                                            </td>                                        
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; height: 100%">
                                                <table style="width:100%; height: 100%;" class="table_border">
                                                    <tr>
                                                        <td style="width: 100%; height: 8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 100%; vertical-align: top" align=left>
                                                            <!-- content starts here -->
                                                             <div style="width:98%; height: 100%;" class="table_body" id="div1" >
                                                                <table width="100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%;"valign="top">
                                                                            <table width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699;">
                                                                                <tr>
                                                                                    <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; height: 26px;">
                                                                                        Search :&nbsp;<span id='span_srch_txt'>search_string</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td id=tdSearchParent align="left" style="width: 100%; height: 100%;vertical-align:top;" class="cell_desc" valign="top">
                                                                                        <div id='tdSearchGrid' style="width:750px;overflow-x:auto">
                                                                                        <table id="tblSearchGrid" width="100%" cellspacing="0" cellpadding="3px" style="border: solid 1px #3366CC" >
                                                                                            <thead id="tblSearchHdr" style="height: 22px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                                                                <tr>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">CIF Id</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">User Id</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">User Fullname</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Location</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Role</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Status</th>
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody id="tblSearchBody">
                                                                                            
                                                                                            </tbody>
                                                                                            <tfoot style="background-color:#99CCCC; color:#fffff7; font-weight: bold">
                                                                                                <tr>
                                                                                                    <td colspan="6" style="width: 100%; height:21px;" align="right" valign="middle">
                                                                                                        <table width="200px">
                                                                                                        <tr>
                                                                                                            <td style="width: 5%; height: 23px;" align="right"> 
                                                                                                                <a id="pg_prev" style="text-align: right; cursor: hand; font-weight: bold" href="javascript:gview.pg_previous_clicked();">&lt;</a>
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
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                             </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </div>  
                                                            <!-- content ends here -->
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
                    
                    <div id="divMaint" style="width: 100%; height: 100%; display: none">
                        <table width="100%">
                            <tr>
                                <td valign="top" style="width:16%; height:100%; vertical-align:top; ">
                                    <div style="width:100%">
                                        <script type="text/javascript">

                                            var sideB = new Array
                                            (
                                                new Array(1, 0, 'Search', 'javascript:show_search();'),
                                                new Array(2, 0, 'Search Result', 'javascript:show_search_result();'),
                                                new Array(3, 0, 'Modules Profile', 'javascript:show_maint_modules();'),
                                                new Array(4, 0, 'Reset Password', 'javascript:reset_password();'),
                                                new Array(5, 0, 'CIF Details', 'javascript:hide_combos(true); show_frame_dialog("../CIF/cifmain.aspx?function=search&cifid=" + getText(document.getElementById("txtCifId")), "cif_detail_close", "100%", "100%", "0");'),
                                                new Array(6, 0, 'Close', 'javascript:if (confirm("Logoff session?")) { spawn_errpage("Closed");}')
                                            );

                                            var sidemenuB = new js_sidebar('sidemenuB', sideB);
                                            sidemenuB.use_href = true;
                                            document.write(sidemenuB.toString());
                                        </script>                                
                                    </div>                                
                                </td>                                
                                <td style="width: 84%; padding-top: 10px; vertical-align:top" align=center>
                                    <table width="96%" height="100%" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td style="width:100%; vertical-align:bottom" align=left>
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr>
                                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                                        <td class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                                            <span id="Span2">User Profile Maintenance</span>
                                                        </td>
                                                        <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                                    </tr>
                                                </table>
                                            </td>                                      
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; height: 100%; background-color: Azure;">
                                                <table style="width:100%; height: 100%;" class="table_border">
                                                    <tr>
                                                        <td style="width: 100%; height: 8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 100%; height: 100%; vertical-align: top">
                                                            <!-- content starts here -->
                                                             <div style="width:99%; height: 100%;" class="table_body" id="divUserProfile" >
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td style="width: 100%; vertical-align: top; height: 363px;" align=center>
                                                                            <table id="tblUserProfile" cellpadding="0" cellspacing="5" style="width: 98%; border: solid 1px #336699;">
                                                                                <tr>
                                                                                    <td class="table_caption" style="width: 133px; text-align: left; text-indent: 5px;" colspan="3">User Profile
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width:100%" align=left>
                                                                                        <table id="tdUserProfile" style="width:100%" cellpadding=0 cellspacing=5px>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 133px; height: 24px">CIF Id:&nbsp;</td>
                                                                                                <td align="left" style="width: 230px; height: 24px;" class="cell_desc">
                                                                                                    <span id="txtCifId" style="width: 72%; text-align: right;"></span>
                                                                                                </td>
                                                                                            </tr>                                                                                
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 133px; height: 23px">User Id:&nbsp;</td>
                                                                                                <td align="left" style="width: 230px">
                                                                                                    <input id="txtUserId" type="text" style="width: 99%; height: 100%" onkeydown="enable_button(document.getElementById('btnSave'), true);" onchange="txtfield_changed(this);" onfocusout="focus_out(this);" tabindex="1" />
                                                                                                </td>
                                                                                                <td align="left" style="text-indent: 5px"  class="cell_desc">
                                                                                                    <span id="lblFullname" style="width: 100%;"></span>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 133px; height: 24px">
                                                                                                    Days Expired:&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 230px; height: 24px;">
                                                                                                    <input id="txtDaysExpired" type="text" style="width: 31%; height: 100%; text-align: right;" onkeydown="enable_button(document.getElementById('btnSave'), true);" onchange="txtfield_changed(this); " onfocusout="focus_out(this);" tabindex="3" /></td>
                                                                                                <td align="left" style="text-indent: 5px; height: 24px;">
                                                                                                    <span id="spanDateExpired" style="width: 100%;" class="cell_desc"></span>
                                                                                                </td>
                                                                                            </tr>  
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                                    Flags:&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 230px">
                                                                                                    <select id="ddFlags" style="width: 77%;" onchange="txtfield_changed(this); " onfocusout="focus_out(this);" tabindex="5"></select>
                                                                                                </td>
                                                                                            </tr>                                                                    
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 133px; height: 23px">Date Created:&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 230px" class="cell_desc">
                                                                                                    <span id="spanDateCreated" style="width: 49%"></span>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 133px; height: 23px">Date Last Modified:&nbsp;
                                                                                                </td>
                                                                                                <td align="left" style="width: 230px"  class="cell_desc">
                                                                                                    <span id="spanDateModified" style="width: 49%"></span>
                                                                                            </tr>
                                                                                            <tr>                                                                               
                                                                                                <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                                    Last Access Date:&nbsp;</td>
                                                                                                <td align="left" style="width: 230px"  class="cell_desc">
                                                                                                    <span id="spanLastAccessDate" style="width: 49%"></span>
                                                                                            </tr>                                                                                
                                                                                            <tr>
                                                                                                <td class="cell_caption" style="width: 133px; height: 23px">
                                                                                                    Password Changed:&nbsp;
                                                                                                </td>
                                                                                                <td align="left" class="cell_desc" style="width: 230px">
                                                                                                    <span id="spanDatePwdChange" style="width: 49%"></span>
                                                                                                </td>
                                                                                                <td align=cemter>
                                                                                                    <input id="btnEdit" class="button" onclick="maint_edit_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" />
                                                                                                </td>
                                                                                            </tr>                                                                                        
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td id=tdmaint_locrole colspan=3 class=cell_desc style="width:100%">
                                                                                        <table id="tblLocRole" cellspacing="0" cellpadding="3px" style="width:100%; border: solid 1px #3366CC" >
                                                                                            <thead id="ttblLocRole_hdr" style="height: 22px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                                                                <tr>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Location</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Role</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Primary</th>
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody id="tblLocRole_tbody">
                                                                                            
                                                                                            </tbody>
                                                                                            <tfoot style="background-color:#99CCCC; color:#fffff7; font-weight: bold">
                                                                                                <tr>
                                                                                                    <td colspan="5" style="width: 100%; height:21px;" align="right" valign="middle">
                                                                                                        <table width="200px">
                                                                                                        <tr>
                                                                                                            <td style="width: 5%; height: 23px;" align="right"> 
                                                                                                                <a id="A1" style="text-align: right; cursor: hand; font-weight: bold" href="javascript:gview.pg_previous_clicked();">&lt;</a>
                                                                                                            </td>
                                                                                                            <td style="width: 5%; height: 23px;" align="right">
                                                                                                                <span id="Span1">0</span>
                                                                                                            </td>
                                                                                                            <td style="width: 5%; height: 23px;" align="center">
                                                                                                                &nbsp;Of&nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 5%; height: 23px;">
                                                                                                                <span id="Span3">0</span>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 5%; height: 23px;">
                                                                                                                <a id="A2" href="javascript:pg_next();" style="cursor:hand; font-weight: bold">&gt;</a>
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 15%; height: 23px;">
                                                                                                                <select id="Select1" style="width: 100%"></select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </tfoot>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                                                                                                        
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" colspan="3" style="width: 100%;">
                                                                            <div id="_divButtons" style="width: 60%; background: transparent">
                                                                                <table width="100%" cellpadding="0" cellspacing="5px">
                                                                                    <tr>
                                                                                        <td style="width: 18%">
                                                                                            <input id="btnAdd" class="button"  onclick="maint_add_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="New User" /></td>
                                                                                        <td style="width: 18%">
                                                                                            <input id="btnDelete" class="button" onclick="maint_del_click();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Delete" /></td>
                                                                                        <td style="width: 18%">
                                                                                            <input id="btnSave" class="button" onclick="maint_save_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" /></td>
                                                                                        <td style="width: 18%">
                                                                                            <input id="btnCancel" class="button" onclick="maint_cancel_click();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>                                                                    
                                                                </table>
                                                            </div>  
                                                            <div style="width:100%; height: 100%; display: none;"  class="table_body" id="divModules" >
                                                                <table width="100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%;" valign="top">
                                                                            <table width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699;">
                                                                                <tr>
                                                                                    <td class="table_caption" style="width: 140px; text-align: left; text-indent: 5px; height: 26px;" colspan="3">
                                                                                        Modules Permission&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 20%; height: 20px">User Id:&nbsp;</td>
                                                                                    <td align="left" style="width: 30%">
                                                                                        <span id="spanModUserid" class="cell_desc" style="width: 99%">userid</span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 50%; text-indent: 5px"  class="cell_desc">
                                                                                        <span id="spanModFullName" style="width: 100%;"></span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 20%; height: 20px">Location:&nbsp;</td>
                                                                                    <td align="left" style="width: 30%">
                                                                                        <span id="spanModLoc" class="cell_desc" style="width: 99%"></span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 50%; text-indent: 5px"  class="cell_desc">
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                                <tr>
                                                                                    <td class="cell_caption" style="width: 20%; height: 20px">Role:&nbsp;</td>
                                                                                    <td align="left" style="width: 30%">
                                                                                        <span id="spanModRole" class="cell_desc" style="width: 99%"></span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 50%; text-indent: 5px"  class="cell_desc">
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                                <tr><td style="height: 10px"></td></tr>
                                                                                <tr>
                                                                                    <td colspan="3" style="width:100%" class="cell_desc" >
                                                                                        <div id="divModProfile" style="width:100%; height: 240px; overflow: auto;" class="table_body">
                                                                                            <table id="tblModProfile" style="width: 100%;" cellpadding="0" cellspacing="0">
                                                                                                <caption class="cell_caption" style="text-align: center; vertical-align: middle; height: 20px; width: 100%">Module Profile</caption>
                                                                                                <thead  style="height: 22px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                                                                    <tr>
                                                                                                        <th style="border: solid .1pt #CCCCFF; height: 22px;">Group</th>
                                                                                                        <th style="border: solid .1pt #CCCCFF; height: 22px;">Subgroup</th>
                                                                                                        <th style="border: solid .1pt #CCCCFF; height: 22px;">Module</th>
                                                                                                        <th style="border: solid .1pt #CCCCFF; height: 22px;">Access</th>
                                                                                                    </tr>
                                                                                                </thead>
                                                                                                <tbody id="tblModProfile_body">
                                                                                                    <tr>    
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            System Administration
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">&nbsp;</td>
                                                                                                        <td style="width: 25%; text-align: left">&nbsp;</td>
                                                                                                        <td style="width: 25%; text-align: left"><select style="width:100"></select></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            &nbsp;
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">User Maintenance</td>
                                                                                                        <td style="width: 25%; text-align: left"></td>
                                                                                                        <td style="width: 25%; text-align: left"><select style="width:100"></select></td>
                                                                                                    </tr>                                                                                                
                                                                                                    <tr>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            Menu Maintenance</td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            </td>
                                                                                                        <td style="width: 25%; text-align: left"><select style="width:100">
                                                                                                        </select>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            Branch Administration</td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left"><select style="width:100">
                                                                                                        </select>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            Branch Defaults</td>
                                                                                                        <td style="width: 25%; text-align: left"><select style="width:100">
                                                                                                        </select>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            Branch Setup</td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            <select style="width:100">
                                                                                                            </select>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            Charges</td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            <select style="width:100">
                                                                                                            </select>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            Logout</td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                        </td>
                                                                                                        <td style="width: 25%; text-align: left">
                                                                                                            <select style="width:100">
                                                                                                            </select>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </tbody>
                                                                                            </table>
                                                                                        </div>
                                                                                        <div class="table_body" style="width: 100%; text-align: right; margin-right:25px; border-top:solid 1px midnightblue">
                                                                                            <a id="aModDisAll" href="javascript:maint_mod_disall(false);">Disable all</a>&nbsp;|&nbsp;&nbsp;
                                                                                            <a id="aModAllowAll" href="javascript:maint_mod_disall(true);">Allow all</a>
                                                                                        </div>
                                                                                    </td>                                                                                    
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="3" align=center style="width: 100%">
                                                                                        <div id="Div2" style="width: 40%; background: transparent">
                                                                                            <table width="100%" cellpadding="0" cellspacing="5px">
                                                                                                <tr>
                                                                                                    <td style="width: 18%">
                                                                                                        <input id="btnModEdit" class="button" onclick="mod_edit_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" /></td>
                                                                                                    <td style="width: 18%">
                                                                                                        <input id="btnModSave" class="button" onclick="mod_save_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" /></td>
                                                                                                    <td style="width: 18%">
                                                                                                        <input id="btnModCancel" class="button" onclick="mod_cancel_click();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
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
                                                            <!-- content ends here -->
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
                </td>
            </tr>   
        </table>    
    </div>
    <div id="divPopup" style="width: 400px;border: solid 1px midnightblue; display: none; z-index: 10;">
        <table  cellpadding="0" cellspacing="0">
            <tr>
                <td id=td_popup align=center style="width:100%; vertical-align:middle">
                    <div>
                    <table  cellpadding="0" cellspacing="0">
                        <tr>
                            <td id=pop_heading style="width:400px; height: 24px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Password Mailing</td>
                        </tr>
                        <tr>
                            <td class="cell_desc" style="width: 100%; height: 25px; text-align: left; text-indent: 5px">
                                A Password was generated for:
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; text-align: left;">
                                <table cellspacing=5 style="width:100%">
                                    <tr>
                                        <td class=cell_caption style="width: 30%">User ID:&nbsp;
                                        </td>
                                        <td id=popUserId class=cell_desc style="width:70%; text-align:left"></td>
                                    </tr>
                                    <tr>
                                        <td class=cell_caption style="width: 30%">User Name:&nbsp;
                                        </td>
                                        <td id=popUserName class=cell_desc style="width:70%; text-align:left"></td>
                                    </tr> 
                                    <tr>
                                        <td class=cell_caption style="width: 30%">CIF ID:&nbsp;
                                        </td>
                                        <td id=popCifId class=cell_desc style="width:70%; text-align:left"></td>
                                    </tr> 
                                    <tr>
                                        <td colspan=2 style="width:100%;" align=left>
                                            <table id=tblemail style="width:100%; display:block" cellspacing=0>
                                                <tr>
                                                    <td class=cell_desc style="width: 100%" colspan=2>The password will be sent through email:
                                                    </td>
                                                <tr>
                                                <tr>
                                                    <td class=cell_caption style="width: 30%">Email Address:&nbsp;
                                                    </td>
                                                    <td id=popTdEmail class=cell_desc style="width:70%; text-align:left; vertical-align: top; padding-left: 5px">
                                                        <select style="width:100%" id=popDdEmail></select>
                                                    </td>
                                                </tr>                                                   
                                            </table>
                                            <table id=tblnoemail style="width:100%; display:none" cellspacing=0>
                                                <tr>
                                                    <td class=cell_desc style="text-align:left; width:400px">
                                                        No registered email address. You may enter an email address below or view the password and personally disclose it to the client.
                                                    </td>
                                                </tr> 
                                                <tr>
                                                    <td class=cell_desc style="text-align:left; width:100%">
                                                        <input type=text id=popTxtEmail style="width:99%; text-align:left" onchange="pop_email_changed(this);" />
                                                    </td>
                                                </tr>                                                   
                                            </table>
                                            <table id=tblviewpwd style="width:100%; display:none" cellspacing=0>
                                                <tr>
                                                    <td class=cell_desc style="text-align:left; width:100%">The password is:
                                                    </td>
                                                </tr>                                                 
                                                <tr>
                                                    <td id=tdviewpwd class=cell_desc align=center style="text-align:center; width:100%; font-size:larger; font-weight:bold">Password
                                                    </td>
                                                </tr>                                           
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr><td style="width:100%">&nbsp;</td></tr>
                        <tr>
                            <td style="width:100%;vertical-align:middle" align="center">
                                <div style="width:100%; vertical-align:top" align=center>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 18%">
                                                <input id="btnSend" class="button" onclick="btn_send_click(this);" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Send" /></td>
                                            <td style="width: 18%">
                                                <input id="btnView" class="button" onclick="btn_view_click(this);" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="View" /></td>                                            
                                            <td style="width: 18%">
                                                <input id="btnClose" class="button" disabled onclick="close_popup();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Close" /></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
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
    <div id="divPoppwd" style="width: 330px;border: solid 1px midnightblue; display: none; z-index: 20;">
        <table  cellpadding="0" cellspacing="0">
            <tr>
                <td id=tdPoppwd align=center style="width:100%; vertical-align:middle">
                    <table  cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:320px; height: 15px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Enter Your Login Password</td>
                            <td style="width:10px" class="table_head_dark" align="right">
                                <input type="button" value="x" onclick="pwd_close_popup();" style="width:15px; color: red" class="table_head_dark" />
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
                                                <input id=pwdDiscloser type=password style="width:100%" />
                                            </td>
                                        </tr>  
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>                                      
                                    </table>
                                </div>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:100%;vertical-align:middle" align="center" colspan="2">
                                <div style="width:100%; vertical-align:top" align=center>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 18%">
                                                <input id="btnPwdOk" class="button" onclick="btn_pwdok_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="OK" /></td>
                                            <td style="width: 18%">
                                                <input id="btnPwdCancel" class="button" onclick="pwd_close_popup();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
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
    <div id="divPopFind" style="width: 100%;border: solid 1px midnightblue; display: none; z-index: 20;">
        <table  cellpadding="0" cellspacing="0">
            <tr>
                <td id=tdPopFind align=center style="width:100%; vertical-align:middle">
                    <table id=tblPopFind cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:320px; height: 24px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Find (by UserId or Username)</td>
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
                                                <input id=txtfind type=text style="width:100%" />
                                            </td>
                                        </tr>  
                                        <tr>
                                            <td>&nbsp;</td>
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
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_parent"></form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../Admin/UserAdmin.aspx" target="_parent"></form>
    </center>
</body>
</html>

