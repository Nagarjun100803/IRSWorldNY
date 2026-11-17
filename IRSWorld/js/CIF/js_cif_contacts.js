function cif_contacts()
{
    this.commons         = null;
    this.generic         = null;
    this.SEQ_ID          = null;
    this.CIFID           = null;
    this.ContactType     = null;
    this.ContactNo       = null;
    this.Remarks         = null;
    //this.SMSAlert        = null;
    this.EmailAddress    = null;    
    this.PreferredNo     = null; 
    this.grid            = null;    
    var iContacts        = this;
    
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblContact");
        this.commons = null;
        
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_contacts~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFCONTACTSGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_contacts~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFCONTACTSGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null; 
    }
    
//    this.show_smsalert_field = function()
//    {
//        if (document.getElementById("ddContactType").value == 7) /*contact type is mobile*/
//        {
//            document.getElementById("chkSMSAlert").disabled = false;
//        }
//        else
//        {
//            document.getElementById("chkSMSAlert").disabled = true;
//        }
//    }
    
    this.get_contact_type = function(seq_id)
    {
        var ds = null;
        ds = IRSWorld.CIF.cifmain.get_contact_type(seq_id).value; 
        if (ds.Tables[0].Rows.length > 0) var contact_type = ds.Tables[0].Rows[0].ContactType;
        return contact_type;
    }
    
    this.cancel_insert = function()
    {
	    this.commons = new common_method();
	    this.commons.to_clear_screen("tblContact");
	    this.commons.to_read_only("tblContact", true);
	    this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {                
        this.commons = new common_method();      
        this.commons.to_clear_screen("tblContact"); 
        this.commons.to_read_only("tblContact", true);
        this.commons = null;
        
        this.pull_server_data();                
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {
        this.ContactType    = document.getElementById("ddContactType").value;
        this.ContactNo      = document.getElementById("txtContactNo").value;
        this.Remarks        = document.getElementById("txtContactRemarks").value;   
        this.EmailAddress   = document.getElementById("txtEmailAddress").value;
        
        //this.set_smsalert();
        this.set_preferredno(); 
         
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
        
        var cont_type = this.get_contact_type(this.SEQ_ID); 
        //this.show_smsalert_field();       
        IRSWorld.CIF.cifmain.GET_CONTACTS_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
     
    this.pull_server_data_callback = function(result)
    {                 
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                this.commons = new common_method();
                this.commons.to_clear_screen("tblContact");
                this.commons.to_read_only("tblContact", true);
                
                iContacts.SEQ_ID       = ds.Tables[0].Rows[0].SEQ_ID;
                iContacts.ContactType  = ds.Tables[0].Rows[0].ContactType;
                iContacts.ContactNo    = ds.Tables[0].Rows[0].ContactNo;
                iContacts.Remarks      = ds.Tables[0].Rows[0].Remarks;
                //this.SMSAlert     = ds.Tables[0].Rows[0].SMSAlert;
                iContacts.EmailAddress = ds.Tables[0].Rows[0].EmailAddress;
                iContacts.PreferredNo  = ds.Tables[0].Rows[0].PreferredNo;
                                
                document.getElementById("txtHiddenSeqId").value    = iContacts.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value    = iContacts.CIFID; 
                document.getElementById("txtContactNo").value      = iContacts.ContactNo;
                document.getElementById("txtContactRemarks").value = iContacts.Remarks;    
                document.getElementById("txtEmailAddress").value   = iContacts.EmailAddress;     
//                document.getElementById("chkSMSAlert").value       = this.SMSAlert;         
//                
//                document.getElementById("chkSMSAlert").value   = "off";
//                document.getElementById("chkSMSAlert").checked = false;
//                
//                if (document.getElementById("chkSMSAlert").value != null && this.SMSAlert == true)
//                {
//                    document.getElementById("chkSMSAlert").value = "on";
//                    document.getElementById("chkSMSAlert").checked = true;
//                }
                
                document.getElementById("chkPreferredNo").value   = "off";
                document.getElementById("chkPreferredNo").checked = false;
                
                if (document.getElementById("chkPreferredNo").value != null && iContacts.PreferredNo == true)
                {
                    document.getElementById("chkPreferredNo").value = "on";
                    document.getElementById("chkPreferredNo").checked = true;
                }
               
                this.commons.to_set_dropdownlist_index("ddContactType",iContacts.ContactType);
                
                this.generic = new generic_method();
                this.generic.to_clear_null("txtContactNo~txtEmailAddress");
                this.commons = null;
            }
        }          
    }
        
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"         + this.CIFID;
        data_row[data_row.length]  = "ContactType~"   + this.ContactType;
        data_row[data_row.length]  = "ContactNo~"     + this.ContactNo;
        data_row[data_row.length]  = "Remarks~"       + this.Remarks; 
        data_row[data_row.length]  = "EmailAddress~"  + this.EmailAddress;
        data_row[data_row.length]  = "PreferredNo~"   + this.PreferredNo;
        data_row[data_row.length]  = "Deleted~false";
        
        return data_row.join("|");       
    }
    
    this.insert_record = function()
    {
        document.getElementById("txtHiddenSeqId").value = 0;
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
            this.commons.to_clear_screen("tblContact");
            this.commons.to_read_only("tblContact", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null; 
            
            //iContacts.show_smsalert_field();
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
            this.commons.to_read_only("tblContact", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            //iContacts.show_smsalert_field();
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
        if (document.getElementById("chkPreferredNo").checked == true) IRSWorld.CIF.cifmain.update_preferred_no(this.CIFID, document.getElementById("txtHiddenSeqId").value);
        //if (document.getElementById("chkSMSAlert").checked == true)    IRSWorld.CIF.cifmain.update_sms_alert(this.CIFID, document.getElementById("txtHiddenSeqId").value);
        IRSWorld.CIF.cifmain.commit_record( this.set_record(), this.commit_record_callback );
        this.SEQ_ID = null;
    }
    
    this.commit_record_callback = function(result)
    {                     
        iContacts.load_record();
        
        if (typeof(iContacts.load_grid_viewer) != 'undefined')
        {
            iContacts.load_grid_viewer();
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
            
            /* RFC-31620 */
            var msg = IRSWorld.CIF.cifmain.isAdminApproverTag().value;
            if (msg != "") {
                alert(msg);
            }
            else {
                alert("RECORD COMMITTED SUCCESSFULLY");
            }
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
        return;        
    }
    
    this.set_branchdef_values = function()
    {
        var ds = IRSWorld.CIF.cifmain.get_branchdef_values(document.getElementById("txtHiddenUserLocation").value).value;
        
        if(ds.Tables[0].Rows.length > 0)
        {
            this.ContactType = ds.Tables[0].Rows[0].ContactType;
            
            this.commons = new common_method();
            this.commons.to_set_dropdownlist_index("ddContactType",this.ContactType);
            this.commons = null;
        }
    }
    
    this.validate_input = function()
    {     
        this.generic     = new generic_method();        
        this.error_input = '';
        var foc_elem     = '';
        var get_mer      = IRSWorld.CIF.cifmain.get_cifclass_count(document.getElementById("txtHiddenMasterSeqId").value, 3).value;
        var is_pref      = '';
        var hdsq         = document.getElementById("txtHiddenSeqId").value;
        
        if (get_mer > 0)
        {
            if(this.generic.is_empty(document.getElementById("txtEmailAddress").value) == true)
            {
                this.error_input += "Email address is required for merchant.\n"
                if (foc_elem.length == 0) foc_elem = 'txtEmailAddress'; 
            }            
        }    
        if (this.generic.to_validate_email(document.getElementById("txtEmailAddress").value) == false)
        {
            this.error_input += "Wrong Email Address Format.\n"
            if (foc_elem.length == 0) foc_elem = 'txtEmailAddress'; 
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
    
//    this.set_smsalert = function()
//    {
//        this.SMSAlert = false;          
//        if (document.getElementById("chkSMSAlert").value != null && document.getElementById("chkSMSAlert").checked == true)
//        {
//            this.SMSAlert = true;
//        }
//    }
    
    this.set_preferredno = function()
    {
        this.PreferredNo = false;  
        
        var contacts_count = 0;
        contacts_count     = IRSWorld.CIF.cifmain.get_contacts_count(document.getElementById("txtHiddenCIFID").value);
        (contacts_count.value < 1) ? this.PreferredNo = true : this.PreferredNo = false; 
              
        if (document.getElementById("chkPreferredNo").value != null && document.getElementById("chkPreferredNo").checked == true)
        {
            this.PreferredNo = true;
        }
    }
}