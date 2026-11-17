var dbgrid;
var uid;
var brcode;

function page_setup()
{
    list_table_names();
    populate_rec();
    uid = IRSWorld.APAS.APASApprovalWindow.get_irsclient_data('UID').value;
    brcode = IRSWorld.APAS.APASApprovalWindow.get_irsclient_data('LOCATION').value;
}


function test(args)
{
    dbgrid.setRowToEditMode(args.datarow,args.datarow,true);
}


function create_grid(tbl_name)
{
    if(tbl_name == '1')
    {
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = '';
            col.datafield = 'checked';
            col.align = 'left';
            col.type= 'check';
            col.styles.cell += 'width:5%';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'BranchCode';
            col.datafield = 'BranchCode';
            col.align = 'left';
            col.styles.cell += 'width:auto';
            col.editable = false;
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Payout Currency';
	        col.datafield = 'Payout_Curr';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Transaction Limit';
	        col.datafield = 'TransactionLimit';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Account Number';
	        col.datafield = 'AcctNum';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Action';
	        col.datafield = 'Action';
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
	        col.datafield = 'SEQ_ID';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Add/Modify By';
	        col.datafield = 'AddedBy';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
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
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID TEMP';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID_TEMP';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }  
      
        
        
        dbgrid.caption = 'For Approval';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 12;
        dbgrid.row.normal_style = 'background-color: #f7ffff;';  
        //dbgrid.row.onclick = 'test';
    }
    else if(tbl_name == '2')
    {
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = '';
            col.datafield = '';
            col.align = 'left';
            col.type= 'check';
            col.styles.cell += 'width:5%';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'Currency';
            col.datafield = 'CurrencyDesc';
            col.align = 'left';
            col.editable = false;
            col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'ORD AccountNo';
	        col.datafield = 'ORDAccountNo';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Add/Modify By';
	        col.datafield = 'AddedBy';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        
        {
	        var col = new datagrid_column();
	        col.header = 'Action';
	        col.datafield = 'Action';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
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
        
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID TEMP';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID_TEMP';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }  
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }
        dbgrid.caption = 'For Approval';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 12;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
    }
    
    else if(tbl_name == '3')
    {
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = '';
            col.datafield = '';
            col.align = 'left';
            col.type= 'check';
            col.styles.cell += 'width:5%';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'BranchCode';
            col.datafield = 'BranchCode';
            col.align = 'left';
            col.editable = false;
            col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'CurrencyCode';
	        col.datafield = 'CurrencyCode';
	        col.align = 'left';
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Description';
	        col.datafield = 'CurrencyDesc';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Add/Modify By';
	        col.datafield = 'AddedBy';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Action';
	        col.datafield = 'Action';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
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
        
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID TEMP';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID_TEMP';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }  
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }
        dbgrid.caption = 'For Approval';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 12;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
    }
    else if(tbl_name == '4')
    {
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = '';
            col.datafield = '';
            col.align = 'left';
            col.type= 'check';
            col.styles.cell += 'width:5%';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'BranchCode';
            col.datafield = 'BranchCode';
            col.align = 'left';
            col.editable = false;
            col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'FeeCode';
	        col.datafield = 'FeeCode';
	        col.align = 'left';
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Description';
	        col.datafield = 'Description';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Minimum Processed Transaction';
	        col.datafield = 'MinNum';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Maximum Processed Transaction';
	        col.datafield = 'MaxNum';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Fee Amount';
	        col.datafield = 'FeeAmount';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Add/Modify By';
	        col.datafield = 'AddedBy';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Action';
	        col.datafield = 'Action';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
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
	        col.styles.cell += 'width:5%';
	        col.styles.datafield += 'width:100%;';
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
	        dbgrid.addColumn(col);
        }  
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }
        dbgrid.caption = 'For Approval';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 12;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
    }
    else if(tbl_name == '5')
    {
        dbgrid = new datagrid();
        {
            var col = new datagrid_column();
            col.header = '';
            col.datafield = '';
            col.align = 'left';
            col.type= 'check';
            col.styles.cell += 'width:5%';
            dbgrid.addColumn(col);
        }
        {
            var col = new datagrid_column();
            col.header = 'SubBranchCode';
            col.datafield = 'SubBranchCode';
            col.align = 'left';
            col.editable = false;
            col.styles.cell += 'width:auto';
            dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'CurrencyCode';
	        col.datafield = 'CurrencyCode';
	        col.align = 'left';
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Description';
	        col.datafield = 'CurrencyDesc';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Add/Modify By';
	        col.datafield = 'AddedBy';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
	        dbgrid.addColumn(col);
        }
        {
	        var col = new datagrid_column();
	        col.header = 'Action';
	        col.datafield = 'Action';
	        col.align = 'left';
	        col.editable = false;
	        col.styles.cell += 'width:auto';
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
        
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID TEMP';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID_TEMP';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }  
        {
	        var col = new datagrid_column();
	        col.header = 'SEQ ID';
	        col.styles.header += 'display:none;';
	        col.styles.datafield += 'display:none;';
	        col.datafield = 'SEQ_ID';
	        col.align = 'left';
	        col.styles.cell += 'width:8%;display:none;';
	        dbgrid.addColumn(col);
        }
        dbgrid.caption = 'For Approval';
        dbgrid.classNames.table = 'table_body';
        dbgrid.rows_page = 12;
        dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
    }
    

}


