<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountingEntries.aspx.cs" Inherits="IRSWorld.GEFUUpload.AccountingEntries" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Accounting Entries</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css"/>
    <script src = "../js/GEFUUpload/js_accountingentries.js" type ="text/javascript"></script>
    <script type="text/javascript">
        
        var accountingentries = new accountingentries();
        
        function load_main()
        {
            if (top==self)
            {
                document.irs_loader.submit();
            }
        }
        
    
    </script>
    
</head>
<body onload = "javascript:load_main();">
    <center>
        <form id="form1" runat="server">
            <table id="AccountingEntries_table">
                <tr>
                    <td style="height: 470px">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="height: 307px">
                                    <div id="AccountingEntries_div" >                                              
                                        <table cellpadding="0" cellspacing="0" style="width:750px">
                                            <tr> <!-- header -->
                                                <td align="left" valign="bottom" style="width: 11px; height:24px; border-collapse:collapse">
                                                    <img src="../images/toe.GIF" alt=""/></td>      
                                                <td class="table_head_dark" style="width:50%; height:24px">
                                                    Accounting Entries</td>                                                
                                                <td align="left" valign="bottom" style="width:100%; height:24px">
                                                    <img align="bottom" src="../images/rend.gif" alt=""/></td>
                                            </tr>
                                            <tr><!-- main body -->
                                                <td colspan="4">
                                                    <table cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure;">
                                                            <tr>                                                    
                                                                <td class="cell_caption"                    style="width: 15%; height:24px">Transaction Source:</td>
                                                                <td align="left"                            style="width: 13%; height: 24px">
                                                                    <select id="ddTransaction_Source"       style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select></td>
                                                                <td class="cell_caption"                    style="width: 15%; height:24px">Transaction Currency:</td>
                                                                <td align="left"                            style="width: 13%; height: 24px">
                                                                    <select id="ddTransaction_Currency"     style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select></td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"                    style="width: 15%; height:24px">Transaction GL Code:</td>
                                                                <td align="left"                            style="width: 13%; height: 24px">
                                                                   <input id="txtTransaction_GL_Code"       style="text-align: left; width: 97%;" type="text" /></td>
                                                                <td class="cell_caption"                    style="width: 15%; height:24px">Transaction Description:</td>
                                                                <td align="left"                            style="width: 13%; height: 24px">
                                                                    <input id="txtTranxsaction_Description" style="text-align: left; width: 97%;" type="text"/></td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"                    style="width: 15%; height:24px">Transaction Status:</td>
                                                                <td align="left"                            style="width: 13%; height: 24px">
                                                                    <select id="ddTransaction_Status"       style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                                <td class="cell_caption"                    style="width: 15%; height:24px">Receipt Mode:</td>
                                                                <td align="left"                            style="width: 13%; height: 24px">
                                                                    <select id="ddReceipt_Mode"             style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Service Charge GL Code:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <input id="txtService_Charge_GLCode"        style="text-align: left; width: 97%;" type="text" /></td>
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Service Charge Description:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <input id="txtService_Charge_Description"   style="text-align: left; width: 97%;" type="text" /></td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Implementation Type:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <select id="ddImplementation_type"          style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Implementation Currency:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <select id="ddImplementation_Currency"      style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Implementation GL Code:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <input id="txtImplementation_GL_Code"       style="text-align: left; width: 97%;" type="text"  /></td>
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Implementation Description:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <input id="txtImplementation_Description"   style="text-align: left; width: 97%;" type="text"/></td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">PRX GL Code:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <input id="txtPRX_GL_Code"                  style="text-align: left; width: 97%;" type="text" /></td>
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Location Code:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <select id="ddLocation_Code"                style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">Negotiating Branch Code:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <select id="ddNegotiating_Branch_Code"      style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                                <td class="cell_caption"                        style="width: 15%; height:24px">MIS Code:</td>
                                                                <td align="left"                                style="width: 13%; height: 24px">
                                                                    <input id="txtMIS_Code"                     style="text-align: left; width: 97%;" type="text"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption"    style="width: 15%; height:24px">Enable Other GL Code:</td>
                                                                <td style="width: 100px; text-align: left;">
                                                                    <input type="checkbox" id="chkEnable_Other_GL_Code" enabletheming="True" style="width: 22%"/></td>  
                                                            </tr>
                                                    </table>
                                                </td>
                                            </tr> <!-- second body -->
                                            <tr>
                                                <td colspan="4">
                                                    <table cellpadding="0" cellspacing="5"    style="border: solid 1px #336699; background:azure;">
                                                        <tr>                                                    
                                                            <td class="cell_caption"            style="width: 15%; height:24px">OPE GL Code:</td>
                                                            <td align="left"                    style="width: 13%; height: 24px">
                                                                <input id="txtOPE_GL_Code" type="text"   style="text-align: left; width: 97%;" /></td>
                                                            <td class="cell_caption"            style="width: 15%; height:24px">OPE Description:</td>
                                                            <td align="left"                    style="width: 13%; height: 24px">
                                                                <input id="txtOPE_Description" type="text"  style="text-align: left; width: 97%;" /></td>
                                                        </tr>
                                                        <tr>                                                    
                                                            <td class="cell_caption" style="width: 15%; height:24px">Doc Stamp GL Code:</td>
                                                            <td align="left" style="width: 13%; height: 24px">
                                                                <input id="txtDoc_Stamp_GL_Code" type="text" style="text-align: left; width: 97%;" /></td>
                                                            <td class="cell_caption" style="width: 15%; height:24px">Doc Stamp Description:</td>
                                                            <td align="left" style="width: 13%; height: 24px">
                                                                <input id="txtDoc_Stamp_Description" type="text" style="text-align: left; width: 97%;" /></td>
                                                        </tr>
                                                        <tr>                                                    
                                                            <td class="cell_caption" style="width: 15%; height:24px">Local Cable GL Code:</td>
                                                            <td align="left" style="width: 13%; height: 24px">
                                                                <input id="txtLocal_Cable_GL_Code" type="text" style="text-align: left; width: 97%;" /></td>
                                                            <td class="cell_caption" style="width: 15%; height:24px">Local Cable Description:</td>
                                                            <td align="left" style="width: 13%; height: 24px">
                                                                <input id="txtLocal_Cable_Description" type="text" style="text-align: left; width: 97%;" /></td>
                                                        </tr>
                                                        <tr>                                                    
                                                            <td class="cell_caption" style="width: 15%; height:24px">Local Service Fee GL Code:</td>
                                                            <td align="left" style="width: 13%; height: 24px">
                                                                <input id="txtLocal_Service_Fee_GL_Code" type="text" style="text-align: left; width: 97%;" /></td>
                                                            <td class="cell_caption" style="width: 15%; height:24px">Local Service Description:</td>
                                                            <td align="left" style="width: 13%; height: 24px">
                                                                <input id="txtLocal_Service_Description" type="text" style="text-align: left; width: 97%;" /></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <input id = "txtHiddenSeqID" type="hidden" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure; width:100%">
                                                        <tr>
                                                            <td style="width: 150px"> </td>
                                                            <td ><input id="btnFirst"       class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="First"       style="width: 100px"/></td>
                                                            <td ><input id="btnPrevious"    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Previous"    style="width: 100px" /></td>
                                                            <td ><input id="txtDataNumber"                                                                                                          type="text"                       style="text-align:center; width: 100px;" /></td>
                                                            <td ><input id="btnNext"        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Next"        style="width: 100px" /></td>
                                                            <td ><input id="btnLast"        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Last"        style="width: 100px" /></td>
                                                            <td style="width: 150px"> </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table id = "tblbuttons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background: azure; width:100%">
                                                        <tr>
                                                            <td style="width: 150px"> </td>
                                                            <td style="height: 22px" ><input id="btnAdd"        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add"    style="width: 100px" /></td>
                                                            <td style="height: 22px" ><input id="btnEdit"       class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit"    style="width: 100px" /></td>
                                                            <td style="height: 22px" ><input id="btnDelete"     class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Delete"    style="width: 100px" /></td>
                                                            <td style="height: 22px" ><input id="btnClose"      class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Close"    style="width: 100px" /></td>
                                                            <td style="width: 150px"> </td>
                                                        </tr>
                                                    </table>
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
        </form>
    </center>
    <form name="irs_loader" id="irs_loader" method ="post" action="../Main/IRSMain.aspx?load=../GEFUUpload/AccountingEntries.aspx" target="_parent" style="overflow:auto"></form>
</body>
</html>
