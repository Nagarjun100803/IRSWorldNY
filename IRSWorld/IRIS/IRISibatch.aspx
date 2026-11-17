<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IRISibatch.aspx.cs" Inherits="IRSWorld.IRIS.IRIS_iBatch" %>
<%@ Register Src="../Controls/MessageBoard.ascx" TagName="msg_board" TagPrefix="uc3" %>
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
    <script src="../js/IRIS/jsIRIS_ibatch.js"          type = "text/javascript"></script>
    <script src="../js/js_sidebar.js"                  type = "text/javascript"></script>    
    <script src="../js/popcalendar.js"                 type="text/javascript"></script>
    
    <script type="text/javascript">
        
        var cls_IRIS = new IRIS();  
        var controls  = new controls();
        
        
    </script>
    <script type="text/javascript">
        document.onkeydown = function(ev) 
        {
            var key;
            ev = ev || event;
            key = ev.keyCode;
            if (key == 13) 
            {
               cls_IRIS.search();
            }
        }
    </script>
</head>

<body onload="javascript:javascript:controls.hide_div('div_results',true);">
<form runat="server" >
    <div style="width: 100%" id="DIV_Search" >
        <table cellpadding="0" cellspacing="5" style="width:100%">
            <tr>
                <td align="center" class="content_height" colspan="2" style="width: 100%; border-collapse: collapse;
                    height: 380px" valign="top">                    
                    <br />
                    <br />
                    <br />
                        <table style="width:100%">
                            <tr>
                                <td id="tdIRISMenu" style="width:10%; vertical-align:top; padding-top: 30px;">                                    
                                    <div id="divtdIRISMenu" style="margin-top: 20px; display: block">        
                                    <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>                                
                                    </div>                                    
                                </td>
                                <td style="vertical-align: top">
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width:50%" >
                                        <tr>
                                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="capResult" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                                height: 24px">
                                                IRIS - Search (Batch)</td>
                                            <td align="left" class="bg_body" style="height: 24px; width: 342px;" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 50%; height: 100%">
                                                <table class="table_border" style="width: 100%; height: 100%">
                                                    
                                                    <tr>
                                                        <td style="vertical-align: top; width: 50%; height: 100%">
                                                            <!-- content starts here -->
                                                            <table cellpadding="0"  style="width:100%">
                                                                <tr>
                                                                    <td style="width:100%; height: 283px">
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
                                                                                <input id="txtFDate" name="txtFDate" type="text" style="width: 55%" />
                                                                                    <img onclick="popUpCalendar(this, document.forms[0].txtFDate, 'mm/dd/yyyy',document.getElementById('DIV_Search'));"
                                                                                        src="../images/calendar/imgcalen.png" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                            <td class="cell_caption" style="width: 25%">
                                                                                Transaction Date To :</td>
                                                                                <td style="width: 25%; text-align: left;">
                                                                                <input id="txtTDate" name="txtTDate" type="text" style="width: 55%" />
                                                                                    <img onclick="popUpCalendar(this, document.forms[0].txtTDate, 'mm/dd/yyyy',document.getElementById('DIV_Search'));"
                                                                                        src="../images/calendar/imgcalen.png" /></td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                    </td>
                                                                </tr>                                                                
                                                            </table>
                                                            <!-- content ends here -->
                                                        <input id="Button1" type="button" value="Search" onclick="javascript:cls_IRIS.search();" style="width: 31%" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                 <td id="td4" style="width:10%; vertical-align:top; padding-top: 30px;">                                    
                                    <div id="div2" style="margin-top: 20px; display: block">        
                                    <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>                                
                                    </div>                                    
                                </td>
                            </tr>
                        </table>                    
                </td>
            </tr>
        </table>
    </div>    
    <div style="width: 100%" id="div_results" >
        <table cellpadding="0" cellspacing="5" style="width:100%">
            <tr>
                <td align="center" class="content_height" colspan="2" style="width: 100%; border-collapse: collapse;
                    height: 380px" valign="top">                    
                    <br />
                        <table style="width:100%">
                            <tr>
                                <td id="td1" style="width:10%; vertical-align:top; padding-top: 30px;">                                    
                                    <div id="div3" style="margin-top: 20px; display: block">        
                                    <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>                                
                                    </div>                                    
                                </td>
                                <td style="vertical-align: top; width: 821px;">
                                    <table cellpadding="0" cellspacing="0" style="background-color: azure; width: 100%;" >
                                        <tr>
                                            <td align="left" style="width: 1%; height: 24px" valign="bottom">
                                                <img src="../images/toe.GIF" /></td>
                                            <td id="Td2" align="left" class="table_head_dark" style="width: 40%; border-collapse: collapse;
                                                height: 24px">
                                                Batch Transaction Details</td>
                                            <td align="left" class="bg_body" style="height: 24px; width: 416px;" valign="bottom">
                                                <img align="absBottom" src="../images/rend.gif" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="width: 100%; height: 100%">
                                                <table class="table_border" style="width: 100%; height: 100%">                                                    
                                                    <tr>
                                                        <td style="vertical-align: top; width: 64%; height: 95%">                                                            
                                                                        <table style="width: 100%;border-right: #336699 1px solid; border-top: #336699 1px solid; border-left: #336699 1px solid;
                                                                            border-bottom: #336699 1px solid"> <!--width="75%"-->
                                                                            <tr>
                                                                                <td class="table_caption" colspan="5" style="height: 23px">
                                                                                    Remittance
                                                                                    Transaction Details :</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                        Transaction Status :</td>
                                                                                <td align="left" class="cell_desc" colspan="2" style="width: 32%;">
                                                                                <label id="tranx_status" ></label>
                                                                                </td><td class="cell_caption" style="width: 182px;">
                                                                                    Date Received :</td>
                                                                                <td align="left" class="cell_desc" colspan="1" style="width: 217px;">
                                                                                    <label id="DateReceived" ></label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                                    Source :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="Source">
                                                                                        </label></td><td class="cell_caption" style="width: 182px;">
                                                                                            Transaction Date :</td>
                                                                                <td align="left" class="cell_desc" colspan="1" style="width: 217px;">
                                                                                    <label id="tranx_date">
                                                                                        </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                        Transaction Ref. No. :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="tranx_ref"></label></td><td class="cell_caption" style="width: 182px">
                                                                        Date Implemented :</td>
                                                                                <td align="left" class="cell_desc" colspan="1" style="width: 217px">
                                                                                <label id="date_imp">
                                                                                        </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                                    RefnumTheirs :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="RefnumTheirs">
                                                                                        </label></td><td class="cell_caption" style="width: 182px">
                                                                                    Clearing Date :</td>
                                                                                <td align="left" class="cell_desc" colspan="1" style="width: 217px">
                                                                                <label id="clearing_date"></label>
                                                                                </td>
                                                                            </tr>
                                                                            
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                                    Beneficiary Name :</td>
                                                                                <td align="left" class="cell_desc" colspan="4">
                                                                                    <label id="Bene_name">
                                                                                        </label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                                    Account No. :</td>
                                                                                <td id="" align="left" class="cell_desc" colspan="4">
                                                                                <label id="acctno" ></label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                        GFMC Card No./CK :</td>
                                                                                <td id="" align="left" class="cell_desc" colspan="4">
                                                                                <label id="GFMC" ></label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                                    Bene Address :</td>
                                                                                <td align="left" class="cell_desc" colspan="4">
                                                                                </td>
                                                                            </tr>
                                                                            
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                                    Remitter ID :</td>
                                                                                <td align="left" class="cell_desc" colspan="4">
                                                                                    <label id="rem_cif">
                                                                                        </label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                                    Remitter Name :</td>
                                                                                <td align="left" class="cell_desc" colspan="4">
                                                                                    <label id="Rem_name">
                                                                                        </label></td>
                                                                            </tr>
                                                                              <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                                    Remitting BankName :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                     <label id="RemittingBankName">
                                                                                        </label></td><td class="cell_caption" style="width: 182px">
                                                                                    PayBank Name :</td>
                                                                                <td align="left" class="cell_desc" colspan="1" style="width: 217px">
                                                                                 <label id="PayBankName">
                                                                                        </label>
                                                                                </td>
                                                                            </tr>
                                                                             <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                                    Branch :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                     <label id="BranchAccountName">
                                                                                        </label></td><td class="cell_caption" style="width: 182px">
                                                                        Remitting Overseas Office :</td>
                                                                                <td align="left" class="cell_desc" colspan="1" style="width: 217px">
                                                                                 <label id="rem_office">
                                                                                        </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                        Remitted (Gross) Amount :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="Gross_amt">
                                                                                        &nbsp;</label></td><td class="cell_caption" style="width: 182px;">
                                                                        FX Rates Used :</td>
                                                                                <td id="" align="left" class="cell_desc" colspan="1" style="width: 217px;">
                                                                                 <label id="fx_rate">
                                                                                        </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px;">
                                                                        Net to Benef Amount :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="Net_amt">
                                                                                        &nbsp;</label></td><td class="cell_caption" style="width: 182px;">
                                                                                    Remit method :</td>
                                                                                <td id="" align="left" class="cell_desc" colspan="1" style="width: 217px;">
                                                                                 <label id="Sys_source">
                                                                                        </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                        Product Type :</td>
                                                                                <td align="left" class="cell_desc" colspan="2">
                                                                                    <label id="product_type">
                                                                                        </label></td><td class="cell_caption" style="width: 182px">
                                                                                    Implementation :
                                                                                </td>
                                                                                <td id="Td5" align="left" class="cell_desc" colspan="1" style="width: 217px">
                                                                                 <label id="Implementation">
                                                                                        </label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                                    <span id="Label25" class="Label">
                                                                                    Processor :</span></td>
                                                                                <td align="left" class="cell_desc" colspan="4">
                                                                                    <label id="Processor">
                                                                                        &nbsp;&nbsp;</label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                                    Special Message :</td>
                                                                                <td id="" align="left" class="cell_desc" colspan="4">
                                                                                <label id="SpecialMessage">
                                                                                        &nbsp;&nbsp;</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td class="cell_caption" style="width: 160px">
                                                                                    Courier :</td>
                                                                                <td id="" align="left" class="cell_desc" colspan="4">
                                                                                <label id="Courier">
                                                                                        &nbsp;&nbsp;</label>
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
                                 <td id="td3" style="width:11%; vertical-align:top; padding-top: 30px;">                                    
                                    <div id="div1" style="margin-top: 20px; display: block">        
                                    <script type="text/javascript">
                                           // var sider = new js_sidebar('sidemenu', sidebar);
                                            //document.write(sider.toString());
                                        </script>                                
                                    </div>                                    
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
