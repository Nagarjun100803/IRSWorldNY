function execute_ajaxgridview_command(param)
{
    if (Internet_Transaction2 != null && Internet_Transaction2 != "undefined")
    {
        Internet_Transaction2.do_ajaxgridview(param);
    }
}

function Internet_Transaction2()
{
    this.CIFID = null;
    this.grid  = null;
    
    this.do_ajaxgridview = function(parameter)
    {        
        if (parameter != null && parameter != "undefined")
        {
            this.process_grid_viewer(parameter);
        }
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_internet_search_remitters_trans~" + this.CIFID;
        this.grid.div_container  = document.getElementById("GRIDTXNHIST");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.get_profile = function()
    {
        var ds = InternetTransaction2.get_profile(this.CIFID).value;
        
        if (ds.Tables[0].Rows.length > 0)
        {
            var fname   = ds.Tables[0].Rows[0].LastName;
            var mname   = ds.Tables[0].Rows[0].MiddleName
            var lname   = ds.Tables[0].Rows[0].FirstName
            var address = ds.Tables[0].Rows[0].ADDRESS;
            var cifid   = ds.Tables[0].Rows[0].CIFID;
            var ciftype = ds.Tables[0].Rows[0].CIFType;
            
            if (ciftype == '1')
            {
                document.getElementById("lblRemFullName").innerHTML = fname + ' ' + mname + ' ' + lname;
            }
            else
            {
                document.getElementById("lblRemFullName").innerHTML = lname;
            }
            document.getElementById("lblRemAddress").innerHTML = address;
        }
    }
    
    this.load_txn_hist = function()
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_internet_search_remitters_trans~" + this.CIFID;
        this.grid.div_container  = document.getElementById("GRIDTXNHIST");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.load_dd_beneficiaries = function()
    {
        InternetTransaction2.getdropdown_source_benename(this.CIFID, this.getdropdown_source_benename_callback)
    }
    
    this.getdropdown_source_benename_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();  
            this.commons.to_load_dropdownlist("ddBeneficiaries", result.value, "SEQ_ID", "BENE_NAME");
            this.commons = null;
        }   
    }
    
}
