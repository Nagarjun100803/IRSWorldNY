function execute_ajx_command(param)
{
    if (otherchannels_mode != null | otherchannels_mode != 'undefined')
    {
        switch (otherchannels_mode)
        {
            case 'DEF':
                if (def != null && def != 'undefined')
                {
                    def.do_ajax_grid_search(param);
                }
                break;
            
            case 'VIEW':
                if (vw != null && vw != 'undefined')
                {
                    vw.do_ajax_grid_search(param);
                }
                break;
            
            default: break;
        }
    }
}

function disable_element(element, disabled)
{
    var arr_element = element.split(',');
    
    if (arr_element.length > 0)
    {
        for (var i = 0; i < arr_element.length; i++)
        {
            document.getElementById(arr_element[i]).disabled = disabled;
        }
    }
}

function execute_ajaxgridview_command(param)
{
    if (otherchannels_mode != null | otherchannels_mode != 'undefined')
    {
        switch (otherchannels_mode)
        {
            case 'DEF':
//                if (def != null && def != "undefined")
//                {
//                    def.do_ajaxgridview(param);
//                }
                break;
            
            case 'VIEW':
                if (vw != null && vw != "undefined")
                {
                    vw.do_ajaxgridview(param);
                }
                break;
                
            case 'PROCESS':
                if (vw != null && vw != "undefined")
                {
                    vw.do_ajaxgridview(param);
                }
                break;
            
            default: break;
        }
    }
}

function do_nothing()
{
}

function clear_labels(element)
{
    var elem_list = element.split(',');
    
    for (var i = 0; i < elem_list.length; i++)
    {
        document.getElementById(elem_list[i]).innerText = '';
    }
}

function set_innerHTML(element, value)
{
    document.getElementById(element).innerHTML = '';
    document.getElementById(element).innerHTML = value;
}

function set_option_title(ddl_names)
{
    var ddl_id = ddl_names.split(',');
    
    for (var a = 0; a < ddl_id.length; a++)
    {
        var drpdwn = document.getElementById(ddl_id[a]);
        
        if (drpdwn.options.length > 0)
        {
            for (var i = 1; i < drpdwn.options.length; i++)
            {
                drpdwn.options[i].title = drpdwn.options[i].text;
            }
        }
    }
}

function dd_value(element)
{
    var dd = document.getElementById(element);
    
    if (dd.options.length > 0)
    {
        if (dd.selectedIndex > 0)
        {
            dd.title = dd[dd.selectedIndex].text;
        }
    }
}

