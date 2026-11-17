
// **************************************************************************************** //
// *** AJAX GRID
// **************************************************************************************** //
function execute_ajaxgridview_command(param)
{
    if (internet_page != null && internet_page != undefined)
    {
        internet_page.do_ajaxgridview(param);
    }
}

// **************************************************************************************** //
// *** AGENT CLIENT RETURN VALUE
// **************************************************************************************** //
function internetRemitterClose()
{
    var response = self.returnValue;
    if (internet_page._isAgent == true)
    {
        internet_page.enterValue("1","lblCIFClass",false);
        internet_page.getAgentRemitter(internet_page._agentSelectedRemitterID,internet_page._agentSelectedRemitterID);
    }
    else
    {
        internet_page.getBeneficiary(internet_page._remitterid);
    }
    close_frame_dialog(false);
    // do nothing.. or alert a message of the return value which is always set to "OK".
}

// **************************************************************************************** //
// *** SHOW FRAME DIALOG RETURN VALUE
// **************************************************************************************** //
function passwordClose()
{
    var response = self.returnValue;
    close_frame_dialog(false);
}

function OAAMClose()
{
    var response = self.returnValue;
    close_frame_dialog(false);
}

// **************************************************************************************** //
// *** 3D SECURE RETURN VALUE
// **************************************************************************************** //
function internetClose(param)
{
    if (param != null && param != '')
    {
        internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
        //close_frame_dialog(false);
        if (param == "eXact")
        {
            response = IRSWorld.Internet.T__Remitter.getEXACTResponse().value;
        }
        
        //var response = IRSWorld.Internet.T__Remitter.getEnetsResponse().value;
        var arrResponse = "";
        
        if (response != null && response != "" && response != undefined && response != "|||" && response != "||||")
        {            
            arrResponse = response.split("|");
            if (param == "eXact") // Cannot test yet ======================================================
            {
                arrResponse = response.split(";");
                if (arrResponse[1].indexOf('Transaction has been approved') > "-1")
                {
                    internet_page.getORNo();
                    internet_page.enterValue(arrResponse[6], "lblAuthCode", false);
                    internet_page.enterValue(arrResponse[1], "lblStatusDetail", false);
                    internet_page.enterValue(arrResponse[9], "spnTIDCCard", false);
                    internet_page.enterValue(arrResponse[5], "spnTIExpiryDate", false);
                    internet_page.enterValue(arrResponse[8], "spnTICardName", false);
                    internet_page.enterValue(arrResponse, "lblPGMessage", false);
                    internet_page.edButtons("btnSubmitTransaction", false);
                    internet_page.edButtons("btnCancelTransaction", false);
                    internet_page.hidePanel("divSecurity");
                    internet_page.showPGLoading("", true);
                    close_frame_dialog(false);
                }
                else
                {
                    internet_page.showLoading("", true);
                    internet_page.showPGLoading("", true);
                    internet_page.removeValue("txtSecurityCode", false);
                    internet_page.edButtons("btnSubmitTransaction", false);
                    internet_page.edButtons("btnCancelTransaction", false);
                    internet_page.showAlert("There transaction received an error. \r\n\r\n" + arrResponse[2] + "\r\n\r\n Please report to the nearest PNB branch.");
                    internet_page.hidePanel("divSecurity");
                    internet_page.showTransactionLoading("", true);
                    internet_page.createTransaction('create');
                }
                IRSWorld.Internet.T__Remitter.endEXACTSession();
            }
            
            
            
            //IRSWorld.Internet.T__Remitter.endEnetsSession();
        }
        else
        {
            internet_page.showLoading("", true);
            internet_page.showPGLoading("", true);
            internet_page.removeValue("txtSecurityCode", false);
            internet_page.edButtons("btnSubmitTransaction", false);
            internet_page.edButtons("btnCancelTransaction", false);
            internet_page.showAlert("There transaction received an error. \r\n\r\n" + arrResponse[2] + "\r\n\r\n Please report to the nearest PNB branch.");
            internet_page.hidePanel("divSecurity");
            internet_page.showTransactionLoading("", true);
        }
    }
    else
    {
        internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
        close_frame_dialog(false);
        var response = self.returnValue;
        var arrResponse = "";
        
        if (response != null && response != "" && response != undefined && response != "|||" && response != "||||")
        {
            arrResponse = response.split("|");
            if (arrResponse[0] == "OK")
            {
                internet_page.getORNo();
                internet_page.enterValue(arrResponse[1], "lblAuthCode", false);
                internet_page.enterValue(arrResponse[2], "lblStatusDetail", false);
                internet_page.enterValue(arrResponse[3], "lblPGMessage", false);
                internet_page.edButtons("btnSubmitTransaction", false);
                internet_page.edButtons("btnCancelTransaction", false);
                internet_page.hidePanel("divSecurity");
                internet_page.showPGLoading("", true);
            }
            else
            {
                internet_page.showLoading("", true);
                internet_page.showPGLoading("", true);
                internet_page.removeValue("txtSecurityCode", false);
                internet_page.edButtons("btnSubmitTransaction", false);
                internet_page.edButtons("btnCancelTransaction", false);
                internet_page.showAlert("There transaction received an error. \r\n\r\n" + arrResponse[2] + "\r\n\r\n Please report to the nearest PNB branch.");
                internet_page.hidePanel("divSecurity");
                internet_page.showTransactionLoading("", true);
            }
        }
        else
        {
            internet_page.showLoading("", true);
            internet_page.showPGLoading("", true);
            internet_page.removeValue("txtSecurityCode", false);
            internet_page.edButtons("btnSubmitTransaction", false);
            internet_page.edButtons("btnCancelTransaction", false);
            internet_page.showAlert("There transaction received an error. \r\n\r\n" + arrResponse[2] + "\r\n\r\n Please report to the nearest PNB branch.");
            internet_page.hidePanel("divSecurity");
            internet_page.showTransactionLoading("", true);
        }
    }
}

// **************************************************************************************** //
// *** Internet Page
// **************************************************************************************** //
function internet_page()
{
    // **************************************************************************************** //
    // *** Variables
    // **************************************************************************************** //
    var tblCards                        = "";
    var tblPNBAccounts                  = "";
    var tblBeneficiary                  = "";
    var tblBeneficiaryAddress           = "";
    var tblBeneficiaryContacts          = "";
    var tblDefaults                     = "";
    var tblRemittance                   = "";
    var tblRemittanceHistory            = "";
    var tblChargesDefault               = "";
    var tblForex                        = "";
    var tblCurrency                     = "";
    var tblBranchOperations             = "";
    var tblComputation                  = "";
    var tblChargeType                   = "";
    var _remittance                     = "";
    var _lookupField                    = "";
    var _calc                           = "";
    var _arrCalc                        = "";
    var _accept                         = false;
    var _beneficiary                    = false;
    var _receiptMode                    = "5"; //Online (Internet)
    var _isDC                           = "0"; //0 = none, 1 = DC, 2 = PNBE
    var _internetLimit                  = "0";
    var _chargeIsValid                  = false;
    var _sql                            = "";
    var _griddiv                        = "";
    var _gridrow                        = 15;
    var _computation                    = "1";
    var _isAgent                        = false;
    var _showHistory                    = false;
    var _netToBenefCurrency             = "";
    var _isoverride                     = false;
    var _agentSelectedRemitterID        = "";
    var _agentSelectedBeneficiary       = "";
    var _exchangeRateValid              = false;
    var _auditRemittance                = "";
    var _agentIsApprover                = false;
    var _agentProcessor                 = "";
    var _agentApprover                  = "";
    var _agentApproverProcessor         = "";
    var _tranStatus                     = "";
    var _productcode                    = "";
    var _implementationtype             = "";
    var _targetcurrency                 = "";
    var _isOSA                          = false;
    var _branchCode                     = "";
    var _refno                          = "";
    var _erefno                         = "";
    var _isSearch                       = false;
    var _securityCode                   = "";
    var _userLocation                   = "";
    var _accountno                      = "";
    var _fcSuccess                      = "";
    var _isSubmit                       = "";
    var _amountPG                       = "";
    var _remittingCurrency              = "";
    var _txnseqid                       = "";
    var _readme                         = "";
    var _remitterid                     = "";
    var _expiringOK                     = false;
    var _beneficiaryAddressSEQID        = "";
    var _beneficiaryContactSEQID        = "";
    var _beneficiaryEmailSEQID          = "";
    var _ARow                           = 0;
    var _CRow                           = 0;
    var _ERow                           = 0;
    var _loginID                        = "";
    var _viptype                        = 0;
    var _vippercent                     = 0;
    var _chargeResponse                 = "";
    var _checktransaction               = 0;
    var _submittransaction              = 0;
    var _transmitremittance             = 0;
    var _timeout                        = null;
    var _remaining_balance              = 0;
    var _totalremittance                = 0;
    var _sourcecurrency                 = "";
    var _maxstraight                    = 0;
    var benef_param                     = new Array();
    
    var _chargetype                     = "";
    var _paymode                        = "";

    // **************************************************************************************** //
    // *** AJAX GRID
    // **************************************************************************************** //
    this.do_ajaxgridview = function(parameter)
    {
        if (parameter != null && parameter != undefined)
        {
            var command_param = parameter.split("$");
            if (command_param.length > 0)
            {
                if (command_param[0] == "ajx_selected")
                {
                    if (internet_page._isAgent == true)
                    {
                        if (internet_page._showHistory == true)
                        {
                            this.getRemittance(command_param[1]);
                        }
                        else
                        {
                            internet_page.isSearch = true;
                            var arrSplit = command_param[1].split("|");
                            internet_page._agentSelectedRemitterID = arrSplit[0];
                            internet_page._agentSelectedBeneficiary = arrSplit[1] + "|" + arrSplit[2] + "|" + arrSplit[3] + "|" + arrSplit[4] + "|" + arrSplit[5];
                            if (internet_page.getValue("lblCIFClass", false) == "1") //Remitter
                            {
                                this.getAgentRemitter(arrSplit[0], arrSplit[0]); // [0] = Remitter
                                this._remaining_balance = IRSWorld.Internet.T__Remitter.getRemainingRemittance(internet_page._agentSelectedRemitterID).value;
                                document.getElementById('txtTotalRemitted').value = this._remaining_balance;
                            }
                            else
                            {
                                this.getAgentRemitter(arrSplit[1], arrSplit[0]); // [1] = Beneficiary
                            }
                        }
                    }
                    else
                    {
                        this.getRemittance(command_param[1]);
                    }
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
        if (internet_page._isAgent == true)
        {
            if (internet_page._showHistory == true)
            {
                internet_page._sql      = "isp_getTransactionHistory~" + internet_page.getValue("lblRemitterID", false);
                internet_page._griddiv  = "divRemitterTransaction";
                internet_page._gridrow  = 15;
            }
            else
            {
                internet_page._sql      = "isp_getTDDCIF~" + internet_page._userLocation + "~" + internet_page.getValue("lblCIFClass", false)  + "~" + internet_page.getValue("txtSLastName", false) + "~" + internet_page.getValue("txtSFirstName", false) + "~" + internet_page.getValue("txtSMiddleName", false);
                internet_page._griddiv  = "divSearchGridTop";
                internet_page._gridrow  = 10;
            }
        }
        else
        {
            internet_page._sql      = "isp_getTransactionHistory~" + internet_page.getValue("lblRemitterID", false);
            internet_page._griddiv  = "divRemitterTransaction";
            internet_page._gridrow  = 15;
        }

        this.grid = new grid_viewer();
        this.grid.sql_source        = internet_page._sql;
        this.grid.div_container     = document.getElementById(internet_page._griddiv);
        this.grid.datarow_column    = 0;
        this.grid.page_size         = internet_page._gridrow;
        this.grid.grd_header_back_color = "#26afe5";
        this.grid.grd_header_fore_color = "#FFFFFF";
        this.grid.grd_customed_date = true;
        this.grid.process_ajax(parameter);
        this.grid                   = null;
    }
    
    // **************************************************************************************** //
    // *** Common Functions and Methods
    // **************************************************************************************** //
    this.fixdim = function()
    {
        if (internet_page._isprinting == false)
        {
            if (top.document.getElementById('divCenter'))
            {
                var div = document.getElementById('divPage');
                div.style.height = (top.document.getElementById('divCenter').offsetHeight) + 'px';
                div.style.width = (top.document.getElementById('divCenter').offsetWidth + 5) + 'px';
                if (document.getElementById('divInternetPageMenu'))
                {
                    document.getElementById('tdInternetPagemenu').style.width = '245px';
                    document.getElementById('tdInternetPagemenu').style.height = div.style.height;
                    document.getElementById('tdInternetPageCenter').style.width = (parseInt(div.style.width) - 725) + 'px';
                    document.getElementById('tdInternetPageCenter').style.height = div.style.height;                    
                    
                    var div_menu = document.getElementById('divInternetPageMenu');
                    div_menu.style.height = (parseInt(div.style.height)) + 'px';
                    div_menu.style.width = document.getElementById('tdInternetPagemenu').style.width;
                }
            }
        }
    }
    
    this.ajaxTimeout = function()
    {
        AjaxPro.timeoutPeriod = 1000 * 1000;
        AjaxPro.onTimeout = function()
        {
            internet_page.showAlert("THE SERVER STOPPED RESPONDING. PLEASE TRY AGAIN");
        }
    }
    
    this.isNumberKey = function(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57))
        return false;

        return true;
    }
    
    this.isNumberKeyPeriod = function(evt)
    {
        var charCode = (evt.which) ? evt.which : event.keyCode
        if (charCode > 31 && charCode != 46 && (charCode < 48 || charCode > 57))
        return false;

        return true;
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
    
    this.formatdate = function(date)
    {
        var arrDate = date.split("/"); //year/month/day
        return arrDate[1] + "/" + arrDate[2] + "/" + arrDate[0];
    }
    
    this.computeMonths = function(dt1, dt2)
    {
        //var datediff = dt2 - dt1;
        //return Math.round((datediff % 31536000000)/2628000000);
        //datediff/31536000000; //year
        //((datediff % 31536000000) % 2628000000)/86400000; //day
        var dateRegEx = /^([01]?\d)[\.\-\/\s]?([0123]?\d)[\.\-\/\s]?(\d{4})$/;
        var result1 = dt1.match(dateRegEx);
        var result2 = dt2.match(dateRegEx);
        
        if (result1 != null)
        {
            var month1 = result1[1];
            var day1 = result1[2];
            var year1 = result1[3];
        }
        
        if (result2 != null)
        {
            var month2 = result2[1];
            var day2 = result2[2];
            var year2 = result2[3];
        }
        
        if (result1 && result2)
        {
            var dif = 0;
            dif += (month2 - month1);
            dif += ((year2 - year1) * 12);
            dif += (day2 - day1 > 0) ? 1 : 0;            
            return dif;
        }
        return null;
    }
    
    this.autoFormatCurrency = function(fld, milSep, decSep, e)
    {
        var sep = 0;
        var key = "";
        var i = j = 0;
        var len = len2 = 0;
        var strCheck = "0123456789";
        var aux = aux2 = "";
        var whichCode = (window.event) ? e.keyCode : e.which;

        if (whichCode == 13) return true; // Enter
        if (whichCode == 8) return true; // Delete
        if (whichCode == 9) return true; // Tab
        if (whichCode == 11) return true; // Tab
        key = String.fromCharCode(whichCode); // Get key value from key code
        if (strCheck.indexOf(key) == -1) return false; // Not a valid key
        len = fld.value.length;
        
        for(i = 0; i < len; i++)
        if ((fld.value.charAt(i) != "0") && (fld.value.charAt(i) != decSep)) break;
        aux = "";
        for(; i < len; i++)
        if (strCheck.indexOf(fld.value.charAt(i))!=-1) aux += fld.value.charAt(i);
        aux += key;
        len = aux.length;
        if (len == 0) fld.value = "";
        if (len == 1) fld.value = "0" + decSep + "0" + aux;
        if (len == 2) fld.value = "0" + decSep + aux;
        if (len > 2) 
        {
            aux2 = '';
            for (j = 0, i = len - 3; i >= 0; i--) 
            {
                if (j == 3) 
                {
                    aux2 += milSep;
                    j = 0;
                }
                aux2 += aux.charAt(i);
                j++;
            }
            fld.value = "";
            len2 = aux2.length;
            for (i = len2 - 1; i >= 0; i--)
            fld.value += aux2.charAt(i);
            fld.value += decSep + aux.substr(len - 2, len);
            if (parseFloat(fld.value) > 0)
            {
                internet_page.edButtons("btnSubmit", false);
            }
            else
            {
                internet_page.edButtons("btnSubmit", true);
            }
        }
        return false;
    }
    
    this.formatCurrency = function(num, places)
    {
        num = num.toString().replace(/\$|\,/g,'');
        var amount = num.toString();
        var count = amount.indexOf(".");
        var cents;
        
        if (count == -1)
        {
            cents = "00";
        }
        else
        {
            cents = amount.substring(100, count + 1);
            if (cents.length == 1)
            {
                cents = cents + "0";
            }
            else
            {
                cents = cents.substring(2, 0);
            }
        }
        if (isNaN(num) == false)
        {
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            //cents = num % 100;
            num = Math.floor(num / 100).toString();
            len = 0;
            //cents = cents.toString();
            len = cents.length;
            
            /*
            for (var x = 0; x < (places - len); x++)
            {
                cents = cents + "0";
            }
            */
            
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
            {
                num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
            }
            return (((sign) ? "" : "-") + num + "." + cents);
        }
    }
    
    this.submitQuit = function()
    {
        if (confirm("Are you sure you want to exit?"))
        {
            window.close();
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
      // alert(value);alert(document.getElementById(element));
        (isspan == true) ? document.getElementById(element).innerHTML = value : document.getElementById(element).value = value;
    }

    this.removeValue = function(element, isspan)
    {
        (isspan == true) ? document.getElementById(element).innerHTML = "" : document.getElementById(element).value = "";
    }

    this.edButtons = function(button, isdisable)
    {
        if (isdisable == true)
        {
            document.getElementById(button).disabled = "disabled";
        }
        else
        {
            document.getElementById(button).disabled = "";
        }
    }
    
    this.roButtons = function(button, isdisable)
    {
        if (isdisable == true)
        {
            document.getElementById(button).readOnly = true;
        }
        else
        {
            document.getElementById(button).readOnly = false;
        }
    }
    
    this.isChecked = function(button)
    {
        return (document.getElementById(button).checked == true) ? true : false;
    }

    this.showFocus = function(element)
    {
        document.getElementById(element).focus();
    }
    
    this.showLoading = function(message, ishide)
    {
        var span = document.getElementById("spnLoading");
        if (ishide == true)
        {
            span.innerHTML = "";
        }
        else
        {
            span.innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;" + message;
        }
    }
    
    this.showAlert = function(message)
    {
        (message != "") ? alert(message) : alert("An error occured while getting information. Please report to the nearest PNB branch.");
    }
    
    this.closeWindow = function()
    {
        window.close();
    }
    
    this.concatString = function(table, element, value, concatvalue)
    {
        var returnvalue = "";
        
        for (var i = 0; i <= (table.Rows.length - 1); i++)
        {
            returnvalue = returnvalue + table.Rows[i][value] + concatvalue;
        }
        returnvalue = returnvalue.substring(0, returnvalue.length - 1);
        internet_page.enterValue(returnvalue, element, false);
    }
    
    this.getTableValue = function(table, fieldValue)
    {
        var arr = new Array();
        for (var i = 0; i <= (table.Rows.length - 1); i++)
        {
            arr = table.Rows[i];
            return arr[fieldValue];
        }
    }
    
    this.getTableValueRow = function(table, fieldValue, row)
    {
        var arr = new Array();
        if (table.Rows.length != -1)
        {
            arr = table.Rows[row];
            return arr[fieldValue];
        }
    }

    this.findValue = function(table, matchValue, fieldValue, returnField)
    {
        var returnValue = "";
        var iRow = table;
        var sMatchValue = matchValue.split("|");
        var sFieldValue = fieldValue.split("|");
        var boolMatch = new Array();
        var iOk = 0;
        var count = 0;
        
        if (iRow.Rows.length > 0)
        {
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var j = 0; j <= (sMatchValue.length - 1); j++)
                {
                    if (iRow.Rows[i][sFieldValue[j]] != null)
                    {
                        if (internet_page.trim(iRow.Rows[i][sFieldValue[j]].toString()) == internet_page.trim(sMatchValue[j].toString()))
                        {
                            boolMatch[count] = true;
                            count = count + 1;
                        }
                    }
                }
                if (boolMatch[0] != undefined && boolMatch[0] != null)
                {
                    if (boolMatch.length > 0)
                    {
                        for (var k = 0; k <= (boolMatch.length - 1); k++)
                        {
                            if (boolMatch[k] == true)
                            {
                                iOk = iOk + 1;
                            }
                        }
                        if (iOk == sMatchValue.length)
                        {
                            returnValue = iRow.Rows[i][returnField];
                            break;
                        }
                        else
                        {
                            iOk = 0;
                            count = 0;
                            boolMatch = new Array();
                        }
                    }
                }
            }
        }
        return returnValue;
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
            //benef_param = new Array();
            select.options[0] = new Option("", "");
            for (var i = 0; i <= (iRow.Rows.length - 1); i++)
            {
                for (var arr = 0; arr <= (arrSplit.length - 1); arr++)
                {
                    if ((arrSplit.length - 1) == arr)
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && internet_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
                        {
                            arrValue = arrValue + iRow.Rows[i][arrSplit[arr]];
                        }
                    }
                    else
                    {
                        if (iRow.Rows[i][arrSplit[arr]] != null && internet_page.trim(iRow.Rows[i][arrSplit[arr]]) != "")
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
                if(element == "ddlBeneficiary")
                {
                    benef_param[benef_param.length] = arrValue;
                    select.options[i + 1] = new Option(arrValue.split(",")[0], arrIDValue);
                }
                else
                {
                     select.options[i + 1] = new Option(arrValue, arrIDValue);
                }
                
                //select.options[i + 1] = new Option(arrValue, iRow.Rows[i][value]);
               
                arrValue = "";
                arrIDValue = "";
            }
            internet_page.getDDL(element, selected, true);
        }
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
                    internet_page.getDDL(element, selected, true);
                    break;
                }
            }
            else
            {
                if (select.options[i].text == findvalue)
                {
                    select.selectedIndex = i;
                    internet_page.getDDL(element, selected, true);
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
    
    this.isNumeric = function(value)
    {
        if (value == parseInt(value))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    // **************************************************************************************** //
    // *** Page Functions and Methods
    // **************************************************************************************** //
    this.showPGLoading = function(message, ishide)
    {
        var span = document.getElementById("spnPGLoading");
        if (ishide == true)
        {
            span.innerHTML = "";
        }
        else
        {
            span.innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;" + message;
        }
    }
    
    this.showTransactionLoading = function(message, ishide)
    {
        if (ishide == true)
        {
            document.getElementById("lblLoadingMsg").innerHTML = "";
            internet_page.hidePanel("divPopProcess");
        }
        else
        {
            new common_method().to_show_loading_message('divLoadingMsg');
            document.getElementById("lblLoadingMsg").innerHTML = message;
            internet_page.showPanel("divPopProcess");
        }
    }
    
    this.showCIFLoading = function(message, ishide)
    {
        var span = document.getElementById("spnCIFLoading");
        if (ishide == true)
        {
            span.innerHTML = "";
        }
        else
        {
            span.innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;" + message;
        }
    }
    
    this.showAccountLoading = function(message, ishide)
    {
        var span = document.getElementById("spnAccountLoading");
        if (ishide == true)
        {
            span.innerHTML = "";
        }
        else
        {
            span.innerHTML = "<IMG SRC = '../images/ajaxloader.gif'></IMG>&nbsp;" + message;
        }
    }
    
    this.getDDLAjax = function(element, selected, isValue)
    {    
        //this.message_tat_prompt();
    
        switch (element)
        {
            case "Amount":
                if (internet_page._isAgent == true)
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0 && document.getElementById("ddlChargeType").selectedIndex)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                    else
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                break;
                
                
            case "ddlBenefName":
                if (document.getElementById("ddlBenefName").selectedIndex > 0)
                {
                    var beneficiary_value = internet_page.getDDLValue("ddlBenefName", true).split("|");
                    internet_page.showTransactionLoading("Please wait...", false);
                    internet_page.getProducts(internet_page.getValue("lblRemitterID", false),beneficiary_value[0]);
                    internet_page.showTransactionLoading("", true);
                }
 
            case "ddlBeneficiary":
                if (document.getElementById("ddlBeneficiary").selectedIndex > 0)
                {
                    internet_page.showTransactionLoading("Please wait...", false);
                    internet_page.getDDL("ddlBeneficiary", "lblBeneficiaryInfo", true);
                    internet_page.setBeneficiaryInfo();
                    internet_page.showTransactionLoading("", true);
                    if (internet_page._isAgent == true)
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0 && document.getElementById("ddlChargeType").selectedIndex)
                        {
                           // document.getElementById("ddlComputation").selectedIndex = -1;
                            internet_page.getCharges(false);
                        }
                        internet_page.getFilteredChargeType();
                    }
                    else
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0)
                        {
                            //document.getElementById("ddlComputation").selectedIndex = -1;
                            internet_page.getCharges(false);
                        }
                    }
                    var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
                    if (beneficiary_value[1] == "8" || beneficiary_value[1] == "9") //Door to Door - Check if OSA
                    {
                        internet_page.getOSA(beneficiary_value[0]);
                    }
                    else
                    {
                        internet_page._isOSA = false;
                    }
                }
                break;
            case "ddlPurpose":
                if (document.getElementById("ddlPurpose").selectedIndex > 0)
                {
                    internet_page.getDDL("ddlPurpose", "lblPurpose", true);
                }
                break;
            case "ddlPaymentMethod":
                if (document.getElementById("ddlPaymentMethod").selectedIndex > 0)
                {
                    if (internet_page._isAgent == false)
                    {
                        if (internet_page.getDDLValue("ddlPaymentMethod").match("Cash"))
                        {
                            internet_page.showAlert("Please select another payment method. Cash is not allowed on this transaction");
                            document.getElementById("ddlPaymentMethod").selectedIndex = -1;
                            return false;
                        }
                    }
                    /*
                    internet_page.showTransactionLoading("Please wait...", false);
                    internet_page.getDDL("ddlPaymentMethod", "lblPaymentMethod", true);
                    internet_page.removeValue("spnSourceCurrency", true);
                    internet_page.removeValue("spnRemittingCurrency", true);
                    */
                    var paymentMethod = internet_page.getDDLValue("ddlPaymentMethod", false);
                    if(paymentMethod != "ACH" && paymentMethod != "US Savings/ Checking Account")
                    {
                        if (paymentMethod.match("Debit"))
                        {
                            internet_page._isDC = "1";
                            internet_page.getDCCard();
                            internet_page.showControl('trOnline',  true);  
                        }
                        else if (paymentMethod.match("Bank Account"))
                        {
                            internet_page._isDC = "3";
                            //internet_page.getOtherAccounts();
                            internet_page.showControl('trOnline',  true);  
                        }
                        else if (paymentMethod.match("First Data") || paymentMethod.match("Credit Card"))
                        {
                            internet_page._isDC = "4";
                            internet_page.getOtherAccounts();
                            internet_page._ischeck = 0;
                            internet_page._isach = 0;
                            internet_page.showControl('trOnline',  false);
                            var curr = internet_page.getLookupValue("Currency","FldValue",internet_page._sourcecurrency.toString(),"FldCode");
                            internet_page.enterValue(curr, "spnRemittingCurrency", true);
                            internet_page.enterValue(curr , "spnSourceCurrency", true);
                        }
                        else
                        {
                            internet_page._isDC = "2";
                            internet_page.getPNBAccounts();
                            internet_page.showControl('trOnline',  true);  
                        }  
                        
                       
                    }
                    else
                    {
                        internet_page._isDC = "";
                        internet_page.showControl('trOnline',  false);
                    }
                    
                    
                    internet_page.showTransactionLoading("", true);
                    if (internet_page._isAgent == true)
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0 && document.getElementById("ddlChargeType").selectedIndex)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                    else
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                }
                else
                {
                    internet_page.clearDDL("ddlDCCard");
                    internet_page.removeValue("spnSourceCurrency", true);
                    internet_page.removeValue("spnRemittingCurrency", true);
                }
                break;
