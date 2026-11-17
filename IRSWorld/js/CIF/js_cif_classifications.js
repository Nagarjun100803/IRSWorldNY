function cif_classifications()
{
    this.CIFID       = null;
    this.CIFClass    = null;
    this.SEQ_ID      = null; 
    this.grid        = null;    
    this.active_grid = null;
    var iClass       = this;
    
    this.call_server_side_message = function()
    {
        IRSWorld.CIF.cifmain.read_message(this.call_server_side_message_callback);
    }
    
    this.call_server_side_message_callback = function()
    {
        this.server_side_msg = null;
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
     
    this.do_class = function(param)
    {
        if(document.getElementById("txtHiddenCIFID").value == '')
        {
            alert("Add personal information first!");
            return;
        }
        
        if(this.active_grid == "GRIDCLASSADD")
        {
            var class_desc = '';
            var ds = IRSWorld.CIF.cifmain.get_cif_classdesc(param).value;
            
            if(ds.Tables[0].Rows.length > 0)
            {
                class_desc = ds.Tables[0].Rows[0].FldDescription; 
            }
            
            if (document.getElementById("txtHiddenUserCIFID").value != document.getElementById("txtHiddenCIFID").value) { //DMP RFC-20327
                if (confirm("Add " + class_desc + " class?") == true)
                {
                    var class_count = IRSWorld.CIF.cifmain.get_cifclass_count(document.getElementById("txtHiddenMasterSeqId").value, param).value;
                    if (class_count == 0)
                    {
                        this.insert_record();
                
                        this.commit_record();
                        this.load_grid_viewer_add();
                        this.load_grid_viewer_remove();
                    }
                }
            }
        }
        else
        {
            var class_desc2 = '';
            var ds2 = IRSWorld.CIF.cifmain.get_cif_classdesc2(param).value;
            
            if(ds2.Tables[0].Rows.length > 0)
            {
                class_desc2 = ds2.Tables[0].Rows[0].FldDescription; 
            }
            
            var cclass = IRSWorld.CIF.cifmain.count_class(document.getElementById('txtHiddenCIFID').value).value; 
            
            if(Number(cclass) > 1)
            {
                if (document.getElementById("txtHiddenUserCIFID").value != document.getElementById("txtHiddenCIFID").value) { //DMP RFC-20327
                    if (confirm("Delete " + class_desc2 + " class?") == true) {
                        this.delete_record();
                        this.load_grid_viewer_add();
                        this.load_grid_viewer_remove();
                    }
                }
            }
            else
            {
                alert("Deletion of CIF Class not allowed, client has only one CIF Class.");
            }
        }
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
                iClass.call_server_side_message(); 
                
                if (iClass.server_side_msg != null && iClass.server_side_msg != "")
                {
                    alert("ERROR ENCOUNTERED WHILE DELETING. PLEASE TRY AGAIN ...\n\n" + iClass.server_side_msg); 
                    this.value = iClass; return;
                }
            }
        }        
    }
        
    this.commit_record = function()
    {
        IRSWorld.CIF.cifmain.commit_record(this.set_record(), this.commit_record_callback );        
    }
    
    this.commit_record_callback = function(result)
    {                        
        if (result.error != null && result.error != "undefined")
        {            
            alert("UNABLE TO COMMIT RECORD. " + result.error); return;
        }
                                
        if (result != null && result.value != "undefined")
        {                                
            if (result.value == false)
            {     
                iClass.call_server_side_message();
                                
                if (iClass.server_side_msg != null && iClass.server_side_msg != "")
                {
                    alert("ERROR ENCOUNTERED WHILE SAVING. PLEASE TRY AGAIN ...\n\n" + iClass.server_side_msg); 
                    this.value = iClass; return;
                }               
            }            
            alert("RECORD COMMITTED SUCCESSFULLY");
         }
    }
    
    this.delete_record = function()
    {
        IRSWorld.CIF.cifmain.delete_record(document.getElementById("txtHiddenSeqId").value, this.delete_record_callback);
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
                iClass.call_server_side_message(); 
                
                if (iClass.server_side_msg != null && iClass.server_side_msg != "")
                {
                    alert("ERROR ENCOUNTERED WHILE DELETING. PLEASE TRY AGAIN ...\n\n" + iClass.server_side_msg); 
                    this.value = iClass; return;
                }
            }
            return true;
        }     
    }
    
    this.set_record = function()
    {                                
        return this.push_data_to_server();        
    }    
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"    + this.CIFID;
        data_row[data_row.length]  = "CIFClass~" + this.CIFClass;
        data_row[data_row.length]  = "Deleted~false";
        
        return data_row.join("|");        
    }
    
    this.load_grid_viewer_add = function()
    { 
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_availbleclass~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFMASTERGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.load_grid_viewer_remove = function()
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_currentclass~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFGRIDCURRENTCLASS");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.session_end = true;

        if (this.active_grid == "GRIDCLASSREMOVE")
        {
            this.grid.sql_source     = "sp_cif_grid_currentclass~" + this.CIFID;
            this.grid.div_container  = document.getElementById("CIFGRIDCURRENTCLASS");
            this.grid.datarow_column = 0;
            this.grid.page_size      = 10;     
        }

        if (this.active_grid  == "GRIDCLASSADD")
        {
            this.grid.sql_source     = "sp_cif_grid_availbleclass~" + this.CIFID;
            this.grid.div_container  = document.getElementById("CIFMASTERGRID");
            this.grid.datarow_column = 0;
            this.grid.page_size      = 10;
        }

        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
}
