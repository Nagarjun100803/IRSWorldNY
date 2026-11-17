<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AccountTransactionStatus.aspx.cs"
    Inherits="IRSWorld.Transaction.AccountTransactionStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Account Transaction Status</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />

    <script src="../js/datagrid.js" type="text/javascript""></script>

    <script src="../js/commons.js" type="text/javascript""></script>

    <script src="../js/jscommonmethod.js" type="text/javascript"></script>

    <script src="../js/Transaction/jsAccountTransactionStatus.js?v=01202015" type="text/javascript"></script>

    <script type="text/javascript">
        var rem_search = new AccountTransactionStatus();
    
        function fix_height()
        {
            if (top.document.getElementById("divCenter"))
            {
                var cent = top.document.getElementById("divCenter");
                var div = document.getElementById("divmain");
                if (div)
                {
                    div.style.width = cent.offsetWidth + "px";
                    div.style.height = cent.offsetHeight + "px";
                    
                    var divr = document.getElementById('div_results');
                    if (divr)
                    {
                        divr.style.height = div.style.height;
                    }
                }
            }
        }    
    </script>

</head>
<body onload="fix_height();javascript:rem_search.initialize_page();" onresize="fix_height();">
    <form id="form1" runat="server">
        <div id="divmain" style="width: 100%; height: 100%; padding-top: 30px;">
            <table style="width: 100%; border-collapse: collapse;" cellpadding="0" cellspacing="5">
                <tr>
                    <td id="tdMain" style="width: 82%; vertical-align: top;">
                        <center>
                            <br />
                            <div id="div_search" style="width: 100%; display: block;">
                                <table cellspacing="0" cellpadding="0" style="width: 90%">
                                    <tr>
                                        <td style="width: 100%; vertical-align: bottom" align="left">
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="left" style="width: 1%; height: 24px; border-collapse: collapse; vertical-align: bottom;">
                                                        <img src="../images/toe.GIF" /></td>
                                                    <td id="capSearch" class="table_head_dark" style="width: 30%; height: 24px; border-collapse: collapse"
                                                        align="left">
                                                        Search</td>
                                                    <td align="left" style="width: 54%; height: 24px; vertical-align: bottom; border-collapse: collapse">
                                                        <img align="absBottom" src="../images/rend.gif" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="width: 100%; background: azure; height: 195px;">
                                            <table style="width: 100%;" class="table_border">
                                                <tr>
                                                    <td style="width: 92%; height: 21px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 92%; height: 49px;">
                                                        <!-- content starts here -->
                                                        <table cellpadding="0" cellspacing="0" class="table_body" style="width: 100%; border: solid 1px skyblue">
                                                            <tr>
                                                                <td style="width: 100%; height: 115px" align="center" valign="middle">
                                                                    <div>
                                                                        <table style="width: 96%" cellspacing="5" cellpadding="0" id="tblSearch">
                                                                            <tr>
                                                                                <td align="right" class="cell_desc" style="width: 10%; height: 25px;">
                                                                                    &nbsp;Search by :</td>
                                                                                <td style="width: 55%; height: 49px;">
                                                                                    <!-- content starts here -->
                                                                                    <select id="ddSearchType" style="width: 98%; text-align: left; font-size: medium;"
                                                                                        onchange="rem_search.showSearchType();">
                                                                                        <option value="">--Please Select--</option>
                                                                                        <option value="Name">Search by Name</option>
                                                                                        <option value="AccountNumber">Search by Account Number</option>
                                                                                        <option value="Filter">Search by Filter</option>
                                                                                    </select>
                                                                                </td>
                                                                                <td style="width: 35%;">                                                                                
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 10%;">
                                                                                </td >
                                                                                <td style="width: 55%;">
                                                                                    <table cellpadding="0" cellspacing="0" class="table_body" style="width: 100%;">
                                                                                        <tr id="trName" style="display: none">
                                                                                            <td style="width: 100%; height: 115px" align="center" valign="middle">
                                                                                                <div>
                                                                                                    <table style="width: 96%" cellspacing="5" cellpadding="0" id="Table1">
                                                                                                        <tr>
                                                                                                            <td align="right" class="cell_desc" style="width: 35%; height: 25px;">
                                                                                                                &nbsp;First Name. :</td>
                                                                                                            <td align="left" style="width: 65%; height: 25px;">
                                                                                                                <input id="txtSearchFirstName" type="text" style="width: 98%; text-align: left; font-size: medium;"
                                                                                                                    onkeyup="checkinput();" /></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="right" class="cell_desc" style="width: 35%; height: 25px;">
                                                                                                                &nbsp;Middle Name. :</td>
                                                                                                            <td align="left" style="width: 65%; height: 25px;">
                                                                                                                <input id="txtSearchMiddleName" type="text" style="width: 98%; text-align: left;
                                                                                                                    font-size: medium;" onkeyup="checkinput();" /></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="right" class="cell_desc" style="width: 35%; height: 25px;">
                                                                                                                &nbsp;Last Name. :</td>
                                                                                                            <td align="left" style="width: 65%; height: 25px;">
                                                                                                                <input id="txtSearchLastName" type="text" style="width: 98%; text-align: left; font-size: medium;"
                                                                                                                    onkeyup="checkinput();" /></td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trFilter" style="display: none">
                                                                                            <td style="width: 100%; height: 50px" align="center" valign="middle">
                                                                                                <div>
                                                                                                    <table style="width: 96%" cellspacing="5" cellpadding="0" id="Table2">
                                                                                                        <tr>
                                                                                                            <td align="right" class="cell_desc" style="width: 35%; height: 25px;">
                                                                                                                &nbsp;Filter :</td>
                                                                                                            <td align="left" class="cell_desc" style="width: 65%; height: 25px;">
                                                                                                                <select id="ddFilterType" style="width: 98%; text-align: left; font-size: medium;"
                                                                                                                    onchange="checkinput();">
                                                                                                                    <option value="Blank"></option>
                                                                                                                    <option value="IsValidated">Validated</option>
                                                                                                                    <option value="Unvalidated">Unvalidated</option>
                                                                                                                    <option value="Deleted">Deleted</option>
                                                                                                                    <option value="UserDeleted">User Deleted</option>
                                                                                                                    <option value="DeletedAndUserDeleted">Deleted and User Deleted</option>
                                                                                                                    <option value="ShowAll">Show All</option>
                                                                                                                </select>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trAccountNumber" style="display: none">
                                                                                            <td style="width: 100%; height: 50px" align="center" valign="middle">
                                                                                                <div>
                                                                                                    <table style="width: 96%" cellspacing="5" cellpadding="0" id="Table3">
                                                                                                        <tr id="tr1" style="display: block">
                                                                                                            <td align="right" class="cell_desc" style="width: 35%; height: 25px;">
                                                                                                                &nbsp;Account No. :
                                                                                                            </td>
                                                                                                            <td align="left" style="width: 65%; height: 25px;">
                                                                                                                <input id="txtSearchAccountNumber" type="text" style="width: 98%; text-align: left;
                                                                                                                    font-size: medium;" onkeyup="checkinput();" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trButtons" style="display: none" align="center">
                                                                                            <td style="height: 28px" valign="top" colspan="2" align="center">
                                                                                                <input type="button" value="Search" onclick="rem_search.loadRecord();" class="button"
                                                                                                    onmouseover="this.className='button_over';" onmouseout="this.className='button';"
                                                                                                    id="btnSearchSearch" />&nbsp;
                                                                                                <input type="button" value="Clear" onclick="rem_search.clearFields();" class="button"
                                                                                                    onmouseover="this.className='button_over';" onmouseout="this.className='button';"
                                                                                                    id="btnSearchClear" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="width: 35%;">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="tdReportLookup" colspan="2" style="width: 100%; border-collapse: collapse;
                                                                    height: 21px; display: none;">
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
                        </center>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
