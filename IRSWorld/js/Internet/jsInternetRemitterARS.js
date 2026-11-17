//For Search Grid
var dbgrid = null;
var datatable = null;
var gv = null;

/************************/
/*** AJAX GRID        ***/
/************************/
//function execute_ajaxgridview_command(param)
//{
//    if (ars_client != null && ars_client != undefined)
//    {
//        ars_client.do_ajaxgridview(param);
//    }
//}

//function execute_ajx_command(param)
//{
//    if (ars_client != null && ars_client != undefined)
//    {
//        ars_client.do_ajaxsgridview_search(param);
//    }
//}

function ars_client()
{
    var _branchcode                 = "";
    
    //Remitter Info
    var _RemFName                   = "";
    var _RemMName                   = "";
    var _RemLName                   = "";
    var _RemNationality             = "";   //Nationality - txtNationality
    var _RemDateOfBirth             = "";   //Date of Birth - txtDateofBirth
    var _PlaceOfBirth               = "";   //Place of Birth - txtPlaceofBirth
    var _RemContactNo               = "";   //Contact No - txtContactNo
    var _RemOccupation              = "";   //Occupation - txtOccupation
    
    //REMITTER CURRENT RESIDENCE ADDRESS
    var _RemStreetCurr              = "";   //Street No./Name - txtRemAddress1
    var _RemStateCurr               = "";   //State - txtState
    var _RemCityCurr                = "";   //City - txtRemCity
    var _RemZipCodeCurr             = "";   //Zipcode / Postal Code - txtRemCurrZipCode
    var _RemCountryCurr             = 0;    //Country - ddRemCountry
    var _RemResidencePermanentCurr  = false;//Residence Address is Permanent Address - chkRemPermanentAdd

    //REMITTER MAILING ADDRESS
    var _RemStreetMail              = "";   //Street No. / Name - txtRemAddress1_Mail
    var _RemStatemail               = "";   //State - txtState_Mail
    var _RemCityMail                = "";   //City - txtRemCity_Mail
    var _RemZipCodeMail             = "";   //Zipcode / Postal Code - txtRemMailZipcode
    var _RemCountryMail             = 0;    //Country - ddRemCountry_Mail
    var _RemEmailMail               = "";   //Email Address - txtRemEmailAdd

    //REMITTER IDENTIFICATION
    var _RemIDType                  = 0;    //ID Type - ddRemIDType
    var _RemIDNo                    = "";   //ID No - txtRemIDNo
    var _RemIDPlaceOfIssuance       = "";   //ID Place of Issuance - txtRemPlaceOfIssuance
    var _RemIDExpiryDate            = "";   //Expiry Date - txtRemIDExpDate

    //BENEFICIARY INFORMATION
    var _BenLName                   = "";   //Last Name - txtBenefLName
    var _BenMName                   = "";   //Middle Name - txtBenefMName
    var _BenFName                   = "";   //First Name - txtBenefFName
    var _BenStreet                  = "";   //Street No. / Name - txtBenefStreet
    var _BenState                   = "";   //State / City - txtBenefCity
    var _BenZipCode                 = "";   //Zipcode / Postal Code - txtBeneZip
    var _BenProvince                = "";   //Province - txtBenefProvince
    var _BenDateOfBirth             = "";   //Date of Birth - txtBenefDOB
    var _BenEmailAddress            = "";   //Email Address - txtBenefEmailAdd
    var _BenTelNo                   = "";   //Telephone No - txtBenefTelNo
    
    var _RemitterBeneficiarySEQID   = "";
    var _RemDataList                = "";
    var _BenefDataList              = "";
    var _SaveBenefOnly              = null; //Null-> Not for saving. Viewing only | True-> Save Benef only | False-> Save Remitter and Benef
    
    var _RemCIFID                   = "";
    var _BenefCIFID                 = "";
    
    var _IsAddMode                  = true;
    //var _IsSearchMode               = null;
    var _RemitterSEQID              = "";
    var _BeneSEQID                  = "";
    
    
    var _RemCIFMasterSEQID          = "";
    var _RemCIFAddressHomeSEQID     = "";
    var _RemCIFAddressMailSEQID     = "";
    var _RemCIFContactsSEQID        = "";
    var _RemCIFAddressSEQID         = "";
    var _RemCIFOccupationSEQID      = "";
    var _BenefCIFMasterSEQID        = "";
    var _BenefCIFAddressSEQID       = "";
    var _BenefCIFContactsSEQID      = "";
    
    var _PNBAccountCurrency         = "";
    
   
    

    /************************/
    /*** INITIALIZE PAGE  ***/
    /************************/

    this.initialize = function()
    {
        dbgrid = null;
        gv = new datagrid();
        this.getBranchCode();
        this.getCountry();
        this.getIDType();
        this.getCitizenship();
        this.getProduct();
        this.checkIfFromTransaction();
        this.disableFields("Remitter", true);
        this.disableFields("Beneficiary", true);
        this.disableFields("Implementation", true);
        this.disableFields("Products", true);
        
        ars_client._IsAddMode = true;
        ars_client._BenZipCode = "";
    }
    
    this.setRemBenVal = function(param)
    {
        ars_client._RemitterBeneficiarySEQID = param;
        
        if (param != null && param.value != "")
        {
            RemBenVal= param.split('~');
            
            ars_client._RemitterSEQID = RemBenVal[0];
            ars_client._BeneSEQID = RemBenVal[1];
            ars_client._RemCIFID = RemBenVal[2];
            ars_client._BenefCIFID = RemBenVal[3];
        }
    }
    
    this.clearGlobalVariable    = function()
    {
        ars_client._RemFName                   = "";
        ars_client._RemMName                   = "";
        ars_client._RemLName                   = "";
        ars_client._RemNationality             = "";   //Nationality - txtNationality
        ars_client._RemDateOfBirth             = "";   //Date of Birth - txtDateofBirth
        ars_client._PlaceOfBirth               = "";   //Place of Birth - txtPlaceofBirth
        ars_client._RemContactNo               = "";   //Contact No - txtContactNo
        ars_client._RemOccupation              = "";   //Occupation - txtOccupation
        
        //REMITTER CURRENT RESIDENCE ADDRESS
        ars_client._RemStreetCurr              = "";   //Street No./Name - txtRemAddress1
        ars_client._RemStateCurr               = "";   //State - txtState
        ars_client._RemCityCurr                = "";   //City - txtRemCity
        ars_client._RemZipCodeCurr             = "";   //Zipcode / Postal Code - txtRemCurrZipCode
        ars_client._RemCountryCurr             = 0;    //Country - ddRemCountry
        ars_client._RemResidencePermanentCurr  = false;//Residence Address is Permanent Address - chkRemPermanentAdd

        //REMITTER MAILING ADDRESS
        ars_client._RemStreetMail              = "";   //Street No. / Name - txtRemAddress1_Mail
        ars_client._RemStatemail               = "";   //State - txtState_Mail
        ars_client._RemCityMail                = "";   //City - txtRemCity_Mail
        ars_client._RemZipCodeMail             = "";   //Zipcode / Postal Code - txtRemMailZipcode
        ars_client._RemCountryMail             = 0;    //Country - ddRemCountry_Mail
        ars_client._RemEmailMail               = "";   //Email Address - txtRemEmailAdd

        //REMITTER IDENTIFICATION
        ars_client._RemIDType                  = 0;    //ID Type - ddRemIDType
        ars_client._RemIDNo                    = "";   //ID No - txtRemIDNo
        ars_client._RemIDPlaceOfIssuance       = "";   //ID Place of Issuance - txtRemPlaceOfIssuance
        ars_client._RemIDExpiryDate            = "";   //Expiry Date - txtRemIDExpDate

        //BENEFICIARY INFORMATION
        ars_client._BenLName                   = "";   //Last Name - txtBenefLName
        ars_client._BenMName                   = "";   //Middle Name - txtBenefMName
        ars_client._BenFName                   = "";   //First Name - txtBenefFName
        ars_client._BenStreet                  = "";   //Street No. / Name - txtBenefStreet
        ars_client._BenState                   = "";   //State / City - txtBenefCity
        ars_client._BenZipCode                 = "";   //Zipcode / Postal Code - txtBeneZip
        ars_client._BenProvince                = "";   //Province - txtBenefProvince
        ars_client._BenDateOfBirth             = "";   //Date of Birth - txtBenefDOB
        ars_client._BenEmailAddress            = "";   //Email Address - txtBenefEmailAdd
        ars_client._BenTelNo                   = "";   //Telephone No - txtBenefTelNo
    }
    
    /************************/
    /*** COMMON FUNCTIONS ***/
    /************************/
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            ars_client.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
    }
    
    this.isNumberKey = function(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

        return true;
    }
    
    this.showAlert = function(message)
    {
        alert(message);
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
            
            select.options[0] = new Option("", "");
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && ars_client.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && ars_client.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]] + ((delimeter == "") ? " • " : delimeter);
                        }
                        else
                        {
                            arrValue = arrValue + ((delimeter == "") ? " • " : delimeter);
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
            if (selected != '')
            {
                ars_client.getDDL(element, selected, true);
            }
        }
    }
    
    this.getDDL = function(element, selected, isValue)
    {
        var select = document.getElementById(element);
        document.getElementById(selected).value = (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
    }

    this.getDDLValue = function(element, isValue)
    {
        var select = document.getElementById(element);
        return (isValue == true) ? select.options[select.selectedIndex].value : select.options[select.selectedIndex].text;
    }
    
    this.findDDL = function(element, findvalue, selected, isvalue)
    {
        var select = document.getElementById(element);
        for (var i = 0; i <= (select.length - 1); i++)
        {
            if (isvalue == true)
            {
                if (select.options[i].value == findvalue)
                {
                    select.selectedIndex = i;
                    if (selected != "")
                    {
                        ars_client.getDDL(element, selected, true);
                    }
                    break;
                }
            }
            else
            {
                if (select.options[i].text == findvalue)
                {
                    select.selectedIndex = i;
                    if (selected != "")
                    {
                        ars_client.getDDL(element, selected, true);
                    }
                    break;
                }
            }
        }
    }
    
    this.removeDDL = function(element, findvalue, isvalue)
    {
        var select = document.getElementById(element);
        for (var i = 0; i <= (select.length - 1); i++)
        {
            if (isvalue == true)
            {
                if (select.options[i].value == findvalue)
                {
                    select.remove(i);
                    break;
                }
            }
            else
            {
                if (select.options[i].text == findvalue)
                {
                    select.remove(i);
                    break;
                }
            }
        }
    }
    
    this.clearDDL = function(element)
    {
        var select = document.getElementById(element);
        select.options.length = 0;
    }

    this.showControl = function(element, isvisible)
    {
        if (isvisible == true)
        {
            document.getElementById(element).style.display = "";
        }
        else
        {
            document.getElementById(element).style.display = "none";
        }
    }
    
    this.addValue = function(value, element)
    {
        document.getElementById(element).value = document.getElementById(element).value + value;
    }

    this.getValue = function(element, isspan)
    {
        return (isspan == true) ? document.getElementById(element).innerHTML : document.getElementById(element).value;
    }

    this.enterValue = function(value, element, isspan)
    {
        (isspan == true) ? document.getElementById(element).innerHTML = value : document.getElementById(element).value = value;
    }

    this.removeValue = function(element, isspan)
    {
        (isspan == true) ? document.getElementById(element).innerHTML = "" : document.getElementById(element).value = "";
    }
    
    this.showFocus = function(element)
    {
        document.getElementById(element).focus();
    }

    this.edElement = function(element, isdisable)
    {
        if (isdisable == true)
        {
            document.getElementById(element).disabled = "disabled";
        }
        else
        {
            document.getElementById(element).disabled = "";
        }
    }
    
    this.roElement = function(element, isdisable)
    {
        if (isdisable == true)
        {
            document.getElementById(element).readOnly = true;
        }
        else
        {
            document.getElementById(element).readOnly = false;
        }
    }
    
    this.showLoading = function(message, ishide)
    {
        if (ishide == true)
        {
            document.getElementById("lblLoadingMsg").innerHTML = "";
            ars_client.hidePanel("divPopProcess");
        }
        else
        {
            new common_method().to_show_loading_message('divLoadingMsg');
            document.getElementById("lblLoadingMsg").innerHTML = message;
            ars_client.showPanel("divPopProcess");
        }
    }
    
    /************************/
    /*** MODAL PANEL      ***/
    /************************/
    
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        ars_client.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {
        ars_client.showControl(divName, false);
        divmask.hide();
    }
    
    this.hide = function()
    {
        this.waitifrm.style.display = this.divobj.style.display = "none";
    }
    
    /************************/
    /*** AJAX GRID SEARCH ***/
    /************************/
    
    this.do_ajaxgridview_search = function(parameter)
    {
        if (parameter != null && parameter != undefined)
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    if (command_param[1] != '0')
                    {
                        this.ajax_grid.process_ajax(parameter);
                        ars_client.GetZipcodeValue(command_param[1]);
                    }
                    else
                    {
                        ars_client.showAlert("Unable to select. Please refresh the page and try again.");
                    }
                    return;
                }
                else
                {
                    this.ajax_grid.process_ajax(parameter);
                    return;
                }
            }
        }
    }
    
    this.load_grid_viewer_zipcode = function()
    {
        this.process_grid_viewer_search_zipcode("ajx_loading");
    }
    
    this.process_grid_viewer_search_zipcode = function(parameter)
    {
        this.ajax_grid = new grid_selector();
        this.grid_ajax_query = "sp_int_search_zipcode_filtered~2~";
        this.ajax_grid.open_selection_grid(this.grid_ajax_page, this.grid_ajax_query, 0, ars_client._seq_id); 
    }
    
    /************************/
    /*** AJAX GRID SIMPLE ***/
    /************************/
    
