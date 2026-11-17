var dbgrid;
var ctrlname2;
var excess_amt;
var no_of_months;
var pipe_value = '';
var comma_value = '';
var product_charges_id = '';
var uRole = '';
var uLoc = '';
var prn_value = '';
var has_prereq = '';

/* RFC-25832 PAG-IBIG API INTEGRATION */
var pagibig_api_details = null;
var xdeduction = null;
var xamount_due = null;
var xdue_date = null;
var obps_referenceno = null;
/* RFC-25832 PAG-IBIG API INTEGRATION */

/* RFC-27195 GIFTAWAY API */
var giftaway_select = null;
var merchant_id = "";
var merchant_name = "";
var merchant_deno = "";
var merchant_qty = "";
var recipient_name = ""
var recipient_mobile = "";
var recipient_email = "";
var giftaway_special_msg = "";
/* RFC-27195 GIFTAWAY API*/

function load_page(type)//** Hide and display controls
{
    if (type == '')
    {
        document.getElementById("ddlSourceCurrency").disabled = true;
        document.getElementById("tblForexRate").style.display = "none";
        document.getElementById("trTodayExRate").style.display = "";
    }
    else
    {
        document.getElementById("tdSearchRemitter").style.display = "none";
        document.getElementById("tdProduct").style.display = "none";
        document.getElementById("tdProductCode").style.display = "none";
        document.getElementById("tdImplem").style.display = "none";
        document.getElementById("tdImplemType").style.display = "none";
        document.getElementById("tdbtnCharges").style.display = "none";
        document.getElementById("tbPaymentDet_1").style.display = "none";
        document.getElementById("tbPaymentDet_2").style.display = "none";
        document.getElementById("tbPaymentDet_4").style.display = "none";
        document.getElementById("tbSpace_1").style.display = "none";
        document.getElementById("tbSpace_3").style.display = "none";
        document.getElementById("trRemitterID").style.display = "none";
        document.getElementById("tblForexRate").style.display = "";
        document.getElementById("trTodayExRate").style.display = "none";
        document.getElementById("btnAnchor").style.display = "none";
    }

    document.getElementById("tblSearch").style.display = "none";
    sidebar.get_side_bar_visibility("divtdBillsPayOBPSTransactionsMenu", "btnAnchor");

    document.getElementById("tdBillsPayOBPSTransactions").style.display = "";
    document.getElementById("tblMain").style.display = "";
    document.getElementById("tblBPRemitter").style.display = "";
    document.getElementById('TrnxDetails').style.display = '';
    document.getElementById("tdbtnSave").style.display = "none";
    document.getElementById("tdbtnDetails").style.display = "none";
    document.getElementById("tdbtnCancel").style.display = "none";
    document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.display = "";
    document.getElementById("tblBillsPayOBPSTransactionsMerchants").align = "right";
    document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.height = "100%";
    document.getElementById("Button1").disabled = true;
    document.getElementById("btnSave").disabled = true;
    document.getElementById("btnCharges").disabled = true;
    document.getElementById("txtActualAmount").value = '0';

    has_prereq = '';
}

function search_page() //**Search remitter box
{
    document.getElementById('tblSearch').style.display = "";

    showPanelOB('divSearch');
}

function fix_dim() //** Fix Design upon load for Cross-browser compatibility **
{
    if (top.document.getElementById('divCenter'))
    {
        var div = document.getElementById('divalpha');

        if (div)
        {
            div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
            div.style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';            
            div = document.getElementById('divmain');

            if (div)
            {
                var xd = (isFF | isIE8) ? 45 : 0;

                div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
                div.style.height = (top.document.getElementById('divCenter').offsetHeight - xd) + 'px';
            }
        }
    }
}

function detectsearch_keyup(enable) //** Search Enter button event
{
    document.getElementById('divSearch').onkeyup = (enable) ? btn_enter : null;
}

function detectamount_keyup(enable)//** Amount Enter button event
{
    document.getElementById('txtTargetAmount').onkeyup = (enable) ? enter_targetamoumt : null;
}

function detectamount_keypress(enable) //** Amount Keydown button event
{
    document.getElementById('txtTargetAmount').onkeypress = (enable) ? keyDown : null;
}

/*  DMND0002382 -Remittance SSS Enhanced e-Collection Service */
function convertToMMYYYY(val) {
    const months = {
        JANUARY: "01",
        FEBRUARY: "02",
        MARCH: "03",
        APRIL: "04",
        MAY: "05",
        JUNE: "06",
        JULY: "07",
        AUGUST: "08",
        SEPTEMBER: "09",
        OCTOBER: "10",
        NOVEMBER: "11",
        DECEMBER: "12"
    };

    const parts = val.toUpperCase().split(" ");
    const month = months[parts[0]];
    const year = parts[1];

    return month + year;
}
function page_setup() //** Initialize Page
{
    uRole = IRSWorld.BillsPayment.OBPSTransactions.getUserRole().value;
    uLoc = IRSWorld.BillsPayment.OBPSTransactions.getUserLoc().value;

    AjaxPro.timeoutPeriod = 10000 * 1000;
    AjaxPro.onTimeout = function()
    {
        alert("SERVER NOT RESPONDING.");
    }
    
    fix_dim();    
    detectsearch_keyup(true);

    if (uRole == '10')
    {
        if (hasQueryStrings(window))
        {        
            remteller.getbranches();
            remteller.getcurrencycat();
            remteller.SEQ_ID = getQueryString(window, 'rem_seqid');
            remteller.popRemDetail();
            detectamount_keyup(true);
            detectamount_keypress(true);
            remteller.RemitMethod = '9'
        }
        else
        {
            idisclaimer.disclaimer_form();
        }

        remteller.getTodayForex();
    }
    else
    {
        if (hasQueryStrings(window))
        {
            var fun = getQueryString(window, 'function');

            if (fun == 'back_to_billspayment')
            {            
                remteller.getbranches();
                remteller.getcurrencycat();
                remteller.SEQ_ID = getQueryString(window, 'rem_seqid');

                if(uRole == '7' || uRole == '27' || uRole == '25')
                {
                    remteller.RemitMethod = '10';
                    document.getElementById("btnWaive").style.display = "none";
                }
                else
                {
                    remteller.RemitMethod = '2'
                }

                document.getElementById("tblForexRate").style.display = "none";
                remteller.popRemDetail();

                sidebar.get_side_bar_visibility("divtdBillsPayOBPSTransactionsMenu","btnAnchor");                
                detectamount_keyup(true);
                detectamount_keypress(true);
            }        
        }
        else
        {        
            load_page('');
            sidebar.construct_side_bar();

            remteller.getbranches();
            remteller.getcurrencycat();

            if (uRole == '7' || uRole == '27' || uRole == '25')
            {
                remteller.RemitMethod = '10';
                document.getElementById("btnWaive").style.display = "none";
            }
            else
            {
                remteller.chkRatesComplete();
                remteller.RemitMethod = '2';
            }

            detectamount_keyup(true);
            detectamount_keypress(true);
        }
    }
    //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
    remteller.close_session();
    remteller.Check_ACH_Branch_Extract_Setup();
}

function btn_enter(e) //** Triggers the Enter Button
{
    var src;    
    if (!e) var e = window.event;
        if (!e.target) 
            src = window.event.srcElement;
        else 
            src = e.target;
    if (src.tagName == "INPUT" & src.type == 'text')
    {
        if (e.keyCode == 13)
        {
            document.getElementById('btnsearch').click();
            detectsearch_keyup(false);
        }
    }
}

function enter_targetamoumt(e)
{
    var src;

    if (!e) var e = window.event;
    if (!e.target)
        src = window.event.srcElement;
    else
        src = e.target;

    if (src.tagName == "INPUT" & src.type == 'text')
    {
        if (e.keyCode == 13)
        {
            if (has_prereq == "yes")
            {
                alert('Click Validate button to validate the payment amount.');
            }
         
            else
            {
                document.getElementById("txtTargetAmount").value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(document.getElementById("txtTargetAmount").value).value;
                remteller.NettoBeneAmountPHP = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);
                populate_rec();
            }
        }
    }
}

function execute_ajx_command(param)
{
    if (remteller != null && remteller != "undefined")
    {
        remteller.do_gridsearch(param);
    }
}

function execute_ajaxgridview_command(param)
{    
    if (remteller != null && remteller != "undefined")
    {
        remteller.do_gridview(param);
    }
}

function frm_validation()
{
    this.generate_opt_enable = function(parameter)
    {    
        this.clear_optbtn_remitter_info();

        document.getElementById(parameter).value = "on";
        document.getElementById(parameter).checked = true;

        if (parameter == "optWalkin")
            document.getElementById("btnSearchRemitter").value = "New";
        else
            document.getElementById("btnSearchRemitter").value = "Search";
    }

    this.clear_optbtn_remitter_info = function()
    {
        var ids = new Array();

        ids[0] = "optInternet";
        ids[1] = "optWalkin";
        ids[2] = "optExisting";
            
        var ctr = 0;

        while (ctr <= 2)
        {
            document.getElementById(ids[ctr]).value = "off"; 
            document.getElementById(ids[ctr]).checked = false;    
            ctr++;
        }
    }
       
    this.check_btnSearchRemitter = function()
    {
        remteller.callLookUp('REMITTER');            
    }
}

function frm_bills()
{
    this.ShowSearch = function()
    {
        if (confirm('Do you want to cancel payment?'))
        {
            window.open('../BillsPayment/OBPSTransactions.aspx','_self');
        }
    }

    this.ChargesVisibility = function(tblname)
    {
        
        if (document.getElementById(tblname).style.display  ==  "")
        {
            if (tblname=='BillsPayOBPSTransactionstrxncolumn')
            {
                document.getElementById('btnHide').value = 'Show Transaction Details';
            }
            else if (tblname=='tblBillsPayOBPSTransactionsMDetails')
            {
                
                document.getElementById('btnMerchD').value = 'Show Merchant Details';
                document.getElementById('tblMerchantDetails').style.display = "none";
                document.getElementById('tblBillsPayOBPSTransactionsMDetails').style.display = "none";
                
            }
            else if (tblname=='tblChargeTable')
            {
                document.getElementById('tblBPRemitter').style.display = "";
                document.getElementById('tblBillsPayOBPSTransactionsMerchants').style.display = "";
                document.getElementById('btnViewCharges').value = 'Show Charges';
            }

            document.getElementById(tblname).style.display = "none";
        }
        else
        {
            if (tblname=='BillsPayOBPSTransactionstrxncolumn')
            {
                document.getElementById('btnHide').value = 'Hide Transaction Details';
            }
            else if (tblname=='tblBillsPayOBPSTransactionsMDetails')
            {
                document.getElementById('btnMerchD').value = 'Hide Merchant Details';
                document.getElementById('tblMerchantDetails').style.display = "";
                document.getElementById('tblBillsPayOBPSTransactionsMDetails').style.display = "";
            }
            else if (tblname=='tblChargeTable')
            {
                document.getElementById('tblBPRemitter').style.display = "none";
                document.getElementById('tblBillsPayOBPSTransactionsMerchants').style.display = "none";
                document.getElementById('btnViewCharges').value = 'Hide Charges';
            }

            document.getElementById(tblname).style.display = "";
        }
    }
    
    this.hide_panel = function()
    {
        document.getElementById("tblBillsPayOBPSTransactionsMDetails").style.display = "none";
        document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.display = "none";
    }
}

