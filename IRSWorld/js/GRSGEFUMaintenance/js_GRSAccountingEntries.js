
function GRSAccountingEntries()
{
    var SEQ_ID              = null;
    var TranxCcyCode        = null;
    var AcctCcyCode         = null;
    var ImplementationType  = null;
    var GLAcctCode          = null;
    var GLDesc              = null;
    var DebitCreditTag      = null;
    var TranxSource         = null;
    var RecordID            = null;  
    var NSVS                = null;
    var FCCFCR              = null;
    var FCDURBU             = null;  
    var HandlingType        = null;
   
    this.commit_record = function ()
    {
//        if (trim(document.getElementById("ddTranxCcyCode").value) == "null")
//        {
//            alert ("Remitting Currency is required");
//            document.getElementById( "ddTranxCcyCode" ).focus();
//            return;
//        }
//        
//        if (trim(document.getElementById("ddAcctCcyCode").value) == "null")
//        {
//            alert ("Account Currency is required");
//            document.getElementById( "ddAcctCcyCode" ).focus();
//            return;
//        }
//        
//        if (trim(document.getElementById("ddProductCode").value) == "null")
//        {
//            alert ("Implementation Type is required");
//            document.getElementById( "ddProductCode" ).focus();
//            return;
//        }
        
//        if (trim(document.getElementById("ddTranxSource").value) == "null")
//        {
//            alert ("Transaction Source is required");
//            document.getElementById( "ddTranxSource" ).focus();
//            return;
//        }
        
        if (trim(document.getElementById("ddDebitCreditTag").value) == "")
        {
            alert ("Debit Credit Tag is required");
            document.getElementById( "ddDebitCreditTag" ).focus();
            return;
        }
        
//        if (trim(document.getElementById("txtGLAcctCode").value)== "")
//        {
//            alert ("GL Code is required");
//            document.getElementById( "txtGLAcctCode" ).focus();
//            return;        
//        }
//        else if (trim(document.getElementById("txtGLAcctCode").value).length != 9)
//        {
//            alert ("GL Code must consist of 9 characters");
//            document.getElementById( "txtGLAcctCode" ).focus();
//            return;
//        }
        
        if (trim(document.getElementById("txtGLDesc").value) == "")
        {
            alert ("GL's Description is required");
            document.getElementById( "txtGLDesc" ).focus();
            return;
        }
        
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.commit_record(this.set_TBL_GRS_ACCOUNTINGENTRIES(), this.commit_record_callback);       
    }
    
    this.commit_record_callback = function(result)
    {        
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message();
                alert(msg.value);
                return;
            }
            
            alert("RECORD COMMITTED SUCCESSFULLY");
            this.commons = new common_method();
            
            this.commons.to_clear_screen("tblGRSAccountingEntries");  
            this.commons.to_read_only("tblGRSAccountingEntries", true);  
            
            this.commons.to_clear_screen("tbl_Tranx_Rec_All");  
            this.commons.to_read_only("tbl_Tranx_Rec_All", true);  
            
            this.commons.to_clear_screen("tbl_TranxSource");  
            this.commons.to_read_only("tbl_TranxSource", true);  
            
            this.commons.to_clear_screen("tbl_RecordID");  
            this.commons.to_read_only("tbl_RecordID", true);
            
            this.commons.to_clear_screen("tbl_NSVS");
            this.commons.to_read_only("tbl_NSVS", true);          
            
            this.commons.to_clear_screen("tbl_GLCode_Desc_DrCr");  
            this.commons.to_read_only("tbl_GLCode_Desc_DrCr", true);  
            
            this.commons.to_unlock_button("tblGRSAccountingEntriesButtons","btnAddGRSAccountingEntries~");          
            
            document.getElementById("div_Tranx_Rec_All").style.display          = "none";                      
            document.getElementById("div_TranxSource").style.display            = "none";                      
            document.getElementById("div_RecordID").style.display               = "none";                      
            document.getElementById("div_NSVS").style.display                   = "none";                    
            document.getElementById("div_GLCode_Desc_DrCr").style.display       = "none";
            
            this.commons = null;      

            this.grs_accountingentries_grid = new grid_viewer();
            this.grs_accountingentries_grid.sql_source  = "sp_grs_accountingentries_grid~";
            this.grs_accountingentries_grid.div_container = document.getElementById("divtbl_grs_accountingentries");
            this.grs_accountingentries_grid.datarow_column = 0;
            this.grs_accountingentries_grid.page_size = 10;
            this.grs_accountingentries_grid.open_grid_view();
            this.grs_accountingentries_grid = null;        
        }
    }
    
    this.delete_record = function()
    {     
        if (confirm("Delete Record"))
        {
            if (this.SEQ_ID == null)
            {
                this.SEQ_ID = document.getElementById("txtHidden_GRSAccountingEntries_SEQ_ID").value;
            }
            
            if (this.SEQ_ID == null || this.SEQ_ID == "")
            {
                alert("RECORD NOT FOUND.");return;
            }
            
            IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.delete_record(this.SEQ_ID, this.delete_record_callback); 
        }
        else
        {
            this.commons = new common_method();
            this.commons.to_clear_screen("tblGRSAccountingEntries");  
            this.commons.to_read_only("tblGRSAccountingEntries",true);
            this.commons.to_unlock_button("tblGRSAccountingEntriesButtons","btnAddGRSAccountingEntries~");
            this.commons = null;   
        }
    }     
        
    this.delete_record_callback = function(result)
    {       
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message();
                alert(msg.value);return;
            }             
            
            alert("RECORD DELETED SUCCESSFULLY");
            this.commons = new common_method();
            this.commons.to_clear_screen("tblGRSAccountingEntries");  
            this.commons.to_read_only("tblGRSAccountingEntries",true);
            this.commons.to_unlock_button("tblGRSAccountingEntriesButtons","btnAddGRSAccountingEntries~");
            this.commons = null;      

            this.grs_accountingentries_grid = new grid_viewer();
            this.grs_accountingentries_grid.sql_source  = "sp_grs_accountingentries_grid~";
            this.grs_accountingentries_grid.div_container = document.getElementById("divtbl_grs_accountingentries");
            this.grs_accountingentries_grid.page_size = 10;
            this.grs_accountingentries_grid.datarow_column = 0;
            this.grs_accountingentries_grid.open_grid_view();
            this.grs_accountingentries_grid = null;                    
        }
    } 
    
    this.ActiveModule = function (divName)
    {
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.ActiveModule(divName);
    }
    
    this.do_ajaxgridviewer = function (parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                document.getElementById("txtHidden_GRSAccountingEntries_SEQ_ID").value = command_param[1];

                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    //this.rollback_record();
                    this.load_record();
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }
    
    this.process_grid_viewer = function (parameter)
    {
        this.grs_accountingentries_grid = new grid_viewer();
        this.grs_accountingentries_grid.sql_source     = "sp_grs_accountingentries_grid~";
        this.grs_accountingentries_grid.div_container  = document.getElementById("divtbl_grs_accountingentries");
        this.grs_accountingentries_grid.datarow_column = 0;
        this.grs_accountingentries_grid.page_size = 10;
        this.grs_accountingentries_grid.process_ajax(parameter);
        this.grs_accountingentries_grid = null;
    }
    
    this.load_record = function()
    {
        this.pull_server_data()
    }   
    
    this.load_grs_accountingentries_grid_viewer = function()
    {        
        this.grs_accountingentries_grid = new grid_viewer();
        this.grs_accountingentries_grid.sql_source  = "sp_grs_accountingentries_grid~";
        this.grs_accountingentries_grid.div_container = document.getElementById("divtbl_grs_accountingentries");
        this.grs_accountingentries_grid.datarow_column = 0;
        this.grs_accountingentries_grid.page_size = 10;
        this.grs_accountingentries_grid.open_grid_view();
        this.grs_accountingentries_grid = null;
    }
    
    this.dropdown_transactonccy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddTranxCcyCode", result.value, "fldvalue", "FldCode");
            this.commons = null;
        }
    }

    this.dropdown_accountccy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddAcctCcyCode", result.value, "fldvalue", "FldCode");
            this.commons = null;
        }
    }
    
    this.dropdown_implementationtype_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddImplementationType", result.value, "fldvalue", "flddescription");
            this.commons = null;
        }
    }
    
    this.dropdown_transactionsource_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddTranxSource", result.value, "fldvalue", "flddescription");
            this.commons = null;
        }
    }
    
    this.dropdown_recordid_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddRecordID", result.value, "fldvalue", "flddescription");
            this.commons = null;
        }
    }
    
    this.insert_record = function()
    {    
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.insert_record(this.insert_record_callback);    
    }
    
    this.insert_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message(); alert(msg.value); return;
            }

            this.commons = new common_method();              
            this.commons.to_clear_screen("tblGRSAccountingEntries");  
            this.commons.to_read_only("tblGRSAccountingEntries", false);  
            this.commons.to_unlock_button("tblGRSAccountingEntriesButtons","btnSaveGRSAccountingEntries~btnCancelGRSAccountingEntries");
            this.commons = null;            
    
            document.getElementById("div_Tranx_Rec_All").style.display          = "none";                      
            document.getElementById("div_TranxSource").style.display            = "none";                      
            document.getElementById("div_RecordID").style.display               = "none";                      
            document.getElementById("div_NSVS").style.display                   = "none";
            document.getElementById("div_GLCode_Desc_DrCr").style.display       = "none";  
        }
    }
        
    this.populate_dropdownboxes = function ()
    {
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getTransactionCcy(this.dropdown_transactonccy_callback);
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getAccountCcy(this.dropdown_accountccy_callback);        
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getImplementationType(this.dropdown_implementationtype_callback);
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getTransactionSource(this.dropdown_transactionsource_callback);
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getRecordID(this.dropdown_recordid_callback);        
    }
    
    this.pull_server_data = function()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.get_seq_id().value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == '' || this.SEQ_ID =='undefined')
        {
            return;
        }
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.GET_GRS_ACCOUNTINGENTRIES_RECORD(this.SEQ_ID, this.pull_server_data_callback);
    }
    
    this.pull_server_data_callback = function(result)
    {
        if (result!= null && result.value != null)
        {
            var ds = result.value;
            
            if (ds.Tables[0].Rows.length > 0)
            {                
                this.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                this.TranxCcyCode       = ds.Tables[0].Rows[0].TranxCcyCode;
                this.AcctCcyCode        = ds.Tables[0].Rows[0].AcctCcyCode;
                this.ImplementationType = ds.Tables[0].Rows[0].ImplementationType;
                this.GLAcctCode         = ds.Tables[0].Rows[0].GLAcctCode;
                this.GLDesc             = ds.Tables[0].Rows[0].GLDesc; 
                this.DebitCreditTag     = ds.Tables[0].Rows[0].DebitCreditTag;
                this.TranxSource        = ds.Tables[0].Rows[0].TranxSource; 
                this.RecordID           = ds.Tables[0].Rows[0].RecordID;   
                this.NSVS               = ds.Tables[0].Rows[0].NSVS;  
                this.FCCFCR             = ds.Tables[0].Rows[0].FCCFCR;  
                this.FCDURBU            = ds.Tables[0].Rows[0].FCDURBU;    
                this.HandlingType       = ds.Tables[0].Rows[0].HandlingType;
                
                document.getElementById("btnEditGRSAccountingEntries").disabled = "";
                document.getElementById("btnDeleteGRSAccountingEntries").disabled = "";
                
                document.getElementById("txtHidden_GRSAccountingEntries_SEQ_ID").value  = this.SEQ_ID;
                document.getElementById("ddTranxCcyCode").value                         = this.TranxCcyCode;
                document.getElementById("ddAcctCcyCode").value                          = this.AcctCcyCode;
                document.getElementById("ddImplementationType").value                   = this.ImplementationType;
                document.getElementById("txtGLAcctCode").value                          = this.GLAcctCode;
                document.getElementById("txtGLDesc").value                              = this.GLDesc;
                if (trim(this.DebitCreditTag) == "D")
                {
                    document.getElementById("ddDebitCreditTag").selectedIndex           = 1;
                }
                else
                {
                    document.getElementById("ddDebitCreditTag").selectedIndex           = 2;
                }
                
                if (this.TranxSource == 0)
                {
                    document.getElementById("ddTranxSource").selectedIndex              = 0;
                }
                else
                {
                    document.getElementById("ddTranxSource").value                      = this.TranxSource;
                }
                
                if (this.RecordID == 0)
                {
                    document.getElementById("ddRecordID").selectedIndex                 = 0;
                }
                else
                {
                    document.getElementById("ddRecordID").value                         = this.RecordID;
                }
                
                if (this.NSVS == "0")
                {
                    document.getElementById("ddNSVS").selectedIndex                     = 1;
                }
                else if (this.NSVS == "1")
                {
                    document.getElementById("ddNSVS").selectedIndex                     = 2;
                }
                else
                {
                    document.getElementById("ddNSVS").selectedIndex                     = 0;
                }
        
                if (this.FCCFCR == "0")
                {
                    document.getElementById("ddFCCFCR").selectedIndex                   = 1;
                }
                else if (this.FCCFCR == "1")
                {
                    document.getElementById("ddFCCFCR").selectedIndex                   = 2;
                }        
                else
                {
                    document.getElementById("ddFCCFCR").selectedIndex                   = 0;
                } 
              
                if (this.FCDURBU == "0")
                {
                    document.getElementById("ddFCDURBU").selectedIndex                  = 1;
                }
                else if (this.FCDURBU == "1")
                {
                    document.getElementById("ddFCDURBU").selectedIndex                  = 2;
                }  
                else
                {
                    document.getElementById("ddFCDURBU").selectedIndex                  = 0;
                }
                
                document.getElementById("ddHandlingType").selectedIndex                 = this.HandlingType;
                
//                if (trim(this.HandlingType) == "0")
//                {
//                    document.getElementById("ddHandlingType").selectedIndex                  = 1;
//                }
//                else
//                {
//                    document.getElementById("ddHandlingType").selectedIndex                  = 2;
//                }
                
                
               
                this.commons = new common_method();              
                //this.commons.to_clear_screen("tbl_Tranx_Rec_All");  
                this.commons.to_read_only("tbl_Tranx_Rec_All", true);  
                
                //this.commons.to_clear_screen("tbl_TranxSource");  
                this.commons.to_read_only("tbl_TranxSource", true);  
                
                //this.commons.to_clear_screen("tbl_RecordID");  
                this.commons.to_read_only("tbl_RecordID", true);  
                
                //this.commons.to_clear_screen("tbl_NSVS");  
                this.commons.to_read_only("tbl_NSVS", true);          
                
                //this.commons.to_clear_screen("tbl_GLCode_Desc_DrCr");  
                this.commons.to_read_only("tbl_GLCode_Desc_DrCr", true);  
                document.getElementById("div_GLCode_Desc_DrCr").style.display          = "block";    
                           
                if (this.HandlingType == "1")
                {
                    //this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
                    //this.commons.to_read_only("tbl_TranxSource", false);
                    document.getElementById("div_Tranx_Rec_All").style.display          = "block";                      
                    document.getElementById("div_TranxSource").style.display            = "block";                      
                    document.getElementById("div_RecordID").style.display               = "none";                      
                    document.getElementById("div_NSVS").style.display                   = "none";
                }
                else if (this.HandlingType == "2")
                {
                    //this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
                    //this.commons.to_read_only("tbl_RecordID", false);  
                    document.getElementById("div_Tranx_Rec_All").style.display          = "block";                      
                    document.getElementById("div_TranxSource").style.display            = "none";                      
                    document.getElementById("div_RecordID").style.display               = "block";                      
                    document.getElementById("div_NSVS").style.display                   = "none";
                }
                else if (this.HandlingType == "3")
                {
                    //this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
                    document.getElementById("div_Tranx_Rec_All").style.display          = "block";                      
                    document.getElementById("div_TranxSource").style.display            = "none";                      
                    document.getElementById("div_RecordID").style.display               = "none";                      
                    document.getElementById("div_NSVS").style.display                   = "none";
                }
                else if (this.HandlingType == "4")
                {
                    //this.commons.to_read_only("tbl_NSVS", false);  
                    document.getElementById("div_Tranx_Rec_All").style.display          = "none";                      
                    document.getElementById("div_TranxSource").style.display            = "none";                      
                    document.getElementById("div_RecordID").style.display               = "none";                      
                    document.getElementById("div_NSVS").style.display                   = "block";
                }
                
                this.commons = null;
            }
        }        
    }
    
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]   = "TranxCcyCode~"           + TranxCcyCode;
        data_row[data_row.length]   = "AcctCcyCode~"            + AcctCcyCode;
        data_row[data_row.length]   = "ImplementationType~"     + ImplementationType;
        data_row[data_row.length]   = "GLAcctCode~"             + GLAcctCode;
        data_row[data_row.length]   = "GLDesc~"                 + GLDesc;
        data_row[data_row.length]   = "DebitCreditTag~"         + DebitCreditTag;
        data_row[data_row.length]   = "TranxSource~"            + TranxSource;
        data_row[data_row.length]   = "RecordID~"               + RecordID;
        data_row[data_row.length]   = "NSVS~"                   + NSVS;
        data_row[data_row.length]   = "FCCFCR~"                 + FCCFCR;
        data_row[data_row.length]   = "FCDURBU~"                + FCDURBU;
        data_row[data_row.length]   = "HandlingType~"           + HandlingType;
        
        return data_row.join("$");
    }
    
    this.rollback_record = function()
    {
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.rollback_record(this.rollback_record_callback);
    }    
    
    this.rollback_record_callback = function(result)
    {
        var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message();

        if (msg != null && msg.value.length > 0)
        {
            alert(msg.value);
        }
        this.commons = new common_method();        
        //this.commons.to_read_only("tblGRSAccountingEntries",true);
        //this.commons.to_clear_screen("tblGRSAccountingEntries");   
        this.commons.to_clear_screen("tblGRSAccountingEntries");  
        this.commons.to_read_only("tblGRSAccountingEntries", true);  
        
        this.commons.to_clear_screen("tbl_Tranx_Rec_All");  
        this.commons.to_read_only("tbl_Tranx_Rec_All", true);  
        
        this.commons.to_clear_screen("tbl_TranxSource");  
        this.commons.to_read_only("tbl_TranxSource", true);  
        
        this.commons.to_clear_screen("tbl_RecordID");  
        this.commons.to_read_only("tbl_RecordID", true);
        
        this.commons.to_clear_screen("tbl_NSVS");
        this.commons.to_read_only("tbl_NSVS", true);          
        
        this.commons.to_clear_screen("tbl_GLCode_Desc_DrCr");  
        this.commons.to_read_only("tbl_GLCode_Desc_DrCr", true);  
        
        document.getElementById("div_Tranx_Rec_All").style.display          = "none";                      
        document.getElementById("div_TranxSource").style.display            = "none";                      
        document.getElementById("div_RecordID").style.display               = "none";                      
        document.getElementById("div_NSVS").style.display                   = "none";                    
        document.getElementById("div_GLCode_Desc_DrCr").style.display       = "none";
        
        this.commons.to_unlock_button("tblGRSAccountingEntriesButtons","btnAddGRSAccountingEntries~");
        this.commons = null;
        this.SEQ_ID = null;
        document.getElementById("txtHidden_GRSAccountingEntries_SEQ_ID").value = "";
        
        return;
    }
    
    this.show_required_fields = function (handlingvalue)
    {
        this.commons = new common_method();              
        this.commons.to_clear_screen("tbl_Tranx_Rec_All");  
        this.commons.to_read_only("tbl_Tranx_Rec_All", true);  
        
        this.commons.to_clear_screen("tbl_TranxSource");  
        this.commons.to_read_only("tbl_TranxSource", true);  
        
        this.commons.to_clear_screen("tbl_RecordID");  
        this.commons.to_read_only("tbl_RecordID", true);  
        
        this.commons.to_clear_screen("tbl_NSVS");  
        this.commons.to_read_only("tbl_NSVS", true);          
        
        this.commons.to_clear_screen("tbl_GLCode_Desc_DrCr");  
        this.commons.to_read_only("tbl_GLCode_Desc_DrCr", false);  
        
        this.commons.to_clear_screen("tbl_GLCode_Desc_DrCr");  
        this.commons.to_read_only("tbl_GLCode_Desc_DrCr", false);          
        document.getElementById("div_GLCode_Desc_DrCr").style.display          = "block";        

        if (handlingvalue == "1")// TranxSource
        {
            this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
            this.commons.to_read_only("tbl_TranxSource", false);  
            
            document.getElementById("div_Tranx_Rec_All").style.display          = "block";                      
            document.getElementById("div_TranxSource").style.display            = "block";                      
            document.getElementById("div_RecordID").style.display               = "none";                      
            document.getElementById("div_NSVS").style.display                   = "none";
        }
        else if (handlingvalue == "2")//Record ID
        {
            this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
            this.commons.to_read_only("tbl_RecordID", false);  
            document.getElementById("div_Tranx_Rec_All").style.display          = "block";                      
            document.getElementById("div_TranxSource").style.display            = "none";                      
            document.getElementById("div_RecordID").style.display               = "block";                      
            document.getElementById("div_NSVS").style.display                   = "none";
        }
        else if (handlingvalue == "3")//For All
        {
            this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
            document.getElementById("div_Tranx_Rec_All").style.display          = "block";                      
            document.getElementById("div_TranxSource").style.display            = "none";                      
            document.getElementById("div_RecordID").style.display               = "none";                      
            document.getElementById("div_NSVS").style.display                   = "none";
            document.getElementById("ddDebitCreditTag").selectedIndex           = 2;
            document.getElementById("ddDebitCreditTag").disabled                = "disabled";
        }
        else if (handlingvalue == "4")//NSVS
        {
            this.commons.to_read_only("tbl_NSVS", false);  
            document.getElementById("div_Tranx_Rec_All").style.display          = "none";                      
            document.getElementById("div_TranxSource").style.display            = "none";                      
            document.getElementById("div_RecordID").style.display               = "none";                      
            document.getElementById("div_NSVS").style.display                   = "block";
            document.getElementById("ddNSVS").selectedIndex                     = 2;
            document.getElementById("ddNSVS").disabled                          = "disabled";
        }
        
        this.commons = null;
    }
    
    this.set_TBL_GRS_ACCOUNTINGENTRIES = function ()
    {
        if (document.getElementById("ddTranxCcyCode").value == "null" || document.getElementById("ddTranxCcyCode").value == "")
        {         
            TranxCcyCode    = 0;
        }
        else
        {            
            TranxCcyCode    = document.getElementById("ddTranxCcyCode").value;
        }
        
        if (document.getElementById("ddAcctCcyCode").value == "null" || document.getElementById("ddAcctCcyCode").value == "")
        {         
            AcctCcyCode = 0;
        }
        else
        {            
            AcctCcyCode = document.getElementById("ddAcctCcyCode").value;
        }
        
        if (document.getElementById("ddImplementationType").value == "null" || document.getElementById("ddImplementationType").value == "")
        {         
            ImplementationType  = 0;
        }
        else
        {            
            ImplementationType  = document.getElementById("ddImplementationType").value;
        }
        
        GLAcctCode      = document.getElementById("txtGLAcctCode").value;
        GLDesc          = document.getElementById("txtGLDesc").value;
        DebitCreditTag  = document.getElementById("ddDebitCreditTag").value;
        
        if (document.getElementById("ddTranxSource").value == "null" || document.getElementById("ddTranxSource").value == "")
        {         
            TranxSource = 0;
        }
        else
        {           
            TranxSource = document.getElementById("ddTranxSource").value;
        }
        
        if (document.getElementById("ddRecordID").value == "null" || document.getElementById("ddRecordID").value == "")
        {         
            RecordID = 0;
        }
        else
        {           
            RecordID = document.getElementById("ddRecordID").value;
        }
        
        if (document.getElementById("ddNSVS").value == "null" || document.getElementById("ddNSVS").value == "")
        {         
            NSVS    = 0;
        }
        else
        {           
            NSVS    = document.getElementById("ddNSVS").value;
        }
        
        if (document.getElementById("ddFCCFCR").value == "null" || document.getElementById("ddFCCFCR").value == "")
        {         
            FCCFCR  = 0;
        }
        else
        {           
            FCCFCR  = document.getElementById("ddFCCFCR").value;
        }
        
        if (document.getElementById("ddFCDURBU").value == "null" ||document.getElementById("ddFCDURBU").value == "")
        {         
            FCDURBU = 0;
        }
        else
        {           
            FCDURBU = document.getElementById("ddFCDURBU").value;
        }
        
        if (document.getElementById("ddHandlingType").value == "null" ||document.getElementById("ddHandlingType").value == "")
        {         
            HandlingType    = 0;
        }
        else
        {           
            HandlingType    = document.getElementById("ddHandlingType").value;
        }        
        
        return this.push_data_to_server();
    }   
    
    this.update_record = function ()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHidden_GRSAccountingEntries_SEQ_ID").value;
        }
        
        if (this.SEQ_ID == null || this.SEQ_ID == "")
        {
            alert ("RECORD NOT FOUND."); return;
        }
        
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.update_record(this.SEQ_ID, this.update_record_callback);
    }   
    
    this.update_record_callback = function(result)
    {
        if (result != null && result.value != "undefined")
        {
            if (result.value == false)
            {
                var msg = IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.read_message();
                alert(msg.value);
                return;
            }
            
            this.commons = new common_method();  
            this.commons.to_read_only("tblGRSAccountingEntries", false);  
            this.commons.to_unlock_button("tblGRSAccountingEntriesButtons","btnSaveGRSAccountingEntries~btnCancelGRSAccountingEntries");
                                 
            
            this.commons.to_read_only("tbl_GLCode_Desc_DrCr", false);          
            
            if (document.getElementById("ddHandlingType").selectedIndex == 1)
            {
                this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
                this.commons.to_read_only("tbl_TranxSource", false);  
            }
            else if (document.getElementById("ddHandlingType").selectedIndex == 2)
            {
                this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
                this.commons.to_read_only("tbl_RecordID", false);  
            }
            else if (document.getElementById("ddHandlingType").selectedIndex == 3)
            {
                this.commons.to_read_only("tbl_Tranx_Rec_All", false);  
            }
            else if (document.getElementById("ddHandlingType").selectedIndex == 4)
            {
                this.commons.to_read_only("tbl_NSVS", false);  
            }
            
            this.commons = null;
        }
    }
    
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
    
    this.check_currency_combination = function ()
    {
        if (document.getElementById("ddTranxCcyCode").value != "null" || document.getElementById("ddTranxCcyCode").value != "")
        {
            var i = document.getElementById("ddTranxCcyCode").selectedIndex;
            var tranxccy = document.getElementById("ddTranxCcyCode").options[i].text;
            
            if (tranxccy == "PHP")
            {            
                document.getElementById("ddAcctCcyCode").value          = 1;
                document.getElementById("ddAcctCcyCode").disabled       = "disabled";
            }
            else
            {   
                document.getElementById("ddAcctCcyCode").selectedIndex  = 0;
                document.getElementById("ddAcctCcyCode").disabled       = "";
            }
        }        
    }
}