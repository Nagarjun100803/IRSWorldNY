var dbgrid;
var dbgrid_curr;
var uid;
var subbrcode;
var seq_id;

function get_branches()
{
    APASSubBranch.getListPartners(get_branches_callback)
}

function get_branches_callback(result)
{
    this.commons           = null;
    if (result != null && result != "undefined")
    {
        this.commons = new common_method();
        this.commons.to_load_dropdownlist("ddlPartners", result.value, "MotherBranchCode", "MotherBranchName");
    }
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'BranchCode';
        col.datafield = 'BranchCode';
        col.align = 'left';
        col.styles.cell += 'width:10%';
        col.editable = false;
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'BranchName';
        col.datafield = 'BranchName';
        col.align = 'left';
        col.styles.cell += 'width:20%';
        col.editable = false;
        dbgrid.addColumn(col);
    }
    dbgrid.row.onclick = 'get_currency';
    
}

function create_grid_curr()
{
    dbgrid_curr = new datagrid();
    {
        var col = new datagrid_column();
        col.header = '';
        col.datafield = 'checked';
        col.align = 'left';
        col.type= 'check';
        col.styles.cell += 'width:5%';
        dbgrid_curr.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'CurrencyCode';
        col.datafield = 'CurrencyCode';
        col.align = 'left';
        col.styles.cell += 'width:10%';
        col.editable = false;
        dbgrid_curr.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Currency';
        col.datafield = 'Currency';
        col.align = 'left';
        col.styles.cell += 'width:20%';
        col.editable = false;
        dbgrid_curr.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'SEQ ID';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'SEQ_ID_TEMP';
        col.align = 'left';
        col.styles.cell += 'width:8%;display:none;';
        dbgrid_curr.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = '';
        col.datafield = '';
        col.align = 'center';
        col.type = 'button';
        col.value = 'Edit';
        col.edit_button = true;
        col.styles.cell += 'width:2%';
        col.styles.datafield += 'width:100%;';
        dbgrid_curr.addColumn(col);
    }
    
    
    dbgrid_curr.classNames.table = 'table_body';
    dbgrid_curr.rows_page = 12;
    dbgrid_curr.row.normal_style = 'background-color: #f7ffff;';  

}





function page_setup()
{
    get_branches();
    uid = APASSubBranch.get_irsclient_data('UID').value;
}


function getSubBranch(partner_code)
{
    APASSubBranch.getSubBranch(partner_code,bind_grid);
}

function bind_grid(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error);
        return;
    }
    
    create_grid();
    ajax_show_status(true);
    dbgrid.bind(result.value, 'tdListSubBranch', 'grid2');
    ajax_show_status(false);
}


function bind_grid_curr(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error);
        return;
    }
    
    create_grid_curr();
    ajax_show_status(true);
    dbgrid_curr.bind(result.value, 'tdCurrency', 'grid1');
    ajax_show_status(false);
}


function get_currency(args)
{
    hide_details('');
    subbrcode = dbgrid.getValueAt(args.datarow,0)
    APASSubBranch.getCurrencyPerPartner(document.getElementById('ddlPartners').value,subbrcode,bind_grid_curr);
}

function hide_details(tblnm)
{
    if(tblnm == 'Currency')
    {
        document.getElementById('tdCurrency').style.display = 'none';
        document.getElementById('tdListSubBranch').style.display = 'block';
        document.getElementById('tblButtons').style.display = 'none';
    }
    else
    {
        document.getElementById('tdCurrency').style.display = 'block';
        document.getElementById('tdListSubBranch').style.display = 'none';
        document.getElementById('tblButtons').style.display = 'block';
    }
}

function save_details()
{
    //var subbrcode = dbgrid.getValueAt(args.gridrow,0);
    var ischeck;
    var ctr=0;
    var error_selected=0;
    
    if(confirm('Do you want to proceed with the changes?'))
    {
        for(var i=0; i <= dbgrid_curr.datatable.Rows.length - 1;i++)
        {
            seq_id_temp = dbgrid_curr.getValueAt(i,3);
            seq_id = dbgrid_curr.getValueAt(i,4);
            ischeck = dbgrid_curr.getValueAt(i,0);
            var isExisting = APASSubBranch.isExisting(subbrcode,dbgrid_curr.getValueAt(i,1)).value;

            if(ischeck)
            {
                if(!isExisting)
                {
                    var data_row = new Array();
                    data_row[data_row.length] = (seq_id == "") ? "SEQ_ID_TEMP~" + 0 : "SEQ_ID_TEMP~"  + seq_id_temp;     
                    data_row[data_row.length] = "isForApproval~"        + true;
                    data_row[data_row.length] = "AddedBy~"              + uid;
                    data_row[data_row.length] = "DateAdded~"            + dateToString(APASSubBranch.get_time_zone().value);    
                    data_row[data_row.length] = (isExisting) ? 'Action~UPDATE' : 'Action~INSERT';
                    data_row[data_row.length] = "SubBranchCode~"           + subbrcode;
                    data_row[data_row.length] = "CurrencyCode~"             + dbgrid_curr.getValueAt(i,1);
                    var s = data_row.join('|');
                    ctr++;
                    APASSubBranch.do_process_apas('INSERT',s,seq_id,save_allowed_currency_sub_branch);
                } 
            }
            else
            {
                if(isExisting)
                {
                    var data_row = new Array();
                    data_row[data_row.length] = "Deleted~"              + true;
                    data_row[data_row.length] = "Action~"               + "DELETE";
                    data_row[data_row.length] = "SubBranchCode~"        + subbrcode;
                    data_row[data_row.length] = "CurrencyCode~"         + dbgrid_curr.getValueAt(i,1); 
                    data_row[data_row.length] = "isForApproval~"        + true;
                    data_row[data_row.length] = "AddedBy~"              + uid;
                    data_row[data_row.length] = "DateAdded~"            + dateToString(APASSubBranch.get_time_zone().value);    
                    data_row[data_row.length] = (seq_id_temp == "") ? "SEQ_ID_TEMP~" + 0 : "SEQ_ID_TEMP~"  + seq_id_temp;     
                    var s = data_row.join('|');
                    ctr++;
                    APASSubBranch.do_process_apas('INSERT',s,seq_id,save_allowed_currency_sub_branch); 
                }
            }
        }
    }
    if(ctr <= 0)
    {
        alert('No record selected.');
        return;
    }
    else if(error_selected > 0)
    {
        alert('Some of selected fields are now queued for approval');
    }
    else
    {
        alert('Setup already queued for approval');
        page_setup();
    }
}

function save_allowed_currency_sub_branch(result)
{
    if(result.error != null)
    {
        alert(result.error);
        return
    }
}