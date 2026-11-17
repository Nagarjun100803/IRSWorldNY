var ord3d_maintenance = new ord3d_maintenance();

function execute_ajaxgridview_command(param)
{
    if (ord3d_maintenance != null)
    {
        ord3d_maintenance.ord3d_maintenance_click(param);
    }
}

function execute_ajx_command(param)
{
    if (ord3d_maintenance != null)
    {
        ord3d_maintenance.exec_modal_summary(param);
    }
}

function ord3d_maintenance()
{
    this.grid           = null;
    this.grd_source     = null;
    this.grd_search     = null;
    this.grid_nm        = null;
    this.sequence_id    = null;
    this.zip_seq        = null;
        
    this.ord3d_maintenance_click = function(param)
    {
        var icls = this;
        
        var command_param = param.split("$");

        if (command_param != null)
        {
            if (command_param[0].toUpperCase() == "AJX_SELECTED")
            {
                icls.sequence_id = command_param[1];
                
                var com_param = command_param[1].split("~");
                icls.zip_seq = com_param[1];
                
                if (icls.grid_nm == "div_tbl_maintenance")
                {
                    this.display_osa(icls.sequence_id)
                    return;
                }
                else if (icls.grid_nm == "div_tbl_3Dbranch")
                {
                    this.display_3d_branch(icls.sequence_id)
                    return;
                }
                else if (icls.grid_nm == "div_Threshold")
                {
                    this.display_threshold(icls.sequence_id)
                    return;
                }
            }
        }
        this.div_js_function(icls.grid_nm, param);
    }
    
    this.exec_modal_summary = function(param)
    {
        var command_param = param.split("$");

        if (command_param != null)
        {
            if (command_param[0].toUpperCase() == "AJX_SELECTED")
            {
                var com_param = command_param[1].split("~");
                IRSWorld._3D.OSA_Maintenance.show_zip_details(parseInt(com_param[0]),this.show_zip_details_callback)
                this.grd_search.process_ajax(param);
            }
        }

        this.grd_search.process_ajax(param);
    }   
    
    this.get_zip = function()
    {
        this.grd_source  = "sp_get_zipcode~";
        this.grd_search  = new grid_selector();
        this.grd_search.open_selection_grid(null, this.grd_source, 0 , null);
    }
    
    this.show_div = function(divName)    
    {
        var div = new Array("div_tbl_maintenance","div_tbl_3Dbranch","div_Threshold"); // condensed array
        var divgrid = new Array("div_tbl_osa", "div_tbl_branch","divThreshold" );// condensed array
        var y = 0;      
        
        this.commons = new common_method();
        this.commons.to_clear_screen("tbl_osa_maintenance");  
        this.commons.to_clear_screen("tbl_branch_maintenance");  
        this.commons.to_clear_screen("tbl_threshold");  
        
        this.commons.to_read_only("tbl_osa_buttons",true);
        this.commons.to_unlock_button("tbl_osa_buttons","btn_osa_add~btn_osa_edit");
       
        this.commons.to_read_only("tbl_thresh_buttons",true);
        this.commons.to_unlock_button("tbl_thresh_buttons","btn_thresh_add~btn_thresh_edit");
        
        this.commons = null;
        
        for(y = 0; y < div.length; y++)
        {
            if (divName == div[y])
            {
                document.getElementById(div[y]).style.display = "block";
                
                if (y < 2)
                {
                    document.getElementById(divgrid[y]).style.display = "block"
                }
                this.div_js_function(divName, 'ajx_loading$');
            }
            else
            {
                document.getElementById(div[y]).style.display = "none";

                if (y < 2)
                {
                    document.getElementById(divgrid[y]).style.display   = "none"
                }
            }
        }
    } 
    
    this.div_js_function = function (divName,test)
    {            
        this.jsFunction = null;
        this.grid = null;
        
        switch(divName)
        {
            case "div_tbl_maintenance":
            {        
                this.grid = new grid_viewer();       
                this.grid.sql_source     = "sp_get_osazip~"; 
                this.grid.div_container  = document.getElementById("div_tbl_osa");
                this.grid.datarow_column = 0;
                this.grid.page_size = 10;
                this.grid.process_ajax(test);                               
                document.getElementById("btn_osa_edit").disabled = "disabled";
            
                this.disable_controls('div_tbl_maintenance');
                
                IRSWorld._3D.OSA_Maintenance.load_Courier_OSA(this.load_courier_callback)
                break;
            }
            case "div_Threshold":
            {        
                this.grid = new grid_viewer();       
                this.grid.sql_source     = "sp_get_threshold~"; 
                this.grid.div_container  = document.getElementById("divThreshold");
                this.grid.datarow_column = 0;
                this.grid.page_size = 10;
                this.grid.process_ajax(test);                               
                document.getElementById("btn_thresh_edit").disabled = "disabled";

                this.disable_controls('div_Threshold');
                IRSWorld._3D.OSA_Maintenance.load_thresh_currency(this.load_thresh_currency_callback)
                break;
            }
            case "div_tbl_3Dbranch":
            {
                this.grid = new grid_viewer();        
                this.grid.sql_source     = "sp_get_3dbranch~"; 
                this.grid.div_container  = document.getElementById("div_tbl_branch");
                this.grid.datarow_column = 0;
                this.grid.page_size = 10;
                this.disable_controls('div_tbl_3Dbranch');
                this.grid.process_ajax(test);
                break;
            }
            default: break;
        }
    }
    
    this.display_osa = function(seq_id)
    {
        document.getElementById("btn_osa_edit").disabled = "";
        IRSWorld._3D.OSA_Maintenance.search_osa(parseInt(seq_id), this.display_osa_callback)
    }

    this.display_3d_branch = function(seq_id)
    {
        document.getElementById("btn_osa_edit").disabled = "disabled";
        IRSWorld._3D.OSA_Maintenance.search_branch(parseInt(seq_id), this.display_3d_branch_callback)
    }
            
    this.display_threshold = function(seq_id)
    {
        document.getElementById("btn_thresh_edit").disabled = "";
        IRSWorld._3D.OSA_Maintenance.display_threshold(parseInt(seq_id), this.display_threshold_callback)
    }
       
    this.update_record = function()
    {      
        this.enable('btnZip');
        this.enable('chkOSA');
        this.enable('cboOSA');

        document.getElementById('div_tbl_osa').style.display   = "none";
        
        this.disable('btn_osa_add');
        this.disable('btn_osa_edit');
        this.enable('btn_osa_save');        
        this.enable('btn_osa_cancel');
    }
    
    this.update_threshold = function()
    {
        this.enable('txtAccountNum');
        this.enable('txtProvince');
        this.enable('txtThreshAmount');
        this.enable('cboThreshCourier');
        this.enable('cboThreshCurrency');
        
        document.getElementById('divThreshold').style.display   = "none";
                
        this.disable('btn_thresh_add');
        this.disable('btn_thresh_edit');
        this.enable('btn_thresh_save');        
        this.enable('btn_thresh_cancel');
    }
    
    this.update_cancel = function()
    {     
        this.disable_controls('div_tbl_maintenance');
    }

    this.update_thresh_cancel = function()
    {
        this.disable_controls('div_Threshold');
    }
    
    this.insert_record = function()
    {
    
        this.sequence_id = 0;
        this.zip_seq = 0;
        document.getElementById('txtMunicipality').innerText = "";
        document.getElementById('txtProvince').innerText = "";
        document.getElementById('txtRegion').innerText = "";
        document.getElementById('txtZip').innerText = "";
        document.getElementById('chkOSA').checked = "";
              
        this.enable('btnZip');
        this.enable('chkOSA');
        this.enable('cboOSA');
        
        var ddl1 = new common_method()
        ddl1.to_set_dropdownlist_index('cboOSA', "");
        
        document.getElementById('div_tbl_osa').style.display   = "none";
        
        this.disable('btn_osa_add');
        this.disable('btn_osa_edit');
        this.enable('btn_osa_save');        
        this.enable('btn_osa_cancel');
    }
    
    this.insert_thresh_record = function()
    {
        document.getElementById('txtAccountNum').innerText = "";
        document.getElementById('txtThreshAmount').innerText = "";
        
        var ddl1 = new common_method()
        ddl1.to_set_dropdownlist_index('cboThreshCourier', "");
        ddl1.to_set_dropdownlist_index('cboThreshCurrency', "");
        
        document.getElementById('divThreshold').style.display   = "none";
                
        this.enable('txtAccountNum');
        this.enable('txtThreshAmount');
        this.enable('cboThreshCourier');
        this.enable('cboThreshCurrency');
        
        this.disable('btn_thresh_add');
        this.disable('btn_thresh_edit');
        this.enable('btn_thresh_save');        
        this.enable('btn_thresh_cancel');
    }

    this.save_record = function()
    {
        var icls = this;
        var test_chk;
        
        if (document.getElementById("chkOSA").checked == true)
        {
            this.test_chk = "1";
        }
        else
        {
            this.test_chk = "0";
        }
        
        this.disable_controls('div_tbl_maintenance');

        IRSWorld._3D.OSA_Maintenance.Update_OSA_Table(parseInt(this.zip_seq), this.test_chk, document.getElementById("cboOSA").value,this.save_record_callback)
        
    }
    
    this.save_record_callback = function()
    {
        alert('Save Successfully.');
        //this.show_div('div_tbl_maintenance');
    }
    
    this.save_thresh = function()
    {
        var icls = this;
        var test_chk;
        
        this.disable_controls('div_Threshold');

        IRSWorld._3D.OSA_Maintenance.Update_Thresh_Table(document.getElementById("cboThreshCourier").value, document.getElementById("cboThreshCurrency").value, document.getElementById("txtAccountNum").value, document.getElementById("txtThreshAmount").value, this.save_thresh_callback)
    }
    
    this.save_record_callback = function()
    {
        alert('Save Successfully.');
        //this.show_div('div_tbl_maintenance');
    }
    
    this.disable = function(param)
    {        
        var id = document.getElementById(param);
        var cset = id.disabled;
       
        id.disabled = (cset == "false") ? "true" : "false";
    }
    
    this.enable = function(param)
    {
        var id = document.getElementById(param);
        var cset = id.disabled;
       
        id.disabled = cset == "false";
    }

    this.load_courier_callback = function(result)
    {
        var ddl1 = new common_method(); ddl1.to_load_dropdownlist("cboOSA", result.value, "alias", "Courier", null);
        ddl1.to_set_dropdownlist_index('cboOSA', "");
        ddl1.to_load_dropdownlist("cboThreshCourier", result.value, "alias", "Courier", null);
        ddl1.to_set_dropdownlist_index('cboThreshCourier', "");
    }
        
    this.load_thresh_currency_callback = function(result)
    {
        var ddl2 = new common_method(); ddl2.to_load_dropdownlist("cboThreshCurrency", result.value, "flddescription", "flddescription", null);
        ddl2.to_set_dropdownlist_index('cboThreshCurrency', "");
    }
    
    this.display_osa_callback = function(result)
    {
        document.getElementById('txtMunicipality').innerText = "";
        document.getElementById('txtProvince').innerText = "";
        document.getElementById('txtRegion').innerText = "";
        document.getElementById('txtZip').innerText = "";
        
        if (result.error != null)
        {
            alert(result.error);
        }
        else
        {  
            var ds = result.value;
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {                                 
                var dt = ds.Tables[0];            
                document.getElementById('txtMunicipality').innerText = (dt.Rows[0].municipality == null) ? "" : dt.Rows[0].municipality.toString();
                document.getElementById('txtProvince').innerText = (dt.Rows[0].province == null) ? "" : dt.Rows[0].province.toString();
                document.getElementById('txtRegion').innerText = (dt.Rows[0].region == null) ? "" : dt.Rows[0].region.toString();
                document.getElementById('txtZip').innerText = (dt.Rows[0].truezipcode == null) ? "" : dt.Rows[0].truezipcode.toString();
                
                if (dt.Rows[0].osa_ind == "1")
                {
                    document.getElementById('chkOSA').checked = "checked";
                }
                else
                {
                    document.getElementById('chkOSA').checked = "";
                }
                
                var ddl = new common_method()
                ddl.to_set_dropdownlist_index('cboOSA', dt.Rows[0].courier.toString());
             }
        }
    }
    
    this.display_3d_branch_callback = function(result)
    {
        document.getElementById('txtBranchCd').innerText = "";
        document.getElementById('txtBranchName').innerText = "";
        document.getElementById('txtBranchCourier').innerText = "";
        document.getElementById('txtBranchLoc').innerText = "";
        
        if (result.error != null)
        {
            alert(result.error);
        }
        else
        {  
            var ds = result.value;
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {                                 
                var dt = ds.Tables[0];            
                document.getElementById('txtBranchCd').innerText = (dt.Rows[0].BranchCode == null) ? "" : dt.Rows[0].BranchCode.toString();
                document.getElementById('txtBranchName').innerText = (dt.Rows[0].BranchName == null) ? "" : dt.Rows[0].BranchName.toString();
                document.getElementById('txtBranchCourier').innerText = (dt.Rows[0].CourierCode == null) ? "" : dt.Rows[0].CourierCode.toString();
                document.getElementById('txtBranchLoc').innerText = (dt.Rows[0].Dom_Br_Loc == null) ? "" : dt.Rows[0].Dom_Br_Loc.toString();
             }
        }
    }

    this.display_threshold_callback = function(result)
    {
        document.getElementById('txtAccountNum').innerText = "";
        document.getElementById('txtThreshAmount').innerText = "";
        
        if (result.error != null)
        {
            alert(result.error);
        }
        else
        {  
            var ds = result.value;
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {                                 
                var dt = ds.Tables[0];            
                document.getElementById('txtAccountNum').innerText = (dt.Rows[0].acct_num == null) ? "" : dt.Rows[0].acct_num.toString();
                document.getElementById('txtThreshAmount').innerText = (dt.Rows[0].threshold == null) ? "" : dt.Rows[0].threshold.toString();
                var ddl1 = new common_method()
                ddl1.to_set_dropdownlist_index('cboThreshCourier', dt.Rows[0].courier_cd.toString());
                ddl1.to_set_dropdownlist_index('cboThreshCurrency', dt.Rows[0].currency.toString());
             }
        }
    }
    
    this.show_zip_details_callback = function(result)
    {
        document.getElementById('txtMunicipality').innerText = "";
        document.getElementById('txtProvince').innerText = "";
        document.getElementById('txtRegion').innerText = "";
        document.getElementById('txtZip').innerText = "";
        
        if (result.error != null)
        {
            alert(result.error);
        }
        else
        {  
            var ds = result.value;
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {                                 
                var dt = ds.Tables[0];            
                document.getElementById('txtMunicipality').innerText = (dt.Rows[0].municipality == null) ? "" : dt.Rows[0].municipality.toString();
                document.getElementById('txtProvince').innerText = (dt.Rows[0].province == null) ? "" : dt.Rows[0].province.toString();
                document.getElementById('txtRegion').innerText = (dt.Rows[0].region == null) ? "" : dt.Rows[0].region.toString();
                document.getElementById('txtZip').innerText = (dt.Rows[0].truezipcode == null) ? "" : dt.Rows[0].truezipcode.toString();
             }
        }
    }
    
    this.disable_controls = function(divname)
    {
        if (divname == 'div_tbl_maintenance')
        {      
            this.disable('btnZip');
            this.disable('chkOSA');
            this.disable('cboOSA');

            this.enable('btn_osa_add');
            this.disable('btn_osa_edit');
            this.disable('btn_osa_save');        
            this.disable('btn_osa_cancel');

            document.getElementById('div_tbl_osa').style.display   = "block";
        }
        else if (divname == 'div_Threshold')
        {
            this.disable('txtAccountNum');
            this.disable('txtProvince');
            this.disable('txtThreshAmount');
            this.disable('cboThreshCourier');
            this.disable('cboThreshCurrency');
            
            document.getElementById('divThreshold').style.display   = "block";
                    
            this.enable('btn_thresh_add');
            this.disable('btn_thresh_edit');
            this.disable('btn_thresh_save');        
            this.disable('btn_thresh_cancel');
        }
        else if (divname == 'div_tbl_3Dbranch')
        {
            this.disable('btnZip');
            this.disable('chkOSA');
            this.disable('cboOSA');

            this.enable('btn_osa_add');
            this.disable('btn_osa_edit');
            this.disable('btn_osa_save');        
            this.disable('btn_osa_cancel');

            document.getElementById('div_tbl_osa').style.display   = "block";
            
            this.disable('txtAccountNum');
            this.disable('txtProvince');
            this.disable('txtThreshAmount');
            this.disable('cboThreshCourier');
            this.disable('cboThreshCurrency');
            
            document.getElementById('divThreshold').style.display   = "block";
                    
            this.enable('btn_thresh_add');
            this.disable('btn_thresh_edit');
            this.disable('btn_thresh_save');        
            this.disable('btn_thresh_cancel');
            
        }
    }
}