<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Modules.aspx.cs" Inherits="IRSWorld.Admin.Modules" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRS Modules Registration Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../js/jstree/dtree.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/jstree/dtree.js" type="text/javascript"></script>
    <script src="../js/Admin/jsModules.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>     
</head>
<body onload="page_setup();" onunload="close_session();" onresize="fixdim();">
    <center>
    <form id="form1" runat="server">
    <div id="divBody" style="width:100%; height: 100%; overflow:auto">
        <table id=tblpopup1 style="width:100%; height:100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td align="center" valign="middle" style="width:100%; border-collapse:collapse" class="content_height">
                    <table id=tblpopup2 style="width:75%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:60%; vertical-align:bottom" align=left>
                                    <table style="width:100%" cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td id=tdpopup1 align="left" style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom;"><img src="../images/toe.GIF" /></td>
                                        <td id="capLabel" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            IRSWorld Modules/Pages</td>
                                        <td id=tdpopup2 align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                                         
                            </tr>
                            <tr>
                                <td style="width: 100%; background: azure">
                                <table style="width:100%; border:solid 3px #336699;">
                                    <tr>
                                        <td style="width: 100%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdModules" style="width: 100%">
                                            <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                            <table id="tblModules" style="width:100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td></td>
                                                </tr>
                                            </table>
                                            <!-- content ends here -->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td id="tdcloser" align="center" style="width: 100%; border-collapse: collapse; display: none">
                                            <input type="button" class="button" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" value="Close" onclick="close_frame_dialog();" />
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
    <div id="divExplore" style="width: 205px; border: solid 1px midnightblue; display: none; position: absolute; z-index: 10" class="ds_gray">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:185px; height: 15px; font: bold 12px Arial; color: Ivory" class="navbar" align="left">&nbsp;&nbsp;IRSWorld</td>
                <td style="width:15px" class="navbar" align=left>
                    <input type="button" value="x" onclick="close_explore();" style="width:15px; color: red; text-align:center;" class="table_caption" />
                </td>
            </tr>
            <tr>
                <td style="width:100%; height:200px;" align="left" colspan="2">
                    <div id="divTree" style="width:100%; height:100%; overflow:auto" class="dtree">
                        <script type="text/javascript">
                            var fs = IRSWorld.Admin.Modules.getFs();
                            if (fs.error != null)
                            {
                                document.write('Unable to get IRSWorld File Structure.');
                            }
                            else
                            {
                                root_path = fs.value[0];
                                var lsdir = fs.value[1];
                                d3 = new dTree('d3');
                                for (var k = 0; k < lsdir.length; k++)
                                {
                                    if (lsdir[k][3] == 'F')
                                    {
                                        d3.add(lsdir[k][0], lsdir[k][1], lsdir[k][2], 'javascript:fselect();');
                                    }
                                    else
                                    {
                                        d3.add(lsdir[k][0], lsdir[k][1], lsdir[k][2]);
                                    }
                                }
                                document.write(d3);  
                            } 
                            fs = null;                        
                        </script>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divPopFind" style="width: 100%;border: solid 1px midnightblue; display: none; z-index: 20;">
        <table style="width:100%; height:100%;"  cellpadding="0" cellspacing="0">
            <tr>
                <td id=tdPopFind align=center style="width:100%; height:100%; vertical-align:middle">
                    <table id=tblPopFind cellpadding="0" cellspacing="0">
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
                                    <table style="width:100%;"; cellpadding="0" cellspacing="0">
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
    </form>
        <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../Admin/Modules.aspx" target="_parent"></form>
    </center>
</body>
</html>
