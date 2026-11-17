function choose_account()
{
    if (remittance != null)
    {
        remittance.load_account_selector();
    }
}

function choose_account_grid_window()
{
    if (remittance != null)
    {
        remittance.load_account_selector_for_grid_view();
    }
}

function choose_lookup_data(fld_group)
{
    if (remittance != null)
    {
        remittance.load_remittance_lookup_data(fld_group);
    }
}

function choose_lookup_data_grid_window(fld_group)
{
    if (remittance != null)
    {
        remittance.load_remittance_lookup_data_for_grid_window(fld_group);
    }
}

function open_transaction_details()
{
    if (remittance != null)
    {
        remittance.open_gridview_panel("GENERALINFO");
    }
}

function open_compute_charges()
{
    if (remittance != null)
    {
        remittance.open_gridview_panel("CALCULATOR");
    }
}

function open_override_window()
{
    if (remittance != null)
    {
        remittance.open_override_window();
    }
}

function push_commit_result(icls)
{
    if (remittance != null)
    {
        remittance.get_commit_result(icls);
    }
}

function push_back_calculator_result(icls)
{
    if (remittance != null)    
    {
        remittance.get_calculator_results(icls);
    }
}

function execute_ajx_command(param)
{
    if (remittance != null)
    {
        remittance.do_gridsearch(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (remittance != null)
    {
        remittance.do_gridview(param);
    }
}

function view_transaction()
{
    if (remittance != null)
    {
        remittance.load_transaction_view_details();
    }
}

function trigger_override_result(param)
{
    if (remittance != null)
    {
        remittance.execute_override_result(param);
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
    this.grid             = null;
    this.rem_common_cls   = null;
    this.rem_override_cls = null;

    this.execute_override_result = function(icls)
    {
        if (icls != null)
        {
            this.rem_override_cls = icls;
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
    
    this.open_gridview_panel = function(panel_key)
    {
        this.hide_content_divs();        
        this.rem_common_cls.grd_search_keyword = panel_key;

        switch(this.rem_common_cls.grd_search_keyword.toUpperCase())
        {
            case "TRANXPURPOSE"       : document.getElementById("div_purpose_ui").style.display            = "block"; document.getElementById("lbl_swapable").innerText = "PURPOSE";             break;
            case "TRANXSOURCE"        : document.getElementById("div_transaction_source_ui").style.display = "block"; document.getElementById("lbl_swapable").innerText = "TRANSACTION SOURCE";  break;
            case "SOURCEOFFUNDS"      : document.getElementById("div_source_of_fund_ui").style.display     = "block"; document.getElementById("lbl_swapable").innerText = "SOURCE OF FUND";      break;
            case "CALCULATOR"         : document.getElementById("div_calculator_ui").style.display         = "block"; document.getElementById("lbl_swapable").innerText = "CHARGE DETAILS";      break;
            case "GENERALINFO"        : document.getElementById("div_general_info_ui").style.display       = "block"; document.getElementById("lbl_swapable").innerText = "TRANSACTION DETAILS"; break;
            case "BENEFICIARYACCOUNT" : document.getElementById("div_benef_accounts_ui").style.display     = "block"; document.getElementById("lbl_swapable").innerText = "BENEFICIARY ACCOUNT"; break;
        }
    }
    
    this.show_or_close_side_bar = function(method)
    {        
        this.rem_common_cls.get_side_bar_visibility('div_transaction_menu', 'a_hide_btn');
    }
    
    this.hide_content_divs = function()
    {
        document.getElementById("div_purpose_ui").style.display            = "none";
        document.getElementById("div_transaction_source_ui").style.display = "none";
        document.getElementById("div_source_of_fund_ui").style.display     = "none";
        document.getElementById("div_calculator_ui").style.display         = "none";
        document.getElementById("div_general_info_ui").style.display       = "none";
        document.getElementById("div_benef_accounts_ui").style.display     = "none";        
    }
    
    this.begin_process = function()
    {
        this.hide_content_divs();
        
        if (this.rem_common_cls == null)
        {
            this.rem_common_cls = new rem_commons();
        }
                
        this.rem_common_cls.get_transaction_module_query_string();
        this.load_remittance_screen_information();
        this.construct_side_bar();
        this.compute_charges(0);

    }
    
    this.load_remittance_screen_information = function()
    {            
        if (this.rem_common_cls != null)
        {
            document.getElementById("lbl_rem_fullname").innerText         = this.rem_common_cls.remitter_fullname;
            document.getElementById("lbl_rem_address").innerText          = this.rem_common_cls.remitter_address;
            document.getElementById("lbl_bene_fullname").innerText        = this.rem_common_cls.beneficiary_fullname;
            document.getElementById("lbl_bene_address").innerText         = this.rem_common_cls.beneficiary_address;
            document.getElementById("lbl_transacting_branch").innerText   = this.rem_common_cls.remitting_branch_code_desc;
            document.getElementById("lbl_product_type").innerText         = this.rem_common_cls.product_code_desc;
            document.getElementById("lbl_implementation_type").innerText  = this.rem_common_cls.implementation_code_desc;
            document.getElementById("lbl_computation_type").innerText     = this.rem_common_cls.computing_method_desc;
            document.getElementById("lbl_purpose").innerText              = this.rem_common_cls.purpose_desc;
            document.getElementById("lbl_purpose_2").innerText            = this.rem_common_cls.purpose_desc;
            document.getElementById("lbl_source_of_funds").innerText      = this.rem_common_cls.source_of_fund_desc;
            document.getElementById("lbl_source_of_funds_2").innerText    = this.rem_common_cls.source_of_fund_desc;
            document.getElementById("lbl_transaction_source").innerText   = this.rem_common_cls.transaction_source_desc;
            document.getElementById("lbl_transaction_source_2").innerText = this.rem_common_cls.transaction_source_desc;
            document.getElementById("lbl_bank_name").innerText            = this.rem_common_cls.beneficiary_account_branch_name;
            document.getElementById("lbl_account_no").innerText           = this.rem_common_cls.beneficiary_account_number;
            document.getElementById("lbl_account_type").innerText         = this.rem_common_cls.beneficiary_account_type_desc;
            document.getElementById("lbl_account_currency").innerText     = this.rem_common_cls.beneficiary_account_currency_desc;
            document.getElementById("lbl_pay_mode").innerText             = this.rem_common_cls.paymode_desc;
            document.getElementById("lbl_pay_mode_2").innerText           = this.rem_common_cls.paymode_desc;
            document.getElementById("lbl_charge_type").innerText          = this.rem_common_cls.charge_type_desc;
            document.getElementById("lbl_txn_status").innerText           = this.rem_common_cls.transaction_status_desc;
            document.getElementById("lbl_computation_type").innerText     = this.rem_common_cls.computing_method_desc;
            document.getElementById("lbl_computation_type_2").innerText   = this.rem_common_cls.computing_method_desc;
            
            if (new common_method().is_numeric(this.rem_common_cls.remittance_txn_id) == false)
            {
                this.open_gridview_panel("CALCULATOR");
            }
            else
            {
                this.rem_common_cls.do_recompute_previous_transaction(this.rem_common_cls.remittance_txn_id);
            
                document.getElementById("btn_compute_method").style.visibility = "hidden";
                document.getElementById("btn_remit").style.visibility          = "hidden";
                document.getElementById("btn_cancel").style.visibility         = "hidden";                
                document.getElementById("tr_amounts").style.visibility         = "hidden";                
            }
        }
    }

    this.set_beneficiary_account_information = function(acct_nos)
    {

        if (this.rem_common_cls != null)
        {
            this.rem_common_cls.get_remittance_account_information(acct_nos);

            document.getElementById("lbl_bank_name").innerText            = this.rem_common_cls.beneficiary_account_branch_name;
            document.getElementById("lbl_account_no").innerText           = this.rem_common_cls.beneficiary_account_number;
            document.getElementById("lbl_account_type").innerText         = this.rem_common_cls.beneficiary_account_type_desc;
            document.getElementById("lbl_account_currency").innerText     = this.rem_common_cls.beneficiary_account_currency_desc;            
            document.getElementById("lbl_account_no_selection").innerText = this.rem_common_cls.beneficiary_account_number;
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
    
    this.load_remittance_lookup_data_for_grid_window = function(fld_group)
    {
        var division_control = null;

        this.rem_common_cls.grd_search_keyword = fld_group;
    
        switch(this.rem_common_cls.grd_search_keyword.toUpperCase())
        {
            case "TRANXPURPOSE"  : division_control = document.getElementById("div_purpose_grd")           ;break;
            case "TRANXSOURCE"   : division_control = document.getElementById("div_transaction_source_grd");break;
            case "SOURCEOFFUNDS" : division_control = document.getElementById("div_source_of_fund_grd")    ;break;
        }
        if (this.rem_common_cls != null)
        {
            this.open_gridview_panel(fld_group); 
            this.rem_common_cls.get_remittance_lookup_data_for_grid_viewer(fld_group, division_control);
        }
    }
    

    this.load_account_selector = function()
    {     
        if (this.rem_common_cls != null)
        {
            this.rem_common_cls.get_account_selector();
        }
    }
    
    this.load_account_selector_for_grid_view = function()
    {
        if (this.rem_common_cls != null)
        {
            this.open_gridview_panel("BENEFICIARYACCOUNT");
            this.rem_common_cls.get_account_selector_for_grid_window(document.getElementById("div_benef_accounts_grd"));
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
                            case "COMPUTE_METHOD" : this.reset_product_setup_id(command_param[1]); break;
                        }
                    }
                    this.rem_common_cls.grd_search.process_ajax(parameter);
                }
            }
        }
    }

    this.do_gridview = function(parameter)
    {
        if (parameter != null)
        {                    
            if (this.rem_common_cls != null)
            {
                var command_param = parameter.split("$");
                
                if (command_param.length > 0)
                {
                    if (command_param[0] == "ajx_selected")
                    {
                    
                        var column_value_arrays = null;
                        
                        if (command_param[1] != null)
                        {
                            column_value_arrays = command_param[1].split("~");                            
                        }
                        
                        switch(this.rem_common_cls.grd_search_keyword.toUpperCase())
                        {
                            case "TRANXPURPOSE"       : this.set_lookup_description_for_grid_window(column_value_arrays); break;
                            case "TRANXSOURCE"        : this.set_lookup_description_for_grid_window(column_value_arrays); break;
                            case "SOURCEOFFUNDS"      : this.set_lookup_description_for_grid_window(column_value_arrays); break;
                            case "BENEFICIARYACCOUNT" : this.set_beneficiary_account_information(command_param[1]); break;
                            case "COMPUTE_METHOD"     : this.reset_product_setup_id(column_value_arrays[0]); break;
                        }
                    }
                    else
                    {
                        this.rem_common_cls.process_grid_viewer(parameter);
                    }
                }
            }
        }
    }

    this.set_lookup_description_for_grid_window = function(column_value_arrays)
    {
        var key_word = this.rem_common_cls.grd_search_keyword.toUpperCase();
        
        if (column_value_arrays.length > 0)
        {
            switch(key_word)
            {
                case "TRANXPURPOSE"  :
                    document.getElementById("lbl_purpose").innerText   = column_value_arrays[1];
                    document.getElementById("lbl_purpose_2").innerText = column_value_arrays[1];
                    break;
                case "TRANXSOURCE"   : 
                    document.getElementById("lbl_transaction_source").innerText   = column_value_arrays[1];
                    document.getElementById("lbl_transaction_source_2").innerText = column_value_arrays[1];
                    break;
                case "SOURCEOFFUNDS" : 
                    document.getElementById("lbl_source_of_funds").innerText   = column_value_arrays[1];
                    document.getElementById("lbl_source_of_funds_2").innerText = column_value_arrays[1];
                    break;
            }

            switch(key_word)
            {
                case "TRANXPURPOSE"  : this.rem_common_cls.set_transaction_values("SOURCEOFFUNDS",      column_value_arrays[0]); break;
                case "TRANXSOURCE"   : this.rem_common_cls.set_transaction_values("TRANSACTIONPURPOSE", column_value_arrays[0]); break;
                case "SOURCEOFFUNDS" : this.rem_common_cls.set_transaction_values("TRANSACTIONSOURCE",  column_value_arrays[0]); break;
            }            
        }
    }
    
    this.reset_product_setup_id = function(product_charges_id)
    {
        this.rem_common_cls.product_charge_id = product_charges_id;
        this.rem_common_cls.get_product_and_implementation();
        
        document.getElementById("txt_remitting_amount").value       = 0;
        document.getElementById("lbl_computation_type").innerText   = this.rem_common_cls.computing_method_desc;
        document.getElementById("lbl_computation_type_2").innerText = this.rem_common_cls.computing_method_desc;

        this.compute_charges(0);
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
    
    this.chk_remitting_amount = function(e)
    {                    
        var amount_rem = 0;
        
        /* AUTOMATICALLY TRIGGER COMPUTATION IF TEXTBOX CONTROL IS EMPTY OR SPACE */

        if(window.event)
	    {
	        keynum = e.keyCode;
	    }
        else if(e.which)
	    {
	        keynum = e.which;
	    }        
        
        if (keynum == 13)
        {
            amount_rem = new common_method().to_remove_comma(document.getElementById("txt_remitting_amount").value);
            
            if (new common_method().is_decimal(amount_rem) == false)
            {
                alert("AMOUNT ENTERED (" + amount_rem + ") IS INVALID!\n CLICK OK TO CORRECT AMOUNT." ); 
                document.getElementById("txt_remitting_amount").value = 0; amount_rem = 0;
            }
            this.compute_charges(amount_rem);
        }
    }
    
    this.compute_charges = function(amount_rem)
    {
        if (this.rem_common_cls != null)
        {
            if (amount_rem == null)
            {
                amount_rem = 0;

                if (this.rem_common_cls.remitting_amount != null)
                {
                    if (this.rem_common_cls.remitting_amount != 0)
                    {
                        amount_rem = this.rem_common_cls.remitting_amount;
                    }
                }
            }
            if (new common_method().is_decimal(amount_rem) == true)
            {
                this.rem_common_cls.remitting_amount = amount_rem;
            }            
            this.rem_common_cls.do_compute_charges();
        }
    }
    
    this.change_compute_method = function()
    {
        this.rem_common_cls.get_compute_method();
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
                if (new common_method().is_decimal(document.getElementById("txt_remitting_amount").value) == true)
                {
                    this.rem_common_cls.load_content_to_cls_saving();
                
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
    
    this.get_calculator_results = function(icls)
    {
        document.getElementById("div_computed_charges").innerHTML  = icls.calculator_breakdown;
        document.getElementById("div_calculator_review").innerHTML = icls.calculator_breakdown;
    }
}


