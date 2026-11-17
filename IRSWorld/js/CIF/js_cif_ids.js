function cif_ids()
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
    //added By Alyssa
    this.PlaceOfIssuance    = null;
    this.grid               = null;
    this.error_msg          = '';
    var IDs                 = this;
    
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblID");
        this.commons = null;
        
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_ids~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFIDGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_ids~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFIDGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.cancel_insert = function()
    {
	    this.commons = new common_method();
	    this.commons.to_clear_screen("tblID");
	    this.commons.to_read_only("tblID", true);
	    this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblID");
        this.commons.to_read_only("tblID", true);
        this.commons = null;
        
        this.pull_server_data();
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {                     
        this.IdType             = document.getElementById("ddIDType").value; 
        this.VisaType           = document.getElementById("ddVisaType").value; 
        this.IDNo               = document.getElementById("txtIDNo").value; 
        this.ExpiryDate         = document.getElementById("txtIDExpiryDate").value;
        this.Remarks            = document.getElementById("txtIDRemarks").value; 
        this.IDClassification   = document.getElementById("ddIDClassification").value; 
        //Added By Alyssa
        this.PlaceOfIssuance    = document.getElementById("txtPlaceOfIssuance").value;
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
        
        var id_type = this.get_id_type(this.SEQ_ID); 
        this.show_visatype_field(id_type); 
        IRSWorld.CIF.cifmain.GET_IDS_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
    this.get_id_type = function(seq_id)
    {
        var id_type = -1;
        var ds = null;
        ds = IRSWorld.CIF.cifmain.get_id_type(seq_id).value; 
        
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {
            id_type = ds.Tables[0].Rows[0].IDType;
        } 
        return id_type;
    }
    
    this.show_visatype_field = function(id_type)
    {
        if (id_type == 4)
        {
            document.getElementById("ddVisaType").disabled = false; 
        }
        else
        {
            document.getElementById("ddVisaType").disabled = true; 
        }
    }
     
    this.pull_server_data_callback = function(result)
    {    
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                this.commons = new common_method();
                this.commons.to_clear_screen("tblID");
                this.commons.to_read_only("tblID", true);
            
                IDs.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                IDs.IdType             = ds.Tables[0].Rows[0].IdType;
                IDs.IDNo               = ds.Tables[0].Rows[0].IDNo;
                IDs.ExpiryDate         = ds.Tables[0].Rows[0].ExpiryDate;
                IDs.Remarks            = ds.Tables[0].Rows[0].Remarks;
                IDs.ScannedID          = ds.Tables[0].Rows[0].ScannedID;
                IDs.VisaType           = ds.Tables[0].Rows[0].VisaType;
                IDs.IDClassification   = ds.Tables[0].Rows[0].IDClassification;
                //Added By Alyssa
                IDs.PlaceOfIssuance    = ds.Tables[0].Rows[0].PlaceOfIssuance
                
                document.getElementById("txtHiddenSeqId").value         = IDs.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value         = IDs.CIFID;                        
                document.getElementById("txtIDNo").value                = IDs.IDNo; 
                document.getElementById("txtIDExpiryDate").value        = this.commons.to_mmddyyyy_date(IDs.ExpiryDate);
                document.getElementById("txtIDRemarks").value           = IDs.Remarks;
                //Added By Alyssa
                document.getElementById("txtPlaceOfIssuance").value  = IDs.PlaceOfIssuance;
                
                this.commons.to_set_dropdownlist_index("ddIDType",          IDs.IdType);
                this.commons.to_set_dropdownlist_index("ddVisaType",        IDs.VisaType);
                this.commons.to_set_dropdownlist_index("ddIDClassification",IDs.IDClassification);
                
                this.generic = new generic_method();
                this.generic.to_clear_null("txtIDNo~txtIDExpiryDate~txtIDRemarks~txtPlaceOfIssuance");
                this.commons = null;
            }
        }          
    }
        
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"            + this.CIFID;
        data_row[data_row.length]  = "IdType~"           + this.IdType;
        data_row[data_row.length]  = "IDNo~"             + this.IDNo;
        data_row[data_row.length]  = "ExpiryDate~"       + this.ExpiryDate;
        data_row[data_row.length]  = "Remarks~"          + this.Remarks;
        data_row[data_row.length]  = "ScannedID~"        + this.ScannedID;
        data_row[data_row.length]  = "VisaType~"         + this.VisaType;
        data_row[data_row.length]  = "IDClassification~" + this.IDClassification;
        data_row[data_row.length]  = "PlaceOfIssuance~" + this.PlaceOfIssuance;
        data_row[data_row.length]  = "Deleted~false";
        
        return data_row.join("|");        
    }
    
    this.insert_record = function()
    {
        IRSWorld.CIF.cifmain.insert_record(this.insert_record_callback);
    }
    
    this.insert_record_callback = function(result)
    {
        if (result.error != null && result.error != "undefined")
        {
            alert("UNABLE TO INSERT RECORD. " + result.error); return;
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
            this.commons.to_clear_screen("tblID");
            this.commons.to_read_only("tblID", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            IDs.show_visatype_field(document.getElementById("ddIDType").value);
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
            alert("UNABLE TO INSERT RECORD. " + result.error); return;
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
            this.commons.to_read_only("tblID", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            IDs.show_visatype_field(document.getElementById("ddIDType").value);
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
        IRSWorld.CIF.cifmain.commit_record( this.set_record(), this.commit_record_callback ); 
        this.SEQ_ID = null;      
    }
    
    this.commit_record_callback = function(result)
    {
	    IDs.load_record();
        
        if (typeof(IDs.load_grid_viewer) != 'undefined')
        {
            IDs.load_grid_viewer();
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
    
    this.validate_input = function()
    {        
        this.generic     = new generic_method();        
        this.error_input = '';
        var foc_elem     = '';
        var idtype       = document.getElementById("ddIDType")[document.getElementById("ddIDType").selectedIndex].text;
        
        if (this.generic.is_alphanumeric2(document.getElementById("txtIDNo").value) == false)
        {           
            this.error_input += "ID number is invalid.\n";   
            if (foc_elem.length == 0) foc_elem = 'txtIDNo';          
        }
        if ((document.getElementById("txtIDExpiryDate").value).length < 10 ||
        this.generic.validate_USDate(document.getElementById("txtIDExpiryDate").value) == false)
        {           
            this.error_input += "Expiry date is invalid.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtIDExpiryDate';         
        }
        if (this.generic.to_validate_expiration(document.getElementById("txtIDExpiryDate").value) == false)
        {           
            this.error_input += "ID Expired.\n";  
            if (foc_elem.length == 0) foc_elem = 'txtIDExpiryDate';                      
        }
        if(idtype == 'SSS ID')
        {
            var ds = null;
            ds = IRSWorld.CIF.cifmain.validate_sssno(document.getElementById("txtIDNo").value, '').value;
            
            if (ds != '1')
            {
                this.error_input += "SSS number is invalid.\n";  
                if (foc_elem.length == 0) foc_elem = 'txtIDNo';
            }
        }
        
        if (this.check_id_details() == false)
        {
            this.error_input += "Saving of ID is cancelled.\n";
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
    
	this.check_id_details = function()
	{
	    var ID_Exists = false;	    
	    var ID_Name = null;
	    
	    var remCIF  =   this.CIFID;
        remCIF = ((remCIF != null && remCIF != '' && remCIF != 'null' && remCIF != undefined) ? remCIF : "");		    
	    
        if (document.getElementById('txtIDNo').value != null && document.getElementById('txtIDNo').value != '' && document.getElementById('ddIDType').value != null && document.getElementById('ddIDType').value != 'null' && document.getElementById('ddIDType').value != '')
        {
            var ID_DS = IRSWorld.CIF.cifmain.get_id_details(document.getElementById('txtIDNo').value, document.getElementById('ddIDType').value, remCIF).value;
            
            if (ID_DS != null && ID_DS.Tables[0].Rows.length > 0)
            {
                ID_Exists  =   true;
                ID_Name    =   ID_DS.Tables[0].Rows[0].ClientName.toString();
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
}