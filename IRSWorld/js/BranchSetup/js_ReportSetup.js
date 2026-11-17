
 var dbgrid = null;
    var datatable = null;
    var opt_values = new Array(2);
    var pg_no = 1;
    var cls = this;
    opt_values[0] = 'ACTIVE~ACTIVE';
    opt_values[1] = 'NOT ACTIVE~NOT ACTIVE';
    var rowchanged = false;
    // start of grid lookup
     function page_setup()
    {
        if (top == self)
        {
            window.open('./BranchSetupMain.aspx?load=IRSWorld.BranchSetup.ReportSetup.aspx', '_top');
        }
        else if (top.isAuthenticated())
        {
            cls.load_report_lookup();
        }
        else
        {
            spawn_errpage("This module requires login!");
        }      
    }

    
    function get_branchcode()
    {
        var cbo = document.getElementById("ddBranchCode");
          var sid;
          
          if (cbo.selectedIndex == 0)
          {
            return cbo[0].value;
          }
          else
          {
            return cbo[cbo.selectedIndex].value;
          }
                
        
    }
    function change_branch(branchcode)
    {
        IRSWorld.BranchSetup.ReportSetup.load_datagrid(branchcode,load_report_lookup_callback)
    }
    function show_groups()
    {
    
          var cbo = document.getElementById("ddlRepMain");
          var sid;
          
          if (cbo.selectedIndex == 0)
          {
            sid = cbo[0].value;
          }
          else
          {
            sid = cbo[cbo.selectedIndex].value;
          }
                
                
            if (sid != "ALL")
            {
                dbgrid.clearFilter();            
                dbgrid.filter.enabled = true;
                var filter = new datagrid_filter();
                filter.datafield = 'SUBGROUPID';
                filter.operator = '==';
                filter.value = "'" + sid + "'";
                dbgrid.addFilter(filter);
            }
            else
            {
                dbgrid.clearFilter();            
                dbgrid.filter.enabled = false;   
                
            }
               dbgrid.refresh(datatable);
    }
    // end of Grid
    
    function load_ddl()
    {
        var isadmin     =   document.getElementById("txtisadmin").value;
        var branchcode  =   document.getElementById("txtbranchcode").value;
        var bisadmin    =   (isadmin == "true")? 1: 0;
        IRSWorld.BranchSetup.ReportSetup.load_rep_group(branchcode,bisadmin,load_ddl_callback);
    }
    function  load_ddl_callback(result)
    {
        if (result != null && result != "undefined")
        {
            var branchcode  =   document.getElementById("txtbranchcode").value;
            cls.commons = new common_method();         
            cls.commons.to_load_dropdownlist("ddBranchCode", null, "BranchCode", "BranchName",result.value.Tables[1]);
            if (branchcode.length > 0 && branchcode.indexOf(' ') <= -1 )
            cls.commons.to_set_dropdownlist_index("ddBranchCode",branchcode);
            else
            cls.commons.to_set_dropdownlist_index("ddBranchCode","");
            cls.commons = null;
            rep_grp_callback(result.value);
        }
    }
    function rep_grp_callback(result)
    {
        if (result != null && result != "undefined")
        {
            cls.commons = new common_method();         
            cls.commons.to_load_dropdownlist("ddlRepMain", null , "SubGroupID", "TextDisplay",result.Tables[0]);
            cls.commons = null;            
            cls.load_datagrid();
        }
    }
    function load_datagrid()
    {
        var branchcode = this.get_branchcode();
        IRSWorld.BranchSetup.ReportSetup.load_datagrid(branchcode,load_report_lookup_callback)
    }
    function init_state()
    {
        this.load_ddl();
    }
    function load_report_lookup()
    {
        this.load_ddl();        
    }
    function load_report_lookup_callback(res)
    {
        dbgrid = create_grid();
        dbgrid.filter.enabled = false;
        datatable = res.value.Tables[0];
        dbgrid.bind(datatable, 'tdReportLookup', 'tblReportLookup');
        dbgrid.pg_position(this.pg_no);
        show_groups();
    }
    function filter_new_edits(edit_array)
    {
        //var origarray = new Array();
        var i;
        var temp_array = edit_array.split("~");
        var final_array = new Array();
        var fv_fg = "";
        
        if (temp_array.length > 1)
        {
            final_array.push(temp_array[0].toString() + "~")
            var sarr;
            sarr = temp_array[1].split("|");                        
            fv_fg = sarr[1].toString() + sarr[2].toString(); 
            
            var indx;
            if (temp_array.length - 1 == 1)
            {
                final_array.push(temp_array[1].toString());
            }
            for (i  = 1 ; i < temp_array.length ; i++)
            {
                sarr = temp_array[i].split("|");
                fv_fg = sarr[1].toString() + sarr[3].toString(); 
                if (validate_array(temp_array[i],final_array))
                {
                   final_array.push(temp_array[i].toString()  + "~");                    
                }
                   
            }
            
        }
        
        return final_array.toString();
        
    }
    function validate_array(objarray,origarray)
    {
        var sarr;
        var t = true;
        for (i = 1; i < origarray.length ; i++)
        {
            sarr = objarray.split("|");
            tarr = origarray[i].split("|");
            if (sarr[1] + sarr[3]  +  sarr[0] == tarr[1] + tarr[3] + tarr[0])
            {
                t = false;
            }
        }
        
        return t;
    }

    function grid_btn_save(args)
    {
        if (!confirm("Continue with Save?")) return;
        var gridid =    dbgrid.id;
        var cbopgselected = document.getElementById(dbgrid.id + '_pg_select')
        if (cbopgselected.selectedtIndex == 0 )
            cls.pg_no   = 1
        else 
            cls.pg_no = parseInt(cbopgselected[cbopgselected.selectedIndex].innerText);
        if (args.value == dbgrid.footer.save_button.value)
        {
            var targs = filter_new_edits(args.edits);
            IRSWorld.BranchSetup.ReportSetup.applyEdits(args.additions, args.deletes, targs, updates_saved);            
        }
    }
    function updates_saved(rs)
    {   
        if (rs.error != null)
        {
            alert(rs.error);
            return;
        }
        else
        {  
            dbgrid.clearChanges();
            datatable = rs.value
            dbgrid.clearFilter();            
            dbgrid.filter.enabled = false;   
            dbgrid.bind(datatable, 'tdReportLookup', 'tblReportLookup');
            show_groups();
            dbgrid.pg_position(cls.pg_no);
            enable_dropdown();

            /* RFC-31620 */
            var msg = IRSWorld.BranchSetup.ReportSetup.isAdminApproverTag().value;
            if (msg != "") {
                alert(msg);
            }
            else {
                alert("Successfully saved record.");
            }
        }
    }        
    function create_grid()
    {
        gv = new datagrid();
         {
		    var col = new datagrid_column(); //0
		    col.align = 'left';
		    col.header = 'SEQ ID';
		    col.datafield = 'REP_ID';
            col.styles.cell += 'width:5%;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); //0
		    col.align = 'left';
		    col.header = 'SEQ ID';
		    col.datafield = 'SEQ_ID';
            col.styles.cell += 'width:5%;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 1
		    col.header = 'FLD VALUE';
		    col.datafield = 'FldValue'; //used in alias SQL
		    col.align = 'right';
		    col.styles.cell += 'width:15%'; // adjustments by chr
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 2
		    col.align = 'left';
		    col.header = 'Fld Group';
		    col.datafield = 'SUBGROUPID';
		    col.editable = false;
            col.styles.cell += 'width:5%;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 3
		    col.header = 'Description';
		    col.datafield = 'FldDescription';
		    col.align = 'left';
		    col.editable = false;
		    col.styles.cell += 'width:55%'; // adjustments by chr
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'Status';
		    col.datafield = 'status';
		    col.align = 'center';	
		    col.type = 'select';
    		col.options = opt_values;	    		
		    col.styles.cell += 'width:30%'; // adjustments by chr
		    col.styles.datafield += 'width:100%;';  // adjustments by chr
		    gv.addColumn(col);
        }
       {
		    var col = new datagrid_column(); // 5
		    col.align = 'center';
		    col.header = 'Branch Code';
		    col.datafield = 'BRCD';		    
            col.styles.cell += 'width:5%;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
         {
		    var col = new datagrid_column(); // 5
		    col.align = 'center';
		    col.header = 'BranchCode';
		    col.datafield = 'BranchCode';		    
            col.styles.cell += 'width:5%;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
        // for footer
            gv.footer.edit_button.none = false;            
            gv.footer.edit_button.style += 'background-color: lavender;'; 
            gv.footer.edit_button.onclick = 'locked_dropdown';              
            gv.footer.save_button.none = false; 
            gv.footer.save_button.onclick = 'grid_btn_save';  
            gv.footer.save_button.style += 'background-color: lavender;';  
            gv.footer.cancel_button.none = false;
            gv.footer.cancel_button.onclick = 'enable_dropdown'; 
            gv.footer.cancel_button.style += 'background-color: lavender;'; 
            gv.row.normal_style = 'background-color: #f7ffff;';                      
            return gv;
        
    }
    function locked_dropdown()
    {
        var ddlbr = document.getElementById("ddBranchCode")
        var ddldef = document.getElementById("ddlRepMain")
        ddlbr.disabled = true;
        ddldef.disabled = true;
    }
    function enable_dropdown()
    {
        var ddlbr = document.getElementById("ddBranchCode")
        var ddldef = document.getElementById("ddlRepMain")
        ddlbr.disabled = false;
        ddldef.disabled = false;
    }
   