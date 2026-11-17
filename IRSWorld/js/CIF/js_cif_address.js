function cif_address()
{
    this.commons               = null;
    this.generic               = null;
    this.SEQ_ID                = null;
    this.CIFID                 = null;
    this.Address1              = null;
    this.Address2              = null;
    this.Address3              = null;
    this.Address4              = null;
    this.Address5              = null;
    this.Address6              = null;
    this.CountryCode           = null;
    this.ZipCodeSeqId          = null;
    this.POBox                 = null;
    this.POBoxDistrict         = null;
    this.Deleted               = null;
    this.AddressType           = null;
    this.Remarks               = null;
    this.PreferredMailAdd      = null;
    this.PostalCode            = null;
    this.grid                  = null;
    var iAddress               = this; 
    
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblAddress");
        this.commons.to_clear_screen("tblPostalCode");
        this.commons = null;
        
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_address~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFADDRESSGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_cif_grid_address~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFADDRESSGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.cancel_insert = function()
    {
	    this.commons = new common_method();
	    this.commons.to_clear_screen("tblAddress");
	    this.commons.to_read_only("tblAddress", true);
	    this.commons.to_clear_screen("tblPostalCode");
	    this.commons.to_read_only("tblPostalCode", true);
	    this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {
        this.commons = new common_method();  
        this.commons.to_clear_screen("tblAddress");
        this.commons.to_read_only("tblAddress", true);
        this.commons.to_clear_screen("tblPostalCode");
        this.commons.to_read_only("tblPostalCode", true);        
        this.commons = null; 
           
        this.pull_server_data();
    }
    
    this.disable_some_fields = function()
    {   
        document.getElementById("txtMuniProv").readOnly = true;
        document.getElementById("txtPostalCode").readOnly = true;
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {
        this.Address1     = document.getElementById("txtAddress1").value;
        this.Address2     = document.getElementById("txtAddress2").value;
        this.POBox        = document.getElementById("txtPOBox").value;
        this.Address3     = document.getElementById("txtDistrict").value;
        this.Address4     = document.getElementById("txtCity").value;
        this.Address5     = document.getElementById("txtState").value;
        this.Address6     = document.getElementById("txtRegion").value;        
        this.Remarks      = document.getElementById("txtAddRemarks").value;
        this.AddressType  = document.getElementById("ddAddressType").value;
        this.CountryCode  = document.getElementById("ddCountryCode").value;
                
        this.set_prefmailadd();
        this.set_zipcode();
        this.set_postalcode();
        
        //if (this.ZipCodeSeqId != null && this.ZipCodeSeqId != '') this.set_countrycode(this.ZipCodeSeqId);
        
        return this.push_data_to_server();
    }
    
    this.pull_server_data = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.CIF.cifmain.get_seq_id().value;
        }
                
        if (this.SEQ_ID == null || this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }
        IRSWorld.CIF.cifmain.GET_ADDRESS_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
     
    this.pull_server_data_callback = function(result)
    {
        if (result != null && result.value != null)
        {            
            var ds = result.value;
            this.commons = new common_method();
            this.generic = new generic_method();
                            
            if (ds.Tables[0].Rows.length > 0)
            {     
                this.commons.to_clear_screen("tblAddress");
                this.commons.to_read_only("tblAddress", true);
                this.commons.to_clear_screen("tblPostalCode");
                this.commons.to_read_only("tblPostalCode", true);
                
                iAddress.SEQ_ID               = ds.Tables[0].Rows[0].SEQ_ID;
                iAddress.Address1             = ds.Tables[0].Rows[0].Address1;
                iAddress.Address2             = ds.Tables[0].Rows[0].Address2;
                iAddress.Address3             = ds.Tables[0].Rows[0].Address3;
                iAddress.Address4             = ds.Tables[0].Rows[0].Address4;
                iAddress.Address5             = ds.Tables[0].Rows[0].Address5;
                iAddress.Address6             = ds.Tables[0].Rows[0].Address6;
                iAddress.CountryCode          = ds.Tables[0].Rows[0].CountryCode;
                iAddress.ZipCodeSeqId         = ds.Tables[0].Rows[0].ZipCodeSeqId;
                iAddress.POBox                = ds.Tables[0].Rows[0].POBox;
                iAddress.POBoxDistrict        = ds.Tables[0].Rows[0].POBoxDistrict;
                iAddress.Deleted              = ds.Tables[0].Rows[0].Deleted;
                iAddress.AddressType          = ds.Tables[0].Rows[0].AddressType;
                iAddress.Remarks              = ds.Tables[0].Rows[0].Remarks;
                iAddress.PreferredMailAdd     = ds.Tables[0].Rows[0].PreferredMailAdd;
                iAddress.PostalCode           = ds.Tables[0].Rows[0].PostalCode;
                iAddress.CountryCode          = ds.Tables[0].Rows[0].CountryCode;
                                     
                document.getElementById("txtHiddenSeqId").value = iAddress.SEQ_ID;
                document.getElementById("txtHiddenCIFID").value = iAddress.CIFID;
                document.getElementById("txtAddress1").value    = iAddress.Address1;
                document.getElementById("txtAddress2").value    = iAddress.Address2; 
                document.getElementById("txtAddRemarks").value  = iAddress.Remarks;
                document.getElementById("chkPrefMailAdd").value = iAddress.PreferredMailAdd;
                document.getElementById("txtPOBox").value       = iAddress.POBox; 
                
                document.getElementById("txtDistrict").value    = iAddress.Address3; 
                document.getElementById("txtCity").value        = iAddress.Address4; 
                document.getElementById("txtState").value       = iAddress.Address5; 
                document.getElementById("txtRegion").value      = iAddress.Address6;
                document.getElementById("ddCountryCode").value  = iAddress.CountryCode; 
                
                document.getElementById("trPostalCode").style.display = 'none';
                if (iAddress.ZipCodeSeqId != null && iAddress.ZipCodeSeqId != 'undefined' && iAddress.ZipCodeSeqId != '')
                {
                    var ds1 = null;
                    ds1 = IRSWorld.CIF.cifmain.get_zipcode(iAddress.ZipCodeSeqId).value;
            
                    if (ds1.Tables[0].Rows.length > 0)
                    {
                        var zipcode      = ds1.Tables[0].Rows[0].Truezipcode;
                        var municipality = ds1.Tables[0].Rows[0].Municipality;
                        var province     = ds1.Tables[0].Rows[0].Province;
                        
                        document.getElementById("txtMuniProv").value = municipality + ", " + province + " " + zipcode;
                        
                        if(this.generic.to_trim(zipcode) == "8680")
                        {                    
                            document.getElementById("trPostalCode").style.display = 'block';
                        }
                    }
                }
                
                if (iAddress.PostalCode != null && iAddress.PostalCode != 'undefined' && iAddress.PostalCode != '')
                {
                    var ds1 = null;
                    ds1 = IRSWorld.CIF.cifmain.get_state(iAddress.PostalCode).value;
            
                    if (ds1.Tables[0].Rows.length > 0)
                    {
                        var state = ds1.Tables[0].Rows[0].State;
                        
                        document.getElementById("txtPostalCode").value = iAddress.PostalCode + " " + "(" + state + ")";
                    }
                } 
                
                document.getElementById("chkPrefMailAdd").value   = "off";
                document.getElementById("chkPrefMailAdd").checked = false;
                
                if (document.getElementById("chkPrefMailAdd").value != null && iAddress.PreferredMailAdd == true)
                {
                    document.getElementById("chkPrefMailAdd").value = "on";
                    document.getElementById("chkPrefMailAdd").checked = true;
                }
                
                this.commons.to_set_dropdownlist_index("ddAddressType",  iAddress.AddressType);
                
                this.generic.to_clear_null("txtAddress1~txtAddress2~txtAddRemarks~txtDistrict~txtCity~txtState~txtRegion~txtPOBox");
                
                this.commons = null;
            }
        }          
    }
        
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"                + this.CIFID;
        data_row[data_row.length]  = "Address1~"             + this.Address1;
        data_row[data_row.length]  = "Address2~"             + this.Address2;
        data_row[data_row.length]  = "Address3~"             + this.Address3;
        data_row[data_row.length]  = "Address4~"             + this.Address4;
        data_row[data_row.length]  = "Address5~"             + this.Address5;
        data_row[data_row.length]  = "Address6~"             + this.Address6;
        data_row[data_row.length]  = "CountryCode~"          + this.CountryCode;
        data_row[data_row.length]  = "ZipCodeSeqId~"         + this.ZipCodeSeqId;
        data_row[data_row.length]  = "POBox~"                + this.POBox;
        data_row[data_row.length]  = "POBoxDistrict~"        + this.POBoxDistrict;
        data_row[data_row.length]  = "Deleted~"              + this.Deleted;
        data_row[data_row.length]  = "AddressType~"          + this.AddressType;
        data_row[data_row.length]  = "Remarks~"              + this.Remarks;
        data_row[data_row.length]  = "PreferredMailAdd~"     + this.PreferredMailAdd;
        data_row[data_row.length]  = "PostalCode~"           + this.PostalCode;
        data_row[data_row.length]  = "Deleted~false";
        
        return data_row.join("|");       
    }

    this.insert_record = function()
    {   
        document.getElementById("txtHiddenSeqId").value = '';
        IRSWorld.CIF.cifmain.insert_record(this.insert_record_callback);
    }
    
    this.insert_record_callback = function(result)
    {
        if (result.error != null && result.error != "undefined")
        {
            alert("UNABLE TO INSERT RECORD. " + result.error);
        }
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {                
                var msg = IRSWorld.CIF.cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }
            }
            
            this.commons = new common_method();
            this.commons.to_clear_screen("tblAddress");
            this.commons.to_read_only("tblAddress", false);
            this.commons.to_clear_screen("tblPostalCode");
            this.commons.to_read_only("tblPostalCode", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;            
            
            iAddress.disable_some_fields();
        }        
    }   
    
    this.update_record = function()
    {
        IRSWorld.CIF.cifmain.update_record(this.SEQ_ID, this.update_record_callback);
    }
    
    this.update_record_callback = function(result)
    {
        if (result.error != null && result.error != "undefined")
        {
            alert("UNABLE TO UPDATE RECORD. " + result.error); return; 
        }
        
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {                
                var msg = IRSWorld.CIF.cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }
            }

            this.commons = new common_method();
            this.commons.to_read_only("tblAddress", false);
            this.commons.to_read_only("tblPostalCode", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            iAddress.disable_some_fields();
        }        
    }    
    
    this.delete_record = function()
    {
        IRSWorld.CIF.cifmain.delete_record(this.SEQ_ID, this.delete_record_callback);
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
        
    this.commit_record = function()
    {
        if (document.getElementById("chkPrefMailAdd").checked == true) 
        {
            IRSWorld.CIF.cifmain.update_preferred_mailadd(this.CIFID, document.getElementById("txtHiddenSeqId").value);
        }
        
        IRSWorld.CIF.cifmain.commit_record( this.set_record(), this.commit_record_callback ); 
        this.SEQ_ID = null; 
        document.getElementById("txtHiddenZipSeqId").value = '';
        document.getElementById("txtHiddenSeqId").value = '';
    }
    
    this.commit_record_callback = function(result)
    {                        
        iAddress.load_record();
        
        if (typeof(iAddress.load_grid_viewer) != 'undefined')
        {
            iAddress.load_grid_viewer();
        }  
            
        if (result.error != null && result.error != "undefined")
        {            
            alert("UNABLE TO COMMIT RECORD. " + result.error); return;
        }
                                
        if (result != null && result.value != "undefined")
        {                                
            if (result.value == false)
            {     
                var msg = IRSWorld.CIF.cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }                
            }
            
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");            
            this.commons = null;
          
            alert("RECORD COMMITTED SUCCESSFULLY");                      
         }
    }
    
    this.rollback_record = function()
    {
        IRSWorld.CIF.cifmain.rollback_record(this.rollback_record_callback);
    }
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.CIF.cifmain.read_message();
        
        if (msg != null && msg.value != "undefined" && msg.value.length > 0)
        {
           alert(msg.value);
        }
        
        this.commons = new common_method();
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");
        this.commons = null;
    }
    
    this.set_prefmailadd = function()
    {
        this.PreferredMailAdd = false;
        
        var address_count = 0;
        address_count     = IRSWorld.CIF.cifmain.get_address_count(document.getElementById("txtHiddenCIFID").value);
        (address_count.value < 1) ? this.PreferredMailAdd = true : this.PreferredMailAdd = false; 
          
        if (document.getElementById("chkPrefMailAdd").value != null && document.getElementById("chkPrefMailAdd").checked == true)
        {
            this.PreferredMailAdd = true;
        }
    }
    
    this.set_zipcode = function()
    {
        if (document.getElementById("txtHiddenSeqId").value != '')
        {
            var ds_zip = null;
            ds_zip = IRSWorld.CIF.cifmain.get_zipcode_seqid(document.getElementById("txtHiddenSeqId").value).value;
            
            if (ds_zip.Tables[0].Rows.length > 0) var zip_seqid = ds_zip.Tables[0].Rows[0].ZipCodeSeqId;
            
            if (document.getElementById("txtHiddenZipSeqId").value == '')
            {
                this.ZipCodeSeqId = zip_seqid;
                return;
            }
            this.ZipCodeSeqId = document.getElementById("txtHiddenZipSeqId").value;
            return;   
        } 
        this.ZipCodeSeqId = document.getElementById("txtHiddenZipSeqId").value;
    }
    
    this.set_postalcode = function()
    {
        if (document.getElementById("txtHiddenSeqId").value != '')
        {
            var ds_post = null;
            ds_post = IRSWorld.CIF.cifmain.get_cif_postalcode(document.getElementById("txtHiddenSeqId").value).value;
            
            if (ds_post.Tables[0].Rows.length > 0) var postal_code = ds_post.Tables[0].Rows[0].PostalCode;
            
            if (document.getElementById("txtHiddenPostalCode").value == '')
            {
                this.PostalCode = postal_code;
                return;
            }
            this.PostalCode = document.getElementById("txtHiddenPostalCode").value;
            return;   
        } 
        this.PostalCode = document.getElementById("txtHiddenPostalCode").value;
    }
    
    this.set_countrycode = function(zip_seqid)
    {
        var ds_countrycode = null;
        ds_countrycode = IRSWorld.CIF.cifmain.get_country_code(zip_seqid).value;
        
        if (ds_countrycode.Tables[0].Rows.length > 0) var countrycode = ds_countrycode.Tables[0].Rows[0].fldvalue;
        
        if (String(countrycode) == "undefined") countrycode = null;
        
        this.CountryCode = countrycode;
    }
    
    this.validate_input = function()
    {
        this.generic     = new generic_method();        
        this.error_input = '';
        var foc_elem     = '';
        var postal       = document.getElementById("txtPostalCode").value  ;
        var municipality = document.getElementById("txtMuniProv").value;
        var zip = (municipality).substring(municipality.length-4, municipality.length);
        var is_pref = '';
        var is_bene = IRSWorld.CIF.cifmain.get_cifclass_count(document.getElementById("txtHiddenMasterSeqId").value, 2).value;
        
        if (this.generic.is_alphanumeric2(document.getElementById("txtAddress1").value) == false)
        { 
            this.error_input += "Invalid Room No. / Bldg.\n";
            if (foc_elem.length == 0) foc_elem = 'txtAddress1';     
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtAddress2").value) == false)
        { 
            this.error_input += "Invalid Street No. / Name.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtAddress2';                                 
        }        
        if (this.generic.is_alphanumeric2(document.getElementById("txtPOBox").value) == false)
        { 
            this.error_input += "Invalid PO Box.\n";
            if (foc_elem.length == 0) foc_elem = 'txtPOBox';     
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtDistrict").value) == false)
        { 
            this.error_input += "Invalid District.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtDistrict';                                 
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtCity").value) == false)
        { 
            this.error_input += "Invalid City.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtCity';
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtState").value) == false)
        { 
            this.error_input += "Invalid State.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtState';
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtRegion").value) == false)
        { 
            this.error_input += "Invalid Region.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtRegion';
        }
        if(zip == "8680")
        {
            if(postal.length == 0)
            {
                this.error_input += "Please enter postal code.\n";
            }
        }
        if (foc_elem.length > 0)
        {
            document.getElementById(foc_elem).focus();
        }
        if (this.error_input != '')
        {
            return false;
        }
        
        return true;
    }
}