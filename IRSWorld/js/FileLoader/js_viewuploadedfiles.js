function execute_ajx_command(param)
{
    if (vf != null && vf != "undefined")
    {
        vf.do_ajax_grid_search(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (vf != null && vf != "undefined")
    {
        vf.do_ajaxgridview(param);
    }
}

function js_view_uploaded_files()
{
    this.rSEQ_ID            = null;
    this.fSEQ_ID            = null;
    this.grid_ajax_page     = "../AjxPage/ajaxgrid.aspx";
    this.ajax_grid          = "GRIDCLASSADD";
    this.js_com             = null;

    this.do_ajax_grid_search = function(parameter)
    {
        this.ajax_grid.process_ajax(parameter)
        
        var command_param = null;
        
        if (parameter != null && parameter != 'undefined')
        {
            command_param = parameter.split('$');
            
            if (command_param.length > 0)
            {
                document.getElementById('hdnFld').value = command_param[1];
                
                if (command_paramp[0] == 'ajx_selected')
                {
                    this.fSEQ_ID = command_param[1];
                }
            }
        }
    }
        
    this.btnSearch_click = function()
    {
        var file_type = document.getElementById('ddFileTypeSearch').selectedIndex;
        var file_format = document.getElementById('ddFileFormatSearch').selectedIndex;
        //ajax grid
        var grid_query = null;
        this.ajax_grid = new grid_selector();
        
        grid_query = 'sp~' + file_type + '~'
                    + file_format + '~';
                    //+ 

        this.ajax_grid.open_selection_grid(this.grid_ajax_page, grid_query, 0, this.fSEQ_ID);
    }
}
