var dbgrid; 
var seq_id;

var CIFID;
var BranchCode;
var CIFClass;
var Role;
var uid;
var filter;
var processed_tranx = "2";


function page_setup()
{
    get_client_data_details();
    load_dropdown(CIFClass);
    loadfilter();
    
    show_hide_form('gridDetails','block');
    show_hide_form('frmDetails','none');
    populate_rec(false);
}

function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'Implementation Date';
        col.datafield = 'Implem_Date';
        col.align = 'right';
        col.styles.cell += 'width:10%';
        dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'RefNo (Theirs)';
	    col.datafield = 'RefNoTheirs';
	    col.align = 'left';
	    col.styles.cell += 'width:10%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'RefNo (Ours)';
	    col.datafield = 'RefNoTheirs';
	    col.align = 'left';
	    col.onclick  = 'get_data';
	    col.styles.cell += 'width:10%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'Beneficiary Fullname';
	    col.datafield = 'BeneFull_Name';
	    col.align = 'left';
	    col.styles.cell += 'width:20%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'CurrencyCode';
	    col.datafield = 'Currency';
	    col.align = 'left';
	    col.styles.cell += 'width:5%';
	    dbgrid.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'AmountPayable';
	    col.datafield = 'AmountPayable';
	    col.align = 'left';
	    col.styles.cell += 'width:10%';
	    dbgrid.addColumn(col);
    }
//    {
//	    var col = new datagrid_column();
//	    col.header = 'DatePaid';
//	    col.datafield = 'DatePaid';
//	    col.align = 'left';
//	    col.styles.cell += 'width:10%';
//	    dbgrid.addColumn(col);
//    }
//    {
//	    var col = new datagrid_column();
//	    col.header = 'PayingBranch';
//	    col.datafield = 'PayingBranch';
//	    col.align = 'left';
//	    col.styles.cell += 'width:15%';
//	    dbgrid.addColumn(col);
//    }

    {
	    var col = new datagrid_column();
	    col.header = '';
	    col.align = 'center';
	    col.type = 'button';
	    col.value = 'View Details';
	    col.onclick = 'get_data';
	    col.styles.cell += 'width:10%';
	    col.disabled = false;
	    col.styles.datafield += 'width:100%;';
	    dbgrid.addColumn(col);
    }
    
    dbgrid.caption = 'Transactions';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;';  
    dbgrid.row.onclick = 'get_data';
}

function bind_grid(currgrp)
{
    if (currgrp.error != null)
    {
        spawn_errpage(currgrp.error);
        return;
    }
    
    create_grid();
    dbgrid.bind(currgrp.value, 'trnxStatus', 'gridProcessedTrnx');
    ajax_show_status(false);
} 

function populate_rec(from_button)
{
    var stat = document.getElementById('ddlTranxStat').value;
    if(from_button)
    {
        var  val = document.getElementById('txtFilter').value;
        var filt = document.getElementById('ddlFilter').value;
        if(trim(filt) == 'null' | trim(filt) == '')
        {
            alert('Please enter search criteria.');
            return;
        }
        else if(trim(val) == null | trim(val) == '')
        {
            alert('Please enter a value for the search criteria selected');
            return;
        }
        else if(CIFClass == null | CIFClass == '')
        {
            CIFClass = 0;
        }
    }
    
    
    show_hide_form('gridDetails','none');
    ajax_show_status(true);

    
    APASTransactionStatus.getAPASbyTransaction(stat,BranchCode,CIFClass,getFilter_values(),bind_grid);
    show_hide_form('gridDetails','block');
    show_hide_form('frmDetails','none');
    if(stat == processed_tranx) 
    { 
        document.getElementById('btnReprint').style.display = 'block';  
    }
    else
    {
        document.getElementById('btnReprint').style.display = 'none';  
    }

}

function get_data(args)
{
    var s = args.value.split('_');
    var refno = dbgrid.getValueAt(s[2],1);
    var stat = document.getElementById('ddlTranxStat').value;
    ajax_show_status(true);
    APASTransactionStatus.getAPASInfo(refno,stat,get_data_callback);
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
    ajax_show_status(false);
}


