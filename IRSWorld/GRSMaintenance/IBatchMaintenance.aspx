<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IBatchMaintenance.aspx.cs" Inherits="IRSWorld.GRSMaintenance.IBatchMaintenance" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Batch Maint</title>
         
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/GRS/js_BatchMaint.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var BatchMaint = new BatchMaint();
        
        function load_main()
        {
            if (top == self)
            {
                //window.open("../Main/IRSMain.aspx?load=../Look_Up/Look_Up.aspx","_parent");
                document.irs_loader.submit();            
            }
        }
        function fix_height()
        {
            if (top.document.getElementById('divCenter'))
            {
                var tbl = document.getElementById('tblContent');
                var div = document.getElementById('divContent');
                
                tbl.style.height = top.document.getElementById('divCenter').offsetHeight;
                div.style.height = top.document.getElementById('divCenter').offsetHeight;
            }
        }
    </script>
    
</head>
<body onload = "javascript:BatchMaint.load_all_dropdownlist();BatchMaint.load_grid_viewer();">
    <form id="form1" runat="server">
    <div style="overflow:auto;height:552px">
    <center> 
    <div id="Batch_Maint" style="display: block ">
    
            <table> <!--Base Table-->
            <tr>
                <td> 
                    <!--Batch Maint table-->
                     <table id="tblBatchMaint" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                        border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; text-align: center; width: 100%; height: 100%">
                        <tr>
                            <td class="table_caption" colspan="5" style="color: black; text-indent: 5px; text-align: left;">Batch Maintenance</td>                   
                        </tr>
                        <tr>
                            <td style="height: 150px; width: 900px; text-align: center;">
                                   <table id="tblBatchMaintField" style="width: 642px; height: 132px;">
                                  
                                       <tr> <!-- Type Format-->
                                           <td class="cell_caption" style="width: 143px; height: 23px">File Type : </td>
                                           <td align="left" style="width: 305px; height: 23px;" colspan="4">
                                            <select id="ddlType" style="width: 75%" tabindex="4"></select>    
                                           </td> 
                                       </tr>
                                       <tr> <!-- File Format-->
                                           <td class="cell_caption" style="width: 143px; height: 23px">File Format :</td>
                                           <td align="left" style="width: 305px; height: 23px;" colspan="4"> 
                                            <select id="ddlFileFormat" style="width: 75%" tabindex="4"></select>   
                                           </td>
                                       </tr>
                                       <tr> <!-- Target table-->
                                           <td class="cell_caption" style="width: 143px; height: 23px">Target Table :</td>
                                           <td align="left" style="width: 305px; height: 23px;" colspan="4">
                                            <select id="ddlTargetTable" style="width: 75%" tabindex="4"></select>
                                           </td>
                                       </tr>
                                       <tr> <!-- Target Field-->
                                           <td class="cell_caption" style="width: 143px; height: 23px">Target Field :</td>
                                           <td align="left" style="width: 305px; height: 23px;" colspan="4">
                                            <select id="ddlTargetField" style="width: 75%" tabindex="4"> </select>
                                           </td>
                                       </tr>
                                        <tr> <!-- Source Field-->
                                           <td class="cell_caption" style="width: 143px; height: 23px">Source Field :</td>
                                           <td align="left" style="width: 305px; height: 23px;" colspan="4">
                                            <select id="ddlSourceField" style="width: 75%" tabindex="4"></select>
                                           </td>
                                       </tr>
                                        <tr> <!-- CIF Class-->
                                           <td class="cell_caption" style="width: 143px; height: 23px">CIF Class Type :</td>   
                                           <td align="left" style="width: 305px; height: 23px;" colspan="4">
                                            <select id="ddlCIFClass" style="width: 75%" tabindex="4">  </select>
                                           </td>
                                       </tr> 
                                      </table>
                        
                                      <table cellpadding="0" cellspacing="5" width="300px" id="tblbuttons">
                                                <tr>
                                                    <!--Add-->
                                                    <td style="width: 25%">
                                                        <input id="btnAdd" class="button" onclick = "javascript:BatchMaint.insert_record();" onmouseout="this.className='button';"
                                                            onmouseover="this.className='button_over';" type="button" value="  Add  " />
                                                    </td>
                                                    <!--Edit-->
                                                    <td style="width: 25%">
                                                        <input id="btnEdit" class="button" onclick = "javascript:BatchMaint.update_record();" onmouseout="this.className='button';"
                                                            onmouseover="this.className='button_over';" type="button" value= "  Edit  " disabled="disabled" />
                                                    </td>
                                                    <!--Save-->  
                                                    <td style="width: 25%" >
                                                        <input id="btnSave" class="button" onclick = "javascript:BatchMaint.commit_record();"  onmouseout="this.className='button';"
                                                            onmouseover="this.className='button_over';" type="button" value=" Save " disabled ="disabled"/>
                                                    </td>
                                                    <!--Delete-->
                                                    <td style="width: 25%" >
                                                        <input id="btnDelete" class="button"   onmouseout="this.className='button';"
                                                            onmouseover="this.className='button_over';" type="button" value=" Delete " disabled ="disabled"/>
                                                    </td>
                                                    <!--Cancel-->
                                                    <td style="width: 25%" >
                                                        <input id="btnCancel" class="button" onclick = "javascript:BatchMaint.rollback_record();" onmouseout="this.className='button';"
                                                            onmouseover="this.className='button_over';" type="button" value="Cancel" disabled ="disabled" />
                                                    </td>
                                                </tr>
                                      </table>
                                           
                                      <input id="txtHiddenSeqID"  type="hidden" style="width: 2%" />
                                      <input id="txtHiddenIsEditable"  type="hidden" style="width: 2%" />
                              </td>      
                        </tr>
                    </table> <!--End of Table Batch Maint-->
                </td>
            </tr>
            <tr>
                <td>
                 <div style="text-align: center">
                    <table style="width:50%; border:solid 3px #336699"><!--Table filter-->
                        <tr>
                            <td class="cell_caption" style="width: 143px; height: 23px">Filter By:&nbsp; </td> 
                            <td align="left" style="width: 305px; height: 24px;" colspan="4">
                                <select id="ddFilterBy" style="width: 100%" onchange="javascript:execute_ajaxgridview_command('ajx_search$' + this.value)">
                                    <option selected="selected"></option>
                                </select>
                            </td>
                        </tr>
                    </table><!-- End of Table filter-->
                    
                </div><center>
                    <div style="overflow:auto;width:714px; height: 225px;"> 
                        <div style="text-align: center" id="BatchMaintGrid"></div>
                    </div> </center>
                </td>
            </tr>
            </table> <!--End of Base Table-->
        </div>
    </center>                                   
    </div>
    </form>
</body>
</html>
 