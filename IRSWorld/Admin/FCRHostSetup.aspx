<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FCRHostSetup.aspx.cs" Inherits="IRSWorld.Admin.FCRHostSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>FCR Interface Setup</title>
    <link href="../css/Default.css"  rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>
    <script src="../js/Admin/jsFCRHostSetup.js" type="Text/javascript"></script>
    <script type="text/javascript">

    </script>
</head>
<body onload="page_setup();" onresize="fixdim();">
<center>
    <form id="form1" runat="server">
    <div id=divmain style="width: 100%; vertical-align: middle; overflow:auto;">
    <table style="width:90%;padding-top: 30px;" cellpadding="0" cellspacing="0">
        <tr>
            <td style="width: 100%; height: 100%; vertical-align: middle;" align="center" valign="middle">
                <table id="tblContent" cellspacing="0" cellpadding="0" style="width:100%; border-collapse:collapse">
                    <tr>
                        <td style="width:60%; vertical-align:bottom" align=left>
                            <table style="width: 100%" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td align="left" style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom;"><img src="../images/toe.GIF" /></td>
                                <td id="tdTitle" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                    FCR Interface Configuration</td>
                                <td align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                    <img align="absBottom" src="../images/rend.gif" /></td> 
                            </tr>
                            </table>
                        </td>                                        
                    </tr>
                    <tr>
                        <td style="width: 100%; height:100%; background: azure; padding-top: 10px" class="table_border">
                            <table style="width: 98%">
                                <tr>
                                    <td style="width:100%" align=center>
                                        <table style="width: 60%">
                                            <tr>
                                                <td style="width: 30%"class=cell_caption>FCR Settings ID:&nbsp;</td>
                                                <td style="width:65%">
                                                    <select id=ddsettingsid style="width:98%" onchange="ddl_setting_change(this);">
                                                    </select>
                                                </td>
                                                <td valign=middle>
                                                    <table>
                                                        <tr>
                                                            <td valign=middle>
                                                                <input id=btnnew type=button class=button onmouseover="this.className='button_over';" onmouseout="this.className='button';" value="New" onclick="btn_new_click();" />
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td valign=middle>
                                                                <input id=btndelete type=button class=button disabled onmouseover="this.className='warning_over';" onmouseout="this.className='button';" value="Delete" onclick="btn_delete_click();" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdsettingsgrid" style="width:100%">
                                            <table id="tblsettingsgrid" style="width:100%" cellspacing="0" cellpadding="0"  class=cell_caption>
                                                <thead id="tblsettingsgrid_hdr" style="height: 22px;background-color: #003399; color: #CCCCFF; text-align: center; border: solid 1px azure">
                                                    <tr>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 108px">
                                                            Name</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 304px">
                                                            Value</th>
                                                        <th style="border: solid 1px #CCCCFF; height: 21px; width: 10%">
                                                            Enabled</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tblmsgindex_body">
                                                    <tr>
                                                        <td style="width: 108px">&nbsp;</td>
                                                        <td style="width: 304px">&nbsp;</td>
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
                            </table>
                        </td>
                    </tr>
                </table>            
            </td>
        </tr>
    </table>                            
    </div>
    <div id="divpopnew" style="width: 371px; border: solid 1px midnightblue; display: none; position: absolute; z-index: 15; left:50px;" class="ds_gray">
        <table style="width:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:320px; height: 15px; font: bold 12px Arial; color: Ivory" class="navbar" align="left">&nbsp;&nbsp;Settings ID</td>
                <td style="width:10px" class="navbar">
                    <input type="button" value="x" onclick="close_popnew();" style="width:15px; color: red" class="table_caption" />
                </td>
            </tr>
            <tr>
                <td style="width:100%;" align="left" colspan="2" valign=top>
                    <table style="width:100%">
                        <tr>
                            <td style="width:100%; padding-top: 10; padding-left: 20px" class=cell_desc>Enter new settings id:
                            </td> 
                        </tr>
                        <tr>
                            <td style="width:100%; height: 38px;" class=cell_desc align=center>
                                <input id=txtnew type="text" onblur="txtnew_blur(this);" style="width:90%" />
                            </td> 
                        </tr> 
                        <tr>
                            <td style="width:100%; height: 38px; vertical-align: middle" align=center>
                                <input id=btnOK type=button class=button onmouseover="this.className='button_over';" onmouseout="this.className='button';" value="Ok" onclick="btn_ok_click();" />
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
