 //Email
var dbgrid_EM;
var dbgrid_MS;
var prod = new Array();
var impl = new Array();
var br_seq_id;
var filetype;
var fileformat;

function filemaintenance_dropdown()
{
    this.load_branches = function()
    {
        IRSWorld.GRSMaintenance.TranxFileMaintenance.getBranches(filemaintenance_dropdown.load_branches_callback);
    }
    
    this.load_branches_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddBranch", result.value, "SEQ_ID", "BranchName");
            this.commons = null;           
        }
    }
    
    this.branch_select = function()
    {
        br_seq_id = document.getElementById('ddBranch').value;
        IRSWorld.GRSMaintenance.TranxFileMaintenance.branchSelect(br_seq_id, filemaintenance_dropdown.branch_select_callback);
    }
    
    this.branch_select_callback = function(result)
    {
        filetype    =   document.getElementById('ddFileType').value.toString();
        fileformat  =   document.getElementById('ddFileFormat').value.toString();
        try
        {
            IRSWorld.GRSMaintenance.TranxFileMaintenance.GetEMGridValue(filetype,fileformat,filemaintenance_dropdown.GetEMGridValue_callback);
        }
        catch (e)
        {
        }
    }
    
    this.GetEMGridValue_callback = function(result)
    {
        try
        {
            create_grid_EM();
            ajax_show_status(true);
            
            dbgrid_EM.bind(result.value, 'tdMailAddress', 'gridMailAdd');
        }
        catch (e)
        {
        }
        ajax_show_status(false);
    }

    this.Load_FileType = function()
    {
        IRSWorld.GRSMaintenance.TranxFileMaintenance.getFileType("FileUploadType", filemaintenance_dropdown.load_ddFileType_callback);
    }
    
    this.load_ddFileType_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddFileType", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
   this.Load_FileFormat = function()
    {
        var file_type    =   document.getElementById('ddFileType').value;
        IRSWorld.GRSMaintenance.TranxFileMaintenance.getFileFormat(parseInt(file_type), filemaintenance_dropdown.load_ddFileFormat_callback);
    }
    
    this.load_ddFileFormat_callback = function(result)
    {
        if (result != null && result != "undefined")
        {   var file_type    =   document.getElementById('ddFileType').value;
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddFileFormat", result.value, "FldValue", "FldDescription");
            this.commons = null; 
        }
    }

    this.page_load = function()
    {
        filemaintenance_dropdown.load_branches();
        MailSetup();
        filemaintenance_dropdown.Load_FileType();
        new SideBar().construct_side_bar();
    }

    this.fileformat_load = function()
    {
        ft = document.getElementById('ddFileType').value;
        this.Load_FileFormat();
    }

 }  
   
function show(elements)
{
    var elem_list = elements.split('~');
    
    for (var i = 0; i < elem_list.length; i++)
    {
        document.getElementById(elem_list[i]).style.display = 'block';
    }
}

function hide(elements)
{
    var elem_list = elements.split('~');
    
    for (var i = 0; i < elem_list.length; i++)
    {
        document.getElementById(elem_list[i]).style.display = 'none';
    }
}

function MailSetup()
{
    show('trBranch');
    show('divEmailSetup');
    hide('divMethodSetup');
    hide('divBatchSetup');
}

function MethodSetup()
{
    hide('trBranch');
    show('divMethodSetup');
    hide('divEmailSetup');
    hide('divBatchSetup');
}

function BatchSetup()
{
    hide('trBranch');
    show('divBatchSetup');
    hide('divEmailSetup');
    hide('divMethodSetup');
}


