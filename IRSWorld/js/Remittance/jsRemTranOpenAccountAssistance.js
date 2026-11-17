
function set_side_bar_selected_value(fld_group,fld_value)
    {
        RemTranOpenAccount_Assistance.set_database_column_value(fld_group,fld_value);
    }
    
function RemTranOpenAccount_Assistance()
    { 
        this.load_all_dropdownlist = function()
        {
            this.dropdown_pnbbranch();
            this.dropdown_accy();
        }
        
        this.dropdown_pnbbranch = function()
            {                                                                                  
                RemTranOpenAccountAssistance.getdropdown_source_code_description("BranchCode", this.dropdown_branchcode_callback)
                
            }
        this.dropdown_branchcode_callback = function(result)
            {                                    
                if (result != null && result != "undefined")
                {
                    this.commons = new common_method();                                 
                    this.commons.to_load_dropdownlist("ddlOpenAccountAssistanceRemPNBBranch", result.value, "FldValue", "FldDescription");
                    this.commons = null;           
                }
            }
    
    
        this.dropdown_accy = function()
            {                  
                RemTranOpenAccountAssistance.getdropdown_source_code_description("Currency", this.dropdown_accy_callback)
            }
        
        this.dropdown_accy_callback = function(result)
            {           
                if (result != null && result != "undefined")
                {
                    this.commons = new common_method();        
                    this.commons.to_load_dropdownlist("ddlOpenAccountAssistanceRemBeneACCurrency", result.value, "FldValue", "FldDescription");
                    this.commons = null;           
                }
            }
        
    
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
            document.getElementById("lblRemTranOpenAccountAssistanceRemName").innerText         = this.rem_commons_class.remitter_fullname;
            document.getElementById("lblRemTranOpenAccountAssistanceRemAddress").innerText      = this.rem_commons_class.remitter_address;
            document.getElementById("lblRemTranOpenAccountAssistanceRemBeneName").innerText     = this.rem_commons_class.beneficiary_fullname;
            document.getElementById("lblRemTranOpenAccountAssistanceRemBeneAddress").innerText  = this.rem_commons_class.beneficiary_address;
            document.getElementById("lblRemTranOpenAccountAssistanceRemTARC").innerText         = this.rem_commons_class.total_amount_rmtccy;
            document.getElementById("lblRemTranOpenAccountAssistanceRemNRAPE").innerText        = this.rem_commons_class.net_remit_amount_rmtccy;
            document.getElementById("lblRemTranOpenAccountAssistanceRemNRARC").innerText        = this.rem_commons_class.net_remit_amount_phpeqv;
            document.getElementById("lblRemTranOpenAccountAssistanceRemNTBACC").innerText       = this.rem_commons_class.net_to_beneficiary_acccy;
            
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
