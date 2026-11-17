function set_sidebar_selected_value(fld_group, fld_value)
{
    RemTran_GFMC.set_database_column_value(fld_group, fld_value);
}

function RemTran_GFMC()
{
    this.rem_commons_class = null;
    
    this.construct_side_bar = function(div_container)
    {
        this.rem_commons_class= new rem_commons(); 
        document.getElementById(div_container).innerHTML = this.rem_commons_class.get_side_bar();
        this.execute_txn_product_main_loading();
        this.dropdown_currency();
    }
    
    this.dropdown_currency = function()
    {
        RemTranGFMC.getdropdown_currency("Currency", this.dropdown_currency_callback)
    }
    
    this.dropdown_currency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlGFMCRemACCurrency", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
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
        document.getElementById('lblGFMCRemName').innerText = this.rem_commons_class.remitter_fullname;
        document.getElementById('lblGFMCRemAddress').innerText = this.rem_commons_class.remitter_address;
        document.getElementById('lblGFMCRemBeneName').innerText = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblGFMCRemBeneAddress').innerText = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblGFMCRemPNBBranch').innerText = this.rem_commons_class.pnb_branch;
        document.getElementById('lblGFMCRemCaSaNo').innerText = this.rem_commons_class.ca_sa_no;
        document.getElementById('lblGFMCRemAccountType').innerText = this.rem_commons_class.account_type;
        document.getElementById('lblGFMCRemACCurrency').innerText = this.rem_commons_class.ac_currency;
        document.getElementById('lblGFMCRemTARC').innerText = this.rem_commons_class.total_amount;
        document.getElementById('lblGFMCRemNRARC').innerText = this.rem_commons_class.netremit_amount_rc;
        document.getElementById('lblGFMCRemNRAPE').innerText = this.rem_commons_class.netremit_amount_pe;
        document.getElementById('lblGFMCRemNTBACC').innerText = this.rem_commons_class.net_to_beneficiary;
    }
    
    this.load_transaction_view_details = function()
    {
        this.rem_commons_class.load_transaction_view_details()
    }
    
    this.set_database_column_value = function(fld_group, fld_value)
    {
        this.rem_commons_class.set_transaction_values(fld_group, fld_value);
    }
    
    this.generate_avail = function(rdoGFMCRemPurchase, txtGFMCRemGlobalCardNo, txtGFMCRemIDPresented)
    {
        this.generate_rdo_enable(rdoGFMCRemPurchase);
        this.generate_text_disable(txtGFMCRemGlobalCardNo);
        this.generate_text_disable(txtGFMCRemIDPresented);
        
    }
    
    this.generate_text_disable = function(text_id_disable)
    {

        var id  = document.getElementById(text_id_disable);
        var cset = id.disabled;
        var clear = id.value;
        
        id.disabled = (cset == "false") ? "true" : "false";
        id.value = "";
    }
 
    this.generate_purchase = function(rdoGFMCRemAvailment, txtGFMCRemGlobalCardNo, txtGFMCRemIDPresented)
    {
        this.generate_rdo_enable(rdoGFMCRemAvailment);
        this.generate_text_enable(txtGFMCRemGlobalCardNo);
        this.generate_text_enable(txtGFMCRemIDPresented);

    }
    
    this.generate_rdo_enable = function(rdo_id)
    {

        var id  = document.getElementById(rdo_id);
        var cset = id.checked;
        
        id.checked = cset == "false";
    }
    
    this.generate_text_enable = function(text_id_enable)
    {
        var id  = document.getElementById(text_id_enable);
        var cset = id.disabled;
        
        id.disabled = cset == "false";
    }
}

