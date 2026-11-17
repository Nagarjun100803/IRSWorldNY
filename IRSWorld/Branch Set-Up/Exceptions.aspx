<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Exceptions.aspx.cs" Inherits="IRSWorld.BranchSetup.Exceptions" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch </title>    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
     <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_Exception.js" type="text/javascript"></script>
    <script type="text/javascript">
        var exceptions = new exceptions();
    </script>
</head>
<body onload="javascript:exceptions.load_grid_viewer();">
    <form id="form1" runat="server" autocomplete="Off">
    <div>
    <center>
        <div id="Exceptions" style="display: block">
            <br />
            <br />
            <br />
            <br />
            <table cellpadding="0" cellspacing="0" style="width:50%">
              <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 60%; vertical-align: bottom; text-align: left" align="left">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width: 0%; border-collapse: collapse; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td class="table_head_dark" style="width: 40%; height: 24px">
                                                Exceptions</td>
                                            <td align="left" style="width: 60%; height: 24px" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3"><table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid; background-color: azure">
                                    <tr>
                                        <td style="width: 100%; height: 21px">
                                            <table id="tblExceptions" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                                border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                                width:800px">
                                                <tr>
                                                    <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                        height: 36px; text-align: left">
                                                        Exceptions</td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td style="height: 190px; background-color: azure">
                                                        <div style="text-align: center">
                                                            <table id="tblExceptions1">
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 260px; height: 23px">
                                                                        Remitter ID:&nbsp;</td>
                                                                    <td style="width: 100px; text-align: left">
                                                                        <input id="txtRemitterID" onkeyup="" style="width: 188px"
                                                                            type="text" /></td>
                                                                    <td style="width: 50px">
                                                                        <input id="btnSearch" class="button" onclick="javascript:exceptions.panel_search_exception()"
                                                                            onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                            type="button" value="Search" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 260px; height: 23px">
                                                                        Last Name:&nbsp;</td>
                                                                    <td style="width: 342px; text-align: left">
                                                                        <input id="txtLastName" onkeyup="" type="text" style="width: 98%" /></td>
                                                                    <td style="width: 50px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 260px; height: 23px">
                                                                        First Name:&nbsp;</td>
                                                                    <td style="width: 342px; text-align: left">
                                                                        <input id="txtFirstName" onkeyup="" type="text" style="width: 98%" /></td>
                                                                    <td style="width: 50px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 260px; height: 23px">
                                                                        Middle Name:&nbsp;</td>
                                                                    <td style="width: 342px; text-align: left">
                                                                        <input id="txtMiddleName" onkeyup="" type="text" style="width: 98%" /></td>
                                                                    <td style="width: 50px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 260px; height: 26px">
                                                                        &nbsp;Reason for Exception:&nbsp;
                                                                    </td>
                                                                    <td align="left" style="width: 342px"><input id="txtEReason" onkeyup="" type="text" style="width: 98%" /></td>
                                                                    <td style="width: 50px; height: 26px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 260px; height: 23px">
                                                                        Reason for Cancellation:&nbsp;
                                                                    </td>
                                                                    <td style="width: 342px; text-align: left"><input id="txtCReason" onkeyup="" type="text" style="width: 98%" /></td>
                                                                    <td style="width: 50px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 260px; height: 15px">
                                                                        Remarks:&nbsp;
                                                                    </td>
                                                                    <td id="" style="width: 342px; text-align: left">
                                                                        <input id="txtRemarks" type="text" style="width: 98%" /></td>
                                                                    <td style="width: 50px">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <table id="tblbuttons" cellpadding="0" cellspacing="5" style="width:300">
                                                            <tr>
                                                                <td style="width: 25%; height: 22px">
                                                                    <input id="btnAdd" class="button" onclick="javascript:exceptions.insert_record();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="  Add  " /></td>
                                                                <td style="width: 25%; height: 22px">
                                                                    <input id="btnEdit" class="button" disabled="disabled" onclick="javascript:exceptions.update_record();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="  Edit  " /></td>
                                                                <td style="width: 25%; height: 22px">
                                                                    <input id="btnSave" class="button" disabled="disabled" onclick="javascript:exceptions.commit_record();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value=" Save " /></td>
                                                                <td style="width: 25%; height: 22px">
                                                                    <input id="btnCancel" class="button" disabled="disabled" onclick="javascript:exceptions.rollback_record();"
                                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                        type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                        <div id="divException">
                                                        </div>
                                                        <input id="txtHiddenSeqID" style="width: 3%" type="hidden" />
                                                        <input id="txtHiddenSeqID2" style="width: 3%" type="hidden" />
                                                        <input id="txtDateCreated" style="width: 3%" type="hidden" />
                                                        <input id="txtDateModified" style="width: 3%" type="hidden" /></td>
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
    </center>
    </div>
    </form>
</body>
</html>
