<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Accounts.aspx.cs" Inherits="IRSWorld.BranchSetup.Accounts" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRS Currency Group</title>    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        A { margin-right: 12px }
    </style>

    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>       
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_accounts_min.js" type="text/javascript"></script>
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
                Accounts.close_session();
            }
            else
            {
                Accounts.drop_irsUsers();
            }
        }
	
	</script>
	
	
</head>
<body id="theBody" onload="page_setup();">
    <center>
    <form id="form1" runat="server">
    <div id="divBody" style="width:100%; height: 100%;vertical-align: top; margin-top: 30px;"> 
        <table cellpadding="0" cellspacing="5" style="width: 95%">
        
            <tr>
                <td valign="middle" style="width:100%; border-collapse:collapse; vertical-align:top" class="content_height">
               
                    <div id="divSearch" style="width:80%;">
                        <table id="tblSearch" style="width:100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:20%; vertical-align:bottom;" align="left">
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%;height:24px; border-collapse: collapse"> 
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capSearch" align="left" class="table_head_dark" style="width:20%; height: 24px; border-collapse:collapse">
                                                Accounts</td>
                                            <td align="left" valign="bottom" style="height: 24px; width: 80%;" class="bg_body"> 
                                                <img align="absbottom" src="../images/rend.gif" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; background: azure">
                                    <table id="tblMain" style="width:100%;" class="table_border">
                                        <tr>
                                            <td style="width: 32%; height: 18px;" align="right" class="cell_desc" >
                                                Branch Code</td>
                                            <td style="width: 100%; height: 18px;" >
                                               
                                                <select id="ddlBranch" style="width: 87%" onchange="new getAccountCurrency().Load_AccountCurrency();">
                                                    <option selected="selected"> </option>
                                                </select>
                                           
                                                
                                            </td>                                          
                                        </tr>
                                        <tr>
                                            <td style="width: 32%; height: 18px;" align="right" class="cell_desc" >
                                                Account
                                                Currency</td>
                                            <td style="width: 100%; height: 18px;">
                                                <select id="ddlAcctCurrency" style="width: 87%" >
                                                    <option selected="selected"></option>
                                                </select>
                                                <!-- content starts here --><!-- content ends here --></td>
                 
                                        </tr>

                                        <tr>
                                           <td>
                                           </td>
                                            <td style="width: 100%; height: 18px;">
                                             <input type="button" value="Search" onclick="populate_record_IBFT();populate_record_NSVS();populate_record_GL();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';"  />
                                                <!-- content starts here --><!-- content ends here --></td>
                 
                                        </tr>  
                                        
                                                                         
                                    </table>
          
                    
                                </td>

                            </tr>
   
                        </table>
                             
                         <table id="tblIBFT" style="width: auto">
                             <tr>
                             </tr>
                             <tr>
                                <td id="tdIBFT" style="width: auto" class=table_border>
                                                                                
                             </td>
                             
                             </tr>
 
                          </table>
                          <br /><br />
                          <table id="tblNSVS" style="width: auto">
                             <tr>
                             
                             </tr>
                             <tr>
                                <td id="tdNSVS" style="width: auto" class=table_border>
                                                                                
                             </td>
                             
                             </tr>
 
                          </table>
                             <br /><br />
                            <table id = "tblGL" style="width:auto">
                             
                             <tr>
                             </tr>
                             <tr>
                                <td id="tdGL" style="width:auto" class=table_border>
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
    <form name="irs_login" id="irs_login" method="post" action="../Main/IRSMain.aspx?load=../Admin/Login.aspx" target="_parent"></form>
    <form name="irs_loader" id="irs_loader" method="post" action="../Main/IRSMain.aspx?load=../BranchSetup/Accounts.aspx" target="_parent"></form>
    </center>
</body>
</html>
