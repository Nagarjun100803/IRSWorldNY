<%--

Philippine National Bank
Developer: Rennier Borgoños
Description: Teller Cash Management program. Maintenance of Currency and its Denomination
Creation Date: 2009/09/10 
Version ID: IRSWOv2009.01.01
Related Files: CashTransactionEntry.aspx.cs / js_cash_tellercashmanagement.js
Restrictions: .Net Framework V2.0 / IE 6.0 or higher / javascript must be activated


--%>


<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CashTransactionEntry.aspx.cs" Inherits="IRSWorld.Cash.CashTransactionEntry" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Cash Breakdown - Transaction Entry</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css"></style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/Cash/js_cash_breakdown_txn.js" type="text/javascript"></script>        
</head>
<body onload="new cash_breakdown_txn().load_ddlCurrency();" onunload="close_session();">
    <form id="form1" runat="server">
        <div style="width:100%">
            <table style="width:90%" cellpadding="0" cellspacing="5">
                <tr>
                    <td align="center" valign="top" style="width:100%; border-collapse:collapse" class="content_height" colspan = "2">

                                <table style="width:80%"  cellpadding="0" cellspacing="5">
                                    <tr>
                                        <td  valign="top" style="height: 351px; width: 683px;" >
                                           <table id="tblCurrency" cellpadding="0" cellspacing="5"style="width:50%; display:block">
                                            
                                            <tr>
                                            
                                                <div id = "divCBMain" style =" display:block" >
                                                    <table cellspacing="0" cellpadding="0" style="width: 80%" >
                                                        <tr>
                                                            <td class="table_head_dark"  style="width:40%; height: 26px;">Cash Breakdown - Transaction Entry</td>
                                                            <td align="left" valign="bottom" style="width:40%; height: 26px;"> 
                                                                <img align="absbottom" src="../images/rend.gif" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3" style="height: 100%; width: 100%;">
                                                                <table style="width:100%; border:solid 3px #336699; height: 100%; background: azure">
                                                                    <tr>
                                                                        <td style="width: 100%; height: 80%;">
                                                                            <div id="divCurrency" style="width: 50%;display: block; vertical-align:middle; background: azure">
                                                                                  <table style="width:90%" cellpadding="0" cellspacing="5px">
                                                                                      <tr>
                                                                                            <td style="width: 100%; height: 100px;"valign="top">
                                                                                                <table id="Table2" cellpadding="0" cellspacing="5" style="width:80%; border: solid 1px #336699; height: 70px; background: azure">
                                                                                                    <tr>
                                                                                                        <td class="table_caption" style="width: 100%; text-align: left; text-indent: 5px; color: Black" colspan="3"> Currency</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td class="cell_caption" style="width: 60%; height: 23px">Currency:&nbsp;</td>
                                                                                                        <td align="left" style="width: 40%">
                                                                                                         <select name="ddlCurrency" size="1" style="   width: 200px; text-align: right; vertical-align: middle;" onchange="javascript:populate_denomination(this.value);">
                                                                                                            </select>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                   
                                                                                                </table>
                                                                                            </td>
                                                                                     </tr>
                                                                                    </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                      
                                                                    <!--  Grid Denomination -->
                                                                    <tr>
                                                                        <td style="vertical-align:middle; width:20%; height: 100%;"> 
                                                                                  
                                                                            <div id = "divDenomination" style =" display:block" >
                                                                                <table id="tblDenomination" cellspacing="0" cellpadding="0" style= "width:100%; display: block">
                                                                                        
                                                                                        <tr>
                                                                                            <td  style="width: 50%; background: azure">
                                                                                                <table id="tblGridDenomination" style="width:90%; border:solid 3px #336699;display: block">
                                                                                                    <tr>
                                                                                                </tr>
                                                                                                
                                                                                                    <tr>
                                                                                                        <td id="tdDenomination" style="width: 100%; height: 40px;">
                                                                                                            <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                                                                                            <!--
                                                                                                            <table id="tblCashM" width="100%" cellspacing="0" cellpadding="0">
                                                                                                                <tr>
                                                                                                                    <td></td>
                                                                                                                </tr>
                                                                                                            </table>
                                                                                                           --> 
                                                                                                            <!-- content ends here -->
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
