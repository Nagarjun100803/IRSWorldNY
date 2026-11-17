<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HTMLPrint.aspx.cs" Inherits="IRSWorld.Reports.HTMLReport.HTMLPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>HTML Print</title>
    <script type="text/javascript" src="../../js/Reports/HTMLReport/Reports.js"> </script>    
   
</head>
<body onload="javascript:HTMLRep.load_report();" style="height:100%;width:100%;background-color:White;">
    <form id="form1" runat="server">    
    <asp:HiddenField ID="hidQueryString" runat="server" />
    <asp:HiddenField ID="hidBranchcode" runat="server" />
    <asp:HiddenField ID="hidURL" runat="server" />
    </form>
    <p style="font-size:12pt;color:blue;text-align:center;font-family:Arial;font-weight:bold;">Please click the close button to proceed.</p>
</body>
</html>
