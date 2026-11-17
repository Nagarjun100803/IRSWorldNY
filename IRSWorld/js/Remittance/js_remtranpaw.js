
function RemTran_PAW()
{
    this.load_all_dropdownlist = function()
    {
         this.dropdown_purpose();
         this.dropdown_implementation();
         this.dropdown_bank();
    }
    
    this.dropdown_purpose = function()
    {
        RemTranPAW.getdropdown_source("TranxPurpose", this.dropdown_purpose_callback)
    }
    
    this.dropdown_purpose_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlPAWRemPurpose", result.value, "FldValue", "FldDescription");
            this.commons = null;  
        }
    }
    
    this.dropdown_implementation = function()
    {
        RemTranPAW.getdropdown_source("ImplementationType", this.dropdown_implementation_callback)
    }
    
    this.dropdown_implementation_callback = function(result)
    { 
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlPAWRemImplementation", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.dropdown_bank = function()
    {
        RemTranPAW.getdropdown_bank("MegalinkIBFTBank", this.dropdown_bank_callback)
    }
    
    this.dropdown_bank_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlPAWRemBank", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }

}