//===========================================================================================
/* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
function push_override_hit_list_result(param) {
    if (remteller != null) {
        remteller.hit_list_override_response(param);
    }
}
//===========================================================================================

function bills_rem_teller()
{               
    this.SEQ_ID = null;
    this.UserID = null;
    this.RemitMethod = null;
    this.RemitStationID = null;
    this.RemittingBranch = null;
    this.ReferenceNo = null;
    this.ExtlReferenceNo = null;
    this.Transtatus = null;
    this.TranStatusDesc = null;
    this.TranxDate = null;
    this.ProductChargesID = null;
    this.RemitterCIFID = null;
    this.BeneficiaryCIFID = null;
    this.AccountSEQID = null;
    this.RemittingCurrency = null;
    this.AccountCurr = null;
    this.GrossRemAmount = null;
    this.NetRemAmount = null;
    this.NettoBeneCurrency = null;
    this.NettoBeneAmount = null;
    this.NettoBeneAmountPHP = null;
    this.TotalLocalCharge = null;
    this.TransactionSource = null;
    this.ReceiptMode = null;
    this.TransactionPurpose = null;
    this.OtherPurpose = null;
    this.SpecialMessage = null;
    this.DateImplemented = null;
    this.DateSent = null;
    this.printed = null;
    this.ForexLocalToRemitting = null;
    this.ForexRemittingToUSD = null;
    this.ForexRemittingToPHP = null;
    this.ForexLocalToUSD = null;
    this.ForexLocalToPHP = null;
    this.ForexRemittingToAccount = null;
    this.SourceofFunds = null;
    this.AccountNo = null;
    this.Currency = null;
    this.RemitterFullName = null;
    this.RemitterAddress = null;
    this.RemitterPhoneNumber = null;
    this.RemitterAddressSEQID = 0;
    this.RemitterPhoneNumberSEQID = 0;
    this.RemitterID = null;
    this.EnabledRoundUp = null;
    this.MerchantName = null;
    this.MerchantCode = null;
    this.MemberID = null;
    this.MerAccountCurr = null;
    this.MerAccountNumber = null;
    this.MerProdCode = null;
    this.MerProdDesc = null;
    this.MerchantCode2 = null;
    this.grid_ajax_page = "../AjxPage/ajaxgrid.aspx";
    this.LookUpMode = null;
    this.LIFldRef = null;
    this.MerchantType = null;
    this.Clicked_Grid = null;
    this.BranchCode = null;
    this.fForApproval = null;
    this.RemitterChargeType = null;
    this.DefaultsChargeType = null;
    this.MerchantSeqID = null;
    this.Exceptions = null;
    this.exception_screen = null;
    this.cheque_payments = null;
    this.card_payments = null;
    this.debit_account_payments = null;
    this.ach_payments = null;
    this.iscard_mode = 0;
    this.isdacct_mode = 0;
    this.the_mode = 0;
    this.remittance_amount_limit = 0;
    this.remittance_teller_limit = 0;
    this.remittance_aggregate_tag = 0;
    this.remittance_superv_limit = 0;
    this.remittance_limit_return = null;
    this.branch_cutend = null;
    this.allowed_cash_mgt = 0;
    this.cash_management = null;
    this.got_limit = 0;
    this.got_rem_contact = 0;
    this.NotificationMode = null;
    this.trandata = null;
    this.debitcreditscreen = null;
    this.remittance_cash_limit_tag = 0;
    this.remittance_cash_limit = 0;
    this.agentlogreport = 0;
    this.agentsysctrl = 0;
    this.isRemitterIDs = false;
    this.agentrejects = false
    this.checkidamount = 0;
    this.pending_ro_status = 0;
    this.transaction_status = 0;

    //Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017
    this.Remitter_AccountType = null;
    this.Remitter_AccountNo = null;
    this.Remitter_RoutingNo = null;
    this.Remitter_BankName = null;
    this.paymenttype = null;
    this.ACH_Branch_Setup = false;
    this.IsRemitter_ACH = false;
    
    //===========================================================================================
    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
    this.hit_stream_lining = null;
    //===========================================================================================

    /* RFC-21138 RED RIBBON */
    this.dropdown_li_fieldanme = null;
    /* RFC-21138 RED RIBBON */

    this.display_transactions = function()
    {
        document.getElementById("divCharges").style.display = "";
    }
    
    this.displaycharges = function()
    {
        if(document.getElementById('txtTargetAmount').value != '')
        {
            if (product_charges_id != '')
            {
                var target_amt = parseFloat(new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value));
                var account_curr = document.getElementById('lbl_merchant_currency').value;

                IRSWorld.BillsPayment.OBPSTransactions.getCalculator(product_charges_id, account_curr, false, 3, target_amt.toString(), this.displaycharges_callback);
                showPanelOB('divpopProcess');
                new common_method().to_show_loading_message('divLoadingMsg');
                setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
            }
            else
            {
                document.getElementById('calc').innerHTML = '';

                this.getchargesdet();

                document.getElementById("btnSave").disabled = true;
                document.getElementById("btnCharges").disabled = true;
            }
        }
    }

    this.displaycharges_callback = function(param)
    {
        if (param.error == null)
        {
            document.getElementById('calc').innerHTML = param.value;
            remteller.getchargesdet();

            if (remteller.EnabledRoundUp == '1')
            {
                var actualamount = IRSWorld.BillsPayment.OBPSTransactions.getActualPayment().value;
                document.getElementById('txtActualAmount').value = actualamount;
            }

            document.getElementById("btnSave").disabled = false;
            document.getElementById("btnCharges").disabled = false;
            document.getElementById("btnTrnxDetails").disabled = false;
            document.getElementById("ddlSourceCurrency").disabled = false;

            hidePanelOB('divpopProcess');

            if (document.getElementById('divTransactionDetails').style.display == '')
            {
                hidePanelOB('divTransactionDetails');
                showPanelOB('divTransactionDetails');
            }
        }
        else
        {
            alert(param.error);
            hidePanelOB('divpopProcess');
        }
    }

    this.getbranches = function()
    {
        var dt = IRSWorld.BillsPayment.OBPSTransactions.getBranches().value;

        this.BranchCode = dt.Rows[0].BranchCode.toString();

        document.getElementById("lblBranch").value = dt.Rows[0].BranchCode.toString();
        setText(document.getElementById("lblBranch"), dt.Rows[0].BranchName.toString());
    }

    this.callLookUp = function(parameter)
    {
        this.LookUpMode = parameter;
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = this.getLookUpSQL();

        var indx = (parameter == 'REMITTER' || parameter == 'INTERNET') ? 3 : 0;

        if (this.grid_ajax_query != null)
        {
            this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, indx, this.SEQ_ID);
        }
    }

    this.getLookUpSQL = function()
    {   
        var SQL = null;

        if (this.LookUpMode == "REMITTER")
        {
            var firstname = document.getElementById('txtSearchFirstname').value;
            var lastname = document.getElementById('txtSearchLastname').value;
            var middlename = document.getElementById('txtSearchMiddlename').value;
            var remCIFID = document.getElementById('txtRemitterCIFID').value;
            var oldCIFID = document.getElementById('txtOldRemitterCIFID').value;
            SQL = 'sp_CIFSearch~1~1~' + lastname + '~' + firstname + '~' + middlename + '~' + remCIFID + '~' + oldCIFID + '~' + uRole + '~' + uLoc + '~';
        }
        else if (this.LookUpMode == "REMITTER_ADDRESS")
        {
            SQL = 'sp_GetOBPRemitterAdd~' + remteller.RemitterID + '~';
        }
        else if (this.LookUpMode == "REMITTER_CONTACT")
        {
            SQL = 'sp_GetOBPRemitterContact~' + remteller.RemitterID + '~';
        }
        else if (this.LookUpMode == "MERCHANT")    
        {
            if (this.RemitterID != null || this.RemitterID != '')
            {
                var merchcount = IRSWorld.BillsPayment.OBPSTransactions.getMerchantList(this.RemitterID).value.Rows.length;

                if (merchcount > 0)
                {
                    SQL = "spGetEnrollMerchantInfo~" + "0~" + this.RemitterID;
                }
                else
                {
                    if (confirm('Remitter has no enrolled merchant. Do you want to enroll merchant for this remitter?'))
                    {
                        window.location = '../CIF/cifmain.aspx?function=add_merchant&cifid=' + this.RemitterID + '';
                    }
                    else
                    {
                        window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
                        return;
                    }
                }
            }
        }
        else if (this.LookUpMode == "TRANXDEFAULTS")
        {
            SQL = "sp_GetTranxDefaults~" + this.BranchCode;
        }
        else if (this.LookUpMode == "TRANXDETAILS")
        {
            SQL = "spGetOBPSTransactionHistory~" + "0~" + this.RemitterID;
        }
        else if (this.LookUpMode == "REMACCOUNTNO")
        {
            SQL = "usp_REMAcctNo~" + remteller.RemitterID;
        }
        else if (this.LookUpMode == "REMITTER_ID" && remteller.isRemitterIDs == true)
        {
            SQL = "isp_agentsys_gridremitter_ids~" + remteller.RemitterID;
        }
        else
        {
            SQL = "sp_GetlookupPerbranch~" + this.LookUpMode;
        }

        return SQL;
    }

    this.do_ajax = function(parameter)
    {   
        if (this.LookUpMode == 'SourceOfFunds' | this.LookUpMode == 'TranxSource' | this.LookUpMode =='TranxPurpose' | this.LookUpMode =='PayMode' | this.LookUpMode =='ReceiptMode' | this.LookUpMode =='ChargeType')
        {
            sidebar.grd_search.process_ajax(parameter);
            this.call_record_loader(parameter);
        }
        else
        {
            this.ajax_grid.process_ajax(parameter);
            this.call_record_loader(parameter);
        }
    }

    this.call_record_loader = function(parameter)
    {        
        var command_param = null;

        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if (this.LookUpMode == 'SourceOfFunds' | this.LookUpMode == 'TranxSource' | this.LookUpMode =='TranxPurpose' | this.LookUpMode =='PayMode' | this.LookUpMode =='ReceiptMode' | this.LookUpMode =='ChargeType')
                {
                    sidebar.do_gridfind(parameter);
                    return;
                }
                else if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];

                    if (this.LookUpMode == "REMITTER")
                    {
                        this.popRemDetail();
                    }
                    else if (this.LookUpMode == "REMITTER_ADDRESS")
                    {
                        this.remitter_address();
                    }
                    else if (this.LookUpMode == "REMITTER_CONTACT")
                    {
                        this.remitter_contact();
                    }
                    else if (this.LookUpMode == "MERCHANT")
                    {
                        this.popMerDetail();
                    }
                    else if (this.LookUpMode == "TRANXDETAILS")
                    {
                    }
                    else if(this.LookUpMode == "REMACCOUNTNO")
                    {
                        this.getRemAccountNo();
                    }
                }

                this.ajax_grid.process_ajax(parameter);
            }
        }
    }

    this.do_gridview = function(parameter)
    {        
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {                           
                if (command_param[0] == "ajx_selected")
                {
                    var command_value = command_param[1].split('~');

                    if (this.Clicked_Grid == 'TRANXDETAILS')
                    {
                        obpsview.Display_Transaction(command_value[0]);
                    }
                    else if (remteller.isRemitterIDs == true)
                    {
                        agentRem.getSystemControl_RemitterID(getText(document.getElementById('lbl_rem_id')), command_param[1]);
                    }
                    else if (this.the_mode == 6)
                    {
                        this.cheque_payments.do_ajaxgridview(parameter);
                    }
                    else if (this.iscard_mode == 1)
                    {
                        remteller.card_payments.do_ajaxgridview(parameter);
                    }
                    else if (this.isdacct_mode == 1)
                    {
                        this.debit_account_payments.do_ajaxgridview(parameter);
                    }
                    //===================================================================================================
                    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
                    else if (this.the_mode == 11)
                    {
                        //Do nothing as selected grid is watchlist grid. Override will be done once OK button is clicked 
                        this.hit_stream_lining.do_ajaxgridview(parameter);
                    }
                        //===================================================================================================
                    else
                    { 
                        this.ProductChargesID = command_value[0];
                        product_charges_id = command_value[0];

                        document.getElementById("tdBillsPayOBPSTransactions").style.display = "";
                        document.getElementById("tblMain").style.display = "";
                        document.getElementById("tblBPRemitter").style.display = "";
                        document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.display = "";
                        document.getElementById("tblBillsPayOBPSTransactionsMerchants").align = "right";
                        document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.height = "100%";

                        this.RemitterID = getText(document.getElementById('lbl_rem_id'));
                        this.callLookUp('MERCHANT');
                    }
                }
                else
                {    
                    //===================================================================================================
                    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
                    if (this.the_mode == 11) {
                        this.hit_stream_lining.do_ajaxgridview(parameter);
                    }
                        //===================================================================================================
                    else {
                    this.process_viewer(parameter);
                    }
                }
            }
        }
    }

    this.process_viewer = function(param)
    { 
        this.grid = new grid_viewer();
        this.grid.sql_source = "sp_rem_tran_grid_cards~" + this.cifid + "~";
        this.grid.div_container = document.getElementById("CARDACCOUNTGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.process_ajax(param);
        this.grid = null;
    }

    this.hit_test = function(div_name)
    {
        this.Clicked_Grid = div_name;
    }

    this.pop_remittance_details = function(txn_seqid)
    {
    }

    this.get_remitter_info = function()
    {
        var SQL = "sp_GetOBPRemitterInfo~" + this.SEQ_ID;

        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
        IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL, remteller.get_remitter_info_callback);
    }

    this.get_remitter_info_callback = function(param)
    {
        hidePanelOB('divpopProcess');
        var dt = param.value;

        if (dt != null && dt.Rows.length != 0)
        {
            remteller.RemitterFullName = dt.Rows[0].Name.toString();
            remteller.RemitterID = dt.Rows[0].CIFID.toString();

            if (uRole != '10') {
                remteller.RemitterChargeType = dt.Rows[0].RemitterChargeType.toString();
            }
            else
            {
                remteller.RemitterChargeType = IRSWorld.BillsPayment.OBPSTransactions.getWebRemittanceChargeType(this.BranchCode).value;

                if (remteller.RemitterChargeType == null || remteller.RemitterChargeType == '' || remteller.RemitterChargeType == '0')
                {
                    remteller.RemitterChargeType = dt.Rows[0].RemitterChargeType.toString();
                }
            }

            remteller.SourceofFunds = dt.Rows[0].SourceofFunds.toString();

            if (remteller.RemitterChargeType != null && remteller.RemitterChargeType != '' && remteller.RemitterChargeType != '0')
            {                
                document.getElementById("lblChargeType").value = remteller.RemitterChargeType;
                setText(document.getElementById("lnkChargeType"), sidebar.lookup_desc("ChargeType", remteller.RemitterChargeType));
            }

            dt = null;

            document.getElementById("tblSearch").style.display = "none";

            setText(document.getElementById("lbl_rem_fullname"), remteller.RemitterFullName);
            setText(document.getElementById('lbl_rem_id'), remteller.RemitterID);

            if (remteller.RemitterID != null)
            {
                document.getElementById("Button1").disabled = false;
            }

            remteller.display_main();
            remteller.reminfo_check_remids();
        }
        else
        {
            window.open('../Main/frmCenter.aspx', '_self');
            alert('Remitter Info is incomplete. Please check if remitter has a preferred address.');
        }
    }

    //===========================================================================================
    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
    this.populate_merchant_info = function ()
    {
        setText(document.getElementById("lbl_merchant_name"), this.MerchantName);
        document.getElementById("lbl_merchant_name").Class = 'cell_desc';

        setText(document.getElementById('lbl_merchant_prodcode'), this.MerProdDesc);
        setText(document.getElementById('lbl_merchant_currency'), this.MerAccountCurr);

        document.getElementById('lbl_merchant_currency').value = this.AccountCurr;
        document.getElementById("txtMemberID").style.display = "";
        document.getElementById("txtMemberID").value = this.MemberID;

        if (document.getElementById("txtMemberID").value == '') {
            document.getElementById("txtMemberID").disabled = false;
        }
        else {
            document.getElementById("txtMemberID").disabled = true;
        }

        setText(document.getElementById('spnBenefAmount'), this.MerAccountCurr + " 0.00");

        document.getElementById('spnBenefAmount').value = this.AccountCurr + " 0.00";
        document.getElementById("btnSave").disabled = true;
        document.getElementById("btnCharges").disabled = true;
        document.getElementById('spnTotalCharges').innerHTML = "0.00";
        document.getElementById('spnNetAmount').innerHTML = "0.00";

        this.setAmountVisibility('');
        this.MerchantFields();

        document.getElementById('txtRemAccountNo').style.display = '';
        document.getElementById('btnRemAccountNo').style.display = ''
    }

    //===========================================================================================

    this.reminfo_check_remids = function()
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Checking Required Remitter IDs.....');
        IRSWorld.BillsPayment.OBPSTransactions.checkSystemControl_RemitterIDs(remteller.RemitterID, remteller.reminfo_check_remids_callback);
    }

    this.reminfo_check_remids_callback = function(result)
    {
        var response = result.value;

        if (response.length > 5 && response.substr(0, 6) == "Error:")
        {
            alert(response);
            return "";
        }

        var idmessage = "";

        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            var retobj = remteller.remitter_ids_checker(response);

            if (retobj.expireIDs > 0)
            {
                idmessage = retobj.idmessage_exp;
            }
        }

        hidePanelOB('divpopProcess');

        if (idmessage.length > 0)
        {
            alert(idmessage);
            remteller.RemitterID = null;
            document.getElementById("Button1").disabled = true;
        }
        else
        {
            remteller.LookUpMode = 'REMITTER_ADDRESS';
            remteller.callLookUp('REMITTER_ADDRESS');
        }
    }

    this.remitter_address = function()
    {
        var SQL = "sp_GetOBPRemitterAdd~" + remteller.RemitterID;

        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
        IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL, remteller.remitter_address_callback);
    }

    this.remitter_address_callback = function(param)
    {
        hidePanelOB('divpopProcess');

        var dt = param.value;
        var i;

        if (dt != null && dt.Rows.length != 0)
        {
            remteller.RemitterAddressSEQID = remteller.SEQ_ID;

            for (i = 0; i < dt.Rows.length; i++)
            {
                if (dt.Rows[i].SEQ_ID.toString() == remteller.RemitterAddressSEQID)
                {
                    break;
                }
            }

            remteller.RemitterAddress = dt.Rows[i].Address.toString();
            setText(document.getElementById('lbl_rem_address'), remteller.RemitterAddress);
            remteller.load_rem_states();
        }
    }

    this.check_remitter_contact = function()
    {
        var SQL = "sp_GetOBPRemitterContact~" + remteller.RemitterID;

        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
        IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL, remteller.check_remitter_contact_callback);
    }

    this.check_remitter_contact_callback = function(param)
    {
        hidePanelOB('divpopProcess');
        var dt = param.value;

        if (dt != null && dt.Rows.length != 0)
        {
            remteller.LookUpMode = 'REMITTER_CONTACT';
            remteller.callLookUp('REMITTER_CONTACT');
        }
        else
        {
            remteller.rem_no_contact();
        }
    }

    this.remitter_contact = function()
    {
        var SQL = "sp_GetOBPRemitterContact~" + remteller.RemitterID;

        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
        IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL, remteller.remitter_contact_callback);
    }

    this.remitter_contact_callback = function(param)
    {
        hidePanelOB('divpopProcess');

        var dt = param.value;
        var i;

        if (dt != null && dt.Rows.length != 0)
        {
            remteller.RemitterPhoneNumberSEQID = remteller.SEQ_ID;

            for (i = 0; i < dt.Rows.length; i++)
            {
                if (dt.Rows[i].SEQ_ID.toString() == remteller.RemitterPhoneNumberSEQID)
                {
                    break;
                }
            }

            remteller.RemitterPhoneNumber = dt.Rows[i].ContactNo.toString();
            setText(document.getElementById('lbl_rem_telno'), remteller.RemitterPhoneNumber);
            remteller.LookUpMode = 'MERCHANT';

            if (getQueryString(window, 'merchant_seqid') != null && getQueryString(window, 'merchant_seqid') != '')
            {
                remteller.SEQ_ID = getQueryString(window, 'merchant_seqid');
                remteller.popMerDetail();
            }
            else
            {
                remteller.callLookUp('MERCHANT');
            }

            if (uRole != '10')
            {
                sidebar.get_side_bar_visibility("divtdBillsPayOBPSTransactionsMenu", "btnAnchor");
            }

            detectamount_keyup(true);
            detectamount_keypress(true);
        }
    }

    this.rem_no_contact = function()
    {
        remteller.LookUpMode = 'MERCHANT';

        if (getQueryString(window, 'merchant_seqid') != null && getQueryString(window, 'merchant_seqid') != '')
        {
            remteller.SEQ_ID = getQueryString(window, 'merchant_seqid');
            remteller.popMerDetail();
        }
        else
        {
            remteller.callLookUp('MERCHANT');
        }

        if (uRole != '10')
        {
            sidebar.get_side_bar_visibility("divtdBillsPayOBPSTransactionsMenu", "btnAnchor");
        }

        detectamount_keyup(true);
        detectamount_keypress(true);
    }

    this.display_main = function()
    {
        if(uRole != '10')
        {
            document.getElementById("tdbtnCharges").style.display = "";
            document.getElementById("ddlSourceCurrency").disabled = false;
        }
        else
        {
            document.getElementById("tdbtnCharges").style.display = "none";
        }

        document.getElementById("tdBillsPayOBPSTransactions").style.display = '';
        document.getElementById("tblMain").style.display = '';
        document.getElementById("tblBPRemitter").style.display = '';
        document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.display = '';
        document.getElementById("tblBillsPayOBPSTransactionsMerchants").align = "right";
        document.getElementById("tblBillsPayOBPSTransactionsMerchants").style.height = "100%";
        document.getElementById("tdbtnSave").style.display = '';
        document.getElementById("tdbtnDetails").style.display = '';
        document.getElementById("tdbtnCancel").style.display = '';
    }

    this.load_tranxdefaults_list = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source = this.getLookUpSQL();
        this.grid.div_container = document.getElementById("DIV_BENE_LIST");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }

    this.load_transaction_list = function() //** May still be useful in the future...
    {
    }

    this.get_merchant_info = function()
    {
        var SEQ_ID = this.SEQ_ID;
        var dt = IRSWorld.BillsPayment.OBPSTransactions.getEnrollMerchantInfo(SEQ_ID, this.RemitterID).value;

        if (dt != null && dt.Rows.length != 0)
        {
            this.MerchantName = dt.Rows[0].Name.toString();
            this.MerchantCode = dt.Rows[0].MerchantCode.toString();
            this.MemberID = dt.Rows[0].MemberID.toString();
            this.MerAccountCurr = dt.Rows[0].AccountCurrency.toString();
            this.MerAccountNumber = dt.Rows[0].AccountNo.toString();
            this.MerProdCode = dt.Rows[0].MerchantProductCode.toString();
            this.MerProdDesc = dt.Rows[0].ProductDesc.toString();
            this.AccountSEQID = dt.Rows[0].AccountSeqID.toString();
            this.AccountNo = dt.Rows[0].AccountNo.toString();
            this.BeneficiaryCIFID = dt.Rows[0].CIFID.toString();
            this.MerchantType = dt.Rows[0].MerchantType.toString();

            IRSWorld.BillsPayment.OBPSTransactions.setMerchantType(this.MerchantType);

            this.AccountCurr = dt.Rows[0].Currency.toString();
            this.MerchantSeqID = dt.Rows[0].MerchanSeqID.toString();

            document.getElementById('txtActualAmount').value = 0;
            document.getElementById('txtTargetAmount').value = 0;

            var d = new Date();
            var month = (d.getMonth() == '0') ? '1' : d.getMonth() + 1;
            var tday = month + "/" + d.getDate() + "/" + d.getFullYear();

            dt = null;

            document.getElementById('TrnxDetails').style.display = '';
            document.getElementById('tdMerchantTable').style.height = '200px';
            document.getElementById('lblTranxHistory').style.display = "none";
            document.getElementById('DIV_TRAN_DETAILS').style.display = "none";

            if (this.MerchantType != "Generic")
            {
                document.getElementById('tdlblCharges').style.display = "none";
                document.getElementById('calc').style.display = "none";
                document.getElementById('trMerchant').style.display = '';
                document.getElementById('trMerchantDetails').style.display = '';
            }
            else
            {
                document.getElementById('txtMemberID').style.width = '70%';
                document.getElementById('ddMAccountCurrency').style.width = '100%';
                document.getElementById('ddMProductCode').style.width = '100%';
            }
        }
        else
        {
            alert('Failed to get Merchant Info.');
        }
    }

    this.popMerDetail = function()
    {
        if (this.LookUpMode == "MERCHANT")
        {
            this.get_merchant_info();
            this.LookUpMode = ''
        }

        if (this.MerchantName != null)
        {

            //===========================================================================================
            /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
            if (remteller.get_hit_list_all(this.RemitterID, this.BeneficiaryCIFID) == false) {
                this.populate_merchant_info();
            } */
            //===========================================================================================
            /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
            if (remteller.get_hit_list_all_refinitiv(this.RemitterID, this.BeneficiaryCIFID) == false) {
                this.populate_merchant_info();
            }
        }

        if (uRole == '10')
        {
            showPanelOB('divTransactionDetails');
        }
    }

    this.setAmountVisibility = function(visibility)
    {
        document.getElementById('txtTargetAmount').style.display = visibility;
        document.getElementById('txtActualAmount').style.display = visibility;
    }

    this.MerchantFields = function()
    {
        if (document.getElementById("ddMProductCode").style.display != "none")
        {
            this.MerProdCode = document.getElementById("ddMProductCode")[document.getElementById("ddMProductCode").selectedIndex].value;
        }

        this.removeTableRow();
        this.insertTableRow();
        this.getdefaults();
        this.load_settings();

        if (remteller.trim(this.MerchantCode.toUpperCase()) == "GENERIKA")
        {
            remteller.getTranxDetailHistory();
        }
    }

    this.ValidateTransactionSource = function(description)
    {
        if (description == 'Others')
        {
            document.getElementById('lblOthers').style.display = '';
            document.getElementById('tdOthers').style.display = '';
        }
        else
        {
            if (document.getElementById('lblOthers').style.display == '')
            {
                document.getElementById('lblOthers').style.display = 'none';
                document.getElementById('tdOthers').style.display = 'none';
            }
        }
    }

    this.getdefaults = function()
    {
        var dt = IRSWorld.BillsPayment.OBPSTransactions.getDefaults(this.BranchCode);
        dt = dt.value;

        if (dt == null || dt.Rows.length <= 0)
        {
            alert('Please set branch defaluts.');
        }
        else
        {
            var i;

            for (i=0; i <= dt.Rows.length - 1; i++)
            {
                if (dt.Rows[i].BranchDefValue.toString()=='RemittingCurrency')
                {                    
                    if (this.ProductChargesID != null & this.ProductChargesID != '')
                    {
                        var tbl = IRSWorld.BillsPayment.OBPSTransactions.getRemittingCurrency(this.ProductChargesID).value;

                        document.getElementById('lblRemitCurr').value = tbl.Rows[0].Currency.toString();
                        setText(document.getElementById('lblRemitCurr'), tbl.Rows[0].CurrencyDesc.toString());
                        document.getElementById('ddlSourceCurrency').value = tbl.Rows[0].Currency.toString();

                        if (IRSWorld.BillsPayment.OBPSTransactions.chkChargeType(remteller.RemitterChargeType,document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].value,document.getElementById("lblPayMode").value).value == true)
                        {
                            document.getElementById("lblChargeType").value = remteller.RemitterChargeType;
                            setText(document.getElementById("lnkChargeType"), sidebar.lookup_desc("ChargeType", remteller.RemitterChargeType));
                        }
                        else if (IRSWorld.BillsPayment.OBPSTransactions.chkChargeType(remteller.DefaultsChargeType,document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].value,document.getElementById("lblPayMode").value).value == true)
                        {
                            if (uRole != '10')
                            {
                                remteller.RemitterChargeType = remteller.DefaultsChargeType;
                            }
                            else
                            {
                                remteller.RemitterChargeType = IRSWorld.BillsPayment.OBPSTransactions.getWebRemittanceChargeType(this.BranchCode).value;
                            }

                            document.getElementById("lblChargeType").value = remteller.DefaultsChargeType;
                            setText(document.getElementById("lnkChargeType"), sidebar.lookup_desc("ChargeType", remteller.DefaultsChargeType));
                        }
                        else
                        {
                            document.getElementById('lblChargeType').value = '0'
                            setText(document.getElementById('lnkChargeType'), 'Please select');
                        }

                        if (document.getElementById('ddlSourceCurrency').value != 0)
                        {
                            this.setremittingccy();
                        }
                    }
                    else
                    {
                        document.getElementById('lblRemitCurr').value = dt.Rows[i].BranchDefCode.toString();
                        setText(document.getElementById('lblRemitCurr'), dt.Rows[i].BranchDefDescription.toString());
                        document.getElementById('ddlSourceCurrency').value = dt.Rows[i].BranchDefCode.toString();

                        if (IRSWorld.BillsPayment.OBPSTransactions.chkChargeType(remteller.RemitterChargeType,document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].value,document.getElementById("lblPayMode").value).value == true)
                        {
                            document.getElementById("lblChargeType").value = remteller.RemitterChargeType;
                            setText(document.getElementById("lnkChargeType"), sidebar.lookup_desc("ChargeType", remteller.RemitterChargeType));
                        }
                        else if (IRSWorld.BillsPayment.OBPSTransactions.chkChargeType(remteller.DefaultsChargeType,document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].value,document.getElementById("lblPayMode").value).value == true)
                        {
                            if (uRole != '10')
                            {
                                remteller.RemitterChargeType = remteller.DefaultsChargeType;
                            }
                            else
                            {
                                remteller.RemitterChargeType = IRSWorld.BillsPayment.OBPSTransactions.getWebRemittanceChargeType(this.BranchCode).value;
                            }

                            document.getElementById("lblChargeType").value = remteller.DefaultsChargeType;
                            setText(document.getElementById("lnkChargeType"), sidebar.lookup_desc("ChargeType", remteller.DefaultsChargeType));
                        }
                        else
                        {
                            document.getElementById('lblChargeType').value = '0'
                            setText(document.getElementById('lnkChargeType'), 'Please select');
                        }

                        if (document.getElementById('ddlSourceCurrency').value != 0)
                        {
                            this.setremittingccy();
                        }
                    }
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='PayMode')
                {
                    if (uRole != '10')
                    {
                        if (dt.Rows[i].BranchDefCode.toString() == '4')
                        {
                            var hCards = IRSWorld.TranClass.rem_card_screen.has_cards(this.RemitterID).value;

                            if (hCards != true)
                            {
                                alert("There is no cards maintained for this remitter.")
                            }
                        }
                        else if (dt.Rows[i].BranchDefCode.toString() == '6')
                        {
                            var hDAccount = IRSWorld.TranClass.rem_debit_account_screen.has_debit_accounts(this.RemitterID).value;

                            if (hDAccount != true)
                            {
                                alert("There is no account maintained for this remitter.")
                            }
                        }

                        document.getElementById('lblPayMode').value = dt.Rows[i].BranchDefCode.toString();
                        setText(document.getElementById('lnkPaymode'), dt.Rows[i].BranchDefDescription.toString());
                    }
                    else
                    {
                        if (dt.Rows[i].BranchDefCode.toString() == '4' || dt.Rows[i].BranchDefCode.toString() == '6')
                        {
                            if (dt.Rows[i].BranchDefCode.toString() == '4')
                            {
                                var hCards = IRSWorld.TranClass.rem_card_screen.has_cards(this.RemitterID).value;

                                if (hCards != true)
                                {
                                    alert("There is no cards maintained for this remitter. Please go to the nearest PNB branch.")
                                }
                            }
                            else if (dt.Rows[i].BranchDefCode.toString() == '6')
                            {
                                var hDAccount = IRSWorld.TranClass.rem_debit_account_screen.has_debit_accounts(this.RemitterID).value;

                                if (hDAccount != true)
                                {
                                    alert("There is no account maintained for this remitter. Please go to the nearest PNB branch.")
                                }
                            }

                            document.getElementById('lblPayMode').value = dt.Rows[i].BranchDefCode.toString();
                            setText(document.getElementById('lnkPaymode'), dt.Rows[i].BranchDefDescription.toString());
                        }
                    }
                    //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
                    this.paymenttype = document.getElementById('lblPayMode').value;
                    this.Check_IsRemitter_ACH();
                    this.Enable_RemAccount_Details();
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='SourceOfFunds')
                {
                    if (remteller.SourceofFunds != '0' && remteller.SourceofFunds != null && remteller.SourceofFunds != '')
                    {
                        document.getElementById('lblSourceOfFunds').value = remteller.SourceofFunds;
                        setText(document.getElementById('btnSourceOfFunds'), sidebar.lookup_desc("SourceOfFunds", remteller.SourceofFunds));
                    }
                    else
                    {
                        document.getElementById('lblSourceOfFunds').value = dt.Rows[i].BranchDefCode.toString();
                        setText(document.getElementById('btnSourceOfFunds'), dt.Rows[i].BranchDefDescription.toString());
                    }
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='ReceiptMode')
                {
                    if (uRole != '10')
                    {
                        document.getElementById('lblReceiptMode').value = dt.Rows[i].BranchDefCode.toString();
                        setText(document.getElementById('lnkReceiptMode'), dt.Rows[i].BranchDefDescription.toString());
                    }
                    else
                    {
                        document.getElementById('lblReceiptMode').value = '5';
                        setText(document.getElementById('lnkReceiptMode'), sidebar.lookup_desc("ReceiptMode", '5'));
                    }
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='TranxSource')
                {
                    document.getElementById('lblTranxSource').value = dt.Rows[i].BranchDefCode.toString();
                    setText(document.getElementById('lnkTranxSource'), dt.Rows[i].BranchDefDescription.toString());

                    this.ValidateTransactionSource(dt.Rows[i].BranchDefDescription.toString());
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='TranxPurpose')
                {
                    document.getElementById('lblTranxPurpose').value = '2';
                    setText(document.getElementById('lnkTranxPurpose'), IRSWorld.BillsPayment.OBPSTransactions.get_tbl_lookup_description('TranxPurpose', '2').value);
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='RoundupOBPSAmount')
                {
                    this.EnabledRoundUp = dt.Rows[i].BranchDefCode.toString();

                    if (uRole != '10')
                    {
                        if (this.EnabledRoundUp == 1)
                        {
                            document.getElementById('tdlblActualAmount').style.display = '';
                            document.getElementById('tdActualAmount').style.display = '';
                        }
                        else
                        {
                            document.getElementById('tdlblActualAmount').style.display = 'none';
                            document.getElementById('tdActualAmount').style.display = 'none';
                        }
                    }
                }
                else if (dt.Rows[i].BranchDefCode.toString()=='9' & dt.Rows[i].BranchDefValue.toString().split('_')[0]=='BranchDefaultSetup')
                {
                    if (uRole != '10' && uRole != '7')
                    {
                        if (uRole == '25')
                        {
                            this.fForApproval = 'false'
                        }
                        else
                        {
                            this.fForApproval = dt.Rows[i].BranchDefValue.toString().split('_')[1];
                        }
                    }
                    else
                    {
                        this.fForApproval = 'true'
                    }
                }
                else if (dt.Rows[i].BranchDefCode.toString()=='6' & dt.Rows[i].BranchDefValue.toString().split('_')[0]=='BranchDefaultSetup')
                {
                    this.Exceptions = dt.Rows[i].BranchDefValue.toString().split('_')[1];
                }
                else if (dt.Rows[i].BranchDefCode.toString()=='4' & dt.Rows[i].BranchDefValue.toString().split('_')[0]=='BranchDefaultSetup')
                {
                    this.allowed_cash_mgt = ((dt.Rows[i].BranchDefValue.toString().split('_')[1] == 'true') ? 1 : 0);
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='ChargeType')
                {
                    remteller.DefaultsChargeType = dt.Rows[i].BranchDefCode.toString();

                    if (remteller.RemitterChargeType == null || remteller.RemitterChargeType == '' || remteller.RemitterChargeType == '0')
                    {
                        if (uRole != '10')
                        {
                            remteller.RemitterChargeType = remteller.DefaultsChargeType;
                        }
                        else
                        {
                            remteller.RemitterChargeType = IRSWorld.BillsPayment.OBPSTransactions.getWebRemittanceChargeType(this.BranchCode).value;
                        }

                        document.getElementById("lblChargeType").value = remteller.DefaultsChargeType;
                        setText(document.getElementById("lnkChargeType"), sidebar.lookup_desc("ChargeType", remteller.DefaultsChargeType));
                    }
                    else
                    {
                        document.getElementById("lblChargeType").value = remteller.RemitterChargeType;
                        setText(document.getElementById("lnkChargeType"), sidebar.lookup_desc("ChargeType", remteller.RemitterChargeType));
                    }
                }
                else if (dt.Rows[i].BranchDefValue.toString()=='NotificationMode')
                {
                    remteller.NotificationMode = dt.Rows[i].BranchDefCode.toString();
                }
            }
        }

        if (document.getElementById('lblRemitCurr').value != undefined)
        {
        }
        else
        {
            alert('Please set branch defaluts.');
        }

        this.getbranchsetup();
    }

    this.getbranchsetup = function()
    {
        var dt = IRSWorld.BillsPayment.OBPSTransactions.getBranchSetup(this.BranchCode).value;

        if (dt == null || dt.Rows.length <= 0)
        {
            alert('Please set branch setup.');
        }
        else
        {
            this.remittance_amount_limit = dt.Rows[0].RemittanceAmtLimit;
            this.remittance_teller_limit = dt.Rows[0].TellerLimit;
            this.remittance_superv_limit = dt.Rows[0].SupervisorLimit;
            this.remittance_aggregate_tag = dt.Rows[0].AggregateAmtLimit;
            this.branch_cutend = dt.Rows[0].CutOffEnd;
            this.remittance_cash_limit_tag = dt.Rows[0].CashAggregationLimit;
            this.remittance_cash_limit = dt.Rows[0].CashAggregationAmtLimit;
            this.agentlogreport = dt.Rows[0].AgentLogReport;
            this.agentsysctrl = dt.Rows[0].AgentSysControl;
            this.agentrejects = dt.Rows[0].AgentAggAmtReject;
            this.checkidamount = dt.Rows[0].AgentCheckIDAmount;
        }
    }

    this.setproductchargesid = function(currency, brcode)
    {
        var dt = IRSWorld.BillsPayment.OBPSTransactions.getProductChargesID(currency, brcode).value;

        if(dt.Rows.length > 0)
        {
            var ict;

            for (ict = 0; ict <= dt.Rows.length - 1; ict++)
            {
                if (this.RemitterChargeType == dt.Rows[ict].ChargeType.toString() & document.getElementById('lblPayMode').value == dt.Rows[ict].PayMode.toString())
                {
                    this.ProductChargesID = dt.Rows[ict].ProductChargesID.toString();
                    product_charges_id = dt.Rows[ict].ProductChargesID.toString();

                    if (this.fForApproval != 'true')
                    {
                        if (dt.Rows[ict].DefaultStatus.toString() == '15')
                        {
                            if (uRole == '25')
                            {
                                this.fForApproval = 'false';
                            }
                            else
                            {
                                this.fForApproval = 'true';
                            }
                        }
                        else
                        {
                            this.fForApproval = 'false';
                        }
                    }

                    break;
                }
                else
                {
                    this.ProductChargesID = '';
                    product_charges_id = '';
                }
            }

            if (this.ProductChargesID == '')
            {
                alert('There are no applicable charges for the combination selected!');
                showPanelOB('divTransactionDetails');
            }
        }
        else
        {
            this.ProductChargesID = '';
            product_charges_id = '';

            alert('There are no applicable charges for the combination selected!');
            showPanelOB('divTransactionDetails');
        }
    }

    this.checkPayMode = function()
    {
        this.PayMode = document.getElementById('lblPayMode').value;

        if (this.PayMode == '2')
        {
            document.getElementById('txtchkno').style.display = '';
            document.getElementById('lblcheckno').style.display = '';
            document.getElementById('txtClearanceDate').style.display = '';
            document.getElementById('lblClearanceDt').style.display = '';

            var d = new Date();
            var month = (d.getMonth() == '0') ? '1' : d.getMonth() + 1;
            var tday = month + "/" + d.getDate() + "/" + d.getFullYear();

            document.getElementById('txtClearanceDate').value = tday;
        }
        else
        {
            document.getElementById('txtClearanceDate').style.display = 'none';
            document.getElementById('lblClearanceDt').style.display = 'none';
            document.getElementById('txtchkno').style.display = 'none';
            document.getElementById('lblcheckno').style.display = 'none';
        }
    }

    this.newButton = function ()
    {
        var theTable = document.getElementById("TrnxDetails");
        var theTableBody = theTable.tBodies[0];
        var newCell;
        var newRow = theTableBody.insertRow(theTableBody.rows.length);

        newCell = newRow.insertCell(0);
        newRow.id = "tr_" + theTableBody.rows.length;
        newCell.id = "td1";
        newCell.style.fontSize = "11px";
        newCell.className = 'cell_caption';
        newCell.style.textAlign = "right";
        newCell.style.width = "30%";
        newCell.style.height = "23px";

        var el = document.createElement('input')

        el.type = 'button';
        el.name = "";
        el.id = "btnValidate";
        el.style.width = '45%';
        el.style.display = 'inline';
        el.value = "Validate";
        el.title = "";

        newCell.id = "tdValidate";
        el.onclick = Function('remteller.validate_prn();');
        newCell.appendChild(el);
        document.getElementById('tf1').value = "PRN";

        this.check_selected_sss_payment_type();

        var el2 = document.createElement('input')

        el2.type = 'button';
        el2.name = "";
        el2.id = "btnCancel";
        el2.style.width = '45%';
        el2.style.display = 'inline';
        el2.value = "Cancel";
        el2.title = "";
        el2.onclick = Function('remteller.clear_prn();');

        newCell.id = "tdCancel";
        newCell.appendChild(el2);
    }

    this.newSSSPEButton = function ()
    {

        document.getElementById('nf1').disabled = true;
        document.getElementById('nf1').style.display = "none";

        var theTable = document.getElementById("TrnxDetails");
        var theTableBody = theTable.tBodies[0];
        var newCell;
        var newRow = theTableBody.insertRow(theTableBody.rows.length);

        newCell = newRow.insertCell(0);
        newRow.id = "tr_" + theTableBody.rows.length;
        newCell.id = "td1";
        newCell.style.fontSize = "11px";
        newCell.className = 'cell_caption';
        newCell.style.textAlign = "right";
        newCell.style.width = "30%";
        newCell.style.height = "23px";

        var el = document.createElement('input')
        el.type = 'button';
        el.name = "";
        el.id = "btnValidate";
        el.style.width = '45%';
        el.style.display = 'inline';
        el.value = "Validate";
        el.title = "";

        newCell.id = "tdValidate";
        el.onclick = Function('remteller.validate_pesofund();');
        newCell.appendChild(el);                
        document.getElementById('tr_6').style.display = "none";

        var el2 = document.createElement('input')

        el2.type = 'button';
        el2.name = "";
        el2.id = "btnCancel";
        el2.style.width = '45%';
        el2.style.display = 'inline';
        el2.value = "Cancel";
        el2.title = "";
        el2.onclick = Function('remteller.clear_pesofund();');

        newCell.id = "tdCancel";
        newCell.appendChild(el2);

        document.getElementById('btnSave').disabled = true;
        has_prereq = "yes";
    }
	
    /* RFC-5305 SSS RTPL API CONNECTION */
    this.add_sss_rtpl_button = function()
    {
        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];
        var sss_rtpl_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var sss_rtpl_cell = null;

        sss_rtpl_cell = sss_rtpl_row.insertCell(0);
        sss_rtpl_row.id = "tr_" + optional_mer_body.rows.length;
        sss_rtpl_cell.id = "tdCancel";
        sss_rtpl_cell.className = "cell_caption";

        var sss_rtpl_el = document.createElement("input");

        sss_rtpl_el.type = 'button';
        sss_rtpl_el.id = "btnValidate";
        sss_rtpl_el.style.width = '50%';
        sss_rtpl_el.style.display = 'inline';
        sss_rtpl_el.value = "Validate";

        sss_rtpl_el.onclick = function ()
        {
            showPanelOB('divpopProcess');
            remteller.validate_sss_rtpl_prn();
        }

        sss_rtpl_cell.appendChild(sss_rtpl_el);

        document.getElementById('txtTargetAmount').disabled = true;

        if (document.getElementById('tr_6') != null)
        {
            document.getElementById('tr_6').style.display = 'none';
        }

        if (document.getElementById('tr_7') != null)
        {
            document.getElementById('tr_7').style.display = 'none';
        }

        if (document.getElementById('tr_8') != null)
        {
            document.getElementById('tr_8').style.display = 'none';
        }        
    }

    this.validate_sss_rtpl_prn = function()
    {
        var sss_rtpl_prn = "";

        if (document.getElementById('tf1') != null)
        {
            sss_rtpl_prn= document.getElementById('tf1').value
        }

        if (sss_rtpl_prn == null || sss_rtpl_prn == "")
        {
            alert("Please enter valid PRN."); hidePanelOB('divpopProcess'); return;
        }

        IRSWorld.BillsPayment.OBPSTransactions.validate_sss_rtpl_prn_number(sss_rtpl_prn, this.validate_sss_rtpl_prn_callback);
    }

    this.validate_sss_rtpl_prn_callback = function(result)
    {
        hidePanelOB('divpopProcess');

        var val = null;

        if (result != null && result.value != null && result.value != "")
        {
            val = result.value.split('~');

            if (val != null && val.length > 0 && val[1] == "0")
            {
                document.getElementById('tf2').value = "";
                document.getElementById('tf3').value = "";
                document.getElementById('tf4').value = "";

                if (document.getElementById('tr_6') != null)
                {
                    document.getElementById('tr_6').style.display = 'block';                    
                    document.getElementById('tr_6').style.display = "";
                    document.getElementById('tf2').value = val[3];
                }

                if (document.getElementById('tr_7') != null)
                {
                    document.getElementById('tr_7').style.display = 'block';
                    document.getElementById('tr_7').style.display = "";
                    document.getElementById('tf3').value = val[7];
                }

                if (document.getElementById('tr_8') != null)
                {
                    document.getElementById('tr_8').style.display = 'block';
                    document.getElementById('tr_8').style.display = "";
                    document.getElementById('tf4').value = val[8];
                }

                document.getElementById('tf1').disabled = true;
                document.getElementById('tf2').disabled = true;
                document.getElementById('tf3').disabled = true;
                document.getElementById('tf4').disabled = true;
                document.getElementById('btnValidate').disabled = true;
                document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(val[6]).value;

                detectamount_keypress(true);
                remteller.NettoBeneAmountPHP = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);
                populate_rec();
            }
            else
            {
                alert(result.value);
                document.getElementById('tf1').value = "";
                document.getElementById('tf2').value = "";
                document.getElementById('tf3').value = "";
                document.getElementById('tf4').value = "";
                document.getElementById('btnValidate').disabled = false;
            }
        }        
    }

    this.post_sss_rtpl_prn = function(tran_referenceno)
    {
        var sss_rtpl_prn = "";

        sss_rtpl_prn = document.getElementById('tf1').value;

        IRSWorld.BillsPayment.OBPSTransactions.post_sss_rtpl_prn_number(sss_rtpl_prn, tran_referenceno, this.post_sss_rtpl_prn_callback);
    }

    this.post_sss_rtpl_prn_callback = function(result)
    {
        hidePanelOB('divpopProcess');
    }
    /* RFC-5305 SSS RTPL API CONNECTION */

    /* BOYET. PARAMOUNT API INTEGRATION RFC-1111. 01-29-2020 */
    this.add_paramount_button = function()
    {
        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];
        var parammount_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var paramount_cell = null;

        paramount_cell = parammount_row.insertCell(0);
        parammount_row.id = "tr_" + optional_mer_body.rows.length;
        paramount_cell.id = "td_parmount_" + optional_mer_body.rows.length;
        paramount_cell.className = "cell_caption";

        var paramount_el = document.createElement("input");

        paramount_el.type = 'button';
        paramount_el.id = "btnValidate";
        paramount_el.style.width = '50%';
        paramount_el.style.display = 'inline';
        paramount_el.value = "Validate";

        paramount_el.onclick = function()
        {
            showPanelOB('divpopProcess');
            remteller.chk_paramount_reference_no();
        }

        paramount_cell.appendChild(paramount_el);

        document.getElementById('txtTargetAmount').disabled = true;
    }

    this.chk_paramount_reference_no = function()
    {
        var paramount_refno = "";

        if (document.getElementById("tf1") != null)
        {
            paramount_refno = document.getElementById("tf1").value;
        }

        IRSWorld.BillsPayment.OBPSTransactions.check_paramount_reference_number(paramount_refno, this.chk_paramount_reference_no_callback);
    }

    this.chk_paramount_reference_no_callback = function(res)
    {        
        if (res != null && res.value != "" && res.value.toUpperCase().indexOf("ERROR") < 0)
        {
            alert("Valid Paramount reference number. \nAmount to pay: " + res.value + "\n\nPlease click OK to continue.");

            if (document.getElementById("tf1") != null)
            {
                document.getElementById("tf1").disabled = true;
            }            

            if (document.getElementById("btnValidate") != null)
            {
                document.getElementById("btnValidate").disabled = true;
            }

            document.getElementById('txtTargetAmount').value = res.value;
            populate_rec();
            hidePanelOB('divpopProcess');

            return;
        }

        hidePanelOB('divpopProcess');

        alert(res.value);
    }

    this.post_paramount_payment = function()
    {
        var paramount_refno = "";
        var paramount_amt = "";
        var paramount_pay_mode = "";

        if (document.getElementById("tf1") != null)
        {
            paramount_refno = document.getElementById("tf1").value;
        }

        if (document.getElementById("lblPayMode") != null)
        {
            paramount_pay_mode = document.getElementById("lblPayMode").value;
            paramount_pay_mode = getText("lblPayMode");
            paramount_pay_mode = paramount_pay_mode.replace("\n", "");
            paramount_pay_mode = paramount_pay_mode.trim();
        }

        paramount_amt = document.getElementById('txtTargetAmount').value;

        IRSWorld.BillsPayment.OBPSTransactions.post_paramount_payment(paramount_refno, paramount_amt, paramount_pay_mode, this.post_paramount_payment_callback);
    }

    this.post_paramount_payment_callback = function(res)
    {
        if (res != null && res.value != null)
        {
            /* DO NOTHING. DAILY COLLECTION FILE SENT TO PARAMOUNT SFTP WILL BE USED FOR POSTING. */
        }
    }
	/* BOYET. PARAMOUNT API INTEGRATION RFC-1111. 01-29-2020 */

    /* RFC-4404 DEFINITION OF NEW BILLER MERALCO 05-26-2020 */
    this.chk_meralco_ref_is_valid = function ()
    {
        var meralco_refno = "";
        var meralco_amt = "";

        if (document.getElementById("tf1") != null)
        {
            meralco_refno = document.getElementById("tf1").value;
        }

        if (document.getElementById('txtTargetAmount') != null)
        {
            meralco_amt = document.getElementById('txtTargetAmount').value;
        }

        if (this.MerchantCode != null && this.MerchantCode.trim().toUpperCase() == "MERALCO")
        {
            if (this.MerProdCode != null && this.MerProdCode == "202")
            {
                if (meralco_amt == "" && meralco_refno == "")
                {
                    return;
                }

                if (meralco_refno == null || meralco_amt == null)
                {
                    alert("Please enter valid reference number and amount to pay."); return;
                }

                if (meralco_refno == "" || meralco_amt == "")
                {
                    alert("Please enter valid reference number and amount to pay."); return;
                }
            }

            if (this.MerProdCode != null && this.MerProdCode == "202")
            {
                IRSWorld.BillsPayment.OBPSTransactions.chk_meralco_refno(meralco_refno, meralco_amt, this.chk_meralco_ref_is_valid_callback); return;
            }
        }
    }

    this.chk_meralco_ref_is_valid_callback = function (result)
    {
        var targetamount = parseFloat(new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value));

        if (result != null && result.value == "OK")
        {
            remteller.pending_ro_status = 0;
            remteller.displaycharges();

            if (remteller.EnabledRoundUp == '1' & targetamount > 0)
            {
                var actualamount = IRSWorld.BillsPayment.OBPSTransactions.getActualPayment().value;
                document.getElementById('txtActualAmount').value = actualamount;
            }

            document.getElementById('txtTargetAmount').disabled = true;
            document.getElementById("tf1").disabled = true;
        }
        else
        {
            alert(result.value); return;
        }
    }
    /* RFC-4404 DEFINITION OF NEW BILLER MERALCO 05-26-2020 */

    /* 2. RFC-10309 PNB GENERAL INSURER OCTOBER 2020 */
    this.chk_pnb_gen_refnum = function ()
    {
        var pnb_gen_ref_num = "";
        var pnbgen_amt = null;

        if (this.MerchantCode != null && this.MerchantCode.trim().toUpperCase() == "PNB GENERAL INSURERS")
        {
            if (document.getElementById("tf1") != null)
            {
                pnb_gen_ref_num = document.getElementById("tf1").value;
            }

            if (document.getElementById('txtTargetAmount') != null)
            {
                pnbgen_amt = document.getElementById('txtTargetAmount').value;
            }

            if (pnb_gen_ref_num == "" && pnbgen_amt == "")
            {
                return;
            }

            if (pnb_gen_ref_num == null || pnb_gen_ref_num == "")
            {
                alert("Please enter valid reference number."); return;
            }

            if (pnbgen_amt == null || pnbgen_amt == "null" || pnbgen_amt == "")
            {
                alert("Please enter valid amount."); return;
            }

            if (parseFloat(pnbgen_amt) <= 0)
            {
                alert("Please enter valid amount."); return;
            }

            if (parseFloat(pnbgen_amt) > 0)
            {
                IRSWorld.BillsPayment.OBPSTransactions.chk_pnb_gen_refnum(pnb_gen_ref_num, this.chk_pnb_gen_refnum_callback);
            }
        }
    }

    this.chk_pnb_gen_refnum_callback = function (result)
    {
        var targetamount = parseFloat(new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value));

        if (result != null && result.value == "VALID")
        {
            remteller.pending_ro_status = 0;
            remteller.displaycharges();

            if (remteller.EnabledRoundUp == '1' & targetamount > 0)
            {
                var actualamount = IRSWorld.BillsPayment.OBPSTransactions.getActualPayment().value;
                document.getElementById('txtActualAmount').value = actualamount;
            }

            document.getElementById('txtTargetAmount').disabled = true;
            document.getElementById("tf1").disabled = true;
        }
        else
        {
            alert(result.value); return;
        }
    }
    /* 2. RFC-10309 PNB GENERAL INSURER OCTOBER 2020 */

    /* 2. RFC-10310 BENEFICIAL LIFE */
    this.chk_beneflife_premium = function ()
    {
        var benlife_premium_amt = null;
        var benlife_amt = null;

        if (this.MerchantCode != null && this.MerchantCode.trim().toUpperCase() == "BENEFICIAL LIFE")
        {
            if (document.getElementById("tf1") != null)
            {
                benlife_premium_amt = document.getElementById("tf1").value;
            }

            if (document.getElementById('txtTargetAmount') != null)
            {
                benlife_amt = document.getElementById('txtTargetAmount').value;
            }

            if (benlife_premium_amt == "" && benlife_amt == "")
            {
                return;
            }

            if (benlife_premium_amt == null || benlife_premium_amt == "")
            {
                alert("Please enter valid premium amount."); return;
            }

            if (benlife_amt == null || benlife_amt == "null" || benlife_amt == "")
            {
                alert("Please enter valid amount."); return;
            }

            if (parseFloat(benlife_amt) <= 0)
            {
                alert("Please enter valid amount."); return;
            }

            if (parseFloat(benlife_amt) > 0)
            {
                IRSWorld.BillsPayment.OBPSTransactions.chk_beneficial_premium_amt(benlife_premium_amt, this.chk_beneflife_premium_callback);
            }
        }
    }

    this.chk_beneflife_premium_callback = function (result)
    {
        var targetamount = parseFloat(new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value));

        if (result != null && result.value == "VALID")
        {
            remteller.pending_ro_status = 0;
            remteller.displaycharges();

            if (remteller.EnabledRoundUp == '1' & targetamount > 0)
            {
                var actualamount = IRSWorld.BillsPayment.OBPSTransactions.getActualPayment().value;
                document.getElementById('txtActualAmount').value = actualamount;
            }

            document.getElementById('txtTargetAmount').disabled = true;
            document.getElementById("tf1").disabled = true;
        }
        else
        {
            alert(result.value); return;
        }
    }
    /* 2. RFC-10310 BENEFICIAL LIFE */

    /* RFC-25832 PAG-IBIG API INTEGRATION */
    this.get_pagibig_api_details = function()
    {
        pagibig_api_details = null;

        if (this.MerchantCode != null && this.MerchantCode == 'PAG-IBIG CALAMITY LOAN' || this.MerchantCode == 'PAG-IBIG CONTRIBUTION' || this.MerchantCode == 'PAG-IBIG HOUSING - HAN' || this.MerchantCode == 'PAG-IBIG MP2 SAVINGS' || this.MerchantCode == 'PAG-IBIG SHORT TERM LOAN')
        {
            this.inquire_pagibig_api();
        }
        else if (this.MerchantCode != null && this.MerchantCode == 'PAG-IBIG HOUSING - PRN')
        {
            this.pagibig_housing_prn_button();
        }
    }

    this.pagibig_housing_prn_button = function()
    {
        document.getElementById('tf1').disabled = true;
        document.getElementById('tf2').disabled = true;
        document.getElementById('tf3').disabled = true;

        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];

        var pagibig_loan_prn_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var pagibig_loan_prn_cell = null;
        var pagibig_loan_prn_el = null;

        pagibig_loan_prn_row.id = "tr_" + optional_mer_body.rows.length;

        pagibig_loan_prn_cell = pagibig_loan_prn_row.insertCell(0);
        pagibig_loan_prn_cell.id = "tdValidate";
        pagibig_loan_prn_cell.className = "cell_caption";

        pagibig_loan_prn_el = document.createElement("input");
        pagibig_loan_prn_el.type = 'button';
        pagibig_loan_prn_el.id = "btnValidate";
        pagibig_loan_prn_el.style.width = '50%';
        pagibig_loan_prn_el.style.display = 'inline';
        pagibig_loan_prn_el.value = "Validate";

        pagibig_loan_prn_el.onclick = function ()
        {
            remteller.inquire_pagibig_api();
        }

        pagibig_loan_prn_cell.appendChild(pagibig_loan_prn_el);
    }

    this.inquire_pagibig_api = function ()
    {
        var member_id = document.getElementById("txtMemberID").value;

        if (member_id == null || member_id == "")
        {
            member_id = document.getElementById("tf5").value;
        }

        pagibig_api_details = null;
        xdeduction = null;
        xamount_due = null;
        xdue_date = null;

        document.getElementById('tf1').value = "";
        document.getElementById('tf2').value = "";
        document.getElementById('tf3').value = "";
        document.getElementById('tf1').disabled = true;
        document.getElementById('tf2').disabled = true;
        document.getElementById('tf3').disabled = true;

        showPanelOB('divpopProcess');

        /* MARCH 23 2023*/
        AjaxPro.timeoutPeriod = 10000 * 1000;
        AjaxPro.onTimeout = function() {
            alert("SERVER NOT RESPONDING.");
        }
        /* MARCH 23 2023*/

        IRSWorld.BillsPayment.OBPSTransactions.inquire_pagibig(member_id, this.MerchantCode, this.inquire_pagibig_api_callback);
    }

    this.inquire_pagibig_api_callback = function (response)
    {
        var api_content = null; pagibig_api_details = null;

        document.getElementById('tf1').value = "";
        document.getElementById('tf2').value = "";
        document.getElementById('tf3').value = "";

        if (response != null)
        {
            pagibig_api_details = response.value;
        }

        if (pagibig_api_details.indexOf("ERROR") >= 0)
        {
            document.getElementById('txtTargetAmount').disabled = true; hidePanelOB('divpopProcess'); alert(pagibig_api_details); return;
        }

        if (pagibig_api_details != null && pagibig_api_details != "" && pagibig_api_details.indexOf("OK") >= 0)
        {
            api_content = pagibig_api_details.split('|');
        }

        if (api_content != null && api_content.length > 0)
        {
            document.getElementById('tf1').value = api_content[1];
            document.getElementById('tf2').value = api_content[2];
            document.getElementById('tf3').value = api_content[3];

            xdeduction = api_content[8];
            xamount_due = api_content[9];
            xdue_date = api_content[10];

            if (api_content[12] != null && api_content[12] != "")
            {
                if (remteller.MerchantCode == 'PAG-IBIG HOUSING - PRN' || remteller.MerchantCode == 'PAG-IBIG HOUSING - HAN' && xamount_due != null && xamount_due != "" && xdue_date != null && xdue_date != "")
                {
                    alert(api_content[12] + "\n\n" + "Amount Due: " + xamount_due + "\n\n" + "Due Date: " + xdue_date);
                }
                else
                {
                    alert(api_content[12]);
                }
            }
        }

        hidePanelOB('divpopProcess');
    }

    this.validate_mp2_amt = function ()
    {
        var mp2_amt = document.getElementById('txtTargetAmount').value;
        var mp2_from = document.getElementById('df1').value;
        var mp2_to = document.getElementById('df2').value;

        var res = IRSWorld.BillsPayment.OBPSTransactions.validate_mp2_amt(mp2_amt, mp2_from, mp2_to).value;

        if (res == '')
        {
            return true;
        }
        else
        {
            this.error_input += res;
        }

        return false;
    }

    this.pagibig_hl_prn_paid = function ()
    {
        var housing_prn = document.getElementById("tf5").value;
        var res = IRSWorld.BillsPayment.OBPSTransactions.pagibig_housing_prn_paid(housing_prn).value;

        if (res == '')
        {
            return true;
        }
        else
        {
            this.error_input += res;
        }

        return false;
    }
    /* RFC-25832 PAG-IBIG API INTEGRATION */

    /* RFC-27195 GIFTAWAY */

    this.giftway_special_msg_length = function (dt, el) {
        var merchant_code = this.MerchantCode;

        if (merchant_code == "GIFTAWAY_APPAREL" ||
            merchant_code == "GIFTAWAY_ECOMMERCE" ||
            merchant_code == "GIFTAWAY_FOOD" ||
            merchant_code == "GIFTAWAY_GROCERY" ||
            merchant_code == "GIFTAWAY_HEALTH" ||
            merchant_code == "GIFTAWAY_PREPAID" ||
            merchant_code == "GIFTAWAY_SPECIALTY" ||
            merchant_code == "GIFTAWAY_TRAVEL") {
            if (dt == "Personal Message:") {
                el.maxLength = 300; return el;
            }
            else if (dt == "Recipients Mobile:") {
                el.maxLength = 11; return el;
            }
        }

        el.maxLength = 100; return el;
    }

    this.insert_giftway_btn = function () {
        document.getElementById('txtTargetAmount').disabled = true;
        document.getElementById('btnSave').disabled = true;

        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];

        var giftaway_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var giftaway_cell = null;
        var giftaway_el = null;

        giftaway_row.id = "tr_" + optional_mer_body.rows.length;

        giftaway_cell = giftaway_row.insertCell(0);
        giftaway_cell.id = "tdValidate";
        giftaway_cell.className = "cell_caption";

        giftaway_el = document.createElement("input");
        giftaway_el.type = 'button';
        giftaway_el.id = "btnValidate";
        giftaway_el.style.width = '50%';
        giftaway_el.style.display = 'inline';
        giftaway_el.value = "Validate";

        giftaway_el.onclick = function () {
            remteller.inquire_giftaway_api();
        }

        giftaway_cell.appendChild(giftaway_el);
    }

    this.inquire_giftaway_api = function () {
        AjaxPro.timeoutPeriod = 10000 * 1000;
        AjaxPro.onTimeout = function () {
            alert("SERVER NOT RESPONDING.");
        }

        showPanelOB('divpopProcess');

        recipient_name = document.getElementById("tf2").value;
        recipient_mobile = document.getElementById("tf3").value;
        recipient_email = document.getElementById("tf4").value;
        giftaway_special_msg = document.getElementById("tf5").value;

        giftaway_select = document.getElementById("tf1");
        merchant_id = giftaway_select.options[giftaway_select.selectedIndex].value;
        merchant_name = giftaway_select.options[giftaway_select.selectedIndex].text;

        giftaway_select = document.getElementById("tf6");
        merchant_deno = giftaway_select.options[giftaway_select.selectedIndex].value;

        giftaway_select = document.getElementById("tf7");
        merchant_qty = giftaway_select.options[giftaway_select.selectedIndex].value;

        AjaxPro.timeoutPeriod = 10000 * 1000;
        AjaxPro.onTimeout = function () {
            alert("SERVER NOT RESPONDING.");
        }

        IRSWorld.BillsPayment.OBPSTransactions.validate_giftway(merchant_id, merchant_name, merchant_deno, merchant_qty, recipient_name, recipient_mobile, recipient_email, giftaway_special_msg, this.inquire_giftway_api_callback);
    }

    this.inquire_giftway_api_callback = function (result) {
        var res = null;

        if (result != null && result.value != null) {
            if (result.value.indexOf("OK") >= 0) {
                document.getElementById('tf1').disabled = true;
                document.getElementById('tf2').disabled = true;
                document.getElementById('tf3').disabled = true;
                document.getElementById('tf4').disabled = true;
                document.getElementById('tf5').disabled = true;
                document.getElementById('tf6').disabled = true;
                document.getElementById('tf7').disabled = true;
                document.getElementById('btnValidate').disabled = true;
                document.getElementById('btnSave').disabled = false;

                res = result.value.split("~");

                if (res != null && res[1] != null) {
                    setText("txtTargetAmount", res[1]);
                    detectamount_keypress(true);
                    populate_rec();
                }

                return;
            }

            alert(result.value);
        }

        hidePanelOB('divpopProcess');
    }

    this.process_giftaway = function (reference_no)
    {
        var merchant_code = this.MerchantCode;
        var sender_name = getText("lbl_rem_fullname");
        var special_msg = null;

        if (merchant_code == "GIFTAWAY_APPAREL" ||
            merchant_code == "GIFTAWAY_ECOMMERCE" ||
            merchant_code == "GIFTAWAY_FOOD" ||
            merchant_code == "GIFTAWAY_GROCERY" ||
            merchant_code == "GIFTAWAY_HEALTH" ||
            merchant_code == "GIFTAWAY_PREPAID" ||
            merchant_code == "GIFTAWAY_SPECIALTY" ||
            merchant_code == "GIFTAWAY_TRAVEL") {

            AjaxPro.timeoutPeriod = 10000 * 1000;
            AjaxPro.onTimeout = function () {
                alert("SERVER NOT RESPONDING.");
            }

            special_msg = document.getElementById('tf5').value;

            IRSWorld.BillsPayment.OBPSTransactions.process_order_giftaway(reference_no, merchant_id, merchant_deno, merchant_qty, recipient_name, recipient_mobile, recipient_email, sender_name, special_msg, this.process_giftaway_callback);
        }
    }

    this.process_giftaway_callback = function (response) {
        if (response != null) {
        }
    }

    /* RFC-27195 GIFTAWAY */

    /* RFC-31110 - Maxicare Prepaid */

    this.insert_maxicareprepaid_btn = function () {
        document.getElementById('txtTargetAmount').disabled = true;
        document.getElementById('btnSave').disabled = true;

        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];

        var maxicareprepaid_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var maxicareprepaid_cell = null;
        var maxicareprepaid_el = null;

        maxicareprepaid_row.id = "tr_" + optional_mer_body.rows.length;

        maxicareprepaid_cell = maxicareprepaid_row.insertCell(0);
        maxicareprepaid_cell.id = "tdValidate";
        maxicareprepaid_cell.className = "cell_caption";

        maxicareprepaid_el = document.createElement("input");
        maxicareprepaid_el.type = 'button';
        maxicareprepaid_el.id = "btnValidate";
        maxicareprepaid_el.style.width = '50%';
        maxicareprepaid_el.style.display = 'inline';
        maxicareprepaid_el.value = "Validate";

        maxicareprepaid_el.onclick = function () {
            remteller.compute_maxicare_prepaid();
        }

        maxicareprepaid_cell.appendChild(maxicareprepaid_el);
    }

    this.compute_maxicare_prepaid = function ()
    {
        var mandatory_fields = get_data_value('TrnxDetails', false);

        if (mandatory_fields != null && mandatory_fields != "")
        {
            alert(mandatory_fields); return;
        }

        var maxi_prep_select = null;
        var maxi_prep_amt = null;
        var maxi_qty = null;

        maxi_prep_select = document.getElementById("tf4");
        maxi_prep_amt = maxi_prep_select.options[maxi_prep_select.selectedIndex].value;

        try
        {
            maxi_prep_select = document.getElementById("tf5");
            maxi_qty = maxi_prep_select.options[maxi_prep_select.selectedIndex].value;
            maxi_qty = parseInt(maxi_qty);
        }
        catch (e)
        {
            maxi_qty = document.getElementById("tf5").value;
            maxi_qty = parseInt(maxi_qty);
        }

        if (maxi_prep_amt != null && maxi_prep_amt != "" && maxi_qty != null && maxi_qty != "") {
            var amt = parseFloat(maxi_prep_amt.split('-')[0]);

            amt *= maxi_qty;

            setText("txtTargetAmount", amt);
            detectamount_keypress(true);
            populate_rec();

            document.getElementById('tf4').disabled = true;
            document.getElementById('tf5').disabled = true;
            document.getElementById('btnValidate').disabled = true;

            return;
        }

        alert("Please select Prepaid Health Card or Quantity.");
    }


    /* RFC-31110 - Maxicare Prepaid */

    this.clear_prn = function()
    {
        document.getElementById('tf1').value = "PRN";
        this.check_selected_sss_payment_type();
        document.getElementById('tf2').value = '';
        document.getElementById('tf3').value = '';
    }

    this.clear_pesofund = function ()
    {
        has_prereq = "yes";
        document.getElementById('nf1').value = "";
        document.getElementById('txtTargetAmount').value = "";
        document.getElementById('txtTargetAmount').readOnly = false;
        document.getElementById('txtTargetAmount').disabled = false;
        document.getElementById('nf1').disabled = false;
        document.getElementById('nf1').style.display = "none";
        document.getElementById('btnSave').disabled = true;    
    }

    this.validate_pesofund = function()
    {        
        if (document.getElementById('txtTargetAmount').value != '' || document.getElementById('txtTargetAmount').value != null)
        {
            var amount = document.getElementById('txtTargetAmount').value;
            IRSWorld.BillsPayment.OBPSTransactions.ValidatePesoFund(amount, this.validate_pesofund_callback);
        }
        else
        {
            alert('Please enter payment amount.');       
        }
    }

    this.validate_pesofund_callback = function (result)
    {
        document.getElementById('txtTargetAmount').disabled = true;

        if (result.value != null || result.value != 'undefined')
        {
            if (result.value == "valid")
            {
                has_prereq = "done";
                alert('Payment amount is valid. Proceed to send payment.');

                document.getElementById('nf1').value = document.getElementById('txtTargetAmount').value;
                document.getElementById('nf1').style.display = "";
                document.getElementById('nf1').disabled = true;
                document.getElementById("txtTargetAmount").value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(document.getElementById("txtTargetAmount").value).value;

                remteller.NettoBeneAmountPHP = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);
                populate_rec();

                document.getElementById('txtTargetAmount').readOnly = true;
                document.getElementById('txtTargetAmount').disabled = true;
                document.getElementById('btnSave').readOnly = false;
                document.getElementById('btnSave').disabled = false;
            }
            else if (result.value == "0") 
            {
                alert('Payment amount should be in multiples of P100.');
                remteller.clear_pesofund();
            }
            else
            {
                has_prereq = "yes";
                alert('Minimum payment amount is P' + result.value + '.');
                remteller.clear_pesofund();
            }
        }
    }

    this.validate_prn = function ()
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        var RTPFlag = IRSWorld.Internet.InternetPage.getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription").value;
        if (RTPFlag == "1") {
            if (document.getElementById('tf1').value == 'PRN') {
                setText(document.getElementById('lblLoadingMsg'), 'Validating PRN Request');
                var prnno = document.getElementById('tf2').value;
                var RemitterId = document.getElementById('lbl_rem_id').value;
                IRSWorld.BillsPayment.OBPSTransactions.InquireRTP("", prnno, "0.00", "BC_SSS", "", "", "VALIDATE", this.validate_rtp_callback);
            }
            else if (document.getElementById('tf1').value == 'SSSNO') {
                alert('Please enter PRN.');
                hidePanelOB('divpopProcess');
            }
            else {
                alert('Please enter PRN.');
                hidePanelOB('divpopProcess');
            }
        }
        else {
            if (document.getElementById('tf1').value == 'PRN') {
                setText(document.getElementById('lblLoadingMsg'), 'Validating PRN Request');
                var prnno = document.getElementById('tf2').value;
                IRSWorld.BillsPayment.OBPSTransactions.InquirePRN(prnno, this.validate_prn_callback);
            }
            else if (document.getElementById('tf1').value == 'SSSNO') {
                var sssno = document.getElementById('tf3').value;
                var birthday = document.getElementById('df3').value;
                setText(document.getElementById('lblLoadingMsg'), 'Validating SSS Number Request');
                IRSWorld.BillsPayment.OBPSTransactions.InquireSSSNo(sssno, birthday, this.validate_prn_callback);
            }
            else {
                alert('Please enter PRN or SSS Number');
                hidePanelOB('divpopProcess');
            }
        }
    }

    /*  DMND0002382 -Remittance SSS Enhanced e-Collection Service */
    this.validate_rtp_callback = function (result) {
        if (result.value != null || result.value != 'undefined') {
            var val = result.value.split('~');
            var SSSNo = IRSWorld.Internet.InternetPage.getLookupValue("RTPConfiguration", "FldCode", "SSS_DUMMY_DATA", "FldDescription").value;
            if (val.length > 0) {
                if (val[4] == "01") //resCode
                {
                    document.getElementById('nf1').value = val[3]; //amtDue
                    document.getElementById('df1').value = val[5]; //applDtFr
                    document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(val[3]).value; //amtDue
                    //document.getElementById('tf3').value = "NAXXXXXXXXX";
                    document.getElementById('tf3').value = SSSNo;

                    detectamount_keypress(true);
                    remteller.NettoBeneAmountPHP = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);
                    populate_rec();

                    document.getElementById('df1').value = convertToMMYYYY(val[5]); //applDtFr
                    document.getElementById('df2').value = convertToMMYYYY(val[6]); //applDtTo

                    document.getElementById('tf2').value = val[8]; //
                    document.getElementById('df1').readOnly = true;
                    document.getElementById('df2').readOnly = true;
                    document.getElementById('tf2').readOnly = true;
                    document.getElementById('tf3').readOnly = true;
                    document.getElementById('nf1').readOnly = true;
                    document.getElementById('txtTargetAmount').readOnly = true;
                    document.getElementById('df2').disabled = true;
                    document.getElementById('tf2').disabled = true;
                    document.getElementById('tf3').disabled = true;
                    document.getElementById('nf1').disabled = true;
                    document.getElementById('txtTargetAmount').readOnly = true;

                    document.getElementById('tr_7').style.display = '';
                    document.getElementById('tr_9').style.display = '';
                    document.getElementById('tr_10').style.display = '';
                    document.getElementById('tr_11').style.display = '';
                    prn_value = val[8]; //prn
                }
                else if (val[4] == "00") //resCode
                {
                    document.getElementById('nf1').value = val[3]; //amtDue
                    document.getElementById('df1').value = val[5]; //applDtFr
                    document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(val[3]).value; //amtDue
                    //document.getElementById('tf3').value = "NAXXXXXXXXX";
                    document.getElementById('tf3').value = SSSNo;

                    detectamount_keypress(true);
                    remteller.NettoBeneAmountPHP = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);
                    populate_rec();

                    document.getElementById('df1').value = convertToMMYYYY(val[5]); //applDtFr
                    document.getElementById('df2').value = convertToMMYYYY(val[6]); //applDtTo

                    document.getElementById('tf2').value = val[8]; //
                    document.getElementById('df1').readOnly = true;
                    document.getElementById('df2').readOnly = true;
                    document.getElementById('tf2').readOnly = true;
                    document.getElementById('tf3').readOnly = true;
                    document.getElementById('nf1').readOnly = true;
                    document.getElementById('txtTargetAmount').readOnly = true;
                    document.getElementById('df2').disabled = true;
                    document.getElementById('tf2').disabled = true;
                    document.getElementById('tf3').disabled = true;
                    document.getElementById('nf1').disabled = true;
                    document.getElementById('txtTargetAmount').readOnly = true;

                    document.getElementById('tr_7').style.display = '';
                    document.getElementById('tr_9').style.display = '';
                    document.getElementById('tr_10').style.display = '';
                    document.getElementById('tr_11').style.display = '';
                    prn_value = val[8]; //prn
                }
                else {
                    alert('SSS Response: ' + val[7] + ', cannot continue with the transaction');
                }
            }

            alert(result.value);
        }

        hidePanelOB('divpopProcess');
    }
	
    this.validate_prn_callback = function (result)
    {
        if (result.value != null || result.value != 'undefined')
        {
            var val = result.value.split('~');

            if (val.length > 0)
            {
                if (val[1] == "0")
                {
                    document.getElementById('nf1').value = val[6];
                    document.getElementById('df1').value = val[7];
                    document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(val[6]).value;
                    document.getElementById('tf3').value = val[3];

                    detectamount_keypress(true);
                    remteller.NettoBeneAmountPHP = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);
                    populate_rec();

                    if (val.length >= 9)
                    {
                        document.getElementById('df2').value = val[8];
                    }
                    else
                    {
                        document.getElementById('df2').value = val[7];
                    }

                    document.getElementById('tf2').value = val[2];
                    document.getElementById('df1').readOnly = true;
                    document.getElementById('df2').readOnly = true;
                    document.getElementById('tf2').readOnly = true;
                    document.getElementById('tf3').readOnly = true;
                    document.getElementById('nf1').readOnly = true;
                    document.getElementById('txtTargetAmount').readOnly = true;
                    document.getElementById('df2').disabled = true;
                    document.getElementById('tf2').disabled = true;
                    document.getElementById('tf3').disabled = true;
                    document.getElementById('nf1').disabled = true;
                    document.getElementById('txtTargetAmount').readOnly = true;
                    document.getElementById('tr_7').style.display = '';
                    document.getElementById('tr_9').style.display = '';
                    document.getElementById('tr_10').style.display = '';
                    document.getElementById('tr_11').style.display = '';
                    prn_value = val[2];
                }
                else
                {
                    alert('SSS Response: ' + val[0] + ', cannot continue with the transaction');
                }
            }

            alert(result.value);
        }

        hidePanelOB('divpopProcess');
    }

    this.insertTableRow = function ()
    {
        var theTable = document.getElementById("TrnxDetails");
        var theTableBody = theTable.tBodies[0];
        var sp = 'spGetOptionalField';
        var dt = IRSWorld.BillsPayment.OBPSTransactions.getOBPData(sp, this.MerchantCode, this.MerProdCode).value;

        if ((this.MerProdCode == 1 && this.MerchantCode=='SSS') || this.MerProdCode == 14)
        {
            document.getElementById('txtTargetAmount').value = '';
            document.getElementById('txtTargetAmount').disabled = 'disabled';
            document.getElementById('txtActualAmount').value = '0';
        }
        else
        {
            document.getElementById('txtTargetAmount').value = '';
            document.getElementById('txtTargetAmount').disabled = '';
            document.getElementById('txtActualAmount').value = '0';
        }

        if (dt != null && dt.Rows.length != 0)
        {
            var rowcnt = dt.Rows;
            var tfld = 0;
            var dfld = 0;
            var rfld = 0;
            var nfld = 0;
            var amfld = 0;

            for (var cnt = 0; cnt < dt.Rows.length; cnt++)
            {                
                var rowData = [dt.Rows[cnt].FieldLabel.toString(), dt.Rows[cnt].FieldName.toString()];
                var newCell;

                if (dt.Rows[cnt].FieldLabel.toString().indexOf('Account No') >= 0 && this.MerchantCode != 'Moldex RC')
                {
                    if (dt.Rows[cnt].FieldLabel.toString().indexOf('Account No') >= 0 && this.MerchantCode !='Landco Pacific Corp')
                    {
                        /* G. RFC-12313 SSS NOW IN USD 2021 */
                        if (remteller.is_sss_usd_merchant() == false)
                        {
                            continue;
                        }
                        /* G. RFC-12313 SSS NOW IN USD 2021 */;
                    }
                }

                if (dt.Rows[cnt].FieldName.toString() == "id1")
                {                    
                    document.getElementById("lbl_MID").innerHTML = "<a>" + dt.Rows[cnt].FieldLabel.toString() + "&nbsp;</a>";
                    document.getElementById("lbl_MID").style.fontSize = "11px";
                    document.getElementById("txtMemberID").title = dt.Rows[cnt].IsMandatory.toString();

                    if (dt.Rows[cnt].FieldLabel.toString() == "")
                    {
                        document.getElementById("trMembID").style.display = 'none';
                    }
                    else
                    {
                        document.getElementById("trMembID").style.display = '';
                    }

                    if (dt.Rows[cnt].FieldLabel.toString() == "Dragonpay Ref No")
                    {
                        document.getElementById("lblEnter").innerHTML = "Press enter to validate" + "&nbsp;";
                        document.getElementById("lblEnter").style.fontSize = "11px";
                        document.getElementById("lblEnter").style.display = "block";

                        var elem = document.getElementById('txtMemberID');

                        elem.onkeypress = function (e)
                        {
                            var ev;

                            if (window.event)
                            {
                                ev = event.keyCode;
                            }
                            else
                            {
                                ev = e.charCode;
                            }

                            if (ev == 13)
                            { //Enter keycode
                                var memid = document.getElementById("txtMemberID").value;

                                this.MerProdCode = memid;
                                this.MerchantCode = "Dragonpay";

                                var bool = IRSWorld.BillsPayment.OBPSTransactions.validate_dragonpay(memid, false).value;

                                if(bool == true || bool == "true")
                                {
                                    document.getElementById('btnSave').disabled = '';
                                    alert('Reference number is valid');
                                }
                                else
                                {
                                    alert('Invalid reference no');
                                    document.getElementById('btnSave').disabled = 'true';
                                }
                            }
                        }
                    }
                }
                else
                {
                    var newRow = theTableBody.insertRow(theTableBody.rows.length);
                    newRow.id = "tr_" + theTableBody.rows.length;

                    for (var i = 0; i < rowData.length; i++)
                    {
                        newCell = newRow.insertCell(i);

                        if (i == 0)
                        {                          
                            newCell.innerHTML = "<a>" + rowData[i] + "&nbsp;</a>";
                            newCell.id = rowData[i];
                            newCell.style.fontSize = "11px";
                            newCell.className = 'cell_caption';
                            newCell.style.textAlign = "right";
                            newCell.style.width = "30%";
                            newCell.style.height = "23px";
                        }
                        else
                        {
                            var mandatory = document.createElement('label');

                            mandatory.id = cnt;
                            mandatory.style.textAlign = 'left';
                            mandatory.style.color = 'Red';

                            setText(mandatory, '*');
                            mandatory.style.display = 'inline';

                            if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "tf")
                            {
                                tfld = tfld + 1;

                                if (dt.Rows[cnt].ValidationType.toString() == "LI")
                                {
                                    var el = document.createElement('select');

                                    el.name = dt.Rows[cnt].ControlName.toString();
                                    el.id = dt.Rows[cnt].FieldName.toString();
                                    el.style.width = '90%';
                                    el.style.display = 'inline';
                                    el.title = dt.Rows[cnt].IsMandatory.toString();

                                    newCell.appendChild(el);

                                    if (el.title == 'true')
                                    {                                    
                                        newCell.appendChild(mandatory);
                                    }

                                    this.LIFldRef = dt.Rows[cnt].LIFldREf.toString();
                                    this.popDDL(el, this.LIFldRef);

                                    if (this.MerProdCode == 14 && el.name == 'FoodPackage')
                                    {
                                        document.getElementById(el.id).onchange = Function('get_total_ssscontri("TrnxDetails");populate_rec();');
                                    }

                                    if (dt.Rows[cnt].FieldLabel.toString() == "No Of Months" && this.MerchantCode=='PhilHealth')
                                    {
                                        document.getElementById(el.id).onchange = Function('get_total_ssscontri("TrnxDetails");remteller.ComputeNumberOfMonths();populate_rec();');
                                    }

                                    if (dt.Rows[cnt].FieldLabel.toString() == "Search By" && this.MerchantCode == 'SSS')
                                    {
                                        if (document.getElementById('tf2') != null)
                                        {
                                            document.getElementById('tf2').value = '';
                                            document.getElementById('tf3').value = '';
                                        }

                                        this.MerProdCode = '100';
                                        document.getElementById(el.id).onchange = Function('remteller.check_selected_sss_payment_type();');
                                    }

                                    /* RFC-21138 RED RIBBON BY BEAM AND GO. ADDED A NEW ROUTINE TO AUTO TRIGGER COMPUTATION AND AUTO FILL AMOUNT BASED ON THE VALUE ON DROPDOWN IF IN OPTIONAL FIELD CONTAINS CONTROLNAME AUTOCOMPUTE_FEES_DROPDOWN_VALUE_FLDCODE*/
                                    if (this.MerchantCode != null && el.name != null)
                                    {
                                        if (el != null && el != "" && el.name.trim().toLocaleUpperCase() == "AUTOCOMPUTE_FEES_DROPDOWN_VALUE_FLDCODE")
                                        {
                                            document.getElementById('txtTargetAmount').disabled = true;

                                            remteller.dropdown_li_fieldanme = null;
                                            remteller.dropdown_li_fieldanme = el.id;

                                            document.getElementById(remteller.dropdown_li_fieldanme).onchange = function()
                                            {
                                                var dropdow_amt = document.getElementById(remteller.dropdown_li_fieldanme).value;

                                                if (dropdow_amt != null && dropdow_amt != "")
                                                {
                                                    var dropdown_actual_amt = dropdow_amt.split('-');

                                                    if (dropdown_actual_amt != null && dropdown_actual_amt.length > 0)
                                                    {
                                                        document.getElementById('txtTargetAmount').value = dropdown_actual_amt[0];
                                                        populate_rec();
                                                    }
                                                }
                                                else
                                                {
                                                    document.getElementById("btnSave").disabled = true;
                                                }
                                            }
                                        }
                                    }
                                    /* RFC-21138 RED RIBBON BY BEAM AND GO. ADDED A NEW ROUTINE TO AUTO TRIGGER COMPUTATION AND AUTO FILL AMOUNT BASED ON THE VALUE ON DROPDOWN IF IN OPTIONAL FIELD CONTAINS CONTROLNAME AUTOCOMPUTE_FEES_DROPDOWN_VALUE_FLDCODE*/

                                    /* BOYET BEAM AND GO PACKAGE JULY 1 2019 */
                                    if (this.MerchantCode != null && el.name != null)
                                    {
                                        if (this.MerchantCode.trim().toUpperCase() == "BMGP" && el.name.trim().toLocaleUpperCase() == "CTRLNAMEBEAMANDGOPACKAGE")
                                        {
                                            document.getElementById('txtTargetAmount').disabled = true;

                                            document.getElementById(el.id).onchange = function ()
                                            {
                                                var beam_amt = document.getElementById(el.id).value;

                                                if (beam_amt != null && beam_amt != "") {
                                                    var beam_actual_amt = beam_amt.split('-');

                                                    if (beam_actual_amt != null && beam_actual_amt.length > 0)
                                                    {
                                                        document.getElementById('txtTargetAmount').value = beam_actual_amt[0];
                                                        populate_rec();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    /* BOYET BEAM AND GO PACKAGE JULY 1 2019 */

                                }
                                else
                                {
                                    var el = document.createElement('input');

                                    el.type = 'text';
                                    el.name = dt.Rows[cnt].ControlName.toString();
                                    el.id = dt.Rows[cnt].FieldName.toString();
                                    el.style.width = '90%';
                                    el.style.display = 'inline';
                                    el.title = dt.Rows[cnt].IsMandatory.toString();
                                    
                                    if (dt.Rows[cnt].FieldLabel.toString() == "Reference No") 
				                    {
                                        el.maxLength = 16;
                                    }
                                    else
                                    {
                                        /* RFC-27195 GIFTAWAY ORIGINAL VALUE el.maxLength = 16 */
                                        el = this.giftway_special_msg_length(dt.Rows[cnt].FieldLabel.toString(), el);
                                        /* RFC-27195 GIFTAWAY */
                                    }

                                    newCell.appendChild(el);

                                    if (el.title == 'true')
                                    {
                                        mandatory.id = el.id;
                                        newCell.appendChild(mandatory);
                                    }

                                    if (dt.Rows[cnt].DataType.toString() == "N")
                                    {
                                        document.getElementById(el.id).onkeyup = Function('keyUP(this.keyCode);isNumeric_keycode(this.keyCode);');
                                        document.getElementById(el.id).onkeydown = Function('return isNumeric_keycode(event.keyCode)');
                                    }
                                }
                            }
                            else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "df")
                            {                                
                                var d = new Date();
                                var month = (d.getMonth() == '0') ? '1' : d.getMonth() + 1;
                                var tday = month + "/" + d.getDate() + "/" + d.getFullYear();

                                dfld = dfld + 1;

                                var el = document.createElement('input');

                                el.type = 'text';
                                el.name = dt.Rows[cnt].ControlName.toString();
                                el.id = dt.Rows[cnt].FieldName.toString();
                                el.readOnly = true;
                                el.style.width = '80%';
                                el.style.display = 'inline';
                                el.title = dt.Rows[cnt].IsMandatory.toString();

                                newCell.appendChild(el);

                                document.getElementById(el.id).value = tday;

                                var el2 = document.createElement('img');

                                el2.name = dt.Rows[cnt].ControlName.toString();
                                el2.id = 'dtim' + dfld;
                                el2.src = '../images/calendar/imgcalen.png';
                                el2.alt = 'Pick a date.'
                                el2.title = dt.Rows[cnt].IsMandatory.toString();
                                el2.style.display = 'inline';

                                newCell.appendChild(el2);

                                if (el2.title == 'true')
                                {                                
                                    newCell.appendChild(mandatory);
                                }

                                var popc = 'popUpCalendar(this,document.forms[0].' + el.id + ',"m/dd/yyyy",document.getElementById("divmain"))';

                                document.getElementById('dtim' + dfld).onclick = Function(popc);

                                if (this.MerchantCode=='SSS' & dt.Rows[cnt].ProductCode.toString()=='1')
                                {
                                    document.getElementById(el.id).onchange = Function('remteller.ComputeNumberOfMonths();');
                                }
                            }
                            else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "rf")
                            {
                                rfld = rfld + 1;

                                var el = document.createElement('textarea');

                                el.name = dt.Rows[cnt].ControlName.toString();
                                el.id = dt.Rows[cnt].FieldName.toString();
                                el.style.width = '90%';
                                el.title = dt.Rows[cnt].IsMandatory.toString();
                                el.maxlength = '100';
                                el.onkeyup = Function("this.value=this.value.replace(',','')");
                                el.onkeydown = Function("this.value=this.value.replace(',','')");

                                newCell.style.display = "inline";
                                newCell.appendChild(el);

                                if (el.title == 'true')
                                {                                
                                    newCell.appendChild(mandatory);
                                }
                            }
                            else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "nf")
                            {
                                nfld = nfld + 1;

                                if (dt.Rows[cnt].ValidationType.toString() == "LI")
                                {
                                    var ctrlname = dt.Rows[cnt].ControlName.toString();
                                    var el = document.createElement('select');

                                    el.name = dt.Rows[cnt].ControlName.toString();
                                    el.id = dt.Rows[cnt].FieldName.toString();
                                    el.style.width = 'auto';
                                    el.title = dt.Rows[cnt].IsMandatory.toString();
                                    el.style.display = 'inline';

                                    newCell.appendChild(el);

                                    this.LIFldRef = dt.Rows[cnt].LIFldREf.toString();

                                    if (this.MerchantCode=='SSS' & ctrlname=='MonthlyContribution')
                                    {
                                        ctrlname2 = dt.Rows[cnt].ControlName.toString();
                                        el.style.width = '90%';                                        
                                        
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onchange = Function('remteller.ComputeNumberOfMonths();');
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onload = Function('remteller.ComputeNumberOfMonths();');

                                        if (el.title == 'true')
                                        {
                                            newCell.appendChild(mandatory);
                                        }
                                    }
                                    else if (ctrlname=='NumberOfMonths')
                                    {
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onchange = Function('get_total_ssscontri("TrnxDetails");populate_rec();');
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onload = Function('get_total_ssscontri("TrnxDetails")');
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).disabled = true;
                                    }

                                    this.popDDL(el, this.LIFldRef);

                                    if (ctrlname=='NumberOfMonths')
                                    {
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).selectedIndex = 1;
                                    }
                                }
                                else
                                {
                                    var el = document.createElement('input');

                                    el.type = 'text';
                                    el.id = dt.Rows[cnt].FieldName.toString()
                                    el.name = dt.Rows[cnt].ControlName.toString();
                                    el.style.width = '90%';
                                    el.style.display = 'inline';
                                    el.title = dt.Rows[cnt].IsMandatory.toString();
                                    el.maxlength = '9';
                                    el.onkeyup = Function('keyUP(this.keyCode);isNumeric_keycode(this.keyCode);');

                                    newCell.appendChild(el);

                                    if (el.title == 'true')
                                    {
                                        newCell.appendChild(mandatory);
                                    }

                                    if (el.name=='ExcessPayment')
                                    {
                                        document.getElementById('nf3').value = 0;
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onkeyup = Function('get_total_ssscontri("TrnxDetails");populate_rec();keyUP(event.keyCode);');
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onkeydown = Function('return isNumeric_keycode(event.keyCode)');
                                        el.style.display = 'none';
                                        document.getElementById('Excess Payment').style.display = 'none';
                                        document.getElementById('nf3').maxlength = '9';
                                        document.getElementById(mandatory.id).style.display = 'none';
                                    }
                                    else
                                    {
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onkeyup = Function('keyUP(event.keyCode)');
                                        document.getElementById(dt.Rows[cnt].FieldName.toString()).onkeydown = Function('return isNumeric_keycode(event.keyCode)');
                                    }
                                }
                            }
                            else if (dt.Rows[cnt].FieldName.toString().substring(0,2) == "am")
                            {
                                amfld = amfld + 1;
                            }

                            newCell.style.height = "24px";
                            newCell.style.textAlign = "left";
                            newCell.style.width = "50%";
                        }
                    }
                }

                document.getElementById("tblBillsPayOBPSTransactionsMDetails").style.display = "";
            }

            document.getElementById("tblMerchantDetails").style.display = "";
        }

        if (this.MerchantCode == "SSS" && this.MerProdCode == "100")
        {
            this.newButton();
        }

        if (this.MerchantCode == "SSSPE" && this.MerProdCode == "99")
        {
            this.newSSSPEButton();
        }
		
        /* RFC-5305 SSS RTPL API CONNECTION */
		if (this.MerchantCode != null && this.MerchantCode.toUpperCase() == "LOAN PRN" && this.MerProdCode == "201")
		{
		    this.add_sss_rtpl_button();
		}
        /* RFC-5305 SSS RTPL API CONNECTION */

        /* BOYET. PARAMOUNT API INTEGRATION RFC-111 01-29-2020 */
        if (this.MerchantCode != null && this.MerchantCode.toUpperCase() == "PLGI")
        {
            this.add_paramount_button();
        }
        /* BOYET. PARAMOUNT API INTEGRATION RFC-111 01-29-2020 */

        /* C. RFC-12313 SSS CURRENCY IN USD */
        this.set_merchants_dynamic_code(this.MerchantCode, this.MerProdCode);
        /* C. RFC-12313 SSS CURRENCY IN USD */

        /* RFC-25832 PAG-IBIG API INTEGRATION */
        this.get_pagibig_api_details();
        /* RFC-25832 PAG-IBIG API INTEGRATION */
    }

    this.check_selected_sss_payment_type = function ()
    {
        if (document.getElementById('tf1').value == "PRN")
        {
            document.getElementById('tr_6').style.display = '';
            document.getElementById('tr_7').style.display = 'none';
            document.getElementById('tr_8').style.display = 'none';
            document.getElementById('tr_9').style.display = 'none';
            document.getElementById('tr_10').style.display = 'none';
            document.getElementById('tr_11').style.display = 'none';
        }
        else if (document.getElementById('tf1').value == "SSSNO")
        {
            document.getElementById('tr_6').style.display = 'none';
            document.getElementById('tr_7').style.display = '';
            document.getElementById('tr_8').style.display = '';
            document.getElementById('tr_9').style.display = 'none';
            document.getElementById('tr_10').style.display = 'none';
            document.getElementById('tr_11').style.display = 'none';
            
        }
    }

    this.ComputeNumberOfMonths = function() 
    {
        var date1 = '';
        var date2 = '';
        var jan = 1;
        var sep = 9;
        var oct = 10;
        var dec = 12;
        var d = new Date();
        var month = (d.getMonth() == '0') ? '1' : d.getMonth() + 1;

        this.generic = new generic_method();
        this.error_input = '';

        var foc_elem = '';

        if (this.MerchantType == 'Generic')
        {
        }
        else
        {
            if (this.fcontrolexist('TrnxDetails','df1') == true)
            {
                if (this.generic.is_empty(trim(document.getElementById("df1").value)) == true)
	            {
                    this.error_input += "Date From is mandatory.\n";
                    foc_elem = 'df1';
                }
	            else
	            {
                    date1 = document.getElementById("df1").value;
                }
            }

	        if (this.fcontrolexist('TrnxDetails','df2') == true)
	        {
	            if (this.generic.is_empty(trim(document.getElementById("df2").value)) == true)
	            {
                    this.error_input += "Date To is mandatory.\n";
                    foc_elem = 'df2';
                }
	            else
	            {
                    date2 = document.getElementById("df2").value;
                }
	        }

            if (date1 != '' & date2 != '')
            {
               if (this.MerchantCode == "PhilHealth")
               {
                    var count_month = parseInt(document.getElementById("tf3").value);
                    var check_month = parseInt(date1.split('/')[0]) + count_month;
                    var total_month = count_month + check_month;
                    var yr = parseInt(date1.split('/')[2])
                    var monthz = parseInt(date1.split('/')[0]);
                    var yearz = "";

                    if (parseInt(total_month) >=12)
                    {
                        yearz = parseInt(yr) + parseInt(check_month / 12);
                        
                        if (count_month == 6 && monthz > 6)
                        {
                            monthz = 6 - (12 - monthz);
                        }
                    }

                    var compute_month = total_month - parseInt(date2.split('/')[0]);

                    document.getElementById("df2").value = "";
                    document.getElementById("df2").value = monthz + "/" + date2.split('/')[1] + "/" + yearz;
                    date2 = monthz + "/" + date1.split('/')[1] + "/" + yearz;
               }

               if (date1.split('/')[2] <= date2.split('/')[2])
	           {
	                if (d.getFullYear() >= parseInt(date2.split('/')[2]) && d.getFullYear() > parseInt(date1.split('/')[2]))
	                {
                        if (month == jan)//(month >= jan && month <= sep)
                        {
                            if ((parseInt(date1.split('/')[0], 10) >= oct & parseInt(date1.split('/')[0], 10) <= dec))
                            {
                                var datedif = date2.split('/')[0] - date1.split('/')[0];

                                if (datedif == 0)
                                {
                                    datedif = 1;
                                }
                                else
                                {
                                    datedif = datedif + 1;
                                }

                                datedif = (parseInt(date2.split('/')[0], 10) + 12 * parseInt(date2.split('/')[2], 10)) - (parseInt(date1.split('/')[0], 10) + 12 * parseInt(date1.split('/')[2], 10));

                                if (datedif > 0)
                                {
                                    datedif = datedif + 1;
                                }

                                if (parseInt(date2.split('/')[0],10) <= parseInt(date1.split('/')[0],10))
                                {
                                    datedif = (parseInt(date2.split('/')[0], 10) + 12) - parseInt(date1.split('/')[0], 10);
                                    datedif = datedif + 1;
                                }

                                if (parseInt(date2.split('/')[2], 10) == parseInt(date1.split('/')[2], 10))
                                {
                                    if (datedif == 13)
                                    {
                                        datedif = 1;
                                    }
                                }

                                if (this.MerchantCode == "Philhealth")
                                {
                                    document.getElementById("tf3").value = datedif;
                                }
                                else
                                {
                                    document.getElementById("nf2").value = datedif;
                                }

                                get_total_ssscontri("TrnxDetails");
                                populate_rec();
                            }
	                        else
	                        {
                                alert('Invalid date period combination.');
                                document.getElementById("btnSave").disabled = true;
                            }
                        }
                        else if (parseInt(date1.split('/')[2],10) <= parseInt(date2.split('/')[2],10))
                        {
                            var yeardiff = date2.split('/')[2] - date1.split('/')[2];
                            var month_convert = yeardiff * 12;

                            var datedif = date2.split('/')[0] - date1.split('/')[0];

                            if (datedif == 0)
                            {
                                datedif = 1;
                            }
                            else
                            {
                                datedif = datedif + 1;
                            }

                            if (parseInt(date2.split('/')[0],10) < parseInt(date1.split('/')[0],10))
                            {
                                datedif = (parseInt(date2.split('/')[0], 10)) - parseInt(date1.split('/')[0], 10);
                                datedif = datedif + 1;
                            }

                            datedif = month_convert - datedif;

                            if (datedif < 0)
                            {
                                datedif = datedif * -1;
                            }

                            if (this.MerchantCode == "Philhealth")
                            {
                                document.getElementById("tf3").value = datedif;
                            }
                            else
                            {
                                document.getElementById("nf2").value = datedif;
                            }

                            get_total_ssscontri("TrnxDetails");
                            populate_rec();
                        }
                        else
                        {
                            alert('Invalid date period combination.');
                            document.getElementById("btnSave").disabled = true;
                        }
                    }
                    else if (d.getFullYear() < parseInt(date2.split('/')[2]))
                    {
	                    if (month >= oct && month <= dec)
	                    {
	                        if (parseInt(date1.split('/')[0],10) == parseInt(date2.split('/')[0],10))
                            {
                                if (parseInt(date1.split('/')[1],10) <= parseInt(date2.split('/')[1],10))
                                {
                                    var datedif = date2.split('/')[0] - date1.split('/')[0];
                                    datedif = (parseInt(date2.split('/')[0], 10) + 12 * parseInt(date2.split('/')[2], 10)) - (parseInt(date1.split('/')[0], 10) + 12 * parseInt(date1.split('/')[2], 10));

                                    if (datedif > 0)
                                    {
                                        datedif = datedif + 1;
                                    }

                                    var yeardiff = date2.split('/')[2] - date1.split('/')[2];
                                    var month_convert = yeardiff * 12;

                                    if (parseInt(date2.split('/')[0],10) <= parseInt(date1.split('/')[0],10))
                                    {
                                        datedif = (parseInt(date2.split('/')[0], 10) + month_convert) - parseInt(date1.split('/')[0], 10);
                                        datedif = datedif + 1;
                                    }

                                    if (this.MerchantCode == "Philhealth")
                                    {
                                        document.getElementById("tf3").value = datedif;
                                    }
                                    else
                                    {
                                        document.getElementById("nf2").value = datedif;
                                    }

                                    get_total_ssscontri("TrnxDetails");
                                    populate_rec();
                                }
                                else
                                {
                                    alert('Invalid date period combination.');
                                    document.getElementById("btnSave").disabled = true;
                                }
                            }
                            else if (parseInt(date1.split('/')[2],10) <= parseInt(date2.split('/')[2],10))
                            {
                                var yeardiff = date2.split('/')[2] - date1.split('/')[2];
                                var month_convert = yeardiff * 12;
                                var datedif = date2.split('/')[0] - date1.split('/')[0];

                                if (datedif == 0)
                                {
                                    datedif = 1;
                                }
                                else
                                {
                                    datedif = datedif + 1;
                                }

                                if (parseInt(date2.split('/')[0],10) < parseInt(date1.split('/')[0],10))
                                {
                                    datedif = (parseInt(date2.split('/')[0], 10)) - parseInt(date1.split('/')[0], 10);

                                    if (datedif >0)
                                    {
                                        datedif = datedif + 1;
                                    }
                                    else if (datedif <0)
                                    {
                                        datedif = (datedif * -1) - 1;
                                        datedif = month_convert - datedif;
                                    }
                                }
                                else
                                {
                                    //add month difference if year is not equal
                                    if(parseInt(date1.split('/')[2],10) < parseInt(date2.split('/')[2],10) && datedif>0)
                                    {
                                        datedif = month_convert + datedif;
                                    }
                                    else
                                    {
                                        datedif = month_convert - datedif;
                                    }
                                }

                                if (datedif < 0)
				                {
                                    datedif = datedif * -1;
                                }

                                if (this.MerchantCode == "Philhealth")
                                {
                                    document.getElementById("tf3").value = datedif;
                                }
                                else
                                {
                                    document.getElementById("nf2").value = datedif;
                                }

                                get_total_ssscontri("TrnxDetails");
                                populate_rec();
                            }
                            else if (parseInt(date1.split('/')[0],10) <= parseInt(date2.split('/')[0],10))
                            {
                                var datedif = date2.split('/')[0] - date1.split('/')[0];

                                if (datedif == 0)
                                {
                                    datedif = 1;
                                }
                                else
                                {
                                    datedif = datedif + 1;
                                }

                                if (this.MerchantCode == "Philhealth")
                                {
                                    document.getElementById("tf3").value = datedif;
                                }
                                else
                                {
                                    document.getElementById("nf2").value = datedif;
                                }

                                get_total_ssscontri("TrnxDetails");
                                populate_rec();
                            }
                            else
                            {
                                alert('Invalid date period combination.');
                                document.getElementById("btnSave").disabled = true;
                            }
                        }
	                    else if (parseInt(date1.split('/')[2],10) <= parseInt(date2.split('/')[2],10))
                        {
                            var yeardiff = date2.split('/')[2] - date1.split('/')[2];
                            var month_convert = yeardiff * 12;
                            var datedif = date2.split('/')[0] - date1.split('/')[0];
                            var dif = date2.split('/')[0] - date1.split('/')[0];                            
                            
                            datedif = (parseInt(date2.split('/')[0], 10) + 12 * parseInt(date2.split('/')[2], 10)) - (parseInt(date1.split('/')[0], 10) + 12 * parseInt(date1.split('/')[2], 10));

                            if (dif > 0)
                            {
                                datedif = datedif + 1;
                            }

                            if (this.MerchantCode == "Philhealth")
                            {
                                document.getElementById("tf3").value = datedif;
                            }
                            else
                            {
                                document.getElementById("nf2").value = datedif;
                            }

                            get_total_ssscontri("TrnxDetails");
                            populate_rec();
                        }
	                    else
	                    {
                            alert('Invalid date period combination.');
                            document.getElementById("btnSave").disabled = true;
                        }
                    }
                    else //Within the year
                    {                        
                        if (parseInt(date1.split('/')[0],10) == parseInt(date2.split('/')[0],10))
                        {
                            if (parseInt(date1.split('/')[1],10) <= parseInt(date2.split('/')[1],10) && parseInt(date1.split('/')[2],10) == parseInt(date2.split('/')[2],10))
                            {
                                var datedif = date2.split('/')[0] - date1.split('/')[0];

                                if (datedif == 0)
                                {
                                    datedif = 1;
                                }
                                else
                                {
                                    datedif = datedif + 1;
                                }

                                if (this.MerchantCode == "Philhealth")
                                {
                                    document.getElementById("tf3").value = datedif;
                                }
                                else
                                {
                                    document.getElementById("nf2").value = datedif;
                                }

                                get_total_ssscontri("TrnxDetails");
                                populate_rec();
                            }
                            else
                            {
                                alert('Invalid date period combination.');
                                document.getElementById("btnSave").disabled = true;
                            }
                        }
                        else if (parseInt(date1.split('/')[0],10) <= parseInt(date2.split('/')[0],10))
                        {
                            var datedif = date2.split('/')[0] - date1.split('/')[0];

                            if (datedif == 0)
                            {
                                datedif = 1;
                            }
                            else
                            {
                                datedif = datedif + 1;
                            }

                            if (this.MerchantCode == "Philhealth")
                            {
                                document.getElementById("tf3").value = datedif;
                            }
                            else
                            {
                                document.getElementById("nf2").value = datedif;
                            }

                            get_total_ssscontri("TrnxDetails");
                            populate_rec();
                        }
                        else if (parseInt(date1.split('/')[2],10) <= parseInt(date2.split('/')[2],10))
                        {
                            var yeardiff = date2.split('/')[2] - date1.split('/')[2];
                            var month_convert = yeardiff * 12;

                            var datedif = date2.split('/')[0] - date1.split('/')[0];

                            if (datedif == 0)
                            {
                                datedif = 1;
                            }
                            else
                            {
                                datedif = datedif + 1;
                            }

                            if (parseInt(date2.split('/')[0],10) < parseInt(date1.split('/')[0],10))
                            {
                                datedif = (parseInt(date2.split('/')[0], 10)) - parseInt(date1.split('/')[0], 10);
                                datedif = datedif + 1;
                            }

                            datedif = month_convert - datedif;

                            if (this.MerchantCode == "Philhealth")
                            {
                                document.getElementById("tf3").value = datedif;
                            }
                            else
                            {
                                document.getElementById("nf2").value = datedif;
                            }

                            get_total_ssscontri("TrnxDetails");
                            populate_rec();
                        }
                        else
                        {
                            alert('Invalid date period combination.');
                            document.getElementById("btnSave").disabled = true;
                        }
                    }
               }
	           else if (date1.split('/')[2] < date2.split('/')[2] && date1.split('/')[2] == d.getFullYear())
	           {                    
                    var dt1 = new Date(date1);
                    var dt2 = new Date(date2);
                    var datedif = (dt2.getMonth() + 12 * dt2.getFullYear()) - (dt1.getMonth() + 12 * dt1.getFullYear());

                    if (datedif == 0)
                    {
                        datedif = 1;
                    }
                    
                    if (this.MerchantCode == "Philhealth")
                    {
                        document.getElementById("tf3").value = datedif;
                    }
                    else
                    {
                        document.getElementById("nf2").value = datedif;
                    }

                    get_total_ssscontri("TrnxDetails");
                    populate_rec();
                }
	           else if (date1.split('/')[2] < date2.split('/')[2] && date1.split('/')[2] > d.getFullYear())
	           {
                    var dt1 = new Date(date1);
                    var dt2 = new Date(date2);
                    var datedif = (dt2.getMonth() + 12 * dt2.getFullYear()) - (dt1.getMonth() + 12 * dt1.getFullYear());

                    if (datedif == 0)
                    {
                        datedif = 1;
                    }
                    
                    if (this.MerchantCode == "Philhealth")
                    {
                        document.getElementById("tf3").value = datedif;
                    }
                    else
                    {
                        document.getElementById("nf2").value = datedif;
                    }

                    get_total_ssscontri("TrnxDetails");
                    populate_rec();
                }
	           else 
	           {
                    alert('Invalid date period combination.');
                    document.getElementById("btnSave").disabled = true;
               }
            }
        }
    }

    this.SaveTransaction = function()
    {
        //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
        this.Set_RemAccount_Details();

        if (has_prereq == "yes")
        {
            alert("You have pending prerequisites. Please complete the process to continue");
        }
        else
        {
            document.getElementById("btnSave").disabled = true;
            var validation = this.validate_input();

            if (validation == false)
            {
                alert(this.error_input); return;
            }

            validation = this.remittance_limit_return;

            if (validation == false)
            {
                return;
            }
        }
    }

    this.commit_payment = function(validation)
    {
        if (remteller.error_input.length > 0)
        {
            return;
        }

        //=========================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        //if (remteller.pending_ro_status == null)
        //{
        //    remteller.pending_ro_status = 0;
        //}
        if (remteller.pending_ro_status == null || remteller.pending_ro_status == 'null' || remteller.pending_ro_status == '0' || remteller.pending_ro_status == 0 || remteller.pending_ro_status == "") {
            remteller.pending_ro_status = 0;
        }
        //=========================================================================================

        var d = new Date();
        var curr_date = d.getDate();
        var curr_month = d.getMonth();

        curr_month++;
        curr_month = (curr_month.toString().length > 1) ? curr_month : "0" + curr_month;

        var curr_year = d.getFullYear();
        var curr_hour = d.getHours();
        var curr_min = d.getMinutes();
        var curr_sec = d.getSeconds();

        var formatted_date = curr_date + '' + curr_month + '' + curr_year + '' + curr_hour + '' + curr_min + '' + curr_sec;
        var datenow = dateToString(new Date());
        var calculator_values = IRSWorld.BillsPayment.OBPSTransactions.get_calculator_computed_values().value;
        var forex = calculator_values.split('~');

        if (calculator_values == "")
        {
            alert('Failed to get charges computation.');
            return;
        }

        /* PAOLO PAL 06-20-2019 */
        if (this.MerchantCode == "PAL Book and Hold" || this.MerchantCode == "PAL BOOK AND HOLD")
        {
            if (!this.validate_pal())
            {
                alert(this.error_input);
                return;
            }
        }
        /* PAOLO PAL 06-20-2019 */

        /* BOYET BEAM 06-10-2019 */
        if (this.MerchantCode == "BeamAndGo" || this.MerchantCode == "BEAMANDGO")
        {
            var beam_value = this.validate_beam_reference();

            if (beam_value.indexOf("ERROR") >= 0)
            {
                alert(beam_value); return;
            }
        }
        /* BOYET BEAM 06-10-2019 */
	
        /* PAOLO VISTA RESIDENCE 09-30-2019 */
        if (this.MerchantCode != null && this.MerchantCode.toUpperCase() == "VISTA RESIDENCES") 
        {
            if (!this.validate_so_number())
            {
                alert(this.error_input);
                return;
            }
        }
        /* PAOLO VISTA RESIDENCE 09-30-2019 */

        /* D. RFC-12313 SSS NOW IN USD 2021 */
        if (this.MerchantCode != null && this.MerchantCode.toUpperCase().trim() == "PRIMA CASA")
        {
            var r = IRSWorld.BillsPayment.OBPSTransactions.primacasa_so_valid(getText("tf1")).value;

            if (r != null && r != "")
            {
                alert(r); return;
            }
        }
        /* D. RFC-12313 SSS NOW IN USD 2021 */
	
        if (confirm('Are you sure you want to send payment?'))
        {
            var tranxstatus = '1';
            var dateOverseas = '';

            if (validation == false)
	        {
                alert(remteller.error_input); return;
            }
            else
            {
                if (document.getElementById("lblPayMode").value == '4')
                {
                    remteller.iscard_mode = 1;
                }
                else
                {
                    remteller.iscard_mode = 0;
                }

                if (document.getElementById("lblPayMode").value == '6')
                {
                    remteller.isdacct_mode = 1;
                }
                else
                {
                    remteller.isdaact_mode = 0;
                }

                remteller.ExtlReferenceNo = IRSWorld.BillsPayment.OBPSTransactions.generateOBPRefno(remteller.MerchantCode, remteller.MerProdCode).value;

                if (remteller.ExtlReferenceNo == '')
                {
                    remteller.ExtlReferenceNo = 'OBPS' + formatted_date;
                }

                if (parseInt(remteller.pending_ro_status) > 0)
                {
                    tranxstatus = parseInt(remteller.pending_ro_status);
                }

                if (remteller.fForApproval == 'true' && parseInt(remteller.pending_ro_status) == 0)
                {
                    tranxstatus = '15';
                }

                remteller.NetToBeneAmount = forex[2];
                dateOverseas = dateToString(IRSWorld.BillsPayment.OBPSTransactions.getlocaltime().value);

                if (IRSWorld.BillsPayment.OBPSTransactions.chkCutoffDate(dateToString(remteller.branch_cutend), dateOverseas).value == true)
                {
                    var dO = stringToDate(dateOverseas);
                    var dMonth = dO.getMonth();

                    dMonth++;

                    var dDay = null;
                    var dyear = dO.getFullYear();

                    if ((dyear % 4) == 0 && dMonth == 2 && dO.getDate() == 28)
                    {
                        dDay = dO.getDate() + 1;
                    }
                    else if (dMonth == 2 && dO.getDate() == 28)
                    {
                        dMonth++;
                        dDay = 1;
                    }
                    else if (dMonth == 2 && dO.getDate() == 29)
                    {
                        dMonth++;
                        dDay = 1;
                    }
                    else
                    {
                        dDay = dO.getDate() + 1;
                    }

                    var dhour = dO.getHours();
                    var dmin = dO.getMinutes();
                    var dsec = dO.getSeconds();
                    var dmsec = dO.getMilliseconds();

                    dateOverseas = dMonth + '/' + dDay + '/' + dyear + ' ' + dhour + ':' + dmin + ':' + dsec + '.' + dmsec;
                }

                var delimitted_data = this.MerchantCode + '!' + this.MerProdCode + '!';
                var tabid = document.getElementById('TrnxDetails');
                var tblrows = tabid.rows;

                if (tblrows == undefined) return;

                for (var i = 0; i < tblrows.length; i++)
                {
                    var sells = tblrows[i].cells;

                    for (var j = 0; j < sells.length; j++)
                    {
                        if (sells.item(j).hasChildNodes())
                        {
                            var childs = sells.item(j).childNodes;

                            for (var k=0; k < childs.length; k++)
                            {
                                if (childs[k].nodeName == 'INPUT')
                                {
                                    if (childs[k].display != 'none')
                                    {
                                        var label = (sells.item(0).id == '') ? getText(sells.item(0)) : sells.item(0).id;
                                        delimitted_data += this.trim(label) + '^' + childs[k].value.replace(',', ' ') + '!';
                                    }
                                }
                                else if (childs[k].nodeName == 'SELECT')
                                {
                                    if (childs[k].length > 0)
                                    {
                                        /* RFC-21138 RED RIBBON BY BEAM AND GO */
                                        if (remteller.dropdown_li_fieldanme != null && remteller.dropdown_li_fieldanme == "")
                                        {
                                            remteller.dropdown_li_fieldanme = null;
                                        }

                                        if (remteller.dropdown_li_fieldanme == null && this.trim(sells.item(0).id) == 'Food Package:' && document.getElementById("lbl_merchant_prodcode").innerHTML == 'Food Remittance') 
                                        {
                                            delimitted_data += this.trim(sells.item(0).id) + '^' + childs[k][childs[k].selectedIndex].text + '!';
                                        }
                                        else
                                        {
                                            delimitted_data += this.trim(sells.item(0).id) + '^' + childs[k].value.replace(',', ' ') + '!';
                                        }
                                        /* RFC-21138 RED RIBBON BY BEAM AND GO */

                                        /*
                                        COMMENTED
                                        if (this.trim(sells.item(0).id) == 'Food Package:' && document.getElementById("lbl_merchant_prodcode").innerHTML == 'Food Remittance')
                                        {
                                            delimitted_data += this.trim(sells.item(0).id) + '^' + childs[k][childs[k].selectedIndex].text + '!';
                                        }
                                        else
                                        {
                                            delimitted_data += this.trim(sells.item(0).id) + '^' + childs[k].value.replace(',', ' ') + '!';
                                        }
                                        */
                                    }
                                }
                                else if (childs[k].nodeName == 'TEXTAREA')
                                {
                                    delimitted_data += this.trim(sells.item(0).id) + '^' + childs[k].value.replace(',', ' ') + '!';
                                }
                                else if (childs[k].nodeName == 'TABLE')
                                {
                                    alert(set_read_only(childs[k], read_only)); // go deeper!
                                }
                            }
                        }
                    }
                }

                delimitted_data += 'Target Amount:' + new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value) + '!';

                if (this.EnabledRoundUp == '1')
                {
                    delimitted_data += 'Actual Amount:' + new common_method().to_remove_comma(document.getElementById("txtActualAmount").value) + '!';
                }

                /* J. RFC-12313 SSS NOW IN USD */
                if (this.is_sss_usd_merchant() == true)
                {
                    delimitted_data = delimitted_data.replace('tdValidate^Validate!', '')

                    if (forex != null && forex.length > 4)
                    {

                        forex[0] = parseFloat(forex[0]).toFixed(2);
                        forex[1] = parseFloat(forex[1]).toFixed(2);
                        forex[2] = parseFloat(forex[2]).toFixed(2);
                    }
                }
                /* J. RFC-12313 SSS NOW IN USD */

                tran_data = 'RemitMethod=' + remteller.RemitMethod + ',';
                tran_data += 'RemitStationID=' + '' + ',';
                tran_data += 'RemittingBranch=' + remteller.BranchCode + ',';
                tran_data += 'ChargeLocally=' + '0' + ',';
                tran_data += 'ReferenceNo=' + '' + ',';
                tran_data += 'ExtReferenceNo=' + remteller.ExtlReferenceNo + ',';
                tran_data += 'TransStatus=' + tranxstatus + ',';
                tran_data += 'TranxDate= ' + dateOverseas + ',';
                tran_data += 'ProductChargesID= ' + product_charges_id + ',';
                tran_data += 'RemitterCIFID= ' + remteller.RemitterID + ',';
                tran_data += 'BeneficiaryCIFID= ' + remteller.BeneficiaryCIFID + ',';
                tran_data += 'AccountSeqID= ' + remteller.AccountSEQID + ',';
                tran_data += 'RemittingCurrency= ' + document.getElementById('lblRemitCurr').value + ',';
                tran_data += 'GrossRemAmount= ' + forex[0] + ',';
                tran_data += 'NetRemAmt= ' + forex[1] + ',';
                tran_data += 'NetToBeneCurrency= ' + ((document.getElementById('ddMAccountCurrency').style.display == '') ? document.getElementById("ddMAccountCurrency").value + ',' : document.getElementById('lbl_merchant_currency').value + ',');
                tran_data += 'NetToBeneAmount= ' + forex[2] + ',';
                tran_data += 'TotalCharge= ' + forex[3] + ',';
                tran_data += 'TransactionSource= ' + document.getElementById('lblTranxSource').value + ',';
                tran_data += 'ReceiptMode= ' + document.getElementById('lblReceiptMode').value + ',';
                tran_data += 'TransactionPurpose= ' + document.getElementById('lblTranxPurpose').value + ',';
                tran_data += 'OtherPurpose= ' + document.getElementById('txtOthers').value + ',';
                tran_data += 'SpecialMessage= ' + delimitted_data + ',';
                tran_data += 'Printed= ' + 0 + ',';
                tran_data += 'ForexLocalToRemitting=' + forex[4] + ',';
                tran_data += 'ForexRemittingToUSD=' + forex[5] + ',';
                tran_data += 'ForexRemittingToPHP=' + forex[6] + ',';
                tran_data += 'ForexLocalToUSD=' + forex[7] + ',';
                tran_data += 'ForexLocalToPHP=' + forex[8] + ',';
                tran_data += 'ForexRemittingToAccount=' + forex[9] + ',';
                tran_data += 'SourceOfFunds=' + document.getElementById('lblSourceOfFunds').value + ',';
                tran_data += 'ForexRemittingToLocal=' + forex[10] + ',';
                tran_data += 'ForexRemittingToEUR=' + forex[11] + ',';
                tran_data += 'LocalCurrency=' + forex[12] + ',';
                tran_data += 'DatePostedOverseas=' + dateOverseas + ',';
                tran_data += 'DatePostedLocal=' + datenow + ',';
                tran_data += 'DateImplementedOverseas=' + dateOverseas + ',';
                tran_data += 'DateImplementedLocal=' + '' + ',';
                tran_data += 'total_backend_charges= ' + forex[13] + ',';

                //RFC-12313 COMMENTED tran_data += 'NettoBeneAmountPHP= ' + new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value) + ',';

                /* F. RFC-12313 SSS NOW IN USD 2021 */
                if (this.is_sss_usd_merchant() == true)
                {
                    tran_data += 'NettoBeneAmountPHP= ' + remteller.NettoBeneAmountPHP + ',';
                }
                else
                {
                    tran_data += 'NettoBeneAmountPHP= ' + new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value) + ',';
                }
                /* F. RFC-12313 SSS NOW IN USD 2021 */

                tran_data += 'RemitterAddressSeqID=' + remteller.RemitterAddressSEQID + ',';
                tran_data += 'RemitterContactSeqID=' + remteller.RemitterPhoneNumberSEQID + ',';
                tran_data += 'BeneficiaryAddressSeqID=0,';
                tran_data += 'BeneficiaryContactSeqID=0,';
                tran_data += 'NotificationMode=' + remteller.NotificationMode + ',';
                tran_data += 'RemAccountNo=' + getText(document.getElementById('txtRemAccountNo'));

                showPanelOB('divpopProcess');
                new common_method().to_show_loading_message('divLoadingMsg');
                setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
                trandata = tran_data;

                try
                {
                    /* E2EAPICHANGE 20220209 Removed submitOAAM */
                        var val = IRSWorld.BillsPayment.OBPSTransactions.saveOBPS(tran_data, remteller.saveOBPS_callback);
                }
                catch(err)
                {
                    var val = IRSWorld.BillsPayment.OBPSTransactions.saveOBPS(tran_data, remteller.saveOBPS_callback);
                }
            }
        }
    }

    /* E2EAPICHANGE 20220209 Removed submitOAAM_callback */

    this.saveOBPS_callback = function(param)
    {
        remteller.save_merchant_details('TrnxDetails');

        if (document.getElementById("lblPayMode").value == '2' || document.getElementById("lblPayMode").value == '4' || document.getElementById("lblPayMode").value == '6' || document.getElementById("lblPayMode").value == '12')
        {
            hidePanelOB('divpopProcess');
        }
    }

    this.save_merchant_details = function(tabid)
    {
        var delimitted_data = '';
        var tabid = document.getElementById(tabid);
        var tblrows = tabid.rows;

        if (tblrows == undefined) return;

        for (var i = 0; i < tblrows.length; i++)
        {
            var sells = tblrows[i].cells;

            for (var j = 0; j < sells.length; j++)
            {
                if (sells.item(j).hasChildNodes())
                {
                    var childs = sells.item(j).childNodes;

                    for (var k=0; k < childs.length; k++)
                    {
                        if (childs[k].nodeName == 'INPUT')
                        {
                            if (childs[k].display != 'none')
                            {
                                var label = (sells.item(0).id == '') ? getText(sells.item(0)) : sells.item(0).id;
                                delimitted_data += this.trim(label) + '=' + new common_method().to_remove_comma(childs[k].value) + ',';
                            }
                        }
                        else if (childs[k].nodeName == 'SELECT')
                        {
                            if (childs[k].length > 0)
                            {
                                /* RFC-21138 RED RIBBON BY BEAM AND GO */
                                if (remteller.dropdown_li_fieldanme != null && remteller.dropdown_li_fieldanme == "")
                                {
                                    remteller.dropdown_li_fieldanme = null;
                                }

                                if (remteller.dropdown_li_fieldanme == null && this.trim(sells.item(0).id) == 'Food Package:' && document.getElementById("lbl_merchant_prodcode").innerHTML == 'Food Remittance')
                                {
                                    delimitted_data += this.trim(sells.item(0).id) + '=' + childs[k][childs[k].selectedIndex].text + ',';                                    
                                }
                                else
                                {
                                    delimitted_data += this.trim(sells.item(0).id) + '=' + new common_method().to_remove_comma(childs[k].value) + ',';
                                }
                                /* RFC-21138 RED RIBBON BY BEAM AND GO */

                                /*
                                if (this.trim(sells.item(0).id) == 'Food Package:' && document.getElementById("lbl_merchant_prodcode").innerHTML == 'Food Remittance')
                                {
                                    delimitted_data += this.trim(sells.item(0).id) + '=' + childs[k][childs[k].selectedIndex].text + ',';
                                }
                                else
                                {
                                    delimitted_data += this.trim(sells.item(0).id) + '=' + new common_method().to_remove_comma(childs[k].value) + ',';
                                }
                                */
                            }
                        }
                        else if (childs[k].nodeName == 'TEXTAREA')
                        {
                            delimitted_data += this.trim(sells.item(0).id) + '=' + new common_method().to_remove_comma(childs[k].value) + ',';
                        }
                        else if (childs[k].nodeName == 'TABLE')
                        {
                            alert(set_read_only(childs[k], read_only)); // go deeper!
                        }
                    }
                }
            }
        }

        delimitted_data += 'Target Amount=' + new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value) + ',';

        if (this.EnabledRoundUp == '1')
        {
            delimitted_data += 'Actual Amount=' + new common_method().to_remove_comma(document.getElementById("txtActualAmount").value) + ',';
        }

        /* I. RFC-12313 SSS NOW IN USD */
        if (this.is_sss_usd_merchant() == true)
        {
            delimitted_data = delimitted_data.replace('tdValidate=Validate,', '')
        }
        /* I. RFC-12313 SSS NOW IN USD */

        var merdetails = IRSWorld.BillsPayment.OBPSTransactions.save_merch_details('', this.MerchantCode, this.MerProdCode, this.RemitterID, delimitted_data, this.MerchantSeqID);

        if (merdetails.value == true | merdetails.value == 'true')
        {
            if (document.getElementById("txtMemberID").disabled == false & document.getElementById("txtMemberID") != "")
            {
                IRSWorld.BillsPayment.OBPSTransactions.upDateMemberID(parseFloat(this.MerchantSeqID), this.BeneficiaryCIFID, this.RemitterID, document.getElementById("txtMemberID").value);
            }

            if (document.getElementById("lblPayMode").value == '2')
            {
                this.check_payment();
            }
            else if (document.getElementById("lblPayMode").value == '4')
            {
                remteller.debitcreditscreen.txn_seqid = IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value;
                remteller.debitcreditscreen.commit_transaction();
            }
            else if (document.getElementById("lblPayMode").value == '6')
            {
                this.debit_account_payment();
            }
            else if (document.getElementById("lblPayMode").value == '12')
            {
                this.ach_payment();
            }
            else
            {
                this.process_tranx(true);
            }
        }
        else
        {
            /* BOYET FEB 10 2019 */
            var merchant_det_response = IRSWorld.BillsPayment.OBPSTransactions.get_obp_transaction_response().value;
            /* BOYET FEB 10 2019 */

            alert('Failed to save transaction details.' + merchant_det_response);            
            window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
        }
    }

    this.remit_callback= function(param)
    {      
        if (param != null && param != undefined)
        {
            var ds = param.value;
            var message;
            var tranxstatus = ds.Tables[0].Rows[0].Transtatus;
            var tranxdesc = ds.Tables[0].Rows[0].TranStatusDesc;

            if (ds.Tables[0].Rows.length > 0)
            {
                if (tranxstatus!= null)
                {
                    this.Transtatus = tranxstatus;
                    this.TranStatusDesc = tranxdesc;
                }

                if (tranxdesc!= null )   
                {                     
                }

                /* E2EAPICHANGE 20220209 Removed updateOAAM */
                message = ds.Tables[0].Rows[0].Message;

                if (uRole == '25')
                {
                    alert('Status: ' + this.TranStatusDesc + '\r' + 'Message: ' + message);
                }
                else
                {
                    alert(message);
                }

                hidePanelOB('divpopProcess');
            }
            /* E2EAPICHANGE 20220209 Removed updateOAAM */

            if ((tranxstatus == 256 || tranxstatus == 9) && uRole == '25')
            {
                window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
            }
            else if ((remteller.MerchantType.toUpperCase() == 'BAYADCENTER' || remteller.MerchantCode.toUpperCase() == 'GENERIKA') && (tranxstatus == 256 || tranxstatus == 9))
            {
                window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
            }
            else if(uRole != '25' && uRole != '7')
            {            
                remteller.open_cash_management();
            }
            else
            {
                remteller.daily_record_keeping_log_report();
            }
        }
        else
        {
            alert('Server Problem. Could not process transaction.');
            hidePanelOB('divpopProcess');
        }
    }

    this.print_callback = function()
    {
        hidePanelOB('divpopProcess');
        document.js_closer = "obp_receipt_closeure";
        show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OBP&tranvalue=' + IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value, null, '100%', '100%', '0', null, null, true);
    }

    this.email_receipt = function()
    {
        var hasEmail = IRSWorld.TranClass.rem_other_details.chkCIFEmail(remteller.RemitterID).value;

        if (hasEmail != null && hasEmail == true)
        {
            if (confirm('Do you want to send transaction receipt thru email?'))
            {                
                IRSWorld.BillsPayment.OBPSTransactions.sendReceipt(remteller.RemitterID, remteller.send_or_callback);
                showPanelOB('divpopProcess');
                new common_method().to_show_loading_message('divLoadingMsg');
                setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
            }
            else
            {
                if(uRole == '10' || uRole == '7' || uRole == '25' || uRole == '27')
                {
                }
                else
                {
                    window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
                }
            }
        }
        else
        {
            if (uRole == '10' || uRole == '7' || uRole == '25' || uRole == '27')
            {
                if (document.getElementById('divTransactionHistoryModal').style.display == 'none')
                {
                    window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
                }
            }
            else
            {
                window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
            }
        }
    }

    this.send_or_callback = function(param)
    {
        hidePanelOB('divpopProcess');
        alert(param.value);
        window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
    }

    this.check_payment = function()
    {
        this.cheque_payments = new rem_modal_check_payments();
        this.cheque_payments.transaction_id = IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value;
        this.cheque_payments.open_check_payment_form("remteller.cheque_payments");
    }

    this.check_payment_result = function()
    {
        this.check_is_post_dated();
    }

    this.check_is_post_dated = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Unable to validate cheque date."); return;
        }

        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Validating cheque date ...');
        
        IRSWorld.BillsPayment.OBPSTransactions.check_is_post_dated(this.cheque_payments.clearing_date, this.check_is_post_dated_callback);
    }

    this.check_is_post_dated_callback = function(result)
    {
        if (remteller.pending_ro_status == null)
        {
            remteller.pending_ro_status = 0;
        }

        if (result != null && result.value != null)
        {
            if (result.value == true)
            {
                if (remteller.pending_ro_status != null && parseInt(remteller.pending_ro_status) == 0)
                {
                    remteller.fForApproval = 'false';
                    remteller.run_transaction_status_update('14');
                }
            }
        }

        hidePanelOB('divpopProcess');
        remteller.cheque_payments.commit_transaction();
    }

    this.check_payment_commit_result = function(param)
    {
        var s = param.split('~'); this.payment_error_msg = null;

        if (s != null)
        {
            if (parseInt(s[1]) == false)
            {
                this.payment_error_msg = "ERROR ENCOUNTERED UPDATING CHEQUE INFORMATION: " + s[1];
                remteller.process_tranx(false);
            }
            else
            {
                remteller.process_tranx(true);
            }
        }
    }

    this.card_payment = function()
    {
        this.card_payments = new rem_modal_card_payments();        
        this.card_payments.otc_card_payment = true;        
        this.card_payments.card_form(this.RemitterID, IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(1).value, "remteller.card_payments", document.getElementById('ddlSourceCurrency').value, document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].text, remteller);
    }

    this.card_payment_result = function()
    {
        this.card_payments.transaction_refno = IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value;
        this.card_payments.txn_seqid = this.card_payments.transaction_refno;
        this.card_payments.commit_transaction();
    }

    this.card_payment_commit_result = function(param)
    {
        var s = param.split('~'); this.payment_error_msg = null;

        if (s != null)
        {
            if (parseInt(s[0]) == 0)
            {
                this.payment_error_msg = "ERROR ENCOUNTERED UPDATING CARD INFORMATION: " + s[1];
                remteller.process_tranx(false);
            }
            else
            {
                remteller.process_tranx(true);
            }
        }
    }

    this.debit_account_payment = function()
    {
        this.debit_account_payments = new rem_modal_debit_account_payments();
        this.debit_account_payments.transaction_id = IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value;
        this.debit_account_payments.transaction_refno = IRSWorld.BillsPayment.OBPSTransactions.GetRefNo().value;
        this.debit_account_payments.debit_account_form(this.RemitterID, IRSWorld.BillsPayment.OBPSTransactions.NetToBeneAmount().value, "remteller.debit_account_payments");
    }

    this.debit_account_payment_result = function()
    {
        this.debit_account_payments.commit_transaction();
    }

    this.debit_account_payment_commit_result = function(param)
    {
        var s = param.split('~'); this.payment_error_msg = null;

        if (s != null)
        {
            if (parseInt(s[0]) == 0)
            {
                this.payment_error_msg = "ERROR ENCOUNTERED UPDATING DEBIT ACCOUNT INFORMATION: " + s[1];
                remteller.process_tranx(false);
            }
            else
            {
                remteller.process_tranx(true);
            }
        }
    }

    this.ach_payment = function()
    {
        this.ach_payments = new ach_payment();
        this.ach_payments.create_clearing_popup();
        this.ach_payments.transaction_id = IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value;
    }

    this.chk_ach_pay_result = function()
    {
        this.chk_ach_post_dated();
    }

    this.chk_ach_post_dated = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. ACH clearing date validation failed."); return;
        }

        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Checking ACH clearing date ...');
        IRSWorld.BillsPayment.OBPSTransactions.check_is_post_dated(this.ach_payments.clearing_date, this.chk_ach_post_dated_callback);
    }

    this.chk_ach_post_dated_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            if (result.value == true)
            {
                if (remteller.pending_ro_status != null && parseInt(remteller.pending_ro_status) == 0)
                {
                    remteller.run_transaction_status_update(14);
                }
            }
        }

        hidePanelOB('divpopProcess');
        remteller.ach_payments.commit_transaction();
    }

    this.ach_payment_commit_result = function(param)
    {
        this.payment_error_msg = null;

        if (param != null)
        {
            if (param == false)
            {
                this.payment_error_msg = "Error encountered saving ach payment";
                remteller.process_tranx(false);
            }
            else
            {
                remteller.process_tranx(true);
            }
        }
    }

    this.run_transaction_status_update = function(transaction_status)
    {
        if (this.pending_ro_status == null)
        {
            this.pending_ro_status = 0;
        }

        var data_row = new Array();
        this.Transtatus = transaction_status;
        data_row[data_row.length] = "Transtatus~" + transaction_status;
        data_row = data_row.join("|");

        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            hidePanelOB('divpopProcess');
            alert("Server not responding. Unable to modify otc transaction status."); return;
        }

        if (this.pending_ro_status != null && parseInt(this.pending_ro_status) == 0)
        {
            IRSWorld.TranClass.RemTranSaving.update_otc_transaction_status(data_row, IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value);
        }
    }

    this.process_tranx = function(val)
    {
        var tranxrefno = IRSWorld.BillsPayment.OBPSTransactions.GetRefNo().value;

        /* RFC-25832 PAG-IBIG API */
        remteller.obps_referenceno = tranxrefno;

        if (remteller.MerchantCode != null && remteller.MerchantCode == 'PAG-IBIG CALAMITY LOAN' || remteller.MerchantCode == 'PAG-IBIG CONTRIBUTION' || remteller.MerchantCode == 'PAG-IBIG HOUSING - PRN' || remteller.MerchantCode == 'PAG-IBIG HOUSING - HAN' || remteller.MerchantCode == 'PAG-IBIG MP2 SAVINGS' || remteller.MerchantCode == 'PAG-IBIG SHORT TERM LOAN')
        {
            var extl_ref = IRSWorld.BillsPayment.OBPSTransactions.generate_pagibig_api_ext_ref(remteller.obps_referenceno).value;
        }
        /* RFC-25832 PAG-IBIG API */

        if (this.MerchantCode == "SSS" && this.MerProdCode == '100')
        {
            if (!this.validate_sss(tranxrefno))
            {
            }
        }
		
		/* BOYET BEAM AND GO 06-10-2019*/
        if (this.MerchantCode == "BeamAndGo" || this.MerchantCode == "BEAMANDGO")
        {
            this.post_beam_and_go_reference();
        }
        /* BOYET BEAM AND GO 06-10-2019*/
		
        /* BOYET RFC-5305 SSS RTPL API CONNECTION 11-18/2019 */
        if (this.MerchantCode != null && this.MerchantCode.toUpperCase() == "LOAN PRN" && this.MerProdCode == '201')
        {
            this.post_sss_rtpl_prn(tranxrefno);
        }
        /* BOYET RFC-5305 SSS RTPL API CONNECTION 11-18/2019 */

        /* BOYET. PARAMOUNT API INTEGRATION. RFC-1111 01-29-2020 */
        if (this.MerchantCode != null && this.MerchantCode.toUpperCase() == "PLGI")
        {
            this.post_paramount_payment();
        }
        /* BOYET. PARAMOUNT API INTEGRATION. RFC-1111 01-29-2020 */

        /* H. RFC-12313 SSS NOW IN USD */
        if (this.is_sss_usd_merchant() == true)
        {
            if (this.MerchantCode.trim().toUpperCase() == "SSS CONTRIBUTION" || this.MerchantCode.trim().toUpperCase() == "SSS LOAN")
            {
                if (val == true)
                {
                    this.post_sss_loancontri_prn(tranxrefno, this.MerchantCode);
                }
            }
        }
        /* H. RFC-12313 SSS NOW IN USD */

        /* RFC-27195 GIFTAWAY API */
        this.process_giftaway(tranxrefno);
        /*RFC-27195  GIFTAWAY API */

        //=======================================================================================
        /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
        var obp_tranx_seq_id = IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value;
        IRSWorld.BillsPayment.OBPSTransactions.log_hitlist_details(obp_tranx_seq_id);
        //=======================================================================================

        if (val == true | val == 'true')
        {
            //=========================================================================================
            /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
            if (remteller.pending_ro_status != null && parseInt(remteller.pending_ro_status) > 0) {
                this.pending_ro_status = remteller.pending_ro_status;
            }
            else {
                this.pending_ro_status = 0;
            }
            //=========================================================================================

            setText(document.getElementById('lblLoadingMsg'), '');
            hidePanelOB("divpopProcess");

            if (remteller.fForApproval == 'false' || remteller.fForApproval == false) 
	        {
                if (remteller.MerchantType.toUpperCase() == 'BAYADCENTER') 
		        {
                    alert('Please wait your Overseas Bills Payment Transaction is being process. \rReference No: ' + tranxrefno);
                }
                else
                {
                    alert('Overseas Bills Payment Transaction has been successfully processed. \rReference No: ' + tranxrefno);
                }


                if(this.Transtatus  == '')
                {
                    this.Transtatus = 1;
                }


                IRSWorld.BillsPayment.OBPSTransactions.OBPRemit(remteller.RemitterID, remteller.AccountNo, remteller.NetToBeneAmount, this.Transtatus, 10, 2, document.getElementById('lblPayMode').value, document.getElementById('lblRemitCurr').value, ((document.getElementById('ddMAccountCurrency').style.display == '') ? document.getElementById("ddMAccountCurrency").value : document.getElementById('lbl_merchant_currency').value), remteller.remit_callback);
                if (document.getElementById('divpopProcess').style.display == 'none')
                {
                    showPanelOB('divpopProcess');
                    new common_method().to_show_loading_message('divLoadingMsg');
                    setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
                }
            }
            else
            {
                if (uRole == '10' || uRole == '7' || uRole == '25' || uRole == '27')
                {
                    if (IRSWorld.BillsPayment.OBPSTransactions.tranxstatusOK(tranxrefno).value == true)
                    {
                        if (remteller.MerchantType.toUpperCase() == 'BAYADCENTER')
                        {
                            alert('Please wait your Overseas Bills Payment Transaction is being process. \rReference No: ' + tranxrefno);
                        }
                        else
                        {
                            alert('Overseas Bills Payment Transaction has been successfully processed. \rReference No: ' + tranxrefno);
                        }

                        /* E2EAPICHANGE 20220209 Removed updateOAAM */
                        hidePanelOB('divpopProcess');

                        if (uRole == '10')
                        {
                            if (confirm('Do you want to view receipt?'))
                            {
                                oreceipt.online_recept_form(tranxrefno);
                            }
                            else
                            {
                                window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
                            }
                        }
                        else
                        {
                            remteller.daily_record_keeping_log_report();
                        }
                    }
                    else
                    {
                        alert('Overseas Bills Payment Transaction has failed to process. \rReference No: ' + tranxrefno);
                        /* E2EAPICHANGE 20220209 Removed updateOAAM */
                        window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
                    }
                }
                else
                {
                    //=========================================================================================
                    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
                    if (remteller.pending_ro_status != null && parseInt(remteller.pending_ro_status) > 0) {
                        alert('Overseas Bills Payment Transaction has been successfully processed. \rReference No: ' + tranxrefno + '\rTransaction Status: Pending Override');
                    }
                    else {
                        alert('Overseas Bills Payment Transaction has been successfully processed. \rReference No: ' + tranxrefno + '\rTransaction Status: For Approval');
                    }
                    //=========================================================================================
                    
                    /* E2EAPICHANGE 20220209 Removed updateOAAM */
					
                    if (document.getElementById('divpopProcess').style.display == '')
                    {
                        hidePanelOB('divpopProcess');
                    }
                    remteller.open_cash_management();
                }
            }
            //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
            this.Save_RemAccount_Details(tranxrefno);
        }
        else
        {
            if(remteller.payment_error_msg != '')
            {
                alert(remteller.payment_error_msg);
                window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
            }
            else
            {
                alert('Error occured while saving to database');
                window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
            }
        }
    }

    this.open_cash_management = function()
    {
        if (parseInt(remteller.allowed_cash_mgt) > 0)
        {
            remteller.cash_management = new rem_modal_denomination();
            remteller.cash_management.open_denomination_form(IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value, 'remteller.cash_management');
        }
        else
        {
            remteller.daily_record_keeping_log_report();
        }
    }

    this.denomination_commit_response = function(param)
    {
        var r = param.split("~");

        if (r != null)
        {
            if (parseInt(r[0]) == 0)
            {
                alert(r[1]);
            }
        }
        remteller.daily_record_keeping_log_report();
    }

    this.daily_record_keeping_log_report = function()
    {
        if (parseInt(remteller.got_limit) == 1)
        {
            var iconfirm = confirm("Would you like to print daily keeping log?");

            if (iconfirm == true)
            {
                document.js_closer = "otc_daily_log_closure"; show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OTCDAILY&tranvalue=' + IRSWorld.BillsPayment.OBPSTransactions.getTranxSeq_ID().value, null, '100%', '100%', '0', null, null, true); return;
            }
        }

        remteller.print_receipt();
    }

    this.print_receipt = function()
    {
        var hasEmail = IRSWorld.TranClass.rem_other_details.chkCIFEmail(remteller.RemitterID).value;

        if (confirm('Do you want to view receipt?'))
        {                            
            if (uRole != '10' && uRole != '7' && uRole != '25' && uRole != '27')
            {
                IRSWorld.BillsPayment.OBPSTransactions.printReceipt(remteller.RemitterID, remteller.print_callback);
                showPanelOB('divpopProcess');
                new common_method().to_show_loading_message('divLoadingMsg');
                setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
            }
            else
            {
                if (IRSWorld.BillsPayment.OBPSTransactions.isPNBReceipt(remteller.BranchCode).value == true)
                {
                    IRSWorld.BillsPayment.OBPSTransactions.printReceipt(remteller.RemitterID, remteller.print_callback);
                }

                oreceipt.online_recept_form(IRSWorld.BillsPayment.OBPSTransactions.GetRefNo().value);
            }
        }
        else
        {
            if (hasEmail != null && hasEmail == true)
            {
                if (confirm('Do you want to send transaction receipt thru email?'))
                {                
                    IRSWorld.BillsPayment.OBPSTransactions.sendReceipt(remteller.RemitterID, remteller.send_or_callback);
                    showPanelOB('divpopProcess');
                    new common_method().to_show_loading_message('divLoadingMsg');
                    setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
                }
                else
                {
                    window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
                }
            }
            else
            {
                window.open('../BillsPayment/OBPSTransactions.aspx', '_self');
            }
        }
    }

    this.removeTableRow = function()
    {        
        var theTable = document.getElementById("TrnxDetails");
        var theTableBody = theTable.tBodies[0];

        if (theTableBody.rows.length > 4)
        {
            var lastrow = theTableBody.rows.length;

            for (var i = 0; i < lastrow; i++)
            {
                if (theTableBody.rows.length != 4)
                {
                    theTableBody.deleteRow(theTableBody.rows.length - 1)
                }
            }
        }
    }

    this.LTrim = function( value ) 
    {	
        var re = /\s*((\S+\s*)*)/;
        return value.replace(re, "$1");
    }

    this.RTrim = function( value ) 
    {	
        var re = /((\s*\S+)*)\s*/;
        return value.replace(re, "$1");
    }

    this.trim = function( value ) 
    {	
        return this.LTrim(this.RTrim(value));
    }

    this.popRemDetail = function()
    {
        this.get_remitter_info();
    }

    this.remteller_vaporize = function()
    {       
        this.commons.to_clear_screen("tblTellerRemitter");
        this.commons.to_clear_screen("tblTellerRemitter");
    }

    this.getRemInfo = function()
    {     
        var mw = new modal_window();

        mw.open_modal_window();
        mw = null;
    }

    this.popDDL = function(DDL, REFID)
    {
        var SQL = this.getDDLSQL(DDL, REFID);
        var dt;
        var dt2;
        this.cleanDDL(DDL);

        if(DDL.id=='ddMAccountCurrency')
        {
            dt = IRSWorld.BillsPayment.OBPSTransactions.getCIFAccountCurrency(REFID).value;
        }
        else
        {
            dt = IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL).value;
            dt2 = IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL).value;
        }


        if (dt != null && dt.Rows.length > 0)
        {            
            for (var i = 0; i < dt.Rows.length; i++)
            {
                if (i == 0)
                {
                    var opt2 = document.createElement("option");
                    opt2.text = '';
                    opt2.value = '';
                    DDL.options.add(opt2);
                }
                var opt = document.createElement("option");
                opt.text = dt.Rows[i].Description.toString();
                opt.value = dt.Rows[i].Code.toString();
                DDL.options.add(opt);
            }
        }
    }

    this.cleanDDL = function(DDL)
    {        
        DDL.length = 0;
    }

    this.cleanDDL_2 = function(DDL)
    {
        var select = document.getElementById(DDL);
        select.options.length = 0;
    }

    this.getDDLSQL = function(DDL, REFID)
    {
        var SQL;
        var DDLid = DDL.id.substring(0, DDL.id.length - 1);

        switch(DDLid)
        {
            case 'txt':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            case 'ddSourceFund':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            case 'ddPayMod':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            case 'ddMProductCod':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            case 'ddRemCurrenc':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            case 'ddTrnxSourc':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            case 'ddReceiptMod':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            case 'ddTransactionPurpos':
                SQL = "sp_GetDDLSQL~" + DDLid + "~" + REFID;
                break;
            default:
                if (this.MerchantCode == 'SSS' & REFID == 'Contribution')
                {
                SQL = "sp_GetDDLSQL~"+ DDLid + "~" + REFID;
                }
                else if (this.MerchantCode == 'SSS' & REFID == 'NumberOfMonths')
                {
                SQL = "sp_GetDDLSQL~"+ DDLid + "~" + REFID;
                }
                else
                {
                SQL = "sp_GetDDLSQL~"+ DDLid + "~" + REFID;
                }
                break;
        }
        return SQL;
    }

    this.ShowTransaction = function()
    {
        document.getElementById('div_details').style.display = '';
    }
    
    this.HideTransaction = function()
    {
        document.getElementById('div_details').style.display = 'none';
    }
    
    this.ShowHistory = function()
    {
        if (getText(document.getElementById('lbl_rem_id')) != '')
        {
            this.LookUpMode = 'TRANXDETAILS';            
            this.callLookUp(this.LookUpMode); 
            document.getElementById('tdlblCharges').style.display = "none";
            document.getElementById('calc').style.display = "none";
            document.getElementById('trMerchant').style.display = "none";
            document.getElementById('trMerchantDetails').style.display = "none";
            document.getElementById('lblTranxHistory').style.display = "";
            document.getElementById('DIV_TRAN_DETAILS').style.display = "";
        }
        else
        {
            alert('Search a remitter first.');
        }
    }

    this.validate_sss = function (refno)
    {
        /*  DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        var RTPFlag = IRSWorld.Internet.InternetPage.getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription").value;
        var amtDue = document.getElementById('nf1').value;
        var applDtFr = document.getElementById('df1').value;
        var applDtTo = document.getElementById('df2').value;
        var instructionid = IRSWorld.BillsPayment.OBPSTransactions.getRTP_InstructionID(prn_value, "").value;
        if (RTPFlag == "1") {
            //string subscriberName, string subscriberNumber, decimal amount, string bc_code, string instructionId, string referenceno, string endpoint
            s = IRSWorld.BillsPayment.OBPSTransactions.InquireRTP("", prn_value, amtDue, "BC_SSS", instructionid, refno, "POST").value;
        }
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        else {
            s = IRSWorld.BillsPayment.OBPSTransactions.SubmitPRN(prn_value, refno).value;
        }

        if (s.length > 0) {
            var split_val = s.split('~');
            if (split_val[1] == "0") {
                return true;
            }
            else {
                return false;
            }
        }

        return false;
    }

    /* BOYET BEAM 06-10-2019 */
    this.validate_beam_reference = function ()
    {
        var beam_refno = document.getElementById("tf1").value;
        var transaction_amt = document.getElementById("txtTargetAmount").value;

        return IRSWorld.BillsPayment.OBPSTransactions.validate_beam_and_go_reference_number(beam_refno, transaction_amt).value;
    }

    this.post_beam_and_go_reference = function ()
    {
        var beam_refno = document.getElementById("tf1").value;
        var transaction_amt = document.getElementById("txtTargetAmount").value;
        var remarks = document.getElementById("tf2").value;
        var currency_code = this.MerAccountCurr;

        return IRSWorld.BillsPayment.OBPSTransactions.post_beam_and_go_reference_number(beam_refno, remarks, currency_code, transaction_amt, "0").value;
    }
    /* BOYET BEAM 06-10-2019 */

    this.validate_beam = function()
    {
        var amount = document.getElementById("txtTargetAmount").value;
        var refno = document.getElementById("tf1").value;
        var remarks = document.getElementById("tf2").value;
        return IRSWorld.BillsPayment.OBPSTransactions.PostTransactionBeam(refno, remarks, this.MerAccountCurr, amount).value;
    }
    
    /* PAL BOOK AND HOLD IMS-28268 - PAOLO 07-16-2019 */
    this.validate_pal = function()
    {
        var amount = document.getElementById("txtTargetAmount").value;
        var refno = document.getElementById("tf1").value;
        var currency = document.getElementById("lbl_merchant_currency").value;

        this.MerProdCode = 19;
        this.MerchantCode = "PAL Book and Hold";

        var strCheckdigit = IRSWorld.BillsPayment.OBPSTransactions.validate_checkdigit_pal(amount, refno, currency).value;

        if (strCheckdigit == "0")
        {
            document.getElementById('btnSave').disabled = '';
            return true;
        }
        else
        {
            this.error_input += strCheckdigit;
            return false;
        }
    }
    /* IMS-28268 - PAO 07-16-2019 */
    
    /*RFC-2676 - PAO VISTA RESIDENCES 10-04-2019*/
    this.validate_so_number = function () 
    {
        var so_num = document.getElementById("tf5").value;        
        
        if (so_num != null && so_num != "" && so_num.length == 10)
        {

            document.getElementById('btnSave').disabled = '';
            return true;
        }
        else
        {
            this.error_input = "Please input a 10-digit Sales Order (S.O.) Number";
            return false;
        }
    }
    /*RFC-2676 - PAO VISTA RESIDENCES 10-04-2019*/
    
    this.validate_input = function()
    {
        this.generic = new generic_method();
        this.error_input = '';
        var foc_elem = '';

        var target_amoount = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);

        /* E. RFC-12313 SSS NOW IN USD 2021 */
        if (this.is_sss_usd_merchant() == false)
        {
            if (this.NettoBeneAmountPHP != target_amoount)
            {
	            this.error_input += "PLEASE PRESS ENTER TO COMPUTE CHARGES \n";
	            foc_elem = 'txtTargetAmount';
	            document.getElementById(foc_elem).focus();
	        }
	    }
        /* E. RFC-12313 SSS NOW IN USD 2021 */

	    if (this.generic.is_empty(target_amoount) == true)
	    {
            this.error_input += "Target Amount is mandatory.\n";
            foc_elem = 'txtTargetAmount';
        }

        if (this.generic.is_numeric(target_amoount) == false || this.generic.is_positive(target_amoount) == false)
        {           
            this.error_input += "Target Amount is invalid.\n";
            foc_elem = 'txtTargetAmount';
        }

    	if(parseFloat(target_amoount) <= 0)
    	{
    	    this.error_input += "Target Amount is invalid.\n"; 
            foc_elem = 'txtTargetAmount'; 
    	}

        var date_from = new RegExp(/^\d{1,2}\/\d{1,2}\/\d{4}$/);
        var date_to = new RegExp(/^\d{1,2}\/\d{1,2}\/\d{4}$/);
        var txtdate_from = '';
        var txtdate_to = '';

        if (this.fcontrolexist('TrnxDetails','df1') == true)
        {
            txtdate_from = document.getElementById('df1').value;
        }

        if (this.fcontrolexist('TrnxDetails','df2') == true)
        {
            txtdate_to = document.getElementById('df2').value;
        }

        if (txtdate_from != null && txtdate_from != '' && txtdate_to != null && txtdate_to != '' )
        {
            this.date_from = txtdate_from.split('/');
            this.from_month = Number(this.date_from[0]);
            this.from_date = Number(this.date_from[1]) - 1;
            this.from_year = Number(this.date_from[2]);

            this.date_to = txtdate_to.split('/');
            this.to_month = Number(this.date_to[0]);
            this.to_date = Number(this.date_to[1]) - 1;
            this.to_year = Number(this.date_to[2]);

            this.Date_From = new Date();
            this.Date_To = new Date();
            this.Date_Current = new Date();

            this.current_month = this.Date_Current.getMonth() + 1;
            this.current_year = this.Date_Current.getFullYear();

            this.Date_From.setFullYear(this.from_year, this.from_month, this.from_date);
            this.Date_To.setFullYear(this.to_year, this.to_month, this.to_date);

            if (parseInt(this.date_from[2]) == parseInt(this.date_to[2]))
            {
                if (parseInt(this.date_to[0],10) < parseInt(this.date_from[0],10))
                {
                    this.error_input += "Period From should be less than Period To.\n";
                    foc_elem = 'df2';
                }
                else if(parseInt(this.date_to[0],10) == parseInt(this.date_from[0],10))
                {
                    if(parseInt(this.date_to[1],10) < parseInt(this.date_from[1],10))
                    {
                        this.error_input += "Period From should be less than Period To.\n";
                        foc_elem = 'df2';
                    }
                }
            }
            else if (parseInt(this.date_from[2]) < parseInt(this.date_to[2]))
            {
            }
            else if (parseInt(this.date_from[2]) > parseInt(this.date_to[2]))
            {
                this.error_input += "Period From should be less than Period To.\n";
                foc_elem = 'df2';
            }
        }

        this.error_input += get_data_value('TrnxDetails', false);

        if (this.error_input != '')
        {
            return false;
        }

        this.error_input += gettranxdetails();

        if (this.error_input != '')
        {
            showPanelOB('divTransactionDetails');
            return false;
        }

        //Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017
        if(this.IsRemitter_ACH == true && this.ACH_Branch_Setup == true)
        {
            if (this.Remitter_AccountType == null || this.Remitter_AccountType == "") { this.error_input = "Please input Remitter Account Type required for ACH payment transactions."; return false; }
            if (this.Remitter_AccountNo == null || this.Remitter_AccountNo == "") { this.error_input = "Please input Remitter Account Number required for ACH payment transactions."; return false; }
            if (this.Remitter_RoutingNo == null || this.Remitter_RoutingNo == "") { this.error_input = "Please input Remitter Routing Number required for ACH payment transactions."; return false; }
            if (this.Remitter_BankName == null || this.Remitter_BankName == "") { this.error_input = "Please input Remitter Bank Name required for ACH payment transactions."; return false; }
        }

        var isValidID = IRSWorld.BillsPayment.OBPSTransactions.ValidateMemberID((this.fcontrolexist('TrnxDetails', 'tf2') & this.MerchantCode == 'SSS' ? document.getElementById('tf2').value : document.getElementById('txtMemberID').value), this.MerchantCode, (this.fcontrolexist('TrnxDetails', 'tf1') & this.MerchantCode == 'PAGIBIG' ? document.getElementById('tf1').value : '')).value;

        /* RFC-25832 PAG-IBIG API */
        if (this.MerchantCode == 'PAG-IBIG MP2 SAVINGS' && remteller.validate_mp2_amt() == false)
        {
            return false;
        }

        if (this.MerchantCode == 'PAG-IBIG HOUSING - PRN' && remteller.pagibig_hl_prn_paid() == false)
        {
            return false;
        }

        if (this.MerchantCode != null && this.MerchantCode == 'PAG-IBIG CALAMITY LOAN' || this.MerchantCode == 'PAG-IBIG CONTRIBUTION' || this.MerchantCode == 'PAG-IBIG HOUSING - PRN' || this.MerchantCode == 'PAG-IBIG HOUSING - HAN' || this.MerchantCode == 'PAG-IBIG MP2 SAVINGS' || this.MerchantCode == 'PAG-IBIG SHORT TERM LOAN')
        {
            isValidID = true;
        }
        /* RFC-25832 PAG-IBIG API */

        if (isValidID == false)
        {
            if (this.MerchantCode == 'SSS')
            {
                this.error_input += 'Invalid ' + (this.fcontrolexist('TrnxDetails', 'tf2') & this.MerchantCode == 'SSS' ? 'Housing Loan AcctNo' : 'SS No.');
            }
            else if(this.MerchantCode == 'PAGIBIG')
            {
                this.error_input += 'Invalid Member ID for the selected Pay Type: ' + document.getElementById('tf1')[document.getElementById('tf1').selectedIndex].text;
            }
            else
            {
                this.error_input += getText(document.getElementById('lbl_MID')) + ' is not valid.'
            }

            return false;
        }

        if (uRole != '10')
        {
            if (remteller.checkidamount > 0)
            {
                if (Number(parseFloat(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(1).value.replace(",", "")).toFixed(2)) >= Number(parseFloat(remteller.checkidamount).toFixed(2)))
                {
                    if (confirm("Please present a valid ID for verification. Proceed?"))
                    {
                        if (uRole == '7' || uRole == '27' || uRole == '25')
                        {
                            if (remteller.fForApproval != 'true')
                            {
                                remteller.open_acct_inquiry_screen();
                            }
                            else
                            {                                
                                if (document.getElementById("lblPayMode").value == "1")
                                {
                                    if (remteller.remittance_cash_limit_tag == true)
                                    {
                                        remteller.RemitterCashAgg();
                                    }
                                    else
                                    {
                                        remteller.AggregateInternetRem();
                                    }
                                }
                                else
                                {
                                    remteller.AggregateInternetRem();
                                }
                            }
                        }
                        else
                        {
                            this.check_remittance_limit(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(0).value, IRSWorld.BillsPayment.OBPSTransactions.NetToBeneAmount().value);
                        }
                    }
                    else
                    {
                        document.getElementById("btnSave").disabled = true;
                        show_frame_dialog('../Internet/InternetRemitter.aspx?remitter_id=' + remteller.RemitterID + '&isRemit=otc', 'cifclose', '100%', '100%', '0');
                    }
                }
                else
                {
                    if (uRole == '7' || uRole == '27' || uRole == '25')
                    {
                        if (remteller.fForApproval != 'true')
                        {
                            remteller.open_acct_inquiry_screen();
                        }
                        else
                        {                            
                            if (document.getElementById("lblPayMode").value == "1")
                            {
                                if (remteller.remittance_cash_limit_tag == true)
                                {
                                    remteller.RemitterCashAgg();
                                }
                                else
                                {
                                    remteller.AggregateInternetRem();
                                }
                            }
                            else
                            {
                                remteller.AggregateInternetRem();
                            }
                        }
                    }
                    else
                    {
                        this.check_remittance_limit(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(0).value, IRSWorld.BillsPayment.OBPSTransactions.NetToBeneAmount().value);
                    }
                }
            }
            else
            {
                if (uRole == '7' || uRole == '27' || uRole == '25')
                {
                    if (remteller.fForApproval != 'true')
                    {
                        remteller.open_acct_inquiry_screen();
                    }
                    else
                    {
                        if (document.getElementById("lblPayMode").value == "1")
                        {
                            if (remteller.remittance_cash_limit_tag == true)
                            {
                                remteller.RemitterCashAgg();
                            }
                            else
                            {
                                remteller.AggregateInternetRem();
                            }
                        }
                        else
                        {
                            remteller.AggregateInternetRem();
                        }
                    }
                }
                else
                {
                    this.check_remittance_limit(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(0).value, IRSWorld.BillsPayment.OBPSTransactions.NetToBeneAmount().value);
                }
            }            
        }
        else
        {
            if (remteller.fForApproval != 'true')
            {
                remteller.open_acct_inquiry_screen();
            }
            else
            {
                remteller.commit_payment(remteller.remittance_limit_return);
            }
        }

        return true;
    }

    this.check_remittance_limit = function(remittance_amount, net_to_bene_amount)
    {
        var limits = new remittance_amt_limits();

        limits.remittance_amount_limit = this.remittance_amount_limit;
        limits.remittance_teller_limit = this.remittance_teller_limit;
        limits.remittance_supervisor_limit = this.remittance_superv_limit;
        limits.remittance_aggregate_tag = this.remittance_aggregate_tag;
        limits.remitter_id = this.RemitterID;
        limits.remitting_currency = document.getElementById('lblRemitCurr').value;
        limits.payment_type = document.getElementById("lblPayMode").value
        limits.cash_aggregation_enabled = this.remittance_cash_limit_tag
        limits.cash_aggregation_limit = this.remittance_cash_limit
        limits.check_amount_limit(remittance_amount.replace(',', ''), net_to_bene_amount.replace(',', ''), remteller);
    }

    this.remit_limit_response = function(param)
    {
        remteller.got_limit = 0;

        if (parseInt(param) > 0)
        {
            this.remittance_limit_return = true;

            /* FOR PENDING OVERRIDE */
            if (param != null && param instanceof Array)
            {
                if (param.length == 2)
                {
                    this.pending_ro_status  = parseInt(param[1]);
                    this.transaction_status = parseInt(param[1]);
                }

                if (param.length > 0)
                {
                    param = parseInt(param[0]);
                }
            }
            /* FOR PENDING OVERRIDE */

            if(parseInt(param) == 100)
            {
                remteller.got_limit = 1;
            }
            if (document.getElementById("lblPayMode").value == '4')
            {
                remteller.iscard_mode = 1;
                this.card_payment();
            }
            else
            {
                if(this.fForApproval != 'true')
                {
                    remteller.open_acct_inquiry_screen();
                }
                else
                {
                    remteller.commit_payment(remteller.remittance_limit_return);
                }
            }
        }
        else
        {
            remteller.remittance_limit_return = false;
        }
    }    

    this.open_acct_inquiry_screen = function()
    {
        IRSWorld.BillsPayment.OBPSTransactions.pnb_account_ok(remteller.MerAccountNumber, remteller.open_acct_inquiry_screen_callback);        
    }
    
    this.open_acct_inquiry_screen_callback = function(result)
    {
        AjaxPro.timeoutPeriod = 10000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("SERVER NOT RESPONDING.");
        }
        if (result != null && result.value == true)
        {
            IRSWorld.BillsPayment.OBPSTransactions.getAccountStatus(remteller.MerAccountNumber,remteller.accountStatus_callback);
            showPanelOB('divpopProcess');
            new common_method().to_show_loading_message('divLoadingMsg');
            setText(document.getElementById('lblLoadingMsg'), 'SYSTEM IS PROCESSING YOUR REQUEST...');
        }
        else
        {
            alert("NOT A PNB ACCOUNT.");
        }
    }
    
    this.accountStatus_callback = function(result)
    {
        if (result != null && result.value != '' && result.value != null)
        {
            var accstatus = result.value.split('|');

            try
            {
                /* E2EAPICHANGE 20220209 Updated Code */
                if (accstatus.length == 3)
                {
                    alert('Account Status is : ' + accstatus[2].toString());
                }
                else if (accstatus.length >= 8)
                {
                    if (accstatus[0].toString().toUpperCase() != '00')
                    {
                        alert('Account Status is : ' + accstatus[8].toString());
                    }
                }
                else
                {
                    alert('Account Status is : ' + accstatus[3].toString());
                }
            }
            catch(e)
            {

            }

            hidePanelOB('divpopProcess');
            
            if (uRole == '7' || uRole == '27' || uRole == '25')
            {
                if (document.getElementById("lblPayMode").value == "1")
                {
                    if (remteller.remittance_cash_limit_tag == true)
                    {
                        remteller.RemitterCashAgg();
                    }
                    else
                    {
                        remteller.AggregateInternetRem();
                    }
                }
                else
                {
                    remteller.AggregateInternetRem();
                }
            }
            else
            {
                remteller.commit_payment(remteller.remittance_limit_return);
            }
        }
        else
        {
            hidePanelOB('divpopProcess');

            if(result.error != null && result.error != '')
            {
                alert(result.error);
            }
            else
            {
                if (uRole == '7' || uRole == '27' || uRole == '25')
                {
                    if (document.getElementById("lblPayMode").value == "1")
                    {
                        if (remteller.remittance_cash_limit_tag == true)
                        {
                            remteller.RemitterCashAgg();
                        }
                        else
                        {
                            remteller.commit_payment(remteller.remittance_limit_return);
                        }
                    }
                    else
                    {
                        remteller.commit_payment(remteller.remittance_limit_return);
                    }
                }
                else
                {
                    remteller.commit_payment(remteller.remittance_limit_return);
                }
            }
        }
    }

    this.getcurrencycat = function()
    {
        var DDL = document.getElementById('ddlSourceCurrency');
        var dt = IRSWorld.BillsPayment.OBPSTransactions.getCurrencyCategories(this.BranchCode).value;

        if (dt != null && dt.Rows.length != 0)
        {
            for (var cnt = 0; cnt < dt.Rows.length; cnt++)
            {
                if (dt.Rows[cnt].used == true)
                {
                    var opt = document.createElement("option");

                    if (DDL.length == 0)
                    {
                        var opt2 = document.createElement("option");
                        opt2.text = '';
                        opt2.value = 0;
                        DDL.options.add(opt2);
                    }

                    opt.text = dt.Rows[cnt].fldcode.toString();
                    opt.value = dt.Rows[cnt].currency.toString();
                    DDL.options.add(opt);
                }
            }
        }
    }

    this.setremittingccy = function()
    {
        var i = document.getElementById('ddlSourceCurrency').selectedIndex;
        var ccy = document.getElementById('ddlSourceCurrency').options[i].text;
        var sourceccy = document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].value;

        if (ccy != "")
        {
            var fxrate = IRSWorld.BillsPayment.OBPSTransactions.getFXRate(this.BranchCode, sourceccy, document.getElementById('lbl_merchant_currency').value).value;

            if (fxrate != null && fxrate != '')
            {                
                if (uRole != '10' && uRole != '25' && uRole != '7' && uRole != '27')
                {
                    setText(document.getElementById('lblfxRate'), ccy + " 1 = " + getText(document.getElementById('lbl_merchant_currency')) + " " + fxrate);
                }
                else
                {
                    setText(document.getElementById('lblfxRate'), fxrate );
                }
            }
            else
            {
                alert('Please set Currency Rates first!');
            }
        }

        document.getElementById('lblRemitCurr').value = sourceccy;
        setText(document.getElementById('lblRemitCurr'), sidebar.lookup_desc('Currency',sourceccy));
        var blCharges = null;

        if (this.RemitterChargeType != '' && this.RemitterChargeType != '0' && document.getElementById('lblPayMode').value != undefined && document.getElementById('lblPayMode').value != '0')
        {
            blCharges = IRSWorld.BillsPayment.OBPSTransactions.chkDefaults_charges(this.BranchCode, sourceccy, this.RemitterChargeType, document.getElementById('lblPayMode').value).value;

            if (blCharges == false)
            {
                document.getElementById('lblPayMode').value = '0'
                setText(document.getElementById('lnkPaymode'), 'Please select');
            }
        }

        if (this.RemitterChargeType == '' || this.RemitterChargeType == '0' || document.getElementById('lblChargeType').value == '0')
        {
            alert('Please select Charge Type.');
            showPanelOB('divTransactionDetails');
        }
        else if (document.getElementById('lblPayMode').value == undefined || document.getElementById('lblPayMode').value == '0')
        {
            if (blCharges != null && (uRole == '10'))
            {
                alert('There is no charge maintained for the selected paymode. Please contact the nearest PNB Branch.');
            }
            else
            {
                alert('Please select Pay Mode.');
                showPanelOB('divTransactionDetails');
            }  
        }
        else
        {
            this.setproductchargesid(sourceccy, this.BranchCode);
            populate_rec();
        }
        
        //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
        this.paymenttype = document.getElementById('lblPayMode').value;
        this.Check_IsRemitter_ACH();
        this.Enable_RemAccount_Details();
    }
    
    this.waivecharges = function()
    {        
        var target_amt = new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value);
        var account_curr = document.getElementById('lbl_merchant_currency').value;
        
        hidePanelOB('divMerchantDetails');

        this.waiver_screen = new otc_waive_charges();
        this.waiver_screen.open_waiver_screen(parseInt(this.ProductChargesID), parseInt(account_curr), parseFloat(account_curr), "OBPSCalculator");        
    }
    
    this.open_override_window = function()
    {
        var RemID = getText(document.getElementById('lbl_rem_id'));

        if (uRole == '7' || uRole == '27' || uRole == '25')
        {
            if (RemID != '')
            {
                hidePanelOB('divMerchantDetails');
                this.obps_override_cls = new override_calculator_obps();
                this.obps_override_cls.open_override_form("OBPSCalculator");            
            }
            else
            {
                alert('Search a remitter first.');
            }
        }
        else
        {
            if (RemID != '')
            {
                hidePanelOB('divMerchantDetails');
                this.obps_override_cls = new override_calculator();
                this.obps_override_cls.open_override_form("OBPSCalculator");            
            }
            else
            {
                alert('Search a remitter first.');
            }
        }
    }
    
    this.compute_override = function(param)
    {
        if (param != null)
        {
            this.override = param;
            this.override.close_modal_window();
        }

        var amount_rem = new common_method().to_remove_comma(document.getElementById("txtTargetAmount").value);

        if (new common_method().is_decimal(amount_rem) == false)
        {
            alert("AMOUNT ENTERED (" + amount_rem + ") IS INVALID!\n CLICK OK TO CORRECT AMOUNT." ); amount_rem = 0;
        }
        else
        {            
            var display = IRSWorld.BillsPayment.OBPSTransactions.compute_override(parseInt(this.product_charge_id), parseInt(this.account_currency), parseFloat(amount_rem)).value;
            document.getElementById('calc').innerHTML = display;
            this.getchargesdet();
          
            /* PENDING ROV */
            if (remteller.override != null && remteller.override.pending_override_status != null && remteller.override.pending_override_status != "" && parseInt(remteller.override.pending_override_status) > 0)
            {
                remteller.pending_ro_status = parseInt(remteller.override.pending_override_status);
                remteller.transaction_status = parseInt(remteller.override.pending_override_status);
            }
            /* PENDING ROV */
        }
    }

    this.getchargesdet = function()
    {
        var chargetable = document.getElementById('calc');
        var ntom = getText(chargetable).substring(getText(chargetable).indexOf("NET TO MERCHANT - (A/C CCY.)") + 28);
        var netamount = getText(chargetable).substring(getText(chargetable).indexOf("TOTAL AMOUNT - (RMT CCY.)") + 25, getText(chargetable).indexOf("NET RMT AMOUNT"));

        if (getText(chargetable).indexOf("TOTAL CHARGE - (RMT CCY.)") >=0 )
        {
            var tCharge = getText(chargetable).substring(getText(chargetable).indexOf("TOTAL CHARGE - (RMT CCY.)") + 25,getText(chargetable).indexOf("TOTAL CHARGE - (LOCAL CCY.)"));
        }
        else
        {
            var tCharge = getText(chargetable).substring(getText(chargetable).indexOf("TOTAL CHARGE - (SPL CCY.)") + 25,getText(chargetable).indexOf("TOTAL CHARGE - (LOCAL CCY.)"));
        }

        document.getElementById('spnBenefAmount').innerHTML = ntom.substring(0,3) + ' ' + IRSWorld.BillsPayment.OBPSTransactions.NetToBeneAmount().value;
        document.getElementById('spnNetAmount').innerHTML = netamount.substring(0,3) + ' ' + IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(1).value;
        document.getElementById('spnTotalCharges').innerHTML = tCharge.substring(0,3) + ' ' + IRSWorld.BillsPayment.OBPSTransactions.TotalChargesAmount().value;
    }
    
    this.fcontrolexist = function(tabid, crtlid)
    {
        var startIndex = 0;
        var tab = document.getElementById(tabid);
        var tblrows = tab.rows;
        if (tblrows == undefined) {return false};        
        for (var i = startIndex; i < tblrows.length; i++)
        {
            var cells = tblrows[i].cells;
            for (var j = startIndex; j < cells.length; j++)
            {
                if (cells.item(j).hasChildNodes())
                {
                    var childs = cells.item(j).childNodes;
                    for (var k = startIndex; k < childs.length; k++)
                    {
                        if (childs[k].id == crtlid)
                        {
                            return true;
                        }
                    }
                }
            }            
        }
    }

    this.posttransaction = function()
    {
        document.getElementById("tdbtnSave").style.display = "none";
        document.getElementById("tdbtnCancel").style.display = "none";
        document.getElementById("btnOverrideCharges").style.display = "none";
        document.getElementById("btnWaive").style.display = "none";
        document.getElementById("Button1").disabled = true;
        document.getElementById("Button2").disabled = true;
        document.getElementById("ddlSourceCurrency").disabled = true;
        sidebar.get_side_bar_visibility("divtdBillsPayOBPSTransactionsMenu", "btnAnchor");

        if (document.getElementById("btnSourceOfFunds") != null)
        {
            setText(document.getElementById("lblSourceOfFunds"), getText(document.getElementById("btnSourceOfFunds")));
            setText(document.getElementById("lblPayMode"), getText(document.getElementById("lnkPaymode")));
            setText(document.getElementById("lblReceiptMode"), getText(document.getElementById("lnkReceiptMode")));
            setText(document.getElementById("lblTranxSource"), getText(document.getElementById("lnkTranxSource")));
            setText(document.getElementById("lblTranxPurpose"), getText(document.getElementById("lnkTranxPurpose")));
        }
    }

    this.load_settings = function()
    {
        if (this.Exceptions != null && (parseInt(this.Exceptions) > 0 || this.Exceptions == 'true'))
        {
            IRSWorld.TranClass.RemTranExtra.remitter_exceptions(this.RemitterID, this.check_exceptions_callback);
        }        
    }
    
    this.check_exceptions_callback = function(result)
    {
        if (result != null && result.value != null && parseInt(result.value) > 0)
        {
            remteller.open_exception_screen();
        }        
    }

    this.open_exception_screen = function()
    {
        this.exception_screen = new rem_exceptions();
        this.exception_screen.remitter_id = this.RemitterID;
        this.exception_screen.open_remitter_exceptions();
    }
    
    this.exception_screen_result = function(param)
    {
        if (param != null)
        {
            if (parseInt(param) == 0)
            {                
                window.open('../BillsPayment/OBPSTransactions.aspx','_self');
            }
        }
    }

    this.chkRatesComplete = function()
    {
        var isComplete = IRSWorld.BillsPayment.OBPSTransactions.bRatesComplete(this.BranchCode).value
        
        if (isComplete == false || isComplete == 'false')
        {
            alert('CURRENCY RATES IS INCOMPLETE.');
            document.getElementById('Button2').disabled = true;
        }
        else
        {
            document.getElementById('Button2').disabled = false;
        }
    }
    
    this.remote_override_response = function(param)
    {
        if (param != null)
        {
            if (param.length == 4)
            {
                if (param[0] != null && parseInt(param[0]) == 2)
                {
                    if (param[1] != null && parseInt(param[1]) == 1 && this.waiver_screen != null)
                    {
                        document.getElementById("txt_otc_rem_waived_user_id").value = param[2];
                        document.getElementById("txt_otc_rem_waived_user_password").value = param[3];

                        this.waiver_screen.check_user_rights();
                    }
                    else if (param[1] != null && parseInt(param[1]) == 2 && this.override != null)
                    {
                        document.getElementById("txt_username").value = param[2];
                        document.getElementById("txt_password").value = param[3];
                                            
                        this.override.override_execute();                        
                    }
                }
                else if (param[0] != null && parseInt(param[0]) == 3)
                {
                    if (param[1] != null && parseInt(param[1]) == 1 && this.waiver_screen != null)
                    {
                        this.waiver_screen.close_modal_window();
                    }
                    else if (param[1] != null && parseInt(param[1]) == 2 && this.override != null)
                    {
                        this.override.close_modal_window();
                    }
                }
            }
        }        
    }

    this.getTodayForex = function()
    {
        AjaxPro.timeoutPeriod = 10000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("SERVER NOT RESPONDING.");
        }

        IRSWorld.BillsPayment.OBPSTransactions.getTodayForex(remteller.getTodayForex_callback);
    }
    
    this.getTodayForex_callback = function(result)
    {
        var response = result.value;
        var forexValue;
        var tr1 = "<tr><td style='width:70%; background-color:#c02f2f; font-size:10px; font-family: Arial; height:1px; text-align:left;color:white;'>";
        var tr2 = "<tr><td style='width:70%; font-size:10px; font-family: Arial; height:1px; text-align:left; color:white;'>";
        var tr3 = "</td><td style='width:30%; background-color:#c02f2f; font-size:10px; font-family: Arial; height:1px; text-align:left;color:white;'>";
        var tr4 = "</tr><td style='width:30%; font-size:10px; font-family: Arial; height:1px; text-align:left; color:white;'>";
        var tr5 = "</td></tr>";
        var forexRate;

        if (response != null && response != "" && response != undefined)
        {
            setText(document.getElementById("spnForexDate"),response.Rows[0]["TranxDate"]);
            forexValue = "<table style='width:210px'>";

            for (var i = 0; i <= (response.Rows.length - 1); i++)
            {
                forexRate = String(response.Rows[i]["SourceToTarget"]);

                if (forexRate.indexOf('.') < 0)
                {
                    forexRate = forexRate + ".0000";
                }

                if (i % 2 == 0)
                {
                    forexValue = forexValue + tr1 + response.Rows[i]["Forex"] + tr3 + forexRate + tr5;
                }
                else
                {
                    forexValue = forexValue + tr2 + response.Rows[i]["Forex"] + tr4 + forexRate + tr5;
                }
            }

            forexValue = forexValue + "</table>";
            setText(document.getElementById("spnForexValue"),forexValue);
        }
    }
    
    this.getRemAccountNo = function()
    {
        IRSWorld.BillsPayment.OBPSTransactions.getRemitterAccountNo(remteller.SEQ_ID,remteller.getRemAccountNo_callback)
    }

    this.getRemAccountNo_callback = function(param)
    {
        if (param.value != null || param.value != '')
        {
            setText(document.getElementById("txtRemAccountNo"),param.value);
        }
    }
    
    this.is_debit_success = function(issuccess, icrd) //Added by Troy 20120809
    {
        remteller.debitcreditscreen = icrd;

        if (issuccess == true)
        {
            remteller.commit_payment(remteller.remittance_limit_return);
        }
        else
        {
            alert("Debit from client card was unsuccessful. Please try again.");
            return 0;
        }
    }
    
    this.close_msg_prompt = function()
    {
        hidePanelOB('divpopProcess');
    }

    this.open_msg_prompt = function(msg)
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), msg);
    }

    this.hasTranxDetailHistory = function()
    {
        IRSWorld.BillsPayment.OBPSTransactions.hasTranxDetailsHistory(remteller.MerchantCode, remteller.MerProdCode, remteller.RemitterID, remteller.hasTranxDetailHistory_callback);
    }

    this.hasTranxDetailHistory_callback = function(param)
    {
        var result = param.value;

        if (result == true || result == 'true')
        {
            if (confirm('Do you want to use the previous transaction detail for this transaction?'))
            {
                remteller.getTranxDetailHistory();
            }
        }
    }

    this.getTranxDetailHistory = function()
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Retrieving previous transaction details.');
        IRSWorld.BillsPayment.OBPSTransactions.TranxDetailsHistory(remteller.MerchantCode, remteller.MerProdCode, remteller.RemitterID, remteller.getTranxDetailHistory_callback);
    }

    this.getTranxDetailHistory_callback = function(param)
    {
        hidePanelOB('divpopProcess');
        var dt = param.value;

        if (dt != null && dt != 'null' && dt != undefined && dt != 'undefined')
        {
            if (dt.Rows.length > 0)
            {
                for (var i = 0; i < dt.Rows.length; i++)
                {
                    document.getElementById(dt.Rows[i].FieldName.toString()).value = dt.Rows[i].FieldValue.toString()
                }
            }
        }
    }

    this.RemitterCashAgg = function()
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Verifying Cash Transaction Limit.');
        IRSWorld.BillsPayment.OBPSTransactions.RemitterCashAgg(remteller.RemitterID, document.getElementById('lblRemitCurr').value, IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(0).value, remteller.RemitterCashAgg_callback);
    }

    this.RemitterCashAgg_callback = function(result)
    {
        hidePanelOB('divpopProcess');
        var response = result.value;

        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            if (parseFloat(response.Rows[0].GrossCashAmount) + parseFloat(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(0).value) > parseFloat(remteller.remittance_cash_limit))
            {                
                remteller.remittance_limit_return = false;
                alert("Aggregate cash transaction now ( " + (parseFloat(response.Rows[0].GrossCashAmount) + parseFloat(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(0).value)) + " ) and exceeds limit.");
            }
            else
            {
                remteller.AggregateInternetRem();
            }
        }
        else
        {
            remteller.AggregateInternetRem();
        }
    }

    this.AggregateInternetRem = function()
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Verifying Transaction Limit.');
        IRSWorld.BillsPayment.OBPSTransactions.checkInternetRemittanceAggregateLimit(remteller.RemitterID, document.getElementById('lblRemitCurr').value, IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(0).value, remteller.AggregateInternetRem_callback);
    }

    this.AggregateInternetRem_callback = function(result)
    {
        hidePanelOB('divpopProcess');

        if (result.value == true)
        {
            remteller.remittance_limit_return = true;

            if (Number(parseFloat(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(1).value.replace(",", "")).toFixed(2)) >= Number(parseFloat(remteller.agentlogreport).toFixed(2)))
            {
                remteller.got_limit = 1;
            }
            
            remteller.checkAgentSystemControl();
        }
        else
        {
            if (remteller.agentrejects == true)
            {
                alert('Amount exceeds maximum remittance aggregate limit of ' + document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].text + ' ' + IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(remteller.remittance_amount_limit.toString()).value);
                window.open('../BillsPayment/OBPSTransactions.aspx','_self');
            }
            else
            {
                if (confirm("Amount exceeds remittance maximum limit. Remitter must provide latest Bank Statement and/or withdrawal receipt and proof of fund."))
                {
                    remteller.remittance_limit_return = true;

                    if(Number(parseFloat(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(1).value.replace(",", "")).toFixed(2)) >= Number(parseFloat(remteller.agentlogreport).toFixed(2)))
                    {
                        remteller.got_limit = 1;
                    }
                    
                    remteller.checkAgentSystemControl();
                }
                else
                {
                    window.open('../BillsPayment/OBPSTransactions.aspx','_self');
                }
            }
        }
    }
    
    this.checkAgentSystemControl = function()
    {
        if (remteller.agentsysctrl == true)
        {
            if (Number(parseFloat(IRSWorld.BillsPayment.OBPSTransactions.GrossAmount(1).value.replace(",", "")).toFixed(2)) >= Number(parseFloat(remteller.agentlogreport).toFixed(2)))
            {
                agentRem.loadSystemControl();
            }
            else
            {
                remteller.commit_payment(remteller.remittance_limit_return);
            }
        }
        else
        {
            remteller.commit_payment(remteller.remittance_limit_return);
        }
    }
    
    this.checkSysCrtl_RemitterInfo = function()
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Checking Required Remitter Info.....');         
        IRSWorld.BillsPayment.OBPSTransactions.checkSystemControl_RemitterInfo(remteller.RemitterID,'', remteller.checkSysCrtl_RemitterInfo_callback);
    }
    
    this.checkSysCrtl_RemitterInfo_callback = function(result)
    {
        var response = result.value;

        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            agentRem._RemitterInfoComplete = response;
            remteller.checkSysCrtl_RemitterIDs();
        }
        else
        {
            remteller.checkSysCrtl_RemitterIDs();
        }
    }

    this.checkSysCrtl_RemitterIDs = function()
    {
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Checking Required Remitter IDs.....');        
        IRSWorld.BillsPayment.OBPSTransactions.checkSystemControl_RemitterIDs(remteller.RemitterID,remteller.checkSysCrtl_RemitterIDs_callback);
    }
    
    this.remitter_ids_checker = function(response)
    {
        var retobj =
        {
            noIDs: 0,
            expireIDs: 0,
            idmessage_noid: "",
            idmessage_exp: ""
        }

        var remids = response.split("~");

        for (var i = 0; i < (remids.length - 1); i++)
        {
            if (remids[i].length == 0) continue;

            if (remids[i].split("_")[0] == "0")
            {
                if (retobj.noIDs == 0)
                {
                    retobj.idmessage_noid += "Please add the following IDs: \r\n";
                }

                retobj.idmessage_noid += remids[i].split("_")[1] + "\r\n";
                retobj.noIDs += 1;
            }
            else if (remids[i].split("_")[0] == "2")
            {
                if (retobj.expireIDs == 0)
                {
                    retobj.idmessage_exp += "Please update the expired IDs:\r\n\r\n";
                }

                var exobj = remids[i].split("_")[1].split('|');

                retobj.idmessage_exp += exobj[0] + "\r\n" + exobj[1] + "\r\n" + exobj[2] + "\r\n\r\n";
                retobj.expireIDs += 1;
            }
        }

        if (retobj.noIDs == 0 && retobj.expireIDs == 0)
        {
            agentRem._RemitterIDsComplete = true;
        }
        else
        {
            agentRem._RemitterIDsComplete = false;
        }

        return retobj;
    }

    this.checkSysCrtl_RemitterIDs_callback = function(result)
    {
        var response = result.value;
        
        if (response.length > 5 && response.substr(0, 6) == "Error:")
        {
            alert(response);
            return "";
        }        
        
        var idmessage = "";        
        
        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            var retobj = remteller.remitter_ids_checker(response);
            idmessage = retobj.idmessage_noid + retobj.idmessage_exp;
        }

        hidePanelOB('divpopProcess');

        if (agentRem._RemitterIDsComplete == true && agentRem._RemitterInfoComplete == true)
        {            
            hidePanelOB('divRemitterInfo');
            remteller.isRemitterIDs = false;
            remteller.commit_payment(remteller.remittance_limit_return);
        }
        else
        {
            agentRem._RemitterIDsMessage = idmessage;            
            agentRem.getRemitterInfo(remteller.RemitterID,'');
            remteller.isRemitterIDs = true;
            remteller.LookUpMode = 'REMITTER_ID';

            this.grid = new grid_viewer();
            this.grid.sql_source = remteller.getLookUpSQL();
            this.grid.div_container = document.getElementById('divGridIds'); page_setup
            this.grid.datarow_column = 0;
            this.grid.page_size = 5;
            this.grid.grd_header_back_color = "#26afe5";
            this.grid.grd_header_fore_color = "#FFFFFF";
            this.grid.grd_customed_date = false;
            this.grid.process_ajax("ajx_loading");
            this.grid = null;

            showPanelOB('divRemitterInfo');
        }
    }
    
    this.close_session = function()
    {
        IRSWorld.BillsPayment.OBPSTransactions.clear_session();
    }
    
    this.load_rem_states = function()
    {
        var rem_loc = new rem_states();

        rem_loc.remitter_address_seq_id = remteller.RemitterAddressSEQID;
        rem_loc.method_to_call = function(){ remteller.check_remitter_contact();};
        rem_loc.chk_rem_states();
    }
    
    //===========================================================================================
    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
    this.get_hit_list_all = function (remitter_id, beneficiary_id) {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function () {
            alert("Server not responding. Checking customer information hit list failed."); return;
        }
        var isWatchlist = IRSWorld.BillsPayment.OBPSTransactions.got_client_hit_list_all_obp(remitter_id, beneficiary_id).value;

        if (isWatchlist != null && isWatchlist == true)
        {
            remteller.the_mode = 11;
            remteller.hit_stream_lining = new generic_override();
            remteller.hit_stream_lining.remitter_id = remitter_id;
            remteller.hit_stream_lining.beneficiary_id = beneficiary_id;
            remteller.hit_stream_lining.open_client_hit_list_all();
            return true;
        }
        return false;
    }

    this.check_client_pending_ovr = function () {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function () {
            alert("check_client_pending_ovr. failed"); return;
        }
        IRSWorld.BillsPayment.OBPSTransactions.check_cif_enrollment_pending_ovr_obp(this.RemitterID, "", this.check_client_pending_ovr_callback);
    }

    this.check_client_pending_ovr_callback = function (result) {
        var hit_list = null;

        if (result != null && result.value != null) {
            hit_list = result.value.split("~");
        }

        if (hit_list != null && hit_list.length > 0) {
            var msg = "CIF enrollment with pending override.";

            alert(msg);
        }
        else {
            remteller.get_hit_list_all(this.RemitterID, this.BeneficiaryCIFID);
        }
    }

    /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    this.get_hit_list_all_refinitiv = function (remitter_id, beneficiary_id) {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        
        AjaxPro.onTimeout = function () {
            alert("Server not responding. Checking customer information hit list failed."); return;
        }
        var isWatchlist = IRSWorld.BillsPayment.OBPSTransactions.got_client_hit_list_all_obp_refinitiv(remitter_id, beneficiary_id).value;
        //if (isWatchlist != null && isWatchlist == true) {
        //    remteller.the_mode = 11;
        //    remteller.hit_stream_lining = new generic_override();
        //    remteller.hit_stream_lining.remitter_id = remitter_id;
        //    remteller.hit_stream_lining.beneficiary_id = beneficiary_id;
        //    remteller.hit_stream_lining.isOBPscreenTransaction = 1;
        //    remteller.hit_stream_lining.open_client_hit_list_all();
        //    return true;
        //}
        if (isWatchlist != null && isWatchlist == true) {
            var refinitivisdownsession = IRSWorld.TranClass.Refinitiv.get_refinitiv_is_down_OTC().value;
            if (refinitivisdownsession == "Refinitiv is down.") {
                remteller.the_mode = 11;
                remteller.hit_stream_lining = new generic_override();
                remteller.hit_stream_lining.remitter_id = remitter_id;
                remteller.hit_stream_lining.beneficiary_id = beneficiary_id;
                remteller.hit_stream_lining.isOBPscreenTransaction = 1;
                remteller.hit_stream_lining.open_client_hit_list_all_refinitivisdown();
            }
            else {
                remteller.the_mode = 11;
                remteller.hit_stream_lining = new generic_override();
                remteller.hit_stream_lining.remitter_id = remitter_id;
                remteller.hit_stream_lining.beneficiary_id = beneficiary_id;
                remteller.hit_stream_lining.isOBPscreenTransaction = 1;
                remteller.hit_stream_lining.open_client_hit_list_all();
            }
            return true;
        }
        return false;
    }

    /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
    this.check_client_pending_ovr_refinitiv = function () {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function () {
            alert("check_client_pending_ovr. failed"); return;
        }
        IRSWorld.BillsPayment.OBPSTransactions.check_cif_enrollment_pending_ovr_obp_refinitiv(this.RemitterID, "", this.check_client_pending_ovr_refinitiv_callback);
    }

    this.check_client_pending_ovr_refinitiv_callback = function (result) {
        var hit_list = null;

        if (result != null && result.value != null) {
            hit_list = result.value.split("~");
        }

        if (hit_list != null && hit_list.length > 0) {
            var msg = "CIF enrollment with pending override.";

            alert(msg);
        }
        else {
            remteller.get_hit_list_all_refinitiv(this.RemitterID, this.BeneficiaryCIFID);
        }
    }

    this.hit_list_override_response = function (param) {
        if (param == false) {
            page_setup();
        }
        else if (param == true) {
            remteller.populate_merchant_info();
        }
        else {
            /* PENDING ROV */
            remteller.client_was_hit = 1;
            remteller.pending_ro_status = parseInt(param.pending_override_status);
            remteller.transaction_status = parseInt(param.pending_override_status);
            remteller.populate_merchant_info();
            /* PENDING ROV */
        }
    }
    //===========================================================================================


    //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
    this.Enable_RemAccount_Details = function()
    {
        document.getElementById("ddRemAcctType").value = "";
        document.getElementById("txtRemAcctNo").value = "";
        document.getElementById("txtRemRoutingNo").value = "";
        document.getElementById("txtRemBankName").value = "";
            
        if (this.IsRemitter_ACH == true && this.ACH_Branch_Setup == true)
        {
            document.getElementById("divRemAccountDetails").style.display = "block";
        }
        else
        {
            document.getElementById("divRemAccountDetails").style.display = "none";
        }
    }

    this.Check_ACH_Branch_Extract_Setup = function()
    {
        this.ACH_Branch_Setup = IRSWorld.BillsPayment.OBPSTransactions.Check_ACH_Extract_Setup().value;
    }

    this.Check_IsRemitter_ACH = function()
    {
        this.IsRemitter_ACH = IRSWorld.BillsPayment.OBPSTransactions.Check_IsRemitter_ACH(this.paymenttype).value;
    }

    this.Set_RemAccount_Details = function()
    {
        this.Remitter_AccountType = document.getElementById("ddRemAcctType").value;
        this.Remitter_AccountNo = document.getElementById("txtRemAcctNo").value;
        this.Remitter_RoutingNo = document.getElementById("txtRemRoutingNo").value;
        this.Remitter_BankName = document.getElementById("txtRemBankName").value;
    }

    this.Save_RemAccount_Details = function(refno)
    {
        if (this.IsRemitter_ACH == true && this.ACH_Branch_Setup == true)
        {
            IRSWorld.BillsPayment.OBPSTransactions.Save_RemAccount_Details(refno, this.Remitter_AccountType, this.Remitter_AccountNo, this.Remitter_RoutingNo, this.Remitter_BankName);
        }
    }
    //************//

    /* A. RFC-12313 SSS BILLERS CURRENCY WILL NOW BE IN USD INSTEAD OF PHP 2021 */

    this.is_sss_usd_merchant = function()
    {
        if (this.MerchantCode.toUpperCase().trim() == "SSS CONTRIBUTION" || this.MerchantCode.toUpperCase().trim() == "SSS LOAN" || this.MerchantCode.toUpperCase().trim() == "SSS REAL ESTATE LOAN" || this.MerchantCode.toUpperCase().trim() == "SSS PESO FUND")
        {
            return true;
        }

        return false;
    }

    this.set_sss_netremit_peso_equivalent = function()
    {
        var remitting_branch = remteller.BranchCode;
        var remitting_currency = document.getElementById('ddlSourceCurrency')[document.getElementById('ddlSourceCurrency').selectedIndex].value;

        this.get_sss_peso_equiv(remitting_branch, remitting_currency);
    }

    this.get_sss_peso_equiv = function(remitting_branch, remitting_currency)
    {
        if (this.MerchantCode.toUpperCase() == "SSS CONTRIBUTION" || this.MerchantCode.toUpperCase() == "SSS LOAN" || this.MerchantCode.toUpperCase() == "SSS REAL ESTATE LOAN" || this.MerchantCode.toUpperCase() == "SSS PESO FUND")
        {
            IRSWorld.BillsPayment.OBPSTransactions.getFXRate(remitting_branch, remitting_currency, "1", this.get_sss_peso_equiv_callback);
        }
    }

    this.get_sss_peso_equiv_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            remteller.NettoBeneAmountPHP = parseFloat(getText("txtTargetAmount")) * parseFloat(result.value);
        }
    }

    this.set_merchants_dynamic_code = function (merchant_code, merchant_productcode)
    {
        if (merchant_code == null || merchant_code == "")
        {
            alert("Please check if merchant is properly selected."); return;
        }
        else
        {
            merchant_code = merchant_code.toUpperCase().trim();
        }

        /* RFC-27195 GIFTAWAY API */
        if (merchant_code == "GIFTAWAY_APPAREL" ||
            merchant_code == "GIFTAWAY_ECOMMERCE" ||
            merchant_code == "GIFTAWAY_FOOD" ||
            merchant_code == "GIFTAWAY_GROCERY" ||
            merchant_code == "GIFTAWAY_HEALTH" ||
            merchant_code == "GIFTAWAY_PREPAID" ||
            merchant_code == "GIFTAWAY_SPECIALTY" ||
            merchant_code == "GIFTAWAY_TRAVEL"
            ) {
            merchant_code = "GIFTAWAY_PRODUCT_API";
        }
        /* RFC-27195 GIFTAWAY API */

        switch (merchant_code)
        {
            case "SSS CONTRIBUTION": remteller.sss_cont_prn_usd_button(); break;
            case "SSS LOAN": remteller.sss_loan_prn_usd_button(); break;
            case "SSS REAL ESTATE LOAN": remteller.sss_real_state_usd_button(); break;
            case "SSS PESO FUND": remteller.sss_peso_fund_usd_button(); break;
            case "GIFTAWAY_PRODUCT_API": remteller.insert_giftway_btn(); break;  /* RFC-27195 GIFTAWAY API */
            case "MAXICARE PREPAID": remteller.insert_maxicareprepaid_btn(); break /* RFC-31110 - Maxicare Prepaid */
            default:
        }
    }

    this.sss_cont_prn_usd_button = function()
    {
        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];

        var sss_cont_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var sss_cont_cell = null;

        sss_cont_row.id = "tr_" + optional_mer_body.rows.length;

        sss_cont_cell = sss_cont_row.insertCell(0);
        sss_cont_cell.id = "tdValidate";
        sss_cont_cell.className = "cell_caption";

        var sss_cont_el = document.createElement("input");

        sss_cont_el.type = 'button';
        sss_cont_el.id = "btnValidate";
        sss_cont_el.style.width = '50%';
        sss_cont_el.style.display = 'inline';
        sss_cont_el.value = "Validate";

        sss_cont_el.onclick = function()
        {
            remteller.sss_cont_prn_usd_validate();
        }

        sss_cont_cell.appendChild(sss_cont_el);

        document.getElementById('tf2').disabled = true;
        document.getElementById('tf3').disabled = true;
        document.getElementById('tf4').disabled = true;
        document.getElementById('tf5').disabled = true;
        document.getElementById('tf6').disabled = true;
        document.getElementById('txtTargetAmount').disabled = true;
    }

    this.sss_loan_prn_usd_button = function()
    {
        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];

        var sss_loan_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var sss_loan_cell = null;

        sss_loan_row.id = "tr_" + optional_mer_body.rows.length;

        sss_loan_cell = sss_loan_row.insertCell(0);
        sss_loan_cell.id = "tdValidate";
        sss_loan_cell.className = "cell_caption";

        var sss_loan_el = document.createElement("input");

        sss_loan_el.type = 'button';
        sss_loan_el.id = "btnValidate";
        sss_loan_el.style.width = '50%';
        sss_loan_el.style.display = 'inline';
        sss_loan_el.value = "Validate";

        sss_loan_el.onclick = function()
        {
            remteller.sss_loan_prn_usd_validate();
        }

        sss_loan_cell.appendChild(sss_loan_el);

        document.getElementById('tf2').disabled = true;
        document.getElementById('tf3').disabled = true;
        document.getElementById('tf4').disabled = true;
        document.getElementById('tf5').disabled = true;
        document.getElementById('tf6').disabled = true;
        document.getElementById('txtTargetAmount').disabled = true;
    }

    this.sss_peso_fund_usd_button = function()
    {
        var bsp_rate = IRSWorld.BillsPayment.OBPSTransactions.get_bsp_fxrate().value;

        if (bsp_rate != null && bsp_rate != "" && bsp_rate.indexOf("ERROR") >= 0)
        {
            alert("Message: " + bsp_rate + " Cannot proceed with transaction."); return;
        }

        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];
        var sss_peso_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var sss_peso_cell = null;

        sss_peso_row.id = "tr_" + optional_mer_body.rows.length;

        sss_peso_cell = sss_peso_row.insertCell(0);
        sss_peso_cell.id = "tdValidate";
        sss_peso_cell.className = "cell_caption";

        var sss_peso_el = document.createElement("input");

        sss_peso_el.type = 'button';
        sss_peso_el.id = "btnValidate";
        sss_peso_el.style.width = '50%';
        sss_peso_el.style.display = 'inline';
        sss_peso_el.value = "Validate";

        sss_peso_el.onclick = function()
        {
            remteller.sss_pesofund_usd_validate();
        }

        sss_peso_cell.appendChild(sss_peso_el);

        setText("tf1", bsp_rate);
        document.getElementById('tf1').disabled = true;
        document.getElementById('txtTargetAmount').disabled = true;
    }

    this.sss_real_state_usd_button = function ()
    {
        var bsp_rate = IRSWorld.BillsPayment.OBPSTransactions.get_bsp_fxrate().value;

        if (bsp_rate != null && bsp_rate != "" && bsp_rate.indexOf("ERROR") >= 0)
        {
            alert("Message: " + bsp_rate + " Cannot proceed with transaction."); return;
        }

        var optional_mer_table = document.getElementById("TrnxDetails");
        var optional_mer_body = optional_mer_table.tBodies[0];
        var sss_rel_row = optional_mer_body.insertRow(optional_mer_body.rows.length);
        var sss_rel_cell = null;

        sss_rel_row.id = "tr_" + optional_mer_body.rows.length;

        sss_rel_cell = sss_rel_row.insertCell(0);
        sss_rel_cell.id = "tdValidate";
        sss_rel_cell.className = "cell_caption";

        var sss_rel_el = document.createElement("input");

        sss_rel_el.type = 'button';
        sss_rel_el.id = "btnValidate";
        sss_rel_el.style.width = '50%';
        sss_rel_el.style.display = 'inline';
        sss_rel_el.value = "Validate";

        sss_rel_el.onclick = function () {
            remteller.sss_real_state_usd_validate();
        }

        sss_rel_cell.appendChild(sss_rel_el);

        setText("tf1", bsp_rate);
        document.getElementById('tf1').disabled = true;
        document.getElementById('txtTargetAmount').disabled = true;
    }

    this.sss_loan_prn_usd_validate = function () {
        if (getText("tf1") != null || getText("tf1") != "") {
            showPanelOB('divpopProcess'); IRSWorld.BillsPayment.OBPSTransactions.get_sss_loan_bsprates_prndet(getText("tf1"), this.sss_loan_prn_usd_validate_callback);
        }
        else {
            alert("Please input valid Loan PRN.");
        }
    }

    this.sss_loan_prn_usd_validate_callback = function (result) {
        var sss_msg = "";
        var SSSNo = IRSWorld.Internet.InternetPage.getLookupValue("RTPConfiguration", "FldCode", "SSS_DUMMY_DATA", "FldDescription").value;
        hidePanelOB('divpopProcess');

        setText("tf2", "");
        setText("tf3", "");
        setText("tf4", "");
        setText("tf5", "");
        setText("tf6", "");

        if (result.value != null || result.value != 'undefined')
        {
            var val = result.value.split('~');

            if (val.length > 0)
            {
                var RTPFlag = IRSWorld.Internet.InternetPage.getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription").value;
                if (RTPFlag == "1") {
                    /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                    if (val[4] == "0") {
                        var bspfx = null;

                        if (val.length == 10) {
                            bspfx = val[9].split(':')[1];
                        }

                        sss_msg = "Valid SSS Loan PRN.\n\n"
                        sss_msg += "SSS PRN: " + val[8] + "\n";
                        sss_msg += "SSS Number: " + SSSNo + "\n";
                        sss_msg += "SSS Member Name: " + val[0];

                        setText("tf2", "");
                        setText("tf3", "");
                        setText("tf4", "");
                        setText("tf5", "");
                        setText("tf6", "");
                        setText("tf2", SSSNo); //SSS Number
                        setText("tf3", val[5]); //applDtFr 
                        setText("tf4", val[6]); //applDtTo
                        setText("tf5", bspfx);
                        setText("tf6", val[3]); //amtDue

                        alert(sss_msg);

                        var converted_amt = 0;

                        converted_amt = parseFloat(val[3].replace(/,/g, '')) / parseFloat(bspfx.replace(/,/g, ''));
                        converted_amt = parseFloat(converted_amt).toFixed(2);

                        setText("txtTargetAmount", converted_amt);

                        document.getElementById('tf1').disabled = true;
                        detectamount_keypress(true);
                        populate_rec();
                    }
                    else if (val[4] == "1") {
                        var bspfx = null;

                        if (val.length == 10) {
                            bspfx = val[9].split(':')[1];
                        }

                        sss_msg = "Valid SSS Loan PRN.\n\n"
                        sss_msg += "SSS PRN: " + val[8] + "\n";
                        sss_msg += "SSS Number: " + SSSNo + "\n";
                        sss_msg += "SSS Member Name: " + val[0];

                        setText("tf2", "");
                        setText("tf3", "");
                        setText("tf4", "");
                        setText("tf5", "");
                        setText("tf6", "");
                        setText("tf2", SSSNo); //SSS Number
                        setText("tf3", val[5]); //applDtFr 
                        setText("tf4", val[6]); //applDtTo
                        setText("tf5", bspfx);
                        setText("tf6", val[3]); //amtDue

                        alert(sss_msg);

                        var converted_amt = 0;

                        converted_amt = parseFloat(val[3].replace(/,/g, '')) / parseFloat(bspfx.replace(/,/g, ''));
                        converted_amt = parseFloat(converted_amt).toFixed(2);

                        setText("txtTargetAmount", converted_amt);

                        document.getElementById('tf1').disabled = true;
                        detectamount_keypress(true);
                        populate_rec();
                    }
                    else {
                        alert('Message: ' + result.value + '. Cannot proceed with the transaction.');
                    }
                }
                    /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                else {
                    if (val[1] == "0") {
                        var bspfx = null;

                        if (val.length == 10) {
                            bspfx = val[9].split(':')[1];
                        }

                        sss_msg = "Valid SSS Loan PRN.\n\n"
                        sss_msg += "SSS PRN: " + val[2] + "\n";
                        sss_msg += "SSS Number: " + val[3] + "\n";
                        sss_msg += "SSS Member Name: " + val[4];

                        setText("tf2", "");
                        setText("tf3", "");
                        setText("tf4", "");
                        setText("tf5", "");
                        setText("tf6", "");
                        setText("tf2", val[3]);
                        setText("tf3", val[7]);
                        setText("tf4", val[8]);
                        setText("tf5", bspfx);
                        setText("tf6", val[6]);

                        alert(sss_msg);

                        var converted_amt = 0;

                        converted_amt = parseFloat(val[6]) / parseFloat(bspfx);
                        converted_amt = parseFloat(converted_amt).toFixed(2);

                        setText("txtTargetAmount", converted_amt);

                        document.getElementById('tf1').disabled = true;
                        detectamount_keypress(true);
                        populate_rec();
                    }
                    else {
                        alert('Message: ' + val[0] + '. Cannot proceed with the transaction.');
                    }
                }
            }
        }
    }

    this.sss_cont_prn_usd_validate = function () {
        if (getText("tf1") != null || getText("tf1") != "") {
            showPanelOB('divpopProcess'); IRSWorld.BillsPayment.OBPSTransactions.get_sss_contri_bsprates_prndet(getText("tf1"), this.sss_cont_prn_usd_validate_callback);
        }
        else {
            alert("Please input valid PRN.");
        }
    }

    this.sss_cont_prn_usd_validate_callback = function (result) {
        var sss_msg = "";
        var SSSNo = IRSWorld.Internet.InternetPage.getLookupValue("RTPConfiguration", "FldCode", "SSS_DUMMY_DATA", "FldDescription").value;
        hidePanelOB('divpopProcess');

        setText("tf2", "");
        setText("tf3", "");
        setText("tf4", "");
        setText("tf5", "");
        setText("tf6", "");

        if (result.value != null || result.value != 'undefined')
        {
            var val = result.value.split('~');

            if (val.length > 0)
            {
                /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                var RTPFlag = IRSWorld.Internet.InternetPage.getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription").value;
                if (RTPFlag == "1") {
                    if (val[4] == "1") {
                        var bspfx = null;

                        if (val.length == 10) {
                            bspfx = val[9].split(':')[1];
                        }

                        sss_msg = "Valid SSS Contribution PRN.\n\n"
                        sss_msg += "SSS PRN: " + val[8] + "\n";
                        sss_msg += "SSS Number: " + SSSNo + "\n";
                        sss_msg += "SSS Member Name: " + val[0];

                        setText("tf2", "");
                        setText("tf3", "");
                        setText("tf4", "");
                        setText("tf5", "");
                        setText("tf6", "");
                        setText("tf2", SSSNo); //SSS Number
                        setText("tf3", val[5]); //applDtFr 
                        setText("tf4", val[6]); //applDtTo
                        setText("tf5", bspfx);
                        setText("tf6", val[3]); //amtDue

                        alert(sss_msg);

                        var converted_amt = 0;

                        converted_amt = parseFloat(val[3].replace(/,/g, '')) / parseFloat(bspfx.replace(/,/g, ''));
                        converted_amt = parseFloat(converted_amt).toFixed(2);

                        setText("txtTargetAmount", converted_amt);

                        document.getElementById('tf1').disabled = true;
                        detectamount_keypress(true);
                        populate_rec();
                    }
                    else {
                        alert('Message: ' + result.value + '. Cannot proceed with the transaction.');
                    }
                }
                    /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                else {
                    if (val[1] == "0") {
                        var bspfx = null;

                        if (val.length == 10) {
                            bspfx = val[9].split(':')[1];
                        }

                        sss_msg = "Valid SSS Contribution PRN.\n\n"
                        sss_msg += "SSS PRN: " + val[2] + "\n";
                        sss_msg += "SSS Number: " + val[3] + "\n";
                        sss_msg += "SSS Member Name: " + val[4];

                        setText("tf2", "");
                        setText("tf3", "");
                        setText("tf4", "");
                        setText("tf5", "");
                        setText("tf6", "");
                        setText("tf2", val[3]);
                        setText("tf3", val[7]);
                        setText("tf4", val[8]);
                        setText("tf5", bspfx);
                        setText("tf6", val[6]);

                        alert(sss_msg);

                        var converted_amt = 0;

                        converted_amt = parseFloat(val[6]) / parseFloat(bspfx);
                        converted_amt = parseFloat(converted_amt).toFixed(2);

                        setText("txtTargetAmount", converted_amt);

                        document.getElementById('tf1').disabled = true;
                        detectamount_keypress(true);
                        populate_rec();
                    }
                    else {
                        alert('Message: ' + val[0] + '. Cannot proceed with the transaction.');
                    }
                }
            }
        }
    }

    this.sss_pesofund_usd_validate = function () {
        var sss_number = getText("txtMemberID");
        var peso_amoount = getText("tf2");

        if (sss_number == null || sss_number == "") {
            alert("Please enter valid SSS number."); return;
        }

        if (sss_number == null || sss_number == "") {
            alert("Please enter valid amount."); return;
        }

        showPanelOB('divpopProcess');

        IRSWorld.BillsPayment.OBPSTransactions.sss_peso_fund_chk_amount(sss_number, peso_amoount, this.sss_pesofund_usd_validate_callback);

    }

    this.sss_pesofund_usd_validate_callback = function (result) {
        var peso_amoount = getText("tf2");
        var bsp_rate = getText("tf1");

        hidePanelOB('divpopProcess');

        if (result != null && result.value != null) {
            if (result.value != "") {
                alert(result.value); return
            }

            if (result.value == "") {
                var converted_amt = 0;

                converted_amt = parseFloat(peso_amoount) / parseFloat(bsp_rate);
                converted_amt = parseFloat(converted_amt).toFixed(2);

                setText("txtTargetAmount", converted_amt);

                document.getElementById('tf2').disabled = true;
                detectamount_keypress(true);
                populate_rec();
            }
        }
    }

    this.sss_real_state_usd_validate = function () {
        var sss_number = getText("txtMemberID");
        var peso_amoount = getText("tf2");

        if (sss_number == null || sss_number == "") {
            alert("Please enter valid SSS loan account number."); return;
        }

        if (sss_number == null || sss_number == "") {
            alert("Please enter valid amount."); return;
        }

        showPanelOB('divpopProcess');

        IRSWorld.BillsPayment.OBPSTransactions.sss_real_estate_chk_amount(peso_amoount, this.sss_real_state_usd_validate_callback);
    }

    this.sss_real_state_usd_validate_callback = function (result) {
        var peso_amoount = getText("tf2");
        var bsp_rate = getText("tf1");

        hidePanelOB('divpopProcess');

        if (result != null && result.value != null) {
            if (result.value != "") {
                alert(result.value); return
            }

            if (result.value == "") {
                var converted_amt = 0;

                converted_amt = parseFloat(peso_amoount) / parseFloat(bsp_rate);
                converted_amt = parseFloat(converted_amt).toFixed(2);

                setText("txtTargetAmount", converted_amt);

                document.getElementById('tf2').disabled = true;
                detectamount_keypress(true);
                populate_rec();
            }
        }
    }

    this.post_sss_loancontri_prn = function (reference_number, merchant_code) {
        var prn = getText("tf1");

        if (merchant_code != null && merchant_code != "") {
            if (merchant_code == "SSS CONTRIBUTION" || merchant_code == "SSS LOAN") {
                showPanelOB('divpopProcess');
                IRSWorld.BillsPayment.OBPSTransactions.post_sss_loancontri_prn(prn, reference_number, merchant_code, this.post_sss_loancontri_prn_callback);
            }
        }
    }

    this.post_sss_loancontri_prn_callback = function (result) {
        hidePanelOB('divpopProcess');
    }

    /* A. RFC-12313 SSS BILLERS CURRENCY WILL NOW BE IN USD INSTEAD OF PHP 2021 */
}

