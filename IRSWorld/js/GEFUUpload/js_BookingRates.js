function bookingrates()
{
    var SEQ_ID              = null;
    var BranchCode          = null;
    var Effectivitydate     = null;
    var SourceCcy           = null;
    var TargetCcy           = null;
    var SourceToTargetRate  = null;
    var TargetToSourceRate  = null;
    var DateInserted        = null;
    var UserID              = null;
    this.BranchCode         = null;
    this.tranxdate          = null;

    this.ActiveModule = function(divName)
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.ActiveModule(divName);
    }

    this.load_bookingrates_grid_viewer = function()
    {        
        this.bookingrates_grid = new grid_viewer();
        this.bookingrates_grid.sql_source  = "sp_bookingrates_grid~" + this.BranchCode.value + "~" + this.tranxdate;
        this.bookingrates_grid.div_container = document.getElementById("divtbl_bookingrates");
        this.bookingrates_grid.datarow_column = 0;
        this.bookingrates_grid.open_grid_view();
        this.bookingrates_grid = null;
    }
    
    this.compute_bookingrates = function(divisor)
    {
        var quotient;

//        if (event.keyCode >= 48 && event.keyCode <= 57) 
//        {
//            quotient = 1/document.getElementById('txtSourceToTargetRate').value;        
//            return formatNumber(quotient, 9);
//            event.returnValue = true;
//        }
//        else
//        {
//            event.returnValue=false;
////            alert("Enter only Numbers");
//        }

            quotient = 1/divisor;        
            return formatNumber(quotient, 9);

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
    
    this.check_currencies = function(source, target)
    {        
        if (document.getElementById("ddSourceCcy").value != "null" && document.getElementById("ddTargetCcy").value != "null" )
        {
            //document.getElementById("tdSourceToTargetRate").innerText = "1 " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + " to " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + ":";
            //document.getElementById("tdTargetToSourceRate").innerText = "1 " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + " to " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + ":";      
            
            document.getElementById("tdSourceToTargetRate").innerHTML = "1 " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + " to " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + ":";
            document.getElementById("tdTargetToSourceRate").innerHTML = "1 " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + " to " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + ":";      
        }
        else if (document.getElementById("ddSourceCcy").value != "null" && document.getElementById("ddTargetCcy").value == "null" )
        {
            //document.getElementById("tdSourceToTargetRate").innerText = "1 " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + " to Ccy:";
            //document.getElementById("tdTargetToSourceRate").innerText = "1 Ccy to " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + ":";
            
            document.getElementById("tdSourceToTargetRate").innerHTML = "1 " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + " to Ccy:";
            document.getElementById("tdTargetToSourceRate").innerHTML = "1 Ccy to " + trim(document.getElementById("ddSourceCcy").options[document.getElementById("ddSourceCcy").selectedIndex].text) + ":";
        }
        else if (document.getElementById("ddSourceCcy").value == "null" && document.getElementById("ddTargetCcy").value != "null" )
        {
            //document.getElementById("tdSourceToTargetRate").innerText = "1 Ccy to " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + ":";
            //document.getElementById("tdTargetToSourceRate").innerText = "1 " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + " to Ccy:";
            
            document.getElementById("tdSourceToTargetRate").innerHTML = "1 Ccy to " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + ":";
            document.getElementById("tdTargetToSourceRate").innerHTML = "1 " + trim(document.getElementById("ddTargetCcy").options[document.getElementById("ddTargetCcy").selectedIndex].text) + " to Ccy:";
        }
        else if (document.getElementById("ddSourceCcy").value == "null" && document.getElementById("ddTargetCcy").value == "null" )
        {
            //document.getElementById("tdSourceToTargetRate").innerText = "1 Ccy to Ccy:";
            //document.getElementById("tdTargetToSourceRate").innerText = "1 Ccy to Ccy:";
            
            document.getElementById("tdSourceToTargetRate").innerHTML = "1 Ccy to Ccy:";
            document.getElementById("tdTargetToSourceRate").innerHTML = "1 Ccy to Ccy:";
        }
        
        if (source == target)
        {
            document.getElementById("txtSourceToTargetRate").value = 1;
            document.getElementById("txtTargetToSourceRate").value = 1;
            
            document.getElementById("txtSourceToTargetRate").disabled = "disabled"; 
            document.getElementById("txtTargetToSourceRate").disabled = "disabled";     
        }        
        else
        {   
            document.getElementById("txtSourceToTargetRate").value = "";
            document.getElementById("txtTargetToSourceRate").value = "";
            
            document.getElementById("txtSourceToTargetRate").disabled = ""; 
            document.getElementById("txtTargetToSourceRate").disabled = "disabled";  
        }
    }
    
    this.do_ajaxgridviewer = function (parameter)
    {
        this.tranxdate = document.getElementById("txtEffectivityDate").value;
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                document.getElementById("txtHidden_bookingrates_SEQ_ID").value = command_param[1];

                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    
                    if (document.getElementById("btnAddBookingRates").disabled == true)
                    {
                        this.rollback_record();
                        this.load_record();
                    }
                    //
                    this.load_record();
                    return;
                }
                this.process_grid_viewer(parameter);
                
                //this.load_bookingrates_grid_viewer();
            }
        }
    }   

    this.process_grid_viewer = function (parameter)
    {
        this.loadergrid = new grid_viewer();
        this.loadergrid.sql_source     = "sp_bookingrates_grid~" + this.BranchCode.value + "~" + this.tranxdate;
        this.loadergrid.div_container  = document.getElementById("divtbl_bookingrates");
        this.loadergrid.datarow_column = 0;
        this.loadergrid.process_ajax(parameter);
        this.loadergrid = null;
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
        IRSWorld.GEFUUpload.GEFUMaintenance.GET_BOOKINGRATES_RECORD(this.SEQ_ID, this.pull_server_data_callback);
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
                this.DateInserted       = ds.Tables[0].Rows[0].DateInserted;
    
                document.getElementById("txtHidden_bookingrates_SEQ_ID").value  = this.SEQ_ID;
                //document.getElementById("BranchCode").value                   = this.IflexAccount;
                //document.getElementById("Effectivitydate").value              = this.Description;
                document.getElementById("ddSourceCcy").value                    = this.SourceCcy;
                document.getElementById("ddTargetCcy").value                    = this.TargetCcy;
                document.getElementById("txtSourceToTargetRate").value          = this.SourceToTargetRate;
                document.getElementById("txtTargetToSourceRate").value          = this.TargetToSourceRate;
                //document.getElementById("DateInserted").value                 = this.DateInserted;
                this.commons.to_unlock_button("tblBookingRatesButtons","btnCancelBookingRates~btnDeleteBookingRates");
            }
        }
    }    
    
    
    this.delete_record = function()
    {   
        if(confirm('Are you sure you want to delete this booking rate?'))
        {
            IRSWorld.GEFUUpload.GEFUMaintenance.delete_bookingrate(this.SEQ_ID,this.delete_record_callback);    
        }     
        
    }
    
    this.delete_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
                 
            
            this.tranxdate = document.getElementById("txtEffectivityDate").value;
            
            //document.getElementById("tdEffectivityDate").innerText = "Add by Effectivity Date :";
            
            document.getElementById("tdEffectivityDate").innerHTML = "Add by Effectivity Date :";
            
            this.commons = new common_method();                    
            this.commons.to_clear_screen("tblBookingRates");  
            this.commons.to_read_only("tblBookingRates",false);
            this.commons.to_unlock_button("tblBookingRatesButtons","btnSaveBookingRates~btnCancelBookingRates~btnDeleteBookingRates");
            this.commons = null;  
            this.BranchCode = IRSWorld.GEFUUpload.GEFUMaintenance.getBranchCode();            
            document.getElementById("txtEffectivityDate").value = this.tranxdate; 
            alert("Booking rate successfully deleted");
             this.bookingrates = new grid_viewer();
            this.bookingrates.sql_source  = "sp_bookingrates_grid~" + this.BranchCode.value + "~" + this.tranxdate;
            this.bookingrates.div_container = document.getElementById("divtbl_bookingrates");
            this.bookingrates.datarow_column = 0;
            this.bookingrates.open_grid_view();
            this.bookingrates = null;
            
            
        }        
    }    
   
   
    

    this.insert_record = function()
    {        
        IRSWorld.GEFUUpload.GEFUMaintenance.insert_record(this.insert_record_callback);    
    }
    
    
    
    this.insert_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message(); alert(msg.value); return;
            }            
            
            this.tranxdate = document.getElementById("txtEffectivityDate").value;
            
            //document.getElementById("tdEffectivityDate").innerText = "Add by Effectivity Date :";
            
            document.getElementById("tdEffectivityDate").innerHTML = "Add by Effectivity Date :";
            
            this.commons = new common_method();                    
            this.commons.to_clear_screen("tblBookingRates");  
            this.commons.to_read_only("tblBookingRates",false);
            this.commons.to_unlock_button("tblBookingRatesButtons","btnSaveBookingRates~btnCancelBookingRates~btnDeleteBookingRates");
            this.commons = null;  
                        
            document.getElementById("txtEffectivityDate").value = this.tranxdate; 
            
           
            
        }        
    }    
    
    this.populate_dropdownboxes = function ()
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.getSourceCcy(this.BranchCode.value, this.dropdown_SourceCcy_callback);
        IRSWorld.GEFUUpload.GEFUMaintenance.getTargetCcy(this.BranchCode.value, this.dropdown_TargetCcy_callback);        
    }
    
    this.dropdown_BookingRates_callback = function (result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddBookingRatesFilter", result.value, "datefilter", "datefilter");
            this.commons = null;
        }
    }  
    
    this.dropdown_SourceCcy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddSourceCcy", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_TargetCcy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddTargetCcy", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.commit_record = function ()
    {
        if (trim(document.getElementById("ddSourceCcy").value) == "null")
        {
            alert ("Source currency is required");
            document.getElementById( "ddSourceCcy").focus();
            return;
        }
        
        if (trim(document.getElementById("ddTargetCcy").value) == "null")
        {
            alert ("Target currency is required");
            document.getElementById( "ddTargetCcy").focus();
            return;
        }

        if (trim(document.getElementById("txtSourceToTargetRate").value) == "")
        {
            alert ("Booking Rate is required");
            document.getElementById( "txtSourceToTargetRate").focus();
            return;
        }
        
        if (trim(document.getElementById("txtEffectivityDate").value) == "")
        {
            alert ("Effectivity Date is required");
            return;
        }
        var check_bookingexist = IRSWorld.GEFUUpload.GEFUMaintenance.checkbookrates_exist(document.getElementById("ddSourceCcy").value,document.getElementById("ddTargetCcy").value,document.getElementById("txtEffectivityDate").value,this.BranchCode.value).value;
        if(parseInt(check_bookingexist) >0)
        {
            alert('Booking Rate already exist');
            return;
        }
        
        
        IRSWorld.GEFUUpload.GEFUMaintenance.commit_record(this.set_BookingRates(), this.commit_record_callback);        
    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();
                alert(msg.value);
                return;
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");            
            
            this.tranxdate = document.getElementById("txtEffectivityDate").value;
            
            this.commons = new common_method();     
            this.commons.to_clear_screen("tblBookingRates");   
            this.commons.to_read_only("tblBookingRates",true);
            this.commons.to_unlock_button("tblBookingRatesButtons","btnAddBookingRates~btnDeleteBookingRates");
            this.commons = null;       
        
            this.BranchCode = IRSWorld.GEFUUpload.GEFUMaintenance.getBranchCode();
            
            document.getElementById("txtEffectivityDate").value = this.tranxdate;
        
            //document.getElementById("tdEffectivityDate").innerText = "View by Effectivity Date : ";
            
            document.getElementById("tdEffectivityDate").innerHTML = "View by Effectivity Date : ";
            
            this.bookingrates = new grid_viewer();
            this.bookingrates.sql_source  = "sp_bookingrates_grid~" + this.BranchCode.value + "~" + this.tranxdate;
            this.bookingrates.div_container = document.getElementById("divtbl_bookingrates");
            this.bookingrates.datarow_column = 0;
            this.bookingrates.open_grid_view();
            this.bookingrates = null;
            
            var bookingratesresult = null;
            bookingratesresult = IRSWorld.GEFUUpload.GEFUMaintenance.Check_Booking_Rates(this.tranxdate, this.BranchCode.value);
            if (bookingratesresult.value  == "")
            {
                alert("Booking Rates Completed");
            }           
        }
    }
    
    this.rollback_record = function()
    {        
        IRSWorld.GEFUUpload.GEFUMaintenance.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();

        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }
        
        this.tranxdate = document.getElementById("txtEffectivityDate").value;
        
        this.commons = new common_method();        
        this.commons.to_read_only("tblBookingRates",true);
        this.commons.to_clear_screen("tblBookingRates");
        this.commons.to_unlock_button("tblBookingRatesButtons","btnAddBookingRates~");
        this.commons = null;
        
        //document.getElementById("tdEffectivityDate").innerText = "View by Effectivity Date :";
        //document.getElementById("tdSourceToTargetRate").innerText = "1 Ccy to Ccy:";
        //document.getElementById("tdTargetToSourceRate").innerText = "1 Ccy to Ccy:";
        
        document.getElementById("tdEffectivityDate").innerHTML = "View by Effectivity Date :";
        document.getElementById("tdSourceToTargetRate").innerHTML = "1 Ccy to Ccy:";
        document.getElementById("tdTargetToSourceRate").innerHTML = "1 Ccy to Ccy:";
        
        document.getElementById("txtEffectivityDate").value = this.tranxdate;
        
        return;
    }
    
