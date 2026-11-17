<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EndOfTransfer.aspx.cs" Inherits="IRSWorld.BranchSetup.EndOfTransfer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>End Of Transfer</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_EndOfTransfer.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var eot = new eot();
        
        
    </script>
    
</head>
<body onload = "javascript:eot.load_grid_viewer();eot.load_all_dropdownlist();">
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="EOT" style="display: block">
            <table cellpadding="0" cellspacing="0" style="width:50%">
                <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" >
                        <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                        <img src="../images/toe.GIF" /></td>    
                        <td class="table_head_dark" style="width: 50%; height: 24px">
                            Branch Setup</td>
                        <td align="left" style="width: 100%; height: 24px" valign="bottom">
                        <img align="absBottom" src="../images/rend.gif" /></td>   
                    </table>    

                </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 900px">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 49%; border-bottom: #336699 3px solid; height: 200px;">
                            <tr>
                                <td style="width: 100%; height: 100%">
                                    <table id="tblEOT" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; text-align: center; width: 794px; height: 200px;">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                text-align: left; height: 26px;">
                                                End Of Transfer</td>
                                        </tr>
                                      
                                    <tr>
                                       <td style="height: 116px; width: 900px; text-align: center; background-color: azure;">
                                     
                                                 <table id="tblEOTField" cellspacing="8" cellpadding="0" style="width:100%">
                                                <tr>
                                                    <td class="cell_caption" style="width: 25%">
                                                        CutOff: &nbsp;</td>
                                                    <td style="WIDTH: 25%" align="left">
                                                        <select id="ddCutOff">
                                                            <option selected="selected"></option>
                                                        </select>
                                                    </td>
                                                     <td align="left" style="width: 25%; text-align: right;" class="cell_caption">
                                                         Time:&nbsp
                                                     </td>
                                                    <td align="left" style="width: 25%; text-align: center;"><select id="ddTime" size="1">
                                                        <option value="12:00 AM"> 12:00 AM</option>
                                                        <option value="1:00 AM"> 1:00  AM</option>
                                                        <option value="2:00 AM"> 2:00  AM</option>
                                                        <option value="3:00 AM"> 3:00  AM</option>
                                                        <option value="4:00 AM"> 4:00  AM</option>
                                                        <option value="5:00 AM"> 5:00  AM</option>
                                                        <option value="6:00 AM"> 6:00  AM</option>
                                                        <option value="7:00 AM"> 7:00  AM</option>
                                                        <option value="8:00 AM"> 8:00  AM</option>
                                                        <option value="9:00 AM"> 9:00  AM</option>
                                                        <option value="10:00 AM"> 10:00 AM</option>
                                                        <option value="11:00 AM"> 11:00 AM</option>
                                                        <option value="12:00 PM"> 12:00 PM</option>
                                                        <option value="1:00 PM"> 1:00  PM</option>
                                                        <option value="2:00 PM"> 2:00  PM</option>
                                                        <option value="3:00 PM"> 3:00  PM</option>
                                                        <option value="4:00 PM"> 4:00  PM</option>
                                                        <option value="5:00 PM"> 5:00  PM</option>
                                                        <option value="6:00 PM"> 6:00  PM</option>
                                                        <option value="7:00 PM"> 7:00  PM</option>
                                                        <option value="8:00 PM"> 8:00  PM</option>
                                                        <option value="9:00 PM"> 9:00  PM</option>
                                                        <option value="10:00 PM"> 10:00 PM</option>
                                                        <option value="11:00 PM"> 11:00 PM</option>
                                                        <option selected="selected"></option>
                                                    </select>
                                                    </td>
                                                </tr>
                                               
                                          
                                           </table>
                                        <br />
                                                        <table cellpadding="0" cellspacing="5" style="width:300px" id="tblbuttons">
                                                            <tr>
                                                                <td style="width: 25%; height: 22px;">
                                                                    <input id="btnAdd" class="button" onclick = "javascript:eot.insert_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="  Add  " /></td>
                                                                <td style="width: 25%; height: 22px;">
                                                                    <input id="btnEdit" class="button" onclick = "javascript:eot.update_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value= "  Edit  " disabled="disabled" /></td>
                                                                <td style="width: 25%; height: 22px;" ><input id="btnDelete" class="button" onclick = "javascript:eot.delete_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value= "Delete" disabled="disabled" /></td>
                                                                <td style="width: 25%; height: 22px;" >
                                                                    <input id="btnSave" class="button" onclick = "javascript:eot.commit_record();"  onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value=" Save " disabled ="disabled"/></td>
                                                                <td style="width: 25%; height: 22px;" >
                                                                    <input id="btnCancel" class="button" onclick = "javascript:eot.rollback_record();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="Cancel" disabled ="disabled" /></td>
                                                            </tr>
                                                        </table>
                                                       
                                                       <br />
                                                       <input id="txtHiddenSeqID"  type="hidden" style="width: 2%" /></td>
                                           
                                    </tr>
                                        
                                    </table>
                                    
                                        <div style="text-align: center" id="EOTGrid">
                                        </div>
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