<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MICCS.aspx.cs" Inherits="IRSWorld.RITS.MICCS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >

<head>
    <title>MICCS (Mizuho Confirmation Control System)</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/jsCommons.js" type="text/javascript"></script>  
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>     
    <script src="../js/RITS/jsMICCSConv.js" type="text/javascript"></script>
    <script type="text/javascript">
        var micc = new MICCSConv();
    </script>
</head>

<body id="bodyM" onload="javascript:micc.load_miccs();" topmargin ="6" leftmargin="0">
    <form id="frmM" runat="server">
    <div id = "divMICCS">
        <center>
        <div id ="divMICCSBody">
            <table id = "tbl_outermost" cellpadding = "10"; cellspacing = "0" style = "width:100%">
                <tr>
                    <td style="vertical-align: top; width:220px; height: auto; padding-top: 10px">
                        <div id="divmenu" style="margin-top: 20px; display: block; overflow: auto"></div>
                            <a href= "javascript:micc.load_miccs();" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Confirmation List</a> <br /> 
                            <a href="javascript:micc.check_miccs();" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Process Items</a> <br />                                                     
                    </td>
                    <td style="width:auto; height: auto;">
                        <table cellpadding="0" cellspacing="0" style="width:100%">
                        <tr>
                            <td style ="text-align:left; width:100%">
                                <table cellpadding="0" cellspacing="0" style ="width:100%" >
                                    <tr>
                                        <td align = "left" style = "width:10px; height:auto; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                        <td align = "left" style = "width: 45%; height:auto; border-collapse:collapse" class="table_head_dark" >Confirmation List</td>
                                        <td align = "left" style = "width: 59%; height:auto; vertical-align:bottom; border-collapse:collapse"><img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table style="width:100%; border:solid 3px #336699;">                                  
                                       <tr>
                                        <td id="tdConfList" colspan="2" style="width: 100%; height: 21px">
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
        <div id="divSearch" style="z-index: 101; position:absolute; left: 116px; color: #00008B;
                border: solid 2px #000; padding: 10px; display: none; font-family: Arial; top: 55px;
                width: 500px;" class="table_body">
                <table id="tblSearch" cellpadding="0" cellspacing="0" width="90%" <%--class="table_border"--%>>
                    <tr>
                        <td align="left" style="width: 1%; border-collapse: collapse; height: 24px" valign="bottom">
                            <img alt="toe." src="../images/toe.GIF" /></td>
                        <td id="capSearch" align="left" class="table_head_dark" style="width: 32%; border-collapse: collapse;
                            height: 24px">
                            Search Remitter</td>
                        <td align="left" class="bg_body" style="width: 300px; height: 24px" valign="bottom">
                            <img alt="rend." align="absBottom" src="../images/rend.gif" /></td>
                    </tr>
                    <tr>
                        <td colspan="3" style="background: azure; width: 100%">
                            <table class="table_border" style="width: 100%">
                                <tr>
                                    <td style="width: 99%; height: 192px;">
                                        <table cellpadding="0" cellspacing="0" class="table_body" style="border-right: skyblue 1px solid;
                                            border-top: skyblue 1px solid; border-left: skyblue 1px solid; border-bottom: skyblue 1px solid;
                                            width: 99%; height: 181px;">
                                            <tr>
                                                <td class="table_caption" style="width: 50%; height: 26px;">
                                                    Search by:</td>
                                                <td align="left" class="table_caption" style="width: 50%; height: 26px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2" style="width: 100%; height: 115px" valign="middle">
                                                    <table cellpadding="0" cellspacing="5" width="96%" style="margin-top: 2%">
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width: 30%; height: 25px;">
                                                                &nbsp;Lastname :</td>
                                                            <td align="left" style="width: 70%; height: 25px;">
                                                                <input id="txtSearchLastname" style="font-size: medium; width: 100%; text-align: left"
                                                                    type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width: 30%">
                                                                &nbsp;Firstname :</td>
                                                            <td align="left" style="width: 70%">
                                                                <input id="txtSearchFirstname" style="font-size: medium; width: 100%; text-align: left"
                                                                    type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="cell_desc" style="width: 30%">
                                                                &nbsp;Middlename :</td>
                                                            <td align="left" style="width: 70%">
                                                                <input id="txtSearchMiddlename" style="font-size: medium; width: 100%; text-align: left"
                                                                    type="text" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="height: 28px;vertical-align:top;" valign="top" align="center">
                                                    <input id="btnsearch" class="button" onclick="javascript:micc.search_cif();micc.hidePanel('divSearch');"
                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                        type="button" value="Search" />
                                                    <input id="btnCancelSearch" class="button" onclick="javascript:micc.hidePanel('divSearch');"
                                                        onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                        type="button" value="Cancel" />
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
        </center>
        </div>        
    </form>
</body>
</html>
