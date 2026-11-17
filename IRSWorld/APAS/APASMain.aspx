<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APASMain.aspx.cs" Inherits="IRSWorld.APAS.APASMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Advise and Pay</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>    
    <script src="../js/js_sidebar.js"                  type = "text/javascript"></script>
    <script src="../js/APAS/jsAPAS.js"                 type = "text/javascript"></script>
    
<script type="text/javascript">        
        var cls_APAS = new APAS();  
        var controls  = new controls();  
        var approval_window = new approval_window();      
    </script>
</head>
<body id="theBody" onload="page_setup();" onunload="">
    <form id="form1" runat="server">
    <div id="divBody" style="width:100%; height: 100%;vertical-align: top; margin-top: 30px;">
    <div style="width: 100%" id="DIV_Search">
        <table width="100%">
            <tr>
                <td id="tdAPASMenu" style="vertical-align: top; width: 10%; padding-top: 30px">
                    <div id="divtdAPASMenu" style="margin-top: 20px; display: block">

                        <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                        </script>

                    </div>
                </td>
                <td style="vertical-align: top; text-align: center; ">
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; margin-top:2%; width: 43%;">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                                Search</td>
                            <td align="left" class="bg_body" style="height: 24px; width: 424px;" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%; height: 100%">
                                <table class="table_border" style="width: 100%; height: 100%">                                    
                                    <tr>
                                        <td style="vertical-align: top; width: 100%; height: 100%">
                                            <!-- content starts here -->
                                            <table cellpadding="0" width="100%">
                                                <tr>
                                                    <td style="text-align: center">
                                                        <table width="100%"style="border: solid 1px #336699;">
                                                            <tr>
                                                                <td class="table_caption" colspan="2" style="height: 23px">
                                                                    Search Transaction By :</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%; height: 23px" id="FldDatePaid">
                                                                    Date Paid (mm/dd/yyyy) :</td>
                                                                <td style="width: 25%; height: 23px" id="tdDatePaid">
                                                                    <input id="txtDatePaid" name="txtFDate" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%" id="FldRefNum">
                                                                    Reference Number :</td>
                                                                <td class="cell_desc" style="width: 25%" id="tdRefNum">
                                                                    <input id="txtRefno" name="txtRefno" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%; height: 23px" id="FldBenefLastName">
                                                                    Beneficiary Last Name :</td>
                                                                <td style="width: 25%; height: 23px" id="tdBeneLname">
                                                                    <input id="txtBeneLname" name="txtBeneLname" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%; height: 21px" id="FldBenefFirstName">
                                                                    Beneficiary First Name :</td>
                                                                <td style="width: 25%; height: 21px" id="tdBeneFname">
                                                                    <input id="txtBeneFname" name="txtBeneFname" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%" id="FldBenefMiddleName">
                                                                    Beneficiary Middle Name :</td>
                                                                <td style="width: 25%" id="tdBeneMname">
                                                                    <input id="txtBeneMname" name="txtBeneMname" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%; height: 23px" id="FldAmount">
                                                                    Amount :</td>
                                                                <td style="width: 25%; height: 23px" id="tdAmount">
                                                                    <input id="txtAmount" name="txtAcct" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%" id="FldPayingBranch">
                                                                    Paying Branch :</td>
                                                                <td style="width: 25%" id="tdPayBr">
                                                                    <input id="txtPayBr" name="txtCK" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%" id="FldTransactionStatus">
                                                                    Transaction Status :</td>
                                                                <td style="width: 25%" id="tdTransactionStatus">
                                                                    <select id="cmb_trxn_stat" name="cmb_trxn_stat">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="tdProductCode" class="cell_caption" style="width: 25%; display:none">
                                                                    Product Code</td>
                                                                <td id="cmbProductCode" style="width: 25%; display:none">
                                                                    <select id="ddlProductCode">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="tdCoverFrom" class="cell_caption" style="width: 25%;">
                                                                    Period Covered From (mm/dd/yyyy):</td>
                                                                <td id="tdCoverFromDesc" style="width: 25%;">
                                                                    &nbsp;<input id="txtPeriodFrom" name="txtCK" type="text" />&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%" id="tdCoverTo">
                                                                    Period Covered To (mm/dd/yyyy):</td>
                                                                <td id="txtCoverToDesc" style="width: 25%">
                                                                    <input id="txtPeriodTo" name="txtCK" type="text" /></td>
                                                            </tr>
                                                        </table>
                                                        <input id="Button1" onclick="javascript:cls_APAS.search();" style="width: 20%" type="button"
                                                            value="Search" /></td>
                                                </tr>
                                                
                                            </table>
                                            <!-- content ends here -->
                                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td id="td4" style="vertical-align: top; padding-top: 30px; width:9%">
                    <div id="div2" style="margin-top: 20px; display: block">

                        <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                        </script>

                    </div>
                </td>
            </tr>
        </table>    
    </div>

    <div style="width: 100%" id="div_results">
        <table width="100%">
            <tr>
                <td id="Td1" style="vertical-align: top; width: 10%; padding-top: 30px">
                    <div id="Div1" style="margin-top: 20px; display: block">

                        <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                        </script>

                    </div>
                </td>
          
                <td style="vertical-align: top;  text-align: center;">
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 60%; height: 383px; margin-top:2%">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="Td2" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px; text-align:left">
                                Search Result</td>
                            <td align="left" class="bg_body" style="height: 24px; width: 434px;" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%; height: 100%">
                                <table class="table_border" style="width: 100%; height: 60%">                                    
                                    <tr>
                                        <td style="vertical-align: top; width: 100%; height: 100%" id="btnSave"> 
                                            <table cellpadding="0" width="100%">
                                                <tr>   
                                                <td style="height: 348px">
                                                <table style="width:100%">
                                                <tr>
                                                    <td class="table_caption" colspan="3" style="height: 23px; width: 268px; text-align:left">
                                                        TRANSACTION DETAILS
                                                    </td>
                                                </tr>
                                                </table>
                                                    <div id='div_result'> <!--style="overflow:auto;height:300px;" >--> <!--style="overflow:auto;height:300px;"-->
                                                        <table>
                                                            
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldImplemDate">
                                                                    Implementation Date :</td>
                                                                <td id="ImplemDate" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                 
                                                                        Implementation Date
                                                               
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldRefNoTheirs">
                                                                    Ref No. Theirs (RB) :</td>
                                                                <td id="RefNoTheirs" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                   
                                                                        Ref No. Theirs (RB)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRefNoOurs">
                                                                    Ref. No. Ours (ORD) :</td>
                                                                <td id="RefNoOurs" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                    
                                                                        Ref. No. Ours (ORD)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldORCDiv">
                                                                    ORC Div. :</td>
                                                                <td id="ORC" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                  
                                                                        ORC Div.
                                                             
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldBenefName">
                                                                    Last Name :</td>
                                                                <td  id="BenefLastNm"  align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td5">
                                                                    First Name :</td>
                                                                <td  id="BenefFirstNm"  align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td7">
                                                                    Middle Name :</td>
                                                                <td  id="BenefMiddleNm"  align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldBenefAddress">
                                                                    Beneficiary Address :</td>
                                                                <td id="BeneAdd" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                    
                                                                        Beneficiary Address</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldContactNumber">
                                                                    Beneficiary Contact Number : &nbsp;</td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 287px;" id="BeneContactNo">
                                                                    Beneficiary Contact Number</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldSpecialMsg">
                                                                    Special Message :</td>
                                                                <td id="SpecialMSG" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                   
                                                                        Special Message
                                                                
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldPurpose">
                                                                    Purpose of Remittance :</td>
                                                                <td id="Purpose" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                    
                                                                        Purpose of Remittance
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterID">
                                                                    Remitter ID :</td>
                                                                <td id="RemID" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                               
                                                                        Remitter ID</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterName">
                                                                    Remitter's Name :</td>
                                                                <td id="RemName" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                   
                                                                        Remitter's Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterAddress">
                                                                    Remitter Address :</td>
                                                                <td id="RemAdd" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                    
                                                                        Remitter Address</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldRemittingBank">
                                                                    Remitting Bank :</td>
                                                                <td id="RemBank" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                    
                                                                        Remitting Bank
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px;" id="FldProductCode">
                                                                    Product Code :</td>
                                                                <td id="ProdCode" align="left" class="cell_desc" colspan="4" style="width: 287px; height: 18px;">
                                                                    
                                                                        Product Code</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px;" id="FldCurrencyCode">
                                                                    Currency Code :</td>
                                                                <td id="CurrencyCode" align="left" class="cell_desc" colspan="4" style="width: 287px; height: 18px;">
                                                                  
                                                                        Currency Code</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    <span id="FldAmountPayable" class="Label">Amount Payable :</span></td>
                                                                <td id="Amount" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                   
                                                                        Amount Payable 999,999.99
                                                                  
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRateUsed">
                                                                    Rate Used :
                                                                </td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 287px" id="RateUsed">
                                                                    Rate Used</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldEVAA">
                                                                    EVAA :</td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 287px;" id="EVAA">
                                                                    EVAA
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldAccount">
                                                                    NSVS/CASA Account :</td>
                                                                <td class="cell_desc" colspan="4" style="text-align: left; width: 287px;" id="NSVS">
                                                                    NSVS/CASA Account</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldTranxStatus">
                                                                    Transaction Status :</td>
                                                                <td id="trxn_stat" class="cell_desc" colspan="4" style="width: 287px;
                                                                    text-align: left">
                                                                    trxn_stat</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; display:none" id="tdCancelReason">
                                                                    Cancellation Reason :</td>
                                                                <td id="lblCancelReason" class="cell_desc" colspan="4" style="text-align: left; display:none; width: 287px;">
                                                                    <input id="txtCancelReason" type="text" style="display:none" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldMCNo">
                                                                    MC No. :
                                                                </td>
                                                                <td id="tdMC" class="cell_desc" colspan="4" style="text-align: left; width: 287px;">
                                                                    <input id="txtMC" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldRemarks">
                                                                    Remarks :
                                                                </td>
                                                                <td id="tdRemarks" class="cell_desc" colspan="4" style="text-align: left; width: 287px;">
                                                                    <input id="txtRemarks" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%;" id="FldIDPresented">
                                                                    ID Presented:</td>
                                                                <td class="cell_desc"  style="width: 25%; height: auto; text-align: left">
                                                                    <select id="ddlIDs" style="width: 100%">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                   
                                                                    
                                                                </td>
                                                                <td class="cell_caption" style="width: 25%; height: auto" id="FldIDNumber">
                                                                    ID Number:</td>
                                                                <td class="cell_desc"  style="width: 25%; height: auto; text-align: left">
                                                                    <input id="txtIdNum" type="text" /></td>    
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%; height: auto; " id="lblOnBehalf">
                                                                    On Behalf:</td>
                                                                <td class="cell_desc" id="fldOnBehalf"  style="width: 25%; height: auto; text-align: left; ">
                                                                    <input id="chkOnBehalf" onclick="cls_APAS.check_uncheck_onbehalf();" style="width: 41%" type="checkbox" /></td> 
                                                                  
                                                            </tr>
                                                            <tr>
                                                                <td  class="cell_caption" style="width: 25%; height: auto; display:none " id="lblPartners">
                                                                    Branch/Partners:</td>    
                                                                 <td colspan = 2 class="cell_desc" id="fldPartners"  style="width: 25%; height: auto; display:none text-align: left;  ">
                                                                     <select id="ddlPartners" style="width: 99%; display:none">
                                                                         <option selected="selected"></option>
                                                                     </select>
                                                                 </td>  
                                                            </tr>

                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; display: none;" id="tdProcessedBy">
                                                                        Processed By:</td>
                                                                    <td id="lblProcessedBy" class="cell_desc"; colspan="4";  style="width: 25%; height: auto; text-align: left; display: none;">
                                                                    
                                                                    </td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; display: none;" id="tdDateProcessed">
                                                                        Date Processed:</td>
                                                                    <td id="lblDateProc" class="cell_desc" colspan="4"  style="width: 25%; height: auto; text-align: left; display: none;">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; display: none;" id="tdApprovedBy">
                                                                        Approved By:</td>
                                                                    <td id="lblApprovedBy" class="cell_desc" colspan="4"  style="width: 25%; height: auto; text-align: left; display: none;">
                                                                    </td>
                                                                    
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; display: none;" id="tdDatePaid2">
                                                                        Date Paid:</td>
                                                                    <td id="lblDatePaid" class="cell_desc" colspan="4"  style="width: 25%; height: auto; text-align: left; display: none;">
                                                                    </td>
                                                                    
                                                                    
                                                                </tr>
                                                 
                                                            
                                                        </table>
                                                       
                                                        
                                                        <input id="id" style="width: 1%; height: 3px" type="hidden" /></div>
                                            <table id="tblButtons" cellpadding="0" cellspacing="6" style="width: 100%">
                                                <tr>
                                                    <td align="center" colspan="3" style="width: 100%">
                                                        <table cellpadding="0"  cellspacing="6" style="width: 100%">
                                                            <tr>
                                                                <td id="tdHold">
                                                                    <input id="btnHold" type="button"
                                                                    value="Hold" onclick="javascript:cls_APAS.hold_unhold_tranx('UPDATE',true);" style="display: none" /></td>
                                                                <td id="tdUnHold">
                                                                    <input id="btnUnHold" type="button"
                                                                    value="UnHold" onclick="javascript:cls_APAS.hold_unhold_tranx('UPDATE',false);" style="display: none" /></td>  
                                                                <td id="tdpayout" >
                                                                    <input id="payout" onclick="javascript:approval_window.open_window_approval();"  type="button"
                                                                    value="Payout" />
                                                                </td>
                                                                <td id="tdAmend" >
                                                                    <input id="btnAmend" style=" display: none;" type="button"
                                                                    value="Amend" onclick="cls_APAS.create_textbox();" /></td>
                                                                <td id="tdSaveCancellation" >
                                                                    <input id="btnSaveCancellation" style=" display: none;" type="button"
                                                                    value="Cancel Transaction" onclick="controls.hide_div('div_results',true);controls.hide_div('DIV_Search',false);"  /></td>
                                                                <td id="tdCancel">
                                                                    <input id="btnCancel" style=" display: none; " type="button"
                                                                    value="Cancel" onclick="cls_APAS.set_cancellation_button('Cancel');" /></td>
                                                                <td id="tdSave">
                                                                    <input id="Save" style=" display: none; " type="button"
                                                                    value="Save" onclick="cls_APAS.create_textbox();" /></td>
                                                                <td id="tdBack">
                                                                    <input id="Back" onclick="javascript:controls.hide_div('div_results',true);controls.hide_div('DIV_Search',false);cls_APAS.locked_unlocked_tranx('UPDATE',false);" type="button"
                                                                    value="Close" /></td>
                                                                      
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                                   
                                                           
                                                           </td>                                                                                               
                                                </tr>
                                            </table>
                                            <!-- content ends here -->
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
              
                <td id="Td3" style="vertical-align: top; width: 10%; padding-top: 30px">
                    <div id="Div3" style="margin-top: 20px; display: block">

                        <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                        </script>

                    </div>
                </td>
            </tr>
        </table>
        </div>

</div>
    </form>
</body>
</html>
