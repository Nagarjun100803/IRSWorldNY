
function branch()
{
    this.                  = null;
    this.                = null;
    this.                 = null;
    this.                = null;
    this.FldDescription          = null;
 
    this.message                 = null;
    this.common_method          = new common_method();
     
     
    this.load_record = function()
    {            
         this.SEQ_ID = "1";           
         this.pull_server_data();        
    }
       
     this.load_branch = function()
    {
        document.getElementById("").value                       = this.SEQ_ID;
        document.getElementById("").value           = this.FldGroup;
        document.getElementById("").value            = this.FldCode;
        document.getElementById("").value           = this.FldValue;
        document.getElementById("txtCodeDescriptions").value    = this.FldDescription;
    }
    
    this.set_branch = function()
    {
        this.FldGroup           = document.getElementById("").value;
        this.FldCode            = document.getElementById("").value;
        this.FldValue           = document.getElementById("").value;
        this.FldDescription     = document.getElementById("txtCodeDescriptions").value;
        
        this.push_data_to_server();
     }
            
      this.pull_server_data = function()
    {        
        alert (" testing1");
        Branch.GET_BRANCH_RECORD(this.SEQ_ID,this.pull_server_data_callback);
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
               
                
               document.getElementById("").value           = this.FldGroup;
               document.getElementById("").value            = this.FldCode;
               document.getElementById("").value           = this.FldValue;
               document.getElementById("").value    = this.FldDescription;
        
                
                                
            }
        }          
    }
          
    this.push_data_to_server = function()    
    {    
        
        
        var data_row = new Array();
                               
        data_row[data_row.length]  = "FldGroup~"        + this.FldGroup;
        data_row[data_row.length]  = "FldCode~"         + this.FldCode;
        data_row[data_row.length]  = "FldValue~"        + this.FldValue;
        data_row[data_row.length]  = "FldDescription~"  + this.FldDescription;
                
        var result = Branch.SET_BRANCH_RECORD(data_row.join("|"));
                                                              
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {                                
                var mgs = Branch.read_message();
                
                if (mgs != null && mgs.value != "undefined")
                {                    
                    this.set_message(mgs); return false;
                }                
            }            
            return true;
        }                
        this.set_message("UNABLE TO SET EXECUTE METHOD: push_data_to_server()."); return false;        
    }
        
    
    this.pull_data_from_server = function()
    {
        return true;
    }    
        
    this.insert_record = function()
    {         
        var result = Branch.insert_record();
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                this.set_message(Branch.read_message()); return false;
            }
            
            this.common_method.to_unlock_button("tblbuttons","btnSave~btnCancel");
            
            return true;
        }        
        this.set_message("UNABLE TO INSERT RECORD."); return false;
    }  
    
     this.update_record = function()
    {
        var result = Branch.update_record(this.SEQ_ID);
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                this.set_message(Branch.read_message()); return false;
            }
            
            this.common_method.to_unlock_button("tblbuttons","btnSave~btnCancel");
            
            return true;
        }
        this.set_message("UNABLE TO UPDATE RECORD."); return false;        
    }    
    
    this.delete_record = function()
    {    
        Branch.delete_record();
    }    
        
    this.commit_record = function()
    {             
        
        
        this.set_branch();
    
        var result = Branch.commit_record();
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                this.set_message(Branch.read_message()); return false;
            }
            
            this.common_method.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");
            return true;
        }
        
        this.set_message("UNABLE TO COMMIT CHANGES IN RECORD."); return false;        
    }
    
    this.rollback_record = function()
    {        
        Branch.rollback_record();  
        this.common_method.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");
    }
     
    this.get_message = function()
    {    
        if (this.message == null || this.message == "undefined")
        {            
            return "";
        }
        else
        {        
            return this.message;
        }
    }
    
    this.set_message = function(msg)
    {        
        this.message = null;
        this.message = msg;
    }
    this.clear = function()
    {
           this.common_method.to_clear_screen("tblLookUpField");
    }
     
}