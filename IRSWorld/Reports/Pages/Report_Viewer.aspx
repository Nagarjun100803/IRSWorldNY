<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report_Viewer.aspx.cs" Inherits="IRSWorld.Reports.Pages.Report_Viewer" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Report Generator</title>
    <link href="../../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../../js/jsCommons.js" type="text/javascript"></script>    
    <script src="../../js/Reports/js_ReportsTemplate.js" language="javascript"> 

</script>
    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />
    
 
    
   <script language="javascript" type="text/javascript">
   
   function create_window_html()
   {
        //    var shtml = '<html><head>';
        //     shtml +=    '<script language="javascript>"'    
        //     shtml +=    'function PrintWindow()\n'
        //     shtml +=    '{    \n'
        //     shtml +=    'if (navigator.appName == "Microsoft Internet Explorer") { \n'
        //     shtml +=    'var PrintCommand = ''<OBJECT ID="PrintCommandObject" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></OBJECT>';' \n';         
        //     shtml +=    'document.body.insertAdjacentHTML("beforeEnd", PrintCommand); \n'; 
        //     shtml +=    'PrintCommandObject.ExecWB(6, 2); \n'; 
        //     shtml +=    'PrintCommandObject.outerHTML = ""; \n'; 
        //     shtml +=    'else { window.print();} \n'; 
        //     shtml +=    '} \n';
        //     shtml +=    '<//script><//head> \n';
        //     shtml +=    '<body id="reportctrl" onload="javascript:load_dochtml()"></body></html> \n'    
   }
   function PrintWindow()
   {    
       // if (navigator.appName == "Microsoft Internet Explorer") { 
       //  var PrintCommand = '<OBJECT ID="PrintCommandObject" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></OBJECT>';    
     
       //     document.body.insertAdjacentHTML('beforeEnd', PrintCommand); 
       //     PrintCommandObject.ExecWB(6, 2); 
       //     PrintCommandObject.outerHTML = ""; 
       //  } 
       // else { window.print();} 
   } 
        function load_complete()
        {
            
        //            try
        //            {
        //                var autoprint = document.getElementById("hdautoprint").value;
        //                if (autoprint == "true")
        //                {
        //                    printWindow();
        //                }
        //                else {window.print();}
        //            }
        //            catch(e){ window.print();}    
        }
    
    var gAutoPrint = true; // Flag for whether or not to automatically call the print function 
    function printSpecial() { 
    if (document.getElementById != null) { 
    var html = '<HTML>\n<HEAD>\n'; 
    if (document.getElementsByTagName != null) { 
    var headTags = document.getElementsByTagName("head"); 
    if (headTags.length > 0) html += headTags[0].innerHTML; 
    } 
    if (gAutoPrint) { 
    if (navigator.appName == "Microsoft Internet Explorer") { 
    html += '\n</HEAD>\n<'
    html += 'BODY onLoad="PrintCommandObject.ExecWB(6, 2);window.close();">\n'; 
    } 
    else { 
    html += '\n</HEAD>\n<BODY>\n'; 
    } } 
    else { 
    html += '\n</HEAD>\n<BODY>\n'; 
    } 

    var printReadyElem = document.getElementById("printReady"); 
    if (printReadyElem != null) { 
    html += printReadyElem.innerHTML; 
    } 
    else { 
    alert("Could not find the printReady section in the HTML"); 
    return; 
    }
    if (gAutoPrint) { 
    if (navigator.appName == "Microsoft Internet Explorer") { 
    html += '<OBJECT ID="PrintCommandObject" WIDTH=0 HEIGHT=0 '
    html += 'CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></OBJECT>\n</BODY>\n</HTML>'; } 
    else { 
    html += '\n</BODY>\n</HTML>'; 
    } } 
    else { 
    html += '\n</BODY>\n</HTML>'; 
    } 
    var printWin = window.open("","printSpecial"); 
    printWin.document.open(); 
    printWin.document.write(html); 
    printWin.document.close();
    if (gAutoPrint) { 
    if (navigator.appName != "Microsoft Internet Explorer") { 
        printWin.print();     
    }}} 
    else { 
        alert("Sorry, the print ready feature is only available in modern browsers.");
        }
    }     
    
    function printWindow()
    { 
        if (document.getElementById != null) { 
        var html = '<HTML>\n<HEAD>\n'; 
        if (document.getElementsByTagName != null) { 
        var headTags = document.getElementsByTagName("head"); 
        if (headTags.length > 0) html += headTags[0].innerHTML; 
        }   
    if (gAutoPrint) { 
    if (navigator.appName == "Microsoft Internet Explorer") { 
        html += '\n</HEAD>\n<'
        html += 'BODY onLoad="javascript:window.print();window.close();">\n'; 
        } 
    else { 
        html += '\n</HEAD>\n<BODY>\n'; 
        } 
    } 
    else { 
    html += '\n</HEAD>\n<BODY>\n'; 
    } 
    var printReadyElem = document.getElementById("printReady"); 
    if (printReadyElem != null) { 
    html = printReadyElem.innerHTML; 
    } 
    else { 
    alert("Could not find the printReady section in the HTML"); 
    return; 
    }    
    var printWin = window.open("","printSpecial"); 
    printWin.document.open(); 
    printWin.document.write(html);    
    printWin.document.close();  
    
     }     
    }
    function handle_eod_error(msg)
    {
        alert("THERE WAS A PROBLEM WHILE GETTING THE EOD REPORT")
        
        alert(document.getElementById("hdEODmsg").value);

        var branchcode = document.getElementById("hdBranchCode").value;            
        var userid = document.getElementById("hdUserID").value;            
        if (confirm("CONTINUE WITH FORCE PUSH?"))
        {
            IRSWorld.Reports.Pages.Report_Viewer.exec_forcepush(userid,branchcode,force_push_callback)
        }
      
    }
    function force_push_callback(result)
    {
        if (result.value != null)
        {
            var rs =  result.value.toString();
            if (rs.indexOf("ERROR")== -1)
            {
                ajax_show_status(false);
                alert("FORCE PUSH SUCCESSFUL");
                return;
            }
        }
        ajax_show_status(false);
    }
   </script>
 

    <link href="/aspnet_client/System_Web/2_0_50727/CrystalReportWebFormViewer3/css/default.css"
        rel="stylesheet" type="text/css" />

 
</head>

<body  id="printReady" topmargin ="0" leftmargin="0" onload="javascript:load_complete()">

<form id="form1" runat="server" >

<table border="0" cellpadding="0" cellspacing="0" width="100%" >
<tr>
<td>
    &nbsp;
        </td>
        </tr>
</table>
    <asp:HiddenField ID="hdautoprint" runat="server" />
    &nbsp;&nbsp;
    <asp:HiddenField ID="hdHasError" runat="server" />
    <asp:HiddenField ID="hdEODmsg" runat="server" />
    <asp:HiddenField ID="hdBranchCode" runat="server" />
   <asp:HiddenField ID="hdUserID" runat="server" />
   <asp:HiddenField ID="hdTranxDate" runat="server" />
</form>

</body>
</html>
