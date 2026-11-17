function GRSBuyingRate()
{
    var SEQ_ID              = null;
    var Effectivitydate     = null;
    var SourceCcy           = null;
    var TargetCcy           = null;
    var SourceToTargetRate  = null;
    var TargetToSourceRate  = null;
    var DateTimeInserted    = null;
    var UserID              = null;
        
    this.tranxdate          = null;

    this.ActiveModule = function (divName)
    {
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.ActiveModule(divName);
    }
    
    this.load_buyingrates_grid_viewer = function()
    {        
        this.buyingrates_grid = new grid_viewer();
        this.buyingrates_grid.sql_source  = "sp_grs_buyingrates_grid~" + this.tranxdate;
        this.buyingrates_grid.div_container = document.getElementById("divtbl_buyingrates");
        this.buyingrates_grid.datarow_column = 0;
        this.buyingrates_grid.open_grid_view();
        this.buyingrates_grid = null;
    }
    
    this.check_booking_rates = function()
    {
        if ((event.keyCode >= 48 && event.keyCode <= 57) || event.keyCode == 46) 
        {
            event.returnValue = true;
        }
        else
        {
            event.returnValue=false;
            alert("Enter only Numbers");
        }
    }
    
    this.set_default_date = function ()
    {
        document.getElementById("txtGRSEffectivityDate").value = get_current_date_string().substring(0, 10);
    }

    this.check_currencies = function(source, target)
    {        
        if (document.getElementById("ddGRSSourceCcy").value != "null" && document.getElementById("ddGRSTargetCcy").value != "null" )
        {
            document.getElementById("tdGRSSourceToTargetRate").innerText = "1 " + trim(document.getElementById("ddGRSSourceCcy").options[document.getElementById("ddGRSSourceCcy").selectedIndex].text) + " to " + trim(document.getElementById("ddGRSTargetCcy").options[document.getElementById("ddGRSTargetCcy").selectedIndex].text) + ":";
            document.getElementById("tdGRSTargetToSourceRate").innerText = "1 " + trim(document.getElementById("ddGRSTargetCcy").options[document.getElementById("ddGRSTargetCcy").selectedIndex].text) + " to " + trim(document.getElementById("ddGRSSourceCcy").options[document.getElementById("ddGRSSourceCcy").selectedIndex].text) + ":";      
        }
        else if (document.getElementById("ddGRSSourceCcy").value != "null" && document.getElementById("ddGRSTargetCcy").value == "null" )
        {
            document.getElementById("tdGRSSourceToTargetRate").innerText = "1 " + trim(document.getElementById("ddGRSSourceCcy").options[document.getElementById("ddGRSSourceCcy").selectedIndex].text) + " to Ccy:";
            document.getElementById("tdGRSTargetToSourceRate").innerText = "1 Ccy to " + trim(document.getElementById("ddGRSSourceCcy").options[document.getElementById("ddGRSSourceCcy").selectedIndex].text) + ":";
        }
        else if (document.getElementById("ddGRSSourceCcy").value == "null" && document.getElementById("ddGRSTargetCcy").value != "null" )
        {
            document.getElementById("tdGRSSourceToTargetRate").innerText = "1 Ccy to " + trim(document.getElementById("ddGRSTargetCcy").options[document.getElementById("ddGRSTargetCcy").selectedIndex].text) + ":";
            document.getElementById("tdGRSTargetToSourceRate").innerText = "1 " + trim(document.getElementById("ddGRSTargetCcy").options[document.getElementById("ddGRSTargetCcy").selectedIndex].text) + " to Ccy:";
        }
        else if (document.getElementById("ddGRSSourceCcy").value == "null" && document.getElementById("ddGRSTargetCcy").value == "null" )
        {
            document.getElementById("tdGRSSourceToTargetRate").innerText = "1 Ccy to Ccy:";
            document.getElementById("tdGRSTargetToSourceRate").innerText = "1 Ccy to Ccy:";
        }
        
        if (source == target)
        {
            document.getElementById("txtGRSSourceToTargetRate").value = 1;
            document.getElementById("txtGRSTargetToSourceRate").value = 1;
            
            document.getElementById("txtGRSSourceToTargetRate").disabled = "disabled"; 
            document.getElementById("txtGRSTargetToSourceRate").disabled = "disabled";     
        }        
        else
        {   
            document.getElementById("txtGRSSourceToTargetRate").value = "";
            document.getElementById("txtGRSTargetToSourceRate").value = "";
            
            document.getElementById("txtGRSSourceToTargetRate").disabled = ""; 
            document.getElementById("txtGRSTargetToSourceRate").disabled = "disabled";  
        }
    }
    
    this.populate_dropdownboxes = function ()
    {
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getSourceCcy(this.dropdown_SourceCcy_callback);
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getTargetCcy(this.dropdown_TargetCcy_callback);        
    }
    
    this.dropdown_SourceCcy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddGRSSourceCcy", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_TargetCcy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddGRSTargetCcy", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.insert_record = function()
    {        
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.insert_record(this.insert_record_callback);    
    }
    
    this.insert_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message(); alert(msg.value); return;
            }            
                        
            this.tranxdate = document.getElementById("txtGRSEffectivityDate").value;
            
            document.getElementById("tdGRSEffectivityDate").innerText = "Add by Effectivity Date :";
            
            this.commons = new common_method();                    
            this.commons.to_clear_screen("tblGRSBuyingRates");  
            this.commons.to_read_only("tblGRSBuyingRates",false);
            this.commons.to_unlock_button("tblGRSBuyingRatesButtons","btnGRSSaveBuyingRates~btnGRSCancelBuyingRates");
            this.commons = null;  
            
            document.getElementById("txtGRSEffectivityDate").value = this.tranxdate; 
        }        
    }  
    
    
    this.commit_record = function ()
    {
        if (trim(document.getElementById("ddGRSSourceCcy").value) == "null")
        {
            alert ("Source currency is required");
            document.getElementById( "ddGRSSourceCcy").focus();
            return;
        }
        
        if (trim(document.getElementById("ddGRSTargetCcy").value) == "null")
        {
            alert ("Target currency is required");
            document.getElementById( "ddGRSTargetCcy").focus();
            return;
        }

        if (trim(document.getElementById("txtGRSSourceToTargetRate").value) == "")
        {
            alert ("Booking Rate is required");
            document.getElementById( "txtGRSSourceToTargetRate").focus();
            return;
        }
        
        if (trim(document.getElementById("txtGRSEffectivityDate").value) == "")
        {
            alert ("Effectivity Date is required");
            return;
        }
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.commit_record(this.set_BookingRates(), this.commit_record_callback);        
    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message();
                alert(msg.value);
                return;
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");            
            
            this.tranxdate = document.getElementById("txtGRSEffectivityDate").value;
            
            this.commons = new common_method();     
            this.commons.to_clear_screen("tblGRSBuyingRates");   
            this.commons.to_read_only("tblGRSBuyingRates",true);
            this.commons.to_unlock_button("tblGRSBuyingRatesButtons","btnGRSAddBuyingRates~");
            this.commons = null;  
        
            document.getElementById("txtGRSEffectivityDate").disabled = false;
            
            document.getElementById("txtGRSEffectivityDate").value = this.tranxdate;
        
            document.getElementById("tdGRSEffectivityDate").innerText = "View by Effectivity Date : ";
            
            this.accountingentries_grid = new grid_viewer();
            this.accountingentries_grid.sql_source  = "sp_grs_buyingrates_grid~" + this.tranxdate;
            this.accountingentries_grid.div_container = document.getElementById("divtbl_buyingrates");
            this.accountingentries_grid.datarow_column = 0;
            this.accountingentries_grid.open_grid_view();
            this.accountingentries_grid = null;
        }
    } 
    
    this.set_BookingRates = function ()
    {  
        
        Effectivitydate     = document.getElementById("txtGRSEffectivityDate").value; //get_current_date_string();//               = document.getElementById("Effectivitydate").value;
        SourceCcy           = document.getElementById("ddGRSSourceCcy").value;
        TargetCcy           = document.getElementById("ddGRSTargetCcy").value;
        SourceToTargetRate  = document.getElementById("txtGRSSourceToTargetRate").value;
        TargetToSourceRate  = document.getElementById("txtGRSTargetToSourceRate").value;
        DateTimeInserted    = get_current_date_string();//             = document.getElementById("DateInserted").value;        
        UserID              ="UUUIII";
        return this.push_data_to_server();
    }
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]   = "Effectivitydate~"    + Effectivitydate;
        data_row[data_row.length]   = "SourceCcy~"          + SourceCcy;
        data_row[data_row.length]   = "TargetCcy~"          + TargetCcy;
        data_row[data_row.length]   = "SourceToTargetRate~" + SourceToTargetRate;
        data_row[data_row.length]   = "TargetToSourceRate~" + TargetToSourceRate;
        data_row[data_row.length]   = "DateTimeInserted~"   + DateTimeInserted;
        data_row[data_row.length]   = "UserID~"             + UserID;
        
        return data_row.join("$");
    }     
    
    this.rollback_record = function()
    {        
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message();

        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }
        
        this.tranxdate = document.getElementById("txtGRSEffectivityDate").value;
        
        this.commons = new common_method();        
        this.commons.to_read_only("tblGRSBuyingRates",true);
        this.commons.to_clear_screen("tblGRSBuyingRates");
        this.commons.to_unlock_button("tblGRSBuyingRatesButtons","btnGRSAddBuyingRates~");
        this.commons = null;
        
        document.getElementById("tdGRSEffectivityDate").innerText = "View by Effectivity Date :";
        document.getElementById("tdGRSSourceToTargetRate").innerText = "1 Ccy to Ccy:";
        document.getElementById("tdGRSTargetToSourceRate").innerText = "1 Ccy to Ccy:";
        
        document.getElementById("txtGRSEffectivityDate").value = this.tranxdate;
        return;
    }
    
    this.compute_bookingrates = function(divisor)
    {
        var quotient;

        quotient = 1/divisor;        
        return formatNumber(quotient, 9);
    }
    
     this.do_ajaxgridviewer = function (parameter)
    {
        this.tranxdate = document.getElementById("txtGRSEffectivityDate").value;
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                document.getElementById("txtHidden_buyingrates_SEQ_ID").value = command_param[1];

                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    if (document.getElementById("btnGRSAddBuyingRates").disabled == true)
                    {
                        this.rollback_record();
                        this.load_record();
                    }
                    //this.rollback_record();
                    this.load_record();
                    return;
                }
                this.process_grid_viewer(parameter);
                
                //this.load_bookingrates_grid_viewer();
            }
        }
    }   

    this.load_record = function()
    {
        this.pull_server_data()
    }
    
    this.pull_server_data = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.GEFUUpload.GEFUMaintenance.get_seq_id().value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == '' || this.SEQ_ID =='undefined')
        {
            return;
        }
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.GET_BUYINGRATES_RECORD(this.SEQ_ID, this.pull_server_data_callback);
    }    
    
    this.pull_server_data_callback = function(result)
    {
        if (result!= null && result.value != null)
        {
            var ds = result.value;
            
            if (ds.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                this.BranchCode         = ds.Tables[0].Rows[0].BranchCode;
                this.Effectivitydate    = ds.Tables[0].Rows[0].Effectivitydate;
                this.SourceCcy          = ds.Tables[0].Rows[0].SourceCcy;
                this.TargetCcy          = ds.Tables[0].Rows[0].TargetCcy;
                this.SourceToTargetRate = ds.Tables[0].Rows[0].SourceToTargetRate;
                this.TargetToSourceRate = ds.Tables[0].Rows[0].TargetToSourceRate;
                this.DateTimeInserted   = ds.Tables[0].Rows[0].DateInserted;
    
                document.getElementById("txtHidden_buyingrates_SEQ_ID").value  = this.SEQ_ID;
                //document.getElementById("BranchCode").value                   = this.IflexAccount;
                //document.getElementById("Effectivitydate").value              = this.Description;
                document.getElementById("ddGRSSourceCcy").value                 = this.SourceCcy;
                document.getElementById("ddGRSTargetCcy").value                 = this.TargetCcy;
                document.getElementById("txtGRSSourceToTargetRate").value       = this.SourceToTargetRate;
                document.getElementById("txtGRSTargetToSourceRate").value       = this.TargetToSourceRate;
                //document.getElementById("DateInserted").value                 = this.DateInserted;
            }
        }
    }    
    
    this.process_grid_viewer = function (parameter)
    {
        this.loadergrid = new grid_viewer();
        this.loadergrid.sql_source     = "sp_grs_buyingrates_grid~" + this.tranxdate;
        this.loadergrid.div_container  = document.getElementById("divtbl_buyingrates");
        this.loadergrid.datarow_column = 0;
        this.loadergrid.process_ajax(parameter);
        this.loadergrid = null;
    }   
}
