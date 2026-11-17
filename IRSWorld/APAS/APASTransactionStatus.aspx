<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APASTransactionStatus.aspx.cs" Inherits="IRSWorld.APAS.APASTransactionStatus" %>

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
    <script src="../js/APAS/jsAPAS_Processed_trnx.js"  type = "text/javascript"></script>
    
    <script type="text/javascript">        
    



    </script>
</head>
<body onload="page_setup();" id="btnOK">
    <form id="form1" runat="server">
    <div style="width: 100%" id="DIV_Search">
        <table width="100%">
            <tr>
                

                <td id="td4" style="vertical-align: top; width: 10%; padding-top: 30px">
                    <table id="gridDetails" cellpadding="0" cellspacing="0" style="background-color: azure; width: 100%;">
                        <tr>
                            <td align="left" style="width: 0%; height: 24px" valign="bottom">
                                <img src="../images/toe.GIF" /></td>
                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                height: 24px">
                                Tansaction Status</td>
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
                                                    <td style="width: 18%; height: 21px;" align="left" class="cell_caption">
                                                        Transaction Status:&nbsp; </td>
                                                    <td align="left" style="height: 21px"><div id="divfldgroup">
                                                        <select id="ddlTranxStat" style="width: 28%" onchange="show_hide_form('gridDetails','block');show_hide_form('frmDetails','none');populate_rec(false);">
                                                            
                                                           
                                                        </select>
                                                         </div>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td colspan=3 >
                                                        <table id="tblButtons" cellpadding="0" cellspacing="5" style="width: 90%">
                                                            <tr>
                                                                <td align="left" colspan="3" style="width: 71%; height: 50px;">
                                                                    <table cellpadding="0" cellspacing="5" style="width: 70%">
                                                                        <tr>
                                                                            <td colspan="3" style="width: 20%" class="cell_caption"  > Filter By:</td>
                                                                            <td  colspan="3"  id="tdSelect" style="width: 30%">
                                                                                <select id="ddlFilter" style="width: 100%">
                                                                                    <option selected="selected"></option>
                                                                                </select>
                                                                            </td>
                                                                            <td colspan="4"  id="tdValue" style="width: 30%">
                                                                                <input id="txtFilter" type="text" style="width: 95%" /></td>
                                                                            <td colspan="4" style="width:20%">
                                                                                <input id="btnOk2" style="width: 96%" type="button" value="OK" onclick="populate_rec(true);"  /></td>    
                                                                        </tr>
                                                                      
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                 </tr>
                                                <tr>
                                                    <td  id="trnxStatus" style="text-align: center; width:100%" colspan="2">
                                                    
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
        <table id = "frmDetails" cellpadding="0" cellspacing="0" style="background-color: azure; width: 20%;" >
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
                    <table class="table_border" style="width: 50%; height: 100%">
                        <tr>
                            <td style="vertical-align: top; width: 100%; height: 100%">
                                <!-- content starts here -->
                                <table cellpadding="0" style="width: 95%">
                                    <tr>
                                        <td id="Td2" style="text-align: left">
                                            <table style="width: 457px; height: 208px;">
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
                                                    <td id="tdDetails" class="table_caption" colspan="3" style="height: 23px; width: 268px; text-align:left">
                                                        Details</td>
                                                </tr>
                                              
                                               
                                                <tr>
                                                    <td id="tdBy" class="cell_caption" style="width: 29%">
                                                        Processed By:</td>
                                                    <td id="lbl1" class="cell_desc" colspan="2" style="; width: 208px;
                                                        height: auto; text-align: left">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td id="tdDate" class="cell_caption" style="width: 29%">
                                                        Date Processed:</td>
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
                                                  <input id="btnOk" style="width: 31%; " type="button" value="OK" onclick="show_hide_form('gridDetails','block');show_hide_form('frmDetails','none');"  />
                                                  <input id="btnReprint" style="width: 31%; display:none " type="button" value="Re-Print Receipt" onclick="ShowReport();"   />
                                            </center>
                                </table>
                                <!-- content ends here -->
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