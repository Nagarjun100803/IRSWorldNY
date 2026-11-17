/* BOYET CBIP JULY 31 2017 */
var gfc_existing_card = false;
/* BOYET CBIP JULY 31 2017 */

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
    this.BenSeqId       = null;    
    this.ReqAcct        = null;
    this.BranchName     = null;
    this.Remarks        = null;
    //Added for SG RFS 20160616
    this.AccountRemarks = null;
    this.AccountRemarksOthers = null;
    this.edit           = null;
    this.acctseq_id     = null;
    
    this.load_grid_viewer = function()
    {
        this.grid = new grid_viewer();        
        this.session_end         = true;
        
        var remcifid = this.get_rem_cifid();
        var role =  document.getElementById('txtHiddenUserRole').value.toString();
        var bencifid = (view_bene) ? document.getElementById("txtHiddenSelectedBeneCIFID").value : '-1';
        
        this.grid.sql_source = "sp_internet_grid_cifbene_products~" + remcifid + '~' + bencifid + '~' + role;
        this.grid.div_container  = document.getElementById("DIVPRODUCTSGRID");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = '0,1';
        this.grid.function_exec = function()
        {
            Internet_Remitter_Main.bene_product_grid_ready("DIVPRODUCTSGRID");
        };        
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
        
        /*  CBIP BOYET JULY 31 2017*/
        gfc_existing_card = false;
        /*  CBIP BOYET JULY 31 2017*/

        if(a.length > 0)
        {
            ckno     = a[0].toString();
            gfcno    = a[1].toString();
            currency = a[2].toString();
            mode = a[3].toString();

            /* EXISTING CARD CBIP BOYET JULY 31 2017 */
            if (mode == "excard" || mode == "EXCARD")
            {
                gfc_existing_card = true;
            }
            /* EXISTING CARD CBIP BOYET JULY 31 2017 */
            
            if (document.getElementById("txtHiddenUserRole").value == '10')
            {
                acctseqid = ckno;
            }
            else
            {
                var rs = IRSWorld.Internet.InternetRemitter.GET_CIF_ACCOUNTSEQID(ckno);
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
            
            var accttype = IRSWorld.Internet.InternetRemitter.get_account_type(acctseqid).value;
            var acctstat = IRSWorld.Internet.InternetRemitter.get_account_status(acctseqid).value;
            var actbcode = IRSWorld.Internet.InternetRemitter.get_account_bcode(acctseqid).value;
            
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
            /* FOR CBIP */
            
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
        //this.grid.datarow_column = 0;
        this.grid.datarow_column = '0,1';
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
	    this.showRemarks(false, false);
	    if (document.getElementById("txtHiddenBeneProdSeqId").value != '')
	    {
	        this.pull_server_data();
	    }
	    else
	    {
	        this.add_mode_prod();
	    }
	}
	
	this.pull_server_data = function()
	{
	    ajax_show_status(true);
	    IRSWorld.Internet.InternetRemitter.get_bene_prod(document.getElementById("txtHiddenBeneProdSeqId").value, document.getElementById("txtHiddenUserRole").value,  this.pull_server_data_callback);
	}
	
	this.pull_server_data_callback = function(result)
	{
	    if (result.error != null)
	    {
	        alert('RemitterIds@pull_server_data: ' + result.error.Message);
	    }

	    if (result != null && result.value != null)
        {
	        Internet_Remitter_Products.add_mode_prod();
	        
            this.commons = new common_method();
            this.generic = new generic_method();
            
            var ds = result.value;
            var pd = null;
            
            if (ds != null && ds.Tables[0].Rows.length > 0)
            {
                pd = ds.Tables[0].Rows[0].ProductCode;
                pd = (pd == '12' & (document.getElementById("txtHiddenUserRole").value == '10')) ? '13' : pd;
                
                Internet_Remitter_Products.load_dd_filtered_currency(pd);
                Internet_Remitter_Products.load_dd_filtered_InstapayBank(); //INSTAPAY ALYSSA SEPTEMBER 2019

                document.getElementById("ddBeneficiaryName").value  = ds.Tables[0].Rows[0].CIFID;
                document.getElementById("ddProductType").value      = pd;
                document.getElementById("txtAccountNo").value       = ds.Tables[0].Rows[0].AccountNo;
                document.getElementById("ddAccountType").value      = ds.Tables[0].Rows[0].AccountType;
                document.getElementById("ddOtherBankName").value    = ds.Tables[0].Rows[0].DomBankCode;
                document.getElementById("ddBranchCode").value       = ds.Tables[0].Rows[0].BranchCode;
                document.getElementById("ddAccountStatus").value    = ds.Tables[0].Rows[0].Status;
                this.OtherBankSeqId                                 = ds.Tables[0].Rows[0].OtherBankSeqId;
                document.getElementById('txtHiddenMT103BankSeqId').value = ds.Tables[0].Rows[0].OtherBankSeqId;
                document.getElementById("chkIsRuralBank").checked   = false;
                //Added for SG RFS 20160616
                document.getElementById("ddlRemarks").value         = ds.Tables[0].Rows[0].AccountRemarks;
                if (ds.Tables[0].Rows[0].AccountRemarks != null && ds.Tables[0].Rows[0].AccountRemarks == 4)
                {
                    document.getElementById("divAccountRemarksOthers").style.display = "";
                    document.getElementById("txtRemarks").value     = ds.Tables[0].Rows[0].AccountRemarksOthers;
                }
                else
                {
                    document.getElementById("divAccountRemarksOthers").style.display = "none";
                }
                Internet_Remitter_Products.acctseq_id = ds.Tables[0].Rows[0].SEQ_ID;
                
                if(pd == '256' || pd == '257')
                {
                    Internet_Remitter_Products.get_other_bankdetails(this.OtherBankSeqId);
                }
                
                document.getElementById("ddTargetCurrency").value  = ds.Tables[0].Rows[0].Currency; 
                
                Internet_Remitter_Products.show_gfc = false;
                Internet_Remitter_Products.show_fields(); 
                
                if(pd == '13' && ds.Tables[0].Rows[0].DomBankCode == '000')
                {
                    document.getElementById("chkIsRuralBank").checked = true;
                    Internet_Remitter_Products.IsRuralBank_OnClick();
                    document.getElementById("txtOthBankName").value = ds.Tables[0].Rows[0].BranchName;
                }
                if(pd == '12')
                {
                    document.getElementById("ddOtherBankNamePDDTS").value   = ds.Tables[0].Rows[0].DomBankCode;                   
                }
                if (pd == '258') {
                    document.getElementById("ddOtherBankNameINSTAPAY").value = ds.Tables[0].Rows[0].DomBankCode;
                }
                
                Internet_Remitter_Products.edit_mode_prod();
                
                this.generic.to_clear_null("txtAccountNo");
            }
        }
        ajax_show_status(false);
	}
	
	this.get_other_bankdetails = function(othbankseqid)
    {
        if (othbankseqid != '' && othbankseqid != null)
        {
            var ds = null;
            ds = IRSWorld.Internet.InternetRemitter.get_otherbanks_details(String(othbankseqid)).value;
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
        this.commons.to_read_only("tblOthBankNameINSTAPAY", isDisabled);
        this.commons.to_read_only("tblBranchCode",  isDisabled);
        this.commons.to_read_only("tblAccountType", isDisabled);
        this.commons.to_read_only("tblAccountCurr", isDisabled);
        this.commons.to_read_only("tblAccountStatus", isDisabled);
        this.commons.to_read_only("tblSearchOtherBank1", isDisabled);
        this.commons.to_read_only("tblOtherBank1",  isDisabled);
        this.commons.to_read_only("tblOtherBank2",  isDisabled);
        //Added for SG RFS 20160502
        this.commons.to_read_only("tblAccountName", isDisabled);
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
        //Added for SG RFS 20160502
        //var fields = "txtAccountNo~ddTargetCurrency~ddOtherBankName~ddBranchCode~ddAccountType~ddAccountCurrency~txtOthBankName~txtOthBankAddress~ddAccountStatus~ddOtherBankNamePDDTS~divAccountName~ddlRemarks~txtRemarks";
        var fields = "txtAccountNo~ddTargetCurrency~ddOtherBankName~ddBranchCode~ddAccountType~ddAccountCurrency~txtOthBankName~txtOthBankAddress~ddAccountStatus~ddOtherBankNamePDDTS~ddOtherBankNameINSTAPAY~divAccountName";

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
        IRSWorld.Internet.InternetRemitter.getdropdown_source_filteredbranches(prod_type, this.dd_filtered_branches_callback);
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

    //Instapay Alyssa SEPTEMBER 2019
	this.load_dd_filtered_InstapayBank = function () {
	    var result = IRSWorld.Internet.InternetRemitter.getdropdown_source_filteredInstapayBank();

	    if (result != null && result != "undefined") {
	        this.commons = new common_method();
	        this.commons.to_load_dropdownlist("ddOtherBankNameINSTAPAY", result.value, "BranchCode", "BranchName");
	        this.commons = null;
	    }
	}

	this.dd_filtered_InstapayBank_callback = function (result) {
	    if (result != null && result != "undefined") {
	        this.commons = new common_method();
	        this.commons.to_load_dropdownlist("ddOtherBankNameINSTAPAY", result.value, "BranchCode", "BranchName");
	        this.commons = null;
	    }
	}
    //Instapay Alyssa

    this.load_dd_filtered_currency = function(prod_type)
    {
        var result = IRSWorld.Internet.InternetRemitter.getdropdown_source_filteredcurrency(prod_type, document.getElementById("txtHiddenUserRole").value).value;
        
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
        document.getElementById("divAccountRemarks").style.display = "none";
        var pd = document.getElementById("ddProductType").value;
        switch(pd)
        {
            case  '1': this.show_div("divTarCurr~divBranchCode~divAccountType"); break; // Wealth Multiplier 
            case  '2': this.show_div("divTarCurr~divBranchCode~divAccountType~divAccountNo"); break; // Additional Wealth Multiplier
            case  '3': this.show_div("divTarCurr~divBranchCode"); break; // PNBig Placement excluded accttype
            case  '4': this.show_div("divTarCurr~divBranchCode"); break; // Time Deposit Placement Excluded accttype
            case  '5': this.show_div("divTarCurr~divBranchCode~divAccountNo~divAccountRemarks"); break; // Add PNBig Placement Excluded accttype
            case  '6': this.show_div("divTarCurr~divBranchCode~divAccountNo~divAccountRemarks"); break; // Add Time Deposit PLacement Excluded accttype 
            case  '7': this.show_div("divTarCurr~divBranchCode"); break;  // Advise and Pay - Batch
            case  '8': ((document.getElementById("txtHiddenUserRole").value == '10') ? this.show_div("divTarCurr~divAccountRemarks") : this.show_div("divTarCurr~divBranchCode")); break;  // Door-To-Door - Metro Manila
            case  '9': ((document.getElementById("txtHiddenUserRole").value == '10') ? this.show_div("divTarCurr~divAccountRemarks") : this.show_div("divTarCurr~divBranchCode")); break;  // Door-To-Door - Provinces
            case '10': this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divValidateAcct~divAccountStatus~divAccountRemarks"); break;   // Rapid Remit       
            case '11': this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divValidateAcct~divAccountStatus~divAccountRemarks"); break;   // Credit to Account
            case '12': this.show_div("divAccountNo~divTarCurr~divOthBankNamePDDTS~divAccountRemarks"); break;  // PDDTS / EPCS
            case '258': this.show_div("divAccountNo~divTarCurr~divOthBankNameINSTAPAY"); break;  // INSTAPAY ALYSSA SEPTEMBER 2019
            case '13':
            {
                if (document.getElementById("txtHiddenUserRole").value != '10')
                {
                    this.show_div("divAccountNo~divTarCurr~divOtherBank1~divAccountRemarks");
                }
                else
                {
                    this.show_div("divAccountNo~divTarCurr~divOthBankName~divIsRuralBank~divAccountRemarks");
                }
                
                break;  // Non-PDDTS / Non-EPCS   
            }
            case '14': this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divValidateAcct~divAccountStatus~divAccountRemarks"); break;   // Pangarap Account
            case '15': this.show_div("divTarCurr~divBranchCode~divAccountType"); break; // Open Acct Asst
            case '16': this.show_div("divAccountNo~divTarCurr~divOthBankName~divAccountRemarks"); break;  // Door-To-Bank
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
                if  (isAgentClient == true)
                {
                    this.show_div("divAccountType~divTarCurr~divBranchCode");
                }
                else if(document.getElementById("txtHiddenUserRole").value == '10')
                {
                    document.getElementById('tdAccountNo').innerHTML = 'GFC No:&nbsp;';
                    this.show_div("divAccountNo~divValidateAcct~divAccountRemarks");
                }
                else
                {
                    this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divAccountRemarks");
                }
                break;
            }
            case '23': this.show_div("divAccountNo~divAccountType~divTarCurr~divBranchCode~divAccountRemarks"); break;  // Megalink IBFT                       
            case '24' : this.show_div("divTarCurr"); break; // Pay Anywhere in the World       
            case '26' : this.show_div("divAccountNo~divTarCurr~divOthBankName"); break; // SWIFT
            case '27' : this.show_div("divTarCurr"); break; // Advise and Pay Online
            case '256': this.show_div("divTarCurr~divAccountNo~divCommentOthers~divSearchOtherBank1~divOtherBank1~divOtherBank2~divAccountRemarks"); break;   // Other Bank - BOC China
            case '257': this.show_div("divTarCurr~divAccountNo~divCommentOthers~divSearchOtherBank1~divOtherBank1~divOtherBank2~divAccountRemarks"); break;   // Other Bank - ICBC
            default: break; 
        }
        Internet_Remitter_Products.show_gfc = true;
    }

    this.hide_all_div = function()
    {
        //Added for SG RFS 20160502
        //var div = "divTarCurr~divAccountNo~divOthBankName~divBranchCode~divAccountType~divAccountCurr~divSearchOtherBank1~divOtherBank1~divOtherBank2~divCommentOthers~divValidateAcct~divAccountStatus~divIsRuralBank~divOthBankNamePDDTS~divAccountName";
        var div = "divOthBankNameINSTAPAY~divTarCurr~divAccountNo~divOthBankName~divBranchCode~divAccountType~divAccountCurr~divSearchOtherBank1~divOtherBank1~divOtherBank2~divCommentOthers~divValidateAcct~divAccountStatus~divIsRuralBank~divOthBankNamePDDTS~divAccountName";

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
        this.edit = true;
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
    
    //Added for SG RFS 20160622
    this.showRemarks = function(remarks, others)
    {
        if (remarks)
        {
            document.getElementById("divAccountRemarks").style.display = "";
        }
        else
        {
            document.getElementById("divAccountRemarks").style.display = "none";
            var select = document.getElementById("ddlRemarks");
	        select.selectedIndex = -1;
	        document.getElementById("txtRemarks").value = "";
        }
        
        if (others)
        {
            document.getElementById("divAccountRemarksOthers").style.display = "";
        }
        else
        {
            document.getElementById("divAccountRemarksOthers").style.display = "none";
            var select = document.getElementById("ddlRemarks");
	        select.selectedIndex = -1;
	        document.getElementById("txtRemarks").value = "";
        }
    }
    
    this.btn_add_prod = function()
    {
        //Added for SG RFS 20160616
        this.showRemarks(false, false);
        this.edit = false;
        Internet_Remitter_Products.acctseq_id = "";
        
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
            ds = IRSWorld.Internet.InternetRemitter.get_cifid(document.getElementById("txtHiddenBeneMasterSeqId").value).value;
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
            case '22': implementation_type = 2; break;    //case '23': implementation_type = 4;  break;
            case '23': implementation_type = 17;  break;
            case '24': implementation_type = 1; break;    case '26': implementation_type = 12; break;
            case '27': implementation_type = 1; break;    case '256':implementation_type = 4;  break;
            case '257': implementation_type = 4; break;   case '258': implementation_type = 29; break; //Instapay
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
	        this.ReqAcct = IRSWorld.Internet.InternetRemitter.check_product_if_req_account(pt).value;
	        
        	if (this.ReqAcct == true & (document.getElementById("txtAccountNo").value == '' | document.getElementById("txtAccountNo").value == null | document.getElementById("txtAccountNo").value == 'null') )
            { 
                alert("Account number required.");
                return;  
            }
		    
		    if(this.insert_flag == true) /* add product */
		    {   
		        var sq = this.get_first_rec();

		        /*BOYET CBIP JULY 31 2017 */
		        if (pt != null && parseInt(pt) == 22 && gfc_existing_card == true)
		        {
		            var ret_val = IRSWorld.Internet.InternetRemitter.add_account(this.set_account_record()).value;
		            var tostr_ret_val = String(Number(ret_val));

		            if (ret_val != null && parseInt(ret_val) > 0)
		            {
		                this.AccountSeqId = tostr_ret_val;
		            }
		            else
		            {
		                alert(ret_val + '\n' + "Please try again."); return;
		            }
		        }
		        /*BOYET CBIP JULY 31 2017 */


			    if(pt != '22' & this.ReqAcct == true) /* not GFC */
			    {
				    var ret_val = IRSWorld.Internet.InternetRemitter.add_account(this.set_account_record()).value; 
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
    			
			    if(pt != '22' && this.ReqAcct == true && this.AccountSeqId == null)
			    {
				    alert("There is no account created. Please try again."); return;
			    }
    			
			    if (sq == '0') /* not the first record */
			    {
				    var add_bene_ret_val = IRSWorld.Internet.InternetRemitter.add_bene(this.set_prod_record(false)).value;
				    if(add_bene_ret_val.length > 0 && add_bene_ret_val.length <= 16)
				    {
				        if(pt == '256' || pt == '257')
				        {
				            this.get_other_bankdetails(this.OtherBankSeqId);
				            document.getElementById("txtOthBankName").value = '';
				            document.getElementById("txtOthBankAddress").value = '';
				        }
				        
				        /*update beneficiry details: relationship and notification mode*/
	                    IRSWorld.Internet.InternetRemitter.update_rel_notmode(document.getElementById("ddBeneficiaryName").value, this.get_rem_cifid());
	                    
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
    				
				    var upd_bene_ret_val = IRSWorld.Internet.InternetRemitter.update_bene(this.set_prod_record(false)).value;
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
			    this.edit = false;
                Internet_Remitter_Products.acctseq_id = "";
                this.btn_add_prod();
			    this.load_grid_viewer();
		    }
		    else /* update product */
		    {
		        if((document.getElementById("txtHiddenBeneProdSeqId").value).length != 0)
	            {
	                this.ACCTSEQID = IRSWorld.Internet.InternetRemitter.get_acctseqid(document.getElementById("txtHiddenBeneProdSeqId").value).value;
	            }
		        else this.ACCTSEQID = this.AccountSeqId;
		        
		        var acct_ret_val = '';
		        var tostr_ret_val = '';
		        // Modified 
		        if (this.ReqAcct == true)
		        {
		            //var acct_ret_val  = IRSWorld.Internet.InternetRemitter.update_account(this.set_account_record()).value;
		            // Soft delete old account number
		            var plist = '';
	                plist += "CIFID~" + document.getElementById("ddBeneficiaryName").value + '|';
	                plist += "SEQ_ID~" + this.ACCTSEQID + '|';
	                plist += "AccountDeleted~" + 'true';
	                
		            acct_ret_val  = IRSWorld.Internet.InternetRemitter.update_account(plist).value;
		            this.ACCTSEQID = null;
		            acct_ret_val = IRSWorld.Internet.InternetRemitter.add_account(this.set_account_record()).value; 
				    tostr_ret_val = String(Number(acct_ret_val));
    				
				    if(tostr_ret_val != "NaN")
				    {
					    this.AccountSeqId = tostr_ret_val;
				    }
		        }
			    
			    if (this.ReqAcct == false)
			    {
		            this.BenSeqId           =   document.getElementById("txtHiddenBeneProdSeqId").value;
		        }
		        else
		        {
		            this.sq                 =   document.getElementById("txtHiddenBeneProdSeqId").value;
		        }
		        
		        var upd_bene_ret_val    =   IRSWorld.Internet.InternetRemitter.update_bene(this.set_prod_record(false)).value;
				
			    if(tostr_ret_val == "NaN")
			    {
				    alert(acct_ret_val + '\n' + "Please try again."); return;
			    }
			    else
			    {
			        var ds_bd = IRSWorld.Internet.InternetRemitter.get_bene_details(document.getElementById("txtHiddenBeneProdSeqId").value).value;
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
			    this.edit = false;
                Internet_Remitter_Products.acctseq_id = "";
                this.btn_add_prod();
		    }
	    }
    }
    
    this.get_first_rec = function()
    {
        var bcifid = document.getElementById("ddBeneficiaryName").value;
        var rcifid = this.get_rem_cifid();
        var first_rec = IRSWorld.Internet.InternetRemitter.get_first_rec(bcifid, rcifid).value;
        return first_rec;
    }
    
    this.isAgent = function(UserRole)
    {
        return IRSWorld.Internet.InternetRemitter.check_userrole_is_agent(UserRole).value;
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
        var pd = document.getElementById("ddProductType").value;
        
        this.BeneCIFID      = document.getElementById("ddBeneficiaryName").value;
        this.AccountNo      = document.getElementById("txtAccountNo").value;
        this.AccountType    = document.getElementById("ddAccountType").value;
        this.TargetCurrency = document.getElementById("ddTargetCurrency").value;
        this.OtherBankName  = (pd == 23) ? document.getElementById("ddBranchCode").value : document.getElementById("ddOtherBankName").value;
        this.BranchCode     = (pd == 23) ? '' : document.getElementById("ddBranchCode").value;
        this.KBIAccount     = document.getElementById("txtHiddenKBIAccount").value;
        this.ACCTSTAT       = document.getElementById("ddAccountStatus").value;
        this.GFMCNo         = null;
        this.GFMCCurrCode   = null;
        
        if (pd == '13' && (document.getElementById("txtHiddenUserRole").value == '10') && document.getElementById("chkIsRuralBank").checked == false)
        {
            pd = '12';
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
        
        if (pd == '12' && document.getElementById("txtHiddenUserRole").value != '10')
        {
            this.OtherBankName  = document.getElementById("ddOtherBankNamePDDTS").value;    
        }        
        
        if (pd == '13')
        {
            this.BranchName = document.getElementById("txtOthBankName").value;
            this.OtherBankName = '000';
        }
        if (pd == '258') //INSTAPAY Alyssa 2019
        {
            this.OtherBankName = document.getElementById("ddOtherBankNameINSTAPAY").value;
        }
            
        return this.set_plist_acct();
    }
    
    this.set_plist_acct = function()
	{
	    var plist = '';
	    plist += "CIFID~" + document.getElementById("ddBeneficiaryName").value + '|';
	    
	    if (this.AccountNo != '' && this.AccountNo != null && this.AccountNo != 'null')                                     plist += "AccountNo~"               + this.AccountNo            + '|';
	    if (this.KBIAccount != '' && this.KBIAccount != null && this.KBIAccount != 'null')                                  plist += "KBIAccount~"              + this.KBIAccount           + '|';
	    if (this.AccountType != '' && this.AccountType != null && this.AccountType != 'null')                               plist += "AccountType~"             + this.AccountType          + '|';	    
	    if (this.TargetCurrency != '' && this.TargetCurrency != null && this.TargetCurrency != 'null')                      plist += "AccountCurrency~"         + this.TargetCurrency       + '|';
	    if (this.OtherBankName != '' && this.OtherBankName != null && this.OtherBankName != 'null')                         plist += "DomBankCode~"             + this.OtherBankName        + '|';
	    if (this.BranchCode != '' && this.BranchCode != null && this.BranchCode != 'null')                                  plist += "AccountBranchCode~"       + this.BranchCode           + '|';
	    if (this.OtherBankSeqId != '' && this.OtherBankSeqId != null && this.OtherBankSeqId != 'null')                      plist += "OtherBankSeqId~"          + this.OtherBankSeqId       + '|';
	    if (this.ACCTSTAT != '' && this.ACCTSTAT != null && this.ACCTSTAT != 'null')                                        plist += "AccountStatus~"           + this.ACCTSTAT             + '|';
	    if (this.BranchName != '' && this.BranchName != null && this.BranchName != 'null')                                  plist += "branchname~"              + this.BranchName           + '|';
	    if (this.ACCTSEQID != null)                                                                                         plist += "SEQ_ID~"                  + this.ACCTSEQID            + '|';
	    if (this.Remarks != '' && this.Remarks != null && this.Remarks != 'null')                                           plist += "Remarks~"                 + this.Remarks              + '|';
	    //Added for SG RFS 20160616
	    if (this.AccountRemarks != '' && this.AccountRemarks != null && this.AccountRemarks != 'null')                      plist += "AccountRemarks~"          + this.AccountRemarks       + '|';
	    if (this.AccountRemarksOthers != '' && this.AccountRemarksOthers != null && this.AccountRemarksOthers != 'null')    plist += "AccountRemarksOthers~"    + this.AccountRemarksOthers + '|';	    
	    return plist.substring(0, plist.length-1);
	}
	
    this.set_prod_record = function(isRequest)
    {
        var rcifid = this.get_rem_cifid();        
        
        this.BeneCIFID      = document.getElementById("ddBeneficiaryName").value; 
        this.ProductType    = document.getElementById("ddProductType").value;
        
        if (this.ProductType == '13' && (document.getElementById("txtHiddenUserRole").value == '10') && document.getElementById("chkIsRuralBank").checked == false)
        {
            this.ProductType = '12';
        }
        
        this.ImplemType     = this.set_implementation_type(this.ProductType);
        
        if (this.ReqAcct == false)
        {
            this.TargetCurrency = document.getElementById("ddTargetCurrency").value;
            this.AccountType    = document.getElementById("ddAccountType").value;  
            this.BranchCode     = document.getElementById("ddBranchCode").value;            
        }
        if (this.ReqAcct == true && this.ProductType == '258') //Instapay Alyssa 2019
        {
            this.TargetCurrency = document.getElementById("ddTargetCurrency").value;
            this.AccountType = document.getElementById("ddAccountType").value;
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

                if(document.getElementById("ddProductType").value == '14')                
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
                    
                    IRSWorld.Internet.InternetRemitter.validate_acctno_fcalgo(acctno, this.check_digit_acctno_callback);
                    this.show_div_loading(true);
		            this.commons.to_show_loading_message('divLoading3');
		            document.getElementById("lblLoad3").innerHTML = "Validating account number. Please wait...";
		            //Added for SG RFS 20160502
		            document.getElementById("txtAccountName").value = "";
                }
                    // CJ RFC: 25631 - SSS Pension Exception
                else if (document.getElementById("ddProductType").value == '10' || document.getElementById("ddProductType").value == '11') {

                    this.generic = new generic_method();
                    this.commons = new common_method();

                    /* clears and enables branch, acct type, acct curr*/
                    this.clear_branch_acctype_acctcurr();
                    //this.disable_rr_fields(false);
                    this.disable_rr_fields(true);

                    var acctno = this.generic.to_trim(document.getElementById('txtAccountNo').value);
                    var currcifid = this.generic.to_trim(document.getElementById("txtHiddenRemCIFID").value);

                    /* check if account is empty */
                    if (this.generic.is_empty(acctno) == true) {
                        alert('Please enter account number.'); return;
                    }
                    acctno = this.generic.to_remove_dash(acctno);
                    document.getElementById('txtAccountNo').value = acctno;

                    this.show_div_loading(true);
                    this.commons.to_show_loading_message('divLoading3');
                    document.getElementById("lblLoad3").innerHTML = "Checking Product Code. Please wait...";
                    document.getElementById("txtAccountName").value = "";

                    var res = IRSWorld.TranClass.RemTranGFMCCommons.RapidRemitDetails_(currcifid, acctno, 0);
                    if (res.value != null || res.value != '' || res.value != 'null') {
                        if (res.value == '04') {
                            Internet_Remitter_Products.show_div_loading(false);
                            alert("Error Message: Remittance is not allowed for this account");
                        }
                            //else if (res.value.toString() == 'null') {
                            //    alert("Cannot connect to GFC WS.");
                            //}
                        else {
                            IRSWorld.Internet.InternetRemitter.validate_acctno_fcalgo(acctno, this.check_digit_acctno_callback);
                            //this.show_div_loading(true);
                            //this.commons.to_show_loading_message('divLoading3');
                            document.getElementById("lblLoad3").innerHTML = "Validating account number. Please wait...";
                            document.getElementById("txtAccountName").value = "";
                        }
                    }
                    else {
                        /* RFC-26582 - Pending Override (GFC WS is down) */
                        IRSWorld.Internet.InternetRemitter.validate_acctno_fcalgo(acctno, this.check_digit_acctno_callback);
                        //this.show_div_loading(true);
                        //this.commons.to_show_loading_message('divLoading3');
                        document.getElementById("lblLoad3").innerHTML = "Validating account number. Please wait...";
                        document.getElementById("txtAccountName").value = "";
                        /* RFC-26582 - Pending Override (GFC WS is down) */
                    }
                }
                else if (document.getElementById("txtHiddenUserRole").value == '10' && document.getElementById("ddProductType").value == '22')
                {
                    this.generic = new generic_method();
                    this.commons = new common_method();
                    
                    /* clears and enables branch, acct type, acct curr*/
                    this.clear_branch_acctype_acctcurr();
                    
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
        this.edit              =   '';
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

        /* RFS BLOCK TIME DEPOSIT */
        document.getElementById("lblLoad3").innerHTML = "Validating Account. Please wait...";
        var TD = IRSWorld.Internet.InternetRemitter.isAccountTD(acctno, true).value;
        if (TD != null && TD.length > 0) {
            alert(TD);
            document.getElementById("btnSaveProd1").disabled = true;
            Internet_Remitter_Products.show_div_loading(false);
        }
        else {
            IRSWorld.Internet.InternetRemitter.get_acct_details(acctno, this.get_acct_details_callback);
            document.getElementById("lblLoad3").innerHTML = "Retrieving account details. Please wait...";
        }
    }
    
    this.get_acct_details_callback = function(result)
    {
        Internet_Remitter_Products.show_div_loading(false);    
        
        this.generic = new generic_method();
        var acctno = this.generic.to_trim(document.getElementById('txtAccountNo').value);
        
        document.getElementById("txtHiddenKBIAccount").value = '';
        
        Internet_Remitter_Products.set_acct_details(IRSWorld.Internet.InternetRemitter.get_algo_type(acctno).value, result.value);
    }
    
    this.set_acct_details = function(algo_type, acct_det)
    {
        if(algo_type == "KBI")
        {
            document.getElementById("txtHiddenKBIAccount").value = document.getElementById("txtAccountNo").value;
        }
        this.Remarks = algo_type;
        
        if(acct_det != null && acct_det != 'null')
        {
            var ret_val = acct_det.split("~");
            
            if(ret_val.length > 1) /* successful retrieval of fc account details */
            {
                if (document.getElementById('txtAccountNo').value != ret_val[4].toString()) {
                    alert("Account no. will be converted to new SI account.")
                    document.getElementById('txtAccountNo').value = ret_val[4].toString();
                }
                
                var ds = IRSWorld.Internet.InternetRemitter.get_converted_branchcode(ret_val[3]).value;
                var converted_bcode = -1;
                
                if (ds != null && ds.Tables[0].Rows.length > 0) 
                {
                    converted_bcode = ds.Tables[0].Rows[0].BranchCode;
                }
                
                this.commons = new common_method();
                this.commons.to_set_dropdownlist_index("ddTargetCurrency",  ret_val[0]);
                this.commons.to_set_dropdownlist_index("ddAccountType",     ret_val[1]);
                this.commons.to_set_dropdownlist_index("ddBranchCode", converted_bcode);

                /* FOR CBIP */
                var select = document.getElementById("ddAccountStatus");
                var length = select.options.length;
                for (i = 0; i < length; i++) {
                    select.options[i] = null;
                }

                var ds;
                this._accountType = ret_val[1];
                if (this._accountType == "1") {
                    ds = IRSWorld.Internet.InternetRemitter.getAccountStatus("1").value;
                }
                else {
                    ds = IRSWorld.Internet.InternetRemitter.getAccountStatus("2").value;
                }

                load_ddl("ddAccountStatus", ds.Tables[0], "FldValue", "FldDescription", 0, 'i', null, '');
                this.ACCTSTAT = ret_val[2];
                this.commons.to_set_dropdownlist_index("ddAccountStatus", this.ACCTSTAT);

                /*
                if (ret_val[4] != "") {
                    document.getElementById("txtAccountNo").value = ret_val[4];
                }
                */

                if (this._accountType == "2") //IM
                {
                    if (this.ACCTSTAT != 1 && this.ACCTSTAT != 10) {
                        alert("Account Status is invalid");
                    }
                }
                else //ST
                {
                    if (this.ACCTSTAT != 1) {
                        alert("Account Status is invalid");
                    }
                }

                //Added for SG RFS 20160502
                var arr = ret_val[6].split("=");
                if (arr[0] == "1") {
                    document.getElementById("divAccountName").style.display = "";
                    document.getElementById("txtAccountName").value = ret_val[5];
                }
                else {
                    document.getElementById("divAccountName").style.display = "none";
                    document.getElementById("txtAccountName").value = "";
                }
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
        
        oth_seqid = IRSWorld.Internet.InternetRemitter.add_other_banks(data_row).value;
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
	    var rem      = document.getElementById("txtHiddenRemCIFID").value;
	    var bene     = document.getElementById("ddBeneficiaryName").value;
	    var ptype    = document.getElementById("ddProductType").value;
	    var acctno   = document.getElementById("txtAccountNo").value;
        var accttype = document.getElementById("ddAccountType").value;
        var tarcurr  = document.getElementById("ddTargetCurrency").value;
        var othbname = document.getElementById("ddOtherBankName").value;
        var othbnamepddts = document.getElementById("ddOtherBankNamePDDTS").value;
        var othbnameinstapay = document.getElementById("ddOtherBankNameINSTAPAY").value;
        var pnbbcode = document.getElementById("ddBranchCode").value;
        var othlblbankname = document.getElementById("txtMT103BankName").value; 
        var othtxtbankname = document.getElementById("txtOthBankName").value;   
        var othtxtbankadd  = document.getElementById("txtOthBankAddress").value; 
        var isRural = document.getElementById("chkIsRuralBank").checked;
        var branchname = document.getElementById("txtOthBankName").value;
	    this.generic = new generic_method();
	    this.error_input = '';
	    var foc_elem     = '';
	    var branchtype   = '';
	    var ds = null;
	    
	    //ds = IRSWorld.Internet.InternetRemitter.get_bankname(pnbbcode).value;
	    //INSTAPAY ALYSSA SEPTEMBER 2019
	    if (ptype == 258) {
	        ds = IRSWorld.Internet.InternetRemitter.get_bankname_instapay(pnbbcode).value;
	    }
	    else {
	        ds = IRSWorld.Internet.InternetRemitter.get_bankname(pnbbcode).value;
	    }

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
	            
	            /* FOR CBIP */
                if (ptype == 10 || ptype == 11 || ptype == 14) {
                    if (this._accountType == "2") //IM
                    {
                        if (this.ACCTSTAT != 1 && this.ACCTSTAT != 10) {
                            alert("Account Status is invalid");
                            return false;
                        }
                    }
                    else //ST
                    {
                        if (this.ACCTSTAT != 1) {
                            alert("Account Status is invalid");
                            return false;
                        }
                    }
                }
                /* FOR CBIP */
                else if (ptype == 10 || ptype == 11 || ptype == 14) {
                    if (this.generic.is_empty(acctno) == true) {
                        this.error_input += "Account number required.\n";
                        foc_elem = 'txtAccountNo';
                    }
                }
	            //else if (ptype == 12 || ptype == 13 || ptype == 16 || ptype == 23)
                else if (ptype == 12 || ptype == 13 || ptype == 16 || ptype == 23 || ptype == 258) //Instapay Alyssa 2019
	            {
	                if (this.generic.is_empty(acctno) == true)
                    { 
                        this.error_input += "Account number required.\n";  
                        foc_elem = 'txtAccountNo';           
                    }
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
                    if (ptype == 258 && (othbnameinstapay == 'null' || othbnameinstapay == '')) { //INSTAPAY
                        this.error_input += "INSTAPAY bank name required.\n";
                        if (foc_elem.length == 0) foc_elem = 'ddOtherBankNameINSTAPAY';
                    }
                    if (ptype==23 && (pnbbcode == 'null' || pnbbcode == ''))
                    {
                        this.error_input += "IBFT bank code/name required.\n"; 
                        if (foc_elem.length == 0) foc_elem = 'ddBranchCode';    
                    }
                    else if (ptype==23)
                    {
                        othbname = pnbbcode;
                        pnbbcode = '';
                    }
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
        if (ptype == 10)
        {
            if (this.Remarks == '' || this.Remarks == null || this.Remarks == 'null' || this.Remarks == undefined)
            {
                if (this.edit == false)
                {
                    this.error_input += "Incomplete record retrieved from the host. Please press ENTER on the account number and try again.";
                }
            }
            
            if (this.edit == true && (this.Remarks == null || this.Remarks == '' || this.Remarks == undefined))
            {
                if (IRSWorld.Internet.InternetRemitter.checkAlgoRemarks(Internet_Remitter_Products.acctseq_id.toString()).value)
                {
                    this.error_input += "Incomplete record retrieved from the host. Please press ENTER on the account number and try again.";
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
        if (this.edit == false)
        {
            if (IRSWorld.Internet.InternetRemitter.check_product_duplicate(bene, rem, ptype, ((acctno == null | acctno == "null") ? "" : acctno), ((tarcurr == null | tarcurr == "null") ? "" : tarcurr), ((othbname == null | othbname == "null") ? "" : othbname), ((pnbbcode == null | pnbbcode == "null") ? "" : pnbbcode), ((othlblbankname == null | othlblbankname == "null") ? "" : othlblbankname)).value)
            {
                alert("Cannot insert duplicate entry");
                return false;
            }
        }
        
        //Added for SG RFS 20160616
        var select = document.getElementById("ddlRemarks");
        if (select.selectedIndex > 0)
        {
            this.AccountRemarks = "";
            this.AccountRemarksOthers = "";
            if (select.options[select.selectedIndex].text == "Others" && document.getElementById("txtRemarks").value == "")
            {
                alert("Please enter detailed remarks if Others is selected");
                return false;
            }
            else
            {
                this.AccountRemarks = select.options[select.selectedIndex].value;
                this.AccountRemarksOthers = document.getElementById("txtRemarks").value;
            }
        }
        
        //Added for SG RFS 20160714
        if (document.getElementById("astBranchCode").innerHTML != "")
        {
            var select = document.getElementById("ddBranchCode");
            if (select.selectedIndex <= 0)
            {
                alert("Branch code is required. Please select a branch.");
                return false;
            }
        }
        
        return true;
	}
	
	this.Productype_Onchange = function()
	{
	    if (document.getElementById("astBranchCode") != null)
	    {
	        document.getElementById("astBranchCode").style.display = "none";
	    }
	    else
	    {
	        document.getElementById("astBranchCode").innerHTML = "*";
	    }
	    this.clear_prod_fields();
	    this.show_fields();
	    this.filter_branches();
	    this.disable_acct_fields();
	    this.load_dd_filtered_currency(document.getElementById('ddProductType').value);
	    //Added for SG RFS 20160714
	    this.loadAsterisk();
	    this.load_dd_filtered_InstapayBank();
	}
	
	//Added for SG RFS 20160714
	this.loadAsterisk = function()
	{
	    var pt = document.getElementById("ddProductType").value;
	    var dyna = IRSWorld.Internet.InternetRemitter.getDynamicValidationBranchCode().value;
	    
	    if (dyna)
	    {
	        var manda = IRSWorld.Internet.InternetRemitter.getBranchCodeMandatory(pt).value;
	        if (manda)
	        {
	            document.getElementById("astBranchCode").style.display = "block";
	            document.getElementById("astBranchCode").innerHTML = "*";
	        }
	        else
	        {
	            document.getElementById("astBranchCode").innerHTML = "";
	            document.getElementById("astBranchCode").style.display = "none";
	        }
	    }
	}
	
	//Added for SG RFS 20160616
	this.SelectRemarks = function()
	{
	    var select = document.getElementById("ddlRemarks");
	    document.getElementById("txtRemarks").value = "";
        if (select.options[select.selectedIndex].text == "Others")
        {
            document.getElementById("divAccountRemarksOthers").style.display = "";
        }
        else
        {
            document.getElementById("divAccountRemarksOthers").style.display = "none";
        }
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