var dbgrid;
var dt = new Array();
var payout_curr;
var uid;
var feecode;
var isEditable;

function load_dropdown()
{
    this.load_branches = function()
    {
        APASPayoutPartnerFees.load_payout_partners(this.load_branches_callback);
    }
    
    this.load_branches_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlPayoutPartner", result.value, "BranchCode", "BranchName");
        }
    }
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'Fee Code';
        col.datafield = 'FeeCode';
        col.align = 'left';
        col.id = 'gridFeeCode';
        col.styles.cell += 'width:10%';
        dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Description';
	    col.datafield = 'Description';
	    col.align = 'left';
	    col.styles.cell += 'width:20%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Minimum Number';
	    col.datafield = 'MinNum';
	    col.onkeyup = 'check_inputted';
	    col.align = 'left';
	    col.styles.cell += 'width:15%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Maximum Number';
	    col.datafield = 'MaxNum';
	    col.align = 'left';
	    col.onkeyup = 'check_inputted';
	    col.styles.cell += 'width:15%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Fee Amount';
	    col.datafield = 'FeeAmount';
	    col.align = 'left';
	    col.onkeyup = 'check_inputted';
	    col.styles.cell += 'width:15%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Currency';
	    col.datafield = 'PayoutCurr';
	    col.type = 'select';
	    col.align = 'left';
	    col.options = dt;
	    col.styles.cell += 'width:auto%';
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
   
    
    dbgrid.caption = 'Payout Partner Fee';
    dbgrid.footer.add_button.style += 'background-color: lavender;'; 
    dbgrid.footer.add_button.none = false;  
    dbgrid.footer.edit_button.none = false;            
    dbgrid.footer.edit_button.style += 'background-color: lavender;';
    dbgrid.footer.edit_button.onpost = 'disablebutton_onedit'; 
    dbgrid.footer.edit_button.beforepost = 'add_before_post';
    dbgrid.footer.edit_button.onclick = 'edit_onclick';
    dbgrid.footer.save_button.none = false;
    dbgrid.footer.save_button.onclick = 'save_data';  
    dbgrid.footer.save_button.style += 'background-color: lavender;';  
    dbgrid.footer.cancel_button.none = false;
    dbgrid.footer.cancel_button.style += 'background-color: lavender;';  
    dbgrid.footer.delete_button.none = false;
    dbgrid.footer.delete_button.beforepost = 'disablebutton_ondelete';
    dbgrid.footer.delete_button.style += 'background-color: lavender;';
    dbgrid.footer.add_button.beforepost = 'add_before_post';
    dbgrid.footer.add_button.onpost = 'disablebutton_onadd';
    
    dbgrid.caption = 'APAS Partner Fee';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;';  
}

function edit_onclick(args)
{
    isEditable = true;
    dbgrid.columns[0].editable = false;
}



function disablebutton_onadd(args)
{
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
    isEditable = false;
}

function disablebutton_ondelete(args)
{
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.add_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
    isEditable = false;
}

function disablebutton_onedit(args)
{
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.add_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
    isEditable = true;
}

function populate_rec()
{
   APASPayoutPartnerFees.ViewPartnerFee(document.getElementById('ddlPayoutPartner').value,bind_grid);   
}


