<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrencyGroup.aspx.cs" Inherits="IRSWorld.BranchSetup.CurrencyGroup" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRS Currency Group</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A { margin-right: 12px }
    </style>

    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_currencygroup.js" type="text/javascript"></script>
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
                CurrencyGroup.close_session();
            }
            else
            {
                CurrencyGroup.drop_irsUsers();
            }
        }
	
	</script>
	
	
</head>
<body onload="page_setup();">
    <center>
    <form id="form1" runat="server">
    
    <div style="height: 100%;vertical-align: top; margin-top: 30px;"> 
        <table cellpadding="0" cellspacing="5" style="width:80%;" >
            <tr>
                <td  style="width:100%; border-collapse:collapse; vertical-align:top" align=left>
                    <div id="divSearch" style="width:99%; display: block;">
                        <table id="tblSearch" style="width:75%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width: 60%; vertical-align: bottom; text-align:left">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width:0%;height:24px; border-collapse: collapse; vertical-align: bottom;"> 
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capSearch" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse; vertical-align:bottom;">
                                                Currency Group</td>
                                            <td align="left" style="height: 24px; width: 60%; vertical-align:bottom;" class="bg_body"> 
                                                <img align="absbottom" src="../images/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; background: azure; vertical-align:top" align=center>
                                    <table id="tblMain" style="width:100%;" class="table_border">
                                        <tr>
                                            <td style="width: 25%; height: 18px;" align="right" class="cell_desc" >
                                                Branch Code</td>
                                            <td style="width: 75%; height: 18px;" align=left >
                                               
                                                <select id="ddlBranch" style="width: 87%">
                                                    <option selected="selected"></option>
                                                </select>
                                           
                                                
                                            </td>                                          
                                        </tr>
                                        <tr>
                                            <td style="width: 25%; height: 18px;" align="right" class="cell_desc" >
                                                Currency Group<!-- content starts here --><!-- content ends here --></td>
                                            <td style="width: 75%; height: 18px;" align=left>
                                                <select id="ddCurrGrp" style="width: 87%">
                                                    <option selected="selected"></option>
                                                </select>
                                                <!-- content starts here --><!-- content ends here --></td>
                 
                                        </tr>

                                        <tr>
                                           <td>
                                           </td>
                                            <td style="width: 100%; height: 18px;">
                                             <input type="button" value="Search" onclick="validate_dropdown();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="Button1" />
                                                <!-- content starts here --><!-- content ends here --></td>
                 
                                        </tr>  
                                        
                                                                         
                                    </table>
          
                    
                                </td>

                            </tr>
   
                        </table>
                         <table id="tblCurrencyGroup" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                           border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid; width:47%">
                                 <tr>
                                 </tr>
                                 <tr>
                                 <td id="tdCurrencyGroup" style="width: 100%; text-align:center; margin:0 auto;">
                                 </td>
                                 
                                 </tr>
               
                             </table>
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
                         <td style="width: 17%; height: 18px;" align="right" >
                         </td>
                         </tr>
        </table>   

 
    </div>
    
    </form>
    </center>
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_parent"></form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BranchSetup/CurrencyGroup.aspx" target="_parent"></form>
    
</body>
</html>
