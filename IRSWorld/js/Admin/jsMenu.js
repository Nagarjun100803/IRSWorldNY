        var d3;
        var lmode = -1;
        var grp_opts;
        var par_opts;
        var grpgrid;
        var pargrid;
        var curr_act = '';
        
        var sel_modid;
        
        function grptab()
        {
            this.Rows = new Array();
        }
        
        grptab.prototype.add_group = function(grow)
        {
            this.Rows.push(grow);
        }
        
        function grptab_row()
        {
            this.SEQ_ID = 0;
            this.GroupName = '';
            this.GroupId = 0;
        }
        
        function is_submenu(rows, idx)
        {
            if (rows[idx].MenuParentID == 0) return false;
            
            for (var j = 0; j < rows.length; j++)
            {
                if (rows[j].MenuParentID == rows[idx].SEQ_ID) return true;
            }
            
            return false;
        }
        
        function is_parent(rows, idx)
        {
            if (rows[idx].MenuParentID == 0) return true;

            return false;
        }

        function add_tree_row(dtri, rows, i, lnarr, gtab, ptab)
        {
            if (is_parent(rows, i))
            {
                dtri.add(rows[i]['SEQ_ID'], rows[i]['MenuParentID'], rows[i]['MenuName'], lnarr[1]);
                
                grow = new grptab_row();
                grow.SEQ_ID = rows[i].SEQ_ID;
                grow.GroupName = rows[i].MenuName;
                grow.GroupId = rows[i].MenuGroup;
                gtab.add_group(grow);
                
                ptab.add_group(grow);
            }
            else if (is_submenu(rows, i))
            {
                dtri.add(rows[i]['SEQ_ID'], rows[i]['MenuParentID'], rows[i]['MenuName'], lnarr[2]);
                grow = new grptab_row();
                grow.SEQ_ID = rows[i].SEQ_ID;
                grow.GroupName = rows[i].MenuName;
                grow.GroupId = rows[i].MenuGroup;
                ptab.add_group(grow);
            }
            else // menu item
            {
                dtri.add(rows[i]['SEQ_ID'], rows[i]['MenuParentID'], rows[i]['MenuName'], lnarr[3]);
            }
            
            dtri.aNodes[dtri.aNodes.length - 1].userData = new Array( rows[i] );
        }
        
        function create_group_grid(gtab)
        {
            grpgrid = new datagrid();
            {
	            var col = new datagrid_column();
	            col.header = 'SEQID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            grpgrid.addColumn(col);	
		    }
            {
	            var col = new datagrid_column();
	            col.header = 'Menu Groups';
	            col.datafield = 'GroupName';
	            col.align = 'center';
		        col.styles.cell += 'width:100%;';
	            grpgrid.addColumn(col);	
		    }
            grpgrid.classNames.table = 'table_body';
            grpgrid.row.normal_style = 'background-color: #f7ffff;'; 
            grpgrid.row.onclick='grpgrid_row_selected';
            grpgrid.bind(gtab, 'divPopgrp', 'tbPopgrp');
        }
        
        function grpgrid_row_selected(args)
        {
            var txtdesc = document.getElementById('txtMenuGroup');
            var txtid = document.getElementById('txtMenuGroupId');
            
            var gid = grpgrid.getValueAt(args.gridrow, 0);
            if (gid == 0)
            {
                alert("Group ID cannot be 0");
            }
            else
            {
                txtid.value = gid;
                txtdesc.value = grpgrid.getValueAt(args.gridrow, 1);
                
                close_explore();
                
                fire_event('txtdesc', 'onchange');
            }
        }
        
        function create_parent_grid(ptab)
        {
            pargrid = new datagrid();
            {
	            var col = new datagrid_column();
	            col.header = 'SEQID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            pargrid.addColumn(col);	
		    }
            {
	            var col = new datagrid_column();
	            col.header = 'Parents';
	            col.datafield = 'GroupName';
	            col.align = 'center';
		        col.styles.cell += 'width:100%;';
	            pargrid.addColumn(col);	
		    }
            {
	            var col = new datagrid_column();
	            col.header = 'GID';
	            col.datafield = 'GroupId';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            pargrid.addColumn(col);	
		    }
		    
            pargrid.classNames.table = 'table_body';
            pargrid.row.normal_style = 'background-color: #f7ffff;'; 
            pargrid.row.onclick = 'pargrid_row_selected';
            pargrid.bind(ptab, 'divPopma', 'tbPopma');
        }
        
        function pargrid_row_selected(args)
        {
            var txtdesc = document.getElementById('txtMenuParent');
            var txtid = document.getElementById('txtMenuParentId');
            var txtgid = document.getElementById('txtMenuGroupId');
            
            txtid.value = pargrid.getValueAt(args.gridrow, 0);
            txtdesc.value = pargrid.getValueAt(args.gridrow, 1);
            var gid = pargrid.getValueAt(args.gridrow, 2);
            
            close_explore();
            
            if (gid != txtgid.value)
            {
                txtgid.value = gid;
                document.getElementById('txtMenuGroup').value = getGroupName(gid);
            }   
            fire_event(txtdesc, 'onchange');                    
        }
        
        function create_tree(dt, div, trid, lnarr)
        {
            div.innerHTML = '';
            var iw = 'IRSWorld V1.0';
            
            var dtri = new dTree(trid);
            
            var gtab = new grptab();
            var gtab_row = new grptab_row();
            gtab_row.SEQ_ID = 0;
            gtab_row.GroupName = iw;
            gtab.add_group(gtab_row);
            
            var ptab = new grptab();
            ptab.add_group(gtab_row);
            
            dtri.add(0, -1, iw, lnarr[0]);
            for (var i = 0; i < dt.Rows.length; i++)
            {
                add_tree_row(dtri, dt.Rows, i, lnarr, gtab, ptab);
            }
            
            create_group_grid(gtab);
            create_parent_grid(ptab);
            
            if (div) div.innerHTML = dtri.toString();
            
            return dtri;
        }
        
        function show_all()
        {
            if (d3 != null)
            {
                d3.oAll(true);
            }
        }
        
        function show_explore(txt, div, tle)
        {
            var pop = document.getElementById('divExplore');
            if (pop.style.display == 'block') 
            {
                pop.style.display = 'none';
                return;
            }
            setText(document.getElementById('tdPopTitle'), tle);
            var divs = document.getElementById('tdPop').getElementsByTagName('div');
            for (var i = 0; i < divs.length; i++)
            {
                divs.item(i).style.display = 'none';
            }
            var ixy = calcpos(txt);
            pop.style.left = ixy.left + 'px';
            pop.style.top = (ixy.top + txt.offsetHeight) + 'px';
            pop.style.height = div.style.height + 'px';
            div.style.display = 'block';
            pop.style.display = 'block';
        }
        
        function close_explore()
        {
            var pop = document.getElementById('divExplore');
            if (pop.style.display == 'block') 
            {
                pop.style.display = 'none';
            }
        }
        
        function btn_enable(btn, enable)
        {
            if (btn == 'All') 
            {
                var tbl = document.getElementById('tblbuttons');
                if (tbl)
                {
                    var btns = tbl.getElementsByTagName('input');
                    for (var i = 0; i < btns.length; i++)
                    {
                        enable_button(btns.item(i), enable);
                    }
                }
            }
            else
            {
                var btnobj = document.getElementById('btn' + btn);
                if (btnobj)
                {
                    enable_button(btnobj, enable);
                }
            }
        }
        
        function getGroupName(gid)
        {
            for (var i = 0; i < grpgrid.datatable.Rows.length; i++)
            {
                var xgid = grpgrid.datatable.Rows[i].GroupId;
                if (xgid == gid) return grpgrid.datatable.Rows[i].GroupName;
            }
            return '';
        }
        
        function display_group()
        {
            var txt = document.getElementById('txtMenuGroup');
            var tid = document.getElementById('txtMenuGroupId');
            var ud = d3.aNodes[d3.selectedNode].userData;
            if (ud == null)
            {
                txt.value = '';
                tid.value = '0';
                return;
            }
            var grp = ud[0]["MenuGroup"];
            tid.value = grp;
            txt.value = getGroupName(grp); //d3.aNodes[grp].name;
         }
        
        function display_menu_fields()
        {
            document.getElementById('txtMenuParent').value = d3.aNodes[d3.selectedNode]._p.name;
            document.getElementById('txtMenuParentId').value = d3.aNodes[d3.selectedNode]._p.id;
            document.getElementById('txtMenuSeqId').value = d3.aNodes[d3.selectedNode].id;
            document.getElementById('txtMenuName').value = d3.aNodes[d3.selectedNode].name;
            var ud = d3.aNodes[d3.selectedNode].userData;
            if (ud != null)
            {
                var u = ud[0]["ModuleUrl"];
                var i = ud[0]["ModuleId"];
                var n = ud[0] ["MenuItemNo"];
                u = (u == null) ? '' : u;
                i = (i == null) ? '0' : i;
                n = (n == null) ? '0' : n;
                document.getElementById('txtMenuUrl').value = u;
                document.getElementById('txtModuleId').value = i;
                document.getElementById('txtMenuItemNo').value = n;
            }
        }
        
        function d3_click(nid)
        {
            lmode = nid;
            btn_enable('All', false);
            if (nid == 0)
            {
                document.getElementById('txtMenuSeqId').value = 0;
                document.getElementById('txtMenuGroupId').value = 0;
                document.getElementById('txtMenuGroup').value = grpgrid.getValueAt(nid, 1);
                document.getElementById('txtMenuParentId').value = 0;
                document.getElementById('txtMenuParent').value = grpgrid.getValueAt(nid, 1) + " Root"; 
                document.getElementById('txtMenuItemNo').value = 0;
                document.getElementById('txtMenuName').value = grpgrid.getValueAt(nid, 1);
            }
            else
            {            
                display_group();
                display_menu_fields();
                btn_enable('Delete', true);
                btn_enable('Edit', true);
            }
            
            btn_enable('Add', true);
        }
        
        function grid_browse_close()
        {
            var retval = self.returnValue;
            if (retval == null || retval.length == 0) return;
            document.getElementById('txtModuleId').value = '' + retval[0]; //'ModuleId'
            document.getElementById('txtMenuUrl').value = retval[1]; //'ModuleUrl'
            var nm = document.getElementById('txtMenuName').value;
            if (nm == null || nm.length == 0) 
            {
                document.getElementById('txtMenuName').value = retval[2];
            }
            if (curr_act != '')
            {
                fire_event(document.getElementById('txtMenuName'), 'onchange');
            }
        }
        
        function grid_btn_browse()
        {
            show_frame_dialog('Modules.aspx?function=popup', 'grid_browse_close', '100%', '100%', 0); 
        }
        
        function isMainNode(pid)
        {
            var s = getGroupName(pid);
            if (s != '') return 0;
            return pid;
        }
        
        function menu_txt_change(txtobj)
        {
            if (txtobj.id == 'txtMenuName')
            {
                var tgroup = document.getElementById('txtMenuGroup');
                if (tgroup.value == '')
                {
                    tgroup.value = txtobj.value;
                }
            }
            else if (curr_act == 'Edit' & txtobj.id == 'txtMenuGroup')
            {
                document.getElementById('txtMenuParentId').value = -1;
                document.getElementById('txtMenuParent').value = '';
                document.getElementById('btnBrowParent').onclick();
            }
            else if (curr_act == 'Edit' & txtobj.id == 'txtMenuParent')
            {
                var ud = d3.aNodes[d3.selectedNode].userData;
                if (ud != null)
                {
                    var pid = parseInt(document.getElementById('txtMenuParentId').value);
                    var gid = parseInt(document.getElementById('txtMenuGroupId').value);
                    if (pid != ud[0].MenuParentID & gid != ud[0].MenuGroup)
                    {
                        var pud = isMainNode(pid);
                        if (pid == 0 & gid == 0)
                        {
                            document.getElementById('txtMenuGroupId').value = ud[0].SEQ_ID;
                            document.getElementById('txtMenuGroup').value = ud[0].MenuName;
                        }
                        else
                        {
                            IRSWorld.Admin.Menu.getNextMenuItemNo("" + gid, "" + pud, hasLastSeqId);
                        }
                    }
                }
            }
            btn_enable('Save', true);
        }
        
        function hasLastSeqId(rs)
        {
            var ino = document.getElementById('txtMenuItemNo');
            if (rs.error != null)
            {
                alert(rs.error.Message);
                ino.value = 0;
                return;
            }
            
            ino.value = rs.value;
        }
        
        function getNextMenuItemNo()
        {
            var ud = d3.aNodes[d3.selectedNode].userData;
            if (ud != null)
            {
                var gid = ud[0]["MenuGroup"];
                var pid = (lmode == 2) ? ud[0]["SEQ_ID"] : ud[0]["MenuParentID"];
                IRSWorld.Admin.Menu.getNextMenuItemNo("" + gid, "" + pid, hasLastSeqId);
            }
        }
        
        function getLastGroupNoTree()
        {
            var hi = 0;
            
            for (var i = 0; i < d3.aNodes.length; i++)
            {
                var ud = d3.aNodes[i].userData;
                if (ud != null)
                {
                    var lo = ud[0]["MenuGroup"];
                    if (lo > hi) hi = lo;
                }
            }
            
            return hi + 1;
        }
        
        function hasLastGroupNo (rs)
        {
            var gid = 0;
            
            if (rs.error != null)
            {
                alert(rs.error.Message);
                gid = getLastGroupNoTree();
            }
            else
            {
                gid = rs.value;
            }
            
            document.getElementById('txtMenuGroupId').value = gid;
        }
        
        function menu_add_click()
        {
            curr_act = 'Add';
        
            set_read_only(document.getElementById('tblitems'), false);
            clear_fields(document.getElementById('tblitems'));
            display_group();
            
            document.getElementById('txtMenuSeqId').value = '0';
            document.getElementById('txtMenuParent').value = d3.aNodes[d3.selectedNode].name;
            document.getElementById('txtMenuParentId').value = d3.aNodes[d3.selectedNode].id;
            document.getElementById('txtMenuParent').readOnly = true;
            document.getElementById('txtModuleId').value = 0;
            if (lmode == 0)
            {
                document.getElementById('txtMenuItemNo').value = 1;
                document.getElementById('txtMenuGroup').readOnly = true;
                IRSWorld.Admin.Menu.getLastGroupNo(hasLastGroupNo);
            }
            else
            {
                getNextMenuItemNo();
            }
            btn_enable('Cancel', true);
            btn_enable('Add', false);
            btn_enable('Edit', false);
            btn_enable('Delete', false);
        }
        
        function menu_save_click()
        {
            if (document.getElementById('txtMenuParentId').value == '-1')
            {
                alert("You must select a valid Menu Parent.");
                return;
            }
            
            if (document.getElementById('txtMenuName').value.length == 0)
            {
                alert("A menu name must be provided.");
                return;
            }
            
            btn_enable('Save', false);
            btn_enable('Cancel', false);
            
            var rec = 'crud=' + curr_act + '~';
            rec += 'SEQ_ID=' + document.getElementById('txtMenuSeqId').value + '~';
            rec += 'MenuGroup=' + document.getElementById('txtMenuGroupId').value + '~';
            rec += 'MenuParentID=' + document.getElementById('txtMenuParentId').value + '~';
            rec += 'MenuItemNo=' + document.getElementById('txtMenuItemNo').value + '~';
            rec += 'MenuName=' + document.getElementById('txtMenuName').value + '~';
            rec += 'ModuleId=' + document.getElementById('txtModuleId').value;

            //=======================================================================
            //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
            //IRSWorld.Admin.Menu.applyChanges(rec, gotMenuData);

            var param_approver = IRSWorld.Admin.Menu.ParamApproverFlag().value;
            if (param_approver == true) {
                alert("RECORD SUBMITTED FOR APPROVAL.");
            }

            IRSWorld.Admin.Menu.applyChanges(rec, gotMenuData_reload);
            //=======================================================================
        }
        
        function menu_cancel_click()
        {
            curr_act = '';
            set_read_only(document.getElementById('tblitems'), true);
            var obj = document.getElementById('sd3' + d3.aNodes[d3.selectedNode].id);
            if (obj == null)
            {
                obj = document.getElementById('sd30');
            }
            fire_event(obj, 'onclick');
        }
        
        function menu_del_click()
        {
            var ud = d3.aNodes[d3.selectedNode].userData;
            if (ud != null)
            {
                var mname = ud[0]["MenuName"];
                var parname = d3.aNodes[d3.selectedNode]._p.name;
                if (confirm("Delete " + parname + " -> " + mname))
                {
                    btn_enable("Edit", false);
                    curr_act = "Delete";
                    menu_save_click();
                }
            }
        }
        
        function menu_edit_click()
        {
            set_read_only(document.getElementById('tblitems'), false);
            btn_enable('All', false);
            btn_enable('Cancel', true);
            curr_act = 'Edit';
        }
              
        function gotMenuData(rs)
        {
            if (rs.error != null) 
            {
                alert(rs.error.Message);
                return;
            }
           
            curr_act = '';
            
            d3 = create_tree
            (
                rs.value,
                document.getElementById('divTree'), 
                'd3', 
                new Array('javascript:d3_click(0);', 'javascript:d3_click(1);', 'javascript:d3_click(2);', 'javascript:d3_click(3);')
            );
            
            //show_all();
            set_read_only(document.getElementById('tblitems'), true);
            clear_fields(document.getElementById('tblitems'));
            
            if (d3)
            {
                document.getElementById("sd30").onclick(); 
            }
        }

        //=======================================================================
        //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
        function gotMenuData_reload(rs) {
            if (rs.error != null) {
                alert(rs.error.Message);
                return;
            }

            if (top != self) {
                fixht();
                IRSWorld.Admin.Menu.getMenuData(gotMenuData);
            }
            else {
                window.open('../Main/IRSMain.aspx?load=../Admin/Menu.aspx', '_top');
            }
        }
        //=======================================================================

        function fixht()
        {
            var div = document.getElementById('divmain');
            if (div)
            {
                div.style.height = (top.document.getElementById('divCenter').offsetHeight) + 'px';
                div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
                div.style.display = 'block';
            }
        }
    
        function page_setup()
        {            
            if (top != self)
            {
                fixht();
                IRSWorld.Admin.Menu.getMenuData(gotMenuData);
            }
            else
            {
                window.open('../Main/IRSMain.aspx?load=../Admin/Menu.aspx', '_top');
            }
        }
        
        function close_session()
        {
            if (is_closing())
            {
                IRSWorld.Admin.Menu.close_session();
            }
            else
            {
                IRSWorld.Admin.Menu.drop_irsmenu();
            }
        } 

