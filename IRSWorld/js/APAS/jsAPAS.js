var s='';
var moduleid=1; /*default to apas*/

function execute_ajx_command(param)
{
    if (cls_APAS != null && cls_APAS != "undefined")
    { cls_APAS.do_gridsearch(param); }
    
}

function fixht()
{
    document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
    document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
}

function fixmain()
{
    var brmain = false;
    try
    {
         brmain = parent.is_branchsetup_main();
    }
    catch(e){}
    
    if (!brmain)
    {
        document.getElementById('theBody').onresize = fixht;
        document.getElementById('divBody').style.overflow = 'auto';
        
        fixht();
    }        
}  

function page_setup()
{
    /* Load Required Id*/
        cls_APAS.BranchCode = IRSWorld.APAS.APASMain.getClientDataBranchCode().value;
        cls_APAS.load_required_ids(cls_APAS.BranchCode);   
    /* END */
    
    /*Hide div Result */     
        controls.hide_div('div_results',true);
    /* END */
    
    /*Get transaction status*/
        cls_APAS.get_apas_trxnstat();
    /* END */
    
    /*Get Products*/
    cls_APAS.get_products();
    /*END*/
    
    /*GET CIF ID UPON LOADING */
        cls_APAS.get_client_data_details();
    /*END*/
    
    /*FIELD VISIBILITY*/
        cls_APAS.hide_field_per_agent_login(cls_APAS.CIFClass);
    /*END*/
    
    /*ON BEHALF BRANCHES*/
        cls_APAS.getOnBehalfPayoutPartners();
    /*END*/
    
    fixmain();
}

