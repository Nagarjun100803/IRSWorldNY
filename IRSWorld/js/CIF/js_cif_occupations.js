function cif_occupations()
{
    this.commons          = null;
    this.generic          = null;
    this.CIFCLASS         = null;
    this.SEQ_ID           = null;
    this.CIFID            = null;
    this.OccupationType   = null;
    this.WorkerType       = null;
    this.Position         = null;
    this.Division         = null;
    this.Sector           = null;
    this.Employer         = null;
    this.Salary           = null;
    this.Currency         = null;
    this.Remarks          = null;
    this.Occupation       = null;
    this.DetailedDesc     = null;
    this.SourceOfFunds    = null;
    this.grid             = null;
    var iOcc              = this;
    
    var seq_id      = document.getElementById("txtHiddenMasterSeqId").value;        
    var role_admin  = IRSWorld.CIF.cifmain.get_cifclass_count(seq_id, 7);
    var role_teller = IRSWorld.CIF.cifmain.get_cifclass_count(seq_id, 8);        
    
    if (role_teller.value > 0){document.getElementById("txtHiddenCIFClass").value = 7;}
    if (role_admin.value  > 0){document.getElementById("txtHiddenCIFClass").value = 8;}
    
    this.load_grid_viewer = function()
    {
    
        this.commons = new common_method();
        if (role_teller.value > 0 || role_admin.value > 0) 
        {
            this.commons.to_clear_screen("tblPNBOccupation");
            this.commons.to_read_only("tblPNBOccupation", true);
            this.commons = null;
            this.grid = new grid_viewer();
            this.session_end         = true;
            this.grid.sql_source     = "sp_cif_grid_pnb_occupations~" + this.CIFID;
            this.grid.div_container  = document.getElementById("CIFPNBOCCUPATIONGRID");
        }
        else
        {
            this.commons.to_clear_screen("tblOccupation");
            this.commons.to_read_only("tblOccupation", true);
            this.commons = null;
            this.grid = new grid_viewer();
            this.session_end         = true;
            this.grid.sql_source     = "sp_cif_grid_occupations~" + this.CIFID;
            this.grid.div_container  = document.getElementById("CIFOCCUPATIONSGRID");
        }        
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        if (role_teller.value > 0 || role_admin.value > 0) 
        {
            this.grid.sql_source     = "sp_cif_grid_pnb_occupations~" + this.CIFID;
            this.grid.div_container  = document.getElementById("CIFPNBOCCUPATIONGRID");
        }
        else
        {
            this.grid.sql_source     = "sp_cif_grid_occupations~" + this.CIFID;
            this.grid.div_container  = document.getElementById("CIFOCCUPATIONSGRID");
        }
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.cancel_insert = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblOccupation");
        this.commons.to_read_only("tblOccupation", true);
        this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {
        this.commons = new common_method();   
        if (role_teller.value > 0 || role_admin.value > 0)
        {
            this.commons.to_clear_screen("tblPNBOccupation");
            this.commons.to_read_only("tblPNBOccupation", true);        
        }
        else
        {
            this.commons.to_clear_screen("tblOccupation");
            this.commons.to_read_only("tblOccupation", true);
        }          
        this.commons = null;   
             
        this.pull_server_data();   
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
        
    this.set_record = function()
    {   
        if (role_admin.value > 0 || role_teller.value > 0)
        {
            this.OccupationType = document.getElementById("ddPNBOccupationType").value;
            this.Position       = document.getElementById("txtPNBPosition").value; 
            this.Division       = document.getElementById("txtPNBDivision").value; 
            this.Sector         = document.getElementById("txtPNBSector").value; 
            this.Employer       = document.getElementById("txtPNBSupervisor").value; 
            this.Remarks        = document.getElementById("txtPNBOccuRemarks").value;
            this.WorkerType     = document.getElementById("ddPNBWorkerType").value;
            this.Occupation     = document.getElementById("ddPNBOccupation").value;
        }              
        else
        { 
            this.OccupationType = document.getElementById("ddOccupationType").value; 
            this.Position       = document.getElementById("txtPosition").value; 
            this.Employer       = document.getElementById("txtEmployer").value; 
            this.Salary         = document.getElementById("txtSalary").value; 
            this.Remarks        = document.getElementById("txtOccuRemarks").value; 
            this.WorkerType     = document.getElementById("ddWorkerType").value;  
            this.Currency       = document.getElementById("ddCurrency").value; 
            this.Occupation     = document.getElementById("ddOccupation").value; 
            this.DetailedDesc   = document.getElementById("txtDetailedDesc").value; 
            this.SourceOfFunds  = document.getElementById("ddSourceOfFunds").value;  
        }
        
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
        
        IRSWorld.CIF.cifmain.GET_OCCUPATIONS_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
     
    this.pull_server_data_callback = function(result)
    {            
        iOcc.CIFCLASS = document.getElementById("txtHiddenCIFClass").value;
        
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                this.commons = new common_method();
                this.generic = new generic_method();
                if (role_teller.value > 0 || role_admin.value > 0)
                {
                    this.commons.to_clear_screen("tblPNBOccupation");
                    this.commons.to_read_only("tblPNBOccupation", true);        
                }
                else
                {
                    this.commons.to_clear_screen("tblOccupation");
                    this.commons.to_read_only("tblOccupation", true);
                } 
            
                iOcc.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                iOcc.OccupationType     = ds.Tables[0].Rows[0].OccupationType;
                iOcc.WorkerType         = ds.Tables[0].Rows[0].WorkerType;
                iOcc.Position           = ds.Tables[0].Rows[0].Position;
                iOcc.Division           = ds.Tables[0].Rows[0].Division;
                iOcc.Sector             = ds.Tables[0].Rows[0].Sector;
                iOcc.Employer           = ds.Tables[0].Rows[0].Employer;
                iOcc.Salary             = ds.Tables[0].Rows[0].Salary;
                iOcc.Currency           = ds.Tables[0].Rows[0].Currency;
                iOcc.Remarks            = ds.Tables[0].Rows[0].Remarks;
                iOcc.Occupation         = ds.Tables[0].Rows[0].Occupation;
                iOcc.DetailedDesc       = ds.Tables[0].Rows[0].DetailedDesc;
                iOcc.SourceOfFunds      = ds.Tables[0].Rows[0].SourceOfFunds;

                document.getElementById("txtHiddenSeqId").value        = iOcc.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value        = iOcc.CIFID;                  
                
                if (role_admin.value > 0 || role_teller.value > 0)
                {
                    document.getElementById("txtPNBPosition").value        = iOcc.Position; 
                    document.getElementById("txtPNBSupervisor").value      = iOcc.Employer;
                    document.getElementById("txtPNBOccuRemarks").value     = iOcc.Remarks;
                    document.getElementById("txtPNBDivision").value        = iOcc.Division;
                    document.getElementById("txtPNBSector").value          = iOcc.Sector;
                 
                    this.commons.to_set_dropdownlist_index("ddPNBWorkerType",    iOcc.WorkerType);
                    this.commons.to_set_dropdownlist_index("ddPNBOccupationType",iOcc.OccupationType);
                    this.commons.to_set_dropdownlist_index("ddPNBOccupation",iOcc.Occupation);
                    
                    this.generic.to_clear_null("txtPNBPosition~txtPNBSupervisor~txtPNBOccuRemarks~txtPNBDivision~txtPNBSector");
                }                
                else
                {
                    document.getElementById("txtPosition").value        = iOcc.Position; 
                    document.getElementById("txtEmployer").value        = iOcc.Employer;
                    document.getElementById("txtSalary").value          = this.generic.formatCurrency(iOcc.Salary);
                    document.getElementById("txtOccuRemarks").value     = iOcc.Remarks; 
                    document.getElementById("txtDetailedDesc").value     = iOcc.DetailedDesc; 
                    
                    this.commons.to_set_dropdownlist_index("ddCurrency",       iOcc.Currency);
                    this.commons.to_set_dropdownlist_index("ddWorkerType",     iOcc.WorkerType);
                    this.commons.to_set_dropdownlist_index("ddOccupationType", iOcc.OccupationType);
                    this.commons.to_set_dropdownlist_index("ddOccupation",     iOcc.Occupation);
                    this.commons.to_set_dropdownlist_index("ddSourceOfFunds",  iOcc.SourceOfFunds);
                    
                    this.generic.to_clear_null("txtDetailedDesc~txtPosition~txtEmployer~txtSalary~txtOccuRemarks");
                }                
                this.commons = null;           
            }
        }       
    }
        
    this.push_data_to_server = function()
    {
        var data_row;
        
        data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"           + this.CIFID;
        data_row[data_row.length]  = "OccupationType~"  + this.OccupationType;
        data_row[data_row.length]  = "Position~"        + this.Position;
        data_row[data_row.length]  = "Division~"        + this.Division;
        data_row[data_row.length]  = "Sector~"          + this.Sector;
        data_row[data_row.length]  = "Employer~"        + this.Employer;
        data_row[data_row.length]  = "Salary~"          + this.Salary;
        data_row[data_row.length]  = "Remarks~"         + this.Remarks;
        data_row[data_row.length]  = "Currency~"        + this.Currency;
        data_row[data_row.length]  = "WorkerType~"      + this.WorkerType;
        data_row[data_row.length]  = "Occupation~"      + this.Occupation;
        data_row[data_row.length]  = "DetailedDesc~"    + this.DetailedDesc;
        data_row[data_row.length]  = "SourceOfFunds~"   + this.SourceOfFunds;
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
            if (role_teller.value > 0 || role_admin.value > 0)
            {
                this.commons.to_clear_screen("tblPNBOccupation"); 
                this.commons.to_read_only("tblPNBOccupation", false); 
            }
            else
            {
                this.commons.to_clear_screen("tblOccupation");
                this.commons.to_read_only("tblOccupation", false);
            }
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
            if (role_teller.value > 0 || role_admin.value > 0) 
            {
                this.commons.to_read_only("tblPNBOccupation", false);
            }
            else
            {
                this.commons.to_read_only("tblOccupation", false);
            } 
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
	    iOcc.load_record();
        
        if (typeof(iOcc.load_grid_viewer) != 'undefined')
        {
            iOcc.load_grid_viewer();
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
        return;        
    }
    
    this.set_branchdef_values = function()
    {
        var ds = IRSWorld.CIF.cifmain.get_branchdef_values(document.getElementById("txtHiddenUserLocation").value).value;
        
        if(ds.Tables[0].Rows.length > 0)
        {
            this.Occupation = ds.Tables[0].Rows[0].Occupation;
            this.WorkerType = ds.Tables[0].Rows[0].WorkerType;
            
            this.commons = new common_method();
            this.commons.to_set_dropdownlist_index("ddWorkerType",this.WorkerType);
            this.commons.to_set_dropdownlist_index("ddOccupation",this.Occupation);
            this.commons = null;
        }
    }
    
    this.validate_input = function()
    { 
        this.generic = new generic_method();       
        this.error_input = '';
        var foc_elem      = '';
        
        if (role_teller.value > 0 || role_admin.value > 0)
        {
            if (this.generic.is_alphanumeric2(document.getElementById("txtPNBPosition").value) == false)
            {           
                this.error_input += "Position contains invalid characters.\n";  
                foc_elem = 'txtPNBPosition';          
            }
            if (this.generic.is_alphanumeric2(document.getElementById("txtPNBDivision").value) == false)
            {           
                this.error_input += "Division contains invalid characters.\n";
                if (foc_elem.length == 0) foc_elem = 'txtPNBDivision';    
            }
            if (this.generic.is_alphanumeric2(document.getElementById("txtPNBSector").value) == false)
            {           
                this.error_input += "Sector contains invalid characters.\n";  
                if (foc_elem.length == 0) foc_elem = 'txtPNBSector';              
            }
            if (this.generic.is_alphanumeric2(document.getElementById("txtPNBSupervisor").value) == false)
            {           
                this.error_input += "Supervisor contains invalid characters.\n";            
                if (foc_elem.length == 0) foc_elem = 'txtPNBSupervisor';
            }
        }
        else
        {
            /*
            if(document.getElementById("ddOccupation").value == 'null')
            {
                this.error_input += "Occupation required.\n"; 
                if (foc_elem.length == 0) foc_elem = 'ddOccupation';    
            }            
            if(this.generic.is_empty(document.getElementById("txtSalary").value) == true)
            {
                this.error_input += "Salary required.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtSalary';     
            }             
            if (this.generic.is_alphanumeric2(document.getElementById("txtPosition").value) == false)
            {           
                this.error_input += "Position contains invalid characters.\n";  
                foc_elem = 'txtPosition'; 
            }        
            if (this.generic.is_alphanumeric2(document.getElementById("txtEmployer").value) == false)
            {           
                this.error_input += "Employer contains invalid characters.\n";
                if (foc_elem.length == 0) foc_elem = 'txtEmployer';      
            }
            */
        }
        
        var source = document.getElementById('ddSourceOfFunds').value;
	    var sal = document.getElementById('txtSalary').value;
    	    
        if (sal != null && sal != 'null' && sal != '' && parseFloat(sal) != 0)
        {
            if(source == '' | source == undefined | source == 'null' | source == null)
	        {
                this.error_input += "Source of fund should not be blank if Annual Salary is not blank and not equal to zero.\n";	                
                if (foc_elem.length == 0) foc_elem = 'ddSourceOfFunds'; 
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