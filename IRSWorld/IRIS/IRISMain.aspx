<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IRISMain.aspx.cs" Inherits="IRSWorld.IRIS.IRISMain" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRIS - Search</title>
    
    <link href="../css/Default_min.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons_min.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod_min.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest_min.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow_min.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector_min.js"              type = "text/javascript"></script>    
    <script src="../js/Remittance/jsRemExternals_min.js"   type = "text/javascript"></script>    
    <script src="../js/IRIS/jsIRIS.js"                  type = "text/javascript"></script>
    <script src="../js/js_sidebar_min.js"                  type = "text/javascript"></script>
    <script src="../js/popcalendar_min.js"                 type="text/javascript"></script>    
    
    
    <script type="text/javascript">        
        var cls_IRIS = new IRIS();  
        var controls  = new controls();                
    </script>

</head>

<body onresize="fixdim();" onload="javascript:fixdim(); controls.hide_div('div_results',true);controls.hide_div('div_results',true)">
<form runat="server" >
<!--
<br />
<br />
<br />
-->
    <div style="width: 100%; text-align:center" id="DIV_Search">
    <table style="width:100%">
    <tr>
    <td id="tdsearch" align="center" style="vertical-align:middle; height: 100%;">
    
        <table style="width:50%">
                            <tr>                               
                                <td align="center">
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width:100%" >
                                        <tr>
                                          <td>
                                            <!--
                                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;height: 24px">IRIS - Search</td>
                                            <td align="left" class="bg_body" style="height: 24px; width: 342px;" valign="bottom"><img align="absBottom" src="../images/rend.gif" /></td>
                                            -->
                                            <table cellpadding="0" cellspacing="0" style ="width:100%" >
                                                <tr>
                                                    <td align = "left" style = "width:10px; height:auto; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                                    <td align = "left" style = "width: 45%; height:auto; border-collapse:collapse" class="table_head_dark" >IRIS - Search</td>
                                                    <td align = "left" style = "width: 59%; height:auto; vertical-align:bottom; border-collapse:collapse"><img align="absBottom" src="../images/rend.gif" /></td> 
                                                </tr>
                                            </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 50%; height: 100%">
                                                <table class="table_border" style="width: 100%; height: 100%">                                                    
                                                    <tr>
                                                        <td align="center" style="text-align:center; vertical-align: top; width: 50%; height: 100%">
                                                            <!-- content starts here -->
                                                            <table cellpadding="0"  style="width:100%">
                                                                <tr>
                                                                    <td style="width:auto;height:auto;vertical-align: top">
                                                                        <table style="width:100%; border: solid 1px #336699;">
                                                                            <tr>
                                                                                <td class="table_caption" colspan="2" style="height: 30px; text-align: center;">
                                                                                    Search By:</td>
                                                                            </tr>
                                                                            <tr>
                                                                            <td class="cell_caption" style="width: 25%;">
                                                                                Reference Number :</td>
                                                                                <td class = "cell_desc" style="width: 25%; text-align: left;">
                                                                                <input id="txtRefno" name="txtRefno" style="width: 100" type="text"/></td>
                                                                            </tr>
                                                                                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px;">
                                                                                Remitter Last Name :</td>
                                                                                                                                            <td style="width: 25%; height: 23px; text-align: left;">
                                                                                <input id="txtRemLname" name="txtRemLname" type="text" /></td>
                                                                                                                                        </tr>
                                                                                                                                        <tr>
                                                                            <td class="cell_caption" style="width: 25%">
                                                                                Remitter First Name :</td>
                                                                                                                                            <td style="width: 25%; text-align: left;">
                                                                                <input id="txtRemFname" name="txtRemFname" type="text" /></td>
                                                                                                                                        </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%">
                                                                                    Beneficiary Last Name :</td>
                                                                                <td style="width: 25%; text-align: left;">
                                                                                    <input id="txtBeneLname" name="txtBeneLname" type="text" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 21px">
                                                                                    Beneficiary First Name :</td>
                                                                                <td style="width: 25%; height: 21px; text-align: left;">
                                                                                    <input id="txtBeneFname" name="txtBeneFname" type="text" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%">
                                                                                    Account Number/GFMC Card No. :</td>
                                                                                <td style="width: 25%; text-align: left;">
                                                                                    <input id="txtAcct" name="txtAcct" type="text" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                            <td class="cell_caption" style="width: 25%; height: 23px;">
                                                                                Transaction Date From :</td>
                                                                                <td style="width: 25%; height: 23px; text-align: left;">
                                                                                <input id="txtFDate" name="txtFDate" type="text" style="width: 55%" /> <img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtFDate, 'mm/dd/yyyy',document.getElementById('DIV_Search'));"></td>
                                                                            </tr>
                                                                            <tr>
                                                                            <td class="cell_caption" style="width: 25%">
                                                                                Transaction Date To :</td>
                                                                                <td style="width: 25%; text-align: left;">
                                                                                <input id="txtTDate" name="txtTDate" type="text" style="width: 55%" />
                                                                                    <img src="../images/calendar/imgcalen.png" / onclick="popUpCalendar(this, document.forms[0].txtTDate, 'mm/dd/yyyy',document.getElementById('DIV_Search'));"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>                                                                                                                                
                                                            </table>
                                                            <input id="Button1" type="button" value="Search" onclick="javascript:cls_IRIS.search();" style="width: 31%; height: 23px;" /> 
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
    </td>    
    </tr>
    </table>
    </div>    
    
    <div style="width: 100%; padding-top: 20px; overflow:auto" id="div_results" >
        <table style="width:100%">
                            <tr>
                                
                                <td id="tdresult" align="center" style="width: 100%; height:100%; vertical-align: middle;">
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 81%;" >
                                        <tr>
                                        <td>
                                           <!--
                                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="Td2" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                                height: 24px">Transaction Details</td>
                                            <td align="left" class="bg_body" style="height: 24px; width: 398px;" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                                -->
                                                <table cellpadding="0" cellspacing="0" style ="width:100%" >
                                                <tr>
                                                    <td align = "left" style = "width:10px; height:auto; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                                    <td align = "left" style = "width: 45%; height:auto; border-collapse:collapse" class="table_head_dark" >Transaction Details</td>
                                                    <td align = "left" style = "width: 59%; height:auto; vertical-align:bottom; border-collapse:collapse"><img align="absBottom" src="../images/rend.gif" /></td> 
                                                </tr>
                                            </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 100%">
                                                <table class="table_border" style="width: 100%; height: 100%">                                                    
                                                    <tr>
                                                        <td style="text-align:center; vertical-align: top; width: 77%; height: 95%">                                                            
                                                                        <table style="width: 100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid;
                                                                            border-bottom: #336699 1px solid"> <!--width="75%"-->
                                                                            <tr>
                                                                                <td class="table_caption" colspan="3" style="height: 23px">
                                                                                    Remittance
                                                                                    Transaction Details :</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 23px">
                                                                                    Transaction Date :</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 23px">
                                                                                    <label id="tranx_date">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                                    Clearing Date :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="clearing_date">
                                                                                        &nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                        Date Implemented :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="date_imp">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                        Transaction Ref. No.:</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="tranx_ref">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 15px">
                                                                        Remitting Overseas Office :</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 15px">
                                                                                    <label id="rem_office">
                                                                                        &nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 21px">
                                                                                    Branch Account Name :</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 21px">
                                                                                    <label id="3DBankName">&nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 21px">
                                                                                    Remitter ID :</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 21px">
                                                                                    <label id="rem_cif">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 21px">
                                                                                    Remitter Name :</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 21px">
                                                                                    <label id="Rem_name">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                                    Beneficiary Name :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="Bene_name">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 21px">
                                                                        Remitted (Gross) Amount :</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 21px">
                                                                                    <label id="Gross_amt">
                                                                                        &nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                        Net to Benef Amount :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="Net_amt">
                                                                                        &nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                        FX Rates Used :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="fx_rate">
                                                                                        &nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                        Product Type :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="product_type">
                                                                                        &nbsp; &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                                    Account No. :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="acctno">
                                                                                        &nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                        GFMC Card No./CK No.:</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="GFMC">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 18px;">
                                                                        Transaction Status:</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="height: 18px">
                                                                                    <label id="tranx_status">
                                                                                        &nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%; height: 18px">
                                                                                    Source:</td>
                                                                                <td id="Source" align="left" class="cell_desc" colspan="2" style="height: 18px">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 29%">
                                                                                    <span id="Label25" class="Label">Remit method:</span></td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="Sys_source">
                                                                                        &nbsp;&nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="3">
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        
                                                                        <input id="Button2" type="button" value="Back" onclick="javascript:controls.hide_div('DIV_Search',false);javascript:controls.hide_div('div_results',true);cls_IRIS.search();" style="width: 11%" /></td>
                                                                        
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
