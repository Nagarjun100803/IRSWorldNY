function execute_ajaxgridview_command(param)
{
    if (Internet_Main != null && Internet_Main != "undefined")
    {
        Internet_Main.do_ajaxgridview(param);
    }
}    

function Internet_Main()
{
    this.Internet_Sidebar_class = null;
    
    this.construct_side_bar = function(div_container)
    {
        this.Internet_Sidebar_class = new Internet_Sidebar(); 
        document.getElementById(div_container).innerHTML = this.Internet_Sidebar_class.get_side_bar();
    }
      
    this.set_side_bar_visibility = function(division_id, anchor_id)
    {
        this.Internet_Sidebar_class.get_side_bar_visibility(division_id, anchor_id);
    }
    
    this.fill_label = function()
    {
        this.fill_name();
        this.fill_address();
        this.fill_contact();
    }
  
    this.fill_name = function()
    {
        InternetMain.get_name(document.getElementById("txtHiddenCIFID").value, this.get_names)
    }
    
    this.get_names = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetMainRemName").innerHTML = res.Tables[0].Rows[0].RemName;
    }
    
    this.fill_address = function()
    {
        InternetMain.get_address(document.getElementById("txtHiddenCIFID").value, this.get_address)
    }
    
    this.get_address = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetMainRemAddress").innerHTML = res.Tables[0].Rows[0].Address;
    }
    
    this.fill_contact = function()
    {
        InternetMain.get_contact(document.getElementById("txtHiddenCIFID").value, this.get_contact)
    }
    
    this.get_contact = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetMainRemContactNo").innerHTML = res.Tables[0].Rows[0].Contact;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_internetmainbene~";
        this.grid.div_container  = document.getElementById("gridInternetMainBenefeciary");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.load_grid_viewer = function()
    {     
        this.commons = new common_method();  
//        this.commons.to_read_only("tblLookUpField",true);                      
//        this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnClear");
        this.commons = null;   
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_internetmainbene~" + document.getElementById("txtHiddenCIFID").value;
        this.grid.div_container  = document.getElementById("gridInternetMainBenefeciary");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.do_ajaxgridview = function(parameter)
    {                        
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenCIFID").value = command_param[1];
            
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
//                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnSearch~btnClear");
                    this.commons = null;
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
}


