var js_com             = null;
var otherchannels_mode = null;
var loc                = null;
var sidebar            = null;

function page_load()
{
    loc = OtherChannels.get_branch();
    sidebar = new SideBar();
    js_com = new common_method();
    otherchannels_mode = 'VIEW';
    
    var qRecStat = look_up_cmd("UploadedRecordStatus");
    var qFileFormat = look_up_cmd("OtherChannels");
    var qFileStatus = look_up_cmd("FileUploadStatus");
    
    sidebar.get_side_bar_visibility('divOtherChannelsMenu','btnAnchor');
    show_element('divSearch,divOtherChannels');
    remove_element('divGrdControl,divGrid,divView,divOtherChannelsDefaults');
    
    js_com.to_load_dropdownlist("ddFileFormat", get_dataset(qFileFormat, false), "VALUE", "CODE");
    js_com.to_load_dropdownlist('ddFileStatus', get_dataset(qFileStatus, false), 'VALUE', 'CODE');
    ddRecordStatus_opt();
    //js_com.to_load_dropdownlist("ddRecordStatus", get_dataset(qRecStat, false), "VALUE", "CODE");
    
    document.getElementById('txtDateFrom').value = get_date();
    document.getElementById('txtDateTo').value = get_date();
    document.getElementById('lblOtherChannels').innerText = 'Other Channels';
}

function ddRecordStatus_opt()
{   
    var opt = document.createElement('option');
    document.getElementById('ddRecordStatus').options.length = 0;
    opt.value = null;
    opt.text = '';
    
    try
    {
        document.getElementById('ddRecordStatus').add(opt,null); // NON INTERNET EXPLORER BROWSER
    }
    catch(ex)
    {
        document.getElementById('ddRecordStatus').add(opt); // INTERNET EXPLORER BROWSER
    }
    
    var options = new Array 
    (   
        'REJECTED',
        'VALID'
    );
    
    for (i = 0; i < options.length; i++)
    {
        var opt = document.createElement('option');
        opt.value = i;
        opt.text = options[i];
        
        try
        {
            document.getElementById('ddRecordStatus').add(opt,null);// NON INTERNET EXPLORER BROWSER
        }
        catch(ex)
        {
            document.getElementById('ddRecordStatus').add(opt);// INTERNET EXPLORER BROWSER
        }
    }
}

function get_date()
{
    var dt = new Date();
    var month = parseInt(dt.getMonth()) + 1;
    return month.toString() + '/' + dt.getDate() + '/' + dt.getFullYear();
}

function get_dataset(sqlQuery, bStoredProc)
{
    var ds = OtherChannels.getDataSet(sqlQuery, bStoredProc).value;
    
    if (ds != null)
        return ds;
    else
        return null;
}
    
function look_up_cmd(fldGroup)
{
    var sQuery = "SELECT fldCode [CODE], fldValue [VALUE] FROM TBL_LOOKUP"
               + " WHERE fldgroup = '" + fldGroup + "'";

    return sQuery;
}
    
function show_element(elements)
{
    var elem_list = elements.split(',');
    
    for (var i = 0; i < elem_list.length; i++)
    {
        document.getElementById(elem_list[i]).style.display = 'block';
    }
}
    
function remove_element(elements)
{
    var elem_list = elements.split(',');
    
    for (var i = 0; i < elem_list.length; i++)
    {
        document.getElementById(elem_list[i]).style.display = 'none';
    }
}

