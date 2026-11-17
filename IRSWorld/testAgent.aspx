<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testAgent.aspx.cs" Inherits="IRSWorld.testAgent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <strong>IRSWORLD AGENT</strong><br />
        <br />
        <strong>Agent for crediting</strong><br />
        <asp:Button ID="Start" runat="server" OnClick="Button2_Click" Text="Start" />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Stop" />
        <br />
        <br />
        <strong>Agent for scheduled job<br />
        </strong>
        <asp:TextBox ID="TextBox2" runat="server" Width="103px">05/28/2010</asp:TextBox>Transaction
        Date:<br />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="test job" />
        changed from scheduled to manual<br />
        <br />
        <strong>MT103 Extract</strong><br />
        <asp:TextBox ID="TextBox1" runat="server" Width="103px">05/28/2010</asp:TextBox>
        Transaction Date:<br />
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click1" Text="extract MT103" /><br />
        <br />
        <strong>SMS</strong><br />
        <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Send SMS" /><br />
        <br />
        <strong>Email</strong><br />
        <asp:Button ID="Button6" runat="server" OnClick="Button6_Click1" Text="Send Email" /><br />
        <br />
        <strong>3D Agent<br />
            <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="Run 3D Agent" /><br />
            <br />
            Other Credit<br />
            <asp:Button ID="Button7" runat="server" OnClick="Button7_Click" Text="Go Credit" /></strong></div>
    </form>
</body>
</html>
