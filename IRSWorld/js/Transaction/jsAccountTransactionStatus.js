
var dbgrid = null;
var datatable = null;
var _AccountNo = null;
var _Filter = null;
var tranStatus = new AccountTransactionStatus();
var gv = new datagrid();
var IsFirstTimeLoading = null;
function AccountTransactionStatus()
{
    this.initialize_page = function()
    {
        this.ajaxTimeout();
    }
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            AccountTransactionStatus.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN.");
        }
    }
    this.loadRecord = function()
    {
        _Filter = document.getElementById("ddSearchType").value;
        if (_Filter == '' || _Filter == null)
        {
            tranStatus.hideSearchType();
            return;
        }
        this.ajaxTimeout();
        return IRSWorld.Transaction.AccountTransactionStatus.getRecord(this.loadRecord_callback);    
    }
    
    this.loadRecord_callback = function(result)
    {
        val = result.value;
        
        if (val != null && val != "" && val != undefined)
        {
            datatable = val;
            show_groups();
        }
        else
        {
            alert("No record found.");
            document.getElementById("tdReportLookup").style.display = 'none';
        }
        return;
    }
    
    this.clearFields = function()
    {
        _Filter     =   null;
        _AccountNo  =   null;
        
        document.getElementById("ddSearchType").value               = "";
        document.getElementById("ddFilterType").value               = "";
        document.getElementById("txtSearchAccountNumber").value     = "";
        datatable                                                   = null;
        document.getElementById("tdReportLookup").style.display     = 'none';
        document.getElementById("txtSearchFirstName").value         = "";
        document.getElementById("txtSearchMiddleName").value        = "";
        document.getElementById("txtSearchLastName").value          = "";
        tranStatus.hideSearchType();  
    }

    this.showSearchType = function()
    {
        var searchType = document.getElementById('ddSearchType').value;
        
        if(searchType != '')
        {
            var teear = 'tr' + searchType;
        
            var x = ['trName', 'trFilter', 'trAccountNumber'];
            for (ctr = 0; ctr < x.length; ctr++)
            {
                if (x[ctr] != teear)
                {
                    document.getElementById(x[ctr]).style.display = 'none';
                }
                else
                {
                    document.getElementById(teear).style.display = 'block';
                    document.getElementById('trButtons').style.display = 'block'
                }
            }
        }
        else
        {
            tranStatus.hideSearchType();
        }
        
    }
    
    this.hideSearchType = function()
    {
        var x = ['trName', 'trFilter', 'trAccountNumber', 'trButtons'];
        for (ctr = 0; ctr < x.length; ctr++)
        {
            document.getElementById(x[ctr]).style.display = 'none';
        }
    }
}

