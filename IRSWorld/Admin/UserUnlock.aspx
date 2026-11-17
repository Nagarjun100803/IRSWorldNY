<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserUnlock.aspx.cs" Inherits="IRSWorld.Admin.UserUnlock" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>User Administration - Unlock</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/Admin/UserUnlock.js" type="text/javascript"></script>
    <script type="text/javascript">
	</script>
</head>
<body onload="page_setup();" onunload="close_session();" onresize="fixdim();">
    <form id="form1" runat="server">
    <div id="divContent" style="width:100%; height: 100%; overflow:auto;">
        <table style="width:100%;height:100%" cellpadding="0" cellspacing="5">
            <tr>
                <td id=tdvert align="center" style="width:100%; border-collapse:collapse;vertical-align:middle">
                    <div id="divResult" style="width: 100%; height: 100%; display: none; vertical-align:top">
                        <table style="width:100%">
                            <tr>
                                <td style="width: 84%; padding-top: 30px; vertical-align:top">
                                    <table cellspacing="0" cellpadding="0" style="width:96%; height:100%;">
                                        <tr>
                                            <td style="width:100%; vertical-align:bottom" align=left>
                                                <table style="width:100%" cellpadding="0" cellspacing="0" >
                                                <tr>
                                                    <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;">
                                                    <img src="../images/toe.GIF" /></td>
                                                    <td class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                                        User Status Maintenance</td>
                                                    <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                                        <img align="absBottom" src="../images/rend.gif" /></td> 
                                                </tr>
                                                </table>
                                            </td>                                        
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; height: 100%;">
                                                <table style="width:100%; height: 100%;border: solid 2px #336699;background-color:Azure;">
                                                    <tr>
                                                        <td style="width: 100%; height: 100%; vertical-align: top" align=left>
                                                            <!-- content starts here -->
                                                                <table style="width:100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%;" valign="top">
                                                                            <table  cellpadding="0" cellspacing="5" style="width:100%">
                                                                                <tr>
                                                                                    <td class="table_caption" style="width: 100%; text-indent: 5px; height: 26px;">
                                                                                        <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                                            <tr>
                                                                                                <td style="width: 80%; text-align: left">&nbsp;</td>
                                                                                                <td style="width: 20%; text-align: right;">
                                                                                                    <input id="btn_showsearch" type="button" value="Search" onclick="javascript:btn_showsearch_clicked();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td id="tdSearchGrid" align="left" style="width: 100%; height: 100%;vertical-align:top" class="cell_desc">
                                                                                        <table id="tblSearchGrid" cellspacing="0" cellpadding="3px" style="width: 100%; border: solid 1px #3366CC" >
                                                                                            <thead id="tblSearchHdr" style="height: 22px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                                                                <tr>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">CIF Id</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">User Id</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">User Fullname</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Location</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Role</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Status</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 21px;">Logged On</th>
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody id="tblSearchBody">
                                                                                            
                                                                                            </tbody>
                                                                                            <tfoot style="background-color:#99CCCC; color:#fffff7; font-weight: bold">
                                                                                                <tr>
                                                                                                    <td colspan="7" style="width: 100%; height:21px;" align="right" valign="middle">
                                                                                                        <table style="width:200px">
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
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                             </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
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
                    
                    <div id="divSearch" style="width:64%; display: none;" onkeyup="search_enter_click(e);">
                        <table style="width:100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table style="width:100%"cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;">
                                        <img src="../images/toe.GIF" /></td>
                                        <td id="tdPageTitle" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            User Status Search</td>
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
                                            <td style="width: 100%" align=center>
                                                <!-- content starts here -->
                                                <table style="width:96%;border: solid 1px skyblue" cellpadding="0" cellspacing="5" class="table_body">
                                                    <tr>
                                                        <td class="table_caption">
                                                        Search users 
                                                        </td>                                                            
                                                    </tr>
                                                    <tr>
                                                        <td style="width:100%;height: 100px" align="center" valign="middle">                                                         
                                                            <table style="width:96%" cellspacing="5" cellpadding="0">
                                                                <tr>
                                                                    <td align="right" class="cell_desc">User id:</td>
                                                                    <td style="width:75%; border-collapse: collapse" align="left">
                                                                        <input type="text" id="txt_search" style="width: 100%; height: 21px;" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc">
                                                                        Branch:</td>
                                                                    <td align="left"><select id="ddl_search_location" style="width: 100%"></select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc">Flag:</td>
                                                                    <td align="left"><select id="ddl_search_flags" style="width: 70%"></select></td>
                                                                </tr>  
                                                                <tr style="border-collapse:collapse;">
                                                                    <td align="right" class="cell_desc">Logged on/off:</td>
                                                                    <td align="left">
                                                                        <select id="ddl_search_logon" style="height: 21px; width: 29%; text-align: right;">
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
                                                            <input id="btnSearch" type="button" value="Search" onclick="javascript:search_clicked();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
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
                    <div id="divPopFind" style="width: 100%;border: solid 1px midnightblue; display: none; z-index: 20;">
                        <table  cellpadding="0" cellspacing="0" style="width:100%">
                            <tr>
                                <td id=tdPopFind align=center style="width:100%; vertical-align:middle">
                                    <table id=tblPopFind cellpadding="0" cellspacing="0" style="width:332px">
                                        <tr>
                                            <td style="width:320px; height: 24px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Find (by UserId or Username)</td>
                                            <td style="width:12px; height: 24px;" class="table_head_dark" align="right">
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
                                                            <td class=cell_desc align=center style="width:70%; text-align:left; vertical-align: top;">
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
                                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 50%" align=center>
                                                                <input id="btnFind" class="button" onclick="btn_find_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Find" /></td>
                                                            <td style="width: 50%" align=center>
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
                </td>
            </tr>   
        </table>    
    </div>
    </form>
</body>
</html>
