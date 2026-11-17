var dbgrid;
var seq_id;
var branchcode;
var uid;
var status;
function page_setup()
{
    get_client_data_details();
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
    {
	    var col = new datagrid_column();
	    col.header = 'Status';
	    col.datafield = 'Status';
	    col.align = 'left';
	    col.styles.cell += 'width:10%';
	    dbgrid.addColumn(col);
    }
    
    
    dbgrid.caption = 'Transaction for Approval';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 12;
    dbgrid.row.normal_style = 'background-color: #f7ffff;';  
    dbgrid.row.onclick = 'get_data';
}


function populate_rec()
{
    APASAmendments.ViewAmendments('',bind_grid);
}

function bind_grid(currgrp)
{
    if (currgrp.error != null)
    {
        spawn_errpage(currgrp.error);
        return;
    }
    
    create_grid();
    dbgrid.bind(currgrp.value, 'tdAmendments', 'grAmendment');
} 

function bind_grid(currgrp)
{
    if (currgrp.error != null)
    {
        spawn_errpage(currgrp.error);
        return;
    }
    
    create_grid();
    dbgrid.bind(currgrp.value, 'tdAmendments', 'gridProcessedTrnx');
} 

function amendments()
{   
    this.trxn_stat          = null;
    this.SEQ_ID             = null;

    this.set_details = function(dataset)
    {
        if (dataset != null && typeof(dataset) == "object" && dataset.Tables != null)
        {   
            if(status == 'AMENDED')
            {
                document.getElementById('ImplemDate').innerHTML         = dateToString(dataset.Tables[0].Rows[0]['TranxDate']) == null ? '' : dateToString(dataset.Tables[0].Rows[0]['TranxDate']);
                document.getElementById('RefNoTheirs').innerHTML        = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo'];
                document.getElementById('RefNoOurs').innerHTML          = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo']; 
                document.getElementById('ORC').innerHTML                = 'ORC';
                document.getElementById('BenefLastNm').innerHTML        = dataset.Tables[0].Rows[0]['LastNm'];
                document.getElementById('BenefFirstNm').innerHTML       = dataset.Tables[0].Rows[0]['FirstNm'];
                document.getElementById('BenefMiddleNm').innerHTML      = dataset.Tables[0].Rows[0]['MiddleNm'];
                document.getElementById('BeneAdd').innerHTML            = dataset.Tables[0].Rows[0]['Bene_Address'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_Address'];
                document.getElementById('BeneContactNo').innerHTML      = dataset.Tables[0].Rows[0]['Bene_ContactNum'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_ContactNum'];
                document.getElementById('SpecialMSG').innerHTML         = dataset.Tables[0].Rows[0]['SpecialMessage'] == null ? '' : dataset.Tables[0].Rows[0]['SpecialMessage'];
                document.getElementById('Purpose').innerHTML            = dataset.Tables[0].Rows[0]['Purpose'] == null ? '' : dataset.Tables[0].Rows[0]['Purpose'];
                document.getElementById('RemID').innerHTML              = dataset.Tables[0].Rows[0]['RemitterID'];
                document.getElementById('RemName').innerHTML            = dataset.Tables[0].Rows[0]['RemFull_Name'];
                document.getElementById('RemAdd').innerHTML             = dataset.Tables[0].Rows[0]['Rem_Adress'] == null ? '' : dataset.Tables[0].Rows[0]['Rem_Adress'];
                document.getElementById('RemBank').innerHTML            = dataset.Tables[0].Rows[0]['RemittingBank'] == null ? '' : dataset.Tables[0].Rows[0]['RemittingBank'];
                document.getElementById('ProdCode').innerHTML           = dataset.Tables[0].Rows[0]['ProductName'] == null ? '' : dataset.Tables[0].Rows[0]['ProductName'];
                document.getElementById('CurrencyCode').innerHTML       = dataset.Tables[0].Rows[0]['Currency'] == null ? '' : dataset.Tables[0].Rows[0]['Currency'];
                document.getElementById('RateUsed').innerHTML           = formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3);
                document.getElementById('Amount').innerHTML             = formatNumber(dataset.Tables[0].Rows[0]['Amount'],2) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['Amount'],2);            
                document.getElementById('EVAA').innerHTML               = 'EVAA';
                document.getElementById('NSVS').innerHTML               = 'NSVS';
                document.getElementById('trxn_stat').innerHTML          = dataset.Tables[0].Rows[0]['TransactionStatus'];
                seq_id                                                  = dataset.Tables[0].Rows[0]['SEQ_id'];            
                this.trxn_stat                                          = dataset.Tables[0].Rows[0]['TransactionStatusCode'] == null ? '' : dataset.Tables[0].Rows[0]['TransactionStatusCode'];    
                
                document.getElementById('ImplemDate2').innerHTML         = dateToString(dataset.Tables[0].Rows[0]['TranxDate']) == null ? '' : dateToString(dataset.Tables[0].Rows[0]['TranxDate']);
                document.getElementById('RefNoTheirs2').innerHTML        = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo'];
                document.getElementById('RefNoOurs2').innerHTML          = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo']; 
                document.getElementById('ORC2').innerHTML                = 'ORC';
                //document.getElementById('BeneName2').innerHTML           = dataset.Tables[0].Rows[0]['BenefName'];
                document.getElementById('BenefLastNm2').innerHTML        = dataset.Tables[0].Rows[0]['aLastName'];
                document.getElementById('BenefFirstNm2').innerHTML       = dataset.Tables[0].Rows[0]['aFirstName'];
                document.getElementById('BenefMiddleNm2').innerHTML      = dataset.Tables[0].Rows[0]['aMiddleName'];
                document.getElementById('BeneAdd2').innerHTML            = dataset.Tables[0].Rows[0]['BenefAddress'] == null ? '' : dataset.Tables[0].Rows[0]['BenefAddress'];
                document.getElementById('BeneContactNo2').innerHTML      = dataset.Tables[0].Rows[0]['Bene_ContactNum'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_ContactNum'];
                document.getElementById('SpecialMSG2').innerHTML         = dataset.Tables[0].Rows[0]['SpecialMessage'] == null ? '' : dataset.Tables[0].Rows[0]['SpecialMessage'];
                document.getElementById('Purpose2').innerHTML            = dataset.Tables[0].Rows[0]['Purpose'] == null ? '' : dataset.Tables[0].Rows[0]['Purpose'];
                document.getElementById('RemID2').innerHTML              = dataset.Tables[0].Rows[0]['RemitterID'];
                document.getElementById('RemName2').innerHTML            = dataset.Tables[0].Rows[0]['RemFull_Name'];
                document.getElementById('RemAdd2').innerHTML             = dataset.Tables[0].Rows[0]['Rem_Adress'] == null ? '' : dataset.Tables[0].Rows[0]['Rem_Adress'];
                document.getElementById('RemBank2').innerHTML            = dataset.Tables[0].Rows[0]['RemittingBank'] == null ? '' : dataset.Tables[0].Rows[0]['RemittingBank'];
                document.getElementById('ProdCode2').innerHTML           = dataset.Tables[0].Rows[0]['ProductName'] == null ? '' : dataset.Tables[0].Rows[0]['ProductName'];
                document.getElementById('CurrencyCode2').innerHTML       = dataset.Tables[0].Rows[0]['Currency'] == null ? '' : dataset.Tables[0].Rows[0]['Currency'];
                document.getElementById('RateUsed2').innerHTML           = formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3);
                document.getElementById('Amount2').innerHTML             = formatNumber(dataset.Tables[0].Rows[0]['Amount'],2) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['Amount'],2);            
                document.getElementById('EVAA2').innerHTML               = 'EVAA';
                document.getElementById('NSVS2').innerHTML               = 'NSVS';
                document.getElementById('trxn_stat2').innerHTML          = dataset.Tables[0].Rows[0]['TransactionStatus'];
                document.getElementById('lblDateAmended').innerHTML      = dataset.Tables[0].Rows[0]['DateAmended'] == null ? '' : dateToString(dataset.Tables[0].Rows[0]['DateAmended']);
                document.getElementById('lblAmendedBy2').innerHTML       = dataset.Tables[0].Rows[0]['AmendedBy'];
            }
            else if(status == 'CANCEL')
            {
                document.getElementById('CImplemDate').innerHTML            = dateToString(dataset.Tables[0].Rows[0]['TranxDate']) == null ? '' : dateToString(dataset.Tables[0].Rows[0]['TranxDate']);
                document.getElementById('CRefNoTheirs').innerHTML           = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo'];
                document.getElementById('CRefNoOurs').innerHTML             = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo']; 
                document.getElementById('CORC').innerHTML                   = 'ORC';
                document.getElementById('CBenefName').innerHTML             = dataset.Tables[0].Rows[0]['BeneFull_Name'];
                document.getElementById('CBenefAddress').innerHTML          = dataset.Tables[0].Rows[0]['Bene_Address'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_Address'];
                document.getElementById('CBenefContactNo').innerHTML        = dataset.Tables[0].Rows[0]['Bene_ContactNum'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_ContactNum'];
                document.getElementById('CSpecialMsg').innerHTML            = dataset.Tables[0].Rows[0]['SpecialMessage'] == null ? '' : dataset.Tables[0].Rows[0]['SpecialMessage'];
                document.getElementById('CPurpose').innerHTML               = dataset.Tables[0].Rows[0]['Purpose'] == null ? '' : dataset.Tables[0].Rows[0]['Purpose'];
                document.getElementById('CRemID').innerHTML                 = dataset.Tables[0].Rows[0]['RemitterID'];
                document.getElementById('CRemName').innerHTML               = dataset.Tables[0].Rows[0]['RemFull_Name'];
                document.getElementById('CRemAddress').innerHTML            = dataset.Tables[0].Rows[0]['Rem_Adress'] == null ? '' : dataset.Tables[0].Rows[0]['Rem_Adress'];
                document.getElementById('CRemBank').innerHTML               = dataset.Tables[0].Rows[0]['RemittingBank'] == null ? '' : dataset.Tables[0].Rows[0]['RemittingBank'];
                document.getElementById('CProductCode').innerHTML           = dataset.Tables[0].Rows[0]['ProductName'] == null ? '' : dataset.Tables[0].Rows[0]['ProductName'];
                document.getElementById('CCurrCode').innerHTML              = dataset.Tables[0].Rows[0]['Currency'] == null ? '' : dataset.Tables[0].Rows[0]['Currency'];
                document.getElementById('CRateUsed').innerHTML              = formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3);
                document.getElementById('CAmountPayable').innerHTML         = formatNumber(dataset.Tables[0].Rows[0]['Amount'],2) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['Amount'],2);            
                document.getElementById('CEVAA').innerHTML                  = 'EVAA';
                document.getElementById('CNSVS').innerHTML                  = 'NSVS';
                document.getElementById('C').innerHTML                      = dataset.Tables[0].Rows[0]['TransactionStatus'];
                seq_id                                                      = dataset.Tables[0].Rows[0]['SEQ_id'];            
                this.trxn_stat                                              = dataset.Tables[0].Rows[0]['TransactionStatusCode'] == null ? '' : dataset.Tables[0].Rows[0]['TransactionStatusCode'];    
                document.getElementById('CCancelBy').innerHTML              = dataset.Tables[0].Rows[0]['CancelledBy'] == null ? '' : dataset.Tables[0].Rows[0]['CancelledBy'];    
                document.getElementById('CDateCancelled').innerHTML         = dateToString(dataset.Tables[0].Rows[0]['DateCancelled']) == null ? '' : dateToString(dataset.Tables[0].Rows[0]['DateCancelled']);    
                document.getElementById('CReasonForCancellation').innerHTML = dataset.Tables[0].Rows[0]['CancelReason'] == null ? '' : dataset.Tables[0].Rows[0]['CancelReason'];    
            }
        }
    }
    
    
}

