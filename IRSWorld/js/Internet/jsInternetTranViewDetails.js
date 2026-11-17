
function InternetTran_Details()
{
    this.txn_save = null;
    
    this.load_transaction_details = function()
    {        
        this.clear_screen();
        this.txn_save = new rem_saving_trans();

        var ds = this.txn_save.get_column_description().value;
        
        if (ds != null)
        {        
            document.getElementById("lblInternetTVTranSource").innerHTML     = ds.Tables[0].Rows[0].TRANSACTION_SOURCE;
            document.getElementById("lblInternetTVSourceFund").innerHTML     = ds.Tables[0].Rows[0].SOURCE_OF_FUND;
            document.getElementById("lblInternetTVTranStatus").innerHTML     = ds.Tables[0].Rows[0].TRANSACTION_STATUS;
            document.getElementById("lblInternetTVPurpose").innerHTML        = ds.Tables[0].Rows[0].TRANSACTION_PURPOSE;
            document.getElementById("lblInternetTVReceiptMode").innerHTML    = ds.Tables[0].Rows[0].RECEIPT_MODE;
            document.getElementById("lblInternetTVRemName").innerHTML        = ds.Tables[0].Rows[0].REMITTER_NAME;
            document.getElementById("lblInternetTVRemAddress").innerHTML     = ds.Tables[0].Rows[0].REMITTER_ADDR;
            document.getElementById("lblInternetTVBeneName").innerHTML       = ds.Tables[0].Rows[0].BENEFICIARY_NAME;
            document.getElementById("lblInternetTVBeneAddress").innerHTML    = ds.Tables[0].Rows[0].BENEFICIARY_ADDR;
            document.getElementById("lblInternetTVChargeType").innerHTML     = ds.Tables[0].Rows[0].CHARGE_TYPE;
            document.getElementById("lblInternetTVProductType").innerHTML    = ds.Tables[0].Rows[0].PRODUCT_TYPE;
            document.getElementById("lblInternetTVPayMode").innerHTML        = ds.Tables[0].Rows[0].PAYMODE_TYPE;
            document.getElementById("lblInternetTVImplementation").innerHTML = ds.Tables[0].Rows[0].IMPLEMENTATION_TYPE;
            document.getElementById("lblInternetTVRemitCurrency").innerHTML  = ds.Tables[0].Rows[0].REMITTING_CURRENCY;
            document.getElementById("lblInternetTVTranDate").innerHTML       = ds.Tables[0].Rows[0].TRANXDATE;
            document.getElementById("lblInternetTVDateToday").innerHTML      = ds.Tables[0].Rows[0].DATETODAY;

            /*
            document.getElementById("lblInternetTVRefNo").innerHTML          = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVCoverBank").innerHTML      = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVLocalCurrency").innerHTML  = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVEuroToUSD").innerHTML      = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVUSDToEuro").innerHTML      = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVTARC").innerHTML           = ds.Tables[0].Rows[0];
            document.getElementById("lblNRAPE").innerHTML            = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVNRARC").innerHTML          = ds.Tables[0].Rows[0];
            document.getElementById("lblNTBACC").innerHTML           = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVCommStatus").innerHTML = ds.Tables[0].Rows[0];
            
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
        document.getElementById("lblInternetTVTranSource").innerHTML     = '';
        document.getElementById("lblInternetTVSourceFund").innerHTML     = '';
        document.getElementById("lblInternetTVTranStatus").innerHTML     = '';
        document.getElementById("lblInternetTVPurpose").innerHTML        = '';
        document.getElementById("lblInternetTVReceiptMode").innerHTML    = '';
        document.getElementById("lblInternetTVRemName").innerHTML        = '';
        document.getElementById("lblInternetTVRemAddress").innerHTML     = '';
        document.getElementById("lblInternetTVBeneName").innerHTML       = '';
        document.getElementById("lblInternetTVBeneAddress").innerHTML    = '';        
        document.getElementById("lblInternetTVRefNo").innerHTML          = '';
        document.getElementById("lblInternetTVTranDate").innerHTML       = '';
        document.getElementById("lblInternetTVChargeType").innerHTML     = '';
        document.getElementById("lblInternetTVProductType").innerHTML    = '';
        document.getElementById("lblInternetTVCoverBank").innerHTML      = '';
        document.getElementById("lblInternetTVImplementation").innerHTML = '';
        document.getElementById("lblInternetTVPayMode").innerHTML        = '';
        document.getElementById("lblInternetTVRemitCurrency").innerHTML  = '';
        document.getElementById("lblInternetTVLocalCurrency").innerHTML  = '';
        document.getElementById("lblInternetTVEuroToUSD").innerHTML      = '';
        document.getElementById("lblInternetTVUSDToEuro").innerHTML      = '';
        document.getElementById("lblInternetTVTARC").innerHTML           = '';
        document.getElementById("lblNRAPE").innerHTML            = '';
        document.getElementById("lblInternetTVNRARC").innerHTML          = '';
        document.getElementById("lblNTBACC").innerHTML           = '';
        document.getElementById("lblInternetTVCommStatus").innerHTML     = '';
        document.getElementById("lblInternetTVDateToday").innerHTML      = '';

    }
    
    
}