//            case "ddlDCCard":
//                if (document.getElementById("ddlDCCard").selectedIndex > 0)
//                {
//                    internet_page.showTransactionLoading("Please wait....", false);
//                    internet_page.getDDL("ddlDCCard", "lblRemAcctSeqID", true);
//                    internet_page.removeValue("spnSourceCurrency", true);
//                    internet_page.removeValue("spnRemittingCurrency", true);
//                    var paymentMethod = internet_page.getDDLValue("ddlPaymentMethod", false);
//                    if (paymentMethod.match("Debit"))
//                    {
//                        internet_page.enterValue(internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode"), "spnSourceCurrency", true);
//                        internet_page.enterValue(internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "Currency"), "lblSourceCurrency", false);
//                        internet_page._remittingCurrency = internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode");
//                        
//                        /*edited remove computation type*/
//                        var computationType = "1";
//                        if (computationType != "" && computationType != null && computationType != undefined)
//                        {
//                            if (computationType == "3")
//                            {
//                                internet_page.getBeneficiaryTargetCurrency();
//                            }
//                            else
//                            {
//                                internet_page.enterValue(internet_page._remittingCurrency, "spnRemittingCurrency", true);
//                            }
//                        }
//                      
//                    }
//                    else if (paymentMethod.match("Bank Account"))
//                    {
//                        internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode"), "spnSourceCurrency", true);
//                        internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "Currency"), "lblSourceCurrency", false);
//                        internet_page._remittingCurrency = internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode");
//                        
//                        var computationType = "1";
//                        if (computationType != "" && computationType != null && computationType != undefined)
//                        {
//                            if (computationType == "3")
//                            {
//                                internet_page.getBeneficiaryTargetCurrency();
//                            }
//                            else
//                            {
//                                internet_page.enterValue(internet_page._remittingCurrency, "spnRemittingCurrency", true);
//                            }
//                        }
//                        
//                    }
//                    else
//                    {
//                        internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode"), "spnSourceCurrency", true);
//                        internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "Currency"), "lblSourceCurrency", false);
//                        internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode"), "spnRemittingCurrency", true);
//                    }
//                    internet_page.showTransactionLoading("", true);
//                    if (internet_page._isAgent == true)
//                    {
//                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0 && document.getElementById("ddlChargeType").selectedIndex)
//                        {
//                            internet_page.getCharges(false);
//                        }
//                    }
//                    else
//                    {
//                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0)
//                        {
//                            internet_page.getCharges(false);
//                        }
//                    }
//                }
//                else
//                {
//                    internet_page.removeValue("spnSourceCurrency", true);
//                    internet_page.removeValue("spnRemittingCurrency", true);
//                }
//                break;
            case "ddlChargeType":
                if (document.getElementById("ddlChargeType").selectedIndex > 0)
                {
                    internet_page.showTransactionLoading("Please wait...", false);
                    internet_page.getDDL("ddlChargeType", "lblChargeType", true);
                    internet_page._chargetype = internet_page.getValue("lblChargeType", false);
                    internet_page.showTransactionLoading("", true);
                    if (internet_page._isAgent == true)
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0 && document.getElementById("ddlChargeType").selectedIndex)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                    else
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                }
                break;
            case "ddlComputation":
                
                    internet_page.showTransactionLoading("Please wait...", false);
                    internet_page.getDDL("ddlComputation", "lblComputation", true);
                    internet_page._computation = internet_page.getValue("lblComputation", false);
                    internet_page.showTransactionLoading("", true);
                    if (internet_page._isAgent == true)
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0 && document.getElementById("ddlChargeType").selectedIndex)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                    else
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0)
                        {
                            internet_page.getCharges(false);
                            var computationType = "1";
                            if (computationType != "" && computationType != null && computationType != undefined)
                            {
                                if (computationType == "3")
                                {
                                    internet_page.getBeneficiaryTargetCurrency();
                                }
                                else
                                {
                                    internet_page.enterValue(internet_page._remittingCurrency, "spnRemittingCurrency", true);
                                }
                            }
                        }
                    }
                
                break;
            case "ddlRemittingCurrency":
                if (document.getElementById("ddlRemittingCurrency").selectedIndex > 0)
                {
                    internet_page.showTransactionLoading("Please wait...", false);
                    internet_page.getDDL("ddlRemittingCurrency", "lblRemittingCurrency", true);
                    internet_page.getExchangeRate(internet_page.getValue("lblRemittingCurrency", false), internet_page.getValue("lblTargetCurrency", false));
                    internet_page.enterValue(internet_page.getValue("lblRemittingCurrency", false), "lblSourceCurrency", false);
                    internet_page.showTransactionLoading("", true);
                    if (internet_page._isAgent == true)
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0 && document.getElementById("ddlChargeType").selectedIndex)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                    else
                    {
                        if (document.getElementById("ddlBeneficiary").selectedIndex > 0 && document.getElementById("ddlPaymentMethod").selectedIndex > 0 && document.getElementById("ddlDCCard").selectedIndex > 0)
                        {
                            internet_page.getCharges(false);
                        }
                    }
                }
                break;
        }
    }
    
    this.getBeneficiaryTargetCurrency = function()
    {
        var targetCurrency = internet_page.getValue("lblTargetCurrency", false);
        var targetDescription = "";
        if (targetCurrency == "" || targetCurrency == null || targetCurrency == undefined)
        {
            internet_page.showBeneficiaryInformation(false);
        }
        targetCurrency = internet_page.getValue("lblTargetCurrency", false);
        switch (targetCurrency)
        {
            case "1":
                targetDescription = "PHP";
                break;
            case "2":
                targetDescription = "USD";
                break;
            case "5":
                targetDescription = "EUR";
                break;
        }
        internet_page.enterValue(targetDescription, "spnRemittingCurrency", true);
        
       
            
    }

    this.maskString = function(element, retainValue)
    {
        var select = document.getElementById(element); 
        var option = "";
        var trimValue = "";
        var maskValue = "";
        
        for (var i = 1; i <= (select.length - 1); i++)
        {
            option = select.options[i].text;
            for (var count = 0; count <= (option.length - 1); count++)
            {
                trimValue = option.substr(count, 1);
                if (trimValue == "-")
                {
                    maskValue = maskValue + trimValue;
                }
                else
                {
                    if (count <= (option.length - retainValue - 1))
                    {
                        maskValue = maskValue + "*";
                    }
                    else
                    {
                        maskValue = maskValue + trimValue;
                    }
                }
            }
            select.options[i].text = maskValue;
            maskValue = "";
        }
    }

    this.printDiv = function(divName)
    {
//        internet_page._isprinting = true;
//        internet_page.showControl("btnPrintPanel", false);
//        internet_page.showControl("btnClosePanel", false);
//        internet_page.showControl("btnSendReceipt", false);
//        internet_page.hidePanel("divTransactionHistoryModal");
//        var headstr = "<html><head><title></title><link href=\"../css/reskin.css\" rel=\"stylesheet\" type=\"text/css\" /><link href=\"../css/cssWeb.css\" rel=\"stylesheet\" type=\"text/css\" /></head><body>";
//        var footstr = "</body>";
//        var newstr = document.all.item(divName).innerHTML;
//        var oldstr = document.body.innerHTML;
//        document.body.innerHTML = headstr + newstr + footstr;
//        window.print();
//        document.body.innerHTML = oldstr;
//        internet_page.showControl("btnPrintPanel", true);
//        internet_page.showControl("btnClosePanel", true);
//        internet_page.showControl("btnSendReceipt", true);
        window.open('../Reports/Pages/Report_viewer.aspx?trantype=OTC&tranvalue='+ internet_page._txnseqid.toString());  
        internet_page._isprinting = false;
    }
    
    this.cancelSearch = function()
    {
        internet_page.removeValue("lblCIFClass", false);
        internet_page.showSearch("divSearchGrid", "divSearch");
    }
    
    this.showSearch = function(divhide, divshow)
    {
        if (divhide != "" && divhide != null)
        {
            internet_page.hidePanel(divhide);
        }
        if (divshow != "" && divshow != null)
        {
            internet_page.showPanel(divshow);
        }
    }
    
    this.clearSearch = function()
    {
        var select = document.getElementById("ddlSearchBy");
        select.selectedIndex = -1;
        internet_page.removeValue("txtSLastName", false);
        internet_page.removeValue("txtSFirstName", false);
        internet_page.removeValue("txtSMiddleName", false);
    }
    
    this.searchCIF = function(issearch)
    {
        if (internet_page._isAgent == true)
        {
            internet_page.showControl("divTransactionHistory", false);
            internet_page.showControl("divTransactionInfo", false);
            internet_page.showControl("divCharges",false);
            internet_page.edButtons("btnSubmit", false);
            document.getElementById("divSearchGridTop").innerHTML = "<center><span id='spnCIFLoading'></span></center>";
            internet_page.showCIFLoading("Please wait...", false);
            internet_page.showControl("btnSearch", true);
            if (issearch == true)
            {
                internet_page.showSearch("", "divSearch");
            }
            else
            {
                if (document.getElementById("ddlSearchBy").selectedIndex > 0)
                {
                    internet_page.showSearch("divSearch", "divSearchGrid");
                }
                else
                {
                    internet_page.showAlert("Please select a search filter.");
                }
            }
            internet_page.load_grid_viewer();
            internet_page.clearSearch();
        }
        else
        {
            internet_page.showAlert("This link is not available for the selected user role.");
        }
    }
    
    this.callSearch = function()
    {
        if (window.event.keyCode == 13) // Enter key
        {
            internet_page.searchCIF(false);
        }
    }
    
    this.setPaymentMethod = function(isenabled)
    {
        if (isenabled == true)
        {
            internet_page.edButtons("ddlPaymentMethod", false);
            internet_page.edButtons("ddlDCCard", false);
            internet_page.removeDDL("ddlPaymentMethod", "Cash (Notes)", false);
        }
        else
        {
            internet_page.edButtons("ddlPaymentMethod", true);
            internet_page.edButtons("ddlDCCard", true);
            internet_page.findDDL("ddlPaymentMethod", "Cash (Notes)", "lblPaymentMethod", false);
        }
    }
    
    this.showAgentControl = function(show)
    {
        if (show == true)
        {
            internet_page.showControl("trAgent1", true);
            //internet_page.showControl("trAgent2", true);
            internet_page.showControl("trAgent3", true);
            internet_page.showControl("trAgent4", true);
            internet_page.showControl("trSearchCIF", true);
            //internet_page.showControl("trAddCIF", true);
            internet_page.showControl("trOnline", false);
            internet_page.showControl("btnOverride", true);
            internet_page.showControl("btnACompute", false);
            internet_page.roButtons("txtCExchangeRate", false);
            internet_page.roButtons("txtCBankChargeAmount", false);
            //internet_page.showControl("btnCompute", true);
            internet_page.showControl("btnConfirmRem",true);
            internet_page.showControl("trTransactionHistory",false);
            internet_page.showControl("trPassword",false);
            internet_page.showControl("trOAAM",false);
            internet_page.showControl("trDislaimer",false);
            internet_page.showControl("btnRemHistory",false);
        }
        else
        {
            internet_page.showControl("trAgent1", false);
            //internet_page.showControl("trAgent2", true); //Request of Sir Yoyong 20110801
            internet_page.showControl("trAgent3", false); 
            internet_page.showControl("trAgent4", false);
            internet_page.showControl("trSearchCIF", false);
            //internet_page.showControl("trAddCIF", true); //Request of Sir Yoyong 20110801
            //internet_page.showControl("trOnline", true);
            internet_page.showControl("btnOverride", false);
            internet_page.showControl("btnACompute", false);
            internet_page.roButtons("txtCExchangeRate", true);
            internet_page.roButtons("txtCBankChargeAmount", true);
            internet_page.enterValue("Change Amount", "btnCClosePanel", false);
            document.getElementById("btnCClosePanel").style.width = "120px";
            //internet_page.showControl("btnCompute", false);
            internet_page.showControl("btnConfirmRem",true);
        }
    }
    
    this.setBeneficiaryInfo = function()
    {
        var arrBeneficiary = internet_page.getValue("lblBeneficiaryInfo", false).split("|");
        internet_page.enterValue(arrBeneficiary[0], "lblBeneficiaryID", false);
        internet_page.enterValue(arrBeneficiary[1], "lblProductCode", false);
        internet_page.enterValue(arrBeneficiary[2], "lblImplementationCode", false);
        internet_page.enterValue(arrBeneficiary[3], "lblBenAcctSeqID", false);
        internet_page.enterValue(arrBeneficiary[4], "lblTargetCurrency", false);
        internet_page._ARow = -1;
        internet_page._CRow = -1;
        internet_page._ERow = -1;
        internet_page.check_watchlist(arrBeneficiary[0], true);
        internet_page.showBeneficiaryInformation(false);
    }
    
    this.clearCompute = function()
    {
        internet_page.removeValue("txtCExchangeRate", false);
        internet_page.removeValue("spnCExchangeRateCurrency", true);
        internet_page.removeValue("spnCNetAmountCurrency", true);
        internet_page.removeValue("txtCNetAmount", false);
        internet_page.removeValue("spnCBankChargeAmountCurrency", true);
        internet_page.removeValue("txtCBankChargeAmount", false);
        internet_page.removeValue("spnCGrossAmountCurrency", true);
        internet_page.removeValue("txtCGrossAmount", false);
        internet_page.removeValue("spnCNetToBenefAmountCurrency", true);
        internet_page.removeValue("txtCNetToBenefAmount", false);
        //internet_page.removeValue("spnCPHPConvertedAmountCurrency", true);
        //internet_page.removeValue("txtCPHPConvertedAmount", false);
        internet_page.removeValue("spnNetToBeneAmount", true);
    }
    
    this.closeComputation = function()
    {
        internet_page.edButtons("btnSubmit", false)
        //internet_page.hidePanel("divCharges");
        internet_page.showControl("divCharges",false);
        internet_page.edButtons("txtAmount",false);
    }
    
    this.writeLogger = function(remittance, tablename, key)
    {
        IRSWorld.Internet.T__Remitter.writeLogger(remittance, tablename, key);
    }
    
    // **************************************************************************************** //
    // *** Page Initialize
    // **************************************************************************************** //
    this.initializePage = function()
    {

        internet_page.showInitializePage("Loading. Please wait...", false);
        
       // internet_page.showTransactionLoading("", true);
       
    }
    this.showInitializePage = function(message, ishide)
    {
        internet_page.ajaxTimeout();
        this.fixdim();
        new common_method().to_show_loading_message('divLoadingMsg');
        document.getElementById("lblLoadingMsg").innerHTML = message;
        internet_page.showPanel("divPopProcess");
        internet_page._accept                   = false;
        internet_page._isOSA                    = false;
        internet_page._isSearch                 = false;
        internet_page._isSubmit                 = false;
        internet_page._expiringOK               = false;
        internet_page._isprinting               = false;
        internet_page._ARow                     = -1;
        internet_page._CRow                     = -1;
        internet_page._ERow                     = -1;
        internet_page._beneficiaryAddressSEQID  = "0";
        internet_page._beneficiaryContactSEQID  = "0";
        internet_page._beneficiaryEmailSEQID    = "0";
        internet_page._computation              = "2";
        internet_page._viptype                  = "0";
        internet_page._vippercent               = "0";
        this.getLoginID();
        this.getTodayForex();
        this.getUserLocation();
        this.getBranchCode();
        this.getUserID();
        this.getSearchBy();
        this.getRemitterID(); //Enable to get actual remitter
        this.getLocation();
        this.getLocalTime();
        this.getServerTime();
        this.getInternetLimit();
        this.getTotalRemittance();
        internet_page.edButtons("btnAccept", true);
        //internet_page.showLoading("Retrieving Beneficiaries Information... Please wait", false);
        this.getPurpose();
        this.load_grid_viewer();
        
        shoh("first");
        shoh("second");
        window.setInterval(function(){internet_page.getTodayForex()},100000);
        document.getElementById("lblLoadingMsg").innerHTML = "";
            internet_page.hidePanel("divPopProcess");
    }
    this.showInitializePage_callback = function()
    {
       
            
    }
    
    
    
    
    // **************************************************************************************** //
    // *** Asynchoronous Callback AJAX
    // **************************************************************************************** //
    this.getExpiringIDs = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getExpiringIDs(internet_page._remitterid, this.getExpiringIDs_callback);
    }
    
    this.getExpiringIDs_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._expiringOK = true;
            internet_page.enterValue(response, "spnReminder", true);
        }
        else
        {
            internet_page._expiringOK = false;
        }
    }
    
    this.getSearchBy = function()
    {
        internet_page.ajaxTimeout();
        internet_page.clearDDL("ddlSearchBy");
        return IRSWorld.Internet.T__Remitter.getSearchBy(this.getSearchBy_callback);
    }
    
    this.getSearchBy_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlSearchBy", "FldDescription", "FldValue", "lblCIFClass", "");
        }
    }
    
    this.getAgentRole = function()
    {
        internet_page.ajaxTimeout();
        internet_page.getAgentProcessor();
        internet_page.getAgentApprover();
        internet_page.getAgentApproverProcessor();
        internet_page.getUserRole();
    }
    
    this.getAgentApproverProcessor = function()
    {
        internet_page._agentApproverProcessor = IRSWorld.Internet.T__Remitter.getAgentRoles(6).value;
    }
    
    this.getAgentApprover = function()
    {
        internet_page._agentApprover = IRSWorld.Internet.T__Remitter.getAgentRoles(5).value;
    }
    
    this.getAgentProcessor = function()
    {
        internet_page._agentProcessor = IRSWorld.Internet.T__Remitter.getAgentRoles(4).value;
    }
    
    this.getAgentCurrency = function()
    {
        internet_page.clearDDL("ddlRemittingCurrency");
        return IRSWorld.Internet.T__Remitter.getAgentCurrency(this.getAgentCurrency_callback);
    }
    
    this.getAgentCurrency_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlRemittingCurrency", "FldCode", "FldValue", "lblRemittingCurrency", "");
        }
    }
    
    this.getAgentRemitter = function(beneficiarycifid, remittercifid)
    {
        internet_page.ajaxTimeout();
        internet_page.removeValue("spnHitMessage", true);
        internet_page.removeValue("spnHitMessage2", true);
        switch (internet_page.getValue("lblCIFClass", false))
        {
            case "1": //Remitter
                internet_page.enterValue(remittercifid, "lblRemitterID", false);
                internet_page.hidePanel("divSearchGrid");
                internet_page.clearDDL("ddlBeneficiary");
                internet_page.getBeneficiary(internet_page.getValue("lblRemitterID", false));
                internet_page.acceptDisclaimer(true); 
                internet_page.check_watchlist(remittercifid,false);
                break;
            case "2": //Beneficiary
                return IRSWorld.Internet.T__Remitter.getAgentRemitter(beneficiarycifid, remittercifid, this.getAgentRemitter_callback);                
                break;
        }
    }
    
    this.getAgentRemitter_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.tblBeneficiary = response;
            internet_page._isOSA = false;
            internet_page._isSearch = false;
            internet_page._beneficiary = true;
            //internet_page.enterValue(response.Rows[0]["RemCIFID"], "lblRemitterID", false);
            internet_page.enterValue(internet_page._agentSelectedRemitterID, "lblRemitterID", false);
            internet_page.clearDDL("ddlBeneficiary");
            internet_page.fillDDL(response, "ddlBeneficiary", "ProductCodeDesc|BeneficiaryName|CurrencyDesc|BranchName|AccountNo", "BenCIFID|ProductCode|ImplementationCode|BenAcctSeqID|Currency", "lblBeneficiaryInfo", ", ");
            internet_page.findDDL("ddlBeneficiary", internet_page._agentSelectedBeneficiary, "lblBeneficiaryInfo", true);
            internet_page.getDDL("ddlBeneficiary", "lblBeneficiaryInfo", true);
            internet_page.setBeneficiaryInfo();
            internet_page.hidePanel("divSearchGrid");
            internet_page.acceptDisclaimer(true);            
        }
    }
    
    this.getLookup = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getSubLookup(internet_page._lookupField, this.getLookup_callback);
    }
    
    this.getLookup_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            switch (response.Rows[0]["FldGroup"])
            {
                case "RemitMethod":
                    internet_page.tblRemitMethod = response;
                    break;
                case "Currency":
                    internet_page.tblCurrency = response;
                    break;
                case "ReceiptMode":
               
                    internet_page.tblReceiptMode = response;
                    internet_page._receiptMode = internet_page.findValue(response, "Online (Internet)", "FldDescription", "FldValue");
                    if (internet_page._receiptMode == "" || internet_page._receiptMode == undefined || internet_page._receiptMode == null)
                    {
                        internet_page._receiptMode = "5";
                    }
                    break;
//                case "Computation":
//                    internet_page.tblComputation = response;
//                    internet_page.fillDDL(response, "ddlComputation", "FldDescription", "FldValue", "lblComputation", ", "); 
//                    internet_page._computation = internet_page.findValue(response, "Gross", "FldDescription", "FldValue");
//                    break;
                case "ChargeType":
                    internet_page.tblChargeType = response;
                    internet_page.fillDDL(response, "ddlChargeType", "FldDescription", "FldValue", "lblChargeType", ", ");
            }
            internet_page._lookupField = "";
        }
    }
    
    this.getLookupValue = function(groupname, findcolumn, lookvalue, returncolumn)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getLookupValue(groupname, findcolumn, lookvalue, returncolumn).value;
    }
    
    this.getLocation = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getLocation(this.getLocation_callback);
    }

    this.getLocation_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "lblLocation", false);
            internet_page.getTransactionDate();
            internet_page.getBranchOperations();
            internet_page.getDefaults();
        }
    }
    
    this.getTransactionDate = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getTransactionDate(this.getTransactionDate_callback);
    }
    
    this.getTransactionDate_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "lblTransactionDate", false);
        }
    }
    
    this.getBranchOperations = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getBranchOperations(this.getBranchOperations_callback);
    }
    
    this.getBranchOperations_callback = function(result)
    {
        var response = result.value;
        var address = "";
        var count = 0;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.tblBranchOperations = response;
            internet_page.enterValue("Philippine National Bank " + response.Rows[0]["Branch"], "spnBranch", true);
            internet_page.enterValue(response.Rows[0]["Description"] + "<br><p>", "spnDescription", true);
            internet_page.enterValue(response.Rows[0]["Email"], "spnEmail", true);
            
            for (var i = 0; i <= (response.Rows.length - 1); i++)
            {
                count = count + 1;
                if (count <= 3)
                {
                    address = response.Rows[i]["SubBranch"] + "<br>" + response.Rows[i]["Address"] + "<br>Tel:" + response.Rows[i]["TelNo"];
                    internet_page.enterValue(address, "spnBranch" + count, true);
                }
                else
                {
                    break;
                }
            }
        }
    }
    
    this.getDefaults = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getDefaults(internet_page.getValue("lblLocation", false), this.getDefaults_callback);
    }
    
    this.getDefaults_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.tblDefaults = response;
            internet_page._lookupField = "RemitMethod";
            internet_page.getLookup();
            internet_page._lookupField = "Currency";
            internet_page.getLookup();
            internet_page._lookupField = "ReceiptMode";
            internet_page.getLookup();
            internet_page._lookupField = "Computation";
            internet_page.getLookup();
            internet_page._sourcecurrency = response.Rows[0].RemittingCurrency; 
            internet_page.ajaxTimeout();

            //var fldcode = internet_page.getLookupValue("Currency", "FldValue", internet_page._sourcecurrency.toString(), "FldCode");
        
            //alert(" test : " + fldcode);
           
           // internet_page.enterValue(fldcode, "spnRemittingCurrency", true);
            
            
            //alert(fldcode);
            //alert("test");
            //alert(response.Rows[0]["Currency"]);                       
           //alert(document.getElementById("spnRemittingCurrency").innerHTML);            
            //internet_page.enterValue(internet_page.findValue(internet_page.tblCurrency, response.Rows[0].RemittingCurrency, "FldValue", "FldCode"), "lblSourceCurrency", false);
        }
    }
    
    this.getInternetLimit = function()
    {
        internet_page.ajaxTimeout();
        //_branchCode
        return IRSWorld.Internet.T__Remitter.getBranchSetup(this.getInternetLimit_callback);
    }

    this.getInternetLimit_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._internetLimit = response.Rows[0]["TDD_ClDays_Max"];
            internet_page._maxstraight = response.Rows[0]["TDD_StImp_Max"];
        }
        internet_page.showLoading("", true);
    }
    
    this.getBeneficiary = function(remitterid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getBeneficiary(remitterid, this.getBeneficiary_callback);
    }
    
    this.getBeneficiary_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._isOSA = false;
            internet_page._isSearch = false;
            internet_page._beneficiary = true;
            internet_page.fillDDL(response, "ddlBeneficiary", "ProductCodeDesc|BeneficiaryName|CurrencyDesc|BranchName|AccountNo", "BenCIFID|ProductCode|ImplementationCode|BenAcctSeqID|Currency", "lblBeneficiaryInfo", ", ");
            //internet_page.fillDDL(response, "ddlBeneficiary", "ProductCodeDesc", "BenCIFID|ProductCode|ImplementationCode|BenAcctSeqID|Currency", "lblBeneficiaryInfo", ", ");
            internet_page.tblBeneficiary = response;
            if (internet_page._isAgent == false && internet_page._accept == false)
            {
                internet_page.getRemitterDisclaimer();
            }
            else
            {
                internet_page.findDDL("ddlBeneficiary", internet_page._agentSelectedBeneficiary, "lblBeneficiaryInfo", true);
                internet_page.setBeneficiaryInfo();
                if (internet_page._isAgent == true)
                {
                    internet_page.getFilteredChargeType();
                }
            }
        }
        else
        {
            internet_page._beneficiary = false;
            //internet_page.showAlert("There is no beneficiary maintained for this remitter. Please go to the nearest PNB branch.");
            internet_page.showAlert("You don’t have a beneficiary enrolled. Kindly go to Add Beneficiary module.");
        }
        if (internet_page._isSearch == true)
        {
            var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
            if (beneficiary_value[1] == "8" || beneficiary_value[1] == "9") //Door to Door - Check if OSA
            {
                internet_page.getOSA(beneficiary_value[0]);
            }
        } 
    }
    
    this.getRemitterDisclaimer = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getRemitterDisclaimer(internet_page.getValue("lblRemitterID", false), this.getRemitterDisclaimer_callback);
    }

    this.getRemitterDisclaimer_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response != "1")
            {
                internet_page.getDisclaimer();
            }
            else
            {
                internet_page.acceptDisclaimer(true);
            }
        }
        else
        {
            internet_page.getDisclaimer();
        }
    }
    
    this.getDisclaimer = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getDisclaimer(this.getDisclaimer_callback);
    }

    this.getDisclaimer_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "spnDisclaimer", true);
            internet_page.showControl("divDisclaimer", true);
        }
        else
        {
            internet_page.acceptDisclaimer(true);
        }
    }
    
    
    
    this.getDisclaimer2 = function()
    {
        internet_page.ajaxTimeout();
        var ProductCode = internet_page.tblBeneficiary.Rows[document.getElementById("ddlBeneficiary").selectedIndex - 1].ProductCode.toString(); //internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "ProductCode");
        return IRSWorld.Internet.T__Remitter.getDisclaimer2(ProductCode,this.getDisclaimer2_callback);
    }

    this.getDisclaimer2_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._accept = true;
            internet_page.checkTransaction();
            //internet_page.enterValue(response, "spnDisclaimer2", true);
            //internet_page.showPanel("divDisclaimer2", true);
        }
        else
        {
            internet_page.acceptDisclaimer(true);
        }
    }
    
    
    this.getPurpose = function()
    {
        internet_page.ajaxTimeout();
        internet_page.clearDDL("ddlPurpose");
        return IRSWorld.Internet.T__Remitter.getPurpose(this.getPurpose_callback);
    }
    
    this.getPurpose_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlPurpose", "FldDescription", "FldValue", "lblPurpose", "");
        }
    }
    
    this.getPaymentMethod = function()
    {

        internet_page.ajaxTimeout();
        internet_page.clearDDL("ddlPaymentMethod");
        internet_page.removeValue("spnSourceCurrency", true);
        return IRSWorld.Internet.T__Remitter.getPaymentMethod(this.getPaymentMethod_callback);
    }
    
    this.getPaymentMethod_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlPaymentMethod", "FldDescription", "FldValue", "lblPaymentMethod", "");
            (internet_page._isAgent == true) ? internet_page.setPaymentMethod(true) : internet_page.setPaymentMethod(true);
             
            var fieldtable = IRSWorld.Internet.T__Remitter.getSubLookup("WEB REMITTANCE ACTIVE FIELDS").value;
            if (fieldtable != null && fieldtable.Rows.length > 0)
            {
                for (var i = 0; i <= (fieldtable.Rows.length - 1); i++)
                {
                    internet_page.showControl(fieldtable.Rows[i].FldCode.toString(), true);
                }
            }
            else
            {
                if (internet_page._isAgent != true)
                {
                    document.getElementById("ddlPaymentMethod").selectedIndex = 1;
                    internet_page.getDDLAjax('ddlPaymentMethod', 'lblPaymentMethod', true);
                     
                     var fldcode = internet_page.getLookupValue("Currency", "FldValue", internet_page._sourcecurrency.toString(), "FldCode");
                     internet_page.enterValue(fldcode, "spnRemittingCurrency", true);
                    
                }
                else
                {
                    internet_page.showControl("trPayMode",true);
                }
            }
        }
        
        internet_page.showControl('trOnline',  false);
    }
    
    this.getRemitterID = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getRemitterID(this.getRemitterID_callback);
    }

    this.getRemitterID_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "lblRemitterID", false);
            internet_page._remitterid = response;
            internet_page.getExpiringIDs();
            internet_page.getAgentRole();
        }
        else
        {
            internet_page.showAlert("Session expired. Please close your browser and try to login again.");
        }
    }
    
    this.getUserRole = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getUserRole(this.getUserRole_callback);
    }

    this.getUserRole_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (internet_page._agentProcessor == null || internet_page._agentApprover == null || internet_page._agentApproverProcessor == null)
            {
                internet_page.showAlert("Cannot determine user role. Please close your browser and try to login again.");
            }
            else
            {
                if (response == internet_page._agentProcessor || response == internet_page._agentApprover || response == internet_page._agentApproverProcessor) //Processor - Agent (7) | Approver - Agent (27) | Approver & Processor - Agent (25)
                {
                    internet_page.enterValue(internet_page.getValue("lblRemitterID", false), "lblAgentID", false);
                    internet_page.removeValue("lblRemitterID", false);
                    internet_page._isAgent = true;
                    if (response == internet_page._agentApprover || response == internet_page._agentApproverProcessor)
                    {
                        internet_page._agentIsApprover = true;
                    }
                    else
                    {
                        internet_page._agentIsApprover = false;
                    }
                    document.getElementById("rLogout").style.display = 'none';
                    internet_page._showHistory = false;
                    internet_page.showPanel("divSearch");
                    internet_page.getAgentCurrency();
                    internet_page.showAgentControl(true);
                    internet_page._lookupField = "ChargeType";
                    internet_page.getLookup();
                }
                else
                {
                    document.getElementById("rLogout").style.display = '';
                    internet_page._isAgent = false;
                    internet_page._showHistory = true;
                    internet_page.showAgentControl(false);
                    //internet_page.getPaymentGateway();
                    internet_page.getBenefNames(internet_page.getValue("lblRemitterID", false));
                }
                internet_page.getPaymentMethod();
            }
        }
    }
    
    this.getPaymentGateway = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getPaymentGateway(this.getPaymentGateway_callback);
    }
    
    this.getPaymentGateway_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined && response == true)
        {
            internet_page.getBeneficiary(internet_page.getValue("lblRemitterID", false));
        }
        else
        {
            internet_page.showAlert("There is no payment gateway setup for this region. Please report to the nearest PNB branch.");
        }
    }
    
    
    this.getProducts = function(remitterid,beneid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getProducts(remitterid,beneid, this.getProducts_callback);
    }
    
    this.getProducts_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.clearDDL("ddlBeneficiary");
            internet_page._isOSA = false;
            internet_page._isSearch = false;
            internet_page._beneficiary = true;
            //internet_page.fillDDL(response, "ddlBeneficiary", "ProductCodeDesc|BeneficiaryName|CurrencyDesc|BranchName|AccountNo", "BenCIFID|ProductCode|ImplementationCode|BenAcctSeqID|Currency", "lblBeneficiaryInfo", ", ");
            internet_page.fillDDL(response, "ddlBeneficiary", "ProductCodeDesc|BeneficiaryName|CurrencyDesc|BranchName|AccountNo", "BenCIFID|ProductCode|ImplementationCode|BenAcctSeqID|Currency", "lblBeneficiaryInfo", ", ");
            internet_page.tblBeneficiary = response;
            if (internet_page._isAgent == false && internet_page._accept == false)
            {
                internet_page.getRemitterDisclaimer();
            }
            else
            {

                internet_page.findDDL("ddlBeneficiary", internet_page._agentSelectedBeneficiary, "lblBeneficiaryInfo", true);
                internet_page.setBeneficiaryInfo();
                if (internet_page._isAgent == true)
                {
                    internet_page.getFilteredChargeType();
                }
            }
        }
      
        if (internet_page._isSearch == true)
        {
            var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
            if (beneficiary_value[1] == "8" || beneficiary_value[1] == "9") //Door to Door - Check if OSA
            {
                internet_page.getOSA(beneficiary_value[0]);
            }
        } 
    }
    
    
    
    this.getBenefNames = function(remid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getBeneficiaryInfo(remid,this.getBenefNames_callback);
    }
    
    this.getBenefNames_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._isOSA = false;
            internet_page._isSearch = false;
            internet_page._beneficiary = true;
            internet_page.clearDDL("ddlBenefName");
            internet_page.fillDDL(response, "ddlBenefName", "BeneficiaryName", "BenCIFID", "lblBeneficiaryCIF", ", ");
            internet_page.tblBeneficiary = response;
            if (internet_page._isAgent == false && internet_page._accept == false)
            {
                internet_page.getRemitterDisclaimer();
            }
            else
            {
                internet_page.findDDL("ddlBenefName", internet_page._agentSelectedBeneficiary, "lblBeneficiaryInfo", true);

            }
        }
        else
        {
            internet_page._beneficiary = false;
            //internet_page.showAlert("There is no beneficiary maintained for this remitter. Please go to the nearest PNB branch.");
            internet_page.showAlert("You don’t have a beneficiary enrolled. Kindly go to Add Beneficiary module.");
        }
        if (internet_page._isSearch == true)
        {
            var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
            if (beneficiary_value[1] == "8" || beneficiary_value[1] == "9") //Door to Door - Check if OSA
            {
                internet_page.getOSA(beneficiary_value[0]);
            }
        } 
        
    }
    
    
    
    this.getUserLocation = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getUserLocation(this.getUserLocation_callback);
    }
    
    this.getUserLocation_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._userLocation = response;
        }
    }
    
    this.getBranchCode = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getLocation(this.getBranchCode_callback);
    }
    
    this.getBranchCode_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._branchCode = response;
        }
    }
    
    this.getLoginID = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getLoginID(this.getLoginID_callback);
    }

    this.getLoginID_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._loginID = response;
        }
    }
    
    this.getUserID = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getUserID(this.getUserID_callback);
    }

    this.getUserID_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "lblUserID", false);
        }
    }
    
    this.getCharges = function(isoverride) 
    {
        internet_page.ajaxTimeout();
        if (document.getElementById("ddlBeneficiary").selectedIndex > 0)
        {
            var beneficiaryID = internet_page.getValue("lblBeneficiaryInfo", false).split("|"); //CIFID|ProductCode|ImplementationCode
            var amount = internet_page.getValue("txtAmount", false);
            amount = (amount == "" || amount == null || amount == undefined) ? "0.00" : amount;
            
            internet_page.enterValue(beneficiaryID[0], "lblBeneficiaryID", false);
            internet_page._productcode = beneficiaryID[1].toString();
            internet_page._implementationtype = beneficiaryID[2].toString();
            internet_page._targetcurrency = beneficiaryID[4].toString();
            
            internet_page._chargetype = "";
            if(internet_page._targetcurrency  == 1)
            {
                internet_page._chargetype = IRSWorld.Internet.T__Remitter.getChargeTypePerTargetCurrency("ChargeTypePHP",internet_page._branchCode).value;
            }
            else if(internet_page._targetcurrency  == 2)
            {
                internet_page._chargetype = IRSWorld.Internet.T__Remitter.getChargeTypePerTargetCurrency("ChargeTypeUSD",internet_page._branchCode).value;
            }
            else if(internet_page._targetcurrency  == 5)
            {
                internet_page._chargetype = IRSWorld.Internet.T__Remitter.getChargeTypePerTargetCurrency("ChargeTypeEUR",internet_page._branchCode).value;
            }
            
            
            internet_page._pay_mode = document.getElementById('ddlPaymentMethod').options[document.getElementById('ddlPaymentMethod').selectedIndex].value;
            
            if (internet_page._isAgent != true)
            {
                internet_page._chargetype = IRSWorld.Internet.T__Remitter.getWebRemittanceChargeType().value;
                //internet_page._computation = "2";
                //document.getElementById("ddlComputation").selectedIndex = 2;
                //internet_page.enterValue(internet_page._remittingCurrency, "spnRemittingCurrency", true);
                internet_page._chargetype = (internet_page._chargetype == "" || internet_page._chargetype == null || internet_page._chargetype == undefined) ? internet_page.getTableValue(internet_page.tblDefaults, "ChargeType") : internet_page._chargetype;
            }
            //IRSWorld.Internet.T__Remitter.getCharges(amount, internet_page.getValue("lblLocation", false), internet_page._productcode, internet_page._implementationtype, internet_page.getValue("lblSourceCurrency", false), internet_page._targetcurrency, String(internet_page._chargetype), internet_page.getValue("lblPaymentMethod", false), internet_page._computation, true, ((isoverride == true) ? true : false), String(internet_page._vippercent), internet_page.getCharges_callback);
            IRSWorld.Internet.T__Remitter.getCharges(amount, internet_page.getValue("lblLocation", false), String(internet_page._productcode), String(internet_page._implementationtype), String(internet_page._sourcecurrency), String(internet_page._targetcurrency), String(internet_page._chargetype), internet_page._pay_mode, internet_page._computation, true, ((isoverride == true) ? true : false), String(internet_page._vippercent), internet_page.getCharges_callback);
            internet_page.hidePanel("divACHWorks");
            internet_page.showLoading("Please wait....", false);
        }
    }
    
    this.getCharges_callback = function(param)
    {
        internet_page.clearCompute();        
        var response = param.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._chargeResponse = response;
            if (response == "NOCHARGE")
            {
                internet_page.enterValue(internet_page._netToBenefCurrency, "spnNetToBeneAmount", true);
                internet_page.edButtons("btnSubmit", true);
                internet_page.edButtons("btnConfirmRem", false);
                (internet_page._isAgent == true) ? internet_page.showAlert("There is no applicable charge for " + internet_page.getDDLValue("ddlRemittingCurrency", false) + " currency. Please select another currency.") : internet_page.showAlert("There is no applicable charge for " + internet_page.getValue("spnRemittingCurrency", true) + " currency. Please select another currency.");
                internet_page._chargeIsValid = false;
            }
            else
            {
                if (response == "NOTALLOWED")
                {
                    internet_page.enterValue(internet_page._netToBenefCurrency, "spnNetToBeneAmount", true);
                    internet_page.edButtons("btnSubmit", true);
                    internet_page.edButtons("btnConfirmRem", false);
                    internet_page.showAlert("Role is not yet allowed to use this charge combination. Please report to the nearest PNB branch and/or your System Administrator.");
                    internet_page._chargeIsValid = false;
                }
                else
                {
                    internet_page.edButtons("btnConfirmRem", false);
                    internet_page._chargeIsValid = true;
                    (internet_page._isSubmit = false) ? internet_page.edButtons("btnSubmit", false) : internet_page.edButtons("btnSubmit", true);
                    internet_page._calc = response;
                    internet_page._arrCalc = response.split("|");
                    internet_page.enterValue(internet_page._arrCalc[1], "txtCExchangeRate", false);
                    if (internet_page._isAgent == true)
                    {
                        internet_page.enterValue(internet_page.getDDLValue("ddlRemittingCurrency", false) + " to " + internet_page._arrCalc[3], "spnCExchangeRateCurrency", true);
                    }
                    else
                    {
                        internet_page.enterValue(internet_page.getValue("spnSourceCurrency", true) + " to " + internet_page._arrCalc[3], "spnCExchangeRateCurrency", true);
                    }
                    internet_page.enterValue(internet_page._arrCalc[36], "spnCNetAmountCurrency", true);
                    internet_page.enterValue(internet_page._arrCalc[27], "txtCNetAmount", false);
                    internet_page.enterValue(internet_page._arrCalc[36], "spnCBankChargeAmountCurrency", true);
                    internet_page.enterValue(internet_page._arrCalc[31], "txtCBankChargeAmount", false);
                    internet_page.enterValue(internet_page._arrCalc[36], "spnCGrossAmountCurrency", true);
                    internet_page.enterValue(internet_page._arrCalc[24], "txtCGrossAmount", false);
                    internet_page.enterValue(internet_page._arrCalc[3], "spnCNetToBenefAmountCurrency", true);
                    internet_page.enterValue(internet_page._arrCalc[29], "txtCNetToBenefAmount", false);
                    //internet_page.enterValue("PHP", "spnCPHPConvertedAmountCurrency", true);
                    //internet_page.enterValue(internet_page._arrCalc[28], "txtCPHPConvertedAmount", false);
                    internet_page._amountPG = internet_page._arrCalc[24];
                    if (internet_page._isAgent == true)
                    {
                        if (internet_page._exchangeRateValid == true)
                        {
                            internet_page.enterValue(internet_page._netToBenefCurrency + " " + internet_page._arrCalc[29], "spnNetToBeneAmount", true);
                        }
                    }
                    else
                    {
                        internet_page.enterValue(internet_page._netToBenefCurrency + " " + internet_page._arrCalc[29], "spnNetToBeneAmount", true);
                    }
                    internet_page.getChargesDefaults(internet_page._arrCalc[39]);
                }
            } 
            
        }
        internet_page.showLoading("", true);
        if (internet_page._checktransaction == 1)
        {
            internet_page._checktransaction = 0;
            //internet_page.checkTransaction_final();
            internet_page.getAccountInquiry();
        }
        if (internet_page._submittransaction == 1)
        {
             internet_page._submittransaction = 0;
            internet_page.submitTransaction_final();
        }
        if (internet_page._transmitremittance == 1)
        {
            internet_page._transmitremittance = 0;
            internet_page.transmitRemittance_final();
        }
    }
    
    this.getChargesDefaults = function(chargeid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getChargesDefaults(chargeid, this.getChargesDefaults_callback);
    }
    
    this.getChargesDefaults_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.tblChargesDefault = response;
        }
    }
    
    this.getExchangeRate = function(sourcecurrency, targetcurrency)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getExchangeRate(sourcecurrency, targetcurrency, internet_page._isAgent, this.getExchangeRate_callback);
    }
    
    this.getExchangeRate_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._exchangeRateValid = true;
            internet_page.enterValue(response, "txtExchangeRate", false);
        }
        else
        {
            internet_page._exchangeRateValid = false;
            internet_page.enterValue("No exchanged rate defined.", "txtExchangeRate", false);
        }
        internet_page.showLoading("", true);
    }
    
    this.getRemitterName = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getRemitterName(internet_page.getValue("lblRemitterID", false), this.getRemitterName_callback);
    }
    
    this.getRemitterName_callback = function(result)
    {
        var response = result.value;
        var arrResponse = "";
        if (response != null && response != "" && response != undefined)
        {
            arrResponse = response.split("|");
            internet_page.enterValue("WELCOME " + arrResponse[0], "spnRemitterName", true);
            internet_page._viptype = arrResponse[2];
            internet_page._chargetype = arrResponse[3];
            if (internet_page._viptype != null && internet_page._viptype != "" && internet_page._viptype != undefined)
            {
                if (parseInt(internet_page._viptype))
                {
                    if (Number(internet_page._viptype) > 0)
                    {
                        internet_page.enterValue("VIP ", "spnVIP", true);
                    }
                }
            }
            internet_page.getVIPPercent();
            if (internet_page._isAgent != true)
            {
                internet_page.showControl("tblRemitterName", true);
            }
        }
    }
    
    this.getVIPPercent = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getVIPPercent(internet_page._viptype, this.getVIPPercent_callback);
    }
    
    this.getVIPPercent_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page._vippercent = response;
        }
        else
        {
            internet_page._vippercent = 0;
        }
    }
    
    this.getDCCard = function()
    {
        internet_page.ajaxTimeout();
        internet_page.clearDDL("ddlDCCard");
        return IRSWorld.Internet.T__Remitter.getDCCard(internet_page.getValue("lblRemitterID", false), this.getDCCard_callback);
    }
    
    this.getDCCard_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlDCCard", "CardNo", "SEQ_ID", "lblRemAcctSeqID", "");
            internet_page.tblCards = response;
            internet_page.maskString("ddlDCCard", 4);
        }
        else
        {
            internet_page.showLoading("", true);
            internet_page.showAlert("There is no cards maintained for this remitter. Please go to the nearest PNB branch.");
        }
        internet_page.showLoading("", true);
    }
    
    this.getPNBAccounts = function()
    {
        internet_page.ajaxTimeout();
        internet_page.clearDDL("ddlDCCard");
        return IRSWorld.Internet.T__Remitter.getPNBAccounts(internet_page.getValue("lblRemitterID", false), internet_page.getValue("lblLocation", false), this.getPNBAccounts_callback);
    }
    
    this.getPNBAccounts_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlDCCard", "AccountNo", "SEQ_ID", "lblRemAcctSeqID", "");
            internet_page.tblPNBAccounts = response;
            internet_page.maskString("ddlDCCard", 4);
        }
        else
        {
            internet_page.showLoading("", true);
            internet_page.showAlert("There is no accounts maintained for this remitter. Please go to the nearest PNB branch.");
        }
        internet_page.showLoading("", true);
    }
    
    this.getOtherAccounts = function()
    {
        internet_page.ajaxTimeout();
        internet_page.clearDDL("ddlDCCard");
        return IRSWorld.Internet.T__Remitter.getOtherAccounts(internet_page.getValue("lblRemitterID", false), this.getOtherAccounts_callback);
    }
    
    this.getOtherAccounts_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.fillDDL(response, "ddlDCCard", "AccountNo", "SEQ_ID", "lblRemAcctSeqID", "");
            internet_page.tblPNBAccounts = response;
            internet_page.maskString("ddlDCCard", 4);
            document.getElementById("ddlDCCard").selectedIndex = 1;
            internet_page.getDDLAjax('ddlDCCard', 'lblDCCard', true);
        }
        else
        {
            internet_page.showLoading("", true);
            internet_page.showAlert("There is no account maintained for this remitter. Please go to My Accounts to add account. ");
        }
        internet_page.showLoading("", true);
    }
    
    this.getRemitterCIFInformation = function(cifid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getCIFInformation(cifid, this.getRemitterCIFInformation_callback);
    }
    
    this.getRemitterCIFInformation_callback = function(result)
    {
        var response = result.value;
        internet_page.removeValue("spnTRemitterName", true);
        internet_page.removeValue("spnTRemitterAddress", true);
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response.Rows[0]["FullName"], "spnTRemitterName", true);
            internet_page.enterValue(response.Rows[0]["Address"], "spnTRemitterAddress", true);
            internet_page._chargetype = response.Rows[0]["ChargeType"];
        }
    }
    
    this.getBeneficiaryCIFInformation = function(cifid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getCIFInformation(cifid, this.getBeneficiaryCIFInformation_callback);
    }
    
    this.getBeneficiaryCIFInformation_callback = function(result)
    {
        var response = result.value;
        var beneficiaryfound = false;
        
        internet_page.removeValue("spnTBeneficiaryName", true);
        internet_page.removeValue("spnTBeneficiaryAddress", true);
        internet_page.removeValue("spnTBeneficiaryAccount", true);
        internet_page.removeValue("spnTBeneficiaryBranch", true);
                
        if (response != null && response != "" && response != undefined)
        {
            for (var i = 0; i <= (response.Rows.length - 1); i++)
            {
                if (internet_page.getTableValue(internet_page.tblRemittanceHistory, "ProductCode") == response.Rows[i]["ProductCode"] && internet_page.getTableValue(internet_page.tblRemittanceHistory, "ImplementationType") == response.Rows[i]["ImplementationCode"])
                {
                    beneficiaryfound = true;
                    internet_page.enterValue(response.Rows[i]["FullName"], "spnTBeneficiaryName", true);
                    internet_page.enterValue(response.Rows[i]["Address"], "spnTBeneficiaryAddress", true);
                    internet_page.enterValue(response.Rows[i]["AccountNo"], "spnTBeneficiaryAccount", true);
                    internet_page.enterValue(response.Rows[i]["PNBBranchName"], "spnTBeneficiaryBranch", true);
                    break;
                }
            }
        }
    }
    
    this.getBeneficiaryAddress = function(cifid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getBeneficiaryAddress(cifid, this.getBeneficiaryAddress_callback);
    }
    
    this.getBeneficiaryAddress_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (internet_page._ARow == -1)
            {
                internet_page.tblBeneficiaryAddress = response;
                internet_page._ARow = 0;
            }
            internet_page.enterValue(internet_page.getTableValueRow(response, "Address", internet_page._ARow), "spnAddress", false);
            internet_page._beneficiaryAddressSEQID = response.Rows[internet_page._ARow]["SEQ_ID"];
        }
    }
    
    this.getBeneficiaryContacts = function(cifid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getBeneficiaryContacts(cifid, this.getBeneficiaryContacts_callback);
    }
    
    this.getBeneficiaryContacts_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (internet_page._CRow == -1)
            {
                internet_page.tblBeneficiaryContacts = response;
                internet_page._CRow = 0;
            }
            if (internet_page._ERow == -1)
            {
                internet_page.tblBeneficiaryEmail = response;
                internet_page._ERow = 0;
            }
            internet_page.enterValue(internet_page.getTableValueRow(response, "ContactNo", internet_page._CRow), "spnContacts", false);
            internet_page.enterValue(internet_page.getTableValueRow(response, "EmailAddress", internet_page._ERow), "spnEmailAddress", false);
            internet_page._beneficiaryContactSEQID = response.Rows[internet_page._CRow]["SEQ_ID"];
            internet_page._beneficiaryEmailSEQID = response.Rows[internet_page._ERow]["SEQ_ID"];
        }
    }
    
    this.getRemittance = function(refno)
    {
        internet_page.ajaxTimeout();
        internet_page.removeValue("spnError", true);
        return IRSWorld.Internet.T__Remitter.getRemittance(refno, this.getRemittance_callback);
    }
    
    this.getRemittance_callback = function(result)
    {
        var response = result.value;
        var remittingCurrency = "";
        var netToBenefCurrency = "";
        var message = "";
        var authcode = "";
        
        if (response != null && response != "" && response != undefined)
        {
            
            internet_page.tblRemittanceHistory = response;
            remittingCurrency = internet_page.getTableValue(internet_page.tblRemittanceHistory, "RemittingCurrencyCode");
            netToBenefCurrency = internet_page.getTableValue(internet_page.tblRemittanceHistory, "NetToBenefCurrencyCode");
            internet_page._txnseqid = internet_page.getTableValue(internet_page.tblRemittanceHistory, "Trxn_ID");
            internet_page._refno = internet_page.getTableValue(internet_page.tblRemittanceHistory, "ReferenceNo");
            
            internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "ReferenceNo"), "lblReferenceNo", false);
            internet_page.enterValue(internet_page.getValue("lblReferenceNo", false), "spnTReferenceNo", true);
            internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "TransDate"), "spnTTransactionDate", true);
            internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "TranxStatus"), "spnTStatus", true);
            internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "UserID"), "spnTEncodedBy", true);
            //internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "ApprovingOfficer"), "spnTValidatedBy", true);
            //internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "TransactionSourceDesc"), "spnTType", true);
            internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "ReceiptModeDesc"), "spnTSubType", true);
            internet_page.enterValue(internet_page.getTableValue(internet_page.tblRemittanceHistory, "ProductDescription"), "spnTProductDescription", true);
            message = (internet_page.getTableValue(internet_page.tblRemittanceHistory, "Message") == null) ? "" : internet_page.getTableValue(internet_page.tblRemittanceHistory, "Message");
            authcode = (internet_page.getTableValue(internet_page.tblRemittanceHistory, "AuthCode") == null) ? "" : internet_page.getTableValue(internet_page.tblRemittanceHistory, "AuthCode");
            internet_page.enterValue("Auth Code: " + authcode + " / Response: " + message, "spnPGResponse", true);
            
            internet_page.enterValue(remittingCurrency + " to " + netToBenefCurrency, "spnTExchangeRateCurrency", true);
            internet_page.enterValue(remittingCurrency, "spnTBankChargeCurrency", true);
            internet_page.enterValue(remittingCurrency, "spnNetAmountCurrency", true);
            internet_page.enterValue(remittingCurrency, "spnTGrossAmountCurrency", true);
            internet_page.enterValue(netToBenefCurrency, "spnTNetToBenefAmountCurrency", true);
            
            internet_page.enterValue(internet_page.formatCurrency(Number(internet_page.getTableValue(internet_page.tblRemittanceHistory, "ForexRemittingToAccount")), 2), "spnTExchangeRate", true);
            internet_page.enterValue(internet_page.formatCurrency(Number(internet_page.getTableValue(internet_page.tblRemittanceHistory, "TotalLocalCharge")), 2), "spnTBankCharge", true);
            internet_page.enterValue(internet_page.formatCurrency(Number(internet_page.getTableValue(internet_page.tblRemittanceHistory, "NetRemAmount")), 2), "spnNetAmount", true);
            internet_page.enterValue(internet_page.formatCurrency(Number(internet_page.getTableValue(internet_page.tblRemittanceHistory, "GrossRemAmount")), 2), "spnTGrossAmount", true);
            internet_page.enterValue(internet_page.formatCurrency(Number(internet_page.getTableValue(internet_page.tblRemittanceHistory, "NettoBeneAmount")), 2), "spnTNetToBenefAmount", true);
            
            internet_page.getRemitterCIFInformation(internet_page.getTableValue(internet_page.tblRemittanceHistory, "RemitterCIFID"));
            internet_page.getBeneficiaryCIFInformation(internet_page.getTableValue(internet_page.tblRemittanceHistory, "BeneficiaryCIFID"));
        }
        else
        {
            internet_page.enterValue("There was an error while retrieving your record. Please try again later.", "spnError", true);
        }
        internet_page.showPanel("divTransactionHistoryModal");
    }
    
    this.submitPaymentGateway = function()
    {
        internet_page.ajaxTimeout();
        if (internet_page._isDC == "1")
        {
            IRSWorld.Internet.T__Remitter.submitPaymentGateway(internet_page.getValue("lblLocation", false), internet_page.getValue("lblRemitterID", false), internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CardNo"), internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CardName"), internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "ExpiryDate"), internet_page._securitycode, internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CardTypeCode"), internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode"), internet_page._amountPG, "Internet Remittance Transaction [IRT]", this.submitPaymentGateway_callback);
        }
        else if (internet_page._isDC == "3")
        {
            IRSWorld.Internet.T__Remitter.submitPaymentGateway(internet_page.getValue("lblLocation", false).toString(), internet_page.getValue("lblRemitterID", false), internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountNo"), internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "BranchName"), "", internet_page._securitycode, internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountType").toString(), internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode"), internet_page._amountPG, "Internet Remittance Transaction [IRT]", this.submitPaymentGateway_callback);
        }
        else if (internet_page._isDC == "4")
        {
            IRSWorld.Internet.T__Remitter.submiteXactPaymentGateway(internet_page.getValue("lblLocation", false).toString(), internet_page.getValue("lblRemitterID", false), internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountNo"), internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "BranchName"), "", internet_page._securitycode, internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountType").toString(), internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CurrencyCode"), internet_page._amountPG, "Internet Remittance Transaction [IRT]", this.submitPaymentGateway_callback);
        }
    }
    
    this.submitPaymentGateway_callback = function(result)
    {
        var arrResponse = "";
        var response = "";
        var boolOK = false;
        
        response = result.value;
        if (response != null && response != "" && response != undefined && response != "|||" && response != "||||")
        {
            arrResponse = response.split("|");
            // *****************************************
            // *** FOR TESTING PURPOSES [REMOVE] - START
            // *****************************************
            /*
            if (confirm("(FOR UAT) Do you want to get forced OK result?"))
            {
                if (arrResponse[0] != "OK")
                {
                    arrResponse[0] = "OK";
                    arrResponse[1] = "1234567";
                    arrResponse[2] = "(FORCED OK) The Authorisation was successful!";
                    arrResponse[3] = "OK|1234567|(FORCED OK) The Authorisation was successful!";
                }
            }
            if (arrResponse[0] != "OK")
            {
                arrResponse[0] = "OK";
                arrResponse[1] = "1234567";
                arrResponse[2] = "(FORCED OK) The Authorisation was successful!";
                arrResponse[3] = "OK|1234567|(FORCED OK) The Authorisation was successful!";
            }
            */
            // *****************************************
            // *** FOR TESTING PURPOSES [REMOVE] - END
            // *****************************************
            if (arrResponse[0] == "OK")
            {
                internet_page.getORNo();
                internet_page.enterValue(arrResponse[1], "lblAuthCode", false);
                internet_page.enterValue(arrResponse[2], "lblStatusDetail", false);
                internet_page.enterValue(arrResponse[3], "lblPGMessage", false);
                internet_page.edButtons("btnSubmitTransaction", false);
                internet_page.edButtons("btnCancelTransaction", false);
                internet_page.hidePanel("divSecurity");
                internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                internet_page.showPGLoading("", true);
            }
            else
            {
                if (arrResponse[0] == "3DSECURE")
                {
                    internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                    internet_page.showLoading("", true);
                    internet_page.showPGLoading("", true);
                    internet_page.hidePanel("divSecurity");
                    internet_page.enterValue(arrResponse[1], "lbl3dSecure", false);
                    internet_page.enterValue(arrResponse[2], "lbl3dSecureURL", false);
                    internet_page.enterValue("Please wait...", "div3DSecure", true);
                    internet_page.showPanel("div3DSecure");
                    show_frame_dialog("../Internet/Internet3DSecure.aspx", "internetClose(null)", "100%", "100%", "0");
                    internet_page.hidePanel("div3DSecure");
                }
                else if (arrResponse[0] == "ENETS")
                {
                    internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                    internet_page.showLoading("", true);
                    internet_page.showPGLoading("", true);
                    internet_page.hidePanel("divSecurity");
                    internet_page.enterValue("Please wait...", "div3DSecure", true);
                    internet_page.showPanel("div3DSecure");
                    internet_page._timeout = window.setInterval(function(){IRSWorld.Internet.T__Remitter.pingSession(internet_page.pingSession_callback)},100000);
                    var winenets = window.open("../Internet/Internet3DSecure.aspx?enets=1","ENETS","status=1,scrollbars=yes,resizable=yes,fullscreen=yes,location=no,controlbox=no,width=" + screen.width + ",height=" + screen.height + "");
                    internet_page.hidePanel("div3DSecure");
                    internet_page.showTransactionLoading("", true);
                    internet_page.enterValue("<center><b>Please close after ENETS transaction is done.</b></center>", "spnDisclaimerAlert", true);
                    internet_page.showPanel("divDisclaimerAlert");
                }
                else if (arrResponse[0] == "EXACT")
                {
                    internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                    internet_page.showLoading("", true);
                    internet_page.showPGLoading("", true);
                    internet_page.hidePanel("divSecurity");
                    internet_page.enterValue("Please wait...", "div3DSecure", true);
                    internet_page.showPanel("div3DSecure");
                    internet_page._timeout = window.setInterval(function(){IRSWorld.Internet.T__Remitter.pingSession(internet_page.pingSession_callback)},100000);
                    //var winenets = window.open("../Remittance/eXactRequest.aspx","EXACT","status=1,scrollbars=yes,resizable=yes,fullscreen=yes,location=no,controlbox=no,width=" + screen.width + ",height=" + screen.height + "");
                    show_frame_dialog('../Remittance/eXactRequest.aspx', 'internetRemitterClose', '100%', '100%', '0','0','250',true);
                    internet_page.hidePanel("div3DSecure");
                    internet_page.showTransactionLoading("", true);
                    //internet_page.enterValue("<center><b>Transaction Successful, Please click ok to proceed.</b></center>", "spnDisclaimerAlert", true);
                    //internet_page.showPanel("divDisclaimerAlert");
                   
                }
                else
                {
                    internet_page.showLoading("", true);
                    internet_page.showPGLoading("", true);
                    internet_page.removeValue("txtSecurityCode", false);
                    internet_page.edButtons("btnSubmitTransaction", false);
                    internet_page.edButtons("btnCancelTransaction", false);
                    if (arrResponse.length > 1)
                    {
                        internet_page.showAlert("The transaction received an error. \r\n\r\n" + arrResponse[2] + "\r\n\r\n Please report to the nearest PNB branch.");
                    }
                    else
                    {
                        internet_page.showAlert("A problem occured while connecting/receiving information from the payment gateway. Please try again.")
                    }
                    internet_page.hidePanel("divSecurity");
                    internet_page.showTransactionLoading("", true);
                }
            }
        }
        else
        {
            internet_page.showLoading("", true);
            internet_page.showPGLoading("", true);
            internet_page.removeValue("txtSecurityCode", false);
            internet_page.edButtons("btnSubmitTransaction", false);
            internet_page.edButtons("btnCancelTransaction", false);
            internet_page.showAlert("A problem occured while connecting/receiving information from the payment gateway. Please try again.")
            internet_page.hidePanel("divSecurity");
        }
    }
    
    this.close_dialog_box = function()
    {
        close_frame_dialog('');
        internet_page.hidePanel("divDisclaimerAlert");
        
    }
    
    this.getORNo = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getORNo(this.getORNo_callback);
    }
    
    this.getORNo_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "lblORNo", false);
            internet_page.verifyOAAMTransaction();
        }
        else
        {
            internet_page.showAlert("Problem retrieving information from the server. Please try again.");
        }
    }
    
    this.verifyOAAMTransaction = function()
    {
        var response = "";
        response = IRSWorld.Internet.T__Remitter.verifyOAAMTransaction(internet_page.getValue("lblBenAcctSeqID", false), internet_page._arrCalc[14], internet_page._arrCalc[29]).value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == "OK")
            {
                internet_page.submitRemittance();
            }
            else
            {
                if (response == "Transaction Blocked.")
                {
                    spawn_errpage("Transaction blocked! Please contact your system administrator and/or PNB for assistance.");
                }
                else
                {
                    internet_page.submitRemittance();
                }
            }
        }
        else
        {
            internet_page.showAlert("No response from OAAM. Please try again.");
        }
    }

    this.submitRemittance = function()
    {
        internet_page.ajaxTimeout();
        IRSWorld.Internet.T__Remitter.submitRemittance(internet_page.createRemittance(), this.submitRemittance_callback);
    }

    this.submitRemittance_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (Number(response) >= 0)
            {
                internet_page.getReferenceNo();
            }
            else
            {
                internet_page.showTransactionLoading("", true);
                internet_page.showAlert("Transaction failed. Please report to the nearest PNB branch.");
            }
        }
        else
        {
            internet_page.showTransactionLoading("", true);
            internet_page.showAlert("Transaction failed. Please report to the nearest PNB branch.");
        }
        internet_page.showLoading("", true);
    }
    
    this.getReferenceNo = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getReferenceNo(internet_page.getValue("lblORNo", false), internet_page.getValue("lblTransactionDate", false), this.getReferenceNo_callback);
    }

    this.getReferenceNo_callback = function(result)
    {
        var response = result.value;
        var refno = "";
        var erefno = "";
        var txnseqid = "";
        
        internet_page._fcSuccess = false;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.tblRemittance = response;
            refno = internet_page.getTableValue(internet_page.tblRemittance, "ReferenceNo");
            erefno = internet_page.getTableValue(internet_page.tblRemittance, "ExtlReferenceNo");
            txnseqid = internet_page.getTableValue(internet_page.tblRemittance, "SEQ_ID");
            internet_page.enterValue(refno, "spnReferenceNo", false);
            internet_page._refno = refno;
            internet_page._txnseqid = txnseqid;
            internet_page._erefno = erefno;
            internet_page.enterValue(txnseqid, "lblTransactionSEQID", false);
            internet_page.writeLogger(internet_page._auditRemittance, "TBL_REMITTANCE", txnseqid);
            internet_page.submitRemittanceCharges(txnseqid);
            internet_page.submitORDCharges(txnseqid, internet_page._arrCalc[29], internet_page._productcode, internet_page._implementationtype, internet_page._targetcurrency, internet_page._computation);
            internet_page.submitGFC(refno);
            var beneficiaryID = internet_page.getValue("lblBeneficiaryInfo", false).split("|");
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, beneficiaryID[0] + "|" + beneficiaryID[1], "BenCIFID|ProductCode", "CurrencyDesc") + " " + internet_page.findValue(internet_page.tblBeneficiary, beneficiaryID[0] + "|" + beneficiaryID[1], "BenCIFID|ProductCode", "AccountNo"), "spnTIBeneficiaryAccount", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, beneficiaryID[0] + "|" + beneficiaryID[1], "BenCIFID|ProductCode", "BeneficiaryName"), "spnTIBeneficiaryName", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, beneficiaryID[0] + "|" + beneficiaryID[1], "BenCIFID|ProductCode", "ProductCodeDesc"), "spnTIProductCode", false);
            internet_page.enterValue(internet_page.getValue("spnAddress", false), "spnTIBeneficiaryAddress", false);
            internet_page.enterValue(internet_page._arrCalc[36] + " " + internet_page._arrCalc[31], "spnTICharges", false);
            internet_page.enterValue(internet_page._arrCalc[36] + " " + internet_page._arrCalc[24], "spnTIGrossAmount", false);
            if (internet_page._tranStatus == "1") //New Transaction
            {
                internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                internet_page.processTransaction(txnseqid);
            }
            else
            {
                internet_page.transactionSuccess("");
            }
            internet_page._tranStatus = "";
            var paymentMethod = internet_page.getDDLValue("ddlPaymentMethod", false);
            if (internet_page._isDC == "1")
            {
                internet_page.submitRemittanceCards(txnseqid, internet_page.getValue("lblRemAcctSeqID", false), internet_page.getValue("lblStatusDetail", false), internet_page.getValue("lblPGMessage", false), internet_page.getValue("lblAuthCode", false), true);
                internet_page.enterValue("Debit/Credit Card", "spnPM1", true);
                internet_page.enterValue("Expiry Date", "spnPM2", true);
                internet_page.enterValue("Card Name", "spnPM3", true);
                internet_page.enterValue(internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CardNo"), "spnTIDCCard", false);
                internet_page.enterValue(internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "ExpiryDate"), "spnTIExpiryDate", false);
                internet_page.enterValue(internet_page.findValue(internet_page.tblCards, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "CardName"), "spnTICardName", false);
                internet_page.enterValue(internet_page.getValue("lblAuthCode", false), "spnTIAuthCode", false);
                internet_page.enterValue(internet_page.getValue("lblStatusDetail", false), "spnTIResponse", false);
            }
            else if (internet_page._isDC == "3")
            {
                internet_page.submitRemittanceCards(txnseqid, internet_page.getValue("lblRemAcctSeqID", false), internet_page.getValue("lblStatusDetail", false), internet_page.getValue("lblPGMessage", false), internet_page.getValue("lblAuthCode", false), false);
                internet_page.enterValue("Account Number", "spnPM1", true);
                internet_page.enterValue("Account Type", "spnPM2", true);
                internet_page.enterValue("Account Branch", "spnPM3", true);
                internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountNo"), "spnTIDCCard", false);
                internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountTypeDesc"), "spnTIExpiryDate", false);
                internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "BranchName"), "spnTICardName", false);
                internet_page.enterValue(internet_page.getValue("lblAuthCode", false), "spnTIAuthCode", false);
                internet_page.enterValue(internet_page.getValue("lblStatusDetail", false), "spnTIResponse", false);
            }
            else
            {
                if (internet_page._isAgent == true)
                {
                    internet_page.enterValue("n/a", "spnTIDCCard", false);
                    internet_page.enterValue("n/a", "spnTIExpiryDate", false);
                    internet_page.enterValue("n/a", "spnTICardName", false);
                }
                else
                {
                    if(paymentMethod != "ACH")
                    {
                        if(internet_page.tblPNBAccounts != null)
                        {
                            internet_page.submitRemittanceCards(txnseqid, internet_page.getValue("lblRemAcctSeqID", false), "", "", "", false);
                            internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountNo"), "spnTIDCCard", false);
                            internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "AccountTypeDesc"), "spnTIExpiryDate", false);
                            internet_page.enterValue(internet_page.findValue(internet_page.tblPNBAccounts, internet_page.getValue("lblRemAcctSeqID", false), "SEQ_ID", "BranchName"), "spnTICardName", false);
                        }
                    }
                }
                internet_page.enterValue("Account Number", "spnPM1", (paymentMethod != "ACH") ? true : false);
                internet_page.enterValue("Account Type", "spnPM2", (paymentMethod != "ACH") ? true : false);
                internet_page.enterValue("Account Branch", "spnPM3", (paymentMethod != "ACH") ? true : false);
                internet_page.enterValue("n/a", "spnTIAuthCode", false);
                internet_page.enterValue("n/a", "spnTIResponse", false);
            }
            internet_page.enterValue(internet_page._arrCalc[3] + " " + internet_page._arrCalc[29], "spnTINetToBenefAmount", false);
            (internet_page._isAgent == true) ? internet_page.enterValue(internet_page.getDDLValue("ddlRemittingCurrency", false) + " to " + internet_page._arrCalc[3] + " " + internet_page._arrCalc[1], "spnTIForex", false) : internet_page.enterValue(internet_page.getValue("spnSourceCurrency", true) + " to " + internet_page._arrCalc[3] + " " + internet_page._arrCalc[1], "spnTIForex", false);
            internet_page.isSearch = false;
            internet_page._accountno = "";
            internet_page.showTransactionLoading("", true);
            IRSWorld.Internet.T__Remitter.updateOAAM(internet_page._fcSuccess, internet_page._refno, internet_page._erefno);
            internet_page._fcSuccess = false;
            internet_page._refno = "";
            internet_page._erefno = "";
            internet_page._isSubmit = false;
            
            internet_page.submitClearingDays();
        }
    }
    
    this.transactionSuccess = function(message)
    {
        internet_page.showTransactionLoading("", true);
        var response = "Transaction successfully commited with reference number: " + internet_page.getValue("spnReferenceNo", false) + ".";
        if (message == "")
        {
            internet_page.showAlert(response);
            internet_page._fcSuccess = true;
        }
        else
        {
            var arrSplit = message.split("|");
            if (internet_page._isAgent == true)
            {
                internet_page.showAlert(response + "\r\n\r\n FC Response: " + arrSplit[0] + "\r\n Status:" + arrSplit[2]);
            }
            else
            {
                internet_page.showAlert(response);// + "\r\n\r\n Status: PROCESSED"
            }
            if (arrSplit[1] == "9")
            {
                internet_page._fcSuccess = false;
            }
            else
            {
                internet_page._fcSuccess = true;
            }
        }
        if (internet_page._isAgent == true)
        {
            if (confirm("Do you want to receive your transaction receipt thru email?"))
            {
                internet_page.showTransactionLoading("Sending transaction receipt thru email. Please wait...", false);
                internet_page.getOREmail(internet_page._refno, internet_page._txnseqid);
            }
            else
            {
                internet_page.showPrintReceipt();
            }
        }
        else
        {
            if (confirm("Do you want to receive your transaction receipt thru email?"))
            {
                internet_page.showTransactionLoading("Sending transaction receipt thru email. Please wait...", false);
                internet_page.getOREmail(internet_page._refno, internet_page._txnseqid);
            }
            else
            {
                internet_page.showPrintReceipt();
            }
        }
        
        this.getTotalRemittance();//
    }
    
    this.showPrintReceipt = function()
    {
        if (internet_page._isAgent == true)
        {
            if (confirm("Do you want to view transaction receipt?"))
            {
                internet_page._showHistory = true;
                internet_page.getRemittance(internet_page.getValue("spnReferenceNo", false));
                internet_page.load_grid_viewer();
                if (internet_page._branchCode == "E000")
                {
                    var isloaded = internet_page.getEhudReceipt(internet_page._refno);
                    if (isloaded.toString() == "true")
                    {
                        window.open('../Reports/Pages/Report_viewer.aspx?ehud');
                    }
                }
                else
                {
                    show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OTC&tranvalue='+ internet_page._txnseqid.toString(), 'internetRemitterClose', '100%', '100%', '0','0','250',true);
                }
            }
        }
        else
        {
            internet_page._showHistory = true;
            internet_page.getRemittance(internet_page.getValue("spnReferenceNo", false));
            internet_page.load_grid_viewer();
             //show_frame_dialog('../Reports/Pages/Report_viewer.aspx?trantype=OTC&tranvalue='+ internet_page._txnseqid.toString(), null , '100%', '100%', '0',null, null, true);
           window.open('../Reports/Pages/Report_viewer.aspx?trantype=OTC&tranvalue='+ internet_page._txnseqid.toString());  
        }
        internet_page.createTransaction("transaction");
        internet_page.showControl("divCharges",false);
        internet_page.edButtons("txtAmount",false);
        internet_page.edButtons("btnSubmit", true);
    }
    
    this.getEhudReceipt = function(refno)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getEhudReceipt(refno).value;
    }
    
    this.processTransaction = function(seqid)
    {
        var response = IRSWorld.Internet.T__Remitter.processTransaction(seqid).value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.transactionSuccess(response);
        }
        else
        {
            internet_page.transactionSuccess("");
        }
    }
    
    this.submitRemittanceCharges = function(seqid)
    {
        var response = IRSWorld.Internet.T__Remitter.submitRemittanceCharges(seqid).value;
        if (response != null && response != "" && response != undefined)
        {
            if (response != "1")
            {
                internet_page.showAlert("Transaction was successfully committed however error was encountered while saving charges. " + response + ". \r\n\r\n Please report to the nearest PNB branch.");
            }
        }
    }
    
    this.submitORDCharges = function(seqid, amount, productcode, implementationtype, targetcurrency, computationlevel)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.submitORDCharges(seqid, amount, productcode, implementationtype, targetcurrency, computationlevel, this.submitORDCharges_callback);
    }
    
    this.submitORDCharges_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response != "1")
            {
                if (response != "NOCHARGE")
                {
                    internet_page.showAlert("There was a problem saving ORD charges. " + response + ". \r\n\r\n Please report to the nearest PNB branch.");
                }
            }
        }
    }
    
    this.submitRemittanceCards = function(remittanceseqid, remitteracct, message, response, authcode, iscard)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.submitRemittanceCards(String(remittanceseqid), remitteracct, message, response, authcode, iscard, this.submitRemittanceCards_callback);
    }
    
    this.submitRemittanceCards_callback = function(result)
    {
        var response = result.value;
        if (response != "1")
        {
           // internet_page.showAlert("Transaction was successfully committed however error was encountered while saving card information. " + response + ". \r\n\r\n Please report to the nearest PNB branch.");
        }
    }
    
    this.getLocalTime = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getLocalTime(this.getLocalTime_callback);
    }

    this.getLocalTime_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "lblLocalTime", false);
        }
        internet_page.showLoading("", true);
    }
    
    this.getServerTime = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getServerTime(this.getServerTime_callback);
    }

    this.getServerTime_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response, "lblServerTime", false);
        }
        internet_page.showLoading("", true);
    }
    
    this.getGFCAggregate = function()
    {
        internet_page.ajaxTimeout();
        var beneficiary = benef_param[document.getElementById("ddlBeneficiary").selectedIndex - 1].split(",");  //internet_page.getDDLValue("ddlBeneficiary", false).split(",");
        var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
        
        if (beneficiary_value[1] == "10" || beneficiary_value[1] == "22") //Rapid Remit & GFC
        {
            if (beneficiary[3] != null && beneficiary[3] != "" && beneficiary[3] != undefined)
            {
                internet_page._accountno = (beneficiary_value[1] == "22") ? IRSWorld.Internet.T__Remitter.getCIFAccount(beneficiary_value[3]).value : beneficiary[3];
                if (internet_page._arrCalc != null || internet_page._arrCalc != undefined)
                {
                    if (internet_page._arrCalc[28] == null || internet_page._arrCalc[28] == .00)
                    {
                        internet_page.getCharges(false);
                    }
                    internet_page.showTransactionLoading("Please wait....", false);
                    return IRSWorld.Internet.T__Remitter.getGFCAggregate(beneficiary_value[0], internet_page._accountno, beneficiary_value[4], internet_page._arrCalc[28], this.getGFCAggregate_callback);
                }
                else
                {
                    internet_page.showAlert("There is no applicable charge for " + internet_page.getValue("spnRemittingCurrency", true) + " currency. Please select another currency.");
                }
            }
            else
            {
                internet_page.showAlert("The account number is missing. Transaction cannot be process.");
                //internet_page.transmitRemittance();
            }
        }
        else
        {
            internet_page.transmitRemittance();
        }
    }
    
    this.getGFCAggregate_callback = function(result)
    {
        var response = result.value;
        if (response == true)
        {
            internet_page.transmitRemittance();
        }
        else
        {
            internet_page.showAlert("You have exceeded the maximum GFC aggregated amount for this account. Please enter a smaller amount.");
        }
        //internet_page.showLoading("", true);
        internet_page.showTransactionLoading("", true);
    }
    
    this.getOSA = function(beneficiaryid)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getOSA(beneficiaryid, this.getOSA_callback);
    }
    
    this.getOSA_callback = function(result)
    {
        internet_page._isOSA = false;
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == true || response == "true")
            {
                internet_page._isOSA = true;
                internet_page.showAlert("Destination is not a serviceable area. Please transact using advise and pay or go to the nearest PNB branch.");
            }
        }
        else
        {
            internet_page._isOSA = false;
        }
    }
    
    this.getOnlineProduct = function(productcode)
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getOnlineProduct(productcode, this.getOnlineProduct_callback);
    }
    
    this.getOnlineProduct_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            if (response == true || response == "true")
            {
                //internet_page.showControl("btnBAccountInquiry", true); //DISABLED DUE TO COMPLIANCE ISSUE 20110920
            }
            else
            {
                //internet_page.showControl("btnBAccountInquiry", false);
            }
        }
        else
        {
            //internet_page.showControl("btnBAccountInquiry", false);
        }
    }
    
    this.getAccountInquiry = function()
    {
//        internet_page.ajaxTimeout();
        internet_page.ajaxTimeout();
        //internet_page.showAccountLoading("Retrieving account inquiry. Please wait...", false);
        internet_page.showTransactionLoading("Please wait....", false);
        //internet_page.edButtons("btnBAccountInquiry", true); //DISABLED DUE TO COMPLIANCE ISSUE 20110920
//        internet_page.edButtons("btnBClosePanel", true);
        var accountnumber = internet_page.getValue("spnAccountNumber", false);
        var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
        if (internet_page.trim(accountnumber) != "")
        {
            if (parseInt(accountnumber))
            {
                if (accountnumber.length > 12 && beneficiary_value[1] == "22")
                {
                    internet_page.showTransactionLoading("", false);
                    internet_page.checkTransaction_final();
                }
                else if (beneficiary_value[1] == "8" || beneficiary_value[1] == "9" || beneficiary_value[1] == "27" || beneficiary_value[1] == "12" || beneficiary_value[1] == "13" || beneficiary_value[1] == "16")
                {
                    internet_page.showTransactionLoading("", false);
                    internet_page.checkTransaction_final();
                }
                else
                {
                    if (Number(accountnumber) > 0 && accountnumber.length >= 10)
                    {
                        return IRSWorld.Internet.T__Remitter.getAccountInquiry(accountnumber, this.getAccountInquiry_callback);
                    }
                    else
                    {
                        //internet_page.showAccountLoading("", true);
                        internet_page.showTransactionLoading("", false);
    //                    internet_page.edButtons("btnBAccountInquiry", false);
    //                    internet_page.edButtons("btnBClosePanel", false);
                        internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
                    }
                }
            }
            else if (beneficiary_value[1] == "8" || beneficiary_value[1] == "9" || beneficiary_value[1] == "27" || beneficiary_value[1] == "12" || beneficiary_value[1] == "13" || beneficiary_value[1] == "16")
            {
                internet_page.showTransactionLoading("", false);
                internet_page.checkTransaction_final();
            }
            else
            {
                //internet_page.showAccountLoading("", true);
                internet_page.showTransactionLoading("", false);
//                internet_page.edButtons("btnBAccountInquiry", false);
//                internet_page.edButtons("btnBClosePanel", false);
                internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
            }
        }
        else if (beneficiary_value[1] == "8" || beneficiary_value[1] == "9" || beneficiary_value[1] == "27" || beneficiary_value[1] == "12" || beneficiary_value[1] == "13" || beneficiary_value[1] == "16")
        {
            //internet_page.showAccountLoading("", true);
            internet_page.showTransactionLoading("", false);
//            internet_page.edButtons("btnBAccountInquiry", false);
//            internet_page.edButtons("btnBClosePanel", false);
            //internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
            internet_page.checkTransaction_final();
        }
        else
        {
            internet_page.showTransactionLoading("", false);
            internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
        }
    }
    
    this.getAccountInquiry_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            //internet_page.showAccountLoading("", true);
            internet_page.showTransactionLoading("", false);
            //internet_page.showAlert(response);
            if (response.toUpperCase().indexOf("DORMANT") >= 0 || response.toUpperCase().indexOf("CLOSED") >= 0)
            {
                internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
            }
            else
            {
                internet_page.checkTransaction_final();
            }
        }
        else
        {
            internet_page.showAlert("Please check account status of your beneficiary. Kindly call branch.");
        }
        internet_page.showTransactionLoading("", true);
