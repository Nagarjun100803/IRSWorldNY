<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="IRSWorld.Admin.Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRS Menu Maintenance Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../js/jstree/dtree.css" rel="stylesheet" type="text/css" />    
    <style type="text/css">
    </style>    
    <script src="../js/commons.js" type="text/javascript"></script> 
    <script src="../js/datagrid.js" type="text/javascript"></script> 
    <script src="../js/jstree/dtree.js" type="text/javascript"></script>
    <script src="../js/Admin/jsMenu.js" type="text/javascript"></script>
</head>
<body  onload="page_setup();" onunload="close_session();" onresize="fixht();">
    <center>
    <form id="form1" runat="server">
    <div id=divmain style="width: 100%; height:100%; display:none; overflow:auto">
        <table  style="width:100%;height:100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td align="center" valign="middle" style="width:100%; border-collapse:collapse">
                    <table id="tblMenuMaint" style="width: 90%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width:100%; vertical-align:bottom" align=left>
                                <table style="width:100%"cellpadding="0" cellspacing="0" >
                                <tr>
                                <td style="width:60%; vertical-align:bottom" align=left>
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width:0px; height:24px; border-collapse:collapse; vertical-align:bottom;">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capLabel" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                                Menu Maintenance</td>
                                            <td align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                                <img align="absBottom" src="../images/rend.gif" /></td> 
                                        </tr>
                                    </table>
                                </td>
                                </tr>
                                </table>
                            </td>                                 
                        </tr>
                        <tr>
                            <td style="width: 100%; border:solid 3px #336699;" class=table_body>
                                <table style="width:100%; " cellspacing="3">
                                    <tr>
                                        <td id="tdTree" style="width:25%; height:340px; vertical-align:top" align=left>
                                            <table style="width: 100%; height:100%" cellspacing="2" cellpadding="0">
                                                <tr>
                                                    <td align="left" style="height: 22px;background-color:#c0c0ff; padding-left: 7px">                                            
                                                        <a id="lnkshowall" style="height:100%; font-weight: bold; font-size: 10px; font-family: Arial;" href="javascript:show_all();">Show all</a>
                                                    </td>
                                                </tr>                                              
                                                <tr>
                                                    <td style="width: 100%;height: 100%;">
                                                        <div id=divTree style="width: 100%;height: 330px; overflow-y:scroll; background-color:Transparent">
                                                        </div>
                                                    </td>
                                                </tr>
                                              
                                            </table>
                                        </td>
                                        <td style="width:75%; vertical-align:top" align="left">
                                            <table style="width:100%" cellspacing="0" cellpadding="0">
                                                <caption class="navbar" style="font-weight: bold; color: Lavender;font-family:Arial; font-size: medium">IRSWorld Menu</caption>
                                                <tr><td>&nbsp;</td></tr>
                                                <tr><td>&nbsp;</td></tr>
                                                <tr>
                                                    <td id="tditems" style="width:637px; height: 140px;" align=center>
                                                        <table id="tblitems" style="width:98%" cellspacing=5>
                                                            <tr>
                                                                <td style="width: 25%;" class=cell_caption>Menu Group:&nbsp;
                                                                </td>
                                                                <td style="width: 75%;" align=left>
                                                                    <input type=text id=txtMenuGroup style="width:221px; height: 17px;" onchange="menu_txt_change(this);" />
                                                                    <input id=btnBrowGroup type=button style="width: 20px" class=button value="»" onclick="show_explore(document.getElementById('txtMenuGroup'), document.getElementById('divPopgrp'), 'Menu Groups');" />
                                                                    <input type=text id=txtMenuSeqId style="width:30px; height: 17px;" />
                                                                    <input type=text id=txtMenuGroupId style="width:30px; height: 17px;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 25%;" class=cell_caption>Menu Parent:&nbsp;
                                                                </td>
                                                                <td style="width: 75%;" align=left valign=middle>
                                                                    <input id=txtMenuParent type=text style="width:220px; height:17px;" onchange="menu_txt_change(this);" />
                                                                    <input id=btnBrowParent type=button style="width: 20px" class=button value="»" onclick="show_explore(document.getElementById('txtMenuParent'), document.getElementById('divPopma'), 'Menu Parents');" />
                                                                    <input type=text id=txtMenuParentId style="width:30px; height: 17px;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 25%;" class=cell_caption>Menu Item No:&nbsp;
                                                                </td>
                                                                <td style="width: 75%;" align=left valign=middle>
                                                                    <input id=txtMenuItemNo type=text style="width:44px; height:17px; text-align: right;" onchange="menu_txt_change(this);" />
                                                                </td>
                                                            </tr>                                                            
                                                            <tr>
                                                                <td style="width: 25%;" class=cell_caption>Menu Name/Title:&nbsp;
                                                                </td>
                                                                <td style="width: 75%;" align=left>
                                                                    <input id=txtMenuName type=text style="width:390px;height:17px" onchange="menu_txt_change(this);"/>
                                                                    <input type=text id=txtModuleId style="width:30px; height: 17px;" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 25%;" class=cell_caption>Module Url:&nbsp;
                                                                </td>
                                                                <td style="width: 75%;" align=left>
                                                                    <input id=txtMenuUrl type=text style="width:390px;height:17px;margin:0" onchange="menu_txt_change(this);" />
                                                                    <input id=btnBrowMod type=button style="width: 20px"class=button value="»" onclick="grid_btn_browse();" />
                                                                </td>
                                                            </tr> 
                                                            <tr>
                                                                <td>&nbsp;</td>
                                                            </tr> 
                                                            <tr>
                                                                <td></td>
                                                            </tr>  
                                                            <tr>
                                                                <td style="width:100%" colspan=2 align=center>
                                                                
                                                                    <div id="divButtons" style="width: 60%; background: transparent">
                                                                        <table id=tblbuttons style="width:100%" cellpadding="0" cellspacing="5px">
                                                                            <tr>
                                                                                <td style="width: 18%">
                                                                                    <input id="btnEdit" class="button" onclick="menu_edit_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" /></td>
                                                                                <td style="width: 18%">
                                                                                    <input id="btnAdd" class="button"  onclick="menu_add_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add" /></td>
                                                                                <td style="width: 18%">
                                                                                    <input id="btnDelete" class="button" onclick="menu_del_click();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Delete" /></td>
                                                                                <td style="width: 18%">
                                                                                    <input id="btnSave" class="button" onclick="menu_save_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" /></td>
                                                                                <td style="width: 18%">
                                                                                    <input id="btnCancel" class="button" onclick="menu_cancel_click();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
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
                            </td>
                        </tr>
                    </table> 
                </td>
            </tr>
        </table>    
    </div>
    <div id="divExplore" style="width: 220px; height: 210px; border: solid 1px midnightblue; display: none; position: absolute; z-index: 10" class="ds_gray">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td id=tdPopTitle style="width:210px; height: 15px; font: bold 12px Arial; color: Ivory" class="navbar" align="left">&nbsp;&nbsp;IRSWorld</td>
                <td style="width:10px" class="navbar">
                    <input type="button" value="x" onclick="close_explore();" style="width:15px; color: coral" class="table_caption" />
                </td>
            </tr>
            <tr>
                <td id=tdPop style="width:100%; height:100%;" align="left" colspan="2">
                    <div id="divPopgrp" style="width:100%; height:100%; overflow:auto;display:none" class="cell_caption">
                    </div>
                        <div id=divPopma  style="width:100%; height:100%; overflow:auto;display:none" class="cell_caption">
                    </div>
                </td>
            </tr>
        </table>
    </div>        
    </form>
    </center>
</body>
</html>
