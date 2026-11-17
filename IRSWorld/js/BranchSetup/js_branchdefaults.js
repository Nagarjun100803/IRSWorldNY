var dbgrid;
var selected_form;
var iclick;
var dbgrid_vip;
var myrow=new Array();
var typerow=new Array();
var guserrolecode = '0';

$(document).ready(function () {
    getrolecode();
});
function getrolecode() {
    try {
        IRSWorld.BranchSetup.BranchDefaults.getrolecode(function (result) {
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

function fixht()
{
    if (top.document.getElementById('divCenter'))
    {
        document.getElementById('divBody').style.height = (top.document.getElementById('divCenter').offsetHeight - 30) + 'px';
        document.getElementById('divBody').style.width = (top.document.getElementById('divCenter').offsetWidth - 3) + 'px';
    
        document.getElementById('tblContent').style.width = document.getElementById('divBody').style.width;
        document.getElementById('tdContent').style.width = document.getElementById('divBody').style.width;
    }
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
function getDropDown()
{
        this.commons           = null;  
        this.Load_BenType = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getBenType(this.load_ddlBenType_callback);
        }
        
        this.load_ddlBenType_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlBenType", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_NotifyMode = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getNotifyMode(this.load_ddlNotify_callback);
        }
        
        this.load_ddlNotify_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlNotify", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_RelationToRemitter = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getRelationToRemitter(this.load_ddlRelation_callback);
        }
        
        this.load_ddlRelation_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRelation", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Country = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getCountry(this.load_ddlCountry_callback);
        }
        
        this.load_ddlCountry_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlCountry", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlCountry2", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_RiskRating = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getRiskRating(this.Load_RiskRating_callback);
        }
        
        this.Load_RiskRating_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRiskRating", result.value, "fldvalue", "flddescription");
            }
        }
        
        
        this.Load_ProductCode = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getProductCode(this.load_ddlProductCode_callback);
        }
        
        this.load_ddlProductCode_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlProductCode", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_PayMode = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getPayMode(this.load_ddlPayMode_callback);
        }
        
        this.load_ddlPayMode_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlPayMode", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_PayMode = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getPayMode(this.load_ddlPayMode_callback);
        }
        
        this.load_ddlPayMode_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlPayMode", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Branches = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.GetBranches(this.load_ddlBranches_callback);
        }
        
        this.load_ddlBranches_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlBranches", result.value, "BranchCode", "BranchName");
            }
        }
        
        this.Load_LocalCurrency= function()
        {
            var brCode_loc=document.getElementById('ddlBranches').value;
            IRSWorld.BranchSetup.BranchDefaults.getLocalCurrency(brCode_loc,this.load_ddlLocalCurrency_callback);
        }
        
        this.load_ddlLocalCurrency_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlLocalCurrency", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_ReceiptMode = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getReceiptMode(this.load_ddlReceiptMode_callback);
        }
        
        this.load_ddlReceiptMode_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlReceiptMode", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_ImplemType = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getImplementationType(this.Load_ImplemType_callback);
        }
        
        this.Load_ImplemType_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlImplemType", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_ReceiptMode = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getReceiptMode(this.load_ddlReceiptMode_callback);
        }
        
        this.load_ddlReceiptMode_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlReceiptMode", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_AccountType = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getAccountType(this.load_ddlAccountType_callback);
        }
        
        this.load_ddlAccountType_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlAccountType", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_AccountCurrency = function()
        {
            var brCode_acct=document.getElementById('ddlBranches').value;
            IRSWorld.BranchSetup.BranchDefaults.getAccountCurrency(brCode_acct,this.load_ddlAccountCurrency_callback);
        }
        
        this.load_ddlAccountCurrency_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlAccountCurrency", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_CoveringBank = function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getCoveringBank(this.load_ddlCoveringBank_callback);
        }
        
        this.load_ddlCoveringBank_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlCoveringBank", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_RemitCurrency= function()
        {
            var brCode_remit=document.getElementById('ddlBranches').value;
            IRSWorld.BranchSetup.BranchDefaults.getLocalCurrency(brCode_remit,this.load_ddlRemitCurrency_callback);
        }
        
        this.load_ddlRemitCurrency_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRemitCurr", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_RapidRemit= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getRapidRemit(this.load_ddlRapid_callback);
        }
        
        this.load_ddlRapid_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRapid", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_TransactionSource= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getTransactionSource(this.load_ddltrnxSource_callback);
        }
        
        this.load_ddltrnxSource_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddltrnxSource", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_TransactionPurpose= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getTransactionPurpose(this.load_ddltrnxPurpose_callback);
        }
        
        this.load_ddltrnxPurpose_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddltrnxPurpose", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Computation= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getComputation(this.load_ddlComputation_callback);
        }
        
        this.load_ddlComputation_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlComputation", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_SourceOfFunds= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getSourceOfFunds(this.load_ddlSourceOfFunds_callback);
        }
        
        this.load_ddlSourceOfFunds_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlSourceOfFunds", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_RoundUpOBPS= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getRoundUpOBPS(this.load_ddlRoundUp_callback);
        }
        
        this.load_ddlRoundUp_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRoundUp", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_ExcessSSS= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getRoundUpOBPS(this.load_ddlExcess_callback);
        }
        
        this.load_ddlExcess_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlExcess", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_RemittanceAggregation= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getRemitAggregation1(this.load_ddlDateValue1_callback);
        }
        
        this.load_ddlDateValue1_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlDateValue1", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlDateValue2", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlDateValue3", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlCashAggValue1", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlAgentAggValue1", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_WorkerType= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getWorkerType(this.load_ddlWorkerType_callback);
        }
        
        this.load_ddlWorkerType_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlWorkType", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_RemitterType= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getRemitterType(this.load_ddlRemitterType_callback);
        }
        
        this.load_ddlRemitterType_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRemitterType", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_ChargeType= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getChargeType(this.load_ddlRemitChargeType_callback);
        }
        
        this.load_ddlRemitChargeType_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlRemitChargeType", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlChargeTypePHP", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlChargeTypeUSD", result.value, "fldvalue", "flddescription");
                this.commons.to_load_dropdownlist("ddlChargeTypeEUR", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_VIPStatus= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getVIPStatus(this.load_ddlVipStat_callback);
        }
        
        this.load_ddlVipStat_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlVipStat", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Gender= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getGender(this.load_ddlGender_callback);
        }
        
        this.load_ddlGender_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlGender", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_CivilStatus= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getCivilStatus(this.load_ddlCivilStatus_callback);
        }
        
        this.load_ddlCivilStatus_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlCivilStat", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Occupation= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getOccupation(this.load_ddlOccupation_callback);
        }
        
        this.load_ddlOccupation_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlOccupation", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Citizenship= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getCitizenship(this.load_ddlCitizenship_callback);
        }
        
        this.load_ddlCitizenship_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlCitizenship", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Mail= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getMail(this.load_ddlMail_callback);
        }
        
        this.load_ddlMail_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlMail", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_Contact= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getContact(this.load_ddlContact_callback);
        }
        
        this.load_ddlContact_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlContact", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_ChargeGroup= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getChargeGroup(this.load_ddlChargeGroup_callback);
        }
        
        this.load_ddlChargeGroup_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlChargeGroup", result.value, "fldvalue", "flddescription");
            }
        }
        
        
        this.Load_GFCChargeType= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.GFCgetChargeType(this.load_ddlGFCCharge_callback);
        }
        
        this.load_ddlGFCCharge_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("ddlGFCCharge", result.value, "fldvalue", "flddescription");
            }
        }
        
        this.Load_VIPType= function()
        {
            IRSWorld.BranchSetup.BranchDefaults.getVIPType(this.load_cmbVIP_callback);
        }
        
        this.load_cmbVIP_callback = function(result)
        {
            if (result.value != null && result.value != "undefined")
            {
                this.commons = new common_method();
                this.commons.to_load_dropdownlist("cmbVipType", result.value, "fldvalue", "flddescription");
            }
        }
}


