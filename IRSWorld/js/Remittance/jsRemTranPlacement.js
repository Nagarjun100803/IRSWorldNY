var iremit = new iremit();

function execute_ajx_command(param)
{
    if (iremit != null)
    {
        iremit.exec_modal_lookup(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (iremit != null)
    {
    }
}

function side_bar_link(modal_keycode, modal_return_value)
{
    iremit.side_bar_link_execute(modal_keycode, modal_return_value);
}

function iremit()
{
    this.branch_code         = "LDN";
    this.product_id          = 1;
    this.product_code        = 0;
    this.implementation_code = 0;
    this.remitter_id         = 1;
    this.beneficiary_id      = 0;
    this.grd_search          = null;
    this.grd_search_keyword  = null;

    this.get_transacting_branch = function()
    {            
    }
    
    this.get_transacting_branch_callback = function(result)
    {
    }
    
    this.get_sidebar_list = function()
    {
        var sidebar;

        if (this.remitter_id == null)
        {
            sidebar = new Array
            (
                new Array(1,0,'Search Client','javascript:side_bar_link("CLIENT");'),
                new Array(2,0,'Quit',         'javascript:side_bar_link("QUIT");')
            );
        }
        else
        {
            if (this.product_id == null)
            {
                sidebar = new Array
                (
                    new Array(1,0,'Select Product','javascript:side_bar_link("CLIENT");'),
                    new Array(2,0,'Cancel',         'javascript:side_bar_link("QUIT");')
                );
            }
            else
            {
                sidebar = new Array
                (
                    new Array(3,0,  'Change Product',     'javascript:side_bar_link("LOOKUP", "ReceiptMode");'),
                    new Array(3,0,  'Payment Type',       'javascript:side_bar_link("LOOKUP", "ReceiptMode");'),
                    new Array(3,0,  'Currency',           'javascript:side_bar_link("LOOKUP", "ReceiptMode");'),
                    new Array(3,0,  'Charge Type',        'javascript:side_bar_link("LOOKUP", "ReceiptMode");'),
                    new Array(3,0,  'Receipt Mode',       'javascript:side_bar_link("LOOKUP", "ReceiptMode");'),
                    new Array(4,0,  'Purpose',            'javascript:side_bar_link("LOOKUP", "TranxPurpose");'),
                    new Array(5,0,  'Transaction Source', 'javascript:side_bar_link("LOOKUP", "TranxSource");'),
                    new Array(6,0,  'Source of Fund',     'javascript:side_bar_link("LOOKUP", "SourceOfFunds");'),
                    new Array(100,0,'Cancel',             'javascript:side_bar_link("CANCEL");')
                );
            }
        }

        return sidebar;
    }

    this.load_sidebar = function()
    {
        var side_array  = this.get_sidebar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);

        document.getElementById("div_transaction_menu").innerHTML = side_winder.toString();
    }
    
    this.load_unload_sidebar = function()
    {
        var div  = document.getElementById("div_transaction_menu");
        var cset = div.style.display;
                
        div.style.display = (cset == "block") ? "none" : "block";
        document.getElementById("anchor_off").innerText = (cset == "block") ? String.fromCharCode(187) : String.fromCharCode(171);
        document.getElementById('td_remittance').style.width = (cset == "block") ? "0%" : "20%";
    }

    this.side_bar_link_execute = function(modal_keycode, modal_return_value)
    {
        switch(modal_keycode.toUpperCase())
        {
            case "LOOKUP" : this.modal_lookup(modal_return_value); return;
        }
    }
    
    this.exec_modal_lookup = function(param)
    {
        var command_param = param.split("$");

        if (command_param != null)
        {
            if (command_param[0].toUpperCase() == "AJX_SELECTED")
            {
                var param_value = command_param[1].split("~");
                
                if (this.grd_search_keyword.toUpperCase() == "RECEIPTMODE")
                {
                    document.getElementById("lbl_receipt_mode").innerText = param_value[1];
                }
                else if (this.grd_search_keyword.toUpperCase() == "TRANXPURPOSE")
                {
                    document.getElementById("lbl_purpose").innerText = param_value[1];
                }
                else if (this.grd_search_keyword.toUpperCase() == "TRANXSOURCE")
                {
                    document.getElementById("lbl_transaction_source").innerText = param_value[1];
                }
                else if (this.grd_search_keyword.toUpperCase() == "SOURCEOFFUNDS")
                {
                    document.getElementById("lbl_source_of_fund").innerText = param_value[1];
                }
            }
        }

        this.grd_search.process_ajax(param);
    }    
    
    this.modal_lookup = function(keycode)
    {
        var grd_source  = "sp_otc_rem_lookup_modal~" + keycode + "~" + this.branch_code;

        this.grd_search_keyword = keycode;                       
        this.grd_search = new grid_selector();
        this.grd_search.open_selection_grid(null, grd_source, 0 , null);            
    }
    
    

//    this.modal_search_client = function()
//    {
//sp_otc_rem_product_setup(@keycode INT , @branch_code VARCHAR(10), @product_code BIGINT, @implementation_type BIGINT, @payment_type BIGINT, @currency BIGINT, @charge_type BIGINT, @computation_direction BIGINT) AS
//    }

//    this.modal_branch_product = function()
//    {sp_otc_rem_product_setup
//    }

//    this.modal_benef_product = function()
//    {
//    }
//    

//    
//    this.modal_override = function()
//    {
//    }

}