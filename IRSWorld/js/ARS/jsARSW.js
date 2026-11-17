/************************/
/*** KEYBOARD CONTROL ***/
/************************/
var version = navigator.appVersion;
function showKeyCode(e) 
{
	var keycode = (window.event) ? event.keyCode : e.keyCode;

	if ((version.indexOf('MSIE') != -1)) 
	{
		if (keycode == 116) 
		{
			event.keyCode = 0;
			event.returnValue = false;
			window.alert("Please refrain from pressing the F5 button while the transaction is being processed");
			return false;
		}
	}
	else 
	{
		if (keycode == 116) 
		{
			window.alert("Please refrain from pressing the F5 button while the transaction is being processed");
			return false;
		}
	}
}

/************************/
/*** AJAX GRID        ***/
/************************/
function execute_ajaxgridview_command(param)
{
    if (ars_page != null && ars_page != undefined)
    {
        ars_page.do_ajaxgridview(param);
    }
}

/************************/
/*** AJAX GRID        ***/
/************************/
function execute_ajaxgridview_command(param)
{
    if (ars_page != null && ars_page != undefined)
    {
        ars_page.do_ajaxgridview(param);
    }
}

function execute_ajx_command(param)
{
    if (ars_page != null && ars_page != undefined)
    {
        ars_page.do_ajaxgridview_search(param);
    }
}

/************************/
/*** CLOSE FRAME      ***/
/************************/
function close()
{
    var response = self.returnValue;
    close_frame_dialog(false);
}