function create_grid()
{
    dbgrid = new datagrid();
    {
        var col = new datagrid_column();
        col.header = '';
        col.datafield = 'checked';
        col.align = 'left';
        col.type= 'check';
        col.onclick = 'validate_check_field';
        col.name = 'BranchDefaultSetup'
        col.styles.cell += 'width:5%';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Code';
        col.datafield = 'fldvalue';
        col.align = 'left';
        col.styles.cell += 'width:3%';
        dbgrid.addColumn(col);
    }
   {
        var col = new datagrid_column();
        col.header = 'Description';
        col.datafield = 'flddescription';
        col.align = 'right';
        col.styles.cell += 'width:20%';
        dbgrid.addColumn(col);
    }
    {
        var col = new datagrid_column();
	    col.header = '';
	    col.datafield = '';
	    col.align = 'center';
	    col.type = 'button';
	    col.value = 'Edit';
	    col.edit_button = true;
	    col.styles.cell += 'width:2%';
	    col.styles.datafield += 'width:100%;';
	    dbgrid.addColumn(col);
        }  
    dbgrid.caption = 'Default Setup';
    dbgrid.classNames.table = 'table_body';
//    dbgrid.footer.save_button.none = false;
//    dbgrid.footer.save_button.style += 'background-color: lavender;'; 
    dbgrid.rows_page = 10;
    dbgrid.row.normal_style = 'background-color: #f7ffff;'; 
}

function create_grid_vip()
{
    dbgrid_vip = new datagrid();
    {
        var col = new datagrid_column();
        col.header = 'VIP Type';
        col.options = myrow;
        col.datafield = 'VipType';
        col.type = 'Select';
        col.align = 'left';
        col.styles.cell += 'width:30%';
        dbgrid_vip.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Type';
        col.options = typerow;
        col.datafield = 'DataType';
        col.type = 'Select';
        col.align = 'left';
        col.styles.cell += 'width:23%';
        dbgrid_vip.addColumn(col);
    }
    {
        var col = new datagrid_column();
        col.header = 'Value';
        col.datafield = 'Value';
        col.align = 'left';
        col.styles.cell += 'width:13%';
        dbgrid_vip.addColumn(col);
    }
    {
	    var col = new datagrid_column();
	    col.header = 'SEQ ID';
	    col.styles.header += 'display:none;';
	    col.styles.datafield += 'display:none;';
	    col.datafield = 'SEQ_ID';
	    col.align = 'left';
	    col.styles.cell += 'width:8%;display:none;';
	    dbgrid_vip.addColumn(col);
    }
    
    dbgrid_vip.caption = 'VIP Type';
    dbgrid_vip.classNames.table = 'table_body';
    dbgrid_vip.rows_page = 10;
    dbgrid_vip.row.normal_style = 'background-color: #f7ffff;'; 
    
    dbgrid_vip.footer.edit_button.none = false;
    dbgrid_vip.footer.edit_button.style += 'background-color: lavender;color:maroon;';            
    dbgrid_vip.footer.edit_button.beforepost += '';
    
    dbgrid_vip.footer.add_button.none = false;
    dbgrid_vip.footer.add_button.style += 'background-color: lavender;color:maroon;';            
    dbgrid_vip.footer.add_button.beforepost = 'add_before_post';
    
    dbgrid_vip.footer.save_button.none = false;
    dbgrid_vip.footer.save_button.onclick = 'save_vip';  
    dbgrid_vip.footer.save_button.style += 'background-color: lavender;'; 
    
    dbgrid_vip.footer.delete_button.none = false;
    dbgrid_vip.footer.cancel_button.onclick = 'populate_rec';  
    dbgrid_vip.footer.cancel_button.none = false;
    dbgrid_vip.footer.cancel_button.style += 'background-color: lavender;';
}

