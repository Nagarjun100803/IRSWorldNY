<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditLogViewer.aspx.cs" Inherits="IRSWorld.Admin.AuditLogViewer" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRS User Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A { margin-right: 12px }
    </style>

    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/Admin/AuditLogViewer.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript">
	</script>
	<script type="text/jscript">
	    function close_session()
        {
            //alert("closed");
            if (is_closing())
            {
                AuditLogViewer.close_session();
            }
            else
            {
                AuditLogViewer.drop_irsUsers();
            }
        }
	
	</script>
	
	
</head>
<body onload="new auditlog_viewer().Load_ServerRole();new auditlog_viewer().load_branches();" onunload="close_session();">
    <center>
    <form id="form1" runat="server">
    <div style="width:100%; height: 100%">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="5">
            <tr>
                <td align="center" valign="middle" style="width:100%; border-collapse:collapse;" class="content_height">
                    <div id="divSearch" style="width:60%; display: block;">
                        <table id="tblSearch" style="width:100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                    <img src="../images/toe.GIF" /></td>
                                <td id="capSearch" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                    User Profile Search</td>
                                <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                    <table style="width:100%;" class="table_border">
                                        <tr>
                                            <td style="width: 100%">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%">
                                                <!-- content starts here -->
                                                <table cellpadding="0" cellspacing="5" class="table_body" style="width:100%; border: solid 1px skyblue">
                                                    <tr>
                                                        <td class="table_caption">
                                                        Search users by category
                                                        </td>                                                            
                                                    </tr>
                                                    <tr>
                                                        <td style="width:100%;height: 100px" align="center" valign="middle">
                                                            <table style="width:96%" cellspacing="5" cellpadding="0">
                                                                <tr>
                                                                    <td align="right" style="width: 25%">
                                                                        <select id="ddl_search" style="height: 21px; width: 100%; text-align: right;" onclick="show_search_calendar();">
                                                                            <option value="1" SELECTED>User id</option>
                                                                            <option value="2">User Name</option>
                                                                            <option value="3">Modified Date</option>
                                                                           
                                                                        </select>
                                                                    </td>                                                                
                                                                    <td style="width:75%; border-collapse: collapse" align="left">
                                                                        <input type="text" id="txt_search" style="width: 93%; height: 21px;" />
                                                                        <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.getElementById('txt_search'), 'mm/dd/yyyy');" style="visibility:hidden" id="img_search_calendar" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc">Location:</td>
                                                                    <td align="left"><select id="ddl_loc"></select></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" class="cell_desc" style="height: 21px">Role:</td>
                                                                    <td align="left" style="height: 21px"><select id="ddlRole"></select></td>
                                                                </tr> 
                                                                <tr>
                                                                    <td align="right" class="cell_desc">
                                                                        Event Type:</td>
                                                                    <td align="left"><select id="ddl_search_event">
                                                                    <option value="" SELECTED></option>
                                                                    <option value="INSERT">INSERT</option>
                                                                    <option value="UPDATE">UPDATE</option>
                                                                    <option value="DELETE">DELETED</option>
                                                                    </select></td>
                                                                </tr>  
                                                                <tr>
                                                                    <td align="right" class="cell_desc">Logged on/off:</td>
                                                                    <td align="left">
                                                                        <select id="ddl_search_logon" style="height: 21px; width: 29%; text-align: right;" onclick="show_search_calendar();">
                                                                            <option value="All" SELECTED>All</option>
                                                                            <option value="1">Logged on</option>
                                                                            <option value="0">Logged off</option>
                                                                        </select>                                                                    
                                                                        </td>
                                                                </tr>   
                                                                 
                                                                </table>
                                                        </td>
                                                    </tr>
                                                    <tr><td style="height: 10px" class="comment">&nbsp;</td></tr>
                                                    <tr>
                                                        <td style="height: 28px" valign="bottom">
                                                            <input type="button" value="Search" onclick="on_search_click();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                        </td>
                                                    </tr>
                                                    <tr><td style="height: 10px" class="comment">&nbsp;</td></tr>
                                                </table>
                                                <!-- content ends here -->

                      
                                            </td>
                 
                                        </tr>
                                        <tr>

                                        </tr>                                    
                                    </table>
                                
                                </td>
                            </tr>

                        </table>
                    </div>
                                
                   
                      
                    <table id="tblAudit" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                    display: NONE; border-left: #336699 3px solid; width: 90%; border-bottom: #336699 3px solid">
                    <tr>
                     </tr>
                     <tr>
                     <td id="tdAudit" style="width: 100%;">
                                                        
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
