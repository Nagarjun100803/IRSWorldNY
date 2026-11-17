function Internet_Remitter_Cards()
{
    this.SEQ_ID        = null;
    this.RemitterCIFID = null;
    this.CreatedBy     = null;
    this.CardNo        = null;
    this.CardName      = null;
    this.IssueDate     = null;
    this.ExpiryDate    = null;
    this.BankCode      = null;
    this.CardType      = null;
    this.CardClass     = null;
    this.Currency      = null;
    this.grid_ajax_query = null;
    this.grid_ajax_page  = "../AjxPage/ajaxgrid.aspx";
    this.update_flag     = false;
    this.grid            = null;
    this.CARDTOAPPSEQID  = null;
    this.insert_flag     = false;
    this.card_seqid      = null;
    this.othBankName     = null;
    this.IssueNumber     = null;
    var iCards           = this;
    
    /********* GRID FOR APPROVAL CARDS PER CLIENT **********/
    this.load_grid_viewer = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_cards~" + document.getElementById("txtHiddenUserCIFID").value;
        this.grid.div_container  = document.getElementById("INTCARDSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_cards~" + document.getElementById("txtHiddenUserCIFID").value;
        this.grid.div_container  = document.getElementById("INTCARDSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    /*******************************************************/
    
    
    this.load_grid_viewer_approve = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_cards~" + document.getElementById("txtHiddenUserCIFID").value;
        this.grid.div_container  = document.getElementById("APPINTCARDSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer_approve = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_cards~" + document.getElementById("txtHiddenUserCIFID").value;
        this.grid.div_container  = document.getElementById("APPINTCARDSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    
    /************* GRID FOR APPROVAL CARDS *****************/    
    this.load_grid_viewer_appcard = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_card_for_approval~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVCARDFORAPP");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_appcard = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_card_for_approval~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVCARDFORAPP");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    /*******************************************************/
    
    /*************** GRID FOR REJECTED CARDS ***************/
    this.load_grid_viewer_rejcard = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_internet_grid_card_rejected~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVCARDREJ");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_rejcard = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_internet_grid_card_rejected~" + document.getElementById("txtHiddenUserLoc").value;
        this.grid.div_container  = document.getElementById("DIVCARDREJ");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    /*******************************************************/
    
    /****************** GRID FOR CIF CARDS *****************/
    this.load_grid_viewer_cardcif = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_cards~" + this.get_rem_cifid();
        this.grid.div_container  = document.getElementById("DIVGRIDCARDCIF");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_cardcif = function(parameter)
    {
        this.grid = new grid_viewer();
        this.grid.sql_source     = "sp_cif_grid_cards~" + this.get_rem_cifid();
        this.grid.div_container  = document.getElementById("DIVGRIDCARDCIF");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    /*******************************************************/
    
    this.load_record = function()
	{
	    this.clear_screen();
	    this.disable_field(true);
	    
	    if(document.getElementById("txtHiddenUserRole").value == '10')
        {
            this.add_mode_card();
            this.pull_server_data();
        }
        else
        {
            this.edit_mode_card();
            this.pull_server_cards_info();
            if(document.getElementById("txtHiddenCardSeqId").value != '')
            {
                this.card_seqid = document.getElementById("txtHiddenCardSeqId").value;
            }            
        }
	}
	
	this.pull_server_cards_info = function()
	{   
        IRSWorld.Internet.InternetRemitter.get_cards_record(document.getElementById("txtHiddenCardSeqId").value,this.pull_server_cards_info_callback);
	}
	
	this.pull_server_cards_info_callback = function(result)
	{
	    if (result != null && result.value != null)
        {    
            var ds = result.value;
                        
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID      = ds.Tables[0].Rows[0].SEQ_ID;
                this.RemitterCIFID = ds.Tables[0].Rows[0].CIFID;
	            this.CardNo      = ds.Tables[0].Rows[0].CardNo;
	            this.CardName    = ds.Tables[0].Rows[0].CardName;
	            this.IssueDate   = ds.Tables[0].Rows[0].IssueDate;
	            this.ExpiryDate  = ds.Tables[0].Rows[0].ExpiryDate;
	            this.BankCode    = ds.Tables[0].Rows[0].BankCode;
	            this.CardType    = ds.Tables[0].Rows[0].CardType;
	            this.CardClass   = ds.Tables[0].Rows[0].CardClass;
	            this.Currency    = ds.Tables[0].Rows[0].Currency;
	            this.OthBankName = ds.Tables[0].Rows[0].OthBankName;
	            this.BankCode    = ds.Tables[0].Rows[0].BankCode;
	            this.IssueNumber = ds.Tables[0].Rows[0].IssueNumber;
	            
	            this.commons = new common_method();
	            this.generic = new generic_method();
	            
	            document.getElementById("txtHiddenCardSeqId").value     = this.SEQ_ID; 
                document.getElementById("txtCardNo").value              = "xxxxxxxx" + this.CardNo.substr(this.CardNo.length-4,4);  //this.CardNo;
                document.getElementById("txtCardName").value            = this.CardName;
                document.getElementById("txtExpiryDate").value          = this.generic.to_mmyyyy_date(this.ExpiryDate);
                document.getElementById("txtIssueDate").value           = this.generic.to_mmyyyy_date(this.IssueDate);
                document.getElementById("txtOthBankName1").value        = this.OthBankName;
                
                this.commons.to_set_dropdownlist_index("ddCardType",      this.CardType);
                this.commons.to_set_dropdownlist_index("ddCardClass",     this.CardClass);
                this.commons.to_set_dropdownlist_index("ddCardsCurrency", this.Currency);
                                
                document.getElementById("txtBankName").value            = ds.Tables[0].Rows[0].BankName;
                document.getElementById("txtHiddenCardBankCode").value  = this.BankCode;
                
                document.getElementById("txtIssueNumber").value         = this.IssueNumber;
                
                if (this.BankCode == 'OTH')
                {
                    iCards.show_issuing_bank_dataentry(true);
                }
                else
                {
                    iCards.show_issuing_bank_dataentry(false);
                }                
                
//                if(this.BankCode != null)
//                {   
//                    var ds = IRSWorld.Internet.InternetRemitter.get_bankname(this.BankCode).value;
//                    if (ds != null && ds.Tables[0].Rows.length > 0)
//                    {
//                        document.getElementById("txtBankName").value = ds.Tables[0].Rows[0].BranchName;
//                    }
//                }
	            
	            this.generic = new generic_method();
	            this.generic.to_clear_null("txtCardNo~txtCardName~txtExpiryDate~txtBankName~txtOthBankName1~txtIssueNumber");
            }
        }
	}
	
	this.pull_server_data = function()
	{   
        IRSWorld.Internet.InternetRemitter.GET_INTERNET_REMITTER_CARDS_RECORD(document.getElementById("txtHiddenCardSeqId").value,this.pull_server_data_callback);
	}
    
    this.pull_server_data_callback = function(result)
    {    
        if (result != null && result.value != null)
        {    
            var ds = result.value;
                        
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID      = ds.Tables[0].Rows[0].SEQ_ID;
                this.RemitterCIFID = ds.Tables[0].Rows[0].RemitterCIFID;
	            this.CardNo      = ds.Tables[0].Rows[0].CardNo;
	            this.CardName    = ds.Tables[0].Rows[0].CardName;
	            this.IssueDate   = ds.Tables[0].Rows[0].IssueDate;
	            this.ExpiryDate  = ds.Tables[0].Rows[0].ExpiryDate;
	            this.BankCode    = ds.Tables[0].Rows[0].BankCode;
	            this.CardType    = ds.Tables[0].Rows[0].CardType;
	            this.CardClass   = ds.Tables[0].Rows[0].CardClass;
	            this.Currency    = ds.Tables[0].Rows[0].Currency;
	            
	            this.commons = new common_method();
	            this.generic = new generic_method();
	            
	            document.getElementById("txtHiddenCardSeqId").value   = this.SEQ_ID; 
                document.getElementById("txtCardNo").value              = "xxxxxxxx" + this.CardNo.substr(this.CardNo.length-4,4);
                document.getElementById("txtCardName").value            = this.CardName;
                document.getElementById("txtExpiryDate").value          = this.generic.to_mmyyyy_date(this.ExpiryDate);
                document.getElementById("txtIssueDate").value           = this.generic.to_mmyyyy_date(this.IssueDate);
                this.commons.to_set_dropdownlist_index("ddCardType",      this.CardType);
                this.commons.to_set_dropdownlist_index("ddCardClass",     this.CardClass);
                this.commons.to_set_dropdownlist_index("ddCardsCurrency", this.Currency);
                
                if(this.BankCode != null)
                {
                    var ds = IRSWorld.Internet.InternetRemitter.get_bankname(this.BankCode).value;
                    if (ds != null && ds.Tables[0].Rows.length > 0)
                    {
                        document.getElementById("txtBankName").value = ds.Tables[0].Rows[0].BranchName;
                    }
                }
	            
	            this.generic = new generic_method();
	            this.generic.to_clear_null("txtCardNo~txtCardName~txtExpiryDate~txtBankName");
            }
        }
    }
    
    this.set_cards_record = function(isRequest)
    {
        this.RemitterCIFID = document.getElementById("txtHiddenUserCIFID").value;
        this.CreatedBy   = document.getElementById("txtHiddenUserID").value;
        this.CardNo      = document.getElementById("txtCardNo").value;
        this.CardName    = document.getElementById("txtCardName").value;
        this.ExpiryDate  = document.getElementById("txtExpiryDate").value;
        this.IssueDate   = document.getElementById("txtIssueDate").value;
		this.CardType    = document.getElementById("ddCardType").value;
        this.CardClass   = document.getElementById("ddCardClass").value;
        this.Currency    = document.getElementById("ddCardsCurrency").value;
        this.OthBankName = document.getElementById("txtOthBankName1").value;
        this.BankCode    = document.getElementById("txtHiddenCardBankCode").value;
		this.IssueNumber = document.getElementById("txtIssueNumber").value;		        
//		this.set_bankcode();
		
		if (isRequest == true) return this.push_data_to_server();
        else return this.set_plist();
    }
    
    this.set_plist = function()
    {
        var plist = '';
	    
	    if(this.card_seqid != null)	 plist += "SEQ_ID~"         + this.card_seqid + '|';
	    if(this.CardNo != '')	     plist += "CardNo~"         + this.CardNo + '|';
	    if(this.CardName != '')	     plist += "CardName~"       + this.CardName + '|';
	    if(this.BankCode != '')	     plist += "BankCode~"       + this.BankCode + '|';
	    if(this.CardType != '')	     plist += "CardType~"       + this.CardType + '|';
	    if(this.CardClass != '')	 plist += "CardClass~"      + this.CardClass + '|';
	    if(this.Currency != '')	     plist += "CardCurrency~"   + this.Currency + '|';
	    if(this.IssueDate != '')	 plist += "CardIssueDate~"  + this.IssueDate + '|';
	    if(this.ExpiryDate != '')	 plist += "CardExpiryDate~" + this.ExpiryDate + '|';
	    plist += "OthBankName~" +   this.OthBankName + '|';
	    plist += "IssueNumber~" +   this.IssueNumber + '|';
	    plist += "CIFID~" + this.get_rem_cifid() + '|';
	    plist  = plist.substring(0, plist.length-1);
        
        var ret_val = null;
        
        if(this.insert_flag == true)
        {
            this.card_seqid = null;
            ret_val = IRSWorld.Internet.InternetRemitter.add_card(plist).value; 
        }
        else
        {
            ret_val = IRSWorld.Internet.InternetRemitter.update_card(plist).value; 
        }
        
        var tostr_ret_val = String(Number(ret_val));
    				
	    if(tostr_ret_val == "NaN" || tostr_ret_val == "0")
	    {
		    alert(ret_val + '\n' + "Please try again.");
	    }
	    else
	    {
	        alert("RECORD COMMITTED SUCCESSFULLY!");	          
	        this.card_seqid = tostr_ret_val;
            this.disable_field(true);
            this.edit_mode_card();
            this.load_grid_viewer_cardcif();
	    }
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
    
    this.push_data_to_server = function()    
	{    
		var data_row = new Array();
			 
		data_row[data_row.length] = "RemitterCIFID~" + this.RemitterCIFID;
		data_row[data_row.length] = "CreatedBy~"     + this.CreatedBy;
		data_row[data_row.length] = "CardNo~"        + this.CardNo;
		data_row[data_row.length] = "CardName~"      + this.CardName;
		data_row[data_row.length] = "ExpiryDate~"    + this.ExpiryDate;
		data_row[data_row.length] = "BankCode~"      + this.BankCode;
		data_row[data_row.length] = "IssueDate~"     + this.IssueDate;
		data_row[data_row.length] = "CardType~"      + this.CardType;
		data_row[data_row.length] = "CardClass~"     + this.CardClass;
		data_row[data_row.length] = "Currency~"      + this.Currency;
		data_row[data_row.length] = "Status~0"; 
		/* default is 0 - for approval
		   in case, editing of card information will be allowed, delete this default value */
		
		return data_row.join("|");
	}
	
	this.insert_record = function()
	{   
	    IRSWorld.Internet.InternetRemitter.insert_record(this.insert_record_callback);
	    this.clear_screen();
	    this.disable_field(false);
	}  
	
	this.insert_record_callback = function(result)
	{       
		if (result != null && result.value != "undefined")
		{
			if (result.value == false)
			{
				var msg = IRSWorld.Internet.InternetRemitter.read_message(); alert(msg.value); return;
			}			 
		}
		
		this.commons = new common_method();
		//this.commons.to_unlock_button("tblButtonsAddCard","btnSaveCard~btnClearCard");  
	}
	
	this.commit_record = function()
	{  
	    IRSWorld.Internet.InternetRemitter.commit_record(this.set_cards_record(true),this.commit_record_callback);
	}
	
	this.commit_record_callback = function(result)
	{
		if (result != null && result.value != "undefined")
		{        
			if (result.value == false)
			{
				var msg = IRSWorld.Internet.InternetRemitter.read_message(); alert(msg.value); return;
			}
			
			this.SEQ_ID = IRSWorld.Internet.InternetRemitter.INTERNET_REMITTER_SEQ_ID().value;
			
			this.commons = new common_method();
            //this.commons.to_unlock_button("tblButtonsAddCard","btnUpdateCard");     
            this.commons = null;
            
			alert("REQUEST SENT SUCCESSFULLY!");       
		}
		
		this.commons = new common_method();
		//this.commons.to_unlock_button("tblButtonsAddCard","btnInsertCard~btnApproveCard");
		
		iCards.load_grid_viewer();
	}
	
	this.update_record = function()
    {
        this.update_flag = true;
        this.SEQ_ID = IRSWorld.Internet.InternetRemitter.INTERNET_REMITTER_SEQ_ID().value;
        IRSWorld.Internet.InternetRemitter.update_record(this.SEQ_ID, this.update_record_callback);
    }
    
    this.update_record_callback = function(result)
    {
        if (result.error != null && result.error != "undefined")
        {
            this.set_message("UNABLE TO UPDATE RECORD. " + result.error); return; 
        }
        
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {                
                var msg = cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }
            }
            
            this.commons = new common_method();
            this.commons.to_read_only("tblAddCards", false);
            //this.commons.to_unlock_button("tblButtonsAddCard","btnSaveCard~btnClearCard");
            this.commons = null;
        }        
    } 
	
	this.btn_add_card = function()
	{
	    Internet_Remitter_Main.check_session();
	    document.getElementById("txtHiddenCardSeqId").value = '';
	    this.insert_flag = true;
	    this.disable_field(false);
	    this.clear_screen();
	    this.save_mode_card();
	}
	
	this.btn_add_card_req = function()
	{
	    Internet_Remitter_Main.check_session();
	    this.insert_record();
	    this.disable_field(false);
	    this.clear_screen();
	    this.save_mode_card();
	}
	
	this.btn_cancel_card_req = function()
	{
	    this.clear_screen();
	    this.disable_field(true);
	    this.add_mode_card();
	}
	
	this.btn_edit_card = function()
	{
	    Internet_Remitter_Main.check_session();
	    
	    if(Internet_Remitter_Main.check_gfc_user(document.getElementById("txtHiddenRemCIFID").value))
        {
            this.insert_flag = false;
            this.disable_field(false);
            this.save_mode_card();
        }        
	}
	
	this.btn_save_card = function()
	{   
	    Internet_Remitter_Main.check_session();
	    if(Internet_Remitter_Main.check_mandatory_fields() == true)
	    {
	        if(this.validate_input() == true)
            {
                this.set_cards_record(false);
            }
	    }	    
	}
	
	this.btn_save = function()
	{   
	    Internet_Remitter_Main.check_session();
	    if(Internet_Remitter_Main.check_mandatory_fields() == true)
	    {
	        if(this.validate_input() == true)
            {
                if (confirm("Are you sure you want to send this request?"))
                {
                    this.commit_record();
                    this.disable_field(true);
                    this.add_mode_card();
                }
            }
	    }	    
	}
    
    this.btn_edit = function()
    {
        Internet_Remitter_Main.check_session();
        this.insert_flag = false;
        this.disable_field(false);
        this.save_mode_card();
    }
    
	this.set_bankcode = function()
	{
	    if((document.getElementById("txtHiddenBankSeqId").value).length > 0)
	    {
	        var ds1 = IRSWorld.Internet.InternetRemitter.get_bank(document.getElementById("txtHiddenBankSeqId").value).value;
		    if (ds1 != null && ds1.Tables[0].Rows.length > 0)
		    {
		        this.BankCode = ds1.Tables[0].Rows[0].BranchCode;
		        document.getElementById("txtHiddenCardBankCode").value = this.BankCode;
		    } 
	    }		
	}
	
	this.show_panel_app_card = function()
	{
	    document.getElementById("txtRejectCardReason").value = '';
	    this.clear_fields();
	    showPanel('divCard');
	    this.get_card_record();
	    this.get_other_details();
	    this.disable_btns(false, false);
	}
	
	this.get_other_details = function()
	{
	   var ds = IRSWorld.Internet.InternetRemitter.get_card_details(this.CARDTOAPPSEQID).value;
	   
	   if(ds != null && ds.Tables[0].Rows.length > 0)
	   {
	        document.getElementById('spnRemName').innerHTML     = ds.Tables[0].Rows[0].RemitterName
	        document.getElementById('spnCardType').innerHTML    = ds.Tables[0].Rows[0].CardType
            document.getElementById('spnCardClass').innerHTML   = ds.Tables[0].Rows[0].CardClass
            document.getElementById('spnCurrency').innerHTML    = ds.Tables[0].Rows[0].Currency
            document.getElementById('spnIssuingBank').innerHTML = ds.Tables[0].Rows[0].BankCode
	   } 
	   
	   this.generic = new generic_method();
       this.generic.to_clear_null("spnRemName~spnCardType~spnCardClass~spnCurrency~spnIssuingBank");
	}
	
	this.clear_fields = function()
    {   
        var fields = "spnCardNo~spnCardName~spnIssueDate~spnExpiryDate";
        var fld = fields.split('~');
        var i = 0;
        
        for(i=0; i<fld.length; i++)
        {
            document.getElementById(fld[i]).innerHTML = '';
        }
        
    }
	this.get_card_record = function()
	{
	    IRSWorld.Internet.InternetRemitter.GET_INTERNET_REMITTER_CARDS_RECORD(this.CARDTOAPPSEQID,this.get_card_rec_callback);	    	    
	}
	
	this.get_card_rec_callback = function(result)
	{
	    if (result != null && result.value != null)
        {    
            var ds = result.value;
            this.generic = new generic_method();    
                 
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID        = ds.Tables[0].Rows[0].SEQ_ID;
                this.RemitterCIFID = ds.Tables[0].Rows[0].RemitterCIFID;
	            this.CardNo        = ds.Tables[0].Rows[0].CardNo;
	            this.CardName      = ds.Tables[0].Rows[0].CardName;
	            this.IssueDate     = ds.Tables[0].Rows[0].IssueDate;
	            this.ExpiryDate    = ds.Tables[0].Rows[0].ExpiryDate;
	            
	            document.getElementById('spnCardNo').innerHTML      = this.CardNo;
                document.getElementById('spnCardName').innerHTML    = this.CardName;
                document.getElementById('spnIssueDate').innerHTML   = this.generic.to_mmyyyy_date(this.IssueDate);
                document.getElementById('spnExpiryDate').innerHTML  = this.generic.to_mmyyyy_date(this.ExpiryDate);
	        }
	        
	        this.generic = new generic_method();
            this.generic.to_clear_null("spnCardNo~spnCardName~spnIssueDate~spnExpiryDate");
	    }
	}
	
	this.disable_btns = function(isDisableApp, isDisableRej)
	{
	    document.getElementById("btnApproveCard").disabled = isDisableApp;
	    document.getElementById("btnRejectCard").disabled  = isDisableRej;
	}
	
	/* CARD STATUS
    0 or null - For Approval
    1 - Approved
    2 - Rejected */
    
    this.reject_card = function()
    {
        var rej_rsn = document.getElementById('txtRejectCardReason').value;
        if(rej_rsn == '')
        {
            alert('Please enter reason.');
        }
        else
        {
            if(confirm('Reject card?'))
            {
                IRSWorld.Internet.InternetRemitter.set_card_status(String(this.CARDTOAPPSEQID), '2', rej_rsn);
                alert('CARD REJECTED!');
                Internet_Remitter_Cards.load_grid_viewer_appcard();
                this.disable_btns(true, true);
            }
        }
    }
    
	this.approve_card = function()
	{
	    Internet_Remitter_Main.check_session();	    
	    if(confirm('Approve card?'))
	    {
	        IRSWorld.Internet.InternetRemitter.GET_INTERNET_REMITTER_CARDS_RECORD(this.CARDTOAPPSEQID,this.approve_card_callback);
	    }
	}
	
	this.approve_card_callback = function(result)
	{
	    if (result != null && result.value != null)
        {    
            var ds = result.value;
            this.generic = new generic_method();
            this.commons = new common_method();
                 
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {   
	            var plist = '';
	    
	            plist += "CardNo~"          + ds.Tables[0].Rows[0].CardNo + '|';
	            plist += "CardName~"        + ds.Tables[0].Rows[0].CardName + '|';
	            plist += "BankCode~"        + ds.Tables[0].Rows[0].BankCode + '|';	    
	            plist += "CardType~"        + ds.Tables[0].Rows[0].CardType + '|';
	            plist += "CardClass~"       + ds.Tables[0].Rows[0].CardClass + '|';
	            plist += "CardCurrency~"    + ds.Tables[0].Rows[0].Currency + '|';
	            plist += "CardIssueDate~"   + this.generic.to_mmyyyy_date(ds.Tables[0].Rows[0].IssueDate) + '|';	    
	            plist += "CardExpiryDate~"  + this.generic.to_mmyyyy_date(ds.Tables[0].Rows[0].ExpiryDate) + '|';
        	    plist += "CIFID~"           + ds.Tables[0].Rows[0].RemitterCIFID + '|';
        	    
	            plist = plist.substring(0, plist.length-1);
	            
	            var ret_val = IRSWorld.Internet.InternetRemitter.add_card(plist).value; 
	            
	            var tostr_ret_val = String(Number(ret_val));
    				
			    if(tostr_ret_val == "NaN" || tostr_ret_val == "0")
			    {
				    alert(ret_val + '\n' + "Please try again.");
	                Internet_Remitter_Cards.disable_btns(false, false);
			    }
			    else
			    {
			        alert("CARD APPROVED!");	           
	                IRSWorld.Internet.InternetRemitter.set_card_status(String(ds.Tables[0].Rows[0].SEQ_ID), '1', '');
	                Internet_Remitter_Cards.disable_btns(true, true);
	                Internet_Remitter_Cards.load_grid_viewer_appcard();
			    }
	        }
	    }
	}
	
	this.clear_screen = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblAddCards");
        this.commons = null;
    }
	
	this.disable_field = function(isDisabled)
	{
		this.commons = new common_method();     
        this.commons.to_read_only("tblAddCards", isDisabled);
        this.commons = null;
        
        document.getElementById("txtBankName").readOnly = true;
	}	
	
	this.save_mode_card = function()
    {
        document.getElementById("btnAddCard2").disabled    = true;
        document.getElementById("btnSaveCard2").disabled   = false;
        document.getElementById("btnCancelCard2").disabled = false;
        
        document.getElementById("btnAddCard1").disabled    = true;
        document.getElementById("btnEditCard1").disabled   = true;
        document.getElementById("btnSaveCard1").disabled   = false;
        document.getElementById("btnCancelCard1").disabled = false;
    }
      
    this.add_mode_card = function()
    {
        document.getElementById("btnAddCard2").disabled    = false;
        document.getElementById("btnSaveCard2").disabled   = true;
        document.getElementById("btnCancelCard2").disabled = true;
        
        document.getElementById("btnAddCard1").disabled    = false;
        document.getElementById("btnEditCard1").disabled   = true;
        document.getElementById("btnSaveCard1").disabled   = true;
        document.getElementById("btnCancelCard1").disabled = true;
    }
    
    this.edit_mode_card = function()
    {
        document.getElementById("btnAddCard1").disabled    = false;
        document.getElementById("btnEditCard1").disabled   = false;
        document.getElementById("btnSaveCard1").disabled   = true;
        document.getElementById("btnCancelCard1").disabled = true;
    }
    
    this.show_issuing_bank_dataentry = function(enable)
    {
        if (enable == false)
        {
            document.getElementById('trOthBankName').style.display = 'none';
            //document.getElementById('trOthBankNameComment').style.display = 'none';
        }
        else
        {
            document.getElementById('trOthBankName').style.display = '';
            //document.getElementById('trOthBankNameComment').style.display = 'block';
        }
    }
    
	this.validate_input = function()
	{
	    this.generic     = new generic_method();        
        this.error_input = '';
        var foc_elem     = '';
        var cardno       = document.getElementById("txtCardNo").value;
        var cardname     = document.getElementById("txtCardName").value;
        var issdate      = document.getElementById("txtIssueDate").value;
        var expdate      = document.getElementById("txtExpiryDate").value;
        var othBankName  = document.getElementById("txtOthBankName1").value; 
        //this.set_bankcode();
        var bankcode     = document.getElementById("txtHiddenCardBankCode").value;
        var issuenumber  = document.getElementById("txtIssueNumber").value;
        var cards_cnt    = 0;
        var is_valid_exp = true;
        
        cards_cnt = IRSWorld.Internet.InternetRemitter.get_cards_count(cardno, cardname, bankcode, document.getElementById("txtHiddenUserCIFID").value); 
        
        if(cards_cnt.value > 0)
        {
            alert('Record already exists!'); return false;
        }
        
        //validate gfc login card
        
//        if (document.getElementById("ddCardType").value == '11')
//        {
//            var card_isGFC = IRSWorld.Internet.InternetRemitter.check_card_if_exists_in_gfc(cardno).value;
//            
//            if (card_isGFC == true)
//            {
//                alert('GFC ARM Card Number already exists in GFC');return false;
//            }
//        }
        
        /*if(this.generic.is_empty(cardno) == true)
		{
			this.error_input += "Card number required.\n";  
			foc_elem = 'txtCardNo';
		}
		if(document.getElementById('ddCardType').value == 'null')
        {
            this.error_input += "Card type required.\n";
            if (foc_elem.length == 0) foc_elem = 'ddCardType'; 
        }
		if(this.generic.is_empty(cardname) == true)
		{
			this.error_input += "Card name required.\n";  
			foc_elem = 'txtCardName';
		}
		if(document.getElementById('ddCardClass').value == 'null')
        {
            this.error_input += "Card class required.\n";
            if (foc_elem.length == 0) foc_elem = 'ddCardClass'; 
        }
		if(this.generic.is_empty(issdate) == true)
		{
			this.error_input += "Issue date required.\n";  
			foc_elem = 'txtIssueDate';
		}
		if(this.generic.is_empty(expdate) == true)
		{
			this.error_input += "Expiry date required.\n";  
			foc_elem = 'txtExpiryDate';
		}
		if(this.generic.is_empty(bankcode) == true)
		{
			this.error_input += "Bank code required.\n";  
			foc_elem = 'txtCardNo';
		}*/
        if(this.generic.is_alphanumeric2(cardname) == false)
		{
		    this.error_input += "Card name contains invalid character(s).\n";  
			if (foc_elem.length == 0) foc_elem = 'txtCardName'; 
		}
		if (this.generic.is_empty(issdate) == false && (this.generic.validate_mmyyyy(issdate) == false ||
		    this.generic.to_validate_issdate_mmyyyy(issdate) == false))
        {           
            this.error_input += "Issue date is invalid.\n";     
            if (foc_elem.length == 0) foc_elem = 'txtIssueDate';        
        }
		if (this.generic.is_empty(expdate) == false && this.generic.validate_mmyyyy(expdate) == false)
        {           
            is_valid_exp = false;
            this.error_input += "Expiry date is invalid.\n";     
            if (foc_elem.length == 0) foc_elem = 'txtExpiryDate';        
        }
        if (this.generic.is_empty(expdate) == false && this.generic.to_validate_expiration(expdate) == false
            && is_valid_exp == true)
        {           
            this.error_input += "Card is expired.\n";
            if (foc_elem.length == 0) foc_elem = 'txtExpiryDate';    
        }
        if (bankcode == 'OTH')
        {
            if (this.generic.is_empty(othBankName) == true)
            {
                this.error_input += "Issuing Bank Name is required when the selected bank is OTHERS.\n";
                if (foc_elem.length == 0) foc_elem = 'txtOthBankName1';
            }
        }        
        if (this.generic.is_numeric(issuenumber) != true)
        {
            this.error_input += "Issue Number is invalid.\n";
            if (foc_elem.length == 0) foc_elem = 'txtIssueNumber';   
        }
		if (foc_elem.length > 0)
        {
            //document.getElementById(foc_elem).focus();
            if(foc_elem.substring(0,3) == 'txt') document.getElementById(foc_elem).select();
        }
        if (this.error_input != '')
        {
            alert(this.error_input);
            return false;
        }
        
        return true;
	}
}
