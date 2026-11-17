<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranSearch2.aspx.cs" Inherits="IRSWorld.Remittance.RemTranSearch2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Remittance - Search</title>
    <link href="../css/Default.css" rel="stylesheet"   type = "text/css" />
    <script src="../js/jsCommons.js"                   type = "text/javascript"></script>
    <script src="../js/jscommonmethod.js"              type = "text/javascript"></script>
    <script src="../js/jshttprequest.js"               type = "text/javascript"></script>    
    <script src="../js/jsmodalwindow.js"               type = "text/javascript"></script>
    <script src="../js/jsgridselector.js"              type = "text/javascript"></script>
    <script src="../js/Remittance/jsremittersearch.js" type = "text/javascript"></script>            
</head>
<body>
    <form id="form1" runat="server">
                    
    <center>           
        <div id="div_search" style="width:40%; display: block;">
            <table width="100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td id="capSearch" align="left" class="table_head_dark" style="width:25%; height: 24px; border-collapse:collapse">Search</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table width="100%" cellpadding="0" cellspacing="0" class="table_body" style="border: solid 1px skyblue">
                                        <tr>
                                            <td class="table_caption" style="width: 50%"; >Search by (Remitter) :</td>    
                                            <td class="table_caption" style="width: 50%;" align="left"><select id="ddlSearchClass" size="1" style="width: 90%; text-align: left;"></select></td>                                                        
                                        </tr>
                                        <tr>
                                            <td style="width:100%;height: 100px" align="center" valign="middle" colspan = "2">
                                                <table width="96%" cellspacing="5" cellpadding="0">
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 30%";>&nbsp;Lastname :</td>
                                                        <td align="left" style="width: 70%";><input id="txtSearchLastname" type="text" style="width: 100%;text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 30%";>&nbsp;Firstname :</td>
                                                        <td align="left" style="width: 70%";><input id="txtSearchFirstname" type="text" style="width: 100%;text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 30%";>&nbsp;Middlename :</td>
                                                        <td align="left" style="width: 70%";><input id="txtSearchMiddlename" type="text" style="width: 100%;text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                               </table>  
                                           </td> 
                                       </tr> 
                                       <tr>
                                            <td style="height: 28px" valign="top" colspan = "2">
                                                <input type="button" value="Search" onclick="search_RemitCIF();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /> &nbsp;&nbsp;&nbsp;
                                                <input type="button" value="New CIF" onclick="search_RemitNewCIF();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
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
        </div>
    </center>            
 
    <center>           
        <div id="div_new_cif" style="width:40%; display: block;">
            <table width="100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td id="Td1" align="left" class="table_head_dark" style="width:25%; height: 24px; border-collapse:collapse">New CIF</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table width="100%" cellpadding="0" cellspacing="0" class="table_body" style="border: solid 1px skyblue">
                                        <tr>
                                            <td class="table_caption" style="width: 100%"; >Please provide the following :</td>    
                                        </tr>
                                        <tr>
                                            <td style="width:100%;height: 100px" align="center" valign="middle" colspan = "2">
                                                <table width="96%" cellspacing="5" cellpadding="0">
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 30%";>&nbsp;CIF Class :</td>
                                                        <td align="left" style="width: 70%";><select id="ddlNewCIFClass" size="1" style="width: 90%; text-align: left;"></select></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 30%";>&nbsp;CIF Type :</td>
                                                        <td align="left" style="width: 70%";><select id="ddlNewCIFType" size="1" style="width: 90%; text-align: left;"></select></td> 
                                                    </tr>
                                               </table>  
                                           </td> 
                                       </tr> 
                                       <tr>
                                            <td style="height: 28px" valign="top" colspan = "2">
                                                <input type="button" value="Create" onclick="new_CIF();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /> &nbsp;&nbsp;&nbsp;
                                                <input type="button" value="Cancel" onclick="new_CancelCIF();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
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
        </div>
    </center>            

    <center>           
        <div id="div_improd_change" style="width:40%; display: block;">
            <table width="100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td id="Td2" align="left" class="table_head_dark" style="width:70%; height: 24px; border-collapse:collapse">Change Product / Implementation</td>
                    <td align="left" valign="bottom" style="height: 24px; width: 300px;" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100%; height: 49px;">
                                    <!-- content starts here -->
                                    <table width="100%" cellpadding="0" cellspacing="0" class="table_body" style="border: solid 1px skyblue">
                                        <tr>
                                            <td class="table_caption" style="width: 100%"; >Please provide the following :</td>    
                                        </tr>
                                        <tr>
                                            <td style="width:100%;height: 100px" align="center" valign="middle" colspan = "2">
                                                <table width="96%" cellspacing="5" cellpadding="0">
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 40%";>&nbsp;Product Type :</td>
                                                        <td align="left" style="width: 60%";><select id="ddlChangeProdcutType" size="1" style="width: 95%; text-align: left;"></select></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_desc" style="width: 40%";>&nbsp;Implementation Type :</td>
                                                        <td align="left" style="width: 60%";><select id="ddlImplemType" size="1" style="width: 95%; text-align: left;"></select></td> 
                                                    </tr>
                                               </table>  
                                           </td> 
                                       </tr> 
                                       <tr>
                                            <td style="height: 28px" valign="top" colspan = "2">
                                                <input type="button" value="Commit" onclick="change_Commit();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /> &nbsp;&nbsp;&nbsp;
                                                <input type="button" value="Cancel" onclick="change_Cancel();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
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
        </div>
    </center>            

    
     <center>   
        <div id="div_results" style="width: 100%; height: 100%; display: block">
            <table width="100%">
                <tr>
                    <td style="width: 84%; padding-top: 30px">
                        <table width="96%" height="100%" cellspacing="0" cellpadding="0" style="background-color:Azure">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px;"><img src="../images/toe.GIF" /></td>
                                <td id="Td16" align="left" class="table_head_dark" style="width:20%; height: 24px; border-collapse:collapse;">Remitter's Transaction</td>
                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"> 
                                    <img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; height: 100%">
                                    <table style="width:100%; height: 100%;" class="table_border">
                                        <tr>
                                            <td align="center" style="width: 85%"><label id="lblRemitterName" class="cell_desc" style="font-size: 20px; color: blue;" >&nbsp;Alvin C. Santos</label></td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="width: 85%"><label id="lblRemitterAddress" class="cell_desc" style="font-size: medium; font-weight: normal;" >&nbsp;Via Fauche 37, Milan, Italy</label></td>
                                        </tr>
                                       <tr>
                                            <td style="width: 100%; height: 8px"></td>
                                       </tr> 
                                        
                                        <tr>
                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                <!-- content starts here -->
                                                 <div style="width:98%; height: 100%; margin: 0" class="table_body" id="div2" >

                                                    <table id = "tblRemBeneficiary" width="100%" cellpadding="0" cellspacing="5px">
                                                        <tr>
                                                            <td style="width: 100%;"valign="top">
                                                                list of bene</td>
                                                        </tr>
                                                    </table>
                                                   
                                                    <table id = "tblRemBeneTransaction" width="100%" cellpadding="0" cellspacing="5px">
                                                        <tr>
                                                            <td style="width:100%;" valign="top">
                                                                remitters txn list to particular bene</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table width="100%" cellpadding="0" cellspacing="0" id="TABLE1" >
                                                                    <tr>
                                                                        <td style="height: 28px; width: 100%" valign="top" align = "center">
                                                                            <input type="button"  style="width: 15% " value="New Transaction" onclick="btnNewRemitTrans();" class="button" />&nbsp;&nbsp;
                                                                            <input type="button"  style="width: 15% " value="Change Product" onclick="btnNewChangeProductImplem();" class="button" />&nbsp;&nbsp;                                                               
                                                                            <input type="button"  style="width: 15% " value="New Beneficiary" onclick="btnNewBeneficairy();" class="button" />&nbsp;&nbsp;                                                                                    
                                                                            <input type="button"  style="width: 15% " value="Search" onclick="btnNewRemitSearch();" class="button" />&nbsp;&nbsp;
                                                                            <input type="button"  style="width: 15% " value="Close" onclick="btnNewRemitClose();" class="button" />
                                                                        </td>                                                                                    
                                                                    </tr>
                                                                </table> 
                                                            </td>
                                                                        
                                                        </tr>
                                                    </table>
                                                   
                                                </div>  
                                                <!-- content ends here -->
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100%; height: 8px">
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
       
    </form>
</body>
</html>