//        internet_page.edButtons("btnBAccountInquiry", false);
//        internet_page.edButtons("btnBClosePanel", false);
    }
    
    this.getTodayForex = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getTodayForex(this.getTodayForex_callback);
    }
    
    this.getTodayForex_callback = function(result)
    {
        var response = result.value;
        var forexValue;
        var tr1 = "<tr><td width='70%' style='background-color:#26A6E1; font-size:10px; font-family: Arial; height:1px; text-align:left;color:white;margin-left:auto;margin-right:auto;'>";
        var tr2 = "<tr><td width='70%' style='font-size:10px; font-family: Arial; height:1px; text-align:left; color:white;margin-left:auto;margin-right:auto;'>";
        var tr3 = "</td><td width='30%' style='background-color:#26A6E1; font-size:10px; font-family: Arial; height:1px; text-align:right;color:white;margin-left:auto;margin-right:auto;'>";
        var tr4 = "</td><td width='30%' style='font-size:10px; font-family: Arial; height:1px; text-align:right; color:white;margin-left:auto; margin-right:auto;'>";
        var tr5 = "</td></tr>";
        var forexRate;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.enterValue(response.Rows[0]["TranxDate"], "spnForexDate", true);
            forexValue = "<table width='210px'>";
            for (var i = 0; i <= (response.Rows.length - 1); i++)
            {
                forexRate = String(response.Rows[i]["SourceToTarget"].toFixed(4));
                if (forexRate.indexOf('.') < 0)
                {
                    forexRate = forexRate + ".0000";
                }
                if (i % 2 == 0)
                {
                    forexValue = forexValue + tr1 + response.Rows[i]["Forex"] + tr3 + forexRate + tr5;
                }
                else
                {
                    forexValue = forexValue + tr2 + response.Rows[i]["Forex"] + tr4 + forexRate + tr5;
                }
            }
            forexValue = forexValue + "</table>";
            internet_page.enterValue(forexValue, "spnForexValue", true);
        }
    }
    
    this.getOREmail = function(referenceno, txnseqid)
    {
        internet_page.ajaxTimeout();
        internet_page.showTransactionLoading("Sending OR thru Email, please wait..", false);
        return IRSWorld.Internet.T__Remitter.sendOREmail(referenceno.toString(), txnseqid.toString(), this.getOREmail_callback);
    }
    
    this.getOREmail_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            internet_page.showAlert(response);
            //internet_page.hidePanel('divTransactionHistoryModal');
        }
        
        
        
        internet_page.showPrintReceipt();
        internet_page.showTransactionLoading("", true);
        
    }
    
    this.showDisclaimer = function()
    {
        internet_page.ajaxTimeout();
        return IRSWorld.Internet.T__Remitter.getDisclaimer(this.showDisclaimer_callback);
    }

    this.showDisclaimer_callback = function(result)
    {
        var response = result.value;
        if (response != null && response != "" && response != undefined)
        {
            //internet_page.enterValue(response, "spnDisclaimerAlert", true);
            //internet_page.showPanel("divDisclaimerAlert");
            internet_page._accept = true;
            internet_page.checkTransaction();
        }
    }
    
