<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ARM.aspx.cs" Inherits="IRSWorld.Main.ARM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Automated Remittance Machine</title>
    <script src="../js/Main/ARM2.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/Main/jsCalendar.js" type="text/javascript"></script>
    <link href="../css/ARM2.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var arm_page = new arm_page();
    </script>
    <script language="vbscript" type="text/vbscript">
        Sub Print()
               OLECMDID_PRINT = 6
               OLECMDEXECOPT_DONTPROMPTUSER = 2
               OLECMDEXECOPT_PROMPTUSER = 1
               call WB.ExecWB(OLECMDID_PRINT, OLECMDEXECOPT_DONTPROMPTUSER, 1)
        End Sub
        document.write "<object ID='WB' WIDTH=0 HEIGHT=0 CLASSID='CLSID:8856F961-340A-11D0-A96B-00C04FD705A2'></object>"
    </script>
</head>
<body onload="arm_page.initialize()">
    <form id="form1" runat="server">
    <div>
        <!-- *** HEADER STARTS HERE *** -->
        <div id="header" style="background-color:White;">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="33%" align="left"><img src="../images/PNBLOGO.jpg" /></td>
                    <td width="33%" align="center">
                        <span id="spnClock" style="vertical-align:middle;font-family:Arial;font-size:smaller;"></span><br />
                        <span id="spnTimeType" style="vertical-align:middle;font-family:Arial;font-size:smaller;"></span><br />
                    </td>
                    <td width="34%" align="right">
                        <span id="spnUser" style="vertical-align:middle;font-family:Arial;font-size:smaller;"></span>&nbsp;&nbsp;<br />
                        <span id="spnGFCNo" style="vertical-align:middle;font-family:Arial;font-size:smaller;"></span>&nbsp;&nbsp;<br />
                        <span id="spnBranch" style="vertical-align:middle;font-family:Arial;font-size:smaller;"></span>&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <!-- *** HEADER ENDS HERE *** -->
        
        <!-- *** CONTENT STARTS HERE *** -->        
        <div id="content" style="background-color:White;">
            <div id="divPage1" style="width:100%;display: block;">
                <center>
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr><td>&nbsp;</td></tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td><span class="txtLarge">Today's Exchange Rate is</span></td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td><span id="spnExchangeRate" class="txtSmall">0.00</span></td>
                        </tr>
                    </table>
                </center>
            </div>
        </div>
        <!-- *** CONTENT ENDS HERE *** -->
        
        <!-- *** FOOTER STARTS HERE *** -->
        <div id="footer">
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr><td colspan="2"></td></tr>
                <tr>
                    <td>
                        <input id="btnStart" type="button" value="Start Again" class="btnFooter" />
                        <input id="btnClose" type="button" value="Close" class="btnFooter" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <!-- *** FOOTER ENDS HERE *** -->
    </div>
    </form>
</body>
</html>
