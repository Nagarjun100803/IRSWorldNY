<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Branch.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Branch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch </title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    
    </script>
    
    <script type="text/javascript">
    
     function BranchSearch()
        {
       
            
           var brNB = parseInt(document.getElementById('ddlNegotiatingBranch').value);
           var brBC = parseInt(document.getElementById('ddlBranchCode').value);
           var brSBC = parseInt(document.getElementById('ddlSubBranchCode').value);
           var brAC = parseInt(document.getElementById('ddlAgentCode').value);
           var brSAC = parseInt(document.getElementById('ddlSubAgentCode').value);
           var brLoc = parseInt(document.getElementById('ddlLocation').value);
            var brTZ = parseInt(document.getElementById('ddlTimeZone').value);

            if (brNB == "0" & brBC == "0" & brSBC == 0 & brAC == "0" & brSAC == "0" & brLoc == "0" & brTZ == "0")
            {
                if (!confirm('No criteria selected/entered, list all users?')) return;
            }
            
            ajax_show_status(true);
            document.getElementById('span_SearchBranch').innerText = brNB + ', ' + brBC + ', ' + brSBC + ', ' + brAC + ', ' + brSAC + ', ' + brLoc + ', ' + brTZ;
            Branch.searchBR(brNB, brBC, brSBC, brAC, brSAC, brLoc, brTZ, searchBranch);

        }
    
        function searchBranch(rs)
        {
            ajax_show_status(false);
            if (rs.error != null) 
            {
                show_message('E', 'UserAdmin@search: ' + rs.error);
                return;
            }
            if (rs.value != null)
            {
                db_grid = new datagrid(rs.value, document.getElementById('tblSearchGrid'), 12);
                db_grid.setColumnHeaders('CIF ID, User ID, User Name, Location, Role, Status');
                db_grid.setDataColumns('CIFID, UserID, FullName, BranchName, RoleDesc, FlagDesc');
                db_grid.setLinkColumns('UserID');
                db_grid.setLinkHrefs('link_userid_clicked');
                db_grid.bind();
            }
            
            show_search_result();
        }
    
           
    </script>