function add_before_post(args)
{
    var amount = dbgrid_vip.getValueAt(args.datarow,2);
    var type = dbgrid_vip.getValueAt(args.datarow,1);
    var viptype = dbgrid_vip.getValueAt(args.datarow,0);
    
    /*check setup*/
    var is_setup = IRSWorld.BranchSetup.BranchDefaults.checkSetupVIP(document.getElementById('ddlBranches').value,viptype).value;
    if(!is_setup)
    {
        if(type == "Percentage")
        {
            if(amount == "")
            {
                alert("Please enter a value");
                dbgrid_vip.curract = '';
                return;
            }
            else if(parseFloat(amount) > 1)
            {
                alert("Maximum percentage must not exceed to 1%");
                dbgrid_vip.curract = '';
                return;
            }
        }
        else
        {
            if(amount == "")
            {
                alert("Please enter a value");
                dbgrid_vip.curract = '';
                return;
            }
            else if(parseFloat(amount) > 2)
            {
                alert("Maximum amount must not exceed to 2");
                dbgrid_vip.curract = '';
                return;
            }
        }
    }
    else
    {
        alert("Setup already exist for VIP Type: " + viptype);
        dbgrid_vip.curract = '';
        return;
    }
    
}



function load_type(result)
{
    if (result.error != null)
    {
        spawn_errpage(result.error.Message);
        return;
    }
    create_grid_vip();
    dbgrid_vip.bind(result.value, 'tdVIP','viptable');
}



function get_viptype()
{
    var rows =  IRSWorld.BranchSetup.BranchDefaults.getVIPType().value.Rows;
    myrow = new Array();
    for(var i = 0;i <= rows.length - 1;i++)
    {
        myrow[myrow.length] = rows[i].fldvalue + '~' + rows[i].flddescription;
    }
}

function getType()
{
    typerow = new Array();
    typerow[typerow.length] = 'Percentage~Percentage'; 
    typerow[typerow.length] = 'Amount~Amount';
}


function page_setup()
{
    ajax_show_status(true);
    fixmain();
    document.getElementById('btnSearch').disabled = true;
    document.getElementById('ddlBranches').disabled = true;
    new getDropDown().Load_ProductCode();
    new getDropDown().Load_Branches();
    load_dropdown();
    //populate_rec();
    set_visibility('Beneficiary','false');
    set_init_value();   
}

function set_init_value()
{
    /*
    this.commons = new common_method();
    var loc = IRSWorld.BranchSetup.BranchDefaults.getStartUpLocation();
    this.commons.to_set_dropdownlist_index('ddlBranches',loc.value);
    */
    IRSWorld.BranchSetup.BranchDefaults.getStartUpLocation(
        function (rs)
        {
            if (rs.error == null)
            {
                document.getElementById('ddlBranches').value = rs.value;
            }
            document.getElementById('ddlBranches').disabled = false;
            document.getElementById('btnSearch').disabled = false;
            ajax_show_status(false);
        }
    );
}

function bind_grid(defaults)
{
    if (defaults.error != null)
    {
        spawn_errpage(defaults.error.Message);
        return;
    }
    
    create_grid();
    ajax_show_status(true);
    dbgrid.bind(defaults.value, 'tdDefaults', 'gridDefaults');
    ajax_show_status(false);
} 

function populate_rec()
{
    var branchcode=document.getElementById('ddlBranches').value;
    var prodcode = (document.getElementById('ddlProductCode').value=='') ? 0 : document.getElementById('ddlProductCode').value;
    get_viptype();
    getType();
    IRSWorld.BranchSetup.BranchDefaults.getDefaultSetup(branchcode,bind_grid);
    IRSWorld.BranchSetup.BranchDefaults.getVIPTable(branchcode,load_type);
}

function load_dropdown()
{
    new getDropDown().Load_BenType();
    new getDropDown().Load_NotifyMode();
    new getDropDown().Load_RelationToRemitter();
    new getDropDown().Load_Country();
    //new getDropDown().Load_ProductCode();
    //new getDropDown().Load_Branches();
    new getDropDown().Load_PayMode();
    new getDropDown().Load_LocalCurrency();
    new getDropDown().Load_ReceiptMode();
    new getDropDown().Load_AccountType();
    new getDropDown().Load_AccountCurrency();
    new getDropDown().Load_CoveringBank();
    new getDropDown().Load_RemitCurrency();
    new getDropDown().Load_RapidRemit();
    new getDropDown().Load_TransactionSource();
    new getDropDown().Load_TransactionPurpose();
    new getDropDown().Load_Computation();
    new getDropDown().Load_SourceOfFunds();
    new getDropDown().Load_RoundUpOBPS();
    new getDropDown().Load_ExcessSSS();
    new getDropDown().Load_RemittanceAggregation();
    new getDropDown().Load_WorkerType();
    new getDropDown().Load_RemitterType();
    new getDropDown().Load_ChargeType();
    new getDropDown().Load_VIPStatus();
    new getDropDown().Load_Gender();
    new getDropDown().Load_CivilStatus();
    new getDropDown().Load_Occupation();
    new getDropDown().Load_Citizenship();
    new getDropDown().Load_Mail();
    new getDropDown().Load_Contact();
    new getDropDown().Load_ChargeGroup();
    new getDropDown().Load_ProductCode();
    new getDropDown().Load_ImplemType();
    new getDropDown().Load_RiskRating();
    new getDropDown().Load_GFCChargeType();

}

function set_selected_form(formid)
{
    if(document.getElementById('ddlBranches').value != 'null' )
    {
        iclick = true;
        document.getElementById('rem_tab').className = 'tab_unselected';
        document.getElementById('bene_tab').className = 'tab_selected';
        document.getElementById('search').className = 'tab_unselected';
    }
    
}

function form_selected(tbl,i)
{
    selected_form = (tbl=='' ? selected_form : tbl);
    //ididisplay lang kung ano yung nakahighlight na tab
    if (document.getElementById('search').className=='tab_selected')
    {
        selected_form = 'Search';
        select(selected_form,i);
        enabled_controls('disabled');
        enabled_controls2('disabled');
    }
    if (document.getElementById('rem_tab').className=='tab_selected')
    {
        selected_form = 'Remitter';
        select(selected_form,i);
        enabled_controls('disabled');
        enabled_controls2('disabled');
    }
    else if(document.getElementById('bene_tab').className=='tab_selected')
    {
        selected_form = 'Beneficiary';
        select(selected_form,i);
        enabled_controls('disabled');
        enabled_controls2('disabled');
    }
    //set_visibility(selected_form,'true');
}

