<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranSearch.aspx.cs" Inherits="IRSWorld.Remittance.RemTranSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remittance Transcations Search</title> 
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>
    <script src="../js/Remittance/jsremittersearch.js" type = "text/javascript"></script>        
    
    <script type="text/javascript">    
        var txn_rem_search = new remitter_search();
    </script>
    
</head>

<body>
    <div id = "REMTRANSEARCHMAIN">
        <form id="frmREMTRANSEARCH" runat="server" >
        <table width="100%"  cellpadding="0" cellspacing="5">
            <tr>
                <td style="background: azure"> 
                    <div id = "REMTRANSEARCHENTRY">
                        <table width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <table cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                            <img src="../images/toe.GIF" /></td>    
                                        <td class="table_head_dark" style="width: 50%; height: 24px">
                                            Remittance Transaction</td>
                                        <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                     </tr>                                                                              
                                    </table>    
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table id="tblREMTRANSEARCH" style="width:100%; background:azure;" class="table_border">
                                        <tr>
                                            <td colspan="2">
                                                <div id = "REMTRANSEARCHENTRIES">
                                                     <table id="Table1" width="100%" cellpadding="0" style="border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" colspan = "2" style="width:50%; text-align: left; text-indent: 5px; color: Black; height: 26px;" valign="top" >
                                                                Remitter Search</td>
                                                            <td align="left" style="width: 10%; height: 24px;">
                                                                <input id="btnCreateRemitter" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Create Remitter" style="width: 100%" />
                                                            </td>
                                                            <td align="left" style="width: 10%; height: 24px;">
                                                                <input id="btnCreateBene" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Create Beneficiary" style="width: 100%"/>
                                                            </td>
                                                            <td align="left" style="width: 5%; height: 24px;">
                                                                <input id="btnSearchBene" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" onclick = "javascript:txn_rem_search.search_remitter();" type="button" value="Search" style="width: 100%"/>
                                                            </td>                                                            
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 14%; height: 24px">Last Name:&nbsp;</td>
                                                            <td align="left" colspan="4" style="width: 75%; height: 24px">
                                                                <input id="txtSearchLName" style="width: 34%" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 14%; height: 24px" >First Name:&nbsp;</td>
                                                            <td align="left" style="width: 75%; height: 24px;" colspan = "4">
                                                                <input id="txtSearchFName" style="width: 34%" type="text" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 14%; height: 24px" >Middle Name:&nbsp;</td>
                                                            <td align="left" style="width: 75%; height: 24px;" colspan = "4">
                                                                <input id="txtSearchMName" style="width: 34%" type="text" /></td>
                                                        </tr>                                                                                
                                                         <tr>
                                                             <td class="cell_caption" style="width: 14%; height: 24px">
                                                                 Search By:</td>
                                                             <td align="left" colspan="4" style="width: 75%; height: 24px">
                                                                 <select id="ddSearchBy" style="width: 35%">
                                                                     <option selected="selected"></option>
                                                                 </select>
                                                             </td>
                                                         </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width:50%">
                                                <div id="BENEINFO">
                                                    <table id="tblBeneInfo" width="100%" cellpadding="0" style="border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="width: 90%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="3">
                                                                Beneficiaries Information
                                                            </td>
                                                            <td style="width:10%;">
                                                                <input id="btnChangeProdType" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Change Product Type" style="width: 100%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 46%; height: 24px">
                                                                Product Type:&nbsp;</td>
                                                            <td align="left" style="width: 25%; height: 24px" colspan="3">
                                                                <label id="lblBeneProdType" class="cell_desc">Product Type&nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 46%; height: 24px">
                                                                Implementation Type:&nbsp;</td>
                                                            <td align="left" style="width: 25%; height: 24px" colspan="3">
                                                                <label id="lblBeneImpType" class="cell_desc">
                                                                    Implementation Type&nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 46%; height: 24px">Last Name | Business Name:&nbsp;</td>
                                                            <td align="left" style="width: 30%; height: 24px" colspan="3">
                                                                <label id="lblBeneLName" class="cell_desc">Beneficiary Last Name&nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 46%; height: 24px">First Name | Short Name:&nbsp;</td>
                                                            <td align="left" style="width: 30%; height: 24px" colspan="3">
                                                                <label id="lblBeneFName" class="cell_desc">Beneficiary First Name&nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 46%; height: 24px" >Middle Name | Business Type:&nbsp;</td>
                                                            <td align="left" style="width: 25%; height: 24px;" colspan="3">
                                                                <label id="lblBeneMName" class="cell_desc">Beneficiary Middle Name&nbsp;</label></td> 
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 46%; height: 24px">Address: &nbsp;</td>
                                                             <td align="left" valign="top" style="width: 25%; height: 24px" colspan="3" rowspan="2">
                                                                 <label id="lblBeneAddress" class="cell_desc">Beneficiary Address Beneficiary Address Beneficiary Address Beneficiary Address
                                                                     </label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 46%; height: 24px">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                            <td style="width:50%">
                                                <div id="REMINFO">
                                                    <table id="tblRemInfo" width="100%" cellpadding="0" style="border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="width: 90%; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="3">
                                                                Remitters Information
                                                            </td>
                                                            <td style="width:10%;">
                                                                <input id="btnTransact" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Transact" style="width: 100%" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 40%; height: 24px">Last Name | Business Name:&nbsp;</td>
                                                            <td align="left" style="width: 30%; height: 24px" colspan="3">
                                                                <label id="lblRemLName" class="cell_desc">Remitter Last Name&nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 40%; height: 24px">First Name | Short Name:&nbsp;</td>
                                                            <td align="left" style="width: 30%; height: 24px" colspan="3">
                                                                <label id="lblRemFName" class="cell_desc">Remitter First Name&nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 40%; height: 24px" >Middle Name | Business Type:&nbsp;</td>
                                                            <td align="left" style="width: 25%; height: 24px;" colspan="3">
                                                                <label id="lblRemMName" class="cell_desc">Remitter Middle Name&nbsp;</label></td> 
                                                        </tr>
                                                        <tr>
                                                            <td class="cell_caption" style="width: 40%; height: 24px">Address: &nbsp;</td>
                                                             <td align="left" valign="top" style="width: 25%; height: 24px" colspan="3" rowspan="2">
                                                                 <label id="lblRemAddress" class="cell_desc">17 A Mabait St. Teacher's Village Diliman Quezon City&nbsp;</label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 40%; height: 24px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 40%; height: 24px">&nbsp;</td>
                                                            <td align="left" style="width: 25%; height: 24px" colspan="3"></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 40%; height: 24px">&nbsp;</td>
                                                            <td align="left" style="width: 25%; height: 24px" colspan="3"></td>
                                                        </tr>
                                                        
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <div id = "REMTRANSEARCHGRIDMAIN">
                                                     <table id="tblGridRemTranSearch" width="100%" cellpadding="0" cellspacing="5" style="border: solid 1px #336699; background:azure;">
                                                        <tr>
                                                            <td class="table_caption" style="width: 500px; text-align: left; text-indent: 5px; color: Black; height: 26px;" colspan="5">List of Transactions</td>                                                                                        
                                                        </tr>
                                                        <tr>
                                                            <td><div id = "REMTRANSEARCHGRID">&nbsp;</div></td>
                                                        </tr>
                                                      </table><br />
                                                 </div> 
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
    </form>    
    </div>   
</body>
</html>