function add_before_post(args)
{
    var payout_curr = dbgrid.getValueAt(args.gridrow,5);
    var min_limit = dbgrid.getValueAt(args.gridrow,2);
    var max_limit = dbgrid.getValueAt(args.gridrow,3);
    var fee_amt = dbgrid.getValueAt(args.gridrow,4);
    feecode = dbgrid.getValueAt(args.gridrow,0);

    if(document.getElementById('ddlPayoutPartner').value == 'null')
    {
        alert('Please select payout partner');
        dbgrid.curract = '';
        return;
    }
    else if(args.action == 'Edit')
    {
        for(var i=0;i<=dbgrid.datatable.Rows.length - 1;i++)
        {
            if(args.gridrow != i)
            {
                if(feecode == dbgrid.getValueAt(i,0))
                {
                    alert('Fee code already exist in the database')
                    dbgrid.curract = '';
                    return;
                }
            }
        }
    }
    
    else if(payout_curr == 'null' | payout_curr == '')
    {
        alert('Payout Currency must not be empty');
        dbgrid.curract = '';
        return;
    }
    else if(min_limit == 'null' | min_limit == '')
    {
        alert('Minimum processed transaction must not be empty');
        dbgrid.curract = '';
        return;
    }
    else if(max_limit == 'null' | max_limit == '')
    {
        alert('Maximum processed transaction must not be empty');
        dbgrid.curract = '';
        return;
    }
    else if(!isNumber(max_limit))
    {
        alert('Maximum processed transaction must be numeric');
        dbgrid.curract = '';
        return;
    }
    else if(!isNumber(min_limit))
    {
        alert('Minimum processed transaction must be numeric');
        dbgrid.curract = '';
        return;
    }
    else if(!isNumber(fee_amt))
    {
        alert('Fee amount must be in correct numeric format');
        dbgrid.curract = '';
        return;
    }
    else if(fee_amt == 'null' | fee_amt == '')
    {
        alert('Fee amount must not be empty');
        dbgrid.curract = '';
        return;
    }
    else if(parseInt(max_limit) < parseInt(min_limit))
    {
        alert('Maximum processed transaction should be greater than the minimum processed transaction');
        dbgrid.curract = '';
        return;
    }
    else if(parseInt(fee_amt) < 0)
    {
        alert('Fee amount cannot be negative');
        dbgrid.curract = '';
        return;
    }
    //.footer.delete_button.disabled = 'disabled';
}

function disable_button(btnid, enable)
{
    var fbtn = document.getElementById(btnid);
    if (fbtn != null) 
    {
        var x = (enable) ? '' : 'disabled';
        fbtn.disabled = x;
    }
}



function check_inputted(args)
{
    if(!isNumeric(document.getElementById(args.control).value))
    {
        document.getElementById(args.control).value = '';
    }
}

function page_setup()
{
    new load_dropdown().load_branches();
    getPayoutCurrencies();
    uid = APASPayoutPartnerFees.get_irsclient_data('UID').value;
    populate_rec();
}

function bind_grid(values)
{
    if (values.error != null)
    {
        spawn_errpage(values.error);
        return;
    }
    
    create_grid();
    dbgrid.bind(values.value, 'tdFeeGrid', 'grid');
} 


function getPayoutCurrencies()
{
   var rows =  APASPayoutPartnerFees.getPayoutCurrency().value.Rows;
   
   for(var i = 0;i <= rows.length - 1;i++)
   {
        dt[dt.length] = rows[i].Currency + '~' + rows[i].CurrencyDesc;
   }
}

function save_data(args)
{
    var has_pending;
    var isExisting;
    if(args.edits.length > 0)
    {
        payout_curr = args.edits.split('|')[6].split('^')[1]
    }
    else if(args.additions.length >0)
    {
        payout_curr = args.additions.split('|')[6].split('^')[1]
    }
    else if(args.deletes.length >0)
    {
        payout_curr = args.deletes.split('|')[6].split('^')[1]
        feecode = args.deletes.split('|')[1].split('^')[1]
    }
    
    
    has_pending = APASPayoutPartnerFees.has_pending(document.getElementById('ddlPayoutPartner').value,payout_curr,feecode).value;
    if(args.additions.length > 0)
    {
        isExisting = APASPayoutPartnerFees.isExisting(feecode).value;
    }
   
    
    if(isExisting)
    {
        alert('Fee code entered already exist in the database');
        populate_rec();
        return;
    }
    if(has_pending)
    {
        alert('Setup already exist in approval list');
        populate_rec();
        return;
    }
    
    

    if (args.value == dbgrid.footer.save_button.value)
    {
        var brcode = document.getElementById('ddlPayoutPartner').value;
        var deleted = (args.deletes.length > 0 ? true : false); 
        APASPayoutPartnerFees.applyEdits(args.additions, args.deletes, args.edits,brcode,uid,updates_saved);
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
    alert('Setup already queued for approval');     
    populate_rec();
}

function isNumber(sText)
{
    var s = trim(sText);
    var valid_chars = "0123456789.";

    for (var i = 0; i < s.length; i++) 
    { 
        var c = s.charAt(i); 
        if (i == 0 && (c == '+' | c == '-')) continue;
        if (valid_chars.indexOf(c) == -1) 
        {
            return false;
        }
    }
    return true;
}