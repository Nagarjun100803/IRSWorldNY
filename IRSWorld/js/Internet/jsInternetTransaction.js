
function Internet_Transaction()
{

    this.Internet_Sidebar_class = null;
    this.rem_commons_class      = null;
    this.txn_save               = null;
    this.SEQ_ID                 = null;
    this.UserID                 = null;
    this.RemitMethod            = null;
    this.RemitStationID         = null;
    this.RemittingBranch        = null;
    this.ReferenceNo            = null;
    this.ExtlReferenceNo        = null;
    this.Transtatus             = null;
    this.TranxDate              = null;
    this.ProductChargesID       = null;
    this.RemitterCIFID          = null;
    this.BeneficiaryCIFID       = null;
    this.AccountSEQID           = null;
    this.RemittingCurrency      = null;
    this.GrossRemAmount         = null;
    this.NetRemAmount           = null;
    this.NettoBeneCurrency      = null;
    this.NettoBeneAmount        = null;
    this.TotalLocalCharge       = null;
    this.TransactionSource      = null;
    this.ReceiptMode            = null;
    this.TransactionPurpose     = null;
    this.OtherPurpose           = null;
    this.SpecialMessage         = null;
    this.DateImplemented        = null;
    this.DateSent               = null;
    this.printed                = null;
    this.ForexSourceToTarget    = null;
    this.ForexTargetToUSD       = null;
    this.ForexTargetToPHP       = null;
    this.ForexSourceToUSD       = null;
    this.ForexSourceToPHP       = null;
    this.ForexTargetToAccount   = null;
    this.SourceofFunds          = null;
    var protype = null;
    
    this.load = function()
    {
         this.construct_side_bar();
         this.name();
         this.load_remitter_transactions();
         this.load_beneficiary_prod_imp();
    }
    

    this.construct_side_bar = function()
    {
        this.rem_commons_class= new rem_commons(); 
        this.Internet_Sidebar_class = new Internet_Sidebar(); 
        document.getElementById("divtdInternetTransactionMenu").innerHTML = this.Internet_Sidebar_class.get_side_bar();
    }
      
    this.set_side_bar_visibility = function(division_id, anchor_id)
    {
        this.Internet_Sidebar_class.get_side_bar_visibility(division_id, anchor_id);
      
    }
    
    this.name = function()
    {
        this.fill_remname();
        this.fill_benename();
          
    }
    this.fill_remname = function()
    {
        InternetTransaction.get_remname(document.getElementById("txtHiddenRemCIFID").value, this.get_tranremname)
    }
    
    this.get_tranremname = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetTransactionRemName").innerHTML    = '';
        document.getElementById("lblInternetTransactionRemAddress").innerHTML = '';
        document.getElementById("lblInternetTransactionRemName").innerHTML = res.Tables[0].Rows[0].RemFullName;
        document.getElementById("lblInternetTransactionRemAddress").innerHTML = res.Tables[0].Rows[0].Address;
    }
    
    this.fill_benename = function()
    {
        InternetTransaction.get_benename(document.getElementById("txtHiddenBeneCIFID").value, this.get_tranbenename)
    }
    
    this.get_tranbenename = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetTransactionBeneName").innerHTML        = '';
        document.getElementById("lblInternetTransactionBenefAddress").innerHTML    = '';
        document.getElementById("lblInternetTransactionBeneName").innerHTML = res.Tables[0].Rows[0].BeneFullName;
        document.getElementById("lblInternetTransactionBenefAddress").innerHTML = res.Tables[0].Rows[0].Address;
    }
    this.rapid = function()
    {
        this.open_division("divInternetRapidRemit");
        this.fill_rapidname();
        this.rapid_account();
//        this.execute_rapid_txn_product_main_loading();

    }
    
    this.gfmc = function()
    {
        this.fill_gfmcname();
        this.gfmc_account();
        this.dropdown_currency();
        this.open_division("divInternetGFMC");
//        this.execute_gfmc_txn_product_main_loading();
    }
    
    this.credit = function()
    {
        this.fill_creditname();
        this.credit_account();
        this.open_division("divInternetCreditToAccount");
//        this.execute_credit_txn_product_main_loading();
    }
    
    this.fill_rapidname = function()
    {
        document.getElementById("lblInternetRRRemName").innerHTML    = '';
        document.getElementById("lblInternetRRRemAddress").innerHTML = '';
        document.getElementById("lblInternetRRRemName").innerHTML = document.getElementById("lblInternetTransactionRemName").innerHTML;
        document.getElementById("lblInternetRRRemAddress").innerHTML = document.getElementById("lblInternetTransactionRemAddress").innerHTML;
        document.getElementById("lblInternetRRRemBeneName").innerHTML        = '';
        document.getElementById("lblInternetRRRemBeneAddress").innerHTML    = '';
        document.getElementById("lblInternetRRRemBeneName").innerHTML        = document.getElementById("lblInternetTransactionBeneName").innerHTML;
        document.getElementById("lblInternetRRRemBeneAddress").innerHTML    = document.getElementById("lblInternetTransactionBenefAddress").innerHTML;
    }
    
    this.fill_gfmcname = function()
    {
        document.getElementById("lblInternetGFMCRemName").innerHTML    = '';
        document.getElementById("lblInternetGFMCRemAddress").innerHTML = '';
        document.getElementById("lblInternetGFMCRemName").innerHTML = document.getElementById("lblInternetTransactionRemName").innerHTML;
        document.getElementById("lblInternetGFMCRemAddress").innerHTML = document.getElementById("lblInternetTransactionRemAddress").innerHTML;
        document.getElementById("lblInternetGFMCRemBeneName").innerHTML        = '';
        document.getElementById("lblInternetGFMCRemBeneAddress").innerHTML    = '';
        document.getElementById("lblInternetGFMCRemBeneName").innerHTML        = document.getElementById("lblInternetTransactionBeneName").innerHTML;
        document.getElementById("lblInternetGFMCRemBeneAddress").innerHTML    = document.getElementById("lblInternetTransactionBenefAddress").innerHTML;
    }
    
    this.fill_creditname = function()
    {
        document.getElementById("lblInternetCARemName").innerHTML    = '';
        document.getElementById("lblInternetCARemAddress").innerHTML = '';
        document.getElementById("lblInternetCARemName").innerHTML = document.getElementById("lblInternetTransactionRemName").innerHTML;
        document.getElementById("lblInternetCARemAddress").innerHTML = document.getElementById("lblInternetTransactionRemAddress").innerHTML;
        document.getElementById("lblInternetCARemBeneName").innerHTML        = '';
        document.getElementById("lblInternetCARemBeneAddress").innerHTML    = '';
        document.getElementById("lblInternetCARemBeneName").innerHTML        = document.getElementById("lblInternetTransactionBeneName").innerHTML;
        document.getElementById("lblInternetCARemBeneAddress").innerHTML    = document.getElementById("lblInternetTransactionBenefAddress").innerHTML;
    }
    
    this.rapid_account = function()
    {
        InternetTransaction.get_account(document.getElementById("txtHiddenBeneCIFID").value, this.get_rapidaccount)
    }
    
    this.get_rapidaccount = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetRRRemPNBBranch").innerHTML        = '';
        document.getElementById("lblInternetRRRemBeneCASANo").innerHTML    = '';
        document.getElementById("lblInternetRRRemBeneAccountType").innerHTML    = '';
        document.getElementById("lblInternetRRRemBeneACCurrency").innerHTML    = '';
        document.getElementById("lblInternetRRRemPNBBranch").innerHTML = res.Tables[0].Rows[0].BranchCode;
        document.getElementById("lblInternetRRRemBeneCASANo").innerHTML = res.Tables[0].Rows[0].AccountNumber;
        document.getElementById("lblInternetRRRemBeneAccountType").innerHTML = res.Tables[0].Rows[0].AccountType;
        document.getElementById("lblInternetRRRemBeneACCurrency").innerHTML = res.Tables[0].Rows[0].Currency;
    }
    
    this.gfmc_account = function()
    {
        InternetTransaction.get_account(document.getElementById("txtHiddenBeneCIFID").value, this.get_gfmcaccount)
    }
    
    this.get_gfmcaccount = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetGFMCPNBBranch").innerHTML        = '';
        document.getElementById("lblInternetGFMCCaSaNo").innerHTML    = '';
        document.getElementById("lblInternetGFMCAccountType").innerHTML    = '';
        document.getElementById("lblInternetGFMCACCurrency").innerHTML    = '';
        document.getElementById("lblInternetGFMCPNBBranch").innerHTML = res.Tables[0].Rows[0].BranchCode;
        document.getElementById("lblInternetGFMCCaSaNo").innerHTML = res.Tables[0].Rows[0].AccountNumber;
        document.getElementById("lblInternetGFMCAccountType").innerHTML = res.Tables[0].Rows[0].AccountType;
        document.getElementById("lblInternetGFMCACCurrency").innerHTML = res.Tables[0].Rows[0].Currency;
    }
    
    this.dropdown_currency = function()
    {
        InternetTransaction.getdropdown_currency("Currency", this.dropdown_currency_callback)
    }
    
    this.dropdown_currency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlInternetGFMCRemACCurrency", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.generate_avail = function()
    {
        this.generate_rdo_enable(document.getElementById("rdoInternetGFMCRemPurchase"));
        this.generate_text_disable(document.getElementById("txtInternetGFMCRemGlobalCardNo"));
        this.generate_text_disable(document.getElementById("txtInternetGFMCRemIDPresented"));
    }
    
    this.generate_text_disable = function(text_id_disable)
    {

        var id  = text_id_disable;
        var cset = id.disabled;
        var clear = id.value;
        
        id.disabled = (cset == "false") ? "true" : "false";
        id.value = "";
    }
 
    this.generate_purchase = function()
    {
        this.generate_rdo_enable(document.getElementById("rdoInternetGFMCRemAvailment"));
        this.generate_text_enable(document.getElementById("txtInternetGFMCRemGlobalCardNo"));
        this.generate_text_enable(document.getElementById("txtInternetGFMCRemIDPresented"));

    }
    
    this.generate_rdo_enable = function(rdo_id)
    {

        var id  = rdo_id;
        var cset = id.checked;
        
        id.checked = cset == "false";
    }
    
    this.generate_text_enable = function(text_id_enable)
    {
        var id  = text_id_enable;
        var cset = id.disabled;
        
        id.disabled = cset == "false";
    }
    
    this.credit_account = function()
    {
        InternetTransaction.get_account(document.getElementById("txtHiddenBeneCIFID").value, this.get_creditaccount)
    }
    
    this.get_creditaccount = function(result)
    {
        var res = result.value;
        document.getElementById("lblInternetCARemPNBBranch").innerHTML        = '';
        document.getElementById("lblInternetCARemCASANo").innerHTML    = '';
        document.getElementById("lblInternetCARemAccountType").innerHTML    = '';
        document.getElementById("lblInternetCARemBeneACCurrency").innerHTML    = '';
        document.getElementById("lblInternetCARemPNBBranch").innerHTML = res.Tables[0].Rows[0].BranchCode;
        document.getElementById("lblInternetCARemCASANo").innerHTML = res.Tables[0].Rows[0].AccountNumber;
        document.getElementById("lblInternetCARemAccountType").innerHTML = res.Tables[0].Rows[0].AccountType;
        document.getElementById("lblInternetCARemBeneACCurrency").innerHTML = res.Tables[0].Rows[0].Currency;
    }
    
    this.load_remitter_transactions = function()
    {
        this.open_division("divInternetTransaction");
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_search_remitters_trans~" + document.getElementById("txtHiddenRemCIFID").value;
        this.grid.div_container  = document.getElementById("divInternetTransactionRemTran");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.load_beneficiary_prod_imp = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_search_benef_prod_imp~" + document.getElementById("txtHiddenBeneCIFID").value;
        this.grid.div_container  = document.getElementById("divInternetTransactionBeneImp");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;        
    }
    
    this.close_divisions = function()
    {
        document.getElementById("divInternetTransaction").style.display = 'none';
        document.getElementById("divInternetRapidRemit").style.display = 'none';
        document.getElementById("divInternetGFMC").style.display = 'none';
        document.getElementById("divInternetCreditToAccount").style.display = 'none';
        document.getElementById("divInternetTranView").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
//    this.compute = function()
//    { 
//        usdollar =  document.getElementById("txtInternetRRRemTARC").value; 
//        if(usdollar != "NaN" && usdollar != null && usdollar != "0" && usdollar != 0)
//        {
//             document.getElementById("txtInternetRRRemTARC").value = "";
//             document.getElementById("txtInternetRRRemTARC").value = usdollar;
//             document.getElementById("txtInternetRRRemTARC").value = document.getElementById("txtInternetRRRemTARC").value - document.getElementById("txtInternetRRRemNRARC").value;
//             var datenow = new Date();
//             var Month = (datenow.getMonth() + 1) + "/";
//             var Day = datenow.getDate() + "/";
//             var Year = datenow.getFullYear();
//             var Time = "12:00:00 AM";
//             var datenow = Month + Day + Year + " " + Time;
//             InternetTransaction.get_conversions(datenow, this.get_usdconversion)
//         }
//         else
//         {
//             alert("Please enter an amount in Total Amount (Rmt Ccy)!");
//         }
//    }
//    
//    this.get_usdconversion = function(result)
//    {
//        var res = result.value;
//        document.getElementById("txtInternetRRRemNRAPE").value = res.Tables[0].Rows[0].PesoConversion * document.getElementById("txtInternetRRRemNRARC").value;
//        var temp = document.getElementById("txtInternetRRRemNRAPE").value * 0.05; 
//        document.getElementById("txtInternetRRRemNTBACC").value = document.getElementById("txtInternetRRRemNRAPE").value - temp;
//        
//    }
//    
//    this.inverse = function()
//    { 
//        var DateTime = new Date();
//        var Month = (DateTime.getMonth() + 1) + "/";
//        var Day = DateTime.getDate() + "/";
//        var Year = DateTime.getFullYear();
//        var Time = "12:00:00 AM";
//        var DateTime = Month + Day + Year + " " + Time;
//        InternetTransaction.get_conversions(DateTime, this.get_pesoconversion)
//    }
//    
//    this.get_pesoconversion = function(result)
//    {
//        var res = result.value;
//        document.getElementById("txtInternetRRRemNRARC").value = document.getElementById("txtInternetRRRemNRAPE").value / res.Tables[0].Rows[0].Conversion;
//        document.getElementById("txtInternetRRRemTARC").value = "";
//        document.getElementById("txtInternetRRRemTARC").value = usdollar;
//        document.getElementById("txtInternetRRRemTARC").value = document.getElementById("txtInternetRRRemTARC").value - document.getElementById("txtInternetRRRemNRARC").value;
//        var temp = document.getElementById("txtInternetRRRemNRAPE").value * 0.05; 
//        document.getElementById("txtInternetRRRemNTBACC").value = document.getElementById("txtInternetRRRemNRAPE").value - temp;
//    
//    }
//    
//    this.execute_rapid_txn_product_main_loading = function()
//    {
//        this.rem_commons_class.do_txn_product_main_loading();
//        this.load_rapid_remitter_and_beneficiary_info();
//    }
    
    this.load_rapid_remitter_and_beneficiary_info = function()
    {                        
        document.getElementById('lblInternetRRRemName').innerHTML            = this.rem_commons_class.remitter_fullname;
        document.getElementById('lblInternetRRRemAddress').innerHTML         = this.rem_commons_class.remitter_address;
        document.getElementById('lblInternetRRRemBeneName').innerHTML        = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblInternetRRRemBeneAddress').innerHTML     = this.rem_commons_class.beneficiary_address;
        document.getElementById('lblInternetRRRemPNBBranch').innerHTML       = this.rem_commons_class.beneficiary_pnb_branch_name;
        document.getElementById('lblInternetRRRemBeneACCurrency').innerHTML  = this.rem_commons_class.beneficiary_account_currency;
        document.getElementById('lblInternetRRRemBeneAccountType').innerHTML = this.rem_commons_class.beneficiary_account_type;
        document.getElementById('lblInternetRRRemBeneCASANo').innerHTML      = this.rem_commons_class.beneficiary_account_number;
    }
    
    this.execute_gfmc_txn_product_main_loading = function()
    {
        this.rem_commons_class.do_txn_product_main_loading();
        this.load_gfmc_remitter_and_beneficiary_info();
    }
    
    this.load_gfmc_remitter_and_beneficiary_info = function()
    {                        
        document.getElementById('lblInternetGFMCRemName').innerHTML            = this.rem_commons_class.remitter_fullname;
        document.getElementById('lblInternetGFMCRemAddress').innerHTML         = this.rem_commons_class.remitter_address;
        document.getElementById('lblInternetGFMCRemBeneName').innerHTML        = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblInternetGFMCRemBeneAddress').innerHTML     = this.rem_commons_class.beneficiary_address;
        document.getElementById('lblInternetGFMCPNBBranch').innerHTML       = this.rem_commons_class.beneficiary_pnb_branch_name;
        document.getElementById('lblInternetGFMCACCurrency').innerHTML  = this.rem_commons_class.beneficiary_account_currency;
        document.getElementById('lblInternetGFMCAccountType').innerHTML = this.rem_commons_class.beneficiary_account_type;
        document.getElementById('lblInternetGFMCCASANo').innerHTML      = this.rem_commons_class.beneficiary_account_number;
    }
    
    this.execute_credit_txn_product_main_loading = function()
    {
        this.rem_commons_class.do_txn_product_main_loading();
        this.load_credit_remitter_and_beneficiary_info();
    }
    
    this.load_credit_remitter_and_beneficiary_info = function()
    {                        
        document.getElementById('lblInternetCARemName').innerHTML            = this.rem_commons_class.remitter_fullname;
        document.getElementById('lblInternetCARemAddress').innerHTML         = this.rem_commons_class.remitter_address;
        document.getElementById('lblInternetCARemBeneName').innerHTML        = this.rem_commons_class.beneficiary_fullname;
        document.getElementById('lblInternetCARemBeneAddress').innerHTML     = this.rem_commons_class.beneficiary_address;
        document.getElementById('lblInternetCARemPNBBranch').innerHTML       = this.rem_commons_class.beneficiary_pnb_branch_name;
        document.getElementById('lblInternetCARemBeneACCurrency').innerHTML  = this.rem_commons_class.beneficiary_account_currency;
        document.getElementById('lblInternetCARemAccountType').innerHTML = this.rem_commons_class.beneficiary_account_type;
        document.getElementById('lblInternetCARemCASANo').innerHTML      = this.rem_commons_class.beneficiary_account_number;
    }
    
    this.load_transaction_view_details = function()
    {
        this.rem_commons_class.load_transaction_view_details()
    }
    
    this.set_database_column_value = function(fld_group, fld_value)
    {
        this.rem_commons_class.set_transaction_values(fld_group, fld_value);
    }
    
//    this.load_transaction_details = function(remname, remaddress, benename, beneaddress, branch, accno, currency, protype, totalamt, netremamt, netremconv, nettobene)
//    {        
//        this.clear_screen();
//        this.txn_save = new rem_saving_trans();
//        var datenow = new Date();
////        var ds = this.txn_save.get_column_description().value;
//        
////        if (ds != null)
////        {        
//            document.getElementById("lblInternetTVTranSource").innerHTML     = "";
//            document.getElementById("lblInternetTVSourceFund").innerHTML     = "";
//            document.getElementById("lblInternetTVTranStatus").innerHTML     = "";
//            document.getElementById("lblInternetTVPurpose").innerHTML        = "";
//            document.getElementById("lblInternetTVReceiptMode").innerHTML    = "";
//            document.getElementById("lblInternetTVRemName").innerHTML        = remname;
//            document.getElementById("lblInternetTVRemAddress").innerHTML     = remaddress;
//            document.getElementById("lblInternetTVBeneName").innerHTML       = benename;
//            document.getElementById("lblInternetTVBeneAddress").innerHTML    = beneaddress;
//            document.getElementById("lblInternetTVChargeType").innerHTML     = "";
//            document.getElementById("lblInternetTVProductType").innerHTML    = protype;
//            document.getElementById("lblInternetTVPayMode").innerHTML        = "";
//            document.getElementById("lblInternetTVImplementation").innerHTML = "";
//            document.getElementById("lblInternetTVRemitCurrency").innerHTML  = currency;
//            document.getElementById("lblInternetTVTranDate").innerHTML       = datenow;
//            document.getElementById("lblInternetTVDateToday").innerHTML      = datenow;
//            document.getElementById("lblInternetTVTARC").innerHTML           = totalamt;
//            document.getElementById("lblInternetNRAPE").innerHTML            = netremamt;
//            document.getElementById("lblInternetTVNRARC").innerHTML          = netremconv;
//            document.getElementById("lblInternetNTBACC").innerHTML           = nettobene;
//            document.getElementById("lblInternetTVRefNo").innerHTML          = accno;
//            
//            document.getElementById("lblInternetTVCoverBank").innerHTML      = branch;
//            /*document.getElementById("lblInternetTVLocalCurrency").innerHTML  = ds.Tables[0].Rows[0];
//            document.getElementById("lblInternetTVEuroToUSD").innerHTML      = ds.Tables[0].Rows[0];
//            document.getElementById("lblInternetTVUSDToEuro").innerHTML      = ds.Tables[0].Rows[0];
//            document.getElementById("lblInternetTVTARC").innerHTML           = ds.Tables[0].Rows[0];
//            document.getElementById("lblNRAPE").innerHTML            = ds.Tables[0].Rows[0];
//            document.getElementById("lblInternetTVNRARC").innerHTML          = ds.Tables[0].Rows[0];
//            document.getElementById("lblNTBACC").innerHTML           = ds.Tables[0].Rows[0];
//            document.getElementById("lblInternetTVCommStatus").innerHTML = ds.Tables[0].Rows[0];
//            
//            */
////        }
//    }      
    this.load_transaction_details = function(protype)
    {    
        this.open_division("divInternetTranView");    
        this.clear_screen();
        this.txn_save = new rem_saving_trans();
        var datenow = new Date();
        var Month = (datenow.getMonth() + 1) + "/";
        var Day = datenow.getDate() + "/";
        var Year = datenow.getFullYear();
//        var Time = datenow.();
        var datenow = Month + Day + Year ;
        
//        alert(protype);
        
        switch(protype)
        {
        case "Rapid Remit":
            {
                document.getElementById("lblInternetTVTranSource").innerHTML     = "";
                document.getElementById("lblInternetTVSourceFund").innerHTML     = "";
                document.getElementById("lblInternetTVTranStatus").innerHTML     = "";
                document.getElementById("lblInternetTVPurpose").innerHTML        = "";
                document.getElementById("lblInternetTVReceiptMode").innerHTML    = "";
                document.getElementById("lblInternetTVRemName").innerHTML        = document.getElementById('lblInternetRRRemName').innerHTML;
                document.getElementById("lblInternetTVRemAddress").innerHTML     = document.getElementById('lblInternetRRRemAddress').innerHTML;
                document.getElementById("lblInternetTVBeneName").innerHTML       = document.getElementById('lblInternetRRRemBeneName').innerHTML;
                document.getElementById("lblInternetTVBeneAddress").innerHTML    = document.getElementById('lblInternetRRRemBeneAddress').innerHTML;
                document.getElementById("lblInternetTVChargeType").innerHTML     = "";
                document.getElementById("lblInternetTVProductType").innerHTML    = protype;
                document.getElementById("lblInternetTVPayMode").innerHTML        = "";
                document.getElementById("lblInternetTVImplementation").innerHTML = "";
                document.getElementById("lblInternetTVRemitCurrency").innerHTML  = document.getElementById('lblInternetRRRemBeneACCurrency').innerHTML;
                document.getElementById("lblInternetTVTranDate").innerHTML       = datenow;
                document.getElementById("lblInternetTVDateToday").innerHTML      = datenow;
                document.getElementById("lblInternetTVTARC").innerHTML           = document.getElementById('txtInternetRRRemTARC').value;
                document.getElementById("lblInternetNRAPE").innerHTML            = document.getElementById('txtInternetRRRemNRARC').value;
                document.getElementById("lblInternetTVNRARC").innerHTML          = document.getElementById('txtInternetRRRemNRAPE').value;
                document.getElementById("lblInternetNTBACC").innerHTML           = document.getElementById('txtInternetRRRemNTBACC').value;
                document.getElementById("lblInternetTVRefNo").innerHTML          = document.getElementById('lblInternetRRRemBeneCASANo').innerHTML;
                document.getElementById("lblInternetTVCoverBank").innerHTML      = document.getElementById('lblInternetRRRemPNBBranch').innerHTML;
                break;
            }
           
            case "GFMC":
            {
                document.getElementById("lblInternetTVTranSource").innerHTML     = "";
                document.getElementById("lblInternetTVSourceFund").innerHTML     = "";
                document.getElementById("lblInternetTVTranStatus").innerHTML     = "";
                document.getElementById("lblInternetTVPurpose").innerHTML        = "";
                document.getElementById("lblInternetTVReceiptMode").innerHTML    = "";
                document.getElementById("lblInternetTVRemName").innerHTML        = document.getElementById('lblInternetGFMCRemName').innerHTML;
                document.getElementById("lblInternetTVRemAddress").innerHTML     = document.getElementById('lblInternetGFMCRemAddress').innerHTML;
                document.getElementById("lblInternetTVBeneName").innerHTML       = document.getElementById('lblInternetGFMCRemBeneName').innerHTML;
                document.getElementById("lblInternetTVBeneAddress").innerHTML    = document.getElementById('lblInternetGFMCRemBeneAddress').innerHTML;
                document.getElementById("lblInternetTVChargeType").innerHTML     = "";
                document.getElementById("lblInternetTVProductType").innerHTML    = protype;
                document.getElementById("lblInternetTVPayMode").innerHTML        = "";
                document.getElementById("lblInternetTVImplementation").innerHTML = "";
                document.getElementById("lblInternetTVRemitCurrency").innerHTML  = document.getElementById('lblInternetGFMCACCurrency').innerHTML;
                document.getElementById("lblInternetTVTranDate").innerHTML       = datenow;
                document.getElementById("lblInternetTVDateToday").innerHTML      = datenow;
                document.getElementById("lblInternetTVTARC").innerHTML           = document.getElementById('txtInternetGFMCRTARC').value;
                document.getElementById("lblInternetNRAPE").innerHTML            = document.getElementById('txtInternetGFMCNRARC').value;
                document.getElementById("lblInternetTVNRARC").innerHTML          = document.getElementById('txtInternetGFMCNRAPE').value;
                document.getElementById("lblInternetNTBACC").innerHTML           = document.getElementById('txtInternetGMFCNTBACC').value;
                document.getElementById("lblInternetTVRefNo").innerHTML          = document.getElementById('lblInternetGFMCCaSaNo').innerHTML;
                document.getElementById("lblInternetTVCoverBank").innerHTML      = document.getElementById('lblInternetGFMCPNBBranch').innerHTML;
                break;
            }
            case "Credit to Account":
            {
                document.getElementById("lblInternetTVTranSource").innerHTML     = "";
                document.getElementById("lblInternetTVSourceFund").innerHTML     = "";
                document.getElementById("lblInternetTVTranStatus").innerHTML     = "";
                document.getElementById("lblInternetTVPurpose").innerHTML        = "";
                document.getElementById("lblInternetTVReceiptMode").innerHTML    = "";
                document.getElementById("lblInternetTVRemName").innerHTML        = document.getElementById('lblInternetCARemName').innerHTML;
                document.getElementById("lblInternetTVRemAddress").innerHTML     = document.getElementById('lblInternetCARemAddress').innerHTML;
                document.getElementById("lblInternetTVBeneName").innerHTML       = document.getElementById('lblInternetCARemBeneName').innerHTML;
                document.getElementById("lblInternetTVBeneAddress").innerHTML    = document.getElementById('lblInternetCARemBeneAddress').innerHTML;
                document.getElementById("lblInternetTVChargeType").innerHTML     = "";
                document.getElementById("lblInternetTVProductType").innerHTML    = protype;
                document.getElementById("lblInternetTVPayMode").innerHTML        = "";
                document.getElementById("lblInternetTVImplementation").innerHTML = "";
                document.getElementById("lblInternetTVRemitCurrency").innerHTML  = document.getElementById('lblInternetCARemBeneACCurrency').innerHTML;
                document.getElementById("lblInternetTVTranDate").innerHTML       = datenow;
                document.getElementById("lblInternetTVDateToday").innerHTML      = datenow;
                document.getElementById("lblInternetTVTARC").innerHTML           = document.getElementById('txtInternetCARRemTARC').value;
                document.getElementById("lblInternetNRAPE").innerHTML            = document.getElementById('txtInternetCARRemNRARC').value;
                document.getElementById("lblInternetTVNRARC").innerHTML          = document.getElementById('txtInternetCARRemNRAPE').value;
                document.getElementById("lblInternetNTBACC").innerHTML           = document.getElementById('txtInternetCARRemNTBACC').value;
                document.getElementById("lblInternetTVRefNo").innerHTML          = document.getElementById('lblInternetCARemCASANo').innerHTML;
                document.getElementById("lblInternetTVCoverBank").innerHTML      = document.getElementById('lblInternetCARemPNBBranch').innerHTML;
                
                break;
            }
            default: break;
          }

//        var ds = this.txn_save.get_column_description().value;
//        if (ds != null)
//        {        
//            document.getElementById("lblInternetTVTranSource").innerHTML     = "";
//            document.getElementById("lblInternetTVSourceFund").innerHTML     = "";
//            document.getElementById("lblInternetTVTranStatus").innerHTML     = "";
//            document.getElementById("lblInternetTVPurpose").innerHTML        = "";
//            document.getElementById("lblInternetTVReceiptMode").innerHTML    = "";
//            document.getElementById("lblInternetTVRemName").innerHTML        = remname;
//            document.getElementById("lblInternetTVRemAddress").innerHTML     = remaddress;
//            document.getElementById("lblInternetTVBeneName").innerHTML       = benename;
//            document.getElementById("lblInternetTVBeneAddress").innerHTML    = beneaddress;
//            document.getElementById("lblInternetTVChargeType").innerHTML     = "";
//            document.getElementById("lblInternetTVProductType").innerHTML    = protype;
//            document.getElementById("lblInternetTVPayMode").innerHTML        = "";
//            document.getElementById("lblInternetTVImplementation").innerHTML = "";
//            document.getElementById("lblInternetTVRemitCurrency").innerHTML  = currency;
//            document.getElementById("lblInternetTVTranDate").innerHTML       = datenow;
//            document.getElementById("lblInternetTVDateToday").innerHTML      = datenow;
//            document.getElementById("lblInternetTVTARC").innerHTML           = totalamt;
//            document.getElementById("lblInternetNRAPE").innerHTML            = netremamt;
//            document.getElementById("lblInternetTVNRARC").innerHTML          = netremconv;
//            document.getElementById("lblInternetNTBACC").innerHTML           = nettobene;
//            document.getElementById("lblInternetTVRefNo").innerHTML          = accno;
//            
//            document.getElementById("lblInternetTVCoverBank").innerHTML      = branch;
            /*document.getElementById("lblInternetTVLocalCurrency").innerHTML  = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVEuroToUSD").innerHTML      = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVUSDToEuro").innerHTML      = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVTARC").innerHTML           = ds.Tables[0].Rows[0];
            document.getElementById("lblNRAPE").innerHTML            = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVNRARC").innerHTML          = ds.Tables[0].Rows[0];
            document.getElementById("lblNTBACC").innerHTML           = ds.Tables[0].Rows[0];
            document.getElementById("lblInternetTVCommStatus").innerHTML = ds.Tables[0].Rows[0];
            
            */
//        }
    }
    
    this.post_transaction = function()
    {
        var i_save = confirm('DO YOU WANT TO SAVE THIS TRANSACTION ? \n\n CLICK CANCEL TO ABORT SAVING AND RETURN TO TRANSACTION MODULE. ');

        if (this.txn_save != null)
        {
            if (i_save == true)
            {
                this.txn_save.commit_record();
                this.load();
            }
            else
            {
                this.rapid();
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
        document.getElementById("lblInternetNRAPE").innerHTML            = '';
        document.getElementById("lblInternetTVNRARC").innerHTML          = '';
        document.getElementById("lblInternetNTBACC").innerHTML           = '';
        document.getElementById("lblInternetTVCommStatus").innerHTML     = '';
        document.getElementById("lblInternetTVDateToday").innerHTML      = '';

    }
    
    this.post = function()
    {
        var i_save = confirm('DO YOU WANT TO SAVE THIS TRANSACTION ? \n\n CLICK CANCEL TO ABORT SAVING AND RETURN TO TRANSACTION MODULE. ');

            if (i_save == true)
            {
                this.commit_record();
                this.load();
            }
            else
            {
                switch(document.getElementById("lblInternetTVProductType").innerHTML)
                {
                    case "Rapid Remit":
                    {
                    this.rapid();
                    break;
                    }
                    case "Credit to Account":
                    {
                    this.credit();
                    break;
                    }
                    case "GFMC":
                    {
                    this.gfmc();
                    break;
                    }
                    default: break;
                }
            }
    }
    
    this.set_internet_remitter = function()
    {   
        this.UserID                 = null;
        this.RemitMethod            = null;
        this.RemitStationID         = null;
        this.RemittingBranch        = document.getElementById("lblInternetTVCoverBank").innerHTML;
        this.ReferenceNo            = null;
        this.ExtlReferenceNo        = null;
        this.Transtatus             = '1';
        this.TranxDate              = document.getElementById("lblInternetTVTranDate").innerHTML;
        this.ProductChargesID       = null;
        this.RemitterCIFID          = document.getElementById("txtHiddenRemCIFID").value;
        this.BeneficiaryCIFID       = document.getElementById("txtHiddenBeneCIFID").value;
        this.AccountSEQID           = null;
        this.RemittingCurrency      = null;
        this.GrossRemAmount         = document.getElementById("txtInternetRRRemTARC").value;
        this.NetRemAmount           = document.getElementById("txtInternetRRRemNRARC").value;
        this.NettoBeneCurrency      = null;
        this.NettoBeneAmount        = document.getElementById("txtInternetRRRemNTBACC").value;
        this.TotalLocalCharge       = null;
        this.TransactionSource      = null;
        this.ReceiptMode            = null;
        this.TransactionPurpose     = null;
        this.OtherPurpose           = null;
        this.SpecialMessage         = null;
        this.DateImplemented        = null;
        this.DateSent               = null;
        this.printed                = null;
        this.ForexSourceToTarget    = null;
        this.ForexTargetToUSD       = null;
        this.ForexTargetToPHP       = null;
        this.ForexSourceToUSD       = null;
        this.ForexSourceToPHP       = null;
        this.ForexTargetToAccount   = null;
        this.SourceofFunds          = null;
        
        return this.push_data_to_server();
     }
     
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
             
        data_row[data_row.length] = "UserID~"                   + this.UserID
        data_row[data_row.length] = "RemitMethod~"              + this.RemitMethod
        data_row[data_row.length] = "RemitStationID~"           + this.RemitStationID
        data_row[data_row.length] = "RemittingBranch~"          + this.RemittingBranch
        data_row[data_row.length] = "ReferenceNo~"              + this.ReferenceNo
        data_row[data_row.length] = "ExtlReferenceNo~"          + this.ExtlReferenceNo
        data_row[data_row.length] = "Transtatus~"               + this.Transtatus
        data_row[data_row.length] = "TranxDate~"                + this.TranxDate
        data_row[data_row.length] = "ProductChargesID~"         + this.ProductChargesID
        data_row[data_row.length] = "RemitterCIFID~"            + this.RemitterCIFID
        data_row[data_row.length] = "BeneficiaryCIFID~"         + this.BeneficiaryCIFID
        data_row[data_row.length] = "AccountSEQID~"             + this.AccountSEQID
        data_row[data_row.length] = "RemittingCurrency~"        + this.RemittingCurrency
        data_row[data_row.length] = "GrossRemAmount~"           + this.GrossRemAmount
        data_row[data_row.length] = "NetRemAmount~"             + this.NetRemAmount
        data_row[data_row.length] = "NettoBeneCurrency~"        + this.NettoBeneCurrency
        data_row[data_row.length] = "NettoBeneAmount~"          + this.NettoBeneAmount
        data_row[data_row.length] = "TotalLocalCharge~"         + this.TotalLocalCharge
        data_row[data_row.length] = "TransactionSource~"        + this.TransactionSource
        data_row[data_row.length] = "ReceiptMode~"              + this.ReceiptMode
        data_row[data_row.length] = "TransactionPurpose~"       + this.TransactionPurpose
        data_row[data_row.length] = "OtherPurpose~"             + this.OtherPurpose
        data_row[data_row.length] = "SpecialMessage~"           + this.SpecialMessage
        data_row[data_row.length] = "DateSent~"                 + this.DateSent
        data_row[data_row.length] = "printed~"                  + this.printed   
        data_row[data_row.length] = "ForexSourceToTarget~"      + this.ForexSourceToTarget                  
        data_row[data_row.length] = "ForexTargetToUSD~"         + this.ForexTargetToUSD
        data_row[data_row.length] = "ForexTargetToPHP~"         + this.ForexTargetToPHP
        data_row[data_row.length] = "ForexSourceToUSD~"         + this.ForexSourceToUSD
        data_row[data_row.length] = "ForexSourceToPHP~"         + this.ForexSourceToPHP   
        data_row[data_row.length] = "ForexTargetToAccount~"     + this.ForexTargetToAccount                  
        data_row[data_row.length] = "SourceofFunds~"            + this.SourceofFunds     
        
        return data_row.join("|");       
    }
     
     
     this.insert_record = function()
    {   
        InternetTransaction.insert_record(this.insert_record_callback);
    }  
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = InternetTransaction.read_message(); alert(msg.value); return;
            }
             
        }
    }   
    
    this.commit_record = function()
    {
         this.insert_record();
       
//       if (typeof(this.validate_input) != 'undefined')
//            {
//                if (this.validate_input() == false)
//                {
//                   alert(this.error_input); return;
//                }                            
//            }       
            InternetTransaction.commit_record(this.set_internet_remitter(),this.commit_record_callback);

    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = InternetTransaction.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = InternetTransaction.INTERNET_TRANSACTION_SEQ_ID().value;
            
            alert("TRANSACTION COMMITTED SUCCESSFULLY");       
        }
    }
}
