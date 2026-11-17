
function remtrandoortobank()
{
    this.load_all_dropdownlist = function()
    {
         this.dropdown_purpose();
    }
    
    this.dropdown_purpose = function()
    {
        alert();
        RemTranDoorToBank.getdropdown_source("ImplementationType", this.dropdown_purpose_callback)
    }
    
    this.dropdown_purpose_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlDBRemPurpose", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
}
