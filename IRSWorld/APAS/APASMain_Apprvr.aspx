<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="APASMain_Apprvr.aspx.cs" Inherits="IRSWorld.APAS.APASMain_Apprvr" %>

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
    <script src="../js/APAS/jsAPAS_apprvr.js"          type = "text/javascript"></script>
    
    <script type="text/javascript">        
        var cls_APAS = new APAS();  
        var controls  = new controls();  
        var approval_window = new approval_window();      
    </script>
</head>
<body onload="javascript:controls.hide_div('div_results',true);cls_APAS.get_apas_trxnstat()">
    <form runat="server">
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
                <td style="vertical-align: top; text-align: center;">
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; margin-top:2%" width="50%">
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
                                                                <td class="cell_caption" style="width: 25%; height: 23px">
                                                                    Date Paid :</td>
                                                                <td style="width: 25%; height: 23px">
                                                                    <input id="txtDatePaid" name="txtFDate" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%">
                                                                    Reference Number :</td>
                                                                <td class="cell_desc" style="width: 25%">
                                                                    <input id="txtRefno" name="txtRefno" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%; height: 23px">
                                                                    Beneficiary Last Name :</td>
                                                                <td style="width: 25%; height: 23px">
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
                                                                <td class="cell_caption" style="width: 25%; height: 23px">
                                                                    Amount :</td>
                                                                <td style="width: 25%; height: 23px">
                                                                    <input id="txtAmount" name="txtAcct" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%">
                                                                    Paying Branch :</td>
                                                                <td style="width: 25%">
                                                                    <input id="txtPayBr" name="txtCK" type="text" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 25%">
                                                                    Transaction Status :</td>
                                                                <td style="width: 25%">
                                                                    <select id="cmb_trxn_stat" name="cmb_trxn_stat">
                                                                        <option selected="selected"></option>
                                                                    </select>
                                                                </td>
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
                <td id="td4" style="vertical-align: top; width: 10%; padding-top: 30px">
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
                <td style="vertical-align: top; width: 771px; text-align: center;">
                    <table cellpadding="0" cellspacing="0" style="background-color: azure; margin-top:2%" width="75%">
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
                                <table class="table_border" style="width: 100%; height: 100%">                                    
                                    <tr>
                                        <td style="vertical-align: top; width: 100%; height: 100%"> 
                                            <table cellpadding="0" width="100%">
                                                <tr>   
                                                <td>
                                                <table style="width:100%">
                                                <tr>
                                                    <td class="table_caption" colspan="3" style="height: 23px; width: 268px;">
                                                        TRANSACTION DETAILS
                                                    </td>
                                                </tr>
                                                </table>
                                                    <div id='div_result' style="overflow:auto;height:300px;">
                                                        <table>
                                                            
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 23px">
                                                                    Implementation Date :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 23px; width: 287px;">
                                                                    <label id="ImplemDate">
                                                                        Implementation Date
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px;">
                                                                    Ref No. Theirs (RB) :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px; height: 18px;">
                                                                    <label id="RefNoTheirs">
                                                                        Ref No. Theirs (RB)</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    Ref. No. Ours (ORD) :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="RefNoOurs">
                                                                        Ref. No. Ours (ORD)</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    ORC Div. :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="ORC">
                                                                        ORC Div.
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 15px">
                                                                    Beneficiary&nbsp; Name :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 15px; width: 287px;">
                                                                    <label id="BeneName">
                                                                        Beneficiary&nbsp; Name</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 21px">
                                                                    Beneficiary Address :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 21px; width: 287px;">
                                                                    <label id="BeneAdd">
                                                                        Beneficiary Address</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 21px">
                                                                    Beneficiary Contact Number : &nbsp;</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 21px; width: 287px;" id="BeneContactNo">
                                                                    Beneficiary Contact Number</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    Special Message :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="SpecialMSG">
                                                                        Special Message
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 21px">
                                                                    Purpose of Remittance :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 21px; width: 287px;">
                                                                    <label id="Purpose">
                                                                        Purpose of Remittance
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    Remitter ID :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="RemID">
                                                                        Remitter ID</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    Remitter's Name :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="RemName">
                                                                        Remitter's Name</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    Remitter Address :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="RemAdd">
                                                                        Remitter Address</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 12px;">
                                                                    Remitting Bank :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 12px; width: 287px;">
                                                                    <label id="RemBank">
                                                                        Remitting Bank
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px">
                                                                    Product Code :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 18px; width: 287px;">
                                                                    <label id="ProdCode">
                                                                        Product Code</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    Currency Code :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="CurrencyCode">
                                                                        Currency Code</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    <span id="Label25" class="Label">Amount Payable :</span></td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px">
                                                                    <label id="Amount">
                                                                        Amount Payable 999,999.99
                                                                    </label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%">
                                                                    Rate Used :
                                                                </td>
                                                                <td align="left" class="cell_desc" colspan="2" style="width: 287px" id="RateUsed">
                                                                    Rate Used</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px">
                                                                    EVAA :</td>
                                                                <td align="left" class="cell_desc" colspan="2" style="height: 18px; width: 287px;" id="EVAA">
                                                                    EVAA
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px">
                                                                    NSVS/CASA Account :</td>
                                                                <td class="cell_desc" colspan="2" style="height: 18px; text-align: left; width: 287px;" id="NSVS">
                                                                    NSVS/CASA Account</td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 29%; height: 18px">
                                                                    Transaction Status :</td>
                                                                <td id="trxn_stat" class="cell_desc" colspan="2" style="width: 287px; height: 18px;
                                                                    text-align: left">
                                                                    trxn_stat</td>
                                                            </tr>
                                                        </table>
                                                        <input id="id" style="width: 1%; height: 3px" type="hidden" /></div>
                                                                    <input id="activate" onclick="javascript:approval_window.open_window_approval();" style="width: 21%" type="button"
                                                            value="Activate" />
                                                                    <input id="Back" onclick="javascript:controls.hide_div('div_results',true);controls.hide_div('DIV_Search',false)" style="width: 21%" type="button"
                                                            value="Back" />
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
        
    </form>
</body>
</html>
