
function set_side_bar_selected_value(fld_group,fld_value)
    {
        RemTranPDDTS.set_database_column_value(fld_group,fld_value);
    }
    
function RemTranPDDTS()
    {
        this.rem_commons_class=null;
        this.construct_side_bar=function(div_container)
        
        {
            this.rem_commons_class=new rem_commons();
            document.getElementById(div_container).innerHTML=this.rem_commons_class.get_side_bar();
            this.execute_txn_product_main_loading();
        }
        
        this.set_side_bar_visibility=function(division_id,anchor_id)
        
        {   
            this.rem_commons_class.get_side_bar_visibility(division_id,anchor_id);
        }
        
        this.execute_txn_product_main_loading=function()
        
        {
            this.rem_commons_class.do_txn_product_main_loading();
            this.load_remitter_and_beneficiary_info();
        }
        
        this.load_remitter_and_beneficiary_info=function()
        
        {
            document.getElementById("lblPDDTSRemName").innerText = this.rem_commons_class.remitter_fullname;
            document.getElementById("lblPDDTSRemAddress").innerText = this.rem_commons_class.remitter_address;
            document.getElementById("lblPDDTSRemBeneName").innerText = this.rem_commons_class.beneficiary_fullname;
            document.getElementById("lblPDDTSRemBeneAddress").innerText = this.rem_commons_class.beneficiary_address;
            document.getElementById("lblPDDTSRemBenePayeeBank").innerText = this.rem_commons_class.payee_bank;
            document.getElementById("lblPDDTSRemBeneAccountType").innerText = this.rem_commons_class.account_type;
            document.getElementById("lblPDDTSRemBeneCASANo").innerText = this.rem_commons_class.ca_sa_no;
            document.getElementById("lblPDDTSRemBeneACCurrency").innerText = this.rem_commons_class.ac_currency;
            document.getElementById("lblPDDTSRemTARC").innerText = this.rem_commons_class.total_amount_rmtccy;
            document.getElementById("lblPDDTSRemNRAPE").innerText = this.rem_commons_class.net_remit_amount_rmtccy;
            document.getElementById("lblPDDTSRemNRARC").innerText = this.rem_commons_class.net_remit_amount_phpeqv;
            document.getElementById("lblPDDTSRemNTBACC").innerText = this.rem_commons_class.net_to_beneficiary_acccy;
            
        }
        
        this.load_transaction_view_details=function()
        
        {
            this.rem_commons_class.load_transaction_view_details();
            
        }   
        
        this.set_database_column_value=function(fld_group,fld_value)
        
        {
            this.rem_commons_class.set_transaction_value(fld_group,fld_value);
            
        }
    }