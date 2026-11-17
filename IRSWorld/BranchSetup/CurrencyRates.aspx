<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrencyRates.aspx.cs" Inherits="IRSWorld.BranchSetup.CurrencyRates" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Fx Rates Maintenance Page</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tab_unselected A { color:#5697c1; text-decoration: none; }
        .tab_unselected A:hover { color:135f94; text-decoration: underline; }
        .tab_unselected A:visited { color:#5697c1; text-decoration: none; }
        .tab_selected A { color:#135f94; text-decoration: none; }
        .tab_selected A:hover { color:5697c1; text-decoration: underline; }
        .tab_selected A:visited { color:#135f94; text-decoration: none; }
        
        .tab_unselected 
        {
            width: 120px; 
            height: 100%;
            background-color: dimgray;
            color: #5697c1;
            font-family: Arial; font-weight: bold; font-size: 12px; 
            border-collapse: collapse;
            border: solid 2px #5697c1;     
            cursor:hand;  
        }
        .tab_selected
        {
            width: 120px; 
            height: 100%;
            background-color:#87ceeb;
            color: #135f94; 
            font-family: Arial; font-weight: bold; font-size: 12px; 
            border-collapse: collapse;
            border: solid 2px #5697c1;  
            cursor: hand;     
        }
    </style>
    <script src="../js/jsCommons_min.js" type="text/javascript"></script> 
    <script src="../js/popcalendar_min.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_CurrencyRates.js" type="text/javascript"></script>
</head>
<body id="theBody" onload="page_setup();" onunload="close_session();">
    <center>
    <form id="form1" runat="server">
    <div id="divBody" style="width:98%;display:none; vertical-align:top; margin-top:30px;">
        <table style="width:100%; height:100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td id=tdContent align="center" style="width:100%; border-collapse:collapse; vertical-align:middle;">
                    <table style="width:95%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr>
                                        <td align="left" style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                        <td id="tdPageTitle" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            Currency Rates</td>
                                        <td align="left" style="width: 60%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                            
                            </tr>
                            <tr>
                                <td style="width: 100%; border:solid 3px #336699;" class=table_body>
                                    <table style="width:100%;" cellspacing="3">
                                        <tr>
                                            <td colspan=3 style="height:10px"></td>
                                        </tr>
                                        <tr>    
                                            <td class=cell_caption style="width: 23%; height: 21px">Branch:&nbsp;</td>
                                            <td style="width:60%; height: 21px;" align="left">
                                                <select id=ddbranch style="width: 100%; height:100%" onchange="new_branch(this);"></select>
                                            </td>
                                            <td align=left style="height:21px">
                                            </td>
                                        </tr>
                                        <tr>    
                                            <td class=cell_caption style="width: 23%; height: 21px">Effective Date:&nbsp;</td>
                                            <td style="width:60%;" align="left">
                                                <input type="text" id="txtValueDate" onchange="txtValueDate_changed(this.value);" class=cell_desc style="width: 93px; height: 100%;" />
                                                <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtValueDate, 'mm/dd/yyyy');" />
                                            </td>
                                            <td align="left">
                                                <input id=btncybermag type=button class=cell_caption value="Cybermag" style="width: 90px; text-align: center" onclick="javascript:btn_cybermag_click();" onmouseover="btn_cybermag_over();" />
                                                <input id= btncyberset type=button class=cell_caption value="»" style="width:20px; visibility:hidden; text-align:center" onclick="javascript:btn_cyberset_click();" onmouseover="javascript:btn_cyberset_over();" onmouseout="javascript:btn_cyberset_out();" />
                                                </td>
                                        </tr> 
                                        <tr>
                                            <td colspan=3 style="width:100%;height:10px;"></td>
                                        </tr> 
                                        <tr>
                                            <td colspan=3 style="width:100%; vertical-align:top">
                                                <table style="width:100%" cellpadding=0 cellspacing=0>
                                                    <tr>
                                                        <td id=td_tab_cell style="width:100%; vertical-align:bottom; display:none" align=left>
                                                             <!-- 
                                                            <table id=tbl_tabrow>
                                                            
                                                                <tr id=rate_type_tabrow>
                                                                   
                                                                    <td id=td_tab_std class=tab_selected><a href="javascript:show_rate_type(1);">Standard Rates</a></td>
                                                                    <td id=td_tab_spl class=tab_unselected><a href="javascript:show_rate_type(2);">Special Rates</a></td>
                                                                   
                                                                </tr>
                                                             
                                                            </table> 
                                                            -->
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id="tdfxrates" style="width: 100%" class="cell_desc">
                                                            <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                                           
                                                            <!-- content ends here -->
                                                        </td>
                                                    </tr>                                                
                                                </table>
                                            </td>
                                        </tr>                                   
                                        <tr>
                                            <td colspan=3 style="height:10px"></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                    </table> 
                </td>
            </tr>
        </table>    
    </div> 
    <div id="divPopup" style="border: solid 1px midnightblue; display: none; position: absolute; z-index: 10;">
        <table  cellpadding="0" cellspacing="0" style="width: 330px;">
            <tr>
                <td id=td_popup style="width:100%; vertical-align:middle; text-align:center; border-collapse:collapse;">
                    <table  cellpadding="0" cellspacing="0" style="width: 330px;">
                        <tr>
                            <td style="width:320px; height: 15px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Cybermag Rates</td>
                            <td style="width:10px" class="table_head_dark" align="right">
                                <input type="button" value="x" onclick="close_popup();" style="width:15px; color: red" class="table_head_dark" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; border-collapse:collapse;" align="left" colspan="2">
                                <table>
                                    <tr>
                                        <td id=aphp class=tab_unselected><a href="javascript:show_div_rate('php');">FX in PHP Terms</a></td>
                                        <td id=ausd class=tab_selected><a href="javascript:show_div_rate('usd');">FX in USD Terms</a></td>
                                        <td id=amid class=tab_unselected><a href="javascript:show_div_rate('mid');">Booking Rates</a></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:100%;" align="center" colspan="2">
                                <div id="divFxPHP" style="width:100%; height:100%; overflow:auto; display:none">
                                </div>                
                                <div id="divFxUSD" style="width:100%; height:100%; overflow:auto; display:none">
                                </div>
                                <div id="divBmRate" style="width:100%; height:100%; overflow:auto; display:none">
                                </div>                    
                            </td>
                        </tr> 
                        <tr>
                            <td style="width:100%;vertical-align:middle;" align="center" colspan="2">
                                <div style="width:100%; padding-top: 20px;">
                                    <table style="width: 70%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 50%">
                                                <input id="btnUpload" class="button" onclick="btn_upload_click();" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Upload" /></td>
                                            <td style="width: 50%">
                                                <input id="btnClose" class="button" onclick="close_popup();" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Close" /></td>
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
    <div id=divCybersettings style="width: 230px; display:none; position: absolute; z-index: 5" class=ds_gray>
        <table style="width:100%">
            <caption class="table_caption" style="vertical-align:middle">Cybermag upload settings</caption>
            <tr>
                <td style="width:24px">
                    <input type=radio name=rbcyberset id=chksetupbuying checked onclick="rbcyberset_change('buying');" />
                </td>
                <td class="cell_desc" align=left>Upload using FXUSD buying rate</td>
            </tr>
            <tr>
                <td style="width:24px">
                    <input type=radio name=rbcyberset id=chksetupselling onclick="rbcyberset_change('selling');" />
                </td>
                <td class="cell_desc" align=left>Upload using FXUSD selling rate</td>
            </tr> 
            <tr>
                <td style="width:24px">
                    <input type=radio name=rbcyberset  id=chksetupbooking onclick="rbcyberset_change('booking');" />
                </td>
                <td class="cell_desc" align=left>Upload using booking rate</td>
            </tr>   
            <tr>
                <td style="width:24px">
                    <input type=radio name=rbcyberset id=chksetupmid  onclick="rbcyberset_change('mid');" />
                </td>
                <td class="cell_desc" align=left>Upload using mid rate</td>
            </tr> 
            <tr><td style="width:100%;height:8px" colspan=2><hr /></td></tr>
            <tr>
                <td colspan=2 align=center style="width:100%; vertical-align:middle;">
                    <input id=btncybersetclose style="width:70px;height:18px;text-align:center;" class=button value="Close" onclick="document.getElementById('divCybersettings').style.display = 'none';" />
                </td>
            </tr>                                    
        </table>
    </div>
    </form>
    </center>
</body>
</html>