function js_otherchannels_defaults()
{   
    this.arr_defaults       = null;
    this.defaults_mode      = null;
    this.ds_defaults        = null;
    this.def_index          = 0;
    
    //OtherChannels defaults items
    this.defaults_SEQ_ID    = 0;
    this.prod_charge_id     = 0;
    this.file_format        = 0;
    this.remit_cur          = 0;
    this.tranx_source       = 0;
    this.receipt_mode       = 0;
    this.tranx_purpose      = 0;
    this.source_funds       = 0;
    this.tranx_stat         = 0;
    
    this.grid_ajax_page     = "../AjxPage/ajaxgrid.aspx";
    this.ajax_grid          = "GRIDCLASSADD";
    this.grd_view           = null;
    //this.loc                = null;
    
    this.search_prod_charge = function()
    {
        if (this.remit_cur < 1)
        {
            alert('Invalid search. Please select remitting currency.');
            return;
        }
        
        var grd_query = null;
        this.ajax_grid = new grid_selector();
        
        if (this.remit_cur == null && this.remit_cur == 0)
        {
            alert('Invalid search.\nPlease select remitting currency.'); return;
        }
        
        grd_query = "SP_SRCH_OTHERCHANNELS_CHARGES~" + loc.value + "~" 
                    + this.remit_cur + "~";
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, grd_query, 0, this.prod_charge_id);
    }
    
    this.do_ajax_grid_search = function(param)
    {
        var command_param = null;
        this.ajax_grid.process_ajax(param);
        
        if (param != null && param != "undefined")
        {
            command_param = param.split("$");
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenFileFormat").value = command_param[1];
                if (command_param[0] == "ajx_selected")
                {
                    this.prod_charge_id = command_param[1];
                    this.show_prod_charg_grid();
                }
            }
        }
    }
        
    this.show_prod_charg_grid = function()
    {
        show_element('dgDefaults');
        
        if (this.prod_charge_id > 0 && this.prod_charge_id != null)
        {
            this.grd_view                   = new grid_viewer();
            this.grd_view.sql_source        = 'SP_OC_CHARGEMASTER~' + this.prod_charge_id
                                            + '~';
            this.grd_view.div_container    = document.getElementById('dgDefaults');
            this.grd_view.datarow_column    = 0;
            this.grd_view.open_grid_view();
        }
    }
    
    this.defaults_ini = function()
    {
        this.defaults_mode = 'VIEW';
        otherchannels_mode = 'DEF';
        js_com == new common_method();
        this.lbl_set_innerHTML(null);
        
        var query_fileformat = look_up_cmd('OtherChannels');
        disable_element('ddRemitCurrency', true);
        disable_element('ddFFDefaults', false);
        show_element('divOtherChannelsDefaults');
        remove_element('divSearch,divGrdControl,divGrid,divGrdControl,dgSelectDefaults,dgDefaults,divView,lnkPrevious,lnkNext');
        
        enable_button(document.getElementById('btnAdd'), true);
        enable_button(document.getElementById('btnPostSave'), false);
        enable_button(document.getElementById('btnEdit'), false);
        enable_button(document.getElementById('btnCancelDelete'), false);
        
        js_com.to_load_dropdownlist('ddFFDefaults', get_dataset(query_fileformat, false), 'VALUE', 'CODE');
    }
    
    this.lnkPrevious_click = function()
    {
        disable_element('lnkNext', false);
        document.getElementById('lnkPrevious').href = 'javascript:def.lnkNext_click';
        this.def_index = this.def_index - 1;
        if (this.def_index == 0)
        {
            disable_element('lnkPrevious',true);
            document.getElementById('lnkPrevious').href = 'javascript:do_nothing()';
        }
            
        this.populate_defaults(this.def_index);
    }
    
    this.lnkNext_click = function()
    {
        disable_element('lnkPrevious', false);
        document.getElementById('lnkPrevious').href = 'javascript:def.lnkPrevious_click()';
        if (this.def_index = this.ds_defaults.Tables[0].Rows.length)
        {
            disable_element('lnkNext', true)
            document.getElementById('lnkNext').href = 'javascript:do_nothing()';
        }
        this.def_index = this.def_index + 1;
        this.populate_defaults(this.def_index);
    }
    
    this.populate_defaults = function(index)
    {
        this.prod_charge_id  = this.ds_defaults.Tables[0].Rows[index].Prod_Charg_ID;
        this.defaults_SEQ_ID = this.ds_defaults.Tables[0].Rows[index].SEQ_ID;
        this.tranx_source    = this.ds_defaults.Tables[0].Rows[index].TranxSource_ID;
        this.receipt_mode    = this.ds_defaults.Tables[0].Rows[index].ReceiptMode_ID;
        this.tranx_purpose   = this.ds_defaults.Tables[0].Rows[index].TranxPurpose_ID;
        this.source_funds    = this.ds_defaults.Tables[0].Rows[index].SourceFunds_ID;
        this.remit_cur       = this.ds_defaults.Tables[0].Rows[index].RemitCurrency;
        this.tranx_stat      = this.ds_defaults.Tables[0].Rows[index].TransStatus_ID;
        
        this.lbl_set_innerHTML();
        this.show_prod_charg_grid();
    }
    
    this.ddFFDefaults_onchange = function()
    {
        var i = 0;
        var j = 1;
        switch (this.defaults_mode)
        {
            case 'EDIT':
                var answer = confirm('Cancel current procedure?');
                if (answer)
                {
                    otherchannels_mode = 'DEF';
                    js_com == new common_method();
                    disable_element('ddRemitCurrency', true);
                    remove_element('divSearch,divGrdControl,divGrid,divGrdControl,dgSelectDefaults,dgDefaults,divView');
                    this.defaults_mode = 'VIEW';
                }
                else
                {
                    document.getElementById('ddFFDefaults').selectedIndex = this.file_format;
                    return;
                }
                
                enable_button(document.getElementById('btnCancelDelete'), false);
                enable_button(document.getElementById('btnPostSave'), false);
                break;
            
            //case 'VIEW': break;
            
            case 'ADD':
                var file_format_add = document.getElementById('ddFFDefaults').selectedIndex;
                
                if (OtherChannels.defaults_exist(file_format_add).value)
                {
                    alert("Defaults is already defined for this file.");
                    this.defaults_ini();
                    return;
                }
                else
                {
                    this.file_format = document.getElementById('ddFFDefaults').selectedIndex;
                    return;
                }
                
            default: break;
        }
        
        this.file_format = document.getElementById('ddFFDefaults').selectedIndex;
        this.ds_defaults = OtherChannels.get_defaults(this.file_format).value;
        if (this.ds_defaults != null && this.ds_defaults.Tables[0].Rows.length > 0)
        {
            this.def_index = 0;
            if (this.ds_defaults.Tables[0].Rows.length > 1)
            {
                this.populate_defaults(this.def_index);
                show_element('lnkPrevious,lnkNext');
                disable_element('lnkPrevious',true);
            }
            else
            {
                this.populate_defaults(this.def_index);
                remove_element('lnkPrevious,lnkNext');
            }
            
            enable_button(document.getElementById('btnEdit'), true);
        }
        else 
        {
            enable_button(document.getElementById('btnEdit'), false);
            set_innerHTML('dgDefaults', 'NO DEFAULTS DEFINED FOR THIS FORMAT.');
            show_element('dgDefaults');
        }
    }
    
    this.get_defaults_values = function()
    {
        this.ds_defaults = OtherChannels.get_defaults(this.file_format).value;
        if (this.ds_defaults != null && this.ds_defaults.Tables[0].Rows.length > 0)
        {
//            this.prod_charge_id  = this.ds_defaults.Tables[0].Rows[0].Prod_Charg_ID;
            this.defaults_SEQ_ID = this.ds_defaults.Tables[0].Rows[0].SEQ_ID;
//            this.tranx_source    = this.ds_defaults.Tables[0].Rows[index].TranxSource_ID;
//            this.receipt_mode    = this.ds_defaults.Tables[0].Rows[index].ReceiptMode_ID;
//            this.tranx_purpose   = this.ds_defaults.Tables[0].Rows[index].TranxPurpose_ID;
//            this.source_funds    = this.ds_defaults.Tables[0].Rows[index].SourceFunds_ID;
//            this.remit_cur       = this.ds_defaults.Tables[0].Rows[index].RemitCurrency;
//            this.tranx_stat      = this.ds_defaults.Tables[0].Rows[index].TransStatus_ID;
        }
    }
    
    this.lbl_set_innerHTML = function()
    {
        if (this.ds_defaults != null && this.ds_defaults.Tables[0].Rows.length > 0)
        {
            document.getElementById('ddRemitCurrency').selectedIndex = this.remit_cur;
            set_innerHTML('tdTranxStat', '<label id="lblTranxStat"></label>');
            document.getElementById("lblTranxStat").innerText = this.ds_defaults.Tables[0].Rows[0].TransStatus;
            set_innerHTML('tdTranxSource', '<label id="lblTranxSource"></label>');
            document.getElementById("lblTranxSource").innerText = this.ds_defaults.Tables[0].Rows[0].TranxSource;
            set_innerHTML('tdReceiptMode', '<label id="lblReceiptMode"></label>');
            document.getElementById("lblReceiptMode").innerText = this.ds_defaults.Tables[0].Rows[0].ReceiptMode;
            set_innerHTML('tdTranxPurpose', '<label id="lblTranxPurpose"></label>');
            document.getElementById("lblTranxPurpose").innerText = this.ds_defaults.Tables[0].Rows[0].TranxPurpose;
            set_innerHTML('tdSourceFunds', '<label id="lblSourceFunds"></label>');
            document.getElementById("lblSourceFunds").innerText = this.ds_defaults.Tables[0].Rows[0].SourceFunds;
        }
        else
        {
            set_innerHTML('tdTranxStat', '<label id="lblTranxStat"></label>');
            set_innerHTML('tdTranxSource', '<label id="lblTranxSource"></label>');
            set_innerHTML('tdReceiptMode', '<label id="lblReceiptMode"></label>');
            set_innerHTML('tdTranxPurpose', '<label id="lblTranxPurpose"></label>');
            set_innerHTML('tdSourceFunds', '<label id="lblSourceFunds"></label>');
        }
    }
    
    this.lbl_set_val = function()
    {
        document.getElementById("lblTranxStat").text = 1;
    }
    
    this.lbl_post_innerHTML = function()
    {
        set_innerHTML('tdTranxStat', '<label id="lblTranxStat">' + document.getElementById('ddTranxStat')[document.getElementById('ddTranxStat').selectedIndex].text + '</label>');
        set_innerHTML('tdTranxSource', '<label id="lblTranxSource">' + document.getElementById('ddTranxSource')[document.getElementById('ddTranxSource').selectedIndex].text + '</label>');
        set_innerHTML('tdReceiptMode', '<label id="lblReceiptMode">' + document.getElementById('ddReceiptMode')[document.getElementById('ddReceiptMode').selectedIndex].text + '</label>');
        set_innerHTML('tdTranxPurpose', '<label id="lblTranxPurpose">' + document.getElementById('ddTranxPurpose')[document.getElementById('ddTranxPurpose').selectedIndex].text + '</label>');
        set_innerHTML('tdSourceFunds', '<label id="lblSourceFunds">' + document.getElementById('ddSourceFunds')[document.getElementById('ddSourceFunds').selectedIndex].text + '</label>');
    }
    
    this.dd_set_innerHTML = function()
    {
        set_innerHTML('tdTranxStat', '<select id="ddTranxStat" onmouseover="javascript:dd_value(this.id)"></select>');
        set_innerHTML('tdTranxSource', '<select id="ddTranxSource" onmouseover="javascript:dd_value(this.id)"></select>');
        set_innerHTML('tdReceiptMode', '<select id="ddReceiptMode" onmouseover="javascript:dd_value(this.id)"></select>');
        set_innerHTML('tdTranxPurpose', '<select id="ddTranxPurpose" onmouseover="javascript:dd_value(this.id)"></select>');
        set_innerHTML('tdSourceFunds', '<select id="ddSourceFunds" onmouseover="javascript:dd_value(this.id)"></select>');
    }
    
    this.set_item_values = function()
    {   
        var data_row = new Array();
        //var 
        data_row[data_row.length] = 'Prod_Charg_ID~'      + this.prod_charge_id;
        data_row[data_row.length] = 'FileFormat~'         + this.file_format;
        data_row[data_row.length] = 'RemitMethod~1';
        data_row[data_row.length] = 'RemitCurrency~'      + this.remit_cur;
        data_row[data_row.length] = 'TransStatus~'        + this.tranx_stat;
        data_row[data_row.length] = 'TransactionSource~'  + this.tranx_source;
        data_row[data_row.length] = 'ReceiptMode~'        + this.receipt_mode;
        data_row[data_row.length] = 'TransactionPurpose~' + this.tranx_purpose;
        data_row[data_row.length] = 'SourceOfFunds~'      + this.source_funds;
        data_row[data_row.length] = 'LastUpdate~'         + get_date();
        
        return data_row.join('$');
//        this.arr_defaults = this.file_format + '$'
//                          + this.tranx_source + '$' 
//                          + this.receipt_mode + '$'
//                          + this.tranx_purpose + ','
//                          + this.source_funds + ','
//                          + this.tranx_stat + ','
//                          + this.prod_charge_id;
    }
    
    this.remit_cur_select = function()
    {
        this.remit_cur = document.getElementById('ddRemitCurrency').selectedIndex;
    }
    
    this.defaults_dd_ini = function()
    {
        js_com.to_load_dropdownlist('ddRemitCurrency', OtherChannels.dd_remit_currency_val().value, 'VALUE', 'CODE');
        js_com.to_load_dropdownlist('ddTranxSource', OtherChannels.dd_tranx_source_val().value, 'VALUE', 'CODE');
        js_com.to_load_dropdownlist('ddReceiptMode', OtherChannels.dd_receipt_mode_val().value, 'VALUE', 'CODE');
        js_com.to_load_dropdownlist('ddTranxPurpose', OtherChannels.dd_tranx_purpose_val().value, 'VALUE', 'CODE');
        js_com.to_load_dropdownlist('ddSourceFunds', OtherChannels.dd_source_funds_val().value, 'VALUE', 'CODE');
        js_com.to_load_dropdownlist('ddTranxStat', OtherChannels.dd_tranx_stat_val().value, 'VALUE', 'CODE');
        
        set_option_title('ddTranxSource,ddRemitCurrency,ddReceiptMode,ddTranxSource,ddSourceFunds,ddTranxStat');
        set_innerHTML('tdPostSave', '<input onclick="javascript:def.btnPost_click()" id="btnPostSave" type="button" value="POST" class="button" style="width:66%"/>');
    }
    
    this.clear_defaults = function()
    {
        this.arr_defaults       = null;
        this.defaults_SEQ_ID     = 0;
        this.prod_charge_id     = 0;
        this.file_format        = 0;
        this.remit_cur          = 0;
        this.tranx_source       = 0;
        this.receipt_mode       = 0;
        this.tranx_purpose      = 0;
        this.source_funds       = 0;
        this.tranx_stat         = 0;
    }
    
    this.valid_items = function()
    {
        if (this.file_format < 1)
        {
            alert('File format cannot be empty.');
            return false;
        }
        
        if (this.remit_cur < 1)
        {
            alert('Remitting currency cannot be empty.');
            return false;
        }
        
        if (this.tranx_source < 1)
        {
            alert('Transaction source cannot be empty.');
            return false;
        }
        
        if (this.receipt_mode < 1)
        {
            alert('Receipt mode cannot be empty.');
            return false;
        }
        
        if (this.tranx_purpose < 1)
        {
            alert('Transaction purpose cannot be empty.');
            return false;
        }
        
        if (this.source_funds < 1)
        {
            alert('Source of funds cannot be empty.');
            return false;
        }
        
        if (this.prod_charge_id < 1)
        {
            alert('Product charge cannot be empty.');
            return false;
        }
        
        return true;
    }
    
    this.show_defaults = function()
    {
        var element = document.ElementById('divOtherChannelsDefaults');
        if (element.style.display == 'none')
        {
            element.style.display = 'block';
            document.getElementById('tdShowDefaults').innerHTML = '<a style="width:10%" id="lnkShowDefaults" href="#" onclick="javascript:def.show_defaults()">Hide</a>';
        }
        else if (element.style.display == 'block')
        {
            element.style.display = 'none';
            document.getElementById('tdShowDefaults').innerHTML = '<a style="width:20%" id="lnkShowDefaults" href="#" onclick="javascript:def.show_defaults()">Show Defaults</a>';
        }
    }
    
    this.btnAdd_click = function()
    {
        this.clear_defaults();
        this.dd_set_innerHTML();
        this.defaults_mode = 'ADD';
        js_com == new common_method();
        
        disable_element('ddRemitCurrency', false);
        show_element('dgSelectDefaults');
        remove_element('dgDefaults');
        enable_button(document.getElementById('btnAdd'), false);
        enable_button(document.getElementById('btnEdit'), false);
        enable_button(document.getElementById('btnPostSave'), true);
        enable_button(document.getElementById('btnCancelDelete'), true);
        document.getElementById('btnCancelDelete').value = 'CANCEL';
        
        this.defaults_dd_ini();
        set_innerHTML('tdPostSave', '<input onclick="javascript:def.btnPost_click()" id="btnPostSave" type="button" value="POST" class="button" style="width:66%"/>');
        document.getElementById('ddFFDefaults').selectedIndex = 0;
    }
    
    this.btnPost_click = function()
    {
        this.tranx_source  = document.getElementById('ddTranxSource').selectedIndex;
        this.receipt_mode  = document.getElementById('ddReceiptMode').selectedIndex;
        this.tranx_purpose = document.getElementById('ddTranxPurpose').selectedIndex;
        this.source_funds  = document.getElementById('ddSourceFunds').selectedIndex;
        this.tranx_stat  = document.getElementById('ddTranxStat').selectedIndex;
        
        if (!this.valid_items()) return;
        
        if (this.defaults_mode == 'ADD')
            OtherChannels.add_defaults(this.post_callback);
        else
            OtherChannels.update_defaults(this.defaults_SEQ_ID, this.post_callback);
    }
    
    this.btnSave_click = function()
    {
        var answer = confirm('Continue on saving...');
        if (answer)
        {
            var ret_val = OtherChannels.save_defaults(this.set_item_values(), this.commit_callback);   
        }
        else
        {
            return;
        }
    }
    
    this.btnEdit_click = function()
    {
        this.defaults_mode = 'EDIT';
        this.dd_set_innerHTML();
        this.defaults_dd_ini();
        
        document.getElementById('ddRemitCurrency').selectedIndex = this.remit_cur;
        document.getElementById('ddTranxSource').selectedIndex = this.tranx_source;
        document.getElementById('ddReceiptMode').selectedIndex = this.receipt_mode;
        document.getElementById('ddTranxPurpose').selectedIndex = this.tranx_purpose;
        document.getElementById('ddSourceFunds').selectedIndex = this.source_funds;
        document.getElementById('ddTranxStat').selectedIndex = this.tranx_stat;
        
        show_element('dgSelectDefaults');
        enable_button(document.getElementById('btnEdit'), false);
        enable_button(document.getElementById('btnAdd'), false);
        enable_button(document.getElementById('btnCancelDelete'), true);
        disable_element('dgDefaults,ddFFDefaults,ddRemitCurrency,btnPostSave', false);
        
        document.getElementById('btnCancelDelete').value = 'CANCEL';
        set_innerHTML('tdPostSave', '<input onclick="javascript:def.btnPost_click()" id="btnPostSave" type="button" value="POST" class="button" style="width:66%"/>');
    }
    
    this.btnCancel_click = function()
    {
        var answer = confirm('Cancel current procedure.\nDo you want to continue?');

        if (answer)
        {
            switch (otherchannels_mode)
            {
                case 'DEF':
                    document.getElementById('ddRemitCurrency').selectedIndex = -1;
                    if (this.file_format > 0)
                        this.show_defaults(otherchannels_mode);
                    else this.defaults_ini();
                    break;
                
                case 'PROCESS':
                    this.show_defaults(otherchannels_mode);
                    break;
                
                default: break;
            }
            disable_element('ddFFDefaults,dgDefaults',false);
        }
        else
        {
            switch (otherchannels_mode)
            {
                case 'PROCESS':
                    disable_element('ddFFDefaults', false);
                    return;
                
                case 'DEF':
                    return;
                
                default: break;
            }
        }
        
        this.defaults_mode = 'VIEW';
    }
    
    this.btnDelete_click = function()
    {
    }
        
    this.show_defaults = function(mode)
    {
        otherchannels_mode = mode;
        js_com == new common_method();
        this.lbl_set_innerHTML(null);
        
        var query_fileformat = look_up_cmd('OtherChannels');
        disable_element('ddRemitCurrency,ddFFDefaults', true);
        
        if (otherchannels_mode == 'PROCESS') enable_button(document.getElementById('btnAdd'), false);
        else enable_button(document.getElementById('btnAdd'), true);
        enable_button(document.getElementById('btnPostSave'), false);
        enable_button(document.getElementById('btnEdit'), true);
        enable_button(document.getElementById('btnCancelDelete'), false);
        
        js_com.to_load_dropdownlist('ddFFDefaults', get_dataset(query_fileformat, false), 'VALUE', 'CODE');
        js_com.to_load_dropdownlist('ddRemitCurrency', OtherChannels.dd_remit_currency_val().value, 'VALUE', 'CODE');
        
        var ds = OtherChannels.get_defaults(this.file_format).value;
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {
            this.prod_charge_id = ds.Tables[0].Rows[0].Prod_Charg_ID;
            this.defaults_SEQ_ID = ds.Tables[0].Rows[0].SEQ_ID;
            this.tranx_source   = ds.Tables[0].Rows[0].TranxSource_ID;
            this.receipt_mode   = ds.Tables[0].Rows[0].ReceiptMode_ID;
            this.tranx_purpose  = ds.Tables[0].Rows[0].TranxPurpose_ID;
            this.source_funds   = ds.Tables[0].Rows[0].SourceFunds_ID;
            this.remit_cur      = ds.Tables[0].Rows[0].RemitCurrency;
            this.tranx_stat     = ds.Tables[0].Rows[0].TransStatus_ID;
            
            this.lbl_set_innerHTML(ds);
            this.show_prod_charg_grid();
        }
        else 
        {
            set_innerHTML('dgDefaults', 'NO DEFAULTS DEFINED FOR THIS FORMAT.');
            show_element('dgDefaults');
        }
        
        document.getElementById('ddRemitCurrency').selectedIndex = this.remit_cur;
        document.getElementById('ddFFDefaults').selectedIndex = this.file_format;
        show_element('divOtherChannelsDefaults,dgDefaults');
    }
    
    //call backs
    this.post_callback = function(result)
    {
        if (result != null && result != 'undefined')
        {
            if (!result.value)
            {
                var msg = OtherChannels.get_defaults_error();
                alert(msg.value);
                return;
            }
            
            def.lbl_post_innerHTML();
            disable_element('dgDefaults,ddFFDefaults,ddRemitCurrency', true);
            enable_button(document.getElementById('btnEdit'), true);
            remove_element('dgSelectDefaults');
            set_innerHTML('tdPostSave', '<input onclick="javascript:def.btnSave_click()" id="btnPostSave" type="button" value="SAVE" class="button" style="width:66%"/>');
        }
    }
    
    this.commit_callback = function(result)
    {
        if (result != null && result != 'undefined')
        {
            if (!result.value)
            {
                var msg = OtherChannels.get_defaults_error();
                alert(msg.value);
                return;
            }
            
            alert('RECORD SUCCESSFULY SAVED.');
            def.defaults_ini();
//            set_innerHTML('tdPostSave', '<input onclick="javascript:def.btnPost_click()" id="btnPostSave" type="button" value="POST" class="button" style="width:66%"/>');
//            enable_button(document.getElementById('btnEdit'), true);
//            enable_button(document.getElementById('btnCancelDelete'), true);
//            enable_button(document.getElementById('btnAdd'), false);
//            enable_button(document.getElementById('btnPostSave'), false);
        }
    }
}