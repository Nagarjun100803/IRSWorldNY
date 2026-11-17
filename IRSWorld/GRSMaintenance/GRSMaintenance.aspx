<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSMaintenance.aspx.cs" Inherits="IRSWorld.GRSMaintenance.GRSMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>GRS Maintenance</title>
            <link href="../css/Default.css" rel="stylesheet" type="text/css" />
            <script src="../js/js_sidebar.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
 
       <div id="divMain" style="width:100%; height:100%; padding-top:30px;">
            <table style="width:100%; height:100%" cellpadding="0" cellspacing="5">  
          
                <tr>  
                 <td id="tdsidebar" align="left" valign="top" style="width: 20%">
                             <div style="width:100%">
                                <script type="text/javascript">
                                    var sideA = new Array
                                    (
                                        new Array(1, 0, 'GRS Splitter', 'javascript:window.open("../GRSMaintenance/GRSSplitter.aspx","iframeSplit");'),
                                        new Array(2, 0, 'PUNLA Maintenance', 'javascript:window.open("../GRSMaintenance/PUNLAMaintenance.aspx", "iframeSplit");'),
                                        new Array(3, 0, 'I.Batch Maintenance ', 'javascript:window.open("../GRSMaintenance/IBatchMaintenance.aspx", "iframeSplit");')
                                    );
                                    
                                    var sidemenuA = new js_sidebar('sidemenuA', sideA);
                                    document.write(sidemenuA.toString());
                                </script>
                            </div>
                 </td>  
                    <td align="center" valign="middle" style="width:80%; border-collapse:collapse">
                        <table id="tblHelp" width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                    <img src="../images/toe.gif" /></td>
                                <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse"> GRS Maintenance</td>            
                                <td align="left" valign="bottom" style="height: 24px" class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style=" border:solid 3px #336699;" class="table_body">
                                    <table style="width:100%; background-color:#FFFFFF; " cellspacing="3">
                                        <tr>
                                            <td class="header" width="75%" valign="top" align="left" style="height: 500px">
                                                <iframe name="iframeSplit" id="iframeSplit" src="../GRSMaintenance/GRSSplitter.aspx" height="100%" width="100%" frameborder="0" style="margin-bottom:0; margin-left:0; margin-right:0; margin-top:0; background-color:#EDF4F8;"></iframe>                                               
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
