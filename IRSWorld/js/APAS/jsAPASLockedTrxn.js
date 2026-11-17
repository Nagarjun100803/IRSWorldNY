var brcode;
var db_grid;
var seq_id;
var lockedby;
var uid;
function page_setup()
{
    brcode = APASLockedTransaction.get_irsclient_data('LOCATION').value;
    uid = APASLockedTransaction.get_irsclient_data('UID').value;
    populate_rec();
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'Implementation Date';
        col.datafield = 'Implem_Date';
        col.align = 'right';
        col.styles.cell += 'width:auto';
        dbgrid.addColumn(col);
    }
//    {
//	    var col = new datagrid_column();
//	    col.header = 'RefNo (Theirs)';
//	    col.datafield = 'RefNoTheirs';
//	    col.align = 'left';
//	    col.styles.cell += 'width:auto';
//	    dbgrid.addColumn(col);
//    }
    {
	    var col = new datagrid_column();
	    col.header = 'RefNo (Ours)';
	    col.datafield = 'RefNoOurs';
	    col.align = 'left';
	    col.onclick  = 'get_data';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Beneficiary Fullname';
	    col.datafield = 'BeneFull_Name';
	    col.align = 'left';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'CurrencyCode';
	    col.datafield = 'Currency';
	    col.align = 'left';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'AmountPayable';
	    col.datafield = 'AmountPayable';
	    col.align = 'left';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Task';
	    col.datafield = 'Task';
	    col.align = 'left';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'LockedBy';
	    col.datafield = 'LockedBy';
	    col.align = 'left';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
     {
	    var col = new datagrid_column();
	    col.header = 'LockedDate';
	    col.datafield = 'LockedDate';
	    col.align = 'left';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'BranchName';
	    col.datafield = 'BranchName';
	    col.align = 'left';
	    col.styles.cell += 'width:auto';
	    dbgrid.addColumn(col);
    }
    
    dbgrid.caption = 'Locked Transactions';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;';  
    dbgrid.row.onclick = 'get_data';
}



function get_data(args)
{
    var s = args.value.split('_');
    var refno = dbgrid.getValueAt(s[2],1);
    lockedby = dbgrid.getValueAt(s[2],6); //lockedby column
    if(uid != lockedby)  
    {
        document.getElementById('btnUnlock').style.display = 'none';
    }
    else
    {
        document.getElementById('btnUnlock').style.display = 'block';
    }
    
    if(brcode == '1170')
    {
        document.getElementById('btnUnlock').style.display = 'block';
    }
    
    
    var stat = 5;
    APASLockedTransaction.getAPASInfo(refno,stat,get_data_callback);
}

function get_data_callback(data)
{
    if(data.error != null)
    {
        spawn_errpage(currgrp.error);
        return;
    }

    set_details(data.value);
    show_hide_form('gridDetails','none');
    show_hide_form('frmDetails','block');
}

function show_hide_form(tblname,hidden)
{
    var tbl = document.getElementById(tblname);
    tbl.style.display = hidden;
    
}


function set_details(dataset)
{   
    if (dataset != null && typeof(dataset) == "object" && dataset.Tables != null)
    {   
        document.getElementById('ImplemDate').innerHTML         = dateToString(dataset.Tables[0].Rows[0]['Implem_Date']) == null ? '' : dateToString(dataset.Tables[0].Rows[0]['Implem_Date']);
        document.getElementById('RefNoTheirs').innerHTML        = dataset.Tables[0].Rows[0]['RefNoTheirs'] == null ? '' : dataset.Tables[0].Rows[0]['RefNoTheirs'];
        document.getElementById('RefNoOurs').innerHTML          = dataset.Tables[0].Rows[0]['RefNoOurs'] == null ? '' : dataset.Tables[0].Rows[0]['RefNoOurs']; 
        document.getElementById('BeneName').innerHTML           = dataset.Tables[0].Rows[0]['BeneFull_Name'];
        document.getElementById('BeneAdd').innerHTML            = dataset.Tables[0].Rows[0]['Bene_Address'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_Address'];
        document.getElementById('BeneContactNo').innerHTML      = dataset.Tables[0].Rows[0]['Bene_ContactNum'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_ContactNum'];
        document.getElementById('SpecialMSG').innerHTML         = dataset.Tables[0].Rows[0]['SpecialMessage'] == null ? '' : dataset.Tables[0].Rows[0]['SpecialMessage'];
        document.getElementById('RemName').innerHTML            = dataset.Tables[0].Rows[0]['RemFull_Name'];
        seq_id                                                  = dataset.Tables[0].Rows[0]['SEQ_id'];            
        document.getElementById('Amount').innerHTML             = formatNumber(dataset.Tables[0].Rows[0]['Amount'],2) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['Amount'],2);            
        document.getElementById('lbl1').innerText               = dataset.Tables[0].Rows[0]['LockedBy'];
        document.getElementById('lblDate').innerText            = dateToString(dataset.Tables[0].Rows[0]['LockedDate']);
    }
}

function populate_rec()
{
    //APASLockedTransaction.ordbranchcode();
    APASLockedTransaction.getLockStatus(brcode,bind_grid);
    show_hide_form('gridDetails','block');
    show_hide_form('frmDetails','none');
}

function bind_grid(currgrp)
{
    if (currgrp.error != null)
    {
        spawn_errpage(currgrp.error);
        return;
    }
    
    create_grid();
    dbgrid.bind(currgrp.value, 'tblLocked', 'gridProcessedTrnx');
} 


function unlock_transaction()
{
    if(confirm('Do you want to unlock this transaction?'))
    {
        var data_row = new Array();

        data_row[data_row.length] = "isLocked~"               + false; 
        data_row[data_row.length] = "LockedDate~"             + '';
        data_row[data_row.length] = "LockedBy~"               + ''; 
        var s = data_row.join('|');
            

        APASLockedTransaction.do_process_apas('UPDATE',s,seq_id,unlock_transaction_callback); 
        alert('Transaction successfully unlocked');
        populate_rec();
        show_hide_form('gridDetails','block');
        show_hide_form('frmDetails','none');
    }
}

function unlock_transaction_callback(result)
{
    if (result.value == null)
    {
    }        
    else
    {
        alert(result.value);
    }

}