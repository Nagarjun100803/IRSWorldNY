<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTellering.aspx.cs" Inherits="IRSWorld.Remittance.RemTellering" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Teller</title>
    
    
    
    
    <script src="../js/jsCommons.js"            type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"        type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"        type="text/javascript"></script>
    <script src="../js/jsgridselector.js"       type="text/javascript"></script>        
    <script src="../js/CIF/js_cif_master.js"    type="text/javascript"></script>
    <script src="../js/CIF/js_cif_ids.js"       type="text/javascript"></script>
    <script src="../js/CIF/js_cif_main.js"      type="text/javascript"></script>
    <script src="../js/Remittance/jsremteller.js" type="text/javascript"></script>


</head>
<body>

    <script language = "javascript">
        var remtellering = new rem_teller();
    </script>

    <form id="frmteller" runat="server">
    
        <a href = "#" onclick ="javascript:remtellering.tester();">CLICK ME</a>
        <div id="divdiv">replace me<input id="Button1" type="button" value="button" /></div>
    </form>
</body>
</html>