function select(tab,bool)
{
    if(document.getElementById('ddlBranches').value != 'null')
    {
        if(iclick)
        {
            if(tab=='Remitter')
            {
                document.getElementById('rem_tab').className = 'tab_selected';
                document.getElementById('bene_tab').className = 'tab_unselected';
                document.getElementById('search').className = 'tab_unselected';
                set_visibility('Remitter','true');
            }
            
            else if(tab=='Beneficiary')
            {
                document.getElementById('bene_tab').className = 'tab_selected';
                document.getElementById('rem_tab').className = 'tab_unselected';
                document.getElementById('search').className = 'tab_unselected';
                set_visibility('Beneficiary','true');
            }
            else if(tab=='search')
            {
                document.getElementById('bene_tab').className = 'tab_unselected';
                document.getElementById('rem_tab').className = 'tab_unselected';
                document.getElementById('search').className = 'tab_selected';
                set_visibility('Search','true');
            }
        }
    }
    else
    {
        document.getElementById('bene_tab').className = 'tab_unselected';
        document.getElementById('rem_tab').className = 'tab_unselected';
        document.getElementById('search').className = 'tab_selected';
        set_visibility('Search','true');
    }
}

function set_visibility(tbl,display)
{

    if (tbl=='Beneficiary')
    {
        if(display=='false')
        {   
             //clear_text();
             document.getElementById('tblBranch').style.display="";
             document.getElementById('tblBene').style.display="none";
             document.getElementById('tblRemittance').style.display="none";
             document.getElementById('tblDefaults').style.display="none";
             document.getElementById('tblButtonsMain').style.display="none";
             document.getElementById('tblSelFields').style.display="none";
             document.getElementById('tblRemitter').style.display = "none";
             document.getElementById('tblRemitter').style.display = "none";
             document.getElementById('tblRemitterButton').style.display = "none";
             document.getElementById('tblVIP').style.display = "none";
        }
        else if(display=='true')
        {
              if (document.getElementById('ddlBranches').value != 'null')
              {
                 document.getElementById('tblBranch').style.display="none";
                 document.getElementById('tblBene').style.display="";
                 document.getElementById('tblRemittance').style.display="";
                 document.getElementById('tblDefaults').style.display="";
                 document.getElementById('tblButtonsMain').style.display="";
                 document.getElementById('tblSelFields').style.display="";
                 document.getElementById('tblRemitter').style.display = "none";
                 document.getElementById('tblRemitterButton').style.display = "none";
                 document.getElementById('tblVIP').style.display = "";
                 //getBranchDefaults();
                 //populate_rec();
              }
              else
              {
                 alert('Please complete the search');
              }  
        }
    }
    
    if (tbl=='Remitter')
    {
        if(display=='false')
        {   
             //clear_text();
             document.getElementById('tblBranch').style.display="";
             document.getElementById('tblBene').style.display="none";
             document.getElementById('tblRemittance').style.display="none";
             document.getElementById('tblDefaults').style.display="none";
             document.getElementById('tblButtonsMain').style.display="none";
             document.getElementById('tblSelFields').style.display="none";
             document.getElementById('tblRemitter').style.display = "none";
             document.getElementById('tblRemitter').style.display = "none";
             document.getElementById('tblRemitterButton').style.display = "none";
             document.getElementById('tblVIP').style.display = "none";
        }
        else if(display=='true')
        {
              if (document.getElementById('ddlBranches').value != 'null')
              {
                 document.getElementById('tblBranch').style.display="none";
                 document.getElementById('tblBene').style.display="none";
                 document.getElementById('tblRemittance').style.display="none";
                 document.getElementById('tblDefaults').style.display="none";
                 document.getElementById('tblButtonsMain').style.display="";
                 document.getElementById('tblSelFields').style.display="none";
                 document.getElementById('tblRemitter').style.display = "";
                 document.getElementById('tblRemitterButton').style.display = "none";
                 document.getElementById('tblVIP').style.display = "none";
                 //getBranchDefaults();
                 //populate_rec();
              }
              else
              {
                 alert('Please complete the search');
              }  
        }
    }
    if (tbl=='Search')
    {
         document.getElementById('tblBranch').style.display="";
         document.getElementById('tblBene').style.display="none";
         document.getElementById('tblRemittance').style.display="none";
         document.getElementById('tblDefaults').style.display="none";
         document.getElementById('tblButtonsMain').style.display="none";
         document.getElementById('tblSelFields').style.display="none";
         document.getElementById('tblRemitter').style.display = "none";
         document.getElementById('tblRemitterButton').style.display = "none";    
         document.getElementById('tblVIP').style.display = "none";
    }
    
    
    
}

function enabled_button(btn_type)
{
    if (btn_type=='Save')
    {
        document.getElementById('btnSave').disabled = 'disabled';
        document.getElementById('btnEdit').disabled = '';
        document.getElementById('btnCancel').disabled = 'disabled';
        //document.getElementById('btnDelete').disabled = 'disabled';
        //document.getElementById('MainMenu').disabled = '';
        //document.getElementById('Master').disabled = '';
        clear_text();
        set_visibility('Beneficiary','false');
        enabled_controls('disabled');
        enabled_controls2('disabled');
        iclick = false;
    }
    else if (btn_type=='Cancel')
    {
        document.getElementById('btnSave').disabled = 'disabled';
        document.getElementById('btnEdit').disabled = '';
        document.getElementById('btnCancel').disabled = 'disabled';
        //document.getElementById('btnDelete').disabled = '';
        //document.getElementById('MainMenu').disabled = '';
        //document.getElementById('Master').disabled = '';
        enabled_controls('disabled');
        enabled_controls2('disabled');
    }
    else if (btn_type=='Edit')
    {
        document.getElementById('btnSave').disabled = '';
        document.getElementById('btnEdit').disabled = 'disabled';
        document.getElementById('btnCancel').disabled = '';
        //document.getElementById('btnDelete').disabled = '';
        //document.getElementById('MainMenu').disabled = 'disabled';
        //document.getElementById('Master').disabled = 'disabled';
        enabled_controls('');
        enabled_controls2('');
    }
}


