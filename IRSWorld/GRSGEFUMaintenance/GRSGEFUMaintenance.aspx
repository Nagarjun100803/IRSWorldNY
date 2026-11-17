<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GRSGEFUMaintenance.aspx.cs" Inherits="IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>GRS GEFU Maintenance</title>
    <link href="../css/Default.css"                                     type="text/css" rel="stylesheet"/>
    <script src="../js/jscommonmethod.js"                               type="text/javascript"></script>
    <script src="../js/jsCommons.js"                                    type="text/javascript"></script>
    <script src="../js/jsgridselector.js"                               type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                                type="text/javascript"></script>
    <script src="../js/popcalendar.js"                                  type="text/javascript"></script>
    <script src="../js/GRSGEFUMaintenance/js_grsgefumaintenance.js"     type="text/javascript"></script>
    <script src="../js/GRSGEFUMaintenance/js_GRSAccountingEntries.js"   type="text/javascript"></script>
    <script src="../js/GRSGEFUMaintenance/js_grsCorrBank.js"            type="text/javascript"></script>
    <script src="../js/GRSGEFUMaintenance/js_GRSBuyingRate.js"            type="text/javascript"></script>
    <script type="text/javascript" >
    
        var grsGEFUMaintenance = new GRSGEFUMaintenance();
        
    </script>   
