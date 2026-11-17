var req_br = new required_branch_id();

function execute_ajaxgridview_command(param)
{
    if (req_br != null)
    {
        req_br.load_branch_req_id(param);
    }
}

function required_branch_id()
{
    this.button_flag = 0;

    this.branch_code = null;
    this.id_seq_id = null;
    this.id_description = null;
    this.id_required = 0;

    this.commit_record = function()
    {
        if (confirm("Are you sure you want to save this record?") == false)
        {
            return;
        }

        this.id_description = document.getElementById("cbo_id_type").value;
        this.id_required = 0;

        if (document.getElementById("chk_required").checked == true)
        {
            this.id_required = 1;
        }

        if (this.branch_code == null || this.branch_code == '')
        {
            alert("Please select branch code."); return;
        }
        
        if (this.id_description == null || this.id_description == '')
        {
            alert("Please select ID type."); return;
        }

        if (this.button_flag == 2 && this.id_seq_id == null || this.id_seq_id == '')
        {
            alert("Please select record from the list to update."); return;
        }

        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to save record."); return;
        }

        if (this.button_flag == 1)
        {
            IRSWorld.TranClass.br_reqids.insert_record(this.branch_code, this.id_description, this.id_required, this.commit_record_callback);
        }
        else if (this.button_flag == 2)
        {
            IRSWorld.TranClass.br_reqids.update_record(this.branch_code, this.id_description, this.id_required, this.id_seq_id, this.commit_record_callback);
        }
    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value == '')
        {
            alert("Record Saved!");

            req_br.button_flag = 0;
            req_br.control_disable(false);             
            req_br.load_branch_req_id();
        }
        else
        {
            alert("System Message\n\n" + result.value);
        }
    }

    this.cancel_record = function()
    {
        this.button_flag = 0;
        this.control_disable(false);
        this.load_branch_req_id();
    }

    this.insert_record = function()
    {
        this.button_flag = 1;
        this.control_disable(true);        
        this.id_seq_id = null;
        this.id_description = null;
        this.id_required = 0;
        
        new common_method().to_set_dropdownlist_index("cbo_id_type", "");
        document.getElementById("chk_required").checked = false;
    }
    
    this.update_record = function()
    {
        this.button_flag = 2;
        this.control_disable(true);    
    }
    
    this.delete_record = function()
    {    
        this.button_flag = 0;

        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to delete record."); return;
        }
        
        if (this.id_seq_id == null || this.id_seq_id == '')
        {
            alert("Please select record to delete."); return;
        }

        if (confirm("Are you sure you want to delete this record?") == false)
        {
            return;
        }
            
        IRSWorld.TranClass.br_reqids.delete_record(this.id_seq_id, this.delete_record_callback);
    }
    
    this.delete_record_callback = function(result)
    {
        if (result != null && result.value != '')
        {
            alert("Encountered error while deleting record. \n\n" + result.value);
        }

        req_br.button_flag = 0;
        req_br.load_branch_req_id();
    }

    this.load_branch_list = function()
    {
        this.control_disable(false);
        
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to load Branch List."); return;
        }
        
        IRSWorld.TranClass.br_reqids.get_branches(this.load_branch_list_callback);
    }
    
    this.load_branch_list_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            var commons = new common_method();

            commons.to_load_dropdownlist("cbo_branch", result.value, "BRANCHCODE", "BRANCHNAME");
            commons = null;
        }
    }

    this.load_id_req = function()
    {
        IRSWorld.TranClass.br_reqids.get_branch_id_lookup(this.branch_code, this.load_id_req_callback)   
    }
    
    this.load_id_req_callback = function(result)
    {
        if (result != null && result.value != null)
        {    
            var commons = new common_method();

            commons.to_load_dropdownlist("cbo_id_type", result.value, "FLDCODE", "FLDESC");
            commons = null;
        }
    }

    this.load_branch_req_id = function(param)
    {
        var grid = new grid_viewer();

        if (param == null | param == '')
        {
            param = 'ajx_loading$';
        }
        
        if (param.indexOf("ajx_selected") >= 0)
        {
            var content = param.split('$')[1].split('~');

            this.id_description = content[0];
            this.id_required = parseInt(content[1]);
            this.id_seq_id = parseInt(content[2]);            

            new common_method().to_set_dropdownlist_index("cbo_id_type", content[0]);

            if (parseInt(this.id_required) > 0)
            {
                document.getElementById("chk_required").checked = true
            }
            else
            {
                document.getElementById("chk_required").checked = false;
            }

            return;
        }

        grid.sql_source     = "branch_req_id_list~" + this.branch_code;
        grid.div_container  = document.getElementById("div_branch_req_id");
        grid.datarow_column = 0;
        grid.page_size      = 10;
        grid.process_ajax(param);
        grid = null;
        
        if (param == 'ajx_loading$')
        {
            this.load_id_req();
            this.id_seq_id = null;
            this.id_description = null;
            this.id_required = 0;

            new common_method().to_set_dropdownlist_index("cbo_id_type", "");
            document.getElementById("chk_required").checked = false;
        }
    }
    
    this.control_disable = function(bool)
    {
        document.getElementById("btn_add").disabled  = bool;
        document.getElementById("btn_edit").disabled = bool;        
        document.getElementById("btn_delete").disabled  = bool;
        document.getElementById("btn_cancel").disabled = !bool;
        document.getElementById("btn_save").disabled = !bool;
    }
}