    var ajax_timeout = 60 * 1000;
    var issa = false;
    var isba = false;
    var flag_opts = null;
    
    function btn_find_click()
    {
        var val = document.getElementById('txtfind').value;
        if (val.length == 0) 
        {
            alert('Find needs search criteria.');
            return;
        }
        var find_args = new datagrid_find_arguments();
        var farg = new datagrid_find_argument();
        farg.column_name = 'UserID';
        farg.operator = "search";
        farg.value = val;
        find_args.addArgument(farg);
        find_args.addConjunction('|');
        farg = new datagrid_find_argument();
        farg.column_name = 'FullName';
        farg.operator = "search";
        farg.value = val;
        find_args.addArgument(farg);
        find_args.select_row = false;
        var locrow = db_grid.find(find_args, true);
        
        if (locrow == -1)
        {
            alert("No row found.");
        }
        
        find_close_popup();
    }
    
    function show_popup_find(args)
    {
        var div = document.getElementById('divPopFind');
        var td = document.getElementById('tdPopFind');
        var topsize = top.document.getElementById('divCenter').offsetHeight;
        var topparent = document.getElementById('divContent').offsetHeight;
        div.style.height = ((topsize > topparent) ? topsize : topparent + 30) + 'px';
        if (isIE) div.className = 'div_undock';
        else div.setAttribute("class", "div_undock");
        td.style.height = div.offsetHeight + 'px'; 
        td.style.width = div.offsetWidth + 'px'; 
        td.style.paddingTop = ( (parseFloat(div.style.height) / 2) - (parseFloat(document.getElementById('tblPopFind').offsetHeight) / 2) ) - 30 + 'px';         
        div.style.display = 'block'; 
    }
    
    function find_close_popup()
    {
        document.getElementById('divPopFind').style.display = 'none';
    }    
    
    function search_clicked()
    {
        var args = document.getElementById('txt_search').value;
        var loc = document.getElementById('ddl_search_location').value;
        var flag = parseInt(document.getElementById('ddl_search_flags').value);
        var logged = parseInt(document.getElementById('ddl_search_logon').value);
        if (args.length == 0 & loc == "0" & flag == 0 & logged == 0)
        {
            if (!confirm('No criteria selected/entered, list all users?')) return; 
        }
        
        ajax_show_status(true);
        IRSWorld.Admin.UserUnlock.search(args, loc, flag, logged, searched);    
    }
    
    function search_enter_click(e)
    {
        if (!e) var e = window.event;
        
        if (e.keyCode == 13) // Enter key
        {
            document.getElementById('btn_search').onclick();
        }
    }
    
    function btn_showsearch_clicked()
    {
        document.getElementById('ddl_search_location').selectedIndex = 0;
        document.getElementById('ddl_search_flags').selectedIndex = 0;
        document.getElementById('ddl_search_logon').selectedIndex = 0;
        show_panel('Search');
    }
    
    function show_panel(panel_suffix)
    {
        var pblock = 'div' + panel_suffix;
        var pnone = 'div' + ((panel_suffix == 'Result') ? 'Search' : 'Result');
        
        document.getElementById(pblock).style.display = 'block';
        document.getElementById(pnone).style.display = 'none';
        
        document.onkeyup = (panel_suffix == 'Search') ? search_enter_click : null;
    }
        
    function fixdim()
    {
        if (top.document.getElementById('divCenter'))
        {
            var div = document.getElementById('divContent');
            
            div.style.height = (top.document.getElementById('divCenter').offsetHeight + 10) + 'px';
            div.style.width = (top.document.getElementById('divCenter').offsetWidth + 5) + 'px';
        }        
    }
    
    function set_ddlflags_option(opt_flags)
    {
        if (opt_flags == null)
        {
            var msg = "UserUnlock@set_ddlflags_option: server did not return values";
            spawn_errpage(msg);
        }
        
        set_ddl_options(document.getElementById("ddl_search_flags"), opt_flags);
    }
    
    function set_ddlbranch_option(opt_br, selbr)
    {
        if (opt_br == null)
        {
            var msg = "UserUnlock@set_ddlbranch_option: server did not return values";
            spawn_errpage(msg);
        }
        
        set_ddl_options(document.getElementById("ddl_search_location"), opt_br);
        document.getElementById("ddl_search_location").value = selbr;
    }
    
    function flags_onchange(args)
    {
        var iflag = parseInt(args.oldvalue);
        var tab = db_grid.getTable();
        if (iflag == 2 | iflag == 4)
        {
            var nflag = parseInt(args.newvalue);
            if (nflag == 2 | nflag == 4) { return; }
            {
                alert("Only Active/Disabled are allowed.");
            }
        }
        else
        {
            alert("Only Disabled/Active state are editable");
        }
        var ddl = document.getElementById(args.control);
        ddl.value = iflag;
        db_grid.footerButtonClick('', 'Cancel');
        return;
        
    }
    
    function applyUpdates(args)
    {
        IRSWorld.Admin.UserUnlock.applyUpdates(args.additions, args.deletes, args.edits, searched);
    }
    
	function create_result_grid()
	{
		db_grid = new datagrid();

		var col1 = new datagrid_column();
		col1.header = 'CIF ID';
		col1.datafield = 'CIFID';
		col1.align = 'left';
		col1.editable = false;
		col1.styles.cell += 'width:10%';
		db_grid.addColumn(col1);

		var col2 = new datagrid_column();
		col2.header = 'User ID';
		col2.datafield = 'UserID';
		col2.editable = false;
		col2.styles.cell += 'width:10%';
		db_grid.addColumn(col2);

		var col3 = new datagrid_column();
		col3.header = 'User Name';
		col3.datafield = 'FullName';
		col3.editable = false;
		col3.styles.cell += 'width:35%;';
		db_grid.addColumn(col3);

		var col4 = new datagrid_column();
		col4.header = 'Location';
		col4.datafield = 'location';
		col4.editable = false;
		col4.styles.cell += 'width:8%';
		col4.styles.datafield += 'text-align: center;';
		db_grid.addColumn(col4);
		
		var col5 = new datagrid_column();
		col5.header = 'Role';
		col5.datafield = 'RoleDesc';
		col5.editable = false;
		col5.styles.cell += 'width:19%';
		db_grid.addColumn(col5);

		var col6 = new datagrid_column();
		col6.header = 'Status';
		col6.datafield = 'Flags';
		col6.editable = true;
	    col6.type = 'select';
	    col6.onchange = 'flags_onchange';
		col6.options = flag_opts;	
		col6.styles.cell += 'width:10%';
		db_grid.addColumn(col6);
		
		var col7 = new datagrid_column();
		col7.header = 'Logged On';
		col7.datafield = 'LoggedOn';
		col7.editable = true;
	    col7.type = 'check';
		col7.styles.cell += 'width:8%';
		db_grid.addColumn(col7);
		
		db_grid.footer.find_button.none = false;
		db_grid.footer.find_button.onclick = 'show_popup_find';
        db_grid.footer.edit_button.none = false;            
        db_grid.footer.edit_button.style += 'background-color: lavender;'; 
        db_grid.footer.save_button.none = false;
        db_grid.footer.save_button.style += 'background-color: lavender;';  
        db_grid.footer.save_button.onclick = 'applyUpdates';
        db_grid.footer.cancel_button.none = false;
        db_grid.footer.cancel_button.style += 'background-color: lavender;'; 
        db_grid.row.normal_style = 'background-color: #f7ffff;';          
	}       
    
    function searched(rs)
    {
        ajax_show_status(false);
        if (rs.error != null)
        {
            spawn_errpage(rs.error.Message);
            return;
        }
        
        if (rs.value != null)
        {
            create_result_grid();
		    db_grid.bind(rs.value, 'tdSearchGrid', 'tblSearchGrid');    		
        }            
        
        show_panel("Result");
    }
            
    function haveSetupData(rs)
    {
        if (rs.error != null)
        {
            spawn_errpage(rs.error.Message);
            return;
        }
        
        issa = rs.value[0];
        isba = rs.value[1];
        flag_opts = rs.value[2];
        
        set_ddlflags_option(rs.value[2]);
        set_ddlbranch_option(rs.value[3], rs.value[4]);
        ajax_show_status(true);
        IRSWorld.Admin.UserUnlock.search('', rs.value[4], 0, 0, searched);
    }
            
    function page_setup()
    {
        fixdim();
        if (top != self)
        {
            AjaxPro.timeoutPeriod = ajax_timeout;
            IRSWorld.Admin.UserUnlock.getSetupData(haveSetupData);
        }
        else
        {
            window.open('../Main/IRSMain.aspx?load=../Admin/UserUnlock.aspx', '_parent');
        }
    }
    
    function close_session()
    {
        if (is_closing())
        {
            IRSWorld.Admin.UserUnlock.close_session();
        }
        else
        {
            IRSWorld.Admin.UserUnlock.drop_irsusers();
        }
    }