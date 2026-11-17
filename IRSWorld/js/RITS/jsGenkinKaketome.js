var cls_Genkin = new Genkin(); 

function execute_ajx_command(param)
{
    if (cls_Genkin != null && cls_Genkin != "undefined")
    { cls_Genkin.do_gridsearch(param); }
    
}


function Genkin()
{
this.ajax_container = null;
this.url            = "../AjxPage/ajaxgrid.aspx";



    this.do_gridsearch = function (parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if(command_param[0] == "ajx_paging")
                {
                    this.ajax_grid.process_ajax(parameter);
                    return;
                }
                if(command_param[0] == "ajx_search")
                {
                    this.ajax_grid.process_ajax(parameter);
                    return;
                }
                
                if (command_param[0] == "ajx_selected")
                {
                    
                    var command_values = command_param[1].split("~"); // TO GET FIELD VALUE                                            
                    this.command_values = command_values[0];
                               
                } 
                
                               
            }
        }
    }
    
    this.search = function()
    {
        var sql = "spGetAllJapanCIF~" + "TOK";
        this.ajax_grid = new grid_selector();                                                     
        this.ajax_grid.open_selection_grid(this.url, sql, 0, this.ajax_container );
    }

}