function set_details(dataset)
{   
    if (dataset != null && typeof(dataset) == "object" && dataset.Tables != null)
    {   
        if(document.getElementById('ddlTranxStat').value == 2)
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
            document.getElementById('lbl1').innerText               = dataset.Tables[0].Rows[0]['ProcessedBy'];
            document.getElementById('lblDate').innerText            = dateToString(dataset.Tables[0].Rows[0]['DateProcessed']);
            document.getElementById('lbl200').innerText             = dataset.Tables[0].Rows[0]['ApprovedBy'];
            document.getElementById('lblPaid').innerText            = dateToString(dataset.Tables[0].Rows[0]['DatePaid']);
            
            document.getElementById('tdBy').innerText               = 'ProcessedBy'
            document.getElementById('tdDate').innerText             = 'DateProcessed';
            document.getElementById('lblPaid').style.display        = 'block';
            document.getElementById('tdDatePaid').style.display     = 'block';
            document.getElementById('td200').innerText              = 'Approved By'
            document.getElementById('lbl200').style.display         = 'block';
            document.getElementById('td200').style.display          = 'block';
            document.getElementById('tdDetails').style.display      = 'block';
            document.getElementById('tdBy').style.display           = 'block';
            document.getElementById('tdDate').style.display         = 'block';
            //document.getElementById('btnUnlock').style.display      = 'none';
            
        }
        else if(document.getElementById('ddlTranxStat').value == 5)
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
            
            
            document.getElementById('tdBy').innerText               = 'LockedBy'
            document.getElementById('tdDate').innerText             = 'LockedDate';
            
            document.getElementById('lblPaid').style.display        = 'none';
            document.getElementById('tdDatePaid').style.display     = 'none';
            document.getElementById('lbl200').style.display         = 'none';
            document.getElementById('td200').style.display          = 'none';
            document.getElementById('tdBy').style.display           = 'block';
            document.getElementById('tdDate').style.display         = 'block';
        }
        else if(document.getElementById('ddlTranxStat').value == 4)
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
            document.getElementById('lbl1').innerText               = dataset.Tables[0].Rows[0]['CancelledBy'];
            document.getElementById('lblDate').innerText            = dateToString(dataset.Tables[0].Rows[0]['DateCancelled']);
            document.getElementById('lbl200').innerText             = dataset.Tables[0].Rows[0]['CancelReason'];
            
            document.getElementById('tdBy').innerText               = 'CancelledBy'
            document.getElementById('tdDate').innerText             = 'DateCancelled';
            document.getElementById('td200').innerText              = 'Cancel Reason'
            
            document.getElementById('lblPaid').style.display        = 'none';
            document.getElementById('tdDatePaid').style.display     = 'none';
            document.getElementById('lbl200').style.display         = 'block';
            document.getElementById('td200').style.display          = 'block';
            document.getElementById('tdBy').style.display           = 'block';
            document.getElementById('tdDate').style.display         = 'block';
        }
        else if(document.getElementById('ddlTranxStat').value == 1)
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
            

            document.getElementById('td200').innerText              = '';
            document.getElementById('lbl1').innerText               = '';
            document.getElementById('lblDate').innerText            = '';
            document.getElementById('tdDetails').style.display      = 'none';
            document.getElementById('lblPaid').style.display        = 'none';
            document.getElementById('tdDatePaid').style.display     = 'none';
            document.getElementById('lbl200').style.display         = 'none';
            document.getElementById('td200').style.display          = 'none';
            document.getElementById('lblPaid').style.display        = 'none';
            document.getElementById('tdDatePaid').style.display     = 'none';
            document.getElementById('tdBy').style.display           = 'none'
            document.getElementById('tdDate').style.display         = 'none';
        }
        
        else if(document.getElementById('ddlTranxStat').value == 3)
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
            

            document.getElementById('td200').innerText              = '';
            document.getElementById('lbl1').innerText               = '';
            document.getElementById('lblDate').innerText            = '';
            document.getElementById('tdDetails').style.display      = 'none';
            document.getElementById('lblPaid').style.display        = 'none';
            document.getElementById('tdDatePaid').style.display     = 'none';
            document.getElementById('lbl200').style.display         = 'none';
            document.getElementById('td200').style.display          = 'none';
            document.getElementById('lblPaid').style.display        = 'none';
            document.getElementById('tdDatePaid').style.display     = 'none';
            document.getElementById('tdBy').style.display           = 'none'
            document.getElementById('tdDate').style.display         = 'none';
        }
        
    }
}


