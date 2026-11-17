<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BranchSetup.aspx.cs" Inherits="IRSWorld.BranchSetup.BranchSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Branch Setup</title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default_min.css" rel="stylesheet" type="text/css" />
    <script src="../js/commons_min.js" type="text/javascript"></script>
    <script src="../js/BranchSetup/js_BranchSetup.js" type="text/javascript"></script>
    <script language = "javascript" type="text/javascript">
    </script>
</head>
<body id="theBody" onload="page_setup();">
    <form id="form1" runat="server">
    <div id="divBody" style="width:98%;display:none; vertical-align:top; padding-top:30px;">        
        <table id="tblContent" style="width:100%;">
            <tr>
                <td id="tdContent" style="width: 85%; border-collapse:collapse; vertical-align:middle;" align=center>
                    <div id="divBranchSetupContent" style="width:70%;">
                        <table style="width:100%" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                        <td id="tdPageTitle" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            Branch Setup</td>
                                        <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:100%; border:solid 3px #336699;border-collapse:collapse" class=table_body align="center">
                                    <br />
                                    <div id="divBranchSetting" style="width: 98%; display: block">
                                        <table  cellpadding="0" cellspacing="5" style="width:95%">
                                               <tr>
                                                    <td class="cell_caption" style="width: 35%; height:23px">
                                                        Branch (Location):&nbsp;</td>
                                                    <td align="left" style="width: 70%;height: 23px;">
                                                      <select id="ddSetupBranch" style="width:100%" onchange="new_branch(this.value);">
                                                      </select>                                                        
                                                    </td>
                                               </tr>                                        
                                        </table>
                                       <table id="tblBranchSetupEntry" cellpadding="0" cellspacing="5" style="width:95%">
                                            <tr>
                                               <td class="cell_caption" style="width: 35%; height: 23px">
                                                   Cutoff Start:&nbsp;</td>
                                                <td align="left" style="width: 376px;height: 24px;">
                                                    <input id=txtCutStart value="HH:MM:SS tt" style="width: 28%" onfocus="this.select();" onblur="value_changed(this);" />
                                                </td>
                                            </tr>
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   Cutoff End:&nbsp;</td>
                                                <td align="left" style="width: 376px;height: 24px;">
                                                    <input id=txtCutEnd value="HH:MM:SS tt" style="width: 28%" onfocus="this.select();" onblur="value_changed(this);" />
                                                </td>
                                           </tr>
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;Stale Period: &nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=text style="width:22%; text-align: right;" id="txtStale" onfocus="this.select();" onblur="value_changed(this);" />
                                                   <label class=cell_desc>&nbsp;Days</label></td>
                                           </tr>
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   Comm. Setup:&nbsp;
                                               </td>
                                              <td align="left" style="height: 24px; width: 376px;">
                                                  <select id="ddcommsetup" style="width:34%" onblur="value_changed(this);">
                                                      
                                                  </select>
                                              </td>
                                           </tr>
                                            <tr>
                                                <td style="height: 100%; width: 100%;" align="center" colspan="2">
                                               </td> 
                                            </tr> 
                                            <tr>
                                               <td class="cell_desc" style="width: 35%; height: 23px" colspan=2 align=left valign=bottom>
                                                   Remittance Amount To Be Monitored:&nbsp;</td>
                                            </tr>                                                                                        
                                            <tr>
                                               <td class="cell_caption" style="width: 35%; height: 23px">
                                                   Required ID Limit:&nbsp;</td>
                                                <td align="left" style="width: 376px;height: 24px;">
                                                    <input type="text" id="txtAmountLimit" value="0.00" style="width: 33%; text-align:right" onfocus="this.select();" onblur="value_changed(this);" />&nbsp;
                                                    <input type="checkbox" id="ckAggregateAmtLimit" checked style="width: 7%; color: navy;zoom:25px;" onclick="value_changed(this);" />&nbsp;<label class="cell_desc">Aggregate</label>
                                                    <input type="checkbox" id="chkAgentAggAmtReject" checked style="width: 7%; color: navy;zoom:25px; display:none;" onblur="value_changed(this);" />&nbsp;<label id="lblAgentReject" style="display:none;" class="cell_desc">Agent Reject</label>
                                                </td>
                                            </tr>
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   Required IDs:&nbsp;
                                               </td>
                                              <td align="left" style="height: 24px; width: 376px;">
                                                  <select id="ddreqid" style="width:69%" onblur="value_changed(this);">
                                                  </select>
                                              </td>
                                           </tr>
                                           <tr><td colspan=2 style="height:3px"></td></tr>
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   Supervisor Limit:&nbsp;</td>
                                                <td align="left" style="width: 376px;height: 24px;">
                                                    <input id=txtSuperLimit value="0.00" style="width: 33%; text-align: right" onfocus="this.select();" onblur="value_changed(this);" />
                                                </td>
                                           </tr>
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;Teller Limit: &nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=text style="width:33%; text-align: right;" id="txtTellerLimit" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                   </td>
                                           </tr>
                                            <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;ARM Limit: &nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=text style="width:33%; text-align: right;" id="txtArmLimit" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                   </td>
                                           </tr>                                           
                                            <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;Internet Transaction Limit: &nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=text style="width:33%; text-align: right;" id="txtInetLimit" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                   </td>
                                           </tr>
                                            <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;3 Days Aggregation Amount:&nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=text style="width:33%; text-align: right;" id="txtThreeDayAggregation" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                </td>
                                            </tr>
                                            <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;30 Days Aggregation Amount:&nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=text style="width:33%; text-align: right;" id="txtThirtyDayAggregation" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                </td>
                                            </tr>
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;Enable Cash Aggregation: &nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=checkbox id=ckCashAggregation style="width: 7%; color: navy;zoom:25px;" onclick="value_changed(this);" />
                                                   </td>
                                            </tr>
                                            <tr id="trCashAggregation" style="border-collapse:collapse; display: none;">
                                               <td class="cell_caption" style="height: 23px">
                                                   &nbsp;Cash Aggregation Amount Limit:&nbsp;
                                               </td>
                                               <td align="left" style="width: 376px; height: 24px;">
                                                    <input type=text style="width:33%; text-align: right;" id="txtCashAggregationAmt" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                </td>
                                            </tr>
                                            
                                            <tr id="trAgentLogReport" style="border-collapse:collapse; display: none;">
                                                <td class="cell_caption" style="height: 23px">
                                                    &nbsp;Agent Daily Keeping Log Report:&nbsp;
                                                </td>
                                                <td align="left" style="width: 376px; height: 24px;">
                                                    <input type="text" style="width:33%; text-align: right;" id="txtAgentLogReport" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />&nbsp;
                                                    <input type="checkbox" id="chkAgentLogReport" checked style="width: 7%; color: navy;zoom:25px;" onblur="value_changed(this);" />&nbsp;<label class="cell_desc">System Control</label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cell_caption" style="height: 23px">
                                                    &nbsp;Check ID on Amount:&nbsp;
                                                </td>
                                                <td align="left" style="width: 376px; height: 24px;">
                                                    <input type="text" style="width:33%; text-align: right;" id="txtCheckIDAmount" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />&nbsp;
                                                </td>
                                            </tr>
                                            
                                           <tr id=tdd_rows style="display:none; border-collapse:collapse;">
                                                <td id=tdd_cell colspan=2 style="width:100%; height: 50px; border: solid 1px midnightblue;">
                                                    <table cellpadding=0 cellspacing=5  style="width:100%">
                                                        <tr>
                                                               <td class="cell_caption" style="width: 35%; height: 23px">
                                                                   &nbsp;T$$ Straight Implementation:&nbsp;
                                                               </td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                    <input type=text style="width:70%; text-align: right;" id="txt_tdd_stimp_min" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                                    <label class=cell_desc>&nbsp;Min.</label>
                                                                </td>
                                                                <td style="width:10px">&nbsp;</td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                    <input type=text style="width:70%; text-align: right;" id="txt_tdd_stimp_max" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                                    <label class=cell_desc>&nbsp;Max.</label>
                                                                </td>
                                                        </tr>
                                                        <!--
                                                        <tr>
                                                               <td class="cell_caption" style="width: 35%; height: 23px">
                                                                   &nbsp;T$$ Funds With Clearing Days:&nbsp;
                                                               </td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                    <input type=text style="width:70%; text-align: right;" id="txt_tdd_cldays_min" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                                    <label class=cell_desc>&nbsp;Min.</label>
                                                                </td>
                                                                <td style="width:10px">&nbsp;</td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                    <input type=text style="width:70%; text-align: right;" id="txt_tdd_cldays_max" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                                    <label class=cell_desc>&nbsp;Max.</label>
                                                                </td>
                                                        </tr>  
                                                        -->
                                                         <tr>
                                                               <td class="cell_caption" style="width: 35%; height: 23px">
                                                                   &nbsp;T$$ First Time Remitter Straight Implementation:&nbsp;
                                                               </td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                    <input type=text style="width:70%; text-align: right;" id="txt_tdd_firsttime_stimp_min" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                                    <label class=cell_desc>&nbsp;Min.</label>
                                                                </td>
                                                                <td style="width:10px">&nbsp;</td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                    <input type=text style="width:70%; text-align: right;" id="txt_tdd_firsttime_stimp_max" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                                    <label class=cell_desc>&nbsp;Max.</label>
                                                                </td>
                                                        </tr>
                                                         <tr id="trDailyLimit" style="display: none; border-collapse:collapse;">
                                                               <td class="cell_caption" style="width: 35%; height: 23px">
                                                                   &nbsp;Daily Limit:&nbsp;
                                                               </td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                    <input type=text style="width:70%; text-align: right;" id="txt_daily_limit" value="0.00" onfocus="this.select();" onblur="value_changed(this);" />
                                                                </td>
                                                                <td style="width:10px">&nbsp;</td>
                                                               <td align="left" style="width: 200px; height: 24px;">
                                                                </td>
                                                        </tr>                                                                                                                                                                            
                                                    </table>
                                                </td>
                                           </tr>                                                
                                           
                                           <tr>
                                               <td class="cell_caption" style="height: 23px">
                                                   Fx Rate Deviation:&nbsp;
                                               </td>
                                              <td align="left" style="height: 24px; width: 376px;">
                                                  <input type=text style="width:23%; text-align: right;" id="txtFxDeviate" value="0" onfocus="this.select();" onblur="value_changed(this);" />
                                                  <label class=cell_desc>%</label></td>
                                           </tr>                                                                                                                                         
                                        </table>                                        
                                        <table cellpadding="0" cellspacing="0" style="width:100%">
                                            <tr><td style="height:10px"></td></tr>
                                            <tr>
                                                <td style="width: 100%" align="center">
                                                    <table id="tblbuttons" cellpadding="0" cellspacing="5">
                                                        <tr>
                                                            <td style="width: 20%;" valign=middle>
                                                                <input id="btnAdd" class="button" onclick="javascript:new_setup();"
                                                                    onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                    type="button" value="New" /></td>
                                                            <td style="width: 20%">
                                                                <input id="btnEdit" class="button"  onclick="javascript:update_setup();"
                                                                    onmouseout="this.className='button';" onmouseover="this.className='button_over';"
                                                                    type="button" value="Edit" /></td>
                                                            <td style="width: 20%">
                                                                <input id="btnDelete" class="button"  onclick="javascript:delete_setup();"
                                                                    onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                    type="button" value="Delete" /></td>
                                                            <td style="width: 20%">
                                                                <input id="btnSave" class="button" onclick="javascript:save_setup();"
                                                                    onmouseout="this.className='button';" onmouseover="this.className='button_over';" 
                                                                    type="button" value="Save" /></td>
                                                            <td style="width: 20%">
                                                                <input id="btnCancel" class="button" onclick="javascript:cancel_setup();"
                                                                    onmouseout="this.className='button';" onmouseover="this.className='warning_over';"
                                                                    type="button" value="Cancel" /></td>
                                                        </tr>
                                                    </table>                                                
                                                </td>
                                            </tr>
                                            <tr><td style="height:10px">&nbsp;</td></tr>
                                            </table>
                                    </div>
                                    <div id=divReceipt style="display:none">
                                    </div>
                                    <div id=divRecptArchive style="display:none">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp</td>
                            </tr>
                        </table>            
                    </div>
                </td>
            </tr>        
        </table>
        </div>
    </form>
</body>
</html>

  
