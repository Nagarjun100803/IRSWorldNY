<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Branch_Definition.aspx.cs" Inherits="IRSWorld.BranchSetup.Branch_Definition" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch Definition</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="<%=ResolveUrl("~/") %>js/BranchSetup/BS/jquery-3.5.1.min.js"></script>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/js_generic.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
    <script src="../js/jsmodalwindow.js" type="text/javascript"></script>
    <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <%--<script src="../js/BranchSetup/js_branchsetup_branch_definition_min.js" type="text/javascript"></script>--%>
    <script src="../js/BranchSetup/js_branchsetup_branch_definition_min.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    var branch_definition = new branch_definition();
    
        
        function fixht()
        {
            document.getElementById('BranchDefinition').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
            document.getElementById('BranchDefinition').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
        }
        
        function fixmain()
        {
            var brmain = false;
            try
            {
                 brmain = parent.is_branchsetup_main();
            }
            catch(e){}
            
            if (!brmain)
            {
                document.getElementById('theBody').onresize = fixht;
                document.getElementById('BranchDefinition').style.overflow = 'auto';
                
                fixht();
            }        
        }
        
        function page_setup()
        {
            if (top == self)
            {
                window.open('./BranchSetupMain.aspx?load=Branch_Defenition.aspx', 'top');
            }
            else if (top.isAuthenticated())
            {
                fixmain();
            }
            else
            {
                spawn_errpage("This module requires login!");
            }
        }
    </script>
