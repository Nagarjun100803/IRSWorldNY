
function Payment_Gateway()
{
    this.commons                  = null;
    this.SEQ_ID                   = null;
    this.RegionID                 = null;
    this.login                    = null;
    this.tran_key                 = null;
    this.version                  = null;
    this.test_request             = null;
    this.type                     = null;
    this.relay_response           = null;
    this.method                   = null;
    this.delim_data               = null;
    this.delim_char               = null;
    this.email_customer           = null;
    this.url                      = null;
    this.SecureURL                = null;
    this.VPSProtocol              = null;
    this.TxType                   = null;
    this.Vendor                   = null;
    this.Description              = null;
    this.GiftAidPayment           = null;
    this.ApplyAVSCV2              = null;
    this.Apply3DSecure            = null;
    this.AccountType              = null;
    this.NewTxType                = null;
    this.merchant_email           = null;
    this.CountryId                = null;
    this.response_key             = null;
       
    this.load = function()
    { 
        this.open_division("divBodyRegion");
        this.dropdown_region();
        this.dropdown_test_request();
        this.dropdown_type();
        this.dropdown_method();
        this.dropdown_txtype();
        this.dropdown_accounttype();
        document.getElementById("txtHiddenSeqID").value = "";
        document.getElementById("idCountry").style.display = 'none';
    }
    
    this.close_divisions = function()
    {
        document.getElementById("divBodyRegion").style.display = 'none';
        document.getElementById("divBodyAuthorizeNet").style.display = 'none';
        document.getElementById("divBodySagepay").style.display = 'none';
        document.getElementById("divBodyChaseExact").style.display = 'none';
    }
    
    this.load_country = function()
    {
        this.RegionID = document.getElementById("ddlRegion").value;
        document.getElementById("txtHiddenSeqID").value = this.RegionID;
        document.getElementById("idCountry").style.display = '';
        IRSWorld.BranchSetup.PaymentGatewayDefaults.get_country(this.RegionID, this.dropdown_country_callback)
//        switch(this.RegionID)
//        {
//            case 0,1,2:
//            {
//                document.getElementById("idCountry").style.display = 'none';
//                break;
//            }
//            case 3:
//            {
//                document.getElementById("idCountry").style.display = 'block';
//                IRSWorld.BranchSetup.PaymentGatewayDefaults.get_country(this.RegionID, this.dropdown_country_callback)
//            }            
//            default: break;
//        }
    }
    
    this.dropdown_country_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlCountry", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.open_division = function(division_id)
    {
        this.close_divisions(); document.getElementById(division_id).style.display = '';       
    }
    
    this.dropdown_region = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.getdropdown_region("ChargeGroup", this.dropdown_region_callback)
    }
    
    this.dropdown_region_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlRegion", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    this.check_region = function()
    {
        this.RegionID = document.getElementById("ddlRegion").selectedIndex;
        document.getElementById("txtHiddenSeqID").value = this.RegionID;
        this.CountryId = document.getElementById("ddlCountry").value;
        switch(this.RegionID)
        {
            case 0:
            {
                alert("Please Select a Region");
                break;
            }
            case 3:
            {
                if(this.CountryId == 8)
                {
                    var result = IRSWorld.BranchSetup.PaymentGatewayDefaults.verify_seqid(this.CountryId);
                    
                    if (result.value.Tables[0].Rows.length > 0)
                    {
                        this.SEQ_ID = result.value.Tables[0].Rows[0].SEQ_ID;
                        this.load_record_exact();
                    }
                    else
                    {
                        
                        this.insert_record_exact();
                    }
                    
                    this.open_division('divBodyChaseExact');
                    
                }
            
                else
                {
                    
                    var result = IRSWorld.BranchSetup.PaymentGatewayDefaults.verify_seqid(this.CountryId);
                    if (result != null && result.value != null)
                    {
                        if (result.value.Tables[0].Rows.length > 0)
                        {
                            this.SEQ_ID = result.value.Tables[0].Rows[0].SEQ_ID;
                            this.load_record();
                        }
                        else
                        {
                            
                            this.insert_record();
                        }
                        
                        this.open_division('divBodyAuthorizeNet');
                    }
                    else
                    {
                        alert("Please select a country.");
                    }
                }
                break; 
            }     
            case 2:
            {
                //var result = IRSWorld.BranchSetup.PaymentGatewayDefaults.check_seqid(this.RegionID);
                var result = IRSWorld.BranchSetup.PaymentGatewayDefaults.verify_seqid(this.CountryId);

                if (result.value.Tables[0].Rows.length > 0)
                {
                    this.SEQ_ID = result.value.Tables[0].Rows[0].SEQ_ID;
                    this.load_record_sagepay();
                }
                else
                {
                    this.insert_record_sagepay();
                }
                this.open_division('divBodySagepay');
                break;
            }
            
            case 1: 
            {
//                alert("Payment Gateway is unavailable for this location.");
//                break;
                //var result = IRSWorld.BranchSetup.PaymentGatewayDefaults.check_seqid(this.RegionID);
                var result = IRSWorld.BranchSetup.PaymentGatewayDefaults.verify_seqid(this.CountryId);

                if (result.value.Tables[0].Rows.length > 0)
                {
                    this.SEQ_ID = result.value.Tables[0].Rows[0].SEQ_ID;
                    this.load_record();
                }
                else
                {
                    
                    this.insert_record();
                }
                
                this.open_division('divBodyAuthorizeNet');
                break;
            }
            
            case 4: 
            {
                alert("Payment Gateway is unavailable for this location.");
                break;
            }
          
            default:break;
        }

    }
   
    this.load_record = function()
    {                     
         this.pull_server_data();  
    }
        
    this.set_branch_gateway = function()
    {   
        this.RegionID                 = document.getElementById("ddlRegion").selectedIndex;
        this.url                      = document.getElementById("txtXURL").value;
        this.login                    = document.getElementById("txtXLogin").value;
        this.tran_key                 = document.getElementById("txtXTrankey").value;
        this.version                  = document.getElementById("txtXVersion").value;
        this.test_request             = document.getElementById("ddlXTestRequest").selectedIndex;
        this.type                     = document.getElementById("ddlXType").selectedIndex;
        this.relay_response           = document.getElementById("chkXRelayResponse").checked;
        this.method                   = document.getElementById("ddlXMethod").selectedIndex;
        this.merchant_email           = document.getElementById("txtXMerchantEmail").value;
        this.delim_data               = document.getElementById("chkXDataDelimiter").checked;
        this.delim_char               = document.getElementById("txtXCharacterDelimiter").value;
        this.email_customer           = document.getElementById("chkXEmailCustomer").checked;
        this.CountryId                = document.getElementById("ddlCountry").value;
        
        if(document.getElementById("txtXCharacterDelimiter").value == "^")
        {
            alert("Character cannot be used as a delimiter");
            return false;
        } 
        
        return this.push_data_to_server();
     }
     
    this.pull_server_data = function()
    {             
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.BranchSetup.PaymentGatewayDefaults.BRANCH_GATEWAY_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.BranchSetup.PaymentGatewayDefaults.GET_BRANCH_GATEWAY_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
    
     
    this.pull_server_data_callback = function(result)
    {        
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    this.RegionID           = ds.Tables[0].Rows[0].RegionID;
                    this.url                = ds.Tables[0].Rows[0].url;
                    this.login              = ds.Tables[0].Rows[0].x_login;
                    this.tran_key           = ds.Tables[0].Rows[0].x_tran_key;
                    this.version            = ds.Tables[0].Rows[0].x_version;
                    this.test_request       = parseInt(ds.Tables[0].Rows[0].x_test_request);
                    this.type               = parseInt(ds.Tables[0].Rows[0].x_type);
                    this.relay_response     = ds.Tables[0].Rows[0].x_relay_response;
                    this.method             = parseInt(ds.Tables[0].Rows[0].x_method);
                    this.delim_data         = ds.Tables[0].Rows[0].x_delim_data;
                    this.delim_char         = ds.Tables[0].Rows[0].x_delim_char;
                    this.email_customer     = ds.Tables[0].Rows[0].x_delim_char;
                    this.merchant_email     = ds.Tables[0].Rows[0].merchant_email;
           
                   
                    document.getElementById("txtHiddenSeqID").value             = this.SEQ_ID;
                    document.getElementById("txtXURL").value                    = this.url;
                    document.getElementById("txtXLogin").value                  = this.login;
                    document.getElementById("txtXTrankey").value                = this.tran_key;
                    document.getElementById("txtXVersion").value                = this.version;
                    document.getElementById("ddlXTestRequest").selectedIndex    = this.test_request;
                    document.getElementById("ddlXType").selectedIndex           = this.type;
                    document.getElementById("chkXRelayResponse").value          = this.relay_response;       
                    document.getElementById("ddlXMethod").selectedIndex         = this.method;
                    document.getElementById("txtXMerchantEmail").value          = this.merchant_email;
                    document.getElementById("chkXDataDelimiter").value          = this.delim_data;
                    document.getElementById("txtXCharacterDelimiter").value     = this.delim_char;
                    document.getElementById("chkXEmailCustomer").checked        = this.email_customer;
                    
                    document.getElementById("chkXRelayResponse").checked = false;
                    if (document.getElementById("chkXRelayResponse").value != null && this.relay_response == "true")
                    {
                        document.getElementById("chkXRelayResponse").value = "on";
                        document.getElementById("chkXRelayResponse").checked = true;
                    }
                    
                    document.getElementById("chkXDataDelimiter").checked = false;
                    if (document.getElementById("chkXDataDelimiter").value != null && this.relay_response == "true")
                    {
                        document.getElementById("chkXDataDelimiter").value = "on";
                        document.getElementById("chkXDataDelimiter").checked = true;
                    }
                    
                    document.getElementById("chkXEmailCustomer").checked = false;
                    if (document.getElementById("chkXEmailCustomer").value != null && this.relay_response == "true")
                    {
                        document.getElementById("chkXEmailCustomer").value = "on";
                        document.getElementById("chkXEmailCustomer").checked = true;
                    }
                   
                   this.commons = new common_method();
                   this.commons.to_read_only("tblAuthorizeNetDefault", true);                     
                   this.commons.to_unlock_button("tblbuttons","btnXEdit~btnXCancel");
                   this.commons = null;
            }
        }
    }
    
    
 
    this.push_data_to_server = function()    
    {    
        var data_row = new Array();
        
        data_row[data_row.length]  = "RegionID|"          + this.RegionID; 
        data_row[data_row.length]  = "url|"               + this.url;     
        data_row[data_row.length]  = "x_login|"           + this.login;          
        data_row[data_row.length]  = "x_tran_key|"        + this.tran_key;          
        data_row[data_row.length]  = "x_version|"         + this.version;          
        data_row[data_row.length]  = "x_test_request|"    + this.test_request;        
        data_row[data_row.length]  = "x_type|"            + this.type;        
        data_row[data_row.length]  = "x_relay_response|"  + this.relay_response; 
        data_row[data_row.length]  = "x_method|"          + this.method;
        data_row[data_row.length]  = "x_delim_data|"      + this.delim_data;   
        data_row[data_row.length]  = "x_delim_char|"      + this.delim_char;
        data_row[data_row.length]  = "x_email_customer|"  + this.email_customer;              
        data_row[data_row.length]  = "merchant_email|"    + this.merchant_email;          
                     
           
        return data_row.join("^");       
    }
    
     
    this.insert_record = function()
    {     
        this.load_us_dropdown();
        this.add_data(); 
        IRSWorld.BranchSetup.PaymentGatewayDefaults.insert_record(this.insert_record_callback);
    }
      
    
    this.insert_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblAuthorizeNetDefault",false);                
            this.commons.to_unlock_button("tblbuttons","btnXSave~btnXCancel");
            this.commons = null;            
        }
    }
    
    this.update_record = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert("Record not found.");return;
        }
        
        IRSWorld.BranchSetup.PaymentGatewayDefaults.update_record(this.SEQ_ID, this.update_record_callback);
    } 
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblAuthorizeNetDefault",false);                       
            this.commons.to_unlock_button("tblbuttons","btnXSave~btnXCancel");
            this.commons = null;                                  
        }
    }
    
            
     
    this.commit_record = function()
    {
        if (confirm("Are you sure you want to save this record?"))
        {
            IRSWorld.BranchSetup.PaymentGatewayDefaults.commit_record(this.set_branch_gateway(),this.commit_record_callback);
        }

    }

    this.commit_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.BranchSetup.PaymentGatewayDefaults.BRANCH_GATEWAY_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_read_only("tblAuthorizeNetDefault",true);                          
            this.commons.to_unlock_button("tblbuttons","btnXEdit~btnXCancel"); 
            this.commons = null;    
            
            alert("Record committed successfully");
      
        }
    }
      
    this.rollback_record = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.rollback_record(this.rollback_record_callback);
        this.commons = new common_method();
        this.commons.to_clear_screen("tblAuthorizeNetDefault");
        this.commons.to_clear_screen("tblbuttons");
        this.commons.to_read_only("tblAuthorizeNetDefault",true);
        this.commons.to_unlock_button("tblbuttons","btnNew~btnSearch");
        this.commons = null;
        this.load_record();
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }                                             
    }
      
    this.clear = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblAuthorizeNetDefault");
       this.commons = null;           
    }
    
    this.add_data = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblAuthorizeNetDefault");
        this.commons.to_clear_screen("tblbuttons");
        this.commons.to_read_only("tblAuthorizeNetDefault",false);
        this.commons.to_unlock_button("tblbuttons","btnXSave~btnXCancel");
        this.commons = null;
    }
    
    this.load_us_dropdown = function()
    {
        this.dropdown_test_request();
        this.dropdown_type();
        this.dropdown_method();
    }
    
    this.dropdown_test_request = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.getdropdown_region("TestRequest", this.dropdown_test_request_callback)
    }
    
    this.dropdown_test_request_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlXTestRequest", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.dropdown_type = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.getdropdown_region("Authorized_Net_Type", this.dropdown_type_callback)
    }
    
    this.dropdown_type_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlXType", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    this.dropdown_method = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.getdropdown_region("Authorized_Net_Method", this.dropdown_method_callback)
    }
    
    this.dropdown_method_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlXMethod", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    
    this.load_sagepay_dropdown = function()
    {
        this.dropdown_txtype();
        this.dropdown_accounttype();
    }
    
    this.dropdown_txtype= function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.getdropdown_region("TxType", this.dropdown_txtype_callback)
    }
    
    this.dropdown_txtype_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlTxType", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    
    
    this.dropdown_accounttype = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.getdropdown_type("SagePayAccountType", this.dropdown_accounttype_callback)
    }
    
    this.dropdown_accounttype_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();         
            this.commons.to_load_dropdownlist("ddlAccountType", result.value, "FldValue", "FldDescription");
            this.commons = null;           
        }
    }
    this.load_record_sagepay = function()
    {                     
         this.pull_server_data_sagepay();  
    }
        
    this.set_branch_gateway_sagepay = function()
    {
        this.RegionID                 = document.getElementById("ddlRegion").selectedIndex;
        this.url                      = document.getElementById("txtSagePayURL").value;
        this.SecureURL                = document.getElementById("txt3DSecureURL").value;
        this.VPSProtocol              = document.getElementById("txtVPSProtocol").value;
        this.TxType                   = document.getElementById("ddlTxType").value;
        this.Vendor                   = document.getElementById("txtVendor").value;
        this.Description              = document.getElementById("txtDescription").value;
        this.GiftAidPayment           = document.getElementById("ChkGiftAidPayment").checked;
        this.ApplyAVSCV2              = document.getElementById("ChkApplyAVSCV2").checked;
        this.Apply3DSecure            = document.getElementById("ChkApply3DSecure").checked;
        this.AccountType              = document.getElementById("ddlAccountType").selectedIndex;
        this.merchant_email           = document.getElementById("txtMerchantEmail").value;
        this.CountryId                = document.getElementById("ddlCountry").value;   
        
        return this.push_data_to_server_sagepay();
     }
     
   
    this.pull_server_data_sagepay = function()
    {             
          
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.BranchSetup.PaymentGatewayDefaults.BRANCH_GATEWAY_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.BranchSetup.PaymentGatewayDefaults.GET_BRANCH_GATEWAY_RECORD(this.SEQ_ID,this.pull_server_data_callback_sagepay);
        
   
    }   
    
     
    this.pull_server_data_callback_sagepay = function(result)
    {       
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    this.RegionID                 = ds.Tables[0].Rows[0].RegionID;
                    this.url                      = ds.Tables[0].Rows[0].url;
                    this.SecureURL                = ds.Tables[0].Rows[0].SecureURL;
                    this.VPSProtocol              = ds.Tables[0].Rows[0].VPSProtocol;
                    this.TxType                   = parseInt(ds.Tables[0].Rows[0].TxType);
                    this.Vendor                   = ds.Tables[0].Rows[0].Vendor;
                    this.Description              = ds.Tables[0].Rows[0].Description;
                    this.GiftAidPayment           = ds.Tables[0].Rows[0].GiftAidPayment;
                    this.ApplyAVSCV2              = ds.Tables[0].Rows[0].ApplyAVSCV2;
                    this.Apply3DSecure            = ds.Tables[0].Rows[0].Apply3DSecure;
                    this.AccountType              = parseInt(ds.Tables[0].Rows[0].AccountType);   
                    this.merchant_email           = ds.Tables[0].Rows[0].merchant_email;  
           
                    document.getElementById("txtHiddenSeqID2").value            = this.SEQ_ID;
                    document.getElementById("txtSagePayURL").value              = this.url;
                    document.getElementById("txt3DSecureURL").value             = this.SecureURL;
                    document.getElementById("txtVPSProtocol").value             = this.VPSProtocol;
                    document.getElementById("ddlTxType").selectedIndex          = this.TxType;
                    document.getElementById("txtVendor").value                  = this.Vendor;
                    document.getElementById("txtDescription").value             = this.Description;
                    document.getElementById("ChkGiftAidPayment").checked        = this.GiftAidPayment;
                    document.getElementById("ChkApplyAVSCV2").checked           = this.ApplyAVSCV2;       
                    document.getElementById("ChkApply3DSecure").checked         = this.Apply3DSecure;
                    document.getElementById("ddlAccountType").selectedIndex     = this.AccountType;
                    document.getElementById("txtMerchantEmail").value           = this.merchant_email;

                    document.getElementById("ChkGiftAidPayment").checked        = false;
                    document.getElementById("ChkApplyAVSCV2").checked           = false;
                    document.getElementById("ChkApply3DSecure").checked         = false;
                    if (document.getElementById("ChkGiftAidPayment").value != null && this.GiftAidPayment == "true")
                    {
                        document.getElementById("ChkGiftAidPayment").value = "on";
                        document.getElementById("ChkGiftAidPayment").checked = "true";
                    }
                    if (document.getElementById("ChkApplyAVSCV2").value != null && this.ApplyAVSCV2 == "true")
                    {
                        document.getElementById("ChkApplyAVSCV2").value = "on";
                        document.getElementById("ChkApplyAVSCV2").checked = "true";
                    }
                    if (document.getElementById("ChkApply3DSecure").value != null && this.Apply3DSecure == "true")
                    {
                        document.getElementById("ChkApply3DSecure").value = "on";
                        document.getElementById("ChkApply3DSecure").checked = "true";
                    }
                    
                   this.commons = new common_method();
                   this.commons.to_read_only("tblSagePay", true);                     
                   this.commons.to_unlock_button("tblbuttons2","btnEdit~btnCancel");
                   this.commons = null;
            }
        }
    }
    
 
    this.push_data_to_server_sagepay = function()    
    {    
        var data_row = new Array();
        data_row[data_row.length]  = "RegionID|"            + this.RegionID;
        data_row[data_row.length]  = "url|"                 + this.url;
        data_row[data_row.length]  = "SecureURL|"           + this.SecureURL;     
        data_row[data_row.length]  = "VPSProtocol|"         + this.VPSProtocol;          
        data_row[data_row.length]  = "TxType|"              + this.TxType;          
        data_row[data_row.length]  = "Vendor|"              + this.Vendor;          
        data_row[data_row.length]  = "Description|"         + this.Description;        
        data_row[data_row.length]  = "GiftAidPayment|"      + this.GiftAidPayment;        
        data_row[data_row.length]  = "ApplyAVSCV2|"         + this.ApplyAVSCV2; 
        data_row[data_row.length]  = "Apply3DSecure|"       + this.Apply3DSecure;          
        data_row[data_row.length]  = "AccountType|"         + this.AccountType;          
        data_row[data_row.length]  = "merchant_email|"      + this.merchant_email;
        data_row[data_row.length]  = "x_country|"           + this.CountryId;
                     
        return data_row.join("^");       
    }
    
     
    this.insert_record_sagepay = function()
    {     
        this.load_sagepay_dropdown();
        this.add_data_sagepay(); 
        IRSWorld.BranchSetup.PaymentGatewayDefaults.insert_record(this.insert_record_callback_sagepay);
    }
      
    
    this.insert_record_callback_sagepay = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblSagePay",false);                
            this.commons.to_unlock_button("tblbuttons2","btnSave~btnCancel");
            this.commons = null;            
        }
    }
    
    this.update_record_sagepay = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID2").value;
        }
        
        if (this.RegionID == null || this.SEQ_ID == "")
        {
            alert("Record not found.");return;
        }
        
        IRSWorld.BranchSetup.PaymentGatewayDefaults.update_record(this.SEQ_ID, this.update_record_callback_sagepay);
    } 
    
    this.update_record_callback_sagepay = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblSagePay",false);                       
            this.commons.to_unlock_button("tblbuttons2","btnSave~btnCancel");
            this.commons = null;                                  
        }
    }
              
     
    this.commit_record_sagepay = function()
    {
			if (confirm("Are you sure you want to save this record?"))
			{
				 IRSWorld.BranchSetup.PaymentGatewayDefaults.commit_record(this.set_branch_gateway_sagepay(),this.commit_record_callback_sagepay);
			}

    }

    this.commit_record_callback_sagepay = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.BranchSetup.PaymentGatewayDefaults.BRANCH_GATEWAY_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_read_only("tblSagePay",true);                        
            this.commons.to_unlock_button("tblbuttons2","btnEdit~btnCancel"); 
            this.commons = null;    
            
            alert("Record committed successfully");
      
        }
    }
      
    this.rollback_record_sagepay = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.rollback_record(this.rollback_record_callback_sagepay);
        this.commons = new common_method();
        this.commons.to_clear_screen("tblSagePay");
        this.commons.to_clear_screen("tblbuttons2");
        this.commons.to_read_only("tblSagePay",true);
        this.commons.to_unlock_button("tblbuttons2","btnNew~btnSearch");
        this.commons = null;
        this.load_record();
    }    
    
    this.rollback_record_callback_sagepay = function(result)
    {
        var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }                                             
    }
      
    this.clear_sagepay = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblSagePay");
       this.commons = null;           
    }
    
    this.add_data_sagepay = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblSagePay");
        this.commons.to_clear_screen("tblbuttons2");
        this.commons.to_read_only("tblSagePay",false);
        this.commons.to_unlock_button("tblbuttons2","btnSave~btnCancel");
        this.commons = null;
    }
    
    this.get_type = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.NewTxType = result.value.Tables[0].Rows[0].FldDescription;
        }
    }

    this.insert_record_exact = function()
    {     
        //this.load_exact_dropdown();
        this.add_data_exact(); 
        IRSWorld.BranchSetup.PaymentGatewayDefaults.insert_record(this.insert_record_callback_exact);
    }
      
    this.insert_record_callback_exact = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message(); alert(msg.value); return;
            }
          
            this.commons = new common_method();
      
            this.commons.to_read_only("tblExactChaseDefault",false);                
            this.commons.to_unlock_button("tblbuttons3","btnExactSave~btnExactCancel");
            this.commons = null;            
        }
    }
    
    this.update_record_exact = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID3").value;
        }
        
        if (this.RegionID == null || this.SEQ_ID == "")
        {
            alert("Record not found.");return;
        }
        
        IRSWorld.BranchSetup.PaymentGatewayDefaults.update_record(this.SEQ_ID, this.update_record_callback_exact);
    } 
    
    this.update_record_callback_exact = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message();
                alert(msg.value);return;
            }
                        
            this.commons = new common_method();  
            this.commons.to_read_only("tblExactChaseDefault",false);                       
            this.commons.to_unlock_button("tblbuttons3","btnExactSave~btnExactCancel");
            this.commons = null;                                  
        }
    }
              
     
    this.commit_record_exact = function()
    {
			if (confirm("Are you sure you want to save this record?"))
			{
				 IRSWorld.BranchSetup.PaymentGatewayDefaults.commit_record(this.set_branch_exact(),this.commit_record_callback_exact);
			}

    }

    this.commit_record_callback_exact = function(result)
    {
        if (result != null && result.value != "undefined")
        {        
            if (result.value == false)
            {
                var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message(); alert(msg.value); return;
            }
            
            this.SEQ_ID = IRSWorld.BranchSetup.PaymentGatewayDefaults.BRANCH_GATEWAY_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_read_only("tblExactChaseDefault",true);                        
            this.commons.to_unlock_button("tblbuttons3","btnExactEdit~btnExactCancel"); 
            this.commons = null;    
            
            alert("Record committed successfully");
      
        }
    }
      
    this.rollback_record_exact = function()
    {
        IRSWorld.BranchSetup.PaymentGatewayDefaults.rollback_record(this.rollback_record_callback_exact);
        this.commons = new common_method();
        this.commons.to_clear_screen("tblExactChaseDefault");
        this.commons.to_clear_screen("tblbuttons3");
        this.commons.to_read_only("tblExactChaseDefault",true);
        this.commons.to_unlock_button("tblbuttons3","btnExactNew~btnExactSearch");
        this.commons = null;
        this.load_record();
    }    
    
    this.rollback_record_callback_exact = function(result)
    {
        var msg = IRSWorld.BranchSetup.PaymentGatewayDefaults.read_message();
        
        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }                                             
    }
      
    this.clear_exact = function()
    {
       this.commons = new common_method();
       this.commons.to_clear_screen("tblExactChaseDefault");
       this.commons = null;           
    }
    
    this.add_data_exact = function()
    {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblExactChaseDefault");
        this.commons.to_clear_screen("tblbuttons3");
        this.commons.to_read_only("tblExactChaseDefault",false);
        this.commons.to_unlock_button("tblbuttons3","btnExactSave~btnExactCancel");
        this.commons = null;
    }
    
    this.load_record_exact = function()
    {                     
         this.pull_server_data_exact();  
    }

    this.pull_server_data_exact = function()
    {             
        if (this.SEQ_ID == null)
        {            
            this.SEQ_ID = IRSWorld.BranchSetup.PaymentGatewayDefaults.BRANCH_GATEWAY_SEQ_ID().value;
        }
        if (this.SEQ_ID == null ||this.SEQ_ID == '' || this.SEQ_ID == 'undefined')
        {
            return;
        }                      
        IRSWorld.BranchSetup.PaymentGatewayDefaults.GET_BRANCH_GATEWAY_RECORD(this.SEQ_ID,this.pull_server_data_callback_exact);
    }   
    
     
    this.pull_server_data_callback_exact = function(result)
    {        
        if (result != null && result.value != null)
        {
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {
                    this.RegionID           = ds.Tables[0].Rows[0].RegionID;
                    this.url                = ds.Tables[0].Rows[0].url;
                    this.login              = ds.Tables[0].Rows[0].x_login;
                    this.tran_key           = ds.Tables[0].Rows[0].x_tran_key;
                    this.response_key       = ds.Tables[0].Rows[0].x_response_key; 
                    this.CountryId          = ds.Tables[0].Rows[0].x_country;     
                    this.Description        = ds.Tables[0].Rows[0].Description; 
                   
                    document.getElementById("txtHiddenSeqID3").value            = this.SEQ_ID;
                    document.getElementById("txtExactURL").value                = this.url;
                    document.getElementById("txtExactLogin").value              = this.login;
                    document.getElementById("txtExactTranKey").value            = this.tran_key;
                    document.getElementById("txtExactResponseKey").value        = this.response_key;
                    document.getElementById("txtExactDescription").value        = this.Description;
                    
                  
                   
                   this.commons = new common_method();
                   this.commons.to_read_only("tblExactChaseDefaultChaseDefault", true);                     
                   this.commons.to_unlock_button("tblbuttons3","btnExactEdit~btnExactCancel");
                   this.commons = null;
            }
        }
    }
    
    this.push_data_to_server_exact = function()    
    {    
        var data_row = new Array();
        
        data_row[data_row.length]  = "RegionID|"          + this.RegionID; 
        data_row[data_row.length]  = "url|"               + this.url;     
        data_row[data_row.length]  = "x_login|"           + this.login;          
        data_row[data_row.length]  = "x_tran_key|"        + this.tran_key;          
        data_row[data_row.length]  = "x_response_key|"    + this.response_key;          
        data_row[data_row.length]  = "x_country|"         + this.CountryId;
        data_row[data_row.length]  = "Description|"       + this.Description;
           
        return data_row.join("^");       
    }
    
    this.set_branch_exact = function()
    {   
        this.RegionID                 = document.getElementById("ddlRegion").selectedIndex;
        this.url                      = document.getElementById("txtExactURL").value;
        this.login                    = document.getElementById("txtExactLogin").value;
        this.tran_key                 = document.getElementById("txtExactTranKey").value;
        this.response_key             = document.getElementById("txtExactResponseKey").value;
        this.CountryId                = document.getElementById("ddlCountry").value;
        this.Description              = document.getElementById("txtExactDescription").value;
        
        return this.push_data_to_server_exact();
     }
     

}
