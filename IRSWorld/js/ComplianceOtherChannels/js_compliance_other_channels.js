
function compliance_otherchannels()
{
    this.commons = null;



    this.load_all_dropdownlist = function()
    {
        this.dropdown_fields();
    }
    
    this.dropdown_fields = function()
    {
        ComplianceLoader.getdropdown_source("BranchType", this.dropdown_branch_type_callback)
    }
    this.dropdown_branch_type_callback = function(result)
    {
        if (result != null && result != "undefined")
        {
            this.commons = new common_method();
            this.commons.to_load_dropdownlist("ddRecordField", result.value, "valueher", "descriptionhere");
            this.commons = null;
        }        
    }
    
    this.commit_record = function()
    {
        if (document.getElementId("txtFileFormat").value == "" || document.getElementId("txtFileDescription").value == "" || document.getElementId("txtFileSequenceNo").value == "" || document.getElementId("txtFileNameVerifier").value =="" || document.getElementId("txtDelimiter").value == "" || document.getElementId("txtRecordLineVerifierPos").value == "" || document.getElementId("txtRecordLineVerifierLen").value == "" || document.getElementId("txtRecordLineHint").value == "" || document.getElementId("txtRecordLineDesc").value == "" || document.getElementId ("txtRecordPosition").value == "" || document.getElementId("txtRecordLen").value =="" || document.getElementId("txtDestinationTable").value == "")
        {
            alert ("Please complete all necessary fields")
            return;
        }
        ComplianceLoader.
    }
}