function get_data_value(tabid, read_only)/*This Validates merchant details input*/
{
    var tabid = document.getElementById(tabid);
    var strreturn = '';
    var tblrows = tabid.rows;

    if (tblrows == undefined) return;

    for (var i = 0; i < tblrows.length; i++)
    {
        var sells = tblrows[i].cells;

        for (var j = 0; j < sells.length; j++)
        {
            if (sells.item(j).hasChildNodes())
            {
                var childs = sells.item(j).childNodes;

                for (var k=0; k < childs.length; k++)
                {
                    if (childs[k].nodeName == 'INPUT')
                    {
                        if (remteller.MerchantCode == "Dragonpay")
                        {
                            if (childs[k].title == 'true' & childs[k].name != '')
                            {
                                if (childs[k].value == '')
                                {
                                    strreturn += childs[k].name + ' is mandatory \n';
                                }
                            }
                        }
                        else
                        {
                            if (childs[k].title == 'true' & childs[k].value == '') 
			                {
                                strreturn += sells.item(0).id + ' is mandatory \n';
                            }
                            else
                            {                            
                                pipe_value += childs[k].id + '|';
                                comma_value += childs[k].value + ',';
                            }
                        }

                        if(childs[k].title == 'true' & childs[k].name == 'ExcessPayment' & childs[k].value < 200 & childs[k].style.display != 'none')
                        {
                            strreturn += sells.item(0).id + ' is mandatory or should be equal or greater than 200 \n';
                        }
                    }
                    else if (childs[k].nodeName == 'SELECT')
                    {
                        if (childs[k].length > 0)
                        {
                            if (childs[k].title == 'true' & childs[k].selectedIndex == '0')
                            {
                                strreturn += sells.item(0).id + ' is mandatory \n';
                            }
                            else
                            {
                                pipe_value += childs[k].id + '|';
                                comma_value += document.getElementById(childs[k].id)[childs[k].selectedIndex].text + ',';
                            }
                        }
                    }
                    else if (childs[k].nodeName == 'TEXTAREA')
                    {
                        if (childs[k].title == 'true' & childs[k].value == '')
                        {
                            strreturn += sells.item(0).id + ' is mandatory \n';
                        }
                        else
                        {
                            pipe_value += childs[k].id + '|';
                            comma_value += childs[k].value + ',';
                        }
                    }
                    else if (childs[k].nodeName == 'TABLE')
                    {
                        if (childs[k].title == 'true' & childs[k].value == '')
                        {
                            strreturn += sells.item(0).id + ' is mandatory \n';
                        }
                        else
                        {
                            pipe_value += childs[k].id + '|';
                            comma_value += childs[k].value + ',';
                        }
                    }
                }
            }
        }
    }

    return strreturn;
}

