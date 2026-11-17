
function execute_ajaxgridview_command(param)
{
    if (look_up != null && look_up != "undefined")
    {
        look_up.do_ajaxgridview(param);
    }
}

function look_up()
{
    this.commons                 = null;
    this.generic                 = null;
    this.SEQ_ID                  = null;
    /*
    this.FldGroup                = null;
    this.FldCode                 = null;
    this.FldValue                = null;
    this.FldDescription          = null;
    */
    this.ddclass                 = null;
    this.type                    = null;
    this.targetfield             = null;
    this.sourcefield             = null;
    this.fileformat              = null;
    this.sqlcmd                  = null;
    this.targettable             = null;
    
    this.IsEditable              = null;
    this.message                 = null;
    this.customer_info           = null;    
    this.ajax_grid               = null;
    this.grid                    = null;
     
       
    this.load_grid_viewer = function()
    {     
        this.commons = new common_method();  
        this.commons.to_read_only("tblLookUpField",true);                      
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;    
        this.grid.sql_source     = "sp_ibatch_filemapper_filter~";
        this.grid.div_container  = document.getElementById("LookUpGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_ibatch_filemapper_filter~";
        this.grid.div_container  = document.getElementById("LookUpGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
   
    this.load_record = function()
    {
        this.pull_server_data();        
    } 
     
 
    this.load_look_up = function()
    {   
        document.getElementById("ddClass").value               = this.ddclass;            
        document.getElementById("txtTpye").value                = this.type;
        document.getElementById("txtTargetField").value         = this.targetfield;
        document.getElementById("txtSourceField").value         = this.sourcefield;
        document.getElementById("txtFileFormat").value          = this.fileformat;
        document.getElementById("txtSqlCmd").value              = this.sqlcmd;
        document.getElementById("txtTargetTable").value         = this.targettable;
    }
    

    this.set_look_up = function()
    {
        this.commons = new common_method();
        this.ddclass        = document.getElementById("ddClass").value;
        this.type           = document.getElementById("txtType").value;               
        this.targetfield    = document.getElementById("txtTargetField").value;        
        this.sourcefield    = document.getElementById("txtSourceField").value;         
        this.fileformat     = document.getElementById("txtFileFormat").value;         
        this.sqlcmd         = document.getElementById("txtSqlCmd").value;               
        this.targettable    = document.getElementById("txtTargetTable").value;        
        this.commons = null;
        return this.push_data_to_server();
     }     
     
            
    this.pull_server_data = function()
    {                 
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID =  IRSWorld.GRSMaintenance.IBatchMaintenance.LOOK_UP_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      
                  
        IRSWorld.GRSMaintenance.IBatchMaintenance.GET_LOOK_UP_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
    this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                this.ddclass        = ds.Tables[0].Rows[0].ddclass;
                this.type           = ds.Tables[0].Rows[0].type;
                this.targetfield    = ds.Tables[0].Rows[0].targetfield;
                this.sourcefield    = ds.Tables[0].Rows[0].sourcefield;
                this.fileformat     = ds.Tables[0].Rows[0].fileformat;
                this.sqlcmd         = ds.Tables[0].Rows[0].sqlcmd;
                this.targettable    = ds.Tables[0].Rows[0].targettable;          
                this.FldDescription = ds.Tables[0].Rows[0].FldDescription;
               // this.IsEditable     = ds.Tables[0].Rows[0].IsEditable;
                
                document.getElementById("txtHiddenSeqID").value      = this.SEQ_ID;
                docuemnt.getElementById("ddClass").value             = this.ddclass;
                document.getElementById("txtType").value             = this.type;           
                document.getElementById("txtTargetField").value      = this.targetfield;    
                document.getElementById("txtSourceField").value      = this.sourcefield;    
                document.getElementById("txtFileFormat").value       = this.fileformat;     
                document.getElementById("txtSqlCmd").value           = this.sqlcmd;         
                document.getElementById("txtTargeTable").value       = this.targettable;    
               // document.getElementById("txtHiddenIsEditable").value = this.IsEditable;      
            }
        }          
    }
       
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "class~"               + this.ddclass;                      
        data_row[data_row.length]  = "type~"                + this.type;
        data_row[data_row.length]  = "targetfield~"         + this.targetfield;
        data_row[data_row.length]  = "sourcefield~"         + this.sourcefield;
        data_row[data_row.length]  = "fileformat~"          + this.fileformat;    
        data_row[data_row.length]  = "sqlcmd~"              + this.sqlcmd;
        data_row[data_row.length]  = "targettable~"         + this.targettable;      
        
        return data_row.join("|");
    }               
        
    this.insert_record = function()
    {      
        document.getElementById("txtHiddenSeqId").value = 0; 
        document.getElementById("txtHiddenIsEditable").value = "";
        if (document.getElementById("ddFilterBy").value == 'null' ||document.getElementById("ddFilterBy").value == "" ||document.getElementById("ddFilterBy").value == 'undefined')
        {
            this.clear();
	        document.getElementById("txtTargetTable").disabled = false;
	    }

        else 
        {
	        document.getElementById("txtTargetTable").value = document.getElementById("ddFilterBy").value;
	        document.getElementById("txtTargetTable").disabled = true;
	        //this.insert_fldvalue();
	    }
            IRSWorld.GRSMaintenance.IBatchMaintenance.insert_record(this.insert_record_callback);
    }  
    
    this.insert_record_callback = function(result)
    {     
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSMaintenance.IBatchMaintenance.read_message(); alert(msg.value); return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblLookUpField",false);                          
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
        
        IRSWorld.GRSMaintenance.IBatchMaintenance.update_record(this.SEQ_ID, this.update_record_callback);    
    }     
        
    this.update_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSMaintenance.IBatchMaintenance.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblLookUpField",false);                      
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel"); 
            this.commons = null;                        
        }
    }   
    
    this.delete_record = function()
    {    
        alert("NOT YET IMPLEMENTED"); //Look_Up.delete_record();
    }       

    this.commit_record = function()
    {
        this.targetfield  = document.getElementById("txtTargetField").value;   
        this.sourcefield  = document.getElementById("txtSourceField").value;
       
        var rs = null;
        rs = IRSWorld.GRSMaintenance.IBatchMaintenance.check_lookupvalue(this.targetfield,this.sourcefield).value;
        
        this.commons     = new common_method();  
        this.generic     = new generic_method();    
        if (document.getElementById("txtHiddenIsEditable").value == "true")
        {
           alert("This Field Group is Uneditable (Target Table)");
           document.getElementById("txtTargetTable").focus();
           return false;
        }  
        if (document.getElementById("txtTargetTable").value == "" ||this.generic.is_empty(document.getElementById("txtTargetTable").value) == true)
        {
           alert("Please Complete All Necessary Fields (Target Table)");
           document.getElementById("txtTargetTable").focus();
           return false;
        }
        if(document.getElementById("txtTargetField").value == ""||this.generic.is_empty(document.getElementById("txtTargetField").value) == true)
        {
           alert("Please Complete All Necessary Fields (Target Field)");
           document.getElementById("txtTargetField").focus();
           return false;
        }
        if(document.getElementById("txtSourceField").value == ""||this.generic.is_empty(document.getElementById("txtSourceField").value) == true)
        {
           alert("Please Complete All Necessary Fields (Source Field)");
           document.getElementById("txtSourceField").focus();
           return false;
        }
        /*
        if (this.generic.is_alphanumeric(document.getElementById("txtTargetField").value) == false)
        {
            alert("Code Value Contains invalid characters.");
            document.getElementById("txtTargetField").focus();
            return false;       
        }
        if (this.generic.is_numeric(document.getElementById("txtSourceField").value) == false)
        {
            alert(" Code Value Contains invalid characters. ");
            document.getElementById("txtSourceField").focus();
            return false;       
        }
        */
        if ((document.getElementById("txtHiddenSeqId").value).length > 0 && document.getElementById("txtHiddenSeqId").value >= '0')
        {
            if (rs.Tables[0].Rows.length > 0)
            {
             var wee = rs.Tables[0].Rows[0].FLDVALUE;
            }
            if (typeof(wee) != 'undefined')
            {
                  alert("Code Value already exists/active.");
                  document.getElementById("txtTargetField").focus();
                  return false;
            }
        }
        if(document.getElementById("txtTargetTable").value == ""||this.generic.is_empty(document.getElementById("txtTargetTable").value) == true)
        {
           alert("Please Complete All Necessary Fields (Target Table)");
           document.getElementById("txtTargetTable").focus();
           return false;
        }
        IRSWorld.GRSMaintenance.IBatchMaintenance.commit_record(this.set_look_up(),this.commit_record_callback);   
        document.getElementById("txtHiddenSeqId").value = '';     
        this.dropdown_lookup_filter();
        this.commons = null;
        this.generic = null;
    }
        
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSMaintenance.IBatchMaintenance.read_message();alert(msg.value);return;
            }
            
            alert("RECORD COMITTED SUCCESSFULLY");
            this.SEQ_ID = IRSWorld.GRSMaintenance.IBatchMaintenance.LOOK_UP_SEQ_ID().value;
            
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete~btnSearch"); 
            this.commons = null;
            look_up.load_grid_viewer();
            execute_ajaxgridview_command('ajx_search$' + document.getElementById("txtGroupName").value);
        }
    }
    
    this.rollback_record = function()
    {        
        document.getElementById("txtHiddenIsEditable").value == 'null'
        this.clear();
        this.dropdown_lookup_filter();
        IRSWorld.GRSMaintenance.IBatchMaintenance.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GRSMaintenance.IBatchMaintenance.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }        
        this.commons = new common_method();
        this.commons.to_read_only("tblLookUpField",true);         
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;
        return;
    }
         
    this.clear = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("tblLookUpField");
        this.commons = null;
    }
    
    this.load_all_dropdownlist = function()
    {       
        this.dropdown_lookup_filter();
    } 
    
    this.dropdown_lookup_filter = function()
    {
        IRSWorld.GRSMaintenance.IBatchMaintenance.getcifclass_filter(this.dropdown_lookup_filter_callback)
    }
    
    this.dropdown_lookup_filter_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            var commons = new common_method();         
            var ds = result.value;
            
            if (commons != null && commons != "undefined")
            {
                commons.to_load_dropdownlist("ddclass", ds, "FldValue", "FldDescription");
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
//     this.insert_fldvalue = function()
//    {
//            var fld_grp = document.getElementById("txtGroupname").value;
//            var code_value1 = IRSWorld.Look_Up.Look_Up.getfld_value(fld_grp).value;
//            //alert(code_value1.Tables[0].Rows[0].FldValue);
//            if(code_value1.Tables[0].Rows[0].FldValue != null)
//            {
//	            document.getElementById("txtCodeValue").value = code_value1.Tables[0].Rows[0].FldValue;
//	        }
//	        else
//	        {
//	            document.getElementById("txtCodeValue").value = 1;
//	        }  
//    }
}