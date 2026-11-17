<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Branch_Set_Up.aspx.cs" Inherits="IRSWorld.Branch_Set_Up.Branch_Set_Up" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch Setup</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <script src="../js/jsCommons.js"                        type="text/javascript"></script>
    <script src="../js/jscommonmethod.js"                   type="text/javascript"></script>
    <script src="../js/jshttprequest.js"                    type="text/javascript"></script>    
    <script src="../js/jsgridselector.js"                   type="text/javascript"></script>
    <script src="../js/BranchSetup/js_BranchSetup.js"       type="text/javascript"></script>
    <script type="text/javascript">
    
        var branch_setup = new branch_setup();
      
        function page_setup()
        {
            if (top == self)
            {
                window.open('../Main/IRSMain.aspx?load=../Branch\ Set-Up/Branch_Set_Up.aspx', '_top');
            }
            else
            {
                document.getElementById('tbl_content').style.display = 'block';
            }
        }
    </script>
</head>
<body onload ="page_setup(); javascript:branch_setup.hide_panel();">
    <center>
    <form id="form1" runat="server">
    <table id="tbl_content" style="width:100%;display:none" class="content_height">
    <tr>
       <td style ="vertical-align:top;width:18%">
        <div id = "BranchSetupMenu" style= "margin-top: 30px">
              <table width="100%"  cellpadding="0" cellspacing="5">
                    <tr>
                        <td id="td1" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a id="Branch1" href="#"  onclick="javascript:branch_setup.panel_branch();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                Branch</a>
                        </td>
                    </tr>                                    
                    <tr>
                        <td id="td2" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;">
                           <a id="Fees & Charges1" href="#"  onclick="javascript:branch_setup.panel_feescharges();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                Fees & Charges</a></td>
                    </tr>           
                    <tr>
                        <td id="td3" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a id="Currencies1" href="#" onclick="javascript:branch_setup.panel_currencies();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                Currencies</a>
                        </td>
                    </tr>                                                                                                                                       
                    <tr>
                        <td id="td4" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a id="Transactions1" href="#" onclick="javascript:branch_setup.panel_transactions();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                Transactions</a>
                        </td>
                    </tr>                                                                                                                                       
                    <tr>
                        <td id="td5" style="width: 100%; text-align: right; border-bottom: solid 1px #5697c1;">
                            <a id="Offices1" href="#" onclick="javascript:branch_setup.panel_offices();" style="font-weight: bold; font-size: 12px; font-style: italic; font-family: Arial;">
                                Offices</a>
                        </td>
                    </tr>                                                                                                                                       
                                                                                                      
                </table>
        </div>
        </td>
        <td style="width: 82%; height: 100%; border-collapse:collapse" valign="middle">
        <table cellpadding="0" cellspacing="0" style="width: 58%;">
                <tr>
                    <td style="border-collapse: collapse">
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
                    <td style="width: 100%" valign="top">
                        <table style="width: 100%; border:solid 3px #336699;">
                            <tr>
                                <td style="width: 100%; height: 100%; border-collapse:collapse">
                                   <div id="Branch" style="display: block">
                                    <table id="tblBranchSetup" cellpadding="0" cellspacing="5" width="100%">
                                        <tr>
                                            <td class="table_caption" colspan="5" style="width: 125px; color: ivory; text-indent: 5px;
                                                text-align: left; height: 26px;">
                                                Branch</td>
                                        </tr>
                                        <tr>
                                        <td style="height: 150px; width: 500px;">
                                               <table id="tblBranchSetupEntry">
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Negotiating Branch:&nbsp;</td>
                                                       
                                                           <td align="left" style="width: 305px; height: 24px;" colspan="4">
                                                               <select id="ddNegotiatingBranch">
                                                                   <option selected="selected"></option>
                                                               </select>
                                                           </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Branch Code:&nbsp;</td>
                                                      <td align="left" style="width: 305px; height: 24px;" colspan="4">
                                                          <select id="ddBranchCode">
                                                              <option selected="selected"></option>
                                                          </select>
                                                      </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           &nbsp;Sub Branch Code: &nbsp;
                                                       </td>
                                                       <td align="left" style="width: 305px; height: 24px;" colspan="4">
                                                           <select id="ddSubBranchCode">
                                                               <option selected="selected"></option>
                                                           </select>
                                                       </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Location Code:&nbsp;
                                                       </td>
                                                      <td align="left" style="width: 305px; height: 24px;" colspan="4">
                                                          <select id="ddLocationCode">
                                                              <option selected="selected"></option>
                                                          </select>
                                                      </td>
                                                   </tr>
                                                   <tr>
                                                       <td class="cell_caption" style="width: 143px; height: 23px">
                                                           Time Zone:&nbsp</td>
                                                       <td align="left" colspan="4" style="width: 305px; height: 24px">
                                                           <select id="ddTimeZone">
                                                               <option selected="selected"></option>
                                                           </select>
                                                       </td>
                                                   </tr>
                                            
                                                   </table>

                                                       
                                       </td>
                                           
                                    </tr>
                                        
                                    </table>
                                    </div>
                                    <div id="Fees&Charges" style="display: block">
                                        <table id="tblFees&Charges" cellpadding="0" cellspacing="5"width="100%">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 125px; color: ivory; text-indent: 5px;
                                                    text-align: left; height: 26px;">
                                                    Fees &amp; Charges</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 24px; text-align: left;">
                                                    <strong>
                                                    Float&nbsp;</strong></td>
                                                
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 225px; height: 23px">
                                                    Manual/Regular:&nbsp;</td>
                                                <td align="left" colspan="4" style="width: 475px; height: 24px">
                                                    <input id="txtManualRegular" style="width: 102px" type="text" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 225px; height: 21px">
                                                    Special:&nbsp</td>
                                                
                                                <td align="left" style="width: 349px; height: 21px;">
                                                    <input id="txtSpecial" style="width: 102px" type="text" /></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 23px; text-align: left;">
                                                    <strong>Charges</strong></td>
                                                
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 23px; text-align: center">
                                                     <div style="text-align: center">
                                                        <table style="text-align: center">
                                                            <tr>
                                                                <td style="width: 480px; text-align: center;">
                                                    Place Grid View</td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                               
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 225px; height: 23px">
                                                    Product Code: &nbsp;</td>
                                                <td align="left" colspan="4" style="width: 475px; height: 24px";><select id="ddProductCode" style="width: 100%">
                                                    <option selected="selected"></option>
                                                </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 225px; height: 23px">
                                                    Charge Type: &nbsp;</td>
                                                <td align="left" colspan="4" style="width: 475px; height: 24px";><select id="ddChargeType" style="width: 100%">
                                                    <option selected="selected"></option>
                                                </select>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 225px; height: 23px">
                                                    Amount: &nbsp;</td>
                                                <td align="left" style="width: 475px"><input id="txtAmount" style="width: 294px" type="text" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="Currencies" style="width:100%; display: block">
                                        <table id="tblCurrencies" cellpadding="0" cellspacing="5" style="width: 100%">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 125px; color: ivory; text-indent: 5px;
                                                    text-align: left; height: 26px;">
                                                    Currencies</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 24px; text-align: left;">
                                                    <strong>Fx Rates</strong></td>
                                                
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 130px; height: 23px">
                                                    Input/ Manual:&nbsp;</td>
                                                <td align="left" colspan="4" style="width: 475px; height: 24px">
                                                    <input id="txtInputManual" style="width: 136px" type="text" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 130px; height: 23px">
                                                    Upload: &nbsp;</td>
                                                
                                                <td align="left" style="width: 403px">
                                                    <input id="File2" type="file" style="width: 92%" /></td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="width: 130px; height: 23px">
                                                    Automatic: &nbsp;</td>
                                                <td align="left" style="width: 403px"><input style="WIDTH: 8%" id="rdCybermag" 
                                                    type=radio /><strong>Cybermag</strong></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 23px; text-align: left;">
                                                    <strong>Cash Management</strong></td>
                                                
                                            </tr>
                                            <tr>
                                            <td colspan="2" style="text-align: left">
                                                    
                                                <table align="center"><tbody><tr><td style="WIDTH: 128px" class="cell_caption">
                                                    Setup:&nbsp
                                                </td><td style="WIDTH: 200px">
                                                    <input style="WIDTH: 15%" id="rdEnable" 
                                                    type=radio /><strong>Enable</strong></td><td style="WIDTH: 200px"><input style="WIDTH: 15%" id="rdDisable" 
                                                    type=radio /><strong>Disable</strong></td></tr></tbody>
                                                </table>
                                                <strong>Currencies</strong></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: center">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 137px; height: 40px; text-align: center">
                                                                <strong>Selected Currencies</strong></td>
                                                            <td style="width: 137px; height: 40px">
                                                                <strong>Add Currencies</strong></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 137px">
                                                            <div id="SelectedCurrenciesGrid">
                                                                Grid</div>
                                                            </td>
                                                            <td style="width: 137px">
                                                            <div id="AddCurrenciesGrid">
                                                                Grid</div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                            
                                    </div>
                                    
                                    <div id="Transactions" style="display: block">
                                        <table id="tblTransactions" cellpadding="0" cellspacing="5" width="100%">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 125px; color: ivory; text-indent: 5px;
                                                    text-align: left; height: 26px;">
                                                    Transactions</td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: left">
                                                    <div style="text-align: center">
                                                        <table>
                                                            <tr>
                                                                <td style="width: 100px; text-align: left">
                                                                    <strong>Mode</strong></td>
                                                                <td style="width: 100px; text-align: left">
                                                                    <strong>Receipts</strong></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 100px">
                                                                    <table>
                                                                        <tr>
                                                                            <td class="cell_caption">
                                                                                Sending:&nbsp;</td>
                                                                            <td style="width: 103px">
                                                                                <input id="chkSending" type="checkbox" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption">
                                                                                Payout: &nbsp;
                                                                            </td>
                                                                            <td style="width: 103px">
                                                                                <input id="chkPayout" type="checkbox" /></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption">
                                                                                Bills: &nbsp;</td>
                                                                            <td style="width: 103px">
                                                                                <input id="chkBills" type="checkbox" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 100px">
                                                                    <table>
                                                                        <tr>
                                                                            <td class="cell_caption">
                                                                                Branch:&nbsp;</td>
                                                                            <td style="width: 103px">
                                                                                <input id="chkBranch" type="checkbox">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="cell_caption">
                                                                                Form: &nbsp;
                                                                            </td>
                                                                            <td style="width: 103px;">
                                                                                <input id="chkForm" type="checkbox"></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                             <tr>
                                            <td style="text-align: left">
                                                <strong>Office Hours</strong></td>
                                            </tr>
                                             <tr>
                                             <td>
                                                <table>
                                                <tr>
                                                <td class="cell_caption">
                                                    AM: &nbsp;</td>
                                                    <td style="width: 103px">
                                                        <input id="Text1" type="text" /></td>
                                                    <td style="width: 103px">
                                                        <strong>To</strong></td>
                                                    <td style="width: 103px">
                                                        <input id="Text4" type="text" /></td>
                                                </tr>
                                                    <tr>
                                                        <td class="cell_caption">
                                                            PM: &nbsp;
                                                        </td>
                                                        <td style="width: 103px">
                                                            <input id="Text3" type="text" /></td>
                                                        <td style="width: 103px">
                                                            <strong>To</strong></td>
                                                        <td style="width: 103px">
                                                            <input id="Text5" type="text" /></td>
                                                    </tr>
                                                </table> 
       
                                             </td>
                                             </tr>
                                        </table>
                                    </div>
                                    <div id="Offices" style="display: block">
                                        <table id="tblOffices" cellpadding="0" cellspacing="5" width="100%">
                                            <tr>
                                                <td class="table_caption" colspan="5" style="width: 125px; color: ivory; text-indent: 5px;
                                                    text-align: left; height: 26px;">
                                                    Offices</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 24px; text-align: left;">
                                                    <strong>Mandatory Fields</strong></td>
                                                
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 24px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 23px; text-align: left;">
                                                    <strong>ID's</strong></td>
                                                
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="width: 225px; height: 23px; text-align: left">
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    
                            </tr>
                        </table>
                                        
                                    <br />
                                    <table id="tblbuttons" cellpadding="0" cellspacing="5" width="300">
                                        <tr>
                                            <td style="width: 25%">
                                                <input id="btnEdit" class="button" disabled="disabled" onclick="javascript:look_up.update_record();"
                                                    onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                    type="button" value="  Edit  " /></td>
                                            <td style="width: 25%">
                                                <input id="btnClear" class="button" onclick="javascript:look_up.clear();"
                                                    onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                    type="button" value=" Clear " /></td>
                                            <td style="width: 25%">
                                                <input id="btnSave" class="button" disabled="disabled" onclick="javascript:look_up.commit_record();"
                                                    onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                    type="button" value=" Save " /></td>
                                            <td style="width: 25%">
                                                <input id="btnCancel" class="button" disabled="disabled" onclick="javascript:look_up.rollback_record();"
                                                    onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                    type="button" value="Cancel" /></td>
                                        </tr>
                                    </table>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        </table>
    </form>
    </center>
</body>
</html>

  
