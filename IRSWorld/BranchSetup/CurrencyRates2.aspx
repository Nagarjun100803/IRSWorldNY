<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CurrencyRates2.aspx.cs" Inherits="IRSWorld.BranchSetup.CurrencyRates2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Fx Rates Maintenance Page</title>
    <link href="../css/Default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .tab_unselected A { color:#5697c1; text-decoration: none; }
        .tab_unselected A:hover { color:135f94; text-decoration: underline; }
        .tab_unselected A:visited { color:#5697c1; text-decoration: none; }
        .tab_selected A { color:#135f94; text-decoration: none; }
        .tab_selected A:hover { color:5697c1; text-decoration: underline; }
        .tab_selected A:visited { color:#135f94; text-decoration: none; }
        
        .cyber_a A { color:midnightblue; text-decoration: none; }
        .cyber_a A:hover { color:#135f94; text-decoration: underline; }
        .cyber_a A:visited { color:midnightblue; text-decoration: none; }        
        .cyber_a
        {
            font-family: Arial; font-weight: bold; font-size: 12px; color: midnightblue;
        }
        
        .tab_unselected 
        {
            width: 120px; 
            height: 100%;
            background-color: transparent;/*#135f94;*/
            color: #5697c1;
            font-family: Arial; font-weight: bold; font-size: 12px; 
            border-collapse: collapse;
            cursor: hand; 
            border: solid 2px #5697c1;       
        }
        .tab_selected
        {
            width: 120px; 
            height: 100%;
            background-color:#87ceeb;
            color: #135f94; 
            font-family: Arial; font-weight: bold; font-size: 12px; 
            border-collapse: collapse;
            cursor: hand;  
            border: solid 2px #5697c1;       
        }
    </style>
    <script src="../js/jsCommons.js" type="text/javascript"></script>   
    <script src="../js/popcalendar.js" type="text/javascript"></script>   
    <script>
    
        var issa;
        var currgrid;       
    
        function show_div_rate(dtype)
        {
            if (dtype == 'php')
            {
                document.getElementById('divFxPHP').style.display = 'block';
                document.getElementById('aphp').className = 'tab_selected';
                document.getElementById('divFxUSD').style.display = 'none';
                document.getElementById('ausd').className = 'tab_unselected';
                document.getElementById('divBmRate').style.display = 'none';
                document.getElementById('amid').className = 'tab_unselected';
            }
            else if (dtype == 'usd')
            {
                document.getElementById('divFxPHP').style.display = 'none';
                document.getElementById('aphp').className = 'tab_unselected';
                document.getElementById('divFxUSD').style.display = 'block';
                document.getElementById('ausd').className = 'tab_selected';
                document.getElementById('divBmRate').style.display = 'none';
                document.getElementById('amid').className = 'tab_unselected';
            }
            else
            {
                document.getElementById('divFxPHP').style.display = 'none';
                document.getElementById('aphp').className = 'tab_unselected';
                document.getElementById('divFxUSD').style.display = 'none';
                document.getElementById('ausd').className = 'tab_unselected';
                document.getElementById('divBmRate').style.display = 'block';
                document.getElementById('amid').className = 'tab_selected';
            }
        }
        
        function close_popup()
        {
            document.getElementById('divBmRate').bmgrid = null;
            document.getElementById('divFxUSD').fxgrid = null;
            document.getElementById('divFxPHP').fxgrid = null;            
            document.getElementById('divPopup').style.display = 'none';
        }
            
        function create_fxgrid()
        {
            var grid = new datagrid();
            {
	            var col = new datagrid_column();
	            col.header = 'SEQID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            grid.addColumn(col);	
		    }
            {
	            var col = new datagrid_column();
	            col.header = 'Currency';
	            col.datafield = 'CURRENCY_CODE';
	            col.align = 'center';
		        col.styles.cell += 'width:40%;';
	            grid.addColumn(col);	
		    }	
            {
	            var col = new datagrid_column();
	            col.header = 'Buying Rate';
	            col.datafield = 'BUYING_RATE';
	            col.align = 'right';
		        col.styles.cell += 'width:30%;';
		        col.styles.datafield += 'text-align:right;';
	            grid.addColumn(col);	
		    }
            {
	            var col = new datagrid_column();
	            col.header = 'Selling Rate';
	            col.datafield = 'SELLING_RATE';
	            col.align = 'right';
		        col.styles.cell += 'width:30%;';
		        col.styles.datafield += 'text-align:right;';
	            grid.addColumn(col);	
		    }
		    /*
            {
	            var col = new datagrid_column();
	            col.header = 'Guiding Rate';
	            col.datafield = 'GUIDING_RATE';
	            col.align = 'right';
		        col.styles.cell += 'width:20%;';
		        col.styles.datafield += 'text-align:right;';
	            grid.addColumn(col);	
		    }
		    */
		    
            grid.classNames.table = 'table_body';
            grid.row.normal_style = 'background-color: #f7ffff;'; 
            
            return grid; 
		}
        
        function create_forex_usdgrid(dt)
        {
            if (dt.Rows.length > 0)
            {
                var fxgrid = create_fxgrid();
                var asof = dateToString(dt.Rows[0]['VALUE_DATE']);
                fxgrid.caption = 'Foreign Currency in USD Terms AS OF: ' + asof;
                fxgrid.styles.table += "width: 98%;border:solid 1px #5697c1;";
                fxgrid.bind(dt, 'divFxUsd', 'fxUsdgrid');
                document.getElementById('divFxUsd').style.display = 'none';
                document.getElementById('divFxUsd').fxgrid = fxgrid;
            }
        }
        
        function create_forex_phpgrid(dt)
        {
            if (dt.Rows.length > 0)
            {
                var fxgrid = create_fxgrid();
                var asof = dateToString(dt.Rows[0]['VALUE_DATE']);
                fxgrid.caption = 'Foreign Currency in PHP Terms AS OF: ' + asof;
                fxgrid.styles.table += "width: 98%;border:solid 1px #5697c1;";
                fxgrid.bind(dt, 'divFxPHP', 'fxPhpgrid');
                document.getElementById('divFxPHP').style.display = 'none';
                document.getElementById('divFxPHP').fxgrid = fxgrid;
            }
        }        
        
        function create_bmgrid()
        {
            var grid = new datagrid();
            {
	            var col = new datagrid_column();
	            col.header = 'SEQID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            grid.addColumn(col);	
		    }
            {
	            var col = new datagrid_column();
	            col.header = 'Currency';
	            col.datafield = 'CURRENCY_CODE';
	            col.align = 'center';
		        col.styles.cell += 'width:40%;';
	            grid.addColumn(col);	
		    }	
            {
	            var col = new datagrid_column();
	            col.header = 'Booking Rate';
	            col.datafield = 'BOOKING_RATE';
	            col.align = 'right';
		        col.styles.cell += 'width:30%;';
		        col.styles.datafield += 'text-align:right;';
	            grid.addColumn(col);	
		    }
            {
	            var col = new datagrid_column();
	            col.header = 'Mid Rate';
	            col.datafield = 'MID_RATE';
	            col.align = 'right';
		        col.styles.cell += 'width:30%;';
		        col.styles.datafield += 'text-align:right;';
	            grid.addColumn(col);	
		    }
		    
            grid.classNames.table = 'table_body';
            grid.row.normal_style = 'background-color: #f7ffff;'; 
            
            return grid;        
        }
        
        function create_bmrate_grid(dt)
        {
            var bmgrid = create_bmgrid();
            var asof = dateToString(dt.Rows[0]['VALUE_DATE']);
            bmgrid.caption = 'BOOKING/MID RATES AS OF: ' + asof;
            bmgrid.styles.table += "width: 98%;border:solid 1px #5697c1;";
            bmgrid.bind(dt, 'divBmRate', 'bmgrid');
            document.getElementById('divBmRate').style.display = 'none';
            document.getElementById('divBmRate').bmgrid = bmgrid;
        }    
    
        function got_cybermag_rates(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var ds = rs.value[0];
            var msg = rs.value[1];
            if (ds.Tables[0].Rows.length > 0 | ds.Tables[1].Rows.length > 0)
            {
                create_forex_phpgrid(ds.Tables[0]);
                create_forex_usdgrid(ds.Tables[1]);
                create_bmrate_grid(ds.Tables[2]);
                var fxpos = calcpos(document.getElementById('tdfxrates'));
                var div = document.getElementById('divPopup');
                div.style.left = fxpos.left + 280;
                div.style.top = fxpos.top + 20;
                div.style.display = 'block';
                div.style.position = 'absolute';
                show_div_rate('php');
                return;
            }
            alert(document.getElementById('txtValueDate').value + " No available currency rates from cybermag.");        
        }
    
        function show_cyber_rates()
        {
            var sdt = sdt = document.getElementById('txtValueDate').value;
            if (sdt == null || sdt == '') sdt = get_current_date_string();
            IRSWorld.BranchSetup.CurrencyRates2.getCybermagRates(sdt, got_cybermag_rates);        
        }
        
        function fill_branch_ddl(rs)
        {
            if (rs.error != null) 
            {
                alert(rs.error.Message);
                return;
            }
            
            var selbr = rs.value[0];
            issa = rs.value[2];
            if (rs.value[1] == null)
            {
                spawn_errpage("CurrencyRates@fill_branch_ddl: server did not return values for branch ddl");
                return;
            }
            
            if (rs.value[3] == null | rs.value[4] == null | rs.value[5] == null)
            {
                spawn_errpage("CurrencyRates@fill_branch_ddl: server did not return values for Currency ddl");
                return;
            }
            
            var ddl = dom("ddbranch");
            set_ddl_options(ddl, rs.value[1]);
            ddl.value = selbr;
            ddl.disabled = !issa;
            
            set_ddl_options(dom('ddsource'), rs.value[3]);
            set_ddl_options(dom('ddtarget'), rs.value[4]);
            set_ddl_options(dom('ddcurrency'), rs.value[5]);
        }
        
        function src_tgt_formatter(val)
        {
            var fval = parseFloat(val);
            if (isNaN (fval))
            {
                fval = 0.0000000;
            }
            return formatNumber(fval, 7);
        }
        
        function currgrid_translate_currcode(val)
        {
            var code = parseInt(val);
            var ddl = dom('ddcurrency');
            ddl.value = code;
            
            return ddl.options[ddl.selectedIndex].text;
        }
                
        function define_currgrid()
        {
            currgrid = new datagrid();
            {//0
	            var col = new datagrid_column();
	            col.header = 'SID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            currgrid.addColumn(col);	
		    }	    
		    {//1
		        var col = new datagrid_column();
		        col.header = 'Location';
		        col.datafield = 'BranchCode';
		        col.align = 'left';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
		        currgrid.addColumn(col);
            }
		    {//2
		        var col = new datagrid_column();
		        col.header = 'Eff. Date';
		        col.datafield = 'TranxDate';
		        col.align = 'left';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
		        currgrid.addColumn(col);
            }            
            {//3
		        var col = new datagrid_column();
		        col.header = 'Source Currency';
		        col.datafield = 'SourceCurrency';
		        col.align = 'left';
		        col.formatter = 'currgrid_translate_currcode';
		        col.styles.cell += 'width:15%;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy; text-align:center;';
		        currgrid.addColumn(col);
		    }
            {//4
		        var col = new datagrid_column();
		        col.header = 'Target Currency';
		        col.datafield = 'TargetCurrency';
		        col.align = 'left';
		        col.formatter = 'currgrid_translate_currcode';
		        col.styles.cell += 'width:15%;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy;text-align:center;';
		        currgrid.addColumn(col);
		    }
            {//5
		        var col = new datagrid_column();
		        col.header = 'Source To Target';
		        col.datafield = 'SourceToTarget';
		        col.align = 'center';
		        col.styles.cell += 'width:20%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        currgrid.addColumn(col);
		    }
            {//6
		        var col = new datagrid_column();
		        col.header = 'Target To Source';
		        col.datafield = 'TargetToSource';
		        col.align = 'center';
		        col.editable = false;
		        col.formatter = 'src_tgt_formatter';
		        col.styles.cell += 'width:20%;;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';		        
		        currgrid.addColumn(col);
		    }		    
            {//7
		        var col = new datagrid_column();
		        col.header = 'Date Inserted';
		        col.datafield = 'DateInserted';
		        col.editable = false;
		        col.align = 'left';
		        col.styles.cell += 'width:15%;text-align:center;';
		        currgrid.addColumn(col);
		    }
		    {//8		    
		        var col = new datagrid_column();
	            col.header = 'RecID';
	            col.datafield = 'Recid';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
		        currgrid.addColumn(col);
		    }
		    {//9		    
		        var col = new datagrid_column();
	            col.header = 'PID';
	            col.datafield = 'Parid';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
		        currgrid.addColumn(col);
		    }
            {//10
		        var col = new datagrid_column();
		        col.header = 'Source';
		        col.datafield = 'SourceCurrency';
		        col.align = 'left';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
		        currgrid.addColumn(col);
		    }
            {//11
		        var col = new datagrid_column();
		        col.header = 'Target';
		        col.datafield = 'TargetCurrency';
		        col.align = 'left';
		        col.styles.cell += 'width:5%;display=none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
		        currgrid.addColumn(col);
		    }		    
		    
            currgrid.classNames.table = 'table_body';
            currgrid.row.normal_style = 'background-color: #f7ffff;';  
        } 
        
        function is_future_date(sval)
        {
            var dtin = Date.parse(sval);
            if (!isNaN(dtin))
            {
                var now = new Date().getTime();
                var val = now - dtin;
                return (val < 0);
            }
            
            return NaN;
        }   
        
        function got_rates(rs)
        {
            ajax_show_status(false); 
            
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            define_currgrid();
            currgrid.bind(rs.value, 'divfxrates', 'tblfxrates');
        }        
        
        function txtValueDate_changed(val)
        {
            if (val != null && val.length > 0)
            {
                var rv = isValidDate(val);
                if (rv[0])
                {
                    if (!is_future_date(val))
                    {
                        ajax_show_status(true);
                        var brcode = document.getElementById('ddbranch').value;
                        IRSWorld.BranchSetup.CurrencyRates2.getCurrencyRates(val, brcode, got_rates);
                        document.getElementById('divBody').style.display = 'block';
                        return;   
                    }
                    alert("Selection must be within current date");           
                }
                else
                {
                    alert(rv[1] + ": " + val);
                    document.getElementById('txtValueDate').focus();
                }
            }
        }   
        
        function currgrid_btnAdd_clicked()
        {
            var pup = dom('divFxAdd');
            pup.setAttribute('className', 'div_undock');
            pup.style.position = 'absolute';
            var tap = top.document.getElementById('divCenter');
            dom('tdfxadd').style.height = (tap) ? tap.offsetHeight : document.documentElement.clientHeight;
            dom('tdfxadd').style.width = (tap) ? tap.offsetWidth : document.documentElement.clientWidth;
            dom('txtEffDate').value = dom('txtValueDate').value;
            pup.style.display = 'block';
        }
        
        function page_setup()
        {
            ajax_show_status(true);
            var x = '';
            IRSWorld.BranchSetup.CurrencyRates2.getDdlOptions(x, fill_branch_ddl);
            var tdt = document.getElementById('txtValueDate');
            tdt.value = get_current_date_string().split(' ')[0];
            ajax_show_status(false);
            tdt.fireEvent('onchange');  
        }        
        
    </script> 
</head>
<body id="theBody" onload="page_setup();">
    <form id="form1" runat="server">
    <div id="divBody" style="width:98%;display:block; vertical-align:top; margin-top:30px;">
        <table width="100%" height="100%" cellpadding="0" cellspacing="5">
            <tr>    
                <td align="center" valign="middle" style="width:100%; border-collapse:collapse">
                    <table width="95%" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="left" valign="bottom" style="width:1%;height:24px; border-collapse: collapse"> 
                                <img src="../images/toe.GIF" /></td>
                            <td id="capLabel" align="left" class="table_head_dark" style="width:40%; height: 24px; border-collapse:collapse">
                                Currency Rates</td>
                            <td align="left" valign="bottom" style="height: 24px" class="bg_body">
                                <img align="absbottom" src="../images/rend.gif" /></td>
                        </tr>
                        <tr>
                            <td colspan="3" style="width: 100%; border:solid 3px #336699;" class=table_body>
                                <table style="width:100%;" cellspacing="5">
                                    <tr>    
                                        <td class=cell_caption style="width: 25%; height: 21px">Branch:&nbsp;</td>
                                        <td style="width:60%; height: 21px;" align="left">
                                            <select id=ddbranch style="width: 100%; height:100%" onchange="new_branch(this.value);"></select>
                                        </td>
                                        <td align=left style="height:21px">
                                        </td>
                                    </tr>
                                    <tr>    
                                        <td class=cell_caption style="width: 25%; height: 21px">Effective Date:&nbsp;</td>
                                        <td align="left">
                                            <input type="text" id="txtValueDate" onchange="txtValueDate_changed(this.value);" class=cell_desc style="width: 93px; height: 100%;" />
                                            <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtValueDate, 'mm/dd/yyyy');" />
                                        </td>
                                        <td align=left style="height:21px">
                                            <a id=acybermag class=cyber_a href="javascript:show_cyber_rates();">Cybermag&nbsp;Rates&nbsp;&raquo;</a>
                                        </td>                                        
                                    </tr>  
                                    <tr>
                                        <td id="tdfxrates" style="width: 100%;" colspan="3" class="cell_desc">
                                            <!-- content starts here, if necessary increase/decrease the size of the bounding table -->
                                            <div id="divfxrates" style="width:100%; height: 305px; border: solid 1px midnightblue;">
                                            </div>
                                            <!-- content ends here -->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan=3 style="width:100%"><hr /></td>
                                    </tr> 
                                    <tr>
                                        <td colspan=3 style="width:100%" align=center>
                                            <table id=tblprodbuttons width="50%" cellpadding="0" cellspacing="5px">
                                                <tr>
                                                    <td style="width: 18%">
                                                        <input id="btnEdit"  onclick="prodgrid.footerButtonClick('prod_btn_clicked', this.value);" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Edit" /></td>
                                                    <td style="width: 18%">
                                                        <input id="btnAdd"  onclick="currgrid_btnAdd_clicked(this);" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add" /></td>
                                                    <td style="width: 18%">
                                                        <input id="btnDelete"  onclick="prodgrid.footerButtonClick('prod_btn_clicked', this.value);" class="button"  onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Delete" /></td>
                                                    <td style="width: 18%">
                                                        <input id="btnSave"  class="button" onclick="prodgrid.footerButtonClick('prod_btn_clicked', this.value);" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Save" /></td>
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
    <div id="divPopup" style="width: 330px;border: solid 1px midnightblue; display: none; z-index: 10;" class="ds_gray">
        <table width="100%" height="100%" cellpadding="0" cellspacing="0">
            <tr>
                <td id=td_popup style="width:320px; height: 15px; color:Ivory; font: bold 12px Arial" class="table_head_dark" align="left">&nbsp;&nbsp;Cybermag Rates</td>
                <td style="width:10px" class="table_head_dark" align="right">
                    <input type="button" value="x" onclick="close_popup();" style="width:15px; color: red" class="table_head_dark" />
                </td>
            </tr>
            <tr>
                <td style="width: 100%; height: 15px" align="left" colspan="2">
                <table>
                    <tr>
                        <td id=aphp class=tab_unselected><a href="javascript:show_div_rate('php');">FX in PHP Terms</a></td>
                        <td id=ausd class=tab_selected><a href="javascript:show_div_rate('usd');">FX in USD Terms</a></td>
                        <td id=amid class=tab_unselected><a href="javascript:show_div_rate('mid');">Booking Rates</a></td>
                    </tr>
                </table></td>
            </tr>
            <tr>
                <td style="width:100%; height:340px;" align="center" colspan="2">
                    <div id="divFxPHP" style="width:100%; height:100%; overflow:auto; display:none">
                    </div>                
                    <div id="divFxUSD" style="width:100%; height:100%; overflow:auto; display:none">
                    </div>
                    <div id="divBmRate" style="width:100%; height:100%; overflow:auto; display:none">
                    </div>                    
                </td>
            </tr>
        </table>
    </div>
    <div id="divFxAdd" style="width:100%;height:100%;display:none; vertical-align:middle;"> <!-- class="div_undock"> -->
            <table>
                <tr>
                    <td id=tdfxadd style="width: 100%; vertical-align:middle" align="center">
                        <div style="width:61%">
                        <table id=tbladdfx; class="table_body">
                            <tr>
                                <td class=table_caption colspan=5 style="width:100%; height: 24px;text-align:center">Add Currency Rates</td>
                            </tr>
                            <tr>
                                <td class=cell_caption style="width: 130px; height: 24px;">
                                    Effective Date:&nbsp</td>
                                <td align=left style="height: 24px">
                                    <input type="text" id="txtEffDate" onchange="txtEffDate(this.value);" class=cell_desc style="width: 124px; height: 100%;" />
                                    <img src="../images/calendar/imgcalen.png" align="absmiddle" onclick="popUpCalendar(this, document.forms[0].txtValueDate, 'mm/dd/yyyy');" />
                                </td>
                                <td style="width:7px"></td>
                                <td style="width: 130px; height:24px;"></td>
                                <td align=left style="height: 24px; width: 157px; visibility:hidden">
                                    <select id=ddcurrency style="width: 130px; visibility:hidden"></select>
                                </td>
                            </tr>                            
                            
                            <tr>
                                <td class=cell_caption style="width: 130px; height: 24px;">Source Currency:&nbsp</td>
                                <td align=left style="height: 24px">
                                    <select id=ddsource style="width: 130px"></select>
                                </td>
                                <td style="width:7px"></td>
                                <td class=cell_caption style="width: 130px; height:24px;">Target Currency:&nbsp</td>
                                <td align=left style="height: 24px; width: 157px;">
                                    <select id=ddtarget style="width: 130px"></select>
                            </tr>
                            
                            <tr>
                                <td id=lblsource class=cell_caption style="width: 130px; height: 24px;">Source:&nbsp</td>
                                <td align=left style="height: 24px">
                                    <input type="text" id="txtSrcToTarget" class=cell_desc style="width: 152px; height: 100%;" />
                                </td>
                                <td style="width:7px"></td>
                                <td id=lbltarget class=cell_caption style="width: 130px; height:24px;">Target:&nbsp</td>
                                <td align=left style="height: 24px; width: 157px;">
                                    <input type="text" id="txtTargetToSrc" class=cell_desc style="width: 152px; height: 100%;" />
                            </tr>                            
                            
                            <tr>
                                <td colspan=5 style="width:100%"><hr /></td>
                            </tr>
                            <tr>
                                <td colspan=5 style="width:100%" align=center>
                                    <table id=tblPopAdd width="50%" cellpadding="0" cellspacing="5px">
                                        <tr>
                                            <td style="width: 18%">
                                                <input id="btnPopAdd" onclick="btnadd_clicked();" class="button" onmouseover="this.className='button_over';" onmouseout="this.className='button';" type="button" value="Add" /></td>
                                            <td style="width: 18%">
                                                <input id="btnPopClear" onclick="btnclear_clicked();" class="button" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Clear" /></td>
                                            <td style="width: 18%">
                                                <input id="btnPopCancel" onclick="btncancel_clicked();" class="button" onmouseover="this.className='warning_over';" onmouseout="this.className='button';" type="button" value="Cancel" /></td>
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
    
    </form>
</body>
</html>
