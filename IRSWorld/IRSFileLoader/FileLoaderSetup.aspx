<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileLoaderSetup.aspx.cs" Inherits="IRSWorld.IRSFileLoader.FileLoaderSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>File Loader Setup</title>
    <link href="../css/Default.css" rel ="stylesheet" type="text/css" />    
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/FileLoader/js_irsfileloader.js" type="text/javascript"></script>
    <script type="text/javascript" >
    
    var fileloader = new irs_fileloader(); 
    
    function fixdim()
    {
        if (top.document.getElementById('divCenter'))
        {
            var div = document.getElementById('divFileLoaderSetup');
            if (div)
            {
                div.style.height = (parseInt(top.document.getElementById('divCenter').offsetHeight) - 30) + 'px';
            }
            div = document.getElementById('divCreateTempTable');
            if (div)
            {
                div.style.height = (parseInt(top.document.getElementById('divCenter').offsetHeight) - 30) + 'px';
            }            
        }
    }   

    </script>    
</head>
<body onload="fixdim(); javascript:fileloader.load_file_loader_grid_viewer();fileloader.load_all_dropdownlist();javascript:fileloader.show_div('divFileLoaderSetup');" onresize="fixdim();">
    <form id="form1" runat="server">
        <div style="padding-top: 30px;">
            <table id ="fileloadersetup" style="width:100%" cellpadding ="0" cellspacing ="5">
                <tr>
                    <td style="vertical-align:top; width:15%;">
                        <div id = "divFileLoaderMenu" style= "display:block;">
                             <table style="width:100%"  cellpadding="0" cellspacing="5">
                                <tr id="trFileSetup">
                                    <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                        <a id="Master" href="javascript:fileloader.show_div('divFileLoaderSetup');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">File Loader Setup</a>
                                    </td>
                                </tr>
                                <tr id="trTempTableSetup">
                                    <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                        <a id="CIFClass" href="javascript:fileloader.show_div('divCreateTempTable');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Create Table</a>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td align="center" style="vertical-align:top; width:81%;">
                        <div id= "divFileLoaderSetup" style= "display:none; overflow:auto; height:530px;" >
                            <table id="tblLoaderFields" cellpadding="0" cellspacing="0" style="width:95%; overflow:auto">
                                <tr>
                                <td style="width:60%; vertical-align:bottom" align=left>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>                          
                                    <td align="left" valign="bottom" style="border-collapse:collapse; height: 100%; vertical-align:bottom">
                                        <img src="../images/toe.GIF" /></td>
                                    <td class="table_head_dark" style="width:40%; height: 100%;">FileUploader Setup </td>
                                    <td align="left" valign="bottom" style="width:60%; height:100%;"> 
                                        <img align="absBottom" src="../images/rend.gif"/></td>
                                </tr> 
                                </table>
                                </td>
                                </tr>
                                <tr>
                                    <td> <!--colspan = "3">-->
                                        <table id="tblFileLoaderSetup" style="width:100%; border:solid 3px #336699">
                                            <tr>
                                                <td class="table_caption" colspan="2" style="width:388px; text-align:left; text-indent:5px; color:Black; height:26px;" valign="top">File Setup</td>
                                            </tr>   
                                            <tr>
                                                <td style="width:50%; height: auto; vertical-align:top">
                                                    <table id = "tblFileUploadType" style="width:100%; padding-top:auto; ">
                                                        <tr> <!--filetype row-->
                                                            <td class="cell_caption" style="width: 45%; height:24px">File Type :&nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <select id="ddFileUploadType" style ="width:100%;height:24px" onchange="javascript:fileloader.reloadfileformat()"></select>
                                                            </td>
                                                        </tr>
                                                        <tr> <!--fileformat row-->
                                                            <td class="cell_caption" style="width: 45%; height:24px">File Format :&nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <select id="ddFileFormat" style ="width:100%;height:24px" onchange="javascript:fileloader.check_table()"></select>
                                                            </td>
                                                        </tr>
                                                        <tr> <!-- Nooffiles row-->
                                                            <td class="cell_caption" style="width:45%; height:24px">No of files : &nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <select id="ddNoofFiles" style="width:100%;height:24px">
                                                                    <option value="" style ="text-align:left"></option>
                                                                    <option value="1" style="text-align:left"> 1 </option>
                                                                    <option value="2" style="text-align:left"> 2 </option>
                                                                    <option value="3" style="text-align:left"> 3 </option>
                                                                </select>
                                                            </td>
                                                        </tr>                                                           
                                                        <tr> <!--filesequenceno row-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">File Sequence No. :&nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <input id="txtFileSequenceNo" style="width:98%; text-align: left" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                            </td>
                                                        </tr>
                                                        <tr> <!--delimitter row-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Delimiter :&nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <input id="txtDelimiter" style="width:98%; text-align:left" onkeyup=""/>
                                                            </td>
                                                        </tr>     
                                                        <tr> <!--SkipLine row-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Start at Line : &nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <input id="txtStartAtLine" style="width: 98%; text-align:left" onkeypress ="javascript:fileloader.check_numeric()"/><!--  />-->
                                                            </td>
                                                        </tr>    
                                                        <tr> <!--RecordField row-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Record Field : &nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <input id="txtRecordField" style="width: 98%; text-align:left" onkeypress="javascript:fileloader.check_alpha()"/><!--  />-->
                                                            </td>
                                                        </tr> 
                                                        <tr> <!--recordlength row-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Record Length : &nbsp</td>
                                                            <td align="left" style="width: 55%; height: 24px">
                                                                <input id="txtRecordLen" style="width: 98%; text-align:left" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                            </td>
                                                        </tr>                                            
                                                        <tr> <!--Split char-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Record Character Splitter : &nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <input id="txtSplitChar" style="width: 98%; text-align:left"/>
                                                            </td>
                                                        </tr>   
                                                        <tr> <!--Split index-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Splitted Value Position : &nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <input id="txtSplitIndex" style="width: 98%; text-align:left" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                            </td>
                                                        </tr>
                                                        <tr> <!--Invalid characters-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Invalid Character/s : &nbsp</td>
                                                            <td align="left" style="width:55%; height:24px">
                                                                <input id="txtInvalidCharacters" style="width: 98%; text-align:left" />
                                                            </td>
                                                        </tr>
                                                        <tr> <!-- datecreated row-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Date Created : &nbsp</td>
                                                            <td align="left" style="width: 55%; height: 24px">
                                                                <input id="txtDateCreated" style="width: 98%; text-align:left" readonly="readOnly"/>
                                                            </td>
                                                        </tr>
                                                        <tr> <!--datemodified row-->
                                                            <td class="cell_caption" style="width: 45%;height:24px">Date Modified : &nbsp</td>
                                                            <td align="left" style="width: 55%; height: 24px">
                                                                <input id="txtDateLastModified" style="width: 98%; text-align:left" readonly="readOnly"/>
                                                            </td>                                                            
                                                        </tr>    
                                                        <tr>
                                                            <td class="cell_caption"    style="width:15%; height:22px" colspan="2">Note: Use (~) tilde character as a 
                                                                <br />separator for invalid characters 
                                                                <br />For example (!~@~#)
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width:50%; height:auto; vertical-align:top;">
                                                    <table style="width:100%; height: auto">                         
                                                        <tr> <!--recordposition row-->
                                                            <td colspan ="2" style="width: 100%;">                                        
                                                                <div id="divRecordPosition" style=";height:auto;">
                                                                    <table id="tblRecordPosition" style="width: 100%; height:auto">                           
                                                                        <tr> 
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Record Position : &nbsp</td>
                                                                            <td align="left" style="width:55%; height:24px">
                                                                                <input id="txtRecordPosition" style="width: 98%;text-align:left" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>  <!--destination table row-->
                                                            <td colspan ="2" style="width: 100%;">                                        
                                                                <div id="divDestinationTable" style="border-style:hidden; display:none;height:auto;">
                                                                    <table id="tblDestinationTable" style="width: 100%;height:auto;">
                                                                       <tr>
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Destination Table : &nbsp</td>
                                                                            <td align="left" style="width:55%;height:24px">
                                                                                <select id="ddDestinationTable" style="width:100%;height:24px" onchange="javascript:fileloader.get_Destination_Fields()"></select>
                                                                            </td>
                                                                        </tr>    
                                                                    </table>
                                                                </div>
                                                            </td>      
                                                        </tr>             
                                                        <tr>  <!--destination table field row-->
                                                            <td colspan ="2" style="width: 100%;">                                        
                                                                <div id="divDestinationTableField" style="border-style:hidden;height:auto;">
                                                                    <table id="tblDestinationTableField" style="width: 100%;height:auto;">
                                                                       <tr>
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Destination Field : &nbsp</td>
                                                                            <td align="left" style="width:55%;height:24px">
                                                                                <select id="ddDestinationTableField" style="width:100%;height:24px"></select>
                                                                            </td>
                                                                        </tr>    
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr> 
                                                        <tr> <!--temp table field row-->
                                                            <td colspan ="2" style="width: 100%;">                                        
                                                                <div id="divTempTableField" style="display:none;;height:auto;">
                                                                    <table id="tblTempTableField" style="width: 100%;height:auto;">
                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Temp Table Field : &nbsp</td>
                                                                            <td align="left" style="width:55%;height:24px">
                                                                                <select id="ddtempTableField" style="width:100%;height:24px"></select>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr> 
                                                        <tr>  <!--CIFClass table row-->
                                                            <td colspan ="2" style="width: 100%;">                                        
                                                                <div id="divCIFClass" style="border-style:hidden; display:none;height:auto;">
                                                                    <table id="tblCIFClass" style="width: 100%;height:auto;">
                                                                       <tr>
                                                                            <td class="cell_caption" style="width: 45%;height:24px">CIF Class : &nbsp</td>
                                                                            <td align="left" style="width:55%;height:24px">
                                                                                <select id="ddCIFClass" style="width:100%;height:24px"></select>
                                                                            </td>
                                                                        </tr>    
                                                                    </table>
                                                                </div>
                                                            </td>      
                                                        </tr>     
                                                        <tr> <!--Conditions row-->
                                                            <td colspan ="2" style="width: 100%;">                                        
                                                                <div id="divConditions" style="display:none;height:auto;">
                                                                    <table id="tblConditions" style="width: 100%;height:auto;">                           
                                                                        <tr>  <!--where1 row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">1st Condition : &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">
                                                                                <select id="ddCondition1" style="width:100%;height:24px"></select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr> <!--1st Condition Position row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Condition Position : &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">
                                                                                <input id="txtConditionPosition1" style="width: 98%; text-align:left" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>  <!--1st Condition Operator row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Condition Operator: &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">                                                                
                                                                                <select id="ddOperator1" style="width:100%;height:24px" >
                                                                                    <option value="" style ="text-align:left"></option>
                                                                                    <option value="AND" style="text-align:left"> AND </option>
                                                                                    <option value="OR" style="text-align:left"> OR </option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>  <!--where2 row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">2nd Condition : &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">
                                                                                <select id="ddCondition2" style="width:100%;height:24px"></select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr> <!--2nd Condition Position row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Condition Position : &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">
                                                                                <input id="txtConditionPosition2" style="width: 98%; text-align:left" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>  <!--2nd Condition Operator row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Condition Operator: &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">
                                                                                <select id="ddOperator2" style="width:100%;height:24px" >
                                                                                    <option value="" style ="text-align:left"></option>
                                                                                    <option value="AND" style="text-align:left"> AND </option>
                                                                                    <option value="OR" style="text-align:left"> OR </option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>  <!--where3 row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">3rd Condition : &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">
                                                                                <select id="ddCondition3" style="width:100%"></select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr> <!--3rd Condition Position row-->
                                                                            <td class="cell_caption" style="width: 45%;height:24px">Condition Position : &nbsp</td>
                                                                            <td align="left" style ="width:55%; height:24px">
                                                                                <input id="txtConditionPosition3" style="width: 98%; text-align:left" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                            </td>
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
                                <tr> <!-- cmdbuttons rows-->
                                    <td colspan="3">
                                        <table id = "tblbuttons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%; " >
                                                                     
                                        <!--table id= "tblbuttons"-->
                                            <tr>
                                                <td style="width:100px; height:17px">
                                                    <input id="btnAdd" type="button" class="button" value="Add" onclick ="javascript:fileloader.insert_record();"/>
                                                </td>
                                                <td style="width:100px; height:17px;">
                                                    <input id="btnEdit" type="button" class="button" value="Edit" onclick = "javascript:fileloader.update_record();"/>
                                                </td>
                                                <td style="width:100px; height:17px;">
                                                    <input id="btnDelete" type="button" class="button" value="Delete"/>
                                                </td>          
                                                <td style="width:100px; height:17px;">
                                                    <input id="btnSave" type="button" class="button" value="Save" onclick ="javascript:fileloader.commit_record();"/>
                                                </td>                                  
                                                <td style="width:100px; height:17px;">
                                                    <input id="btnCancel" type="button" class="button" value="Cancel" onclick ="javascript:fileloader.rollback_record()"/>
                                                </td>                                            
                                                <td style="width:100px; height:17px;">
                                                    <input id="btnClose" type="button" class="button" value="Close"/>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <input id="txtHiddenSeqID" type="hidden" />
                                    </td>
                                </tr>                                            
                                <tr> <!--filterby row-->
                                    <td colspan="3">
                                        <table style="width:100%; ">
                                            <tr>
                                                <td class="cell_caption" style="height: 26px">Filter By : &nbsp</td>
                                                <td align="left" style="width:60%; height:26px">
                                                    <select id="ddLoaderFilter" style="width:100%" onchange ="javascript:execute_ajaxgridview_command('ajx_search$' + this.value)"></select>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td colspan="3" style="height: 44px">
                                        <div id="DefinitionMapperGrid" style="text-align:center; width:100%" ></div>                                   
                                    </td>
                                </tr>
                            </table>             
                        </div>
                        <div id="divCreateTempTable" style="display:none; overflow:auto; height:600px"> 
                            <table id= "tblTempTable" cellpadding="0" cellspacing="0" style="width:470px">
                                <tr>
                                    <td style="width:60%; vertical-align:bottom" align=left>
                                        <table style="width:100%;" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="bottom" style="width:0%; border-collapse:collapse; height: 100%; vertical-align:bottom"><img src="../images/toe.GIF" /></td>                            
                                                <td class="table_head_dark" style="width:40%; height: 100%;">File Uploader Setup</td>
                                                <td align="left" valign="bottom" style="width:60%; height:100%;"><img align="absBottom" src="../images/rend.gif"/></td>                                                                                                                             
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr> <!--setup table-->
                                    <td> <!--colspan="3">-->
                                        <table id="Table2" style="width:100%; border:solid 3px #336699">
                                            <tr>
                                                <td class="table_caption" colspan="2" style="width:388px; text-align:left; text-indent:5px; color:Black; height:26px;" valign="top">Table Setup</td>
                                            </tr>
                                            <tr> <!--filetype row-->
                                                <td class="cell_caption" style="width: 137px; height:24px">File Type :&nbsp</td>
                                                <td align="left" style="width:70%; height:24px">
                                                    <select id="ddFileUploadTypeTempTable" style ="width:100%" onchange="javascript:fileloader.reload_parent_fileformat()"></select>
                                                </td>
                                            </tr>
                                            <tr> <!--fileformat row-->
                                                <td class="cell_caption" style="width: 137px; height:24px">File Format :&nbsp</td>
                                                <td align="left" style="width:70%; height:24px">
                                                    <select id="ddFileFormatTempTable" style ="width:100%" onchange="javascript:fileloader.check_temp_table()"></select>
                                                </td>
                                            </tr>
                                            <tr> <!-- Nooffiles row-->
                                                <td class="cell_caption" style="width:137px; height:24px">No of files : &nbsp</td>
                                                <td align="left" style="width:70%; height:24px">
                                                    <select id="ddNoofFilesTempTable" style="width:100%" onchange="javascript:fileloader.show_no_of_controls()" >
                                                        <option value="" style ="text-align:left"></option>
                                                        <option value="1" style="text-align:left"> 1 </option>
                                                        <option value="2" style="text-align:left"> 2 </option>
                                                        <option value="3" style="text-align:left"> 3 </option>
                                                    </select>
                                                </td>
                                            </tr>                                                                                      
                                            <tr> <!---multiplefiles row--->
                                                <td colspan="2">
                                                    <div id="multiplefiles" style="display:block">
                                                        <table id="tbmultiplefiles" style ="width:100%" >
                                                            <tr> <!--div file1-5 row -->
                                                                <td colspan = "2" >
                                                                    <div id="fileone" style="display:none">
                                                                        <table id = "tblFileName1" style="width:100%">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width:35%">Filename 1. :&nbsp</td>
                                                                                <td align="left" style="width:65%; height:24px">
                                                                                    <input id="txtFileName1" style="width:98%; text-align: left" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>                                                
                                                                    <div id="filetwo" style="display:none">
                                                                         <table id = "tblFileName2"  style="width:100%">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width:35%">Filename 2. :&nbsp</td>
                                                                                <td align="left" style="width:65%; height:24px">
                                                                                    <input id="txtFileName2" style="width:98%; text-align: left" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>                                                
                                                                    <div id="filethree" style="display:none">
                                                                         <table id = "tblFileName3" style="width:100%">
                                                                            <tr>
                                                                                <td class="cell_caption" style="width:35%">Filename 3. :&nbsp</td>
                                                                                <td align="left" style="width:65%; height:24px">
                                                                                    <input id="txtFileName3" style="width:98%; text-align: left" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>                                 
                                                            <tr> <!--temp table name row -->
                                                                <td class="cell_caption" style ="width:35%">Table Name :&nbsp</td>
                                                                <td align="left" style="width:65%; height:24px">
                                                                    <input id="txtTempTableName" style="width:98%; text-align: left" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                </td>
                                                            </tr>                             
                                                            <tr> <!--no of fields row-->
                                                                <td class="cell_caption" style="width:35%">No of fields : &nbsp</td>
                                                                <td align="left" style="width:65%; height:24px">
                                                                    <select id="ddNoofFields" style="width:100%" onchange="javascript:fileloader.hide_elements_fields(this.value)">
                                                                        <option value="" style ="text-align:left"></option>
                                                                        <option value="1" style="text-align:left"> 1 </option>
                                                                        <option value="2" style="text-align:left"> 2 </option>
                                                                        <option value="3" style="text-align:left"> 3 </option>
                                                                        <option value="4" style="text-align:left"> 4 </option>
                                                                        <option value="5" style="text-align:left"> 5 </option>
                                                                        <option value="6" style="text-align:left"> 6 </option>
                                                                        <option value="7" style="text-align:left"> 7 </option>
                                                                        <option value="8" style="text-align:left"> 8 </option>
                                                                        <option value="9" style="text-align:left"> 9 </option>
                                                                        <option value="10" style="text-align:left"> 10 </option>
                                                                        <option value="11" style="text-align:left"> 11 </option>
                                                                        <option value="12" style="text-align:left"> 12 </option>
                                                                        <option value="13" style="text-align:left"> 13 </option>
                                                                        <option value="14" style="text-align:left"> 14 </option>
                                                                        <option value="15" style="text-align:left"> 15 </option>
                                                                        <option value="16" style="text-align:left"> 16 </option>
                                                                        <option value="17" style="text-align:left"> 17 </option>
                                                                        <option value="18" style="text-align:left"> 18 </option>
                                                                        <option value="19" style="text-align:left"> 19 </option>
                                                                        <option value="20" style="text-align:left"> 20 </option>
                                                                        <option value="21" style="text-align:left"> 21 </option>
                                                                        <option value="22" style="text-align:left"> 22 </option>
                                                                        <option value="23" style="text-align:left"> 23 </option>
                                                                        <option value="24" style="text-align:left"> 24 </option>
                                                                        <option value="25" style="text-align:left"> 25 </option>
                                                                        <option value="26" style="text-align:left"> 26 </option>
                                                                        <option value="27" style="text-align:left"> 27 </option>
                                                                        <option value="28" style="text-align:left"> 28 </option>
                                                                        <option value="29" style="text-align:left"> 29 </option>
                                                                        <option value="30" style="text-align:left"> 30 </option>
                                                                        <option value="31" style="text-align:left"> 31 </option>
                                                                        <option value="32" style="text-align:left"> 32 </option>
                                                                        <option value="33" style="text-align:left"> 33 </option>
                                                                        <option value="34" style="text-align:left"> 34 </option>
                                                                        <option value="35" style="text-align:left"> 35 </option>
                                                                        <option value="36" style="text-align:left"> 36 </option>
                                                                        <option value="37" style="text-align:left"> 37 </option>
                                                                        <option value="38" style="text-align:left"> 38 </option>
                                                                        <option value="39" style="text-align:left"> 39 </option>
                                                                        <option value="40" style="text-align:left"> 40 </option>
                                                                    </select>
                                                                </td>
                                                            </tr>  
                                                            <tr> <!-- div1-40 row-->
                                                                <td colspan ="2" >
                                                                    <div id="multiplefields" style="display:none">    
                                                                        <table id="tblmultiplefields" style="width:100%">  
                                                                            <tr>
                                                                                <td colspan = "3">                                                                                
                                                                                    <div id="DivFieldNameTypeLength" style="display:none">
                                                                                        <table style="width:100%">
                                                                                            <tr>
                                                                                                <td class="table_caption" style="width:35%">FieldName&nbsp</td>
                                                                                                <td class="table_caption" style="width:35%">Data type&nbsp</td>
                                                                                                <td class="table_caption" style="width:30%">Data length&nbsp</td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>   
                                                                                    <div id="Div1" style="display:none">
                                                                                        <table id= "tbl1" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName1" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType1" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength1" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div2" style="display:none">
                                                                                         <table id= "tbl2" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName2" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType2" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength2" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div3" style="display:none">
                                                                                         <table id= "tbl3" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:26px">
                                                                                                    <input id="txtFieldName3" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%; height: 26px;">
                                                                                                    <select id="ddDataType3" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:26px">
                                                                                                    <input id="txtDataLength3" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div4" style="display:none">
                                                                                         <table id= "tbl4" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName4" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType4" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength4" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div5" style="display:none">
                                                                                         <table id= "tbl5" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName5" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType5" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength5" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div6" style="display:none">
                                                                                        <table id= "tbl6" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName6" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType6" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength6" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div7" style="display:none">
                                                                                         <table id= "tbl7" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName7" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType7" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength7" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div8" style="display:none">
                                                                                         <table id= "tbl8" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName8" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType8" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength8" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div9" style="display:none">
                                                                                         <table id= "tbl9" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName9" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType9" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength9" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div10" style="display:none">
                                                                                         <table id= "tbl10" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName10" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType10" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength10" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div11" style="display:none">
                                                                                        <table id= "tbl11" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName11" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType11" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength11" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div12" style="display:none">
                                                                                         <table id= "tbl12" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName12" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType12" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength12" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div13" style="display:none">
                                                                                         <table id= "tbl13" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName13" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType13" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength13" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div14" style="display:none">
                                                                                         <table id= "tbl14" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName14" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType14" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength14" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div15" style="display:none">
                                                                                         <table id= "tbl15" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName15" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType15" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength15" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div16" style="display:none">
                                                                                        <table id= "tbl16" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName16" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType16" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength16" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div17" style="display:none">
                                                                                         <table id= "tbl17" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName17" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType17" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength17" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div18" style="display:none">
                                                                                         <table id= "tbl18" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName18" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType18" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength18" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div19" style="display:none">
                                                                                         <table id= "tbl19" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName19" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType19" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength19" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div20" style="display:none">
                                                                                         <table id= "tbl20" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName20" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType20" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength20" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div21" style="display:none">
                                                                                        <table id= "tbl21" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName21" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType21" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength21" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div22" style="display:none">
                                                                                         <table id= "tbl22" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName22" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType22" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength22" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div23" style="display:none">
                                                                                         <table id= "tbl23" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName23" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType23" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength23" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div24" style="display:none">
                                                                                         <table id= "tbl24" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName24" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType24" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength24" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div25" style="display:none">
                                                                                         <table id= "tbl25" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName25" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType25" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength25" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div26" style="display:none">
                                                                                        <table id= "tbl26" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName26" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType26" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength26" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div27" style="display:none">
                                                                                         <table id= "tbl27" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName27" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType27" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength27" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                                <!--<td class="cell_caption" style="width:40%">FieldName 27. :&nbsp</td>
                                                                                                <td colspan="2" align="left" style="width:60%; height:24px">
                                                                                                    <input id="txtFieldName27" style="width:98%; text-align: left" />
                                                                                                </td>-->
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div28" style="display:none">
                                                                                         <table id= "tbl28" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName28" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType28" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength28" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                                <!--<td class="cell_caption" style="width:40%">FieldName 28. :&nbsp</td>
                                                                                                <td colspan="2" align="left" style="width:60%; height:24px">
                                                                                                    <input id="txtFieldName28" style="width:98%; text-align: left" />
                                                                                                </td>-->
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div29" style="display:none">
                                                                                         <table id= "tbl29" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName29" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType29" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength29" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                                <!--<td class="cell_caption" style="width:40%">FieldName 29. :&nbsp</td>
                                                                                                <td colspan="2" align="left" style="width:60%; height:24px">
                                                                                                    <input id="txtFieldName29" style="width:98%; text-align: left" />
                                                                                                </td>-->
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div30" style="display:none">
                                                                                         <table id= "tbl30" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName30" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType30" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength30" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                                <!--<td class="cell_caption" style="width:40%">FieldName 30. :&nbsp</td>
                                                                                                <td colspan="2" align="left" style="width:60%; height:24px">
                                                                                                    <input id="txtFieldName30" style="width:98%; text-align: left" />
                                                                                                </td>-->
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div31" style="display:none">
                                                                                        <table id= "tbl31" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName31" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType31" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength31" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div32" style="display:none">
                                                                                         <table id= "tbl32" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName32" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType32" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength32" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div33" style="display:none">
                                                                                         <table id= "tbl33" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName33" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType33" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength33" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div34" style="display:none">
                                                                                         <table id= "tbl34" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName34" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType34" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength34" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div35" style="display:none">
                                                                                         <table id= "tbl35" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName35" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType35" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength35" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div36" style="display:none">
                                                                                        <table id= "tbl36" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName36" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType36" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength36" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div37" style="display:none">
                                                                                         <table id= "tbl37" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName37" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType37" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength37" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>                                                
                                                                                    <div id="Div38" style="display:none">
                                                                                         <table id= "tbl38" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName38" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()" />
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType38" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength38" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div39" style="display:none">
                                                                                         <table id= "tbl39" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName39" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType39" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength39" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                    <div id="Div40" style="display:none">
                                                                                         <table id= "tbl40" style="width:100%">
                                                                                            <tr>
                                                                                                <td align="left" style="width:35%; height:24px">
                                                                                                    <input id="txtFieldName40" style=" text-align: left; width: 98%;" onkeypress="javascript:fileloader.check_alpha()"/>
                                                                                                </td>
                                                                                                <td style="width:35%">
                                                                                                    <select id="ddDataType40" style="width: 97%" onchange="javascript:fileloader.disable_datalength(this.id)">
                                                                                                        <option value="" style ="text-align:left"></option>
                                                                                                        <option value="numeric" style="text-align:left"> numeric </option>
                                                                                                        <option value="alphanumeric" style="text-align:left"> alphanumeric </option>
                                                                                                        <option value="money" style="text-align:left"> money </option>
                                                                                                        <option value="date" style="text-align:left"> date </option>
                                                                                                    </select>
                                                                                                </td>
                                                                                                <td align="left" style="width:30%; height:24px">
                                                                                                    <input id="txtDataLength40" style=" text-align: left; width: 98%;" onkeypress ="javascript:fileloader.check_numeric()"/>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>                                                                     
                                                                         </table>                                                                       
                                                                    </div>
                                                                </td>
                                                            </tr>                                          
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>  
                                            <tr> <!--cmd button saves temp table-->
                                                <td colspan="3">
                                                    <table id = "tbltemptablebuttons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%; " >
                                        
                                                    <!--table id= "tbltemptablebuttons"-->
                                                        <tr>
                                                            <td style="width:100px; height:17px;">
                                                                <input id="btnAddTempTable" type="button" class="button" value="Add" onclick = "javascript:fileloader.add_create_table_controls();" disabled="disabled"/>
                                                            </td>
                                                            <td style="width:100px; height:17px;">
                                                                <input id="btnEditTempTable" type="button" class="button" value="Edit" onclick = "javascript:fileloader.edit_create_table_controls();" disabled="disabled"/>
                                                            </td>
                                                            <td style="width:100px; height:17px;">
                                                                <input id="btnSaveTempTable" type="button" class="button" value="Save" onclick ="javascript:fileloader.save_temp_table();" disabled="disabled"/> <!--onclick ="javascript:fileloader.add_temptable_lookupvalues();"/-->
                                                            </td>
                                                            <td style="width:100px; height:17px;">
                                                                <input id="btnCancelTempTable" type="button" class="button" value="Cancel" onclick = "javascript:fileloader.cancel_create_table_controls();" disabled="disabled"/>
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
                    </td>
                </tr> 
            </table> 
        </div>
    </form>
</body>
</html>