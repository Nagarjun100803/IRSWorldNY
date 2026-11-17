var db_grid;
var popup;
    function valid_id_dropdown()
    {
        this.commons           = null;  
        this.Load_Branches = function()
        {
            IRSWorld.BranchSetup.Valid_IDs.getBranches(0,this.load_ddlBranches_callback);
        }
        
        this.load_ddlBranches_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlBranch", result.value, "BranchCode", "BranchName");
            }
        }
        set_visibility('Main');
     }
     
    function set_visibility(tbl)
    { 
        if(tbl=='ValidID')
        {
            document.getElementById('tblValidID').style.display="block";
            document.getElementById('tblMain').style.display="none";
            document.getElementById('btnBack').style.display="block";
        }
        else
        {
            document.getElementById('tblValidID').style.display="none";
            document.getElementById('tblMain').style.display="block"; 
            document.getElementById('btnBack').style.display="none";
        }
    }
    
    function create_grid()
    {
        dbgrid = new datagrid();
        {
		    var col = new datagrid_column();
		    col.header = 'ID Description';
		    col.datafield = 'IDDescription';
		    col.align = 'right';
		    col.styles.cell += 'width:40%';
		    col.onchange = 'get_inputted_data';
		    dbgrid.addColumn(col);
        }
        
        {
		    var col = new datagrid_column();
		    col.header = 'Required';
		    col.datafield = 'Required';
            col.type = 'check';
		    col.align = 'left';
		    col.styles.cell += 'width:8%';
		    dbgrid.addColumn(col);
        }
        
        {
		    var col = new datagrid_column();
		    col.header = 'Validation DLL';
		    col.datafield = 'ValidationDLL';
		    col.align = 'left';
		    col.styles.cell += 'width:30%';
		    col.editable = false;
		    dbgrid.addColumn(col);
        }
        if (! popup)
        {
	        var col = new datagrid_column();
	        col.header = '';
	        col.datafield = '';
	        col.align = 'center';
	        col.type = 'button';
	        col.value = String.fromCharCode(187);
	        //col.edit_button = true;
	        col.onclick = 'grid_btn_browse';
	        col.styles.cell += 'width:5%';
	        col.styles.datafield += 'width:100%;';
	        dbgrid.addColumn(col);
        }
        
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }
        
