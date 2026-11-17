
function execute_ajaxgridview_command(param)
{
    if (BatchMaint != null && BatchMaint != "undefined")
    {
        BatchMaint.do_ajaxgridview(param);
    }
}

function BatchMaint()
{
    this.commons                 = null;
    this.generic                 = null;
    this.SEQ_ID                  = null;
    this.FileType                = null;
    this.FileFormat              = null;
    this.TargetTable             = null;
    this.TargetField             = null;
    this.SourceField             = null;
    this.CIFClass                = null;
    this.ajax_grid               = null;
    this.grid                    = null;   
    
    this.load_grid_viewer = function()
    {     
        this.commons = new common_method();  
        this.commons.to_read_only("tblBatchMaint",true);                      
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;    
        this.grid.sql_source     = "sp_ibatch_filemapper_filter~";
        this.grid.div_container  = document.getElementById("BatchMaintGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();        
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_ibatch_filemapper_filter~";
        this.grid.div_container  = document.getElementById("BatchMaintGrid");
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
    
    
    this.load_BatchMaint = function()
    {
        document.getElementById("ddlFileType").value    = this.FileType;
        document.getElementById("ddlFileFormat").value  = this.FileFormat;
        document.getElementById("ddlTargetTable").value = this.TargetTable;
        document.getElementById("ddlTargetField").value = this.TargetField;
        document.getelementById("ddlSourceField").value = this.SourceField;
        document.getelementById("ddlCIFClass").value    = this.CIFClass;  
    }
    
    this.set_MaintBatch = function()
    {
        this.commons = new common_method();
        this.FileType                = document.getElementById("ddlFileType").value;
        this.FileFormat              = document.getElementById("ddlFileFormat").value;
        this.TargetTable             = document.getElementById("ddlTargetTable").value;
        this.TargetField             = document.getElementById("ddlTargetField").value;
        this.SourceField             = document.getElementById("ddlSourceField").value;
        this.CIFClass                = document.getElementById("ddlCIFClass").value;
        this.commons                 = null;
        
        return this.push_data_to_server();
    } 
    this.pull_server_data = function()
    {                 
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.GRSMaintenance.IBatchMaintenance.BATCHMAINT_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      
                  
        IRSWorld.GRSMaintenance.IBatchMaintenance.GET_BATCHMAINT_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }
    
     this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                this.FileType         = ds.Tables[0].Rows[0].FileType;
                this.FileFormat       = ds.Tables[0].Rows[0].FileFormat;
                this.TargetTable      = ds.Tables[0].Rows[0].TargetTable;
                this.TargetField      = ds.Tables[0].Rows[0].TargetField;
                this.SourceField      = ds.Tables[0].Rows[0].SourceField;
                this.CIFClass         = ds.Tables[0].Rows[0].CIFClass;

                document.getElementById("txtHiddenSeqID").value         = this.SEQ_ID;
                document.getElementById("ddlFileType").value            = this.FileType;
                document.getElementById("ddlFileFormat").value          = this.FileFormat;
                document.getElementById("ddlTargetTable").value         = this.TargetTable;
                document.getElementById("ddlTargetField").value         = this.TargetField;
                document.getElementById("ddlSourceField").value         = this.SourceField;
                document.getElementById("ddlCIFClass").value            = this.CIFClass;                           
                //document.getElementById("txtHiddenIsEditable").value = this.IsEditable;      
            }
        }          
    }
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "FileType~"        + this.FileType;
        data_row[data_row.length]  = "FileFormat~"      + this.FileFormat;
        data_row[data_row.length]  = "TargetTable~"     + this.TargetTable;
        data_row[data_row.length]  = "TargetField~"     + this.TargetField;
        data_row[data_row.length]  = "SourceField~"     + this.SourceField;
        data_row[data_row.length]  = "CIFCLass~"        + this.CIFClass;                
        
        return data_row.join("|");
    }
    
    this.insert_record = function()
    {      
        document.getElementById("txtHiddenSeqId").value = 0; 
        if (document.getElementById("ddFilterBy").value == 'null' ||document.getElementById("ddFilterBy").value == "" ||document.getElementById("ddFilterBy").value == 'undefined')
        {
            this.clear();
	        //document.getElementById("ddlTargetTable").disabled = false;
        }

        else 
        {
	        document.getElementById("ddlTargetTable").value = document.getElementById("ddFilterBy").value;
	        //document.getElementById("txtGroupname").disabled = true;
	        this.insert_fldvalue();
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
            this.commons.to_read_only("tblBatchMaintField",false);                          
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel~btnDelete");
            this.commons = null;
        }
    } 
    
    this.update_record = function()
    {     
        document.getElementById("txtHiddenSeqId").value = '';
        //document.getElementById("txtCodeValue").readOnly   = true;  
       // document.getElementById("txtGroupName").readOnly   = true;  
        
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
        document.getElementById("txtHiddenSeqId").value = '';
        //document.getElementById("txtCodeValue").readOnly   = true;  
       // document.getElementById("txtGroupName").readOnly   = true;  
        
         if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.GRSMaintenance.IBatchMaintenance.delete_record(this.SEQ_ID, this.delete_record_callback);    
    }     
        
    this.delete_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSMaintenance.IBatchMaintenance.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblBatchMaintField",false);                      
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel"); 
            this.commons = null;                        
        }
    }   
    this.commit_record = function()
    {
        //add validations
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

            this.SEQ_ID = IRSWorld.GRSMaintenance.IBatchMaintenance.BATCHMAINT_SEQ_ID().value;            
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete~btnSearch"); 
            
            BatchMaint.load_grid_viewer();
            execute_ajaxgridview_command('ajx_search$' + this.commons.to_left_trim(document.getElementById("ddlTargetTable").value));
            BatchMaint.set_page_index(document.getElementById("txtHiddenPage").value);
            this.commons = null;
            
        }
    } 

     this.rollback_record = function()
    {        
        //document.getElementById("txtHiddenIsEditable").value == 'null'
        this.clear();
        this.dropdown_lookup_filter();
        IRSWorld.GRSMaintenace.BatchMaintenace.rollback_record(this.rollback_record_callback);
    }
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GRSMaintenance.IBatchMaintenance.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }        
        this.commons = new common_method();
        this.commons.to_read_only("tblBatchMaintField",true);         
        this.commons.to_unlock_button("tblbuttons","btnAdd~");
        this.commons = null;
        return;
    }
         
    this.clear = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("tblBatchMaintField");
        this.commons = null;
    }  
    this.load_all_dropdownlist = function()
    {       
        this.dropdown_batchmaint_filter();
    } 
    this.dropdown_batchmaint_filter = function()
    {
        IRSWorld.GRSMaintenance.IBatchMaintenance.getbatchmaint_filter(this.dropdown_batchmaint_filter_callback)
    }
    
       this.dropdown_batchmaint_filter_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            var commons = new common_method();         
            var ds = result.value;
            
            if (commons != null && commons != "undefined")
            {
                commons.to_load_dropdownlist("ddFilterBy", ds, "TargetTable", "TargetTable"); // to edit
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