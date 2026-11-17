//document.getElementById("lbl_other_purpose").innerText        = this.rem_common_cls.other_purpose_desc;
//document.getElementById("lbl_special_msg").innerText          = this.rem_common_cls.special_msg_desc;
//this.rem_common_cls.set_transaction_values("OTHERPURPOSE"   ,document.getElementById("txt_other_purpose").value);
//this.rem_common_cls.set_transaction_values("SPECIALMESSAGE" ,document.getElementById("txt_special_msg").value);

function push_commit_result(icls)
{
    if (remittance != null)
    {
        remittance.get_commit_result(icls);
    }
}

function open_override_window()
{
    if (remittance != null)
    {
        remittance.open_override_window();
    }
}

function trigger_override_result(param)
{
    if (remittance != null)
    {
        remittance.execute_override_result(param);
    }
}

function execute_ajx_command(param)
{
    if (remittance != null)
    {
        remittance.do_gridsearch(param);
    }
}

function set_side_bar_selected_value(fld_group, fld_value)
{
    if (remittance != null)
    {
        remittance.set_database_column_value(fld_group, fld_value);
    }
}

function cancel_transaction()
{
    if (remittance != null)
    {
        remittance.cancel_transaction();
    }
}

function commit_transaction()
{
    if (remittance != null)
    {
        remittance.commit_transaction();
    }
}

function choose_account()
{
    if (remittance != null)
    {
        remittance.load_account_selector();
    }    
}

function choose_lookup_data(fld_group)
{
    if (remittance != null)
    {
        remittance.load_remittance_lookup_data(fld_group);
    }
}

function view_transaction()
{
    if (remittance != null)
    {
        remittance.load_transaction_view_details();
    }
}

function back_to_search()
{
    if (remittance != null)
    {
        remittance.back_to_search_screen();
    }
}

