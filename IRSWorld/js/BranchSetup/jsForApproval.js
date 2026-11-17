
function page_setup() {
    forapprovaltagging.Load_Branches();
}

function forapprovaltagging() {

    //this.LoadDropdown = function () {
    this.Load_Branches = function () {
        IRSWorld.BranchSetup.ForApprovalTag.load_branches(forapprovaltagging.load_branches_callback);
    }

    this.load_branches_callback = function (result) {
        if (result.value != null && result.value != "undefined") {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("cmbBranches", result.value, "BranchCode", "BranchName");
        }
    }
    //}

    this.oncheckChanged = function () {
        if (document.getElementById('chkApplyToAll').checked) {
            document.getElementById('cmbBranches').disabled = 'disabled';
        }
        else {
            var branch = document.getElementById('cmbBranches').value;
            if (branch != "null") {
                IRSWorld.BranchSetup.ForApprovalTag.getForApprovalSetUp(branch, forapprovaltagging.getForApprovalSetUp_callback);
            }
            else {
                document.getElementById('cmbBranches').disabled = '';
                document.getElementById('chkApplyToAll').checked = false;
                document.getElementById('chkForApproval').checked = false;
            }
        }
    }

    this.getForApprovalSetUp_callback = function (result) {
        AjaxPro.timeoutPeriod = 10000 * 1000;
        AjaxPro.onTimeout = function () {
            alert("SERVER NOT RESPONDING.");
        }
        if (result != null && result.value == true) {
            document.getElementById('chkForApproval').checked = true;
        }
        else { document.getElementById('chkForApproval').checked = false; }
    }

    this.apply_changes = function () {
        var applyall = document.getElementById('chkApplyToAll').checked;
        var brcode = document.getElementById('cmbBranches').value;
        var approved = document.getElementById('chkForApproval').checked;

        IRSWorld.BranchSetup.ForApprovalTag.apply_changes(applyall, brcode, approved, forapprovaltagging.apply_changes_callback);

    }

    this.apply_changes_callback = function (result) {
        if (result.value == true) {
            alert('Changes successfully commited.');
            document.getElementById('chkApplyToAll').checked = false;
            document.getElementById('chkForApproval').checked = false;
            document.getElementById('cmbBranches').selectedIndex = 0;
        }
        else { alert('Please select value to update.'); }
    }
}