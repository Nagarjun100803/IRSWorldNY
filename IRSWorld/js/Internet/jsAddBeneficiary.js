function Internet_Beneficiary()
{
    this.SEQ_ID             = null;
	this.Lastname           = null;
	this.Firstname          = null;
	this.Middlename         = null;
	this.Age                = null;
	this.Birthdate          = null;
	this.TelNo              = null;
	this.Email              = null;
	this.ZipCode            = null;
	this.CompanyName        = null;
	this.Occupation         = null;
	this.BankName           = null;
	this.BranchName         = null;
	this.AccountNo          = null;
	this.AccountCurrency    = null;
	this.AccountType        = null;
	this.ProductCode        = null;
	this.CreatedBy          = null;	
	this.DateCreated        = null;
	this.CIFID              = null;
	this.RemitterCIFID      = null;
	this.grid_ajax_query    = null;
    this.grid_ajax_page     = "../AjxPage/ajaxgrid.aspx";
    this.update_flag        = false;
    this.grid               = null;    
    this.insert_flag        = null;
    this.BENETOAPPSEQID     = null;
    this.REMTOAPPSEQID      = null;
    this.REMCIFID           = null;
    this.RR_ACCTNO          = null;
    this.VALIDFC            = null;
    this.ACCT_DET           = null;
    this.IS_EXISTING        = false;
    this.IS_KBI             = false;
    this.ACCTSEQID          = null;
    this.Alias              = null;
    this.Address2           = null;
    this.AddressType        = null;
    this.PreferredMailAdd   = null;
    this.DetailedDesc       = null;
    this.WorkerType         = null;
    this.AnnualSalary       = null;
    this.SalCurrency        = null;
    this.Relationship       = null;
    this.NotificationMode   = null;
    this.GFCCArdNo          = null;
    this.PreferredContact   = null;
    this.GFCIsValid         = null;
    var iBene               = this;
    this.isClientBene       = false;
    this.OtherBankName      = false;
    
    //  FOR HOME ADDRESS
    this.Address1           = null;
	this.POBox              = null;
    this.Address2           = null;
    this.PreferredMailAdd   = null;
    //  FOR COMPANY ADDRESS
    this.Address1_Com           = null;
	this.POBox_Com              = null;
    this.Address2_Com           = null;
    this.PreferredMailAdd_Com   = null;
    //  FOR MAILING ADDRESS
    this.Address1_Mail           = null;
	this.POBox_Mail              = null;
    this.Address2_Mail           = null;
    this.PreferredMailAdd_Mail   = null;
    
    this.AddSeqID           =   null;
    this.AddSeqID_Com       =   null;
    this.AddSeqID_Mail      =   null;
        
    this.load_grid_viewer = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
    
        this.grid.sql_source     = "sp_internet_grid_bene~" + ((document.getElementById("txtHiddenUserRole").value == '10') ? document.getElementById("txtHiddenUserCIFID").value : document.getElementById("txtHiddenRemCIFID").value);
        this.grid.div_container  = document.getElementById("DIVGRIDBENE");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_bene~" + ((document.getElementById("txtHiddenUserRole").value == '10') ? document.getElementById("txtHiddenUserCIFID").value : document.getElementById("txtHiddenRemCIFID").value);
        this.grid.div_container  = document.getElementById("DIVGRIDBENE");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.load_grid_viewer_cifbene = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        
        var remcifid = this.get_rem_cifid();
        this.grid.sql_source = "sp_internet_grid_cifbene~" + remcifid;
        
        this.grid.div_container  = document.getElementById("DIVGRIDBENECIF");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer_cifbene = function(parameter)
    {
        this.grid = new grid_viewer();
     
        var remcifid = this.get_rem_cifid();
        this.grid.sql_source = "sp_internet_grid_cifbene~" + remcifid;      
        this.grid.div_container  = document.getElementById("DIVGRIDBENECIF");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.load_grid_viewer_appbene = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_bene_for_approval~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVBENEFORAPP");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_appbene = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_bene_for_approval~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVBENEFORAPP");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.load_grid_viewer_rejbene = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_bene_rejected~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVBENEREJ");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_rejbene = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_bene_rejected~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVBENEREJ");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
        
    this.load_record = function()
	{
	    this.clear_screen();
	    this.disable_field(true);
	    this.color_asterisks();
	    this.add_mode_bene();
	    this.pull_server_data();
	   document.getElementById("tblBeneInfo").style.visibility = "hidden";
	   document.getElementById("tblBeneInfo").style.display = "none";
	   document.getElementById("tblGridBene").style.visibility = "visible";
	   document.getElementById("btnSenReq").style.visibility = "hidden";
	   document.getElementById("btnCancelBene2").style.visibility = "hidden";
       document.getElementById("tblGridBene").style.display = "block";
       document.getElementById("tblGridBeneCIF").style.visibility = "visible";
       document.getElementById("tblGridBeneCIF").style.display = "block";
	}
	
	this.pull_server_data = function()
	{   
	    IRSWorld.Internet.InternetAddBeneficiary.GET_INTERNET_REMITTER_BENE_RECORD(document.getElementById("txtHiddenBeneSeqId").value,this.pull_server_data_callback);
	}
	
	this.pull_server_data_callback = function(result)
    {    
        if (result != null && result.value != null)
        {    
            var ds = result.value;
            this.generic = new generic_method();
            this.commons = new common_method();
                 
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                document.getElementById("txtHiddenSelectedBeneCIFID").value = ds.Tables[0].Rows[0].CIFID;
	            this.ProductCode        = ds.Tables[0].Rows[0].ProductCode;
	            this.OtherBankName      = ds.Tables[0].Rows[0].BankCode;
	            Internet_Beneficiary.BranchCode         = ds.Tables[0].Rows[0].BranchCode;
	            document.getElementById("txtBeneLName").value     = ds.Tables[0].Rows[0].Lastname;
	            document.getElementById("txtBeneFName").value     = ds.Tables[0].Rows[0].Firstname;
	            document.getElementById("txtBeneMName").value     = ds.Tables[0].Rows[0].Middlename;
	            document.getElementById("txtBeneBDate").value     = this.commons.to_mmddyyyy_date(ds.Tables[0].Rows[0].Birthdate); 
	            document.getElementById("txtBeneBPlace").value    = ds.Tables[0].Rows[0].BirthPlace;
	            
	            //  FOR HOME ADDRESS
	            document.getElementById("txtBeneAddress1").value  = ds.Tables[0].Rows[0].Address1;
	            document.getElementById("txtBeneAddress2").value  = ds.Tables[0].Rows[0].Address2;
	            document.getElementById("txtBenePOBox").value     = ds.Tables[0].Rows[0].POBox;	            
	            document.getElementById("chkBenePrefMailAdd").checked = ds.Tables[0].Rows[0].PreferredMailAdd;
	            if (ds.Tables[0].Rows[0].ZipCode != null && ds.Tables[0].Rows[0].ZipCode != 'undefined' && ds.Tables[0].Rows[0].ZipCode != '')
                {   
                    document.getElementById("txtHiddenZipSeqId").value = ds.Tables[0].Rows[0].ZipCode;
                    var ds1 = IRSWorld.Internet.InternetAddBeneficiary.get_zipcode(ds.Tables[0].Rows[0].ZipCode).value;
                    if (ds1 != null && ds1.Tables[0].Rows.length > 0)
                    {   
                        document.getElementById("txtBeneZip").value = ds1.Tables[0].Rows[0].Municipality + ", " + ds1.Tables[0].Rows[0].Province + " " + ds1.Tables[0].Rows[0].Truezipcode;
                    }
                }
	            Internet_Beneficiary.AddSeqID                         = ds.Tables[0].Rows[0].SEQ_ID_Home;
	            //  FOR COMPANY ADDRESS
	            document.getElementById("txtBeneAddress1_Com").value  = ds.Tables[0].Rows[0].Address1_Com;
	            document.getElementById("txtBeneAddress2_Com").value  = ds.Tables[0].Rows[0].Address2_Com;
	            document.getElementById("txtBenePOBox_Com").value     = ds.Tables[0].Rows[0].POBox_Com;	            
	            document.getElementById("chkBenePrefMailAdd_Com").checked = ds.Tables[0].Rows[0].PreferredMailAdd_Com;
	            if (ds.Tables[0].Rows[0].ZipCode_Com != null && ds.Tables[0].Rows[0].ZipCode_Com != 'undefined' && ds.Tables[0].Rows[0].ZipCode_Com != '')
                {   
                    document.getElementById("txtHiddenZipSeqId_Com").value = ds.Tables[0].Rows[0].ZipCode_Com;
                    var ds1_Com = IRSWorld.Internet.InternetAddBeneficiary.get_zipcode(ds.Tables[0].Rows[0].ZipCode_Com).value;
                    if (ds1_Com != null && ds1_Com.Tables[0].Rows.length > 0)
                    {   
                        document.getElementById("txtBeneZip_Com").value = ds1_Com.Tables[0].Rows[0].Municipality + ", " + ds1_Com.Tables[0].Rows[0].Province + " " + ds1_Com.Tables[0].Rows[0].Truezipcode;
                    }
                }
                Internet_Beneficiary.AddSeqID_Com                      = ds.Tables[0].Rows[0].SEQ_ID_Com;
                
                //  FOR MAILING ADDRESS
	            document.getElementById("txtBeneAddress1_Mail").value  = ds.Tables[0].Rows[0].Address1_Mail;
	            document.getElementById("txtBeneAddress2_Mail").value  = ds.Tables[0].Rows[0].Address2_Mail;
	            document.getElementById("txtBenePOBox_Mail").value     = ds.Tables[0].Rows[0].POBox_Mail;	            
	            document.getElementById("chkBenePrefMailAdd_Mail").checked = ds.Tables[0].Rows[0].PreferredMailAdd_Mail;
	            if (ds.Tables[0].Rows[0].ZipCode_Mail != null && ds.Tables[0].Rows[0].ZipCode_Mail != 'undefined' && ds.Tables[0].Rows[0].ZipCode_Mail != '')
                {   
                    document.getElementById("txtHiddenZipSeqId_Mail").value = ds.Tables[0].Rows[0].ZipCode_Mail;
                    var ds1_Mail = IRSWorld.Internet.InternetAddBeneficiary.get_zipcode(ds.Tables[0].Rows[0].ZipCode_Mail).value;
                    if (ds1_Mail != null && ds1_Mail.Tables[0].Rows.length > 0)
                    {   
                        document.getElementById("txtBeneZip_Mail").value = ds1_Mail.Tables[0].Rows[0].Municipality + ", " + ds1_Mail.Tables[0].Rows[0].Province + " " + ds1_Mail.Tables[0].Rows[0].Truezipcode;
                    }
                }
                Internet_Beneficiary.AddSeqID_Mail                = ds.Tables[0].Rows[0].SEQ_ID_Mail;
                
	            document.getElementById("txtBeneTelNo").value     = ds.Tables[0].Rows[0].TelNo;
	            document.getElementById("txtBeneMobileNo").value  = ds.Tables[0].Rows[0].MobileNo;
	            document.getElementById("txtBeneEmailAdd").value  = ds.Tables[0].Rows[0].Email;
	            document.getElementById("txtBeneCompany").value   = ds.Tables[0].Rows[0].CompanyName;
	            document.getElementById("ddBeneOccupation").value = ds.Tables[0].Rows[0].Occupation;
	            document.getElementById("txtBAccountNo").value    = ds.Tables[0].Rows[0].AccountNo;
	            document.getElementById("txtBGFCNo").value        = ds.Tables[0].Rows[0].GFCCArdNo;
	            document.getElementById("ddBTargetCurr").value    = ds.Tables[0].Rows[0].AccountCurrency;
                document.getElementById("txtBeneDetDesc").value   = ds.Tables[0].Rows[0].DetailedDesc;
	            document.getElementById("ddBeneWorkerType").value = ds.Tables[0].Rows[0].WorkerType;
	            document.getElementById("txtBeneAnnualSal").value = this.generic.formatCurrency(ds.Tables[0].Rows[0].Salary);            
	            document.getElementById("ddBeneSalCurr").value    = ds.Tables[0].Rows[0].SalaryCurrency;
	            document.getElementById("ddBeneRel").value        = ds.Tables[0].Rows[0].Relationship;
	            document.getElementById("ddBeneNotMode").value    = ds.Tables[0].Rows[0].NotificationMode;
	            
	            if (this.ProductCode == '12' || this.ProductCode == '13')
	            {
	                this.ProductCode = '13';
	                if (this.OtherBankName.substring(0,4) == '000-')
	                {
	                    this.OtherBankName = this.OtherBankName.substring(4,this.OtherBankName.length);
	                    document.getElementById("txtBRuralBankName").value = this.OtherBankName;
	                    document.getElementById("chkIsRuralBankBene").checked = true;
	                }
	                else
	                {
	                    document.getElementById("ddBOtherBankName").value = this.OtherBankName;
	                    document.getElementById("chkIsRuralBankBene").checked = false;
	                }
	            }
	            	            
	            document.getElementById("ddBProductType").value   = this.ProductCode;
	            document.getElementById("ddBenePrefContact").value    = ds.Tables[0].Rows[0].PreferredContact;
	            //document.getElementById("ddBusBenePrefContact").value    = this.PreferredContact;
	            
               // document.getElementById('trDetailedDescription').style.display = 'none';
                if (document.getElementById('ddBeneOccupation').value != null && document.getElementById('ddBeneOccupation').value.toString() == '75')
                {
                    document.getElementById('trDetailedDescription').style.display = 'block';
                }      	            
	            
                Internet_Remitter_Main.show_acctno_field();
                
	            this.generic.to_clear_null("txtBeneLName~txtBeneFName~txtBeneMName~txtBeneBDate~txtBeneBPlace~txtBeneAddress1~txtBeneAddress2~txtBenePOBox");
	            this.generic.to_clear_null("txtBeneTelNo~txtBeneMobileNo~txtBeneEmailAdd~txtBeneCompany~txtBeneZip~txtBeneDetDesc~txtBeneAnnualSal");
	            this.generic.to_clear_null("txtBeneAddress1_Com~txtBeneAddress2_Com~txtBenePOBox_Com~txtBeneZip_Com");
	            this.generic.to_clear_null("txtBeneAddress1_Mail~txtBeneAddress2_Mail~txtBenePOBox_Mail~txtBeneZip_Mail");
	        }
	    }
	    try{document.getElementById("txtBeneLName").focus()}catch(e){};
	}
	
    this.set_ben_record = function(isRequest)
	{
	    this.generic = new generic_method();
	    this.CIFID      = document.getElementById("txtHiddenSelectedBeneCIFID").value;
	    this.CIFClass   = '2';
	    this.CreatedBy  = document.getElementById("txtHiddenUserID").value;
	    //this.BranchCode = '';
        //this.BranchCode = document.getElementById("txtHiddenUserLoc").value;
	    if (this.insert_flag == true)
        {    
            this.BranchCode = document.getElementById("txtHiddenUserLoc").value;
        }    
            
        this.CIFType    = ciftype_b;
        
        if(ciftype_b == 2) /*business type*/
        {
            this.Lastname   = document.getElementById("txtBeneBusinessName").value;
	        this.Firstname  = document.getElementById("txtBeneShortName").value;
	        this.Middlename = document.getElementById("txtBeneBBusType").value;
	        this.Alias      = document.getElementById("txtBeneBAlias").value;
	        
	        //  FOR HOME ADDRESS
	        this.Address1   = document.getElementById("txtBeneBAddress1").value;
	        this.Address2   = document.getElementById("txtBeneBAddress2").value;
	        this.POBox      = document.getElementById("txtBeneBPOBox").value;
            this.AddressType= '2';
            this.PreferredMailAdd = document.getElementById("chkBeneBPrefMailAdd").checked;
	        this.ZipCode    = this.generic.to_trim(document.getElementById("txtHiddenZipSeqId").value);
	        
	        //  FOR COMPANY ADDRESS
	        this.Address1_Com   = document.getElementById("txtBeneBAddress1_Com").value;
	        this.Address2_Com   = document.getElementById("txtBeneBAddress2_Com").value;
	        this.POBox_Com      = document.getElementById("txtBeneBPOBox_Com").value;
            this.AddressType_Com= '1';
            this.PreferredMailAdd_Com = document.getElementById("chkBeneBPrefMailAdd_Com").checked;
	        this.ZipCode_Com    = this.generic.to_trim(document.getElementById("txtHiddenZipSeqId_Com").value);
	        
	        //  FOR MAILING ADDRESS
	        this.Address1_Mail   = document.getElementById("txtBeneBAddress1_Mail").value;
	        this.Address2_Mail   = document.getElementById("txtBeneBAddress2_Mail").value;
	        this.POBox_Mail      = document.getElementById("txtBeneBPOBox_Mail").value;
            this.AddressType_Mail= '3';
            this.PreferredMailAdd_Mail = document.getElementById("chkBeneBPrefMailAdd_Mail").checked;
	        this.ZipCode_Mail    = this.generic.to_trim(document.getElementById("txtHiddenZipSeqId_Mail").value);
	        
	        this.TelNo      = document.getElementById("txtBeneBTelNo").value;
	        this.MobileNo   = document.getElementById("txtBeneBMobileNo").value;
	        this.Email      = document.getElementById("txtBeneBEmailAdd").value;
	        this.Age        = '';
	        this.Birthdate  = '';
	        this.Birthplace = '';
	        this.Company    = ''
	        this.Occupation = 'null';
	        this.DetailedDesc   = '';
            this.WorkerType     = '';
            this.AnnualSalary   = '';
            this.SalCurrency    = '';
	        this.Relationship   = document.getElementById("ddBeneBRel").value;
            this.NotificationMode= document.getElementById("ddBeneBNotMode").value;
            
            this.PreferredContact= document.getElementById("ddBusBenePrefContact").value;            
	        
	        
            /*if(document.getElementById("txtHiddenUserRole").value == '10')
            {
                this.RemitterCIFID  = document.getElementById("txtHiddenUserCIFID").value;
                
                this.ProductCode = document.getElementById("ddBProductType").value;
                this.AccountNo   = document.getElementById("txtBAccountNo").value;
                if(this.ProductCode != '10')
                {
                    this.AccountCurrency = document.getElementById("ddBTargetCurr").value; 
                }
                else this.AccountCurrency = null;
            }
            else this.RemitterCIFID = this.get_rem_cifid();*/
            this.RemitterCIFID = this.get_rem_cifid();
        }
        else /*individual*/
        {
            this.Lastname       = document.getElementById("txtBeneLName").value;
            this.Firstname      = document.getElementById("txtBeneFName").value;
            this.Middlename     = document.getElementById("txtBeneMName").value;
            this.Birthdate      = document.getElementById("txtBeneBDate").value;
            this.Age            = document.getElementById("txtHiddenBeneAge").value;
            this.Birthplace     = document.getElementById("txtBeneBPlace").value;
            
            //  FOR HOME ADDRESS
            this.Address1       = document.getElementById("txtBeneAddress1").value;
            this.Address2       = document.getElementById("txtBeneAddress2").value;
            this.POBox          = document.getElementById("txtBenePOBox").value;
            this.AddressType    = '2';
            this.PreferredMailAdd = document.getElementById("chkBenePrefMailAdd").checked;
            this.ZipCode        = this.generic.to_trim(document.getElementById("txtHiddenZipSeqId").value);
            
            //  FOR COMPANY ADDRESS
            this.Address1_Com       = document.getElementById("txtBeneAddress1_Com").value;
            this.Address2_Com       = document.getElementById("txtBeneAddress2_Com").value;
            this.POBox_Com          = document.getElementById("txtBenePOBox_Com").value;
            this.AddressType_Com    = '1';
            this.PreferredMailAdd_Com = document.getElementById("chkBenePrefMailAdd_Com").checked;
            this.ZipCode_Com        = this.generic.to_trim(document.getElementById("txtHiddenZipSeqId_Com").value);
            
            //  FOR MAIL ADDRESS
            this.Address1_Mail       = document.getElementById("txtBeneAddress1_Mail").value;
            this.Address2_Mail       = document.getElementById("txtBeneAddress2_Mail").value;
            this.POBox_Mail          = document.getElementById("txtBenePOBox_Mail").value;
            this.AddressType_Mail    = '3';
            this.PreferredMailAdd_Mail = document.getElementById("chkBenePrefMailAdd_Mail").checked;
            this.ZipCode_Mail        = this.generic.to_trim(document.getElementById("txtHiddenZipSeqId_Mail").value);
                        
            this.TelNo          = document.getElementById("txtBeneTelNo").value;
            this.MobileNo       = document.getElementById("txtBeneMobileNo").value;
            this.Email          = document.getElementById("txtBeneEmailAdd").value;
            this.Company        = document.getElementById("txtBeneCompany").value;
            this.Occupation     = document.getElementById("ddBeneOccupation").value;
            this.Alias          = '';
            this.DetailedDesc   = document.getElementById("txtBeneDetDesc").value;
            this.WorkerType     = document.getElementById("ddBeneWorkerType").value;
            this.AnnualSalary   = document.getElementById("txtBeneAnnualSal").value;
            this.SalCurrency    = document.getElementById("ddBeneSalCurr").value;
            this.Relationship   = document.getElementById("ddBeneRel").value;
            this.NotificationMode= document.getElementById("ddBeneNotMode").value;
            this.PreferredContact= document.getElementById("ddBenePrefContact").value;
                             
            //if(document.getElementById("txtHiddenUserRole").value == '10' || isAgentClient == true)
            if(document.getElementById("txtHiddenUserRole").value == '10')
            {
                this.RemitterCIFID  = (isAgentClient == true) ? this.get_rem_cifid() : document.getElementById("txtHiddenUserCIFID").value;
                
                this.ProductCode = document.getElementById("ddBProductType").value;
                this.GFCCArdNo      = document.getElementById("txtBGFCNo").value;
                this.AccountNo   = document.getElementById("txtBAccountNo").value;
                
                if(this.ProductCode != '10')
                {
                    this.AccountCurrency = document.getElementById("ddBTargetCurr").value; 
                }
                else this.AccountCurrency = null;
                
                if (this.ProductCode == '12' || this.ProductCode == '13')   // Special Handling for Otherbank Products
                {
                    if (document.getElementById("chkIsRuralBankBene").checked == true)
                    {
                        this.OtherBankName = '000-' + document.getElementById("txtBRuralBankName").value;
                        // Change product code to EPCS / Non-PDDTS if bank selected is rural bank
                        this.ProductCode = '13'
                    }
                    else
                    {
                        this.OtherBankName = document.getElementById("ddBOtherBankName").value;
                        this.ProductCode = '12'; 
//                        if (IRSWorld.Internet.InternetAddBeneficiary.isPDDTSBank(this.OtherBankName).value == true)
//                        {
                            // Change product code to EPCS / Non-PDDTS if bank selected is not PDDTS Member
//                              this.ProductCode = '12'; 
//                        }
                    }
                }
            }
            else this.RemitterCIFID = this.get_rem_cifid();
        }
        
        this.DateCreated = dateToString(IRSWorld.Internet.InternetAddBeneficiary.get_branch_localtime(document.getElementById("txtHiddenUserLoc").value).value);
        
        if (isRequest == true) 
        {
            this.BranchCode = document.getElementById("txtHiddenUserLoc").value;
            return this.push_data_to_server();
        }
        else return this.set_plist();
        if (isRequest == false)
        {
            return this.set_plist();
        } 
        
	}
	
	this.push_data_to_server = function()    
	{    
		var data_row = new Array();
		//if (this.insert_flag == false) 	 data_row[data_row.length] = "SEQ_ID~" + document.getElementById("txtHiddenBeneSeqId").value.toString();
		
		data_row[data_row.length] = "CIFClass~2";
		data_row[data_row.length] = "Lastname~"         + this.Lastname;
		data_row[data_row.length] = "Firstname~"        + this.Firstname;
		data_row[data_row.length] = "Middlename~"       + this.Middlename;
		data_row[data_row.length] = "Age~"              + this.Age;
		data_row[data_row.length] = "Birthdate~"        + this.Birthdate;
		data_row[data_row.length] = "Birthplace~"       + this.Birthplace;
		data_row[data_row.length] = "TelNo~"            + this.TelNo;
		data_row[data_row.length] = "MobileNo~"         + this.MobileNo;
		data_row[data_row.length] = "Email~"            + this.Email;		
		data_row[data_row.length] = "CompanyName~"      + this.Company;
		data_row[data_row.length] = "Occupation~"       + this.Occupation;
		data_row[data_row.length] = "Email~"            + this.Email;
		
		//  FOR HOME ADDRESS
		data_row[data_row.length] = "Address1~"         + this.Address1;
		data_row[data_row.length] = "Address2~"         + this.Address2;
		data_row[data_row.length] = "POBox~"            + this.POBox;		
		data_row[data_row.length] = "PreferredMailAdd~" + this.PreferredMailAdd;
		data_row[data_row.length] = "AddressType~"      + this.AddressType;
		data_row[data_row.length] = "ZipCode~"          + this.ZipCode;
		
		//  FOR COMPANY ADDRESS
		data_row[data_row.length] = "Address1_Com~"         + this.Address1_Com;
		data_row[data_row.length] = "Address2_Com~"         + this.Address2_Com;
		data_row[data_row.length] = "POBox_Com~"            + this.POBox_Com;		
		data_row[data_row.length] = "PreferredMailAdd_Com~" + this.PreferredMailAdd_Com;
		data_row[data_row.length] = "AddressType_Com~"      + this.AddressType_Com;
		data_row[data_row.length] = "ZipCode_Com~"          + this.ZipCode_Com;
		
		//  FOR MAIL ADDRESS
		data_row[data_row.length] = "Address1_Mail~"         + this.Address1_Mail;
		data_row[data_row.length] = "Address2_Mail~"         + this.Address2_Mail;
		data_row[data_row.length] = "POBox_Mail~"            + this.POBox_Mail;		
		data_row[data_row.length] = "PreferredMailAdd_Mail~" + this.PreferredMailAdd_Mail;
		data_row[data_row.length] = "AddressType_Mail~"      + this.AddressType_Mail;
		data_row[data_row.length] = "ZipCode_Mail~"          + this.ZipCode_Mail;
		
		data_row[data_row.length] = "BankCode~"         +  ((this.BankName == null) ? this.OtherBankName : this.BankName);
		data_row[data_row.length] = "BranchCode~"       + this.BranchCode;   
		data_row[data_row.length] = "AccountNo~"        + this.AccountNo;     
		data_row[data_row.length] = "AccountCurrency~"  + this.AccountCurrency;   
		data_row[data_row.length] = "AccountType~"      + this.AccountType;   
		data_row[data_row.length] = "ProductCode~"      + this.ProductCode;
		
		if (this.insert_flag == true)
		{
		    data_row[data_row.length] = "CreatedBy~"        + this.CreatedBy;
		    data_row[data_row.length] = "DateCreated~"      + this.DateCreated;
		    data_row[data_row.length] = "Status~0";
		}
		 
		data_row[data_row.length] = "RemitterCIFID~"    + this.RemitterCIFID; 
		data_row[data_row.length] = "DetailedDesc~"     + this.DetailedDesc;   
		data_row[data_row.length] = "WorkerType~"       + this.WorkerType;   
		data_row[data_row.length] = "Salary~"           + this.AnnualSalary; 
		data_row[data_row.length] = "SalaryCurrency~"   + this.SalCurrency;
		data_row[data_row.length] = "Relationship~"     + this.Relationship;
		data_row[data_row.length] = "NotificationMode~" + this.NotificationMode;
		data_row[data_row.length] = "PreferredContact~" + this.PreferredContact;
		data_row[data_row.length] = "GFCCArdNo~" + this.GFCCArdNo;
		//data_row[data_row.length] = "BankCode~" + this.OtherBankName;
		/* default is 0 - for approval
		   in case, editing of beneficiary information will be allowed, delete this default value */
		   	   
		return data_row.join("|");       
	}
	
	this.set_plist = function()
	{
	    if(this.MobileNo == '') this.MobileNo = '0';
	    if(this.TelNo == '')    this.TelNo    = '0';
	    
	    var plist = '';
	    
	    plist += "CIFType~" + this.CIFType + '|';
	    plist += "CIFClass~2|";
	    plist += "IRSWorldSystem~1|";
	    //plist += "PreferredNo~True|";
	    plist += "PreferredNo~" + ((this.PreferredContact == '7') ? "True^False" : "False^True") + "|";	    
	    plist += "Remarks~Internet Beneficiary|";
	    
	    if(this.BranchCode != null && this.BranchCode != '' && this.BranchCode != undefined)	 plist += "BranchCode~"    + this.BranchCode + '|';
	    if(this.Lastname != '')	     plist += "LastName~"       + this.Lastname + '|';
	    if(this.Firstname != '')	 plist += "FirstName~"      + this.Firstname + '|';
	    if(this.Middlename != '')	 plist += "MiddleName~"     + this.Middlename + '|';
	    if(this.Birthdate != '')	 plist += "BirthDate~"      + this.Birthdate + '|';
	    if(this.Birthplace != '')	 plist += "Birthplace~"     + this.Birthplace + '|';
	    if(this.Alias != '')	     plist += "Alias~"          + this.Alias + '|';	    
	    
	    //  FOR HOME ADDRESS
	    plist += "Address1~"       + ((this.Address1.length > 50) ? this.Address1.substring(0,50) : this.Address1) + '|';
	    plist += "Address2~"       + ((this.Address2.length > 50) ? this.Address2.substring(0,50) : this.Address2) + '|';
	    plist += 'Address3~|';
	    plist += 'Address4~|';
	    plist += 'Address5~|';
	    plist += 'Address6~|';
	    plist += 'RemZip1Code~|';
	    if(this.AddressType != 'null'       && this.AddressType != '')	    plist += "AddressType~"    + this.AddressType + '|';
	    plist += "PreferredMailAdd~" + this.PreferredMailAdd + '|';
	    plist += "ZipCode~"        + this.ZipCode + '|';
	    plist += "POBox~"          + this.POBox + '|';
	    if(this.AddSeqID != '' && this.AddSeqID != 'null' && this.AddSeqID != null)        plist += "AddSeqID~"  + this.AddSeqID + '|';
	    
	    //  FOR COMPANY ADDRESS
	    plist += "Address1_Com~"       + ((this.Address1_Com.length > 50) ? this.Address1_Com.substring(0,50) : this.Address1_Com) + '|';
	    plist += "Address2_Com~"       + ((this.Address2_Com.length > 50) ? this.Address2_Com.substring(0,50) : this.Address2_Com) + '|';
	    plist += 'Address3_Com~|';
	    plist += 'Address4_Com~|';
	    plist += 'Address5_Com~|';
	    plist += 'Address6_Com~|';
	    plist += 'RemZip1Code_Com~|';
	    plist += "PreferredMailAdd_Com~" + this.PreferredMailAdd_Com + '|';
	    if(this.AddressType_Com != 'null'       && this.AddressType_Com != '')	    plist += "AddressType_Com~"    + this.AddressType_Com + '|';
	    plist += "ZipCode_Com~"        + this.ZipCode_Com + '|';
	    plist += "POBox_Com~"          + this.POBox_Com + '|';
	    if(this.AddSeqID_Com != '' && this.AddSeqID_Com != 'null' && this.AddSeqID_Com != null)        plist += "AddSeqID_Com~"  + this.AddSeqID_Com + '|';
	    
	    //  FOR MAILING ADDRESS
	    plist += "Address1_Mail~"       + ((this.Address1_Mail.length > 50) ? this.Address1_Mail.substring(0,50) : this.Address1_Mail) + '|';
	    plist += "Address2_Mail~"       + ((this.Address2_Mail.length > 50) ? this.Address2_Mail.substring(0,50) : this.Address2_Mail) + '|';
	    plist += 'Address3_Mail~|';
	    plist += 'Address4_Mail~|';
	    plist += 'Address5_Mail~|';
	    plist += 'Address6_Mail~|';
	    plist += 'RemZip1Code_Mail~|';
	    plist += "PreferredMailAdd_Mail~" + this.PreferredMailAdd_Mail + '|';
	    if(this.AddressType_Mail != 'null'       && this.AddressType_Mail != '')	    plist += "AddressType_Mail~"    + this.AddressType_Mail + '|';
	    plist += "ZipCode_Mail~"        + this.ZipCode_Mail + '|';
	    plist += "POBox_Mail~"          + this.POBox_Mail + '|';
	    if(this.AddSeqID_Mail != '' && this.AddSeqID_Mail != 'null' && this.AddSeqID_Mail != null)        plist += "AddSeqID_Mail~"  + this.AddSeqID_Mail + '|';
	    
	    plist += "ContactNo~"   + this.MobileNo + '^' + this.TelNo + '|';	    
	    if(this.Email != '')	     plist += "EmailAddress~"   + this.Email + '|';
	    if(this.Company != '')	     plist += "Employer~"       + this.Company + '|';
	    if(this.RemitterCIFID != '') plist += "RemitterCIFID~"  + this.RemitterCIFID + '|';
	    if(this.CIFID != '')	     plist += "CIFID~"          + this.CIFID + '|';
	    if(this.DetailedDesc != '')	 plist += "DetailedDesc~"   + this.DetailedDesc + '|';
	    if(this.AnnualSalary != '')	 plist += "Salary~"         + this.AnnualSalary + '|';	    
	    if(this.Occupation != 'null'        && this.Occupation != '')       plist += "Occupation~"     + this.Occupation + '|';
	    if(this.WorkerType != 'null'        && this.WorkerType != '')       plist += "WorkerType~"         + this.WorkerType + '|';
	    if(this.SalCurrency != 'null'       && this.SalCurrency != '')	    plist += "OccCurrency~"        + this.SalCurrency + '|';
	    if(this.Relationship != 'null'      && this.Relationship != '')	    plist += "Relationship~"       + this.Relationship + '|';
	    if(this.NotificationMode != 'null'  && this.NotificationMode != '')	plist += "NotificationMode~"   + this.NotificationMode + '|';
	    
	    //if(this.PreferredContact != 'null'  && this.PreferredContact != '')	plist += "PreferredContact~"   + this.PreferredContact + '|';	    
    
	    return plist.substring(0, plist.length-1);
	}
	
	this.insert_record = function()
	{   
	    IRSWorld.Internet.InternetAddBeneficiary.insert_record(this.insert_record_callback);
		this.clear_screen();
		this.disable_field(false);
		this.color_asterisks();
		
		document.getElementById("trAcctNo").style.display      = 'none';
        document.getElementById("trTarCurrency").style.display = 'none';
        document.getElementById("trGFCNo").style.display = 'none';
        document.getElementById("trNoteAcctValidation").style.display = 'none';
        document.getElementById("trOtherBankName").style.display = 'none';
	}  
	
	this.insert_record_callback = function(result)
	{       
		if (result != null && result.value != "undefined")
		{
			if (result.value == false)
			{
				var msg = IRSWorld.Internet.InternetAddBeneficiary.read_message(); alert(msg.value); return;
			}			 
		} 
	}
	
	this.edit_bene = function()
    {
        Internet_Remitter_Main.check_session();
        
        if(Internet_Remitter_Main.check_gfc_user(document.getElementById("txtHiddenSelectedBeneCIFID").value))
        {
            this.disable_field(false);
            this.color_asterisks();
            this.save_mode_bene_fromedit();
            this.insert_flag = false;
        }        
    }
    
    this.edit_bene_2 = function()
    {
        Internet_Remitter_Main.check_session();
            document.getElementById("tblBeneInfo").style.visibility = "visible";
           document.getElementById("tblBeneInfo").style.display = "block";
           document.getElementById("tblGridBene").style.visibility = "hidden";
           document.getElementById("btnSenReq").style.visibility = "visible";
document.getElementById("btnCancelBene2").style.visibility = "visible";
           document.getElementById("tblGridBene").style.display = "none";
           document.getElementById("tblGridBeneCIF").style.visibility = "hidden";
           document.getElementById("tblGridBeneCIF").style.display = "none";
        if(Internet_Remitter_Main.check_gfc_user(document.getElementById("txtHiddenSelectedBeneCIFID").value))
        {
            this.disable_field(false);
            this.color_asterisks();
            this.save_mode_bene_fromedit_2();
            this.insert_flag = false;
            this.disable_fields_for_edit();
        }        
    } 
      
    this.disable_fields_for_edit = function()
    {
        var res = IRSWorld.Internet.InternetAddBeneficiary.get_web_remittance_bene_non_editable_fields();
        var defaultNonEditableFields = 'ddBProductType~txtBAccountNo~txtBGFCNo~ddBTargetCurr~txtBRuralBankName~ddBOtherBankName~chkIsRuralBankBene';
        if (res.error != null)
        {
            alert(res.error.Message);
        }
        else
        {
            var ds = res.value;
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                for (var i = 0; i < ds.Tables[0].Rows.length; i++)
                {
                    var o = document.getElementById(ds.Tables[0].Rows[i].FldCode);
                    if (o.nodeName == 'INPUT')
                    {
                        o.readOnly = true;
                        if (o.id == 'txtBeneBDate')
                        {
                            document.getElementById('calBeneBDate').disabled = true;
                        }
                        else if(o.id == 'txtBeneZip')
                        {
                            document.getElementById('Button6').disabled = true;
                        }
                        else if(o.id == 'chkIsRuralBankBene')
                        {
                            document.getElementById('chkIsRuralBankBene').disabled = true;
                        }
                    }
                    else if(o.nodeName == 'SELECT')
                    {
                        o.disabled = true;
                    }
                }
            }
        }
        var defaultNonEditableFieldsList = defaultNonEditableFields.split('~');
                
        for (var j = 0; j < defaultNonEditableFieldsList.length; j++)
        {
            var p = document.getElementById(defaultNonEditableFieldsList[j]);
            if (p.nodeName == 'INPUT')
            {
                p.readOnly = true;
                if (p.id == 'txtBeneBDate')
                {
                    document.getElementById('calBeneBDate').disabled = true;
                }
                else if(p.id == 'txtBeneZip')
                {
                    document.getElementById('Button6').disabled = true;
                }
                else if(p.id == 'chkIsRuralBankBene')
                {
                    document.getElementById('chkIsRuralBankBene').disabled = true;
                }
            }
            else if(p.nodeName == 'SELECT')
            {
                p.disabled = true;
            }
        }
    }
      
	this.btn_add_bene_cif = function()
	{
	    Internet_Remitter_Main.check_session();
	    document.getElementById("txtHiddenSelectedBeneCIFID").value = '';
	    document.getElementById("txtHiddenBeneCIFID").value = '';
	    document.getElementById("txtHiddenZipSeqId").value = '';
	    
	    this.clear_screen();
		this.disable_field(false);
		this.color_asterisks();
		this.save_mode_bene();
		this.insert_flag = true;
		Internet_Remitter_Main.disable_cif_type2(false);
		
		this.set_branchdef_values();
		this.set_contact_def_values();
	}
	  
	this.set_contact_def_values = function()
    {
        var contacts = "txtBeneTelNo~txtBeneMobileNo~txtBeneBTelNo~txtBeneBMobileNo";        

        var conts = contacts.split("~");
        var i=0;        
        
        while (i < conts.length) /*while loop naman, lagi na lng for loop eh*/
        {
            document.getElementById(conts[i]).value = '0';
            i++;
        }    
    }
      
	this.btn_add_bene_req = function()
	{
	    Internet_Remitter_Main.check_session();
	    this.insert_record();
	    this.disable_field(false);
	    this.color_asterisks();
	    this.clear_screen();
	    this.save_mode_bene();
	    //this.show_bene_comments();
document.getElementById("btnSenReq").style.visibility = "visible";
document.getElementById("btnCancelBene2").style.visibility = "visible";
	    document.getElementById("tblBeneInfo").style.visibility = "visible";
	    document.getElementById("tblBeneInfo").style.display = "block";
	    document.getElementById("tblGridBene").style.visibility = "hidden";
        document.getElementById("tblGridBene").style.display = "none";
        document.getElementById("tblGridBeneCIF").style.visibility = "hidden";
        document.getElementById("tblGridBeneCIF").style.display = "none";
	    try{document.getElementById("txtBeneLName").focus()}catch(e){};
	    
	}
	
	this.btn_cancel_bene = function()
    {
        this.clear_screen();   
        this.disable_field(true);
        this.color_asterisks();
        this.add_mode_bene();
        //this.show_bene_comments();
    }
    
    this.save_mode_bene = function()
    {
        document.getElementById("btnAddBene1").disabled    = true;
        document.getElementById("btnAddBene2").disabled    = true;
        document.getElementById("btnEditBene1").disabled   = false;
        document.getElementById("btnSenReq").disabled      = false;
        document.getElementById("btnCancelBene2").disabled = false;
        document.getElementById("btnSaveBene1").disabled   = false;
        document.getElementById("btnCancelBene1").disabled = false;
        document.getElementById("btnEditBene2").disabled   = true
        
        this.insert_flag = true;
    }
    
	this.save_mode_bene_fromedit = function()
    {
        document.getElementById("btnAddBene1").disabled    = true;
        document.getElementById("btnEditBene1").disabled   = true;
        document.getElementById("btnSaveBene1").disabled   = false;
        document.getElementById("btnCancelBene1").disabled = false;
    }
    
	this.save_mode_bene_fromedit_2 = function()
    {
        document.getElementById("btnAddBene2").disabled    = true;
        document.getElementById("btnEditBene2").disabled   = true;
        document.getElementById("btnSenReq").disabled   = false;
        document.getElementById("btnCancelBene2").disabled = false;
        
        this.insert_flag = false;
    }    
      
    this.add_mode_bene = function()
    {
        document.getElementById("btnAddBene1").disabled    = false;
        document.getElementById("btnAddBene2").disabled    = false;
        document.getElementById("btnEditBene1").disabled   = false;
        document.getElementById("btnSenReq").disabled      = true;
        document.getElementById("btnCancelBene2").disabled = true;
        document.getElementById("btnSaveBene1").disabled   = true;
        document.getElementById("btnCancelBene1").disabled = true;
        document.getElementById("btnEditBene2").disabled   = false;
        
        document.getElementById("trNoteAcctValidation").style.display = 'none';
    }
    
    this.show_tr_loading = function(isVisible)
	{
	    if(isVisible == true)
	    {
	        document.getElementById('trLoading2').style.display = 'block';
	    }
	    else
	    {
	        document.getElementById('trLoading2').style.display = 'none';
	    }
	}
	
	this.set_branchdef_values = function()
    {
        var ds = IRSWorld.Internet.InternetAddBeneficiary.get_branchdef_values(document.getElementById("txtHiddenUserLoc").value).value;
        
        if(ds != null && ds.Tables[0].Rows.length > 0)
        {
            this.commons = new common_method();
            
            if(document.getElementById("ddCIFType2").value == 1)
            {
                document.getElementById("ddBeneRel").value     = ds.Tables[0].Rows[0].Relationship;
                document.getElementById("ddBeneNotMode").value = ds.Tables[0].Rows[0].NotificationMode;
            }
            else
            {
                document.getElementById("ddBeneBRel").value     = ds.Tables[0].Rows[0].Relationship;
                document.getElementById("ddBeneBNotMode").value = ds.Tables[0].Rows[0].NotificationMode;
            }
            this.commons = null;
        }
    }
    
    this.BeneOccupationChanged = function()
    {
        var Occupation = null;
        Occupation = document.getElementById('ddBeneOccupation').value;
        document.getElementById('trDetailedDescription').style.display = 'none';
        if (Occupation != null && Occupation.toString() == '75')
        {
            document.getElementById('trDetailedDescription').style.display = 'block';
        }
    }
    
    this.add_to_rems_bene = function()
    {
        var bene_cifid = document.getElementById("txtHiddenSelectedBeneCIFID").value;
        var rem_cifid  = null;
        var ret_val = '';
        
        rem_cifid = this.get_rem_cifid();
        
        plist = "CIFID~" + bene_cifid + "|RemitterCIFID~" + rem_cifid;
        
        var ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_bene(plist).value;
	        
        if(ret_val.length > 0 && ret_val.length <= 16)
        {
            alert("BENEFICIARY RECORD COMMITTED SUCCESSFULLY!");
        }
        else alert(ret_val + '\n' + "Please try again.");
    }
    
    this.btn_save_beneficiary = function()
	{
	    Internet_Remitter_Main.check_session();
	    if(Internet_Remitter_Main.check_mandatory_fields() == true)
	    {
	        if (this.validate_input() == true)
	        {   
                if(ciftype_b == 2) /*business type*/
                {
                    Internet_Remitter_Main.check_and_get_hitlist(document.getElementById("txtBeneShortName").value, document.getElementById("txtBeneBusinessName").value, document.getElementById("txtBeneBBusType").value, 2);
                }
                else
                {
                    Internet_Remitter_Main.check_and_get_hitlist(document.getElementById("txtBeneFName").value, document.getElementById("txtBeneLName").value, document.getElementById("txtBeneMName").value, 2);
                }
	            
//	            var bcifid = '';
//    	        
//	            if(this.insert_flag == true)
//	            {
//	                var ret_val = IRSWorld.Internet.InternetAddBeneficiary.enroll_agent(this.set_ben_record(false)).value;
//    	        
//	                if(ret_val.length > 0 && ret_val.length <= 16)
//	                {
//	                    var rcifid = this.get_rem_cifid();
//	                    if(fxn_isRemit == 'otc')
//	                    {
//	                        ret_otc = rcifid + '|' + ret_val;
//	                    }
//	                    else if (fxn_isRemit == 'gfc')
//	                    {
//	                        ret_gfc = rcifid + '|' + ret_val;
//	                    }
//	                    document.getElementById("txtHiddenSelectedBeneCIFID").value = ret_val;
//	                    var ds = IRSWorld.Internet.InternetAddBeneficiary.get_master_seqid(ret_val).value;
//	                    if (ds != null && ds.Tables[0].Rows.length > 0)
//                        {
//                            document.getElementById("txtHiddenBeneMasterSeqId").value = ds.Tables[0].Rows[0].SEQ_ID;
//                        }
//                        this.disable_field(true);
//                        this.color_asterisks();
//	                    this.add_mode_bene();
//	                    Internet_Remitter_Main.disable_cif_type2(true);
//	                    
//	                    //for soundex saving
//	                    IRSWorld.Internet.InternetAddBeneficiary.save_soundexlist(ret_val, this.Firstname, this.Lastname, this.Middlename, 2);
//	                    
//	                    alert("BENEFICIARY RECORD COMMITTED SUCCESSFULLY!")
//	                }
//	                else alert(ret_val + '\n' + "Please try again.");
//	            }
//	            else //if (this.insert_flag == false)
//	            {
//	                var ret_val = IRSWorld.Internet.InternetAddBeneficiary.update_agent(this.set_ben_record(false)).value;
//    	        
//	                if(ret_val.length > 0 && ret_val.length <= 16)
//	                {
//	                    /*update beneficiry details: relationship and notification mode*/
//	                    IRSWorld.Internet.InternetAddBeneficiary.update_rel_notmode(ret_val, this.get_rem_cifid());
//	                    
//	                    document.getElementById("txtHiddenSelectedBeneCIFID").value = ret_val;
//	                    var ds = IRSWorld.Internet.InternetAddBeneficiary.get_master_seqid(ret_val).value;
//	                    if (ds != null && ds.Tables[0].Rows.length > 0)
//                        {
//                            document.getElementById("txtHiddenBeneMasterSeqId").value = ds.Tables[0].Rows[0].SEQ_ID;
//                        }
//                        
//                        this.disable_field(true);
//                        this.color_asterisks();
//	                    this.add_mode_bene();
//	                    Internet_Remitter_Main.disable_cif_type2(true);
//	                    
//	                    //for soundex saving
//	                    IRSWorld.Internet.InternetAddBeneficiary.save_soundexlist(ret_val, this.Firstname, this.Lastname, this.Middlename, 2);	                    
//	                    
//	                    alert("BENEFICIARY RECORD COMMITTED SUCCESSFULLY!")
//	                }
//	                else alert(ret_val + '\n' + "Please try again.");
//	            }
//	            /*else
//	            {
//	                this.add_to_rems_bene();
//	            }*/
//    	        
//	            this.load_grid_viewer_cifbene();
	        }
	    }	    	    
	}
	
    this.save_beneficiary = function()
    {
        var bcifid = '';
        if(this.insert_flag == true)
        {
            var ret_val = IRSWorld.Internet.InternetAddBeneficiary.enroll_agent(this.set_ben_record(false)).value;
        
            if(ret_val.length > 0 && ret_val.length <= 16)
            {
                var rcifid = this.get_rem_cifid();
                if(fxn_isRemit == 'otc')
                {
                    ret_otc = rcifid + '|' + ret_val;
                }
                else if (fxn_isRemit == 'gfc')
                {
                    ret_gfc = rcifid + '|' + ret_val;
                }
                document.getElementById("txtHiddenSelectedBeneCIFID").value = ret_val;
                var ds = IRSWorld.Internet.InternetAddBeneficiary.get_master_seqid(ret_val).value;
                if (ds != null && ds.Tables[0].Rows.length > 0)
                {
                    document.getElementById("txtHiddenBeneMasterSeqId").value = ds.Tables[0].Rows[0].SEQ_ID;
                }
                this.disable_field(true);
                this.color_asterisks();
                this.add_mode_bene();
                Internet_Remitter_Main.disable_cif_type2(true);
                
                //for soundex saving
                IRSWorld.Internet.InternetAddBeneficiary.save_soundexlist(ret_val, this.Firstname, this.Lastname, this.Middlename, 2);
                
                Internet_Beneficiary.pull_server_bene_info(2);
                alert("BENEFICIARY RECORD COMMITTED SUCCESSFULLY!")
            }
            else alert(ret_val + '\n' + "Please try again.");
        }
        else //if (this.insert_flag == false)
        {
            var ret_val = IRSWorld.Internet.InternetAddBeneficiary.update_agent(this.set_ben_record(false)).value;
        
            if(ret_val.length > 0 && ret_val.length <= 16)
            {
                /*update beneficiry details: relationship and notification mode*/
                IRSWorld.Internet.InternetAddBeneficiary.update_rel_notmode(ret_val, this.get_rem_cifid());
                
                document.getElementById("txtHiddenSelectedBeneCIFID").value = ret_val;
                var ds = IRSWorld.Internet.InternetAddBeneficiary.get_master_seqid(ret_val).value;
                if (ds != null && ds.Tables[0].Rows.length > 0)
                {
                    document.getElementById("txtHiddenBeneMasterSeqId").value = ds.Tables[0].Rows[0].SEQ_ID;
                }
                
                this.disable_field(true);
                this.color_asterisks();
                this.add_mode_bene();
                Internet_Remitter_Main.disable_cif_type2(true);
                
                //for soundex saving
                IRSWorld.Internet.InternetAddBeneficiary.save_soundexlist(ret_val, this.Firstname, this.Lastname, this.Middlename, 2);	                    
                
                Internet_Beneficiary.pull_server_bene_info(2);
                alert("BENEFICIARY RECORD COMMITTED SUCCESSFULLY!")
            }
            else alert(ret_val + '\n' + "Please try again.");
        }
        
        if (this.isClientBene == true)
        {   
            this.load_grid_viewer();
        }
        else
        {
            this.load_grid_viewer_cifbene();
        }       
    }
	
	this.btn_send_req = function()
	{ 
	    Internet_Remitter_Main.check_session();

	    if(Internet_Remitter_Main.check_mandatory_fields() == true)
	    {
	        if (this.validate_input() == true)
            {
                if (confirm("Save beneficiary details?"))
                {
                    this.commit_record();
                    this.disable_field(true);
                    this.color_asterisks();
                    this.add_mode_bene();
                     
                }
            }
	    }	
//	    Internet_Remitter_Main.check_session();
//	    	    
//	    if(Internet_Remitter_Main.check_mandatory_fields() == true)
//	    {
//	        if (this.validate_input() == true)
//            {
//                this.insert_flag = true;
//                this.isClientBene = true;
//                this.approve_bene();
//            }
//	    }        
	}	
	
	this.commit_record = function()
	{	
	
        Internet_Beneficiary.showTransactionLoading("Please wait while saving beneficiary record....",false);	
	    if (this.insert_flag == true)
	    {
	        AjaxPro.timeoutPeriod = 100 * 1000;

            AjaxPro.onTimeout = function()
            {
                alert('SESSION TIMEOUT');
                return; 
            }
            	    
	        IRSWorld.Internet.InternetAddBeneficiary.commit_record(this.set_ben_record(true),this.commit_record_callback);
	    }
	    else
	    {
	        AjaxPro.timeoutPeriod = 100 * 1000;

            AjaxPro.onTimeout = function()
            {
                alert('SESSION TIMEOUT');
                return; 
            }
            
            if (document.getElementById("txtHiddenSelectedBeneCIFID").value != null && document.getElementById("txtHiddenSelectedBeneCIFID").value != '' && document.getElementById("txtHiddenSelectedBeneCIFID").value != 'null' && document.getElementById("txtHiddenSelectedBeneCIFID").value != undefined)
            { 
	            IRSWorld.Internet.InternetAddBeneficiary.update_agent(this.set_ben_record(false), this.commit_record_update_callback);
	        }
	        else
	        {
	            var seq_id = document.getElementById("txtHiddenBeneSeqId").value;
	            seq_id = (seq_id < 0) ? (seq_id * -1) : seq_id;
	            
	            if (IRSWorld.Internet.InternetAddBeneficiary.update_record(seq_id.toString()).value == true)
	            {
	                IRSWorld.Internet.InternetAddBeneficiary.commit_record(this.set_ben_record(true),this.commit_record_update_callback);
	            }
	        }
	    }
	}
	
	this.commit_record_update_callback = function(result)
	{
	    if (result.error != null)
	    {
	        alert(result.error.Message);
	    }
	    else
	    {
	       iBene.load_grid_viewer();
	       alert("Change of information is successful.");
	       Internet_Beneficiary.load_record();
	    }
	    Internet_Beneficiary.showTransactionLoading("",true);
	}

	this.commit_record_callback = function(result)
	{
		if (result != null && result.value != "undefined")
		{        
			if (result.value == false)
			{
				var msg = IRSWorld.Internet.InternetAddBeneficiary.read_message(); alert(msg.value); return;
			}
			
			this.SEQ_ID = IRSWorld.Internet.InternetAddBeneficiary.INTERNET_REMITTER_SEQ_ID().value;
			
			//check if hitlisted
			var res = IRSWorld.Internet.InternetAddBeneficiary.check_and_get_hitlist(document.getElementById("txtBeneFName").value, document.getElementById("txtBeneLName").value, document.getElementById("txtBeneMName").value);
			if (res.value != null && res.value != '')
			{
			    // This client is hitlisted and will undergo approval process.
			    if (isAgentClient == false)
			    {
			        alert("Enrollment request sent successfully. Kindly check status after 24 hours or call branch.");
			    }
			    else
			    {   
			        var msg = 'Beneficiary Name: ' + document.getElementById("txtBeneFName").value + ' ' +  document.getElementById("txtBeneMName").value + ' ' + document.getElementById("txtBeneLName").value + '\nis in the WatchList'
			        alert(msg);
			        Internet_Beneficiary.approve_bene(this.SEQ_ID);
			    }
			}   
	        else
            {
                //Auto approve this client
                //Internet_Beneficiary.auto_approve_bene(this.SEQ_ID);
                Internet_Beneficiary.approve_bene(this.SEQ_ID);
                //alert("Enrollment request sent successfully. Kindly check status after 24 hours or call branch.");
            }      
		}
		Internet_Beneficiary.showTransactionLoading("",true);
		iBene.load_grid_viewer();
	}
	
	
	this.load_benecif_record = function(param) 
	{
	    this.clear_screen();
	    this.disable_field(true);
	    this.color_asterisks();
	    this.add_mode_bene();
	    this.pull_server_bene_info(param);
	}
	
	this.pull_server_bene_info = function(param)
	{
	    if(param == 2)
	    {
	        IRSWorld.Internet.InternetAddBeneficiary.get_clientbene_info(document.getElementById("txtHiddenBeneMasterSeqId").value, this.get_rem_cifid(), this.pull_server_bene_info_callback);
	    }
	    else
	    {
	        //IRSWorld.Internet.InternetAddBeneficiary.get_client_info(document.getElementById("txtHiddenBeneMasterSeqId").value, this.pull_server_bene_info_callback);
	        IRSWorld.Internet.InternetAddBeneficiary.get_clientbene_info(document.getElementById("txtHiddenBeneMasterSeqId").value, '', this.pull_server_bene_info_callback);
	    }	    
	}
	
	this.pull_server_bene_info_callback = function(result)
	{
	    if (result != null && result.value != null)
        {
            //this.commons = new common_method();
            this.generic = new generic_method();
            
            var ds = result.value;
            var chkPrefMailAdd = null;
            
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                Internet_Remitter_Main.disable_cif_type2(true);
                Internet_Remitter_Main.set_cif_type2(ciftype_b);
                
                document.getElementById("txtHiddenSelectedBeneCIFID").value = ds.Tables[0].Rows[0].CIFID;
                
                Internet_Beneficiary.BranchCode = ds.Tables[0].Rows[0].BranchCode;
                
                if(ciftype_b == 2) /*business type*/
                {
                    chkPrefMailAdd = "chkBeneBPrefMailAdd";
                    document.getElementById("txtBeneBusinessName").value = ds.Tables[0].Rows[0].LastName;
                    document.getElementById("txtBeneShortName").value    = ds.Tables[0].Rows[0].FirstName;
                    document.getElementById("txtBeneBBusType").value     = ds.Tables[0].Rows[0].MiddleName;
                    document.getElementById("txtBeneBAlias").value       = ds.Tables[0].Rows[0].Alias;
                    
                    //  FOR HOME ADDRESS
                    document.getElementById("txtBeneBAddress1").value    = ds.Tables[0].Rows[0].Address1;
                    document.getElementById("txtBeneBPOBox").value       = ds.Tables[0].Rows[0].POBox;
                    document.getElementById("txtBeneBAddress2").value    = ds.Tables[0].Rows[0].Address2;
                    Internet_Beneficiary.AddSeqID                        = ds.Tables[0].Rows[0].SEQ_ID_Home;
                    if (ds.Tables[0].Rows[0].ZipCodeSeqId != null && ds.Tables[0].Rows[0].ZipCodeSeqId != 'undefined' && ds.Tables[0].Rows[0].ZipCodeSeqId != '')
                    {
                        document.getElementById("txtHiddenZipSeqId").value = ds.Tables[0].Rows[0].ZipCodeSeqId;
                        document.getElementById("txtBeneBZip").value = ds.Tables[0].Rows[0].ZipCodeDesc;
                    }
                    document.getElementById("chkBeneBPrefMailAdd").value   = (ds.Tables[0].Rows[0].PreferredMailAdd == true) ? "on" : "off";
                    document.getElementById("chkBeneBPrefMailAdd").checked = (ds.Tables[0].Rows[0].PreferredMailAdd == true) ? true : false;
                    
                    //  FOR COMPANY ADDRESS
                    document.getElementById("txtBeneBAddress1_Com").value    = ds.Tables[0].Rows[0].Address1_Com;
                    document.getElementById("txtBeneBPOBox_Com").value       = ds.Tables[0].Rows[0].POBox_Com;
                    document.getElementById("txtBeneBAddress2_Com").value    = ds.Tables[0].Rows[0].Address2_Com;
                    Internet_Beneficiary.AddSeqID_Com                        = ds.Tables[0].Rows[0].SEQ_ID_Com;
                    if (ds.Tables[0].Rows[0].ZipCodeSeqId_Com != null && ds.Tables[0].Rows[0].ZipCodeSeqId_Com != 'undefined' && ds.Tables[0].Rows[0].ZipCodeSeqId_Com != '')
                    {
                        document.getElementById("txtHiddenZipSeqId_Com").value = ds.Tables[0].Rows[0].ZipCodeSeqId_Com;
                        document.getElementById("txtBeneBZip_Com").value = ds.Tables[0].Rows[0].ZipCodeDesc_Com;
                    }
                    document.getElementById("chkBeneBPrefMailAdd_Com").value   = (ds.Tables[0].Rows[0].PreferredMailAdd_Com == true) ? "on" : "off";
                    document.getElementById("chkBeneBPrefMailAdd_Com").checked = (ds.Tables[0].Rows[0].PreferredMailAdd_Com == true) ? true : false;
                    
                    //  FOR MAILING ADDRESS
                    document.getElementById("txtBeneBAddress1_Mail").value    = ds.Tables[0].Rows[0].Address1_Mail;
                    document.getElementById("txtBeneBPOBox_Mail").value       = ds.Tables[0].Rows[0].POBox_Mail;
                    document.getElementById("txtBeneBAddress2_Mail").value    = ds.Tables[0].Rows[0].Address2_Mail;
                    Internet_Beneficiary.AddSeqID_Mail                        = ds.Tables[0].Rows[0].SEQ_ID_Mail;
                    if (ds.Tables[0].Rows[0].ZipCodeSeqId_Mail != null && ds.Tables[0].Rows[0].ZipCodeSeqId_Mail != 'undefined' && ds.Tables[0].Rows[0].ZipCodeSeqId_Mail != '')
                    {
                        document.getElementById("txtHiddenZipSeqId_Mail").value = ds.Tables[0].Rows[0].ZipCodeSeqId_Mail;
                        document.getElementById("txtBeneBZip_Mail").value = ds.Tables[0].Rows[0].ZipCodeDesc_Mail;
                    }
                    document.getElementById("chkBeneBPrefMailAdd_Mail").value   = (ds.Tables[0].Rows[0].PreferredMailAdd_Mail == true) ? "on" : "off";
                    document.getElementById("chkBeneBPrefMailAdd_Mail").checked = (ds.Tables[0].Rows[0].PreferredMailAdd_Mail == true) ? true : false;
                    
                    document.getElementById("txtBeneBTelNo").value       = ds.Tables[0].Rows[0].TelNo;
                    document.getElementById("txtBeneBMobileNo").value    = ds.Tables[0].Rows[0].MobileNo;
                    document.getElementById("txtBeneBEmailAdd").value    = ds.Tables[0].Rows[0].EmailAddress;
                    
                    document.getElementById("ddBeneBRel").value          = ds.Tables[0].Rows[0].Relationship;
                    document.getElementById("ddBeneBNotMode").value      = ds.Tables[0].Rows[0].NotificationMode;
                    document.getElementById("ddBusBenePrefContact").value    = ds.Tables[0].Rows[0].PreferredContact; 
                    
                    this.generic.to_clear_null("txtBeneBusinessName~txtBeneShortName~txtBeneBBusType~txtBeneBAlias~txtBeneBAddress1~txtBeneBAddress2~txtBeneBTelNo~txtBeneBMobileNo~txtBeneBEmailAdd~txtBeneBZip~txtBeneBPOBox");
                    this.generic.to_clear_null("txtBeneBAddress1_Com~txtBeneBAddress2_Com~txtBeneBZip_Com~txtBeneBPOBox_Com");
                    this.generic.to_clear_null("txtBeneBAddress1_Mail~txtBeneBAddress2_Mail~txtBeneBZip_Mail~txtBeneBPOBox_Mail");
                }    
                else /*individual*/
                {
                    chkPrefMailAdd = "chkBenePrefMailAdd";
                    document.getElementById("txtBeneLName").value     = ds.Tables[0].Rows[0].LastName;
                    document.getElementById("txtBeneFName").value     = ds.Tables[0].Rows[0].FirstName;
                    document.getElementById("txtBeneMName").value     = ds.Tables[0].Rows[0].MiddleName;
                    document.getElementById("txtBeneBDate").value     = dateToString(ds.Tables[0].Rows[0].BirthDate).split(' ')[0]; //this.commons.to_mmddyyyy_date(ds.Tables[0].Rows[0].BirthDate); 
                    document.getElementById("txtBeneBPlace").value    = ds.Tables[0].Rows[0].BirthPlace;
                    
                    //  FOR HOME ADDRESS
                    document.getElementById("txtBeneAddress1").value  = ds.Tables[0].Rows[0].Address1;
                    document.getElementById("txtBeneAddress2").value  = ds.Tables[0].Rows[0].Address2;
                    document.getElementById("txtBenePOBox").value     = ds.Tables[0].Rows[0].POBox;
                    Internet_Beneficiary.AddSeqID                        = ds.Tables[0].Rows[0].SEQ_ID_Home;
                    if (ds.Tables[0].Rows[0].ZipCodeSeqId != null && ds.Tables[0].Rows[0].ZipCodeSeqId != 'undefined' && ds.Tables[0].Rows[0].ZipCodeSeqId != '')
                    {
                        document.getElementById("txtHiddenZipSeqId").value = ds.Tables[0].Rows[0].ZipCodeSeqId;
                        document.getElementById("txtBeneZip").value = ds.Tables[0].Rows[0].ZipCodeDesc;
                    }
                    document.getElementById("chkBenePrefMailAdd").value   = (ds.Tables[0].Rows[0].PreferredMailAdd == true) ? "on" : "off";
                    document.getElementById("chkBenePrefMailAdd").checked = (ds.Tables[0].Rows[0].PreferredMailAdd == true) ? true : false;
                    
                    //  FOR COMPANY ADDRESS
                    document.getElementById("txtBeneAddress1_Com").value  = ds.Tables[0].Rows[0].Address1_Com;
                    document.getElementById("txtBeneAddress2_Com").value  = ds.Tables[0].Rows[0].Address2_Com;
                    document.getElementById("txtBenePOBox_Com").value     = ds.Tables[0].Rows[0].POBox_Com;
                    Internet_Beneficiary.AddSeqID_Com                        = ds.Tables[0].Rows[0].SEQ_ID_Com;
                    if (ds.Tables[0].Rows[0].ZipCodeSeqId_Com != null && ds.Tables[0].Rows[0].ZipCodeSeqId_Com != 'undefined' && ds.Tables[0].Rows[0].ZipCodeSeqId_Com != '')
                    {
                        document.getElementById("txtHiddenZipSeqId_Com").value = ds.Tables[0].Rows[0].ZipCodeSeqId_Com;
                        document.getElementById("txtBeneZip_Com").value = ds.Tables[0].Rows[0].ZipCodeDesc_Com;
                    }
                    document.getElementById("chkBenePrefMailAdd_Com").value   = (ds.Tables[0].Rows[0].PreferredMailAdd_Com == true) ? "on" : "off";
                    document.getElementById("chkBenePrefMailAdd_Com").checked = (ds.Tables[0].Rows[0].PreferredMailAdd_Com == true) ? true : false;
                    
                    //  FOR MAILING ADDRESS
                    document.getElementById("txtBeneAddress1_Mail").value  = ds.Tables[0].Rows[0].Address1_Mail;
                    document.getElementById("txtBeneAddress2_Mail").value  = ds.Tables[0].Rows[0].Address2_Mail;
                    document.getElementById("txtBenePOBox_Mail").value     = ds.Tables[0].Rows[0].POBox_Mail;
                    Internet_Beneficiary.AddSeqID_Mail                        = ds.Tables[0].Rows[0].SEQ_ID_Mail;
                    if (ds.Tables[0].Rows[0].ZipCodeSeqId_Mail != null && ds.Tables[0].Rows[0].ZipCodeSeqId_Mail != 'undefined' && ds.Tables[0].Rows[0].ZipCodeSeqId_Mail != '')
                    {
                        document.getElementById("txtHiddenZipSeqId_Mail").value = ds.Tables[0].Rows[0].ZipCodeSeqId_Mail;
                        document.getElementById("txtBeneZip_Mail").value = ds.Tables[0].Rows[0].ZipCodeDesc_Mail;
                    }
                    document.getElementById("chkBenePrefMailAdd_Mail").value   = (ds.Tables[0].Rows[0].PreferredMailAdd_Mail == true) ? "on" : "off";
                    document.getElementById("chkBenePrefMailAdd_Mail").checked = (ds.Tables[0].Rows[0].PreferredMailAdd_Mail == true) ? true : false;
                                        
                    document.getElementById("txtBeneTelNo").value     = ds.Tables[0].Rows[0].TelNo;
                    document.getElementById("txtBeneMobileNo").value  = ds.Tables[0].Rows[0].MobileNo;
                    document.getElementById("txtBeneEmailAdd").value  = ds.Tables[0].Rows[0].EmailAddress;
                    document.getElementById("txtBeneCompany").value   = ds.Tables[0].Rows[0].Employer;
                    document.getElementById("ddBeneOccupation").value = ds.Tables[0].Rows[0].Occupation;
                    
                    document.getElementById("txtBeneDetDesc").value   = ds.Tables[0].Rows[0].DetailedDesc;
                    document.getElementById("ddBeneWorkerType").value = ds.Tables[0].Rows[0].WorkerType;
                    document.getElementById("txtBeneAnnualSal").value = this.generic.formatCurrency(ds.Tables[0].Rows[0].Salary);
                    document.getElementById("ddBeneSalCurr").value    = ds.Tables[0].Rows[0].Currency;
                    document.getElementById("ddBeneRel").value        = ds.Tables[0].Rows[0].Relationship;
                    document.getElementById("ddBeneNotMode").value    = ds.Tables[0].Rows[0].NotificationMode;
                    document.getElementById("txtBeneFCCustId1").value    = ds.Tables[0].Rows[0].OtherIDs;
                    document.getElementById("ddBenePrefContact").value    = ds.Tables[0].Rows[0].PreferredContact;
                    
                    
                    var Occupation = null;
                    Occupation = document.getElementById('ddBeneOccupation').value;
                    document.getElementById('trDetailedDescription').style.display = 'none';
                    if (Occupation != null && Occupation.toString() == '75')
                    {
                        document.getElementById('trDetailedDescription').style.display = 'block';
                    }
                    
                    this.generic.to_clear_null("txtBeneLName~txtBeneFName~txtBeneMName~txtBeneBDate~txtBeneBPlace~txtBeneAddress1~txtBeneAddress2~txtBeneTelNo~txtBenePOBox");
                    this.generic.to_clear_null("txtBeneEmailAdd~txtBeneCompany~txtBeneZip~txtBeneMobileNo~txtBeneAnnualSal~txtBeneDetDesc~txtBeneFCCustId1");
                    this.generic.to_clear_null("txtBeneAddress1_Com~txtBeneAddress2_Com~txtBenePOBox_Com~txtBeneZip_Com");
                    this.generic.to_clear_null("txtBeneAddress1_Mail~txtBeneAddress2_Mail~txtBenePOBox_Mail~txtBeneZip_Mail");
                }
                
                //this.PreferredMailAdd = ds.Tables[0].Rows[0].PreferredMailAdd;
                //document.getElementById(chkPrefMailAdd).value   = "off";
                //document.getElementById(chkPrefMailAdd).checked = false;
                //
                //if (document.getElementById(chkPrefMailAdd).value != null && this.PreferredMailAdd == true)
                //{
                //    document.getElementById(chkPrefMailAdd).value = "on";
                //    document.getElementById(chkPrefMailAdd).checked = true;
                //}            
            }
        }
	}
	
	this.disable_btns = function(isDisableApp, isDisableRej)
	{
	    document.getElementById("btnApproveBene").disabled = isDisableApp;
	    document.getElementById("btnRejectBene").disabled  = isDisableRej;
	}
	
	this.show_panel_app_bene = function()
	{
	    document.getElementById("txtRejectBeneReason").value = '';
	    iBene.clear_fields();
	    showPanel('divBeneficiary');	    
	    this.get_ben_record();
	    this.disable_btns(false, false);
	    
	    var ds = IRSWorld.Internet.InternetAddBeneficiary.GET_INTERNET_REMITTER_BENE_RECORD(this.BENETOAPPSEQID).value;
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {
            if (String(ds.Tables[0].Rows[0].ProductCode) == "10" || String(ds.Tables[0].Rows[0].ProductCode) == "22")
            {
                this.RR_ACCTNO = String(ds.Tables[0].Rows[0].AccountNo);
                hidePanel('divBeneficiary');
                this.IS_EXISTING = false;
                
                if(IRSWorld.Internet.InternetAddBeneficiary.is_existing_accountno("AccountNo~" + this.RR_ACCTNO).value == true)
                {
                    this.IS_EXISTING = true;
                    showPanel('divBeneficiary');
                }
                else
                {
                    this.IS_EXISTING = false;
                    if(IRSWorld.Internet.InternetAddBeneficiary.validate_acctno_fcalgo(this.RR_ACCTNO).value == true)
                    {
                        this.VALIDFC = true;
                        //this.ACCT_DET = IRSWorld.Internet.InternetAddBeneficiary.get_acct_details(this.RR_ACCTNO).value;
                        Internet_Beneficiary.get_acct_details(this.RR_ACCTNO);
                    }
                    else
                    {
                        this.VALIDFC = false;
                        showPanel('divBeneficiary');
                    }
                }                
            }
        }
	}
    
    this.show_div_app = function(isVisible)
    {
        if(isVisible == true)
        {
            document.getElementById("DIVBENEFORAPP").style.display = 'block';
        }
        else
        {
            document.getElementById("DIVBENEFORAPP").style.display = 'none';
        }
    }
    
    this.get_acct_details = function(acctno)
    {
        this.RR_ACCTNO = acctno;
        var algotype   = IRSWorld.Internet.InternetAddBeneficiary.get_algo_type(acctno).value;
        this.IS_KBI    = false;
        
        if(algotype == "KBI") this.IS_KBI = true;
        
        AjaxPro.timeoutPeriod = 100 * 1000;

        AjaxPro.onTimeout = function()
        {
            showPanel('divBeneficiary'); 
            return; 
        } 
        
        IRSWorld.Internet.InternetAddBeneficiary.get_acct_details(acctno, this.get_acct_details_callback);
        this.commons = new common_method();
        this.show_tr_loading(true);
        this.show_div_app(false);
        this.commons.to_show_loading_message('divLoading4');
        setText("lblLoad4", "Retrieving account details. Please wait...");
    }
    
    this.get_acct_details_callback = function(result)
    {
        Internet_Beneficiary.show_tr_loading(false);  
        Internet_Beneficiary.show_div_app(true);     
        var acctno   = Internet_Beneficiary.RR_ACCTNO;
        var algotype = IRSWorld.Internet.InternetAddBeneficiary.get_algo_type(acctno).value;
        
        if(result.value != null)
        {
            Internet_Beneficiary.set_acct_details(algotype, result.value);
        }  
        else showPanel('divBeneficiary'); 
    }
    
    this.set_acct_details = function(algo_type, acct_det)
    {
        this.ACCT_DET = null;
        
        if(acct_det != null && acct_det != 'null')
        {
            this.ACCT_DET = acct_det;
            var acct      = acct_det.split("~");
            
            if(algo_type == "KBI")
            {
                if(acct[4] != null && acct[4] != '')
                {
                    alert("Account no. entered is a KBI account, this will be replaced by an FC account.");
                    this.ACCT_DET += "~" + this.RR_ACCTNO;                    
                }                
            }
            else
            {
                setText('spnBeneProductCurrency', IRSWorld.Internet.InternetAddBeneficiary.get_fld_desc(String(acct[0]), "Currency").value);
            }
            showPanel('divBeneficiary');
        }
    }
    
    this.validate_account = function(acctno)
    {
        this.RR_ACCTNO = acctno;
        var algotype   = IRSWorld.Internet.InternetAddBeneficiary.get_algo_type(acctno).value;
        this.IS_KBI    = false;
        
        if(algotype == "KBI") this.IS_KBI = true;
        
        AjaxPro.timeoutPeriod = 100 * 1000;

        AjaxPro.onTimeout = function()
        {
            showPanel('divBeneficiary'); 
            return; 
        } 
        
        IRSWorld.Internet.InternetAddBeneficiary.get_acct_details(acctno, this.get_acct_details_callback);
        this.commons = new common_method();
        this.show_tr_loading(true);
        this.show_div_app(false);
        this.commons.to_show_loading_message('divLoading4');
        setText("lblLoad4", "Retrieving account details. Please wait...");
    }
    
    this.clear_fields = function()
    {   
        var fields = "spnBeneName~spnBeneBDate~spnBeneEmailAdd~spnBeneProductCode~spnBeneAccountNumber~";
            fields += "spnBeneTelNo~spnBeneMobNo~spnBeneProductCurrency~spnBeneAddress";
        var fld = fields.split('~');
        var i = 0;
        
        for(i=0; i<fld.length; i++)
        {
            setText(fld[i], '');
        }
    }
    
	this.get_ben_record = function()
	{
	    IRSWorld.Internet.InternetAddBeneficiary.GET_INTERNET_REMITTER_BENE_RECORD(this.BENETOAPPSEQID,this.get_bene_rec_callback);
	}
	
	this.get_bene_rec_callback = function(result)
	{
	    if (result != null && result.value != null)
        {    
            var temp_addr = '';
            var ds = result.value;
            this.generic = new generic_method();
            this.commons = new common_method();
                 
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
	            this.CIFClass           = ds.Tables[0].Rows[0].CIFClass;
	            Internet_Beneficiary.Lastname           = ds.Tables[0].Rows[0].Lastname;
	            Internet_Beneficiary.Firstname          = ds.Tables[0].Rows[0].Firstname;
	            Internet_Beneficiary.Middlename         = ds.Tables[0].Rows[0].Middlename;
	            this.Age                = ds.Tables[0].Rows[0].Age;
	            this.Birthdate          = ds.Tables[0].Rows[0].Birthdate;
	            this.Birthplace         = ds.Tables[0].Rows[0].BirthPlace;
	            this.Occupation         = ds.Tables[0].Rows[0].Occupation;
	            this.Address1           = ds.Tables[0].Rows[0].Address1;
	            this.POBox              = ds.Tables[0].Rows[0].POBox;
	            this.Address2           = ds.Tables[0].Rows[0].Address2;
	            this.TelNo              = ds.Tables[0].Rows[0].TelNo;
	            this.MobileNo           = ds.Tables[0].Rows[0].MobileNo;
	            this.Email              = ds.Tables[0].Rows[0].Email;
	            this.ZipCode            = ds.Tables[0].Rows[0].ZipCode;
	            this.AccountNo          = ds.Tables[0].Rows[0].AccountNo;
	            
	            if(this.Address1 == null)   this.Address1 = '';
	            if(this.POBox == null)      this.POBox = '';
	            if(this.Address2 == null)   this.Address2 = '';
	            if(this.Firstname == null)  this.Firstname = '';
	            if(this.Middlename == null) this.Middlename = '';
	            if(this.Lastname == null)   this.Lastname = '';
	            
	            setText('spnBeneName', Internet_Beneficiary.Firstname + ' ' + Internet_Beneficiary.Middlename + ' ' + Internet_Beneficiary.Lastname);
                setText('spnBeneBDate', this.commons.to_mmddyyyy_date(this.Birthdate));
                setText('spnBeneEmailAdd', this.Email);
                setText('spnBeneProductCode', IRSWorld.Internet.InternetAddBeneficiary.get_fld_desc(String(ds.Tables[0].Rows[0].ProductCode), "ProductCode").value);
                
                //if(ds.Tables[0].Rows[0].ProductCode == 10 | ds.Tables[0].Rows[0].ProductCode == 22)
                if (IRSWorld.Internet.InternetAddBeneficiary.check_product_if_req_account(ds.Tables[0].Rows[0].ProductCode.toString()).value == true)
                {
                    setText('spnBeneAccountNumber', this.AccountNo);
                }
                else
                {
                    setText('spnBeneAccountNumber', 'n/a');
                }
                
                if (ds.Tables[0].Rows[0].ProductCode.toString() == '12' || ds.Tables[0].Rows[0].ProductCode.toString() == '13')
                {
                    document.getElementById('trBankName').style.display = 'block';
                    
                    if (ds.Tables[0].Rows[0].BankCode.toString().substring(0,4) == '000-')
                    {
                        setText('spnBeneBankName', ds.Tables[0].Rows[0].BankCode.toString());
                    }
                    else
                    {
                        var res = IRSWorld.Internet.InternetAddBeneficiary.get_bankname(ds.Tables[0].Rows[0].BankCode.toString()).value;
                        if (res != null && res.Tables[0].Rows.length > 0)
                        {
                            setText('spnBeneBankName', ds.Tables[0].Rows[0].BankCode.toString() + '-' + res.Tables[0].Rows[0].BranchName.toString());
                        }
                        else
                        {
                            setText('spnBeneBankName', ds.Tables[0].Rows[0].BankCode.toString());
                        }
                    }
                }
                else
                {
                    document.getElementById('trBankName').style.display = 'none';
                    setText('spnBeneBankName', '');
                }
                
                setText('spnBeneTelNo', this.TelNo);
                setText('spnBeneMobNo', this.MobileNo);
                
                if(ds.Tables[0].Rows[0].AccountCurrency != null)
                {
                    setText('spnBeneProductCurrency', IRSWorld.Internet.InternetAddBeneficiary.get_fld_desc(String(ds.Tables[0].Rows[0].AccountCurrency), "Currency").value);
                }                
                
                temp_addr = this.Address1 + ' ' + this.POBox + ' ' + this.Address2 + ' ';
                
                //var zip = IRSWorld.Internet.InternetAddBeneficiary.get_zip(ds.Tables[0].Rows[0].ZipCode).value;
                var zip = IRSWorld.Internet.InternetAddBeneficiary.get_zipcode(ds.Tables[0].Rows[0].ZipCode).value;
                
                if (zip.Tables[0].Rows.length > 0)
                {
                    temp_addr += zip.Tables[0].Rows[0].municipality + ' ';
                    temp_addr += zip.Tables[0].Rows[0].province + ' ';
                    temp_addr += zip.Tables[0].Rows[0].truezipcode;
                    
                    setText('spnBeneAddress', temp_addr);
                }
                
                this.generic = new generic_method();
                this.generic.to_clear_null("spnBeneName~spnBeneAddress~spnBeneBDate~spnBeneEmailAdd~spnBeneProductCode~spnBeneAccountNumber~spnBeneProductCurrency~spnBeneTelNo~spnBeneMobNo");
                
                setText('lblInfoHeader', "Beneficiary Information");
                setText('lblClientName', "Beneficiary Name");
                document.getElementById('trProdCode').style.display = 'block';
                document.getElementById('trAcctNum').style.display = 'block';
                document.getElementById('trCurrency').style.display = 'block';
                document.getElementById('Button1').style.display = 'none';
                document.getElementById('trSupportingDocs').style.display = 'none';
	        }
	    }
	}
	
	    this.process_grid_rejrem = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_rem_rejected~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVREMREJ");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }    
    
    this.load_grid_viewer_rejrem = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_rem_rejected~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVREMREJ");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }	
	
    
	
	this.get_rem_record = function()
	{
	    IRSWorld.Internet.InternetAddBeneficiary.GET_INTERNET_REMITTER_REM_RECORD(this.REMTOAPPSEQID,this.get_rem_rec_callback);
	}
	
	
	/* Enrolls account of beneficiary */
	this.add_account = function(plist) 
	{
	    /* 
	        IRSWorld.Internet.InternetAddBeneficiary.add_account method returns acctseqid if successful else it returns error msg.
	        If the return value can be converted to number then successful.
	    */
	    var ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_account(plist).value;
	    var tostr_ret_val = String(Number(ret_val));
        
        if(tostr_ret_val == "NaN" || tostr_ret_val == "0") /* can not be converted, with error */
        {
            alert(ret_val + '\n' + "Please try again."); return false;
        }
        else /* successful account enrollment */
        {
            this.ACCTSEQID = tostr_ret_val; return true;
        }
	}
    
    /* BENEFICIARY STATUS
    0 or null - For Approval
    1 - Approved
    2 - Rejected */
    
    this.reject_bene = function()
    {
        var rej_rsn = document.getElementById('txtRejectBeneReason').value;
        if(rej_rsn == '')
        {
            alert('Please enter reason.');
        }
        else
        {
            if(confirm('Reject beneficiary?'))
            {
                IRSWorld.Internet.InternetAddBeneficiary.set_bene_status(String(this.BENETOAPPSEQID), '2', rej_rsn);
                alert('BENEFICIARY REJECTED!');
                Internet_Beneficiary.load_grid_viewer_appbene();
                this.disable_btns(true, true);
            }
        }
    }
    
    /*
        SPAGHETTI CODE!
        comments included for comprehension
    */
