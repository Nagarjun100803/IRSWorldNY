<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuProfiles.aspx.cs" Inherits="IRSWorld.Admin.MenuProfiles" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Default Profiles By Role</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../js/Admin/treetab.css" rel="Stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/Admin/treetab.js" type="text/javascript"></script>
    <script src="../js/Admin/jsMenuProfiles.js" type="text/javascript"></script>
</head>
<body onload="page_setup();" onunload="close_session()" onresize="fixdim();">
    <center>
    <form id="form1" runat="server">
    <div id=divmain style="width: 100%; height:100%; padding-top: 30px; display:none; overflow:auto">
        <table  style="width:100%; height:100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td align="center" valign="middle" style="width:100%; border-collapse:collapse">
                    <table id="tblMenuMaint" style="width:90%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td style="width:100%; vertical-align:bottom; text-align:left" align=left>
                                <table style="width:100%" cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left" style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom;"><img src="../images/toe.GIF" /></td>
                                    <td id="capLabel" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                        Default Profiles By Role</td>
                                    <td align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                        <img align="absBottom" src="../images/rend.gif" /></td> 
                                </tr>
                                </table>
                            </td>                         
                        </tr>
                        <tr>
                            <td style="width: 100%; border:solid 3px #336699;" class=table_body>
                                <table style="width:99%">
                                    <tr><td style="height:8px" colspan=2>&nbsp;</td></tr>
                                    <tr>
                                        <td class=cell_caption style="width:25%">Role:&nbsp;</td>
                                        <td style="width:75%" align=left>
                                            <select id=ddroles style="width: 330px" onchange="modtab_cancel_click();"></select>
                                        </td>
                                    </tr>
                                    <tr><td style="height:8px" colspan=2 valign=bottom><hr /></td></tr>
                                    <tr>
                                        <td colspan=2 style="width:100%" align=left>
                                            <div style="width: 96%;">
                                              <b class="rr">
                                              <b class="rr1"><b></b></b>
                                              <b class="rr2"><b></b></b>
                                              <b class="rr3"></b>
                                              <b class="rr4"></b>
                                              <b class="rr5"></b></b>                                                                
                                            <div id=divmods class="rrfg" style="width:100%; height: 300px; overflow:auto; vertical-align:middle; border:solid 1px #87ceeb">
                                            </div>
                                              <b class="rr">
                                              <b class="rr5"></b>
                                              <b class="rr4"></b>
                                              <b class="rr3"></b>
                                              <b class="rr2"><b></b></b>
                                              <b class="rr1"><b></b></b></b>                                                                    
                                            </div>                                                    
                                        </td>
                                    </tr>
                                    <tr><td style="height:8px" colspan=2><hr /></td></tr>
                                    <tr>
                                        <td colspan=2 style="width:100%" align=center>
                                            <table id=tblbuttons style="width:60%" cellpadding="0" cellspacing="5px">
                                                <tr>
                                                    <td style="width: 18%">
                                                        <input id="btnEdit" class="button" onclick="modtab_edit_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" /></td>
                                                    <td style="width: 18%">
                                                        <input id="btnSave" class="button" onclick="modtab_save_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" /></td>
                                                    <td style="width: 18%">
                                                        <input id="btnCancel" class="button" onclick="modtab_cancel_click();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
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
    </form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../Admin/MenuProfiles.aspx" target="_parent"></form>
    </center>
</body>
</html>
