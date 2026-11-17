<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Charges.aspx.cs" Inherits="IRSWorld.BranchSetup.Charges" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Charges </title>
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../css/Default.css" rel="stylesheet"        type="text/css" />
    <style type="text/css">
    </style>
    <script src="../js/commons.js" type="text/javascript"></script>
    <script src="../js/datagrid.js" type="text/javascript"></script>   
    <script src="../js/BranchSetup/js_charges.js" type="text/javascript"></script>
    <script src="../js/popcalendar.js" type="text/javascript">
</script>
</head>
<body id="theBody" onload="page_setup();" onunload="close_session();">
    <center>
    <form id="form1" runat="server">
    <div id="divBody" style="width:100%;display:none; vertical-align:top; padding-top:25px;">        
        <table id="tblContent" style="width:100%;display:block">
            <tr>
                <td id="tdContent" style="width: 100%; height: 94%; border-collapse:collapse; vertical-align:middle;">
                    <div id="divBranchSetupContent" style="width:100%">
                        <table style="width:100%" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td style="width:100%; vertical-align:bottom" align=left>
                                    <table cellpadding="0" cellspacing="0" >
                                    <tr>
                                        <td align="left" style="width:10px; height:24px; border-collapse:collapse; vertical-align:bottom;"><img align=left src="../images/toe.GIF" /></td>
                                        <td id="td1" class="table_head_dark" style="width: 40%; height: 24px; border-collapse:collapse" align=left>
                                            Charges</td>
                                        <td align="left" style="width: 59%;height: 24px; vertical-align:bottom; border-collapse:collapse">
                                            <img align="absBottom" src="../images/rend.gif" /></td> 
                                    </tr>
                                    </table>
                                </td>                            
                            </tr>
                            <tr>
                                <td style="width:100%; border:solid 3px #336699" class=table_body align="center">
                                    <table style="width:100%">
                                        <tr>
                                            <td style="width:100%;vertical-align:top;" colspan=2 align=left>
                                                <table style="width:100%">
                                                    <tr>
                                                        <td style="width: 25%;" class=cell_caption>
                                                            Charge Group:&nbsp;</td>
                                                        <td style="width:70%;border-collapse:collapse" align="left">
                                                            <select id=ddbrgroup style="width: 54%" onchange="new_branch_group(this.value);"></select>
                                                            <input id="btnShowbr" onclick="show_chgroup_branches();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="»" style="width: 25px" />
                                                            &nbsp<span id=spclosetip class=comment style="visibility:hidden">click again to close popup</span>
                                                        </td>
                                                    </tr>                                                
                                                    <tr>
                                                        <td style="width: 25%;" class=cell_caption>Branch:&nbsp;</td>
                                                        <td style="width:70%" align="left">
                                                            <select id=ddbranches style="width: 70%" onchange="new_branch(this.value);"></select>
                                                            <input id="btnGoChgroup" onclick="goto_branch_group();" class="button" onmouseover="this.className='button_over';dom('spchgrouptip').style.visibility = 'visible';" onmouseout="this.className='button';dom('spchgrouptip').style.visibility = 'hidden'" type="button" value="«" style="width: 25px" />
                                                            &nbsp<span id=spchgrouptip class=comment style="visibility:hidden">Show Charge Group setting</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height:10px" valign=top></td>
                                                        <!-- * To use charge group, leave the Branch dropdown to &quot;Select&quot; position -->
                                                        <td class="comment">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td id=prodpane style="width: 25%;border: solid 1px gray;" valign="top">
                                                            <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                <tr>
                                                                    <td id=tdprodgrid style="height: 100%">
                                                                        <table id=tblprodgrid style="width:100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <select style="width:100%"></select>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>  
                                                        <td id=detlpane style="width: 75%" valign="top">
                                                            <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                <tr>
                                                                    <td style="width:100%" valign="top">
                                                                        <table style="width:100%" cellpadding=0 cellspacing=0>
                                                                            <tr>
                                                                                <td style="width:100%">
                                                                                    <div>
                                                                                        <table id=tblprodmast style="width:100%;">
                                                                                            <tr>
                                                                                                <td id=capprodmast class=table_caption colspan=5 style="width:100%;height: 22px" align=center>Product Name</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class=cell_caption style="width: 120px; height: 22px;">Branch Level:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <input id=ckbrlevel type=checkbox style="width: 9%" checked="CHECKED" onclick="prod_value_change(this, 2);" />&nbsp;<span class=cell_desc id=spbrlevel style="visibility:hidden">Negotiating Branch</span>
                                                                                                </td>
                                                                                                <td style="width:7px"></td>
                                                                                                <td class=cell_caption style="width: 120px; height:22px;">Currency:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <select id=ddcurrs style="width: 154px" onchange="prod_value_change(this, 5);"></select>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class=cell_caption style="width: 120px; height: 22px;">Pay Mode:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <select id=ddpaymode style="width: 154px" onchange="prod_value_change(this, 7);"></select>
                                                                                                </td>
                                                                                                <td style="width:7px"></td>
                                                                                                <td class=cell_caption style="width: 120px; height:22px;">Charge Type:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <select id=ddchtype style="width: 154px" onchange="prod_value_change(this, 6);"></select>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class=cell_caption style="width: 120px; height: 22px;">Implem. Type:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <select id=ddimplem style="width: 194px" onchange="prod_value_change(this, 4);"></select>
                                                                                                </td>
                                                                                                <td style="width:7px"></td>
                                                                                                <td class=cell_caption style="width: 120px; height:22px;">Default Status:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <select id=dddefstat style="width: 154px" onchange="prod_value_change(this, 9);"></select>
                                                                                                </td>
                                                                                            </tr> 
                                                                                            <tr>
                                                                                                <!--
                                                                                                <td class=cell_caption style="width: 130px; height: 22px;">Computation:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <select id=ddcomplevel style="width: 154px" onchange="prod_value_change(this, 8);"></select>
                                                                                                </td>
                                                                                                -->
                                                                                                <td class=cell_caption style="width: 120px; height: 22px;">
                                                                                                    Promotion:&nbsp;
                                                                                                </td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <input id=chkPromo type=checkbox style="width: 9%" checked="CHECKED" onclick="prod_value_change(this, 12);" />
                                                                                                    <span id=spvalidity class=cell_desc style="width:95%; font-weight: bold"></span>
                                                                                                </td>
                                                                                                <td style="width:7px"></td>
                                                                                                <td class=cell_caption style="width: 120px; height: 22px;">Clearing Days:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <input type=text id=txtcleardays style="width: 54px; text-align:right" value="0" onchange="prod_value_change(this, 10);" />
                                                                                                </td>                                                                                                
                                                                                            </tr> 
                                                                                            
                                                                                            <tr>
                                                                                                <td class=cell_caption style="width: 120px; height: 22px;">Charges Currency:&nbsp</td>
                                                                                                <td align=left style="height: 22px">
                                                                                                    <input id=chkChargesCurr type=checkbox style="width: 9%" checked="CHECKED" onclick="prod_value_change(this, 15);" />&nbsp;<label id=lblChargesCurr class=comment>Off - Remitting Currency</label>
                                                                                                </td>
                                                                                                <td style="width:7px"></td>
                                                                                                <td></td>
                                                                                                <td></td>
                                                                                            </tr> 
                                                                                            
                                                                                            <tr><td style="height:10px;" colspan=5></td></tr>
                                                                                            <tr>
                                                                                                <td colspan=5 style="width:100%; vertical-align:top;" align=center>
                                                                                                    <table id=tblprodbuttons style="width:50%" cellpadding="0" cellspacing="5px">
                                                                                                        <tr>
                                                                                                            <td style="width: 18%">
                                                                                                                <input id="btnSearch" onclick="prodgrid_search_clicked();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" /></td>
                                                                                                            <td style="width: 18%">
                                                                                                                <input id="tblprodgrid_footerbtn_Edit"  onclick="prodgrid.footerButtonClick('prod_btn_clicked', this.value);" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" /></td>
                                                                                                            <td style="width: 18%">
                                                                                                                <input id="tblprodgrid_footerbtn_Add"  onclick="prod_add_button_click(this);" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add" /></td>
                                                                                                            <td style="width: 18%">
                                                                                                                <input id="tblprodgrid_footerbtn_Delete"  onclick="prodgrid.footerButtonClick('prod_btn_clicked', this.value);" class="button"  onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Delete" /></td>
                                                                                                            <td style="width: 18%">
                                                                                                                <input id="tblprodgrid_footerbtn_Save"  class="button" onclick="prodgrid.footerButtonClick('prod_btn_clicked', this.value);" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" /></td>
                                                                                                            <td style="width: 18%">
                                                                                                                <input id="tblprodgrid_footerbtn_Cancel"  onclick="prodgrid.footerButtonClick('prod_cancel_click', this.value);" class="button" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
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
                                                                <tr>
                                                                    <td><hr /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class=navbar style="height:20px">
                                                                        <table id=tbltabs>
                                                                            <tr>
                                                                                <td id=acharges class=tab_selected><a href="javascript:show_div_charges('charges');">Charges</a></td>
                                                                                <td id=atier class=tab_unselected><a href="javascript:show_div_charges('tier');">Tiered Charges</a></td>
                                                                                <td id=ausers class=tab_unselected><a href="javascript:show_div_charges('users');">Users</a></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>                                                                
                                                                <tr>
                                                                    <td id=tdchgrid valign=top align=center style="width:100%;padding-top:0;background-color:DimGray;">
                                                                        <div id=div_charges style="width:100%;display:block">
                                                                            <!--
                                                                            <table id=tblchgrid width=100%>
                                                                                <caption class=cell_caption style="text-align:left">Charges</caption>
                                                                            </table>
                                                                            -->
                                                                        </div>
                                                                        <div id=div_tier style="width:70%;display:none; padding-top: 5px; padding-bottom:3px; background-color:DimGray;">
                                                                            <table id=tbltier style="width:100%">
                                                                                <caption class=cell_caption style="text-align:left">Tiered Charges</caption>
                                                                            </table>
                                                                        </div>
                                                                        <div id=div_users style="width:100%;display:none; padding-top: 5px; padding-bottom:3px; background-color:DimGray; overflow:auto">
                                                                            <table id=tblusers style="width:100%">
                                                                                <tr>
                                                                                <td id=tdusergrid style="width:50%" valign="top">
                                                                                    <!--
                                                                                    <table id=tblusergrid width=100% class=cell_caption>
                                                                                        <caption style="text-align:left">Allowed Users</caption>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th style="width:10%">Allowed</th>
                                                                                                <th style="width:90%">Role</th>
                                                                                            </tr>
                                                                                        </thead>
                                                                                    </table>
                                                                                    -->
                                                                                </td>
                                                                                <td id=tdovergrid style="width:50%" valign="top">
                                                                                    <!--
                                                                                    <table id=tblovergrid width=100% class=cell_caption>
                                                                                        <caption style="text-align:left">Override Permissions</caption>
                                                                                        <thead>
                                                                                            <tr>
                                                                                                <th style="width:10%">Allowed</th>
                                                                                                <th style="width:90%">Role</th>
                                                                                            </tr>
                                                                                        </thead>                                                                                        
                                                                                    </table>
                                                                                    -->
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
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>                            
                        </table>            
                    </div>
                    <div id="divSearch" style="width:100%;height:100%;display:none; vertical-align:middle">
                        <table style="width:100%">
                            <tr>
                                <td id=tdsearch style="width: 100%; vertical-align:middle" align="center">
                                    <table id=tblsearch; class="table_body" style="width:70%">
                                        <tr>
                                            <td class=table_caption colspan=5 style="width:100%; height: 22px;text-align:center">Product Search</td>
                                        </tr>
                                        <tr>
                                            <td class=cell_caption style="width: 130px; height: 22px;">Product:&nbsp</td>
                                            <td align=left style="height: 22px">
                                                <select id=ddsearch_ProductCode onchange="ddsearch_value_changed();" style="width: 194px"></select>
                                            </td>
                                            <td style="width:7px"></td>
                                            <td class=cell_caption style="width: 120px; height:22px;">Currency:&nbsp</td>
                                            <td align=left style="height: 22px; width: 157px;">
                                                <select id=ddsearch_Currency onchange="ddsearch_value_changed();" style="width: 194px"></select>
                                        </tr>
                                        <tr>
                                            <td class=cell_caption style="width: 130px; height: 22px;">Pay Mode:&nbsp</td>
                                            <td align=left style="height: 22px">
                                                <select id=ddsearch_PayMode onchange="ddsearch_value_changed();" style="width: 194px"></select>
                                            </td>
                                            <td style="width:7px"></td>
                                            <td class=cell_caption style="width: 120px; height:22px;">Charge Type:&nbsp</td>
                                            <td align=left style="height: 22px; width: 157px;">
                                                <select id=ddsearch_ChargeType onchange="ddsearch_value_changed();" style="width: 194px"></select>
                                        </tr>
                                        <tr>
                                            <td class=cell_caption style="width: 130px; height: 22px;">Implem. Type:&nbsp</td>
                                            <td align=left style="height: 22px">
                                                <select id=ddsearch_ImplementationType onchange="ddsearch_value_changed();" style="width: 194px"></select>
                                            </td>
                                            <td style="width:7px"></td>
                                            <td class=cell_caption style="width: 120px; height:22px;">Default Status:&nbsp</td>
                                            <td align=left style="height: 22px; width: 157px;">
                                                <select id=ddsearch_DefaultStatus onchange="ddsearch_value_changed();" style="width: 194px"></select>
                                        <!--
                                        <tr>
                                            <td class=cell_caption style="width: 130px; height: 22px;">
                                                Promotion:&nbsp;
                                            </td>
                                            <td align=left style="height: 22px">
                                                <input id=cksearch_Promotion type=checkbox style="width: 9%" checked="CHECKED" onclick="prod_value_change(this, 12);" />
                                            </td>

                                            <td style="width:7px"></td>
                                            <td class=cell_caption style="width: 120px; height: 22px;">Clearing Days:&nbsp</td>
                                            <td align=left style="height: 22px">
                                                <input type=text id=Text3 style="width: 54px; text-align:right" value="0" onchange="prod_value_change(this, 10);" />
                                            </td>
                                        </tr>
                                            -->
                                        <!--                                        
                                        <tr>

                                            <td class=cell_caption style="width: 130px; height: 22px;">Computation:&nbsp</td>
                                            <td align=left style="height: 22px">
                                                <select id=ddsearch_ComputationLevel style="width: 194px"></select>
                                            </td>
                                            <td style="width:7px"></td>
                                            <td class=cell_caption style="width: 120px; height: 22px;">Clearing Days:&nbsp</td>
                                            <td align=left style="height: 22px; width: 157px;">
                                                <input type=text id=ddsearch_clear style="width: 54px; text-align:right" value="0" onchange="prod_value_change(this, 10);" />
                                            </td>
                                        </tr> 
                                        -->
                                        </tr> 
                                        <tr>
                                            <td colspan=5 style="width:100%;border-collapse:collapse; display:none;"><hr /></td>
                                        </tr>
                                        <tr>
                                            <td colspan=5 style="width:100%;border-collapse:collapse; display:none;" align=center>
                                                <table style="width:100%">
                                                    <tr>
                                                        <td class=cell_caption style="width: 120px; height: 22px;">Start row:&nbsp</td>
                                                        <td align=left style="height: 22px; width: 157px;">
                                                            <input type=text id=txtsearch_start_row style="width: 110px; text-align:right" value="0" onchange="prod_value_change(this, 10);" language="javascript" onclick="return txtsearch_start_row_onclick()" />
                                                        <td style="width: 8px">&nbsp;</td>
                                                        <td class=cell_caption style="width: 120px; height: 22px;">End row:&nbsp</td>
                                                        <td align=left style="height: 22px; width: 157px;">
                                                            <input type=text id=txtsearch_end_row style="width: 110px; text-align:right" value="0" onchange="prod_value_change(this, 10);" />
                                                        
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan=5 style="width:100%;height:10px"><hr /></td>
                                        </tr>
                                        <tr>
                                            <td colspan=5 style="width:100%" align=center>
                                                <table id=Table2 width="50%" cellpadding="0" cellspacing="5px">
                                                    <tr>
                                                        <td style="width: 18%">
                                                            <input id="btnsearch_go" onclick="btnsearch_search_clicked();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Search" /></td>
                                                        <td style="width: 18%">
                                                            <input id="btnsearch_clear" onclick="btnsearch_clear_clicked();" class="button" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Clear" /></td>
                                                        <td style="width: 18%">
                                                            <input id="btnsearch_cancel" onclick="btnsearch_cancel_clicked();" class="button" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
                                                    </tr>
                                                </table>                                                                                                
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
        </div>
        <div id="divPromoDates" style="width:300px;display:none; vertical-align:middle; border: solid 2px midnightblue; z-index:5" class=ds_gray>
            <table id=tblPromoDates style="width:100%" cellspacing=5>
                <caption class=table_caption style="height: 22px; vertical-align:middle">Validity Period</caption>
                <tr>
                    <td class=cell_caption style="width: 120px; height: 22px;">
                        Valid From:&nbsp</td>
                    <td align=left style="height: 22px">
                        <input type=text id=txtPopValidFrom style="width: 116px; text-align:left"  />
                        <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtPopValidFrom, 'mm/dd/yyyy');" />
                    </td>
                </tr>
                <tr>
                    <td class=cell_caption style="width: 120px; height: 22px;">
                        &nbsp;To:&nbsp</td>
                    <td align=left style="height: 22px">
                        <input type=text id=txtPopValidTo style="width: 116px; text-align:left"  />
                        <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtPopValidTo, 'mm/dd/yyyy');" />
                    </td>
                </tr>  
                <tr><td style="height:8px" colspan=2></td></tr>     
                <tr>
                    <td colspan=2 style="width:100%" align=center>
                        <table style="width:100%">
                            <tr>
                                <td style="width:50%" align=center>
                                    <INPUT id="btnPromoOk" onmouseover="this.className='button_over';" class="button" onmouseout="this.className='button';" onclick="btnpromo_ok_clicked();" type=button value="Ok" />
                                </td>
                                <td style="width:50%" align=center>
                                    <INPUT id="btnClosePromo" onmouseover="this.className='warning_over';" class="button" onmouseout="this.className='button';" onclick="btnpromo_close_clicked();" type=button value="Cancel" />                    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>         
            </table>
        </div>
        <div id="divPopChgroup" style="width:420px;display:none; vertical-align:middle; border: solid 2px midnightblue; z-index:5">
            <table id=tblPopChgroup style="width:100%" cellspacing=5>
                <tr>
                    <td id=tdPopChgroup align=center style="width:100%; vertical-align:middle">
                        <div id=divPopChgroup_parent style="width:450px;">
                        </div>
                    </td>
                </tr>         
            </table>
        </div>        
    </form>
    </center>
</body>
</html>

