var courier_extract = new courier_extract();

function execute_ajaxgridview_command(param)
{
    if (courier_extract != null)
    {
        courier_extract.extract_table_click(param);
    }
}

function courier_extract()
{
    this.grid = null;
    
    this.extract_viewer = function()
    {
    
        if (document.getElementById("ddl_3D_Currency").value != 'null')
        {
            this.grid = new grid_viewer();       
            this.grid.sql_source     = "sp_extract_3D_txn~" + document.getElementById("ddl_3D_Currency").value + "~"; 
            this.grid.div_container  = document.getElementById("div_courier_3d_extract");
            this.grid.datarow_column = 0;
            this.grid.page_size = 10;
            this.grid.process_ajax('ajx_loading$');                               
            this.grid = null;
            
            this.show_3D_summary('div_courier_3d_extract');
        }
        else
        {
            alert('Please select currency for 3D Extract.');
        }
    } 

    this.extract_table_click = function(command_param)
    {
        var s = command_param.split('$');

        if (s != null)
        {
            if (s[0].toUpperCase() == "AJX_SELECTED")
            {
               return;
            }
            else
            {
               return;
            }
        }
    }
    
    this.load_currency = function()
    {
        IRSWorld._3D.Extract_3D.load_currency(this.load_currency_callback);
    }
    
    this.load_currency_callback = function(result)
    {
        document.getElementById('div_courier_3d_extract').style.display = 'none';
        var ddl = new common_method(); ddl.to_load_dropdownlist("ddl_3D_Currency", result.value, "fldvalue", "flddescription", null);
    }
    
    this.show_3D_summary = function(param)
    {
        document.getElementById(param).style.display = 'block';
    }
    
    this.hide_3d_summary = function(param)
    {
        document.getElementById(param).style.display = 'none';
    }

}