//    this.do_ajaxgridview = function(parameter)
//    {
//        if (parameter != null && parameter != undefined)
//        {
//            var command_param = parameter.split("$");
//            if (command_param.length > 0)
//            {
//                if (command_param[0] == "ajx_selected")
//                {
//                    if (command_param[1] != '0')
//                    {
//                        //ars_client.showAlert(command_param[1]);
//                        ars_client.setRemBenVal(command_param[1]);
//                    }
//                    else
//                    {
//                        ars_client.showAlert("Unable to select. Please try re-loading again.");
//                    }
//                    return;
//                }
//                this.process_grid_viewer(parameter);
//            }
//        }
//    }

    this.load_grid_viewer = function()
    {
        ars_client.loadRecord();
        return;
        //this.process_grid_viewer("ajx_loading");
    }

//    this.process_grid_viewer = function(parameter)
//    {
//        //ars_client.showPanel("divSearch");
//        ars_client.showControl("divSearch", true);
//        var sql = "asp_Search_CIF~" + 
//                    ars_client.getValue("txtSLastName", false) + "~" + 
//                    ars_client.getValue("txtSFirstName", false) + "~" + 
//                    ars_client.getValue("txtSMiddleName", false) + "~" +
//                    ars_client._branchCode;

//        this.grid = new grid_viewer();
//        this.grid.sql_source        = sql;
//        this.grid.div_container     = document.getElementById("divSearchResults");
//        this.grid.datarow_column    = 0;
//        this.grid.page_size         = 8;
//        this.grid.grd_header_back_color = "#26afe5";
//        this.grid.grd_header_fore_color = "#FFFFFF";
//        this.grid.grd_customed_date = false;
//        this.grid.process_ajax(parameter);
//        this.grid                   = null;
//        document.getElementById("divLoading").innerHTML = "";
//        ars_client.showControl("trSearchResults", true);
//    }
    
    /************************/
    /*** FUNCTIONS        ***/
    /************************/
    
    this.setDate = function(date, format, delimeter, addyear, isexpiry)
    {
        var arrSplit
        var year;
        var month;
        var day;
        var isDash;
        var ret;
        
        if (date.indexOf("-") >= 0)
        {
            arrSplit = date.split('-');
            year = arrSplit[0];
            month = arrSplit[1];
            day = arrSplit[2];
            isDash = true;
        }
        else
        {
            arrSplit = date.split('/');
            year = arrSplit[2];
            month = arrSplit[1];
            day = arrSplit[0];
            isDash = false;
        }
        
        if (month.toString().length == 1)
        {
            month = "0" + month;
        }
        if (day.toString().length == 1)
        {
            day = "0" + day;
        }
        
        if (addyear)
        {
            year = Number(year) + 1;
        }
        if (isexpiry)
        {
            day = Number(day) - 1;
        }
        
        switch (format)
        {
            case "mmddyyyy":
                ret = month + delimeter + day + delimeter + year;
                break;
            
            case "ddmmyyyy":
                ret = day + delimeter + month + delimeter + year;
                break;
                
            case "yyyymmdd":
                ret = year + delimeter + month + delimeter + day;
                break;
        }
        return ret;
    }
    
    /************************/
    /*** AJAX CALLBACK    ***/
    /************************/
    
    this.getBranchCode = function()
    {
        ars_client.ajaxTimeout();
        ars_client.showLoading("Retrieving Branch Code. Please wait...", false);
        return IRSWorld.Internet.InternetRemitterARS.getBranchCode(this.getBranchCode_callback);
    }
    
    this.getBranchCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client._branchCode = response;
        }
        ars_client.showLoading("", true);
    }
    
    this.getCountry = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getLookup("Country", this.getCountry_callback);
    }
    
    this.getCountry_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddRemCountry", "FldDescription", "FldValue", "", "");
            ars_client.fillDDL(response, "ddRemCountry_Mail", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.getIDType = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getSubLookup("IDType", this.getIDType_callback);
    }
    
    this.getIDType_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddRemIDType", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.getCitizenship = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getSubLookup("Citizenship", this.getCitizenship_callback);
    }
    
    this.getCitizenship_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddRemCitizenship", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.getProduct = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getModeOfRemittance(this.getProduct_callback);
    }
    
    this.getProduct_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddProduct", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.checkIfFromTransaction = function()
    {
        return IRSWorld.Internet.InternetRemitterARS.getCIFInformation(this.getRemitterAndBeneInfo_callback);
    }
    
    this.GetZipcodeValue = function(zipcode)
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.Get_Zipcode(zipcode, this.GetZipcodeValue_callback);
    }
    
    this.GetZipcodeValue_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.enterValue(response, "txtRemCurrZipCode", false);
        }
    }
    
    this.getGFCMode = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getLookup("GFCImplemMode", this.getGFCMode_callback);
    }
    
    this.getGFCMode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddGFCMode", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.getGFCcurr = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getsubLookup("Currency", this.getGFCcurr_callback);
    }
    
    this.getGFCcurr_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddGFCCurrency", "FldDescription", "FldValue", "", "");
        }
    }
   
    this.getOtherBankcurr = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getsubLookup("Currency", this.getOtherBankcurr_callback);
    }
    
    this.getOtherBankcurr_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddCredittoOtherBankCurr", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.getOtherBank = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getLookup("PDDTSBank", this.getOtherBank_callback);
    }
    
    this.getOtherBank_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddOtherBankName", "FldDescription", "FldValue", "", "");
        }
    }
    
    this.getApasCurr = function()
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getsubLookup("Currency", this.getApasCurr_callback);
    }
    
    this.getApasCurr_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddCredittoAPASCurr", "FldDescription", "FldValue", "", "");
        }
    }
    
    
    this.hideSearch = function(isHide, btn)
    {
        if (isHide == true)
        {
            ars_client.showControl("divmain", true);
            ars_client.showControl("divSearch", false);
            
            if(btn.id == "btnAddNewBenef")
            {
                ars_client.disableFields("Remitter", true);
                ars_client.disableFields("Beneficiary", false);
                ars_client.disableFields("Products", true);
                ars_client.disableFields("Implementation", true);
                ars_client._SaveBenefOnly = true;
            }
            if (btn.id == "btnAddRemCIF")
            {
                ars_client.disableFields("Remitter", false);
                ars_client.disableFields("Beneficiary", false);
                ars_client.disableFields("Products", true);
                ars_client.disableFields("Implementation", true);
                
                ars_client._SaveBenefOnly = false;
            }
            if (btn=="ViewRemitterAndBene")
            {
                //Existing Remitter and Bene
                //View Info. May choose a product (viewing only)
                ars_client.disableFields("Remitter", true);
                ars_client.disableFields("Beneficiary", true);
                ars_client.disableFields("Products", false);
                ars_client.disableFields("Implementation", true);
                ars_client.edElement('btnSaveBenef', true);
                ars_client._SaveBenefOnly = false;
            }
        }
        else
        {
            ars_client.showControl("divSearch", true);
            ars_client.showControl("divmain", false);
        }
    }
    
    this.getRemitterAndBeneInfo = function(RemCIFID, BenCIFID)//param)
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.Get_CIF_Details(RemCIFID, BenCIFID, this.getRemitterAndBeneInfo_callback);
    }
    
    this.getRemitterAndBeneInfo_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.hideSearch(true, "ViewRemitterAndBene");
            ars_client._SaveBenefOnly = false;
            ars_client._IsAddMode = false;
            
            //Assign value to global variable
            ars_client._RemFName = response.Rows[0]["FIRSTNAME"];
            ars_client._RemMName = response.Rows[0]["MIDDLENAME"];
            ars_client._RemLName = response.Rows[0]["LASTNAME"];
        
           //Remitter
            ars_client.enterValue(response.Rows[0]["LASTNAME"], "txtRemLName", false);
            ars_client.enterValue(response.Rows[0]["MIDDLENAME"], "txtRemMName", false);
            ars_client.enterValue(response.Rows[0]["FIRSTNAME"], "txtRemFName", false);
            
            ars_client.findDDL("ddRemCitizenship", response.Rows[0]["NATIONALITY"], "", true);
            ars_client.enterValue(response.Rows[0]["DATEOFBIRTH"], "txtDateofBirth", false);
            ars_client.enterValue(response.Rows[0]["PLACEOFBIRTH"], "txtPlaceofBirth", false);
            ars_client.enterValue(response.Rows[0]["CONTACTNO"], "txtContactNo", false);
            ars_client.enterValue(response.Rows[0]["OCCUPATION"], "txtOccupation", false);
            
            //Current Residence Address
            ars_client.enterValue(response.Rows[0]["STREETNO"], "txtRemAddress1", false);
            ars_client.enterValue(response.Rows[0]["STATE"], "txtState", false);
            ars_client.enterValue(response.Rows[0]["CITY"], "txtRemCity", false);
            ars_client.enterValue(response.Rows[0]["REMZIPCODE"], "txtRemCurrZipCode", false);
            ars_client.findDDL("ddRemCountry", response.Rows[0]["REMCOUNTRY"], "", true);
            ars_client.enterValue(response.Rows[0]["Permanent_Address"], "chkRemPermanentAdd", false);
            
            //Mailing Address
            ars_client.enterValue(response.Rows[0]["STREETNO_MAIL"], "txtRemAddress1_Mail", false);
            ars_client.enterValue(response.Rows[0]["STATE_MAIL"], "txtState_Mail", false);
            ars_client.enterValue(response.Rows[0]["CITY_MAIL"], "txtRemCity_Mail", false);
            ars_client.enterValue(response.Rows[0]["REMZIPCODE_MAIL"], "txtRemMailZipcode", false);
            ars_client.enterValue(response.Rows[0]["EMAILADDRESS"], "txtRemEmailAdd", false);            
            ars_client.findDDL("ddRemCountry_Mail", response.Rows[0]["REMCOUNTRY_MAIL"], "", true);
                        
            //Identification
            ars_client.findDDL("ddRemIDType", response.Rows[0]["IDTYPE"], "", true);
            ars_client.enterValue(response.Rows[0]["IDNUMBER"], "txtRemIDNo", false);
            ars_client.enterValue(response.Rows[0]["STATEOFISSUANCE"], "txtRemPlaceOfIssuance", false);
            ars_client.enterValue(response.Rows[0]["EXPIRYDATE"], "txtRemIDExpDate", false);
            
            
            //BENEFICIARY
            ars_client.enterValue(response.Rows[0]["BEN_LASTNAME"], "txtBenefLName", false);
            ars_client.enterValue(response.Rows[0]["BEN_MIDDLENAME"], "txtBenefMName", false);
            ars_client.enterValue(response.Rows[0]["BEN_FIRSTNAME"], "txtBenefFName", false);
            ars_client.enterValue(response.Rows[0]["BEN_STREET"], "txtBenefStreet", false);
            ars_client.enterValue(response.Rows[0]["BEN_BIRTHDATE"], "txtBenefDOB", false);
            ars_client.enterValue(response.Rows[0]["BEN_EMAIL"], "txtBenefEmailAdd", false);
            ars_client.enterValue(response.Rows[0]["BEN_CONTACT"], "txtBenefTelNo", false);
            
            if (response.Rows[0]["BEN_ZIPCODE"] != null && response.Rows[0]["BEN_ZIPCODE"] != undefined)
            {
                var concatZip = response.Rows[0]["BEN_ZIPCODE"];
                var arrZip = concatZip.split('~');
                ars_client._BenZipCode = arrZip[0];
                ars_client.enterValue(arrZip[1], "txtBenefProvince", false);
                ars_client.enterValue(arrZip[2], "txtBenefCity", false);
                ars_client.enterValue(arrZip[3], "txtBeneZip", false);
            }
            
            //Populate Product
            var prodcode = response.Rows[0]["BEN_PRODUCT_ID"];
            var targetcurr = response.Rows[0]["BEN_ACCTCURR"];
            var accttype = '';//response.Rows[0][""];
            var acctno = response.Rows[0]["BEN_ACCOUNTNO"];
            var acctstat = '';//response.Rows[0][""];
            var accountBranchCode = response.Rows[0]["BEN_ACCTBRANCHNAME"];
            
            ars_client.populateProduct(prodcode, targetcurr, accttype, acctno, acctstat, accountBranchCode);
            
            //Assign SEQ_ID value
            _RemCIFMasterSEQID          = response.Rows[0]["RemCIFMasterID"];
            _RemCIFAddressHomeSEQID     = response.Rows[0]["RemCIFAddressAID"];
            _RemCIFAddressMailSEQID     = response.Rows[0]["RemCIFAddressBID"];
            _RemCIFContactsSEQID        = response.Rows[0]["RemCIFContactsID"];
            _RemCIFIDsSEQID             = response.Rows[0]["RemCIFIDsID"];
            _RemCIFOccupationSEQID      = response.Rows[0]["RemCIFOccupationID"];
            _BenefCIFMasterSEQID        = response.Rows[0]["BenefCIFMasterID"];
            _BenefCIFAddressSEQID       = response.Rows[0]["BenefCIFAddressID"];
            _BenefCIFContactsSEQID      = response.Rows[0]["BenefCIFContactsID"];
            
            ars_client.hideSearch(true, 'ViewRemitterAndBene');
        }
        else
        {
            //ars_client.showAlert("No record(s) found.");
        }
        
        ars_client.disableFields("Remitter", false);
        ars_client.disableFields("Beneficiary", false);
        ars_client.disableFields("Implementation", true);
        
        ars_client.edElement('txtBenefCity', true);
        ars_client.edElement('txtBenefProvince', true);
        
        ars_client.edElement('txtRemLName', true);
        ars_client.edElement('txtRemMName', true);
        ars_client.edElement('txtRemFName', true);
        ars_client.edElement('btnSaveBenef', true);
    }
    
    this.getSelectedGridRow = function()
    {
        var selectedGridRow = null;
        selectedGridRow = gv.row.selected_data_row;
        if (selectedGridRow > -1)
        {
            var ID = 'tblReportLookup_txt_Column1_' + selectedGridRow;
            var IDs = document.getElementById(ID).value;
            ars_client.setRemBenVal(IDs);
            var SEQID = IDs.split('~');
            var RemSEQID = SEQID[0];
            var BenSEQID = SEQID[1];
                
            return;
        }
        return;
    }
    
    this.getRemitterInfo = function()
    {
        ars_client.getSelectedGridRow();
        if (ars_client._RemitterBeneficiarySEQID != "" && ars_client._RemitterBeneficiarySEQID != null)
        {
            ars_client.hideSearch(true, "btnAddNewBenef");
            ars_client.ajaxTimeout();
            
            return IRSWorld.Internet.InternetRemitterARS.Get_CIF_Details(ars_client._RemCIFID, ars_client._BenefCIFID, this.getRemitterInfo_callback);
        }
        else
        {
            ars_client.showAlert('Please select a record.\n');
        }
    }
    
    this.getRemitterInfo_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            //Assign value to global variable
            ars_client._RemFName = response.Rows[0]["FIRSTNAME"];
            ars_client._RemMName = response.Rows[0]["MIDDLENAME"];
            ars_client._RemLName = response.Rows[0]["LASTNAME"];
        
           //Remitter
            ars_client.enterValue(response.Rows[0]["LASTNAME"], "txtRemLName", false);
            ars_client.enterValue(response.Rows[0]["MIDDLENAME"], "txtRemMName", false);
            ars_client.enterValue(response.Rows[0]["FIRSTNAME"], "txtRemFName", false);
            //ars_client.enterValue(response.Rows[0]["NATIONALITY"], "txtNationality", false);
            ars_client.findDDL("ddRemCitizenship", response.Rows[0]["NATIONALITY"], "", true);
            ars_client.enterValue(response.Rows[0]["DATEOFBIRTH"], "txtDateofBirth", false);
            ars_client.enterValue(response.Rows[0]["PLACEOFBIRTH"], "txtPlaceofBirth", false);
            ars_client.enterValue(response.Rows[0]["CONTACTNO"], "txtContactNo", false);
            ars_client.enterValue(response.Rows[0]["OCCUPATION"], "txtOccupation", false);
            
            //Current Residence Address
            ars_client.enterValue(response.Rows[0]["STREETNO"], "txtRemAddress1", false);
            ars_client.enterValue(response.Rows[0]["STATE"], "txtState", false);
            ars_client.enterValue(response.Rows[0]["CITY"], "txtRemCity", false);
            ars_client.enterValue(response.Rows[0]["REMZIPCODE"], "txtRemCurrZipCode", false);
            ars_client.findDDL("ddRemCountry", response.Rows[0]["REMCOUNTRY"], "", true);
            ars_client.enterValue(response.Rows[0]["Permanent_Address"], "chkRemPermanentAdd", false);
            
            //Mailing Address
            ars_client.enterValue(response.Rows[0]["STREETNO_MAIL"], "txtRemAddress1_Mail", false);
            ars_client.enterValue(response.Rows[0]["STATE_MAIL"], "txtState_Mail", false);
            ars_client.enterValue(response.Rows[0]["CITY_MAIL"], "txtRemCity_Mail", false);
            ars_client.enterValue(response.Rows[0]["REMZIPCODE_MAIL"], "txtRemMailZipcode", false);
            ars_client.enterValue(response.Rows[0]["EMAILADDRESS"], "txtRemEmailAdd", false);            
            ars_client.findDDL("ddRemCountry_Mail", response.Rows[0]["REMCOUNTRY_MAIL"], "", true);
                        
            //Identification
            ars_client.findDDL("ddRemIDType", response.Rows[0]["IDTYPE"], "", true);
            ars_client.enterValue(response.Rows[0]["IDNUMBER"], "txtRemIDNo", false);
            ars_client.enterValue(response.Rows[0]["STATEOFISSUANCE"], "txtRemPlaceOfIssuance", false);
            ars_client.enterValue(response.Rows[0]["EXPIRYDATE"], "txtRemIDExpDate", false);
        }
        else
        {
            //ars_client.showAlert("No record(s) found.");
        }
            ars_client.disableFields("Remitter", true);
            ars_client.disableFields("Beneficiary", false);
            
            ars_client.validateElement("divBeneficiary", "input", "text", "clear", "", "");
            ars_client.validateElement("divBeneficiary", "select", "select", "clear", "", "");
            ars_client.edElement("btnSaveBenef", true);
    }
    
    this.disableFields = function(category, disable)
    {
        //Categories: Remitter / Beneficiary / Implementation / Products
        if (category == "Remitter")
        {
            ars_client.validateElement("divRemitter", "input", "text", "disable", disable, "");
            ars_client.validateElement("divRemitter", "input", "button", "disable", disable, "");
            ars_client.validateElement("divRemitter", "select", "select-one", "disable", disable, "");
        }
        else if (category == "Beneficiary")
        {
            ars_client.validateElement("divBeneficiary", "input", "text", "disable", disable, "");
            ars_client.validateElement("divBeneficiary", "input", "button", "disable", disable, "");
            ars_client.validateElement("divBeneficiary", "select", "select-one", "disable", disable, "");
            ars_client.edElement('btnSaveBenef', true);
        }
        else if (category == "Implementation")
        {
            ars_client.validateElement("divImplementation", "input", "text", "disable", disable, "");
            ars_client.validateElement("divImplementation", "input", "button", "disable", disable, "");
            ars_client.validateElement("divImplementation", "select", "select-one", "disable", disable, "");
        }
        else if (category == "Products")
        {
            ars_client.validateElement("divProducts", "input", "text", "disable", disable, "");
            ars_client.validateElement("divProducts", "input", "button", "disable", disable, "");
            ars_client.validateElement("divProducts", "select", "select-one", "disable", disable, "");
        }
    }
    
    this.validateDate = function()
    {
        var v = document.getElementById('txtDateofBirth').value;
        //mm  
        if (v.match (/^\d{2}$/) !== null)
        {
            document.getElementById('txtDateofBirth').value = v + '/';
        }
        //dd
        else if (v.match(/^\d{2}\/\d{2}$/) !== null)
        {
            document.getElementById('txtDateofBirth').value = v + '/';
        }
    } 
    
    this.validateElement = function(div, tag, type, condition, yesno, element)
    {
        var inputs = document.getElementById(div).getElementsByTagName(tag);
        var inputName;

        for (i = 0; i <= (inputs.length - 1); i++)
        {
            inputName = inputs[i].id;
            if (inputs[i].type == type)
            {
                switch (condition)
                {
                    case "clear":
                        if (element != "")
                        {
                            ars_client.removeValue(element, false);
                            i = inputs.length;
                        }
                        else
                        {
                            ars_client.removeValue(inputName, false);
                        }
                        break;

                    case "readonly":
                        if (element != "")
                        {
                            ars_client.roElement(element, yesno);
                            i = inputs.length;
                        }
                        else
                        {
                            ars_client.roElement(inputName, yesno);
                        }
                        break;

                    case "disable":
                        if (element != "")
                        {
                            ars_client.edElement(element, yesno);
                            i = inputs.length;
                        }
                        else
                        {
                            ars_client.edElement(inputName, yesno);
                        }
                        break;
                }
            }
        }
    }
    
    this.check_CIF_Watchlist = function()
    {
//        //TESTING PURPOSES (REPLICATE SAVING BEHAVIOR)
//        ars_client.showAlert("Remitter and Beneficiary successfully saved.");
//        ars_client.disableFields('Products', false);
//        ars_client.disableFields('Implementation', false);
//        ars_client.disableFields('Remitter', true);
//        ars_client.disableFields('Beneficiary', true);
//        ars_client.edElement('btnSaveBenef', false);
//        return;
//        //END TEST
        
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.check_and_get_hitlist(ars_client._RemFName, ars_client._RemLName, ars_client._RemMName, this.check_CIF_Watchlist_callback);
//        return IRSWorld.Internet.InternetRemitterARS.check_and_get_hitlist("AJ", "OBSUM", "C", this.check_CIF_Watchlist_callback);
    } 
    
    this.check_CIF_Watchlist_callback = function(result)
    {
        if (result.value != "" && result != null && result != undefined)
        {
            ars_client.showAlert('Remitter Name: ' +  ((ars_client._RemFName == null) ? "" : ars_client._RemFName) + ' ' + ((ars_client._RemMName == null) ? "" : ars_client._RemMName) + ' ' + ((ars_client._RemLName == null) ? "" : ars_client._RemLName) + ' is in the WatchList');
        }

        ars_client.saveInfo();
    }
    
    this.saveInfo = function()
    {
        ars_client.ajaxTimeout();
        var val = ars_client.setValue();
        if (val != "" && val != null)
        {
            return IRSWorld.Internet.InternetRemitterARS.saveInfo(val, ars_client._SaveBenefOnly, ars_client._IsAddMode, this.saveInfo_callback);
        }
    } 
    
    this.saveInfo_callback = function(res)
    {
        var result = res.value;
        if (result != "" && result != null)
        {
         //   ars_client.showAlert(result);
            var val= result.split('|');
            
            if (val[0] == "OK")
            {
                //if SAVING is successful,
                //show the products
                ars_client.showAlert("Remitter and Beneficiary successfully saved.");
                ars_client.disableFields('Products', false);
                ars_client.disableFields('Implementation', false);
                ars_client.disableFields('Remitter', true);
                ars_client.disableFields('Beneficiary', true);
                ars_client.edElement('btnSaveBenef', false);
            }
        }
    }
    
    this.setValue = function() 
    {
        var dataList = "";
        //Save Bene Only OR Save Remitter and Bene
        var IsSaveBeneOnly = ars_client._SaveBenefOnly;
        
        //BENEFICIARY INFO
        dataList += "B CIF Type~1^";//CIF Type
        dataList += "B Last Name~" + ars_client.getValue("txtBenefLName", false) + "^"; //Ben LastName
        dataList += "B First Name~" + ars_client.getValue("txtBenefFName", false) + "^"; //Ben FirstName
        dataList += "B Middle Name~" + ars_client.getValue("txtBenefMName", false) + "^"; //Ben MiddleName
        dataList += "B Address1~^"; //Address 1
        dataList += "B Address2~" + ars_client.getValue("txtBenefStreet", false) + "^"; //Address 2
        dataList += "B Address3~^"; //Address 3
        dataList += "B Address4~" + ars_client.getValue("txtBenefCity", false) + "^"; //Address 4
        dataList += "B Address5~" + ars_client.getValue("txtBenefProvince", false) + "^"; //Address 5
        dataList += "B Address6~^"; //Address 6
        dataList += "B CountryID~^"; //Country ID
        dataList += "B Date of Birth~" + ars_client.getValue("txtBenefDOB", false) + "^"; //BirthDate
        dataList += "B Place of Birth~^"; //BirthPlace
        dataList += "B Email~" + ars_client.getValue("txtBenefEmailAdd", false) + "^"; //EmailAdd
        dataList += "B Contact No~" + ars_client.getValue("txtBenefTelNo", false) + "^"; //Contact No
        dataList += "B Citizenship~^";

        dataList += "B Zip Code~" + ars_client._BenZipCode + "^";//ZipCode
        dataList += "B CIF Class~2"; //CIFClass
            
        if (IsSaveBeneOnly == false) //save beneficiary and remitter
        {
            //REMITTER INFO
            dataList += "^R CIF Type~1^";//CIF Type
            dataList += "R Last Name~" + ars_client.getValue("txtRemLName", false) + "^"; //Rem LastName
            dataList += "R First Name~" + ars_client.getValue("txtRemFName", false) + "^"; //Rem FirstName
            dataList += "R Middle Name~" + ars_client.getValue("txtRemMName", false) + "^"; //Rem MiddleName
            dataList += "R Citizenship~" + ars_client.getValue("ddRemCitizenship", false) + "^"; //Citizenship
            dataList += "R Date of Birth~" + ars_client.getValue("txtDateofBirth", false) + "^"; //BirthDate
            dataList += "R Place of Birth~" + ars_client.getValue("txtPlaceofBirth", false) + "^"; //BirthPlace
            dataList += "R Contact No~" + ars_client.getValue("txtContactNo", false) + "^";//Contact No
            dataList += "R Occupation~" + ars_client.getValue("txtOccupation", false) + "^";//Occupation
            dataList += "R CIF Class~1^"; //CIFClass
            dataList += "R Address1 Home~^";
            dataList += "R Address2 Home~" + ars_client.getValue("txtRemAddress1", false) + "^"; //address2
            dataList += "R Address3 Home~^";
            dataList += "R Address4 Home~" + ars_client.getValue("txtRemCity", false) + "^"; //address4
            dataList += "R Address5 Home~" + ars_client.getValue("txtState", false) + "^"; //address5
            dataList += "R Address6 Home~^";
            dataList += "R Zip Code Home~" + ars_client.getValue("txtRemCurrZipCode", false) + "^"; //Zip Code
            dataList += "R CountryID Home~" + ars_client.getValue("ddRemCountry", false) + "^"; //Country
            dataList += "R Address1 Mail~^";
            dataList += "R Address2 Mail~" + ars_client.getValue("txtRemAddress1_Mail", false) + "^"; //address2
            dataList += "R Address3 Mail~^";
            dataList += "R Address4 Mail~" + ars_client.getValue("txtRemCity_Mail", false) + "^"; //address4
            dataList += "R Address5 Mail~" + ars_client.getValue("txtState_Mail", false) + "^"; //address5
            dataList += "R Address6 Mail~^";
            dataList += "R Zip Code Mail~" + ars_client.getValue("txtRemMailZipcode", false) + "^"; //ZipCode
            dataList += "R CountryID Mail~" + ars_client.getValue("ddRemCountry_Mail", false) + "^"; 
            dataList += "R Email~" + ars_client.getValue("txtRemEmailAdd", false) + "^"; 
            dataList += "R ID Type~" + ars_client.getValue("ddRemIDType", false) + "^"; 
            dataList += "R ID No~" + ars_client.getValue("txtRemIDNo", false) + "^"; 
            dataList += "R ID Place of Issuance~" + ars_client.getValue("txtRemPlaceOfIssuance", false) + "^"; 
            dataList += "R ID Expiration Date~" + ars_client.getValue("txtRemIDExpDate", false); 
            
            var isPermanentAdd = false;
            if (ars_client.getValue("chkRemPermanentAdd", false) == true)
            {
                isPermanentAdd = true;
            }
           
            dataList += "^R Preferred Residence~" + isPermanentAdd;
            dataList += "^R Preferred Mailing~" + !isPermanentAdd;
            
            if (ars_client._IsAddMode == false) //Update CIF
            {
                dataList += "^R CIF Master SEQID~" + _RemCIFMasterSEQID + "^";
                dataList += "R Address Home SEQID~" + _RemCIFAddressHomeSEQID + "^";
                dataList += "R Address Mail SEQID~" + _RemCIFAddressMailSEQID + "^";
                dataList += "R Contact SEQID~" + _RemCIFContactsSEQID + "^";
                dataList += "R ID SEQID~" + _RemCIFIDsSEQID + "^";
                dataList += "R Occupation SEQID~" + _RemCIFOccupationSEQID + "^";
                dataList += "B CIF Master SEQID~" + _BenefCIFMasterSEQID + "^";
                dataList += "B Address SEQID~" + _BenefCIFAddressSEQID + "^";
                dataList += "B Contact SEQID~" + _BenefCIFContactsSEQID;
            }
        }
        else
        {
            if (ars_client._IsAddMode == false)
            {
                dataList += "^B CIF Master SEQID~" + _BenefCIFMasterSEQID + "^";
                dataList += "B Address SEQID~" + _BenefCIFAddressSEQID + "^";
                dataList += "B Contact SEQID~" + _BenefCIFContactsSEQID;
            }
        }
        
        ars_client._RemDataList = dataList;
        return dataList;
    }

    this.showSpecificProduct = function(pID)
    {
        var productID = pID.value
        var divName = "";
        if (productID == 10)//Credit to PNB Account
        {
            divName = "divImplementationCredittoPNBacct";
            ars_client.edElement("btnPNBproduct_Remit", true);
        }
        if (productID == 22)//GFC
        {
            divName = "divImplemCredittoGFC";
            Internet_Remitter_GFC = new GFMC_Maintenance();
            Internet_Remitter_GFC.open_gfc_form(ars_client._BenefCIFID + "~" + ars_client._RemCIFID,'','Internet_Remitter_GFC', true); 
        }
        if (productID ==8)//Door-to-Door
        {
            //ddDoorProduct
            divName = "divImplemDoortoDoor";
        }
        if (productID == 27)//Cash Pick-up
        {
            divName = "divImplementationApas";
            ars_client.load_dropdown("ddImplemApasCurrency");
        }
        if (productID == 12)//Credit to Other Bank Account
        {
            divName = "divImplemCredittoOtherBank";
            ars_client.load_dropdown('ddCredittoOtherBankCurr');
            ars_client.edElement('ddOtherBankName', true);
        }
        
        if (divName != "" && divName != null)
        {
            ars_client.showControl('divImplementationCredittoPNBacct', false);
            ars_client.showControl('divImplemCredittoGFC', false);
            ars_client.showControl('divImplemDoortoDoor', false);
            ars_client.showControl('divImplementationApas', false);
            ars_client.showControl('divImplemCredittoOtherBank', false);
            ars_client.showControl(divName, true);
            divName = "";
        }
    }
    
    this.getPDDTSBankCurrency_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddlcurrency", "CurrName", "CurrCode", "", true);
        }
        else
        {
//            ars_client._reloadCurrency = true;
//            ars_client.load_dropdowns("targetcurrency");
        }
    }
    
    this.validateProduct = function(product, param)
    {
        if (product == 'PNB Account')
        {
            if (param == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        if (product == 'GFC')
        {
            
        }
        
        if (product == 'DTD')
        {
            /*validate if Beneficiary Zip Code is correct based on 
            chosen delivery destination (provincial or metro manila)*/
            if (param == "")
            {
                var x = ars_client.getDDLValue('ddDoorProduct', true);
                var isInMetroManila = null;
                isInMetroManila = (x == 8) ? true : false;
                return IRSWorld.Internet.InternetRemitterARS.isValidZipcode(isInMetroManila, ars_client.getValue("txtSearchedZip", false), this.validateDTD_callback);
            }
            else
            {
                return true;
            }
        }
        
        if (product == 'APAS')
        {
            var curr = ars_client.getValue("ddImplemApasCurrency", false);
            if (curr == "" || curr == null || curr == undefined)
            {
                return;
            }
            else
            {
                this.product_APAS_save(curr);
            }
        }
        
        if (product == 'Other Bank')
        {
            return true;
        }
    }
    
    this.validateDTD_callback = function(result)
    {
        var response = result.value;
        if (response == "" || response == null)
        {
            ars_client.showAlert('Valid: FALSE');
        }
        else
        {
            
            //Save DTD
            ars_client.product_DTD_save();
        }
    }
    
    this.product_APAS_save = function(currency)
    {
        //string prodcode, string targetcurr, string accttype, string acctno, string acctstat
        var PNBProdCode = "";
        var PNBTargetCurr = currency;
        var PNBAccType = "";
        var PNBAcctNo = "";
        var PNBAccStat = "";
        
        PNBProdCode = ars_client.getValue("ddProduct", false);
        return IRSWorld.Internet.InternetRemitterARS.saveMode(PNBProdCode, PNBTargetCurr, PNBAccType, PNBAcctNo, PNBAccStat, "", this.product_APAS_save_callback);
    }
    
    this.product_APAS_save_callback = function(res)
    {
        var result = res.value;
        if (result != "" && result != null && result != undefined)
        {
           var aResult = result.split('|');
           ars_client.showAlert(aResult[1]+"");
        }
    }
    
    this.product_DTD_save = function()
    {
        //string prodcode, string targetcurr, string accttype, string acctno, string acctstat
        var PNBProdCode = "";
        var PNBTargetCurr = "";
        var PNBAccType = "";
        var PNBAcctNo = "";
        var PNBAccStat = "";
        
        PNBProdCode = ars_client.getValue("ddDoorProduct", false);
        return IRSWorld.Internet.InternetRemitterARS.saveMode(PNBProdCode, PNBTargetCurr, PNBAccType, PNBAcctNo, PNBAccStat, "", this.product_DTD_save_callback);
    }
    
    this.product_DTD_save_callback = function(res)
    {
        ars_client.showAlert('Record successfully saved!');
    }
    
    this.product_validatePNBAccount = function()
    {
        //validate Benef PNB Account No
        var val = ars_client.getValue("txtBenefPNBaccountno", false);
        if(ars_client.validateProduct('PNB Account', val) == true)
        {
            ars_client.populateFCRecord(val);
        }
        else
        {
            ars_client.showControl('trImplementationCreditToPNBAcct', false);
            ars_client.showAlert('Account no is an invalid PNB account.');
        }
    }
    
    this.populateFCRecord = function(AccountNo)
    {
        if (AccountNo != "" && AccountNo != null)
        {
            return IRSWorld.Internet.InternetRemitterARS.getFCAccount(AccountNo, this.populateFCRecord_callback);
        }
    }
    
    this.populateFCRecord_callback = function(result)
    {
        var response = result.value;
        if (response != "" && response != null)
        {
            var pval = response.split('|');
//            pval[1] = //AccountStatus
//            pval[3] = //AccountType
//            pval[4] = //Currency 
            
            //return of Flexcube should be displayed here based on account no inputed
            ars_client.enterValue(pval[4], "txtBenefPNBcurrency", false);
            ars_client.enterValue(pval[3], "txtBenefPNBaccountType", false);
            ars_client.enterValue(pval[1], "txtBenefPNBaccountStatus", false);
            ars_client._PNBAccountCurrency = pval[4];
            ars_client.enterValue(ars_client.getEquivalentBranchname(pval[2]), "txtBenefPNBbank", false);
            //show account info based on flexcube return
            ars_client.showControl('trImplementationCreditToPNBAcct', true);
            ars_client.edElement("txtBenefPNBcurrency", true);
            ars_client.edElement("txtBenefPNBaccountType", true);
            ars_client.edElement("txtBenefPNBaccountStatus", true);
            ars_client.edElement("txtBenefPNBbank", true);
        }
    }
    
    this.getEquivalentBranchname = function(BranchCode)
    {
        return IRSWorld.Internet.InternetRemitterARS.getEquivalentBranchName(BranchCode).value;
    }
    
    this.product_CreditToPNBAcct_save = function()
    {
        //string prodcode, string targetcurr, string accttype, string acctno, string acctstat
        var PNBProdCode = "";
        var PNBTargetCurr = "";
        var PNBAccType = "";
        var PNBAcctNo = "";
        var PNBAccStat = "";
        
        PNBProdCode = ars_client.getValue("ddProduct", false);
        PNBTargetCurr = ars_client.getValue("txtBenefPNBcurrency", false);
        PNBAccType = ars_client.getValue("txtBenefPNBaccountType", false);
        PNBAccStat = ars_client.getValue("txtBenefPNBaccountStatus", false);
        PNBAcctNo = ars_client.getValue ("txtBenefPNBaccountno", false);
               
        return IRSWorld.Internet.InternetRemitterARS.saveMode(PNBProdCode, PNBTargetCurr, PNBAccType, PNBAcctNo, PNBAccStat, "", this.product_CreditToPNBAcct_save_callback);
    }
    
    this.product_CreditToPNBAcct_save_callback = function(result)
    {
        ars_client.showAlert("Record successfully saved!");
        ars_client.edElement("btnPNBproduct_saveDetails", true);
        ars_client.edElement("btnPNBproduct_Remit", false);
    }
    
    this.product_GFC_save = function()
    {
        ars_client.showAlert('Record successfully saved!');
        //Go to Transaction Screen
    }
    
//    this.product_DTD_save = function()
//    {
//        var beneZip = ars_client.getValue("txtBeneZip", false);
//        if (ars_client.validateProduct('DTD', beneZip) == true)
//        {
//            ars_client.showAlert('Record successfully saved!');
//            //Go to Transaction Screen
//        }
//        else
//        {
//            ars_client.showAlert('Beneficiary ADdress is within / outside Metro Manila');
//        }
//    }
    
//    this.product_APAS_save = function()
//    {
//        if (ars_client.validateProduct('APAS', '') == true)
//        {
//            ars_client.showAlert('Record successfully saved!');
//        }
//    }
    
    this.product_OtherBank_save = function()
    {
        if (ars_client.validateProduct('Other Bank', '') == true)
        {
            //string prodcode, string targetcurr, string accttype, string acctno, string acctstat, string accountBankName
            var PNBProdCode = "";
            var PNBTargetCurr = "";
            var PNBAccType = "";
            var PNBAcctNo = "";
            var PNBAccStat = "";
            var PNBAcctBranchCode = "";
            
            PNBProdCode = ars_client.getValue("ddProduct", false);
            PNBTargetCurr = ars_client.getValue("ddCredittoOtherBankCurr", false);
            PNBAccType = "";
            PNBAcctNo = ars_client.getValue("txtOtherBankAccountNo", false);
            PNBAccStat = "";
            PNBAcctBranchCode = ars_client.getValue("ddOtherBankName", false);
                   
            return IRSWorld.Internet.InternetRemitterARS.saveMode(PNBProdCode, PNBTargetCurr, PNBAccType, PNBAcctNo, PNBAccStat, PNBAcctBranchCode, this.product_CreditToPNBAcct_save_callback);
        }
        else
        {
        
        }
    }
    
    this.product_OtherBank_save_callback = function(res)
    {
        if (res != null && res.value != "")
        {
            ars_client.showAlert('Record successfully saved.');
        }
    }
    
    this.editCIF = function(IsRemitter)//True = Remitter or False = Beneficary
    {
        if (IsRemitter == true)
        {
            this.disableFields("Remitter", false);
            ars_client._IsAddMode = false;
            ars_client._SaveBenefOnly = false;
        }
        else
        {
            this.disableFields("Beneficiary", false);
            ars_client.edElement('btnSaveBenef', true);
            ars_client.edElement('txtBenefCity', true);
            ars_client.edElement('txtBenefProvince', true);
            ars_client._IsAddMode = false;
            ars_client._SaveBenefOnly = true;
        }
    }
    
    this.getCIFMasterSEQID = function(CIFID)
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getCIFMasterSEQID(CIFID, this.getCIFMasterSEQID_callback);
    }
    
    this.getCIFMasterSEQID_callback = function(param)
    {
        if (param.value != null && param.value != "")
        {
            return param.value;
        }
        else
        {
            return "";
        }
    }
    
    this.getCIFContactSEQID = function(CIFID)
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getCIFContactSEQID(CIFID, this.getCIFContactSEQID_callback);
    }
    
    this.getCIFContactSEQID_callback = function(param)
    {
        if (param.value != null && param.value != "")
        {
            return param.value;
        }
        else
        {
            return "";
        }
    }
    
    this.getCIFAddressSEQID = function(CIFID, addressType)
    {
        //1 - Business
        //2 - Home
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getCIFAddressSEQID(CIFID, addressType, this.getCIFAddressSEQID_callback);
    }
    
    this.getCIFAddressSEQID_callback = function(param)
    {
        if (param.value != null && param.value != "")
        {
            return param.value;
        }
        else
        {
            return "";
        }
    }
    
    this.getCIFIDsSEQID = function(CIFID)
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getCIFIDsSEQID(CIFID, this.getCIFIDsSEQID_callback);
    }
    
    this.getCIFIDsSEQID_callback = function(param)
    {
        if (param.value != null && param.value != "")
        {
            return param.value;
        }
        else
        {
            return "";
        }
    }
    
    this.getCIFOccupationsSEQID = function(CIFID)
    {
        ars_client.ajaxTimeout();
        return IRSWorld.Internet.InternetRemitterARS.getCIFIDsSEQID(CIFID, this.getCIFIDsSEQID_callback);
    }
    
    this.getCIFOccupationsSEQID_callback = function(param)
    {
        if (param.value != null && param.value != "")
        {
            return param.value;
        }
        else
        {
            return "";
        }
    }
    
    this.preventsubmit = function(key_code)
    {
        var e           = window.event  || key_code;
        var keynum      = e.charCode    || e.keyCode        || e.which;
        var src_element = e.target      || e.srcElement;

        if (src_element.id.toString().toUpperCase() == "TXTBENEZIP")
        {
            if (keynum == 13)
            {
                ars_client.load_dropdown("BeneZipCode");
                return false;
            }
        }
        return true;
    }
    
    this.zip_visibility = function(nm)
    {
        var sel = document.getElementById("ddlZipCode");
        if (nm == "Search")
        {
            document.getElementById("searchzipcode").style.display = "";
            //document.getElementById("searchzipcodeDdl").style.display = "block";
            document.getElementById("lblfield").style.display = "none";
            
        }
        else
        {
            //Assign Benef Zip Code SEQID
            var ZipCodeSEQID = sel.options[sel.selectedIndex].value;
            ars_client._BenZipCode = ZipCodeSEQID;
            
            var concatVal = sel.options[sel.selectedIndex].text;
            var arrVal = concatVal.split('-');
            
            ars_client.enterValue(arrVal[0], "txtBenefCity", false);
            ars_client.enterValue(arrVal[1], "txtBenefProvince", false);
            ars_client.enterValue(arrVal[2], "txtSearchedZip", false);
            
            document.getElementById("searchzipcode").style.display = "none";
            //document.getElementById("searchzipcodeDdl").style.display = "none";
            document.getElementById("lblfield").style.display = "";
            var select = document.getElementById("ddlZipCode");
            var ZipCodeSEQID = select.options[select.selectedIndex].value;
        }
    }
    
    this.load_dropdown = function(ddl)
    {
        switch (ddl)
        {
            case "ddCredittoOtherBankCurr":
                ars_client.ajaxTimeout();
                return IRSWorld.Internet.InternetRemitterARS.getTargetCurrency(document.getElementById('ddProduct').value, this.load_OtherBankAccountCurr_callback);
                break;
        
            case "ddOtherBankName":
                ars_client.ajaxTimeout();
                var currency = document.getElementById('ddCredittoOtherBankCurr').value;
                if (currency != "" && currency != null)
                {
                    return IRSWorld.Internet.InternetRemitterARS.getOtherBanks(currency, this.load_OtherBanks_callback);
                }
                else
                {
                    ars_client.clearDDL('ddOtherBankName');
                    ars_client.edElement('ddOtherBankName', true);
                }
                
                break;
                
            case "ddImplemApasCurrency":
                ars_client.ajaxTimeout();
                return IRSWorld.Internet.InternetRemitterARS.getTargetCurrency(document.getElementById('ddProduct').value, this.load_ApasCurr_callback);
                break;
            
            case "BeneZipCode":
                ars_client.ajaxTimeout();
                return IRSWorld.Internet.InternetRemitterARS.getBeneZipCode(ars_client.getValue("txtBeneZip", false), this.getBeneZipCode_callback);
                break;
                
        }
    }
    
    this.getBeneZipCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            document.getElementById('ddlZipCode').style.display = '';
            ars_client.fillDDL(response, "ddlZipCode", "ZIPCODE", "SEQ_ID", "", true);
            document.getElementById("ddlZipCode").setAttribute("size", 5);
            var select = document.getElementById("ddlZipCode");
            if (select.options.length == 2)
            {
                select.options.selectedIndex = 1;
                ars_client.zip_visibility("Select");
            }
        }
        else
        {
            ars_client.showAlert("", "Zip code not found.");
        }
    }
    
    this.load_OtherBankAccountCurr_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddCredittoOtherBankCurr", "CurrName", "CurrCode", "", "");
        }
    }
    
    this.load_OtherBanks_callback = function(result)
    {
        var response = result.value;
        ars_client.clearDDL('ddOtherBankName');
        ars_client.edElement('ddOtherBankName', false);
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddOtherBankName", "BranchName", "BranchCode", "", "");
        }
    }
    
    this.load_ApasCurr_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_client.fillDDL(response, "ddImplemApasCurrency", "CurrName", "CurrCode", "", "");
        }
    }
    
    this.loadRecord = function()
    {
        this.ajaxTimeout();
        
        var pLastName = ars_client.getValue("txtSLastName", false);
        var pFirstName = ars_client.getValue("txtSFirstName", false);
        var pMiddleName = ars_client.getValue("txtSMiddleName", false);
                        
        return IRSWorld.Internet.InternetRemitterARS.searchRem(pLastName, pFirstName, pMiddleName, this.loadRecord_callback); 
    }

    this.loadRecord_callback = function(result)
    {
        val = result.value;
        
        if (val != null && val != "" && val != undefined)
        {
            ars_client.showControl("trSearchResults", true);
            datatable = val;
            show_groups();
        }
        else
        {
            alert("No record found.");
        }
        return;
    }
    
    this.storeSessionForTransaction_callback = function(RemCIFID, BenCIFID, ProductID)
    {
        location.href='../ARS/ARSW.aspx';
        return;   
    }
    
    this.SameRemAndBene = function()
    {
        ars_client.enterValue(ars_client.getValue("txtRemLName", false), "txtBenefLName", false);
        ars_client.enterValue(ars_client.getValue("txtRemMName", false), "txtBenefMName", false);
        ars_client.enterValue(ars_client.getValue("txtRemFName", false), "txtBenefFName", false);
        ars_client.enterValue(ars_client.getValue("txtDateofBirth", false), "txtBenefDOB", false);
        ars_client.enterValue(ars_client.getValue("txtRemEmailAdd", false), "txtBenefEmailAdd", false);
        ars_client.enterValue(ars_client.getValue("txtContactNo", false), "txtBenefTelNo", false);
    }
    
    this.populateProduct = function(prodcode, targetcurr, accttype, acctno, acctstat, accountBranchCode)
    {
        if (prodcode == 9)
        {
            ars_client.findDDL("ddProduct", "8", '', true);
        }
        else
        {
            ars_client.findDDL("ddProduct", prodcode, '', true);
        }
        
        var divName = "";
        if (prodcode == 10)//Credit to PNB Account
        {
            divName = "divImplementationCredittoPNBacct";
            ars_client.edElement("btnPNBproduct_Remit", true);
        }
        if (prodcode == 22)//GFC
        {
            divName = "divImplemCredittoGFC";
            Internet_Remitter_GFC = new GFMC_Maintenance();
            //Internet_Remitter_GFC.open_gfc_form(ars_client._BenefCIFID + "~" + ars_client._RemCIFID,'','Internet_Remitter_GFC', true); 
        }
        if (prodcode == 8 || prodcode == 9)//Door-to-Door
        {
            //ddDoorProduct
            divName = "divImplemDoortoDoor";
            ars_client.findDDL("ddDoorProduct", prodcode, "", true);
        }
        if (prodcode == 27)//Cash Pick-up
        {
            divName = "divImplementationApas";
            //ars_client.load_dropdown("ddImplemApasCurrency");
        }
        if (prodcode == 12)//Credit to Other Bank Account
        {
            divName = "divImplemCredittoOtherBank";
            ars_client.load_dropdown('ddCredittoOtherBankCurr');
            
            ars_client.edElement('ddOtherBankName', false);
            ars_client.edElement('txtOtherBankAccountNo', false);
            ars_client.edElement('ddCredittoOtherBankCurr', false);
            
            ars_client.enterValue(acctno, "txtOtherBankAccountNo", false);
            ars_client.findDDL("ddOtherBankName", accountBranchCode, "", true);
            ars_client.findDDL("ddCredittoOtherBankCurr", targetcurr, "", true);
        }
        
        if (divName != "" && divName != null)
        {
            ars_client.showControl('divImplementationCredittoPNBacct', false);
            ars_client.showControl('divImplemCredittoGFC', false);
            ars_client.showControl('divImplemDoortoDoor', false);
            ars_client.showControl('divImplementationApas', false);
            ars_client.showControl('divImplemCredittoOtherBank', false);
            ars_client.showControl(divName, true);
            divName = "";
        }
    }
}