function show_hide_form(tblname,hidden)
{
    var tbl = document.getElementById(tblname);
    tbl.style.display = hidden;
    
}

function load_dropdown(cif_class)
{

    if(cif_class == APASTransactionStatus.isAgent().value) //An Agent
    {
        var DDL = document.getElementById('ddlTranxStat');
        var proc = document.createElement('option');
        proc.text = 'Processed Transaction'
        proc.value = '2';
        DDL.options.add(proc);
        
        var can = document.createElement('option');
        can.text = 'Locked Transaction'
        can.value = '5';
        DDL.options.add(can);
    }
    else if(cif_class != APASTransactionStatus.isAgent().value) //PNB Teller Maker
    {
        var DDL = document.getElementById('ddlTranxStat');
        
        var out = document.createElement('option');
        out.text = 'Outstanding Transaction'
        out.value = '1';
        DDL.options.add(out);
        
        var proc = document.createElement('option');
        proc.text = 'Processed Transaction'
        proc.value = '2';
        DDL.options.add(proc);
        
        var disp = document.createElement('option');
        disp.text = 'Transaction for Disposition'
        disp.value = '3';
        DDL.options.add(disp);
        
        var can = document.createElement('option');
        can.text = 'Cancelled Transaction'
        can.value = '4';
        DDL.options.add(can);
        
        var lok = document.createElement('option');
        lok.text = 'Locked Transaction'
        lok.value = '5';
        DDL.options.add(lok);
    }
//    else if(cif_class = 8) //PNB Supervisor
//    {
//        var DDL = document.getElementById('ddlTranxStat');
//        
//        var out = document.createElement('option');
//        out.text = 'Outstanding Transaction'
//        out.value = '1';
//        DDL.options.add(out);
//        
//        var proc = document.createElement('option');
//        proc.text = 'Processed Transaction'
//        proc.value = '2';
//        DDL.options.add(proc);
//        
//        var disp = document.createElement('option');
//        disp.text = 'Transaction for Disposition'
//        disp.value = '3';
//        DDL.options.add(disp);
//        
//        var can = document.createElement('option');
//        can.text = 'Cancelled Transaction'
//        can.value = '4';
//        DDL.options.add(can);
//        
//        var lok = document.createElement('option');
//        lok.text = 'Locked Transaction'
//        lok.value = '5';
//        DDL.options.add(lok);
//    }
}

function get_client_data_details()
{
    CIFID = APASTransactionStatus.get_irsclient_data('CIF').value;
    BranchCode = APASTransactionStatus.get_irsclient_data('LOCATION').value;
    CIFClass = APASTransactionStatus.CIFClass(CIFID).value;
    Role = APASTransactionStatus.get_irsclient_data('RoleID').value;
    uid = APASTransactionStatus.get_irsclient_data('UID').value;
}


function ShowReport()
{
   var refno = document.getElementById('RefNoTheirs').innerText;
   APASTransactionStatus.ShowReport(refno,ShowReport_callback);        
}

function ShowReport_callback(result)
{
   window.open('../Reports/Pages/Report_viewer.aspx',1);           
}

function loadfilter()
{
    APASTransactionStatus.getFilter(loadfilter_callback);
}

function loadfilter_callback(result)
{
   this.commons = null;
   if (result != null && result != "undefined")
   {
        this.commons = new common_method();
        this.commons.to_load_dropdownlist("ddlFilter", result.value, "FldValue", "FldDescription");
   }
   
}

function getFilter_values()
{
    var strQuery = document.getElementById('txtFilter').value;
    var filter_indx = document.getElementById('ddlFilter').value;
    switch(filter_indx)
    {
        case "1": //Amount
            return " AND Amount=" + strQuery;
        case "2": //Bene Name
            return " AND BeneFull_Name like '" + strQuery + "%'";
        case "3":
            return " AND RefNoTheirs = '" + strQuery + "'";
        case "4":
            return " AND RefNoOurs = '" + strQuery + "'";
    }    
    return '';
   
}