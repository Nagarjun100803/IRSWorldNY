function execute_ajaxgridview_command(param)
{
    if (cifviewdetails != null && cifviewdetails != "undefined")
    {
        cifviewdetails.do_ajaxgridview(param);
    }
}

function fix_dim()
{
    if (top.document.getElementById('divCenter'))
    {
        var div = document.getElementById('divmain');
        if (div)
        {
            div.style.width = top.document.getElementById('divCenter').offsetWidth + 'px';
            div.style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
            
            var td = document.getElementById('tdmain');
            if (td) 
            {
                td.style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
            }
            
            var ov = document.getElementById('divover');
            if (ov) 
            {
                ov.style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
                ov.style.width = (top.document.getElementById('divCenter').offsetWidth - (top.document.getElementById('divCenter').offsetWidth * .18)) + 'px';
            }  
        }
    }
}

function cif_viewdetails()
{
    this.viewdetails_loc = null;
    this.CIFID           = null;
    this.SEQ_ID          = null;
    this.CIFType         = null;
    this.generic         = null;
    this.caller          = null;
    
    this.do_ajaxgridview = function(parameter)
    {   
    }
    
    this.load_main = function()
    {
        fix_dim();
        
        this.param1 = String(window.location).split('?');
        this.param2 = this.param1[1].split('&');
        this.CIFID  = this.param2[0];
        this.SEQ_ID = this.param2[1];
        this.caller = this.param2[2];
        
        this.show_loading_progress();
        this.show_panel(this.CIFID, this.SEQ_ID);
        this.clear_screen();
        this.load_record(this.CIFID);
    }
    
    this.show_loading_progress = function()
    {
        this.commons = new common_method();
        this.commons.to_show_loading_message("CIFVIEWADDRESSGRID~CIFVIEWOCCUPATIONSGRID~CIFVIEWACCOUNTSGRID~CIFVIEWCARDSGRID~CIFVIEWIDSGRID~CIFVIEWCONTACTSGRID~CIFVIEWMERCHANTSGRID~CIFVIEWBENEGRID~CIFVIEWMERINFOGRID~CIFVIEWOTHERIDS~CIFVIEWWATCHLIST");
        this.commons = null;
    }
    
    this.back = function()
    {
        if(this.caller == 'cif')
        {
            window.location = "../CIF/cifmain.aspx?function=edit_cif&cifid=" + this.CIFID;
        }
        else if(this.caller == 'cif_from_useradmin')
        {
            window.location = "../CIF/cifmain.aspx?function=search&cifid=" + this.CIFID;
        }
        else if(this.caller == 'int')
        {
            //window.location = "../../Internet/InternetRemitter.aspx?function=show_rem_info&cifid=" + this.CIFID + "&seqid=" + this.SEQ_ID;
            window.location = "../Internet/InternetRemitter.aspx?function=show_rem_info&cifid=" + this.CIFID + "&seqid=" + this.SEQ_ID;
        }
    }
        
    this.show_elements = function(elements)
    {
        var elems = elements.split("~");
        var y = 0;
       
        for(y = 0; y < elems.length; y++)
        {
           if (elems[y] != null || elems[y] != "")
            { 
                document.getElementById(elems[y]).style.display = "block";
            } 
        }
    }
    
    this.show_panel = function(cifid, seq_id)
    {
        var ds_ciftype  = IRSWorld.CIF.CIFViewDetails.get_ciftype(seq_id).value;
        
        if (ds_ciftype.Tables[0].Rows.length > 0) this.CIFType = ds_ciftype.Tables[0].Rows[0].CIFTYPE;
        
        var rem_count  = IRSWorld.CIF.CIFViewDetails.get_cifclass_count(seq_id, 1).value;
        var bene_count = IRSWorld.CIF.CIFViewDetails.get_cifclass_count(seq_id, 2).value;
        var mer_count  = IRSWorld.CIF.CIFViewDetails.get_cifclass_count(seq_id, 3).value; 
        var agt_count  = IRSWorld.CIF.CIFViewDetails.get_cifclass_count(seq_id, 4).value;
        var cou_count  = IRSWorld.CIF.CIFViewDetails.get_cifclass_count(seq_id, 9).value;
        var oth_count  = IRSWorld.CIF.CIFViewDetails.get_cifclass_count(seq_id,10).value;
        
        if (this.CIFType == 1)      this.show_elements("CIFVIEWINDIVIDUAL");
        else if (this.CIFType == 2) this.show_elements("CIFVIEWBUSINESS");
        
        if (oth_count > 0)
        {
            this.show_elements("CIFOTHERIDSGRID~CIFVIEWOTHERIDS");
        }
        if (cou_count > 0)
        {
            this.show_elements("CIFACCOUNTSGRID~CIFVIEWACCOUNTSGRID~CIFOTHERIDSGRID~CIFVIEWOTHERIDS");
        }
        if (mer_count > 0)
        {
            this.show_elements("CIFACCOUNTSGRID~CIFVIEWACCOUNTSGRID~CIFCONTACTSGRID~CIFVIEWCONTACTSGRID~CIFMERINFOGRID~CIFVIEWMERINFOGRID~CIFOTHERIDSGRID~CIFVIEWOTHERIDS");
        }
        if (agt_count > 0)
        {
            this.show_elements("CIFVIEWADDRESSGRID~CIFADDRESSGRID~CIFACCOUNTSGRID~CIFVIEWACCOUNTSGRID~CIFCONTACTSGRID~CIFVIEWCONTACTSGRID~CIFOTHERIDSGRID~CIFVIEWOTHERIDS");
        }
        if (bene_count > 0)
        {
            this.show_elements("CIFVIEWADDRESSGRID~CIFADDRESSGRID~CIFOCCUPATIONSGRID~CIFVIEWOCCUPATIONSGRID~CIFACCOUNTSGRID~CIFVIEWACCOUNTSGRID~CIFCARDSGRID~CIFVIEWCARDSGRID~CIFIDSGRID~CIFVIEWIDSGRID~CIFCONTACTSGRID~CIFVIEWCONTACTSGRID~CIFOTHERIDSGRID~CIFVIEWOTHERIDS~CIFWATCHLISTGRID~CIFVIEWWATCHLIST");
        }
        if (rem_count > 0) 
        {
            this.show_elements("CIFVIEWADDRESSGRID~CIFADDRESSGRID~CIFOCCUPATIONSGRID~CIFVIEWOCCUPATIONSGRID~CIFCARDSGRID~CIFVIEWCARDSGRID~CIFIDSGRID~CIFVIEWIDSGRID~CIFCONTACTSGRID~CIFVIEWCONTACTSGRID~CIFMERCHANTSGRID~CIFVIEWMERCHANTSGRID~CIFVIEWREMINFO~CIFBENEGRID~CIFVIEWBENEGRID~CIFOTHERIDSGRID~CIFVIEWOTHERIDS~CIFWATCHLISTGRID~CIFVIEWWATCHLIST");
        }
        
        else this.show_elements("CIFVIEWADDRESSGRID~CIFADDRESSGRID~CIFOCCUPATIONSGRID~CIFVIEWOCCUPATIONSGRID~CIFACCOUNTSGRID~CIFVIEWACCOUNTSGRID~CIFCARDSGRID~CIFVIEWCARDSGRID~CIFIDSGRID~CIFVIEWIDSGRID~CIFCONTACTSGRID~CIFVIEWCONTACTSGRID~CIFVIEWBENEGRID~CIFOTHERIDSGRID~CIFVIEWOTHERIDS~CIFWATCHLISTGRID~CIFVIEWWATCHLIST");
    }
    
    this.clear_screen = function()
    {
        this.commons = new common_method();
        
        this.commons.to_clear_screen("tblCIFIndividualDetails");
        this.commons.to_clear_screen("tblCIFBusinessDetails");
        this.commons.to_clear_screen("tblCIFAddressDetails");
        this.commons.to_clear_screen("tblCIFOccupationDetails");
        this.commons.to_clear_screen("tblCIFAccountsDetails");
        this.commons.to_clear_screen("tblCIFCardsDetails");
        this.commons.to_clear_screen("tblCIFIdsDetails");
        this.commons.to_clear_screen("tblCIFContactsDetails");
        this.commons.to_clear_screen("tblMerchantInfoDetails");
        this.commons.to_clear_screen("tblRemitterInfoDetails");
        this.commons.to_clear_screen("tblBeneficiariesDetails");
        this.commons.to_clear_screen("tblCIFSpouseDetails");
    
        this.commons = null;
    }
        
    this.load_record = function(cifid)
    {
        this.load_master_record(cifid);
        
        this.load_reminfo_record(cifid);
        
        this.load_grid(cifid, "sp_cif_grid_address~",        "CIFVIEWADDRESSGRID");
        this.load_grid(cifid, "sp_cif_grid_occupations~",    "CIFVIEWOCCUPATIONSGRID");
        this.load_grid(cifid, "sp_cif_grid_accounts~",       "CIFVIEWACCOUNTSGRID");
        this.load_grid(cifid, "sp_cif_grid_cards~",          "CIFVIEWCARDSGRID");
        this.load_grid(cifid, "sp_cif_grid_ids~",            "CIFVIEWIDSGRID");
        this.load_grid(cifid, "sp_cif_grid_contacts~",       "CIFVIEWCONTACTSGRID");
        this.load_grid(cifid, "sp_cif_grid_merchants~",      "CIFVIEWMERCHANTSGRID");
        this.load_grid(cifid, "sp_cif_grid_beneficiaries~",  "CIFVIEWBENEGRID");
        this.load_grid(cifid, "sp_cif_grid_spouse~",         "CIFVIEWSPOUSEGRID");
        this.load_grid(cifid, "sp_cif_grid_merchants_info~", "CIFVIEWMERINFOGRID");
        this.load_grid(cifid, "sp_cif_grid_other_ids~",      "CIFVIEWOTHERIDS");
        this.load_grid(cifid, "sp_cif_grid_watchlist~",      "CIFVIEWWATCHLIST");
    }
    
    this.load_master_record = function(cifid)
    {
        var ds1 = IRSWorld.CIF.CIFViewDetails.get_master_record(cifid).value;
        
        if (ds1 != null && ds1.Tables[0].Rows.length > 0)
        {
            this.commons = new common_method();
            this.generic = new generic_method();
            
            this.SEQ_ID      = ds1.Tables[0].Rows[0].SEQ_ID;
            this.CIFType     = ds1.Tables[0].Rows[0].CIFType;
            this.CIFClass    = ds1.Tables[0].Rows[0].CIFClass;
            
            if (this.CIFType == 1)
            {
                document.getElementById("lblCIFID").innerHTML       = ds1.Tables[0].Rows[0].CIFID;
                document.getElementById("lblLastName").innerHTML    = ds1.Tables[0].Rows[0].LastName;
                document.getElementById("lblFirstName").innerHTML   = ds1.Tables[0].Rows[0].FirstName;
                document.getElementById("lblMiddleName").innerHTML  = ds1.Tables[0].Rows[0].MiddleName;
                document.getElementById("lblGender").innerHTML      = ds1.Tables[0].Rows[0].Gender;
                document.getElementById("lblAge").innerHTML         = this.generic.compute_age(this.commons.to_mmddyyyy_date(ds1.Tables[0].Rows[0].BirthDate));
                document.getElementById("lblCivilStatus").innerHTML = ds1.Tables[0].Rows[0].CivilStatus;
                document.getElementById("lblCitizenship").innerHTML = ds1.Tables[0].Rows[0].Citizenship;
                document.getElementById("lblBirthDate").innerHTML   = this.commons.to_mmddyyyy_date(ds1.Tables[0].Rows[0].BirthDate);
                document.getElementById("lblBirthPlace").innerHTML  = ds1.Tables[0].Rows[0].BirthPlace;
                document.getElementById("lblRemarks").innerHTML     = ds1.Tables[0].Rows[0].Remarks;
                document.getElementById("lblTitle").innerHTML       = ds1.Tables[0].Rows[0].Title;
                document.getElementById("lblAlias").innerHTML       = ds1.Tables[0].Rows[0].Alias;            
                document.getElementById("lblReligion").innerHTML    = ds1.Tables[0].Rows[0].Religion; 
                document.getElementById("lblBranch").innerHTML      = ds1.Tables[0].Rows[0].BranchCode;       
                document.getElementById("lblDateCreated").innerHTML      = this.commons.to_mmddyyyy_date(ds1.Tables[0].Rows[0].DateCreated);
                
                this.generic = new generic_method();
                this.generic.to_clear_null("lblCIFID~lblLastName~lblFirstName~lblMiddleName~lblGender~lblAlias~lblAge~lblCivilStatus~lblCitizenship~lblBirthDate~lblBirthPlace~lblRemarks~lblReligion~lblTitle~lblDateCreated");  
            }      
            else if (this.CIFType == 2)
            {
                document.getElementById("lblBusCIFID").innerHTML     = ds1.Tables[0].Rows[0].CIFID;
                document.getElementById("lblBusinessName").innerHTML = ds1.Tables[0].Rows[0].LastName;
                document.getElementById("lblShortName").innerHTML    = ds1.Tables[0].Rows[0].FirstName;
                document.getElementById("lblBusType").innerHTML      = ds1.Tables[0].Rows[0].MiddleName;
                document.getElementById("lblBusAlias").innerHTML     = ds1.Tables[0].Rows[0].Alias;                
                document.getElementById("lblBusRemarks").innerHTML   = ds1.Tables[0].Rows[0].Remarks;
                document.getElementById("lblBusBranch").innerHTML    = ds1.Tables[0].Rows[0].BranchCode;
                document.getElementById("lblBusDateCreated").innerHTML      = this.commons.to_mmddyyyy_date(ds1.Tables[0].Rows[0].DateCreated);
                
                this.generic = new generic_method();
                this.generic.to_clear_null("lblCIFID~lblBusinessName~lblShortName~lblBusType~lblBusAlias~lblBusRemarks~lblBusDateCreated");
            }     
        }
    }
    
    this.load_reminfo_record = function(cifid)
    {
        var ds4 = IRSWorld.CIF.CIFViewDetails.get_reminfo_record(cifid).value;
        
        if (ds4 != null && ds4.Tables[0].Rows.length > 0)
        {
            document.getElementById("lblVIPStatus").innerHTML      = ds4.Tables[0].Rows[0].VIPStatus;
            document.getElementById("lblRemitterPoints").innerHTML = ds4.Tables[0].Rows[0].RemitterPoints;
            document.getElementById("lblRiskRating").innerHTML     = ds4.Tables[0].Rows[0].RiskRating;
            document.getElementById("lblChargeType").innerHTML     = ds4.Tables[0].Rows[0].ChargeType;
            
            this.generic = new generic_method();
            this.generic.to_clear_null("lblVIPStatus~lblRemitterPoints~lblRiskRating~lblChargeType");
        }
    }
    
    this.load_grid = function(cifid, sp, grid)
    {
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = sp + cifid;
        this.grid.div_container  = document.getElementById(grid);
        this.grid.datarow_column = 0;
        this.grid.page_size      = 100;
        this.grid.open_grid_view();
        this.grid = null;
    }
}
