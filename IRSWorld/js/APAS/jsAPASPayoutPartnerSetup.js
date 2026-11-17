var dbgrid;
var dt = new Array();
var popup;
var uid;

function load_dropdown()
{
    this.HeadOffice=document.getElementById('chkMother').checked;
    
    this.load_branches = function()
    {
        APASPayoutPartnerSetup.load_payout_partners(this.HeadOffice,this.load_branches_callback);
    }
    
    this.load_branches_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlBranches", result.value, "BranchCode", "BranchName");
        }
    }
    
    
    this.load_sub_branches = function()
    {
        APASPayoutPartnerSetup.load_sub_branch(document.getElementById('ddlBranches').value,this.load_sub_branches_callback);
    }
    
    this.load_sub_branches_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlSubBranch", result.value, "BranchCode", "BranchName");
        }
    }
    
    
    
}


function page_setup()
{
    populate_rec();
    getPayoutCurrencies();
    get_client_details();
    
    new load_dropdown().load_branches();
    if (hasQueryStrings(window))
    {
        var fun = getQueryString(window, 'function');
        popup = (fun == 'popup');
    }
}

function create_grid()
{
     dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'Payout Currency';
        col.datafield = 'Payout_Curr';
        col.align = 'center';
        col.type = 'select';
        col.options = dt;
        col.styles.cell += 'width:30%';
        dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Transaction Limit';
	    col.datafield = 'TransactionLimit';
	    col.align = 'left';
	    col.styles.cell += 'width:20%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Account Number';
	    col.datafield = 'AcctNum';
	    col.align = 'left';
	    col.styles.cell += 'width:20%';
	    dbgrid.addColumn(col);
    }
    
    /*
    {
	    var col = new datagrid_column();
	    col.header = 'Path';
	    col.datafield = 'Logo';
	    col.align = 'left';
	    col.styles.cell += 'width:30%';
	    col.editable = false;
	    dbgrid.addColumn(col);
    }
    if (! popup)
    {
        var col = new datagrid_column();
        col.header = '';
        col.datafield = '';
        col.align = 'center';
        col.type = 'button';
        col.value = String.fromCharCode(187);
        //col.edit_button = true;
        col.onclick = 'grid_btn_browse';
        col.styles.cell += 'width:5%';
        col.styles.datafield += 'width:100%;';
        dbgrid.addColumn(col);
    }
    */
    
    {
	    var col = new datagrid_column();
	    col.header = 'OnBehalf';
	    col.datafield = 'OnBehalf';
	    col.align = 'left';
	    col.type = 'check';
	    col.styles.cell += 'width:10%';
	    dbgrid.addColumn(col);
    }
    
    //
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


    dbgrid.caption = 'Payout Partner Setup';
    dbgrid.footer.add_button.style += 'background-color: lavender;'; 
    dbgrid.footer.add_button.none = false;  
    dbgrid.footer.edit_button.none = false;            
    dbgrid.footer.edit_button.style += 'background-color: lavender;'; 
    dbgrid.footer.save_button.none = false;
    dbgrid.footer.save_button.onclick = 'save_data';  
    dbgrid.footer.save_button.style += 'background-color: lavender;';  
    dbgrid.footer.cancel_button.none = false;
    dbgrid.footer.cancel_button.style += 'background-color: lavender;';  
    dbgrid.footer.delete_button.none = false;
    dbgrid.footer.delete_button.style += 'background-color: lavender;';
    dbgrid.footer.add_button.onpost = 'disablebutton_onadd';
    dbgrid.footer.delete_button.beforepost = 'disablebutton_ondelete';
    dbgrid.footer.edit_button.onpost = 'disablebutton_onedit';
    
    
    dbgrid.footer.add_button.beforepost = 'add_before_post';
    dbgrid.caption = 'Payout Partner Setup';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;';  
    
}


function add_before_post(args)
{
    var payout_curr = dbgrid.getValueAt(args.gridrow,0);
    var trxn_limit = dbgrid.getValueAt(args.gridrow,1)
    var acct_num = dbgrid.getValueAt(args.gridrow,2)
    
    if(payout_curr == 'null' | payout_curr == '')
    {
        alert('Payout Currency must not be empty');
        dbgrid.curract = '';
        return
    }
    else if(!isNumber(trxn_limit))
    {
        alert('Transaction limit must be numeric.');
        dbgrid.curract = '';
        return;
    }
    else if(!isNumeric(trxn_limit))
    {
        alert('Transaction limit must be numeric.');
        dbgrid.curract = '';
        return;
    }
    else if(trxn_limit == 'null' | trxn_limit == '')
    {
        alert('Transaction Limit must not be empty');
        dbgrid.curract = '';
        return;
    }
    else if(!isNumber(acct_num))    
    {
        alert('Account number must be in numeric format');
        dbgrid.curract = '';
        return;
    }
    else if(acct_num == 'null' | acct_num == '')
    {
        alert('Account number must not be empty');
        dbgrid.curract = '';
        return;
    }
    else if(document.getElementById('ddlBranches').value == 'null' | document.getElementById('ddlBranches').value == '')
    {
        alert('Please select a Payout Partner');
        dbgrid.curract = '';
        return;
    }
    else if(!APASPayoutPartnerSetup.check_account_number(acct_num).value)
    {
        alert('Account number invalid.\n Please check the account number inputted');
        dbgrid.curract = '';
        return;
    }
}


