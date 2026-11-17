<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EXCO_Al_Rajhi_Response.aspx.cs" Inherits="IRSWorld.EXCO.EXCO_Al_Rajhi_Response" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>AL RAJHI RESPONSE GENERATOR</title>
    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />    
    <script src="../js/jsCommons.js"                        type="text/javascript"> </script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript" > </script>
    <script type="text/javascript" src="../js/Remittance/jsRemExternals.js"> </script>
    <script src="../js/js_generic.js"                       type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"  ></script>    
    <script src="../js/EXCO/js_EXCO_Al_Rajhi_Response.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript"></script> 
    <script type="text/javascript">
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
<body onload="javascript:js_Al_Rajhi.init();" onresize="fixdim();">
    <form id="form1" runat="server">
    
    <div id=divmain style="width:100% ;overflow:auto; padding-top:20px;">
         <table style="width: 100%">
            <tr>
                <td id=tdmain align=center style="width:100%; vertical-align: top;">
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 90%;">
                        <tr>
                        <td style="width:60%; vertical-align:bottom; text-align:left">
                        <table style="width:100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="left" style="width: 0%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;height: 24px">
                                AL RAJHI RESPONSE GENERATOR</td>
                            <td align="left" class="bg_body" style="width: 60%; height: 24px" valign="bottom">
                                <img  src="../images/rend.gif" /></td>
                        </tr>
                        </table>
                        </td>
                        </tr>
                        <tr>
                            <td style="width: 100%; height: 100%"> <!--colspan="3" -->
                                <table class="table_border" style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="vertical-align: top; width: 60%; height: 30%">
                                            
                                            <!-- content starts here -->
                                             <div id="divFileListing" style= "">                                           
                                                 <table id="tbl1" class="table_border" style="width: 100%; text-align: center" border="1">
                                                                                                     
                                                     <tr>
                                                        <td class="cell_caption" style="width: 20%; height: 21px" align = "right">
                                                            <label class="cell_desc">
                                                                 File Date &nbsp;&nbsp;: &nbsp;</label>
                                                        </td>
                                                        <td align="left" style="width: 80%; height: 24px; ">
                                                            <input id="txtFileDate" style="width: 30%; text-align: left"
                                                                type="text" readonly="readonly" onchange="javascript:js_Al_Rajhi.FileDate_on_change();"/>
                                                            <img alt="file date" id="calFileDate" align="absMiddle" onclick="javascript:popUpCalendar(this, document.getElementById('txtFileDate'), 'mm/dd/yyyy', document.getElementById('divmain'));"
                                                                                        src="../images/calendar/imgcalen.png" />
                                                        </td>
                                                     </tr>
                                                     <tr>
                                                         <td class="cell_caption" id="td2" style="width: 20%; height: 21px" align = "right">
                                                             <label class="cell_desc">
                                                                 File Name &nbsp;&nbsp;: &nbsp;</label></td>
                                                         <td align = "left">
                                                                <select id="ddlFileList" onchange="javascript:js_Al_Rajhi.ddlfilelist_onchange();"
                                                                 style="width: 70%">
                                                                </select>                                                         
                                                         </td>
                                                     </tr>
                                                     <tr>
                                                         <td class="cell_caption" id="td1" style="width: 20%; height: 21px" align = "right">
                                                             <label class="cell_desc">
                                                                 View &nbsp;&nbsp;: &nbsp;</label></td>
                                                         <td align = "left">
                                                                <select id="ddlView" onchange="javascript:js_Al_Rajhi.view_onchange();"
                                                                 style="width: 70%">
                                                                </select>                                                         
                                                         </td>
                                                     </tr>                                                                                                          
                                                     <tr>                                                     
                                                         <td id="tdEXCOfilelisting" style="width: 100%; height: 21px" colspan="2">
                                                         &nbsp;
                                                         </td>
                                                     </tr>
                                                     <tr>
                                                         <td style="width: 100%; height: 21px" colspan = "2">
                                                             <input id="btnSave"  onclick="javascript:js_Al_Rajhi.btnSave_click();"
                                                                 style="width: 15%" type="button" value="Save" />
                                                             <input id="btnCancel"  onclick="javascript:js_Al_Rajhi.btnCancel_onclick();"
                                                                 style="width: 15%" type="button" value="Cancel" />
                                                             <input id="btnGenerate"  onclick="javascript:js_Al_Rajhi.btnGenerate_onclick();"
                                                                 style="width: 15%" type="button" value="Generate Response" />
                                                             <asp:Button ID="Button1" runat="server" Text="Button" Width="105px" OnClick="buttonDL_Click"/>
                                                             <asp:HiddenField ID="HPath" runat="server" />
                                                         </td>    
                                                     </tr>
                                                 </table>
                                            </div>
                                            <!--</td> -->
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
