
function execute_ajaxgridview_command(param)
{
    if (hit_viewer != null && hit_viewer != "undefined")
    {
        hit_viewer.do_ajaxgridview(param);
    }
}

function hit_viewer()
{
    this.commons                = null;
    this.generic                = null;
	this.SEQ_ID                 = null;	
	this.grid_ajax_query        = null;
	this.message                = null;
	this.grid                   = null;
	this.FullnameCif            = null;
	this.FullnameUni            = null;
	this.AccountNo              = null;
    this.grid_ajax_page         = "../AjxPage/ajaxgridview.aspx"; 
    this.ajax_grid              = "GRIDCLASSADD";
    this.grid_ajax_query        = null;
    this.grid_ajax_page         = "../AjxPage/ajaxgrid.aspx";
    this.TotalCountUnconverted  = null; 
    this.BranchCode         = null;
    
    
    
    this.load_grid_viewer = function()
    {     
        this.load_percent_scr();
        //this.BranchCode = IRSWorld.IRSFileLoader.HitViewer.getBranchCode();
        this.BranchCode = IRSWorld.IRSFileLoader.HitViewer.getLocBranch();        
        
        this.grid = new grid_viewer();    
        //this.grid.session_end = true;
        this.grid.sql_source = "sp_hitlist~" + this.BranchCode.value + "~";
        this.grid.div_container  = document.getElementById("hitgrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
        this.commons = new common_method();
        this.commons.to_read_only("tblClear",true);
    }  
    
    this.searchHitlist = function()
    {
        var isExactMatch;
        this.BranchCode = IRSWorld.IRSFileLoader.HitViewer.getBranchCode();
          
        if (document.getElementById("chkExactmatch").checked == true)
        {
            isExactMatch = true;
        }
        else
        {
            isExactMatch = false;
        }
        
        this.grid = new grid_viewer();  
        if (document.getElementById("ddFileFormat").value == "27")
        {
        //this.grid.sql_source     =  "proc_accthitlist_search~" + this.BranchCode.value + "~" + document.getElementById("ddFileFormat").value + "~" + isExactMatch;
        this.grid.sql_source     =  "proc_accthitlist_search~" + document.getElementById("ddFileFormat").value + "~" + isExactMatch;
        }
        else
        {
        this.grid.sql_source     =  "sp_hitlist_search~" + this.BranchCode.value + "~" + document.getElementById("ddFileFormat").value + "~" + isExactMatch;
        }
        //this.grid.sql_source     =  "sp_hitlist~" + this.BranchCode.value + "~";
        this.grid.div_container  = document.getElementById("hitgrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
        this.commons = new common_method();
        this.commons.to_read_only("tblClear",true); 
    }
    
    this.refresh_table = function()
    {
        IRSWorld.IRSFileLoader.HitViewer.refresh_table();
    }    
    
    this.refresh_table_callback = function (result)
    {
        if (result != null || result.value != null || result.value != "0")
        {
         
        }
    }
        
    /*this.count_not_ConvertedCIF = function()
    {
        IRSWorld.IRSFileLoader.HitViewer.count_not_ConvertedCIF(this.count_not_ConvertedCIF_callback);
    }  */  
    
    this.count_not_ConvertedCIF_callback = function (result)
    {
        if (result != null || result.value != null || result.value != "0")
        {
            document.getElementById("txtCIFNoToConvert").disabled = false;
            document.getElementById("txtTotalNotConvertedCIF").value = result.value;
            document.getElementById("btnConvertPhonetics").disabled = false;
            this.TotalCountUnconverted = result.value;
        }        
        else
        {                
            document.getElementById("txtCIFNoToConvert").disabled = true;
            document.getElementById("btnConvertPhonetics").disabled = true;
        }
    }
    
    this.convert_phonetics = function()
    {
    
        var noToConvert
        noToConvert = document.getElementById("txtCIFNoToConvert").value;
        
        if (trim(noToConvert) != "")
        {           
            if (Number(noToConvert)> 10000)
            {
                alert ("10,000 is the maximum allowed numbers of CIF to convert");
                document.getElementById("txtCIFNoToConvert").value = "";
                document.getElementById("txtCIFNoToConvert").disabled = false;
                document.getElementById("txtCIFNoToConvert").focus();
                return;
            }
            
            AjaxPro.timeoutPeriod = 10000 * 1000;        
            IRSWorld.IRSFileLoader.HitViewer.convert_CIF_phonetics(noToConvert, this.convert_phonetics_callback);

            new common_method().to_show_loading_message("divLoadingMsg");
            setText(document.getElementById("lblLoadingMsg"), "Processing");
            showpanel("divPopProcess");
        }
        else
        {
            alert ("Input numbers of CIF to convert its corresponding phonetics");
            document.getElementById("txtCIFNoToConvert").disabled = false;            
            document.getElementById( "txtCIFNoToConvert" ).focus();
        }
    }  
    
    this.load_percent_scr = function()
    {
        IRSWorld.IRSFileLoader.HitViewer.get_percentage_match_setup(this.load_percent_scr_callback);
    }
    
    this.load_percent_scr_callback = function(result)
    {
        document.getElementById("divPercentageScore").style.display = "none";
    
        if (result != null && result.value != '')
        {
            var return_value = result.value.split('|');
            
            if (return_value[0] == "true")
            {        
                document.getElementById("divPercentageScore").style.display = "block";
                new common_method().to_set_dropdownlist_index("ddlPercentageScore", return_value[1]);
            }
        }
    }
    
    this.convert_phonetics_callback = function (result)
    {
        if (result.value > 0)
        {
            var intTotalCountUnconverted
            intTotalCountUnconverted = IRSWorld.IRSFileLoader.HitViewer.count_not_ConvertedCIF();
            this.TotalCountUnconverted = intTotalCountUnconverted.value;
            alert ("Successfully converted " + result.value + " CIF");
            document.getElementById("txtCIFNoToConvert").disabled = false;
            document.getElementById("txtCIFNoToConvert").value = "";
            document.getElementById("txtTotalNotConvertedCIF").value = this.TotalCountUnconverted;
        }
        hidepanel("divPopProcess");
    }
    
    this.process_grid_viewer = function(parameter)
    {
        //this.BranchCode = IRSWorld.IRSFileLoader.HitViewer.getBranchCode();
        this.BranchCode = IRSWorld.IRSFileLoader.HitViewer.getLocBranch();        
    
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_hitlist~" + this.BranchCode.value + "~";
        this.grid.div_container  = document.getElementById("hitgrid");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
        
    }
    this.load_record = function()
	{   
	    if (document.getElementById("ddFileFormat").value == "27")
	    {
	        this.pull_server_data_acct();  
	    }
	    else
	    { 
            this.pull_server_data();  
        }
	}
	
	this.pull_server_data = function()
    {     
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.IRSFileLoader.HitViewer.HIT_VIEWER_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      

        IRSWorld.IRSFileLoader.HitViewer.GET_HIT_VIEWER_RECORD(this.SEQ_ID,this.pull_server_data_callback);
        
    }      	
	
	this.pull_server_data_callback = function(result)
    {      
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                   this.SEQ_ID              = ds.Tables[0].Rows[0].SEQ_ID;
                   this.isDeleted           = ds.Tables[0].Rows[0].isDeleted;
                   this.FullnameCif         = ds.Tables[0].Rows[0].CIFField;
                   this.FullnameUni         = ds.Tables[0].Rows[0].UWField;
                   
                   document.getElementById("txtHiddenSeqID2").value = this.SEQ_ID;
                   document.getElementById("txtCif").value          = this.FullnameCif;
                   document.getElementById("txtUni").value          = this.FullnameUni;
                   document.getElementById("chkCleared").value      = "off";
                   document.getElementById("chkCleared").checked    = false;
                   
                   if (document.getElementById("chkCleared").value != null && this.isDeleted == true)
                   {
                        document.getElementById("chkCleared").value = "on";
                        document.getElementById("chkCleared").checked = true;
                   }
             }
        }
    } 
    
    this.pull_server_data_acct = function()
    {     

        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.IRSFileLoader.HitViewer.HIT_VIEWER_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }      

        IRSWorld.IRSFileLoader.HitViewer.GET_HIT_VIEWER_RECORD_ACCT(this.SEQ_ID,this.pull_server_data_acct_callback);
        
    }      	
	
	this.pull_server_data_acct_callback = function(result)
    {      
        if (result != null && result.value != null)
        {
            var ds = result.value;

            if (ds.Tables[0].Rows.length > 0)
            {
                   this.SEQ_ID              = ds.Tables[0].Rows[0].SEQ_ID;
                   this.isDeleted           = ds.Tables[0].Rows[0].Status;
                   this.AccountNo           = ds.Tables[0].Rows[0].AccountNo;
//                   this.FullnameCif         = ds.Tables[0].Rows[0].CIFField;
//                   this.FullnameUni         = ds.Tables[0].Rows[0].UWField;
                   
                   document.getElementById("txtHiddenSeqID2").value = ds.Tables[0].Rows[0].SEQ_ID;
                   document.getElementById("txtCif").value          = ds.Tables[0].Rows[0].AccountNo;
                   document.getElementById("txtUni").value          = ds.Tables[0].Rows[0].AccountNo;
                   document.getElementById("chkCleared").value      = "off";
                   document.getElementById("chkCleared").checked    = false;
                
                   
                   if (document.getElementById("chkCleared").value != null && ds.Tables[0].Rows[0].IsDeleted == true)
                   {
                        document.getElementById("chkCleared").value = "on";
                        document.getElementById("chkCleared").checked = true;
                   }
             }
        }
    } 
    
    
    this.set_hit_viewer = function()
    {   
        this.isDeleted = false;
          
        if (document.getElementById("chkCleared").value != null && document.getElementById("chkCleared").checked == true)
        {
            this.isDeleted = true;
        }
           
        return this.push_data_to_server();
    }

    this.getBranchCode = function()
    {
        this.BranchCode = IRSWorld.IRSFileLoader.HitViewer.getBranchCode();
    }
    
    this.dropdown_FileFormat = function()
    {
        IRSWorld.IRSFileLoader.HitViewer.getFileFormat(this.dropdown_FileFormat_callback);
    }    
    
    this.dropdown_FileFormat_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddFileFormat", result.value, "fldvalue", "fldcode");
            this.commons = null;
        }
    }
    
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
             
          //data_row[data_row.length]  = "LastUpdate~"       + this.LastUpdate;
          data_row[data_row.length]  = "isDeleted~"       + this.isDeleted;   
                              
        return data_row.join("|");       
    }      
    
    this.update_pscore_record = function()
    {
        if (this.PercScore == null)
        {
            this.PercScore = document.getElementById("ddlPercentageScore").value;
        }
        
        IRSWorld.IRSFileLoader.HitViewer.update_pscore_record(this.PercScore, this.update_pscore_record_callback);
    }
    
    this.update_pscore_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == true)
            {
                alert("RECORD COMMITTED SUCCESSFULLY.");return;
                hit_viewer.load_grid_viewer();
            }                                                    
        }
    }
    
    this.update_record = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID2").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("RECORD NOT FOUND.");return;
        }

        if (document.getElementById("ddFileFormat").value == "27")
        {
            IRSWorld.IRSFileLoader.HitViewer.update_record_acct(this.SEQ_ID, this.update_record_callback);
        }
        else
        {         
            IRSWorld.IRSFileLoader.HitViewer.update_record(this.SEQ_ID, this.update_record_callback);
        }

    } 
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.IRSFileLoader.HitViewer.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblClear",false);    
            this.commons.to_unlock_button("tblbuttons2","btnSave~btnCancel");
            this.commons = null;                                  
        }
    }
    
    this.commit_record = function()
    {
        if (document.getElementById("ddFileFormat").value == "27")
        {
            IRSWorld.IRSFileLoader.HitViewer.commit_record_acct(this.set_hit_viewer(),this.commit_record_callback);
        }
        else
        {
            IRSWorld.IRSFileLoader.HitViewer.commit_record(this.set_hit_viewer(),this.commit_record_callback);
        }
    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.IRSFileLoader.HitViewer.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.IRSFileLoader.HitViewer.HIT_VIEWER_SEQ_ID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons2","btnEdit~btnCancel"); 
            this.commons.to_read_only("tblClear",true);
            this.commons = null;    
            alert("RECORD COMITTED SUCCESSFULLY");     
            hit_viewer.load_grid_viewer(); 
             
        }
    }
    
    this.rollback_pscore_record = function()
    {
        this.load_percent_scr();
        hit_viewer.load_grid_viewer(); 
    }    
    
    this.rollback_record = function()
    {
        if (document.getElementById("ddFileFormat").value == "27")
        {
            IRSWorld.IRSFileLoader.HitViewer.rollback_record_acct(this.rollback_record_callback);
        }
        else
        {
            IRSWorld.IRSFileLoader.HitViewer.rollback_record(this.rollback_record_callback);
        }
        this.commons = new common_method();
        this.commons.to_unlock_button("tblbuttons2","btnEdit");
        this.commons = null;
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.IRSFileLoader.HitViewer.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }                                             
    }
    
    this.do_ajaxgridview = function(parameter)
    {        
        var command_param = null;
        //this.ajax_grid.process_ajax(parameter);
          
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID2").value = command_param[1];
                
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    this.commons = new common_method();
                    this.commons.to_unlock_button("tblbuttons2","btnEdit");
                    this.commons.to_read_only("tblClear",true);
                    this.commons = null;
                }
                else
                {
                    this.process_grid_viewer(parameter);
                }
            }
        }
    }
    
    this.check_alpha = function()
    {
        if (event.keyCode >= 48 && event.keyCode <= 57) 
        {
            event.returnValue = true;          
        }
        else
        {
            event.returnValue=false;
            alert("Please input only numbers");
        }
    } 
}