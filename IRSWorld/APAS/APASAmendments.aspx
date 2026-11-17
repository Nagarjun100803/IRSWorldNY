<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APASAmendments.aspx.cs" Inherits="IRSWorld.APAS.APASAmendments" %>

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
    <script src="../js/APAS/jsAPASAmendments.js"       type = "text/javascript"></script>
    <script type="text/javascript">        
    </script>
</head>
<body onload="page_setup();">
    <form id="Form1" runat="server">
    <div id="divscroll" style="height:430px; overflow:scroll" >
    <div style="width: 100%" id="DIV_Search">
    
        <table id="tblSearch" cellpadding="0" cellspacing="0" width="100%" style="margin-top:3%;  ">
            <tr>
                <td align="left" style="width: 1%; border-collapse: collapse; height: 24px" valign="bottom">
                    <img src="../images/toe.GIF" /></td>
                <td id="capSearch" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                    height: 24px">
                    For Approval</td>
                <td align="left" class="bg_body" style="width: 300px; height: 24px" valign="bottom">
                    <img align="absBottom" src="../images/rend.gif" /></td>
            </tr>
            <tr>
                <td colspan="3" style="background: azure; width: 100%">
                    <table id="tblMain" class="table_border" style="width: 100%">
                        <tr>
                            <td id="tdAmendments" >
                                </td>
        
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    
        </div>
    <div style="width: 100%; display:none" id="div_results">
        <table width="100%" style="margin-top:2%">
            
            <tr>
           
                
                <td style="vertical-align: top; width: 771px; text-align: left;">
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 101%; height: 383px;" id="tblMainTable">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="Td2" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                                Search&nbsp; Result</td>
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
                                                    <td class="table_caption" colspan="3" style="height: 23px; width: 268px;">
                                                        TRANSACTION DETAILS
                                                    </td>
                                                </tr>
                                                </table>
                                                    <div id='div_result'> <!--style="overflow:auto;height:300px;" >--> <!--style="overflow:auto;height:300px;"-->
                                                        <table>
                                                            
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldImplemDate">
                                                                    Implementation Date :</td>
                                                                <td id="ImplemDate" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                 
                                                                        Implementation Date
                                                               
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldRefNoTheirs">
                                                                    Ref No. Theirs (RB) :</td>
                                                                <td id="RefNoTheirs" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                   
                                                                        Ref No. Theirs (RB)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRefNoOurs">
                                                                    Ref. No. Ours (ORD) :</td>
                                                                <td id="RefNoOurs" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                                    
                                                                        Ref. No. Ours (ORD)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldORCDiv">
                                                                    ORC Div. :</td>
                                                                <td id="ORC" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                                  
                                                                        ORC Div.
                                                             
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldBenefName">
                                                                    Last Name :</td>
                                                                <td id="BenefLastNm" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                  
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td6">
                                                                    First Name :</td>
                                                                <td id="BenefFirstNm" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                  
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td10">
                                                                    Middle Name :</td>
                                                                <td id="BenefMiddleNm" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                  
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldBenefAddress">
                                                                    Beneficiary Address :</td>
                                                                <td id="BeneAdd" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                    
                                                                        Beneficiary Address</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldContactNumber">
                                                                    Beneficiary Contact Number : &nbsp;</td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 254px;" id="BeneContactNo">
                                                                    Beneficiary Contact Number</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldSpecialMsg">
                                                                    Special Message :</td>
                                                                <td id="SpecialMSG" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                   
                                                                        Special Message
                                                                
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldPurpose">
                                                                    Purpose of Remittance :</td>
                                                                <td id="Purpose" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                    
                                                                        Purpose of Remittance
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterID">
                                                                    Remitter ID :</td>
                                                                <td id="RemID" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                               
                                                                        Remitter ID</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterName">
                                                                    Remitter's Name :</td>
                                                                <td id="RemName" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                                   
                                                                        Remitter's Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterAddress">
                                                                    Remitter Address :</td>
                                                                <td id="RemAdd" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                                    
                                                                        Remitter Address</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldRemittingBank">
                                                                    Remitting Bank :</td>
                                                                <td id="RemBank" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                                    
                                                                        Remitting Bank
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px;" id="FldProductCode">
                                                                    Product Code :</td>
                                                                <td id="ProdCode" align="left" class="cell_desc" colspan="4" style="width: 254px; height: 18px;">
                                                                    
                                                                        Product Code</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldCurrencyCode">
                                                                    Currency Code :</td>
                                                                <td id="CurrencyCode" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                                  
                                                                        Currency Code</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    <span id="FldAmountPayable" class="Label">Amount Payable :</span></td>
                                                                <td id="Amount" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                                   
                                                                        Amount Payable 999,999.99
                                                                  
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRateUsed">
                                                                    Rate Used :
                                                                </td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 254px" id="RateUsed">
                                                                    Rate Used</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldEVAA">
                                                                    EVAA :</td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 254px;" id="EVAA">
                                                                    EVAA
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldAccount">
                                                                    NSVS/CASA Account :</td>
                                                                <td class="cell_desc" colspan="4" style="text-align: left; width: 254px;" id="NSVS">
                                                                    NSVS/CASA Account</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldTranxStatus">
                                                                    Transaction Status :</td>
                                                                <td id="trxn_stat" class="cell_desc" colspan="4" style="width: 254px;
                                                                    text-align: left">
                                                                    trxn_stat</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="display: none; width: 29%">
                                                                </td>
                                                                <td class="cell_desc" colspan="4" style="width: 254px; text-align: left">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="display: none; width: 29%">
                                                                </td>
                                                                <td class="cell_desc" colspan="4" style="width: 254px; text-align: left">
                                                                </td>
                                                            </tr>
                                                            

                                                                
                                                                
                                                 
                                                            
                                                        </table>
                                                       
                                                        
                                                      </div>
                                                   
                                                           
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
               
                <td style="vertical-align: top; width: 771px; text-align: left;">
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 100%; height: 383px;" id="tblAmend">
                        <tr>
                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="Td1" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                                Search&nbsp; Result</td>
                            <td align="left" class="bg_body" style="height: 24px; width: 434px;" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%; height: 100%">
                                <table class="table_border" style="width: 100%; height: 60%">                                    
                                    <tr>
                                        <td style="vertical-align: top; width: 100%; height: 100%" id="Td3"> 
                                            <table cellpadding="0" width="100%">
                                                <tr>   
                                                <td style="height: 348px">
                                                <table style="width:100%">
                                                <tr>
                                                    <td class="table_caption" colspan="3" style="height: 23px; width: 268px;">
                                                        TRANSACTION DETAILS
                                                    </td>
                                                </tr>
                                                </table>
                                                    <div id='div1'> <!--style="overflow:auto;height:300px;" >--> <!--style="overflow:auto;height:300px;"-->
                                                        <table>
                                                            
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldImplemDate2">
                                                                    Implementation Date :</td>
                                                                <td id="ImplemDate2" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                 
                                                                        Implementation Date
                                                               
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldRefNoTheirs2">
                                                                    Ref No. Theirs (RB) :</td>
                                                                <td id="RefNoTheirs2" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                   
                                                                        Ref No. Theirs (RB)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRefNoOurs2">
                                                                    Ref. No. Ours (ORD) :</td>
                                                                <td id="RefNoOurs2" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                    
                                                                        Ref. No. Ours (ORD)</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldORCDiv2">
                                                                    ORC Div. :</td>
                                                                <td id="ORC2" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                  
                                                                        ORC Div.
                                                             
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="fldBenefName2">
                                                                    Last Name :</td>
                                                                <td id="BenefLastNm2" align="left" class="cell_desc" colspan="4" style="width: 287px; color:Red">
                                                                  
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td8">
                                                                    First Name :</td>
                                                                <td id="BenefFirstNm2" align="left" class="cell_desc" colspan="4" style="width: 287px; color:Red">
                                                                  
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td15">
                                                                    Middle Name :</td>
                                                                <td id="BenefMiddleNm2" align="left" class="cell_desc" colspan="4" style="width: 287px; color:Red">
                                                                  
                                                                        Beneficiary Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td14">
                                                                    Beneficiary Address :</td>
                                                                <td id="BeneAdd2" align="left" class="cell_desc" colspan="4" style="width: 287px; color:Red">
                                                                    
                                                                        Beneficiary Address</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 33px;" id="fldContactNumber2">
                                                                    Beneficiary Contact Number : &nbsp;</td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 287px; height: 33px;" id="BeneContactNo2">
                                                                    Beneficiary Contact Number</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldSpecialMsg2">
                                                                    Special Message :</td>
                                                                <td id="SpecialMSG2" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                   
                                                                        Special Message
                                                                
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="FldPurpose2">
                                                                    Purpose of Remittance :</td>
                                                                <td id="Purpose2" align="left" class="cell_desc" colspan="4" style="width: 287px;">
                                                                    
                                                                        Purpose of Remittance
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterID2">
                                                                    Remitter ID :</td>
                                                                <td id="RemID2" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                               
                                                                        Remitter ID</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterName2">
                                                                    Remitter's Name :</td>
                                                                <td id="RemName2" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                   
                                                                        Remitter's Name</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldRemitterAddress2">
                                                                    Remitter Address :</td>
                                                                <td id="RemAdd2" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                    
                                                                        Remitter Address</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 11px;" id="FldRemittingBank2">
                                                                    Remitting Bank :</td>
                                                                <td id="RemBank2" align="left" class="cell_desc" colspan="4" style="width: 287px; height: 11px;">
                                                                    
                                                                        Remitting Bank
                                                                   
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px;" id="FldProductCode2">
                                                                    Product Code :</td>
                                                                <td id="ProdCode2" align="left" class="cell_desc" colspan="4" style="width: 287px; height: 18px;">
                                                                    
                                                                        Product Code</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="FldCurrencyCode2">
                                                                    Currency Code :</td>
                                                                <td id="CurrencyCode2" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                  
                                                                        Currency Code</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    <span id="FldAmountPayable2" class="Label">Amount Payable :</span></td>
                                                                <td id="Amount2" align="left" class="cell_desc" colspan="4" style="width: 287px">
                                                                   
                                                                        Amount Payable 999,999.99
                                                                  
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%" id="Td35">
                                                                    Rate Used :
                                                                </td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 287px" id="RateUsed2">
                                                                    Rate Used</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td37">
                                                                    EVAA :</td>
                                                                <td align="left" class="cell_desc" colspan="4" style="width: 287px;" id="EVAA2">
                                                                    EVAA
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td39">
                                                                    NSVS/CASA Account :</td>
                                                                <td class="cell_desc" colspan="4" style="text-align: left; width: 287px;" id="NSVS2">
                                                                    NSVS/CASA Account</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%;" id="Td41">
                                                                    Transaction Status :</td>
                                                                <td id="trxn_stat2" class="cell_desc" colspan="4" style="width: 287px;
                                                                    text-align: left">
                                                                    trxn_stat</td>
                                                            </tr>
                                                            <tr>
                                                                <td id="fldDateAmended" class="cell_caption" style="width: 29%">
                                                                Date Amended:
                                                                </td>
                                                                <td class="cell_desc" colspan="4" style="width: 287px; text-align: left" id="lblDateAmended">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="lblAmendedBy" class="cell_caption" style="width: 29%">
                                                                    Amended By:</td>
                                                                <td id="lblAmendedBy2" class="cell_desc" colspan="4" style="width: 287px; text-align: left">
                                                                </td>
                                                            </tr>
                                                            

                                                               
                                                                
                                                 
                                                            
                                                        </table>
                                                       
                                                        </div>
                                                   
                                                           
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
               
               
               
               
            </tr>
        </table>
        <center>
        <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 50%; height: 383px;" id="tblCancel">
            <tr>
                <td align="left" style="width: 1%; height: 24px" valign="bottom">
                    <img src="../images/toe.GIF" /></td>
                <td id="Td4" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                    Cancellation</td>
                <td align="left" class="bg_body" style="height: 24px; width: 434px;" valign="bottom">
                    <img align="absBottom" src="../images/rend.gif" /></td>
            </tr>
            <tr>
                <td colspan="3" style="width: 100%; height: 100%">
                    <table class="table_border" style="width: 100%; height: 60%">
                        <tr>
                            <td style="vertical-align: top; width: 100%; height: 100%" id="Td49">
                                <table cellpadding="0" width="100%">
                                    <tr>
                                        <td style="height: 348px">
                                            <table style="width:100%">
                                                <tr>
                                                    <td class="table_caption" colspan="3" style="height: 23px; width: 268px;">
                                                        TRANSACTION DETAILS
                                                    </td>
                                                </tr>
                                            </table>
                                            <div id='Div2'> <!--style="overflow:auto;height:300px;" >-->
                                                <!--style="overflow:auto;height:300px;"-->
                                                <table>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td5">
                                                            Implementation Date :</td>
                                                        <td id="CImplemDate" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                            Implementation Date
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td7">
                                                            Ref No. Theirs (RB) :</td>
                                                        <td id="CRefNoTheirs" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                            Ref No. Theirs (RB)</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%" id="Td9">
                                                            Ref. No. Ours (ORD) :</td>
                                                        <td id="CRefNoOurs" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                            Ref. No. Ours (ORD)</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%" id="Td11">
                                                            ORC Div. :</td>
                                                        <td id="CORC" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                            ORC Div.
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td13">
                                                            Beneficiary&nbsp; Name :</td>
                                                        <td id="CBenefName" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                            Beneficiary Name</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td16">
                                                            Beneficiary Address :</td>
                                                        <td id="CBenefAddress" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                            Beneficiary Address</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td18">
                                                            Beneficiary Contact Number : &nbsp;</td>
                                                        <td align="left" class="cell_desc" colspan="4" style="width: 254px;" id="CBenefContactNo">
                                                            Beneficiary Contact Number</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td20">
                                                            Special Message :</td>
                                                        <td id="CSpecialMsg" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                            Special Message
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td22">
                                                            Purpose of Remittance :</td>
                                                        <td id="CPurpose" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                            Purpose of Remittance
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%" id="Td24">
                                                            Remitter ID :</td>
                                                        <td id="CRemID" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                            Remitter ID</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%" id="Td26">
                                                            Remitter's Name :</td>
                                                        <td id="CRemName" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                            Remitter's Name</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%" id="Td28">
                                                            Remitter Address :</td>
                                                        <td id="CRemAddress" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                            Remitter Address</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td30">
                                                            Remitting Bank :</td>
                                                        <td id="CRemBank" align="left" class="cell_desc" colspan="4" style="width: 254px;">
                                                            Remitting Bank
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%; height: 18px;" id="Td32">
                                                            Product Code :</td>
                                                        <td id="CProductCode" align="left" class="cell_desc" colspan="4" style="width: 254px; height: 18px;">
                                                            Product Code</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%" id="Td34">
                                                            Currency Code :</td>
                                                        <td id="CCurrCode" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                            Currency Code</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%">
                                                            <span id="Span1" class="Label">Amount Payable :</span></td>
                                                        <td id="CAmountPayable" align="left" class="cell_desc" colspan="4" style="width: 254px">
                                                            Amount Payable 999,999.99
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%" id="Td40">
                                                            Rate Used :
                                                        </td>
                                                        <td align="left" class="cell_desc" colspan="4" style="width: 254px" id="CRateUsed">
                                                            Rate Used</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td43">
                                                            EVAA :</td>
                                                        <td align="left" class="cell_desc" colspan="4" style="width: 254px;" id="CEVAA">
                                                            EVAA
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td45">
                                                            NSVS/CASA Account :</td>
                                                        <td class="cell_desc" colspan="4" style="text-align: left; width: 254px;" id="CNSVS">
                                                            NSVS/CASA Account</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%;" id="Td47">
                                                            Transaction Status :</td>
                                                        <td id="C" class="cell_desc" colspan="4" style="width: 254px;
                                                                    text-align: left">
                                                            trxn_stat</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%">
                                                            Cancelled By:</td>
                                                        <td id="CCancelBy" class="cell_desc" colspan="4" style="width: 254px; text-align: left">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%">
                                                            Date Cancelled:</td>
                                                        <td id="CDateCancelled" class="cell_desc" colspan="4" style="width: 254px; text-align: left">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="cell_caption" style="width: 29%">
                                                            Reason for Cancellation</td>
                                                        <td id="CReasonForCancellation" class="cell_desc" colspan="4" style="width: 254px;
                                                            text-align: left">
                                                        </td>
                                                    </tr>
                                                    
                                                </table>
                                            </div>
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
        <table id="tblRejection" style="width:50%; display:none">
            <tr>  
                <td  class="cell_caption" style="width:30%">
                    Reason for Rejection</td>
                <td  style="width:70%">
                    <input id="txtReason" type="text" style="width: 91%" /></td>
            </tr>
            <tr>
                <td colspan="2" style="font-size:smaller; font-style:italic; color: red;">
                    
                        Note: Reason for Rejection is mandatory if you clicked on disapprove
                    
                    </td>
            </tr>
        
        </table>
        <table id="Amendments" style="width: 50%">
            
            <tr>
                <td>
                    <input id="btnApproved" value="Approve Amendments" type="button" style="width: 100%" onclick="approve_cancel_amendments('Approved');"  /></td>
                <td>
                    <input id="btnCancel" type="button" value="Disapprove Amendments" style="width: 100%" onclick="approve_cancel_amendments('Cancelled');"  /></td>
                <td> 
                    <input id="btnBack" type="button" value="Back" style="width: 100%" onclick="show_hide_form('tblSearch','block');show_hide_form('div_results','none');"  />
                </td>  
            </tr>
           
        </table>
        
        <table id="Cancellation" style="width: 50%">
            <tr>
                <td>
                    <input id="btnApproved1" value="Approve Cancellation" type="button" style="width: 100%" onclick="approve_cancellation('Approved');"  /></td>
                <td>
                    <input id="btnCancel1" type="button" value="Disapprove Cancellation" style="width: 100%" onclick="approve_cancellation('Cancelled');"  /></td>
                <td> 
                    <input id="btnBack2" type="button" value="Back" style="width: 100%" onclick="show_hide_form('tblSearch','block');show_hide_form('div_results','none');"  />
                </td>  
            </tr>
           
        </table>
        </center>       
        </div>
        </div>
    </form>
</body>
</html>
