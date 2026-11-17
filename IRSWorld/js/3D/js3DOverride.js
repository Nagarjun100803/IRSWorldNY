

function execute_ajaxgridview_command(param)
{
    extract_table_click(param);
}


function display_search()
{
    var courier = document.getElementById('ddlCourier').value;
    var proc_date = document.getElementById('txtDate').value;
    this.grd_source = "sp_get_3D_Details_for_override~" + "261" + "~" + courier + "~" + proc_date;
    this.grd_search  = new grid_selector();
    this.grd_search.open_selection_grid(null, this.grd_source, 0 , null);
}


function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'Reference Number';
        col.datafield = 'ReferenceNoOurs';
        col.align = 'left';
        col.styles.cell += 'width:auto';
        col.editable = false;
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'RemitterName';
        col.datafield = 'RemName';
        col.align = 'left';
        col.editable = false;
        col.styles.cell += 'width:auto';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Beneficiary Name';
        col.datafield = 'BenefName';
        col.align = 'left';
        col.editable = false;
        col.styles.cell += 'width:auto';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'RemittingBank';
        col.datafield = 'RemittingBank';
        col.align = 'left';
        col.editable = false;
        col.styles.cell += 'width:auto';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Amount';
        col.datafield = 'NetToBeneAmount';
        col.align = 'left';
        col.editable = false;
        col.styles.cell += 'width:auto';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'SEQ ID';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'tag';
        col.align = 'left';
        col.styles.cell += 'width:8%;display:none;';
        dbgrid.addColumn(col);
    }

    dbgrid.row.onclick = 'grid_override_details_per_seq';
    dbgrid.caption = '3D';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 10;
    dbgrid.row.normal_style = 'background-color: #f7ffff;';
}



function getdropdown()
{
    this.Load_Courier = function()
    {
        _DOverride.load_Courier(this.Load_Courier_callback);
    }
    
    this.Load_Courier_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlCourier", result.value, "alias", "Courier");
            
        }
    }
    
    this.LoadReason = function()
    {
        _DOverride.get3DReason(this.LoadReason_callback);
    }
    
    this.LoadReason_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlReason", result.value, "FldValue", "FldDescription");
            
        }
    }
    
    this.LoadStatus = function()
    {
        _DOverride.get3DStatus(this.LoadStatus_callback);
    }
    
    this.LoadStatus_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlStatus", result.value, "FldValue", "FldDescription");
            
        }
    }
    
}


function page_setup()
{
    new getdropdown().Load_Courier();
    new getdropdown().LoadReason();
    new getdropdown().LoadStatus();
    set_visibility('page');
}

function set_visibility(str)
{
    if(str == 'Search')
    {
        document.getElementById('tblSearch').style.display = 'none';
        document.getElementById('tblTranDetails').style.display = 'block';
    }
    else    
    {
        document.getElementById('tblSearch').style.display = 'block';
        document.getElementById('tblTranDetails').style.display = 'none';
    }
}


function get_override_details()
{
    var courier = document.getElementById('ddlCourier').value;
    var proc_date = document.getElementById('txtDate').value;
    _DOverride.sp_get_3D_Details_for_override('261',courier,proc_date,0,bind_grid)
}


function bind_grid(result)
{
    var dt = result.value;
    if(result == null | result == 'undefined')
    {
        spawn_errpage(result.error);
        return;
    }
    create_grid();
    dbgrid.bind(result.value, 'tbl_data', 'grids');
}

function grid_override_details_per_seq(args)
{
    document.getElementById('txtSeq_Hidden').value = dbgrid.getValueAt(args.datarow,5);
    _DOverride.sp_get_3D_Details_for_override('','','',dbgrid.getValueAt(args.datarow,5),get_override_details_callback)    
}


function get_override_details_callback(result)
{
    var dt = result.value;
    if(result == null | result == 'undefined')
    {
        spawn_errpage(result.error);
        return;
    }
    
    if(dt.Rows.length > 0)
    {
        setText(document.getElementById('lblRefNoOurs'),dt.Rows[0].ReferenceNoOurs);
        setText(document.getElementById('lblTranxDate'), dt.Rows[0].TranDate);
        setText(document.getElementById('lblRefnumTheirs'), dt.Rows[0].ReferenceNoTheirs);
        setText(document.getElementById('lblProdNm'), dt.Rows[0].ProductNm);
        setText(document.getElementById('lblRemittanceDate') , dateToString(dt.Rows[0].TranDate));
        setText(document.getElementById('lblProdType'),  "");
        setText(document.getElementById('lblRemittingBank'), dt.Rows[0].RemittingBank);
        setText(document.getElementById('lblAmount'), dt.Rows[0].NetToBeneAmount);
        setText(document.getElementById('lblRemitterName'), dt.Rows[0].RemName);
        setText(document.getElementById('lblBeneficiaryName'), dt.Rows[0].BenefName);
        setText(document.getElementById('lblRemitterAddress'), dt.Rows[0].RemAddress);
        setText(document.getElementById('lblBeneficiaryAddress'), dt.Rows[0].BenefAddress);
        
//        document.getElementById('lblTranxDate').value = dt.Rows[0].TranDate;
//        document.getElementById('lblRefnumTheirs').value = dt.Rows[0].ReferenceNoTheirs;
//        document.getElementById('lblProdNm').value = dt.Rows[0].ProductNm;
//        document.getElementById('lblRemittanceDate').value = dt.Rows[0].TranDate;
//        document.getElementById('lblProdType').value = "";
//        document.getElementById('lblRemittingBank').value = dt.Rows[0].RemittingBank;
//        document.getElementById('lblAmount').value = dt.Rows[0].NetToBeneAmount;
//        document.getElementById('lblRemitterName').value = dt.Rows[0].RemName;
//        document.getElementById('lblBeneficiaryName').value = dt.Rows[0].BenefName;
//        document.getElementById('lblRemitterAddress').value = dt.Rows[0].RemAddress;
//        document.getElementById('lblBeneficiaryAddress').value = dt.Rows[0].BenefAddress;
        set_visibility('Search');
    }
}

function update_status()
{
    _DOverride.update_status(parseInt(document.getElementById('txtSeq_Hidden').value),document.getElementById('ddlStatus').value,update_status_callback);
}

function update_status_callback(result)
{
    if(result == null | result == 'undefined')
    {
        spawn_errmsg(result.error);
        return;
    }
    alert('Status successfully updated');
    page_setup();
}