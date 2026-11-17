
function GRSCoreBank ()
{
    var SEQ_ID          = null;
    var Corr_Bank_Code  = null;
    var Corr_Bank_Name  = null;
    var SWIFT_Code      = null;
    var Courier_Code    = null;
    var Currency_Code   = null;
    var Account_Type    = null;
    var CK_Account      = null;
    var GL_Account      = null;
    var GL_Description  = null;
    var OnHold          = null;
    var IBatchCountry   = null;
    var ChargeType      = null;

    this.ActiveModule = function (divName)
    {
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.ActiveModule(divName);
    }
    
    this.commit_record = function ()
    {
        if (trim(document.getElementById("txtCorr_Bank_Name").value) == "")
        {
            alert ("CorrBank Name is required");
            document.getElementById( "txtCorr_Bank_Name" ).focus();
            return;
        }
        
        if (trim(document.getElementById("ddCurrency_Code").value) == "null")
        {
            alert ("Currency is required");
            document.getElementById( "ddCurrency_Code" ).focus();
            return;
        }
        
//        if (trim(document.getElementById("txtCKAccnt").value) == "")
//        {
//            alert ("Input CK Account");
//            document.getElementById( "txtCKAccnt" ).focus();
//            return;
//        }

//        if (trim(document.getElementById("txtGLAccnt").value) == "")
//        {
//            alert ("Input GL Account");
//            document.getElementById( "txtGLAccnt" ).focus();
//            return;
//        }

        if (trim(document.getElementById("ddCountry").value) == "")
        {
            alert ("Country is required");
            document.getElementById( "ddCountry" ).focus();
            return;
        }
        
        if (trim(document.getElementById("ddAccount_Type").value) == "")
        {
            alert ("Account type is required");
            document.getElementById( "ddAccount_Type" ).focus();
            return;
        }
        
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.commit_record(this.set_TBL_GRS_CORRBANK(), this.commit_record_callback);       
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
            this.commons.to_clear_screen("tblGRSCorrBank");  
            this.commons.to_read_only("tblGRSCorrBank",true);
            this.commons.to_unlock_button("tblGRSCorrBankButtons","btnAddGRSCorrBank~");
            this.commons = null;      

            this.grs_corrbank_grid = new grid_viewer();
            this.grs_corrbank_grid.sql_source  = "sp_grs_corrbank_grid~";
            this.grs_corrbank_grid.div_container = document.getElementById("divtbl_GRS_CorrBank");
            this.grs_corrbank_grid.datarow_column = 0;
            this.grs_corrbank_grid.open_grid_view();
            this.grs_corrbank_grid = null;        
        }
    }
    
    this.delete_record = function()
    {     
        if (confirm("Delete Record"))
        {
            if (this.SEQ_ID == null)
            {
                this.SEQ_ID = document.getElementById("txtHidden_tbl_GRS_CorrBank_SEQ_ID").value;
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
            this.commons.to_clear_screen("tblGRSCorrBank");  
            this.commons.to_read_only("tblGRSCorrBank",true);
            this.commons.to_unlock_button("tblGRSCorrBankButtons","btnAddGRSCorrBank~");
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
            this.commons.to_clear_screen("tblGRSCorrBank");  
            this.commons.to_read_only("tblGRSCorrBank",true);
            this.commons.to_unlock_button("tblGRSCorrBankButtons","btnAddGRSCorrBank~");
            this.commons = null;      

            this.grs_corrbank_grid = new grid_viewer();
            this.grs_corrbank_grid.sql_source  = "sp_grs_corrbank_grid~";
            this.grs_corrbank_grid.div_container = document.getElementById("divtbl_GRS_CorrBank");
            this.grs_corrbank_grid.datarow_column = 0;
            this.grs_corrbank_grid.open_grid_view();
            this.grs_corrbank_grid = null;                          
        }
    }
    
    this.do_ajaxgridviewer = function (parameter)
    {
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                document.getElementById("txtHidden_tbl_GRS_CorrBank_SEQ_ID").value = command_param[1];

                if (command_param[0] == "ajx_selected")
                {
                    this.SEQ_ID = command_param[1];
                    this.load_record();
                    return;
                }
                this.process_grid_viewer(parameter);
            }
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
            this.commons.to_clear_screen("tblGRSCorrBank");  
            this.commons.to_read_only("tblGRSCorrBank", false);  
            this.commons.to_unlock_button("tblGRSCorrBankButtons","btnSaveGRSCorrBank~btnCancelGRSCorrBank");
            this.commons = null;
        }
    }
    
    this.load_record = function()
    {
        this.pull_server_data()
    }  
    
    this.load_grs_CorrBank_grid_viewer = function()
    {        
        this.grs_corrbank_grid = new grid_viewer();
        this.grs_corrbank_grid.sql_source  = "sp_grs_corrbank_grid~";
        this.grs_corrbank_grid.div_container = document.getElementById("divtbl_GRS_CorrBank");
        this.grs_corrbank_grid.datarow_column = 0;
        this.grs_corrbank_grid.open_grid_view();
        this.grs_corrbank_grid = null;
    }
        
    this.populate_dropdownboxes = function ()
    {
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getCorrBankCcy(this.dropdown_corebankccy_callback);
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getCorrBanCountry(this.dropdown_corebankcountry_callback);        
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.getChargeType(this.dropdown_chargetype_callback);  
    }
    
    this.dropdown_corebankccy_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCurrency_Code", result.value, "fldvalue", "FldCode");
            this.commons = null;
        }
    }
    
    this.dropdown_corebankcountry_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCountry", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.dropdown_chargetype_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddChargeType", result.value, "fldvalue", "FldDescription");
            this.commons = null;
        }
    }
    
    this.process_grid_viewer = function (parameter)
    {
        this.loadergrid = new grid_viewer();        
        this.loadergrid.sql_source     = "sp_grs_corrbank_grid~";
        this.loadergrid.div_container  = document.getElementById("divtbl_GRS_CorrBank");
        this.loadergrid.datarow_column = 0;
        this.loadergrid.process_ajax(parameter);
        this.loadergrid = null;
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
        IRSWorld.GRSGEFUMaintenance.GRSGEFUMaintenance.GET_GRS_CORRBANK_RECORD(this.SEQ_ID, this.pull_server_data_callback);
    }
    
    this.pull_server_data_callback = function(result)
    {
        if (result!= null && result.value != null)
        {
            var ds = result.value;
            
            if (ds.Tables[0].Rows.length > 0)
            {
                
                this.SEQ_ID             = ds.Tables[0].Rows[0].SEQ_ID;
                this.Corr_Bank_Code     = ds.Tables[0].Rows[0].Corr_Bank_Code;
                this.Corr_Bank_Name     = ds.Tables[0].Rows[0].Corr_Bank_Name;
                this.SWIFT_Code         = ds.Tables[0].Rows[0].SWIFT_Code;
                this.Courier_Code       = ds.Tables[0].Rows[0].Courier_Code;
                this.Currency_Code      = ds.Tables[0].Rows[0].Currency_Code;
                this.Account_Type       = ds.Tables[0].Rows[0].Account_Type;
                this.CK_Account         = ds.Tables[0].Rows[0].CK_Account;
                this.GL_Account         = ds.Tables[0].Rows[0].GL_Account;
                this.GL_Description     = ds.Tables[0].Rows[0].GL_Description;
                this.OnHold             = ds.Tables[0].Rows[0].OnHold;
                this.IBatchCountry      = ds.Tables[0].Rows[0].IBatchCountry;
                this.ChargeType         = ds.Tables[0].Rows[0].ChargeType;

                
                document.getElementById("txtHidden_tbl_GRS_CorrBank_SEQ_ID").value  = this.SEQ_ID;
                
                document.getElementById("btnEditGRSCorrBank").disabled = "";                                
                document.getElementById("btnDeleteGRSCorrBank").disabled = "";
                
                
                document.getElementById("txtCorr_Bank_Code").value  = this.Corr_Bank_Code;
                document.getElementById("txtCorr_Bank_Name").value  = this.Corr_Bank_Name;
                document.getElementById("txtSWIFT_Code").value      = this.SWIFT_Code;
                document.getElementById("txtCourier_Code").value    = this.Courier_Code;
                document.getElementById("ddCurrency_Code").value    = this.Currency_Code;
                if (trim(this.Account_Type) == "RBU")
                {
                    document.getElementById("ddAccount_Type").selectedIndex = 1;
                }
                else
                {
                    document.getElementById("ddAccount_Type").selectedIndex = 2;
                }
                
                document.getElementById("txtCK_Account").value      = this.CK_Account;
                document.getElementById("txtGL_Account").value      = this.GL_Account;
                document.getElementById("txtGL_Description").value  = this.GL_Description;
                
                if (this.OnHold == "1")
                {
                    document.getElementById("ddOnHold").selectedIndex = 1;
                }
                else
                {
                    document.getElementById("ddOnHold").selectedIndex = 2;                
                }
                
                document.getElementById("ddCountry").value          = this.IBatchCountry;
                document.getElementById("ddChargeType").value       = this.ChargeType;
                
            }
        }
    }
        
    this.push_data_to_server = function()
    {
        var data_row = new Array();
        
        data_row[data_row.length]   = "Corr_Bank_Code~"     + Corr_Bank_Code;
        data_row[data_row.length]   = "Corr_Bank_Name~"     + Corr_Bank_Name;
        data_row[data_row.length]   = "SWIFT_Code~"         + SWIFT_Code;
        data_row[data_row.length]   = "Courier_Code~"       + Courier_Code;
        data_row[data_row.length]   = "Currency_Code~"      + Currency_Code;
        data_row[data_row.length]   = "Account_Type~"       + Account_Type;
        data_row[data_row.length]   = "CK_Account~"         + CK_Account;
        data_row[data_row.length]   = "GL_Account~"         + GL_Account;
        data_row[data_row.length]   = "GL_Description~"     + GL_Description;
        data_row[data_row.length]   = "OnHold~"             + OnHold;
        data_row[data_row.length]   = "IBatchCountry~"      + IBatchCountry;        
        data_row[data_row.length]   = "ChargeType~"         + ChargeType;      
        
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
        this.commons.to_read_only("tblGRSCorrBank",true);
        this.commons.to_clear_screen("tblGRSCorrBank");  
        this.commons.to_unlock_button("tblGRSCorrBankButtons","btnAddGRSCorrBank~");
        this.commons = null;
        
        this.SEQ_ID = null;
        document.getElementById("txtHidden_tbl_GRS_CorrBank_SEQ_ID").value = "";
        return;
    }
    
    this.set_TBL_GRS_CORRBANK = function ()
    {    
        Corr_Bank_Code  = document.getElementById("txtCorr_Bank_Code").value;
        Corr_Bank_Name  = document.getElementById("txtCorr_Bank_Name").value;
        SWIFT_Code      = document.getElementById("txtSWIFT_Code").value;
        Courier_Code    = document.getElementById("txtCourier_Code").value;
        Currency_Code   = document.getElementById("ddCurrency_Code").value;
        Account_Type    = document.getElementById("ddAccount_Type").value;
        CK_Account      = document.getElementById("txtCK_Account").value;
        GL_Account      = document.getElementById("txtGL_Account").value;
        GL_Description  = document.getElementById("txtGL_Description").value;
        OnHold          = document.getElementById("ddOnHold").value;
        IBatchCountry   = document.getElementById("ddCountry").value;
        ChargeType      = document.getElementById("ddChargeType").value;
        
        return this.push_data_to_server();
    }
    
    this.update_record = function ()
    {
        if (this.SEQ_ID == null)
        {
            this.SEQ_ID = document.getElementById("txtHidden_tbl_GRS_CorrBank_SEQ_ID").value;
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
            this.commons.to_read_only("tblGRSCorrBank", false);  
            this.commons.to_unlock_button("tblGRSCorrBankButtons","btnSaveGRSCorrBank~btnCancelGRSCorrBank");
            this.commons = null;
        }
    }
    
    this.check_ck_account = function()
    {
        if (event.keyCode >= 48 && event.keyCode <= 57) 
        {
            event.returnValue = true;
        }
        else
        {
            event.returnValue=false;
            alert("Please input only numbers");
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
}