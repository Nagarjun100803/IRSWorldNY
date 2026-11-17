function execute_ajx_command(param)
{
    if (Faultlog_Status != null && Faultlog_Status != "undefined")
    {
        Faultlog_Status.do_gridsearch(param);
    }
}


function execute_ajaxgridview_command(param)
{
    if (Faultlog_Status != null && Faultlog_Status != "undefined")
    {
        Faultlog_Status.do_gridview(param);
    }
}

var path = null;
var code = null;
var user_id = null;
var date = null;
var br = null;
var name = null;
var image = "";

function Faultlog_Status()
{

    this.commons                    = null;
    this.SEQ_ID                     = null;
    this.IssueReference             = null;
    this.DateReported               = null;
    this.IssueDescription           = null;
    this.ApplicationModule          = null;
    this.BranchAgent                = null;
    this.TestedBy                   = null;
    this.Remarks                    = null;
    this.Status                     = null;
    this.AttendedBy                 = null;
    this.DateAttended               = null;
    this.Image                      = null;
    this.DatePosted                 = null;
    this.UserId                     = null;
    this.Comment                    = null;
    this.ScreenShot                 = null;
    this.ajax_grid                  = null;
    this.grid_ajax_query            = null;
    this.grid_ajax_page             = "../AjxPage/ajaxgrid.aspx";
    this.panel                      = null;
    this.generic                    = new generic_method();
    this.error_input                = "";
    
    this.load = function()
    { 
        this.load_status_grid();
        this.open_division("divFaultlog");
        this.open_division("divFaultlogSummary");
        var is_postback = IRSWorld.Faultlog.Faultlog.get_is_postback().value;
        if(is_postback)
        {
            this.update_record();
        }
        else
        {
            IRSWorld.Faultlog.Faultlog.set_is_postback(false);
        }
    }
    
    this.load_issue = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblFaultlogDefault");
        document.getElementById("txtIssueDescription").value = "";
        document.getElementById("txtIssueDescription").disabled = true;  
        this.commons.to_read_only("tblFaultlogDefault",true);
        this.commons.to_unlock_button("tblFaultlogIssueButtons","btnFaultlogIssueNew~btnFaultlogIssueCancel~");
        this.commons = null;
        this.open_division("divFaultlogIssue");
        
    }

    this.get_disabled = function()
    {
        document.getElementById("txtFaultlogIssueDate").value = date;
        document.getElementById("txtFaultlogCommentDate").value = date;
        document.getElementById("txtFaultlogActionDate").value = date;
        document.getElementById("txtFaultlogIssueFCLSNo").disabled = true;
        document.getElementById("txtFaultlogIssueDate").disabled = true;
        document.getElementById("txtFaultlogIssuePreparedBy").disabled = true;
        document.getElementById("txtFaultlogIssueBranch").disabled = true;
        document.getElementById("txtClosed").disabled = true;
        document.getElementById("txtDuplicate").disabled = true;
        document.getElementById("txtForDiscussion").disabled = true;
        document.getElementById("txtForRetest").disabled = true;
        document.getElementById("txtOpen").disabled = true;
        document.getElementById("txtGrandTotal").disabled = true;
        document.getElementById("txtFaultlogActionFCLSNo").disabled = true; 
        document.getElementById("txtFaultlogActionDate").disabled = true;
        document.getElementById("txtFaultlogActionModule").disabled = true;
        document.getElementById("ddlFaultlogActionStatus").disabled = true;
        document.getElementById("txtFaultlogActionAttendedBy").disabled = true;
        document.getElementById("txtFaultlogAction").disabled = true;
        document.getElementById("txtFaultlogCommentFCLSNo").disabled = true;
        document.getElementById("txtFaultlogCommentModule").disabled = true;
        document.getElementById("txtFaultlogCommentDate").disabled = true;
        document.getElementById("txtFaultlogCommentUser").disabled = true;
        document.getElementById("txtFaultlogCommentDate").disabled = date;
    }
   
    
    this.get_user = function(result)
    {
        if (result != null && result != "undefined" && result.value != null)
        {
            var command_param = result.value.split("~");
            
            if(command_param[0] == "Error")
            {
                alert(result.value);
            }
            else
            {
                if(command_param[4] != null && command_param[4] != "undefined")
                {
                    document.getElementById("txtFaultlogIssuePreparedBy").value = command_param[0];
                    document.getElementById("txtFaultlogActionAttendedBy").value = command_param[0];
                    document.getElementById("txtFaultlogCommentUser").value = command_param[0];
                    document.getElementById("txtFaultlogIssueDate").value = command_param[4];
                    code = command_param[1];
                    document.getElementById("txtFaultlogIssueBranch").value = command_param[2]; 
                    user_id = command_param[3];
                    date = command_param[4];
                    name = command_param[0];
                    br = command_param[2];
                }
                else
                {
                    alert("Unable to get server date.");
                }
            }
                    
        }
        else
        {
            this.commons = new common_method(); 
            this.commons.to_read_only("tblFaultlog", true);
            document.getElementById("btnFaultlogIssueSave").disabled = true;
            document.getElementById("btnFaultlogActionSave").disabled = true;
            document.getElementById("btnFaultlogCommentSave").disabled = true;
            this.commons = null;   
            alert("Invalid User!");
            
            
        }
    }

    this.close_divisions = function()
    {
        document.getElementById("divFaultlogIssue").style.display = 'none';
        document.getElementById("divFaultlogSummary").style.display = 'none';
        document.getElementById("divFaultlogAction").style.display = 'none';
        document.getElementById("divFaultlogComment").style.display = 'none';
        document.getElementById("divFaultlogScreenShot").style.display = 'none';
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = 'block';       
    }
    
    this.dropdown_keyword = function()
    {
        IRSWorld.Faultlog.Faultlog.getdropdown_keyword("FaultlogStatus", this.dropdown_keyword_callback)
    }
    
    this.dropdown_keyword_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlFaultlogIssueStatus", result.value, "FldValue", "FldDescription");
            this.commons.to_load_dropdownlist("ddlFaultlogActionStatus", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.get_faultlogstatus_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            var ds = result.value;
            
            if (ds.Tables[0].Rows.length > 0)
            {
                document.getElementById("txtClosed").value = ds.Tables[0].Rows[0].Closed; 
                document.getElementById("txtDuplicate").value = ds.Tables[0].Rows[0].Duplicate; 
                document.getElementById("txtForDiscussion").value = ds.Tables[0].Rows[0].ForDiscussion; 
                document.getElementById("txtForRetest").value = ds.Tables[0].Rows[0].ForRetest; 
                document.getElementById("txtOpen").value = ds.Tables[0].Rows[0].Open;
                document.getElementById("txtGrandTotal").value = (ds.Tables[0].Rows[0].Closed + ds.Tables[0].Rows[0].Duplicate + 
                                                                  ds.Tables[0].Rows[0].ForDiscussion + ds.Tables[0].Rows[0].ForRetest +
                                                                  ds.Tables[0].Rows[0].Open);
                                                                  
                this.commons = new common_method();
                Faultlog_Status.get_disabled();
                this.commons.to_read_only("tblFaultlogSummary", false);   
                this.commons.to_unlock_button("tblFaultlogSummaryButtons","btnFaultlogSummaryNew~btnFaultlogSummaryCancel~");
                this.commons = null;              
            }
            else
            {
                alert("Unable to connect Database!");
            }
        }
    }
    
    this.load_status_grid_search = function(panel)
    {
        this.panel = panel;
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = 'sp_faultlog_summary_grid~';
        this.ajax_grid.open_selection_grid(this.grid_ajax_page,this.grid_ajax_query, 0, this.SEQ_ID);
    }
    
    
    this.load_status_grid = function()
    {
        IRSWorld.Faultlog.Faultlog.get_faultlogstatus(this.get_faultlogstatus_callback)
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_faultlog_summary_grid~";
        this.grid.div_container  = document.getElementById("DIV_STATUS");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.load_status_grid_comment = function()
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_faultlog_comment_grid~";
        this.grid.div_container  = document.getElementById("DIV_COMMENT");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.do_gridsearch = function(parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");                
                
                if (command_param.length > 0)
                {
                    if (command_param[0] == "ajx_selected")
                    { 
                        var command_value = command_param[1].split('~');
                    
                        this.dropdown_keyword();
                        
                        this.SEQ_ID                 = command_value[0];
                        this.IssueReference         = command_value[1];
                        this.DateReported           = command_value[2];
                        this.IssueDescription       = command_value[3];
                        this.ApplicationModule      = command_value[4];
                        this.BranchAgent            = command_value[5];
                        this.Status                 = command_value[6];
                        this.TestedBy               = command_value[7];
                        this.ScreenShot             = command_value[8];
                       
                        if(this.panel == "ACTION")
                        {
                            document.getElementById("txtHiddenSeqID2").value  = this.SEQ_ID;     
                            document.getElementById("txtFaultlogActionFCLSNo").value = command_value[1];
                            document.getElementById("txtFaultlogActionDate").value = date;
                            document.getElementById("txtFaultlogActionModule").value = command_value[4]; 
                            document.getElementById("ddlFaultlogActionStatus").selectedIndex = parseInt(command_value[6]);
                            
                            this.commons = new common_method();
                            this.commons.to_read_only("tblFaultlogAction", true);
                            this.get_disabled();      
                            this.commons.to_unlock_button("tblFaultlogActionButtons","btnFaultlogActionEdit~btnFaultlogActionCancel~");
                            this.commons = null;
                            this.open_division('divFaultlogAction');
                            this.ajax_grid.process_ajax(parameter); return;
                        }
                        else if(this.panel == "COMMENT")
                        {
                            document.getElementById("txtHiddenSeqID3").value  = this.SEQ_ID;     
                            document.getElementById("txtFaultlogCommentFCLSNo").value = command_value[1];
                            document.getElementById("txtFaultlogCommentDate").value = date;
                            document.getElementById("txtFaultlogCommentModule").value = command_value[4];

                            this.load_status_grid_comment();
                            
                            this.commons = new common_method();
                            this.commons.to_read_only("tblFaultlogComment", true);
                            document.getElementById("txtFaultlogComment").disabled = true;
                            this.get_disabled();    
 
                            this.commons.to_unlock_button("tblFaultlogCommentButtons","btnFaultlogCommentNew~btnFaultlogCommentCancel~");
                            this.commons = null;
                            this.open_division('divFaultlogComment'); 
                            this.ajax_grid.process_ajax(parameter); return;
                        }
                        else
                        {
                            document.getElementById("txtHiddenSeqID4").value  = this.SEQ_ID;
                            document.getElementById("btnDownload").disabled   = true;
                            this.open_division('divFaultlogScreenShot'); 
                            this.ajax_grid.process_ajax(parameter); return;
                        }
                                                       
                    }
                    else
                    {                 
                        this.ajax_grid.process_ajax(parameter); return;
                    }
                                  
                }
            
        }
    }
    
    this.get_file_path = function()
    {
        IRSWorld.Faultlog.Faultlog.get_path(this.SEQ_ID, this.get_file_path_callback);
    }
    
    this.get_file_path_callback = function(result)
    {
        if (result != null && result.value != "undefined" && result.value != "IRSClassLib.localhost.DBNull")
        {
            document.getElementById("txtScreenShot").value = result.value;
            document.getElementById("btnDownload").disabled   = false;
            path = result.value;
            this.generic = new generic_method();
            if(this.generic.is_empty(document.getElementById("txtScreenShot").value) == true)
	        {
	            document.getElementById("btnDownload").disabled   = true;
	            alert("No uploaded Screenshot!");
	        }
	        this.generic = null;
 
        }
        else
        {
        	    document.getElementById("btnDownload").disabled   = true;
	            alert("No uploaded Screenshot!");
        }
    }
    
    this.dl_screen = function()
    {
        IRSWorld.Faultlog.Faultlog.download_screenshot(this.ScreenShot)
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
                    
                    this.dropdown_keyword();
                    
                    if(command_value[0] == 'COMMENT')
                    {
                        this.SEQ_ID                 = command_value[1];

                        document.getElementById("txtHiddenSeqID3").value  = this.SEQ_ID;     
                        document.getElementById("txtFaultlogCommentFCLSNo").value = command_value[2];
                        document.getElementById("txtFaultlogCommentDate").value = command_value[3];
                        document.getElementById("txtFaultlogCommentUser").value = command_value[4];
                        document.getElementById("txtFaultlogComment").value = command_value[5];

                        this.commons = new common_method();
                        this.commons.to_read_only("tblFaultlogDefault", false);
                        this.get_disabled();    
                        document.getElementById("txtFaultlogCommentFCLSNo").disabled = true; 
                        document.getElementById("txtFaultlogCommentDate").disabled = true;
                        document.getElementById("txtFaultlogCommentUser").disabled = true;   
                        this.commons.to_unlock_button("tblFaultlogCommentButtons","btnFaultlogCommentEdit~btnFaultlogCommentCancel~");
                        this.commons = null;
                        this.open_division('divFaultlogComment');
                    }
                    else
                    {
                        this.SEQ_ID                                                     = command_value[0];
                        document.getElementById("txtHiddenSeqID").value                 = this.SEQ_ID;
                        document.getElementById("txtFaultlogIssueFCLSNo").value         = command_value[1];
                        document.getElementById("txtFaultlogIssueDate").value           = command_value[2];
                        document.getElementById("txtIssueDescription").value            = command_value[3];
                        document.getElementById("txtFaultlogIssueModuleTested").value   = command_value[4];
                        document.getElementById("txtFaultlogIssueBranch").value         = command_value[5];
                        document.getElementById("ddlFaultlogIssueStatus").selectedValue = command_value[6];
                        document.getElementById("txtFaultlogIssuePreparedBy").value     = command_value[7];
                        
                        this.commons = new common_method();
                        this.commons.to_read_only("tblFaultlogDefault", true);
                        this.get_disabled();    
                        this.commons.to_unlock_button("tblFaultlogIssueButtons","btnFaultlogIssueEdit~btnFaultlogIssueCancel~");
                        this.commons = null;
                        this.open_division('divFaultlogIssue');
                    }
                    
                }
                else
                {
                    this.process_grid_viewer(parameter); 
                }
                
            }
        }
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_faultlog_summary_grid~";
        this.grid.div_container  = document.getElementById("DIV_STATUS");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.load_record = function()
    {                     
         this.pull_server_data();  
    }
        
    this.set_faultlog = function(method)
    {   

        this.IssueReference         = document.getElementById("txtFaultlogIssueFCLSNo").value;
        this.DateReported           = document.getElementById("txtFaultlogIssueDate").value;
        this.IssueDescription       = document.getElementById("txtIssueDescription").value;
        this.ApplicationModule      = document.getElementById("txtFaultlogIssueModuleTested").value;
        this.BranchAgent            = document.getElementById("txtFaultlogIssueBranch").value;
        this.TestedBy               = document.getElementById("txtFaultlogIssuePreparedBy").value;
	   
        if(method == "ISSUE")
        {
            this.Status             = document.getElementById("ddlFaultlogIssueStatus").selectedIndex;
        }
        else
        {
            this.Status             = document.getElementById("ddlFaultlogActionStatus").selectedIndex;
        }
        
        this.Remarks                = document.getElementById("txtFaultlogAction").value;
        this.AttendedBy             = document.getElementById("txtFaultlogActionAttendedBy").value;
        this.DateAttended           = document.getElementById("txtFaultlogActionDate").value;
        
        return this.push_data_to_server();
     }
    
    
    this.set_faultlog_comment = function()
    {   
        this.IssueReference         = document.getElementById("txtFaultlogCommentFCLSNo").value;
        this.DatePosted             = document.getElementById("txtFaultlogCommentDate").value;
        this.UserId                 = document.getElementById("txtFaultlogCommentUser").value;
        this.Comment                = document.getElementById("txtFaultlogComment").value;
        
        return this.push_comment_data_to_server();
     }
   
    this.pull_server_data = function()
    {             
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.Faultlog.Faultlog.FAULTLOG_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.Faultlog.Faultlog.GET_FAULTLOG_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
     
    this.pull_server_data_callback = function(result)
    {        
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
            
                    this.IssueReference         = ds.Tables[0].Rows[0].IssueReference;
                    this.commons                = new common_method();
                    var dd                      = this.commons.to_mmddyyyy_date(ds.Tables[0].Rows[0].DateReported);
                    var tt                      = ds.Tables[0].Rows[0].DateReported.toLocaleTimeString();
                    this.DateReported           = dd + " " + tt ;
                    this.IssueDescription       = ds.Tables[0].Rows[0].IssueDescription;
                    this.ApplicationModule      = ds.Tables[0].Rows[0].ApplicationModule;
                    this.BranchAgent            = ds.Tables[0].Rows[0].BranchAgent
                    this.TestedBy               = ds.Tables[0].Rows[0].TestedBy;
                    this.Remarks                = ds.Tables[0].Rows[0].Remarks;
                    this.Status                 = ds.Tables[0].Rows[0].Status;
                   
                    document.getElementById("txtHiddenSeqID").value                 = this.SEQ_ID;
                    document.getElementById("txtHiddenSeqID2").value                = this.SEQ_ID;
                    document.getElementById("txtFaultlogIssueFCLSNo").value         = this.IssueReference;
                    document.getElementById("txtFaultlogActionFCLSNo").value        = this.IssueReference;
                    document.getElementById("txtFaultlogIssueDate").value           = this.DateReported;
                    document.getElementById("txtIssueDescription").value            = this.IssueDescription;
                    document.getElementById("txtFaultlogIssueModuleTested").value   = this.ApplicationModule;
                    document.getElementById("txtFaultlogActionModule").value        = this.ApplicationModule;
                    document.getElementById("txtFaultlogIssueBranch").value         = br;
                    document.getElementById("txtFaultlogIssuePreparedBy").value     = name;
                    document.getElementById("ddlFaultlogIssueStatus").selectedIndex = this.Status; 
                    document.getElementById("ddlFaultlogActionStatus").selectedIndex = this.Status;
                    if(this.Remarks == null || this.Remarks == "undefined")
                    {
                        document.getElementById("txtFaultlogAction").value = "";
                    }
                    else
                    {
                        document.getElementById("txtFaultlogAction").value              = this.Remarks;
                    }      

                   this.commons = new common_method();
                   this.commons.to_read_only("tblFaultlogAction", true);
                   document.getElementById("txtFaultlogIssueFCLSNo").style.display = "block";
                   document.getElementById("txtIssueDescription").disabled = false; 
                   document.getElementById("txtFaultlogIssueModuleTested").disabled = false;
                   document.getElementById("ddlFaultlogIssueStatus").disabled = false; 
                   document.getElementById("txtFaultlogAction").disabled = false;
                   document.getElementById("ddlFaultlogActionStatus").disabled = false; 
                   document.getElementById("txtFaultlogActionFCLSNo").disabled = true;
                   document.getElementById("txtFaultlogActionDate").disabled = true;
                   document.getElementById("txtFaultlogActionAttendedBy").disabled = true;
                   document.getElementById("ScreenShot").disabled = false;
                                         
                   this.commons.to_unlock_button("tblFaultlogIssueButtons","btnFaultlogIssueEdit~btnFaultlogIssueCancel");
                   this.commons = null;
            }
        }
    }
    
    this.get_branch_name = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            br = result.value;     
        }
    
    }
 
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
        
        data_row[data_row.length]  = "IssueReference~"      + this.IssueReference; 
        data_row[data_row.length]  = "DateReported~"        + this.DateReported;     
        data_row[data_row.length]  = "IssueDescription~"    + this.IssueDescription;          
        data_row[data_row.length]  = "ApplicationModule~"   + this.ApplicationModule;          
        data_row[data_row.length]  = "BranchAgent~"         + code;          
        data_row[data_row.length]  = "TestedBy~"            + user_id;        
        data_row[data_row.length]  = "Remarks~"             + this.Remarks;        
        data_row[data_row.length]  = "Status~"              + this.Status;
        data_row[data_row.length]  = "AttendedBy~"          + user_id;  
        data_row[data_row.length]  = "DateAttended~"        + this.DateAttended;  
        data_row[data_row.length]  = "ScreenShot~"               + image;               
           
        return data_row.join("^");       
    }
    
    this.push_comment_data_to_server = function()    
    {    
        var data_row = new Array();
        
        data_row[data_row.length]  = "IssueReference~"      + this.IssueReference; 
        data_row[data_row.length]  = "DatePosted~"          + this.DatePosted;     
        data_row[data_row.length]  = "UserId~"              + user_id;          
        data_row[data_row.length]  = "Comment~"             + this.Comment;          
                     
        return data_row.join("^");       
    }
    
    this.insert_record = function()
    {
        IRSWorld.Faultlog.Faultlog.user_info(this.get_user);
        this.IssueReference = "";
        this.IssueReference = null;
        document.getElementById("txtFaultlogIssueFCLSNo").style.display = "none";
        this.add_data();
        this.dropdown_keyword();
        this.get_disabled();
        IRSWorld.Faultlog.Faultlog.insert_record(this.insert_record_callback);
    }
    
    this.insert_record_comment = function()
    {
        IRSWorld.Faultlog.Faultlog.user_info(this.get_user);
        this.add_data_comment();
        this.get_disabled();
        IRSWorld.Faultlog.Faultlog.insert_record_comment(this.insert_record_comment_callback);
    }
      
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Faultlog.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblFaultlog",true);                
            this.commons.to_unlock_button("tblFaultlogIssueButtons","btnFaultlogIssueSave~btnFaultlogIssueCancel");
            this.commons = null;            
        }
    }
    
    this.insert_record_comment_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Faultlog.read_message_comment(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblFaultlogComment",true);
            document.getElementById("txtFaultlogComment").disabled = false;                
            this.commons.to_unlock_button("tblFaultlogCommentButtons","btnFaultlogCommentSave~btnFaultlogCommentCancel");
            this.commons = null;            
        }
    }
    
    this.update_record = function()
    {
        IRSWorld.Faultlog.Faultlog.user_info(this.get_user);
        this.pull_server_data();
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.Faultlog.Faultlog.update_record(this.SEQ_ID, this.update_record_callback);
    } 
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Faultlog.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblFaultlog",false);                   
            this.commons.to_unlock_button("tblFaultlogIssueButtons","btnFaultlogIssueSave~btnFaultlogIssueCancel");
            this.commons = null;                                  
        }
    }
    
            
     
    this.commit_record = function(method)
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
            IRSWorld.Faultlog.Faultlog.commit_record(Faultlog_Status.set_faultlog(method),Faultlog_Status.commit_record_callback);
        }

    }
    
    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Faultlog.read_message(); alert(msg.value); return;
            }
             
            this.SEQ_ID = IRSWorld.Faultlog.Faultlog.FAULTLOG_SEQ_ID().value;
            document.getElementById("txtHiddenSeqID").value = this.SEQ_ID;
            if (this.IssueReference == null || this.IssueReference == "undefined")
            {
                this.IssueReference = "IRSW00" + this.SEQ_ID;
                IRSWorld.Faultlog.Faultlog.update_refno(this.SEQ_ID, this.IssueReference);
                this.IssueReference = null;
            }

            this.commons = new common_method();
            this.commons.to_read_only("tblFaultlogDefault",true);                          
            this.commons.to_unlock_button("tblFaultlogIssueButtons","btnFaultlogIssueEdit~btnFaultlogIssueCancel");
            this.commons.to_read_only("tblFaultlogAction",true);
            document.getElementById("txtIssueDescription").disabled = true;                          
            this.commons.to_unlock_button("tblFaultlogActionButtons","btnFaultlogActionEdit~btnFaultlogActionCancel");  
            this.commons = null;    
            
            alert("RECORD COMMITTED SUCCESSFULLY");
      
        }
        else
        {
            alert(result.error);
        }
    }
      
    this.rollback_record = function()
    {
        IRSWorld.Faultlog.Faultlog.rollback_record(this.rollback_record_callback);
        this.commons = new common_method();
        this.commons.to_clear_screen("tblFaultlog");
        this.commons.to_clear_screen("tblbuttons");
        this.commons.to_read_only("tblFaultlog",true);
        this.commons.to_unlock_button("tblbuttons","btnNew~btnCancel");
        this.commons = null;
        this.load_record();
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.Faultlog.Faultlog.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }                                             
    }
      
    this.clear = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblFaultlog");
       this.commons = null;           
    }
    
    this.add_data = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblFaultlogDefault");
        document.getElementById("txtIssueDescription").value = "";
        document.getElementById("txtIssueDescription").disabled = false;
        this.commons.to_read_only("tblFaultlogDefault",false);
        this.commons.to_unlock_button("tblFaultlogIssueButtons","btnFaultlogIssueSave~btnFaultlogIssueCancel");
        this.commons = null;
    }
    
    this.add_data_comment = function()
    {
        this.commons = new common_method();
        document.getElementById("txtFaultlogComment").value = "";
        this.commons.to_read_only("tblFaultlogComment",false);
        this.commons.to_unlock_button("tblFaultlogCommentButtons","btnFaultlogCommentSave~btnFaultlogCommentCancel");
        this.commons = null;
    }
    
    this.update_record_action = function()
    {
        IRSWorld.Faultlog.Faultlog.user_info(this.get_user);
        this.pull_server_data();
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID2").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.Faultlog.Faultlog.update_record(this.SEQ_ID, this.update_record_callback_action);
    } 
    
    this.update_record_callback_action = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Faultlog.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblFaultlogAction",false);
            document.getElementById("txtFaultlogComment").disabled = false;                       
            this.commons.to_unlock_button("tblFaultlogActionButtons","btnFaultlogActionSave~btnFaultlogActionCancel");
            this.commons = null;                                  
        }
    }
    
            
     
    this.commit_record_comment = function()
    {
        if (confirm("Are you sure you want to save this record?"))
        {
            IRSWorld.Faultlog.Faultlog.commit_record_comment(this.set_faultlog_comment(),this.commit_record_callback_comment);
        }

    }

    this.commit_record_callback_comment= function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Faultlog.read_message_comment(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.Faultlog.Faultlog.FAULTLOG_COMMENT_SEQ_ID().value;
            document.getElementById("txtHiddenSeqID3").value = this.SEQ_ID;
            
            this.commons = new common_method();
            this.commons.to_read_only("tblFaultlogComment",true);                          
            this.commons.to_unlock_button("tblFaultlogCommentButtons","btnFaultlogCommentEdit~btnFaultlogCommentCancel"); 
            this.commons = null;    
            
            alert("RECORD COMMITTED SUCCESSFULLY");
      
        }
    }
    
    this.update_record_comment = function()
    {
        IRSWorld.Faultlog.Faultlog.user_info(this.get_user);
        this.pull_server_data_comment();
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID3").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }
        
        IRSWorld.Faultlog.Faultlog.update_record_comment(this.SEQ_ID, this.update_record_callback_comment);
    } 
    
    this.update_record_callback_comment = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.Faultlog.Faultlog.read_message_comment();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblFaultlogComment",true);
                                   
            this.commons.to_unlock_button("tblFaultlogCommentButtons","btnFaultlogCommentSave~btnFaultlogCommentCancel");
            this.commons = null;                                  
        }
    }
    
    this.pull_server_data_comment = function()
    {             
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.Faultlog.Faultlog.FAULTLOG_COMMENT_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.Faultlog.Faultlog.GET_FAULTLOG_COMMENT_RECORD(this.SEQ_ID,this.pull_server_data_callback_comment);
    }   
    
     
    this.pull_server_data_callback_comment = function(result)
    {        
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
            
                    this.IssueReference         = ds.Tables[0].Rows[0].IssueReference;
                    this.commons                = new common_method();
                    var dd                      = this.commons.to_mmddyyyy_date(ds.Tables[0].Rows[0].DatePosted);
                    var tt                      = ds.Tables[0].Rows[0].DatePosted.toLocaleTimeString();
                    this.DatePosted             = dd + " " + tt ;
                    this.UserId                 = ds.Tables[0].Rows[0].UserId;
                    this.Comment                = ds.Tables[0].Rows[0].Comment;
                                     
                    document.getElementById("txtHiddenSeqID3").value                = this.SEQ_ID;
                    document.getElementById("txtFaultlogCommentFCLSNo").value       = this.IssueReference;
                    document.getElementById("txtFaultlogCommentDate").value   = this.DatePosted;
                    document.getElementById("txtFaultlogComment").value             = this.Comment;
                    document.getElementById("txtFaultlogCommentUser").value       = this.UserId;
                   
                    this.commons = new common_method();
                    this.commons.to_read_only("tblFaultlogComment", true);
                    document.getElementById("txtFaultlogComment").disabled = false;                       
                    this.commons.to_unlock_button("tblFaultlogCommentButtons","btnFaultlogCommentSave~btnFaultlogCommentCancel");
                    this.commons = null;
            }
        }
    }
    
    this.upload_scrshot = function()
    {  
//            IRSWorld.Faultlog.Faultlog.upload_scrshot(this.get_path);
//         document.getElementById('btnUpload').Visible = "true";
         var btn = document.getElementById('ScreenShot').value;
//         IRSWorld.Faultlog.Faultlog.upload_scrshot(document.getElementById('ScreenShot').value);
         document.getElementById('btnUpload').disabled = false;
         document.getElementById('btnUpload').click();
//        IRSWorld.Faultlog.Faultlog.upload_scrshot(document.getElemen.tById("ScreenShot").value, this.get_path);
//         document.getElementById('frameupload').onload();
         image = IRSWorld.Faultlog.Faultlog.get_finalPath();
         
    }
    
    this.get_path = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            var scrshot_path = result.value.split("~");
            if(scrshot_path[0] == "Error")
            {
                alert(result.value);
            }
            else
            {
                image = result.value;
            }     
        }
        else
        {
            alert("Unable to upload Screenshot!");
        }
    }
    
    this.validate_input = function()
    {
        this.error_input = "";
        
        if(this.generic.is_empty(document.getElementById("txtFaultlogIssueModuleTested").value) == true)
	    {
	        this.error_input += "Module is mandatory.\n";
	    }
	    if(this.generic.is_empty(document.getElementById("ddlFaultlogIssueStatus").value) == true)
	    {
	        this.error_input += "Status is mandatory.\n";
	    }
	    if(this.generic.is_empty(document.getElementById("txtIssueDescription").value) == true)
	    {
	        this.error_input += "Issue Description is mandatory.\n";
	    }

	    
	    
        if(this.generic.is_empty(this.error_input) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    this.download_file = function()
    {
        fname = path;
        if (fname.length > 0)
        {
            IRSWorld.Faultlog.Faultlog.download_file(fname, this.download_file_callback);
        }
        
        alert("Your download will start shortly. Pls wait...");
    }
    
    this.download_file_callback = function(result)
    {
        var dl_pop = window.open("../Faultlog/FaultlogDL.aspx", "dl_pop","location=1,status=1,scrollbars=1, width=1,height=1");
        dl_pop.moveTo(0,0);
    }
    
}

