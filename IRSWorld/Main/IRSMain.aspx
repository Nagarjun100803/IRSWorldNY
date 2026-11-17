<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IRSMain.aspx.cs" Inherits="IRSWorld.Main.IRSMain" %>
<%@ Register Src="../Controls/PageHeader.ascx" TagName="page_header" TagPrefix="uc1" %>
<!DOCTYPE html> <!-- PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"> -->
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="hd" runat="server">
    <title>IRSWorld</title>
    <link href="../css/Default_min.css" rel="stylesheet" type="text/css" />
    <meta http-equiv="pragma" content="no-cache" />    
    <style type="text/css">
    </style>
    <script src="../js/commons_min.js" type="text/javascript"></script>
    <script src="../js/jsmenu_min.js" type="text/javascript"></script>
    <script src="../js/Main/jsMain_min.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body onload="main_loader();" onunload="close_session();" scroll="no" onresize="adjust_dimensions();" style="overflow:hidden;">
    <center>
    <form id="form1" runat="server">
    <div id=divHead style="width: 100%; vertical-align: top;" class="frame_body" align="center" >
        <table style="width: 100%" cellpadding=0 cellspacing=0>
            <tr>
                <td style="width: 100%" class="frame_body">
                    <div style="width: 100%">
                        <uc1:page_header id="Page_header1" runat="server">
                        </uc1:page_header>
                    </div>                
                </td>
            </tr>
            <tr>
                <td id="tdNavbar" style="width:100%; border-collapse: collapse; display: none;" class="navbar" align="left">
                    <!--<table id="tblMenuBar">
                    </table>-->
                </td>                            
            </tr>
        </table>
    </div>     
    <div id="divCenter" style="width:100%;height:500px;">
        <iframe id="frmCenter" name="center_frame" frameborder="0" scrolling="auto" allowtransparency="true" width="100%" height="100%" src="frmCenter.aspx" style="z-index: 0">
        </iframe>
    </div>
    <div id="div_frame_closer" style="width:66px; height:16px; position: absolute; z-index: 300; top: 5px; left:5px; display: none;">
        <table cellpadding=0 cellspacing=0 style="width:100%">
            <tr>
                <td>
                    <input type=button value="Close" style="width:100%;height:100%;background-color:Maroon;color:AliceBlue" onclick="javascript:main_close_frame();" />
                </td>
            </tr>
        </table>
    </div>
    <div id=divTail style="width:100%; height:20px;">
        <iframe id="frmFooter" name="footer_frame" frameborder="0" scrolling="no" allowtransparency="true" width="100%" height="100%" src="frmfooter.aspx">
        </iframe>
    </div>    
    </form>
    </center>
</body>
</html>