function APAS()
{
    var cls = this;
    this.ajax_grid      = null;
    this.ajax_container = null;
    this.strsearch      = null;
    this.url            = "../AjxPage/ajaxgrid.aspx";
    this.ctrl           = null;       
    this.SEQ_ID         = null;
    this.trxn_SEQ_id    = null;
    this.Pay_Branch_code= null;
    this.MC_No          = null;
    this.Remarks        = null;
    this.ProcessedBy    = null;
    this.DateProcessed  = null;    
    this.DatePaid       = null;
    this.param          = " where SEQ_id = ";
    this.val            = null;  
    this.rembankcode    = null;
    this.trxn_seq_id    = null;
    this.approver       = null;   
    this.trxn_stat      = null; 
    this.IDSEQ          = null;
    this.IDNo           = null;
    this.isLock         = null;
    this.LockBy         = null;
    this.CIFID          = null;
    this.BranchCode     = null;
    this.UserName       = null;
    this.BranchName     = null;
    this.RoleID         = null;
    this.CIFClass       = null;
    this.BranchCode     = null;
    this.LockById       = null;
    this.isCancel       = null;
    this.CancelBy       = null;
    this.isAmend        = null;
    this.AmendBy        = null;
    this.DispapprovedCancel = null;
    this.CurrencyCode   = null;
    this.WithAmendments = null;
    this.CancelledAmendments  = null;
    this.ApprovedAmendments =null;
    this.tempBranch = null;
    this.MotherBranchCode = null;
    this.ProductType    = null;
    this.OnHold     = null;
    this.ORDAccountNo = null;
    this.BranchAcct = null;
    this.RefNo = null;
    this.Amount = null;
    this.PartnerAcct = null;
    this.ErrMsg = null;
    this.a_seq_id = null;
    this.isHeadOffice = null;
    this.command_values = null;
    this.parameter = null;
    
    this.get_apas_trxnstat = function()
    { IRSWorld.APAS.APASMain.get_apas_trxnstat(this.get_apas_trxnstat_callback); }    
    this.get_apas_trxnstat_callback = function(ds)
    {
        var comm_method = new common_method();
        if (ds != null && ds != "undefined")
        { comm_method.to_load_dropdownlist("cmb_trxn_stat", ds.value, "FldCode", "FldDescription", null);return; }
    }
    /*Code here*/
    this.get_products = function()
    {
        var DDL = document.getElementById('ddlProductCode');
      
        var y = document.createElement('option');
        y.text = 'APAS'
        y.value = '1';
        DDL.options.add(y);
        
        var z = document.createElement('option');
        z.text = 'PAW'
        z.value = '2';
        DDL.options.add(z);
        
        if(this.BranchCode == IRSWorld.APAS.APASMain.getORDBranchCode().value)
        {
            document.getElementById('cmbProductCode').style.display = 'block';
            document.getElementById('tdProductCode').style.display = 'block';
        }
    }
    
    
    
    this.set_dateprocessed = function()
    { IRSWorld.APAS.APASMain.get_time_zone(this.set_dateporcessed_callback) }
    
    this.set_dateporcessed_callback = function(result)
    { cls.DateProcessed = dateToString(result.value); }
    
    this.pay = function()
    { return this.do_apas_trnx("UPDATE"); }  
    
    this.do_apas_trnx = function(instruction)
    { 
        if(this.trxn_stat == 1)
        {
            if(document.getElementById('chkOnBehalf').checked)
            {
                if(document.getElementById('ddlPartners').value == 'null' | document.getElementById('ddlPartners').value == '')
                {
                    alert('Please select Payout-Partner');
                    return;
                }
            }
        
        
            ajax_show_status(true);
            document.getElementById('payout').disabled = 'disabled';
            s = this.construct_column_array();
            
            /*Preparation for Fund Transfer*/
            this.ORDAccountNo = IRSWorld.APAS.APASMain.getORDAccountNo(this.CurrencyCode,moduleid).value;
            this.BranchAcct = IRSWorld.APAS.APASMain.getBranchAccountNo(this.CurrencyCode,this.Pay_Branch_code).value;
            this.RefNo = getText(document.getElementById('RefNoTheirs'));
            this.Amount = getText(document.getElementById('Amount'));
            
            /*When PNB Branch*/
            if(this.CIFClass != IRSWorld.APAS.APASMain.isAgent().value)
            {
                IRSWorld.APAS.APASMain.fundTransfer(this.ORDAccountNo,this.BranchAcct,this.RefNo,this.Amount,this.SEQ_ID,this.do_apas_trnx_callback)
                //APASMain.do_process_apas(instruction,s,cls.SEQ_ID,this.do_apas_trnx_callback); 
                
            }
            else
            {
                if(this.isHeadOffice)
                {
                    this.PartnerAcct = IRSWorld.APAS.APASMain.getPartnerAccountNo(this.Pay_Branch_code,this.CurrencyCode).value;
                }
                else
                {
                    this.PartnerAcct = IRSWorld.APAS.APASMain.getPartnerAccountNo(this.MotherBranchCode,this.CurrencyCode).value;
                }
                
                IRSWorld.APAS.APASMain.fundTransfer(this.ORDAccountNo,this.PartnerAcct,this.RefNo,this.Amount,this.SEQ_ID,this.do_apas_trnx_callback)
                //APASMain.do_process_apas('UPDATE',s,cls.SEQ_ID,this.do_apas_trnx_callback); //temp
                //this.locked_unlocked_tranx('UPDATE',false);
            }
        }
    }
    
    /*Code here*/
    
    
    this.do_apas_trnx_callback = function(result)
    {
        if (result.value != null && result.value != undefined)
        {
            var isProcessed=false;
            if(result.value)
            {
                var success = IRSWorld.APAS.APASMain.do_process_apas('UPDATE',s.toString(),cls.SEQ_ID.toString()).value;
                if(success != '')
                {
                    IRSWorld.APAS.APASMain.ShowReport(getText(document.getElementById('RefNoTheirs')),cls_APAS.ShowReport_callback);
                }
            }
            else
            {
                //isProcessed = APASMain.do_process_apas('UPDATE',s,cls.SEQ_ID).value;
                //APASMain.ShowReport(document.getElementById('RefNoTheirs').innerText,ShowReport_callback);
                this.ErrMsg = 'Error in posting CASA. Please call PNB-Overseas Remittance Division at ' + IRSWorld.APAS.APASMain.ordtrunkline().value + ' for more details.';
                alert(this.ErrMsg);
                cls_APAS.audit_log('INSERT',this.ErrMsg);
                ajax_show_status(false);
                controls.hide_div('div_results',true);                 
                controls.hide_div('DIV_Search',false); 
            }
           
            //APASMain.ShowReport(document.getElementById('RefNoTheirs').innerText,ShowReport_callback);
        }        
    }
    
    this.after_process_callback = function(result)
    {
        
        
        if (result.value != null && result.value != 'undefined')
        {
            
        }
    }
    
    
    this.ShowReport_callback = function(result)
    {
            //?&mergeparam=' + result.value + ';' + result.value +  '&paramvalues=' + result.value + ';' + result.value + '&exporttype="PDF" &autoprint=false',1);
           alert("Transaction successfully processed.");
           ajax_show_status(false);
           cls_APAS.locked_unlocked_tranx('UPDATE',false);
           controls.hide_div('div_results',true);                 
           controls.hide_div('DIV_Search',false);            
           window.open('../Reports/Pages/Report_viewer.aspx');
    }
    
    
    this.locked_unlocked_tranx = function(instruction,bool)
    {
        if(cls.SEQ_ID != null)
        {
            var data_row = new Array();
            data_row[data_row.length] = "isLocked~"               + bool; 
            data_row[data_row.length] = (bool == false) ? "LockedBy~"               +  '' : "LockedBy~"               + this.UserName;
            data_row[data_row.length] = (bool == false) ? "LockedDate~"             +  '' : "LockedDate~"             + dateToString(IRSWorld.APAS.APASMain.get_branch_localtime(this.BranchCode).value);
            var s = data_row.join('|');
            IRSWorld.APAS.APASMain.do_process_apas(instruction,s,cls.SEQ_ID.toString(),this.locked_unlocked_tranx_callback); 
        }
    }
    
    this.locked_unlocked_tranx_callback = function(result)
    {
        if (result != null && result != 'undefined')
        {
        
        }        
    }
    
    this.hold_unhold_tranx = function(instruction,bool)
    {
        if(cls.SEQ_ID != null)
        {
            var str = (bool) ? 'Hold' : 'Unhold';
            if(confirm('Are you sure you want to ' + str + ' this transaction'))
            {
                var data_row = new Array();
                data_row[data_row.length] = "onHold~"               + bool; 
                data_row[data_row.length] = (bool == false) ? "DateHold~"             +  '' : "DateHold~"             + dateToString(APASMain.get_branch_localtime(this.BranchCode).value);
                data_row[data_row.length] = "HoldBy~"               + this.UserName;
                var s = data_row.join('|');
                IRSWorld.APAS.APASMain.do_process_apas(instruction,s,cls.SEQ_ID,this.hold_unhold_tranx_callback);
                alert('Transaction successfully '+ str ); 
            }
        }
    }
    
    this.hold_unhold_tranx_callback = function(result)
    {
        if (result.value != null && result.value != 'undefined')
        {
            cls_APAS.locked_unlocked_tranx('UPDATE',false);
            controls.hide_div('div_results',true);                 
            controls.hide_div('DIV_Search',false);
        }        
    }
    
    this.audit_log = function(instruction,errorlog)
    {
        var data_row = new Array();
        data_row[data_row.length] = "ErrorDesc~"              +  errorlog; 
        data_row[data_row.length] = "ErroDate~"              +  dateToString(IRSWorld.APAS.APASMain.get_time_zone().value);
        data_row[data_row.length] = "UserID~"                 +  this.UserName;
        data_row[data_row.length] = "BranchCode~"             +  this.BranchCode;
        data_row[data_row.length] = "ModuleNm~"               +  'APAS/PAW TRANSACTION'
        var s = data_row.join('|');
        IRSWorld.APAS.APASMain.do_process_apas(instruction,s,"AUDIT"); 
    }
    
    
    this.get_client_data_details = function()
    {
        this.CIFID = IRSWorld.APAS.APASMain.get_irsclient_data('CIF').value;
        IRSWorld.APAS.APASMain.get_irsclient_data('LOCATION',this.branch_details_callback);
        //APASMain.CIFClass(this.CIFID,this.cifclass_details_callback);
        this.CIFClass = IRSWorld.APAS.APASMain.CIFClass(this.CIFID).value;
        this.RoleID = IRSWorld.APAS.APASMain.RoleID().value;
        
        
        this.get_username(this.CIFID);
        this.get_branchname(this.BranchCode);
    }
    

    
    this.cif_details_callback = function(result)
    {
        if(result.value != null && result.value != 'undefined')
        {
            this.CIFID = result.value
        }
    }
    
    this.branch_details_callback = function(result)
    {
        if(result.value != null && result.value != 'undefined')
        {
            this.BranchCode = result.value
        }
    }
    
    this.cifclass_details_callback = function(result)
    {
        if(result.value != null && result.value != 'undefined')
        {
            this.CIFClass = result.value
        }
    }
    
    
    
    this.get_username = function(cifid)
    {
        this.UserName = IRSWorld.APAS.APASMain.get_irsclient_data('UID').value;
    }
    
    this.get_branchname = function(brcode)
    {
        this.BranchName = IRSWorld.APAS.APASMain.getBranchName(brcode).value;
    }
    
    this.getOnBehalfPayoutPartners = function()
    {
        IRSWorld.APAS.APASMain.getAllOnBehalfBranches(this.BranchCode,this.getOnBehalfPayoutPartners_callback)
    }
    
    this.getOnBehalfPayoutPartners_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlPartners", result.value, "BranchCode", "BranchName");
        }
    }
    
    
    
    this.clientdata = function()
    { IRSWorld.APAS.APASMain.clientdata(this.clientdata_callback); }
    
    this.clientdata_callback = function(result)
    {
        if(result.value != null && result.value != 'undefined')
        {
            s = result.value;
            cls.approver           = s[3];
            cls.ProcessedBy        = s[4];
            cls.DatePaid           = dateToString(s[5]);
            cls.Pay_Branch_code    = s[1];         
            cls_APAS.pay();
        }
    }
    
    this.prepare_data = function()
    {   
        cls.MC_No          = document.getElementById("txtMC").value;
        cls.Remarks        = document.getElementById("txtRemarks").value;        
        cls.IDNo           = document.getElementById('txtIdNum').value;
        cls.IDSEQ          = document.getElementById('ddlIDs').value;
    }   
    
    this.hide_field_per_agent_login = function(cifclass)
    {
        this.isHeadOffice = IRSWorld.APAS.APASMain.isHeadOffice(this.BranchCode).value;
        if(cifclass == IRSWorld.APAS.APASMain.isAgent().value)
        {
            /*Search Screen*/
            document.getElementById('FldDatePaid').style.display = 'none';
            document.getElementById('tdDatePaid').style.display = 'none';
            document.getElementById('FldBenefLastName').style.display = 'none';
            document.getElementById('tdBeneLname').style.display = 'none';
            document.getElementById('FldBenefFirstName').style.display = 'none';
            document.getElementById('tdBeneFname').style.display = 'none';
            document.getElementById('FldBenefMiddleName').style.display = 'none';
            document.getElementById('tdBeneMname').style.display = 'none';
            document.getElementById('FldAmount').style.display = 'none';
            document.getElementById('tdAmount').style.display = 'none';
            document.getElementById('FldPayingBranch').style.display = 'none';
            document.getElementById('tdPayBr').style.display = 'none';
            document.getElementById('FldTransactionStatus').style.display = 'none';
            document.getElementById('tdTransactionStatus').style.display = 'none';
            document.getElementById('tdCoverFrom').style.display = 'none';
            document.getElementById('tdCoverTo').style.display = 'none';
            document.getElementById('tdCoverFromDesc').style.display = 'none';
            document.getElementById('txtCoverToDesc').style.display = 'none';
            
            /*Main Screen*/
            document.getElementById('ORC').style.display = 'none';
            document.getElementById('Purpose').style.display = 'none';
            document.getElementById('RemID').style.display = 'none';
            document.getElementById('RemBank').style.display = 'none';
            document.getElementById('ProdCode').style.display = 'none';
            document.getElementById('RateUsed').style.display = 'none';
            document.getElementById('EVAA').style.display = 'none';
            document.getElementById('NSVS').style.display = 'none';
            document.getElementById('trxn_stat').style.display = 'none';
            document.getElementById('tdMC').style.display = 'none';
            document.getElementById('tdRemarks').style.display = 'none';
            
            document.getElementById('FldORCDiv').style.display = 'none';
            document.getElementById('FldPurpose').style.display = 'none';
            document.getElementById('FldRemitterID').style.display = 'none';
            document.getElementById('FldRemittingBank').style.display = 'none';
            document.getElementById('FldProductCode').style.display = 'none';
            document.getElementById('FldRateUsed').style.display = 'none';
            document.getElementById('FldEVAA').style.display = 'none';
            document.getElementById('FldAccount').style.display = 'none';
            document.getElementById('FldTranxStatus').style.display = 'none';
            document.getElementById('FldMCNo').style.display = 'none';
            document.getElementById('FldRemarks').style.display = 'none';
            document.getElementById('fldOnBehalf').style.display = 'none';
            document.getElementById('lblOnBehalf').style.display = 'none';
            if(!this.isHeadOffice)
            {   
                document.getElementById('fldOnBehalf').style.display = 'none';
                document.getElementById('lblOnBehalf').style.display = 'none';
            }
            else
            {
                document.getElementById('fldOnBehalf').style.display = 'block';
                document.getElementById('lblOnBehalf').style.display = 'block';
            }
        }
        else
        {
            if(!this.isHeadOffice)
            {   
                document.getElementById('fldOnBehalf').style.display = 'none';
                document.getElementById('lblOnBehalf').style.display = 'none';
            }
            else
            {
                document.getElementById('fldOnBehalf').style.display = 'block';
                document.getElementById('lblOnBehalf').style.display = 'block';
            }
        }
        
    }
    
    this.verify_password = function(uid,pwd)
    {
        if (uid.length == 0 && pwd.length == 0)
        {
            this.ErrMsg = 'ERROR: Please enter User id & password';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            return false;
        }
        else if (uid.length == 0)
        {
            this.ErrMsg = 'ERROR: Please enter User id'
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            return false;
        }
        else if (pwd.length == 0)
        {
            this.ErrMsg = 'ERROR: Please enter password';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            return false;
        }
        
        IRSWorld.APAS.APASMain.verify_password(uid,pwd,this.verify_password_callback);
    }
    
    this.verify_password_callback = function(result)
    {   
        if (result.value != null && result.value != "undefined")        
        {
            s = result.value[0];
            if (s[0])
            {
                approval_window.close_modal_window();
                cls_APAS.clientdata();
            }
            else{alert(s[1]);}
        }
    }    
    
    
    this.check_uncheck_onbehalf = function()
    {
        if(document.getElementById('chkOnBehalf').checked == true)
        {
            document.getElementById('lblPartners').style.display = 'block';
            document.getElementById('fldPartners').style.display = 'block';
            document.getElementById('ddlPartners').style.display = 'block';
        }
        else
        {
            document.getElementById('lblPartners').style.display = 'none';
            document.getElementById('fldPartners').style.display = 'none';
            document.getElementById('ddlPartners').style.display = 'none';
        }
    }
    
    this.check_if_tranx_is_locked = function(seq_id)
    {
        var ds = IRSWorld.APAS.APASMain.CheckIfLockTransaction(seq_id).value;
        this.isLock  = (ds.Tables[0].Rows[0].isLocked==null) ? '' : ds.Tables[0].Rows[0].isLocked;
        this.LockBy = (ds.Tables[0].Rows[0].LockedBy==null) ? '' : ds.Tables[0].Rows[0].LockedBy;
        this.LockById = (ds.Tables[0].Rows[0].LockedByID==null) ? '' : ds.Tables[0].Rows[0].LockedByID;
    } 
    
    this.check_if_tranx_is_for_cancellation = function(seq_id)
    {
        var ds = IRSWorld.APAS.APASMain.CheckIfCancelTransaction(seq_id).value;
        this.isCancel = (ds.Tables[0].Rows[0].WithCancellation == null) ? '' : ds.Tables[0].Rows[0].WithCancellation;
        this.CancelBy = (ds.Tables[0].Rows[0].CancelledBy == null) ? '' : ds.Tables[0].Rows[0].CancelledBy;
        this.DispapprovedCancel = (ds.Tables[0].Rows[0].DisapprovedCancellation==null) ? '' : ds.Tables[0].Rows[0].DisapprovedCancellation;
    }
    
    this.check_if_tranx_with_amendments = function(seq_id)
    {
        var ds = IRSWorld.APAS.APASMain.CheckIfWithAmendments(seq_id).value;
        this.WithAmendments = (ds.Tables[0].Rows[0].WithAmendments == null) ? '' : ds.Tables[0].Rows[0].WithAmendments;
        this.CancelledAmendments = (ds.Tables[0].Rows[0].CancelledAmendments == null) ? '' : ds.Tables[0].Rows[0].CancelledAmendments;
        this.ApprovedAmendments = (ds.Tables[0].Rows[0].ApprovedAmendments==null) ? '' : ds.Tables[0].Rows[0].ApprovedAmendments;
        this.AmendBy = (ds.Tables[0].Rows[0].AmendedBy==null) ? '' : ds.Tables[0].Rows[0].AmendedBy;
    }
    
    this.check_hold_tranx = function(seq_id)
    {
        this.OnHold = IRSWorld.APAS.APASMain.isHold(seq_id).value;
    }
    
    
    this.construct_column_array = function()
    {
        var data_row = new Array();
        this.prepare_data();
        
        data_row[data_row.length] = "SEQ_ID~"               + cls.SEQ_ID;     
        //data_row[data_row.length] = "Pay_Branch_code~"      + (document.getElementById('chkOnBehalf').checked) ? "Pay_Branch_code~" + document.getElementById('ddlPartners').value : cls.Pay_Branch_code;
        if(document.getElementById('chkOnBehalf').checked)
        {
            if(cls.isHeadOffice)
            {
                data_row[data_row.length] = "Pay_Branch_code~" + document.getElementById('ddlPartners').value;
            }
            else
            {
                data_row[data_row.length] = "Pay_Branch_code~" + cls.Pay_Branch_code;
            }
        }
        else
        {
            data_row[data_row.length] = "Pay_Branch_code~" + cls.Pay_Branch_code;
        } 
        
        data_row[data_row.length] = "MC_No~"                + cls.MC_No;
        data_row[data_row.length] = "Remarks~"              + cls.Remarks;
        data_row[data_row.length] = "ProcessedBy~"          + cls.ProcessedBy;
        data_row[data_row.length] = "DateProcessed~"        + cls.DateProcessed;
        data_row[data_row.length] = "ApprovedBy~"           + cls.approver;
        data_row[data_row.length] = "DatePaid~"             + cls.DatePaid;
        data_row[data_row.length] = "trxn_stat~2";   //2 - means processed transaction
        data_row[data_row.length] = "ID_SEQ_ID~"            + cls.IDSEQ;   
        data_row[data_row.length] = "IDNo~"                 + cls.IDNo;
        data_row[data_row.length] = (document.getElementById('chkOnBehalf').checked) ? "OnBehalf~" + document.getElementById('ddlPartners').value : "OnBehalf~" + '';
        return data_row.join("|");
    } 
    
    /* USER DEFINED METHOD */
    this.do_validation = function()
    {
        var isAgent = IRSWorld.APAS.APASMain.isAgent().value;
        var trxn_limit; 
        var can_payout;
        var approvers = IRSWorld.APAS.APASMain.getApproverRoles().value;
        var ordbranch = IRSWorld.APAS.APASMain.getORDBranchCode().value;
        
        this.check_if_tranx_is_locked(this.command_values);
        this.check_if_tranx_is_for_cancellation(this.command_values);
        this.check_if_tranx_with_amendments(this.command_values);     
        this.check_hold_tranx(this.command_values);
        
        
        if(this.isLock != true & this.trxn_stat != 2) 
        {              
            this.locked_unlocked_tranx('UPDATE',true);
        }
        
        if(this.LockById != this.UserName & this.trxn_stat == 1 & this.isLock == 1)
        {
            this.tempBranch = IRSWorld.APAS.APASMain.getBranchNamePerUser(this.LockBy).value;
            this.ErrMsg = 'The Transaction is currently processed by '+ this.LockBy +' , APAS of '+ this.tempBranch +'';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            //this.locked_unlocked_tranx('UPDATE',false); //unlock the transaction
            return;
        }
        else if(this.trxn_stat == 3 & this.CIFClass == isAgent)
        {
            this.ErrMsg = 'Request PNB-ORD for Activation of this transaction';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            this.locked_unlocked_tranx('UPDATE',false);
            return;
        }
        else if(this.OnHold == true & this.RoleID != 12) //12 means ORD Approver
        {
            this.ErrMsg = 'Transaction is currently on-hold'; 
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            this.locked_unlocked_tranx('UPDATE',false);
            return;
        }
        
        else if(this.isCancel == 1 & this.DispapprovedCancel == 0) //if cancellation is disapproved continue with processing APAS
        {
            this.tempBranch = IRSWorld.APAS.APASMain.getBranchNamePerUser(this.CancelBy).value;
            this.ErrMsg = 'The Transaction is currently being processed for cancellation by '+ this.CancelBy +' , APAS of '+ this.tempBranch +'';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            this.locked_unlocked_tranx('UPDATE',false); //unlock the transaction
            return;
        }
        
        else if(this.WithAmendments == 1 & this.ApprovedAmendments == 0) //if cancellation is disapproved continue with processing APAS
        {
            this.tempBranch = IRSWorld.APAS.APASMain.getBranchNamePerUser(this.AmendBy).value;
            this.ErrMsg = 'The Transaction is currently being processed for amendments by '+ this.AmendBy +' , APAS of '+ this.tempBranch +'';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            this.locked_unlocked_tranx('UPDATE',false); //unlock the transaction
            return;
        }
        /*Get Mother branchCode*/
            if(this.CIFClass == isAgent)
            {
                this.isHeadOffice = IRSWorld.APAS.APASMain.isHeadOffice(this.BranchCode).value;
                this.MotherBranchCode = IRSWorld.APAS.APASMain.getMotherBranchCode(this.BranchCode).value;
                can_payout = IRSWorld.APAS.APASMain.getAgentPayoutCurrency(this.CurrencyCode,this.BranchCode,this.isHeadOffice).value;
                if(this.MotherBranchCode == '') //kapag parent na yung branchcode pull the transaction limit on db
                {
                    trxn_limit = IRSWorld.APAS.APASMain.getTransactionLimit(this.BranchCode,this.CurrencyCode).value;
                }
                else //kapag hindi motherbranchcode gamitin ang branch code ng parent
                {
                    trxn_limit = IRSWorld.APAS.APASMain.getTransactionLimit(this.MotherBranchCode,this.CurrencyCode).value;
                }
                
            }
        /*End*/
          
        
        if(can_payout == false & this.CIFClass == isAgent)
        {
            this.ErrMsg = 'Please be informed the Beneficiary that ' + getText(document.getElementById('CurrencyCode')) + ' Remittances cannot be claimed thru your Pay-out Branches. Please instruct them to proceed to nearest PNB Branch';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            this.locked_unlocked_tranx('UPDATE',false); //unlock the transaction
            return;
        }
        else if(parseFloat(getText(document.getElementById('Amount'))) > parseFloat(trxn_limit) & this.CIFClass == isAgent)
        {
            this.ErrMsg = 'Please be informed the Beneficiary that remittances higher than' + getText(document.getElementById('CurrencyCode')) + ' ' + trxn_limit + ' cannot be claimed thru your Payout Branches. Please instruct them to proceed to nearest PNB Branch';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            this.locked_unlocked_tranx('UPDATE',false); //unlock the transaction
            return;
        } 
        
        /*PNB Branch*/
        var arr = new Array();
        var strCurrencies;
        var hit=0;
        var currvalue = IRSWorld.APAS.APASMain.getCurrencyCode(this.CurrencyCode).value;
        if(this.CIFClass != isAgent)
        {
            strCurrencies = IRSWorld.APAS.APASMain.getAllowedCurrencies(this.BranchCode).value;
            arr = strCurrencies.split(',');
            for(var i=0; i <= arr.length - 1;i++)
            {
                if(this.CurrencyCode == arr[i])
                {
                    hit += 1;
                }
            }
            if(this.BranchCode != IRSWorld.APAS.APASMain.getORDBranchCode().value)
            {    
                if(hit >0)
                {
                    //Continue
                }
                else
                {
                    this.ErrMsg = 'This branch did not allowed to payout ' + currvalue + ' currency';
                    alert(this.ErrMsg); 
                    this.audit_log('INSERT',this.ErrMsg);
                    this.locked_unlocked_tranx('UPDATE',false); //unlock the transaction
                    return;
                }
            }
        }
        
        
        
        
        this.ajax_grid.process_ajax(this.parameter); // FROM TEMPLATE (process_ajax)  
        controls.hide_div('DIV_Search',true);
        controls.hide_div('div_results',false);
        
    }
    
    
    
    
    this.do_gridsearch = function (parameter)
    {
        
        
        if (parameter != null && parameter != "undefined")
        {
            var command_param = parameter.split("$");

            if (command_param.length > 0)
            {
                if(command_param[0] == "ajx_paging")
                {
                    this.ajax_grid.process_ajax(parameter);
                    return;
                }
                if(command_param[0] == "ajx_search")
                {
                    this.ajax_grid.process_ajax(parameter);
                    return;
                }
                
                if (command_param[0] == "ajx_selected")
                {
                    
                    var command_values = command_param[1].split("~"); // TO GET FIELD VALUE                                            
                    this.command_values = command_values[0];
                    this.parameter = parameter;
                    this.get_data(command_values);
                               
                } 
                
                               
            }
        }
    }
    
    /*        
        Added Date  : Jan 2010
        Purpose     : For APAS trxn search
    */
    this.search = function()
    {   
        if(this.getStatus())
        {
            alert('Transaction already PAID');
            return;
        }
        else if(document.getElementById('txtRefNo').value == '' & this.CIFClass == IRSWorld.APAS.APASMain.isAgent().value)
        {
            this.ErrMsg = 'Please input reference number';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            return;
        }
        
        
        
        if (this.validate_input())
        {         
            this.setsearch();
            if(this.strsearch != '')
            {
                var sql = "sp_APAS_search~" + this.strsearch;
                this.ajax_grid = new grid_selector();                                                     
                this.ajax_grid.open_selection_grid(this.url, sql, 0, this.ajax_container );
            }
        }
        else{alert(this.error_input);}
    }
    
    
    this.getStatus = function()
    {
        var refno = document.getElementById('txtRefno').value;
        var stat = IRSWorld.APAS.APASMain.getTranxStatus(refno).value;
        if (this.CIFClass == IRSWorld.APAS.APASMain.isAgent().value & stat == 2) // 2- Processed Transaction
        {
            return true;
        }
        return false;
    }
       
    
    this.setsearch = function()
    {
        var vr = "where";
        var i = 0;
        var DatePaid    =   document.getElementById("txtDatePaid").value;        
        var Refno       =   document.getElementById("txtRefno").value;
        var BeneLname   =   document.getElementById("txtBeneLname").value;
        var BeneFname   =   document.getElementById("txtBeneFname").value;
        var BeneMname   =   document.getElementById("txtBeneMname").value;
        var Amount      =   document.getElementById("txtAmount").value;
        var PayBr       =   document.getElementById("txtPayBr").value;
        var trnx_stat   =   document.getElementById("cmb_trxn_stat").value;
        var paw_roles   =   IRSWorld.APAS.APASMain.isPawUser(this.RoleID).value;
        var ismatch     =   false;
        var paw_roles   = paw_roles.split(',');
        var ordbranch = IRSWorld.APAS.APASMain.getORDBranchCode().value;
        var productcode = document.getElementById('ddlProductCode').value;
        var from        = document.getElementById('txtPeriodFrom').value;
        var to          = document.getElementById('txtPeriodTo').value;
        
        
        if(DatePaid == '' & Refno == '' & BeneLname == '' & BeneFname == '' & BeneMname == '' & Amount == '' & PayBr == '' & trnx_stat == 'null' & from == '' & to == '')
        {
            this.ErrMsg = 'Please input at least one on the search criteria';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
            this.strsearch = '';
            return;
        }
        
        
        
        for(var r=0;r <= paw_roles.length - 1;r++)
        {
            if(this.RoleID == paw_roles[r])
            {
                ismatch = true;
            }
        }
        
        if(this.BranchCode != ordbranch)
        {
            if(ismatch)
            {
                if (i != 0)
                { vr += " and "; }
                i = 1;vr += "  ProductCode = 24";  //paw transaction
                moduleid = 2;
            }
            else
            {
                if (i != 0)
                { vr += " and "; }
                i = 1;vr += "  ProductCode in (27,255,7)";  //apas transaction
                moduleid = 1;
            }
        }
        else
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += "  ProductCode in (27,255,7,24)";  //apas transaction

        }
        
        if (Refno.length != 0)
        { 
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " ReferenceNo = '" + Refno + "'";
        }          
        if (DatePaid.length != 0)
        {                
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += "  convert(varchar|TranxDate|101) = '" + DatePaid + "'";  // piped used in convert(datetime|....) method & replaced (,) comma in SP.
        }           
        if (BeneLname.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " BeneFull_Name like '%" + BeneLname + "%'";
        }           
        if (BeneFname.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " BeneFull_Name like '%" + BeneFname + "%'";
        }                      
        if (BeneMname.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " BeneFull_Name like '%" + BeneFname + "%'";
        } 
        if (Amount.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " Amount = '" + Amount + "'";
        }            
        if (PayBr.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " PayBr like '%" + PayBr + "%'";
        }
        if (trnx_stat != "null" & trnx_stat != '5')
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " TransactionStatusCode = '" + trnx_stat + "'";
        }
        if (from.length != 0 & to.length != 0)
        {
            if (i != 0)
            { vr += " and "; }
            i = 1;vr += " (Implem_Date between '" + from + "' AND '"+ to +"')";
        }
