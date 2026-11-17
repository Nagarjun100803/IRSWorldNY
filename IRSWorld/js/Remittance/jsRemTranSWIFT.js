function set_sidebar_selected_value(fld_group, fld_value)
{
    RemTran_SWIFT.set_database_column_value(fld_group, fld_value);
}

function RemTran_SWIFT()
{
    this.rem_commons_class = null;
    
    this.construct_side_bar = function(div_container)
    {
        this.rem_commons_class= new rem_commons(); 
        document.getElementById(div_container).innerHTML = this.rem_commons_class.get_side_bar();
        this.execute_txn_product_main_loading();
        this.dropdown_currency();
        this.dropdown_bank();
    }
    
    this.dropdown_currency = function()
    {
        RemTranSWIFT.getdropdown_currency("Currency", this.dropdown_currency_callback)
    }
    
    this.dropdown_currency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlSWIFTRemACCurrency", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.dropdown_bank = function()
    {
        RemTranSWIFT.getdropdown_bank("MegalinkIBFTBank", this.dropdown_bank_callback)
    }
    
    this.dropdown_bank_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlSWIFTRemBankReceiver", result.value, "FldGroup", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.set_side_bar_visibility = function(division_id, anchor_id)
    {
        this.rem_commons_class.get_side_bar_visibility(division_id, anchor_id);
    }
    
    this.execute_txn_product_main_loading = function()
    {
        this.rem_commons_class.do_txn_product_main_loading();
        this.load_remitter_and_beneficiary_info();
    }
    
    this.load_remitter_and_beneficiary_info = function()
    {
        document.getElementById('lblSWIFTRemName').innerText = this.rem_commons_class.remitter_fullname;
        document.getElementById('lblSWIFTRemAddress').innerText = this.rem_commons_class.remitter_address;
        document.getElementById('lblSWIFTRemBeneName').innerText = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblSWIFTRemBeneAddress').innerText = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblSWIFTRemBankName').innerText = this.rem_commons_class.bank_name;
        document.getElementById('lblSWIFTRemBranchInfo').innerText = this.rem_commons_class.branch_info;
        document.getElementById('lblSWIFTRemCityRegion').innerText = this.rem_commons_class.city_region;
        document.getElementById('lblSWIFTRemTARC').innerText = this.rem_commons_class.total_amount;
        document.getElementById('lblSWIFTRemNRARC').innerText = this.rem_commons_class.netremit_amount_rc;
        document.getElementById('lblSWIFTRemNRAPE').innerText = this.rem_commons_class.netremit_amount_pe;
        document.getElementById('lblSWIFTRemNTBACC').innerText = this.rem_commons_class.net_to_beneficiary;
    }
    
    this.load_transaction_view_details = function()
    {
        this.rem_commons_class.load_transaction_view_details()
    }
    
    this.set_database_column_value = function(fld_group, fld_value)
    {
        this.rem_commons_class.set_transaction_values(fld_group, fld_value);
    }
    
    this.generate_mt = function(division_id)
    {
        var div  = document.getElementById(division_id);
        var cset = div.style.display;
        
        div.style.display = (cset == "block") ? "none" : "block";
    }
}