var oldatatable = null;

 var cls =  this;
 var dbgrid = null;
    var datatable = null;
    var opt_values = new Array(2);
    var pg_no = 1;
    opt_values[0] = 'ACTIVE~ACTIVE';
    opt_values[1] = 'NOT ACTIVE~NOT ACTIVE';
    var rowchanged = false;
    var row_values =
        {
            SEQ_ID       : '',
            FLDVALUE     : '',
            FLDGROUP     : '',
            BRANCHCODE   : ''
            
        };
        
    // start of grid lookup
     function page_setup()
    {
        if (top == self)
        {
            window.open('./BranchSetupMain.aspx?load=BranchLookup.aspx', '_top');
        }
        else if (top.isAuthenticated())
        {   
            this.load_sub_lookup();
        }
        else
        {
            spawn_errpage("This module requires login!");
        }      
    }
    function get_pagectr()
    {
        return document.getElementById("txtpagectr").value;
    }
    function set_pagectr(pgctr)
    {
        document.getElementById("txtpagectr").value = pgctr;
    }
    function get_gridrow_values(row)
    {
        var vals = dbgrid.getValues(row);
        row_values.ID = vals[0]; //ID;
        row_values.SEQ_ID = vals[1]; //SEQ_ID;
        row_values.BRANCHCODE = vals[5]; //BRANCHCODE;
    }
    function show_groups()
    {          
       var cbo = document.getElementById("ddFldGroup")
        if (cbo.length > 0)
        {
            toggle_objects("txtsearch",false);
            toggle_objects("txtsearchby",false);
        }
        else
        {
            toggle_objects("txtsearch",true);
            toggle_objects("txtsearchby",true);
        }
       load_display_by_group();
    }
    // end of Grid
    function init_state()
    {
        this.load_dropdown();
    }
    function load_dropdown()
    {   
        var isadmin     =   document.getElementById("txtisadmin").value;
        var branchcode  =   document.getElementById("txtbranchcode").value;
        var bisadmin    =   (isadmin == "true")? 1: 0;
        IRSWorld.BranchSetup.BranchLookup.get_branchsublookup_data(branchcode,bisadmin, load_dropdown_callback);
    }
    function load_display_by_group()
    {
        var branchcode = get_ddvalue("ddBranchCode");
        var subgroupid = get_ddvalue("ddFldGroup");
      
        IRSWorld.BranchSetup.BranchLookup.get_branchsublookup_bysubgroup(branchcode,subgroupid,load_sub_lookup_callback);
    }
    function get_ddvalue(objid)
    {
          var cbo = document.getElementById(objid);
          var sid;
          
          if (cbo.selectedIndex == 0)
          {
            var s  = cbo[0].value;
            if (s.indexOf("null") > -1 )
            return "";
            else
            return cbo[0].value;
          }
          else
          {
            return cbo[cbo.selectedIndex].value;
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
    
    function load_dropdown_callback(result)
    {
        if (result != null && result != "undefined")
        {
            var branchcode  =   document.getElementById("txtbranchcode").value;
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddBranchCode", null, "BranchCode", "BranchName",result.value.Tables[1]);
            if (branchcode.length > 0 && branchcode.indexOf(' ') <= -1 )
            this.commons.to_set_dropdownlist_index("ddBranchCode",branchcode);
            else
            this.commons.to_set_dropdownlist_index("ddBranchCode",ddBranchCode.value);            
            
            this.commons.to_load_dropdownlist("ddFldGroup", null, "FldGroup", "FldGroup",result.value.Tables[0]);
            this.commons = null;
            var cbo = document.getElementById("ddFldGroup")
            if (cbo.length > 0)
            {
                toggle_objects("txtsearch",false);
            }
            else
            {
                toggle_objects("txtsearch",true);
            }
            load_datagrid();
        }
     
    }
    function toggle_objects(objid,enable)
    {
        document.getElementById(objid).disabled = enable;
    }
    function load_datagrid()
    {
        var branchcode = get_branchcode();
        var subgroup  = get_ddvalue("ddFldGroup");
        IRSWorld.BranchSetup.BranchLookup.load_datagrid(branchcode,subgroup,load_sub_lookup_callback)
    }
    function load_search_datagrid()
    {
        var branchcode = get_branchcode();
        var subgroup   = get_ddvalue("ddFldGroup");
        var searchval  = document.getElementById("txtsearchby").value
        var searchtype = get_ddvalue("ddlColumns");
        IRSWorld.BranchSetup.BranchLookup.load_search_datagrid(branchcode,subgroup,searchval,searchtype,load_sub_lookup_callback)
    }
    
    function load_sub_lookup()
    {
        this.load_dropdown();        
    }
    function load_sub_lookup_callback(res)
    {   
        try
        {
        dbgrid = create_grid();
        datatable = res.value.Tables[0];        
        dbgrid.clearFilter();                    
        dbgrid.filter.enabled = false;  
        dbgrid.bind(datatable, 'tdBRSLookup', 'tblBRSLookup');              
        dbgrid.refresh(datatable);
        oldatatable = datatable;
            if (datatable.Rows.length > 0 )
            {
                try
                {
                    var ctr = cls.get_pagectr();
                    cls.dbgrid.pg_position(ctr);
                }
                catch(e)
                {
                    dbgrid.pg_position(1)
                }
             }
        }
        catch(e)
        {
           load_datagrid();
        }
    }
    /* for editing of buttons */
    function grid_btn_editpost(args)
    {
        if (rowchanged) 
        {
            rowchanged = false;
            return;
        } 
       
        get_gridrow_values(args.gridrow);
        dbgrid.footerButtonClick('grid_edit_values', dbgrid.footer.edit_button.value);
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
            var indx;
            if (temp_array.length - 1 == 1)
            {
                final_array.push(temp_array[1].toString());
            }
            var tempindx;
            var tempstr;
            for (i  = 1 ; i < temp_array.length ; i++)
            {
                i_arr = temp_array[i].split("|");
                tempindx = i_arr[0];
                for(j = 1; j < temp_array.length ; j++)
                {
                    j_arr = temp_array[j].split("|");
                    if (tempindx == j_arr[0])
                    {
                        tempstr = temp_array[j];
                    }
                }
                
                if (validate_array(tempstr,final_array ))
                {
                   final_array.push(tempstr + "~");                    
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
            if (sarr[0] == tarr[0])
            {
                t = false;
            }
        }
        
        return t;
    }
    function ShellSort( array_obj )
    {
         var i, temp, flag = 1, numLength = array_obj.length;
         var d = numLength;
         while( flag || (d > 1))      // boolean flag (true when not equal to 0)
         {
              flag = 0;           // reset flag to 0 to check for future swaps
              d = (d+1) / 2;
              for (i = 0; i < (numLength - d); i++)
            {
                    
                   if (array_obj[i + d] > array_obj[i])
                  {
                          temp = array_obj[i + d];      // swap positions i+d and i
                          array_obj[i + d] = array_obj[i];
                          array_obj[i] = temp;
                          flag = 1;                  // tells swap has occurred
                  }
             }
         }
         return;
}

    function grid_btn_save(args)
    {
    
        if (!confirm("Continue with save?"))
        {
            return;
        }
    
        var gridid =    dbgrid.id;
        var cbopgselected = document.getElementById(dbgrid.id + '_pg_select')
        if (cbopgselected.selectedtIndex == 0 )
            this.pg_no   = 1
        else 
            this.pg_no = parseInt(cbopgselected[cbopgselected.selectedIndex].text);
        if (args.value == dbgrid.footer.save_button.value)
        {
            
            var targs = filter_new_edits(args.edits);
            this.set_pagectr(pg_no);
            IRSWorld.BranchSetup.BranchLookup.applyEdits(args.additions, args.deletes, targs, updates_saved);            
        }
    }
   
    function updates_saved(rs)
    {   
        if (rs.error != null)
        {
            alert(rs.error.Message);
            return;
        }
        else
        {
            alert('Record has been submitted for approval');
             enable_dropdown();
             show_groups();
        }
    }        
    function create_grid()
    {
        gv = new datagrid();
        
        /*{
		    var col = new datagrid_column(); //0
		    col.align = 'left';
		    col.header = '    ';
		    col.type = 'check';
		    col.onclick = "tick_boxes"
		    col.datafield = 'chkstatus';
            col.styles.cell += 'width:auto;';		    
		    gv.addColumn(col);
        }*/
        
         {
		    var col = new datagrid_column(); //0
		    col.align = 'left';
		    col.header = 'SEQ ID';
		    col.datafield = 'SEQ_ID';
            col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 1
		    col.header = 'FLD VALUE';
		    col.datafield = 'FldValue'; //used in alias SQL
		    col.align = 'right';		    
		    col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'CODE';
		    col.datafield = 'FldCode'; //used in alias SQL
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 3
		    col.align = 'left';
		    col.header = 'Fld Group';
		    col.datafield = 'FldGroup';
		    col.editable = false;
            col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 4
		    col.header = 'Description';
		    col.datafield = 'FldDescription';
		    col.align = 'left';
		    col.editable = false;
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 5
		    col.header = 'Status';
		    col.datafield = 'status';
		    col.align = 'center';	
		    col.type = 'select';
    		col.options = opt_values;	    		
		    col.styles.cell += 'width:auto';
		    gv.addColumn(col);
        }
       {
		    var col = new datagrid_column(); // 6
		    col.align = 'center';
		    col.header = 'Branch Code';
		    col.datafield = 'BRCD';		    
            col.styles.cell += 'width:auto;display:none';
		    col.styles.header += 'display:none';
		    col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 7
		    col.header = 'BranchCode';
		    col.datafield = 'BranchCode';
		    col.align = 'left';
		    col.editable = false;
		    col.styles.cell += 'width:0%;display:none';
            col.styles.header += 'display:none';
            col.styles.datafield += 'display:none';
		    gv.addColumn(col);
        }
            //gv.autoPost = false;  
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
        var ddldef = document.getElementById("ddFldGroup")
        ddlbr.disabled = true;
        ddldef.disabled = true;
    }
    function enable_dropdown()
    {
        var ddlbr = document.getElementById("ddBranchCode")
        var ddldef = document.getElementById("ddFldGroup")
        ddlbr.disabled = false;
        ddldef.disabled = false;
    }
   
   function tick_boxes(args)
   {
        alert(args);
   }
   function filter_dropdown(searchstr,combobox)
   {
        var cbo = document.getElementById(combobox);
        
        var s = "";
        
        searchstr = searchstr.toUpperCase();
        document.getElementById("txtsearch").value = searchstr;
        if (cbo.length > 0)
        {
            for (i = 0 ; i < cbo.length; i++)
            {
                s = cbo[i].innerText;
                s = s.toUpperCase();
                if (s.indexOf(searchstr) > -1)
                {
                    cbo.selectedIndex = i;
                    show_groups();
                    return;                    
                }
            }
        }
        
        
   }
   function filter_datagrid(searchstr,combobox)
   {
        var cbo = document.getElementById(combobox);
        
        var s = "";
        
        searchstr = searchstr.toUpperCase();
        document.getElementById("txtsearch").value = searchstr;
        if (cbo.length > 0)
        {
            for (i = 0 ; i < cbo.length; i++)
            {
                s = cbo[i].innerText;
                s = s.toUpperCase();
                if (s.indexOf(searchstr) > -1)
                {
                    cbo.selectedIndex = i;
                    show_groups();
                    return;                    
                }
            }
        }
        
        
   }