function execute_ajx_command(param)
{
    if (fLoad != null && fLoad != "undefined")
    {
        fLoad.do_ajax_grid_search(param);
    }
}

function execute_ajaxgridview_command(param)
{
    if (fLoad != null && fLoad != "undefined")
    {
        fLoad.do_ajaxgridview(param);
    }
}

function js_viewotherchannels()
{
    this.gridView           = null;
    this.jsCom              = null;
    
    this.indexMode          = 0;
    this.nextMode           = 1;
    this.previousMode       = 2;
    
    this.folderName         = null;
    this.recordStatus       = null;
    
    //file info
    this.fileFormat         = null;
    this.batchID            = null;
    this.dateUploaded       = null;
    
    //remitter info
    this.fSEQ_ID            = null;
    this.remitName          = null;
    this.remitAddress       = null;
    
    //benef info
    this.traceNo            = null;
    this.benefName          = null;
    this.benefAddress       = null;
    this.casaNo             = null;
    this.acctType           = null;
    this.branchAcct         = null;
    this.acctCcy            = null;
    
    //transaction info    
    this.RefNo              = null;

    this.amount             = null;
    this.specialMsg         = null;
    this.implemDate         = null;
    
    //ajax grid
    this.rSEQ_ID            = null;
    this.grid_ajax_page     = "../AjxPage/ajaxgrid.aspx";
    this.ajax_grid          = "GRIDCLASSADD";
    
    this.do_ajax_grid_search = function(parameter)
    {
        this.ajax_grid.process_ajax(parameter);
        
        var command_param = null;
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenFileFormat").value = command_param[1];
            
                if (command_param[0] == "ajx_selected")
                {
                    this.fSEQ_ID = command_param[1];
                    this.create_grid_no_status();
                }
            }
        }
    }
    
    this.create_grid_no_status = function()
    {
        if (this.fSEQ_ID != null)
        {
            this.recordStatus = document.getElementById("ddRecordStatus")[document.getElementById("ddRecordStatus").selectedIndex].text;
            
            if (this.recordStatus.length < 2)
            {
                this.recordStatus = null;
            }
            
            this.gridView                   = new grid_viewer();
            this.gridView.sql_source        = "SP_GRIDVIEW_OTHERCHANNELS~" + this.fSEQ_ID + "~" + this.recordStatus + "~";
            this.gridView.div_container     = document.getElementById("dgRecords");
            this.gridView.datarow_column    = 0;
            this.gridView.open_grid_view();
        }
        
        this.createGrid();        
        document.getElementById("divGrid").style.display            = "block";
        document.getElementById("divFileLoader").style.display      = "none";
        document.getElementById("divUploadedRecords").style.display = "none";
    }
    
    this.do_ajaxgridview = function(parameter)
    {        
        var command_param = null;
        this.ajax_grid.process_ajax(parameter);
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                document.getElementById("txtHiddenFileFormat").value = command_param[1];
                
                if (command_param[0] == "ajx_selected")
                {
                    this.rSEQ_ID = command_param[1];
                    this.load_record();
                }
                else
                {
                    this.process_grid_viewer(parameter);
                }
            }
        }
    }
    
    this.load_record = function()
    {
        this.pull_server_data();
        document.getElementById("divUploadedRecords").style.displayll = "block";
    }
    
    this.pull_server_data = function()
    {
        if (this.rSEQ_ID != null | this.rSEQ_ID != 'undefined')
            OtherChannels.get_records(this.rSEQ_ID, this.pull_server_data_callback);
    }
    
    this.pull_server_data_callback = function(result)
    {
        if (result != null | result.value != null)
        {
            var ds = result.value;
            
            if (ds.Tables[0].Rows.length > 0)
            {
                this.fileFormat         = ds.Tables[0].Rows[0].FileFormat.toString();
                this.batchID            = ds.Tables[0].Rows[0].BatchID.toString();
                this.dateUploaded       = ds.Tables[0].Rows[0].dateuploaded.toString();
                this.remitName          = ds.Tables[0].Rows[0].RemitterInfo.toString();
                this.benefName          = ds.Tables[0].Rows[0].BeneficiaryInfo.toString();
                this.casaNo             = ds.Tables[0].Rows[0].AccountNo.toString();
                this.acctType           = ds.Tables[0].Rows[0].AcctType.toString();
                this.branchAcct         = ds.Tables[0].Rows[0].BranchName.toString();
                this.acctCcy            = ds.Tables[0].Rows[0].AcctCurrency.toString();
                this.refNo              = ds.Tables[0].Rows[0].RefNo.toString();
                this.amount             = ds.Tables[0].Rows[0].Amount.toString();
                this.specialMsg         = ds.Tables[0].Rows[0].SpecialMessage.toString();
                this.implemDate         = ds.Tables[0].Rows[0].implementationdate.toString();
                this.traceNo            = ds.Tables[0].Rows[0].TraceNo.toString();
                
                document.getElementById("lblFileFormat").innerText          = this.fileFormat;
                document.getElementById("lblBatchID").innerText             = this.batchID;
                document.getElementById("lblDateUploaded").innerText        = this.dateUploaded;
                document.getElementById("lblRemitName").innerText           = this.remitName;
                document.getElementById("lblRemitAddress").innerText        = this.remitAddress;
                document.getElementById("lblBenefName").innerText           = this.benefName;
                document.getElementById("lblBenefAddress").innerText        = this.benefAddress;
                document.getElementById("lblTraceNo").innerText             = this.traceNo;
                document.getElementById("lblBranchAcct").innerText          = this.branchAcct;
                document.getElementById("lblCasaNo").innerText              = this.casaNo;
                document.getElementById("lblAcctType").innerText            = this.acctType;
                document.getElementById("lblAcctCcy").innerText             = this.acctCcy;
                document.getElementById("lblRefNo").innerText               = this.refNo;                
                document.getElementById("lblSpecialMsg").innerText          = this.specialMsg;
                document.getElementById("lblViewRecordHead").innerText      = this.fileFormat + " Transactions";
                
                document.getElementById("divCommit").style.display          = "block";
                document.getElementById("divUploadedRecords").style.display = "block";
            }
        }
    }
    
    this.page_load = function()
    {
        this.jsCom = new common_method();
        var qRecStat = this.lookUpCmd("UploadedRecordStatus");
        var qFileFormat = this.lookUpCmd("OtherChannels");
        
        document.getElementById("divFileLoader").style.display      = "block";        
        
        this.jsCom.to_load_dropdownlist("ddFileFormat", this.getDataSet(qFileFormat, false), "VALUE", "CODE");
        this.jsCom.to_load_dropdownlist("ddRecordStatus", this.getDataSet(qRecStat, false), "VALUE", "CODE");
    }
    
    this.panel_search_otherchannels = function()
    {   
        var grid_query = null;
        this.ajax_grid = new grid_selector();
        
        if (document.getElementById("ddFileFormat").value == 'null')
        {
            grid_query = "sp_search_otherchannels~";
        }
        else
        {
            grid_query = "sp_search_otherchannels~"
                       + document.getElementById("ddFileFormat").value
                       + "~";
        }
        
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, grid_query, 0, this.SEQ_ID);
    }
    
    this.getDataSet = function(sqlQuery, bStoredProc)
    {
        var ds = OtherChannels.getDataSet(sqlQuery, bStoredProc).value;
        
        if (ds != null)
            return ds;
        else
            return null;
    }
    
    this.lookUpCmd = function(fldGroup)
    {
        var sQuery = "SELECT fldCode [CODE], fldValue [VALUE] FROM TBL_LOOKUP"
                   + " WHERE fldgroup = '" + fldGroup + "'";

        return sQuery;
    }
    
    this.createGrid = function()
    {
        this.recordStatus = document.getElementById("ddRecordStatus")[document.getElementById("ddRecordStatus").selectedIndex].text;
        
        if (this.fSEQ_ID != null)
        {
            this.gridView                   = new grid_viewer();
            
            this.gridView.sql_source        = "SP_GRIDVIEW_OTHERCHANNELS~" + this.fSEQ_ID + "~" + this.recordStatus + "~";
            this.gridView.div_container     = document.getElementById("dgRecords");
            this.gridView.datarow_column    = 0;
            this.gridView.open_grid_view();
        }
                
        document.getElementById("divGrid").style.display            = "block";
        document.getElementById("divFileLoader").style.display      = "none";
        document.getElementById("divUploadedRecords").style.display = "none";
        document.getElementById("lblRecordList").innerText          = "Uploaded Transactions";// for the file " + this.batchID;
    }
    
    this.getUploadedRecords = function()
    {
        var sqlCmd = null;
        
        sqlCmd = "SELECT TOP 1 f.FileFormat, f.BatchID, "
        sqlCmd = sqlCmd + "f.RemitterName, "
        sqlCmd = sqlCmd + "f.BeneficiaryName, f.AccountNo, f.AccountCurrency, f.ImplementationDate, f.Amount, f.SpecialMessage, "
        sqlCmd = sqlCmd + "b.BranchName, l.FldCode AccountType"
        sqlCmd = sqlCmd + " FROM tbl_fileuploadtemp f, tbl_branches b, tbl_lookup l"
        sqlCmd = sqlCmd + " WHERE l.FldGroup = 'AccountType' and l.FldValue = f.AccountType and f.BranchCode = b.BranchCode"
        
        return sqlCmd;
    }
    
    this.showRecord = function(viewMode)
    {
        this.getRecordDetail(viewMode);
        
        document.getElementById("lblFileFormat").innerText          = this.fileFormat;
        document.getElementById("lblBatchID").innerText             = this.batchID;
        document.getElementById("lblDateUploaded").innerText        = this.dateUploaded;
        document.getElementById("lblRemitName").innerText           = this.remitName;
        document.getElementById("lblRemitAddress").innerText        = this.remitAddress;
        document.getElementById("lblBenefName").innerText           = this.benefName;
        document.getElementById("lblBenefAddress").innerText        = this.benefAddress;
        document.getElementById("lblTraceNo").innerText             = this.traceNo;
        document.getElementById("lblBranchAcct").innerText          = this.branchAcct;
        document.getElementById("lblCasaNo").innerText              = this.casaNo;
        document.getElementById("lblAcctType").innerText            = this.acctType;
        document.getElementById("lblAcctCcy").innerText             = this.acctCcy;
        document.getElementById("lblRefNo").innerText               = this.refNo;
        document.getElementById("lblAmount").innerText              = this.amount;
        
        if (this.specialMsg.length > 0)
        {   
            document.getElementById("lblSpecialMsg").innerText      = this.specialMsg;
        }
        else
        {
            document.getElementById("lblSpecialMsg").style.display  = "none";
        }
        
        document.getElementById("divUploadedRecords").style.display = "none";
    }
    
    this.getDataTable = function()
    {
        return OtherChannels.getDataTable(this.getUploadedRecords()).value;
    }
    
    this.setRecordValue = function(table)
    {   
        this.fileFormat         = table.Rows[this.index].FileFormat.toString();
        this.batchID            = table.Rows[this.index].BatchID.toString();
        //this.dateUploaded       = table.Rows[this.index].DateUploaded.toString();
        this.remitName          = table.Rows[this.index].RemitterName.toString();
        this.benefName          = table.Rows[this.index].BeneficiaryName.toString();
        this.casaNo             = table.Rows[this.index].AccountNo.toString();
        this.acctType           = table.Rows[this.index].AccountType.toString();
        this.branchAcct         = table.Rows[this.index].BranchName.toString();
        this.acctCcy            = table.Rows[this.index].AccountCurrency.toString();
        //this.implemDate         = table.Rows[this.index].ImplementationDate.toString();
        this.amount             = this.setMoneyFormat(table.Rows[this.index].Amount.toString());
        this.specialMsg         = table.Rows[this.index].SpecialMessage.toString();
    }
    
    this.getRecordDetail = function(viewMode)
    {   
        var errMsg = null;        
        
        var dtUploaded = this.getDataTable();
        
        if (dtUploaded.Rows.length > 0)
        {            
            switch (this.viewMode)
            {
                case 2:
                    
                    this.index--;
                    break;
                
                case 1:
                    
                    this.index++;
                    break;
            
                default:
                    
                    index = 0;
                    break;
            }
            
            this.setRecordValue(dtUploaded);
        }
        else
        {
            alert("No records found.");
        }
    }
}