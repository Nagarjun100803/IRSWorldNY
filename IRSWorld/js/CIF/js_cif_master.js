function cif_master()
{
    this.commons            = null;
    this.generic            = null;
    this.SEQ_ID             = null;
    this.CIFID              = null;  
    this.CIFType            = null;  
    this.CIFClass           = null;  
    this.LastName           = null;  
    this.FirstName          = null;  
    this.MiddleName         = null;  
    this.Gender             = null;  
    this.Age                = null;  
    this.CivilStatus        = null;  
    this.Citizenship        = null;  
    this.BirthDate          = null;  
    this.BirthPlace         = null; 
    this.Remarks            = null;  
    this.DateCreated        = null;  
    this.DateModified       = null;  
    this.Deleted            = null;  
    this.CreatedBy          = null;  
    this.ModifiedBy         = null;  
    this.Title              = null;  
    this.Alias              = null;  
    this.BranchCode         = null;
    this.Religion           = null;
    this.FullName           = null; 
    this.message            = null;
    this.grid               = null;
    this.server_side_msg    = null;
    this.insert_flag        = null;   
    this.grid               = null; 
    this.dummy_class        = null;
    var iMaster             = this;

    this.call_server_side_message = function()
    {
        this.server_side_msg = IRSWorld.CIF.cifmain.read_message().value;
    }
    
    this.load_grid_viewer_cifclass = function()
    { 
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_get_all_cifclass~"
        this.grid.div_container  = document.getElementById("DIVAVAILCLASSGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.session_end = true;

        this.grid.sql_source     = "sp_cif_get_all_cifclass~";
        this.grid.div_container  = document.getElementById("DIVAVAILCLASSGRID");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;

        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.cancel_insert = function()
    {
        this.commons = new common_method();
        if ( this.CIFType == 1)
        {
            this.commons.to_clear_screen("tblCIFIndividual"); 
            this.commons.to_read_only("tblCIFIndividual", true);
        }
        else 
        {
            this.commons.to_clear_screen("tblUserBusiness"); 
            this.commons.to_read_only("tblUserBusiness", true);
        }
        this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {
        this.commons = new common_method();
        if ( this.CIFType == 1)
        {
            this.commons.to_clear_screen("tblCIFIndividual"); 
            this.commons.to_read_only("tblCIFIndividual", true);
        }
        else 
        {
            this.commons.to_clear_screen("tblUserBusiness"); 
            this.commons.to_read_only("tblUserBusiness", true);
        }
        
        this.commons = null;
        this.pull_server_data();
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {
        this.CIFType    = document.getElementById("txtHiddenCIFType").value;
            
        if (this.CIFType == 1)  
        {
            this.LastName       = document.getElementById("txtLastname").value; 
            this.FirstName      = document.getElementById("txtFirstname").value; 
            this.MiddleName     = document.getElementById("txtMiddlename").value; 
            this.Gender         = document.getElementById("ddGender").value; 
            this.Age            = document.getElementById("txtAge").value; 
            this.CivilStatus    = document.getElementById("ddCivilStatus").value; 
            this.Citizenship    = document.getElementById("ddCitizenship").value; 
            this.BirthDate      = document.getElementById("txtBirthday").value; 
            this.BirthPlace     = document.getElementById("txtBirthplace").value;        
            this.Remarks        = document.getElementById("txtIndRemarks").value;    
            this.Title          = document.getElementById("ddTitle").value;
            this.Alias          = document.getElementById("txtIndAlias").value; 
            this.Religion       = document.getElementById("ddReligion").value;
            this.BranchCode     = document.getElementById("txtHiddenCIFBrCode").value;
            
         }
         else
         {
            this.LastName       = document.getElementById("txtBusinessName").value; 
            this.FirstName      = document.getElementById("txtShortName").value; 
            this.MiddleName     = document.getElementById("txtNatureBusiness").value;
            this.Alias          = document.getElementById("txtBusAlias").value;
            this.Remarks        = document.getElementById("txtBusRemarks").value;
            this.BranchCode     = document.getElementById("txtHiddenCIFBrCode").value;
        }

        /* RFC-31620 */
        if (this.CIFClass == null)
        {
            this.CIFClass = this.get_cifclass();
        }
            
        this.set_created_modified();
         
        return this.push_data_to_server();        
    }
    
    this.pull_server_data = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.CIF.cifmain.get_seq_id().value;
        }
                
        if (this.SEQ_ID == null || this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }
        
        IRSWorld.CIF.cifmain.GET_MASTER_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
    this.pull_server_data_callback = function(result)
    {
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                this.commons = new common_method();
                this.generic = new generic_method();
                    
                if (iMaster.CIFType == 1)
                {
                    this.commons.to_clear_screen("tblCIFIndividual"); 
                    this.commons.to_read_only("tblCIFIndividual", true);
                }
                else 
                {
                    this.commons.to_clear_screen("tblUserBusiness"); 
                    this.commons.to_read_only("tblUserBusiness", true);
                }
            
                iMaster.SEQ_ID      = ds.Tables[0].Rows[0].SEQ_ID;
                iMaster.CIFType     = ds.Tables[0].Rows[0].CIFType;
                iMaster.CIFClass    = ds.Tables[0].Rows[0].CIFClass;
                iMaster.CIFID       = ds.Tables[0].Rows[0].CIFID;                
                iMaster.LastName    = ds.Tables[0].Rows[0].LastName;
                iMaster.FirstName   = ds.Tables[0].Rows[0].FirstName;
                iMaster.MiddleName  = ds.Tables[0].Rows[0].MiddleName;
                iMaster.Gender      = ds.Tables[0].Rows[0].Gender;
                iMaster.Age         = ds.Tables[0].Rows[0].Age;
                iMaster.CivilStatus = ds.Tables[0].Rows[0].CivilStatus;
                iMaster.Citizenship = ds.Tables[0].Rows[0].Citizenship;
                iMaster.BirthDate   = ds.Tables[0].Rows[0].BirthDate;
                iMaster.BirthPlace  = ds.Tables[0].Rows[0].BirthPlace;
                iMaster.Remarks     = ds.Tables[0].Rows[0].Remarks;
                iMaster.DateCreated = ds.Tables[0].Rows[0].DateCreated;
                iMaster.DateModified= ds.Tables[0].Rows[0].DateModified;
                iMaster.Title       = ds.Tables[0].Rows[0].Title;
                iMaster.Alias       = ds.Tables[0].Rows[0].Alias;
                iMaster.BranchCode  = ds.Tables[0].Rows[0].BranchCode;                
                iMaster.Religion    = ds.Tables[0].Rows[0].Religion;
                iMaster.FullName    = ds.Tables[0].Rows[0].FullName;
                
                document.getElementById("txtHiddenCIFType").value      = iMaster.CIFType;                
                document.getElementById("txtHiddenMasterSeqId").value  = iMaster.SEQ_ID;
                document.getElementById("txtHiddenSeqId").value        = iMaster.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value        = iMaster.CIFID;
                document.getElementById("txtHiddenCIFBrCode").value    = iMaster.BranchCode;
                if (iMaster.CIFType == 1)
                {
                    if (document.getElementById("txtHiddenCIFID").value == document.getElementById("txtHiddenUserCIFID").value) { //RFC-24048
                        this.commons.to_lock_button("tblbuttons", "btnAdd~btnEdit~btnDelete");
                    }
                    else {
                        this.commons.to_unlock_button("tblbuttons", "btnAdd~btnEdit~btnDelete");
                    }

                    document.getElementById("txtLastname").value        = iMaster.LastName; 
                    document.getElementById("txtFirstname").value       = iMaster.FirstName; 
                    document.getElementById("txtMiddlename").value      = iMaster.MiddleName; 
                    /*Modified by Hajz to correct the age 01/03/2010 */
                    //document.getElementById("txtAge").value             = this.Age;             
                    document.getElementById("txtAge").value             = (iMaster.BirthDate == null || iMaster.BirthDate == "null") ? "" : this.generic.compute_age(this.commons.to_mmddyyyy_date(iMaster.BirthDate));
                    /*End*/
                    document.getElementById("txtBirthday").value        = this.commons.to_mmddyyyy_date(iMaster.BirthDate);
                    document.getElementById("txtBirthplace").value      = iMaster.BirthPlace;
                    document.getElementById("txtIndRemarks").value      = iMaster.Remarks;
                    document.getElementById("txtIndAlias").value        = iMaster.Alias;
                    
                    document.getElementById("txtMasterIndBranchCode").value  = iMaster.BranchCode + ' - ' + iMaster.get_branch_name(iMaster.BranchCode);
                    
                    this.commons = new common_method();                                        
                    this.commons.to_set_dropdownlist_index("ddGender",       iMaster.Gender);
                    this.commons.to_set_dropdownlist_index("ddCivilStatus",  iMaster.CivilStatus);
                    this.commons.to_set_dropdownlist_index("ddCitizenship",  iMaster.Citizenship);
                    this.commons.to_set_dropdownlist_index("ddReligion",     iMaster.Religion);
                    this.commons.to_set_dropdownlist_index("ddTitle",        iMaster.Title);  
                    
                    this.generic.to_clear_null("txtLastname~txtFirstname~txtMiddlename~txtAge~txtBirthday~txtBirthplace~txtIndRemarks~txtIndAlias~txtMasterIndBranchCode");                   
                }     
                else
                {            
                    document.getElementById("txtBusinessName").value        = iMaster.LastName;
                    document.getElementById("txtShortName").value           = iMaster.FirstName;
                    document.getElementById("txtNatureBusiness").value      = iMaster.MiddleName;
                    document.getElementById("txtBusAlias").value            = iMaster.Alias;
                    document.getElementById("txtBusRemarks").value          = iMaster.Remarks;
                    document.getElementById("txtMasterBusBranchCode").value = iMaster.BranchCode + ' - ' + iMaster.get_branch_name(iMaster.BranchCode);

                    /* RFC-31620 */
                    this.CIFClass = iMaster.CIFClass;
                    
                    this.generic.to_clear_null("txtBusinessName~txtShortName~txtNatureBusiness~txtBusAlias~txtBusRemarks~txtMasterBusBranchCode");                
                }  
                              
                this.commons = null;                         
            }
        }          
    }
        
    this.push_data_to_server = function()
    {
        var data_row = new Array();
                                                    
        data_row[data_row.length] = "CIFID~"         + this.CIFID;
        data_row[data_row.length] = "CIFType~"       + this.CIFType;        
        data_row[data_row.length] = "LastName~"      + this.LastName;
        data_row[data_row.length] = "FirstName~"     + this.FirstName;
        data_row[data_row.length] = "MiddleName~"    + this.MiddleName;
        data_row[data_row.length] = "Gender~"        + this.Gender;
        data_row[data_row.length] = "Age~"           + this.Age;
        data_row[data_row.length] = "CivilStatus~"   + this.CivilStatus;
        data_row[data_row.length] = "Citizenship~"   + this.Citizenship;
        data_row[data_row.length] = "BirthDate~"     + this.BirthDate;
        data_row[data_row.length] = "BirthPlace~"    + this.BirthPlace; 
        data_row[data_row.length] = "Remarks~"       + this.Remarks;
        data_row[data_row.length] = "DateCreated~"   + this.DateCreated;
        data_row[data_row.length] = "DateModified~"  + this.DateModified;
        data_row[data_row.length] = "Alias~"         + this.Alias;
        data_row[data_row.length] = "BranchCode~"    + this.BranchCode;
        data_row[data_row.length] = "Religion~"      + this.Religion;
        data_row[data_row.length] = "Title~"         + this.Title;
        data_row[data_row.length] = "CreatedBy~"     + this.CreatedBy;
        data_row[data_row.length] = "ModifiedBy~"    + this.ModifiedBy;
        data_row[data_row.length] = "FullName~"      + this.FullName;
        data_row[data_row.length] = "CIFClass~"      + this.CIFClass;
        data_row[data_row.length] = "Deleted~false";
        
        return data_row.join("|");
    }        
    
    this.get_branch_name = function(loc)
    {
        var brname = '';
        var ds = null;
        ds = IRSWorld.CIF.cifmain.get_bankname(loc).value;
        
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {
            document.getElementById("txtHiddenCIFBrName").value = ds.Tables[0].Rows[0].BranchName;
            brname = ds.Tables[0].Rows[0].BranchName;
        }
        
        return brname;
    }
    
    this.insert_record = function()
    {
        this.insert_flag = true;
        document.getElementById("txtHiddenMasterSeqId").value = 0;
        document.getElementById("txtHiddenCIFID").value = '';
        IRSWorld.CIF.cifmain.insert_record(this.insert_record_callback);
        //this.set_branchdef_values();
    }
    
    this.insert_record_callback = function(result)
    {
        if (result.error != null && result.error != "undefined")
        {
            alert("UNABLE TO INSERT RECORD. " + result.error);
        }
        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {                
                iMaster.call_server_side_message(); return;
            }
            
            var lname = document.getElementById("txtSearchLastName").value;
            var fname = document.getElementById("txtSearchFirstName").value;
            var mname = document.getElementById("txtSearchMiddleName").value;
        
            this.commons = new common_method();
            if (document.getElementById("txtHiddenCIFType").value == '1')
            {
                this.commons.to_clear_screen("tblCIFIndividual"); 
                this.commons.to_read_only("tblCIFIndividual", false);
                                
                if (document.getElementById("txtHiddenSearchFlag").value != 'true')
                {
                    document.getElementById("txtLastname").value   = lname;
                    document.getElementById("txtFirstname").value  = fname;
                    document.getElementById("txtMiddlename").value = mname; 
                }
        
                document.getElementById("txtHiddenCIFBrCode").value = document.getElementById("txtHiddenUserLocation").value;
                /* VAPT IAG 20180801 */
                //document.getElementById("txtMasterIndBranchCode").value = document.getElementById("txtHiddenCIFBrCode").value + ' - ' + iMaster.get_branch_name(document.getElementById("txtHiddenUserLocation").value);
            }
            else 
            {
                this.commons.to_clear_screen("tblUserBusiness"); 
                this.commons.to_read_only("tblUserBusiness", false);
                
                if (document.getElementById("txtHiddenSearchFlag").value != 'true')
                {
                    document.getElementById("txtBusinessName").value   = lname;
                    document.getElementById("txtShortName").value      = fname;
                    document.getElementById("txtNatureBusiness").value = mname;
                }
                            
                document.getElementById("txtHiddenCIFBrCode").value   = document.getElementById("txtHiddenUserLocation").value;
                document.getElementById("txtMasterBusBranchCode").value = document.getElementById("txtHiddenCIFBrCode").value + ' - ' + iMaster.get_branch_name(document.getElementById("txtHiddenUserLocation").value);
            } 
            document.getElementById("txtSearchLastName").value = '';
            document.getElementById("txtSearchFirstName").value = '';
            document.getElementById("txtSearchMiddleName").value = '';
            
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            iMaster.disable_branch_fields();
        }
    }
    
    this.disable_branch_fields = function()
    {
        document.getElementById("txtMasterIndBranchCode").readOnly = true;
        document.getElementById("txtMasterBusBranchCode").readOnly = true;
    }
    
    this.update_record = function()
    {
        IRSWorld.CIF.cifmain.update_record(this.SEQ_ID, this.update_record_callback);        
    }
    
    this.update_record_callback = function(result)
    {
        if (result.error != null && result.error != "undefined")
        {
            alert("UNABLE TO UPDATE RECORD. " + result.error); return; 
        }
        
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {                
                iMaster.call_server_side_message(); return;
            }
            
            this.commons = new common_method();
            if ( iMaster.CIFType == 1)
            {
                this.commons.to_read_only("tblCIFIndividual", false);
                document.getElementById('btnSearchBranchCode').disabled = true;
            }
            else 
            {
                this.commons.to_read_only("tblUserBusiness", false);
                document.getElementById('btnSearchBusBranchCode').disabled = true;
            }
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            iMaster.disable_branch_fields();
        }        
    } 
    
    this.delete_record = function()
    {
        IRSWorld.CIF.cifmain.delete_record(this.SEQ_ID, this.delete_record_callback);
        //===========================================================
        //RFC-31620 : IRSW Maker-Checker Function for User Admin
        //cif_main.delete_master();
        //===========================================================
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
                iMaster.call_server_side_message(); 
                
                if (iMaster.server_side_msg != null && iMaster.server_side_msg != "")
                {
                    alert(iMaster.server_side_msg); 
                    this.value = iMaster; return;
                }
            }
            //===========================================================
            //RFC-31620 : IRSW Maker-Checker Function for User Admin
            cif_main.delete_master();
            //===========================================================
            return true;
        }     
    }

    this.get_cifclass = function()
    {
        var param = document.getElementById("txtHiddenParameter").value;
        if(param == "addrem")
        {
            this.insert_flag = true;
            document.getElementById("ddSRole").value = 1;
            document.getElementById("txtHiddenDummyClass").value = '';
        } 
        else if(param == "addbene")
        {
            this.insert_flag = true;
            document.getElementById("ddSRole").value = 2;
            document.getElementById("txtHiddenDummyClass").value = '';
        }
        if(this.insert_flag == true)
        {
            var cifclass = null;
            var dummy    = document.getElementById("txtHiddenDummyClass").value;
            var srole    = document.getElementById("ddSRole").value;
            
            if(srole != 'null' && srole != '' && srole != null)
            {
                cifclass = srole;
            }
            else
            {
                cifclass = dummy;
            }
            
            document.getElementById("txtHiddenDummyClass").value = '';
            document.getElementById("ddSRole").value = 'null';
            
            return cifclass;
        }
        return null;
    }
    
    this.commit_record = function()
    {
        IRSWorld.CIF.cifmain.commit_record( this.set_record(), this.commit_record_callback );
        var mseqid = null;
        mseqid = IRSWorld.CIF.cifmain.get_seq_id().value;
        
        if (mseqid != null)
        {
            document.getElementById("txtHiddenCIFID").value = IRSWorld.CIF.cifmain.GET_CIF_MASTER_CIFID(mseqid).value;
        }
        this.SEQ_ID = null;
    }
    
    this.commit_record_callback = function(result)
    {               
	    iMaster.load_record();
        
        if (typeof(iMaster.load_grid_viewer) != 'undefined')
        {
            iMaster.load_grid_viewer();
        }    
             
        if (result.error != null && result.error != "undefined")
        {            
            alert("UNABLE TO COMMIT RECORD. " + result.error); return;
        }
                                
        if (result != null && result.value != "undefined")
        {                                
            if (result.value == false)
            {     
                iMaster.call_server_side_message();
                                
                if (iMaster.server_side_msg != null && iMaster.server_side_msg != "")
                {
                    alert(iMaster.server_side_msg); 
                    this.value = iMaster; return;
                }
            }
            
            iMaster.commons = new common_method();
            iMaster.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");   
            iMaster.commons = null;

            /* RFC-31620 */
            var msg = IRSWorld.CIF.cifmain.isAdminApproverTag().value;
            if (msg != "") {
                alert(msg);
            }
            else {
                alert("RECORD COMMITTED SUCCESSFULLY");
            }
         }
    }
    
    
    this.rollback_record = function()
    {
        IRSWorld.CIF.cifmain.rollback_record(this.rollback_record_callback);
    }
    
    this.rollback_record_callback = function(result)
    {
        iMaster.call_server_side_message();
        
        if (iMaster.server_side_msg != null && iMaster.server_side_msg != "")
        {
            alert(iMaster.server_side_msg); this.value = iMaster; return;
        }
        
        iMaster.commons = new common_method();
        iMaster.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");   
        iMaster.commons = null;
    }   
    
    this.add_spouse_to_master = function(remitter_id)
    {
        /*var ds = null;
        ds = IRSWorld.CIF.cifmain.get_branch_code(document.getElementById("txtHiddenMasterSeqId").value).value;
        var branch_code = ds.Tables[0].Rows[0].BranchCode;*/
        
        this.BranchCode = '072'; //branch_code; '072';
        
        var insert_master_flag = IRSWorld.CIF.cifmain.insert_record();        
        if (insert_master_flag.value == false)
        {
            alert("UNABLE TO ADD SPOUSE CLIENT INFORMATION!"); return;
        }
        
        var commit_flag =  IRSWorld.CIF.cifmain.commit_record(this.set_spouse_master_record());        
        if (commit_flag.value == false)
        {
            alert("UNABLE TO SAVE SPOUSE CLIENT INFORMATION!"); return;
        }        
        
        var spouse_bene_seq_id = IRSWorld.CIF.cifmain.get_seq_id();
        var spouse_ben_cifid   = IRSWorld.CIF.cifmain.GET_CIF_MASTER_CIFID(spouse_bene_seq_id.value);
        
        this.add_spouse_to_class(spouse_ben_cifid.value);        
        this.add_spouse_to_bene(remitter_id, spouse_ben_cifid.value);        
        this.update_spouse(spouse_ben_cifid.value);                
        alert("SPOUSE ADDED TO BENEFICIARIES!");        
    }   
    
    this.set_spouse_master_record = function()
    {
    
        var data_row = new Array();
        
        data_row[data_row.length] = "CIFID~";
        data_row[data_row.length] = "CIFType~1";
        data_row[data_row.length] = "LastName~"      + document.getElementById("txtSpouseLName").value;
        data_row[data_row.length] = "FirstName~"     + document.getElementById("txtSpouseFName").value;
        data_row[data_row.length] = "MiddleName~"    + document.getElementById("txtSpouseMName").value;
        data_row[data_row.length] = "BranchCode~"    + this.BranchCode;
        data_row[data_row.length] = "DateCreated~"   + cif_main.get_branch_localtime();//get_current_date_string();
        data_row[data_row.length] = "DateModified~"  + cif_main.get_branch_localtime();//get_current_date_string();
                  
        return data_row.join("|");
    }
    
    this.add_spouse_to_class = function(benef_id)
    {        
        IRSWorld.CIF.cifmain.set_module_name("CIF_CLASSIFICATIONS");        
        
        var commit_flag =  IRSWorld.CIF.cifmain.commit_record(this.set_spouse_class_record(benef_id));        
        if (commit_flag.value == false)
        {
            alert("UNABLE TO SET SPOUSE AS BENEFICIARY!"); return;
        }  
    }
    
    this.set_spouse_class_record = function(benef_ids)
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~" + benef_ids;
        data_row[data_row.length]  = "CIFClass~2"; 
        
        return data_row.join("|");        
    }
    
    this.add_spouse_to_bene = function(rem_id, bene_id)
    {
        IRSWorld.CIF.cifmain.set_module_name("CIF_BENEFICIARIES");        
        
        var insert_bene_flag = IRSWorld.CIF.cifmain.insert_record();        
        if (insert_bene_flag.value == false)
        {
            alert("UNABLE TO ADD SPOUSE AS BENEFICIARY!"); return;
        }
        
        var commit_flag =  IRSWorld.CIF.cifmain.commit_record(this.set_spouse_bene_record(rem_id, bene_id));        
        if (commit_flag.value == false)
        {
            alert("UNABLE TO SAVE SPOUSE AS BENEFICIARY!"); return;
        } 
    }
    
    this.set_spouse_bene_record = function(rem_ids, bene_ids)
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "CIFID~"         + bene_ids;
        data_row[data_row.length]  = "RemitterCIFID~" + rem_ids;
        
        return data_row.join("|"); 
    }
    
    this.update_spouse = function(bene_cifid)
    {
        IRSWorld.CIF.cifmain.set_module_name("CIF_SPOUSE");        
        
        this.spouse_seq_id = document.getElementById("txtHiddenSeqId").value;
        
        var update_flag = IRSWorld.CIF.cifmain.update_record(this.spouse_seq_id);        
        if (update_flag.value == false)
        {
            alert("UNABLE TO UPDATE SPOUSE AS BENEFICIARY!"); return;
        }
        
        var commit_flag =  IRSWorld.CIF.cifmain.commit_record(this.update_spouse_bene_record(bene_cifid));        
        if (commit_flag.value == false)
        {
            alert("UNABLE TO SAVE SPOUSE TO BENEFICIARIES!"); return;
        }
    }
    
    this.update_spouse_bene_record = function(bene_cifid)
    {
        var data_row = new Array();
        
        data_row[data_row.length]  = "BeneCIFID~" + bene_cifid;
        
        return data_row.join("|");
    }    
    
    this.set_created_modified = function()
    {
        if (document.getElementById("txtHiddenMasterSeqId").value != '0')
        {
            var ds = null;
            var ds = IRSWorld.CIF.cifmain.GET_CIF_MASTER_CREATEDMODIFIED(document.getElementById("txtHiddenMasterSeqId").value).value;
            
            if (ds != null && ds.Tables[0].Rows.length > 0)
            { 
                var date_created = ds.Tables[0].Rows[0].DateCreated;
                var created_by   = ds.Tables[0].Rows[0].CreatedBy;
                 
                date_created = dateToString(date_created);
                
                if (date_created == null || date_created == '') this.DateCreated = cif_main.get_branch_localtime();
                else this.DateCreated = date_created;
                
                if (created_by == null || created_by == '') this.CreatedBy = document.getElementById("txtHiddenUserID").value;
                else this.CreatedBy = created_by;
                
                this.DateModified = cif_main.get_branch_localtime();
                this.ModifiedBy   = document.getElementById("txtHiddenUserID").value;
            }
            return;
        }
        this.DateCreated  = cif_main.get_branch_localtime();
        this.DateModified = cif_main.get_branch_localtime();
        this.CreatedBy    = document.getElementById("txtHiddenUserID").value;
        this.Modifiedby   = document.getElementById("txtHiddenUserID").value;
    } 
    
    this.set_branchdef_values = function()
    {
        var ds = IRSWorld.CIF.cifmain.get_branchdef_values(document.getElementById("txtHiddenUserLocation").value).value;
        
        if(ds != null && ds.Tables[0].Rows.length > 0)
        {
            this.CivilStatus = ds.Tables[0].Rows[0].CivilStatus;
	        this.Citizenship = ds.Tables[0].Rows[0].Citizenship;
	        this.Gender      = ds.Tables[0].Rows[0].Gender;
            
            this.commons = new common_method();
            this.commons.to_set_dropdownlist_index("ddCivilStatus",this.CivilStatus);
            this.commons.to_set_dropdownlist_index("ddCitizenship",this.Citizenship);
            this.commons.to_set_dropdownlist_index("ddGender",     this.Gender);
            
            this.commons = null;
        }  
    }
    
    this.validate_input = function()
    { 
        this.commons     = new common_method();  
        this.generic     = new generic_method();        
        this.error_input = '';  
        var foc_elem     = '';
        
        if (document.getElementById("txtHiddenCIFType").value == '1')
        {
            if (this.generic.is_alpha(document.getElementById("txtLastname").value) == false)
            {           
                this.error_input += "Last name contains invalid characters.\n";
                foc_elem = 'txtLastname';       
            }        
            if (this.generic.is_alpha(document.getElementById("txtFirstname").value) == false)
            {           
                this.error_input += "First name contains invalid characters.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtFirstname';            
            }        
            if (this.generic.is_alpha(document.getElementById("txtMiddlename").value) == false)
            {           
                this.error_input += "Middle name contains invalid characters.\n";
                if (foc_elem.length == 0) foc_elem = 'txtMiddlename';          
            }
            if (this.generic.is_alphanumeric2(document.getElementById("txtIndAlias").value) == false)
            {           
                this.error_input += "Alias contains invalid characters.\n";
                if (foc_elem.length == 0) foc_elem = 'txtIndAlias';          
            }
            if (this.generic.to_validate_bdate(document.getElementById("txtBirthday").value) == false)
            {           
                this.error_input += "Birth date is invalid date.\n";    
                if (foc_elem.length == 0) foc_elem = 'txtBirthday';       
            }
            if (this.generic.is_positive(document.getElementById("txtAge").value) == false)
            {           
                this.error_input += "Age is invalid.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtAge';
            }
            
            var age_limit = IRSWorld.CIF.cifmain.AgeLimit(document.getElementById("txtHiddenCIFBrCode").value).value;
            
            if(age_limit != null | age_limit != '')
            {
                if(document.getElementById("txtAge").value < parseFloat(age_limit))
                {
                    this.error_input += "Client who are below "+ age_limit +" yrs old are not allowed.\n";
                    if (foc_elem.length == 0) foc_elem = 'txtAge';
                } 
            }
//            else if (parseFloat(document.getElementById("txtAge").value) < 18) //parameter here
//            {
//                this.error_input += "Client who are below 18 yrs old are not allowed.\n";
//                if (foc_elem.length == 0) foc_elem = 'txtAge';
//            }
        }
        
        else if (document.getElementById("txtHiddenCIFType").value == '2')
        {
            if (this.generic.is_alphanumeric_bus(document.getElementById("txtBusinessName").value) == false)
            {           
                this.error_input += "Business name contains invalid characters.\n";
                foc_elem = 'txtBusinessName';       
            }        
            if (this.generic.is_alphanumeric_bus(document.getElementById("txtShortName").value) == false)
            {           
                this.error_input += "Short name contains invalid characters.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtShortName';            
            } 
            if (this.generic.is_alphanumeric_bus(document.getElementById("txtBusAlias").value) == false)
            {           
                this.error_input += "Alias contains invalid characters.\n";
                if (foc_elem.length == 0) foc_elem = 'txtBusAlias'; 
            }        
            if (this.generic.is_alphanumeric2(document.getElementById("txtNatureBusiness").value) == false)
            {           
                this.error_input += "Type of business contains invalid characters.\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtNatureBusiness';            
            }
        }
        
        if (foc_elem.length > 0)
        {
            try
            {
                document.getElementById(foc_elem).focus();
                document.getElementById(foc_elem).select();
            }
            catch(e){}
        }
        if (this.error_input != '')
        {
            return false;
        }
        
        return true; 
    }                                                                           
}
