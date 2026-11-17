
function execute_ajx_command(param)
{
    if (exceptions != null && exceptions != undefined)
    {
         exceptions.do_ajax(param);
    }
}
function execute_ajaxgridview_command(param)
{
    if (exceptions != null && exceptions != undefined)
    {
         exceptions.do_ajaxgridview(param);
    }
}

function exceptions()
{
    this.commons                  = null;
    this.generic                  = null;
    this.SEQ_ID                   = null;
    this.CIFID                    = null;
    this.RemitterName             = null;
    this.LastName                 = null;
    this.FirstName                = null;
    this.MiddleName               = null;
    this.BranchCode               = null;
    this.EReason                  = null;
    this.CReason                  = null;
    this.DateCreated              = null;
    this.DateModified             = null;
    this.Status                   = null;
    this.Remarks                  = null;
    this.message                  = null;
    this.grid                     = null;
    this.grid2                    = null;
    this.grid_ajax_page           = "../AjxPage/ajaxgridview.aspx";
    this.ajax_grid                = "GRIDCLASSADD";
    this.grid_ajax_query          = null;
    this.grid_ajax_page           = "../AjxPage/ajaxgrid.aspx";
    this.negBranchCode            = null;
    
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();
        this.commons.to_read_only("tblExceptions1",true);
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;
        this.grid = new grid_viewer();
        this.grid.session_end = true;
        this.grid.sql_source     = "sp_exceptions~";
        this.grid.div_container  = document.getElementById("divException");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }

    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_exceptions~";
        this.grid.div_container  = document.getElementById("divException");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }

    this.load_record = function()
    {                     
         this.pull_server_data();  
    }
  
    this.set_exceptions = function()
    {   
        this.commons = new common_method();
        this.CIFID            = document.getElementById("txtRemitterID").value;
        this.LastName         = document.getElementById("txtLastName").value;
        this.FirstName        = document.getElementById("txtFirstName").value;
        this.MiddleName       = document.getElementById("txtMiddleName").value;
        this.RemitterName     = this.LastName + ', ' + this.FirstName  + ' ' + this.MiddleName;
        this.EReason          = document.getElementById("txtEReason").value;
        this.CReason          = document.getElementById("txtCReason").value;
        this.Status           = '1';
        this.BranchCode       = IRSWorld.BranchSetup.Exceptions.getUserLoc().value;

        if(document.getElementById("txtDateCreated").value == 'null' ||document.getElementById("txtDateCreated").value == "" ||document.getElementById("txtDateCreated").value == 'undefined')
        {
            this.DateCreated = get_current_date_string();
        }
        else 
        {
            this.DateCreated = document.getElementById("txtDateCreated").value;
        }

        if (document.getElementById("txtEReason").value != 'null' && document.getElementById("txtCReason").value == "")
        {
            this.Status           = '1';
        }
        else
        {
            this.Status           = '0';
        }
       
        this.DateModified     = get_current_date_string();
        this.Remarks          = document.getElementById("txtRemarks").value;
        this.commons = null;
        
        return this.push_data_to_server();
    }  
    
    this.set_exceptions2 = function()
    {   
        this.commons = new common_method();
        this.CIFID            = document.getElementById("txtRemitterID").value;
        this.LastName         = document.getElementById("txtLastName").value;
        this.FirstName        = document.getElementById("txtFirstName").value;
        this.MiddleName       = document.getElementById("txtMiddleName").value;
        this.RemitterName     = this.LastName + ', ' + this.FirstName  + ' ' + this.MiddleName;
        this.EReason          = document.getElementById("txtEReason").value;
        this.CReason          = document.getElementById("txtCReason").value;
        this.Status           = '0';
        this.BranchCode       = IRSWorld.BranchSetup.Exceptions.getUserLoc().value;

        if(document.getElementById("txtDateCreated").value == 'null' ||document.getElementById("txtDateCreated").value == "" ||document.getElementById("txtDateCreated").value == 'undefined')
        {
            this.DateCreated          = get_current_date_string();
        }
        
        if(document.getElementById("txtDateCreated").value == 'null' ||document.getElementById("txtDateCreated").value == "" ||document.getElementById("txtDateCreated").value == 'undefined')
        {
            this.DateCreated          = get_current_date_string();
        }
        else 
        {
            this.DateCreated = document.getElementById("txtDateCreated").value;
        }
        
        this.DateModified     = get_current_date_string();
        this.Remarks          = document.getElementById("txtRemarks").value;
        this.commons = null;
        
        return this.push_data_to_server();
    }            
    
    this.pull_server_data = function()
    {          
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID =  IRSWorld.BranchSetup.Exceptions.EXCEPTION_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.BranchSetup.Exceptions.GET_EXCEPTION_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }        
    
    this.pull_server_data_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            var ds = result.value;

            if (ds.Tables[0].Rows.length > 0)
            {
                    this.CIFID           = ds.Tables[0].Rows[0].CIFID;
                    this.RemitterName    = ds.Tables[0].Rows[0].RemitterName;
                    this.LastName        = ds.Tables[0].Rows[0].LastName;
                    this.FirstName        = ds.Tables[0].Rows[0].FirstName;
                    this.MiddleName       = ds.Tables[0].Rows[0].MiddleName;
                    this.EReason         = ds.Tables[0].Rows[0].EReason;
                    this.CReason         = ds.Tables[0].Rows[0].CReason;
                    this.DateCreated     = ds.Tables[0].Rows[0].DateCreated;
                    this.DateModified    = ds.Tables[0].Rows[0].DateModified;
                    this.Status          = ds.Tables[0].Rows[0].Status;
                    this.Remarks         = ds.Tables[0].Rows[0].Remarks;
                    this.BranchCode      = ds.Tables[0].Rows[0].BranchCode;
                    
                    document.getElementById("txtHiddenSeqID").value = this.SEQ_ID;
                    document.getElementById("txtRemitterID").value  = this.CIFID;
                    document.getElementById("txtLastName").value    = this.LastName;
                    document.getElementById("txtFirstName").value   = this.FirstName;
                    document.getElementById("txtMiddleName").value  = this.MiddleName;
                    document.getElementById("txtEReason").value     = this.EReason;
                    document.getElementById("txtCReason").value     = this.CReason;
                    document.getElementById("txtRemarks").value     = this.Remarks;
                    document.getElementById("txtDateCreated").value = dateToString(this.DateCreated);
                    
                    this.commons = new common_method();
                    this.generic = new generic_method();
                    this.generic.to_clear_null("txtRemarks~txtCReason~txtEReason");
                    this.commons = null; 
                    this.generic = null;
            }
        }       
    }
    
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
             
        data_row[data_row.length]  = "CIFID~"               + this.CIFID;
        data_row[data_row.length]  = "RemitterName~"        + this.RemitterName;
        data_row[data_row.length]  = "LastName~"            + this.LastName;
        data_row[data_row.length]  = "FirstName~"           + this.FirstName;
        data_row[data_row.length]  = "MiddleName~"          + this.MiddleName;
        data_row[data_row.length]  = "BranchCode~"          + this.BranchCode;
        data_row[data_row.length]  = "EReason~"             + this.EReason;
        data_row[data_row.length]  = "CReason~"             + this.CReason;
        data_row[data_row.length]  = "DateCreated~"         + this.DateCreated;
        data_row[data_row.length]  = "DateModified~"        + this.DateModified;
        data_row[data_row.length]  = "Status~"              + this.Status;
        data_row[data_row.length]  = "Remarks~"             + this.Remarks;
        
        return data_row.join("|");       
    }
    
    this.insert_record = function()
    {     
        document.getElementById("txtHiddenSeqID").value = 0;
        document.getElementById("txtHiddenSeqID2").value = 0;
        this.clear();
        this.enable();
        IRSWorld.BranchSetup.Exceptions.insert_record(this.insert_record_callback);
    }
    
    this.insert_record_callback = function(result)
    {
        if (result != null && result.value != undefined)
        {
            if (result.value == false)
            {
                var msg =  IRSWorld.BranchSetup.Exceptions.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();       
            this.commons.to_read_only("tblExceptions1",false);  
            document.getElementById("txtCReason").disabled = true;
            this.commons.to_unlock_button("tblbuttons", "btnSave~btnCancel");
            this.commons.to_unlock_button("tblExceptions1", "btnSearch");
            this.commons = null;
        }
    }
    
    this.update_record = function()
    {
        document.getElementById("txtHiddenSeqID2").value = '';
        document.getElementById("txtCReason").disabled = false;    
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        this.disable();
        IRSWorld.BranchSetup.Exceptions.update_record(this.SEQ_ID, this.update_record_callback);
    }

    this.update_record_callback = function(result)
    {
        if (result != null && result.value != undefined)
        {
            if (result.value == false)
            {
                var msg =  IRSWorld.BranchSetup.Exceptions.read_message();
                alert(msg.value);return;
            }

            this.commons = new common_method();
            this.commons.to_read_only("tblExceptions1",false);
            this.commons.to_unlock_button("tblbuttons", "btnSave~btnCancel"); 
            this.commons = null;
        }
    }

    this.commit_record = function()
    {
        if (typeof(this.validate_input) != 'undefined')
        {
            if (this.validate_input() == false)
            {
                alert(this.error_input); return;
            }
        }

        IRSWorld.BranchSetup.Exceptions.commit_record(this.set_exceptions(), this.commit_record_callback);
        document.getElementById("txtHiddenSeqID").value = 0;
        document.getElementById("txtHiddenSeqID2").value = '';
    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != undefined)
        {        
            if (result.value == false)
            {
                var msg =  IRSWorld.BranchSetup.Exceptions.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID =  IRSWorld.BranchSetup.Exceptions.EXCEPTION_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete"); 
            this.commons = null;
            alert("RECORD COMITTED SUCCESSFULLY");
            exceptions.load_grid_viewer();
            exceptions.clear();
        }
    }
    
    this.rollback_record = function()
    {
        IRSWorld.BranchSetup.Exceptions.rollback_record(this.rollback_record_callback);
        this.commons = new common_method();
        this.clear();
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg =  IRSWorld.BranchSetup.Exceptions.read_message();
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }
        this.commons = new common_method();
        this.commons.to_read_only("tblExceptions1", true);
        this.commons.to_unlock_button("tblbuttons", "btnAdd~");
        this.commons = null;
    }
    
    this.set_deleted = function()
    {
        IRSWorld.BranchSetup.Exceptions.commit_record(this.set_exceptions2());
        this.clear();
    }
    
    this.delete_record = function()
    {
        IRSWorld.BranchSetup.Exceptions.delete_record(this.SEQ_ID, this.delete_record_callback);
        this.set_deleted(); 
    }

    this.delete_record_callback = function(result)
    {
        if (result != null && result.value != undefined)
        {
            if (result.value == false)
            {
                var msg =  IRSWorld.BranchSetup.Exceptions.read_message(); alert(msg.value); return;
            }
            this.SEQ_ID =  IRSWorld.BranchSetup.Exceptions.EXCEPTION_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");
            this.commons = null;
            alert("RECORD DELETED SUCCESSFULLY");
            exceptions.load_grid_viewer();
        }
    }
    
    this.clear = function()
    {   
        document.getElementById("txtHiddenSeqID").value = 0;
        document.getElementById("txtHiddenSeqID2").value = 0;
        this.commons = new common_method();
        this.commons.to_clear_screen("tblExceptions1");
        this.commons = null;
    }
    
    this.do_ajaxgridview = function(parameter)
    {
        var command_param = null;
        this.ajax_grid.process_ajax(parameter);
        
        if (parameter != null && parameter != undefined)
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID").value = command_param[1];
                
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");
                    this.commons.to_read_only("tblExceptions1",true);
                    this.commons = null;
                    this.load_record();
                }
                else
                {
                    this.process_grid_viewer(parameter);
                }
            }
        }
    }
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
    }
    
    this.panel_search_exception = function()
    {
        this.commons = new common_method();
        this.generic = new generic_method();
        if(document.getElementById("txtLastName").value == "" 
            && this.generic.is_empty(document.getElementById("txtLastName").value) == true 
            && document.getElementById("txtFirstName").value == "" 
            && this.generic.is_empty(document.getElementById("txtFirstName").value) == true 
            && document.getElementById("txtRemitterID").value == "" 
            && this.generic.is_empty(document.getElementById("txtRemitterID").value) == true)
        {
            alert("Please enter criteria to search.\n");
            return false;
        }
        this.negBranchCode  = IRSWorld.BranchSetup.Exceptions.getNegBranchCode().value;
        
        this.ajaxTimeout();
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "sp_exceptionsearch~" + 
                                document.getElementById("txtRemitterID").value + "~" + 
                                document.getElementById("txtLastName").value + "~" +
                                document.getElementById("txtFirstName").value + "~" +
                                document.getElementById("txtMiddleName").value + "~" +
                                this.negBranchCode;
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.SEQ_ID);
    }
   
    this.do_ajax = function(parameter)
    {
        this.disable();
        this.ajax_grid.process_ajax(parameter);
        var command_param = null;
        
        if (parameter != null && parameter != undefined)
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID2").value = command_param[1];
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.ajaxTimeout();
                    IRSWorld.BranchSetup.Exceptions.get_remitter(this.SEQ_ID, this.get_remitter_callback);
                }
            }
        }
    }
    
    //Edited by Troy 20121120
    this.get_remitter_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            var ds = null;
            ds = result.value;
            
            this.CIFID       = ds.Tables[0].Rows[0].CIFID;
            this.LastName    = ds.Tables[0].Rows[0].LastName;
            this.FirstName   = ds.Tables[0].Rows[0].FirstName;
            this.MiddleName  = ds.Tables[0].Rows[0].MiddleName;
            
            document.getElementById("txtRemitterID").value  = this.CIFID;
            document.getElementById("txtLastName").value    = this.LastName;
            document.getElementById("txtFirstName").value   = this.FirstName;
            document.getElementById("txtMiddleName").value  = this.MiddleName;
        }
    }
    
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != undefined)
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID").value = command_param[1];
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");
                    this.commons.to_read_only("tblExceptions1",true);
                    this.commons = null;
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
    this.disable = function()
    {
        document.getElementById("txtRemitterID").readOnly = true;
        document.getElementById("txtLastName").readOnly = true;
        document.getElementById("txtFirstName").readOnly = true;
        document.getElementById("txtMiddleName").readOnly = true;
    }
     
    this.enable = function()
    {
        document.getElementById("txtRemitterID").readOnly = false;
        document.getElementById("txtLastName").readOnly = false;
        document.getElementById("txtFirstName").readOnly = false;
        document.getElementById("txtMiddleName").readOnly = false;
    }
     
    this.validate_input = function()
    {   
        this.error_input = '';
        this.CIFID  = document.getElementById("txtRemitterID").value;   
        var rs = null;
        rs = IRSWorld.BranchSetup.Exceptions.check_remitter(this.CIFID).value;
       
        this.commons = new common_method();
        this.generic = new generic_method();

        if (this.generic.is_alphanumeric(document.getElementById("txtRemitterID").value) == false)
        {
            this.error_input += " Remitter ID Contains invalid characters. \n";
            document.getElementById("txtRemitterID").focus();
            return false;
        }
        
        if ((document.getElementById("txtHiddenSeqID2").value).length > 0 && document.getElementById("txtHiddenSeqID2").value >= '0')
        {
            if (rs.Tables[0].Rows.length > 0)
            {
                var wee = rs.Tables[0].Rows[0].CIFID;
            }
            if (typeof(wee) != 'undefined')
            {
                this.error_input += "Remitter already exists/active.\n";
                document.getElementById("txtRemitterID").focus();
                return false;
            }
        }
        
        if(document.getElementById("txtLastName").value == ""||this.generic.is_empty(document.getElementById("txtLastName").value) == true)
        {
            this.error_input += "Please Complete Necessary Fields. (Last Name)\n";
            document.getElementById("txtLastName").focus();
            return false;
        }
        
        if(document.getElementById("txtFirstName").value == ""||this.generic.is_empty(document.getElementById("txtFirstName").value) == true)
        {
            this.error_input += "Please Complete Necessary Fields. (First Name)\n";
            document.getElementById("txtFirstName").focus();
            return false;
        }
        
        if (this.generic.is_alphanumeric2(document.getElementById("txtLastName").value) == false)
        {
            this.error_input += " Last Name Contains invalid characters. \n";
            document.getElementById("txtLastName").focus();
            return false;
        }
        
        if (this.generic.is_alphanumeric2(document.getElementById("txtFirstName").value) == false)
        {
            this.error_input += " First Name Contains invalid characters. \n";
            document.getElementById("txtFirstName").focus();
            return false;
        }
        
        if((document.getElementById("txtEReason").value == "" || this.generic.is_empty(document.getElementById("txtEReason").value) == true) && (document.getElementById("txtCReason").value == "" ||this.generic.is_empty(document.getElementById("txtCReason").value) == true))
        {
            this.error_input += "Please Complete Necessary Fields. (Exception Reason)\n";
            document.getElementById("txtEReason").focus();
            return false;
        }
       
        if (this.error_input != '')
        {
            return false;
        }
        return true; 
    }
}