// Create Table Grid Email
function create_grid_EM()
{
    dbgrid_EM = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'SEQ_ID';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'SEQ_ID';
        col.align = 'left';
        col.styles.cell += 'width:8%;display:none;';
        dbgrid_EM.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'FileType';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'FileType';
        col.align = 'left';
        col.styles.cell += 'width:8%;display:none;';
        dbgrid_EM.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'FileFormat';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'FileFormat';
        col.align = 'left';
        col.styles.cell += 'width:8%;display:none;';
        dbgrid_EM.addColumn(col);
    }
     {
        var col = new datagrid_column();
        col.header = 'Email';
        col.datafield = 'Email';
        col.align = 'left';
        col.styles.cell += 'width:75%;';
        col.editable = true;
        dbgrid_EM.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'Deleted';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'Deleted';
        col.align = 'left';
        col.styles.cell += 'width:8%;display:none;';
        dbgrid_EM.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'Upload ';
        col.datafield = 'Upload';
        col.align = 'center';
        col.type= 'check';
        col.styles.cell += 'width:5%';
        dbgrid_EM.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Process ';
        col.datafield = 'Process';
        col.align = 'center';
        col.type= 'check';
        col.styles.cell += 'width:3%';
        dbgrid_EM.addColumn(col);
    }



    dbgrid_EM.caption = 'RECEIVE AT';
    dbgrid_EM.classNames.table = 'table_body';
    dbgrid_EM.rows_page = 12;
    dbgrid_EM.row.normal_style = 'background-color: #f7ffff;'; 
    dbgrid_EM.footer.add_button.none = false;
    dbgrid_EM.footer.add_button.style += 'background-color: lavender;color:maroon;'; 
    dbgrid_EM.footer.edit_button.none = false;
    dbgrid_EM.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
    dbgrid_EM.footer.save_button.none = false;
    dbgrid_EM.footer.save_button.onclick = 'gridEM_btn_click';  
    dbgrid_EM.footer.save_button.style += 'background-color: lavender;';  
    dbgrid_EM.footer.delete_button.none = false;
    dbgrid_EM.footer.cancel_button.none = false;
    dbgrid_EM.footer.cancel_button.style += 'background-color: lavender;';
}

function populate_rec()
{
    filetype    =   document.getElementById('ddFileType').value.toString();
    fileformat  =   document.getElementById('ddFileFormat').value.toString();
    IRSWorld.GRSMaintenance.TranxFileMaintenance.GetEMGridValue(filetype,fileformat,bind_grid_EM);
    getProductCode();
    getImplementationType();
    IRSWorld.GRSMaintenance.TranxFileMaintenance.GetMSGridValue(filetype,fileformat,bind_grid_MS);
    IRSWorld.GRSMaintenance.TranxFileMaintenance.GetBSGridValue(filetype,fileformat,bind_grid_BS);
}

function bind_grid_EM(fileformat)
{
    if (fileformat.error != null)
    {
        spawn_errpage(fileformat.error.Message);
        return;
    }
    
    create_grid_EM();
    ajax_show_status(true);
    
    dbgrid_EM.bind(fileformat.value, 'tdMailAddress', 'gridMailAdd');
    ajax_show_status(false);
} 


