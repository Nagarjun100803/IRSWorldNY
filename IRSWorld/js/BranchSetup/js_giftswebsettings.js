function execute_ajaxgridview_command(param)
{
    if (gws != null && gws != "undefined")
    {
        gws.do_ajaxgridview(param);
    }
}

function gws()
{
    this.commons            = null;
    this.generic            = null;
    this.SEQ_ID             = null;
    this.FtpUrl             = null;
    this.Region             = null;
    this.Branch             = null;
    this.UserName           = null;
    this.Password           = null;
    this.Folder             = null;
    this.Currency           = null;
    this.BranchCode         = null;
    this.BranchPrefix       = null;
    this.AccountOfficer     = null;
    this.AccOfficerTel      = null;
    this.AccOfficerEmail    = null;
    this.ComplianceOfficer  = null;
    this.CompOfficerTel     = null;
    this.CompOfficerEmail   = null;
    this.ExtractRuntime     = null;
    this.message            = null;
    this.ajax_grid          = null;
    this.grid               = null;
    this.Role               = null;
    this.IsDomestic        = null;
    
    this.load_grid_viewer = function()
    {     
        this.commons = new common_method();  
        this.commons.to_read_only("tblGWSField",true);
        this.commons.to_read_only("tbl_FTP",true);    
        this.commons.to_read_only("tbl_Extract",true);                        
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;      
        this.grid.sql_source     = "sp_gws~";
        this.grid.div_container  = document.getElementById("GWSGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_gws~";
        this.grid.div_container  = document.getElementById("GWSGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    this.load_record = function()
    {
        this.pull_server_data();        
    }      
    
    this.set_giftsweb_settings = function()
    {
        this.Region             = document.getElementById("ddRegion").value;
        this.Branch             = document.getElementById("ddBranch").value;
        this.FtpUrl             = document.getElementById("txtUrl").value;
        this.UserName           = document.getElementById("txtUserName").value;
        
        
        var intCharCount
        intCharCount            = document.getElementById("txtPassword").value.length
        
        this.Password           = "";
        for (var intCounter = 0; intCounter < intCharCount; intCounter ++)
        {
            this.Password       = this.Password + document.getElementById("txtPassword").value.charCodeAt(intCounter);
        }       
        
        this.Folder             = document.getElementById("txtFolderName").value;
        this.Currency           = document.getElementById("ddCurrency").value;
        this.BranchCode         = document.getElementById("txtBranchCode").value;
        this.BranchPrefix       = document.getElementById("txtBranchPrefix").value;        
        this.AccountOfficer     = document.getElementById("txtAccountOfficer").value;
        this.AccOfficerTel      = document.getElementById("txtAccOfficerTel").value;
        this.AccOfficerEmail    = document.getElementById("txtAccOfficerEmail").value;
        this.ComplianceOfficer  = document.getElementById("txtComplianceOfficer").value;
        this.CompOfficerTel     = document.getElementById("txtCompOfficerTel").value;
        this.CompOfficerEmail   = document.getElementById("txtCompOfficerEmail").value;
        this.ExtractRuntime     = document.getElementById("txtExtractRuntime").value;
        this.IsDomestic         = document.getElementById("chkIsDomestic").checked;
       
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
                  
        IRSWorld.BranchSetup.GiftsWebSettings.GET_GWSETTINGS_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
    this.pull_server_data_callback = function(result)
    {  
        if (result != null && result.value != null)
        {                   
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {   
                //this.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                this.Region             = ds.Tables[0].Rows[0].Region;
                this.Branch             = ds.Tables[0].Rows[0].Branch;
                this.FtpUrl             = ds.Tables[0].Rows[0].FtpUrl;
                this.UserName           = ds.Tables[0].Rows[0].UserName;
                this.Password           = ds.Tables[0].Rows[0].Password;
                this.Folder             = ds.Tables[0].Rows[0].Folder;
                this.Currency           = ds.Tables[0].Rows[0].Currency;
                this.BranchCode         = ds.Tables[0].Rows[0].BranchCode;
                this.BranchPrefix       = ds.Tables[0].Rows[0].BranchPrefix;
                this.AccountOfficer     = ds.Tables[0].Rows[0].AccountOfficer;
                this.AccOfficerTel      = ds.Tables[0].Rows[0].AccOfficerTel;
                this.AccOfficerEmail    = ds.Tables[0].Rows[0].AccOfficerEmail;
                this.ComplianceOfficer  = ds.Tables[0].Rows[0].ComplianceOfficer;
                this.CompOfficerTel     = ds.Tables[0].Rows[0].CompOfficerTel;
                this.CompOfficerEmail   = ds.Tables[0].Rows[0].CompOfficerEmail;
                this.ExtractRuntime     = ds.Tables[0].Rows[0].ExtractRuntime;
                this.IsDomestic         = ds.Tables[0].Rows[0].IsDomestic;
                
                document.getElementById("txtHiddenSeqId").value       = this.SEQ_ID;
                document.getElementById("ddRegion").value             = this.Region;
                document.getElementById("ddBranch").value             = this.Branch;
                document.getElementById("txtUrl").value               = this.FtpUrl;
                document.getElementById("txtUserName").value          = this.UserName;
                
                var pwd;
                pwd = "";
                if (this.Password == null) this.Password = '';
                
                for (var intCounter = 0; intCounter < this.Password.length; intCounter ++)
                {
                    if (this.Password.charAt(intCounter) == 1)
                    {
                        pwd = pwd + String.fromCharCode(this.Password.charAt(intCounter) + this.Password.charAt(intCounter+1) + this.Password.charAt(intCounter+2))
                        intCounter = intCounter + 2
                    }
                    else
                    {
                        pwd = pwd + String.fromCharCode(this.Password.charAt(intCounter) + this.Password.charAt(intCounter+1))
                        intCounter = intCounter + 1
                    }            
                }
        
                document.getElementById("txtPassword").value            = pwd;    //this.Password;
                document.getElementById("txtFolderName").value          = this.Folder;
                document.getElementById("ddCurrency").value             = this.Currency;
                document.getElementById("txtBranchCode").value          = this.BranchCode;
                document.getElementById("txtBranchPrefix").value        = this.BranchPrefix;
                document.getElementById("txtAccountOfficer").value      = this.AccountOfficer;
                document.getElementById("txtAccOfficerTel").value       = this.AccOfficerTel;
                document.getElementById("txtAccOfficerEmail").value     = this.AccOfficerEmail;
                document.getElementById("txtComplianceOfficer").value   = this.ComplianceOfficer;
                document.getElementById("txtCompOfficerTel").value      = this.CompOfficerTel;
                document.getElementById("txtCompOfficerEmail").value    = this.CompOfficerEmail;
                document.getElementById("txtExtractRuntime").value      = this.ExtractRuntime;
                document.getElementById("chkIsDomestic").checked        = this.IsDomestic;
            }
        }
    }
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                               
        data_row[data_row.length]  = "Region~"              + this.Region;
        data_row[data_row.length]  = "Branch~"              + this.Branch;
        data_row[data_row.length]  = "FtpUrl~"              + this.FtpUrl;
        data_row[data_row.length]  = "UserName~"            + this.UserName;
        data_row[data_row.length]  = "Password~"            + this.Password;
        data_row[data_row.length]  = "Folder~"              + this.Folder;
        data_row[data_row.length]  = "Currency~"            + this.Currency;
        data_row[data_row.length]  = "BranchCode~"          + this.BranchCode;
        data_row[data_row.length]  = "BranchPrefix~"        + this.BranchPrefix;        
        data_row[data_row.length]  = "AccountOfficer~"      + this.AccountOfficer;
        data_row[data_row.length]  = "AccOfficerTel~"       + this.AccOfficerTel;
        data_row[data_row.length]  = "AccOfficerEmail~"     + this.AccOfficerEmail;
        data_row[data_row.length]  = "ComplianceOfficer~"   + this.ComplianceOfficer;
        data_row[data_row.length]  = "CompOfficerTel~"      + this.CompOfficerTel;
        data_row[data_row.length]  = "CompOfficerEmail~"    + this.CompOfficerEmail;
        data_row[data_row.length]  = "ExtractRuntime~"      + this.ExtractRuntime; 
        data_row[data_row.length]  = "IsDomestic~"          + this.IsDomestic;
        
        return data_row.join("|");
    } 
    
    this.insert_record = function()
    {     
        this.clear();
        
        this.Role =  IRSWorld.BranchSetup.GiftsWebSettings.getRole();
            
        this.commons = new common_method();  
        
        if (this.Role.value == 28)
        {
            this.commons.to_read_only("tbl_FTP",false);    
            this.commons.to_read_only("tbl_Extract",true);     
        }
        else
        {
            this.commons.to_read_only("tbl_FTP",true);    
            this.commons.to_read_only("tbl_Extract",false);     
        } 
        this.commons = null;
            
        IRSWorld.BranchSetup.GiftsWebSettings.insert_record(this.insert_record_callback);
    }
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.GiftsWebSettings.read_message(); alert(msg.value); return;
            }
           
            this.commons = new common_method();  
            this.commons.to_read_only("tblGWSField",false);                  
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
            alert("RECORD NOT FOUND.");return;
        }
        
        this.Role =  IRSWorld.BranchSetup.GiftsWebSettings.getRole();
            
        this.commons = new common_method();  
        
        if (this.Role.value == 28)
        {
            this.commons.to_read_only("tbl_FTP",false);    
            this.commons.to_read_only("tbl_Extract",true);     
        }
        else
        {
            this.commons.to_read_only("tbl_FTP",true);    
            this.commons.to_read_only("tbl_Extract",false);     
        } 
        this.commons = null;
            
        IRSWorld.BranchSetup.GiftsWebSettings.update_record(this.SEQ_ID, this.update_record_callback);    
    }     
        
    this.update_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.GiftsWebSettings.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
                       
            this.commons.to_read_only("tblGWSField",false);
            //this.commons.to_read_only("tbl_FTP",false);    
            //this.commons.to_read_only("tbl_Extract",false);                     
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
        this.clear();
        IRSWorld.BranchSetup.GiftsWebSettings.delete_record(this.SEQ_ID, this.delete_record_callback);    
    }     
        
    this.delete_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.GiftsWebSettings.read_message();
                alert(msg.value);return;
            }
            
            alert("RECORD DELETED SUCCESSFULLY");            
            this.commons = new common_method();  
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit"); 
            this.commons = null;   
            gws.load_grid_viewer();                     
        }
    }     
    
    this.check_ExtractTime = function()
    {
        var varExtractRunTime 
        varExtractRunTime = document.getElementById("txtExtractRuntime").value;
        if (varExtractRunTime.length == 5)
        {
            event.returnValue = false;
        }
        else
        {
            if (event.keyCode == 58 || event.keyCode == 127 || (event.keyCode >= 48 && event.keyCode <= 57))
            {        
                event.returnValue = true;
                if (varExtractRunTime.length == 1)
                {
                    if (event.keyCode == 58)
                    {
                        document.getElementById("txtExtractRuntime").value = "0" + document.getElementById("txtExtractRuntime").value + String.fromCharCode(event.keyCode);
                        event.returnValue = false;                   
                    }
                    else
                    {
                        document.getElementById("txtExtractRuntime").value = document.getElementById("txtExtractRuntime").value + String.fromCharCode(event.keyCode) + ":";
                        event.returnValue = false;
                    }
                    
                }                
            }
            else
            {
                event.returnValue = false;
                alert("Please input valid characters");
            }
        }
    }    
    
    this.commit_record = function()
    {
        //this.validate_time();
        if (typeof(this.validate_input) != 'undefined')
        {
            if (this.validate_input() == false)
            {
               alert(this.error_input); return;
            }                            
        }
        IRSWorld.BranchSetup.GiftsWebSettings.commit_record(this.set_giftsweb_settings(),this.commit_record_callback);
    }
    
    this.validate_time = function()
    {
        var varExtractRunTime;
        varExtractRunTime = document.getElementById("txtExtractRuntime").value;
        
        if(varExtractRunTime.length != 5)
        {
            alert("invalid time. Time format should be HH:MM (Military Time).");
            return false;
        }        
        
        if (varExtractRunTime.substring(0,2) > 23)
        {
            alert("Invalid inputted hour, should be equal or less than 23")
        }
        
        if (varExtractRunTime.substring(3,5) > 59)
        {
            alert("Invalid inputted minutes, should be equal or less than 59")
        }                
    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {            
            //alert("RECORD COMITTED SUCCESSFULLY");
            alert("Record has been sent for approval");
            this.SEQ_ID = IRSWorld.BranchSetup.GiftsWebSettings.GWSETTINGS_SEQ_ID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit"); 
            this.commons = null;
            gws.load_grid_viewer();
        }
    }
    
    this.rollback_record = function()
    {        
        this.clear();
        IRSWorld.BranchSetup.GiftsWebSettings.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.BranchSetup.GiftsWebSettings.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }        
        this.commons = new common_method();
        this.commons.to_read_only("tblGWSField",true);
        this.commons.to_read_only("tbl_FTP",true);    
        this.commons.to_read_only("tbl_Extract",true);       
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnClear");
        this.commons = null;
        return;
    }   
    
    this.clear = function()
    {   
        this.commons = new common_method();
        this.commons.to_clear_screen("GWSFIELD");
        this.commons.to_clear_screen("tbl_FTP");
        this.commons.to_clear_screen("tbl_Extract");
        this.commons = null;
    }    
    
    
    this.load_all_dropdownlist = function()
    {       
       this.dropdown_region();
       this.dropdown_currency();
       this.dropdown_branch();
       this.getRole();
    } 
    
    this.getRole = function()
    {
        IRSWorld.BranchSetup.GiftsWebSettings.getRole(this.Role_callback);
    }
        
    this.Role_callback  = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.Role = result.value;
        }
    }
    
    this.reload_branches = function()
    {
        this.Region
        var intRegion 
        intRegion = document.getElementById("ddRegion").value;
        
        IRSWorld.BranchSetup.GiftsWebSettings.reload_branches(intRegion, this.dropdown_branch_callback)
    } 
    
    this.dropdown_region = function()
    {
        IRSWorld.BranchSetup.GiftsWebSettings.getdropdown_source("ChargeGroup", this.dropdown_region_callback)
    }
    
    this.dropdown_region_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddRegion", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_currency = function()
    {
        IRSWorld.BranchSetup.GiftsWebSettings.getdropdown_source("Currency", this.dropdown_currency_callback)
    }
    
    this.dropdown_currency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddCurrency", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_branch = function()
    {
        IRSWorld.BranchSetup.GiftsWebSettings.getdropdown_branch(this.dropdown_branch_callback)
    }
    
    this.dropdown_branch_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddBranch", result.value, "BranchName", "BranchName");
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
                document.getElementById("txtHiddenSeqId").value = command_param[1];
            
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit");
                    this.commons.to_read_only("tblGWSField",true);
                    this.commons.to_read_only("tbl_FTP",true);    
                    this.commons.to_read_only("tbl_Extract",true);  
                    this.commons = null;
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
    this.validate_input = function()
    {     
        this.commons     = new common_method();    
        this.generic     = new generic_method();    
        this.error_input = '';
        this.folderdir = document.getElementById("txtFolderName").value;
        
        if(document.getElementById("ddRegion").value == null ||document.getElementById("ddRegion").value == 'null')
        {
            alert("Please Complete Necessary Fields. (Region)\n");
            document.getElementById("ddRegion").focus();
            return false;
        }
        
        if(document.getElementById("ddBranch").value == null ||document.getElementById("ddBranch").value == 'null')
        {
            alert ("Please Complete Necessary Fields. (Branch)\n");
            document.getElementById("ddBranch").focus();
            return false;
        }
        
        if (this.Role == 28)
        {
            if (this.generic.to_validate_pass_length(document.getElementById("txtPassword").value) == false)
            {
                alert (" Password must be at least 6 characters long. \n");
                document.getElementById("txtPassword").focus();
                return false;       
            }
            
            if (this.generic.to_validate_pass_length(document.getElementById("txtUserName").value) == false)
            {
                alert (" UserName must be at least 6 characters long. \n");
                document.getElementById("txtUserName").focus();
                return false;       
            }
            
            if (trim(document.getElementById("txtUrl").value) == "")
            {
                alert ("Please input FTP Server Name / IP Address");
                document.getElementById("txtUrl").focus();
                return false;       
            }
            
            if (trim(document.getElementById("txtFolderName").value) == "")
            {
                alert ("Please input Folder Name");
                document.getElementById("txtFolderName").focus();
                return false;       
            }
        }        
        
        if(document.getElementById("txtBranchCode").value == "" ||this.generic.is_empty(document.getElementById("txtBranchCode").value) == true)
        {
            alert ("Please Complete Necessary Fields. (Branch Code)\n");
            document.getElementById("txtBranchCode").focus();
            return false;
        }
        
        if (this.generic.is_alphanumeric(document.getElementById("txtBranchCode").value) == false)
        {
            alert (" Branch Code Contains invalid characters. \n");
            document.getElementById("txtBranchCode").focus();
            return false;       
        }
       
        if(document.getElementById("txtBranchPrefix").value == "" ||this.generic.is_empty(document.getElementById("txtBranchPrefix").value) == true)
        {
            alert ("Please Complete Necessary Fields. (Branch Prefix)\n");
            document.getElementById("txtBranchPrefix").focus();
            return false;
        }
        
        if (this.generic.is_alphanumeric(document.getElementById("txtBranchPrefix").value) == false)
        {
            alert (" Branch Prefix Contains invalid characters. \n");
            document.getElementById("txtBranchPrefix").focus();
            return false;       
        }
    }
   
}
