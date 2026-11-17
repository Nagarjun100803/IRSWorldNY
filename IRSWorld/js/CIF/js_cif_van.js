
function cif_van()
{
    this.commons            = null;
    this.generic            = null;
    this.SEQ_ID             = null;
    this.CIFID              = null;
    this.VanType            = null;
    this.Van                = null;
    this.VanBeneSeqId       = null;
    this.grid               = null;
    this.error_msg          = '';
    this.RemitterCIFID      = null;
    var icls                = this;
    
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblVan");
        this.commons = null;
        
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_van~" + this.RemitterCIFID + "~" + document.getElementById('txtHiddenUserRole').value;
        this.grid.div_container  = document.getElementById("CIFVANGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_internet_grid_van~" + this.RemitterCIFID + "~" + document.getElementById('txtHiddenUserRole').value;
        this.grid.div_container  = document.getElementById("CIFVANGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.cancel_insert = function()
    {
	    this.commons = new common_method();
	    this.commons.to_clear_screen("tblVan");
	    this.commons.to_read_only("tblVan", true);
	    this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblVan");
        this.commons.to_read_only("tblVan", true);
        this.commons = null;
        
        this.pull_server_data();
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {
        this.VanBeneSeqId       = document.getElementById('txtHiddenVanProductSeqID').value;
        this.CIFID              = document.getElementById('txtHiddenVanBeneCIFID').value;
        this.VanType            = document.getElementById("ddVanType").value;
        this.Van                = document.getElementById("txtVan").value; 
        
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
        
        IRSWorld.CIF.cifmain.GET_VAN_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
         
    this.pull_server_data_callback = function(result)
    {    
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                this.commons = new common_method();
                this.commons.to_clear_screen("tblVan");
                this.commons.to_read_only("tblVan", true);
            
                icls.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                icls.VanType            = ds.Tables[0].Rows[0].VANType;
                icls.Van                = ds.Tables[0].Rows[0].VAN;
                icls.CIFID              = ds.Tables[0].Rows[0].CIFID;
                icls.VanBeneSeqID       = ds.Tables[0].Rows[0].BeneficiarySEQ_ID;
                
                document.getElementById('txtHiddenVanProductSeqID').value   = icls.VanBeneSeqId;
                document.getElementById('txtHiddenVanBeneCIFID').value      = icls.CIFID;
                document.getElementById("txtHiddenSeqId").value     = icls.SEQ_ID;
                //document.getElementById("txtHiddenCIFID").value     = icls.CIFID;
                document.getElementById("txtVan").value             = icls.Van;
                
                var ds2 = IRSWorld.CIF.cifmain.get_van_product(icls.VanBeneSeqID).value;
                                
                if (ds2.Tables[0].Rows.length > 0)
                {                
                    document.getElementById("txtVanProduct").value      = ds2.Tables[0].Rows[0].Description;
                }
                else
                {
                    document.getElementById("txtVanProduct").value      = "";
                }                
                
                this.commons.to_set_dropdownlist_index("ddVanType", icls.VanType);
                this.generic = new generic_method();
                this.generic.to_clear_null("txtVan");
                this.commons = null;
            }
        }          
    }
        
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"            + this.CIFID;
        data_row[data_row.length]  = "VanType~"          + this.VanType;
        data_row[data_row.length]  = "Van~"              + this.Van;
        data_row[data_row.length]  = "BeneficiarySEQ_ID~"+ this.VanBeneSeqId;
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
            this.commons.to_clear_screen("tblVan");
            this.commons.to_read_only("tblVan", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
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
            this.commons.to_read_only("tblVan", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
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
	    icls.load_record();
        
        if (typeof(icls.load_grid_viewer) != 'undefined')
        {
            icls.load_grid_viewer();
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
        
        if (this.generic.is_alphanumeric(document.getElementById("txtVan").value) == false)
        {           
            this.error_input += "Virtual Account number is invalid.\n";   
            if (foc_elem.length == 0) foc_elem = 'txtVan';          
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