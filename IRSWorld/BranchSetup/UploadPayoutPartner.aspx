<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadPayoutPartner.aspx.cs" Inherits="IRSWorld.BranchSetup.UploadPayoutPartner" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    
    <div style="margin-top:5%">
    <center>
    <table id="tblUpload" style="width:50%" class="table_border">
    <tr>
        <td style="width:25%" class="cell_caption">
            Time Zone:
        </td>
        <td style="width:75%" class="cell_desc" align=left>
            <asp:DropDownList ID="ddlTimeZone" runat="server" style="width:353px" CssClass="cell_desc">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td style="width:25%" class="cell_caption">
            Upload File:
            
        </td>
        <td style="width:75%" class="cell_desc" align=left>
            <asp:FileUpload ID="flUpload" runat="server" style="width:354px" CssClass="cell_desc" />
            
        </td>
        
    </tr>
    <tr>
        <td style="width:25%" colspan=2>
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan=2>
            <asp:Button ID="btnUpload" runat="server" Text="Upload" style="width:75px" OnClick="btnUpload_Click" /></td>
    </tr>

    </table>
    </center>
    </div>
    </form>
</body>
</html>
