function rem_commons()
{
    this.charge_locally                    = false;
    this.txn_save                          = null;
    this.iframe_modal                      = null;
    this.computing_method                  = null;
    this.computing_method_desc             = null;
    this.remitting_amount                  = null;            
    this.remitter_id                       = null;
    this.remitter_fullname                 = null;
    this.remitter_address                  = null;    
    this.beneficiary_id                    = null;
    this.beneficiary_fullname              = null;
    this.beneficiary_address               = null;
    this.beneficiary_account_branch_name   = null;
    this.beneficiary_account_type_desc     = null;
    this.beneficiary_account_currency_desc = null;
    this.beneficiary_account_number        = null;
    this.beneficiary_gfmc_card_number      = null;    
    this.remittance_txn_id                 = null;
    this.transaction_status                = null;
    this.account_seq_id                    = null;    
    this.account_currency                  = null;    
    this.product_charge_id                 = null;
    this.product_code                      = null;
    this.product_code_desc                 = null;
    this.implementation_code               = null;
    this.implementation_code_desc          = null;    
    this.remitting_branch_code             = null;
    this.remitting_branch_code_desc        = null;
    this.paymode_code                      = null;
    this.paymode_desc                      = null;
    this.charge_type_code                  = null;
    this.charge_type_desc                  = null;    
    this.purpose                           = null;
    this.transaction_source                = null;
    this.source_of_fund                    = null;    
    this.purpose_desc                      = null;
    this.transaction_source_desc           = null;
    this.source_of_fund_desc               = null;
    this.transaction_status_desc           = null;
    this.other_purpose_desc                = null;
    this.special_msg_desc                  = null;
    this.receipt_mode                      = null;    
    this.grd_search                        = null;
    this.grd_search_keyword                = "BENEFICIARYACCOUNT";
    this.grid                              = null;
    this.calculator_breakdown              = null;
    this.lookup_table_description          = null;
    
    var icls = this;

    this.load_remittance_screen_information = function()
    {
        RemTranCommons.get_rem_screen_information(this.remitter_id, this.beneficiary_id, this.load_remittance_screen_information_callback);
    }
    
    this.load_remittance_screen_information_callback = function(result)
    {            
        var ds = null;
            
        if (result != null || result.value != null)
        {
            ds = result.value;
        }
                
        if (ds != null)
        {
            if (ds.Tables[0].Rows.length > 0)
            {
                for(var i = 0; i < ds.Tables[0].Rows.length; i++)
                {
                    var cif_class = ds.Tables[0].Rows[i].cifclass;
                    
                    if (cif_class == "REMITTER")
                    {
                        icls.remitter_fullname = ds.Tables[0].Rows[i].fullname;
                        icls.remitter_address  = ds.Tables[0].Rows[i].address;
                    }
                    else if (cif_class == "BENEFICIARY")
                    {
                        icls.beneficiary_fullname = ds.Tables[0].Rows[i].fullname;
                        icls.beneficiary_address  = ds.Tables[0].Rows[i].address;                        
                    }
                }
                this.value = icls;
                
                icls.get_product_and_implementation();
            }
        }        
    }
        
    this.get_remittance_account_information = function(s)
    {
        var splitted = s.split("~");
        
        this.account_seq_id = splitted[0];
        this.account_currency = splitted[5];
        this.beneficiary_account_number        = splitted[1];
        this.beneficiary_account_branch_name   = splitted[2];
        this.beneficiary_account_type_desc     = splitted[3];
        this.beneficiary_account_currency_desc = splitted[4];

        this.set_transaction_values("ACCOUNTSEQID",      this.account_seq_id);
        this.set_transaction_values("NETTOBENECURRENCY", this.account_currency);
    }
    
    this.get_beneficiary_account_info = function(seq_id)
    {
        if (seq_id != null)
        {
            var ds = RemTranCommons.get_cif_account_data(seq_id).value;
            
            if (ds != null)
            {
                if (ds.Tables[0].Rows.length > 0)
                {
                    this.account_seq_id = seq_id;
                    this.beneficiary_account_number        = ds.Tables[0].Rows[0].accountno;
                    this.beneficiary_account_type_desc     = ds.Tables[0].Rows[0].accounttype_desc;
                    this.beneficiary_account_currency_desc = ds.Tables[0].Rows[0].currency_desc;
                    this.beneficiary_account_branch_name   = ds.Tables[0].Rows[0].acct_branch_desc;
                    this.account_currency                  = ds.Tables[0].Rows[0].currency;
                }
            }
        }
    }

    this.get_product_and_implementation = function()
    {
        RemTranCommons.get_remittance_product_charges(this.product_charge_id, this.get_product_and_implementation_callback);
    }
    
    this.get_product_and_implementation_callback = function(result)
    {
        var ds = null;
        
        if (result != null || result.value != null)
        {
            ds = result.value;
        }

        if (ds != null)
        {
            if (ds.Tables[0].Rows.length > 0)
            {
                icls.product_code               = ds.Tables[0].Rows[0].ProductCode;
                icls.product_code_desc          = ds.Tables[0].Rows[0].ProductCodeDesc;
                icls.implementation_code        = ds.Tables[0].Rows[0].ImplementationType;
                icls.implementation_code_desc   = ds.Tables[0].Rows[0].ImplementationTypeDesc;
                icls.remitting_branch_code      = ds.Tables[0].Rows[0].BranchCode;
                icls.remitting_branch_code_desc = ds.Tables[0].Rows[0].BranchName;
                icls.remitting_currency         = ds.Tables[0].Rows[0].Currency;
                icls.computing_method           = ds.Tables[0].Rows[0].ComputationLevel;
                icls.computing_method_desc      = ds.Tables[0].Rows[0].ComputationLevelDesc;
                icls.paymode_code               = ds.Tables[0].Rows[0].PayMode;
                icls.paymode_desc               = ds.Tables[0].Rows[0].PayModeDesc;
                icls.charge_type_code           = ds.Tables[0].Rows[0].ChargeType;
                icls.charge_type_desc           = ds.Tables[0].Rows[0].ChargeTypeDesc;
                icls.transaction_status         = ds.Tables[0].Rows[0].DefaultStatus;
                icls.transaction_status_desc    = ds.Tables[0].Rows[0].DefaultStatusDesc;
                
                this.value = icls;
            }
        }                    
    }

    this.get_transaction_module_query_string = function()
    {
        var i_qstring       = new getQueryStrings(null);        
        var i_qstring_value = null;
        
        if (i_qstring != null && i_qstring.length > 0)
        {
            for(var i = 0; i <  i_qstring.length; i++)
            {
                if (i_qstring[i][1] != null && i_qstring[i][1].length > 0 && i_qstring[i][1] != '')
                {
                    i_qstring_value = i_qstring[i][1];
                }
                switch(i_qstring[i][0].toUpperCase())
                {
                    case "REMITTER_ID"       : this.remitter_id       = i_qstring_value; break;
                    case "BENEFICIARY_ID"    : this.beneficiary_id    = i_qstring_value; break;
                    case "PRODUCT_CHARGE_ID" : this.product_charge_id = i_qstring_value; break;
                    case "REMITTANCE_TXN_ID" : this.remittance_txn_id = i_qstring_value; break;
                }
            }
            
//            if (this.remittance_txn_id != null && this.remittance_txn_id != "" && this.remittance_txn_id > 0)
//            {
//                this.get_record_from_remittance_table();
//                this.get_beneficiary_account_info(this.account_seq_id);
//            }
            
            if (this.product_charge_id != null && this.product_charge_id != "" && this.product_charge_id > 0)
            {
                this.load_remittance_screen_information();
                                
                if (this.remittance_txn_id == null || this.remittance_txn_id == '')
                {
                    this.load_content_to_cls_saving();
                }
            }
        }
    }
    
    this.get_side_bar_visibility = function(division_id, anchor_id)
    {
        var div  = document.getElementById(division_id);
        var cset = div.style.display;
                
        div.style.display = (cset == "block") ? "none" : "block";
                
        document.getElementById(anchor_id).innerText = (cset == "block") ? String.fromCharCode(187) : String.fromCharCode(171);
        document.getElementById('td_remittance').style.width = (cset == "block") ? "0%" : "18%";
    }

    this.get_side_bar = function()
    {
        var side_array  = this.get_side_bar_list();
        var side_winder = new js_sidebar('sidemenu', side_array);
        
        return side_winder.toString();
    }

    this.get_side_bar_list = function()
    {
        var sidebar;        
                
        if (new common_method().is_numeric(this.remittance_txn_id) == true)
        {        
            sidebar = new Array
            (
                new Array(89,0,'Charge Details',     'javascript:open_compute_charges();'),
                new Array(90,0,'Transaction Details','javascript:open_transaction_details();'),
                new Array(99,0,'Back To Search',     'javascript:back_to_search();')                
            );         
            return sidebar;
        }        
    
        sidebar = new Array
        (
            new Array(2,0,'Purpose',            'javascript:choose_lookup_data_grid_window("TranxPurpose");'),
            new Array(3,0,'Souce of Fund',      'javascript:choose_lookup_data_grid_window("SourceOfFunds");'),
            new Array(4,0,'Transaction Source', 'javascript:choose_lookup_data_grid_window("TranxSource");')
        );
             
        if (this.product_code != null)
        {
            if (this.product_code != 22)
            {
                if (this.implementation_code == 2 || this.implementation_code == 4 || this.implementation_code == 5 || this.implementation_code == 11 || this.implementation_code == 12)
                {
                    sidebar[sidebar.length] = new Array(90, 0, 'Select Account No.', 'javascript:choose_account_grid_window();');
                }
            }
            else
            {
                sidebar[sidebar.length] = new Array(90, 0, 'Purchase/Avail GFMC', 'javascript:choose_account();');
            }
        }

        sidebar[sidebar.length] = new Array(94,0, 'Compute Charges',     'javascript:open_compute_charges();');
        sidebar[sidebar.length] = new Array(95,0, 'Override Rates',      'javascript:open_override_window();');
        sidebar[sidebar.length] = new Array(96,0, 'Transaction Details', 'javascript:open_transaction_details();');

        return sidebar;
    }
    
    this.get_compute_method = function()
    {
        this.grd_search_keyword = "COMPUTE_METHOD";
        
        var grd_container  = null;
        var grd_source     = "sp_txn_rem_comp_method~" + this.remitting_branch_code + "~" + this.product_code + "~" + this.implementation_code + "~" + this.remitting_currency + "~" + this.charge_type_code + "~" + this.paymode_code;
        
        this.grd_search = new grid_selector();
        this.grd_search.open_selection_grid("../AjxPage/ajaxgrid.aspx", grd_source, 0 , grd_container);
    }
    
    this.get_remittance_lookup_data = function(field_group)
    {        
        this.grd_search_keyword = field_group;    
        
        var grd_container  = null;
        var grd_source     = "SELECT FldValue, UPPER(FldDescription) AS [DESCRIPTION] FROM V_TBL_LOOKUP_PER_BRANCH WHERE FldGroup = '" + field_group + "'";
        var grd_page       = "../AjxPage/ajaxgrid.aspx";

        this.grd_search = new grid_selector();
        this.grd_search.open_selection_grid(grd_page, grd_source, 0 , grd_container);
    }        
    
    this.get_account_selector = function()
    {
        this.grd_search_keyword = "BENEFICIARYACCOUNT";
    
        var grd_source    = null;
        var grd_container = null;

        if (this.product_code == 22)
        {
            alert("GMFC - MODULE TO BE CALLED");
        }
        else
        {
            switch(this.implementation_code)
            {
                case 2  : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~1"; break; // PNB CREDIT TO ACCOUNT
                case 4  : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~2"; break; // OTHER BANKS PDDTS / EPCS
                case 5  : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~3"; break; // OTHER BANKS NON PDDTS / EPCS
                case 11 : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~1"; break; // RAPIT REMIT
                case 12 : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~1"; break; // CREDIT TO SWIFT
            }
        }
    
        if (grd_source != null)
        {
            this.grd_search = new grid_selector();
            this.grd_search.open_selection_grid("../AjxPage/ajaxgrid.aspx", grd_source, 0 , grd_container);
        }
    }
    
    this.get_account_selector_for_grid_window = function(grd_container)
    {
        this.grd_search_keyword = "BENEFICIARYACCOUNT";                                
                        
        var grd_source    = null;
        try
        {
            if (this.product_code != 22)
            {
                switch(this.implementation_code)
                {
                    case 2  : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~1"; break; // PNB CREDIT TO ACCOUNT
                    case 4  : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~2"; break; // OTHER BANKS PDDTS / EPCS
                    case 5  : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~3"; break; // OTHER BANKS NON PDDTS / EPCS
                    case 11 : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~1"; break; // RAPIT REMIT
                    case 12 : grd_source = "sp_txn_benef_accounts~" + this.beneficiary_id  + "~1"; break; // CREDIT TO SWIFT
                }
            }

            this.grid = null;
            this.grid = new grid_viewer();
            this.grid.sql_source     = grd_source;
            this.grid.div_container  = grd_container;
            this.grid.datarow_column = 0;
            this.grid.page_size      = 10;
            this.grid.open_grid_view();

        }
        catch(error)
        {
            alert("ERROR LOADING GRID");
        }
    }
    
    
    this.get_record_from_remittance_table = function()
    {
        var ds = RemTranCommons.get_remittance_record(this.remittance_txn_id).value;
        
        if (ds != null)
        {
            if (ds.Tables[0].Rows.length > 0)
            {
                var com_method = new common_method();                
                
                this.set_transaction_values("TRANXDATE"            ,com_method.to_mmddyyyy_date(ds.Tables[0].Rows[0].TranxDate));
                this.set_transaction_values("SEQ_ID"               ,com_method.to_set_null(ds.Tables[0].Rows[0].SEQ_ID));
                this.set_transaction_values("PRINTED"              ,com_method.to_set_null(ds.Tables[0].Rows[0].printed));
                this.set_transaction_values("REMITMETHOD"          ,com_method.to_set_null(ds.Tables[0].Rows[0].RemitMethod));
                this.set_transaction_values("REMITSTATIONID"       ,com_method.to_set_null(ds.Tables[0].Rows[0].RemitStationID));
                this.set_transaction_values("REMITTINGBRANCH"      ,com_method.to_set_null(ds.Tables[0].Rows[0].RemittingBranch));
                this.set_transaction_values("REFERENCENO"          ,com_method.to_set_null(ds.Tables[0].Rows[0].ReferenceNo));
                this.set_transaction_values("EXTLREFERENCENO"      ,com_method.to_set_null(ds.Tables[0].Rows[0].ExtlReferenceNo));
                this.set_transaction_values("TRANSTATUS"           ,com_method.to_set_null(ds.Tables[0].Rows[0].Transtatus));                
                this.set_transaction_values("PRODUCTCHARGESID"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ProductChargesID));
                this.set_transaction_values("REMITTERCIFID"        ,com_method.to_set_null(ds.Tables[0].Rows[0].RemitterCIFID));
                this.set_transaction_values("BENEFICIARYCIFID"     ,com_method.to_set_null(ds.Tables[0].Rows[0].BeneficiaryCIFID));
                this.set_transaction_values("ACCOUNTSEQID"         ,com_method.to_set_null(ds.Tables[0].Rows[0].AccountSEQID));
                this.set_transaction_values("REMITTINGCURRENCY"    ,com_method.to_set_null(ds.Tables[0].Rows[0].RemittingCurrency));
                this.set_transaction_values("LOCALCURRENCY"        ,com_method.to_set_null(ds.Tables[0].Rows[0].LocalCurrency));                
                this.set_transaction_values("GROSSREMAMOUNT"       ,com_method.to_set_null(ds.Tables[0].Rows[0].GrossRemAmount));
                this.set_transaction_values("NETREMAMOUNT"         ,com_method.to_set_null(ds.Tables[0].Rows[0].NetRemAmount));
                this.set_transaction_values("NETTOBENECURRENCY"    ,com_method.to_set_null(ds.Tables[0].Rows[0].NettoBeneCurrency));
                this.set_transaction_values("NETTOBENEAMOUNT"      ,com_method.to_set_null(ds.Tables[0].Rows[0].NettoBeneAmount));
                this.set_transaction_values("TOTALLOCALCHARGE"     ,com_method.to_set_null(ds.Tables[0].Rows[0].TotalLocalCharge));
                this.set_transaction_values("TRANSACTIONSOURCE"    ,com_method.to_set_null(ds.Tables[0].Rows[0].TransactionSource));
                this.set_transaction_values("RECEIPTMODE"          ,com_method.to_set_null(ds.Tables[0].Rows[0].ReceiptMode));
                this.set_transaction_values("TRANSACTIONPURPOSE"   ,com_method.to_set_null(ds.Tables[0].Rows[0].TransactionPurpose));
                this.set_transaction_values("OTHERPURPOSE"         ,com_method.to_set_null(ds.Tables[0].Rows[0].OtherPurpose));
                this.set_transaction_values("SPECIALMESSAGE"       ,com_method.to_set_null(ds.Tables[0].Rows[0].SpecialMessage));                
                this.set_transaction_values("FOREXTARGETTOSOURCE"  ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToSource));
                this.set_transaction_values("FOREXTARGETTOACCOUNT" ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToAccount));
                this.set_transaction_values("FOREXTARGETTOUSD"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToUSD));
                this.set_transaction_values("FOREXTARGETTOPHP"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToPHP));
                this.set_transaction_values("FOREXTARGETTOPEUR"    ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToEUR));                
                this.set_transaction_values("FOREXSOURCETOTARGET"  ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexSourceToTarget));
                this.set_transaction_values("FOREXSOURCETOUSD"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexSourceToUSD));
                this.set_transaction_values("FOREXSOURCETOPHP"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexSourceToPHP));                
                this.set_transaction_values("SOURCEOFFUNDS"        ,com_method.to_set_null(ds.Tables[0].Rows[0].SourceofFunds));
                this.set_transaction_values("CHARGELOCALLY"        ,com_method.to_set_null(ds.Tables[0].Rows[0].ChargeLocally));
                
                this.account_seq_id     = com_method.to_set_null(ds.Tables[0].Rows[0].AccountSEQID);
                this.source_of_fund     = com_method.to_set_null(ds.Tables[0].Rows[0].SourceofFunds);
                this.transaction_status = com_method.to_set_null(ds.Tables[0].Rows[0].Transtatus);
                this.transaction_source = com_method.to_set_null(ds.Tables[0].Rows[0].TransactionSource);
                this.purpose            = com_method.to_set_null(ds.Tables[0].Rows[0].TransactionPurpose);
                this.other_purpose_desc = com_method.to_set_null(ds.Tables[0].Rows[0].OtherPurpose);
                this.special_msg_desc   = com_method.to_set_null(ds.Tables[0].Rows[0].SpecialMessage);
                this.account_currency   = com_method.to_set_null(ds.Tables[0].Rows[0].NettoBeneCurrency);
            }
        }
    }
    
    this.get_save_class_object = function()
    {
        return this.txn_save;
    }
    
    this.load_content_to_cls_saving = function()
    {
        this.set_transaction_values("REMITMETHOD", "4");
        this.set_transaction_values("RECEIPTMODE", "1");
        this.set_transaction_values("REMITTINGBRANCH",   this.remitting_branch_code);
        this.set_transaction_values("REMITTINGCURRENCY", this.remitting_currency);
        this.set_transaction_values("ACCOUNTSEQID",      this.account_seq_id);
        this.set_transaction_values("PRODUCTCHARGESID",  this.product_charge_id);
        this.set_transaction_values("BENEFICIARYCIFID",  this.beneficiary_id);
        this.set_transaction_values("REMITTERCIFID",     this.remitter_id);
        this.set_transaction_values("TRANSTATUS",        this.transaction_status);
        this.set_transaction_values("NETTOBENECURRENCY", this.account_currency);
        
    }
    
    this.set_transaction_values = function(column_name, column_value)
    {
        this.txn_save = new rem_saving_trans();
        this.txn_save.pull_column_from_session();

        switch(column_name.toUpperCase())
        {
            case "SEQ_ID"               : this.txn_save.SEQ_ID               = column_value; break;
            case "PRINTED"              : this.txn_save.printed              = column_value; break;
            case "REMITMETHOD"          : this.txn_save.RemitMethod          = column_value; break;
            case "REMITSTATIONID"       : this.txn_save.RemitStationID       = column_value; break;
            case "REMITTINGBRANCH"      : this.txn_save.RemittingBranch      = column_value; break;
            case "REFERENCENO"          : this.txn_save.ReferenceNo          = column_value; break;
            case "EXTLREFERENCENO"      : this.txn_save.ExtlReferenceNo      = column_value; break;
            case "TRANSTATUS"           : this.txn_save.Transtatus           = column_value; break;
            case "TRANXDATE"            : this.txn_save.TranxDate            = column_value; break;
            case "PRODUCTCHARGESID"     : this.txn_save.ProductChargesID     = column_value; break;
            case "REMITTERCIFID"        : this.txn_save.RemitterCIFID        = column_value; break;
            case "BENEFICIARYCIFID"     : this.txn_save.BeneficiaryCIFID     = column_value; break;
            case "ACCOUNTSEQID"         : this.txn_save.AccountSEQID         = column_value; break;
            case "REMITTINGCURRENCY"    : this.txn_save.RemittingCurrency    = column_value; break;
            case "LOCALCURRENCY"        : this.txn_save.LocalCurrency        = column_value; break;            
            case "GROSSREMAMOUNT"       : this.txn_save.GrossRemAmount       = column_value; break;
            case "NETREMAMOUNT"         : this.txn_save.NetRemAmount         = column_value; break;
            case "NETTOBENECURRENCY"    : this.txn_save.NettoBeneCurrency    = column_value; break;
            case "NETTOBENEAMOUNT"      : this.txn_save.NettoBeneAmount      = column_value; break;
            case "TOTALLOCALCHARGE"     : this.txn_save.TotalLocalCharge     = column_value; break;            
            case "RECEIPTMODE"          : this.txn_save.ReceiptMode          = column_value; break;
            case "OTHERPURPOSE"         : this.txn_save.OtherPurpose         = column_value; break;
            case "SPECIALMESSAGE"       : this.txn_save.SpecialMessage       = column_value; break;
            case "FOREXTARGETTOACCOUNT" : this.txn_save.ForexTargetToAccount = column_value; break;
            case "FOREXTARGETTOSOURCE"  : this.txn_save.ForexTargetToSource  = column_value; break;
            case "FOREXTARGETTOUSD"     : this.txn_save.ForexTargetToUSD     = column_value; break;
            case "FOREXTARGETTOPHP"     : this.txn_save.ForexTargetToPHP     = column_value; break;
            case "FOREXTARGETTOEUR"     : this.txn_save.ForexTargetToEUR     = column_value; break;            
            case "FOREXSOURCETOTARGET"  : this.txn_save.ForexSourceToTarget  = column_value; break;
            case "FOREXSOURCETOUSD"     : this.txn_save.ForexSourceToUSD     = column_value; break;
            case "FOREXSOURCETOPHP"     : this.txn_save.ForexSourceToPHP     = column_value; break;
            case "CHARGELOCALLY"        : this.txn_save.ChargeLocally        = column_value; break;
            case "TRANSACTIONPURPOSE"   : this.txn_save.TransactionPurpose   = column_value; break;
            case "TRANSACTIONSOURCE"    : this.txn_save.TransactionSource    = column_value; break;
            case "SOURCEOFFUNDS"        : this.txn_save.SourceofFunds        = column_value; break;
        }
         
        this.txn_save.push_column_to_session();
        this.txn_save = null;
    }
    
    this.cancel_transaction = function()
    {
        this.go_back_to_search();
    }
    
    this.go_back_to_search = function()
    {
        window.location = "../Remittance/RemTranSearch.aspx";
    }

    this.do_recompute_previous_transaction = function(transaction_sequence_id)
    {
        RemTranCommons.get_charge_previous_transaction_result(transaction_sequence_id, this.do_recompute_previous_transaction_callback);
    }
    
    this.do_recompute_previous_transaction_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            icls.calculator_breakdown = result.value; eval("javascript:push_back_calculator_result(icls)");
        }
    }

    this.do_compute_overrided_data = function()
    {
        if (this.account_currency == null)
        {
            this.account_currency = 1;            
        }

        if (this.product_charge_id == null || this.product_charge_id == '')
        {
            alert("PRODUCT & IMPLEMENTATION NOT FOUND."); return "";
        }
        else
        {
            RemTranCommons.get_override_computed_result(this.remitting_amount, this.do_compute_overrided_data_callback);
        }        
    }
    
    this.do_compute_overrided_data_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            var iamt =  RemTranCommons.get_calculator_computed_values().value;            
            
            if (iamt != null)
            {
                if (iamt != "")
                {
                    var i_array = iamt.split("~");
                    
                    if (i_array != null && i_array.length > 0)
                    {
                        icls.set_transaction_values("CHARGELOCALLY",this.charge_locally);
                    
                        icls.set_transaction_values("LOCALCURRENCY",        i_array[0]);
                        icls.set_transaction_values("GROSSREMAMOUNT",       i_array[1]);
                        icls.set_transaction_values("NETREMAMOUNT",         i_array[2]);
                        icls.set_transaction_values("NETTOBENEAMOUNT",      i_array[3]);
                        icls.set_transaction_values("TOTALLOCALCHARGE",     i_array[4]);
                        icls.set_transaction_values("FOREXTARGETTOACCOUNT", i_array[5]);
                        icls.set_transaction_values("FOREXTARGETTOSOURCE",  i_array[6]);
                        icls.set_transaction_values("FOREXTARGETTOUSD",     i_array[7]);
                        icls.set_transaction_values("FOREXTARGETTOPHP",     i_array[8]);
                        icls.set_transaction_values("FOREXTARGETTOEUR",     i_array[9]);
                        icls.set_transaction_values("FOREXSOURCETOTARGET",  i_array[10]);
                        icls.set_transaction_values("FOREXSOURCETOUSD",     i_array[11]);
                        icls.set_transaction_values("FOREXSOURCETOPHP",     i_array[12]);
                    }
                }
            }            
            
            icls.calculator_breakdown = result.value;
            
            eval("javascript:push_back_calculator_result(icls)");        
        }
    }
    

    this.do_compute_charges = function()
    {   
        if (this.account_currency == null)
        {
            this.account_currency = 1;            
        }

        if (this.product_charge_id == null || this.product_charge_id == '')
        {
            alert("PRODUCT & IMPLEMENTATION NOT FOUND."); return "";
        }
        else
        {
            RemTranCommons.get_charge_computed_result(this.product_charge_id, this.account_currency, this.charge_locally, this.remitting_amount, this.do_compute_charges_callback)
        }
    }

    this.do_compute_charges_callback = function(result)
    {
        if (result != null && result.value != null)
        {       
            icls.calculator_breakdown = result.value;
            icls.get_calculator_computed_values();
            eval("javascript:push_back_calculator_result(icls)");
        }
    }

    this.get_calculator_computed_values = function()
    {
        RemTranCommons.get_calculator_computed_values(this.get_calculator_computed_values_callback)
    }
    
    this.get_calculator_computed_values_callback = function(result)
    {
        if (result != null || result.value != null || result.value != "undefined")
        {
            var iamt =  result.value;
            
            if (iamt != null)
            {
                if (iamt != "")
                {
                    var i_array = iamt.split("~");
                    
                    if (i_array != null && i_array.length > 0)
                    {
                        icls.set_transaction_values("CHARGELOCALLY",this.charge_locally);
                    
                        icls.set_transaction_values("LOCALCURRENCY",        i_array[0]);
                        icls.set_transaction_values("GROSSREMAMOUNT",       i_array[1]);
                        icls.set_transaction_values("NETREMAMOUNT",         i_array[2]);
                        icls.set_transaction_values("NETTOBENEAMOUNT",      i_array[3]);
                        icls.set_transaction_values("TOTALLOCALCHARGE",     i_array[4]);
                        icls.set_transaction_values("FOREXTARGETTOACCOUNT", i_array[5]);
                        icls.set_transaction_values("FOREXTARGETTOSOURCE",  i_array[6]);
                        icls.set_transaction_values("FOREXTARGETTOUSD",     i_array[7]);
                        icls.set_transaction_values("FOREXTARGETTOPHP",     i_array[8]);
                        icls.set_transaction_values("FOREXTARGETTOEUR",     i_array[9]);
                        icls.set_transaction_values("FOREXSOURCETOTARGET",  i_array[10]);
                        icls.set_transaction_values("FOREXSOURCETOUSD",     i_array[11]);
                        icls.set_transaction_values("FOREXSOURCETOPHP",     i_array[12]);
                    }
                }
            }
        }
    }

    this.get_sidebar_lookup_description = function(key_group, key_valcode)
    {
        if (key_valcode == null || key_group == null)
        {
            return "";
        }
    
        RemTranCommons.get_tbl_lookup_description(key_group, key_valcode, this.get_sidebar_lookup_description_callback);
    }
    
    this.get_sidebar_lookup_description_callback = function(result)
    {
        if (result != null || result.value != null || result.value != "undefined")
        {
            icls.lookup_table_description[icls.lookup_table_description.length] = result.value;
        }

        eval("javascript:push_back_sidebar_lookup_description(icls);");        
    }
    
    this.process_grid_viewer = function(parameter)
    {
        if (this.grid != null)
        {
            this.grid.process_ajax(parameter);
        }
        else
        {
            alert("SESSION EXPIRED. RETRY OPERATION TO CONTINUE WORKING.");
        }
    }
    
    this.get_remittance_lookup_data_for_grid_viewer = function(field_group, grd_container)
    {
        this.grd_search_keyword = field_group;

        var grd_source = "sp_remteller_lookup_desc~" + field_group;

        try
        {
            this.grid = null;
            this.grid = new grid_viewer();
            this.grid.sql_source     = grd_source;
            this.grid.div_container  = grd_container;
            this.grid.datarow_column = 0;
            this.grid.page_size      = 10;
            this.grid.open_grid_view();
        }
        catch(error)
        {
            alert("ERROR LOADING GRID");
        }
    }
    
}

