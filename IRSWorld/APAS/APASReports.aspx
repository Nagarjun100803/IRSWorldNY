<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APASReports.aspx.cs" Inherits="IRSWorld.APAS.APASReports" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRS Approval Window</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A { margin-right: 12px }
    </style>

    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/APAS/jsAPASReports.js" type="text/javascript"></script>
    <script src="../js/jstree/dtree.js" type="text/javascript"></script>

	
	
</head>
<body onload="page_setup();" >
    <center>
    <form id="form1" runat="server">
    <div id="divmain" style="width:100%; height: 100%;vertical-align: top; margin-top: 30px;"> 
        <table height="100%" cellpadding="0" cellspacing="5" style="width: 95%">
        
            <tr>
                <td valign="middle" style="width:100%; border-collapse:collapse; vertical-align:top" class="content_height">
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
               
                    <div id="divSearch" style="width:100%; display: block;">
                        <table id="tblSearch" cellspacing="0" cellpadding="0" style="width: 90%">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                    <img src="../images/toe.GIF" /></td>
                                <td id="capSearch" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                    Report Viewer</td>
                                <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background: azure">
                                    <table id="tblMain" style="width:100%;" class="table_border">
                                       
                                        <tr>
                                            <td style="width: 32%; height: 18px;" align="right" class="cell_desc" >
                                                Select Report</td>
                                            <td style="width: 100%; height: 18px;">
                                                <select id="ddlTables" style="width: 100%" onchange="javascript:create_dynamic_fields();">
                                                    <option selected="selected"></option>
                                                </select>
                                                <!-- content starts here --><!-- content ends here --></td>
                 
                                        </tr>
                                        <tr>
                                            <td id="exp_cell" style="width: 32%; height: 18px;" align="right" class="cell_desc" >
                                                Export To</td>
                                            <td id="exp_data" style="width: 100%; height: 18px;">
                                                <select id="ddlExport" style="width: 100%">
                                                    <option selected="selected"></option>
                                                </select>
                                                <!-- content starts here --><!-- content ends here --></td>
                 
                                        </tr>
                                        <tr>
                                            <td id="tdGrid" colspan=2 style="width: 32%; height: 18px;" align="right" class="cell_desc" >
                                                <center>
                                                
                                                   <table id="tblReportParam" width=50% style="margin-top:2%">
                                                           <tr>
                                                                <td style="height: 17px"></td>
                                                           
                                                           </tr>
                                                       
                                           
                                                    </table>
                                                  
                                           
                                                </center>   
                                                <table id="tblButtons" cellpadding="0" cellspacing="6" style="width: 100%">
                                                    <tr>
                                                        <td align="center" colspan="3" style="width: 100%">
                                                            <table cellpadding="0" cellspacing="6" style="width: 50%">
                                                                <tr>
                                                                    <td id="tdHold">
                                                                        <input id="btnPrint" onclick="load_report();" type="button" value="Preview Report" style="width: 50%" />
                                                                    </td>
                                                                 
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>

                                           </td>
                                           
                                        
                 
                                        </tr>
                                     
                                        <tr>
                                           
                                            <td style="height: 21px">
                                                
                                            </td>
                                          
                                        </tr>
                                       
        
                                          
                                        
                                                                         
                                    </table>
          
                    
                                </td>

                            </tr>
   
                        </table>

                    </div>

                </td>
            </tr>   
                        
                        
           <tr>
                <td style="width: 17%; height: 18px;" align="right" >
                </td>
           </tr>
           
        </table>   

 
    </div>
    
    
    </form>
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_parent"></form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BranchSetup/Accounts.aspx" target="_parent"></form>
    </center>
</body>
</html>
