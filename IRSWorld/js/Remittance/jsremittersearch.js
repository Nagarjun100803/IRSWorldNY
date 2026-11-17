function execute_ajx_command(param)
{
    if (rem_search != null && rem_search != "undefined")
    {
        rem_search.do_gridsearch(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (rem_search != null && rem_search != "undefined")
    {
        rem_search.do_gridview(param);
    }
}

function remittance_search()
{
    this.product_charge_id   = null;
    this.rem_txn_sequence_id = null;
    this.remitter_id         = null;
    this.remitter_name       = null;
    this.remitter_addr       = null;    
    this.beneficiary_id      = null;
    this.beneficiary_name    = null;
    this.beneficiary_addr    = null;

    this.grid_ajax_page      = "../AjxPage/ajaxgrid.aspx";
    this.grid_ajax_query     = null;    
    this.ajax_grid           = null;
    this.ajax_container      = null;
    this.commons             = null;

    this.active_grid         = null;
    this.branch_code         = 'LDN';
    
    var productcode = ""; 
    var implemtype  = "";
    var chargetype  = "";
    var paymode     = "";

    this.open_new_cif_account = function(cif_classification)
    {
       this.open_division("div_new_cif");
    }

    this.open_search_box = function()    
    {
        this.branch_code = RemTranCommons.get_logged_branch_code().value;
        this.division_loading_progress();                
        this.load_customer_class();
        this.load_customer_type();
        this.load_product_charges_criteria();
        this.open_division("div_search");
    }

    this.search_customers_info = function()
    {                
        var cif_classification = document.getElementById("ddlSearchClass").value;
        var last_name          = document.getElementById("txtSearchLastname").value;
        var first_name         = document.getElementById("txtSearchFirstname").value;
        var middle_name        = document.getElementById("txtSearchMiddlename").value;

        if (last_name == '' && first_name == '' && middle_name == '')
        {
            alert("Invalid Search Value."); return;
        }

        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "sp_txn_fullsearch~" + cif_classification + "~" + last_name + "~" + first_name + "~" + middle_name;
        
        
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.ajax_container ); 
    }
        
    this.close_divisions = function()
    {
        document.getElementById("div_search").style.display        = 'none';
        document.getElementById("div_new_cif").style.display       = 'none';
        document.getElementById("div_improd_change").style.display = 'none';
        document.getElementById("div_results").style.display       = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
    this.load_rem_product_implementation = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_txn_search_prod_implem~" + this.branch_code;
        this.grid.div_container  = document.getElementById("DIV_PROD_IMPLEM");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 20;
        this.grid.open_grid_view();
        this.grid = null;        
    }

    this.load_remitter_and_bene_information = function()
    {
        document.getElementById("lblRemitterName").innerText    = '';
        document.getElementById("lblRemitterAddress").innerText = '';
        document.getElementById("lblBeneName").innerText        = '';
        document.getElementById("lblBenefAddress").innerText    = '';
        document.getElementById("lblRemitterName").innerText    = this.remitter_name;
        document.getElementById("lblRemitterAddress").innerText = this.remitter_addr;
        document.getElementById("lblBeneName").innerText        = this.beneficiary_name;
        document.getElementById("lblBenefAddress").innerText    = this.beneficiary_addr;
    }
    
    this.load_remitter_transactions = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_txn_search_remitters_trans~" + this.remitter_id;
        this.grid.div_container  = document.getElementById("DIV_REM_TXN");
        this.grid.datarow_column = 0;        
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.load_beneficiary_prod_imp = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_txn_search_benef_prod_imp~" + this.beneficiary_id + "~" + this.branch_code;
        this.grid.div_container  = document.getElementById("DIV_BENE_PROD_IMP");
        this.grid.datarow_column = 0;        
        this.grid.open_grid_view();
        this.grid = null;        
    }

    this.load_customer_class = function()
    {
        RemTranCommons.get_customers_class_source(this.load_customer_class_callback);
    }
        
    this.load_customer_class_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlSearchClass", result.value, "FldValue", "FldDescription");
            this.commons.to_load_dropdownlist("ddlNewCIFClass", result.value, "FldValue", "FldDescription");
            this.commons = null;
            
            document.getElementById("ddlSearchClass").options[1].selected = true;
            document.getElementById("ddlNewCIFClass").options[1].selected = true;
        }        
    }
        
    this.load_customer_type = function()
    {        
        RemTranCommons.get_customers_type_source(this.load_customer_type_callback);
    }
        
    this.load_customer_type_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlNewCIFType", result.value, "FldValue", "FldDescription");
            this.commons = null;                        
        }
    }

    this.do_gridsearch = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    var command_values = command_param[1].split("~");

                    this.remitter_id      = command_values[0];
                    this.beneficiary_id   = command_values[1];
                    this.remitter_name    = command_values[2];
                    this.remitter_addr    = command_values[3];
                    this.beneficiary_name = command_values[4];
                    this.beneficiary_addr = command_values[5];

                    this.load_remitter_and_bene_information();                                    
                    this.load_rem_product_implementation();
                    this.load_beneficiary_prod_imp();
                    this.open_division("div_results");
                }                
                this.ajax_grid.process_ajax(parameter);
            }
        }
    }

    this.do_gridview = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
                                    
            if (command_param.length > 0)
            {                                                
                if (command_param[0] == "ajx_selected")
                {
                    switch(this.active_grid.toUpperCase())
                    {
                        case "DIV_PROD_IMPLEM" :

                            this.product_charge_id = command_param[1];
                            this.remittance_redirection(); return;

                        case "DIV_REM_TXN" :

                            var command_value = command_param[1].split('~');

                            this.rem_txn_sequence_id = command_value[0];
                            this.product_charge_id   = command_value[1];
                            this.remitter_id         = command_value[2];
                            this.beneficiary_id      = command_value[3];
                            this.remittance_redirection(); return;

                        case "DIV_BENE_PROD_IMP" :

                            this.product_charge_id = command_param[1]; 
                            this.remittance_redirection(); return;
                    }
                }                                
                this.process_grid_viewer(parameter);
            }
        }
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.session_end = true;
        var txt_search = null;
        switch(this.active_grid.toUpperCase())
        {
            case "DIV_PROD_IMPLEM" :

                txt_search = document.getElementById("txt_search");
                                
                this.grid.sql_source     = "sp_txn_search_prod_implem_param~" + this.branch_code  + "~" + productcode + "~" + implemtype + "~" + paymode + "~" + chargetype;
                this.grid.div_container  = document.getElementById("DIV_PROD_IMPLEM");
                this.grid.datarow_column = 0;
                this.grid.page_size      = 20;                
                break;

            case "DIV_REM_TXN" :

                this.grid.sql_source     = "sp_txn_search_remitters_trans~" + this.remitter_id;
                this.grid.div_container  = document.getElementById("DIV_REM_TXN");                
                this.grid.datarow_column = 0;
                break;

            case "DIV_BENE_PROD_IMP" :
                this.grid.sql_source     = "sp_txn_search_benef_prod_imp~" + this.beneficiary_id + "~" + this.branch_code;               
                this.grid.div_container  = document.getElementById("DIV_BENE_PROD_IMP");
                this.grid.datarow_column = 0;                
                break;
        }        
        
        if ((txt_search != null ) && (txt_search.value.length > 0))
        {
            this.grid.process_ajax(parameter + txt_search.value );
        }
        else
        {
        
            this.grid.process_ajax(parameter);
        }
        this.grid = null;
    }

    this.division_loading_progress = function()
    {
        this.commons = new common_method();
        this.commons.to_show_loading_message("DIV_BENE_PROD_IMP");        
        this.commons.to_show_loading_message("DIV_PROD_IMPLEM");        
        this.commons = null;
    }
    
    this.remittance_redirection = function()
    {
        var redirect_link = null;
        
        if (this.rem_txn_sequence_id == null)
        {
            this.rem_txn_sequence_id = "";
        }
        
        redirect_link = "?remittance_txn_id="  + this.rem_txn_sequence_id;
        redirect_link += "&product_charge_id=" + this.product_charge_id;
        redirect_link += "&remitter_id="       + this.remitter_id;
        redirect_link += "&beneficiary_id="    + this.beneficiary_id;

        RemTranSearch.get_remittance_page(this.product_charge_id, redirect_link, this.remittance_redirection_callback);
    }
    
    this.remittance_redirection_callback = function(result)
    {
        if (result != null && result.value != 'undefined' && result.value != '')
        {
            window.location = result.value; return;
        }
        
        alert("PAGE NOT FOUND!");
    }    
    
    
    
    this.load_product_charges_criteria = function()
    {
        RemTranSearch.get_ProductType(this.branch_code,this.load_producttype_callback); 
        RemTranSearch.get_ImplemType(this.branch_code,this.load_implemtype_callback); 
        RemTranSearch.get_Paymode(this.branch_code,this.load_paymode_callback); 
        RemTranSearch.get_ChargeType(this.branch_code,this.load_chargetype_callback); 
        
    }
    this.load_producttype_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("cmbproducttype", result.value, "FldValue", "FldDescription");
            this.commons = null;                        
        }     
    } 
    this.load_implemtype_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("cmbimplemtype", result.value, "FldValue", "FldDescription");
            this.commons = null;                        
        }     
    } 
     this.load_paymode_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("cmbpaymode", result.value, "FldValue", "FldDescription");
            this.commons = null;                        
        }     
    } 
     this.load_chargetype_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("cmbchargetype", result.value, "FldValue", "FldDescription");
            this.commons = null;                        
        }     
    } 
    
    this.search_product_charges_param = function()
    {
        productcode = document.getElementById("cmbproducttype").value; 
        implemtype  = document.getElementById("cmbimplemtype").value;
        chargetype  = document.getElementById("cmbchargetype").value;
        paymode     = document.getElementById("cmbpaymode").value;

        if (productcode=='null')
        {
            productcode="";
        }

        if (implemtype=='null')
        {
            implemtype="";
        }  
        if (chargetype=='null')
        {
            chargetype="";
        }

        if (paymode=='null')
        {
            paymode="";
        }   
        
        document.getElementById("txt_search").value = "";                          
                
        this.process_grid_viewer('ajx_loading$');
    }
    
    this.clear_search_param= function()
    {
        document.getElementById("cmbproducttype").value=""; 
        document.getElementById("cmbimplemtype").value="";
        document.getElementById("cmbchargetype").value="";
        document.getElementById("cmbpaymode").value="";
        productcode = ""; 
        implemtype  = "";
        chargetype  = "";
        paymode     = "";
    }
  
    this.open_product_charges= function()
    {
        this.clear_search_param();
        this.process_grid_viewer('ajx_loading$');
        rem_search.open_division('div_results');
    }
}