function gridEM_btn_click(args) {
    filetype    = document.getElementById('ddFileType').value;
    fileformat  = document.getElementById('ddFileFormat').value;
    br          = document.getElementById('ddBranch').value;
    
    if (filetype < 1) {
        alert('Please select file type.');
        return;
    }
    if (fileformat < 1) {
        alert('Please select file format.');
        return;
    }
    if (br  < 1) {
        alert('Please select branch.');
        return;
    }
    
    if (args.value == dbgrid_EM.footer.save_button.value) {
        var process     = 'false';
        var upload      = 'false';
        
        var req = new Array( args.additions, args.edits );
        for (var i = 0; i < req.length; i++) {
            if (req[i].length > 0) {
                var arr = req[i].split('~');
                for (var ii = 0; ii < arr.length; ii++) {//[1].split('|');
                    var rec = arr[ii].split('|');
                    if (rec.length > 1) {
                        for (var iii = 0; iii < rec.length; iii++) {
                            if (rec[iii].indexOf('Email') > -1) {
                                var mail = rec[iii].split('^');
                                if (mail[1].length < 7 | mail[1].toUpperCase().indexOf('.COM') == -1 | mail[1].indexOf('@') == -1) {
                                    alert('Please enter a valid email.');
                                    //gv = dbgrid_EM;
                                    //gv.clearChanges();
                                    //dbgrid_EM.row.selected_data_row = rec[0];
                                    //gv.footerButtonClick('', 'Edit');
                                    dbgrid_EM.clearChanges();
                                    dbgrid_EM.createRows();
                                    dbgrid_EM.row.selected_data_row = rec[0];
                                    dbgrid_EM.footerButtonClick('', 'Edit');
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
                                   
        IRSWorld.GRSMaintenance.TranxFileMaintenance.applyEditsEM(args.additions, args.deletes, args.edits,filetype,fileformat, updates_saved);
    }
}

function updates_saved(cm)
{  
    if (cm.error != null)
    {
        alert(cm.error.Message);
        populate_rec();
        return;
    }
    alert('Record has been submitted for approval');
    populate_rec();
}
        

function bind_grid_MS(fileformat)
{
    if (fileformat.error != null)
    {
        spawn_errpage(fileformat.error);
        return;
    }
    
    create_grid_MS();
    ajax_show_status(true);
    
    dbgrid_MS.bind(fileformat.value, 'tdMethodSetup', 'gridMethodSetup');
    ajax_show_status(false);
} 

function bind_grid_BS(fileformat)
{
    if (fileformat.error != null)
    {
        spawn_errpage(fileformat.error);
        return;
    }
    
    create_grid_BS();
    ajax_show_status(true);
    
    dbgrid_BS.bind(fileformat.value, 'tdBatchSetup', 'gridBatchSetup');
    ajax_show_status(false);
} 

//Method Setup
function create_grid_MS()
{
    dbgrid_MS = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'SEQ_ID';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'SEQ_ID';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_MS.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'FileType';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'FileType';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_MS.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'FileFormat';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'FileFormat';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_MS.addColumn(col);
    }
     {
        var col = new datagrid_column();
        col.header = 'File Code';
        col.datafield = 'FileCode';
        col.align = 'left';
        col.styles.cell += 'width:10%';
        col.editable = true;
        dbgrid_MS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Product Code';
        col.datafield = 'ProductCode';
        col.align = 'center';
        col.type = 'select';
        col.options = prod;
        col.styles.cell += 'width:40%';
        dbgrid_MS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Implementation Type';
        col.datafield = 'ImplementationType';
        col.align = 'center';
        col.type = 'select';
        col.options = impl;
        col.styles.cell += 'width:40%';
        dbgrid_MS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'For Retrap';
        col.datafield = 'ForRetrap';
        col.align = 'center';
        col.type= 'check';
        col.styles.cell += 'width:10%';
        dbgrid_MS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Deleted';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'Deleted';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_MS.addColumn(col);
    }


    dbgrid_MS.caption = 'METHOD SETUP';
    dbgrid_MS.classNames.table = 'table_body';
    dbgrid_MS.rows_page = 12;
    dbgrid_MS.row.normal_style = 'background-color: #f7ffff;'; 
    dbgrid_MS.footer.add_button.none = false;
    dbgrid_MS.footer.add_button.style += 'background-color: lavender;color:maroon;'; 
    dbgrid_MS.footer.edit_button.none = false;
    dbgrid_MS.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
    dbgrid_MS.footer.save_button.none = false;
    dbgrid_MS.footer.save_button.onclick = 'gridMS_btn_click';  
    dbgrid_MS.footer.save_button.style += 'background-color: lavender;';  
    dbgrid_MS.footer.delete_button.none = false;
    dbgrid_MS.footer.cancel_button.none = false;
    dbgrid_MS.footer.cancel_button.style += 'background-color: lavender;';
}

function gridMS_btn_click(args)
{
    if (args.value == dbgrid_MS.footer.save_button.value)
    {
        filetype    =   document.getElementById('ddFileType').value;
        fileformat  =   document.getElementById('ddFileFormat').value;                            
        IRSWorld.GRSMaintenance.TranxFileMaintenance.applyEditsMS(args.additions, args.deletes, args.edits,filetype,fileformat, updates_saved);
    }
}

function getProductCode()
{
    var rows =  IRSWorld.GRSMaintenance.TranxFileMaintenance.getProductCode("ProductCode").value.Rows;
    prod = new Array();
    for(var i = 0;i <= rows.length - 1;i++)
    {
        prod[prod.length] = rows[i].FldValue + '~' + rows[i].FldDescription;
    }
}


function getImplementationType()
{  
   var rows =  IRSWorld.GRSMaintenance.TranxFileMaintenance.getImplementationType("ImplementationType").value.Rows;
   impl = new Array();
   for(var i = 0;i <= rows.length - 1;i++)
   {
        impl[impl.length] = rows[i].FldValue + '~' + rows[i].FldDescription;
   }
}


//Batch Setup
function create_grid_BS()
{
    dbgrid_BS = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'SEQ_ID';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'SEQ_ID';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_BS.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'FileType';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'FileType';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_BS.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'FileFormat';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'FileFormat';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_BS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Amount Limit';
        col.datafield = 'AmountLimit';
        col.align = 'left';
        col.styles.cell += 'width:20%';
        col.editable = true;
        dbgrid_BS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Batch Limit';
        col.datafield = 'BatchLimit';
        col.align = 'left';
        col.styles.cell += 'width:15%';
        col.editable = true;
        dbgrid_BS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'UserGroup';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'UserGroup';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_BS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'BatchFormat';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'BatchFormat';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_BS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Batchctr';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'Batchctr';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_BS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Batch Number Length';
        col.datafield = 'BatchNumLength';
        col.align = 'left';
        col.styles.cell += 'width:10%';
        col.editable = true;
        dbgrid_BS.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Deleted';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'Deleted';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid_BS.addColumn(col);
    }


    dbgrid_BS.caption = 'BATCH SETUP';
    dbgrid_BS.classNames.table = 'table_body';
    dbgrid_BS.rows_page = 12;
    dbgrid_BS.row.normal_style = 'background-color: #f7ffff;'; 
    dbgrid_BS.footer.add_button.none = false;
    dbgrid_BS.footer.add_button.style += 'background-color: lavender;color:maroon;'; 
    dbgrid_BS.footer.edit_button.none = false;
    dbgrid_BS.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
    dbgrid_BS.footer.save_button.none = false;
    dbgrid_BS.footer.save_button.onclick = 'gridBS_btn_click';  
    dbgrid_BS.footer.save_button.style += 'background-color: lavender;';  
    dbgrid_BS.footer.delete_button.none = false;
    dbgrid_BS.footer.cancel_button.none = false;
    dbgrid_BS.footer.cancel_button.style += 'background-color: lavender;';
}

function gridBS_btn_click(args)
{
    if (args.value == dbgrid_BS.footer.save_button.value)
    {
        filetype    =   document.getElementById('ddFileType').value;
        fileformat  =   document.getElementById('ddFileFormat').value;                            
        IRSWorld.GRSMaintenance.TranxFileMaintenance.applyEditsBS(args.additions, args.deletes, args.edits,filetype,fileformat, updates_saved);
    }
}

//SideBar
function SideBar()
{
    this.construct_side_bar = function()
    {
        document.getElementById('divSBTxnFileMaintenance').innerHTML = this.get_side_bar();
    }
    this.get_side_bar = function()
    {
        var side_array = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        
        return side_winder.toString();
    }
    
    this.get_side_bar_list = function()
    {
        var side_bar  = new Array
        (
            new Array(2, 0, 'Email Setup', 'MailSetup();'),
            new Array(3, 0, 'Method Of Transaction', 'MethodSetup();'),    
            new Array(4, 0, 'Batch Setup', 'BatchSetup();')            
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
        document.getElementById('tdSBTxnFileMaintenance').style.width = (cset == 'block') ? '0%' : '18%';
        this.construct_side_bar();
    }
}