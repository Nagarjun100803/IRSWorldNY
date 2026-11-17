<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFooter.aspx.cs" Inherits="IRSWorld.Main.frmFooter" %>
<%@ Register Src="../Controls/PageFooter.ascx" TagName="page_footer" TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Frame Footer Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script type="text/javascript">    
    </script>    
</head>
<body class="frame_body">
<center>
    <form id="form1" runat="server">
    <div style="width:99%; vertical-align:bottom;" class="frame_body">
        <uc2:page_footer id="Page_footer1" runat="server">
        </uc2:page_footer>
    </div>
    </form>
</center>
</body>
</html>