function enabled_button2(btn_type)
{
    if (btn_type=='Save')
    {
        document.getElementById('btnSave2').disabled = 'disabled';
        document.getElementById('btnEdit2').disabled = '';
        document.getElementById('btnCancel2').disabled = 'disabled';
        document.getElementById('btnClear2').disabled = 'disabled';
        clear_text();
        enabled_controls2('disabled');
    }
    else if (btn_type=='Cancel')
    {
        document.getElementById('btnSave2').disabled = 'disabled';
        document.getElementById('btnEdit2').disabled = '';
        document.getElementById('btnCancel2').disabled = 'disabled';
        document.getElementById('btnClear2').disabled = '';
        enabled_controls2('disabled');
    }
    else if (btn_type=='Edit')
    {
        document.getElementById('btnSave2').disabled = '';
        document.getElementById('btnEdit2').disabled = 'disabled';
        document.getElementById('btnCancel2').disabled = '';
        document.getElementById('btnClear2').disabled = '';
        enabled_controls2('');
    }
}

function enabled_controls(x)
{
    document.getElementById('ddlBenType').disabled = x;
    document.getElementById('ddlRelation').disabled = x;
    document.getElementById('ddlNotify').disabled = x;
    document.getElementById('ddlCountry').disabled = x;
    document.getElementById('chkSameAsRemitter').disabled = x;
    document.getElementById('chkOnBehalf').disabled = x;
    document.getElementById('chkSMSNotification').disabled = x;
    document.getElementById('ddlLocalCurrency').disabled = x;
    document.getElementById('ddlPayMode').disabled = x;
    document.getElementById('ddlRapid').disabled = x;
    document.getElementById('ddlReceiptMode').disabled = x;
    document.getElementById('chkRapidRemit').disabled = x;
    document.getElementById('ddlAccountType').disabled = x;
    document.getElementById('txtSSSNo').disabled = x;
    document.getElementById('ddlAccountCurrency').disabled = x;
    document.getElementById('ddltrnxSource').disabled = x;
    document.getElementById('ddlRemitCurr').disabled = x;
    document.getElementById('ddltrnxPurpose').disabled = x;
    document.getElementById('ddlCoveringBank').disabled = x;
    document.getElementById('ddlComputation').disabled = x;
    document.getElementById('ddlSourceOfFunds').disabled = x;
    document.getElementById('tdDefaults').disabled = x;
    document.getElementById('ddlRoundUp').disabled = x;
    document.getElementById('ddlExcess').disabled = x;
    document.getElementById('txtSSSBackend').disabled = x;
    document.getElementById('txtDateValue1').disabled = x;
    document.getElementById('txtDateValue2').disabled = x;
    document.getElementById('txtDateValue3').disabled = x;
    document.getElementById('txtCashAggValue1').disabled = x;
    document.getElementById('txtAgentAggValue1').disabled = x;
    document.getElementById('ddlDateValue1').disabled = x;
    document.getElementById('ddlDateValue2').disabled = x;
    document.getElementById('ddlDateValue3').disabled = x;  
    document.getElementById('ddlCashAggValue1').disabled = x;
    document.getElementById('ddlAgentAggValue1').disabled = x;
    document.getElementById('ddlChargeGroup').disabled = x;  
    document.getElementById('ddlProductCode').disabled = x; 
    document.getElementById('ddlImplemType').disabled = x;
    document.getElementById('ddlGFCCharge').disabled = x;
    document.getElementById('txtThreshold').disabled = x;
    document.getElementById('chkBehalf').disabled = x;
    document.getElementById('chkHalf').disabled = x;
    document.getElementById('chkStream').disabled = x;
    document.getElementById('txtAgeLimit').disabled = x;
    document.getElementById('chkInputtedId').disabled = x;
    document.getElementById('ddlChargeTypePHP').disabled = x;
    document.getElementById('ddlChargeTypeUSD').disabled = x;
    document.getElementById('ddlChargeTypeEUR').disabled = x;
    document.getElementById('chkCurrRateNoEdit').disabled = x;
    document.getElementById('chkRemDetDesc').disabled = x;
}

function enabled_controls2(y)
{
    var x =y;
    document.getElementById('ddlWorkType').disabled = x;
    document.getElementById('ddlRemitterType').disabled = x;
    document.getElementById('ddlRemitChargeType').disabled = x;
    document.getElementById('ddlVipStat').disabled = x;
    document.getElementById('ddlGender').disabled = x;
    document.getElementById('ddlCivilStat').disabled = x;
    document.getElementById('ddlOccupation').disabled = x;
    document.getElementById('ddlCitizenship').disabled = x;
    document.getElementById('ddlMail').disabled = x;
    document.getElementById('ddlContact').disabled = x;
    document.getElementById('txtDistrict').disabled = x;
    document.getElementById('txtCity').disabled = x;
    document.getElementById('txtState').disabled = x;
    document.getElementById('txtRegion').disabled = x;
    document.getElementById('ddlCountry2').disabled = x;
    document.getElementById('txtPOBoxDistrict').disabled = x;
    document.getElementById('txtHomeZip').disabled = x;
    document.getElementById('txtOfficeZip').disabled = x;
    document.getElementById('chkCardTranx').disabled = x;
    document.getElementById('ddlChargeGroup').disabled = x;
    document.getElementById('ddlImplemType').disabled = x;
    document.getElementById('ddlRiskRating').disabled = x;
    document.getElementById('ddlGFCCharge').disabled = x;
    document.getElementById('txtThreshold').disabled = x;
    document.getElementById('chkBehalf').disabled = x;
    document.getElementById('chkHalf').disabled = x;
    document.getElementById('chkStream').disabled = x;
    document.getElementById('txtAgeLimit').disabled = x;
    document.getElementById('chkInputtedId').disabled = x;
    document.getElementById('ddlChargeTypePHP').disabled = x;
    document.getElementById('ddlChargeTypeUSD').disabled = x;
    document.getElementById('ddlChargeTypeEUR').disabled = x;
}