//        {
//          var col = new datagrid_column();
//		    col.header = '';
//		    col.datafield = '';
//		    col.align = 'center';
//		    col.type = 'button';
//		    col.value = 'Edit';
//		    col.edit_button = true;
//		    col.styles.cell += 'width:10%';
//		    col.styles.datafield += 'width:100%;';
//		    dbgrid.addColumn(col);
//        }   
        
        
        dbgrid.caption = 'Valid ID';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 12;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
                    
        dbgrid.footer.add_button.none = false;
        dbgrid.footer.add_button.style += 'background-color: lavender;color:maroon;';  
        dbgrid.footer.add_button.onpost = 'disablebutton_onadd';  
        dbgrid.footer.add_button.beforepost = 'add_before_post';        
        
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.onclick = 'save_valid_id';  
        dbgrid.footer.save_button.style += 'background-color: lavender;'; 
        
        dbgrid.footer.delete_button.onclick = 'delete_valid_id';  
        dbgrid.footer.delete_button.none = false;
        dbgrid.footer.delete_button.style += 'background-color: lavender;'; 
        dbgrid.footer.delete_button.beforepost = 'disablebutton_ondelete';
        
        dbgrid.footer.edit_button.none = false;
        dbgrid.footer.edit_button.style += 'background-color: lavender;'; 
        dbgrid.footer.edit_button.onpost = 'disablebutton_onedit';

        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;';
    }
    
    function add_before_post(args)
    {
        var iddesc = dbgrid.getValueAt(args.datarow,0);
        if(trim(iddesc) == null | trim(iddesc) == '')
        {
            alert('ID Description must not be empty');
            dbgrid.curract = '';
            return;
        }
    }
    
    
    
    function disablebutton_onadd(args)
    {
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.cancel_button.value, true);
    }

    function disablebutton_ondelete(args)
    {
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.add_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
    }

    function disablebutton_onedit(args)
    {
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.add_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
    }
    
    function get_inputted_data(args)
    {
        if(isSpecial(document.getElementById(args.control).value))
        {
            alert('Special character(s) not allowed');
            document.getElementById(args.control).value = '';
            return;
        }
    }

    function bind_grid(currgrp)
    {
        if (currgrp.error != null)
        {
            spawn_errpage(currgrp.error);
            return;
        }
        
        create_grid();
        ajax_show_status(true);
        dbgrid.bind(currgrp.value, 'tdValidID', 'gridValidID');
        ajax_show_status(false);
    } 
        
    function populate_rec()
    {
        var brcode = document.getElementById('ddlBranch').value;
        IRSWorld.BranchSetup.Valid_IDs.getValidID(brcode,bind_grid);
        set_visibility('ValidID');
    }
    
    var save_row = -1;    
    function grid_btn_browse(args)
    {
        var div = document.getElementById('divExplore');
        if (save_row == args.gridrow)
        {
            if (div.style.display == 'block') div.style.display = 'none';
            save_row = -1;
            return;
        }
        save_row = args.gridrow;
        var btn = document.getElementById(args.control).parentElement;
        var diff = 70 //(args.gridrow < 5) ? +220 : -20;
        div.style.top = btn.offsetTop + diff;
        div.style.left = btn.offsetLeft - (150);
        div.style.display = 'block';
    }
    
    function fselect()
    {
        var sn = d3.aNodes[d3.selectedNode];
        var npath = '/' + sn._p.name + '/' + sn.name;
        
        dbgrid.setColumn(2, npath);
        
        close_explore();
    }    
    
    function page_setup()
    {
        if (top != self)
        {
            var dumy = "";
            new valid_id_dropdown().Load_Branches();
            init_value();
            //populate_rec()
            //Modules.getIrsModules(dumy, gotIrsModules);
            if (hasQueryStrings(window))
            {
                var fun = getQueryString(window, 'function');
                popup = (fun == 'popup');
            }
        }
        else
        {
            document.irs_loader.submit();
        }
    }
    
    function close_explore()
    {
        var div = document.getElementById('divExplore');
        div.style.display = 'none';
        save_row = -1;
    }
    
    function validate_branch()
    {
        if(document.getElementById('ddlBranch').value != 'null')
        {
            populate_rec();
            //set_visibility('Main');
        }
        else
        {
            alert('Please select branch');
        }
    }
    
    function save_valid_id(args)
    {
        if (args.value == dbgrid.footer.save_button.value)
        {        
            var brcode = document.getElementById('ddlBranch').value;
            var deleted = (args.deletes.length > 0 ? true : false);                
            IRSWorld.BranchSetup.Valid_IDs.applyEdits(args.additions, args.deletes, args.edits,brcode,deleted,updates_saved);
          
            //alert('Record successfully saved');
        }
    }
    
//    function delete_valid_id(args)
//    {
//        if (args.value == dbgrid.footer.delete_button.value)
//        {        
//            var brcode = document.getElementById('ddlBranch').value;  
//            //var deleted = true;              
//            Valid_IDs.applyEdits(args.additions, args.deletes, args.edits,brcode,deleted,updates_deleted);
//            //alert('Record Saved');
//        }
//    }
    
    
      function delete_valid_id(args)
        {
            if (args.value == dbgrid.footer.delete_button.value)
            {  
//                if(confirm('Are you sure you want to delete this record?'))
//                {
//                    var brcode = document.getElementById('ddlBranch').value;                       
//                    //alert(indx);
//                    //Valid_IDs.DeleteValidId(indx,brcode,updates_saved);
//                }
                //updates_saved('');
            }
        }
    
    
    
    function init_value()
    {
        var branch = IRSWorld.BranchSetup.Valid_IDs.getStartUpValue();
        document.getElementById('ddlBranch').value = branch.value;
    }
        
    function updates_saved(cm)
    {   
        if (cm.error != null)
        {
            alert(cm.error);
            populate_rec();
            return;
        }     
       populate_rec();
       alert('Record has been submitted for approval');
    }
    
    function updates_deleted(cm)
    {   
        if (cm.error != null)
        {
            alert(cm.error);
            populate_rec();
            return;
        }     
       populate_rec();
       alert('Record Deleted');
    }
    
    function disable_button(btnid, enable)
    {
        var fbtn = document.getElementById(btnid);
        if (fbtn != null) 
        {
            var x = (enable) ? '' : 'disabled';
            fbtn.disabled = x;
        }
    }