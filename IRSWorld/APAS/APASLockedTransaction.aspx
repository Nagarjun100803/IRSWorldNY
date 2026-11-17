<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APASLockedTransaction.aspx.cs" Inherits="IRSWorld.APAS.APASLockedTransaction" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Advise and Pay</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <link href="../css/cssmodals.css" rel="stylesheet" type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>    
    <script src="../js/js_sidebar.js"                  type = "text/javascript"></script>
    <script src="../js/APAS/jsAPAS.js"                  type = "text/javascript"></script>
    <script src="../js/APAS/jsAPASLockedTrxn.js"        type = "text/javascript"></script>
    
    <script type="text/javascript">        
    



    </script>
</head>
<body onload="page_setup();">
    <form id="Form1" runat="server">
    <div style="width: 100%" id="DIV_Search">
        <table width="100%">
            <tr>
                

                <td id="td4" style="vertical-align: top; width: 10%; padding-top: 30px">
                    <table id="gridDetails" cellpadding="0" cellspacing="0" style="background-color: azure; width: 80%;">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                                Locked Transactions</td>
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
                                                    <td  id="tblLocked" style="text-align: center; width:100%" colspan=2>
                                                    
                                                    </td>
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
            </tr>
        </table> 
        
        <center>
        <table id = "frmDetails" cellpadding="0" cellspacing="0" style="background-color: azure; width: 43%;" >
            <tr>
                <td align="left" style="width: 1%; height: 24px" valign="bottom">
                    <img src="../images/toe.GIF" /></td>
                <td id="Td1" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                    Transaction Details</td>
                <td align="left" class="bg_body" style="height: 24px; width: 424px;" valign="bottom">
                    <img align="absBottom" src="../images/rend.gif" /></td>
            </tr>
            <tr>
                <td colspan="3" style="width: 100%; height: 100%">
                    <table class="table_border" style="width: 100%; height: 100%">
                        <tr>
                            <td style="vertical-align: top; width: 100%; height: 100%">
                                <!-- content starts here -->
                                <table cellpadding="0" style="width: 100%">
                                    <tr>
                                        <td id="Td2" style="text-align: left">
                                            <table style="width: 100%; height: 208px;">
                                                <tr>
                                                    <td id="fldImplemDate" class="cell_caption" style="width: 29%">
                                                        Implementation Date :</td>
                                                    <td id="ImplemDate" align="left" class="cell_desc" colspan="2" style="width: 208px">
                                                        Implementation Date
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="fldRefNoTheirs" class="cell_caption" style="width: 29%">
                                                        Ref No. Theirs (RB) :</td>
                                                    <td id="RefNoTheirs" align="left" class="cell_desc" colspan="2" style="width: 208px">
                                                        Ref No. Theirs (RB)</td>
                                                </tr>
                                                <tr>
                                                    <td id="FldRefNoOurs" class="cell_caption" style="width: 29%">
                                                        Ref. No. Ours (ORD) :</td>
                                                    <td id="RefNoOurs" align="left" class="cell_desc" colspan="2" style="width: 208px">
                                                        Ref. No. Ours (ORD)</td>
                                                </tr>
                                               
                                                <tr>
                                                    <td id="fldBenefName" class="cell_caption" style="width: 29%">
                                                        Beneficiary&nbsp; Name :</td>
                                                    <td id="BeneName" align="left" class="cell_desc" colspan="2" style="width: 208px">
                                                        Beneficiary Name</td>
                                                </tr>
                                                <tr>
                                                    <td id="fldBenefAddress" class="cell_caption" style="width: 29%">
                                                        Beneficiary Address :</td>
                                                    <td id="BeneAdd" align="left" class="cell_desc" colspan="2" style="width: 208px">
                                                        Beneficiary Address</td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 29%;" id="fldContactNumber">
                                                        Beneficiary Contact Number : &nbsp;</td>
                                                    <td align="left" class="cell_desc" colspan="2" style="width: 208px;" id="BeneContactNo">
                                                        Beneficiary Contact Number</td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 29%;" id="FldSpecialMsg">
                                                        Special Message :</td>
                                                    <td id="SpecialMSG" align="left" class="cell_desc" colspan="2" style="width: 208px;">
                                                   
                                                        Special Message
                                                
                                                    </td>
                                                </tr>
                                               
                                                
                                                <tr>
                                                    <td id="FldRemitterName" class="cell_caption" style="width: 29%">
                                                        Remitter's Name :</td>
                                                    <td id="RemName" align="left" class="cell_desc" colspan="2" style="width: 208px">
                                                        Remitter's Name</td>
                                                </tr>
                                              
                                                <tr>
                                                    <td class="cell_caption" style="width: 29%">
                                                        <span id="FldAmountPayable" class="Label" style="width: 15%">Amount Payable :</span></td>
                                                    <td id="Amount" align="left" class="cell_desc" colspan="2" style="width: 208px">
                                                        Amount Payable 999,999.99
                                                    </td>
                                                </tr>
                                               
                                                <tr>
                                                    <td class="table_caption" colspan="3" style="height: 23px; width: 268px; text-align:left">
                                                        Details</td>
                                                </tr>
                                              
                                               
                                                <tr>
                                                    <td id="tdBy" class="cell_caption" style="width: 29%">
                                                        LockedBy:</td>
                                                    <td id="lbl1" class="cell_desc" colspan="2" style="; width: 208px;
                                                        height: auto; text-align: left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdDate" class="cell_caption" style="width: 29%">
                                                        Locked Date:</td>
                                                    <td id="lblDate" class="cell_desc" colspan="2" style=" width: 208px;
                                                        height: auto; text-align: left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="td200" class="cell_caption" style=" width: 29%">
                                                        Approved By:</td>
                                                    <td id="lbl200" class="cell_desc" colspan="2" style="; width: 208px;
                                                        height: auto; text-align: left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdDatePaid" class="cell_caption" style=" width: 29%">
                                                        Date Paid:</td>
                                                    <td id="lblPaid" class="cell_desc" colspan="2" style=" width: 208px;
                                                        height: auto; text-align: left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="width:208px" colspan="2">
                                                        &nbsp;</td>
                                                   
                                                </tr>
                                            </table>
                                            <center>
                                                &nbsp;
                                            </center>
                                </table>
                                <!-- content ends here -->
                                <table id="tblButtons" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td align="center" colspan="3" style="width: 100%">
                                            <table cellpadding="0" cellspacing="0" style="width: 65%; height: 38px;">
                                                <tr>
                                                  
                                                    <td id="tdUnlock">
                                                        <input id="btnUnlock" style="width: 97%" type="button" value="Unlock Transaction" onclick="unlock_transaction();" /></td>
                                                    
                                                    <td id="tdBack">
                                                        <input id="btnBack" style="width: 97%" type="button" value="Go Back to Search" onclick="show_hide_form('gridDetails','block');show_hide_form('frmDetails','none');" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
        </table>
        </center>
    </div>
    
    <div style="width: 100%" id="div_results">
        </div>
        
    </form>
</body>
</html>