//	this.approve_bene = function()
//	{
//        this.set_ben_record(true);
//        var enroll_stat = 0; /* 1=bene enrolled, 2=account enrolled, 3=remitter and bene linked */
//        
//        this.generic = new generic_method();
//        this.commons = new common_method();             
// 
//        var plist = '';
//        var ac_plist = '';
//        
//        if (this.ProductCode == "10") /* Product is Rapid Remit*/
//        {
//            this.RR_ACCTNO = this.AccountNo;
//            this.IS_EXISTING = false;
//            
//            if(IRSWorld.Internet.InternetAddBeneficiary.validate_acctno_fcalgo(this.RR_ACCTNO).value == true)
//            {
//                this.VALIDFC = true;
//                //Internet_Beneficiary.get_acct_details(this.RR_ACCTNO);
//                this.ACCT_DET = IRSWorld.Internet.InternetAddBeneficiary.get_acct_details(this.RR_ACCTNO).value;
//            }
//            else
//            {
//                this.VALIDFC = false;
//            }
//        
//            if(this.IS_EXISTING == false) /* Checks if account number of beneficiary already exists */
//            {
//                if(this.VALIDFC == true) /* Checks if account number of beneficiary is valid FC account*/
//                {
//                    if(this.ACCT_DET != null) /* Account details from FC*/
//                    {
//                        var acctdet = (this.ACCT_DET).split("~");
//                        if(acctdet.length > 0)
//                        {
//                            plist += "AccountCurrency~"  + acctdet[0] + '|';
//                            plist += "AccountType~"      + acctdet[1] + '|';
//                            plist += "AccountStatus~"    + acctdet[2] + '|';
//                            plist += "AccountBranchCode~"+ acctdet[3] + '|';
//                            /* If account number enrolled by remitter is a KBI Account, it will be replaced by an FC account else same value */
//                            plist += "AccountNo~"        + acctdet[4] + '|'; 
//                        }
//                        if(acctdet.length == 6)
//                        { 
//                            plist += "KBIAccount~" + acctdet[5] + '|'; /* The original kbi account */
//                        }                                                        
//                    }   
//                    else /* Unable to retrieve account details from FC */
//                    {
//                        plist += "AccountNo~" + this.AccountNo + '|';
//                        if(this.IS_KBI == true) /* If account number is KBI account, AccountNo and KBIAccount fields are the same. */
//                        {
//                            plist += "KBIAccount~" + this.AccountNo + '|';
//                        }
//                    }
//                }
//                else /* Invalid FC account */
//                {
//                    alert("Account number is an invalid PNB account."); 
//                    //this.disable_btns(true, false);
//                    return;
//                }
//            }
//            else /* Existing account */
//            {
//                alert("Account number already exists. Failed to save beneficiary."); 
//                //this.disable_btns(true, false);
//                return; 
//            }                
//        }
//        else /* Product is not Rapid Remit */
//        {
//            plist += "AccountCurrency~" + this.AccountCurrency + '|';
//        }
//        
//        if (plist != '')
//        {
//            ac_plist = plist; /* save account details to a temp variable */
//            plist = ''; /* free up plist for bene info */
//        }
//        
//        var telno    = '0';
//        var mobileno = '0';
//        
//        plist += "CIFType~1|";
//        plist += "CIFClass~2|";
//        plist += "IRSWorldSystem~1|";
//        plist += "Remarks~Internet Beneficiary|";
//        plist += "PreferredNo~True|";
//        plist += "PreferredMailAdd~True|";
//        if(this.Lastname != '')   plist += "LastName~"    + this.Lastname + '|';
//        if(this.Firstname != '')  plist += "FirstName~"   + this.Firstname + '|';
//        if(this.Middlename != '') plist += "MiddleName~"  + this.Middlename + '|';	    
//        if(this.Birthplace != '') plist += "BirthPlace~"  + this.Birthplace + '|';
//        if(this.Birthdate != '')  plist += "BirthDate~"   + this.Birthdate + '|';	    
//        if(this.Address1 != '')   plist += "Address1~"    + this.Address1 + '|';
//        if(this.POBox != '')      plist += "POBox~"       + this.POBox + '|';
//        if(this.Address2 != '')   plist += "Address2~"    + this.Address2 + '|';
//        if(this.AddressType != 'null' && this.AddressType != '')plist += "AddressType~" + this.AddressType + '|';
//        if(this.ZipCode != '')    plist += "ZipCode~"     + this.ZipCode + '|';
//        if(this.CreatedBy != '')  plist += "CreatedBy~"   + this.CreatedBy + '|';
//        if(this.TelNo != '')      telno    = this.TelNo;
//        if(this.MobileNo != '')   mobileno = this.MobileNo;
//                                                    plist += "ContactNo~"   + mobileno + '^' + telno + '|';
//                                                    plist += "BranchCode~"  + document.getElementById("txtHiddenUserLoc").value + '|';
//        if(this.Email != '')      plist += "EmailAddress~"+ this.Email + '|';
//        if(this.Company != '')plist += "Employer~"    + this.Company + '|';
//        if(this.Occupation != 'null' && this.Occupation != '') plist += "Occupation~"  + this.Occupation + '|';
//        
//        if(this.DetailedDesc != '')   plist += "DetailedDesc~"+ this.DetailedDesc + '|';
//        if(this.WorkerType != 'null' && this.WorkerType != '')     plist += "WorkerType~"  + this.WorkerType + '|';
//        if(this.AnnualSalary != '')         plist += "Salary~"      + this.AnnualSalary + '|';
//        if(this.SalCurrency != 'null' && this.SalCurrency != '') plist += "OccCurrency~" + this.SalCurrency + '|';
//        
//        plist = plist.substring(0, plist.length-1);
//        
//        /* Save beneficiary information except in TBL_CIF_ACCOUNTS and TBL_CIF_BENEFICIARIES*/
//        var ret_val = IRSWorld.Internet.InternetAddBeneficiary.enroll_agent(plist).value;
//        
//        if(ret_val.length > 0 && ret_val.length <= 16) /* Successful saving of beneficiary info */
//        {
//            enroll_stat = 1;
//            ac_plist += "CIFID~" + ret_val; /* Appends CIFID to account details */
//            
//            if(this.add_account(ac_plist) == true) /* Successful saving of account in TBL_CIF_ACCOUNTS */
//            {
//                enroll_stat = 2;
//                /* For saving other bene info in TBL_CIF_BENEFICIARIES */
//                var bene_plist = '';
//                bene_plist += "CIFID~" + ret_val + '|';
//                bene_plist += "ProductCode~"   + this.ProductCode + '|';
//                bene_plist += "ImplementationCode~" + Internet_Remitter_Products.set_implementation_type(String(this.ProductCode)) + '|';
//                bene_plist += "RemitterCIFID~" + this.RemitterCIFID + '|';
//                bene_plist += "AccountSeqId~" + this.ACCTSEQID + '|'; /* Appends accountseqid in bene details */
//                if(this.Relationship != 'null'      && this.Relationship != '')     bene_plist += "Relationship~"      + this.Relationship + '|';
//                if(this.NotificationMode != 'null'  && this.NotificationMode != '') bene_plist += "NotificationMode~"  + this.NotificationMode + '|';
//        
//                bene_plist = bene_plist.substring(0, bene_plist.length-1);
//                
//                var b_ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_bene(bene_plist).value;
//                
//                if(b_ret_val.length > 16) /* error in saving other bene info */
//                {
//                    IRSWorld.Internet.InternetAddBeneficiary.clean_rejected(ret_val); /* clean up rejected bene */
//                    alert(b_ret_val + '\n' + "Please try again."); 
//                    //this.disable_btns(false, false);
//                    return;
//                }
//                else
//                {
//                    enroll_stat = 3;
//                }
//            }
//            else /* error in saving account */ 
//            {
//                IRSWorld.Internet.InternetAddBeneficiary.clean_rejected(ret_val); /* clean up rejected bene */
//                alert("Error saving account" + '\n' + "Please try again.");
//                //this.disable_btns(true, false);
//                return; 
//            }
//            
//            if(enroll_stat == 3)
//            {
//                //IRSWorld.Internet.InternetAddBeneficiary.set_bene_status(String(ds.Tables[0].Rows[0].SEQ_ID), '1', '');
//                //IRSWorld.Internet.InternetAddBeneficiary.set_bene_created_cifid(String(ds.Tables[0].Rows[0].SEQ_ID), ret_val);
//                alert("BENEFICIARY RECORD COMMITTED SUCCESSFULLY!");
//                //Internet_Beneficiary.load_grid_viewer_appbene();
//                //this.disable_btns(true, true);
//                this.disable_field(true);
//                this.color_asterisks();
//                this.add_mode_bene();
//                this.load_grid_viewer();
//            }               
//        }
//        else /* error in enrolling beneficiary */
//        {
//            alert(ret_val + '\n' + "Please try again.");
//        }
//	}
//========================================================================================================================================
//  
//========================================================================================================================================
//	this.approve_bene = function()
//	{
//	    Internet_Remitter_Main.check_session();
//	    
//	    if(confirm('Approve beneficiary?'))
//	    {
//	        var ds = IRSWorld.Internet.InternetAddBeneficiary.GET_INTERNET_REMITTER_BENE_RECORD(this.BENETOAPPSEQID).value;
//	        var enroll_stat = 0; /* 1=bene enrolled, 2=account enrolled, 3=remitter and bene linked */
//	        
//	        var ReqAcct = IRSWorld.Internet.InternetAddBeneficiary.check_product_if_req_account(ds.Tables[0].Rows[0].ProductCode.toString()).value;
//	        
//	        this.generic = new generic_method();
//            this.commons = new common_method();
//                 
//            if (ds != null && ds.Tables[0].Rows.length > 0)
//            {   
//                var plist = '';
//                var ac_plist = '';
//                
//                if (String(ds.Tables[0].Rows[0].ProductCode) == "10") /* Product is Rapid Remit*/
//                {
//                    if(this.IS_EXISTING == false) /* Checks if account number of beneficiary already exists */
//                    {
//                        if(this.VALIDFC == true) /* Checks if account number of beneficiary is valid FC account*/
//                        {
//                            if(this.ACCT_DET != null) /* Account details from FC*/
//                            {
//                                var acctdet = (this.ACCT_DET).split("~");
//                                if(acctdet.length > 0)
//                                {
//                                    plist += "AccountCurrency~"  + acctdet[0] + '|';
//                                    plist += "AccountType~"      + acctdet[1] + '|';
//                                    plist += "AccountStatus~"    + acctdet[2] + '|';
//                                    plist += "AccountBranchCode~"+ acctdet[3] + '|';
//                                    /* If account number enrolled by remitter is a KBI Account, it will be replaced by an FC account else same value */
//                                    plist += "AccountNo~"        + acctdet[4] + '|'; 
//                                }
//                                if(acctdet.length == 6)
//                                { 
//                                    plist += "KBIAccount~" + acctdet[5] + '|'; /* The original kbi account */
//                                }                                                        
//                            }   
//                            else /* Unable to retrieve account details from FC */
//                            {
//                                plist += "AccountNo~" + ds.Tables[0].Rows[0].AccountNo + '|';
//                                if(this.IS_KBI == true) /* If account number is KBI account, AccountNo and KBIAccount fields are the same. */
//                                {
//                                    plist += "KBIAccount~" + ds.Tables[0].Rows[0].AccountNo + '|';
//                                }
//                            }
//                        }
//                        else /* Invalid FC account */
//                        {
//                            alert("Account number is an invalid PNB account."); 
//                            this.disable_btns(true, false);
//                            return;
//                        }
//                    }
//                    else /* Existing account */
//                    {
//                        alert("Account number already exists. Failed to approve beneficiary."); 
//                        this.disable_btns(true, false);
//                        return; 
//                    }                
//                }
////                else /* Product is not Rapid Remit */
////                {
////                    plist += "AccountCurrency~" + ds.Tables[0].Rows[0].AccountCurrency + '|';
////                }
//	            
//	            if (plist != '')
//                {
//                    ac_plist = plist; /* save account details to a temp variable */
//                    plist = ''; /* free up plist for bene info */
//                }
//                
//                var telno    = '0';
//                var mobileno = '0';
//                
//                plist += "CIFType~1|";
//	            plist += "CIFClass~2|";
//	            plist += "IRSWorldSystem~1|";
//	            plist += "Remarks~Internet Beneficiary|";
//	            plist += "PreferredNo~True|";
//	            plist += "PreferredMailAdd~True|";
//	            if(ds.Tables[0].Rows[0].Lastname != null)   plist += "LastName~"    + ds.Tables[0].Rows[0].Lastname + '|';
//	            if(ds.Tables[0].Rows[0].Firstname != null)  plist += "FirstName~"   + ds.Tables[0].Rows[0].Firstname + '|';
//	            if(ds.Tables[0].Rows[0].Middlename != null) plist += "MiddleName~"  + ds.Tables[0].Rows[0].Middlename + '|';	    
//	            if(ds.Tables[0].Rows[0].BirthPlace != null) plist += "BirthPlace~"  + ds.Tables[0].Rows[0].BirthPlace + '|';
//	            if(ds.Tables[0].Rows[0].Birthdate != null)  plist += "BirthDate~"   + this.commons.to_mmddyyyy_date(ds.Tables[0].Rows[0].Birthdate) + '|';	    
//	            if(ds.Tables[0].Rows[0].Address1 != null)   plist += "Address1~"    + ds.Tables[0].Rows[0].Address1 + '|';
//	            if(ds.Tables[0].Rows[0].POBox != null)      plist += "POBox~"       + ds.Tables[0].Rows[0].POBox + '|';
//	            if(ds.Tables[0].Rows[0].Address2 != null)   plist += "Address2~"    + ds.Tables[0].Rows[0].Address2 + '|';
//	            if(ds.Tables[0].Rows[0].AddressType != null)plist += "AddressType~" + ds.Tables[0].Rows[0].AddressType + '|';
//	            if(ds.Tables[0].Rows[0].ZipCode != null)    plist += "ZipCode~"     + ds.Tables[0].Rows[0].ZipCode + '|';
//	            if(ds.Tables[0].Rows[0].CreatedBy != null)  plist += "CreatedBy~"   + ds.Tables[0].Rows[0].CreatedBy + '|';
//	            if(ds.Tables[0].Rows[0].TelNo != null)      telno    = ds.Tables[0].Rows[0].TelNo;
//	            if(ds.Tables[0].Rows[0].MobileNo != null)   mobileno = ds.Tables[0].Rows[0].MobileNo;
//	                                                        plist += "ContactNo~"   + mobileno + '^' + telno + '|';
//	                                                        plist += "BranchCode~"  + document.getElementById("txtHiddenUserLoc").value + '|';
//	            if(ds.Tables[0].Rows[0].Email != null)      plist += "EmailAddress~"+ ds.Tables[0].Rows[0].Email + '|';
//	            if(ds.Tables[0].Rows[0].CompanyName != null)plist += "Employer~"    + ds.Tables[0].Rows[0].CompanyName + '|';
//	            if(ds.Tables[0].Rows[0].Occupation != null) plist += "Occupation~"  + ds.Tables[0].Rows[0].Occupation + '|';
//	            
//	            if(ds.Tables[0].Rows[0].DetailedDesc != null)   plist += "DetailedDesc~"+ ds.Tables[0].Rows[0].DetailedDesc + '|';
//	            if(ds.Tables[0].Rows[0].WorkerType != null)     plist += "WorkerType~"  + ds.Tables[0].Rows[0].WorkerType + '|';
//	            if(ds.Tables[0].Rows[0].Salary != null)         plist += "Salary~"      + ds.Tables[0].Rows[0].Salary + '|';
//	            if(ds.Tables[0].Rows[0].SalaryCurrency != null) plist += "OccCurrency~" + ds.Tables[0].Rows[0].SalaryCurrency + '|';
//	            
//	            plist = plist.substring(0, plist.length-1);
//	            
//	            /* Save beneficiary information except in TBL_CIF_ACCOUNTS and TBL_CIF_BENEFICIARIES*/
//	            var ret_val = IRSWorld.Internet.InternetAddBeneficiary.enroll_agent(plist).value;
//	            
//	            if(ret_val.length > 0 && ret_val.length <= 16) /* Successful saving of beneficiary info */
//	            {
//	                enroll_stat = 1;
//	                ac_plist += "CIFID~" + ret_val; /* Appends CIFID to account details */
//	                
//                    if (String(ds.Tables[0].Rows[0].ProductCode) == "22")
//                    {
//                        ReqAcct = false;
//                        var gfc_details = this.enroll_bene_gfc(ret_val, ds.Tables[0].Rows[0].RemitterCIFID, ds.Tables[0].Rows[0].GFCCArdNo);                    
//                        
//                        this.ACCTSEQID = gfc_details.split("|")[0];
//                    }	                
//	                
//	                if(ReqAcct == false || this.add_account(ac_plist) == true ) /* Successful saving of account in TBL_CIF_ACCOUNTS */
//	                {
//	                    enroll_stat = 2;
//	                    /* For saving other bene info in TBL_CIF_BENEFICIARIES */
//	                    var bene_plist = '';
//	                    bene_plist += "CIFID~" + ret_val + '|';
//	                    bene_plist += "ProductCode~"   + ds.Tables[0].Rows[0].ProductCode + '|';
//	                    bene_plist += "ImplementationCode~" + Internet_Remitter_Products.set_implementation_type(String(ds.Tables[0].Rows[0].ProductCode)) + '|';
//	                    bene_plist += "RemitterCIFID~" + ds.Tables[0].Rows[0].RemitterCIFID + '|';
//	                    if(ds.Tables[0].Rows[0].Relationship != null)     bene_plist += "Relationship~"      + ds.Tables[0].Rows[0].Relationship + '|';
//	                    if(ds.Tables[0].Rows[0].NotificationMode != null) bene_plist += "NotificationMode~"  + ds.Tables[0].Rows[0].NotificationMode + '|';
//	                    
//                        if (ReqAcct == false && String(ds.Tables[0].Rows[0].ProductCode) != "22")
//                        {   
//                            bene_plist += "ProdCurrency~" + ds.Tables[0].Rows[0].AccountCurrency + '|';
//                        }