//        if (from.length != & to.length != 0)
//        {
//            if (i != 0)
//            { vr += " and "; }
//            i = 1;
//            vr = " Implem_Date = '"+ to +"'";
//        }
        
        if(productcode.length != 0)
        {
            if(i != 0)
            { vr += " and "; }
            i = 1; 
            if(productcode == 1)
            {
                vr += " ProductCode in (27,255,7)";            
            }
            else if(productcode == 2)
            {
                vr += " ProductCode = 24";
            }
        } 
                                
        if (vr == "where")
        { vr = ""; }
        this.strsearch = vr;
    }      
    this.setdetails = function(dataset)
    {   
        var _roles = IRSWorld.APAS.APASMain.getApproverRoles().value;
        if (dataset != null && typeof(dataset) == "object" && dataset.Tables != null)
        {   
            document.getElementById('ImplemDate').innerHTML         = dateToString(dataset.Tables[0].Rows[0]['TranxDate']) == null ? '' : dateToString(dataset.Tables[0].Rows[0]['TranxDate']);
            document.getElementById('RefNoTheirs').innerHTML        = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo'];
            document.getElementById('RefNoOurs').innerHTML          = dataset.Tables[0].Rows[0]['ReferenceNo'] == null ? '' : dataset.Tables[0].Rows[0]['ReferenceNo']; 
            document.getElementById('ORC').innerHTML                = 'ORC';
            if((dataset.Tables[0].Rows[0]['LastNm'] != null | dataset.Tables[0].Rows[0]['LastNm'] != '')) //& dataset.Tables[0].Rows[0]['ApprovedAmendments'] == true)
            {
                document.getElementById('BenefLastNm').innerHTML           = dataset.Tables[0].Rows[0]['LastNm'];
                document.getElementById('BenefFirstNm').innerHTML          = dataset.Tables[0].Rows[0]['FirstNm'];
                document.getElementById('BenefMiddleNm').innerHTML         = dataset.Tables[0].Rows[0]['MiddleNm'];
            }
//            else
//            {
//                document.getElementById('BeneName').innerHTML           = dataset.Tables[0].Rows[0]['BeneFull_Name'];
//            }
            if((dataset.Tables[0].Rows[0]['BenefAddress'] != null | dataset.Tables[0].Rows[0]['BenefAddress'] != '') & dataset.Tables[0].Rows[0]['ApprovedAmendments'] == true)
            {
                document.getElementById('BeneAdd').innerHTML           = dataset.Tables[0].Rows[0]['BenefAddress'];
            }
            else
            {
                document.getElementById('BeneAdd').innerHTML            = dataset.Tables[0].Rows[0]['Bene_Address'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_Address'];
            }
            document.getElementById('BeneContactNo').innerHTML      = dataset.Tables[0].Rows[0]['Bene_ContactNum'] == null ? '' : dataset.Tables[0].Rows[0]['Bene_ContactNum'];
            document.getElementById('SpecialMSG').innerHTML         = dataset.Tables[0].Rows[0]['SpecialMessage'] == null ? '' : dataset.Tables[0].Rows[0]['SpecialMessage'];
            document.getElementById('Purpose').innerHTML            = dataset.Tables[0].Rows[0]['Purpose'] == null ? '' : dataset.Tables[0].Rows[0]['Purpose'];
            document.getElementById('RemID').innerHTML              = dataset.Tables[0].Rows[0]['RemitterID'];
            document.getElementById('RemName').innerHTML            = dataset.Tables[0].Rows[0]['RemFull_Name'];
            document.getElementById('RemAdd').innerHTML             = dataset.Tables[0].Rows[0]['Rem_Adress'] == null ? '' : dataset.Tables[0].Rows[0]['Rem_Adress'];
            document.getElementById('RemBank').innerHTML            = dataset.Tables[0].Rows[0]['RemittingBank'] == null ? '' : dataset.Tables[0].Rows[0]['RemittingBank'];
            document.getElementById('ProdCode').innerHTML           = dataset.Tables[0].Rows[0]['ProductName'] == null ? '' : dataset.Tables[0].Rows[0]['ProductName'];
            document.getElementById('CurrencyCode').innerHTML       = dataset.Tables[0].Rows[0]['Currency'] == null ? '' : dataset.Tables[0].Rows[0]['Currency'];
            document.getElementById('RateUsed').innerHTML           = formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['RateUsed'],3);
            document.getElementById('Amount').innerHTML             = formatNumber(dataset.Tables[0].Rows[0]['Amount'],2) == null ? '' : formatNumber(dataset.Tables[0].Rows[0]['Amount'],2);            
            document.getElementById('EVAA').innerHTML               = 'EVAA';
            document.getElementById('NSVS').innerHTML               = 'NSVS';
            document.getElementById('trxn_stat').innerHTML          = (dataset.Tables[0].Rows[0]['OnHold']) ? dataset.Tables[0].Rows[0]['TransactionStatus'] + '-On Hold' : dataset.Tables[0].Rows[0]['TransactionStatus'];
            cls.SEQ_ID                                              = dataset.Tables[0].Rows[0]['SEQ_id'];  
            cls.CurrencyCode                                        = dataset.Tables[0].Rows[0]['CurrencyCode'];           
            document.getElementById('txtRemarks').value             = dataset.Tables[0].Rows[0]['Remarks'] == null ? '' : dataset.Tables[0].Rows[0]['Remarks'];
            document.getElementById('txtMC').value                  = dataset.Tables[0].Rows[0]['MC_No'] == undefined ? '' : dataset.Tables[0].Rows[0]['MC_No'];
            document.getElementById('ddlIDs').value                 = dataset.Tables[0].Rows[0]['ID'];
            document.getElementById('txtIdNum').value               = dataset.Tables[0].Rows[0]['IDNo'] == null ? '' : dataset.Tables[0].Rows[0]['IDNo'];
            this.trxn_stat                                          = dataset.Tables[0].Rows[0]['TransactionStatusCode'] == null ? '' : dataset.Tables[0].Rows[0]['TransactionStatusCode'];    
            
            if (this.trxn_stat  == "1" | this.trxn_stat == 3) //OUTSTANDING TRANSACTION  & FOR DISPOSITION CAN AMEND             
            {
                /*Added May 13 2010 Michael Dizon*/
                if(this.BranchCode == IRSWorld.APAS.APASMain.getORDBranchCode().value & _roles.indexOf(this.RoleID) > -1)
                {
                    document.getElementById('tdpayout').style.display = 'none';
                    document.getElementById('tdBack').style.display = 'block';
                    document.getElementById('tdAmend').style.display = 'none';
                    document.getElementById('btnAmend').style.display = 'none';
                    document.getElementById('tdCancel').style.display = 'none';
                    document.getElementById('btnCancel').style.display = 'none';
                    document.getElementById('tdHold').style.display = 'block';
                    document.getElementById('btnHold').style.display = 'block';
                    document.getElementById('btnUnHold').style.display = 'block';
                    document.getElementById('tdUnHold').style.display = 'block';
                    
                    //Cancellation Button and Textfield
                    document.getElementById('btnSaveCancellation').style.display = 'none';
                    document.getElementById('lblCancelReason').style.display = 'none';
                    document.getElementById('txtCancelReason').style.display = 'none';
                    document.getElementById('Back').style.display = 'block';
                    document.getElementById('tdCancelReason').style.display = 'none';
                    
                    //For Saving Button
                    document.getElementById('tdSave').style.display = 'none';
                    
                    document.getElementById('txtRemarks').style.display = 'none';
                    document.getElementById('txtMC').style.display = 'none';
                    document.getElementById('ddlIDs').style.display = 'none';
                    document.getElementById('txtIdNum').style.display = 'none';
                    
                    document.getElementById('FldIDPresented').style.display = 'none';
                    document.getElementById('FldIDNumber').style.display = 'none';
                    document.getElementById('FldRemarks').style.display = 'none';
                    document.getElementById('FldMCNo').style.display = 'none';
                }
                
                /*End*/
                else if (this.BranchCode == IRSWorld.APAS.APASMain.getORDBranchCode().value) //if ORD
                {
                    document.getElementById('tdpayout').style.display = 'none';
                    document.getElementById('tdBack').style.display = 'block';
                    document.getElementById('tdAmend').style.display = 'block';
                    document.getElementById('btnAmend').style.display = 'block';
                    document.getElementById('tdCancel').style.display = 'block';
                    document.getElementById('btnCancel').style.display = 'block';
                    document.getElementById('tdHold').style.display = 'none';
                    document.getElementById('btnHold').style.display = 'none';
                    document.getElementById('btnUnHold').style.display = 'none';
                    document.getElementById('tdUnHold').style.display = 'none';
                    
                    //Cancellation Button and Textfield
                    document.getElementById('btnSaveCancellation').style.display = 'none';
                    document.getElementById('lblCancelReason').style.display = 'none';
                    document.getElementById('txtCancelReason').style.display = 'none';
                    document.getElementById('Back').style.display = 'block';
                    document.getElementById('tdCancelReason').style.display = 'none';
                    
                    //For Saving Button
                    document.getElementById('tdSave').style.display = 'none';
                    
                    document.getElementById('txtRemarks').style.display = 'none';
                    document.getElementById('txtMC').style.display = 'none';
                    document.getElementById('ddlIDs').style.display = 'none';
                    document.getElementById('txtIdNum').style.display = 'none';
                    
                    document.getElementById('FldIDPresented').style.display = 'none';
                    document.getElementById('FldIDNumber').style.display = 'none';
                    document.getElementById('FldRemarks').style.display = 'none';
                    document.getElementById('FldMCNo').style.display = 'none';
                    
                    
                    

                }
                
                if(_roles.indexOf(this.RoleID) > -1)
                {
                    controls.disable('txtRemarks');
                    controls.disable('txtMC');
                    controls.disable("payout");
                }
                else
                {
                    controls.enable('txtRemarks');
                    controls.enable('txtMC');
                    (this.trxn_stat == 1) ? controls.enable("payout") : controls.disable("payout"); //cannot payout when in disposition
                    controls.enable('ddlIDs');
                    controls.enable('txtIdNum');
                }
            }            
            else    
            {
                document.getElementById('tdpayout').style.display = 'block';
                document.getElementById('tdBack').style.display = 'block';
                document.getElementById('tdAmend').style.display = 'none';
                document.getElementById('btnAmend').style.display = 'none';
                document.getElementById('tdCancel').style.display = 'none';
                document.getElementById('btnCancel').style.display = 'none';
                document.getElementById('tdHold').style.display = 'none';
                document.getElementById('btnHold').style.display = 'none';
                document.getElementById('btnUnHold').style.display = 'none';
                document.getElementById('tdUnHold').style.display = 'none';
                
                controls.disable('txtRemarks');
                controls.disable('txtMC');
                controls.disable("payout");
                controls.disable('ddlIDs');
                controls.disable('txtIdNum');
                
                setText(document.getElementById('lblProcessedBy'), dataset.Tables[0].Rows[0]['ProcessedBy']);
                setText(document.getElementById('lblDateProc'), dateToString(dataset.Tables[0].Rows[0]['DateProcessed']));
                setText(document.getElementById('lblApprovedBy'),  dataset.Tables[0].Rows[0]['ApprovedBy']);
                setText(document.getElementById('lblDatePaid'), dateToString(dataset.Tables[0].Rows[0]['DatePaid']));
                this.open_processed_data_info('block');
            }
            cls_APAS.do_validation();
        }
    }
    this.get_data = function(val)
    {   
        this.val = this.param + "'" + val + "'";
        IRSWorld.APAS.APASMain.get_details(this.val, this.get_data_callback)
    }  
    
    this.get_data_callback = function(result)
    {
         if (result.value != null && result.value != "undefined")
        {            
            cls_APAS.setdetails(result.value);            
            fixmain();
        }     
        else
        {
            this.ErrMsg = 'No data found!';
            alert(this.ErrMsg);
            this.audit_log('INSERT',this.ErrMsg);
        }
    }    
    
    this.load_required_ids = function(branchcd)
    {
        IRSWorld.APAS.APASMain.getRequiredID(branchcd,this.load_required_ids_callback);
    }
    
    this.create_textbox = function()
    {
    
        /*CREATE DYNAMIC TEXTBOX*/
        var txtlname = document.createElement('input');
        var txtfname = document.createElement('input');
        var txtmname = document.createElement('input');
        
        var lname = document.getElementById('BenefLastNm');
        var fname = document.getElementById('BenefFirstNm');
        var mname = document.getElementById('BenefMiddleNm');
        
        //txt.value = '';
        txtlname.id = 'txtLastNm';
        txtlname.value = getText(document.getElementById('BenefLastNm'));
        lname.innerHTML = txtlname.outerHTML;
        
        txtfname.id = 'txtFirstNm';
        txtfname.value = getText(document.getElementById('BenefFirstNm'));
        fname.innerHTML = txtfname.outerHTML;
        
        txtmname.id = 'txtMiddleNm';
        txtmname.value= getText(document.getElementById('BenefMiddleNm'));
        mname.innerHTML = txtmname.outerHTML;
        
        
        var txtAdd = document.createElement('input');
        var nameAdd = document.getElementById('BeneAdd');
        
        //txtAdd.value = ''
        txtAdd.id = 'txtBeneAddress';
        txtAdd.value = getText(document.getElementById('BeneAdd'));
        nameAdd.innerHTML = txtAdd.outerHTML;
        

        var btn = document.createElement('input');
        //var name = document.getElementById('BeneName');
        var address = document.getElementById('BeneAdd');
        address.style.width = '100%';
        lname.style.width = '100%';
        fname.style.width = '100%';
        mname.style.width = '100%';

        
        /*SET VISIBILITY OF BUTTON*/
        document.getElementById('tdSave').style.display = 'block';
        document.getElementById('Save').style.display = 'block';
        document.getElementById('btnAmend').style.display = 'none';
        document.getElementById('tdCancel').style.display = 'none';
        document.getElementById('btnCancel').style.display = 'none';
        document.getElementById('Save').onclick = Function('cls_APAS.update_bene_info()');
    }
    
    this.set_cancellation_button = function(clicked)
    {
        if(clicked == 'Cancel')
        {
            document.getElementById('btnCancel').style.display = 'none';
            document.getElementById('btnSaveCancellation').style.display = 'block';
            document.getElementById('lblCancelReason').style.display = 'block';
            document.getElementById('txtCancelReason').style.display = 'block';
            document.getElementById('tdCancelReason').style.display = 'block';
            document.getElementById('tdAmend').style.display = 'none';
            document.getElementById('btnSaveCancellation').onclick = Function('cls_APAS.cancel_transaction();');
        }
        else
        {
            document.getElementById('btnCancel').style.display = 'block';
            document.getElementById('btnSaveCancellation').style.display = 'none';
            document.getElementById('lblCancelReason').style.display = 'none';
            document.getElementById('txtCancelReason').style.display = 'none';
            document.getElementById('tdCancelReason').style.display = 'none';
        }
    
    }
    
    
    this.update_bene_info = function()
    {
        if(confirm('Are you sure you want to amend this transaction?'))
        {
            var data_row = new Array();
            
            data_row[data_row.length] = "SEQ_ID~"               + cls.SEQ_ID;     
            //data_row[data_row.length] = "BenefName~"            + document.getElementById('txtBeneName').value;     
            data_row[data_row.length] = "BenefFirstNm~"         + document.getElementById('txtFirstNm').value;     
            data_row[data_row.length] = "BenefLastNm~"          + document.getElementById('txtLastNm').value;   
            data_row[data_row.length] = "BenefMiddleNm~"        + document.getElementById('txtMiddleNm').value;   
            data_row[data_row.length] = "BenefAddress~"         + document.getElementById('txtBeneAddress').value;
            data_row[data_row.length] = "WithAmendments~"       + true;
            data_row[data_row.length] = "AmendedBy~"            + this.UserName;
            data_row[data_row.length] = "DateAmended~"          + dateToString(APASMain.get_branch_localtime(this.BranchCode).value)
            data_row[data_row.length] = "ApprovedAmendments~"   + false;
            data_row[data_row.length] = "CancelledAmendments~"  + false;
            var s = data_row.join("|");
            IRSWorld.APAS.APASMain.do_process_apas('UPDATE',s,cls.SEQ_ID,this.update_bene_info_callback); 
        }
    }
    
    this.cancel_transaction = function()
    {
        if(confirm('Are you sure you want to cancel this transaction?'))
        {
            if(document.getElementById('txtCancelReason').value.length >0)
            {
                var data_row = new Array();
                
                data_row[data_row.length] = "CancelledBy~"          + this.UserName
                data_row[data_row.length] = "DateCancelled~"        + dateToString(APASMain.get_branch_localtime(this.BranchCode).value)
                data_row[data_row.length] = "CancelReason~"         + document.getElementById('txtCancelReason').value;
                data_row[data_row.length] = "WithCancellation~"         + true;
                data_row[data_row.length] = "ApprovedCancellation~"     + false;
                data_row[data_row.length] = "DisapprovedCancellation~"  + false;
                var s = data_row.join("|");
                IRSWorld.APAS.APASMain.do_process_apas('UPDATE',s,cls.SEQ_ID,this.update_bene_info_callback); 
            }
            else
            {
                this.ErrMsg = 'Please input the reason for cancellation';
                alert(this.ErrMsg);
                this.audit_log('INSERT',this.ErrMsg);
                return;
            }
        }
    }
    
    this.update_bene_info_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {            
            alert('Transaction now queued for approval');
            cls_APAS.locked_unlocked_tranx('UPDATE',false);
            controls.hide_div("div_results",true);
            controls.hide_div("DIV_Search",false);
            return;
        }   
    }
    
    this.open_processed_data_info = function(bool)
    {
        document.getElementById('tdProcessedBy').style.display = bool;
        document.getElementById('lblProcessedBy').style.display = bool;
        document.getElementById('tdDateProcessed').style.display = bool;
        document.getElementById('lblDateProc').style.display = bool;
        document.getElementById('tdApprovedBy').style.display = bool;
        document.getElementById('lblApprovedBy').style.display = bool;
        document.getElementById('tdDatePaid2').style.display = bool;
        document.getElementById('lblDatePaid').style.display = bool;
    }
    
    this.load_required_ids_callback = function(result)
    {
        if (result.value != null && result.value != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddlIDs", result.value, "SEQ_ID", "IDDescription");
        }
        else
        {
            
        }
    }
      
    this.validate_input = function()
    { 
	    this.commons     = new common_method();        
	    this.error_input = "";   
		
		
		if(document.getElementById("txtDatePaid").value != '' | document.getElementById("txtDatePaid").value != null)
		{
	        if (isValidDate(document.getElementById("txtDatePaid").value)[0] == false && document.getElementById("txtDatePaid").value.length !=0)
	        {           
		        this.error_input += "Invalid date Date Paid.\n";            
	        }	    	    
            if (this.error_input != '')
	        {   
		        return false;
	        }
	    }
	    if(isValidDate(document.getElementById("txtPeriodFrom").value)[0] == false & document.getElementById("txtPeriodFrom").value.length !=0)
        {
            this.error_input += "Invalid Period Date From";
            return false;
        }
        else if(isValidDate(document.getElementById("txtPeriodTo").value)[0] == false & document.getElementById("txtPeriodTo").value.length !=0)
        {
            this.error_input += "Invalid Period Covered To";
            return false;
        }
	    else if(document.getElementById("txtPeriodFrom").value.length > 0 & document.getElementById("txtPeriodTo").value.length <= 0)
	    {
            this.error_input += "Please input Period Covered To";
            return false;
	    }
	    else if(document.getElementById("txtPeriodTo").value.length > 0 & document.getElementById("txtPeriodFrom").value.length <= 0)
	    {
            this.error_input += "Please input Period Covered From";
            return false;
	    }
	    return true;     
    }  
}

