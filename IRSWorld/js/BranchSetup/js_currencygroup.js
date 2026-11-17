    var dbgrid;
    
    function currencygroup_dropdown()
    {
        this.commons           = null;  
        this.Load_Branches = function()
        {
            IRSWorld.BranchSetup.CurrencyGroup.getBranches('', this.load_ddlBranches_callback);
        }
        
        this.load_ddlBranches_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlBranch", result.value, "BranchCode", "BranchName");
            }
        }
        
        this.Load_CurrencyGroup = function()
        {
            IRSWorld.BranchSetup.CurrencyGroup.getCurrencyGroup('', this.load_ddlCurrGrp_callback);
        }
        
        this.load_ddlCurrGrp_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddCurrGrp", result.value, "fldvalue", "flddescription");
            }
        }
        set_visibility('Main');
     }  
     
      // Create Table Grid 
        function create_grid()
        {
            dbgrid = new datagrid();
            {
    		    var col = new datagrid_column();
    		    col.header = 'Used';
    		    col.datafield = 'used';
    		    col.align = 'left';
    		    col.type= 'check';
    		    col.styles.cell += 'width:10%';
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.header = 'Code';
    		    col.datafield = 'fldcode';
    		    col.align = 'center';
    		    col.styles.cell += 'width:10%;';
    		    col.editable = false;
    		    dbgrid.addColumn(col);
            }
           {
    		    var col = new datagrid_column();
    		    col.header = 'Currency';
    		    col.datafield = 'flddescription';
    		    col.align = 'right';
    		    col.styles.cell += 'width:80%';
    		    col.editable = false;
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.header = '';
    		    col.datafield = 'currency';
    		    col.styles.header += 'display:none;';
    		    col.styles.datafield += 'display:none;';
    		    col.align = 'left';
    		    col.styles.cell += 'width:0%;display:none;'
    		 
    		    dbgrid.addColumn(col);
            }
            {
    		    var col = new datagrid_column();
    		    col.header = 'SEQ ID';
    		    col.styles.header += 'display:none;';
    		    col.styles.datafield += 'display:none;';
    		    col.datafield = 'seq_id';
    		    col.align = 'left';
    		    col.styles.cell += 'width:0%;display:none;';
    		    dbgrid.addColumn(col);
            }
            
            dbgrid.caption = 'Currency';
            dbgrid.classNames.table = 'table_body';
            dbgrid.rows_page = 12;
            dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
            dbgrid.footer.edit_button.none = false;
            dbgrid.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
            dbgrid.footer.save_button.none = false;
            dbgrid.footer.save_button.onclick = 'grid_btn_click';  
            dbgrid.footer.save_button.style += 'background-color: lavender;';  
            dbgrid.footer.cancel_button.none = false;
            dbgrid.footer.cancel_button.style += 'background-color: lavender;';
         }
         
         function populate_rec()
         {
            var brcode=document.getElementById('ddlBranch').value;
            var currgrp=document.getElementById('ddCurrGrp').value;
            IRSWorld.BranchSetup.CurrencyGroup.GetGridValue(brcode,currgrp,bind_grid);
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
            set_visibility('Search');
            dbgrid.bind(currgrp.value, 'tdCurrencyGroup', 'gridCurrGrp');
            ajax_show_status(false);
        } 
        
        function set_visibility(tbl)
        { 
            if(tbl=='Search')
            {
                document.getElementById('tblCurrencyGroup').style.display="block";
                document.getElementById('tblMain').style.display="none";
                document.getElementById('btnBack').style.display="block";
            }
            else
            {
                document.getElementById('tblCurrencyGroup').style.display="none";
                document.getElementById('tblMain').style.display="block"; 
                document.getElementById('btnBack').style.display="none";
            }
        }  
        
        function grid_btn_click(args)
        {
            if (args.value == dbgrid.footer.save_button.value)
            {
                var brcode=document.getElementById('ddlBranch').value;
                var currgrp=document.getElementById('ddCurrGrp').value;                            
                IRSWorld.BranchSetup.CurrencyGroup.applyEdits(args.additions, args.deletes, args.edits,brcode,currgrp, updates_saved);
            }
        }
        
        function updates_saved(cm)
        {   
            if (cm.error != null)
            {
                alert(cm.error.Message);
                return;
            }

            alert("Record has been submitted for approval");
            populate_rec();
        }
        
        function page_setup()
        {
            if (top != self)
            {
                var dumy = "";
                new currencygroup_dropdown().Load_Branches();
                new currencygroup_dropdown().Load_CurrencyGroup();
                set_init_value();
            }
            else
            {
                document.irs_loader.submit();
            }
        }
        
        function validate_dropdown()
        {
            var brcode = document.getElementById('ddlBranch').value;
            var currgrp = document.getElementById('ddCurrGrp').value;
            
            if (brcode=='null' | brcode=='' | brcode == 'undefined')
            {
                alert('Please select branch');
            }
            else if (currgrp=='null' | currgrp =='' | currgrp == 'undefined')
            {
                alert('Please select currency group');
            }
            else
            {
                populate_rec();
            }
        }
        
        function set_init_value()
        {
            this.commons = new common_method();
            var loc = IRSWorld.BranchSetup.CurrencyGroup.getStartUpLocation();
            this.commons.to_set_dropdownlist_index('ddlBranch',loc.value);
            this.commons.to_set_dropdownlist_index('ddCurrGrp',1);
        }