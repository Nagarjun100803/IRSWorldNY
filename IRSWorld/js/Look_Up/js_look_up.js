
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
    this.FldGroup                = null;
    this.FldCode                 = null;
    this.FldValue                = null;
    this.IsEditable              = null;
    this.FldDescription          = null;
    this.message                 = null;
    this.customer_info           = null;    
    this.ajax_grid               = null;
    this.grid                    = null;
     
       
    this.load_grid_viewer = function()
    {
        /* VAPT 20190206 */
        var role = IRSWorld.Look_Up.Look_Up.getRole().value;
        this.commons = new common_method();  
        this.commons.to_read_only("tblLookUpField",true);                      
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;   
        this.grid = new grid_viewer();
        this.grid.session_end = true;    
        this.grid.sql_source = "sp_lookup2~" + role;
        this.grid.div_container  = document.getElementById("LookUpGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();        
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_lookup2~";
        this.grid.div_container  = document.getElementById("LookUpGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);        
        this.grid = null;

        if (document.getElementById("DROP_DOWN_PAGER_LookUpGrid") != null)
        {
            document.getElementById("txtHiddenPage").value = (document.getElementById("DROP_DOWN_PAGER_LookUpGrid").value);  
        }
    }
   
    this.load_record = function()
    {
        this.pull_server_data();        
    } 
     
 
    this.load_look_up = function()
    {               
        document.getElementById("txtGroupname").value        = this.FldGroup;
        document.getElementById("txtCodeName").value         = this.FldCode;
        document.getElementById("txtCodeValue").value        = this.FldValue;
        document.getElementById("txtCodeDescriptions").value = this.FldDescription;
    }
    

    this.set_look_up = function()
    {
        this.commons = new common_method();
        //this.FldGroup       = this.commons.to_left_trim(document.getElementById("txtGroupname").value); chr09302010 case-sensitivity issue
        this.FldGroup       = this.commons.to_left_trim(document.getElementById("txtGroupName").value);
        this.FldCode        = document.getElementById("txtCodeName").value;
        this.FldValue       = document.getElementById("txtCodeValue").value;
        this.FldDescription = document.getElementById("txtCodeDescriptions").value;
        this.commons = null;
        return this.push_data_to_server();
     }     
     
            
    this.pull_server_data = function()
    {                 
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.Look_Up.Look_Up.LOOK_UP_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      
                  
        IRSWorld.Look_Up.Look_Up.GET_LOOK_UP_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
    this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                this.FldGroup         = ds.Tables[0].Rows[0].FldGroup;
                this.FldCode          = ds.Tables[0].Rows[0].FldCode;
                this.FldValue         = ds.Tables[0].Rows[0].FldValue;
                this.FldDescription   = ds.Tables[0].Rows[0].FldDescription;
                this.IsEditable       = ds.Tables[0].Rows[0].IsEditable;
                
                document.getElementById("txtHiddenSeqID").value      = this.SEQ_ID;
                //document.getElementById("txtGroupname").value        = this.FldGroup; // chr09302010 case-sensitivity for firefox
                document.getElementById("txtGroupName").value        = this.FldGroup;
                document.getElementById("txtCodeName").value         = this.FldCode;
                document.getElementById("txtCodeValue").value        = this.FldValue;
                document.getElementById("txtCodeDescriptions").value = this.FldDescription;                        
                document.getElementById("txtHiddenIsEditable").value = this.IsEditable;      
            }
        }          
    }
       
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "FldGroup~"       + this.FldGroup;
        data_row[data_row.length]  = "FldCode~"        + this.FldCode;
        data_row[data_row.length]  = "FldValue~"       + this.FldValue;
        data_row[data_row.length]  = "FldDescription~" + this.FldDescription;        
        
        return data_row.join("|");
    }               
        
    this.insert_record = function()
    {      
        //document.getElementById("txtHiddenSeqId").value = 0; // chr09302010 case sensitivity issue
        document.getElementById("txtHiddenSeqID").value = 0; 
        document.getElementById("txtHiddenIsEditable").value = "";
        document.getElementById("txtCodeValue").readOnly   = true;
        document.getElementById("txtGroupName").readOnly   = false;  
        if (document.getElementById("ddFilterBy").value == 'null' ||document.getElementById("ddFilterBy").value == "" ||document.getElementById("ddFilterBy").value == 'undefined')
        {
            this.clear();
	        //document.getElementById("txtGroupname").disabled = false; --do--
	        document.getElementById("txtGroupName").disabled = false;
        }

        else 
        {
	        //document.getElementById("txtGroupname").value = document.getElementById("ddFilterBy").value; -- do --
	        document.getElementById("txtGroupName").value = document.getElementById("ddFilterBy").value;
	        //document.getElementById("txtGroupname").disabled = true; -- do --
	        document.getElementById("txtGroupName").disabled = true;
	        this.insert_fldvalue();
	    }
           
	        IRSWorld.Look_Up.Look_Up.insert_record(this.insert_record_callback);
    }  
    
    this.insert_record_callback = function(result)
    {     
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Look_Up.Look_Up.read_message(); alert(msg.value); return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblLookUpField",false);                          
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
        }
    } 
    
    this.update_record = function()
    {     
        //document.getElementById("txtHiddenSeqId").value = ''; chr09302010 - for case sensitive browsers
        document.getElementById("txtHiddenSeqID").value = '';
        document.getElementById("txtCodeValue").readOnly   = true;  
        document.getElementById("txtGroupName").readOnly   = true;  
        
         if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.Look_Up.Look_Up.update_record(this.SEQ_ID, this.update_record_callback);    
    }     
        
    this.update_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Look_Up.Look_Up.read_message();
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
        this.FLDCODE  = document.getElementById("txtCodeName").value;   
        this.FLDGROUP  = document.getElementById("txtGroupName").value;
        var rs = null;
        rs = IRSWorld.Look_Up.Look_Up.check_lookupvalue(this.FLDCODE,this.FLDGROUP).value;
        //document.getElementById("txtHiddenSeqId").value = this.SEQ_ID; 
        
        this.commons     = new common_method();  
        this.generic     = new generic_method();    
        if (document.getElementById("txtHiddenIsEditable").value == "false")
        {
           alert("This Field Group is Uneditable (Group Name)");
           document.getElementById("txtGroupName").focus();
           return false;
        }  
        if (document.getElementById("txtGroupName").value == "" ||this.generic.is_empty(document.getElementById("txtGroupName").value) == true)
        {
           alert("Please Complete All Necessary Fields (Group Name)");
           document.getElementById("txtGroupName").focus();
           return false;
        }
        if(document.getElementById("txtCodeName").value == ""||this.generic.is_empty(document.getElementById("txtCodeName").value) == true)
        {
           alert("Please Complete All Necessary Fields (Code Name)");
           document.getElementById("txtCodeName").focus();
           return false;
        }
        
        if(this.FLDCODE.indexOf('|') >-1 )
        {
           alert("Code Name Contains invalid characters.");
           document.getElementById("txtCodeName").focus();
           return false;
        }
        if(document.getElementById("txtCodeValue").value == ""||this.generic.is_empty(document.getElementById("txtCodeValue").value) == true)
        {
           alert("Please Complete All Necessary Fields (Code Value)");
           document.getElementById("txtCodeValue").focus();
           return false;
        }
        if (this.generic.is_alphanumeric(document.getElementById("txtCodeValue").value) == false)
        {
            alert("Code Value Contains invalid characters.");
            document.getElementById("txtCodeValue").focus();
            return false;       
        }
        if (this.generic.is_numeric(document.getElementById("txtCodeValue").value) == false)
        {
            alert(" Code Value Contains invalid characters. ");
            document.getElementById("txtCodeValue").focus();
            return false;       
        }
        // chr09302010 case-sensitive issue
        if ((document.getElementById("txtHiddenSeqID").value).length > 0 && document.getElementById("txtHiddenSeqID").value >= '0')
        {
            if (rs.Tables[0].Rows.length > 0)
            {
             var wee = rs.Tables[0].Rows[0].FLDCODE;
            }
            if (typeof(wee) != 'undefined')
            {
                  alert("Code Name already exists/active.");
                  document.getElementById("txtCodeName").focus();
                  return false;
            }
        }
        if(document.getElementById("txtCodeDescriptions").value == ""||this.generic.is_empty(document.getElementById("txtCodeDescriptions").value) == true)
        {
           alert("Please Complete All Necessary Fields (Code Description)");
           document.getElementById("txtCodeDescriptions").focus();
           return false;
        }
        IRSWorld.Look_Up.Look_Up.commit_record(this.set_look_up(),this.commit_record_callback);   
        //document.getElementById("txtHiddenSeqId").value = ''; chr09302010 for case-sensitive browser.     
        document.getElementById("txtHiddenSeqID").value = '';     
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
                var msg = IRSWorld.Look_Up.Look_Up.read_message();alert(msg.value);return;
            }

            /* RFC-31620 */
            var msg = IRSWorld.Look_Up.Look_Up.isAdminApproverTag().value;
            if (msg != "") {
                alert(msg);
            }
            else {
                alert("RECORD COMMITTED SUCCESSFULLY");
            }

            this.SEQ_ID = IRSWorld.Look_Up.Look_Up.LOOK_UP_SEQ_ID().value;            
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete~btnSearch"); 
            
            look_up.load_grid_viewer();
            execute_ajaxgridview_command('ajx_search$' + this.commons.to_left_trim(document.getElementById("txtGroupname").value));
            look_up.set_page_index(document.getElementById("txtHiddenPage").value);
            this.commons = null;
            
        }
    }
    
    this.rollback_record = function()
    {        
        document.getElementById("txtHiddenIsEditable").value == 'null'
        this.clear();
        this.dropdown_lookup_filter();
        IRSWorld.Look_Up.Look_Up.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.Look_Up.Look_Up.read_message();
        
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
        IRSWorld.Look_Up.Look_Up.getlookup_filter(this.dropdown_lookup_filter_callback)
    }
    
    this.dropdown_lookup_filter_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            var commons = new common_method();         
            var ds = result.value;
            
            if (commons != null && commons != "undefined")
            {
                commons.to_load_dropdownlist("ddFilterBy", ds, "FldGroup", "FldGroups");
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
                //chr09302010 - case sensitivity correction for cross browser
                //document.getElementById("txtHiddenSeqId").value = command_param[1];
                document.getElementById("txtHiddenSeqID").value = command_param[1];
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnSearch");
                    this.commons.to_read_only("tblLookUpField",true);       
                    this.commons = null;
                    return;
                }
                
                this.process_grid_viewer(parameter);
                
                
                
                
            }
        }
    }
    
    this.insert_fldvalue = function()
    {
            //var fld_grp = document.getElementById("txtGroupname").value; // chr09302010 - case sensitivity
            var fld_grp = document.getElementById("txtGroupName").value;
            var code_value1 = IRSWorld.Look_Up.Look_Up.getfld_value(fld_grp).value;
            if(code_value1.Tables[0].Rows[0].FldValue != null)
            {
	            document.getElementById("txtCodeValue").value = code_value1.Tables[0].Rows[0].FldValue;
	        }
	        else
	        {
	            document.getElementById("txtCodeValue").value = 1;
	        }
	        
    }
    
    this.set_page_index = function(index)
    {
        this.process_grid_viewer("ajx_paging$" + index);
    }
}