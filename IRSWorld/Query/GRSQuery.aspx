<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSQuery.aspx.cs" Inherits="IRSWorld.Query.GRSQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GRS Direct Query</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .body
        {
         filter: 
            progid:DXImageTransform.Microsoft.Gradient                
            (gradientType=1,startColorStr=ivory,endColorStr=azure);        
        }
        .bar
        {
         font-size: 11px;
         font-family: Arial;
         padding: 3px;
         color: ivory;
         font-weight: bold;
         filter: 
            progid:DXImageTransform.Microsoft.Gradient                
            (gradientType=1,startColorStr=dimgray,endColorStr=#99a0ff);
        }
        .pgcaption
        {
            background-color:none; font-family:Arial; font-size:12px; color:ivory;
            border-style:outset; border:0;        
        }
        .tbcaption
        {
            font-family:Arial; font-size:12px; color:navy; font-weight: bold;
            border-style:outset; border:0;         
            filter: 
                 progid:DXImageTransform.Microsoft.Gradient                
                (gradientType=1,startColorStr=turquoise,endColorStr=ivory);        
        }        
        .proplab
        {
             background-color:none;font-family:Arial; font-size:11px; color:Navy; font-weight: bold; 
        }  
        .clsResult
        {
            width:expression(document.documentElement.clientWidth - 20);
            height:expression(document.documentElement.clientHeight / 2);
        }
        .rscaption
        {
            font-family:Arial; font-size:12px; color:darkSlateGray; font-weight: bold;
            border-style:outset; border:1; 
            border-right: slategray 1px solid; border-top: slategray 1px solid; border-left: slategray 1px solid; border-bottom: slategray 1px solid;         
            filter: 
                 progid:DXImageTransform.Microsoft.Gradient                
                (gradientType=1,startColorStr=wheat,endColorStr=ivory);        
        }
        .rstable
        {
            font-family:Arial; font-size:11px; color:navy;
            border-width:1px;border-style:solid;border-color:darkslategray;border-collapse:collapse;
            filter: 
                progid:DXImageTransform.Microsoft.Gradient                
                (gradientType=1,startColorStr=ivory,endColorStr=azure);        
        }
        .rscell
        {
            font-family:Arial; font-size:11px; color:navy;border-width:1px;border-style:solid;border-color:darkslategray;
        }
        .rsspan { font-family:Arial; font-size:12px; color: maroon; }          
        label
        {
             font-family:Arial; font-size:11px; color:Navy; font-weight: bold; text-align: left; 
        }  
        .panel_upload
        {
            z-index: 2000; 
            position:absolute; 
            left: 200px; 
            top: 150px;
            background-color: beige; 
            width: 55%; 
            height: 60%; 
            display: none; 
            border-top-style: ridge; border-right-style: ridge; border-left-style: ridge; border-bottom-style: ridge;
        } 
        .frame_upload
        {
            background-color: beige; 
            color: beige; 
            DISPLAY:block;
            z-index:-1000;
            height:20px;
            width:50px; 
            position:absolute;
        }                       
    </style> 
    <script language=JavaScript src="../js/jsCommons.js"></script>
    <script language="javascript">
    
        var save_flag = false;
        
        function display_status(stype, imgstat)
        {            
            // var imgstat = document.getElementById('imgstat');
            var src;
            var display;
            
            switch (stype)
            {
                case 0: 
                    display = "none";
                    src = null;
                    break;
                case 1:
                    src = '../images/status.gif';
                    display = 'block';
                    break;
                case 2:
                    src = '../images/upload.gif';
                    display = 'block';
                    break;
                case 3:
                    src = '../images/ajaxloader.gif';
                    display = 'block';
                    break;
            }
            
            imgstat.src = src;
            imgstat.style.display = display;
        }        
        
        function show_status(stype)
        {
            display_status(stype, document.getElementById('imgstat'));
        }
        
        function enable_clear(enabled)
        {
             document.getElementById('btnClear').disabled = !enabled;        
        }
        
        function enable_exec(enabled)
        {
            document.getElementById('btnExec').disabled = !enabled;
        }
        
        function show_result(on)
        {
            var s = (on) ? 'block' : 'none';
            document.getElementById('tdResult').style.display = s;
            if (!on)
            {
                document.getElementById('divResult').innerHTML = '';
            }
        }
        
        var authenticated = false;
        
        function btn_pwdok_click()
        {
            var pwd = document.getElementById('pwdDiscloser').value;
            if (pwd == null || pwd.length == 0)
            {
                alert ('Please enter your password!');
                document.getElementById('pwdDiscloser').focus();
                return;
            }
            
            var rs = IRSWorld.Query.GRSQuery.verify_pasword(pwd);
            if (rs.error != null)
            {
                alert(rs.error.Message);
                document.getElementById('pwdDiscloser').focus();
                return;
            }
            
            if (rs.error != null)
            {
                alert(rs.error.Message);
                close_popup(false);
                return;
            }
            
            close_popup(true);
        }        
        
        function close_popup(ok)
        {
            var div = document.getElementById('divPoppwd');
            document.onkeyup = null;
            div.style.display = 'none';
            authenticated = ok;
        }  
              
        function authenticate()
        {
            var div = document.getElementById('divPoppwd');
            var td = document.getElementById('tdPoppwd');
            div.style.height = document.documentElement.offsetHeight + 'px';
            div.style.width = document.documentElement.clientWidth + 'px';
            div.style.display = 'block';
            if (isIE) div.className = 'div_undock';
            else div.setAttribute("class", "div_undock");
            td.style.height = div.offsetHeight + 'px';  
            
            document.onkeyup = login_click;
            document.getElementById('pwdDiscloser').focus();       
        }
        
        function login_click(e)
        {
            var src;
            if (!e) var e = window.event;
            if (!e.target) 
                src = window.event.srcElement;
            else 
                src = e.target;
            
            if (src.tagName == "INPUT" & src.type == 'text')
            {
                if (e.keyCode == 13) // Enter key
                {
                    document.getElementById('btnPwdOk').onclick();
                }
                else if (e.keyCode == 27) // Escape Key
                {
                    e.returnValue = false;
                    document.getElementById('btnPwdCancel').onclick();
                }
            }
        } 
               
        function app_init()
        {
            authenticate();
            
            if (authenticate)
            {
                enable_exec(true);
                enable_clear(false);
                show_result(false);
            }
            else
            {
                window.location = '/ErrorPage.aspx?ErrorMessage=Invalid Password.';
            }
            
        }        
        
        function clear_stmt()
        {
            document.getElementById('txtStmt').value = '';
            show_result(false);
            IRSWorld.Query.GRSQuery.drop_ds();
            enable_exec(true);
            enable_clear(false);
        }
        
        function get_result(rs)
        {
            show_status(0);
            
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var tabs = rs.value;
            if (tabs.length < 1)
            {
                alert('No result set');
                return;
            }
           
            var div_result = document.getElementById('divResult');
            
            for (var i = 0; i < tabs.length; i++)
            {
                var htm = IRSWorld.Query.GRSQuery.getResult(tabs[i]);
                var s = div_result.innerHTML;
                s = s + htm.value + '</br>';
                div_result.innerHTML = s;
            }
            show_result(true);
            enable_exec(false);
        }
        
        function cmd_result(rs)
        {
            show_status(0);
            
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var div_result = document.getElementById('divResult');
            if (rs.value instanceof Array)
            {
                div_result.innerHTML = rs.value[0] + '</br>';
                eval(rs.value[1]);
            }
            else
            {
                div_result.innerHTML = rs.value + '</br>';
            }
            show_result(true);
            enable_exec(false);        
        }
        
        function exec_stmt()
        {
            var stmt = document.getElementById('txtStmt').value;
            if (stmt.length < 1) 
            {
                alert('Nothing to execute');
                enable_clear(false);
                return;
            }
            if (document.getElementById('tdResult').style.display == 'block')
            {
                show_result(false);
            }
            if (stmt.substring(0, 1) == '!')
            {
                show_status(1);
                IRSWorld.Query.GRSQuery.execute_cmd(stmt, cmd_result);
            }
            else
            {
                show_status(3);
                IRSWorld.Query.GRSQuery.execute_stmt(stmt, get_result);
            }
        }
    </script>
</head>
<body onload="app_init()" onbeforeunload="clear_stmt()">
    <center>
    <form id="form1" runat="server">
    <div style="width: 95%">
        <table width="100%" cellpadding="0" cellspacing="0" class="body">
            <tr>
                <td style="width: 100%; height: 24px" class="bar" align="left">
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" style="width: 20%; border-right: solid thin ivory">
                                <strong>&nbsp;
                                <a id="ahome" href="../Main/irsMain.aspx" class="pgcaption" name="ahome" runat="server">Home</a>
							    </strong>
							</td>
							<td align="left" width="30%" height="30"><strong>
								<asp:label id="lblTitle" runat="server" CssClass="pgcaption">&nbsp;&nbsp;GRS Inquiry</asp:label></strong></td>
							<td width="50%" align="right">
                                <img id="imgstat" align="absMiddle" alt="File loading.." style="display:none"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
				<td align="center" height="20">
                    &nbsp;
                </td>
            </tr>  
            <tr>
                <td style="width:90%" align="center">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:80%;" valign="top">
                                <textarea id="txtStmt" cols="80" rows="10" style="width: 98%" onbeforeeditfocus="enable_exec(true);" onchange="enable_clear(true);"></textarea></td>
                            <td style="width: 20%" valign="top">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <INPUT style="FONT-WEIGHT: bolder; FONT-SIZE: Small; WIDTH: 100%; COLOR: beige; BACKGROUND-COLOR: darkslategray; height: 62px; border-top-width: thin; border-left-width: thin; border-left-color: navy; border-bottom-width: thin; border-bottom-color: navy; border-top-color: navy; border-right-width: thin; border-right-color: navy;" id="btnExec"  type=button value="Find" onclick="exec_stmt()" />                            
                                        </td>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <INPUT style="FONT-WEIGHT: bolder; FONT-SIZE: Small; WIDTH: 52%; COLOR: firebrick; BACKGROUND-COLOR: silver; height: 23px; border-top-width: thin; border-left-width: thin; border-left-color: navy; border-bottom-width: thin; border-bottom-color: navy; border-top-color: navy; border-right-width: thin; border-right-color: navy;" id="btnClear" type=button value="Close" onclick="clear_stmt()" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>    
            </tr>
            <tr>
                <td>&nbsp;
                </td>
            </tr> 
            
            <tr>
                <td id="tdResult" style="width: 100%; display:block; height: 65px;">
                    <table width="100%">
                        <caption style="width: 100%; display: block;" class="tbcaption" align="left">Query Result</caption>
                        <tr>
                            <td style="width: 100%">
                                <div id="divResult" align="left" style="overflow:scroll" class="clsResult">
                                </div>
                            </td>
                        </tr>
                    </table> 
                </td>
            </tr>            
        </table>                  
    </div>
    <div id="divPoppwd" style="width: 330px;border: solid 1px midnightblue; display: none; z-index: 20;">
        <table  cellpadding="0" cellspacing="0">
            <tr>
                <td id=tdPoppwd align=center style="width:100%; vertical-align:middle">
                    <table  cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:320px; height: 15px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Enter Your Login Password</td>
                            <td style="width:10px" class="table_head_dark" align="right">
                                <input type="button" value="x" onclick="pwd_close_popup(false);" style="width:15px; color: red" class="table_head_dark" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%;" align="center" colspan="2">
                                <div>
                                    <table cellspacing=5 style="width:100%">
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>                                      
                                        <tr>
                                            <td class=cell_desc style="width:70%; text-align:left; vertical-align: top; padding-left: 5px">
                                                <input id=pwdDiscloser type=password style="width:100%" />
                                            </td>
                                        </tr>  
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>                                      
                                    </table>
                                </div>
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:100%;vertical-align:middle" align="center" colspan="2">
                                <div style="width:100%; vertical-align:top" align=center>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 18%">
                                                <input id="btnPwdOk" class="button" onclick="btn_pwdok_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="OK" /></td>
                                            <td style="width: 18%">
                                                <input id="btnPwdCancel" class="button" onclick="pwd_close_popup(false);" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>                  
                                    </table>
                                </div>
                            </td>
                        </tr> 
                    </table>
                </td>
            </tr>
        </table>
    </div>     
    </form>
    </center>
</body>
</html>
