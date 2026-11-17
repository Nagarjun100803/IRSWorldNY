function rem_commons()
{
    this.txn_save                     = null;
    this.iframe_modal                 = null;
    this.remitter_fullname            = null;
    this.remitter_address             = null;
    this.beneficiary_fullname         = null;
    this.beneficiary_address          = null;
    this.beneficiary_pnb_branch_name  = null;
    this.beneficiary_domestic_bank    = null;
    this.beneficiary_account_type     = null;
    this.beneficiary_account_currency = null;
    this.beneficiary_account_number   = null;    
    this.remitter_id                  = null;
    this.beneficiary_id               = null;
    this.product_charge_id            = null;
    this.remittance_txn_id            = null;
    this.transaction_status           = null;
    this.account_seq_id               = null;
       
    this.get_record_from_remittance_table = function()
    {
        var ds = RemTranCommons.get_remittance_record(this.remittance_txn_id).value;
        
        if (ds != null)
        {
            if (ds.Tables[0].Rows.length > 0)
            {
                var com_method = new common_method();
                
                this.set_transaction_values("DATESENT"             ,com_method.to_mmddyyyy_date(ds.Tables[0].Rows[0].DateSent));
                this.set_transaction_values("TRANXDATE"            ,com_method.to_mmddyyyy_date(ds.Tables[0].Rows[0].TranxDate));
                this.set_transaction_values("DATEIMPLEMENTED"      ,com_method.to_mmddyyyy_date(ds.Tables[0].Rows[0].DateImplemented));
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
                this.set_transaction_values("FOREXSOURCETOTARGET"  ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexSourceToTarget));
                this.set_transaction_values("FOREXTARGETTOUSD"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToUSD));
                this.set_transaction_values("FOREXTARGETTOPHP"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToPHP));
                this.set_transaction_values("FOREXSOURCETOUSD"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexSourceToUSD));
                this.set_transaction_values("FOREXSOURCETOPHP"     ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexSourceToPHP));
                this.set_transaction_values("FOREXTARGETTOACCOUNT" ,com_method.to_set_null(ds.Tables[0].Rows[0].ForexTargetToAccount));
                this.set_transaction_values("SOURCEOFFUNDS"        ,com_method.to_set_null(ds.Tables[0].Rows[0].SourceofFunds));
            }
        }
    }
    
    this.get_remitter_basic_info = function(customer_id, account_seq_id)
    {
        var ds = RemTranCommons.get_customer_basic_info(customer_id, account_seq_id).value;
        
        if (ds != null)
        {
            if (ds.Tables[0].Rows.length > 0)
            {
                this.remitter_fullname = ds.Tables[0].Rows[0].customer_fullname;
                this.remitter_address  = ds.Tables[0].Rows[0].cif_address;
            }
        }
    }
    
    this.get_beneficiary_basic_info = function(customer_id, account_seq_id)
    {
        this.account_seq_id = account_seq_id;
        
        var ds = RemTranCommons.get_customer_basic_info(customer_id, account_seq_id).value;
        
        if (ds != null)
        {
            if (ds.Tables[0].Rows.length > 0)
            {
                this.beneficiary_pnb_branch_name  = ds.Tables[0].Rows[0].customer_fullname;
                this.beneficiary_domestic_bank    = null;
                this.beneficiary_fullname         = ds.Tables[0].Rows[0].customer_fullname;
                this.beneficiary_address          = ds.Tables[0].Rows[0].cif_address;
                this.beneficiary_account_type     = ds.Tables[0].Rows[0].accounttype_desc;
                this.beneficiary_account_currency = ds.Tables[0].Rows[0].account_curr_desc;
                this.beneficiary_account_number   = ds.Tables[0].Rows[0].account_no_desc;
            }
        }
    }
    this.get_transaction_module_query_string = function()
    {
        var i_qstring = new getQueryStrings(null);        
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
        }
    }
    
    this.get_product_default_settings = function()
    {
        /* FOR NEW TRANSACTION ONLY. GET ALL DEFAULT VALUES. CAN BE MODIFIED BY TELLER */
        
        this.set_transaction_values("REMITTERCIFID",    this.remitter_id );
        this.set_transaction_values("BENEFICIARYCIFID", this.beneficiary_id );
        this.set_transaction_values("PRODUCTCHARGESID", this.product_charge_id );
        this.set_transaction_values("ACCOUNTSEQID",     this.account_seq_id);
        
        var ds = RemTranCommons.get_remittance_product_charges(this.product_charge_id).value;
        
        if (ds != null)
        {
            if (ds.Tables[0].Rows.length > 0)
            {
                var com_method = new common_method(); this.set_transaction_values("TRANSTATUS", com_method.to_set_null(ds.Tables[0].Rows[0].DefaultStatus)); com_method = null;
            }
        }
        
        ds = null;
    }
    this.do_txn_product_main_loading = function()
    {
        this.get_transaction_module_query_string();
        
        if (this.remitter_id != null && this.beneficiary_id != null && this.product_charge_id != null)
        {
            if (this.remittance_txn_id == null || this.remittance_txn_id == '' || this.remittance_txn_id.toUpperCase() == 'NULL')
            {
                this.get_product_default_settings();
            }
            else
            {
                this.get_record_from_remittance_table();
            }
            
            this.get_remitter_basic_info(this.remitter_id, '');
            this.get_beneficiary_basic_info(this.beneficiary_id,'');
        }
    }
    this.load_transaction_view_details = function()
    {
        if (this.iframe_modal == null)
        {
            this.iframe_modal = new modal_window();
        }                
        this.iframe_modal.open_modal_iframe('../../Internet/InternetTranViewDetails.aspx');        
    }

    this.set_transaction_values = function(column_name, column_value)
    {
        this.txn_save = new rem_saving_trans();
        this.txn_save.pull_column_from_session();

        switch(column_name.toUpperCase())
        {
            case "SEQ_ID"               : this.txn_save.SEQ_ID               = column_value; break;
            case "DATEIMPLEMENTED"      : this.txn_save.DateImplemented      = column_value; break;
            case "DATESENT"             : this.txn_save.DateSent             = column_value; break;
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
            case "GROSSREMAMOUNT"       : this.txn_save.GrossRemAmount       = column_value; break;
            case "NETREMAMOUNT"         : this.txn_save.NetRemAmount         = column_value; break;
            case "NETTOBENECURRENCY"    : this.txn_save.NettoBeneCurrency    = column_value; break;
            case "NETTOBENEAMOUNT"      : this.txn_save.NettoBeneAmount      = column_value; break;
            case "TOTALLOCALCHARGE"     : this.txn_save.TotalLocalCharge     = column_value; break;
            case "TRANSACTIONSOURCE"    : this.txn_save.TransactionSource    = column_value; break;
            case "RECEIPTMODE"          : this.txn_save.ReceiptMode          = column_value; break;
            case "TRANSACTIONPURPOSE"   : this.txn_save.TransactionPurpose   = column_value; break;
            case "OTHERPURPOSE"         : this.txn_save.OtherPurpose         = column_value; break;
            case "SPECIALMESSAGE"       : this.txn_save.SpecialMessage       = column_value; break;
            case "FOREXSOURCETOTARGET"  : this.txn_save.ForexSourceToTarget  = column_value; break;
            case "FOREXTARGETTOUSD"     : this.txn_save.ForexTargetToUSD     = column_value; break;
            case "FOREXTARGETTOPHP"     : this.txn_save.ForexTargetToPHP     = column_value; break;
            case "FOREXSOURCETOUSD"     : this.txn_save.ForexSourceToUSD     = column_value; break;
            case "FOREXSOURCETOPHP"     : this.txn_save.ForexSourceToPHP     = column_value; break;
            case "FOREXTARGETTOACCOUNT" : this.txn_save.ForexTargetToAccount = column_value; break;
            case "SOURCEOFFUNDS"        : this.txn_save.SourceofFunds        = column_value; break;
        }
                
        this.txn_save.push_column_to_session();
    }
    
    this.get_save_class_object = function()
    {
        return this.txn_save;
    }
}
