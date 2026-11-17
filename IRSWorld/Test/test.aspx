<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="IRSWorld.Test.test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>IRS CIF Main Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    
    <script type="text/javascript">
    
        function test_closer()
        {
            var si = parent.document.getElementById('txtCifId').innerText;
            if (si == null || si == '') si = '0';
            var i = parseFloat(si);
            
            i++;
            tst_name = 'Lastname Firstname mi';
            var ra = new Array(2);
            ra[0] = i;
            ra[1] = tst_name;
            ra[2] = 1;      // cif classL
            ra[3] = 'LDN'   // London
            window.returnValue = ra;
            
            close_frame_dialog();
        }
        
        function over_close()
        {
            var rv = window.returnValue;
            if (rv.success == false)
            {
                alert(rv.message);
                return;
            }
            alert (" success: " + rv.success + "\n message: " + rv.message + "\n location: " + rv.location + "\n Role: " + rv.role);
        }
        
        function over_test()
        {
            var btn = document.getElementById('btnover');
            var xy = calcpos(btn);
            show_frame_dialog('../Admin/Override.aspx?ChargeId=3', 'over_close', '505px', '205px', 0, xy.top + btn.offsetHeight, xy.left + btn.offsetWidth); //100, 100); 
        }
        
        function page_setup()
        {
            if (top == self)
            {
                window.open('../Main/IRSMain.aspx?load=../Test/test.aspx', '_top');
            }
        }
        
        function settext_test()
        {
            setText(document.getElementById('spantest'), '');
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <!--
    <div>
        ** cif main.** &nbsp; &nbsp;
        <input type="button" id="btntest" value="close" onclick="test_closer();" style="width: 8%" />
    </div>
    -->
    <div>
        ** over test.** &nbsp; &nbsp;
        <input type="button" id="btnover" value="over" onclick="settext_test();" style="width: 8%" />
        <input type="text" id=spantest />
    </div>    
</form>
</body>
</html>

