

function execute_ajaxgridview_command(param)
{
    if (machine_setup != null && machine_setup != "undefined")
    {
        machine_setup.do_ajaxgridview(param);
    }
}

function machine_setup()
{
    this.commons                 = null;
    this.generic                 = null;
    this.SEQ_ID                  = null;
    this.MachineName             = null;
    this.MacAddress              = null;
    this.IPAddress               = null;
    this.BranchCode              = null;
    this.TouchScreen             = null;
    this.CardReader              = null;
    this.CashDispenser           = null;
    this.DefaultCurrency         = null;
    this.isDeleted               = null;
    this.message                 = null;
    this.customer_info           = null;    
    this.ajax_grid               = null;
    this.grid                    = null;
    
    this.load_record = function()
    {
        this.pull_server_data();        
    } 
    
     this.load_grid_viewer = function()
    {     
        this.commons = new common_method();  
        this.commons.to_read_only("tblMachineField",true);                      
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;      
        this.grid.sql_source     = "sp_machinesetup~";
        this.grid.div_container  = document.getElementById("MachineGrid");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_machinesetup~";
        this.grid.div_container  = document.getElementById("MachineGrid");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.set_machine_setup = function()
    {
        this.MachineName        = document.getElementById("txtMachineName").value;
        this.MacAddress         = document.getElementById("txtMACAddress").value;
        this.IPAddress          = document.getElementById("txtIPAddress").value;
        this.BranchCode         = document.getElementById("ddBranchCode").value;
        this.DefaultCurrency    = document.getElementById("ddCurrency").value;
        this.CardReader         = false;
        this.TouchScreen        = false;
        this.CashDispenser      = false;
        this.isDeleted  ='0';
         
          
        if (document.getElementById("chkCardReader").value != null && document.getElementById("chkCardReader").checked == true)
        {
            this.CardReader = true;
        }
        if (document.getElementById("chkTouchScreen").value != null && document.getElementById("chkTouchScreen").checked == true)
        {
            this.TouchScreen = true;
        }
        if (document.getElementById("chkCashDispenser").value != null && document.getElementById("chkCashDispenser").checked == true)
        {
            this.CashDispenser = true;
        }
        
        return this.push_data_to_server();
     }   
     
    this.set_machine_setup2 = function()
    {
       
        this.MachineName        = document.getElementById("txtMachineName").value;
        this.MacAddress         = document.getElementById("txtMACAddress").value;
        this.IPAddress          = document.getElementById("txtIPAddress").value;
        this.BranchCode         = document.getElementById("ddBranchCode").value;
        this.DefaultCurrency    = document.getElementById("ddCurrency").value;
        this.CardReader         = false;
        this.TouchScreen        = false;
        this.CashDispenser      = false;
        this.isDeleted  ='1';
         
          
        if (document.getElementById("chkCardReader").value != null && document.getElementById("chkCardReader").checked == true)
        {
            this.CardReader = true;
        }
        if (document.getElementById("chkTouchScreen").value != null && document.getElementById("chkTouchScreen").checked == true)
        {
            this.TouchScreen = true;
        }
        if (document.getElementById("chkCashDispenser").value != null && document.getElementById("chkCashDispenser").checked == true)
        {
            this.CashDispenser = true;
        }
        
        return this.push_data_to_server();
     }   
     
    this.pull_server_data = function()
    {                 
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.BranchSetup.MachineSetup.MACHINE_SETUP_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      
                  
        IRSWorld.BranchSetup.MachineSetup.GET_MACHINESETUP_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
       
    this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                
                this.MachineName         = ds.Tables[0].Rows[0].MachineName;
                this.MacAddress          = ds.Tables[0].Rows[0].MacAddress;
                this.IPAddress           = ds.Tables[0].Rows[0].IPAddress;
                this.BranchCode          = ds.Tables[0].Rows[0].BranchCode;
                this.TouchScreen         = ds.Tables[0].Rows[0].TouchScreen;
                this.CardReader          = ds.Tables[0].Rows[0].CardReader;
                this.CashDispenser       = ds.Tables[0].Rows[0].CashDispenser;
                this.DefaultCurrency     = ds.Tables[0].Rows[0].DefaultCurrency;
              
                

                document.getElementById("txtHiddenSeqID").value      = this.SEQ_ID;
                document.getElementById("txtMachineName").value      = this.MachineName;
                document.getElementById("txtMACAddress").value       = this.MacAddress;
                document.getElementById("txtIPAddress").value        = this.IPAddress;
                document.getElementById("ddBranchCode").value        = this.BranchCode;
                document.getElementById("ddCurrency").value          = this.DefaultCurrency; 
                
                document.getElementById("chkCardReader").value       = "off";
                document.getElementById("chkCardReader").checked     = false;
                document.getElementById("chkTouchScreen").value      = "off";
                document.getElementById("chkTouchScreen").checked    = false;
                document.getElementById("chkCashDispenser").value    = "off";
                document.getElementById("chkCashDispenser").checked  = false;
                    
                   if (document.getElementById("chkCardReader").value != null && this.CardReader == true)
                   {
                        document.getElementById("chkCardReader").value   = "on";
                        document.getElementById("chkCardReader").checked = true;
                   }
                                
                   if (document.getElementById("chkTouchScreen").value != null && this.TouchScreen == true)
                   {
                        document.getElementById("chkTouchScreen").value   = "on";
                        document.getElementById("chkTouchScreen").checked = true;
                   }
                   
                   if (document.getElementById("chkCashDispenser").value != null && this.CashDispenser == true)
                   {
                        document.getElementById("chkCashDispenser").value   = "on";
                        document.getElementById("chkCashDispenser").checked = true;
                   }
            }
        }          
    } 
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "MachineName~"       + this.MachineName;
        data_row[data_row.length]  = "MacAddress~"        + this.MacAddress;
        data_row[data_row.length]  = "IPAddress~"         + this.IPAddress;
        data_row[data_row.length]  = "BranchCode~"        + this.BranchCode;   
        data_row[data_row.length]  = "TouchScreen~"       + this.TouchScreen;
        data_row[data_row.length]  = "CashDispenser~"     + this.CashDispenser;
        data_row[data_row.length]  = "CardReader~"        + this.CardReader;
        data_row[data_row.length]  = "DefaultCurrency~"   + this.DefaultCurrency; 
        data_row[data_row.length]  = "isDeleted~"         + this.isDeleted; 
        return data_row.join("|");
    } 
    
    this.insert_record = function()
    {     
        this.clear();
        IRSWorld.BranchSetup.MachineSetup.insert_record(this.insert_record_callback);
    }  
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.MachineSetup.read_message(); alert(msg.value); return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblMachineField",false);                          
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
        }
    }   
    
    this.update_record = function()
    {     
         if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("Record not found.");return;
        }
        
        IRSWorld.BranchSetup.MachineSetup.update_record(this.SEQ_ID, this.update_record_callback);    
    }     
        
    this.update_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.MachineSetup.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblMachineField",false);                      
            this.commons.to_unlock_button("tblbuttons","~btnDelete~btnSave~btnCancel"); 
            this.commons = null;                        
        }
    }       
   
    this.delete_record = function()
    {    
        IRSWorld.BranchSetup.MachineSetup.commit_record(this.set_machine_setup2(), this.delete_record_callback);  
    }
    
   this.delete_record_callback = function(result)
   {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.MachineSetup.read_message();alert(msg.value);return;
            }
            
            alert("Record deleted successfully");
            this.SEQ_ID = IRSWorld.BranchSetup.MachineSetup.MACHINE_SETUP_SEQ_ID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit"); 
            this.commons = null;
            machine_setup.load_grid_viewer();
        }
   }
    

    this.commit_record = function()
    {
        this.generic = new generic_method();

        if (document.getElementById("txtMachineName").value == "" ||this.generic.is_empty(document.getElementById("txtMachineName").value) == true)
        {
           alert("Please Complete All Necessary Fields (Machine Name)");
           document.getElementById("txtMachineName").focus();
           return false;
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtMachineName").value) == false)
        {
           alert(" Machine Name Contains invalid characters. ");
           document.getElementById("txtMachineName").focus();
           return false;
        }
        if(document.getElementById("txtMACAddress").value == "" ||this.generic.is_empty(document.getElementById("txtMACAddress").value) == true)
        {
            alert("Please Complete All Necessary Fields (MAC Address)");
            document.getElementById("txtMACAddress").focus();
            return false;
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtMACAddress").value) == false)
        {
           alert(" Mac Address Contains invalid characters. ");
           document.getElementById("txtMACAddress").focus();
           return false;
        }
        if(document.getElementById("txtIPAddress").value == ""||this.generic.is_empty(document.getElementById("txtIPAddress").value) == true)
        {
            alert("Please Complete All Necessary Fields (IP Address)");
            document.getElementById("txtIPAddress").focus(); 
            return false;
        }
        if (this.generic.checkipadd(document.getElementById("txtIPAddress").value) == false)
        {
           alert(" Ip Address Contains invalid characters. ");
           document.getElementById("txtIPAddress").focus();
           return false;
        }
        this.generic = null;
        IRSWorld.BranchSetup.MachineSetup.commit_record(this.set_machine_setup(),this.commit_record_callback);        
    }
        
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.MachineSetup.read_message();alert(msg.value);return;
            }
            
            alert("Record committed successfully");
            this.SEQ_ID = IRSWorld.BranchSetup.MachineSetup.MACHINE_SETUP_SEQ_ID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit"); 
            this.commons = null;
            machine_setup.load_grid_viewer();
        }
    }    
    
    this.rollback_record = function()
    {        
        this.clear();
        IRSWorld.BranchSetup.MachineSetup.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.BranchSetup.MachineSetup.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }        
        this.commons = new common_method();
        this.commons.to_read_only("tblMachineField",true);         
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
        this.commons = null;
        return;
    }   
    
    this.clear = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("tblMachineField");
        this.commons = null;
    } 
    
    this.do_ajaxgridview = function(parameter)
    {                        
        if (parameter != null && parameter != "undefined")
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
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit");
                    this.commons.to_read_only("tblMachineField",true);     
                    this.commons = null;
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
    this.load_all_dropdownlist = function()
    {       
       this.dropdown_branchcode();
       this.dropdown_currency();
    } 
    
    this.dropdown_branchcode = function()
    {
        IRSWorld.BranchSetup.MachineSetup.getdropdown_source2("BranchCode", this.dropdown_branchcode_callback)
    }
    
    this.dropdown_branchcode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddBranchCode", result.value, "BranchCode", "BranchCode");
            this.commons = null;
        }
    }
    
    this.dropdown_currency = function()
    {
        IRSWorld.BranchSetup.MachineSetup.getdropdown_source("Currency", this.dropdown_currency_callback)
    }
    
    this.dropdown_currency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddCurrency", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
}