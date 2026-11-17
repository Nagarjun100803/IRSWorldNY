var uRole = '';
var uLoc = '';

function execute_ajaxgridview_command(param)
{
    try
    {
        if(epsco_proc){};
    }
    catch(e)
    {
        if (epsco_trans != null && epsco_trans != "undefined")
        {
         epsco_trans.do_ajaxgridview(param);
        }
        if (epsco_client != null && epsco_client != "undefined")
        {
            epsco_client.do_ajaxgridview(param);
        }
    }
}


function epsco_commit_response(param)
{
    if (epsco_trans != null && epsco_trans != "undefined")
    {
        epsco_trans.epsco_commit_response(param);
    }
}


function epsco_trans()
{
    this.RemitterCIFID     = null;
    this.RemitterSeqID     = null;
    this.RemitterName      = null;
    this.BeneficiaryName   = null;
    this.BeneficiarySEQID  = null;
    this.BeneficiaryCIFID  = null;
    this.BeneAddressSEQID  = null;
    this.BeneAddressCount  = null;
    this.BeneAddressRow    = 0;
    this.BeneContactSEQID  = null;
    this.BeneContactCount  = null;
    this.BeneContactRow    = 0;
    this.BeneEmailSEQID    = null;
    this.BeneEmailCount    = null;
    this.BeneEmailRow      = 0;
    this.BeneAccountSEQID  = null;
    this.Barcode           = null;
    this.grid_ajax_page          = "../AjxPage/ajaxgrid.aspx";
    this.RemittingCurrency = 0;
    this.TargetCurrency    = 0;
    
    this.ImplementationCode = null;
    this.ProductCode = null;
    
    this.page_initialize = function()
    {
        showPanelEP('divSearch');
        uRole = IRSWorld.EPSCO.EPSCOTransactions.getUserRole().value;
        uLoc = IRSWorld.EPSCO.EPSCOTransactions.getUserLoc().value;
    }
    
    this.commit_record = function()
    {    
        var commit = new epsco_commit();
        
        //PLEASE SET VALUE
        
		commit.RemitMethod = "" ;
		commit.RemitStationID = "" ;
		commit.RemittingBranch = "" ;
		commit.ReferenceNo = "" ;
		commit.ExtlReferenceNo = "" ;
		commit.Transtatus = "" ;
		commit.TranxDate = "" ;
		commit.ProductChargesID = "" ;
		commit.RemitterCIFID = "" ;
		commit.BeneficiaryCIFID = "" ;
		commit.AccountSEQID = "" ;
		commit.RemittingCurrency = "" ;
		commit.GrossRemAmount = "" ;
		commit.NetRemAmount = "" ;
		commit.NettoBeneCurrency = "" ;
		commit.NettoBeneAmount = "" ;
		commit.TotalLocalCharge = "" ;
		commit.TransactionSource = "" ;
		commit.ReceiptMode = "" ;
		commit.TransactionPurpose = "" ;
		commit.OtherPurpose = "" ;
		commit.SpecialMessage = "" ;
		commit.LocalCurrency= "" ;
		commit.ForexTargetToAccount = "" ;
		commit.ForexTargetToSource = "" ;
		commit.ForexTargetToUSD = "" ;
		commit.ForexTargetToPHP= "" ;
		commit.ForexTargetToEUR = "" ;        
		commit.ForexSourceToTarget= "" ;
		commit.ForexSourceToUSD= "" ;
		commit.ForexSourceToPHP= "" ;        
		commit.DatePostedOverseas= "" ;
		commit.DatePostedLocal= "" ;
		commit.SourceofFunds= "" ;
		commit.printed= "" ;
		commit.ChargeLocally= "" ;
		commit.total_backend_charges= "" ;        
		commit.total_benef_charges= "" ;
		commit.remitter_address_seq_id= "" ;
		commit.remitter_contact_seq_id= "" ;
		commit.beneficiary_address_seq_id= "" ;
		commit.beneficiary_contact_seq_id= "" ;
		
		commit.commit_record();
		        
    }
    
    this.epsco_commit_response = function(result)
    {
        if (result != null && result.value != null)
        {
            var com = result.value.split("~"); 
            
            if (com != null)
            {
                if (parseInt(com[0]) == 0)
                {
                    alert("Transaction saved.");
                }
                else
                {
                    alert("Error saving transaction: " + com[1]);
                }
            }
        }
        else
        {
            alert("Error saving transaction.");
        }
    }

    this.search_remitter = function()
    {
        hidePanelEP('divSearch');
        var firstname = document.getElementById('txtSFirstName').value;
        var lastname = document.getElementById('txtSLastName').value;
        var middlename = document.getElementById('txtSMiddleName').value;
        var remCIFID = document.getElementById('txtSCIFID').value;
        var oldCIFID = '';//document.getElementById('txtOldRemitterCIFID').value;
        if (firstname != '' || lastname != '' || middlename != '' || remCIFID !='')
        {
            var SQL = 'sp_CIFSearch~1~1~' +  lastname + '~' + firstname + '~' + middlename + '~' + remCIFID + '~' + oldCIFID + '~' + uRole + '~' + uLoc + '~';
            
            this.ajax_grid = new grid_selector();
            this.grid_ajax_query = SQL;
            
            if (this.grid_ajax_query != null)
            {
                this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 3, this.SEQ_ID );                     
            }
        }
        
        
    }
    
    this.do_ajax = function(parameter)
    {
        this.ajax_grid.process_ajax(parameter);
        this.call_record_loader(parameter);
    }
    
    this.call_record_loader = function(parameter)
    {        
        var command_param = null;
        
        if (parameter != null && parameter != "undefined")
        {
            command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    this.RemitterSeqID = command_param[1];                    
                    this.popRemitterDetail()                    
                }
                
                this.ajax_grid.process_ajax(parameter);
            }
            
        }        
    }
    
    this.popRemitterDetail = function()
    {
        this.RemitterCIFID = document.getElementById("txtSCIFID").value;
        IRSWorld.EPSCO.EPSCOTransactions.getRemitterInfo(epsco_trans.RemitterCIFID, epsco_trans.popRemitterDetail_callback)
    }
    
    this.popRemitterDetail_callback = function(param)
    {
        var dt = null;
        var dt = param.value;
        if (dt != null && dt.Rows.length != 0)
        {
            hidePanelEP('divSearch');
            epsco_trans.RemitterCIFID = dt.Rows[0].CIFID.toString();
            epsco_trans.RemitterName = dt.Rows[0].LastName.toString() + ', ' + dt.Rows[0].FirstName.toString() + ' ' + dt.Rows[0].MiddleName.toString();
            
            document.getElementById('spnRemitterID').value = epsco_trans.RemitterCIFID;
            setText(document.getElementById('spnRemitterID'), epsco_trans.RemitterCIFID + " - " + epsco_trans.RemitterName);
            document.getElementById('spnRemitterName').value = epsco_trans.RemitterName;
            setText(document.getElementById('spnRemitterName'), epsco_trans.RemitterName);

            epsco_trans.popBeneDDL(epsco_trans.RemitterCIFID);
            
            document.getElementById('tblRemitterName').style.display = '';
            document.getElementById('divTransaction').style.display = '';
        }
    }
    
    this.popBeneDDL = function(param)
    {
        if (param != null)
        {
            var SQL = IRSWorld.EPSCO.EPSCOTransactions.getBeneDDLSQL(param).value;
            var dt = IRSWorld.EPSCO.EPSCOTransactions.getDataTable(SQL).value;
        
            if (dt != null && dt.Rows.length > 0)
            {
                epsco_trans.BeneficiarySEQID = dt.Rows[0].Code;
                epsco_trans.BeneficiaryName = dt.Rows[0].Description.toString();
                epsco_trans.getBenefInfo();
            }
        }
    }
    
    this.getBenefInfo = function()
    {
        //epsco_trans.clear_bene();
        //epsco_trans.BeneficiarySEQID = document.getElementById("ddlBeneficiary")[document.getElementById("ddlBeneficiary").selectedIndex].value;

        if (epsco_trans.BeneficiarySEQID != '' || epsco_trans.BeneficiarySEQID != null)
        {
            IRSWorld.EPSCO.EPSCOTransactions.getBeneInfo(epsco_trans.BeneficiarySEQID,epsco_trans.getBenefInfo_callback);
        }
    }
    
    this.getBenefInfo_callback = function(param)
    {
        var dt = null;
        if(param != undefined || param.value != null)
        {
            dt = param.value;
            if(dt != null && dt.Rows.length > 0)
            {
                epsco_trans.BeneficiaryCIFID = dt.Rows[0].CIFID.toString();
                setText(document.getElementById("lblBeneficiary"),dt.Rows[0].CIFID.toString() + " - " +  epsco_trans.BeneficiaryName);
                setText(document.getElementById("spnBeneficiaryLName"),dt.Rows[0].Lastname.toString());
                setText(document.getElementById("spnBeneficiaryFName"),dt.Rows[0].FirstName.toString());
                setText(document.getElementById("spnBeneficiaryMName"),dt.Rows[0].Middlename.toString());
                epsco_trans.getBeneAccounts();
                epsco_trans.getBeneAddress();
            }
        }
        else
        {
            alert('FAILED TO GET BENEFICIARY INFO.');
        }
    }
    
    this.getBeneAddress = function()
    {
        if (epsco_trans.BeneficiaryCIFID != null || epsco_trans.BeneficiaryCIFID != '')
        {
            IRSWorld.EPSCO.EPSCOTransactions.getBeneAddress(epsco_trans.BeneficiaryCIFID, epsco_trans.getBeneAddress_callback);
        }
    }
    
    this.getBeneAddress_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        if (dt != null && dt.Rows.length > 0)
        {
            if (dt.Rows.length > 1)
            {
                document.getElementById("btnALeft").disabled = false
                document.getElementById("btnARight").disabled = false
            }
            else
            {
                document.getElementById("btnALeft").disabled = true
                document.getElementById("btnARight").disabled = true
            }
            epsco_trans.BeneAddressCount = dt.Rows.length;
            epsco_trans.BeneAddressSEQID = dt.Rows[epsco_trans.BeneAddressRow].SEQ_ID;
            setText(document.getElementById("spnAddress"),dt.Rows[epsco_trans.BeneAddressRow].Address.toString());
        }
        else
        {
            document.getElementById("btnALeft").disabled = true
            document.getElementById("btnARight").disabled = true
        }
        epsco_trans.getBeneContact();
    }
    
    this.getBeneContact = function()
    {
        if (epsco_trans.BeneficiaryCIFID != null || epsco_trans.BeneficiaryCIFID != '')
        {
            IRSWorld.EPSCO.EPSCOTransactions.getBeneContact(epsco_trans.BeneficiaryCIFID, epsco_trans.getBeneContact_callback);
        }
    }
    
    this.getBeneContact_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        if (dt != null && dt.Rows.length > 0)
        {
            if (dt.Rows.length > 1)
            {
                document.getElementById("btnCLeft").disabled = false
                document.getElementById("btnCRight").disabled = false
            }
            else
            {
                document.getElementById("btnCLeft").disabled = true
                document.getElementById("btnCRight").disabled = true
            }
            epsco_trans.BeneContactCount = dt.Rows.length;
            epsco_trans.BeneContactSEQID = dt.Rows[epsco_trans.BeneContactRow].SEQ_ID;
            setText(document.getElementById("spnContacts"),dt.Rows[epsco_trans.BeneContactRow].ContactNo.toString());
        }
        else
        {
            document.getElementById("btnCLeft").disabled = true
            document.getElementById("btnCRight").disabled = true
        }
        epsco_trans.getBeneEmail();
    }
    
    this.getBeneEmail = function()
    {
        if (epsco_trans.BeneficiaryCIFID != null || epsco_trans.BeneficiaryCIFID != '')
        {
            IRSWorld.EPSCO.EPSCOTransactions.getBeneEmail(epsco_trans.BeneficiaryCIFID, epsco_trans.getBeneEmail_callback);
        }
    }
    
    this.getBeneEmail_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        if (dt != null && dt.Rows.length > 0)
        {
            if (dt.Rows.length > 1)
            {
                document.getElementById("btnELeft").disabled = false
                document.getElementById("btnERight").disabled = false
            }
            else
            {
                document.getElementById("btnELeft").disabled = true
                document.getElementById("btnERight").disabled = true
            }
            epsco_trans.BeneContactCount = dt.Rows.length;
            epsco_trans.BeneContactSEQID = dt.Rows[epsco_trans.BeneEmailRow].SEQ_ID;
            setText(document.getElementById("spnEmailAddress"),dt.Rows[epsco_trans.BeneEmailRow].EmailAddress.toString());
        }
        else
        {
            document.getElementById("btnELeft").disabled = true
            document.getElementById("btnERight").disabled = true
        }
        epsco_trans.load_grid_viewer();
    }
    
    this.load_grid_viewer = function()
    {     
        this.commons = new common_method();
        this.commons = null;   
        this.grid = new grid_viewer();    
        this.grid.session_end = true;      
        this.grid.sql_source     = "sp_getBeneAccounts~" + epsco_trans.BeneficiaryCIFID + "~";
        this.grid.div_container  = document.getElementById("grdAccounts");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.open_grid_view();
        this.grid = null;
    }
    
    this.process_grid_viewer = function(parameter)
    {
        this.grid = new grid_viewer();        
        this.grid.sql_source     = "sp_getBeneAccounts~" + epsco_trans.BeneficiaryCIFID + "~";
        this.grid.div_container  = document.getElementById("grdAccounts");
        this.grid.datarow_column = 0;
        this.grid.page_size      = 10;
        this.grid.process_ajax(parameter);
        
        this.grid = null;
    }
    
    this.do_ajaxgridview = function(parameter)
    {         
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");
            
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    epsco_trans.BeneAccountSEQID = command_param[1];
                    epsco_trans.getBeneAccountDet();
                    return;
                }
                if (command_param[0] != "ajx_paging")
                {
                    this.process_grid_viewer(parameter);
                }
            }
        }
    }
    
    this.getBeneAccounts = function()
    {
        IRSWorld.EPSCO.EPSCOTransactions.getBeneAccounts(epsco_trans.BeneficiaryCIFID, epsco_trans.getBeneAccounts_callback);
    }
    
    this.getBeneAccounts_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        if (dt != null && dt.Rows.length > 0)
        {
            epsco_trans.BeneAccountSEQID = dt.Rows[0].SEQ_ID.toString();
            epsco_trans.getBeneAccountDet();
        }
    }
    
    this.getBeneAccountDet = function()
    {
        if (epsco_trans.BeneAccountSEQID != null || epsco_trans.BeneAccountSEQID != 0)
        {
            IRSWorld.EPSCO.EPSCOTransactions.getBeneAccountDetails(epsco_trans.BeneAccountSEQID, epsco_trans.getBeneAccountDet_callback);
        }
    }
    
    this.getBeneAccountDet_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        if (dt != null && dt.Rows.length > 0)
        {
            setText(document.getElementById("spnAccountNumber"),dt.Rows[0].AccountNo.toString());
            setText(document.getElementById("spnAccountType"),dt.Rows[0].AccountType.toString());
            setText(document.getElementById("spnPNBBranch"),dt.Rows[0].Branch.toString());
            setText(document.getElementById("spnProductCurrency"),dt.Rows[0].Currency.toString());
            document.getElementById('trAgent3').style.display = '';
            epsco_trans.TargetCurrency = dt.Rows[0].CurrencyValue.toString();
            epsco_trans.getRemittingCurrency();//epsco_trans.popDDL('dllRemittingCurrency','');
            //setText(document.getElementById("spnRemittingCurrency"),'');
            setText(document.getElementById("txtExchangeRate"),'');
            setText(document.getElementById("txtAmount"),'');
        }
    }
    
    this.getRemittingCurrency = function()
    {
        var SQL = IRSWorld.EPSCO.EPSCOTransactions.getRemCurrDDLSQL().value;
        var dt = IRSWorld.EPSCO.EPSCOTransactions.getDataTable(SQL).value;
        
        if (dt != null && dt.Rows.length > 0)
        {
            setText(document.getElementById("lblRemittingCurr"),dt.Rows[0].Description);
            epsco_trans.setRemittingCurrency();
        }
        
    }
    
    
    this.getCIFInfo = function(type, isnext)
    {
        switch(type)
        {
            case "Address":
                if (isnext == true)
                {
                    if (epsco_trans.BeneAddressRow != (epsco_trans.BeneAddressCount - 1))
                    {
                        epsco_trans.BeneAddressRow = epsco_trans.BeneAddressRow + 1;
                    }
                    else
                    {
                        epsco_trans.BeneAddressRow = 0;
                    }
                }
                else
                {
                    if (epsco_trans.BeneAddressRow != (epsco_trans.BeneAddressCount - 1))
                    {
                        if (epsco_trans.BeneAddressRow != 0)
                        {
                            epsco_trans.BeneAddressRow = epsco_trans.BeneAddressRow - 1;
                        }
                        else
                        {
                            epsco_trans.BeneAddressRow = (epsco_trans.BeneAddressCount - 1);
                        }
                    }
                    else
                    {
                        if (epsco_trans.BeneAddressRow != 0)
                        {
                            epsco_trans.BeneAddressRow = epsco_trans.BeneAddressRow - 1;
                        }
                        else
                        {
                            epsco_trans.BeneAddressRow = (epsco_trans.BeneAddressCount - 1);
                        }
                    }
                }
                IRSWorld.EPSCO.EPSCOTransactions.getCIFInFo(type,epsco_trans.getBeneAddress_callback);
                break;
            case "Email":
                if (isnext == true)
                {
                    if (epsco_trans.BeneEmailRow != (epsco_trans.BeneEmailCount - 1))
                    {
                        epsco_trans.BeneEmailRow = epsco_trans.BeneEmailRow + 1;
                    }
                    else
                    {
                        epsco_trans.BeneEmailRow = 0;
                    }
                }
                else
                {
                    if (epsco_trans.BeneEmailRow != (epsco_trans.BeneEmailCount - 1))
                    {
                        if (epsco_trans.BeneEmailRow != 0)
                        {
                            epsco_trans.BeneEmailRow = epsco_trans.BeneEmailRow - 1;
                        }
                        else
                        {
                            epsco_trans.BeneEmailRow = (epsco_trans.BeneEmailCount - 1);
                        }
                    }
                    else
                    {
                        if (epsco_trans.BeneEmailRow != 0)
                        {
                            epsco_trans.BeneEmailRow = epsco_trans.BeneEmailRow - 1;
                        }
                        else
                        {
                            epsco_trans.BeneEmailRow = (epsco_trans.BeneEmailCount - 1);
                        }
                    }
                }
                IRSWorld.EPSCO.EPSCOTransactions.getCIFInFo(type,epsco_trans.getBeneEmail_callback);
                break;
            case "Contacts":
                if (isnext == true)
                {
                    if (epsco_trans.BeneContactRow != (epsco_trans.BeneContactCount - 1))
                    {
                        epsco_trans.BeneContactRow = epsco_trans.BeneContactRow + 1;
                    }
                    else
                    {
                        epsco_trans.BeneContactRow = 0;
                    }
                }
                else
                {
                    if (epsco_trans.BeneContactRow != (epsco_trans.BeneContactCount - 1))
                    {
                        if (epsco_trans.BeneContactRow != 0)
                        {
                            epsco_trans.BeneContactRow = epsco_trans.BeneContactRow - 1;
                        }
                        else
                        {
                            epsco_trans.BeneContactRow = (epsco_trans.BeneContactCount - 1);
                        }
                    }
                    else
                    {
                        if (epsco_trans.BeneContactRow != 0)
                        {
                            epsco_trans.BeneContactRow = epsco_trans.BeneContactRow - 1;
                        }
                        else
                        {
                            epsco_trans.BeneContactRow = (epsco_trans.BeneContactCount - 1);
                        }
                    }
                }
                IRSWorld.EPSCO.EPSCOTransactions.getCIFInFo(type,epsco_trans.getBeneContact_callback);
                break;
        }
    }
    
    this.setRemittingCurrency = function()
    {
        epsco_trans.RemittingCurrency = 4;//HKD
        epsco_trans.getExchangeRate();
    }
    
    this.getExchangeRate = function()
    {
        if (epsco_trans.RemittingCurrency != 0 && epsco_trans.TargetCurrency != 0)
        {
            IRSWorld.EPSCO.EPSCOTransactions.getExchangeRates(epsco_trans.RemittingCurrency, epsco_trans.TargetCurrency, epsco_trans.getExchangeRate_callback);
        }
    }
    
    this.getExchangeRate_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        if (dt != null && dt.Rows.length > 0)
        {
            setText(document.getElementById("txtExchangeRate"),dt.Rows[0].SourceToTarget.toString());
        }
        else
        {setText(document.getElementById("txtExchangeRate"),'0.00');}
    }
    
    this.callSearch = function()
    {
        if (window.event.keyCode == 13) // Enter key
        {
            epsco_trans.popRemitterDetail();
        }
    }
    
    this.popDDL = function(DDL, REFID)
    {
        var SQL =  epsco_trans.getDDLSQL(DDL, REFID);
        var dt;  
        var dt2;
        DDL = document.getElementById(DDL);
        epsco_trans.cleanDDL(DDL);
       
        /*if(DDL.id == "ddlRemCountry")
        {
            dt = IRSWorld.EPSCO.EPSCOTransactions.getDDLCountry(SQL).value;
        }
        else
        {*/
            dt = IRSWorld.EPSCO.EPSCOTransactions.getDataTable(SQL).value;
        //}
        
        if (dt != null && dt.Rows.length > 0)
        {            
            for (var i = 0; i < dt.Rows.length; i++)
            {
                if (i == 0)
                {
                    var opt2 = document.createElement("option");
                    opt2.text = '';
                    opt2.value = '';
                    DDL.options.add(opt2);
                }
                var opt = document.createElement("option");                 
                opt.text = dt.Rows[i].Description.toString();
                opt.value = dt.Rows[i].Code.toString();
                DDL.options.add(opt);
            }      
        }
    }
    
    this.cleanDDL = function(DDL)
    {        
        DDL.length = 0;
    }
    
    this.getDDLSQL = function(DDL, RefID)
    {
        var SQL;
        if (DDL == 'ddlBeneficiary')
        {
            SQL = IRSWorld.EPSCO.EPSCOTransactions.getBeneDDLSQL(RefID).value;
        }
        else if (DDL == 'ddlRemittingCurrency')
        {
            SQL = IRSWorld.EPSCO.EPSCOTransactions.getRemCurrDDLSQL().value;
        }
        else if (DDL == 'ddlRemCountry')
        {
            SQL = IRSWorld.EPSCO.EPSCOTransactions.getRemCountryDDLSQL().value;
        }
        else if (DDL == 'ddlBenBranch')
        {
            SQL = IRSWorld.EPSCO.EPSCOTransactions.getBenBranchDDLSQL().value;
        }
        else if (DDL == 'ddlEnrollingBranch')
        {
            SQL = IRSWorld.EPSCO.EPSCOEnrollment.getEnrollingBranchDDLSQL().value;
        }
        return SQL;
    }
    
    this.autoFormatCurrency = function(fld, milSep, decSep, e)
    {
        var sep = 0;
        var key = "";
        var i = j = 0;
        var len = len2 = 0;
        var strCheck = "0123456789";
        var aux = aux2 = "";
        var whichCode = (window.Event) ? e.which : e.keyCode;

        if (whichCode == 13) return true; // Enter
        if (whichCode == 8) return true; // Delete
        if (whichCode == 9) return true; // Tab
        if (whichCode == 11) return true; // Tab
        key = String.fromCharCode(whichCode); // Get key value from key code
        if (strCheck.indexOf(key) == -1) return false; // Not a valid key
        len = fld.value.length;
        
        for(i = 0; i < len; i++)
        if ((fld.value.charAt(i) != "0") && (fld.value.charAt(i) != decSep)) break;
        aux = "";
        for(; i < len; i++)
        if (strCheck.indexOf(fld.value.charAt(i))!=-1) aux += fld.value.charAt(i);
        aux += key;
        len = aux.length;
        if (len == 0) fld.value = "";
        if (len == 1) fld.value = "0" + decSep + "0" + aux;
        if (len == 2) fld.value = "0" + decSep + aux;
        if (len > 2) 
        {
            aux2 = '';
            for (j = 0, i = len - 3; i >= 0; i--) 
            {
                if (j == 3) 
                {
                    aux2 += milSep;
                    j = 0;
                }
                aux2 += aux.charAt(i);
                j++;
            }
            fld.value = "";
            len2 = aux2.length;
            for (i = len2 - 1; i >= 0; i--)
            fld.value += aux2.charAt(i);
            fld.value += decSep + aux.substr(len - 2, len);
        }
        return false;
    }
    
    this.clear_bene = function()
    {
        epsco_trans.BeneficiarySEQID  = null;
        epsco_trans.BeneficiaryCIFID  = null;
        epsco_trans.BeneAddressSEQID  = null;
        epsco_trans.BeneAddressCount  = null;
        epsco_trans.BeneContactSEQID  = null;
        epsco_trans.BeneContactCount  = null;
        epsco_trans.BeneEmailSEQID    = null;
        epsco_trans.BeneEmailCount    = null;
        epsco_trans.BeneAccountSEQID  = null;
        epsco_trans.RemittingCurrency = 0;
        epsco_trans.TargetCurrency    = 0;
        
        setText(document.getElementById("spnBeneficiaryLName"),"");
        setText(document.getElementById("spnBeneficiaryFName"),"");
        setText(document.getElementById("spnBeneficiaryMName"),"");
        setText(document.getElementById("spnAddress"),"");
        setText(document.getElementById("spnContacts"),"");
        setText(document.getElementById("spnEmailAddress"),"");
        setText(document.getElementById("spnAccountNumber"),"");
        setText(document.getElementById("spnAccountType"),"");
        setText(document.getElementById("spnPNBBranch"),"");
        setText(document.getElementById("spnProductCurrency"),"");
    }
    //-----------------------------------------------------------
    this.initCDet = function()
    {
        
        this.getCBankChargeID();
    
        var amt = parseFloat(document.getElementById("txtAmount").value.replace(",",""));
        var xrate = parseFloat(document.getElementById("txtExchangeRate").value.replace(",",""));
        var recamt = parseFloat(document.getElementById("txtAmount").value.replace(",",""));
        var netamt = (amt*xrate) + parseFloat(document.getElementById("txtCBankChargeAmount").value.replace(",",""));
        
        if(this.RemittingCurrency!=1)
        {
            if(this.TargetCurrency!=1)
            {this.getPHPExchangeRate();}
            else
            {var amttophp = amt * xrate;}
        }
        else
        {var amttophp = amt;}

        setText(document.getElementById("txtCExchangeRate"),document.getElementById("txtExchangeRate").value);
        setText(document.getElementById("txtCGrossAmount"),roundoff(amt*xrate));
        setText(document.getElementById("txtCNetAmount"),roundoff(netamt));
        setText(document.getElementById("txtCNetToBenefAmount"),roundoff(recamt));
        setText(document.getElementById("txtCPHPConvertedAmount"),roundoff(amttophp));
        
    }
       
    this.getPHPExchangeRate = function()
    {
        IRSWorld.EPSCO.EPSCOTransactions.getExchangeRates(epsco_trans.RemittingCurrency, 1, epsco_trans.getPHPExchangeRate_callback);
    }
    
    this.getPHPExchangeRate_callback = function(param)
    {
        var dt = null;
        dt = param.value;
        if (dt != null && dt.Rows.length > 0)
        {
            var xrate = parseFloat(dt.Rows[0].SourceToTarget.toString());
        }
        else
        {var xrate = 0;}
        
        var amt = parseFloat(document.getElementById("txtAmount").value.replace(",",""));
        setText(document.getElementById("txtCPHPConvertedAmount"),roundoff(amt*xrate));
    }
    
    this.getCBankChargeID = function()
    {
        IRSWorld.EPSCO.EPSCOTransactions.getBankChargeID(this.RemitterCIFID, this.getCBankChargeID_callback);
    }
    
    this.getCBankChargeID_callback = function(param)
    {
        var dt = null;
        dt = param.value;

        if (dt != null && dt.Rows.length > 0)
            {
                var chrgamt = floatParse(IRSWorld.EPSCO.EPSCOTransactions.getCharges(dt.Rows[0].SEQ_ID.toString(),epsco_trans.TargetCurrency.toString()));
                setText(document.getElementById("txtCBankChargeAmount"),chrgamt);
            }
        else
            {setText(document.getElementById("txtCBankChargeAmount"),"undefined");}
    }
    //-----------------------------------------------------------
}


