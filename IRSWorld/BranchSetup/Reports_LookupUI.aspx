<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports_LookupUI.aspx.cs" Inherits="IRSWorld.BranchSetup.Reports_LookupUI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Reports Lookup UI</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <link href="../js/jstree/dtree.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .dragme 
    {
        cursor: move ;	
        font-size: 13px;
	    font-weight: bold;
	    color: aliceblue;
	    font-family: Arial;
	    background-color: #5697c1;
	    height: 26px;
	    border-top-width: 1px;
	    border-left-width: 1px;
	    border-left-color: #9999ff;
	    border-bottom-width: 1px;
	    border-bottom-color: #9999ff;
	    border-top-color: #9999ff;
	    border-right-width: 1px;
	    border-right-color: #9999ff;
	    vertical-align: middle;
    }
    </style>
    <script src="../js/commons.js" type="text/javascript"></script> 
    <script src="../js/datagrid.js" type="text/javascript"></script> 
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>      
    <script src="../js/BranchSetup/js_BranchLookup.js"></script>
    <script src="../js/jstree/dtree.js" type="text/javascript"></script>
    <script src="../js/Admin/jsModules.js" type="text/javascript"></script>
    <script src="../js/Reports/js_Reports_LookupUI.js" language="javascript">    </script>
    
</head>
<body onload="javascript:this.init_reports_maintenance();" topmargin ="0" leftmargin="0">    
<center>
    <form id="form1" runat="server">
    <div id="divreplookup" style="height:27%" class=content_height>
        <table style="width:400; height:300" cellpadding="30" cellspacing="0">
            <tr>    
                <td align="center" valign="top" style="width:100; border:collapse;" class="content_height">
                <div >
                    <table cellspacing="0" cellpadding="0" style="width: 736px">
                            <tr>
                                <!-- by chr -->
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table style="width:100%" cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img src="../images/toe.GIF" /></td>
                                        <td id="Td1" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            Reports Lookup</td>
                                        <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                            
                            </tr>
                            <tr>
                                <td colspan="3" style=" background: azure; height: 120px; vertical-align:top;">
                                <table style=" width:736px; border:solid 3px #336699;">
                                <tr>
                                        <td nowrap style="width: 18%; height: 21px;" align="left" class="cell_caption">
                                            &nbsp; Report Group</td>
                                        <td nowrap align="left" style="height: 21px"><div id="divbranchcode">
                                            <select id="ddlRepMain"   onchange="javascript:display_group()" name="ddlRepMain" style=" height: 21px;">
                                                <option selected="selected"></option>
                                               
                                            </select>
                                            <asp:HiddenField ID="popup_type" runat="server" />
                                            <asp:HiddenField ID="hfsessionid" runat="server" />
                                            &nbsp;
                                             </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td  colspan="2" style="height: 21px" id="tdIRSReports">
                                        <!-- <div  id="tdIRSReports" style="overflow:auto ;width:100;">
                                        </div> -->
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
    <div id="divcpopup" style="z-index:101;position:absolute ;padding:10px;display:none; font-family: Arial; width: auto;height: auto; background-color: transparent; left: 344px; top: 88px;">
    
    <table cellpadding="0" cellspacing="0"  style="background: azure;" >
        <tr>
            <td id="boxholder" align="left" class='dragme'   style="font: bold 12px Arial;  color: ivory; height: 15px; width: 94px;" >
                &nbsp; Parameters </td>
            <td align="right" class="table_caption" >
                <input class="table_caption" onmouseover="this.style.cursor='hand';" onclick="close_popup();" style="width: 12px; color: red; text-align:right ;"
                    type="button" value="x" />
            </td>
        </tr>
        <tr>
            <td  align="left" colspan="2" valign="top">
            <div id="tdCReports">
            </div>
                
            </td>
        </tr>
    </table>
    
    </div>

    </div> 
<div id="divExplore" style="overflow: scroll ;   width: 448px; height: 304px; border: solid 1px midnightblue; display: none; position: absolute; z-index: 101; left: 320px; top: 112px;" class="ds_gray">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:185px; height: 15px; font: bold 12px Arial; color: Ivory" class="navbar" align="left"  >&nbsp;&nbsp;IRSWorld</td>
                <td class="navbar" style="width: 4px" align="right">
                    <input type="button" value="x" onclick="close_explore();" style="width:15px; color: red" class="table_caption" />
                </td>
            </tr>
            <tr>
                <td id="tdreppath" class="navbar" style="width:auto; height: 15px; font: bold 12px Arial; color: Ivory" colspan="2" align="left">
                    &nbsp;Path: </td>
            </tr>
            <tr>
                <td style="width:100%; height:100%" align="left" colspan="2">
                    <div id="divTree" style="width:100%; height:100%; overflow:auto" class="dtree">
                        <script type="text/javascript">
                            var fs = IRSWorld.BranchSetup.Reports_LookupUI.getFs();
                            if (fs.error != null)
                            {
                                document.write('Unable to get IRSWorld File Structure.');
                            }
                            else
                            {
                                root_path = fs.value[0];
                                var lsdir = fs.value[1];
                                d3 = new dTree('d3');
                                for (var k = 0; k < lsdir.length; k++)
                                {
                                    if (lsdir[k][3] == 'F')
                                    {
                                        d3.add(lsdir[k][0], lsdir[k][1], lsdir[k][2], 'javascript:fselect();');
                                    }
                                    else
                                    {
                                        d3.add(lsdir[k][0], lsdir[k][1], lsdir[k][2]);
                                    }
                                }
                                document.write(d3);  
                            } 
                            fs = null;                        
                        </script>
                    </div>
                </td>
            </tr>
        </table>
    </div>        
       
    </form>
    </center>
</body>
</html>