function gettranxdetails()
{
    var strreturn = '';

    if (document.getElementById('lblPayMode').value == undefined || document.getElementById('lblPayMode').value == '0')
    {
        strreturn += 'Please select Pay Mode \n';
    }

    if (document.getElementById('lblSourceOfFunds').value == undefined || document.getElementById('lblPayMode').value == '0')
    {
        strreturn += 'Please select Source of Funds \n';
    }

    if (document.getElementById('lblReceiptMode').value == undefined || document.getElementById('lblPayMode').value == '0')
    {
        strreturn += 'Please select Receipt Mode \n';
    }

    if (document.getElementById('lblTranxSource').value == undefined || document.getElementById('lblPayMode').value == '0')
    {
        strreturn += 'Please select Transaction Source \n';
    }

    if (document.getElementById('lblTranxPurpose').value == undefined || document.getElementById('lblPayMode').value == '0')
    {
        strreturn += 'Please select Transaction Purpose \n';
    }

    return strreturn;
}

function get_total_ssscontri(tabid)
{
    var tabid = document.getElementById(tabid);
    var tblrows = tabid.rows;
    if (tblrows == undefined) return;

    for (var i = 0; i < tblrows.length; i++)
    {
        var sells = tblrows[i].cells;

        for (var j = 0; j < sells.length; j++)
        {
            if (sells.item(j).hasChildNodes())
            {
                var childs = sells.item(j).childNodes;

                for (var k=0; k < childs.length; k++)
                {
                    if (childs[k].nodeName == 'INPUT')
                    {
                        if (childs[k].type == 'text')
                        {                            
                            if (childs[k].name=='ExcessPayment')
                            {   
                                var target_amt;

                                if (document.getElementById('nf1')[document.getElementById('nf1').selectedIndex].text.indexOf('OVER') < 0)
                                {
                                    target_amt = document.getElementById('nf1').value;
                                    excess_amt = 0;
                                }
                                else
                                {
                                    target_amt = document.getElementById('nf1')[document.getElementById('nf1').selectedIndex].text.split(' ')[1];
                                }

                                if (document.getElementById('nf3').value == '') 
                                { 
                                    excess_amt = 0;
                                }

                                excess_amt =  parseFloat((document.getElementById('nf3').value ) == '' ? 0 : document.getElementById('nf3').value );
                                document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(formatCurrency((target_amt * no_of_months) + excess_amt)).value;  // TargetAmount + ExcessPayment
                            }
                        }
                    }
                    
                    else if (childs[k].nodeName == 'SELECT')
                    {
                        if (childs[k].name=='MonthlyContribution')
                        {
                            if (document.getElementById(childs[k].id)[childs[k].selectedIndex].text.indexOf('OVER') < 0) 
                            {
                                document.getElementById('nf3').style.display = 'none';
                                document.getElementById('Excess Payment').style.display = 'none';
                                document.getElementById('txtTargetAmount').value = '0';
                                document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(formatCurrency(document.getElementById(childs[k].id)[childs[k].selectedIndex].text)).value;
                            }
                            else
                            {
                                document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(formatCurrency(document.getElementById('nf1')[document.getElementById('nf1').selectedIndex].text.split(' ')[1])).value;
                                document.getElementById('nf3').style.display = 'inline';
                                document.getElementById('Excess Payment').style.display = 'inline';
                                document.getElementById('6').style.display = 'inline';
                            }
                        }                        
                        else if (childs[k].name == 'NumberOfMonths')
                        {
                            var sss_contri = parseFloat(document.getElementById('nf1').value);

                            if (isNaN(sss_contri))
                            {
                                sss_contri = 1560;
                            }

                            no_of_months = parseFloat(document.getElementById('nf2').value);
                            document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(formatCurrency((sss_contri * no_of_months) + excess_amt)).value;
                        }
                        else if (childs[k].name == 'FoodPackage')
                        {
                            document.getElementById('txtTargetAmount').value = IRSWorld.BillsPayment.OBPSTransactions.FoodAmount(document.getElementById('tf4').value, remteller.MerchantCode).value;
                        }
                        else if(childs[k].name=="PhilhealthMonths")
                        {
                            document.getElementById('txtTargetAmount').value = parseFloat(IRSWorld.BillsPayment.OBPSTransactions.getPhilhealthAmount().value) * parseFloat(childs[k].value);
                        }
                    }           
                }
            }
        }
    }    
}


