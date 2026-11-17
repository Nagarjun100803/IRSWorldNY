<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherChannelsViewer.aspx.cs" Inherits="IRSWorld.IRSFileLoader.OtherChannelsViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link href="../css/Default.css" rel ="stylesheet" type="text/css" />    
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/ComplianceOtherChannels/js_compliance_other_channels.js" type="text/javascript"></script>
    <script type="text/javascript">
            var OtherChannelsViewer = new OtherChannelsViewer();
            
            function load_main()
            {
                if (top == self)
                {
                    document.irs_loader.submit();
                }
            }
            
    </script>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
