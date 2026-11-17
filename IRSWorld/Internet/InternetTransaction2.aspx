<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InternetTransaction2.aspx.cs" Inherits="IRSWorld.Internet.InternetTransaction2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Internet Transaction</title>    
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js"                 type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"            type="text/javascript"></script>
    <script src="../js/jshttprequest.js"             type="text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"             type="text/javascript"></script>
    <script src="../js/jsgridselector.js"            type="text/javascript"></script>
    <script src = "../js/Internet/jsInternetTransaction2.js" type="text/javascript"></script>
    <script type="text/javascript">
        var Internet_Transaction2 = new Internet_Transaction2();    
    </script>
    <script type="text/javascript">
        function load_main()
        {
            document.getElementById("txtHiddenRemCIFID").value = '1072102109000001'; //cifid;
            Internet_Transaction2.CIFID = document.getElementById("txtHiddenRemCIFID").value;
            Internet_Transaction2.show_loading_progress();
            Internet_Transaction2.get_profile();
            Internet_Transaction2.load_txn_hist();
            Internet_Transaction2.load_dd_beneficiaries();
        }
        
    </script>
</head>
<body onload="javascript:load_main();">
    <form id="form1" runat="server">
    <div>
        <table style="width:100%">
            <tr>
                <td style=" vertical-align:top; width: 15%;">
                </td>
                <td style=" vertical-align:top; width: 85%;">
                    <table cellspacing="0" cellpadding="0" style="width:96%; background-color:Azure">
                        <tr>
                            <td align="left" valign="bottom" style="width:1%;height:24px;"> 
                                <img src="../images/toe.GIF" /></td>
                            <td id="capResult" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse;">Internet - Transaction</td>
                            <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                <img align="absbottom" src="../images/rend.gif" /></td>
                        </tr>
		                <tr>
			                <td colspan="3" style="width: 100%; height: 100%">
			                    <input id="txtHiddenRemCIFID" type="hidden" style="width: 5%" />
				                <table style="width:100%; height: 100%;" class="table_border" cellpadding="0" cellspacing="8" >					                    
				                    <tr>
				                        <td colspan="2">
				                            <table id="Table1" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699">
				                                <tr>
				                                <td colspan="2" align="center"><label id="lblRemFullName" class="cell_desc" style="font-size: 20px; color: blue;" >Alvin C. Santos</label></td>
				                                </tr>
				                                <tr>
				                                    <td colspan="2" align="center"><label id="lblRemAddress" class="cell_desc" style="font-size: medium; font-weight: normal;" >Via Fauche 37, Milan, Italy</label></td>
				                                </tr>
				                                <tr>
				                                    <td class="table_caption" colspan="2"; align="center">Transaction History</td>
				                                </tr>
				                                <tr>
                                                    <td colspan="2"><div id = "GRIDTXNHIST">PLACE GRIDVIEW ACCOUNTS</div></td>
                                                </tr>
				                            </table>
				                        </td>
				                    </tr>
				                            
                                    <tr>
                                        <td colspan="2">
                                            <table id="tblInternetRRRemBeneficiary" cellpadding="0" cellspacing="8" style="width:100%; border: solid 1px #336699">
                                                <tr>
                                                    <td class="cell_caption" style="width:20%"><label>Select Beneficiary:&nbsp;</label></td>
                                                    <td>
                                                        <select id="ddBeneficiaries" style="width: 100%">
                                                            <option selected="selected"></option>
                                                        </select>
                                                    </td>
                                                </tr>
                                                <tr>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%; height: 23px">Beneficiary Name :&nbsp;</td>
                                                    <td align="left" style="width: 85%" colspan = "3"><label id="lblInternetRRRemBeneName" class="cell_desc" style="font-size: medium;" >&nbsp;<span style="font-size: 13pt">Janet Romasanta</span></label></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%; height: 34px">Address :&nbsp;</td>
                                                    <td align="left" style="width: 85%; height: 34px;" colspan = "3"><label id="lblInternetRRRemBeneAddress" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Lot 12, Block 2, PH-1, Tierra De Sta Maria, Pulang Buhangin, Sta. Maria, Bulacan,
                                                        Philippines, 3022</label></td>
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%; height: 23px">PNB Branch :&nbsp;</td>
                                                    <td align="left" style="width: 35%"><label id="lblInternetRRRemPNBBranch" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;1394 PNB Sta. Maria Bulacan</label></td>
                                                    <td class="cell_caption" style="width: 15%; height: 23px">CA / SA No. :&nbsp;</td>
                                                    <td align="left" style="width: 35%"><label id="lblInternetRRRemBeneCASANo" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;11112222333374444</label></td>                                                                                        
                                                </tr>
                                                <tr>
                                                    <td class="cell_caption" style="width: 15%; height: 23px">Account Type :&nbsp;</td>
                                                    <td align="left" style="width: 35%"><label id="lblInternetRRRemBeneAccountType" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;Savings</label></td>
                                                    <td class="cell_caption" style="width: 15%; height: 23px">A/C Currency :&nbsp;</td>
                                                    <td align="left" style="width: 35%"><label id="lblInternetRRRemBeneACCurrency" class="cell_desc" style="font-size: 15px; font-weight: normal;" >&nbsp;PHP</label></td>                                                                                        
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
    </form>
</body>
</html>
