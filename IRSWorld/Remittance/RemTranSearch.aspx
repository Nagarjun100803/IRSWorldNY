<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RemTranSearch.aspx.cs" Inherits="IRSWorld.Remittance.RemTranSearch" %>

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
    
    <script type="text/javascript">
        var rem_search = new remittance_search();
    </script>
    
</head>

<body onload="javascript:rem_search.open_search_box();">
    <form id="form1" runat="server">
                    
    <center>
        <br />
        <div id="div_search" style="width:50%; display: block;">
            <table style="width:100%" cellspacing="0" cellpadding="0">
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
                                    <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
                                        <tr>
                                            <td class="table_caption" colspan="2"; >
                                                PLEASE PROVIDE THE FOLLOWING ENTRIES</td>    
                                        </tr>
                                        <tr>
                                            <td style="width:100%;height:auto" align="center" valign="middle" colspan = "2">
                                                <br />
                                                <table style="width:96%" cellspacing="1" cellpadding="0">
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 30%; height:24px">Search By :</td>                                                            
                                                        <td align="left" style="width: 70%">
                                                            <select id="ddlSearchClass" size="0" style="width: 90%; text-align: left;"></select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 30%;height:24px">&nbsp;Lastname :</td>
                                                        <td align="left" style="width: 70%";><input id="txtSearchLastname" type="text" style="text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 30%;height:24px";>&nbsp;Firstname :</td>
                                                        <td align="left" style="width: 70%";><input id="txtSearchFirstname" type="text" style="text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                                    <tr>
                                                        <td align="right" class="cell_caption" style="width: 30%; height:24px">&nbsp;Middlename :</td>
                                                        <td align="left" style="width: 70%";><input id="txtSearchMiddlename" type="text" style="text-align: left; font-size: medium;" /></td> 
                                                    </tr>
                                               </table>  
                                           </td> 
                                       </tr> 
                                       <tr>
                                            <td style="height: 28px" valign="top" colspan = "2">
                                                <input type="button" value="Search"  onclick = "javascript:rem_search.search_customers_info();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" id="btnsearch" /></td>
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
        <div id="div_new_cif" style="width:50%; display: block;">
            <table style="width:100%" cellspacing="0" cellpadding="0">
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
                                    <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
                                        <tr>
                                            <td class="table_caption" style="width: 100%"; >Please provide the following :</td>    
                                        </tr>
                                        <tr>
                                            <td style="width:100%;height: 100px" align="center" valign="middle" colspan = "2">
                                                <table style="width:96%" cellspacing="5" cellpadding="0">
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
                                                <input type="button" value="Create" onclick="javascript:rem_search.test_me_3();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /> &nbsp;&nbsp;&nbsp;
                                                <input type="button" value="Cancel" onclick="javascript:rem_search.open_division('div_search');" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" />
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
        <div id="div_improd_change" style="width:90%; display: block;">
            <table style="width:100%" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"><img src="../images/toe.GIF" /></td>
                    <td id="Td2" align="left" class="table_head_dark" style="width:39%; height: 24px; border-collapse:collapse">BRANCH DEFAULT PRODUCTS</td>                        
                    <td align="left" valign="bottom" style="height: 24px" class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 100%; background: azure;">
                        <table style="width:100%;" class="table_border">
                            <tr>
                                <td style="width: 100%; height: 21px;">&nbsp;&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <!-- content starts here -->
                                    <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
                                        <tr><td class="table_caption" style="width: 100%"; >FILTER WINDOW</td></tr>
                                        <tr>
                                            <td style="height: auto; width:auto;" valign="top" colspan = "2">
                                                <table style="width: 100%">
                                                    <tr><td style="width: 8%; text-align: right" class =  "cell_caption">Product&nbsp; :</td><td style="text-align: left; width: 40%;"><select id="cmbproducttype" size="1" style="text-align: left; width: 45%;" onchange="javascript:rem_search.search_product_charges_param();"></select></td></tr>
                                                    <tr><td style="width: 8%; text-align: right; height: 23px;" class =  "cell_caption">Implementation Type :</td><td style="text-align: left; width: 40%;"><select id="cmbimplemtype" size="1" style="text-align: left; width: 45%;" onchange="javascript:rem_search.search_product_charges_param();"></select></td></tr>
                                                    <tr><td style="width: 8%; text-align: right" class =  "cell_caption">Pay Mode :</td><td style="text-align: left; width: 40%;"><select id="cmbpaymode" size="1" style="text-align: left; width: 45%;" onchange="javascript:rem_search.search_product_charges_param();"></select></td></tr>
                                                    <tr><td style="width: 8%; text-align: right" class =  "cell_caption">Charge Type:</td><td style="text-align: left; width: 40%;"><select id="cmbchargetype" size="1" style="text-align: left; width: 45%;" onchange="javascript:rem_search.search_product_charges_param();"></select></td></tr>                                                                                                                        
                                                    <tr>
                                                        <td style="width: 8%; text-align: right" class =  "cell_caption">Find :</td>                                                            
                                                        <td style="width: 40%; text-align: left"><input id="txt_search"  onkeyup = "javascript:execute_ajaxgridview_command('ajx_search$')";type="text" style="width: 45%;text-align: left; font-size: medium;" /></td>                                                                    
                                                    </tr>
                                                </table>
                                             </td>
                                        </tr>                                    
                                        <tr>
                                            <td style="width:auto; height: 19px;" align="center" valign="middle" colspan = "2">
                                               <div id = "DIV_PROD_IMPLEM" onmousemove = "javascript:rem_search.active_grid = 'DIV_PROD_IMPLEM';">
                                                   GRIDVIEW</div>
                                            </td> 
                                       </tr> 
                                       <tr><td style="height: 28px; width: 852px;" valign="top" colspan = "2"></td></tr>
                                       <tr><td colspan="2" style="height: 28px; width: 852px;" valign="top"><input type="button" value="Cancel" onclick="javascript:rem_search.open_product_charges();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" /></td></tr>
                                    </table>                                    
                               </td>
                               <!-- content ends here -->
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </div>
    </center>            
        <center>
           <div id = "div_results">
                        <table cellspacing="0" cellpadding="0" style="background-color:Azure; height:auto; width: 90%">
                            <tr>
                                <td align="left" valign="bottom" style="width:1%;height:24px;"><img src="../images/toe.GIF" /></td>
                                <td align="left" class="table_head_dark" style="width:36%; height: 24px; border-collapse:collapse;">REMITTER &amp; BENEFICIARY INFORMATION</td>
                                <td align="left" valign="bottom" style="height: 24px"  class="bg_body"><img align="absbottom" src="../images/rend.gif" /></td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table style="width:100%; height: 100%;" class="table_border">                                        
                                        <tr>
                                            <td style="width: 100%; height: 100%; vertical-align: top">
                                                <!-- content starts here -->
                                                 <br/>
                                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 18%; text-align: right; height: 24px" class = "cell_caption">Remitter Name :</td>                                                                
                                                            <td style="text-align: left; width: 70%; height: 24px" class="cell_desc"><label id="lblRemitterName"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 18%; text-align: right; height: 24px" class = "cell_caption">Remitter Address :</td>                                                                
                                                            <td style="text-align: left; width: 70%; height: 24px" class="cell_desc"><label id="lblRemitterAddress" ></label></td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    
                                                     <table style="width:100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 18%; text-align: right; height: 24px" class = "cell_caption">Beneficiary Name :</td>                                                                
                                                            <td style="text-align: left; width: 70%; height: 24px" class="cell_desc"><label id="lblBeneName"></label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 18%; text-align: right; height: 24px" class = "cell_caption">Beneficiary Address :</td>                                                                
                                                            <td style="text-align: left; width: 70%; height: 24px" class="cell_desc"><label id="lblBenefAddress" ></label></td>
                                                        </tr>
                                                    </table>
                                                    

                                                    <br/>
                                                    <table cellpadding="0" cellspacing="0" class="table_body" style="width:100%; border: solid 1px skyblue">
                                                        <tr><td class="table_caption" style="width: 100%"; >BENEFICIARY DEFAULT PRODUCT (CIF SETUP)</td></tr>                                                            
                                                        <tr><td align="center" valign="middle" colspan = "2"><div id = "DIV_BENE_PROD_IMP" onmousemove = "javascript:rem_search.active_grid = 'DIV_BENE_PROD_IMP';">
                                                            GRIDVIEW</div></td></tr>
                                                    </table>
                                                    <table id = "tblRemBeneTransaction" style="width:100%" cellpadding="0" cellspacing="5px">
                                                        <tr><td style="width:100%;" valign="top"><div id = "DIV_BENE_LIST"></div></td></tr>
                                                        <tr>
                                                            <td>
                                                                <table style="width:100%" cellpadding="0" cellspacing="0" id="TABLE1" >
                                                                    <tr>
                                                                        <td style="height: 28px; width: 100%" valign="top" align = "center">
                                                                            <input type="button"  style="width: 15% " value="Select Product"  onclick = "javascript:rem_search.open_division('div_improd_change');" class="button" />&nbsp;
                                                                            <input type="button"  style="width: 15% " value="Search"          onclick = "javascript:rem_search.open_division('div_search');"        class="button" /></td>                                                                                    
                                                                    </tr>
                                                                </table> 
                                                            </td>
                                                                        
                                                        </tr>
                                                    </table>                                                                                                   
                                                <!-- content ends here -->
                                            </td>
                                        </tr>
                                        <tr><td style="width: 100%; height: 8px"></td></tr>                                        
                                    </table>
                                    </td>
                            </tr>
                        </table>
            </div>
        </center>

    
     <center>   &nbsp;</center>
       
    </form>
</body>
</html>