function ars_page()
{
    /************************/
    /*** INITIALIZE PAGE  ***/
    /************************/
    
    var _branchcode             = "";
    var _tranxdate              = "";
    var _accountseqid           = "";
    var _targetcurrencycode     = "";
    var _targetcurrency         = "";
    var _remittingcurrency      = "";
    var _productcode            = "";
    var _implementationtype     = "";
    var _chargetype             = "";
    var _paymode                = "";
    var _computation            = "";
    var _amount                 = "";
    var _calc                   = "";
    var _forex                  = "";
    var _grossamount            = "";
    var _netamount              = "";
    var _nettobenamount         = "";
    var _chargeamount           = "";
    var _remitterid             = "";
    var _beneficiaryid          = "";
    var _osa                    = "";
    var _isobp                  = "";
    var _productseqid           = "";
    var _purpose                = "";
    var _remitteraddressid      = "";
    var _remittercontactid      = "";
    var _beneficiaryaddressid   = "";
    var _beneficiarycontactid   = "";
    var _referenceno            = "";
    var _txnseqid               = "";
    var _generatereceipt        = false;
    var _ishold                 = false;
    var _clearingdate           = "";
    var _checknumber            = "";

    this.initialize = function()
    {
        ars_page.clearAll();
        this.getSession();
        this.getBranchCode();
        this.getTranxDate();
        this.getPreReceiptHeader();
        this.getRemittingCurrency();
        this.getPaymentMethod();
        this.getPurpose();
        this.getCIFInformation();
    }
    
    this.clearAll = function()
    {
        ars_page._branchcode            = "";
        ars_page._tranxdate             = "";
        ars_page._accountseqid          = "";
        ars_page._targetcurrencycode    = "";
        ars_page._targetcurrency        = "";
        ars_page._remittingcurrency     = "";
        ars_page._productcode           = "";
        ars_page._implementationtype    = "";
        ars_page._chargetype            = "";
        ars_page._paymode               = "";
        ars_page._computation           = "";
        ars_page._amount                = "";
        ars_page._calc                  = "";
        ars_page._forex                 = "";
        ars_page._grossamount           = "";
        ars_page._netamount             = "";
        ars_page._nettobenamount        = "";
        ars_page._chargeamount          = "";
        ars_page._remitterid            = "";
        ars_page._beneficiaryid         = "";
        ars_page._osa                   = "";
        ars_page._isobp                 = "";
        ars_page._productseqid          = "";
        ars_page._purpose               = "";
        ars_page._remitteraddressid     = "";
        ars_page._remittercontactid     = "";
        ars_page._beneficiaryaddressid  = "";
        ars_page._beneficiarycontactid  = "";
        ars_page._referenceno           = "";
        ars_page._txnseqid              = "";
        ars_page._generatereceipt       = false;
        ars_page._ishold                = false;
        ars_page._clearingdate          = "";
        ars_page._checknumber           = "";
    }
    
    /************************/
    /*** COMMON FUNCTIONS ***/
    /************************/
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            ars_page.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
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
        //(message != "") ? show_jq_message("VIP Message", message, true) : show_jq_message("VIP Message", "An error occured while getting information. Please try again.", true);
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
                        if (iRow.Rows[i][arrSplit[arr]] != null && ars_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && ars_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
                ars_page.getDDL(element, selected, true);
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
                        ars_page.getDDL(element, selected, true);
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
                        ars_page.getDDL(element, selected, true);
                    }
                    break;
                }
            }
        }
    }
    
    this.findDDLHash = function(element, findvalue, isvalue, delimiter)
    {
        var select = document.getElementById(element);
        var split;
        for (var i = 0; i <= (select.length - 1); i++)
        {
            if (isvalue == true)
            {
                split = select.options[i].value.split(delimiter);
                for (var j = 0; j <= (split.length - 1); j++)
                {
                    if (split[j] == findvalue)
                    {
                        select.selectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                split = select.options[i].value.split(delimiter);
                for (var j = 0; j <= (split.length - 1); j++)
                {
                    if (split[j] == findvalue)
                    {
                        select.selectedIndex = i;
                        break;
                    }
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
            ars_page.hidePanel("divPopProcess");
        }
        else
        {
            new common_method().to_show_loading_message('divLoadingMsg');
            document.getElementById("lblLoadingMsg").innerHTML = message;
            ars_page.showPanel("divPopProcess");
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
                            ars_page.removeValue(element, false);
                            i = inputs.length;
                        }
                        else
                        {
                            ars_page.removeValue(inputName, false);
                        }
                        break;
                        
                    case "readonly":
                        if (element != "")
                        {
                            ars_page.roElement(element, yesno);
                            i = inputs.length;
                        }
                        else
                        {
                            ars_page.roElement(inputName, yesno);
                        }
                        break;
                        
                    case "disable":
                        if (element != "")
                        {
                            ars_page.edElement(element, yesno);
                            i = inputs.length;
                        }
                        else
                        {
                            ars_page.edElement(inputName, yesno);
                        }
                        break;
                }
            }
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
        ars_page.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {
        ars_page.showControl(divName, false);
        divmask.hide();
    }
    
    this.hide = function()
    {
        this.waitifrm.style.display = this.divobj.style.display = "none";
    }
    
    /************************/
    /*** ENTER KEY        ***/
    /************************/
    
    this.preventsubmit = function(key_code)
    {
        var e           = window.event  || key_code;
        var keynum      = e.charCode    || e.keyCode || e.which;
        var src_element = e.target      || e.srcElement;

        if (src_element.id.toString().toUpperCase() == "TXTGROSSAMOUNT" ||
            src_element.id.toString().toUpperCase() == "TXTNETAMOUNT" ||
            src_element.id.toString().toUpperCase() == "TXTNETTOBENAMOUNT")
        {
            if (keynum == 13)
            {
                if (ars_page.validateCharges())
                {
                    switch (src_element.id.toString().toUpperCase())
                    {
                        case "TXTGROSSAMOUNT":
                            ars_page._computation = "1";
                            ars_page._amount = ars_page.getValue("txtGrossAmount", false);
                            break;
                            
                        case "TXTNETAMOUNT":
                            ars_page._computation = "2";
                            ars_page._amount = ars_page.getValue("txtNetAmount", false);
                            break;
                            
                        case "TXTNETTOBENAMOUNT":
                            ars_page._computation = "3";
                            ars_page._amount = ars_page.getValue("txtNetToBenAmount", false);
                            break;
                    }
                    ars_page.getCharges(false);
                }
                return false;
            }
        }
        return true;
    }
    
    /************************/
    /*** AJAX GRID SIMPLE ***/
    /************************/
    
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != undefined)
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    return;
                }
                this.process_grid_viewer(parameter);
            }
        }
    }

    this.load_grid_viewer = function()
    {
        this.process_grid_viewer("ajx_loading");
    }

    this.process_grid_viewer = function(parameter)
    {
        ars_page.showPanel("divAggregate");
        var sql = "sp_rem_otc_remitter_aggregate2~" + 
                    ars_page._branchcode.toString()        + "~" + 
                    ars_page._tranxdate.toString()         + "~" + 
                    ars_page._remittingcurrency.toString() + "~" + 
                    ars_page._remitterid.toString();

        this.grid = new grid_viewer();
        this.grid.sql_source        = sql;
        this.grid.div_container     = document.getElementById("divAggregateGrid");
        this.grid.datarow_column    = 0;
        this.grid.page_size         = 5;
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.grd_customed_date = false;
        this.grid.process_ajax(parameter);
        this.grid                   = null;
        document.getElementById("divLoading").innerHTML = "";
        document.getElementById("divAggregateTop").innerHTML = "";
    }
    
    /************************/
    /*** FUNCTIONS        ***/
    /************************/
    
    this.validateCharges = function()
    {
        if (ars_page._productcode == "")
        {   
            ars_page.showAlert("Please select a Product");
            ars_page.showFocus("ddlProduct");
            return false;
        }
        else
        {
            if (ars_page._remittingcurrency == "")
            {
                ars_page.showAlert("Please select a Remitting Currency");
                ars_page.showFocus("ddlRemittingCurrency");
                return false;
            }
            else
            {
                if (ars_page._chargetype == "")
                {
                    ars_page.showAlert("Please select a Charge Type");
                    ars_page.showFocus("ddlChargeType");
                    return false;
                }
                else
                {
                    if (ars_page._paymode == "")
                    {
                        ars_page.showAlert("Please select a Payment Method");
                        ars_page.showFocus("ddlPaymentMethod");
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    this.getDDLAjax = function(ddlname)
    {
        var split;
        var clear = false;
        
        switch (ddlname)
        {
            case "ddlProduct":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    split = ars_page.getDDLValue(ddlname, true).split("|");
                    ars_page._accountseqid = split[0];
                    ars_page._targetcurrencycode = split[1];
                    ars_page._targetcurrency = split[2];
                    ars_page._productcode = split[3];
                    ars_page._implementationtype = split[4];
                    ars_page._productseqid = split[5];
                    ars_page._accountno = split[6];
                    ars_page.enterValue(ars_page._targetcurrency, "spnNetToBenCurrency", true);
                    if (ars_page._accountno != "" && ars_page._implementationtype == "2")
                    {
                        ars_page._isobp = IRSWorld.ARS.ARSW.validateOBP(ars_page._accountno).value;
                    }
                    else
                    {
                        ars_page._isobp = "";
                    }
                    ars_page.getChargeType();
                    clear = true;
                    break;
                }
                
            case "ddlRemittingCurrency":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    var currency = ars_page.getDDLValue(ddlname, false);
                    ars_page._remittingcurrency = ars_page.getDDLValue(ddlname, true);
                    ars_page.enterValue(currency, "spnGrossCurrency", true);
                    ars_page.enterValue(currency, "spnChargeCurrency", true);
                    ars_page.enterValue(currency, "spnNetCurrency", true);
                    clear = true;
                }
                break;
                
            case "ddlChargeType":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    ars_page._chargetype = ars_page.getDDLValue(ddlname, true);
                    clear = true;
                }
                break;
                
            case "ddlPaymentMethod":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    ars_page._paymode = ars_page.getDDLValue(ddlname, true);
                    clear = true;
                }
                break;
                
            case "ddlPurpose":
                if (document.getElementById(ddlname).selectedIndex > 0)
                {
                    ars_page._purpose = ars_page.getDDLValue(ddlname, true);
                    clear = false;
                }
                break;
        }
        
        if (ars_page._remittingcurrency != "" && ars_page._targetcurrencycode != "")
        {
            ars_page.getForex();
        }
        if (clear)
        {
            ars_page.enterValue("0.00", "txtGrossAmount", false);
            ars_page.enterValue("0.00", "txtNetAmount", false);
            ars_page.enterValue("0.00", "txtNetToBenAmount", false);
            ars_page.enterValue("0.00", "txtChargeAmount", false);
        }
    }
    
    this.setComputation = function(computation)
    {
        ars_page._computation = computation;
    }
    
    this.compute = function()
    {
        var ok = false;
        switch (ars_page._computation)
        {
            case "1":
                if (ars_page.getValue("txtGrossAmount", false) == "0.00" || Number(ars_page.getValue("txtGrossAmount", false)) <= 0)
                {
                    ars_page.showAlert("Please enter amount in the Total Cost field.")
                    ars_page.showFocus("txtGrossAmount");
                }
                else
                {
                    ars_page._amount = ars_page.getValue("txtGrossAmount", false);
                    ok = true;
                }
                break;
                
            case "2":
                if (ars_page.getValue("txtNetAmount", false) == "0.00" || Number(ars_page.getValue("txtNetAmount", false)) <= 0)
                {
                    ars_page.showAlert("Please enter amount in the Amount to be Sent field.")
                    ars_page.showFocus("txtNetAmount");
                }
                else
                {
                    ars_page._amount = ars_page.getValue("txtNetAmount", false);
                    ok = true;
                }
                break;
                
            case "3":
                if (ars_page.getValue("txtNetToBenAmount", false) == "0.00" || Number(ars_page.getValue("txtNetToBenAmount", false)) <= 0)
                {
                    ars_page.showAlert("Please enter amount in the Amount to be Received field.")
                    ars_page.showFocus("txtNetToBenAmount");
                }
                else
                {
                    ars_page._amount = ars_page.getValue("txtNetToBenAmount", false);
                    ok = true;
                }
                break;
        }
        if (ok)
        {
            ars_page.getCharges(false);
        }
    }
    
    this.validate = function()
    {
        if (document.getElementById("ddlProduct").selectedIndex <= 0)
        {
            ars_page.showAlert("Product is a required field. Please select from the dropdown and try again.");
            ars_page.showFocus("ddlProduct");
            return false;
        }
        if (document.getElementById("ddlRemittingCurrency").selectedIndex <= 0)
        {
            ars_page.showAlert("Remitting Currency is a required field. Please select from the dropdown and try again.");
            ars_page.showFocus("ddlRemittingCurrency");
            return false;
        }
        if (document.getElementById("ddlChargeType").selectedIndex <= 0)
        {
            ars_page.showAlert("Charge Type is a required field. Please select from the dropdown and try again.");
            ars_page.showFocus("ddlChargeType");
            return false;
        }
        if (document.getElementById("ddlPaymentMethod").selectedIndex <= 0)
        {
            ars_page.showAlert("Payment Method is a required field. Please select from the dropdown and try again.");
            ars_page.showFocus("ddlPaymentMethod");
            return false;
        }
        if (document.getElementById("ddlPurpose").selectedIndex <= 0)
        {
            ars_page.showAlert("Transaction Purpose is a required field. Please select from the dropdown and try again.");
            ars_page.showFocus("ddlPurpose");
            return false;
        }
        if (ars_page.getValue("txtNetToBenAmount", false) == "" || ars_page.getValue("txtNetToBenAmount", false) == "0.00" || Number(ars_page.getValue("txtNetToBenAmount", false)) <= 0 || ars_page.getValue("txtNetToBenAmount", false) == ".")
        {
            ars_page.showAlert("Transaction Amount is a required field. Please compute an amount and try again.");
            ars_page.showFocus("txtNetToBenAmount");
            return false;
        }
        if (new common_method().is_decimal(new common_method().to_remove_comma(ars_page.getValue("txtNetToBenAmount", false))) == false)
        {
            ars_page.showAlert("Amount entered is not valid!\n Click ok to correct amount.");
            return false;
        }
        if (ars_page._productcode == "8" || ars_page._productcode == "9")
        {
            if (ars_page._osa.toString() == "0")
            {
                ars_page.showAlert("Destination is not a serviceable area. Please transact using advise and pay or go to the nearest PNB branch.");
                return false;
            }
        }
        if (ars_page._isobp == true)
        {
            ars_page.showAlert("Merchant accounts are not allowed. Please use the bills payment module.");
            return false;
        }
        return true;
    }
    
    this.submitClearingDate = function()
    {
        ars_page._clearingdate = ars_page.getValue("txtClearingDate", false);
        ars_page._checknumber = ars_page.getValue("txtCheckNumber", false);
        if (ars_page._clearingdate == "")
        {
            ars_page.showAlert("Please input a clearing date for this transaction.");
            ars_page.showFocus("txtClearingDate");
        }
        else
        {
            if (ars_page._checknumber == "")
            {
                ars_page.showAlert("Please input a cheque number for this transaction.");
                ars_page.showFocus("txtCheckNumber");
            }
            else
            {
                ars_page.hidePanel("divClearingDate");
                if (ars_page.validate())
                {
                    ars_page.validateAccountStatus();
                }
            }
        }
    }
    
    this.remit = function()
    {
        if (ars_page._paymode == "2")
        {
            ars_page._ishold = true;
            ars_page.showPanel("divClearingDate");
        }
        else
        {
            ars_page._ishold = false;
            if (ars_page.validate())
            {
                ars_page.validateAccountStatus();
            }
        }
    }
    
    this.remitfinal = function()
    {
        ars_page.edElement("btnContinue", true);
        ars_page.hidePanel("divAggregate");
        ars_page.submitTransaction();
    }
    
    this.printDiv = function(divName)
    {
        ars_page.showControl("btnPrintPanel", false);
        ars_page.showControl("btnClosePanel", false);
        ars_page.showControl("btnSendReceipt", false);
        ars_page.hidePanel("divTransactionHistoryModal");
        var headstr = "<html><head><title></title><link href=\"../css/reskin.css\" rel=\"stylesheet\" type=\"text/css\" /><link href=\"../css/cssWeb.css\" rel=\"stylesheet\" type=\"text/css\" /></head><body>";
        var footstr = "</body>";
        var newstr = document.all.item(divName).innerHTML;
        var oldstr = document.body.innerHTML;
        document.body.innerHTML = headstr + newstr + footstr;
        window.print();
        document.body.innerHTML = oldstr;
        ars_page.showControl("btnPrintPanel", true);
        ars_page.showControl("btnClosePanel", true);
        ars_page.showControl("btnSendReceipt", true);
    }
    
    this.sendReceiptEmail = function()
    {
        if (confirm("Do you want to send transaction receipt thru email?"))
        {
            ars_page.sendOREmail();
        }
    }
    
    this.closeTransactionHistoryDiv = function()
    {
        ars_page.hidePanel("divTransactionHistoryModal");
        ars_page.getReceiptFinal();
    }
    
    this.getReceiptFinal = function()
    {
        if (ars_page._generatereceipt == false)
        {
            if (confirm("Do you want to view transaction receipt?"))
            {
                ars_page._generatereceipt = true;
                show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OTC&tranvalue=' + ars_page._txnseqid, "close" , '100%', '100%', '0', null, null, true);
            }
        }
        ars_page.removeValue("spnRName", true);
        ars_page.removeValue("spnRAddress", true);
        ars_page.removeValue("spnROccupation", true);
        ars_page.removeValue("spnRDOB", true);
        ars_page.removeValue("spnRID", true);
        ars_page.removeValue("spnBName", true);
        ars_page.removeValue("spnBAddress", true);
        ars_page.removeValue("spnBZipCode", true);
        ars_page.removeValue("spnBDOB", true);
        ars_page.removeValue("spnBContactNo", true);
        ars_page.removeValue("txtForex", false);
        ars_page.removeValue("txtNetAmount", false);
        ars_page.removeValue("txtChargeAmount", false);
        ars_page.removeValue("txtGrossAmount", false);
        ars_page.removeValue("txtNetToBenAmount", false);
        ars_page.removeValue("spnNetCurrency", true);
        ars_page.removeValue("spnChargeCurrency", true);
        ars_page.removeValue("spnGrossCurrency", true);
        ars_page.removeValue("spnNetToBenCurrency", true);
        ars_page.clearDDL("ddlProduct");
        ars_page.clearDDL("ddlRemittingCurrency");
        ars_page.clearDDL("ddlChargeType");
        ars_page.clearDDL("ddlPaymentMethod");
        ars_page.clearDDL("ddlPurpose");
        ars_page.closeSession();
    }
    
    this.showLink = function(param)
    {
        switch (param)
        {
            case "REMITTER":
                ars_page.redirectToCIF("R");
                break;
                
            case "BEENFICIARY":
                ars_page.redirectToCIF("B");
                break;
                
            case "PRODUCT":
                ars_page.redirectToCIF("P");
                break;
        }
    }
    
    this.redirectToCIF = function(module)
    {
        IRSWorld.ARS.ARSW.redirectToCIF(module);
        location.href = "../Internet/InternetRemitterARS.aspx";
    }
    
    /************************/
    /*** AJAX CALLBACK    ***/
    /************************/
    
    this.getSession = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Validating CIF. Please wait...", false);
        return IRSWorld.ARS.ARSW.getSession(this.getSession_callback);
    }
    
    this.getSession_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == "0")
            {
                ars_page.closeSession();
            }
        }
        ars_page.showLoading("", true);
    }
    
    this.getBranchCode = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Agent Branch. Please wait...", false);
        return IRSWorld.ARS.ARSW.getLocation(this.getBranchCode_callback);
    }
    
    this.getBranchCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page._branchcode = response;
        }
        ars_page.showLoading("", true);
    }
    
    this.getTranxDate = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Transaction Date. Please wait...", false);
        return IRSWorld.ARS.ARSW.getTranxDate(this.getTranxDate_callback);
    }
    
    this.getTranxDate_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page._tranxdate = response;
        }
        ars_page.showLoading("", true);
    }
    
    this.getPreReceiptHeader = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Please wait...", false);
        return IRSWorld.ARS.ARSW.getPreReceiptHeader(this.getPreReceiptHeader_callback);
    }
    
    this.getPreReceiptHeader_callback = function(result)
    {
        var response = result.value;
        var address = "";
        var count = 0;
        
        if (response != null && response != "" && response != undefined)
        {
            ars_page.enterValue("Philippine National Bank " + response.Rows[0]["Branch"], "spnBranch", true);
            ars_page.enterValue(response.Rows[0]["Description"] + "<br><p>", "spnDescription", true);
            ars_page.enterValue(response.Rows[0]["Email"], "spnEmail", true);
            
            for (var i = 0; i <= (response.Rows.length - 1); i++)
            {
                count = count + 1;
                if (count <= 3)
                {
                    address = response.Rows[i]["SubBranch"] + "<br>" + response.Rows[i]["Address"] + "<br>Tel:" + response.Rows[i]["TelNo"];
                    ars_page.enterValue(address, "spnBranch" + count, true);
                    return;
                }
                else
                {
                    break;
                }
            }
            ars_page.showLoading("", true);
        }
    }
    
    this.getRemittingCurrency = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Remitting Currency. Please wait...", false);
        return IRSWorld.ARS.ARSW.getRemittingCurrency(this.getRemittingCurrency_callback);
    }
    
    this.getRemittingCurrency_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page.fillDDL(response, "ddlRemittingCurrency", "FldCode", "FldValue", "", "");
        }
        ars_page.showLoading("", true);
    }
    
    this.getPaymentMethod = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Payment Method. Please wait...", false);
        return IRSWorld.ARS.ARSW.getPaymentMethod(this.getPaymentMethod_callback);
    }
    
    this.getPaymentMethod_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page.fillDDL(response, "ddlPaymentMethod", "FldDescription", "FldValue", "", "");
        }
        ars_page.showLoading("", true);
    }
    
    this.getPurpose = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Transaction Purpose. Please wait...", false);
        return IRSWorld.ARS.ARSW.getPurpose(this.getPurpose_callback);
    }
    
    this.getPurpose_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page.fillDDL(response, "ddlPurpose", "FldDescription", "FldValue", "", "");
        }
        ars_page.showLoading("", true);
    }
    
    this.getCIFInformation = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving CIF Information. Please wait...", false);
        return IRSWorld.ARS.ARSW.getCIFInformation(this.getCIFInformation_callback);
    }
    
    this.getCIFInformation_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page._remitterid = response.Rows[0]["RemitterID"];
            ars_page._beneficiaryid = response.Rows[0]["BeneficiaryID"];
            ars_page._remitteraddressid = response.Rows[0]["RemitterAddressSEQID"];
            ars_page._remittercontactid = response.Rows[0]["RemitterContactSEQID"];
            ars_page._beneficiaryaddressid = response.Rows[0]["BeneficiaryAddressSEQID"];
            ars_page._beneficiarycontactid = response.Rows[0]["BeneficiaryContactSEQID"];
            ars_page._osa = response.Rows[0]["OSA"];
            ars_page.enterValue(response.Rows[0]["RemitterName"], "spnRName", true);
            ars_page.enterValue(response.Rows[0]["RemitterAddress"], "spnRAddress", true);
            ars_page.enterValue(response.Rows[0]["RemitterOccupation"], "spnROccupation", true);
            ars_page.enterValue(response.Rows[0]["RemitterDOB"], "spnRDOB", true);
            ars_page.enterValue(response.Rows[0]["RemitterIDNo"], "spnRID", true);
            ars_page.enterValue(response.Rows[0]["BeneficiaryName"], "spnBName", true);
            ars_page.enterValue(response.Rows[0]["BeneficiaryAddress"], "spnBAddress", true);
            ars_page.enterValue(response.Rows[0]["BeneficiaryZipCode"], "spnBZipCode", true);
            ars_page.enterValue(response.Rows[0]["BeneficiaryDOB"], "spnBDOB", true);
            ars_page.enterValue(response.Rows[0]["BeneficiaryContactNo"], "spnBContactNo", true);
            ars_page.fillDDL(response, "ddlProduct", "ProductCode", "AccountSEQID|TargetCurrencyCode|TargetCurrency|ProductCodeID|ImplementationTypeID|ProductSEQID|AccountNo", "", "");
            ars_page.getSelectedProduct();
        }
        ars_page.showLoading("", true);
    }
    
    this.getSelectedProduct = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Product. Please wait...", false);
        return IRSWorld.ARS.ARSW.getSelectedProduct(this.getSelectedProduct_callback);
    }
    
    this.getSelectedProduct_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page.findDDLHash("ddlProduct", response, true, "|");
            ars_page.getDDLAjax("ddlProduct");
        }
        ars_page.showLoading("", true);
    }
    
    this.getChargeType = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Charge Type. Please wait...", false);
        return IRSWorld.ARS.ARSW.getChargeType(ars_page._productcode, this.getChargeType_callback);
    }
    
    this.getChargeType_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page.fillDDL(response, "ddlChargeType", "FldDescription", "FldValue", "", "");
        }
        ars_page.showLoading("", true);
    }
    
    this.getForex = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Foreign Exchange Rate. Please wait...", false);
        return IRSWorld.ARS.ARSW.getForex(ars_page._remittingcurrency, ars_page._targetcurrencycode, this.getForex_callback);
    }
    
    this.getForex_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page._forex = response.Rows[0]["SourceToTarget"];
            ars_page.enterValue(ars_page._forex, "txtForex", false);
        }
        ars_page.showLoading("", true);
    }
    
    this.getCharges = function(recompute)
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Retrieving Charges Computation. Please wait...", false);
        
        if (!recompute)
        {
            ars_page.enterValue(ars_page._forex, "txtForex", false);
        }
        return IRSWorld.ARS.ARSW.getCharges(
                        ars_page._amount,
                        ars_page._computation,
                        ars_page._productcode, 
                        ars_page._implementationtype, 
                        ars_page._remittingcurrency, 
                        ars_page._targetcurrencycode, 
                        ars_page._chargetype, 
                        ars_page._paymode, 
                        recompute, 
                        this.getCharges_callback);
    }
    
    this.getCharges_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response != "ERROR")
            {
                ars_page._calc = response.split('|');
                ars_page._grossamount = ars_page._calc[24];
                ars_page._netamount = ars_page._calc[27];
                ars_page._nettobenamount = ars_page._calc[29];
                ars_page._chargeamount = ars_page._calc[31];
                ars_page.enterValue(ars_page._grossamount, "txtGrossAmount", false);
                ars_page.enterValue(ars_page._netamount, "txtNetAmount", false);
                ars_page.enterValue(ars_page._nettobenamount, "txtNetToBenAmount", false);
                ars_page.enterValue(ars_page._chargeamount, "txtChargeAmount", false);
                if (ars_page._calc[36] != "")
                {
                    ars_page.enterValue(ars_page._calc[36], "spnGrossCurrency", true);
                    ars_page.enterValue(ars_page._calc[36], "spnNetCurrency", true);
                    ars_page.enterValue(ars_page._calc[36], "spnChargeCurrency", true);
                }
                if (ars_page._calc[39].toLowerCase() == "true")
                {
                    ars_page.showAlert("Override Successful");
                }
            }
            else
            {
                ars_page.showAlert("No charge maintained for the combination selected.");
            }
        }
        ars_page.showLoading("", true);
    }
    
    this.override = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Setting Override Permission. Please wait...", false);
        return IRSWorld.ARS.ARSW.setOverride(
                        ars_page._forex.toString(), 
                        ars_page.getValue("txtForex", false), 
                        ars_page._chargeamount.toString(), 
                        ars_page.getValue("txtChargeAmount", false), 
                        this.override_callback);
    }
    
    this.override_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page.getCharges(true);
        }
        ars_page.showLoading("", true);
    }
    
    this.validateAccountStatus = function()
    {
        if (ars_page._accountno != "" && ars_page._implementationtype == "2")
        {
            ars_page.ajaxTimeout();
            ars_page.showLoading("Validating account number. Please wait...", false);
            return IRSWorld.ARS.ARSW.validateAccountStatus(
                        ars_page._accountno,
                        ars_page._beneficiaryid,
                        ars_page._targetcurrencycode,
                        ars_page._nettobenamount,
                        this.validateAccountStatus_callback);
        }
        else
        {
            ars_page.validateAgentAccount();
        }
    }
    
    this.validateAccountStatus_callback = function(result)
    {
        var response = result.value;
        var split;
        if (response != null && response != "" && response != undefined)
        {
            split = response.split('|');
            if (split[0] == "6")
            {
                ars_page.showAlert("The remittance amount of " + ars_page._targetcurrency + " " + ars_page._nettobenamount + " has reached the maximum allowable limits. The remaining acceptable remittance is P100,000. Remittance may be implemented as APAS.");
            }
            else
            {
                if (split[0] != "0")
                {
                    ars_page.showAlert("Error code: " + split[1].toString() + ". Please check the beneficiary account and try again.");
                }
                else
                {
                    ars_page.validateAgentAccount();
                }
            }
        }
        ars_page.showLoading("", true);
    }
    
    this.validateAgentAccount = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Validating Agent Account Balance. Please wait...", false);
        return IRSWorld.ARS.ARSW.validateAgentAccount(ars_page._nettobenamount, ars_page._targetcurrencycode, this.validateAgentAccount_callback);
    }
    
    this.validateAgentAccount_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == true)
            {
                ars_page.showLoading("", true);
                document.getElementById("divAggregateTop").innerHTML = "";
                document.getElementById("divAggregateTop").innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;Please Wait...";
                ars_page.load_grid_viewer();
            }
            else
            {
                ars_page.showLoading("", true);
                ars_page.showAlert("Insufficient balance on your account. Please replenish your account to continue this transaction.");
            }
        }
    }
    
    this.submitTransaction = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Creating transaction. Please wait...", false);
        return IRSWorld.ARS.ARSW.submitTransaction(
                        ars_page._remitterid.toString(), 
                        ars_page._beneficiaryid.toString(),  
                        ars_page._accountseqid.toString(), 
                        ars_page._purpose.toString(), 
                        ars_page._remitteraddressid.toString(), 
                        ars_page._remittercontactid.toString(), 
                        ars_page._beneficiaryaddressid.toString(), 
                        ars_page._beneficiarycontactid.toString(), 
                        ars_page._clearingdate.toString(), 
                        ars_page._checknumber.toString(), 
                        this.submitTransaction_callback);
    }
    
    this.submitTransaction_callback = function(result)
    {
        var response = result.value;
        var split;
        
        if (response != null && response != "" && response != undefined)
        {
            split = response.split('|');
            if (split[1] == "Successful")
            {
                if (split[2] != null)
                {
                    ars_page._referenceno = split[2].toString();
                }
                if (split[3] != null)
                {
                    ars_page._txnseqid = split[3].toString();
                }
                ars_page.showLoading("", true);
                if (split[4] != null && split[4] != "" && split[4] != undefined)
                {
                    ars_page.showAlert("Transaction successfully committed with reference number: " + ars_page._referenceno + ".\r\n\r\n FC Response: " + split[4] + "\r\n Status: " + split[6]);
                }
                else
                {
                    ars_page.showAlert("Transaction successfully committed with reference number: " + ars_page._referenceno + ".");
                }
                ars_page.getTransactionLimitForKeepingLogs();
            }
            else
            {
                ars_page.showLoading("", true);
                ars_page.showAlert("Transaction Failed: " + split[1]);
            }
        }
        //ars_page.showLoading("", true);
    }
    
    this.getPreReceipt = function(referenceno)
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Generating Pre-Receipt. Please wait...", false);
        return IRSWorld.ARS.ARSW.getPreReceipt(referenceno, this.getPreReceipt_callback);
    }
    
    this.getPreReceipt_callback = function(result)
    {
        var response = result.value;
        var exchangeRate;
        ars_page.showLoading("", true);
        
        if (response != null && response != "" && response != undefined)
        {
            exchangeRate = response.Rows[0]["RemittingCurrency"] + " to " + response.Rows[0]["TargetCurrency"];
            ars_page.enterValue(response.Rows[0]["RemitterName"], "spnTRemitterName", true);
            ars_page.enterValue(response.Rows[0]["RemitterAddress"], "spnTRemitterAddress", true);
            ars_page.enterValue(response.Rows[0]["BeneficiaryName"], "spnTBeneficiaryName", true);
            ars_page.enterValue(response.Rows[0]["BeneficiaryAddress"], "spnTBeneficiaryAddress", true);
            ars_page.enterValue(response.Rows[0]["AccountNo"], "spnTBeneficiaryAccount", true);
            ars_page.enterValue(response.Rows[0]["BranchName"], "spnTBeneficiaryBranch", true);
            ars_page.enterValue(exchangeRate, "spnTExchangeRateCurrency", true);
            ars_page.enterValue(response.Rows[0]["ExchangeRate"], "spnTExchangeRate", true);
            ars_page.enterValue(response.Rows[0]["ReferenceNo"], "spnTReferenceNo", true);            
            ars_page.enterValue(response.Rows[0]["TranxDate"], "spnTTransactionDate", true);
            ars_page.enterValue(response.Rows[0]["TranxStatus"], "spnTStatus", true);
            ars_page.enterValue(response.Rows[0]["UserID"], "spnTEncodedBy", true);
            ars_page.enterValue(response.Rows[0]["ReceiptMode"], "spnTSubType", true);
            ars_page.enterValue(response.Rows[0]["ProductCode"], "spnTProductDescription", true);
            ars_page.enterValue(response.Rows[0]["RemittingCurrency"], "spnTGrossAmountCurrency", true);
            ars_page.enterValue(response.Rows[0]["GrossAmount"], "spnTGrossAmount", true);
            ars_page.enterValue(response.Rows[0]["RemittingCurrency"], "spnTBankChargeCurrency", true);
            ars_page.enterValue(response.Rows[0]["TotalCharge"], "spnTBankCharge", true);
            ars_page.enterValue(response.Rows[0]["RemittingCurrency"], "spnNetAmountCurrency", true);
            ars_page.enterValue(response.Rows[0]["NetAmount"], "spnNetAmount", true);
            ars_page.enterValue(response.Rows[0]["TargetCurrency"], "spnTNetToBenefAmountCurrency", true);
            ars_page.enterValue(response.Rows[0]["NetToBenAmount"], "spnTNetToBenefAmount", true);
            ars_page.showPanel("divTransactionHistoryModal");
        }
    }
    
    this.getTransactionLimitForKeepingLogs = function()
    {
        ars_page.ajaxTimeout();
        ars_page.showLoading("Validating transaction for keeping logs. Please wait...", false);
        return IRSWorld.ARS.ARSW.getTransactionLimitForKeepingLogs(this.getTransactionLimitForKeepingLogs_callback);
    }
    
    this.getTransactionLimitForKeepingLogs_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            var keepinglogsamount = response.Rows[0]["AgentLogReport"]
            if (Number(parseFloat(ars_page.getValue("txtGrossAmount", false).replace(",", "")).toFixed(2)) >= Number(parseFloat(keepinglogsamount).toFixed(2)))
            {
                var confirmlog = confirm("Would you like to print daily keeping log?");
                if (confirmlog)
                {
                    show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OTCDAILY&tranvalue='+ ars_page._txnseqid, null , '100%', '100%', '0', null, null, true);
                }
            }
        }
        ars_page.getPreReceipt(ars_page._referenceno);
        ars_page.showLoading("", true);
    }
    
    this.sendOREmail = function()
    {
        ars_page.hidePanel("divTransactionHistoryModal");
        ars_page.ajaxTimeout();
        ars_page.showLoading("Sending transaction receipt thru email. Please wait...", false);
        return IRSWorld.ARS.ARSW.sendOREmail(ars_page._referenceno, ars_page._txnseqid, this.sendOREmail_callback);
    }
    
    this.sendOREmail_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            ars_page.showAlert(response);
            ars_page.getReceiptFinal();
        }
        ars_page.showLoading("", true);
    }
    
    this.closeSession = function()
    {
        ars_page.clearAll();
        return IRSWorld.ARS.ARSW.closeSession(this.closeSession_callback);
    }
    
    this.closeSession_callback = function(result)
    {
        var response = result.value;
        location.href = "../Internet/InternetRemitterARS.aspx";
    }
}