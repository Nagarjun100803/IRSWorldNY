

function execute_ajaxgridview_command(param)
{
    if (PUNLA_Maint != null && PUNLA_Maint != "undefined")
    {
        PUNLA_Maint.do_ajaxgridview(param);
    }
}

function PUNLA_Maint()
{
    this.commons                 = null;
    this.generic                 = null;
   
    //fileds of PUNLA
    this.SEQ_ID                  = null;
    this.Agency                  = null;
    this.RemitterName            = null;
    this.AccountNo               = null;
    this.PercentContri            = null;
    this.ClientID                = null;
    this.AmtCrPUNLA              = null;
 
    this.message                 = null;
    this.customer_info           = null;    
    this.ajax_grid               = null;
    this.grid                    = null;
     
       
    this.load_grid_viewer = function()
    {     
    
        this.commons = new common_method();  
        this.commons.to_read_only("tblMaintFields",true);                      
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;    
        this.grid.sql_source     = "sp_PUNLAviewer~"
        this.grid.div_container  = document.getElementById("PUNLAMaintGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {

        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_PUNLAviewer~"
        this.grid.div_container  = document.getElementById("PUNLAMaintGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
   
    this.load_record = function()
    {
        this.pull_server_data();        
    } 
     
 
    this.load_PUNLA_Maint = function()
    {               
        document.getElementById("txtAgency").value          = this.Agency;
        document.getElementById("txtRemitterName").value    = this.RemitterName;
        this.commons.to_right_trim(document.getElementById("txtAccountNo").value)       = this.AccountNo;
        document.getElementById("txtPercentContri").value   = this.PercentContri;
        document.getElementById("txtClientID").value        = this.ClientID;
        document.getElementById("txtAmtCrPUNLA").value      = this.AmtCrPUNLA;
    }
    

    this.set_PUNLA_Maint = function()
    {
        this.commons = new common_method();
        this.Agency         = this.commons.to_left_trim(document.getElementById("txtAgency").value);
        this.RemitterName   = document.getElementById("txtRemitterName").value;
        this.AccountNo      = this.commons.to_right_trim(document.getElementById("txtAccountNo").value);
        this.PercentContri  = document.getElementById("txtPercentContri").value;
        this.ClientID       = document.getElementById("txtClientID").value;
        this.AmtCrPUNLA     = document.getElementById("txtAmtCrPUNLA").value;
        this.commons = null;
        return this.push_data_to_server();
     }     
     
            
    this.pull_server_data = function()
    {                 
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.GRSMaintenance.PUNLAMaintenance.PUNLA_Maint_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      
                  
        IRSWorld.GRSMaintenance.PUNLAMaintenance.GET_PUNLA_Maint_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
    this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                //this.SEQ_ID          = ds.Tables[0].Rows[0].PUNLAID;
                this.Agency          = ds.Tables[0].Rows[0].Agency;
                this.RemitterName    = ds.Tables[0].Rows[0].RemitterName;
                this.AccountNo       = ds.Tables[0].Rows[0].AccountNo;
                this.PercentContri   = ds.Tables[0].Rows[0].PercentContri;
                this.ClientID         = ds.Tables[0].Rows[0].ClientID;
                this.AmtCrPUNLA      = ds.Tables[0].Rows[0].AmtCrPUNLA;

                document.getElementById("txtHiddenSeqID").value         = this.SEQ_ID;
                document.getElementById("txtAgency").value              = this.Agency;
                document.getElementById("txtRemitterName").value        = this.RemitterName;
                document.getElementById("txtAccountNo").value           = this.AccountNo;
                document.getElementById("txtPercentContri").value       = this.PercentContri;
                document.getElementById("txtClientID").value            = this.ClientID;   
                document.getElementById("txtAmtCrPUNLA").value          = this.AmtCrPUNLA;                     
 
            }
        }          
    }
       
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "Agency~"          + this.Agency;
        data_row[data_row.length]  = "RemitterName~"    + this.RemitterName;
        data_row[data_row.length]  = "AccountNo~"       + this.AccountNo;
        data_row[data_row.length]  = "PercentContri~"   + this.PercentContri;        
        data_row[data_row.length]  = "ClientID~"        + this.ClientID;
        data_row[data_row.length]  = "AmtCrPUNLA~"      + this.AmtCrPUNLA;      
        
        return data_row.join("|");
    }               
        
    this.insert_record = function()
    {      
        document.getElementById("txtHiddenSeqId").value = 0; 
        if (document.getElementById("ddFilterBy").value == 'null' ||document.getElementById("ddFilterBy").value == "" ||document.getElementById("ddFilterBy").value == 'undefined')
        {
            this.clear();
	        document.getElementById("txtAgency").disabled = false;
        }

        else 
        {
	        document.getElementById("txtAgency").value = document.getElementById("ddFilterBy").value;
	        document.getElementById("txtAgency").disabled = true;
	    }
           
	        IRSWorld.GRSMaintenance.PUNLAMaintenance.insert_record(this.insert_record_callback);
    }  
    
    this.insert_record_callback = function(result)
    {     
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSMaintenance.PUNLAMaintenance.read_message(); alert(msg.value); return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblMaintFields",false);                          
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
        }
    } 
    
    this.update_record = function()
    {     
        document.getElementById("txtHiddenSeqId").value = '';
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.GRSMaintenance.PUNLAMaintenance.update_record(this.SEQ_ID, this.update_record_callback);    
    }     
        
    this.update_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSMaintenance.PUNLAMaintenance.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblMaintFields",false);                      
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel"); 
            this.commons = null;                        
        }
    }   
    
    this.delete_record = function()
    {    
        alert("NOT YET IMPLEMENTED"); //PUNLA_Maint.delete_record();
    }       

    this.commit_record = function()
    {
        this.AccountNo  = document.getElementById("txtAccountNo").value;   
        var rs = null;
        rs = IRSWorld.GRSMaintenance.PUNLAMaintenance.check_PUNLAMaintvalue(this.AccountNo).value;
       
        this.commons     = new common_method();  
        this.generic     = new generic_method();    
  
        if (document.getElementById("txtAgency").value == "" ||this.generic.is_empty(document.getElementById("txtAgency").value) == true)
        {
           alert("Please Complete All Necessary Fields (Agency)");
           document.getElementById("txtAgency").focus();
           return false;
        }
        if(document.getElementById("txtRemitterName").value == ""||this.generic.is_empty(document.getElementById("txtRemitterName").value) == true)
        {
           alert("Please Complete All Necessary Fields (Remitter Name)");
           document.getElementById("txtRemitterName").focus();
           return false;
        }
        if(document.getElementById("txtClientID").value == ""||this.generic.is_empty(document.getElementById("txtClientID").value) == true)
        {
           alert("Please Complete All Necessary Fields (Client ID)");
           document.getElementById("txtClientID").focus();
           return false;
        }
        if (this.generic.is_numeric(document.getElementById("txtAccountNo").value) == false)
        {
            alert("Account No contains invalid characters.");
            document.getElementById("txtAccountNo").focus();
            return false;       
        }
        if (this.generic.is_numeric(document.getElementById("txtAmtCrPUNLA").value) == false)
        {
            alert(" Amount Credit to PUNLA contains invalid characters. ");
            document.getElementById("txtAmtCrPUNLA").focus();
            return false;       
        }
        if (this.generic.is_numeric(document.getElementById("txtPercentContri").value) == false)
        {
            alert(" Amount Credit to PUNLA contains invalid characters. ");
            document.getElementById("txtPercentContri").focus();
            return false;       
        }
        if ((document.getElementById("txtHiddenSeqId").value).length > 0 && document.getElementById("txtHiddenSeqId").value >= '0')
        {
            if (rs.Tables[0].Rows.length > 0)
            {
             var wee = rs.Tables[0].Rows[0].AccountNo;
            }
            if (typeof(wee) != 'undefined')
            {
                  alert("Account No already exists/active.");
                  document.getElementById("txtAccountNo").focus();
                  return false;
            }
        }

        IRSWorld.GRSMaintenance.PUNLAMaintenance.commit_record(this.set_PUNLA_Maint(),this.commit_record_callback);   
        document.getElementById("txtHiddenSeqId").value = '';     
        this.dropdown_PUNLAMaint_filter();
        this.commons = null;
        this.generic = null;
    }
        
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSMaintenance.PUNLAMaintenance.read_message();alert(msg.value);return;
            }
            
            alert("RECORD COMITTED SUCCESSFULLY");
            this.SEQ_ID = IRSWorld.GRSMaintenance.PUNLAMaintenance.PUNLA_SEQ_ID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete~btnSearch"); 
            PUNLA_Maint.load_grid_viewer();
            execute_ajaxgridview_command('ajx_search$' + this.commons.to_right_trim(document.getElementById("txtAgency").value));
            this.commons = null;
        }
    }
    
    this.rollback_record = function()
    {        
        this.clear();
        this.dropdown_PUNLAMaint_filter();
        IRSWorld.GRSMaintenance.PUNLAMaintenance.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GRSMaintenance.PUNLAMaintenance.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }        
        this.commons = new common_method();
        this.commons.to_read_only("tblMaintFields",true);         
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;
        return;
    }
         
    this.clear = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("tblMaintFields");
        this.commons = null;
    }
    
    this.load_all_dropdownlist = function()
    {       
        this.dropdown_PUNLAMaint_filter();
    } 
    
    this.dropdown_PUNLAMaint_filter = function()
    {
        IRSWorld.GRSMaintenance.PUNLAMaintenance.getPUNLAMaint_filter(this.dropdown_PUNLAMaint_filter_callback)
    }
    
    this.dropdown_PUNLAMaint_filter_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            var commons = new common_method();         
            var ds = result.value;
            
            if (commons != null && commons != "undefined")
            {
                commons.to_load_dropdownlist("ddFilterBy", ds, "Agency", "Agencys");
            }            
        }   
    }
      
    this.do_ajaxgridview = function(parameter)
    {                        
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqId").value = command_param[1];
            
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnSearch");
                    this.commons = null;
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
 }