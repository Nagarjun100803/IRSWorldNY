<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MessageGroup.aspx.cs" Inherits="IRSWorld.BranchSetup.MessageGroup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Message Groups Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_MessageGroup.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body id="theBody" onload="page_setup();">
    <center>
    <form id="form1" runat="server">
    <div id="divBody" style="width:96%;display:none; vertical-align:top; margin-top:30px;">
        <table style="width:100%;height:100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td id="tdBody" align="center" style="width:100%; border-collapse:collapse; vertical-align:middle">
                    <table style="width: 95%" cellspacing="0" cellpadding="0">
                        <tr>
                                <td style="width:60%; vertical-align:bottom; text-align: left" align="left">
                                    <table style="width:100%"cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:0px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                        <td id="tdPageTitle" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            Message Groups</td>
                                        <td align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                        
                        </tr>
                        <tr>
                            <td id=tdContent style="width: 100%;border:solid 3px #336699;" class=table_body align=center> <!--height:435px; -->
                                <br />
                                <table style="width: 98%">
                                    <tr>
                                        <td style="width: 100%" align=center>
                                            <table style="width:70%">
                                                <tr>
                                                    <td class=cell_caption style="width: 20%">Branch:&nbsp;</td>
                                                    <td style="width: 80%">
                                                        <select id=ddbranches style="width:100%" onchange="ddbraches_change(this);"></select>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr><td style="width:100%;height:10px"></td></tr>
                                    <tr>
                                        <td id=tdgroupgrid style="width: 100%" align=center>
                                            <table id=tblgroupgrid style="width:100%" class=cell_desc>
                                                <thead id="tbgrid_out_hdr" style="height: 22px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                    <tr>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px;">Group Name</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px;">Members</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 30%;background-color:skyblue;color:navy;" valign=top align=left>Tropang Labas ang litaw</td>
                                                        <td style="width: 70%;background-color:Azure;color:navy;" valign=top >
                                                            <table id=Table1 style="width:100%" class=cell_desc>
                                                                <thead id="Thead1" style="height: 22px;background-color: skyblue; color: #003399; text-align: center; border: solid 1px azure">
                                                                    <tr>
                                                                        <th style="border: solid 1px #CCCCFF; height: 21px;">Member Type</th>
                                                                        <th style="border: solid 1px #CCCCFF; height: 21px;">Member ID</th>
                                                                        <th style="border: solid 1px #CCCCFF; height: 21px;">Member Name</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width:20%;">
                                                                            <select style="width:100%;background-color:transparent">
                                                                                <option selected style="width:100%;background-color:transparent">Amo</option>
                                                                            </select></td>
                                                                        <td style="width:20%;background-color:transparent"><input type=text style="width:100%;border:0;background-color:transparent" value="exzur" /></td>
                                                                        <td style="width:60%;background-color:transparent"><input type=text style="width:100%;border:0;background-color:transparent" value="Carlos Palitaw" /></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:20%;background-color:transparent">
                                                                            <select style="width:100%">
                                                                                <option selected>Aso</option>
                                                                            </select></td>
                                                                        <td style="width:20%;background-color:transparent"><input type=text style="width:100%;border:0;background-color:transparent" value="olik" /></td>
                                                                        <td style="width:60%;background-color:transparent"><input type=text style="width:100%;border:0;background-color:transparent" value="Olik baho" /></td>
                                                                    </tr> 
                                                                    <tr>
                                                                        <td style="width:20%;background-color:transparent">
                                                                            <select style="width:100%">
                                                                                <option selected>Pest</option>
                                                                            </select></td>
                                                                        <td style="width:20%;background-color:transparent"><input type=text style="width:100%;border:0;background-color:transparent" value="toink" /></td>
                                                                        <td style="width:60%;background-color:transparent"><input type=text style="width:100%;border:0;background-color:transparent" value="Gara Poink" /></td>
                                                                    </tr>                                                                                                                                     </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>                                            
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
    <div id="divPopup" style="width: 100%; border: solid 1px midnightblue; display: none; z-index: 10;">
        <table>
            <tr>
                <td id=tdPopup style="width: 100%;vertical-align:middle; padding-top:100px" align="center"> 
                    <table id=tblxgrid cellpadding="0" cellspacing="0">
                        <tr>
                            <td id=td_popup style="width:400px; height: 15px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;
                                <span id=spPopTitle style="text-align:left;width:100%">Popup Window</span>
                            </td>
                            <td style="width:10px" class="table_head_dark" align="right">
                                <input type="button" value="x" onclick="close_popup();" style="width:15px; color: red" class="table_head_dark" />
                            </td>
                        </tr>
                        <tr>
                            <td id=tdpopgrid style="width: 100%;" align="left" colspan="2">
                                <table id=tblpopgrid width=100%></table>
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
