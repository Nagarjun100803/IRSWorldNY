
 function set_side_bar_selected_value(fld_group, fld_value)
    {
        RemTran_IBFT.set_database_column_value(fld_group, fld_value);
    }

function RemTran_IBFT()
{
    this.load_all_dropdownlist = function()
    {
         this.dropdown_megalink();
         this.dropdown_account_type();
         this.dropdown_currency();
    }
    
    this.dropdown_megalink = function()
    {
        RemTranIBFT.getdropdown_source("MegalinkIBFTBank", this.dropdown_megalink_callback)
    }
    
    this.dropdown_megalink_callback = function(result)
    { 
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlIBFTRemMegalinkMemberName", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.dropdown_account_type = function()
    {
        RemTranIBFT.getdropdown_accountType("AccountType", this.dropdown_accountType_callback)
    }
    
    this.dropdown_accountType_callback = function(result)
    { 
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlIBFTRemAcctType", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.dropdown_currency = function()
    {
        RemTranIBFT.getdropdown_source("CurrencyALL", this.dropdown_currency_callback)
    }
    
    this.dropdown_currency_callback = function(result)
    { 
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlIBFTRemACCcy", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.rem_commons_class = null;
    
    this.construct_side_bar = function(div_container)
    {
        this.rem_commons_class = new rem_commons();
        document.getElementById(div_container).innerHTML = this.rem_commons_class.get_side_bar();
        this.execute_txn_product_main_loading();
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
        document.getElementById("lblIBFTRemName").innerText = this.rem_commons_class.remitter_fullname;
        document.getElementById("lblIBFTRemAddress").innerText = this.rem_commons_class.remitter_address;
        document.getElementById("lblIBFTRemBeneName").innerText = this.rem_commons_class.beneficiary_fullname;
        document.getElementById("lblIBFTRemBeneAddress").innerText = this.rem_commons_class.beneficiary_address;
        document.getElementById("lblIBFTRemTARC").innerText = this.rem_commons_class.total_amount_rmtccy;
        document.getElementById("lblIBFTRemNRARC").innerText = this.rem_commons_class.net_remit_amount_rmtccy;
        document.getElementById("lblIBFTRemNRAPE").innerText = this.rem_commons_class.net_remit_amount_phpeqv;
        document.getElementById("lblIBFTRemNTBACC").innerText = this.rem_commons_class.net_to_beneficiary_acccy;
    }
    
    this.load_transaction_view_details = function()
    {
        this.rem_commons_class.load_transaction_view_details();
    }
    
    this.set_database_column_value=function(fld_group, fld_value)
    {
        this.rem_commons_class.set_transaction_values(fld_group, fld_value);
    }
}