/* TEMPORARY JS LOC FOR OVERRRIDE */

function override_calculator()
{
    var icls = this;

    this.currency_rate_source_to_target                = 0;
    this.currency_rate_source_to_target_curr_code_desc = '';    
    this.currency_rate_target_to_source                = 0;
    this.currency_rate_target_to_source_curr_code_desc = '';
    
    this.currency_rate_target_to_usd                   = 0;    
    this.currency_rate_target_to_usd_curr_code_desc    = '';
    this.currency_rate_target_to_php                   = 0;
    this.currency_rate_target_to_php_curr_code_desc    = '';
    this.currency_rate_target_to_eur                   = 0;
    this.currency_rate_target_to_eur_curr_code_desc    = '';
    
    this.currency_rate_source_to_usd                   = 0;
    this.currency_rate_source_to_usd_code_desc         = '';
    this.currency_rate_source_to_php                   = 0;
    this.currency_rate_source_to_php_code_desc         = '';

    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;

    this.login_result_flag = null;
    this.login_result_msgs = null;

    this.irs_calculator = null;
    this.array_override_data = null;    
    this.irs_calculator_session_name = null;
    
    this.array_override_joined = null;
    
    this.open_override_form = function(irs_calculator_session_name)
    {
        this.irs_calculator = RemTranCommons.get_calculator_for_override_values(irs_calculator_session_name).value;
        this.irs_calculator_session_name = irs_calculator_session_name;

        if (this.irs_calculator != null)
        {
            this.open_modal_window();
            this.construct_override_window();
        }
    }
    
    this.construct_override_window = function()
    {
        var irs_calculator_data = this.irs_calculator;

        if (irs_calculator_data != null)
        {
            for(var i = 0; i < irs_calculator_data.Tables[0].Rows.length; i++)
            {
                this.currency_rate_source_to_target                = irs_calculator_data.Tables[0].Rows[i].SourceToTarget;
                this.currency_rate_target_to_source                = irs_calculator_data.Tables[0].Rows[i].TargetToSource;
                this.currency_rate_source_to_target_curr_code_desc = irs_calculator_data.Tables[0].Rows[i].SourceCurrShort;
                this.currency_rate_target_to_source_curr_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;

                switch(irs_calculator_data.Tables[0].Rows[i].currency_factor_code.toLowerCase())
                {
                    case "currency_rate_target_to_usd" :

                        this.currency_rate_target_to_usd           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_target_to_usd_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;
                        break;

                    case "currency_rate_target_to_php" :

                        this.currency_rate_target_to_php           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_target_to_php_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;
                        break;

                    case "currency_rate_target_to_eur" :

                        this.currency_rate_target_to_eur           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_target_to_eur_code_desc = irs_calculator_data.Tables[0].Rows[i].TargetCurrShort;
                        break;

                    case "currency_rate_source_to_usd" :

                        this.currency_rate_source_to_usd           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_source_to_eur_code_desc = irs_calculator_data.Tables[0].Rows[i].SourceCurrShort;
                        break;
                    
                    case "currency_rate_source_to_php" :
                    
                        this.currency_rate_source_to_php           = irs_calculator_data.Tables[0].Rows[i].currency_factor_value;
                        this.currency_rate_source_to_php_code_desc = irs_calculator_data.Tables[0].Rows[i].SourceCurrShort;
                        break;
                }
            }
        }
        
        var html_string = "";
        
        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_usd_code_desc + " TO USD </br> (TARGET TO USD)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_php_code_desc + " TO PHP </br> (TARGET TO PHP)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_eur_code_desc + " TO EUR </br> (TARGET TO EUR)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_source_curr_code_desc + " TO " + this.currency_rate_source_to_target_curr_code_desc + "</br>(TARGET TO SOURCE)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_source_to_target_curr_code_desc + " TO " + this.currency_rate_target_to_source_curr_code_desc + "</br>(SOURCE TO TARGET)</td>";
        html_string += "    </tr>";
        html_string += "    <tr class = 'cell_desc' align = 'center'>";
        html_string += "        <td>" + this.currency_rate_target_to_usd.toFixed(9)    + "</td>";
        html_string += "        <td>" + this.currency_rate_target_to_php.toFixed(9)    + "</td>";
        html_string += "        <td>" + this.currency_rate_target_to_eur.toFixed(9)    + "</td>";
        html_string += "        <td>" + this.currency_rate_target_to_source.toFixed(9) + "</td>";
        html_string += "        <td>" + this.currency_rate_source_to_target.toFixed(9) + "</td>";
        html_string += "    </tr>";
        html_string += "</table></br>";

        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>USD TO " + this.currency_rate_target_to_usd_code_desc + "</br>(USD TO TARGET)</td>";
        html_string += "        <td align = 'center'>PHP TO " + this.currency_rate_target_to_php_code_desc + "</br>(PHP TO TARGET)</td>";
        html_string += "        <td align = 'center'>EUR TO " + this.currency_rate_target_to_eur_code_desc + "</br>(EUR TO TARGET)</td>";
        html_string += "    </tr>";
        html_string += "    <tr class = 'cell_desc' align = 'center'>";
        html_string += "        <td>" + (1 / this.currency_rate_target_to_usd).toFixed(9) + "</td>";
        html_string += "        <td>" + (1 / this.currency_rate_target_to_php).toFixed(9) + "</td>";
        html_string += "        <td>" + (1 / this.currency_rate_target_to_eur).toFixed(9) + "</td>";
        html_string += "    </tr>";
        html_string += "</table></br>";
        
        html_string += "<table id = 'tbl_override_forex_rates' style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_usd_code_desc + " TO USD </br> (TARGET TO USD)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_php_code_desc + " TO PHP </br> (TARGET TO PHP)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_eur_code_desc + " TO EUR </br> (TARGET TO EUR)</td>";
        html_string += "        <td align = 'center'>" + this.currency_rate_target_to_source_curr_code_desc + " TO " + this.currency_rate_source_to_target_curr_code_desc + "</br>(TARGET TO SOURCE)</td>";
        html_string += "    </tr>";
        html_string += "    <tr class = 'cell_desc' align = 'center'>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_usd'    onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_usd.toFixed(9)    + "'/></td>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_php'    onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_php.toFixed(9)    + "'/></td>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_eur'    onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_eur.toFixed(9)    + "'/></td>";
        html_string += "        <td><input id= 'txt_currency_rate_target_to_source' onkeyup = 'javascript: this.orig_value = this.value' onblur = 'javascript: eval(this.value = (1 * this.orig_value).toFixed(9))' onfocus = 'javascript: this.orig_value = this.value; eval(this.value = (1 * this.value).toFixed(2))' type='text' style = 'text-align:center; width:auto' value = '" + this.currency_rate_target_to_source.toFixed(9) + "'/></td>";
        html_string += "    </tr>";
        html_string += "</table></br>";
        
        html_string += "<table id = 'tbl_override_charges_setup' style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr  class = 'cell_caption'>";
        html_string += "        <td align = 'center'>CHARGE NAME</td>";
        html_string += "        <td align = 'center'>CURRENCY</td>";
        html_string += "        <td align = 'center'>AMOUNT</td>";
        html_string += "    </tr>"
        
        if (irs_calculator_data != null)
        {        
            for(var i = 0; i < irs_calculator_data.Tables[1].Rows.length; i++)
            {
                var txt_charge_name_hidden = "<input type = 'hidden' style = 'text-align:right; width:auto' id = 'hidden_charges_id_" + (i + 1) + "' value = '" + irs_calculator_data.Tables[1].Rows[i].ChargeName + "'/>";
                var txt_charge_name_txtbox = "<input type = 'text'   style = 'text-align:right; width:auto' id = 'txt_charges_value_" + (i + 1) + "' value = '" + ( 1 * irs_calculator_data.Tables[1].Rows[i].ChargeValue).toFixed(2) + "'/>";
                
                html_string += "    <tr  class = 'cell_desc'>";
                html_string += "        <td align = 'left'>"   + irs_calculator_data.Tables[1].Rows[i].ChargeNameDesc.toUpperCase()    + "</td>";
                html_string += "        <td align = 'center'>" + irs_calculator_data.Tables[1].Rows[i].CurrencyShortCode + "</td>";
                html_string += "        <td align = 'right'>"  + txt_charge_name_hidden + txt_charge_name_txtbox + "</td>";
                html_string += "    </tr>"
            }
        }
        
        html_string += "</table></br>";
        
        html_string += "<table style='border: solid 1px #336699;' cellspacing='1' cellpadding='0' width='100%'>";
        html_string += "    <tr>";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%'>LOGIN NAME:</td>";
        html_string += "        <td class = 'cell_desc'><input type='text' id = 'txt_username' style = 'width: auto'/></td>";
        html_string += "    </tr>";
        html_string += "    <tr>";
        html_string += "        <td class = 'cell_caption' style = 'width: 30%'>PASSWORD:</td>";
        html_string += "        <td class = 'cell_desc'><input type='password' id = 'txt_password' style = 'width: auto'/></td>";
        html_string += "    </tr>";
        html_string += "</table></br>";
        
        html_string += "</br><center><input id='btn_override' style='width: 15%; height:15%' type='submit' value='APPLY CHANGES' />&nbsp<input id='btn_override_cancel' style='width: 15%; height:15%' type='submit' value='CANCEL' /></center></br>";

        document.getElementById("content_mod_div").innerHTML = html_string;
        
        document.getElementById("content_mod_div").document.getElementById("btn_override").onclick = function()
        {
            RemTranCommons.user_can_override_rates(document.getElementById("content_mod_div").document.getElementById("txt_username").value, document.getElementById("content_mod_div").document.getElementById("txt_password").value, icls.override_callback);
        }
        
        document.getElementById("content_mod_div").document.getElementById("btn_override_cancel").onclick = function()
        {
            icls.close_modal_window();
        }
        
        irs_calculator_data = null;
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
        RemTranCommons.set_calculator_override_values(icls.array_override_joined, icls.irs_calculator_session_name, this.send_parsed_data_callback);
    }
    
    this.send_parsed_data_callback = function(result)
    {
        if (result != null)
        {
            eval("javascript:trigger_override_result(icls);");
        }
    }
    
    this.parse_data_changes = function()
    {
        if (this.irs_calculator != null)
        {
            this.array_override_data = new Array();

            if (this.irs_calculator.Tables[0].Rows.length > 0)
            {
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_usd"    + "~" + document.getElementById("txt_currency_rate_target_to_usd").value;
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_php"    + "~" + document.getElementById("txt_currency_rate_target_to_php").value;
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_eur"    + "~" + document.getElementById("txt_currency_rate_target_to_eur").value;
                this.array_override_data[this.array_override_data.length] = "FOREX_RATES" + "~" + "currency_rate_target_to_source" + "~" + document.getElementById("txt_currency_rate_target_to_source").value;                
            }
        
            if (this.irs_calculator.Tables[1].Rows.length > 0)
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
            
            this.array_override_joined =  this.array_override_data.join("|");
        }
    }

    this.open_modal_window = function()
	{
        this.load_modal_css();
		this.outer_modal_div           = document.createElement("div");
		this.outer_modal_div.id        = "outer_mod_div";
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

		if(this.outer_modal_div != null){document.body.removeChild(this.outer_modal_div);this.outer_modal_div  = null;}
		if(this.inner_modal_div != null){document.body.removeChild(this.inner_modal_div);this.inner_modal_div  = null;}
		if(this.content_iframe  != null){this.content_iframe = null;}

		document.body.style.margin = "";		
	}

    this.construct_div_content = function()
	{
	    var content_div = null;

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = parseInt(parent.screen.availWidth)  - 300;
		this.inner_modal_div.style.top    = 10;
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10;
		this.inner_modal_div.style.border = "medium solid white";

		content_div                  = document.createElement("div");		
		content_div.id               = "content_mod_div";
		content_div.align            = "left";
		content_div.style.background = "transparent";
		content_div.style.width      = "95%";
		content_div.innerHTML        = "<center><img src = '../images/ajaxloader.gif'></img></center>";

        this.inner_modal_div.appendChild(document.createElement("br"));
		this.inner_modal_div.appendChild(content_div);
		
		document.body.appendChild(this.inner_modal_div);
	}
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet")
	  	file_reference.setAttribute("type", "text/css")
	  	file_reference.setAttribute("href", "../../css/cssmodals.css")

		if (typeof file_reference !="undefined")
		{		    
	  		document.getElementsByTagName("head")[0].appendChild(file_reference)
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
	            divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break;
	        }	        
	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        {
	            divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break;
	        }	        
	    }	    
	}        
}