function remtranpddts()
{
    this.load_all_dropdownlist = function()
    {
         this.dropdown_purpose();
    }
    this.dropdown_purpose = function()
    {
        RemTranPDDTS.getdropdown_source("TranxPurpose", this.dropdown_purpose_callback)
    }
    this.dropdown_purpose_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlPDDTSRemPurpose", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
}
