var report_builder = new report_builder()
       
function report_builder()
{   
    this.CheckProfileName = function()
    {
        var ProfileName = report_builder.PName;
        return IRSWorld.Report_Profiler.tranx_profile_create.CheckProfileName(trim(ProfileName)).value;
    }

    this.Click_CheckProfileName = function()
    {
        var txtProfName = document.getElementById("txtProfileName").value
        if(trim(txtProfName).length <= 0)
        {
            alert("Please input profile name.");
        }
        else
        {
            if (report_builder.CheckProfileName())
            {
                alert("Profile name exists. Please choose another name.");
            }
            else
            {
                alert("Profile is name available.");
            }
        }  
    }

    this.moveitems = function(lstbxFrom,lstbxTo)
    {
        var varFromBox = document.all(lstbxFrom);
        var varToBox = document.all(lstbxTo); 
        if ((varFromBox != null) && (varToBox != null)) 
        { 
            if(varFromBox.length < 1) 
            {
                alert("There are no items in the source listBox.");
            }
            if(varFromBox.options.selectedIndex == -1)
            {
                alert("Please select an item to move.");
            }
            while ( varFromBox.options.selectedIndex >= 0 )
            { 
                var newOption = new Option();
                newOption.text = varFromBox.options[varFromBox.options.selectedIndex].text; 
                newOption.value = varFromBox.options[varFromBox.options.selectedIndex].value; 
                varToBox.options[varToBox.length] = newOption;
                varFromBox.remove(varFromBox.options.selectedIndex);
            } 
        }
    }
        
    this.moveitemsAll = function(lstbxFrom,lstbxTo)
    {
        var varFromBox = document.all(lstbxFrom);
        var varToBox = document.all(lstbxTo);
        
        if ((varFromBox != null) && (varToBox != null)) 
        {
            if(varFromBox.length < 1) 
            {
                alert("There are no items in the source listbox.");
            }
            
            report_builder.selectAll(lstbxFrom,true,false);
                          
            while ( varFromBox.options.selectedIndex >= 0 ) 
            {
                var newOption = new Option(); 
                newOption.text = varFromBox.options[varFromBox.options.selectedIndex].text; 
                newOption.value = varFromBox.options[varFromBox.options.selectedIndex].value; 
                varToBox.options[varToBox.length] = newOption; 
                varFromBox.remove(varFromBox.options.selectedIndex);
            } 
        }    
    }
        
    this.getAllList = function(element)
    {
        var selected = new Array(); 
        var mySelect = document.getElementById(element);
        
        for (j = 0; j < mySelect.options.length; j++) 
        {
            selected.push(mySelect.options[j].value); 
        }
        return selected;
    }
       
    this.getAllTextList = function(element)
    {
        var selected = new Array(); 
        var mySelect = document.getElementById(element);
        
        for(j = 0; j < mySelect.options.length; j++) 
        {
            selected.push(mySelect.options[j].text); 
        }
        return selected;
    }
           
    this.getmultipleselected = function(element)
    {
        var selected = new Array(); 
        var mySelect = document.getElementById(element);
        
        for (j = 0; j < mySelect.options.length; j++) 
        { 
            if(mySelect.options[j].selected) 
            { 
                selected.push(mySelect.options[j].value); 
            } 
        }
        return selected;
    }
       
    this.getmultipleselectedText = function(element)
    {
        var selected = new Array(); 
        var mySelect = document.getElementById(element);
        
        for (j = 0; j < mySelect.options.length; j++) 
        { 
            if(mySelect.options[j].selected) 
            { 
                selected.push(mySelect.options[j].text); 
            } 
        } 
        return selected;
    }  
       
    this.getselectedValue = function(element)
    {
        var select = document.getElementById(element);
        return select.options[select.selectedIndex].value;
    }
       
    this.getselectedText = function(element)
    {
        var select = document.getElementById(element);
        if (select.options.length > 0)
        {
            return select.options[select.selectedIndex].text;
        }
        else
        return "";         
    }
       
    this.checkboxcheck = function (element)
    {
        var checkbox = document.getElementById(element);
        if (checkbox.checked == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    this.selectAll = function(element,selectAll,vardisable) 
    {
        var element = document.getElementById(element);
        for (var i = 0; i < element.options.length; i++) 
        {
		    element.options[i].selected = selectAll;
        }
        element.disabled = vardisable;
    }

    this.disableElement = function(element) 
    {
        var element = document.getElementById(element);
        element.disabled = true;
	}  
	 
    this.getValue = function(element)
    {
        return document.getElementById(element).value;
    }
       
    this.getText = function(element)
    {
        return document.getElementById(element).text;
    }
       
    this.setValue = function(element,value)
    {
        document.getElementById(element).value = value;
    }

    this.clearDDL = function(element)
    {
        var select = document.getElementById(element);
        select.options.length = 0;
    }
    
    this.fillDDL = function(table, element, description, value, selected, delimeter)
    {
        var select = document.getElementById(element);
        if (select.length == 0)
        {
            var iRow = table;
            var arrSplit = description.split("|");
            var arrValue = "";
            var arrIDSplit = value.split("|");
            var arrIDValue = "";
            
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]] + ((delimeter == "") ? " • " : delimeter);
                        }
                    }
                }
                
                for (var arrID = 0; arrID <= (arrIDSplit.length - 1); arrID++)
                {
                    if ((arrIDSplit.length - 1) == arrID)
                    {
                        arrIDValue = arrIDValue + iRow.Rows[i][arrIDSplit[arrID]];
                    }
                    else
                    {
                        arrIDValue = arrIDValue + iRow.Rows[i][arrIDSplit[arrID]] + "|";
                    }
                }
                select.options[i] = new Option(arrValue, arrIDValue);
                arrValue = "";
                arrIDValue = "";
            }
        }
    }
        
    this.fillDDLwithSelectALL = function(table, element, description, value, selected, delimeter)
    {
        var select = document.getElementById(element);
        if (select.length == 0)
        {
            var iRow = table;
            var arrSplit = description.split("|");
            var arrValue = "";
            var arrIDSplit = value.split("|");
            var arrIDValue = "";
            
            select.options[0] = new Option("", "");
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]] + ((delimeter == "") ? " • " : delimeter);
                        }
                    }
                }
                
                for (var arrID = 0; arrID <= (arrIDSplit.length - 1); arrID++)
                {
                    if ((arrIDSplit.length - 1) == arrID)
                    {
                        arrIDValue = arrIDValue + iRow.Rows[i][arrIDSplit[arrID]];
                    }
                    else
                    {
                        arrIDValue = arrIDValue + iRow.Rows[i][arrIDSplit[arrID]] + "|";
                    }
                }
                select.options[i + 1] = new Option(arrValue, arrIDValue);
                arrValue = "";
                arrIDValue = "";
            }
        }
    }

    this.initializepage = function()
    {     
        var clientData = null;
        var PName = null;
        report_builder.LoadBranchFilterPerRegion();
        report_builder.LoadDDLRemitMethod();
        report_builder.LoadDDLBatchTypes();
        report_builder.LoadDDLOverseasBranches();
        report_builder.LoadDDLDomesticBranches();
        report_builder.LoadDateColumns();
        report_builder.LoadAmtFilter();
        report_builder.LoadAmtComputation();
        report_builder.LoadDDLRemittingCCY();
        report_builder.LoadDDLTargetCCY();
        report_builder.LoadDDLProductCode();
        report_builder.LoadDDLTranxSource();
        report_builder.LoadDDLPayMode();
        report_builder.LoadDDLTranxStatus();
        report_builder.LoadDDLReportField();
        report_builder.LoadTxtDateToday();
        report_builder.LoadDDLProfiles();
        report_builder.LoadDDLCriteria(); 
        report_builder.LoadMerchants();
        report_builder.getOFWReports();
        report_builder.getClientData();
        report_builder.LoadStates();
        
        report_builder.setValue("txtRemitterID","");
        report_builder.setValue("txtBeneficiaryID","");
        report_builder.setValue("txtAccountNo","");
        report_builder.setValue("txtAmount","0.00");
        report_builder.setValue("txtProfileName","");
        report_builder.setValue("txtMerchMemId","");
        report_builder.setValue("txtSaveRptAs","");
        
        var btnExtract= document.getElementById("btnExtract");
        var btnPreview  = document.getElementById("btnPreview");
        var btnSave = document.getElementById("btnSave");
        var btnSaveAs = document.getElementById("btnSaveAs");
        
        btnExtract.disabled = true;
        btnPreview.disabled = true;
        btnSave.disabled = true;
        btnSaveAs.disabled = true;

        var chkAllRemitMethod = document.getElementById("chkAllRemitMethod");
        chkAllRemitMethod.disabled = false;
        chkAllRemitMethod.checked = false;
        
        var chkAllOverseas = document.getElementById("chkAllOverseas");
        chkAllOverseas.disabled = false;
        chkAllOverseas.checked = false;
        
        var chkAllDomestic = document.getElementById("chkAllDomestic");
        chkAllDomestic.disabled = false;
        chkAllDomestic.checked = false;                           
        
        var chkAllBatchTypes = document.getElementById("chkAllBatchTypes");
        chkAllBatchTypes.disabled = false;
        chkAllBatchTypes.checked = false;
        
        var RemitMethod = document.getElementById("ddRemitMethod");
        RemitMethod.disabled = false;
        
        var OverseasBranches = document.getElementById("ddOverseasBranches");
        OverseasBranches.disabled = false;
        
        var DomesticBranches = document.getElementById("ddDomesticBranches");
        DomesticBranches.disabled = false;
        
        var OFWReports = document.getElementById("ddOFWReports");
        OFWReports.disabled = true;
        
        var chkOFWReports = document.getElementById("chkOFWReports");
        chkOFWReports.disabled = false;
        chkOFWReports.checked = false
        
        var chkProdCode = document.getElementById("chkAllProduct");
        chkProdCode.disabled = false;
        chkProdCode.checked = false;

        var chkPayMode = document.getElementById("chkAllPayMode");
        chkPayMode.disabled = false;
        chkPayMode.checked = false;
        
        var chkTSource = document.getElementById("chkAllTranxSource");
        chkTSource.disabled = false;
        chkTSource.checked = false;
        
        var chkTStatus = document.getElementById("chkAllTranxStatus");
        chkTStatus.disabled = false;
        chkTStatus.checked = false;
        
        var chkMerch = document.getElementById("chkAllMerchants");
        chkMerch.disabled = false;
        chkMerch.checked = false;
        
        var chkStates = document.getElementById("chkAllStates");
        chkStates.disabled = false;
        chkStates.checked = false;
        
        document.getElementById("lstProduct").disabled = false;
        document.getElementById("lstTranxSource").disabled = false;
        document.getElementById("lstTranxStatus").disabled = false;
        document.getElementById("lstPayMode").disabled = false;
        document.getElementById("lstBatchTypes").disabled = false;
        document.getElementById("lstMerchants").disabled = false;
        document.getElementById("txtProfileName").disabled = false;
        document.getElementById("lstStates").disabled = false;

        report_builder.clearDDL("lstProduct");
        report_builder.clearDDL("lstTranxSource");
        report_builder.clearDDL("lstTranxStatus");
        report_builder.clearDDL("lstPayMode");
        report_builder.clearDDL("lstBatchTypes");
        report_builder.clearDDL("lstStates");
        
        report_builder.SetButtonUpdate("SAVE");        
    } 
    
    this.LoadBranchFilterPerRegion = function()
    {
        IRSWorld.Report_Profiler.tranx_profile_create.LoadBracnhFilterPerRegion(report_builder.LoadBranchFilterPerRegion_callback);
    }
    
    this.LoadBranchFilterPerRegion_callback = function(result)
    {
        var response = result.value;
        var ddRegion = document.getElementById("ddRegion");
        
        if (response != null && response != "" && response != undefined)
        {
            report_builder.clearDDL("ddRegion");
            report_builder.fillDDLwithSelectALL(response, "ddRegion", "Description", "Value", "", "");
        }
        
        if(report_builder.clientData.Location == "1074")
        {
            ddRegion.disabled = false
        }
        else
        {
            ddRegion.disabled = true
            report_builder.LoadDDLOverseasBranches();
        }

    }
    
    this.getClientData = function()
    {
        var result;
              
        result = IRSWorld.Report_Profiler.tranx_profile_create.clientData().value;
        if(result != null)
        {
            this.clientData = result;    
        }        
    }    
         
    this.getOFWReports = function()
    {
         IRSWorld.Report_Profiler.tranx_profile_create.getOFWReports(report_builder.getOFWReports_callback);
    }
    
    this.getOFWReports_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDLwithSelectALL(response, "ddOFWReports", "Description", "Value", "", "");
        }
    }

    this.LoadTxtDateToday = function()
    {
        IRSWorld.Report_Profiler.tranx_profile_create.getserverDate(report_builder.LoadTxtDateToday_callback);
    }

    this.LoadTxtDateToday_callback = function(result)
    {
        var response = result.value;
        report_builder.setValue("txtDateFrom",response);
        report_builder.setValue("txtDateTo",response);
    }
    
    this.LoadDDLRemitMethod = function()
    {
        report_builder.clearDDL("ddRemitMethod");
        IRSWorld.Report_Profiler.tranx_profile_create.getRemitMethod(report_builder.LoadDDLRemitMethod_callback);
    }

    this.LoadDDLRemitMethod_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "ddRemitMethod", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.LoadDDLOverseasBranches = function()
    {
        report_builder.clearDDL("ddOverseasBranches");
        IRSWorld.Report_Profiler.tranx_profile_create.getIRSOverseas_Branches(report_builder.LoadDDLOverseasBranches_callback);
    }
    
    this.LoadDDLOverseasBranches_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "ddOverseasBranches", "BranchName", "BranchCode", "", "");
        }
    }

    this.LoadDDLDomesticBranches = function()
    {
        report_builder.clearDDL("ddDomesticBranches");
        IRSWorld.Report_Profiler.tranx_profile_create.getIRSDomestic_Branches(report_builder.LoadDDLDomesticBranches_callback);
    }

    this.LoadDDLDomesticBranches_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "ddDomesticBranches", "BranchName", "BranchCode", "", "");
        }
    }
    
    this.LoadStates = function()
    {
        report_builder.clearDDL("lstStates");
        IRSWorld.Report_Profiler.tranx_profile_create.GetGetStates(report_builder.LoadStates_callback);
    }

    this.LoadStates_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "lstStates", "Description", "Value", "", "");
        }
    }

    this.LoadDateColumns = function()
    {
        report_builder.clearDDL("ddDateFilter");
        IRSWorld.Report_Profiler.tranx_profile_create.getDateColumns(report_builder.LoadDateColumns_callback);     
    }

    this.LoadDateColumns_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "ddDateFilter", "DateDescription", "DateColumn", "", "");
        }   
    }
    
    this.LoadAmtFilter = function()
    {
        report_builder.clearDDL("ddAmtFilter");
        IRSWorld.Report_Profiler.tranx_profile_create.getAmtFilter(report_builder.LoadAmtFilter_callback);     
    }

    this.LoadAmtFilter_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "ddAmtFilter", "Description", "Value", "", "");
        }   
    }

    this.LoadAmtComputation = function()
    {
        report_builder.clearDDL("ddAmtComputation");
        IRSWorld.Report_Profiler.tranx_profile_create.getComputation(report_builder.LoadAmtComputation_callback);
    }

    this.LoadAmtComputation_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "ddAmtComputation", "Description", "Value", "", "");
        }
    }

    this.LoadDDLRemittingCCY = function()
    {
        report_builder.clearDDL("ddRemittingCCY");
        IRSWorld.Report_Profiler.tranx_profile_create.getRemittingCCY(report_builder.LoadDDLRemittingCCY_callback);
    }

    this.LoadDDLRemittingCCY_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDLwithSelectALL(response, "ddRemittingCCY", "FldCode", "FldValue", "", "");
        }
    }

    this.LoadMerchants = function()
    {
        report_builder.clearDDL("lstMerchants");
        IRSWorld.Report_Profiler.tranx_profile_create.getMerchantName(report_builder.LoadMerchants_callback);
    }

    this.LoadMerchants_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "lstMerchants", "MerchantCode", "CIFID", "", "");
        }
    }

    this.LoadDDLTargetCCY = function()
    {
        report_builder.clearDDL("ddTargetCCY");
        IRSWorld.Report_Profiler.tranx_profile_create.getTargetCCY(report_builder.LoadDDLTargetCCY_callback);
    }

    this.LoadDDLTargetCCY_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDLwithSelectALL(response, "ddTargetCCY", "FldCode", "FldValue", "", "");
        }
    }
    
    this.LoadDDLProductCode = function()
    {
        report_builder.clearDDL("lstProduct");
        IRSWorld.Report_Profiler.tranx_profile_create.getProductCode(report_builder.LoadDDLProductCode_callback);
    }

    this.LoadDDLProductCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "lstProduct", "FldDescription", "FldValue", "", "");
        }   
    }

    this.LoadDDLTranxSource = function()
    {
        report_builder.clearDDL("lstTranxSource");
        IRSWorld.Report_Profiler.tranx_profile_create.getTranxSource(report_builder.LoadDDLTranxSource_callback);
    }

    this.LoadDDLTranxSource_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "lstTranxSource", "FldDescription", "FldValue", "", "");
        }   
    }

    this.LoadDDLPayMode = function()
    {
        report_builder.clearDDL("lstPayMode");
        IRSWorld.Report_Profiler.tranx_profile_create.getPayMode(report_builder.LoadDDLPayMode_callback);     
    }
        
    this.LoadDDLPayMode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "lstPayMode", "FldDescription", "FldValue", "", "");
        }
    }

    this.LoadDDLTranxStatus = function()
    {
        report_builder.clearDDL("lstTranxStatus");
        IRSWorld.Report_Profiler.tranx_profile_create.getTranxStatus(report_builder.LoadDDLTranxStatus_callback);     
    }

    this.LoadDDLTranxStatus_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "lstTranxStatus", "FldDescription", "FldValue", "", "");
        }   
    }
       
    this.LoadDDLBatchTypes = function()
    {
        report_builder.clearDDL("lstBatchTypes");
        IRSWorld.Report_Profiler.tranx_profile_create.getBatchUploadMethods(report_builder.LoadDDLBatchTypes_callback);     
    }

    this.LoadDDLBatchTypes_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {       
            report_builder.fillDDL(response, "lstBatchTypes", "FldDescription", "Seq_ID", "", "");
        }
    }

    this.LoadDDLReportField = function()
    {
        report_builder.clearDDL("ddFieldSelected");
        report_builder.clearDDL("ddFieldChoices");
        IRSWorld.Report_Profiler.tranx_profile_create.getReportField(report_builder.LoadDDLReportField_callback);     
    }

    this.LoadDDLReportField_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDL(response, "ddFieldChoices", "FldCode", "FldDescription", "", "");
        }
    }

    this.LoadDDLProfiles = function()
    {
        report_builder.clearDDL("ddProfiles");
        IRSWorld.Report_Profiler.tranx_profile_create.getProfileList(report_builder.LoadDDLProfiles_callback);
    }

    this.LoadDDLProfiles_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDLwithSelectALL(response, "ddProfiles", "ProfileName", "ProfileName", "", "");
        }
    }

    this.LoadDDLCriteria = function()
    {
        report_builder.clearDDL("ddFieldCriteria");
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            alert("The server stopped responding. Please try again.");
        }
        return IRSWorld.Report_Profiler.tranx_profile_create.getCriteria(report_builder.LoadDDLCriteria_callback);     
    }

    this.LoadDDLCriteria_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            report_builder.fillDDLwithSelectALL(response, "ddFieldCriteria", "FldDescription", "FldCode", "", "");
        }
    }

    this.CheckselectAll = function (selectelement,checkboxelement)
    {
        var Ischecked = report_builder.checkboxcheck(checkboxelement);
        report_builder.selectAll(selectelement,Ischecked,Ischecked);
    }
    
    this.CheckOFWReport = function (checkboxelement)
    {
        var Ischecked = report_builder.checkboxcheck(checkboxelement);
        var repDd = document.getElementById("ddOFWReports");
        
        if(Ischecked)
        {
            if(this.clientData.Location == "1074")
            {
                repDd.disabled = false;    
            }
            else
            {
                alert("Sorry you are not allowed to use this report.");
                repDd.disabled = true;
                document.getElementById(checkboxelement).checked = false;
                
            }   
        }
        else
        {
            repDd.disabled = true;
        }
    }
        
    this.createReportParams = function()
    {
        var repIndex = document.getElementById("ddOFWReports").selectedIndex;
        if(repIndex = 0)
        {
            
        }
        else
        {
            var paramval = report_builder.getParams();
            if (paramval.length <= 0)  return;
            var sHTML;
            var arrstr = paramval.split("^");
            var pnlholder = document.getElementById("pnlParameters");                
            var pnlExplore = document.getElementById("divExplore");  
            try 
            {             
                if (paramval.length > 0 && (arrstr[1] != null  || arrstr[1] != "undefined" ))
                { 
                     var dispname = arrstr[3].split("$");                      
                     var disptype = arrstr[2].split("$");             
                     var dispparam = arrstr[1].split("$");                    
                     var sqlvalue = arrstr[7].split("$");             
                     var seq_id  = arrstr[0];       
                     strHTML = "<table width='100%'>";         
                     for (i = 0; i < dispname.length ;  i++ )
                     {
                        if (dispname[i].length > 0)
                        strHTML +=  "<tr><td class='cell_caption' nowrap>" +  dispname[i] + "</td> " ;
                        else
                            break;
                        try
                        {   
                            strHTML +=  report_builder.getRepDataType(disptype[i] ,  dispparam[i]);
                        }
                        catch(e)
                        {
                            strHTML +=  report_builder.getRepDataType(disptype[i] ,  dispparam[i]);
                        }
                     }   
                     strHTML += "<tr> <td class='cell_caption' nowrap>Export Type</td> <td align='left' class='cell_desc' style='height: 23px'> <select id='ddlExport' style='width: auto' <option value=''>&nbsp;</option><option value='PDF'> PDF</option> <option value='XLS'>EXCEL</option> <option value=RTF'>RTF</option><option value='DOC'>WORD</option></select> </td> </tr>";    
                     strHTML +=  "</table>";
                     pnlholder.innerHTML = strHTML;                 
                     showPanel("divExplore");
                 }
                 else
                 {
                     pnlholder.innerHTML = "";
                     showPanel("divExplore");
                 }
              }
              catch(e) 
              {
                hidePanel("divExplore");
              }
         } 
    }
  
    this.getRepDataType = function(datatype ,param_name)
    {
        var s ="<input width='182px' type='text' id='" + param_name + "' size='10'>";
        if (datatype.toLowerCase().indexOf("date") > -1 )
        {   
            s = "<td   class='cell_description' align='left'  nowrap><table width='auto' height='auto' border='0' cellspacing='0' cellpadding='0'><tr><td nowrap width='70'><input disabled  dtype='date' type='text' id='T" + param_name + "' size='50'></td> <td nowrap><img";
            s += ' align="absMiddle" onclick="' + "javascript:popUpCalendar(this,document.getElementById('T" + param_name + "'),'mm-dd-yyyy')" + '"' + " src='../../images/calendar/imgcalen.png' /></td></tr></table></td></tr>";
            s = s.replace("\\","");                  
            return s;
        }
        else if (datatype.toLowerCase().indexOf("money") > -1 || datatype.toLowerCase().indexOf("int")  > -1 || datatype.toLowerCase().indexOf("decimal") > -1  || datatype.toLowerCase().indexOf("bit") > -1 )
        {   
            s ="<td  class='cell_description'  width='70' nowrap><input  dtype='number' type='text' id='T" + param_name + "' size='50'></td></tr>";
            return s;
        }    
        else
        {
            s ="<td  class='cell_description' width='70' nowrap><input   dtype='string' type='text' id='T" + param_name + "' size='50'  " ;
            
            var sdisabled = "";
            var branchcode = "";
            if(param_name.toString().toLowerCase().indexOf("branchcode") > 0)     
            {
                branchcode =  this.clientData.Location;
            }
           
            s +=   sdisabled +  " value='" + branchcode   +"' ></td></tr>";
            return s;
        }        
    }
    
    this.getParamsValues = function()
    {   
        var frm = document.forms[0];
        var fid = "";
        var params = "";
        var objtype = "";
        for (i = 0 ; i < frm.length; i++ )
        {   
            objtype = frm[i].type.toString();
            if (frm[i].type == "text" )
            {
                try
                {
                    fid = frm[i].id;
                    if(fid.substring(0,2) == "T@")
                    {   
                        params += fid + ";" + frm[i].value + "^";
                    }
                }
                catch(e)
                {
                    alert(e);
                }
            }
            else if (objtype.indexOf("select") > -1 )
            {
                try
                {
                    fid = frm[i].id;
                    if(fid.substring(0,2) == "T@")
                    {   
                        params += fid + ";" + frm[i].value + "^";
                    }
                }
                catch(e)
                {
                    alert(e);
                }
            }
        }
        return params;
    }

    this.getParams = function()
    {
        var params;
        params = report_builder.getselectedValue("ddOFWReports");
        
        return params;
    }
    
    this.generateOFWReport = function()
    {
        var param;
        var paramValue;
        var ddOFWReports = document.getElementById("ddOFWReports");
        var ddExportType = document.getElementById("ddlExport").value;
        
        param = report_builder.getParams();
        paramValue = report_builder.getParamsValues();
        
        url =  "../Reports/Pages/Report_Viewer.aspx?mergeparam=" + param + "&paramvalues=" + paramValue + "&exporttype=" + ddExportType + "&autoprint=false" ;                       
        window.open(url,"mywindow","fullscreen=yes,scrollbars=yes,titlebar=yes"); 
        hidePanel("divExplore");
        ddOFWReports.selectedIndex = 0;
    }

    this.ParseData = function()
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById("lblLoadingMsg"), "Parsing request please wait...");
        report_builder.ParseQuery();
    }

    this.ParseQuery = function()
    {
        var IsValid = true;
        var RemitMethod = report_builder.getmultipleselected("ddRemitMethod");
        var BatchTypes = report_builder.getmultipleselected("lstBatchTypes");
        var RemBrCode = report_builder.getmultipleselected("ddOverseasBranches");
        var CredBrCode = report_builder.getmultipleselected("ddDomesticBranches");
        var ProductCode = report_builder.getmultipleselected("lstProduct");
        var TransactionSource = report_builder.getmultipleselected("lstTranxSource");
        var PayMode = report_builder.getmultipleselected("lstPayMode");
        var TransactionStatus = report_builder.getmultipleselected("lstTranxStatus");
        var DateFilter = report_builder.getselectedValue("ddDateFilter");
        var DateFrom = report_builder.getValue("txtDateFrom");
        var DateTo = report_builder.getValue("txtDateTo");
        var RemittingCCY = report_builder.getValue("ddRemittingCCY");
        var TargetCCY = report_builder.getValue("ddTargetCCY");
        var AmtComputation = report_builder.getValue("ddAmtComputation");
        var AmtFilter = report_builder.getValue("ddAmtFilter");
        var Amount = report_builder.getValue("txtAmount");
        var RemitterID = report_builder.getValue("txtRemitterID");
        var BeneficiaryID = report_builder.getValue("txtBeneficiaryID");
        var AccountNo = report_builder.getValue("txtAccountNo");
        var ReportField = report_builder.getAllList("ddFieldSelected");
        var CheckMainBr = report_builder.checkboxcheck("chkAllDomestic");
        var CheckRemBr =  report_builder.checkboxcheck("chkAllOverseas");
        var ProfileName = report_builder.getValue("txtProfileName");
        var CheckProd = report_builder.checkboxcheck("chkAllProduct");
        var CheckTranxSource = report_builder.checkboxcheck("chkAllTranxSource");
        var CheckTranxStatus = report_builder.checkboxcheck("chkAllTranxStatus");
        var CheckPayMode = report_builder.checkboxcheck("chkAllPayMode");
        var MerchCode = report_builder.getmultipleselected("lstMerchants");
        var MemberId = report_builder.getValue("txtMerchMemId");
        var CheckMerchs = report_builder.checkboxcheck("chkAllMerchants");       
        var Criteria = report_builder.getValue("ddFieldCriteria");
        var States = report_builder.getmultipleselected("lstStates");
        var CheckStates = report_builder.checkboxcheck("chkAllStates");
        

        if (trim(ProfileName) == "")
        {
            IsValid = false;
            alert("Please fill up profile name.");
        }
        else if(ReportField.length == 0)
        {
            IsValid = false;
            alert("Please select at least one from choices.");
        }
        else if(RemitMethod.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remit method.");
        }
        else if(RemBrCode.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remitting branch.");        
        }

        if (IsValid)
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function()
            {
                alert("The server stopped responding. Please try again");
            }
            IRSWorld.Report_Profiler.tranx_profile_create.ParseQuery(RemBrCode, CredBrCode, DateFilter, DateFrom, DateTo,
                    RemittingCCY, TargetCCY, AmtComputation, AmtFilter, trim(Amount),
                    RemitMethod, BatchTypes, ProductCode, TransactionSource, PayMode, TransactionStatus,
                    trim(RemitterID), trim(BeneficiaryID), trim(AccountNo),
                    ReportField, CheckMainBr, CheckRemBr, Criteria, MemberId, MerchCode, CheckMerchs,
                    CheckProd, CheckPayMode, CheckTranxSource, CheckTranxStatus, States, CheckStates, this.ParseQuery_callback); 
        }
        else
        {
            hidePanel("divPopProcess");
        }
    }

    this.ParseQuery_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            var iRow = response;
            alert("Parse OK with " +  iRow.Rows.length + " row(s)");

            var btnExtract= document.getElementById("btnExtract");
            var btnPreview  = document.getElementById("btnPreview");
            var btnSave = document.getElementById("btnSave");

            btnExtract.disabled = false;
            btnPreview.disabled = false;
            btnSave.disabled = false;
            hidePanel("divPopProcess");
        }
        else
        {
            hidePanel("divPopProcess");
            alert("Parse query returned null.");
        }
    }

    this.ExtractReport = function()
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById("lblLoadingMsg"), "Preparing to extract please wait...");
        report_builder.ExtractReporttoFile();
    }

    this.ExtractReporttoFile = function()
    {
        var IsValid = true;
        var RemitMethod = report_builder.getmultipleselected("ddRemitMethod");
        var ReportField = report_builder.getAllList("ddFieldSelected");
        var ProfileName = report_builder.getValue("txtProfileName");
        var RemBrCode = report_builder.getmultipleselected("ddOverseasBranches");

        if (trim(ProfileName) == "")
        {
            IsValid = false;
            alert("Please fill up profile name.");
        }
        else if(ReportField.length == 0)
        {
            IsValid = false;
            alert("Please select at least one from choices.");
        }
        else if(RemitMethod.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remit method.");
        }
        else if(RemBrCode.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remitting branch");        
        }

        if (IsValid)
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function()
            {
                alert("The server stopped responding. Please try again");
            }
            IRSWorld.Report_Profiler.tranx_profile_create.ExtractReporttoFile(ProfileName,report_builder.ExtractReporttoFile_callback);
        }
        else
        {
            hidePanel("divPopProcess");
        }
    }

    this.ExtractReporttoFile_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response.substring(0,17) == "Extract Complete:")
            {
                if (confirm("Extract completed, would you like to download file?"))
                {
                    var dl_pop = window.open("../Report_Profiler/ReportDownload.aspx", "dl_pop","location=1,status=1,scrollbars=1, width=1,height=1");
                    dl_pop.moveTo(0,0);
                }
            }
            else
            {
                alert(response);
            }
            
            var btnExtract= document.getElementById("btnExtract");
            var btnPreview  = document.getElementById("btnPreview");
            var btnSave = document.getElementById("btnSave");
            var btnSaveAs = document.getElementById("btnSaveAs");
            
            btnExtract.disabled = true;
            btnPreview.disabled = true;
            btnSave.disabled = true;
            btnSaveAs.disabled = true;
            hidePanel("divPopProcess");
        }
        else
        {
            hidePanel("divPopProcess");
        }
    }
   
    this.CancelSaveReportProfileAs = function()
    {
        var ProfileNameAs = document.getElementById("txtSaveRptAs");
        ProfileNameAs.value = "";
        ProfileNameAs.text = "";
        hidePanel('divSaveRptAs')    
    }
    
    this.PreSaveReportProfileAs = function()
    {
        var ProfileNameAs = document.getElementById("txtSaveRptAs");
        ProfileNameAs.value = "";
        ProfileNameAs.text = "";
        ProfileNameAs.value = report_builder.getValue("txtProfileName");
        ProfileNameAs.text = report_builder.getValue("txtProfileName");
        showPanel('divSaveRptAs');
        ProfileNameAs.focus();
    }
    
    this.SaveReportProfileAs = function(ProfileName)
    {   
        if(ProfileName != "")
        {
            if(ProfileName.length > 4)
            {
                hidePanel("divSaveRptAs");
                report_builder.PreSaveReportProfile(ProfileName);
            }
            else
            {
                alert("Please input valid report profile name.");
            }
        }
        else
        {
            alert("Please input report profile name.");
        }
    }
        
    this.PreSaveReportProfile = function(ProfileName)
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById("lblLoadingMsg"), "Preparing to save profile...");
        report_builder.PName = ProfileName;
        report_builder.BuildSQLQuery();
    }
      
    this.BuildSQLQuery = function()
    {
        var IsValid = true;
        var RemitMethod = report_builder.getmultipleselected("ddRemitMethod");
        var BatchTypes = report_builder.getmultipleselected("lstBatchTypes");
        var RemBrCode = report_builder.getmultipleselected("ddOverseasBranches");
        var CredBrCode = report_builder.getmultipleselected("ddDomesticBranches");
        var ProductCode = report_builder.getmultipleselected("lstProduct");
        var TransactionSource = report_builder.getmultipleselected("lstTranxSource");
        var PayMode = report_builder.getmultipleselected("lstPayMode");
        var TransactionStatus = report_builder.getmultipleselected("lstTranxStatus");
        var DateFilter = report_builder.getselectedValue("ddDateFilter");
        var DateFrom = report_builder.getValue("txtDateFrom");
        var DateTo = report_builder.getValue("txtDateTo");
        var RemittingCCY = report_builder.getValue("ddRemittingCCY");
        var TargetCCY = report_builder.getValue("ddTargetCCY");
        var AmtComputation = report_builder.getValue("ddAmtComputation");
        var AmtFilter = report_builder.getValue("ddAmtFilter");
        var Amount = report_builder.getValue("txtAmount");
        var RemitterID = report_builder.getValue("txtRemitterID");
        var BeneficiaryID = report_builder.getValue("txtBeneficiaryID");
        var AccountNo = report_builder.getValue("txtAccountNo");
        var ReportField = report_builder.getAllList("ddFieldSelected");
        var CheckMainBr = report_builder.checkboxcheck("chkAllDomestic");
        var CheckRemBr =  report_builder.checkboxcheck("chkAllOverseas");
        var ProfileName = report_builder.PName;
        var CheckProd = report_builder.checkboxcheck("chkAllProduct");
        var CheckTranxSource = report_builder.checkboxcheck("chkAllTranxSource");
        var CheckTranxStatus = report_builder.checkboxcheck("chkAllTranxStatus");
        var CheckPayMode = report_builder.checkboxcheck("chkAllPayMode");
        var MerchCode = report_builder.getmultipleselected("lstMerchants");
        var MemberId = report_builder.getValue("txtMerchMemId");
        var chkMerchs = report_builder.checkboxcheck("chkAllMerchants");
        var Criteria = report_builder.getValue("ddFieldCriteria");
        var States = report_builder.getmultipleselected("lstStates");
        var CheckStates = report_builder.checkboxcheck("chkAllStates");
        
        if (trim(ProfileName) == "")
        {
            IsValid = false;
            alert("Please fill up profile name.");
        }
        else if(ReportField.length == 0)
        {
            IsValid = false;
            alert("Please select at least one from choices.");
        }
        else if(RemitMethod.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remit method.");
        }
        else if(RemBrCode.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remitting branch.");        
        }

        if (IsValid)
        {
            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function()
            {
                alert("The server stopped responding. Please try again.");
            }
            IRSWorld.Report_Profiler.tranx_profile_create.BuildSQLQuery(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
                        RemittingCCY,TargetCCY,AmtComputation, AmtFilter, trim(Amount),
                        RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,TransactionStatus,
                        trim(RemitterID),trim(BeneficiaryID),trim(AccountNo),
                        ReportField,CheckMainBr,CheckRemBr,Criteria,MemberId,MerchCode,chkMerchs,
                        CheckProd,CheckPayMode,CheckTranxSource,CheckTranxStatus,States,CheckStates,report_builder.BuildSQLQuery_callback);                       
        }
        else
        {
            hidePanel("divPopProcess");
        }
    } 

    this.BuildSQLQuery_callback = function(result)
    {
        var Response = result.value;
        if (Response != null && Response != "" && Response != undefined)
        {
            report_builder.SaveReportProfile(Response);          
        }
        else
        {
            hidePanel("divPopProcess");
        }
    }

    this.SaveReportProfile = function(SQLQuerySyntax)
    {
        var IsValid = true;
        var RemitMethod = report_builder.getmultipleselectedText("ddRemitMethod");
        var BatchTypes = report_builder.getmultipleselectedText("lstBatchTypes");
        var RemBrCode = report_builder.getmultipleselectedText("ddOverseasBranches");
        var CredBrCode = report_builder.getmultipleselectedText("ddDomesticBranches");
        var ProductCode = report_builder.getmultipleselectedText("lstProduct");
        var TransactionSource = report_builder.getmultipleselectedText("lstTranxSource");
        var PayMode = report_builder.getmultipleselectedText("lstPayMode");
        var TransactionStatus = report_builder.getmultipleselectedText("lstTranxStatus");
        var DateFilter = report_builder.getselectedText("ddDateFilter");
        var RemittingCCY = report_builder.getselectedText("ddRemittingCCY");
        var TargetCCY = report_builder.getselectedText("ddTargetCCY");
        var AmtComputation = report_builder.getselectedText("ddAmtComputation");
        var AmtFilter = report_builder.getselectedText("ddAmtFilter");
        var DateFrom = report_builder.getValue("txtDateFrom");
        var DateTo = report_builder.getValue("txtDateTo");
        var Amount = report_builder.getValue("txtAmount");
        var RemitterID = report_builder.getValue("txtRemitterID");
        var BeneficiaryID = report_builder.getValue("txtBeneficiaryID");
        var AccountNo = report_builder.getValue("txtAccountNo");
        var ProfileName = report_builder.PName;
        var SQLSyntax = SQLQuerySyntax
        var ReportField = report_builder.getAllTextList("ddFieldSelected");
        var CheckMainBr = report_builder.checkboxcheck("chkAllDomestic");
        var CheckRemBr =  report_builder.checkboxcheck("chkAllOverseas");
        var chkRemitMethod = report_builder.checkboxcheck("chkAllRemitMethod");
        var chkBatchTypes =  report_builder.checkboxcheck("chkAllBatchTypes");
        var MerchCode = report_builder.getmultipleselectedText("lstMerchants");
        var MemberId = report_builder.getValue("txtMerchMemId");
        var chkProduct = report_builder.checkboxcheck("chkAllProduct");
        var chkPayMode = report_builder.checkboxcheck("chkAllPayMode");
        var chkTranStatus = report_builder.checkboxcheck("chkAllTranxStatus");
        var chkTranSource = report_builder.checkboxcheck("chkAllTranxSource");
        var chkMerchs = report_builder.checkboxcheck("chkAllMerchants");
        var Criteria = report_builder.getselectedText("ddFieldCriteria");
        var Region = report_builder.getselectedText("ddRegion");
        var States = report_builder.getmultipleselected("lstStates");
        var CheckStates = report_builder.checkboxcheck("chkAllStates");

        if (trim(ProfileName) == "")
        {
            IsValid = false;
            alert("Please fill up profile name.");
        }
        else if(ReportField.length == 0)
        {
            IsValid = false;
            alert("Please select at least one from choices.");
        }
        else if(RemitMethod.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remit method.");
        }
        else if(RemBrCode.length == 0)
        {
            IsValid = false;
            alert("Please select at least one remitting branch.");        
        }
        
        if (IsValid)
        {
            setText(document.getElementById("lblLoadingMsg"), "Saving report profile...")
            var button = document.getElementById("btnSave").value;
            var ProfileNameAs = document.getElementById("txtSaveRptAs").value;
            
            if(button == "UPDATE")
            {
                if(report_builder.CheckProfileName() && ProfileNameAs == "")
                {
                    if(confirm("Do you want to update profile?"))
                    {
                        AjaxPro.timeoutPeriod = 1000 * 1000;
                        AjaxPro.onTimeout = function()
                        {
                            alert("The server stopped responding. Please try again.");
                        }
                        IRSWorld.Report_Profiler.tranx_profile_create.UpdateProfile(RemBrCode, CredBrCode,
                                                DateFilter, DateFrom, DateTo, RemittingCCY,
                                                TargetCCY, AmtComputation, AmtFilter, 
                                                trim(Amount), RemitMethod, BatchTypes, ProductCode,
                                                TransactionSource, PayMode, TransactionStatus,
                                                trim(RemitterID), trim(BeneficiaryID), trim(AccountNo), ReportField,
                                                CheckMainBr, CheckRemBr, trim(ProfileName), SQLSyntax,
                                                chkRemitMethod, chkBatchTypes, Criteria, MemberId, MerchCode,chkMerchs,
                                                chkProduct, chkPayMode, chkTranSource, chkTranStatus, Region, States, CheckStates, report_builder.SaveReportProfile_callback);
                    }
                    else
                    {
                        hidePanel("divPopProcess");
                    }
                }
                else
                {
                    if(!report_builder.CheckProfileName() && ProfileNameAs != "")
                    {
                        if(confirm("Do you want to save this profile as: " + ProfileName + "?"))
                        {
                            if (report_builder.CheckProfileName())
                            {
                                alert("Profile name exists! Please choose another name.");
                                hidePanel("divPopProcess");
                            }
                            else
                            {   
                                AjaxPro.timeoutPeriod = 1000 * 1000;
                                AjaxPro.onTimeout = function()
                                {
                                    alert("The server stopped responding. Please try again");
                                }
                                IRSWorld.Report_Profiler.tranx_profile_create.SaveReportProfile(RemBrCode, CredBrCode,
                                                        DateFilter, DateFrom, DateTo, RemittingCCY,
                                                        TargetCCY, AmtComputation, AmtFilter, 
                                                        trim(Amount), RemitMethod, BatchTypes, ProductCode,
                                                        TransactionSource, PayMode, TransactionStatus,
                                                        trim(RemitterID), trim(BeneficiaryID), trim(AccountNo), ReportField,
                                                        CheckMainBr, CheckRemBr, trim(ProfileName), SQLSyntax,
                                                        chkRemitMethod, chkBatchTypes, Criteria, MemberId, MerchCode,chkMerchs,
                                                        chkProduct, chkPayMode, chkTranSource, chkTranStatus, Region, States, CheckStates, report_builder.SaveReportProfile_callback); 
                            }
                        }
                        else
                        {
                            hidePanel("divPopProcess");
                        }
                    }
                    else
                    {
                        alert("Please check pofile name. Empty or use of existing profile name is not allowed.")
                        hidePanel("divPopProcess");
                    }
                }
            }
            else
            {
                if (report_builder.CheckProfileName())
                {
                    alert("Profile name exists. Please choose another name.");
                    hidePanel("divPopProcess");
                }
                else
                {
                    AjaxPro.timeoutPeriod = 1000 * 1000;
                    AjaxPro.onTimeout = function()
                    {
                        alert("The server stopped responding. Please try again");
                    }
                    IRSWorld.Report_Profiler.tranx_profile_create.SaveReportProfile(RemBrCode, CredBrCode,
                                                        DateFilter, DateFrom, DateTo, RemittingCCY,
                                                        TargetCCY, AmtComputation, AmtFilter, 
                                                        trim(Amount), RemitMethod, BatchTypes, ProductCode,
                                                        TransactionSource, PayMode, TransactionStatus,
                                                        trim(RemitterID), trim(BeneficiaryID), trim(AccountNo), ReportField,
                                                        CheckMainBr, CheckRemBr, trim(ProfileName), SQLSyntax,
                                                        chkRemitMethod, chkBatchTypes, Criteria, MemberId, MerchCode,chkMerchs,
                                                        chkProduct, chkPayMode, chkTranSource, chkTranStatus, Region, States, CheckStates, report_builder.SaveReportProfile_callback); 
               }
            }
        }
        else
        {
            hidePanel("divPopProcess");
        }
    }

    this.SaveReportProfile_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined && response.indexOf("ERROR") >= 0)
        {
            alert(response);
            report_builder.initializepage();
            report_builder.SetButtonUpdate("SAVE");   
        }
        else
        {
            document.getElementById("txtProfileName").value = report_builder.PName;
            alert(response);
        }
        var btnExtract= document.getElementById("btnExtract");
        var btnPreview  = document.getElementById("btnPreview");
        var btnSave = document.getElementById("btnSave");
        var btnSaveAs = document.getElementById("btnSaveAs");
        var ProfileNameAs = document.getElementById("txtSaveRptAs");
        
        btnExtract.disabled = true;
        btnPreview.disabled = true;
        btnSave.disabled = true;
        btnSaveAs.disabled = true;
        ProfileNameAs.value = "";
        ProfileNameAs.text = "";
        report_builder.LoadDDLProfiles();
        
        hidePanel("divPopProcess");
    }
    
    this.LoadProfile = function()
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById("lblLoadingMsg"), "Loading profile please wait...");
        
        var ProfileName = report_builder.getselectedText("ddProfiles");
        if (trim(ProfileName) == "")
        {
            alert("No available profile to load. Please choose Profile.");
        }
        else
        {
            report_builder.initializepage();
            IRSWorld.Report_Profiler.tranx_profile_create.getProfileDetails(trim(ProfileName),report_builder.LoadProfile_callback);
        }
    }

    this.LoadProfile_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {    
            var DT = response;
            var txtPName = document.getElementById("txtProfileName");
            var chkAllRemitMethod = document.getElementById("chkAllRemitMethod");
            var chkAllOverseas = document.getElementById("chkAllOverseas");
            var chkAllDomestic = document.getElementById("chkAllDomestic");
            var chkAllBatchTypes = document.getElementById("chkAllBatchTypes");
            var chkAllProduct = document.getElementById("chkAllProduct");
            var chkAllPayMode = document.getElementById("chkAllPayMode");
            var chkAllTranxStatus = document.getElementById("chkAllTranxStatus");
            var chkAllTranxSource = document.getElementById("chkAllTranxSource");
            var chkAllMerchs = document.getElementById("chkAllMerchants");
            var chkAllStates = document.getElementById("chkAllStates");
            var arrStates = DT.Rows[0]["States"].toString().split("|");
            var arrProdCode = DT.Rows[0]["ProductCode"].toString().split("|");
            var arrPayMode = DT.Rows[0]["PayMode"].toString().split("|");
            var arrTranSource = DT.Rows[0]["TransactionSource"].toString().split("|");
            var arrTranStatus = DT.Rows[0]["TransactionStatus"].toString().split("|");
            var arrMerchs = DT.Rows[0]["MerchantID"].toString().split("|");
            var ddlbatchtype = document.getElementById("lstBatchTypes");
            var DateFrom = report_builder.dateToString(DT.Rows[0]["DateFrom"]);
            var DateTo = report_builder.dateToString(DT.Rows[0]["DateTo"]);
            var SQLQuery = DT.Rows[0]["SQLSyntax"];
            var arrRemitMethod = DT.Rows[0]["RemitMethod"].toString().split("|");
            var arrBatchType = DT.Rows[0]["BatchType"].toString().split("|");
            var arrRemittingBranch = DT.Rows[0]["RemittingBranch"].toString().split("|");
            var arrMaintainingBranch = DT.Rows[0]["MaintainingBranch"].toString().split("|");
            var arrFields = DT.Rows[0]["Fields"].toString().split("|");
            
            report_builder.setValue("txtRemitterID",DT.Rows[0]["RemitterID"]);
            report_builder.setValue("txtBeneficiaryID",DT.Rows[0]["BeneficiaryID"]);
            report_builder.setValue("txtAccountNo",DT.Rows[0]["AccountNo"]);
            report_builder.setValue("txtAmount",DT.Rows[0]["Amount"]);
            report_builder.setValue("txtProfileName",trim(DT.Rows[0]["ProfileName"]));
            report_builder.setValue("txtDateFrom",DateFrom);
            report_builder.setValue("txtDateTo",DateTo);      
            report_builder.SetDDSelected("ddDateFilter",DT.Rows[0]["DateFilter"]);
            report_builder.SetDDSelected("ddRemittingCCY",DT.Rows[0]["RemittingCurrency"]);
            report_builder.SetDDSelected("ddTargetCCY",DT.Rows[0]["TargetCurrency"]);
            report_builder.SetDDSelected("ddAmtComputation",DT.Rows[0]["AmountComputation"]);
            report_builder.SetDDSelected("ddAmtFilter",DT.Rows[0]["AmountEquation"]);
            report_builder.SetDDSelected("ddFieldCriteria",DT.Rows[0]["FieldCriteria"]);
            report_builder.setValue("txtMerchMemId",DT.Rows[0]["MemberIDNo"]);
            report_builder.SetDDSelected("ddProfiles",DT.Rows[0]["ProfileName"]);
            
            if(DT.Rows[0]["Region"]!= "" && DT.Rows[0]["Region"].length > 0)
            {
                if(report_builder.clientData.Location != "1074")
                {
                    report_builder.clearDDL("ddRegion");
                    report_builder.LoadDDLOverseasBranches();
                }
                else
                {
                    report_builder.SetBranchRegion(DT.Rows[0]["Region"]);
                    report_builder.SetDDSelected("ddRegion",DT.Rows[0]["Region"]);
                    
                    if (DT.Rows[0]["ChkRemittingBranch"])
                    {                       
                        report_builder.selectAll("ddOverseasBranches",true,true);
                        chkAllOverseas.checked = true;
                    }
                    else
                    {
                        report_builder.selectAll("ddOverseasBranches",false,false);
                        chkAllOverseas.checked = false;
                        report_builder.CheckDDSelected("ddOverseasBranches",arrRemittingBranch);
                    }
                }
            }
            else
            {
                if (DT.Rows[0]["ChkRemittingBranch"])
                {
                    report_builder.selectAll("ddOverseasBranches",true,true);
                    chkAllOverseas.checked = true;
                }
                else
                {
                    report_builder.selectAll("ddOverseasBranches",false,false);
                    chkAllOverseas.checked = false;
                    report_builder.CheckDDSelected("ddOverseasBranches",arrRemittingBranch);
                }
            }
          
            if (DT.Rows[0]["ChkProductCode"])
            {
                report_builder.selectAll("lstProduct",true,true);
                chkAllProduct.checked = true;
            }
            else
            {
                report_builder.selectAll("lstProduct",false,false);
                chkAllProduct.checked = false;
                report_builder.CheckDDSelected("lstProduct",arrProdCode);
            }
            
            if (DT.Rows[0]["ChkPayMode"])
            {
                report_builder.selectAll("lstPayMode",true,true);
                chkAllPayMode.checked = true;
            }
            else
            {
                report_builder.selectAll("lstPayMode",false,false);
                chkAllPayMode.checked = false;
                report_builder.CheckDDSelected("lstPayMode",arrPayMode);
            }
            
            if (DT.Rows[0]["ChkTranxSource"])
            {
                report_builder.selectAll("lstTranxSource",true,true);
                chkAllTranxSource.checked = true;
            }
            else
            {
                report_builder.selectAll("lstTranxSource",false,false);
                chkAllTranxSource.checked = false;
                report_builder.CheckDDSelected("lstTranxSource",arrTranSource);
            }
            
            if (DT.Rows[0]["ChkTranxStatus"])
            {
                report_builder.selectAll("lstTranxStatus",true,true);
                chkAllTranxStatus.checked = true;
            }
            else
            {
                report_builder.selectAll("lstTranxStatus",false,false);
                chkAllTranxStatus.checked = false;
                report_builder.CheckDDSelected("lstTranxStatus",arrTranStatus);
            }
            
            if(DT.Rows[0]["ChkMerchants"]) 
            {
                report_builder.selectAll("lstMerchants",true,true);
                chkAllMerchs.checked = true;
            }
            else
            {
                report_builder.selectAll("lstMerchants",false,false);
                chkAllMerchs.checked = false;
                report_builder.CheckDDSelected("lstMerchants",arrMerchs);
            }
            
            if (DT.Rows[0]["ChkRemitMethod"])
            {
                report_builder.selectAll("ddRemitMethod",true,true);
                chkAllRemitMethod.checked = true;
            }
            else
            {
                report_builder.selectAll("ddRemitMethod",false,false);
                chkAllRemitMethod.checked = false;
                report_builder.CheckDDSelected("ddRemitMethod",arrRemitMethod);
            }

            if (DT.Rows[0]["ChkBatchType"])
            {
                report_builder.selectAll("lstBatchTypes",true,true);
                chkAllBatchTypes.checked = true;
            }
            else
            {
                report_builder.selectAll("lstBatchTypes",false,false);
                chkAllBatchTypes.checked = false;
                report_builder.CheckDDSelected("lstBatchTypes",arrBatchType);
            }

            if (DT.Rows[0]["ChkMaintainingBranch"])
            {
                report_builder.selectAll("ddDomesticBranches",true,true);
                chkAllDomestic.checked = true;
            }
            else
            {
                report_builder.selectAll("ddDomesticBranches",false,false);
                chkAllDomestic.checked = false;
                report_builder.CheckDDSelected("ddDomesticBranches",arrMaintainingBranch);
            }
            
            if (DT.Rows[0]["ChkStates"])
            {
                report_builder.selectAll("lstStates",true,true);
                chkAllStates.checked = true;
            }
            else
            {
                report_builder.selectAll("lstStates",false,false);
                chkAllStates.checked = false;
                report_builder.CheckLstSelected("lstStates",arrStates);
            }

            var btnSaveAs = document.getElementById("btnSaveAs");
            report_builder.LoadFields(arrFields);
            report_builder.PName = trim(DT.Rows[0]["ProfileName"]);
            report_builder.SetButtonUpdate("UPDATE");
            txtPName.disabled = true;
            btnSaveAs.disabled = false;
            hidePanel("divPopProcess");
        }
    }
        
    this.SetButtonUpdate = function(value)
    {
        document.getElementById("btnSave").value = value;
    }

    this.dateToString = function(dt)
    {
        var mo = "" + (dt.getMonth() + 1);
        var dd = "" + dt.getDate();
        var hh = dt.getHours();
        var mm = "" + dt.getMinutes();
        var ss = "" + dt.getSeconds();
        var tt = (hh > 12 | (hh == 12 & mm > 0)) ? "PM" : "AM";
        var shh = "" + ((hh > 12) ? hh - 12 : hh);
        return dt.getFullYear() + "-" + ((mo.length == 1) ? "0" + mo : "" + mo) + "-" +((dd.length == 1) ? "0" + dd : "" + dd);
    }

    this.SetDDSelected = function(ddlist,textval)
    {
        var mySelect = document.getElementById(ddlist); 
        var myValue = textval;
        var indexcount = 0;
        
        for(i=0;i<mySelect.length;i++)
        {
            if (trim(mySelect.options[i].text).toLowerCase() == trim(myValue).toLowerCase())
            {
                indexcount = i;
                break;
            }
        }
        mySelect.selectedIndex = indexcount;
    }
    
    this.CheckDDSelected = function(ddlist,arrayselection)
    {
        var mySelect = document.getElementById(ddlist);
        var Arrayselected = arrayselection;
                    
        for (i=0;i<mySelect.length;i++)
        {
            for (j=0;j<Arrayselected.length;j++)
            {
                if (trim(mySelect.options[i].text).toLowerCase() == trim(Arrayselected[j].toLowerCase()))
                {
                    mySelect.options[i].selected = true;
                }
            }
        }
    }
    
    this.CheckLstSelected = function(ddlist,arrayselection)
    {
        var mySelect = document.getElementById(ddlist);
        var Arrayselected = arrayselection;
                    
        for (i=0;i<mySelect.length;i++)
        {
            for (j=0;j<Arrayselected.length;j++)
            {
                if (trim(mySelect.options[i].value).toLowerCase() == trim(Arrayselected[j].toLowerCase()))
                {
                    mySelect.options[i].selected = true;
                }
            }
        }
    }
    
    this.LoadFields = function(arrayselection)
    {
        var varFromBox = document.getElementById("ddFieldChoices");
        var varToBox = document.getElementById("ddFieldSelected");
        var Arrayselected = arrayselection;

        for (j=0;j<Arrayselected.length;j++)
        {
            for (i=0;i<varFromBox.length;i++)
            {
                if (trim(varFromBox.options[i].text).toLowerCase() == trim(Arrayselected[j]).toLowerCase())
                {
                    varFromBox.options[i].selected = true;
                     while ( varFromBox.options.selectedIndex >= 0 ) 
                     { 
                        var newOption = new Option(); 
                        newOption.text = varFromBox.options[varFromBox.options.selectedIndex].text; 
                        newOption.value = varFromBox.options[varFromBox.options.selectedIndex].value; 
                        varToBox.options[varToBox.length] = newOption;
                        varFromBox.remove(varFromBox.options.selectedIndex);
                     } 
                }
            }
        }
    }

    this.PreviewData = function()
    {
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "";
        
        if (this.grid_ajax_query != null)
        {
            this.ajax_grid.open_modal_gridview(0, 5);
        }
        var btnExtract = document.getElementById("btnExtract");
        var btnPreview = document.getElementById("btnPreview");
        var btnSave = document.getElementById("btnSave");

    }
     
    this.SetBranchRegion = function(value)
    {
        var param = trim(value);
        if(param != "")
        {
            var response;
            response = IRSWorld.Report_Profiler.tranx_profile_create.GetBranchRegion(param).value;    
            if (response != null && response != "" && response != undefined)
            {
                report_builder.clearDDL("ddOverseasBranches");
                report_builder.fillDDL(response, "ddOverseasBranches", "BranchName", "BranchCode", "", "");
            }
        }
        else
        {
            report_builder.LoadDDLOverseasBranches();
        }
    }
           
    this.do_ajax = function(param)
    {
        var command_param = param.split("$");
        if (command_param != null)
        {
            if (command_param[0].toUpperCase() == "AJX_SELECTED")
            {
            }
	        else
            {
		        this.ajax_grid.process_ajax(param);		
	        }
        }
    }    
}

function execute_ajx_command(param)
{
    if (report_builder != null && report_builder != "undefined")
    {
        report_builder.do_ajax(param);                 
    }
}