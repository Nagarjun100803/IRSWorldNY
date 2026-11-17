function iflexaccounts()
{
    var SEQ_ID          = null;
    var IflexAccount    = null;
    var Description     = null;
    var GLType          = null;
    this.Role           = null;

    this.ActiveModule = function(divName)
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.ActiveModule(divName);
    }
    
    this.load_iflexaccounts_grid_viewer = function()
    {        
        this.iflexaccounts_grid = new grid_viewer();
        
        if (this.BranchCode.value != "1074")
        {
            this.iflexaccounts_grid.sql_source  = "sp_iflexaccounts_grid~" + this.BranchCode.value;
        }
        else
        {
            this.iflexaccounts_grid.sql_source  = "sp_iflexaccounts_grid_admin~";
        }
        
        this.iflexaccounts_grid.div_container = document.getElementById("divtbl_iflexaccounts");
        this.iflexaccounts_grid.datarow_column = 0;
        this.iflexaccounts_grid.open_grid_view();
        this.grid = null;
    }    
    
    this.do_ajaxgridviewer = function (parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                document.getElementById("txtHidden_iflexaccounts_SEQ_ID").value = command_param[1];

                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    if (this.Role.value != "1")
                    {
                        document.getElementById("btnEditIflexAccounts").disabled = "disable"; 
                    }
                    else
                    {
                        document.getElementById("btnEditIflexAccounts").disabled = ""; 
                    }
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }   

    this.process_grid_viewer = function (parameter)
    {
        this.loadergrid = new grid_viewer();
        this.loadergrid.sql_source     = "sp_iflexaccounts_grid~" + this.BranchCode.value;
        this.loadergrid.div_container  = document.getElementById("divtbl_iflexaccounts");
        this.loadergrid.datarow_column = 0;
        this.loadergrid.process_ajax(parameter);
        this.loadergrid = null;
    }       
    
    this.load_record = function()
    {
        this.pull_server_data()
    }
    
    this.pull_server_data = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.GEFUUpload.GEFUMaintenance.get_seq_id().value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == '' || this.SEQ_ID =='undefined')
        {
            return;
        }
        IRSWorld.GEFUUpload.GEFUMaintenance.GET_IFLEXACCOUNTS_RECORD(this.SEQ_ID, this.pull_server_data_callback);
    }
    
    this.getRole = function()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.getRole();
    }    
    
    this.pull_server_data_callback = function(result)
    {
        if (result!= null && result.value != null)
        {
            var ds = result.value;
            
            if (ds.Tables[0].Rows.length > 0)
            {
    
                this.SEQ_ID         = ds.Tables[0].Rows[0].SEQ_ID;
                this.IflexAccount   = ds.Tables[0].Rows[0].IflexAccount;
                this.Description    = ds.Tables[0].Rows[0].Description;
                this.GLType         = ds.Tables[0].Rows[0].GLType;    
//    
//                if (this.Role.value != "1")
//                {
//                    document.getElementById("btnEditIflexAccounts").disabled = "disable"; 
//                }
//                else
//                {
//                    document.getElementById("btnEditIflexAccounts").disabled = ""; 
//                }
//                    
//                //document.getElementById("btnEditIflexAccounts").disabled    = "";
                document.getElementById("txtHidden_iflexaccounts_SEQ_ID").value = this.SEQ_ID;
                document.getElementById("txtIflexAccount").value                = this.IflexAccount;
                document.getElementById("txtDescription").value                 = this.Description;
                document.getElementById("txtGLType").value                      = this.GLType;

            }
        }
    }
    
    this.update_record = function ()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHidden_iflexaccounts_SEQ_ID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert ("RECORD NOT FOUND."); return;
        }
        
        IRSWorld.GEFUUpload.GEFUMaintenance.update_record(this.SEQ_ID, this.update_record_callback);
    }   
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();
                alert(msg.value);return;
            }
            document.getElementById("txtDescription").disabled = "";                        
            this.commons = new common_method();  
            this.commons.to_unlock_button("tblIFlexAccountsButtons","btnSaveIflexAccounts~btnCancelIflexAccounts");
            this.commons = null;
        }
    }
    
    this.commit_record = function ()
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.commit_record(this.set_IflexAccounts(), this.commit_record_callback);
    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();
                alert(msg.value);
                return;
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");
            this.commons = new common_method();
            this.commons.to_clear_screen("tblIflexAccounts");  
            this.commons.to_read_only("tblIflexAccounts",true);
            this.commons.to_unlock_button("tblIFlexAccountsButtons","~");
            this.commons = null;      

            this.BranchCode = IRSWorld.GEFUUpload.GEFUMaintenance.getBranchCode();

            this.accountingentries_grid = new grid_viewer();
            this.accountingentries_grid.sql_source  = "sp_iflexaccounts_grid~" + this.BranchCode.value;
            this.accountingentries_grid.div_container = document.getElementById("divtbl_iflexaccounts");
            this.accountingentries_grid.datarow_column = 0;
            this.accountingentries_grid.open_grid_view();
            this.grid = null;
        }
    }
    
    this.rollback_record = function()
    {        
        IRSWorld.GEFUUpload.GEFUMaintenance.rollback_record(this.rollback_record_callback);
    }
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();

        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }
        
        this.SEQ_ID == null;
        document.getElementById("txtHidden_iflexaccounts_SEQ_ID").value = "";
        document.getElementById("txtDescription").disabled = "disabled";
        
        this.commons = new common_method();
        this.commons.to_clear_screen("tblIflexAccounts");
        this.commons.to_read_only("tblIflexAccounts",true);
        this.commons.to_unlock_button("tblIFlexAccountsButtons","~");
        this.commons = null;
        return;
    }
    this.set_IflexAccounts = function ()
    {
        IflexAccount    = document.getElementById("txtIflexAccount").value;
        Description     = document.getElementById("txtDescription").value;
        GLType          = document.getElementById("txtGLType").value;
        
        return this.push_data_to_server();
    }
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]   = "IflexAccount~"   + IflexAccount;
        data_row[data_row.length]   = "Description~"    + Description;
        data_row[data_row.length]   = "GLType~"         + GLType;
        
        return data_row.join("$");
    }    
    
    this.getBranchCode = function()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.getBranchCode();
    }
    
    this.check_alpha = function()
    {
        if (event.keyCode == 32 || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 48 && event.keyCode <= 57) ||  (event.keyCode >= 97 && event.keyCode <= 122)) 
        {   
            event.returnValue = true;
        }
        else
        {
            event.returnValue = false;
            alert("Please input valid characters");
        }
    } 
}
