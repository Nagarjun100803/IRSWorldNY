<%@ Page Language="C#" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="ajaxgridview.aspx.cs" Inherits="IRSWorld.ajaxgridview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        &nbsp;<asp:GridView ID="grd_ajax" runat="server" OnRowDataBound="grd_ajax_rowdata_bound" Font-Bold="False" CellPadding="0">
        </asp:GridView>        
    </form>
</body>
</html>
