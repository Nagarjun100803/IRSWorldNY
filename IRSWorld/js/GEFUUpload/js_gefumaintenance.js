function execute_ajaxgridview_command(param)
{
    if (gefu_maintenance != null && gefu_maintenance != "undefined")
    {
        gefuMaintenance.do_ajaxgridviewer(param);
    }
}

function gefu_maintenance()
{
    this.commons    = null;
    this.jsFunction = null;
    this.jsName     = null;
    this.BranchCode = null;
    this.Role       = null;
    
    this.create_reports_control = function()
    {
        this.jsFunction.create_reports_control();
    }    
    
    this.check_gl_codes = function()
    {
        this.jsFunction.check_gl_codes();
    }    
    
    this.check_alpha = function()
    {
        this.jsFunction.check_alpha();
    }
    
    this.check_booking_rates = function ()
    {
        this.jsFunction.check_booking_rates();
    }
    
    this.do_ajaxgridviewer = function (parameter)
    {
        this.jsFunction.do_ajaxgridviewer(parameter)
    }
    
    this.create_report = function()
    {
        var tranxsource;
        var intCounter;
        
        tranxsource = "";
        
        if (this.Role.value == "1")
        {
            if(document.getElementById("ddReportName").options[document.getElementById("ddReportName").selectedIndex].text != "Accounting Entries Combination")
            {
                alert ("You are only allowed to process Accounting Entries Combination Report");
                return;
            }
            else
            {
                this.jsFunction.create_report(document.getElementById("ddReportName").options[document.getElementById("ddReportName").selectedIndex].text, document.getElementById("txtTranxdate").value, this.BranchCode.value, document.getElementById("ddTranxStatus").value, tranxsource);
            }
        }
        else
        {
            if (trim(document.getElementById("txtTranxdate").value) == "")
            {
                alert ("Select Transaction Date");
                return;
            }
            
            intCounter = 0;
            tranxsource = "'"
            while (document.getElementById("ddTransactionSource").selectedIndex != -1)
            {
                if (intCounter > 0)
                {
                    tranxsource = tranxsource + ",";
                }
                            
                if (document.getElementById("ddTransactionSource").selectedIndex != 0) 
                tranxsource = tranxsource + document.getElementById("ddTransactionSource").options[document.getElementById("ddTransactionSource").selectedIndex].value                
                document.getElementById("ddTransactionSource").options[document.getElementById("ddTransactionSource").selectedIndex].selected = false;
                intCounter += 1;
            }
            tranxsource = tranxsource + "'";
            
            var is_Report_Created;
        
            //is_Report_Created = this.jsFunction.check_is_report_created(this.BranchCode.value, document.getElementById("txtTranxdate").value, document.getElementById("ddTranxStatus").value, document.getElementById("ddReportName").options[document.getElementById("ddReportName").selectedIndex].text);
            
            this.jsFunction.create_report(document.getElementById("ddReportName").options[document.getElementById("ddReportName").selectedIndex].text, document.getElementById("txtTranxdate").value, this.BranchCode.value, document.getElementById("ddTranxStatus").value, tranxsource);
            document.getElementById("btnProcess").disabled = false;   
            
            //if (is_Report_Created.value.Rows.length > 0)
            //{
            //    this.jsFunction.search_specific_reports(this.BranchCode.value, is_Report_Created.value.Rows[0].filename );    
            //    alert ("Existing report found \n" + "Download report below");                
            //    document.getElementById("btnProcess").disabled = true;
            //}
            //else
            //{
            //    this.jsFunction.create_report(document.getElementById("ddReportName").options[document.getElementById("ddReportName").selectedIndex].text, document.getElementById("txtTranxdate").value, this.BranchCode.value, document.getElementById("ddTranxStatus").value, tranxsource);
            //    document.getElementById("btnProcess").disabled = false;                
            //}
        }
    }
    
    this.create_bookingrate = function()
    {
        this.jsFunction.create_bookingrate(document.getElementById("txtTranxdate").value, this.BranchCode.value);
    }
    
    this.get_locationcode_by_negbranchcode = function()
    {
        this.jsFunction.get_locationcode_by_negbranchcode(document.getElementById("ddnegbranchcode").value);
    }
    
    this.get_branch_sub_lookups = function()
    {
        this.jsFunction.get_branch_sub_lookups(document.getElementById("ddlocationcode").value);
    }
    this.insert_record = function()
    {   
        this.jsFunction.insert_record();
    }
    
    this.update_record = function()
    {
        this.jsFunction.update_record();        
    }
    
    this.commit_record = function()
    {
        this.jsFunction.commit_record();
    }    
    
    this.rollback_record = function()
    {
        this.jsFunction.rollback_record();
    }
   
    this.delete_record = function()
    {
        this.jsFunction.delete_record();
    }
    
    this.delete_bookingraterecord = function()
    {
        this.jsFunction.delete_bookingraterecord();
    }
    
    this.enable_process_button = function()
    {
        document.getElementById("btnProcess").disabled                      = false;
    }
    
    this.check_report_name = function (reportname)
    {
        document.getElementById("divGEFUBookingRates").style.display        = "none";
        document.getElementById("divGEFUBookingRatesView").style.display    = "none";
        document.getElementById("ddTransactionSource").disabled             = true; 
        document.getElementById("ddTranxStatus").disabled                   = false;       
        document.getElementById("ddTranxStatus").selectedIndex              = -1;
        document.getElementById("btnProcess").disabled                      = false;  
        
        if (reportname == "8" || reportname == "10")
        {        
            document.getElementById("ddTranxStatus").disabled           = true;
        }
        else if (reportname == "9")
        {
            document.getElementById("ddTransactionSource").disabled     = false;
            document.getElementById("ddTranxStatus").disabled           = true;        
        }
        else if (reportname == "11")
        {
            document.getElementById("divGEFUBookingRatesView").style.display    = "block";
            document.getElementById("ddTranxStatus").disabled                   = true;
            this.show_bookingrates();
        }
        else if (reportname == "5" || reportname == "7" || reportname == "10" || reportname == "12")
        {        
            document.getElementById("ddTranxStatus").disabled           = true;
        }
    }
    
    this.show_bookingrates = function ()
    {
        this.jsFunction.tranxdate = document.getElementById("txtTranxdate").value;
        this.jsFunction.load_bookingrates_grid_viewer();
    }
    
    this.load_bookingrates_grid_viewer = function (effectivitydate)
    {    
        this.jsFunction.BranchCode = this.BranchCode;
        this.jsFunction.tranxdate = effectivitydate;
        this.jsFunction.load_bookingrates_grid_viewer();
    }
    
    this.check_data = function(reportname)
    {
        if (reportname == "11")
        {
            this.jsFunction.check_data(document.getElementById("txtTranxdate").value);
        }
    }
    
    this.show_div = function(divName)    
    {
        var div = new Array("div_AccountingEntries","div_IflexAccounts","div_BookingRates","div_GEFU_Reports","divIncAccnt", "divReportsDownload", "divIncBookingRAtes","divGEFUBookingRatesView"); // condensed array
        var divgrid = new Array("divtbl_AccountingEntries", "divtbl_iflexaccounts", "divtbl_bookingrates");// condensed array
        var divTable = new Array("divtbl_AccountingEntries");
        var y = 0;

        this.commons = new common_method();
        this.commons.to_clear_screen("tblAccountingEntries");
        this.commons.to_clear_screen("tblIflexAccounts");
        this.commons.to_clear_screen("tblBookingRates");
        this.commons.to_clear_screen("tblGEFU");
        this.commons.to_clear_screen("tblTranx_Status_Date");

        this.commons.to_read_only("tblAccountingEntries",true);
        this.commons.to_unlock_button("tblAccountingEntriesButtons","btnAddAccountingEntries~btnEditAccountingEntries");
     
        this.commons.to_read_only("tblIFlexAccountsButtons",true);
        this.commons.to_unlock_button("tblIFlexAccountsButtons","btnEditIflexAccounts");

        this.commons = null;    
            
        
        for(y = 0; y < div.length; y++)
        {
            if (divName == div[y])
            {
                if (y != 0)
                {
                    document.getElementById(div[y]).style.display       = "block";
                }                

                if (y <3)
                {
                    document.getElementById(divgrid[y]).style.display   = "block"
                }
                this.div_js_function(divName);                
            }
            else
            {
                document.getElementById(div[y]).style.display       = "none";
                if (y<3)
                {
                    document.getElementById(divgrid[y]).style.display = "none";
                }
            }
        }        
    }
    
    this.div_js_function = function (divName)
    {            
        this.jsFunction = null
        switch(divName)
        {
            case "div_AccountingEntries":
            {
                var is_negbranchcode_exist;
                var is_fc_branch_code;
                var is_tranxsource_exist;
                var is_receiptmode_exist;
                
                this.jsFunction = new accountingentries();
                this.jsFunction.ActiveModule(divName);
                this.BranchCode = this.jsFunction.getBranchCode();                
                this.Role       = this.jsFunction.getRole();                
                this.jsFunction.BranchCode = this.BranchCode;          
                this.jsFunction.Role = this.Role;
                
//                alert(this.Role.value);
                is_negbranchcode_exist = this.jsFunction.check_negbranchcode();
                
                if (this.BranchCode.value != '1074')
                {
                    if (is_negbranchcode_exist.value == 0)
                    {
                        alert("Kindly setup the negotiating branch code for " + this.BranchCode.value);
                        break;                 
                    }          

                    is_fc_branch_code = this.jsFunction.check_fc_branch_code();
                    if (is_fc_branch_code.value == 0)
                    {
                        alert("Kindly setup the fc branch code for " + this.BranchCode.value);
                        break;                 
                    }         
                    
                    is_tranxsource_exist = this.jsFunction.check_tranxsource_sublookup();
                    if (is_tranxsource_exist.value == 0)
                    {
                        alert("Kindly setup lookup for Transaction Source of " + this.BranchCode.value + " branch");
                        break;
                    }

                    is_receiptmode_exist = this.jsFunction.check_receiptmode_sublookup();
                    if (is_receiptmode_exist.value == 0)
                    {
                        alert("Kindly setup the Receipt Mode to be used of " + this.BranchCode.value + " branch");
                        break;
                    }
                }
                
                document.getElementById("div_AccountingEntries").style.display = "block";
                document.getElementById("btnEditAccountingEntries").disabled = "disable";
               
                    document.getElementById("btnAddAccountingEntries").disabled = "";
        
                
                this.jsFunction.populate_dropdownboxes();             
                this.jsFunction.load_accountingentries_grid_viewer();
                break;
            }
            case "div_IflexAccounts":
            {
                this.jsFunction = new iflexaccounts();
                this.jsFunction.ActiveModule(divName);
                this.jsFunction.BranchCode = this.BranchCode;
                this.Role       = this.jsFunction.getRole();
                this.jsFunction.Role = this.Role;
                
                if (this.Role.value != "1")
                {
                    document.getElementById("btnEditIflexAccounts").disabled = "disable";
                }
                else
                {
                    document.getElementById("btnEditIflexAccounts").disabled = "";
                }
                this.jsFunction.load_iflexaccounts_grid_viewer();
                break;
            }      
            case "div_BookingRates":
            {
                if (this.Role.value == "1")
                {
                    alert ("You are not allowed to access Booking Rates")
                    document.getElementById("div_BookingRates").style.display = "none";
                    break;
                }
                
                this.jsFunction = new bookingrates();
                this.jsFunction.ActiveModule(divName);      
                this.jsFunction.BranchCode = this.BranchCode;
                this.jsFunction.set_default_date();
                this.jsFunction.tranxdate = document.getElementById("txtEffectivityDate").value;
                this.jsFunction.populate_dropdownboxes();
                this.jsFunction.load_bookingrates_grid_viewer();
                break;
            }                
            case "div_GEFU_Reports":
            {
                var is_fc_branch_code;
                
                this.jsFunction = new GEFU_Reports();
                this.jsFunction.ActiveModule(divName);
                this.jsFunction.BranchCode = this.BranchCode;
                this.jsFunction.set_default_date();
                this.jsFunction.tranxdate = document.getElementById("txtTranxdate").value                
                
                is_fc_branch_code = this.jsFunction.check_fc_branch_code();
                if (is_fc_branch_code.value == 0)
                {
                    alert("Kindly setup the fc branch code for " + this.BranchCode.value);
                    document.getElementById("div_GEFU_Reports").style.display = "none";
                    break;                 
                }
                
                if (this.Role.value == "1")
                {
                    alert ("You are only allowed to Search, Download and Process Accounting Entries Report")
                    break;
                }
                
                this.jsFunction.populate_dropdownboxes();
                break;
            }          
            default: break;
        }            
    }
    
    this.searchreports = function ()
    {
        this.jsFunction.searchreports();
    }
    
    this.get_bookingrates = function(divisor)
    {
        var bookingrates;
        if (divisor == "")
        {
            document.getElementById("txtTargetToSourceRate").value = "";
        }
        else
        {
            bookingrates = this.jsFunction.compute_bookingrates(divisor);
            document.getElementById("txtTargetToSourceRate").value = bookingrates;
        }        
    }
    
    this.check_booking_rates = function()
    {
        this.jsFunction.check_booking_rates();
    }
    
    this.check_currencies = function (source, target)
    {
        this.jsFunction.check_currencies(source, target);
    }
    
    this.checksession = function ()
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.checkbranchcode();
    }
}