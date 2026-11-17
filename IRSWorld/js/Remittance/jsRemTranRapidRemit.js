
function set_side_bar_selected_value(fld_group, fld_value)
{
    rapid_remit.set_database_column_value(fld_group, fld_value);
}

function cancel_transaction()
{
    rapid_remit.cancel_transaction();
}

function rem_rapid_remit()
{
    this.rem_commons_class  = null;

    this.cancel_transaction = function()
    {
        this.rem_commons_class.cancel_transaction();
    }

    this.construct_side_bar = function(div_container)
    {
        this.rem_commons_class = new rem_commons();
        this.execute_txn_product_main_loading();

        document.getElementById(div_container).innerHTML = this.rem_commons_class.get_side_bar();        
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
        document.getElementById('lblRRRemName').innerText            = this.rem_commons_class.remitter_fullname;
        document.getElementById('lblRRRemAddress').innerText         = this.rem_commons_class.remitter_address;
        document.getElementById('lblRRRemBeneName').innerText        = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblRRRemBeneAddress').innerText     = this.rem_commons_class.beneficiary_address;
        document.getElementById('lblRRRemBeneACCurrency').innerText  = this.rem_commons_class.beneficiary_account_currency;
        document.getElementById('lblRRRemBeneAccountType').innerText = this.rem_commons_class.beneficiary_account_type;
        document.getElementById('lblRRRemBeneCASANo').innerText      = this.rem_commons_class.beneficiary_account_number;
    }

    this.load_transaction_view_details = function()
    {
        this.rem_commons_class.load_transaction_view_details();
    }

    this.set_database_column_value = function(fld_group, fld_value)
    {
        this.rem_commons_class.set_transaction_values(fld_group, fld_value);
    }
}

        
