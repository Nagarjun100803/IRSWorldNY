function cif_spouse()
{
    this.commons          = null;
    this.generic          = null;
    this.SEQ_ID           = null;
    this.RemitterCIFID    = null;
    this.SpouseLastName   = null;
    this.SpouseFirstName  = null;
    this.SpouseMiddleName = null;
    this.BeneCIFID        = null;
    var iSpouse           = this;
        
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblSpouse");
        this.commons.to_read_only("tblSpouse", true); 
        this.commons.to_unlock_button("tblSpouse","btnAddtoBene");
        this.commons = null;
       
        this.grid = new grid_viewer();     
        this.session_end         = true;   
        this.grid.sql_source     = "sp_cif_grid_spouse~" + this.RemitterCIFID;
        this.grid.div_container  = document.getElementById("CIFSPOUSEGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;                       
    }
    
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_cif_grid_spouse~" + this.RemitterCIFID;
        this.grid.div_container  = document.getElementById("CIFSPOUSEGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
        
    this.load_record = function()
    {
        this.commons = new common_method();  
        this.commons.to_clear_screen("tblSpouse");
        this.commons.to_read_only("tblSpouse", true); 
        this.commons.to_unlock_button("tblSpouse","btnAddtoBene");
        this.commons = null;
            
        this.pull_server_data();
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {
        this.SpouseLastName   = document.getElementById("txtSpouseLName").value;  
        this.SpouseFirstName  = document.getElementById("txtSpouseFName").value;  
        this.SpouseMiddleName = document.getElementById("txtSpouseMName").value;  
        
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
        
        IRSWorld.CIF.cifmain.GET_SPOUSE_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }
    
    this.pull_server_data_callback = function(result)
    {
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                /*
                this.commons = new common_method();
                this.commons.to_clear_screen("tblSpouse");
                this.commons.to_read_only("tblSpouse", true); 
                this.commons.to_unlock_button("tblSpouse","btnAddtoBene");
                */
                
                iSpouse.SEQ_ID           = ds.Tables[0].Rows[0].SEQ_ID;
                iSpouse.RemitterCIFID    = ds.Tables[0].Rows[0].RemitterCIFID;
                iSpouse.SpouseLastName   = ds.Tables[0].Rows[0].SpouseLastName;
                iSpouse.SpouseFirstName  = ds.Tables[0].Rows[0].SpouseFirstName;
                iSpouse.SpouseMiddleName = ds.Tables[0].Rows[0].SpouseMiddleName;
                iSpouse.BeneCIFID        = ds.Tables[0].Rows[0].BeneCIFID;                

                document.getElementById("txtHiddenSeqId").value     = iSpouse.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value     = iSpouse.RemitterCIFID; 
                document.getElementById("txtHiddenBeneCIFID").value = iSpouse.BeneCIFID; 
                document.getElementById("txtSpouseLName").value     = iSpouse.SpouseLastName; 
                document.getElementById("txtSpouseFName").value     = iSpouse.SpouseFirstName;
                document.getElementById("txtSpouseMName").value     = iSpouse.SpouseMiddleName;
                
                this.generic = new generic_method();
                this.generic.to_clear_null("txtSpouseLName~txtSpouseFName~txtSpouseMName");
                
            }
        }          
    }
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "RemitterCIFID~"    + this.RemitterCIFID;
        data_row[data_row.length]  = "SpouseLastName~"   + this.SpouseLastName;
        data_row[data_row.length]  = "SpouseFirstName~"  + this.SpouseFirstName;
        data_row[data_row.length]  = "SpouseMiddleName~" + this.SpouseMiddleName;
        
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
            this.set_message("UNABLE TO INSERT RECORD. " + result.error);
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
            this.commons.to_clear_screen("tblSpouse");
            this.commons.to_read_only("tblSpouse", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel"); 
            this.commons.to_lock_button("tblSpouse", "btnAddtoBene");
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
            this.set_message("UNABLE TO UPDATE RECORD. " + result.error); return; 
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
            this.commons.to_read_only("tblSpouse", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel"); 
            this.commons.to_lock_button("tblSpouse", "btnAddtoBene");
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
        IRSWorld.CIF.cifmain.commit_record( this.set_record(), this.commit_record_callback ); this.SEQ_ID = null;
    }
    
    this.commit_record_callback = function(result)
    {
	    iSpouse.load_record();
        
        if (typeof(iSpouse.load_grid_viewer) != 'undefined')
        {
            iSpouse.load_grid_viewer();
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
                    alert("ERROR ENCOUNTERED WHILE SAVING. PLEASE TRY AGAIN ...\n\n" + msg.value); return;
                }                
            }
            
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");              
            this.commons = null;
            
            document.getElementById("trSpouseWdBtn").style.display = "block";
            document.getElementById("trSpouseWoBtn").style.display = "none";
            
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
        this.commons.to_unlock_button("tblSpouse","btnAddtoBene");
        this.commons = null;
    }
    
    this.validate_input = function()
    { 
        this.generic     = new generic_method();      
        this.error_input = '';  
        var foc_elem     = '';
        
        this.LName = document.getElementById("txtSpouseLName").value;
        this.FName = document.getElementById("txtSpouseFName").value;
        this.MName = document.getElementById("txtSpouseMName").value;
        
        if (this.generic.is_alphanumeric2(this.LName) == false)
        {           
            this.error_input += "Last name contains invalid characters.\n";   
            foc_elem = 'txtSpouseLName';         
        }
        
        if (this.generic.is_alphanumeric2(this.FName) == false)
        {           
            this.error_input += "First name contains invalid characters.\n";    
            if (foc_elem.length == 0) foc_elem = 'txtSpouseFName';         
        }
        
        if (this.generic.is_alphanumeric2(this.MName) == false)
        {           
            this.error_input += "Middle name contains invalid characters.\n";     
            if (foc_elem.length == 0) foc_elem = 'txtSpouseMName';        
        }
        
        if (this.generic.is_empty(this.LName + this.FName + this.MName) == true)
        {
            this.error_input += "Name is required.\n"
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