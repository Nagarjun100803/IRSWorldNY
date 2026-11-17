function Internet_Remitter_Ids()
{
    this.commons            = null;
    this.generic            = null;
    this.SEQ_ID             = null;
    this.CIFID              = null;
    this.IdType             = null;
    this.IDNo               = null;
    this.ExpiryDate         = null;
    this.Remarks            = null;
    this.ScannedID          = null;
    this.VisaType           = null;
    this.IDClassification   = null;
    this.PlaceOfIssuance    = null;
    this.grid               = null;
    this.error_msg          = '';
    var IDs                 = this;
    
    this.load_grid_viewer = function()
    {
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_ids~" + this.get_rem_cifid();
        this.grid.div_container  = document.getElementById("DIVIDSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.function_exec = function()
        {
            Internet_Remitter_Main.rem_ids_grid_ready("DIVIDSGRID");
        };        
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.get_rem_cifid = function()
    {
        var rcifid    = '';
        var rem_cifid = document.getElementById("txtHiddenRemCIFID").value;
        var src_cifid = document.getElementById("txtHiddenSearchCIFID").value;
        
        if (rem_cifid != '') rcifid = rem_cifid;
        else rcifid = src_cifid;
        
        return rcifid;
    }
    
    this.save_mode_ids = function()
    {
        document.getElementById("btnAddIds").disabled    = true;
        document.getElementById("btnDeleteIds").disabled    = true;
        document.getElementById("btnEditIds").disabled   = true;
        document.getElementById("btnSaveIds").disabled   = false;
        document.getElementById("btnCancelIds").disabled = false;
        document.getElementById("btnGetGFCCK").disabled = true;
        
        if (document.getElementById("txtGFCCK").value != '' && document.getElementById("txtGFCCK").value != null && document.getElementById("txtGFCCK").value != 'null')
        {
            document.getElementById('txtRemIDNo').disabled = true;
        }
    }
      
    this.add_mode_ids = function()
    {
        document.getElementById("btnAddIds").disabled    = false;
        document.getElementById("btnDeleteIds").disabled    = true;
        document.getElementById("btnEditIds").disabled   = true;
        document.getElementById("btnSaveIds").disabled   = true;
        document.getElementById("btnCancelIds").disabled = true;
        document.getElementById("btnGetGFCCK").disabled = true;
    }
    
    this.edit_mode_ids = function()
    {
        document.getElementById("btnAddIds").disabled    = false;
        document.getElementById("btnEditIds").disabled   = false;
        document.getElementById("btnDeleteIds").disabled = false;
        document.getElementById("btnSaveIds").disabled   = true;
        document.getElementById("btnCancelIds").disabled = true;
        //document.getElementById("btn_delete").disabled = true;
        //btn_delete
        
        if (    
                document.getElementById("ddRemIDType").value == '38' && 
                (document.getElementById("txtGFCCK").value == '' | document.getElementById("txtGFCCK").value == null | document.getElementById("txtGFCCK").value == 'null')
            )                
        {
            document.getElementById("btnGetGFCCK").disabled = false;
        }
        else
        {
            document.getElementById("btnGetGFCCK").disabled = true;
        }
        
    }
    
    this.btn_add = function()
    {
        Internet_Remitter_Main.check_session();
        
        this.clear_screen();   
        this.disable_field(false);
        this.color_asterisks();
        this.save_mode_ids();
        this.insert_flag = true;
    }
    
    this.btn_edit = function()
    {
        Internet_Remitter_Main.check_session();

        this.disable_field(false);
        this.color_asterisks();
        this.save_mode_ids();
        this.insert_flag = false;
    }
    
    this.btn_cancel = function()
    {
        Internet_Remitter_Main.check_session();
        this.load_record();
    }
    
    this.btn_delete = function()
    {
        Internet_Remitter_Main.check_session();
        var plist = "";
	    plist += "CIFID~" + this.get_rem_cifid() + '|';	    
	    if (this.SEQ_ID != '' && this.SEQ_ID != null && this.SEQ_ID != 'null')                                  plist += "SEQ_ID~"           + this.SEQ_ID           + '|';
	    plist += "Deleted~true";
               	    
	    var ret_val = IRSWorld.Internet.InternetRemitter.update_ids(plist);
        if (ret_val.error != null)
        {
            alert("Failed to delete record. Please try again")
        }
        else
        {
            this.SEQ_ID = "";//tostr_ret_val;
            this.load_record();
            this.load_grid_viewer();
            alert("RECORD COMMITTED SUCCESSFULLY!");
            //this.disable_field(true);
            //this.edit_mode_ids();
        }	 	            
    }
    
    this.btn_save = function()
	{
	    Internet_Remitter_Main.check_session();
	    if(Internet_Remitter_Main.check_mandatory_fields() == true)
	    {
	        if (this.validate_input() == true)
	        {
	            if (this.check_id_details() == true)
	            {
                    if(this.insert_flag == true)
	                {
	                    var ret_val = IRSWorld.Internet.InternetRemitter.add_ids(this.set_ids_record()).value; 
	                }
	                else
	                {
	                    var ret_val = IRSWorld.Internet.InternetRemitter.update_ids(this.set_ids_record()).value; 
	                }
	                var tostr_ret_val = String(Number(ret_val));
        				
			        if(tostr_ret_val == "NaN")
			        {
				        alert(ret_val + '\n' + "Please try again."); return;
			        }
			        else
			        {
			            //this.SEQ_ID = tostr_ret_val;
			            this.load_grid_viewer();
			            alert("RECORD COMMITTED SUCCESSFULLY!");
			            this.disable_field(true);
			            this.edit_mode_ids();
			        }	            
	            }
	        }
	    }
	}
	
	this.check_id_details = function()
	{
	    var ID_Exists = false;	    
	    var ID_Name = null;
	    
	    var remCIF  =   document.getElementById('txtHiddenRemCIFID').value;
        remCIF = ((remCIF != null && remCIF != '' && remCIF != 'null' && remCIF != undefined) ? remCIF : "");		    
	    
        if (document.getElementById('txtRemIDNo').value != null && document.getElementById('txtRemIDNo').value != '' && document.getElementById('ddRemIDType').value != null && document.getElementById('ddRemIDType').value != 'null' && document.getElementById('ddRemIDType').value != '')
        {
            var ID_DS = IRSWorld.Internet.InternetRemitter.get_id_details(document.getElementById('txtRemIDNo').value, document.getElementById('ddRemIDType').value, remCIF).value;
            
            if (ID_DS[0] != null && ID_DS[0].Tables[0].Rows.length > 0)
            {
                ID_Exists  =   true;
                ID_Name    =   ID_DS[0].Tables[0].Rows[0].ClientName.toString();
            }
        }
	    	    
	    if (ID_Exists == true)
	    {
	        if (confirm("ID Description and ID Number already belong to \n\r" + ID_Name + ". Do you want to proceed in adding ID Number?") != true) 
	        {
	            return false;
	        }    
	    }
	    
	    return true;
	}	
	
    this.btn_get_gfc_ck = function()
    {
        Internet_Remitter_Main.check_session(); 
        
        if (document.getElementById("txtRemIDNo").value != null &&  document.getElementById("txtRemIDNo").value != 'null' && document.getElementById("txtRemIDNo").value != '')
        {
            this.show_div_enroll_gfc_loading(true);
            AjaxPro.timeoutPeriod = 100 * 1000;
            AjaxPro.onTimeout = function()
            {
                this.show_div_enroll_gfc_loading(false);       
                alert("SESSION TIMEOUT");
                return; 
            }            
            IRSWorld.Internet.InternetRemitter.enrollgfc(document.getElementById("txtRemIDNo").value, this.get_rem_cifid(), this.get_gfc_ck_callback); 
        }
        else
        {
            alert("Client ID field is empty.");
        }
    }	
	
	this.get_gfc_ck_callback = function(result)
	{
	    Internet_Remitter_Ids.show_div_enroll_gfc_loading(false);
	    if (result.error != null)
	    {
	        alert(result.error.Message.toString());
	    }
	    else
	    {   
	        document.getElementById('txtGFCCK').value = result.value.toString();
	        alert('GFC Enrollment is successful');
	    }
	}
	
    this.show_div_enroll_gfc_loading = function(isVisible)
	{   
	    if(isVisible == true)
	    {
	        this.commons = new common_method();
            this.commons.to_show_loading_message('divEnrollGFCLoading_');
	        document.getElementById('divEnrollGFCLoading').style.display = '';
	        document.getElementById('Label5').innerHTML = "Retrieving GFC CK, please wait...";
	        this.commons = null;
	        
            document.getElementById("btnGetGFCCK").disabled    = true;        	        
	    }
	    else
	    {
	        document.getElementById('divEnrollGFCLoading').style.display = 'none';
            document.getElementById("btnGetGFCCK").disabled    = false;
	    }
	}	
	
    this.set_ids_record = function()
    {  
   
        this.IDNo               = document.getElementById("txtRemIDNo").value; 
        this.ExpiryDate         = document.getElementById("txtRemIDExpiryDate").value;
        this.Remarks            = document.getElementById("txtRemIDRemarks").value;
        this.IdType             = document.getElementById('ddRemIDType').value;
        this.VisaType           = document.getElementById('ddRemVisaType').value;
        this.IDClassification   = document.getElementById('ddRemIDClassification').value;
        this.PlaceOfIssuance    = document.getElementById('txtRemPlaceOfIssuance1').value;
        
        return this.set_plist_ids();
    }
    
    this.set_plist_ids = function()
	{
	    var plist = '';
	    
	    plist += "CIFID~" + this.get_rem_cifid() + '|';	    
	    if (this.SEQ_ID != '' && this.SEQ_ID != null && this.SEQ_ID != 'null')                                  plist += "SEQ_ID~"           + this.SEQ_ID           + '|';
	    if (this.IDNo != '' && this.IDNo != null && this.IDNo != 'null')                                        plist += "IDNo~"             + this.IDNo             + '|';
	    if (this.ExpiryDate != '' && this.ExpiryDate != null && this.ExpiryDate != 'null')                      plist += "IDExpiryDate~"       + this.ExpiryDate       + '|';
	    if (this.Remarks != '' && this.Remarks != null && this.Remarks != 'null')                               plist += "IDRemarks~"          + this.Remarks          + '|';	    
	    if (this.IdType != '' && this.IdType != null && this.IdType != 'null')                                  plist += "IdType~"           + this.IdType           + '|';
	    if (this.VisaType != '' && this.VisaType != null && this.VisaType != 'null')                            plist += "VisaType~"         + this.VisaType         + '|';
	    if (this.IDClassification != '' && this.IDClassification != null && this.IDClassification != 'null')    plist += "IDClassification~" + this.IDClassification + '|';
	    if (this.PlaceOfIssuance != '' && this.PlaceOfIssuance != null && this.PlaceOfIssuance != 'null')    plist += "PlaceOfIssuance~" + this.PlaceOfIssuance + '|';
	    
	    return plist.substring(0, plist.length-1);
	}	
	
	this.load_record = function()
	{
	    this.clear_screen();
	    this.disable_field(true);
	    /* has been called in InternetRemitter_Main before coming here..
	    this.color_asterisks(); 
	    this.add_mode_ids();
	    */
	    if(this.SEQ_ID != null) 
	    {
	        ajax_show_status(true);
	        this.pull_server_data();
	    }
	}
	
	this.pull_server_data = function()
	{
	    IRSWorld.Internet.InternetRemitter.get_ids_record(this.SEQ_ID, this.pull_server_data_callback);
	}
	
    this.pull_server_data_callback = function(result)
    {    
        if (result.error != null)
        {
            alert(result.error.Messages);
        }
    
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                this.commons = new common_method();
           
                this.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                this.IdType             = ds.Tables[0].Rows[0].IdType;
                this.IDNo               = ds.Tables[0].Rows[0].IDNo;
                this.ExpiryDate         = ds.Tables[0].Rows[0].ExpiryDate;
                this.Remarks            = ds.Tables[0].Rows[0].Remarks;
                this.ScannedID          = ds.Tables[0].Rows[0].ScannedID;
                this.VisaType           = ds.Tables[0].Rows[0].VisaType;
                this.IDClassification   = ds.Tables[0].Rows[0].IDClassification;
                this.PlaceOfIssuance   = ds.Tables[0].Rows[0].PlaceOfIssuance;
                
                document.getElementById("txtRemIDNo").value             = this.IDNo; 
                document.getElementById("txtRemIDExpiryDate").value     = this.commons.to_mmddyyyy_date(this.ExpiryDate);
                document.getElementById("txtRemIDRemarks").value        = this.Remarks;
                                
                document.getElementById('ddRemIDType').value            = this.IdType;
                document.getElementById('ddRemVisaType').value          = this.VisaType;
                document.getElementById('ddRemIDClassification').value  = this.IDClassification;
                document.getElementById('txtRemPlaceOfIssuance1').value  = this.PlaceOfIssuance;
                /*var x = document.getElementById('txtRemPlaceOfIssuance1')*/
                document.getElementById('txtGFCCK').value  = ds.Tables[0].Rows[0].GFCCK;
                
                this.generic = new generic_method();
                
                Internet_Remitter_Ids.edit_mode_ids();
                
                this.generic.to_clear_null("txtRemIDNo~txtRemIDExpiryDate~txtRemIDRemarks~txtRemPlaceOfIssuance1");
                this.commons = null;
            }
        }
        ajax_show_status(false);          
    }
    
    this.delete_record = function()
    {
        IRSWorld.Internet.InternetRemitter.delete_record(this.SEQ_ID, this.delete_record_callback);
        cif_main.set_deleted(); 
    }  
    
    this.delete_record_callback = function(result)
    {
        if (result.error != "undefined" && result.error != null)
        {
            alert("UNABLE TO DELETE RECORD. " + result.error); return;
        }
    
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                alert(IRSWorld.CIF.cifmain.read_message()); return false;
            }
            return true;
        }     
    }     
    
	this.clear_screen = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblIds");
        this.commons = null;
    }
    
    this.disable_field = function(isDisabled)
	{   
		this.commons = new common_method();     
		this.commons.to_read_only("tblIds", isDisabled);
		document.getElementById('txtGFCCK').disabled = true;
        this.commons = null;
	}
	
	this.color_asterisks = function()
	{
	    Internet_Remitter_Main.color_asterisk("tblIds");
	}    

    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_ids~" + this.CIFID;
        this.grid.div_container  = document.getElementById("DIVIDSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
   
    this.get_id_type = function(seq_id)
    {
        var id_type = -1;
        var ds = null;
        ds = IRSWorld.Internet.InternetRemitter.get_id_type(seq_id).value; 
        
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {
            id_type = ds.Tables[0].Rows[0].IDType;
        } 
        return id_type;
    }
   
    this.show_visa_type = function()
    {
        
        if (document.getElementById("ddRemIDType").value == 4)
        {
            document.getElementById("ddRemVisaType").disabled = false; 
        }
        else
        {
            document.getElementById("ddRemVisaType").disabled = true;
            document.getElementById("ddRemVisaType").value = 0;
        }
    }
     
    this.validate_input = function()
    {        
        this.generic     = new generic_method();        
        this.error_input = '';
        var foc_elem     = '';
        var idtype       = (document.getElementById("ddRemIDType").selectedIndex >= 0) ? document.getElementById("ddRemIDType")[document.getElementById("ddRemIDType").selectedIndex].text : "";
        
        if (this.generic.is_alphanumeric2(document.getElementById("txtRemIDNo").value) == false)
        {           
            this.error_input += "ID number is invalid.\n";   
            if (foc_elem.length == 0) foc_elem = 'txtRemIDNo';          
        }
        if ((document.getElementById("txtRemIDExpiryDate").value).length < 10 ||
        this.generic.validate_USDate(document.getElementById("txtRemIDExpiryDate").value) == false)
        {           
            this.error_input += "Expiry date is invalid.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtRemIDExpiryDate';         
        }
        if (this.generic.to_validate_expiration(document.getElementById("txtRemIDExpiryDate").value) == false)
        {           
            this.error_input += "ID Expired.\n";  
            if (foc_elem.length == 0) foc_elem = 'txtRemIDExpiryDate';                      
        }
        if(idtype == 'SSS ID')
        {
            var ds = null;
            ds = IRSWorld.Internet.InternetRemitter.validate_sssno(document.getElementById("txtRemIDNo").value, '').value;
            
            if (ds != '1')
            {
                this.error_input += "SSS number is invalid.\n";  
                if (foc_elem.length == 0) foc_elem = 'txtRemIDNo';
            }
        }
        if (foc_elem.length > 0)
        {
            document.getElementById(foc_elem).focus();
        }        
        if (this.error_input != '')
        {
            alert(this.error_input);
            return false;
        }
        
        return true;
    }
}