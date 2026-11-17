<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Branch_Definition.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Branch_Definition" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch Definition</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js" type="text/javascript"></script>
    <script src="../js/jscommonmethod.js" type="text/javascript"></script>
    <script src="../js/jshttprequest.js" type="text/javascript"></script>
     <script src="../js/jsgridselector.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_branchsetup_branch_definition.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    var branch_definition = new branch_definition();
    
    
   
    </script>
    

</head>
<body onload="javascript:new common_method().to_read_only('tblBranchDesc',true);branch_definition.load_grid_viewer();branch_definition.load_all_dropdownlist();">
<br />
    <form id="form1" runat="server">
    <div>
    <center>
        <div id="BranchDefinition" style="display: block">
            <table cellpadding="0" cellspacing="0" width="30%">
                <tr>
                <td> 
                    <table cellpadding="0" cellspacing="0" >
                        <tr>
                            <td align="left" valign="bottom" style="width:0%; height:24px; border-collapse:collapse"> 
                                <img src="../images/toe.GIF" /></td>    
                            <td class="table_head_dark" style="width: 50%; height: 24px">
                                Branch Setup</td>
                            <td align="left" style="width: 100%; height: 24px" valign="bottom">
                                <img align="absBottom" src="../images/rend.gif" /></td>   
                      </tr>
                    </table>    
                </td>
                    
                </tr>
                <tr>
                    <td colspan="2" style="width: 625px">
                        <table style="border-right: #336699 3px solid; border-top: #336699 3px solid; border-left: #336699 3px solid;
                            width: 80%; border-bottom: #336699 3px solid">
                            <tr>
                                <td style="width: 100%; height: 21px">
                                    
                                    <div style="text-align: center">
                                        <table id="tblOccupation" cellpadding="0" cellspacing="5" style="border-right: #336699 1px solid;
                                            border-top: #336699 1px solid; border-left: #336699 1px solid; border-bottom: #336699 1px solid"
                                            width="600">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 125px; color: black; text-indent: 5px;
                                                    height: 26px; text-align: left">
                                                    Branch Definition</td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                                <td style="height: 190px; width: 800px;">
                                                    <div style="text-align: center">
                                                        <table id="tblBranchDesc">
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px">
                                                                    Branch Type:&nbsp;</td>
                                                                
                                                                <td style="width: 100px; height: 26px;">
                                                                    <select id="ddBranchType" style="width: 173px; height: 100%">
    
                                                                    </select>
                                                                </td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px">
                                                                    Branch Code:&nbsp;</td>
                                                                <td style="width: 80px; height: 26px;">
                                                                    <input type="text" id="txtBranchCode" style="width: 150%"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px">
                                                                    Branch Name:&nbsp;</td>
                                                                <td style="width: 100px; height: 26px;">
                                                                   
                                                                    <input type="text" id="txtBranchName" style="width: 168%"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px">
                                                                    Negotiating Code:&nbsp;
                                                                </td>
                                                                <td style="width: 100px; height: 26px; text-align: left;">
                                                                    <input type="text" id="txtNegBranchCode" style="width: 169%"/></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px">
                                                                    Covering Code:&nbsp;
                                                                </td>
                                                                <td style="width: 80px; height: 26px; text-align: left;">
                                                                    <input type="text" id="txtCoveringBankCode" style="width: 150%"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px">
                                                                    Flexcube Code:&nbsp;
                                                                </td>
                                                                <td style="width: 100px; text-align: left;">
                                                                    <input type="text" id="txtFCBranchCode" style="width: 170%"/></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px">
                                                                    Sub Branch:&nbsp;
                                                                </td>
                                                                <td style="width: 100px; text-align: left;">
                                                                    <input type="checkbox" id="chkClassification" enabletheming="True" style="width: 22%"/></td>
                                                                
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 26px">
                                                                    Contact Person:&nbsp;
                                                                </td>
                                                                <td style="width: 100px; height: 26px;">
                                                                    <input type="text" id="txtContactPerson" style="width: 170%" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px">
                                                                    Address:&nbsp;
                                                                </td>
                                                                <td style="width: 100px; text-align: left;">
                                                                    <input type="text" id="txtAddress" style="width: 170%"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px">
                                                                    State:&nbsp;</td>
                                                                <td style="width: 100px; text-align: left;">
                                                                    <input type="text" id="txtState" style="width: 170%"/></td>
                                                               <td class="cell_caption" style="width: 136px; height: 26px">
                                                                   Country:&nbsp;
                                                                </td>
                                                                <td style="width: 112px"><select id="ddCountry" style="width: 173px; height: 100%">
                                                                    
                                                                </select>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px">
                                                                    Phone:</td>
                                                                <td style="width: 100px; text-align: left;">
                                                                    <input type="text" id="txtPhone" style="width: 170%"/></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px">
                                                                    Fax:&nbsp;
                                                                </td>
                                                                <td style="width: 112px">
                                                                    <input type="text" id="txtFax" style="width: 150%"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="cell_caption" style="width: 137px; height: 23px">
                                                                    Email:</td>
                                                                <td style="width: 100px; text-align: left;">
                                                                    <input type="text" id="txtEmail" style="width: 170%"/></td>
                                                                <td class="cell_caption" style="width: 136px; height: 26px">
                                                                    Time Zone:&nbsp;
                                                                </td>
                                                                <td style="width: 112px">
                                                                    <input type="text" id="txtTimeZone" style="width: 150%"/></td>
                                                                     
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                        </table>
                                                     
                                                        <table id="tblbuttons" cellpadding="0" cellspacing="5" width="60%">
                                                            <tr>
                                                                <td style="width: 10%">
                                                                    <input id="btnNew" class="enable_button"  onclick = "javascript:branch_definition.insert_record()" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="New" /></td>
                                                                <td style="width: 10%">
                                                                    <input id="btnEdit" class="enable_button" onclick = "javascript:branch_definition.update_record();" disabled="disabled" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Edit" /></td>
                                                                <td style="width: 10%">
                                                                    <input id="btnSave" class="enable_button"  onclick = "javascript:branch_definition.commit_record();"onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Save" /></td>
                                                                <td style="width: 10%">
                                                                    <input id="btnCancel" class="enable_button"  onclick = "javascript:branch_definition.rollback_record();" onmouseout="this.className='enable_button';"
                                                                        onmouseover="this.className='mouseover_button';" type="button" value="Cancel" /></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <input id="txtHiddenSeqID" type="hidden" /></td>
                                            </tr>
                                        </table>
                                        
                                        
                                    </div>
                                    <div id="BranchDefGrid" style="text-align: center">BRANCH DEFINITION LIST</div>
                                  </td> 
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div></center>
    
    </div>
    </form>

</body>
</html>
