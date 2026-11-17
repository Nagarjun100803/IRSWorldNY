<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POLINK.aspx.cs" Inherits="IRSWorld.RITS.POLINK" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Postal Link Processing</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script src="../js/RITS/jsPOLINK.js" type="text/javascript"></script>
    
   <script language="javascript">
   
    function fixht()
    {
        document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
        document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
    }

    function fixmain()
    {
        var brmain = false;
        try
        {
             brmain = parent.is_branchsetup_main();
        }
        catch(e){}
        
        if (!brmain)
        {
            document.getElementById('theBody').onresize = fixht;
            document.getElementById('divBody').style.overflow = 'auto';
            
            fixht();
        }        
    } 
   </script>




</head>
<body id="theBody" onload="javascript:show_grid()" >
    <form id="form1" runat="server">
    <div id="divBody" style="font-family: Arial">
&nbsp
        <table id="tbl_outermost" cellpadding="10" cellspacing="0" style="width: 100%">
            <tr>
                <td style="vertical-align: top; width:220px; height: auto; padding-top: 10px">
                        <div id="divmenu" style="margin-top: 20px; display: block; overflow: auto"></div>
                            <a href="javascript:show_grid()" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">POLINK Confirmation List</a> <br /> 
                            <a href="javascript:show_panel('divSearch','POLINK');" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Filter by date</a> <br />                         
                            <a href="javascript:filter_by_processed('POLINK');" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Filter by Processed</a> <br /> 
                            <a href="javascript:filter_by_hold('POLINK');" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Filter by hold status</a> <br /> 
                            <a href="javascript:filter_by_other('POLINK');" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Filter by Other Transactions</a> <br />                        
                            <a href="javascript:filter_by_overlimit('POLINK');" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Filter by Overlimit Amounts</a> <br />                        
                            <a href="javascript:filter_by_pdep('POLINK');" style="font-weight: bold;font-size: 12px; font-style: italic; font-family: Arial;">Entries with deposit accounts</a> <br />                        
                            <a id="anchor_off" href="#">«</a>
                    </td>
                <td >
                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                        <tr>
                            <td style="width: 100%; text-align: left; height: 42px;">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                   <tr>
                                        <td align="left" style="vertical-align: bottom; width: 10px; border-collapse: collapse;
                                            height: auto">
                                            <img align="left" src="../images/toe.GIF" alt=""/></td>
                                        <td align="left" class="table_head_dark" style="width: 45%; border-collapse: collapse;
                                            height: auto">
                                            Confirmation List</td>
                                        <td align="left" style="vertical-align: bottom; width: 59%; border-collapse: collapse;
                                            height: auto">
                                            <img align="absBottom" src="../images/rend.gif" /></td>
                                    </tr>
                                </table><table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                                    width: 100%; border-bottom: #336699 3px solid; overflow: auto;">
                                    <tr>
                                        <td id="maincontainer" colspan="2" style="width: 100%;">
                                            <div style="overflow: auto; width: 100%; height: 250px">
                                    <table style="width: 100%; border-top-width: 3px; border-left-width: 3px; border-left-color: #336699; border-bottom-width: 3px; border-bottom-color: #336699; border-top-color: #336699; border-right-width: 3px; border-right-color: #336699;">
                                        <tr>
                                            <td id="container" colspan="2" style="width: 100%; ">
                                            </td>
                                        </tr>
                                    </table>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table id="Table1" style="border-right: #336699 3px solid; border-top: #336699 3px solid;
                                    border-left: #336699 3px solid; width: 100%; border-bottom: #336699 3px solid">
                                    <tr>
                                        <td align="center" style="width: 25%">
                                            <input id="btnAdd" type="button" value="Export to CSV" onclick="javascript:generate_txt('POLINK');"/></td>
                                        <td align="center" style="width: 25%">
                                            <input id="btnCancel" type="button" value="Enrollment" onclick="javascript:show_panel('divEnroll','ENROLLMENT');" /></td>
                                        <td align="center" style="width: 25%">
                                            </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

    <div id="divSearch" class="table_body" style="border-right: #000 2px solid; padding-right: 10px;
            border-top: #000 2px solid; display: none; padding-left: 10px; z-index: 101;
            left: 3px; padding-bottom: 10px; border-left: #000 2px solid; width: 500px;
            color: #00008b; padding-top: 10px; border-bottom: #000 2px solid; font-family: Arial;
            position: absolute; top: 617px">
            <table id="tblSearch" cellpadding="0" cellspacing="0" width="90%">
                <tr>
                    <td align="left" style="width: 1%; border-collapse: collapse; height: 24px" valign="bottom">
                        <img alt="toe." src="../images/toe.GIF" /></td>
                    <td id="capSearch" align="left" class="table_head_dark" style="width: 32%; border-collapse: collapse;
                        height: 24px">
                        Filter by Date</td>
                    <td align="left" class="bg_body" style="width: 300px; height: 24px" valign="bottom">
                        <img align="absBottom" alt="rend." src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="background: azure; width: 100%">
                        <table class="table_border" style="width: 100%">
                            <tr>
                                <td style="width: 99%; height: 192px">
                                    <table cellpadding="0" cellspacing="0" class="table_body" style="border-right: skyblue 1px solid;
                                        border-top: skyblue 1px solid; border-left: skyblue 1px solid; width: 99%; border-bottom: skyblue 1px solid;
                                        height: 181px">
                                        <tr>
                                            <td class="table_caption" style="width: 50%; height: 26px">
                                                Select Tranasaction Date</td>
                                            <td align="left" class="table_caption" style="width: 50%; height: 26px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="width: 100%; height: 65px" valign="middle">
                                                <table cellpadding="0" cellspacing="5" style="margin-top: 2%" width="96%">
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 30%; height: 25px">
                                                            &nbsp;Date From :</td>
                                                        <td align="left" style="width: 70%; height: 25px">
                                                            <input id="txtFDate" name="txtFDate" onkeypress="return false;" onkeyup="return false;"
                                                                onpaste="return false;" style="width: 92%" type="text" /><img onclick="popUpCalendar(this, document.forms[0].txtFDate, 'mm/dd/yyyy',document.getElementById('divSearch'));"
                                                                    src="../images/calendar/imgcalen.png" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 30%; height: 21px;">
                                                            Date To :</td>
                                                        <td align="left" style="width: 70%; height: 21px;">
                                                            <input id="txtTDate" name="txtTDate" onkeypress="return false;" onkeyup="return false;"
                                                                onpaste="return false;" style="width: 92%" type="text" /><img onclick="popUpCalendar(this, document.forms[0].txtTDate, 'mm/dd/yyyy',document.getElementById('divSearch'));"
                                                                    src="../images/calendar/imgcalen.png" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="vertical-align: top; height: 28px" valign="top">
                                                <input id="btnsearch" class="button" onclick="javascript:filter_by_date();"
                                                    onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                    type="button" value="Search" />
                                                <input id="btnCancelSearch" class="button" onclick="javascript:hidePanel('divSearch','');"
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
       
        <div id="divEnroll" class="table_body" style="border-right: #000 2px solid; padding-right: 10px;
            border-top: #000 2px solid; display: none; padding-left: 10px; z-index: 101;
            left: 6px; padding-bottom: 10px; border-left: #000 2px solid; width: 98%;
            color: #00008b; padding-top: 10px; border-bottom: #000 2px solid; font-family: Arial;
            position: absolute; top: 625px; overflow: auto; height: 98%;">
            
            <iframe id="testframe" src="" width="95%" height="95%" style="width: 100%; height: 100%" ><p>Your browser does not support iframes</p></iframe>
            <br />
            <input id="Button1" type="button" value="Cancel Enrollment" onclick="javascript:hidePanel('divEnroll','ENROLLMENT');" style="width: 17%"/></div>
        
            
</div>
    </form>
</body>
</html>
