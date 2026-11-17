
function accountingentries()
{
    this.commons                        = null;
    this.grid                           = null

    var SEQ_ID                          = null;
    var negbranchcode                   = null;
    var locationcode                    = null;
    var TranxSource                     = null;
    var ReceiptMode                     = null;
    var TranxCurrency                   = null;
    var ImplementationCurrency          = null;
    var TranxGLCode                     = null;
    var ServiceChargeGLCode             = null;
    var ImplementationGLCode            = null;
    var OPEGLCode                       = null;
    var DocStampGLCodes                 = null;
    var LocalCableChargesGLCode         = null;
    var LocalServiceFeeGLCode           = null;
    var PRXGLCode                       = null;
    var MISCode                         = null;
    var UserId                          = null;
    this.BranchCode                     = null;
    this.Role                           = null;
    
    this.check_gl_codes = function()
    {
        if (event.keyCode >= 48 && event.keyCode <= 57) 
        {
            if (event.srcElement.value.length > 8)
            {
                event.returnValue = false;
                alert("GL codes maximum character cannot be greater than nine (9)");
            }
            else
            {
                event.returnValue = true;
            }            
        }
        else
        {
            event.returnValue=false;
            alert("Please input only numbers");
        }
    }
    
    this.getBranchCode = function()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.getBranchCode();
    }
    
    this.getRole = function()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.getRole();
    }    
    
    this.load_grid_viewer = function()
    {
        this.commons    = new common_method();
        this.commons.to_read_only("tblaccountingentries", false);
        this.commons.to_unlock_button("tblbuttons","btnAdd~btnEdit~btnDelete~btnClose");
        this.commons    = null;  
    }

    this.load_record = function()
    {
        this.pull_server_data()
    }   
    
    this.pull_server_data = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.GEFUUpload.GEFUMaintenance.get_seq_id().value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == '' || this.SEQ_ID =='undefined')
        {
            return;
        }
        IRSWorld.GEFUUpload.GEFUMaintenance.GET_ACCOUNTINGENTRIES_RECORD(this.SEQ_ID, this.pull_server_data_callback);
    }
    
    this.pull_server_data_callback = function(result)
    {
        if (result!= null && result.value != null)
        {
            var ds = result.value;
            
            if (ds.Tables[0].Rows.length > 0)
            {
                this.SEQ_ID                         = ds.Tables[0].Rows[0].SEQ_ID;
                this.negbranchcode                  = ds.Tables[0].Rows[0].negbranchcode;
                this.locationcode                   = ds.Tables[0].Rows[0].locationcode;
                this.TranxSource                    = ds.Tables[0].Rows[0].TranxSource;
                this.ReceiptMode                    = ds.Tables[0].Rows[0].ReceiptMode;
                this.TranxCurrency                  = ds.Tables[0].Rows[0].TranxCurrency;                                                                
                this.ImplementationCurrency         = ds.Tables[0].Rows[0].ImplementationCurrency;
                this.TranxGLCode                    = ds.Tables[0].Rows[0].TranxGLCode;
                this.ServiceChargeGLCode            = ds.Tables[0].Rows[0].ServiceChargeGLCode;
                this.ImplementationGLCode           = ds.Tables[0].Rows[0].ImplementationGLCode;
                this.OPEGLCode                      = ds.Tables[0].Rows[0].OPEGLCode;
                this.DocStampGLCodes                = ds.Tables[0].Rows[0].DocStampGLCodes;
                this.LocalCableChargesGLCode        = ds.Tables[0].Rows[0].LocalCableChargesGLCode;
                this.LocalServiceFeeGLCode          = ds.Tables[0].Rows[0].LocalServiceFeeGLCode; 
                this.PRXGLCode                      = ds.Tables[0].Rows[0].PRXGLCode; 
                this.MISCode                        = ds.Tables[0].Rows[0].MISCode;
                
                document.getElementById("txtHidden_Accountingentries_SEQ_ID").value = this.SEQ_ID;
                document.getElementById("ddnegbranchcode").value                    = this.negbranchcode;
                document.getElementById("ddlocationcode").value                     = this.locationcode;
                document.getElementById("ddTranxSource").value                      = this.TranxSource;
                document.getElementById("ddReceiptMode").value                      = this.ReceiptMode;
                document.getElementById("ddTranxCurrency").value                    = this.TranxCurrency;
                document.getElementById("ddImplementationCurrency").value           = this.ImplementationCurrency;
                document.getElementById("txtTranxGLCode").value                     = this.TranxGLCode;
                document.getElementById("txtServiceChargeGLCode").value             = this.ServiceChargeGLCode;
                document.getElementById("txtImplementationGLCode").value            = this.ImplementationGLCode;
                document.getElementById("txtOPEGLCode").value                       = this.OPEGLCode;
                document.getElementById("txtDocStampGLCodes").value                 = this.DocStampGLCodes;
                document.getElementById("txtLocalCableChargesGLCode").value         = this.LocalCableChargesGLCode;
                document.getElementById("txtLocalServiceFeeGLCode").value           = this.LocalServiceFeeGLCode;       
                document.getElementById("txtPRXGLCode").value                       = this.PRXGLCode;  
                document.getElementById("txtMISCode").value                         = this.MISCode;   
                document.getElementById('btnEditAccountingEntries').disabled        = false;
            }
        }        
    }
    
    this.check_negbranchcode = function ()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.check_negbranchcode(this.BranchCode.value);
    }

    this.check_fc_branch_code = function ()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.check_fc_branch_code(this.BranchCode.value);
    }    
    
    this.check_tranxsource_sublookup = function ()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.check_tranxsource_sublookup(this.BranchCode.value);
    }
        
    this.check_receiptmode_sublookup = function ()
    {
        return IRSWorld.GEFUUpload.GEFUMaintenance.check_receiptmode_sublookup(this.BranchCode.value);
    }
        
    this.ActiveModule = function(divName)
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.ActiveModule(divName);
    }
    
    this.insert_record = function()
    {    
        IRSWorld.GEFUUpload.GEFUMaintenance.insert_record(this.insert_record_callback);
    }
    
    this.insert_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message(); alert(msg.value); return;
            }

            this.commons = new common_method();              
            this.commons.to_clear_screen("tblAccountingEntries");  
            this.commons.to_read_only("tblAccountingEntries", false);  
            this.commons.to_unlock_button("tblAccountingEntriesButtons","btnSaveAccountingEntries~btnCancelAccountingEntries");
            this.commons = null;
            
            if (document.getElementById("ddnegbranchcode").length -1 == 1)
            {
                document.getElementById("ddnegbranchcode").selectedIndex = 1;
                document.getElementById("ddnegbranchcode").disabled = true;
            }
            else
            {
                document.getElementById("ddnegbranchcode").selectedIndex = 0;
                document.getElementById("ddnegbranchcode").disabled = false;            
            }
                  
            if (document.getElementById("ddlocationcode").length -1 == 1)
            {
                document.getElementById("ddlocationcode").selectedIndex = 1;
                document.getElementById("ddlocationcode").disabled = true;
            }
            else
            {
                document.getElementById("ddlocationcode").selectedIndex = 0;
                document.getElementById("ddlocationcode").disabled = false;
            }    
            
            if (trim(document.getElementById("ddnegbranchcode").value) == "3725")
            {            
                document.getElementById("ddReceiptMode").disabled = true;
                document.getElementById("ddReceiptMode").value = "not required";
            }    
        }        
    }
    
    this.update_record = function ()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHidden_Accountingentries_SEQ_ID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert ("Record not found, kindly select one"); return;
        }
        
        IRSWorld.GEFUUpload.GEFUMaintenance.update_record(this.SEQ_ID, this.update_record_callback);
    }   
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();
                alert(msg.value);
                return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblAccountingEntries", false);  
            this.commons.to_unlock_button("tblAccountingEntriesButtons","btnSaveAccountingEntries~btnCancelAccountingEntries");
            this.commons = null;
        }
    }
    
    this.delete_record = function()
    {     
        if (confirm("Delete Record"))
        {
            if (this.SEQ_ID == null)
            {
                this.SEQ_ID = document.getElementById("txtHidden_Accountingentries_SEQ_ID").value;
            }
            
            if (this.SEQ_ID == null || this.SEQ_ID == "")
            {
                alert("RECORD NOT FOUND.");return;
            }
            
            IRSWorld.GEFUUpload.GEFUMaintenance.delete_record(this.SEQ_ID, this.delete_record_callback);    
        }
        else
        {
            this.commons = new common_method();
            this.commons.to_clear_screen("tblAccountingEntries");  
            this.commons.to_read_only("tblAccountingEntries",true);
            this.commons.to_unlock_button("tblAccountingEntriesButtons","btnAddAccountingEntries~");
            this.commons = null;        
        }
    }     
        
    this.delete_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();
                alert(msg.value);return;
            } 
            
            alert("RECORD DELETED SUCCESSFULLY");   
            this.commons = new common_method();  
            this.commons.to_read_only("tblAccountingEntries", false);  
            this.commons.to_unlock_button("tblAccountingEntriesButtons","btnAddAccountingEntries~");
            this.commons = null;
            
            this.BranchCode = IRSWorld.GEFUUpload.GEFUMaintenance.getBranchCode();
            
            this.accountingentries_grid = new grid_viewer();
            this.accountingentries_grid.sql_source  = "sp_accountingentries_grid~" + this.BranchCode.value;
            this.accountingentries_grid.div_container = document.getElementById("divtbl_AccountingEntries");
            this.accountingentries_grid.datarow_column = 0;
            this.accountingentries_grid.open_grid_view();
            this.accountingentries_grid = null;                 
        }
    }     
    
    this.commit_record = function ()
    {
        if (trim(document.getElementById("ddTranxSource").value) == "null")
        {
            alert ("Transaction Source is required");
            document.getElementById( "ddTranxSource").focus();
            return;
        }
        
        if (trim(document.getElementById("ddnegbranchcode").value) != "3725")
        {
            if (trim(document.getElementById("ddReceiptMode").value) == "null")
            {
                alert ("Receipt Mode is required");
                document.getElementById( "ddReceiptMode").focus();
                return;
            }
        }        
        
        if (trim(document.getElementById("ddTranxCurrency").value) == "null")
        {
            alert ("Remitting Currency is required");
            document.getElementById( "ddTranxCurrency").focus();
            return;
        }
        
        if (trim(document.getElementById("ddImplementationCurrency").value) == "null")
        {
            alert ("Account Currency is required");
            document.getElementById( "ddImplementationCurrency" ).focus();
            return;
        }
        
        if (trim(document.getElementById("txtTranxGLCode").value) == "")
        {
            alert ("Transaction's GL Code is required");
            document.getElementById( "txtTranxGLCode" ).focus();
            return;
        }
       

        if (trim(document.getElementById("txtImplementationGLCode").value) == "")
        {
            alert ("Implementation's GL Code is required");
            document.getElementById( "txtImplementationGLCode" ).focus();
            return;
        }
       
        
        if (trim(document.getElementById("txtServiceChargeGLCode").value) == "")
        {
            alert ("Service Charge's GL Code is required");
            document.getElementById( "txtServiceChargeGLCode" ).focus();
            return;
        }
        
        
        if (trim(document.getElementById("txtPRXGLCode").value) == "")
        {
            alert ("PRX's GL Code is required");
            document.getElementById( "txtPRXGLCode" ).focus();
            return;
        } 
        

        if (trim(document.getElementById("txtDocStampGLCodes").value) != "")
        {
            if (trim(document.getElementById("txtDocStampGLCodes").value).length != 9)
            {
                alert ("Doc Stamp GL Code GL Code must consist of 9 characters");
                return;
            }
        }

        if (trim(document.getElementById("txtOPEGLCode").value) != "")
        {
            if (trim(document.getElementById("txtOPEGLCode").value).length != 9)
            {
                alert ("OPE's GL Code GL Code must consist of 9 characters");
                return;
            }
        }
        
        if (trim(document.getElementById("txtLocalServiceFeeGLCode").value) != "")
        {
            if (trim(document.getElementById("txtLocalServiceFeeGLCode").value).length != 9)
            {
                alert ("Local Service Fee's GL Code GL Code must consist of 9 characters");
                return;
            }
        }

        if (trim(document.getElementById("txtLocalCableChargesGLCode").value) != "")
        {
            if (trim(document.getElementById("txtLocalCableChargesGLCode").value).length != 9)
            {
                alert ("Local Cable Charges' GL Code GL Code must consist of 9 characters");
                return;
            }
        }

        IRSWorld.GEFUUpload.GEFUMaintenance.commit_record(this.set_AccountingEntries(), this.commit_record_callback);       
    }
    
    this.commit_record_callback = function(result)
    {        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();
                alert(msg.value);
                return;
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");
            this.commons = new common_method();
            this.commons.to_clear_screen("tblAccountingEntries");  
            this.commons.to_read_only("tblAccountingEntries",true);
            this.commons.to_unlock_button("tblAccountingEntriesButtons","btnAddAccountingEntries~");
            this.commons = null;

            this.BranchCode = IRSWorld.GEFUUpload.GEFUMaintenance.getBranchCode();

            this.accountingentries_grid = new grid_viewer();
            this.accountingentries_grid.sql_source  = "sp_accountingentries_grid~" + this.BranchCode.value;
            this.accountingentries_grid.div_container = document.getElementById("divtbl_AccountingEntries");
            this.accountingentries_grid.datarow_column = 0;
            this.accountingentries_grid.open_grid_view();
            this.accountingentries_grid = null;
        }
    }
    
    this.rollback_record = function()
    {            
        IRSWorld.GEFUUpload.GEFUMaintenance.rollback_record(this.rollback_record_callback);  
        this.SEQ_ID = null;
        document.getElementById("txtHidden_Accountingentries_SEQ_ID").value = "";     
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GEFUUpload.GEFUMaintenance.read_message();

        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }
        this.commons = new common_method();        
        this.commons.to_read_only("tblAccountingEntries",true);
        this.commons.to_clear_screen("tblAccountingEntries");   
        this.commons.to_unlock_button("tblAccountingEntriesButtons","btnAddAccountingEntries~");
        this.commons = null;
        return;
    }
    
    this.set_AccountingEntries = function ()
    {
        negbranchcode                   = document.getElementById("ddnegbranchcode").value;
        locationcode                    = document.getElementById("ddlocationcode").value;
        TranxSource                     = document.getElementById("ddTranxSource").value;
        if (trim(document.getElementById("ddnegbranchcode").value) == "3725")
        {
            ReceiptMode                 = 0;
        }
        else        
        {
            ReceiptMode                 = document.getElementById("ddReceiptMode").value;
        }
        
        TranxCurrency                   = document.getElementById("ddTranxCurrency").value;
        ImplementationCurrency          = document.getElementById("ddImplementationCurrency").value;
        TranxGLCode                     = document.getElementById("txtTranxGLCode").value;
        ServiceChargeGLCode             = document.getElementById("txtServiceChargeGLCode").value;
        ImplementationGLCode            = document.getElementById("txtImplementationGLCode").value;
        OPEGLCode                       = document.getElementById("txtOPEGLCode").value;
        DocStampGLCodes                 = document.getElementById("txtDocStampGLCodes").value;
        LocalCableChargesGLCode         = document.getElementById("txtLocalCableChargesGLCode").value;
        LocalServiceFeeGLCode           = document.getElementById("txtLocalServiceFeeGLCode").value;
        PRXGLCode                       = document.getElementById("txtPRXGLCode").value;
        MISCode                         = document.getElementById("txtMISCode").value; 
        UserId                          = "UUUIII";
        
        return this.push_data_to_server();
    }
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]   = "negbranchcode~"                  + negbranchcode;
        data_row[data_row.length]   = "locationcode~"                   + locationcode;
        data_row[data_row.length]   = "TranxSource~"                    + TranxSource;
        data_row[data_row.length]   = "ReceiptMode~"                    + ReceiptMode;
        data_row[data_row.length]   = "TranxCurrency~"                  + TranxCurrency;
        data_row[data_row.length]   = "ImplementationCurrency~"         + ImplementationCurrency;
        data_row[data_row.length]   = "TranxGLCode~"                    + TranxGLCode;
        data_row[data_row.length]   = "ServiceChargeGLCode~"            + ServiceChargeGLCode;
        data_row[data_row.length]   = "ImplementationGLCode~"           + ImplementationGLCode;
        data_row[data_row.length]   = "OPEGLCode~"                      + OPEGLCode;
        data_row[data_row.length]   = "DocStampGLCodes~"                + DocStampGLCodes;   
        data_row[data_row.length]   = "LocalCableChargesGLCode~"        + LocalCableChargesGLCode;    
        data_row[data_row.length]   = "LocalServiceFeeGLCode~"          + LocalServiceFeeGLCode;   
        data_row[data_row.length]   = "PRXGLCode~"                      + PRXGLCode;   
        data_row[data_row.length]   = "MISCode~"                        + MISCode;
        data_row[data_row.length]   = "UserId~"                         + UserId;  
        
        return data_row.join("$");
    }
    
    this.load_accountingentries_grid_viewer = function()
    {        
        this.accountingentries_grid = new grid_viewer();
        if (this.BranchCode.value != "1074")
        {
            this.accountingentries_grid.sql_source  = "sp_accountingentries_grid~" + this.BranchCode.value;
        }
        else
        {
            this.accountingentries_grid.sql_source  = "sp_accountingentries_grid_admin~";
        }
        
        
        this.accountingentries_grid.div_container = document.getElementById("divtbl_AccountingEntries");
        this.accountingentries_grid.datarow_column = 0;
        this.accountingentries_grid.open_grid_view();
        this.grid = null;
    }
    
    this.do_ajaxgridviewer = function (parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                document.getElementById("txtHidden_Accountingentries_SEQ_ID").value = command_param[1];

                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    if (this.Role.value != "1")
                    {
                        document.getElementById("btnEditAccountingEntries").disabled = "disable"; 
                        document.getElementById("btnDeleteAccountingEntries").disabled = "disable"; 
                    }
                    else
                    {
                        document.getElementById("btnEditAccountingEntries").disabled = ""; 
                        document.getElementById("btnDeleteAccountingEntries").disabled = ""; 
                    }                    
                    
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
   
    this.process_grid_viewer = function (parameter)
    {
        this.loadergrid = new grid_viewer();
        this.loadergrid.sql_source     = "sp_accountingentries_grid~" + this.BranchCode.value;
        this.loadergrid.div_container  = document.getElementById("divtbl_AccountingEntries");
        this.loadergrid.datarow_column = 0;
        this.loadergrid.process_ajax(parameter);
        this.loadergrid = null;
    }
    
    this.populate_dropdownboxes = function ()
    {
            IRSWorld.GEFUUpload.GEFUMaintenance.getnegbranchcode(this.BranchCode.value, this.dropdown_negbranchcode_callback);
            IRSWorld.GEFUUpload.GEFUMaintenance.getlocationcode(this.BranchCode.value, this.dropdown_locationcode_callback);
            IRSWorld.GEFUUpload.GEFUMaintenance.getTranxSource_sublookup_accnt(this.BranchCode.value, this.dropdown_TranxSource_callback);
            IRSWorld.GEFUUpload.GEFUMaintenance.getReceiptMode_sublookup(this.BranchCode.value, this.dropdown_ReceiptMode_callback);
            IRSWorld.GEFUUpload.GEFUMaintenance.getSourceCcy(this.BranchCode.value, this.dropdown_TranxCurrency_callback);
            IRSWorld.GEFUUpload.GEFUMaintenance.getTargetCcy(this.BranchCode.value, this.dropdown_ImplementationCurrency_callback);
                        
            IRSWorld.GEFUUpload.GEFUMaintenance.getAccNegbranchCode(this.dropdown_AccNegbranchCode_callback);
//            IRSWorld.GEFUUpload.GEFUMaintenance.getAccLocationCode(this.NegBranchCode.value, this.dropdown_AccLocationCode_callback);
//            IRSWorld.GEFUUpload.GEFUMaintenance.getAccTranxSource(this.NegBranchCode.value, this.LocationCode.value, this.dropdown_AccTranxSource_callback);
//            IRSWorld.GEFUUpload.GEFUMaintenance.getAccReceiptMode(this.NegBranchCode.value, this.LocationCode.value, this.dropdown_AccReceiptMode_callback);
//            IRSWorld.GEFUUpload.GEFUMaintenance.getAccSourceCcy(this.NegBranchCode.value, this.LocationCode.value, this.dropdown_AccSourceCcy_callback);
//            IRSWorld.GEFUUpload.GEFUMaintenance.getAccTargetCcy(this.NegBranchCode.value, this.LocationCode.value, this.dropdown_AccTargetCcy_callback);
    }
    
    this.dropdown_AccTargetCcy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddSearchAccountCurrency", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }

    this.dropdown_AccSourceCcy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddSearchRemittingCurrency", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }

    this.dropdown_AccReceiptMode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddSearchReceiptMode", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_AccTranxSource_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddSearchTransactionSource", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }
    
    
    this.dropdown_AccLocationCode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddSearchLocationCode", result.value, "locationcode", "locationcode");
            this.commons = null;
        }
    }
    
    this.dropdown_AccNegbranchCode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddSearchNegBranchCode", result.value, "negbranchcode", "negbranchcode");
            this.commons = null;
        }
    }
    
    this.get_locationcode_by_negbranchcode = function (negbranchcode)
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.getlocationcode_negbranchcode(negbranchcode, this.dropdown_locationcode_autoselect_callback);
    }
    
    this.get_filtered_accountingentries = function ()
    {
        IRSWorld.GEFUUpload.GEFUMaintenance.get_filtered_accountingentries(negbranchcode, this.dropdown_locationcode_autoselect_callback);
    }
    
    this.reload_accountingentries = function(result)
    {
        this.accountingentries_grid = new grid_viewer();
        this.accountingentries_grid.sql_source  = "sp_accountingentries_grid~" + this.BranchCode.value;
        this.accountingentries_grid.div_container = document.getElementById("divtbl_AccountingEntries");
        this.accountingentries_grid.datarow_column = 0;
        this.accountingentries_grid.open_grid_view();
        this.accountingentries_grid = null;
    }
    this.get_branch_sub_lookups = function(branchcode)
    {        
        IRSWorld.GEFUUpload.GEFUMaintenance.getTranxSource_sublookup_accnt(branchcode, this.dropdown_TranxSource_callback);
        IRSWorld.GEFUUpload.GEFUMaintenance.getReceiptMode_sublookup(branchcode, this.dropdown_ReceiptMode_callback);
        IRSWorld.GEFUUpload.GEFUMaintenance.getTranxCurrency_sublookup(branchcode, this.dropdown_TranxCurrency_callback);
        IRSWorld.GEFUUpload.GEFUMaintenance.getImplementationCurrency_sublookup(branchcode, this.dropdown_ImplementationCurrency_callback);
    }
    
    this.dropdown_negbranchcode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddnegbranchcode", result.value, "negbranchcode", "negbranchcode");
            this.commons = null;
        }
    }
    
    this.dropdown_locationcode_autoselect_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlocationcode", result.value, "branchcode", "branchname");
            this.commons = null;
        }
        
        if (document.getElementById("ddlocationcode").length -1 == 1)
        {
            document.getElementById("ddlocationcode").selectedIndex = 1;
            document.getElementById("ddlocationcode").disabled = true;
        }
        else
        {
            document.getElementById("ddlocationcode").selectedIndex = 0;
            document.getElementById("ddlocationcode").disabled = false;            
        }
        
    }

    this.dropdown_locationcode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlocationcode", result.value, "branchcode", "branchname");
            this.commons = null;
        }
    }
    
    this.dropdown_TranxSource_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddTranxSource", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_ReceiptMode_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddReceiptMode", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_TranxCurrency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddTranxCurrency", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_ImplementationCurrency_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddImplementationCurrency", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }    
        
    this.check_alpha = function()
    {
        if (event.keyCode == 32 || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode >= 48 && event.keyCode <= 57) ||  (event.keyCode >= 97 && event.keyCode <= 122)) 
        {   
            event.returnValue = true;
        }
        else
        {
            event.returnValue = false;
            alert("Please input valid characters");
        }
    } 
}