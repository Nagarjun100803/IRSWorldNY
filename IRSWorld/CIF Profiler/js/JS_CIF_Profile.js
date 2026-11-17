var cif = new cif_builder()


function cif_builder()
{
    this.initializePage = function()
    {
        cif.loadCIFClass();
        cif.loadCIFbranches();
        cif.loadCIFType();
        cif.loadCIFAcctType();
        cif.loadCIFFields();
        cif.loadDateFilter();
        cif.loadCIFProfiles();
        cif.loadCIFAcctStatus();
        cif.loadCIFMaintainBranch();
        cif.loadCIFMerchants();
        cif.loadCIFAcctCcy();
        document.getElementById("txtProfileName").focus();
        document.getElementById("txtIdNumber").value = "";
        document.getElementById("txtCIFGUID").value = "";
        document.getElementById("txtAccountNo").value = "";
        document.getElementById("txtCIFName").value = "";
        document.getElementById("txtDateFrom").value = "";
        document.getElementById("txtDateTo").value = "";      
        document.getElementById("txtProfileName").value = "";
        document.getElementById("txtRemitInputID").value = "";
        document.getElementById("txtMerchMemID").value = "";
        document.getElementById("txtTopSelect").value="";
        document.getElementById("lstSelectedParam").options.length = 0;
        document.getElementById("chbxCIFBranches").checked = false;
        document.getElementById("chbxCIFType").checked = false;
        document.getElementById("chbxCIFAcctBranch").checked = false;
        document.getElementById("lstCIFType").disabled = false;
        document.getElementById("lstCIFbranches").disabled = false;
        document.getElementById("lstSelectParam").disabled = false;
        document.getElementById("lstMaintainingBranch").disabled = false;
        document.getElementById("txtRemitInputID").disabled = true;
        document.getElementById("btnExtract").disabled = true;
        document.getElementById("btnPreview").disabled = true;
        document.getElementById("btnSave").disabled = true;
        document.getElementById("txtRemitInputID").disabled = true;
        document.getElementById("txtMerchMemID").disabled = true;
        document.getElementById("ddlMerchProdCode").disabled = true;

        
        document.getElementById("lstUndefined").disabled = true;
        document.getElementById("cbxUndefined").disabled = true;
        document.getElementById("lstUndefined").style.backgroundColor = "gray";
    }
    
    this.loadCIFProfiles = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFProfiles(this.loadCIFProfiles_callback);
    }
    
    this.loadCIFProfiles_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlCIFProfiles", "Description", "Value");
        }
    }
      
    this.loadCIFClass = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFClass(this.loadCIFClass_callback);
    }
    
    
    this.loadCIFClass_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlCIFClass", "Description", "Value");
        }
    }
    
    this.loadCIFAcctStatus = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFAcctStatus(this.loadCIFAcctStatus_callback);
    }
    
    this.loadCIFAcctStatus_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlCIFAcctStatus", "Description", "Value");
        }
    }
    
    this.loadCIFMerchants = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFMerchs(this.loadCIFMerchants_callback);
    }
    
    this.loadCIFMerchants_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlMerchProdCode", "Description", "Value");
        }
    }
    
    this.loadCIFbranches = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFBranch(this.loadCIFbranches_callback);
    }
    
    
    this.loadCIFbranches_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillView(value, "lstCIFBranches", "BranchName", "BranchCode");
        }
    }
    
    this.loadCIFAcctCcy = function ()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFAcctCcy(this.loadCIFAcctCcy_callback);
    }
    
    this.loadCIFAcctCcy_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlCIFAcctCcy", "Description", "Value");
        }
    }
    
    this.loadCIFMaintainBranch = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFBranch(this.loadCIFMaintainBranch_callback);
    }
 
    this.loadCIFMaintainBranch_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillView(value, "lstMaintainingBranch", "BranchName", "BranchCode");
        }
    }
    
    this.loadCIFType = function()
    {
         IRSWorld.Report_Profiler.CIF_profile_create.getCIFType(this.loadCIFType_callback);
    }
      
    this.loadCIFType_callback = function(result)
    {
        var value = result.value;
        if(value != null && value != "" && value!= undefined)
        {
            cif.fillView(value, "lstCIFType", "Description", "Value");
        }
    } 
    
    this.loadCIFAcctType = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFAccountType(this.loadCIFAcctType_callback);
    }
    
    
    this.loadCIFAcctType_callback = function(result)
    {
        var value = result.value;
        if(value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlCIFAcctType", "Description", "Value");
        }
    }
    
    this.loadCIFFields = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFFields(this.loadCIFFields_callback);

    }
    
    this.loadCIFFields_callback = function(result)
    {
        var value = result.value;
        if(value != null && value != "" && value != undefined)
        {
            cif.fillView(value, "lstSelectParam", "Description", "Value");
        }
    }
    
    this.loadDateFilter = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFDates(this.loadDateFilter_callback);
    }
    
    this.loadDateFilter_callback = function(result)
    {
        var value = result.value;
        if(value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlDateFilter", "Description", "Value")
        }
    }
     
    this.checkBox = function(element)
    {
        var checkBx = document.getElementById(element);
        if (checkBx.checked == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    this.checkAll = function(chkBox,lstBox)
    {
        var chk = document.getElementById(chkBox);
        var lst = document.getElementById(lstBox);
        if(chk.checked == 1)
        {
            isDisabled(lstBox,true);
            lst.options.selectedIndex = -1;

        }
        else
        {
            isDisabled(lstBox,false);
        }
     }

    this.fillView = function(table, element, description, value)
    {
        var select = document.getElementById(element);
      
        var iRow = table;
        var fldDesc = description;
        var fldValue = value;
        var txtValue="";
        var IDValue="";
        
        for (var k = 0; k <= (iRow.Rows.length -1); k++)
        {   
            if(iRow.Rows[k][fldDesc] != null && iRow.Rows[k][fldDesc] != "")
            {
                txtValue = txtValue + iRow.Rows[k][fldDesc];
            }
            if(iRow.Rows[k][fldValue] != null && iRow.Rows[k][fldValue] != "")
            {
                IDValue = IDValue + iRow.Rows[k][fldValue];
            }
            select.options[k] = new Option(txtValue, IDValue);
            
            txtValue="";
            IDValue ="";    
        }
    }
    
    this.fillDDL = function(table, element, description, value)
    {
        var select = document.getElementById(element);
      
        var iRow = table;
        var fldDesc = description;
        var fldValue = value;
        var txtValue="";
        var IDValue="";
        
        select.options[0] = new Option("--Select--", "");
        for (var k = 0; k <= (iRow.Rows.length -1); k++)
        {   
            if(iRow.Rows[k][fldDesc] != null && iRow.Rows[k][fldDesc] != "")
            {
                txtValue = txtValue + iRow.Rows[k][fldDesc];
            }
            if(iRow.Rows[k][fldValue] != null && iRow.Rows[k][fldValue] != "")
            {
                IDValue = IDValue + iRow.Rows[k][fldValue];
            }
            select.options[k + 1] = new Option(txtValue, IDValue);
            
            txtValue="";
            IDValue ="";    
        }
     }
            
     this.selectAllItems = function(lstBoxFrom,lstBoxTo)
     {
        var fromBox = document.getElementById(lstBoxFrom);
        var toBox = document.getElementById(lstBoxTo);
        var txtValue = "";
        var IDValue = "";
        if((fromBox != null && fromBox != "" && fromBox != undefined) && (toBox != null && toBox != "" && toBox != undefined))
        {
            if(fromBox.length < 1)
            {
                alert("There are no Items in the Source ListBox!");
            }
            
            for(var k = 0; k < fromBox.options.length; k++)
            { 
                fromBox.options[k].selected = true;
            }
            while(fromBox.options.selectedIndex >= 0)
            {
               var addOption = new Option(); 
               addOption.text = fromBox.options[fromBox.options.selectedIndex].text; 
               addOption.value = fromBox.options[fromBox.options.selectedIndex].value; 
               toBox.options[toBox.length] = addOption; 
               fromBox.remove(fromBox.options.selectedIndex); 
            }
        }
      }
     
      this.selectedItems = function(lstBoxFrom,lstBoxTo)
      {
        var fromBox = document.getElementById(lstBoxFrom);
        var toBox = document.getElementById(lstBoxTo);
        var txtValue = "";
        var IDValue = "";
        if((fromBox != null && fromBox != "" && fromBox != undefined) && (toBox != null && toBox != "" && toBox != undefined))
        {
            if(fromBox.length < 1)
            {
               alert("There are no Items in the Source ListBox!");
            }
            else if(fromBox.options.selectedIndex < 0)
            {
                alert("There are no Items Selected!");
            }
            else
            {
                while(fromBox.options.selectedIndex >= 0)
                {
                   var addOption = new Option(); 
                   addOption.text = fromBox.options[fromBox.options.selectedIndex].text; 
                   addOption.value = fromBox.options[fromBox.options.selectedIndex].value; 
                   toBox.options[toBox.length] = addOption; 
                   fromBox.remove(fromBox.options.selectedIndex); 
                }
            }
         }
      }
      
      this.getMultiSelect = function(element)
      {
        var arr = new Array(); 
        var mySelect = document.getElementById(element);
            for(k = 0; k < mySelect.options.length; k++) 
            { 
                if(mySelect.options[k].selected) 
                { 
                    arr.push(mySelect.options[k].value); 
                } 
            } 
            return arr;
      }
      
      this.getSelectedFields = function(element)
      {
        var arr = new Array();
        var mySelect = document.getElementById(element);
            for(k = 0; k < mySelect.options.length; k++)
            {
                arr.push(mySelect.options[k].value);
            }
         return arr;
      }
      
      this.getSelectedText = function(element)
      {
        var arr = new Array();
        var mySelect = document.getElementById(element);
            for(k = 0; k < mySelect.options.length; k++)
            {
                arr.push(mySelect.options[k].text);
            }
         return arr;
      }

      this.getCheckAll = function (element)
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
      
      this.parseData = function ()
      {
            showPanel("divPopProcess");
            new common_method().to_show_loading_message("divLoadingMsg");
            setText(document.getElementById('lblLoadingMsg'),'Parsing request please wait...');
     
            var idNo = document.getElementById("txtIdNumber").value;
            var guid = document.getElementById("txtCIFGUID").value;
            var acctNo = document.getElementById("txtAccountNo").value;
            var cifClass = document.getElementById("ddlCIFClass").value;
            var cifType = cif.getMultiSelect("lstCIFType");
            var acctType = document.getElementById("ddlCIFAcctType").value;
            var acctStatus = document.getElementById("ddlCIFAcctStatus").value
            var acctBranch = cif.getMultiSelect("lstMaintainingBranch");
            var dateFilter = document.getElementById("ddlDateFilter").value;
            var dateFrom = document.getElementById("txtDateFrom").value;
            var dateTo = document.getElementById("txtDateTo").value;
            var top = document.getElementById("txtTopSelect").value;
            var cifBranch = cif.getMultiSelect("lstCIFBranches");
            var selectField = cif.getSelectedFields("lstSelectedParam");
            var chkAllCIFBranch = cif.getCheckAll("chbxCIFBranches");
            var chkAllCIFType = cif.getCheckAll("chbxCIFType");
            var chkAllAcctBranch = cif.getCheckAll("chbxCIFAcctBranch");
            var cifName = document.getElementById("txtCIFName").value;
            var profileName = document.getElementById("txtProfileName").value;
            var remInputId = document.getElementById("txtRemitInputID").value;
            var merchMemID = document.getElementById("txtMerchMemID").value;
            var merchProdCode = document.getElementById("ddlMerchProdCode").value; 
            var acctCcy = document.getElementById("ddlCIFAcctCcy").value;           
            
            if(selectField == null || selectField == undefined || selectField == "" || selectField.length < 1)
            {
                alert("No Items Selected from Listbox Below!");
                hidePanel("divPopProcess");
            }
            else if(cifClass == null || cifClass == undefined || cifClass == "")
            {
                alert("Please Select CIF Class!");
                hidePanel("divPopProcess");
            }
            else if((!chkAllCIFType) && ((cifType == null || cifType == "")))
            {
                alert("Please Select CIF Type");
                hidePanel("divPopProcess");
            }
            else if((!chkAllCIFBranch) && (cifBranch == "" || cifBranch == null))
            {
                alert("Please Select CIF Branch");
                hidePanel("divPopProcess");
            }
            else
            {
                AjaxPro.timeoutPeriod = 1000 * 1000;
                AjaxPro.onTimeout = function()
                {
                    alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                }
                IRSWorld.Report_Profiler.CIF_profile_create.parseQuery(top,idNo,guid,cifName,acctNo,acctType,acctStatus,acctCcy,acctBranch,cifClass,cifType,dateFilter,
                                                                         dateFrom,dateTo,cifBranch,selectField,chkAllCIFBranch,chkAllCIFType,chkAllAcctBranch,
                                                                         remInputId,merchMemID,merchProdCode,this.parseData_callback);
            }
      }
      
      this.parseData_callback = function(result)
      {
         var value = result.value;
         if(value != null && value != undefined)
         {
            alert("Query Ok with " + value.Rows.length + " Row/s!");
            
            document.getElementById("btnExtract").disabled = false;
            document.getElementById("btnPreview").disabled = false;
            document.getElementById("btnSave").disabled = false;
            hidePanel("divPopProcess");
         }
         else
         {
            alert("Query OK with 0 Results Found");
            hidePanel("divPopProcess");
         }
    }
    
    this.extractFile = function()
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById('lblLoadingMsg'),'Exracting profile please wait...');
         
        var profileName = document.getElementById("txtProfileName").value;
        if(profileName == "" || profileName == null || profileName == undefined)
        {
            alert("Please Input Profile Name!");
            hidePanel("divPopProcess");
        }
        else
        {
            if(cif.checkProfileName())
            {
                alert("Profile Name is Already in Use!");
                hidePanel("divPopProcess");
            }
            else
            {
                IRSWorld.Report_Profiler.CIF_profile_create.ExtractFile(profileName,this.extractFile_callback);
            }   
        }
    }
    
    this.extractFile_callback = function(result)
    {
        var value = result.value;
        if(value != null || value != "" || value != undefined)
        {
            if(value.indexOf("Extract Complete")!= -1)
            {
                var answer = confirm("File extract completed, Would you like to download file?");
                if (answer)
                {
                    var dl_pop = window.open("../Report_Profiler/ReportDownload.aspx", "dl_pop","location=1,status=1,scrollbars=1, width=1,height=1");
                    dl_pop.moveTo(0,0);
                }
            }
            else
            {
                alert(value);
            }
            document.getElementById("btnExtract").disabled = true;
            document.getElementById("btnPreview").disabled = true;
            document.getElementById("btnSave").disabled = true;
            hidePanel("divpopProcess");
        }
        else
        {
            alert("Error While Proccessing Query");
        }
        hidePanel("divpopProcess");
    }
    
    this.saveProfile = function()
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById('lblLoadingMsg'),'Saving Profile please wait...');
        
        var idNo = document.getElementById("txtIdNumber").value;
        var guid = document.getElementById("txtCIFGUID").value;
        var acctNo = document.getElementById("txtAccountNo").value;
        var cifClass = document.getElementById("ddlCIFClass").value;
        var cifType = cif.getMultiSelect("lstCIFType");
        var acctType = document.getElementById("ddlCIFAcctType").value;
        var acctStatus = document.getElementById("ddlCIFAcctStatus").value
        var acctBranch = cif.getMultiSelect("lstMaintainingBranch");
        var dateFilter = document.getElementById("ddlDateFilter").value;
        var dateFrom = document.getElementById("txtDateFrom").value;
        var dateTo = document.getElementById("txtDateTo").value;
        var cifBranch = cif.getMultiSelect("lstCIFBranches");
        var top = document.getElementById("txtTopSelect").value;
        var selectText = cif.getSelectedText("lstSelectedParam");
        var chkAllCIFBranch = cif.getCheckAll("chbxCIFBranches");
        var chkAllCIFType = cif.getCheckAll("chbxCIFType");
        var chkAllAcctBranch = cif.getCheckAll("chbxCIFAcctBranch");
        var cifName = document.getElementById("txtCIFName").value;
        var profileName = document.getElementById("txtProfileName").value;
        var remInputId = document.getElementById("txtRemitInputID").value;
        var merchMemID = document.getElementById("txtMerchMemID").value;
        var merchProdCode = document.getElementById("ddlMerchProdCode").value; 
        var acctCcy = document.getElementById("ddlCIFAcctCcy").value;
         
       if(profileName == null || profileName == undefined || profileName == "")
       {    
            alert("Please Input Profile Name!");   
            hidePanel("divPopProcess");
       }
       else if(cif.checkProfileName())
       {
            alert("Profile Name is Already in Use!");
            hidePanel("divpopProcess");
       }
       else if(selectText == null || selectText == undefined || selectText == "")
       {
            alert("No Items Selected from Listbox Below!");
            hidePanel("divPopProcess");
       }
       else if(cifClass == null || cifClass == undefined || cifClass == "")
       {
            alert("Please Select CIF Class!");
            hidePanel("divPopProcess");
       }
       else if((!chkAllCIFType) && (cifType == null || cifType == ""))
       {
            alert("Please Select CIF Type");
            hidePanel("divPopProcess");
       }
       else if((!chkAllCIFBranch) && (cifBranch == "" || cifBranch == null))
       {
            alert("Please Select CIF Branch");
            hidePanel("divPopProcess");
       }
       else
       {           
            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function()
            {
                alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
            }
            IRSWorld.Report_Profiler.CIF_profile_create.saveProfile(top,idNo,guid,cifName,acctNo,acctType,acctStatus,acctCcy,acctBranch,cifClass,cifType,dateFilter,
                                                                     dateFrom,dateTo,cifBranch,selectText,chkAllCIFBranch,chkAllCIFType,chkAllAcctBranch,
                                                                     remInputId,merchMemID,merchProdCode,profileName,this.saveProfile_callback);
                                                                     
       }  
    }
    
    this.saveProfile_callback = function(result)
    {
        var value = result.value;
        if(value !="" || value != null || value != undefined)
        {
            alert(value);
        }
        document.getElementById("btnExtract").disabled = true;
        document.getElementById("btnPreview").disabled = true;
        document.getElementById("btnSave").disabled = true;
        cif.initializePage();
        hidePanel("divpopProcess");
    }  
    
    
    this.loadExistingProfile = function(element)
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById('lblLoadingMsg'),'Loading Profile please wait...');
        
        var index = document.getElementById(element).selectedIndex;
        var value = document.getElementById(element);
        var profileName = value.options[index].text;
        var profileIndex = value.options[index].value;
        if(index == 0 || index == -1)
        {
            alert("Please Select Profile to Load!");
            hidePanel("divPopProcess");
        }
        else
        {   
             cif.initializePage();
             IRSWorld.Report_Profiler.CIF_profile_create.loadProfile(profileName,this.loadExistingProfile_callback);
        }
    }
    
    this.loadExistingProfile_callback = function(result)
    {
        var table = result.value;
        var idNo = document.getElementById("txtIdNumber");
        var guid = document.getElementById("txtCIFGUID");
        var acctNo = document.getElementById("txtAccountNo");
        var cifClass = document.getElementById("ddlCIFClass");
        var cifType = document.getElementById("lstCIFType");
        var acctType = document.getElementById("ddlCIFAcctType");
        var acctStatus = document.getElementById("ddlCIFAcctStatus");
        var acctBranch = document.getElementById("lstMaintainingBranch");       
        var dateFilter = document.getElementById("ddlDateFilter");
        var dateFrom = document.getElementById("txtDateFrom");
        var dateTo = document.getElementById("txtDateTo");
        var cifBranch = document.getElementById("lstCIFBranches");
        var top = document.getElementById("txtTopSelect");
        var fromSelect = document.getElementById("lstSelectParam");
        var selectText = document.getElementById("lstSelectedParam");
        var chkAllCIFBranch = document.getElementById("chbxCIFBranches");
        var chkAllCIFType = document.getElementById("chbxCIFType");
        var chkAllAcctBranch = document.getElementById("chbxCIFAcctBranch");
        var cifName = document.getElementById("txtCIFName");
        var profileName = document.getElementById("txtProfileName");
        var remInputId = document.getElementById("txtRemitInputID");
        var merchMemID = document.getElementById("txtMerchMemID");
        var merchProdCode = document.getElementById("ddlMerchProdCode"); 
        var acctCcy = document.getElementById("ddlCIFAcctCcy");
        var arrSelectText = table.Rows[0]["SelectField"].toString().split("|");
        var arrAcctBranch = table.Rows[0]["Account_Branch"].toString().split("|");
        var arrCifBranch = table.Rows[0]["BranchCode"].toString().split("|");
        var arrCifType = table.Rows[0]["CIFType"].toString().split("|")
        var evalDateFrom = dateToStr(table.Rows[0]["DateFrom"]);
        var evalDateTo = dateToStr(table.Rows[0]["DateTo"]);        
        var chkCIFBranch = table.Rows[0]["Cbx_Branches"]; 
        var chkCIFType = table.Rows[0]["Cbx_CIFType"]; 
        var chkAcctBranch = table.Rows[0]["Cbx_AcctBranch"]; 
        
        idNo.value = table.Rows[0]["CIFID"];
        guid.value = table.Rows[0]["CIFGUID"];
        acctNo.value = table.Rows[0]["Account_No"];
        top.value = table.Rows[0]["RecordCount"];
        cifName.value = table.Rows[0]["Fullname"];
        remInputId.value = table.Rows[0]["Rem_Inputed_Id"];
        merchMemID.value = table.Rows[0]["Merchant_Member_Id"];
        profileName.value = table.Rows[0]["ProfileName"];
        cifClass.selectedIndex = table.Rows[0]["CIFClass"];
        acctType.selectedIndex = table.Rows[0]["Account_Type"];
        acctStatus.selectedIndex = table.Rows[0]["Account_Status"];
        cif.dateFilterSelect(dateFilter,trim(table.Rows[0]["DateFilter"]));
        merchProdCode.selectedIndex = table.Rows[0]["Merchant_ProductCode"];
        acctCcy.selectedIndex = table.Rows[0]["Account_Ccy"];
        cif.fillBoxSelected(fromSelect,selectText,arrSelectText);
        cif.fillListValue(acctBranch,arrAcctBranch);
        cif.fillListValue(cifBranch,arrCifBranch);
        cif.fillSelectedIndex(cifType,arrCifType);
        
        if(evalDateFrom != "1900-01-01")
        {
            dateFrom.value = evalDateFrom;
        }
        if(evalDateTo != "1900-01-01")
        {
            dateTo.value = evalDateTo;
        }
        if(cifClass.selectedIndex == 1)
        {
            var txtBoxRem = document.getElementById("txtRemitInputID");
            txtBoxRem.disabled = false;
        }
        if(cifClass.selectedIndex == 3)
        {
            var txtBoxMerch = document.getElementById("txtMerchMemID");
            txtBoxMerch.disabled = false;
            var ddlMerch = document.getElementById("ddlMerchProdCode");
            ddlMerch.disabled = false;
        }
        if(chkCIFBranch == 1)
        {
            chkAllCIFBranch.checked = true;
            cif.checkAll(chkAllCIFBranch.id,cifBranch.id);
        }
        if(chkCIFType == 1)
        {
            chkAllCIFType.checked = true;
            cif.checkAll(chkAllCIFType.id,cifType.id);
        }
        if(chkAcctBranch == 1)
        {
            chkAllAcctBranch.checked = true;
            cif.checkAll(chkAllAcctBranch.id,acctBranch.id);
        }
        alert("Profile '"+ profileName.value +"' Successfully Loaded!");
        hidePanel("divPopProcess");
    }
    
    this.previewData = function()
    {
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "";
        
        if (this.grid_ajax_query != null)
        {
            this.ajax_grid.open_modal_gridview(0,5);
        }        
    }
    
    this.fillBoxSelected = function(elemFrom,elemTo,arrValue)
    {
        if(elemFrom !=null && elemFrom != undefined)
        {
           for(k=0;k<elemFrom.length;k++)
            {
                for(d=0;d<arrValue.length;d++)
                {
                    if(trim(elemFrom.options[k].text) == trim(arrValue[d]))
                    {
                        elemFrom.options[k].selected = true;
                    }
                }
            }
            while(elemFrom.options.selectedIndex >= 0)
            {
               var addOption = new Option(); 
               addOption.text = elemFrom.options[elemFrom.options.selectedIndex].text; 
               addOption.value = elemFrom.options[elemFrom.options.selectedIndex].value; 
               elemTo.options[elemTo.length] = addOption; 
               elemFrom.remove(elemFrom.options.selectedIndex); 
            }
        }
    }
    
    this.fillSelectedIndex = function(element,arrValue)
    {       
        if(element != null && element != undefined)
        {
           for(k=0;k<element.length;k++)
            {
                for(d=0;d<arrValue.length;d++)
                {
                    if(element.options[k].index == (arrValue[d] - 1))
                    {
                        element.options[k].selected = true;
                    }
                }
            }
        }
    }

    this.fillListValue = function(element,arrValue)
    {       
        if(element != null && element != undefined)
        {
           for(k=0;k<element.length;k++)
            {
                for(d=0;d<arrValue.length;d++)
                {
                    if(element.options[k].value == arrValue[d])
                    {
                        element.options[k].selected = true;
                    }
                }
            }
        }
    }
    
    this.dateFilterSelect = function(element,result)
    {
        for(k = 0;k< element.length;k++)
        {
            if(element.options[k].value == result)
            {
                element.selectedIndex = k;
                break;
            }
        }
    }
    
    this.checkProfileName = function()
    {
        var pname = document.getElementById("txtProfileName").value;
        if(pname !="" || pname != null || pname != undefined)
        {
            var result = IRSWorld.Report_Profiler.CIF_profile_create.isAvailable(pname).value;
            if(result)
            { return true; }
            else
            { return false; }
        }
        else
        {
            alert("Please Input Profile Name!")
        }
    }
    
    this.isAvailable = function()
    {
            var result = cif.checkProfileName();
            if (result != null && result != undefined)
            {
                if(result)
                {
                    alert("Profile Name is Already in Use!");
                }
                else
                {
                    alert("Profile Name is Available");
                }
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


function dateToStr(dt)
{
    var mo = "" + (dt.getMonth() + 1);
    var dd = "" + dt.getDate();
    return dt.getFullYear() + "-" + ((mo.length == 1) ? "0" + mo : "" + mo) + "-" +((dd.length == 1) ? "0" + dd : "" + dd);
}
   
function execute_ajx_command(param)
{
    if (cif != null && cif != undefined)
    {
        cif.do_ajax(param);                 
    }
}

function isDisabled(element,value)
{
    var elem = document.getElementById(element)
    if(value)
    {
        elem.disabled = value;
    }
    else
    {
        elem.disabled = value;
    }
}

function evaluateCIFClass(source)
{
    var src = document.getElementById(source);
    if(src.selectedIndex == 1)
    {
        isDisabled("txtRemitInputID",false);
        document.getElementById("txtRemitInputID").value = "";
        
        isDisabled("txtMerchMemID",true);
        isDisabled("ddlMerchProdCode",true);
        document.getElementById("txtMerchMemID").value = "";
        document.getElementById("ddlMerchProdCode").selectedIndex = 0;    
    }
    else if(src.selectedIndex == 3)
    {
        isDisabled("ddlMerchProdCode",false);
        isDisabled("txtMerchMemID",false);
        document.getElementById("txtMerchMemID").value = "";
        
        isDisabled("txtRemitInputID",true);
        document.getElementById("txtRemitInputID").value = "";

    }
    else
    {
        isDisabled("txtMerchMemID",true);
        isDisabled("ddlMerchProdCode",true);
        document.getElementById("txtMerchMemID").value = "";
        isDisabled("txtRemitInputID",true);
        document.getElementById("txtRemitInputID").value = "";
        document.getElementById("ddlMerchProdCode").selectedIndex = 0;
    }
}

function isNumberKey(evt, ctrlName) 
{
    var str = ctrlName.value;
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 57) 
    {
        return false;
    }
    else if(charCode < 48)
    {
        return false;
    }
    else 
    {
        return true;
    }
}
