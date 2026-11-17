<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Valid_IDs.aspx.cs" Inherits="IRSWorld.BranchSetup.Valid_IDs" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Valid ID</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A { margin-right: 12px }
    </style>

    <script src="../js/commons.js" type="text/javascript"></script> 
    <script src="../js/datagrid.js" type="text/javascript"></script> 
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_valid_id.js" type="text/javascript"></script>
    <script src="../js/jstree/dtree.js" type="text/javascript"></script>
    <script type="text/javascript">
	</script>
	<script type="text/jscript">	
	</script>
	<script type="text/jscript">
	    function close_session()
        {
            //alert("closed");
            if (is_closing())
            {
                IRSWorld.BranchSetup.Valid_IDs.close_session();
            }
            else
            {
                IRSWorld.BranchSetup.Valid_IDs.drop_irsUsers();
            }
        }
	
	</script>
	
	
</head>
<body onload="page_setup();" >
    <center>
    <form id="form1" runat="server">
    <div style="width:75%; height: 100%;"> 
        <table style="width:100% ;height:100%" cellpadding="0" cellspacing="0">
        
            <tr>
                <td style="width:100%; border-collapse:collapse; height: 274px;vertical-align:top;" class="content_height">
     <%--                               <td valign="top" style="width:17%; vertical-align:top; padding-top: 30px; height: 324px;"> <!-- border-right: solid 1px #5697c1;"> -->
                                    <table width="100%"  cellpadding="0" cellspacing="7px">
                                        <tr>
                                            <td id="td5" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;"> <!-- border-right:solid 1px #5697c1"> -->
                                                <a id="A1" href="javascript:set_visibility('Main');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                                    Go Back to Search</a>
                                            </td>
                                        </tr> 
                                                                                                 
                                        <tr>
                                            <td id="Td9" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;"> <!-- border-right:solid 1px #5697c1"> -->
                                                <a id="A5" href="javascript:spawn_errpage('Closed');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">Close</a>
                                            </td>
                                        </tr>                                                                                                                                       
                                    </table>
                                </td>  
               <td style="width: 80%; padding-top: 30px; height: 324px;">--%>
               
                    <div id="divSearch" style="width:100%; margin-top: 30px; margin-left:0; text-align:left; vertical-align:top;">
                        <table id="tblSearch" style="width:90%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:60%; vertical-align: bottom; text-align:left">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:60%; vertical-align:bottom; text-align:left">
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td align="left" valign="bottom" style="width:0%;height:24px; border-collapse: collapse"> 
                                                            <img src="../images/toe.GIF" /></td>
                                                        <td id="capSearch" align="left" class="table_head_dark" style="width:30%; height: 24px; border-collapse:collapse">
                                                            Valid ID</td>
                                                        <td align="left" valign="bottom" style="height: 24px; width: 70%;" class="bg_body"> 
                                                            <img align="absbottom" src="../images/rend.gif" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            
                            <tr>
                                <td style="width: 100%; background: azure">
                                    <table id="tblMain" style="width:100%;" class="table_border">
                                    <tr><td colspan="2" style="width:100%">&nbsp;</td></tr>
                                        <tr>
                                         <td style="width: 32%; height: 18px;" align="right" class="cell_desc" >
                                                Branch Code</td>
                                            <td style="width: 100%; height: 18px;" >
                                               
                                                <select id="ddlBranch" style="width: 87%">
                                                    <option selected="selected"></option>
                                                </select>
                                           
                                                
                                            </td>              
                                        
                                        </tr>
                                         <tr>
                                           <td>
                                           </td>
                                            <td style="width: 100%; height: 18px;">
                                             <input type="button" value="Search" onclick="validate_branch();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                                <!-- content starts here --><!-- content ends here --></td>
                                            
                                        </tr>
                                        <tr><td colspan="2" style="width:100%">&nbsp;</td></tr>  
                                    </table>
                                
                                    
                                </td>

                            </tr>
   
                        </table>
                             
                         <table id="tblValidID" style="width: auto;text-align:left; margin-left:0; margin-top:0;">
                                 <tr>
                                 </tr>
                                 <tr>
                                 <td id="tdValidID" style="width: auto; " class=table_border>
                                                    
                                 </td>
                                 
                                 </tr>
                            
                             </table>  
                    <!--<label align="right" class="cell_desc" onclick="set_visibility('Main');">Go back to main</label>-->
                    
                                    <table>
                    <tr>
                        <td>
                            <a id="btnBack" href="#" onclick="set_visibility('Main');" style="font-weight: bold;
                                font-size: 12px; font-style: italic; font-family: Arial;">Go back to search</a>
                        </td>
                    </tr>
                </table>
                    </div>

                </td>
            </tr>   
                        
                        
                         <tr>
                         <td style="width: 17%; height: 18px;" align="right" class="content_height" >
                         
                         </td>
                         </tr>
        </table>   

 
    </div>
    <div id="divExplore" style="width: 200px; height: 200px; border: solid 1px midnightblue; display: none; position: absolute; z-index: 10" class="ds_gray">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:185px; height: 15px; font: bold 12px Arial; color: Ivory" class="navbar" align="left">&nbsp;&nbsp;IRSWorld</td>
                <td style="width:10px" class="navbar">
                    <input type="button" value="x" onclick="close_explore();" style="width:15px; color: red" class="table_caption" />
                </td>
            </tr>
            <tr>
                <td style="width:100%; height:185px;" align="left" colspan="2">
                    <div id="divTree" style="width:100%; height:100%; overflow:auto" class="dtree">
                        <script type="text/javascript">
                            var fs = IRSWorld.BranchSetup.Valid_IDs.getFs();
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
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_parent"></form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BranchSetup/Valid_IDs.aspx" target="_parent"></form>
   
</body>
</html>
