var dbgrid; 
var zdt;     

function create_grid()
    {
        // chr10072010 - modified style widths to occupy fully 100% 
        //              for firefox and ie8 browsers.
        dbgrid = new datagrid();
         {
		    var col = new datagrid_column(); //0
		    col.align = 'left';
		    col.header = 'Record ID';
		    col.datafield = 'SEQ_ID';
            col.styles.cell += 'width:5%;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column(); //1
	        col.header = 'Region';
	        col.datafield = 'Region';
	        col.align = 'right';
	        col.styles.cell += 'width:8%';
	        col.onblur = 'is_alpha';
	        dbgrid.addColumn(col);
        }
        
        {
	        var col = new datagrid_column(); // 2
	        col.align = 'left';
	        col.styles.cell += 'width:30%';
	        col.header = 'Province';
	        col.datafield = 'Province';
	        col.onblur = 'is_alpha';
	        dbgrid.addColumn(col);
        }
        
        {
	        var col = new datagrid_column(); //3
	        col.header = 'Municipality';
	        col.datafield = 'Municipality';
	        col.align = 'left';
	        col.styles.cell += 'width:30%';
	        col.onblur = 'is_alpha';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column(); //4
	        col.header = 'Zip Code';
	        col.datafield = 'Zip';
	        col.align = 'center';	
	        col.onblur = 'is_alpha';
	        //col.type = 'select';
		    //col.options = opt_values;	    		
	        col.styles.cell += 'width:12%';
	        dbgrid.addColumn(col);
        }
       
        {
	        var col = new datagrid_column(); //5
	        col.header = 'OSA';
	        col.datafield = 'osa';
	        col.align = 'center';	
	        col.onblur = 'is_alpha';
	        //col.type = 'select';
		    //col.options = opt_values;	    		
	        col.styles.cell += 'width:15%';
	        dbgrid.addColumn(col);
        }
       
        // for footer
        dbgrid.caption = 'Zip Codes';
        dbgrid.footer.add_button.none = false;
        dbgrid.footer.add_button.style += 'background-color: lavender;'; 
        dbgrid.footer.add_button.onpost = 'disablebutton_onadd';  
        dbgrid.footer.add_button.beforepost = 'check_for_dup_nulls';
        
        dbgrid.footer.edit_button.none = false;            
        dbgrid.footer.edit_button.style += 'background-color: lavender;'; 
        dbgrid.footer.edit_button.beforepost = 'check_for_nulls';
        dbgrid.footer.edit_button.onpost = 'disablebutton_onedit';
        
        dbgrid.footer.save_button.none = false;
        dbgrid.footer.save_button.onclick = 'grid_btn_click';  
        dbgrid.footer.save_button.style += 'background-color: lavender;';  
        
        dbgrid.footer.cancel_button.none = false;
        dbgrid.footer.cancel_button.style += 'background-color: lavender;'; 
        dbgrid.row.normal_style = 'background-color: #f7ffff;';          
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
    
    function disablebutton_onedit(args)
    {
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.add_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
    }
    function disablebutton_onadd(args)
    {
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
        disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.cancel_button.value, true);
    }
    
    function is_alpha(args)
    {
        var rowvalues = args.newvalue;
        var ctl = document.getElementById(args.control);
        if(rowvalues != null)
        {
            var regexps = /^([\w\s\,\-\.\: ])*$/;
            var m = regexps.test(rowvalues); 
            if (m != true) 
            {
               alert("Contains invalid characters.");
               ctl.parentElement.focus();
               ctl.select();
            }
            return true;
        }
        return true;
    }
    
    function check_for_nulls(args)
    {
        var region = dbgrid.getValueAt(args.datarow,1);
        var province = dbgrid.getValueAt(args.datarow,2);
        var municipality = dbgrid.getValueAt(args.datarow,3);
        var zipcode = dbgrid.getValueAt(args.datarow,4);
        
        if(region == 'null' | region == '')
        {
            alert('Region must not be empty');
            dbgrid.curract = '';
            return;
        }
        
        else if(province == 'null' | province == '')
        {
            alert('Province must not be empty');
            dbgrid.curract = '';
            return;
        }
        
        else if(municipality == 'null' | municipality == '')
        {
            alert('Municipality must not be empty');
            dbgrid.curract = '';
            return;
        }
        
        else if(zipcode == 'null' | zipcode == '')
        {
            alert('ZipCode must not be empty');
            dbgrid.curract = '';
            return;
        }
        
    }
    
     function check_for_dup_nulls(args)
    {
        var region = dbgrid.getValueAt(args.datarow,1);
        var province = dbgrid.getValueAt(args.datarow,2);
        var municipality = dbgrid.getValueAt(args.datarow,3);
        var zipcode = dbgrid.getValueAt(args.datarow,4);
        var rx = null;
        rx = IRSWorld.BranchSetup.ZipCode.check_municipality(province,municipality).value;
        var rs = null;
        rs = IRSWorld.BranchSetup.ZipCode.check_zipcode(zipcode).value;
        
        
        if(region == 'null' | region == '')
        {
            alert('Region must not be empty');
            dbgrid.curract = '';
            return;
        }
        
        else if(province == 'null' | province == '')
        {
            alert('Province must not be empty');
            dbgrid.curract = '';
            return;
        }
        
        else if(municipality == 'null' | municipality == '')
        {
            alert('Municipality must not be empty');
            dbgrid.curract = '';
            return;
        }
        
        else if(zipcode == 'null' | zipcode == '')
        {
            alert('ZipCode must not be empty');
            dbgrid.curract = '';
            return;
        }
        
        else if (rx.Tables[0].Rows.length > 0)
        {
             var hee = rx.Tables[0].Rows[0].MUNICIPALITY;
        
            if (typeof(hee) != 'undefined')
            {
                  alert("Municipality already exists/active.");
                  dbgrid.curract = '';
                  return;
            }
        }
        
        else if (rs.Tables[0].Rows.length > 0)
        {
             var wee = rs.Tables[0].Rows[0].TRUEZIPCODE;
        
            if (typeof(wee) != 'undefined')
            {
                  alert("ZipCode already exists/active.");
                  dbgrid.curract = '';
                  return;
            }
        }
    }
    
    // Add
   
    function updates_saved(cm)
    {   
          
        if (cm.error != null)
        {
            alert(cm.error.Message);
            
            return;
        }
         else
        {       
             dbgrid.clearChanges();  
             datatable = cm.value
             dbgrid.clearFilter();            
             dbgrid.filter.enabled = false;   
            // dbgrid.refresh(datatable);
        }
       zip_code.load_all_dropdownlist();
    }
           
    function grid_btn_click(args)
    {
        if (args.value == dbgrid.footer.save_button.value)
        {
            IRSWorld.BranchSetup.ZipCode.applyEdits(args.additions, args.deletes, args.edits, updates_saved);
        }
    }

    
    function zip_code()
    {
        this.load_all_dropdownlist = function()
        {       
            ajax_show_status(true);
            document.getElementById('ddRegion').disabled = true;
            document.getElementById('ddProvince').disabled = true;
            this.dropdown_region_filter();
            this.dropdown_province_filter();
            IRSWorld.BranchSetup.ZipCode.get_initial_data(this.get_initial_callback);
        } 
        
        this.get_initial_callback = function(result)
        {
            if (result.error != null)
            {
                spawn_errpage(result.error.Message); //corrected chr 11/21/2010
                return;
            }
            zdt = result.value;
            create_grid();
            dbgrid.bind(zdt, 'tdZiptab', 'gridZiptab');
            document.getElementById('ddRegion').disabled = false;
            document.getElementById('ddProvince').disabled = false;
            ajax_show_status(false);
        }
        
        this.dropdown_region_filter = function()
        {
            IRSWorld.BranchSetup.ZipCode.getdropdown_ddlRegion(this.dropdown_region_filter_callback)
        }
        
        this.dropdown_region_filter_callback = function(result)
        {
            if (result != null && result.value != undefined) // from result != "undefined")
            {
                var commons = new common_method();         
                //var ds = result.value;
                
                if (commons != null && commons != undefined)
                {
                    commons.to_load_dropdownlist("ddRegion", result.value, "region", "Regions"); //ds, "region", "Regions");
                }            
            }   
        }
        
        this.opts_contains = function(opts, ss)
        {
            for (var i = 0; i < opts.length; i++)
            {
                if (opts[i] == ss) return true;
            }
            
            return false;
        }
        
        this.filter_provinces = function(region)
        {
            var opts = new Array();
            var rows = dbgrid.datatable.Rows;
            
            for (var i = 0; i < rows.length; i++)
            {
                var ss = rows[i].Province + '~' + rows[i].Province;
                if (this.opts_contains(opts, ss)) continue;
                
                opts.push(ss);
            }
            var dd = document.getElementById('ddProvince');
            dd.innerHTML = '';
            var opt = document.createElement('option');
            dd.appendChild(opt);
            set_ddl_options(dd, opts);
        }
        
        this.ddregion_change = function()
        {
            var reg = document.getElementById('ddRegion').value;
            if (reg == null) return;
          
           if (reg != "ALL" && reg !='null')
           { 
            dbgrid.clearFilter();            
            dbgrid.filter.enabled = true;
            var filter = new datagrid_filter();
            filter.datafield = 'Region';
            filter.operator = '==';
            filter.value = "'" + reg + "'";
            dbgrid.addFilter(filter);
            dbgrid.refresh(zdt); 
            var prov_opts = this.filter_provinces(reg);
           }
           
           else
           {
                this.dropdown_province_filter();
                dbgrid.clearFilter();  
                dbgrid.refresh(zdt);         
                dbgrid.filter.enabled = false;   
           }
          
            
        }
        
        this.ddprovince_change = function()
        {
            var reg = document.getElementById('ddProvince').value;
            if (reg == null) return;
         
            if(reg != 'null' && reg !="")
            {
                dbgrid.clearFilter();            
                dbgrid.filter.enabled = true;
                var filter = new datagrid_filter();
                filter.datafield = 'Province';
                filter.operator = '==';
                filter.value = "'" + reg + "'";
                dbgrid.addFilter(filter);
                dbgrid.refresh(zdt);   
            }
            else
            {
                this.ddregion_change();
                dbgrid.refresh(zdt);         
                dbgrid.filter.enabled = false;   
            }      
        }
       
        this.search_file = function()
        {
            var searchval = document.getElementById("txtSearchZip").value;
            if (searchval.length == 0)
            {
                dbgrid.clearFilter();            
                dbgrid.filter.enabled = false;
                dbgrid.refresh(zdt);
                return;
            }
            dbgrid.clearFilter();            
            dbgrid.filter.enabled = true;
            var filter = new datagrid_filter();
            filter.datafield = 'Zip';
            filter.operator = '!=';
            filter.value = "'" + searchval +  "z' && row.Zip.indexOf('" +  searchval + "') > -1";
            dbgrid.addFilter(filter);
            dbgrid.refresh(zdt);
        }
    
        this.dropdown_province_filter = function()
        {
            IRSWorld.BranchSetup.ZipCode.getdropdown_ddlProvince(this.dropdown_province_filter_callback)
        }
        
        this.dropdown_province_filter_callback = function(result)
        {
            if (result != null && result.value != undefined) //from result != "undefined")
            {
                var commons = new common_method();         
                //var ds = result.value;
                
                if (commons != null && commons != undefined)
                {
                    commons.to_load_dropdownlist("ddProvince", result.value, "province", "Provinces"); //from ds, "province", "Provinces");
                }            
            }   
        }
    }
    
    
