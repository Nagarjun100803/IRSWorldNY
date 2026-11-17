<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="IRSWorld.Help.Help" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Help</title>
    <link rel="stylesheet" type="text/css" href="../js/tabcontent/tabcontent.css" />
    
<script type="text/javascript" src="../js/tabcontent/tabcontent.js">

/***********************************************
* Tab Content script v2.2- © Dynamic Drive DHTML code library (www.dynamicdrive.com)
* This notice MUST stay intact for legal use
* Visit Dynamic Drive at http://www.dynamicdrive.com/ for full source code
***********************************************/

</script>
    
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table width="100%" style="border-right: black 1pt solid; padding-right: 1px; border-top: black 1pt solid; padding-left: 1px; background-image: none; padding-bottom: 1px; margin: 1px; border-left: black 1pt solid; width: 100%; padding-top: 1px; border-bottom: black 1pt solid; height: 100%">
        <tr>
        <td style="width: 30%;  height: 100%;">
        <table style="border-right: black 1pt solid; padding-right: 1px; border-top: black 1pt solid; padding-left: 1px; background-image: none; padding-bottom: 1px; margin: 1px; border-left: black 1pt solid; width: 100%; padding-top: 1px; border-bottom: black 1pt solid; height: 600px">
            <tr>
                <td valign="top">
                <div id="pettabs" class="indentmenu">
                    <ul>
                    <li><a href="#" rel="tabContents" class="selected">Contents</a></li>
                    <li><a href="#" rel="tabIndex">Index</a></li>
                   
                    </ul>
                    <br style="clear: left" />
                    </div>

                    <div style="border:1px solid gray; width:300px;  padding: 5px; margin-bottom:1em ">

                    <div id="tabContents" class="tabcontent">

                    <iframe src ="/Help/HelpContents.aspx" width="100%" height="535px">
                    <p>Your browser does not support iframes.</p>
                     </iframe>
                     
                    </div>

                    <div id="tabIndex" class="tabcontent">
                   
                        <iframe src ="/Admin/AuditLogViewer.aspx" width="100%" height="535px">
                        <p>Your browser does not support iframes.</p>
                        </iframe>
                      
                    </div>

                    </div>


                    <script type="text/javascript">

                    var mypets=new ddtabcontent("pettabs")
                    mypets.setpersist(true)
                    mypets.setselectedClassTarget("link")
                    mypets.init()// put some integer to slide show

                    </script>
                    
                </td>
            </tr>
        
        </table>
        </td>
        
        <td style="height: 100%">
        <table style="border-right: black 1pt solid; vertical-align:top; padding-right: 1px; border-top: black 1pt solid; padding-left: 1px; background-image: none; padding-bottom: 1px; margin: 1px; border-left: black 1pt solid; width: 100%; padding-top: 1px; border-bottom: black 1pt solid; height: 600px">
            <tr>
                <td valign="top">
                <div  style="padding: 5px;  margin-bottom:1em"> 
                <iframe src ="/Admin/AuditLogViewer.aspx" width="100%" height="565px">
                 <p>Your browser does not support iframes.</p>
                </iframe>
                </div>
                </td>
                
            </tr>
        </table>
        </td>
        </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
