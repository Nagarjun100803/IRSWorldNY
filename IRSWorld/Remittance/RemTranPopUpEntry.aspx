<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranPopUpEntry.aspx.cs" Inherits="IRSWorld.Remittance.RemTranPopUpEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Pop Entry</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>
    <script src="../js/Remittance/jsremittersearch.js" type = "text/javascript"></script> 
    <script src="../js/Remittance/jsRemTranPopUpEntry.js" type="text/javascript"></script>   
    <script type="text/javascript">
            var RemTran_PopUpEntry = new RemTran_PopUpEntry();
    </script>           
    
</head>
<body onload="javascript:RemTran_PopUpEntry.load_all_dropdownlist();">
    <form id="form1" runat="server">
    <center>
        <p>&nbsp;</p>
        <div id="div_PurposeOthers" style="width:60%; display: block;">
            <table style="width:100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">Purpose - Others</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table cellpadding="0" cellspacing="2" class="table_body" style="width:80%; border: solid 1px skyblue">
                                        <tr>
                                            <td align="center" style="width: 100%; height: 100px;  "><textarea id="txtPopUpEntryRemPurpose" style="width: 99%; height:100px;"></textarea>
                                                <%--&nbsp;--%></td> 
                                        </tr>
                                        
                                    </table>  
                                </td> 
                            </tr> 
                            <tr>
                                <td style="height: 28px" valign="top" >
                                    <input type="button" value="Save" onclick="javascript:rem_PUOthersSave();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /> &nbsp;&nbsp;&nbsp;
                                    <input type="button" value="Cancel" onclick="javascript:rem_PUOthersCancel()" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                </td>
                            </tr>

                        </table>
                                    <!-- content ends here -->
                    </td>
                </tr>
            </table>
        </div>
    </center>            
    
    <center>
        <p>&nbsp;</p>
        <div id="divSpecialMessage" style="width:60%; display: block;">
            <table style="width:100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td  align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">Special Message</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table cellpadding="0" cellspacing="0" class="table_body" style="width:80%; border: solid 1px skyblue">
                                        <tr>
                                            <td align="center" style="width: 100%; height: 100px;  "><textarea id="txtPopUpEntryRemMessage" style="width: 99%; height:100px;"></textarea>
                                                <%--&nbsp;--%></td> 
                                        </tr>
                                    </table>  
                                </td> 
                            </tr> 
                            <tr>
                                <td style="height: 28px" valign="top" >
                                    <input type="button" value="Save" onclick="javascript:rem_PUSMSave();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /> &nbsp;&nbsp;&nbsp;
                                    <input type="button" value="Cancel" onclick="javascript:rem_PUSMCancel()" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                </td>
                            </tr>

                        </table>
                                    <!-- content ends here -->
                    </td>
                </tr>
            </table>
        </div>
    </center>            
    <center>
        <p>&nbsp;</p>
        <div id="divCheck" style="width:60%; display: block;">
            <table style="width:100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td  align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">Paymode : Check</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table cellpadding="0" cellspacing="2" class="table_body" style="width:80%; border: solid 1px skyblue">
                                        <tr>
                                           <%-- <td align="center" style="width: 100%; height: 100px;  ">--%>
                                                <%--&nbsp;--%><%--</td> --%>
                                            <td class="cell_caption" style="width:20%; height:24px;">Check No :&nbsp;</td>
                                            <td align="left" style="width: 30%; height: 24px;"> <input id="txtPopUpEntryRemCheckNo" type="text" style="width:95%; font-weight:normal;" /> </td>
                                            <td class="cell_caption" style="width:20%; height:24px;">Check Date :&nbsp;</td>
                                            <td align="left" style="width: 30%; height: 24px;"><input id="txtPopUpEntryRemCheckDate" type="text" style="width:95%; font-weight:normal";/> </td>
                                        </tr>
                                        <tr>
                                            <td class="cell_caption" style="width:20%; height:24px;"> Bank :&nbsp;</td>
                                            <td colspan=3 style="width: 80%; height: 24px;"><select id="ddlPopUpEntryRemBank" style="width:100%; font-weight:normal;"/></td>
                                        </tr>
                                    </table>  
                                </td> 
                            </tr> 
                            <tr>
                                <td style="height: 28px" valign="top" >
                                    <input type="button" value="Save" onclick="javascript:rem_PUSMSave();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /> &nbsp;&nbsp;&nbsp;
                                    <input type="button" value="Cancel" onclick="javascript:rem_PUSMCancel()" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
                                </td>
                            </tr>

                        </table>
                                    <!-- content ends here -->
                    </td>
                </tr>
            </table>
        </div>
    </center>      
    
    </form>
</body>
</html>