function disablebutton_onadd(args)
{
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
}

function disablebutton_ondelete(args)
{
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.add_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
}

function disablebutton_onedit(args)
{
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.delete_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.add_button.value, false);
    disable_button(dbgrid.id + '_footerbtn_' + dbgrid.footer.edit_button.value, false);
}

function populate_rec()
{
    getPayoutCurrencies();
    var ischeck = document.getElementById('chkMother').checked;
    var brcode;
    if(ischeck)
    {
        brcode = document.getElementById('ddlBranches').value;
    }
    else
    {
        brcode = document.getElementById('ddlSubBranch').value;
    }
    APASPayoutPartnerSetup.viewSetup(brcode,bind_grid);   
}

function bind_grid(values)
{
    if (values.error != null)
    {
        spawn_errpage(values.error);
        return;
    }
    
    create_grid();
    dbgrid.bind(values.value, 'tdGrid', 'grid');
} 

function getPayoutCurrencies()
{
   var rows =  APASPayoutPartnerSetup.getPayoutCurrency(document.getElementById('ddlBranches').value).value.Rows;
   dt = new Array();
   for(var i = 0;i <= rows.length - 1;i++)
   {
        dt[dt.length] = rows[i].Currency + '~' + rows[i].CurrencyDesc;
   }
}



var save_row = -1;    
function grid_btn_browse(args)
{
    var div = document.getElementById('divExplore');
    if (save_row == args.gridrow)
    {
        if (div.style.display == 'block') div.style.display = 'none';
        save_row = -1;
        return;
    }
    save_row = args.gridrow;
    var btn = document.getElementById(args.control).parentElement;
    var diff = 85 //(args.gridrow < 5) ? +220 : -20;
    div.style.top = btn.offsetTop + diff;
    div.style.left = btn.offsetLeft - (-245);
    div.style.display = 'block';
}


function fselect()
{
    var sn = d3.aNodes[d3.selectedNode];
    var npath = '/' + sn._p.name + '/' + sn.name;
    
    dbgrid.setColumn(3, npath);
    
    close_explore();
} 

function close_explore()
{
    var div = document.getElementById('divExplore');
    div.style.display = 'none';
    save_row = -1;
}

function save_data(args)
{
    var has_pending;
    var branch='';
    
    if(document.getElementById('chkMother').checked)
    {
        branch = document.getElementById('ddlBranches').value;
    }
    else
    {
        branch = document.getElementById('ddlSubBranch').value;
    }
    
    
    if(args.edits.length > 0)
    {
        payout_curr = args.edits.split('|')[1].split('^')[1];
    }
    else if(args.additions.length >0)
    {
        payout_curr = args.additions.split('|')[1].split('^')[1];
    }
    else if(args.deletes.length >0)
    {
        payout_curr = args.deletes.split('|')[1].split('^')[1];
    }
    
    has_pending = APASPayoutPartnerSetup.has_pending(branch,payout_curr).value;
    
    
    if(has_pending)
    {
        alert('Setup already exist in approval list');
        populate_rec();
        return;
    }
    
   

    if (args.value == dbgrid.footer.save_button.value)
    {
    
        var ischeck = document.getElementById('chkMother').checked;
        var brcode;
        if(ischeck)
        {
            brcode = document.getElementById('ddlBranches').value;
        }
        else
        {
            brcode = document.getElementById('ddlSubBranch').value;
        }
        
        var deleted = (args.deletes.length > 0 ? true : false); 
        APASPayoutPartnerSetup.applyEdits(args.additions, args.deletes, args.edits,brcode,deleted,uid,document.getElementById('txtLogoPath').value,updates_saved);
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
    document.getElementById('txtLogoPath').value = '';    
    populate_rec();
}



function get_client_details()
{
    uid = APASPayoutPartnerSetup.get_irsclient_data('UID').value;
}


function isNumber(s)
{
    var pat = /[`~!@#$%^&*\(\)|\\\{\}\[\]:;"'<>?,\/]/g;
    rs = s.match(pat);
    return (rs == null);
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


function hide_sub_branch(bool)
{
    if(bool)
    {
        document.getElementById('tdSubBranch').style.display = 'none'
        document.getElementById('tdSubBranchDesc').style.display = 'none'
    }
    else
    {
        document.getElementById('tdSubBranch').style.display = 'block'
        document.getElementById('tdSubBranchDesc').style.display = 'block'
    }
}


//function save_valid_id(args)
//    {
//        if (args.value == dbgrid.footer.save_button.value)
//        {        
//            var brcode = document.getElementById('ddlBranch').value;
//            var deleted = (args.deletes.length > 0 ? true : false);                
//            Valid_IDs.applyEdits(args.additions, args.deletes, args.edits,brcode,deleted,updates_saved);
//          
//            //alert('Record successfully saved');
//        }
//    }
//function getPayoutCurrencies_callback(result)
//{
//    if(result != null && result != 'undefined')
//    {
//        payout_currencies = result.value.Rows;
//    }
//}