function controls()
{
    this.hide_div = function(div_id, show)
    {
        if (!show)
        { document.getElementById(div_id).style.display = 'block'; }
        else
        { document.getElementById(div_id).style.display = 'none'; }
    }
    this.enable = function(oName)
    {
        var id = document.getElementById(oName);
        var cset = id.disabled;       
        id.disabled = cset == "false";
    }  
    this.disable = function(oName)
    {                 
        var id = document.getElementById(oName);
        var cset = id.disabled;       
        id.disabled = (cset == "false") ? "true" : "false";
    }   
}

function approval_window()
{
    this.outer_modal_div  = null;
    this.inner_modal_div  = null;
    this.commons          = null;     
    this.approver         = null;
    var html_string;   
     
    this.open_window_approval = function()
    {
        var ibol = confirm("Are you sure you want to process this?");
        if (ibol)
        {        
            this.open_modal_window();
            this.constract_window();
        }
    }    
    
    this.constract_window = function()
    {
        html_string = "<table id ='tbl_approval' width='100%' class='table_border'>";
        html_string += "<tr><td colspan='3' class='table_caption' style='text-align: center; width = 100%'>Enter Approver UserID &amp; Password</td></tr>";
        html_string += "<tr>                                                                    ";
        html_string += "        <td colspan='2' class='cell_caption' style='width: 50%'>        ";
        html_string += "            User ID :</td>                                              ";
        html_string += "        <td style='width: 50%' class='cell_desc'>                       ";
        html_string += "            <input id='txtuid' type='text'/></td>                       ";
        html_string += "</tr>                                                                   ";
        html_string += "<tr>                                                                    ";
        html_string += "        <td colspan='2' class='cell_caption' style='width: 50%'>        ";
        html_string += "            Password :</td>                                             ";
        html_string += "        <td style='width: 50%' class='cell_desc'>                       ";
        html_string += "            <input id='txtpwd' type='password'/></td>                   ";
        html_string += "</tr>                                                                   ";
        html_string += "<tr>                                                                    ";
        html_string += "        <td colspan='2' class='cell_caption' style='width: 50%'>        ";
        html_string += "            <input id='btn_approve' type='button' value='Approve' style='width: 50%' /></td>    ";  
        html_string += "<td style='width: 50%'>                                                                     ";
        html_string += "<input id='btn_cancel' type='button' value='Cancel' style='width: 50%' /></td>                 ";
        html_string += "</tr>";
        html_string += "</table>";

        document.getElementById("content_mod_div").innerHTML = html_string;
        
        var control_finder  = document.getElementById("content_mod_div");
        var btn_approve     = control_finder.document.getElementById("btn_approve");
        var btn_cancel      = control_finder.document.getElementById("btn_cancel");
        var i_cls           = this;        
        control_finder.document.getElementById('txtuid').focus();      
        cls_APAS.set_dateprocessed();
            
        btn_approve.onclick = function()
        {
            var txtuid          = control_finder.document.getElementById("txtuid").value;
            var txtpwd          = control_finder.document.getElementById("txtpwd").value;
            this.approver       = txtuid;
            cls_APAS.verify_password(txtuid,txtpwd);            
        }
        btn_cancel.onclick = function()
        { approval_window.close_modal_window(); }
    }
    this.open_modal_window = function()
	{
        this.load_modal_css();
		this.outer_modal_div           = document.createElement("div");
		this.outer_modal_div.id        = "outer_mod_div";
		this.outer_modal_div.className = "outer_div";		
		
		document.body.style.margin = "0px";
		document.body.appendChild(this.outer_modal_div);
		document.getElementById("outer_mod_div").style.height = screen.height;		
		
		this.construct_div_content();
        this.commons = new common_method();
        this.commons.to_show_hide_combobox(false);
        this.commons = null;
	}

	this.close_modal_window = function()
	{
	    this.commons = new common_method();
        this.commons.to_show_hide_combobox(true);
        this.commons = null;

		if(this.outer_modal_div != null){document.body.removeChild(this.outer_modal_div);this.outer_modal_div  = null;}
		if(this.inner_modal_div != null){document.body.removeChild(this.inner_modal_div);this.inner_modal_div  = null;}
		if(this.content_iframe  != null){this.content_iframe = null;}

		document.body.style.margin = "";		
	}

    this.construct_div_content = function()
	{
	    var close_div   = null;	    
	    var content_div = null;

		this.inner_modal_div              = document.createElement("div");		
		this.inner_modal_div.align        = "center";
		this.inner_modal_div.className    = "inner_div";		
		this.inner_modal_div.id           = "inner_mod_div";
		this.inner_modal_div.style.height = "auto";		
		this.inner_modal_div.style.width  = parseInt(parent.screen.availWidth)  - 650;
		this.inner_modal_div.style.top    = 20;
		this.inner_modal_div.style.left   = (parseInt(screen.width) - parseInt(this.inner_modal_div.style.width)) / 2 - 10;
		this.inner_modal_div.style.border = "medium solid white";		

		content_div                  = document.createElement("div");		
		content_div.id               = "content_mod_div";
		content_div.align            = "left";
		content_div.style.background = "transparent";
		content_div.style.width      = "95%";
		content_div.innerHTML        = "<center><img src = '../images/ajaxloader.gif'></img></center>";		
		
		this.inner_modal_div.appendChild(content_div);		
		document.body.appendChild(this.inner_modal_div);
	}
	
	this.load_modal_css = function()
	{	    
		var file_reference = document.createElement("link");
		
	  	file_reference.setAttribute("rel", "stylesheet")
	  	file_reference.setAttribute("type", "text/css")
	  	file_reference.setAttribute("href", "../../css/cssmodals.css")

		if (typeof file_reference !="undefined")
		{ document.getElementsByTagName("head")[0].appendChild(file_reference) }
	}

	window.onscroll = function () 
	{		
	    var i = 0;
	    var m = 0;
    	
    	var divelement_outer = document.documentElement.getElementsByTagName("div");
    	var divelement_inner = document.documentElement.getElementsByTagName("div");
    	    	    	
	    for(i = 0; i < divelement_outer.length;i++)
	    {
	        if (divelement_outer.item(i).id == "outer_mod_div")
	        { divelement_outer.item(i).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;break; }	        	    }
	    for(m = 0; m < divelement_inner.length;m++)
	    {
	        if (divelement_inner.item(m).id == "inner_mod_div")
	        { divelement_outer.item(m).style.top = document.documentElement.scrollTop ? document.documentElement.scrollTop + 20 : document.body.scrollTop + 20;break; }	        
	    }	    
	}    
	
	this.amend_transaction = function()
    {
        var data_row = new Array();
        this.prepare_data();
        
        data_row[data_row.length] = "SEQ_ID~"               + cls.SEQ_ID;     
        data_row[data_row.length] = "Pay_Branch_code~"      + cls.Pay_Branch_code;
        data_row[data_row.length] = "MC_No~"                + cls.MC_No;
        data_row[data_row.length] = "Remarks~"              + cls.Remarks;
        data_row[data_row.length] = "ProcessedBy~"          + cls.ProcessedBy;
        data_row[data_row.length] = "DateProcessed~"        + cls.DateProcessed;
        data_row[data_row.length] = "ApprovedBy~"           + cls.approver;
        data_row[data_row.length] = "DatePaid~"             + cls.DatePaid;
        data_row[data_row.length] = "trxn_stat"             + cls.trxn_stat;   
        data_row[data_row.length] = "ID_SEQ_ID~"            + cls.IDSEQ;   
        data_row[data_row.length] = "IDNo~"                 + cls.IDNo;
        return data_row.join("|");
    } 
    
    
}