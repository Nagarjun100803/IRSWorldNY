<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Holidays.aspx.cs" Inherits="IRSWorld.BranchSetup.Holidays" %>

<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IRS Currency Group</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A { margin-right: 12px }
    </style>
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_holidays.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
    <script type="text/jscript">	
    </script>

    <script type="text/jscript">
	    function close_session()
        {
            //alert("closed");
            if (is_closing())
            {
                Holidays.close_session();
            }
            else
            {
                Holidays.drop_irsUsers();
            }
        }
	
    </script>

</head>
<body id="theBody" onload="page_setup();new holiday_dropdown().Load_Branches();set_init_value();" >
    <center>
        <form id="form1" runat="server">
            <div id=divBody style="width: 100%; vertical-align: top; margin-top: 30px;">
                <table style="width:60%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 100%; height: auto; vertical-align: top;">
                                <table id="tblSearch" style="width: 100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td style="width:100%; vertical-align:bottom; text-align:left">
                                            <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom" style="width: 0%; height: 24px; border-collapse: collapse">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="capSearch" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                                        border-collapse: collapse">
                                                        Holidays</td>
                                                    <td align="left" valign="bottom" style="height: 24px; width: 60%;" class="bg_body">
                                                        <img align="absbottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:100%; vertical-align:top;">
                                            <table id="tblMain" style="width: 100%;" class=table_border>
                                                <tr>
                                                    <td style="width: 32%; height: 18px;" align="right" class="cell_desc">
                                                        Branch Code</td>
                                                    <td style="width: 68%; height: 18px;">
                                                        <select id="ddlBranch" style="width: 93%">
                                                            <option selected="selected"></option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 26px;">
                                                    </td>
                                                    <td style="height: 26px;">
                                                        <input type="button" value="Search" onclick="validate_holidays();" class="button"
                                                            onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                        <!-- content starts here -->
                                                        <!-- content ends here -->
                                                    </td>
                                                </tr>
                                            </table>
                                            <table id="tblHolidays" style="width:100%;">
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td id="tdHolidays" style="width: 100%;"  class=table_border>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <!--<label align="right" class="cell_desc" onclick="set_visibility('Main');">Go back to main</label>-->
                                <!--<input type="button" value="Go Back" onclick="set_visibility('Main');" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnBack" />-->
                                <table>
                                    <tr>
                                        <td>
                                            <a id="btnBack" href="#" onclick="set_visibility('Main');" style="font-weight: bold;
                                                font-size: 12px; font-style: italic; font-family: Arial;">Go back to search</a>
                                        </td>
                                    </tr>
                                </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 18px;" align="right">
                        </td>
                    </tr>
                </table>
            </div>
        </form>
        <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx"
            target="_parent">
        </form>
        <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BranchSetup/Holidays.aspx"
            target="_parent">
        </form>
    </center>
</body>
</html>