</head>
<body>
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="CIFBranch" style="display: block">
            <table cellpadding="0" cellspacing="0" width="30%">
                <tr>
                    <td class="table_head_dark" style="width: 30%; height: 24px">
                        Branch Setup</td>
                    <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                <img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 80%; border-bottom: #336699 3px solid" class="table_body">
                            <tr>
                                <td style="width: 100%; height: 21px">
                                    
                                    <div style="text-align: center">
                                        <table id="tblBranch" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                            width="400" class="table_body">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                    height: 26px; text-align: left">Branch Codes</td>
                                                    
                                            </tr>
                                            <tr>
                                                <td style="height: 190px; width: 388px;">
                                                    <div style="text-align: center; display: none; overflow: auto">
                                                        <table class="table_body">
                                                            <tr>
                                                                <td class="cell_caption" style="width: 133px; height: 23px">Negotiating Branch:&nbsp;</td>
                                                                <td style="width: 100px"><select id="ddlNegotiatingBranch" style="width: 250px; height: 100%"></select></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 133px; height: 23px">Branch Code:&nbsp;</td>
                                                                <td style="width: 100px"><select id="ddlBranchCode" style="width: 250px; height: 100%"></select></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 133px; height: 23px">Sub Branch Code:&nbsp;</td>
                                                                <td style="width: 100px"><select id="ddlSubBranchCode" style="width: 250px; height: 100%"></select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 133px; height: 23px">Agent Code:&nbsp;</td>
                                                                <td style="width: 100px"><select id="ddlAgentCode" style="width: 250px; height: 100%"></select></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 133px; height: 23px">Sub Agent Code:&nbsp;</td>
                                                                <td style="width: 100px"><select id="ddlSubAgentCode" style="width: 250px; height: 100%"></select></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 133px; height: 23px">Location:&nbsp;</td>
                                                                <td style="width: 100px"><select id="ddlLocation" style="width: 250px; height: 100%"></select></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 133px; height: 23px">Time Zone:&nbsp;</td>
                                                                <td style="width: 100px"><select id="ddlTimeZone" style="width: 250px; height: 100%"></select></td>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                        </table>
                                                        <table id="tblbuttons" cellpadding="0" cellspacing="5" width="20%" class="table_body">
                                                            <tr>
                                                                <td style="width: 10%">
                                                                    <input id="btnBranchSearch" class="enable_button" disabled="disabled" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Search" onclick="BranchSearch();"/></td>

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
       
                    <div id="divResult" style="width: 100%; height: 100%; display: none">
                    
                        <table width="100%">
                            <tr>
                                <td valign="top" style="width:16%; vertical-align:top; padding-top: 30px;"> <!-- border-right: solid 1px midnightblue"> -->
                                    <table width="100%"  cellpadding="0" cellspacing="7px">
                                        <tr>
                                            <td id="td5" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;"> <!-- border-right:solid 1px #5697c1"> -->
                                                <a id="A1" href="javascript:show_search();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                                    Search</a>
                                            </td>
                                        </tr> 
                                        <tr>
                                            <td id="td3" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;"> <!-- border-right:solid 1px #5697c1"> -->
                                                <a id="A2" href="javascript:show_user_maint();maint_add_click();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                                    Add Branch</a>
                                            </td>
                                        </tr>                                                                               
                                        <tr>
                                            <td id="Td9" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;"> <!-- border-right:solid 1px #5697c1"> -->
                                                <a id="A5" href="javascript:spawn_errpage('Closed');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Close</a>
                                            </td>
                                        </tr>                                                                                                                                       
                                    </table>
                                </td>                                
                                <td style="width: 82%; padding-top: 30px">
                                    <table width="95%" height="100%" cellspacing="0" cellpadding="0" style="background-color:Azure">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capResult" align="left" class="table_head_dark" style="width:41%; height: 24px; border-collapse:collapse;">
                                                Search Result</td>
                                            <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                                <img align="absbottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 100%">
                                                <table style="width:100%; height: 100%;" class="table_border">
                                                    <tr>
                                                        <td style="width: 80%; height: 8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 90%; height: 100%; vertical-align: top">
                                                            <!-- content starts here -->
                                                             <div style="width:98%; height: 100%; margin: 0" class="table_body" id="div1" >
                                                                <table width="100%" cellpadding="0" cellspacing="5px">
                                                                    <tr>
                                                                        <td style="width: 100%;"valign="top">
                                                                            <table width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699;">
                                                                                <tr>
                                                                                    <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; height: 26px;">
                                                                                        Search :&nbsp;<span id='span_SearchBranch'>search_branch</span>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" style="width: 100%; height: 99px;" class="cell_desc" valign="top">
                                                                                        <table id="tblSearchBranchGrid" width="100%" cellspacing="0" cellpadding="3px" style="border: solid 1px #3366CC" >
                                                                                            <thead id="tblSearchBranch" style="height: 24px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                                                                <tr>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 20px;">Negotiating</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 20px;">Branch</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 20px;">Sub Branch</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 20px;">Agent</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 20px;">Sub Agent</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 20px;">Location</th>
                                                                                                    <th style="border: solid 1px #CCCCFF; height: 20px;">Time Zone</th>
                                                                                                </tr>
                                                                                            </thead>
                                                                                            <tbody id="tblSearchBody">
                                                                                            </tbody>
                                                                                            <tfoot style="background-color:#99CCCC; color:#fffff7; font-weight: bold">
                                                                                                <tr>
                                                                                                    <td colspan="7" style="width: 100%" align="right" valign="middle">
                                                                                                        <table width="200px">
                                                                                                        <tr>
                                                                                                            <td style="width: 5%; height: 23px;" align="right"> 
                                                                                                                <a id="pg_prev" style="text-align: right; cursor: hand; font-weight: bold" href="javascript:dbgrid.pg_previous_clicked();">&lt;</a>
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
       
       
        </center>
    
    </div>
    </form>

</body>
</html>
