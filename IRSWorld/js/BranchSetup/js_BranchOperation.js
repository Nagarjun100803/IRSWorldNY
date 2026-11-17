function execute_ajx_command(param)
{
    if (Branch_Operation != null && Branch_Operation != "undefined")
    {
        Branch_Operation.do_gridsearch(param);
    }
}


function execute_ajaxgridview_command(param)
{
    if (Branch_Operation != null && Branch_Operation != "undefined")
    {
        Branch_Operation.do_gridview(param);
    }
}
function Branch_Operation()
{
    this.commons                = null;
    this.SEQ_ID                 = null;
    this.Merge                  = null;
    this.RegionID               = null;
    this.BranchID               = null;
    this.BranchOperation        = null;
    this.Branch                 = null;
    this.Email                  = null;
    this.Description            = null;
    this.SubBranch              = null;
    this.Address                = null;
    this.TelNo                  = null;
    this.SUB_SEQ_ID             = null;
    
    this.grid_ajax_page      = "../AjxPage/ajaxgrid.aspx";
    this.grid_ajax_query     = null;    
    
    this.generic     = new generic_method(); 
    this.error_input = null;
    
    this.load = function()
    { 
        this.open_division("divBodyRegion");
        this.dropdown_region();
//        document.getElementById("txtHiddenSeqID").value = "";
//        document.getElementById("txtHiddenSeqID2").value = "";
    }
    
    this.close_divisions = function()
    {
        document.getElementById("divBodyRegion").style.display = 'none';
        document.getElementById("divBodyBranchOperation").style.display = 'none';
        document.getElementById("divBodySubBranch").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
    this.dropdown_region = function()
    {
        IRSWorld.BranchSetup.BranchOperation.getdropdown_region(this.dropdown_region_callback)
    }
    
    this.dropdown_region_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlBranch", result.value, "SEQ_ID", "BranchName");
            this.commons = null;           
        }
    }
    
    this.dropdown_branch = function(branch)
    {
        IRSWorld.BranchSetup.BranchOperation.getdropdown_region(branch, this.dropdown_branch_callback)
    }
    
    this.dropdown_branch_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlBranch", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
     

    this.check_region = function()
    {
//        this.RegionID = document.getElementById("ddlRegion").selectedIndex;
        this.BranchID = document.getElementById("ddlBranch").value;
//        this.Merge = this.RegionID + '-' + this.BranchID;
        document.getElementById("txtHiddenSeqID").value = this.SEQ_ID;
        if(this.BranchID == 0)
        {
            alert("Please Select a Branch");
        }        
        else
        {
            var result = IRSWorld.BranchSetup.BranchOperation.check_seqid(this.BranchID);
            if (result.value.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID = result.value.Tables[0].Rows[0].SEQ_ID;
                this.load_record();
                this.load_sub_branch_grid();

            }
            else
            {
                this.insert_record();
                this.load_sub_branch_grid();
            }
            this.open_division('divBodyBranchOperation');
        }

    }
   
    this.load_record = function()
    {                     
         this.pull_server_data();  
    }
        
    this.set_BranchOperation = function()
    {   
        return this.push_data_to_server();
     }
     
   
    this.pull_server_data = function()
    {             
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.BranchSetup.BranchOperation.BranchOperation_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.BranchSetup.BranchOperation.GET_BranchOperation_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
     
    this.pull_server_data_callback = function(result)
    {        
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
//                   this.RegionID         = ds.Tables[0].Rows[0].RegionID;
                   this.BranchID         = ds.Tables[0].Rows[0].BranchID;
                   this.Email            = ds.Tables[0].Rows[0].Email;
                   this.Description      = ds.Tables[0].Rows[0].Description;
                   this.Branch           = ds.Tables[0].Rows[0].Branch;  
                   
                   document.getElementById("txtHiddenSeqID").value             = this.SEQ_ID;
                   document.getElementById("txtBranch").value                  = this.Branch;
                   document.getElementById("txtEmail").value                   = this.Email;
                   document.getElementById("txtDescription").value             = this.Description;
                   
                   this.commons = new common_method();
                   this.commons.to_read_only("tblBranchOperationDefault", true);
                   document.getElementById("txtDescription").disabled = 'disabled';                     
                   this.commons.to_unlock_button("tblbuttons","btnNew~btnEdit~btnCancel");
                   this.commons = null;
            }
        }
    }
    
 
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
        data_row[data_row.length]  = "BranchID~"            + document.getElementById("ddlBranch").value;
        data_row[data_row.length]  = "Branch~"              + document.getElementById("txtBranch").value;
        data_row[data_row.length]  = "Email~"               + document.getElementById("txtEmail").value;     
        data_row[data_row.length]  = "Description~"         + document.getElementById("txtDescription").value;          

           
        return data_row.join("^");       
    }
    
     
    this.insert_record = function()
    {    
        this.add_data(); 
        
        IRSWorld.BranchSetup.BranchOperation.insert_record(this.insert_record_callback);
    }
      
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.BranchOperation.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblBranchOperation",false);                
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
        
        IRSWorld.BranchSetup.BranchOperation.update_record(this.SEQ_ID, this.update_record_callback);
    } 
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.BranchOperation.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblBranchOperationDefault",false); 
            document.getElementById("txtDescription").disabled = '';                        
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;                                  
        }
    }
    
            
     
    this.commit_record = function()
    {
        if (typeof(this.validate_input_branch) != 'undefined')
        {
            if (this.validate_input_branch() == false)
            {
               alert(this.error_input); return;
            }                            
        }  
        if (confirm("Are you sure you want to save this record?"))
        {
            IRSWorld.BranchSetup.BranchOperation.commit_record(this.set_BranchOperation(),this.commit_record_callback);
        }

    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.BranchOperation.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.BranchSetup.BranchOperation.BranchOperation_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_read_only("tblBranchOperationDefault",true);                          
            this.commons.to_unlock_button("tblbuttons","btnNew~btnEdit~btnCancel"); 
            this.commons = null;    
            alert("Record has been submitted for approval");
      
        }
    }
      

    this.clear = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblBranchOperationDefault");
       this.commons = null;           
    }
    
    this.add_data = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblBranchOperationDefault");
        document.getElementById("txtDescription").value = "";
        document.getElementById("txtDescription").disabled = '';  
        this.commons.to_clear_screen("tblbuttons");
        this.commons.to_read_only("tblBranchOperationDefault",false);
        this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
        this.commons = null;
    }
    
    this.load_sub_branch_grid = function()
    {
//        this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_sub_branch_grid~" + this.BranchID;
        this.grid.div_container  = document.getElementById("DIV_BRANCHES");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.do_gridview = function(parameter)
    {
        this.ajax_grid = new grid_selector();
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
                                    
            if (command_param.length > 0)
            {                                                
                if (command_param[0] == "ajx_selected")
                {
                        
                    var command_value = command_param[1].split('~');
                    
                    this.SUB_SEQ_ID = command_value[0];
                    this.SubBranch = command_value[2];
                    this.Address = command_value[3];
                    this.TelNo = command_value[4];
                    this.populate_sub_branch();
                    this.open_division('divBodySubBranch');
                }
                else
                {
                    this.process_grid_viewer(parameter); 
                }
                
            }
        }
    }
    
    this.populate_sub_branch = function()
    {
        document.getElementById('txtHiddenSeqID2').value = this.SUB_SEQ_ID;
        document.getElementById('txtSubBranch').value = this.SubBranch;
        document.getElementById('txtAddress').value = this.Address;
        document.getElementById('txtTelNo').value = this.TelNo;
        this.commons = new common_method();  
        this.commons.to_read_only("tblSubBranch",true);                       
        this.commons.to_unlock_button("tblbuttons2","btnSubEdit~btnSubCancel");
        this.commons = null;    
    }
    

        
    this.set_sub_branch = function()
    {   
        return this.push_data_to_server_sub_branch();
    }

    this.push_data_to_server_sub_branch = function()    
    {    
        var data_row = new Array();
        
        data_row[data_row.length]  = "Operation_SEQ_ID~"    + this.BranchID;     
        data_row[data_row.length]  = "SubBranch~"         + document.getElementById("txtSubBranch").value;          
        data_row[data_row.length]  = "Address~"         + document.getElementById("txtAddress").value;    
        data_row[data_row.length]  = "TelNo~"         + document.getElementById("txtTelNo").value;    
           
        return data_row.join("^");       
    }
    
     
    this.insert_record_sub_branch = function()
    {   
        this.open_division('divBodySubBranch'); 
        this.add_data_sub_branch(); 
        IRSWorld.BranchSetup.BranchOperation.insert_record_sub_branch(this.insert_record_callback_sub_branch);
    }
      
    
    this.insert_record_callback_sub_branch = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.BranchOperation.read_message_sub_branch(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblSubBranch",false);                
            this.commons.to_unlock_button("tblbuttons2","btnSubSave~btnSubCancel");
            this.commons = null;            
        }
    }
    
    this.update_record_sub_branch = function()
    {
        if (this.SUB_SEQ_ID == null)
        {
            this.SUB_SEQ_ID = document.getElementById("txtHiddenSeqID2").value;
        }
        
        if (this.SUB_SEQ_ID == null || this.SUB_SEQ_ID == "")
        {
            alert("Record not found.");return;
        }
        
        IRSWorld.BranchSetup.BranchOperation.update_record_sub_branch(this.SUB_SEQ_ID, this.update_record_callback_sub_branch);
    } 
    
    this.update_record_callback_sub_branch = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.BranchOperation.read_message();
                alert(msg.value);return;
            }
            document.getElementById('txtHiddenSeqID2').value = this.SUB_SEQ_ID;            
            this.commons = new common_method();  
            this.commons.to_read_only("tblSubBranch",false);                    
            this.commons.to_unlock_button("tblbuttons2","btnSubSave~btnSubCancel");
            this.commons = null;


        }
    }
    
            
     
    this.commit_record_sub_branch = function()
    {

        if (typeof(this.validate_input) != 'undefined')
        {
            if (this.validate_input() == false)
            {
               alert(this.error_input); return;
            }                            
        }  
                                    
        if (confirm("Are you sure you want to save this record?"))
        {
            IRSWorld.BranchSetup.BranchOperation.commit_record_sub_branch(this.set_sub_branch(),this.commit_record_callback_sub_branch);
        }

    }

    this.commit_record_callback_sub_branch = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.BranchOperation.read_message(); alert(msg.value); return;
            }
            
            this.SUB_SEQ_ID = IRSWorld.BranchSetup.BranchOperation.SubBranch_SEQ_ID().value;
            document.getElementById('txtHiddenSeqID2').value = this.SUB_SEQ_ID;
            this.commons = new common_method();
            this.commons.to_read_only("tblSubBranch",true);                          
            this.commons.to_unlock_button("tblbuttons2","btnSubNew~btnSubEdit~btnSubCancel"); 
            this.commons = null;    
            
            alert("Record has been submitted for approval");
      
        }
    }
      

    this.clear_sub_branch = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblSubBranch");
       this.commons = null;           
    }
    
    this.add_data_sub_branch = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblSubBranch");
        this.commons.to_clear_screen("tblbuttons2");
        this.commons.to_read_only("tblSubBranch",false);
        this.commons.to_unlock_button("tblbuttons2","btnSubSave~btnSubCancel");
        this.commons = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_sub_branch_grid~" + this.BranchID;
        this.grid.div_container  = document.getElementById("DIV_BRANCHES");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.validate_input = function()
    {
        this.error_input = "";
        if (this.generic.is_empty(document.getElementById("txtSubBranch").value) == true ||document.getElementById("txtSubBranch").value == "")
        {
            this.error_input += "Please enter a Sub Branch!";
            document.getElementById("txtSubBranch").focus();
            return false;
        } 
        
        if (this.generic.is_empty(document.getElementById("txtAddress").value) == true ||document.getElementById("txtAddress").value == "")
        {
            this.error_input += "Please enter an Address!";
            document.getElementById("txtAddress").focus();
            return false;
        } 
        
        if (this.generic.is_empty(document.getElementById("txtTelNo").value) == true ||document.getElementById("txtTelNo").value == "")
        {
            this.error_input += "Please enter a Telephone Number!";
            document.getElementById("txtTelNo").focus();
            return false;
        } 
        
        if (this.error_input != "")
        {
            return false;
        }        
        return true; 	
    }
    
    this.validate_input_branch = function()
    {
        this.error_input = "";
        if (this.generic.is_empty(document.getElementById("txtBranch").value) == true ||document.getElementById("txtBranch").value == "")
        {
            this.error_input += "Please enter a Branch!";
            document.getElementById("txtBranch").focus();
            return false;
        } 
        
        if (this.generic.is_empty(document.getElementById("txtEmail").value) == true ||document.getElementById("txtEmail").value == "")
        {
            this.error_input += "Please enter an Email!";
            document.getElementById("txtEmail").focus();
            return false;
        } 
        
        if (this.generic.is_empty(document.getElementById("txtDescription").value) == true ||document.getElementById("txtDescription").value == "")
        {
            this.error_input += "Please enter a Description!";
            document.getElementById("txtDescription").focus();
            return false;
        } 
        
        if (this.error_input != "")
        {
            return false;
        }        
        return true; 	
    }
}

