<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCenter.aspx.cs" Inherits="IRSWorld.Main.frmCenter" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Center Frame Page</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script> 
    <script src="../js/Main/jsCenter.js" type="text/javascript"></script>    
    <script type="text/javascript">
    </script>
</head>
<body onload="page_setup();" onresize="fix_height();">
    <form id="form1" runat="server">
    <div id=divCenter style="width:100%; height: 100%; display:none; background-image:url(../images/header/pnb_hands.gif); background-repeat: no-repeat; background-position: right;">
        <table id=tblContent style="width:100%; height:100%">
            <tr>
                <td id=tdContent align="center" style="width: 100%; height: 100%;vertical-align:middle">
                    <table style="width:80%; height: 20%">
                        <tr>
                            <td style="width: 100%; vertical-align: middle" align=center>
                                <span id="spanWelcome" style="font-family:Arial; font-weight: bold; font-size: 2em; color:MidnightBlue; display: block;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td id=hrsep style="display: none;"><hr /></td>
                        </tr>
                        <tr>
                            <td align=center>
                                <div id=divCountMsg class=cell_desc style="display:none; background-color:Transparent;">
                                    <a id=aback href=# onclick="javascript:show_prev_msg();" class=cell_desc><</a>
                                    <span id=spanMsgNum>0</span>&nbsp;Of&nbsp;<span id=spanTotalMsg>0</span>&nbsp;Messages&nbsp;&nbsp;
                                    &nbsp;<a id=anext href=# onclick="javascript:show_next_msg();" class="cell_desc">></a>&nbsp;|&nbsp;&nbsp;&nbsp;
                                    &nbsp;<a id=alist href=# onclick="javascript:list_msgs();">List messages?</a><br /><br />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="divMsgView" style="width: 700px; height: 200px;word-wrap:break-word;overflow-y:auto; display:none; background-color: Transparent;"></div>
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
