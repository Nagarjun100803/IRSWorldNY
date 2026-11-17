<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchSetupMain.aspx.cs" Inherits="IRSWorld.BranchSetup.BranchSetupMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch Setup Main</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_BranchSetupMain.js" type="text/javascript"></script>
    <script src="../js/js_sidebar.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body onload ="page_setup();" onresize="fix_height()">
    <center>
    <form id="form1" runat="server">
    <div id="tblContent" style="width:100%;height:expression(screen.availHeight - 248);display:none;">
    <table style="width:100%;"> <!--class="content_height"> -->
        <tr>
           <td style="vertical-align: top; width:18%; text-align:right;" id="tdbrsetupmenu">
               <a id=ahider href=# onclick=javascript:hide_menu()>«</a>
               <div id=divBranchSetupMenuTop style="width: 100%; margin-top: 5px;display:block;">
               <b class="rr">
               <b class="rr1"><b></b></b>
               <b class="rr2"><b></b></b>
               <b class="rr3"></b>
               <b class="rr4"></b>
               <b class="rr5"></b></b>                                                                
                <div id="divBranchSetupMenu" class="rrfg" style="border-right: solid 1px #87ceeb; overflow-y:auto; overflow-x:hidden">
                </div>
              <b class="rr">
              <b class="rr5"></b>
              <b class="rr4"></b>
              <b class="rr3"></b>
              <b class="rr2"><b></b></b>
              <b class="rr1"><b></b></b></b>                                                                    
            </div>                                                 
            </td>
            <td id=tdContent style="width: 82%;height: 100%;" valign="top">
                <div id=divContent style="width:100%;height:expression(screen.availHeight - 248); vertical-align:top">
                    <iframe id=ifbranchsetup name="ifbranchsetup" frameborder=0 allowtransparency="true" scrolling="auto" style="width:100%; height:100%; vertical-align:top;display:block">
                    </iframe>
                </div>
            </td>
        </tr>
    </table>
    </div>
    </form>
    </center>
</body>
</html>