function get_data(args)
{
    var s = args.value.split('_');
    var refno = dbgrid.getValueAt(s[2],1);
    status = dbgrid.getValueAt(s[2],6);
    APASAmendments.getFullInfo(refno,get_data_callback);
}

function get_data_callback(data)
{
    if(data.error != null)
    {
        spawn_errpage(currgrp.error);
        return;
    }

    new amendments().set_details(data.value);
    
    if(status == 'AMENDED')
    {
        show_hide_form('div_results','block');
        show_hide_form('tblSearch','none');
        show_hide_form('tblMainTable','block');
        show_hide_form('tblAmend','block');
        show_hide_form('tblCancel','none');
        show_hide_form('Amendments','block'); //button
        show_hide_form('Cancellation','none');
        show_hide_form('tblRejection','block');
    }
    else if(status == 'CANCEL')
    {
        show_hide_form('div_results','block');
        show_hide_form('tblSearch','none');
        show_hide_form('tblCancel','block');
        show_hide_form('Cancellation','block'); // button
        show_hide_form('tblMainTable','none');
        show_hide_form('tblAmend','none');
        show_hide_form('Amendments','none');
        show_hide_form('tblRejection','block');
    }
}

function show_hide_form(tblname,hidden)
{
    var tbl = document.getElementById(tblname);
    tbl.style.display = hidden;
    
}

