
  
function execute_ajx_command(param)
        {            
            if (report_builder != null && report_builder != "undefined")
            {
                report_builder.do_ajax(param);                 
            }             
        }
        


function showPanel(divName)
{
    var div = document.getElementById(divName);
    
    divmask = new maskpanel();
    divmask.show();
    showDiv(divName, true);
    
    div.style.top = (((document.documentElement.clientHeight/2) + document.documentElement.scrollTop) - (div.offsetHeight/2)) + 'px';
    div.style.left = (((document.documentElement.clientWidth/2) + document.documentElement.scrollLeft) - (div.offsetWidth/2)) + 'px';
}

function hidePanel(divName)
{
    showDiv(divName, false);
    divmask.hide();
}

       
function report_builder()
{ 
        this.grid_ajax_page = "../AjxPage/ajaxgrid.aspx"; 
       

        this.msg_screen = new screen_msg_demo();       
          

        
       this.moveitems = function(lstbxFrom,lstbxTo)
       {
            var varFromBox = document.all(lstbxFrom);
             var varToBox = document.all(lstbxTo); 
             if ((varFromBox != null) && (varToBox != null)) 
             { 
              if(varFromBox.length < 1) 
              {
               alert('There are no items in the source ListBox');
               //return false;
              }
              if(varFromBox.options.selectedIndex == -1) // when no Item is selected the index will be -1

              {
               alert('Please select an Item to move');
               //return false;
              }
              while ( varFromBox.options.selectedIndex >= 0 ) 
              { 
               var newOption = new Option(); // Create a new instance of ListItem 

               newOption.text = varFromBox.options[varFromBox.options.selectedIndex].text; 
               newOption.value = varFromBox.options[varFromBox.options.selectedIndex].value; 
               varToBox.options[varToBox.length] = newOption; //Append the item in Target Listbox
               varFromBox.remove(varFromBox.options.selectedIndex); //Remove the item from Source Listbox 

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
               alert('There are no items in the source ListBox');
               //return false;
              }
              report_builder.selectAll(lstbxFrom,true,false);
              
              while ( varFromBox.options.selectedIndex >= 0 ) 
              { 
               var newOption = new Option(); // Create a new instance of ListItem 

               newOption.text = varFromBox.options[varFromBox.options.selectedIndex].text; 
               newOption.value = varFromBox.options[varFromBox.options.selectedIndex].value; 
               varToBox.options[varToBox.length] = newOption; //Append the item in Target Listbox
               varFromBox.remove(varFromBox.options.selectedIndex); //Remove the item from Source Listbox 

              } 
              
              } 
             
        }
        
        this.getAllList = function(element)
       {
            var selected = new Array(); 
            var mySelect = document.getElementById(element);
                for(j = 0; j < mySelect.options.length; j++) 
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
        
       this.selectBatchType = function()
       {
            var selected = new Array(); 
            var mySelect = document.getElementById("ddRemitMethod");
            var ddlbatchtype = document.getElementById("ddBatchTypes");
            var chkAllBatchTypes = document.getElementById("chkAllBatchTypes");
                report_builder.clearDDL("ddBatchTypes");
                ddlbatchtype.disabled = true;
                chkAllBatchTypes.disabled = true;
                for(j = 0; j < mySelect.options.length; j++) 
                    { 
                         if(mySelect.options[j].selected) 
                    { 
                        selected.push(mySelect.options[j].value); 
                    } 
                                               
                    
                 }
                 for (i = 0; i <= selected.length - 1; i++)
                    {
                        if (selected != null)
                    {
                                    
                   if (selected[i] == "1")
                   
                   {
                        this.LoadDDLBatchTypes();
                        ddlbatchtype.disabled = false;
                        chkAllBatchTypes.disabled = false;
                   }
                                  
                    }
                                        
                    } 
               
                    
                
       }
       
       this.getmultipleselected = function(element)
       {
            var selected = new Array(); 
            var mySelect = document.getElementById(element);
                for(j = 0; j < mySelect.options.length; j++) 
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
                for(j = 0; j < mySelect.options.length; j++) 
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
             return true;
            else
            return false;
       }
              
       this.selectAll = function(element,selectAll,vardisable) 
       {
		    var element = document.getElementById(element);
		    for (var i = 0; i < element.options.length; i++) 
		    {
				element.options[i].selected = selectAll;
		    }
		        element.disabled = vardisable;
		        return selectAll;
		        
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
        
        
        
        //this.getDDL = function(element, selected, isValue)
        this.getDDL = function(element, isValue)
        {
            var select = document.getElementById(element);
            //document.getElementById(selected).value = (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
        }
        
        
     
        this.LTrim = function(value) // Removes leading whitespaces
        {
	        var re = /\s*((\S+\s*)*)/;
	        return value.replace(re, "$1");
        }
        
        

        this.RTrim = function(value) // Removes ending whitespaces
        {
	        var re = /((\s*\S+)*)\s*/;
	        return value.replace(re, "$1");
        }
        
        
    
        this.trim = function(value) // Removes leading and ending whitespaces
        {
            if (value != null && value != "" && value != undefined)
            {
                return this.LTrim(this.RTrim(value));
            }
	        else
	        {
	            return value;
	        }
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
                        if (iRow.Rows[i][arrSplit[arr]] != null && report_builder.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && report_builder.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
                //select.options[i + 1] = new Option(arrValue, iRow.Rows[i][value]);
                //select.options[i + 1] = new Option(arrValue, arrIDValue);
                select.options[i] = new Option(arrValue, arrIDValue);
                
                
                arrValue = "";
                arrIDValue = "";
            }
            //report_builder.getDDL(element, selected, true);
            report_builder.getDDL(element, true);
        }
    }
    
    
    this.fillDDLCondition = function(element)
    
    {
        var select = document.getElementById(element);
        select.options[0] = new Option(">=", ">=");
        select.options[1] = new Option("<=", "<=");
        select.options[2] = new Option("=", "=");
        
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
            
            select.options[0] = new Option("--SELECT ALL--", "");
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && report_builder.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && report_builder.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
                //select.options[i + 1] = new Option(arrValue, iRow.Rows[i][value]);
                select.options[i + 1] = new Option(arrValue, arrIDValue);
                arrValue = "";
                arrIDValue = "";
            }
            //report_builder.getDDL(element, selected, true);
            report_builder.getDDL(element, true);
        }
    }
    
    
    
    
        this.initializepage = function()
        {
          
            this.LoadDDLRemitMethod();
            this.LoadDDLBatchTypes();
            this.LoadDDLOverseasBranches();
            this.LoadDDLDomesticBranches();
            this.LoadDateColumns();
            this.LoadAmtFilter();
            this.LoadAmtComputation();
            this.LoadDDLRemittingCCY();
            this.LoadDDLTargetCCY();
            this.LoadDDLProductCode();
            this.LoadDDLTranxSource();
            this.LoadDDLPayMode();
            this.LoadDDLTranxStatus();
            this.LoadDDLReportField();
            this.LoadTxtDateToday();
            this.LoadDDLProfiles();
            report_builder.setValue("txtRemitterID","");
            report_builder.setValue("txtBeneficiaryID","");
            report_builder.setValue("txtAccountNo","");
            report_builder.setValue("txtAmount","0.00");
            report_builder.setValue("txtSQLQuery","");
            report_builder.setValue("txtProfileName","");
            
            var btnExtract= document.getElementById("btnExtract");
            var btnPreview  = document.getElementById("btnPreview");
            var btnSave = document.getElementById("btnSave");
            
            btnExtract.disabled = true;
            btnPreview.disabled = true;
            btnSave.disabled = true;
          
            var chkAllRemitMethod = document.getElementById("chkAllRemitMethod");
            chkAllRemitMethod.disabled = false;
            chkAllRemitMethod.checked = false;
            
            var chkAllOverseas = document.getElementById("chkAllOverseas");
            chkAllOverseas.disabled = true;
            chkAllOverseas.checked = false;
            
            var chkAllDomestic = document.getElementById("chkAllDomestic");
            chkAllDomestic.disabled = false;
            chkAllDomestic.checked = false;                           
            
            var chkAllBatchTypes = document.getElementById("chkAllBatchTypes");
            chkAllBatchTypes.disabled = false;
            chkAllBatchTypes.checked = false;
            report_builder.clearDDL("ddBatchTypes");
            
            var RemitMethod = document.getElementById("ddRemitMethod");
            RemitMethod.disabled = false;
            
            var OverseasBranches = document.getElementById("ddOverseasBranches");
            OverseasBranches.disabled = false;
            
            var DomesticBranches = document.getElementById("ddDomesticBranches");
            DomesticBranches.disabled = false;
            
            

        }
        
        
        
        
         this.LoadTxtDateToday = function()
        {
        
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getserverDate(this.LoadTxtDateToday_callback).value;
            IRSWorld.Report_Profiler.tranx_profile_createV2.getserverDate(this.LoadTxtDateToday_callback);
        }
    
        this.LoadTxtDateToday_callback = function(result)
        {
            var response = result.value;
            report_builder.setValue("txtDateFrom",response)
            report_builder.setValue("txtDateTo",response)
            
        }
        
        this.LoadDDLRemitMethod = function()
        {
            report_builder.clearDDL("ddRemitMethod");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getRemitMethod(this.LoadDDLRemitMethod_callback).value;
            IRSWorld.Report_Profiler.tranx_profile_createV2.getRemitMethod(this.LoadDDLRemitMethod_callback);
        
        }
    
        this.LoadDDLRemitMethod_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
        {
            //rpt_profile_tranx.fillDDL(response, "ddBranches", "BranchName", "BranchCode", "lblCIFClass", "");
            report_builder.fillDDL(response, "ddRemitMethod", "FldDescription", "FldValue", "", "");
        }
        }
    
        
        
        this.LoadDDLOverseasBranches = function()
        {
            report_builder.clearDDL("ddOverseasBranches");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getIRSOverseas_Branches(this.LoadDDLOverseasBranches_callback).value;
            IRSWorld.Report_Profiler.tranx_profile_createV2.getIRSOverseas_Branches(this.LoadDDLOverseasBranches_callback);
        }
    
        this.LoadDDLOverseasBranches_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
        {
            //rpt_profile_tranx.fillDDL(response, "ddBranches", "BranchName", "BranchCode", "lblCIFClass", "");
            report_builder.fillDDL(response, "ddOverseasBranches", "BranchName", "BranchCode", "", "");
        }
        }
    
        this.LoadDDLDomesticBranches = function()
        {
        
            report_builder.clearDDL("ddDomesticBranches");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getIRSDomestic_Branches(this.LoadDDLDomesticBranches_callback).value;
            IRSWorld.Report_Profiler.tranx_profile_createV2.getIRSDomestic_Branches(this.LoadDDLDomesticBranches_callback);
        }
    
        this.LoadDDLDomesticBranches_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
            //rpt_profile_tranx.fillDDL(response, "ddBranches", "BranchName", "BranchCode", "lblCIFClass", "");
            report_builder.fillDDL(response, "ddDomesticBranches", "BranchName", "BranchCode", "", "");
            }
        }
        
        this.LoadDateColumns = function()
        {
            report_builder.clearDDL("ddDateFilter");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getDateColumns(this.LoadDateColumns_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getDateColumns(this.LoadDateColumns_callback);     
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
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getAmtFilter(this.LoadAmtFilter_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getAmtFilter(this.LoadAmtFilter_callback);     
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
        //return IRSWorld.Report_Profiler.tranx_profile_createV2.getComputation(this.LoadAmtComputation_callback).value;     
        IRSWorld.Report_Profiler.tranx_profile_createV2.getComputation(this.LoadAmtComputation_callback);     
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
             //return IRSWorld.Report_Profiler.tranx_profile_createV2.getRemittingCCY(this.LoadDDLRemittingCCY_callback).value;     
             IRSWorld.Report_Profiler.tranx_profile_createV2.getRemittingCCY(this.LoadDDLRemittingCCY_callback);     
        }
        
        this.LoadDDLRemittingCCY_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                
                report_builder.fillDDLwithSelectALL(response, "ddRemittingCCY", "FldCode", "FldValue", "", "");
                
            }
        }
        
        this.LoadDDLTargetCCY = function()
        {
            report_builder.clearDDL("ddTargetCCY");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getTargetCCY(this.LoadDDLTargetCCY_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getTargetCCY(this.LoadDDLTargetCCY_callback);     
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
            report_builder.clearDDL("ddProductCode");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getProductCode(this.LoadDDLProductCode_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getProductCode(this.LoadDDLProductCode_callback);     
            
        }
        
        this.LoadDDLProductCode_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                report_builder.fillDDLwithSelectALL(response, "ddProductCode", "FldDescription", "FldValue", "", "");
            }
            
        }
        
        this.LoadDDLTranxSource = function()
        {
            report_builder.clearDDL("ddTransactionSource");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getTranxSource(this.LoadDDLTranxSource_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getTranxSource(this.LoadDDLTranxSource_callback);     
        }
        
        this.LoadDDLTranxSource_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                report_builder.fillDDLwithSelectALL(response, "ddTransactionSource", "FldDescription", "FldValue", "", "");
            }
            
        }
        
        this.LoadDDLPayMode = function()
        {
            report_builder.clearDDL("ddPayMode");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getPayMode(this.LoadDDLPayMode_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getPayMode(this.LoadDDLPayMode_callback);     
        }
        
        this.LoadDDLPayMode_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                report_builder.fillDDLwithSelectALL(response, "ddPayMode", "FldDescription", "FldValue", "", "");
            }
            
        }
        
        this.LoadDDLTranxStatus = function()
        {
            report_builder.clearDDL("ddTransactionStatus");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getTranxStatus(this.LoadDDLTranxStatus_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getTranxStatus(this.LoadDDLTranxStatus_callback);     
        }
        
        this.LoadDDLTranxStatus_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                report_builder.fillDDLwithSelectALL(response, "ddTransactionStatus", "FldDescription", "FldValue", "", "");
            }
            
        }
        
        
        
        this.LoadDDLBatchTypes = function()
        {
            report_builder.clearDDL("ddBatchTypes");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getBatchUploadMethods(this.LoadDDLBatchTypes_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getBatchUploadMethods(this.LoadDDLBatchTypes_callback);     
        }
        
        this.LoadDDLBatchTypes_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                
                report_builder.fillDDL(response, "ddBatchTypes", "FldDescription", "Seq_ID", "", "");
                
            }
        }
        
        this.LoadDDLReportField = function()
        {
            report_builder.clearDDL("ddFieldSelected");
            report_builder.clearDDL("ddFieldChoices");
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getReportField(this.LoadDDLReportField_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getReportField(this.LoadDDLReportField_callback);     
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
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getProfileList(this.LoadDDLProfiles_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getProfileList(this.LoadDDLProfiles_callback);     
        }
        
        this.LoadDDLProfiles_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                
                report_builder.fillDDL(response, "ddProfiles", "ProfileName", "ProfileName", "", "");
                
            }
        }
        
                        
        this.CheckselectAll = function (selectelement,checkboxelement)
    
        {
            var Ischecked = report_builder.checkboxcheck(checkboxelement);
            report_builder.selectAll(selectelement,Ischecked,Ischecked);
            
        
        }
        
        this.CheckselectAllRemitMethod = function (selectelement,checkboxelement)
    
        {
            
            var Ischecked = report_builder.checkboxcheck(checkboxelement);
            report_builder.selectAll(selectelement,Ischecked,Ischecked);
           
            //var ddlbatchtype = document.getElementById("ddBatchTypes");
            //var chkAllBatchTypes = document.getElementById("chkAllBatchTypes");
            //report_builder.clearDDL("ddBatchTypes");    
            
            //if (Ischecked)
            //{
            //    this.LoadDDLBatchTypes();
            //}
          
            //ddlbatchtype.disabled = !Ischecked;
            //chkAllBatchTypes.disabled = false;
            //chkAllBatchTypes.checked = false;
            
        
        }

        this.ParseData = function()
        
        {   
            //report_builder.msg_screen.open_screen(); 
            //report_builder.msg_screen.change_msg("LOADING PLEASE WAIT ...");
            //showPanel('divtxtarea');
            showPanel('divPopProcess');
            new common_method().to_show_loading_message('divLoadingMsg');
            setText(document.getElementById('lblLoadingMsg'), 'Parsing request please wait...');
            report_builder.ParseQuery();
        }
        
        this.ParseQuery = function()
    
        {
            var IsValid = true;
            var RemitMethod = report_builder.getmultipleselected("ddRemitMethod");
            var BatchTypes = report_builder.getmultipleselected("ddBatchTypes");
            var RemBrCode = report_builder.getmultipleselected("ddOverseasBranches");
            var CredBrCode = report_builder.getmultipleselected("ddDomesticBranches");
            var ProductCode = report_builder.getValue("ddProductCode");
            var TransactionSource = report_builder.getValue("ddTransactionSource");
            var PayMode = report_builder.getValue("ddPayMode");
            var TransactionStatus = report_builder.getValue("ddTransactionStatus");
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
            
            if (report_builder.trim(ProfileName) == "")
            {
            IsValid = false;
            alert('Please fill up profile name');
            }
            
            if (ReportField.length == 0)
            {
            IsValid = false;
            alert('Please select fields/columns');
            }
            
            if (RemitMethod.length == 0)
            {
            IsValid = false;
            alert('Please select at least 1 Remit Method');
            }
           
                   
            if (IsValid)
            {
                    
            
//            return IRSWorld.Report_Profiler.tranx_profile_createV2.ParseQuery(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
//                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
//                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
//                                                 TransactionStatus,report_builder.trim(RemitterID),report_builder.trim(BeneficiaryID),
//                                                 report_builder.trim(AccountNo),ReportField,CheckMainBr,CheckRemBr,this.ParseQuery_callback).value;


            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function()
            {
                alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                
            }
             IRSWorld.Report_Profiler.tranx_profile_createV2.ParseQuery(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
                                                 TransactionStatus,report_builder.trim(RemitterID),report_builder.trim(BeneficiaryID),
                                                 report_builder.trim(AccountNo),ReportField,CheckMainBr,CheckRemBr,this.ParseQuery_callback);


            }
            else
            {
                //report_builder.msg_screen.close_screen();
                //hidePanel('divtxtarea');
                hidePanel('divPopProcess');
            }
        }
        
        this.ParseQuery_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                  
                    var iRow = response;                        
                    alert('Parse OK with ' +  iRow.Rows.length + 'row(s)');
                    
                        var btnExtract= document.getElementById("btnExtract");
                        var btnPreview  = document.getElementById("btnPreview");
                        var btnSave = document.getElementById("btnSave");
                        
                        btnExtract.disabled = false;
                        btnPreview.disabled = false;
                        btnSave.disabled = false;    
                        
                        //report_builder.msg_screen.close_screen();
                        //hidePanel('divtxtarea');
                        hidePanel('divPopProcess');
              }
              else
              {
               //report_builder.msg_screen.close_screen();
               //hidePanel('divtxtarea');
               hidePanel('divPopProcess');
              }
            
            
                    
        }
        
        this.ExtractReport = function()
        
        {   
            //showPanel('divtxtarea');
            showPanel('divPopProcess');
            new common_method().to_show_loading_message('divLoadingMsg');
            setText(document.getElementById('lblLoadingMsg'), 'Preparing to extract please wait...');
            report_builder.ExtractReporttoFile();
        }
        
        this.ExtractReporttoFile = function()
    
        {
            var IsValid = true;
            var RemitMethod = report_builder.getmultipleselected("ddRemitMethod");
            var BatchTypes = report_builder.getmultipleselected("ddBatchTypes");
            var RemBrCode = report_builder.getmultipleselected("ddOverseasBranches");
            var CredBrCode = report_builder.getmultipleselected("ddDomesticBranches");
            var ProductCode = report_builder.getValue("ddProductCode");
            var TransactionSource = report_builder.getValue("ddTransactionSource");
            var PayMode = report_builder.getValue("ddPayMode");
            var TransactionStatus = report_builder.getValue("ddTransactionStatus");
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
            
            if (report_builder.trim(ProfileName) == "")
            {
            IsValid = false;
            alert('Please fill up profile name');
            }
            
            if (ReportField.length == 0)
            {
            IsValid = false;
            alert('Please select fields/columns');
            }
            
            if (RemitMethod.length == 0)
            {
            IsValid = false;
            alert('Please select at least 1 Remit Method');
            }
           
                   
            if (IsValid)
            {
                    
            
//            return IRSWorld.Report_Profiler.tranx_profile_createV2.ExtractReporttoFile(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
//                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
//                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
//                                                 TransactionStatus,report_builder.trim(RemitterID),report_builder.trim(BeneficiaryID),
//                                                 report_builder.trim(AccountNo),ReportField,CheckMainBr,CheckRemBr,ProfileName,
//                                                 this.ExtractReporttoFile_callback).value;

            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function()
            {
                alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                
            }

              IRSWorld.Report_Profiler.tranx_profile_createV2.ExtractReporttoFile(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
                                                 TransactionStatus,report_builder.trim(RemitterID),report_builder.trim(BeneficiaryID),
                                                 report_builder.trim(AccountNo),ReportField,CheckMainBr,CheckRemBr,ProfileName,
                                                 this.ExtractReporttoFile_callback);

            }
            else
            {
                //report_builder.msg_screen.close_screen();
                //hidePanel('divtxtarea');
                hidePanel('divPopProcess');
            }
        }
        
        this.ExtractReporttoFile_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                    
                    
                    if (response.substring(0,17) == "Extract Complete:")
                    {
                        var answer = confirm  ("File extract completed would you like to download file?");
                        if (answer)
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
                        
                        btnExtract.disabled = true;
                        btnPreview.disabled = true;
                        btnSave.disabled = true;    
                        
                        //report_builder.msg_screen.close_screen();
                        //hidePanel('divtxtarea');
                        hidePanel('divPopProcess');
              }
              else
              {
               //report_builder.msg_screen.close_screen();
               //hidePanel('divtxtarea');
               hidePanel('divPopProcess');
              }
            
            
                    
        }
        
        
        this.PreSaveReportProfile = function()
        {
           //showPanel('divtxtarea');
           showPanel('divPopProcess');
           new common_method().to_show_loading_message('divLoadingMsg');
           setText(document.getElementById('lblLoadingMsg'), 'Preparing to save...');
           report_builder.BuildSQLQuery();
        } 
          
        this.BuildSQLQuery = function()
    
        {
            
            var IsValid = true;
            var RemitMethod = report_builder.getmultipleselected("ddRemitMethod");
            var BatchTypes = report_builder.getmultipleselected("ddBatchTypes");
            var RemBrCode = report_builder.getmultipleselected("ddOverseasBranches");
            var CredBrCode = report_builder.getmultipleselected("ddDomesticBranches");
            var ProductCode = report_builder.getValue("ddProductCode");
            var TransactionSource = report_builder.getValue("ddTransactionSource");
            var PayMode = report_builder.getValue("ddPayMode");
            var TransactionStatus = report_builder.getValue("ddTransactionStatus");
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
            
            if (report_builder.trim(ProfileName) == "")
            {
            IsValid = false;
            alert('Please fill up profile name');
            }
            
            if (ReportField.length == 0)
            {
            IsValid = false;
            alert('Please select fields/columns');
            }
            
            if (RemitMethod.length == 0)
            {
            IsValid = false;
            alert('Please select at least 1 Remit Method');
            }
           
                   
            if (IsValid)
            {
                    
//            return IRSWorld.Report_Profiler.tranx_profile_createV2.BuildSQLQuery(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
//                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
//                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
//                                                 TransactionStatus,report_builder.trim(RemitterID),report_builder.trim(BeneficiaryID),
//                                                 report_builder.trim(AccountNo),ReportField,CheckMainBr,CheckRemBr,this.BuildSQLQuery_callback).value;

            AjaxPro.timeoutPeriod = 1000 * 1000;
            AjaxPro.onTimeout = function()
            {
                alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                
            }
            IRSWorld.Report_Profiler.tranx_profile_createV2.BuildSQLQuery(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
                                                 TransactionStatus,report_builder.trim(RemitterID),report_builder.trim(BeneficiaryID),
                                                 report_builder.trim(AccountNo),ReportField,CheckMainBr,CheckRemBr,this.BuildSQLQuery_callback);

            
           }
           else
           {
                //report_builder.msg_screen.close_screen();
                //hidePanel('divtxtarea');
                hidePanel('divPopProcess');
           }
        } 
    
        this.BuildSQLQuery_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
            //report_builder.setValue( "txtSQLQuery",response);                  
            //var btnExtract= document.getElementById("btnExtract");
            //var btnPreview  = document.getElementById("btnPreview");
            //var btnSave = document.getElementById("btnSave");            
            //btnExtract.disabled = false;
            //btnPreview.disabled = false;
            //btnSave.disabled = false;
            
            report_builder.SaveReportProfile(response);
            }
            else
            {
                //report_builder.msg_screen.close_screen();
                //hidePanel('divtxtarea');
                hidePanel('divPopProcess');
            }
        
        }
        
        
        
       
        
        this.SaveReportProfile = function(SQLQuerySyntax)
    
        {
            
            var IsValid = true;
            var RemitMethod = report_builder.getmultipleselectedText("ddRemitMethod");
            var BatchTypes = report_builder.getmultipleselectedText("ddBatchTypes");
            var RemBrCode = report_builder.getmultipleselectedText("ddOverseasBranches");
            var CredBrCode = report_builder.getmultipleselectedText("ddDomesticBranches");
            
            var ProductCode = report_builder.getselectedText("ddProductCode");
            var TransactionSource = report_builder.getselectedText("ddTransactionSource");
            var PayMode = report_builder.getselectedText("ddPayMode");
            var TransactionStatus = report_builder.getselectedText("ddTransactionStatus");
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
            var ProfileName = report_builder.getValue("txtProfileName");
            //var SQLSyntax = report_builder.getValue("txtSQLQuery");
            var SQLSyntax = SQLQuerySyntax
            var ReportField = report_builder.getAllTextList("ddFieldSelected");
            
            
            var CheckMainBr = report_builder.checkboxcheck("chkAllDomestic");
            var CheckRemBr =  report_builder.checkboxcheck("chkAllOverseas");
            var chkRemitMethod = report_builder.checkboxcheck("chkAllRemitMethod");
            var chkBatchTypes =  report_builder.checkboxcheck("chkAllBatchTypes");
            
            if (report_builder.trim(ProfileName) == "")
            {
            IsValid = false;
            alert('Please fill up profile name');
            }
            
            if (ReportField.length == 0)
            {
            IsValid = false;
            alert('Please select fields/columns');
            }
            
            if (RemitMethod.length == 0)
            {
            IsValid = false;
            alert('Please select at least 1 Remit Method');
            }
            
            if (report_builder.CheckProfileName())
            {
            IsValid = false;
            alert('Profile name already in use');
            }
           
            if (IsValid)
            {
                                                                                                                          
//                return IRSWorld.Report_Profiler.tranx_profile_createV2.SaveReportProfile(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
//                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
//                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
//                                                 TransactionStatus,report_builder.trim(RemitterID),
//                                                 report_builder.trim(BeneficiaryID),report_builder.trim(AccountNo),ReportField,
//                                                 CheckMainBr,CheckRemBr,report_builder.trim(ProfileName),SQLSyntax,chkRemitMethod,
//                                                 chkBatchTypes,this.SaveReportProfile_callback).value;
                
                AjaxPro.timeoutPeriod = 1000 * 1000;
                AjaxPro.onTimeout = function()
                {
                    alert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
                    
                }
                IRSWorld.Report_Profiler.tranx_profile_createV2.SaveReportProfile(RemBrCode,CredBrCode,DateFilter,DateFrom,DateTo,
                                                 RemittingCCY,TargetCCY,AmtComputation, AmtFilter, report_builder.trim(Amount),
                                                 RemitMethod,BatchTypes,ProductCode,TransactionSource,PayMode,
                                                 TransactionStatus,report_builder.trim(RemitterID),
                                                 report_builder.trim(BeneficiaryID),report_builder.trim(AccountNo),ReportField,
                                                 CheckMainBr,CheckRemBr,report_builder.trim(ProfileName),SQLSyntax,chkRemitMethod,
                                                 chkBatchTypes,this.SaveReportProfile_callback);

                                               

            } 
            
            else
            {
               //report_builder.msg_screen.close_screen(); 
               //hidePanel('divtxtarea');
               hidePanel('divPopProcess');
            }
            
        }        
            
        
        this.SaveReportProfile_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
            alert(response);
            //report_builder.setValue( "txtSQLQuery",response);
            }
            
            report_builder.LoadDDLProfiles();
            
            var btnExtract= document.getElementById("btnExtract");
            var btnPreview  = document.getElementById("btnPreview");
            var btnSave = document.getElementById("btnSave");
            
            btnExtract.disabled = true;
            btnPreview.disabled = true;
            btnSave.disabled = true;
            //report_builder.msg_screen.close_screen();
            //hidePanel('divtxtarea');
            hidePanel('divPopProcess');
           
        }
        
        this.CheckProfileName = function()
        
        {
            var ProfileName = report_builder.getValue("txtProfileName");
            
            return IRSWorld.Report_Profiler.tranx_profile_createV2.CheckProfileName(report_builder.trim(ProfileName)).value;
                   
           
        }
        
        this.Click_CheckProfileName = function()
        
        {
            if (report_builder.CheckProfileName())
            {
            alert('Profile name already existed');
          
            }
            else
            {
            alert('Profile name available');
          
            }
           
        }
        
        this.LoadProfile = function()
        
        {
            
            var ProfileName = report_builder.getselectedText("ddProfiles");
                       
            if (report_builder.trim(ProfileName) == "")
            {
            alert('No available profile to load');
            }
            else
            {
            report_builder.initializepage();
            //return IRSWorld.Report_Profiler.tranx_profile_createV2.getProfileDetails(report_builder.trim(ProfileName),this.LoadProfile_callback).value;     
            IRSWorld.Report_Profiler.tranx_profile_createV2.getProfileDetails(report_builder.trim(ProfileName),this.LoadProfile_callback);     
            }
        }
        
               
        this.LoadProfile_callback = function(result)
        {
            var response = result.value;
            if (response != null && response != "" && response != undefined)
            {
                
                report_builder.fillLoadedProfile(response);
                
            }
        }
        
        this.fillLoadedProfile = function(table)
        {
            
            var DT = table;
            
            var chkAllRemitMethod = document.getElementById("chkAllRemitMethod");
            var chkAllOverseas = document.getElementById("chkAllOverseas");
            var chkAllDomestic = document.getElementById("chkAllDomestic");
            var chkAllBatchTypes = document.getElementById("chkAllBatchTypes");
            var ddlbatchtype = document.getElementById("ddBatchTypes");
            
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
            report_builder.setValue("txtSQLQuery",SQLQuery);
            report_builder.setValue("txtProfileName",DT.Rows[0]["ProfileName"]);
            report_builder.setValue("txtDateFrom",DateFrom);
            report_builder.setValue("txtDateTo",DateTo);
            
            report_builder.setddselected("ddProductCode",DT.Rows[0]["ProductCode"]);
            report_builder.setddselected("ddTransactionSource",DT.Rows[0]["TransactionSource"]);
            report_builder.setddselected("ddPayMode",DT.Rows[0]["PayMode"]);
            report_builder.setddselected("ddTransactionStatus",DT.Rows[0]["TransactionStatus"]);
            report_builder.setddselected("ddDateFilter",DT.Rows[0]["DateFilter"]);
            report_builder.setddselected("ddRemittingCCY",DT.Rows[0]["RemittingCurrency"]);
            report_builder.setddselected("ddTargetCCY",DT.Rows[0]["TargetCurrency"]);
            report_builder.setddselected("ddAmtComputation",DT.Rows[0]["AmountComputation"]);
            report_builder.setddselected("ddAmtFilter",DT.Rows[0]["AmountEquation"]);
            
            
            if (DT.Rows[0]["ChkRemitMethod"])
            {
                
                report_builder.selectAll("ddRemitMethod",true,true);
                chkAllRemitMethod.checked = true;
                                
            }
            else
            {
                report_builder.selectAll("ddRemitMethod",false,false);
                chkAllRemitMethod.checked = false;
                report_builder.checkddselected("ddRemitMethod",arrRemitMethod);
                
            }
            
            if (DT.Rows[0]["ChkBatchType"])
            {
                
                report_builder.selectAll("ddBatchTypes",true,true);
                chkAllBatchTypes.checked = true;
                               
            }
            else
            {
                
                
                report_builder.selectAll("ddBatchTypes",false,false);
                chkAllBatchTypes.checked = false;
                report_builder.checkddselected("ddBatchTypes",arrBatchType);
                

            }
            
            if (DT.Rows[0]["ChkRemittingBranch"])
            {
                report_builder.selectAll("ddOverseasBranches",true,true);
                chkAllOverseas.checked = true;
            }
            else
            {
                report_builder.selectAll("ddOverseasBranches",false,false);
                chkAllOverseas.checked = false;
                report_builder.checkddselected("ddOverseasBranches",arrRemittingBranch);
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
                report_builder.checkddselected("ddDomesticBranches",arrMaintainingBranch);
            }
            
            report_builder.ddloadFields(arrFields);
                        
        
            
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
                //return ((mo.length == 1) ? "0" + mo : "" + mo) + "/" + ((dd.length == 1) ? "0" + dd : "" + dd) + "/" + dt.getFullYear() + " " + ((shh.length == 1) ? "0" + shh : "" + shh) + ":" + ((mm.length == 1) ? "0" + mm : mm) + ":" + ((ss.length == 1) ? "0" + ss : ss) + " " + tt;
                return dt.getFullYear() + "-" + ((mo.length == 1) ? "0" + mo : "" + mo) + "-" +((dd.length == 1) ? "0" + dd : "" + dd);
            
            
        }
        
        this.setddselected = function(ddlist,textval)
        {
            var mySelect = document.getElementById(ddlist); 
            var myValue = textval;
            var indexcount = 0;
            for(i=0;i<mySelect.length;i++)
              {

                    if (report_builder.trim(mySelect.options[i].text)==report_builder.trim(myValue))
                    {
                     indexcount = i;
                     break;
                    }
              }
              
              mySelect.selectedIndex = indexcount;
              
        }
        
        
        this.checkddselected = function(ddlist,arrayselection)
        
        {
        
            var mySelect = document.getElementById(ddlist);
            var Arrayselected = arrayselection;
                    
            for (i=0;i<mySelect.length;i++)
                {
                    for (j=0;j<Arrayselected.length;j++)
                    {
                        if (report_builder.trim(mySelect.options[i].text) == report_builder.trim(Arrayselected[j]))
                        {
                            mySelect.options[i].selected = true;
                        }
                    }
                }

		        
         
        }
        
        this.ddloadFields = function(arrayselection)
       {
            var varFromBox = document.getElementById("ddFieldChoices");
            var varToBox = document.getElementById("ddFieldSelected");
            
            var Arrayselected = arrayselection;
                    
            for (i=0;i<varFromBox.length;i++)
                {
                    for (j=0;j<Arrayselected.length;j++)
                    {
                        if (report_builder.trim(varFromBox.options[i].text) == report_builder.trim(Arrayselected[j]))
                        {
                            varFromBox.options[i].selected = true;
                        }
                    }
                }
             
             if ((varFromBox != null) && (varToBox != null)) 
             { 
              
              
              while ( varFromBox.options.selectedIndex >= 0 ) 
              { 
               var newOption = new Option(); // Create a new instance of ListItem 

               newOption.text = varFromBox.options[varFromBox.options.selectedIndex].text; 
               newOption.value = varFromBox.options[varFromBox.options.selectedIndex].value; 
               varToBox.options[varToBox.length] = newOption; //Append the item in Target Listbox
               varFromBox.remove(varFromBox.options.selectedIndex); //Remove the item from Source Listbox 

              } 
             }
        }
        
        this.PreviewData = function()
        
        {
           
        this.ajax_grid = new grid_selector();        
        //this.grid_ajax_query = report_builder.getValue("txtSQLQuery");
        this.grid_ajax_query = "";
        //var indx = (parameter=='REMITTER' || parameter=='INTERNET') ? 3 : 0;
        if (this.grid_ajax_query != null)
        {
            //this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, "" );
            this.ajax_grid.open_modal_gridview(0, 5);
            
        }
        
            var btnExtract= document.getElementById("btnExtract");
            var btnPreview  = document.getElementById("btnPreview");
            var btnSave = document.getElementById("btnSave");
            
            btnExtract.disabled = true;
            btnPreview.disabled = true;
            btnSave.disabled = true;
        
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
        
