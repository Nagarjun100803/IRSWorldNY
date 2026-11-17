function cif_accounts() {
    this.commons = null;
    this.generic = null;
    this.SEQ_ID = null;
    this.CIFID = null;
    this.AccountNo = null;
    this.AccountType = null;
    this.Currency = null;
    this.BranchCode = null;
    this.DomBankCode = null;
    this.Remarks = null;
    this.Status = null;
    this.grid = null;
    this.KBIAccount = null;
    this.OtherBankSeqId = null;
    this.BranchType = null;
    var iAccounts = this;

    this.load_grid_viewer = function () {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblAccounts");
        this.commons = null;

        this.grid = new grid_viewer();
        this.session_end = true;
        this.grid.sql_source = "sp_cif_grid_accounts~" + this.CIFID;
        this.grid.div_container = document.getElementById("CIFACCOUNTSGRID");
        this.grid.datarow_column = 0;
        this.grid.open_grid_view();
        this.grid = null;
    }

    this.process_grid_viewer = function (parameter) {
        this.grid = new grid_viewer();

        this.grid.sql_source = "sp_cif_grid_accounts~" + this.CIFID;
        this.grid.div_container = document.getElementById("CIFACCOUNTSGRID");
        this.grid.datarow_column = 0;
        this.grid.process_ajax(parameter);
        this.grid = null;
    }

    this.cancel_insert = function () {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblAccounts");
        this.commons.to_read_only("tblAccounts", true);
        this.commons.to_unlock_button("tblbuttons", "btnAdd");
    }

    this.load_record = function () {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblAccounts");
        this.commons.to_read_only("tblAccounts", true);
        this.commons = null;

        this.pull_server_data();
    }

    this.disable_some_fields = function () {
        document.getElementById("txtPNBBranchCode").readOnly = true;
        document.getElementById("txtDomBankCode").readOnly = true;
        document.getElementById("txtMT103BankName").readOnly = true;
    }

    this.set_name_module = function (module_name) {
        IRSWorld.CIF.cifmain.set_module_name(module_name);
    }

    this.set_record = function () {
        this.AccountNo = document.getElementById("txtAccountNo").value;
        this.Currency = document.getElementById("ddAccCurrency").value;
        this.Remarks = document.getElementById("txtAccountRemarks").value;
        this.Status = document.getElementById("ddAccountStatus").value;
        this.AccountType = document.getElementById("ddAccountType").value;
        this.KBIAccount = document.getElementById("txtHiddenKBIAccount").value;
        this.BranchType = document.getElementById("ddOtherBankType").value;
        this.set_bankcode();
        this.set_other_banks();

        return this.push_data_to_server();
    }

    this.pull_server_data = function () {
        if (this.SEQ_ID == null) {
            this.SEQ_ID = IRSWorld.CIF.cifmain.get_seq_id().value;
        }

        if (this.SEQ_ID == null || this.SEQ_ID == '' || this.SEQ_ID == 'undefined') {
            return;
        }

        IRSWorld.CIF.cifmain.GET_ACCOUNTS_RECORD(this.SEQ_ID, this.pull_server_data_callback);
    }

    this.pull_server_data_callback = function (result) {
        if (result != null && result.value != null) {
            var ds = result.value;

            if (ds.Tables[0].Rows.length > 0) {
                this.commons = new common_method();
                this.commons.to_clear_screen("tblAccounts");
                this.commons.to_read_only("tblAccounts", true);

                iAccounts.SEQ_ID = ds.Tables[0].Rows[0].SEQ_ID;
                iAccounts.AccountNo = ds.Tables[0].Rows[0].AccountNo;
                iAccounts.AccountType = ds.Tables[0].Rows[0].AccountType;
                iAccounts.Currency = ds.Tables[0].Rows[0].Currency;
                iAccounts.BranchCode = ds.Tables[0].Rows[0].BranchCode;
                iAccounts.DomBankCode = ds.Tables[0].Rows[0].DomBankCode;
                iAccounts.Remarks = ds.Tables[0].Rows[0].Remarks;
                iAccounts.Status = ds.Tables[0].Rows[0].Status;
                iAccounts.OtherBankSeqId = ds.Tables[0].Rows[0].OtherBankSeqId;
                iAccounts.BranchType = ds.Tables[0].Rows[0].BranchType;

                document.getElementById("txtHiddenSeqId").value = iAccounts.SEQ_ID;
                document.getElementById("txtHiddenCIFID").value = iAccounts.CIFID;
                document.getElementById("txtAccountNo").value = iAccounts.AccountNo;
                document.getElementById("txtAccountRemarks").value = iAccounts.Remarks;

                this.commons.to_set_dropdownlist_index("ddAccountType", iAccounts.AccountType);
                this.commons.to_set_dropdownlist_index("ddAccountStatus", iAccounts.Status);
                this.commons.to_set_dropdownlist_index("ddAccCurrency", iAccounts.Currency);
                this.commons.to_set_dropdownlist_index("ddOtherBankType", iAccounts.BranchType);

                this.accts = new cif_accounts();
                this.accts.get_bankname();

                if (iAccounts.DomBankCode != '' && iAccounts.DomBankCode != null) {
                    cif_main.is_mt103(iAccounts.DomBankCode);
                    this.accts.get_other_bankdetails(iAccounts.OtherBankSeqId);
                }

                this.generic = new generic_method();
                this.generic.to_clear_null("txtAccountNo~txtAccountRemarks~txtPNBBranchCode~txtDomBankCode");

                this.commons = null;
            }
        }
    }

    this.push_data_to_server = function () {
        var data_row = new Array();

        data_row[data_row.length] = "CIFID~" + this.CIFID;
        data_row[data_row.length] = "AccountNo~" + this.AccountNo;
        data_row[data_row.length] = "AccountType~" + this.AccountType;
        data_row[data_row.length] = "Currency~" + this.Currency;
        data_row[data_row.length] = "BranchCode~" + this.BranchCode;
        data_row[data_row.length] = "DomBankCode~" + this.DomBankCode;
        data_row[data_row.length] = "Remarks~" + this.Remarks;
        data_row[data_row.length] = "Status~" + this.Status;
        data_row[data_row.length] = "KBIAccount~" + this.KBIAccount;
        data_row[data_row.length] = "OtherBankSeqId~" + this.OtherBankSeqId;
        data_row[data_row.length] = "BranchType~" + this.BranchType;
        data_row[data_row.length] = "Deleted~false";

        return data_row.join("|");
    }


    this.insert_record = function () {
        document.getElementById('txtHiddenAcctBankSeqId').value = '';
        document.getElementById('txtHiddenAcctPNBBranchSeqId').value = '';
        document.getElementById('txtHiddenAcctDomBankSeqId').value = '';
        document.getElementById('txtHiddenSeqId').value = '';
        document.getElementById('txtHiddenMT103BankSeqId').value = '';

        cif_main.hide_elements("trLblSearchBank~trSearchOtherBank1~trOtherBank1~trOtherBank2");
        IRSWorld.CIF.cifmain.insert_record(this.insert_record_callback);
    }

    this.insert_record_callback = function (result) {
        if (result.error != null && result.error != "undefined") {
            alert("UNABLE TO INSERT RECORD. " + result.error);
        }

        if (result != null && result.value != "undefined") {
            if (result.value == false) {
                var msg = IRSWorld.CIF.cifmain.read_message();

                if (msg != null && msg.value != "undefined") {
                    alert(msg.value); return;
                }
            }

            this.commons = new common_method();
            this.commons.to_clear_screen("tblAccounts");
            this.commons.to_read_only("tblAccounts", false);
            this.commons.to_unlock_button("tblbuttons", "btnSave~btnCancel");
            this.commons = null;

            iAccounts.disable_some_fields();
        }
    }

    this.update_record = function () {
        IRSWorld.CIF.cifmain.update_record(this.SEQ_ID, this.update_record_callback);
    }

    this.update_record_callback = function (result) {
        if (result.error != null && result.error != "undefined") {
            alert("UNABLE TO UPDATE RECORD. " + result.error); return;
        }

        if (result != null && result.value != "undefined") {
            if (result.value == false) {
                var msg = IRSWorld.CIF.cifmain.read_message();

                if (msg != null && msg.value != "undefined") {
                    alert(msg.value); return;
                }
            }

            this.commons = new common_method();
            this.commons.to_read_only("tblAccounts", false);
            this.commons.to_unlock_button("tblbuttons", "btnSave~btnCancel");
            this.commons = null;

            iAccounts.disable_some_fields();
            if ((document.getElementById("txtPNBBranchCode").value).length != 0) {
                //this.accts = new cif_accounts();
                iAccounts.disable_acctfields(true);
            }
        }
    }

    this.delete_record = function () {
        IRSWorld.CIF.cifmain.delete_record(this.SEQ_ID, this.delete_record_callback);
        cif_main.set_deleted();
    }

    this.delete_record_callback = function (result) {
        if (result.error != "undefined" && result.error != null) {
            alert("UNABLE TO DELETE RECORD. " + result.error); return;
        }

        if (result != null && result.value != "undefined") {
            if (result.value == false) {
                alert(IRSWorld.CIF.cifmain.read_message()); return false;
            }
            return true;
        }
    }

    this.commit_record = function () {
        IRSWorld.CIF.cifmain.commit_record(this.set_record(), this.commit_record_callback);
        this.SEQ_ID = null;
        document.getElementById('txtHiddenAcctBankSeqId').value = '';
        document.getElementById('txtHiddenAcctPNBBranchSeqId').value = '';
        document.getElementById('txtHiddenAcctDomBankSeqId').value = '';
        document.getElementById('txtHiddenSeqId').value = '';
        document.getElementById('txtHiddenMT103BankSeqId').value = '';
    }

    this.commit_record_callback = function (result) {
        iAccounts.load_record();

        if (typeof (iAccounts.load_grid_viewer) != 'undefined') {
            iAccounts.load_grid_viewer();
        }

        if (result.error != null && result.error != "undefined") {
            alert("UNABLE TO COMMIT RECORD. " + result.error); return;
        }

        if (result != null && result.value != "undefined") {
            if (result.value == false) {
                var msg = IRSWorld.CIF.cifmain.read_message();

                if (msg != null && msg.value != "undefined") {
                    alert(msg.value); return;
                }
            }

            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons", "btnAdd~btnEdit~btnDelete");
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

    this.rollback_record = function () {
        IRSWorld.CIF.cifmain.rollback_record(this.rollback_record_callback);
    }

    this.rollback_record_callback = function (result) {
        var msg = IRSWorld.CIF.cifmain.read_message();

        if (msg != null && msg.value != "undefined" && msg.value.length > 0) {
            alert(msg.value);
        }

        this.commons = new common_method();
        this.commons.to_unlock_button("tblbuttons", "btnAdd~btnEdit~btnDelete");
        this.commons = null;
    }

    this.validate_input = function () {
        this.generic = new generic_method();
        this.error_input = '';
        var accountno = '';
        var kbiacct = '';
        var pnbbranch = '';
        var othbank = '';
        var accttype = '';
        var acctcurr = '';
        var othlblbankname = '';
        var othtxtbankname = '';
        var othtxtbankadd = '';
        var pnbseqid = '';
        var branchtype = '';

        accountno = this.generic.to_remove_dash(document.getElementById("txtAccountNo").value);
        kbiacct = this.generic.to_remove_dash(document.getElementById("txtHiddenKBIAccount").value);
        pnbbranch = document.getElementById("txtPNBBranchCode").value;
        othbank = document.getElementById("txtDomBankCode").value;
        accttype = document.getElementById("ddAccountType").value;
        acctcurr = document.getElementById("ddAccCurrency").value;
        acctstat = document.getElementById("ddAccountStatus").value;
        othlblbankname = document.getElementById("txtMT103BankName").value;
        othtxtbankname = document.getElementById("txtOthBankName").value;
        othtxtbankadd = document.getElementById("txtOthBankAddress").value;
        pnbseqid = document.getElementById("txtHiddenAcctPNBBranchSeqId").value;

        var ds = null;
        ds = IRSWorld.CIF.cifmain.get_bank(pnbseqid).value;

        if (ds != null && ds.Tables[0].Rows.length > 0) {
            document.getElementById("txtHiddenBranchType").value = ds.Tables[0].Rows[0].BranchType;
            branchtype = ds.Tables[0].Rows[0].BranchType;
        }

        if (IRSWorld.CIF.cifmain.check_acct_gfc(accountno, kbiacct).value == true) {
            this.error_input += "Can not enroll GFC Account!";
            return false;
        }

        /* FOR CBIP */
        if (accttype == 2) {
            if (acctstat != 1 && acctstat != 8 && acctstat != 10) {
                this.error_input += "Account Status is invalid";
                return false;
            }
        }
        else {
            if (acctstat != 1 && acctstat != 2) {
                this.error_input += "Account Status is invalid";
                return false;
            }
        }

        if ((this.generic.is_empty(pnbbranch) == false) && (Number(branchtype) == 2 || Number(branchtype) == 1)) {
            if (IRSWorld.CIF.cifmain.validate_acctno_fcalgo(accountno).value == false) {
                this.error_input += "Account number is an invalid FC account.\n";
            }
        }
        if (document.getElementById('txtHiddenISMT103').value == 'true') {
            if (othlblbankname == '') {
                if (othtxtbankname == '' && othtxtbankadd == '') this.error_input += "Bank name and address required.\n";
                else if (othtxtbankname == '') this.error_input += "Please enter bank name.\n";
                else if (othtxtbankadd == '') this.error_input += "Please enter bank address.\n";
            }
        }
        if (this.error_input != '') {
            return false;
        }

        return true;
    }

    this.set_other_banks = function () {
        var hidseqid = document.getElementById('txtHiddenSeqId').value;

        if (hidseqid == '') {
            if (document.getElementById('txtHiddenMT103BankSeqId').value == '') {
                var othbname = document.getElementById('txtOthBankName').value;
                var othbaddr = document.getElementById('txtOthBankAddress').value;

                if (othbname.length == 0 && othbaddr.length == 0) return;
                var data_row = new Array();

                data_row[data_row.length] = "BankName~" + othbname;
                data_row[data_row.length] = "BankAddress~" + othbaddr;

                data_row.join("|");

                this.add_other_banks(data_row);
            }
            else {
                this.OtherBankSeqId = document.getElementById('txtHiddenMT103BankSeqId').value;
            }
        }
        else {
            if (document.getElementById('txtHiddenMT103BankSeqId').value == '') {
                var ds = null;
                ds = IRSWorld.CIF.cifmain.GET_ACCOUNTS_RECORD(hidseqid).value;

                if (ds.Tables[0].Rows.length > 0) {
                    this.OtherBankSeqId = ds.Tables[0].Rows[0].OtherBankSeqId;
                }
            }
            else {
                this.OtherBankSeqId = document.getElementById('txtHiddenMT103BankSeqId').value;
            }
        }
    }

    this.add_other_banks = function (data_row) {
        //cifmain.add_other_banks( data_row, this.add_other_banks_callback ); 

        var oth_seqid = null;

        oth_seqid = IRSWorld.CIF.cifmain.add_other_banks(data_row).value;
        document.getElementById('txtHiddenMT103BankSeqId').value = oth_seqid;
        this.OtherBankSeqId = oth_seqid;
    }

    this.add_other_banks_callback = function (result) {
        if (result != null && result.value != "undefined") {
            document.getElementById('txtHiddenMT103BankSeqId').value = result.value;
            this.OtherBankSeqId = result.value;
        }
    }

    this.set_bankcode = function () {
        var pnbseqid = document.getElementById('txtHiddenAcctPNBBranchSeqId').value;
        var domseqid = document.getElementById('txtHiddenAcctDomBankSeqId').value;
        var bankseqid = document.getElementById('txtHiddenAcctBankSeqId').value;
        var hidseqid = document.getElementById('txtHiddenSeqId').value;

        if (domseqid.length > 0) {
            var ds = null;
            ds = IRSWorld.CIF.cifmain.get_bank(domseqid).value;

            if (ds.Tables[0].Rows.length > 0) {
                this.DomBankCode = ds.Tables[0].Rows[0].BranchCode;
                this.BranchCode = '';
            }
        }
        else if (pnbseqid.length > 0) {
            var ds = null;
            ds = IRSWorld.CIF.cifmain.get_bank(pnbseqid).value;

            if (ds.Tables[0].Rows.length > 0) {
                this.BranchCode = ds.Tables[0].Rows[0].BranchCode;
                this.DomBankCode = '';
            }
        }
        else if (bankseqid.length == 0 && hidseqid.length > 0) {
            var ds = null;
            ds = IRSWorld.CIF.cifmain.get_accounts_bankcode(hidseqid).value;

            if (ds.Tables[0].Rows.length > 0) {
                this.BranchCode = ds.Tables[0].Rows[0].BranchCode;
                this.DomBankCode = ds.Tables[0].Rows[0].DomBankCode;
            }
        }

        if (bankseqid.length == 0 && pnbseqid.length == 0 && domseqid.length == 0 && hidseqid.length == 0) {
            this.BranchCode = null;
            this.DomBankCode = null;
        }
    }

    this.get_other_bankdetails = function (othbankseqid) {
        if (othbankseqid != '' && othbankseqid != null) {
            var ds = null;
            ds = IRSWorld.CIF.cifmain.get_otherbanks_details(othbankseqid).value;
            if (ds.Tables[0].Rows.length > 0) {
                var bank_name = ds.Tables[0].Rows[0].BankName;
                var bank_address = ds.Tables[0].Rows[0].BankAddress;

                document.getElementById("txtMT103BankName").value = bank_name + ' - ' + bank_address;
            }
        }
    }

    this.get_bankname = function () {
        if ((document.getElementById("txtHiddenSeqId").value).length > 0) {
            var ds = null;
            ds = IRSWorld.CIF.cifmain.get_accounts_bankname(document.getElementById("txtHiddenSeqId").value).value;

            if (ds.Tables[0].Rows.length > 0) {
                if (ds.Tables[0].Rows[0].PNBBranchName != null) {
                    document.getElementById("txtPNBBranchCode").value = ds.Tables[0].Rows[0].BranchCode + ' - ' + ds.Tables[0].Rows[0].PNBBranchName;
                }
                if (ds.Tables[0].Rows[0].DomBankName != null) {
                    document.getElementById("txtDomBankCode").value = ds.Tables[0].Rows[0].DomBankCode + ' - ' + ds.Tables[0].Rows[0].DomBankName;
                }
            }
        }
    }

    this.clear_acctfields = function () {
        document.getElementById("ddAccCurrency").value = '';
        document.getElementById("ddAccountType").value = '';
        document.getElementById("ddAccountStatus").value = '';
        document.getElementById("ddOtherBankType").value = '';
        document.getElementById("txtAccountRemarks").value = '';
        document.getElementById("txtPNBBranchCode").value = '';
        document.getElementById("txtDomBankCode").value = '';
        document.getElementById("txtHiddenBranchType").value = '';
    }

    this.disable_acctfields = function (isDisabled) {
        var ac = document.getElementById("ddAccCurrency");
        var at = document.getElementById("ddAccountType");
        var as = document.getElementById("ddAccountStatus");
        var pb = document.getElementById("btnSearchPNBBranch");
        var db = document.getElementById("btnSearchDomBank");
        var bt = document.getElementById("ddOtherBankType");

        if (isDisabled == true) {
            if (ac.value != "null" && ac.value != "") ac.disabled = true; else ac.disabled = false;
            if (at.value != "null" && at.value != "") at.disabled = true; else at.disabled = false;
            if (as.value != "null" && as.value != "") as.disabled = true; else as.disabled = false;
            if ((document.getElementById("txtPNBBranchCode").value).length != 0) {
                pb.disabled = true;
                db.disabled = true;
                bt.disabled = true;
            }
            else {
                pb.disabled = false;
                db.disabled = false;
                bt.disabled = false;
            }
        }
        else {
            ac.disabled = false;
            at.disabled = false;
            as.disabled = false;
            pb.disabled = false;
            db.disabled = false;
            bt.disabled = false;
        }
    }

    this.set_acct_details = function (algo_type, acct_det) {
        if (algo_type == "KBI") {
            document.getElementById("txtHiddenKBIAccount").value = document.getElementById("txtAccountNo").value;
        }

        if (acct_det != null && acct_det != 'null') {
            var ret_val = acct_det.split("~");

            if (ret_val.length > 1) /* successful retrieval of fc account details */ {
                if (algo_type == "KBI") {
                    if (ret_val[4] != null && ret_val[4] != '') {
                        alert("Account no. entered is a KBI account, this will be replaced by an FC account.");
                        document.getElementById("txtAccountNo").value = ret_val[4];
                    }
                }

                this.commons = new common_method();
                this.commons.to_set_dropdownlist_index("ddAccCurrency", ret_val[0]);
                this.commons.to_set_dropdownlist_index("ddAccountType", ret_val[1]);

                /* FOR CBIP */
                var select = document.getElementById("ddAccountStatus");
                var length = select.options.length;
                for (i = 0; i < length; i++) {
                    select.options[i] = null;
                }

                var ds;
                var accountType = ret_val[5];
                if (accountType == "1") {
                    ds = IRSWorld.CIF.cifmain.getAccountStatus("1").value;
                }
                else {
                    ds = IRSWorld.CIF.cifmain.getAccountStatus("2").value;
                }
                this.commons.to_load_dropdownlist("ddAccountStatus", ds, "FldValue", "FldDescription");
                this.commons.to_set_dropdownlist_index("ddAccountStatus", ret_val[2]);

                this.disable_acctfields(true);

                if (this._accountType == "2") //IM
                {
                    if (ret_val[2] != 1 && ret_val[2] != 8 && ret_val[2] != 10) {
                        alert("Account Status is invalid");
                    }
                }
                else //ST
                {
                    if (ret_val[2] != 1 && ret_val[2] != 10) {
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

            this.BranchCode = '';
            document.getElementById("txtPNBBranchCode").value = '';
            document.getElementById("txtHiddenAcctPNBBranchSeqId").value = '';

            this.commons = new common_method();
            this.commons.to_set_dropdownlist_index("ddAccCurrency", 0);
            this.commons.to_set_dropdownlist_index("ddAccountType", 0);
            this.commons.to_set_dropdownlist_index("ddAccountStatus", 0);
        }
    }
}

