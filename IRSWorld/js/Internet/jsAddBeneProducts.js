
function Internet_Remitter_Products()
{
    this.SEQ_ID         = null;
    this.RemitterCIFID  = null;
    this.BeneCIFID      = null;
    this.ProductType    = null;
    this.ImplemType     = null;    
    this.TargetCurrency = null;
    this.AccountNo      = null;
    this.OtherBankName  = null;
    this.BranchCode     = null;
    this.AccountType    = null;
    this.AccountCurrency= null;
    this.GFMCNo         = null;
    this.GFMCCurrCode   = null;
    this.AccountSeqId   = null;
    this.OtherBankSeqId = null;
    this.BankName       = null;
    this.BankAddress    = null;
    this.sq             = null;
    this.REMCIFID       = null;    
    this.show_gfc       = true;
    this.VALIDFC        = false;
    this.insert_flag    = false;
    this.ACCTSEQID      = null;
    this.ACCTSTAT       = null;
    this.KBIAccount     = null;
    this.BenSeqId   = null;    
    this.ReqAcct = null;
    this.BranchName =   null;
    
    this.load_grid_viewer = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        
        var remcifid = this.get_rem_cifid();
        var bencifid = document.getElementById("txtHiddenSelectedBeneCIFID").value;
        var role =  document.getElementById('txtHiddenUserRole').value.toString();
        if(view_bene == false)
        {
            this.grid.sql_source = "sp_internet_grid_cifbene_products~" + remcifid + '~' + '-1' + '~' + role;
        }
        else
        {
            this.grid.sql_source = "sp_internet_grid_cifbene_products~" + remcifid + '~' + bencifid + '~' + role;
        }
        this.grid.div_container  = document.getElementById("DIVPRODUCTSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
   
    this.gfc_callback= function(response)
    {
        var a           = response;
        var acctseqid   = '';
        var ckno        = '';
        var gfcno       = '';
        var currency    = '';
        var mode        = '';
        
        if(a.length > 0)
        {
            ckno     = a[0].toString();
            gfcno    = a[1].toString();
            currency = a[2].toString();
            mode     = a[3].toString(); 
            
            if (document.getElementById("txtHiddenUserRole").value == '10')
            {
                // a[0] value is already the account seq_id
                acctseqid = ckno;
            }
            else
            {
                var rs = IRSWorld.Internet.InternetAddBeneficiary.GET_CIF_ACCOUNTSEQID(ckno);
                if (rs.error != null)
                {
                    alert(rs.error.Message);
                }
                acctseqid = rs.value;
            }
            
            
            if (acctseqid != '' && acctseqid != null)
            {
                this.AccountSeqId = acctseqid;
            }
            
            var accttype = IRSWorld.Internet.InternetAddBeneficiary.get_account_type(acctseqid).value;
            var acctstat = IRSWorld.Internet.InternetAddBeneficiary.get_account_status(acctseqid).value;
            var actbcode = IRSWorld.Internet.InternetAddBeneficiary.get_account_bcode(acctseqid).value;
            
            /* save in tbl_cif_beneficiaries*/
            /* set account no and dropdown fields */
            if (document.getElementById("txtHiddenUserRole").value == '10')
            {
                document.getElementById("txtAccountNo").value = gfcno;
            }
            else
            {
                document.getElementById("txtAccountNo").value = ckno;
            }
            
            
            this.commons = new common_method();
            this.commons.to_set_dropdownlist_index("ddTargetCurrency", currency);
            this.commons.to_set_dropdownlist_index("ddAccountType",     accttype);
            this.commons.to_set_dropdownlist_index("ddBranchCode",      actbcode);   
            
            this.ACCTSTAT = acctstat;
            if(acctstat == 1 || acctstat == 5)
            {
                alert("Can not enroll closed accounts!");
                return;
            }
            
            if (document.getElementById("txtHiddenUserRole").value == '10')
            {
                this.btn_save_prod_1();
            }
            else
            {
                this.btn_save_prod();
            }
            
            this.disable_field(true);     
            this.add_mode_prod();    
        }
    }  
   
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        var remcifid = this.get_rem_cifid();
        this.grid.sql_source = "sp_internet_grid_cifbene_products~" + remcifid;
        
        this.grid.div_container  = document.getElementById("DIVPRODUCTSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.load_record = function()
	{
	    this.clear_screen();
	    this.clear_variables();
	    this.clear_prod_fields();
	    this.hide_all_div();
	    this.disable_field(true);
	    this.pull_server_data();
	}
	
	this.pull_server_data = function()
	{
	    IRSWorld.Internet.InternetAddBeneficiary.get_bene_prod(document.getElementById("txtHiddenBeneProdSeqId").value, document.getElementById("txtHiddenUserRole").value,  this.pull_server_data_callback);
	}
	
	this.pull_server_data_callback = function(result)
	{
	    Internet_Remitter_Products.add_mode_prod();
	    
	    if (result != null && result.value != null)
        {
            this.commons = new common_method();
            this.generic = new generic_method();
            
            var ds = result.value;
            var pd = null;
            
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                pd = ds.Tables[0].Rows[0].ProductCode;
                //Changing product code to (13) non-pddts / epcs for Net Client and Agent when product is 'PDDTS (12)'
                //pd = (pd == '12' & (document.getElementById("txtHiddenUserRole").value == '10' || isAgentClient == true)) ? '13' : pd;
                pd = (pd == '12' & (document.getElementById("txtHiddenUserRole").value == '10')) ? '13' : pd;
                
                Internet_Remitter_Products.load_dd_filtered_currency(pd);   
                         
                document.getElementById("ddBeneficiaryName").value = ds.Tables[0].Rows[0].CIFID;
                document.getElementById("ddProductType").value     = pd;
                document.getElementById("txtAccountNo").value      = ds.Tables[0].Rows[0].AccountNo; 
                document.getElementById("ddAccountType").value     = ds.Tables[0].Rows[0].AccountType;  
                document.getElementById("ddOtherBankName").value   = ds.Tables[0].Rows[0].DomBankCode; 
                document.getElementById("ddBranchCode").value      = ds.Tables[0].Rows[0].BranchCode;
                document.getElementById("ddAccountStatus").value      = ds.Tables[0].Rows[0].Status; 
                this.OtherBankSeqId                                = ds.Tables[0].Rows[0].OtherBankSeqId;
                document.getElementById('txtHiddenMT103BankSeqId').value = ds.Tables[0].Rows[0].OtherBankSeqId;
                document.getElementById("chkIsRuralBank").checked   =   false;
                
                if(pd == '256' || pd == '257')
                {
                    Internet_Remitter_Products.get_other_bankdetails(this.OtherBankSeqId);
                }
                
                document.getElementById("ddTargetCurrency").value  = ds.Tables[0].Rows[0].Currency; 
                /*if(pd == 8 || pd == 9 || pd == 7 || pd == 27|| pd == 256 || pd == 257)
                {
                    document.getElementById("ddTargetCurrency").value  = ds.Tables[0].Rows[0].Currency; 
                }
                else
                {
                    document.getElementById("ddAccountCurrency").value = ds.Tables[0].Rows[0].Currency; 
                }*/
                
                Internet_Remitter_Products.show_gfc = false;
                Internet_Remitter_Products.show_fields(); 
                
                if(pd == '13' && ds.Tables[0].Rows[0].DomBankCode == '000')
                {
                    document.getElementById("chkIsRuralBank").checked = true;
                    Internet_Remitter_Products.IsRuralBank_OnClick();
                    document.getElementById("txtOthBankName").value = ds.Tables[0].Rows[0].branchname;                    
                }
                if(pd == '12')
                {
                    document.getElementById("ddOtherBankNamePDDTS").value   = ds.Tables[0].Rows[0].DomBankCode;                   
                }                                
                
                Internet_Remitter_Products.edit_mode_prod();
                
                this.generic.to_clear_null("txtAccountNo");
            }
        }
	}
	
	this.get_other_bankdetails = function(othbankseqid)
    {
        if (othbankseqid != '' && othbankseqid != null)
        {
            var ds = null;
            ds = IRSWorld.Internet.InternetAddBeneficiary.get_otherbanks_details(String(othbankseqid)).value;
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {
                var bank_name    = ds.Tables[0].Rows[0].BankName;
                var bank_address = ds.Tables[0].Rows[0].BankAddress;
                
                document.getElementById("txtMT103BankName").value = bank_name + ' - ' + bank_address;
            }
        }
    }
    
    this.clear_screen = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblProductInfo");
        this.commons.to_clear_screen("tblSearchOtherBank1");
        this.commons = null;
    }
	
	this.disable_field = function(isDisabled)
	{
		this.commons = new common_method();     
        this.commons.to_read_only("tblProductInfo", isDisabled);        
        this.commons.to_read_only("tblAccountNo",   isDisabled);
        this.commons.to_read_only("tblTarCurr",     isDisabled);
        this.commons.to_read_only("tblOthBankName", isDisabled);
        this.commons.to_read_only("tblOthBankNamePDDTS", isDisabled);
        this.commons.to_read_only("tblBranchCode",  isDisabled);
        this.commons.to_read_only("tblAccountType", isDisabled);
        this.commons.to_read_only("tblAccountCurr", isDisabled);
        this.commons.to_read_only("tblAccountStatus", isDisabled);
        this.commons.to_read_only("tblSearchOtherBank1", isDisabled);
        this.commons.to_read_only("tblOtherBank1",  isDisabled);
        this.commons.to_read_only("tblOtherBank2",  isDisabled);
        document.getElementById("txtMT103BankName").readOnly = true;
        this.commons = null;
        
        document.getElementById("chkIsRuralBank").disabled = isDisabled;
	}
	
	this.save_mode_prod = function()
    {
        document.getElementById("btnAddProd1").disabled    = true;
        document.getElementById("btnEditProd1").disabled   = true;
        document.getElementById("btnSaveProd1").disabled   = false;
        document.getElementById("btnCancelProd1").disabled = false;
    }
      
    this.add_mode_prod = function()
    {
        document.getElementById("btnAddProd1").disabled    = false;
        document.getElementById("btnEditProd1").disabled   = true;
        document.getElementById("btnSaveProd1").disabled   = true;
        document.getElementById("btnCancelProd1").disabled = true;
    }
    
    this.edit_mode_prod = function()
    {
        document.getElementById("btnAddProd1").disabled    = false;
        document.getElementById("btnEditProd1").disabled   = ((document.getElementById("txtHiddenUserRole").value == '10') ? true : false);
        document.getElementById("btnSaveProd1").disabled   = true;
        document.getElementById("btnCancelProd1").disabled = true;
        
        if(document.getElementById("ddProductType").value == '22')
        {
            document.getElementById("btnEditProd1").disabled   = true;
        }
    }
    
    this.clear_prod_fields = function()
    {
        var fields = "txtAccountNo~ddTargetCurrency~ddOtherBankName~ddBranchCode~ddAccountType~ddAccountCurrency~txtOthBankName~txtOthBankAddress~ddAccountStatus~ddOtherBankNamePDDTS";
        
        var elems = fields.split("~");
        var y = 0;
        
        for(y = 0; y < elems.length; y++)
        {
            if (elems[y] != null || elems[y] != "")
            { 
                document.getElementById(elems[y]).value = '';
            } 
        }
        document.getElementById('chkIsRuralBank').checked = false;
    }
    
    this.filter_branches = function()
    {
        this.load_dd_filtered_branches(document.getElementById("ddProductType").value);
    }
    
    this.load_dd_filtered_branches = function(prod_type)
    {
        IRSWorld.Internet.InternetAddBeneficiary.getdropdown_source_filteredbranches(prod_type, this.dd_filtered_branches_callback);
    }
	
	this.dd_filtered_branches_callback = function(result)
	{
	    if(result != null && result != "undefined")
	    {
	        this.commons = new common_method();         
			this.commons.to_load_dropdownlist("ddBranchCode", result.value, "BranchCode", "BranchName");
			this.commons = null; 
	    }
	}
	
    this.load_dd_filtered_currency = function(prod_type)
    {
        var result = IRSWorld.Internet.InternetAddBeneficiary.getdropdown_source_filteredcurrency(prod_type, document.getElementById("txtHiddenUserRole").value).value;
        
        if(result != null && result != "undefined")
	    {
	        this.commons = new common_method();         
			this.commons.to_load_dropdownlist("ddTargetCurrency", result, "CurrCode", "CurrName");
			this.commons = null; 
	    }
    }
		
    this.show_fields = function()
    {
        var benecifid = document.getElementById('ddBeneficiaryName').value;
        
        if(benecifid == 'null')
        {
            alert('Please select beneficiary!'); 
            document.getElementById('ddBeneficiaryName').focus();
            return;
        }
        
        document.getElementById('tdAccountNo').innerHTML = 'Account No:&nbsp;';
        
        switch(document.getElementById("ddProductType").value)
        {
            case  '1': this.show_div("divTarCurr~divBranchCode~divAccountType"); break; // Wealth Multiplier 
            case  '2': this.show_div("divTarCurr~divBranchCode~divAccountType~divAccountNo"); break; // Additional Wealth Multiplier
            case  '3': this.show_div("divTarCurr~divBranchCode"); break; // PNBig Placement excluded accttype
            case  '4': this.show_div("divTarCurr~divBranchCode"); break; // Time Deposit Placement Excluded accttype
            case  '5': this.show_div("divTarCurr~divBranchCode~divAccountNo"); break; // Add PNBig Placement Excluded accttype
            case  '6': this.show_div("divTarCurr~divBranchCode~divAccountNo"); break; // Add Time Deposit PLacement Excluded accttype 
            case  '7': this.show_div("divTarCurr~divBranchCode"); break;  // Advise and Pay - Batch
            case  '8': ((document.getElementById("txtHiddenUserRole").value == '10') ? this.show_div("divTarCurr") : this.show_div("divTarCurr~divBranchCode")); break;  // Door-To-Door - Metro Manila
            case  '9': ((document.getElementById("txtHiddenUserRole").value == '10') ? this.show_div("divTarCurr") : this.show_div("divTarCurr~divBranchCode")); break;  // Door-To-Door - Provinces
            case '10': this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divValidateAcct~divAccountStatus"); break;   // Rapid Remit       
            case '11': this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divValidateAcct~divAccountStatus"); break;   // Credit to Account
            case '12': this.show_div("divAccountNo~divTarCurr~divOthBankNamePDDTS"); break;  // PDDTS / EPCS
            //case '13': this.show_div("divAccountNo~divTarCurr~divOthBankName~divIsRuralBank"); break;  // Non-PDDTS / EPCS   
            case '13': 
            {
                if (document.getElementById("txtHiddenUserRole").value != '10')
                {
                    this.show_div("divAccountNo~divTarCurr~divOtherBank1"); 
                }
                else
                {
                    this.show_div("divAccountNo~divTarCurr~divOthBankName~divIsRuralBank");   
                }
                
                break;  // Non-PDDTS / Non-EPCS   
            }
            case '14': this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divValidateAcct~divAccountStatus"); break;   // Pangarap Account
            case '15': this.show_div("divTarCurr~divBranchCode~divAccountType"); break; // Open Acct Asst
            case '16': this.show_div("divAccountNo~divTarCurr~divOthBankName"); break;  // Door-To-Bank
            case '17': this.show_div("divTarCurr~divBranchCode"); break;  // Trust Placement
            case '18': this.show_div("divTarCurr~divBranchCode~divAccountType"); break; // Punla Placement
            case '22':  // GFC
            {
                if(Internet_Remitter_Products.show_gfc == true)
                {                    
                    if (Internet_Remitter_GFC != null)
                    {
                        Internet_Remitter_GFC = new GFMC_Maintenance();
                    }
                    
                    if (document.getElementById("txtHiddenUserRole").value != '10')
                    {
                        document.getElementById('btnSaveProd1').disabled = true;
                        Internet_Remitter_GFC.open_gfc_form(benecifid + "~" + this.get_rem_cifid(),'','Internet_Remitter_GFC', true); 
                    }
                }
                
                // do not show account number when user role is agent
                                
                //if  (this.isAgent(document.getElementById('txtHiddenUserRole').value) == true)
                if  (isAgentClient == true)
                {
                    this.show_div("divAccountType~divTarCurr~divBranchCode");
                }
                else if(document.getElementById("txtHiddenUserRole").value == '10')
                {
                    document.getElementById('tdAccountNo').innerHTML = 'GFC No:&nbsp;';
                    //this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divValidateAcct");
                    this.show_div("divAccountNo~divValidateAcct");
                }
                else
                {
                    this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode");
                }
                break;
            }
            case '23': this.show_div("divAccountNo~divTarCurr~divOthBankName"); break;  // Megalink IBFT                       
            case '24' : this.show_div("divTarCurr"); break; // Pay Anywhere in the World       
            case '26' : this.show_div("divAccountNo~divTarCurr~divOthBankName"); break; // SWIFT
            case '27' : this.show_div("divTarCurr"); break; // Advise and Pay Online
            case '256': this.show_div("divTarCurr~divAccountNo~divCommentOthers~divSearchOtherBank1~divOtherBank1~divOtherBank2"); break;   // Other Bank - BOC China
            case '257': this.show_div("divTarCurr~divAccountNo~divCommentOthers~divSearchOtherBank1~divOtherBank1~divOtherBank2"); break;   // Other Bank - ICBC
            default: break; 
        }

        Internet_Remitter_Products.show_gfc = true;
    }
        
    this.hide_all_div = function()
    {
        //var div = "divTarCurr~divAccountNo~divOthBankName~divBranchCode~divAccountType~divAccountCurr~divSearchOtherBank1~divOtherBank1~divOtherBank2~divCommentOthers~divValidateAcct";
        var div = "divTarCurr~divAccountNo~divOthBankName~divBranchCode~divAccountType~divAccountCurr~divSearchOtherBank1~divOtherBank1~divOtherBank2~divCommentOthers~divValidateAcct~divAccountStatus~divIsRuralBank~divOthBankNamePDDTS";
        
        var elems = div.split("~");
        var y = 0;
        
        for(y = 0; y < elems.length; y++)
        {
            if (elems[y] != null || elems[y] != "")
            { 
                document.getElementById(elems[y]).style.display   = "none";
            } 
        }
    }
    
    this.show_div = function(div)
    {
       this.hide_all_div(); 
       var elems = div.split("~");
       var y = 0;
        
       for(y = 0; y < elems.length; y++)
       {
           if (elems[y] != null || elems[y] != "")
           { 
               document.getElementById(elems[y]).style.display   = "";                              
           } 
       }
    }    
    
    this.disable_acct_fields = function()
    {
        var pCode = document.getElementById("ddProductType").value;
        
        if (pCode == '10' | pCode == '11' | pCode == '14')
        {
            this.disable_rr_fields(true);
        }
        else
        {
            this.disable_rr_fields(false);
        }
    }
    
    this.disable_rr_fields = function(isDisabled)
    {
        var ac = document.getElementById("ddTargetCurrency");
        var at = document.getElementById("ddAccountType");
        var bc = document.getElementById("ddBranchCode");
        var as = document.getElementById("ddAccountStatus");
            
        if(isDisabled == true)
        {
//            if(ac.value != "null" && ac.value != "") ac.disabled = true; else ac.disabled = false;
//            if(at.value != "null" && at.value != "") at.disabled = true; else at.disabled = false;
//            if(bc.value != "null" && bc.value != "") bc.disabled = true; else bc.disabled = false;
//            if(as.value != "null" && as.value != "") as.disabled = true; else as.disabled = false;
            ac.disabled = true;
            at.disabled = true;
            bc.disabled = true;
            as.disabled = true;
        }
        else
        {
            ac.disabled = false;
            at.disabled = false;
            bc.disabled = false;
            as.disabled = false;
        }
    }
       
    this.btn_edit_prod = function()
    {
        Internet_Remitter_Main.check_session();
        
        if(Internet_Remitter_Main.check_gfc_user(document.getElementById("txtHiddenSelectedBeneCIFID").value))
        {
            this.insert_flag = false;
            this.disable_field(false);
            this.save_mode_prod();
            
            document.getElementById("ddBeneficiaryName").disabled = true;
            document.getElementById("ddProductType").disabled     = true;
            
            if(document.getElementById("ddProductType").value == '10' | document.getElementById("ddProductType").value == '11' | document.getElementById("ddProductType").value == '14')
            {
                this.disable_rr_fields(true);
            }
            else
            {
                this.disable_rr_fields(false);
            }
        }        
    }
    
    this.btn_add_prod = function()
    {
        Internet_Remitter_Main.check_session();
        document.getElementById('txtHiddenMT103BankSeqId').value = '';
        document.getElementById('txtHiddenKBIAccount').value = '';
        document.getElementById('txtHiddenBeneProdSeqId').value = '';
     
        this.insert_flag = true;   
        this.hide_all_div();
        this.clear_screen();
        this.disable_field(false);
        this.save_mode_prod();
        this.clear_variables();
        
        if(view_bene == true)
        {
            var ds = null;
            ds = IRSWorld.Internet.InternetAddBeneficiary.get_cifid(document.getElementById("txtHiddenBeneMasterSeqId").value).value;
            if(ds != null && ds.Tables[0].Rows.length > 0)
            {
                document.getElementById("txtHiddenSelectedBeneCIFID").value = ds.Tables[0].Rows[0].CIFID;                                
                document.getElementById("ddBeneficiaryName").value = ds.Tables[0].Rows[0].CIFID;
                document.getElementById("ddBeneficiaryName").disabled = false;
            }
        }
    }
    
    this.remit = function()
    {
        alert(document.getElementById("txtHiddenBeneProdSeqId").value);
    }
    
    this.set_implementation_type = function(product_code)
    {
        var implementation_type = 0;
        
        switch(product_code)
        {
            case '1':  implementation_type = 1; break;    case '2': implementation_type  = 1;  break;
            case '3':  implementation_type = 1; break;    case '4': implementation_type  = 1;  break;    
            case '5':  implementation_type = 1; break;    case '6': implementation_type  = 1;  break;
            case '7':  implementation_type = 1; break;    case '8': implementation_type  = 3;  break;
            case '9':  implementation_type = 3; break;    case '10': implementation_type = 2;  break;
            case '11': implementation_type = 1; break;    case '12': implementation_type = 4;  break;
            case '13': implementation_type = 5; break;    case '14': implementation_type = 2;  break;
            case '15': implementation_type = 7; break;    case '16': implementation_type = 10; break;
            case '17': implementation_type = 1; break;    case '18': implementation_type = 1;  break;
            case '22': implementation_type = 2; break;    case '23': implementation_type = 4;  break;
            case '24': implementation_type = 1; break;    case '26': implementation_type = 12; break;
            case '27': implementation_type = 1; break;    case '256':implementation_type = 4;  break;
            case '257':implementation_type = 4; break;
            default: break;
        }
        return implementation_type;
    }

    this.save_gfc_for_client = function()
    {
        this.generic = new generic_method();
                    
        if (this.generic.is_empty(document.getElementById('txtAccountNo').value) == true)
        {
            alert("GFC number is required.");
            return;      
        }
        else
        {
            var GFCNo   = document.getElementById('txtAccountNo').value;
            var res = Internet_Beneficiary.enroll_bene_gfc(document.getElementById("ddBeneficiaryName").value, this.get_rem_cifid(), GFCNo);
            if (res != null && res != false && res != '')
            {   
                this.gfc_callback(res.split('|'));
            }
        }   
        
    }

    this.btn_save_prod = function()
    {
        if (document.getElementById("txtHiddenUserRole").value == '10' && document.getElementById("ddProductType").value == '22')
        {
            this.save_gfc_for_client();
        }
        else
        {
            this.btn_save_prod_1();
        }
    }
    
    this.btn_save_prod_1 = function()
    {
        Internet_Remitter_Main.check_session();
        
        if (this.validate_input() == true)
	    {
	        var pt = document.getElementById("ddProductType").value;
	        this.ReqAcct = IRSWorld.Internet.InternetAddBeneficiary.check_product_if_req_account(pt).value;
	        
        	if (this.ReqAcct == true & (document.getElementById("txtAccountNo").value == '' | document.getElementById("txtAccountNo").value == null | document.getElementById("txtAccountNo").value == 'null') )
            { 
                alert("Account number required.");
                return;  
            }
		    
		    if(this.insert_flag == true) /* add product */
		    {   
			    var sq = this.get_first_rec();    			

			    if(pt != '22' & this.ReqAcct == true) /* not GFC */
			    {
				    var ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_account(this.set_account_record()).value; 
				    var tostr_ret_val = String(Number(ret_val));
    				
				    if(tostr_ret_val == "NaN")
				    {
					    alert(ret_val + '\n' + "Please try again."); return;
				    }
				    else
				    {
				        this.AccountSeqId = tostr_ret_val;
				    }                
			    }
			    
    			//if(this.AccountSeqId == null)
			    if(pt != '22' && this.ReqAcct == true && this.AccountSeqId == null)
			    {
				    alert("There is no account created. Please try again."); return;
			    }
    			
			    if (sq == '0') /* not the first record */
			    {
				    var add_bene_ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_bene(this.set_prod_record(false)).value;
				    if(add_bene_ret_val.length > 0 && add_bene_ret_val.length <= 16)
				    {
				        if(pt == '256' || pt == '257')
				        {
				            this.get_other_bankdetails(this.OtherBankSeqId);
				            document.getElementById("txtOthBankName").value = '';
				            document.getElementById("txtOthBankAddress").value = '';
				        }
				        
				        /*update beneficiry details: relationship and notification mode*/
	                    IRSWorld.Internet.InternetAddBeneficiary.update_rel_notmode(document.getElementById("ddBeneficiaryName").value, this.get_rem_cifid());
	                    
	                    //if (document.getElementById("txtHiddenUserRole").value == '10' || isAgentClient == true)
	                    if (document.getElementById("txtHiddenUserRole").value == '10')
                        {
                            alert('Mode of delivery is added successfully. You may now send remittance.')
                        }
                        else
                        {
                            alert("RECORD COMMITTED SUCCESSFULLY!");
                        }
					    
					    this.disable_field(true);
					    this.edit_mode_prod();
				    } 
				    else alert(add_bene_ret_val + '\n' + "Please try again.");
			    }
			    else /* first record */
			    {
				    this.sq = sq;
    				
				    var upd_bene_ret_val = IRSWorld.Internet.InternetAddBeneficiary.update_bene(this.set_prod_record(false)).value;
				    if(upd_bene_ret_val.length > 0 && upd_bene_ret_val.length <= 16)
				    {
				        if(pt == '256' || pt == '257')
				        {
				            this.get_other_bankdetails(this.OtherBankSeqId);
				            document.getElementById("txtOthBankName").value = '';
				            document.getElementById("txtOthBankAddress").value = '';
				        }
				        
					    alert("RECORD COMMITTED SUCCESSFULLY!");
					    this.disable_field(true);
					    this.edit_mode_prod();
				    } 
				    else alert(upd_bene_ret_val + '\n' + "Please try again.");
			    }
			    this.load_grid_viewer();
		    }
		    else /* update product */
		    {
		        if((document.getElementById("txtHiddenBeneProdSeqId").value).length != 0)
	            {
	                this.ACCTSEQID = IRSWorld.Internet.InternetAddBeneficiary.get_acctseqid(document.getElementById("txtHiddenBeneProdSeqId").value).value;    
	            }
		        else this.ACCTSEQID = this.AccountSeqId;
		        
		        var acct_ret_val = '';
		        var tostr_ret_val = '';
		        // Modified 
		        if (this.ReqAcct == true)
		        {
		            //var acct_ret_val  = IRSWorld.Internet.InternetAddBeneficiary.update_account(this.set_account_record()).value;
		            // Soft delete old account number
		            var plist = '';
	                plist += "CIFID~" + document.getElementById("ddBeneficiaryName").value + '|';
	                plist += "SEQ_ID~" + this.ACCTSEQID + '|';
	                plist += "AccountDeleted~" + 'true';
	                
		            acct_ret_val  = IRSWorld.Internet.InternetAddBeneficiary.update_account(plist).value;
		            this.ACCTSEQID = null;
		            acct_ret_val = IRSWorld.Internet.InternetAddBeneficiary.add_account(this.set_account_record()).value; 
				    tostr_ret_val = String(Number(acct_ret_val));
    				
				    if(tostr_ret_val != "NaN")
				    {
					    this.AccountSeqId = tostr_ret_val;
				    }
		        }
		        		 		        
			    //tostr_ret_val = String(Number(acct_ret_val));
			    
			    if (this.ReqAcct == false)
			    {
		            this.BenSeqId           =   document.getElementById("txtHiddenBeneProdSeqId").value;
		        }
		        else
		        {
		            this.sq                 =   document.getElementById("txtHiddenBeneProdSeqId").value;
		        }
		        
		        var upd_bene_ret_val    =   IRSWorld.Internet.InternetAddBeneficiary.update_bene(this.set_prod_record(false)).value;
				
			    if(tostr_ret_val == "NaN")
			    {
				    alert(acct_ret_val + '\n' + "Please try again."); return;
			    }
			    else
			    {
			        var ds_bd = IRSWorld.Internet.InternetAddBeneficiary.get_bene_details(document.getElementById("txtHiddenBeneProdSeqId").value).value;
	                if (ds_bd != null && ds_bd.Tables[0].Rows.length > 0) 
                    {
                        ret_otc = ds_bd.Tables[0].Rows[0].RemitterCIFID + '|' +
                                  ds_bd.Tables[0].Rows[0].CIFID + '|' +
                                  ds_bd.Tables[0].Rows[0].ProductCode + '|' +
                                  ds_bd.Tables[0].Rows[0].ImplementationCode + '|' +
                                  ds_bd.Tables[0].Rows[0].AccountSeqId;
                    }
	                
			        if(pt == '256' || pt == '257')
			        {
			            this.get_other_bankdetails(this.OtherBankSeqId);
			            document.getElementById("txtOthBankName").value = '';
			            document.getElementById("txtOthBankAddress").value = '';
			        }
			        
				    alert('RECORD COMMITTED SUCCESSFULLY!');
				    this.ACCTSEQID = null;
				    this.disable_field(true);
					this.edit_mode_prod();
					this.load_grid_viewer();
			    }
		    }		
	    }        
    }
    
    this.get_first_rec = function()
    {
        var bcifid = document.getElementById("ddBeneficiaryName").value;
        var rcifid = this.get_rem_cifid();
        
        var first_rec = IRSWorld.Internet.InternetAddBeneficiary.get_first_rec(bcifid, rcifid).value;
        
        return first_rec;
    }
    
    this.isAgent = function(UserRole)
    {
        return IRSWorld.Internet.InternetAddBeneficiary.check_userrole_is_agent(UserRole).value;
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
    
    this.set_account_record = function()
    {
        this.BeneCIFID      = document.getElementById("ddBeneficiaryName").value;
        this.AccountNo      = document.getElementById("txtAccountNo").value;
        this.AccountType    = document.getElementById("ddAccountType").value;
        this.TargetCurrency = document.getElementById("ddTargetCurrency").value;
        this.OtherBankName  = document.getElementById("ddOtherBankName").value;
        this.BranchCode     = document.getElementById("ddBranchCode").value;
        this.KBIAccount     = document.getElementById("txtHiddenKBIAccount").value;
        this.ACCTSTAT       = document.getElementById("ddAccountStatus").value;
        this.GFMCNo         = null;
        this.GFMCCurrCode   = null;
        var pd = document.getElementById("ddProductType").value;
        if (pd == '13' && (document.getElementById("txtHiddenUserRole").value == '10') && document.getElementById("chkIsRuralBank").checked == false)
        {
            pd = '12';
//            if (IRSWorld.Internet.InternetAddBeneficiary.isPDDTSBank(this.OtherBankName).value == true)
//            {
//                pd = '12';
//            }
        }
        
        if(pd == '256')
        {
            this.OtherBankName = '888';
            this.set_other_banks();
        }
        else if(pd == '257')
        {
            this.OtherBankName = '777';
            this.set_other_banks();
        }
        //if (pd == '12' && document.getElementById("txtHiddenUserRole").value != '10' && isAgentClient == false)
        if (pd == '12' && document.getElementById("txtHiddenUserRole").value != '10')
        {
            this.OtherBankName  = document.getElementById("ddOtherBankNamePDDTS").value;    
        }        
        //if (pd == '13' && document.getElementById("chkIsRuralBank").checked == true)
        if (pd == '13')
        {
            this.BranchName = document.getElementById("txtOthBankName").value;
            this.OtherBankName = '000';
        }
              
        return this.set_plist_acct();
    }
    
    this.set_plist_acct = function()
	{
	    var plist = '';
	    
	    plist += "CIFID~" + document.getElementById("ddBeneficiaryName").value + '|';
	    
	    if (this.AccountNo != '' && this.AccountNo != null && this.AccountNo != 'null')                     plist += "AccountNo~"        + this.AccountNo        + '|';
	    if (this.KBIAccount != '' && this.KBIAccount != null && this.KBIAccount != 'null')                  plist += "KBIAccount~"       + this.KBIAccount       + '|';
	    if (this.AccountType != '' && this.AccountType != null && this.AccountType != 'null')               plist += "AccountType~"      + this.AccountType      + '|';	    
	    if (this.TargetCurrency != '' && this.TargetCurrency != null && this.TargetCurrency != 'null')      plist += "AccountCurrency~"  + this.TargetCurrency   + '|';
	    if (this.OtherBankName != '' && this.OtherBankName != null && this.OtherBankName != 'null')         plist += "DomBankCode~"      + this.OtherBankName    + '|';
	    if (this.BranchCode != '' && this.BranchCode != null && this.BranchCode != 'null')                  plist += "AccountBranchCode~"+ this.BranchCode       + '|';
	    if (this.OtherBankSeqId != '' && this.OtherBankSeqId != null && this.OtherBankSeqId != 'null')      plist += "OtherBankSeqId~"   + this.OtherBankSeqId   + '|';
	    if (this.ACCTSTAT != '' && this.ACCTSTAT != null && this.ACCTSTAT != 'null')                        plist += "AccountStatus~"    + this.ACCTSTAT         + '|';
	    if (this.BranchName != '' && this.BranchName != null && this.BranchName != 'null')                  plist += "branchname~"       + this.BranchName       + '|';
	    if (this.ACCTSEQID != null)                                                                         plist += "SEQ_ID~"           + this.ACCTSEQID        + '|';
	    
	    return plist.substring(0, plist.length-1);
	}
	
    this.set_prod_record = function(isRequest)
    {
        var rcifid = this.get_rem_cifid();        
        
        this.BeneCIFID      = document.getElementById("ddBeneficiaryName").value; 
        this.ProductType    = document.getElementById("ddProductType").value;
        //if (this.ProductType == '13' & (document.getElementById("txtHiddenUserRole").value == '10' || isAgentClient == true))
        if (this.ProductType == '13' && (document.getElementById("txtHiddenUserRole").value == '10') && document.getElementById("chkIsRuralBank").checked == false)
        {
            this.ProductType = '12';
//            if (IRSWorld.Internet.InternetAddBeneficiary.isPDDTSBank(document.getElementById("ddOtherBankName").value).value == true)
//            {
//                this.ProductType = '12';
//            }
        }
        
        this.ImplemType     = this.set_implementation_type(this.ProductType);
        
        if (this.ReqAcct == false)
        {
            this.TargetCurrency = document.getElementById("ddTargetCurrency").value;
            this.AccountType    = document.getElementById("ddAccountType").value;  
            this.BranchCode     = document.getElementById("ddBranchCode").value;            
        }
        
        if (isRequest == false) return this.set_plist(rcifid);
    }
    
    this.set_plist = function(rem_cifid)
	{
	    var plist = '';
	    
	    if(this.sq != '' && this.sq != null) plist += "SEQ_ID~" + this.sq + '|';
	    
	    plist += "RemitterCIFID~"       + rem_cifid + '|';
	    plist += "CIFID~"               + this.BeneCIFID + '|';
	    plist += "ProductCode~"         + this.ProductType + '|';
	    plist += "ImplementationCode~"  + this.ImplemType + '|';
	       
	    if(this.AccountSeqId != '' && this.AccountSeqId != null) 
	    { 
	        plist += "AccountSeqId~" + this.AccountSeqId + '|'; 
	    }
	    else 
	    {
	        plist += "AccountSeqId~0|" ; 
	    }
	    
	    if (this.ReqAcct == false)
	    {   
	        if (this.BranchCode != '' && this.BranchCode != null && this.BranchCode != 'null')                  plist += "ProdBranchCode~"+ this.BranchCode      + '|'; 
	        if (this.TargetCurrency != '' && this.TargetCurrency != null && this.TargetCurrency != 'null')      plist += "ProdCurrency~"  + this.TargetCurrency  + '|';
	        if (this.AccountType != '' && this.AccountType != null && this.AccountType != 'null')      plist += "ProdAccountType~"  + this.AccountType  + '|';
	        if (this.BenSeqId != '' && this.BenSeqId != null && this.BenSeqId != 'null')                        plist += "SEQ_ID~"        + this.BenSeqId        + '|';
	    }
	    
	    if(fxn_isRemit == 'otc')
	    {
	        ret_otc = rem_cifid + '|' + this.BeneCIFID + '|' + this.ProductType + '|' + this.ImplemType + '|' + this.AccountSeqId;
	    }
	    return plist.substring(0, plist.length-1);
	}
	
	this.show_div_loading = function(isVisible)
	{
	    if(isVisible == true)
	    {
	        document.getElementById('divLoadingx').style.display = '';
	    }
	    else
	    {
	        document.getElementById('divLoadingx').style.display = 'none';
	    }
	}
	
	this.check_digit_acctno = function(e)
    {
        var src;
        if (!e) e = window.event;
        if (!e.target) 
            src = window.event.srcElement;
        else 
            src = e.target;
            
        if (src.tagName == "INPUT" & src.type == 'text')
        {
            if (e.keyCode == 13) 
            {
                //if(document.getElementById("ddProductType").value == '10')
                // Edited by Hajz : 01/25/2010
                if(document.getElementById("ddProductType").value == '10' || document.getElementById("ddProductType").value == '11' || document.getElementById("ddProductType").value == '14')                
                {
                    this.generic = new generic_method();
                    this.commons = new common_method();
                    
                    /* clears and enables branch, acct type, acct curr*/
                    this.clear_branch_acctype_acctcurr();
                    //this.disable_rr_fields(false);
                    this.disable_rr_fields(true);
                    
                    var acctno   = this.generic.to_trim(document.getElementById('txtAccountNo').value);
                    
                    /* check if account is empty */
                    if(this.generic .is_empty(acctno) == true)
                    {
                        alert('Please enter account number.'); return;
                    }                   
                    
                    acctno = this.generic.to_remove_dash(acctno);
                    document.getElementById('txtAccountNo').value = acctno;
                    
                    IRSWorld.Internet.InternetAddBeneficiary.validate_acctno_fcalgo(acctno, this.check_digit_acctno_callback);
                    this.show_div_loading(true);
		            this.commons.to_show_loading_message('divLoading3');
		            document.getElementById("lblLoad3").innerHTML = "Validating account number. Please wait...";
                }
                else if (document.getElementById("txtHiddenUserRole").value == '10' && document.getElementById("ddProductType").value == '22')
                {
                    this.generic = new generic_method();
                    this.commons = new common_method();
                    
                    /* clears and enables branch, acct type, acct curr*/
                    this.clear_branch_acctype_acctcurr();
                    //this.disable_rr_fields(false);
                    //this.disable_rr_fields(true);
                    
                    var GFCNo   = this.generic.to_trim(document.getElementById('txtAccountNo').value);
                    var acctnum = '';
                                                     
                    var res = IRSWorld.TranClass.RemTranGFMCCommons.GFMCDetails_(GFCNo);
                    if (res.error != null)
                    {
                        alert(res.error.Message);
                    }
                    else
                    {
                        var ds = res.value;
                        if (ds != null && ds.Tables[0].Rows.length > 0 && this.generic.to_trim(ds.Tables[0].Rows[0].rec_CkAcctNo.toString()))
                        {
                            alert('GFC Number is valid');
                            //document.getElementById('txtBAccountNo').value = ds.Tables[0].Rows[0].rec_CkAcctNo.toString();            
                        }
                        else
                        {
                            alert('GFC number is invalid');
                        }
                    }
                }
                else return;
            }
        }        
    }
    
    this.check_digit_acctno_callback = function(result)
    {
        if (result != null && result.value != null)
        {
            if (result.value == false)
            {
                Internet_Remitter_Products.VALIDFC = false;
                Internet_Remitter_Products.show_div_loading(false);
                Internet_Remitter_Products.clear_branch_acctype_acctcurr();
                Internet_Remitter_Products.disable_rr_fields(false);
                alert("Account number is an invalid PNB account."); 
            }
            else
            {
                Internet_Remitter_Products.VALIDFC = true;
                Internet_Remitter_Products.get_acct_details();
            }
        }
    }
    
    this.clear_variables = function()
    {
        this.AccountNo         =   '';
        this.KBIAccount        =   '';
        this.AccountType       =   '';
        this.TargetCurrency    =   '';
        this.OtherBankName     =   '';
        this.BranchCode        =   '';
        this.OtherBankSeqId    =   ''; 
        this.ACCTSEQID         =   '';

        this.BeneCIFID         =   '';
        this.ProductType       =   '';
        this.ImplemType        =   '';
        this.AccountSeqId      =   '';
        this.BenSeqId          =   '';
        
        this.BranchName        =   '';
    }
    
    this.get_acct_details = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            internet_page.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
        this.generic = new generic_method();
        var acctno = this.generic.to_trim(document.getElementById('txtAccountNo').value);
        var acct_det = null;
        
        IRSWorld.Internet.InternetAddBeneficiary.get_acct_details(acctno, this.get_acct_details_callback);
        document.getElementById("lblLoad3").innerHTML = "Retrieving account details. Please wait...";
    }
    
    this.get_acct_details_callback = function(result)
    {
        Internet_Remitter_Products.show_div_loading(false);    
        
        this.generic = new generic_method();
        var acctno = this.generic.to_trim(document.getElementById('txtAccountNo').value);
        
        document.getElementById("txtHiddenKBIAccount").value = '';
        
        Internet_Remitter_Products.set_acct_details(IRSWorld.Internet.InternetAddBeneficiary.get_algo_type(acctno).value, result.value);
    }
    
    this.set_acct_details = function(algo_type, acct_det)
    {
        if(algo_type == "KBI")
        {
            document.getElementById("txtHiddenKBIAccount").value = document.getElementById("txtAccountNo").value;
        }
        
        if(acct_det != null && acct_det != 'null')
        {
            var ret_val = acct_det.split("~");
            
            if(ret_val.length > 1) /* successful retrieval of fc account details */
            {
                if(algo_type == "KBI")
                {
                    if(ret_val[4] != null && ret_val[4] != '')
                    {
                        alert("Account no. entered is a KBI account, this will be replaced by an FC account.");
                        document.getElementById("txtAccountNo").value = ret_val[4];
                    }                
                }
                
                var ds = IRSWorld.Internet.InternetAddBeneficiary.get_converted_branchcode(ret_val[3]).value;
                var converted_bcode = -1;
                
                if (ds != null && ds.Tables[0].Rows.length > 0) 
                {
                    converted_bcode = ds.Tables[0].Rows[0].BranchCode;
                }
                
                this.commons = new common_method();
                this.commons.to_set_dropdownlist_index("ddTargetCurrency",  ret_val[0]);
                this.commons.to_set_dropdownlist_index("ddAccountType",     ret_val[1]);
                this.commons.to_set_dropdownlist_index("ddBranchCode",      converted_bcode);
                this.commons.to_set_dropdownlist_index("ddAccountStatus",   ret_val[2]);
                
                this.ACCTSTAT = ret_val[2];
                if(ret_val[2] == 1 || ret_val[2] == 5)
                {
                    alert("Account is already closed!");
                }
                this.disable_rr_fields(true);
            }
            else /* has errMsg */
            {
                alert(ret_val);
            }        
        }
        else
        {
            if (document.getElementById("txtHiddenUserRole").value == '10')
            {
                alert('Unable to retrieve account details. Please check account number.');
            }
            else
            {
                alert('Unable to retrieve account details, please fill in the following fields.');           
                this.disable_rr_fields(false);            
            }
            
            this.clear_branch_acctype_acctcurr();
            
        }
    }
    
    this.clear_branch_acctype_acctcurr = function()
    {
        this.commons = new common_method();
        this.commons.to_set_dropdownlist_index("ddTargetCurrency",  '');
        this.commons.to_set_dropdownlist_index("ddAccountType",     '');
        this.commons.to_set_dropdownlist_index("ddBranchCode",      '');
        this.commons.to_set_dropdownlist_index("ddAccountStatus",   '');
    }
    
    this.set_other_banks = function()
    {
        var seqid103 = document.getElementById('txtHiddenMT103BankSeqId').value;
        var bankname = document.getElementById('txtOthBankName').value;
        var bankaddr = document.getElementById('txtOthBankAddress').value;
        
        if (seqid103 == '' || bankname != '' || bankaddr != '')
        {            
            if(bankname.length == 0 && bankaddr.length == 0) return;
            
            var data_row = new Array();
        
            data_row[data_row.length]  = "BankName~"    + bankname;
            data_row[data_row.length]  = "BankAddress~" + bankaddr;
            
            data_row.join("|");
            
            this.add_other_banks(data_row);
        }
        else
        {
            this.OtherBankSeqId = seqid103;
        }
    }    
    
    this.add_other_banks = function(data_row)
    {
        var oth_seqid = null;
        
        oth_seqid = IRSWorld.Internet.InternetAddBeneficiary.add_other_banks(data_row).value;
        document.getElementById('txtHiddenMT103BankSeqId').value = oth_seqid;
        this.OtherBankSeqId = oth_seqid;
    }
    
    this.add_other_banks_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {              
            document.getElementById('txtHiddenMT103BankSeqId').value = result.value;
            this.OtherBankSeqId = result.value;
        }
    }
            
	this.validate_input = function()
	{
	    var bene     = document.getElementById("ddBeneficiaryName").value;
	    var ptype    = document.getElementById("ddProductType").value;
	    var acctno   = document.getElementById("txtAccountNo").value;
        var accttype = document.getElementById("ddAccountType").value;
        var tarcurr  = document.getElementById("ddTargetCurrency").value;
        var othbname = document.getElementById("ddOtherBankName").value;
        var othbnamepddts = document.getElementById("ddOtherBankNamePDDTS").value;
        var pnbbcode = document.getElementById("ddBranchCode").value;
        var othlblbankname = document.getElementById("txtMT103BankName").value; 
        var othtxtbankname = document.getElementById("txtOthBankName").value;   
        var othtxtbankadd  = document.getElementById("txtOthBankAddress").value; 
        var isRural = document.getElementById("chkIsRuralBank").checked;
        var branchname = "";
        if(document.getElementById('ddOtherBankName').value == "" || document.getElementById('ddOtherBankName').value == "null" || document.getElementById('ddOtherBankName').value == null)
        {
            branchname = document.getElementById("txtOthBankName").value; 
        }
        else
        {
            branchname = document.getElementById("ddOtherBankName").value; 
        }

	    this.generic = new generic_method();
	    this.error_input = '';
	    var foc_elem     = '';
	    var branchtype   = '';
	    
	    var ds = null;
	    ds = IRSWorld.Internet.InternetAddBeneficiary.get_bankname(pnbbcode).value;	    
	    
	    if(ds != null && ds.Tables[0].Rows.length > 0)
	    {
	        branchtype = ds.Tables[0].Rows[0].BranchType;
	    }	    
	    
	    if(bene == 'null' || bene == '')
	    {
	        this.error_input += "Please select beneficiary.\n"; 
            if (foc_elem.length == 0) foc_elem = 'ddBeneficiaryName';
	    }
	    else
	    {
	        if(ptype == 'null' || ptype == '')
            {
                if (document.getElementById("txtHiddenUserRole").value != '10')
                {
                    this.error_input += "Please select product type.\n"; 
                }
                else
                {
                    this.error_input += "Please select mode of remittance.\n"; 
                }
                
                
                if (foc_elem.length == 0) foc_elem = 'ddProductType';    
            }
            else
            {
                if(tarcurr == 'null' || tarcurr == '')
                {
                    this.error_input += "Target currency required.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'ddTargetCurrency';    
                }
                /*
                if(pnbbcode == 'null' || pnbbcode == '')
                {
                    this.error_input += "Branch code required.\n"; 
                    if (foc_elem.length == 0) foc_elem = 'ddBranchCode';    
                }
                */
	            
	            if (ptype == 22)
	            {
                    if(this.ACCTSTAT == 1 || this.ACCTSTAT == 5)
                    {
                        alert("Can not enroll closed accounts!");
                        return false;
                    }
                    
                    // Added by Hajz : 01/24/2011 : Added FC Acct Status (7,10,11,12,18,19) (dormant accounts) in invalid list 
                    if(this.ACCTSTAT == 7 || this.ACCTSTAT == 10 || this.ACCTSTAT == 11 || this.ACCTSTAT == 12 || this.ACCTSTAT == 18 || this.ACCTSTAT == 19)
                    {
                        alert("Can not enroll dormant accounts!");
                        return false;
                    }                      
                }
	            else if (ptype == 10 || ptype == 11 || ptype == 14)
	            {
                    if(this.ACCTSTAT == 1 || this.ACCTSTAT == 5)
                    {
                        alert("Can not enroll closed accounts!");
                        return false;
                    }
                    // Added by Hajz : 01/24/2011 : Added FC Acct Status (7,10,11,12,18,19) (dormant accounts) in invalid list 
                    if(this.ACCTSTAT == 7 || this.ACCTSTAT == 10 || this.ACCTSTAT == 11 || this.ACCTSTAT == 12 || this.ACCTSTAT == 18 || this.ACCTSTAT == 19)
                    {
                        alert("Can not enroll dormant accounts!");
                        return false;
                    }              
                
	                if (this.generic.is_empty(acctno) == true)
                    { 
                        this.error_input += "Account number required.\n";  
                        foc_elem = 'txtAccountNo';
                    }
                    if(IRSWorld.Internet.InternetAddBeneficiary.validate_acctno_fcalgo(acctno).value == false)
                    {
                        this.error_input += "Account number is an invalid PNB account.\n";
                        foc_elem = 'txtAccountNo';
                    }                                        
	            }
	            else if (ptype == 12 || ptype == 13 || ptype == 16)
	            {
	                if (this.generic.is_empty(acctno) == true)
                    { 
                        this.error_input += "Account number required.\n";  
                        foc_elem = 'txtAccountNo';           
                    }
//                    if(ptype == 13 && (othbname == 'null' || othbname == '') & !isRural)
//                    {
//                        this.error_input += "Other bank name required.\n"; 
//                        if (foc_elem.length == 0) foc_elem = 'ddOtherBankName';    
//                    }
                    //if(ptype == 13 && (branchname == 'null' || branchname == '') & isRural)
                    if(ptype == 13 && (branchname == 'null' || branchname == ''))
                    {
                        this.error_input += "Bank name required.\n"; 
                        if (foc_elem.length == 0) foc_elem = 'txtOthBankName';    
                    }
                    if(ptype == 12 && (othbnamepddts == 'null' || othbnamepddts == ''))
                    {
                        this.error_input += "Other bank name required.\n"; 
                        if (foc_elem.length == 0) foc_elem = 'ddOtherBankNamePDDTS';    
                    }
//                    if(ptype == 13 && isRural && tarcurr != 'null' && tarcurr != '' && tarcurr != '1' )
//                    {
//                        this.error_input += "Invalid Target Currency, Only Peso is allowed when Remitting to Rural Banks.\n"; 
//                        if (foc_elem.length == 0) foc_elem = 'ddTargetCurrency';
//                    }
	            }	            
	            else if (ptype == 256 || ptype == 257)
	            {
    	            if (this.generic.is_empty(acctno) == true)
                    { 
                        this.error_input += "Account number required.\n";  
                        foc_elem = 'txtAccountNo';           
                    }
                    if(othlblbankname == '')
                    {
                        if (othtxtbankname == '' && othtxtbankadd == '') this.error_input += "Bank name and address required.\n";
                        else if(othtxtbankname == '') this.error_input += "Bank name required.\n";
                        else if(othtxtbankadd == '')  this.error_input += "Bank address required.\n";
                    }
	            }    
            }
        }
        if (foc_elem.length > 0)
        {
            try{ document.getElementById(foc_elem).focus(); }catch(e){}
        }
        if (this.error_input != '')
        {
            alert(this.error_input)
            return false;
        }
        
        // Check for duplicate product entry
        if (IRSWorld.Internet.InternetAddBeneficiary.check_product_duplicate(document.getElementById('ddBeneficiaryName').value, ptype, ((acctno == null | acctno == "null") ? "" : acctno), ((tarcurr == null | tarcurr == "null") ? "" : tarcurr), ((othbname == null | othbname == "null") ? "" : othbname), ((pnbbcode == null | pnbbcode == "null") ? "" : pnbbcode), ((othlblbankname == null | othlblbankname == "null") ? "" : othlblbankname)).value)
        {
            alert("Cannot insert duplicate entry");
            return false;
        }
        
        return true;	    
	}
	this.Productype_Onchange = function()
	{
	    this.clear_prod_fields();
	    this.show_fields();
	    this.filter_branches();
	    this.disable_acct_fields();
	    this.load_dd_filtered_currency(document.getElementById('ddProductType').value);
	}
	
	this.IsRuralBank_OnClick = function()
	{
	    var a = document.getElementById('chkIsRuralBank').checked;
	    
	    if (a == true)
	    {
	        document.getElementById('ddOtherBankName').value = 'null';
	        document.getElementById('divOthBankName').style.display = 'none';
	        
	        document.getElementById('divOtherBank1').style.display = '';
	    }
	    else
	    {
	        document.getElementById('divOtherBank1').value = 'null';
	        document.getElementById('divOthBankName').style.display = '';
	        
	        document.getElementById('divOtherBank1').style.display = 'none';	    
	    }
	}
	

	
}