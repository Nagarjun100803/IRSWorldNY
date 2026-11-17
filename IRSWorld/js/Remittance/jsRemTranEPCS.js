
function set_side_bar_selected_value(fld_group, fld_value)
    {
        RemTranEPCS.set_database_column_value(fld_group, fld_value);
    }

function RemTranEPCS()
    {
        this.rem_commons_class = null;
        
        this.construct_side_bar = function(div_container) //executes the new side menu
            {
                this.rem_commons_class = new rem_commons();
                document.getElementById(div_container).innerHTML = this.rem_commons_class.get_side_bar();
                this.execute_txn_product_main_loading();
            }
            
        this.set_side_bar_visibility = function(division_id, anchor_id) // displays and hides the side bar menu
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
                document.getElementById("lblEPCSRemName").innerText = this.rem_commons_class.remitter_fullname;
                document.getElementById("lblEPCSRemAddress").innerText = this.rem_commons_class.remitter_address;
                document.getElementById("lblEPCSRemBeneName").innerText = this.rem_commons_class.beneficiary_fullname;
                document.getElementById("lblEPCSRemBeneAddress").innerText = this.rem_commons_class.beneficiary_address;
                document.getElementById("lblEPCSRemBenePayeeBank").innerText = this.rem_commons_class.beneficiary_payee_bank;
                document.getElementById("lblEPCSRemCaSaNo").innerText = this.rem_commons_class.beneficiary_CaSaNo;
                document.getElementById("lblEPCSRemAccountType").innerText = this.rem_commons_class.beneficiary_account_type;                  
                document.getElementById("lblEPCSRemACCurrency").innerText = this.rem_commons_class.beneficiary_ACCurrency;
                document.getElementById("lblEPCSRemTARC").innerText = this.rem_commons_class.total_amount;
                document.getElementById("lblEPCSRemNRARC").innerText = this.rem_commons_class.net_remit_amount_rmtccy;
                document.getElementById("lblEPCSRemNRAPE").innerText = this.rem_commons_class.net_remit_amount_phpeqv;
                document.getElementById("lblEPCSRemNTBACC").innerText = this.rem_commons_class.net_to_beneficiary;
            }
            
       this.load_transaction_view_details = function()  //displays load_transaction_view_details page
            {
                this.rem_commons_class.load_transaction_view_details();
            }
            
      this.set_database_column_value = function(fld_group, fld_value)
            {
                this.rem_commons_class.set_transaction_values(fld_group, fld_value);
            }
            
  }
        
            
  
      
      



