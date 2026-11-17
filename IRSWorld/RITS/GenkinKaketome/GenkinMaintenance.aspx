<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GenkinMaintenance.aspx.cs" Inherits="IRSWorld.RITS.GenkinKaketome.GenkinMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Genkin Kaketome</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jsCommons.js" type="text/javascript"></script>
    <script src="../../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../../js/RITS/jsGenkinkaketome.js" type="text/javascript"></script>
    
</head>
<body>
    <form id="form1" runat="server">

       

            <div style="width: 75%; height: 100%; vertical-align: top; margin-top: 30px;">
                <table width="100%" height="100%" cellpadding="0" cellspacing="5">
                    <tr>
                        <td style="width: 100%; border-collapse: collapse; height: 274px; vertical-align: top;"
                            class="content_height">
                           
                            <div id="divSearch" style="width: 95%; display: block;">
                                <table id="tblSearch" width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" valign="bottom" style="width: 1%; height: 24px; border-collapse: collapse">
                                            <img src="../../images/toe.GIF" /></td>
                                        <td id="capSearch" align="left" class="table_head_dark" style="width: 40%; height: 24px;
                                            border-collapse: collapse">
                                            Genkin Kakitome</td>
                                        <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body">
                                            <img align="absbottom" src="../../images/rend.gif" /></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; background: azure">
                                            <table id="tblMain" style="width: 100%; height: 56px;" >
                                                <tr>
                                                    
                                                    <td style="width: 100%; height: 18px;" colspan=2>
                                                       <table style="width:100%" class="table_border">
                                                      <%-- <tr>
                                                            <td style="width:25%" class="cell_caption">Teller
                                                                </td>
                                                            <td style="width:60%" class="cell_desc" align=left>
                                                                <input id="txtTeller" type="text" style="width:100%" readonly=readonly /></td>
                                                            <td style="width:15%" align=center>
                                                                <input id="Button1" type="button" value="..." style="width: 66%" onclick="" /></td>
                                                       </tr>--%>
                                                       
                                                       <tr>
                                                            <td style="width:25%" class="cell_caption">Batch No
                                                                </td>
                                                            <td style="width:60%" class="cell_desc" align=left colspan=3>
                                                                <input id="txtBatchNo" type="text" style="width:10%"  /></td>
                                                            
                                                       </tr>
                                                      <tr>
                                                            <td style="width:25%" class="cell_caption">Remitter
                                                                </td>
                                                            <td style="width:60%" class="cell_desc" align=left>
                                                                <input id="txtRemName" type="text" style="width:100%" readonly=readonly /></td>
                                                            <td style="width:15%" align=center>
                                                                <input id="Button2" type="button" value="..." style="width: 66%" onclick="cls_Genkin.search();" /></td>
                                                       </tr>
                                                       <tr>
                                                            <td style="width:25%" class="cell_caption">Amount Received in Yen
                                                                </td>
                                                            <td style="width:60%" class="cell_desc" align=left colspan=3>
                                                                <input id="txtYen" type="text" style="width:30%; text-align:right" value="0" /></td>
                                                            
                                                       </tr>
                                                       <tr>
                                                            <td style="width:25%" class="cell_caption">Amount Received in Dollar
                                                                </td>
                                                            <td style="width:60%" class="cell_desc" align=left colspan=3>
                                                                <input id="txtDollar" type="text" style="width:30%; text-align:right" value="0"  /></td>
                                                            
                                                       </tr>
                                                       <tr>
                                                            <td style="width:25%" class="cell_caption">Remarks
                                                                </td>
                                                            <td style="width:60%" class="cell_desc" align=left colspan=3>
                                                                <textarea id="TextArea1" cols="20" rows="5" style="width:90%"></textarea></td>
                                                            
                                                       </tr>
                                                       
                                                       
                                                       
                                                       
                                                       
                                                       
                                                       </table>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <table id="Table1" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                    border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid">

                                    <tr>
                                        <td style="width: 25%; " align=center>
                                            <input id="btnAdd" type="button" value="Add" /></td>
                                        <td style="width: 25%; " align=center><input id="btnSave" type="button" value="Save/Exit" /></td>
                                        <td style="width: 25%; " align=center><input id="btnCancel" type="button" value="Cancel" /></td>
                                    </tr>
                                </table>
                                
                                
                                
                                <table id="tblButtons" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                    border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid; margin-top:5px" visible=false>

                                    <tr>
                                        <td style="width: 25%; " align=center>
                                            <input id="btn102k" type="button" value="Print List of more than 102k" /></td>
                                        <td style="width: 25%; " align=center><input id="btnSplit" type="button" value="Split/Transfer/Follow-up" /></td>
                                        <td style="width: 25%; " align=center><input id="btnFind" type="button" value="Find" /></td>
                                        <td style="width: 25%; " align=center><input id="btnSave2" type="button" value="Save/Exit" /></td>
                                    </tr>
                                </table>
                                <!--<label align="right" class="cell_desc" onclick="set_visibility('Main');">Go back to main</label>-->
                                <!--<input type="button" value="Go Back" onclick="set_visibility('Main');" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnBack" />-->
                            </div>
                        </td>
                        
                    </tr>
                    <tr>
                        <td style="width: 17%; height: 18px;" align="right">
                        </td>
                    </tr>
                </table>
            </div>
           
        </form>
    
 

</body>
</html>
