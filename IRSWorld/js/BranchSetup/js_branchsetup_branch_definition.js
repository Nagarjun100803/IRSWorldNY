var guserrolecode = '0';
$(document).ready(function () {
    getrolecode();
});
function getrolecode() {
    try {
        IRSWorld.BranchSetup.Branch_Definition.getrolecode(function (result) {
            if (result.error == null) {
                guserrolecode = result.value;
            }
            else {
                alert("Failed to get role, please reload the page");
            }
        });
    }
    catch (err) { //err.message
        spawn_errpage('Failed to get role. Please try again.');
    }
}

function execute_ajx_command(param) {
    if (branch_definition != null && branch_definition != undefined) {
        branch_definition.do_ajax(param);
    }
}

function execute_ajaxgridview_command(param) {
    if (branch_definition != null && branch_definition != undefined) {
        branch_definition.do_ajaxgridview(param);
    }
}

function branch_definition() {
    this.commons = null;
    this.generic = null;
    this.SEQ_ID = null;
    this.BranchType = null;
    this.BranchCode = null;
    this.BranchName = null;
    this.NegBranchCode = null;
    this.CoveringBankCode = null;
    this.FCBranchCode = null;
    this.Classification = null;
    this.ContactPerson = null;
    this.Address = null;
    /* ALESSA */
    this.ZipCode = null;
    this.State = null;
    this.Country = null;
    this.Phone = null;
    this.Fax = null;
    this.TimeZone = null;
    this.ChargeGroup = null;
    this.Brstn = null;
    this.PCHC = null;
    this.Dom_loc = null;
    this.CourierCode = null;
    this.message = null;
    this.ajax_grid = null;
    this.grid_ajax_query = null;
    this.grid_ajax_page = "../AjxPage/ajaxgridview.aspx";
    this.ajax_grid = "GRIDCLASSADD";
    this.grid_ajax_query = null;
    this.grid_ajax_page = "../AjxPage/ajaxgrid.aspx";
    this.CovBranch = null;
    this.add = null;
    /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
    this.BankInformationCode = null;

    this.set_branch_definition = function () {
        this.BranchType = document.getElementById("ddBranchType").value;
        this.BranchCode = document.getElementById("txtBranchCode").value;
        this.BranchName = document.getElementById("txtBranchName").value;
        this.NegBranchCode = document.getElementById("txtNegBranchCode").value;
        this.CoveringBankCode = document.getElementById("ddCovCode").value;
        this.FCBranchCode = document.getElementById("txtFCBranchCode").value;
        this.ContactPerson = document.getElementById("txtContactPerson").value;
        this.Address = document.getElementById("txtAddress").value;
        /* ALESSA */
        this.ZipCode = document.getElementById("txtZipCode").value;
        this.State = document.getElementById("txtState").value;
        this.Country = document.getElementById("ddCountry").value;
        this.Phone = document.getElementById("txtPhone1").value + "-" + document.getElementById("txtPhone2").value + "-" + document.getElementById("txtPhone3").value;
        this.Email = document.getElementById("txtEmail").value;
        this.Fax = document.getElementById("txtFax").value;
        this.TimeZone = document.getElementById("ddTimeZone").value;
        this.ChargeGroup = document.getElementById("ddChargeGroup").value;
        this.Brstn = document.getElementById("txtBrstn").value;
        this.CourierCode = document.getElementById("txtCourierCode").value;
        this.PCHC = document.getElementById("ddPCHC").value;
        this.Dom_loc = document.getElementById("ddDom_loc").value;
        this.CovBranch = document.getElementById("ddCovBranch").value;
        this.Classification = false;
        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        this.BankInformationCode = document.getElementById("txtBankInformationCode").value;


        if (document.getElementById("chkClassification").value != null && document.getElementById("chkClassification").checked == true) {
            this.Classification = true;
        }
        return this.push_data_to_server();
    }

    this.pull_server_data = function () {
        this.hide_grid();
        if (this.SEQ_ID == null) {
            this.SEQ_ID = IRSWorld.BranchSetup.Branch_Definition.BRANCH_DEFINITION_SEQ_ID().value;
        }
        if (this.SEQ_ID == null || this.SEQ_ID == "" || this.SEQ_ID == "undefined") {
            return;
        }
        IRSWorld.BranchSetup.Branch_Definition.GET_BRANCH_DEFINITION_RECORD(this.SEQ_ID, this.pull_server_data_callback);
    }

    this.pull_server_data_callback = function (result) {
        if (result != null && result.value != null) {
            var ds = result.value;
            if (ds.Tables[0].Rows.length > 0) {
                this.BranchType = ds.Tables[0].Rows[0].BranchType;
                this.BranchCode = ds.Tables[0].Rows[0].BranchCode;
                this.BranchName = ds.Tables[0].Rows[0].BranchName;
                this.NegBranchCode = ds.Tables[0].Rows[0].NegBranchCode;
                this.CoveringBankCode = ds.Tables[0].Rows[0].CoveringBankCode;
                this.FCBranchCode = ds.Tables[0].Rows[0].FCBranchCode;
                this.Classification = ds.Tables[0].Rows[0].Classification;
                this.ContactPerson = ds.Tables[0].Rows[0].ContactPerson;
                this.Address = ds.Tables[0].Rows[0].Address;
                /* ALESSA */
                this.ZipCode = ds.Tables[0].Rows[0].ZipCode;
                this.State = ds.Tables[0].Rows[0].State;
                this.Country = ds.Tables[0].Rows[0].Country;
                this.Phone = ds.Tables[0].Rows[0].Phone;
                this.Email = ds.Tables[0].Rows[0].Email;
                this.Fax = ds.Tables[0].Rows[0].Fax;
                this.TimeZone = ds.Tables[0].Rows[0].TimeZone;
                this.ChargeGroup = ds.Tables[0].Rows[0].ChargeGroup;
                this.Brstn = ds.Tables[0].Rows[0].Brstn;
                this.CourierCode = ds.Tables[0].Rows[0].CourierCode;
                this.PCHC = ds.Tables[0].Rows[0].PCHC;
                this.Dom_loc = ds.Tables[0].Rows[0].Dom_Br_Loc;
                this.CovBranch = ds.Tables[0].Rows[0].CoveringBranchCode;
                /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
                this.BankInformationCode = ds.Tables[0].Rows[0].BIC;
                document.getElementById("txtHiddenSeqID").value         = this.SEQ_ID;
                document.getElementById("txtBranchCode").value          = this.BranchCode;
                document.getElementById("txtBranchName").value          = this.BranchName;
                document.getElementById("txtNegBranchCode").value       = this.NegBranchCode;
                //document.getElementById("txtCoveringBankCode").value     = this.CoveringBankCode;
                document.getElementById("ddCovCode").value              = this.CoveringBankCode;
                document.getElementById("ddCovBranch").value            = this.CovBranch;
                document.getElementById("txtFCBranchCode").value        = this.FCBranchCode;
                document.getElementById("txtContactPerson").value       = this.ContactPerson;
                document.getElementById("txtAddress").value             = this.Address;
                /* ALESSA */
                document.getElementById("txtZipCode").value             = this.ZipCode;
                document.getElementById("txtState").value               = this.State;
                document.getElementById("ddCountry").value              = this.Country;
                document.getElementById("ddChargeGroup").value          = this.ChargeGroup;
                document.getElementById("txtEmail").value               = this.Email;
                document.getElementById("txtFax").value                 = this.Fax;
                document.getElementById("ddTimeZone").value             = this.TimeZone;
                document.getElementById("txtBrstn").value               = this.Brstn;
                document.getElementById("txtCourierCode").value         = this.CourierCode;
                document.getElementById("ddPCHC").value                 = this.PCHC;
                document.getElementById("ddDom_loc").value              = this.Dom_loc;
                /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
                document.getElementById("txtBankInformationCode").value = this.BankInformationCode;

                if (this.Phone == null) {
                    this.Phone = "--";
                }

                var phone_nos = this.Phone.split("-");
                document.getElementById("txtPhone1").value              = phone_nos[0];
                document.getElementById("txtPhone2").value              = phone_nos[1];
                document.getElementById("txtPhone3").value              = phone_nos[2];
                document.getElementById("chkClassification").value      = "off";
                document.getElementById("chkClassification").checked    = false;

                if (document.getElementById("chkClassification").value != null && this.Classification == true) {
                    document.getElementById("chkClassification").value = "on";
                    document.getElementById("chkClassification").checked = true;
                }

                this.commons = new common_method();
                this.generic = new generic_method();
                /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
		this.generic.to_clear_null("txtNegBranchCode~txtFCBranchCode~txtContactPerson~txtAddress~txtState~txtFax~txtEmail~txtBrstn~txtCourierCode~txtBankInformationCode");
                this.commons.to_set_dropdownlist_index("ddBranchType", this.BranchType);
                this.commons.to_read_only("tblBranchDesc", true);
                this.commons.to_read_only("tblPhone", true);
                this.commons.to_unlock_button("tblbuttons", "btnNew~btnSearch");
                this.commons = null;
                this.generic = null;
                branch_definition.createAsterisk(this.BranchType);
            }
        }
    }

    this.push_data_to_server = function () {
        var data_row = new Array();
        data_row[data_row.length] = "BranchType~" + this.BranchType;
        data_row[data_row.length] = "BranchCode~" + this.BranchCode;
        data_row[data_row.length] = "BranchName~" + this.BranchName;
        data_row[data_row.length] = "NegBranchCode~" + this.NegBranchCode;
        data_row[data_row.length] = "CoveringBankCode~" + this.CoveringBankCode;
        data_row[data_row.length] = "FCBranchCode~" + this.FCBranchCode;
        data_row[data_row.length] = "Classification~" + this.Classification;
        data_row[data_row.length] = "ContactPerson~" + this.ContactPerson;
        data_row[data_row.length] = "Address~" + this.Address;
        /* ALESSA */
        data_row[data_row.length] = "ZipCode~" + this.ZipCode;
        data_row[data_row.length] = "State~" + this.State;
        data_row[data_row.length] = "Country~" + this.Country;
        data_row[data_row.length] = "Phone~" + this.Phone;
        data_row[data_row.length] = "Email~" + this.Email;
        data_row[data_row.length] = "Fax~" + this.Fax;
        data_row[data_row.length] = "TimeZone~" + this.TimeZone;
        data_row[data_row.length] = "ChargeGroup~" + this.ChargeGroup;
        data_row[data_row.length] = "Brstn~" + this.Brstn;
        data_row[data_row.length] = "CourierCode~" + this.CourierCode;
        data_row[data_row.length] = "PCHC~" + this.PCHC;
        data_row[data_row.length] = "Dom_Br_Loc~" + this.Dom_loc;
        data_row[data_row.length] = "CoveringBranchCode~" + this.CovBranch;
        data_row[data_row.length] = "BIC~" + this.BankInformationCode; /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        return data_row.join("|");
    }

    this.insert_record = function () {
        document.getElementById("txtHiddenSeqID").value = 0;
        this.clear();
        this.populatefield();
        this.hide_grid();
        branch_definition.add = true;
        IRSWorld.BranchSetup.Branch_Definition.insert_record(this.insert_record_callback);
    }

    this.insert_record_callback = function (result) {
        if (result != null && result.value != undefined) {
            if (result.value == false) {
                var msg = IRSWorld.BranchSetup.Branch_Definition.read_message(); alert(msg.value); return;
            }

            this.commons = new common_method();
            this.commons.to_read_only("tblBranchDesc", false);
            this.commons.to_read_only("tblPhone", false);
            this.commons.to_unlock_button("tblbuttons2", "btnSave~btnCancel2");
            this.commons = null;
        }
        branch_definition.loadAsterisk();
    }

    this.update_record = function () {
        document.getElementById("txtHiddenSeqID").value = "";
        branch_definition.add = false;
        if (this.SEQ_ID == null) {
            this.SEQ_ID = document.getElementById("txtHiddenSeqID").value;
        }

        if (this.SEQ_ID == null || this.SEQ_ID == "") {
            alert("RECORD NOT FOUND."); return;
        }
        IRSWorld.BranchSetup.Branch_Definition.update_record(this.SEQ_ID, this.update_record_callback);
    }

    this.update_record_callback = function (result) {
        if (result != null && result.value != undefined) {
            if (result.value == false) {
                var msg = IRSWorld.BranchSetup.Branch_Definition.read_message();
                alert(msg.value); return;
            }

            this.commons = new common_method();
            this.commons.to_read_only("tblBranchDesc", false);
            this.commons.to_read_only("tblPhone", false);
            this.commons.to_unlock_button("tblbuttons2", "btnSave~btnCancel2");
            this.commons = null;
        }
    }

    /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile  */
    this.delete_record = function () {

        if (confirm("Delete " + document.getElementById("txtBranchCode").value + " " + document.getElementById("txtBranchName").value + "?")) {
            IRSWorld.BranchSetup.Branch_Definition.delete_record(this.set_branch_definition(), this.delete_record_callback);
        }
    }

    this.delete_record_callback = function (result) {
        if (result != null && result.value != undefined) {
            if (result.value == false) {
                var msg = IRSWorld.BranchSetup.Branch_Definition.read_message(); alert(msg.value); return;
            }

            this.SEQ_ID = IRSWorld.BranchSetup.Branch_Definition.BRANCH_DEFINITION_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons", "btnNew~btnSearch");
            this.commons = null;

            //alert("RECORD COMITTED SUCCESSFULLY");
            alert("Successfully submitted for approval");

            document.getElementById("txtHiddenSeqID").value = "";
            branch_definition.pull_server_data();
            branch_definition.show_grid();
        }
    }
    /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile  */

    this.commit_record = function () {
        if (guserrolecode == '2') {
            alert('Approver is not allowed to modify current set-up.')
            return;
        }

        if (typeof (this.validate_input) != "undefined") {
            if (this.validate_input() == false) {
                alert(this.error_input); return;
            }
        }

        IRSWorld.BranchSetup.Branch_Definition.commit_record(this.set_branch_definition(), this.commit_record_callback);
    }

    this.commit_record_callback = function (result) {
        if (result != null && result.value != undefined) {
            if (result.value == false) {
                var msg = IRSWorld.BranchSetup.Branch_Definition.read_message(); alert(msg.value); return;
            }

            this.SEQ_ID = IRSWorld.BranchSetup.Branch_Definition.BRANCH_DEFINITION_SEQ_ID().value;
            this.commons = new common_method();
            this.commons.to_unlock_button("tblbuttons", "btnNew~btnSearch");
            this.commons = null;

            //alert("RECORD COMITTED SUCCESSFULLY");
            alert("Successfully submitted for approval");

            document.getElementById("txtHiddenSeqID").value = "";
            branch_definition.pull_server_data();
            branch_definition.show_grid();
        }
    }

    this.rollback_record = function () {
        //IRSWorld.BranchSetup.Branch_Definition.rollback_record(this.rollback_record_callback);
        this.commons = new common_method();
        this.commons.to_clear_screen("tblBranchDesc");
        this.commons.to_clear_screen("tblbuttons");
        this.commons.to_read_only("tblBranchDesc", true);
        this.commons.to_unlock_button("tblbuttons", "btnNew~btnSearch");
        this.commons = null;
        branch_definition.pull_server_data();
    }

    this.rollback_record_callback = function (result) {
        var msg = IRSWorld.BranchSetup.Branch_Definition.read_message();

        if (msg != null && msg.value.length > 0) {
            alert(msg.value);
        }
    }

    this.clear = function () {
        this.commons = new common_method();
        this.commons.to_clear_screen("tblBranchDesc");
        this.commons.to_clear_screen("tblphone");
        this.commons = null;
    }

    this.do_ajaxgridview = function (parameter) {
        var command_param = null;
        this.ajax_grid.process_ajax(parameter);

        if (parameter != null && parameter != undefined) {
            command_param = parameter.split("$");
            if (command_param.length > 0) {
                document.getElementById("txtHiddenSeqID").value = command_param[1];
                if (command_param[0] == "ajx_selected") {
                    this.SEQ_ID = command_param[1];
                    branch_definition.pull_server_data();
                }
                else {
                    this.process_grid_viewer(parameter);
                }
            }
        }
    }

    this.loadAsterisk = function () {
        document.getElementById("astBranchType").style.display = "";
        document.getElementById("astBranchCode").style.display = "none";
        document.getElementById("astBranchName").style.display = "none";
        document.getElementById("astChargeGroup").style.display = "none";
        document.getElementById("astNegBranchCode").style.display = "none";
        document.getElementById("astCovCode").style.display = "none";
        document.getElementById("astSIBranchCode").style.display = "none";
        document.getElementById("astBRSTN").style.display = "none";
        document.getElementById("astCourierCode").style.display = "none";
        document.getElementById("astPCHC").style.display = "none";
        document.getElementById("astDomesticLocation").style.display = "none";
        document.getElementById("astContactPerson").style.display = "none";
        document.getElementById("astAddress").style.display = "none";
        document.getElementById("astZipCode").style.display = "none";
        document.getElementById("astState").style.display = "none";
        document.getElementById("astCountry").style.display = "none";
        document.getElementById("astPhone").style.display = "none";
        document.getElementById("astFax").style.display = "none";
        document.getElementById("astEmail").style.display = "none";
        document.getElementById("astTimeZone").style.display = "none";
        document.getElementById("astBankInformationCode").style.display = "none";  /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile  */
    }

    this.load_all_dropdownlist = function () {
        this.dropdown_branch_type();
        this.dropdown_country();
        this.dropdown_chargegroup();
        this.dropdown_timezone();
        this.dropdown_pchc();
        this.dropdown_dom_loc();
        this.coveringcode();
        this.coveringbranchcode();
    }

    this.dropdown_branch_type = function () {
        IRSWorld.BranchSetup.Branch_Definition.getdropdown_source("BranchType", this.dropdown_branch_type_callback);
    }

    this.dropdown_branch_type_callback = function (result) {
        if (result != null && result != undefined) {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddBranchType", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    }

    this.dropdown_country = function () {
        IRSWorld.BranchSetup.Branch_Definition.getdropdown_source2("Country", this.dropdown_country_callback);
    }

    this.dropdown_country_callback = function (result) {
        if (result != null && result != undefined) {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCountry", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }

    this.dropdown_chargegroup = function () {
        IRSWorld.BranchSetup.Branch_Definition.getdropdown_source2("ChargeGroup", this.dropdown_chargegroup_type_callback);
    }

    this.dropdown_chargegroup_type_callback = function (result) {
        if (result != null && result != undefined) {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddChargeGroup", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }

    this.dropdown_timezone = function () {
        IRSWorld.BranchSetup.Branch_Definition.getTimeZone(this.dropdown_timezone_type_callback);
    }

    this.dropdown_timezone_type_callback = function (result) {
        var opts = new Array(result.value.length);
        for (var i = 0; i < result.value.length; i++) {
            opts[i] = result.value[i] + "~" + result.value[i];
        }

        var ddl = document.getElementById("ddTimeZone");
        set_ddl_options(ddl, opts);
    }

    this.dropdown_pchc = function () {
        IRSWorld.BranchSetup.Branch_Definition.getdropdown_source("PCHCMember", this.dropdown_pchc_callback);
    }

    this.dropdown_pchc_callback = function (result) {
        if (result != null && result != undefined) {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddPCHC", result.value, "FldValue", "FldCode");
            this.commons = null;
        }
    }

    this.dropdown_dom_loc = function () {
        IRSWorld.BranchSetup.Branch_Definition.getdropdown_source2("BrDomesticLocation", this.dropdown_dom_loc_callback);
    }

    this.dropdown_dom_loc_callback = function (result) {
        if (result != null && result != undefined) {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddDom_loc", result.value, "FldValue", "FldDescription");
            this.commons = null;
        }
    }

    this.coveringcode = function () {
        IRSWorld.BranchSetup.Branch_Definition.get_coveringbankcode(this.coveringcode_callback);
    }

    this.coveringcode_callback = function (result) {
        if (result != null && result != undefined) {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCovCode", result.value, "FldCode", "FldCode");
            this.commons = null;
        }
    }

    this.coveringbranchcode = function () {
        IRSWorld.BranchSetup.Branch_Definition.getdropdown_source("COVBANKBRANCHCODE", this.coveringbranchcode_callback);
    }

    this.coveringbranchcode_callback = function (result) {
        if (result != null && result != undefined) {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddCovBranch", result.value, "FldCode", "FldCode");
            this.commons = null;
        }
    }

    this.getphone = function () {
        this.Phone = document.getElementById("txtPhone1").value + "-" + document.getElementById("txtPhone2").value + "-" + document.getElementById("txtPhone3").value;
        var phone_nos = this.Phone.split("-");

        document.getElementById("txtPhone1").value = phone_nos[0];
        document.getElementById("txtPhone2").value = phone_nos[1];
        document.getElementById("txtPhone3").value = phone_nos[2];
    }

    this.checkRegex = function (type, val)
    {
        var alphanumeric = /^([a-zA-Z0-9]+)$/;
        var email = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
        //var numeric = /^([0-9-]+)$/;
        var numeric = /^\d+$/;
        var ret;
        switch (type) {
            case 1: //alpha-numeric
                ret = alphanumeric.test(val);
                break;

            case 2: //email
                ret = email.test(val);
                break;

            case 3: //numeric
                ret = numeric.test(val);
                break;
        }
        return ret;
    }

    this.validate_input = function () {
        this.commons = new common_method();
        this.generic = new generic_method();
        this.error_input = "";
        this.getphone();
        this.BranchCode = document.getElementById("txtBranchCode").value;
        var rs = IRSWorld.BranchSetup.Branch_Definition.check_branches(this.BranchCode, document.getElementById("ddBranchType").value).value;

        //Branch Type
        if (document.getElementById("astBranchType").style.display == "") {
            if (document.getElementById("ddBranchType").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (Branch Type)\n";
                document.getElementById("ddBranchType").focus();
                return false;
            }
        }

        //Branch Code
        if (document.getElementById("astBranchCode").style.display == "") {
            if (document.getElementById("txtBranchCode").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Branch Code)\n";
                document.getElementById("txtBranchCode").focus();
                return false;
            }
        }
        if (document.getElementById("txtBranchCode").value != "") {
            if (!this.checkRegex(1, document.getElementById("txtBranchCode").value)) {
                this.error_input += "Invalid input found!. Only Alpha, Numeric, or Combination of Alpha Numeric is allowed. Please try again. (Branch Code)\n";
                document.getElementById("txtBranchCode").focus();
                return false;
            }
        }

        //Branch Name
        if (document.getElementById("astBranchName").style.display == "") {
            if (document.getElementById("txtBranchName").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Branch Name)\n";
                document.getElementById("txtBranchName").focus();
                return false;
            }
        }

        //Charge Group
        if (document.getElementById("astChargeGroup").style.display == "") {
            if (document.getElementById("ddChargeGroup").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (Charge Group)\n";
                document.getElementById("ddChargeGroup").focus();
                return false;
            }
        }

        //Negotiating Branch Code
        if (document.getElementById("astNegBranchCode").style.display == "") {
            if (document.getElementById("txtNegBranchCode").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Negotiating Code)\n";
                document.getElementById("txtNegBranchCode").focus();
                return false;
            }
        }

        //Covering Code
        if (document.getElementById("astCovCode").style.display == "") {
            if (document.getElementById("ddCovCode").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (Covering Code)\n";
                document.getElementById("ddCovCode").focus();
                return false;
            }
            if (document.getElementById("ddCovBranch").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (Covering Code)\n";
                document.getElementById("ddCovBranch").focus();
                return false;
            }
        }

        //SI Branch Code
        if (document.getElementById("astSIBranchCode").style.display == "") {
            if (document.getElementById("txtFCBranchCode").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (SI Branch Code)\n";
                document.getElementById("txtFCBranchCode").focus();
                return false;
            }
        }

        //BRSTN
        if (document.getElementById("astBRSTN").style.display == "") {
            if (document.getElementById("txtBrstn").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (BRSTN)\n";
                document.getElementById("txtBrstn").focus();
                return false;
            }
        }

        //Courier Code
        if (document.getElementById("astCourierCode").style.display == "") {
            if (document.getElementById("txtCourierCode").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Courier Code)\n";
                document.getElementById("txtCourierCode").focus();
                return false;
            }
        }

        //PCHC
        if (document.getElementById("astPCHC").style.display == "") {
            if (document.getElementById("ddPCHC").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (PCHC)\n";
                document.getElementById("ddPCHC").focus();
                return false;
            }
        }

        //Domestic Location
        if (document.getElementById("astDomesticLocation").style.display == "") {
            if (document.getElementById("ddDom_loc").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (Domestic Location)\n";
                document.getElementById("ddDom_loc").focus();
                return false;
            }
        }

        //Contact Person
        if (document.getElementById("astContactPerson").style.display == "") {
            if (document.getElementById("txtContactPerson").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Contact Person)\n";
                document.getElementById("txtContactPerson").focus();
                return false;
            }
        }

        //Address
        if (document.getElementById("astAddress").style.display == "") {
            if (document.getElementById("txtAddress").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Address)\n";
                document.getElementById("txtAddress").focus();
                return false;
            }
        }
        if (document.getElementById("astZipCode").style.display == "") {
            if (document.getElementById("txtZipCode").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Zip Code)\n";
                document.getElementById("txtZipCode").focus();
                return false;
            }
        }

        //State
        if (document.getElementById("astState").style.display == "") {
            if (document.getElementById("txtState").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (State)\n";
                document.getElementById("txtState").focus();
                return false;
            }
        }

        //Country
        if (document.getElementById("astCountry").style.display == "") {
            if (document.getElementById("ddCountry").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (Country)\n";
                document.getElementById("ddCountry").focus();
                return false;
            }
        }

        //Phone
        if (document.getElementById("astPhone").style.display == "") {
            if (document.getElementById("txtPhone1").value == "") { //|| isNaN(document.getElementById("txtPhone1").value)) {
                this.error_input += "Incomplete information found! Please check input and try again. (Country Code)\n";
                document.getElementById("txtPhone1").focus();
                return false;
            }
            if (document.getElementById("txtPhone2").value == "") { //|| isNaN(document.getElementById("txtPhone2").value)) {
                this.error_input += "Incomplete information found! Please check input and try again. (Area Code)\n";
                document.getElementById("txtPhone2").focus();
                return false;
            }
            if (document.getElementById("txtPhone3").value == "") { //|| isNaN(document.getElementById("txtPhone3").value)) {
                this.error_input += "Incomplete information found! Please check input and try again. (Phone Number)\n";
                document.getElementById("txtPhone3").focus();
                return false;
            }
        }
        /*
        if (document.getElementById("txtPhone1").value != "") {
            if (!this.checkRegex(3, document.getElementById("txtPhone1").value)) {
                this.error_input += "Invalid input found!. Phone number is not the correct format. Please try again. (Country Code)\n";
                document.getElementById("txtPhone1").focus();
                return false;
            }
        }
        if (document.getElementById("txtPhone2").value != "") {
            if (!this.checkRegex(3, document.getElementById("txtPhone2").value)) {
                this.error_input += "Invalid input found!. Phone number is not the correct format. Please try again. (Area Code)\n";
                document.getElementById("txtPhone2").focus();
                return false;
            }
        }
        if (document.getElementById("txtPhone1").value != "") {
            if (!this.checkRegex(3, document.getElementById("txtPhone3").value)) {
                this.error_input += "Invalid input found!. Phone number is not the correct format. Please try again. (Phone Number)\n";
                document.getElementById("txtPhone3").focus();
                return false;
            }
        }
        */

        //Fax
        if (document.getElementById("astFax").style.display == "") {
            if (document.getElementById("txtFax").value == "" || isNaN(document.getElementById("txtFax").value)) {
                this.error_input += "Incomplete information found or data is not numeric! Please check input and try again. (Fax)\n";
                document.getElementById("txtFax").focus();
                return false;
            }
        }

        //Email
        if (document.getElementById("astEmail").style.display == "") {
            if (document.getElementById("txtEmail").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (Email)\n";
                document.getElementById("txtEmail").focus();
                return false;
            }
        }
        /*
        if (document.getElementById("txtEmail").value != "") {
            if (!this.checkRegex(2, document.getElementById("txtEmail").value)) {
                this.error_input += "Invalid input found!. Email address is not the correct format. Please try again. (Email)\n";
                document.getElementById("txtBranchCode").focus();
                return false;
            }
        }
        */

        //Time Zone
        if (document.getElementById("astTimeZone").style.display == "") {
            if (document.getElementById("ddTimeZone").selectedIndex <= 0) {
                this.error_input += "Incomplete information found! Please check input and try again. (Time Zone)\n";
                document.getElementById("ddTimeZone").focus();
                return false;
            }
        }

        //BIC /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile  */
        if (document.getElementById("astBankInformationCode").style.display == "") {
            if (document.getElementById("txtBankInformationCode").value == "") {
                this.error_input += "Incomplete information found! Please check input and try again. (BIC)\n";
                document.getElementById("txtBankInformationCode").focus();
                return false;
            }
        }
        
        //Validate if duplicate
        if (branch_definition.add) {
            if (rs != null && rs.Tables[0].Rows.length > 0) {
                this.error_input += "Branch Code already exists/active.\n";
                document.getElementById("txtBranchCode").focus();
                return false;
            }
        }
        
        if (this.error_input != "") {
            return false;
        }
        return true;
    }

    this.hide_panel = function () {
        document.getElementById("BranchDefEntry").style.display = "none";
        document.getElementById("BranchDefBtn").style.display = "";
        document.getElementById("BranchDefBtn2").style.display = "none";
    }

    this.hide_grid = function () {
        document.getElementById("BranchDefEntry").style.display = "";
        document.getElementById("BranchDefBtn2").style.display = "";
        document.getElementById("BranchDefBtn").style.display = "none";

        this.commons = new common_method();
	/* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        this.commons.to_unlock_button("tblbuttons2", "btnEdit~btnCancel2~btnDelete");
        this.commons = null;
    }

    this.show_grid = function () {
        document.getElementById("BranchDefEntry").style.display = "none";
        document.getElementById("BranchDefBtn").style.display = "";
        document.getElementById("BranchDefBtn2").style.display = "none";
    }

    this.panel_search_branch = function () {
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "sp_branchsearch~" +
                                document.getElementById("txtSearchBCode").value + "~" +
                                document.getElementById("txtSearchBName").value + "~";
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, this.SEQ_ID);
    }

    this.do_ajax = function (parameter) {
        this.ajax_grid.process_ajax(parameter);
        var command_param = null;

        if (parameter != null && parameter != undefined) {
            command_param = parameter.split("$");
            if (command_param.length > 0) {
                document.getElementById("txtHiddenSeqID").value = command_param[1];
                if (command_param[0] == "ajx_selected") {
                    this.SEQ_ID = command_param[1];
                    branch_definition.pull_server_data();
                }
            }
        }
    }

    this.populatefield = function () {
        if (document.getElementById("txtSearchBCode").value != "" || document.getElementById("txtSearchBName").value != "") {
            document.getElementById("txtBranchCode").value = document.getElementById("txtSearchBCode").value;
            document.getElementById("txtBranchName").value = document.getElementById("txtSearchBName").value;
        }
    }

    this.createAsterisk = function () {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function () {
            alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
        return IRSWorld.BranchSetup.Branch_Definition.getAsterisk(document.getElementById("ddBranchType").value, this.createAsterisk_callback);
    }

    this.createAsterisk_callback = function (result) {
        if (result != null && result != undefined) {
            var asterisk = "";
            for (var i = 0; i <= (result.value.Rows.length - 1) ; i++) {
                asterisk = result.value.Rows[i]["Asterisk"].toString();
            }

            document.getElementById("lblNegotiatingCode").innerText = "Negotiating Code";
            document.getElementById("lblCoveringCode").innerText = "Covering Code";
            document.getElementById("lblFlexcubeCode").innerText = "SI Code";
            
            var arr = asterisk.split('~');
            if (arr != null && arr.length > 0) {
                document.getElementById("astBranchType").style.display = "";
                document.getElementById("astBranchCode").style.display = "none";
                document.getElementById("astBranchName").style.display = "none";
                document.getElementById("astChargeGroup").style.display = "none";
                document.getElementById("astNegBranchCode").style.display = "none";
                document.getElementById("astCovCode").style.display = "none";
                document.getElementById("astSIBranchCode").style.display = "none";
                document.getElementById("astBRSTN").style.display = "none";
                document.getElementById("astCourierCode").style.display = "none";
                document.getElementById("astPCHC").style.display = "none";
                document.getElementById("astDomesticLocation").style.display = "none";
                document.getElementById("astContactPerson").style.display = "none";
                document.getElementById("astAddress").style.display = "none";
                document.getElementById("astZipCode").style.display = "none";
                document.getElementById("astState").style.display = "none";
                document.getElementById("astCountry").style.display = "none";
                document.getElementById("astPhone").style.display = "none";
                document.getElementById("astFax").style.display = "none";
                document.getElementById("astEmail").style.display = "none";
                document.getElementById("astTimeZone").style.display = "none";
                document.getElementById("astBankInformationCode").style.display = "none"; /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */

                for (var i = 0; i <= (arr.length - 1) ; i++)
                {
                    document.getElementById(arr[i]).style.display = "";
                }
            }
        }
    }
}