function showPanelEP(divName)
{
    var div = document.getElementById(divName);
    
    divmask = new maskpanel();
    divmask.show();
    
    document.getElementById(divName).style.display = "";
    
    if(div.offsetHeight < document.documentElement.clientHeight / 3)
    {
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (document.documentElement.clientHeight / 3)) + 'px';
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
    }
    else
    {
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + 'px';
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + 'px';
    }
}

function hidePanelEP(divName)
{
    document.getElementById(divName).style.display = "none";
    if(document.getElementById("divEnrollees").display != "")
    {
        document.getElementById("xdivmasking").style.display = "none";
        document.getElementById("xmaskframe").style.display = "none";
    }
    //divmask.hide();  
}

function roundoff(num)
{
    return (Math.round(num*100)/100);
}

function epsco_commit()
{
	this.RemitMethod          = 99; // pls. put the right remitmethod
	this.SEQ_ID               = 0;
	this.DateImplemented      = null;
	this.DateSent             = null;
	this.printed              = null;    
	this.RemitStationID       = null;
	this.RemittingBranch      = null;
	this.ReferenceNo          = null;
	this.ExtlReferenceNo      = null;
	this.Transtatus           = null;
	this.TranxDate            = null;
	this.ProductChargesID     = null;
	this.RemitterCIFID        = null;
	this.BeneficiaryCIFID     = null;
	this.AccountSEQID         = null;
	this.RemittingCurrency    = null;
	this.GrossRemAmount       = null;
	this.NetRemAmount         = null;
	this.NettoBeneCurrency    = null;
	this.NettoBeneAmount      = null;
	this.TotalLocalCharge     = null;
	this.TransactionSource    = null;
	this.ReceiptMode          = null;
	this.TransactionPurpose   = null;
	this.OtherPurpose         = null;
	this.SpecialMessage       = null;
	this.LocalCurrency        = null;
	this.ForexTargetToAccount = null;
	this.ForexTargetToSource  = null;
	this.ForexTargetToUSD     = null;
	this.ForexTargetToPHP     = null;
	this.ForexTargetToEUR     = null;
	this.ForexSourceToTarget  = null;
	this.ForexSourceToUSD     = null;
	this.ForexSourceToPHP     = null;
	this.DatePostedOverseas   = null; 
	this.DatePostedLocal      = null; 
	this.SourceofFunds        = null;
	this.ChargeLocally        = null;
	this.total_backend_charges      = null;
	this.total_benef_charges        = null;    
	this.remitter_address_seq_id    = null;
	this.remitter_contact_seq_id    = null;
	this.beneficiary_address_seq_id = null;
	this.beneficiary_contact_seq_id = null;
    

	this.commit_record = function()
	{
		IRSWorld.EPSCO.epsco_commit.insert_record(this.construct_column_array, this.commit_record_callback);
	}

	this.commit_record_callback = function(result)
	{
	    if (result != null && result.value != null)
	    {
	        var i = result.value.split("~"); eval("javascript:epsco_commit_response(i);");	        
	    }
	}

	this.construct_column_array = function()
	{
		var data_row = new Array();

		data_row[data_row.length] = "SEQ_ID~"                  + this.SEQ_ID;
		data_row[data_row.length] = "RemitMethod~"             + this.RemitMethod;
		data_row[data_row.length] = "RemitStationID~"          + this.RemitStationID;
		data_row[data_row.length] = "RemittingBranch~"         + this.RemittingBranch;
		data_row[data_row.length] = "ReferenceNo~"             + this.ReferenceNo;
		data_row[data_row.length] = "ExtlReferenceNo~"         + this.ExtlReferenceNo;
		data_row[data_row.length] = "Transtatus~"              + this.Transtatus;
		data_row[data_row.length] = "TranxDate~"               + this.TranxDate;
		data_row[data_row.length] = "ProductChargesID~"        + this.ProductChargesID;
		data_row[data_row.length] = "RemitterCIFID~"           + this.RemitterCIFID;
		data_row[data_row.length] = "BeneficiaryCIFID~"        + this.BeneficiaryCIFID;
		data_row[data_row.length] = "AccountSEQID~"            + this.AccountSEQID;
		data_row[data_row.length] = "RemittingCurrency~"       + this.RemittingCurrency;
		data_row[data_row.length] = "GrossRemAmount~"          + this.GrossRemAmount;
		data_row[data_row.length] = "NetRemAmount~"            + this.NetRemAmount;
		data_row[data_row.length] = "NettoBeneCurrency~"       + this.NettoBeneCurrency;
		data_row[data_row.length] = "NettoBeneAmount~"         + this.NettoBeneAmount;
		data_row[data_row.length] = "TotalLocalCharge~"        + this.TotalLocalCharge;
		data_row[data_row.length] = "TransactionSource~"       + this.TransactionSource;
		data_row[data_row.length] = "ReceiptMode~"             + this.ReceiptMode;
		data_row[data_row.length] = "TransactionPurpose~"      + this.TransactionPurpose;
		data_row[data_row.length] = "OtherPurpose~"            + this.OtherPurpose;
		data_row[data_row.length] = "SpecialMessage~"          + this.SpecialMessage;
		data_row[data_row.length] = "LocalCurrency~"           + this.LocalCurrency;
		data_row[data_row.length] = "ForexRemittingToAccount~" + this.ForexTargetToAccount;
		data_row[data_row.length] = "ForexRemittingToLocal~"   + this.ForexTargetToSource;
		data_row[data_row.length] = "ForexRemittingToUSD~"     + this.ForexTargetToUSD;
		data_row[data_row.length] = "ForexRemittingToPHP~"     + this.ForexTargetToPHP;
		data_row[data_row.length] = "ForexRemittingToEUR~"     + this.ForexTargetToEUR;        
		data_row[data_row.length] = "ForexLocalToRemitting~"   + this.ForexSourceToTarget;
		data_row[data_row.length] = "ForexLocalToUSD~"         + this.ForexSourceToUSD;
		data_row[data_row.length] = "ForexLocalToPHP~"         + this.ForexSourceToPHP;        
		data_row[data_row.length] = "DatePostedOverseas~"      + this.DatePostedOverseas;
		data_row[data_row.length] = "DatePostedLocal~"         + this.DatePostedLocal;
		data_row[data_row.length] = "SourceofFunds~"           + this.SourceofFunds;
		data_row[data_row.length] = "printed~"                 + this.printed;
		data_row[data_row.length] = "ChargeLocally~"           + this.ChargeLocally;
		data_row[data_row.length] = "total_backend_charges~"   + this.total_backend_charges;        
		data_row[data_row.length] = "total_benef_charges~"     + this.total_benef_charges;
		data_row[data_row.length] = "RemitterAddressSeqID~"    + this.remitter_address_seq_id;
		data_row[data_row.length] = "RemitterContactSeqID~"    + this.remitter_contact_seq_id;
		data_row[data_row.length] = "BeneficiaryAddressSeqID~" + this.beneficiary_address_seq_id;
		data_row[data_row.length] = "BeneficiaryContactSeqID~" + this.beneficiary_contact_seq_id;

		return data_row.join("|");
	}
}