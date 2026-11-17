// **************************************************************************************** //
// *** AJAX
// **************************************************************************************** //
function execute_ajaxgridview_command(param)
{
    if (OTC_page != null && OTC_page != undefined)
    {
        OTC_page.do_ajaxgridview(param);
    }
}

// **************************************************************************************** //
// *** Over The Counter Page
// **************************************************************************************** //
function OTC_page()
{
    // **************************************************************************************** //
    // *** Variables
    // **************************************************************************************** //
    var X = 0;
    var Y = 0;
    var tblChargeType = "";
    var tblComputation = "";
    var tblCurrency = "";
    var tblDefaults = "";
    var tblDefaultCurrency = "";
    var tblRemitter = "";
    var tblRemitterCards = "";
    var tblRemitterAccounts = "";
    var tblBeneficiary = "";
    var tblBeneficiaryAccounts = "";
    var tblCurrency = "";
    var tblPayMode = "";
    var tblExchangeRate = "";
    var _searchType = "";
    var _lookupField = "";
    var _Brow = 0;
    var _RCrow = 0;
    var _RArow = 0;
    var _calc = 0;
    var _arrCalc = 0;
    
    // **************************************************************************************** //
    // *** Asynchorous Callback AJAX
    // **************************************************************************************** //
    this.getLookup = function()
    {
        return OTCTemplate.getLookup(OTC_page._lookupField, this.getLookup_callback).value;
    }
    
    this.getLookup_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            switch (response.Rows[0]["FldGroup"])
            {
                case "ChargeType":
                    OTC_page.tblChargeType = response;
                    break;
                case "Computation":
                    OTC_page.tblComputation = response;
                    OTC_page.fillDDL(response, "ddlComputation", "FldDescription", "FldValue", "lblComputation", "");
                    var select = document.getElementById("ddlComputation");
                    select.selectedIndex = 1;
                    OTC_page.enterValue("1", "lblComputation", false);
                    break;
                case "Currency":
                    OTC_page.tblCurrency = response;
                    break;
                case "PayMode":
                    OTC_page.tblPayMode = response;
                    break;
            }
            OTC_page._lookupField = "";
        }
    }
    
    this.getLocation = function()
    {
        return OTCTemplate.getLocation(this.getLocation_callback).value;
    }
    
    this.getLocation_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.enterValue(response, "lblLocation", false);
        }
    }
    
    this.getExchangeRate = function()
    {
        var select = document.getElementById("ddlSourceCurrency");
        var sourcecurrency = select.options[select.selectedIndex].text;
        OTC_page.getDDL("ddlSourceCurrency", "lblSourceCurrency", true);
        return OTCTemplate.getExchangeRate(OTC_page.findValue(OTC_page.tblCurrency, sourcecurrency, "FldCode", "FldValue"), OTC_page.findValue(OTC_page.tblCurrency, OTC_page.getValue("spnBeneficiaryAccountCurrency", true), "FldCode", "FldValue"), this.getExchangeRate_callback).value;
    }
    
    this.getExchangeRate_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.tblExchangeRate = response;
            OTC_page.enterValue("Today's Exchange Rate: " + response.Rows[0]["Forex2"], "spnExchangeRate", true);
            OTC_page.enterValue(response.Rows[0]["SourceToTarget"], "lblSourceForex", false);
            OTC_page.enterValue(response.Rows[0]["TargetToSource"], "lblTargetForex", false);
        }
        else
        {
            OTC_page.enterValue("Today's Exchange Rate:", "spnExchangeRate", true);
        }
    }
    
    this.getSearch = function()
    {
        return OTCTemplate.getSearch(OTC_page.getValue("lblCIFClass", false), OTC_page.getValue("txtSCIFID", false), OTC_page.getValue("txtSLastName", false), OTC_page.getValue("txtSFirstName", false), OTC_page.getValue("txtSMiddleName", false), this.getSearch_callback).value;
    }
    
    this.getSearch_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.enterValue(response, "lblRemitterID", false);
        }
    }
    
    this.getRemitterAccounts = function()
    {
        return OTCTemplate.getRemitterAccounts(OTC_page.getValue("lblRemitterID"), this.getRemitterAccounts_callback).value;
    }
    
    this.getRemitterAccounts_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.tblRemitterAccounts = response;
            OTC_page.showRemitterAccounts();
        }
    }
    
    this.getRemitterCards = function()
    {
        return OTCTemplate.getRemitterCards(OTC_page.getValue("lblRemitterID"), this.getRemitterCards_callback).value;
    }
    
    this.getRemitterCards_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.tblRemitterCards = response;
            OTC_page.showRemitterCards();
        }
    }
    
    this.getRemitter = function(seqid)
    {
        return OTCTemplate.getRemitter(seqid, this.getRemitter_callback).value;
    }
    
    this.getRemitter_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.tblRemitter = response;
            OTC_page.getBeneficiary(response.Rows[0]["RemitterCIFID"]);
        }
    }
    
    this.getBeneficiary = function(remitterid)
    {
        return OTCTemplate.getBeneficiary(remitterid, this.getBeneficiary_callback).value;
    }
    
    this.getBeneficiary_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.tblBeneficiary = response;
            OTC_page.showRemitterBeneficiary();
        }
        else
        {
            if (confirm("There is no beneficiary maintained for this Remitter. Do you want to continue?"))
            {
                OTC_page.tblBeneficiary = "";
                OTC_page.showRemitterBeneficiary();
            }
        }
    }
    
    this.getBeneficiaryAccounts = function(beneficiaryid, iscif)
    {
        return OTCTemplate.getBeneficiaryAccounts(beneficiaryid, iscif, this.getBeneficiaryAccounts_callback).value;
    }
    
    this.getBeneficiaryAccounts_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.tblBeneficiaryAccounts = response;
            OTC_page.enterValue(OTC_page.getTableValue(response, "AcctSeqID", 0), "lblBeneficiaryAcctSeqID", false);
            (OTC_page.getTableValue(response, "BranchName", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(response, "BranchName", 0), "spnBeneficiaryMaintainingBranch", true) : OTC_page.enterValue("", "spnBeneficiaryMaintainingBranch", true);
            (OTC_page.getTableValue(response, "AccountNo", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(response, "AccountNo", 0), "spnBeneficiaryAccountNo", true) : OTC_page.enterValue("", "spnBeneficiaryAccountNo", true);
            (OTC_page.getTableValue(response, "AccountTypeDesc", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(response, "AccountTypeDesc", 0), "spnBeneficiaryAccountType", true) : OTC_page.enterValue("", "spnBeneficiaryAccountType", true);
            (OTC_page.getTableValue(response, "CurrencyCode", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(response, "CurrencyCode", 0), "spnBeneficiaryAccountCurrency", true) : OTC_page.enterValue("", "spnBeneficiaryAccountCurrency", true);
            (OTC_page.getTableValue(response, "DomBranchName", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(response, "DomBranchName", 0), "spnBeneficiaryOtherBanks", true) : OTC_page.enterValue("", "spnBeneficiaryOtherBanks", true);
            (OTC_page.getTableValue(response, "Currency", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(response, "Currency", 0), "lblTargetCurrency", false) : OTC_page.enterValue("", "lblTargetCurrency", false);
        }
        OTC_page.hidePanel("divSearchGrid");
    }
    
    this.getSearchBy = function()
    {
        OTC_page.clearDDL("ddlSearchBy");
        return OTCTemplate.getSearchBy(this.getSearchBy_callback).value;
    }
    
    this.getSearchBy_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.fillDDL(response, "ddlSearchBy", "FldDescription", "FldValue", "lblCIFClass", "");
        }
    }
    
    this.getLookupDefaults = function(group)
    {
        return OTCTemplate.getLookupDefaults(group, this.getLookupDefaults_callback).value;
    }
    
    this.getLookupDefaults_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            switch (response.Rows[0]["FldGroup"])
            {
                case "Currency":
                    OTC_page.fillDDL(response, "ddlSourceCurrency", "FldCode", "FldValue", "lblSourceCurrency", "");
                    break;
            }
        }
    }
    
    this.getDefaults = function()
    {
        return OTCTemplate.getDefaults(this.getDefaults_callback).value;
    }
    
    this.getDefaults_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            OTC_page.tblDefaults = response;
        }
    }
    
    this.getCharges = function()
    {
        if (OTC_page.getValue("txtAmount", false) != "0.00" && OTC_page.getValue("txtAmount", false) != "" && OTC_page.getValue("txtAmount", false) != undefined)
        {
            if (OTC_page.getValue("lblProductCode", false) != "" && OTC_page.getValue("lblProductCode", false) != undefined)
            {
                if (OTC_page.getValue("lblSourceCurrency", false) != "" && OTC_page.getValue("lblSourceCurrency", false) != undefined)
                {
                    if (OTC_page.getValue("lblTargetCurrency", false) != "" && OTC_page.getValue("lblTargetCurrency", false) != undefined)
                    {
                        if (OTC_page.getValue("lblPaymentMethod", false) != "" && OTC_page.getValue("lblPaymentMethod", false) != undefined)
                        {
                            if (OTC_page.getValue("lblChargeType", false) != "" && OTC_page.getValue("lblChargeType", false) != undefined)
                            {
                                if (OTC_page.getValue("lblComputation", false) != "" && OTC_page.getValue("lblComputation", false) != undefined)
                                {
                                    return OTCTemplate.getCharges(OTC_page.getValue("txtAmount", false), OTC_page.getValue("lblProductCode", false), OTC_page.getValue("lblImplementationCode", false), OTC_page.getValue("lblSourceCurrency", false), OTC_page.getValue("lblTargetCurrency", false), OTC_page.getValue("lblChargeType", false), OTC_page.getValue("lblPaymentMethod", false), OTC_page.getValue("lblComputation", false), this.getCharges_callback).value;
                                }
                                else
                                {
                                    OTC_page.showAlert("Please select a computation.");
                                }
                            }
                            else
                            {
                                OTC_page.showAlert("Please select a charge type.");
                            }
                        }
                        else
                        {
                            OTC_page.showAlert("Please select your payment method.");
                        }
                    }
                    else
                    {
                        OTC_page.showAlert("Please select beneficiary account.");
                    }
                }
                else
                {
                    OTC_page.showAlert("Please select your remitting currency.");
                }
            }
            else
            {
                OTC_page.showAlert("Please select a beneficiary.");
            }
        }
        else
        {
            OTC_page.showAlert("Please enter an amount.");
        }
    }
    
    this.getCharges_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response != "NOCHARGE")
            {
                OTC_page._calc = response;
                OTC_page._arrCalc = response.split('|');
                OTC_page.enterValue(OTC_page._arrCalc[9] + " " + OTC_page._arrCalc[31], "spnCharge", true);
                OTC_page.enterValue(OTC_page._arrCalc[3] + " " + OTC_page._arrCalc[29], "spnBenefAmount", true);
                OTC_page.enterValue("PHP " + OTC_page._arrCalc[28], "spnPHPAmount", true);
                //OTC_page.getChargesDefaults(internet_page._arrCalc[35]);
            }
            else
            {
                OTC_page.showAlert("There is no applicable charge. Please report immediately to your branch admin.");
            }
        }
    }
    
    // **************************************************************************************** //
    // *** Page Functions and Methods
    // **************************************************************************************** //
    this.inititalizePage = function()
    {
        OTC_page.getLocation();
        OTC_page.getSearchBy();
        
        OTC_page._lookupField = "ChargeType";
        OTC_page.getLookup();
        
        OTC_page._lookupField = "Computation";
        OTC_page.getLookup();
        
        OTC_page._lookupField = "Currency";
        OTC_page.getLookup();
        
        OTC_page._lookupField = "Paymode";
        OTC_page.getLookup();
        
        OTC_page.getLookupDefaults("Currency");
        OTC_page.getDefaults();
        
        var Message = '';
        Message = "Warning! Remitter is OFAC listed. Please verify some IDs before processing transaction." + "\n\r\n\r";
        Message += "Warning! Remitter is AMLA listed. Please verify some IDs before processing transaction.";
        OTC_page.enterValue(Message, "txtMessage", false);
    }
    
    this.showRemitterBeneficiary = function()
    {
        OTC_page.clearText("Remitter");
        OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitter, "RemitterCIFID", 0), "lblRemitterID", false);
        OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitter, "FirstName", 0) + " " + OTC_page.getTableValue(OTC_page.tblRemitter, "MiddleName", 0) + " " + OTC_page.getTableValue(OTC_page.tblRemitter, "LastName", 0), "spnRemitterName", true);
        OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitter, "Address1", 0) + " " + OTC_page.getTableValue(OTC_page.tblRemitter, "Address2", 0) + " " + OTC_page.getTableValue(OTC_page.tblRemitter, "Address3", 0) + " " + OTC_page.getTableValue(OTC_page.tblRemitter, "Address4", 0) + " " + OTC_page.getTableValue(OTC_page.tblRemitter, "Address5", 0) + " " + OTC_page.getTableValue(OTC_page.tblRemitter, "Address6", 0), "spnRemitterAddress", true);
        (OTC_page.getTableValue(OTC_page.tblRemitter, "CountryDesc", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitter, "CountryDesc", 0), "spnRemitterCountry", true) : OTC_page.enterValue("", "spnRemitterCountry", true);
        (OTC_page.getTableValue(OTC_page.tblRemitter, "TrueZipCode", 0) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitter, "TrueZipCode", 0), "spnRemitterZipCode", true) : OTC_page.enterValue("", "spnRemitterZipCode", true);
        OTC_page.showBeneficiary();
        OTC_page.hidePanel("divSearchGrid");
    }
    
    this.showBeneficiary = function()
    {
        OTC_page.clearText("Beneficiary");
        if (OTC_page.tblBeneficiary != "" && OTC_page.tblBeneficiary != null && OTC_page.tblBeneficiary != undefined)
        {
            var rowcount = (_Brow + 1) + " of " + (OTC_page.tblBeneficiary.Rows.length);
            OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "BeneficiaryCIFID", _Brow), "lblBeneficiaryID", false);
            OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "FirstName", _Brow) + " " + OTC_page.getTableValue(OTC_page.tblBeneficiary, "MiddleName", _Brow) + " " + OTC_page.getTableValue(OTC_page.tblBeneficiary, "LastName", _Brow), "spnBeneficiaryName", true);
            OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "Address1", _Brow) + " " + OTC_page.getTableValue(OTC_page.tblBeneficiary, "Address2", _Brow) + " " + OTC_page.getTableValue(OTC_page.tblBeneficiary, "Address3", _Brow) + " " + OTC_page.getTableValue(OTC_page.tblBeneficiary, "Address4", _Brow) + " " + OTC_page.getTableValue(OTC_page.tblBeneficiary, "Address5", _Brow) + " " + OTC_page.getTableValue(OTC_page.tblBeneficiary, "Address6", _Brow), "spnBeneficiaryAddress", true);
            (OTC_page.getTableValue(OTC_page.tblBeneficiary, "CountryDesc", _Brow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "CountryDesc", _Brow), "spnBeneficiaryCountry", true) : OTC_page.enterValue("", "spnBeneficiaryCountry", true);
            (OTC_page.getTableValue(OTC_page.tblBeneficiary, "TrueZipCode", _Brow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "TrueZipCode", _Brow), "spnBeneficiaryZipCode", true) : OTC_page.enterValue("", "spnBeneficiaryZipCode", true);
            (OTC_page.getTableValue(OTC_page.tblBeneficiary, "ProductCodeDesc", _Brow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "ProductCodeDesc", _Brow), "spnProductCode", true) : OTC_page.enterValue("", "spnProductCode", true);
            (OTC_page.getTableValue(OTC_page.tblBeneficiary, "ProductCode", _Brow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "ProductCode", _Brow), "lblProductCode", false) : OTC_page.enterValue("", "lblProductCode", false);
            (OTC_page.getTableValue(OTC_page.tblBeneficiary, "ImplementationCode", _Brow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblBeneficiary, "ImplementationCode", _Brow), "lblImplementationCode", false) : OTC_page.enterValue("", "lblImplementationCode", false);
            OTC_page.enterValue(rowcount, "spnBeneficiaryCount", true);
        }
    }
    
    this.showRemitterCards = function()
    {
        var rowcount = (_RCrow + 1) + " of " + (OTC_page.tblRemitterCards.Rows.length);
        OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitterCards, "RemitterAcctSeqID", _RCrow), "lblRemitterAcctSeqID", false);
        (OTC_page.getTableValue(OTC_page.tblRemitterCards, "CardNo", _RCrow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitterCards, "CardNo", _RCrow), "txtPM1", false) : OTC_page.enterValue("", "txtPM1", false);
        (OTC_page.getTableValue(OTC_page.tblRemitterCards, "ExpiryDate", _RCrow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitterCards, "ExpiryDate", _RCrow), "txtPM2", false) : OTC_page.enterValue("", "txtPM2", false);
        (OTC_page.getTableValue(OTC_page.tblRemitterCards, "CurrencyCode", _RCrow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitterCards, "CurrencyCode", _RCrow), "txtPM3", false) : OTC_page.enterValue("", "txtPM3", false);
        (OTC_page.getTableValue(OTC_page.tblRemitterCards, "CardTypeDesc", _RCrow) != null) ? OTC_page.enterValue(OTC_page.getTableValue(OTC_page.tblRemitterCards, "CardTypeDesc", _RCrow), "txtPM4", false) : OTC_page.enterValue("", "txtPM4", false);
        OTC_page.enterValue(rowcount, "spnRemitterCount", true);
    }
    
    this.showRemitterAccounts = function()
    {
        
    }
    
    this.getCIF = function(type, isnext)
    {
        switch (type)
        {
            case "Remitter":
                OTC_page.clearText("RemitterInfo");
                switch (OTC_page.getValue("spnPaymentMethod", true))
                {
                    case "Cash (Notes)":
                        OTC_page.showAlert("This function is not available for this payment method.");
                        break;
                    case "Checks":
                        OTC_page.showAlert("This function is not available for this payment method.");
                        break;
                    case "Debit / Credit Card":
                        if (OTC_page.tblRemitterCards != "" && OTC_page.tblRemitterCards != undefined)
                        {
                            if (isnext == true)
                            {
                                _RCrow = _RCrow + 1;
                            }
                            else
                            {
                                _RCrow = _RCrow - 1;
                            }
                            
                            if (_RCrow > (OTC_page.tblRemitterCards.Rows.length - 1))
                            {
                                OTC_page.showAlert("You have reached the last card for this remitter.");
                                _RCrow = OTC_page.tblRemitterCards.Rows.length - 1;
                            }
                            else
                            {
                                if (_RCrow == -1)
                                {
                                    OTC_page.showAlert("You have reached the first card for this remitter.");
                                    _RCrow = 0;
                                }
                                else
                                {
                                    OTC_page.showRemitterCards();
                                }
                            }
                        }
                        else
                        {
                            OTC_page.showAlert("There is no cards maintained for this remitter. Please choose another payment method.");
                            OTC_page.tblRemitterCards = "";
                        }
                        break;
                    case "PNBE Account":
                        if (OTC_page.tblRemitterAccounts != undefined && OTC_page.tblRemitterAccounts != "")
                        {
                            if (isnext == true)
                            {
                                _RArow = _RArow + 1;
                            }
                            else
                            {
                                _RArow = _RArow - 1;
                            }
                            
                            if (_RArow > (OTC_page.tblRemitterAccounts.Rows.length - 1))
                            {
                                OTC_page.showAlert("You have reached the last account for this remitter.");
                                _RArow = OTC_page.tblRemitterAccounts.Rows.length - 1;
                            }
                            else
                            {
                                if (_RArow == -1)
                                {
                                    OTC_page.showAlert("You have reached the first account for this remitter.");
                                    _RArow = 0;
                                }
                                else
                                {
                                    OTC_page.showRemitterAccounts();
                                }
                            }
                        }
                        else
                        {
                            OTC_page.showAlert("There is no accounts maintained for this remitter. Please choose another payment method.");
                            OTC_page.tblRemitterAccounts = "";
                        }
                        break;
                }
                break;
            case "Beneficiary":
                OTC_page.clearText("BeneficiaryInfo");
                if (OTC_page.tblBeneficiary == "" && OTC_page.tblBeneficiary == undefined)
                {
                    OTC_page.showAlert("There is no beneficiary. Please select another remitter.");
                }
                else
                {
                    if (isnext == true)
                    {
                        _Brow = _Brow + 1;
                    }
                    else
                    {
                        _Brow = _Brow - 1;
                    }
                    
                    if (_Brow > (OTC_page.tblBeneficiary.Rows.length - 1))
                    {
                        OTC_page.showAlert("You have reached the last beneficiary for this remitter.");
                        _Brow = OTC_page.tblBeneficiary.Rows.length - 1;
                    }
                    else
                    {
                        if (_Brow == -1)
                        {
                            OTC_page.showAlert("You have reached the first beneficiary for this remitter.");
                            _Brow = 0;
                        }
                        else
                        {
                            OTC_page.showBeneficiary();
                        }
                    }
                }
                break;
        }
    }
    
    this.clearText = function(type)
    {
        switch (type)
        {
            case "Remitter":
                OTC_page.removeValue("spnRemitterName", true);
                OTC_page.removeValue("spnRemitterAddress", true);
                OTC_page.removeValue("spnRemitterCountry", true);
                OTC_page.removeValue("spnRemitterZipCode", true);
                OTC_page.removeValue("spnPaymentMethod", true);
                OTC_page.removeValue("txtPM1", false);
                OTC_page.removeValue("txtPM2", false);
                OTC_page.removeValue("txtPM3", false);
                OTC_page.removeValue("txtPM4", false);
                OTC_page.removeValue("lblPaymentMethod", false);
                OTC_page.removeValue("lblRemitterID", false);
                OTC_page.removeValue("lblRemitterAcctSeqID", false);
                break;
            case "RemitterInfo":
                OTC_page.removeValue("txtPM1", false);
                OTC_page.removeValue("txtPM2", false);
                OTC_page.removeValue("txtPM3", false);
                OTC_page.removeValue("txtPM4", false);
                OTC_page.removeValue("lblPaymentMethod", false);
                OTC_page.removeValue("lblRemitterID", false);
                OTC_page.removeValue("lblRemitterAcctSeqID", false);
                break;
            case "Beneficiary":
                OTC_page.removeValue("spnBeneficiaryName", true);
                OTC_page.removeValue("spnBeneficiaryAddress", true);
                OTC_page.removeValue("spnBeneficiaryCountry", true);
                OTC_page.removeValue("spnBeneficiaryZipCode", true);
                OTC_page.removeValue("spnBeneficiaryMaintainingBranch", true);
                OTC_page.removeValue("spnBeneficiaryAccountNo", true);
                OTC_page.removeValue("spnBeneficiaryAccountType", true);
                OTC_page.removeValue("spnBeneficiaryAccountCurrency", true);
                OTC_page.removeValue("spnBeneficiaryOtherBanks", true);
                OTC_page.removeValue("lblBeneficiaryAcctSeqID", false);
                OTC_page.removeValue("lblBeneficiaryID", false);
                break;
            case "BeneficiaryInfo":
                OTC_page.removeValue("spnBeneficiaryMaintainingBranch", true);
                OTC_page.removeValue("spnBeneficiaryAccountNo", true);
                OTC_page.removeValue("spnBeneficiaryAccountType", true);
                OTC_page.removeValue("spnBeneficiaryAccountCurrency", true);
                OTC_page.removeValue("spnBeneficiaryOtherBanks", true);
                OTC_page.removeValue("lblBeneficiaryAcctSeqID", false);
                OTC_page.removeValue("lblBeneficiaryID", false);
                break;
        }
    }
    
    this.showInput = function(element, isshow)
    {
        var input = document.getElementById(element);
        (isshow == true) ? input.style.borderStyle = "" : input.style.borderStyle = "none";
        (isshow == true) ? input.style.borderTopStyle = "" : input.style.borderTopStyle = "none";
        (isshow == true) ? input.style.borderRightStyle = "" : input.style.borderRightStyle = "none";
        (isshow == true) ? input.style.borderBottomStyle = "" : input.style.borderBottomStyle = "none";
        (isshow == true) ? input.style.borderLeftStyle = "" : input.style.borderLeftStyle = "none";
        (isshow == true) ? input.readOnly = false : input.readOnly = true;
    }
    
    this.setChargeType = function(seqid)
    {
        OTC_page.enterValue(OTC_page.findValue(OTC_page.tblChargeType, seqid, "SEQ_ID", "FldDescription"), "spnChargeType", true);
        OTC_page.enterValue(OTC_page.findValue(OTC_page.tblChargeType, seqid, "SEQ_ID", "FldValue"), "lblChargeType", false);
        OTC_page.hidePanel("divSearchGrid");
    }
    
    this.setPaymentMethod = function(seqid)
    {
        var paymode = OTC_page.findValue(OTC_page.tblPayMode, seqid, "SEQ_ID", "FldDescription");
        OTC_page.enterValue(paymode, "spnPaymentMethod", true);
        OTC_page.enterValue(OTC_page.findValue(OTC_page.tblPayMode, seqid, "SEQ_ID", "FldValue"), "lblPaymentMethod", false);
        OTC_page.hidePanel("divSearchGrid");
        
        OTC_page.showInput("txtPM1", false);
        OTC_page.showInput("txtPM2", false);
        OTC_page.showInput("txtPM3", false);
        OTC_page.showInput("txtPM4", false);
        
        switch (paymode)
        {
            case "Cash (Notes)":
                OTC_page.enterValue("", "spnPM1", true);
                OTC_page.enterValue("", "spnPM2", true);
                OTC_page.enterValue("", "spnPM3", true);
                OTC_page.enterValue("", "spnPM4", true);
                break;
            case "Checks":
                OTC_page.enterValue("Check No.", "spnPM1", true);
                OTC_page.enterValue("Bank", "spnPM2", true);
                OTC_page.enterValue("Currency", "spnPM3", true);
                OTC_page.enterValue("Clearing Date", "spnPM4", true);
                OTC_page.showInput("txtPM1", true);
                OTC_page.showInput("txtPM2", true);
                OTC_page.showInput("txtPM3", false);
                OTC_page.showInput("txtPM4", true);
                OTC_page.showFocus("txtPM1");
                break;
            case "Debit / Credit Card":
                OTC_page.enterValue("Card No.", "spnPM1", true);
                OTC_page.enterValue("Card Expiry", "spnPM2", true);
                OTC_page.enterValue("Currency", "spnPM3", true);
                OTC_page.enterValue("Card Class", "spnPM4", true);
                OTC_page.getRemitterCards();
                break;
            case "PNBE Account":
                OTC_page.enterValue("Account No.", "spnPM1", true);
                OTC_page.enterValue("Account Type", "spnPM2", true);
                OTC_page.enterValue("Currency", "spnPM3", true);
                OTC_page.enterValue("", "spnPM4", true);
                OTC_page.getRemitterAccounts();
                break;
        }
    }
    
    this.showHelp = function(caption)
    {
        switch (caption)
        {
            case "PM":
                OTC_page.enterValue("Payment Method - mode of payment to be used (e.g. Cash, Debit/Credit Card, Checks, etc.)", "spnHelp", true);
                break;
        }
        OTC_page.showControl("divHelp", true);
        OTC_page.assignPosition("divHelp");
    }
    
    this.hideHelp = function()
    {
        OTC_page.removeValue("spnHelp", true);
        OTC_page.showControl("divHelp", false);
    }
    
    this.callSearch = function()
    {
        if (window.event.keyCode == 13) // Enter key
        {
            OTC_page.searchLink("Search", false);
        }
    }
    
    this.getComputation = function()
    {
        if (window.event.keyCode == 13) // Enter key
        {
            OTC_page.computeAmount();
        }
    }
    
    this.computeAmount = function()
    {
        OTC_page.getCharges();
    }
    
    this.searchLink = function(search, issearch)
    {
        _searchType = search;
        OTC_page.enterValue("", "divSearchGridTop", true);
        OTC_page.showLoading("Retrieving Information... Please wait a moment.", false);
        OTC_page.load_grid_viewer();
        
        switch (_searchType)
        {
            case "Search":
                OTC_page.showControl("btnSearch", true);
                if (issearch == true)
                {
                    OTC_page.showSearch("", "divSearch");
                    OTC_page.showFocus("txtSCIFID");
                }
                else
                {
                    OTC_page.showSearch("divSearch", "divSearchGrid");
                }
                OTC_page.clearSearch();
                break;
            case "ChargeType":
                OTC_page.showControl("btnSearch", true);
                if (OTC_page.getValue("spnRemitterName", true) != null && OTC_page.getValue("spnRemitterName", true) != "")
                {
                    OTC_page.showSearch("", "divSearchGrid");
                }
                else
                {
                    OTC_page.showAlert("Please select a remitter before getting a charge type.");
                }
                break;
            case "Payment":
                OTC_page.showControl("btnSearch", false);
                if (OTC_page.getValue("spnRemitterName", true) != null && OTC_page.getValue("spnRemitterName", true) != "")
                {
                    OTC_page.showSearch("", "divSearchGrid");
                }
                else
                {
                    OTC_page.showAlert("Please select a remitter before getting a payment method.");
                }
                break;
            case "BeneficiaryAccounts":
                OTC_page.showControl("btnSearch", false);
                if (OTC_page.getValue("spnProductCode", true) != null && OTC_page.getValue("spnProductCode", true) != "")
                {
                    OTC_page.showSearch("", "divSearchGrid");
                }
                else
                {
                    OTC_page.showAlert("Please select a beneficiary before getting an account.");
                }
                break;
        }
        
        OTC_page.showLoading("", true);
        //OTC_page.panel_search(); //BIG PANEL
    }
    
    this.clearSearch = function()
    {
        var select = document.getElementById("ddlSearchBy");
        select.selectedIndex = -1;
        OTC_page.removeValue("txtSCIFID", false);
        OTC_page.removeValue("txtSLastName", false);
        OTC_page.removeValue("txtSFirstName", false);
        OTC_page.removeValue("txtSMiddleName", false);
    }
    
    this.showSearch = function(divhide, divshow)
    {
        if (divhide != '' && divhide != null)
        {
            OTC_page.hidePanel(divhide);
        }
        if (divshow != '' && divshow != null)
        {
            OTC_page.showPanel(divshow);
        }
    }
    
    // **************************************************************************************** //
    // *** Common Functions and Methods
    // **************************************************************************************** //
    this.submitQuit = function()
    {
        if (confirm("Are you sure you want to exit?"))
        {
            window.close();
        }
    }
    
    this.clearDDL = function(element)
    {
        var select = document.getElementById(element);
        select.options.length = 0;
    }
    
    this.addValue = function(value, element)
    {
        document.getElementById(element).value = document.getElementById(element).value + value;
    }
    
    this.edButtons = function(button, isdisable)
    {
        if (isdisable == true)
        {
            document.getElementById(button).disabled = "disabled";
        }
        else
        {
            document.getElementById(button).disabled = "";
        }
    }
    
    this.getValue = function(element, isspan)
    {
        return (isspan == true) ? document.getElementById(element).innerHTML : document.getElementById(element).value;
    }
    
    this.enterValue = function(value, element, isspan)
    {
        (isspan == true) ? document.getElementById(element).innerHTML = value : document.getElementById(element).value = value;
    }
    
    this.removeValue = function(element, isspan)
    {
        (isspan == true) ? document.getElementById(element).innerHTML = "" : document.getElementById(element).value = "";
    }
    
    this.showControl = function(element, isvisible)
    {
        (isvisible == true) ? document.getElementById(element).style.display = "block" : document.getElementById(element).style.display = "none";
    }
    
    this.assignPosition = function(element)
    {
        document.getElementById(element).style.left = (window.event.clientX + 10) + "px";
		document.getElementById(element).style.top = (window.event.clientY + 10) + "px";
    }
    
    this.showFocus = function(element)
    {
        document.getElementById(element).focus();
    }
    
    this.showLoading = function(message, ishide)
    {
        var span = document.getElementById('spnLoading');
        if (ishide == true)
        {
            span.innerHTML = '';
        }
        else
        {
            span.innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;" + message;
        }
    }
    
    this.showAlert = function(message)
    {
        (message != "") ? alert(message) : alert("An error occured while getting information. Please report to the nearest PNB branch.");
    }
    
    this.closeWindow = function()
    {
        window.close();
    }
    
    this.concatString = function(table, element, value, concatvalue)
    {
        var returnvalue = "";
        
        for (var i = 0; i <= (table.Rows.length - 1); i++)
        {
            returnvalue = returnvalue + table.Rows[i][value] + concatvalue;
        }
        returnvalue = returnvalue.substring(0, returnvalue.length - 1);
        OTC_page.enterValue(returnvalue, element, false);
    }
    
    this.getTableValue = function(table, fieldValue, row)
    {
        var arr = new Array();
        if (table.Rows.length != -1)
        {
            arr = table.Rows[row];
            return arr[fieldValue];
        }
    }

    this.findValue = function(table, matchValue, fieldValue, returnField)
    {
        var returnValue = "";
        var iRow = table;
        var sMatchValue = matchValue.split("|");
        var sFieldValue = fieldValue.split("|");
        var boolMatch = new Array();
        var iOk = 0;
        var count = 0;
        
        if (iRow.Rows.length > 0)
        {
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var j = 0; j <= (sMatchValue.length - 1); j++)
                {
                    if (iRow.Rows[i][sFieldValue[j]] == sMatchValue[j])
                    {
                        boolMatch[count] = true;
                        count = count + 1;
                    }
                }
                if (boolMatch[0] != undefined && boolMatch[0] != null)
                {
                    if (boolMatch.length > 0)
                    {
                        for (var k = 0; k <= (boolMatch.length - 1); k++)
                        {
                            if (boolMatch[k] == true)
                            {
                                iOk = iOk + 1;
                            }
                        }
                        if (iOk == boolMatch.length)
                        {
                            returnValue = iRow.Rows[i][returnField];
                            break;
                        }
                    }
                }
            }
        }
        return returnValue;
    }
    
    this.fillDDL = function(table, element, description, value, selected, delimeter)
    {
        var select = document.getElementById(element);
        
        if (select.length == 0)
        {
            var iRow = table;
            var arrSplit = description.split("|");
            var arrValue = "";
            var arrIDSplit = value.split("|");
            var arrIDValue = "";
            
            select.options[0] = new Option("", "");
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                    }
                    else
                    {
                        arrValue = arrValue + iRow.Rows[i][arrSplit[arr]] + ((delimeter == "") ? " • " : delimeter);
                    }
                }
                
                for (var arrID = 0; arrID <= (arrIDSplit.length - 1); arrID++)
                {
                    if ((arrIDSplit.length - 1) == arrID)
                    {
                        arrIDValue = arrIDValue + iRow.Rows[i][arrIDSplit[arrID]];
                    }
                    else
                    {
                        arrIDValue = arrIDValue + iRow.Rows[i][arrIDSplit[arrID]] + "|";
                    }
                }
                //select.options[i + 1] = new Option(arrValue, iRow.Rows[i][value]);
                select.options[i + 1] = new Option(arrValue, arrIDValue);
                arrValue = "";
                arrIDValue = "";
            }
            OTC_page.getDDL(element, selected, true);
        }
    }
    
    this.getDDL = function(element, selected, isValue)
    {
        var select = document.getElementById(element);
        document.getElementById(selected).value = (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
    }

    this.getDDLValue = function(element, isValue)
    {
        var select = document.getElementById(element);
        return (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
    }
    
    this.isNumeric = function(value)
    {
        if (value == parseInt(value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        OTC_page.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {
        OTC_page.showControl(divName, false);
        divmask.hide();
    }
    
    var maskpanel = function()
    {
        this.divobj;
        this.show = function()
        {
            if(!document.getElementById("xdivmasking"))
            {
                var divEle = document.createElement("div");
                divEle.setAttribute("id", "xdivmasking");
                document.body.appendChild(divEle);
                
                var divSty = document.getElementById("xdivmasking").style;
                divSty.position = "absolute"; 
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "46";
                divSty.opacity = ".50";
                divSty.backgroundColor="#000";
                divSty.filter = "alpha(opacity=50)";
     
                var divFram = document.createElement("iframe");
                divFram.setAttribute("id", "xmaskframe");
                document.body.appendChild(divFram);
                divSty = document.getElementById("xmaskframe").style;
                divSty.position = "absolute"; 
                divSty.top = "0px"; 
                divSty.left = "0px";
                divSty.zIndex = "45";
                divSty.border = "none";
                divSty.filter = "alpha(opacity=0)";
          }
     
          this.divobj = document.getElementById("xdivmasking");
          this.waitifrm = document.getElementById("xmaskframe");
            
          var dsh = document.documentElement.scrollHeight;
          var dch = document.documentElement.clientHeight;
          var dsw = document.documentElement.scrollWidth;
          var dcw = document.documentElement.clientWidth;
          var bdh = (dsh>dch) ? dsh:dch;
          var bdw = (dsw>dcw) ? dsw:dcw;
     
          this.waitifrm.style.height = this.divobj.style.height = bdh + "px";
          this.waitifrm.style.width = this.divobj.style.width = bdw + "px";
          this.waitifrm.style.display = this.divobj.style.display = "block";
       }
       
       this.hide = function()
       {
           this.waitifrm.style.display = this.divobj.style.display = "none";
       }   
    }
    
    // **************************************************************************************** //
    // *** AJAX GRID
    // **************************************************************************************** //
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != undefined)
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    switch (_searchType)
                    {
                        case "Search":
                            OTC_page.getRemitter(command_param[1]);
                            break;
                        case "ChargeType":
                            OTC_page.setChargeType(command_param[1]);
                            break;
                        case "Payment":
                            OTC_page.enterValue(command_param[1], "lblPaymentMethod", false);
                            OTC_page.setPaymentMethod(command_param[1]);
                            break;
                        case "BeneficiaryAccounts":
                            OTC_page.enterValue(command_param[1], "lblBeneficiaryAcctSeqID", false);
                            OTC_page.getBeneficiaryAccounts(command_param[1], false);
                            break;
                    }
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
    this.load_grid_viewer = function()
    {
        this.process_grid_viewer("ajx_loading");
    }
    
    this.process_grid_viewer = function(parameter)
    {
        var sSQL = "";
        switch (_searchType)
        {
            case "Search":
                sSQL = "osp_getSearch~" + OTC_page.getValue("lblCIFClass", false) + "~" + OTC_page.getValue("txtSCIFID", false) + "~" + OTC_page.getValue("txtSLastName", false) + "~" + OTC_page.getValue("txtSFirstName", false) + "~" + OTC_page.getValue("txtSMiddleName", false);
                break;
            case "ChargeType":
                sSQL = "osp_getLookupDefaults~" + OTC_page.getValue("lblLocation", false) + "~ChargeType";
                break;
            case "Payment":
                sSQL = "osp_getLookupDefaults~" + OTC_page.getValue("lblLocation", false) + "~PayMode";
                break;
            case "BeneficiaryAccounts":
                sSQL = "osp_getBeneficiaryAccounts~" + OTC_page.getValue("lblBeneficiaryID", false) + "~true";
                break;
        }
        
        this.grid = new grid_viewer();
        this.grid.sql_source        = sSQL;
        this.grid.div_container     = document.getElementById("divSearchGridTop");
        this.grid.datarow_column    = 0;
        this.grid.page_size         = 15;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    // **************************************************************************************** //
    // *** BIG PANEL
    // **************************************************************************************** //
    this.panel_search = function()
    {
        var grid_ajax_page = "../AjxPage/ajaxgridview.aspx";
        var SEQ_ID = '';
        //var grid_ajax_page = "../AjxPage/ajaxgrid.aspx";
        
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "osp_getSearch~" + OTC_page.getDDL("ddlSearchBy", "lblCIFClass", true) + "~" + OTC_page.getValue("txtSCIFID", false) + "~" + OTC_page.getValue("txtSLastName", false) + "~" + OTC_page.getValue("txtSFirstName", false) + "~" + OTC_page.getValue("txtSMiddleName", false);
        this.ajax_grid.open_selection_grid(grid_ajax_page, this.grid_ajax_query, 0, SEQ_ID );
        OTC_page.showSearch("divSearch", "divSearchGrid");
    }
}