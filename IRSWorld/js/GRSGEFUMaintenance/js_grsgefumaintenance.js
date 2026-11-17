function execute_ajaxgridview_command(param)
{
    if (GRSGEFUMaintenance != null && GRSGEFUMaintenance != "undefined")
    {
        grsGEFUMaintenance.do_ajaxgridviewer(param);
    }
}

function GRSGEFUMaintenance()
{
    this.create_grs_gefu = function()
    {
        this.jsFunction.create_grs_gefu(document.getElementById("txtTranxdate").value);
    }

    this.div_js_function = function (divName)
    {            
        this.jsFunction = null
        switch(divName)
        {
            case "div_GRS_AccountingEntries":
            {
                this.jsFunction = new GRSAccountingEntries();
                this.jsFunction.ActiveModule(divName);
                this.jsFunction.load_grs_accountingentries_grid_viewer();
                this.jsFunction.populate_dropdownboxes();
                document.getElementById("btnEditGRSAccountingEntries").disabled = "disabled";
                break;
            }
            case "div_GRS_CorrBank":
            {
                this.jsFunction = new GRSCoreBank();
                this.jsFunction.ActiveModule(divName);                
                this.jsFunction.load_grs_CorrBank_grid_viewer();
                this.jsFunction.populate_dropdownboxes();
                document.getElementById("btnEditGRSCorrBank").disabled = "disabled";
                break;
            }
            case "div_GRS_BuyingRates":
            {
                this.jsFunction = new GRSBuyingRate();
                this.jsFunction.ActiveModule(divName);
                this.jsFunction.set_default_date();
                this.jsFunction.tranxdate = document.getElementById("txtGRSEffectivityDate").value;
                this.jsFunction.load_buyingrates_grid_viewer();
                this.jsFunction.populate_dropdownboxes();
                break;
            }
            default: break;
        }            
    }
    
    this.show_required_fields = function()
    {
        this.jsFunction.show_required_fields(document.getElementById("ddHandlingType").value);
    }
    
    this.check_currency_combination = function()
    {
        this.jsFunction.check_currency_combination();
    }
    
    this.do_ajaxgridviewer = function (parameter)
    {
        this.jsFunction.do_ajaxgridviewer(parameter)
    }
    
    this.insert_record = function()
    {   
        this.jsFunction.insert_record();
    }
    
    this.check_alpha = function ()
    {
        this.jsFunction.check_alpha();    
    }
    this.check_gl_codes = function()
    {   
        this.jsFunction.check_gl_codes();
    }
    
    this.check_ck_account = function()
    {   
        this.jsFunction.check_ck_account();
    }
    
    this.update_record = function()
    {
        this.jsFunction.update_record();        
    }
    
    this.delete_record = function()
    {
        this.jsFunction.delete_record();
    }
    
    this.commit_record = function()
    {
        this.jsFunction.commit_record();
    }    
    
    this.rollback_record = function()
    {
        this.jsFunction.rollback_record();
    }
    //GET_GRS_ACCOUNTINGENTRIES_RECORD
    this.show_div = function(divName)    
    {
        var div = new Array("div_GRS_AccountingEntries","div_GRS_CorrBank", "div_GRS_BuyingRates"); // condensed array
        var divgrid = new Array("divtbl_grs_accountingentries", "divtbl_GRS_CorrBank", "divtbl_buyingrates");// condensed array
        var y = 0;      
        
        this.commons = new common_method();
        this.commons.to_clear_screen("tblGRSAccountingEntries");  
        this.commons.to_clear_screen("tblGRSCorrBank");  
        
        this.commons.to_read_only("tblGRSAccountingEntriesButtons",true);
        this.commons.to_unlock_button("tblGRSAccountingEntriesButtons","btnAddGRSAccountingEntries~btnEditGRSAccountingEntries");
        
        this.commons.to_read_only("tblGRSCorrBankButtons",true);
        this.commons.to_unlock_button("tblGRSCorrBankButtons","btnAddGRSCorrBank~btnEditGRSCorrBank");
        
        this.commons = null;
        
        for(y = 0; y < div.length; y++)
        {
            if (divName == div[y])
            {
                document.getElementById(div[y]).style.display       = "block";
                document.getElementById(divgrid[y]).style.display   = "block"
                
                this.div_js_function(divName);
            }
            else
            {
                document.getElementById(div[y]).style.display       = "none";
                document.getElementById(divgrid[y]).style.display   = "none"
            }
        }
    } 
    
    this.check_currencies = function (source, target)
    {
        this.jsFunction.check_currencies(source, target);
    }
    
    this.check_booking_rates = function()
    {
        this.jsFunction.check_booking_rates();
    }
    
    this.get_bookingrates = function(divisor)
    {
        var bookingrates;
        if (divisor == "")
        {
            document.getElementById("txtGRSTargetToSourceRate").value = "";
        }
        else
        {
            bookingrates = this.jsFunction.compute_bookingrates(divisor);
            document.getElementById("txtGRSTargetToSourceRate").value = bookingrates;
        }        
    }
    
    this.load_buyingrates_grid_viewer = function (effectivitydate)
    {    
        this.jsFunction.tranxdate = effectivitydate;
        this.jsFunction.load_buyingrates_grid_viewer();
    }
}