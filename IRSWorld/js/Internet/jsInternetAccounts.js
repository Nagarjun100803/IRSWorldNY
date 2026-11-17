function Internet_Accounts() {
    this.insert_flag = false;
    this.VALIDFC = false;
    this.KBIAccount = null;
    this.AccountNo = null;
    this.AccountType = null;
    this.AccountCurrency = null;
    this.AccountBranchCode = null;
    this.AccountBranchName = null;
    this.AccountStatus = null;
    this.AccountSeqId = null; /*seq id of account created*/
    this.DomBankCode = null;
    this.BranchType = null;
    this.BranchSeqID = null;

    this.CreatedBy = null;

    // Account defaults for web remittance
    this.enable_pnb_account_enrollment = false;
    this.enable_otb_dom_account_enrollment = false;
    this.enable_otb_ove_account_enrollment = false;
    this.enable_pnb_account_enrollment_approval = false;

    /* FOR CBIP */
    this.AccountRemarks = null;

    /****************** GRID FOR CIF ACCOUNTS *****************/
    this.load_grid_viewer_acctcif = function () {
        this.grid = new grid_viewer();
        this.session_end = true;

        if (this.enable_pnb_account_enrollment_approval == true && document.getElementById("txtHiddenUserRole").value == '10') {
            this.grid.sql_source = "sp_internet_grid_accounts_for_app_cif~" + this.get_rem_cifid();
        }
        else {
            this.grid.sql_source = "sp_internet_grid_accounts_cif~" + this.get_rem_cifid();
        }

        this.grid.div_container = document.getElementById("DIVGRIDACCTCIF");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }

    this.process_grid_acctcif = function (parameter) {
        this.grid = new grid_viewer();

        if (this.enable_pnb_account_enrollment_approval == true && document.getElementById("txtHiddenUserRole").value == '10') {
            this.grid.sql_source = "sp_internet_grid_accounts_for_app_cif~" + this.get_rem_cifid();
        }
        else {
            this.grid.sql_source = "sp_internet_grid_accounts_cif~" + this.get_rem_cifid();
        }

        this.grid.div_container = document.getElementById("DIVGRIDACCTCIF");
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }
    /*******************************************************/


    this.get_rem_cifid = function () {
        var rcifid = '';
        var rem_cifid = document.getElementById("txtHiddenRemCIFID").value;
        var src_cifid = document.getElementById("txtHiddenSearchCIFID").value;

        if (rem_cifid != '') rcifid = rem_cifid;
        else rcifid = src_cifid;

        return rcifid;
    }

    this.save_mode_acct = function () {
        document.getElementById("btnAddAcct").disabled = true;
        document.getElementById("btnEditAcct").disabled = true;
        document.getElementById("btnSaveAcct").disabled = false;
        document.getElementById("btnCancelAcct").disabled = false;
    }

    this.add_mode_acct = function () {
        document.getElementById("btnAddAcct").disabled = false;
        document.getElementById("btnEditAcct").disabled = true;
        document.getElementById("btnSaveAcct").disabled = true;
        document.getElementById("btnCancelAcct").disabled = true;
    }

    this.edit_mode_acct = function () {
        document.getElementById("btnAddAcct").disabled = false;
        document.getElementById("btnEditAcct").disabled = false;
        document.getElementById("btnSaveAcct").disabled = true;
        document.getElementById("btnCancelAcct").disabled = true;
    }

    this.btn_add = function () {
        Internet_Remitter_Main.check_session();

        this.clear_screen();
        this.disable_field(false);
        this.color_asterisks();
        this.save_mode_acct();
        this.insert_flag = true;

        if (document.getElementById("txtHiddenUserRole").value == '10') {
            document.getElementById("ddRemAcctCurr").value = IRSWorld.Internet.InternetRemitter.get_web_remittance_defaults_acct_currency().value;
            if (this.enable_pnb_account_enrollment_approval == true) {
                this.insert_record();
            }
        }

        document.getElementById("txtHiddenKBIAccount").value = '';
    }

    this.insert_record = function () {
        IRSWorld.Internet.InternetRemitter.insert_record(this.insert_record_callback);
    }

    this.insert_record_callback = function (result) {
        if (result != null && result.value != "undefined") {
            if (result.value == false) {
                var msg = IRSWorld.Internet.InternetRemitter.read_message(); alert(msg.value); return;
            }
        }

        this.commons = new common_method();
    }

    this.btn_edit = function () {
        Internet_Remitter_Main.check_session();
        if (Internet_Remitter_Main.check_gfc_user(this.get_rem_cifid())) {
            this.disable_field(false);
            this.color_asterisks();
            this.save_mode_acct();
            this.insert_flag = false;
        }
    }

    this.btn_cancel = function () {
        Internet_Remitter_Main.check_session();

        this.load_record();
    }

    this.btn_save = function () {
        Internet_Remitter_Main.check_session();
        if (Internet_Remitter_Main.check_mandatory_fields() == true) {
            if (this.validate_input() == true) {
                if (this.enable_pnb_account_enrollment_approval == true && document.getElementById("txtHiddenUserRole").value == '10' && this.insert_flag == true) {
                    if (confirm("Are you sure you want to send this request?")) {
                        this.commit_record();
                        this.disable_field(true);
                        this.add_mode_acct();
                    }
                }
                else // Account is not for approval
                {
                    var ret_val = '';

                    if (this.insert_flag == true) {
                        ret_val = IRSWorld.Internet.InternetRemitter.add_account(this.set_account_record()).value;
                    }
                    else {
                        //This block is deprecated by Single Remittance Table implementation
                        // Soft delete the old account number
                        ////var plist = '';
                        //plist += "CIFID~" + this.get_rem_cifid() + '|';
                        //plist += "SEQ_ID~" + this.AccountSeqId + '|';
                        //plist += "AccountDeleted~" + 'true';
                        //   
                        //ret_val = IRSWorld.Internet.InternetRemitter.update_account(plist).value;
                        //
                        //// Add the new account number
                        //ret_val = IRSWorld.Internet.InternetRemitter.add_account(this.set_account_record()).value;  
                        ret_val = IRSWorld.Internet.InternetRemitter.update_account(this.set_account_record()).value;
                    }
                    var tostr_ret_val = String(Number(ret_val));

                    if (tostr_ret_val == "NaN") {
                        alert(ret_val + '\n' + "Please try again."); return;
                    }
                    else {
                        this.AccountSeqId = tostr_ret_val;
                        this.load_grid_viewer_acctcif();
                        alert("RECORD COMMITTED SUCCESSFULLY!");
                        this.disable_field(true);
                        this.edit_mode_acct();
                    }
                }
            }
        }
    }

    this.commit_record = function () {
        IRSWorld.Internet.InternetRemitter.commit_record(this.set_account_record(true), this.commit_record_callback);
    }

    this.commit_record_callback = function (result) {
        if (result != null && result.value != "undefined") {
            if (result.value == false) {
                var msg = IRSWorld.Internet.InternetRemitter.read_message(); alert(msg.value); return;
            }

            this.SEQ_ID = IRSWorld.Internet.InternetRemitter.INTERNET_REMITTER_SEQ_ID().value;

            this.commons = new common_method();
            this.commons = null;

            alert("REQUEST SENT SUCCESSFULLY!");
        }

        this.commons = new common_method();

        Internet_Accounts.load_grid_viewer();
    }

    this.load_record = function () {
        this.clear_screen();
        this.disable_field(true);
        this.color_asterisks();
        this.add_mode_acct();
        if (this.AccountSeqId != null) {
            this.pull_server_data();
        }
    }

    this.pull_server_data = function () {
        IRSWorld.Internet.InternetRemitter.get_accounts_record(this.AccountSeqId, this.pull_server_data_callback);
    }

    this.pull_server_data_callback = function (result) {
        if (result != null && result.value != null) {
            //this.commons = new common_method();
            this.generic = new generic_method();

            var ds = result.value;
            var pd = null;

            if (ds != null && ds.Tables[0].Rows.length > 0) {
                var brcode = ds.Tables[0].Rows[0].BranchCode;
                var brname = ds.Tables[0].Rows[0].BranchName;
                var dbcode = ds.Tables[0].Rows[0].DomBankCode;
                var otname = ds.Tables[0].Rows[0].OthBankName;

                document.getElementById("txtHiddenKBIAccount").value = ds.Tables[0].Rows[0].KBIAccount;
                document.getElementById('txtRemAcctNo').value = ds.Tables[0].Rows[0].AccountNo;
                document.getElementById('ddRemAcctType').value = ds.Tables[0].Rows[0].AccountType;
                document.getElementById('ddRemAcctCurr').value = ds.Tables[0].Rows[0].Currency;

                document.getElementById('ddRemAcctStat').value = ds.Tables[0].Rows[0].Status;
                document.getElementById('ddOtherBankType').value = ds.Tables[0].Rows[0].BranchType;

                Internet_Accounts.AccountBranchCode = brcode;
                Internet_Accounts.DomBankCode = dbcode;

                if (document.getElementById("txtHiddenUserRole").value == '10' && ds.Tables[0].Rows[0].BranchType == '6') {
                    brname = ds.Tables[0].Rows[0].OthOveBankName;
                }

                Internet_Accounts.get_bank_name(brcode, dbcode, brname, otname);
                Internet_Accounts.edit_mode_acct();


                this.generic.to_clear_null("txtAccountNo~txtRemPNBBranch");
            }
        }
    }

    this.get_bank_name = function (pnbbcode, dombankcode, branchname, otname) {
        if (pnbbcode != null) {
            document.getElementById("txtRemPNBBranch").value = pnbbcode + ' - ' + branchname;
        }
        else if (dombankcode != null) {
            document.getElementById("txtRemOthBank").value = dombankcode + ' - ' + otname;
        }
        else if (document.getElementById("txtHiddenUserRole").value == '10') {
            document.getElementById("txtRemOthBank").value = branchname;
        }
    }

    this.set_account_record = function (isRequest) {
        this.KBIAccount = document.getElementById("txtHiddenKBIAccount").value;
        this.AccountNo = document.getElementById('txtRemAcctNo').value;
        this.AccountType = document.getElementById('ddRemAcctType').value;
        this.AccountCurrency = document.getElementById('ddRemAcctCurr').value;
        this.AccountStatus = document.getElementById('ddRemAcctStat').value;
        this.BranchType = document.getElementById('ddOtherBankType').value;
        this.CreatedBy = document.getElementById("txtHiddenUserID").value;

        if (isRequest == true) return this.push_data_to_server();
        else return this.set_plist_acct();
    }

    this.set_plist_acct = function () {
        var plist = '';

        plist += "CIFID~" + this.get_rem_cifid() + '|';

        if (this.AccountSeqId != '' && this.AccountSeqId != null && this.AccountSeqId != 'null') plist += "SEQ_ID~" + this.AccountSeqId + '|';
        if (this.AccountNo != '' && this.AccountNo != null && this.AccountNo != 'null') plist += "AccountNo~" + this.AccountNo + '|';
        if (this.KBIAccount != '' && this.KBIAccount != null && this.KBIAccount != 'null') plist += "KBIAccount~" + this.KBIAccount + '|';
        if (this.AccountType != '' && this.AccountType != null && this.AccountType != 'null') plist += "AccountType~" + this.AccountType + '|';
        if (this.AccountCurrency != '' && this.AccountCurrency != null && this.AccountCurrency != 'null') plist += "AccountCurrency~" + this.AccountCurrency + '|';
        if (this.AccountStatus != '' && this.AccountStatus != null && this.AccountStatus != 'null') plist += "AccountStatus~" + this.AccountStatus + '|';
        if (this.AccountBranchCode != '' && this.AccountBranchCode != null && this.AccountBranchCode != 'null') plist += "AccountBranchCode~" + this.AccountBranchCode + '|';
        if (this.DomBankCode != '' && this.DomBankCode != null && this.DomBankCode != 'null') plist += "DomBankCode~" + this.DomBankCode + '|';
        if (this.BranchType != '' && this.BranchType != null && this.BranchType != 'null') plist += "BranchType~" + this.BranchType + '|';

        plist += "branchname~" + document.getElementById('txtRemOthBank').value + '|';
        if (document.getElementById("txtHiddenUserRole").value == '10' && this.BranchType == '6') {
            plist += "branchname~" + document.getElementById('txtRemOthBank').value + '|';
        }

        return plist.substring(0, plist.length - 1);
    }

    this.push_data_to_server = function () {
        var data_row = new Array();

        data_row[data_row.length] = "RemitterCIFID~" + this.get_rem_cifid();
        data_row[data_row.length] = "CreatedBy~" + this.CreatedBy;
        data_row[data_row.length] = "AccountNo~" + this.AccountNo;
        data_row[data_row.length] = "BranchCode~" + this.AccountBranchCode;
        data_row[data_row.length] = "AccountType~" + this.AccountType;
        data_row[data_row.length] = "Currency~" + this.AccountCurrency;
        data_row[data_row.length] = "AccountStatus~" + this.AccountStatus;
        data_row[data_row.length] = "Status~0";
        /* default is 0 - for approval
		   in case, editing of card information will be allowed, delete this default value */

        return data_row.join("|");
    }

    this.clear_screen = function () {
        //this.commons = new common_method();     
        //this.commons.to_clear_screen("tblAccountInfo");
        //this.commons = null;
        clear_fields("tblAccountInfo");
    }

    this.disable_field = function (isDisabled) {
        //this.commons = new common_method();     
        //this.commons.to_read_only("tblAccountInfo", isDisabled);
        //this.commons = null;
        set_read_only("tblAccountInfo", isDisabled);

        if (isDisabled == true) {
            document.getElementById("txtRemPNBBranch").disabled = true;
            document.getElementById("txtRemOthBank").disabled = true;
        }
        else {
            document.getElementById("txtRemPNBBranch").readOnly = true;
            document.getElementById("txtRemOthBank").readOnly = true;
        }

        if (document.getElementById("txtHiddenUserRole").value == '10' && isDisabled == false) {
            this.apply_web_remittance_defaults();
        }
    }

    this.apply_web_remittance_defaults = function () {
        if (this.enable_pnb_account_enrollment == false) {
            document.getElementById('btnSearchPNBBranch').disabled = true;
            document.getElementById('ddRemAcctStat').disabled = true;
        }

        if (this.enable_otb_dom_account_enrollment == false && this.enable_otb_ove_account_enrollment == false) {
            document.getElementById('ddOtherBankType').disabled = true;
        }
        else if (this.enable_otb_dom_account_enrollment == true && this.enable_otb_ove_account_enrollment == false) {
            document.getElementById('ddOtherBankType').value = '5';
            document.getElementById('ddOtherBankType').disabled = true;
            document.getElementById('txtRemOthBank').readOnly = true;
            document.getElementById('Button8').disabled = false;
        }
        else if (this.enable_otb_dom_account_enrollment == false && this.enable_otb_ove_account_enrollment == true) {
            document.getElementById('ddOtherBankType').value = '6';
            document.getElementById('ddOtherBankType').disabled = true;
            document.getElementById('txtRemOthBank').readOnly = false;
            document.getElementById('Button8').disabled = true;
        }
        else if (document.getElementById('ddOtherBankType').value != null && document.getElementById('ddOtherBankType').value != 'null' && document.getElementById('ddOtherBankType').value != '') {
            if (document.getElementById('ddOtherBankType').value == '5') {
                document.getElementById('txtRemOthBank').readOnly = true;
                document.getElementById('Button8').disabled = false;
            }
            else if (document.getElementById('ddOtherBankType').value == '6') {
                document.getElementById('txtRemOthBank').readOnly = false;
                document.getElementById('Button8').disabled = true;
            }
        }
    }

    this.ddOtherBankType_OnChange = function () {
        if (document.getElementById("txtHiddenUserRole").value == '10') {
            document.getElementById('txtRemOthBank').value = '';
            this.BranchSeqID = '';
            this.DomBankCode = '';
            this.AccountBranchName = '';

            if (document.getElementById('ddOtherBankType').value == '5') {
                document.getElementById('txtRemOthBank').readOnly = true;
                document.getElementById('Button8').disabled = false;
            }
            else if (document.getElementById('ddOtherBankType').value == '6') {
                document.getElementById('txtRemOthBank').readOnly = false;
                document.getElementById('Button8').disabled = true;
                try { document.getElementById("txtRemOthBank").focus() } catch (e) { };
            }
        }
    }

    this.color_asterisks = function () {
        Internet_Remitter_Main.color_asterisk("tblAccountInfo");
    }

    this.show_tr_loading = function (isVisible) {
        if (isVisible == true) {
            document.getElementById('trLoading1').style.display = '';
        }
        else {
            document.getElementById('trLoading1').style.display = 'none';
        }
    }

    this.clear_acct_fields = function () {
        document.getElementById('txtRemPNBBranch').value = '';
        document.getElementById('ddRemAcctType').value = '';
        document.getElementById('ddRemAcctCurr').value = '';
        document.getElementById('ddRemAcctStat').value = '';
    }

    this.disable_acct_fields = function (isDisabled) {
        var ac = document.getElementById("ddRemAcctCurr");
        var at = document.getElementById("ddRemAcctType");
        var as = document.getElementById("ddRemAcctStat");
        var pb = document.getElementById("btnSearchPNBBranch");

        if (isDisabled == true) {
            if (ac.value != "null" && ac.value != "") ac.disabled = true; else ac.disabled = false;
            if (at.value != "null" && at.value != "") at.disabled = true; else at.disabled = false;
            if (as.value != "null" && as.value != "") as.disabled = true; else as.disabled = false;
            if ((document.getElementById("txtRemPNBBranch").value).length != 0) {
                pb.disabled = true;
            }
            else {
                pb.disabled = false;
            }
        }
        else {
            ac.disabled = false;
            at.disabled = false;
            as.disabled = false;
            pb.disabled = false;
        }
    }

    this.check_digit_acctno = function (acctno, e) {
        var src;
        if (!e) e = window.event;
        if (!e.target)
            src = window.event.srcElement;
        else
            src = e.target;

        if (src.tagName == "INPUT" & src.type == 'text') {
            if (e.keyCode == 13) {
                if (document.getElementById("txtHiddenUserRole").value == '10' && this.enable_pnb_account_enrollment == false) {
                    // bypass account checking if pnb account enrollment is disabled
                    return;
                }

                this.generic = new generic_method();
                this.commons = new common_method();

                /* check if account is empty */
                if (this.generic.is_empty(acctno) == true) {
                    alert('Please enter account number.'); return;
                }

                this.clear_acct_fields();
                this.disable_acct_fields(false);
                acctno = this.generic.to_remove_dash(acctno);
                document.getElementById('txtRemAcctNo').value = acctno;

                IRSWorld.Internet.InternetRemitter.validate_acctno_fcalgo(acctno, this.check_digit_acctno_callback);
                this.show_tr_loading(true);
                this.commons.to_show_loading_message('divLoading1');
                document.getElementById("lblLoad1").innerHTML = "Validating account number. Please wait...";
            }
        }
    }

    this.check_digit_acctno_callback = function (result) {
        if (result != null && result.value != null) {
            if (result.value == false) {
                Internet_Accounts.VALIDFC = false;
                Internet_Accounts.show_tr_loading(false);
                alert("Account number is an invalid FC account.");
            }
            else {
                Internet_Accounts.VALIDFC = true;
                Internet_Accounts.get_acct_details();
            }
        }
    }

    this.get_web_remittance_defaults = function () {
        this.enable_pnb_account_enrollment = IRSWorld.Internet.InternetRemitter.get_web_remittance_defaults('PNBACCTENROLL').value;
        this.enable_otb_dom_account_enrollment = IRSWorld.Internet.InternetRemitter.get_web_remittance_defaults('PNBOBDACCTENROLL').value;
        this.enable_otb_ove_account_enrollment = IRSWorld.Internet.InternetRemitter.get_web_remittance_defaults('PNBOBOACCTENROLL').value;
        this.enable_pnb_account_enrollment_approval = IRSWorld.Internet.InternetRemitter.get_web_remittance_defaults('PNBACCTENROLLAPP').value;
        if (this.enable_pnb_account_enrollment == false) document.getElementById('tdAcctPressEnterComment').innerHTML = '';
    }

    this.get_acct_details = function () {
        this.generic = new generic_method();
        var acctno = this.generic.to_trim(document.getElementById('txtRemAcctNo').value);
        var acct_det = null;

        IRSWorld.Internet.InternetRemitter.get_acct_details(acctno, this.get_acct_details_callback);
        document.getElementById("lblLoad1").innerHTML = "Retrieving account details. Please wait...";
    }

    this.get_acct_details_callback = function (result) {
        Internet_Accounts.show_tr_loading(false);

        this.generic = new generic_method();
        var acctno = this.generic.to_trim(document.getElementById('txtRemAcctNo').value);

        document.getElementById("txtHiddenKBIAccount").value = '';

        Internet_Accounts.set_acct_details(IRSWorld.Internet.InternetRemitter.get_algo_type(acctno).value, result.value);
    }

    this.set_acct_details = function (algo_type, acct_det) {
        if (algo_type == "KBI") {
            document.getElementById("txtHiddenKBIAccount").value = document.getElementById("txtRemAcctNo").value;
        }

        /* FOR CBIP */
        this.AccountRemarks = algo_type;

        if (acct_det != null && acct_det != 'null') {
            var ret_val = acct_det.split("~");

            if (ret_val.length > 1) /* successful retrieval of fc account details */ {
                if (algo_type == "KBI") {
                    if (ret_val[4] != null && ret_val[4] != '') {
                        alert("Account no. entered is a KBI account, this will be replaced by an FC account.");
                        document.getElementById("txtRemAcctNo").value = ret_val[4];
                    }
                }

                var ds = IRSWorld.Internet.InternetRemitter.get_converted_branchcode(ret_val[3]).value;
                var converted_bcode = -1;

                if (ds != null && ds.Tables[0].Rows.length > 0) {
                    converted_bcode = ds.Tables[0].Rows[0].BranchCode;
                    document.getElementById("txtRemPNBBranch").value = converted_bcode + ' - ' + ds.Tables[0].Rows[0].BranchName;
                    this.AccountBranchCode = converted_bcode;
                }

                this.commons = new common_method();
                this.commons.to_set_dropdownlist_index("ddRemAcctCurr", ret_val[0]);
                this.commons.to_set_dropdownlist_index("ddRemAcctType", ret_val[1]);

                /* FOR CBIP */
                var select = document.getElementById("ddRemAcctStat");
                var length = select.options.length;
                for (i = 0; i < length; i++) {
                    select.options[i] = null;
                }

                var ds;
                this._accountType = ret_val[5];
                if (this._accountType == "1") {
                    ds = IRSWorld.Internet.InternetRemitter.getAccountStatus("1").value;
                }
                else {
                    ds = IRSWorld.Internet.InternetRemitter.getAccountStatus("2").value;
                }

                load_ddl("ddRemAcctStat", ds.Tables[0], "FldValue", "FldDescription", 0, 'i', null, '');
                this.ACCTSTAT = ret_val[2];
                this.commons.to_set_dropdownlist_index("ddRemAcctStat", this.ACCTSTAT);

                if (ret_val[4] != "") {
                    document.getElementById("txtRemAcctNo").value = ret_val[4];
                }

                if (this._accountType == "2") //IM
                {
                    if (this.ACCTSTAT != 1 && this.ACCTSTAT != 10) {
                        alert("Account Status is invalid");
                    }
                }
                else //ST
                {
                    if (this.ACCTSTAT != 1) {
                        alert("Account Status is invalid");
                    }
                }
            }
            else /* has errMsg */ {
                alert(ret_val);
            }
        }
        else {
            alert('Unable to retrieve account details, please fill in the following fields.');
            this.clear_acct_fields();
        }
    }

    this.validate_input = function () {
        var acctstat = document.getElementById('ddRemAcctStat').value;

        if ((document.getElementById("txtRemPNBBranch").value).length > 0) {
            var branchtype = -1;
            var ds = null;
            ds = IRSWorld.Internet.InternetRemitter.get_bank(Number(this.BranchSeqID)).value;

            if (ds != null && ds.Tables[0].Rows.length > 0) {
                branchtype = ds.Tables[0].Rows[0].BranchType;
            }
            if (Number(branchtype) == 2 || Number(branchtype) == 1) {
                if (IRSWorld.Internet.InternetRemitter.validate_acctno_fcalgo(document.getElementById('txtRemAcctNo').value).value == false) {
                    alert("Account number is an invalid FC account.\n");
                    this.VALIDFC = false;
                    return false;
                }
            }
        }

        /* FOR CBIP */
        if (document.getElementById("txtRemPNBBranch").value != "") {
            var accttype = document.getElementById('ddRemAcctType').value;
            if (accttype == 2) {
                if (acctstat != 1 && acctstat != 10) {
                    this.error_input += "Account Status is invalid";
                    return false;
                }
            }
            else {
                if (acctstat != 1) {
                    this.error_input += "Account Status is invalid";
                    return false;
                }
            }
        }

        this.VALIDFC = true;
        return true;
    }
}
