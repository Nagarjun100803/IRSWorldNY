
function RemTran_PopUpEntry()
{
    this.load_all_dropdownlist = function()
    {
        this.dropdown_bank();

    }
    
    this.dropdown_bank = function()
    {
        RemTranPopUpEntry.getdropdown_bank("MegalinkIBFTBank", this.dropdown_bank_callback)
    }
    
    this.dropdown_bank_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlPopUpEntryRemBank", result.value, "FldGroup", "FldDescription");
            this.commons = null;
        }
    }
}