function create_charges_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();

        col.header = '';
        col.datafield = 'Field';
        col.align = 'right';
        col.styles.cell += 'width:5%';
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();

        col.header = '';
        col.datafield = 'Ccy';
        col.align = 'right';
        col.styles.cell += 'width:3%';
        dbgrid.addColumn(col);
    }

    {
        var col = new datagrid_column();

        col.header = '';
        col.datafield = 'Val';
        col.align = 'right';
        col.formatter = 'formatter';
        col.styles.cell += 'width:3%';
        dbgrid.addColumn(col);
    }

    dbgrid.caption = 'Charges';
    dbgrid.classNames.table = 'table_body';
    dbgrid.rows_page = 15;
    dbgrid.row.normal_style = 'background-color: #f7ffff;';

}

function formatter(val)
{
    if (isNaN(val) | val==null)
    {
        return 0;
    }

    var fval = parseFloat(val);

    if (isNaN (fval))
    {
        fval = 0.0000;
    }

    return formatNumber(val, 4);
}


function bind_grid(charges)
{
    if (charges.error != null)
    {
        spawn_errpage(charges.error);
        return;
    }

    create_charges_grid();
    dbgrid.bind(charges.value, 'tblCharges', 'gridCharges');
}

function populate_rec()
{
    var brcode = this.BranchCode;
    var remitcurr = document.getElementById('lblRemitCurr').value;
    var accode = document.getElementById('lbl_merchant_currency').value;
    var targetamount = parseFloat(new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value));

    if (document.getElementById("txtTargetAmount").disabled == true)
    {
        remteller.NettoBeneAmountPHP = targetamount;
    }

    /* B. RFC-12313 SSS USD */
    remteller.set_sss_netremit_peso_equivalent();
    /* B. RFC-12313 SSS USD */


    if (remteller.RemitterChargeType != null && remteller.RemitterChargeType != '' && remteller.RemitterChargeType != '0')
    {
    }
    else
    {
        alert('Please select Charge Type.');
        showPanelOB('divTransactionDetails');
    }

    if (accode == '')
    {
        accode = document.getElementById('lbl_merchant_currency').value;
    }

    if (targetamount == '')
    {
        targetamount = 0;
    }

    /* RFC - 4404 DEFINITION OF NEW BILLER MERALCO */
    if (remteller.MerchantCode != null && remteller.MerchantCode.trim().toUpperCase() == "MERALCO")
    {
        if (remteller.MerProdCode != null && remteller.MerProdCode == "202")
        {
            remteller.chk_meralco_ref_is_valid(); return;
        }
    }
    /* RFC - 4404 DEFINITION OF NEW BILLER MERALCO */

    /* 1. RFC-10309 PNB GENERAL INSURER OCTOBER 2020 */
    if (remteller.MerchantCode != null && remteller.MerchantCode.trim().toUpperCase() == "PNB GENERAL INSURERS")
    {
        remteller.chk_pnb_gen_refnum(); return;
    }
    /* 1. RFC-10309 PNB GENERAL INSURER OCTOBER 2020 */

    /* 1. RFC-10310 BENEFICIAL LIFE */
    if (remteller.MerchantCode != null && remteller.MerchantCode.trim().toUpperCase() == "BENEFICIAL LIFE")
    {
        remteller.chk_beneflife_premium(); return;
    }
    /* 1. RFC-10310 BENEFICIAL LIFE */

    //=========================================================================================
    /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
    //remteller.pending_ro_status = 0;
    if (remteller.pending_ro_status == null || remteller.pending_ro_status == 'null' || remteller.pending_ro_status == '0' || remteller.pending_ro_status == 0 || remteller.pending_ro_status == "")
    {
        remteller.pending_ro_status = 0;
    }
    //=========================================================================================

    remteller.displaycharges();

    if (remteller.EnabledRoundUp == '1' & targetamount > 0)
    {
        var actualamount = IRSWorld.BillsPayment.OBPSTransactions.getActualPayment().value;
        document.getElementById('txtActualAmount').value = actualamount;
    }
}

