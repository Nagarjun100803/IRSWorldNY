var seqid = null;

function pull_server_data()
    {             
        if (seqid == null)
        {            
            seqid = IRSWorld.BranchSetup.Declaration.Declaration_SEQ_ID().value;
        }
        if (seqid == null ||seqid == '' || seqid == 'undefined')
        {
            return;
        }                      
        IRSWorld.BranchSetup.Declaration.GET_Declaration_RECORD(seqid, pull_server_data_callback);
    }   
    
     
function pull_server_data_callback(result)
    {        
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    seqid                       = ds.Tables[0].Rows[0].SEQ_ID;
                    this.SEQ_ID                 = ds.Tables[0].Rows[0].SEQ_ID;
                    this.NegBranchCode          = ds.Tables[0].Rows[0].NegBranchCode;
                    this.BranchCode             = ds.Tables[0].Rows[0].BranchCode;
                    this.Declaration1           = ds.Tables[0].Rows[0].Declaration1;
                    this.Declaration2           = ds.Tables[0].Rows[0].Declaration2;
                    this.Declaration3           = ds.Tables[0].Rows[0].Declaration3;
                    this.Declaration4           = ds.Tables[0].Rows[0].Declaration4;
                    this.Declaration5           = ds.Tables[0].Rows[0].Declaration5;
                    
                    if(this.Declaration1 == null ||  this.Declaration1 == 'NULL')
                    {
                        this.Declaration1 = '';
                    }
                    
                    if(this.Declaration2 == null ||  this.Declaration2 == 'NULL')
                    {
                        this.Declaration2 = '';
                    }
                    
                    if(this.Declaration3 == null ||  this.Declaration3 == 'NULL')
                    {
                        this.Declaration3 = '';
                    }
                    
                    if(this.Declaration4 == null ||  this.Declaration4 == 'NULL')
                    {
                        this.Declaration4 = '';
                    }
                    
                    if(this.Declaration5 == null ||  this.Declaration5 == 'NULL')
                    {
                        this.Declaration5 = '';
                    }
                   document.getElementById("txtHiddenSeqID").value             = this.SEQ_ID;
                   document.getElementById("ddlNegBranch").value               = this.NegBranchCode;
                   document.getElementById("ddlBranch").value                  = this.BranchCode;
                   document.getElementById("txtDeclaration1").value            = this.Declaration1;
                   document.getElementById("txtDeclaration2").value            = this.Declaration2;
                   document.getElementById("txtDeclaration3").value            = this.Declaration3;
                   document.getElementById("txtDeclaration4").value            = this.Declaration4;
                   document.getElementById("txtDeclaration5").value            = this.Declaration5;
                   
                   
                   //this.commons = new common_method();
                   //this.commons.to_read_only("tblDeclaration", true);
                   //document.getElementById("ddlNegBranch").disabled = 'disabled';
                   //document.getElementById("ddlBranch").value = 'disabled';
                   //document.getElementById("txtDeclaration1").disabled = 'disabled';
                   //document.getElementById("txtDeclaration2").disabled = 'disabled';
                  // document.getElementById("txtDeclaration3").disabled = 'disabled';   
                  // document.getElementById("txtDeclaration4").disabled = 'disabled';   
                  // document.getElementById("txtDeclaration5").disabled = 'disabled';                       
                  // this.commons.to_unlock_button("tblbuttons","btnNew~btnEdit~btnCancel");
                   //this.commons = null;
            }
        }
    }

function load_record()
{
    pull_server_data();

}

function clear_text()
{
    document.getElementById("txtDeclaration1").value = '';
    document.getElementById("txtDeclaration2").value = '';
    document.getElementById("txtDeclaration3").value = '';   
    document.getElementById("txtDeclaration4").value = '';   
    document.getElementById("txtDeclaration5").value = '';
    document.getElementById("txtDeclaration1").disabled = 'disabled';
    document.getElementById("txtDeclaration2").disabled = 'disabled';
    document.getElementById("txtDeclaration3").disabled = 'disabled';   
    document.getElementById("txtDeclaration4").disabled = 'disabled';   
    document.getElementById("txtDeclaration5").disabled = 'disabled'; 
    this.commons = new common_method();
    this.commons.to_read_only("tblDeclaration",true);                          
    this.commons.to_lock_button("tblbuttons"); 
    this.commons = null;

    seqid = null;
    document.getElementById("txtHiddenSeqID").value = '';
}