function save_branchdefaults()
{
    if (guserrolecode == '2') {
        alert('Approver is not allowed to modify current set-up.')
        return;
    }
        DisplayFormValues();
}

function DisplayFormValues()
{
    var str = '';
    var fldGroup='';
    var fldValue='';
    var data='';
    var remitagrregation='';
    var x=0;
    var remitaggr_temp='';
    var look_up='';
    
    function cashaggr_obj (){ this.dval=''; this.fval='';};
    var cashaggr_arr = new Array();
    for (i = 0; i < 3; i++)
    {
        var co = new cashaggr_obj;
        cashaggr_arr[i] = co;
    }
    
    function agentaggr_obj (){ this.dval=''; this.fval='';};
    var agentaggr_arr = new Array();
    for (i = 0; i < 3; i++)
    {
        var ao = new agentaggr_obj;
        agentaggr_arr[i] = ao;
    }
    
    for (i=0; i<document.forms[0].length; i++)
    {
        doc = document.forms[0].elements[i];
        if(doc.type=='text' | doc.type =='select-one' | doc.type=='checkbox')
        {
            fldGroup = doc.name;
            fldValue = (doc.type=='checkbox') ?  doc.checked : doc.value;
            
            //for SSS Backend
            if(fldGroup == 'SSSBackendCharge')
            {
                fldGroup = fldGroup;
                fldValue = fldValue;
            }
            
            //textbox SSS AccountNo
            else if (doc.type=='text' & (fldGroup != 'RemittanceAggregation' && fldGroup != 'CashAggregation' && fldGroup != 'AgentAggregation'))
            {
                fldGroup = fldGroup + '_' + fldValue;
                fldValue = 0;
            }
            
            if (doc.id.substring(0,12) !='gridDefaults' && doc.id.substring(0,8) != 'viptable')
            {
            //for remittance aggregation
                if (fldGroup=='RemittanceAggregation')
                {
                    if (doc.type == 'text')
                    {
                        remitagrregation=''
                        remitagrregation += 'datevalue' + ':' + fldValue + ';';
                    }
                    else if (doc.type=='select-one')
                    {
                        x++;
                        
                        remitagrregation += 'fldvalue' + ':' + fldValue;
                        fldGroup_remitaggr = 'RemittanceAggregation' + x 
                        remitaggr_temp += fldGroup_remitaggr + '=' + remitagrregation + ',';
                    }
                    if (x == 3)
                    {
                        x = 0;
                        data +=   remitaggr_temp;
                    }
                }
                else if (fldGroup == 'CashAggregation')
                {
                    var cx = parseInt(doc.id.charAt(doc.id.length - 1));
                    if (!isNaN(cx) && cx < 4)
                    {
                        if (doc.type == 'text')
                        {
                            cashaggr_arr[cx - 1].dval = fldValue;
                        }
                        else if (doc.type == 'select-one')
                        {
                            cashaggr_arr[cx - 1].fval = fldValue;
                        }
                    }
                }
                else if (fldGroup == 'AgentAggregation')
                {
                    var ax = parseInt(doc.id.charAt(doc.id.length - 1));
                    if (!isNaN(ax) && ax < 4)
                    {
                        if (doc.type == 'text')
                        {
                            agentaggr_arr[ax - 1].dval = fldValue;
                        }
                        else if (doc.type == 'select-one')
                        {
                            agentaggr_arr[ax - 1].fval = fldValue;
                        }
                    }
                }
                
                if (fldGroup != null && (fldGroup != 'RemittanceAggregation' && fldGroup != 'CashAggregation' && fldGroup != 'AgentAggregation'))
                {
                    look_up =  fldGroup + '=' +  fldValue;
                    data += fldGroup + '=' +  fldValue + ','; 
                }
            }
        }
    }
    for (x = 0; x < cashaggr_arr.length; x++)
    {
        if (cashaggr_arr[x].fval == '' & cashaggr_arr[x].dval == '') continue;
        data += 'CashAggregation' + (x + 1) + '_' + cashaggr_arr[x].dval + '=' + cashaggr_arr[x].fval + ',';
    }
    
    for (x = 0; x < agentaggr_arr.length; x++)
    {
        if (agentaggr_arr[x].fval == '' & agentaggr_arr[x].dval == '') continue;
        data += 'AgentAggregation' + (x + 1) + '_' + agentaggr_arr[x].dval + '=' + agentaggr_arr[x].fval + ',';
    }
    var temp_brcode = document.getElementById('ddlBranches').value;
    var temp_prodcode = document.getElementById('ddlProductCode').value;
    getGridValue(data);
    //BranchDefaults.saveDefaults(data,temp_brcode,temp_prodcode);
    //getGridValue();
}

function clear_text()
{
    document.getElementById('chkSameAsRemitter').checked = '';
    document.getElementById('chkOnBehalf').checked = '';
    document.getElementById('chkRapidRemit').checked = '';
    document.getElementById('chkCardTranx').checked = '';
    document.getElementById('chkSMSNotification').checked = '';
    document.getElementById('txtSSSNo').value = '';
    document.getElementById('tdDefaults').disabled = 'disabled';
    document.getElementById('txtDateValue1').value = '';
    document.getElementById('txtDateValue2').value = '';
    document.getElementById('txtDateValue3').value = '';
    document.getElementById('txtCashAggValue1').value = '';
    document.getElementById('txtAgentAggValue1').value = '';
    document.getElementById('txtDistrict').value = '';
    document.getElementById('txtCity').value = '';
    document.getElementById('txtState').value = '';
    document.getElementById('txtRegion').value = '';
    document.getElementById('txtPOBoxDistrict').value = '';
    document.getElementById('txtHomeZip').value = '';
    document.getElementById('txtOfficeZip').value = '';
    document.getElementById('txtThreshold').value = '';
    document.getElementById('txtAgeLimit').value = '';
    document.getElementById('chkInputtedId').checked = '';
    document.getElementById('ddlChargeTypePHP').value = '';
    document.getElementById('ddlChargeTypeUSD').value = '';
    document.getElementById('ddlChargeTypeEUR').value = '';
    document.getElementById('chkCurrRateNoEdit').checked = false;
    document.getElementById('chkRemDetDesc').checked = false;

    load_dropdown();
    //set_visibility('Beneficiary','false');
}