function onEnter(keycode)
{
    if (keycode==13)
    {
        new frm_bills().ChargesVisibility();
    }
}

var isShift = false;

function isNumeric_keycode(keyCode) 
{ 
    if (keyCode == 16)
        isShift = true;

    return ((keyCode >= 48 && keyCode <= 57 || keyCode == 8 ||
		    (keyCode >= 96 && keyCode <= 105)) && isShift == false);
}

function showPanelOB(divName)
{
    var div = document.getElementById(divName);

    if (divName == 'divTransactionDetails')
    {
        if(uRole == '10')
        {
            document.getElementById(divName).style.height = "25%";
        }
    }

    divmask = new maskpanel();
    divmask.show();

    document.getElementById(divName).style.display = "";

    div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
}

function hidePanelOB(divName)
{
    document.getElementById(divName).style.display = "none";
    divmask.hide();
}

function verifyButton(btnName)
{
    if (document.getElementById(btnName).value == "Merchant Details")
    {
        document.getElementById('tdlblCharges').style.display = "none";
        document.getElementById('calc').style.display = "none";
        document.getElementById('lblTranxHistory').style.display = "none";
        document.getElementById('DIV_TRAN_DETAILS').style.display = "none";
        document.getElementById('trMerchant').style.display = "";
        document.getElementById('trMerchantDetails').style.display = "";

        showPanelOB('divMerchantDetails');
    }
    else if (document.getElementById(btnName).value == "Payment Details")
    {
        showPanelOB('divTransactionDetails');
    }
    else
    {
        document.getElementById('trMerchant').style.display = "none";
        document.getElementById('trMerchantDetails').style.display = "none";
        document.getElementById('lblTranxHistory').style.display = "none";
        document.getElementById('DIV_TRAN_DETAILS').style.display = "none";
        document.getElementById('tdlblCharges').style.display = "";
        document.getElementById('calc').style.display = "";

        document.getElementById("trOverride").style.display = 'none';
        document.getElementById("trOverrideDetails").style.display = 'none';
        document.getElementById("trCharges").style.display = '';
        document.getElementById("trChargesDetails").style.display = '';
        document.getElementById("btnOverrideCharges").style.display = 'inline';
        document.getElementById("btnBClosePanel").style.display = 'inline';

        if (uRole == '7' || uRole == '27' || uRole == '25')
        {
            document.getElementById("btnWaive").style.display = 'none';
        }
        else
        {
            document.getElementById("btnWaive").style.display = 'inline';
        }

        showPanelOB('divMerchantDetails');
    }
}