function js_otherchannels()
{
    this.gridView           = null;
    
    this.recordStatus       = null;
    
    //file info
    this.file_format        = null;
    this.batch_id           = null;
    this.date_uploaded      = null;
    
    //remitter info
    this.file_status        = null;
    this.fSEQ_ID            = null;
    this.remit_name         = null;
    this.remit_address      = null;
    this.remit_ccy          = null;
    
    //benef info
    this.trace_no           = null;
    this.benef_name         = null;
    this.benef_address      = null;
    this.casa_no            = null;
    this.acct_type          = null;
    this.branch_acct        = null;
    this.acct_ccy           = null;
    
    //transaction info    
    this.RefNo              = null;
    this.record_count       = null;

    this.amount             = null;
    this.special_msg        = null;
    this.effective_date     = null;
    
    //ajax grid
    this.rSEQ_ID            = null;
    this.grid_ajax_page     = "../AjxPage/ajaxgrid.aspx";
    this.ajax_grid          = "GRIDCLASSADD";
    
    //View OtherChannels
    this.do_ajax_grid_search = function(parameter)
    {
        this.ajax_grid.process_ajax(parameter);
        
        var command_param = null;
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenFileFormat").value = command_param[1];
            
                if (command_param[0] == "ajx_selected")
                {
                    this.fSEQ_ID = command_param[1];
                    this.create_grid_no_status();
                }
            }
        }
    }
    
    this.create_grid_no_status = function()
    {
        var stmt = "SELECT FI.FileFormat, FS.FldCode FROM TBL_FILE_INVENTORY FI "
                    + "LEFT JOIN TBL_LOOKUP FS ON FS.FldValue = FI.Status "
                    + "WHERE FI.SEQ_ID = '" + this.fSEQ_ID
                    + "' AND FS.FldGroup = 'FileUploadStatus'";
    
        //this.file_status = exec_scalar(stmt).value;
        var ds = get_dataset(stmt, false);
        
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {
            this.file_format = ds.Tables[0].Rows[0].FileFormat;
            this.file_status = ds.Tables[0].Rows[0].FldCode;
            
            if (def != null && def != 'undefined')
            {
                def.file_format = this.file_format;
                def.get_defaults_values();
            }
        }
        
        remove_element('divView,divOtherChannelsDefaults');
        show_element('divGrdControl,divGrid');
        
        this.status_changed();
    }
    
    this.do_ajaxgridview = function(parameter)
    {        
        var command_param = null;
        this.ajax_grid.process_ajax(parameter);
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenFileFormat").value = command_param[1];
                
                if (command_param[0] == "ajx_selected")
                {
                    this.rSEQ_ID = command_param[1];
                    this.load_record();
                }
            }
        }
    }
    
    this.load_record = function()
    {
        this.pull_server_data();
    }
    
    this.close_view = function()
    {
        hidepanel('divView');
    }
    
    this.pull_server_data = function()
    {
        if (this.rSEQ_ID != null | this.rSEQ_ID != 'undefined')
        {
            OtherChannels.get_otherchannels_records(this.rSEQ_ID, this.pull_server_data_callback);
            showpanel('divPopProcess');
        }
    }
    
    this.pull_server_data_callback = function(result)
    {
        if (result != null | result.value != null)
        {
            var ds = result.value;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.length > 0)
                {
                    this.file_format         = ds.Tables[0].Rows[0].FileFormat.toString();
                    //this.batch_id            = ds.Tables[0].Rows[0].BatchID.toString();
                    //this.date_uploaded       = ds.Tables[0].Rows[0].dateuploaded.toString();
                    this.remit_name          = ds.Tables[0].Rows[0].RemitterInfo.toString();
                    //this.remit_address = ds
                    this.benef_name          = ds.Tables[0].Rows[0].BeneficiaryInfo.toString();
                    //this.benef_address
                    //this.casa_no             = ds.Tables[0].Rows[0].AccountNo.toString();
                    //this.acct_type           = ds.Tables[0].Rows[0].AcctType.toString();
                    //this.branch_acct         = ds.Tables[0].Rows[0].BranchName.toString();
                    this.acct_ccy            = ds.Tables[0].Rows[0].AcctCurrency.toString();
                    //this.ref_no              = ds.Tables[0].Rows[0].OtherChannelsRefNo.toString();
                    this.amount              = ds.Tables[0].Rows[0].Amount.toString();
                    //this.special_msg         = ds.Tables[0].Rows[0].SpecialMessage.toString();
                    //this.effective_date      = ds.Tables[0].Rows[0].EffectivityDate.toString();
                    //this.trace_no            = ds.Tables[0].Rows[0].ExternalRefNo.toString();
                    
                    document.getElementByID("spnRemitterName").innerText = this.remit_name;
                    document.getElementByID("spnRemitterAddress").innerText = this.remit_address;
                    document.getElementByID("spnRemitCurrency").innerText = this.remit_ccy;
                    document.getElementByID("spnBenefName").innerText = this.benef_name;
                    document.getElementByID("spnBenefAddress").innerText = this.benef_address;
                    document.getElementByID("spnAcctCur").innerText = this.acct_ccy;
                }
            }
        }
        hidepanel('divPopProcess');
        showpanel('divView');
    }
    
    this.panel_search_otherchannels = function()
    {
        otherchannels_mode = 'VIEW';
        var file_status_code = document.getElementById("ddFileStatus").selectedIndex;
        var date_from = document.getElementById('txtDateFrom');
        var date_to = document.getElementById('txtDateTo');
        var grid_query = null;
        this.ajax_grid = new grid_selector();
        this.file_status = document.getElementById("ddFileStatus")[document.getElementById('ddFileStatus').selectedIndex].text;
        
        js_com = new common_method();
        
        if (date_from.value != null && date_from.value != '')
        {
            if (!isValidDate(date_from.value))
            {
                alert('Invalid date format!');
                date_from.focus();
                date_from.select();
                return;
            }
        }
        
        if (date_to.value != null && date_to.value != '')
        {
            if (!isValidDate(date_to.value))
            {
                alert('Invalid date format');
                date_to.focus();
                date_to.select();
                return;
            }
        }
        
        grid_query = "sp_search_otherchannels~"
               + document.getElementById("ddFileFormat").value + "~"
               + file_status_code + "~" + date_from.value + "~" + date_to.value + "~";
        
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, grid_query, 0, this.rSEQ_ID);
    }
    
    this.create_grid = function()
    {
        remove_element('divSearch');
        show_element('divOtherChannels,divGrid');
        
        document.getElementById("lblRecordListHead").innerText          = "Uploaded Transactions";// for the file " + this.batch_id;
        document.getElementById('lblOtherChannels').innerText           = 'Other Channels - ' + this.file_format;
        
        if (this.fSEQ_ID != null && this.fSEQ_ID > 0)
        {
            this.gridView                   = new grid_viewer();
            this.gridView.sql_source    = "SP_GRIDVIEW_OTHERCHANNELS~" + this.fSEQ_ID + "~" + this.recordStatus + "~";
            this.gridView.div_container     = document.getElementById("dgRecords");
            this.gridView.datarow_column    = 0;
            this.gridView.open_grid_view();
        }
    }
    
    //Process OtherChannels
    this.process_file = function()
    {
        var to_process = confirm('Continue processing...');
        if (to_process)
        {
            if (def.defaults_SEQ_ID != null & def.defaults_SEQ_ID > 0)
            {
                OtherChannels.process_otherchannels(this.fSEQ_ID, this.process_callback);
                var comon = new common_method();
                showpanel('divPopProcess');
                comon.to_show_loading_message('divLoadingMsg');
            }
            else
            {
                alert("Cannot process file. No Defaults defined for this format.");
            }
        }
        else return;
    }
    
    this.process_callback = function(result)
    {
        hidepanel('divPopProcess');
        
        if (result.value)
        {
            var rec_count = OtherChannels.count_record().value;
            var rec_processed = OtherChannels.count_processed().value;
            
            alert('Batch Succesfully processed. Total of ' + rec_processed.toString() + ' out of ' + rec_count.toString() + ' records.'
                            + '\n\nPlease view Log for details.');
                            
            var answer = confirm('Process another file?');
            if (answer)
            {
                page_load();
            }
            else
            {
                window.location = 'http://localhost:3550/Main/IRSMain.aspx';
            }
        }
        else
        {
            alert('Processing stopped. Records might be invalid.\nPlease check the uploaded file.');
            page_load();
        }
    }
    
    this.status_changed = function()
    {
        this.recordStatus = document.getElementById("ddRecordStatus")[document.getElementById("ddRecordStatus").selectedIndex].text;
        var ds = OtherChannels.grid_otherchannels(this.fSEQ_ID, this.recordStatus).value;
        this.record_count = ds.Tables[0].Rows.length;
        this.create_grid();
        
        var element = document.getElementById('btnProcessFile');
        //var grd = document.getElementById('grd_ajax');
        if (this.recordStatus == 'VALID')
        {
            if (this.file_status == 'New')
            {
                if (this.record_count > 0)
                {
//                    def.file_format = this.file_format;
//                    def.defaults_mode = 'VIEW';
//                    def.show_defaults('PROCESS');
//                    document.getElementById("divOtherChannelsDefaults").style.display = 'none';
//                    document.getElementById('divShowDefaults').style.display = 'block';
//                    document.getElementById('lnkShowDefaults').innerHTML= 'Show Defaults';
//                    document.getElementById('lnkShowDefaults').style.display = 'block';
                    element.style.display = 'block';
                }
                return;
            }
        }
        
        remove_element('divView,btnProcessFile,divOtherChannelsDefaults');
    }
}

//OTHERCHANNELS SIDE BAR
function SideBar()
{
    this.construct_side_bar = function()
    {
        document.getElementById('divOtherChannelsMenu').innerHTML = this.get_side_bar();
    }
    
    this.get_side_bar = function()
    {
        var side_array = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        
        return side_winder.toString();
    }
    
    this.get_side_bar_list = function()
    {
        var side_bar;
        
        side_bar = new Array
        (
            new Array(2, 0, 'Search File', 'page_load()'),
            new Array(3, 0, 'Set Defaults', 'def.defaults_ini()')
        );
        
        return side_bar;
    }
    
    this.get_side_bar_visibility = function(division_id, anchor_id)
    {
        var div = document.getElementById(division_id);
        var anc = document.getElementById(anchor_id);
        var cset = div.style.display;
        
        div.style.display = (cset == 'block') ? 'none' : 'block';
        
        anc.innerText = (cset == 'block') ? String.fromCharCode(187) : String.fromCharCode(171);
        document.getElementById('tdOtherChannelsMenu').style.width = (cset == 'block') ? '0%' : '18%';
        this.construct_side_bar();
    }
}