function gfc_response()
{   
    var res  = null;
    res  = Internet_Remitter_GFC.CardDetails();
    //GFC Response Information
//    icls._Details[0] = icls.CKAcct;
//    icls._Details[1] = icls.CardNumber;
//    icls._Details[2] = icls.CurrCode;
//    icls._Details[3] = icls.Mode;  

//sample result
//            icls._Details = new Array();
//            icls._Details[0] = document.getElementById('txtGFMCRemCardRefNo').value;       //CASA_Acct 
//            icls._Details[1] = document.getElementById('txtGFMCRemGlobalCardNo').value;   //GFMC_Card
//            icls._Details[2] = document.getElementById('ddlCurrency').value;                     //Currency
//            icls._Details[3] = icls.pMode;                                                                    //Mode
    ars_client.gfc_response_callback(res); 
}  


//SEARCH GRID STARTS HERE






function show_groups()
{
    var FirstName = '';
    var MiddleName = ''
    
    if (dbgrid == null)
    {
        dbgrid = create_grid();
        dbgrid.bind(datatable, 'tdReportLookup', 'tblReportLookup');
    }
    
//    dbgrid.clearFilter();     
//    
    dbgrid.refresh(datatable);
    //document.getElementById("tdReportLookup").style.display = 'block';
}

