function cif_other_ids()
{
    this.SEQ_ID = null;
    this.CIFID  = null;
    
    this.load_grid_viewer = function()
    {
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_other_ids~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFOTHERIDSGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_other_ids~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFOTHERIDSGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
}