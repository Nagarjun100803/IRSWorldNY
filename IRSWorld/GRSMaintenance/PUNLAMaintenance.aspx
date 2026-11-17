<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PUNLAMaintenance.aspx.cs" Inherits="IRSWorld.GRSMaintenance.PUNLAMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>PUNLA Maintenance</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/GRS/js_PUNLAMaint.js" type="text/javascript"></script>
    <script type="text/javascript">
    
        var PUNLA_Maint = new PUNLA_Maint();
        
    </script>
</head>
<body onload = "PUNLA_Maint.load_grid_viewer();PUNLA_Maint.load_all_dropdownlist();">
    <form id="form1" runat="server">
    <div>
    
          <table style="width:100%; border:solid,1,#000000;" cellspacing="0" cellpadding="0" >
              <tr>
                   <td colspan="2"><br /></td>         
              </tr>  
              <tr>
                  <td style="height:26px" colspan="2" class="table_caption" >PUNLA Maintenance</td>
              </tr>  
               
              <tr>
                   <td colspan="2"><br /></td>
              </tr> 
              <tr>
                <td> <table id="tblMaintFields" style="width:100%; border:solid,1,#000000;" cellspacing="0" cellpadding="0">
                
                      <tr>
                           <td style="width:35%; " class="cell_caption" >Agency:&nbsp;</td>   
                           <td ><input id="txtAgency" type="text" /></td>                       
                      </tr> 
            
                      <tr>
                           <td style="width:35%; " class="cell_caption" >Remitter Name:&nbsp;</td>
                           <td >     <input id="txtRemitterName" type="text" /></td>
                      </tr> 
                      
                      <tr>
                           <td style="width:35%; " class="cell_caption" >Account Number:&nbsp;</td>
                           <td ><input id="txtAccountNo" type="text" /></td>                 
                      </tr> 
             
                      <tr>
                          <td style="width:35%;" class="cell_caption" >Percentage of Contribution:&nbsp;</td>
                          <td ><input id="txtPercentContri" type="text" /></td>     
                      </tr>   
                      <tr>
                           <td style="width:35%; height: 21px;" class="cell_caption" >Client ID:&nbsp;</td>      
                           <td style="height: 21px" > <input id="txtClientID" type="text" /></td>    
                      </tr>   
                      <tr>
                           <td style="width:35%; " class="cell_caption" >&nbsp;Amount Credited to PUNLA UITF:&nbsp;</td>
                           <td ><input id="txtAmtCrPUNLA" type="text" /></td> 
                      </tr>            
                </table> 
                </td>
              
              </tr>   
              <tr>
                    <td>
                    <table cellpadding="0" cellspacing="5" width="100%" id="tblbuttons">
                      <tr>
                           <td colspan="2" style="text-align:center " >
                               &nbsp;<input id="btnAdd" class="button" type="button" value="Add" 
                               onclick = "javascript:PUNLA_Maint.insert_record();" onmouseout="this.className='button';"
                               onmouseover="this.className='button_over';"/>
                               &nbsp;<input id="btnEdit" class="button" type="button" value="Edit"  
                               onclick = "javascript:PUNLA_Maint.update_record();" onmouseout="this.className='button';"
                               onmouseover="this.className='button_over';"/>
                               &nbsp;<input id="btnSave" class="button" type="button" value="Save" 
                                onclick = "javascript:PUNLA_Maint.commit_record();" onmouseout="this.className='button';"
                               onmouseover="this.className='button_over';"/>
                               &nbsp;<input id="btnDelete" class="button" type="button" value="Delete" 
                                onclick = "javascript:PUNLA_Maint.delete_record();" onmouseout="this.className='button';"
                               onmouseover="this.className='button_over';"/>
                               &nbsp;<input id="btnCancel" class="button" type="button" value="Cancel" 
                                onclick = "javascript:PUNLA_Maint.rollback_record();" onmouseout="this.className='button';"
                               onmouseover="this.className='button_over';"/>
                           </td>
                      </tr>
                    </table>
                             <input id="txtHiddenSeqID"  type="hidden" style="width: 2%" />&nbsp;
                    </td>
                             
              </tr>
         
              <tr>
                   <td colspan="2">
                <center>
                           <div style="text-align: center">
                                <table id="Table1" >
                                    <tr>
                                        <td class="cell_caption" style="width: 143px; height: 23px">
                                                Filter By:&nbsp;
                                        </td>
                                        <td align="left" style="width: 305px; height: 24px;" colspan="4">
                                            <select id="ddFilterBy" style="width: 100%" onchange="javascript:execute_ajaxgridview_command('ajx_search$' + this.value)">
                                                <option selected="selected"></option>
                                            </select>
                                        </td>
                                    </tr>
                                </table>
                           </div>
                           <div style="overflow:auto;width:714px; height: 225px;"> 
                                 <div style="text-align: center" id="PUNLAMaintGrid">
                                </div>
                            </div>
                           <br />
                   </center>     
                   </td>
                  
              </tr>    
                                             
       </table>
 
    
     
    </div>
    </form>
</body>
</html>