</head>
<body onload="javascript:grsGEFUMaintenance.show_div('div_GRS_AccountingEntries');">
    <form id="form1" runat="server">
        <div style="padding-top: 30px;">
            <table>
                <tr>
                    <td id="tdGRSGEFU" style="width:20%; height:52px; vertical-align:top;">
                        <table style="width:100%">
                            <tr>
                                <td style="text-align:right; border-bottom:solid 1px #5697c1;">
                                    <a id="aGRSAccountingEntries" href="javascript:grsGEFUMaintenance.show_div('div_GRS_AccountingEntries');" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">GRS Accounting Entries</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right; border-bottom:solid 1px #5697c1; height: 22px;">
                                    <a id="aGRSCorrBank" href="javascript:grsGEFUMaintenance.show_div('div_GRS_CorrBank');" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">GRS CorrBank</a>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right; border-bottom:solid 1px #5697c1; height: 22px;">
                                    <a id="aGRSBuyingRate" href="javascript:grsGEFUMaintenance.show_div('div_GRS_BuyingRates');" style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">GRS Buying Rate</a>
                                </td>
                            </tr>
                    <%--        <tr>
                                <td style="text-align:right; border-bottom:solid 1px #5697c1;">
                                    <a id="aGRSGEFU" href="javascript:grsGEFUMaintenance.show_div('div_GRS_GEFU');"style="font-weight:bold; font-size:12px; font-style:italic; font-family:Arial;">GEFU Extraction</a>
                                </td>
                            </tr>--%>
                        </table>
                    </td>
                    <td align="center" id="tdGRSGEFUMainTable" style="width:77%; vertical-align:top; height:52px;">
                        <div id="div_GRS_AccountingEntries" style="display:none; overflow:auto; height:400px">                                              
                            <table cellpadding="0" cellspacing="0" style="width:700px">
                                <tr> <!-- header -->
                                <td style="width: 85%; vertical-align: bottom; text-align:left" align="left">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom"    style="width:0%; height:24px; border-collapse:collapse">
                                        <img src="../images/toe.GIF"/></td>      
                                    <td class="table_head_dark"         style="width:40%; height:24px">GRS Accounting Entries</td>                                                
                                    <td align="left" valign="bottom"    style="width:60%; height:24px">
                                        <img align="bottom" src="../images/rend.gif" /></td>
                                </tr>
                                </table>
                                </td>
                                </tr>
                                <tr><!-- main body -->
                                    <td colspan="3">
                                        <table id="tblGRSAccountingEntries" style="border:solid 3px #336699; background:azure; width:100%">
                                            <tr>
                                                <td colspan="2">
                                                    <div id="div_Tranx_Rec_All" style="display:none">
                                                        <table id="tbl_Tranx_Rec_All" style="width:100%">
                                                            <tr>                                                    
                                                                <td class="cell_caption"        style="width:40%; height:24px">Remitting Currency:</td>
                                                                <td align="left"                style="width:60%; height:24px">
                                                                    <select id="ddTranxCcyCode" style="width:100%" disabled="disabled" onchange="javascript:grsGEFUMaintenance.check_currency_combination()">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption"        style="width:40%; height:24px">Account Currency:
                                                                </td>
                                                                <td align="left"                style="width:60%; height:24px">
                                                                    <select id="ddAcctCcyCode"  style="width:100%" disabled="disabled">
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>                                                    
                                                                <td class="cell_caption"        style="width:40%; height:24px">Implementation Type:</td>
                                                                <td align="left"                style="width:60%; height:24px">
                                                                    <select id="ddImplementationType"  style="width:100%" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>   
                                                            </tr>
                                                        </table> 
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div id="div_GLCode_Desc_DrCr" style="display:none">
                                                        <table id="tbl_GLCode_Desc_DrCr" style="width:100%" >
                                                        <tr>
                                                            <td class="cell_caption"        style="width:40%; height:24px">GL Code:</td>
                                                            <td align="left"                style="width:60%; height:24px">
                                                               <input id="txtGLAcctCode"    style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();"/> 
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption"        style="width:40%; height:24px">GL Code Description:</td>
                                                            <td align="left"                style="width:60%; height:24px">
                                                                <input id="txtGLDesc"       style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_alpha()"/>
                                                            </td>                                                                    
                                                        </tr> 
                                                        <tr>     
                                                            <td class="cell_caption"            style="width:40%; height:24px">Debit Credit Flag:</td>
                                                            <td align="left"                    style="width:60%; height:24px">
                                                                <select id="ddDebitCreditTag"   style="width:100%" disabled="disabled">
                                                                    <option value=""    style ="text-align:left"></option>
                                                                    <option value="D"   style="text-align:left"> Debit </option>
                                                                    <option value="C"   style="text-align:left"> Credit </option>
                                                                </select>                                                                
                                                            </td>
                                                        </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>                                                        
                                            <tr>
                                                <td colspan="2">
                                                    <div id="div_TranxSource" style="display:none">
                                                        <table id="tbl_TranxSource" style="width:100%">
                                                            <tr>      
                                                                <td class="cell_caption"        style="width:40%; height:24px">Transaction Source:</td>
                                                                <td align="left"                style="width:60%; height:24px">
                                                                    <select id="ddTranxSource"  style="width:100%" disabled="disabled">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div id="div_RecordID" style="display:none">
                                                        <table id="tbl_RecordID" style="width:100%">
                                                            <tr>
                                                                <td class="cell_caption"    style="width:40%; height:24px">Record ID:</td>
                                                                <td align="left"            style="width:60%; height:24px">
                                                                <select id="ddRecordID"  style="width:100%" disabled="disabled">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                                    <%--<input id="txtRecordID" style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_alpha()"/-->--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan ="2">
                                                    <div id="div_NSVS" style="display:none">
                                                        <table id="tbl_NSVS" style="width:100%">                                                   
                                                            <tr>                                                    
                                                                <td class="cell_caption"    style="width:40%; height:24px">For Nostro Vostro :</td>
                                                                <td align="left"            style="width:60%; height:24px">
                                                                    <select id="ddNSVS"     style="width:100%" disabled="disabled">
                                                                        <option value=""    style="text-align:left"></option>
                                                                        <option value="0"   style="text-align:left"> No </option>
                                                                        <option value="1"   style="text-align:left"> Yes </option>
                                                                    </select>
                                                                </td>    
                                                            </tr>
                                                            <tr>     
                                                                <td class="cell_caption"    style="width:40%; height:24px">FCC / FCR:</td>
                                                                <td align="left"            style="width:60%; height:24px">
                                                                    <select id="ddFCCFCR"   style="width:100%" disabled="disabled">
                                                                        <option value=""    style ="text-align:left"></option>
                                                                        <option value="0"   style="text-align:left"> FCC </option>
                                                                        <option value="1"   style="text-align:left"> FCR </option>
                                                                    </select>
                                                                </td>
                                                            </tr>   
                                                            <tr>                                                    
                                                                <td class="cell_caption"    style="width:40%; height:24px">FCDU / RBU:</td>
                                                                <td align="left"            style="width:60%; height:24px">
                                                                    <select id="ddFCDURBU"  style="width:100%" disabled="disabled">
                                                                        <option value=""    style ="text-align:left"></option>
                                                                        <option value="0"   style="text-align:left"> FCDU </option>
                                                                        <option value="1"   style="text-align:left"> RBU </option>
                                                                    </select>
                                                                </td>   
                                                            </tr>   
                                                        </table>                                                        
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"    style="width:40%; height:25px">Handling:</td>
                                                <td align="left" style="height: 25px">
                                                    <select id="ddHandlingType"  style="width:100%" disabled="disabled" onchange="javascript:grsGEFUMaintenance.show_required_fields()">
                                                        <option value=""    style ="text-align:left"></option>
                                                        <option value="1"   style="text-align:left"> by TranxSource</option>
                                                        <option value="2"   style="text-align:left"> by Record ID </option>
                                                        <option value="3"   style="text-align:left"> For All </option>
                                                        <option value="4"   style="text-align:left"> by NSVS </option>
                                                    </select>
                                                </td>
                                            </tr>  
                                        </table>
                                    </td>
                                </tr> <!-- second body -->
                                <tr>
                                    <td colspan="4">
                                        <table id = "tblGRSAccountingEntriesButtons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%; " >
                                            <tr>
                                                <td style="height:22px" ><input id="btnAddGRSAccountingEntries"        class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add"      style="width:100px"                      onclick="javascript:grsGEFUMaintenance.insert_record();"/></td>
                                                <td style="height:22px" ><input id="btnEditGRSAccountingEntries"       class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit"     style="width:100px"                      onclick="javascript:grsGEFUMaintenance.update_record();"/></td>
                                                <td style="height:22px" ><input id="btnSaveGRSAccountingEntries"       class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save"     style="width:100px" disabled="disabled"  onclick="javascript:grsGEFUMaintenance.commit_record();" /></td>
                                                <td style="height:22px" ><input id="btnDeleteGRSAccountingEntries"     class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Delete"   style="width:100px" disabled="disabled"  onclick="javascript:grsGEFUMaintenance.delete_record();"/></td>
                                                <td style="height:22px" ><input id="btnCancelGRSAccountingEntries"     class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Cancel"   style="width:100px" disabled="disabled"  onclick="javascript:grsGEFUMaintenance.rollback_record();" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <input id = "txtHidden_GRSAccountingEntries_SEQ_ID" type="hidden" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div id="divtbl_grs_accountingentries" style="text-align:center;">
                                        </div>  
                                    </td>
                                </tr>
                            </table>                                        
                        </div>    
                        <div id="div_GRS_CorrBank" style="display:none; overflow:auto; height:400px">
                            <table cellpadding="0" cellspacing="0"      style="width:700px" >
                                <tr> <!-- header -->
                                <td style="width: 85%; vertical-align:bottom; text-align: left" align="left">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom"    style="width:0%; height:24px; border-collapse:collapse">
                                        <img src="../images/toe.GIF" /></td>      
                                    <td class="table_head_dark"         style="width:40%; height:24px">CorrBank Maintenance</td>                                                
                                    <td align="left" valign="bottom"    style="width:60%; height:24px">
                                        <img align="bottom" src="../images/rend.gif" /></td>
                                </tr>
                                </table>
                                </td>
                                </tr>
                                <tr>
                                    <td colspan ="3" style="height: 110px">
                                        <table id="tblGRSCorrBank" style="background:azure; border:solid 3px #336699; width:100%">
                                            <tr>
                                                <td class="cell_caption"            style="width:40%; height:24px">CorrBank Code:</td>
                                                <td align="left"                    style="width:60%; height:24px">
                                                    <input id="txtCorr_Bank_Code"   style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_alpha()" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"            style="width:40%; height:24px">CorrBank Name:</td>
                                                <td align="left"                    style="width:60%; height:24px">
                                                    <input id="txtCorr_Bank_Name"   style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_alpha()"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"        style="width:40%; height:24px">SWIFT Code:</td>
                                                <td align="left"                style="width:60%; height:24px">
                                                    <input id="txtSWIFT_Code"   style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_alpha()"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"        style="width:40%; height:24px">Courier Code:</td>
                                                <td align="left"                style="width:60%; height:24px">
                                                    <input id="txtCourier_Code" style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_alpha()"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"        style="width:40%; height:24px">Currency:</td>
                                                <td align="left"                style="width:60%; height:24px">
                                                    <select id="ddCurrency_Code"    style="width:100%" disabled="disabled">
                                                        <option value=""            style ="text-align:left"></option>
                                                    </select>                                                                
                                                </td>
                                            </tr> 
                                            <tr>     
                                                <td class="cell_caption"        style="width:40%; height:24px">Account Type:</td>
                                                <td align="left"                style="width:60%; height:24px">
                                                    <select id="ddAccount_Type" style="width:100%" disabled="disabled">
                                                        <option value=""        style ="text-align:left"></option>
                                                        <option value="RBU"     style="text-align:left">RBU</option>
                                                        <option value="FCDU"    style="text-align:left">FCDU</option>
                                                    </select>                                                                
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"        style="width:40%; height:24px">CK Account:</td>
                                                <td align="left"                style="width:60%; height:24px">
                                                    <input id="txtCK_Account"   style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_ck_account();"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"        style="width:40%; height:24px">GL Account:</td>
                                                <td align="left"                style="width:60%; height:24px">
                                                    <input id="txtGL_Account"   style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_gl_codes();" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption"            style="width:40%; height:24px">GL Description:</td>
                                                <td align="left"                    style="width:60%; height:24px">
                                                    <input id="txtGL_Description"   style="text-align:left; width:97%;"   type="text" disabled="disabled" onkeypress="javascript:grsGEFUMaintenance.check_alpha()"/>
                                                </td>
                                            </tr>
                                            <tr>     
                                                <td class="cell_caption"        style="width:40%; height:24px">On Hold:</td>
                                                <td align="left"                style="width:60%; height:24px">
                                                    <select id="ddOnHold"   style="width:100%" disabled="disabled">
                                                        <option value=""    style ="text-align:left"></option>
                                                        <option value="1" style="text-align:left">Yes</option>
                                                        <option value="0"  style="text-align:left">No</option>
                                                    </select>                                                                
                                                </td>
                                            </tr>
                                            <tr>     
                                                <td class="cell_caption"    style="width:40%; height:24px">Country:</td>
                                                <td align="left"            style="width:60%; height:24px">
                                                    <select id="ddCountry"  style="width:100%" disabled="disabled">
                                                    </select>                                                                
                                                </td>
                                            </tr>
                                            <tr>     
                                                <td class="cell_caption"    style="width:40%; height:24px">Charge Type:</td>
                                                <td align="left"            style="width:60%; height:24px">
                                                    <select id="ddChargeType"  style="width:100%" disabled="disabled">
                                                    </select>                                                                
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 53px">
                                        <table id = "tblGRSCorrBankButtons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%">
                                            <tr>
                                                <td style="height:22px" ><input id="btnAddGRSCorrBank"   class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add" style="width:100px"     onclick="javascript:grsGEFUMaintenance.insert_record();"/></td>                                                            
                                                <td style="height:22px" ><input id="btnEditGRSCorrBank"   class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" style="width:100px"     onclick="javascript:grsGEFUMaintenance.update_record();"/></td>
                                                <td style="height:22px" ><input id="btnSaveGRSCorrBank"   class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" style="width:100px"     onclick="javascript:grsGEFUMaintenance.commit_record();" disabled="disabled"/></td>
                                                <td style="height:22px" ><input id="btnDeleteGRSCorrBank"   class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Delete" style="width:100px"     onclick="javascript:grsGEFUMaintenance.delete_record();" disabled="disabled"/></td>
                                                <td style="height:22px" ><input id="btnCancelGRSCorrBank" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Cancel"style="width:100px"    onclick="javascript:grsGEFUMaintenance.rollback_record();" disabled="disabled"/></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <input id = "txtHidden_tbl_GRS_CorrBank_SEQ_ID" type="hidden" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan ="4">
                                        <div id="divtbl_GRS_CorrBank" style="text-align:center; width:500px" >
                                        </div>
                                    </td>
                                </tr>
                            </table>      
                        </div>
                        <div id="div_GRS_BuyingRates" style="display:none; overflow:auto; height:600px">
                            <table cellpadding="0" cellspacing="0"      style="width:700px;" >
                                <tr> <!-- header -->
                                <td style="width:85%; vertical-align:bottom; text-align:left" align="left">
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="bottom"    style="width:0%; height:24px; border-collapse:collapse">
                                        <img src="../images/toe.GIF" /></td>      
                                    <td class="table_head_dark"         style="width:40%; height:24px">Booking Rates</td>                                                
                                    <td align="left" valign="bottom"    style="width:60%; height:24px">
                                        <img align="bottom" src="../images/rend.gif" /></td>
                                </tr>
                                </table>
                                </td> 
                                </tr>
                                <tr>
                                    <td colspan= "3">
                                        <table id="tblGRSBuyingRates" style="width:100%; background:azure; border:solid 3px #336699;">
                                            <tr>
                                                <td class="cell_caption"        style="width:25%; height:24px">Source Currency:</td>
                                                <td align="left"                style="width:13%; height:24px">
                                                    <select id="ddGRSSourceCcy"    style="width:100%" onchange ="javascript:grsGEFUMaintenance.check_currencies(document.getElementById('ddGRSSourceCcy').value, document.getElementById('ddGRSTargetCcy').value);" disabled="disabled">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </td>
                                                <td class="cell_caption"        style="width:25%; height:24px">Target Currency:</td>
                                                <td align="left"                style="width:13%; height:24px">
                                                    <select id="ddGRSTargetCcy"    style="width:100%" onchange ="javascript:grsGEFUMaintenance.check_currencies(document.getElementById('ddGRSSourceCcy').value, document.getElementById('ddGRSTargetCcy').value);" disabled="disabled">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="tdGRSSourceToTargetRate" class="cell_caption"                style="width:25%; height:24px">1 Ccy to Ccy:</td>
                                                <td align="left"                        style="width:25%; height:24px">
                                                    <input id="txtGRSSourceToTargetRate"   style="width:97%; text-align:left;" type="text" onkeypress="javascript:grsGEFUMaintenance.check_booking_rates()" onkeyup="javascript:grsGEFUMaintenance.get_bookingrates(document.getElementById('txtGRSSourceToTargetRate').value);" disabled="disabled" />
                                                </td>
                                                <td id="tdGRSTargetToSourceRate" class="cell_caption"                style="width:25%; height:24px">1 Ccy to Ccy:</td>
                                                <td align="left"                        style="width:25%; height:24px">
                                                    <input id="txtGRSTargetToSourceRate"   style="width:97%; text-align:left;" type="text" disabled="disabled" readonly="readOnly"/>
                                                </td>
                                            </tr>  
                                            <tr>
                                                <td colspan="4">
                                                    <table id = "tblGRSBuyingRatesButtons" cellpadding="0" cellspacing="5" style="border:solid 1px #336699; background:azure; width:100%">
                                                        <tr>
                                                            <td style="height:22px" ><input id="btnGRSAddBuyingRates"     class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add"      style="width:100px" onclick="javascript:grsGEFUMaintenance.insert_record();"/></td>
                                                            <td style="height:22px" ><input id="btnGRSSaveBuyingRates"    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save"     style="width:100px" onclick="javascript:grsGEFUMaintenance.commit_record();"     disabled="disabled"/></td>
                                                            <td style="height:22px" ><input id="btnGRSCancelBuyingRates"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Cancel"   style="width:100px" onclick="javascript:grsGEFUMaintenance.rollback_record();"   disabled="disabled"/></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <br />
                                                    <input id = "txtHidden_buyingrates_SEQ_ID" type="hidden" />
                                                </td>
                                            </tr>                                                
                                            <tr> <!--filterby row-->
                                                <td colspan="4">
                                                    <table id="tbl_effectivity_date" style="width:100%; ">
                                                        <tr>
                                                            <td style="width:25%"></td>
                                                            <td id ="tdGRSEffectivityDate" class="cell_caption" style="width:25%" align="right"> View by Effectivity Date :
                                                            </td>
                                                            <td>
                                                                <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtGRSEffectivityDate, 'mm/dd/yyyy');"/>
                                                            </td>
                                                            <td style="width:25%">
                                                                <input id="txtGRSEffectivityDate" style="width:98%" onchange="javascript:grsGEFUMaintenance.load_buyingrates_grid_viewer(this.value)"/>
                                                            </td>
                                                            <td style="width:25%"></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>                                
                                        </table>
                                    </td>
                                </tr>
                                <%--<tr> <!--filterby row-->
                                    <td colspan="4">
                                        <table id="tbl_effectivity_date" style="width:100%; background:azure; border:solid 3px #336699;">
                                            <tr>
                                                <td style="width:25%"></td>
                                                <td id ="tdGRSEffectivityDate" class="cell_caption" style="width:25%" align="right"> View by Effectivity Date :
                                                </td>
                                                <td>
                                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtGRSEffectivityDate, 'mm/dd/yyyy');"/>
                                                </td>
                                                <td style="width:25%">
                                                    <input id="txtGRSEffectivityDate" style="width:98%" onchange="javascript:grsGEFUMaintenance.load_buyingrates_grid_viewer(this.value)"/>
                                                </td>
                                                <td style="width:25%"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>    --%>           
                                <tr>
                                    <td colspan="3">
                                        <div id="divtbl_buyingrates" style="text-align:center; width:700px" >
                                        </div>                                                
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div_GRS_GEFU" style="display:none">
                            <table cellpadding="0" cellspacing="0"          style="width:350px" >
                                <tr> <!-- header -->
                                    <td align="left"    valign="bottom"    style="width:11px; height:24px; border-collapse:collapse">
                                        <img src="../images/toe.GIF" /></td>      
                                    <td class="table_head_dark"             style="width:50%; height:24px">GRS GEFU</td>                                                
                                    <td align="left"    valign="bottom"    style="width:100%; height:24px">
                                        <img align="bottom" src="../images/rend.gif" /></td>
                                </tr>
                                <tr>
                                    <td colspan= "3" style="height:19px;">
                                        <table style="background:azure; border:solid 3px #336699; width:100%">                                            
                                   <%--         <tr>
                                                <td colspan= "2" align="left" style="width:13%; height:24px">
                                                    <select id="ddBranch"     style="width:100%">
                                                        <option selected="selected"></option>
                                                    </select>
                                                </td>
                                            </tr>--%>
                                            <tr>
                                                <td  align="left"   style="width:13%; height:24px"> <!--colspan ="2" align="left"   style="width:13%; height:24px"-->
                                                    <select id="ddReportName"   style="width:100%">
                                                        <option value="1">GRS GEFU Upload File</option>
                                                     <%--   <option value="2">IRS Preliminary Summary</option>
                                                        <option value="3">IRS Summary</option>
                                                        <option value="4">IRS Consolidated Summary</option>
                                                        <option value="5">Transactions not included in GEFU File</option>
                                                        <option value="6">IRS Consolidated Summary (by GL Codes)</option>
                                                        <option value="7">Transaction Details by Transaction Source</option>
                                                        <option value="8">IRS Consolidated Summary (Online and Non-online)</option>
                                                        <option value="9">IRS Consolidated Summary (by GL Codes) - (Online and Non-online)</option>
                                                        <option value="10">Transaction Details by Receipt Mode</option>
                                                        <option value="11">Booking Rates Used</option>  --%>      
                                                    </select>
                                                </td>
                                            </tr>
                                            <tr>
                                        <%--        <td style="width:50%; height:76px;">
                                                    <select name="drop1" id="Select6" size="4" multiple="multiple">
                                                        <option value="1">item 1</option>
                                                        <option value="2">item 2</option>
                                                        <option value="3">item 3</option>
                                                        <option value="4">item 4</option>                                                                    
                                                        <option value="3">item 5</option>
                                                        <option value="4">item 6</option>
                                                        <option value="0">All</option>
                                                    </select>
                                                </td>--%>
                                                
                                                <td style="height:76px">
                                                    <table style="width:100%">
                                                <%--        <tr>
                                                            <td align="left"                style="width:13%; height:24px" colspan="2">
                                                                <select id="ddTranxStatus"  style="width:100%">
                                                                    <option selected="selected"></option>
                                                                </select>
                                                            </td>
                                                        </tr>--%>
                                                        <tr>
                                                            <td>
                                                                <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtTranxdate, 'mm/dd/yyyy');" />
                                                            </td>
                                                            <td style="width:80%">
                                                                <input id="txtTranxdate" style="width:98%" />                                                                            
                                                            </td>
                                                        </tr>
                                                    </table>
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
                                                    <input id="btnProcess"    class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Process"  style="width:100px" onclick="javascript:grsGEFUMaintenance.create_grs_gefu();"/>
                                                </td>
                                                <td style="height:22px" >
                                                    <input id="btnCloseGEFU"  class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Close"    style="width:100px" onclick="javascript:grsGEFUMaintenance.checksession();" />
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
        </div>
    </form>
</body>
</html>