//                        if (this.ACCTSEQID != null && this.ACCTSEQID != '' && this.ACCTSEQID != undefined ) bene_plist += "AccountSeqId~" + this.ACCTSEQID + '|'; /* Appends accountseqid in bene details */

//	                    bene_plist = bene_plist.substring(0, bene_plist.length-1);
//	                    
//	                    var b_ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_bene(bene_plist).value;
//	                    
//	                    if(b_ret_val.length > 16) /* error in saving other bene info */
//	                    {
//	                        IRSWorld.Internet.InternetAddBeneficiary.clean_rejected(ret_val); /* clean up rejected bene */
//	                        alert(b_ret_val + '\n' + "Please try again."); 
//	                        this.disable_btns(false, false);
//	                        return;
//	                    }
//	                    else
//	                    {
//	                        enroll_stat = 3;
//	                    }
//	                }
//	                else /* error in saving account */ 
//	                {
//	                    IRSWorld.Internet.InternetAddBeneficiary.clean_rejected(ret_val); /* clean up rejected bene */
//	                    this.disable_btns(true, false);
//	                    return; 
//	                }
//	                
//	                if(enroll_stat == 3)
//	                {
//	                    IRSWorld.Internet.InternetAddBeneficiary.set_bene_status(String(ds.Tables[0].Rows[0].SEQ_ID), '1', '');
//	                    IRSWorld.Internet.InternetAddBeneficiary.set_bene_created_cifid(String(ds.Tables[0].Rows[0].SEQ_ID), ret_val);
//	                    alert("BENEFICIARY APPROVED!");
//	                    Internet_Beneficiary.load_grid_viewer_appbene();
//	                    this.disable_btns(true, true);
//	                }               
//	            }
//	            else /* error in enrolling beneficiary */
//	            {
//	                alert(ret_val + '\n' + "Please try again.");
//	                this.disable_btns(true, false);
//	            }
//	        }	        
//	    }
//	}		
	
    this.approve_bene = function(seq_id)
	{
	    Internet_Remitter_Main.check_session();
	    Internet_Beneficiary.showTransactionLoading("",true);
	    var isAutoApprove = false;
	    
	    if (seq_id != null && seq_id != undefined && seq_id != '' && seq_id != 'undefined')
	    {
	        isAutoApprove   =   true;
	    }
	    else
	    {
	        isAutoApprove   =   false;
	    }
	    
	    if (isAutoApprove == false)
	    {
	        if (!confirm('Approve beneficiary?'))
	        {
	            return;
	        }
	    }
	    
        var ds = null;
        
        if (isAutoApprove == true)
        {
            ds = IRSWorld.Internet.InternetAddBeneficiary.GET_INTERNET_REMITTER_BENE_RECORD(seq_id).value;
        }
        else
        {
            ds = IRSWorld.Internet.InternetAddBeneficiary.GET_INTERNET_REMITTER_BENE_RECORD(this.BENETOAPPSEQID).value;
        }
        
        var enroll_stat = 0; /* 1=bene enrolled, 2=account enrolled, 3=remitter and bene linked */
        
        var ReqAcct = IRSWorld.Internet.InternetAddBeneficiary.check_product_if_req_account(ds.Tables[0].Rows[0].ProductCode.toString()).value;
        
        this.generic = new generic_method();
        this.commons = new common_method();
             
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {   
            var plist = '';
            var ac_plist = '';
            
            if (String(ds.Tables[0].Rows[0].ProductCode) == "10") /* Product is Rapid Remit*/
            {
                if(this.IS_EXISTING == false) /* Checks if account number of beneficiary already exists */
                {
                    if(this.VALIDFC == true) /* Checks if account number of beneficiary is valid FC account*/
                    {
                        if(this.ACCT_DET != null) /* Account details from FC*/
                        {
                            var acctdet = (this.ACCT_DET).split("~");
                            if(acctdet.length > 0)
                            {
                                plist += "AccountCurrency~"  + acctdet[0] + '|';
                                plist += "AccountType~"      + acctdet[1] + '|';
                                plist += "AccountStatus~"    + acctdet[2] + '|';
                                plist += "AccountBranchCode~"+ acctdet[3] + '|';
                                /* If account number enrolled by remitter is a KBI Account, it will be replaced by an FC account else same value */
                                plist += "AccountNo~"        + acctdet[4] + '|'; 
                            }
                            if(acctdet.length == 6)
                            { 
                                plist += "KBIAccount~" + acctdet[5] + '|'; /* The original kbi account */
                            }                                                        
                        }   
                        else /* Unable to retrieve account details from FC */
                        {
                            plist += "AccountNo~" + ds.Tables[0].Rows[0].AccountNo + '|';
                            if(this.IS_KBI == true) /* If account number is KBI account, AccountNo and KBIAccount fields are the same. */
                            {
                                plist += "KBIAccount~" + ds.Tables[0].Rows[0].AccountNo + '|';
                            }
                        }
                    }
                    else /* Invalid FC account */
                    {
                        if (isAutoApprove == false)
                        {
                            alert("Account number is an invalid PNB account."); 
                            this.disable_btns(true, false);
                        }
                        else
                        {
                            alert("Enrollment request sent successfully. Kindly check status after 24 hours or call branch.");
                        }
                        return;
                    }
                }
                else /* Existing account */
                {
                    if (isAutoApprove == false)
                    {
                        alert("Account number already exists. Failed to approve beneficiary."); 
                        this.disable_btns(true, false);
                    }
                    else
                    {
                        alert("Enrollment request sent successfully. Kindly check status after 24 hours or call branch.");
                    }
                    return; 
                }                
            }
            else if(ReqAcct == true && (String(ds.Tables[0].Rows[0].ProductCode) == "12" || String(ds.Tables[0].Rows[0].ProductCode) == "13") ) /* Product is not Rapid Remit */
            {
                plist += "AccountCurrency~"     + ds.Tables[0].Rows[0].AccountCurrency + '|';
                plist += "AccountNo~"           + ds.Tables[0].Rows[0].AccountNo + '|';
                plist += "BranchType~"           + '5' + '|';
                if(ds.Tables[0].Rows[0].BankCode.toString().length == 3)
                {
                    plist += "DomBankCode~"       + ds.Tables[0].Rows[0].BankCode + '|'; 
                }
                else if (ds.Tables[0].Rows[0].BankCode.toString().substring(0,4) == '000-')
                {
                    plist += "DomBankCode~"       + '000' + '|';
                    plist += "branchname~"       + ds.Tables[0].Rows[0].BankCode.toString().substring(4, ds.Tables[0].Rows[0].BankCode.toString().length) + '|';
                }
                else
                {
                    plist += "DomBankCode~"       + ds.Tables[0].Rows[0].BankCode + '|';        
                }
            }
            
            if (plist != '')
            {
                ac_plist = plist; /* save account details to a temp variable */
                plist = ''; /* free up plist for bene info */
            }
            
            var telno    = '0';
            var mobileno = '0';
            
            plist += "CIFType~1|";
            plist += "CIFClass~2|";
            plist += "IRSWorldSystem~1|";
            plist += "Remarks~Internet Beneficiary|";
            plist += "PreferredNo~True|";
            plist += "PreferredMailAdd~True|";
            if(ds.Tables[0].Rows[0].Lastname != null)   plist += "LastName~"    + ds.Tables[0].Rows[0].Lastname + '|';
            if(ds.Tables[0].Rows[0].Firstname != null)  plist += "FirstName~"   + ds.Tables[0].Rows[0].Firstname + '|';
            if(ds.Tables[0].Rows[0].Middlename != null) plist += "MiddleName~"  + ds.Tables[0].Rows[0].Middlename + '|';	    
            if(ds.Tables[0].Rows[0].BirthPlace != null) plist += "BirthPlace~"  + ds.Tables[0].Rows[0].BirthPlace + '|';
            if(ds.Tables[0].Rows[0].Birthdate != null)  plist += "BirthDate~"   + this.commons.to_mmddyyyy_date(ds.Tables[0].Rows[0].Birthdate) + '|';	    
            
            //  For Home Address
            if(ds.Tables[0].Rows[0].Address1 != null)   plist += "Address1~"    + ds.Tables[0].Rows[0].Address1 + '|';
            if(ds.Tables[0].Rows[0].Address2 != null)   plist += "Address2~"    + ds.Tables[0].Rows[0].Address2 + '|';
            if(ds.Tables[0].Rows[0].POBox != null)      plist += "POBox~"       + ds.Tables[0].Rows[0].POBox + '|';            
            if(ds.Tables[0].Rows[0].AddressType != null)plist += "AddressType~" + ds.Tables[0].Rows[0].AddressType + '|';
            if(ds.Tables[0].Rows[0].ZipCode != null)    plist += "ZipCode~"     + ds.Tables[0].Rows[0].ZipCode + '|';
            
            //  For Company Address
            if(ds.Tables[0].Rows[0].Address1_Com != null)   plist += "Address1_Com~"    + ds.Tables[0].Rows[0].Address1_Com + '|';
            if(ds.Tables[0].Rows[0].Address2_Com != null)   plist += "Address2_Com~"    + ds.Tables[0].Rows[0].Address2_Com + '|';
            if(ds.Tables[0].Rows[0].POBox_Com != null)      plist += "POBox_Com~"       + ds.Tables[0].Rows[0].POBox_Com + '|';            
            if(ds.Tables[0].Rows[0].AddressType_Com != null)plist += "AddressType_Com~" + ds.Tables[0].Rows[0].AddressType_Com + '|';
            if(ds.Tables[0].Rows[0].ZipCode_Com != null)    plist += "ZipCode_Com~"     + ds.Tables[0].Rows[0].ZipCode_Com + '|';
            
            //  For Mailing Address
            if(ds.Tables[0].Rows[0].Address1_Mail != null)   plist += "Address1_Mail~"    + ds.Tables[0].Rows[0].Address1_Mail + '|';
            if(ds.Tables[0].Rows[0].Address2_Mail != null)   plist += "Address2_Mail~"    + ds.Tables[0].Rows[0].Address2_Mail + '|';
            if(ds.Tables[0].Rows[0].POBox_Mail != null)      plist += "POBox_Mail~"       + ds.Tables[0].Rows[0].POBox_Mail + '|';            
            if(ds.Tables[0].Rows[0].AddressType_Mail != null)plist += "AddressType_Mail~" + ds.Tables[0].Rows[0].AddressType_Mail + '|';
            if(ds.Tables[0].Rows[0].ZipCode_Mail != null)    plist += "ZipCode_Mail~"     + ds.Tables[0].Rows[0].ZipCode_Mail + '|';
            
            if(ds.Tables[0].Rows[0].CreatedBy != null)  plist += "CreatedBy~"   + ds.Tables[0].Rows[0].CreatedBy + '|';
            if(ds.Tables[0].Rows[0].TelNo != null)      telno    = ds.Tables[0].Rows[0].TelNo;
            if(ds.Tables[0].Rows[0].MobileNo != null)   mobileno = ds.Tables[0].Rows[0].MobileNo;
                                                        plist += "ContactNo~"   + mobileno + '^' + telno + '|';
                                                        plist += "BranchCode~"  + document.getElementById("txtHiddenUserLoc").value + '|';
            if(ds.Tables[0].Rows[0].Email != null)      plist += "EmailAddress~"+ ds.Tables[0].Rows[0].Email + '|';
            if(ds.Tables[0].Rows[0].CompanyName != null)plist += "Employer~"    + ds.Tables[0].Rows[0].CompanyName + '|';
            if(ds.Tables[0].Rows[0].Occupation != null) plist += "Occupation~"  + ds.Tables[0].Rows[0].Occupation + '|';
            
            if(ds.Tables[0].Rows[0].DetailedDesc != null)   plist += "DetailedDesc~"+ ds.Tables[0].Rows[0].DetailedDesc + '|';
            if(ds.Tables[0].Rows[0].WorkerType != null)     plist += "WorkerType~"  + ds.Tables[0].Rows[0].WorkerType + '|';
            if(ds.Tables[0].Rows[0].Salary != null)         plist += "Salary~"      + ds.Tables[0].Rows[0].Salary + '|';
            if(ds.Tables[0].Rows[0].SalaryCurrency != null) plist += "OccCurrency~" + ds.Tables[0].Rows[0].SalaryCurrency + '|';
            
            plist = plist.substring(0, plist.length-1);
            
            /* Save beneficiary information except in TBL_CIF_ACCOUNTS and TBL_CIF_BENEFICIARIES*/
            var ret_val = IRSWorld.Internet.InternetAddBeneficiary.enroll_agent(plist).value;
            
            if(ret_val.length > 0 && ret_val.length <= 16) /* Successful saving of beneficiary info */
            {
                enroll_stat = 1;
                ac_plist += "CIFID~" + ret_val; /* Appends CIFID to account details */

                if (String(ds.Tables[0].Rows[0].ProductCode) == "22")
                {
                    ReqAcct = false;
                    var gfc_details = this.enroll_bene_gfc(ret_val, ds.Tables[0].Rows[0].RemitterCIFID, ds.Tables[0].Rows[0].GFCCArdNo);                    
                    
                    this.ACCTSEQID = gfc_details.split('|')[0];
                }
                
                if(ReqAcct == false || this.add_account(ac_plist) == true ) /* Successful saving of account in TBL_CIF_ACCOUNTS */
                {
                    enroll_stat = 2;
                    /* For saving other bene info in TBL_CIF_BENEFICIARIES */
                    var bene_plist = '';
                    bene_plist += "CIFID~" + ret_val + '|';
                    bene_plist += "ProductCode~"   + ds.Tables[0].Rows[0].ProductCode + '|';
                    bene_plist += "ImplementationCode~" + Internet_Remitter_Products.set_implementation_type(String(ds.Tables[0].Rows[0].ProductCode)) + '|';
                    bene_plist += "RemitterCIFID~" + ds.Tables[0].Rows[0].RemitterCIFID + '|';
                    if(ds.Tables[0].Rows[0].Relationship != null)     bene_plist += "Relationship~"      + ds.Tables[0].Rows[0].Relationship + '|';
                    if(ds.Tables[0].Rows[0].NotificationMode != null) bene_plist += "NotificationMode~"  + ds.Tables[0].Rows[0].NotificationMode + '|';
            
	                if (ReqAcct == false && String(ds.Tables[0].Rows[0].ProductCode) != "22")
	                {   
	                    bene_plist += "ProdCurrency~" + ds.Tables[0].Rows[0].AccountCurrency + '|';
	                }

                    if (this.ACCTSEQID != null && this.ACCTSEQID != '' && this.ACCTSEQID != undefined ) bene_plist += "AccountSeqId~" + this.ACCTSEQID + '|'; /* Appends accountseqid in bene details */

            
                    bene_plist = bene_plist.substring(0, bene_plist.length-1);
                    
                    var b_ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_bene(bene_plist).value;
                    
                    if(b_ret_val.length > 16) /* error in saving other bene info */
                    {
                        IRSWorld.Internet.InternetAddBeneficiary.clean_rejected(ret_val); /* clean up rejected bene */
                        if (isAutoApprove == false)
                        {
                            alert(b_ret_val + '\n' + "Please try again.");  
                            this.disable_btns(true, false);
                        }
                        else
                        {
                            alert("Enrollment request sent successfully. Kindly check status after 24 hours or call branch.");
                        }
                        return;
                    }
                    else
                    {
                        enroll_stat = 3;
                    }
                }
                else /* error in saving account */ 
                {
                    IRSWorld.Internet.InternetAddBeneficiary.clean_rejected(ret_val); /* clean up rejected bene */
                    if (isAutoApprove == false)
                    {
                        this.disable_btns(true, false);
                    }
                    else
                    {
                        alert("Enrollment request sent successfully. Kindly check status after 24 hours or call branch.");
                    }
                    return; 
                }
                
                if(enroll_stat == 3)
                {
                    IRSWorld.Internet.InternetAddBeneficiary.set_bene_status(String(ds.Tables[0].Rows[0].SEQ_ID), '1', '');
                    IRSWorld.Internet.InternetAddBeneficiary.set_bene_created_cifid(String(ds.Tables[0].Rows[0].SEQ_ID), ret_val);
                    if (isAutoApprove == true)
                    {
                        alert("Beneficiary details succesfully enrolled, you may now send remittance");
                        Internet_Beneficiary.load_record();
                        
                    }
                    else
                    {
                        alert("BENEFICIARY APPROVED!");
                        Internet_Beneficiary.load_grid_viewer_appbene();
	                    this.disable_btns(true, true);
                    }
                }               
            }
            else /* error in enrolling beneficiary */
            {
                if (isAutoApprove != true)
                {
                    alert(ret_val + '\n' + "Please try again.");
                    this.disable_btns(true, false);
                }
                else
                {
                    alert("Enrollment request sent successfully. Kindly check status after 24 hours or call branch.");
                }
            }
        }
	}			
	
	this.enroll_bene_gfc = function(CIFID, REMCIFID, GFCNo)
	{
	    
	    var result = IRSWorld.TranClass.RemTranGFMCCommons.GFMCDetails(CIFID, "EXCARD", GFCNo);
	    
	    if (result.error != null)
	    {
	        //alert(result.error.Message);
	        alert('GFC number is invalid')
	        return false;
	    }
	    else
	    {
	        var ds = result.value;
            if (ds.Tables[1] != null && typeof(ds.Tables[1]) == "object" && ds.Tables[1].Rows.length > 0)
            {
                return ds.Tables[1].Rows[0].SEQ_ID.toString() + '|' + GFCNo + '|' + ds.Tables[1].Rows[0].Currency.toString() + '|' + 'excard';;
            }
            else if (ds.Tables[2] != null && typeof(ds.Tables[2]) == "object" && ds.Tables[2].Rows.length > 0)
            {   
                var accttype = ""; 
                var irsCurr = "";
                
                if (ds.Tables[3] != null && typeof(ds.Tables[3]) == "object" && ds.Tables[3].Rows.length > 0)
                {
                    accttype    =   ds.Tables[3].Rows[0].AcctType.toString();
                    irsCurr     =   ds.Tables[3].Rows[0].irsCurr.toString();
                }
                                            
                if (accttype == null || accttype == '' || accttype == undefined || accttype == 'null')
                {
                    accttype = '1';
                }
               
                // save data to server
                var res = IRSWorld.TranClass.RemTranGFMCCommons.SaveGFMCAccount(CIFID + "~" + REMCIFID, ds.Tables[2].Rows[0].rec_CardNo.toString(), ds.Tables[2].Rows[0].rec_CkAcctNo.toString(), irsCurr, "", 'excard', accttype);
                if (res.error != null)
                {                    
                    alert(res.error.Message);
                }
                else
                {
                    return res.value[4].toString() + '|' + ds.Tables[2].Rows[0].rec_CardNo.toString() + '|' + irsCurr + '|' + 'excard';                    
                }
            }
            else
            {
                alert("Card entered is either not valid or is not yet linked.");
            }
	    }
	    
	    return false;
	}
	
	this.color_asterisks = function()
	{
	    Internet_Remitter_Main.color_asterisk("tblBeneInfo1");
	    Internet_Remitter_Main.color_asterisk("tblBeneInfo2");
	}
	
	this.show_bene_comments = function()
	{
	    setText('comBeneMobileNo','* For Text Alert Service');
	    setText('comBeneBMobileNo','* For Text Alert Service');
	    document.getElementById('comBeneMobileNo').disabled = false;
	    document.getElementById('comBeneBMobileNo').disabled = false;	
	}
	
	this.disable_field = function(isDisabled)
	{
		this.commons = new common_method();     
        this.commons.to_read_only_2("tblBeneInfo1", isDisabled);
        this.commons.to_read_only_2("tblBeneInfo2", isDisabled);
        this.commons.to_read_only_2("tblBeneProduct", isDisabled);
        this.commons.to_read_only_2("tblBeneAccountNo", isDisabled);
        this.commons.to_read_only_2("tblGFCNo", isDisabled);
        this.commons.to_read_only_2("tblProduct", isDisabled);
        this.commons.to_read_only_2("tblOtherBankName", isDisabled);
        this.commons.to_read_only_2("tblOtherBankNameRural", isDisabled);
        //this.commons.to_read_only_2("tblIsRuralBank", isDisabled);
        this.commons = null;
        
        document.getElementById("tblIsRuralBank").disabled = isDisabled;
        
        document.getElementById("txtBeneZip").readOnly = true;
        document.getElementById("txtBeneBZip").readOnly = true;
        
        document.getElementById("txtBeneZip_Com").readOnly = true;
        document.getElementById("txtBeneBZip_Com").readOnly = true;
        
        document.getElementById("txtBeneZip_Mail").readOnly = true;
        document.getElementById("txtBeneBZip_Mail").readOnly = true;
        
        document.getElementById("txtBeneFCCustId1").readOnly = true;
        document.getElementById("txtBeneFCCustId2").readOnly = true;
        
        if (isDisabled)
        {
            if (    
                    document.getElementById('txtHiddenSelectedBeneCIFID').value != null && 
                    document.getElementById('txtHiddenSelectedBeneCIFID').value != undefined &&
                    document.getElementById('txtHiddenSelectedBeneCIFID').value != "" && 
                    Internet_Remitter_Main.check_cif_customer_id(document.getElementById("txtHiddenUserRole").value)
                )
            {
                document.getElementById("btnFCBeneCustID1").disabled    = false;
                document.getElementById("btnFCBeneCustID2").disabled    = false;      
            }
        }
        else
        {
                document.getElementById("btnFCBeneCustID1").disabled    = true;
                document.getElementById("btnFCBeneCustID2").disabled    = true;        
        }        
        
        document.getElementById('divBeneSaveCIFtoFCLoading').style.display = 'none';
        this.show_bene_comments();
	}
	
	this.clear_screen = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblBeneInfo1");
        this.commons.to_clear_screen("tblBeneInfo2");
        this.commons.to_clear_screen("tblBeneProduct");
        this.commons.to_clear_screen("tblBeneAccountNo");
        this.commons.to_clear_screen("tblProduct");
        this.commons.to_clear_screen("tblGFCNo");
        this.commons.to_clear_screen("tblOtherBankName");
        this.commons.to_clear_screen("tblOtherBankNameRural");
        this.commons.to_clear_screen("tblIsRuralBank");
        this.commons = null;
        
        document.getElementById("ddBTargetCurr").value = '';
        this.show_bene_comments();
    }
    
    this.get_rem_cifid = function()
    {
        var rcifid    = '';
        var rem_cifid = document.getElementById("txtHiddenRemCIFID").value;
        var src_cifid = document.getElementById("txtHiddenSearchCIFID").value;
        
        if (rem_cifid != '') rcifid = rem_cifid;
        else rcifid = src_cifid;
        
        return rcifid;
    }
    
    this.IsRuralBank_OnClick = function()
	{
	    var a = document.getElementById('chkIsRuralBankBene').checked;
	    
	    if (a == true)
	    {
	        document.getElementById('ddBOtherBankName').value = 'null';
	        document.getElementById('trOtherBankName').style.display = 'none';	        
	        document.getElementById('trOtherBankNameRural').style.display = 'block';
	    }
	    else
	    {
	        document.getElementById('txtBRuralBankName').value = '';
	        document.getElementById('trOtherBankNameRural').style.display = 'none';	        
	        document.getElementById('trOtherBankName').style.display = 'block';	    
	    }
	}
    
    this.check_notif = function(ciftype, notif)
    {
        if(ciftype == 1)
	    {
	        if(notif != 0 && notif != -1)
	        {
	            if(notif == 3 && document.getElementById("txtBeneMobileNo").value == '')
	            {
	                alert('Please enter mobile number.');
	            }
	            if(notif == 4 && document.getElementById("txtBeneTelNo").value == '')
	            {
	                alert('Please enter telephone number.');
	            }
	            if(notif == 5 && document.getElementById("txtBeneEmailAdd").value == '')
	            {
	                alert('Please enter email address.');
	            }
	        }
	    }
	    else
	    {
	        if(notif != 0 && notif != -1)
	        {
	            if(notif == 3 && document.getElementById("txtBeneBMobileNo").value == '')
	            {
	                alert('Please enter mobile number.');
	            }
	            if(notif == 4 && document.getElementById("txtBeneBTelNo").value == '')
	            {
	                alert('Please enter telephone number.');
	            }
	            if(notif == 5 && document.getElementById("txtBeneBEmailAdd").value == '')
	            {
	                alert('Please enter email address.');
	            }
	        }
	    }
    }
    
    this.onCheck_Preferred = function(ciftype, addType)
    {
        if (document.getElementById(addType).checked == true)
        {
            if (ciftype == 1)
            {
                document.getElementById("chkBenePrefMailAdd").value = "off";
                document.getElementById("chkBenePrefMailAdd").checked = false;
                
                document.getElementById("chkBenePrefMailAdd_Com").value = "off";
                document.getElementById("chkBenePrefMailAdd_Com").checked = false;
                
                document.getElementById("chkBenePrefMailAdd_Mail").value = "off";
                document.getElementById("chkBenePrefMailAdd_Mail").checked = false;
                
                if (this.hasAddress(addType))
                {
                    document.getElementById(addType).value   = "off";
                    document.getElementById(addType).checked = true;
                }
                else
                {
                    alert("You cannot select a blank address as preferred.");
                }
            }
            else
            {
                document.getElementById("chkBeneBPrefMailAdd").value = "off";
                document.getElementById("chkBeneBPrefMailAdd").checked = false;
                
                document.getElementById("chkBeneBPrefMailAdd_Com").value = "off";
                document.getElementById("chkBeneBPrefMailAdd_Com").checked = false;
                
                document.getElementById("chkBeneBPrefMailAdd_Mail").value = "off";
                document.getElementById("chkBeneBPrefMailAdd_Mail").checked = false;
                
                if (this.hasAddress(addType))
                {
                    document.getElementById(addType).value   = "off";
                    document.getElementById(addType).checked = true;
                }
                else
                {
                    alert("You cannot select a blank address as preferred.");
                }
            }  
        } 
    }
    
    this.hasAddress = function(addType)
    {
        var address = "";
        this.generic = new generic_method();
        
        if (addType == "chkBenePrefMailAdd")
        {   
            address += document.getElementById("txtBeneAddress2").value;
            address += document.getElementById("txtBeneAddress1").value;
            address += document.getElementById("txtBeneZip").value;
        }
        else if (addType == "chkBenePrefMailAdd_Com")
        {
            address += document.getElementById("txtBeneAddress2_Com").value;
            address += document.getElementById("txtBeneAddress1_Com").value;
            address += document.getElementById("txtBeneZip_Com").value;
        }
        else if (addType == "chkBenePrefMailAdd_Mail")
        {
            address += document.getElementById("txtBeneAddress2_Mail").value;
            address += document.getElementById("txtBeneAddress1_Mail").value;
            address += document.getElementById("txtBeneZip_Mail").value;
        }
        else if (addType == "chkBeneBPrefMailAdd")
        {
            address += document.getElementById("txtBeneBAddress1").value;
            address += document.getElementById("txtBeneBAddress2").value;
            address += document.getElementById("txtBeneBZip").value;
        }
        else if (addType == "chkBeneBPrefMailAdd_Com")
        {
            address += document.getElementById("txtBeneBAddress1_Com").value;
            address += document.getElementById("txtBeneBAddress2_Com").value;
            address += document.getElementById("txtBeneBZip_Com").value;
        }
        else if (addType == "chkBeneBPrefMailAdd_Mail")
        {
            address += document.getElementById("txtBeneBAddress1_Mail").value;
            address += document.getElementById("txtBeneBAddress2_Mail").value;
            address += document.getElementById("txtBeneBZip_Mail").value;
        }
        
        if (this.generic.to_trim(address) != "")
        {
            return true;
        }
        
        return false;
    }
    
    this.validate_input = function()
    {     
        this.generic     = new generic_method();
        this.error_input = '';
        var foc_elem     = '';
        ciftype_b        = document.getElementById("ddCIFType2").value;
        var notif        = -1;
        var role         = document.getElementById("txtHiddenUserRole").value;
        
        if(role != '10' && document.getElementById("ddCIFType2").value == 'null' && isAgentClient == false)
		{
			alert('Please select CIF Type of Beneficiary.');
            document.getElementById("ddCIFType2").focus();
            return false;
		}
		//if(role == '10' || isAgentClient == true)
		if(role == '10')
	    {
	        if(document.getElementById("ddBProductType").value == 'null' || document.getElementById("ddBProductType").value == '')
            {
                alert("Please select mode of remittance."); return false;
            }
            else
            {
                if(document.getElementById("ddBProductType").value == '10')
                {
                    if(this.generic.is_empty(trim(document.getElementById("txtBAccountNo").value)) == true)
	                {
	                    alert("Account number required."); return false;
	                }
	                else
	                {
	                    this.RR_ACCTNO = document.getElementById("txtBAccountNo").value;
	                    this.IS_EXISTING = false;
	                    
                        if(IRSWorld.Internet.InternetAddBeneficiary.is_existing_accountno("AccountNo~" + this.RR_ACCTNO).value == true)
                        {
                            this.IS_EXISTING = true;
                            alert("Account number already exists. Failed to approve beneficiary."); return false;
                        }
                        else
                        {
                            this.IS_EXISTING = false;
                            if(IRSWorld.Internet.InternetAddBeneficiary.validate_acctno_fcalgo(this.RR_ACCTNO).value == true)
                            {
                                this.VALIDFC = true;
                                var res = IRSWorld.Internet.InternetAddBeneficiary.get_acct_details(this.RR_ACCTNO);
                                
                                if (res.error != null)
                                {
                                    //alert(res.error.Message);
                                    alert('Account number is invalid');
                                    return false;
                                }
                                
                                if (res.value != null && res.value != 'null' && res.value != '')
                                {
                                    var ret_val = res.value.split("~");
                                    if (ret_val[2] == 1 || ret_val[2] == 5)
                                    {
                                        alert('Account is already closed.');
                                        return false;
                                    }
                                    //7,10,11,12,18,19
                                    else if(ret_val[2] == 7 || ret_val[2] == 10 || ret_val[2] == 11 || ret_val[2] == 12 || ret_val[2] == 18 || ret_val[2] == 19)
                                    {
                                        alert('Account is already dormant.');
                                        return false;
                                    }
                                }
                                
                                this.ACCT_DET = res.value;
                            }
                            else
                            {
                                this.VALIDFC = false;
                                alert("Account number is an invalid PNB account."); return false;
                            }
                        }
	                }
                }
                if(document.getElementById("ddBProductType").value != '10' & document.getElementById("ddBProductType").value != '22')
                {
                    if(document.getElementById("ddBTargetCurr").value == 'null' || document.getElementById("ddBTargetCurr").value == '')
	                {
	                    alert("Target curreny required."); return false;
	                }    
                }
                if(document.getElementById("ddBProductType").value == '22')
                {
                    if(this.generic.is_empty(trim(document.getElementById("txtBGFCNo").value)) == true)
	                {
	                    alert("GFC number required."); return false;
	                }
	                
                    var res = IRSWorld.TranClass.RemTranGFMCCommons.GFMCDetails_(document.getElementById('txtBGFCNo').value);
                    if (res.error != null)
                    {
                        alert('Error Validating GFC Number, Reason : ' + res.error.Message); return false;
                    }        
                    else
                    {
                        var ds = res.value;
                        if (ds != null && ds.Tables[0].Rows.length > 0 && this.generic.to_trim(ds.Tables[0].Rows[0].rec_CkAcctNo.toString()) != '')
                        {
                            document.getElementById('txtBAccountNo').value = ds.Tables[0].Rows[0].rec_CkAcctNo.toString();
                        }
                        else
                        {
                            alert('GFC Number is invalid.'); return false;
                        }
                    }
                }
                if(document.getElementById("ddBProductType").value == '12' || document.getElementById("ddBProductType").value == '13')
                {
                    if(this.generic.is_empty(trim(document.getElementById("txtBAccountNo").value)) == true)
	                {
	                    alert("Account number required."); return false;
	                }
	                if(document.getElementById("chkIsRuralBankBene").checked == true)
	                {
	                    if (document.getElementById("txtBRuralBankName").value == null || document.getElementById("txtBRuralBankName").value == '')
	                    {
	                        alert("Other Rural Bank Name is required."); return false;
	                    }
	                    if (document.getElementById("ddBTargetCurr").value == '2')
	                    {
	                        alert("Invalid Target Currency, Only Peso is allowed when Remitting to Rural Banks."); return false;
	                    }
	                }
                    else if (document.getElementById("ddBOtherBankName").value == 'null' || document.getElementById("ddBOtherBankName").value == '' || document.getElementById("ddBOtherBankName").value == null)
	                {
	                    alert("Other Bank Name is required."); return false;
	                }    
                }                
                
                ciftype_b = 1;
            }            
	    }
        if(ciftype_b == 1)
        {
	        notif = document.getElementById("ddBeneNotMode").selectedIndex;
	        if(notif != 0 && notif != -1)
	        {
	            if(notif == 3 && document.getElementById("txtBeneMobileNo").value == '')
	            {
	                alert('Please enter mobile number.'); return false;
	            }
	            if(notif == 4 && document.getElementById("txtBeneTelNo").value == '')
	            {
	                alert('Please enter telephone number.'); return false;
	            }
	            if(notif == 5 && document.getElementById("txtBeneEmailAdd").value == '')
	            {
	                alert('Please enter email address.'); return false;
	            }
	        }
		    if (this.generic.is_alpha(document.getElementById("txtBeneLName").value) == false)
            {           
                this.error_input += "Last name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBeneLName';            
            }
            if (this.generic.is_alpha(document.getElementById("txtBeneFName").value) == false)
            {           
                this.error_input += "First name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBeneFName';            
            }
            if (this.generic.is_alpha(document.getElementById("txtBeneMName").value) == false)
            {           
                this.error_input += "Middle name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBeneMName';            
            }
//            if (this.check_and_get_hitlist(document.getElementById("txtBeneFName").value, document.getElementById("txtBeneLName").value, document.getElementById("txtBeneMName").value) == false)
//            {
//                this.error_input += "Save cancelled due to Hitlist.\n";
//            }            
		    if (this.generic.to_validate_bdate(this.generic.to_trim(document.getElementById("txtBeneBDate").value)) == false)
            {           
                this.error_input += "Invalid birth date.\n";    
                if (foc_elem.length == 0) foc_elem = 'txtBeneBDate';       
            }
            else
            {
                var beneAge = this.generic.compute_age(document.getElementById("txtBeneBDate").value)
                var age_limit = IRSWorld.Internet.InternetAddBeneficiary.AgeLimit().value;
                if (parseFloat(beneAge) < age_limit)
                {
                    this.error_input += "Client who are below "+ age_limit +" yrs old are not allowed.\n";
                    if (foc_elem.length == 0) foc_elem = 'txtBeneBDate'; 
                }                
            }
            
            if(this.generic.to_validate_email(this.generic.to_trim(document.getElementById("txtBeneEmailAdd").value)) == false)
		    {
			    this.error_input += "Invalid email address.\n";
			    if (foc_elem.length == 0) foc_elem = 'txtBeneEmailAdd'; 
		    }
        }
        else
        {
            notif = document.getElementById("ddBeneBNotMode").selectedIndex;
	        if(notif != 0 && notif != -1)
	        {
	            if(notif == 3 && document.getElementById("txtBeneBMobileNo").value == '')
	            {
	                alert('Please enter mobile number.'); return false;
	            }
	            if(notif == 4 && document.getElementById("txtBeneBTelNo").value == '')
	            {
	                alert('Please enter telephone number.'); return false;
	            }
	            if(notif == 5 && document.getElementById("txtBeneBEmailAdd").value == '')
	            {
	                alert('Please enter email address.'); return false;
	            }
	        }
		    if (this.generic.is_alpha(document.getElementById("txtBeneBusinessName").value) == false)
            {           
                this.error_input += "Business name contains invalid character(s).\n"; 
                if (foc_elem.length == 0) foc_elem = 'txtBeneBusinessName';            
            }
//            if (this.check_and_get_hitlist(document.getElementById("txtBeneShortName").value, document.getElementById("txtBeneBusinessName").value, document.getElementById("txtBeneBBusType").value) == false)
//            {
//                this.error_input += "Save cancelled due to Hitlist.\n";
//            }             
		    if(this.generic.to_validate_email(this.generic.to_trim(document.getElementById("txtBeneBEmailAdd").value)) == false)
		    {
			    this.error_input += "Invalid email address.\n";
			    if (foc_elem.length == 0) foc_elem = 'txtBeneBEmailAdd'; 
		    }
        }
                		
        if (foc_elem.length > 0)
        {
            try
            {
                document.getElementById(foc_elem).focus();
                if(foc_elem.substring(0,3) == 'txt') document.getElementById(foc_elem).select();
            }
            catch(e){}
        }
        if (this.error_input != '')
        {
            alert(this.error_input);
            return false;
        }
        
        return true;
    }
