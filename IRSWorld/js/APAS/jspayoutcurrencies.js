var isExisting;
var seq_id;
var uid;
var loc;
function page_setup()
{
    enabled_button('Save');
    new load_dropdown().Load_Currency();
    get_client_details();
    document.getElementById('txtAcctNo').value = '';
    new load_dropdown().load_system();
}


function enabled_button(btn_type)
{
    if (btn_type=='Save')
    {
        document.getElementById('btnSave').disabled = 'disabled';
        document.getElementById('btnEdit').disabled = '';
        document.getElementById('btnCancel').disabled = 'disabled';
        document.getElementById('txtAcctNo').disabled = 'disabled';
    }
    else if (btn_type=='Cancel')
    {
        document.getElementById('btnSave').disabled = 'disabled';
        document.getElementById('btnEdit').disabled = '';
        document.getElementById('btnCancel').disabled = 'disabled';
        document.getElementById('txtAcctNo').disabled = 'disabled';
        page_setup();
    }
    else if (btn_type=='Edit')
    {
        document.getElementById('btnSave').disabled = '';
        document.getElementById('btnEdit').disabled = 'disabled';
        document.getElementById('btnCancel').disabled = '';
        document.getElementById('txtAcctNo').disabled = '';
    }
}



function load_dropdown()
{
    this.Load_Currency= function()
    {
        APASPayoutCurrency.load_currencies('',this.load_Currency_callback);
    }
    
    this.load_Currency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlCurrency", result.value, "fldvalue", "Description");
        }
    }
    
    this.load_system = function()
    {
        var DDL = document.getElementById('ddlSystems');
        var proc = document.createElement('option');
        proc.text = 'APAS Transaction'
        proc.value = '1';
        DDL.options.add(proc);
        
        var can = document.createElement('option');
        can.text = 'PAW Transaction'
        can.value = '2';
        DDL.options.add(can);

        
    }
}

function get_data_from_server(value,moduleid)
{
    if(document.getElementById('ddlCurrency').value != 'null' & document.getElementById('ddlSystems').value != '')
    {
        APASPayoutCurrency.getCount(value,moduleid,get_data_from_server_callback);
    }
    else
    {
        document.getElementById('txtAcctNo').value = '';
    }
}

function get_data_from_server_callback(result)
{
    if (result != null && result != "undefined")
    {
        //isExisting = (result.value > 0) ? true : false;
        get_details(result.value);
    }
}

function get_details(dt)
{
    isExisting = (dt.Rows.length >0) ? true : false;
    if(dt.Rows.length >0)
    {
        document.getElementById('txtAcctNo').value = dt.Rows[0].ORDAccountNo;
        seq_id = dt.Rows[0].SEQ_ID;
    }
    else
    {
        document.getElementById('txtAcctNo').value = '';
        seq_id = 0;
    }
}

function save_payout_currencies()
{
    var may_pending_ba = APASPayoutCurrency.have_pending_for_approval(document.getElementById('ddlCurrency').value).value;
    if(may_pending_ba)
    {
        alert('This currency has pending for approval');
        return;
    }
    var currency = document.getElementById('ddlCurrency').value;
    var accountnum = document.getElementById('txtAcctNo').value;
    var is_previously_cancelled = APASPayoutCurrency.isPreviouslyCancelled(currency,accountnum).value;
    var system = document.getElementById('ddlSystems').value;
    
    
//    if(is_previously_cancelled)
//    {
//        alert('The account number inputted was previously cancelled by the approver, Please edit your account number.');
//        return;
//    }
    
    if(!APASPayoutCurrency.check_account_number(accountnum).value)
    {
        alert('Invalid account number');
        return;
    }
    
    if(!get_inputted_data(accountnum))
    {
        return;
    }
    
    if(currency == 'null' | currency == '')
    {
        alert('Please select currency');
        return;
    }
    if(accountnum == null | accountnum == '')
    {
        alert('Please input account number');
        return;
    }
    if(NonNumeric(accountnum))
    {
        alert('Account number must be numeric');
        return;
    }
    if(system == null | system == '' | system == 'null')
    {
        alert('Please select a System');
        return;
    }
    
    
    //var mode = (isExisting) ? 'UPDATE' : 'INSERT';
    var data_row = new Array();
    data_row[data_row.length] = "SEQ_ID_TEMP~"          + seq_id;     
    data_row[data_row.length] = "ORDAccountNo~"         + document.getElementById('txtAcctNo').value;
    data_row[data_row.length] = "Currency~"             + document.getElementById('ddlCurrency').value;
    data_row[data_row.length] = "AddedBy~"              + uid;
    data_row[data_row.length] = "DateAdded~"            + dateToString(APASPayoutCurrency.get_branch_localtime(loc).value);    
    data_row[data_row.length] = "isForApproval~"        + true;
    data_row[data_row.length] = "ModuleId~"             + system;
    data_row[data_row.length] = (isExisting) ? 'Action~UPDATE' : 'Action~INSERT';
    
    
    var s = data_row.join('|');
    APASPayoutCurrency.do_process_apas('INSERT',s,seq_id,this.save_payout_currencies_callback);
}

function save_payout_currencies_callback(result)
{
    if(result != null && result != 'undefined')
    {
        alert('Payout currency successfully saved');
        enabled_button('Save');
    }
}

function get_client_details()
{
    uid = APASPayoutCurrency.get_irsclient_data('UID').value;
    loc = APASPayoutCurrency.get_irsclient_data('LOCATION').value;
}

function get_inputted_data(str)
{
    if(isSpecial(str))
    {
        alert('Special character(s) not allowed');
        document.getElementById('txtAcctNo').value='';
        return false;
    }
    else if(!valid_char(str))
    {
        return false;
    }
    return true;
}

function valid_char(str)
{
    if(isNumeric(str))
    {
        return true;
    }
    alert('Please input numeric only');
    return false;
    
}


function NonNumeric(s)
{
    var pat = /[`~!@#$%^&*\(\)-+|\\\{\}\[\]:;"'<>?,.\/]/g;
    rs = s.match(pat);
    return (rs != null);
}

//this.locked_unlocked_tranx = function(instruction,bool)
//{
//    if(cls.SEQ_ID != null)
//    {
//        var data_row = new Array();
//        data_row[data_row.length] = "isLocked~"               + bool; 
//        data_row[data_row.length] = (bool == false) ? "LockedBy~"               +  '' : "LockedBy~"               + this.UserName;
//        data_row[data_row.length] = (bool == false) ? "LockedDate~"             +  '' : "LockedDate~"             + dateToString(APASMain.get_branch_localtime(this.BranchCode).value);
//        var s = data_row.join('|');
//        APASMain.do_process_apas(instruction,s,cls.SEQ_ID,this.locked_unlocked_tranx_callback); 
//    }
//}