function onChangeText(txtbox)
{
    var value = document.getElementById(txtbox).value;
    if(!isInt(value))
    {
        alert('Please input numeric only');
        document.getElementById(txtbox).value = '';
    }
    
}

function isInt(myNum) 
{
     var myMod = myNum % 1;

     if (myMod == 0) {
             return true;
     } else {
             return false;
     }
}


function getBranchDefaults()
{
    var brcode=document.getElementById('ddlBranches').value;
    var prodcode=document.getElementById('ddlProductCode').value;
    //IRSWorld.BranchSetup.BranchDefaults.getBranchDefaults(brcode,get_value);
    IRSWorld.BranchSetup.BranchDefaults.isAgent(brcode,display_agentagg);
}

function display_agentagg(rs)
{
    var result = rs.value;
    if(result == true)
    {
        document.getElementById('tdAgentAgg').style.display = "";
    }
    else
    {
        document.getElementById('tdAgentAgg').style.display = "none";
    }
    var brcode=document.getElementById('ddlBranches').value;
    IRSWorld.BranchSetup.BranchDefaults.getBranchDefaults(brcode,get_value);
}

function get_value(rs)
{
    if (rs.error != null) 
    {   
        alert(rs.error.Message);
        return;
    }
    
    bind_data(rs.value);
}


