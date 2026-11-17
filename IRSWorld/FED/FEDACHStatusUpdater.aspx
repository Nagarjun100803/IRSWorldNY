<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FEDACHStatusUpdater.aspx.cs" Inherits="IRSWorld.FED.FEDACHStatusUpdater" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>FEDACH Status Updater</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons_min.js"                type="text/javascript"></script>
    <script src="../js/js_generic_min.js"               type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js"           type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js"            type="text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"            type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js"           type="text/javascript"></script>  
    <script src="../js/popcalendar.js"                  type="text/javascript"></script>    
    <script src="../js/FED/js_fedachstatusupdater.js"   type="text/javascript"></script>
    <script src="../js/Remittance/jsRemExternals.js"    type="text/javascript"></script>        
    <script src="../js/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript">        
    var fed_viewer = new fedach_status_updater();
    function fixdim()
    {
        if (top.document.getElementById('divCenter'))
        {
            var div = document.getElementById('divmain');
            if (document.getElementById('divmain'))
            {
                div.style.height = (top.document.getElementById('divCenter').offsetHeight + 20) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth) + 'px';
                
                var td = document.getElementById('tdmain');
                td.style.height = div.style.height;
                td.style.width = div.style.width;
            }
        }
    }
    </script>
</head>

<body onload="javasript:fed_viewer.init();" >

    <form id="form1" runat="server" autocomplete="Off">
    <div id="divmain" style="padding-top: 30px;overflow:auto;width:100%;height:700px">
        <table>
            <tr>
                <td   style="width: 135px; vertical-align:top;">
                    <div id="div2" style="vertical-align: top">
                        &nbsp;</div>              
                </td>
                <td style="width: 100px">
            <table cellpadding="0" cellspacing="0" width="1000px" >
            <tr>
                <td > 
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                <img src="../images/toe.GIF" /></td>    
                            <td class="table_head_dark" style="width: 50%; height: 24px">
                                FEDACH Status Updater</td>
                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>   
                      </tr>
                    </table>    
                </td>
                    
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 100%; border-bottom: #336699 3px solid">
                            <tr>
                                <td style="width: 100%; height: 21px; background-color: azure;">

                                    
                                    <div id="divFEDStatusListing" style="background-color: azure;display:block">
                                        <table id="Table2" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:1000px">
                                            <tr>
                                                <td class="cell_caption" colspan="1" style="width: 12%; text-align: center">
                                                    Date From</td>
                                                <td colspan="3" style="width: 25%; text-align: left">
                                                    <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="20" Width="202px" ReadOnly="True"></asp:TextBox>
                                                    <img id="calDateFrom" align="absMiddle" alt="file date" onclick="javascript:popUpCalendar(this, document.getElementById('txtDateFrom'), 'mm/dd/yyyy', document.getElementById('divmain'));"
                                                        src="../images/calendar/imgcalen.png" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" colspan="1" style="width: 12%; text-align: center; height: 21px;">
                                                    Date To</td>
                                                <td colspan="3" style="width: 25%; text-align: left; height: 21px;">
                                                    <asp:TextBox ID="txtDateTo" runat="server" MaxLength="20" Width="202px" ReadOnly="True"></asp:TextBox>
                                                    <img id="calDateTo" align="absMiddle" alt="file date" onclick="javascript:popUpCalendar(this, document.getElementById('txtDateTo'), 'mm/dd/yyyy', document.getElementById('divmain'));"
                                                        src="../images/calendar/imgcalen.png" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" colspan="1" style="width: 12%; text-align: center">
                                                    FEDACH REF NO</td>
                                                <td colspan="3" style="width: 25%; text-align: left">
                                                    <asp:TextBox ID="txtFedAchRefNo" runat="server" MaxLength="20" Width="202px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4" style="height: 24px">
                                                    &nbsp;<input id="btnEdit" class="button" onclick="javascript:fed_viewer.btnEdit_click()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Edit" style="width: 100px"/>
                                                    <input id="btnCancel" class="button" onclick="javascript:fed_viewer.btnCancel_onclick()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 100px" disabled="disabled"/>&nbsp;
                                                    <input id="btnSearch" class="button" onclick="javascript:fed_viewer.search_display()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search" style="width: 100px"/></td>
                                            </tr>
                                        </table>
                                    </div>
                                    
                                 
                                   <div id="div1" style="background-color: azure">
                                   <table id="Table1" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:1000px">
                                            <tr>
                                                    <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                    height: 26px; text-align: left">
                                                        Search Results</td>
                                            </tr>
                                            <tr>
                                                <td  id="tdfedstatgrid"  style="height: 20px; text-align: center "colspan="5">
                                                <div id="divfedstatgrid">
                                                    &nbsp;</div>
                                                </td>
                                            </tr>                                            
                                       <tr>
                                           <td colspan="5" style="height: 20px; text-align: center">
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
                </td>
            </tr>
        </table>
        </div>
    </form>

</body>
</html>