/*
|---------------######--#####----########--#####--------------------|
|---------------##------##--##------##-----##--##-------------------|
|---------------##-###--######------##-----##---##------------------|
|---------------##-###--##--##------##-----##--##-------------------|
|---------------######--##---##--########--#####--------------------|
*/
function create_grid()
{
    {
	    var col = new datagrid_column(); // 0
	    col.align = 'left';
	    col.header = 'SEQ ID';
	    col.datafield = 'Column1';
	    col.styles.header += 'display:none;';
	    col.styles.datafield += 'display:none;';
        col.styles.cell += 'width:1%;display:none;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 2
	    col.header = 'RemitterID';
	    col.datafield = 'REMITTER CIFID';
	    col.align = 'left';
	    col.styles.cell += 'width:8%';
	    col.styles.datafield += 'width:8%;';
	    col.editable = false;
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 2
	    col.header = 'Remitter Name';
	    col.datafield = 'REMITTER NAME';
	    col.align = 'left';
	    col.styles.cell += 'width:18%;padding-right:25px;padding-left:25px;';
	    col.styles.datafield += 'width:18%;';
	    col.editable = false;
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 4
	    col.align = 'left';
	    col.header = 'Beneficiary_Name';
	    col.datafield = 'BENEFICIARY NAME';
	    col.editable = false;
        col.styles.cell += 'width:20%;padding-right:25px;padding-left:25px;';
        col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 5
	    col.header = 'Product';
	    col.datafield = 'PRODUCT';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:15%';
	    col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 6
	    col.header = 'Account Number';
	    col.datafield = 'ACCOUNT NUMBER';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:15%;padding-right:25px;padding-left:25px;';	
	    col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 7
	    col.header = 'Branch Name';
	    col.datafield = 'BRANCH NAME';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:15%;padding-right:25px;padding-left:25px;';
	    col.styles.datafield += 'width:15%;';
	    gv.addColumn(col);
    }
        
   {
        var col = new datagrid_column(); // 7
        col.header = '';
        col.datafield = 'EDIT';
        col.align = 'left';
        col.editable = false;
        col.styles.cell += 'width:1%;padding-right:5px;padding-left:5px;';
        col.styles.datafield += 'width:1%;';
        col.type = 'link';
	    col.href = 'searchGridEdit';
        gv.addColumn(col);
    }
    {
	    var col = new datagrid_column(); // 7
	    col.header = '';
	    col.datafield = 'REMIT';
	    col.align = 'left';
	    col.editable = false;
	    col.styles.cell += 'width:1%;padding-right:5px;padding-left:5px;';
	    col.styles.datafield += 'width:1%;';
	    col.type = 'link';
	    col.href = 'searchGridRemit';
	    gv.addColumn(col);
    } 
    // for footer
        gv.footer.edit_button.none = true;            
        gv.footer.edit_button.style += 'background-color: lavender; display: none;'; 
        gv.footer.edit_button.onclick = '';              
        gv.footer.save_button.none = true; 
        gv.footer.save_button.onclick = '';  
        gv.footer.save_button.style += 'background-color: lavender; display: none;';  
        gv.footer.cancel_button.none = true;
        gv.footer.cancel_button.style += 'background-color: lavender; display: none;'; 
        gv.row.normal_style = 'background-color: #f7ffff;';   
        
        //rows per page to display
        gv.rows_page = 8;                   
        return gv;
}