function push_override_result(param)
{
    if (remteller != null)
    {
        remteller.compute_override(param);
    }
}

function push_waive_charges_result(param)
{
    if (remteller != null)
    {        
        var target_amt = parseFloat(new common_method().to_remove_comma(document.getElementById('txtTargetAmount').value));
        var account_curr = document.getElementById('lbl_merchant_currency').value;
        var display = IRSWorld.BillsPayment.OBPSTransactions.waiveCharges(parseFloat(product_charges_id), parseFloat(account_curr), false, 3, target_amt).value;

        document.getElementById('calc').innerHTML = display;
        remteller.getchargesdet();

        if (remteller.EnabledRoundUp == '1' & target_amt > 0)
        {
            var actualamount = IRSWorld.BillsPayment.OBPSTransactions.getActualPayment().value;
            document.getElementById('txtActualAmount').value = actualamount;
        }
        /* PENDING ROV */
        if (param.pending_override_status != null && parseInt(param.pending_override_status) > 0)
        {
            remteller.pending_ro_status = parseInt(param.pending_override_status);
            remteller.transaction_status = parseInt(param.pending_override_status);
        }
        /* PENDING ROV */

        showPanelOB('divMerchantDetails');
    }
}

function push_exception_screen_result(param)
{
    if (remteller != null)
    {
        remteller.exception_screen_result(param);
    }
}

function push_check_payments_commit_response(param)
{
    if (remteller != null)
    {
        remteller.check_payment_commit_result(param);
    }
}

function push_card_payment_commit_response(param)
{
    if (remteller != null)
    {
        remteller.card_payment_commit_result(param);
    }
}

function push_debit_account_payment_commit_response(param)
{
    if (remteller != null)
    {
        remteller.debit_account_payment_commit_result(param);
    }
}

function push_check_payment_result()
{
    if (remteller != null)
    {
        remteller.check_payment_result();
    }
}

function push_card_payment_commit_respose(param)
{
    if (remteller != null)
    {
        remteller.card_payment_commit_result(param);
    }
}

function push_card_payment_result()
{
    if (remteller != null)
    {
        remteller.card_payment_result();
    }
}

function push_debit_account_payment_commit_respose(param)
{
    if (remteller != null)
    {
        remteller.debit_account_payment_commit_result(param);
    }
}

function push_debit_account_payment_result()
{
    if (remteller != null)
    {
        remteller.debit_account_payment_result();
    }
}

function push_remit_limit_response(param)
{
    if (remteller != null)
    {
        remteller.remit_limit_response(param);
    }
}

function cifclose()
{
    if (remteller != null)
    {
        if (window.returnValue != null && window.returnValue != 'null')
        {
            remCIFID = window.returnValue;

            var SQL = "sp_cif_get_master_seqid~" + remCIFID;
            var dt = IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL).value;

            if (dt != null && dt.Rows.length != 0)
            {
                remteller.SEQ_ID = dt.Rows[0].SEQ_ID.toString();

                if (remteller.popRemDetail() == true)
                {
                    remteller.callLookUp('MERCHANT');
                }
            }
        }
    }
}

function online_disclaimer()
{
    this.disclaimer_form = function()
    {
        this.open_disclaimer_modal_window();
        this.construct_disclaimer_window();
    }

    this.load_disclaimer = function()
    {
        IRSWorld.BillsPayment.OBPSTransactions.getDisclaimer(idisclaimer.load_disclaimer_callback);
    }

    this.load_disclaimer_callback = function(result)
    {
        var response = result.value;

        if (response != null && response != "" && response != undefined)
        {
            document.getElementById("spnDisclaimer").innerHTML = response;
            
        }
        else
        {
            document.getElementById("spnDisclaimer").innerHTML = "There is no disclaimer for this branch. Please accept disclaimer to proceed.";
            
        }

        document.getElementById("btnAccept").disabled = true;

        if (IRSWorld.BillsPayment.OBPSTransactions.getRemitterDisclaimer('').value == '1')
        {
            idisclaimer.acceptDisclaimer(true);
        }        
    }

    this.construct_disclaimer_window = function()
    {
        var html_string = "";

        html_string = "<table border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">";
        html_string += "<tr>";
        html_string += "<td><span id=\"spnDisclaimer\"></span></td>"
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"height: 20px\"><input id=\"chkDisclaimer\" type=\"checkbox\" onclick=\"javascript:idisclaimer.checkDisclaimer();\" style=\"width:20px; border:none;\" /><span class=\"label2\">I have read and agree to the above terms and conditions</span></td>"
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td>";
        html_string += "<input id=\"btnAccept\" type=\"button\" value=\"Accept\" onclick=\"javascript:idisclaimer.acceptDisclaimer(true);\" style=\"width: 100px;\"/>"
        html_string += "<input id=\"btnDecline\" type=\"button\" value=\"Decline\" onclick=\"javascript:idisclaimer.acceptDisclaimer(false);\" style=\"width: 100px;\" />"
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "</table>";

        document.getElementById("divDisclaimer").innerHTML = html_string;
        idisclaimer.load_disclaimer();
    }

    this.construct_disclaimer_div_content = function()
	{
        var content_div = null;

        this.inner_modal_div = document.createElement("div");
        this.inner_modal_div.align = "center";
        this.inner_modal_div.className = "inner_div";
        this.inner_modal_div.id = "inner_mod_div";
        this.inner_modal_div.style.height = "auto";
        this.inner_modal_div.style.width = parseInt(parent.screen.availWidth) - 350 + "px";
        this.inner_modal_div.style.top = "30px";
        this.inner_modal_div.style.left = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
        this.inner_modal_div.style.border = "medium solid white";
        this.inner_modal_div.Code = this;

        content_div = document.createElement("div");
        content_div.id = "divDisclaimer";
        content_div.align = "left";
        content_div.style.background = "transparent";
        content_div.style.width = "100%";
        content_div.style.overflow = "auto";
        content_div.innerHTML = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

        this.inner_modal_div.appendChild(content_div);

        document.body.appendChild(this.inner_modal_div);
    }

	this.close_disclaimer_modal_window = function()
	{
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if (this.outer_modal_div != null) { document.body.removeChild(this.outer_modal_div); this.outer_modal_div = null; }
        if (this.inner_modal_div != null) { document.body.removeChild(this.inner_modal_div); this.inner_modal_div = null; }
        if (this.content_iframe != null) { this.content_iframe = null; }

        document.body.style.margin = "";
    }

	this.open_disclaimer_modal_window = function()
	{
        idisclaimer.load_disclaimer_modal_css();

        this.outer_modal_div = document.createElement("div");
        this.outer_modal_div.id = "outer_mod_div";
        this.outer_modal_div.className = "outer_div";

        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div").style.height = screen.height;

        idisclaimer.construct_disclaimer_div_content();

        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }

	this.load_disclaimer_modal_css = function()
	{	    
        var file_reference = document.createElement("link");

        file_reference.setAttribute("rel", "stylesheet")
        file_reference.setAttribute("type", "text/css")
        file_reference.setAttribute("href", ".././css/cssmodals.css")

		if (typeof file_reference !="undefined")
		{		    
            document.getElementsByTagName("head")[0].appendChild(file_reference)
        }
    }

	this.checkDisclaimer = function()
    {
	    var chkDisclaimer = document.getElementById("chkDisclaimer");

        if (chkDisclaimer.checked == true)
        {
            document.getElementById("btnAccept").disabled = false;
        }
        else
        {
            document.getElementById("btnAccept").disabled = true;
        }
    }

    this.acceptDisclaimer = function(willAccept)
    {
        if (willAccept == true)
        {
            IRSWorld.BillsPayment.OBPSTransactions.updateDisclaimer('', true);
            this.close_disclaimer_modal_window();
            load_page('OBP-Online');

            var remCIF = IRSWorld.BillsPayment.OBPSTransactions.getCIFID().value;
            var SQL = "sp_cif_get_master_seqid~" + remCIF;
            var dt = IRSWorld.BillsPayment.OBPSTransactions.getDataTable(SQL).value;
            var remseqid = '';

            if(dt != null && dt.Rows.length != 0)
            {
                remseqid = dt.Rows[0].SEQ_ID.toString();
            }
            remteller.getbranches();
            remteller.getcurrencycat();
            remteller.LookUpMode = 'REMITTER';
            remteller.SEQ_ID = remseqid;
            remteller.RemitMethod = '9'
            remteller.popRemDetail();
        }
        else
        {
            window.open('../Main/frmCenter.aspx', '_self');
        }
    }
}

function online_recept()
{
    this.refno = null;

    this.online_recept_form = function(refno)
    {
        this.refno = refno;
        this.open_online_receipt_modal_window();
        this.construct_online_receipt_window();
    }

    this.header_receipt = function()
    {
        IRSWorld.BillsPayment.OBPSTransactions.getBranchOperations(remteller.BranchCode, oreceipt.header_receipt_callback)
    }

    this.header_receipt_callback = function(result)
    {
        var dt = result.value;
        var count = 0;

        if (dt != null && dt != "" && dt != undefined && dt.Rows.length != 0)
        {
            setText(document.getElementById("spnBranch"), "Philippine National Bank " + dt.Rows[0].Branch.toString());
            setText(document.getElementById("spnDescription"), dt.Rows[0].Description.toString());
            setText(document.getElementById("spnEmail"), dt.Rows[0].Email.toString())

            for (var i = 0; i <= (dt.Rows.length - 1); i++)
            {
                count = count + 1;

                if (count <= 3)
                {
                    var address = dt.Rows[i]["SubBranch"] + "<br>" + dt.Rows[i]["Address"] + "<br>Tel:" + dt.Rows[i]["TelNo"];
                    setText(document.getElementById("spnBranch1"), address)
                }
                else
                {
                    break;
                }
            }

            oreceipt.load_receipt();
        }
    }

    this.load_receipt = function()
    {
        IRSWorld.BillsPayment.OBPSTransactions.getBillsPayment(oreceipt.refno, oreceipt.load_receipt_callback);
    }

    this.load_receipt_callback = function(result)
    {
        var dt = result.value;

        if (dt != null && dt != "" && dt != undefined && dt.Rows.length != 0)
        {            
            setText(document.getElementById("spnPGResponse"), "Auth Code: " + dt.Rows[0].AuthCode.toString() + " / Response: " + dt.Rows[0].Message.toString());
            setText(document.getElementById("spnTRemitterName"), remteller.RemitterFullName);
            setText(document.getElementById("spnTBeneficiaryName"), dt.Rows[0].BenefName.toString());
            setText(document.getElementById("spnTRemitterAddress"), remteller.RemitterAddress);
            setText(document.getElementById("spnTBeneficiaryAddress"), dt.Rows[0].BenefAddress.toString());
            setText(document.getElementById("spnTBeneficiaryBranch"), dt.Rows[0].ThruPNBBranch.toString());
            setText(document.getElementById("spnTReferenceNo"), dt.Rows[0].ReferenceNo.toString());
            setText(document.getElementById("spnTExchangeRateCurrency"), dt.Rows[0].RemittingCurrencyCode.toString() + ' to ' + dt.Rows[0].NetToBenefCurrencyCode.toString());
            setText(document.getElementById("spnTExchangeRate"), IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(dt.Rows[0].ForexRemittingToAccount.toString()).value);
            setText(document.getElementById("spnTTransactionDate"), dt.Rows[0].TransDate.toString());
            setText(document.getElementById("spnTStatus"), dt.Rows[0].TranxStatus.toString());
            setText(document.getElementById("spnNetAmountCurrency"), dt.Rows[0].RemittingCurrencyCode.toString());
            setText(document.getElementById("spnTGrossAmount"), IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(dt.Rows[0].GrossRemAmount.toString()).value);
            setText(document.getElementById("spnTEncodedBy"), dt.Rows[0].UserID.toString());
            setText(document.getElementById("spnTBankChargeCurrency"), dt.Rows[0].RemittingCurrencyCode.toString());
            setText(document.getElementById("spnTBankCharge"), IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(dt.Rows[0].TotalLocalCharge.toString()).value);                        
            setText(document.getElementById("spnTGrossAmountCurrency"), dt.Rows[0].RemittingCurrencyCode.toString());
            setText(document.getElementById("spnNetAmount2"), IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(dt.Rows[0].NetRemAmount.toString()).value);
            setText(document.getElementById("spnTSubType"), dt.Rows[0].ReceiptModeDesc.toString());
            setText(document.getElementById("spnTNetToBenefAmountCurrency"), dt.Rows[0].NetToBenefCurrencyCode.toString());
            setText(document.getElementById("spnTNetToBenefAmount"), IRSWorld.BillsPayment.OBPSTransactions.ConvertToMoney(dt.Rows[0].NettoBeneAmount.toString()).value);
            setText(document.getElementById("spnTProductDescription"), "RAPID REMIT");
        }
        else
        {
            alert('Could not generate receipt');
            document.getElementById("spnError");
            oreceipt.close_online_receipt_modal_window();
        }
    }

    this.construct_online_receipt_window = function()
    {
        var html_string = "";

        html_string = "<table style=\"border-right: #394397 3px solid; border-top: #394397 3px solid; border-left: #394397 3px solid;border-bottom: #394397 3px solid;width:700px;\">";
        html_string += "<tr>";
        html_string += "<td colspan=\"2\" style=\"height:auto;width:700px;\">";
        html_string += "<table style=\"\" cellpadding=\"0\" cellspacing=\"0\">";
        html_string += "<tr>";
        html_string += "<td rowspan=\"3\" style=\"width:100px;\">";
        html_string += "<img src=\"../images/PNBLOGOembsmaller.png\" />";
        html_string += "</td>";
        html_string += "<td style=\"height:auto;width:600px;text-align:center;\">";
        html_string += "<b><span id=\"spnBranch\"></span></b>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"FONT-SIZE:9px;height:auto;width:600px;text-align:center;\"><b><span id=\"spnDescription\"></span></b></td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"FONT-SIZE:9px;height:auto;width:600px;text-align:center;\">"
        html_string += "<table border=\"2\" style=\"\" cellpadding=\"0\" cellspacing=\"0\">";
        html_string += "<tr>";
        html_string += "<td style=\"width:600px;\"><b><span id=\"spnBranch1\"></span></b></td>";
        html_string += "<%--<td style=\"width: 3px\"><b><span id=\"spnBranch2\"></span></b></td>";
        html_string += "<td><b><span id=\"spnBranch3\"></span></b></td>--%>";
        html_string += "</tr>";
        html_string += "</table>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "</table>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td colspan=\"2\" style=\"FONT-SIZE:9px;height:auto;background-color:paleturquoise;width:700px;\">Email: <span id=\"spnEmail\"></span></td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td colspan=\"2\" style=\"width:700px;font-size: 12px;font-weight: bold;color: #ffffff;font-family: Tahoma;background-color: #394398;height: 24px;text-indent: 10px;\"><center><span id=\"spnPGResponse\"></span></center></td>"
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td colspan=\"2\" style=\"FONT-SIZE: 12px;\">";
        html_string += "<table style=\"width:700px;\" cellpadding=\"0\" cellspacing=\"0\">";
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:left;width:350px;\"><b>Remitter's Information</b></td>";
        html_string += "<td style=\"height:auto;text-align:left;width:350px;\"><b>Beneficiary's Information</b></td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;\">";
        html_string += "<span id=\"spnTRemitterName\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;\">";
        html_string += "<span id=\"spnTBeneficiaryName\"></span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;\">";
        html_string += "<span id=\"spnTRemitterAddress\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;\">";
        html_string += "<span id=\"spnTBeneficiaryAddress\"></span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:left;width:350px;\">&nbsp;</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;FONT-SIZE: 9px;text-align:left;width:350px;\">";
        html_string += "<span id=\"spnTBeneficiaryAccount\"></span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:left;width:350px;\">&nbsp;</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto; FONT-SIZE: 9px;text-align:left;width:350px;\">";
        html_string += "<span id=\"spnTBeneficiaryBranch\"></span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr><td colspan=\"2\"><hr /></td></tr>";
        html_string += "<tr>";
        html_string += "<td colspan=\"2\" style=\"height:auto;\">";
        html_string += "<table style=\"\" cellpadding=\"0\" cellspacing=\"0\">"
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">Reference No</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:175px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTReferenceNo\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">Exchange Rate</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:88px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTExchangeRateCurrency\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:right;width:87px;\">";
        html_string += "<span id=\"spnTExchangeRate\">0.00</span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">Transaction Date</td>";
        html_string += "<td colspan=\"4\" style=\"background-color:aliceblue;height:auto;text-align:left;width:175px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTTransactionDate\">##/##/####</span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">";
        html_string += "Status</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:175px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTStatus\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">";
        html_string += "Total Cost</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:88px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnNetAmountCurrency\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:right;width:87px;\">";
        html_string += "<span id=\"spnTGrossAmount\">0.00</span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">Encoded By</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:175px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTEncodedBy\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">";
        html_string += "Service Fee</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:88px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTBankChargeCurrency\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:right;width:87px;\">";
        html_string += "<span id=\"spnTBankCharge\">0.00</span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<%--<tr>";
        html_string += "<td class=\"cell_caption\" align=\"left\" style=\"width:22%;height:20px;\">Validated By</td>";
        html_string += "<td colspan=\"4\" align=\"left\" style=\"background-color:aliceblue;height:20px;width:30%\">"
        html_string += "&nbsp;:&nbsp;<span id=\"spnTValidatedBy\"></span>"
        html_string += "</td>";
        html_string += "</tr>--%>";
        html_string += "<tr>";
        html_string += "<%--<td class=\"cell_caption\" align=\"left\" style=\"width: 22%\">";
        html_string += "Type</td>";
        html_string += "<td align=\"left\" style=\"background-color:aliceblue;height:20px;width:30%\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTType\"></span>";
        html_string += "</td>--%>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">Sent Via</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:175px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTSubType\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"text-align:right;width:175px;color:Black;\">";
        html_string += "Net Amount</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:88px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTGrossAmountCurrency\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:right;width:87px;\">";
        html_string += "<span id=\"spnNetAmount2\">0.00</span>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">Product Type</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:175px;\">";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTProductDescription\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"height:auto;text-align:right;width:175px;color:Black;\">Amount to be Received</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:left;width:88px;\"><b>";
        html_string += "&nbsp;:&nbsp;<span id=\"spnTNetToBenefAmountCurrency\"></span></b>";
        html_string += "</td>";
        html_string += "<td style=\"background-color:aliceblue;height:auto;text-align:right;width:87px;\"><b>";
        html_string += "<span id=\"spnTNetToBenefAmount\">0.00</span></b>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "</table>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "</table>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "<tr style=\"height:auto;\">";
        html_string += "<td style=\"width:350px;\">";
        html_string += "<span id=\"spnError\"></span>";
        html_string += "</td>";
        html_string += "<td style=\"text-align:right;width:350px;\">";
        html_string += "<table style=\"width:347px;\" cellpadding=\"0\" cellspacing=\"0\">";
        html_string += "<tr>";
        html_string += "<td nowrap style=\"height:auto; width:auto;\">";
        html_string += "<input id=\"btnPrintPanel\" type=\"button\" value=\"Print\" onclick=\"javascript:oreceipt.printDiv('divTransactionHistoryModal');\" style=\"width:110px; visibility:visible;color: black;font-size: 12px;cursor: hand;font-family: Tahoma;background-color: #eeedd9;border: solid 1px black;\" />";
        html_string += "<input id=\"btnClosePanel\" type=\"button\" value=\"Close\" onclick=\"javascript:oreceipt.closeDiv();\" style=\"width: 110px;color: black;font-size: 12px;cursor: hand;font-family: Tahoma;background-color: #eeedd9;border: solid 1px black;\" />";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "</table>";
        html_string += "</td>";
        html_string += "</tr>";
        html_string += "</table>";

        document.getElementById("divTransactionHistoryModal").innerHTML = html_string;

        this.header_receipt();
    }

    this.construct_online_receipt_div_content = function()
	{
        var content_div = null;

        this.inner_modal_div = document.createElement("div");
        this.inner_modal_div.align = "center";
        this.inner_modal_div.className = "inner_div";
        this.inner_modal_div.id = "inner_mod_div";
        this.inner_modal_div.style.height = "auto";
        this.inner_modal_div.style.width = parseInt(parent.screen.availWidth) - 250 + "px";
        this.inner_modal_div.style.top = "30px";
        this.inner_modal_div.style.left = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
        this.inner_modal_div.style.border = "medium solid white";
        this.inner_modal_div.Code = this;

        content_div = document.createElement("div");
        content_div.id = "divTransactionHistoryModal";
        content_div.align = "left";
        content_div.style.background = "transparent";
        content_div.style.width = "100%";
        content_div.style.overflow = "auto";
        content_div.innerHTML = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

        this.inner_modal_div.appendChild(content_div);

        document.body.appendChild(this.inner_modal_div);
    }

	this.close_online_receipt_modal_window = function()
	{
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if (this.outer_modal_div != null) { document.body.removeChild(this.outer_modal_div); this.outer_modal_div = null; }
        if (this.inner_modal_div != null) { document.body.removeChild(this.inner_modal_div); this.inner_modal_div = null; }
        if (this.content_iframe != null) { this.content_iframe = null; }

        document.body.style.margin = "";
    }

	this.open_online_receipt_modal_window = function()
	{
        oreceipt.load_online_receipt_modal_css();

        this.outer_modal_div = document.createElement("div");
        this.outer_modal_div.id = "outer_mod_div";
        this.outer_modal_div.className = "outer_div";

        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div").style.height = screen.height;

        oreceipt.construct_online_receipt_div_content();

        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }

	this.load_online_receipt_modal_css = function()
	{	    
        var file_reference = document.createElement("link");

        file_reference.setAttribute("rel", "stylesheet")
        file_reference.setAttribute("type", "text/css")
        file_reference.setAttribute("href", ".././css/cssmodals.css")

		if (typeof file_reference !="undefined")
		{		    
            document.getElementsByTagName("head")[0].appendChild(file_reference)
        }
    }

	this.printDiv = function(divName)
    {
        document.getElementById("btnPrintPanel").style.display = 'none';
        document.getElementById("btnClosePanel").style.display = 'none';

        var headstr = "<html><head><title></title></head><body>";
        var footstr = "</body>";
        var newstr = document.all.item(divName).innerHTML;
        var oldstr = document.body.innerHTML;

        document.body.innerHTML = headstr + newstr + footstr;
        window.print();
        document.body.innerHTML = oldstr;
        document.getElementById("btnPrintPanel").style.display = '';
        document.getElementById("btnClosePanel").style.display = '';
    }

    this.closeDiv = function()
    {        
        window.open('../Main/frmCenter.aspx', '_self');
    }
}

