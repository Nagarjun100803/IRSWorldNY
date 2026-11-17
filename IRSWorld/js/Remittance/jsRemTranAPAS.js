    function set_side_bar_value(fld_group,fld_value)
{
    RemTran_APAS.set_database_column_value(fld_group, fld_value);
}

function RemTran_APAS()
{

    this.rem_commons_class = null;
    
    this.construct_side_bar = function(div_container)
        {
            this.rem_commons_class = new rem_commons(); document.getElementById(div_container).innerHTML = this.rem_commons_class.get_side_bar();
            this.execute_txn_product_main_loading();
        }
        
    this.set_side_bar_visibility = function(division_id, anchor_id)
        {
            this.rem_commons_class.get_side_bar_visibility(division_id,anchor_id);
        }
        
    this.execute_txn_product_main_loading = function()
        {
            this.rem_commons_class.do_txn_product_main_loading();
            this.load_remitter_and_beneficiary_info();
        }
        
    this.load_remitter_and_beneficiary_info = function()
        {
            document.getElementById('lblAPASRemName').innerText         = this.rem_commons_class.remitter_fullname;
            document.getElementById('lblAPASRemAddress').innerText      = this.rem_commons_class.remitter_address;
            document.getElementById('lblAPASRemBeneName').innerText     = this.rem_commons_class.beneficiary_fullname;
            document.getElementById('lblAPASRemBeneAddress').innerText  = this.rem_commons_class.beneficiary_address;
            document.getElementById('lblAPASRemTARC').innerText         = this.rem_commons_class.total_amount_rmtccy;
            document.getElementById('lblAPASRemNRARC').innerText        = this.rem_commons_class.net_remit_ammount_rmtccy;
            document.getElementById('lblAPASRemNRAPE').innerText        = this.rem_commons_class.net_remit_amount_phpeqv;
            document.getElementById('lblAPASRemNTBACC').innerText       = this.rem_commons_class.net_to_beneficiary_acccy;
        }
        
    this.load_transaction_view_details = function()
        {
            this.rem_commons_class.load_transaction_view_details();
        }
        
    this.set_database_column_value=function(fld_group,fld_value)
        {
            this.rem_commons_class.set_transaction_values(fld_group,fld_value);
        }
}