function show_groups()
{
    var FilterType = '';
    var AccountNo = '';
    var FirstName = '';
    var MiddleName = ''
    if (_Filter == 'AccountNumber' )
    {
        AccountNo = document.getElementById('txtSearchAccountNumber').value
    }
    else if (_Filter == 'Filter')
    {
        FilterType = document.getElementById("ddFilterType").value;
    }
    else if (_Filter == 'Name')
    {
        FirstName = document.getElementById('txtSearchFirstName').value.toUpperCase();
        MiddleName = document.getElementById('txtSearchMiddleName').value.toUpperCase();
        LastName = document.getElementById('txtSearchLastName').value.toUpperCase();
    }
    else
    {
        document.getElementById("tdReportLookup").style.display = 'none';
        return;
    }
    
    if (dbgrid == null)
    {
        dbgrid = create_grid();
        dbgrid.bind(datatable, 'tdReportLookup', 'tblReportLookup');
    }
    
    dbgrid.clearFilter();     
    if (_Filter == 'Filter')
    {
        dbgrid.filter.enabled = true;   
        if (FilterType == "ShowAll")
        {
            dbgrid.filter.enabled = false;   
        }
        else if (FilterType == 'Deleted')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'UserDeleted';
            filter.operator = '==';
            filter.value = false;
            dbgrid.addFilter(filter,'&&');
            dbgrid.filter.enabled = true;   
            
            var filter = new datagrid_filter();
            filter.datafield = 'Deleted';
            filter.operator = '==';
            filter.value = true;
            dbgrid.addFilter(filter,null); 
        }
        else if (FilterType == 'UserDeleted')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'Deleted';
            filter.operator = '==';
            filter.value = false;
            dbgrid.addFilter(filter,'&&');
            dbgrid.filter.enabled = true;   
        
            var filter = new datagrid_filter();
            filter.datafield = 'UserDeleted';
            filter.operator = '==';
            filter.value = true;
            dbgrid.addFilter(filter,null); 
        }
        else if (FilterType == 'DeletedAndUserDeleted')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'Deleted';
            filter.operator = '==';
            filter.value = true;
            dbgrid.addFilter(filter,'||'); 
            
            var filter = new datagrid_filter();
            filter.datafield = 'UserDeleted';
            filter.operator = '==';
            filter.value = true;
            dbgrid.addFilter(filter,null); 
        }
        else if (FilterType == 'IsValidated')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'Deleted';
            filter.operator = '==';
            filter.value = false;
            dbgrid.addFilter(filter,'&&'); 
            
            var filter = new datagrid_filter();
            filter.datafield = 'UserDeleted';
            filter.operator = '==';
            filter.value = false;
            dbgrid.addFilter(filter,'&&'); 
            
            var filter = new datagrid_filter();
            filter.datafield = 'IsValidated';
            filter.operator = '==';
            filter.value = true;
            dbgrid.addFilter(filter,null);
        }
        else if (FilterType == 'Unvalidated')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'Deleted';
            filter.operator = '==';
            filter.value = false;
            dbgrid.addFilter(filter,'&&'); 
            
            var filter = new datagrid_filter();
            filter.datafield = 'UserDeleted';
            filter.operator = '==';
            filter.value = false;
            dbgrid.addFilter(filter,'&&'); 
        
            var filter = new datagrid_filter();
            filter.datafield = 'IsValidated';
            filter.operator = '==';
            filter.value = false;
            dbgrid.addFilter(filter,null); 
        }
    }
    else if (_Filter == 'AccountNumber')
    {
        var filter = new datagrid_filter();
        filter.datafield = 'AccountNo';
        filter.operator = '==';
        filter.value = "'" + AccountNo + "'";
        dbgrid.addFilter(filter,null);
        dbgrid.filter.enabled = true;
    }
    else if (_Filter == 'Name')
    {
        var optrfn = null;
        var optrmn = null;
        
        if (MiddleName != '' || LastName != '')
        {
            optrfn = '&&';
        }
        if (LastName != '')
        {
            optrmn = '&&'
        }
        
        if (FirstName != '')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'FirstName';
            filter.operator = '==';
            filter.value = "'" + FirstName + "'";
            dbgrid.addFilter(filter,optrfn);
            dbgrid.filter.enabled = true;
        }
        if (MiddleName != '')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'MiddleName';
            filter.operator = '==';
            filter.value = "'" + MiddleName + "'";
            dbgrid.addFilter(filter,optrmn);
            dbgrid.filter.enabled = true;
        }
        
        if (LastName != '')
        {
            var filter = new datagrid_filter();
            filter.datafield = 'LastName';
            filter.operator = '==';
            filter.value = "'" + LastName + "'";
            dbgrid.addFilter(filter,null);
            dbgrid.filter.enabled = true;
        }
    }
    
    dbgrid.refresh(datatable);
    document.getElementById("tdReportLookup").style.display = 'block';
}

//SAVING
function grid_edit(args)
{
    tranStatus.ajaxTimeout();
    return IRSWorld.Transaction.AccountTransactionStatus.editData(args.additions, args.deletes, args.edits, this.grid_edit_callback);    
}

function grid_edit_callback(param)
{
    if(param.value == null || param.value == '')
    {
        alert("Error: No Response");
        return;
    }
    
    alert(param.value);
    dbgrid.clearChanges();
    dbgrid.clearFilter();
                    
    tranStatus.loadRecord();
}
//</SAVING>

