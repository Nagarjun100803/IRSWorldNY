function cif_cards()
{
    this.commons          = null;
    this.generic          = null;
    this.SEQ_ID           = null;
    this.CIFID            = null;
    this.CardType         = null;
    this.CardNo           = null;
    this.CardName         = null;
    this.IssueDate        = null;
    this.ExpiryDate       = null;
    this.BankCode         = null;
    this.CardClass        = null;
    this.IssueNumber      = null;
    this.Remarks          = null;
    this.ScannedCard      = null;
    this.Country          = null;
    this.Currency         = null;
    this.GFMCAcctSeqId    = null;
    this.IsArm            = null;
    this.grid             = null;
    var iCards            = this;  
    
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblCards");
        this.commons = null;
        
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_cards~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFCARDSGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_cards~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFCARDSGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.cancel_insert = function()
    {
	    this.commons = new common_method();
	    this.commons.to_clear_screen("tblCards");
	    this.commons.to_read_only("tblCards", true);
	    this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {
        this.commons = new common_method();        
        this.commons.to_clear_screen("tblCards");        
        this.commons.to_read_only("tblCards", true);
        this.commons = null;
        
        this.pull_server_data();        
    }
     
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    {
        this.CardType     = document.getElementById("ddCardType").value;   
        this.CardNo       = document.getElementById("txtCardNo").value;
        this.CardName     = document.getElementById("txtCardName").value;
        this.IssueDate    = document.getElementById("txtIssueDate").value;
        this.ExpiryDate   = document.getElementById("txtExpiryDate").value;
        this.CardClass    = document.getElementById("ddCardClass").value;
        this.IssueNumber  = document.getElementById("txtIssueNo").value;
        this.Remarks      = document.getElementById("txtCardRemarks").value;
        this.ScannedCard  = document.getElementById("fileUploadCardImage").value;
        this.Country      = document.getElementById("ddCountry").value; 
        this.Currency     = document.getElementById("ddCardsCurrency").value; 
        
        //this.AccountSeqId = document.getElementById("ddGFMCAccountNo").value;
        
        this.set_bankcode();
        this.set_is_arm();
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
        
        IRSWorld.CIF.cifmain.GET_CARDS_RECORD(this.SEQ_ID,this.pull_server_data_callback);
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
                this.commons.to_clear_screen("tblCards");
                this.commons.to_read_only("tblCards", true);
                
                iCards.SEQ_ID       = ds.Tables[0].Rows[0].SEQ_ID;
                iCards.CardType     = ds.Tables[0].Rows[0].CardType;
                iCards.CardNo       = ds.Tables[0].Rows[0].CardNo;
                iCards.CardName     = ds.Tables[0].Rows[0].CardName;
                iCards.IssueDate    = ds.Tables[0].Rows[0].IssueDate;
                iCards.ExpiryDate   = ds.Tables[0].Rows[0].ExpiryDate;
                iCards.BankCode     = ds.Tables[0].Rows[0].BankCode;
                iCards.CardClass    = ds.Tables[0].Rows[0].CardClass;
                iCards.IssueNumber  = ds.Tables[0].Rows[0].IssueNumber;
                iCards.Remarks      = ds.Tables[0].Rows[0].Remarks;
                iCards.ScannedCard  = ds.Tables[0].Rows[0].ScannedCard;
                iCards.Country      = ds.Tables[0].Rows[0].Country;
                iCards.Currency     = ds.Tables[0].Rows[0].Currency;
                iCards.GFMCAcctSeqId= ds.Tables[0].Rows[0].GFMCAcctSeqId;
                iCards.IsArm        = ds.Tables[0].Rows[0].IsArm;
                
                document.getElementById("txtHiddenSeqId").value   = iCards.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value   = iCards.CIFID;   
                document.getElementById("txtCardNo").value        = iCards.CardNo; 
                document.getElementById("txtCardName").value      = iCards.CardName;
                document.getElementById("txtIssueNo").value       = iCards.IssueNumber; 
                document.getElementById("txtCardRemarks").value   = iCards.Remarks; 
                document.getElementById("lblCardImage").innerHTML = iCards.ScannedCard; 
                document.getElementById("fileUploadCardImage").value    = iCards.ScannedCard; 
                
                document.getElementById("txtIssueDate").value    = this.generic.to_mmyyyy_date(iCards.IssueDate); 
                document.getElementById("txtExpiryDate").value   = this.generic.to_mmyyyy_date(iCards.ExpiryDate);
                
                if(iCards.GFMCAcctSeqId != null && iCards.GFMCAcctSeqId != 0)
                {
                    this.cifcards = new cif_cards();
                    this.cifcards.get_bene_gfmcaccountno(iCards.GFMCAcctSeqId);
                    this.cifcards = null;
                }
                
                document.getElementById("chkIsARM").value   = "off";
                document.getElementById("chkIsARM").checked = false;
                
                if (document.getElementById("chkIsARM").value != null && iCards.IsArm == true)
                {
                    document.getElementById("chkIsARM").value = "on";
                    document.getElementById("chkIsARM").checked = true;
                }
                
                this.commons.to_set_dropdownlist_index("ddCardType",      iCards.CardType);
                this.commons.to_set_dropdownlist_index("ddCardClass",     iCards.CardClass);
                this.commons.to_set_dropdownlist_index("ddCountry",       iCards.Country);
                this.commons.to_set_dropdownlist_index("ddCardsCurrency", iCards.Currency);
                //this.commons.to_set_dropdownlist_index("ddGFMCAccountNo", this.AccountSeqId);
                
                this.generic.to_clear_null("txtCardNo~txtCardName~txtIssueDate~txtExpiryDate~txtIssueNo~lblCardImage~txtCardRemarks");
                
                this.commons = null;
                var bankname = '';
                var bcode = '';
                var ds_bankname = null
                ds_bankname = IRSWorld.CIF.cifmain.get_cards_bankname(document.getElementById("txtHiddenSeqId").value).value;
                
                if (ds_bankname.Tables[0].Rows.length > 0) 
                {
                    bankname = ds_bankname.Tables[0].Rows[0].BranchName;
                    bcode    = ds_bankname.Tables[0].Rows[0].BranchCode;
                }                    
                
                if (String(bankname) != 'undefined')
                {
                    document.getElementById("txtIssuingBank").value = bcode + ' - ' + bankname;
                }
            }
        }          
    }
        
    this.push_data_to_server = function()
    {
        var data_row = new Array();
       
        data_row[data_row.length]  = "CIFID~"        + this.CIFID; 
        data_row[data_row.length]  = "CardType~"     + this.CardType;
        data_row[data_row.length]  = "CardNo~"       + this.CardNo;
        data_row[data_row.length]  = "CardName~"     + this.CardName;
        data_row[data_row.length]  = "IssueDate~"    + this.IssueDate;
        data_row[data_row.length]  = "ExpiryDate~"   + this.ExpiryDate;
        data_row[data_row.length]  = "BankCode~"     + this.BankCode;
        data_row[data_row.length]  = "CardClass~"    + this.CardClass;
        data_row[data_row.length]  = "IssueNumber~"  + this.IssueNumber;
        data_row[data_row.length]  = "Remarks~"      + this.Remarks;
        data_row[data_row.length]  = "ScannedCard~"  + this.ScannedCard;
        data_row[data_row.length]  = "Country~"      + this.Country;
        data_row[data_row.length]  = "Currency~"     + this.Currency;
        data_row[data_row.length]  = "GFMCAcctSeqId~"+ this.GFMCAcctSeqId;
        data_row[data_row.length]  = "IsArm~"        + this.IsArm; 
        data_row[data_row.length]  = "Deleted~false";       
        
        return data_row.join("|");        
    }

    this.insert_record = function()
    {
        document.getElementById("txtHiddenSeqId").value = '';
        IRSWorld.CIF.cifmain.insert_record(this.insert_record_callback);
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
                var msg = IRSWorld.CIF.cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }
            }
                                    
            this.commons = new common_method();
            this.commons.to_clear_screen("tblCards");
            this.commons.to_read_only("tblCards", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;   
            
            iCards.disable_issuing_bank();
        }
        
    }
    
    this.disable_issuing_bank = function()
    {   
        document.getElementById("txtIssuingBank").readOnly = true;
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
                var msg = IRSWorld.CIF.cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }
            }
            
            this.commons = new common_method();
            this.commons.to_read_only("tblCards", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
            
            iCards.disable_issuing_bank();
        }        
    }
    
    this.delete_record = function()
    {
        IRSWorld.CIF.cifmain.delete_record(this.SEQ_ID, this.delete_record_callback);
        cif_main.set_deleted(); 
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
                alert(IRSWorld.CIF.cifmain.read_message()); return false;
            }
            return true;
        }     
    }     
    
    this.commit_record = function()
    {
         
         
          
         IRSWorld.CIF.cifmain.commit_record( this.set_record(), this.commit_record_callback );
         this.SEQ_ID = null;
         document.getElementById("txtHiddenBankSeqId").value = '';
         
        
    }
    
    this.commit_record_callback = function(result)
    {                        
	    iCards.load_record();
        
        if (typeof(iCards.load_grid_viewer) != 'undefined')
        {
            iCards.load_grid_viewer();
        }     
    
        if (result.error != null && result.error != "undefined")
        {            
            alert("UNABLE TO COMMIT RECORD. " + result.error); return;
        }
                                
        if (result != null && result.value != "undefined")
        {                                
            if (result.value == false)
            {     
                var msg = IRSWorld.CIF.cifmain.read_message();
                   
                if (msg != null && msg.value != "undefined")
                {
                    alert(msg.value); return;
                }                
            }
            
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");                                    
            this.commons = null;
            
            /*lock gfc card*/
            if(document.getElementById("ddCardType").value == 11) 
            {
                var cardno = document.getElementById("txtCardNo").value;
                var uid    = document.getElementById("txtHiddenUserID").value;
                var uloc   = document.getElementById("txtHiddenUserLocation").value;
                var dlockd = cif_main.get_branch_localtime(uloc);
                
                if(IRSWorld.CIF.cifmain.check_card_gfc(cardno).value == true)
                {   
    		        IRSWorld.CIF.cifmain.lock_gfc_card(cardno, uid, uloc, dlockd);
                }
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");
         }
    }
    
    
    this.rollback_record = function()
    {
        IRSWorld.CIF.cifmain.rollback_record(this.rollback_record_callback);
    }
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.CIF.cifmain.read_message();
        
        if (msg != null && msg.value != "undefined" && msg.value.length > 0)
        {
           alert(msg.value);
        }
        
        this.commons = new common_method();
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");                                    
        this.commons = null;
        return;        
    }
    
    this.validate_input = function()
    {        
        this.generic     = new generic_method();        
        this.error_input = '';
        var foc_elem     = '';
        
        if (this.generic.is_numeric(document.getElementById("txtCardNo").value) == false ||
            this.generic.is_positive(document.getElementById("txtCardNo").value) == false)
        {           
            this.error_input += "The card number is invalid.\n"; 
            if (foc_elem.length == 0) foc_elem = 'txtCardNo';     
        }
        if (this.generic.is_alphanumeric2(document.getElementById("txtCardName").value) == false)
        {           
            this.error_input += "The card name is invalid.\n";     
            if (foc_elem.length == 0) foc_elem = 'txtCardName';        
        }        
        if (this.generic.is_empty(document.getElementById("txtIssueDate").value) == false &&        
            this.generic.validate_mmyyyy(document.getElementById("txtIssueDate").value) == false)
        {           
            this.error_input += "Invalid/wrong date format of issue date.\n";     
            if (foc_elem.length == 0) foc_elem = 'txtIssueDate';        
        }
        if (this.generic.is_empty(document.getElementById("txtExpiryDate").value) == false &&
            this.generic.validate_mmyyyy(document.getElementById("txtExpiryDate").value) == false)
        {           
            this.error_input += "Invalid/wrong date format of expiry date.\n";     
            if (foc_elem.length == 0) foc_elem = 'txtExpiryDate';        
        }        
        if (this.generic.validate_mmyyyy(document.getElementById("txtIssueDate").value) == true &&
            this.generic.to_validate_issdate_mmyyyy(document.getElementById("txtIssueDate").value) == false)
        {           
            this.error_input += "Issue date is invalid.\n";
            if (foc_elem.length == 0) foc_elem = 'txtIssueDate';    
        }
        if (this.generic.validate_mmyyyy(document.getElementById("txtExpiryDate").value) == true &&
            this.generic.to_validate_expiration(document.getElementById("txtExpiryDate").value) == false)
        {           
            this.error_input += "Card is expired.\n";
            if (foc_elem.length == 0) foc_elem = 'txtExpiryDate';    
        }
        if (this.generic.is_numeric(document.getElementById("txtIssueNo").value) == false ||
            this.generic.is_positive(document.getElementById("txtIssueNo").value) == false)
        {           
            this.error_input += "The issuing number is invalid.\n";
            if (foc_elem.length == 0) foc_elem = 'txtIssueNo';           
        }
        if (foc_elem.length > 0)
        {
            document.getElementById(foc_elem).focus();
        }
        if (this.error_input != '')
        {
            return false;
        }
        
        return true;
    }
    
    this.get_bene_gfmcaccountno = function(gfmcacctseqid)
    {
        var ds = null;
        ds = IRSWorld.CIF.cifmain.get_bene_gfmcaccountno(gfmcacctseqid, document.getElementById("txtHiddenCIFID").value).value;
        
        if (ds != null && ds.Tables[0].Rows.length > 0)
        {
            var bene_gfmcaccountno    = ds.Tables[0].Rows[0].AccountNoDesc;
            var bene_gfmcaccountseqid = ds.Tables[0].Rows[0].AccountNoValue;
        }
        
        if (typeof(bene_gfmcaccountno) != 'undefined')
        document.getElementById("lblGFMCAccountNo").innerHTML = bene_gfmcaccountno;
    }
    
    this.set_is_arm = function()
    {
        this.IsArm = false;          
        if (document.getElementById("chkIsARM").value != null && document.getElementById("chkIsARM").checked == true)
        {
            this.IsArm = true;
        }
    }
    
    this.set_bankcode = function()
    {
        if (document.getElementById("txtHiddenBankSeqId").value != '')
        {
            var ds2 = null;
            ds2 = IRSWorld.CIF.cifmain.get_bank(document.getElementById("txtHiddenBankSeqId").value).value;
            
            if (ds2.Tables[0].Rows.length > 0) var card_branchcode = ds2.Tables[0].Rows[0].BranchCode;
        }
        
        if (document.getElementById("txtHiddenSeqId").value != '')
        {
            var ds = null;
            ds = IRSWorld.CIF.cifmain.get_cards_bankcode(document.getElementById("txtHiddenSeqId").value).value;
            
            if (ds != null && ds.Tables[0].Rows.length > 0) var card_bankcode = ds.Tables[0].Rows[0].BankCode;
            
            if (document.getElementById("txtHiddenBankSeqId").value == '')
            {
                this.BankCode = card_bankcode;
                return;
            }
            this.BankCode = card_branchcode;
        }
        else
        {
            if (String(card_branchcode) != 'undefined') 
            {
                this.BankCode = card_branchcode;
                return;
            }
            this.BankCode = '';
        }
    }
}
