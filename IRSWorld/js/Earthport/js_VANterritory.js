
function execute_ajaxgridview_command(param)
{
    if (Van_Territory != null && Van_Territory != "undefined")
    {
        Van_Territory.do_ajaxgridview(param);
    }
}

function Van_Territory()
    {
    
        this.commons                    = null;
        this.ajax_grid                  = null;
        this.grid                       = null;
        
        this.var_VanTerritoryID         = null;
            
        this.var_ddCountry              = null;
        this.var_ddCurrency             = null;
        this.var_txtBankName            = null;
        this.var_txtAccountHoldingBank  = null;
        this.var_txtAddress             = null;
        this.var_txtAccountName         = null;
        this.var_txtAccountNumber       = null;
        this.var_txtIBAN                = null;
        this.var_txtSWIFTBIC            = null;
        this.var_txtBSB                 = null;
        this.var_txtBankCode            = null;
        this.var_txtBranchCode          = null;
        this.var_txtCleRIB              = null;
        this.var_txtClearingCode        = null;
        this.var_txtGiroNo              = null;
        this.var_txtSortCode            = null;
        this.var_txtCheckDigit          = null;
        this.var_txtBankGiroCreditNo    = null;
        
        this.isDeleted                  = null;

    this.load_record = function()
    {
        this.pull_server_data();        
    }         
    
    this.load_grid_viewer = function()
        {     
            this.commons = new common_method();  
            this.commons.to_read_only("tblVANField",true);                      
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
            this.commons = null;   
            this.grid = new grid_viewer();    
            this.grid.session_end = true;      
            this.grid.sql_source     = "spz_getVanTerritoryList~";
            this.grid.div_container  = document.getElementById("EarthportTerritoryGrid");
            this.grid.datarow_column = 0;
            this.grid.open_grid_view();
            this.grid = null;
        }
        
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "spz_getVanTerritoryList~";
        this.grid.div_container  = document.getElementById("EarthportTerritoryGrid");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.do_ajaxgridview = function(parameter)
    {                        
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqId").value = command_param[1];
            
                if (command_param[0] == "ajx_selected")
                {
                    this.var_VanTerritoryID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit");
                    this.commons.to_read_only("tblVANField",true);     
                    this.commons = null;
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
     this.pull_server_data = function()
    {                 
        if (this.var_VanTerritoryID == null)
        {
            this.var_VanTerritoryID = VANTerritory.VANTerritoryID().value;
        }
        if (this.var_VanTerritoryID == null ||this.var_VanTerritoryID == '' || this.var_VanTerritoryID == 'undefined')
        {
            return;
        }      
                  
        
        
        IRSWorld.Earthport.VANTerritory.GET_VANTERRITORY_RECORD(Van_Territory.var_VanTerritoryID,Van_Territory.pull_server_data_callback);
    }     
       
    this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                
                this.var_ddCountry              = ds.Tables[0].Rows[0].CountryID;
                this.var_ddCurrency             = ds.Tables[0].Rows[0].Currency;
                this.var_txtBankName            = ds.Tables[0].Rows[0].bank;
                this.var_txtAccountHoldingBank  = ds.Tables[0].Rows[0].accountHolding;
                this.var_txtAddress             = ds.Tables[0].Rows[0].address;
                this.var_txtAccountName         = ds.Tables[0].Rows[0].accountName;
                this.var_txtAccountNumber       = ds.Tables[0].Rows[0].accountNumber;
                this.var_txtIBAN                = ds.Tables[0].Rows[0].IBAN;
                this.var_txtSWIFTBIC            = ds.Tables[0].Rows[0].SWIFTbic;
                this.var_txtBSB                 = ds.Tables[0].Rows[0].BSB;
                this.var_txtBankCode            = ds.Tables[0].Rows[0].bankCode;
                this.var_txtBranchCode          = ds.Tables[0].Rows[0].branchCode;
                this.var_txtCleRIB              = ds.Tables[0].Rows[0].cleRIB;
                this.var_txtClearingCode        = ds.Tables[0].Rows[0].clearingCode;
                this.var_txtGiroNo              = ds.Tables[0].Rows[0].giroNumber;
                this.var_txtSortCode            = ds.Tables[0].Rows[0].sortCode;
                this.var_txtCheckDigit          = ds.Tables[0].Rows[0].checkDigit;
                this.var_txtBankGiroCreditNo    = ds.Tables[0].Rows[0].bankGiroCreditNumber;
        
             
                
                document.getElementById("txtHiddenSeqID").value      = this.var_VanTerritoryID;
                document.getElementById("ddCountry").value      = this.var_ddCountry;
                document.getElementById("ddCurrency").value      = this.var_ddCurrency;
                document.getElementById("txtBankName").value       = this.var_txtBankName;
                document.getElementById("txtAccountHoldingBank").value        = this.var_txtAccountHoldingBank;
                document.getElementById("txtAddress").value        = this.var_txtAddress;
                document.getElementById("txtAccountName").value        = this.var_txtAccountName;
                document.getElementById("txtAccountNumber").value          = this.var_txtAccountNumber; 
                document.getElementById("txtIBAN").value       = this.var_txtIBAN;
                document.getElementById("txtSWIFTBIC").value       = this.var_txtSWIFTBIC;
                document.getElementById("txtBSB").value       = this.var_txtBSB;
                document.getElementById("txtBankCode").value       = this.var_txtBankCode;
                document.getElementById("txtBranchCode").value       = this.var_txtBranchCode;
                document.getElementById("txtCleRIB").value       = this.var_txtCleRIB;
                document.getElementById("txtClearingCode").value       = this.var_txtClearingCode;
                document.getElementById("txtGiroNo").value       = this.var_txtGiroNo;  
                document.getElementById("txtSortCode").value       = this.var_txtSortCode;  
                document.getElementById("txtCheckDigit").value       = this.var_txtCheckDigit;  
                document.getElementById("txtBankGiroCreditNo").value       = this.var_txtBankGiroCreditNo;  
                   
            }
        }          
    } 
    
    this.set_VANTerritory_Maintenance = function()
    {
           
        this.var_ddCountry              = document.getElementById("ddCountry").value;
        this.var_ddCurrency             = document.getElementById("ddCurrency").value;
        this.var_txtBankName            = document.getElementById("txtBankName").value;
        this.var_txtAccountHoldingBank  = document.getElementById("txtAccountHoldingBank").value;
        this.var_txtAddress             = document.getElementById("txtAddress").value;
        this.var_txtAccountName         = document.getElementById("txtAccountName").value;
        this.var_txtAccountNumber       = document.getElementById("txtAccountNumber").value;
        this.var_txtIBAN                = document.getElementById("txtIBAN").value;
        this.var_txtSWIFTBIC            = document.getElementById("txtSWIFTBIC").value;
        this.var_txtBSB                 = document.getElementById("txtBSB").value;
        this.var_txtBankCode            = document.getElementById("txtBankCode").value;
        this.var_txtBranchCode          = document.getElementById("txtBranchCode").value;
        this.var_txtCleRIB              = document.getElementById("txtCleRIB").value;
        this.var_txtClearingCode        = document.getElementById("txtClearingCode").value;
        this.var_txtGiroNo              = document.getElementById("txtGiroNo").value;
        this.var_txtSortCode            = document.getElementById("txtSortCode").value;
        this.var_txtCheckDigit          = document.getElementById("txtCheckDigit").value;
        this.var_txtBankGiroCreditNo    = document.getElementById("txtBankGiroCreditNo").value;
        
        this.isDeleted  = 0;
    
        return this.push_data_to_server();
     }
     
    this.set_VANTerritory_Maintenance_deleted = function()
    {
           
        this.var_ddCountry              = document.getElementById("ddCountry").value;
        this.var_ddCurrency             = document.getElementById("ddCurrency").value;
        this.var_txtBankName            = document.getElementById("txtBankName").value;
        this.var_txtAccountHoldingBank  = document.getElementById("txtAccountHoldingBank").value;
        this.var_txtAddress             = document.getElementById("txtAddress").value;
        this.var_txtAccountName         = document.getElementById("txtAccountName").value;
        this.var_txtAccountNumber       = document.getElementById("txtAccountNumber").value;
        this.var_txtIBAN                = document.getElementById("txtIBAN").value;
        this.var_txtSWIFTBIC            = document.getElementById("txtSWIFTBIC").value;
        this.var_txtBSB                 = document.getElementById("txtBSB").value;
        this.var_txtBankCode            = document.getElementById("txtBankCode").value;
        this.var_txtBranchCode          = document.getElementById("txtBranchCode").value;
        this.var_txtCleRIB              = document.getElementById("txtCleRIB").value;
        this.var_txtClearingCode        = document.getElementById("txtClearingCode").value;
        this.var_txtGiroNo              = document.getElementById("txtGiroNo").value;
        this.var_txtSortCode            = document.getElementById("txtSortCode").value;
        this.var_txtCheckDigit          = document.getElementById("txtCheckDigit").value;
        this.var_txtBankGiroCreditNo    = document.getElementById("txtBankGiroCreditNo").value;
        
        this.isDeleted  = 1;
    
        return this.push_data_to_server();
     }
     
     
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "bank~"                    + this.var_txtBankName;
        data_row[data_row.length]  = "accountHolding~"          + this.var_txtAccountHoldingBank;   
        data_row[data_row.length]  = "address~"                 + this.var_txtAddress;
        data_row[data_row.length]  = "accountNumber~"           + this.var_txtAccountNumber;
        data_row[data_row.length]  = "accountName~"             + this.var_txtAccountName;
        data_row[data_row.length]  = "IBAN~"                    + this.var_txtIBAN; 
        data_row[data_row.length]  = "SWIFTbic~"                + this.var_txtSWIFTBIC;
        data_row[data_row.length]  = "BSB~"                     + this.var_txtBSB; 
        data_row[data_row.length]  = "bankCode~"                + this.var_txtBankCode; 
        data_row[data_row.length]  = "branchCode~"              + this.var_txtBranchCode; 
        data_row[data_row.length]  = "cleRIB~"                  + this.var_txtCleRIB; 
        data_row[data_row.length]  = "clearingCode~"            + this.var_txtClearingCode; 
        data_row[data_row.length]  = "giroNumber~"              + this.var_txtGiroNo; 
        data_row[data_row.length]  = "sortCode~"                + this.var_txtSortCode; 
        data_row[data_row.length]  = "checkDigit~"              + this.var_txtCheckDigit;  
        data_row[data_row.length]  = "bankGiroCreditNumber~"    + this.var_txtBankGiroCreditNo;
        data_row[data_row.length]  = "isDeleted~"               + this.isDeleted;
        data_row[data_row.length]  = "Currency~"                + this.var_ddCurrency;
        data_row[data_row.length]  = "CountryID~"               + this.var_ddCountry;
        
        return data_row.join("|");
    } 
     

    this.clear = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("tblVANField");
        this.commons = null;
    }   
        
   this.load_all_dropdownlist = function()
    {       
       this.LoadDDLCountry();
       this.LoadDDLRemittingCCY();
    }      
        
   this.LoadDDLCountry = function()
        {
             return IRSWorld.Earthport.VANTerritory.getdropdown_country(this.dropdown_country_callback);     
        }
        
   this.dropdown_country_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddCountry", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
        
   this.LoadDDLRemittingCCY = function()
        {
             return IRSWorld.Earthport.VANTerritory.getdropdown_currency(this.dropdown_currency_callback);     
        }
        
   this.dropdown_currency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddCurrency", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    }
    
    this.insert_record = function()
    {     
        this.clear();
        IRSWorld.Earthport.VANTerritory.insert_record(this.insert_record_callback);
    }
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Earthport.VANTerritory.read_message(); alert(msg.value); return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblVANField",false);                          
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
        }
    }   
    
    this.update_record = function()
    {     
         if (this.var_VanTerritoryID == null)
        {
            this.var_VanTerritoryID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.var_VanTerritoryID == null || this.var_VanTerritoryID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.Earthport.VANTerritory.update_record(this.var_VanTerritoryID, this.update_record_callback);    
    }     
        
    this.update_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Earthport.VANTerritory.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblVANField",false);                      
            this.commons.to_unlock_button("tblbuttons","~btnDelete~btnSave~btnCancel"); 
            this.commons = null;                        
        }
    }       
   
    this.delete_record = function()
    {    
        IRSWorld.Earthport.VANTerritory.commit_record(this.set_VANTerritory_Maintenance_deleted(), this.delete_record_callback);  
    }
    
   this.delete_record_callback = function(result)
   {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Earthport.VANTerritory.read_message();alert(msg.value);return;
            }
            
            alert("RECORD DELETED SUCCESSFULLY");
            this.var_VanTerritoryID = IRSWorld.Earthport.VANTerritory.VANTerritoryID().value;
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
        IRSWorld.Earthport.VANTerritory.commit_record(this.set_VANTerritory_Maintenance(),this.commit_record_callback);        
    }
        
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Earthport.VANTerritory.read_message();alert(msg.value);return;
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");
            this.var_VanTerritoryID = IRSWorld.Earthport.VANTerritory.VANTerritoryID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit"); 
            this.commons = null;
            machine_setup.load_grid_viewer();
        }
    }    
    
    this.rollback_record = function()
    {        
        this.clear();
        IRSWorld.Earthport.VANTerritory.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.Earthport.VANTerritory.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }        
        this.commons = new common_method();
        this.commons.to_read_only("tblVANField",true);         
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
        this.commons = null;
        return;
    }   
 
     
       

 }

