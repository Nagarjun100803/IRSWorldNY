<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Look_Up.aspx.cs" Inherits="IRSWorld.Look_Up.Look_Up" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Look-Up</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/Look_Up/js_look_up.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var look_up = new look_up();
        
        function load_main()
        {
            if (top == self)
            {
                //window.open("../Main/IRSMain.aspx?load=../Look_Up/Look_Up.aspx","_parent");
                document.irs_loader.submit();            
            }
            fix_height();
        }
        
        function fix_height()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmain');
                
                div.style.height = top.document.getElementById('divCenter').offsetHeight + 'px';
                div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
            }
        }
    </script>
    
</head>
<!-- chr 09302010 - added onresize to the body tag and related stuff    -->
<!-- chr 09302010 - cross browser works                                 -->
<body onload = "javascript:load_main();look_up.load_grid_viewer();look_up.load_all_dropdownlist();" onresize="fix_height();">
    <center>
    <form id="form1" runat="server">
    <div id=divmain style="overflow:auto;">
        <div id="Look_Up" style="display: block; padding-top: 30px;">
            <table cellpadding="0" cellspacing="0" width="50%">
                <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" >
                        <tr>
                        <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                        <img src="../images/toe.GIF" /></td>    
                        <td class="table_head_dark" style="width: 50%; height: 24px">
                        Look Up</td>
                        <td align="left" style="width: 100%; height: 24px" valign="bottom">
                        <img align="absBottom" src="../images/rend.gif" /></td>   
                        </tr>
                    </table>    

                </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 899px">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 49%; border-bottom: #336699 3px solid; height: 301px;">
                            <tr>
                                <!--<td style="width: 100%; height: 107%"> chr09302010 -->
                                <td style="width: 100%; height: 107%" align=center>
                                    <table id="tblLookUp" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width: 794px; height: 245px;">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left;">
                                                Look Up Definition
                                            </td>
                                        </tr>
                                    <tr>
                                       <!--<td style="height: 150px; width: 900px; text-align: center;"> chr09302010 -->
                                       <td style="height: 150px; width: 900px;" align=center >
                                               <table id="tblLookUpField" style="width: 642px; height: 132px;">
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Group Name:&nbsp;</td>
                                                       
                                                           <td align="left" style="width: 305px; height: 23px;" colspan="4">
                                                                <input id="txtGroupName" type="text" style="width: 412px; height: 100%; text-align: left;"  onchange="javascript:look_up.insert_fldvalue();"/></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Code Name:&nbsp;</td>
                                                      <td align="left" style="width: 305px; height: 23px;" colspan="4"><input id="txtCodeName" type="text" style="width: 412px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Code Value:&nbsp;
                                                       </td>
                                                       <td align="left" style="width: 305px; height: 23px;" colspan="4"><input id="txtCodeValue" type="text" style="width: 412px; height: 100%; text-align: left;"/> </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Code Description:&nbsp;
                                                       </td>
                                                      <td align="left" style="width: 305px; height: 23px;" colspan="4"><input id="txtCodeDescriptions" type="text" style="width: 412px; height: 100%; text-align: left;" /></td>
                                                   </tr>
                                                </table>                                    
                                                <table cellpadding="0" cellspacing="5" width="300px" id="tblbuttons">
                                                    <tr>
                                                        <td style="width: 25%">
                                                            <input id="btnAdd" class="button" onclick = "javascript:look_up.insert_record();" onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value="  Add  " /></td>
                                                        <td style="width: 25%">
                                                            <input id="btnEdit" class="button" onclick = "javascript:look_up.update_record();" onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value= "  Edit  " disabled="disabled" /></td>
                                                        
                                                        <td style="width: 25%" >
                                                            <input id="btnSave" class="button" onclick = "javascript:look_up.commit_record();"  onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value=" Save " disabled ="disabled"/></td>
                                                        <td style="width: 25%" >
                                                            <input id="btnCancel" class="button" onclick = "javascript:look_up.rollback_record();" onmouseout="this.className='button';"
                                                                onmouseover="this.className='button_over';" type="button" value="Cancel" disabled ="disabled" /></td>
                                                    </tr>
                                                </table>    
                                               
                                               <input id="txtHiddenSeqID"  type="hidden" style="width: 2%" />
                                           <input id="txtHiddenIsEditable"  type="hidden" style="width: 2%" />
                                           <input id="txtHiddenPage"  type="hidden" style="width: 2%" /></td>
                                           
                                    </tr>
                                        
                                    </table>
                                    <div style="text-align: center">
                                            <table id="Table1" width='100%'>
                                                <tr>
                                                    <td class="cell_caption" style="width: 20%; height: 23px">
                                                            Filter By:&nbsp;
                                                    </td>
                                                    <td align="left" style="width: 40%; height: 24px;" colspan="4">
                                                        <select id="ddFilterBy" style="width: 100%" onchange="javascript:execute_ajaxgridview_command('ajx_search$' + this.value)">
                                                            <option selected="selected"></option>
                                                        </select>
                                                    </td>
                                                    <td align="left" style="width: 40%; height: 24px;" colspan="4">
                                                        <input id="Text1" type="text" style="width: 96%" onkeyup="javascript:execute_ajaxgridview_command('ajx_search$' + this.value)"/></td>
                                                </tr>
                                            </table>
                                    </div>
                                    <!-- chr09302010 for cross browser
                                    <div style="overflow:auto; width:714px; height: 230px; vertical-align:middle"> 
                                        <div style="text-align: center" id="LookUpGrid"> 
                                        </div>
                                    </div>
                                    -->
                                    <table style="width:100%">
                                        <tr><td>&nbsp;</td></tr>
                                        <tr>
                                            <td style="vertical-align:middle;" align=center>
                                                <div style="overflow:auto; width:714px; height: 230px; vertical-align:middle"> 
                                                    <div id="LookUpGrid"> 
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr> <!--chr09302010 added to put a spacer at the botton of the page -->
            </table>
        </div>
    </div>
    </form>
    </center>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../Look_Up/Look_Up.aspx" target="_parent" style="overflow: auto">
        &nbsp;</form>
</body>
</html>

  