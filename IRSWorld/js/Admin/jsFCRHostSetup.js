
    
        var dbgrid;
        var save_pos = 0;
        var save_row = -1;
        var save_data_row = -1;
    
        function init_ddl(tid, dt, val, desc, ival)
        {
            var ddl = document.getElementById(tid);
            if (dt.Rows.length > 0)
            {
                for (var i = 0; i < dt.Rows.length; i++)
                {
                    create_option(ddl, dt.Rows[i][val], dt.Rows[i][desc]);
                }  
                
                ddl.selectedIndex = 0;         
            }
        }
        
        function btn_new_click()
        {
            var btn = document.getElementById('btnnew');
            var xy = calcpos(btn);
            var div = document.getElementById('divpopnew');
            div.style.top = (xy.top + btn.offsetHeight + 5) + 'px';
            div.style.left = (xy.left - 371 - 5) + 'px';
            div.style.display = 'block';
            
            document.getElementById('txtnew').focus();
        }
        
        function settingsDeleted(rs)
        {
            if (rs.error != null)
            {
                alert (rs.error.Message);
                return;
            }
            
            var ddl = document.getElementById('ddsettingsid');            
            alert ("Settings " + ddl.options[ddl.selectedIndex].text + " deleted.");
            
            var i = ddl.selectedIndex;
            ddl.options.remove(i);
                        
            ddl.selectedIndex = 0;
            ddl_setting_change();       
        }
        
        function btn_delete_click()
        {
            var ddl = document.getElementById('ddsettingsid');            
            IRSWorld.Admin.FCRHostSetup.deleteSettings(ddl.options[ddl.selectedIndex].text, settingsDeleted);         
        }
        
        function hasNewSettings(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var ddl = document.getElementById('ddsettingsid');            
            create_setting_grid(rs.value, ddl.options[ddl.selectedIndex].text);
            
            try
            {
                if (save_pos > 0) dbgrid.pg_position(save_pos);
                if (save_row > -1 & save_data_row > -1)
                {
                    dbgrid.rowSelected(save_data_row, save_row);
                }
            }
            catch (e) {}
        }
        
        function btn_ok_click()
        {
            var txt = document.getElementById('txtnew');
            if (trim(txt.value).length < 1)
            {
                alert("Settings id must not be blank");
                txt.focus();
                return false;
            }        
        
            var ddl = document.getElementById('ddsettingsid');
            create_option(ddl, txt.value, txt.value);
            ddl.selectedIndex = ddl.options.length - 1;
            close_popnew();
            
            ddl_setting_change();        
        }
        
        function txtnew_blur(txt)
        {            
            if (isSpecialEx(txt.value))
            {
                alert("Settings id must not contain special characters");
                txt.focus();
                txt.select();
                return false;
            }
            
            return true;
        }
        
        function ddl_setting_change()
        {
            save_pos = 0;
            save_row = -1;
            save_data_row = 0;
        
            var ddl = document.getElementById('ddsettingsid');
            var txt = ddl.options[ddl.selectedIndex].text;
            if (trim(txt).length > 0)
            {
                if (isSpecialEx(txt))
                {
                    alert("Settings id must not contain special characters");
                    return;
                }
                IRSWorld.Admin.FCRHostSetup.getSettings(txt, hasNewSettings);
            }
            else
            {
                alert("Settings id must not be blank");
            }
        }
        
        function close_popnew()
        {
            var div = document.getElementById('divpopnew').style.display = 'none';
        }
        
        function settingsPosted(rs)
        {
            if (rs.error != null)
            {
                alert(rs.error.Message);
                return;
            }
            
            var ddl = document.getElementById('ddsettingsid');
            IRSWorld.Admin.FCRHostSetup.getSettings(ddl.options[ddl.selectedIndex].text, hasNewSettings);
        }
        
        function dbgrid_onpost(args)
        {
            save_pos = dbgrid.curr_page;
            save_row = dbgrid.row.selected;
            save_data_row = dbgrid.row.selected_data_row;
            IRSWorld.Admin.FCRHostSetup.applyEdits(args.additions, args.deletes, args.edits, settingsPosted);
        }
        
        function grid_save_click(args)
        {
            save_pos = dbgrid.curr_page;
            save_row = dbgrid.row.selected;
            save_data_row = dbgrid.row.selected_data_row;            
            IRSWorld.Admin.FCRHostSetup.saveUpdates(args.additions, args.deletes, args.edits, hasNewSettings);
        }
        
        function dbgrid_add_clicked(args)
        {
            var ddl = document.getElementById('ddsettingsid');
            dbgrid.setColumn(1, ddl.options[ddl.selectedIndex].text);
        }
        
        function dbgrid_beforepost(args)
        {
            var val = dbgrid.getValueAt(args.datarow, 2);
            if (trim(val).length == 0)
            {
                alert("Setting keyword cannot be blank");
                dbgrid.curract = "";
                return;
            }
            if (isSpecialEx(val))
            {
                alert("Setting keyword contains invalid character");
                dbgrid.curract = "";
                return;
            }
            
            val = dbgrid.getValueAt(args.datarow, 3);
            if (trim(val).length == 0)
            {
                alert("Setting value cannot be blank");
                dbgrid.curract = "";
                return;
            }
        }
        
        function define_grid(caps)
        {
            dbgrid = new datagrid();
             { //0
                var col = new datagrid_column();
	            col.header = 'ID';
	            col.datafield = 'SEQ_ID';
	            col.align = 'center';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
	            dbgrid.addColumn(col);	
		    }             
		    { //1
    		    var col = new datagrid_column();
    		    col.header = 'SettingsID';
    		    col.datafield = 'SettingsID';
    		    col.align = 'left';
		        col.styles.cell += 'width:5%;display:none';
		        col.styles.header += 'display:none';
		        col.styles.datafield += 'display:none';
    		    dbgrid.addColumn(col);
            }
            { //2
    		    var col = new datagrid_column();
    		    col.header = 'Name';
    		    col.datafield = 'SettingName';
    		    col.align = 'left';
    		    col.styles.cell += 'width:30%';
    		    dbgrid.addColumn(col);
            }
            { //3
    		    var col = new datagrid_column();
    		    col.header = 'Value';
    		    col.datafield = 'SettingValue';
    		    col.align = 'left';
    		    col.styles.cell += 'width:65%';
    		    dbgrid.addColumn(col);
            } 
            {
    		    var col = new datagrid_column();
    		    col.header = 'Disabled';
    		    col.datafield = 'SettingDisabled';
    		    col.align = 'center';
    		    col.type = 'check';
    		    col.styles.cell += 'width:5%;';
    		    col.styles.datafield += 'width:100%;';
    		    dbgrid.addColumn(col);
            }           
        }
        
        function create_setting_grid(dt, caps)
        {
            dbgrid = null;
            define_grid(caps);

            dbgrid.caption = caps + " Settings";
            dbgrid.classNames.table = 'table_body';
            if (caps != 'Default')
            {
                dbgrid.footer.edit_button.none = false;
                dbgrid.footer.edit_button.onpost = 'dbgrid_onpost';
                dbgrid.footer.edit_button.beforepost = 'dbgrid_beforepost';
                dbgrid.footer.add_button.none = false;
                dbgrid.footer.add_button.onclick = 'dbgrid_add_clicked';
                dbgrid.footer.add_button.style += 'background-color: lavender;'; 
                dbgrid.footer.add_button.onpost = 'dbgrid_onpost';
                dbgrid.footer.add_button.beforepost = 'dbgrid_beforepost';
                dbgrid.footer.delete_button.none = false;
                dbgrid.footer.delete_button.onpost = 'dbgrid_onpost';
                dbgrid.footer.delete_button.style += 'background-color: lavender;color:maroon;';            
                dbgrid.footer.save_button.none = false;
                dbgrid.footer.save_button.always_enabled = (dt.Rows.length > 0);
                dbgrid.footer.save_button.onclick = 'grid_save_click';  
                dbgrid.footer.save_button.style += 'background-color: lavender;';  
                dbgrid.footer.cancel_button.none = false;
                dbgrid.footer.cancel_button.style += 'background-color: lavender;'; 
                dbgrid.row.normal_style = 'background-color: #f7ffff;';  
                dbgrid.postToServer = true;  
            } 
            dbgrid.bind(dt, 'tdsettingsgrid', 'tblsettingsgrid'); 
            dbgrid.clearChanges();
            if (dt.Rows.length > 0 && caps != 'Default') 
            {
                dom('btndelete').disabled = false;
            }  
        }    
    
        function hasInitData(rs)
        {
            if (rs.error != null)
            {
                alert( rs.error );
                spawn_errpage(rs.error.Message);
                return;
            }
            
            init_ddl('ddsettingsid', rs.value[0], "fldCode", "fldDesc", 0);
            var ddl = document.getElementById('ddsettingsid');
            create_setting_grid(rs.value[1], ddl.options[ddl.selectedIndex].text);
        }
        
        function fixdim()
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divmain');
                div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
                div.style.height = top.document.getElementById('divCenter').offsetHeight + 'px';
            }
        }
    
        function page_setup()
        {
            window.onunload = close_session;
            if (top != self)
            {
                fixdim();
                IRSWorld.Admin.FCRHostSetup.getInitData(hasInitData);
            }
            else
            {
                window.open("../Main/IRSMain.aspx?load=../Admin/FCRHostSetup.aspx", "_parent");
            }
        }
        
        function close_session(e)
        {
            if (is_closing(e))
            {
                IRSWorld.Admin.FCRHostSetup.close_session();
            }
            else
            {
                IRSWorld.Admin.FCRHostSetup.drop_fcrsetup();
            }
        }     