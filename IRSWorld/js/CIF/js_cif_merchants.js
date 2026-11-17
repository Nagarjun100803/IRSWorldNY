function cif_merchants()
{
    this.commons      = null;
    this.generic      = null;
    this.SEQ_ID       = null;
    this.CIFID        = null;
    this.ProductCode  = null;
    this.AccountSeqId = null;
    this.MerchantCode = null;
    this.ImplementationCode  = null;
    this.MerchantProductCode = null;
    this.MerchantType = null;
    var iMerchants    = this;
    
    this.load_grid_viewer = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblMerchantOtherInfoEntry");
        this.commons = null;
        
        this.grid = new grid_viewer();
        this.session_end         = true;
        this.grid.sql_source     = "sp_cif_grid_merchants_info~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFMERCHANTSGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();
        
        this.grid.sql_source     = "sp_cif_grid_merchants_info~" + this.CIFID;
        this.grid.div_container  = document.getElementById("CIFMERCHANTSGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    
    this.cancel_insert = function()
    {
	    this.commons = new common_method();
	    this.commons.to_clear_screen("tblMerchantOtherInfoEntry");
	    this.commons.to_read_only("tblMerchantOtherInfoEntry", true);
	    this.commons.to_unlock_button("tblbuttons","btnAdd");
    }
    
    this.load_record = function()
    {
        this.commons = new common_method();     
        this.commons.to_clear_screen("tblMerchantOtherInfoEntry");
        this.commons.to_read_only("tblMerchantOtherInfoEntry", true);
        this.commons = null;
        
        this.pull_server_data();
    }
    
    this.set_implementation_type = function()
    {
        var product_code = document.getElementById("ddProductCodeMer").value;
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
            case '27': implementation_type = 1; break;    case '255':implementation_type = 1;  break;  
            default: break;
        }
        this.set_dropdown_mer_imptype(implementation_type);
    }
    
    this.set_dropdown_mer_imptype = function(implementation_type)
    {
        this.commons = new common_method();
        this.commons.to_set_dropdownlist_index("ddMerchantImplementationType", implementation_type);
        this.commons = null; 
    }
    
    this.set_name_module = function(module_name)
    {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }
    
    this.set_record = function()
    { 
        var ds = null;
        ds = IRSWorld.CIF.cifmain.get_merchant_code(document.getElementById("txtHiddenMasterSeqId").value).value;
        
		
		//RFC-31620
        // this.MerchantCode        = ds.Tables[0].Rows[0].FirstName;
        this.AccountSeqId        = document.getElementById("ddMerchantAccountNo").value; 
        this.ProductCode         = document.getElementById("ddProductCodeMer").value; 
        this.MerchantProductCode = document.getElementById("ddMerchantProductName").value; 
        this.ImplementationCode  = document.getElementById("ddMerchantImplementationType").value; 
        this.MerchantType        = document.getElementById("ddMerchantType").value; 
        
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
        
        IRSWorld.CIF.cifmain.GET_MERCHANTS_RECORD(this.SEQ_ID,this.pull_server_data_callback);
    }   
     
    this.pull_server_data_callback = function(result)
    {                     
        if (result != null && result.value != null)
        {            
            var ds = result.value;
                        
            if (ds.Tables[0].Rows.length > 0)
            {        
                this.commons = new common_method();
                this.commons.to_clear_screen("tblMerchantOtherInfoEntry");
                this.commons.to_read_only("tblMerchantOtherInfoEntry", true);
				

            
                iMerchants.SEQ_ID              = ds.Tables[0].Rows[0].SEQ_ID;
                iMerchants.MerchantProductCode = ds.Tables[0].Rows[0].MerchantProductCode;
                iMerchants.AccountSeqId        = ds.Tables[0].Rows[0].AccountSeqId;
                iMerchants.ImplementationCode  = ds.Tables[0].Rows[0].ImplementationCode; 
                iMerchants.ProductCode         = ds.Tables[0].Rows[0].ProductCode; 
                iMerchants.MerchantType        = ds.Tables[0].Rows[0].MerchantType; 
				
			    /* RFC-31620 */
                iMerchants.MerchantCode = ds.Tables[0].Rows[0].MerchantCode;
                //ds = IRSWorld.CIF.cifmain.get_merchant_code(document.getElementById("txtHiddenMasterSeqId").value).value;
                //iMerchants.MerchantCode = ds.Tables[0].Rows[0].FirstName;  
                                          
                document.getElementById("txtHiddenSeqId").value         = iMerchants.SEQ_ID; 
                document.getElementById("txtHiddenCIFID").value         = iMerchants.CIFID; 
                document.getElementById("ddProductCodeMer").value       = iMerchants.ProductCode; 
                document.getElementById("ddMerchantProductName").value  = iMerchants.MerchantProductCode; 
                document.getElementById("ddMerchantAccountNo").value    = iMerchants.AccountSeqId; 
                document.getElementById("ddMerchantImplementationType").value  = iMerchants.ImplementationCode;
                document.getElementById("ddMerchantType").value    = iMerchants.MerchantType; 
            }
        }          
    }
        
    this.push_data_to_server = function()
    {
        var data_row;
        
        data_row = new Array();
        data_row[data_row.length]  = "CIFID~"               + this.CIFID;
        data_row[data_row.length]  = "ProductCode~"         + this.ProductCode;
        data_row[data_row.length]  = "MerchantProductCode~" + this.MerchantProductCode;
        data_row[data_row.length]  = "AccountSeqId~"        + this.AccountSeqId;
        data_row[data_row.length]  = "MerchantCode~"        + this.MerchantCode;        
        data_row[data_row.length]  = "ImplementationCode~"  + this.ImplementationCode;
        data_row[data_row.length]  = "MerchantType~"        + this.MerchantType;
        data_row[data_row.length]  = "Deleted~false";
        
        return data_row.join("|");        
    }
    
    this.insert_record = function()
    {
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
            this.commons.to_clear_screen("tblMerchantOtherInfoEntry");  
            this.commons.to_read_only("tblMerchantOtherInfoEntry", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;
        }
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
            this.commons.to_read_only("tblMerchantOtherInfoEntry", false);
            this.commons.to_unlock_button("tblbuttons","btnSave~btnCancel");
            this.commons = null;            
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
    }
    
    this.commit_record_callback = function(result)
    {
	    iMerchants.load_record();
        
        if (typeof(iMerchants.load_grid_viewer) != 'undefined')
        {
            iMerchants.load_grid_viewer();
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
        var msg = IRSWorld.CIF.cifmain.read_message();
        
        if (msg != null && msg.value != "undefined" && msg.value.length > 0)
        {
           alert(msg.value);
        }
        
        this.commons = new common_method();
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete");                                    
        this.commons = null;
    }
    
    this.validate_input = function()
    {
        this.commons     = new common_method();        
        this.error_input = '';
        var foc_elem     = '';
        
        if (document.getElementById("ddMerchantType").value == 'null')
        { 
            this.error_input += "Merchant type is required.";
            foc_elem = 'ddMerchantType';     
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
}