/*
|---------------######--#####----########--#####--------------------|
|---------------##------##--##------##-----##--##-------------------|
|---------------##-###--######------##-----##---##------------------|
|---------------##-###--##--##------##-----##--##-------------------|
|---------------######--##---##--########--#####--------------------|
*/
function create_grid()
{
    {
	    var col = new datagrid_column(); // 0
	    col.align = 'left';
	    col.header = 'SEQ ID';
	    col.datafield = 'SEQ_ID';
	    col.styles.header += 'display:none;';
	    col.styles.datafield += 'display:none;';
        col.styles.cell += 'width:1%;display:none;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 2
	    col.header = 'Remitter Name';
	    col.datafield = 'RemitterName';
	    col.align = 'left';
	    col.styles.cell += 'width:20%';
	    col.styles.datafield += 'width:20%;';
	    col.editable = false;
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 2
	    col.header = 'Account No';
	    col.datafield = 'AccountNo';
	    col.align = 'left';
	    col.styles.cell += 'width:15%';
	    col.styles.datafield += 'width:15%;';
	    col.editable = false;
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 4
	    col.align = 'left';
	    col.header = 'Bank Name';
	    col.datafield = 'BankName';
	    col.editable = false;
        col.styles.cell += 'width:15%;';
        col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 5
	    col.header = 'Routing No';
	    col.datafield = 'RoutingNo';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:11%';
	    col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 6
	    col.header = 'Account Type';
	    col.datafield = 'AccountType';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:9%;padding-right:25px;padding-left:25px;';	
	    col.styles.datafield += 'width:5%;';    		
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 7
	    col.header = 'ACH Response Code';
	    col.datafield = 'ACHResponseCode';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:8%;padding-right:25px;padding-left:25px;';
	    col.styles.datafield += 'width:8%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 8
	    col.header = 'Deposit Amount';
	    col.datafield = 'DepositAmount';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:15%;padding-right:25px;padding-left:25px;';
	    col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 9
	    col.align = 'center';
	    col.header = 'Validated';
	    col.datafield = 'IsValidated';		    
        col.type = 'check';
        col.value = 'Edit';
        col.onclick='validateCheckbox';
        col.editable = true;
        col.styles.header += 'width:100%;';
        col.styles.cell += 'width:5%;padding-right:25px;padding-left:25px;';
        col.styles.datafield += 'width:5%;';
        gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 10
	    col.align = 'left';
	    col.header = 'Skip ACH';
	    col.datafield = 'SkipACH';		    
        col.type = 'check';
        col.onclick='validateCheckbox';
        col.editable = true;
        col.styles.header += 'width:100%;';
        col.styles.datafield += 'width:5%;';
        col.styles.cell += 'width:5%;padding-right:20px;padding-left:20px;';
        gv.addColumn(col);
    }    
    {
	    var col = new datagrid_column(); // 11
	    col.align = 'left';
	    col.header = 'Deleted';
	    col.datafield = 'Deleted';		    
        col.type = 'check';
        col.editable = true;
        col.styles.header += 'width:100%;';
        col.styles.datafield += 'width:5%;';
        col.styles.cell += 'width:5%;padding-right:20px;padding-left:20px;';
        gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 12
	    col.align = 'left';
	    col.header = 'User Deleted';
	    col.datafield = 'UserDeleted';		    
        col.type = 'check';
        col.editable = false;
        col.styles.header += 'width:100%;';
        col.styles.datafield += 'width:5%;';
        col.styles.cell += 'width:5%;padding-right:20px;padding-left:20px;';
        gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 12
	    col.align = 'left';
	    col.header = 'Validated By';
	    col.datafield = 'ValidatedBy';		    
        col.editable = false;
        col.styles.cell += 'width:15%;';
        col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 12
	    col.align = 'left';
	    col.header = 'PNB Code';
	    col.datafield = 'PNBCode';		    
        col.editable = false;
        col.styles.cell += 'width:15%;';
        col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
   
    // for footer
        gv.footer.edit_button.none = false;            
        gv.footer.edit_button.style += 'background-color: lavender;'; 
        gv.footer.edit_button.onclick = '';              
        gv.footer.save_button.none = false; 
        gv.footer.save_button.onclick = 'grid_edit';  
        gv.footer.save_button.style += 'background-color: lavender;';  
        gv.footer.cancel_button.none = false;
        gv.footer.cancel_button.style += 'background-color: lavender;'; 
        gv.row.normal_style = 'background-color: #f7ffff;';   
        
        //rows per page to display
        gv.rows_page = 8;                   
        return gv;
}

/*
|---------------------------------------------------------------------------------|
|--##-------##---###-------##-------######---####-------###----########---#####---|
|---##-----##---#---#------##---------##-----##--#-----#---#------##------##------|
|----##---##---#######-----##---------##-----##---#---#######-----##------#####---|
|-----##-##---##-----##----######-----##-----##--#---##-----##----##------##------|
|------###---##-------##---######---######---####---##-------##---##------#####---|
|---------------------------------------------------------------------------------|
*/

function validateCheckbox(args)
{
    var _IsValidated        =   null;
    var _IsSkipped          =   null;
    var _ArgId              =   "";
    var _IsValidatedId      =   "";
    var _IsSkippedId        =   "";
    var x                   =   args.id.split('_');
    var boxChecked          =   x[2];
    _ArgId                  =   x[3];
    _IsValidatedId          =   'tblReportLookup_cb_IsValidated_' + _ArgId;
    _IsSkippedId            =   'tblReportLookup_cb_SkipACH_' + _ArgId;
    
    //Check if IsValidated is checked
    if(document.getElementById(_IsValidatedId).outerHTML.indexOf("type=checkbox CHECKED") >= 0)
    {
        _IsValidated = true;
    }
    else
    {
        _IsValidated = false;
    }
    
    //Check if IsSkipped is checked
    if(document.getElementById(_IsSkippedId).outerHTML.indexOf("type=checkbox CHECKED") >= 0)
    {
        _IsSkipped = true;
    }
    else
    {
        _IsSkipped = false;
    }
    
     //IF isvalidated click
    if(boxChecked == 'IsValidated')
    {
        //If IsValidated is unchecked AND IsSkipped is checked,
        //UNCHECK BOTH        
        if (_IsSkipped == true && _IsValidated == false)
        {
            var str = document.getElementById(_IsSkippedId).outerHTML;
            var replaceStr = 'type=checkbox '
            document.getElementById(_IsSkippedId).outerHTML = str.replace('type=checkbox CHECKED ',replaceStr);
            return;
        }
     }      
    //IF isskipped click
    if(boxChecked == 'SkipACH')
    {
        //If IsSkipped is checked AND Isvalidated is unchecked,
        //CHECK BOTH.
        if (_IsSkipped == true && _IsValidated == false)
        {
            var str = document.getElementById(_IsValidatedId).outerHTML;
            var replaceStr = 'type=checkbox CHECKED '
            document.getElementById(_IsValidatedId).outerHTML = str.replace('type=checkbox ',replaceStr);
            return;
        }
    }
    return;
}

function checkinput()
{
}
