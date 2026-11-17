var uid;
var dbgrid;
var dt;
var seq_id
function get_branches()
{
    IRSWorld.APAS.APASPNBBranches.getBranches(get_branches_callback)
}

function get_branches_callback(result)
{
    this.commons           = null;
    if (result.value != null && result.value != "undefined")
    {
        this.commons = new common_method();
        this.commons.to_load_dropdownlist("ddlBranches", result.value, "BranchCode", "BranchName");
    }
}

function page_setup()
{
    get_branches();
    uid = IRSWorld.APAS.APASPNBBranches.get_irsclient_data('UID').value;
    populate_rec();
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = '';
        col.datafield = 'checked';
        col.align = 'left';
        col.type= 'check';
        col.onclick = 'onhit_checkbox';
        col.styles.cell += 'width:5%';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'CurrencyCode';
        col.datafield = 'CurrencyCode';
        col.align = 'left';
        col.styles.cell += 'width:10%';
        col.editable = false;
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Currency';
        col.datafield = 'Currency';
        col.align = 'left';
        col.styles.cell += 'width:20%';
        col.editable = false;
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'SEQ ID TEMP';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'SEQ_ID_TEMP';
        col.align = 'left';
        col.styles.cell += 'width:8%;display:none;';
        col.editable = false;
        dbgrid.addColumn(col);
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
	    dbgrid.addColumn(col);
    }
    
}

function populate_rec()
{
    if(document.getElementById('ddlBranches').value != "null")
    {
        IRSWorld.APAS.APASPNBBranches.getCurrencyList(document.getElementById('ddlBranches').value,bind_grid);
        document.getElementById('tblButtons').style.display = 'block';
        document.getElementById('tdGrid').style.display = 'block';
    }
    else
    {
        document.getElementById('tblButtons').style.display = 'none';
        document.getElementById('tdGrid').style.display = 'none';
    }
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
    dbgrid.bind(result.value, 'tdGrid', 'grid1');
    ajax_show_status(false);
} 

function save_allowed_currencies()
{
    var brcode = document.getElementById('ddlBranches').value;
    var ischeck;
    var ctr=0;
    var error_selected=0;
    if(brcode == 'null')
    {
        alert('Branch code must not be empty!');
        return;
    }
//    haspending = APASPNBBranches.haspending(brcode).value;
//    if(haspending)
//    {
//        alert('Error');
//        return;
//    }
    if(confirm('Do you want to proceed with the changes?'))
    {
        for(var i=0; i <= dbgrid.datatable.Rows.length - 1;i++)
        {
            seq_id_temp = dbgrid.getValueAt(i,3);
            seq_id = dbgrid.getValueAt(i,4);
            ischeck = dbgrid.getValueAt(i,0);
            var isExisting = IRSWorld.APAS.APASPNBBranches.isExisting(brcode,dbgrid.getValueAt(i,1)).value;

            if(ischeck)
            {
                if(!isExisting)
                {
                    var data_row = new Array();
                    data_row[data_row.length] = (seq_id == "") ? "SEQ_ID_TEMP~" + 0 : "SEQ_ID_TEMP~"  + seq_id;     
                    data_row[data_row.length] = "isForApproval~"        + true;
                    data_row[data_row.length] = "AddedBy~"              + uid;
                    data_row[data_row.length] = "DateAdded~"            + dateToString(IRSWorld.APAS.APASPNBBranches.get_time_zone().value);    
                    data_row[data_row.length] = (isExisting) ? 'Action~UPDATE' : 'Action~INSERT';
                    data_row[data_row.length] = "BranchCode~"           + brcode;
                    data_row[data_row.length] = "Currency~"             + dbgrid.getValueAt(i,1);
                    var s = data_row.join('|');
                    ctr++;
                    IRSWorld.APAS.APASPNBBranches.do_process_apas('INSERT',s,seq_id,save_allowed_currencies_callback);
                } 
            }
            else
            {
                if(isExisting)
                {
                    var data_row = new Array();
                    data_row[data_row.length] = "Deleted~"              + true;
                    data_row[data_row.length] = "Action~"               + "DELETE";
                    data_row[data_row.length] = "BranchCode~"           + brcode;
                    data_row[data_row.length] = "Currency~"             + dbgrid.getValueAt(i,1); 
                    data_row[data_row.length] = "isForApproval~"        + true;
                    data_row[data_row.length] = "AddedBy~"              + uid;
                    data_row[data_row.length] = "DateAdded~"            + dateToString(IRSWorld.APAS.APASPNBBranches.get_time_zone().value);    
                    data_row[data_row.length] = (seq_id_temp == "") ? "SEQ_ID_TEMP~" + 0 : "SEQ_ID_TEMP~"  + seq_id_temp;     
                    var s = data_row.join('|');
                    ctr++;
                    IRSWorld.APAS.APASPNBBranches.do_process_apas('INSERT',s,seq_id,save_allowed_currencies_callback); 
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
        populate_rec();
    }
}

function onhit_checkbox(args)
{
    var grid_data = args.id.split('_');
    var branch = document.getElementById('ddlBranches').value;
    var has_pending = IRSWorld.APAS.APASPNBBranches.haspending(branch,dbgrid.getValueAt(grid_data[3],1)).value;
    var can_continue=true;
        
    if(has_pending)
    {
        alert('This currency has pending for approval');
        populate_rec();
    }
}


function save_allowed_currencies_callback(result)
{
    if(result.error != null)
    {
        alert(result.error.Message);
        return
    }
    
}