function approve_cancel_amendments(stat)
{
    var data_row = new Array();
    var msg = (stat == 'Approved') ?  'Do you want to approve amendment(s) of this transaction' : 'Do you want to disapprove amendment(s) of this transaction';
    if(confirm(msg))
    {
        if(stat == 'Approved')
        {
            data_row[data_row.length] = "ApprovedAmendments~"               + true; 
            data_row[data_row.length] = "DateAmended~"    + dateToString(APASAmendments.get_branch_localtime(branchcode).value); 
            data_row[data_row.length] = "AmendedBy~"               + true; 
            data_row[data_row.length] = "WithAmendments~"               + false; 
            var s = data_row.join('|');
        }
        else if(stat == 'Cancelled')
        {
            if(document.getElementById('txtReason').value.length > 0)
            {
                data_row[data_row.length] = "CancelledAmendments~"               + true; 
                data_row[data_row.length] = "DateAmended~"    + dateToString(APASAmendments.get_branch_localtime(branchcode).value); 
                data_row[data_row.length] = "AmendedBy~"               + uid; 
                data_row[data_row.length] = "WithAmendments~"          + false; 
                data_row[data_row.length] = "ApprovedAmendments~"      + false;
                var s = data_row.join('|');
            }
            else
            {
                alert('Please input reason for rejection');
                return;
            }
        }
        APASAmendments.do_process_apas('UPDATE',s,seq_id,approve_cancel_amendments_callback); 
        alert('Transaction processed');
        populate_rec();
        show_hide_form('tblSearch','block');
        show_hide_form('div_results','none');
    }
    
    
}


