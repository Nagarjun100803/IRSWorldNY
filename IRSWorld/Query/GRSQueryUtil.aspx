<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSQueryUtil.aspx.cs" Inherits="IRSWorld.Query.GRSQueryUtil" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GRSQueryUtil</title>
</head>
<body>
    <center>
    <form id="form1" runat="server">
    <div style="width:90%;">
        <table width="100%">
            <tr>
                <td align="left" style="width: 100%">
                    <asp:FileUpload ID="FileUpload1" runat="server" Visible="False" Width="635px" Font-Names="Arial" ForeColor="Navy" /><br />                
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 100%">
                    <asp:Label ID="Label1" runat="server" Text="Label" Width="466px" Visible="False" Font-Bold="True" Font-Names="Arial" ForeColor="Navy"></asp:Label>&nbsp;<br />
                </td>
            </tr>
            <tr>
                <td align="left" style="width: 100%">
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Save" Visible="False" Font-Bold="True" ForeColor="Green" Width="90px" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    </center>
</body>
</html>
