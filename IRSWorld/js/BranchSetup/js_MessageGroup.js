        var ajax_timeout = 60 * 1000;
    
        var grpgrid;
        var popgrid;
        var memtype_opts;
        var issa;
        
        function hide_memtype_combos(hide)
        {
            for (var i = 0; i < grpgrid.datagrids.length; i++)
            {
                var memgrid = grpgrid.datagrids[i];
                var tbl = memgrid.getTable();
                if (tbl)
                {
                    var sels = tbl.getElementsByTagName('select');
                    for (var j = 0; j < sels.length; j++)
                    {
                        sels.item(j).style.visibility = (hide) ? 'hidden' : 'visible';
                    }
                }
            }
        }
        
        function reset_search(line_no)
        {
            document.getElementById('btnReset').disabled = (line_no == 0);
            document.getElementById('txt_search_point').value = '' + line_no;  
        }
        
        function pop_item_selected(args)
        {
            var popid = popgrid.getValueAt(args.datarow, 0);
            var popdesc = popgrid.getValueAt(args.datarow, 1);
            set_member_idname(popid, popdesc);
            close_popup();                       
        }
        
        function btn_find_click()
        {
            var val = document.getElementById('txtfind').value;
            if (val.length == 0) 
            {
                alert('Find needs search criteria.');
                return;
            }            
            
            var title = document.getElementById('spPopTitle').innerText;
            
            var col1 = (title.indexOf("USER") > -1) ? "UserID" : "BranchCode";
            var col2 = (title.indexOf("USER") > -1) ? "FullName" : "BranchName";
            
            var find_args = new datagrid_find_arguments();
            var farg = new datagrid_find_argument();
            farg.column_name = col1;
            farg.operator = "search";
            farg.value = val;
            find_args.addArgument(farg);
            find_args.addConjunction('|');
            farg = new datagrid_find_argument();
            farg.column_name = col2;
            farg.operator = "search";
            farg.value = val;
            find_args.addArgument(farg);
            find_args.select_row = false;
            find_args.start_row = parseInt(document.getElementById('txt_search_point').value);
            var locrow = popgrid.find(find_args, true);
            
            if (locrow == -1)
            {
                alert("No row found.");
                reset_search(0);
            }
            else
            {
                reset_search( (locrow == (popgrid.dataRowsSize() - 1)) ? 0 : locrow + 1 );
            }
            
            find_close_popup();
        }
        
        function show_popup_find(args)
        {
            var div = document.getElementById('divPopFind');
            var td = document.getElementById('tdPopFind');
            var topsize = top.document.getElementById('divCenter').offsetHeight;
            var topparent = document.getElementById('divBody').offsetHeight;
            div.style.height = ((topsize > topparent) ? topsize : topparent + 30) + 'px';
            div.className = 'div_undock';
            td.style.height = div.offsetHeight + 'px'; 
            td.style.width = div.offsetWidth + 'px'; 
            td.style.paddingTop = ((parseFloat(div.style.height) / 2) - ((23*6) / 2)) + 'px';
            div.style.display = 'block';
            var line_no = parseInt(document.getElementById('txt_search_point').value);
            reset_search(line_no); 
        }
        
        function find_close_popup()
        {
            document.getElementById('divPopFind').style.display = 'none';
        }
        
        function create_popgrid(dt, poptype)
        {
            var div = document.getElementById('divPopup');
            var td = document.getElementById('tdPopup');
            var topsize = top.document.getElementById('divCenter').offsetHeight;
            var topparent = document.getElementById('divBody').offsetHeight;
            div.style.height = (topsize > topparent) ? topsize : topparent + 30;
            div.className = 'div_undock';
            td.style.height = div.offsetHeight; 
            td.style.width = div.offsetWidth; 
            td.style.paddingTop = ((parseFloat(div.style.height) / 2) - ((23*14) / 2)) + 'px';
            div.style.display = 'block';        
        
            popgrid = new datagrid();
            {
                var col = new datagrid_column();
	            col.header = (poptype == 'User') ? 'USER ID' : 'BRANCH CODE';
	            col.datafield = (poptype == 'User') ? 'UserID' : (poptype == 'Branches') ? 'BranchCode' : 'MsgGroupName';
	            col.align = 'left';
	            col.styles.cell += 'width:20%;';
                popgrid.addColumn(col);	
	        } 
            {
                var col = new datagrid_column();
	            col.header = 'DESCRIPTION';
	            col.datafield = (poptype == 'User') ? 'FullName' : (poptype == 'Branches') ? 'BranchName' : 'MsgGroupName';
	            col.align = 'left';
	            col.styles.cell += 'width:80%;';
                popgrid.addColumn(col);	
	        }
	        document.getElementById('spPopTitle').innerText = (poptype == 'User') ? 'USER LIST' : (poptype == 'BRANCHES') ? 'BRANCH TABLE' : 'MESSAGE GROUPS';
            popgrid.classNames.table = 'table_body';
            popgrid.row.normal_style = 'background-color: #f7ffff;';  
            popgrid.rows_page = 12;
            popgrid.row.onclick = 'pop_item_selected';
            popgrid.footer.find_button.none = false;
            popgrid.footer.find_button.onclick = 'show_popup_find';
            popgrid.bind(dt, 'tdpopgrid', 'tblpopgrid'); 
            
            hide_memtype_combos(true);
            document.getElementById(popgrid.id + '_footerbtn_Find').disabled = false;
        }
        
        function close_popup()
        {
            document.getElementById('tdpopgrid').removeChild(document.getElementById('tblpopgrid'));
            document.getElementById('divPopup').style.display = 'none';
            hide_memtype_combos(false);
        }
        
        function set_member_idname(memid, memname)
        {
            var memgrid = grpgrid.datagrids[grpgrid.row.selected];
            memgrid.setColumn(3, memid);
            memgrid.setColumn(5, memname);        
        }

        function do_memtype_all()
        {
            set_member_idname('ALL', 'All Users');
        }
        
        function has_popdata(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                ajax_show_status(false);
                return;
            }
            
            if (rs.value[0] != null && rs.value[0].Rows.length > 0)
            {
                create_popgrid(rs.value[0], rs.value[1]);
            }
            else
            {
                alert("No options for " + rs.value[1]);
            }
            ajax_show_status(false);

        }
        
        function show_memtype_selection(mtype)
        {
            var t = parseInt(mtype);
            switch (t)
            {
                case 1: do_memtype_all(); break;
                case 2: 
                    ajax_show_status(true);
                    IRSWorld.BranchSetup.MessageGroup.getUsers(has_popdata); 
                    break;
                case 3: 
                    ajax_show_status(true);
                    IRSWorld.BranchSetup.MessageGroup.getBranches(has_popdata);
                    break;
                case 4:
                    ajax_show_status(true);
                    IRSWorld.BranchSetup.MessageGroup.getPopGroups(has_popdata);
                    break;
            }
        }
        
        function memgrid_type_changed(args)
        {
            set_member_idname('', '');
            show_memtype_selection(args.newvalue);  
        }
        
        function memgrid_type_browse(args)
        {
            var memgrid = grpgrid.datagrids[grpgrid.row.selected];
            var mtype = memgrid.getValueAt(args.gridrow, 2);
            show_memtype_selection(mtype);
        }
        
        function memgrid_add_clicked(arg)
        {
            do_memtype_all();
            
            var grpid = grpgrid.getValueAt(grpgrid.row.selected, 0);
            var memgrid = grpgrid.datagrids[grpgrid.row.selected];
            memgrid.setColumn(1, grpid);
        }
        
        function is_duplicate_member(memgrid, row, mtype, memid)
        {
            var find_args = new datagrid_find_arguments();
            
            var find_args = new datagrid_find_arguments();
            var farg = new datagrid_find_argument();
            farg.column_name = 'MsgGroupType';
            farg.value = mtype;
            find_args.addArgument(farg);
            find_args.addConjunction('&');
            farg = new datagrid_find_argument();
            farg.column_name = 'MsgGroupMemberId';
            farg.value = "'" + memid + "'";
            find_args.addArgument(farg);
            
            var locrow = memgrid.find(find_args, false);
            
            return (locrow > -1 & locrow != row);
        }
        
        function memgrid_beforepost(args)
        {
            var memgrid = grpgrid.datagrids[grpgrid.row.selected];
            var mtype = memgrid.getValueAt(args.datarow, 2);
            var t = parseInt(mtype);
            
            {
                var mid = memgrid.getValueAt(args.datarow, 3);
                var mname = memgrid.getValueAt(args.datarow, 5);
                
                if (mid == '')
                {
                    alert("Member id is required");
                    memgrid.curract = '';
                    return;
                } 
            }
            
            if (is_duplicate_member(memgrid, args.datarow, mtype, mid))
            {
                alert("Member: " + mid + " is already defined");
                memgrid.curract = '';
                return;
            }
        }
        
        function member_updates_saved(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var memgrid = grpgrid.datagrids[grpgrid.row.selected];
            memgrid.clearChanges();
            memgrid.refresh(rs.value, memgrid.parent_id, memgrid.id);            
        }
        
        function memgrid_save_clicked(args)
        {
            IRSWorld.BranchSetup.MessageGroup.applyEdits('Members', args.additions, args.deletes, args.edits, member_updates_saved);
        }

        function create_member_grid(arg)
        {
            var memgrid = new datagrid();
            {//0
                var col = new datagrid_column();
                col.header = 'ID';
                col.datafield = 'SEQ_ID';
                col.align = 'center';
                col.styles.cell += 'width:5%;display:none';
                col.styles.header += 'display:none';
                col.styles.datafield += 'display:none';
                memgrid.addColumn(col);	
	        } 
            {//1
                var col = new datagrid_column();
                col.header = 'GID';
                col.datafield = 'MsgGroupId';
                col.align = 'center';
                col.styles.cell += 'width:5%;display:none';
                col.styles.header += 'display:none';
                col.styles.datafield += 'display:none';
                memgrid.addColumn(col);	
	        }   	          
            {//2
                var col = new datagrid_column();
	            col.header = 'Member Type';
	            col.datafield = 'MsgGroupType';
	            col.type = 'select';
	            col.options = memtype_opts;
	            col.align = 'left';
	            col.onchange = 'memgrid_type_changed';
	            col.styles.cell += 'width:20%;';
	            col.styles.datafield += 'width:100%';
	            col.styles.header += 'background-color:skyblue;';
                memgrid.addColumn(col);	
	        } 
            {//3
                var col = new datagrid_column();
	            col.header = 'Member ID';
	            col.datafield = 'MsgGroupMemberId';
	            col.align = 'left';
	            col.editable = false;
	            col.styles.cell += 'width:25%;';
	            col.styles.header += 'background-color:skyblue;';
                memgrid.addColumn(col);	
	        }
            {//4
                var col = new datagrid_column();
	            col.header = '';
	            col.datafield = '';
	            col.type = 'button';
	            col.value = String.fromCharCode(187);
	            col.align = 'left';
	            col.onclick = 'memgrid_type_browse';
	            col.styles.cell += 'width:5%;';
	            col.styles.header += 'background-color:skyblue;';
                memgrid.addColumn(col);	
	        }	        
            {//5
                var col = new datagrid_column();
	            col.header = 'Member Name';
	            col.datafield = 'MsgMemberName';
	            col.align = 'left';
	            col.editable = false;
	            col.styles.cell += 'width:50%;';
	            col.styles.header += 'background-color:skyblue;';
                memgrid.addColumn(col);	
	        }
	        
	        memgrid.styles.header += 'color: #003399';	        
            memgrid.classNames.table = 'table_body';
            memgrid.row.normal_style = 'background-color: #f7ffff;'; 
            memgrid.footer.edit_button.none = false;
            memgrid.footer.edit_button.style += 'background-color: lavender;'; 
            memgrid.footer.add_button.none = false;
            memgrid.footer.add_button.style += 'background-color: lavender;';
            memgrid.footer.add_button.onclick = 'memgrid_add_clicked';
            memgrid.footer.add_button.beforepost = 'memgrid_beforepost';
            memgrid.footer.delete_button.none = false;
            memgrid.footer.delete_button.style += 'background-color: lavender;';             
            memgrid.footer.save_button.none = false;
            memgrid.footer.save_button.style += 'background-color: lavender;'; 
            memgrid.footer.save_button.onclick = 'memgrid_save_clicked';
            memgrid.footer.cancel_button.none = false;
            memgrid.footer.cancel_button.style += 'background-color: lavender;'; 
            memgrid.rows_page = 5; 
            
            return memgrid;           
        }
        
        function member_grid_bind(args)
        {           
            var key;
            try
            {
                key = grpgrid.datatable.Rows[args.datarow].SEQ_ID;
            }
            catch (e) {}
            if (key == null) key = -1;
            
            var rs = IRSWorld.BranchSetup.MessageGroup.getGroupMembers(key, args.grow);
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var memgrid = grpgrid.datagrids[rs.value[1]];
            memgrid.bind(rs.value[0]);
        }
        
        function grpgrid_add_clicked(args)
        {
            var memgrid = grpgrid.datagrids[args.gridrow];
            if (memgrid != null)
            {
                try
                {
                    var tbl = memgrid.getTable();
                    tbl.disabled = true;
                }
                catch (e) {}
            }
            grpgrid.setColumn(1, document.getElementById('ddbranches').value);
        }
        
        function group_updates_saved(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            grpgrid.clearChanges();
            grpgrid.refresh(rs.value);
        }
        
        function grpgrid_save_clicked(args)
        {
            IRSWorld.BranchSetup.MessageGroup.applyEdits('Groups', args.additions, args.deletes, args.edits, group_updates_saved);
        }
        
        function grpgrid_name_blur(args)
        {
            if (isSpecialEx(args.newvalue) & grpgrid.curract != '')
            {
                alert("Input: " + args.newvalue + " contains invalid characters.");
                var ctl = document.getElementById(args.control);
                ctl.focus();
                ctl.select();
            }
        }
        
        function grpgrid_before_post(args)
        {
            var s = trim(grpgrid.getValueAt(args.gridrow, 2));
            if (s.length == 0)
            {
                alert("Please enter a valid group name.");
                grpgrid.curract = ''; //cancel post.
            }
        }
    
        function create_group_grid(dt)
        {
            grpgrid = new datagrid();
            {//0
                var col = new datagrid_column();
                col.header = 'ID';
                col.datafield = 'SEQ_ID';
                col.align = 'center';
                col.styles.cell += 'width:5%;display:none';
                col.styles.header += 'display:none';
                col.styles.datafield += 'display:none';
                grpgrid.addColumn(col);	
	        }  
            {//1
                var col = new datagrid_column();
                col.header = 'BR';
                col.datafield = 'MsgGroupBranch';
                col.align = 'center';
                col.styles.cell += 'width:5%;display:none';
                col.styles.header += 'display:none';
                col.styles.datafield += 'display:none';
                grpgrid.addColumn(col);	
	        }   	         
            {//2
                var col = new datagrid_column();
	            col.header = 'Group Name';
	            col.datafield = 'MsgGroupName';
	            col.align = 'left';
	            col.onblur = 'grpgrid_name_blur';
	            col.styles.cell += 'width:40%;vertical-align:top;';
                grpgrid.addColumn(col);	
	        } 
            {//3
                var col = new datagrid_column();
	            col.header = 'Group Members';
	            col.datafield = 'members';
	            col.type = 'datagrid';
	            col.align = 'left';
	            col.datagrid_create_function = 'create_member_grid';
	            col.datagrid_databind_function = 'member_grid_bind';
	            col.styles.cell += 'width:60%;background-color:skyblue;';
                grpgrid.addColumn(col);	
	        }
	        grpgrid.caption = "MESSAGE GROUPS";
            grpgrid.classNames.table = 'table_body';
            grpgrid.row.normal_style = 'background-color: #f7ffff;';  
            grpgrid.footer.edit_button.none = false;
            grpgrid.footer.edit_button.style += 'background-color: lavender;'; 
            grpgrid.footer.edit_button.beforepost = 'grpgrid_before_post';
            grpgrid.footer.add_button.none = false;
            grpgrid.footer.add_button.style += 'background-color: lavender;';
            grpgrid.footer.add_button.onclick = 'grpgrid_add_clicked';
            grpgrid.footer.add_button.beforepost = 'grpgrid_before_post';
            grpgrid.footer.delete_button.none = false;
            grpgrid.footer.delete_button.style += 'background-color: lavender;';             
            grpgrid.footer.save_button.none = false;
            grpgrid.footer.save_button.style += 'background-color: lavender;'; 
            grpgrid.footer.save_button.onclick = 'grpgrid_save_clicked';
            grpgrid.footer.cancel_button.none = false;
            grpgrid.footer.cancel_button.style += 'background-color: lavender;'; 
            grpgrid.rows_page = 12;
            grpgrid.bind(dt, 'tdgroupgrid', 'tblgroupgrid'); 	        
	    }
    
        function has_message_group(rs)
        {
            if (rs.error != null) 
            {
                alert(rs.error.Message);
                return;
            }
            
            create_group_grid(rs.value);
            document.getElementById('divBody').style.display = 'block';
        }
        
        function has_setup_data(rs)
        {
            if (rs.error!= null)
            {
                spawn_errpage(rs.error.Message);
                return;
            }
            var ddbr = document.getElementById('ddbranches');
            if (rs.value[0] != null) 
            {
                set_ddl_options(ddbr, rs.value[0]);
            }
            ddbr.value = rs.value[2];
            
            if (rs.value[1] != null) memtype_opts = rs.value[1];
            
            issa = rs.value[3];
        }
        
        function fixht()
        {
            document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
            document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
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
                document.getElementById('tdBody').style.verticalAlign = 'top';
                
                fixht();
            }        
        }                
        
        function page_setup()
        {
            if (top == self)
            {
                window.open('./BranchSetupMain.aspx?load=MessageGroup.aspx', '_top');
            }
            else if (top.isAuthenticated())
            {
                AjaxPro.timeoutPeriod = ajax_timeout;
                
                fixmain();
                
                var x = '';
                IRSWorld.BranchSetup.MessageGroup.getSetupData(has_setup_data);
                IRSWorld.BranchSetup.MessageGroup.getMessageGroups('', has_message_group);
            }
            else
            {
                spawn_errpage("This module requires login!");
            } 
        }
        
        function ddbraches_change(ddl)
        {
            IRSWorld.BranchSetup.MessageGroup.getMessageGroups(ddl.value, has_message_group);
        }