//    this.rollback_record = function()
//    {
//        this.commons = new common_method();        
//        this.commons.to_read_only("tblBookingRates",true);
//        this.commons.to_clear_screen("tblBookingRates");
//        this.commons.to_unlock_button("tblBookingRatesButtons","btnAddBookingRates~");
//        this.commons = null;            
//    }
        
    this.set_BookingRates = function ()
    {  
        
        BranchCode          ="BBBCCC";//         = document.getElementById("BranchCode").value;
        Effectivitydate     = document.getElementById("txtEffectivityDate").value; //get_current_date_string();//               = document.getElementById("Effectivitydate").value;
        SourceCcy           = document.getElementById("ddSourceCcy").value;
        TargetCcy           = document.getElementById("ddTargetCcy").value;
        SourceToTargetRate  = document.getElementById("txtSourceToTargetRate").value;
        TargetToSourceRate  = document.getElementById("txtTargetToSourceRate").value;
        //DateInserted        = document.getElementById("txtEffectivityDate").value;//get_current_date_string();// 
        DateInserted        = get_current_date_string();//             = document.getElementById("DateInserted").value;        
        UserID              ="UUUIII";
        return this.push_data_to_server();
    }
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]   = "BranchCode~"         + BranchCode;
        data_row[data_row.length]   = "Effectivitydate~"    + Effectivitydate;
        data_row[data_row.length]   = "SourceCcy~"          + SourceCcy;
        data_row[data_row.length]   = "TargetCcy~"          + TargetCcy;
        data_row[data_row.length]   = "SourceToTargetRate~" + SourceToTargetRate;
        data_row[data_row.length]   = "TargetToSourceRate~" + TargetToSourceRate;
        data_row[data_row.length]   = "DateInserted~"       + DateInserted;
        data_row[data_row.length]   = "UserID~"             + UserID;
        
        return data_row.join("$");
    }
    
    this.set_default_date = function ()
    {
        document.getElementById("txtEffectivityDate").value = get_current_date_string().substring(0, 10);
    }
}

