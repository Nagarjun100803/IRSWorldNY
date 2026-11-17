<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxGridNew.aspx.cs" Inherits="IRSWorld.AjaxGridNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    &nbsp;<asp:GridView ID="grd_ajax" runat="server" OnRowDataBound="grid_ajax_rowdata_bound" Font-Bold="False" AllowSorting="True" BackColor="Lavender" BorderColor="DarkBlue" BorderStyle="Solid" BorderWidth="2px" CellPadding="0" Font-Names="Arial" Font-Size="Small" ForeColor="MediumBlue">
        <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" />
        <SelectedRowStyle ForeColor="Black" />
        <HeaderStyle BackColor="#C0C0FF" BorderColor="Blue" BorderStyle="Solid" Font-Bold="True" />
        <EditRowStyle BorderStyle="Solid" BorderWidth="1px" />
        <AlternatingRowStyle BackColor="AliceBlue" BorderColor="Blue" BorderStyle="Solid"
            HorizontalAlign="Center" VerticalAlign="Middle" />
        </asp:GridView>
    </form>
</body>
</html>
