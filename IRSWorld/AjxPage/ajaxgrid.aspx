<%@ Page Language="C#" AutoEventWireup="true"  EnableEventValidation = "false" CodeBehind="ajaxgrid.aspx.cs" Inherits="IRSWorld.ajaxgrid" %>
<html>
<head>
<title>AUTO GRID</title>
</head>
<body>
 <form id="form1" runat="server">    
     <asp:GridView ID="grd_ajax" runat="server" OnRowDataBound="grd_ajax_rowdata_bound" PageSize="15"></asp:GridView>     
 </form>
</body>
</html>
