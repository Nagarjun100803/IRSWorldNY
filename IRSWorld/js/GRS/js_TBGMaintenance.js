var dbgrid;
var currency = new Array();

function TBG_Maintenance()
{
    this.Load_RecordID = function()
    {
        IRSWorld.GRS.TBG_MAINTENANCE.getRecordID("IBatch_Record_ID", this.load_RecordID_callback);
    }
    
    this.load_RecordID_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddRecordID", result.value, "FldCode", "FldDescription");
            this.commons = null;           
        }
    }
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'SEQ_ID';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'seq_id';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'record_id';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'record_id';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid.addColumn(col);
    }
    
    {
        var col = new datagrid_column();
        col.header = 'Remitter Name';
        col.datafield = 'rem_name';
        col.align = 'left';
        col.styles.cell += 'width:18%;';
        col.editable = true;
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'Beneficiary Name';
        col.datafield = 'benf_name';
        col.align = 'left';
        col.styles.cell += 'width:20%;';
        col.editable = true;
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'Amount';
        col.datafield = 'amount';
        col.align = 'left';
        col.onchange = 'validate_input';
        col.styles.cell += 'width:10%;';
        col.editable = true;
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'Account Number';
        col.datafield = 'acct_num';
        col.align = 'left';
        col.styles.cell += 'width:14%;';
        col.editable = true;
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'Account Currency';
        col.datafield = 'acctcurr_cd';
        col.styles.cell += 'width:20%;';
        col.align = 'center';
        col.type = 'select';
        col.options = currency;
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'IsEdited';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'IsEdited';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();
        col.header = 'DateCreated_Edited';
        col.styles.header += 'display:none;';
        col.styles.datafield += 'display:none;';
        col.datafield = 'DateCreated_Edited';
        col.align = 'left';
        col.styles.cell += 'display:none;';
        dbgrid.addColumn(col);
    }


    dbgrid.caption = '';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
    dbgrid.footer.add_button.none = false;
    dbgrid.footer.add_button.style += 'background-color: lavender;color:maroon;'; 
    dbgrid.footer.edit_button.none = false;
    dbgrid.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
    dbgrid.footer.save_button.none = false;
    dbgrid.footer.save_button.onclick = 'grid_btn_click';  
    dbgrid.footer.save_button.style += 'background-color: lavender;';  
    dbgrid.footer.cancel_button.none = false;
    dbgrid.footer.cancel_button.style += 'background-color: lavender;';
}

function populate_rec()
{
    var recordID    =   document.getElementById('ddRecordID').value;
    
    getCurrency();
    IRSWorld.GRS.TBG_MAINTENANCE.GetGridValue(recordID,bind_grid);
    
}

function bind_grid(recordID)
{
    if (recordID.error != null)
    {
        spawn_errpage(recordID.error);
        return;
    }
    
    create_grid();
    ajax_show_status(true);
    
    dbgrid.bind(recordID.value, 'tdTBGmaint', 'gridTBGmaint');
    ajax_show_status(false);
} 

function getCurrency()
{  
   var rows =  IRSWorld.GRS.TBG_MAINTENANCE.getCurrency("Currency").value.Rows;
   currency = new Array();
   
   for(var i = 0;i <= rows.length - 1;i++)
   {     
        currency[currency.length] = rows[i].FldValue + '~' + rows[i].FldDescription;
   }
}

function grid_btn_click(args)
{
    if (args.value == dbgrid.footer.save_button.value)
    {
        var recordID    =   document.getElementById('ddRecordID').value;
                                   
        IRSWorld.GRS.TBG_MAINTENANCE.applyEdits(args.additions, args.edits, recordID, updates_saved);
    }
}

function updates_saved(cm)
{  
    if (cm.error != null)
    {
        alert(cm.error);
        populate_rec();
        return;
    }     
    populate_rec();
}

function validate_input(args)
{
    
    var amount = document.getElementById(args.control).value;
    
    var isvalid = isNumeric(amount);
    if(!isvalid)
    {
        alert('Error! Amount should be numeric.');
        document.getElementById(args.control).value = '';
        return;
    }
}   