function rem_transactions()
{
    this.grd_search       = null;
    this.rem_common_cls   = null;
    this.rem_override_cls = null;

    this.execute_override_result = function(param)
    {
        if (param != null)
        {
            this.rem_override_cls = param;
            this.rem_override_cls.close_modal_window();            
        }
        
        this.compute_override_charges();
    }

    this.construct_side_bar = function()
    {
        if (this.rem_common_cls != null)
        {                                        
            document.getElementById("div_transaction_menu").innerHTML = this.rem_common_cls.get_side_bar();
        }            
    }
    
    this.open_override_window = function()
    {
        this.rem_override_cls = new override_calculator();
        this.rem_override_cls.open_override_form("REMITTANCE_CALCULATOR_CLASS_OBJECT");
    }
    
    this.show_or_close_side_bar = function(method)
    {        
        this.rem_common_cls.get_side_bar_visibility('div_transaction_menu', 'a_hide_btn');
    }
    
    this.begin_process = function()
    {        
        if (this.rem_common_cls == null)
        {
            this.rem_common_cls = new rem_commons();
        }
        
        this.rem_common_cls.get_transaction_module_query_string();
        this.construct_side_bar();
        this.load_remittance_screen_information();

        if (this.rem_common_cls.remittance_txn_id == null || this.rem_common_cls.remittance_txn_id == '')
        {
            this.load_account_selector();
        }
    }
    
    this.load_remittance_screen_information = function()
    {
        if (this.rem_common_cls != null)
        {
            this.rem_common_cls.load_remittance_screen_information();

            document.getElementById("lbl_rem_fullname").innerText         = this.rem_common_cls.remitter_fullname;
            document.getElementById("lbl_rem_address").innerText          = this.rem_common_cls.remitter_address;
            document.getElementById("lbl_bene_fullname").innerText        = this.rem_common_cls.beneficiary_fullname;
            document.getElementById("lbl_bene_address").innerText         = this.rem_common_cls.beneficiary_address;
            document.getElementById("lbl_transacting_branch").innerText   = this.rem_common_cls.remitting_branch_code_desc;
            document.getElementById("lbl_product_type").innerText         = this.rem_common_cls.product_code_desc;
            document.getElementById("lbl_implementation_type").innerText  = this.rem_common_cls.implementation_code_desc;
            document.getElementById("lbl_computation_type").innerText     = this.rem_common_cls.computing_method_desc;
            document.getElementById("lbl_purpose").innerText              = this.rem_common_cls.purpose_desc;
            document.getElementById("lbl_source_of_funds").innerText      = this.rem_common_cls.source_of_fund_desc;
            document.getElementById("lbl_transaction_source").innerText   = this.rem_common_cls.transaction_source_desc;
            document.getElementById("lbl_bank_name").innerText            = this.rem_common_cls.beneficiary_account_branch_name;
            document.getElementById("lbl_account_no").innerText           = this.rem_common_cls.beneficiary_account_number;
            document.getElementById("lbl_account_type").innerText         = this.rem_common_cls.beneficiary_account_type_desc;
            document.getElementById("lbl_account_currency").innerText     = this.rem_common_cls.beneficiary_account_currency_desc;
            document.getElementById("lbl_pay_mode").innerText             = this.rem_common_cls.paymode_desc;
            document.getElementById("lbl_charge_type").innerText          = this.rem_common_cls.charge_type_desc;
            document.getElementById("lbl_txn_status").innerText           = this.rem_common_cls.transaction_status_desc;
            
            if (new common_method().is_numeric(this.rem_common_cls.remittance_txn_id) == true)
            {
                document.getElementById("btn_compute_method").style.visibility = "hidden";
                document.getElementById("tr_amount").style.visibility          = "hidden";
                document.getElementById("div_computed_charges").innerHTML = this.rem_common_cls.do_recompute_previous_transaction(this.rem_common_cls.remittance_txn_id);
            }
        }
    }

    this.get_remittance_account_information = function(s)
    {
        if (this.rem_common_cls != null)
        {            
            this.rem_common_cls.get_remittance_account_information(s);            
            document.getElementById("lbl_bank_name").innerText        = this.rem_common_cls.beneficiary_account_branch_name;
            document.getElementById("lbl_account_no").innerText       = this.rem_common_cls.beneficiary_account_number;
            document.getElementById("lbl_account_type").innerText     = this.rem_common_cls.beneficiary_account_type_desc;
            document.getElementById("lbl_account_currency").innerText = this.rem_common_cls.beneficiary_account_currency_desc;            
        }
        
        this.compute_charges();
    }

    this.load_remittance_lookup_data = function(fld_group)
    {
        if (this.rem_common_cls != null)
        {
            this.rem_common_cls.get_remittance_lookup_data(fld_group);
        }
    }

    this.load_account_selector = function()
    {     
        if (this.rem_common_cls != null)
        {
            this.rem_common_cls.get_account_selector();
        }
    }
    
    this.load_transaction_view_details = function()
    {
        this.rem_common_cls.load_transaction_view_details();
    }
    
    this.do_gridsearch = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {                    
            if (this.rem_common_cls != null)
            {
                var command_param = parameter.split("$");
                
                if (command_param.length > 0)
                {
                    if (command_param[0] == "ajx_selected")
                    {
                        switch(this.rem_common_cls.grd_search_keyword.toUpperCase())
                        {
                            case "TRANXPURPOSE"       : this.set_database_column_value('TRANSACTIONPURPOSE', command_param[1]); break;
                            case "TRANXSOURCE"        : this.set_database_column_value('TRANSACTIONSOURCE',  command_param[1]); break;
                            case "SOURCEOFFUNDS"      : this.set_database_column_value('SOURCEOFFUNDS',      command_param[1]); break;
                            case "BENEFICIARYACCOUNT" : this.get_remittance_account_information(command_param[1]); break;
                            case "COMPUTE_METHOD"     : this.reset_product_setup_id(command_param[1]); break;
                        }
                    }
                    if (this.rem_common_cls.grd_search != null)
                    {
                        this.rem_common_cls.grd_search.process_ajax(parameter);
                    }
                }
            }
        }
    }
    
    this.reset_product_setup_id = function(product_charges_id)
    {
        document.getElementById("txt_remitting_amount").value = 0;

        this.rem_common_cls.product_charge_id = product_charges_id;
        this.rem_common_cls.get_product_and_implementation();
        this.rem_common_cls.load_content_to_cls_saving();
        this.compute_charges();
    }

    this.compute_override_charges = function()
    {
        if (this.rem_common_cls != null)
        {
            this.rem_common_cls.remitting_amount = 0;
            this.rem_common_cls.charge_locally   = 0;

            if (document.getElementById("txt_remitting_amount").value != "")
            {
                if (new common_method().is_numeric(document.getElementById("txt_remitting_amount").value) == true)
                {
                    this.rem_common_cls.remitting_amount = document.getElementById("txt_remitting_amount").value;
                }
            }
            else
            {
                this.rem_common_cls.remitting_amount = 0;
            }
            document.getElementById("div_computed_charges").innerHTML = this.rem_common_cls.do_compute_overrided_data();
        }
    }
    
    this.compute_charges = function()
    {
        if (this.rem_common_cls != null)
        {
            this.rem_common_cls.remitting_amount = 0;
            this.rem_common_cls.charge_locally   = 0;

            if (document.getElementById("txt_remitting_amount").value != "")
            {
                if (new common_method().is_numeric(document.getElementById("txt_remitting_amount").value) == true)
                {
                    this.rem_common_cls.remitting_amount = document.getElementById("txt_remitting_amount").value;
                }
            }
            else
            {
                this.rem_common_cls.remitting_amount = 0;
            }            
            document.getElementById("div_computed_charges").innerHTML = this.rem_common_cls.do_compute_charges();
        }
    }
    
    this.change_compute_method = function()
    {
        this.rem_common_cls.get_compute_method();
    }
    
    this.set_database_column_value = function(fld_group, fld_value)
    {
        this.rem_common_cls.set_transaction_values(fld_group, fld_value);
        document.getElementById("lbl_purpose").innerText              = this.rem_common_cls.purpose_desc;
        document.getElementById("lbl_source_of_funds").innerText      = this.rem_common_cls.source_of_fund_desc;
        document.getElementById("lbl_transaction_source").innerText   = this.rem_common_cls.transaction_source_desc;
    }

    this.commit_transaction = function()
    {
        
        var committed = new rem_saving_trans();

        if (this.rem_common_cls.remittance_txn_id == null || this.rem_common_cls.remittance_txn_id == "" || new common_method().is_numeric(this.rem_common_cls.remittance_txn_id) == true)
        {
            committed.SEQ_ID = null;
        }

        if (committed != null)
        {
            if (document.getElementById("txt_remitting_amount").value != "")
            {
                if (new common_method().is_numeric(document.getElementById("txt_remitting_amount").value) == true)
                {
                    if (document.getElementById("txt_remitting_amount").value > 0)
                    {
                        var i_saved = confirm('Do you want to commit this transaction ? \n\n Click cancel to abort or modify transaction entries. ');
                        
                        if (i_saved == true)
                        {             
                            this.rem_common_cls.remitting_amount = document.getElementById("txt_remitting_amount").value; committed.commit_record(); return;
                        }

                        return;
                    }
                }
            }
        }
        
        alert("Please check amount entered!");
    }
    
    this.get_commit_result = function(icls)
    {
        if (icls.commit_status == false)
        {                    
            alert(icls.msg_string);
        }
        else
        {
            alert("Transaction committed successfully"); this.back_to_search_screen();
        }
    }
    
    this.cancel_transaction = function()
    {
        var i_cancel = confirm('Do you want to cancel this transaction ? \n\n Click cancel to abort or modify transaction entries. ');
        
        if (i_cancel == true)
        {
            if (this.rem_common_cls != null)
            {
                this.rem_common_cls.cancel_transaction();
            }                
        }
    }
    
    this.back_to_search_screen = function()
    {
        this.rem_common_cls.go_back_to_search();
    }
}


