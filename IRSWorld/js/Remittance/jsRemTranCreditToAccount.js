
function set_side_bar_selected_value(fld_group, fld_value)
    {
        RemTranCreditToAccount.set_database_column_value(fld_group, fld_value);
    }

function RemTranCreditToAccount()
    {
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
                document.getElementById("lblCARemName").innerText            = this.rem_commons_class.remitter_fullname;
                document.getElementById("lblCARemAddress").innerText         = this.rem_commons_class.remitter_address;
                document.getElementById("lblCARemBeneName").innerText        = this.rem_commons_class.beneficiary_fullname;
                document.getElementById("lblCARemBeneAddress").innerText     = this.rem_commons_class.beneficiary_address;
                document.getElementById("lblCARemPNBBranch").innerText       = this.rem_commons_class.beneficiary_pnb_branch;
                document.getElementById("lblCARemPNBBranchCASANo").innerText = this.rem_commons_class.beneficiary_CaSaNo;
                document.getElementById("lblCARemAccountType").innerText     = this.rem_commons_class.beneficiary_account_type;                  
                document.getElementById("lblCARemBeneACCurrency").innerText  = this.rem_commons_class.beneficiary_ACCurrency;
                document.getElementById("lblCARemTARC").innerText            = this.rem_commons_class.total_amount;
                document.getElementById("lblCARemNRARC").innerText           = this.rem_commons_class.net_remit_amount_rmtccy;
                document.getElementById("lblCARemNRAPE").innerText           = this.rem_commons_class.net_remit_amount_phpeqv;
                document.getElementById("lblCARemNTBACC").innerText          = this.rem_commons_class.net_to_beneficiary;
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
        
            
  
      
      