function list_table_names()
{
    var DDL = document.getElementById('ddlTables');
    var proc = document.createElement('option');
    proc.text = 'PAYOUT PARTNER SETUP';
    proc.value = '1';
    DDL.options.add(proc);
    
    var can = document.createElement('option');
    can.text = 'PAYOUT CURRENCY';
    can.value = '2';
    DDL.options.add(can);
    
    var all = document.createElement('option');
    all.text = 'PAYOUT ALLOWED CURRENCIES';
    all.value = '3';
    DDL.options.add(all);
    
    var fee = document.createElement('option');
    fee.text = 'PAYOUT PARTNER FEE';
    fee.value = '4';
    DDL.options.add(fee);
    
    var sub = document.createElement('option');
    sub.text = 'SUB BRANCH ALLOWED CURRENCIES';
    sub.value = '5';
    DDL.options.add(sub);
    
    
    document.getElementById('ddlTables').value = 1;
}

function populate_rec()
{
    var tbl_name = (document.getElementById('ddlTables').value== '') ? '0' : document.getElementById('ddlTables').value;
    if(tbl_name != '0')
    {
        IRSWorld.APAS.APASApprovalWindow.getTables(tbl_name,bind_grid);
    }
}

function bind_grid(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error.Message);
        return;
    }
    create_grid(document.getElementById('ddlTables').value);
    dbgrid.bind(result.value, 'tdGrid', 'grids');
}


function approve_cancel_setup(tbl_indx)
{
    if(tbl_indx == 1) //PAYOUT PARTNER SETUP
    {
//        var data_row = new Array();
//        data_row[data_row.length] = "isLocked~"               + bool; 
//        data_row[data_row.length] = (bool == false) ? "LockedBy~"               +  '' : "LockedBy~"               + this.UserName;
//        data_row[data_row.length] = (bool == false) ? "LockedDate~"             +  '' : "LockedDate~"             + dateToString(APASMain.get_branch_localtime(this.BranchCode).value);
//        var s = data_row.join('|');
//        APASMain.do_process_apas(instruction,s,cls.SEQ_ID,this.locked_unlocked_tranx_callback); 
    }
}


function get_datagrid_value_approved(tbl_indx)
{
var ctr =0;
    if(dbgrid.datatable.Rows.length <= 0)
    {
        alert('No record to approve!');
        return;
    }
    if(confirm('Do you want to approve selected setup?'))
    {
        for(var i=0; i<= dbgrid.datatable.Rows.length - 1; i++)
        {
            var tempvalue = dbgrid.getValueAt(i,0);
            var seq_id;
            if(tempvalue)
            {
            ctr++;
                
                if(tbl_indx == '1')
                {
                    seq_id = dbgrid.getValueAt(i,6);
                    var data_row = new Array();
                    data_row[data_row.length] = "forApproval~"              + false; 
                    data_row[data_row.length] = "ApprovedBy~"               + uid; 
                    data_row[data_row.length] = "DateApproved~"             + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_callback); 
                }
                else if(tbl_indx == '2')
                {
                    seq_id = dbgrid.getValueAt(i,7);
                    var data_row = new Array();
                    data_row[data_row.length] = "isForApproval~"            + false; 
                    data_row[data_row.length] = "ApprovedBy~"               + uid; 
                    data_row[data_row.length] = "ApprovedDate~"             + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_callback); 
                }
                else if(tbl_indx == '3')
                {
                    seq_id = dbgrid.getValueAt(i,8);
                    var data_row = new Array();
                    data_row[data_row.length] = "isForApproval~"            + false; 
                    data_row[data_row.length] = "ApprovedBy~"               + uid; 
                    data_row[data_row.length] = "ApprovedDate~"             + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_callback); 
                }
 
                else if(tbl_indx == '4')
                {
                    seq_id = dbgrid.getValueAt(i,11);
                    var data_row = new Array();
                    data_row[data_row.length] = "forApproval~"            + false; 
                    data_row[data_row.length] = "ApprovedBy~"               + uid; 
                    data_row[data_row.length] = "DateApproved~"             + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_callback); 

                }
                else if(tbl_indx == '5')
                {
                    seq_id = dbgrid.getValueAt(i,8);
                    var data_row = new Array();
                    data_row[data_row.length] = "isforApproval~"            + false; 
                    data_row[data_row.length] = "ApprovedBy~"               + uid; 
                    data_row[data_row.length] = "ApprovedDate~"             + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_callback); 

                }
            }
           
        }
       if(ctr>0)
       { 
            alert('Setup Successfully Approved');
            populate_rec();
       }
       else
       {
            alert('Please select an item(s) to approve');
       }
       
    }
}