function searchGridEdit(args)
{
    if (args != undefined && args != null)
    {
        var paramSplit = args.split('_');
        var argID = paramSplit[3];
        
        var ID = 'tblReportLookup_txt_Column1_' + argID;
        var IDs = document.getElementById(ID).value;
        ars_client.setRemBenVal(IDs);
        var SEQID = IDs.split('~');
        var RemSEQID = SEQID[0];
        var BenSEQID = SEQID[1];
        var RemCIFID = SEQID[2];
        var BenCIFID = SEQID[3];
        
        
        ars_client.getRemitterAndBeneInfo(RemCIFID, BenCIFID);
        return;
    }
    
    return;
}

function searchGridRemit(args)
{
    if (args != undefined && args != null)
    {
        var paramSplit = args.split('_');
        var argID = paramSplit[3];
        
        var ID = 'tblReportLookup_txt_Column1_' + argID;
        var IDs = document.getElementById(ID).value;
        ars_client.setRemBenVal(IDs);
        var SEQID = IDs.split('~');
        var RemSEQID = SEQID[0];
        var BenSEQID = SEQID[1];
        var RemCIFID = SEQID[2];
        var BenCIFID = SEQID[3];
        var ProductID = SEQID[4];
        var productcode = SEQID[5];
        //Go to Transaction Screen
        //window.location = 'ARS/ARSW.aspx';
        //Store session
        
        //1TEL00000048
        //2TEL00000030
        //3741002	
        if (productcode != undefined && productcode != null && productcode != '')
        {
            return IRSWorld.Internet.InternetRemitterARS.storeSessionForTransaction(RemCIFID, BenCIFID, ProductID, ars_client.storeSessionForTransaction_callback);
        }
        else
        {
            ars_client.showAlert('Please add a Product.');
            return;
        }
    }
    
    return;
}