function clear_text2()
{
    document.getElementById("txtDeclaration1").value = '';
    document.getElementById("txtDeclaration2").value = '';
    document.getElementById("txtDeclaration3").value = '';   
    document.getElementById("txtDeclaration4").value = '';   
    document.getElementById("txtDeclaration5").value = '';
    document.getElementById("txtDeclaration1").disabled = 'disabled';
    document.getElementById("txtDeclaration2").disabled = 'disabled';
    document.getElementById("txtDeclaration3").disabled = 'disabled';   
    document.getElementById("txtDeclaration4").disabled = 'disabled';   
    document.getElementById("txtDeclaration5").disabled = 'disabled'; 
    this.commons = new common_method();
    this.commons.to_read_only("tblDeclaration",true);                          
    this.commons.to_unlock_button("tblbuttons","btnEdit~btnCancel");
    this.commons = null;

    seqid = null;
    document.getElementById("txtHiddenSeqID").value = '';
}


function Declaration()
{
    seqid                       = null;
    this.commons                = null;
    this.SEQ_ID                 = null;
    this.NegBranchCode          = null;
    this.BranchCode             = null;
    this.Declaration1           = null;
    this.Declaration2           = null;
    this.Declaration3           = null;
    this.Declaration4           = null;
    this.Declaration5           = null;
    
    
    this.generic     = new generic_method(); 
    this.error_input = null;

    var declaration_function = this;
    
    this.load = function()
    { 
        this.dropdown_negbranchcode();
        this.dropdown_branch();
        clear_text();
        //this.clear_fields();
    }
    
    this.load_branches = function()
    { 
        seqid = null;
        this.dropdown_branch2();
        this.populate_declaration();
    }
    
    this.load_declarations = function()
    {
        seqid = null;
        IRSWorld.BranchSetup.Declaration.get_declaration_bybranchcode(document.getElementById("ddlNegBranch").value, document.getElementById("ddlBranch").value, this.load_declarations_callback)
    }
    
    this.load_declarations_callback = function(result)
    {
         if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    
                    seqid                       = ds.Tables[0].Rows[0].SEQ_ID;
                    this.SEQ_ID                 = ds.Tables[0].Rows[0].SEQ_ID;
                    this.NegBranchCode          = ds.Tables[0].Rows[0].NegBranchCode;
                    this.BranchCode             = ds.Tables[0].Rows[0].BranchCode;
                    this.Declaration1           = ds.Tables[0].Rows[0].Declaration1;
                    this.Declaration2           = ds.Tables[0].Rows[0].Declaration2;
                    this.Declaration3           = ds.Tables[0].Rows[0].Declaration3;
                    this.Declaration4           = ds.Tables[0].Rows[0].Declaration4;
                    this.Declaration5           = ds.Tables[0].Rows[0].Declaration5;
                    
                    if(this.Declaration1 == null ||  this.Declaration1 == 'NULL')
                    {
                        this.Declaration1 = '';
                    }
                    
                    if(this.Declaration2 == null ||  this.Declaration2 == 'NULL')
                    {
                        this.Declaration2 = '';
                    }
                    
                    if(this.Declaration3 == null ||  this.Declaration3 == 'NULL')
                    {
                        this.Declaration3 = '';
                    }
                    
                    if(this.Declaration4 == null ||  this.Declaration4 == 'NULL')
                    {
                        this.Declaration4 = '';
                    }
                    
                    if(this.Declaration5 == null ||  this.Declaration5 == 'NULL')
                    {
                        this.Declaration5 = '';
                    }
                   
                    //IRSWorld.BranchSetup.Declaration.getUpdateDeclaration(seqid);
                   document.getElementById("txtHiddenSeqID").value             = this.SEQ_ID;
                   //document.getElementById("ddlNegBranch").value               = this.NegBranchCode;
                   //document.getElementById("ddlBranch").value                  = this.BranchCode;
                   document.getElementById("txtDeclaration1").value            = this.Declaration1;
                   document.getElementById("txtDeclaration2").value            = this.Declaration2;
                   document.getElementById("txtDeclaration3").value            = this.Declaration3;
                   document.getElementById("txtDeclaration4").value            = this.Declaration4;
                   document.getElementById("txtDeclaration5").value            = this.Declaration5;
                   
                   
                   this.commons = new common_method();
                   this.commons.to_read_only("tblDeclaration", true);
                   //document.getElementById("ddlNegBranch").disabled = 'disabled';
                   //document.getElementById("ddlBranch").value = 'disabled';
                   document.getElementById("txtDeclaration1").disabled = 'disabled';
                   document.getElementById("txtDeclaration2").disabled = 'disabled';
                   document.getElementById("txtDeclaration3").disabled = 'disabled';   
                   document.getElementById("txtDeclaration4").disabled = 'disabled';   
                   document.getElementById("txtDeclaration5").disabled = 'disabled';                       
                   this.commons.to_unlock_button("tblbuttons","btnEdit~btnCancel");
                   this.commons = null;
                   load_record();
            }
            else
            {
                clear_text2();
            }
        }
        else
        {
            clear_text2();
        }
    }
    
    this.populate_declaration = function()
    {
        IRSWorld.BranchSetup.Declaration.get_declaration_bynegbranchcode(document.getElementById("ddlNegBranch").value, this.populate_declaration_callback)
    }
    
    this.clear_fields = function()
    {
         document.getElementById("txtDeclaration1").value = '';
         document.getElementById("txtDeclaration2").value = '';
         document.getElementById("txtDeclaration3").value = '';   
         document.getElementById("txtDeclaration4").value = '';   
         document.getElementById("txtDeclaration5").value = '';  
    }
    
    this.populate_declaration_callback = function(result)
    {
         if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    
                    seqid                       = ds.Tables[0].Rows[0].SEQ_ID;
                    this.SEQ_ID                 = ds.Tables[0].Rows[0].SEQ_ID;
                    this.NegBranchCode          = ds.Tables[0].Rows[0].NegBranchCode;
                    this.BranchCode             = ds.Tables[0].Rows[0].BranchCode;
                    this.Declaration1           = ds.Tables[0].Rows[0].Declaration1;
                    this.Declaration2           = ds.Tables[0].Rows[0].Declaration2;
                    this.Declaration3           = ds.Tables[0].Rows[0].Declaration3;
                    this.Declaration4           = ds.Tables[0].Rows[0].Declaration4;
                    this.Declaration5           = ds.Tables[0].Rows[0].Declaration5;
                    
                    //IRSWorld.BranchSetup.Declaration.getUpdateDeclaration(seqid);
                    if(this.Declaration1 == null ||  this.Declaration1 == 'NULL')
                    {
                        this.Declaration1 = '';
                    }
                    
                    if(this.Declaration2 == null ||  this.Declaration2 == 'NULL')
                    {
                        this.Declaration2 = '';
                    }
                    
                    if(this.Declaration3 == null ||  this.Declaration3 == 'NULL')
                    {
                        this.Declaration3 = '';
                    }
                    
                    if(this.Declaration4 == null ||  this.Declaration4 == 'NULL')
                    {
                        this.Declaration4 = '';
                    }
                    
                    if(this.Declaration5 == null ||  this.Declaration5 == 'NULL')
                    {
                        this.Declaration5 = '';
                    }
                   
                   document.getElementById("txtHiddenSeqID").value             = this.SEQ_ID;
                   //document.getElementById("ddlNegBranch").value               = this.NegBranchCode;
                   //document.getElementById("ddlBranch").value                  = this.BranchCode;
                   document.getElementById("txtDeclaration1").value            = this.Declaration1;
                   document.getElementById("txtDeclaration2").value            = this.Declaration2;
                   document.getElementById("txtDeclaration3").value            = this.Declaration3;
                   document.getElementById("txtDeclaration4").value            = this.Declaration4;
                   document.getElementById("txtDeclaration5").value            = this.Declaration5;
                   
                   
                   this.commons = new common_method();
                   this.commons.to_read_only("tblDeclaration", true);
                   //document.getElementById("ddlNegBranch").disabled = 'disabled';
                   //document.getElementById("ddlBranch").value = 'disabled';
                   document.getElementById("txtDeclaration1").disabled = 'disabled';
                   document.getElementById("txtDeclaration2").disabled = 'disabled';
                   document.getElementById("txtDeclaration3").disabled = 'disabled';   
                   document.getElementById("txtDeclaration4").disabled = 'disabled';   
                   document.getElementById("txtDeclaration5").disabled = 'disabled';                       
                   this.commons.to_unlock_button("tblbuttons","btnNew~btnEdit~btnCancel");
                   this.commons = null;
                   load_record();
            }
            else
            {
                clear_text2();
            }
        }
        else
        {
            clear_text2();
        }
    }
   
    this.dropdown_negbranchcode = function()
    {
        IRSWorld.BranchSetup.Declaration.getdropdown_negbranchcode(this.dropdown_negbranchcode_callback)
    }
    
    this.dropdown_negbranchcode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlNegBranch", result.value, "NegBranchCode", "NegBranchCode");
            this.commons = null;           
        }
    }
    
    this.dropdown_branch = function()
    {
        IRSWorld.BranchSetup.Declaration.getdropdown_branch(this.dropdown_branch_callback)
    }
    
    this.dropdown_branch_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlBranch", result.value, "BranchCode", "BranchName");
            this.commons = null;           
        }
    }
     
    this.dropdown_branch2 = function()
    {
        IRSWorld.BranchSetup.Declaration.getdropdown_branch2(document.getElementById("ddlNegBranch").value,this.dropdown_branch_callback2)
    }
    
    this.dropdown_branch_callback2 = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlBranch", result.value, "BranchCode", "BranchName");
            this.commons = null;           
        }
    }
     
    //this.load_record = function()
    //{                     
     //    this.pull_server_data();  
    //}
        
    this.set_Declaration = function()
    {   
        return this.push_data_to_server();
     }
     
  
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
        data_row[data_row.length] = "MAIN_SEQ_ID~"          + document.getElementById("txtHiddenSeqID").value;
        data_row[data_row.length]  = "NegBranchCode~"       + document.getElementById("ddlNegBranch").value;
        data_row[data_row.length]  = "BranchCode~"          + document.getElementById("ddlBranch").value;
        data_row[data_row.length]  = "Declaration1~"        + document.getElementById("txtDeclaration1").value;     
        data_row[data_row.length]  = "Declaration2~"        + document.getElementById("txtDeclaration2").value;  
        data_row[data_row.length]  = "Declaration3~"        + document.getElementById("txtDeclaration3").value;
        data_row[data_row.length]  = "Declaration4~"        + document.getElementById("txtDeclaration4").value;
        data_row[data_row.length]  = "Declaration5~"        + document.getElementById("txtDeclaration5").value;            

           
        return data_row.join("^");       
    }
    
     
    this.insert_record = function()
    {    
        this.add_data(); 
        
        IRSWorld.BranchSetup.Declaration.insert_record(this.insert_record_callback);
    }
      
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.Declaration.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblDeclaration",false);                
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;            
        }
    }
    
    this.update_record = function()
    {
       // if (this.SEQ_ID == null || this.SEQ_ID == "")
        //{
           // this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
       // }
        
        if (seqid == null || seqid == "")
        {
            this.insert_record();
            return;
            //alert("Record not found.");return;
        }
        else
        {
            load_record();
        }
        
        IRSWorld.BranchSetup.Declaration.update_record(seqid, this.update_record_callback);
    } 
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.Declaration.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblDeclaration",false); 
            document.getElementById("ddlNegBranch").disabled = '';
            document.getElementById("ddlBranch").disabled = '';
            document.getElementById("txtDeclaration1").disabled = '';
            document.getElementById("txtDeclaration2").disabled = '';
            document.getElementById("txtDeclaration3").disabled = '';   
            document.getElementById("txtDeclaration4").disabled = '';   
            document.getElementById("txtDeclaration5").disabled = '';                        
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
            IRSWorld.BranchSetup.Declaration.commit_record(this.set_Declaration(),this.commit_record_callback);
        }

    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.Declaration.read_message(); alert(msg.value); declaration_function.load_declarations(); return;
            }
            
            this.SEQ_ID = IRSWorld.BranchSetup.Declaration.Declaration_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_read_only("tblDeclaration",true); 
            document.getElementById("txtDeclaration1").disabled = 'disabled';
            document.getElementById("txtDeclaration2").disabled = 'disabled';
            document.getElementById("txtDeclaration3").disabled = 'disabled';   
            document.getElementById("txtDeclaration4").disabled = 'disabled';   
            document.getElementById("txtDeclaration5").disabled = 'disabled';                           
            this.commons.to_unlock_button("tblbuttons","btnNew~btnEdit~btnCancel"); 
            this.commons = null;    
            
            if (IRSWorld.BranchSetup.Declaration.isParamApproval_declaration().value == true) {
                alert("Record has been sent for approval");
            }
            else {
                alert("Record committed successfully");
            }

            declaration_function.load_declarations();
        }
    }
      

    this.clear = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblDeclaration");
       this.commons = null;           
    }
    
    this.add_data = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblDeclaration");
        document.getElementById("txtDeclaration1").value = "";
        document.getElementById("txtDeclaration1").disabled = '';
        document.getElementById("txtDeclaration2").value = "";
        document.getElementById("txtDeclaration2").disabled = '';
        document.getElementById("txtDeclaration3").value = "";
        document.getElementById("txtDeclaration3").disabled = '';
        document.getElementById("txtDeclaration4").value = "";
        document.getElementById("txtDeclaration4").disabled = '';
        document.getElementById("txtDeclaration5").value = "";
        document.getElementById("txtDeclaration5").disabled = '';
          
        this.commons.to_clear_screen("tblbuttons");
        this.commons.to_read_only("tblDeclaration",false);
        this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
        this.commons = null;
    }
}
