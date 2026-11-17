<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reminders.aspx.cs" Inherits="IRSWorld.Faultlog.Reminders" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRS WORLD</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    #suggest{
 	position:absolute;
 	background:#fff;
 	width:150px;
 	}
 
    #suggest div{
 	background:#fff;
 	color:#000;
 	padding-left:1px;
 	cursor:hand;
 	text-align:left;
 	}
 
    #suggest div.over{
 	color:#fff;
 	background:#000;
 	}
 	
    .br{
    padding: 0;
    margin: 0;
 	}
    </style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    
    <script src="../js/js_generic.js" type="text/javascript"></script>
    
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/jshttprequest.js" type="text/javascript"></script>

    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>

    <script src="../js/jsgridselector.js" type="text/javascript"></script>

    <script src="../js/Faultlog/jsReminders.js" type="text/javascript"></script>

    <script type="text/javascript">
            var Reminders_Message  = new Reminders_Message();
    </script>
    
    
</head>
<body onload="javascript:Reminders_Message.onload()">
    <form id="form1" runat="server" autocomplete="off">
    <div style =" width:95%" >
        <table style="width:100%" cellpadding="0" cellspacing="5">
            <tr>
                <td align ="center" valign = "top" style="width:100%; border-collapse:collapse; height: 584px;" class ="content_height" colspan ="2">
                    
                    <div id="divReminder" style ="width:100%; height:100%; display:block">
                        
                        <table width ="100%">
                            <tr>
                                
                                <td style="vertical-align:top; width:15%; padding-top:40px">
                                    <div style="margin:0" >
                                        <table>
                                            <tr>
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a href="javascript:Reminders_Message.open_division('divReminders');" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;"> New Reminder</a></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; border-bottom: solid 1px #5697c1;">
                                                    <a href="javascript:Reminders_Message.open_division('divRemindersList');Reminders_Message.load_RemindersList();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">List of Reminders</a>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </div>
                                </td>
                                
                                <td style = "width:80%; padding-top:20px;">
                                    <table cellspacing="0" cellpadding="0" style ="width:96%; background-color:azure">
                                        <tr>
                                            <td align="left" valign="bottom" style ="width:1%;height:24px">
                                            <img alt="" src="../images/toe.GIF" /></td>
                                            <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">REMINDERS / SPECIAL MESSAGES</td>
                                            <td align="left" valign="bottom" style="height: 24px; width: 497px;"  class="bg_body"> 
                                            <img alt="" align="absbottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="3" style ="width:100%;height:100%">
                                                <table style ="width:100%; height:100%" class="table_border">
                                                    <tr>
                                                        <td style="width:100%; height :8px;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style ="width:100%; height:100%; vertical-align:top">
                                                        <table style="width:100%" cellpadding ="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width:78%; height:100%" valign="middle">
                                                                    <div id="divReminders" style="width:98%; height: 100%; margin:0" class="table_body" >
                                                                        <table id = "tblReminders" cellpadding="0" cellspacing="5" style="width:100%; border:solid 1px #336699;">
                                                                            <tr>
                                                                                <td style="height:20px" class ="cell_caption"> Reference No : &nbsp; </td>
                                                                                <td align ="left" style="width:85%"> <input type="text" id = "txtRefno" style="width:35%" runat="server"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="height:10px" class="cell_caption" > For : &nbsp; </td>
                                                                                <td  align ="left" style="width:85%"><input type ="text" id = "txtFor" style="width:600px" runat="server" autocomplete="off" onkeyup="javascript:Reminders_Message.get_email('txtFor')" /></td> 
                                                                            </tr>
                                                                            <tr>
                                                                                <td class ="cell_caption" style="width:15%; height:10px"> Thru : &nbsp; </td>
                                                                                <td align ="left" style="height: 10px; width:85%"><input type ="text" id = "txtThru" style="width:600px" runat="server" autocomplete="off" onkeyup="javascript:Reminders_Message.get_email('txtThru')" /></td> 
                                                                            </tr>
                                                                            <tr>
                                                                                <td class ="cell_caption" style="width:15%; height:20px"> From : &nbsp; </td>
                                                                                <td align ="left" style="width:85%"><input type="text" id="txtFrom"  style="width:35%" runat="server"/><div id="suggest" style="visibility:hidden;border:#000000 1px solid;width:600px;"></div></td> 
                                                                            </tr>
                                                                            <tr>
                                                                                <td class ="cell_caption" style="width:15%; height:20px"> Date : &nbsp; </td>
                                                                                <td align ="left" style="width:85%"><input type="text" id="txtDate" style="width:35%" runat="server" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class ="cell_caption" style="width:15%; height:10px"> Subject : &nbsp; </td>
                                                                                <td align ="left" style="height:10px; width:85%"><input type ="text" id = "txtSubject" style="width:600px" runat="server" /></td>                                                                               
                                                                            </tr>
                                                                            <tr>
                                                                                <td class ="cell_caption" style="width:15%; height: 21px;"> Attachments : &nbsp </td>
                                                                                <td id="tdFiles" align="left" style="height: 21px; width:85%"><textarea id="Forward_Files" style="width:98%" runat="server" ></textarea></td>
                                                                            </tr>
                                                                            <tr id="tr_forward_hidden"><td></td><td id="td_forward_hidden"><textarea id="forward_hidden" style="width:98%" runat="server"></textarea></td></tr>
                                                                            <tr>
                                                                                <td align="left" colspan="2" style="width:100%">
                                                                                <input id="file1" type="file" style="width:56%" runat="server" />&nbsp<%--onpropertychange="javascript:Reminders_Message.file_browse()"--%>
                                                                                <%--<input id="btnAttach" type="button" value="Attach more files" style="width:20%" onclick="javascript:Reminders_Message.attach_files()" />--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td id="tdfile2" align="left" colspan="2" style="width:100%; display:none"><input id="file2" type="file" style="width:56%" runat="server" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td id="tdfile3" align="left" colspan="2" style="width:100%; display:none"><input id="file3" type="file" style="width:56%" runat="server" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2" style="width:100%"><textarea id="txtareaReminderMsg" cols="2" style="width:100%; height:100px;" runat="server"></textarea></td>
                                                                            </tr>
                                                                            <tr><td colspan="2" >
                                                                                <table id="tblButton" style="width:100%">
                                                                                    <tr style="height:30px"><td style="width:20%"><input id="txtHiddenSeqId" type="hidden" style="width:2%" /></td>
                                                                                        <td style="width:15%"><input id="btnNew" type="button" class="button" value="New" onclick="javascript:Reminders_Message.insert_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" /></td>
                                                                                        <td style="width:15%"><input id="btnForward" type="button" class="button" value="Forward" onclick="javascript:Reminders_Message.forward_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';"/></td>
                                                                                        <%--<td style="width:15%"><input id="btnSend" type="button" value="Send" onclick="javascript:Reminders_Message.commit_record()" /></td>--%>
                                                                                        <td style="width:15%"><asp:Button ID="aspbtnSend" runat="server" Text="Send" CssClass="button" OnClick="send_file" onmouseout="this.className='button';" onmouseover="this.className='button_over';" /></td> <%--OnClientClick="javascript:Reminders_Message.sending()" --%>
                                                                                        <td style="width:15%;"><input id="btnCancel" type="button" class="button" value="Cancel" onclick="javascript:Reminders_Message.onload()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" /></td>
                                                                                        <td style="width:20%"></td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td></tr>
                                                                        </table>
                                                                    </div>
                                                                    <div id="divRemindersList" onmousemove="javascript:Reminders_Message.active_grid = 'divRemindersList';"><br /></div>                                                                                                                                                   
                                                                    <div id="divPopSending" style="width:350px; height:130px; background:white; position:absolute; padding:10px; display:none; top: 114px; left: 314px;"> 
                                                                        <table style="width:100%; height:100%">
                                                                            <tr><td><div id="divSending"></div></td></tr>
                                                                            <tr><td>Sending...</td></tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                </tr>
                            
                        </table>
                    </div>
                </td>
            </tr>
        </table>

    </div>
    
    </form>
</body>
</html>