</head>
<body id="theBody" onload="page_setup();javascript:new common_method().to_read_only('tblBranchDesc',true);branch_definition.load_all_dropdownlist();branch_definition.hide_panel();branch_definition.loadAsterisk();">
    <form id="form1" runat="server">
    <div id="BranchDefinition" style="width:100%;height:100%;overflow:auto;padding-top: 30px; display:block; vertical-align:middle">
    <center>
            <table cellpadding="0" cellspacing="0">
                <tr>
                <td style="width:60%; vertical-align:bottom;" align="left"> 
                    <table style="width:100%" cellpadding="0" cellspacing="0" >
                        <tr>
                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                <img src="../images/toe.GIF" /></td>    
                            <td class="table_head_dark" style="width: 40%; height: 24px">
                                Branch Setup</td>
                            <td align="left" style="width: 60%; height: 24px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>   
                      </tr>
                    </table>    
                </td>
                    
                </tr>
                <tr>
                    <td style="width:100%; text-align:left; margin: 0;">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 80%; border-bottom: #336699 3px solid">
                            <tr>
                                <td style="width: 100%; height: 21px" align=center>
                                        <table id="tblOccupation" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid;
                                            width:800">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 100%; color: black; text-indent: 5px;
                                                    height: 26px; text-align: left">
                                                    Branch Definition</td>
                                            </tr>
                                            <tr>
                                                <td style="height: 190px; width: 800px; background-color: azure;">
                                                    <div id="BranchDefEntry" style="text-align: center; background-color: azure;">
                                                        <table id="tblBranchDesc" style="width: 783px">
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px; text-align: right;">
                                                                    Branch Type:&nbsp;</td>
                                                                <td style="width: 230px; height: 26px;" align=left>
                                                                    <select id="ddBranchType" style="width: 85%;" onchange="javascript:branch_definition.createAsterisk();">
                                                                    </select>
                                                                    <label id="astBranchType" style="color:#ff0000;width:10%;">*</label></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px; text-align: right;">
                                                                    Branch Code:&nbsp;</td>
                                                                <td style="width: 230px; height: 26px;" align=left>
                                                                    <input type="text" id="txtBranchCode" style="width: 50%" maxlength="4"/>
                                                                    <label id="astBranchCode" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px; text-align: right;">
                                                                    Branch Name:</td>
                                                                <td style="width: 230px; height: 26px" align=left>
                                                                    <input type="text" id="txtBranchName" style="width: 85%"/>
                                                                    <label id="astBranchName" style="color:#ff0000;width:10%;">*</label></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px; text-align: right;">
                                                                    Charge Group:</td>
                                                                <td style="width: 230px; height: 26px" align=left>
                                                                    <select id="ddChargeGroup" style="width:85%;">
                                                                    </select>
                                                                    <label id="astChargeGroup" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px; text-align: right;">
                                                                   <label id= "lblNegotiatingCode">Negotiating Code:&nbsp;</label> 
                                                                </td>
                                                                <td style="width: 230px; height: 26px; text-align: left;">
                                                                    <input type="text" id="txtNegBranchCode" style="width: 50%" maxlength="4"/>
                                                                    <label id="astNegBranchCode" style="color:#ff0000;width:10%;">*</label></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px; text-align: right;">
                                                                    <label id="lblCoveringCode">Covering Code:&nbsp;</label>
                                                                </td>
                                                                <td style="width: 230px; height: 26px; text-align: left;">
                                                                    <select id="ddCovCode" style="width: 27%;"></select>
                                                                    <label id="astCovCode" style="color:#ff0000;width:10%;">*</label>
                                                                    <select id="ddCovBranch" style="width: 27%;">
                                                                    </select></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                   <label id="lblFlexcubeCode"> SI Code:&nbsp;</label>
                                                                </td>
                                                                <td style="width: 230px; text-align: left;">
                                                                    <input type="text" id="txtFCBranchCode" style="width: 50%"/>
                                                                    <label id="astSIBranchCode" style="color:#ff0000;width:10%;">*</label></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px; text-align: right;">
                                                                    Sub Branch:&nbsp;
                                                                </td>
                                                                <td style="width: 230px; text-align: left;">
                                                                    <input type="checkbox" id="chkClassification" enabletheming="True" style="width: 10%"/></td>
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                    BRSTN:&nbsp</td>
                                                                <td style="width: 230px; text-align: left">
                                                                    <input type="text" id="txtBrstn" style="width: 60%" maxlength="20"/>
                                                                    <label id="astBRSTN" style="color:#ff0000;width:10%;">*</label></td>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                Courier Code:</td>
                                                                <td style="width: 230px; text-align: left">
                                                                    <input type="text" id="txtCourierCode" style="width: 60%" maxlength="20"/>
                                                                    <label id="astCourierCode" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right">
                                                                    PCHC Member:</td>
                                                                <td style="width: 230px; text-align: left">
                                                                    <select id="ddPCHC" style="width: 85%;"></select>
                                                                    <label id="astPCHC" style="color:#ff0000;width:10%;">*</label></td>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right">
                                                                    Domestic Location:&nbsp</td>
                                                                <td style="width: 230px; text-align: left">
                                                                    <select id="ddDom_loc" style="width: 85%;" disabled="disabled"></select>
                                                                    <label id="astDomesticLocation" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px">
                                                                    Contact Person:&nbsp;
                                                                </td>
                                                                <td style="width: 596px; height: 26px;" colspan="4" align=left>
                                                                    <input type="text" id="txtContactPerson" style="width: 80%" />
                                                                    <label id="astContactPerson" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                    Address:&nbsp;
                                                                </td>
                                                                <td style="width: 596px; text-align: left;" colspan="4" align=left>
                                                                    <input type="text" id="txtAddress" style="width: 80%"/>
                                                                    <label id="astAddress" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                    Zip Code:&nbsp;
                                                                </td>
                                                                <td style="width: 230px; text-align: left;" colspan="4" align=left>
                                                                    <input type="text" id="txtZipCode" style="width: 85%"/>
                                                                    <label id="astZipCode" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                    State:&nbsp;</td>
                                                                <td style="width: 230px; text-align: left;">
                                                                    <input type="text" id="txtState" style="width: 85%"/>
                                                                    <label id="astState" style="color:#ff0000;width:10%;">*</label></td>
                                                               <td class="cell_caption" style="width: 136px; height: 26px; text-align: right;">
                                                                   Country:&nbsp;
                                                                </td>
                                                                <td style="width: 230px; text-align: left;">
                                                                    <select id="ddCountry" style="width: 85%;"></select>
                                                                    <label id="astCountry" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 1px">
                                                                    Phone:<br />
                                                                    +XX(XXX)XXXXXXX</td>
                                                                <td style="width: 230px; text-align: left; height: 1px;">
                                                                    <table style="width: 100%; text-align: center;" id="tblphone">
                                                                        <tr>
                                                                            <td style="width: 26px; height: 25px">
                                                                                <input id="txtPhone1" style="width: 95%" type="text" maxlength="3" />
                                                                            </td>
                                                                            <td style="width: 38px; height: 25px">
                                                                                <input id="txtPhone2" style="width: 95%" type="text" maxlength="5" />
                                                                            </td>
                                                                            <td style="height: 25px">
                                                                                <input id="txtPhone3" style="width: 85%; text-align:left" type="text" maxlength="20" />
                                                                                <label id="astPhone" style="color:#ff0000;width:10%;">*</label></td>
                                                                        </tr>
                                                                    </table>
                                                              </td>
                                                                <td class="cell_caption" style="width: 136px; height: 1px; text-align: right;">
                                                                    Fax:&nbsp;
                                                                </td>
                                                                <td style="width: 230px; text-align: left;">
                                                                    <input type="text" id="txtFax" style="width: 85%" maxlength="20"/>
                                                                    <label id="astFax" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px">
                                                                    Email:</td>
                                                                <td style="width: 230px; text-align: left;">
                                                                    <input type="text" id="txtEmail" style="width: 85%"/>
                                                                    <label id="astEmail" style="color:#ff0000;width:10%;">*</label></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px; text-align: right;">
                                                                    Time Zone:&nbsp;
                                                                </td>
                                                                <td style="width: 230px; text-align: left;">
                                                                    <select id="ddTimeZone" style="width: 85%;"></select>
                                                                    <label id="astTimeZone" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            <tr> <!-- RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile -->
                                                                <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                   BIC:&nbsp;
                                                                </td>
                                                                <td style="width: 230px; text-align: left;" colspan="4" align=left>
                                                                    <input type="text" id="txtBankInformationCode" style="width: 85%"/>
                                                                    <label id="astBankInformationCode" style="color:#ff0000;width:10%;">*</label></td>
                                                            </tr>
                                                            </table>
                                                    </div>
                                                    <input id="txtHiddenSeqID" type="hidden" />
                                                    <div style="width:100%;background-color: azure;" align=center>
                                                    <div id="BranchDefBtn" style="width:437px; background-color: azure;border-right: #336699 2px solid; border-top: #336699 2px solid; border-left: #336699 2px solid; border-bottom: #336699 2px solid" align=center>
                                                     <table id="tblbuttons" cellpadding="0" cellspacing="5">
                                                         <tr>
                                                            <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;">
                                                                Branch Code:</td>
                                                             <td style="width: 280px" align=left>
                                                                 <input id="txtSearchBCode" type="text" style="width:100%" /></td>
                                                         </tr>
                                                         <tr>
                                                             <td class="cell_caption" style="width: 137px; height: 23px; text-align: right;" >
                                                                 Branch Name:</td>
                                                             <td style="width: 280px" >
                                                                 <input id="txtSearchBName" type="text" style="width:100%"  /></td>
                                                         </tr>
                                                            <tr>
                                                                <td style="width:100%;" align="center" colspan=2> 
                                                                    <table style="width:100%;">
                                                                        <tr>
                                                                            <td style="width:33%">&nbsp;</td>
                                                                            <td style="width: 33%" align=left><input id="btnNew" class="button" onclick="javascript:branch_definition.insert_record()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="New"/></td>
                                                                            <td style="width: 33%" align=left><input id="btnSearch" class="button" onclick="javascript:branch_definition.panel_search_branch()" onmouseout="this.className='button';" onmouseover="this.className='button_over';" type="button" value="Search"/></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                     </div>   
                                                     </div>
                                                     <div id="BranchDefBtn2" style="width:100%;background-color: azure" align=center>
                                                     <table id="tblbuttons2" cellpadding="0" cellspacing="5" style="width:60%">
                                                            <tr>
                                                                <td style="width: 10%">
                                                                    <input id="btnEdit" class="button" onclick = "javascript:branch_definition.update_record();" disabled="disabled" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="Edit" /></td>
                                                                <td style="width: 10%">
                                                                    <input id="btnSave" class="button"  onclick = "javascript:branch_definition.commit_record();"onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="Save" /></td>
                                                                <td style="width: 10%"> <!-- RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile -->
                                                                    <input id="btnDelete" class="button"  onclick = "javascript:branch_definition.delete_record();" disabled="disabled" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="Delete" /></td>
                                                                <td style="width: 10%">
                                                                    <input id="btnCancel2" class="button"  onclick = "javascript:branch_definition.rollback_record();javascript:branch_definition.show_grid();" onmouseout="this.className='button';"
                                                                        onmouseover="this.className='button_over';" type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                     </div>   
                                                </td>
                                            </tr>
                                        </table>                                   
                                  </td> 
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </center>
    
    </div>
    </form>

</body>
</html>