//	this.check_and_get_hitlist = function(FName, LName,MName)
//	{
//	    var result = IRSWorld.Internet.InternetAddBeneficiary.check_and_get_hitlist(FName, LName,MName);
//	    var hitlist = result.value;
//	    
//	    if (hitlist != null && hitlist != '')
//	    {   
//	        var msg = "Beneficiary found in the Watched List\nPlease see Matched Name(s) below\n\n";
//            msg = msg + hitlist;
//	        msg = msg + "\nDo you want to proceed?";
//	        
//	        if (!confirm(msg))
//	        {
//	            return false;
//	        }    
//	    }	
//	    return true;
//	}    
    this.showTransactionLoading = function(message, ishide)
    {
        if (ishide == true)
        {
            //document.getElementById("lblLoadingMsg").innerHTML = "";
            //this.hidePanel("divPopProcess");
            ajax_show_status(false);
        }
        else
        {
            //new common_method().to_show_loading_message('divLoadingMsg');
            //document.getElementById("lblLoadingMsg").innerHTML = message;
            //this.showPanel("divPopProcess");
            ajax_show_status(true);
        }
    }
    
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        this.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {        
        divmask = new maskpanel();
        
        this.showControl(divName, false);
        divmask.hide();

    }
    this.showControl = function(element, isvisible)
    {
        if (isvisible == true)
        {
            document.getElementById(element).style.display = "";
        }
        else
        {
            document.getElementById(element).style.display = "none";
        }
    }
    
    this.divobj;
    this.waitifrm;

    var maskpanel = function(source)
    {

        this.show = function()
        {
            if(!document.getElementById("xdivmasking"))
            {
                var divEle = document.createElement("div");
                divEle.setAttribute("id", "xdivmasking");
                document.body.appendChild(divEle);

                var divSty = document.getElementById("xdivmasking").style;
                divSty.position = "absolute"; 
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "46";
                divSty.opacity = ".50";
                divSty.backgroundColor="#000";
                divSty.filter = "alpha(opacity=50)";

                var divFram = document.createElement("iframe");
                divFram.setAttribute("id", "xmaskframe");
                document.body.appendChild(divFram);
                divSty = document.getElementById("xmaskframe").style;
                divSty.position = "absolute";
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "45";
                divSty.border = "none";
                divSty.filter = "alpha(opacity=0)";
          }

          this.divobj = document.getElementById("xdivmasking");
          this.waitifrm = document.getElementById("xmaskframe");

          var dsh = document.documentElement.scrollHeight;
          var dch = document.documentElement.clientHeight;
          var dsw = document.documentElement.scrollWidth;
          var dcw = document.documentElement.clientWidth;
          var bdh = (dsh>dch) ? dsh:dch;
          var bdw = (dsw>dcw) ? dsw:dcw;

          this.waitifrm.style.height = this.divobj.style.height = bdh + "px";
          this.waitifrm.style.width = this.divobj.style.width = bdw + "px";
          this.waitifrm.style.display = this.divobj.style.display = "";
       }
       
       this.hide = function()
       {
          document.getElementById("xmaskframe").style.display = document.getElementById("xdivmasking").style.display = "none";
       }
    }
    
}
