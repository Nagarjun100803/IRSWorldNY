
function set_side_bar_selected_value(fld_group,fld_value)
    {
        RemTranDoorToDoor.set_database_column_value(fld_group,fld_value);
    }

function RemTranDoorToDoor()
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
            document.getElementById("lblDoortoDoorRemName").innerText = this.rem_commons_class.remitter_fullname;
            document.getElementById("lblDoortoDoorRemAddress").innerText = this.rem_commons_class.remitter_address;
            document.getElementById("lblDoortoDoorRemBeneName").innerText = this.rem_commons_class.remitter_beneficiary_fullname;
            document.getElementById("lblDoortoDoorRemBeneAddress").innerText = this.rem_commons_class.remitter_beneficiary_address;
            document.getElementById("lblDoortoDoorRemBenePayeeBank").innerText = this.rem_commons_class.payee_bank;
            document.getElementById("lblDoortoDoorRemBeneAccountType").innerText = this.rem_commons_class.account_type;
            document.getElementById("lblDoortoDoorRemBeneCASANo").innerText = this.rem_commons_class.ca_sa_no;
            document.getElementById("lblDoortoDoorRemBeneACCurrency").innerText = this.rem_commons_class.ac_currency;
            document.getElementById("lblDoortoDoorRemTARC").innerText = this.rem_commons_class.total_amount_rmtccy;
            document.getElementById("lblDoortoDoorRemNRAPE").innerText = this.rem_commons_class.net_remit_amount_rmtccy;
            document.getElementById("lblDoortoDoorRemNRARC").innerText = this.rem_commons_class.net_remit_amount_phpeqv;
            document.getElementById("lblDoortoDoorRemNTBACC").innerText = this.rem_commons_class.net_to_beneficiary_acccy;
            
            
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