function get_datagrid_value_callback(result)
{
    if(result.error != null)
    {
        alert(result.error); 
    }
}

function get_datagrid_value_cancelled(tbl_indx)
{
var ctr=0;
    if(dbgrid.datatable.Rows.length <= 0)
    {
        alert('No record to cancel!');
        return;
    }
    if(confirm('Do you want to cancel selected setup?'))
    {
        for(var i=0; i<= dbgrid.datatable.Rows.length - 1; i++)
        {
            var tempvalue = dbgrid.getValueAt(i,0);
            var seq_id;
            if(tempvalue)
            {
            ctr++;
                
                if(tbl_indx == '1')
                {
                    seq_id = dbgrid.getValueAt(i,6);
                    var data_row = new Array();
                    data_row[data_row.length] = "isCancelled~"              + true; 
                    data_row[data_row.length] = "CancelledBy~"              + uid; 
                    data_row[data_row.length] = "DateCancelled~"            + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    data_row[data_row.length] = "forApproval~"            + false; 
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_cancelled_callback); 
                }
                else if(tbl_indx == '2')
                {
                    seq_id = dbgrid.getValueAt(i,7);
                    var data_row = new Array();
                    data_row[data_row.length] = "isCancelled~"            + true; 
                    data_row[data_row.length] = "CancelledBy~"            + uid; 
                    data_row[data_row.length] = "CancelledDate~"          + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    data_row[data_row.length] = "isForApproval~"          + false; 
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_cancelled_callback); 
                }
                else if(tbl_indx == '3')
                {
                    seq_id = dbgrid.getValueAt(i,8);
                    var data_row = new Array();
                    data_row[data_row.length] = "isCancelled~"            + true; 
                    data_row[data_row.length] = "CancelledBy~"            + uid; 
                    data_row[data_row.length] = "CancelledDate~"          + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    data_row[data_row.length] = "isforApproval~"            + false; 
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_cancelled_callback); 
                }
                else if(tbl_indx == '4')
                {
                    seq_id = dbgrid.getValueAt(i,11);
                    var data_row = new Array();
                    data_row[data_row.length] = "isCancelled~"            + true; 
                    data_row[data_row.length] = "CancelledBy~"            + uid; 
                    data_row[data_row.length] = "DateCancelled~"          + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    data_row[data_row.length] = "forApproval~"            + false; 
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_cancelled_callback); 
                }
                else if(tbl_indx == '5')
                {
                    seq_id = dbgrid.getValueAt(i,8);
                    var data_row = new Array();
                    data_row[data_row.length] = "isCancelled~"            + true; 
                    data_row[data_row.length] = "CancelledBy~"            + uid; 
                    data_row[data_row.length] = "CancelledDate~"          + dateToString(IRSWorld.APAS.APASApprovalWindow.get_time_zone().value);
                    data_row[data_row.length] = "isforApproval~"            + false; 
                    var s = data_row.join('|');
                    IRSWorld.APAS.APASApprovalWindow.do_process_apas('UPDATE',s,seq_id,tbl_indx,get_datagrid_value_cancelled_callback); 
                }
            }
        }
        if(ctr>0)
        {
            alert('Setup successfully cancelled'); 
            populate_rec();  
        }
        else
        {
            alert('Please select an item(s) to cancel');
        }
    } 
    
}

function get_datagrid_value_cancelled_callback(result)
{
    if(result.error != null)
    {
        alert(result.error);
        return;
    }
}