function bind_data(dt)
{
    var dtNew = dt;
    var ctr = 0;
    if (dt.Rows.length > 0)
    {
        for (ctr=0;ctr<=dt.Rows.length - 1;ctr++)
        {
            if (dt.Rows[ctr]['BranchDefValue']=='BeneficiaryType')
            {
                document.getElementById('ddlBenType').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='SameAsRemitter')
            {
                document.getElementById('chkSameAsRemitter').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='Relationship')
            {
                document.getElementById('ddlRelation').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ProductCode')
            {
                document.getElementById('ddlProductCode').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='NotificationMode')
            {
                document.getElementById('ddlNotify').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='OnBehalf')
            {
                document.getElementById('chkOnBehalf').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='Country')
            {
                document.getElementById('ddlCountry').value = dt.Rows[ctr]['BranchDefCode'];
               // document.getElementById('ddlCountry2').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RemitterCountry')
            {
                document.getElementById('ddlCountry2').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='SourceOfFunds')
            {
                document.getElementById('ddlSourceOfFunds').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='LocalCurrency')
            {
                document.getElementById('ddlLocalCurrency').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='PayMode')
            {
                document.getElementById('ddlPayMode').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RapidRemitCode')
            {
                document.getElementById('ddlRapid').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ReceiptMode')
            {
                document.getElementById('ddlReceiptMode').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RapidRemitKey')
            {
                document.getElementById('chkRapidRemit').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='AccountType')
            {
                document.getElementById('ddlAccountType').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,9)=='SSSAcctNo')
            {
                var val = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtSSSNo').value = val[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,11)=='IDThreshold')
            {
                var val = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtThreshold').value = val[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='AccountCurrency')
            {
                document.getElementById('ddlAccountCurrency').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='TranxSource')
            {
                document.getElementById('ddltrnxSource').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RemittingCurrency')
            {
                document.getElementById('ddlRemitCurr').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='TranxPurpose')
            {
                document.getElementById('ddltrnxPurpose').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='CoveringBank')
            {
                document.getElementById('ddlCoveringBank').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='Computation')
            {
                document.getElementById('ddlComputation').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,22)=='RemittanceAggregation1')
            {
                var remitvalue1 = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtDateValue1').value = remitvalue1[1];
                document.getElementById('ddlDateValue1').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,22)=='RemittanceAggregation2')
            {
                var remitvalue2 = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtDateValue2').value = remitvalue2[1];
                document.getElementById('ddlDateValue2').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,22)=='RemittanceAggregation3')
            {
                var remitvalue3 = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtDateValue3').value = remitvalue3[1];
                document.getElementById('ddlDateValue3').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if (dt.Rows[ctr]['BranchDefValue'].substring(0,16) == 'CashAggregation1')
            {
                try
                {
                    document.getElementById('txtCashAggValue1').value = dt.Rows[ctr]['BranchDefValue'].split('_')[1];
                    document.getElementById('ddlCashAggValue1').value = dt.Rows[ctr]['BranchDefCode'];
                }
                catch (e) {}
            }
            else if (dt.Rows[ctr]['BranchDefValue'].substring(0,17) == 'AgentAggregation1')
            {
                try
                {
                    document.getElementById('txtAgentAggValue1').value = dt.Rows[ctr]['BranchDefValue'].split('_')[1];
                    document.getElementById('ddlAgentAggValue1').value = dt.Rows[ctr]['BranchDefCode'];
                }
                catch(e){}
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RoundupOBPSAmount')
            {
                document.getElementById('ddlRoundUp').value = (dt.Rows[ctr]['BranchDefCode']==0) ? 2 : dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='SSSBackendCharge')
            {
                document.getElementById('txtSSSBackend').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ExcessSSS')
            {
                document.getElementById('ddlExcess').value = (dt.Rows[ctr]['BranchDefCode']==0) ? 2 : dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='WorkerType')
            {
                document.getElementById('ddlWorkType').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,8)=='District')
            {
                var district = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtDistrict').value = district[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RemitterType')
            {
                document.getElementById('ddlRemitterType').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,4)=='City')
            {
                var city = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtCity').value = city[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ChargeType')
            {
                document.getElementById('ddlRemitChargeType').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,5)=='State')
            {
                var state = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtState').value = state[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='VIPCardStatus')
            {
                document.getElementById('ddlVipStat').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,6)=='Region')
            {
                var region = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtRegion').value = region[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,5)=='State')
            {
                var state = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtState').value = state[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='Gender')
            {
                document.getElementById('ddlGender').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='Civil Status')
            {
                document.getElementById('ddlCivilStat').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,13)=='POBoxDistrict')
            {
                var pobox = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtPOBoxDistrict').value = pobox[1];
            }
            else if (dt.Rows[ctr]['BranchDefValue'].substring(0,8)=='AgeLimit')
            {
               var agelimit = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtAgeLimit').value = agelimit[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='Occupation')
            {
                document.getElementById('ddlOccupation').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,11)=='HomeZipCode')
            {
                var homezip = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtHomeZip').value = homezip[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='Citizenship')
            {
                document.getElementById('ddlCitizenship').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue'].substring(0,13)=='OfficeZipCode')
            {
                var officezip = dt.Rows[ctr]['BranchDefValue'].split('_');
                document.getElementById('txtOfficeZip').value = officezip[1];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='PreferredMailingAddress')
            {
                document.getElementById('ddlMail').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ContactType')
            {
                document.getElementById('ddlContact').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='CardTransaction')
            {
                document.getElementById('chkCardTranx').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ChargeGroup')
            {
                document.getElementById('ddlChargeGroup').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ImplementationType')
            {
                document.getElementById('ddlImplemType').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RiskRating')
            {
                document.getElementById('ddlRiskRating').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='SMSNotification')
            {
                document.getElementById('chkSMSNotification').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='GFCChargeType')
            {
                document.getElementById('ddlGFCCharge').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='OnBehalf2')
            {
                document.getElementById('chkBehalf').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='HalfPage')
            {
                document.getElementById('chkHalf').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='StreamLining')
            {
                document.getElementById('chkStream').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='RemitterInputtedId')
            {
                document.getElementById('chkInputtedId').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ChargeTypePHP')
            {
                document.getElementById('ddlChargeTypePHP').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ChargeTypeUSD')
            {
                document.getElementById('ddlChargeTypeUSD').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='ChargeTypeEUR')
            {
                document.getElementById('ddlChargeTypeEUR').value = dt.Rows[ctr]['BranchDefCode'];
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='CurrencyRatesNoEdit')
            {
                document.getElementById('chkCurrRateNoEdit').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            else if(dt.Rows[ctr]['BranchDefValue']=='OccupationOthersMandatory')
            {
                document.getElementById('chkRemDetDesc').checked = (dt.Rows[ctr]['BranchDefCode']==0) ? false : true;
            }
            
        }
        
    }
    else
    {
        clear_text();
    }
}

function getGridValue(data)
{
    var grid_data = '';
    var status;
    var error='';
    grid_data = grid_data + data;
    var temp_branchcode = document.getElementById('ddlBranches').value;
    var value=false;
    
    for(var i=0;i<=dbgrid.datatable.Rows.length - 1;i++)
    {
        var value;
        var tempvalue = dbgrid.datatable.Rows[i]['checked'];
        if(tempvalue=='1' | tempvalue=='on' | tempvalue==1)
        {
            value = 'true';
        }
        else
        {
            value = 'false';
        }
        
        
        var name =  dbgrid.datatable.Rows[i]['fldvalue'];
        var fldGroup = 'BranchDefaultSetup';
        grid_data += fldGroup +  "_" + value  + '=' + name  + ',';
    }
    
    try
    {
        ajax_show_status(true);
        status = IRSWorld.BranchSetup.BranchDefaults.saveDefaults(grid_data,temp_branchcode);
        clear_text();
        ajax_show_status(false);
    }
    catch(e)
    {
        error = e.description;
    }
    
    if(status.value==true | status.value == 'true')
    {
        //alert('Record Successfully Saved');
        alert('Successfully submitted for approval');
    }
    else
    {
        alert('Error occured while saving' + ' ' + error);
    }
    
}

function close_session()
{
    //alert("closed");
    if (is_closing())
    {
        IRSWorld.BranchSetup.BranchDefaults.close_session();
    }
    else
    {
        IRSWorld.BranchSetup.BranchDefaults.drop_irsUsers();
    }
}




function save_grid(args)
{

    if (args.value == dbgrid.footer.save_button.value)
    {        
        var brcode = document.getElementById('ddlBranch').value;   
        alert(args.additions);             
        
    }
}

function check_char_inputted(controlname)
{
    var isinvalid = isSpecial(document.getElementById(controlname).value);
    if(isinvalid)
    {
        document.getElementById(controlname).value = '';
        return;
    }
}


function validate_check_field(obj)
{

var split_text = obj.id.split('_');
var y = split_text[3];
var fldval = dbgrid.datatable.Rows[y].fldvalue;


    if (document.getElementById(obj.id).checked == true)
    {
        if (fldval == 16)
        {
            if (dbgrid.datatable.Rows[0].checked==1) //AMLA
            {
                alert('Remove first AMLA');
                document.getElementById(obj.id).checked ='';
                return;
            }
            else if (dbgrid.datatable.Rows[7].checked==1) //FINCEN
            {
                alert('Remove first FINCEN');
                document.getElementById(obj.id).checked ='';
                return;
            }
            else if (dbgrid.datatable.Rows[11].checked==1) //OFAC
            {
                alert('Remove first OFAC');
                document.getElementById(obj.id).checked ='';
                return;
            }
            else if (dbgrid.datatable.Rows[14].checked==1) //OFAC
            {
                alert('Remove first Sanction Watchlist');
                document.getElementById(obj.id).checked ='';
                return;
            }
        }
        else if (fldval == 1 | fldval ==8 | fldval == 12 | fldval ==15)
        {
            if (dbgrid.datatable.Rows[15].checked==1)
            {
                alert('Remove first Unified Watchlist');
                document.getElementById(obj.id).checked ='';
                return;
            }
        }
    }
}

function save_vip(args)
{
    if (guserrolecode == '2') {
        alert('Approver is not allowed to modify current set-up.')
        return;
    }

    var val='';
    if (args.value == dbgrid.footer.save_button.value)
    {        
        var brcode = document.getElementById('ddlBranches').value;                
        val = IRSWorld.BranchSetup.BranchDefaults.applyEdits(args.additions, args.deletes, args.edits,brcode,updates_saved);
       
    }
}

function updates_saved(cm)
{   
    if (cm.error != null)
    {
        alert(cm.error.Message);
        populate_rec();
        return;
    }
    populate_rec();
    alert('Successfully submitted for approval');
}