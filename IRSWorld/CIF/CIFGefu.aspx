<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIFGefu.aspx.cs" Inherits="IRSWorld.CIF.CIFGefu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CIF GEFU</title>
    <link href="../css/Default.css" rel="stylesheet"        type="text/css" />
    <script src="../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>
    <script src="../js/popcalendar.js"                      type="text/javascript"></script> 
    <script src="../js/CIF/js_cif_main.js"                  type="text/javascript"></script>
    <script src="../js/CardRem/jsGFMCMaintenance.js" type="text/javascript"></script> 
    
    <script type="text/javascript">    
        var cif_main = new cif_main();
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table id="CIFGEFU" cellpadding="0" cellspacing="30" style="width: 100%; height:100%; margin-top:30px">
            <tr>
                <td style=" vertical-align:top; width: 15%;">
                </td>
                <td style=" vertical-align:top; width: 85%;">
                    <div id="div_GEFU_Reports">
                        <table cellpadding="0" cellspacing="0"          style="width:70%" >
                            <tr> 
                                <td>
                                    <table cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                                <img src="../images/toe.GIF" /></td>    
                                            <td id="capResult" class="table_head_dark" style="width: 50%; height: 24px">
                                                CIF GEFU</td>
                                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td> 
                                        </tr>  
                                    </table> 
                                </td>                                       
                            </tr>
                            <tr>
                                <td colspan= "3" style="height:19px;">
                                    <table id="tblGEFU" style="background:azure; border:solid 3px #336699; width:100%">
                                        <tr>
                                            <td style="width:100%; height:76px;">
                                                <div id = "CIFGEFUMAIN">
                                                    <table id="tblCIFGEFU" cellpadding="0" cellspacing="5" style="width:100%; border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" colspan = "4" style="width: 85%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" ></td>
                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 39%; height: 24px">
                                                                Date: MM/DD/YYYY&nbsp;</td>
                                                            <td align="left" colspan="2" style="width: 75%; height: 24px">
                                                                <input id="txtCIFDate" style="width: 50%" type="text" maxlength="10" onkeyup="cif_main.keyup(txtCIFDate.id);"/>
                                                                <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtCIFDate, 'mm/dd/yyyy');"/>
                                                            </td>
                                                        </tr>
                                                        <tr></tr>
                                                        <tr>
                                                            <td style="width: 18%" colspan="3" align="right">
                                                                <input id="btnCreateGefu" onclick="javascript:cif_main.create_gefu();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Create" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                               </div>
                                               
                                               <div id="divImgLoad" style="z-index:101;position:absolute;left:501px;color:darkblue;border:solid 2px #000;padding:10px;display:none; font-family: Arial; top: 300px;">
                                                    <table style="width:35%" cellpadding="0" cellspacing="6">
                                                        <tr>
                                                            <td class="cell_desc" align="left" style="font-size: 12px; width: 30%;">
                                                            <label id="lblLoad">Loading...</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <center><div id="divLoading"></div></center>
                                                            </td>
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
                </td>
            </tr>
        </table>        
    </form>
</body>
</html>