function internetClose()
{    
    remteller.card_payments.close_threedsecure_modal_window();
    showPanelOB('divpopProcess');
    new common_method().to_show_loading_message('divLoadingMsg');
    setText(document.getElementById('lblLoadingMsg'), 'Transaction is being processed. Please wait...');

    var response = self.returnValue;
    var arrResponse = "";

    if (response != null && response != "" && response != undefined && response != "|||" && response != "||||" && remteller.card_payments != null)
    {
        arrResponse = response.split("|");

        if (arrResponse[0] == "OK")
        {
            remteller.card_payments.auth_code = arrResponse[1];
            remteller.card_payments.response_all = response;

            document.getElementById('txtaResponse').value = response;

            remteller.card_payments.response = arrResponse[3];

            document.getElementById("btnCARDOK").disabled = "disabled";
            document.getElementById("btnCARDCANCEL").disabled = "disabled";
            document.getElementById("btnCARDCLOSE").disabled = "";

            hidePanelOB('divpopProcess');
        }
    }
}

function obp_receipt_closeure()
{
    if (remteller != null)
    {
        remteller.email_receipt();
    }
}

function otc_daily_log_closure()
{
    if (remteller != null)
    {
        remteller.print_receipt();
    }
}

function push_denomination_commit_response(param)
{
    if (remteller != null)
    {
        remteller.denomination_commit_response(param);
    }
}

function remote_override_result(result)
{
    if (remteller != null)
    {
        remteller.remote_override_response(result);
    }
}

function push_ach_payments_commit_response(param)
{
    if (remteller != null)
    {
        remteller.ach_payment_commit_result(param);
    }
}

function push_ach_payment_result()
{
    if (remteller != null)
    {
        remteller.chk_ach_pay_result();
    }
}

function click_calendar(objid,objt)
{
    popUpCalendar(objt, document.getElementById(objid), 'mm-dd-yyyy');
}

/* To Handle override when user is agent*/
function override_calculator_obps()
{
    var icls = this;

    this.login_result_flag = null;
    this.login_result_msgs = null;
    this.outer_modal_div = null;
    this.inner_modal_div = null;
    this.commons = null;

    this.irs_calculator_obps = null;
    this.array_override_data = null;
    this.irs_calculator_obps_session_name = null;
    this.array_override_joined = null;

    this.remitting_to_account = 0;
    this.account_to_remitting = 0;
    this.remitting_to_account_currency = '';
    this.account_to_remitting_currency = '';

    this.charges_rows = 0;

    this.open_override_form = function(irs_calculator_obps_session_name)
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Loading calculator for override value failed.");
        }

        this.irs_calculator_obps = IRSWorld.BillsPayment.OBPSAgentOverride.get_calculator_for_override_values(irs_calculator_obps_session_name).value;
        this.irs_calculator_obps_session_name = irs_calculator_obps_session_name;

        if (this.irs_calculator_obps != null)
        {
            this.open_modal_window();
            this.construct_override_window();
        }
    }

    this.construct_override_window = function()
    {
        var irs_calculator_obps_data = this.irs_calculator_obps;
        var html_string = "";

        if (irs_calculator_obps_data.Tables[0].Rows.length > 0)
        {
            this.remitting_to_account = irs_calculator_obps_data.Tables[0].Rows[0].remitting_to_account;
            this.remitting_to_account_currency = irs_calculator_obps_data.Tables[0].Rows[0].account_to_remitting_currency;
            this.account_to_remitting = irs_calculator_obps_data.Tables[0].Rows[0].account_to_remitting;
            this.account_to_remitting_currency = irs_calculator_obps_data.Tables[0].Rows[0].remitting_to_account_currency;
        }

        var remitting_to_account = "REMITTING TO ACCOUNT" + "</br>" + this.remitting_to_account_currency + " TO " + this.account_to_remitting_currency;
        var account_to_remitting = "ACCOUNT TO REMITTING" + "</br>" + this.account_to_remitting_currency + " TO " + this.remitting_to_account_currency;

        var txt_remitting_to_account_currency_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_remitting_to_account_currency_hidden' value = '" + this.remitting_to_account_currency + "'/>";
        var txt_account_to_remitting_currency_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_account_to_remitting_currency_hidden' value = '" + this.account_to_remitting_currency + "'/>";

        html_string += "<table style='border: solid 1px #336699; width:100%' cellspacing='1' cellpadding='0'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>" + txt_remitting_to_account_currency_hidden + remitting_to_account + "</td>";
        html_string += "        <td align = 'center'>" + txt_account_to_remitting_currency_hidden + account_to_remitting + "</td>";
        html_string += "    </tr>";
        html_string += "    <tr class = 'cell_desc' align = 'center'>";
        html_string += "        <td><input id = 'txt_currency_rate_remitting_to_account' type='text' style = 'text-align:center; width:auto' value = '" + this.remitting_to_account.toFixed(9) + "' /></td>";
        html_string += "        <td><input id = 'txt_currency_rate_account_to_remitting' type='text' style = 'text-align:center; width:auto' value = '" + this.account_to_remitting.toFixed(9) + "' /></td>";
        html_string += "    </tr>";
        html_string += "</table></br>";

        html_string += "<table id = 'tbl_override_charges_setup' style='border: solid 1px #336699; width:100%' cellspacing='1' cellpadding='0'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center' style = 'width: 30%'>CHARGE NAME</td>";
        html_string += "        <td align = 'center' style = 'width: 30%'>CURRENCY</td>";
        html_string += "        <td align = 'center' style = 'width: 30%'>AMOUNT</td>";
        html_string += "    </tr>"

        if (irs_calculator_obps_data != null)
        {
            this.charges_rows = irs_calculator_obps_data.Tables[1].Rows.length;

            for (var i = 0; i < irs_calculator_obps_data.Tables[1].Rows.length; i++)
            {
                var overrideflag = irs_calculator_obps_data.Tables[1].Rows[i].OverrideFlag;
                var txt_charge_name_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'hidden_charges_id_" + (i + 1) + "' value = '" + irs_calculator_obps_data.Tables[1].Rows[i].ChargeName + "'/>";
                var txt_charge_desc_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_charge_desc_hidden_" + (i + 1) + "' value = '" + irs_calculator_obps_data.Tables[1].Rows[i].ChargeNameDesc.toUpperCase() + "'/>";
                var txt_currency_desc_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'txt_currency_desc_hidden_" + (i + 1) + "' value = '" + irs_calculator_obps_data.Tables[1].Rows[i].CurrencyShortCode.toUpperCase() + "'/>";

                if (overrideflag == true) 
                {
                    var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + (1 * irs_calculator_obps_data.Tables[1].Rows[i].OverrideAmount).toFixed(2) + "'/>";
                }
                else
                {
                    var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + (1 * irs_calculator_obps_data.Tables[1].Rows[i].Amount).toFixed(2) + "'/>";
                }

                html_string += "    <tr  class = 'cell_desc'>";
                html_string += "        <td align = 'left'>" + txt_charge_desc_hidden + irs_calculator_obps_data.Tables[1].Rows[i].ChargeNameDesc.toUpperCase() + "</td>";
                html_string += "        <td align = 'center'>" + txt_currency_desc_hidden + irs_calculator_obps_data.Tables[1].Rows[i].CurrencyShortCode + "</td>";
                html_string += "        <td align = 'right'>" + txt_charge_name_hidden + txt_charge_name_txtbox + "</td>";
                html_string += "    </tr>"
            }
        }

        html_string += "</br><center><input id='btn_override_apply' style='width: 30%; height:24%' type='submit' value='APPLY CHANGES' />&nbsp<input id='btn_override_cancel' style='width: 30%; height:24%' type='submit' value='CANCEL' /></center></br>";

        document.getElementById("content_mod_div").innerHTML = html_string;

        document.getElementById("btn_override_apply").onclick = function()
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;

            AjaxPro.onTimeout = function()
            {
                alert("Server not responding. Checking user rights to override rates failed.");
            }

            IRSWorld.BillsPayment.OBPSAgentOverride.user_can_override_rates("", "", icls.override_callback);
        }

        document.getElementById("btn_override_cancel").onclick = function()
        {
            icls.close_modal_window();
        }

        document.getElementById("txt_currency_rate_remitting_to_account").onkeyup = function()
        {
            var rate = document.getElementById("txt_currency_rate_remitting_to_account").value;

            if (rate > 0)
            {                
                document.getElementById("txt_currency_rate_account_to_remitting").value = (1 / parseFloat(rate)).toFixed(9);
            }
            else
            {
                document.getElementById("txt_currency_rate_account_to_remitting").value = 0;
            }
        }

        document.getElementById("txt_currency_rate_account_to_remitting").onkeyup = function()
        {
            var rate = document.getElementById("txt_currency_rate_account_to_remitting").value;

            if (rate > 0)
            {            
                document.getElementById("txt_currency_rate_remitting_to_account").value = (1 / parseFloat(rate)).toFixed(9);
            }
            else
            {
                document.getElementById("txt_currency_rate_remitting_to_account").value = 0;
            }
        }

        document.getElementById("txt_currency_rate_remitting_to_account").onblur = function()
        {
            document.getElementById("txt_currency_rate_remitting_to_account").value = (1 / parseFloat(document.getElementById("txt_currency_rate_account_to_remitting").value)).toFixed(9);
            document.getElementById("txt_currency_rate_account_to_remitting").value = (1 / parseFloat(document.getElementById("txt_currency_rate_remitting_to_account").value)).toFixed(9);
        }

        document.getElementById("txt_currency_rate_account_to_remitting").onblur = function()
        {
            document.getElementById("txt_currency_rate_remitting_to_account").value = (1 / parseFloat(document.getElementById("txt_currency_rate_account_to_remitting").value)).toFixed(9);
            document.getElementById("txt_currency_rate_account_to_remitting").value = (1 / parseFloat(document.getElementById("txt_currency_rate_remitting_to_account").value)).toFixed(9);
        }

        irs_calculator_obps_data = null;
    }

    this.override_execute = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Checking user rights to override rates failed.");
        }

        IRSWorld.BillsPayment.OBPSAgentOverride.user_can_override_rates("", "", icls.override_callback);
    }

    this.override_callback = function(result)
    {
        if (result.value != null && result.value.length > 0)
        {
            icls.login_result_flag = result.value[0];
            icls.login_result_msgs = result.value[1];

            if (result.value[0] == false)
            {
                alert(result.value[1]); return;
            }
            else
            {
                icls.parse_data_changes();
                icls.send_parsed_data();
            }
        }
    }

    this.send_parsed_data = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;

        AjaxPro.onTimeout = function()
        {
            alert("Server not responding. Sending for override values failed.");
        }

        IRSWorld.BillsPayment.OBPSAgentOverride.set_calculator_obps_override_values(icls.array_override_joined, icls.irs_calculator_obps_session_name, this.send_parsed_data_callback);
    }

    this.send_parsed_data_callback = function(result)
    {
        if (result != null)
        {
            eval("javascript:push_override_result(icls);");
        }
    }

    this.parse_data_changes = function()
    {
        if (this.irs_calculator_obps != null)
        {
            this.array_override_data = new Array();

            if (this.irs_calculator_obps.Tables[0].Rows.length > 0)
            {
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "remitting_to_account_rate" + "~" + document.getElementById("txt_currency_rate_remitting_to_account").value
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "account_to_remitting_rate" + "~" + document.getElementById("txt_currency_rate_account_to_remitting").value
            }

            if (this.irs_calculator_obps.Tables[1].Rows.length > 0)
            {
                var contents = document.getElementById("tbl_override_charges_setup");

                for(var i = 0; i < contents.rows.length; i++)
                {
                    if (i > 0)
                    {
                        this.array_override_data[this.array_override_data.length] = "CHARGES_DATA" + "~" + document.getElementById("hidden_charges_id_" + i).value + "~" + document.getElementById("txt_charges_value_" + i).value;
                    }
                }
            }

            this.array_override_joined = this.array_override_data.join("|");
        }
    }

    this.open_modal_window = function()
    {
        this.load_modal_css();
        this.outer_modal_div = document.createElement("div");
        this.outer_modal_div.id = "outer_mod_div";
        this.outer_modal_div.className = "outer_div";

        document.body.style.margin = "0px";
        document.body.appendChild(this.outer_modal_div);
        document.getElementById("outer_mod_div").style.height = screen.height;

        this.construct_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
    }

    this.close_modal_window = function()
    {
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

        if (this.outer_modal_div != null) { document.body.removeChild(this.outer_modal_div); this.outer_modal_div = null; }
        if (this.inner_modal_div != null) { document.body.removeChild(this.inner_modal_div); this.inner_modal_div = null; }

        document.body.style.margin = "";
    }

    this.construct_div_content = function()
    {
        var content_div = null;

        content_div = document.createElement("div");
        content_div.id = "content_mod_div";
        content_div.align = "left";
        content_div.style.background = "transparent";
        content_div.style.width = "95%";
        content_div.innerHTML = "<center><img src = '.././images/ajaxloader.gif'></img></center>";

        this.inner_modal_div = document.createElement("div");
        this.inner_modal_div.align = "center";
        this.inner_modal_div.className = "inner_div";
        this.inner_modal_div.id = "inner_mod_div";
        this.inner_modal_div.style.height = "auto";
        this.inner_modal_div.style.width = "500px";
        this.inner_modal_div.style.top = "30px";
        this.inner_modal_div.style.left = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10 + "px";
        this.inner_modal_div.style.border = "medium solid white";
        this.inner_modal_div.appendChild(document.createElement("br"));
        this.inner_modal_div.appendChild(content_div);

        document.body.appendChild(this.inner_modal_div);
    }

    this.load_modal_css = function()
    {	    
        var file_reference = document.createElement("link");

        file_reference.setAttribute("rel", "stylesheet")
        file_reference.setAttribute("type", "text/css")
        file_reference.setAttribute("href", ".././css/cssmodals.css")

	    if (typeof file_reference !="undefined")
	    {		    
            document.getElementsByTagName("head")[0].appendChild(file_reference);
        }
    }

    window.onscroll = function () 
    {		
        var i = 0;
        var m = 0;

        var divelement_outer = document.documentElement.getElementsByTagName("div");
        var divelement_inner = document.documentElement.getElementsByTagName("div");

	    for(i = 0; i < divelement_outer.length;i++)
	    {
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        {
                divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop; break;
            }
        }
	    for(m = 0; m < divelement_inner.length; m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
                divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20; break;
            }
        }
    }
}

function agent_remittance()
{
    var agr = this;
    var _RemitterID = "";
    var _RemitterName = "";
    var _RemitterBirthDate = null;
    var _RemitterEmployer = "";
    var _RemitterOccupation = 0;
    var _RemitterOccupationDesc = "";
    var _RemitterSalary = 0;
    var _RemitterSalaryCcy = 0;
    var _SourceOfFunds = 0;
    var _WorkerType = 0;
    var _BeneficaryID = "";
    var _BeneficiaryName = "";
    var _Relationship = 0;
    var _IDSEQID = 0;
    var _IDNo = "";
    var _IDType = 0;
    var _IDClass = 0;
    var _IDExpirydate = null;
    var _RemitterInfoComplete = false;
    var _RemitterIDsComplete = false;
    var _RemitterInfoMessage = "";
    var _RemitterIDsMessage = "";
    var _aeIDMode = false;//true-Add Mode, false-Edit Mode
    var _ddlfldgroup = "";
    var _fillDLL = 0;

    this.loadSystemControl = function()
    {
        remteller.cleanDDL_2("ddlRemInfoOccupation");
        remteller.cleanDDL_2("ddlRemInfoSourceOfFunds");
        remteller.cleanDDL_2("ddlRemInfoIDType");
        remteller.cleanDDL_2("ddlRemInfoIDClass");

        agr._fillDLL = 0;
        agr.populateDDL("Occupation");
        agr.populateDDL("SourceOfFunds");
        agr.populateDDL("IDType");
        agr.populateDDL("IDClassification");
    }

    this.getRemitterInfo = function(remitterID, beneficiaryID)
    {
        hidePanelOB("divRemitterInfo");
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');
        setText(document.getElementById('lblLoadingMsg'), 'Acquiring Remitter Info.....');
        IRSWorld.BillsPayment.OBPSTransactions.getSystemControl_RemitterInfo(remitterID, beneficiaryID, agr.getRemitterInfo_callback)
    }

    this.getRemitterInfo_callback = function(result)
    {
        var response = result.value;

        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            agr._RemitterID = response.Rows[0].RemitterCIFID;
            agr._RemitterName = response.Rows[0].RemitterFullName;
            agr._RemitterBirthDate = response.Rows[0].RemitterBirthDate;
            agr._RemitterEmployer = response.Rows[0].Employer;
            agr._RemitterOccupation = response.Rows[0].Occupation;
            agr._RemitterOccupationDesc = response.Rows[0].DetailedDesc;
            agr._RemitterSalary = response.Rows[0].Salary;
            agr._RemitterSalaryCcy = response.Rows[0].Currency;
            agr._SourceOfFunds = response.Rows[0].SourceofFunds;
            agr._WorkerType = response.Rows[0].WorkerType;

            agr.setRemitterInfo();
            agr.checkRemitterInfoReq();
        }
    }

    this.setRemitterInfo = function()
    {
        setText(document.getElementById('spnRemInfoCIFID'), agr._RemitterID);
        setText(document.getElementById('spnRemInfoRemitterName'), agr._RemitterName);
        setText(document.getElementById('txtRemInfoBirthdate'), agr._RemitterBirthDate);
        document.getElementById('ddlRemInfoOccupation').value = agr._RemitterOccupation;
        setText(document.getElementById('txtRemInfoDetailedOccupation'), agr._RemitterOccupationDesc);
        document.getElementById('ddlRemInfoSourceOfFunds').value = agr._SourceOfFunds;
    }

    this.populateDDL = function(fldgroup)
    {
        return IRSWorld.BillsPayment.OBPSTransactions.get_SubLookup(fldgroup, agr.populateDDL_callback);
    }

    this.populateDDL_callback = function(result)
    {
        var response = result.value;
        var DDL = null;

        if (response != null && response != "" && response != undefined)
        {
            switch (response.Tables[0].Rows[0].FldGroup.toString().toUpperCase())
            {
                case "OCCUPATION":
                    DDL = document.getElementById('ddlRemInfoOccupation');
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "SOURCEOFFUNDS":
                    DDL = document.getElementById('ddlRemInfoSourceOfFunds');
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "WORKERTYPE":
                    DDL = document.getElementById('ddlRemInfoWorkerType');
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "CURRENCY":
                    DDL = document.getElementById('ddlRemInfoSalaryCcy');
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "RELATIONSHIP":
                    break;
                case "IDTYPE":
                    DDL = document.getElementById('ddlRemInfoIDType');
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
                case "IDCLASSIFICATION":
                    DDL = document.getElementById('ddlRemInfoIDClass');
                    agr._fillDLL = agr._fillDLL + 1;
                    break;
            }

            if (DDL != null)
            {
                if (response != null && response.Tables[0].Rows.length > 0)
                {            
                    for (var i = 0; i < response.Tables[0].Rows.length; i++)
                    {
                        if (i == 0)
                        {
                            var opt2 = document.createElement("option");
                            opt2.text = '';
                            opt2.value = '';
                            DDL.options.add(opt2);
                        }

                        var opt = document.createElement("option");
                        opt.text = response.Tables[0].Rows[i].FldDescription.toString();
                        opt.value = response.Tables[0].Rows[i].FldValue.toString();
                        DDL.options.add(opt);
                    }
                }
            }
        }

        if (agr._fillDLL >= 4)
        {
            remteller.checkSysCrtl_RemitterInfo();
        }
    }

    this.checkRemitterInfoReq = function()
    {
        var isRequiredempty = 0;

        agr._RemitterInfoMessage = "Please fill up the required fields: \r\n";

        if (getText(document.getElementById('txtRemInfoBirthdate')) == "" || getText(document.getElementById('txtRemInfoBirthdate')) == "01/01/1900")
        {
            isRequiredempty = isRequiredempty + 1;
            document.getElementById("mnBirthDate").style.display = "";
            agr._RemitterInfoMessage += "BirthDate \t\n";
        }
        else
        {
            document.getElementById("txtRemInfoBirthdate").disabled = true;
            document.getElementById("imgBirthDate").disabled = true;
        }

        if(document.getElementById("ddlRemInfoOccupation").selectedIndex <= 0)
        {
            isRequiredempty = isRequiredempty + 1;
            document.getElementById("mnOccupation").style.display = "";
            agr._RemitterInfoMessage += "Occupation \t\n";
        }
        else
        {
            document.getElementById("ddlRemInfoOccupation").disabled = true;
        }

        if(document.getElementById("ddlRemInfoSourceOfFunds").selectedIndex <= 0)
        {
            isRequiredempty = isRequiredempty + 1;
            document.getElementById("mnSourceOfFunds").style.display = "";
            agr._RemitterInfoMessage += "Source Of Funds \t\n";
        }
        else
        {
            document.getElementById("ddlRemInfoSourceOfFunds").disabled = true;
        }

        hidePanelOB('divpopProcess');
        showPanelOB('divRemitterInfo');

        if (isRequiredempty >= 1)
        {
            alert(agr._RemitterInfoMessage + "\r\n \r\n" + agr._RemitterIDsMessage);
            document.getElementById("btnRemitterInfoSave").disabled = false;
            document.getElementById("btnRemitterInfoCancel").disabled = false;

            setText(document.getElementById('btnRemitterInfoCancel'), 'Cancel');
        }
        else
        {
            document.getElementById("btnRemitterInfoSave").disabled = true;
            document.getElementById("btnRemitterInfoCancel").disabled = false;
            setText(document.getElementById('btnRemitterInfoCancel'), 'Edit');

            if (agr._RemitterIDsComplete != true)
            {
                alert(agr._RemitterIDsMessage);
            }

            agr._RemitterInfoMessage = "";
            agr._RemitterIDsMessage = "";
        }

    }

    this.editSysControl_RemitterInfo = function()
    {
        if (getText(document.getElementById('btnRemitterInfoCancel')) == "Edit")
        {
            document.getElementById("btnRemitterInfoSave").disabled = false;
            document.getElementById("btnRemitterInfoCancel").disabled = false;
            setText(document.getElementById('btnRemitterInfoCancel'), 'Cancel');

            document.getElementById("txtRemInfoBirthdate").disabled = false;
            document.getElementById("imgBirthDate").disabled = false;
            document.getElementById("ddlRemInfoOccupation").disabled = false;
            document.getElementById("ddlRemInfoSourceOfFunds").disabled = false;
            document.getElementById("txtRemInfoDetailedOccupation").disabled = false;
        }
        else if (getText(document.getElementById('btnRemitterInfoCancel')) == "Cancel")
        {
            document.getElementById("btnRemitterInfoSave").disabled = true;
            document.getElementById("btnRemitterInfoSave").disabled = false;
            setText(document.getElementById('btnRemitterInfoCancel'), 'Edit');

            document.getElementById("txtRemInfoBirthdate").disabled = true;
            document.getElementById("imgBirthDate").disabled = true;
            document.getElementById("ddlRemInfoOccupation").disabled = true;
            document.getElementById("ddlRemInfoSourceOfFunds").disabled = true;
            document.getElementById("txtRemInfoDetailedOccupation").disabled = true;
            agr.setRemitterInfo();
        }
    }

    this.saveSysControl_RemitterInfo = function()
    {
        if (getText(document.getElementById('btnRemitterInfoCancel')) == "Edit")
        {
            document.getElementById("btnRemitterInfoSave").disabled = false;
            document.getElementById("btnRemitterInfoCancel").disabled = false;
            setText(document.getElementById('btnRemitterInfoCancel'), 'Cancel');
        }
        else if (getText(document.getElementById('btnRemitterInfoCancel')) == "Cancel")
        {
            agr.checkRemitterInfoReq();

            if (agr._RemitterInfoMessage == "")
            {
                document.getElementById("btnRemitterInfoSave").disabled = true;
                document.getElementById("btnRemitterInfoCancel").disabled = false;
                setText(document.getElementById('btnRemitterInfoCancel'), 'Edit');

                document.getElementById("txtRemInfoBirthdate").disabled = true;
                document.getElementById("imgBirthDate").disabled = true;
                document.getElementById("ddlRemInfoOccupation").disabled = true;
                document.getElementById("ddlRemInfoSourceOfFunds").disabled = true;
                document.getElementById("txtRemInfoDetailedOccupation").disabled = true;

                hidePanelOB("divRemitterInfo");
                showPanelOB('divpopProcess');

                new common_method().to_show_loading_message('divLoadingMsg');
                setText(document.getElementById('lblLoadingMsg'), 'Saving Required Remitter Info.....');
                IRSWorld.BillsPayment.OBPSTransactions.saveSystemControl_RemitterInfo(agentRem.setvalues_RemitterInfo(), agentRem.saveSystemControl_RemitterInfo_callback);
            }
            else
            {
                alert(agr._RemitterInfoMessage);
            }
        }
    }

    this.saveSystemControl_RemitterInfo_callback = function(result)
    {
        var response = result.value;
        hidePanelOB('divpopProcess');
        showPanelOB("divRemitterInfo");

        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            if (response != -1)
            {
                alert("Remitter Info successfully saved.");
                agr.setRemitterInfo();
            }
            else
            {
                alert("Failed to save Remitter Info");
            }
        }
    }

    this.setvalues_RemitterInfo = function()
    {
        var sReturn = "";

        agr._RemitterID = getText(document.getElementById('spnRemInfoCIFID'));
        agr._RemitterName = getText(document.getElementById('spnRemInfoRemitterName'));
        agr._RemitterBirthDate = getText(document.getElementById('txtRemInfoBirthdate'));
        agr._RemitterOccupation = document.getElementById("ddlRemInfoOccupation").value;
        agr._RemitterOccupationDesc = getText(document.getElementById('txtRemInfoDetailedOccupation'));
        agr._SourceOfFunds = document.getElementById("ddlRemInfoSourceOfFunds").value;

        sReturn = agr._RemitterID + "~";
        sReturn += agr._RemitterName + "~";
        sReturn += agr._RemitterBirthDate + "~";
        sReturn += agr._RemitterEmployer + "~";
        sReturn += agr._RemitterOccupation + "~";
        sReturn += agr._RemitterOccupationDesc + "~";
        sReturn += agr._SourceOfFunds + "~";
        sReturn += ((agr._WorkerType == "") ? "0" : agr._WorkerType) + "~";
        sReturn += ((agr._RemitterSalary == "" || agr._RemitterSalary == "null" || agr._RemitterSalary == null) ? "0" : agr._RemitterSalary) + "~";
        sReturn += ((agr._RemitterSalaryCcy == "" || agr._RemitterSalaryCcy == "null" || agr._RemitterSalaryCcy == null) ? "0" : agr._RemitterSalaryCcy) + "~~~0";

        return sReturn;
    }

    this.getSystemControl_RemitterID = function(remitterid, seqid)
    {
        hidePanelOB("divRemitterInfo");
        showPanelOB('divpopProcess');

        new common_method().to_show_loading_message('divLoadingMsg');

        setText(document.getElementById('lblLoadingMsg'), 'Acquiring Required Remitter IDs.....');
        IRSWorld.BillsPayment.OBPSTransactions.getSystemControl_RemitterIDs(remitterid, seqid.toString(), agr.getSystemControl_RemitterID_callback)
    }

    this.getSystemControl_RemitterID_callback = function(result)
    {
        var response = result.value;

        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            agr._IDSEQID = response.Rows[0].SEQ_ID;
            agr._IDNo = response.Rows[0].IDNo;
            agr._IDType = response.Rows[0].IdType;
            agr._IDClass = response.Rows[0].IDClassification;
            agr._IDExpirydate = response.Rows[0].ExpiryDate;
            agr.setRemitterIDs();
        }
    }

    this.setRemitterIDs = function()
    {        
        hidePanelOB('divpopProcess');
        showPanelOB("divRemitterInfo");

        setText(document.getElementById('txtRemInfoIDNo'), agr._IDNo);

        document.getElementById('ddlRemInfoIDType').value = agr._IDType;
        document.getElementById('ddlRemInfoIDClass').value = agr._IDClass;

        setText(document.getElementById('txtRemInfoExpiryDate'), agr._IDExpirydate);

        document.getElementById("btnRemInfoEditID").disabled = false;
    }

    this.clearRemitterIDs = function()
    {
        agr._IDSEQID = 0;
        agr._IDNo = "";
        agr._IDType = 0;
        agr._IDClass = 0;
        agr._IDExpirydate = null;

        setText(document.getElementById('txtRemInfoIDNo'), '');
        setText(document.getElementById('txtRemInfoExpiryDate'), '');

        document.getElementById("ddlRemInfoIDType").selectedIndex = -1;
        document.getElementById("ddlRemInfoIDClass").selectedIndex = -1;
    }

    this.addSystemControl_RemitterIDs = function()
    {
        document.getElementById("btnRemInfoAddID").disabled = true;
        document.getElementById("btnRemInfoEditID").disabled = true;
        document.getElementById("btnRemInfoSaveID").disabled = false;
        document.getElementById("btnRemInfoCancelID").disabled = false;

        document.getElementById("ddlRemInfoIDType").disabled = false;
        document.getElementById("txtRemInfoIDNo").disabled = false;
        document.getElementById("imgIDExpiryDate").disabled = false;
        document.getElementById("ddlRemInfoIDClass").disabled = false;

        agr._aeIDMode = true;
    }

    this.editSystemControl_RemitterIDs = function()
    {
        document.getElementById("btnRemInfoAddID").disabled = true;
        document.getElementById("btnRemInfoEditID").disabled = true;
        document.getElementById("btnRemInfoSaveID").disabled = false;
        document.getElementById("btnRemInfoCancelID").disabled = false;

        document.getElementById("ddlRemInfoIDType").disabled = false;
        document.getElementById("txtRemInfoIDNo").disabled = false;
        document.getElementById("imgIDExpiryDate").disabled = false;
        document.getElementById("ddlRemInfoIDClass").disabled = false;

        agr._aeIDMode = false;
    }

    this.saveSystemControl_RemitterIDs = function()
    {
        document.getElementById("btnRemInfoAddID").disabled = false;
        document.getElementById("btnRemInfoEditID").disabled = true;
        document.getElementById("btnRemInfoSaveID").disabled = true;
        document.getElementById("btnRemInfoCancelID").disabled = true;

        document.getElementById("ddlRemInfoIDType").disabled = true;
        document.getElementById("txtRemInfoIDNo").disabled = true;
        document.getElementById("imgIDExpiryDate").disabled = true;
        document.getElementById("ddlRemInfoIDClass").disabled = true;

        hidePanelOB("divRemitterInfo");
        showPanelOB('divpopProcess');
        new common_method().to_show_loading_message('divLoadingMsg');

        setText(document.getElementById('lblLoadingMsg'), 'Saving Required Remitter IDs.....');

        if (agr._aeIDMode == true)
        {
            agr._IDSEQID = 0;
            IRSWorld.BillsPayment.OBPSTransactions.saveSystemControl_RemitterIDs(agentRem.setvalues_RemitterIDs(), agentRem.saveSystemControl_RemitterIDs_callback);
        }
        else
        {
            IRSWorld.BillsPayment.OBPSTransactions.saveSystemControl_RemitterIDs(agentRem.setvalues_RemitterIDs(), agentRem.saveSystemControl_RemitterIDs_callback);
        }

        agr.clearRemitterIDs();
    }

    this.cancelSystemControl_RemitterIDs = function()
    {
        document.getElementById("btnRemInfoAddID").disabled = false;
        document.getElementById("btnRemInfoEditID").disabled = true;
        document.getElementById("btnRemInfoSaveID").disabled = true;
        document.getElementById("btnRemInfoCancelID").disabled = true;

        document.getElementById("ddlRemInfoIDType").disabled = true;
        document.getElementById("txtRemInfoIDNo").disabled = true;
        document.getElementById("imgIDExpiryDate").disabled = true;
        document.getElementById("ddlRemInfoIDClass").disabled = true;

        agr.clearRemitterIDs();
    }

    this.setvalues_RemitterIDs = function()
    {
        var sReturn = "";

        agr._RemitterID = getText(document.getElementById('spnRemInfoCIFID'));
        agr._IDNo = getText(document.getElementById('txtRemInfoIDNo'));
        agr._IDType = document.getElementById("ddlRemInfoIDType").value;
        agr._IDClass = document.getElementById("ddlRemInfoIDClass").value;
        agr._IDExpirydate = getText(document.getElementById('txtRemInfoExpiryDate'));

        sReturn = agr._RemitterID + "~";
        sReturn += agr._IDNo + "~";
        sReturn += agr._IDType + "~";
        sReturn += agr._IDClass + "~";
        sReturn += ((agr._IDExpirydate == "") ? "01/01/1900" : agr._IDExpirydate) + "~";
        sReturn += agr._IDSEQID;

        return sReturn;
    }

    this.saveSystemControl_RemitterIDs_callback = function(result)
    {
        var response = result.value;
        hidePanelOB('divpopProcess');
        showPanelOB("divRemitterInfo");

        if (response != null && response != "null" && response != undefined && response != "undefined")
        {
            if(response != -1)
            {
                alert("Remitter ID successfully saved.");
                remteller.checkAgentSystemControl();
            }
            else
            {
                alert("Failed to save Remitter ID");
            }
        }
    }
}