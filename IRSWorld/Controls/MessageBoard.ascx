<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MessageBoard.ascx.cs" Inherits="IRSWorld.Controls.MessageBoard" %>
<link href="../css/Default.css" rel="stylesheet" type="text/css" />
<div id="_msg_board" style="display:none;">
<table style="width:100%; height:100%">
<tr>
    <td id="_msg_fix" valign="middle" align="center"  style="width: 100%;">
    <div id= "_msg_tab" style="margin: 5px 5px 5px 5px">
        <table style="width:100%" cellpadding="0" cellspacing="0">
            <tr>
                <td id="_msg_indicator" align="center" style=" width: 10%; font-family: Arial; background: lightblue; color: lightcyan; font-size: 3em; font-weight:bold; border-right: steelblue thin solid; border-top: steelblue thin solid; border-left: steelblue thin solid; border-bottom: steelblue thin solid; height: 77px;">
                    !
                </td>
                <td style="width: 7px;"></td>
                <td style="width: 85%; color: coral; font-size: medium;font-family: Arial;font-weight: bold; text-align: left; vertical-align: top; ">
                    <span id="_msg_text" style="overflow:scroll; color:midnightblue; font-weight: bold; font-size:14px; text-align: left; vertical-align: top;"></span>
                </td> 
                <td valign="top" align="center" style="width: 5%">
                    <input type="button" value="x" style="font: Arial bold 10px; border-right: skyblue thin ridge; border-top: skyblue thin ridge; border-left: skyblue thin ridge; color: #ffcc66; border-bottom: skyblue thin ridge; background-color: lightcyan;" onclick="close_msgboard();" />
                </td>       
            </tr>
        </table>  
    </div>  
    </td>
</tr>
</table>
</div>