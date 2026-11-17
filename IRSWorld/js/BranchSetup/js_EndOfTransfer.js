function execute_ajaxgridview_command(param)
{
    if (eot != null && eot != "undefined")
    {
        eot.do_ajaxgridview(param);
    }
}

function eot()
{
    this.commons                 = null;
    this.generic                 = null;
    this.SEQ_ID                  = null;
    this.CutOff                  = null;
    this.Time                    = null;
    this.message                 = null;
    this.ajax_grid               = null;
    this.grid                    = null;
    
    this.load_grid_viewer = function()
    {     
        this.commons = new common_method();  
        this.commons.to_read_only("tblEOTField",true);                      
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;      
        this.grid.sql_source     = "sp_eot~";
        this.grid.div_container  = document.getElementById("EOTGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_eot~";
        this.grid.div_container  = document.getElementById("EOTGrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.load_record = function()
    {
        this.pull_server_data();        
    } 
    
    this.set_end_of_transfer = function()
    {
        this.CutOff        = document.getElementById("ddCutOff").value;
        this.Time          = document.getElementById("ddTime").value;
       
        return this.push_data_to_server();
     }   
     
    this.pull_server_data = function()
    {                 
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.BranchSetup.EndOfTransfer.ENDOFTRANSFER_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      
                  
        IRSWorld.BranchSetup.EndOfTransfer.GET_ENDOFTRANSFER_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
    this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                
                this.CutOff         = ds.Tables[0].Rows[0].CutOff;
                this.Time           = ds.Tables[0].Rows[0].Time;
                
                document.getElementById("txtHiddenSeqID").value     = this.SEQ_ID;
                document.getElementById("ddCutOff").value           = this.CutOff;
                if (this.Time == '00:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '0';
                }
                if (this.Time == '01:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '1';
                }
                if (this.Time == '02:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '2';
                }
                if (this.Time == '03:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '3';
                }
                if (this.Time == '04:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '4';
                }
                if (this.Time == '05:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '5';
                }
                if (this.Time == '06:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '6';
                }
                if (this.Time == '07:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '7';
                }
                if (this.Time == '08:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '8';
                }
                if (this.Time == '09:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '9';
                }
                if (this.Time == '10:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '10';
                }
                if (this.Time == '11:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '11';
                }
                if (this.Time == '12:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '12';
                }
                if (this.Time == '13:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '13';
                }
                if (this.Time == '14:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '14';
                }
                if (this.Time == '15:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '15';
                }
                if (this.Time == '16:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '16';
                }
                if (this.Time == '17:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '17';
                }
                if (this.Time == '18:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '18';
                }
                if (this.Time == '19:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '19';
                }
                if (this.Time == '20:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '20';
                }
                if (this.Time == '21:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '21';
                }
                if (this.Time == '22:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '22';
                }
                if (this.Time == '23:00:00')
                {
                    document.getElementById("ddTime").selectedIndex = '23';
                }
                //document.getElementById("ddTime").value            = this.Time;
                
            }
        }
        
     }
     
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "CutOff~"       + this.CutOff;
        data_row[data_row.length]  = "Time~"         + this.Time;
        
        return data_row.join("|");
    } 
    
    this.insert_record = function()
    {     
        this.clear();
        document.getElementById("txtHiddenSeqID").value = 0;
        IRSWorld.BranchSetup.EndOfTransfer.insert_record(this.insert_record_callback);
    }  
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.EndOfTransfer.read_message(); alert(msg.value); return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblEOTField",false);                          
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
        }
    }   
    
    this.update_record = function()
    {     
         document.getElementById("txtHiddenSeqID").value = '';
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.BranchSetup.EndOfTransfer.update_record(this.SEQ_ID, this.update_record_callback);    
    }     
        
    this.update_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.EndOfTransfer.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblEOTField",false);                      
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel~btnDelete"); 
            this.commons = null;                        
        }
    }       
    
   this.delete_record = function()
    {     
         if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.BranchSetup.EndOfTransfer.delete_record(this.SEQ_ID, this.delete_record_callback);    
    }     
        
    this.delete_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.EndOfTransfer.read_message();
                alert(msg.value);return;
            }
            
            alert("RECORD DELETED SUCCESSFULLY");            
            this.commons = new common_method();  
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit"); 
            this.commons = null;   
            eot.load_grid_viewer();                     
        }
    }     
    
    this.commit_record = function()
    {
        if (document.getElementById("ddCutOff").value == "" ||document.getElementById("ddCutOff").value == 'null')
        {
           alert(" Please Specify the CutOff Period. ");
           document.getElementById("ddTime").focus();
           return false;
        }
        if (document.getElementById("ddTime").value == "")
        {
           alert(" Please Specify the Time. ");
           document.getElementById("ddTime").focus();
           return false;
        }
       
        this.CutOff = document.getElementById("ddCutOff").value;
        var rs = null;
        rs = IRSWorld.BranchSetup.EndOfTransfer.check_cutoff(this.CutOff).value;
        if((document.getElementById("txtHiddenSeqID").value).length > 0 && document.getElementById("txtHiddenSeqID").value >= '0')
        
        if (rs.Tables[0].Rows.length > 0)
        {
          var wee = rs.Tables[0].Rows[0].CutOff; 
        }
        
        if (typeof(wee) != 'undefined')
        {
              alert("CutOff Period already exists/active.");
              document.getElementById("ddCutOff").focus();
              return false;
        }
 
        IRSWorld.BranchSetup.EndOfTransfer.commit_record(this.set_end_of_transfer(),this.commit_record_callback);
        document.getElementById("txtHiddenSeqID").value = '';         
    }   
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.EndOfTransfer.read_message();alert(msg.value);return;
            }
            
            alert("RECORD COMITTED SUCCESSFULLY");
            this.SEQ_ID = IRSWorld.BranchSetup.EndOfTransfer.ENDOFTRANSFER_SEQ_ID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit"); 
            this.commons = null;
            eot.load_grid_viewer();
        }
    }   
    
    this.rollback_record = function()
    {        
        this.clear();
        IRSWorld.BranchSetup.EndOfTransfer.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.BranchSetup.EndOfTransfer.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }        
        this.commons = new common_method();
        this.commons.to_read_only("tblEOTField",true);         
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
        this.commons = null;
        return;
    }   
    
    this.clear = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("tblEOTField");
        this.commons = null;
    } 
     
    this.load_all_dropdownlist = function()
    {       
       this.dropdown_cutoff();
    } 
    
    this.dropdown_cutoff = function()
    {
        IRSWorld.BranchSetup.EndOfTransfer.getdropdown_source("CutOffPeriod", this.dropdown_cutoff_callback)
    }
    
    this.dropdown_cutoff_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddCutOff", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    }
    
    this.do_ajaxgridview = function(parameter)
    {                        
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID").value = command_param[1];
            
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit");
                    this.commons.to_read_only("tblEOTField",true);     
                    this.commons = null;
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
}
