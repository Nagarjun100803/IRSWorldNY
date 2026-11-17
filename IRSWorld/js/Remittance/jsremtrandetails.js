
function rem_trandetails()
{
    this.txn_save = null;
    
    this.load_transaction_details = function()
    {        
        this.clear_screen();
        this.txn_save = new rem_saving_trans();

        var ds = this.txn_save.get_column_description().value;
        
        if (ds != null)
        {        
            document.getElementById("lblTVTranSource").innerText     = ds.Tables[0].Rows[0].TRANSACTION_SOURCE;
            document.getElementById("lblTVSourceFund").innerText     = ds.Tables[0].Rows[0].SOURCE_OF_FUND;
            document.getElementById("lblTVTranStatus").innerText     = ds.Tables[0].Rows[0].TRANSACTION_STATUS;
            document.getElementById("lblTVPurpose").innerText        = ds.Tables[0].Rows[0].TRANSACTION_PURPOSE;
            document.getElementById("lblTVReceiptMode").innerText    = ds.Tables[0].Rows[0].RECEIPT_MODE;
            document.getElementById("lblTVRemName").innerText        = ds.Tables[0].Rows[0].REMITTER_NAME;
            document.getElementById("lblTVRemAddress").innerText     = ds.Tables[0].Rows[0].REMITTER_ADDR;
            document.getElementById("lblTVBeneName").innerText       = ds.Tables[0].Rows[0].BENEFICIARY_NAME;
            document.getElementById("lblTVBeneAddress").innerText    = ds.Tables[0].Rows[0].BENEFICIARY_ADDR;
            document.getElementById("lblTVChargeType").innerText     = ds.Tables[0].Rows[0].CHARGE_TYPE;
            document.getElementById("lblTVProductType").innerText    = ds.Tables[0].Rows[0].PRODUCT_TYPE;
            document.getElementById("lblTVPayMode").innerText        = ds.Tables[0].Rows[0].PAYMODE_TYPE;
            document.getElementById("lblTVImplementation").innerText = ds.Tables[0].Rows[0].IMPLEMENTATION_TYPE;
            document.getElementById("lblTVRemitCurrency").innerText  = ds.Tables[0].Rows[0].REMITTING_CURRENCY;
            document.getElementById("lblTVTranDate").innerText       = ds.Tables[0].Rows[0].TRANXDATE;
            document.getElementById("lblTVDateToday").innerText      = ds.Tables[0].Rows[0].DATETODAY;

            /*
            document.getElementById("lblTVRefNo").innerText          = ds.Tables[0].Rows[0];
            document.getElementById("lblTVCoverBank").innerText      = ds.Tables[0].Rows[0];
            document.getElementById("lblTVLocalCurrency").innerText  = ds.Tables[0].Rows[0];
            document.getElementById("lblTVEuroToUSD").innerText      = ds.Tables[0].Rows[0];
            document.getElementById("lblTVUSDToEuro").innerText      = ds.Tables[0].Rows[0];
            document.getElementById("lblTVTARC").innerText           = ds.Tables[0].Rows[0];
            document.getElementById("lblNRAPE").innerText            = ds.Tables[0].Rows[0];
            document.getElementById("lblTVNRARC").innerText          = ds.Tables[0].Rows[0];
            document.getElementById("lblNTBACC").innerText           = ds.Tables[0].Rows[0];
            document.getElementById("lblTVCommStatus").innerText = ds.Tables[0].Rows[0];
            
            */
        }
    }
    
    this.post_transaction = function()
    {
        var i_save = confirm('DO YOU WANT TO SAVE THIS TRANSACTION ? \n\n CLICK CANCEL TO ABORT SAVING AND RETURN TO TRANSACTION MODULE. ');

        if (this.txn_save != null)
        {
            if (i_save == true)
            {
                this.txn_save.commit_record();
            }
        }
    }
    
    this.clear_screen = function()
    {
        document.getElementById("lblTVTranSource").innerText     = '';
        document.getElementById("lblTVSourceFund").innerText     = '';
        document.getElementById("lblTVTranStatus").innerText     = '';
        document.getElementById("lblTVPurpose").innerText        = '';
        document.getElementById("lblTVReceiptMode").innerText    = '';
        document.getElementById("lblTVRemName").innerText        = '';
        document.getElementById("lblTVRemAddress").innerText     = '';
        document.getElementById("lblTVBeneName").innerText       = '';
        document.getElementById("lblTVBeneAddress").innerText    = '';        
        document.getElementById("lblTVRefNo").innerText          = '';
        document.getElementById("lblTVTranDate").innerText       = '';
        document.getElementById("lblTVChargeType").innerText     = '';
        document.getElementById("lblTVProductType").innerText    = '';
        document.getElementById("lblTVCoverBank").innerText      = '';
        document.getElementById("lblTVImplementation").innerText = '';
        document.getElementById("lblTVPayMode").innerText        = '';
        document.getElementById("lblTVRemitCurrency").innerText  = '';
        document.getElementById("lblTVLocalCurrency").innerText  = '';
        document.getElementById("lblTVEuroToUSD").innerText      = '';
        document.getElementById("lblTVUSDToEuro").innerText      = '';
        document.getElementById("lblTVTARC").innerText           = '';
        document.getElementById("lblNRAPE").innerText            = '';
        document.getElementById("lblTVNRARC").innerText          = '';
        document.getElementById("lblNTBACC").innerText           = '';
        document.getElementById("lblTVCommStatus").innerText     = '';
        document.getElementById("lblTVDateToday").innerText      = '';

    }
    
    
}


