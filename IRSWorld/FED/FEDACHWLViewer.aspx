<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FEDACHWLViewer.aspx.cs" Inherits="IRSWorld.FED.FEDACHWLViewer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>FEDACH Viewer</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons_min.js"                type="text/javascript"></script>
    <script src="../js/js_generic_min.js"               type="text/javascript"></script>
    <script src="../js/jscommonmethod_min.js"           type="text/javascript"></script>
    <script src="../js/jshttprequest_min.js"            type="text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"            type="text/javascript"></script>
    <script src="../js/jsgridselector_min.js"           type="text/javascript"></script>  
    <script src="../js/popcalendar.js"                  type="text/javascript"></script>    
    <script src="../js/FED/js_fedachwlviewer.js"   type="text/javascript"></script>
    <script src="../js/Remittance/jsRemExternals.js"    type="text/javascript"></script>
        
    <script type="text/javascript">
        
    var fed_viewer = new fedach_watchlist_viewer();
    var divmask;
        
    function close_detail()
    {
        hidepanel('divpresummary');
    }
    
    function hidepanel(divname)
    {
    document.getElementById(divname).style.display = "none";
    divmask.hide();
    }
    
    function showpanel(divname)
    {   
        var div = document.getElementById(divname);
        
        divmask = new maskpanel();
        divmask.show();
        
        document.getElementById(divname).style.display = "block";
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
    }
    
    function page_setup()
    {
        if (top != self)
        {
            top.frame_scroll("yes");
        }
    }
    
    function page_off()
    {
        if (top != self)
        {
            top.frame_scroll("no");
        }
    }
    
    function fixdim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmain');
                
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - 35) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth ) + 'px';
            }
        }
   
    </script>
</head>

<body onload="fixdim(); javasript:fed_viewer.view_fed_watchlist();" onresize="fixdim();">

    <form id="form1" runat="server" autocomplete="Off">
    <div id="divmain" style="padding-top: 30px;overflow:auto;width:100%;height:700px">
        <table>
            <tr>
                <td   style="width: 135px; vertical-align:top;">
                    <div id="divComplianceMenu" style="vertical-align: top">
                        &nbsp;</div>              
                </td>
                <td style="width: 115px">
            <table cellpadding="0" cellspacing="0" >
            <tr>
                <td > 
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                <img src="../images/toe.GIF" /></td>    
                            <td class="table_head_dark" style="width: 50%; height: 24px">
                                FEDACH Account Maintenance Viewer&nbsp;</td>
                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>   
                      </tr>
                    </table>    
                </td>
                    
                </tr>
                <tr>
                    <td colspan="2" style="width: 727px">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 80%; border-bottom: #336699 3px solid">
                            <tr>
                                <td style="width: 100%; height: 21px; background-color: azure;">

                                    
                                    <div id="divFEDMain" style="background-color: azure;display:block">
                                        <table id="Table2" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:800px">
                                            <tr>
                                                <td class="cell_caption" colspan="1" style=" text-align: center; width: 12%">
                                                    Account No : </td>
                                                <td style="text-align: left;width: 25%" colspan="3">
                                                    <input id="txtAccountNo"  type="text" maxlength="40"  onkeypress="javascript:fed_viewer.isNumberKey(event)" disabled="disabled"/>&nbsp;
                                                    <asp:HiddenField ID="hidSeq_id" runat="server" />
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" colspan="1" style="width: 12%; text-align: center; height: 21px;">
                                                    Beneficiary
                                                    Name : </td>
                                                <td colspan="3" style="width: 25%; text-align: left; height: 21px;">
                                                    <input id="txtName"  type="text" style="width: 359px" maxlength="500" disabled="disabled" />&nbsp;
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4" style="height: 24px">
                                                    <input id="btnAdd" class="button" onclick="javascript:fed_viewer.toggle_buttons(this.value,this.id)" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Add" style="width: 100px"/>
                                                    <input id="btnEdit" class="button" onclick="javascript:fed_viewer.toggle_buttons(this.value,this.id)" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Edit" style="width: 100px"/>
                                                    <input id="btnCancel" class="button" onclick="javascript:fed_viewer.toggle_buttons(this.value,this.id)" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Cancel" style="width: 100px" disabled="disabled"/>
                                                    <input id="btnDelete" class="button" onclick="javascript:fed_viewer.toggle_buttons(this.value,this.id)" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Delete" style="width: 100px" disabled="disabled"/>
                                                    <input id="btnSearch" class="button" onclick="javascript:fed_viewer.proc_search(this.value)" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search" style="width: 100px"/></td>
                                            </tr>
                                        </table>
                                    </div>
                                    
                                 
                                   <div id="FedViewer" style="background-color: azure">
                                   <table id="Table1" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid; width:800px">
                                            <tr>
                                                    <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                    height: 26px; text-align: left">
                                                        Search Results</td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px; text-align: center "colspan="5">
                                                <div id="divfedwlgrid">
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
