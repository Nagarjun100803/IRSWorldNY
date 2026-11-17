<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HitViewer.aspx.cs" Inherits="IRSWorld.IRSFileLoader.HitViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Hit Viewer</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons_min.js"                type="text/javascript"></script>
    <script src="../js/js_generic_min.js"               type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js"           type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js"            type="text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"            type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js"           type="text/javascript"></script>     
    <script src="../js/FileLoader/js_hitviewer.js"      type="text/javascript"></script>
        
    <script type="text/javascript">
        
    var hit_viewer = new hit_viewer();
     var divmask;
        
    function close_detail()
    {
        hidepanel('divpresummary');
    }
    
    function hidepanel(divname)
    {
    document.getElementById(divname).style.display = "none";
    divmask.hide();
    }
    
    function showpanel(divname)
    {   
        var div = document.getElementById(divname);
        
        divmask = new maskpanel();
        divmask.show();
        
        document.getElementById(divname).style.display = "block";
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
    }
    
    function page_setup()
    {
        if (top != self)
        {
            top.frame_scroll("yes");
        }
    }
    
    function page_off()
    {
        if (top != self)
        {
            top.frame_scroll("no");
        }
    }
    
    function fixdim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmain');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
            }
        }
   
    </script>
</head>
<%--<body onload="javasript:hit_viewer.refresh_table(); hit_viewer.load_grid_viewer(); hit_viewer.getBranchCode(); hit_viewer.count_not_ConvertedCIF(); hit_viewer.dropdown_FileFormat();"--%>
<body onload="fixdim(); javasript:hit_viewer.load_grid_viewer(); hit_viewer.getBranchCode();hit_viewer.dropdown_FileFormat();" onresize="fixdim();">

    <form id="form1" runat="server">
        <div id="divPopProcess" style="width:400px; height:200px; background:White; z-index: 101; position: absolute; left: 0px; color:darkblue; padding: 10px; display: none; font-family: Arial;top: 361px;">
            <table style="width:100%; height:100%;">
                <tr>
                    <td>
                        <label id="lblLoadingMsg" style="text-align:center; color:Chocolate "></label>
                        <div id="divLoadingMsg">
                        </div>
                    </td>
                </tr>
            </table>
        </div>        
    <div id="divmain" style="padding-top: 30px;overflow:auto;width:100%;height:700px">
        <table>
            <tr>
                <td   style="width: 135px; vertical-align:top;">
                    <div id="divComplianceMenu" style="vertical-align: top">
                        <table style="width:100%; height:auto">
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="../IRSFileLoader/ComplianceViewer.aspx" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">Compliance Viewer</a></td></tr>
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="../IRSFileLoader/HitViewer.aspx" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">Hit Viewer</a></td></tr>
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a href="../IRSFileLoader/FEDACHViewer.aspx" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">FEDACH Viewer</a></td></tr>
                            <tr><td style="width:150px; height:18px; text-align: right; border-bottom: solid 1px #5697c1;"></td>
                            <%--<a href="javascript:page_load()" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial">TEST</a></td>--%></tr>
                        </table>
                    </div>              
                </td>
                <td style="width: 100px">
            <table cellpadding="0" cellspacing="0" style="width:30%">
            <tr>
                <td style="height: 43px"> 
                    <table cellpadding="0" cellspacing="0" >
                        <tr>
                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                <img src="../images/toe.GIF" /></td>    
                            <td class="table_head_dark" style="width: 50%; height: 24px">
                                Compliance </td>
                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>   
                      </tr>
                    </table>    
                </td>
                    
                </tr>
                <tr>
                    <td colspan="2" style="width: 700px">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 80%; border-bottom: #336699 3px solid">
                            <tr>
                                <td style="width: 100%; height: 21px; background-color: azure;">

                                    
                                    <div id="divPercentageScore" style="background-color: azure;display:none">
                                        <table id="Table2" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:800px">
                                            <tr>
                                                <td class="cell_caption" colspan="1" style=" text-align: center; width: 25%">
                                                    Percentage Score:</td>
                                                <td style="text-align: left;width: 25%" colspan="3"><select id="ddlPercentageScore" style="width: 20%">
                                                                                                        <option value="80">80</option>
                                                                                                        <option value="85">85</option>
                                                                                                        <option value="90">90</option>
                                                                                                        <option value="95">95</option>
                                                                                                        <option value="100">100</option>
                                                                                                      </select>
                                                </td>
                                                <td style="width: 10%; height: 24px;">
                                                    <input id="btnPSave" class="button" onclick="javascript:hit_viewer.update_pscore_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Save" style="width: 100px"/>
                                                </td>
                                                <td style="width: 10%; height: 24px;">
                                                    <input id="btnPCancel" class="button" onclick="javascript:hit_viewer.rollback_pscore_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 100px"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    
                                 
                                   <div id="HitViewer" style="background-color: azure">
                                        <table id="Table1" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:800px">
                                            <tr>
                                                    <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                    height: 26px; text-align: left">
                                                        Hit Viewer</td>
                                            </tr>
                                            <tr>
                                                    <td class="cell_caption" colspan="1" style=" text-align: center; width: 25%">
                                                        FullName Cif:</td>
                                                     <td style="text-align: center;width: 25%" colspan="1.5"><input id="txtCif" type="text" style="" readonly="readOnly"/></td>
                                                     <td class="cell_caption" colspan="1" style=" text-align: center;width: 25%">
                                                         FullName Unified:</td>
                                                     <td style="text-align: center;width: 25%" colspan="1.5"><input id="txtUni" type="text" style="" readonly="readOnly"/></td>    
                                            
                                            </tr>
                                            <tr>
                                                <td style="height: 20px; text-align: center "colspan="5">
                                                <div id="hitgrid">
                                                    HIT GRID HERE
                                                </div>
                                                </td>
                                            </tr>                                            
                                            
                                            <tr>
                                                <td style="text-align: center; height: 51px;"colspan="5">
                                                    <div style="text-align: center">    
                                                    <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:40%; text-align: center" cellpadding="0" cellspacing="5" id="tblClear" >
                                                        <tr>
                                                            <td style="width: 10%; height: 19px;" class="cell_caption" colspan="">
                                                                Cleared:&nbsp;</td>
                                                            <td style="width: 10%; height: 19px" colspan="">
                                                                <input id="chkCleared" type="checkbox"></td>
                                                        </tr>
                                                    </table>
                                                    </div>
                                                </td>
                                            </tr>
                                           <tr>
                                                <td style="text-align: center" colspan="5">
                                                    <table id="tblbuttons2" cellspacing="5" style="width:40%; text-align: center">
                                                            <tr>
                                                                <td style="width: 10%; height: 24px;">
                                                                 <input id="btnEdit" class="button" onclick="javascript:hit_viewer.update_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Edit" style="width: 100px" disabled="disabled"/></td>
                                                                 <td style="width: 10%; height: 24px;">
                                                                     <input id="btnSave" class="button" onclick="javascript:hit_viewer.commit_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Save" style="width: 100px" disabled="disabled"/></td>
                                                                 <td style="width: 10%; height: 24px;">
                                                                     <input id="btnCancel" class="button" onclick="javascript:hit_viewer.rollback_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 100px" disabled="disabled"/></td>
                                                            </tr>
                                                    </table><input id="txtHiddenSeqID2" type="hidden" />
                                                </td>  
                                            </tr>
                                            <tr>
                                                    <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                    height: 26px; text-align: left">
                                                        Search Hitlist</td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" colspan="1" style=" text-align: center; width: 25%">
                                                    File Format:</td>
                                                 <td style="text-align: center;width: 25%" colspan="1.5">
                                                    <select id="ddFileFormat" style="width: 100%">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </td>
                                                 <td class="cell_caption" colspan="1" style=" text-align: center;width: 25%">
                                                     Exact match:</td>
                                                 <td style="text-align: center;width: 25%" colspan="1.5">
                                                    <input id="chkExactmatch" type="checkbox" value="off">
                                                </td>   
                                            
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; height: 57px;" colspan="5">
                                                    <table id="Table3" cellspacing="5" style="width:40%; text-align: center">
                                                            <tr>
                                                                <td style="width: 10%; height: 24px;">
                                                                 <input id="btnSearchHitlist" class="button" onclick="javascript:hit_viewer.searchHitlist()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search" style="width: 100px"/></td>                                                                 
                                                            </tr>
                                                    </table><input id="Hidden1" type="hidden" />
                                                </td>  
                                            </tr>
                                            <%--
                                            <tr>
                                                <td  style="text-align:right;" colspan="5">
                                                    <table style="width:100%">
                                                        <tr>
                                                            <td style="text-align: center"colspan="5">
                                                                <div style="text-align: center">    
                                                                <table style="border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; text-align: center" cellpadding="0" cellspacing="5" id="Table2" width="40%">
                                                                    <tr>
                                                                        <td class="table_caption"        style="height:24px; color:Black; text-align:center" colspan="5">Number/s of CIF without Phonetics Value</td>

                                                                        
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="text-align:right; height: 23px;">
                                                                            <input id="txtCIFNoToConvert" type="text" style="width:90%; text-align:center " onkeypress="javascript:hit_viewer.check_alpha();"/>
                                                                        </td> 
                                                                        <td style="width:10%">OF</td> 
                                                                        <td style="text-align:right; height: 23px;">
                                                                            <input id="txtTotalNotConvertedCIF" type="text" style="width:90%; text-align:center" readonly="readOnly"/>
                                                                        </td>  
                                                                    </tr>
                                                                    <tr>                                                   
                                                                        <td colspan ="3">
                                                                            <input id="btnConvertPhonetics" class="button" onclick="javascript:hit_viewer.convert_phonetics()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Make CIF Phonetics" style="width: 50%"/>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>--%>                                   
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
        </div>
    </form>

</body>
</html>
