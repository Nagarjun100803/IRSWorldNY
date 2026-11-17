        var ajax_timeout = 60 * 1000;
        
        var go_chgroup;
        var chgroup;
        var selbr;
        var issa;
        
        var prodopts;
        var chargopts;
        var comptypes;
        var curropts;
        
        var prodgrid;
        var chargrid;
        var tiergrid;
        var brgrid;
        
        var save_add;
        var save_pid;
        var save_datarow;
        var save_gridrow;
        
        var no_chpop;
        var ch_pos;
        var ch_datrow;
        var check_min_amt;
                
        function keepAlive() 
        {
            IRSWorld.BranchSetup.Charges.keepAlive(function(){});
        }

        function init_ddl(tid, dt, val, desc, ival)
        {
            var ddl = document.getElementById(tid);
            if (ddl.options.length > 0) ddl.options.length = 0;
            if (dt.Rows.length > 0)
            {
                create_option(ddl, '0', 'Select');
                
                for (var i = 0; i < dt.Rows.length; i++)
                {
                    create_option(ddl, dt.Rows[i][val], dt.Rows[i][desc]);
                }  
                
                ddl.value = ival;          
            }
        }
        
        function copy_options(ddsrc, ddtgt)
        {
            for (var i = 0; i < ddsrc.options.length; i++)
            {
                create_option(ddtgt, ddsrc.options.item(i).value, getText(ddsrc.options.item(i)));
            }
        }
        
        function load_search_ddls()
        {
            if (document.getElementById('ddsearch_ProductCode').options.length == 0) set_ddl_options(document.getElementById('ddsearch_ProductCode'), prodopts);
            if (document.getElementById('ddsearch_PayMode').options.length == 0) copy_options(document.getElementById('ddpaymode'), document.getElementById('ddsearch_PayMode'));
            if (document.getElementById('ddsearch_ImplementationType').options.length == 0) copy_options(document.getElementById('ddimplem'), document.getElementById('ddsearch_ImplementationType'));
            if (document.getElementById('ddsearch_Currency').options.length == 0) copy_options(document.getElementById('ddcurrs'), document.getElementById('ddsearch_Currency'));
            if (document.getElementById('ddsearch_ChargeType').options.length == 0) copy_options(document.getElementById('ddchtype'), document.getElementById('ddsearch_ChargeType'));
            if (document.getElementById('ddsearch_DefaultStatus').options.length == 0) copy_options(document.getElementById('dddefstat'), document.getElementById('ddsearch_DefaultStatus'));
            
            if (document.getElementById('ddsearch_ImplementationType'))
            {
                document.getElementById('ddsearch_ImplementationType').disabled = false;
            }
        }
        
        function prodgrid_search_clicked()
        {
            load_search_ddls();
            
            var div = document.getElementById('divSearch');
            var td = document.getElementById('tdsearch');
            div.style.height = top.document.getElementById('divCenter').offsetHeight + 'px';
            div.style.display = 'block';
            div.className = 'div_undock';
            td.style.height = div.offsetHeight + 'px';
        }
        
        function btnsearch_cancel_clicked()
        {
            document.getElementById('divSearch').style.display = 'none';
        }
        
        function ddsearch_value_changed()
        {
            document.getElementById('txtsearch_start_row').value = 0;
            document.getElementById('txtsearch_end_row').value = prodgrid.dataRowsSize() - 1;
        }
        
        function btnsearch_clear_clicked()
        {
            document.getElementById('ddsearch_ProductCode').selectedIndex = 0;
            document.getElementById('ddsearch_PayMode').selectedIndex = 0;
            document.getElementById('ddsearch_ImplementationType').selectedIndex = 0;
            document.getElementById('ddsearch_Currency').selectedIndex = 0;
            document.getElementById('ddsearch_ChargeType').selectedIndex = 0;
            document.getElementById('ddsearch_DefaultStatus').selectedIndex = 0;
            
            ddsearch_value_changed();
        }
                
        function btnsearch_search_clicked()
        {
            var ddls = document.getElementById('divSearch').getElementsByTagName('select');
            var sels = 0;
            var i = 0;
            
            for (; i < ddls.length; i++)
            {
                sels += ddls.item(i).selectedIndex;
            }
            
            if (sels  == 0) 
            {
                alert('No Search criteria.');
                return;
            }

            var find_args = new datagrid_find_arguments();
            find_args.start_row = (isNaN(parseFloat(document.getElementById('txtsearch_start_row').value))) ? 0 : parseInt(document.getElementById('txtsearch_start_row').value);
            find_args.end_row = (isNaN(parseFloat(document.getElementById('txtsearch_end_row').value))) ? -1 : parseInt(document.getElementById('txtsearch_end_row').value) + 1;
            var need_and = false;
            
            for (i = 0; i < ddls.length; i++)
            {
                if (ddls.item(i).selectedIndex > 0)
                {
                    if (need_and)
                    {
                        find_args.addConjunction('&');
                    }
                    
                    var farg = new datagrid_find_argument();
                    farg.column_name = ddls.item(i).id.split('_')[1];
                    farg.value = ddls.item(i).value;
                    
                    find_args.addArgument(farg);
                    need_and = true;
                }
            }

            var locrow = prodgrid.find(find_args, true);
            if (locrow > -1)
            {
                document.getElementById('txtsearch_start_row').value = (locrow == (prodgrid.dataRowsSize() - 1)) ? 0 : locrow + 1;
                btnsearch_cancel_clicked();
            }
            else
            {
                alert('No rows found');
            }

        }
        
        function filter_defstat(dt)
        {
            var table = function()
            {
                this.Rows = new Array();
            }
            
            var rows = function()
            {
                this.fldValue = 0;
                this.fldDescription = '';
            };
            
            var dtstat = new table();
            
            for (var i = 0; i < dt.Rows.length; i++)
            {
                var txt = dt.Rows[i].fldDescription.toLowerCase();
                if (txt == "new transaction" | txt == "for approval" | txt == "for batch processing" | txt == "for i-batch processing" | txt == "waiting for funds - miccs")
                {
                    var arow = new rows();
                    arow.fldValue = dt.Rows[i].fldValue;
                    arow.fldDescription = dt.Rows[i].fldDescription;
                    dtstat.Rows[dtstat.Rows.length] = arow;
                }
            }
            
            return dtstat;
        }
                    
        function prod_updates_saved(rs)
        {
            if (go_chgroup) close_pop_chgroup();
            if (rs.error != null)
            {
                alert ('Product Updates saved: ' + rs.error.Message);
                document.getElementById('btnSearch').disabled = false;
                ajax_show_status(false);                
                return;
            }
            
            prodgrid.clearChanges();
            prodgrid.refresh(rs.value);
            
            if (save_add)
            {
                var rspid = IRSWorld.BranchSetup.Charges.getAddedProductId();
                
                if (rspid.value != null && rspid.value > -1)
                {
                    save_pid = rspid.value;
                }
                save_add = false;
            }
            
            var find_args = new datagrid_find_arguments();
            var farg = new datagrid_find_argument();
            farg.column_name = 'SEQ_ID';
            farg.value = save_pid;
            find_args.addArgument(farg);
            var locrow = prodgrid.find(find_args, true);
            
            set_read_only(document.getElementById('tblprodmast'), true);
            document.getElementById('ddbranches').disabled = false;
            
            var auser = document.getElementById('ausers');
            if (auser.disabled)
            {
                auser.disabled = false;
                auser.href="javascript:show_div_charges('users');";
            }
            prodgrid.row.onclick_enabled = true;
            prodgrid.enableFooterBtns("SEL");
            document.getElementById('btnSearch').disabled = false;
            
            ajax_show_status(false);                
        }
        
        function prod_set_empty(args, nodtl)
        {   
            var vals = new Array(prodgrid.columns.length);
            vals[0] = "0";
            vals[1] = document.getElementById('ddbranches').value;
            vals[2] = false;
            vals[3] = prodgrid.getValueAt(args.gridrow, 3);
            vals[4] = 0;
            vals[5] = 0;
            vals[6] = 0;
            vals[7] = 0;
            vals[8] = 0;
            vals[9] = 0;
            vals[10] = 0;
            vals[11] = document.getElementById('ddbrgroup').value;
            vals[12] = false;
            vals[13] = null;
            vals[14] = null;
            vals[15] = false;
            
            prod_set_fields(vals, nodtl);
        }
        
        function show_affected_branches(rs)
        {
            if (rs.error != null)
            {
                alert("Show ChargeGroup Branches: " + rs.error.Message);
                return;
            }
            
            var for_save = rs.value[0];
            var dt = rs.value[1];
            
            
            brgrid = new datagrid();
            {//0
                var col = new datagrid_column();
	            col.header = 'BranchCode';
	            col.datafield = 'BranchCode';
	            col.align = 'left';
		        col.styles.cell += 'width:30%;text-align:left;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
	            brgrid.addColumn(col);	
		    }
            {//1
                var col = new datagrid_column();
		        col.header = 'Branch Name';
		        col.datafield = 'BranchName';
		        col.align = 'left';
		        col.styles.cell += 'width:70%;text-align:left;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
	            brgrid.addColumn(col);	
		    } 
		    
		    if (for_save)
		    {
                brgrid.caption = 'AFFECTED BRANCHES IN CHARGE GROUP';
                brgrid.row.selected_style = 'background-color: beige;';
            }
            else
            {
                brgrid.caption = 'BRANCHES IN CHARGE GROUP';
                brgrid.row.onclick = 'select_chgroup_branch';
            }
            brgrid.bind(dt, 'divPopChgroup_parent', 'gvPopChgroup');
            
            var div = document.getElementById('divPopChGroup');
            if (for_save)
            {
                var td= document.getElementById('tdPopChGroup');
                td.setAttribute('align', 'center');
                div.style.height = top.document.getElementById('divCenter').offsetHeight;
                div.style.width = document.documentElement.clientWidth;
                div.className = 'div_undock';
                td.style.height = div.offsetHeight;  
            }
            else
            {
                div.className = 'ds_gray';
                document.getElementById('tdPopChGroup').setAttribute('align', 'left');
                div.style.position = 'absolute';
                div.style.zIndex = 5;
                var xy = calcpos(document.getElementById('btnShowbr'));
                div.style.top = xy.top + document.getElementById('btnShowbr').offsetHeight;
                div.style.left = xy.left - 422;
                div.style.width = 420;
                div.style.height = (dt.Rows.length + 3) * 23;
            }         
            div.style.display = 'block';
        }
        
        function close_pop_chgroup()
        {
            brgrid = null;
            ajax_show_status(false);
            document.getElementById('divPopChGroup').style.display = 'none';
            document.getElementById('spclosetip').style.visibility = 'hidden';
        }
        
        function select_chgroup_branch(args)
        {
            var sbr = brgrid.getValueAt(args.datarow, 0);
            close_pop_chgroup();
            document.getElementById('ddbranches').value = sbr;
            new_branch(sbr);
        }
        
        function show_chgroup_branches()
        {
            if (document.getElementById('divPopChGroup').style.display == 'block') 
            {
                close_pop_chgroup();
                document.getElementById('spclosetip').style.visibility = 'hidden';
                return;
            }
            if (document.getElementById('ddbrgroup'))
            {
                var i = document.getElementById('ddbrgroup').selectedIndex;
                if (i < 0) return;
                var chtxt = trim(document.getElementById('ddbrgroup').options[i].text);
                if (chtxt.length == 0 | chtxt == 'Select') return;
                document.getElementById('spclosetip').style.visibility = 'visible';
                IRSWorld.BranchSetup.Charges.getChargeGroupBranches(false, show_affected_branches)
            }
        }        

        
        function prod_btn_clicked(args)
        {            
            if (args.value == 'Edit')
            {
                set_read_only(document.getElementById('tblprodmast'), false);
                document.getElementById('ckbrLevel').focus();
                prodgrid.enableFooterBtns("Edit");
                document.getElementById('btnSearch').disabled = true;
                return;
            }
            
            if (args.value == 'Delete')
            {
                if (!prodgrid.hasDataRows()) 
                {
                    prod_set_empty(args);
                }
                document.getElementById('btnSearch').disabled = false;
            }
            
            if (args.value == 'Add')
            {
                if (document.getElementById('ddbranches').value == 0) 
                {
                    alert("A branch must be selected.");
                    prodgrid.footerButtonClick('', 'Cancel');
                    return;
                }
                prodgrid.row.onclick_enabled = false;
                prod_set_empty(args);
                set_read_only(document.getElementById('tblprodmast'), false);
                document.getElementById('ckbrLevel').focus();
                prodgrid.enableFooterBtns("Add");
                document.getElementById('btnSearch').disabled = true;
                return;
            }
            
            if (args.value == 'Save')
            {
                ajax_show_status(true);
                if (go_chgroup)
                {
                    IRSWorld.BranchSetup.Charges.getChargeGroupBranches(true, show_affected_branches)
                }
                IRSWorld.BranchSetup.Charges.applyEdits(go_chgroup, 'Product', args.additions, args.deletes, args.edits, prod_updates_saved);
            }
            
        }

        function prod_post_click(args)
        {
            if (document.getElementById('tblprodmast').has_changed)
            {
                set_read_only(document.getElementById('tblprodmast'), true);
                document.getElementById('tblprodmast').has_changed = false;
                prodgrid.row.onlick_enabled = true;
                
                document.getElementById('tblprodgrid_footerbtn_Save').disabled = false;
            }
            
            document.getElementById('btnSearch').disabled = true;
        }
        
        function prod_changes_valid()
        {
            var ddls = document.getElementById('tblprodmast').getElementsByTagName("SELECT");
            var ddl_errs = 0;
            for (var i = 0; i < ddls.length; i++)
            {
                if (ddls.item(i).value < 1) ddl_errs++;
            }
            if (ddl_errs > 0) return false;
            if (isNaN(parseInt(document.getElementById('txtcleardays').value)))
            {
                return false;
            }
            return true;
        }
        
        function set_prod_columns()
        {
            if (prodgrid.curract == 'Add') 
            {
                prodgrid.setColumn(0, 0);
            }
            
            prodgrid.setColumn(1, document.getElementById('ddbranches').value);
            prodgrid.setColumn(2, document.getElementById('ckbrlevel').checked);
            prodgrid.setColumn(4, parseInt(document.getElementById('ddimplem').value));
            prodgrid.setColumn(5, parseInt(document.getElementById('ddcurrs').value));
            prodgrid.setColumn(6, parseInt(document.getElementById('ddchtype').value));
            prodgrid.setColumn(7, parseInt(document.getElementById('ddpaymode').value));
            prodgrid.setColumn(9, parseInt(document.getElementById('dddefstat').value));            
            prodgrid.setColumn(10, parseInt(document.getElementById('txtcleardays').value)); 
            prodgrid.setColumn(11, document.getElementById('ddbrgroup').value);
            prodgrid.setColumn(12, document.getElementById('chkPromo').checked);
            if (document.getElementById('chkPromo').checked)
            {
                prodgrid.setColumn(13, document.getElementById('txtPopValidFrom').value);
                prodgrid.setColumn(14, document.getElementById('txtPopValidTo').value);
            }
            else
            {
                prodgrid.setColumn(13, '');
                prodgrid.setColumn(14, '');
            }
            prodgrid.setColumn(15, document.getElementById('chkChargesCurr').checked);
        }
        
        function get_correct_chgroup(br, cg)
        {
            if (br == null || br == '' || br == 'null') return cg;
            var rs = IRSWorld.BranchSetup.Charges.setChargeGroupByBrcode(br);
            if (rs.error != null)
            {
                return cg;
            }
            
            return rs.value;
        }
        
        function prod_set_fields(vals, nodtl)
        {
            if (vals)
            {
                document.getElementById('ddbranches').value = vals[1];
                document.getElementById('ckbrlevel').checked = vals[2];
                document.getElementById('ddimplem').value = vals[4];
                document.getElementById('ddcurrs').value = vals[5];
                document.getElementById('ddchtype').value = vals[6];
                document.getElementById('ddpaymode').value = vals[7];
                document.getElementById('dddefstat').value = vals[9];
                document.getElementById('txtcleardays').value = vals[10];
                document.getElementById('ddbrgroup').value = (isNaN(vals[11])) ? chgroup : get_correct_chgroup(vals[1], vals[11]);
                document.getElementById('chkPromo').checked = vals[12];
                if (document.getElementById('chkPromo').checked)
                {
                    var from = (vals[13] == null || vals[13].length == 0 || vals[13] == "null")? '' : dateToString(vals[13]).split(' ')[0];
                    var to = (vals[14] == null || vals[14].length == 0 || vals[14] == "null")? '' : dateToString(vals[14]).split(' ')[0];
                    if (from.length > 0 | to.length > 0)
                    {
                        setText(document.getElementById('spvalidity'), 'Validity: ' + from + ' - ' + to);
                    }
                }
                document.getElementById('chkChargesCurr').checked = vals[15];
                change_curr_lbl();
                
                if (!nodtl) 
                {
                    IRSWorld.BranchSetup.Charges.getChargesDetail(vals[0], charge_updates_saved);
                    IRSWorld.BranchSetup.Charges.getChargesUsers(vals[0], has_charges_users);
                }
                else
                {
                    ajax_show_status(false);
                }
            }
        }
        
        function prod_add_button_click(btn)
        {
            var fun = '';
            if (btn.value == 'Add')
            {
                save_add = true;
                save_pid = prodgrid.getValueAt(prodgrid.row.selected, 0);
                save_datarow = prodgrid.row.selected_data_row;
                save_gridrow = prodgrid.row.selected;
                fun = "prod_btn_clicked";
                
                var cap = document.getElementById('capprodmast');
                cap.innerText = '';

            }
            else
            {
                if (document.getElementById('tblprodmast').has_changed)
                {
                    fun = "prod_post_click";
                }
                else if (confirm("No changes has been made, cancel " + prodgrid.curract + " action?"))
                {
                    document.getElementById('tblprodgrid_footerbtn_Cancel').click();
                }
            }
            if (fun.length > 0) prodgrid.footerButtonClick(fun, btn.value);
        }
        
        function prod_cancel_click(args)
        {
            prodgrid.row.onclick_enabled = true;
            prod_set_fields(prodgrid.getValues(args.datarow), true);
            set_read_only(document.getElementById('tblprodmast'), true);
            document.getElementById('ddbranches').disabled = false;
            prodgrid.enableFooterBtns('SEL');
            document.getElementById('btnSearch').disabled = false;
            
            if (prodgrid.datatable.Rows.length == 0)
            {
                document.getElementById('txtcleardays').value = '0';
                var pobj = document.getElementById('tblprodmast');
                var ddls = pobj.getElementsByTagName('SELECT');
                for (var i = 0; i < ddls.length; i++)
                {
                    ddls.item(i).selectedIndex = 0;
                }               
            }
            else if (save_add)
            {
                save_add = false;
                
                var pgno = ((save_datarow + 1) > prodgrid.rows_page) ? parseInt((save_datarow + 1) / prodgrid.rows_page) : 0;
                prodgrid.pg_position(pgno + 1);
                
                prodgrid.rowSelected(save_datarow, save_gridrow);
            }
        }
        
        function prodgrid_prod_changed(args)
        {
            document.getElementById('tblprodmast').has_changed = true; 
        }
        
        function prod_value_change(obj, colidx)
        {
            var ok = true;
            if (obj.tagName == 'SELECT' && obj.selectedIndex == 0) 
            {
                alert("Please select an item.");
                ok = false;
            }
            else if (obj.id == 'chkPromo')
            {
                if (obj.checked)
                {
                    show_promo_period(obj);
                }
                else
                {
                    if (prodgrid.curract != '')
                    {
                        clear_validity_period();
                    }
                    btnpromo_close_clicked();
                }
            }
            else if (obj.id == 'txtcleardays')
            {
                if (isNaN(parseInt(obj.value)))
                {
                    alert("Clearing days must be numeric.");
                    ok = false;
                }
            }
            else if (obj.id == 'ckbrlevel')
            {
                document.getElementById('spbrlevel').style.visibility = (obj.checked) ? "visible" : "hidden";
            }
            if (ok)
            {
                document.getElementById('tblprodmast').has_changed = true; 
            }
            else
            {
                obj.value = prodgrid.getValueAt(prodgrid.row.selected, colidx);
                obj.focus();
            }                  
        }

        function prod_change_row(args)
        {
            try
            {
                var cap = document.getElementById('capprodmast');
                var seqid = "(" + prodgrid.getValueAt(args.datarow, 0) + ") ";
                var obj = prodgrid.getElementsAt(args.datarow, 3).item(0);
                var desc = obj.options[obj.selectedIndex].text;
                cap.innerText = seqid + desc;
            }
            catch (e) {}
            save_pid = prodgrid.getValueAt(args.datarow, 0);
            if (prodgrid.curract == '')
            {
                prod_set_fields(prodgrid.getValues(args.datarow));
            }
            document.getElementById('btnSearch').disabled = false;
        }
        
        function prod_before_post(args)
        {
            if (!document.getElementById('tblprodmast').has_changed)
            {
                alert("Posting with no changes made?");
                prodgrid.curract = '';
                return;
            }
            
            if (prodgrid.curract == 'Add' | prodgrid.curract == 'Edit')
            {
                var rec = 'BranchGroup=' + document.getElementById('ddbrgroup').value + '~';
                rec += 'BranchCode=' + document.getElementById('ddbranches').value + '~';
                rec += 'ProductCode=' + prodgrid.getValueAt(args.datarow, 3) + '~';
                rec += 'ImplementationType=' + document.getElementById('ddimplem').value + '~';
                rec += 'Currency=' + document.getElementById('ddcurrs').value + '~';
                rec += 'ChargeType=' + document.getElementById('ddchtype').value + '~';
                rec += 'PayMode=' + document.getElementById('ddpaymode').value; // + '~';
                
                if (prodgrid.curract == 'Edit')
                {
                    var seqid = prodgrid.getValueAt(args.datarow, 0);
                    rec += '~SEQ_ID=' + seqid;
                }
                
                var rs = IRSWorld.BranchSetup.Charges.rowExist(rec);
                if (rs.error != null)
                {
                    alert('Product before post: ' + rs.error.Message);
                    return;
                }
                if (rs.value == null || rs.value == true) 
                {
                    alert("Duplicate Row..");
                    prodgrid.curract = '';
                    return;
                }
            }
            if (prod_changes_valid())
            {
                set_prod_columns();
            }
            else
            {
                alert("Entries for the Product charges master has error");
                prodgrid.curract = '';
            }          
        }
        
        function show_promo_period(ck)
        {
            if (ck.checked & prodgrid.curract != '')
            {
                var from = prodgrid.getValueAt(prodgrid.row.selected_data_row, 13);
                var to = prodgrid.getValueAt(prodgrid.row.selected_data_row, 14);
                document.getElementById('txtPopValidFrom').value = (from == null || from.length == 0 || from == "null") ? "" : dateToString(from).split(' ')[0];
                document.getElementById('txtPopValidTo').value = (to == null || to.length == 0 || to == "null") ? "" : dateToString(to).split(' ')[0];
                
                var div = document.getElementById('divPromoDates');
                div.style.position = 'absolute';
                var xy = calcpos(ck);
                div.style.top = xy.top + ck.offsetHeight;
                div.style.left = xy.left + ck.offsetWidth;
                div.style.display = 'block';
            }
            
        }
        
        function clear_validity_period()
        {
            document.getElementById('spvalidity').innerText = '';
            document.getElementById('txtPopValidFrom').value = '';
            document.getElementById('txtPopValidTo').value = '';
        }
        
        function btnpromo_close_clicked()
        {
            clear_validity_period();
                        
            var div = document.getElementById('divPromoDates');
            div.style.display = "none";
        }
        
        function btnpromo_ok_clicked()
        {
            var is_date = isValidDate(document.getElementById('txtPopValidFrom').value);
            if (!is_date[0])
            {
                alert(is_date[1]);
                document.getElementById('txtPopValidFrom').focus();
                return;
            }
            var is_date = isValidDate(document.getElementById('txtPopValidTo').value);
            if (!is_date[0])
            {
                alert(is_date[1]);
                document.getElementById('txtPopValidTo').focus();
                return;
            }
            
            var from = document.getElementById('txtPopValidFrom').value;
            var to = document.getElementById('txtPopValidTo').value;
            if (from.length > 0 | to.length > 0)
            {
                document.getElementById('spvalidity').innerText = "Validity: " + from + " - " + to;
            }
            
            var div = document.getElementById('divPromoDates');
            div.style.display = "none";
        }
        
        function create_prod_grid(dt)
        {
            prodgrid = new datagrid();
             { //0
                var col = new datagrid_column();
	            col.header = 'ID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'right';
	            col.editable = false;
		        col.styles.cell += 'width:30%;text-align:right;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
	            prodgrid.addColumn(col);	
		    } 
             {//1
                var col = new datagrid_column();
	            col.header = 'BRCODE';
	            col.datafield = 'BranchCode';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }    		              
            {//2
                var col = new datagrid_column();
	            col.header = 'brl';
	            col.datafield = 'BranchLevel';
	            col.align = 'center';
	            col.type = 'check';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }
            {//3
                var col = new datagrid_column();
		        col.header = 'Products';
		        col.datafield = 'ProductCode';
		        col.align = 'left';
		        col.type = 'select';
		        col.options = prodopts;
		        col.onchange = 'prodgrid_prod_changed'
		        col.styles.cell += 'width:70%;text-align:left;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
	            prodgrid.addColumn(col);	
		    }
            {//4
                var col = new datagrid_column();
	            col.header = 'IT';
	            col.datafield = 'ImplementationType';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }
            {// 5
                var col = new datagrid_column();
	            col.header = 'Curr';
	            col.datafield = 'Currency';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }
            {//6
                var col = new datagrid_column();
	            col.header = 'Chtype';
	            col.datafield = 'ChargeType';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }
            {//7
                var col = new datagrid_column();
	            col.header = 'pmode';
	            col.datafield = 'PayMode';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }
            {//8
                var col = new datagrid_column();
	            col.header = 'compl';
	            col.datafield = 'ComputationLevel';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }
            {//9
                var col = new datagrid_column();
	            col.header = 'def';
	            col.datafield = 'DefaultStatus';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }		    
            {//10
                var col = new datagrid_column();
	            col.header = 'clear';
	            col.datafield = 'ClearingDays';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }	
             {//11
                var col = new datagrid_column();
	            col.header = 'BRGRP';
	            col.datafield = 'BranchGroup';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    } 	
             {//12
                var col = new datagrid_column();
	            col.header = 'PR';
	            col.datafield = 'Promotion';
	            col.type = 'check';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    } 
             {//13
                var col = new datagrid_column();
	            col.header = 'PRFrom';
	            col.datafield = 'ValidityFrom';
	            col.align = 'center';
	            col.onclick = 'show_promo_period';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    } 
             {//14
                var col = new datagrid_column();
	            col.header = 'PRTo';
	            col.datafield = 'ValidityTo';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    }
             {//15
                var col = new datagrid_column();
	            col.header = 'BC';
	            col.datafield = 'BeneCurrFlag';
	            col.type = 'check';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            prodgrid.addColumn(col);	
		    } 
		        			    		    
		    prodgrid.row.onclick = 'prod_change_row';
		    prodgrid.edit_button_prefix = 'tblprodgrid_footerbtn_Edit';
		    prodgrid.footer.edit_button.beforepost = 'prod_before_post';
		    prodgrid.footer.edit_button.onpost = 'prod_post_click';
		    prodgrid.add_button_prefix = 'tblprodgrid_footerbtn_Add';
		    prodgrid.footer.add_button.onclick = 'prod_btn_clicked';
		    prodgrid.footer.add_button.beforepost = 'prod_before_post';
		    prodgrid.footer.add_button.onpost = 'prod_post_click';
		    prodgrid.footer.save_button.onclick = 'prod_btn_clicked';
		    prodgrid.delete_button_prefix = 'tblprodgrid_footerbtn_Delete';
		    prodgrid.bind(dt, 'tdprodgrid', 'tblprodgrid');
		    if (dt.Rows.length > 0)
		    {
                prodgrid.rowSelected(0, 0, 'tblprodgrid_tr_0');
                btnsearch_clear_clicked();
            }
            else
            {
                document.getElementById('btnSearch').disabled = true;
                ajax_show_status(false);
            }
            
		}
		
		function value_formatter(val)
		{
            var fval = parseFloat(val);
            if (isNaN (fval))
            {
                fval = 0.0000;
            }
            return formatNumber(fval, 3);
		}
		
		function value_formatter6(val)
		{
            var fval = parseFloat(val);
            if (isNaN (fval))
            {
                fval = 0.000000;
            }
            return formatNumber(fval, 6);		
		}
		
		function charge_updates_saved(rs)
		{
            if (go_chgroup) close_pop_chgroup();
		    if (rs.error != null)
		    {
		        alert('Charge detail saved: ' + rs.error.Message);
                if (go_chgroup) close_pop_chgroup();
                else
                    ajax_show_status(false);
		        return;
		    }
		    
		    if (chargrid == null)
		    {
                create_charges_grid(rs.value);
		    }
		    else
		    {
		        chargrid.clearChanges();
		        chargrid.refresh(rs.value);
		        if (rs.value.Rows.length > 0)
		        {
		            var srow = (no_chpop) ? ch_pos : 0;
		            var scol = (no_chpop) ? ch_datrow : 0;
                    chargrid.rowSelected(srow, scol, 'tblchgrid_tr_0');
                    if (no_chpop)
                    {
                        no_chpop = false;
                        show_div_charges('tier');
                        ajax_show_status(false);
                        return;
                    }
                }		    
            }
		    
		    show_div_charges('charges');
            ajax_show_status(false);
		}
		
		function chargrid_save_clicked(args)
		{
            if (args.value == 'Save')
            {
                ajax_show_status(true);
                if (go_chgroup)  // & no_chpop == false)
                {
                    IRSWorld.BranchSetup.Charges.getChargeGroupBranches(true, show_affected_branches)
                }  
                document.getElementById('atier').disabled = false;
                IRSWorld.BranchSetup.Charges.applyEdits(go_chgroup, 'Charges', args.additions, args.deletes, args.edits, charge_updates_saved);
            }		
        }
        
        function chargrid_edit_clicked()
        {
            document.getElementById('atier').disabled = true;
        }
        
        function put_charge(dd, n, admod)
        {
            if (admod) chargrid.setColumn(2, dd.options[n].value);
            var txt = dd.options[n].text;
            var ckb = chargrid.getElementsAt(chargrid.row.selected, 9); // 8
            if (ckb)
            {
                ckb.item(0).checked = (txt == 'Service Fee' | txt == 'Out-of-pocket Expense' | txt == 'Documentary Stamps');
            }                    
        }
        
        function chargrid_add_clicked(args)
        {
            document.getElementById('atier').disabled = true;
            var drow = ((prodgrid.curr_page - 1) * prodgrid.rows_page) + prodgrid.row.selected;
            var pid = prodgrid.getValueAt(drow, 0);
            if (pid > 0)
            {
                chargrid.setColumn(1, pid);
                var dd = chargrid.getElementsAt(args.datarow, 2);
                if (dd == null) 
                {
                    alert("Unable to obtain the handle for charges dropdown.");
                    return;
                }
                var n = 0;
                while (n < (chargopts.length))
                {
                    if (hasCharge(dd.item(0).options[n].value)) 
                    {
                        n++;
                        continue;
                    }
                    else
                    {
                        put_charge(dd.item(0), n, true);                        
                        break;
                    }
                }
                if (n >= chargopts.length) 
                {
                    if (!confirm("All charges had already been defined. Continue?"))
                    {
                        chargrid.footerButtonClick('chargrid_cancel_clicked', 'Cancel');
                        chargrid.rowSelected(n-1, chargopts.length - 1);
                    }
                    n = chargopts.length - 1;
                    put_charge(dd.item(0), n, true);
                }
            }
            else
            {
                alert("ProductChargesID cannot be 0. Save product master first.")
            }
        }
        
        function tiergrid_save_clicked(args)
        {
            if (args.value == 'Save')
            {
                ajax_show_status(true);
                if (go_chgroup)
                {
                    IRSWorld.BranchSetup.Charges.getChargeGroupBranches(true, show_affected_branches)
                }
                check_min_amt = true;              
                IRSWorld.BranchSetup.Charges.applyEdits(go_chgroup, 'Tier', args.additions, args.deletes, args.edits, has_charges_tier);
            }		
        }
                
        function tiergrid_add_clicked(args)
        {
            if (tiergrid.dataRowsSize() > 0)
            {
                var to = parseFloat(tiergrid.getValueAt(0, 3));
                if (!isNaN(to) & to == 0)
                {
                    alert('Cannot add more rows. To amt terminated with 0 entry.');
                    tiergrid.footerButtonClick('', 'Cancel');
                    return;
                }
            }
            
            tiergrid.setColumn(1, chid);
            
            var min_amt = 0.0000;   
            if (chargrid.row.selected_data_row < 0) 
            {
                alert ("A Charge element must be selected");
            }         
            var ddl = chargrid.getElementsAt(chargrid.row.selected_data_row, 3).item(0);
            if (ddl)
            {
                var txt = ddl.options[ddl.selectedIndex].text.toLowerCase();
                if (txt.indexOf('% of gross') > -1 | txt.indexOf('% of net') > -1)
                {
                    min_amt = parseFloat(chargrid.getValueAt(chargrid.row.selected_data_row, 6)); //5
                    if (isNaN(min_amt)) min_amt = 0.0000;
                }
            }

            if (!tiergrid.hasDataRows()) tiergrid.setColumn(2, min_amt); //'1.000');
            else
            {
                var val = parseFloat(tiergrid.getDataAt(args.datarow - 1, 3));
                if (! isNaN(val))
                {
                    tiergrid.setColumn(2, val + 1);
                }
            }
        }
        
        function from_amt_change(args)
        {
            if (char_check_numeric(args))
            {
                var sn = value_formatter(args.newvalue);
                tiergrid.setColumn(2, sn);
            }
        }
               
        function to_amt_change(args)
        {
            var ctl = document.getElementById(args.control);
            if (char_check_numeric(args) & tiergrid.curract != '')
            {
                var from = parseFloat(tiergrid.getValueAt(args.datarow, 2));
                var to = parseFloat(args.newvalue);
                if (!isNaN(to) && (to > 0 && to <= from))
                {
                    alert('To value must be greater than from');
                    ctl.parentElement.focus();
                    ctl.select();
                    return;
                } 
                var sn = value_formatter(args.newvalue);
                tiergrid.setColumn(3, sn);
            }
        }
        
        function charge_amt_change(args)
        {
            if (char_check_numeric(args))
            {
                var sn = value_formatter6(args.newvalue);
                tiergrid.setColumn(4, sn);
            }            
        }
               
        function tiergrid_before_post(args)
        {
            var msg = "must contain positive value.";
            var n = parseFloat(tiergrid.getValueAt(args.datarow, 2));
            if (isNaN(n) || n <= 0)
            {
                alert("AmountFrom " + msg);
                tiergrid.curract = '';
                return;
            }
            n = parseFloat(tiergrid.getValueAt(args.datarow, 3));
            if (isNaN(n) || n < 0)
            {
                alert("AmountTo " + msg + " > AmountFrom.");
                tiergrid.curract = '';
                return;
            } 
            n = parseFloat(tiergrid.getValueAt(args.datarow, 4));
            if (isNaN(n) || n <= 0)
            {
                alert("ChargeAmt " + msg);
                tiergrid.curract = '';
                return;
            }                             
        }
        
        function create_tier_grid(dt)
        {
            tiergrid = new datagrid();
            {//0
                var col = new datagrid_column();
	            col.header = 'ID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            tiergrid.addColumn(col);	
		    } 
            {//1
                var col = new datagrid_column();
	            col.header = 'CID';
	            col.datafield = 'ChargesID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            tiergrid.addColumn(col);	
		    }
            {//2
		        var col = new datagrid_column();
		        col.header = 'Amt. From';
		        col.datafield = 'AmountFrom';
		        col.align = 'center';
		        col.styles.cell += 'width:50%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        col.formatter = 'value_formatter';
		        col.onblur = 'from_amt_change';
		        tiergrid.addColumn(col);
		    } 	
            {//3
		        var col = new datagrid_column();
		        col.header = 'Amt. To';
		        col.datafield = 'AmountTo';
		        col.align = 'center';
		        col.styles.cell += 'width:25%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        col.formatter = 'value_formatter';
		        col.onblur = 'to_amt_change';
		        tiergrid.addColumn(col);
		    }
            {//4
		        var col = new datagrid_column();
		        col.header = 'Charge Amt.';
		        col.datafield = 'ChargeAmount';
		        col.align = 'center';
		        col.styles.cell += 'width:25%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        col.formatter = 'value_formatter6';
		        col.onblur = 'charge_amt_change';
		        tiergrid.addColumn(col);
		    }

		    tiergrid.caption = scharge_name;	
            tiergrid.classNames.table = 'table_body';
            tiergrid.row.normal_style = 'background-color: #f7ffff;';  
            tiergrid.footer.edit_button.none = false;
            tiergrid.footer.edit_button.beforepost = 'tiergrid_before_post';
            tiergrid.footer.edit_button.style += 'background-color: lavender;'; 
            tiergrid.footer.add_button.none = false;
            tiergrid.footer.add_button.onclick = 'tiergrid_add_clicked';
            tiergrid.footer.add_button.beforepost = 'tiergrid_before_post';            
            tiergrid.footer.add_button.style += 'background-color: lavender;';
            tiergrid.footer.delete_button.none = false;
            tiergrid.footer.delete_button.style += 'background-color: lavender;';             
            tiergrid.footer.save_button.none = false;
            tiergrid.footer.save_button.style += 'background-color: lavender;'; 
            tiergrid.footer.save_button.onclick = 'tiergrid_save_clicked';
            tiergrid.footer.cancel_button.none = false;
            tiergrid.footer.cancel_button.style += 'background-color: lavender;'; 
            tiergrid.rows_page = 5;
            tiergrid.bind(dt, 'div_tier', 'tbltier');
		}
        
        function has_charges_tier(rs)
        {
            if (go_chgroup) close_pop_chgroup();
            if (rs.error != null)
            {
                alert( 'Charges tier: ' + rs.error.Message );
                document.getElementById('atier').disabled = true;
                document.getElementById('atier').href = '';
                return;
            }
            create_tier_grid(rs.value);
            ajax_show_status(false);
            if (check_min_amt)
            {
                check_min_amt = false;
                var ddl = chargrid.getElementsAt(chargrid.row.selected_data_row, 3).item(0);
                if (ddl)
                {
                    var txt = ddl.options[ddl.selectedIndex].text.toLowerCase();
                    if (txt.indexOf('% of gross') > -1 | txt.indexOf('% of net') > -1)
                    {
                        var tier_min_amt = parseFloat(tiergrid.getDataAt(0, 2));
                        var chg_min_amt = parseFloat(chargrid.getValueAt(chargrid.row.selected_data_row, 6)); //5
                        if (tier_min_amt != chg_min_amt)
                        {
                            chargrid.footerButtonClick('', 'Edit');
                            chargrid.setColumn(6, tier_min_amt); //5
                            chargrid.footerButtonClick('', 'Post');
                            no_chpop = true;
                            ch_pos = chargrid.row.selected;
                            ch_datrow = chargrid.row.selected_data_row;
                            chargrid.footerButtonClick('chargrid_save_clicked', 'Save');
                        }
                    }
                }
            }
        }
        
        function user_updates_saved(rs)
        {
            if (rs.error != null)
            {
                alert('User updates saved: ' + rs.error.Message);
                ajax_show_status(false);
                return;
            }
            
            create_users_grid(rs.value);
            ajax_show_status(false);
        }
        
        function over_updates_saved(rs)
        {
            if (rs.error != null)
            {
                alert('Override updates saved: ' + rs.error.Message);
                ajax_show_status(false);
                return;
            }
            
            create_over_grid(rs.value);
            ajax_show_status(false);
        }        
        
        function save_user_grid(args)
        {
            ajax_show_status(false)
            if (args.value == 'Save')
            {
                ajax_show_status(true);
                IRSWorld.BranchSetup.Charges.applyEdits(go_chgroup, 'Users', args.additions, args.deletes, args.edits, user_updates_saved);
            }        
        }
        
        function save_over_grid(args)
        {
            if (args.value == 'Save')
            {
                ajax_show_status(true);
                IRSWorld.BranchSetup.Charges.applyEdits(go_chgroup, 'Over', args.additions, args.deletes, args.edits, over_updates_saved);
            }        
        } 
              
        function create_permission_grid()
        {
            var grid = new datagrid();
            {
                var col = new datagrid_column();
	            col.header = 'ID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            grid.addColumn(col);	
		    } 
            {
                var col = new datagrid_column();
	            col.header = 'CID';
	            col.datafield = 'ProductChargesID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            grid.addColumn(col);	
		    }
            {
                var col = new datagrid_column();
	            col.header = 'UType';
	            col.datafield = 'UserType';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            grid.addColumn(col);	
		    }		    
            {
                var col = new datagrid_column();
	            col.header = 'RoleCode';
	            col.datafield = 'Role';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            grid.addColumn(col);	
		    }	
		    {
		        var col = new datagrid_column();
		        col.header = 'Allowed';
		        col.datafield = 'Allowed';
		        col.align = 'center';
		        col.type = 'check';
		        col.styles.cell += 'width:10%;padding-right:3px;';
		        grid.addColumn(col);
		    }		    	
            {
		        var col = new datagrid_column();
		        col.header = 'Role';
		        col.datafield = 'RoleDesc';
		        col.align = 'left';
		        col.styles.cell += 'width:90%;padding-right:3px;';
		        grid.addColumn(col);
		    } 	
            grid.classNames.table = 'table_body';
            grid.row.normal_style = 'background-color: #f7ffff;';  
            grid.footer.edit_button.none = false;
            grid.footer.edit_button.style += 'background-color: lavender;'; 
            grid.footer.save_button.none = false;
            grid.footer.save_button.style += 'background-color: lavender;'; 
            grid.footer.cancel_button.none = false;
            grid.footer.cancel_button.style += 'background-color: lavender;'; 
            grid.rows_page = 5;
            
            return grid;        
        }
        
        function create_users_grid(dtuser)
        {
            var user_grid = create_permission_grid();
            user_grid.caption = "Allowed Users";
            user_grid.footer.save_button.onclick = 'save_user_grid';
            user_grid.bind(dtuser, 'tdusergrid', 'tblusergrid');
        }
        
        function create_over_grid(dtover)
        {
            var over_grid = create_permission_grid();
            over_grid.caption = "Override Permissions";
            over_grid.footer.save_button.onclick = 'save_over_grid';
            over_grid.bind(dtover, 'tdovergrid', 'tblovergrid');
        }
        
        function has_charges_users(rs)
        {
            var auser = document.getElementById('ausers');
            if (rs.error != null)
            {
                alert( 'Charges users: ' + rs.error.Message );
                auser.disabled = true;
                auser.href = '';
                return;
            }
            
            var dt = rs.value[0];
            var msg = rs.value[1];
            
            if (auser.disabled)
            {
                auser.disabled = false;
                auser.href="javascript:show_div_charges('users');";
            }            
            
            create_users_grid(dt[0]);
            create_over_grid(dt[1]);
        }
        
        var scharge_name = '';
        var chid;
        function chargrid_row_selected(args)
        {
            if (chargrid.hasDataRows())
            {
                chid = chargrid.getValueAt(args.datarow, 0);
                if (chid == null || chid.length == 0) chid = "-1";
                var kids = chargrid.getElementsAt(args.datarow, 2);
                try
                {
                    var sel = kids.item(0);
                    scharge_name = sel.options[sel.selectedIndex].text;
                }
                catch (e)
                {
                    //alert ("Unable to get charge name: " + e.description);
                    alert("Please activate charge " + chid + " in branch lookup.");
                }
                IRSWorld.BranchSetup.Charges.getChargesTier(chid, has_charges_tier);
            }
        }
        
        function chargrid_cancel_clicked(args)
        {
            document.getElementById('atier').disabled = false;
        }
        
        function char_check_numeric(args)
        {
            var ctl = document.getElementById(args.control);
            if (!isNumeric(args.newvalue)) 
            {
                alert('The value entered is not a valid number');
                ctl.parentElement.focus();
                ctl.select();
                return false; 
            }
            
            if (parseFloat(args.newvalue) < 0)  
            {
                alert('Negative value is not allowed.');
                ctl.parentElement.focus();
                ctl.select();
                return false;
            }
            
            return true;      
        }
        
        function charge_value_change(args)
        {
            var ddl = chargrid.getElementsAt(chargrid.row.selected_data_row, 3).item(0);
            var txt = ddl.options[ddl.selectedIndex].text.toLowerCase();
            if (txt.indexOf('tier') > -1)
            {
                chargrid.setColumn(5, value_formatter(0)); //4
                return;
            }

            if (char_check_numeric(args))
            {
                chargrid.setColumn(5, value_formatter(args.newvalue)); //4
            }
        }
        
        function min_amt_change(args)
        {
            var ddl = chargrid.getElementsAt(chargrid.row.selected_data_row, 3).item(0);
            var txt = ddl.options[ddl.selectedIndex].text.toLowerCase();

            if (txt.indexOf('tier') > -1)
            {
                if (txt.indexOf('% of gross') > -1 | txt.indexOf('% of net') > -1) {}
                else
                {
                    chargrid.setColumn(6, value_formatter(0)); //5
                    return;
                }
            }

            if (char_check_numeric(args))
            {
                chargrid.setColumn(6, value_formatter(args.newvalue)); //5
            }
        }
        
        function deno_amt_change(args)
        {
            var ddl = chargrid.getElementsAt(chargrid.row.selected_data_row, 3).item(0);
            var txt = ddl.options[ddl.selectedIndex].text.toLowerCase();
            if (txt.indexOf('tier') > -1)
            {
                chargrid.setColumn(7, value_formatter(0)); //6
                return;
            }
        
            if (char_check_numeric(args))
            {
                chargrid.setColumn(7, value_formatter(args.newvalue)); //6
            }
        } 
        
        function hasChargeType(args)
        {
            var cname = chargrid.getElementsAt(args.gridrow, 2).item(0).value;
            var ctype = chargrid.getElementsAt(args.gridrow, 3).item(0).value;
            var exist = false;
            for (var i = 0; i < chargrid.getDataRowsCount(); i++)
            {
                var nval = chargrid.getDataAt(i, 2);
                var tval = chargrid.getDataAt(i, 3);
                if (nval == cname & tval == ctype)
                {
                    exist = true;
                    break;
                }
            }
            
            return exist;
        }
                
        function chargrid_add_beforepost(args)
        {
            if (hasChargeType(args))
            {
                alert('A Charge with same name and type exist.');
                chargrid.curract = '';
                return;
            }
            
            var val = chargrid.getValueAt(args.gridrow, 5); //4
            if (val == null || val.length == 0)
            {
                chargrid.setColumn(5, 0.000); //4
            }
            
            val = chargrid.getValueAt(args.gridrow, 6); //5
            if (val == null || val.length == 0)
            {
                chargrid.setColumn(6, 0.000); // 5
            } 
            
            val = chargrid.getValueAt(args.gridrow, 7); //6
            var valn = parseFloat(val);
            var ddl = chargrid.getElementsAt(args.gridrow, 3).item(0);
            var pertxt = ddl.options[ddl.selectedIndex].text.toLowerCase();
            if (isNaN(valn) | (valn <= 0.000) && (pertxt== 'per amount' | pertxt == 'per usd amount'))
            {
                alert('Denomination must have a positive value when computation type is Per Amount or Per USD Amount');
                chargrid.curract = '';
                return;
            }
            if (val == null || val.length == 0)
            {
                chargrid.setColumn(7, 0.000); //6
            }                    
        }  
        
        function hasCharge(val)
        {
            var exist = false;
            for (var i = 0; i < chargrid.getDataRowsCount(); i++)
            {
                var gval = chargrid.getDataAt(i, 2);
                if (gval == val)
                {
                    exist = true;
                    break;
                }
            } 
            
            return exist;       
        }
        
        function chargrid_chargename_change(args)
        {
            var obj = document.getElementById(args.control);
            var n = parseInt(args.newvalue);
            if (hasCharge(args.newvalue))
            {
                if (!confirm("Charge exist. Continue?"))
                {
                    n++;
                    if (n == 3) n++;
                    if (obj)
                    {
                        while (n < obj.options.length)
                        {
                            if (hasCharge(n))
                            {
                                n++;
                                if (n == 3) n++;
                                continue;
                            }
                            else
                            {
                                obj.value = n;
                                break;
                            }
                        }
                        if (n >= obj.options.length)
                        {
                            chargrid.footerButtonClick('chargrid_cancel_clicked', 'Cancel');
                            return;
                        }
                    }
                }
            }
            
            n = (n > 2) ? n - 2 : n - 1;
            put_charge(obj, n, false);
            var txt = obj.options[n].text;
            var ckb = chargrid.getElementsAt(chargrid.row.selected, 9); //8
            if (ckb)
            {
                ckb.item(0).checked = (txt == 'Service Fee' | txt == 'Out-of-pocket Expense' | txt == 'Documentary Stamps');
            }                    
            
        }
        
        function check_charge_changed_done()
        {
            event.srcElement.detachEvent('onchange');
            event.srcElement.attachEvent(check_charge_changed);
        }
        
        
        function check_charge_changed()
        {
            var ddl = chargrid.getElementsAt(chargrid.row.selected, 2).item(0);
            if (ddl)
            {
                var val = ddl.value;
                var txt = ddl.options[ddl.selectedIndex].text;
                if (txt)
                {
                    var both = (txt.indexOf('Commission') > -1 | txt.indexOf('Cable') > -1);
                    if (both)
                    {
                        var srcid = (event.srcElement.id.indexOf('ChargeLocally') > -1) ? 'ChargeLocally' : 'ChargeToBenef';
                        var tgtid = (event.srcElement.id.indexOf('ChargeLocally') > -1) ? 'ChargeToBenef' : 'ChargeLocally';
                        var cbid = event.srcElement.id.replace(srcid, tgtid);
                        var cbmod = document.getElementById(cbid);
                        if (cbmod)
                        {
                            cbmod.checked = event.srcElement.checked;
                        }
                    }
                    else if (event.srcElement.id.indexOf('ChargeToBenef') > -1 && event.srcElement.checked)
                    {
                        var cblocid = event.srcElement.id.replace('ChargeToBenef', 'ChargeLocally');
                        var cbloc = document.getElementById(cblocid);
                        if (cbloc && cbloc.checked == false)
                        {
                            alert('Charge Locally must be checked!');
                            event.srcElement.checked = false;
                            return;
                        }
                    }
                    else if (event.srcElement.id.indexOf('ChargeLocally') > -1 && event.srcElement.checked == false)
                    {
                        var cbbenid = event.srcElement.id.replace('ChargeLocally', 'ChargeToBenef');
                        var cbben = document.getElementById(cbbenid);
                        if (cbben && cbben.checked)
                        {
                            cbben.checked = false;
                        }
                    }                    
                }
            }
        }
        
        function chargrid_comptype_change(args)
        {
            if (hasChargeType(args))
            {
                alert("A Charge row with same name and type exist.");
                return;
            }
            
            var ddl = document.getElementById(args.control);
            if (ddl)
            {
                var txt = ddl.options[ddl.selectedIndex].text.toLowerCase();
                if (txt && txt.indexOf('tier') > -1)
                {
                    if (confirm('Any entries in Amount & Denominator fields will be Cleared, Posted and Saved, continue?'))
                    {
                        try
                        {
                            for (var i = 0; i < 3; i++)
                            {
                                if ((txt.indexOf('% of gross') > -1 | txt.indexOf('% of net') > -1) & i == 1) continue;
                                var val = parseFloat(chargrid.getValueAt(args.gridrow, 5 + i)); // 4
                                if (!isNaN(val) || val > 0)
                                {
                                    chargrid.setColumn(5 + i, ''); // 4
                                }
                            }
                            if (chargrid.curract != '') 
                            {
                                chargrid.footerButtonClick('', 'Post');
                                no_chpop = true;
                                ch_pos = chargrid.row.selected;
                                ch_datrow = chargrid.row.selected_data_row;
                                chargrid.footerButtonClick('chargrid_save_clicked', 'Save');
                            }
                        }
                        catch (e)
                        {}
                    }
                }
            }
        }
               		
        function create_charges_grid(dt)
        {
            chargrid = new datagrid();
            {//0
                var col = new datagrid_column();
	            col.header = 'ID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            chargrid.addColumn(col);	
		    }
            {//1
                var col = new datagrid_column();
	            col.header = 'PID';
	            col.datafield = 'ProductChargesID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            chargrid.addColumn(col);	
		    }		    
            {//2
                var col = new datagrid_column();
		        col.header = 'Charge';
		        col.datafield = 'ChargeName';
		        col.align = 'left';
		        col.type = 'select';
		        col.options = chargopts;
		        col.onchange = 'chargrid_chargename_change';
		        col.styles.cell += 'width:25%;text-align:left;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
	            chargrid.addColumn(col);	
		    }
            {//3
                var col = new datagrid_column();
		        col.header = 'Comp. Type';
		        col.datafield = 'ComputationType';
		        col.align = 'left';
		        col.type = 'select';
		        col.onchange = 'chargrid_comptype_change';
		        col.options = comptypes;
		        col.styles.cell += 'width:30%;text-align:left;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
	            chargrid.addColumn(col);	
		    }	
            {//4
                var col = new datagrid_column();
		        col.header = 'Currency';
		        col.datafield = 'Currency';
		        col.align = 'left';
		        col.type = 'select';
		        col.options = curropts;
		        col.styles.cell += 'width:10%;text-align:left;';
		        col.styles.datafield += 'width: 100%; border: solid 1px navy;';
	            chargrid.addColumn(col);	
		    }			    	    
            {//4 -- 5
		        var col = new datagrid_column();
		        col.header = 'Value';
		        col.datafield = 'ChargeValue';
		        col.align = 'center';
		        col.styles.cell += 'width:11%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        col.formatter = 'value_formatter';
		        col.onblur = 'charge_value_change';
		        chargrid.addColumn(col);
		    }     
            {//5 -- 6
		        var col = new datagrid_column();
		        col.header = 'Min. Amount';
		        col.datafield = 'MinimumAmount';
		        col.align = 'center';
		        col.styles.cell += 'width:9%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        col.formatter = 'value_formatter';
		        col.onblur = 'min_amt_change';
		        chargrid.addColumn(col);
		    } 
		    {//6 -- 7
		        var col = new datagrid_column();
		        col.header = 'Deno minator';
		        col.datafield = 'Denominator';
		        col.align = 'center';
		        col.styles.cell += 'width:9%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        col.formatter = 'value_formatter';
		        col.onblur = 'deno_amt_change';
		        chargrid.addColumn(col);
		    }	
		    {//7 -- 8
		        var col = new datagrid_column();
		        col.header = 'Charge Benef';
		        col.datafield = 'ChargeToBenef';
		        col.align = 'center';
		        col.type = 'check';
		        col.styles.cell += 'width:3%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        col.onclick = 'check_charge_changed';
		        chargrid.addColumn(col);
		    }
		    {//8 -- 9
		        var col = new datagrid_column();
		        col.header = 'Charge Locally';
		        col.datafield = 'ChargeLocally';
		        col.align = 'center';
		        col.type = 'check';
		        col.onclick = 'check_charge_changed';
		        col.styles.cell += 'width:3%;padding-right:3px;';
		        col.styles.datafield += 'text-align:right;';
		        chargrid.addColumn(col);
		    }
		    chargrid.row.onclick = 'chargrid_row_selected';	    			    	        		     		    
            chargrid.classNames.table = 'table_body';
            chargrid.row.normal_style = 'background-color: #f7ffff;';  
            chargrid.footer.edit_button.none = false;
            chargrid.footer.edit_button.style += 'background-color: lavender;'; 
            chargrid.footer.edit_button.onclick = 'chargrid_edit_clicked';
            chargrid.footer.edit_button.beforepost = 'chargrid_add_beforepost';
            chargrid.footer.add_button.none = false;
            chargrid.footer.add_button.style += 'background-color: lavender;';
            chargrid.footer.add_button.onclick = 'chargrid_add_clicked'; 
            chargrid.footer.add_button.beforepost = 'chargrid_add_beforepost';
            chargrid.footer.delete_button.none = false;
            chargrid.footer.delete_button.style += 'background-color: lavender;';             
            chargrid.footer.save_button.none = false;
            chargrid.footer.save_button.style += 'background-color: lavender;'; 
            chargrid.footer.save_button.onclick = 'chargrid_save_clicked';
            chargrid.footer.cancel_button.none = false;
            chargrid.footer.cancel_button.style += 'background-color: lavender;'; 
            chargrid.footer.cancel_button.onclick = 'chargrid_cancel_clicked';
            chargrid.bind(dt, 'div_charges', 'tblchgrid');
		    if (dt.Rows.length > 0)
		    {
                chargrid.rowSelected(0, 0, 'tblchgrid_tr_0');
            }
            try
            {
                chargrid.rows_page = chargopts.length;
            }
            catch(e) {}
            
        }	
        
        function select_tab(sufx)
        {
            var anchs = document.getElementById('tbltabs').getElementsByTagName('td');
            var divs = document.getElementById('tdchgrid').getElementsByTagName('div');
            var aid = 'a' + sufx;
            for (var i = 0; i < anchs.length; i++)
            {
                anchs.item(i).className = (anchs.item(i).id == aid) ? 'tab_selected' : 'tab_unselected';
                divs.item(i).style.display = (anchs.item(i).className == 'tab_selected') ? 'block' : 'none';
            }
        }
        
        function show_div_charges(sufx)
        {
            if ((sufx == 'tier' | sufx == 'users') & (scharge_name == null || scharge_name == ''))
            {
                alert ("Please select a Charges row.");
                return;
            }
            
            if (sufx == 'tier')
            {
                try
                {
                    var ddl = chargrid.getElementsAt(chargrid.row.selected_data_row, 3).item(0);
                    var txt = ddl.options[ddl.selectedIndex].text.toLowerCase();
                    if (txt.indexOf('tier') == -1)
                    {
                        alert('Computation Type is not tierred');
                        return;
                    }
                }
                catch (e)
                {
                    alert('A Charges row must be selected');
                    select_tab('charges');
                    return;
                }
            }
            
            
            select_tab(sufx);
        }
        
        function has_setup_data(rs)
        {
            if (rs.error != null) 
            {
                spawn_errpage('Page setup: ' + rs.error.Message);
                ajax_show_status(false);
                return;
            }
            
            selbr = rs.value[0];
            issa = rs.value[1];
            init_ddl('ddbranches', rs.value[2].Tables[0], 'BranchCode', 'BranchName', selbr);   // branches
            init_ddl('ddbrgroup', rs.value[2].Tables[1], 'fldValue', 'fldDescription', 0);     //chgroup);
            
            
            document.getElementById('ckbrlevel').checked = false;
            document.getElementById('chkPromo').checked = false;
            document.getElementById('chkChargesCurr').checked = false;
            setText(document.getElementById('spvalidity'), '');
            document.getElementById('divBody').style.display = 'block';
            set_read_only(document.getElementById('tblprodmast'), true);
            document.getElementById('tblprodmast').has_changed = false;
            
            if (!issa)
            {
                document.getElementById('ddbranches').disabled = true;
                document.getElementById('ddbrgroup').disabled = true;
            }
            new_branch(selbr);
            
        }
        
        function change_curr_lbl()
        {
            var ckobj = document.getElementById('chkChargesCurr');
            var lbl = document.getElementById('lblChargesCurr');
            setText(lbl, ( (ckobj.checked) ? 'Account Currency' : 'Remitting Currency'))
        }
                
        function fixht()
        {
            document.getElementById('divBody').style.height = top.document.getElementById('divCenter').offsetHeight - 30;
            document.getElementById('divBody').style.width = top.document.getElementById('divCenter').offsetWidth;
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
                document.getElementById('divBody').style.overflow = 'auto';
                document.getElementById('tdContent').style.verticalAlign = 'top';
                
                fixht();
            }        
        }
        
        function has_group_charges(rs)
        {
            if (rs.error != null)
            {
                alert('Group Charges: ' + rs.error.Message);
                return;
            }
            
            var ddbr = document.getElementById('ddbranches');
            ddbr.selectedIndex = 0;
            
            create_prod_grid(rs.value);
        }	
        
        function has_charges(rs)
        {
            if (rs.error != null)
            {
                alert('Charges Detail: ' + rs.error.Message);
                return;
            }
            
            create_prod_grid(rs.value);
        }
                
        function page_setup()
        {
            if (top == self)
            {
                window.open('./BranchSetupMain.aspx?load=Charges.aspx', '_top');
            }
            else if (top.isAuthenticated())
            {
                AjaxPro.timeoutPeriod = ajax_timeout; 
                if (isIE)
                {
                    document.getElementById('chkChargesCurr').onpropertychange = change_curr_lbl;
                }
                else
                {
                    document.getElementById('chkChargesCurr').addEventListener("DOMAttrModified", change_curr_lbl, false);
                    document.getElementById('chkChargesCurr').onchange = change_curr_lbl;
                }
                
                fixmain();
                
                var x = '';
                check_min_amt = false;
                selbr = '';
                chgroup = 0;
                ajax_show_status(true);
                IRSWorld.BranchSetup.Charges.getSetupData(has_setup_data);
            }
            else
            {
                spawn_errpage("This module requires login!");
            }      
        }
        
        function clear_page()
        {
             if (chargrid != null) 
            {
                chargrid.removeTable();
                chargrid = null;
            }
            if (prodgrid != null)
            {
                prodgrid.removeTable();
                prodgrid = null;
            }   
            
            document.getElementById('ckbrlevel').checked = false;
            document.getElementById('ddimplem').selectedIndex = 0;
            document.getElementById('ddcurrs').selectedIndex = 0;
            document.getElementById('ddchtype').selectedIndex = 0;
            document.getElementById('ddpaymode').selectedIndex = 0;
            document.getElementById('dddefstat').selectedIndex = 0;
            document.getElementById('txtcleardays').value = 0; 
            document.getElementById('chkPromo').checked = false;
            setText(document.getElementById('spvalidity'), ''); 
            setText(document.getElementById('capprodmast'), 'Product Name');
        }
        
        function new_ddl_data(rs)
        {
            if (rs.value[0].Tables.length < 7)
            {
                spawn_errpage('Charges - Some of the initialization data is missing.');
                return;
            }
            if (rs.value.length > 3)
            {
                prodopts = rs.value[1];
                chargopts = rs.value[2];
                comptypes = rs.value[3];
                curropts = rs.value[4];
            }
            else
            {
                spawn_errpage('Charges - Some of the initialization data is missing.');
                return;
            }
            
            init_ddl('ddcurrs', rs.value[0].Tables[0], 'fldValue', 'fldDescription', 0);        // Currency
            init_ddl('ddpaymode', rs.value[0].Tables[1], 'fldValue', 'fldDescription', 0);      // PayMode
            init_ddl('ddchtype', rs.value[0].Tables[2], 'fldValue', 'fldDescription', 0);       // ChargeType
            init_ddl('dddefstat', filter_defstat(rs.value[0].Tables[3]), 'fldValue', 'fldDescription', 0);      // TranxStatus
            init_ddl('ddimplem', rs.value[0].Tables[4], 'fldValue', 'fldDescription', 0);       // implementation
            
            if (document.getElementById('ddbranches').selectedIndex != 0)
            {
                var rs = IRSWorld.BranchSetup.Charges.setChargeGroupByBrcode(selbr);
                if (rs.error != null)
                {
                    alert("Get Charge Group: " + rs.error.Message);
                    return;
                }
                clear_page();
                chgroup = rs.value;
                document.getElementById('btnGoChgroup').disabled = isNaN(chgroup);
                document.getElementById('ddbrgroup').value = chgroup;
                go_chgroup = false;
                IRSWorld.BranchSetup.Charges.getChargeMaster(selbr, has_charges);
            }
            else
            {
                var cg = document.getElementById('ddbrgroup').value;
                clear_page();
                new_branch_group(cg);
            }   
            
            ajax_show_status(false);
        }
        
        function new_branch(brcode)
        {            
            if (prodgrid != null && prodgrid.hasChanges())
            {
                alert("Product grid changes must be saved first.");
                document.getElementById('ddbranches').value = selbr;
                return;
            }
            
            if (chargrid != null && chargrid.hasChanges())
            {
                alert("Charges grid changes must be saved first.");
                document.getElementById('ddbranches').value = selbr;
                return;
            }
        
            ajax_show_status(true);
            selbr = brcode;
            IRSWorld.BranchSetup.Charges.getDdlData(brcode, new_ddl_data);
        } 
        
        function goto_branch_group()
        {
            if (!isNaN(chgroup) && chgroup > 0) new_branch_group(chgroup);
        }
        
        function new_branch_group(cg)
        {
            if (prodgrid != null && prodgrid.curract != null  && prodgrid.curract != '') 
            {
                alert("Product grid is in edit mode, must be saved or cancelled first.");
                document.getElementById('ddbrgroup').value = chgroup;
                return;
            }
            
            if (prodgrid != null && prodgrid.hasChanges())
            {
                alert("Product grid changes must be saved first.");
                document.getElementById('ddbrgroup').value = chgroup;
                return;
            }
            
            if (chargrid != null && chargrid.hasChanges())
            {
                alert("Charges grid changes must be saved first.");
                document.getElementById('ddbrgroup').value = chgroup;
                return;
            }
            
            clear_page();
            document.getElementById('ddbrgroup').value = cg;
            IRSWorld.BranchSetup.Charges.setChargeGroup(parseInt(cg));
            chgroup = cg;
            if (cg == 0) 
            {
                document.getElementById('ddbranches').value = 0;
                go_chgroup = false;
                return;
            }
            
            ajax_show_status(true);
            go_chgroup = true;
            IRSWorld.BranchSetup.Charges.getChargeMasterGroup(chgroup, has_group_charges);
        }
        
        function close_session()
        {
            if (prodgrid != null && prodgrid.hasChanges())
            {
                if (confirm("The Charge Master had changed, save changes?"))
                {
                    prodgrid.footerButtonClick('prod_btn_clicked', 'Save');
                }
            }            
            
            if (chargrid != null && chargrid.hasChanges())
            {
                if (confirm("Charges had changed, save changes?"))
                {
                    chargrid.footerButtonClick('chargrid_save_clicked', 'Save');
                }
            }
            
            if (tiergrid != null && tiergrid.hasChanges())
            {
                if (confirm("The Tiers data had changed, save changes?"))
                {
                    tiergrid.footerButtonClick('tiergrid_save_clicked', 'Save');
                }
            }            
                       
            if (is_closing())
            {
                IRSWorld.BranchSetup.Charges.close_session();
            }
            else
            {
                IRSWorld.BranchSetup.Charges.drop_charges();
            }
        }