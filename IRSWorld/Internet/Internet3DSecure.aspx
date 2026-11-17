<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Internet3DSecure.aspx.cs" Inherits="IRSWorld.Internet.Internet3DSecure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function load3DSecure()
        {
            if (hasQueryStrings(window))
            {
                if (getQueryString(window, 'enets') == '1')
                {
                    document.getElementById('message').value = IRSWorld.Internet.Internet3DSecure.getENETS_REQUEST().value;//window.opener.document.form.lbl3DSecure.value;
                    document.getElementById('form').action = IRSWorld.Internet.Internet3DSecure.getENETS_URL().value;//window.opener.document.form.lbl3DSecureURL.value;
                    document.form.submit();
                }
                else
                {
                    document.getElementById('div3DSecure').innerHTML = parent.document.getElementById('lbl3DSecure').value;
                    document.getElementById('form').action = parent.document.getElementById('lbl3DSecureURL').value;
                    document.form.submit();
                }
            }
            else
            {
                document.getElementById('div3DSecure').innerHTML = parent.document.getElementById('lbl3DSecure').value;
                document.getElementById('form').action = parent.document.getElementById('lbl3DSecureURL').value;
                document.form.submit();
            }
        }
    </script>
</head>
<body onload="javascript:load3DSecure();">
    <form id="form" runat="server" method="post">
    <input type="hidden" id="message" name="message" value="" />
    <div id="div3DSecure">
    </div>
    </form>
</body>
</html>