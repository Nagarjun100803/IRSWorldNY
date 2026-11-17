function execute_ajx_command(param)
{
    if (compliance_viewer != null && compliance_viewer != "undefined")
    {
        compliance_viewer.do_ajax(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (compliance_viewer != null && compliance_viewer != "undefined")
    {
        compliance_viewer.do_ajaxgridview(param);
    }
}

function compliance_viewer()
{
    this.commons            = null;
    this.generic            = null;
	this.SEQ_ID             = null;	
	this.FileFormat	   	    = null;
	this.TrackingNo 	    = null;
	this.RecordID		    = null;
	this.Title		        = null;  
	this.LastName		    = null;
	this.FirstName		    = null;
	this.MiddleName	        = null;
	this.Suffix		        = null;
	this.AliasLastName	    = null;
	this.AliasFirstName	    = null;
	this.AliasMiddleName	= null;
	this.Position		    = null;
	this.DateOfBirth	    = null;
	this.TownOfBirth	    = null;
	this.CountryOfBirth	    = null;
	this.Nationality	    = null;
	this.IDNumber		    = null;
	this.IDType		        = null;
	this.Street		        = null;
	this.City		        = null;
	this.State		        = null;
	this.Zip		        = null;
	this.Country		    = null;
	this.PhoneNo	        = null;
	this.HomeNo		        = null;
	this.BusinessName	    = null;
	this.BusinessPhoneNo	= null;
	this.DBAName		    = null;
	this.OtherInfo		    = null;
	this.GroupType		    = null;
	this.GroupID	  	    = null;
	this.AliasType		    = null;
	this.Regime		        = null;
	this.ListedOn		    = null;
	this.LastUpdate		    = null;
	this.PassportDetails	= null;
	this.Remarks		    = null;
	this.IsDeleted          = null;
	this.DateLastUpdated	= null;
	this.grid_ajax_query    = null;
	this.message            = null;
	this.grid               = null;   
    this.grid_ajax_page           = "../AjxPage/ajaxgridview.aspx"; 
    this.ajax_grid                = "GRIDCLASSADD";
    this.grid_ajax_query          = null;
    this.grid_ajax_page           = "../AjxPage/ajaxgrid.aspx";
    
    this.BranchCode         = null;

	
	this.load_grid_viewer = function()
    {     
        this.grid = new grid_viewer();    
        this.grid.session_end = true;
         
        if (document.getElementById("ddFileFormat").value == 'null')
        {
                //this.grid.sql_source     = "sp_compliance~" + "~" + null;
                this.grid.sql_source     = "sp_compliance~" + null + "~" + this.BranchCode.value;
        }
        else
        {
                //this.grid.sql_source     = "sp_compliance~" + document.getElementById("ddFileFormat").value + "~";
                this.grid.sql_source     = "sp_compliance~" + document.getElementById("ddFileFormat").value + "~" + this.BranchCode.value;
        }
        this.grid.div_container  = document.getElementById("ComplianceGrid");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_compliance~";
        this.grid.div_container  = document.getElementById("ComplianceGrid");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
	    
    this.getBranchCode = function()
    {
        this.BranchCode = IRSWorld.IRSFileLoader.ComplianceViewer.getBranchCode();
    }
    
	this.load_all_dropdownlist = function()
    {
        this.getBranchCode();
        this.dropdown_filetype();   
        this.dropdown_searchby();      
    }
    
    this.dropdown_filetype = function()
    {
        IRSWorld.IRSFileLoader.ComplianceViewer.getdropdown_source(this.BranchCode.value, this.dropdown_filetype_callback);
    }
    
    this.dropdown_filetype_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddFileFormat", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    }
	
	this.dropdown_searchby = function()
    {
        IRSWorld.IRSFileLoader.ComplianceViewer.getsearchby_source(this.dropdown_searchby_callback);
    }
    
    this.dropdown_searchby_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddSearchBy", result.value, "fldvalue", "fldcode");
            this.commons = null;
        }
    }
    
	this.load_record = function()
	{                     
        this.pull_server_data();  
        this.load_grid_viewer();
	}
	
    this.pull_server_data = function()
    {     
        this.hide_grid();
          
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.IRSFileLoader.ComplianceViewer.COMPLIANCE_VIEWER_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.IRSFileLoader.ComplianceViewer.GET_COMPLIANCE_VIEWER_RECORD(this.SEQ_ID,this.pull_server_data_callback);
        
    }      	
	
	this.pull_server_data_callback = function(result)
    {      
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    this.FileFormat	   	     = ds.Tables[0].Rows[0].FileFormat;
                    this.TrackingNo 	     = ds.Tables[0].Rows[0].TrackingNo;
                    this.RecordID		     = ds.Tables[0].Rows[0].RecordID;
                    this.Title		         = ds.Tables[0].Rows[0].Title;
                    this.LastName		     = ds.Tables[0].Rows[0].LastName;
                    this.FirstName		     = ds.Tables[0].Rows[0].FirstName;
                    this.MiddleName		     = ds.Tables[0].Rows[0].MiddleName;
                    this.MiddleName	         = ds.Tables[0].Rows[0].MiddleName;
                    this.Suffix		         = ds.Tables[0].Rows[0].Suffix;
                    this.AliasLastName	     = ds.Tables[0].Rows[0].AliasLastName;
                    this.AliasFirstName	     = ds.Tables[0].Rows[0].AliasFirstName;
                    this.AliasMiddleName	 = ds.Tables[0].Rows[0].AliasMiddleName;
                    this.Position		     = ds.Tables[0].Rows[0].Position;
                    this.DateOfBirth	     = ds.Tables[0].Rows[0].DateOfBirth;
                    this.TownOfBirth         = ds.Tables[0].Rows[0].TownOfBirth;                 
                    this.CountryOfBirth	     = ds.Tables[0].Rows[0].CountryOfBirth;
                    this.Nationality	     = ds.Tables[0].Rows[0].Nationality;
                    this.IDNumber		     = ds.Tables[0].Rows[0].IDNumber;
                    this.IDType		         = ds.Tables[0].Rows[0].IDType;
                    this.Street		         = ds.Tables[0].Rows[0].Street;
                    this.City		         = ds.Tables[0].Rows[0].City;
                    this.State		         = ds.Tables[0].Rows[0].State;
                    this.Zip		 		 = ds.Tables[0].Rows[0].Zip;
                    this.Country		     = ds.Tables[0].Rows[0].Country;
                    this.PhoneNo	         = ds.Tables[0].Rows[0].PhoneNo;
                    this.HomeNo		 		 = ds.Tables[0].Rows[0].HomeNo;
                    this.BusinessName	     = ds.Tables[0].Rows[0].BusinessName;
                    this.BusinessPhoneNo     = ds.Tables[0].Rows[0].BusinessPhoneNo;
                    this.DBAName		     = ds.Tables[0].Rows[0].DBAName;
                    this.OtherInfo		 	 = ds.Tables[0].Rows[0].OtherInfo;
                    this.GroupType		     = ds.Tables[0].Rows[0].GroupType;
                    this.GroupID		     = ds.Tables[0].Rows[0].GroupID;
                    this.AliasType		     = ds.Tables[0].Rows[0].AliasType;
                    this.Regime		         = ds.Tables[0].Rows[0].Regime;
                    this.ListedOn	         = ds.Tables[0].Rows[0].ListedOn;
                    this.LastUpdate		 	 = ds.Tables[0].Rows[0].LastUpdate;
                    this.PassportDetails     = ds.Tables[0].Rows[0].PassportDetails;
                    this.Remarks		     = ds.Tables[0].Rows[0].Remarks;
                    this.DateLastUpdated	 = ds.Tables[0].Rows[0].DateLastUpdated;
                    this.IsDeleted           = ds.Tables[0].Rows[0].IsDeleted;
                    
                    this.commons = new common_method();
                    this.generic = new generic_method();
                    document.getElementById("txtHiddenSeqID").value         = this.SEQ_ID;
                    document.getElementById("txtFullName").value            = this.LastName + ',' + ' ' + this.FirstName + ' ' + this.MiddleName;
                    document.getElementById("txtLastName").value            = this.LastName;
                    document.getElementById("txtFirstName").value           = this.FirstName;
                    document.getElementById("txtMiddleInitial").value       = this.MiddleName;
                    document.getElementById("txtAliasLName").value          = this.AliasLastName;
                    document.getElementById("txtAliasFName").value          = this.AliasFirstName;
                    document.getElementById("txtAliasMName").value          = this.AliasMiddleName;
                    document.getElementById("txtSuffix").value              = this.Suffix;
                    //document.getElementById("txtAliasSuffix").value          = this.SEQ_ID;
                    document.getElementById("txtBusinessName").value        = this.BusinessName;
                    document.getElementById("txtBusinessName1").value        = this.BusinessName;
                    document.getElementById("txtTitle").value               = this.Title;
                    document.getElementById("txtOther").value               = this.OtherInfo;
                    document.getElementById("txtDateofBirth").value         = this.commons.to_mmddyyyy_date(this.DateOfBirth);
                    document.getElementById("txtAge").value                 = this.generic.compute_age(document.getElementById("txtDateofBirth").value); 
                    document.getElementById("txtTownofBirth").value         = this.TownOfBirth;
                    document.getElementById("txtCountryofBirth").value      = this.CountryOfBirth;
                    document.getElementById("txtNationality").value         = this.Nationality;
                    document.getElementById("txtPosition").value            = this.Position;
                    //document.getElementById("txtHomeNumber").value          = this.HomeNo;
                    document.getElementById("txtListedOn").value            = this.commons.to_mmddyyyy_date(this.ListedOn);
                    document.getElementById("txtDays").value                = this.generic.days_passed(document.getElementById("txtListedOn").value);
                    document.getElementById("txtLastUpdated").value         = this.commons.to_mmddyyyy_date(this.LastUpdate);
                    document.getElementById("txtDbaName").value             = this.DBAName;
                    document.getElementById("txtPhoneNumber").value         = this.PhoneNo;
                    document.getElementById("txtCity").value                = this.City;
                    document.getElementById("txtState").value               = this.State;
                    document.getElementById("txtZipCode").value             = this.Zip;
                    document.getElementById("txtCountry").value             = this.Country;
                    document.getElementById("txtRemarks").value             = this.Remarks;
                    document.getElementById("txtGroupType").value           = this.GroupType;
                    document.getElementById("txtAliasType").value           = this.AliasType;
                    document.getElementById("txtRegime").value              = this.Regime;
                    document.getElementById("txtGroupId").value             = this.GroupID;
                    document.getElementById("txtPassport").value            = this.PassportDetails;
                    document.getElementById("txtRegulatory").value          = this.FileFormat;
                    
                   if (document.getElementById("txtRegulatory").value != null && document.getElementById("txtRegulatory").value == '5')
                   {
                       document.getElementById("divName").style.display             = "none";
                       document.getElementById("divBusName").style.display          = "block";
                   }
                   else
                   {
                       document.getElementById("divName").style.display             = "block";
                       document.getElementById("divBusName").style.display          = "none";
                   }
                   this.generic.to_clear_null("txtFullName~txtLastName~txtAliasLName~txtAliasFName~txtFirstName~txtAliasMName~txtSuffix~txtAliasSuffix~txtTitle~txtNationality~txtMiddleInitial~txtDateofBirth~txtTownofBirth~txtCountryofBirth~txtPosition~txtBusinessName~txtOther~txtCity~txtState~txtCountry~txtZipCode~txtPhoneNumber~txtPassport~txtDbaName~txtGroupType~txtAliasType~txtRegime~txtGroupId~txtRegulatory~txtRemarks~");
                   this.commons = null;
                   this.generic = null;                    
             }             
        }         
    }    
    
    this.set_compliance_viewer = function()
    {   
        this.commons = new common_method();
        this.LastUpdate        = get_current_date_string();
        this.commons = null;
        this.IsDeleted = false;
          
        if (document.getElementById("chkCleared").value != null && document.getElementById("chkCleared").checked == true)
        {
            this.IsDeleted = true;
        }
           
        return this.push_data_to_server();
    }
    
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
             
          data_row[data_row.length]  = "LastUpdate~"       + this.LastUpdate;
          data_row[data_row.length]  = "IsDeleted~"       + this.IsDeleted;   
                              
        return data_row.join("|");       
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
        
        IRSWorld.IRSFileLoader.ComplianceViewer.update_record(this.SEQ_ID, this.update_record_callback);
    } 
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.IRSFileLoader.ComplianceViewer.read_message();
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
            IRSWorld.IRSFileLoader.ComplianceViewer.commit_record(this.set_compliance_viewer(),this.commit_record_callback);
    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.IRSFileLoader.ComplianceViewer.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.IRSFileLoader.ComplianceViewer.COMPLIANCE_VIEWER_SEQ_ID().value;
            this.commons = new common_method();                        
            this.commons.to_unlock_button("tblbuttons2","btnEdit~btnBack~btnCancel"); 
            this.commons.to_read_only("tblClear",true);
            this.commons = null;    
            alert("RECORD COMITTED SUCCESSFULLY");     
            compliance_viewer.load_grid_viewer();              
        }
    }
    
    this.rollback_record = function()
    {
        IRSWorld.IRSFileLoader.ComplianceViewer.rollback_record(this.rollback_record_callback);
        this.commons = new common_method();
        this.commons.to_unlock_button("tblbuttons2","btnEdit~btnBack");
        this.commons = null;
        this.hide_panel();
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.IRSFileLoader.ComplianceViewer.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }                                             
    }
      
                 
    this.do_ajax = function(parameter)
    {
        this.ajax_grid.process_ajax(parameter);
        
       var command_param = null;
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID").value = command_param[1];
        
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                }
            }
        }

    }
    
    this.do_ajaxgridview = function(parameter)
    {        
        var command_param = null;
       
       //this.ajax_grid.process_ajax(parameter); 07/09/2013 remove
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenSeqID").value = command_param[1];
                
                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                }
                else
                {
                    this.process_grid_viewer(parameter);
                }
            }
        }
    }
    
    this.panel_search_compliance = function()
    {        
        this.ajax_grid = new grid_selector();
        
        //by fileformat searching only
        if (           
            document.getElementById("ddSearchBy").value == 'null' &&
            document.getElementById("ddEqualorLike").value == '' &&
            document.getElementById("ddFileFormat").value != 'null' &&
            document.getElementById("txtSearchBy").value == ''         
            )
            {
                this.grid_ajax_query = "sp_compliance_search_by_fileformat~" +
                                        document.getElementById("ddFileFormat").value + "~" +
                                        this.BranchCode.value + "~";          
                this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.SEQ_ID );   
            }
        else if (document.getElementById("txtSearchBy").value != '')
                {   
                    if (document.getElementById("ddEqualorLike").value == '')
                    {   
                        alert ("Please select operator for searching");
                        document.getElementById("ddEqualorLike").focus();
                        return;
                    }                    
                    else //search string
                    {
                        //combination with fileformat and search criteria
                        if (document.getElementById("ddFileFormat").value != 'null' &&
                            document.getElementById("ddSearchBy").value != 'null')
                            {
                                this.grid_ajax_query = "sp_compliance_full_search~" +
                                                        document.getElementById("ddFileFormat").value + "~" +
                                                        document.getElementById("ddSearchBy").value + "~" +
                                                        document.getElementById("txtSearchBy").value + "~" +
                                                        this.BranchCode.value + "~" +
                                                        document.getElementById("ddEqualorLike").value + "~";
                                this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.SEQ_ID );
                            }
                        //combination with seach criteria only
                        else if (document.getElementById("ddFileFormat").value == 'null' &&
                                document.getElementById("ddSearchBy").value != 'null')
                                {
                                    this.grid_ajax_query = "sp_compliance_search_by_filter~" +
                                                            document.getElementById("ddSearchBy").value + "~" +
                                                            document.getElementById("txtSearchBy").value + "~" +
                                                            this.BranchCode.value + "~" + 
                                                            document.getElementById("ddEqualorLike").value + "~";
                                    this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.SEQ_ID );
                                }
                        //search in any fields of tbl_unifiedwatchlist
                        else if (document.getElementById("ddSearchBy").value == 'null')
                                {   //any fileformat
                                    if (document.getElementById("ddFileFormat").value == 'null')
                                        {   
                                            this.grid_ajax_query = "sp_compliance_search_columns_by_any_fileformat~" +
                                                                    document.getElementById("txtSearchBy").value + "~" +
                                                                    this.BranchCode.value + "~" + 
                                                                    document.getElementById("ddEqualorLike").value + "~";
                                            this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.SEQ_ID );
                                        }
                                    //by fileformat
                                    else if (document.getElementById("ddFileFormat").value != 'null')
                                        {   
                                            this.grid_ajax_query = "sp_compliance_search_columns_by_fileformat~" +                                            
                                                                    document.getElementById("ddFileFormat").value + "~" +
                                                                    document.getElementById("txtSearchBy").value + "~" +
                                                                    this.BranchCode.value + "~" + 
                                                                    document.getElementById("ddEqualorLike").value + "~";
                                            this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.SEQ_ID );                                        
                                        }
                                }
                    }
                }
        else if (           
            document.getElementById("ddSearchBy").value == 'null' &&
            document.getElementById("ddEqualorLike").value == '' &&
            document.getElementById("ddFileFormat").value == 'null' &&
            document.getElementById("txtSearchBy").value == ''         
            )
            {
                alert ("Search should be by fileformat, search criteria or combination");
                return;
            }
    }   
    
    
    this.hide_panel = function()
    {
        document.getElementById("ComplianceEntry").style.display     = "none"; 
        document.getElementById("ComplianceBtn").style.display       = "block";
        document.getElementById("ComplianceBtn2").style.display      = "none";
        document.getElementById("ComplianceGrid").style.display      = "none";
        document.getElementById("divName").style.display             = "none";
        document.getElementById("divBusName").style.display          = "none";
        //document.getElementById("HitViewer").style.display           = "none";
        
    }
    
//    this.compliance_panel = function()
//    {
//        document.getElementById("divComplianceViewer").style.display    = "block";
//        //document.getElementById("HitViewer").style.display           = "none";
//    }
//    
//    this.hit_panel = function()
//    {
//        document.getElementById("divComplianceViewer").style.display    = "none";
//        //document.getElementById("HitViewer").style.display           = "block";
//    }
    
    this.hide_grid = function()
    {
        document.getElementById("ComplianceEntry").style.display    = "block"; 
        document.getElementById("ComplianceBtn").style.display      = "none"; 
        document.getElementById("ComplianceGrid").style.display     = "block";
        document.getElementById("ComplianceBtn2").style.display     = "block"; 
        this.commons = new common_method();
        this.commons.to_read_only("tblCompliance1",true);  
        this.commons.to_read_only("tblCompliance2",true);  
        this.commons.to_read_only("tblCompliance3",true);  
        this.commons.to_read_only("tblCompliance4",true);  
        this.commons.to_read_only("tblCompliance5",true);  
        this.commons.to_read_only("tblCompliance6",true);
        this.commons.to_read_only("tblCompliance7",true);
//        this.commons.to_read_only("tblClear",true); 
        this.commons = null;    
    }

   
}