//    this.showDisclaimer = function()
//    {
//        internet_page.ajaxTimeout();
//        return IRSWorld.Internet.InternetPage.updateDisclaimer(internet_page._remitterid, false, this.showDisclaimer_callback);
//    }
//    
//    this.showDisclaimer_callback = function(result)
//    {
//        var response = result.value;
//        if (response != null && response != "" && response != undefined)
//        {
//            internet_page.showAlert(response);
//        }
//    }
    
    // **************************************************************************************** //
    // *** Custom Validation
    // **************************************************************************************** //
    this.getCIFInfo = function(type, isnext)
    {
        switch (type)
        {
            case "Address":
                if (isnext == true)
                {
                    internet_page._ARow = internet_page._ARow + 1;
                }
                else
                {
                    internet_page._ARow = internet_page._ARow - 1;
                }
                
                if (internet_page.tblBeneficiaryAddress != undefined)
                {
                    if (internet_page._ARow > (internet_page.tblBeneficiaryAddress.Rows.length - 1))
                    {
                        internet_page.showAlert("You have reached the last beneficiary address.");
                        internet_page._ARow = internet_page.tblBeneficiaryAddress.Rows.length - 1;
                    }
                    else
                    {
                        if (internet_page._ARow == -1)
                        {
                            internet_page.showAlert("You have reached the first beneficiary address.");
                            internet_page._ARow = 0;
                        }
                        else
                        {
                            internet_page.removeValue("spnAddress", false);
                            internet_page.enterValue(internet_page.getTableValueRow(internet_page.tblBeneficiaryAddress, "Address", internet_page._ARow), "spnAddress", false);
                            internet_page._beneficiaryAddressSEQID = internet_page.tblBeneficiaryAddress.Rows[internet_page._ARow]["SEQ_ID"];
                        }
                    }
                }
                break;
            
            case "Contacts":
                if (isnext == true)
                {
                    internet_page._CRow = internet_page._CRow + 1;
                }
                else
                {
                    internet_page._CRow = internet_page._CRow - 1;
                }
                
                if (internet_page.tblBeneficiaryContacts != undefined)
                {
                    if (internet_page._CRow > (internet_page.tblBeneficiaryContacts.Rows.length - 1))
                    {
                        internet_page.showAlert("You have reached the last beneficiary contact.");
                        internet_page._CRow = internet_page.tblBeneficiaryContacts.Rows.length - 1;
                    }
                    else
                    {
                        if (internet_page._CRow == -1)
                        {
                            internet_page.showAlert("You have reached the first beneficiary contact.");
                            internet_page._CRow = 0;
                        }
                        else
                        {
                            internet_page.removeValue("spnContacts", false);
                            internet_page.enterValue(internet_page.getTableValueRow(internet_page.tblBeneficiaryContacts, "ContactNo", internet_page._CRow), "spnContacts", false);
                            internet_page._beneficiaryContactSEQID = internet_page.tblBeneficiaryContacts.Rows[internet_page._CRow]["SEQ_ID"];
                        }
                    }
                }
                break;
                
            case "Email":
                if (isnext == true)
                {
                    internet_page._ERow = internet_page._ERow + 1;
                }
                else
                {
                    internet_page._ERow = internet_page._ERow - 1;
                }
                
                if (internet_page.tblBeneficiaryEmail != undefined)
                {
                    if (internet_page._ERow > (internet_page.tblBeneficiaryEmail.Rows.length - 1))
                    {
                        internet_page.showAlert("You have reached the last beneficiary email.");
                        internet_page._ERow = internet_page.tblBeneficiaryEmail.Rows.length - 1;
                    }
                    else
                    {
                        if (internet_page._ERow == -1)
                        {
                            internet_page.showAlert("You have reached the first beneficiary email.");
                            internet_page._ERow = 0;
                        }
                        else
                        {
                            internet_page.removeValue("spnEmailAddress", false);
                            internet_page.enterValue(internet_page.getTableValueRow(internet_page.tblBeneficiaryEmail, "EmailAddress", internet_page._ERow), "spnEmailAddress", false);
                            internet_page._beneficiaryEmailSEQID = internet_page.tblBeneficiaryEmail.Rows[internet_page._ERow]["SEQ_ID"];
                        }
                    }
                }
                break;
        }
    }
    
    this.submitGFC = function(refno)
    {
        var beneficiary = benef_param[document.getElementById("ddlBeneficiary").selectedIndex - 1].split(","); //internet_page.getDDLValue("ddlBeneficiary", false).split(",");
        var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
        IRSWorld.Internet.T__Remitter.submitGFC(refno, internet_page._accountno, beneficiary_value[4], internet_page._arrCalc[28]).value;
    }
    
    this.generateORNo = function()
    {
        IRSWorld.Internet.T__Remitter.generateORNo().value;
    }
    
    this.transmitRemittance = function()
    {
        internet_page.ajaxTimeout();
        if (internet_page.validateTransaction(true, true) == true)
        {
            if (internet_page._isoverride == false)
            {
                internet_page.getCharges(false); //did not went to compute button
            }
            internet_page._transmitremittance = 1;
        }
        else
        {
            internet_page.showTransactionLoading("", true);
        }
    }
    
    this.transmitRemittance_final = function()
    {
        if (internet_page._chargeIsValid == true)
        {
            if (internet_page._isDC == "1")
            {
                internet_page.showPanel("divSecurity");
                internet_page.showFocus("txtSecurityCode");
            }
            else if (internet_page._isDC == "3")
            {
                internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                //internet_page.showLoading("", true);
                //internet_page.showPGLoading("", true);
                internet_page.enterValue("123", "txtSecurityCode", false);
                internet_page.submitTransaction();
            }
            else if (internet_page._isDC == "4")
            {
                internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                //internet_page.showLoading("", true);
                //internet_page.showPGLoading("", true);
                internet_page.enterValue("123", "txtSecurityCode", false);
                internet_page.submitTransaction();
            }
            else
            {
                internet_page.getORNo();
            }
        }
        else
        {
            internet_page.showAlert("There is no applicable charge for " + internet_page.getValue("spnRemittingCurrency", true) + " currency. Please select another currency.");
        }
    }
    
    this.createRemittance = function()
    {
        var remittance = "";
        var remitMethod = "";
        var beneAddress = "";
        var beneContacts = "";
        
        internet_page._auditRemittance = "";
        (internet_page._isAgent == true) ? remitMethod = internet_page.findValue(internet_page.tblRemitMethod, "Agent Remittance", "FldDescription", "FldValue") : remitMethod = internet_page.findValue(internet_page.tblRemitMethod, "Internet Remittance", "FldDescription", "FldValue");
        
        if (remitMethod == "")
        {
            (internet_page._isAgent == true) ? remitMethod = "8" : remitMethod = "5";
        }
        internet_page._tranStatus = internet_page.getTableValue(internet_page.tblChargesDefault, "DefaultStatus");
        
        
       
  
        
        //if(parseFloat(internet_page._arrCalc[24].replace(',','')) >= parseFloat(maxStraight))
        //{
          //  internet_page._tranStatus = "14";
        //}
        //else
        //{
          //  internet_page._tranStatus = "25";
        //}
       
        if (internet_page.isNumeric(internet_page._beneficiaryAddressSEQID) == false)
        {
            beneAddress = "0";
        }
        else
        {
            beneAddress = internet_page._beneficiaryAddressSEQID;
        }
        
        if (internet_page.isNumeric(internet_page._beneficiaryContactSEQID) == false)
        {
            beneContacts = "0";
        }
        else
        {
            beneContacts = internet_page._beneficiaryContactSEQID;
        }
        
        remittance = internet_page.getValue("lblUserID", false)                                 + "|"; //UserID        
        remittance += remitMethod                                                               + "|"; //RemitMethod
        remittance += "NULL"                                                                    + "|"; //RemitStationID
        remittance += internet_page.getValue("lblLocation", false)                              + "|"; //RemittingBranch
        remittance += "FALSE"                                                                   + "|"; //ChargesLocally
        remittance += "NULL"                                                                    + "|"; //ReferenceNo
        remittance += internet_page.getValue("lblORNo", false)                                  + "|"; //ExtlReferenceNo
        remittance += internet_page._tranStatus                                                 + "|"; //Transtatus
        remittance += internet_page.getValue("lblTransactionDate", false)                       + "|"; //TranxDate
        remittance += internet_page._arrCalc[39]                                                + "|"; //ProductChargesID
        remittance += internet_page.getValue("lblRemitterID", false)                            + "|"; //RemitterCIFID
        remittance += internet_page.getValue("lblBeneficiaryID", false)                         + "|"; //BeneficiaryCIFID
        remittance += internet_page.getValue("lblBenAcctSeqID", false)                          + "|"; //AccountSEQID
        remittance += internet_page._arrCalc[35]                                                + "|"; //RemittingCurrency
        remittance += internet_page._arrCalc[0]                                                 + "|"; //LocalCurrency
        remittance += internet_page._arrCalc[24]                                                + "|"; //GrossRemAmount
        remittance += internet_page._arrCalc[27]                                                + "|"; //NetRemAmount
        remittance += internet_page._arrCalc[14]                                                + "|"; //NettoBeneCurrency
        remittance += internet_page._arrCalc[29]                                                + "|"; //NettoBeneAmount
        remittance += internet_page._arrCalc[28]                                                + "|"; //NettoBeneAmountPHP
        remittance += internet_page._arrCalc[31]                                                + "|"; //TotalLocalCharge
        remittance += internet_page.getTableValue(internet_page.tblDefaults, "TranxSource")     + "|"; //TransactionSource
        remittance += internet_page._receiptMode                                                + "|"; //ReceiptMode
        remittance += internet_page.getValue("lblPurpose", false)                               + "|"; //TransactionPurpose
        remittance += internet_page.getTableValue(internet_page.tblDefaults, "SourceOfFunds")   + "|"; //SourceofFunds
        remittance += "NULL"                                                                    + "|"; //OtherPurpose
        remittance += "NULL"                                                                    + "|"; //SpecialMessage
        remittance += "FALSE"                                                                   + "|"; //Printed
        remittance += internet_page._arrCalc[13]                                                + "|"; //ForexRemittingToAccount
        remittance += internet_page._arrCalc[20]                                                + "|"; //ForexRemittingToLocal
        remittance += internet_page._arrCalc[21]                                                + "|"; //ForexRemittingToUSD
        remittance += internet_page._arrCalc[18]                                                + "|"; //ForexRemittingToPHP
        remittance += internet_page._arrCalc[16]                                                + "|"; //ForexRemittingToEUR
        remittance += internet_page._arrCalc[7]                                                 + "|"; //ForexLocalToRemitting
        remittance += internet_page._arrCalc[10]                                                + "|"; //ForexLocalToUSD
        remittance += internet_page._arrCalc[4]                                                 + "|"; //ForexLocalToPHP
        remittance += internet_page.getValue("lblLocalTime", false)                             + "|"; //DatePostedOverseas
        remittance += internet_page.getValue("lblLocalTime", false)                             + "|"; //DatePostedLocal
        remittance += internet_page.getValue("lblServerTime", false)                            + "|"; //DateImplementedOverseas
        remittance += internet_page.getValue("lblServerTime", false)                            + "|"; //DateImplementedLocal
        remittance += "0"                                                                       + "|"; //BatchNo
        remittance += internet_page._arrCalc[38]                                                + "|"; //Total_Backend_Charges
        remittance += beneAddress                                                               + "|"; //BeneficiaryAddressSEQID
        remittance += beneContacts                                                              + "|"; //BeneficiaryContactSEQID
        remittance += ""                                                                        + "";  //RemitterAccount
        
        internet_page._auditRemittance = "UserID~"                     + internet_page.getValue("lblUserID", false) + "|";
        internet_page._auditRemittance += "RemitMethod~"               + remitMethod + "|";
        internet_page._auditRemittance += "RemitStationID~NULL|";
        internet_page._auditRemittance += "RemittingBranch~"           + internet_page.getValue("lblLocation", false) + "|";
        internet_page._auditRemittance += "ChargesLocally~FALSE|";
        internet_page._auditRemittance += "ReferenceNo~NULL|";
        internet_page._auditRemittance += "ExtlReferenceNo~"           + internet_page.getValue("lblORNo", false) + "|";
        internet_page._auditRemittance += "Transtatus~"                + internet_page._tranStatus + "|";
        internet_page._auditRemittance += "TranxDate~"                 + internet_page.getValue("lblTransactionDate", false) + "|";
        internet_page._auditRemittance += "ProductChargesID~"          + internet_page._arrCalc[39] + "|";
        internet_page._auditRemittance += "RemitterCIFID~"             + internet_page.getValue("lblRemitterID", false) + "|";
        internet_page._auditRemittance += "BeneficiaryCIFID~"          + internet_page.getValue("lblBeneficiaryID", false) + "|";
        internet_page._auditRemittance += "AccountSEQID~"              + internet_page.getValue("lblBenAcctSeqID", false) + "|";
        internet_page._auditRemittance += "RemittingCurrency~"         + internet_page._arrCalc[35] + "|";
        internet_page._auditRemittance += "LocalCurrency~"             + internet_page._arrCalc[0] + "|";
        internet_page._auditRemittance += "GrossRemAmount~"            + internet_page._arrCalc[24] + "|";
        internet_page._auditRemittance += "NetRemAmount~"              + internet_page._arrCalc[27] + "|";
        internet_page._auditRemittance += "NettoBeneCurrency~"         + internet_page._arrCalc[14] + "|";
        internet_page._auditRemittance += "NettoBeneAmount~"           + internet_page._arrCalc[29] + "|";
        internet_page._auditRemittance += "NettoBeneAmountPHP~"        + internet_page._arrCalc[28] + "|";
        internet_page._auditRemittance += "TotalLocalCharge~"          + internet_page._arrCalc[31] + "|";
        internet_page._auditRemittance += "TransactionSource~"         + internet_page.getTableValue(internet_page.tblDefaults, "TranxSource") + "|";
        internet_page._auditRemittance += "ReceiptMode~"               + internet_page._receiptMode + "|";
        internet_page._auditRemittance += "TransactionPurpose~"        + internet_page.getValue("lblPurpose", false) + "|";
        internet_page._auditRemittance += "SourceofFunds~"             + internet_page.getTableValue(internet_page.tblDefaults, "SourceOfFunds") + "|";
        internet_page._auditRemittance += "OtherPurpose~NULL|";
        internet_page._auditRemittance += "SpecialMessage~NULL|";
        internet_page._auditRemittance += "Printed~FALSE|";
        internet_page._auditRemittance += "ForexRemittingToAccount~"   + internet_page._arrCalc[13] + "|";
        internet_page._auditRemittance += "ForexRemittingToLocal~"     + internet_page._arrCalc[20] + "|";
        internet_page._auditRemittance += "ForexRemittingToUSD~"       + internet_page._arrCalc[21] + "|";
        internet_page._auditRemittance += "ForexRemittingToPHP~"       + internet_page._arrCalc[18] + "|";
        internet_page._auditRemittance += "ForexRemittingToEUR~"       + internet_page._arrCalc[16] + "|";
        internet_page._auditRemittance += "ForexLocalToRemitting~"     + internet_page._arrCalc[7] + "|";
        internet_page._auditRemittance += "ForexLocalToUSD~"           + internet_page._arrCalc[10] + "|";
        internet_page._auditRemittance += "ForexLocalToPHP~"           + internet_page._arrCalc[4] + "|";
        internet_page._auditRemittance += "DatePostedOverseas~"        + internet_page.getValue("lblLocalTime", false) + "|";
        internet_page._auditRemittance += "DatePostedLocal~"           + internet_page.getValue("lblLocalTime", false) + "|";
        internet_page._auditRemittance += "DateImplementedOverseas~"   + internet_page.getValue("lblServerTime", false) + "|";
        internet_page._auditRemittance += "DateImplementedLocal~"      + internet_page.getValue("lblServerTime", false) + "|";
        internet_page._auditRemittance += "BatchNo~0|";
        internet_page._auditRemittance += "Total_Backend_Charges~"     + internet_page._arrCalc[38] + "|";
        internet_page._auditRemittance += "BeneficiaryAddressSEQID~"   + beneAddress + "|";
        internet_page._auditRemittance += "BeneficiaryContactSEQID~"   + beneContacts + "|";
        internet_page._auditRemittance += "RemitterAccountNo~"   + "";

        return remittance;
    }
    
    this.computeRemittance = function(recompute)
    {
        if (internet_page.validateTransaction(false, false) == true)
        {
            if (recompute == true)
            {
                internet_page._isoverride = true;
                internet_page.setOverride();
                internet_page.getCharges(true);
                internet_page.edButtons("btnSubmit", true);
            }
            else
            {
                internet_page._isoverride = false;
                internet_page.getCharges(false);
            }
            if (internet_page._chargeResponse != "NOCHARGE" && internet_page._chargeResponse != "NOTALLOWED")
            {
                //internet_page.showPanel("divCharges");
                internet_page.showControl("divCharges",true);
                internet_page.edButtons("txtAmount",true);
            }
        }
        else
        {
            internet_page.showTransactionLoading("", true);
        }
    }
    
    this.setOverride = function()
    {
        IRSWorld.Internet.T__Remitter.setOverride(internet_page._arrCalc[1], internet_page.getValue("txtCExchangeRate", false), internet_page._arrCalc[31], internet_page.getValue("txtCBankChargeAmount", false));
    }
    
    this.validateTransaction = function(showexceed, issubmit)
    {
        if (document.getElementById("ddlBeneficiary").selectedIndex <= 0)
        {
            internet_page.showAlert("Please select a beneficiary.");
            return false;
        }
        if (document.getElementById("ddlPurpose").selectedIndex <= 0)
        {
            internet_page.showAlert("Please select a purpose.");
            return false;
        }
        if (document.getElementById("ddlPaymentMethod").selectedIndex <= 0)
        {
            internet_page.showAlert("Please select a payment method.");
            return false;
        }
//        if (document.getElementById("ddlComputation").selectedIndex <= 0)
//        {
//            internet_page.showAlert("Please select a computation.");
//            return false;
//        }
        if (internet_page._isAgent == true)
        {
            if (document.getElementById("ddlRemittingCurrency").selectedIndex <= 0)
            {
                internet_page.showAlert("Please select a remitting currency.");
                return false;
            }
            if (document.getElementById("ddlChargeType").selectedIndex <= 0)
            {
                internet_page.showAlert("Please select a charge type.");
                return false;
            }
        }
        else
        {
            if (document.getElementById("ddlDCCard").selectedIndex <= 0)
            {
                if (internet_page._isDC == "1")
                {
                    internet_page.showAlert("Please select a debit/credit card.");
                    return false;
                }
                else if (internet_page._isDC == "3")
                {
                    internet_page.showAlert("Please select a bank account.");
                    return false;
                }
            }
        }
        if (internet_page.getValue("txtAmount", false) == "" || internet_page.getValue("txtAmount", false) == "0.00" || Number(internet_page.getValue("txtAmount", false)) <= 0 || internet_page.getValue("txtAmount", false) == ".")
        {
            internet_page.showAlert("Please enter an amount.");
            return false;
        }
        else
        {
            if (showexceed == true)
            {
                //var total_remittance = Number(parseFloat(internet_page.getValue("txtCGrossAmount", false).replace(",", "")).toFixed(2)) + Number(parseFloat(internet_page.getValue("txtTotalRemitted", false).replace(",", "")).toFixed(2));
                //if (total_remittance > Number(internet_page._internetLimit))
                //{
                  //  internet_page.showAlert("You have exceeded the T$$ transaction limit. Please enter a smaller amount.");
                    //return false;
               // }
            }
        }
        if (internet_page._exchangeRateValid == false)
        {
            internet_page.showAlert("There's no exchange rate defined. Please report to the nearest PNB branch and/or system administrator.");
            return false;
        }
        var nettobeneamount = internet_page.getValue("txtCNetToBenefAmount", false);
        if (issubmit == true && Number(nettobeneamount) <= 0)
        {
            internet_page.showAlert("Target amount is invalid. Please check amount to be received by beneficiary.");
            return false;
        }
        else
        {
            if (issubmit == true && nettobeneamount.indexOf('-') >= 0)
            {
                internet_page.showAlert("Target amount is invalid. Please check amount to be received by beneficiary.");
                return false;
            }
        }
        if (internet_page.tblRemitMethod == "" || internet_page.tblRemitMethod == null || internet_page.tblRemitMethod == undefined)
        {
            internet_page.showAlert("No activated remit method. Please contact SAPD for setup.");
            return false;
        }
        /*
        if (internet_page.tblCurrency == "" || internet_page.tblCurrency == null || internet_page.tblCurrency == undefined)
        {
            internet_page.showAlert("No activated currency. Please contact SAPD for setup.");
            return false;
        }
        */
        if (internet_page.tblReceiptMode == "" || internet_page.tblReceiptMode == null || internet_page.tblReceiptMode == undefined)
        {
            internet_page.showAlert("No activated receipt mode. Please contact SAPD for setup.");
            return false;
        }
        if (internet_page.tblDefaults == "" || internet_page.tblDefaults == null || internet_page.tblDefaults == undefined)
        {
            internet_page.showAlert("No branch default found. Please contact SAPD for setup.");
            return false;
        }
        if (internet_page._isOSA == true)
        {
            internet_page.showAlert("Destination is not a serviceable area. Please transact using advise and pay or go to the nearest PNB branch.");
            return false;
        }
        
        var setup = IRSWorld.Internet.T__Remitter.getBranchSetup().value;
        var minStraight = 0;
        var maxStraight = 0;
        var amount = parseFloat(internet_page.getValue("txtAmount", false).replace(',',''));
        var remainingamount = parseFloat(internet_page.getValue("spnNetToBeneAmount", false));
        

        if(setup.Rows.length > 0)
        {
            minStraight = setup.Rows[0]["TDD_StImp_Min"];
            maxStraight = setup.Rows[0]["TDD_StImp_Max"];
            maxClearing = setup.Rows[0]["TDD_ClDays_Max"];
        }
        else
        {
            alert('Cannot proceed with the transaction, branch setup required');
            return false;
        }
        var paymentMethod = internet_page.getDDLValue("ddlPaymentMethod", false);
        var whaticansend = document.getElementById('txtTotalRemitted').value;
        
        if(amount > maxClearing)
        {
            alert('You have exceeded monthly limit of USD' + maxClearing + '. Please contact PNB RCI to know how you can send more than your monthly limit');
            return false;
        }
        if(amount > whaticansend)
        {
            alert('You have exceeded monthly limit of USD' + maxClearing + '. Please contact PNB RCI to know how you can send more than your monthly limit');
            return false;
        }
        if(whaticansend <= 0)
        {
            alert('You have exceeded monthly limit of USD' + maxClearing + '. Please contact PNB RCI to know how you can send more than your monthly limit');
            return false;
        }
        
        
        
        if(paymentMethod == "First Data" || paymentMethod == "Credit Card")
        { 
            if(amount > 2000)
            {
                alert('You have exceeded daily limit of USD 2000 . Please contact PNB RCI to know how you can send more than your monthly limit');
                return false;
            }
            
        }
        
        
        
        if(paymentMethod == "ACH" || paymentMethod == "US Savings/ Checking Account")
        {
        
            var firsttime = IRSWorld.Internet.T__Remitter.first_time_remitter(internet_page.getValue("lblRemitterID", false)).value;
            //var id_approved = IRSWorld.Internet.T__Remitter.check_if_id_approved(internet_page.getValue("lblRemitterID", false)).value;
            if(firsttime)
            {
                if(amount > maxStraight)
                {
                    alert('You have exceeded monthly limit of USD' + maxStraight + '. Please contact PNB RCI to know how you can send more than your monthly limit');
                    return false;
                }
                
                internet_page._tranStatus = "25";
            }
            else
            {
                
                if(amount >maxStraight && amount <= maxClearing)
                {
                       if(confirm("Subject to 3 day clearing, would you like to continue with your transaction?"))
                       {
                            internet_page._tranStatus = "14";
                            return true;
                       }
                       else
                       {
                            return false;
                       }
                }
                
            }
            
            
        }
        
        
        return true;
    }
    
    this.checkDisclaimer = function()
    {
        var chkDisclaimer = document.getElementById("chkDisclaimer");
        if (chkDisclaimer.checked == true)
        {
            internet_page.edButtons("btnAccept", false);
        }
        else
        {
            internet_page.edButtons("btnAccept", true);
        }
    }
    this.checkDisclaimer2 = function()
    {
        var chkDisclaimer = document.getElementById("chkDisclaimer2");
        if (chkDisclaimer.checked == true)
        {
            internet_page.edButtons("btnAccept2", false);
        }
        else
        {
            internet_page.edButtons("btnAccept2", true);
        }
    }
    
    this.acceptDisclaimer = function(willAccept)
    {
        if (willAccept == true)
        {
            internet_page.updateDisclaimer();
            internet_page._accept = true;
            internet_page.showControl("divDisclaimer", false);
            //internet_page.showControl("divTransaction", true);
            if (internet_page._isAgent == true)
            {
                internet_page.showControl("divTransaction", true);
            }
            else
            {
                internet_page.showControl("divStartPage", true);
            }
            if (internet_page._expiringOK == true)
            {
                internet_page.showControl("divReadMe", true);
            }
            internet_page.getRemitterName();
        }
        else
        {
            internet_page._accept = false;
            internet_page.showAlert("You must accept the terms and conditions before making an online transaction.");
        }
    }
    this.acceptDisclaimer2 = function(willAccept)
    {
        if (willAccept == true)
        {
            this.hidePanel("divDisclaimer2");
            internet_page._accept = true;
            internet_page.checkTransaction();
        }
        else
        {
            internet_page._accept = false;
            internet_page.showAlert("You must accept the terms and conditions before making an online transaction.");
        }
    }
    
    this.updateDisclaimer = function()
    {
        IRSWorld.Internet.T__Remitter.updateDisclaimer(internet_page.getValue("lblRemitterID", false), true);
    }
    
    this.showBeneficiaryInformation = function(isshow)
    {
        var matchAgainst = "";
        var matchTo = "";
        var boolBenef = false;
        var productcode = "";
        if (document.getElementById("ddlBeneficiary").selectedIndex > 0)
        {
            var PNBBranch = benef_param[document.getElementById("ddlBeneficiary").selectedIndex - 1].split(","); //internet_page.getDDLValue("ddlBeneficiary", false).split(",");
            var beneficiaryID = internet_page.getValue("lblBeneficiaryInfo", false).split("|");
            boolBenef = true;
            internet_page.getBeneficiaryAddress(beneficiaryID[0]);
            internet_page.getBeneficiaryContacts(beneficiaryID[0]);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, beneficiaryID[0], "BenCIFID", "BeneficiaryName"), "spnBeneficiaryName", false);
            internet_page.enterValue(PNBBranch[3], "spnPNBBranch", false);
            matchAgainst = (PNBBranch[3] != "" && PNBBranch[3] != null && PNBBranch[3] != undefined && internet_page.trim(PNBBranch[3]) != "null" && internet_page.trim(PNBBranch[3]) != "") ? beneficiaryID[0] + "|" + beneficiaryID[1] + "|" + PNBBranch[3] + "|" + beneficiaryID[3] : beneficiaryID[0] + "|" + beneficiaryID[1] + "|" + beneficiaryID[3];
            matchTo = (PNBBranch[3] != "" && PNBBranch[3] != null && PNBBranch[3] != undefined && internet_page.trim(PNBBranch[3]) != "null" && internet_page.trim(PNBBranch[3]) != "") ? "BenCIFID|ProductCode|BranchName|BenAcctSeqID" : "BenCIFID|ProductCode|BenAcctSeqID";
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "ProductCodeDesc"), "spnProductCode", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "AccountNo"), "spnAccountNumber", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "AccountTypeDesc"), "spnAccountType", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "CurrencyDesc"), "spnProductCurrency", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "Currency"), "lblTargetCurrency", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "BranchName"), "spnPNBBranch", false);
            internet_page.enterValue(internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "DomBankName"), "spnOtherBank", false);
            internet_page._netToBenefCurrency = internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "CurrencyDesc");
            internet_page.enterValue(internet_page._netToBenefCurrency, "spnNetToBeneAmount", true);
            productcode = internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "ProductCode");
            internet_page.getOnlineProduct(productcode);
        }
        
        if (isshow == true)
        {
            if (boolBenef == true)
            {
                internet_page.showPanel("divBeneficiary");
            }
            else
            {
                internet_page.showAlert("Please select a beneficiary");
            }
        }
    }
    
    this.clearText = function()
    {
        internet_page.clearDDL("ddlDCCard");
        document.getElementById("ddlBeneficiary").selectedIndex = -1;
        document.getElementById("ddlPurpose").selectedIndex = -1;
        //document.getElementById("ddlPaymentMethod").selectedIndex = -1;
        document.getElementById("ddlDCCard").selectedIndex = -1;
        document.getElementById("ddlChargeType").selectedIndex = -1;
        document.getElementById("ddlBenefName").selectedIndex = -1;
        //document.getElementById("ddlComputation").selectedIndex = -1;
        document.getElementById("ddlRemittingCurrency").selectedIndex = -1;
        document.getElementById("txtExchangeRate").value = "0.00";
        document.getElementById("txtAmount").value = "0.00";
        internet_page.removeValue("txtSecurityCode", false);
        document.getElementById("txtRemitterAcctNo").value = "";
        internet_page.clearDDL("ddlBeneficiary");
        
        
        internet_page.removeValue("txtRemAccountNo", false);
        internet_page.removeValue("txtRemittersBank", false);
        internet_page.removeValue("txtRoutingNumber", false);
        internet_page.removeValue("txtSwift", false);
        internet_page.removeValue("spnSourceCurrency", true);
        internet_page.removeValue("spnRemittingCurrency", true);
        internet_page.removeValue("spnNetToBeneAmount", true);
        internet_page._isDC = "0";
        internet_page.showControl("divCharges",false);
        internet_page.edButtons("txtAmount",false);
        internet_page.removeValue("spnHitMessage", true);
        internet_page.removeValue("spnHitMessage2", true);
    }
    
    this.createTransaction = function(mode)
    {
    
       // var rs = IRSWorld.Internet.T__Remitter.getBeneficiary(internet_page._remitterid);
        //if (rs.value != null && rs.value.Rows.length > 0) 
        //{
            internet_page.getBenefNames(internet_page._remitterid);
            internet_page.getBeneficiary(internet_page._remitterid);
           
            internet_page._beneficiary = true;
            var btndiv = parent.top.document.getElementById('div_frame_closer');
            if (btndiv)
            {
                close_frame_dialog();
            }
            if (!internet_page._accept)
            {
                internet_page.getDisclaimer();
                return;
            }
        //}
    
        if (internet_page._beneficiary == true)
        {
            if (internet_page._accept == true)
            {
                switch (mode)
                {
                    case "create":
                        internet_page.clearText();
                        if (internet_page._isAgent == true)
                        {
                            if (internet_page._isAgentProcessor == "" && internet_page._isAgentApprover == "" && _agentApproverProcessor == "")
                            {
                                internet_page.getUserRole();
                            }
                        }
                        else
                        {
                            internet_page.getUserRole();
                        }
                        internet_page._showHistory = false;
                        internet_page.showControl("divTransactionHistory", false);
                        internet_page.showControl("divTransaction", true);
                        internet_page.showControl("divTransactionInfo", false);
                        internet_page.showControl("divStartPage",false);
                        internet_page.showControl("divCharges",false);
                        if (internet_page._expiringOK == true)
                        {
                            internet_page.showControl("divReadMe", true);
                        }
                        break;
                    case "history":
                        internet_page._showHistory = true;
                        internet_page.process_grid_viewer("ajx_loading");
                        internet_page.showControl("divTransactionHistory", true);
                        internet_page.showControl("divTransaction", false);
                        internet_page.showControl("divTransactionInfo", false);
                        internet_page.showControl("divReadMe", false);
                        internet_page.showControl("divStartPage",false);
                        internet_page.showControl("divCharges",false);
                        break;
                    case "transaction":
                        internet_page._showHistory = false;
                        internet_page.showControl("divTransactionHistory", false);
                        internet_page.showControl("divTransaction", false);
                        internet_page.showControl("divTransactionInfo", true);
                        internet_page.showControl("divReadMe", false);
                        internet_page.showControl("divStartPage",false);
                        internet_page.showControl("divCharges",false);
                        break;
                }
            }
            else
            {
                internet_page.showAlert("Please accept the terms and conditions before you proceed.");
            }
        }
        else
        {
            //internet_page.showAlert("There is no beneficiary maintained for this remitter. Please go to the nearest PNB branch.");
            internet_page.showAlert("You don’t have a beneficiary enrolled. Kindly go to Add Beneficiary module.");
        }
    }
    
    this.checkTransaction = function()
    {
        internet_page.ajaxTimeout();
        internet_page.edButtons("btnSubmit", true);
        internet_page.edButtons("btnConfirmRem", true);
        internet_page._isSubmit = true;
        
        var paymentMethod = internet_page.getDDLValue("ddlPaymentMethod", false);
        if(paymentMethod == "ACH" || paymentMethod == "US Savings/ Checking Account")
        {
            internet_page.showPanel("divACHWorks", true);
        }
        else if (internet_page._isoverride == false)
        {
            internet_page.getCharges(false);
        }
        internet_page._checktransaction = 1;
    }
    
    this.checkTransaction_final = function()
    {
        var beneficiary = benef_param[document.getElementById("ddlBeneficiary").selectedIndex - 1].split(","); //internet_page.getDDLValue("ddlBeneficiary", false).split(",");
        var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
        internet_page._accountno = (beneficiary_value[1] == "22") ? IRSWorld.Internet.T__Remitter.getCIFAccount(beneficiary_value[3]).value : beneficiary[3];

        var isOBPS = internet_page.getOBPSAccount(internet_page._accountno);
        if (isOBPS == false)
        {
            internet_page.generateORNo();
            //if (confirm("Transaction will be submitted for processing. Are you sure you want to commit?"))
            //{
                internet_page.edButtons("btnConfirmRem", true);
                if (internet_page._isAgent == true)
                {
                    internet_page.showTransactionLoading("Transaction is being processed. Please wait...", false);
                }
                internet_page.getGFCAggregate();
            //}
        }
        else
        {
            internet_page.showAlert("Merchant accounts are not allowed. Please use the bills payment module.");
        }
    }
    
    this.connect_achworks = function()
    {
        
        var rem_accountno =  internet_page.getValue("txtRemAccountNo",false);
        var swift =  internet_page.getValue("txtSwift",false);
        var rem_bank_name = internet_page.getValue("txtRemittersBank",false);
        var routing_no = internet_page.getValue("txtRoutingNumber",false);
        var remitterid = internet_page.getValue("lblRemitterID", false); 
        var amount = internet_page.getValue("txtAmount", false);
        this.edButtons("btnSubmitACH",true);
        this.edButtons("btnCancel",true);
        internet_page.showTransactionLoading("System is checking the account details. Please wait...", false);
        IRSWorld.Internet.T__Remitter.connect_achworks(remitterid,rem_accountno, routing_no, rem_bank_name, swift,amount,this.connect_achworks_callback);
        
    }
    
    
    this.connect_achworks_callback = function(result)
    {
    
        if(result.value == null || result.value == "undefined" || result.value == "false" || result.value == false)
        {
            alert('We are unable to process your transaction. Please contact your branch of accounts');
            internet_page._checktransaction = 0;
            internet_page.removeValue("txtRemAccountNo", false);
            internet_page.removeValue("txtRemittersBank", false);
            internet_page.removeValue("txtRoutingNumber", false);
            internet_page.removeValue("txtSwift", false);

            if(confirm('Do you want to input other account details?'))
            {
                document.getElementById("btnSubmitACH").disabled = "";
                document.getElementById("btnCancel").disabled = "";

                internet_page.showTransactionLoading("", true);
                internet_page.hidePanel("divACHWorks");
                internet_page.showPanel("divACHWorks");
            }
            else
            {
                document.getElementById("btnSubmitACH").disabled = "";
                document.getElementById("btnCancel").disabled = "";
                internet_page.hidePanel("divACHWorks");
                internet_page.showTransactionLoading("", true);
            }
            
        }
        else
        {
            if(confirm('Are you sure you want to send this remittance?'))
            {
                internet_page.getCharges(false);
                internet_page.showTransactionLoading("", true);
            }
        }
        
         document.getElementById("btnSubmitACH").disabled = "";
        document.getElementById("btnCancel").disabled = "";
        internet_page.hidePanel("divACHWorks");
        internet_page.showTransactionLoading("", true);
        

        
    }
    
    
    
    
    this.getOBPSAccount = function(accountno)
    {
        return IRSWorld.Internet.T__Remitter.getOBPSAccount(accountno).value;
    }
    
    this.submitCancel = function()
    {
        internet_page.hidePanel("divSecurity");
        internet_page.showAlert("Request cancelled by user.");
    }

    this.submitTransaction = function()
    {
        internet_page.getCharges(false);
        internet_page._submittransaction = 1;
    }
    
    this.submitTransaction_final = function()
    {
        if (internet_page.validateTransaction(true, true) == true)
        {
            if (internet_page.getValue("txtSecurityCode", false) != "" && internet_page.isNumeric(internet_page.getValue("txtSecurityCode", false)))
            {
                if (internet_page.getValue("txtSecurityCode", false).length == 3)
                {
                    internet_page.showTransactionLoading("", true);
                    internet_page._securitycode = internet_page.getValue("txtSecurityCode", false);
                    internet_page.hidePanel("divSecurity");
                    if (confirm("Transaction will now connect to payment gateway for processing. Are you sure you want to commit?"))
                    {
                        if (internet_page._isDC == "1")
                        {
                            internet_page.showPanel("divSecurity");
                            internet_page.showFocus("txtSecurityCode");
                            internet_page.showPGLoading("Connecting to Payment Gateway... Please wait", false);
                            internet_page.edButtons("btnSubmitTransaction", true);
                            internet_page.edButtons("btnCancelTransaction", true);
                            //put validation of payment gateway if exist.
                            internet_page.submitPaymentGateway();
                            internet_page.removeValue("txtSecurityCode", false);
                        }
                        else if (internet_page._isDC == "3")
                        {
                            internet_page.showTransactionLoading("Connecting to Payment Gateway... Please wait", false);
                            internet_page.edButtons("btnSubmitTransaction", true);
                            internet_page.edButtons("btnCancelTransaction", true);
                            //put validation of payment gateway if exist.
                            internet_page.submitPaymentGateway();
                            internet_page.removeValue("txtSecurityCode", false);
                        }
                        else if (internet_page._isDC == "4")
                        {
                            internet_page.showTransactionLoading("Connecting to Payment Gateway... Please wait", false);
                            internet_page.edButtons("btnSubmitTransaction", true);
                            internet_page.edButtons("btnCancelTransaction", true);
                            //put validation of payment gateway if exist.
                            internet_page.submitPaymentGateway();
                            internet_page.removeValue("txtSecurityCode", false);
                        }
                    }
                    else
                    {
                        internet_page.edButtons("btnConfirmRem", false);
                    }
                }
                else
                {
                    internet_page.showAlert("Security code should have 3 numeric values. Please enter your security code (see sample beside) before pressing the continue button.");
                    internet_page.showFocus("txtSecurityCode");
                }
            }
            else
            {
                internet_page.showAlert("Security code cannot be left blank. Please enter your security code (see sample beside) before pressing the continue button.");
                internet_page.showFocus("txtSecurityCode");
            }
        }
        else
        {
            internet_page.showTransactionLoading("", true);
        }   
    }
    
    //7/18/2012-----------------------------------------------
        
    this.getTotalRemittance = function()
    {
         var totalamountremitted = IRSWorld.Internet.T__Remitter.getTotalRemittance().value;
         document.getElementById('txtTotalRemitted').value = totalamountremitted;
    }
    
    this.submitClearingDays = function()
    {
        var data = "";
         
        data += internet_page._branchCode + "|";
        data += internet_page._productcode + "|";
        data += internet_page._implementationtype + "|";
        data += internet_page._sourcecurrency + "|";
        data += internet_page._chargetype + "|";
        data += internet_page._pay_mode;
        
        var trxnseq = internet_page._txnseqid;
        
        
        /* disable saving clearing days
        var result = IRSWorld.Internet.T__Remitter.submitClearingDays(trxnseq, data).value;
        
        if(result == "1")
        {
            //internet_page.getReferenceNo();  -- already called
        }
        else
        {
            internet_page.showAlert("Error encountered while saving other transaction details. " + result + ". \r\n\r\n Please report to the nearest PNB branch.");
        }
        */
    }
    
    //----------------------------------------------------------
        
        
        
    // **************************************************************************************** //
    // *** Panel
    // **************************************************************************************** //
    this.showPanel = function(divName)
    {
        var div = document.getElementById(divName);
        
        divmask = new maskpanel();
        divmask.show();
        internet_page.showControl(divName, true);
        
        div.style.top = (((document.documentElement.clientHeight / 2) + document.documentElement.scrollTop) - (div.offsetHeight / 2)) + "px";
        div.style.left = (((document.documentElement.clientWidth / 2) + document.documentElement.scrollLeft) - (div.offsetWidth / 2)) + "px";
    }

    this.hidePanel = function(divName)
    {        
        divmask = new maskpanel();
        if (divName == "divDisclaimerAlert")
        {
            var response = IRSWorld.Internet.T__Remitter.getEnetsResponse().value;            
            internet_page.showControl(divName, false);
            divmask.hide();
            if (internet_page._isDC == "3")
            {
                if (response != null && response != "" && response != undefined && response != "|||" && response != "||||")
                {
                    window.clearInterval(internet_page._timeout);
                    internetClose("enets");
                }
                else
                {
                    alert('Transaction Failed. Please try again later.');
                    internet_page.createTransaction('create');
                }
            }
            else if (internet_page._isDC == "4")
            {
                response = IRSWorld.Internet.T__Remitter.getEXACTResponse().value;
                if (response != null && response != "" && response != undefined && response != "|||" && response != "||||")
                {
                    window.clearInterval(internet_page._timeout);
                    internetClose("eXact");
                }
                else
                {
                    alert('Transaction Failed. Please try again later.' + response);
                    internet_page.createTransaction('create');
                }
            }
        }
        else
        {
            internet_page.showControl(divName, false);
            divmask.hide();
        }
    }


    this.divobj;
    this.waitifrm;

    var maskpanel = function(source)
    {

        this.show = function()
        {
            if(!document.getElementById("xdivmasking"))
            {
                var divEle = document.createElement("div");
                divEle.setAttribute("id", "xdivmasking");
                document.body.appendChild(divEle);

                var divSty = document.getElementById("xdivmasking").style;
                divSty.position = "absolute"; 
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "46";
                divSty.opacity = ".50";
                divSty.backgroundColor="#000";
                divSty.filter = "alpha(opacity=50)";

                var divFram = document.createElement("iframe");
                divFram.setAttribute("id", "xmaskframe");
                document.body.appendChild(divFram);
                divSty = document.getElementById("xmaskframe").style;
                divSty.position = "absolute";
                divSty.top = "0px";
                divSty.left = "0px";
                divSty.zIndex = "45";
                divSty.border = "none";
                divSty.filter = "alpha(opacity=0)";
          }

          this.divobj = document.getElementById("xdivmasking");
          this.waitifrm = document.getElementById("xmaskframe");

          var dsh = document.documentElement.scrollHeight;
          var dch = document.documentElement.clientHeight;
          var dsw = document.documentElement.scrollWidth;
          var dcw = document.documentElement.clientWidth;
          var bdh = (dsh>dch) ? dsh:dch;
          var bdw = (dsw>dcw) ? dsw:dcw;

          this.waitifrm.style.height = this.divobj.style.height = bdh + "px";
          this.waitifrm.style.width = this.divobj.style.width = bdw + "px";
          this.waitifrm.style.display = this.divobj.style.display = "";
       }
       
       this.hide = function()
       {
          document.getElementById("xmaskframe").style.display = document.getElementById("xdivmasking").style.display = "none";
       }
    }
    
    // **************************************************************************************** //
    // *** LINKS
    // **************************************************************************************** //
    this.showLinks = function(name)
    {
        switch (name)
        {
            case "Password":
                show_frame_dialog("../Admin/Password.aspx?function=change&userid=" + internet_page._loginID, "passwordClose", "100%", "100%", "0", '0','250',false);
                break;
                
            case "OAAM":
                show_frame_dialog("../Admin/RegisterImagePhrase.aspx?main=false", "OAAMClose", "100%", "100%", "0",'0','250',false);
                break;
                
            case "Logout":
                //spawn_errpage('Thank you for using PNB online remittance system.');
                window.open('../ErrorPage.aspx?ErrorMessage=1&Inet=1', '_top');
                break;
        }
    }
    
    // **************************************************************************************** //
    // *** TOOLTIP BALLOON
    // **************************************************************************************** //
    this.showBalloon = function(name)
    {
        internet_page.ajaxTimeout();
        return internet_page.getLookupValue("Tooltip", "FldCode", name, "FldDescription");
    }
    
    this.pingSession_callback = function(param)
    {
        if(param.value == false)
        {
            alert("Session has expired due to inactivity.");
        }
    }
    
    this.sendReceiptEmail = function()
    {
        if (confirm("Do you want to receive your transaction receipt thru email?"))
        {
            //internet_page.showTransactionLoading("Sending transaction receipt thru email. Please wait...", false);
            internet_page.getOREmail(internet_page._refno, internet_page._txnseqid);
        }
    }
    
    this.getFilteredChargeType = function()
    {
        var beneficiary_value = internet_page.getDDLValue("ddlBeneficiary", true).split("|");
        var response = IRSWorld.Internet.T__Remitter.FilteredChargeType(beneficiary_value[1]).value
        internet_page.clearDDL("ddlChargeType");
        internet_page.tblChargeType = response;
        internet_page.fillDDL(response, "ddlChargeType", "FldDescription", "FldValue", "lblChargeType", ", ");
    }
    
    this.addBeneURL = function()
    {
        return show_frame_dialog('../Internet/InternetAddBeneficiary.aspx?function=add_bene&remitter_id=' + internet_page._remitterid, 'internetRemitterClose', '100%', '100%', '0','0','250',true);
    }
    this.UploadPictures = function()
    {
        return show_frame_dialog('../Internet/InternetUploads.aspx', 'internetRemitterClose', '80%', '80%', '0','60','270',false);
    }
    
    this.viewRemInfoURL = function()
    {
        return show_frame_dialog('../Internet/InternetRemitter.aspx?remitter_id=' + internet_page._remitterid, 'internetRemitterClose', '100%', '100%', '0');
    }
    
    this.check_watchlist = function(cifid, isbene)
    {
        IRSWorld.Internet.T__Remitter.check_watchlist(cifid, isbene, internet_page.check_watchlist_callback);
    }
    
    this.check_watchlist_callback = function(param)
    {
        if(param.value != null && param.value != "")
        {
            if (internet_page.getValue("spnHitMessage",true) != "")
            {
                internet_page.enterValue(param.value,"spnHitMessage2",true);
            }
            else
            {
                internet_page.enterValue(param.value,"spnHitMessage",true);
            }
        }
    }
    
    this.message_tat_prompt = function()
    {
        var productcode = 0;
        
        if (document.getElementById("ddlBeneficiary").selectedIndex > 0)
        {
            var PNBBranch = internet_page.getDDLValue("ddlBeneficiary", false).split(",");
            var beneficiaryID = internet_page.getValue("lblBeneficiaryInfo", false).split("|");           
                        
                        
            matchAgainst = (PNBBranch[3] != "" && PNBBranch[3] != null && PNBBranch[3] != undefined && internet_page.trim(PNBBranch[3]) != "null" && internet_page.trim(PNBBranch[3]) != "") ? beneficiaryID[0] + "|" + beneficiaryID[1] + "|" + PNBBranch[3] + "|" + beneficiaryID[3] : beneficiaryID[0] + "|" + beneficiaryID[1] + "|" + beneficiaryID[3];
            matchTo = (PNBBranch[3] != "" && PNBBranch[3] != null && PNBBranch[3] != undefined && internet_page.trim(PNBBranch[3]) != "null" && internet_page.trim(PNBBranch[3]) != "") ? "BenCIFID|ProductCode|BranchName|BenAcctSeqID" : "BenCIFID|ProductCode|BenAcctSeqID";

            ProductCode = internet_page.tblBeneficiary.Rows[document.getElementById("ddlBeneficiary").selectedIndex - 1].ProductCode; //internet_page.findValue(internet_page.tblBeneficiary, matchAgainst, matchTo, "ProductCode");            
        }    
    
        if (parseInt(ProductCode) == 10) 
        {
            internet_page.showAlert("PLEASE READ. \n Credit to PNB Account Type Selected. \n 1. Money is debit from your US bank account the next banking after initiating the Remittance. \n 2. For Amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila immediately deposits the funds to the \n recipient's PNB Bank Account.")
        }
        else if (parseInt(ProductCode) == 12 || parseInt(ProductCode) == 13)
        {
            internet_page.showAlert("PLEASE READ. \n Credit to Other Bank (Non-PNB Bank) Delivery Type Selected \n. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. \n 2. For amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila immediately deposits the funds to the \n recipient's Non-PNB Bank Account. \n 4. Crediting the funds to your beneficiaries Non-PNB Bank Account will depend upon \n the receiving bank. Some Non-PNB Banks will also charge their own transfer fees and \n will deduct the fee from the funds you sent to your beneficiary. \n NOTE: Philippine national or local holidays occuring during the remittance process will lengthen the remittance time.")
        }
        else if (parseInt(ProductCode) == 27 || parseInt(ProductCode) == 7 || parseInt(ProductCode) == 24)
        {
            internet_page.showAlert("PLEASE READ. \n Cash Pick Up Type Selected. \n. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. \n 2. For amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila immediately available for your beneficiary to pick up the remittance at any PNB Branches")
        }  
         else if (parseInt(ProductCode) == 16 || parseInt(ProductCode) == 9 || parseInt(ProductCode) == 8)
        {
            internet_page.showAlert("PLEASE READ. \n Door to Door Delivery Type Selected. \n. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. \n 2. For amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila provides the funds and instructions on the \n same day to its courier services for delivery to your Beneficiaries doorstep \n (delivery time varies according to locations). \n NOTE: Philippine national or local holiday's occuring during the remittance process will lengthen the remittance time.")
        }  
        else
        {
            internet_page.showAlert("Please enter product code to continue the transaction.")
        }
    }
    
    //add here
    /*
    this.message = function(productcode)
    {
        if (parseInt(productcode.value) == 12 && parseInt(productcode.value) == 13) 
        {
            internet_page.showAlert("PLEASE READ. \n Credit to PNB Account Type Selected. \n 1. Money is debit from your US bank account the next banking after initiating the Remittance. \n 2. For Amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila immediately deposits the funds to the \n recipient's PNB Bank Account.")
        }   
        else if (parseInt(productcode.value) == 7 && parseInt(productcode.value) == 4)
        {
            internet_page.showAlert("PLEASE READ. \n Credit to Other Bank (Non-PNB Bank) Delivery Type Selected \n. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. \n 2. For amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila immediately deposits the funds to the \n recipient's Non-PNB Bank Account. \n 4. Crediting the funds to your beneficiaries Non-PNB Bank Account will depend upon \n the receiving bank. Some Non-PNB Banks will also charge their own transfer fees and \n will deduct the fee from the funds you sent to your beneficiary. \n NOTE: Philippine national or local holidays occuring during the remittance process will lengthen the remittance time.")
        )
        else if (parseInt(productcode.value) == 27 && parseInt(productcode.value) == 7)
        {
            internet_page.showAlert("PLEASE READ. \n Cash Pick up \n. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. \n 2. For amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila immediately available for your beneficiary to pick up the remittance at any PNB Branches")
        }  
        else if (parseInt(productcode.value) == 16 && parseInt(productcode.value) == 8)
        {
            internet_page.showAlert("PLEASE READ. \n Cash Pick up \n. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. \n 2. For amount between US$50-1,000, the transaction will be processed immediately. \n Howeverfor amount more than US$1,000 but not exceeding US$2,500, the \n remittance transaction is held for two (2) US banking days to allow funds to clear and \n is then remitted to PNB Manila for processing. \n 3. Upon receiving the money, PNB Manila provides the funds and instructions on the \n same day to its courier services for delivery to your Beneficiaries doorstep \n (delivery time varies according to locations). \n NOTE: Philippine national or local holiday's occuring during the remittance process will lengthen the remittance time.")
        }  
        else
        {
            internet_page.shoeAlert("Please enter product code.")
        }
    }*/
}