function approve_cancellation(stat)
{
    var data_row = new Array();
    var msg = (stat == 'Approved') ?  'Do you really want to cancel this transaction' : 'Do you really want to disapprove cancellation of this transaction';
    if(confirm(msg))
    {
        if(stat == 'Approved')
        {
            data_row[data_row.length] = "ApprovedCancellation~"               + true; 
            data_row[data_row.length] = "DateCancelled~"    + dateToString(APASAmendments.get_branch_localtime(branchcode).value); 
            data_row[data_row.length] = "CancelledBy~"               + uid; 
            data_row[data_row.length] = "WithCancellation~"          + false;
            data_row[data_row.length] = "trxn_stat~"                 + '4';  //Cancelled Transaction
            var s = data_row.join('|');
        }
        else if(stat == 'Cancelled')
        {
            if(document.getElementById('txtReason').value.length > 0)
            {
                data_row[data_row.length] = "DisapprovedCancellation~"               + true; 
                data_row[data_row.length] = "DateCancelled~"    + dateToString(APASAmendments.get_branch_localtime(branchcode).value); 
                data_row[data_row.length] = "CancelledBy~"               + ''; 
                data_row[data_row.length] = "WithCancellation~"          + false; 
                data_row[data_row.length] = "CancelReason~"             + ''; 
                data_row[data_row.length] = "ApprovedCancellation~"     + false;
                var s = data_row.join('|');
            }
            else
            {
                alert('Please input reason for rejection');
                return;
            }
        }
        APASAmendments.do_process_apas('UPDATE',s,seq_id,approve_cancel_amendments_callback); 
        alert('Transaction processed');
        populate_rec();
        show_hide_form('tblSearch','block');
        show_hide_form('div_results','none');
    }
    
    
}




function approve_cancel_amendments_callback(result)
{
    if (result.value == null)
    {
    }        
    else
    {
        alert(result.value);
    }

}

function get_client_data_details()
{
    branchcode = APASAmendments.get_irsclient_data('LOCATION').value;
    uid = APASAmendments.get_irsclient_data('UID').value;
}