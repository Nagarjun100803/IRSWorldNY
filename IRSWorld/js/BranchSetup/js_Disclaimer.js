
function Disclaimer()
{
    this.commons                = null;
    this.SEQ_ID                 = null;
    this.RegionID               = null;
    this.disclaimer             = null;
    
    this.load = function()
    { 
        this.open_division("divBodyRegion");
        this.dropdown_region();
        document.getElementById("txtHiddenSeqID").value = "";
    }
    
    this.close_divisions = function()
    {
        document.getElementById("divBodyRegion").style.display = 'none';
        document.getElementById("divBodyDisclaimer").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
    this.dropdown_region = function()
    {
        IRSWorld.BranchSetup.Disclaimer.getdropdown_region(this.dropdown_region_callback)
    }
    
    this.dropdown_region_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlRegion", result.value, "SEQ_ID", "BranchName");
            this.commons = null;           
        }
    }
    this.check_region = function()
    {
        var jsp = new jspad('msgpad');
        var txta = document.getElementById('txtDisclaimer');
        jspad_no_colorsel = true;
        txta.innerHTML = jsp.toString();
        this.RegionID = document.getElementById("ddlRegion").value;  
        if(this.RegionID == 0)
        {
            alert("Please Select a Branch!");
        }       
        else
        {
            var result = IRSWorld.BranchSetup.Disclaimer.check_seqid(this.RegionID);
            if (result.value.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID = result.value.Tables[0].Rows[0].SEQ_ID;
                this.load_record();
            }
            else
            {
                this.insert_record();
            }
            this.open_division('divBodyDisclaimer');
        }
    }
   
    this.load_record = function()
    {                     
         this.pull_server_data();  
    }
        
    this.set_disclaimer = function()
    {   
        this.disclaimer = jspad_get_text();
        return this.push_data_to_server();
     }
     
   
    this.pull_server_data = function()
    {             
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.BranchSetup.Disclaimer.DISCLAIMER_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.BranchSetup.Disclaimer.GET_DISCLAIMER_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
     
    this.pull_server_data_callback = function(result)
    {        
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    this.disclaimer            = ds.Tables[0].Rows[0].Disclaimer;

                   
                    document.getElementById("txtHiddenSeqID").value             = this.SEQ_ID;
//                    document.getElementById('msgpad_ddl_colors').style.display = 'none';
                    jspad_set_text(this.disclaimer);
                    jspad_canedit = false;
                   this.commons = new common_method();
                   this.commons.to_read_only("tblDisclaimer", true);                     
                   this.commons.to_unlock_button("tblbuttons","btnEdit~btnCancel");
                   this.commons = null;
            }
        }
    }
    
 
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
        
        data_row[data_row.length]  = "RegionID~"          + this.RegionID;     
        data_row[data_row.length]  = "Disclaimer~"        + this.disclaimer;          

           
        return data_row.join("^");       
    }
    
     
    this.insert_record = function()
    {    
        this.add_data(); 
        IRSWorld.BranchSetup.Disclaimer.insert_record(this.insert_record_callback);
    }
      
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.Disclaimer.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblDisclaimer",false);                
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;            
        }
    }
    
    this.update_record = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("Record not found.");return;
        }
        
        IRSWorld.BranchSetup.Disclaimer.update_record(this.SEQ_ID, this.update_record_callback);
    } 
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.Disclaimer.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();
            jspad_canedit = true;  
            this.commons.to_read_only("tblDisclaimer",false);                       
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;                                  
        }
    }
    
            
     
    this.commit_record = function()
    {
        if (confirm("Are you sure you want to save this disclaimer?"))
        {
            IRSWorld.BranchSetup.Disclaimer.commit_record(this.set_disclaimer(),this.commit_record_callback);
        }

    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.Disclaimer.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.BranchSetup.Disclaimer.DISCLAIMER_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_read_only("tblDisclaimer",true);                          
            this.commons.to_unlock_button("tblbuttons","btnEdit~btnCancel"); 
            this.commons = null;    
            
            alert("Record committed successfully");
      
        }
    }
      

    this.clear = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblDisclaimer");
       this.commons = null;           
    }
    
    this.add_data = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblDisclaimer");
        this.commons.to_clear_screen("tblbuttons");
        this.commons.to_read_only("tblDisclaimer",false);
        this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
        this.commons = null;
    }
}

