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
        cif.loadCIFMerchantsName()
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
        document.getElementById("chbxCIFAcctBranch").checked = false;
        document.getElementById("chbxCIFType").checked = false;
        document.getElementById("cbx_CIFMerchants").checked = false;
       
        isDisabled("btnSave",true);
        isDisabled("btnPreview",true);
        isDisabled("btnExtract",true);
        
        isDisabled("lstMaintainingBranch",false);
        isDisabled("lstSelectParam",false);
        isDisabled("lstCIFBranches",false);
        isDisabled("lstCIFType",false);
        isDisabled("chbxCIFAcctBranch",false);
        isDisabled("chbxCIFType",false);
        isDisabled("chbxCIFBranches",false);
        isDisabled("txtProfileName",false);
        setButton("SAVE","btnSave");        
    }
    
    this.loadCIFProfiles = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFProfiles(cif.loadCIFProfiles_callback);
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
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFClass(cif.loadCIFClass_callback);
    }
    
    this.loadCIFClass_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillView(value, "lstCIFClass", "Description", "Value");
        }
    }
    
    this.loadCIFAcctStatus = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFAcctStatus(cif.loadCIFAcctStatus_callback);
    }
    
    this.loadCIFAcctStatus_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillView(value, "lstAcctStatus", "Description", "Value");
        }
    }
    
    this.loadCIFMerchants = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFMerchCode(cif.loadCIFMerchants_callback);
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
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFBranch(cif.loadCIFbranches_callback);
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
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFAcctCcy(cif.loadCIFAcctCcy_callback);
    }
    
    this.loadCIFAcctCcy_callback = function(result)
    {
         var value = result.value;
         if (value != null && value != "" && value != undefined)
        {
            cif.fillView(value, "lstAcctCurrency", "Description", "Value");
        }
    }
    
    this.loadCIFMaintainBranch = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFAcctbranch(cif.loadCIFMaintainBranch_callback);
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
         IRSWorld.Report_Profiler.CIF_profile_create.getCIFType(cif.loadCIFType_callback);
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
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFAccountType(cif.loadCIFAcctType_callback);
    }
    
    
    this.loadCIFAcctType_callback = function(result)
    {
        var value = result.value;
        if(value != null && value != "" && value != undefined)
        {
            cif.fillView(value, "lstAcctType", "Description", "Value");
        }
    }
    
    this.loadCIFFields = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFFields(cif.loadCIFFields_callback);

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
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFDates(cif.loadDateFilter_callback);
    }
    
    this.loadDateFilter_callback = function(result)
    {
        var value = result.value;
        if(value != null && value != "" && value != undefined)
        {
            cif.fillDDL(value, "ddlDateFilter", "Description", "Value")
        }
    }
    
    this.loadCIFMerchantsName = function()
    {
        IRSWorld.Report_Profiler.CIF_profile_create.getCIFMerchants(cif.loadCIFMerchantsName_callback);
    }
    
    this.loadCIFMerchantsName_callback = function(result)
    {
        var value = result.value;
        if(value != null && value !="")
        {
            cif.fillView(value, "lstCIFMerchants", "MerchantCode", "CIFID");
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
        
        select.options[0] = new Option("-Select All-", "");
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
                alert("There are no items in the source listbox!");
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
               alert("There are no items in the source listBox!");
            }
            else if(fromBox.options.selectedIndex < 0)
            {
                alert("There are no items selected!");
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

      this.getCheckAll = function(element)
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
            var cifClass = cif.getMultiSelect("lstCIFClass");
            var cifType = cif.getMultiSelect("lstCIFType");
            var acctType = cif.getMultiSelect("lstAcctType");
            var acctStatus = cif.getMultiSelect("lstAcctStatus");
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
            var acctCcy = cif.getMultiSelect("lstAcctCurrency");
            
            var chkAllCifClass = cif.getCheckAll("cbx_CIFClass");
            var chkAllAcctStatus = cif.getCheckAll("cbx_AcctStatus");
            var chkAllAcctCcy = cif.getCheckAll("cbx_AcctCurrency");
            var chkAllAcctType = cif.getCheckAll("cbx_AcctType");
             
            var chkAllMerchs = cif.getCheckAll("cbx_CIFMerchants");
            var merch = cif.getMultiSelect("lstCIFMerchants");
            
            if(selectField == null || selectField == undefined || selectField == "" || selectField.length < 1)
            {
                alert("No items selected from listbox below!");
                hidePanel("divPopProcess");
            }
            else if((!chkAllCifClass) && ((cifClass == null || cifClass == "")))
            {
                alert("Please select CIF class!");
                hidePanel("divPopProcess");
            }
            else if((!chkAllCIFType) && ((cifType == null || cifType == "")))
            {
                alert("Please select CIF type");
                hidePanel("divPopProcess");
            }
            else if((!chkAllCIFBranch) && (cifBranch == "" || cifBranch == null))
            {
                alert("Please select CIF branch");
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
                                                                         remInputId,merchMemID,merchProdCode,merch,chkAllMerchs,chkAllCifClass,chkAllAcctStatus,
                                                                         chkAllAcctCcy,chkAllAcctType,cif.parseData_callback);
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
            alert("Query OK with 0 results found");
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
            alert("Please input profile name!");
            hidePanel("divPopProcess");
        }
        else
        {
            IRSWorld.Report_Profiler.CIF_profile_create.ExtractFile(profileName,cif.extractFile_callback);
        }
    }
    
    this.extractFile_callback = function(result)
    {
        var value = result.value;
        if(value != null || value != undefined)
        {
            if(value.indexOf("Extract Complete")!= -1)
            {
                var answer = confirm("File extract completed, would you like to download file?");
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
            hidePanel("divPopProcess");
        }
        else
        {
            alert("Error while extracting file");
        }
        hidePanel("divPopProcess");
    }
    
    this.saveProfile = function()
    {
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById('lblLoadingMsg'),'Saving profile please wait...');
        
        var btn = document.getElementById("btnSave")
        var idNo = document.getElementById("txtIdNumber").value;
        var guid = document.getElementById("txtCIFGUID").value;
        var acctNo = document.getElementById("txtAccountNo").value;
        var cifClass = cif.getMultiSelect("lstCIFClass");
        var cifType = cif.getMultiSelect("lstCIFType");
        var acctType = cif.getMultiSelect("lstAcctType");
        var acctStatus = cif.getMultiSelect("lstAcctStatus");
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
        var acctCcy = cif.getMultiSelect("lstAcctCurrency");
        var selectText = cif.getSelectedText("lstSelectedParam");

        var chkAllCifClass = cif.getCheckAll("cbx_CIFClass");
        var chkAllAcctStatus = cif.getCheckAll("cbx_AcctStatus");
        var chkAllAcctCcy = cif.getCheckAll("cbx_AcctCurrency");
        var chkAllAcctType = cif.getCheckAll("cbx_AcctType");
         
        var chkAllMerchs = cif.getCheckAll("cbx_CIFMerchants");
        var merch = cif.getMultiSelect("lstCIFMerchants");
        
       if(profileName == null || profileName == undefined || profileName == "")
       {    
            alert("Please input profile name!");   
            hidePanel("divPopProcess");
       }
       else if(selectText == null || selectText == undefined || selectText == "")
       {
            alert("No items selected from listbox below!");
            hidePanel("divPopProcess");
       }
       else if(cifClass == null || cifClass == undefined || cifClass == "")
       {
            alert("Please select CIF class!");
            hidePanel("divPopProcess");
       }
       else if((!chkAllCIFType) && (cifType == null || cifType == ""))
       {
            alert("Please select CIF type");
            hidePanel("divPopProcess");
       }
       else if((!chkAllCIFBranch) && (cifBranch == "" || cifBranch == null))
       {
            alert("Please select CIF branch");
            hidePanel("divPopProcess");
       }
       else
       {
            if(btn.value == "UPDATE")
            {
                if(confirm("Do you want to Update profile?"))
                {
                    if(confirm("Do you want to update profile?"))
                    {
                        AjaxPro.timeoutPeriod = 1000 * 1000;
                        AjaxPro.onTimeout = function()
                        {
                             alert("The server stopped responding. Please try again.");
                        }
                        IRSWorld.Report_Profiler.CIF_profile_create.UpdateProfile(top,idNo,guid,cifName,acctNo,acctType,acctStatus,acctCcy,acctBranch,cifClass,cifType,dateFilter,
                                                                         dateFrom,dateTo,cifBranch,selectText,chkAllCIFBranch,chkAllCIFType,chkAllAcctBranch,
                                                                         remInputId,merchMemID,merchProdCode,profileName,merch,chkAllMerchs,chkAllCifClass,chkAllAcctStatus,
                                                                         chkAllAcctCcy,chkAllAcctType,cif.saveProfile_callback);
                    }
                    else
                    {
                        hidePanel("divPopProcess");
                    }
                }
                else
                {
                    hidePanel("divPopProcess");
                }
            }
            else
            {      
                if(cif.checkProfileName())
                {
                    alert("Profile name is already in use!");
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
                                                                     remInputId,merchMemID,merchProdCode,profileName,merch,chkAllMerchs,chkAllCifClass,chkAllAcctStatus,
                                                                     chkAllAcctCcy,chkAllAcctType,cif.saveProfile_callback);
                }
            }  
       }
    }
    
    this.saveProfile_callback = function(result)
    {
        var value = result.value;
        if(value != "" || value != null || value != undefined)
        {
            alert(value);
        }
        cif.loadCIFProfiles();
        isDisabled("txtProfileName",false)
        document.getElementById("btnExtract").disabled = true;
        document.getElementById("btnPreview").disabled = true;
        document.getElementById("btnSave").disabled = true;
        setButton("SAVE","btnSave");
        hidePanel("divPopProcess");
    }  
    
    this.loadExistingProfile = function(element)
    {
        var a = 1;
        var b;
        b = a;
        showPanel("divPopProcess");
        new common_method().to_show_loading_message("divLoadingMsg");
        setText(document.getElementById('lblLoadingMsg'),'Loading profile please wait...');
        
        var index = document.getElementById(element).selectedIndex;
        var value = document.getElementById(element);
        if(value.value != "")
        {
            var profileName = value.options[index].text;
            var profileIndex = value.options[index].value;
            if(index == 0 || index == -1)
            {
                alert("No Profile selected, please select profile to load!");
                hidePanel("divPopProcess");
            }
            else
            {   
                cif.initializePage();
                IRSWorld.Report_Profiler.CIF_profile_create.loadProfile(profileName,cif.loadExistingProfile_callback);
            }
        }
        else
        {
            hidePanel("divPopProcess");
            alert("No profile selected, please select profile to load!");
        }
    }
    
    this.loadExistingProfile_callback = function(result)
    {
        if(result.value.Rows[0] != null)
        {
            var table = result.value;
            var idNo = document.getElementById("txtIdNumber");
            var guid = document.getElementById("txtCIFGUID");
            var acctNo = document.getElementById("txtAccountNo");
            var cifClass = document.getElementById("lstCIFClass");
            var cifType = document.getElementById("lstCIFType");
            var acctType = document.getElementById("lstAcctType");
            var acctStatus = document.getElementById("lstAcctStatus");
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
           
            var chkAllCifClass = document.getElementById("cbx_CIFClass");
            var chkAllAcctStatus = document.getElementById("cbx_AcctStatus");
            var chkAllAcctCcy = document.getElementById("cbx_AcctCurrency");
            var chkAllAcctType = document.getElementById("cbx_AcctType");

            var cifName = document.getElementById("txtCIFName");
            var profileName = document.getElementById("txtProfileName");
            var remInputId = document.getElementById("txtRemitInputID");
            var merchMemID = document.getElementById("txtMerchMemID");
            var merchProdCode = document.getElementById("ddlMerchProdCode"); 
            var acctCcy = document.getElementById("lstAcctCurrency");
            var merchs = document.getElementById("lstCIFMerchants");
            var chkMerchs = document.getElementById("cbx_CIFMerchants")
            
            var arrSelectText = table.Rows[0]["SelectField"].toString().split("|");
            var arrAcctBranch = table.Rows[0]["Account_Branch"].toString().split("|");
            var arrCifBranch = table.Rows[0]["BranchCode"].toString().split("|");
            var arrCifType = table.Rows[0]["CIFType"].toString().split("|")
            var evalDateFrom = dateToStr(table.Rows[0]["DateFrom"]);
            var evalDateTo = dateToStr(table.Rows[0]["DateTo"]);        
            var chkCIFBranch = table.Rows[0]["Cbx_Branches"]; 
            var chkCIFType = table.Rows[0]["Cbx_CIFType"]; 
            var chkAcctBranch = table.Rows[0]["Cbx_AcctBranch"]; 
            
            var arrCIFClass = table.Rows[0]["CIFClass"].toString().split("|");
            var arrAcctStatus = table.Rows[0]["Account_Status"].toString().split("|");
            var arrAcctCcy = table.Rows[0]["Account_Ccy"].toString().split("|");
            var arrAcctType = table.Rows[0]["Account_Type"].toString().split("|");
            
            var chkAllMerchs = table.Rows[0]["Cbx_Merchants"];
            var arrMerch = table.Rows[0]["Merchant_Name"].toString().split("|");

            idNo.value = table.Rows[0]["CIFID"];
            guid.value = table.Rows[0]["CIFGUID"];
            acctNo.value = table.Rows[0]["Account_No"];
            top.value = table.Rows[0]["RecordCount"];
            cifName.value = table.Rows[0]["Fullname"];
            remInputId.value = table.Rows[0]["Rem_Inputed_Id"];
            merchMemID.value = table.Rows[0]["Merchant_Member_Id"];
            profileName.value = table.Rows[0]["ProfileName"];
            
            cif.dateFilterSelect(dateFilter,trim(table.Rows[0]["DateFilter"]));
            cif.fillBoxSelected(fromSelect,selectText,arrSelectText);            
            
            if(evalDateFrom != "1900-01-01")
            {
                dateFrom.value = evalDateFrom;
            }
            
            if(evalDateTo != "1900-01-01")
            {
                dateTo.value = evalDateTo;
            }
            
            if(chkAllCifClass == 1)
            {
                chkAllCifClass.checked = true;
                checkAll(chkAllCifClass.id,cifClass.id);
            }
            else
            { 
                cif.fillListValue(cifClass,arrCIFClass); 
            }
            
            if(chkAllAcctStatus == 1)
            {
                chkAllAcctStatus.checked = true;
                checkAll(chkAllAcctStatus.id,acctStatus.id);
            }
            else
            { 
                cif.fillListValue(acctStatus,arrAcctStatus); 
            }
            
            if(chkAllAcctCcy == 1)
            {
                chkAllAcctCcy.checked = true;
                checkAll(chkAllAcctCcy.id,acctCcy.id);
            }
            else
            { 
                cif.fillListValue(acctCcy,arrAcctCcy); 
            }
          
            if(chkAllAcctType == 1)
            {
                chkAllAcctType.checked = true;
                checkAll(chkAllAcctType.id,acctType.id);
            }
            else
            { 
                cif.fillListValue(acctType,arrAcctType); 
            }
           
            if(chkCIFBranch == 1)
            {
                chkAllCIFBranch.checked = true;
                checkAll(chkAllCIFBranch.id,cifBranch.id);
            }
            else
            { 
                cif.fillListValue(cifBranch,arrCifBranch); 
            }
         
            if(chkCIFType == 1)
            {
                chkAllCIFType.checked = true;
                checkAll(chkAllCIFType.id,cifType.id);
            }
            else
            { 
                cif.fillListValue(cifType,arrCifType); 
            }
           
            if(chkAcctBranch == 1)
            {
                chkAllAcctBranch.checked = true;
                checkAll(chkAllAcctBranch.id,acctBranch.id);
            }
            else
            { 
                cif.fillListValue(acctBranch,arrAcctBranch); 
            }
           
            if(chkAllMerchs == 1)
            {
                chkMerchs.checked = true;
                checkAll(chkMerchs.id,merchs.id);
            }
            else 
            { 
                cif.fillListValue(merchs,arrMerch); 
            }
            
            alert("Profile '"+ profileName.value +"' Successfully loaded!");
            setButton("UPDATE","btnSave");
            isDisabled("txtProfileName",true);
            hidePanel("divPopProcess");
        }
        else
        {
            alert("Profile not found.")
            hidePanel("divPopProcess");
        }
    }
    
    this.setddselected = function(ddlist,value)
    {
        var myValue = value;
        var indexcount = 0;
        
        for(i=0;i<ddlist.length;i++)
        {
            if (ddlist.options[i].value == myValue)
            {
                indexcount = i;
                break;
            }
        }
        ddlist.selectedIndex = indexcount;
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
           for(d=0;d<arrValue.length;d++)
            {
                for(k=0;k<elemFrom.length;k++)
                {
                    if(trim(elemFrom.options[k].text) == trim(arrValue[d]))
                    {
                        elemFrom.options[k].selected = true;
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
        if(pname !="")
        {
            var result = IRSWorld.Report_Profiler.CIF_profile_create.isUsed(pname).value;
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
    
    this.isUsed = function()
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

function checkAll(chkBox,lstBox)
{
    var chk = document.getElementById(chkBox);
    var lst = document.getElementById(lstBox);
    if(chk.checked == 1)
    {
      for(var k = 0;k < lst.options.length;k++)
      {
         lst.options[k].selected = true;
      }
      isDisabled(lstBox,true);
    }
    else
    {
      isDisabled(lstBox,false);
      lst.options.selectedIndex = -1;
    }
}

function isNotAvailable(element,value)
{
    var elem = doucment.getElementbyId(element);
    if(value)
    {   
        elem.style.backgroundColor = "gray";
    }
    else
    {
        elem.style.backgroundColor = "";
    }
}

function evaluateCIFClass(source)
{
    var src = document.getElementById(source);
    if(src.value == 1)
    {
        isDisabled("txtRemitInputID",false);
        document.getElementById("txtRemitInputID").value = "";
        
        isDisabled("txtMerchMemID",false);
        isDisabled("ddlMerchProdCode",false);
        isDisabled("lstCIFMerchants",false);
        isDisabled("cbx_CIFMerchants",false);
        document.getElementById("txtMerchMemID").value = "";
        document.getElementById("ddlMerchProdCode").selectedIndex = 0;
        document.getElementById("lstCIFMerchants").options.selectedIndex = -1;
        document.getElementById("cbx_CIFMerchants").checked = false;
    }
    else if(src.value == 3)
    {
        isDisabled("ddlMerchProdCode",false);
        isDisabled("txtMerchMemID",false);
        document.getElementById("txtMerchMemID").value = "";
        isDisabled("lstCIFMerchants",false);
        isDisabled("cbx_CIFMerchants",false);
        
        isDisabled("txtRemitInputID",true);
        document.getElementById("txtRemitInputID").value = "";
        document.getElementById("cbx_CIFMerchants").checked = false;
        document.getElementById("lstCIFMerchants").options.selectedIndex = -1;
    }
    else
    {
        isDisabled("txtMerchMemID",true);
        isDisabled("ddlMerchProdCode",true);
        document.getElementById("txtMerchMemID").value = "";
        isDisabled("txtRemitInputID",true);
        isDisabled("lstCIFMerchants",true);
        isDisabled("cbx_CIFMerchants",true);
        isDisabled("lstCIFMerchants",true);
        isDisabled("cbx_CIFMerchants",true);

        document.getElementById("txtRemitInputID").value = "";
        document.getElementById("ddlMerchProdCode").selectedIndex = 0;
        document.getElementById("lstCIFMerchants").options.selectedIndex = -1;
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

function setButton(value, button)
{
    var btn = document.getElementById(button)
    btn.value = value;
}
