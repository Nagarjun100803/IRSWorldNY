<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IRISMain.aspx.cs" Inherits="IRSWorld.IRIS.IRISMain" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>IRIS - Search</title>
    
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>    
    <script src="../js/js_sidebar.js"                  type = "text/javascript"></script>
    <script src="../js/IRIS/jsIRIS_iuser.js"           type = "text/javascript"></script>
    
    <script type="text/javascript">
        
        var cls_IRIS = new IRIS();  
        var controls  = new controls();
        
    </script>
</head>

<body onload="javascript:javascript:controls.hide_div('div_results',true);">
<form runat="server" >
    <div style="width: 100%" id="DIV_Search" >
        <table cellpadding="0" cellspacing="5" width="100%">
            <tr>
                <td align="center" class="content_height" colspan="2" style="width: 100%; border-collapse: collapse;
                    height: 380px" valign="top">                    
                        <table width="100%">
                            <tr>
                                <td id="tdIRISMenu" style="width:20%; vertical-align:top; padding-top: 30px;">                                    
                                    <div id="divtdIRISMenu" style="margin-top: 20px; display: block">        
                                    <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>                                
                                    </div>                                    
                                </td>
                                <td style="vertical-align: top">
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure" width="96%" >
                                        <tr>
                                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                                height: 24px">
                                                IRIS - Search</td>
                                            <td align="left" class="bg_body" style="height: 24px" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 100%">
                                                <table class="table_border" style="width: 100%; height: 100%">
                                                    <tr>
                                                        <td style="width: 100%; height: 8px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; width: 100%; height: 100%">
                                                            <!-- content starts here -->
                                                            <table cellpadding="0" cellspacing="5" width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table width="50%" style="border: solid 1px #336699;">
                                                                            <tr>
                <td class="cell_caption" style="width: 25%; height: 23px;">
                    Transaction Date From :</td>
                                                                                <td style="width: 25%; height: 23px;">
                    <input id="txtFDate" name="txtFDate" type="text" value="12/01/2009" /></td>
                                                                            </tr>
                                                                            <tr>
                <td class="cell_caption" style="width: 25%">
                    Transaction Date To :</td>
                                                                                <td style="width: 25%">
                    <input id="txtTDate" name="txtTDate" type="text" value="12/15/2009" /></td>
                                                                            </tr>
                                                                            <tr>
                <td class="cell_caption" style="width: 25%;">
                    Reference Number :</td>
                                                                                <td class = "cell_desc" style="width: 25%;">
                    <input id="txtRefno" name="txtRefno" style="width: 100" type="text"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%">
                                                                                    Beneficiary Last Name :</td>
                                                                                <td style="width: 25%">
                                                                                    <input id="txtBeneLname" name="txtBeneLname" type="text" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 21px">
                                                                                    Beneficiary First Name :</td>
                                                                                <td style="width: 25%; height: 21px">
                                                                                    <input id="txtBeneFname" name="txtBeneFname" type="text" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%">
                                                                                    Beneficiary Middle Name :</td>
                                                                                <td style="width: 25%">
                                                                                    <input id="txtBeneMname" name="txtBeneMname" type="text" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%">
                                                                                    Account Number :</td>
                                                                                <td style="width: 25%">
                                                                                    <input id="txtAcct" name="txtAcct" type="text" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 25%; height: 23px;">
                                                                                    GFMC Card No. / CK No. :</td>
                                                                                <td style="width: 25%; height: 23px;">
                                                                                    <input id="txtCK" name="txtCK" type="text" /></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                    <input id="Button1" type="button" value="search" onclick="javascript:cls_IRIS.search();" style="width: 11%" /></td>
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
                </td>
            </tr>
        </table>
    </div>    
    <div style="width: 100%" id="div_results" >
        <table cellpadding="0" cellspacing="5" width="100%">
            <tr>
                <td align="center" class="content_height" colspan="2" style="width: 100%; border-collapse: collapse;
                    height: 380px" valign="top">                    
                        <table width="100%">
                            <tr>
                                <td id="td1" style="width:20%; vertical-align:top; padding-top: 30px;">                                    
                                    <div id="div3" style="margin-top: 20px; display: block">        
                                   <script type="text/javascript">
                                        
                                    </script>                               
                                    </div>                                    
                                </td>
                                <td style="vertical-align: top; width: 760px;">
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure" width="96%" >
                                        <tr>
                                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="Td2" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                                height: 24px">
                                    Transaction Details</td>
                                            <td align="left" class="bg_body" style="height: 24px" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 100%">
                                                <table class="table_border" style="width: 100%; height: 100%">
                                                    <tr>
                                                        <td style="width: 100%; height: 8px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="vertical-align: top; width: 100%; height: 100%">
                                                            <!-- content starts here -->
                                                            <table cellpadding="0" cellspacing="5" width="100%">
                                                                <tr>
                                                                    <td>
                                                            <table width="75%" style="border: solid 1px #336699;">
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; height: 23px;">
                                                                        Transaction Date :</td>
                                                                    <td class = "cell_desc" style="height: 23px;" align="left" colspan="2">
                                                                        <label id="tranx_date">tranx_date
                                                                            </label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        Clearing Date :</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="clearing_date">clearing_date
                                                                            </label>                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%;">
                                                                        Date Implemented :</td>
                                                                    <td class = "cell_desc" align="left" colspan="2">
                                                                    <label id="date_imp">date_imp</label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        Transaction Ref. No.:</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="tranx_ref">tranx_ref
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; height: 15px;">
                                                                        Remitting Overseas Office :</td>
                                                                    <td class = "cell_desc" style="height: 15px;" align="left" colspan="2"><label id="rem_office">rem_office
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; height: 21px;">
                                                                        Remitter ID :</td>
                                                                    <td class = "cell_desc" style="height: 21px;" align="left" colspan="2"><label id="rem_cif">rem_cif
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; height: 21px;">
                                                                        Remitter Name :</td>
                                                                    <td class = "cell_desc" style="height: 21px;" align="left" colspan="2"><label id="Rem_name">Rem_name
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        Beneficiary Name :</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="Bene_name">Bene_name
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%; height: 21px">
                                                                        Remitted (Gross) Amount :</td>
                                                                    <td class = "cell_desc" style="height: 21px" align="left" colspan="2"><label id="Gross_amt">Gross_amt 999,999.99
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        Net to Benef Amount :</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="Net_amt">Net_amt 999,999.99
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        FX Rates Used :</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="fx_rate">fx_rate 999.999
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        Product Type :</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="product_type">product_type
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        Account No. :</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="acctno">acctno
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        GFMC Card No./CK No.:</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="GFMC">GFMC
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        Transaction Status:</td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="tranx_status">tranx_status
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                        <span id="Label25" class="Label">System Source:</span></td>
                                                                    <td class = "cell_desc" align="left" colspan="2"><label id="Sys_source">Sys_source
                                                                            </label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="cell_caption" style="width: 25%">
                                                                    </td>
                                                                    <td class = "cell_desc" style="width: 25%">
                                                                    </td>
                                                                    <td class="cell_desc" style="width: 174px">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2" style="height: 22px">
                                                                        </td>
                                                                    <td colspan="1" style="height: 22px; width: 174px;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <input id="Button2" type="button" value="back" onclick="javascript:controls.hide_div('DIV_Search',false);javascript:controls.hide_div('div_results',true);cls_IRIS.search();" style="width: 11%" /></td>
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
                </td>
            </tr>
        </table>
    </div>
    <center>   &nbsp;</center>
    
</form>           
</body>
</html>
