var fem = new FileExtractMonitor();
var commons = new common_method();  
var dbgrid = null;
var datatable = null;
var olddatatable = null;

function FileExtractMonitor()
{

    this.init_state = function()
    {
        if (top == self)
        {
            window.open('./BranchSetupMain.aspx?load=BranchLookup.aspx', '_top');
        }
        else if (top.isAuthenticated())
        {   
            IRSWorld.EOD.FileExtractMonitor.load_init_state(fem.init_state_callback);
        }
        else
        {
            spawn_errpage("This module requires login!");
        }           
    }
    
    this.init_state_callback = function(results)
    {
        commons.to_load_dropdownlist("ddExtractType", null, "VALUE", "CODE",results.value.Tables[0]);        
        commons.to_load_dropdownlist("ddStatus", null, "VALUE", "CODE",results.value.Tables[1]);
        fem.display_grid(results.value.Tables[2]);
    }
    
    
    this.load_extracted_file = function()
    {    
        var valid = fem.validate_selection();
        if (!valid) return;
        var tran_source =   document.getElementById("ddExtractType").value;
        var tranxdate   =   document.getElementById("txtDate").value;
        var status =  document.getElementById("ddStatus").value;
        
        IRSWorld.EOD.FileExtractMonitor.load_extracted_file(tranxdate,tran_source,status,fem.load_extracted_file_callback);
    }
    this.load_extracted_file_callback = function(result)
    {
        fem.display_grid(result);
    }
    this.display_grid = function(dt)
    {
         try
        {
            dbgrid = fem.create_grid();
            datatable = dt;        
            dbgrid.clearFilter();                    
            dbgrid.filter.enabled = false;  
            dbgrid.bind(datatable, 'divGridHolder', 'tblFileExtract');              
            dbgrid.refresh(datatable);
            oldatatable = datatable;
//            if (datatable.Rows.length > 0 )
//            {
//                try
//                {
//                    var ctr = fem.get_pagectr();
//                    fem.dbgrid.pg_position(ctr);
//                }
//                catch(e)
//                {
//                    dbgrid.pg_position(1)
//                }
//             }
        }
        catch(e)
        {
           alert("cannot load grid");
        }
    }
    this.create_grid = function()
    {
        var gv = new datagrid();    
        
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
		    col.header = 'Extract Date';
		    col.datafield = 'extractdate'; //used in alias SQL
		    col.align = 'right';		    
		    col.styles.cell += 'width:auto;';		    
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 2
		    col.header = 'File Name';
		    col.datafield = 'efilename'; //used in alias SQL
		    col.align = 'right';
		    col.styles.cell += 'width:auto';
		    col.editable = false;
		    gv.addColumn(col);
        }
        {
		    var col = new datagrid_column(); // 3
		    col.align = 'left';
		    col.header = 'STATUS';
		    col.datafield = 'filestatus';
		    col.editable = false;
            col.styles.cell += 'width:auto;';		    
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
    
    this.validate_selection = function()
    {
        var extracttype =  document.getElementById("ddExtractType");
        var status =  document.getElementById("ddStatus");
        var tranxdate =  document.getElementById("txtDate");
        
        if (extracttype.value.toString().length == 0)
        {
            alert("Choose Extractype.");
            extracttype.focus();
            return false;
        }
        if (status.value.toString().length == 0)
        {
            alert("Choose Upload Status.");
            status.focus();
            return false;
        }
        if (tranxdate.value.toString().length == 0)
        {
            alert("Choose Tranxdate");            
            return false;
        }
        
        return true;
    }
}