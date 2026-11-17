<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GEFUMaintenance.aspx.cs" Inherits="IRSWorld.GEFUUpload.GEFUMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GEFU</title>
    <link href="../css/Default.css"                         type="text/css" rel="stylesheet"/>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../js/GEFUUpload/js_gefumaintenance.js"    type="text/javascript"></script>
    <script src="../js/GEFUUpload/js_accountingentries.js"  type="text/javascript"></script>
    <script src="../js/GEFUUpload/js_iflexaccounts.js"      type="text/javascript"></script>
    <script src="../js/GEFUUpload/js_BookingRates.js"       type="text/javascript"></script>
    <script src="../js/GEFUUpload/js_GEFU_Reports.js"       type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>
    <script src="../js/popcalendar.js"                      type="text/javascript"></script>
    <script type="text/javascript">
        var gefuMaintenance = new gefu_maintenance();                                         


    </script>    
</head>
    <body onload="javascript:gefuMaintenance.show_div('div_AccountingEntries');">
        <form id="form1" runat="server">
            <div style="padding-top:30px">
                <table id="tblGEFUMaintenance" style="width:100%">
                    <tr>
                        <td id="tdGEFUMenu" style="width:20%; height:52px; vertical-align:top;">
                            <table style="width:100%">
                                <tr>
                                    <td style="text-align:right; border-bottom:solid 1px #5697c1;">
                                        <a id="aAccountingEntries" href="javascript:gefuMaintenance.show_div('div_AccountingEntries');" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">Accounting Entries</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right; border-bottom:solid 1px #5697c1;">
                                        <a id="aIflexAccounts" href="javascript:gefuMaintenance.show_div('div_IflexAccounts');"         style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">Iflex Accounts</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right; border-bottom:solid 1px #5697c1;">
                                        <a id="aBookingRates" href="javascript:gefuMaintenance.show_div('div_BookingRates');"           style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">Booking Rates</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:right; border-bottom:solid 1px #5697c1;">
                                        <a id="aGEFU" href="javascript:gefuMaintenance.show_div('div_GEFU_Reports');"                   style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">GEFU and Reports</a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="center" id="tdGEFUMainTable" style="width:77%; vertical-align:top;">
                            <table style="overflow:auto">
                                <tr>
                                    <td style="width: 753px">                              
                                        <div id="div_AccountingEntries" style="display:none; overflow:auto; height:550px;">                                              
                                            <table cellpadding="0" cellspacing="0" style="width:750px">
                                                <tr> <!-- header -->
                                                <td style="width:60%; vertical-align:bottom" align=left>
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                    <td align="left" valign="bottom"    style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom">
                                                        <img src="../images/toe.GIF" alt=""/></td>      
                                                    <td class="table_head_dark"         style="width:40%; height:24px">Accounting Entries</td>                                                
                                                    <td align="left" valign="bottom"    style="width:60%; height:24px">
                                                        <!--img align="bottom" src="../images/rend.gif" alt=""/></td-->
                                                        <img src="../images/rend.gif" alt=""/></td>
                                                         
                                                </tr>
                                                </table>
                                                </td>
                                                </tr>
                                                <tr><!-- main body -->
                                                    <td> <!--colspan="3">-->
                                                        <table id="tblAccountingEntries" style="border:solid 3px #336699; background:azure;">
                                                            <tr>                                                    
                                                                <td class="cell_caption"            style="width:10%; height:24px">Negotiating Branch Code:</td>
                                                                <td align="left"                    style="width:10%; height:24px; color:Red ">
                                                                    <select id="ddnegbranchcode"    style="width:94%" disabled="disabled" onchange="javascript:gefuMaintenance.get_locationcode_by_negbranchcode()">
                                                                    </select>*
                                                                </td>
                                                                <td class="cell_caption"            style="width:10%; height:24px">Location Code:
                                                                </td>
                                                                <td align="left"                    style="width:10%; height:24px; color:red">
                                                                    <select id="ddlocationcode"     style="width:94%" disabled="disabled" onchange="javascript:gefuMaintenance.get_branch_sub_lookups()">
                                                                    </select>*
                                                                </td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"        style="width:10%; height:24px">Transaction Source:</td>
                                                                <td align="left"                style="width:10%; height:24px; color:red">
                                                                    <select id="ddTranxSource"  style="width:94%" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>*
                                                                </td>         
                                                                <td class="cell_caption"        style="width:10%; height:24px">Receipt Mode:</td>
                                                                <td align="left"                style="width:10%; height:24px; color:Red ">
                                                                    <select id="ddReceiptMode"  style="width:94%" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>*
                                                                </td>
                                                            </tr>
                                                            <tr>     
                                                                <td class="cell_caption"                    style="width:10%; height:24px">Remitting Currency:</td>
                                                                <td align="left"                            style="width:10%; height:24px; color:red">
                                                                    <select id="ddTranxCurrency"            style="width:94%" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>*
                                                                </td>
                                                                <td class="cell_caption"                    style="width:10%; height:24px">Account Currency:</td>
                                                                <td align="left"                            style="width:10%; height:24px; color:Red ">
                                                                    <select id="ddImplementationCurrency"   style="width:94%" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>*
                                                                </td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"            style="width:10%; height:24px">Transaction GL Code:</td>
                                                                <td align="left"                    style="width:10%; height:24px; color:red">
                                                                   <input id="txtTranxGLCode"       style="text-align:left; width:92%;"   type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();" />*
                                                                </td>
                                                                <td class="cell_caption"                style="width:10%; height:24px">Doc Stamp GL Code:</td>
                                                                <td align="left"                        style="width:10%; height:24px">
                                                                    <input id="txtDocStampGLCodes"      style="text-align:left; width:92%;" type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();"/>
                                                                </td>
                                                            </tr>                                                         
                                                            <tr>                                                    
                                                                <td class="cell_caption"                        style="width:10%; height:24px;">Implementation GL Code:</td>
                                                                <td align="left"                                style="width:10%; height:24px; color:Red">
                                                                    <input id="txtImplementationGLCode"         style="text-align:left; width:92%;"   type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();"/>*
                                                                </td>
                                                                <td class="cell_caption"            style="width:10%; height:24px">OPE GL Code:</td>
                                                                <td align="left"                    style="width:10%; height:24px">
                                                                    <input id="txtOPEGLCode"        style="text-align:left; width:92%;"   type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();"/>
                                                                </td>                                                                    
                                                            </tr>
                                                            <tr>  
                                                                <td class="cell_caption"                    style="width:10%; height:24px">Service Charge GL Code:</td>
                                                                <td align="left"                            style="width:10%; height:24px; color:Red">
                                                                    <input id="txtServiceChargeGLCode"      style="text-align:left; width:92%;"   type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();"/>* 
                                                                </td>
                                                                <td class="cell_caption"                        style="width:10%; height:24px">Local Cable GL Code:</td>
                                                                <td align="left"                                style="width:10%; height:24px">
                                                                    <input id="txtLocalCableChargesGLCode"      style="text-align:left; width:92%;" type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();"/></td>                                                                                                                                   
                                                            </tr>                                                            
                                                            <tr>   
                                                                <td class="cell_caption"    style="width:10%; height:24px">PRX GL Code:</td>
                                                                <td align="left"            style="width:10%; height:24px; color:Red">
                                                                    <input id="txtPRXGLCode"style="text-align:left; width:92%;" type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();"/>*
                                                                </td>                                                   
                                                                <td class="cell_caption"                        style="width:10%; height:24px">Local Service Fee GL Code:</td>
                                                                <td align="left"                                style="width:10%; height:24px">
                                                                    <input id="txtLocalServiceFeeGLCode"        style="text-align:left; width:92%;" type="text" onkeypress="javascript:gefuMaintenance.check_gl_codes();"/></td>                                                            
                                                            </tr>                                                    
                                                            <tr>                                                              
                                                                <td class="cell_caption"    style="width:10%; height:22px">MIS Code:</td>
                                                                <td align="left"            style="width:10%; height:22px">
                                                                    <input id="txtMISCode"  style="text-align:left; width:92%;" type="text" onkeypress="javascript:gefuMaintenance.check_alpha()"/>
                                                                </td>
                                                                <td class="cell_caption"    style="width:15%; height:22px" colspan="2">Fields with asterisk sign (*) are required</td>
                                                                 
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table id = "tblAccountingEntriesButtons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%; " >
                                                                        <tr>
                                                                        
                                                                            <td style="height:22px" ><input id="btnAddAccountingEntries"        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add"      style="width:100px" onclick="javascript:gefuMaintenance.insert_record();"/></td>
                                                                            <td style="height:22px" ><input id="btnEditAccountingEntries"       class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit"     style="width:100px" onclick="javascript:gefuMaintenance.update_record();"/></td>
                                                                            <td style="height:22px" ><input id="btnSaveAccountingEntries"       class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save"     style="width:100px" disabled="disabled" onclick="javascript:gefuMaintenance.commit_record();" /></td>
                                                                            <td style="height:22px" ><input id="btnDeleteAccountingEntries"     class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Delete"   style="width:100px" disabled="disabled" onclick="javascript:gefuMaintenance.delete_record();" /></td>
                                                                            <td style="height:22px" ><input id="btnCancelAccountingEntries"     class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Cancel"   style="width:100px" disabled="disabled" onclick="javascript:gefuMaintenance.rollback_record();" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>  
                                                            <tr>
                                                                <td colspan="4">
                                                                    <div id="divtbl_AccountingEntries" style="text-align:center; width:100%" >
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr> <!-- Search Criteria-->
                                                                <td colspan="4">    
                                                                    <table id= "tblAccountingEntriesSearch" style="width:100%">
                                                                        <tr> <!--Search Button -->
                                                                            <td colspan="4" class="table_head_dark"     style="width:100%; height:24px">Select on the following list to filter the Accounting Entries</td>
                                                                        </tr>  
                                                                        <tr>                                                    
                                                                            <td class="cell_caption"                    style="width:10%; height:24px">Negotiating Branch Code:</td>
                                                                            <td align="left"                            style="width:10%; height:24px; color:Red ">
                                                                                <select id="ddSearchNegBranchCode"      style="width:94%" disabled="disabled" onchange="javascript:gefuMaintenance.get_locationcode_by_negbranchcode()">
                                                                                </select>
                                                                            </td>
                                                                            <td class="cell_caption"                    style="width:10%; height:24px">Location Code:
                                                                            </td>
                                                                            <td align="left"                            style="width:10%; height:24px; color:red">
                                                                                <select id="ddSearchLocationCode"       style="width:94%" disabled="disabled" onchange="javascript:gefuMaintenance.get_branch_sub_lookups()">
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>                                                    
                                                                            <td class="cell_caption"                    style="width:10%; height:24px">Transaction Source:</td>
                                                                            <td align="left"                            style="width:10%; height:24px; color:red">
                                                                                <select id="ddSearchTransactionSource"  style="width:94%" disabled="disabled">
                                                                                    <option selected="selected"></option>
                                                                                </select>
                                                                            </td>         
                                                                            <td class="cell_caption"                    style="width:10%; height:24px">Receipt Mode:</td>
                                                                            <td align="left"                            style="width:10%; height:24px; color:Red ">
                                                                                <select id="ddSearchReceiptMode"        style="width:94%" disabled="disabled">
                                                                                    <option selected="selected"></option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>     
                                                                            <td class="cell_caption"                    style="width:10%; height:24px">Remitting Currency:</td>
                                                                            <td align="left"                            style="width:10%; height:24px; color:red">
                                                                                <select id="ddSearchRemittingCurrency"  style="width:94%" disabled="disabled">
                                                                                    <option selected="selected"></option>
                                                                                </select>
                                                                            </td>
                                                                            <td class="cell_caption"                    style="width:10%; height:24px">Account Currency:</td>
                                                                            <td align="left"                            style="width:10%; height:24px; color:Red ">
                                                                                <select id="ddSearchAccountCurrency"    style="width:94%" disabled="disabled">
                                                                                    <option selected="selected"></option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>                                           
                                                        </table>
                                                    </td>
                                                </tr> <!-- second body -->                                       
                                                <tr>
                                                    <td colspan="4">
                                                        <br />
                                                        <input id = "txtHidden_Accountingentries_SEQ_ID" type="hidden" />
                                                    </td>
                                                </tr>
                                            </table>                                        
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 753px">
                                        <div id="div_IflexAccounts" style="display:none; overflow:auto; height:550px;">
                                            <table cellpadding="0" cellspacing="0"      style="width:500px" >
                                                <tr> <!-- header -->
                                                <td style="width:60%; vertical-align:bottom" align=left>
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom"    style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom">
                                                        <img src="../images/toe.GIF" alt=""/></td>      
                                                    <td class="table_head_dark"         style="width:40%; height:24px">Iflex Accounts</td>                                                
                                                    <td align="left" valign="bottom"    style="width:60%; height:24px">
                                                        <!--img align="bottom" src="../images/rend.gif" alt=""/></td-->
                                                        <img src="../images/rend.gif" alt=""/></td>
                                                </tr>
                                                </table>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <td colspan ="3" style="height: 110px">
                                                        <table id="tblIflexAccounts" style="background:azure; border:solid 3px #336699; width:100%">
                                                            <tr>
                                                                <td class="cell_caption"        style="width:25%; height:24px">IFlex Account:</td>
                                                                <td align="left"                style="width:50%; height:24px">
                                                                    <input id="txtIflexAccount" style="text-align:left; width:97%;"   type="text" disabled="disabled"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption"        style="width:50%; height:24px">Description:</td>
                                                                <td align="left" style="width:50%; height:24px">
                                                                    <input id="txtDescription"  style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:gefuMaintenance.check_alpha()"/>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption"        style="width:50%; height:24px">GL Code Type:</td>
                                                                <td align="left"                style="width:50%; height:24px">
                                                                    <input id="txtGLType"       style="text-align:left; width:97%;"   type="text" disabled="disabled" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table id = "tblIFlexAccountsButtons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%">
                                                                        <tr>
                                                                            <td style="height:22px" ><input id="btnEditIflexAccounts"   class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" style="width:100px"     onclick="javascript:gefuMaintenance.update_record();"/></td>
                                                                            <td style="height:22px" ><input id="btnSaveIflexAccounts"   class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" style="width:100px"     onclick="javascript:gefuMaintenance.commit_record();" disabled="disabled"/></td>
                                                                            <td style="height:22px" ><input id="btnCancelIflexAccounts" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Cancel"style="width:100px"    onclick="javascript:gefuMaintenance.rollback_record();" disabled="disabled"/></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">                                                                    
                                                                    <div id="divtbl_iflexaccounts" style="text-align:center; width:100%" >
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>                                                
                                                </tr>                                      
                                                <tr>
                                                    <td colspan="4">
                                                        <br />
                                                        <input id = "txtHidden_iflexaccounts_SEQ_ID" type="hidden" />
                                                    </td>
                                                </tr>
                                            </table>      
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 753px" >
                                        <div id="div_BookingRates" style="display:none; overflow:auto; height:550px;">
                                            <table cellpadding="0" cellspacing="0"      style="width:700px;" >
                                                <tr> <!-- header -->
                                                <td style="width:60%; vertical-align:bottom" align=left>
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" valign="bottom"    style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom">
                                                        <img src="../images/toe.GIF" alt=""/></td>      
                                                    <td class="table_head_dark"         style="width:40%; height:24px">Booking Rates</td>                                                
                                                    <td align="left" valign="bottom"    style="width:60%; height:24px">
                                                        <img align="bottom" src="../images/rend.gif" alt="" /></td>
                                                </tr>
                                                </table>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <td> <!--colspan= "3"-->
                                                        <table id="tblBookingRates" style=" width:100%; background:azure; border:solid 3px #336699;">                                            
                                                            <tr>
                                                                <td class="cell_caption"        style="width:25%; height:24px">Source Currency:</td>
                                                                <td align="left"                style="width:13%; height:24px">
                                                                    <select id="ddSourceCcy"    style="width:100%" onchange ="javascript:gefuMaintenance.check_currencies(document.getElementById('ddSourceCcy').value, document.getElementById('ddTargetCcy').value);" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                                <td class="cell_caption"        style="width:25%; height:24px">Target Currency</td>
                                                                <td align="left"                style="width:13%; height:24px">
                                                                    <select id="ddTargetCcy"    style="width:100%" onchange ="javascript:gefuMaintenance.check_currencies(document.getElementById('ddSourceCcy').value, document.getElementById('ddTargetCcy').value);" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="tdSourceToTargetRate" class="cell_caption"                style="width:25%; height:24px">1 Ccy to Ccy:</td>
                                                                <td align="left"                        style="width:25%; height:24px">
                                                                    <input id="txtSourceToTargetRate"   style="width:97%; text-align:left;" type="text" onkeypress="javascript:gefuMaintenance.check_booking_rates()" onkeyup="javascript:gefuMaintenance.get_bookingrates(document.getElementById('txtSourceToTargetRate').value);" disabled="disabled" />
                                                                </td>
                                                                <td id="tdTargetToSourceRate" class="cell_caption"                style="width:25%; height:24px">1 Ccy to Ccy:</td>
                                                                <td align="left"                        style="width:25%; height:24px">
                                                                    <input id="txtTargetToSourceRate"   style="width:97%; text-align:left;" type="text" disabled="disabled" readonly="readOnly"/>
                                                                </td>
                                                            </tr>  
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table id = "tblBookingRatesButtons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%" >
                                                                        <tr>
                                                                            <td style="height:22px" ><input id="btnAddBookingRates"     class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add"      style="width:100px" onclick="javascript:gefuMaintenance.insert_record();"/></td>
                                                                            <td style="height:22px" ><input id="btnSaveBookingRates"    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save"     style="width:100px" onclick="javascript:gefuMaintenance.commit_record();" disabled="disabled"/></td>
                                                                            <td style="height:22px" ><input id="btnDeleteBookingRates"    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Delete"     style="width:100px" onclick="javascript:gefuMaintenance.delete_record();" /></td>
                                                                            <td style="height:22px" ><input id="btnCancelBookingRates"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Cancel"   style="width:100px" onclick="javascript:gefuMaintenance.rollback_record();" disabled="disabled"/></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <br />
                                                                    <input id = "txtHidden_bookingrates_SEQ_ID" type="hidden" />
                                                                </td>
                                                            </tr>                                                
                                                            <tr> <!--filterby row-->
                                                                <td colspan="4">
                                                                    <table style="width:100%; ">
                                                                        <tr>
                                                                            <td style="width:25%"></td>
                                                                            <td id ="tdEffectivityDate" class="cell_caption" style="width:25%" align="right"> View by Effectivity Date :
                                                                            </td>
                                                                            <td>
                                                                                <img src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtEffectivityDate, 'mm/dd/yyyy');"/>
                                                                            </td>
                                                                            <td style="width:25%">
                                                                                <input id="txtEffectivityDate" style="width:98%" onchange="javascript:gefuMaintenance.load_bookingrates_grid_viewer(this.value)"/>                                                                                                                                          
                                                                            </td>
                                                                            <td style="width:25%"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>  
                                                            <tr>
                                                                <td colspan = "4">
                                                                    <div id="divtbl_bookingrates" style="text-align:center; width:100%" >
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
                                <tr>
                                    <td style="width: 900px">
                                        <div id="div_GEFU_Reports" style="display:none; overflow:auto; height:550px;">
                                            <table cellpadding="0" cellspacing="0"          style="width:600px" >
                                                <tr> <!-- header -->
                                                <td style="width:60%; vertical-align:bottom" align=left>
                                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left"    valign="bottom"    style="width:0%; height:24px; border-collapse:collapse; vertical-align:bottom">
                                                        <img src="../images/toe.GIF" alt="" /></td>      
                                                    <td class="table_head_dark"             style="width:40%; height:24px">GEFU and Reports</td>                                                
                                                    <td align="left"    valign="bottom"    style="width:60%; height:24px">
                                                        <img align="bottom" src="../images/rend.gif" alt=""/></td>
                                                </tr>
                                                </table>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <td style="height:19px;"> <!--colspan= "3" -->
                                                        <table id="tblGEFU" style="background:azure; border:solid 3px #336699; width:100%;">                                            
                                                            <tr>
                                                                <td colspan ="2" align="left"   style="width:13%; height:24px">
                                                                    <select id="ddReportName"   style="width:100%" onchange ="javascript:gefuMaintenance.check_report_name(document.getElementById('ddReportName').value)">
                                                                        <option value="1">GEFU Upload File</option>
                                                                        <option value="2">Summary</option>                                                                        
                                                                        <option value="3">Preliminary Summary</option>
                                                                        <option value="4">Consolidated Summary</option>
                                                                        <option value="5">Consolidated Summary (Online and Non-online)</option>  
                                                                        <option value="6">Consolidated Summary (by GL Codes)</option>                                                                      
                                                                        <option value="7">Consolidated Summary (by GL Codes) - (Online and Non-online)</option>
                                                                        <option value="8">Transaction Details by Receipt Mode</option>
                                                                        <option value="9">Transaction Details by Transaction Source</option>
                                                                        <option value="10">Transactions not included in GEFU File</option>
                                                                        <option value="11">Booking Rates Used</option>        
                                                                        <option value="12">Accounting Entries Combination</option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width:50%; height:76px;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td class="cell_caption"        style="height:24px; text-align:center">Transaction Source</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width:100%">
                                                                                <select id="ddTransactionSource" size="5" multiple="multiple" style="width:100%">                                                                              
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>                                                                
                                                                <td style="width:50%; height:76px">
                                                                    <table id="tblTranx_Status_Date" style="width:100%;">
                                                                        <tr>
                                                                            <td class="cell_caption"        style="height:24px; text-align:center" colspan="3">Transaction Status</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left"                style="width:13%; height:24px" colspan="3">
                                                                                <select id="ddTranxStatus"  style="width:100%" onchange ="javascript:gefuMaintenance.enable_process_button()">
                                                                                    <option value=""></option>
                                                                                    <option value ="Credited Online">Credited Online</option>                         
                                                                                    <option value="Processed by GRS">Processed by GRS</option>
                                                                                </select>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption"        style="height:24px; text-align:center" colspan="3">Transaction Date</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width:30%">
                                                                                <img src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtTranxdate, 'mm/dd/yyyy');"/>
                                                                            </td>
                                                                            <td style="width:70%">
                                                                                <input id="txtTranxdate" style="width:98%;" onchange="javascript:gefuMaintenance.load_bookingrates_grid_viewer(this.value)"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="4">
                                                                    <table id = "tblGEFUButtons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%">
                                                                        <tr>
                                                                            <td style="height:22px" >
                                                                                <input id="btnProcess"    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Process"  style="width:100px" onclick="javascript:gefuMaintenance.create_report();"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan = "3">
                                                                    <table id="tblReports" style="width:100%">
                                                                        <tr>
                                                                            <td class="cell_caption"        style="width:30%; text-align:center" >Reports Extract Date</td>                                                                            
                                                                            <td style="width:10%">
                                                                                <img src="../images/calendar/imgcalen.png" alt="" align="middle" onclick="popUpCalendar(this, document.forms[0].txtReportsDate, 'mm/dd/yyyy');"/>
                                                                            </td>
                                                                            <td style="width:30%">
                                                                                <input id="txtReportsDate" style="width:98%" />                                                                            
                                                                            </td>         
                                                                            <td style="height:30%" >
                                                                                <input id="btnSearchReports"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search Reports"    style="width:100px" onclick="javascript:gefuMaintenance.searchreports();" />
                                                                            </td>                                                                   
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="4">
                                                                                <div id="divReportsDownload" style="text-align:center; width:100%; display:none">
                                                                                    <table id="tblReportsDownload" style="width:100%">
                                                                                    </table>
                                                                                </div>                                                                                
                                                                            </td>
                                                                        </tr>
                                                                    </table> 
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan = "3">
                                                                    <div id="divIncAccnt" style="text-align:center; width:100%; display:none" >
                                                                    </div>                                                                                                                                        
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan = "3">
                                                                    <div id="divGEFUBookingRates" style="text-align:center; width:100%" >                                                                        
                                                                    </div>
                                                                    <div id="divGEFUBookingRatesView">
                                                                        <table id="tblGEFUBookingRatesView" style="width:100%">
                                                                            <tr>
                                                                                <td id="td1" class="cell_caption" style="width:25%; height:24px">Verified By:</td>
                                                                                <td align="left" style="width:25%; height:24px">
                                                                                    <input id="txtVerifier"   style="width:97%; text-align:left;" type="text"/>
                                                                                </td>                                                            
                                                                            </tr>  
                                                                            <tr>
                                                                                <td id="td2" class="cell_caption" style="width:25%; height:24px">Authorized By:</td>
                                                                                <td align="left" style="width:25%; height:24px">
                                                                                    <input id="txtAuthorizer"   style="width:97%; text-align:left;" type="text"/>
                                                                                </td>                                                            
                                                                            </tr>
                                                                        </table> 
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan = "3">
                                                                    <div id="divIncBookingRAtes" style="text-align:center; width:100%" >
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
                        </td>
                    </tr>
                </table> 
            </div>
        </form>
    </body>
</html>
